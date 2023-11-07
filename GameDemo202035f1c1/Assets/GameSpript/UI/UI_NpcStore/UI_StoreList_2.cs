using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_StoreList_2 : MonoBehaviour {

	public ObscuredString ItemID;           //道具ID
    public ObscuredString ItemNum;          //道具数量
    public ObscuredString BuyType;          //购买类型（0：表示金币 1：表示钻石  3：表示道具）
    public ObscuredString BuyUseItem;       //购买使用道具
    public ObscuredInt BuyUseItemNum;       //购买使用道具数量
    public ObscuredInt BuyNum;              //购买次数
    public ObscuredInt BuySpace;            //购买道具的位置,以后用来扣除购买次数的

	public GameObject Obj_ItemIcon;
	public GameObject Obj_ItemQuality;
	public GameObject Obj_ItemName;
	public GameObject Obj_ItemPrice;
    public GameObject Obj_ItemPriceIcon;
	private ObscuredInt ItmePrice;
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

        switch (BuyType) {

            //金币
            case "0":
                ItmePrice = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuyMoney", "ID", ItemID, "Item_Template"));
                ItmePrice = ItmePrice * int.Parse(ItemNum);
		        Obj_ItemPrice.GetComponent<Text>().text = ItmePrice.ToString();		
                break;

            //钻石
            case "1":
                //Debug.Log("ItemID = " + ItemID);
                ItmePrice = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuyRMB", "ID", ItemID, "Item_Template"));
                ItmePrice = ItmePrice * int.Parse(ItemNum);
		        Obj_ItemPrice.GetComponent<Text>().text = ItmePrice.ToString();		
                //显示钻石图标
                object priceObj = Resources.Load("ItemIcon/" + "10003", typeof(Sprite));
                Sprite priceItemIconObj = priceObj as Sprite;
                Obj_ItemPriceIcon.GetComponent<Image>().sprite = priceItemIconObj;
                break;

            //使用道具
            case "3":
                ItmePrice = BuyUseItemNum;
		        Obj_ItemPrice.GetComponent<Text>().text = ItmePrice.ToString();
		        //显示道具图标
                string priceItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", BuyUseItem, "Item_Template");
                priceObj = Resources.Load("ItemIcon/" + priceItemIcon, typeof(Sprite));
                priceItemIconObj = priceObj as Sprite;
                Obj_ItemPriceIcon.GetComponent<Image>().sprite = priceItemIconObj;
                break;
        }

        //为0直接注销
        if(ItmePrice==0)
        {
            Destroy(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//购买道具
	public void BuyItem(){

        //检测背包是否有1个位置的空位
        if (!Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1)) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_301");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            return;
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("背包已满,请及时清理背包！");
        }

        bool ifBuy = false;


        switch (BuyType)
        {
            //金币
            case "0":
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("购买价格111:" + ItmePrice);
                ItmePrice = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuyMoney", "ID", ItemID, "Item_Template"));
                ItmePrice = ItmePrice * int.Parse(ItemNum);
                ifBuy = Game_PublicClassVar.Get_function_Rose.CostReward("1", ItmePrice.ToString());
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("购买价格222:" + ItmePrice);
                break;

            //钻石
            case "1":
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("购买价格111:" + ItmePrice);
                ItmePrice = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuyRMB", "ID", ItemID, "Item_Template"));
                ItmePrice = ItmePrice * int.Parse(ItemNum);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("购买价格222:" + ItmePrice);
                ifBuy = Game_PublicClassVar.Get_function_Rose.CostReward("2", ItmePrice.ToString());
                break;

            //使用道具
            case "3":
                ifBuy = Game_PublicClassVar.Get_function_Rose.CostBagItem(BuyUseItem, BuyUseItemNum);
                break;
        }


		//扣除消耗的金币
        if (ifBuy)
        {
            //发送道具至背包
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(ItemID, int.Parse(ItemNum));
            if (this.transform.parent.parent.parent.parent.GetComponent<UI_NpcStoreShow_2>() != null)
            {
                this.transform.parent.parent.parent.parent.GetComponent<UI_NpcStoreShow_2>().UpdataMoneyValue = true;
            }
            
            //判定当前是否有购买次数的显示,有的话需要减少购买次数  0表示不限制购买次数
            if (BuyNum > 0) {
                string npcID = this.transform.parent.parent.parent.parent.GetComponent<UI_NpcStoreShow_2>().NpcID;
                Game_PublicClassVar.Get_function_UI.RandomStoreItemNum(npcID, BuySpace, 1);

                //更新界面显示
                if (this.transform.parent.parent.parent.parent.GetComponent<UI_NpcStoreShow_2>() != null)
                {
                    this.transform.parent.parent.parent.parent.GetComponent<UI_NpcStoreShow_2>().updateStore();
                }
            }

            //更新通用界面显示
            Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet();

            //服务器信息记录
            if (BuyType == "1") {
                //发送服务器记录消息
                //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000116, ItmePrice.ToString()+"钻石购买道具" + ItemName);
                Game_PublicClassVar.Get_function_Rose.ServerMsg_SendMsg(ItmePrice.ToString() + "钻石购买道具" + ItemName);
            }
        }
        else {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_334");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("购买失败!请检查一下自己的钱袋子,嘿嘿！");
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
