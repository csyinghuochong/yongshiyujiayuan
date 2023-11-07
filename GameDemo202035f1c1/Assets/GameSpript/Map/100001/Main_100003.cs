using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class Main_100003 : MonoBehaviour {

    //创建怪物的列表
    public GameObject[] MonsterListSet;

    //宝箱实例化
    public GameObject Obj_Chest_1;              //真实宝箱
    public GameObject Obj_Chest_2;              //中毒宝箱
    public GameObject Obj_Chest_3;              //创建怪物宝箱
    public GameObject Obj_ChestSet;
    public ObscuredInt ChestTrueNum;            //真实宝箱数量
    public ObscuredInt NowChestTrueNum;         //当前真实宝箱数量
    public GameObject[] ChestTrueList;

    //public ArrayList ChestTrueListCount = new ArrayList();

    //创建怪物
    public Transform monsterCreatePosition;
    public bool CreateBossStatus;
    private ObscuredFloat CreateBossTime;
    private ObscuredFloat CreateBossTimeSum;

    //创建技能
    public GameObject CreateSkillMonster;
    private ObscuredFloat CreateSkillMonsterTime;
    private ObscuredFloat CreateSkillMonsterTimeSum;
    private bool CreateSkillMonsterStatus;

    private ObscuredInt roseLv;
    private ObscuredString createMonsterID;
    private ObscuredInt createXuHaoID;

    public Vector3[] ChestPositionVec3 = new Vector3[30];

    //时间
    private ObscuredFloat MapTime;
    private ObscuredFloat MapTimeSum;
    private bool MapExitStatus;     //地图退出状态
    private ObscuredFloat OneTimeSum;       //1秒执行1次

    //UI类
    public GameObject Obj_MapTime;
    private GameObject mapTimeObj;
    private GameObject mapTimeShowObj;
    private GameObject mapChestNumObj;

    //初始化宝箱位置显示
    public void IniztiaChestData()
    {
        ChestPositionVec3[0] = new Vector3(149.46f, 28.8f, -56.99f);
        ChestPositionVec3[1] = new Vector3(149.46f, 28.8f, -44.24f);
        ChestPositionVec3[2] = new Vector3(149.46f, 28.8f, -31.79f);
        ChestPositionVec3[3] = new Vector3(162.33f, 28.8f, -56.99f);
        ChestPositionVec3[4] = new Vector3(162.33f, 28.8f, -44.24f);
        ChestPositionVec3[5] = new Vector3(162.33f, 28.8f, -31.79f);
        ChestPositionVec3[6] = new Vector3(176.41f, 28.8f, -77.24f);
        ChestPositionVec3[7] = new Vector3(176.81f, 28.8f, -44.24f);
        ChestPositionVec3[8] = new Vector3(175.86f, 28.8f, -27.17f);
        ChestPositionVec3[9] = new Vector3(175.86f, 28.8f, -11.69f);
        ChestPositionVec3[10] = new Vector3(176.41f, 28.8f, -61.1f);
        ChestPositionVec3[11] = new Vector3(190.5f, 28.8f, -77.24f);
        ChestPositionVec3[12] = new Vector3(190.9f, 28.8f, -44.24f);
        ChestPositionVec3[13] = new Vector3(189.69f, 28.8f, -27.17f);
        ChestPositionVec3[14] = new Vector3(189.95f, 28.8f, -11.69f);
        ChestPositionVec3[15] = new Vector3(190.5f, 28.8f, -61.1f);
        ChestPositionVec3[16] = new Vector3(206.01f, 28.8f, -77.24f);
        ChestPositionVec3[17] = new Vector3(206.41f, 28.8f, -44.24f);
        ChestPositionVec3[18] = new Vector3(201.3f, 28.8f, -27.17f);
        ChestPositionVec3[19] = new Vector3(205.46f, 28.8f, -11.69f);
        ChestPositionVec3[20] = new Vector3(201.6f, 28.8f, -53.6f);
        ChestPositionVec3[21] = new Vector3(217.01f, 28.8f, -77.24f);
        ChestPositionVec3[22] = new Vector3(-217.41f, 28.8f, -44.24f);
        ChestPositionVec3[23] = new Vector3(216.2f, 28.8f, -27.17f);
        ChestPositionVec3[24] = new Vector3(216.46f, 28.8f, -11.69f);
        ChestPositionVec3[25] = new Vector3(217.01f, 28.8f, -61.1f);
        ChestPositionVec3[26] = new Vector3(233.2f, 28.8f, -52.9f);
        ChestPositionVec3[27] = new Vector3(231.99f, 28.8f, -35.83f);
        ChestPositionVec3[28] = new Vector3(248.51f, 28.8f, -52.9f);
        ChestPositionVec3[29] = new Vector3(-247.3f, 28.8f, -35.83f);


        //根据等级获取刷新怪物的序号
        createXuHaoID = 0;
        if (roseLv >= 1)
        {
            createXuHaoID = 0;
            createMonsterID = "71001021";
        }

        if (roseLv >= 20)
        {
            createXuHaoID = 1;
            createMonsterID = "71001022";
        }


        if (roseLv >= 30)
        {
            createXuHaoID = 2;
            createMonsterID = "71001023";
        }

        if (roseLv >= 40)
        {
            createXuHaoID = 3;
            createMonsterID = "71001024";
        }

        if (roseLv >= 50)
        {
            createXuHaoID = 4;
            createMonsterID = "71001025";
        }

    }


    // Use this for initialization
    void Start () {

        roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        CreateSkillMonster.SetActive(false);

        //当前场景内存在6个真实宝箱
        ChestTrueNum = 6;
        ChestTrueList = new GameObject[ChestTrueNum];

        //设定地图时间
        MapTime = 360;
        mapTimeObj = (GameObject)Instantiate(Obj_MapTime);
        mapTimeObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        mapTimeObj.transform.localPosition = new Vector3(0, 0, 0);
        mapTimeObj.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
        mapTimeObj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        mapTimeObj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        mapTimeObj.transform.localScale = new Vector3(1, 1, 1);

        if (mapTimeObj != null) {
            mapTimeShowObj = mapTimeObj.GetComponent<UI_MapTime>().Obj_MapTime;
            string langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("地图剩余时间");
            string langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("秒");
            mapTimeShowObj.GetComponent<Text>().text = langstr_1 + ":" + MapTime + langstr_2;

        }

        mapChestNumObj = mapTimeObj.GetComponent<UI_MapTime>().Obj_MapChestNum;
        string langstr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("寻找到真实宝箱");
        mapChestNumObj.GetComponent<Text>().text = langstr_3 + "0" + "/" + ChestTrueNum;

        //创建追踪怪物的时间
        CreateBossTime = 10.0f;

        //创建技能时间 进入地图15秒触发 
        CreateSkillMonsterTime = 15.0f;

        //初始化坐标数据
        IniztiaChestData();
        //随机场景中的6个宝箱为真实宝箱
        for (int i = 0; i < ChestTrueNum; i++) {
            Vector3 vec3 = GetChestVec3();
            GameObject chestObj = (GameObject)Instantiate(Obj_Chest_1);
            chestObj.transform.SetParent(Obj_ChestSet.transform);
            chestObj.transform.position = vec3;
            chestObj.transform.localScale = new Vector3(1, 1, 1);
            chestObj.GetComponent<UI_GetherItem>().SceneItmeID = "42000001";
            chestObj.GetComponent<UI_GetherItem>().GetHerItemUpdateData();
            ChestTrueList[i] = chestObj;
        }

        //随机场景中的14个宝箱为假的宝箱(创建怪物)
        for (int i = 0; i < 10; i++)
        {
            Vector3 vec3 = GetChestVec3();
            GameObject chestObj = (GameObject)Instantiate(Obj_Chest_2);
            chestObj.transform.SetParent(Obj_ChestSet.transform);
            chestObj.transform.position = vec3;
            chestObj.transform.localScale = new Vector3(1, 1, 1);
            chestObj.GetComponent<UI_GetherItem>().SceneItmeID = "42000002";
            chestObj.GetComponent<UI_GetherItem>().GetHerItemUpdateData();
        }

        //随机场景中的14个宝箱为假的宝箱(创建怪物)
        for (int i = 0; i < 20; i++)
        {
            Vector3 vec3 = GetChestVec3();
            GameObject chestObj = (GameObject)Instantiate(Obj_Chest_3);
            chestObj.transform.SetParent(Obj_ChestSet.transform);
            chestObj.transform.position = vec3;
            chestObj.transform.localScale = new Vector3(1, 1, 1);
            chestObj.GetComponent<UI_GetherItem>().SceneItmeID = "42000003";
            chestObj.GetComponent<UI_GetherItem>().CreateMonsterID = createMonsterID;
            chestObj.GetComponent<UI_GetherItem>().GetHerItemUpdateData();
        }
    }
	
	// Update is called once per frame
	void Update () {
        /*
        CreateBossTimeSum = CreateBossTimeSum + Time.deltaTime;

        //根据时间创建追踪怪物
        if (CreateBossTimeSum >= CreateBossTime) {
            CreateBossTimeSum = 0;
            CreateBossStatus = true;
        }
        if (CreateBossStatus) {
            CreateBossStatus = false;
            //单次创建5个怪
            for (int i = 0; i < 5; i++) {
                CreateMonster();
            }
        }
        */

        //创建技能怪物
        if (!CreateSkillMonsterStatus) {
            CreateSkillMonsterTimeSum = CreateSkillMonsterTimeSum + Time.deltaTime;
            if (CreateSkillMonsterTimeSum >= CreateBossTime)
            {
                CreateSkillMonsterTimeSum = 0;
                CreateSkillMonsterStatus = true;
                CreateSkillMonster.SetActive(true);
            }
        }

        //显示地图时间
        MapTimeSum = MapTimeSum + Time.deltaTime;
        //1秒计时
        OneTimeSum = OneTimeSum + Time.deltaTime;

        if (OneTimeSum >= 1) {
            OneTimeSum = 0;

            int nowMapTime = (int)(MapTime - MapTimeSum);
            if (mapTimeShowObj != null)
            {
                if (nowMapTime >= 0)
                {
                    string langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("地图剩余时间");
                    string langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("秒");
                    mapTimeShowObj.GetComponent<Text>().text = langstr_1 + nowMapTime + langstr_2;
                }
                else
                {
                    string langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("正在退出地图");
                    mapTimeShowObj.GetComponent<Text>().text = langstr_2;
                }
            }

            //显示地图宝箱数量
            NowChestTrueNum = GetChestTrue();
            mapChestNumObj = mapTimeObj.GetComponent<UI_MapTime>().Obj_MapChestNum;
            string langstr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("寻找到真实宝箱");
            mapChestNumObj.GetComponent<Text>().text = langstr_3 + ":" + (ChestTrueNum - NowChestTrueNum).ToString() + "/" + ChestTrueNum;
            //mapChestNumObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);

        }

        //地图超过时间回到主城
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
    }

    //注销时调用
    private void OnDestroy()
    {
        //存储当前寻找的宝箱数
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MeiRiCangBaoTrueChestNum", (ChestTrueNum - NowChestTrueNum).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
    }

    //创建一个怪物
    public void CreateMonster() {

        if (MapExitStatus) {
            return;
        }

        //刷新怪物
        GameObject monsterNow = (GameObject)Instantiate(MonsterListSet[createXuHaoID]);
        //设置怪物的父级和坐标点
        monsterNow.transform.SetParent(monsterCreatePosition);

        //出现位置随机
        float ran_x = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0, 10);
        float ran_z = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0, 10);
        ran_x = ran_x - 5;
        ran_z = ran_z - 5;

        monsterNow.transform.localPosition = new Vector3(-55 + ran_x, 1, -22 + ran_z);

        monsterNow.SetActive(false);
        monsterNow.SetActive(true);

    }

    //随机一个宝箱坐标
    public Vector3 GetChestVec3()
    {
        int intNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, 29);
        Vector3 vec3 = ChestPositionVec3[intNum];

        float ran_x = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0,10);
        float ran_z = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0,10);
        ran_x = ran_x - 5;
        ran_z = ran_z - 5;

        Vector3 ChestVec3 = new Vector3();
        ChestVec3.x = vec3.x + (int)(ran_x);
        ChestVec3.y = vec3.y;
        ChestVec3.z = vec3.z + (int)(ran_z);

        return ChestVec3;
    }

    //获取当前真实宝箱数量
    public int GetChestTrue() {
        int sum = 0;
        for (int i = 0; i < ChestTrueList.Length; i++) {
            if (ChestTrueList[i] != null) {
                sum = sum + 1;
            }
        }
        return sum;
    }

}
