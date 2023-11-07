using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_PaiHangShowPet : MonoBehaviour {

    public GameObject Obj_RosePetSet;
    public GameObject Obj_Pet_ListSet;
    public GameObject Obj_Pet_HeChengSet;
    public GameObject Obj_Pet_XiLianSet;
    public GameObject Obj_Pet_PropertyAdd;
    public GameObject ObjBtn_Pet_List;
    public GameObject ObjBtn_Pet_HeCheng;
    public GameObject ObjBtn_Pet_XiLian;


    //宠物列表相关
    public GameObject Obj_PetList;
    public GameObject Obj_PetListSet;

    public GameObject Obj_PetName;
    public GameObject Obj_PetNameInput;
    public GameObject Obj_PetLv;
    public GameObject Obj_PetExp;
    public GameObject Obj_PetExpPro;
    public GameObject Obj_PetShenShouImg;
    public GameObject Obj_PetBianYiImg;
    public GameObject Obj_PetNumShow;
    public GameObject Obj_PetNumIfNull;
    public GameObject Obj_PetStatusText;    //宠物状态文字

    public GameObject Obj_EquipBtnText_1;
    public GameObject Obj_EquipBtnText_2;
    public GameObject Obj_EquipBtnText_3;

    public bool UpdateShowStatus;
    public int UpdateXuanZhongSum;      	//更换选中状态(确保更新一次)
    public string NowSclectPetID;       	//当前选中的宠物ID
    public int nowPetNum = 0;

    //宠物属性相关
    public GameObject Obj_PropertyValue_Hp;
    public GameObject Obj_PropertyValue_Act;
    public GameObject Obj_PropertyValue_Def;
    public GameObject Obj_PropertyValue_Adf;
    public GameObject Obj_PropertyValue_ActSpeed;
    public GameObject Obj_PropertyValue_MoveSpeed;
    public GameObject Obj_PropertyValueDianShu;

    //宠物资质相关
    public GameObject Obj_ZiZhiValue_Hp;
    public GameObject Obj_ZiZhiValue_Act;
    public GameObject Obj_ZiZhiValue_MageAct;
    public GameObject Obj_ZiZhiValue_Def;
    public GameObject Obj_ZiZhiValue_Adf;
    public GameObject Obj_ZiZhiValue_ActSpeed;
    public GameObject Obj_ZiZhiValue_ChengZhang;


    public GameObject Obj_ZiZhiPro_Hp;
    public GameObject Obj_ZiZhiPro_Act;
    public GameObject Obj_ZiZhiPro_MageAct;
    public GameObject Obj_ZiZhiPro_Def;
    public GameObject Obj_ZiZhiPro_Adf;
    public GameObject Obj_ZiZhiPro_ActSpeed;
    public GameObject Obj_ZiZhiPro_ChengZhang;

    //宠物技能相关
    public GameObject Obj_SkillListSet;
    public GameObject Obj_PetSkillIcon;

    public string RosePetIDListStr;     	//当前角色携带的宠物ID
    private string[] RosePetIDList;
    public GameObject[] Obj_PetEquipSet;

    public Dictionary<string, string> roseEquipHideDic = new Dictionary<string, string>();
    public string roseEquipHideStr;

    public string PetXiuLianStr;

    //服务传输相关数据
    //public string Server_PetDataStr;
    public string[] SerVer_PetDataList;

	// Use this for initialization
	void Start () {

        //测试数据
        //RosePetIDListStr = "1;2;3;4;5";



        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_RosePetSet);

        //显示通用UI
        //Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject, "201");

        //Click_Type("1");

        //打开摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PetModelListSet.SetActive(true);

    }

	//初始化
	public void Inti(){
	
		//初始化界面打开默认选择第一个(需要循环获取第一个为非0ID的宠物)
		/*
		NowSclectPetID = Game_PublicClassVar.Get_function_AI.Pet_ReturnRoseListFirst();
		Debug.Log("NowSclectPetID = " + NowSclectPetID);
		if (NowSclectPetID == "0") {
			Debug.Log("当前未携带任何宠物");
		}

		NowSclectPetID = "1";		//后期需要调整
		*/
		//RosePetIDList = RosePetIDListStr.Split(';');

		Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.Obj_PetModlePosition);

		//初始化当前宠物列表
		showPetList();
		UpdateShowStatus = true;

        EquipHideID();

    }
	
	// Update is called once per frame
	void Update () {



	}

    private void OnDestroy()
    {
        //关闭摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PetModelListSet.SetActive(false);
    }

    void LateUpdate() {
        //显示宠物列表
        if (UpdateShowStatus)
        {
            UpdateShowStatus = false;
            //UpdateXuanZhongSum = UpdateXuanZhongSum + 1;
            showPetProperty();

            //更新宠物选中图标的显示信息
            for (int i = 0; i < Obj_PetListSet.transform.childCount; i++)
            {
                GameObject go = Obj_PetListSet.transform.GetChild(i).gameObject;
				go.GetComponent<UI_PaiHangPetList>().UpdateStatus = true;
            }
        }
    }

    //展示宠物列表
    void showPetList() {

        //清空宠物列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PetListSet);
        nowPetNum = 0;
        //显示宠物列表
		for (int i = 1; i <= SerVer_PetDataList.Length; i++)
        {
            if (SerVer_PetDataList[i - 1] != "" && SerVer_PetDataList[i - 1] != null && SerVer_PetDataList[i - 1] != "0") {
                string[] server_PetDataList_One = SerVer_PetDataList[i - 1].Split('@');
                //string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", i.ToString(), "RosePet");
                string petID = server_PetDataList_One[1];
                if (petID != "0")
                {
                    //实例化一个宠物列表控件
                    GameObject petListObj = (GameObject)Instantiate(Obj_PetList);
                    petListObj.transform.SetParent(Obj_PetListSet.transform);
                    petListObj.transform.localScale = new Vector3(1, 1, 1);

                    petListObj.GetComponent<UI_PaiHangPetList>().ServerPetDataList = server_PetDataList_One;
                    petListObj.GetComponent<UI_PaiHangPetList>().PetOnlyID = i.ToString();
                    petListObj.GetComponent<UI_PaiHangPetList>().UpdateStatus = true;
                    petListObj.GetComponent<UI_PaiHangPetList>().Obj_FuJiObj = this.gameObject;

                    //第一个宠物设置为默认选择
                    if (nowPetNum == 0)
                    {

                        NowSclectPetID = i.ToString();
                    }
                    nowPetNum = nowPetNum + 1;
                }
            }
        }

        //显示宠物拥有数量
        //Obj_PetNumShow.GetComponent<Text>().text = "携带宠物数量：" + nowPetNum + "/" + Game_PublicClassVar.Get_game_PositionVar.RosePetLvMaxNum;
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("携带宠物数量");
		Obj_PetNumShow.GetComponent<Text>().text = langStr + "：" + nowPetNum;
        //更新宠物标题
        //Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet("201");


    }

    //更新宠物信息
    public void showPetProperty() {

        if (nowPetNum == 0)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_16");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前没有任何宠物！");
            Obj_PetNumIfNull.SetActive(true);
            return;
        }
        else {
            Obj_PetNumIfNull.SetActive(false);
        }

        //Debug.Log("刷新宠物信息");
		string[] nowPetDataList = SerVer_PetDataList [int.Parse(NowSclectPetID)-1].Split ('@');
        //获取宠物信息
		string petName = nowPetDataList[6];
		string petLv = nowPetDataList[2];
		float petNowExp = float.Parse(nowPetDataList[5]);
        float petLvExp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetUpExp", "RoseLv", petLv, "RoseExp_Template"));

        //当前资质
		float zizhiNow_Hp = float.Parse(nowPetDataList[11]);
		float zizhiNow_Act = float.Parse(nowPetDataList[12]);
		float zizhiNow_MageAct = float.Parse(nowPetDataList[13]);
		float zizhiNow_Def = float.Parse(nowPetDataList[14]);
		float zizhiNow_Adf = float.Parse(nowPetDataList[15]);
		float zizhiNow_ActSpeed = float.Parse(nowPetDataList[16]);
		float zizhiNow_ChengZhang = float.Parse(nowPetDataList[17]);

        //资质上限
		string petID = nowPetDataList[1];
        //Debug.Log("petID = " + petID);
        float zizhiSum_Hp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_Act = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_MageAct = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_Def = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_Adf = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_ActSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_ChengZhang = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang_Max", "ID", petID, "Pet_Template"));

		string petSkillListStr = nowPetDataList[18];
        string[] petSkillList = petSkillListStr.Split(';');

        //显示宠物形象
        string petModel = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetModel", "ID", petID, "Pet_Template");
		if (petModel != "" && petModel != "0") {
			petModel = petID;
			Game_PublicClassVar.Get_function_UI.ShowPetModel (petModel);
		} else {
			//清理
			//Debug.Log("清理模型显示");
			Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.Obj_PetModlePosition);
		}


        //显示宠物属性
        Obj_PetName.GetComponent<Text>().text = petName;
        Obj_PetLv.GetComponent<Text>().text = "LV"+ petLv;
        Obj_PetExp.GetComponent<Text>().text = petNowExp + "/" + petLvExp;
        Obj_PetExpPro.GetComponent<Image>().fillAmount = petNowExp / petLvExp;

		Game_PublicClassVar.Get_function_AI.Pet_UpdateShowProperty(this.gameObject, NowSclectPetID,"2",nowPetDataList);
        //显示宠物剩余点数
        string NowShengYuNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyNum", "ID", NowSclectPetID, "RosePet");
        Obj_PropertyValueDianShu.GetComponent<Text>().text = "剩余点数:" + NowShengYuNum;

        /*
        string petProperty_Hp ="1" ;
        string petProperty_Act ="2" ;
        string petProperty_Def ="3" ;
        string petProperty_Adf ="4" ;
        string petProperty_ActSpeed ="5" ;
        string petProperty_MoveSpeed ="6" ;

        Obj_PropertyValue_Hp.GetComponent<Text>().text = petProperty_Hp;
        Obj_PropertyValue_Act.GetComponent<Text>().text = petProperty_Act;
        Obj_PropertyValue_Def.GetComponent<Text>().text = petProperty_Def;
        Obj_PropertyValue_Adf.GetComponent<Text>().text = petProperty_Adf;
        Obj_PropertyValue_ActSpeed.GetComponent<Text>().text = petProperty_ActSpeed;
        Obj_PropertyValue_MoveSpeed.GetComponent<Text>().text = petProperty_MoveSpeed;
        */
        
        //显示宠物资质
        Obj_ZiZhiValue_Hp.GetComponent<Text>().text = zizhiNow_Hp + "/" + zizhiSum_Hp;
        Obj_ZiZhiValue_Act.GetComponent<Text>().text = zizhiNow_Act + "/" + zizhiSum_Act;
        Obj_ZiZhiValue_MageAct.GetComponent<Text>().text = zizhiNow_MageAct + "/" + zizhiSum_MageAct;
        Obj_ZiZhiValue_Def.GetComponent<Text>().text = zizhiNow_Def + "/" + zizhiSum_Def;
        Obj_ZiZhiValue_Adf.GetComponent<Text>().text = zizhiNow_Adf + "/" + zizhiSum_Adf;
        Obj_ZiZhiValue_ActSpeed.GetComponent<Text>().text = zizhiNow_ActSpeed + "/" + zizhiSum_ActSpeed;
        Obj_ZiZhiValue_ChengZhang.GetComponent<Text>().text = zizhiNow_ChengZhang + "/" + zizhiSum_ChengZhang;

        //显示宠物资质进度条
        float zizhi_Hp_Pro = zizhiNow_Hp / zizhiSum_Hp;
        float zizhi_Act_Pro = zizhiNow_Act / zizhiSum_Act;
        float zizhi_MageAct_Pro = zizhiNow_MageAct / zizhiSum_MageAct;
        float zizhi_Def_Pro = zizhiNow_Def / zizhiSum_Def;
        float zizhi_Adf_Pro = zizhiNow_Adf / zizhiSum_Adf;
        float zizhi_ActSpeed_Pro = zizhiNow_ActSpeed / zizhiSum_ActSpeed;
        float zizhi_ChengZhang_Pro = zizhiNow_ChengZhang / zizhiSum_ChengZhang;

        Obj_ZiZhiPro_Hp.GetComponent<Image>().fillAmount = zizhi_Hp_Pro;
        Obj_ZiZhiPro_Act.GetComponent<Image>().fillAmount = zizhi_Act_Pro;
        Obj_ZiZhiPro_MageAct.GetComponent<Image>().fillAmount = zizhi_MageAct_Pro;
        Obj_ZiZhiPro_Def.GetComponent<Image>().fillAmount = zizhi_Def_Pro;
        Obj_ZiZhiPro_Adf.GetComponent<Image>().fillAmount = zizhi_Adf_Pro;
        Obj_ZiZhiPro_ActSpeed.GetComponent<Image>().fillAmount = zizhi_ActSpeed_Pro;
        Obj_ZiZhiPro_ChengZhang.GetComponent<Image>().fillAmount = zizhi_ChengZhang_Pro;


        //清空显示
        Obj_ZiZhiValue_Hp.GetComponent<Text>().color = new Color(1, 1, 1);
        Obj_ZiZhiValue_Act.GetComponent<Text>().color = new Color(1, 1, 1);
        Obj_ZiZhiValue_MageAct.GetComponent<Text>().color = new Color(1, 1, 1);
        Obj_ZiZhiValue_Def.GetComponent<Text>().color = new Color(1, 1, 1);
        Obj_ZiZhiValue_Adf.GetComponent<Text>().color = new Color(1, 1, 1);
        Obj_ZiZhiValue_ActSpeed.GetComponent<Text>().color = new Color(1, 1, 1);
        Obj_ZiZhiValue_ChengZhang.GetComponent<Text>().color = new Color(1, 1, 1);

        //超过资质显示绿色
        if (zizhi_Hp_Pro >= 1) {
            Obj_ZiZhiValue_Hp.GetComponent<Text>().color = new Color(0.2f,1,0);
        }
        if (zizhi_Act_Pro >= 1)
        {
            Obj_ZiZhiValue_Act.GetComponent<Text>().color = new Color(0.2f, 1, 0);
        }
        if (zizhi_MageAct_Pro >= 1)
        {
            Obj_ZiZhiValue_MageAct.GetComponent<Text>().color = new Color(0.2f, 1, 0);
        }
        if (zizhi_Def_Pro >= 1)
        {
            Obj_ZiZhiValue_Def.GetComponent<Text>().color = new Color(0.2f, 1, 0);
        }
        if (zizhi_Adf_Pro >= 1)
        {
            Obj_ZiZhiValue_Adf.GetComponent<Text>().color = new Color(0.2f, 1, 0);
        }
        if (zizhi_ActSpeed_Pro >= 1)
        {
            Obj_ZiZhiValue_ActSpeed.GetComponent<Text>().color = new Color(0.2f, 1, 0);
        }
        if (zizhi_ChengZhang_Pro >= 1)
        {
            Obj_ZiZhiValue_ChengZhang.GetComponent<Text>().color = new Color(0.2f, 1, 0);
        }

        //清空父节点
        for (int i = 0; i < Obj_SkillListSet.transform.childCount; i++)
        {
            GameObject go = Obj_SkillListSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        //显示宠物技能
        if (petSkillList[0]!="" && petSkillList[0]!="0"){
            for (int i = 0; i <= petSkillList.Length - 1; i++) {
                //实例化一个宠物列表控件
                GameObject petSkillListObj = (GameObject)Instantiate(Obj_PetSkillIcon);
                petSkillListObj.transform.SetParent(Obj_SkillListSet.transform);
                petSkillListObj.transform.localScale = new Vector3(1, 1, 1);
                petSkillListObj.GetComponent<PetSkillIcon>().PetSkillID = petSkillList[i];
            }
            
            //填补空位
            Game_PublicClassVar.Get_function_AI.Pet_SkillShowNull(petSkillList, Obj_PetSkillIcon, Obj_SkillListSet);
            /*
            if (petSkillList.Length <= 12)
            {
                for (int i = petSkillList.Length; i < 12; i++)
                {
                    //实例化一个宠物列表控件
                    GameObject petSkillListObj = (GameObject)Instantiate(Obj_PetSkillIcon);
                    petSkillListObj.transform.SetParent(Obj_SkillListSet.transform);
                    petSkillListObj.transform.localScale = new Vector3(1, 1, 1);
                    petSkillListObj.GetComponent<PetSkillIcon>().PetSkillID = "0";
                }
            }
            else {

                int skillyushu = petSkillList.Length % 4;
                for (int i = petSkillList.Length; i < 4; i++)
                {
                    //实例化一个宠物列表控件
                    GameObject petSkillListObj = (GameObject)Instantiate(Obj_PetSkillIcon);
                    petSkillListObj.transform.SetParent(Obj_SkillListSet.transform);
                    petSkillListObj.transform.localScale = new Vector3(1, 1, 1);
                    petSkillListObj.GetComponent<PetSkillIcon>().PetSkillID = "0";
                }
            }
            */
        }

        
        string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petID, "Pet_Template");
        //是否变异
        if (monsterType == "1")
        {
            Obj_PetBianYiImg.SetActive(true);
        }
        else
        {
            Obj_PetBianYiImg.SetActive(false);
        }
        //显示是否神兽
        if (monsterType=="2")
        {
            Obj_PetShenShouImg.SetActive(true);
        }else{
            Obj_PetShenShouImg.SetActive(false);
        }

        //显示是出战状态
        string petStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", NowSclectPetID, "RosePet");
        switch (petStatus) { 
            case "0":
                Obj_PetStatusText.GetComponent<Text>().text = "出 战";
                break;
            case "1":
                Obj_PetStatusText.GetComponent<Text>().text = "休 息";
                break;
        }


        //显示宠物装备

        if (nowPetDataList.Length >= 20) {

            string equipID_1 = nowPetDataList[19];
            string equipID_2 = nowPetDataList[21];
            string equipID_3 = nowPetDataList[23];
            string equipHideStrID_1 = nowPetDataList[20];
            string equipHideStrID_2 = nowPetDataList[22];
            string equipHideStrID_3 = nowPetDataList[24];



            Obj_PetEquipSet[0].GetComponent<UI_PaiHangPetEquipShow>().EquipID = equipID_1;
            Obj_PetEquipSet[0].GetComponent<UI_PaiHangPetEquipShow>().PaihangPetEquipHideStr = GetHidePro(equipHideStrID_1);
            Obj_PetEquipSet[0].GetComponent<UI_PaiHangPetEquipShow>().UpdataStatus = true;

            Obj_PetEquipSet[1].GetComponent<UI_PaiHangPetEquipShow>().EquipID = equipID_2;
            Obj_PetEquipSet[1].GetComponent<UI_PaiHangPetEquipShow>().PaihangPetEquipHideStr = GetHidePro(equipHideStrID_2);
            Obj_PetEquipSet[1].GetComponent<UI_PaiHangPetEquipShow>().UpdataStatus = true;

            Obj_PetEquipSet[2].GetComponent<UI_PaiHangPetEquipShow>().EquipID = equipID_3;
            Obj_PetEquipSet[2].GetComponent<UI_PaiHangPetEquipShow>().PaihangPetEquipHideStr = GetHidePro(equipHideStrID_3);
            Obj_PetEquipSet[2].GetComponent<UI_PaiHangPetEquipShow>().UpdataStatus = true;

            if (nowPetDataList.Length >= 27) {
                string equipID_4 = nowPetDataList[25];
                string equipHideStrID_4 = nowPetDataList[26];

                Obj_PetEquipSet[3].GetComponent<UI_PaiHangPetEquipShow>().EquipID = equipID_4;
                Obj_PetEquipSet[3].GetComponent<UI_PaiHangPetEquipShow>().PaihangPetEquipHideStr = GetHidePro(equipHideStrID_4);
                Obj_PetEquipSet[3].GetComponent<UI_PaiHangPetEquipShow>().UpdataStatus = true;
            }
        }
    }

    //获取隐藏属性
    public string GetHidePro(string hideIDStr) {

        if (hideIDStr == null) {
            return "";
        }

        //string[] hideIDList = hideIDStr.Split();
        string equipHideStr = "";
        if (roseEquipHideDic.ContainsKey(hideIDStr))
        {
            equipHideStr = roseEquipHideDic[hideIDStr];
        }

        return equipHideStr;

    }

    //出战按钮
    public void Btn_ChuZhan() {

        //Debug.Log("我点击了出战按钮");
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();

        //判定出战
        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", NowSclectPetID, "RosePet");
        int fightLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FightLv", "ID", petID, "Pet_Template"));
        if (roseStatus.GetComponent<Rose_Proprety>().Rose_Lv < fightLv) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_19");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("出战等级不足");
            return;
        }

        //判定是否进入冷却时间
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ZhaoHuanCDStatus) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_20");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请等待冷却时间结束再次召唤宠物！");
            return;
        }

        //判定当前是否进入出战CD
        float petZhaoHuanCD = Game_PublicClassVar.Get_function_AI.Pet_GetZhaoHuanCD(NowSclectPetID);
        if (petZhaoHuanCD > 0)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_21");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物召唤冷却");
            return;
        }

        //判定出战位
        if (roseStatus.RosePetObj[int.Parse(NowSclectPetID) - 1] == null)
        {
            //判定当前出战
            if(Game_PublicClassVar.Get_function_Rose.GetRosePetFightNum()>=1){

                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_22");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前出战位已满！");
                return;
            }
            //出战
            Game_PublicClassVar.Get_function_Rose.RosePetCreate(int.Parse(NowSclectPetID));
            //显示是否出战
            Obj_PetStatusText.GetComponent<Text>().text = "休 息";
        }
        else {

            //战斗时不能撤回宠物
            if (!roseStatus.RoseFightStatus) {
                //撤回
                Game_PublicClassVar.Get_function_Rose.RosePetDelete(int.Parse(NowSclectPetID));
                //显示是否出战
                Obj_PetStatusText.GetComponent<Text>().text = "出 战";
            }
            else
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_23");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("战斗时不能撤回宠物");
            }
        }

        UpdateShowStatus = true;
    }


    //修改名称
    public void Btn_ReviseName() {

        //Debug.Log("名称");
        string petName = Obj_PetNameInput.GetComponent<InputField>().text;

        //判定名字是否为空
        if (petName == "")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_24");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("名称不能为空");
            return;
        }

        //判定名字是否长度大于7
        if (petName.Length>7)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_25");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("名称长度不能大于7个字符");
            return;
        }

        Obj_PetName.GetComponent<Text>().text = petName;

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetName", petName, "ID", NowSclectPetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

        //初始化当前宠物列表
        showPetList();
    }


    public void Btn_FangShengHint() {

        //判断出战
        string petStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", NowSclectPetID, "RosePet");
        if (petStatus == "1") {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_26");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("出战中不能放生宠物！");
            return;
        }

        GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_3");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint1, Btn_FangSheng, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否放生此宠物？", Btn_FangSheng, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }


    //放生按钮
    public void Btn_FangSheng() {

        //Debug.Log("我点击了放生按钮");
        //Game_PublicClassVar.Get_function_Rose.RosePetClearn(int.Parse(NowSclectPetID));
        Game_PublicClassVar.Get_function_AI.Pet_ClearnData(NowSclectPetID);
        //初始化界面打开默认选择第一个(需要循环获取第一个为非0ID的宠物)
        NowSclectPetID = Game_PublicClassVar.Get_function_AI.Pet_ReturnRoseListFirst();
        Debug.Log("NowSclectPetID = " + NowSclectPetID);
        if (NowSclectPetID == "0")
        {
            Debug.Log("当前未携带任何宠物");
        }

        //RosePetIDList = RosePetIDListStr.Split(';');

        //初始化当前宠物列表
        showPetList();
        UpdateShowStatus = true;
    }

    //点击名称
    public void Btn_ClickName()
    {
        //Debug.Log("开始修改名称");
        if (Obj_PetNameInput.GetComponent<InputField>().text == "") {
            Obj_PetNameInput.GetComponent<InputField>().text = Obj_PetName.GetComponent<Text>().text;
            Obj_PetName.GetComponent<Text>().text = "";
        }
    }

    public void Btn_EndClickName()
    {
        //Debug.Log("结束修改名称");
        //Obj_PetNameInput.GetComponent<InputField>().text = Obj_PetName.GetComponent<Text>().text;

        if (Obj_PetNameInput.GetComponent<InputField>().text == "")
        {
            string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", NowSclectPetID, "RosePet");
            Obj_PetName.GetComponent<Text>().text = petName;
        }
    }

    //打开属性加点
    public void Btn_AddProperty() {
        Obj_Pet_PropertyAdd.SetActive(true);
        Obj_Pet_PropertyAdd.GetComponent<UI_PetAddpProperty>().RosePetID = NowSclectPetID;
        Obj_Pet_PropertyAdd.GetComponent<UI_PetAddpProperty>().UpdateShow();
        Obj_Pet_PropertyAdd.GetComponent<UI_PetAddpProperty>().ShowAddProperty();
    }




    public void CloseUI()
    {
        Destroy(this.gameObject);
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RosePet_Status = false;
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
    }


    //将玩家的隐藏属性加载到Dic中方便调用
    public void EquipHideID()
    {

        //隐藏属性ID和隐藏值
        if (roseEquipHideStr != "" && roseEquipHideStr != null)
        {
            string[] hideList = roseEquipHideStr.Split(']');
            //Debug.Log("roseEquipHideStr = " + roseEquipHideStr);
            for (int i = 0; i < hideList.Length; i++)
            {
                Debug.Log("hideList[i] = " + hideList[i]);
                string[] hideIDPro = hideList[i].Split('[');
                //hideIDPro[0] 为0表示没有装备
                if (hideIDPro[0] != "0")
                {
                    roseEquipHideDic.Add(hideIDPro[0], hideIDPro[1]);
                }
            }
        }
    }

    /*
    //点击按钮
    public void Click_Type(string type)
    {
        Obj_Pet_ListSet.SetActive(false);
        Obj_Pet_HeChengSet.SetActive(false);
        Obj_Pet_XiLianSet.SetActive(false);
        Obj_Pet_PropertyAdd.SetActive(false);

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        ObjBtn_Pet_List.GetComponent<Image>().sprite = img;
        ObjBtn_Pet_HeCheng.GetComponent<Image>().sprite = img;
        ObjBtn_Pet_XiLian.GetComponent<Image>().sprite = img;

        switch (type)
        {
            //宠物信息
            case "1":
                //展示角色
                Obj_Pet_ListSet.SetActive(true);

                //刷新属性
                showPetList();
                UpdateShowStatus = true;

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_Pet_List.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);
                break;

            //宠物合成
            case "2":
                Obj_Pet_HeChengSet.SetActive(true);

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_Pet_HeCheng.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                //重置位置
                Obj_Pet_HeChengSet.GetComponent<UI_PetHeCheng>().PetID_List_1 = "0";
                Obj_Pet_HeChengSet.GetComponent<UI_PetHeCheng>().PetID_List_2 = "0";
                Obj_Pet_HeChengSet.GetComponent<UI_PetHeCheng>().showAllHeChengList();
                break;

            //宠物洗炼
            case "3":
                Obj_Pet_XiLianSet.SetActive(true);

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_Pet_XiLian.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);
                break;
        }
    }
	*/
}
