using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class Main_100006 : MonoBehaviour {

    //创建怪物的列表
    public GameObject[] MonsterListSet;
    //当前层数
    public ObscuredInt NowCengShu;

    //创建怪物
    public Transform monsterCreatePosition;
    public bool CreateBossStatus;
    private ObscuredFloat CreateBossTime;
    private ObscuredFloat CreateBossTimeSum;

    //创建技能
    //public GameObject CreateSkillMonster;
    private ObscuredFloat CreateSkillMonsterTime;
    private ObscuredFloat CreateSkillMonsterTimeSum;
    private bool CreateSkillMonsterStatus;
    public GameObject CreateMonsterEffectObj;
    private ObscuredInt roseLv;

    //时间
    private ObscuredFloat MapTime;
    private ObscuredFloat MapTimeSum;
    private bool MapExitStatus;             //地图退出状态
    private ObscuredFloat OneTimeSum;       //1秒执行1次

    public GameObject Obj_ShaiZi;
    private GameObject shaiZiObj;
    private bool TouZhiEndStatus;
    private float TouZhiTime;

    private int MaxCengShu;

    //UI类
    public GameObject Obj_FuBen_ShangHaiShow;
    private GameObject FuBen_ShangHaiShowObj;


    // Use this for initialization
    void Start () {

        roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        //CreateSkillMonster.SetActive(false);

        //设定地图时间
        //MapTime = 360;
        FuBen_ShangHaiShowObj = (GameObject)Instantiate(Obj_FuBen_ShangHaiShow);
        FuBen_ShangHaiShowObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        FuBen_ShangHaiShowObj.transform.localPosition = new Vector3(0, 0, 0);
        FuBen_ShangHaiShowObj.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
        FuBen_ShangHaiShowObj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        FuBen_ShangHaiShowObj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        FuBen_ShangHaiShowObj.transform.localScale = new Vector3(1, 1, 1);
        FuBen_ShangHaiShowObj.GetComponent<UI_HuoDong_TowerShow>().Obj_Par = this.gameObject;

        //创建追踪怪物的时间
        CreateBossTime = 8.0f;

        //初始化,开启伤害统计状态
        //Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHai_Status = true;

        //隐藏任务栏
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseTask.SetActive(false);


        //默认场景存在 15分钟
        MapTime = 900;

        //默认一层
        string cengStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TowerCeng", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string TowerCengTimeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TowerCengTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

        if (cengStr == "" || cengStr == "0" || cengStr == null)
        {
            NowCengShu = 1;
            TowerCengTimeStr = MapTime.ToString();

        }
        else {
            NowCengShu = int.Parse(cengStr);
        }
        
        MaxCengShu = 100;           //设置最大层数

        if (TowerCengTimeStr == "" || TowerCengTimeStr == null)
        {
            TowerCengTimeStr = MapTime.ToString();
        }
        else {
            MapTime = int.Parse(TowerCengTimeStr);
        }

        FuBen_ShangHaiShowObj.GetComponent<UI_HuoDong_TowerShow>().fightTimeSum = MapTime - CreateBossTime;
        FuBen_ShangHaiShowObj.GetComponent<UI_HuoDong_TowerShow>().Obj_Ceng.GetComponent<Text>().text = "当前层数: " + NowCengShu + "/" + MaxCengShu;

        //地图显示
        string sceneName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", Application.loadedLevelName, "Scene_Template");
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MapName.GetComponent<Text>().text = sceneName + "第" + NowCengShu + "层";

        //按钮显示
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FightBtnSet.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FightPastureBtnSet.SetActive(false);         //有时候从牧场出来要隐藏一下
    }

    // Update is called once per frame
    void Update () {

        //创建技能怪物
        if (!CreateSkillMonsterStatus) {

            CreateSkillMonsterTimeSum = CreateSkillMonsterTimeSum + Time.deltaTime;

            FuBen_ShangHaiShowObj.GetComponent<UI_HuoDong_TowerShow>().Obj_UpdateMonsterTime.GetComponent<Text>().text = "战斗马上开启:" + (int)(CreateBossTime - CreateSkillMonsterTimeSum) + "秒";

            if (CreateSkillMonsterTimeSum >= CreateBossTime)
            {

                FuBen_ShangHaiShowObj.GetComponent<UI_HuoDong_TowerShow>().Obj_UpdateMonsterTime.GetComponent<Text>().text = "战斗开始";
                //CreateSkillMonsterTimeSum = 0;
                CreateSkillMonsterStatus = true;
                //CreateSkillMonster.SetActive(true);

                string TowerID = (10000 + NowCengShu).ToString();
                string MonsterSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterSet", "ID", TowerID, "HuoDong_Tower_Template");
                if (MonsterSetStr != "" && MonsterSetStr != null) {
                    string[] MonsterList = MonsterSetStr.Split(';');
                    //现在只处理召唤1个的,多的后再在处理,目前多的会重叠
                    for (int i = 0; i < MonsterList.Length; i++) {

                        string[] CreateMonsterList = MonsterList[i].Split(',');
                        if (CreateMonsterList.Length >= 2) {
                            GameObject monsterObj = (GameObject)Instantiate(Resources.Load("3DModel/MonsterModel/100006/" + CreateMonsterList[0], typeof(GameObject)));

                            monsterObj.transform.SetParent(monsterCreatePosition.transform);
                            monsterObj.transform.position = monsterCreatePosition.transform.position;           //设置创建位置
                            monsterObj.SetActive(false);
                            monsterObj.transform.localScale = new Vector3(1, 1, 1);
                            monsterObj.SetActive(true);

                            //Game_PublicClassVar.Get_function_AI.AI_CreatMonster(CreateMonsterList[0], monsterCreatePosition.transform.position, this.GetComponent<SkillObjBase>().MonsterSkillObj, monsterObj);

                            //播放特效
                            //GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill/" + "Eff_Skill_ZhaoHuan_4", typeof(GameObject));
                            GameObject SkillObject_p = (GameObject)Instantiate(CreateMonsterEffectObj);

                            //SkillObject_p.transform.position = monsterObj.transform.position;
                            SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
                            SkillObject_p.transform.SetParent(monsterObj.transform);
                            SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);

                            Destroy(SkillObject_p, 2);

                            //设置怪物血量
                            monsterObj.GetComponent<AI_Property>().AI_HpMax = ReturnCengShuMonsterHp();
                            monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_HpMax;
                            monsterObj.GetComponent<AI_Property>().AI_Act = ReturnCengShuMonsterAct();
                            monsterObj.GetComponent<AI_Property>().AI_MageAct = ReturnCengShuMonsterAct();
                            monsterObj.GetComponent<AI_Property>().AI_Lv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                            monsterObj.GetComponent<AI_Property>().NoUpdateProperty = true;

                            if (monsterObj.GetComponent<AI_1>().UI_Hp != null)
                            {
                                monsterObj.GetComponent<AI_1>().UI_Hp.SetActive(true);
                                //monsterObj.GetComponent<AI_Property>().AI_HpMax = ReturnCengShuMonsterHp();
                                //monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_HpMax;
                            }
                        }
                    }
                }

                if (FuBen_ShangHaiShowObj != null) {

                    FuBen_ShangHaiShowObj.GetComponent<UI_HuoDong_TowerShow>().FightStatus = true;
                    Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Pet = 0;
                    Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Rose = 0;

                }

                //隐藏筛子
                if (shaiZiObj != null) {

                    Destroy(shaiZiObj);

                }
            }
        }

        //显示地图时间
        MapTimeSum = MapTimeSum + Time.deltaTime;

        //地图超过时间回到主城            //缓冲5秒
        if (MapTimeSum >= MapTime+5.0f)
        {
            
            //删除怪物
            for (int i = 0; i < monsterCreatePosition.transform.childCount; i++)
            {
                GameObject go = monsterCreatePosition.transform.GetChild(i).gameObject;
                MonoBehaviour.Destroy(go);
            }

            //地图时间结束,将玩家返回地图
            if (!MapExitStatus) {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ReturnBuilding.GetComponent<UI_StartGame>().Btn_RetuenBuilding();
            }

            MapExitStatus = true;

        }


        if (TouZhiEndStatus) {

            TouZhiTime = TouZhiTime + Time.deltaTime;
            if (TouZhiTime >= 5)
            {
                TouZhiTime = 0;
                TouZhiEndStatus = false;

                int num = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 6);
                EnterNextCeng(num);

                //隐藏筛子
                if (shaiZiObj != null)
                {
                    Destroy(shaiZiObj);
                }
            }

            if (shaiZiObj != null) {

                if (shaiZiObj.GetComponent<TouZi>().currentType != PointType.None) {

                    int num = (int)(shaiZiObj.GetComponent<TouZi>().currentType);
                    if (num > 6)
                    {
                        num = 1;
                    }
                    TouZhiEndStatus = false;
                    TouZhiTime = 0;
                    EnterNextCeng(num);

                }
            }
        }
    }

    //注销时调用
    private void OnDestroy()
    {
        //初始化,开启伤害统计状态
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHai_Status = false;
        //关闭统计状态
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Rose = 0;
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Pet = 0;

        SaveTime();
    }

    //初始化战斗
    public void InitFuBen() {

        if (TouZhiEndStatus) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经投掷骰子,请稍后再试...");
            return;
        }

        if (CreateSkillMonsterTimeSum < CreateBossTime)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经投掷骰子,请等待刷新怪物...");
            return;
        }

        int monsterNum = 0;
        for (int i = 0; i < monsterCreatePosition.transform.childCount; i++)
        {
            GameObject go = monsterCreatePosition.transform.GetChild(i).gameObject;
            if (go.GetComponent<AI_1>() != null) {
                if (go.GetComponent<AI_1>().AI_Status != "5"&& go.active==true)
                {
                    monsterNum = monsterNum + 1;
                }
            }
        }

        //请先击杀场景里的怪物
        if (monsterNum >= 1) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先击杀场景里的怪物...");
            return;
        }



        /*
        if (CreateSkillMonster.GetComponent<AI_1>().ai_IfDeath == false) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先击败场内怪物...");
            return;
        }
        */

        //CreateSkillMonster.SetActive(false);

        CreateSkillMonsterTimeSum = 0;
        //CreateBossTime = 8;
        CreateSkillMonsterStatus = false;
        Debug.Log("初始化了战斗...");

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(monsterCreatePosition.gameObject);
        
        TouZhiEndStatus = true;
        TouZhiShaiZi();

        SaveTime();
    }

    //隐藏副本
    public void EndFuBen()
    {

        /*
        CreateSkillMonster.SetActive(false);
        if (CreateSkillMonster.GetComponent<AI_1>().UI_Hp != null) {
            CreateSkillMonster.GetComponent<AI_1>().UI_Hp.SetActive(false);
        }
        */

        for (int i = 0; i < Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj.Length; i++) {
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj[i] != null) {
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj[i].GetComponent<AIPet>().AI_Target = null;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj[i].GetComponent<AIPet>().ai_IfReturn = true;
            }
        }

        for (int i = 0; i < Game_PublicClassVar.Get_game_PositionVar.Obj_RoseCreatePetSet.transform.childCount; i++)
        {
            GameObject go = Game_PublicClassVar.Get_game_PositionVar.Obj_RoseCreatePetSet.transform.GetChild(i).gameObject;

            if (go != null)
            {
                if (go.GetComponent<AIPet>() != null) {
                    go.GetComponent<AIPet>().AI_Target = null;
                    go.GetComponent<AIPet>().ai_IfReturn = true;
                }
            }
        }

    }

    //摇色子 随机进入层数
    public void EnterNextCeng(int addInt) {

        //int randInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 6);
        //Debug.Log("掷骰子数:" + addInt);

        NowCengShu = NowCengShu + addInt;

        //最多100层

        if (NowCengShu >= MaxCengShu) {
            NowCengShu = MaxCengShu;
        }

        //显示地图名称
        GameObject enterMapName = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIEnterMapShowName);
        enterMapName.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        enterMapName.transform.localScale = new Vector3(1, 1, 1);
        string sceneName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", Application.loadedLevelName, "Scene_Template");
        enterMapName.GetComponent<UI_MapNameShow>().mapName = sceneName + "第" + NowCengShu + "层";
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MapName.GetComponent<Text>().text = sceneName + "第" + NowCengShu + "层";
        FuBen_ShangHaiShowObj.GetComponent<UI_HuoDong_TowerShow>().Obj_Ceng.GetComponent<Text>().text = "当前层数: " + NowCengShu + "/" + MaxCengShu;

        //记录层数
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TowerCeng", NowCengShu.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        

    }

    //根据层数和当前服务器系数 计算血量
    public int ReturnCengShuMonsterHp() {

        string TowerID = (10000 + NowCengShu).ToString();
        string monsterHp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterHp", "ID", TowerID, "HuoDong_Tower_Template");
        if (monsterHp == "" || monsterHp == null) {
            monsterHp = "0";
        }

        //获取等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        float hpPro = 1;
        if (roseLv >= 1 && roseLv <= 18) {
            hpPro = 1;
        }

        if (roseLv >= 19 && roseLv <= 29)
        {
            hpPro = 1.5f;
        }

        if (roseLv >= 30 && roseLv <= 39)
        {
            hpPro = 2;
        }

        if (roseLv >= 40 && roseLv <= 49)
        {
            hpPro = 2.5f;
        }

        if (roseLv >= 50)
        {
            hpPro = 3;
        }

        int returnHp = (int)(float.Parse(monsterHp) * hpPro);

        return returnHp;

    }



    //根据层数和当前服务器系数 计算攻击
    public int ReturnCengShuMonsterAct()
    {
        /*
        string TowerID = (10000 + NowCengShu).ToString();
        string monsterHp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterHp", "ID", TowerID, "HuoDong_Tower_Template");
        if (monsterHp == "" || monsterHp == null)
        {
            monsterHp = "0";
        }
        */

        //获取等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        float actValue = 100;
        if (roseLv >= 1 && roseLv <= 18)
        {
            actValue = 150;
        }

        if (roseLv >= 19 && roseLv <= 29)
        {
            actValue = 300;
        }

        if (roseLv >= 30 && roseLv <= 39)
        {
            actValue = 450;
        }

        if (roseLv >= 40 && roseLv <= 49)
        {
            actValue = 600;
        }

        if (roseLv >= 50)
        {
            actValue = 800;
        }

        float actPro = 1 + ((float)(NowCengShu) / 100.0f);

        int returnAct = (int)(actValue * actPro);

        return returnAct;

    }



    private void TouZhiShaiZi() {

        if (shaiZiObj != null)
        {
            Destroy(shaiZiObj);
        }

        shaiZiObj = (GameObject)Instantiate(Obj_ShaiZi);
        shaiZiObj.GetComponent<Rigidbody>().useGravity = false;
        shaiZiObj.transform.localScale = new Vector3(40, 40, 40);
        shaiZiObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        shaiZiObj.transform.position = new Vector3(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.x, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.y + 1, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.z);
        shaiZiObj.GetComponent<Rigidbody>().useGravity = true;
        shaiZiObj.GetComponent<TouZi>().TouZhiStatus = true;

    }

    private void SaveTime() {

        int WriteTime = (int)(MapTime - MapTimeSum);
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TowerCengTime", WriteTime.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
    }


}
