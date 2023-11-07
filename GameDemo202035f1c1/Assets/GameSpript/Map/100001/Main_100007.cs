using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class Main_100007 : MonoBehaviour {

    //创建怪物的列表
    public GameObject[] MonsterListSet_1;
    public GameObject[] MonsterListSet_2;
    public GameObject[] MonsterListSet_3;
    public GameObject[] MonsterListSet_4;
    public GameObject[] MonsterListSet_5;
    public GameObject[] MonsterGuanDiListSet;           //创建官底BOSS
    public GameObject[] ChestMonsterListSet;            //宝箱怪物
    public GameObject[] CreateComMonsterListSet;        //创建普通怪物
    public GameObject[] ChestListSet;                   //创建宝箱

    //当前层数
    public ObscuredInt NowCengShu;
    public ObscuredBool ZuanShiCreate;
    public string EWaiDropID;

    //创建怪物
    public Transform monsterCreatePosition;
    public bool CreateBossStatus;
    private ObscuredFloat CreateBossTime;
    private ObscuredFloat CreateBossTimeSum;

    //创建特效
    public GameObject Obj_ChestEffect;

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

    private ObscuredInt MaxCengShu;

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
        FuBen_ShangHaiShowObj.GetComponent<UI_FengYinZhiTaShow>().Obj_Par = this.gameObject;

        //创建追踪怪物的时间
        CreateBossTime = 8.0f;
        CreateSkillMonsterTimeSum = CreateBossTime;

        //初始化,开启伤害统计状态
        //Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHai_Status = true;

        //隐藏任务栏
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseTask.SetActive(false);

        //默认场景存在 15分钟
        MapTime = 900;

        //默认一层
        string cengStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FengYinCeng", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (cengStr == "" || cengStr == "0" || cengStr == null)
        {
            NowCengShu = 1;
        }
        else
        {
            NowCengShu = int.Parse(cengStr);
        }

        MaxCengShu = 100;           //设置最大层数

        //FuBen_ShangHaiShowObj.GetComponent<UI_HuoDong_TowerShow>().fightTimeSum = MapTime;
        FuBen_ShangHaiShowObj.GetComponent<UI_FengYinZhiTaShow>().Obj_Ceng.GetComponent<Text>().text = "当前层数: " + NowCengShu;

        //地图显示
        string sceneName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", Application.loadedLevelName, "Scene_Template");
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MapName.GetComponent<Text>().text = sceneName + "第" + NowCengShu + "层";

        CreateSkillMonsterStatus = true;

    }

    // Update is called once per frame
    void Update () {

        //创建技能怪物
        if (!CreateSkillMonsterStatus) {

            CreateSkillMonsterTimeSum = CreateSkillMonsterTimeSum + Time.deltaTime;

            FuBen_ShangHaiShowObj.GetComponent<UI_FengYinZhiTaShow>().Obj_UpdateMonsterTime.GetComponent<Text>().text = "战斗马上开启:" + (int)(CreateBossTime - CreateSkillMonsterTimeSum) + "秒";

            if (CreateSkillMonsterTimeSum >= CreateBossTime)
            {

                FuBen_ShangHaiShowObj.GetComponent<UI_FengYinZhiTaShow>().Obj_UpdateMonsterTime.GetComponent<Text>().text = "战斗开始";
                //CreateSkillMonsterTimeSum = 0;
                CreateSkillMonsterStatus = true;
                //CreateSkillMonster.SetActive(true);

                string TowerID = (10000 + NowCengShu).ToString();

                //创建
                ReturenMonster();

                //隐藏筛子
                if (shaiZiObj != null) {

                    Destroy(shaiZiObj);

                }
            }
        }

        //显示地图时间
        MapTimeSum = MapTimeSum + Time.deltaTime;

        //地图超过时间回到主城
        /*
        if (MapTimeSum >= MapTime)
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
        */

        if (TouZhiEndStatus) {

            TouZhiTime = TouZhiTime + Time.deltaTime;
            //5秒累积
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

        //SaveTime();
    }

    //初始化战斗
    public bool InitFuBen() {

        if (TouZhiEndStatus) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经投掷骰子,请稍后再试...");
            return false;
        }

        if (CreateSkillMonsterTimeSum < CreateBossTime)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经投掷骰子,请等待刷新怪物...");
            return false;
        }

        if (NowCengShu >= MaxCengShu) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("勇士,你今天非常棒！已经到达最后一关,请明日再来！");
            return false;
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

            if (go.GetComponent<UI_GetherItem>() != null)
            {
                monsterNum = monsterNum + 1;
            }
        }

        //请先击杀场景里的怪物
        if (monsterNum >= 1) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先击杀场景里的怪物...");
            return false;
        }


        CreateSkillMonsterTimeSum = 0;
        //CreateBossTime = 8;
        CreateSkillMonsterStatus = false;
        Debug.Log("初始化了战斗...");

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(monsterCreatePosition.gameObject);
        
        TouZhiEndStatus = true;
        TouZhiShaiZi();

        
        //记录层数
        string nowFengYinCengNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FengYinCengNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (nowFengYinCengNum == "" || nowFengYinCengNum == null) {
            nowFengYinCengNum = "0";
        }


        //存储今日次数
        int writeInt = int.Parse(nowFengYinCengNum) + 1;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FengYinCengNum", writeInt.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
        return true;
    }

    //隐藏副本
    public void EndFuBen()
    {

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
        FuBen_ShangHaiShowObj.GetComponent<UI_FengYinZhiTaShow>().Obj_Ceng.GetComponent<Text>().text = "当前层数: " + NowCengShu + "/" + MaxCengShu;
        FuBen_ShangHaiShowObj.GetComponent<UI_FengYinZhiTaShow>().UpdateZuanShi(NowCengShu);

        //记录层数
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FengYinCeng", NowCengShu.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

    }

    //根据层数和当前服务器系数 计算血量
    public int ReturnCengShuMonsterHp() {

        //获取等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        int hp = 1;
        if (roseLv >= 1 && roseLv <= 18) {
            hp = 35000;
        }

        if (roseLv >= 19 && roseLv <= 29)
        {
            hp = 120000;
        }

        if (roseLv >= 30 && roseLv <= 39)
        {
            hp = 240000;
        }

        if (roseLv >= 40 && roseLv <= 49)
        {
            hp = 400000;
        }

        if (roseLv >= 50)
        {
            hp = 600000;
        }

        if (roseLv >= 60)
        {
            hp = 1000000;
        }

        return hp;

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


    //投掷骰子
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

    //保存时间
    /*
    private void SaveTime() {

        int WriteTime = (int)(MapTime - MapTimeSum);
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TowerCengTime", WriteTime.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
    }
    */


    //根据等级和层数返回当前怪物
    public void ReturenMonster() {

        //获取当前层级
        bool chestStatus = false;
        bool lastBossStatus = false;

        //宝箱层
        if (NowCengShu.ToString().Substring(NowCengShu.ToString().Length - 1, 1) == "0") {
            chestStatus = true;
        }

        //官底BOSS层
        if (NowCengShu.ToString().Substring(NowCengShu.ToString().Length - 1, 1) == "6") {
            lastBossStatus = true;
        }


        //获取当前等级
        int xuhao = 0;
        string randomMonsterStr = "";
        GameObject[] CreateObjList;
        if (roseLv >= 1 && roseLv <= 18)
        {
            xuhao = 0;
            CreateObjList = MonsterListSet_1;
        }

        //获取当前等级
        if (roseLv >= 19 && roseLv <= 29)
        {
            xuhao = 1;
            CreateObjList = MonsterListSet_2;
        }

        //获取当前等级
        if (roseLv >= 30 && roseLv <= 39)
        {
            xuhao = 2;
            CreateObjList = MonsterListSet_3;
        }

        //获取当前等级
        if (roseLv >= 40 && roseLv <= 49)
        {
            xuhao = 3;
            CreateObjList = MonsterListSet_4;
        }

        //获取当前等级
        if (roseLv >= 50)
        {
            xuhao = 4;
            CreateObjList = MonsterListSet_5;
        }

        //召唤宝箱
        if (chestStatus) {

            CreateMonster("1", ChestListSet[xuhao]);
      
            return;
        }

        //召唤官底BOSS
        if (lastBossStatus) {

            CreateMonster("2", MonsterGuanDiListSet[xuhao]);
            return;
        }

        //从剩下的集中状态随机获取
        float randValue = Random.value;

        if (ZuanShiCreate) {
            //钻石开启 必出怪物
            randValue = 0;
        }

        //boss
        if (randValue >= 0 && randValue < 0.5f) {

            //string[] monsterList = randomMonsterStr.Split(';');
            int xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_1.Length-1);
            CreateRandomMonster(xuhao.ToString());
        }

        //金币
        if (randValue >= 0.5f && randValue < 0.75f)
        {
            //召唤金币怪物
            CreateMonster("2", ChestMonsterListSet[xuhao]);
        }

        //一群小怪
        if (randValue >= 0.75f && randValue <= 1f)
        {
            //召唤金币怪物
            CreateMonster("3", CreateComMonsterListSet[xuhao]);
        }

        if (ZuanShiCreate) {
            ZuanShiCreate = false;
        }
    }


    //获取层数
    public void CreateRandomMonster(string createType) {

        switch (createType) {

            case "0":

                int xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_1.Length - 1);
                //召唤怪物
                EWaiDropID = "51000251";
                
                CreateMonster("2", MonsterListSet_1[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());

                break;

            case "1":

                xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_2.Length - 1);
                //召唤怪物
                EWaiDropID = "51000261";
                float randValue = Random.value;
                if (randValue >= 0.5f) {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_1.Length - 1);
                    CreateMonster("2", MonsterListSet_1[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }
                if (randValue >= 0f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_2.Length - 1);
                    CreateMonster("2", MonsterListSet_2[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                break;

            case "2":

                xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_3.Length - 1);
                //召唤怪物
                EWaiDropID = "51000271";
                //CreateMonster("2", MonsterListSet_3[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                randValue = Random.value;
                if (randValue >= 0.7f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_1.Length - 1);
                    CreateMonster("2", MonsterListSet_1[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                if (randValue >= 0.4f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_2.Length - 1);
                    CreateMonster("2", MonsterListSet_2[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                if (randValue >= 0f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_3.Length - 1);
                    CreateMonster("2", MonsterListSet_3[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                break;

            case "3":

                xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_4.Length - 1);
                //召唤怪物
                EWaiDropID = "51000281";
                randValue = Random.value;

                if (randValue >= 0.7f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_1.Length - 1);
                    CreateMonster("2", MonsterListSet_1[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                if (randValue >= 0.5f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_2.Length - 1);
                    CreateMonster("2", MonsterListSet_2[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                if (randValue >= 0.25f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_3.Length - 1);
                    CreateMonster("2", MonsterListSet_3[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                if (randValue >= 0f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_4.Length - 1);
                    CreateMonster("2", MonsterListSet_4[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }
                break;

            case "4":

                xuhaoInt = 1;
                //召唤怪物
                EWaiDropID = "51000291";
                //CreateMonster("2", MonsterListSet_5[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());

                randValue = Random.value;
                if (randValue >= 0.8f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_1.Length - 1);
                    CreateMonster("2", MonsterListSet_1[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                if (randValue >= 0.6f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_2.Length - 1);
                    CreateMonster("2", MonsterListSet_2[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                if (randValue >= 0.4f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_3.Length - 1);
                    CreateMonster("2", MonsterListSet_3[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                if (randValue >= 0.2f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_4.Length - 1);
                    CreateMonster("2", MonsterListSet_4[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                if (randValue >= 0f)
                {
                    xuhaoInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, MonsterListSet_5.Length - 1);
                    CreateMonster("2", MonsterListSet_5[xuhaoInt], ReturnCengShuMonsterHp(), ReturnCengShuMonsterAct());
                    return;
                }

                break;
        }

    }


    //创建怪物
    public void CreateMonster(string createType , GameObject createObj,int monsterHp=0,int monsterAct = 0)
    {

        GameObject monsterObj = (GameObject)Instantiate(createObj);

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

        //创建类型为1表是
        if (createType == "1") {
            //播放特效
            GameObject chestEffect = (GameObject)Instantiate(Obj_ChestEffect);
            chestEffect.transform.localScale = new Vector3(1, 1, 1);
            chestEffect.transform.SetParent(monsterObj.transform);
            chestEffect.transform.localPosition = new Vector3(0.2f, 0, -0.55f);
        }

        //创建类型为2表示创建的怪物
        if (createType == "2") {

            monsterObj.GetComponent<AI_Property>().AI_MonsterCreateType = "2";
            monsterObj.GetComponent<AI_1>().CreateEWaoDropID = EWaiDropID;

            //设置怪物属性
            if (monsterHp != 0)
            {
                monsterObj.GetComponent<AI_Property>().AI_HpMax = monsterHp;
                monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_HpMax;
                monsterObj.GetComponent<AI_Property>().NoUpdateProperty = true;
            }

            if (monsterAct != 0)
            {
                monsterObj.GetComponent<AI_Property>().AI_Act = monsterAct;
                monsterObj.GetComponent<AI_Property>().AI_MageAct = monsterAct;
                monsterObj.GetComponent<AI_Property>().NoUpdateProperty = true;
            }

            monsterObj.GetComponent<AI_Property>().AI_Lv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

            if (monsterObj.GetComponent<AI_1>().UI_Hp != null)
            {
                monsterObj.GetComponent<AI_1>().UI_Hp.SetActive(true);
                //monsterObj.GetComponent<AI_Property>().AI_HpMax = ReturnCengShuMonsterHp();
                //monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_HpMax;
            }
        }

        //表示随机创建10个小怪
        if (createType == "3") {
            for (int i = 0; i < 10; i++) {

                float ranValue = Random.value * 20;
                ranValue = ranValue - 10;

                monsterObj = (GameObject)Instantiate(createObj);

                monsterObj.transform.SetParent(monsterCreatePosition.transform);
                monsterObj.transform.position = new Vector3(monsterCreatePosition.transform.position.x + ranValue, monsterCreatePosition.transform.position.y, monsterCreatePosition.transform.position.z + ranValue);           //设置创建位置
                monsterObj.SetActive(false);
                monsterObj.transform.localScale = new Vector3(1, 1, 1);
                monsterObj.SetActive(true);
                monsterObj.GetComponent<AI_Property>().AI_MonsterCreateType = "2";

                //Game_PublicClassVar.Get_function_AI.AI_CreatMonster(CreateMonsterList[0], monsterCreatePosition.transform.position, this.GetComponent<SkillObjBase>().MonsterSkillObj, monsterObj);

                //播放特效
                //GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill/" + "Eff_Skill_ZhaoHuan_4", typeof(GameObject));
                SkillObject_p = (GameObject)Instantiate(CreateMonsterEffectObj);

                //SkillObject_p.transform.position = monsterObj.transform.position;
                SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
                SkillObject_p.transform.SetParent(monsterObj.transform);
                SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);
                Destroy(SkillObject_p, 2);

            }
        }

    }

}
