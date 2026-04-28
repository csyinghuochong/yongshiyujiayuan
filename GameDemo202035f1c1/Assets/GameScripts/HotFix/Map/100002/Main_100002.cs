using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class Main_100002 : MonoBehaviour {

    public ObscuredInt MiJingLv;                        //秘境等级
    private ObscuredInt MiJingMaxValie;
    public GameObject Obj_MiJingBoss;           //秘境BOSS
    public ObscuredBool MiJingBossStatus;               //秘境BOSS状态       
    public AI_1 MiJingBoss_AI_1;
    public ObscuredBool MiJingSendRewardStatus;         //秘境发送奖励状态
    private Rose_Proprety rose_Proprety;

    //创建怪物
    public Transform monsterCreatePosition;
    public ObscuredBool CreateBossStatus;
    private ObscuredFloat CreateBossTime;
    private ObscuredFloat CreateBossTimeSum;
    public GameObject[] MonsterListSet;         //创建怪物的列表
    public List<GameObject> CreateMonsterList;
    public Vector3[] CreatePositionVec3 = new Vector3[25];
    public GameObject[] CreatePositionObj = new GameObject[25];
    public GameObject Obj_CreateMonsterEffect;

    //时间
    private ObscuredFloat MapTime;
    private ObscuredFloat MapTimeSum;
    private ObscuredBool MapExitStatus;         //地图退出状态
    private ObscuredFloat OneTimeSum;           //1秒执行1次
    private ObscuredFloat guangboSum;

    //UI类
    public GameObject Obj_MapTime;
    private GameObject mapTimeObj;
    private GameObject mapTimeShowObj;
    private GameObject mapChestNumObj;

    // Use this for initialization
    void Start()
    {
        Init();
        CreateBossTime = 10;                    //10秒创建怪物

        //隐藏BOSS
        Obj_MiJingBoss.SetActive(false);

        //进入地图创建20个怪物
        for (int i = 0; i < 20; i++)
        {
            CreateMonster();
        }

        MiJingBoss_AI_1 = Obj_MiJingBoss.GetComponent<AI_1>();
        rose_Proprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();

        MapTime = 600;          //地图时间600秒
        MiJingMaxValie = 500;     //秘境最大值,召唤BOSS,正常500
        mapTimeObj = (GameObject)Instantiate(Obj_MapTime);
        mapTimeObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        //mapTimeObj.transform.localPosition = new Vector3(0, 0, 0);
        mapTimeObj.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
        mapTimeObj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        mapTimeObj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        mapTimeObj.transform.localScale = new Vector3(1, 1, 1);

        mapTimeShowObj = mapTimeObj.GetComponent<UI_MapTime>().Obj_MapTime;
        string langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("地图剩余时间");
        string langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("秒");
        mapTimeShowObj.GetComponent<Text>().text = langstr_1 + ":" + MapTime + langstr_2;

        mapChestNumObj = mapTimeObj.GetComponent<UI_MapTime>().Obj_MapChestNum;
        string langstr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("秘境值");
        mapChestNumObj.GetComponent<Text>().text = langstr_3 + ":" + "0" + "/" + MiJingMaxValie.ToString();

        //根据秘境等级确定怪物实力
        string daMiJingLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (daMiJingLv == "") {
            daMiJingLv = "1";
        }
        MiJingLv = int.Parse(daMiJingLv);

    }

    void Init() {

        Debug.Log("CreatePositionVec3.length = " + CreatePositionVec3.Length);
        for (int i = 0; i < CreatePositionVec3.Length; i++) {
            CreatePositionVec3[i] = CreatePositionObj[i].transform.position;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        CreateBossTimeSum = CreateBossTimeSum + Time.deltaTime;

        //根据时间创建追踪怪物
        if (CreateBossTimeSum >= CreateBossTime)
        {
            CreateBossTimeSum = 0;
            CreateBossStatus = true;
        }
        if (CreateBossStatus)
        {
            CreateBossStatus = false;
            //单次创建5个怪
            Debug.Log("当前场景怪物:" + monsterCreatePosition.transform.childCount);
            if (monsterCreatePosition.transform.childCount <= 100)
            {
                for (int i = 0; i < 5; i++)
                {

                    CreateMonster();
                }
            }
        }



        if (!MiJingBossStatus)
        {
            //Debug.Log("rose_Proprety.Rose_MijingValue = " + rose_Proprety.Rose_MijingValue);

            if (rose_Proprety.Rose_MijingValue >= MiJingMaxValie)
            {
                MiJingBossStatus = true;

                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_195");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("BOSS出现！！！");

                //显示BOSS
                Obj_MiJingBoss.SetActive(true);

            }
        }

        //检测BOSS是否死亡
        if (MiJingBossStatus)
        {
            if (MiJingBoss_AI_1.ai_IfDeath)
            {
                //秘境发送奖励,只执行一次
                if (!MiJingSendRewardStatus)
                {
                    MiJingSendRewardStatus = true;

                    string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_196");
                    string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_197");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + MiJingLv + langStrHint_2);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你!挑战" + MiJingLv + "层秘境成功!");

                    //记录当前秘境奖励等级
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJingRewardLv", MiJingLv.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    //记录当前秘境等级
                    MiJingLv = MiJingLv + 1;
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJingLv", MiJingLv.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                    
                    //记录调整当前层用的时间
                    /*
                    string tiaozhanTimeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingLvKillTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    if (tiaozhanTimeStr == "" || tiaozhanTimeStr == null) {
                        tiaozhanTimeStr = "0";
                    }
                    float tiaozhanTime = float.Parse(tiaozhanTimeStr);
                    if (tiaozhanTime < MapTimeSum) {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJingLvKillTime", MapTimeSum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    }
                    */

                    int writeTime = (int)(MapTimeSum);
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJingLvKillTime", writeTime.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

                    //清空秘境次数
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJing_DayNum", "0","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                    //循环删除场景内的其他怪物
                    foreach (GameObject nowObj in CreateMonsterList) {
                        if (nowObj != null) {
                            Destroy(nowObj);
                        }
                    }

                    //上传角色自身全部数据

                }

                //每3秒广播一次 撤离现场
                guangboSum = guangboSum + Time.deltaTime;
                if (guangboSum >= 3) {

                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_198");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经击杀BOSS！你现在可以返回地图挑战下一层秘境！");
                }
            }
        }



        //显示地图时间
        MapTimeSum = MapTimeSum + Time.deltaTime;
        //1秒计时
        OneTimeSum = OneTimeSum + Time.deltaTime;

        if (OneTimeSum >= 1)
        {
            OneTimeSum = 0;

            int nowMapTime = (int)(MapTime - MapTimeSum);
            if (mapTimeShowObj != null)
            {
                if (nowMapTime >= 0)
                {
                    string langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("地图剩余时间");
                    string langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("秒");
                    mapTimeShowObj.GetComponent<Text>().text = langstr_1 + ":" + nowMapTime + langstr_2;
                }
                else
                {
                    string langstr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("正在退出地图");
                    mapTimeShowObj.GetComponent<Text>().text = langstr_3;
                }
            }

            //显示地图宝箱数量
            mapChestNumObj = mapTimeObj.GetComponent<UI_MapTime>().Obj_MapChestNum;
            string langstr_4 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("秘境值");
            mapChestNumObj.GetComponent<Text>().text = langstr_4 + ":" + rose_Proprety.Rose_MijingValue + "/" + MiJingMaxValie.ToString();
            mapChestNumObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);

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
            if (!MapExitStatus)
            {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ReturnBuilding.GetComponent<UI_StartGame>().Btn_RetuenBuilding();
            }


            MapExitStatus = true;
        }
    }


    //创建一个怪物
    public void CreateMonster()
    {
        //击杀BOSS出现后不再刷新
        if (MiJingBossStatus) {
            return;
        }

        if (MiJingSendRewardStatus) {
            return;
        }

        //退出地图时不再刷新
        if (MapExitStatus)
        {
            return;
        }

        int randomInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, 2);

        //刷新怪物
        GameObject monsterNow = (GameObject)Instantiate(MonsterListSet[randomInt]);
        //设置怪物的父级和坐标点
        monsterNow.transform.SetParent(monsterCreatePosition);
        monsterNow.GetComponent<AI_Property>().AI_MonsterCreateType = "1";

        //出现位置随机
        //float ran_x = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0, 10);
        //float ran_z = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0, 10);
        //ran_x = ran_x - 5;
        //ran_z = ran_z - 5;

        //monsterNow.transform.localPosition = new Vector3(-55 + ran_x, 1, -22 + ran_z);
        monsterNow.transform.position = GetPosiVec3();

        //创建特效
        GameObject effect = (GameObject)Instantiate(Obj_CreateMonsterEffect);
        effect.transform.position = monsterNow.transform.position;
        Destroy(effect, 1);

        monsterNow.SetActive(false);
        monsterNow.SetActive(true);

        CreateMonsterList.Add(monsterNow);

    }

    //随机一个出生坐标
    public Vector3 GetPosiVec3()
    {
        int intNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, CreatePositionVec3.Length-1);
        Vector3 vec3 = CreatePositionVec3[intNum];

        float ran_x = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0, 10);
        float ran_z = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0, 10);
        ran_x = ran_x - 5;
        ran_z = ran_z - 5;

        Vector3 ChestVec3 = new Vector3();
        ChestVec3.x = vec3.x + (int)(ran_x);
        ChestVec3.y = vec3.y + 0.2f;
        ChestVec3.z = vec3.z + (int)(ran_z);
        return ChestVec3;
    }
}
