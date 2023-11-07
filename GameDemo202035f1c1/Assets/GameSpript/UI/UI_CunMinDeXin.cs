using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_CunMinDeXin : MonoBehaviour {

    public string CunMinDeXinID;
    private string ItemID;
    private int ItemNum;
    private string eventDes;
    private string currencyNum;
    private string currencyType;
    public string eventType;
    public float TimeShowStr;
    public GameObject Obj_ItemIcon;
    public GameObject Obj_ItemQuatily;
    public GameObject Obj_ItemNum;
    public GameObject Obj_EventDes;
    public GameObject Obj_CurrencyNum;
    public GameObject Obj_BtnText;
    public GameObject Obj_TimeShow;
    private GameObject obj_ItemTips;

    //出价
    public GameObject Obj_ChuJiaSet;
    public GameObject Obj_ChujiaTextBtn;

    /*
    public GameObject Obj_ChujiaBtn_1;
    public GameObject Obj_ChujiaBtn_2;
    public GameObject Obj_ChujiaBtn_3;
    private int chuJiaValue_1;
    private int chuJiaValue_2;
    private int chuJiaValue_3;
     */

	// Use this for initialization
	void Start () {
        
        ItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EventItemID", "ID", CunMinDeXinID, "SpecialEvent_Template");
        eventType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EventType", "ID", CunMinDeXinID, "SpecialEvent_Template");
        ItemNum =int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EventItemNum", "ID", CunMinDeXinID, "SpecialEvent_Template"));
        eventDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EventDes", "ID", CunMinDeXinID, "SpecialEvent_Template");
        currencyType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CurrencyType", "ID", CunMinDeXinID, "SpecialEvent_Template");
        currencyNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CurrencyNum", "ID", CunMinDeXinID, "SpecialEvent_Template");
        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
        string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");

        //显示道具Icon
        object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ItemIcon.GetComponent<Image>().sprite = itemIcon;

        //显示品质
        object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
        Sprite itemQuality = obj2 as Sprite;
        Obj_ItemQuatily.GetComponent<Image>().sprite = itemQuality;

        //售价
        switch (currencyType) { 
            case "1":
                Obj_CurrencyNum.GetComponent<Text>().text = "价格：" + currencyNum + "金币";
                break;
            case "2":
                Obj_CurrencyNum.GetComponent<Text>().text = "价格：" + currencyNum + "钻石";
                break;
            //显示购买价格选择
            case "3":

            break;
        }

        //事件描述
        Obj_EventDes.GetComponent<Text>().text = eventDes;

        //调整名字
        switch (eventType) { 
            case "1":
                Obj_BtnText.GetComponent<Text>().text = "购  买";
                Obj_ItemNum.GetComponent<Text>().text = "数量：" + ItemNum;
                Obj_CurrencyNum.SetActive(true);
                Obj_ChuJiaSet.SetActive(false);
            break;

            case "2":
                Obj_BtnText.GetComponent<Text>().text = "兑  换";
                //显示数量
                int selfNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                Obj_ItemNum.GetComponent<Text>().text = "数量：" + selfNum+"/"+ItemNum;
                if (selfNum < ItemNum) {
                    Obj_ItemNum.GetComponent<Text>().color = Color.red;
                    Obj_ItemNum.GetComponent<Text>().text = "数量：" + selfNum + "/" + ItemNum+"(兑换数量不足)";
                }
                Obj_CurrencyNum.SetActive(true);
                Obj_ChuJiaSet.SetActive(false);
            break;

            case "3":
                Obj_BtnText.GetComponent<Text>().text = "确认出价";
                Obj_ItemNum.GetComponent<Text>().text = "数量：" + ItemNum;
                Obj_CurrencyNum.SetActive(false);
                Obj_ChuJiaSet.SetActive(true);
            break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        TimeShowStr = TimeShowStr - Time.deltaTime;
        int timeInt = (int)(TimeShowStr);
        Obj_TimeShow.GetComponent<Text>().text = "剩余时间：" + timeInt + "秒";
        if (timeInt <= 0) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_335");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("倒计时结束！");
            Btn_CloseUI();
            hintBtn();  //隐藏按钮
        }
	}

    //
    public void Btn_Event() {

        switch (eventType)
        {
            //购买
            case"1":
                int roseGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                if (roseGold >= int.Parse(currencyNum)) { 
                    //扣除金币奖励道具
                    Game_PublicClassVar.Get_function_Rose.CostReward(currencyType, currencyNum);
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(ItemID,ItemNum, "0", 0, "0", true, "35");
                    hintBtn();
                    Destroy(this.gameObject);
                }
            break;

            //兑换
            case "2":
                int bagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                if (bagNum >= ItemNum) {
                    //扣除道具奖励金币
                    Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, ItemNum);
                    Game_PublicClassVar.Get_function_Rose.SendReward(currencyType, currencyNum,"35");
                    hintBtn();
                    Destroy(this.gameObject);
                }
            break;

            //显示购买价格选择
            case "3":
            //检测输出价格
            string jiageValueStr = Obj_ChujiaTextBtn.GetComponent<InputField>().text;

            //检测背包是否有输出价格的金币
            roseGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            if (int.Parse(jiageValueStr) > roseGold) {
                Game_PublicClassVar.Get_function_UI.GameHint("输入的金币超过自己携带的金币数量！");
                return;
            }
            //判断出价是否比价格高
            bool ifPayStatus = false;
            float jiaGePro = float.Parse(jiageValueStr) / float.Parse(currencyNum);

            //设置随机概率
            float payValurStr = 0;
            if (jiaGePro >= 0) {
                payValurStr = 0;
            }
            if (jiaGePro >= 0.6f)
            {
                payValurStr = 0.15f;
            }
            if (jiaGePro >= 1.0f) {
                payValurStr = 0.6f;
            }
            if (jiaGePro >= 1.2f)
            {
                payValurStr = 0.75f;
            }
            if (jiaGePro >= 1.5f) {
                payValurStr = 1.0f;
            }
            Debug.Log("payValurStr = " + payValurStr);
            if (Random.value <= payValurStr) {
                ifPayStatus = true;
            }

            if (ifPayStatus)
            {
                //扣除金币奖励道具
                Game_PublicClassVar.Get_function_Rose.CostReward(currencyType, currencyNum);
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(ItemID, ItemNum,"0",0.5f, "0", true, "35");
                Game_PublicClassVar.Get_function_UI.GameHint("果然豪爽,成交！");
            }
            else {
                Game_PublicClassVar.Get_function_UI.GameHint("你的出价太低了,卖给你我可要亏死!");
            }

            //重置状态
            hintBtn();
            Destroy(this.gameObject);
            break;
        }
    }


    //鼠标按下 显示Tips
    public void Mouse_Down()
    {
        //调用方法显示UI的Tips
        if (obj_ItemTips == null)
        {
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
    public void Mouse_Up()
    {
        if (obj_ItemTips != null)
        {
            Destroy(obj_ItemTips);
        }
    }


    public void Btn_CloseUI() {
        //Debug.Log("点击村民的信");
        Destroy(this.gameObject);
    }

    //隐藏按钮
    void hintBtn() {
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnCunMinDeXin.GetComponent<UI_CunMinDeXinBtn>().EventStatus = false ;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnCunMinDeXin.SetActive(false);
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SpecialEventOpenTime", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
    }
}
