using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_SellStoreList : MonoBehaviour
{

	public string ItemID;
    public string SellNum;      //出售的数量
    public string HideID;
	public GameObject Obj_ItemIcon;
	public GameObject Obj_ItemQuality;
	public GameObject Obj_ItemName;
	public GameObject Obj_ItemPrice;
    public GameObject Obj_ItemSellNum;
	private int ItmePrice;
	private GameObject obj_ItemTips;
    private string ItemName;
    public float showTimeSum;
    private bool shuHuiStatus;      //赎回状态
    
	// Use this for initialization
	void Start () {
        //this.gameObject.SetActive(false);
		//获取道具的Icon和品质信息
        if (HideID == "") {
            HideID = "0";
        }

		string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
		string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
		ItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
		//显示道具Icon
		object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
		Sprite itemIcon = obj as Sprite;
		Obj_ItemIcon.GetComponent<Image>().sprite = itemIcon;
		
		//显示道具品质
		string itemQuality = Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality);
		obj = Resources.Load(itemQuality, typeof(Sprite));
		Sprite itemQuility = obj as Sprite;
		Obj_ItemQuality.GetComponent<Image>().sprite = itemQuility;

		//显示道具名称和价格
		Obj_ItemName.GetComponent<Text>().text = ItemName;
        ItmePrice = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", ItemID, "Item_Template"));
        ItmePrice = int.Parse(SellNum) * ItmePrice;
		Obj_ItemPrice.GetComponent<Text>().text = ItmePrice.ToString();		

        //显示回购的数量
        Obj_ItemSellNum.GetComponent<Text>().text = SellNum;

	}
	
	// Update is called once per frame
	void Update () {
        showTimeSum = showTimeSum + Time.deltaTime;
        if (showTimeSum >= 0.5f) {
            this.gameObject.SetActive(true);
        }
	}

	//购买道具
	public void BuyItem(){

        //确保赎回只执行一次
        if (shuHuiStatus) {
            return;
        }
        shuHuiStatus = true;

		//扣除消耗的金币
		if (Game_PublicClassVar.Get_function_Rose.CostReward ("1", ItmePrice.ToString())) {

            //清空回购记录
            Game_PublicClassVar.Get_function_Rose.RemoveSellItemID(ItemID + "," + SellNum + "," + HideID);
            Game_PublicClassVar.Get_game_PositionVar.UpdataSellUIStatus = true;

			//发送道具至背包
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(ItemID, int.Parse(SellNum),"0",0,HideID,true,"9");
            //Debug.Log("商店购买道具");
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_383");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + ItemName + "X" + SellNum);
            //Game_PublicClassVar.Get_function_UI.GameHint("赎回" + ItemName + "X" + SellNum);

            this.transform.parent.parent.GetComponent<UI_NpcStoreShow>().UpdataMoneyValue = true;
		}
	}

	//鼠标按下 显示Tips
	public void Mouse_Down(){
		//调用方法显示UI的Tips
		if (obj_ItemTips == null) {
            obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(ItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet, true, HideID);
		}
	}
	//鼠标松开注销Tips
	public void Mouse_Up(){
		if (obj_ItemTips != null) {
			Destroy(obj_ItemTips);
		}
	}

}
