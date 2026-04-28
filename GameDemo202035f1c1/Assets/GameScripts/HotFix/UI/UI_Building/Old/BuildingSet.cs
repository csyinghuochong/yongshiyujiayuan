using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BuildingSet : MonoBehaviour {

    private bool ClickStatus;
    private float ClickTimeSum;
    private GameObject ClickObj;
    private string buildingID;
    public GameObject Obj_EmenyAct;
    private bool EmenyActStatus;        //怪物进攻状态
    private int EmenyActMax;        //怪物进攻数量上限
    private int FortressMax;        //要塞士兵数量上限

    public GameObject Obj_DayPracticeReward;
    private GameObject dayPracticeRewardObj;        //修炼中心
    public GameObject Obj_GuoWangDaTing;
    private GameObject guoWangDaTingObj;            //国王大厅
    public GameObject Obj_ChouKa;               
    private GameObject chouKaObj;                   //抽卡
    public GameObject Obj_ShiLianZhiTa;
    private GameObject shiLianZhiTaObj;             //试炼之塔
    public GameObject Obj_DuiHuanDaTing;
    private GameObject duiHuanDaTingObj;            //兑换大厅
    public GameObject Obj_DayTask;
    private GameObject dayTaskObj;                  //每日任务

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //鼠标按下移动
        if (Input.GetMouseButtonDown(0))
        {
            //检测是否触碰到UI上,安卓或IOS可能要换一下
#if UNITY_IPHONE
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
            if (!EventSystem.current.IsPointerOverGameObject())
#endif
            {
                //从摄像机处向点击的目标处发送一条射线
                Ray Move_Target_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //声明一个光线触碰器
                RaycastHit Move_Target_Hit;
                LayerMask mask = (1 << 17) ;

                //检测射线是否碰触到对象
                //标记OUT变量在传入参数进去后可能发生改变，和ref类似，但是ref需要给他一个初始值
                //第一个参数射线变量  第二个参数光线触碰器的反馈变量
                if (Physics.Raycast(Move_Target_Ray, out Move_Target_Hit, 100, mask.value))
                {
                    //检测是否修改数据
                    if (Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame)
                    {
                        return;
                    }
                    //获取碰撞地面
                    if (Move_Target_Hit.collider.gameObject.layer == 17)
                    {
                        ClickObj = Move_Target_Hit.collider.gameObject;
                        if (ClickObj != null) {
                            buildingID = ClickObj.GetComponent<BuildingModel>().BuildingID;
                            int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                            switch (buildingID)
                            {
                                //国王大厅
                                case "1":
                                    //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickBuilding();
                                    int openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_GuoWang", "GameMainValue"));
                                    if (roseLv >= openLv)
                                    {
                                        ClickStatus = true;
                                        GameObject cameraPosition = ClickObj.transform.Find("CameraPosition").gameObject;
                                        Camera.main.GetComponent<CameraAI>().BuildEnterStatus = true;
                                        Camera.main.GetComponent<CameraAI>().BuildMoveObj = cameraPosition;
                                        Game_PublicClassVar.Get_game_PositionVar.EnterRoseCameraDrawStatus = true;
                                        //Camera.main.GetComponent<CameraAI>().EnterGameCameraPosition = Camera.main.transform;
                                        Camera.main.GetComponent<CameraAI>().First_CameraQuaternion = Camera.main.transform.rotation;
                                        Camera.main.GetComponent<CameraAI>().First_CameraVec3 = Camera.main.transform.position;
                                        //隐藏建筑名称UI
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingNameSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_HeadSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RightDownSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUISet.SetActive(false);
                                        //Camera.main.GetComponent<CameraAI>().BuildEnterStatus = false;
                                        //Camera.main.GetComponent<CameraAI>().BuildExitStatus = true;
                                    }
                                    else
                                    {
                                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_339");
                                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(openLv + langStrHint);
                                        //Game_PublicClassVar.Get_function_UI.GameGirdHint(openLv + "级开启此功能");
                                    }

                                    break;

                                //修炼中心
                                case "2":
                                    openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_XiuLian", "GameMainValue"));
                                    if (roseLv >= openLv)
                                    {
                                        ClickStatus = true;
                                        GameObject cameraPosition = ClickObj.transform.Find("CameraPosition").gameObject;
                                        Camera.main.GetComponent<CameraAI>().BuildEnterStatus = true;
                                        Camera.main.GetComponent<CameraAI>().BuildMoveObj = cameraPosition;
                                        //隐藏建筑名称UI
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingNameSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_HeadSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RightDownSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUISet.SetActive(false);
                                    }
                                    else
                                    {
                                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_339");
                                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(openLv + langStrHint);
                                        //Game_PublicClassVar.Get_function_UI.GameGirdHint(openLv + "级开启此功能");
                                    }
                                    break;

                                //荣誉大厅
                                case "3":
                                    openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_RongYu", "GameMainValue"));
                                    if (roseLv >= openLv)
                                    {
                                        ClickStatus = true;
                                        GameObject cameraPosition = ClickObj.transform.Find("CameraPosition").gameObject;
                                        Camera.main.GetComponent<CameraAI>().BuildEnterStatus = true;
                                        Camera.main.GetComponent<CameraAI>().BuildMoveObj = cameraPosition;
                                        //隐藏建筑名称UI
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingNameSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_HeadSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RightDownSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUISet.SetActive(false);
                                    }
                                    else
                                    {
                                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_339");
                                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(openLv + langStrHint);
                                        //Game_PublicClassVar.Get_function_UI.GameGirdHint(openLv + "级开启此功能");
                                    }
                                    break;

                                //试炼之塔
                                case "4":
                                    //Game_PublicClassVar.Get_function_UI.GameHint("此功能在后续版本中开放!");
                                    //return;
                                    openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_ShiLian", "GameMainValue"));
                                    if (roseLv >= openLv)
                                    {
                                        ClickStatus = true;
                                        GameObject cameraPosition = ClickObj.transform.Find("CameraPosition").gameObject;
                                        Camera.main.GetComponent<CameraAI>().BuildEnterStatus = true;
                                        Camera.main.GetComponent<CameraAI>().BuildMoveObj = cameraPosition;
                                        //隐藏建筑名称UI
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingNameSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_HeadSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RightDownSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUISet.SetActive(false);
                                    }
                                    else
                                    {
                                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_339");
                                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(openLv + langStrHint);
                                        //Game_PublicClassVar.Get_function_UI.GameGirdHint(openLv + "级开启此功能");
                                    }
                                    break;

                                //幸运殿堂
                                case "5":
                                    openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_XingYunDianTang", "GameMainValue"));
                                    if (roseLv >= openLv)
                                    {
                                        ClickStatus = true;
                                        GameObject cameraPosition = ClickObj.transform.Find("CameraPosition").gameObject;
                                        Camera.main.GetComponent<CameraAI>().BuildEnterStatus = true;
                                        Camera.main.GetComponent<CameraAI>().BuildMoveObj = cameraPosition;
                                        //隐藏建筑名称UI
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingNameSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_HeadSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RightDownSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUISet.SetActive(false);
                                    }
                                    else
                                    {
                                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_339");
                                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(openLv + langStrHint);
                                        //Game_PublicClassVar.Get_function_UI.GameGirdHint(openLv + "级开启此功能");
                                    }
                                    break;

                                //每日任务
                                case "6":
                                    openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_DayTask", "GameMainValue"));
                                    if (roseLv >= openLv)
                                    {
                                        ClickStatus = true;
                                        GameObject cameraPosition = ClickObj.transform.Find("CameraPosition").gameObject;
                                        Camera.main.GetComponent<CameraAI>().BuildEnterStatus = true;
                                        Camera.main.GetComponent<CameraAI>().BuildMoveObj = cameraPosition;
                                        //隐藏建筑名称UI
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingNameSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_HeadSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RightDownSet.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(false);
                                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUISet.SetActive(false);
                                    }
                                    else
                                    {
                                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_339");
                                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(openLv + langStrHint);
                                        //Game_PublicClassVar.Get_function_UI.GameGirdHint(openLv + "级开启此功能");
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        if (ClickStatus) {
            ClickTimeSum = ClickTimeSum + Time.deltaTime;
            if (ClickTimeSum >= 0.5f) {
                ClickStatus = false;
                ClickTimeSum = 0;
                Debug.Log("我点击了建筑:" + buildingID);
                switch (buildingID)
                {
                    //国王大厅
                    case "1":
                        //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickBuilding();
                        if (guoWangDaTingObj == null)
                        {
                            guoWangDaTingObj = (GameObject)Instantiate(Obj_GuoWangDaTing);
                            guoWangDaTingObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.transform);
                            guoWangDaTingObj.transform.localPosition = Vector3.zero;
                            guoWangDaTingObj.transform.localScale = new Vector3(1, 1, 1);
                        }
                    break;

                    //修炼中心
                    case "2":
                        /*  屏蔽领地功能
                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickTerritory();
                        */
                        if (dayPracticeRewardObj == null) {
                            dayPracticeRewardObj = (GameObject)Instantiate(Obj_DayPracticeReward);
                            dayPracticeRewardObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.transform);
                            dayPracticeRewardObj.transform.localPosition = Vector3.zero;
                            dayPracticeRewardObj.transform.localScale = new Vector3(1, 1, 1);
                        }

                    break;

                    //荣誉大厅
                    case "3":
                    //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickTrainCamp();duiHuanDaTingObj
                    if (duiHuanDaTingObj == null)
                    {
                        duiHuanDaTingObj = (GameObject)Instantiate(Obj_DuiHuanDaTing);
                        duiHuanDaTingObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.transform);
                        duiHuanDaTingObj.transform.localPosition = Vector3.zero;
                        duiHuanDaTingObj.transform.localScale = new Vector3(1, 1, 1);
                    }
                    break;

                    //试炼之塔
                    case "4":
                    //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickFortress();
                    //Debug.Log("点击了要塞");
                    if (shiLianZhiTaObj == null)
                    {
                        shiLianZhiTaObj = (GameObject)Instantiate(Obj_ShiLianZhiTa);
                        shiLianZhiTaObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.transform);
                        shiLianZhiTaObj.transform.localPosition = Vector3.zero;
                        shiLianZhiTaObj.transform.localScale = new Vector3(1, 1, 1);
                    }
                    
                    break;

                    //幸运殿堂
                    case "5":
                    //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickTrainCamp();

                    if (chouKaObj == null)
                    {
                        chouKaObj = (GameObject)Instantiate(Obj_ChouKa);
                        chouKaObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.transform);
                        chouKaObj.transform.localPosition = Vector3.zero;
                        chouKaObj.transform.localScale = new Vector3(1, 1, 1);
                    }

                    break;

                    //每日任务
                    case "6":
                    //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickTrainCamp();

                    if (chouKaObj == null)
                    {
                        dayTaskObj = (GameObject)Instantiate(Obj_DayTask);
                        dayTaskObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.transform);
                        dayTaskObj.transform.localPosition = Vector3.zero;
                        dayTaskObj.transform.localScale = new Vector3(1, 1, 1);
                    }

                    break;
                }
            }
        }

        //判定怪物进攻状态是否开启
        if (Game_PublicClassVar.Get_game_PositionVar.emenyActStatus) {

            //隐藏建筑UI
            BuildingMainUI buildingMainUI = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>();
            if (buildingMainUI.Obj_Fortress.active == true)
            {
                buildingMainUI.Btn_ClickFortress();
            }
            if (buildingMainUI.Obj_Building.active == true)
            {
                buildingMainUI.Btn_ClickBuilding();
            }
            if (buildingMainUI.Obj_Territory.active == true)
            {
                buildingMainUI.Btn_ClickTerritory();
            }
            if (buildingMainUI.Obj_TrainCamp.active == true)
            {
                buildingMainUI.Btn_ClickTrainCamp();
            }

            //实例化UI
            GameObject obj = (GameObject)Instantiate(Obj_EmenyAct);
            obj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(1, 1, 1);

            //UI赋值
            EmenyActMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EnemyNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding"));
            //FortressMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SoldierNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding"));
            FortressMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmValue", "ID", Game_PublicClassVar.Get_game_PositionVar.FortressID, "Building_Template"));
            obj.GetComponent<UI_EmenyActSet>().EmenyActMax = EmenyActMax;
            obj.GetComponent<UI_EmenyActSet>().FortressMax = FortressMax;

            //重置数据,设置下次进攻
            int nextEmenyActTime = 1800+(int)(1800*Random.value);   //默认进攻时间30分钟+30分钟*随机值
            //nextEmenyActTime = 30;
            Game_PublicClassVar.Get_game_PositionVar.emenyActStatus = false;
            Game_PublicClassVar.Get_game_PositionVar.emenyActTime = nextEmenyActTime;       
            //存储下次进攻时间
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EnemyTime", nextEmenyActTime.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            //存储下次进攻人数
            int enemyNum = (int)((FortressMax * 0.6f) + (FortressMax * 0.4f * Random.value));
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EnemyNum", enemyNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
        }
	}
}
