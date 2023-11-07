using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ItmeMakeLearn : MonoBehaviour
{
    public GameObject Obj_EquipMakeSet;
	//制作列表相关
	public GameObject Obj_ItemList;
	public GameObject Obj_ItemListSet;
	public string nowSeclect;
    private string nowSelcectNext;
	public string makeItemStr;
	public GameObject Btn_Lianjin;
	public GameObject Btn_CaiYao;
	public GameObject Btn_DaZao;
    public string NpcID;
    public GameObject Obj_LearnGoldValue;

    //制造书
    public string ItemID;
    private string makeEquipID;                     //制作书ID
    public GameObject Obj_MakeItemNameTitle;        //制作书标题
    public GameObject Obj_MakeItemLv;               //制造书等级
    public GameObject Obj_MakeItemStar;             //制造书星级
    public GameObject Obj_MakeItemNum;              //制造书等级
    public GameObject Obj_MakeItemName;             //制造书名称
    public GameObject Obj_MakeItemQuality;          //制造道具品质显示
    public GameObject Obj_MakeItemIcon;             //制造道具图标显示
    //public GameObject Obj_MakeSuccesPro;
    public GameObject Obj_MakeNeedGold;
    public GameObject Obj_MakeNeedItem;             //制造需求的道具源Obj
    public GameObject Obj_MakeEquipNeedItemSet;
    //public GameObject Obj_MakeProficiency;      
    private string makeItemID;                      //制造道具的ID
    private string makeItemName;
    private int makeItemLv;
    private int makeEquipNum;
    private float makeSuccessPro;
    private int makeNeedGold;                 	    //制造道具需要金币
    private GameObject obj_ItemTips;         	    //制造道具的Tips
    private bool ClickMakeBtn;              	    //制造按钮,防止在卡顿的时候多次执行制作操作

    //获取当前的制造点数
    private string proficiencyType;
    private int proficiencymax;
    private string proficiencyValue;
    private int nowficiencyValue;
    private int needProficiencyValue;
	public bool UpdateShowStatus;
	public GameObject makeItemlasterObj;

    //显示空
    public GameObject NullShowSetObj;
    public GameObject Obj_LearnItemShowObj;

	// Use this for initialization
	void Start () {
		//Debug.Log ("ObjCommonHuoBiSetPosiAAAAAAAAAA = " + ObjCommonHuoBiSetPosi.name);
		//makeItemStr = "10001,10002,10003,10004,10005";
		//Debug.Log ("makeItemStr = " + makeItemStr);
		//更新宠物标题
		//Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject, "101");
        
        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_EquipMakeSet);

        //默认打开第一个类型
        Click_Type("1");

        //Debug.Log("makeItemStr = " + makeItemStr);
        //判定是否为空
        if (makeItemStr == "" || makeItemStr == null)
        {
            //表示当前没有可学习的东西
            NullShowSetObj.SetActive(true);
            Obj_LearnItemShowObj.SetActive(false);
        }
        else
        {
            NullShowSetObj.SetActive(false);
            Obj_LearnItemShowObj.SetActive(true);
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
				go.GetComponent<UI_ItemMakeLearnListObj>().UpdateStatus = true;
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
        if (makeItemLv <= 1)
        {
            Obj_MakeItemLv.GetComponent<Text>().text = "制造等级：无限制";
        }
        else {
            Obj_MakeItemLv.GetComponent<Text>().text = "制造等级：" + makeItemLv;
        }
        //Obj_MakeItemStar.GetComponent<Text>().text = "制造数量：" + makeEquipNum;


        //最低显示1%成功概率
        int showMakeSuccessPro = (int)(makeSuccessPro * 100);
        if (showMakeSuccessPro <= 0) {
            showMakeSuccessPro = 1;
        }

        Obj_MakeItemNum.GetComponent<Text>().text = "制造数量：" + makeEquipNum + "个";
        //Obj_MakeSuccesPro.GetComponent<Text>().text = "成功概率：" + showMakeSuccessPro + "%";
        Obj_MakeItemStar.GetComponent<Text>().text = "制造难度：" + makeStar + "星";
        Obj_MakeNeedGold.GetComponent<Text>().text = "消耗金币：" + makeNeedGold;
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

        //显示学习消耗金币
        int learnGoldValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnGoldValue", "ID", makeEquipID, "EquipMake_Template"));
        Obj_LearnGoldValue.GetComponent<Text>().text = learnGoldValue.ToString();
        Obj_LearnGoldValue.GetComponent<Text>().color = new Color(0.33f,0.23f,0.13f);

        //检测金币是否足够
        if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() < learnGoldValue)
        {
            Obj_LearnGoldValue.GetComponent<Text>().color = Color.red;
            Obj_LearnGoldValue.GetComponent<Text>().text = learnGoldValue +"(金币不足)";
        }
        
        //updataNeedItem();   //更新需求道具
    }

    //更新需求道具
    /*
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
    */
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

        //检测自身是否已经学习
        if (Game_PublicClassVar.Get_function_Rose.GetMakeProficiencyIDStatus(nowSeclect)) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_13");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前制造技能你已经学习！");
            return;
        }


        //检测等级是否达到
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        int learnLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnLv", "ID", nowSeclect, "EquipMake_Template"));
        int learnGoldValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnGoldValue", "ID", nowSeclect, "EquipMake_Template"));
        if (roseLv < learnLv)
        {
            makeStatus = false;
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_14");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("角色等级不足！");
            ClickMakeBtn = false;
            return;
        }

        //检测金币是否足够
        if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() < learnGoldValue)
        {
            //金币不足
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_15");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("学习需要的金币不足!");
            makeStatus = false;
            ClickMakeBtn = false;
            return;
        }

        //制造成功
        if (makeStatus)
        {
            string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeItemID", "ID", nowSeclect, "EquipMake_Template");
            //扣除金币
            Game_PublicClassVar.Get_function_Rose.CostReward("1", learnGoldValue.ToString());
            //添加学习ID
            Game_PublicClassVar.Get_function_Rose.AddMakeProficiencyID(nowSeclect);
            //默认选中下一个作为选中
            string[] makeItemList = makeItemStr.Split (';');
	        //显示宠物列表
            for (int i = 0; i < makeItemList.Length; i++)
            {
                int listNum = makeItemList.Length - i - 1;
                if (makeItemList[listNum] == nowSeclect)
                {
                    int nextList = listNum - 1;
                    if (nextList >= 0)
                    {
                        nowSeclect = makeItemList[nextList];
                        //Debug.Log("123nowSeclect = " + nowSeclect);
                    }
                    else {
                        nowSeclect = makeItemList[makeItemList.Length - 1];
                        //Debug.Log("456nowSeclect = " + nowSeclect);
                    }
                    break;
                }
            }

            //制造成功刷新下界面显示
            string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", itemID, "Item_Template");

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_18");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + itemName);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你!学会制作道具:" + itemName);
            updateMake();

        }
        else {
            //Game_PublicClassVar.Get_function_UI.GameHint("制造材料不足");
        }

        //制造完毕,防止卡顿时多次制造道具
        ClickMakeBtn = false;
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

		//Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().OpenMakeItem();

        Destroy(this.gameObject);
    }


	//展示制造列表
	void showPetList() {

        if (makeItemStr == "" || makeItemStr == "0") {
            return;
        }

		string[] makeItemList = makeItemStr.Split (';');

		//清空列表
		Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ItemListSet);

		//显示宠物列表
		for (int i = 0; i < makeItemList.Length; i++)
		{
			//string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", i.ToString(), "RosePet");
            int listNum = makeItemList.Length - i - 1;
            //int listNum = i;
            if (makeItemList[listNum] != "0" && makeItemList[listNum] != "")
            {
                //实例化一个宠物列表控件
                GameObject petListObj = (GameObject)Instantiate(Obj_ItemList);
                petListObj.transform.SetParent(Obj_ItemListSet.transform);
                petListObj.transform.localScale = new Vector3(1, 1, 1);

                petListObj.GetComponent<UI_ItemMakeLearnListObj>().PetOnlyID = makeItemList[listNum];
                petListObj.GetComponent<UI_ItemMakeLearnListObj>().UpdateStatus = true;
                petListObj.GetComponent<UI_ItemMakeLearnListObj>().Obj_FuJiObj = this.gameObject;

                if(nowSeclect==""){
                    nowSeclect = makeItemList[listNum];
                }
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

        /*
		string[] makeItemList = makeItemStr.Split(';');
        int listNum = makeItemList.Length - 1;
        if(listNum < 0){
            listNum = 0;
        }
        nowSeclect = makeItemList[listNum];
        */

        string[] makeItemList = makeItemStr.Split (';');
        string makeItemStrValue = "";
		//清空列表
		Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ItemListSet);

		//显示列表
        for (int i = 0; i < makeItemList.Length; i++)
        {
            //倒序排列
            //int listNum = makeItemList.Length - i - 1;
            int listNum = i;
            bool ifShowStatus = true;

            //判定等级是否满足
            int learnLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnLv", "ID", makeItemList[listNum], "EquipMake_Template"));
            if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < learnLv)
            {
                ifShowStatus = false;
            }

            if (ifShowStatus)
            {
                //判定是否已经拥有
                for (int y = 1; y <= 4; y++)
                {
                    string proficiencyID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_" + y, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");
                    if (proficiencyID != "" || proficiencyID != "0")
                    {
                        string[] proficiencyIDList = proficiencyID.Split(',');
                        for (int z = 0; z < proficiencyIDList.Length; z++)
                        {
                            if (proficiencyIDList[z] == makeItemList[listNum])
                            {
                                //Debug.Log("屏蔽屏蔽屏蔽makeItemList[i] = " + makeItemList[i]);
                                ifShowStatus = false;
                            }
                        }
                    }
                }
            }

            //ifShowStatus = false;
            if (ifShowStatus)
            {
                if (makeItemStrValue != "")
                {
                    makeItemStrValue = makeItemStrValue + ";" + makeItemList[listNum];
                }
                else {
                    makeItemStrValue = makeItemList[listNum];
                }
            }
        }

        makeItemStr = makeItemStrValue;

        //更新数据
		showPetList();
		GetMakeData();

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

                //makeItemStr = "10001,10002,10003,10004,10005";
                makeItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShopValue", "ID", NpcID, "Npc_Template");
                //Debug.Log("makeItemStr = " + makeItemStr);

			    break;
                /*
                //裁缝
                case "2":
                    //Obj_Pet_HeChengSet.SetActive(true);

                    //显示底图
                    obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                    img = obj as Sprite;
                    Btn_CaiYao.GetComponent<Image>().sprite = img;

                    //makeItemStr = "10006,10007,10008,10009";
                    makeItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_" + "type", "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");
                    break;

                        //打造
                case "3":

                    //显示底图
                    obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                    img = obj as Sprite;
                    Btn_DaZao.GetComponent<Image>().sprite = img;

                    //makeItemStr = "10002,10002,10003,10004,10005,10006,10007,10008,10009,10001,10002,10003,10004,10005,10006,10007,10008,10009,10001,10002,10003,10004,10005,10006,10007,10008,10009,10001,10002,10003,10004,10005,10006,10007,10008";
                    makeItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_" + "type", "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");

                    break;
            */
        }
        
		updateMake();
	}
}
