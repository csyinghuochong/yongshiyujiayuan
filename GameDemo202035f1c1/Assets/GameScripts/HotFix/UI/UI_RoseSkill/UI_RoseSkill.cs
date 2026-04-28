using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_RoseSkill : MonoBehaviour {

	//public string NpcID;					    //NpcID
    public GameObject Obj_UIRoseSkillSet;       //角色集合
	public GameObject Obj_SkillList;		    //商品的Obj
	public GameObject Obj_SkillListNum;		    //商品页数的Obj
	public Transform Tra_SkillListSet;
	private string SkillItemListText;		//商品ID的整个Text
    private string AddSkillItemListText;	//商品ID的整个Text,有装备给此技能+等级的
	private string[] SkillItemList;			//商品ID数组
    private string[] AddSkillItemList;		//商品ID数组
	private float SkillListPosition_X;		//商品显示的X坐标
	private float SkillListPosition_Y;		//商品显示的Y坐标
	private int nowSkillListNum;			//当前商品页数
	private int sumSkillListNum;			//商品总页数
    public GameObject Obj_SpValue;          //SP的值

    public GameObject Obj_EquipBtnText_1;
    public GameObject Obj_EquipBtnText_2;
    public GameObject Obj_EquipBtnText_3;

    public GameObject Obj_BagSpace;                 //动态创建的背包格子
    public GameObject Obj_BagSpaceListSet;
    public GameObject Obj_RoseSkillSet;
    public GameObject Obj_RoseItemSkillSet;

    public GameObject Obj_FunctionBtn_1;
    public GameObject Obj_FunctionBtn_2;
    public GameObject Obj_FunctionBtn_3;

    public GameObject Obj_SkillListSet;
    public GameObject Obj_SkillTianFuSet;
    public GameObject Obj_JueXingSet;
    public GameObject[] Obj_SkillIconShowPosition;

    public GameObject Obj_SkillIconShowObj;
    public GameObject Obj_SkillIconShowSet;

    public GameObject Obj_SkillBtn_ZhuDong;
    public GameObject Obj_SkillBtn_Bag;

	private int creatNum;					    //单页里面存在几个技能

    public GameObject Obj_YinDao_SkillItemBtn;
    public GameObject Obj_YinDao_SkillItemUse;

    public GameObject Obj_Btn_JueXing;          //觉醒
	
	// Use this for initialization
	void Start () {

        //显示通用UI
        Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject,"401");

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_UIRoseSkillSet);

		creatNum = 999;
        UpdataSkillData();  //更新技能数据

		//创建第1页显示商品
		nowSkillListNum = 1;
		createSkillList(nowSkillListNum);
		//显示商品页数
		if (SkillItemList.Length <= creatNum) {
			sumSkillListNum = (int)SkillItemList.Length / creatNum;
		} else {
			sumSkillListNum = (int)SkillItemList.Length / creatNum+1;
		}
		
		Obj_SkillListNum.GetComponent<Text> ().text = "1/" + sumSkillListNum.ToString();

        //在引导完装备技能后,引导背包道具使用
        Obj_YinDao_SkillItemBtn.SetActive(false);
        Obj_YinDao_SkillItemUse.SetActive(false);

        //判定当前技能是否已经装备
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv >= 6 && roseLv <= 7)
        {
            ShowYinDao_SkillItemBtn();
        }

        //初始化标签按钮
        if (Game_PublicClassVar.Get_wwwSet.GameSetLanguage._Language != "Chinese")
        {
            Obj_EquipBtnText_1.GetComponent<Text>().fontSize = 20;
            Obj_EquipBtnText_2.GetComponent<Text>().fontSize = 20;
        }

        //觉醒按钮隐藏
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < 60)
        {
            Obj_Btn_JueXing.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update () {

	}

    public void ShowYinDao_SkillItemBtn() {
        if (Game_PublicClassVar.Get_function_Skill.IfEquipMainSkill("60010201"))
        {
            if (!Game_PublicClassVar.Get_function_Skill.IfEquipMainSkill("10010001") && Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10010001") >= 1)
            {
                Obj_YinDao_SkillItemBtn.SetActive(true);
            }
        }
    }

    private void ShowYinDao_SkillItemUse() {
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv >= 6 && roseLv <= 7)
        {
            Obj_YinDao_SkillItemUse.SetActive(true);
        }
    }

	//循环创建对应页数的子商品
	void createSkillList(int storeNum){

        UpdataSkillData();  //更新技能数据
        //显示当前SP值
        string spvalue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillSP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("当前技能点数");
        Obj_SpValue.GetComponent<Text>().text = langStr + "：" + spvalue;

		//循环删除子控件
		for(int i = 0;i<Tra_SkillListSet.transform.childCount;i++)
		{
			GameObject go = Tra_SkillListSet.transform.GetChild(i).gameObject;
			Destroy(go);
		}
		
		int rowSum = 0;
		int createSum = 0;
		SkillListPosition_X = -180.0f;
		SkillListPosition_Y = 220.0f;

		//循环创建
        if(SkillItemList.Length>=9){
            Tra_SkillListSet.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 200 * SkillItemList.Length);
        }


		for(int i = 0; i<SkillItemList.Length;i++){

            //Debug.Log("SkillItemList[i] = " + SkillItemList[i]);
			if(SkillItemList[i]!=""){
                //rowSum = rowSum +1;

                string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", SkillItemList[i], "Skill_Template");
                if (skillType != "3"&& skillType != "5" && SkillItemList[i].Contains("600200")==false) {
                    //实例化技能栏
                    GameObject obj_skillList = (GameObject)MonoBehaviour.Instantiate(Obj_SkillList);
                    obj_skillList.transform.SetParent(Tra_SkillListSet);
                    //obj_skillList.transform.localPosition = new Vector3(SkillListPosition_X, SkillListPosition_Y, 0);
                    obj_skillList.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    obj_skillList.GetComponent<UI_RoseSkillList>().SkillID = SkillItemList[i];
                    obj_skillList.GetComponent<UI_RoseSkillList>().SkillAddID = AddSkillItemList[i];
                    obj_skillList.GetComponent<UI_RoseSkillList>().Obj_SkillParent = this.gameObject;
                }
			}
			
			//循环创建12个，满足12个立即跳出当前循环
            /*
			createSum = createSum + 1;
			if(createSum>=creatNum){
				i = SkillItemList.Length;	//立即跳出循环
            }
            */
		}

        //显示快捷
        for (int i = 1; i <= 10; i++) {
            SkillIconShow(i.ToString());
        }
	}
	
	//显示上一页
	public void SkillList_Up(){
		if (nowSkillListNum > 1) {
			nowSkillListNum = nowSkillListNum - 1;
			createSkillList(nowSkillListNum);
			Obj_SkillListNum.GetComponent<Text> ().text = nowSkillListNum + "/" + sumSkillListNum.ToString();
		}
	}
	
	//显示下一页
	public void SkillList_Down(){
		
		if (nowSkillListNum < sumSkillListNum){
			nowSkillListNum = nowSkillListNum + 1;
			createSkillList(nowSkillListNum);
			Obj_SkillListNum.GetComponent<Text> ().text = nowSkillListNum + "/" + sumSkillListNum.ToString();
		}
	}

    //更新技能数据
    public void UpdataSkillData() {

        //获取本身技能和装备技能合并
        SkillItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        AddSkillItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //获取技能
        string addList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (addList != "")
        {
            SkillItemListText = SkillItemListText + "," + addList;
            AddSkillItemListText = AddSkillItemListText + "," + addList;
        }

        //获取当前携带的精灵技能
        string addJingLingList = "";
        string JingLingIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JingLingEquipID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] jingLingIDList = JingLingIDSet.Split(';');
        for (int i = 0; i < jingLingIDList.Length; i++)
        {
            string jingLingSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkill", "ID", jingLingIDList[i], "Spirit_Template");
            if (addJingLingList!=""& jingLingSkillStr != "" && jingLingSkillStr != "0")
            {
                addJingLingList = addJingLingList + "," + jingLingSkillStr;
            }
            else {
                if (jingLingSkillStr != "" && jingLingSkillStr != "0") {
                    addJingLingList = jingLingSkillStr;
                }
            }
        }

        if (addJingLingList != "")
        {
            SkillItemListText = SkillItemListText + "," + addJingLingList;
            AddSkillItemListText = AddSkillItemListText + "," + addJingLingList;
        }



        //获取当前携带的觉醒技能(被动)
        /*
        string juexingSkillIDStr = "";
        string JueXingJiHuoIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] jueXingJiHuoIDList = JueXingJiHuoIDSet.Split(';');
        for (int i = 0; i < jueXingJiHuoIDList.Length; i++)
        {
            string jingLingSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddSkill", "ID", jueXingJiHuoIDList[i], "JueXing_Template");
            //获取职业
            string roseOcc = Game_PublicClassVar.Get_function_Rose.GetRoseOcc();
            string[] SkillStrList = jingLingSkillStr.Split('@');
            if (SkillStrList.Length >= 2)
            {
                switch (roseOcc)
                {
                    case "1":
                        jingLingSkillStr = SkillStrList[0];
                        break;

                    case "2":
                        jingLingSkillStr = SkillStrList[1];
                        break;
                }
            }

            if (jingLingSkillStr != "" && jingLingSkillStr != "0")
            {
                juexingSkillIDStr = juexingSkillIDStr + "," + jingLingSkillStr;
            }
        }

        if (juexingSkillIDStr != "")
        {
            SkillItemListText = SkillItemListText + "," + juexingSkillIDStr;
            AddSkillItemListText = AddSkillItemListText + "," + juexingSkillIDStr;
        }
        */


        //添加天赋技能(天赋技能在装备技能处会有体现,所以此处会屏蔽)
        /*
        string learnTianSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (learnTianSkillID != "") {
            SkillItemListText = SkillItemListText + "," + learnTianSkillID;
            AddSkillItemListText = AddSkillItemListText + "," + learnTianSkillID;
        }
        */

        //如果没有自身技能就只获取装备技能
        if (SkillItemListText == "")
        {
            SkillItemListText = addList;
            AddSkillItemListText = addList;
        }

        SkillItemList = SkillItemListText.Split(',');
        AddSkillItemList = AddSkillItemListText.Split(',');
    }

    public void Btn_SkillReSPCommont() {

        GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);

        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_9");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, Btn_SkillReSP, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否花费600钻石重置SP值？", Btn_SkillReSP, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    //技能重置
    public void Btn_SkillReSP() {

        Debug.Log("我点击了技能重置按钮");
        
        //消耗800钻石
        if (Game_PublicClassVar.Get_function_Rose.CostReward("2", "600"))
        {
            //重置SP
            Game_PublicClassVar.Get_function_Skill.RoseReSkillSP();
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_86");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("钻石不足!");
        }
    }

	public void CloseUI(){
		Destroy (this.gameObject);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseSkill_Status = false;
	}

    //点击按钮
    public void Click_Type(string type)
    {
        if (type == "2") {
            if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < 10)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_218");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("10级开启天赋系统");
                return;
            }
        }

        if (type == "3")
        {

            if (Game_PublicClassVar.Get_function_Rose.ReturnJueXingTaskStatus()==false)
            {

                //获取是否完成觉醒任务
                string roseCompleteTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (roseCompleteTaskID.Contains("33000018"))
                {
                    Game_PublicClassVar.Get_function_Task.jueXingJianCe();
                }
                else {
                    //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_218");
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("玩家升到60级后,完成觉醒系列职业任务后开启此系统");
                    return;
                }
            }
        }


        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_FunctionBtn_1.GetComponent<Image>().sprite = img;
        Obj_FunctionBtn_2.GetComponent<Image>().sprite = img;
        Obj_FunctionBtn_3.GetComponent<Image>().sprite = img;

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);

        switch (type)
        {
            //技能信息
            case "1":
                //显示底图S
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                Obj_FunctionBtn_1.GetComponent<Image>().sprite = img;
                Obj_SkillListSet.SetActive(true);
                Obj_SkillTianFuSet.SetActive(false);
                Obj_JueXingSet.SetActive(false);
                Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);
                //更新技能列表
                createSkillList(nowSkillListNum);
                break;

            //技能天赋
            case "2":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                Obj_FunctionBtn_2.GetComponent<Image>().sprite = img;
                Obj_SkillListSet.SetActive(false);
                Obj_JueXingSet.SetActive(false);
                Obj_SkillTianFuSet.SetActive(true);
                Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);
                break;

            //觉醒天赋
            case "3":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                Obj_FunctionBtn_3.GetComponent<Image>().sprite = img;
                Obj_SkillListSet.SetActive(false);
                Obj_SkillTianFuSet.SetActive(false);
                Obj_JueXingSet.SetActive(true);
                Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);
                break;
        }
    }

    //传入值获取技能显示的位置坐标
    void SkillIconShow(string iconNum) {

        GameObject showObj = (GameObject)Instantiate(Obj_SkillIconShowObj);
        showObj.transform.SetParent(Obj_SkillIconShowSet.transform);
        showObj.transform.localScale = new Vector3(1, 1, 1);
        showObj.GetComponent<UI_RoseSkillIconShow>().SpaceID = iconNum;
        showObj.transform.localPosition = Obj_SkillIconShowPosition[int.Parse(iconNum) - 1].transform.localPosition;
        Obj_SkillIconShowPosition[int.Parse(iconNum) - 1].SetActive(false);
        showObj.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
        if (iconNum == "9" || iconNum == "10") {
            showObj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }

        /*
        switch (iconNum) { 
            case "1":
                
                break;
            case "2":
                showObj.transform.localPosition = new Vector3(0, 0, 0); 
                break;
            case "3":
                showObj.transform.localPosition = new Vector3(0, 0, 0); 
                break;
            case "4":
                showObj.transform.localPosition = new Vector3(0, 0, 0); 
                break;
            case "5":
                showObj.transform.localPosition = new Vector3(0, 0, 0); 
                break;
            case "6":
                showObj.transform.localPosition = new Vector3(0, 0, 0); 
                break;
            case "7":
                showObj.transform.localPosition = new Vector3(0, 0, 0); 
                break;
            case "8":
                showObj.transform.localPosition = new Vector3(0, 0, 0); 
                break;
        }
        */

    }

    //点击主动技能列表
    public void ZhuDongSkillBtnList() {
        Obj_RoseSkillSet.SetActive(true);
        Obj_RoseItemSkillSet.SetActive(false);

        Obj_SkillBtn_ZhuDong.SetActive(true);
        Obj_SkillBtn_Bag.SetActive(false);
    }

    //点击背包道具按钮
    public void BagItemSkillBtnList() {
        //Debug.Log("点击了道具");
        //清空道具列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_BagSpaceListSet);

        Obj_RoseSkillSet.SetActive(false);
        Obj_RoseItemSkillSet.SetActive(true);
        //将自身的所有消耗品显示
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++) {
            string itemID = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            //Debug.Log("itemID = " + itemID);
            if (itemID != "" && itemID!="0")
            {
                string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
                if (itemType == "1") {
                    //判定是否有技能
                    string itemSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", itemID, "Item_Template");
                    if (itemSkillID != "0")
                    {
                        //Debug.Log("开始创建！");
                        //开始创建背包格子
                        GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
                        bagSpace.transform.SetParent(Obj_BagSpaceListSet.transform);
                        bagSpace.GetComponent<UI_BagSpace>().BagPosition = i.ToString();
                        bagSpace.GetComponent<UI_BagSpace>().SpaceType = "1";   //设置格子为背包属性   
                        bagSpace.GetComponent<UI_BagSpace>().MoveBagStatus = false;
                        bagSpace.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
            }
        }

        Obj_SkillBtn_ZhuDong.SetActive(false);
        Obj_SkillBtn_Bag.SetActive(true);

        //展示引导(上一级引导存在开启下一级引导)
        if (Obj_YinDao_SkillItemBtn.activeSelf) {
            Obj_YinDao_SkillItemBtn.SetActive(false);
            ShowYinDao_SkillItemUse();
        }
    }
}
