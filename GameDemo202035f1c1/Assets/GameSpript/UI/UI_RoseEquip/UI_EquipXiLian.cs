using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_EquipXiLian : MonoBehaviour {


    public GameObject Obj_XiLianSet;
    public GameObject Obj_XiLianDaShi;
    public GameObject Obj_EquipPropertyMove;        //属性转移
    private bool XiLianStatus;

    public GameObject Obj_EquipItem;                //放入的装备图标
    public GameObject Obj_EquipQuality;             //放入的装备图标
    public GameObject Obj_EquipXiLianGold;          //金币洗练消耗金币
    public GameObject Obj_EquipXiLianItem;          //道具洗练消耗道具数量

    public GameObject Obj_EquipPropertyMoveCostMove;    //转移装备属性

    //public GameObject Obj_EquipXiLianShow_Rmb;  //钻石洗炼
    //public GameObject Obj_EquipXiLianShow_Item; //道具洗炼

    public GameObject Obj_EquipXiLianBtn_Gold;      //金币洗练
    public GameObject Obj_EquipXiLianBtn_Item;      //道具洗练

    public GameObject Obj_EquipXiLianBtn_Gold_Ten;      //金币洗练十次
    public GameObject Obj_EquipXiLianBtn_Item_Ten;      //道具洗练十次

    //public GameObject Obj_EquipXiLianDaShi;           //洗炼大师
    public GameObject Obj_ItemXiLianBtn;
    public GameObject Obj_RMBXiLianBtn;
    public GameObject Obj_XiLianDaShiBtn;
    public GameObject Obj_XiLianMoveBtn;

    public GameObject Obj_EquipBtnText_1;
    public GameObject Obj_EquipBtnText_2;
    public GameObject Obj_EquipBtnText_3;
    public GameObject Obj_EquipBtnText_4;

    public bool UpdateXiLianItemStatus;
    public ObscuredString bagSpaceNum;                  //背包格子
    public ObscuredString moveItemID;                   //移动道具ID
    public ObscuredString xiLianItemID;                 //洗练道具ID
    private ObscuredInt xiLianNeedRMB;                  //洗练金币
    private string[] xiLianNeedItem;                    //洗练需要道具
    private GameObject obj_ItemTips;                    //道具Tips

    public GameObject Obj_EquipXiLianSet;               //适配UI
    public GameObject Obj_XiLianHint;
    public GameObject Obj_XiLianHintText;


    //装备转移相关
    public ObscuredString Move_Space_1;
    public ObscuredString Move_Space_2;

    public GameObject Obj_EquipPropertyItem_1;            //放入的装备图标
    public GameObject Obj_EquipPropertyQuality_1;         //放入的装备图标
    public GameObject Obj_EquipPropertyItem_2;            //放入的装备图标
    public GameObject Obj_EquipPropertyQuality_2;         //放入的装备图标

    private ObscuredInt nowXiLianNum;

    private bool nowXiLianStatus;

    private ObscuredBool bXilianTen;
    // Use this for initialization
    void Start () {
        nowXiLianNum = 0;
        bXilianTen = false;

        //隐藏洗炼消耗
        Obj_EquipXiLianItem.SetActive(false);
        Obj_EquipXiLianGold.SetActive(false);

        //默认打开背包
        GameObject functionOpen = GameObject.FindWithTag("UI_FunctionOpen");

        //设置洗炼消耗钻石
        xiLianNeedRMB = 200;

        //Obj_EquipXiLianGold.GetComponent<Text>().text = "消耗钻石：0";
        //Obj_EquipXiLianItem.GetComponent<Text>().text = "消耗洗炼石：0";
        //Obj_EquipXiLianItem.GetComponent<Text>().text = "";
        Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus = true;      //打开装备洗练状态
        XiLian_Item();  //默认显示道具洗练
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_EquipXiLian = this.gameObject;

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_EquipXiLianSet);


        //初始化标签按钮
        if (Game_PublicClassVar.Get_wwwSet.GameSetLanguage._Language != "Chinese")
        {
            Obj_EquipBtnText_1.GetComponent<Text>().fontSize = 20;
            Obj_EquipBtnText_2.GetComponent<Text>().fontSize = 20;
            Obj_EquipBtnText_3.GetComponent<Text>().fontSize = 20;
            Obj_EquipBtnText_4.GetComponent<Text>().fontSize = 20;
        }

    }
	
	// Update is called once per frame
	void Update () {

        //触发移动注销此界面
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus != "1")
        {
            Destroy(this.gameObject);
            //关闭背包
            if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status == true)
            {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenBag();
            }
        }

        if (UpdateXiLianItemStatus)
        {
            UpdateXiLianItemStatus = false;
        }
	}

    //被销毁时调用
    void OnDisable()
    {
        Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus = false;         //关闭装备洗练状态
        Game_PublicClassVar.Get_game_PositionVar.EquipPropertyMoveStatus = false;   //关闭装备转移状态
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_EquipXiLian = null;
    }

    public void MouseEnter() {

        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "1") {
            /*
            bagSpaceNum = Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial;
            moveItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
            if (moveItemID != "")
            { 
                //判定ID是否为装备
                if (moveItemID[0] == '1') {
                    string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItemID, "Item_Template");
                    if (itemType == "3") {

                        //显示道具Icon
                        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", moveItemID, "Item_Template");
                        object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
                        Sprite itemIcon = obj as Sprite;
                        Obj_EquipItem.GetComponent<Image>().sprite = itemIcon;

                        //显示品质
                        string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", moveItemID, "Item_Template");
                        object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
                        Sprite itemQuality = obj2 as Sprite;
                        Obj_EquipQuality.GetComponent<Image>().sprite = itemQuality;

                        //显示洗练金币
                        xiLianNeedGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianMoney", "ID", moveItemID, "Item_Template"));
                        Obj_EquipXiLianGold.GetComponent<Text>().text = "消耗金币：" + xiLianNeedGold;

                        //显示洗练道具
                        xiLianNeedItem = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianStoneNeedNum", "ID", moveItemID, "Item_Template").Split(',');
                        int itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(xiLianNeedItem[0]);
                        Obj_EquipXiLianItem.GetComponent<Text>().text = "消耗洗炼石：" + itemBagNum + "/" + xiLianNeedItem[1];
                    }
                }
            }
            //防止背包移动
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
            */
            UpdateXiLianItem();
        }
    }

    public void UpdateXiLianItem() {

        moveItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
        if (moveItemID != "")
        {
            //判定ID是否为装备
            if (moveItemID.ToString()[0] == '1')
            {
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItemID, "Item_Template");
                if (itemType == "3")
                {

                    Obj_EquipItem.SetActive(true);
                    Obj_EquipQuality.SetActive(true);

                    //显示道具Icon
                    string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", moveItemID, "Item_Template");
                    object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
                    Sprite itemIcon = obj as Sprite;
                    Obj_EquipItem.GetComponent<Image>().sprite = itemIcon;

                    //显示品质
                    string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", moveItemID, "Item_Template");
                    object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
                    Sprite itemQuality = obj2 as Sprite;
                    Obj_EquipQuality.GetComponent<Image>().sprite = itemQuality;

                    //显示洗练金币
                    //string xiLianMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianMoney", "ID", moveItemID, "Item_Template");
                    //Debug.Log("xiLianMoney = " + xiLianMoney + ";moveItemID = " + moveItemID);

                    /*
                    //显示洗练钻石
                    xiLianNeedRMB = 500;
                    Obj_EquipXiLianGold.GetComponent<Text>().text = "消耗钻石：" + xiLianNeedRMB;
                    
                    //显示洗练消耗的洗练石道具
                    xiLianNeedItem = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianStoneNeedNum", "ID", moveItemID, "Item_Template").Split(',');
                    int itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(xiLianNeedItem[0]);
                    string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", xiLianNeedItem[0], "Item_Template"); ;
                    Obj_EquipXiLianItem.GetComponent<Text>().text = "消耗" + itemName + "：" + itemBagNum + "/" + xiLianNeedItem[1];
                    */

                    //根据装备品质调整价格
                    /*
                    xiLianNeedRMB = int.Parse(ItemQuality) * 50;
                    if (xiLianNeedRMB >= 200) {
                        xiLianNeedRMB = 200;
                    }
                    if (xiLianNeedRMB <= 0) {
                        xiLianNeedRMB = 1;
                    }
                    */

                    //显示洗炼钻石
                    Obj_EquipXiLianGold.GetComponent<XiLianEquipNeedItem>().ItemID = "3";
                    Obj_EquipXiLianGold.GetComponent<XiLianEquipNeedItem>().NeedItemNum = xiLianNeedRMB;
                    Obj_EquipXiLianGold.GetComponent<XiLianEquipNeedItem>().UpdateShow();

                    //显示洗炼道具
                    xiLianNeedItem = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianStoneNeedNum", "ID", moveItemID, "Item_Template").Split(',');
                    if (xiLianNeedItem.Length >= 2) {
                        int itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(xiLianNeedItem[0]);
                        Obj_EquipXiLianItem.GetComponent<XiLianEquipNeedItem>().ItemID = xiLianNeedItem[0];
                        Obj_EquipXiLianItem.GetComponent<XiLianEquipNeedItem>().NeedItemNum = int.Parse(xiLianNeedItem[1]);
                        Obj_EquipXiLianItem.GetComponent<XiLianEquipNeedItem>().UpdateShow();
                    }
                }
                else {
                    Obj_EquipItem.SetActive(false);
                    Obj_EquipQuality.SetActive(false);
                }
            }
        }
    }

    public void Btn_EquipXiLian_One()
    {
        this.Btn_EquipXiLian( false );
    }

    public void Btn_EquipXiLian_Ten()
    {
        this.Btn_EquipXiLian(true);
    }

    public void Btn_EquipXiLian( bool ten ) {

        //检测服务器网络
        if (Game_PublicClassVar.gameLinkServer.ServerLinkStatus == false) {

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_58");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1);
            return;

        }


        if (XiLianStatus) {

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_398");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1);
            //Game_PublicClassVar.Get_function_UI.GameHint("短时间内洗炼次数太多！请稍后再试");
            return;

        }

        //不显示Tips
        if (obj_ItemTips != null)
        {
            Destroy(obj_ItemTips);
        }
        ObscuredInt xilianNum = 10;
        ObscuredInt needrmb = xiLianNeedRMB;
        if (ten) {
            needrmb = xiLianNeedRMB * xilianNum;
        }
        //判定洗练金币是否足够
        if (Game_PublicClassVar.Get_function_Rose.GetRoseRMB() < needrmb) {
            //Game_PublicClassVar.Get_function_UI.GameHint("钻石不足");
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_399");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1);
            //XiLianStatus = false;
            return;
        }

        //检测洗炼提示
        if (Obj_XiLianHint != null) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前有洗炼提示未关闭,请稍后再试！");
            return;
        }

        //宠物装备不能洗炼
        xiLianItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
        string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", xiLianItemID, "Item_Template");
        if (itemType == "3")
        {
            string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItemID, "Item_Template");
            if (itemSubType == "" || itemSubType == null) {
                itemSubType = "0";
            }
            
            if (itemSubType == "201" || itemSubType == "202" || itemSubType == "203"|| itemSubType == "204") {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物装备不可以洗炼喔！");
                return;
            }
            if (int.Parse(itemSubType) >= 101 && int.Parse(itemSubType) <= 112)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("生肖装备不可以洗炼喔！");
                return;
            }
        }

        //获取当前装备是否有隐藏属性
        string hintSkillName = Game_PublicClassVar.Get_function_Rose.IfEquipHintSkill(bagSpaceNum);
        if (hintSkillName != "") {
            XiLianStatus = false;
            Obj_XiLianHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_17");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_18");
            if (ten)
            {
                Obj_XiLianHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint_1 + hintSkillName + langStrHint_2, null, EquipXiLian_ZuanShi_Ten, "洗炼提示", "取消", "确定", null);
            }
            else {
                Obj_XiLianHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint_1 + hintSkillName + langStrHint_2, null, EquipXiLianOld, "洗炼提示", "取消", "确定", null);
            }
            //Obj_XiLianHint.GetComponent<UI_CommonHint>().Btn_CommonHint("在此装备上发现了隐藏技能:"+ hintSkillName + "！\n提示:如果再次洗炼隐藏技能有可能消失！", Btn_EquipXiLian, null);
            Obj_XiLianHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            Obj_XiLianHint.transform.localPosition = Vector3.zero;
            Obj_XiLianHint.transform.localScale = new Vector3(1, 1, 1);
            return;
        }



        //金币洗炼40%概率成功
        /*
        if (Random.value > 0.4f) {
            
            Game_PublicClassVar.Get_function_UI.GameHint("装备洗练失败！");
            //扣除金币
            Game_PublicClassVar.Get_function_Rose.CostReward("2", xiLianNeedRMB.ToString());
            return;
        }
        */

        //执行洗炼
        EquipXiLian(ten);
        /*
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_338");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        */
        //Game_PublicClassVar.Get_function_UI.GameGirdHint("请放入将要洗练的装备");
        XiLianStatus = false;

        //更新显示
        UpdateXiLianItem();
    }

    private void EquipXiLianOld()
    {
        EquipXiLian( false );
    }

    private void EquipXiLian_ZuanShi_Ten()
    {
        this.EquipXiLian(true);
    }

    /**
     * ten[true:洗练十次  false:洗练一次]
     */
    public void EquipXiLian( bool ten =false ) {


        if (bagSpaceNum != "" && bagSpaceNum != "0")
        {

            if (xiLianNeedRMB < 100)
            {
                return;     //表示洗炼数据被篡改
            }

            xiLianItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
            string xilianHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagSpaceNum, "RoseBag");
            if (xiLianItemID != "")
            {
                //判定ID是否为装备
                if (xiLianItemID.ToString()[0] == '1')
                {
                    string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", xiLianItemID, "Item_Template");
                    if (itemType == "3")
                    {
                        if (xiLianItemID == moveItemID)
                        {
                            //十倍洗练
                            if (ten)
                            {
                                //扣除钻石
                                ObscuredInt xilianNum = 10;

                                Game_PublicClassVar.Get_function_Rose.CostReward("2", (xiLianNeedRMB * xilianNum).ToString());
                                addXiLianExp(2, true);  //增加洗炼经验

                                Game_PublicClassVar.Get_function_Rose.ReturnHidePropertyIDEx(xiLianItemID, xilianHideID, delegate (string hideID)
                                {
                                    OnXilianReturn(hideID, 2, true);
                                }, true);

                            }
                            else
                            {
                                //扣除钻石
                                Game_PublicClassVar.Get_function_Rose.CostReward("2", xiLianNeedRMB.ToString());

                                //获得洗练装备ID
                                string hideID = Game_PublicClassVar.Get_function_Rose.ReturnHidePropertyID(xiLianItemID, xilianHideID, true);
                                if (hideID == "0")
                                {
                                    //循环洗练10次,防止不出洗练属性
                                    for (int i = 0; i <= 20; i++)
                                    {
                                        hideID = Game_PublicClassVar.Get_function_Rose.ReturnHidePropertyID(xiLianItemID, xilianHideID, true);
                                        if (hideID != "0")
                                        {
                                            i = 21;     //跳出循环
                                            break;
                                        }
                                    }
                                    if (hideID == "0")
                                    {
                                        string langStrHint_A = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_408");
                                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_A);
                                        //Game_PublicClassVar.Get_function_UI.GameHint("好像并没有发生什么变化！");
                                        XiLianStatus = false;
                                        return;
                                    }
                                }
                                OnXilianReturn(hideID, 2,false);
                            }
                        }
                    }
                }
            }
        }
    }

    public void Btn_EquipXiLian_Item_Ten()
    {
        this.Btn_EquipXiLian_Item( true );
    }

    public void Btn_EquipXiLian_Item_One()
    {
        this.Btn_EquipXiLian_Item( false );
    }

    public void Btn_EquipXiLian_Item( bool ten )
    {

        //Debug.Log("洗练装备！！！");

        //检测服务器网络
        if (Game_PublicClassVar.gameLinkServer.ServerLinkStatus == false)
        {
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_58");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1);
            return;
        }

        if (XiLianStatus)
        {
            string langStrHint_A = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_403");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_A);
            //Game_PublicClassVar.Get_function_UI.GameHint("短时间内洗炼次数太多！请稍后再试");
            return;
        }

        XiLianStatus = true;


        if (moveItemID == "")
        {
            string langStrHint_A = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_404");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_A);
            //Game_PublicClassVar.Get_function_UI.GameHint("请点击左侧装备放入要洗炼的装备!");
            XiLianStatus = false;
            return;
        }

        //不显示Tips
        if (obj_ItemTips != null)
        {
            Destroy(obj_ItemTips);
        }

        if (xiLianNeedItem[0] == "" || xiLianNeedItem[0] == "0") {
            string langStrHint_A = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_405");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_A);
            //Game_PublicClassVar.Get_function_UI.GameHint("未放入需要洗炼的装备!");
            XiLianStatus = false;
            return;
        }

        //宠物装备不能洗炼
        xiLianItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
        string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", xiLianItemID, "Item_Template");
        if (itemType == "3")
        {
            string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItemID, "Item_Template");

            if (itemSubType == "" || itemSubType == null)
            {
                itemSubType = "0";
            }

            if (itemSubType == "201" || itemSubType == "202" || itemSubType == "203" || itemSubType == "204")
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物装备不可以洗炼喔！");
                return;
            }

            if (int.Parse(itemSubType) >= 101 && int.Parse(itemSubType) <= 112)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("生肖装备不可以洗炼喔！");
                return;
            }
        }

        ObscuredInt tenAddPro = 1;
        if (ten) {
            tenAddPro = 10;
        }

        //判定洗练道具是否足够
        int itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(xiLianNeedItem[0]);
        if (itemBagNum < int.Parse(xiLianNeedItem[1]) * tenAddPro)
        {
            string langStrHint_A = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_406");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_A);
            //Game_PublicClassVar.Get_function_UI.GameHint("洗炼石不足");
            XiLianStatus = false;
            return;
        }

        //检测洗炼提示
        if (Obj_XiLianHint != null)
        {
            Destroy(Obj_XiLianHint);
        }

        bXilianTen = ten;

        //检测洗炼提示
        if (Obj_XiLianHint == null)
        {
            //bool hintStatus = false;
            //获取当前装备是否有隐藏属性
            string hintSkillName = Game_PublicClassVar.Get_function_Rose.IfEquipHintSkill(bagSpaceNum);
            if (hintSkillName != "")
            {
                XiLianStatus = false;
                Obj_XiLianHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_17");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_18");
                //Obj_XiLianHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint_1 + hintSkillName + langStrHint_2, Btn_EquipXiLian_Item, null);
                if (ten)
                {
                    Obj_XiLianHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint_1 + hintSkillName + langStrHint_2, null, EquipXiLian_Item_Ten, "洗炼提示", "取消", "确定", null);
                }
                else {
                    Obj_XiLianHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint_1 + hintSkillName + langStrHint_2, null, EquipXiLian_Item_Old, "洗炼提示", "取消", "确定", null);
                }
                Obj_XiLianHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                Obj_XiLianHint.transform.localPosition = Vector3.zero;
                Obj_XiLianHint.transform.localScale = new Vector3(1, 1, 1);
                return;
            }
        }

        //执行洗炼
        EquipXiLian_Item(ten);

        /*
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_338");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        */
        //Game_PublicClassVar.Get_function_UI.GameGirdHint("请放入将要洗练的装备");
        XiLianStatus = false;
        return;
    }

    private void EquipXiLian_Item_Old()
    {
        this.EquipXiLian_Item(this.bXilianTen);
    }

    private void EquipXiLian_Item_Ten()
    {
        this.EquipXiLian_Item(true);
    }

    public void EquipXiLian_Item(bool ten) {

        if (bagSpaceNum != "" && bagSpaceNum != "0")
        {
            xiLianItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
            string xilianHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagSpaceNum, "RoseBag");
            if (xiLianItemID != "")
            {
                //判定ID是否为装备
                if (xiLianItemID.ToString()[0] == '1')
                {
                    string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", xiLianItemID, "Item_Template");
                    if (itemType == "3")
                    {
                        if (xiLianItemID == moveItemID)
                        {

                            //扣除道具
                            ObscuredInt tenAddPro = 1;
                            if (ten)
                            {
                                tenAddPro = 10;
                            }

                            Game_PublicClassVar.Get_function_Rose.CostBagItem(xiLianNeedItem[0], int.Parse(xiLianNeedItem[1]) * tenAddPro);
                            Game_PublicClassVar.Get_function_Rose.CostReward(xiLianNeedItem[0], xiLianNeedItem[1]);

                            if (ten)
                            {
                                addXiLianExp(1, true);  //增加洗炼经验

                                Game_PublicClassVar.Get_function_Rose.ReturnHidePropertyIDEx(xiLianItemID, xilianHideID, delegate (string hideID) {
                                    OnXilianReturn(hideID, 1, true);
                                }, true);
                            }
                            else
                            {
                                //获得洗练装备ID
                                string hideID = Game_PublicClassVar.Get_function_Rose.ReturnHidePropertyID(xiLianItemID, xilianHideID, true);
                                if (hideID == "0")
                                {
                                    //循环洗练10次,防止不出洗练属性
                                    for (int i = 0; i <= 20; i++)
                                    {
                                        hideID = Game_PublicClassVar.Get_function_Rose.ReturnHidePropertyID(xiLianItemID, xilianHideID, true);
                                        if (hideID != "0")
                                        {
                                            i = 21;     //跳出循环
                                            break;
                                        }
                                    }
                                    if (hideID == "0")
                                    {
                                        string langStrHint_A = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_408");
                                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_A);
                                        //Game_PublicClassVar.Get_function_UI.GameHint("好像并没有发生什么变化！");
                                        XiLianStatus = false;
                                        return;
                                    }
                                }
                                OnXilianReturn(hideID, 1,false);
                            }
                        }
                    }
                }
            }
        }

    }

    public void OnXilianReturn( string hideID, int addExpType, bool tenStatus)
    {

        //Debug.Log("洗炼开始....");

        string xilianHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagSpaceNum, "RoseBag");
       

        //写入极品属性ID
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", hideID, "ID", bagSpaceNum, "RoseBag");
        string langStrHint_B = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_401");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_B);
        //Game_PublicClassVar.Get_function_UI.GameHint("装备洗练成功！");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");

        //更新洗练道具数量
        int xiLianBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(xiLianNeedItem[0]);
        Obj_EquipXiLianItem.GetComponent<XiLianEquipNeedItem>().UpdateShow();

        //写入成就(洗炼)
        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("107", "0", "1");

        //额外孔位
        OpenGemSpace(bagSpaceNum);

        //增加洗炼经验
        if (tenStatus == false)
        {
            addXiLianExp(addExpType,false);
        }

        XiLianStatus = false;

        //每洗炼10次 收集一次数据
        nowXiLianNum = nowXiLianNum + 1;
        if (nowXiLianNum >= 10)
        {
            nowXiLianNum = 0;
            string[] saveList = new string[] { "", "2", "预留设备号位置", "4" };
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001006, saveList);
        }

        //更新提示
        UpdateXiLianHint(bagSpaceNum);

    }


    private void addXiLianExp(int addExpType, bool tenStatus) {

        //增加洗炼经验
        ObscuredInt tenPro = 1;

        if (tenStatus)
        {
            tenPro = 10;
        }

        int addExp = 0;
        if (addExpType == 1)
        {
            addExp = (int)(Random.value * float.Parse(xiLianNeedItem[1]) * tenPro);
            if (addExp <= 1)
            {
                addExp = 1;
            }
        }
        else
        {
            addExp = (int)(Random.value * float.Parse(xiLianNeedItem[1]) * tenPro);
        }



        switch (xiLianNeedItem[0])
        {
            case "10021015":
                Game_PublicClassVar.Get_function_Rose.AddXiLianDaShiExp("1", addExp);
                break;

            case "10022016":
                Game_PublicClassVar.Get_function_Rose.AddXiLianDaShiExp("2", addExp);
                break;

            case "10023016":
                Game_PublicClassVar.Get_function_Rose.AddXiLianDaShiExp("3", addExp);
                break;

            case "10024016":
                Game_PublicClassVar.Get_function_Rose.AddXiLianDaShiExp("4", addExp);
                break;

            case "10025016":
                Game_PublicClassVar.Get_function_Rose.AddXiLianDaShiExp("5", addExp);
                break;
        }


    }
    public void UpdateXiLianHint(string bagSpaceNum) {

        Obj_XiLianHintText.GetComponent<Text>().text = "";
        string returnStr = "";
        string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagSpaceNum, "RoseBag");
        if (hideID != "" && hideID != "0" && hideID != null)
        {
            string hideStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");

            if (hideStr != "" && hideStr != "0" && hideStr != null)
            {

                string[] hideList = hideStr.Split(';');
                for (int i = 0; i < hideList.Length; i++)
                {
                    string[] hideDataList = hideList[i].Split(',');
                    if (hideDataList.Length >= 2)
                    {
                        switch (hideDataList[0])
                        {
                            case "100":
                                returnStr = returnStr + "[幸运] ";

                                string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                                //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "恭喜玩家" + roseName + "洗炼装备时!意外在装备里发现了隐藏属性:" + "幸运" + "！");

                                Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
                                comStr_4.str_1 = "8";
                                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010191, comStr_4);

                                break;

                            case "10001":
                                string skillName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillName", "ID", hideDataList[1], "Skill_Template");
                                returnStr = returnStr + "[" + skillName + "] ";

                                //roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                                //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "恭喜玩家" + roseName + "洗炼装备时!意外在装备里发现了隐藏技能:" + skillName + "！");

                                comStr_4 = new Pro_ComStr_4();
                                comStr_4.str_1 = "1";
                                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010191, comStr_4);

                                break;
                        }
                    }
                }
            }

            if (returnStr != "" && returnStr != null)
            {
                Obj_XiLianHintText.GetComponent<Text>().text = "提示:本次洗炼意外发现隐藏技能 " + returnStr;
            }
        }
  

    }


    //显示装备ID
    public void Btn_Click() {

        if (obj_ItemTips == null)
        {
            if (moveItemID != "" && moveItemID != "0")
            {
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItemID, "Item_Template");
                obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(moveItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
                //获取目标是否是装备
                if (itemType == "3")
                {
                    obj_ItemTips.GetComponent<UI_EquipTips>().UIBagSpaceNum = bagSpaceNum;
                    obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "1";
                    //获取极品属性
                    string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagSpaceNum, "RoseBag");
                    obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;
                    //获取宝石属性
                    string itemGemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", bagSpaceNum, "RoseBag");
                    string itemGemHole = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", bagSpaceNum, "RoseBag");
                    obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemID = itemGemID;
                    obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemHole = itemGemHole;

                }
            }
        }
        else {
            Destroy(obj_ItemTips);
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
        }
    }

    //点击洗练金币
    public void XiLian_Gold() {

        Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus = true;      //打开装备洗练状态

        Obj_XiLianSet.SetActive(true);
        Obj_XiLianDaShi.SetActive(false);
        Obj_EquipPropertyMove.SetActive(false);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_ItemXiLianBtn.GetComponent<Image>().sprite = img;
        Obj_RMBXiLianBtn.GetComponent<Image>().sprite = img;
        Obj_XiLianDaShiBtn.GetComponent<Image>().sprite = img;
        Obj_XiLianMoveBtn.GetComponent<Image>().sprite = img;

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);

        Obj_EquipXiLianBtn_Gold.SetActive(true);
        Obj_EquipXiLianBtn_Gold_Ten.SetActive(true);
        Obj_EquipXiLianBtn_Item.SetActive(false);
        //Obj_EquipXiLianBtn_Item_Ten.SetActive(false);
        //Obj_EquipXiLianDaShi.SetActive(false);

        Obj_EquipXiLianGold.SetActive(true);
        Obj_EquipXiLianItem.SetActive(false);


        //显示底图
        obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
        img = obj as Sprite;
        Obj_RMBXiLianBtn.GetComponent<Image>().sprite = img;

    }

    //点击洗练道具
    public void XiLian_Item()
    {

        Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus = true;      //打开装备洗练状态

        Obj_XiLianSet.SetActive(true);
        Obj_XiLianDaShi.SetActive(false);
        Obj_EquipPropertyMove.SetActive(false);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_ItemXiLianBtn.GetComponent<Image>().sprite = img;
        Obj_RMBXiLianBtn.GetComponent<Image>().sprite = img;
        Obj_XiLianDaShiBtn.GetComponent<Image>().sprite = img;
        Obj_XiLianMoveBtn.GetComponent<Image>().sprite = img;

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);

        Obj_EquipXiLianBtn_Gold.SetActive(false);
        Obj_EquipXiLianBtn_Gold_Ten.SetActive(false);
        Obj_EquipXiLianBtn_Item.SetActive(true);
        //Obj_EquipXiLianBtn_Item_Ten.SetActive(true);
        //Obj_EquipXiLianDaShi.SetActive(false);
        Obj_EquipXiLianGold.SetActive(false);
        Obj_EquipXiLianItem.SetActive(true);



        //显示底图
        obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
        img = obj as Sprite;
        Obj_ItemXiLianBtn.GetComponent<Image>().sprite = img;
    }


    public void Btn_XiLianDaShi()
    {

        Obj_XiLianSet.SetActive(false);
        Obj_XiLianDaShi.SetActive(true);
        Obj_EquipPropertyMove.SetActive(false);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_ItemXiLianBtn.GetComponent<Image>().sprite = img;
        Obj_RMBXiLianBtn.GetComponent<Image>().sprite = img;
        Obj_XiLianDaShiBtn.GetComponent<Image>().sprite = img;
        Obj_XiLianMoveBtn.GetComponent<Image>().sprite = img;

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);
        Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);

        Obj_EquipXiLianBtn_Gold.SetActive(false);
        Obj_EquipXiLianBtn_Gold_Ten.SetActive(false);
        Obj_EquipXiLianBtn_Item.SetActive(true);
        //Obj_EquipXiLianBtn_Item_Ten.SetActive(true);
        //Obj_EquipXiLianDaShi.SetActive(false);
        Obj_EquipXiLianGold.SetActive(false);
        Obj_EquipXiLianItem.SetActive(true);


        //显示底图
        obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
        img = obj as Sprite;
        Obj_XiLianDaShiBtn.GetComponent<Image>().sprite = img;

        //默认显示英勇洗炼
        Obj_XiLianDaShi.GetComponent<UI_XiLianDaShiSet>().ShowXiLianDaShi("1");
    }

    //装备转移
    public void Btn_XiLianMove()
    {

        Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus = false;      //打开装备洗练状态
        Game_PublicClassVar.Get_game_PositionVar.EquipPropertyMoveStatus = true;   //打开装备转移状态

        Obj_XiLianSet.SetActive(false);
        Obj_XiLianDaShi.SetActive(false);
        Obj_EquipPropertyMove.SetActive(true);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_ItemXiLianBtn.GetComponent<Image>().sprite = img;
        Obj_RMBXiLianBtn.GetComponent<Image>().sprite = img;
        Obj_XiLianDaShiBtn.GetComponent<Image>().sprite = img;
        Obj_XiLianMoveBtn.GetComponent<Image>().sprite = img;

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

        Obj_EquipXiLianBtn_Gold.SetActive(false);
        Obj_EquipXiLianBtn_Gold_Ten.SetActive(false);
        Obj_EquipXiLianBtn_Item.SetActive(false);
        //Obj_EquipXiLianBtn_Item_Ten.SetActive(false);
        //Obj_EquipXiLianDaShi.SetActive(false);
        Obj_EquipXiLianGold.SetActive(false);
        Obj_EquipXiLianItem.SetActive(false);


        //显示底图
        obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
        img = obj as Sprite;
        Obj_XiLianMoveBtn.GetComponent<Image>().sprite = img;

    }


    //判定是否额外开启宝石孔位
    public void OpenGemSpace(string bagSpaceNum){

        //关闭额外开启宝石孔位功能
        return;
        /*
		//5%概率
		if(Random.value>0.05f){
			return;
		}

		//获取目标宝石孔位
		string gemHoleIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", bagSpaceNum, "RoseBag");
		string[] gemHoleIDList = gemHoleIDSet.Split (',');
		//宝石最大数量为4个
		if (gemHoleIDList.Length >= 4) {
			return;
		}

		string gemIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", bagSpaceNum, "RoseBag");

		string Write_GemHoleStr = gemHoleIDSet;
		string Write_GemIDSet = gemIDSet;
		string randHoleValue = "101";

		//随机一个颜色的宝石孔位
		randHoleValue = Game_PublicClassVar.Get_function_Rose.ReturnHoleColoer();

		//写入宝石值
		if (Write_GemHoleStr != "" && Write_GemHoleStr !="0") {
			Write_GemHoleStr = Write_GemHoleStr + "," + randHoleValue;
			Write_GemIDSet = Write_GemIDSet + "," + "0";
		}else{
			Write_GemHoleStr = randHoleValue;
			Write_GemIDSet = "0";
		}

		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole",Write_GemHoleStr, "ID", bagSpaceNum, "RoseBag");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", Write_GemIDSet,"ID", bagSpaceNum, "RoseBag");
		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");

        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_302");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("运气爆炸!洗炼装备时竟然额外钻探出一个宝石孔位");
        */
	}


    //装备转移
    public void BtnEquipPropertyMove() {


        if (Move_Space_1 == "" || Move_Space_2 == "") {

            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_343_1");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            //Debug.Log("请放入需要转移的装备");
            return;
        }

        if (Move_Space_1 == Move_Space_2) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("转移的装备不能是同一件,请再次确认需要转移的装备!");
            return;
        }


        //判断条件是否满足（只有紫色以上的装备菜可以继承属性）
        string item_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Move_Space_1, "RoseBag");
        string item_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Move_Space_2, "RoseBag");

        //宠物装备和生肖装备无法转移
        if (Game_PublicClassVar.Get_function_Rose.ReturnEquipSubType(item_1) != "1" || Game_PublicClassVar.Get_function_Rose.ReturnEquipSubType(item_1) != "1") {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("生肖装备和宠物装备无法进行转移!");
            return;
        }


        //判断品质
        string quality_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", item_1, "Item_Template");
        string quality_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", item_2, "Item_Template");
        if (quality_1 == "" || quality_2 == "") {
            return;
        }

        if (int.Parse(quality_1) < 4 || int.Parse(quality_2) < 4) {

            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_344_1");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            //Debug.Log("需要转移的装备品质必须为紫色品质以上");
            return;
        }

        //需要相同部位
        string itemSubType_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", item_1, "Item_Template");
        string itemSubType_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", item_2, "Item_Template");

        if (itemSubType_1 != itemSubType_2) {

            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_345_1");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            return;
            //Debug.Log("部位不同,不能进行属性转移");
        }

        //装备转移
        string hide_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Move_Space_1, "RoseBag");
        string hide_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Move_Space_2, "RoseBag");

        if (hide_1 == "" || hide_1 == "0" && hide_1 == null) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未发现可转移的隐藏属性");
            return;
        }

        //转移装备需要金币
        string moveToLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", item_2, "Item_Template");
        if (moveToLv == "") {
            moveToLv = "1";
        }

        int costRmb = RetuenMoveGold(int.Parse(moveToLv));
        if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() >= costRmb)
        {

            //扣除金币
            Game_PublicClassVar.Get_function_Rose.CostReward("1", costRmb.ToString());

            hide_2 = hide_1;

            //排除附魔相关属性转移
            string prepeotyListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hide_2, "RoseEquipHideProperty");
            if (prepeotyListStr.Contains("FuMo"))
            {
                string writeHideProStr = Game_PublicClassVar.Get_function_Rose.GetHideNoFuMoValue(hide_2);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PrepeotyList", writeHideProStr, "ID", hide_2, "RoseEquipHideProperty");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquipHideProperty");
            }

            //属性写入
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", Move_Space_1, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", hide_2, "ID", Move_Space_2, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");

            //弹出提示
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_346_1");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
        }
        else {
            //弹出金币不足提示
            string langHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_138");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langHint);
        }
    }

    //根据传入的等级计算消耗的金币
    public int RetuenMoveGold(int equipLv) {

        int getGold = 50000 + (int)(equipLv/10) * 20000;
        return getGold;

    }


    //移动装备
    public void Btn_MoveEquip(string type) {

        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus) {

            if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "1")
            {
                bagSpaceNum = Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial;
                moveItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
                if (moveItemID != "")
                {
                    //判定ID是否为装备
                    if (moveItemID.ToString()[0] == '1')
                    {
                        string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItemID, "Item_Template");
                        if (itemType == "3")
                        {

                            //显示道具Icon
                            string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", moveItemID, "Item_Template");
                            object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
                            Sprite itemIcon = obj as Sprite;

                            //显示品质
                            string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", moveItemID, "Item_Template");
                            object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
                            Sprite itemQuality = obj2 as Sprite;

                            switch (type)
                            {

                                case "1":
                                    Obj_EquipPropertyItem_1.GetComponent<Image>().sprite = itemIcon;
                                    Obj_EquipPropertyQuality_1.GetComponent<Image>().sprite = itemQuality;
                                    Move_Space_1 = bagSpaceNum;
                                    break;

                                case "2":
                                    Obj_EquipPropertyItem_2.GetComponent<Image>().sprite = itemIcon;
                                    Obj_EquipPropertyQuality_2.GetComponent<Image>().sprite = itemQuality;
                                    Move_Space_2 = bagSpaceNum;

                                    if (type == "2")
                                    {

                                        //显示更新金币
                                        string moveToLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", moveItemID, "Item_Template");
                                        if (moveToLv == "")
                                        {
                                            moveToLv = "1";
                                        }

                                        int costRmb = RetuenMoveGold(int.Parse(moveToLv));

                                        Obj_EquipPropertyMoveCostMove.GetComponent<XiLianEquipNeedItem>().ItemID = "1";
                                        Obj_EquipPropertyMoveCostMove.GetComponent<XiLianEquipNeedItem>().NeedItemNum = costRmb;
                                        Obj_EquipPropertyMoveCostMove.GetComponent<XiLianEquipNeedItem>().UpdateShow();

                                    }

                                    break;
                            }
                        }
                    }
                }

                //防止背包移动
                Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
                Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
            }
        }
    }

    //显示转移装备
    public void Btn_ShowEquipPropertyMove(string type)
    {
        Debug.Log("Move_Space_1 =" + Move_Space_1 + ";Move_Space_2 = " + Move_Space_2);
        string showItemID = "";
        string showItemSpace = "";
        switch (type) {

            case "1":

                showItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Move_Space_1, "RoseBag");
                showItemSpace = Move_Space_1;
                break;

            case "2":

                showItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Move_Space_2, "RoseBag");
                showItemSpace = Move_Space_2;

                break;
        }

        if (showItemID == "") {
            return;
        }

        GameObject obj_NowEquipTips = null;

        if (obj_NowEquipTips == null)
        {
            if (showItemID != "" && showItemID != "0")
            {
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", showItemID, "Item_Template");
                obj_NowEquipTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(showItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
                //获取目标是否是装备
                if (itemType == "3")
                {
                    obj_NowEquipTips.GetComponent<UI_EquipTips>().UIBagSpaceNum = showItemSpace;
                    obj_NowEquipTips.GetComponent<UI_EquipTips>().EquipTipsType = "1";

                    //获取极品属性
                    string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", showItemSpace, "RoseBag");
                    obj_NowEquipTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;

                    //获取宝石属性
                    string itemGemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", showItemSpace, "RoseBag");
                    string itemGemHole = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", showItemSpace, "RoseBag");
                    obj_NowEquipTips.GetComponent<UI_EquipTips>().ItemGemID = itemGemID;
                    obj_NowEquipTips.GetComponent<UI_EquipTips>().ItemGemHole = itemGemHole;
                }
            }
        }
        else
        {
            Destroy(obj_NowEquipTips);
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
        }
    }



    //关闭洗练按钮
    public void Btn_Close() {
        Destroy(obj_ItemTips);
        Destroy(this.gameObject);
    }
}
