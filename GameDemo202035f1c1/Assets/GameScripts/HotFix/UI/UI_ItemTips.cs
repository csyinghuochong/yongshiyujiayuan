using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_ItemTips : MonoBehaviour {

    public GameObject Obj_ItemQuality;
    public GameObject Obj_ItemIcon;
    public GameObject ItemName;
    public GameObject ItemDes;
	public GameObject ItemStory;
	public GameObject ItemItemLv;
    public GameObject ItemType;
	public GameObject ItemDi;
    public GameObject Obj_BagOpenSet;
    public GameObject Obj_SaveStoreHouse;
    public GameObject Obj_Diu;
    public GameObject Obj_Btn_StoreHouseSet;
    public GameObject Obj_Btn_GemHoleText;
	public GameObject Obj_Btn_HuiShou;
	public GameObject Obj_Btn_HuiShouCancle;
    public GameObject Obj_Btn_XieXiaGemSet;
    private GameObject selPastureItemUICommonHint;

    public ObscuredString ItemID;
	private ObscuredString itemQuality;
    private ObscuredString Text_ItemName;
    private ObscuredString Text_ItemDes;
	private ObscuredString Text_ItemStory;
	private ObscuredString Text_ItemLv;
    private ObscuredString itemType;            //道具大类
    private ObscuredString itemSubType;         //道具子类
    public ObscuredString UIBagSpaceNum;
    public ObscuredString EquipTipsType;        //1.背包打开  2.装备栏打开 3.无任何按钮显示,点击商店列表弹出

    private bool clickBtnStatus;        //点击按钮状态
    private bool useItemStatus;

    // Use this for initialization
    void Start()
    {

        //获取道具信息
        Text_ItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");

        //职业处理
        string nowOcc = Game_PublicClassVar.Get_function_Rose.GetRoseOcc();
        if (nowOcc == "2") {

        }

        Text_ItemDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemDes", "ID", ItemID, "Item_Template");
		Text_ItemStory = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemBlackDes", "ID", ItemID, "Item_Template");
		Text_ItemLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", ItemID, "Item_Template");
		itemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
        itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");
        itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");

        //类型描述
        string typeName = "消耗品";
        switch (itemType) {
            case "1":
                typeName = "消耗品";
                break;
            case "2":
                typeName = "材料";
                break;
            case "3":
                typeName = "装备";
                break;
            case "4":
                typeName = "宝石";
                break;
            case "5":
                typeName = "技能";
                break;
        }

        typeName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(typeName);
        string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("类型");
        ItemType.GetComponent<Text>().text = langStr_1 + "：" + typeName;

        //描述特殊处理
        /*
        if (itemType == "1") {

            if (itemSubType == "32") {
                if (EquipTipsType == "1") {
                }
                string itemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", UIBagSpaceNum, "RoseBag");
                string scenceName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", itemPar, "Scene_Template");
                }

        }
        */

        //获取道具描述的分隔符
        string[] itemDesArray = Text_ItemDes.ToString().Split(';');
        string itemMiaoShu = "";
        for (int i = 0; i <= itemDesArray.Length - 1; i++)
        {
            if (itemMiaoShu == "")
            {
                itemMiaoShu = itemDesArray[i];
            }
            else {
                itemMiaoShu = itemMiaoShu + "\n" + itemDesArray[i];
            }
        }

        //数组大于2表示有换行符,否则显示原来的描述
        if (itemDesArray.Length >= 2) {
            Text_ItemDes = itemMiaoShu;
        }

		//根据Tips描述长度缩放底的大小
		int i1 = 0;
		int i2 = 0;
		i1 = (int)((Text_ItemDes.Length)/16)+1;
        if (itemDesArray.Length > i1) {
            i1 = itemDesArray.Length;
        }
        string langStr="";
        //宝石显示额外的描述
        if (itemType == "4")
        {
            string holeStr = "";
            string[] holeStrList = itemSubType.ToString().Split(',');
            for (int i = 0; i < holeStrList.Length; i++) {
                switch (holeStrList[i]) { 
                    case "101":
                        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("红色");
                        holeStr = holeStr + langStr + "、";
                        break;

                    case "102":
                        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("紫色");
                        holeStr = holeStr + langStr + "、";
                        break;

                    case "103":
                        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("蓝色");
                        holeStr = holeStr + langStr + "、";
                        break;

                    case "104":
                        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("绿色");
                        holeStr = holeStr + langStr + "、";
                        break;

                    case "105":
                        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("白色");
                        holeStr = holeStr + langStr + "、";
                        break;

                    case "110":
                        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("多彩");
                        holeStr = holeStr+ langStr + "、";
                        break;
                }
            }

            if (holeStr != "") {
                holeStr = holeStr.Substring(0, holeStr.Length-1);
            }

            //特殊处理
            if (itemSubType == "101,102,103,104,105,110")
            {
                string langStr_B = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("任意颜色的");
                holeStr = langStr_B;
            }

            i1 = i1 + 2;

            string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("可镶嵌在");
            string langStr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("孔位");
            //Text_ItemDes = Text_ItemDes + "\n" + "\n" + @"""可镶嵌在"+ holeStr + @"孔位""";
            Text_ItemDes = Text_ItemDes + "\n" + "\n" + @""+ langStr_2 + holeStr + @langStr_3 + "";
        }

        //藏宝图额外描述
        if (itemType == "1")
        {
            if (itemSubType == "32")
            {
                string langStr_4 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("挖宝位置");
                //背包显示
                if (EquipTipsType == "1")
                {
                    string itemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", UIBagSpaceNum, "RoseBag");
                    string scenceName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", itemPar, "Scene_Template");
                    Text_ItemDes = Text_ItemDes + "\n" + "\n" + langStr_4 + ":" + scenceName;
                    i1 = i1 + 2;
                }
                //仓库显示
                if (EquipTipsType == "4")
                {
                    string itemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", UIBagSpaceNum, "RoseStorehouse");
                    string scenceName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", itemPar, "Scene_Template");
                    Text_ItemDes = Text_ItemDes + "\n" + "\n" + langStr_4 + ":" + scenceName;
                    i1 = i1 + 2;
                }
            }
        }

        //牧场道具额外描述
        if (itemType == "6") {
            string langStr_5 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("品质");

            string itemPar = "0";
            if (EquipTipsType == "9"|| EquipTipsType == "10")
            {
                itemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", UIBagSpaceNum, "RosePastureBag");
            }

            if (EquipTipsType == "1")
            {
                itemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", UIBagSpaceNum, "RoseBag");
            }

            if (EquipTipsType == "4")
            {
                itemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", UIBagSpaceNum, "RoseStoreHouse");
            }

            Text_ItemDes = Text_ItemDes + "\n" + "\n" + "<color=#F0E2C0FF>" + langStr_5 + ":" + itemPar + "</color>";
            i1 = i1 + 2;
        }

		ItemDes.GetComponent<RectTransform>().sizeDelta = new Vector2(252.0f,20.0f * i1);
        Vector2 i1Vec2 = new Vector2 (148.0f,-90-10.0f * i1);
        ItemDes.transform.GetComponent<RectTransform>().anchoredPosition = i1Vec2;

        //赞助宝箱设置描述为绿色
        if (itemSubType == "9") {
            ItemDes.GetComponent<Text>().color = Color.green;
        }
        
        //显示图标
        //显示道具Icon
        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
        //职业处理

        //string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
        object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ItemIcon.GetComponent<Image>().sprite = itemIcon;

        object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(itemQuality), typeof(Sprite));
        Sprite itemQualityobj = obj2 as Sprite;
        Obj_ItemQuality.GetComponent<Image>().sprite = itemQualityobj;


        //显示道具描述
        i2 = (int)((Text_ItemStory.Length) / 20) + 1;
		ItemStory.GetComponent<RectTransform>().sizeDelta = new Vector2(260.0f,120.0f+16.0f*i2);
        Vector2 i2Vec2 = new Vector2(148.0f, -205 - 10.0f * i1 - 8.0f * i2);
        ItemStory.transform.GetComponent<RectTransform>().anchoredPosition = i2Vec2;
        float ItemBottomTextNum = 30.0f;

        Obj_BagOpenSet.SetActive(false);
        Obj_Btn_StoreHouseSet.SetActive(false);
        Obj_SaveStoreHouse.SetActive(false);
		Obj_Btn_HuiShou.SetActive(false);
		Obj_Btn_HuiShouCancle.SetActive (false);
        Obj_Btn_XieXiaGemSet.SetActive(false);

        //显示按钮
        switch (EquipTipsType)
        {
			//不显示任何按钮
			case "0":
				ItemBottomTextNum = 0;
				break;

            //背包打开显示对应功能按钮
            case "1":
                Obj_BagOpenSet.SetActive(true);
                Obj_Btn_StoreHouseSet.SetActive(false);
                //判定当前是否打开仓库
                if (Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus)
                {
                    Obj_SaveStoreHouse.SetActive(true);
                    Obj_Diu.SetActive(false);
                }
                else {
                    Obj_SaveStoreHouse.SetActive(false);
                    Obj_Diu.SetActive(true);
                }
                break;
            //角色栏打开显示对应功能按钮
            case "2":
                Obj_BagOpenSet.SetActive(true);
                Obj_Btn_StoreHouseSet.SetActive(false);
                break;
            //商店查看属性
            case "3":
                Obj_BagOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(false);
                ItemBottomTextNum = 0;
                break;

            //仓库查看属性
            case "4":
                Obj_BagOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(true);
                //ItemBottomTextNum = 0;
                break;

			//回收背包打开
			case "6":
				Obj_BagOpenSet.SetActive (true);
				Obj_Btn_StoreHouseSet.SetActive (false);
				Obj_SaveStoreHouse.SetActive (false);
				Obj_Btn_HuiShou.SetActive(true);
				break;

			//回收功能显示
			case "7":
				Obj_BagOpenSet.SetActive(false);
				Obj_Btn_StoreHouseSet.SetActive(false);
				Obj_Btn_HuiShouCancle.SetActive (true);
				break;

            //宝石镶嵌
            case "8":
                Obj_Btn_XieXiaGemSet.SetActive(true);
                break;

            //牧场  不显示任何按钮
            case "9":
                ItemBottomTextNum = 0;
                break;

            //牧场仓库  显示出售按钮
            case "10":
                Obj_BagOpenSet.SetActive(true);
                Obj_Btn_StoreHouseSet.SetActive(false);
                Obj_SaveStoreHouse.SetActive(false);
                Obj_Diu.SetActive(true);
                break;

            //不显示任何按钮
            default:
                ItemBottomTextNum = 0;
                break;
        }

        //判定道具为宝石时显示使用变为镶嵌字样
        if (itemType == "4")
        {
            string langStr_A = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("镶 嵌");
            Obj_Btn_GemHoleText.GetComponent<Text>().text = langStr_A;
        }

		//设置底的长度
        ItemDi.GetComponent<RectTransform>().sizeDelta = new Vector2(301.0f, 170.0f + i1 * 20.0f + i2 * 16.0f + ItemBottomTextNum);

        //显示道具信息
		ItemName.GetComponent<Text>().text = Text_ItemName;
		ItemName.GetComponent<Text>().color = Game_PublicClassVar.Get_function_UI.QualityReturnColor(itemQuality);
        ItemDes.GetComponent<Text>().text = Text_ItemDes;
		ItemStory.GetComponent<Text>().text = Text_ItemStory;

        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("使用等级");
        if (int.Parse (Text_ItemLv) > 0) {
            ItemItemLv.GetComponent<Text>().text = langStr + ":" + Text_ItemLv;
		} else {
            ItemItemLv.GetComponent<Text>().text = langStr + ":1" ;
		}

        //监测UI是否超过底部显示
        float DiHight = ItemDi.GetComponent<RectTransform>().sizeDelta.y;
        //Debug.Log("DiHight = " + DiHight);
        float screen_higeValue = 768 * Game_PublicClassVar.Get_function_UI.ReturnScreenScalePro();
        float UIHeadValue = screen_higeValue - this.transform.localPosition.y - DiHight / 2;            //UI和顶部的距离
        float UIHightValue = UIHeadValue + DiHight + 50.0f;

        if (UIHightValue >= screen_higeValue)
        {
            //Debug.Log("UI触底了");
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 50 + DiHight / 2, 0);
        }
        //监测UI是否超过了顶部显示
        if (UIHeadValue <= 30)
        {
            //Debug.Log("UI触顶了");
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, screen_higeValue - DiHight / 2-50, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {

    }


    //使用道具按钮
    public void UseItem()
    {
        if (useItemStatus) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_379");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("使用道具间隔时间太短!请稍后再使用次道具");
            return;
        }

        //判断背包是否有空格
        //if(bag)

        useItemStatus = true;
        Game_PublicClassVar.Get_game_PositionVar.UseItemStatus = true;

        bool CloseUIStatus = true;

        if (itemType == "1") {

            switch (itemSubType) { 
                //触发技能
                case "0":
                    //判定当前是否在建筑地图
                    /*
                    if (Application.loadedLevelName == "EnterGame")
                    {
                        Game_PublicClassVar.Get_function_UI.GameHint("请进入野外地图在使用此道具");
                        break;
                    }
                    */
                    //指定一些地图不能使用技能
                    if (Application.loadedLevelName == "100001")
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_42");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("此地图不能使用恢复类道具！");
                        break;
                    }

                    string useSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", ItemID, "Item_Template");
                    if (useSkillID != "0") {
                        GameObject skill_0 = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainFunctionUI.transform.Find("UI_MainRoseSkill_0").gameObject;
                        skill_0.GetComponent<MainUI_SkillGrid>().SkillID = ItemID;
                        skill_0.GetComponent<MainUI_SkillGrid>().UseSkillID = useSkillID;
                        skill_0.GetComponent<MainUI_SkillGrid>().updataSkill();
                        skill_0.GetComponent<MainUI_SkillGrid>().cleckbutton();
                    }
                    break;
                //触发掉落(掉落地上)
                case "1":
                    //判定当前是否在建筑地图
                    if (Application.loadedLevelName == "EnterGame")
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_385");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameHint("请进入关卡地图在使用此道具");
                        break;
                    }
                    string ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    Game_PublicClassVar.Get_function_AI.DropIDToDropItem(ItemUsePar, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                    Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID,1, UIBagSpaceNum);        //销魂自身道具
                break;

                //经验盒子
                case "2":
                    //ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    //实例化制作UI
                    GameObject heZiObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIHeZi);
                    heZiObj.GetComponent<UI_HeZi>().ItemID = ItemID;
                    heZiObj.GetComponent<UI_HeZi>().BagSpace = UIBagSpaceNum;
                    heZiObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                    heZiObj.transform.localScale = new Vector3(1, 1, 1);
                    heZiObj.transform.localPosition = Vector3.zero;

                break;

                //金币盒子
                case "3":
                    //ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    heZiObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIHeZi);
                    heZiObj.GetComponent<UI_HeZi>().ItemID = ItemID;
                    heZiObj.GetComponent<UI_HeZi>().BagSpace = UIBagSpaceNum;
                    heZiObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                    heZiObj.transform.localScale = new Vector3(1, 1, 1);
                    heZiObj.transform.localPosition = Vector3.zero;
                break;

                //回城卷轴
                case "4":
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID,1, UIBagSpaceNum);        //销魂自身道具
                    //写入场景
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";                     //设置角色为待机状态
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Status = false;           //设置角色不能移动
                    string ScenceID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", ItemUsePar, "SceneTransfer_Template");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", ScenceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = ItemUsePar;
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();
                    //Game_PublicClassVar.Get_function_UI.PlaySource("10003", "1");
                break;

                //制作书
                case "5":
                    //Debug.Log("调用制作书");
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    //清空之前打开的制作书
                    Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EquipMakeSet);
                    //实例化制作UI
                    GameObject equipMakeObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIEquipMake);
                    equipMakeObj.GetComponent<UI_EquipMake>().ItemID = ItemID;
                    equipMakeObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EquipMakeSet.transform);
                    equipMakeObj.transform.localScale = new Vector3(1, 1, 1);
                    equipMakeObj.transform.localPosition = Vector3.zero;
                break;

                //直接获得经验
                case "6":
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");

                    string bagNum = "1";
                    //默认使用1个,经验道具默认使用当前全部
                    if (ItemID == "10010061" || ItemID == "10010062" || ItemID == "10010063" || ItemID == "10010064" || ItemID == "10010065" || ItemID == "10010066" || ItemID == "10010067") {
                        bagNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", UIBagSpaceNum, "RoseBag");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你使用了" + bagNum + "个经验卷轴");
                    }

                    //获取道具数量
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, int.Parse(bagNum), UIBagSpaceNum)) {
                        Game_PublicClassVar.Get_function_Rose.AddExp(int.Parse(ItemUsePar) * int.Parse(bagNum));
                        //判定自己背包内是否还有道具
                        int itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                        if (itemBagNum > 0)
                        {
                            CloseUIStatus = false;
                        }
                    }

                    break;

                //直接获得金币
                case "7":
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");

                    bagNum = "1";
                    //默认使用1个,经验道具默认使用当前全部
                    if (ItemID == "10010071" || ItemID == "10010072" || ItemID == "10010073" || ItemID == "10010074" || ItemID == "10010075")
                    {
                        bagNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", UIBagSpaceNum, "RoseBag");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你使用了" + bagNum + "个金币袋子");
                    }

                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, int.Parse(bagNum), UIBagSpaceNum)) {
                        Game_PublicClassVar.Get_function_Rose.SendReward("1", (int.Parse(ItemUsePar)* int.Parse(bagNum)).ToString(), "48");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你获得金币:"+ (int.Parse(ItemUsePar) * int.Parse(bagNum)).ToString());
                        //判定自己背包内是否还有道具
                        int itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                        if (itemBagNum > 0)
                        {
                            CloseUIStatus = false;
                        }
                    }

                break;

                //集齐道具触发一个掉落ID
                case "8":
                    //判定当前是否在建筑地图
                    /*
                    if (Application.loadedLevelName == "EnterGame") {
                        Game_PublicClassVar.Get_function_UI.GameHint("请进入关卡地图在使用此道具");
                        break;
                    }
                    */

                    if (Game_PublicClassVar.Get_function_Rose.BagNullNum() <= 0) {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_43");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("开始失败!请背包内至少预留5个位置!");
                        break;
                    }

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    string needItemNum = ItemUsePar.Split(';')[0];
                    string itemDropID = ItemUsePar.Split(';')[1];
                    string hideID = ItemUsePar.Split(';')[2];
                    //获取背包道具是否足够
                    int bagItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                    if (bagItemNum >= int.Parse(needItemNum) && int.Parse(needItemNum)>=1) {
                        //获取当前背包是否足够
                        if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, int.Parse(needItemNum),UIBagSpaceNum)) {
                            //Debug.Log("触发掉落");
                            Game_PublicClassVar.Get_function_AI.DropIDToDropItem(itemDropID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position, hideID);
                        }
                    }
                    break;

                //充值额度,触发掉落
                case "9":
                    //判定当前是否在建筑地图
                    /*
                    if (Application.loadedLevelName == "EnterGame")
                    {
                        Game_PublicClassVar.Get_function_UI.GameHint("请进入关卡地图在使用此道具");
                        break;
                    }
                    */

                    if (Game_PublicClassVar.Get_function_Rose.BagNullNum() <= 0)
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_43");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("开始失败!请背包内至少预留5个位置!");
                        break;
                    }

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");

                    string needPayValue = ItemUsePar.Split(';')[0];
                    string rosePayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    itemDropID = ItemUsePar.Split(';')[1];

                    if (float.Parse(rosePayValue) >= float.Parse(needPayValue))
                    {
                        //获取当前背包是否足够
                        if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum)) {
                            Game_PublicClassVar.Get_function_AI.DropIDToDropItem(itemDropID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                        }
                    }
                    else {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_395");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameHint("这是作者的感谢宝箱,赞助任意额度支持作者后可即可开启！");
                    }

                break;

                //荣誉
                case "10":
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum)) {
                        //获取当前一小时产出荣誉
                        string countryLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                        string rongYu_Hour = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HoureHonor", "ID", countryLv, "Country_Template");
                        //随机获得
                        float value = Random.value + 0.5f;
                        int addRongYuHourValue = (int)(int.Parse(rongYu_Hour) * value);
                        Game_PublicClassVar.Get_function_Country.AddCountryHonor(addRongYuHourValue, true);
                    }
                break;


                //繁荣度
                case "11":
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum)) {
                        //获取当前一小时产出繁荣度
                        string countryLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                        string fanRongDu_Hour = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HoureExp", "ID", countryLv, "Country_Template");
                        //随机获得
                        float value = Random.value + 0.5f;
                        int addFanRongDuHourValue = (int)(int.Parse(fanRongDu_Hour) * value);
                        Game_PublicClassVar.Get_function_Country.addCoutryExp(addFanRongDuHourValue, true);
                    }
                break;

                //BOSS冷却
                case "12":
                //判定当前是否在主城场景
                    if (Application.loadedLevelName == "EnterGame")
                    {
                        if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum)) {
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_1_DayNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_2_DayNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

                            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_319");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint("道具发挥了效果,所有BOSS已刷新!");
                        }
                    }
                    else {

                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_320");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint("使用此道具请移动到主城场景中使用！");
                    }
                break;

                //体力药水
                case "13":
                    //获取当前体力
                    if (Game_PublicClassVar.Get_function_Rose.GetRoseTili() >= 100) {

                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_321");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint("当前体力已满！");
                        break;
                    }
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum))
                    {
                        ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                        Game_PublicClassVar.Get_function_Rose.AddTili(int.Parse(ItemUsePar));
                    }
                break;

                //宠物召唤
                case "14":
                    //判定当前是否在建筑地图
                    if (Application.loadedLevelName == "EnterGame")
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_322");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameHint("请进入关卡地图在使用此道具");
                        break;
                    }
                break;

                //代币券
                case "15":

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    //判定自己背包内是否还有道具
                    int nowitemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum))
                    {
                        //增加额度
                        float rmbValue = float.Parse(ItemUsePar);
                        int zuanshiValue = int.Parse(ItemUsePar);
                        Game_PublicClassVar.Get_function_Rose.SendReward("2", (zuanshiValue * 100).ToString(),"48");
                        Game_PublicClassVar.Get_function_Rose.AddPayValue(rmbValue,"48");

                        if (nowitemBagNum > 0)
                        {
                            CloseUIStatus = false;
                        }
                    }
                    break;

                //制作书ID增加
                case "16":

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    //判定技能是否已经学习

                    if (Game_PublicClassVar.Get_function_Rose.GetMakeProficiencyIDStatus(ItemUsePar)) {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_45");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前制造物品已经学习!请勿重复学习");
                        break;
                    }

                    //增加
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum)) {
                        Game_PublicClassVar.Get_function_Rose.AddMakeProficiencyID(ItemUsePar);
                        string ItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(ItemName + " 已经学习成功!可以在制造系统中查看");
                    }
                break;

                //活力药水
                case "17":
                    //获取当前体力
                    if (Game_PublicClassVar.Get_function_Rose.GetRoseHuoLi() >= 100)
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_323");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint("当前活力已满！");
                        break;
                    }
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum)) {
                        ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                        Game_PublicClassVar.Get_function_Rose.AddHuoLi(int.Parse(ItemUsePar));
                    }

                break;

                //主角等级直升1级
                case "18":
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                    if (roseLv >= int.Parse(ItemUsePar))
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_324");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint("已超过使用等级!");
                        break;
                    }
                    else {
                        if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum)) {
                            int roseExpNow = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                            int Rose_Exp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseUpExp", "RoseLv", roseLv.ToString(), "RoseExp_Template"));
                            int add_Exp = Rose_Exp - roseExpNow;
                            if (add_Exp <= 0)
                            {
                                add_Exp = 0;
                            }
                            Game_PublicClassVar.Get_function_Rose.AddExp(add_Exp);
                        }
                    }

                break;

                //集齐道具触发一个掉落ID
                case "19":

                    if (Game_PublicClassVar.Get_function_Rose.BagNullNum() <= 0)
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_43");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("开始失败!请背包内至少预留5个位置!");
                        break;
                    }

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    string[] ItemUseParList = ItemUsePar.Split(';');
                    for (int i = 0; i < ItemUseParList.Length; i++){

                        int minLv = int.Parse(ItemUseParList[i].Split(',')[0]);
                        int maxLv = int.Parse(ItemUseParList[i].Split(',')[1]);
                        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= minLv) {
                            if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() <= maxLv)
                            {
                                needItemNum = ItemUseParList[i].Split(',')[2];
                                itemDropID = ItemUseParList[i].Split(',')[3];
                                hideID = ItemUseParList[i].Split(',')[4];
                                //获取背包道具是否足够
                                bagItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                                if (bagItemNum >= int.Parse(needItemNum))
                                {
                                    //获取当前背包是否足够
                                    if (Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, int.Parse(needItemNum))) {
                                        Debug.Log("触发掉落");
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem(itemDropID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position, hideID);
                                    }
                                }
                            }
                        }
                    }
                    break;

                //普通装备宝石卸载
                case "21":
                    //查看宝石镶嵌界面是否打开
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipGemHoleSet.activeSelf)
                    {
                        //销毁一个道具
                        if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum)) {
                            //卸载宝石
                            UI_EquipGemHoleSet ui_EquipGemHoleSet = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipGemHoleSet.GetComponent<UI_EquipGemHoleSet>();
                            ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                            Game_PublicClassVar.Get_function_UI.UnloadEquipGem(ItemUsePar, ui_EquipGemHoleSet.equipSpaceType, ui_EquipGemHoleSet.equipSpace, int.Parse(ui_EquipGemHoleSet.nowSelectGemHole));
                            ui_EquipGemHoleSet.UpdateEquipGemStatus = true;
                        }
                    }

                break;

                //宠灵露
                case "22":

                    //关闭背包,打开宠物洗炼界面
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseEquip_Status)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseEquip();
                    }

                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenPet();
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet != null)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().OpenType_ChuShi = "3";
                    }

                    break;

                //超级宠灵露
                case "23":

                    //关闭背包,打开宠物洗炼界面
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseEquip_Status)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseEquip();
                    }

                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenPet();
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet != null)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().OpenType_ChuShi = "3";
                    }

                    break;


                //宠物蛋，百分之5的概率出变异
                case "24":

                    //判断当前是否有宠物空位
                    int nullPetNum = Game_PublicClassVar.Get_function_AI.Pet_ReturnPetFirstNull();
                    if (nullPetNum == -1) {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_46");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前宠物栏位已满！宠物栏位每15、25、35、45、55级均会新增一个宠物栏位！");
                        break;
                    }
                    //销毁一个道具
                    //Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
                    //增加宠物
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    string petStr_PuTong = ItemUsePar.Split(';')[0];
                    string petStr_BianYi = ItemUsePar.Split(';')[1];
                    string[] petList_PuTong = petStr_PuTong.Split(',');
                    string[] petList_BianYi = petStr_BianYi.Split(',');

                    ObscuredFloat bianyiPro = 0.045f;
                    if (Random.value <= bianyiPro)
                    {
                        //触发变异
                        int bianyiNum = petList_BianYi.Length -1;
                        if (bianyiNum <= 0) {
                            bianyiNum = 0;
                        }
                        int nowNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0,bianyiNum);
                        string petID = petList_BianYi[nowNum];

                        //销魂自身道具
                        ObscuredInt costNum = 1;
                        if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, costNum, UIBagSpaceNum))
                        {
                            Game_PublicClassVar.Get_function_AI.Pet_Create(nullPetNum.ToString(), petID, "1");
                            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_47");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("运气爆表！恭喜你获得变异宠物!!!");
                        }
                    }
                    else {
                        //触发普通
                        int petNum = petList_PuTong.Length - 1;
                        if (petNum <= 0)
                        {
                            petNum = 0;
                        }
                        int nowNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, petNum);
                        string petID = petList_PuTong[nowNum];
                        //销魂自身道具
                        if (Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1)) {
                            Game_PublicClassVar.Get_function_AI.Pet_Create(nullPetNum.ToString(), petID, "1");
                            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_48");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你获得宠物!");
                        }
                    }
                break;

                //宠物经验丹（增加当前出战宠物的经验）
                case "25":

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
       
                    string petFightSpaceID = Game_PublicClassVar.Get_function_Rose.GetRosePetFightFirstID().ToString();
                    if (petFightSpaceID == "-1") {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_50");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前无出战宠物,无法使用！");
                        break;
                    }

                    int useExp = int.Parse(ItemUsePar.Split(',')[0]);
                    int usePetLv = int.Parse(ItemUsePar.Split(',')[1]);

                    //高于使用等级无法使用
                    int nowPetLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", petFightSpaceID, "RosePet"));
                    if (nowPetLv < usePetLv)
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_55");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物等级太底,无法使用！");
                        break;
                    }

                    //获取玩家等级
                    int nowRoseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                    if (nowPetLv >= nowRoseLv + 5) {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_56");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你的宠物等级高于自身角色5级,不可使用!");
                        break;
                    }

                    if (petFightSpaceID != "-1")
                    {
                        //销魂自身道具
                        if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum))
                        {
                            //添加经验
                            Game_PublicClassVar.Get_function_AI.Pet_AddExpOne(petFightSpaceID, useExp);
                            //判定自己背包内是否还有道具
                            int itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                            if (itemBagNum > 0)
                            {
                                CloseUIStatus = false;
                            }

                            string langStrHint_11 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_59");
                            string langStrHint_22 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_60");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_11 + useExp.ToString() + langStrHint_22);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你当前出战的宠物获得了" + useExp.ToString() + "点经验值");
                        }
                        else {
                            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_61");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包道具数量不足,请整理背包！");
                            break;
                        }
                    }
                    break;

                //宠物经验丹（增加当前出战宠物的指定等级）
                case "26":

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    petFightSpaceID = Game_PublicClassVar.Get_function_Rose.GetRosePetFightFirstID().ToString();
                    if (petFightSpaceID == "-1")
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_62");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前无出战宠物,无法使用！");
                        break;
                    }

                    useExp = int.Parse(ItemUsePar.Split(',')[0]);
                    usePetLv = int.Parse(ItemUsePar.Split(',')[1]);

                    //比主角大于5级不能获得经验
                    nowPetLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", petFightSpaceID, "RosePet"));
                    if (nowPetLv > usePetLv)
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_63");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物等级太高,不能使用！");
                        break;
                    }

                    //获取玩家等级
                    nowRoseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                    if (nowPetLv >= nowRoseLv + 5)
                    {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_64");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你的宠物等级高于自身角色5级,不可使用!");
                        break;
                    }

                    if (petFightSpaceID != "-1")
                    {
                        if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum)) {
                            //添加经验
                            Game_PublicClassVar.Get_function_AI.Pet_AddLvOne(petFightSpaceID, useExp);
                            //判定自己背包内是否还有道具
                            int itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                            if (itemBagNum > 0)
                            {
                                CloseUIStatus = false;
                            }

                            string langStrHint_11 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_59");
                            string langStrHint_22 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_60");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_11 + useExp.ToString() + langStrHint_22);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你当前出战的宠物获得了" + useExp.ToString() + "点经验值");
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你当前出战的宠物获得了" + useExp.ToString() + "点经验值");
                        }
                    }
                    break;

                //29 藏宝图
                case "29":

                    //string ItemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", ItemID, "Item_Template");
                    string ItemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", UIBagSpaceNum, "RoseBag");
                    Debug.Log("ItemPar = " + ItemPar);
                    string mapID = ItemPar.Split(',')[0];
                    float map_X = float.Parse(ItemPar.Split(',')[1]);
                    float map_Y = float.Parse(ItemPar.Split(',')[2]);

                    //比对当前地图ID
                    if (SceneManager.GetActiveScene().name != mapID) {
                        string scenceName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", mapID, "Scene_Template");
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_65");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + scenceName + " X=" + map_X + "map_Y = " + map_Y);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请移动到地图:" + scenceName + " X=" + map_X + "map_Y = " + map_Y);
                        break;
                    }

                    //获取坐标点
                    float rose_X = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.x;
                    float rose_Y = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.z;

                    Vector2 rose_vec2 = new Vector2(rose_X, rose_Y);
                    Vector2 item_vec2 = new Vector2(map_X, map_Y);
                    float dis = Vector2.Distance(rose_vec2, item_vec2);
                    ObscuredFloat disPro = 5.0f;
                    if (dis < disPro) {
                        ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                        //销魂自身道具
                        if (Game_PublicClassVar.Get_function_Rose.DeleteBagSpaceItem_Num(UIBagSpaceNum,1))
                        {

                            Game_PublicClassVar.Get_function_AI.DropIDToDropItem(ItemUsePar, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                        }
                    }
                    break;

                //28 仓库栏位开启
                case "30":
                    int stroeHouseMaxNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StroeHouseMaxNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                    //销魂自身道具
                    if(Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum)){
                        stroeHouseMaxNum = stroeHouseMaxNum + 1;
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("StroeHouseMaxNum", stroeHouseMaxNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                    }
                    break;

                //28 宠物栏位开启
                case "31":
                    int petAddMaxNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetAddMaxNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                    //销魂自身道具
                    if(Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum)){
                        petAddMaxNum = petAddMaxNum + 1;
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetAddMaxNum", petAddMaxNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                    }
                    //刷新一下宠物显示
                    Game_PublicClassVar.Get_function_AI.Pet_UpdateLvMaxNum();
                    break;


                //29 藏宝图(指定地图ID的藏宝图)
                case "32":
                    UI_FunctionOpen functionOpen = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>();
                    if (functionOpen.Obj_WabaoObj == null)
                    {
                        //string ItemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", ItemID, "Item_Template");
                        ItemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", UIBagSpaceNum, "RoseBag");
                        mapID = ItemPar;

                        //比对当前地图ID
                        Scene scene = SceneManager.GetActiveScene();
                        if (scene.name != mapID)
                        {
                            string scenceName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", mapID, "Scene_Template");

                            string langStrHint_11 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_66");
                            string langStrHint_22 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_67");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_11 + scenceName + langStrHint_22);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请移动到" + scenceName + "地图开启");
                            break;
                        }

                        ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                        functionOpen.Obj_WabaoObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().OBJ_WaBaoPro);
                        functionOpen.Obj_WabaoObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                        functionOpen.Obj_WabaoObj.transform.localScale = new Vector3(1, 1, 1);
                        //wabaoObj.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        //wabaoObj.transform.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                        functionOpen.Obj_WabaoObj.transform.localPosition = new Vector3(0, 0, 0);
                        functionOpen.Obj_WabaoObj.transform.localPosition = new Vector3(0, 0, 0);
                        functionOpen.Obj_WabaoObj.GetComponent<UI_WaBaoPro>().ItemID = ItemID;
                        functionOpen.Obj_WabaoObj.GetComponent<UI_WaBaoPro>().UIBagSpaceNum = UIBagSpaceNum;
                        functionOpen.Obj_WabaoObj.GetComponent<UI_WaBaoPro>().DropID_WaBao = ItemUsePar;

                        Destroy(this.gameObject);
                        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseEquip();
                    }
                    else {

                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_68");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("您已经开始挖宝了!");
                    }
 

                    /*
                    //销魂自身道具
					if (Game_PublicClassVar.Get_function_Rose.DeleteBagSpaceItem_Num(UIBagSpaceNum,1))
                    {
                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem(ItemUsePar, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                    }

                    //写入成就
                    if (ItemID == "10000016") {
                        //低级藏宝图
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("103", "0", "1");
                    }
                    if (ItemID == "10000017") {
                        //高级藏宝图
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("104", "0", "1");
                    }

                    //写入活跃任务
                    Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "131", "1");
                    */
                    break;

                //33 导标旗
                case "33":

                    ItemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", UIBagSpaceNum, "RoseBag");
                    Debug.Log("ItemPar = " + ItemPar);
                    GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                    //获取地图名称
                    string langStrHint_4 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_31");
                    string langStrHint_5 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_32");
                    string langStrHint_6 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_33");

                    string writeHintText = langStrHint_4;
                    //string writeHintText = "请点击保存需要传送的坐标！";
                    if (ItemPar != "") {
                        string moveMapID = ItemPar.Split(';')[0];
                        string mapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", moveMapID, "Scene_Template");
                        string[] posiList = ItemPar.Split(';')[1].Split(',');
                        Vector3 mapVec3 = new Vector3(float.Parse(posiList[0]), float.Parse(posiList[1]), float.Parse(posiList[2]));
                        writeHintText = langStrHint_5 + mapName + langStrHint_6 + mapVec3;
                        //writeHintText = "你当前定位的地区为：" + mapName + "\n 坐标点：" + mapVec3;
                    }
 
                    Debug.Log("uiCommonHint = " + uiCommonHint.name);
                    string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_1");
                    string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_2");
                    string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_3");
                    uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(writeHintText, Map_MoveTargetPosi, Map_SavePosi, langStrHint_1, langStrHint_2, langStrHint_3);
                    uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                    uiCommonHint.transform.localPosition = Vector3.zero;
                    uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

                    /*
                    //ItemPar = "10001;10,30,10";
                    if (ItemPar.Split(';').Length >= 2) {
                        string moveMapID = ItemPar.Split(';')[0];
                        string[] posiList = ItemPar.Split(';')[1].Split(',');
                        if (posiList.Length >= 3) {
                            Vector3 mapVec3 = new Vector3(float.Parse(posiList[0]), float.Parse(posiList[1]), float.Parse(posiList[2]));
                            //ItemUsePar = "1001";
                            //ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                            //Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);        //销魂自身道具
                            //写入场景
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";                     //设置角色为待机状态
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Status = false;           //设置角色不能移动

                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", moveMapID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = ItemUsePar;
                            Debug.Log("mapVec3 = " + mapVec3);
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().MapRosePositionVec3 = mapVec3;
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();
                        }
                    }
                    */
                    break;

                //34 称号使用
                case "34":
                    
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    Game_PublicClassVar.Get_function_Rose.ChengHao_Add(ItemUsePar);

                    //销魂自身道具
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum))
                    {
                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem(ItemUsePar, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                    }

                    break;

			    //35 获取当前boss的刷新状态
			    case "35":
				    string showStr = "";
				    string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
				    string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
				    if (deathMonsterIDListStr != "")
				    {
					    for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
					    {
						    string[] deathList = deathMonsterIDList[i].Split(',');
						    //1000101,589.9813,1189.981,70001901
						    string monsterOnlyID = deathList[0];
						    string monsterID = deathList [3];
						    string deathTime_zaixian = deathList[1];
						    string deathTime_lixian = deathList[2];

						    string monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("MonsterName", "ID", monsterID, "Monster_Template");
						    float time_ZaiXian = float.Parse (deathTime_zaixian);
						    float time_LiXian = float.Parse (deathTime_lixian);
						    int minTime_ZaiXian = (int)(time_ZaiXian / 60f);
						    int minTime_LiXian = (int)(time_LiXian / 60f);

						    string str = monsterName + " 在线刷新:约" + minTime_ZaiXian + "分钟" + " 离线刷新:约" + minTime_LiXian + "分钟";
						    //string str = monsterName + " 在线刷新:约" + minTime_ZaiXian + "分钟";
						    showStr = showStr + str + "\n";
					    }
				    }

				    GameObject commonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
				    commonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(showStr, null, null);
				    commonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
				    commonHint.transform.localPosition = Vector3.zero;
				    commonHint.transform.localScale = new Vector3(1, 1, 1);

				break;

                //爆率
                case "37":

                    //获取当前时间
                    if (System.DateTime.Now.Hour <= 0 && System.DateTime.Now.Minute < 40) {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请到0:40分后使用此道具");
                        return;
                    }

                    //获取当前时间
                    if (System.DateTime.Now.Hour >= 23)
                    {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请到0:40分后使用此道具");
                        return;
                    }

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");

                    //获取当前是否重置了
                    string rankBaoLvStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RankBaoLvStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    if (rankBaoLvStatus == "2")
                    {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("一天之内只能激活2次爆率道具!");
                    }
                    else {
                        //销魂自身道具
                        if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum))
                        {
                            //写入今日爆率
                            string RankBaoLvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RankBaoLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            if (RankBaoLvStr == "" || RankBaoLvStr == null) {
                                RankBaoLvStr = "0";
                            }

                            float writeBaoLv = float.Parse(RankBaoLvStr) + float.Parse(ItemUsePar);
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RankBaoLv", writeBaoLv.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_448");
                            float baolv = float.Parse(ItemUsePar) + 1;
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr + ":" + ((int)(float.Parse(ItemUsePar)*100)).ToString()+"%");
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuffListShowSet.GetComponent<UI_BuffShowListSet>().ShowBaoLvBuff();


                            if (rankBaoLvStatus == ""|| rankBaoLvStatus == null) {
                                rankBaoLvStatus = "0";
                            }
                            int writeNum = int.Parse(rankBaoLvStatus) + 1;
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RankBaoLvStatus", writeNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.function_DataSet.DataSet_SetXml("RoseDayReward");
                        }
                    }

                    break;

                //激活坐骑外观
                case "38":

                    //判断当前是否激活坐骑系统
                    
                    string nowZuoQiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                    if (nowZuoQiLv == "" || nowZuoQiLv == null || nowZuoQiLv == "0")
                    {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先到家园里激活坐骑系统后,再激活此坐骑进行使用");

                        return;
                    }
                    

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");

                    //销魂自身道具
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum))
                    {
                        //写入外观
                        Game_PublicClassVar.Get_function_Pasture.ZuoQi_AddPiFu(ItemUsePar);
                    }

                    break;


                //增加坐骑资质
                case "51":
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");

                    string nowZuoQiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                    if (nowZuoQiID != "" && nowZuoQiID != "0")
                    {
                        //销魂自身道具
                        if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum))
                        {
                            //写入外观
                            Game_PublicClassVar.Get_function_Pasture.AddZuoQiZiZhi();
                        }
                    }
                    else {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先激活坐骑！");
                    }

     
                    break;

                //增加坐骑能力
                case "52":
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");

                    //销魂自身道具
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum))
                    {
                        //写入增加能力值
                        Game_PublicClassVar.Get_function_Pasture.ZuoQiNengLiAddExp("0",int.Parse(ItemUsePar));
                    }
                    break;

                //增加坐骑献祭经验
                case "53":
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");

                    //销魂自身道具
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum))
                    {
                        //写入外观
                        Game_PublicClassVar.Get_function_Pasture.ZuoQiAddXianJiExp(int.Parse(ItemUsePar));

                        //献祭经验超过最大值,表示献祭成功
                        string zuoQiExpStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiXianJiExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                        if (zuoQiExpStr == "" || zuoQiExpStr == null)
                        {
                            zuoQiExpStr = "0";
                        }

                        int zuoQiExp = int.Parse(zuoQiExpStr);
                        int maxXianJiValue = 1000;
                        if (zuoQiExp >= maxXianJiValue)
                        {
                            Game_PublicClassVar.Get_function_Pasture.CreateZuoQi();
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你激活坐骑成功!");
                        }

                    }
                    break;


                //激活外观
                case "61":

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");

                    //销魂自身道具
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum))
                    {
                        //写入外观
                        string[] ranseList = ItemUsePar.Split(';');
                        for (int i = 0; i < ranseList.Length; i++) {
                            Game_PublicClassVar.Get_function_Rose.RoseYanSeAdd(ranseList[i]);
                        }
                    }

                    break;

                //激活装备隐藏属性
                case "62":

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");

                    string[] parList = ItemUsePar.Split(';');
                    string HideIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", parList[0], "RoseEquip");
                    if (HideIDStr == "" || HideIDStr == null || HideIDStr == "0") {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前装备没有隐藏属性,请先洗炼出隐藏属性在使用此卷轴");
                        return;
                    }

                    //销魂自身道具
                    if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(ItemID, 1, UIBagSpaceNum))
                    {

                        string hideProperListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", HideIDStr, "RoseEquipHideProperty");

                        if (hideProperListStr != ""&& hideProperListStr != "0")
                        {
                            hideProperListStr = hideProperListStr + ";10001,"+ parList[1];
                        }
                        else {
                            hideProperListStr = "10001,"+ parList[1];
                        }

                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PrepeotyList", hideProperListStr, "ID", HideIDStr, "RoseEquipHideProperty");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquipHideProperty");
                    }

                    break;
            }
        }

        //点击镶嵌
        if (itemType == "4") {
            
            Debug.Log("我点击了镶嵌");

            //判定当前是否已经打开界面
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipGemHoleSet.activeSelf)
            {

                //获取当前选中镶嵌的位置
                UI_EquipGemHoleSet ui_EquipGemHoleSet = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipGemHoleSet.GetComponent<UI_EquipGemHoleSet>();
                //镶嵌操作
                if (Game_PublicClassVar.Get_function_UI.AddEquipGem(UIBagSpaceNum, ui_EquipGemHoleSet.equipSpaceType, ui_EquipGemHoleSet.equipSpace, int.Parse(ui_EquipGemHoleSet.nowSelectGemHole)))
                {
                    //刷新槽位显示
                    ui_EquipGemHoleSet.UpdateEquipGemStatus = true;
                    //更新背包立即显示（后期优化可改为更新某一个格子的数量显示）
                    Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
                }
                else {
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宝石镶嵌不成功！");
                }
            }
            else {
                //打开界面
                if (!Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.activeSelf)
                {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseEquip();
                }
                //打开镶嵌界面
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Click_Type("3");

                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_81");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("界面已打开,请选择对应颜色的槽位进行宝石镶嵌");
            }

        }

        //技能道具打开对应界面
        if (itemType == "5")
        {
            switch (itemSubType) {
                //技能道具
                case "22":

                    //关闭背包,打开宠物洗炼界面
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseEquip_Status)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseEquip();
                    }

                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenPet();
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet != null)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().OpenType_ChuShi = "3";
                    }

                    break;
            }

        }


        //取消使用状态
        useItemStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.UseItemStatus = false;

        //关闭界面
        if (CloseUIStatus) {
            //更新背包立即显示(连续点击过快容易出现假物品)
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            CloseUI();
        }
    }

    //丢弃道具按钮
    public void ThrowItem()
    {
        if (clickBtnStatus)
        {
            return;
        }

        clickBtnStatus = true;  //防止因为卡顿二次执行


        //弹出提示,出售牧场材料
        string ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");
        if (itemType == "6"&& ItemUsePar=="1")
        {

            if (selPastureItemUICommonHint != null)
            {
                Destroy(selPastureItemUICommonHint);
            }

            int sellGoldValue = Game_PublicClassVar.Get_function_Pasture.GetPastureSellGold(UIBagSpaceNum);

            selPastureItemUICommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
            string jieshaoStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_41") + sellGoldValue;
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_13");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_14");
            string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_15");
            selPastureItemUICommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, SellPastureItem, null, langStrHint_1, langStrHint_2, langStrHint_3);
            selPastureItemUICommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            selPastureItemUICommonHint.transform.localPosition = Vector3.zero;
            selPastureItemUICommonHint.transform.localScale = new Vector3(1, 1, 1);

            Destroy(this.gameObject);

            return;

        }


        //品质>=5的的时候弹出提示
        if (int.Parse(itemQuality) >= 4)
        {
            this.gameObject.SetActive(false);
            //关闭UI背景图片
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);
            //弹出提示
            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
            string jieshaoStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_26") + Text_ItemName;
            //string jieshaoStr = "是否出售道具:"+ Text_ItemName;
            //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, throwItem, null, "出售确认", "出售", "取消");
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_13");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_14");
            string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_15");
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, throwItem, null, langStrHint_1, langStrHint_2, langStrHint_3);
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            throwItem();
        }

    }


    public void SellPastureItem() {
        Game_PublicClassVar.Get_function_Pasture.SellPastureBagSpaceItemToMoney(UIBagSpaceNum);
    }


    //丢弃道具执行
    public void throwItem() {

        if (ItemID == "10010076")
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("此道具无法出售和丢弃");
            return;
        }

        //如果打开回收界面 出售要即时显示
        if (Game_PublicClassVar.Get_game_PositionVar.HuiShouItemStatus)
        {
            string[] huiShouBagNumList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_HuiShouItem.GetComponent<UI_HuiShouItem>().HuiShouBagNumList;
            for (int i = 0; i < huiShouBagNumList.Length; i++)
            {
                if (huiShouBagNumList[i] == UIBagSpaceNum)
                {
                    //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().Obj_HuiShouItem.GetComponent<UI_HuiShouItem> ().HuiShouItemListShowClearn(i);
                    Game_PublicClassVar.Get_function_UI.HuiShouItem_Cancle(UIBagSpaceNum);
                    //Game_PublicClassVar.Get_function_UI.HuiShouItem_Cancle(i.ToString());
                }
            }
        }

        //获取当前道具数量
        string bagItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", UIBagSpaceNum, "RoseBag");
        if (int.Parse(bagItemNum) <= 1)
        {
            //Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(ItemID, 1, UIBagSpaceNum, true);
            Game_PublicClassVar.Get_function_Rose.SellBagSpaceItemToMoney(UIBagSpaceNum);
            CloseUI();
        }
        else
        {
            GameObject throwItemChoice = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIThrowItemChoice);
            //throwItemChoice.transform.SetParent(this.gameObject.transform);
            throwItemChoice.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet.transform);
            throwItemChoice.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);        //在中心,后期分辨率不一样可能需要调整
            throwItemChoice.transform.localScale = new Vector3(1, 1, 1);
            throwItemChoice.GetComponent<UI_ThrowItemChoice>().BagSpaceNum = UIBagSpaceNum;
            throwItemChoice.GetComponent<UI_ThrowItemChoice>().ItemID = ItemID;
        }

        clickBtnStatus = false;  //防止因为卡顿二次执行
    }


    //存入仓库
    public void Btn_SaveStoreHouse()
    {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        //获取仓库是否已经满了
        int nullNum = Game_PublicClassVar.Get_function_Rose.StoreHouseNullNum();
        if (nullNum <= 0) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_82");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("仓库已满！");
            return;
        }

        //获取存入道具的数据
        //string save_ItemID = ItemID;
        //string save_ItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", UIBagSpaceNum, "RoseBag");
        //string save_ItemHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", UIBagSpaceNum, "RoseBag");
        //删除指定道具
        //Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(save_ItemID, int.Parse(save_ItemNum), UIBagSpaceNum, true);
        //添加指定道具到仓库
        //Game_PublicClassVar.Get_function_Rose.SendRewardToStoreHouse(save_ItemID, int.Parse(save_ItemNum), "1");

        if (Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu == 0)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_83");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("仓库未打开！");
            clickBtnStatus = false;
            return;
        }

        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial = "1";
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial = UIBagSpaceNum;

        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "3";
        string nullspace = Game_PublicClassVar.Get_function_Rose.StoreHouse_ReturnNullSpaceNum(Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu.ToString());
        if (nullspace == "-1")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_82");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("仓库已满!");
            clickBtnStatus = false;
            return;
        }
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = nullspace;

        //执行交换
        Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();


        //更新显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        Destroy(this.gameObject);
        //关闭UI背景图片
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);

        clickBtnStatus = false;  //防止因为卡顿二次执行
    }

    //取出仓库
    public void Btn_TaskStoreHouse()
    {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        int nullNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
        if (nullNum <= 0)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_84");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("背包已满！");
            return;
        }
        //获取存入道具的数据
        //string save_ItemID = ItemID;
        //string save_ItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", UIBagSpaceNum, "RoseStoreHouse");
        //string save_ItemHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", UIBagSpaceNum, "RoseBag");
        //删除指定道具
        //Game_PublicClassVar.Get_function_Rose.CostStoreHouseSpaceNumItem(save_ItemID, int.Parse(save_ItemNum), UIBagSpaceNum, true);
        //添加指定道具到背包
        //Game_PublicClassVar.Get_function_Rose.SendRewardToBag(save_ItemID, int.Parse(save_ItemNum), "1");

        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial = "3";
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial = UIBagSpaceNum;

        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "1";
        string nullspace = Game_PublicClassVar.Get_function_Rose.BagFirstNullNum();
        if (nullspace == "-1")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_82");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("仓库已满!");
            clickBtnStatus = false;
            return;
        }
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = nullspace;

        //执行交换
        //Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("执行道具开始!");

        Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();


        //更新显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        Destroy(this.gameObject);
        //关闭UI背景图片
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);

        clickBtnStatus = false;  //防止因为卡顿二次执行
    }

    //点击卸下宝石按钮
    public void Btn_XieXiaGem() {
        //查看宝石镶嵌界面是否打开
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipGemHoleSet.activeSelf)
        {

            string itemsubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");
            if (itemsubType == "110") {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("多彩宝石无法卸载!");
                return;
            }
            //销毁一个道具
            //Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
            //卸载宝石
            UI_EquipGemHoleSet ui_EquipGemHoleSet = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipGemHoleSet.GetComponent<UI_EquipGemHoleSet>();
            string ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");
            if (Game_PublicClassVar.Get_function_UI.UnloadEquipGem(ItemUsePar, ui_EquipGemHoleSet.equipSpaceType, ui_EquipGemHoleSet.equipSpace, int.Parse(ui_EquipGemHoleSet.nowSelectGemHole))) {
                ui_EquipGemHoleSet.UpdateEquipGemStatus = true;
                Destroy(this.gameObject);
            }
        }
    }

    //记录坐标
    public void Map_SavePosi()
    {
        Vector3 selfVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        string saveMapID = SceneManager.GetActiveScene().name;

        if (saveMapID != "EnterGame")
        {
            string ItemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", UIBagSpaceNum, "RoseBag");
            string writeItemPar = "";
            string timeStr = "";
            if (ItemPar == "")
            {
                //如果当前没有被记录
                timeStr = "0";
            }
            else
            {
                timeStr = ItemPar.Split(';')[2];
            }

            writeItemPar = saveMapID + ";" + selfVec3.x + "," + selfVec3.y + 2 + "," + selfVec3.z + ";" + timeStr;
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("保存坐标成功！");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", writeItemPar, "ID", UIBagSpaceNum, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_88");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("不能在城镇地图保存坐标！");
        }
    }

    //移动地图指定坐标
    public void Map_MoveTargetPosi()
    {
        string ItemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", UIBagSpaceNum, "RoseBag");
        if (ItemPar.Split(';').Length >= 2)
        {

            //判断是否存在冷却CD
            string itemIDTime = ItemPar.Split(';')[2];
            if (itemIDTime == "") {
                itemIDTime = "0";
            }
            string timeStr = WWWSet.GetTimeStamp();
            int chaValue = int.Parse(timeStr) - int.Parse(itemIDTime);
            if (chaValue < 1800)
            {
                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_91");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_92");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + (int)(1800 - chaValue) / 60 + langStrHint_2);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("半小时内只能使用一次! 冷却时间剩余:" + (int)(1800 - chaValue) / 60 + "分钟");
                return;
            }

            string moveMapID = ItemPar.Split(';')[0];
            string[] posiList = ItemPar.Split(';')[1].Split(',');
            if (posiList.Length >= 3)
            {
                Vector3 mapVec3 = new Vector3(float.Parse(posiList[0]), float.Parse(posiList[1]), float.Parse(posiList[2]));
                //写入场景
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";                     //设置角色为待机状态
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Status = false;           //设置角色不能移动

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", moveMapID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = ItemUsePar;
                //Debug.Log("mapVec3 = " + mapVec3);
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().MapRosePositionVec3 = mapVec3;
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();

                //修改使用时间
                string writeStr = ItemPar.Split(';')[0] + ";" + ItemPar.Split(';')[1] + ";" + timeStr;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", writeStr, "ID", UIBagSpaceNum, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
            }
        }
        else {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_93");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先保存需要传送的坐标点！");
        }
    }



    //点击回收按钮
    public void Btn_HuiShou(){
		Game_PublicClassVar.Get_function_UI.HuiShouItem_Add (UIBagSpaceNum);
		Destroy(this.gameObject);
		Game_PublicClassVar.Get_function_UI.DestoryTipsUI();    //删除所有Tips
	}

	//点击取消回收按钮
	public void Btn_HuiShouCancle(){
		Game_PublicClassVar.Get_function_UI.HuiShouItem_Cancle(UIBagSpaceNum);
		Destroy(this.gameObject);
		Game_PublicClassVar.Get_function_UI.DestoryTipsUI();    //删除所有Tips
	}


    public void CloseUI() {
        Destroy(this.gameObject);
        //关闭UI背景图片
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);
    }
}
