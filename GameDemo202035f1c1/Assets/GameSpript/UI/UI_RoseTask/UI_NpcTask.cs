using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class UI_NpcTask : MonoBehaviour {

    public string NpcID;                            //NpcID
    public ArrayList CompleteTaskID;                //Npc完成的任务
    private string[] npcTaskIDList;                 //Npc携带的任务列表
    public GameObject Obj_NpcTaskSet;       
    private int creatNpcTask;                       //创建UINpcTask的数量
    public GameObject Obj_NpcSpeak;                 //Npc对话
    public bool UpdataStatus;                       //更新状态,打开开关更新Npc携带的任务
	public GameObject Obj_Function;			        //功能按钮
	private string npcType;
    public GameObject Obj_NpcName;                  //Npc名称Obj
    public GameObject Obj_RoseStoreHouse;           //角色仓库Obj
    public GameObject Obj_EquipXiLi;                //装备洗炼
	public GameObject Obj_HuiShouItem;		        //装备回收
    public GameObject Obj_Npc;                      //NPC源模型
    private bool UseFunctionStatus;                 //使用方法状态
    private float UseFunctionSum;                   //使用方法累计
    private bool NpcCameraStatus;                   //NPC摄像机状态
    private float NpcCameraSum;                     //Npc摄像机累计 
    public GameObject Obj_GiveBtn;                  
    public GameObject Obj_UI_GiveNPC;               
    public ArrayList giveTaskIDList = new ArrayList();                //Npc完成的任务

    //主城相关UI
    private GameObject dayPracticeRewardObj;        //修炼中心
    private GameObject guoWangDaTingObj;            //国王大厅
    private GameObject chouKaObj;                   //抽卡
    private GameObject shiLianZhiTaObj;             //试炼之塔
    private GameObject duiHuanDaTingObj;            //兑换大厅
    private GameObject dayTaskObj;                  //每日任务
    private GameObject shouJiDaTingObj;             //收集大厅

    //藏宝图相关
    public GameObject Obj_EnterCangBaoTu;
    public GameObject Obj_CangBaoTuJieShao;
    public GameObject Obj_BuyCangBaoTu;
    public GameObject Obj_CangBaoTuReward;

    //兑换神兽
    public GameObject Obj_ShenShouDuiHuan;
    public GameObject Obj_ShenShouDuiHuan_LongLong;
    public GameObject Obj_ShenShouDuiHuan_HuanHuan;

    //副本
    public GameObject Obj_EnterFuBen_1;
    public GameObject Obj_EnterFuBen_ShangHai;
    public GameObject Obj_EnterFuBen_ShangHaiReward;
    public GameObject Obj_EnterFuBen_ShangHaiEveryReward;

    public GameObject Obj_EnterFuBen_2;

    //封印之塔
    public GameObject Obj_EnterFuBen_FengYinZhiTa;

    //喜从天降
    public GameObject Obj_EnterHuoDongChest;

    //大秘境
    public GameObject Obj_EnterDaMiJing;
    public GameObject Obj_DaMiJingJieShao;
    public GameObject Obj_DaMiJingReward;

    //注灵
    public GameObject Obj_ZhuLing;

    //

    // Use this for initialization
    void Start () {

        if (NpcID == "" || NpcID == null || NpcID == "0") {
            Destroy(this.gameObject);
        }

        //显示Npc对话   
        string npcSpeakText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpeakText", "ID", NpcID, "Npc_Template");
        string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", NpcID, "Npc_Template");
        //Debug.Log("NpcID = " + NpcID + "npcSpeakText = " + npcSpeakText + "npcName = " + npcName);
        Obj_NpcSpeak.GetComponent<Text>().text = "    "+npcSpeakText;
        Obj_NpcName.GetComponent<Text>().text = npcName;
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

		//获取Npc类型
		npcType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("NpcType", "ID", NpcID, "Npc_Template");
		switch (npcType) {

            //对话NPC
            case "1":
                //UseFunctionStatus = true;
                updataTask();
                break;
            //商店类型NPC
			case "2":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
			break;
		    //仓库类型NPC
            case "3":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                //开启仓库状态
                Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus = true;
            break;
            //装备洗练类型
            case "4":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                //开启仓库状态
                //Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus = true;
            break;
			//回收道具
			case "5":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
			    //开启回收道具状态
			    Game_PublicClassVar.Get_game_PositionVar.HuiShouItemStatus = true;
			break;
            //商店类型NPC
            case "6":
                UseFunctionStatus = true;
                Debug.Log("UI_NpcTaskUI_NpcTaskUI_NpcTaskUI_NpcTask123123123123");
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
            break;

            //师门任务
            case "7":
                updataTask_ShiMen();
            break;

            //学习类型NPC
            case "8":
            UseFunctionStatus = true;
            //移除屏幕外,要不又NPC播放动画时会显示UI
            this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
            break;

            //藏宝图
            case "9":
                Npc_CangBaoTu();
            break;

            //传说宠物碎片_花花
            case "10":
                Npc_ShenShouSuiPian();
            break;

            //进入副本
            case "11":
                ShowFuBenList_1();
            break;

            //邮箱
            case "12":
				UseFunctionStatus = true;
				//移除屏幕外,要不又NPC播放动画时会显示UI
				this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                break;

            //大秘境
            case "13":
                //ShowDaMiJing();
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                break;

            //喜从天降
            case "14":
                ShowHuoDong_Chest();
                break;

            //试炼任务
            case "15":
                updataTask_ShiLian();
                break;

            //伤害试炼
            case "16":
                Npc_ShangHaiShiLian();
                break;

            //封印之塔
            case "17":
                Npc_FengYinZhiTa();
                break;

            //注灵
            case "18":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                break;

            //牧场
            case "31":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                break;

            //牧场兑换
            case "32":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                break;

            //牧场仓库
            case "33":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                break;

            //牧场合成
            case "34":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                break;

            //牧场公告
            case "35":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                break;

            //坐骑大师
            case "41":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                break;

            //染色大师
            case "42":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
                break;

            //国王大厅
            case "101":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
            break;

            //修炼中心
            case "102":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
            break;

            //荣誉大厅
            case "103":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
            break;

            //试炼之塔
            case "104":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
            break;

            //幸运殿堂
            case "105":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
            break;

            //每日任务
            case "106":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
            break;

            //收集大厅
            case "107":
                UseFunctionStatus = true;
                //移除屏幕外,要不又NPC播放动画时会显示UI
                this.GetComponent<RectTransform>().localPosition = new Vector3(99999, 99999, 99999);
            break;


            //传说宠物碎片_龙龙
            case "201":
                Npc_ShenShouSuiPian_LongLong();
                break;

            //传说宠物碎片_花花
            case "202":
                Npc_ShenShouSuiPian_HuanHuan();
                break;
        }

        //Obj_GiveBtn.SetActive(false);
        
	}
	
	// Update is called once per frame
	void Update () {

        //更新Npc携带的完成任务
        if (UpdataStatus)
        {
            UpdataStatus = false;
            //循环清空任务列表下的控件
            for (int i = 0; i < Obj_NpcTaskSet.transform.childCount; i++)
            {
                GameObject go = Obj_NpcTaskSet.transform.GetChild(i).gameObject;
                Destroy(go);
            }

            switch (npcType)
            {
                //更新主线任务
                case "1":
                    updataTask();
                    break;

                //更新师门任务
                case "7":
                    updataTask_ShiMen();
                    break;

                //更新师门任务
                case "15":
                    updataTask_ShiLian();
                    break;
            }
        }

        if (UseFunctionStatus)
        {
            UseFunctionStatus = false;
            string ifCameraLaJin = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfCameraLaJin", "ID", NpcID, "Npc_Template");
            //直接调用
            if (ifCameraLaJin == "0") {
                Destroy(this.gameObject);
                UseFunction();
            }
            if (ifCameraLaJin == "1")
            {
                //点击NPC拉低镜头
                GameObject cameraPosition;
                //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ai_nav.Stop();
                //判定NPC是否有移动点
                if (Obj_Npc.transform.Find("CameraPosition") != null)
                {
                    cameraPosition = Obj_Npc.transform.Find("CameraPosition").gameObject;
                }
                else {
                    //创建一个摄像机移动的目标点
                    GameObject moveObj = new GameObject();
                    moveObj.name = "CameraPosition";
                    moveObj.transform.SetParent(Obj_Npc.transform);
                    moveObj.transform.localPosition = new Vector3(-0.35f, 1.7f, 2.37f);
                    moveObj.transform.localRotation =  Quaternion.Euler(new Vector3(8.3f,-157,2.33f));
                    cameraPosition = moveObj;
                }

                Camera.main.GetComponent<CameraAI>().BuildEnterStatus = true;
                Camera.main.GetComponent<CameraAI>().BuildMoveObj = cameraPosition;
                Game_PublicClassVar.Get_game_PositionVar.EnterRoseCameraDrawStatus = true;
                Camera.main.GetComponent<CameraAI>().First_CameraQuaternion = Camera.main.transform.rotation;
                Camera.main.GetComponent<CameraAI>().First_CameraVec3 = Camera.main.transform.position;
                NpcCameraStatus = true;
            }
        }

        if (NpcCameraStatus) {
            NpcCameraSum = NpcCameraSum + Time.deltaTime;
            if (NpcCameraSum > 0.3f) {
                UseFunction();
                NpcCameraStatus = false;
                NpcCameraSum = 0;
                Destroy(this.gameObject,1);
            }
        }
	}

    void OnDestroy() {
        //Debug.Log("OnDestroyOnDestroyOnDestroyOnDestroyOnDestroyOnDestroy");
    }

    void updataTask() {

        giveTaskIDList.Clear();

        creatNpcTask = 0;
        if (NpcID != "0") {
            //获取NPC的任务
            npcTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskID", "ID", NpcID, "Npc_Template").Split(';');

            if (CompleteTaskID != null) {
                //循环创建完成任务
                foreach (string taskID in CompleteTaskID)
                {
                    if (taskID != "" && taskID != "0") {
                        //创建接取任务的Btn
                        GameObject getTaskUI = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_NpcTaskList);
                        getTaskUI.transform.SetParent(Obj_NpcTaskSet.transform);
                        getTaskUI.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
                        getTaskUI.transform.localScale = new Vector3(1, 1, 1);
                        getTaskUI.GetComponent<UI_NpcTaskList>().TaskID = taskID;
                        getTaskUI.GetComponent<UI_NpcTaskList>().TaskStatus = "4";      //4代表任务已接取已完成
                        creatNpcTask = creatNpcTask + 1;
                    }
                }
            }

            //循环创建任务列表
            for (int i = 0; i <= npcTaskIDList.Length - 1; i++)
            {
                if (npcTaskIDList[i] != "" && npcTaskIDList[i] != "0") {
                    string taskStatus = Game_PublicClassVar.Get_function_Task.TaskReturnStatus(npcTaskIDList[i]);
                    //实例化任务Btn
                    if (taskStatus == "1")
                    {
                        //创建接取任务的Btn
                        GameObject getTaskUI = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_NpcTaskList);
                        getTaskUI.transform.SetParent(Obj_NpcTaskSet.transform);
                        getTaskUI.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
                        getTaskUI.transform.localScale = new Vector3(1, 1, 1);
                        getTaskUI.GetComponent<UI_NpcTaskList>().TaskID = npcTaskIDList[i];
                        getTaskUI.GetComponent<UI_NpcTaskList>().TaskStatus = taskStatus;
                        getTaskUI.GetComponent<UI_NpcTaskList>().TaskType = "0";
                        creatNpcTask = creatNpcTask + 1;
                    }
                }

                //循环判定当前任务是否是给与任务
                string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", npcTaskIDList[i], "Task_Template");
                if (targetType == "11" || targetType == "12" || targetType == "13") {
                    Obj_GiveBtn.SetActive(true);
                    giveTaskIDList.Add(npcTaskIDList[i]);
                }
            }
        }
    }


    //更新师门任务
    void updataTask_ShiMen()
    {
        giveTaskIDList.Clear();

        creatNpcTask = 0;
        //有完成的任务先创建完成的任务
        if (CompleteTaskID != null)
        {
            //循环创建完成任务
            foreach (string taskID in CompleteTaskID)
            {
                //创建接取任务的Btn
                GameObject getTaskUI_1 = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_NpcTaskList);
                getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
                getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
                getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);
                getTaskUI_1.GetComponent<UI_NpcTaskList>().TaskID = taskID;
                getTaskUI_1.GetComponent<UI_NpcTaskList>().TaskStatus = "4";      //4代表任务已接取已完成
                getTaskUI_1.GetComponent<UI_NpcTaskList>().TaskType = "1";
                creatNpcTask = creatNpcTask + 1;
            }
        }

        //每日任务  NPC有1个没完成的每日任务就不显示所有的每日任务列表
        List<string> task_Everyday = new List<string>();        
        task_Everyday = Game_PublicClassVar.Get_function_Task.GetRoseTaskTypeID("3");

        //循环判定当前任务是否是给与任务
        foreach (string taskID in task_Everyday) {
            string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", taskID, "Task_Template");
            if (targetType == "11" || targetType == "12" || targetType == "13")
            {
                Debug.Log("taskID = " + taskID);
                //判断任务是否已经完成
                if (!Game_PublicClassVar.Get_function_Task.TaskComplete(taskID))
                {
                    Obj_GiveBtn.SetActive(true);
                    giveTaskIDList.Add(taskID);
                }
            }
        }


        //当前是否有每日任务
        if (task_Everyday.Count > 0)
        {
            return;
        }

        //当前每日任务超过10,不在显示
        int shiMenTaskNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMenTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        if (shiMenTaskNum >= 10)
        {
            return;
        }

        //判断当前是否有任务要领取
        if (NpcID != "0")
        {
            //Debug.Log("有任务要截取");
            //获取NPC的任务
            npcTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskID", "ID", NpcID, "Npc_Template").Split(';');
            //Debug.Log("npcTaskIDList = " + npcTaskIDList);
            string nowTaskStr = "";

            //重新筛选当前符合自己等级区间的任务
            for(int i = 0; i < npcTaskIDList.Length; i++){
                if (npcTaskIDList[i] != "" && npcTaskIDList[i] != "0")
                {
                    string taskStatus = Game_PublicClassVar.Get_function_Task.TaskReturnStatus(npcTaskIDList[i]);
                    //Debug.Log("taskStatus = " + taskStatus + ";npcTaskIDList[i] = " + npcTaskIDList[i]);
                    if (taskStatus == "1")
                    {
                        if (nowTaskStr!="")
                        {
                            nowTaskStr = nowTaskStr + ";" + npcTaskIDList[i];
                        }
                        else {
                            nowTaskStr = npcTaskIDList[i];
                        }
                    }
                }
            }

            string nowTaskID = "";
            string shiLianNextTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMenNextTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            if (shiLianNextTaskID == "" || shiLianNextTaskID == "0")
            {
                npcTaskIDList = nowTaskStr.Split(';');
                //随机获取一个师门任务ID
                int nowNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, npcTaskIDList.Length - 1);
                if (nowNum < 0)
                {
                    nowNum = 0;
                }

                nowTaskID = npcTaskIDList[nowNum];
                //记录下次的要领取的任务
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiMenNextTaskID", nowTaskID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
            }
            else {
                nowTaskID = shiLianNextTaskID;
            }

            //创建接取任务的Btn
            GameObject getTaskUI = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_NpcTaskList);
            getTaskUI.transform.SetParent(Obj_NpcTaskSet.transform);
            getTaskUI.transform.localPosition = new Vector3(0, creatNpcTask * -45, 0);
            getTaskUI.transform.localScale = new Vector3(1, 1, 1);
            getTaskUI.GetComponent<UI_NpcTaskList>().TaskID = nowTaskID;
            getTaskUI.GetComponent<UI_NpcTaskList>().TaskStatus = "1";
            getTaskUI.GetComponent<UI_NpcTaskList>().TaskType = "1";
            creatNpcTask = creatNpcTask + 1;
        }
    }



    //更新试炼任务
    void updataTask_ShiLian()
    {
        giveTaskIDList.Clear();

        creatNpcTask = 0;
        //有完成的任务先创建完成的任务
        if (CompleteTaskID != null)
        {
            //循环创建完成任务
            foreach (string taskID in CompleteTaskID)
            {
                //创建接取任务的Btn
                GameObject getTaskUI_1 = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_NpcTaskList);
                getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
                getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
                getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);
                getTaskUI_1.GetComponent<UI_NpcTaskList>().TaskID = taskID;
                getTaskUI_1.GetComponent<UI_NpcTaskList>().TaskStatus = "4";      //4代表任务已接取已完成
                getTaskUI_1.GetComponent<UI_NpcTaskList>().TaskType = "2";
                creatNpcTask = creatNpcTask + 1;
            }
        }

        //每日任务  NPC有1个没完成的每日任务就不显示所有的每日任务列表
        
        List<string> task_List = new List<string>();
        task_List = Game_PublicClassVar.Get_function_Task.GetRoseTaskTypeID("2");

        //循环判定当前任务是否是给与任务
        foreach (string taskID in task_List)
        {
            string sonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskSonType", "ID", taskID, "Task_Template");
            if (sonType == "1") {
                string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", taskID, "Task_Template");
                if (targetType == "11" || targetType == "12" || targetType == "13")
                {
                    Debug.Log("taskID = " + taskID);
                    //判断任务是否已经完成
                    if (!Game_PublicClassVar.Get_function_Task.TaskComplete(taskID))
                    {
                        Obj_GiveBtn.SetActive(true);
                        giveTaskIDList.Add(taskID);
                    }
                }
            }
        }
        
        /*
        //当前是否有每日任务
        if (task_Everyday.Count > 0)
        {
            return;
        }
        */


        //获取当前是否接取试炼任务
        //string shiLianTaskStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianTaskStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //保险起见,检测一下自身的试炼任务
        bool ifJianCeStatus = Game_PublicClassVar.Get_function_Task.GetShiLianTaskStatus();

        if (ifJianCeStatus == true) {
            Debug.Log("当前已经接取试炼任务!");
            return;
        }


       //当前每日任务超过10,不在显示
       /*
       int shiLianTaskNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

       if (shiLianTaskNum >= 10)
       {
           return;
       }
       */



        //判断当前是否有任务要领取
        if (NpcID != "0")
        {
            //Debug.Log("有任务要截取");
            //获取NPC的任务
            npcTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskID", "ID", NpcID, "Npc_Template").Split(';');
            //Debug.Log("npcTaskIDList = " + npcTaskIDList);
            string nowTaskStr = "";

            //重新筛选当前符合自己等级区间的任务
            for (int i = 0; i < npcTaskIDList.Length; i++)
            {
                if (npcTaskIDList[i] != "" && npcTaskIDList[i] != "0")
                {
                    string taskStatus = Game_PublicClassVar.Get_function_Task.TaskReturnStatus(npcTaskIDList[i]);
                    //Debug.Log("taskStatus = " + taskStatus + ";npcTaskIDList[i] = " + npcTaskIDList[i]);
                    if (taskStatus == "1")
                    {
                        if (nowTaskStr != "")
                        {
                            nowTaskStr = nowTaskStr + ";" + npcTaskIDList[i];
                        }
                        else
                        {
                            nowTaskStr = npcTaskIDList[i];
                        }
                    }
                }
            }

            //Debug.Log("npcTaskIDList123 = " + npcTaskIDList);
            //npcTaskIDList = nowTaskStr.Split(';');

            ////随机获取一个师门任务ID
            //int nowNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, npcTaskIDList.Length - 1);
            //if (nowNum < 0)
            //{
            //    nowNum = 0;
            //}

            //string nowTaskID = npcTaskIDList[nowNum];


            string nowTaskID = "";
            string shiLianNextTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianNextTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            if (shiLianNextTaskID == "" || shiLianNextTaskID == "0")
            {
                npcTaskIDList = nowTaskStr.Split(';');
                //随机获取一个师门任务ID
                int nowNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, npcTaskIDList.Length - 1);
                if (nowNum < 0)
                {
                    nowNum = 0;
                }

                nowTaskID = npcTaskIDList[nowNum];
                //记录下次的要领取的任务
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianNextTaskID", nowTaskID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
            }
            else
            {
                nowTaskID = shiLianNextTaskID;
            }

            //创建接取任务的Btn
            GameObject getTaskUI = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_NpcTaskList);
            getTaskUI.transform.SetParent(Obj_NpcTaskSet.transform);
            getTaskUI.transform.localPosition = new Vector3(0, creatNpcTask * -45, 0);
            getTaskUI.transform.localScale = new Vector3(1, 1, 1);
            getTaskUI.GetComponent<UI_NpcTaskList>().TaskID = nowTaskID;
            getTaskUI.GetComponent<UI_NpcTaskList>().TaskStatus = "1";
            getTaskUI.GetComponent<UI_NpcTaskList>().TaskType = "2";
            creatNpcTask = creatNpcTask + 1;
        }
    }



    //藏宝图
    void Npc_CangBaoTu() {

        //显示进入藏宝图按钮
        GameObject getTaskUI_1 = (GameObject)Instantiate(Obj_EnterCangBaoTu);
        getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);
        creatNpcTask = creatNpcTask + 1;

        //显示了解藏宝图介绍
        GameObject getTaskUI_2 = (GameObject)Instantiate(Obj_CangBaoTuJieShao);
        getTaskUI_2.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_2.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_2.transform.localScale = new Vector3(1, 1, 1);
        creatNpcTask = creatNpcTask + 1;

        //领取藏宝图奖励
        GameObject getTaskUI_3 = (GameObject)Instantiate(Obj_CangBaoTuReward);
        getTaskUI_3.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_3.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_3.transform.localScale = new Vector3(1, 1, 1);
        creatNpcTask = creatNpcTask + 1;

        //显示兑换藏宝图按钮
        GameObject getTaskUI_4 = (GameObject)Instantiate(Obj_BuyCangBaoTu);
        getTaskUI_4.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_4.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_4.transform.localScale = new Vector3(1, 1, 1);
    }

    //传说碎片
    void Npc_ShenShouSuiPian() {
        //创建接取任务的Btn
        GameObject getTaskUI_1 = (GameObject)Instantiate(Obj_ShenShouDuiHuan);
        getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);
    }

    //传说碎片
    void Npc_ShenShouSuiPian_LongLong()
    {
        //创建接取任务的Btn
        GameObject getTaskUI_1 = (GameObject)Instantiate(Obj_ShenShouDuiHuan_LongLong);
        getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);
    }

    //传说碎片
    void Npc_ShenShouSuiPian_HuanHuan()
    {
        //创建接取任务的Btn
        GameObject getTaskUI_1 = (GameObject)Instantiate(Obj_ShenShouDuiHuan_HuanHuan);
        getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);
    }

    //展示副本列表
    void ShowFuBenList_1() {

        //显示副本列表
        GameObject getTaskUI_1 = (GameObject)Instantiate(Obj_EnterFuBen_1);
        getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);

        creatNpcTask = creatNpcTask + 1;

        //显示副本列表
        GameObject getTaskUI_2 = (GameObject)Instantiate(Obj_EnterFuBen_2);
        getTaskUI_2.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_2.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_2.transform.localScale = new Vector3(1, 1, 1);

    }

    //展示副本列表
    void ShowMail()
    {
        //后期添加

    }

    //展示喜从天降宝箱活动
    void ShowHuoDong_Chest()
    {
        //显示副本列表
        GameObject getTaskUI_1 = (GameObject)Instantiate(Obj_EnterHuoDongChest);
        getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);
    }

    //展示大秘境
    void ShowDaMiJing()
    {
        //显示副本列表
        GameObject getTaskUI_1 = (GameObject)Instantiate(Obj_EnterDaMiJing);
        getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);
        creatNpcTask = creatNpcTask + 1;

        //显示了解藏宝图介绍
        GameObject getTaskUI_2 = (GameObject)Instantiate(Obj_DaMiJingJieShao);
        getTaskUI_2.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_2.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_2.transform.localScale = new Vector3(1, 1, 1);
        creatNpcTask = creatNpcTask + 1;

        //显示了解藏宝图介绍
        GameObject getTaskUI_3 = (GameObject)Instantiate(Obj_DaMiJingReward);
        getTaskUI_3.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_3.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_3.transform.localScale = new Vector3(1, 1, 1);
        creatNpcTask = creatNpcTask + 1;

        
    }

    //展示伤害试炼
    //藏宝图
    void Npc_ShangHaiShiLian()
    {

        //显示进入藏宝图按钮
        GameObject getTaskUI_1 = (GameObject)Instantiate(Obj_EnterFuBen_ShangHai);
        getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);
        creatNpcTask = creatNpcTask + 1;

        //显示了解藏宝图介绍
        GameObject getTaskUI_2 = (GameObject)Instantiate(Obj_EnterFuBen_ShangHaiReward);
        getTaskUI_2.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_2.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_2.transform.localScale = new Vector3(1, 1, 1);
        creatNpcTask = creatNpcTask + 1;

        //显示了解藏宝图介绍
        GameObject getTaskUI_3 = (GameObject)Instantiate(Obj_EnterFuBen_ShangHaiEveryReward);
        getTaskUI_3.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_3.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_3.transform.localScale = new Vector3(1, 1, 1);
        creatNpcTask = creatNpcTask + 1;

    }

    //封印之塔
    void Npc_FengYinZhiTa() {

        //显示进入藏宝图按钮
        GameObject getTaskUI_1 = (GameObject)Instantiate(Obj_EnterFuBen_FengYinZhiTa);
        getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);
        creatNpcTask = creatNpcTask + 1;

        //每日封印
        //GameObject getTaskUI_2 = (GameObject)Instantiate(Obj_EnterFuBen_ShangHaiReward);
        //getTaskUI_2.transform.SetParent(Obj_NpcTaskSet.transform);
        //getTaskUI_2.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        //getTaskUI_2.transform.localScale = new Vector3(1, 1, 1);
        //creatNpcTask = creatNpcTask + 1;

    }

    //封印之塔
    void Npc_ZhuLing()
    {

        //显示进入藏宝图按钮
        GameObject getTaskUI_1 = (GameObject)Instantiate(Obj_ZhuLing);
        getTaskUI_1.transform.SetParent(Obj_NpcTaskSet.transform);
        getTaskUI_1.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        getTaskUI_1.transform.localScale = new Vector3(1, 1, 1);
        creatNpcTask = creatNpcTask + 1;

        //每日封印
        //GameObject getTaskUI_2 = (GameObject)Instantiate(Obj_EnterFuBen_ShangHaiReward);
        //getTaskUI_2.transform.SetParent(Obj_NpcTaskSet.transform);
        //getTaskUI_2.transform.localPosition = new Vector3(5, creatNpcTask * -75, 0);
        //getTaskUI_2.transform.localScale = new Vector3(1, 1, 1);
        //creatNpcTask = creatNpcTask + 1;

    }

    //关闭UI
    public void CloseUI() {
        Debug.Log("Close");
        //退出时,镜头切换
        string ifCameraLaJin = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfCameraLaJin", "ID", NpcID, "Npc_Template");
        if (ifCameraLaJin == "1")
        {
            Camera.main.GetComponent<CameraAI>().BuildEnterStatus = false;
            Camera.main.GetComponent<CameraAI>().BuildExitStatus = true;
        }
        Destroy(this.gameObject);
    }

    public void Btn_GiveNPC() {

        GameObject npcXiLian = (GameObject)Instantiate(Obj_UI_GiveNPC);
        npcXiLian.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        npcXiLian.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        npcXiLian.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        npcXiLian.transform.localScale = new Vector3(1, 1, 1);
        npcXiLian.GetComponent<UI_GiveNPC>().taskIDList = giveTaskIDList;
    }

	//使用Npc功能
	public void UseFunction(){

		switch (npcType) {
                //商店Obj_UINpcStoreShow_2
		    case "2":
			    GameObject npcStoreShow = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UINpcStoreShow);
			    npcStoreShow.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
			    npcStoreShow.transform.localPosition = new Vector3(-300, 5, 0);
			    npcStoreShow.transform.localScale = new Vector3(1, 1, 1);
			    npcStoreShow.GetComponent<UI_NpcStoreShow>().NpcID = NpcID;

		    break;
            //仓库
            case "3":
                GameObject npcStoreHouse = (GameObject)Instantiate(Obj_RoseStoreHouse);
                npcStoreHouse.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcStoreHouse.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcStoreHouse.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                npcStoreHouse.transform.localScale = new Vector3(1, 1, 1);
                //npcStoreHouse.GetComponent<UI_NpcStoreShow>().NpcID = NpcID;
            break;
            //洗练
            case "4":
                GameObject npcXiLian = (GameObject)Instantiate(Obj_EquipXiLi);
                npcXiLian.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcXiLian.transform.localScale = new Vector3(1, 1, 1);
                npcXiLian.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcXiLian.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

            break;
		    //回收
		    case "5":
			    GameObject npcHuiShou = (GameObject)Instantiate(Obj_HuiShouItem);
			    npcHuiShou.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcHuiShou.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcHuiShou.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
			    npcHuiShou.transform.localScale = new Vector3(1, 1, 1);
                npcHuiShou.GetComponent<UI_HuiShouItem>().NpcID = NpcID;
			    break;
            //商店2
            case "6":
                GameObject npcStoreShow_2 = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UINpcStoreShow_2);
                npcStoreShow_2.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcStoreShow_2.transform.localScale = new Vector3(1, 1, 1);
                npcStoreShow_2.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcStoreShow_2.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                npcStoreShow_2.GetComponent<UI_NpcStoreShow_2>().NpcID = NpcID;
                break;

            //学习制造
            case "8":
                GameObject npcItemMake = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIItemMakeLearn);
                npcItemMake.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcItemMake.transform.localScale = new Vector3(1, 1, 1);
                npcItemMake.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcItemMake.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                npcItemMake.GetComponent<UI_ItmeMakeLearn>().NpcID = NpcID;
                break;
            
			//邮箱
			case "12":
				GameObject npcMail = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIMail);
				npcMail.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
				npcMail.transform.localScale = new Vector3(1, 1, 1);
				npcMail.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
				npcMail.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
				break;

            //大秘境
            case "13":
                GameObject npcDaMiJing = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIDaMiJing);
                npcDaMiJing.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcDaMiJing.transform.localScale = new Vector3(1, 1, 1);
                npcDaMiJing.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcDaMiJing.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;

            //大秘境
            case "18":
                GameObject zhulingObj = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().FunctionInstantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_ZhenYing, "UI_ZhuLingSet");
                //GameObject zhulingObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().UI_ZhuLingSet);
                zhulingObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                zhulingObj.transform.localScale = new Vector3(1, 1, 1);
                zhulingObj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                zhulingObj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;

            //牧场
            case "31":
                GameObject npcPastureUpLvSet = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_PastureMain);
                npcPastureUpLvSet.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcPastureUpLvSet.transform.localScale = new Vector3(1, 1, 1);
                npcPastureUpLvSet.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcPastureUpLvSet.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;

            //牧场兑换大厅
            case "32":
                GameObject npcPastureDuiHuanDaTing = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_PastureDuiHuanDaTing);
                npcPastureDuiHuanDaTing.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcPastureDuiHuanDaTing.transform.localScale = new Vector3(1, 1, 1);
                npcPastureDuiHuanDaTing.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcPastureDuiHuanDaTing.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;


            //牧场仓库
            case "33":
                GameObject npcPastureBag = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_PastureBag);
                npcPastureBag.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcPastureBag.transform.localScale = new Vector3(1, 1, 1);
                npcPastureBag.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcPastureBag.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;

            //牧场合成
            case "34":
                GameObject npcPastureHeChengSet = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_PastureHeChengSet);
                npcPastureHeChengSet.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcPastureHeChengSet.transform.localScale = new Vector3(1, 1, 1);
                npcPastureHeChengSet.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcPastureHeChengSet.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;

            //牧场公告
            case "35":
                GameObject npcPastureGongGaoSet = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_PastureGongGaoSet);
                npcPastureGongGaoSet.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcPastureGongGaoSet.transform.localScale = new Vector3(1, 1, 1);
                npcPastureGongGaoSet.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcPastureGongGaoSet.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;

            //坐骑大师
            case "41":
                GameObject npcZuoQiSet = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiSet);
                npcZuoQiSet.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcZuoQiSet.transform.localScale = new Vector3(1, 1, 1);
                npcZuoQiSet.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcZuoQiSet.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;


            //染色大师
            case "42":
                GameObject npcRanSeSet = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_RanSeSet);
                npcRanSeSet.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                npcRanSeSet.transform.localScale = new Vector3(1, 1, 1);
                npcRanSeSet.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                npcRanSeSet.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                break;


            //国王大厅
            case "101":
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickBuilding();
            if (guoWangDaTingObj == null)
            {
                guoWangDaTingObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_GuoWangDaTing);
                guoWangDaTingObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet_2.transform);
                guoWangDaTingObj.transform.localPosition = Vector3.zero;
                guoWangDaTingObj.transform.localScale = new Vector3(1, 1, 1);
                guoWangDaTingObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                guoWangDaTingObj.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
                guoWangDaTingObj.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
            }
            break;

            //修炼中心
            case "102":

            if (dayPracticeRewardObj == null)
            {
                dayPracticeRewardObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_DayPracticeReward);
                dayPracticeRewardObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet_2.transform);
                dayPracticeRewardObj.transform.localPosition = Vector3.zero;
                dayPracticeRewardObj.transform.localScale = new Vector3(1, 1, 1);
                dayPracticeRewardObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                dayPracticeRewardObj.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
                dayPracticeRewardObj.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
            }

            break;

            //荣誉大厅
            case "103":
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickTrainCamp();duiHuanDaTingObj
            if (duiHuanDaTingObj == null)
            {
                duiHuanDaTingObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_DuiHuanDaTing);
                duiHuanDaTingObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet_2.transform);
                duiHuanDaTingObj.transform.localPosition = Vector3.zero;
                duiHuanDaTingObj.transform.localScale = new Vector3(1, 1, 1);
                duiHuanDaTingObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                duiHuanDaTingObj.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
                duiHuanDaTingObj.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
            }
            break;

            //试炼之塔
            case "104":
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickFortress();
            //Debug.Log("点击了要塞");
            if (shiLianZhiTaObj == null)
            {
                shiLianZhiTaObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_ShiLianZhiTa);
                shiLianZhiTaObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet_2.transform);
                shiLianZhiTaObj.transform.localPosition = Vector3.zero;
                shiLianZhiTaObj.transform.localScale = new Vector3(1, 1, 1);
                shiLianZhiTaObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                shiLianZhiTaObj.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
                shiLianZhiTaObj.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
            }

            break;

            //幸运殿堂
            case "105":
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickTrainCamp();

            if (chouKaObj == null)
            {
                chouKaObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_ChouKa);
                chouKaObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet_2.transform);
                chouKaObj.transform.localPosition = Vector3.zero;
                chouKaObj.transform.localScale = new Vector3(1, 1, 1);
                chouKaObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                chouKaObj.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
                chouKaObj.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
            }

            break;

            //每日任务
            case "106":
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickTrainCamp();

            if (dayTaskObj == null)
            {
                dayTaskObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_DayTask);
                dayTaskObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet_2.transform);
                dayTaskObj.transform.localPosition = Vector3.zero;
                dayTaskObj.transform.localScale = new Vector3(1, 1, 1);
                dayTaskObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                dayTaskObj.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
                dayTaskObj.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
            }

            break;

            //收集大厅
            case "107":
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Btn_ClickTrainCamp();

            if (shouJiDaTingObj == null)
            {
                shouJiDaTingObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_ShouJiDaTing);
                shouJiDaTingObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet_2.transform);
                shouJiDaTingObj.transform.localPosition = Vector3.zero;
                shouJiDaTingObj.transform.localScale = new Vector3(1, 1, 1);
                shouJiDaTingObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                shouJiDaTingObj.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
                shouJiDaTingObj.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
            }
            break;

		}

	}

}
