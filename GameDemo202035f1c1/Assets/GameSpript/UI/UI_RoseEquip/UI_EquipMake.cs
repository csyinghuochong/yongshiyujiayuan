using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UI_EquipMake : MonoBehaviour {

    public GameObject Obj_EquipMakeSet;
	//制作列表相关
	public GameObject Obj_ItemList;
	public GameObject Obj_ItemListSet;
	public string nowSeclect;
	public string makeItemStr;
	public GameObject Btn_Lianjin;
	public GameObject Btn_CaiYao;
	public GameObject Btn_DaZao;
    public GameObject Btn_Other;

    //制造书
    public string ItemID;
    private string makeEquipID;                 //制作书ID
    public GameObject Obj_MakeItemNameTitle;    //制作书标题
    public GameObject Obj_MakeItemLv;           //制造书等级
    public GameObject Obj_MakeItemStar;         //制造书星级
    public GameObject Obj_MakeItemNum;          //制造书等级
    public GameObject Obj_MakeItemName;         //制造书名称
    public GameObject Obj_MakeItemQuality;      //制造道具品质显示
    public GameObject Obj_MakeItemIcon;         //制造道具图标显示
    public GameObject Obj_MakeSuccesPro;
    public GameObject Obj_MakeNeedGold;
    public GameObject Obj_MakeNeedItem;         //制造需求的道具源Obj
    public GameObject Obj_MakeEquipNeedItemSet;
    public GameObject Obj_MakeProficiency;
    public GameObject Obj_EquipBtnText_1;
    public GameObject Obj_EquipBtnText_2;
    public GameObject Obj_EquipBtnText_3;
    public GameObject Obj_EquipBtnText_4;
    private string makeItemID;                     //制造道具的ID
    private string makeItemName;
    private int makeItemLv;
    private int makeItemMaxLv;
    private int makeEquipNum;
    private float makeSuccessPro;
    private int makeNeedGold;                 	//制造道具需要金币
    private GameObject obj_ItemTips;         	//制造道具的Tips
    private bool ClickMakeBtn;              	//制造按钮,防止在卡顿的时候多次执行制作操作
    private string nowClickType;
    public GameObject Obj_ProficiencyValue;
    public GameObject Obj_ProficiencyHint;

    //获取当前的制造点数
    private string proficiencyType;
    private int proficiencymax;
    private string proficiencyValue;
    private int nowficiencyValue;
    private int needProficiencyValue;
	public bool UpdateShowStatus;
	public GameObject makeItemlasterObj;

	// Use this for initialization
	void Start () {
		//Debug.Log ("ObjCommonHuoBiSetPosiAAAAAAAAAA = " + ObjCommonHuoBiSetPosi.name);
		//makeItemStr = "10001,10002,10003,10004,10005";
		//Debug.Log ("makeItemStr = " + makeItemStr);
		//更新宠物标题
		Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject, "501");
        
        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_EquipMakeSet);

        //默认打开第一个类型
        nowClickType = "1";
        Click_Type("1");


        //初始化标签按钮
        if (Game_PublicClassVar.Get_wwwSet.GameSetLanguage._Language != "Chinese")
        {
            Obj_EquipBtnText_1.GetComponent<Text>().fontSize = 20;
            Obj_EquipBtnText_2.GetComponent<Text>().fontSize = 20;
            Obj_EquipBtnText_3.GetComponent<Text>().fontSize = 20;
        }

    }
	
	// Update is called once per frame
	void Update () {
	
		if (UpdateShowStatus) {
			UpdateShowStatus = false;
			//showPetList();
			//更新宠物选中图标的显示信息
			for (int i = 0; i < Obj_ItemListSet.transform.childCount; i++)
			{
				GameObject go = Obj_ItemListSet.transform.GetChild(i).gameObject;
				go.GetComponent<UI_EquipMakeListObj>().UpdateStatus = true;
			}
			GetMakeData();
		}

	}

    //获取制作数据
    void GetMakeData() {


		//Debug.Log ("nowSeclect = " + nowSeclect);
		makeEquipID = nowSeclect;
		ItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeItemID", "ID", makeEquipID, "EquipMake_Template");
		string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");

        //获取制造ID
        //makeEquipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
        //string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        string makeStar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeStar", "ID", makeEquipID, "EquipMake_Template");
        makeItemLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeLv", "ID", makeEquipID, "EquipMake_Template"));
        makeItemMaxLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeLvMax", "ID", makeEquipID, "EquipMake_Template"));
        makeEquipNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeEquipNum", "ID", makeEquipID, "EquipMake_Template"));
        makeSuccessPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeSuccessPro", "ID", makeEquipID, "EquipMake_Template"));
        makeNeedGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeNeedGold", "ID", makeEquipID, "EquipMake_Template"));
        //makeNeedGold = 1000;
        makeItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeItemID", "ID", makeEquipID, "EquipMake_Template");
        makeItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", makeItemID, "Item_Template");
        string makeItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", makeItemID, "Item_Template");
        string makeItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", makeItemID, "Item_Template");
        //获取当前的制造点数
        proficiencyType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyType", "ID", makeEquipID, "EquipMake_Template");
        //proficiencymax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyMax", "ID", makeEquipID, "EquipMake_Template"));
        //proficiencyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyValue", "ID", makeEquipID, "EquipMake_Template");
        nowficiencyValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Proficiency_" + proficiencyType, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData"));
        needProficiencyValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedProficiencyValue", "ID", makeEquipID, "EquipMake_Template"));

        //UI赋值
        //Obj_MakeItemNameTitle.GetComponent<Text>().text = itemName;
        if (makeItemLv <= 1&& makeItemMaxLv==0)
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("制造等级：无限制");
            Obj_MakeItemLv.GetComponent<Text>().text = langStr;
        }
        else {

            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("需要等级");
            string langStr_Lv = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("级");
            
            if (makeItemMaxLv == 0)
            {
                Obj_MakeItemLv.GetComponent<Text>().text = langStr + "：" + makeItemLv+ langStr_Lv;
            }
            else {
                Obj_MakeItemLv.GetComponent<Text>().text = langStr + "：" + makeItemLv + " - " + makeItemMaxLv + langStr_Lv;
            }
        }
        //Obj_MakeItemStar.GetComponent<Text>().text = "制造数量：" + makeEquipNum;


        //最低显示1%成功概率
        int showMakeSuccessPro = (int)(makeSuccessPro * 100);
        if (showMakeSuccessPro <= 0) {
            showMakeSuccessPro = 1;
        }

        string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("制造数量");
        string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("成功概率");
        string langStr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("制造难度");
        string langStr_4 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("消耗金币");

        string langStr_5 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("个");
        string langStr_6 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("星");

        Obj_MakeItemNum.GetComponent<Text>().text = langStr_1 + "：" + makeEquipNum + langStr_5;
        Obj_MakeSuccesPro.GetComponent<Text>().text = langStr_2 + "：" + showMakeSuccessPro + "%";
        Obj_MakeItemStar.GetComponent<Text>().text = langStr_3 + "：" + makeStar + langStr_6;
        Obj_MakeNeedGold.GetComponent<Text>().text = langStr_4 + "：" + makeNeedGold;

        //合成道具名称
        Obj_MakeItemName.GetComponent<Text>().text = makeItemName;

        //显示道具Icon
        object obj = Resources.Load("ItemIcon/" + makeItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_MakeItemIcon.GetComponent<Image>().sprite = itemIcon;

        //显示品质
        object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(makeItemQuality), typeof(Sprite));
        Sprite itemQuality = obj2 as Sprite;
        Obj_MakeItemQuality.GetComponent<Image>().sprite = itemQuality;

        //显示熟练度
        string langStr_7 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("需要熟练度");
        Obj_MakeProficiency.GetComponent<Text>().text = langStr_7 + ":" + nowficiencyValue + "/" + needProficiencyValue;
        if (nowficiencyValue >= needProficiencyValue)
        {
            Obj_MakeProficiency.GetComponent<Text>().color = Color.green;
        }
        else {
            Obj_MakeProficiency.GetComponent<Text>().color = Color.red;
        }
        

        //检测金币是否足够
        if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() < makeNeedGold)
        {
            Obj_MakeNeedGold.GetComponent<Text>().color = Color.red;
            string langStr_8 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("金币不足");
            Obj_MakeNeedGold.GetComponent<Text>().text += "("+ langStr_8 + ")";
        }
        

        updataNeedItem();   //更新需求道具
    }

    //更新需求道具
    void updataNeedItem() {

		//清理需求道具
		for (int i = 0; i < Obj_MakeEquipNeedItemSet.transform.childCount; i++)
		{
			GameObject go = Obj_MakeEquipNeedItemSet.transform.GetChild(i).gameObject;
			Destroy (go);
		}

        //创建制作书需求材料
        createNeedItemObj(6);

    }

    //创建制作书需求材料
    void createNeedItemObj(int createNum) {
        for (int i = 1; i <= createNum; i++)
        {
            if (Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID_" + i, "ID", makeEquipID, "EquipMake_Template") != "0")
            { 
                GameObject objMakeNeedItem = (GameObject)Instantiate(Obj_MakeNeedItem);
                objMakeNeedItem.transform.SetParent(Obj_MakeEquipNeedItemSet.transform);
                objMakeNeedItem.transform.localScale = new Vector3(1, 1, 1);

                string needItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID_" + i, "ID", makeEquipID, "EquipMake_Template");
                string needItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemNum_" + i, "ID", makeEquipID, "EquipMake_Template");
                objMakeNeedItem.GetComponent<MakeEquipNeedItem>().ItemID = needItemID;
                objMakeNeedItem.GetComponent<MakeEquipNeedItem>().NeedItemNum = int.Parse(needItemNum);
            }
        }
    }

    //显示制造道具的Tips
    public void Btn_MakeItemTips() {
        if (obj_ItemTips == null)
        {
            //获取当前Tips栏内是否有Tips,如果有就清空处理
            
            GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
            for (int i = 0; i < parentObj.transform.childCount; i++)
            {
                GameObject go = parentObj.transform.GetChild(i).gameObject;
                Destroy(go);
            }
            
            //实例化Tips
            Debug.Log("makeEquipID = " + makeEquipID + ";" + ItemID);
            string itemShowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeItemID", "ID", makeEquipID, "EquipMake_Template");
            obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(itemShowID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemShowID, "Item_Template");
            if (obj_ItemTips != null)
            {
                //获取目标是否是装备
                if (itemType == "3")
                {
                    //obj_ItemTips.GetComponent<UI_EquipTips>().UIBagSpaceNum = BagPosition;
                    obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "3";
                    //获取极品属性
                    //string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", BagPosition, "RoseBag");
                    //obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;
                }
                else
                {
                    //其余默认为道具,如果其他道具需做特殊处理
                    //obj_ItemTips.GetComponent<UI_ItemTips>().UIBagSpaceNum = BagPosition;
                    obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = "3";
                }
            }
        }
        else {
            Destroy(obj_ItemTips);
        }
    }


    //点击制造按钮
    public void Btn_MakeItem() {

        //如果当前处于制作中,则不返回任何制造
        if (ClickMakeBtn) {
            return;
        }

       //Debug.Log("我制造了一件装备");
        bool makeStatus = true;
        ClickMakeBtn = true;

        //检测等级是否达到
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (makeItemMaxLv == 0)
        {
            makeItemMaxLv = 999999;
        }

        if (roseLv < makeItemLv || roseLv > makeItemMaxLv) {
            makeStatus = false;
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_347");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("请查看制造等级,角色不在此制造等级范围内无法制造");
            ClickMakeBtn = false;
            return;
        }

        /*
        //检测本身制造制作书是否存在
        int makeNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        //当某一材料未达成合成显示失败
        if (makeNum < 1)
        {
            makeStatus = false;
            Game_PublicClassVar.Get_function_UI.GameHint("制造书不在背包当中!");
            ClickMakeBtn = false;
            return;
        }
        */

        //检测剩余背包
        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() <= 1) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_348");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("制造道具背包至少预留2个位置...");
            return;
        }

        //检测道具是否足够
        for (int i = 1; i <= 6; i++)
        {
            string needItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID_" + i, "ID", makeEquipID, "EquipMake_Template");
            if (needItemID != "0" && needItemID != "")
            {
                string needItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemNum_" + i, "ID", makeEquipID, "EquipMake_Template");
                if (needItemNum == "")
                {
                    needItemNum = "0";
                }
                int selfItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(needItemID);
                //当某一材料未达成合成显示失败
                if (selfItemNum < int.Parse(needItemNum))
                {
                    makeStatus = false;
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_349");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameHint("制造需要的材料不足!");
                    ClickMakeBtn = false;
                    return;
                }
            }
        }

        //检测金币是否足够
        if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() < makeNeedGold)
        {
            //金币不足
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_350");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("制造需要的金币不足!");
            makeStatus = false;
            ClickMakeBtn = false;
            return;
        }

        //检测熟练度是否足够
        string proficiencyType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyType", "ID", makeEquipID, "EquipMake_Template");
        int needProficiencyValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedProficiencyValue", "ID", makeEquipID, "EquipMake_Template"));
        //获取当前的制造点数
        int nowficiencyValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Proficiency_" + proficiencyType, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData"));
        if (nowficiencyValue < needProficiencyValue) {
            //金币不足
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_351");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("制造需要的熟练度不足!");
            makeStatus = false;
            ClickMakeBtn = false;
            return;
        }

        //制造成功
        if (makeStatus)
        {

            //执行道具

            //判定目标是否有宝石
            bool ifHint = false;
            for (int i = 1; i <= 6; i++)
            {
                string needItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID_" + i, "ID", makeEquipID, "EquipMake_Template");
                if (needItemID != "0" && needItemID != "")
                {
                    //判断目标是否为装备
                    string noeItemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", needItemID, "Item_Template");
                    if (noeItemType == "3") {

                        string nowSpace = Game_PublicClassVar.Get_function_Rose.ReturnBagFirstSpace(needItemID);
                        if (nowSpace != "" && needItemID != "0" && needItemID != "-1" && needItemID != null) {

                           string nowGemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", nowSpace, "RoseBag");
                            if (nowGemID != "0" && nowGemID != "" && nowGemID != null) {
                                ifHint = true;
                            }
                        }
                    }
                }
            }

            if (ifHint)
            {

                GameObject Obj_XiLianHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                //string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_17");
                //string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_18");
                Obj_XiLianHint.GetComponent<UI_CommonHint>().Btn_CommonHint("在即将消耗的装备上发现了宝石或宝石槽位,是否继续制作？\n提示:继续制作将损失宝石。", null, MakeItem, "制作提示", "取消", "确定", null);
                //Obj_XiLianHint.GetComponent<UI_CommonHint>().Btn_CommonHint("在此装备上发现了隐藏技能:"+ hintSkillName + "！\n提示:如果再次洗炼隐藏技能有可能消失！", Btn_EquipXiLian, null);
                Obj_XiLianHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                Obj_XiLianHint.transform.localPosition = Vector3.zero;
                Obj_XiLianHint.transform.localScale = new Vector3(1, 1, 1);

            }
            else {
                MakeItem();
            }

            /*
            //扣除对应道具
            for (int i = 1; i <= 6; i++)
            {
                string needItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID_" + i, "ID", makeEquipID, "EquipMake_Template");
                if (needItemID != "0" && needItemID != "")
                {
                    string needItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemNum_" + i, "ID", makeEquipID, "EquipMake_Template");
                    if (needItemNum == "") {
                        needItemNum = "0";
                    }
                    Game_PublicClassVar.Get_function_Rose.CostBagItem(needItemID, int.Parse(needItemNum));
                }
            }

            //Debug.Log("makeSuccessPro = " + makeSuccessPro);
            //获取成功概率
            if (Random.value <= makeSuccessPro)
            {
                string langStrHint_A = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_352");
                string hintStr = langStrHint_A + makeItemName;
                //string hintStr = "制造成功！获得装备：" + makeItemName;
                //发送对应奖励
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", makeItemID, "Item_Template");
                if (itemType == "3")
                {
                    //触发随机属性
                    string makeHintPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeHintPro", "ID", makeEquipID, "EquipMake_Template");
                    //根据熟练度换算附加概率
                    float makePro = float.Parse(makeHintPro) + shulianToPro();
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(makeItemID, makeEquipNum, "1", makePro,"0",true,"38");

                    //写入活跃任务
                    string itemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", makeItemID, "Item_Template");
                    if (int.Parse(itemQuality) >= 4) {
                        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "101", "1");
                    }
                }
                else {
                    //其余道具直接发送到背包中
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(makeItemID, makeEquipNum, "1", 0, "0", true, "38");

                    //判定是否触发暴击
                    float nowMakeBaoJi = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_YaoJiCirPro;

                    //道具类型不为宝石,宝石不可以附加暴击
                    if (itemType != "4") {
                        //根据熟练度换算附加概率
                        nowMakeBaoJi = nowMakeBaoJi + shulianToPro();
                        if (Random.value < nowMakeBaoJi)
                        {
                            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_297");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你！制造产生了暴击获得双倍物品！");
                            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(makeItemID, makeEquipNum, "1", 0, "0", true, "38");
                        }
                    }
                }

                //扣除背包制造卷轴
                //Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
                Game_PublicClassVar.Get_function_UI.GameHint(hintStr);


                //不论制造的成功和失败都增长相应的制造技能点数
                int proficiencymax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyMax", "ID", makeEquipID, "EquipMake_Template"));
                string proficiencyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyValue", "ID", makeEquipID, "EquipMake_Template");

                if (nowficiencyValue < proficiencymax)
                {
                    //获取加的值是单值还是随机值
                    int addValue = 0;
                    string[] addValueList = proficiencyValue.Split(',');
                    if (addValueList.Length > 1)
                    {
                        addValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(int.Parse(addValueList[0]), int.Parse(addValueList[1]));
                    }
                    else
                    {
                        addValue = int.Parse(addValueList[0]);
                    }
                    //Debug.Log("准备添加");
                    Game_PublicClassVar.Get_function_Rose.AddMakeProficiencyValue(proficiencyType, addValue);
                }
                else
                {
                    //已达上限不增加熟练度

                }

                //Btn_CloseUI();

            }
            else {
                //制造失败
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_353");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameHint("制造失败,下次制造概率提升！");
            }

            //制造成功刷新下界面显示
            //updateMake();
            GetMakeData();
            ShowShuLian();
            */
        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_354");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("制造材料不足");
        }

        //制造完毕,防止卡顿时多次制造道具
        ClickMakeBtn = false;
    }

    public void MakeItem()
    {

        //扣除对应道具
        for (int i = 1; i <= 6; i++)
        {
            string needItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID_" + i, "ID", makeEquipID, "EquipMake_Template");
            if (needItemID != "0" && needItemID != "")
            {
                string needItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemNum_" + i, "ID", makeEquipID, "EquipMake_Template");
                if (needItemNum == "")
                {
                    needItemNum = "0";
                }
                Game_PublicClassVar.Get_function_Rose.CostBagItem(needItemID, int.Parse(needItemNum));
            }
        }

        //Debug.Log("makeSuccessPro = " + makeSuccessPro);
        //获取成功概率
        if (Random.value <= makeSuccessPro)
        {
            string langStrHint_A = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_352");
            string hintStr = langStrHint_A + makeItemName;
            //string hintStr = "制造成功！获得装备：" + makeItemName;
            //发送对应奖励
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", makeItemID, "Item_Template");
            if (itemType == "3")
            {
                //触发随机属性
                string makeHintPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeHintPro", "ID", makeEquipID, "EquipMake_Template");
                //根据熟练度换算附加概率
                float makePro = float.Parse(makeHintPro) + shulianToPro();
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(makeItemID, makeEquipNum, "1", makePro, "0", true, "38");

                //写入活跃任务
                string itemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", makeItemID, "Item_Template");
                if (int.Parse(itemQuality) >= 4)
                {
                    Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "101", "1");
                }
            }
            else
            {
                //其余道具直接发送到背包中
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(makeItemID, makeEquipNum, "1", 0, "0", true, "38");

                //判定是否触发暴击
                float nowMakeBaoJi = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_YaoJiCirPro;

                //道具类型不为宝石,宝石不可以附加暴击
                if (itemType != "4")
                {
                    //根据熟练度换算附加概率
                    nowMakeBaoJi = nowMakeBaoJi + shulianToPro();
                    if (Random.value < nowMakeBaoJi)
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_297");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你！制造产生了暴击获得双倍物品！");
                        Game_PublicClassVar.Get_function_Rose.SendRewardToBag(makeItemID, makeEquipNum, "1", 0, "0", true, "38");
                    }
                }
            }

            //扣除背包制造卷轴
            //Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
            Game_PublicClassVar.Get_function_UI.GameHint(hintStr);


            //不论制造的成功和失败都增长相应的制造技能点数
            int proficiencymax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyMax", "ID", makeEquipID, "EquipMake_Template"));
            string proficiencyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyValue", "ID", makeEquipID, "EquipMake_Template");

            if (nowficiencyValue < proficiencymax)
            {
                //获取加的值是单值还是随机值
                int addValue = 0;
                string[] addValueList = proficiencyValue.Split(',');
                if (addValueList.Length > 1)
                {
                    addValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(int.Parse(addValueList[0]), int.Parse(addValueList[1]));
                }
                else
                {
                    addValue = int.Parse(addValueList[0]);
                }
                //Debug.Log("准备添加");
                Game_PublicClassVar.Get_function_Rose.AddMakeProficiencyValue(proficiencyType, addValue);
            }
            else
            {
                //已达上限不增加熟练度

            }

            //Btn_CloseUI();

        }
        else
        {
            //制造失败
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_353");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("制造失败,下次制造概率提升！");
        }

        //制造成功刷新下界面显示
        //updateMake();
        GetMakeData();
        ShowShuLian();

    }

    private float shulianToPro() {

        float proValue = 0;

        //熟练度转换成概率
        int proficiencyStartProValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyStartProValue", "ID", makeEquipID, "EquipMake_Template"));
        int proficiencyProValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyProValue", "ID", makeEquipID, "EquipMake_Template"));

        //获取对应熟练度
        int ProficiencyValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Proficiency_" + nowClickType, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData"));

        if (ProficiencyValue> proficiencyStartProValue) {
            proValue = (ProficiencyValue - proficiencyStartProValue) / proficiencyProValue;
            if (proValue > 0.1f) {
                proValue = 0.1f;
            }
        }

        return proValue;
    }

    //关闭UI
    public void Btn_CloseUI() {

        //获取当前Tips栏内是否有Tips,如果有就清空处理

        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }

		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().OpenMakeItem();

        Destroy(this.gameObject);
    }


	//展示制造列表
	void showPetList() {

		string[] makeItemList = makeItemStr.Split (',');

		//清空列表
		Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ItemListSet);

        //重置位置
        Obj_ItemListSet.transform.localPosition = new Vector3(Obj_ItemListSet.transform.localPosition.x,0, Obj_ItemListSet.transform.localPosition.z);

        //显示宠物列表
        for (int i = 0; i < makeItemList.Length; i++)
		{

			//string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", i.ToString(), "RosePet");
            int listNum = makeItemList.Length - i - 1;

            if (makeItemList[listNum] != "0" && makeItemList[listNum] != "")
            {
				//实例化一个宠物列表控件
				GameObject petListObj = (GameObject)Instantiate(Obj_ItemList);
				petListObj.transform.SetParent(Obj_ItemListSet.transform);
				petListObj.transform.localScale = new Vector3(1, 1, 1);

                petListObj.GetComponent<UI_EquipMakeListObj>().PetOnlyID = makeItemList[listNum];
				petListObj.GetComponent<UI_EquipMakeListObj>().UpdateStatus = true;
				petListObj.GetComponent<UI_EquipMakeListObj>().Obj_FuJiObj = this.gameObject;
			}
		}

		//设置位置
		if (makeItemList.Length >= 10) {
			Obj_ItemListSet.GetComponent<RectTransform>().sizeDelta = new Vector2 (120, 103 * makeItemList.Length);
		} else {
			Obj_ItemListSet.GetComponent<RectTransform>().sizeDelta = new Vector2 (120, 1030);
		}
	}


	private void updateMake(){

		string[] makeItemList = makeItemStr.Split (',');
        int listNum = makeItemList.Length-1;
        if(listNum<0){
            listNum = 0;
        }
        nowSeclect = makeItemList[listNum];
		showPetList();
		GetMakeData();
        ShowShuLian();

    }


    void ShowShuLian() {

        //展示熟练点数
        string ProficiencyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Proficiency_" + nowClickType, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");

        string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("打造熟练度");
        string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("炼金熟练度");
        string langStr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("宝石熟练度");

        string langStr_4 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("提升打造极品的概率");
        string langStr_5 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("炼金暴击!有概率获得双倍收益");

        //展示熟练度文本
        string showStr = "";
        switch (nowClickType) {
            case "1":
                showStr = langStr_1;
                Obj_ProficiencyValue.GetComponent<Text>().text = showStr + ProficiencyValue;
                Obj_ProficiencyHint.GetComponent<Text>().text = "("+ langStr_4 + ")";
                break;
            case "2":
                showStr = langStr_2;
                Obj_ProficiencyValue.GetComponent<Text>().text = showStr + ProficiencyValue;
                Obj_ProficiencyHint.GetComponent<Text>().text = "("+ langStr_5 + ")";
                break;
            case "3":
                showStr = langStr_3;
                Obj_ProficiencyValue.GetComponent<Text>().text = "";
                Obj_ProficiencyHint.GetComponent<Text>().text = "";
                break;
        }
    }

	//点击按钮
	public void Click_Type(string type)
	{

		//显示按钮
		object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
		Sprite img = obj as Sprite;
		Btn_CaiYao.GetComponent<Image>().sprite = img;
		Btn_DaZao.GetComponent<Image>().sprite = img;
		Btn_Lianjin.GetComponent<Image>().sprite = img;
        Btn_Other.GetComponent<Image>().sprite = img;

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);

        nowClickType = type;


        switch (type)
		{
		//炼金
		case "1":
			//展示角色
			Btn_Lianjin.SetActive(true);

			//显示底图
			obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
			img = obj as Sprite;
			Btn_Lianjin.GetComponent<Image>().sprite = img;
            Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

			//makeItemStr = "10001,10002,10003,10004,10005";
            makeItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_" + type, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");


			break;

		//裁缝
		case "2":
			//Obj_Pet_HeChengSet.SetActive(true);

			//显示底图
			obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
			img = obj as Sprite;
			Btn_CaiYao.GetComponent<Image>().sprite = img;
            Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

			//makeItemStr = "10006,10007,10008,10009";
            makeItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_" + type, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");


			//重置位置
			/*
			Obj_Pet_HeChengSet.GetComponent<UI_PetHeCheng>().PetID_List_1 = "0";
			Obj_Pet_HeChengSet.GetComponent<UI_PetHeCheng>().PetID_List_2 = "0";
			Obj_Pet_HeChengSet.GetComponent<UI_PetHeCheng>().showHeChengList();
			*/
			break;

        //打造
		case "3":

			//显示底图
			obj = Resources.Load ("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
			img = obj as Sprite;
			Btn_DaZao.GetComponent<Image> ().sprite = img;
			Obj_EquipBtnText_3.GetComponent<Text> ().color = new Color (0.415f, 0.25f, 0.1f);

			//makeItemStr = "10002,10002,10003,10004,10005,10006,10007,10008,10009,10001,10002,10003,10004,10005,10006,10007,10008,10009,10001,10002,10003,10004,10005,10006,10007,10008,10009,10001,10002,10003,10004,10005,10006,10007,10008";
			makeItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ProficiencyID_" + type, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");
			//Debug.Log ("makeItemStr11111 = " + makeItemStr);
			//按照ID进行排序
			List<string> strList = new List<string> ();
			strList = makeItemStr.Split (',').ToList ();
			strList.Sort ((x, y) => -x.CompareTo(y));
			string nowMakeItemStr = "";
			foreach (string strValue in strList) {
				nowMakeItemStr = nowMakeItemStr + strValue + ",";
			}
			if (nowMakeItemStr != "") {
				nowMakeItemStr = nowMakeItemStr.Substring (0, nowMakeItemStr.Length - 1);
				makeItemStr = nowMakeItemStr;
			}
			//Debug.Log ("makeItemStr22222 = " + makeItemStr);
			break;

            //其他
            case "4":
                //展示角色
                Btn_Other.SetActive(true);

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                Btn_Other.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                //makeItemStr = "10001,10002,10003,10004,10005";
                makeItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_" + type, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");


                break;
        }

		updateMake();
	}
}
