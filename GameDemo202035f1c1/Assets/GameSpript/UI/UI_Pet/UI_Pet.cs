using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Pet : MonoBehaviour {

    public GameObject Obj_RosePetSet;
    public GameObject Obj_Pet_ListSet;
    public GameObject Obj_Pet_HeChengSet;
    public GameObject Obj_Pet_XiLianSet;
    public GameObject Obj_Pet_XiuLianSet;
    public GameObject Obj_Pet_PropertyAdd;
    public GameObject Obj_Pet_EquipSet;
    public GameObject Obj_Pet_LeftSet;
    public GameObject ObjBtn_Pet_List;
    public GameObject ObjBtn_Pet_HeCheng;
    public GameObject ObjBtn_Pet_XiLian;
    public GameObject ObjBtn_Pet_XiuLian;
    public string OpenType_ChuShi;

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
    public GameObject Obj_PetBaoBaoImg;
    public GameObject Obj_PetYeSheng;
    public GameObject Obj_PetNumIfNull;
    public GameObject Obj_PetStatusText;    //宠物状态文字

    public GameObject Obj_EquipBtnText_1;
    public GameObject Obj_EquipBtnText_2;
    public GameObject Obj_EquipBtnText_3;
    public GameObject Obj_EquipBtnText_4;

    public bool UpdateShowStatus;
    public int UpdateXuanZhongSum;      //更换选中状态(确保更新一次)
    public string NowSclectPetID;       //当前选中的宠物ID
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

    public GameObject Obj_BtnPetLock;
    public GameObject Obj_BtnPetCostLock;


    public string RosePetIDListStr;     //当前角色携带的宠物ID
    private string[] RosePetIDList;


    // Use this for initialization
    void Start() {

        //测试数据
        //RosePetIDListStr = "1;2;3;4;5";

        //初始化界面打开默认选择第一个(需要循环获取第一个为非0ID的宠物)

        if (NowSclectPetID == "") {
            NowSclectPetID = Game_PublicClassVar.Get_function_AI.Pet_ReturnRoseListFirst();
        }

        //Debug.Log("NowSclectPetID = " + NowSclectPetID);
        if (NowSclectPetID == "0") {
            //Debug.Log("当前未携带任何宠物");
        }

        //RosePetIDList = RosePetIDListStr.Split(';');

        //初始化当前宠物列表
        //清空宠物模型显示
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.Obj_PetModlePosition);
        showPetList();

        UpdateShowStatus = true;

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_RosePetSet);

        //显示通用UI
        Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject, "201");

        if (OpenType_ChuShi == "") {
            OpenType_ChuShi = "1";
        }

        Click_Type(OpenType_ChuShi);


        //初始化标签按钮
        if (Game_PublicClassVar.Get_wwwSet.GameSetLanguage._Language != "Chinese")
        {
            Obj_EquipBtnText_1.GetComponent<Text>().fontSize = 20;
            Obj_EquipBtnText_2.GetComponent<Text>().fontSize = 20;
            Obj_EquipBtnText_3.GetComponent<Text>().fontSize = 20;
        }

        //打开摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PetModelListSet.SetActive(true);

        Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().Obj_PetEquipShowSet.SetActive(false);
    }

    // Update is called once per frame
    void Update() {



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
                go.GetComponent<UI_PetList>().UpdateStatus = true;
            }
        }
    }



    //展示宠物列表
    void showPetList() {

        //清空宠物列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PetListSet);
        nowPetNum = 0;

        //显示宠物列表
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePetMaxNum; i++)
        {
            string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", i.ToString(), "RosePet");
            if (petID != "0") {
                //实例化一个宠物列表控件
                GameObject petListObj = (GameObject)Instantiate(Obj_PetList);
                petListObj.transform.SetParent(Obj_PetListSet.transform);
                petListObj.transform.localScale = new Vector3(1, 1, 1);

                petListObj.GetComponent<UI_PetList>().PetOnlyID = i.ToString();
                petListObj.GetComponent<UI_PetList>().UpdateStatus = true;
                petListObj.GetComponent<UI_PetList>().Obj_FuJiObj = this.gameObject;

                nowPetNum = nowPetNum + 1;
            }
        }

        //显示宠物拥有数量
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("携带宠物数量");
        Obj_PetNumShow.GetComponent<Text>().text = langStr + "：" + nowPetNum + "/" + Game_PublicClassVar.Get_game_PositionVar.RosePetLvMaxNum;

        //更新宠物标题
        Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet("201");
    }

    //更新宠物信息
    public void showPetProperty() {

        if (nowPetNum == 0)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_263");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前没有任何宠物！");
            Obj_PetNumIfNull.SetActive(true);
            return;
        }
        else {
            Obj_PetNumIfNull.SetActive(false);
        }

        //Debug.Log("刷新宠物信息");

        //获取宠物信息
        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", NowSclectPetID, "RosePet");
        string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", NowSclectPetID, "RosePet");
        float petNowExp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetExp", "ID", NowSclectPetID, "RosePet"));
        float petLvExp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetUpExp", "RoseLv", petLv, "RoseExp_Template"));

        //当前资质
        float zizhiNow_Hp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp", "ID", NowSclectPetID, "RosePet"));
        float zizhiNow_Act = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act", "ID", NowSclectPetID, "RosePet"));
        float zizhiNow_MageAct = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct", "ID", NowSclectPetID, "RosePet"));
        float zizhiNow_Def = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def", "ID", NowSclectPetID, "RosePet"));
        float zizhiNow_Adf = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf", "ID", NowSclectPetID, "RosePet"));
        float zizhiNow_ActSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed", "ID", NowSclectPetID, "RosePet"));
        float zizhiNow_ChengZhang = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang", "ID", NowSclectPetID, "RosePet"));

        //资质上限
        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", NowSclectPetID, "RosePet");
        float zizhiSum_Hp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_Act = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_MageAct = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_Def = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_Adf = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_ActSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed_Max", "ID", petID, "Pet_Template"));
        float zizhiSum_ChengZhang = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang_Max", "ID", petID, "Pet_Template"));

        string petSkillListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetSkill", "ID", NowSclectPetID, "RosePet");
        petSkillListStr = petSkillListStr.Replace(";;", ";");           //有重复的可以替换
        string[] petSkillList = petSkillListStr.Split(';');

        

        //显示宠物形象
        string petModel = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetModel", "ID", petID, "Pet_Template");
        petModel = petID;
        Game_PublicClassVar.Get_function_UI.ShowPetModel(petModel);

        //显示宠物属性
        if (petName.Length > 8)
        {
            Obj_PetName.GetComponent<Text>().fontSize = 22;
        }
        else {
            Obj_PetName.GetComponent<Text>().fontSize = 25;
        }
        Obj_PetName.GetComponent<Text>().text = petName;
        Obj_PetLv.GetComponent<Text>().text = petLv+"级";
        Obj_PetExp.GetComponent<Text>().text = petNowExp + "/" + petLvExp;
        Obj_PetExpPro.GetComponent<Image>().fillAmount = petNowExp / petLvExp;

        Game_PublicClassVar.Get_function_AI.Pet_UpdateShowProperty(this.gameObject, NowSclectPetID);

        //显示宠物剩余点数
        string NowShengYuNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyNum", "ID", NowSclectPetID, "RosePet");
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("剩余点数");
        Obj_PropertyValueDianShu.GetComponent<Text>().text = langStr + ":" + NowShengYuNum;

        //显示宠物资质
        Obj_ZiZhiValue_Hp.GetComponent<Text>().text = zizhiNow_Hp + "/" + zizhiSum_Hp;
        Obj_ZiZhiValue_Act.GetComponent<Text>().text = zizhiNow_Act + "/" + zizhiSum_Act;
        Obj_ZiZhiValue_MageAct.GetComponent<Text>().text = zizhiNow_MageAct + "/" + zizhiSum_MageAct;
        Obj_ZiZhiValue_Def.GetComponent<Text>().text = zizhiNow_Def + "/" + zizhiSum_Def;
        Obj_ZiZhiValue_Adf.GetComponent<Text>().text = zizhiNow_Adf + "/" + zizhiSum_Adf;
        Obj_ZiZhiValue_ActSpeed.GetComponent<Text>().text = zizhiNow_ActSpeed + "/" + zizhiSum_ActSpeed;
        Obj_ZiZhiValue_ChengZhang.GetComponent<Text>().text = zizhiNow_ChengZhang + "/" + zizhiSum_ChengZhang;

        //显示宠物资质进度条
        Obj_ZiZhiPro_Hp.GetComponent<Image>().fillAmount = zizhiNow_Hp / zizhiSum_Hp;
        Obj_ZiZhiPro_Act.GetComponent<Image>().fillAmount = zizhiNow_Act / zizhiSum_Act;
        Obj_ZiZhiPro_MageAct.GetComponent<Image>().fillAmount = zizhiNow_MageAct / zizhiSum_MageAct;
        Obj_ZiZhiPro_Def.GetComponent<Image>().fillAmount = zizhiNow_Def / zizhiSum_Def;
        Obj_ZiZhiPro_Adf.GetComponent<Image>().fillAmount = zizhiNow_Adf / zizhiSum_Adf;
        Obj_ZiZhiPro_ActSpeed.GetComponent<Image>().fillAmount = zizhiNow_ActSpeed / zizhiSum_ActSpeed;
        Obj_ZiZhiPro_ChengZhang.GetComponent<Image>().fillAmount = zizhiNow_ChengZhang / zizhiSum_ChengZhang;

        //清空父节点
        for (int i = 0; i < Obj_SkillListSet.transform.childCount; i++)
        {
            GameObject go = Obj_SkillListSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }

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
        if (zizhi_Hp_Pro >= 1)
        {
            Obj_ZiZhiValue_Hp.GetComponent<Text>().color = new Color(0.2f, 1, 0);
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


        //显示宠物技能
        if (petSkillList.Length>=1) {
            for (int i = 0; i < petSkillList.Length; i++) {
                if (petSkillList[i] != null && petSkillList[i] != "0" && petSkillList[i] != "")
                {
                    //实例化一个宠物列表控件
                    GameObject petSkillListObj = (GameObject)Instantiate(Obj_PetSkillIcon);
                    petSkillListObj.transform.SetParent(Obj_SkillListSet.transform);
                    petSkillListObj.transform.localScale = new Vector3(1, 1, 1);
                    petSkillListObj.GetComponent<PetSkillIcon>().PetSkillID = petSkillList[i];
                }
            }
        }
        //填补空位
        Game_PublicClassVar.Get_function_AI.Pet_SkillShowNull(petSkillList, Obj_PetSkillIcon, Obj_SkillListSet);

        string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petID, "Pet_Template");

        //清理所有显示
        clearnShowTitle();

        string ifbaby = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", NowSclectPetID, "RosePet");
        if (ifbaby == "0")
        {
            Obj_PetYeSheng.SetActive(true);
        }
        if (ifbaby == "1")
        {
            Obj_PetBaoBaoImg.SetActive(true);
        }

        //是否变异
        if (monsterType == "1")
        {
            clearnShowTitle();
            Obj_PetBianYiImg.SetActive(true);
        }
        else
        {
            Obj_PetBianYiImg.SetActive(false);
        }
        //显示是否神兽
        if (monsterType == "2")
        {
            clearnShowTitle();
            Obj_PetShenShouImg.SetActive(true);
        } else {
            Obj_PetShenShouImg.SetActive(false);
        }

        //显示是出战状态
        string petStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", NowSclectPetID, "RosePet");
        switch (petStatus) {
            case "0":
                string nowlangStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("出 战");
                Obj_PetStatusText.GetComponent<Text>().text = nowlangStr;
                break;
            case "1":
                nowlangStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("休 息");
                Obj_PetStatusText.GetComponent<Text>().text = nowlangStr;
                break;
        }

        //清理名字输入状态
        Obj_PetNameInput.GetComponent<InputField>().text = "";

        //显示锁定状态
        //获取当前宠物的解锁状态
        string lockStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LockStatus", "ID", NowSclectPetID, "RosePet");
        if (lockStatus == "" || lockStatus == "0")
        {
            Obj_BtnPetLock.SetActive(false);
            Obj_BtnPetCostLock.SetActive(true);
        }
        else
        {
            Obj_BtnPetLock.SetActive(true);
            Obj_BtnPetCostLock.SetActive(false);
        }

    }

    //出战按钮
    public void Btn_ChuZhan() {

        Debug.Log("我点击了出战按钮");
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();

        //判定出战
        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", NowSclectPetID, "RosePet");
        int fightLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FightLv", "ID", petID, "Pet_Template"));
        if (roseStatus.GetComponent<Rose_Proprety>().Rose_Lv < fightLv) {

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_265");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_266");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + fightLv + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("出战等级不足！此宠物出战需要角色达到" + fightLv + "级");
            return;
        }

        //判定是否进入冷却时间
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ZhaoHuanCDStatus) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_267");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请等待冷却时间结束再次召唤宠物！");
            return;
        }

        //判定当前是否进入出战CD
        float petZhaoHuanCD = Game_PublicClassVar.Get_function_AI.Pet_GetZhaoHuanCD(NowSclectPetID);
        if (petZhaoHuanCD > 0)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_268");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物召唤冷却");
            return;
        }

        //判定出战位
        if (roseStatus.RosePetObj[int.Parse(NowSclectPetID) - 1] == null)
        {
            //判定当前出战
            if (Game_PublicClassVar.Get_function_Rose.GetRosePetFightNum() >= 1) {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_269");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前出战位已满！");
                return;
            }

            //出战
            Game_PublicClassVar.Get_function_Rose.RosePetCreate(int.Parse(NowSclectPetID));

            //显示是否出战
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("休 息");
            Obj_PetStatusText.GetComponent<Text>().text = langStr;
        }
        else {

            //战斗时不能撤回宠物
            if (!roseStatus.RoseFightStatus) {
                //撤回
                Game_PublicClassVar.Get_function_Rose.RosePetDelete(int.Parse(NowSclectPetID));
                //显示是否出战
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("出 战");
                Obj_PetStatusText.GetComponent<Text>().text = langStr;
            }
            else
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_270");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("战斗时不能撤回宠物");
            }
        }

        UpdateShowStatus = true;
    }


    private void clearnShowTitle() {

        //清理所有显示
        Obj_PetBaoBaoImg.SetActive(false);
        Obj_PetYeSheng.SetActive(false);
        Obj_PetBianYiImg.SetActive(false);
        Obj_PetShenShouImg.SetActive(false);

    }

    //显示
    public void Btn_ShowHintPetType (string type){

        switch (type) {
            //野生
            case "1":

                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_271");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("野生宠物:野外随处可见的宠物,宠物属性点数天生就比宝宝少40点,只能临时用用！");
                break;

            //宝宝
            case "2":

                langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_272");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物宝宝:野外比较少的宠物,你可以自由分配他的宠物点数,达到收益最大化！");
                break;

            //变异
            case "3":

                langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_273");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("变异宝宝:非常稀有的一种宠物,它的资质有可能超过上限！");
                break;

            //神兽
            case "4":

                langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_274");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("神兽宠物:全世界中最珍贵的宠物,没有之一！");
                break;
        }

    }


    //修改名称
    public void Btn_ReviseName() {

        Debug.Log("名称");
        string petName = Obj_PetNameInput.GetComponent<InputField>().text;

        //判定名字是否为空
        if (petName == "")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_275");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("名称不能为空");
            return;
        }

        //判定名字是否长度大于7
        if (petName.Length>7)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_276");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("名称长度不能大于7个字符");
            return;
        }

        //判断宠物字符串是否有特殊字符
        bool ifteshu = Game_PublicClassVar.Get_function_UI.IfTeShuStr(petName);
        if (ifteshu)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_277");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("修改失败!存在特殊字符");
            return;
        }

        Obj_PetName.GetComponent<Text>().text = petName;

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetName", petName, "ID", NowSclectPetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

        Obj_PetNameInput.GetComponent<InputField>().text = "";

        //初始化当前宠物列表
        showPetList();
    }


    public void Btn_FangShengHint() {

        //判断出战
        string petStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", NowSclectPetID, "RosePet");
        if (petStatus == "1") {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_278");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("出战中不能放生宠物！");
            return;
        }

        //判断是否加锁
        string lockStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LockStatus", "ID", NowSclectPetID, "RosePet");
        if (lockStatus == "1" )
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物锁定状态无法分解宠物,请先解锁!");
            return;
        }

        GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);

        string langStrHint1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_10");
        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", NowSclectPetID, "RosePet");
        langStrHint1 = "提示: 你正在分解宠物<color=#00700FFF>" + petName + "</color>;\n" + langStrHint1;
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint1, Btn_FangSheng, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否放生此宠物？", Btn_FangSheng, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
        
    }


    //放生按钮
    public void Btn_FangSheng() {

        Debug.Log("我点击了放生按钮：" + NowSclectPetID);
        //Game_PublicClassVar.Get_function_Rose.RosePetClearn(int.Parse(NowSclectPetID));

        //获取当前宠物数量
        if (Game_PublicClassVar.Get_function_Rose.GetRosePetNum() <= 0) {
            return;
        }


        //发送分解道具
        //判定是否为宝宝宠物(非宝宝获得1-3个,宝宝获得5-15个)
        string ifBaby = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", NowSclectPetID, "RosePet");
        if (ifBaby == "1")
        {

            int sendNum =  Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(5, 15);
            if (sendNum <= 5)
            {
                sendNum = 5;
            }

            Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000026", sendNum);
        }
        else {
            int sendNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 3);
            if (sendNum <= 1)
            {
                sendNum = 1;
            }
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000026", sendNum);
        }

        Game_PublicClassVar.Get_function_AI.Pet_ClearnData(NowSclectPetID);


        //读取战队
        string petTiaoZhanTeam = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanTeam", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (petTiaoZhanTeam.Contains(NowSclectPetID)) {
            petTiaoZhanTeam = petTiaoZhanTeam.Replace(NowSclectPetID,"");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetTiaoZhanTeam", petTiaoZhanTeam, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
        }

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
        Debug.Log("开始修改名称");
        if (Obj_PetNameInput.GetComponent<InputField>().text == "") {
            Obj_PetNameInput.GetComponent<InputField>().text = Obj_PetName.GetComponent<Text>().text;
            Obj_PetName.GetComponent<Text>().text = "";
        }
    }

    public void Btn_EndClickName()
    {
        Debug.Log("结束修改名称");
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

    //点击按钮
    public void Click_Type(string type)
    {
        Obj_Pet_ListSet.SetActive(false);
        Obj_Pet_HeChengSet.SetActive(false);
        Obj_Pet_XiLianSet.SetActive(false);
        Obj_Pet_PropertyAdd.SetActive(false);
        Obj_Pet_XiuLianSet.SetActive(false);

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        ObjBtn_Pet_List.GetComponent<Image>().sprite = img;
        ObjBtn_Pet_HeCheng.GetComponent<Image>().sprite = img;
        ObjBtn_Pet_XiLian.GetComponent<Image>().sprite = img;
        ObjBtn_Pet_XiuLian.GetComponent<Image>().sprite = img;

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
                Obj_Pet_XiLianSet.GetComponent<UI_PetXiLian>().Obj_XiLianNeedItem.SetActive(false);
                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_Pet_XiLian.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);
                break;

            //宠物修炼
            case "4":
                Obj_Pet_XiuLianSet.SetActive(true);
                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_Pet_XiuLian.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);
                break;
        }
    }

    //显示提示
    public void ShowPetNumHint() {

        //string showStr = "宠物栏位每15、25、35、45、55级均会新增一个宠物栏位！";
        string showStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_13"); 
        GameObject commonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
        commonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(showStr, null, null);
        commonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        commonHint.transform.localPosition = Vector3.zero;
        commonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    //宠物锁
    public void Btn_Lock() {

        //获取当前宠物的解锁状态
        string lockStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LockStatus", "ID", NowSclectPetID, "RosePet");
        if (lockStatus == "" || lockStatus == "0")
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LockStatus", "1", "ID", NowSclectPetID, "RosePet");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物已经锁定!");
            Obj_BtnPetLock.SetActive(true);
            Obj_BtnPetCostLock.SetActive(false);
        }
        else {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LockStatus", "0", "ID", NowSclectPetID, "RosePet");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物已经解锁!");
            Obj_BtnPetLock.SetActive(false);
            Obj_BtnPetCostLock.SetActive(true);
        }

        UpdateShowStatus = true;

    }
}
