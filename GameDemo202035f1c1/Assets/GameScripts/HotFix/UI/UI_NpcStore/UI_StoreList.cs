using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_StoreList : MonoBehaviour {

	public string ItemID;
    public string ItemNum;
	public GameObject Obj_ItemIcon;
	public GameObject Obj_ItemQuality;
	public GameObject Obj_ItemName;
	public GameObject Obj_ItemPrice;
	private int ItmePrice;
	private GameObject obj_ItemTips;
    private string ItemName;
	// Use this for initialization
	void Start () {

		//获取道具的Icon和品质信息
        //Debug.Log("ItemID = " + ItemID + ";" +ItemID);
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
        if (ItemNum != "1")
        {
            Obj_ItemName.GetComponent<Text>().text = ItemName+"(" + ItemNum + "个)";
        }
        else {
            Obj_ItemName.GetComponent<Text>().text = ItemName;
        }
		
        ItmePrice = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuyMoney", "ID", ItemID, "Item_Template"));
        ItmePrice = ItmePrice * int.Parse(ItemNum);
		Obj_ItemPrice.GetComponent<Text>().text = ItmePrice.ToString();		//暂时

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//购买道具
	public void BuyItem(){

        //检测背包是否有1个位置的空位
        if (!Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1)) {
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("背包已满,请及时清理背包！");
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_288");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            return;
        }
        
		//扣除消耗的金币
        if (Game_PublicClassVar.Get_function_Rose.CostReward("1", ItmePrice.ToString()))
        {
            //发送道具至背包
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(ItemID, int.Parse(ItemNum),"0",0,"0",true,"6");
            this.transform.parent.parent.GetComponent<UI_NpcStoreShow>().UpdataMoneyValue = true;
            //Game_PublicClassVar.Get_function_UI.GameHint("购买"+ItemName+"X1");
        }
        else {
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("金币不足!");
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_138");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

        }

    }

	//鼠标按下 显示Tips
	public void Mouse_Down(){
		//调用方法显示UI的Tips
		if (obj_ItemTips == null) {
			obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(ItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");

            //获取目标是否是装备
            if (itemType == "3")
            {
                //obj_ItemTips.GetComponent<UI_EquipTips>().UIBagSpaceNum = BagPosition;
                obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "3";
            }
            else
            {
                //其余默认为道具,如果其他道具需做特殊处理
                //obj_ItemTips.GetComponent<UI_ItemTips>().UIBagSpaceNum = BagPosition;
                obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = "3";
            }
		}
	}
	//鼠标松开注销Tips
	public void Mouse_Up(){
		if (obj_ItemTips != null) {
			Destroy(obj_ItemTips);
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
		}
	}

}
