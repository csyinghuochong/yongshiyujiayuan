using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CodeStage.AntiCheat.ObscuredTypes;

//任务相关的子程序
public class Function_Task {

    private int taskNum;  //任务数量上限
    private string roseName; //主角名称
    private string roseID; //主角名称

    //绑点专用
    private GameObject gameStartVar;
    private Game_PositionVar game_PositionVar;
    
    //接取任务
    public bool GetTask(string taskID) {
        try
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_211");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("接取任务成功!");
            if (taskID == "30100100") {
                //新手引导(红狼任务引导打开地图)
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_RoseMap();
            }
        }
        catch {

        }
        //获取任务ID在第几个位置
        string taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (taskIDList != "")
        {
            taskIDList = taskIDList + "," + taskID;
        }
        else {
            taskIDList = taskID;
        }
        
        //获取任务当前完成值
        string taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (taskValueList != "")
        {
            taskValueList = taskValueList + ";0,0,0";
        }
        else {
            taskValueList = "0,0,0";
        }


        //写入任务值
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskID", taskIDList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", taskValueList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

		//更新Npc头部任务状态显示
		Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true;

		//获取当前追踪任务ID数量，少于3将刚接取的任务直接列入追踪任务中
		WriteMainUITaskID (taskID);


        //试炼任务需要写入状态
        
        string taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", taskID, "Task_Template");
        if (taskType == "2")
        {
            string taskSonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskSonType", "ID", taskID, "Task_Template");
            if (taskSonType == "1")
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianTaskStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                //记录下次的要领取的任务
                //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianNextTaskID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            }
        }

        if(taskType == "3") {
            //下次的要领取的任务
            //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiMenNextTaskID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        }
        
        //如果有获取道具任务更新任务显示
        string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID",taskID, "Task_Template");

        //杀怪
        if (targetType == "1") {
            string target1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", taskID, "Task_Template");
            string MonsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", target1, "Monster_Template");
            if (MonsterType == "3") {
                //如果是BOSS就检测当前是否已经击杀BOSS
                string deathMonsterIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string[] deathMonsterID = deathMonsterIDList.Split(';');
                for (int i = 0; i < deathMonsterID.Length; i++)
                {
                    if (deathMonsterID[i] != "")
                    {
                        string[] deathMonsterList = deathMonsterID[i].Split(',');
                        //获取怪物ID
                        string monsterID = "";
                        if (deathMonsterList.Length >= 4)
                        {
                            monsterID = deathMonsterList[3];
                        }
                        if (target1 == monsterID) {
                            //表示当前BOSS已经被击杀
                            TaskMonsterNum(monsterID, 1);
                        }
                    }
                }
            }
        }

        //采集
		if (targetType == "2") 
        {
			updataTaskItemID();
		}

        //师门
        if (targetType == "3")
        {
            /*
            int taskShiMenNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMenTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            taskShiMenNum = taskShiMenNum + 1;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiMenTaskNum", taskShiMenNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            */
            updataTaskItemID();
        }
        


        if (targetType == "4")
        {
            updataTaskItemID();
        }

        if (targetType == "7")
        {
            updataTaskItemID();
        }

        Game_PublicClassVar.Get_function_UI.PlaySource("10010", "1");

        return true;
    
    }

    //写入角色任务进度
    public bool WariteTaskPro(string taskID,string proID,string proValue) {
        /*
        IniScript iniScript = new IniScript();

        //获取绑点
        GameObject gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
        Game_PositionVar game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();

        iniScript.InIWrite(taskID, proID, proValue, Application.dataPath + "/Game_DB/INI/" + game_PositionVar.Rose_ID + ".ini");
         */
        return true;
       
    }

    

    //查询当前NPC身上是否有自身的任务
    //NpcID 需要查询的NpcID
    public string[] TaskRoseQuery(string NpcID)
    {
        /*
        //获取当前自身的任务
        //获取绑点
        gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
        game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();

        //获取NPC身上的任务
        //DB_ReadDate db_ReadDate = new DB_ReadDate();
        IniScript iniScript = new IniScript();
        XMLScript xmlScript = new XMLScript();

        char[] fenge = { ';' };
        string[] npc_TaskID = xmlScript.Xml_GetDate("TaskID", "ID", NpcID, game_PositionVar.Xml_Path + "Npc_Template.xml").Split(fenge);

        string rose_TaskIDstr = "";
        string queryValue = "";

        for (int i = 1; i <= game_PositionVar.Rose_TaskNum; i++)
        {
            queryValue = iniScript.InIRead("Rose_Task", "TaskID" + i.ToString(), Game_PublicClassVar.Get_game_PositionVar.Ini_Path + "RoseTask_" + Game_PublicClassVar.Get_game_PositionVar.Rose_ID + ".ini");

            if (queryValue == "0")
            {

            }
            else {

                rose_TaskIDstr = rose_TaskIDstr + queryValue + ";";

            }

        }

        //处理多余的字符串
        if(rose_TaskIDstr != "" ){

            rose_TaskIDstr = rose_TaskIDstr.Substring(0, rose_TaskIDstr.Length-1);
        
        }

        string[] rose_TaskID = rose_TaskIDstr.Split(fenge);

        //对比重复的数组

        string QueryTaskStr = ""; //存储重复的数据

        for (int i = 1; i <= rose_TaskID.Length; i++) {

            for (int n = 1; n <= npc_TaskID.Length; n++) {

                if (rose_TaskID[i - 1] == npc_TaskID[n - 1]) {

                    QueryTaskStr = QueryTaskStr + rose_TaskID[i - 1] + ";"; 

                }

            }
        
        }

        if (QueryTaskStr != "")
        {

            QueryTaskStr = QueryTaskStr.Substring(0, QueryTaskStr.Length-1);

        }

        string[] QueryTask = QueryTaskStr.Split(fenge);
        
        return QueryTask;
         */

        //删
        char[] fenge = { ';' };
        string[] QueryTask = NpcID.Split(fenge);
        return QueryTask;

    }

    //查询任务是否完成
    //TaskID 任务ID
    public bool TaskComplete(string TaskID) {
        
        if (TaskID != "")
        {
            string TaskValuePro1 = TaskReturnValue(TaskID, "1");
            string TaskValuePro2 = TaskReturnValue(TaskID, "2");
            string TaskValuePro3 = TaskReturnValue(TaskID, "3");
            if (TaskValuePro1 == "" || TaskValuePro2 == "" || TaskValuePro3 == "") {
                return false;
            }


            string TargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", TaskID, "Task_Template");
            string TargetValue2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue2", "ID", TaskID, "Task_Template");
            string TargetValue3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue3", "ID", TaskID, "Task_Template");

            if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
            {
                if (int.Parse(TaskValuePro2) >= int.Parse(TargetValue2))
                {
                    if (int.Parse(TaskValuePro3) >= int.Parse(TargetValue3))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else {
            return false;
        }
    }


    //查询任务是否完成,全部完成返回0,返回1-3表示对应条件的任务未完成,返回-1表示执行错误
    //TaskID 任务ID
    public string TaskIncompleteReturnValue(string TaskID)
    {

        string IncompleteValue = "-1";
        if (TaskID != "")
        {
            string TaskValuePro1 = TaskReturnValue(TaskID, "1");
            string TaskValuePro2 = TaskReturnValue(TaskID, "2");
            string TaskValuePro3 = TaskReturnValue(TaskID, "3");

            string TargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", TaskID, "Task_Template");
            string TargetValue2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue2", "ID", TaskID, "Task_Template");
            string TargetValue3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue3", "ID", TaskID, "Task_Template");

            if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
            {
                if (int.Parse(TaskValuePro2) >= int.Parse(TargetValue2))
                {
                    if (int.Parse(TaskValuePro3) >= int.Parse(TargetValue3))
                    {
                        IncompleteValue = "0";
                    }
                    else
                    {
                        IncompleteValue = "3";
                    }
                }
                else
                {
                    IncompleteValue = "2";
                }
            }
            else
            {
                IncompleteValue = "1";
            }
        }

        return IncompleteValue;

    }

    //根据任务ID发放任务奖励
    public bool TaskRewards(string TaskID) {

        //根据任务ID获取对应的奖励 
        string[] rewardItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", TaskID, "Task_Template").Split(',');
        string[] rewardItemValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", TaskID, "Task_Template").Split(',');

        //获取背包剩余格子数量，判定奖励数量是否大于格子数量
        if (Game_PublicClassVar.Get_function_UI.BagSpaceNullNum() < rewardItemID.Length)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_329");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("背包已满,请先清空背包在提交任务！");
            return false;       //背包格子不足,领取奖励失败
        }


        try {
            //发送经验或金币奖励
            string taskTrophyCoin = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskCoin", "ID", TaskID, "Task_Template");
            string taskTrophyExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskExp", "ID", TaskID, "Task_Template");

            //每日任务按系数递增奖励（师门特殊处理经验和金币）
            string taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", TaskID, "Task_Template");
            if (taskType == "2")
            {
                string taskSonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskSonType", "ID", TaskID, "Task_Template");
                //试炼任务特殊处理
                if (taskSonType == "1") {
                    string shiLianTaskNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    taskTrophyCoin = EveryTaskRewardShow(taskTrophyCoin,"2");
                    taskTrophyExp = EveryTaskRewardShow(taskTrophyExp, "2");
                    //写入活跃任务
                    Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "122", "1");
                }
            }

            if (taskType == "3")
            {
                string shiMenTaskNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMenTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                taskTrophyCoin = EveryTaskRewardShow(taskTrophyCoin, "1");
                taskTrophyExp = EveryTaskRewardShow(taskTrophyExp, "1");

                //第10个师门任务发送奖励
                if (int.Parse(shiMenTaskNumStr) >= 9)
                {
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010043", 1,"0",0,"0",true,"18");
                }

                //写入活跃任务
                Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "7", "1");

            }

            //获取奖励类型
            string taskTrophyType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskTrophyType", "ID", TaskID, "Task_Template");
            if (taskTrophyType == "1")
            {

                //发送奖励
                if (rewardItemID != null)
                {
                    for (int i = 0; i <= rewardItemID.Length - 1; i++)
                    {
                        Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardItemID[i], int.Parse(rewardItemValue[i]),"0",0,"0",true,"20");
                    }
                }
            }

            //根据任务ID开启下面的章节关卡
            string openPveChapter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OpenPVEChapter", "ID", TaskID, "Task_Template");
            if (openPveChapter != "0")
            {
                Game_PublicClassVar.Get_function_Rose.UpdataPVEChapter(openPveChapter);
            }

            //发送经验奖励
            Game_PublicClassVar.Get_function_Rose.SendReward("1", taskTrophyCoin,"20");
            Game_PublicClassVar.Get_function_Rose.AddExp(int.Parse(taskTrophyExp));
        }
        catch {

        }

