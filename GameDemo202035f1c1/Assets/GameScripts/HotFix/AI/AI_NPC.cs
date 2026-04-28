using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AI_NPC : MonoBehaviour {

    public string NpcID;
    public string NpcIDLvStr;
    private Vector2 vec3_NpcName;
    public GameObject NpcNamePosition;
    private GameObject Obj_NpcName;
    private bool npcNameUpdateStatus;
    public GameObject SelectEffect;             //玩家选中这个NPC播放特效
    public GameObject SelectEffectPosition;     //玩家选中特效播放的点
    public bool IfSeclectNpc;                   //玩家是否选中这个Npc
    public bool IfSeclectNpcEffect;             //玩家选中这个Npc实例化的特效，保证只执行一次
    private GameObject selectEffect;            //实例化的选中特效
    private bool selectCancleStatus;            //取消选中后执行第一次
    public ArrayList CompleteTaskID;            //完成任务的ID
    public bool CompleteTaskStatus;             //完成任务只执行一次
    public bool waritUpdataTask;                //待更新状态
    private float taskUpdataTime;               //30码内每隔几秒自动是否有任务完成,修复30码内，杀怪任务或其他任务完成后不更新（不得已的办法）
    private float taskUpdataTimeSum;            
    private bool npcHeadSpeakStatus;            //NPC头上说话状态
    private string[] npcStoryValue;             //NPC身上绑定的故事ID
    public bool ShowNpcNameStatus;              //显示NPC名称状态

    public GameObject npcAnimatorObj;
    private Animator npcAnimator;
    private string movePosition;
    private Quaternion basePositionVec3;
    private float npcMoveSpeed;
    private List<Vector3> moveVec3;
    private int nowMoveVec3ID;
    private bool npcIfMoveStatus;
    private Vector3 nowMoveVec3;
    private bool npcMoveStatus;
    private bool npcNextPositionStatus;
    private float npcMoveWaitTime;
    private float npcMoveWaitTimeSum;
    private UnityEngine.AI.NavMeshAgent ai_nav;
    public UI_NPCName npcNameScri;

    // Use this for initialization
    void Start () {

        //更新NPCID
        UpdateNpcID();

        //展示Npc
        showNpc();

        //等级出现
        string showRoseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowRoseLv", "ID", NpcID, "Npc_Template");

        if (showRoseLv != "0" && showRoseLv!="") {
            int lvMin = int.Parse(showRoseLv.Split(';')[0]);
            int lvMax = int.Parse(showRoseLv.Split(';')[1]);
            int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
            if (roseLv < lvMin || roseLv > lvMax)
            {
                Debug.Log("等级不匹配,NPC销毁自身");
                //销毁自己
                Destroy(this.gameObject);
                return;
            }
        }

        npcNameUpdateStatus = true;
        taskUpdataTime = 5.0f;
        npcMoveWaitTime = 5.0f;             //等待时间
        moveVec3 = new List<Vector3>();
        ai_nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (ai_nav != null) {
            //获取当前NPC类型
            npcMoveSpeed = ai_nav.speed;
            npcAnimator =this.GetComponent<Animator>();
            movePosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MovePosition", "ID", NpcID, "Npc_Template");
            if (movePosition != "" && movePosition != "0")
            {
                npcIfMoveStatus = true;
                npcNextPositionStatus = true;
                string[] movePositionList = movePosition.Split(';');
                if (movePositionList[0] != "" && movePositionList[0] != "0")
                {
                    for (int i = 0; i < movePositionList.Length; i++)
                    {
                        string[] vec3List = movePositionList[i].Split(',');
                        moveVec3.Add(new Vector3(float.Parse(vec3List[0]), float.Parse(vec3List[1]), float.Parse(vec3List[2])));
                    }
                }
                nowMoveVec3ID = 0;
                nowMoveVec3 = moveVec3[nowMoveVec3ID];
                //Debug.Log("nowMoveVec3 = " + nowMoveVec3.ToString());
            }
        }

        //检测自身是否有可以学习的制造技能

	}
	
	// Update is called once per frame
	void Update () {
        //判定与角色相距17米进行名称显示
        float distance = Vector3.Distance(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position, this.gameObject.transform.position);

        if (Game_PublicClassVar.Get_game_PositionVar.UpdatTaskStatus) {
            ShowNpcNameStatus = true;
        }

        if (ShowNpcNameStatus)
		//if(distance>=0)
		{
            if (distance <= 17.0f)
            {
                //修正物体在屏幕中的位置
                vec3_NpcName = Camera.main.WorldToViewportPoint(NpcNamePosition.transform.position);
                vec3_NpcName = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(vec3_NpcName);

                //第一次显示
                if (npcNameUpdateStatus == true)
                {
                    //Debug.Log("显示npc名称第一次触发！！！！");
                    npcNameUpdateStatus = false;
                    //实例化UI
                    if (Obj_NpcName != null) {
                        Destroy(Obj_NpcName);
                    }

                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_UINpcName != null)
                    {
                        Obj_NpcName = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UINpcName);
                        if (Obj_NpcName != null)
                        {
                            Obj_NpcName.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcNameSet.transform);
                            Obj_NpcName.transform.localPosition = new Vector3(vec3_NpcName.x, vec3_NpcName.y, 0);
                            Obj_NpcName.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                            npcNameScri = Obj_NpcName.GetComponent<UI_NPCName>();
                            npcNameScri.Obj_Npc = this.gameObject;
                            //显示Npc名称
                            string NpcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", NpcID, "Npc_Template");
                            //Obj_NpcName.transform.Find("Lab_NpcName").transform.GetComponent<Text>().text = NpcName;

                            npcNameScri.Obj_NpcName.transform.GetComponent<Text>().text = NpcName;
                            npcNameScri.NpcID = NpcID;
                            //有街区任务显示图标
                            string[] npcTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskID", "ID", NpcID, "Npc_Template").Split(';');
                            //循环创建任务列表
                            for (int i = 0; i <= npcTaskIDList.Length - 1; i++)
                            {
                                if (npcTaskIDList[i] != "" && npcTaskIDList[i] != "0")
                                {
                                    string taskStatus = Game_PublicClassVar.Get_function_Task.TaskReturnStatus(npcTaskIDList[i]);
                                    //实例化任务Btn
                                    if (taskStatus == "1")
                                    {
                                        //显示接取任务
                                        //Obj_NpcName.transform.Find("Img_TaskGet").gameObject.SetActive(true);
                                        //Obj_NpcName.transform.Find("Img_TaskComplete").gameObject.SetActive(false);

                                        //每日任务直接发放,此处不处理
                                        string taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", npcTaskIDList[i], "Task_Template");
                                        if (taskType != "3")
                                        {
                                            npcNameScri.Obj_TaskGet.gameObject.SetActive(true);
                                            npcNameScri.Obj_TaskComplete.gameObject.SetActive(false);
                                        }
                                    }
                                }
                            }
                            //更新图标显示
                            createShiMenShow();
                            createCompleteTask();

                            //如果是制造提供特殊标签进行显示
                            string npcType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcType", "ID", NpcID, "Npc_Template");
                            if (npcType == "8")
                            {
                                npcNameScri.Obj_MakeHint.SetActive(true);
                            }
                        }
                    }
                }
                //修正掉落物体的位置
                if (Obj_NpcName != null)
                {
                    Obj_NpcName.transform.localPosition = new Vector3(vec3_NpcName.x, vec3_NpcName.y, 0);
                }

                //修正Npc头顶的任务状态
                if (Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow)
                {
                    //有接取任务显示图标
                    string[] npcTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskID", "ID", NpcID, "Npc_Template").Split(';');
                    if (npcTaskIDList[0] != "0")
                    {

                        waritUpdataTask = false;
                        //显示接取任务
                        if (Obj_NpcName != null)
                        {
                            if (Obj_NpcName.GetComponent<UI_NPCName>() != null)
                            {
                                UI_NPCName npcNameScri = Obj_NpcName.GetComponent<UI_NPCName>();
                                npcNameScri.Obj_TaskGet.gameObject.SetActive(false);
                                npcNameScri.Obj_TaskComplete.gameObject.SetActive(false);
                            }
                        }
                        //循环创建任务列表
                        for (int i = 0; i <= npcTaskIDList.Length - 1; i++)
                        {
                            if (npcTaskIDList[i] != "" && npcTaskIDList[i] != "0")
                            {
                                string taskStatus = Game_PublicClassVar.Get_function_Task.TaskReturnStatus(npcTaskIDList[i]);
                                //实例化任务Btn
                                if (taskStatus == "1")
                                {
                                    //显示接取任务
                                    npcNameScri.Obj_TaskGet.gameObject.SetActive(true);
                                    npcNameScri.Obj_TaskComplete.gameObject.SetActive(false);
                                }
                            }
                        }
                        createShiMenShow();
                        createCompleteTask();
                    }
                }

                //显示快捷对话
                if (distance <= 5.0f)
                {
                    if (!npcHeadSpeakStatus)
                    {
                        //Debug.Log("头顶说话");
                        if (Obj_NpcName != null)
                        {
                            npcHeadSpeakStatus = true;
                            Obj_NpcName.GetComponent<UI_NPCName>().HeadSpeakStatus = true;
                        }
                    }
                }
                else
                {

                    //Obj_NpcName.GetComponent<UI_NPCName>().Obj_HeadSpeak.active = false;

                }

                //当接受到更新完成任务状态时,先保存，等进入NPC范围在更新8

                if (waritUpdataTask)
                {
                    waritUpdataTask = false;
                    createCompleteTask();
                }


                //判定主角是否有任务已经完成,交任务的NPC是否为自己
                if (!CompleteTaskStatus)
                {
                    //确保只执行一次
                    CompleteTaskStatus = true;
                    createCompleteTask();
                }

                //5秒检测附近玩家有没有任务
                /*
                taskUpdataTimeSum = taskUpdataTimeSum +Time.deltaTime;
                if (taskUpdataTimeSum >= taskUpdataTime) {
                    taskUpdataTimeSum = 0;
                    createCompleteTask();
                }
                */
            }
            else
            {
                waritUpdataTask = true;
                npcHeadSpeakStatus = false;  //npc头顶说话

                if (Obj_NpcName != null)
                {
                    Destroy(Obj_NpcName);
                }
                npcNameUpdateStatus = true;
            }
        
        }



        //NPC被玩家选中
        if (IfSeclectNpc)
        {
            if (!IfSeclectNpcEffect)
            {
                //实例化一个选中特效
                if (SelectEffect != null)
                {
                    selectEffect = (GameObject)Instantiate(SelectEffect);
                    selectEffect.transform.SetParent(SelectEffectPosition.transform);
                    selectEffect.transform.localScale = new Vector3(1, 1, 1);
                    selectEffect.transform.localPosition = new Vector3(0, 0, 0);

                    selectEffect.GetComponent<Rose_SelectTarget>().SelectEffectSize = 1;
                    IfSeclectNpcEffect = true;
                    selectCancleStatus = true;
                    basePositionVec3 = this.gameObject.transform.rotation;
                }
            }

            //NPC触发剧情对话
            if (distance <= 3.0f) {
                
                Npc_MoveStop();
            }
        }
        else {
            Destroy(selectEffect);
            IfSeclectNpcEffect = false;
            if (selectCancleStatus) {
                selectCancleStatus = false;
                if (movePosition != "" && movePosition != "0")
                {
                    Npc_MoveStart();
                }
                else {
                    ///this.transform.LookAt(basePositionVec3);
                    //Debug.Log("调整方向！调整方向！调整方向！调整方向！调整方向！");
                    this.transform.rotation = basePositionVec3;
                }
            }
        }

        //更新完成任务列表
        if (Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus=="1") {
            createCompleteTask();
            Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus = "2";
        }

        //修正Npc头顶的任务状态
        if (Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow) {
            waritUpdataTask = true;
        }


        //npc巡逻
        if (npcIfMoveStatus)
        {
            //npcAnimatorObj.transform.localPosition = new Vector3(npcAnimatorObj.transform.localPosition.x,0,npcAnimatorObj.transform.localPosition.z) ;       //如果不加总是往下掉
            //开始移动到坐标点
            if (npcNextPositionStatus) {
                npcNextPositionStatus = false;
                //移动到目标点
                ai_nav.SetDestination(nowMoveVec3);
                npcMoveStatus = true;
                playAnimator("2");
            }

            //移动中持续判定自身与坐标的位置
            if (npcMoveStatus) {
                //Debug.Log("this.gameObject.transform.position = " + this.gameObject.transform.position);
                float disValue =Vector3.Distance(nowMoveVec3, this.gameObject.transform.position);
                //Debug.Log("disValue = " + disValue);
                if (disValue < 1.0f) {
                    //Debug.Log("aaaaaaaaaaaaaaaaaaasssssssssssssssss");
                    npcMoveWaitTimeSum = npcMoveWaitTimeSum + Time.deltaTime;
                    if (npcMoveWaitTimeSum >= npcMoveWaitTime)
                    {
                        npcMoveStatus = false;
                        npcMoveWaitTimeSum = 0;
                        //获取下次移动坐标点
                        if (nowMoveVec3ID >= moveVec3.Count - 1)
                        {
                            nowMoveVec3ID = 0;
                        }
                        else
                        {
                            nowMoveVec3ID = nowMoveVec3ID + 1;
                        }
                        nowMoveVec3 = moveVec3[nowMoveVec3ID];
                        npcNextPositionStatus = true;
                    }
                    else {
                        if (!ai_nav.hasPath)
                        {
                            playAnimator("1");
                        }
                    }
                }
            }
        }

        //隐藏NPC更新状态
        if (Game_PublicClassVar.Get_game_PositionVar.NpcShowValueStatus) {
            showNpc();
        }
	}

    public void Npc_MoveStop() {
        if (ai_nav != null) {
            ai_nav.speed = 0;
            this.transform.LookAt(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
            playAnimator("1");
        }
    }

    public void Npc_MoveStart() {
        //Debug.Log("asdasdasd");
        if (ai_nav != null)
        {
            ai_nav.speed = npcMoveSpeed;
            playAnimator("2");
        }
    }


    void playAnimator(string animatorTyoe) {
        if (animatorTyoe == "2") {
            //Debug.Log("RunRunRunRunRunRunRunRun!");
        }
        npcAnimator.SetBool("Run", false);
        npcAnimator.SetBool("Idle", false);
        switch (animatorTyoe) { 
            //待机
            case "1":
                npcAnimator.SetBool("Idle", true);
                break;
            //走路
            case "2":
                npcAnimator.SetBool("Run", true);
                break;
        }
    }

    //创建完成任务
    void createCompleteTask() {
        //Debug.Log("有完成任务显示图标1111");
        //获取主角当前携带的任务
        string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        CompleteTaskID = new ArrayList();
        for (int i = 0; i <= taskIDList.Length - 1; i++)
        {
            if(taskIDList[i]!="" || taskIDList[i]!="0"){
                //判定当前任务是否完成
                if (Game_PublicClassVar.Get_function_Task.TaskComplete(taskIDList[i]))
                {
                    //获取当前交任务的NPC是否是自己
                    string completeNpcID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteNpcID", "ID", taskIDList[i], "Task_Template");

                    //判定Npc是否符合完成任务或特殊的试炼和日常任务  
                    if (completeNpcID == NpcID || YanZhengNpcCom(completeNpcID))
                    {
                        CompleteTaskID.Add(taskIDList[i]);
                        //显示交任务图标
                        if (Obj_NpcName != null)
                        {
                            Obj_NpcName.transform.Find("Img_TaskGet").gameObject.SetActive(false);
                            Obj_NpcName.transform.Find("Img_TaskComplete").gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
    
    //创建每日任务显示
    void createShiMenShow() {

        //获取当前角色是否有每日任务进行显示
        string npcType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcType", "ID", NpcID, "Npc_Template");
        if (npcType == "7")
        {
            //获取当前是否有每日任务
            List<string> task_Everyday = new List<string>();        
            task_Everyday = Game_PublicClassVar.Get_function_Task.GetRoseTaskTypeID("3");
            //当前身上有每日任务
            if (task_Everyday.Count > 0) {
                Obj_NpcName.transform.Find("Img_TaskGet").gameObject.SetActive(false);
                Obj_NpcName.transform.Find("Img_TaskComplete").gameObject.SetActive(false);
                return;
            }

            //获取当前每日完成的任务是否超过10个
            int taskShiMenNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMenTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            if (taskShiMenNum < 10)
            {
                //显示任务图标
                Obj_NpcName.transform.Find("Img_TaskGet").gameObject.SetActive(true);
                Obj_NpcName.transform.Find("Img_TaskComplete").gameObject.SetActive(false);
            }
        }

        if (npcType == "15")
        {
            //判定当前是否接取试炼任务
            string shiLianTaskStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianTaskStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            if (shiLianTaskStatus == "1")
            {
                //保险起见,检测一下自身的试炼任务
                bool ifJianCeStatus = Game_PublicClassVar.Get_function_Task.GetShiLianTaskStatus();

                //显示任务图标
                if (ifJianCeStatus == true)
                {
                    Obj_NpcName.transform.Find("Img_TaskGet").gameObject.SetActive(false);
                    Obj_NpcName.transform.Find("Img_TaskComplete").gameObject.SetActive(false);
                }
                else {
                    Obj_NpcName.transform.Find("Img_TaskGet").gameObject.SetActive(true);
                    Obj_NpcName.transform.Find("Img_TaskComplete").gameObject.SetActive(false);
                    //修正试炼任务状态
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianTaskStatus", "0","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                }
            }
        }

        //createCompleteTask();
    }

    //更新NPCID
    public void UpdateNpcID() {

        //Debug.Log("NpcIDLvStr = " + NpcIDLvStr);
        if (NpcIDLvStr == "" || NpcIDLvStr == "0" || NpcIDLvStr == null)
        {
            return;
        }

        //NPCID列表
        string [] NpcIDList = NpcIDLvStr.Split(';');
        for (int i = 0; i < NpcIDList.Length; i++)
        {
            //Debug.Log("NpcIDList[i] = " + NpcIDList[i]);
            //等级出现
            string showRoseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowRoseLv", "ID", NpcIDList[i], "Npc_Template");
            if (showRoseLv != "0" && showRoseLv != "")
            {
                int lvMin = int.Parse(showRoseLv.Split(';')[0]);
                int lvMax = int.Parse(showRoseLv.Split(';')[1]);
                int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                //Debug.Log("roseLv = " + roseLv + ";showRoseLv = " + showRoseLv);
                if (roseLv < lvMin || roseLv > lvMax)
                {

                }
                else {
                    //Debug.Log("等级匹配");
                    NpcID = NpcIDList[i];
                    return;
                }
            }
        }
    }

    //触发NPC
    public void showNpc() {

        showObj(false);
        string showType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowType", "ID", NpcID, "Npc_Template");
        string showValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowValue", "ID", NpcID, "Npc_Template");

        //Debug.Log("更新NPC!!!!!!!!!!!!!!!!!!!! showType = " + showType + ";showValue = " + showValue + ";NpcID = " + NpcID);

        switch (showType) { 
            
            //默认显示
            case "0":
                showObj(true);

                break;

            //判定出现时间
            case "1":
                if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus) {
                    float timeMin = float.Parse(showValue.Split(',')[0]);
                    float timeMax = float.Parse(showValue.Split(',')[1]);

                    float updataDayTime = 86400 - (Game_PublicClassVar.Get_wwwSet.dayUpdataTime - Time.realtimeSinceStartup);
                    if (updataDayTime >= timeMin) {
                        if (timeMax >= updataDayTime) {
                            //符合出现时间,出现
                            showObj(true);
                        }
                    }
                }
                break;
            //地图击杀怪物
            case "2":
                int killMonster = int.Parse(showValue);
                Debug.Log("地图杀怪！！！！！！！！！！！！！！！！！！！！！killMonster = " + killMonster);
                if (Game_PublicClassVar.Get_game_PositionVar.MapKillMonsterNum >= killMonster) {
                    //符合出现时间,出现
                    showObj(true);
                }
                break;

            //地图内击败某个怪物ID
            case "3":
                if (showValue != "" && showValue != "0") {
                    if (Game_PublicClassVar.Get_game_PositionVar.MapKillBossID == showValue)
                    {
                        //符合出现时间,出现
                        showObj(true);
                    }
                }
                break;

            //当自身某个ID的任务完成后出现
            case "4":
                string returnValue = Game_PublicClassVar.Get_function_Task.TaskReturnStatus(showValue);
                Debug.Log("showValue = " + showValue + ";returnValue = " + returnValue);
                if (returnValue == "2")
                {
                    //符合出现时间,出现
                    showObj(true);
                }
                break;

            //穿戴要求指定数量的装备出现
            case "5":
                int needEquipNum = int.Parse(showValue.Split(';')[0]);
                string needEquipIDStr = showValue.Split(';')[1];
                string[] needEquipIDList = needEquipIDStr.Split(',');
                int needEquipNumSum = 0;
                for (int i = 0; i < needEquipIDList.Length; i++) {
                    int equipNum = Game_PublicClassVar.Get_function_Rose.IfWearEquipID(needEquipIDList[i]);
                    needEquipNumSum = needEquipNumSum + equipNum;
                }
                if (needEquipNumSum >= needEquipNum) {
                    //符合出现时间,出现
                    showObj(true);
                }
                break;
            //背包内有某个道具出现
            case "6":
                string needItemID = showValue.Split(',')[0];
                int needItemNum = int.Parse(showValue.Split(',')[1]);
                if (Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(needItemID) >= needItemNum)
                {
                    //符合出现时间,出现
                    showObj(true);
                }
                break;
        
        }

    }


    private void showObj(bool showType) {

        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject go = this.transform.GetChild(i).gameObject;
            go.SetActive(showType);
        }
        if (showType)
        {
            //更新名称显示状态
            ShowNpcNameStatus = true;
        }
        else {
            //更新名称显示状态
            ShowNpcNameStatus = false; 
        }
    }


    //验证Npc
    private bool YanZhengNpcCom(string nowNPCID)
    {
        if (NpcIDLvStr == "" || NpcIDLvStr == "0" || NpcIDLvStr == null)
        {
            return false;
        }

        //NPCID列表
        string[] NpcIDList = NpcIDLvStr.Split(';');
        for (int i = 0; i < NpcIDList.Length; i++)
        {
            if (nowNPCID == NpcIDList[i]) {
                return true;
            }

        }

        return false;
    }
}
