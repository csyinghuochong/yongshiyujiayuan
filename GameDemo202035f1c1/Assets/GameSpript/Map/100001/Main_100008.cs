using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class Main_100008 : MonoBehaviour {

    //创建怪物
    public Transform monsterCreatePosition;
    public bool CreateBossStatus;

    public ObscuredInt NowCreateNum;         //当前创建波数

    //时间
    private ObscuredFloat MapTime;
    private ObscuredFloat MapTimeSum;
    private bool MapExitStatus;             //地图退出状态
    private ObscuredFloat OneTimeSum;       //1秒执行1次

    public bool EndFuBenStatus;

    //创建列表
    public GameObject[] CreateObjMonsterList;
    public GameObject CreateObjBoss_1;
    public GameObject CreateObjBoss_2;
    public GameObject CreateObjBoss_3;

    //UI类
    public GameObject Obj_MapTime;
    private GameObject mapTimeObj;
    private GameObject mapTimeShowObj;
    private GameObject mapChestNumObj;

    public GameObject NowBossObj;
    private bool TiQianEndStatus;

    // Use this for initialization
    void Start () {

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


        NowCreateNum = 0;
        MapTimeSum = 0;
        MapTime = 3;           //进入地图10秒后刷新怪物
    }
	
	// Update is called once per frame
	void Update () {

        //显示地图时间
        MapTimeSum = MapTimeSum + Time.deltaTime;
        //1秒计时
        OneTimeSum = OneTimeSum + Time.deltaTime;

        if (EndFuBenStatus == false) {

            if (OneTimeSum >= 1)
            {
                OneTimeSum = 0;

                int nowMapTime = (int)(MapTime - MapTimeSum);
                if (mapTimeShowObj != null)
                {
                    if (nowMapTime >= 0)
                    {
                        //string langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("地图剩余时间");
                        //string langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("秒");
                        mapTimeShowObj.GetComponent<Text>().text = "下一波怪物来临剩余" + nowMapTime + "秒";

                        //BOSS击杀回合提前结束,
                        if (TiQianEndStatus)
                        {
                            if (NowBossObj == null || NowBossObj.GetComponent<AI_1>().ai_IfDeath)
                            {
                                if (nowMapTime >= 15)
                                {
                                    MapTime = 15;
                                    MapTimeSum = 0;
                                    TiQianEndStatus = false;
                                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("击败领主成功! 请做好准备,下一波将提前到来...");
                                }
                            }
                        }
                    }
                    else
                    {
                        //string langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("正在退出地图");
                        //mapTimeShowObj.GetComponent<Text>().text = langstr_2;
                        NowCreateNum = NowCreateNum + 1;
                        CreateNumUpdate(NowCreateNum);

                    }
                }

                mapChestNumObj = mapTimeObj.GetComponent<UI_MapTime>().Obj_MapChestNum;
                string langstr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("寻找到真实宝箱");
                mapChestNumObj.GetComponent<Text>().text = "当前波数" + NowCreateNum + "/" + "18";

            }

        }


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
    }

    //注销时调用
    private void OnDestroy()
    {
        
    }

    //创建一个怪物
    public void CreateMonster(GameObject createObj) {

        if (MapExitStatus) {
            return;
        }

        //刷新怪物
        GameObject monsterNow = (GameObject)Instantiate(createObj);
        //设置怪物的父级和坐标点
        monsterNow.transform.SetParent(monsterCreatePosition);

        //出现位置随机
        float ran_x = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0, 10);
        float ran_z = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0, 10);
        ran_x = ran_x - 5;
        ran_z = ran_z - 5;

        monsterNow.transform.localPosition = new Vector3(ran_x, 1,ran_z);

        monsterNow.SetActive(false);
        monsterNow.SetActive(true);

        //设定BOSS
        if (createObj == CreateObjBoss_1 || createObj == CreateObjBoss_2 || createObj == CreateObjBoss_3)
        {
            NowBossObj = monsterNow;
            TiQianEndStatus = true;
        }

        
    }


    //根据层数创建怪物
    public void CreateNumUpdate(int createNum) {


        if (createNum >= 19) {
            return;
        }

        if (createNum == 18) {
            EndFuBenStatus = true;
        }

        MapTime = 30;           //默认30秒出一波怪物
        MapTimeSum = 0;
        float createPro = 1;
        string createType = "1";
        switch (createNum)
        {

            case 1:
                createType = "1";
                createPro = 1;
                break;

            case 2:
                createType = "2";
                createPro = 1;
                break;

            case 3:
                createType = "3";
                createPro = 1;
                break;

            case 4:
                createType = "4";
                createPro = 1;
                break;

            case 5:
                createType = "5";
                createPro = 1;
                break;

            case 6:
                createType = "11";
                MapTime = 180;
                break;


            case 7:
                createType = "1";
                createPro = 1.5f;
                break;

            case 8:
                createType = "2";
                createPro = 1.5f;
                break;

            case 9:
                createType = "3";
                createPro = 1.5f;
                break;

            case 10:
                createType = "4";
                createPro = 1.5f;
                break;

            case 11:
                createType = "5";
                createPro = 1.5f;
                break;


            case 12:
                createType = "12";
                MapTime = 240;
                break;


            case 13:
                createType = "1";
                createPro = 2f;
                break;

            case 14:
                createType = "2";
                createPro = 2f;
                break;

            case 15:
                createType = "3";
                createPro = 2f;
                break;

            case 16:
                createType = "4";
                createPro = 2f;
                break;

            case 17:
                createType = "5";
                createPro = 2f;
                break;

            case 18:
                createType = "13";
                MapTime = 300;
                break;
        }

        //创建波数
        if (createType == "1") {

            //创建小怪
            for (int i = 0; i < (int)(4 * createPro); i++) {
                CreateMonster(CreateObjMonsterList[0]);
            }

            TiQianEndStatus = false;
        }

        //创建波数
        if (createType == "2")
        {

            //创建小怪
            for (int i = 0; i < (int)(6 * createPro); i++)
            {
                CreateMonster(CreateObjMonsterList[0]);
            }

            TiQianEndStatus = false;
        }

        //创建波数
        if (createType == "3")
        {

            //创建小怪
            for (int i = 0; i < (int)(6 * createPro); i++)
            {
                CreateMonster(CreateObjMonsterList[0]);
            }

            //创建小怪
            for (int i = 0; i < (int)(2 * createPro); i++)
            {
                CreateMonster(CreateObjMonsterList[1]);
            }

            TiQianEndStatus = false;
        }


        //创建波数
        if (createType == "4")
        {

            //创建小怪
            for (int i = 0; i < (int)(6 * createPro); i++)
            {
                CreateMonster(CreateObjMonsterList[0]);
            }

            //创建小怪
            for (int i = 0; i < (int)(4 * createPro); i++)
            {
                CreateMonster(CreateObjMonsterList[1]);
            }

            TiQianEndStatus = false;
        }

        //创建波数
        if (createType == "5")
        {

            //创建小怪
            for (int i = 0; i < (int)(8 * createPro); i++)
            {
                CreateMonster(CreateObjMonsterList[0]);
            }

            //创建小怪
            for (int i = 0; i < (int)(4 * createPro); i++)
            {
                CreateMonster(CreateObjMonsterList[1]);
            }

            TiQianEndStatus = false;
        }


        if (createType == "11")
        {
            CreateMonster(CreateObjBoss_1);
        }

        if (createType == "12")
        {
            CreateMonster(CreateObjBoss_2);
        }

        if (createType == "13")
        {
            CreateMonster(CreateObjBoss_3);
        }

    }
}