        //Debug.Log("任务奖励结束！");
        return true;
    }

    //每日任务奖励计算
    public string EveryTaskRewardShow(string rewardValue,string rewardType){

        string returnRewardValue = "0";

        switch (rewardType) {

            //每日任务计算方式
            case "1":
                string shiMenTaskNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMenTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (shiMenTaskNumStr == "" || shiMenTaskNumStr == null)
                {
                    shiMenTaskNumStr = "0";
                }
                float rewardPro = 1 + 0.2f * float.Parse(shiMenTaskNumStr);
                returnRewardValue = ((int)(float.Parse(rewardValue) * rewardPro)).ToString();
                break;

            //试炼任务计算方式
            case "2":
                string shiLianTaskNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (shiLianTaskNumStr == "" || shiLianTaskNumStr == null) {
                    shiLianTaskNumStr = "0";
                }
                rewardPro = 1.5f + 0.02f * float.Parse(shiLianTaskNumStr);
                returnRewardValue = ((int)(float.Parse(rewardValue) * rewardPro)).ToString();
                break;
        }
        return returnRewardValue;
    }



    //检测当前NPC是否有可接的任务，有的返回任务数组，没有返回1个数组，第一个值为空
    //NpcID       NPC的ID
    //NpcTaskID   NPC对应的任务ID数组
    public string[] IfTask(string NpcID,string[] NpcTaskID)
    {
        
        //获取自身已完成任务
        string[] roseCompleteTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        //获取自身接取任务
        string[] roseGetTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        
        /*
        //检测已完成的任务是否
        gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
        game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();

        roseID = game_PositionVar.Rose_ID;

        //*****************************************************************检测角色已完成的任务列表*****************************************************************
        
        //获得角色以完成的任务
        string path = game_PositionVar.Ini_RoseTask;
        IniScript iniScript = new IniScript();
        //string npcTask = iniScript.InIRead(roseID, NpcID, path);
        string npcTask = Game_PublicClassVar.Get_IniSprict.InIRead("RoseCompleteNpcTask", NpcID,Game_PublicClassVar.Get_game_PositionVar.Ini_Path+"RoseData_"+Game_PublicClassVar.Get_game_PositionVar.Rose_ID+".ini");
        //将字符串转换成数组
        char[] fenge = { ';' };
        string[] npcTaskList = npcTask.Split(fenge);

        //比较是否已完成该任务ID
         
        //newTaskList[0] = "aaaa";
        string newTaskID="";
        //string newTaskID2 = "";
        //int newTaskListNum = 0;
        for (int i=1; i <= NpcTaskID.Length; i++) {

            for (int n = 1; n <= npcTaskList.Length; n++) {

                if (NpcTaskID[i - 1] == npcTaskList[n - 1])
                {
                    NpcTaskID[i - 1] = "0";

                    //n = npcTaskList.Length;

                }
                else
                {   

                }
            }

            //如果是末尾不加“，”号，要不会多生成一个数组成员

            if (NpcTaskID[i - 1] != "0") {

                if (i == NpcTaskID.Length)
                {
                    newTaskID = newTaskID + NpcTaskID[i - 1];
                    //newTaskListNum = newTaskListNum + 1;
                }
                else
                {
                    newTaskID = newTaskID + NpcTaskID[i - 1] + ";";
                    //newTaskListNum = newTaskListNum + 1;  

                }
            
            }

        }

        //for(int i = 1 ;NpcTaskID )

        

        //将字符串转换成数组
        char[] fenge1 = { ';' };
        string[] newTaskList = newTaskID.Split(fenge);

        //*****************************************************************检测角色身上的任务ID*****************************************************************

        //检测当前绑定角色身上的任务
        //读取数据脚本
        //db_ReadDate = new DB_ReadDate();

        for (int i = 1; i <= game_PositionVar.Rose_TaskNum; i++) {

            //string date = db_ReadDate.ReadDate(game_PositionVar.Sql_Rose_Task, game_PositionVar.SqlKey_TaskID + i.ToString(), "RoseID", game_PositionVar.Rose_ID);
            string date = iniScript.InIRead("Rose_Task", "TaskID" + i.ToString(), Game_PublicClassVar.Get_game_PositionVar.Ini_Path + "RoseTask_" + Game_PublicClassVar.Get_game_PositionVar.Rose_ID + ".ini");

            for (int n = 1; n<=newTaskList.Length; n++) {

                if (date == newTaskList[n - 1]) {

                    newTaskList[n - 1] = "0";

                }
            
            }
        
        }

        string newTaskID2="";
        //将数组内为0的值进行删除
        for (int i = 1; i <= newTaskList.Length; i++) {

            if (newTaskList[i - 1] == "0")
            {

            }
            else { 
                    
                    if(i==newTaskList.Length){

                        newTaskID2 = newTaskID2 + newTaskList[i-1];

                    }else{
                    
                        newTaskID2 = newTaskID2+newTaskList[i-1]+";";
                    }

            }


        }

        //将字符串转换成数组
        
        char[] fenge2 = { ';' };
        string[] newTaskList2 = newTaskID2.Split(fenge);

        
        
        return newTaskList2;
        */


        //后期注意删除
        char[] fenge = { ';' };
        string[] newTaskList2 = NpcID.Split(fenge);;
        return newTaskList2;

    }

    //根据数字类型返回文字的任务类型
    //typeValue 为任务的数字类型
    public string TaskTypeToString(string typeValue) {
        /*
        string value="";

        switch (typeValue)
        {

            case "1":
                //case "1":

               value= "主线";

            break;

            case "2":

                value= "支线";

            break;

            case "3":

                value= "每日";

            break;
        }

        return value;
        */

        return ""; //后期删除
    }

    //根据传入的怪物ID判定增加当前任务击杀数量
    
    public bool TaskMonsterNum( string MonsterID,int MonsterNum ) {
		
        //获取自身携带的任务ID
        string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        string[] taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
        string taskValue = "";
        //循环遍历当前任务ID的要求击杀的目标是否是当前怪物
        for (int i = 0; i <= taskIDList.Length - 1; i++)
        {
			if(taskIDList[i]!=""){
				string taskValueSon = "";
				//获取当前任务要求的目标
				string TaskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", taskIDList[i], "Task_Template");
				string TaskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", taskIDList[i], "Task_Template");
				string TaskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", taskIDList[i], "Task_Template");
				string[] TaskValue = taskValueList[i].Split(',');
                string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", taskIDList[i], "Task_Template");
                //任务目标为任意怪物,强迫目标等于Monster,此处仅为第一个任务目标为任意怪物,其余2和3暂时不考虑
                if (targetType == "5") {
                    TaskTarget1 = MonsterID;
                }

                //任务目标为任意BOSS
                if (targetType == "6")
                {   
                    //获取目标是否为BOSS
                    string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", MonsterID, "Monster_Template");
                    if (monsterType == "3") {
                        TaskTarget1 = MonsterID;
                    }
                }

				if (TaskTarget1 == MonsterID)
				{
					//获取任务目标完成值
					string TaskTargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", taskIDList[i], "Task_Template");
					//获取任务完成值
					string TaskValue1 = TaskValue[0];
					//判定完成值是否大于目标值
					if (int.Parse(TaskTargetValue1) > int.Parse(TaskValue1))
					{
						//写入数据
						taskValueSon = (int.Parse(TaskValue1) + MonsterNum).ToString();
					}
					else
					{
						taskValueSon = TaskValue1;
					}
					
					//更新主界面任务显示数据
                    Debug.Log("更新怪物数据");
					Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
				}
				else {
					taskValueSon = TaskValue[0];
				}
				
				if (TaskTarget2 == MonsterID)
				{
					//获取任务目标完成值
					string TaskTargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue2", "ID", taskIDList[i], "Task_Template");
					//获取任务完成值
					string TaskValue1 = TaskValue[1];
					//判定完成值是否大于目标值
					if (int.Parse(TaskTargetValue1) > int.Parse(TaskValue1))
					{
						//写入数据
						taskValueSon = taskValueSon + "," + (int.Parse(TaskValue1) + MonsterNum).ToString();
					}
					else
					{
						taskValueSon = taskValueSon + "," + TaskValue1;
					}
					
					//更新主界面任务显示数据
					Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
				}
				else {
					taskValueSon = taskValueSon + "," + TaskValue[1];
				}
				if (TaskTarget3 == MonsterID)
				{
					//获取任务目标完成值
					string TaskTargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue3", "ID", taskIDList[i], "Task_Template");
					//获取任务完成值
					string TaskValue1 = TaskValue[2];
					//判定完成值是否大于目标值
					if (int.Parse(TaskTargetValue1) > int.Parse(TaskValue1))
					{
						//写入数据
						taskValueSon = taskValueSon + "," + (int.Parse(TaskValue1) + MonsterNum).ToString();
					}
					else
					{
						taskValueSon = taskValueSon + "," + TaskValue1 + ";";
					}
					
					//更新主界面任务显示数据
					Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
				}
				else {
					taskValueSon = taskValueSon + "," + TaskValue[2] + ";";
				}
				taskValue = taskValue + taskValueSon;
			}
        }

        //末尾去掉;符号
        if (taskValue != "") {
            taskValue = taskValue.Substring(0, taskValue.Length - 1);
        }

        //写入对应的任务目标值
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", taskValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //更新Npc头部任务状态显示
        Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true;
        return true;
    
    }

    //任务装备(目前只做了第一个任务要求,如果要做其余2个，需要把每个要求判定重新写成方法来调用)
    public bool TaskEquipNum(string spaceID, int EquipNum,string TaskID) {

        //检测道具是否符合任务要求
        bool ifSuccess = IfTaskItemNum(spaceID, EquipNum, TaskID);


        //判定是否成功
        if (!ifSuccess)
        {
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("放入装备不符合要求");
            return false;
        }
        else {

            string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
            //string TaskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", TaskID, "Task_Template");
            string TargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", TaskID, "Task_Template");
            //string TaskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", TaskID, "Task_Template");
            //string TaskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", TaskID, "Task_Template");

            if (Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(itemID,int.Parse(TargetValue1),spaceID,true))
            {

                //写入任务ID值
                CompleteTaskWriteValue(TaskID, "1,0,0");

                //更新Npc头部任务状态显示
                Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true;
                Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
                Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus = "1";        //更新Npc携带的任务
                //更新主界面显示

                return true;
            }
            return true;
        }
    }


    //任务装备(目前只做了第一个任务要求,如果要做其余2个，需要把每个要求判定重新写成方法来调用)
    public bool IfTaskItemNum(string spaceID, int EquipNum, string TaskID)
    {

        string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
        string itemLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", itemID, "Item_Template");
        string TaskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", TaskID, "Task_Template");
        string TargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", TaskID, "Task_Template");
        //string TaskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", TaskID, "Task_Template");
        //string TaskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", TaskID, "Task_Template");
        string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", TaskID, "Task_Template");
        bool ifSuccess = false;
        switch (targetType)
        {
            //指定属性
            case "11":
                string EquipNeedProType = TaskTarget1.Split(',')[0];
                int EquipNeedValue = int.Parse(TaskTarget1.Split(',')[1]);
                string nowEquipValue = "";
                switch (EquipNeedProType)
                {
                    //攻击
                    case "1":
                        Debug.Log("调用攻击11111111111");
                        nowEquipValue = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideTypeValue(itemID, "1", "2", spaceID);
                        Debug.Log("nowEquipValue = " + nowEquipValue);
                        break;
                    //物防
                    case "2":
                        nowEquipValue = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideTypeValue(itemID, "1", "3", spaceID);
                        break;
                    //魔防
                    case "3":
                        nowEquipValue = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideTypeValue(itemID, "1", "4", spaceID);
                        break;
                    //等级
                    case "4":
                        nowEquipValue = itemLv;
                        break;
                }
                Debug.Log("nowEquipValue = " + nowEquipValue + ";EquipNeedValue = " + EquipNeedValue);
                //判定是否满足
                if (int.Parse(nowEquipValue) >= EquipNeedValue)
                {
                    ifSuccess = true;
                }
                break;

            //指定品质
            case "12":
                EquipNeedProType = TaskTarget1.Split(',')[0];
                EquipNeedValue = int.Parse(TaskTarget1.Split(',')[1]);
                string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", itemID, "Item_Template");
                switch (EquipNeedProType)
                {
                    //白色
                    case "1":
                        if (ItemQuality == "1")
                        {
                            ifSuccess = true;
                        }
                        break;
                    //绿色
                    case "2":
                        if (ItemQuality == "2")
                        {
                            ifSuccess = true;
                        }
                        break;
                    //蓝色
                    case "3":
                        if (ItemQuality == "3")
                        {
                            ifSuccess = true;
                        }
                        break;
                    //紫色
                    case "4":
                        if (ItemQuality == "4")
                        {
                            ifSuccess = true;
                        }
                        break;
                    //橙色
                    case "5":
                        if (ItemQuality == "5")
                        {
                            ifSuccess = true;
                        }
                        break;
                }

                //判定等级是否符合
                if (ifSuccess)
                {
                    //如果等级不符合则判定为false
                    if (int.Parse(itemLv) < EquipNeedValue)
                    {
                        ifSuccess = false;
                    }
                }
                break;

            //指定部位
            case "13":

                EquipNeedProType = TaskTarget1.Split(',')[0];
                EquipNeedValue = int.Parse(TaskTarget1.Split(',')[1]);

                //获取目标装备部位
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
                string itemSubType = "-1";
                if (itemType == "3")
                {
                    itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
                }
                //判定部位是否符合要求
                if (itemSubType == EquipNeedProType.ToString())
                {
                    //如果等级不符合则判定为false
                    if (int.Parse(itemLv) >= EquipNeedValue)
                    {
                        ifSuccess = true;
                    }
                }
                break;
        }

        //判定是否成功
        if (!ifSuccess)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_212");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("放入装备不符合要求");
            return false;
        }
        else
        {
            return true;
        }
    }


    //直接写入任务完成值
    public void CompleteTaskWriteValue(string writeTaskID,string writeTaskValue) {

        //获取自身携带的任务ID
        string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        string[] taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
        string taskValue = "";

		//循环遍历当前任务ID
        for (int i = 0; i <= taskIDList.Length - 1; i++)
        {
            if (taskIDList[i] != "" && taskIDList[i] != "0")
            {
                if (taskIDList[i] == writeTaskID)
                {
                    taskValue = taskValue + writeTaskValue + ";";
                }
                else
                {
                    taskValue = taskValue + taskValueList[i] + ";";
                }
            }
        }

        //末尾去掉;符号
        if (taskValue != "")
        {
            taskValue = taskValue.Substring(0, taskValue.Length - 1);
        }

        //写入对应的任务目标值
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", taskValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
    }

	//更新当前任务目标完成值
	public bool updataTaskItemID(){

		//获取自身携带的任务ID
		string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
		string[] taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
		string taskValue = "";

		//循环遍历当前任务ID的要求击杀的目标是否是当前怪物
		for (int i = 0; i <= taskIDList.Length - 1; i++)
		{
            if (taskIDList[i] != "" && taskIDList[i] != "0")
            {
				string taskValueSon = "";
				//获取当前任务要求的目标
				string TaskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", taskIDList[i], "Task_Template");
				string TaskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", taskIDList[i], "Task_Template");
				string TaskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", taskIDList[i], "Task_Template");
				
				string[] TaskValue = taskValueList[i].Split(',');
				
				//获取任务类型是否为寻找道具
				string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", taskIDList[i], "Task_Template");
                switch (targetType)
                {

                    case "2":
                        if (targetType == "2")
                        {
                            int bagItemNum = 0;
                            if (TaskTarget1 != "0")
                            {
                                //获取目标ID
                                bagItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(TaskTarget1);
                                //int bagItemNum = 0;
                                if (bagItemNum != 0)
                                {
                                    taskValueSon = bagItemNum.ToString();
                                    //更新主界面任务显示数据
                                    Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
                                }
                                else
                                {
                                    taskValueSon = TaskValue[0];
                                }
                            }
                            else
                            {
                                taskValueSon = TaskValue[0];
                            }


                            if (TaskTarget2 != "0")
                            {
                                bagItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(TaskTarget2);
                                if (bagItemNum != 0)
                                {
                                    taskValueSon = taskValueSon + "," + bagItemNum.ToString();
                                    //更新主界面任务显示数据
                                    Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
                                }
                                else
                                {
                                    taskValueSon = taskValueSon + "," + TaskValue[1];
                                }
                            }
                            else
                            {
                                taskValueSon = taskValueSon + "," + TaskValue[1];
                            }

                            if (TaskTarget3 != "0")
                            {
                                bagItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(TaskTarget3);
                                if (bagItemNum != 0)
                                {

                                    taskValueSon = taskValueSon + "," + bagItemNum.ToString() + ";";
                                    //更新主界面任务显示数据
                                    Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
                                }
                                else
                                {
                                    taskValueSon = taskValueSon + "," + TaskValue[2] + ";";
                                }
                            }
                            else
                            {
                                taskValueSon = taskValueSon + "," + TaskValue[2] + ";";
                            }

                            taskValue = taskValue + taskValueSon;

                        }
                        break;

                    case "4":

                        //等级类任务
                        if (targetType == "4")
                        {
                            //获取当前等级
                            string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            taskValue = taskValue + roseLv + ",0,0;";
                        }

                        break;

                    case "7":
                        //宠物类任务(目前只支持第一个任务)
                        if (targetType == "7")
                        {
                            //string addTaskValueStr = "";
                            bool ifAddPetStatus = false;
                            int targetPetNum = 0;
                            for (int y = 1; y <= 12; y++)
                            {
                                string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", y.ToString(), "RosePet");
                                if (petID == "0" || petID == "")
                                {
                                    //为0则表示没有宠物,不做任何操作   
                                }
                                else
                                {
                                    if (TaskTarget1 == petID)
                                    {
                                        //获取目标是否为野生
                                        string ifBaby = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", y.ToString(), "RosePet");
                                        string petLockStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LockStatus", "ID", y.ToString(), "RosePet");
                                        if (ifBaby == "0"&& petLockStatus != "1")
                                        {
                                            targetPetNum = targetPetNum + 1;
                                            taskValue = taskValue + targetPetNum + ",0,0;";
                                            ifAddPetStatus = true;
                                            //更新主界面任务显示数据
                                            Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
                                            Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus = "1";        //更新Npc携带的任务
                                            Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (ifAddPetStatus == false) {
                                taskValue = taskValue + taskValueList[i] + ";";
                            }
                            //taskValue = taskValue + addTaskValueStr;
                        }

                        break;

                    //其他任务要求填写默认的任务要求数量
                    default:
                        taskValue = taskValue + taskValueList[i] + ";";
                        break;
                }

                /*
                if (targetType == "1" || targetType == "3")
                {
                    taskValue = taskValue + taskValueList[i] + ";";
                }
                */
			}
		}
	
		//末尾去掉;符号
		if (taskValue != "") {
			taskValue = taskValue.Substring(0, taskValue.Length - 1);  
		}
		
		//写入对应的任务目标值
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", taskValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
		
		return true;
	}

    //获取当前角色身上的任务ID
    public List<string> GetRoseTaskID() {

        List<string> taskID_List = new List<string>();
        string taskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        char[] fenge = { ',' };
        string[] taskIDList = taskID.Split(fenge);
        for (int i = 0; i <= taskIDList.Length - 1; i++) {
            taskID_List.Add(taskIDList[i]);
        }
        return taskID_List;
    }

    //返回当前自身携带对应类型的任务
    public List<string> GetRoseTaskTypeID(string taskType)
    {
        List<string> taskID_List = new List<string>();
        string taskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        char[] fenge = {','};
        string[] taskIDList = taskID.Split(fenge);
        for (int i = 0; i <= taskIDList.Length - 1; i++)
        {
			if(taskIDList[i]!=""){
				//获取任务类型
				string targerID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", taskIDList[i], "Task_Template");
				if (targerID == taskType)
				{
					taskID_List.Add(taskIDList[i]);
				}
			}
        }
        return taskID_List;
    }

    //根据任务ID返回任务名称
    public string TaskIDtoTaskName(string taskID) {
        
        string taskName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskName", "ID", taskID,"Task_Template");
        return taskName;
    }

    //根据任务ID返回任务等级
    public string TaskIDtoTaskLv(string taskID)
    {
        string taskLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskLv", "ID", taskID, "Task_Template");
        return taskLv;
    }

    public string TaskIDtoTaskDescribe(string taskID)
    {
        /*
        string taskDescribe = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("TaskDes", "ID", taskID, Game_PublicClassVar.Get_game_PositionVar.Xml_Path + "Task_Template.xml");

        return taskDescribe;
        */
        return ""; //后期删除
    }

    //传入任务ID获取当前进度值  (参数1：传入的任务ID,  参数2：需要查询当前任务的第几个任务目标)
    public string TaskReturnValue(String taskID, String taskTargetValue) {
        string taskProValue = "";
        //检测自身是否携带此任务
        bool findStatus = false;
        List<string> roseTaskList = GetRoseTaskID();
        foreach(string roseTaskID in roseTaskList){
            if (roseTaskID == taskID) {
                findStatus = true;
            }
        }
        //开始查询
        if (findStatus)
        { 
            //获取任务ID在第几个位置
            string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
            //获取任务当前完成值
            string[] taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
            for (int i = 0; i <= taskIDList.Length - 1; i++)
            {
                //符合条件
                if (taskIDList[i] == taskID && taskValueList.Length>=i+1)
                {
                    string taskValue = taskValueList[i];
                    if (taskValue.Split(',').Length >= 3) {
                        //当前进度赋值
                        switch (taskTargetValue)
                        {
                            case "1":
                                taskProValue = taskValue.Split(',')[0];
                                break;

                            case "2":
                                taskProValue = taskValue.Split(',')[1];
                                break;

                            case "3":
                                taskProValue = taskValue.Split(',')[2];
                                break;
                        }
                    }
                }
            }
        }
        return taskProValue;
    }

    //传入任务ID返回任务状态 (参数1:任务ID)  返回值 1：未接取 2：已完成 3：已接取,未完成 4：已接取,已完成 5:未达到接取条件
    public string TaskReturnStatus(string taskID) {

        //每日任务直接发放,此处不处理
        /*
        string taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", taskID, "Task_Template");
        if (taskType == "3")
        {
            return "5";
        }
        */

        //获取自身已完成任务
        string[] roseCompleteTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');

        for (int i = 0; i <= roseCompleteTaskID.Length - 1; i++) {
            if (roseCompleteTaskID[i] == taskID) {
                return "2";
            }
        }

        //获取自身接取任务
        string[] roseGetTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        for (int i = 0; i <= roseGetTaskID.Length - 1; i++)
        {
            if (roseGetTaskID[i] == taskID)
            {

                //检测任务是否完成
                if (TaskComplete(taskID))
                {
                    return "4";
                }
                else {
                    return "3";
                }
            }
        }

        //判定是否满足接取任务条件
        string triggerTaskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerType", "ID", taskID, "Task_Template");
        string triggerTaskValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerValue", "ID", taskID, "Task_Template");
        string taskLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskLv", "ID", taskID, "Task_Template");
        switch (triggerTaskType) { 
            //等级触发，检测自身等级是否达到
            case "1":
                string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (int.Parse(roseLv) >= int.Parse(taskLv))
                {
                    return "1";
                }
            break;
            //任务触发，检测完成的任务ID是否包含前置任务
            case "2":
                for (int i = 0; i <= roseCompleteTaskID.Length - 1; i++)
                {
                    if (roseCompleteTaskID[i] == triggerTaskValue)
                    {
                        return "1";
                    }
                }
            break;
            //章节触发
            case "3":
                string pveChapter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PVEChapter", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string nowzhangJie = pveChapter.Substring(0, 1);
                if (int.Parse(nowzhangJie) >= int.Parse(triggerTaskValue))
                {
                    return "1";
                }
                break;

        }
        return "5";

    }

    //传入任务ID,写入完成的任务
    public bool TaskWriteRoseData(string taskID) {

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().obj_NpcTarget == null) {
            Debug.Log("玩家当前没有选中任务NPC");
            return false;
        }

        //写入完成任务
        string roseCompleteTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string writeTaskID = "";
        if (roseCompleteTaskID != "")
        {
            writeTaskID = roseCompleteTaskID + "," + taskID;
        }
        else {
            writeTaskID = taskID;
        }
        //查询自身是否携带此任务,如携带此任务将清空其数据
        //获取任务ID在第几个位置
        string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        //获取任务当前完成值
        string[] taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
        string getTaskIDList = "";
        string getTaskValueList = "";
        for (int i = 0; i <= taskIDList.Length - 1; i++) {
            //完成的任务ID不是当前携带的任务,进行数据记录，如果是自身携带记录,则不进行记录
            if (taskIDList[i] != taskID) {

                if (getTaskIDList == "")
                {
                    getTaskIDList = taskIDList[i];
                    getTaskValueList = taskValueList[i];
                }
                else
                {
                    getTaskIDList = getTaskIDList + "," + taskIDList[i];
                    getTaskValueList = getTaskValueList + ";" + taskValueList[i];
                }
			}else{
				//触发完成任务，扣除任务消耗

                //获取任务要求道具数量
                string TaskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", taskIDList[i], "Task_Template");
                string TaskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", taskIDList[i], "Task_Template");
                string TaskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", taskIDList[i], "Task_Template");

                string TargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", taskIDList[i], "Task_Template");
                string TargetValue2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue2", "ID", taskIDList[i], "Task_Template");
                string TargetValue3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue3", "ID", taskIDList[i], "Task_Template");

				//获取任务类型是否为寻找道具
				string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", taskIDList[i], "Task_Template");
                switch (targetType) { 
                    
                    case "2":
                        //是否为消耗道具任务
                        if (targetType == "2")
                        {
                            if (TaskTarget1 != "0")
                            {
                                bool costItem = Game_PublicClassVar.Get_function_Rose.CostBagItem(TaskTarget1, int.Parse(TargetValue1));
                                if (!costItem)
                                {
                                    //Debug.Log("背包内任务道具不足");
                                }
                            }
                            if (TaskTarget2 != "0")
                            {
                                bool costItem = Game_PublicClassVar.Get_function_Rose.CostBagItem(TaskTarget2, int.Parse(TargetValue2));
                                if (!costItem)
                                {
                                    //Debug.Log("背包内任务道具不足");
                                }
                            }
                            if (TaskTarget3 != "0")
                            {
                                bool costItem = Game_PublicClassVar.Get_function_Rose.CostBagItem(TaskTarget3, int.Parse(TargetValue3));
                                if (!costItem)
                                {
                                    //Debug.Log("背包内任务道具不足");
                                }
                            }
                        }
                        break;

                    case "7":

                        //获取任务要求宠物数量(暂时第一个位置好用,后续需要再添加)
                        if (targetType == "7")
                        {
                            int targetPetNum = int.Parse(TargetValue1);
                            for (int y = 1; y <= 12; y++)
                            {
                                if(targetPetNum<=0){
                                    break;
                                }
                                string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", y.ToString(), "RosePet");
                                if (petID == "0" || petID == "")
                                {
                                    //为0则表示没有宠物,不做任何操作   
                                }
                                else
                                {
                                    if (TaskTarget1 == petID)
                                    {
                                        //获取目标是否为野生
                                        string ifBaby = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", y.ToString(), "RosePet");
                                        if (ifBaby == "0")
                                        {

                                            //不能删除当前出战宠物
                                            string petStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", y.ToString(), "RosePet");
                                            if (petStatus != "1") {
                                                //获取目标是否改过名称
                                                string petName_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", y.ToString(), "RosePet");
                                                string petName_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", petID, "Pet_Template");
                                                if (petName_1 == petName_2)
                                                {
                                                    //清除宠物
                                                    targetPetNum = targetPetNum - 1;
                                                    Game_PublicClassVar.Get_function_AI.Pet_ClearnData(y.ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                }
			}
        }

        //更新玩家剧情状态
        string TriggerStoryID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerStoryID", "ID", taskID, "Task_Template");
        //Debug.Log("TriggerStoryID = " + TriggerStoryID);
        if(TriggerStoryID!="0"){
            //获取玩家当前剧情状态值
            string roseStoryStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Debug.Log("TriggerStoryID = " + TriggerStoryID);
            if (roseStoryStatus == TriggerStoryID)
            {
                Game_PublicClassVar.Get_function_Rose.UpdataRoseStoryStatus();
                //Debug.Log("玩家完成指定任务,更新了剧情值");

            }
            else {
                //如果主线任务不一致则完成任务强制故事ID同步
                string nextStoryID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextStoryID", "ID", TriggerStoryID, "GameStory_Template");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("StoryStatus", nextStoryID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
            }
        }

        //
        //获取当前选中的Npc是否有剧情对话
        bool isStory = false;
        string storyIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakID", "ID", Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().obj_NpcTarget.GetComponent<AI_NPC>().NpcID, "Npc_Template");
        if (storyIDStr != "0")
        {
            string[] storyID = (storyIDStr).Split(';');
            for (int i = 0; i <= storyID.Length - 1; i++)
            {
                if (storyID[i] == Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"))
                {
                    isStory = true;
                }
            }
        }

        if (isStory)
        {
            //实例化一个剧情UI
            GameObject obj_StorySpeakSet = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_StorySpeakSet);
            obj_StorySpeakSet.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_StorySpeakSet.transform);
            obj_StorySpeakSet.transform.localPosition = Vector3.zero;
            obj_StorySpeakSet.transform.localScale = new Vector3(1, 1, 1);
            obj_StorySpeakSet.GetComponent<UI_StorySpeakSet>().GameStoryID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            GameObject obj_NpcTarget = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().obj_NpcTarget;
            if (obj_NpcTarget.GetComponent<AI_NPC>().NpcID != "" && obj_NpcTarget.GetComponent<AI_NPC>().NpcID != "0")
            {
                obj_StorySpeakSet.GetComponent<UI_StorySpeakSet>().NpcID = obj_NpcTarget.GetComponent<AI_NPC>().NpcID;
            }
            //隐藏主界面
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcTaskSet.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.SetActive(false);
        }
        
        string taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", taskID, "Task_Template");
        if(taskType == "2")
        {
            string taskSonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskSonType", "ID", taskID, "Task_Template");
            if (taskSonType == "1") {
                string shiLianTaskNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                int shiLianTaskNum = 0;

                if (shiLianTaskNumStr == "" || shiLianTaskNumStr == null) {
                    shiLianTaskNumStr = "0";
                }

                if (shiLianTaskNumStr != "")
                {

                    shiLianTaskNum = int.Parse(shiLianTaskNumStr) + 1;
                    Debug.Log("shiLianTaskNum = " + shiLianTaskNum);
                    //发送试炼指定环奖励
                    if (shiLianTaskNum == 20 || shiLianTaskNum == 40 || shiLianTaskNum == 60 || shiLianTaskNum == 80)
                    {
                        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() >= 1)
                        {
                            if (UnityEngine.Random.value <= 0.2f)
                            {
                                //20%概率发送装备
                                int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                                if (roseLv >= 1 && roseLv <= 18) {
                                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10015001", 1,"0",0,"0",true,"22");       
                                }
                                if (roseLv >= 19 && roseLv <= 29)
                                {
                                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10015002", 1, "0", 0, "0", true, "22");   
                                }
                                if (roseLv >= 30 && roseLv <= 39)
                                {
                                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10015003", 1, "0", 0, "0", true, "22");   
                                }
                                if (roseLv >= 40 && roseLv <= 49)
                                {
                                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10015004", 1, "0", 0, "0", true, "22");    
                                }
                                if (roseLv >= 50)
                                {
                                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10015005", 1, "0", 0, "0", true, "22");     
                                }
                            }
                            else {
                                Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000023", 1, "0", 0, "0", true, "22");        //试炼任务特殊处理    
                            }
                        }
                        else {
                            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_257");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("您的背包已满,本次试炼奖励领取失败!");
                        }
                    }

                    //发送试炼指定环奖励
                    if (shiLianTaskNum == 100)
                    {
                        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() >= 1)
                        {
                            Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010029", 1, "0", 0, "0", true, "22");        //试炼任务特殊处理    
                        }
                        else
                        {
                            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_257");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("您的背包已满,本次试炼奖励领取失败!");
                        }
                    }

                    //shiLianTaskNum = shiLianTaskNum + 1;
                    //试炼最大100环,当超过100环的时候,自动重置为1环
                    if (shiLianTaskNum >= 101)
                    {
                        shiLianTaskNum = 1;
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianTaskNum", shiLianTaskNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianTaskStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    writeTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    //清空下次要领取的任务
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianNextTaskID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                }
            }
        }

        //每日任务不存储任务完成数据
        if (taskType == "3")
        {
            string shiMenTaskNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMenTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            int shiMenTaskNum = 0;


            if (shiMenTaskNumStr == "" || shiMenTaskNumStr==null)
            {
                shiMenTaskNumStr = "0";
            }

            if (shiMenTaskNumStr != "")
            {
                shiMenTaskNum = int.Parse(shiMenTaskNumStr);
                shiMenTaskNum = shiMenTaskNum + 1;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiMenTaskNum", shiMenTaskNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                writeTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                //清空下次要领取的任务
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiMenNextTaskID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            }
        }

        //写入完成任务
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CompleteTaskID", writeTaskID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskID", getTaskIDList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", getTaskValueList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

		//更新Npc头部任务状态显示
		Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true;

        //更新隐藏NPC显示
        Game_PublicClassVar.Get_game_PositionVar.NpcShowValueStatus = true;

		//删除主界面快捷显示的任务
		DeleteMainUITaskID (taskID);

        Game_PublicClassVar.Get_function_UI.PlaySource("10011", "1");

        //激活觉醒
        if (taskID == "33000018") {
            jueXingJianCe();
        }
        

        return true;

    }


    //返回任务需求宠物数量
    public int ReturnTaskNeedPetNum(string TaskTarget1) {

        int returnNum = 0;
        for (int y = 1; y <= 12; y++)
        {
            string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", y.ToString(), "RosePet");
            if (petID == "0" || petID == "")
            {
                //为0则表示没有宠物,不做任何操作   
            }
            else
            {
                if (TaskTarget1 == petID)
                {
                    //获取目标是否为野生
                    string ifBaby = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", y.ToString(), "RosePet");
                    if (ifBaby == "0")
                    {

                        //不能删除当前出战宠物
                        string petStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", y.ToString(), "RosePet");
                        if (petStatus != "1")
                        {
                            //获取目标是否改过名称
                            string petName_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", y.ToString(), "RosePet");
                            string petName_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", petID, "Pet_Template");
                            if (petName_1 == petName_2)
                            {
                                //检测宝宝上是否附带装备
                                if (Game_PublicClassVar.Get_function_AI.PetIfWearEquip(y.ToString()) == false)
                                {
                                    returnNum = returnNum + 1;
                                }
                                else {
                                    Game_PublicClassVar.Get_function_UI.GameHint("提交失败,宠物上有装备!");
                                }
                            }
                        }
                    }
                }
            }
        }

        return returnNum;

    }

	//获取当前主界面跟踪任务
	public string[] ReturnMainUITask(){

		string[] mainUITask = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainUITaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig").Split(',');
        //检测当前任务自身是否拥有
        string taskIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] roseTaskIDList = taskIDStr.Split(',');
        string saveTaskValue = "";
        bool saveStatus = false;
        for (int i = 0; i < mainUITask.Length; i++) {
            bool ifsave = true;
            for (int y = 0; y < roseTaskIDList.Length; y++) {
                if (mainUITask[i] == roseTaskIDList[y]) {
                    saveTaskValue = saveTaskValue + mainUITask[i] + ",";
                    ifsave = false;
                }
            }
            if (ifsave) {
                saveStatus = true;
            }
        }

        if (saveTaskValue != "") {
            saveTaskValue = saveTaskValue.Substring(0, saveTaskValue.Length - 1);
        }

        if (saveStatus)
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainUITaskID", saveTaskValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            //检查完后重新获取一遍
            mainUITask = saveTaskValue.Split(',');
        }

        return mainUITask;
	}

	//获取当前任务有几个目标
	public int ReturnTaskTargetNum(string taskID){

        //如果是找人任务 返回1
        string taskTargetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", taskID, "Task_Template");
        if (taskTargetType == "3")
        {
            return 1;
        }

		int targetNum = 0;
		string target_1 =  Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", taskID, "Task_Template");
		string target_2 =  Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", taskID, "Task_Template");
		string target_3 =  Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", taskID, "Task_Template");
		if (target_1 != "0") {
			targetNum = targetNum +1;
		}

		if (target_2 != "0") {
			targetNum = targetNum +1;
		}

		if (target_3 != "0") {
			targetNum = targetNum +1;
		}

		return targetNum;
	}

	//写入追踪任务
	public bool WriteMainUITaskID(string taskID){

		if (ReturnMainUITask ().Length < 3) {
			string mainUITask = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainUITaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
			if(mainUITask!=""){
				mainUITask = mainUITask + "," + taskID;
			}else{
				mainUITask = taskID;
			}
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainUITaskID", mainUITask, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
			Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            //检查任务值
            MainTaskID_Check();

            //更新主界面任务显示
            Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
			return true;
		} else {
			return false;
		}
	}

	//删除追踪任务
	public bool DeleteMainUITaskID(string taskID){

		string mainTask = "";
		string[] mainUITaskID = ReturnMainUITask();
		for (int i = 0; i<=mainUITaskID.Length-1; i++) {
			if(mainUITaskID[i]!=taskID){
				mainTask = mainTask + mainUITaskID[i]+",";
			}
		}
		if (mainTask != "") {
			mainTask = mainTask.Substring(0,mainTask.Length-1);
		}

		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainUITaskID", mainTask, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

        //检查任务值
        MainTaskID_Check();

        //更新主界面任务显示
        Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;

		return true;
	}

    //检测主线显示任务是否正常
    private void MainTaskID_Check() {
        string mainTask = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainUITaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (mainTask != "") {
            string[] mainTaskList = mainTask.Split(',');
            string writeTaskStr = "";
            for (int i = 0; i < mainTaskList.Length; i++) {
                //传入任务ID返回任务状态(参数1: 任务ID)  返回值 1：未接取 2：已完成 3：已接取,未完成 4：已接取,已完成 5:未达到接取条件
                string taskStatus = TaskReturnStatus(mainTaskList[i]);
                if (taskStatus == "3" || taskStatus == "4") {
                    writeTaskStr = writeTaskStr + mainTaskList[i] + ",";
                }
            }

            //写入值
            if (writeTaskStr != "")
            {
                writeTaskStr = writeTaskStr.Substring(0, writeTaskStr.Length - 1);
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainUITaskID", writeTaskStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

        }
    }

	//传入任务ID判定是否为追踪任务
	public bool IfMainUITask(string taskID){
		string[] mainUITaskID = ReturnMainUITask();
		for (int i = 0; i<=mainUITaskID.Length-1; i++) {
			if(mainUITaskID[i]==taskID){
				return true;
			}
		}
		return false;
	}

	//取消自身携带的任务
	public bool DeleteRoseTaskID(string taskID){
		//写入完成任务
		string roseCompleteTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		//查询自身是否携带此任务,如携带此任务将清空其数据
		//获取任务ID在第几个位置
		string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
		//获取任务当前完成值
		string[] taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
		string getTaskIDList = "";
		string getTaskValueList = "";
		for (int i = 0; i <= taskIDList.Length - 1; i++) {
			//完成的任务ID不是当前携带的任务,进行数据记录，如果是自身携带记录,则不进行记录
			if (taskIDList[i] != taskID) {
				
				if (getTaskIDList == "")
				{
					getTaskIDList = taskIDList[i];
					getTaskValueList = taskValueList[i];
				}
				else
				{
					getTaskIDList = getTaskIDList + "," + taskIDList[i];
					getTaskValueList = getTaskValueList + ";" + taskValueList[i];
				}
			}
		}
		
		//写入完成任务
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskID", getTaskIDList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", getTaskValueList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //试炼任务需要写入状态
        string taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", taskID, "Task_Template");
        if (taskType == "2")
        {
            string taskSonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskSonType", "ID", taskID, "Task_Template");
            if (taskSonType == "1")
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianTaskStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                //试炼任务只要放弃就需要从第1环开始做
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianTaskNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianNextTaskID", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");      //任务重置
            }
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

		//删除当前追踪任务ID
		DeleteMainUITaskID(taskID);

		//更新主界面任务显示
		Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;

		//刷新任务栏
		Game_PublicClassVar.Get_game_PositionVar.Rose_TaskDataUpdata = true;

		//清空当前选中任务
		if (Game_PublicClassVar.Get_game_PositionVar.NowTaskID == taskID) {
			Game_PublicClassVar.Get_game_PositionVar.NowTaskID = "";
			//再次获取自身的任务
			taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
			if(taskIDList[0]!=""){
				Game_PublicClassVar.Get_game_PositionVar.NowTaskID = taskIDList[0];
			}
		}

		//更新主界面任务列表
		UI_FunctionOpen ui_FunctionOpen = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>();
		ui_FunctionOpen.Obj_roseTask.GetComponent<Rose_TaskList>().Rose_TaskList_Update = true;
		//更新Npc头部任务状态显示
		Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true;

		return true;

	}


    //点击任务移动按钮
    public void Btn_TaskMove(string TaskID)
    {
        //string taskName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskName", "ID", TaskID, "Task_Template");
        //Debug.Log("我点击了任务：" + taskName);

        //获取任务是否完成,如果完成直接奔向交任务的NPC处
        string incompleteValue = Game_PublicClassVar.Get_function_Task.TaskIncompleteReturnValue(TaskID);
        switch (incompleteValue)
        {

            case "-1":
                //Debug.Log(TaskID + "数据错误");
                break;
            //任务全部完成
            case "0":
                //Debug.Log("任务完成");
                string movePosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1Position", "ID", TaskID, "Task_Template");
                //获取场景并对比是否为当前场景
                taskMovePosition("CompleteTaskPosition", TaskID);
                break;
            //任务条件1未完成
            case "1":
                //Debug.Log("任务未完成：1");
                taskMovePosition("Target1Position", TaskID);
                break;
            //任务条件2未完成
            case "2":
                //Debug.Log("任务未完成：2");
                taskMovePosition("Target2Position", TaskID);
                break;
            //任务条件3未完成
            case "3":
                //Debug.Log("任务未完成：3");
                taskMovePosition("Target3Position", TaskID);
                break;

        }
    }

    public void taskMovePosition(string moveName, string TaskID)
    {
        //获取场景并对比是否为当前场景
        string taskMoveID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(moveName, "ID", TaskID, "Task_Template");
        if (taskMoveID == "0")
        {

            string taskTargetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", TaskID, "Task_Template");
            if (taskTargetType == "3") {
                
                string Target1str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteNpcID", "ID", TaskID, "Task_Template");
                if (Target1str != "" && Target1str != null && Target1str != "0") {
                    string npcHintStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskHint", "ID", Target1str, "Npc_Template");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请前往地图:" + npcHintStr + "寻找此人");
                    return;
                }
            }

            //Game_PublicClassVar.Get_function_UI.GameGirdHint("此任务不可以自动寻路！");
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_330");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            return;
        }
        string mapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", taskMoveID, "TaskMovePosition_Template");
        if (Application.loadedLevelName == mapName)
        {
            moveMap(taskMoveID);
            /*
            //获取移动到的目标点
            string positionName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PositionName", "ID", taskMoveID, "TaskMovePosition_Template");
            GameObject scenceRosePosition = GameObject.FindWithTag("ScenceRosePosition");
            GameObject obj_Position = scenceRosePosition.transform.Find(positionName).gameObject;
            Vector3 movePositionVec3 = new Vector3();
            if (obj_Position != null)
            {
                //如果目标点在场景中存在则直接获取此坐标点
                movePositionVec3 = obj_Position.transform.position;
            }
            else {
                //读取XYZ的值进行赋值
                float p_x = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_X", "ID", taskMoveID, "TaskMovePosition_Template"));
                float p_y = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_Y", "ID", taskMoveID, "TaskMovePosition_Template"));
                float p_z = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_Z", "ID", taskMoveID, "TaskMovePosition_Template"));
                movePositionVec3 = new Vector3(p_x, p_y, p_z);
                //movePositionVec3 = new Vector3(-10.21f, 31f, -10.38f);
            }
            
            //移动到目标点
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().TaskRunPositionVec3 = movePositionVec3;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().TaskRunStatus = "1";
            */
        }
        else
        {
            //获取移动其他地图的移动点
            string[] OtherMapMoveList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OtherMapMove", "ID", taskMoveID, "TaskMovePosition_Template").Split(';');
            string movePositionID = "";
            bool moveStatus = false;
            for (int i = 0; i <= OtherMapMoveList.Length - 1; i++)
            {
                string[] moveMapValue = OtherMapMoveList[i].Split(',');
                if (moveMapValue[0] == Application.loadedLevelName)
                {
                    moveStatus = true;
                    movePositionID = moveMapValue[1];
                }
            }

            if (moveStatus)
            {
                if (movePositionID != "")
                {
                    moveMap(movePositionID);        //开始移动               
                }
            }
            else
            {
                string sceneName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", mapName, "Scene_Template");
				string npcID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteNpcID", "ID", TaskID, "Task_Template");
				string nameNpc = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", npcID, "Npc_Template");


                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_331");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_332");
                string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_333");
                Game_PublicClassVar.Get_function_UI.GameGirdHint(langStrHint_1 + sceneName + langStrHint_2 + nameNpc + langStrHint_3);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint("请先前往地图：" + sceneName + ",如需提交任务,请找到" + nameNpc + "可提交任务");
            }
        }
    }

    public void moveMap(string taskMoveID)
    {
        //获取移动到的目标点
        string positionName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PositionName", "ID", taskMoveID, "TaskMovePosition_Template");
        GameObject scenceRosePosition = GameObject.FindWithTag("ScenceRosePosition");
        if (scenceRosePosition.transform.Find(positionName) == null) {
            return;
        }
        GameObject obj_Position = scenceRosePosition.transform.Find(positionName).gameObject;
        Vector3 movePositionVec3 = new Vector3();
        if (obj_Position != null)
        {
            //如果目标点在场景中存在则直接获取此坐标点
            movePositionVec3 = obj_Position.transform.position;
        }
        else
        {
            //读取XYZ的值进行赋值
            float p_x = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_X", "ID", taskMoveID, "TaskMovePosition_Template"));
            float p_y = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_Y", "ID", taskMoveID, "TaskMovePosition_Template"));
            float p_z = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_Z", "ID", taskMoveID, "TaskMovePosition_Template"));
            movePositionVec3 = new Vector3(p_x, p_y, p_z);
            //movePositionVec3 = new Vector3(-10.21f, 31f, -10.38f);
        }

        //移动到目标点
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().TaskRunPositionVec3 = movePositionVec3;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().TaskRunStatus = "1";
    }

    //检测自身是否有试炼任务
    public bool GetShiLianTaskStatus() {

        string nowTaskStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (nowTaskStr != "" && nowTaskStr != "0")
        {
            string[] nowTaskList = nowTaskStr.Split(',');
            for (int i = 0; i < nowTaskList.Length; i++)
            {
                string nowTaskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", nowTaskList[i], "Task_Template");
                if (nowTaskType == "2")
                {
                    string nowTaskSonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskSonType", "ID", nowTaskList[i], "Task_Template");
                    if (nowTaskSonType == "1")
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    //写入成就（写成成就类型,写入成就值,写入成就数量）
    public void ChengJiu_WriteValue(string writeType,string writeValue,string writeNum) {

        switch (writeType)
        {
            //击杀指定数量怪物
            case "1":
                ChengJiu_WriteValue_1("ChengJiu_"+ writeType, writeValue, writeNum);
                break;

            //击杀任意数量怪物
            case "2":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //击杀领主级数量怪物
            case "3":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //击杀指定BOSS(普通难度)
            case "4":
                ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                break;

            //击杀指定BOSS(困难难度)
            case "5":
                ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                break;

            //击杀指定BOSS(地狱难度)
            case "6":
                ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                break;

            //击杀指定BOSS(极限难度)
            case "7":
                ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                break;

            //进入某个地图的次数
            case "101":
                ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                break;

            //累计发现第一章黄金宝箱数量（章节数,对应宝箱数量;章节数,对应宝箱数量;）
            case "102":
                ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                break;

            //挖掘藏宝图的次数
            case "103":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //挖掘高级藏宝图的次数
            case "104":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计获得金币的值
            case "105":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计十连抽次数
            case "106":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计装备洗炼次数
            case "107":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计天赋重置次数
            case "108":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计在线时间(分钟)
            case "109":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计复活次数
            case "110":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计游戏分享次数
            case "111":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //玩家等级
            case "201":
                ChengJiu_WriteValue_3("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //打造熟练度
            case "202":
                ChengJiu_WriteValue_3("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //炼金熟练度
            case "203":
                ChengJiu_WriteValue_3("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计收集装备套装(对应激活套装ID,预留参数(任意填即可));
            case "204":
                ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                break;

            //累计激活稀有属性
            case "205":
                ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                break;

            //装备强化
            case "206":
                //ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                ChengJiu_WriteValue_3("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计获得宠物宝宝
            case "207":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计获得合宠次数
            case "208":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //宠物最高技能书
            case "209":
                ChengJiu_WriteValue_3("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计打书次数
            case "210":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //合宠合出大海龟次数
            case "211":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //家园等级
            case "220":
                ChengJiu_WriteValue_3("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //坐骑激活
            case "221":
                ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                break;

            //坐骑能力等级
            case "222":
                ChengJiu_WriteValue_3("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //饲养动物激活
            case "223":
                ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                break;

            //饲养动物数量
            case "224":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //累计家园资金
            case "225":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //染色次数
            case "226":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //家园交互次数
            case "227":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //家园收获
            case "228":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //商人订单
            case "229":
                ChengJiu_WriteValue_2("ChengJiu_" + writeType, int.Parse(writeNum));
                break;

            //觉醒
            case "250":
                ChengJiu_WriteValue_1("ChengJiu_" + writeType, writeValue, writeNum);
                break;
        }

        bool jianceStatus = true;
        if (writeType == "1" || writeType == "2"|| writeType == "3" || writeType == "228" || writeType == "109" || writeType == "204" || writeType == "105" || writeType == "101") {
            //统计怪物的进行提示(怪物类的为每次打开界面后检查,要不每杀1个怪会就检查会卡)  || 
            jianceStatus = false;
        }

        if (jianceStatus) {
            //Debug.Log("写入成就:" + writeType);
            Game_PublicClassVar.Get_game_PositionVar.ChengJiuJianCeStatus = true;

            if (Game_PublicClassVar.Get_game_PositionVar.ChengJiuJianCeChengJiuIDStr == ""|| Game_PublicClassVar.Get_game_PositionVar.ChengJiuJianCeChengJiuIDStr == null)
            {
                Game_PublicClassVar.Get_game_PositionVar.ChengJiuJianCeChengJiuIDStr = writeType;
            }
            else
            {
                if (Game_PublicClassVar.Get_game_PositionVar.ChengJiuJianCeChengJiuIDStr.Contains(writeType) == false)
                {
                    Game_PublicClassVar.Get_game_PositionVar.ChengJiuJianCeChengJiuIDStr = Game_PublicClassVar.Get_game_PositionVar.ChengJiuJianCeChengJiuIDStr + writeType;
                }
            }

            
            //检测是否有成就完成
            string comChengJiuIDSet = ChengJiu_JianCeTargetSonTypeChengJiuID(writeType);
            if (comChengJiuIDSet != "")
            {
                //Debug.Log("comChengJiuIDSet = " + comChengJiuIDSet);
                string[] comChengJiuIDList = comChengJiuIDSet.Split(';');
                for (int i = 0; i < comChengJiuIDList.Length; i++)
                {
                    //写入成就ID
                    ChengJiu_WriteComChengJiuID(comChengJiuIDList[i]);
                    //弹出成就提示
                    ChengJiu_ComHint(comChengJiuIDList[i]);
                }
            }
            
        }
    }

    //成就展示
    public void ChengJiu_ComHint(string chengJiuID) {

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_ChengJiuHintObj != null) {

            GameObject hintObj = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_ChengJiuHintObj);
            hintObj.GetComponent<UI_ChengJiuHintObj>().ChengJiuID = chengJiuID;
            hintObj.GetComponent<UI_ChengJiuHintObj>().ShowChengJiu();
            hintObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet.transform);
            hintObj.transform.localScale = new Vector3(1, 1, 1);
            //float chang = Game_PublicClassVar.Get_function_UI.ReturnScreen_X(683);
            hintObj.transform.localPosition = new Vector3(683, 150, 0);
        }

    }

    //成就写入值方式1(目标格式为Key,Value;Key,Value;Key,Value……写入方式为匹配key值,增加Value值)
    public void ChengJiu_WriteValue_1(string writeChengJiuZiDuan, string writeValue, string writeNum) {

        string chengjiu_1_Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(writeChengJiuZiDuan, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");

        //获取怪物列表
        bool writeStatus = false;
        string writeStr = "";
        if (chengjiu_1_Str != "" && chengjiu_1_Str != "0")
        {
            string[] monsterSetList = chengjiu_1_Str.Split(';');
            for (int i = 0; i < monsterSetList.Length; i++)
            {
                string[] monsterIDList = monsterSetList[i].Split(',');
                if (monsterIDList[0] == writeValue)
                {
                    int writeKillNum = int.Parse(monsterIDList[1]) + int.Parse(writeNum);
                    monsterIDList[1] = writeKillNum.ToString();
                    writeStatus = true;
                    writeStr = writeStr + monsterIDList[0] + "," + monsterIDList[1] + ";";
                }
                else {
                    writeStr = writeStr + monsterIDList[0] + "," + monsterIDList[1] + ";";
                }
            }

            //循环写入
            /*
            for (int i = 0; i < monsterSetList.Length; i++)
            {
                string[] monsterIDList = monsterSetList[i].Split(',');
                if (monsterIDList.Length >= 2)
                {
                    writeStr = writeStr + monsterIDList[0] + "," + monsterIDList[1] + ";";
                }
            }
            */
        }

        //写入值
        if (!writeStatus)
        {
            writeStr = writeStr + writeValue + "," + writeNum + ";";
        }

        if (writeStr != "")
        {
            writeStr = writeStr.Substring(0, writeStr.Length-1);
        }

        //记录值
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData(writeChengJiuZiDuan, writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseChengJiu");

    }

    //成就写入值方式2(目标格式为Value（单值）写入方式为匹配key值,增加Value值)
    public void ChengJiu_WriteValue_2(string writeChengJiuZiDuan,int writeNum) {

        string chengjiu_2_Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(writeChengJiuZiDuan, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        if (chengjiu_2_Str == "") {
            chengjiu_2_Str = "0";
        }

        int shaGuaiNum = int.Parse(chengjiu_2_Str);
        shaGuaiNum = shaGuaiNum + writeNum;

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData(writeChengJiuZiDuan, shaGuaiNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseChengJiu");

    }

    //成就写入值方式1(目标格式为Value（单值）写入方式为匹配key值,增加Value值)（替换制替换制替换制！取最高）
    public void ChengJiu_WriteValue_3(string writeChengJiuZiDuan, int writeNum)
    {

        string chengjiu_2_Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(writeChengJiuZiDuan, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        if (chengjiu_2_Str == "") {
            chengjiu_2_Str = "0";
        }
        int shaGuaiNum = int.Parse(chengjiu_2_Str);
        if (writeNum > shaGuaiNum) {
            shaGuaiNum = writeNum;

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData(writeChengJiuZiDuan, shaGuaiNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseChengJiu");
        }
    }

    //判定当前成就是否完成（通过值去判断）
    public bool ChengJiu_IfComChengJiu(string chengjiuID) {

        string targetValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue", "ID", chengjiuID, "ChengJiu_Template");
        string nowValueStr = ChengJiu_ReadChengJiuIDValue(chengjiuID);
        //Debug.Log("targetValue = " + targetValue + ";nowValueStr = " + nowValueStr);
        if (targetValue != "" && nowValueStr != "") {
            if (int.Parse(nowValueStr) >= int.Parse(targetValue)){
                return true;
            }
            else {
                return false;
            }
        }
        else{
            return false;
        }
    }

    //通过完成的成就ID判定
    public bool ChengJiu_IfComChengJiu_2(string chengjiuID,string[] comChengJiuIDSet=null)
    {
        if (chengjiuID == "") {
            return false;
        }

        //读取当前已完成的成就ID
        if (comChengJiuIDSet == null) {
            string comChengJiuID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ComChengJiuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
            comChengJiuIDSet = comChengJiuID.Split(';');
        }

        for (int i = 0; i < comChengJiuIDSet.Length; i++) {
            if (comChengJiuIDSet[i] == chengjiuID) {
                return true;
            }
        }

        return false;

    }

    //写入完成成就ID
    public void ChengJiu_WriteComChengJiuID(string chengJiuID) {

        if (chengJiuID == "" || chengJiuID == "0") {
            return;
        }

        string comChengJiuID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ComChengJiuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        if (!ChengJiu_IfComChengJiu_2(chengJiuID)) {
            if (comChengJiuID != "")
            {
                comChengJiuID = comChengJiuID + ";" + chengJiuID;
            }
            else {
                comChengJiuID = chengJiuID;
            }
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ComChengJiuID", comChengJiuID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseChengJiu");
    }

    //读取当前的成就任务(是否强制刷新当前任务)
    public string ChengJiu_GetAllChengJiuID(bool getStatus = false) {

        string chengJiuSetStr = "";
        //bool getStatus = false;
        if (Game_PublicClassVar.Get_wwwSet.RoseChengJiuSetID == "" || Game_PublicClassVar.Get_wwwSet.RoseChengJiuSetID == null)
        {
            getStatus = true;
        }
        else {
            chengJiuSetStr = Game_PublicClassVar.Get_wwwSet.RoseChengJiuSetID;
        }

        if (getStatus) {
            string chengJiuStr_1002 = ChengJiu_GetTargerTypeChengJiuIDSet("1002");
            string chengJiuStr_1003 = ChengJiu_GetTargerTypeChengJiuIDSet("1003");
            string chengJiuStr_1004 = ChengJiu_GetTargerTypeChengJiuIDSet("1004");

            //拼接字符串
            if (chengJiuStr_1002 != "")
            {
                chengJiuStr_1002 = chengJiuStr_1002 + ";";
            }

            if (chengJiuStr_1003 != "")
            {
                chengJiuStr_1003 = chengJiuStr_1003 + ";";
            }

            chengJiuSetStr = chengJiuStr_1002 + chengJiuStr_1003 + chengJiuStr_1004;
        }

        return chengJiuSetStr;

    }

    //读取指定类型（重新筛选一遍当前的成就列表）
    public string ChengJiu_GetTargerTypeChengJiuIDSet(string chengJiuType) {

        string chengJiuSetStr = "";
        string chengJiuStr_Com = ChengJiu_GetTargetZiDuanChengJiuID(chengJiuType, "ChengJiuSet_Com");
        string chengJiuStr_ZhangJie_1 = ChengJiu_GetTargetZiDuanChengJiuID(chengJiuType, "ChengJiuSet_ZhangJie_1");
        string chengJiuStr_ZhangJie_2 = ChengJiu_GetTargetZiDuanChengJiuID(chengJiuType, "ChengJiuSet_ZhangJie_2");
        string chengJiuStr_ZhangJie_3 = ChengJiu_GetTargetZiDuanChengJiuID(chengJiuType, "ChengJiuSet_ZhangJie_3");
        string chengJiuStr_ZhangJie_4 = ChengJiu_GetTargetZiDuanChengJiuID(chengJiuType, "ChengJiuSet_ZhangJie_4");
        string chengJiuStr_ZhangJie_5 = ChengJiu_GetTargetZiDuanChengJiuID(chengJiuType, "ChengJiuSet_ZhangJie_5");

        //拼接字符串
        if (chengJiuStr_Com != "") {
            chengJiuStr_Com = chengJiuStr_Com + ";";
        }

        if (chengJiuStr_ZhangJie_1 != "")
        {
            chengJiuStr_ZhangJie_1 = chengJiuStr_ZhangJie_1 + ";";
        }

        if (chengJiuStr_ZhangJie_2 != "")
        {
            chengJiuStr_ZhangJie_2 = chengJiuStr_ZhangJie_2 + ";";
        }

        if (chengJiuStr_ZhangJie_3 != "")
        {
            chengJiuStr_ZhangJie_3 = chengJiuStr_ZhangJie_3 + ";";
        }

        if (chengJiuStr_ZhangJie_4 != "")
        {
            chengJiuStr_ZhangJie_4 = chengJiuStr_ZhangJie_4 + ";";
        }
        /*
        if (chengJiuStr_ZhangJie_5 != "")
        {
            chengJiuStr_ZhangJie_5 = chengJiuStr_ZhangJie_5 + ";";
        }
        */

        chengJiuSetStr = chengJiuStr_Com + chengJiuStr_ZhangJie_1 + chengJiuStr_ZhangJie_2 + chengJiuStr_ZhangJie_3 + chengJiuStr_ZhangJie_4 + chengJiuStr_ZhangJie_5;

        return chengJiuSetStr;
    }

    //读取指定类型的指定字段的成就(读取的未完成的成就)
    public string ChengJiu_GetTargetZiDuanChengJiuID(string chengJiuTypeID,string chengJiuZiDuan) {

        //读取当前已完成的成就ID
        string comChengJiuID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ComChengJiuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        string[] comChengJiuIDList = comChengJiuID.Split(';');

        //成就集合
        string returnChengJiuIDStr = "";
        string chengJiuSetID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(chengJiuZiDuan, "ID", chengJiuTypeID, "ChengJiuAll_Template");

        string[] chengJiuSetIDList = chengJiuSetID.Split(';');
        for (int i = 0; i < chengJiuSetIDList.Length; i++) {
            string jianceChengJiuID = chengJiuSetIDList[i];
            int sum = 0;
            while (true) {
                //读取当前任务是否完成
                if (ChengJiu_IfComChengJiu_2(jianceChengJiuID))
                {
                    //获取下一级任务
                    jianceChengJiuID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", jianceChengJiuID, "ChengJiu_Template");
                    if (jianceChengJiuID == "" || jianceChengJiuID == "0")
                    {
                        //没有下一级任务
                        break;
                    }
                }
                else {
                    //此任务未完成
                    returnChengJiuIDStr = returnChengJiuIDStr + jianceChengJiuID + ";";
                    break;
                }

                //防止死循环
                sum = sum + 1;
                if (sum >= 1000) {
                    break;
                }
            }
        }

        if (returnChengJiuIDStr != "") {
            returnChengJiuIDStr = returnChengJiuIDStr.Substring(0, returnChengJiuIDStr.Length-1);
        }
        return returnChengJiuIDStr;
    }

    //读取指定类型的指定字段的成就(读取的已经完成的成就)
    public string ChengJiu_GetTargetZiDuanChengJiuID_YiWanCheng(string chengJiuTypeID, string chengJiuZiDuan)
    {

        //读取当前已完成的成就ID
        string comChengJiuID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ComChengJiuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        string[] comChengJiuIDList = comChengJiuID.Split(';');

        //成就集合
        string returnChengJiuIDStr = "";
        string chengJiuSetID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(chengJiuZiDuan, "ID", chengJiuTypeID, "ChengJiuAll_Template");

        string[] chengJiuSetIDList = chengJiuSetID.Split(';');
        for (int i = 0; i < chengJiuSetIDList.Length; i++)
        {
            string jianceChengJiuID = chengJiuSetIDList[i];
            int sum = 0;
            while (true)
            {
                //读取当前任务是否完成
                if (ChengJiu_IfComChengJiu_2(jianceChengJiuID))
                {
                    returnChengJiuIDStr = returnChengJiuIDStr + jianceChengJiuID + ";";
                    //获取下一级任务
                    jianceChengJiuID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", jianceChengJiuID, "ChengJiu_Template");
                    if (jianceChengJiuID == "" || jianceChengJiuID == "0")
                    {
                        //没有下一级任务
                        break;
                    }
                    else {
                        
                    }
                }
                else
                {
                    //此任务未完成
                    break;
                }

                //防止死循环
                sum = sum + 1;
                if (sum >= 1000)
                {
                    break;
                }
            }
        }

        if (returnChengJiuIDStr != "")
        {
            returnChengJiuIDStr = returnChengJiuIDStr.Substring(0, returnChengJiuIDStr.Length - 1);
        }
        return returnChengJiuIDStr;
    }


    //读取指定子类型当前任务
    public string ChengJiu_GetTargetSonTypeChengJiuID(string chengJiuSonType) {

        string comChengJiuSetStr = "";
        //Debug.Log("111111111111111:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
        string chengJiuSetStr = ChengJiu_GetAllChengJiuID();
        //Debug.Log("222222222222222:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
        if (chengJiuSetStr != "") {
            string[] chengjiuSetList = chengJiuSetStr.Split(';');
            for (int i = 0; i < chengjiuSetList.Length; i++) {
                string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", chengjiuSetList[i], "ChengJiu_Template");
                if (targetType == chengJiuSonType) {
                    if (ChengJiu_IfComChengJiu(chengjiuSetList[i])) {
                        //成就完成
                        comChengJiuSetStr = comChengJiuSetStr + chengjiuSetList[i] + ";";
                    }
                }
            }
        }
        //Debug.Log("33333333333333333:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
        if (comChengJiuSetStr != "") {
            comChengJiuSetStr = comChengJiuSetStr.Substring(0, comChengJiuSetStr.Length - 1);
        }
        return comChengJiuSetStr;
    }

    //检测当前子类型任务是否有完成的任务
    public string ChengJiu_JianCeTargetSonTypeChengJiuID(string chengJiuSonType, bool ifHint = false) {

        if (chengJiuSonType == "")
        {
            return "";
        }

        string comChengJiuSetStr = "";

        string[] chengJiuSonTypeList = chengJiuSonType.Split(';');
        for (int y = 0; y < chengJiuSonTypeList.Length; y++) {

            //Debug.Log("aaaaaaaaaaa：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));

            string nowChengJiuSonType = chengJiuSonTypeList[y];
            string chengJiuIDSet = ChengJiu_GetTargetSonTypeChengJiuID(nowChengJiuSonType);
            if (chengJiuIDSet != "") {
                string[] chengjiuSetList = chengJiuIDSet.Split(';');
                for (int i = 0; i < chengjiuSetList.Length; i++)
                {
                    //Debug.Log("bbbbbbbbbbbbbbb：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                    //通过值去检测成就是否完成
                    if (ChengJiu_IfComChengJiu(chengjiuSetList[i]))
                    {
                        //Debug.Log("cccccccccccccc：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                        //检测成就是否已经完成
                        if (!ChengJiu_IfComChengJiu_2(chengjiuSetList[i]))
                        {
                            //成就完成
                            comChengJiuSetStr = comChengJiuSetStr + chengjiuSetList[i] + ";";
                            //Debug.Log("ddddddddddddd：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                        }
                    }
                }
            }


        }

        if (comChengJiuSetStr != "")
        {
            comChengJiuSetStr = comChengJiuSetStr.Substring(0, comChengJiuSetStr.Length - 1);
        }

        return comChengJiuSetStr;
    }

    //检测当前是否有完成的成就
    public string ChengJiu_JianCeAllChengJiuID() {

        string comChengJiuSetStr = "";
        string chengJiuSetStr = ChengJiu_GetAllChengJiuID();
        if (chengJiuSetStr != "")
        {
            string[] chengjiuSetList = chengJiuSetStr.Split(';');
            for (int i = 0; i < chengjiuSetList.Length; i++)
            {
                if (ChengJiu_IfComChengJiu(chengjiuSetList[i]))
                {
                    //成就完成
                    comChengJiuSetStr = comChengJiuSetStr + chengjiuSetList[i] + ";";
                }
            }
        }

        if (comChengJiuSetStr != "")
        {
            comChengJiuSetStr = comChengJiuSetStr.Substring(0, comChengJiuSetStr.Length - 1);
        }
        return comChengJiuSetStr;

    }

    /*
    //检测指定类型的成就是否有有完成,有完成返回完成成就字符串
    public string ChengJiu_JianCeTargetTypeChengJiuID(string chengJiuType) {
    }
    */

    //读取指定成就的当前值
    public string ChengJiu_ReadChengJiuIDValue(string chengjiuID) {

        string readType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType","ID", chengjiuID, "ChengJiu_Template");
        string readValue_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetID", "ID", chengjiuID, "ChengJiu_Template");
        return ChengJiu_ReadValue(readType, readValue_1);
    }


    //写入成就（写成成就类型,写入成就值,写入成就数量）
    public string ChengJiu_ReadValue(string ReadType, string ReadValue)
    {
        string returnStr = "0";
        switch (ReadType)
        {
            //击杀指定数量怪物
            case "1":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;

            //击杀任意数量怪物
            case "2":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //击杀领主级数量怪物
            case "3":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //击杀指定BOSS(普通难度)
            case "4":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;

            //击杀指定BOSS(困难难度)
            case "5":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;

            //击杀指定BOSS(地狱难度)
            case "6":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;

            //击杀指定BOSS(极限难度)
            case "7":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;

            //进入某个地图的次数
            case "101":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;

            //累计发现第一章黄金宝箱数量（章节数,对应宝箱数量;章节数,对应宝箱数量;）
            case "102":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;

            //挖掘藏宝图的次数
            case "103":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //挖掘高级藏宝图的次数
            case "104":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //累计获得金币的值
            case "105":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //累计十连抽次数
            case "106":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //累计装备洗炼次数
            case "107":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //累计天赋重置次数
            case "108":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //累计在线时间(分钟)
            case "109":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //累计复活次数
            case "110":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //累计游戏分享次数
            case "111":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //玩家等级
            case "201":
                returnStr = Game_PublicClassVar.Get_function_Rose.GetRoseLv().ToString();
            break;

            //玩家打造熟练度
            case "202":
                returnStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Proficiency_1","ID",Game_PublicClassVar.Get_wwwSet.RoseID,"RoseData");
                break;

            //玩家炼金熟练度
            case "203":
                returnStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Proficiency_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                break;

            //累计收集装备套装(对应激活套装ID,预留参数(任意填即可));
            case "204":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;

            //累计激活稀有属性
            case "205":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;

            //全身强化装备
            case "206":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //累计获得宠物宝宝
            case "207":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //累计获得合宠次数
            case "208":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //宠物最高技能书
            case "209":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //累计打书次数
            case "210":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //合宠合出大海龟次数
            case "211":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //家园等级
            case "220":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //累计激活坐骑
            case "221":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;

            //坐骑能力
            case "222":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //坐骑养殖
            case "223":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;

            //饲养总数
            case "224":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //家园资金
            case "225":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //家园染色
            case "226":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //家园交互
            case "227":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //家园收获
            case "228":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //商人订单
            case "229":
                returnStr = ChengJiu_ReadValue_2("ChengJiu_" + ReadType);
                break;

            //觉醒
            case "250":
                returnStr = ChengJiu_ReadValue_1("ChengJiu_" + ReadType, ReadValue);
                break;
        }

        return returnStr;
    }


    //成就写入值方式1(目标格式为Key,Value;Key,Value;Key,Value……写入方式为匹配key值,增加Value值)
    public string ChengJiu_ReadValue_1(string ReadChengJiuZiDuan, string ReadValue)
    {

        string chengjiu_1_Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(ReadChengJiuZiDuan, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");

        //获取列表
        if (chengjiu_1_Str != "" && chengjiu_1_Str != "0")
        {
            string[] monsterSetList = chengjiu_1_Str.Split(';');
            for (int i = 0; i < monsterSetList.Length; i++)
            {
                string[] monsterIDList = monsterSetList[i].Split(',');
                if (monsterIDList[0] == ReadValue)
                {
                    return monsterIDList[1];
                }
            }
        }

        return "0";
    }

    //成就写入值方式1(目标格式为Value（单值）写入方式为匹配key值,增加Value值)
    public string ChengJiu_ReadValue_2(string ReadChengJiuZiDuan)
    {

        string chengjiu_2_Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(ReadChengJiuZiDuan, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        if (chengjiu_2_Str == "" || chengjiu_2_Str == null) {
            chengjiu_2_Str = "0";
        }
        return chengjiu_2_Str;

    }



    //领取成就奖励
    public void ChengJiu_RewardLingQu(string rewardID) {

        if (ChengJiu_RewardChaXun(rewardID) == false)
        {
            string chengjiu_Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ComChengJiuRewardID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
            bool sendStatus = false;

            //获取目标成就点数
            string chengJiuNeedNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiuNeedNum", "ID", rewardID, "ChengJiuReward_Template");
            int roseChengJiuNum = ChengJiu_GetComChengJiuNum();

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_262");

            if (roseChengJiuNum < int.Parse(chengJiuNeedNum)) {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("领取失败!成就点数不足!");
                return;
            }

            if (Game_PublicClassVar.Get_function_Rose.GetRoseLv()<=10) {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("领取失败!成就点数不足!");
                //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000116, "领取失败!成就点数不足" + "chengjiu_Str = "+ chengjiu_Str + ";roseChengJiuNum" + roseChengJiuNum +";chengJiuNeedNum = " + chengJiuNeedNum + ";rewardID = " + rewardID);
                Game_PublicClassVar.Get_function_Rose.ServerMsg_SendMsg("领取失败!成就点数不足" + "chengjiu_Str = "+ chengjiu_Str + "; roseChengJiuNum" + roseChengJiuNum +"; chengJiuNeedNum = " + chengJiuNeedNum + "; rewardID = " + rewardID);
                return;
            }

            //判定背包格子
            string reward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Reward", "ID", rewardID, "ChengJiuReward_Template");
            if (reward != "")
            {
                string[] rewardItemList = reward.Split(';');
                int needBagNum = rewardItemList.Length;
                if (Game_PublicClassVar.Get_function_Rose.BagNullNum() >= needBagNum) {
                    //循环发送奖励
                    for (int i = 0; i < rewardItemList.Length; i++)
                    {
                        //发送奖励
                        string[] itemList = rewardItemList[i].Split(',');
                        string itemID = itemList[0];
                        string itemNum = itemList[1];
                        Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID, int.Parse(itemNum),"0",0,"0",true,"42");
                    }
                    sendStatus = true;
                }
            }


            //写入发送奖励数据
            if (sendStatus)
            {
                if (chengjiu_Str != "")
                {
                    chengjiu_Str = chengjiu_Str + ";" + rewardID;
                }
                else
                {
                    chengjiu_Str = rewardID;
                }

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ComChengJiuRewardID", chengjiu_Str, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseChengJiu");

                //领取成就发送广播
                //string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                //string hintName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", rewardID, "ChengJiuReward_Template");
                //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "恭喜玩家" + roseName + "领取"+ hintName + "!获得成就内的丰厚奖励！");
            }
        }
        else {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_264");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("此成就奖励已经领取");
        }
    }


    //判定成就奖励是否已经被领取
    public bool ChengJiu_RewardChaXun(string rewardID) {

        bool ifGetStatus = false;
        string chengjiu_Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ComChengJiuRewardID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        string[] chengJiuRewardList = chengjiu_Str.Split(';');
        for (int i = 0; i < chengJiuRewardList.Length; i++)
        {
            if (chengJiuRewardList[i] == rewardID) {
                ifGetStatus = true;
            }
        }

        return ifGetStatus;
    }


    //获取当前获得的成就点数
    public int ChengJiu_GetComChengJiuNum() {

        int chengJiuNum = 0;
        ObscuredString chengJiu_Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ComChengJiuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        if (chengJiu_Str != "") {
            string[] chengJiuList = chengJiu_Str.ToString().Split(';');
            for (int i = 0; i < chengJiuList.Length; i++) {
                //获取目标成就点数
                ObscuredString rewardNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RewardNum", "ID", chengJiuList[i], "ChengJiu_Template");
                if (rewardNumStr == "") {
                    rewardNumStr = "0";
                }
                chengJiuNum = chengJiuNum + int.Parse(rewardNumStr);
            }
        }
        return chengJiuNum;
    }

    //获取对应字段的成就点数
    public int ChengJiu_GetComTargetStrChengJiuNum(string rewardStr) {
        int chengJiuNum = 0;
        string chengJiu_Str = rewardStr;
        if (chengJiu_Str != "")
        {
            string[] chengJiuList = chengJiu_Str.Split(';');
            for (int i = 0; i < chengJiuList.Length; i++)
            {
                //获取目标成就点数
                string rewardNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RewardNum", "ID", chengJiuList[i], "ChengJiu_Template");
                if (rewardNumStr == "")
                {
                    rewardNumStr = "0";
                }
                chengJiuNum = chengJiuNum + int.Parse(rewardNumStr);
            }
        }
        return chengJiuNum;
    }


    //检测任务(激活完成觉醒任务)
    public void jueXingJianCe()
    {

        string jihuoIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (jihuoIDStr.Contains("10001"))
        {
            return;
        }

        string completeTaskStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (completeTaskStr.Contains("33000018"))
        {

            if (jihuoIDStr == "" || jihuoIDStr == "0" || jihuoIDStr == null)
            {
                jihuoIDStr = "10001";
            }
            else
            {
                jihuoIDStr = "10001" + ";" + jihuoIDStr;
            }
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JueXingJiHuoID", jihuoIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            Game_PublicClassVar.Get_game_PositionVar.JueXingStatus = true;
        }
    }

}
