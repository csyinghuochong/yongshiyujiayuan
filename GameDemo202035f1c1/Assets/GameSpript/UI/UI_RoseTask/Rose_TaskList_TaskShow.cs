using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Rose_TaskList_TaskShow : MonoBehaviour {

    //任务描述控件
    public GameObject UI_TaskDescribe;
    //任务目标控件
    public GameObject UI_TaskTarget_1;
    public GameObject UI_TaskTarget_2;
    public GameObject UI_TaskTarget_3;

    //任务奖励
    public GameObject UI_TaskTrophy_Exp;
    public GameObject UI_TaskTrophy_Money;

	//追踪任务按钮
	public GameObject UI_Btn_MainTaskWrite;
	public GameObject UI_Btn_MainTaskDelete;

	public GameObject UI_ItemTrophySet;				//任务道具奖励父级
	public GameObject Obj_UI_TaskTrophyList;		//任务道具奖励显示

    private Color TaskColor_Complete;
    private Color TaskColor_NoComplete;
    private string CompleteTaskAddStr;

	// Use this for initialization
	void Start () {

        TaskColor_Complete = new Color((float)25 / 255, (float)150 / 255, (float)30 / 255, 1.0f);
        TaskColor_NoComplete = new Color((float)86 / 255, (float)59 / 255, (float)33 / 255, 1.0f);
        string langstr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("已完成");
        CompleteTaskAddStr = " " + "(" + langstr + ")";

        updateTaskData();



	}
	
	// Update is called once per frame
	void Update () {
    
        if (Game_PublicClassVar.Get_game_PositionVar.Rose_TaskDataUpdata) {
            
            updateTaskData();
            Game_PublicClassVar.Get_game_PositionVar.Rose_TaskDataUpdata = false;
            Debug.Log("任务信息刷新");
        }
	}

    private void updateTaskData() {

        Debug.Log("更新任务1111");

        string fuhelangStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("找到符合要求的装备");

        

        //检测当前主线显示的3个任务是否假的



        //获取当前任务第一个任务,作为当前选中任务
        if (Game_PublicClassVar.Get_game_PositionVar.NowTaskID == "") {
			string[] achievementTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
			if (achievementTaskID [0] != "") {
				Game_PublicClassVar.Get_game_PositionVar.NowTaskID=achievementTaskID [0];
			}
		}
		string nowTaskID = Game_PublicClassVar.Get_game_PositionVar.NowTaskID;

		//显示追踪任务按钮
        
		if (Game_PublicClassVar.Get_function_Task.IfMainUITask(nowTaskID)) {
			UI_Btn_MainTaskWrite.SetActive (false);
			UI_Btn_MainTaskDelete.SetActive (true);
		} else {
			UI_Btn_MainTaskWrite.SetActive(true);
			UI_Btn_MainTaskDelete.SetActive(false);
		}
		if (nowTaskID == "") {
			UI_Btn_MainTaskWrite.SetActive(false);
			UI_Btn_MainTaskDelete.SetActive(false);
		}
        

        //如果上一次没有任务默认选择第一个任务显示
        if (nowTaskID == "0" || nowTaskID == "") { 
            //获取自身上的任务
            string taskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            if (taskID != "") {
                string[] taskIDList = taskID.Split(',');
                //找到第一个任务ID进行设置
                nowTaskID = taskIDList[0];
            }
        }

        if (nowTaskID != "0" && nowTaskID != "")
        {
            //更新任务描述
            string taskDesCribe = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskDes", "ID", nowTaskID, "Task_Template");
            UI_TaskDescribe.GetComponent<Text>().text = taskDesCribe;
            //更新任务目标
            UI_TaskTarget_1.SetActive(false);
            UI_TaskTarget_2.SetActive(false);
            UI_TaskTarget_3.SetActive(false);
            //获取目标1
            string TaskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1","ID",nowTaskID,"Task_Template");
            Debug.Log("TaskTarget1 = " + TaskTarget1);
            if (TaskTarget1 != "0") {
                UI_TaskTarget_1.SetActive(true);
                //获取目标值
                string TaskValuePro1 = Game_PublicClassVar.Get_function_Task.TaskReturnValue(nowTaskID,"1");
                if (TaskValuePro1 == "") {
                    TaskValuePro1 = "0";
                }
                string TargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", nowTaskID, "Task_Template");
                //判定当前任务目标是获得道具还是杀怪
				string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", nowTaskID, "Task_Template");
				switch (targetType){ 
                    
                //杀怪
                case "1":
                    
                        //获取杀怪名称
						string monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", TaskTarget1, "Monster_Template");
                        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("击杀");
                        //改变字体颜色
                        if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
                        {
                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_Complete;

                            //显示当前获取的道具
                            UI_TaskTarget_1.GetComponent<Text>().text = langStr + monsterName + " : " + TaskValuePro1 + " / " + TargetValue1 + CompleteTaskAddStr;
                        }
                        else {
                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_NoComplete;
                            //显示当前获取的道具
                            UI_TaskTarget_1.GetComponent<Text>().text = langStr + monsterName + " : " + TaskValuePro1 + " / " + TargetValue1;
                        }
                    break;
                    
                //道具
				case "2":
					//获取道具名称
					string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", TaskTarget1, "Item_Template");
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("获得");
                    //改变字体颜色
                    if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
					{
                        UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_Complete;
                        //显示当前获取的道具
                        UI_TaskTarget_1.GetComponent<Text>().text = langStr + itemName + " : " + TaskValuePro1 + " / " + TargetValue1 + CompleteTaskAddStr;
					}
					else {
                        UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_NoComplete;
                        //显示当前获取的道具
                        UI_TaskTarget_1.GetComponent<Text>().text = langStr + itemName + " : " + TaskValuePro1 + " / " + TargetValue1;
					}
					break;
				//对话Npc
				case "3":
					/*
					//获取道具名称
					string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", TaskTarget1, "Npc_Template");
					//改变字体颜色
					if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
					{
                        UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_Complete;
					}
					else {

                        UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_NoComplete;
					}
					
					//显示当前获取的道具
					UI_TaskTarget_1.GetComponent<Text>().text = "寻找" + npcName + " : " + TaskValuePro1 + " / " + TargetValue1;
					*/
					break;
               

                //等级达到某个值
				case "4":
					
					//获取道具名称
					//string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", TaskTarget1, "Npc_Template");
					//改变字体颜色
					if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
					{
                        UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_Complete;
					}
					else {

                        UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_NoComplete;
					}

                    //显示当前获取的道具
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级到达");
                    UI_TaskTarget_1.GetComponent<Text>().text = langStr + ": " + TaskValuePro1 + " / " + TargetValue1;
					
					break;


                    //击杀任意怪物
                    case "5":

                        //获取道具名称
                        //string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", TaskTarget1, "Npc_Template");
                        //改变字体颜色
                        if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
                        {
                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_Complete;
                        }
                        else
                        {

                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_NoComplete;
                        }

                        //显示当前获取的道具
                        UI_TaskTarget_1.GetComponent<Text>().text = "击杀任意怪物: " + TaskValuePro1 + " / " + TargetValue1;

                        break;

                    //击杀BOSS级别怪物
                    case "6":

                        //获取道具名称
                        //string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", TaskTarget1, "Npc_Template");
                        //改变字体颜色
                        if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
                        {
                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_Complete;
                        }
                        else
                        {

                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_NoComplete;
                        }

                        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("击杀BOSS级别怪物");
                        //显示当前获取的道具
                        UI_TaskTarget_1.GetComponent<Text>().text = langStr + ": " + TaskValuePro1 + " / " + TargetValue1;

                        break;

                    //抓获宠物
                    case "7":

                        //获取道具名称
                        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", TaskTarget1, "Pet_Template");
                        //改变字体颜色
                        if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
                        {
                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_Complete;
                        }
                        else
                        {

                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_NoComplete;
                        }

                        string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("抓获");
                        string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("宠物");
                        //显示当前获取的道具
                        UI_TaskTarget_1.GetComponent<Text>().text = langStr_1 + petName + langStr_2 + ": " + TaskValuePro1 + " / " + TargetValue1;

                        break;

                    //装备指定属性值
                    case "11":

                        //改变字体颜色
                        if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
                        {
                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_Complete;
                        }
                        else
                        {

                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_NoComplete;
                        }

                        //显示当前获取的道具
                        UI_TaskTarget_1.GetComponent<Text>().text = fuhelangStr + ": " + TaskValuePro1 + " / " + TargetValue1;

                        break;

                    //指定品质的装备
                    case "12":

                        //改变字体颜色
                        if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
                        {
                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_Complete;
                        }
                        else
                        {

                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_NoComplete;
                        }

                        //显示当前获取的道具
                        UI_TaskTarget_1.GetComponent<Text>().text = fuhelangStr + ": " + TaskValuePro1 + " / " + TargetValue1;

                        break;

                    //装指定部位的奖励
                    case "13":

                        //改变字体颜色
                        if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
                        {
                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_Complete;
                        }
                        else
                        {

                            UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_NoComplete;
                        }

                        //显示当前获取的道具
                        UI_TaskTarget_1.GetComponent<Text>().text = fuhelangStr + ": " + TaskValuePro1 + " / " + TargetValue1;

                        break;




                }
            }



        else
        {
            //Debug.Log("sdsdsdsdsdsd,nowTaskID = " + nowTaskID);
            //清空显示框
            //UI_TaskTarget_1.GetComponent<Text>().text = "";
            //判定当前任务类型
                
            string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", nowTaskID, "Task_Template");
            if (targetType == "3")
            {
                UI_TaskTarget_1.SetActive(true);
                //获取道具名称
                string npcID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteNpcID", "ID", nowTaskID, "Task_Template");
                string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", npcID, "Npc_Template");
                UI_TaskTarget_1.GetComponent<Text>().color = TaskColor_Complete;       //这里不能用上面的颜色初始化变量,要不会变成透明的
                
                //显示当前获取的道具
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("寻找");
                UI_TaskTarget_1.GetComponent<Text>().text = langStr + "：" + npcName;
                //Debug.Log("sdsdsdsdsdsd,nowTaskID2222 = " + TaskColor_NoComplete);
            }
                



        }

            //获取目标2
            string TaskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", nowTaskID, "Task_Template");
            if (TaskTarget2 != "0")
            {

                UI_TaskTarget_2.SetActive(true);

                //获取目标值
                string TaskValuePro2 = Game_PublicClassVar.Get_function_Task.TaskReturnValue(nowTaskID, "2");
                string TargetValue2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue2", "ID", nowTaskID, "Task_Template");
                //判定当前任务目标是获得道具还是杀怪（根据ID规则判定）

                switch (TaskTarget1[0].ToString())
                {

                    //道具类
                    case "1":

                        //获取道具名称
                        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", TaskTarget2, "Item_Template");
                        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("获得");
                        //改变字体颜色
                        if (int.Parse(TaskValuePro2) >= int.Parse(TargetValue2))
                        {

                            UI_TaskTarget_2.GetComponent<Text>().color = TaskColor_Complete;
                            //显示当前获取的道具
                            UI_TaskTarget_2.GetComponent<Text>().text = langStr + itemName + " : " + TaskValuePro2 + " / " + TargetValue2 + CompleteTaskAddStr;

                        }
                        else
                        {

                            UI_TaskTarget_2.GetComponent<Text>().color = TaskColor_NoComplete;
                            //显示当前获取的道具
                            UI_TaskTarget_2.GetComponent<Text>().text = langStr + itemName + " : " + TaskValuePro2 + " / " + TargetValue2;

                        }


                        break;

                    //怪物类（以后添加）

                }

            }
            else { 
                
                //判定当前任务类型
                /*
                string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", nowTaskID, "Task_Template");
                if (targetType == "3")
                {
                    UI_TaskTarget_2.SetActive(false);
                }
                */
                UI_TaskTarget_2.SetActive(false);
            }



            //获取目标3
            string TaskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", nowTaskID, "Task_Template");
            if (TaskTarget3 != "0")
            {

                UI_TaskTarget_3.SetActive(true);

                //获取目标值
                string TaskValuePro3 = Game_PublicClassVar.Get_function_Task.TaskReturnValue(nowTaskID, "3");
                string TargetValue3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue3", "ID", nowTaskID, "Task_Template");
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("获得");
                //判定当前任务目标是获得道具还是杀怪（根据ID规则判定）

                switch (TaskTarget1[0].ToString())
                {

                    //道具类
                    case "1":

                        //获取道具名称
                        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", TaskTarget3, "Item_Template");
                        //改变字体颜色
                        if (int.Parse(TaskValuePro3) >= int.Parse(TargetValue3))
                        {
                            UI_TaskTarget_3.GetComponent<Text>().color = TaskColor_Complete;
                            //显示当前获取的道具
                            UI_TaskTarget_3.GetComponent<Text>().text = langStr + itemName + " : " + TaskValuePro3 + " / " + TargetValue3 + CompleteTaskAddStr;

                        }
                        else
                        {
                            UI_TaskTarget_3.GetComponent<Text>().color = TaskColor_NoComplete;
                            //显示当前获取的道具
                            UI_TaskTarget_3.GetComponent<Text>().text = langStr + itemName + " : " + TaskValuePro3 + " / " + TargetValue3;

                        }


                        break;

                    //怪物类（以后添加）


                }
            }
            else
            {
                //判定当前任务类型
                /*
                string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", nowTaskID, "Task_Template");
                if (targetType == "3")
                {
                    UI_TaskTarget_3.SetActive(false);
                }
                */
                UI_TaskTarget_3.SetActive(false);
            }

            //寻人任务单独显示
            string taskTargetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", nowTaskID, "Task_Template");
            if (taskTargetType == "3")
            {
                //获取寻找人的名称
                string completeNpcID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteNpcID", "ID", nowTaskID, "Task_Template");
                string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", completeNpcID, "Npc_Template");
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("寻找");
                UI_TaskTarget_3.GetComponent<Text>().text = langStr + "：" + npcName;
            }


        }
		if (nowTaskID != "") {
			//更新任务奖励
			string exp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskExp", "ID", nowTaskID, "Task_Template");
			string money = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskCoin", "ID", nowTaskID, "Task_Template");
            string taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", nowTaskID, "Task_Template");

            if (taskType == "2")
            {
                string taskSonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskSonType", "ID", nowTaskID, "Task_Template");
                //试炼任务特殊处理
                if (taskSonType == "1")
                {
                    exp = Game_PublicClassVar.Get_function_Task.EveryTaskRewardShow(exp, "2");
                    money = Game_PublicClassVar.Get_function_Task.EveryTaskRewardShow(money, "2");
                }
            }

            //每日任务按系数递增奖励
            if (taskType == "3")
            {
                exp = Game_PublicClassVar.Get_function_Task.EveryTaskRewardShow(exp, "1");
                money = Game_PublicClassVar.Get_function_Task.EveryTaskRewardShow(money, "1");
            }

			if (exp != "0") 
			{
                string langStr = Game_PublicClassVar.gameSettingLanguge.LoadLocalization("经验");
				UI_TaskTrophy_Exp.GetComponent<Text>().text = langStr + "+" +exp;
			}

			if (money != "0")
			{
                string langStr = Game_PublicClassVar.gameSettingLanguge.LoadLocalization("金币");
                UI_TaskTrophy_Money.GetComponent<Text>().text = langStr + "+" + money;
			}

			//更新道具奖励的显示
			string[] taskItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID","ID",nowTaskID,"Task_Template").Split(',');
			string[] taskItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum","ID",nowTaskID,"Task_Template").Split(',');

			//循环创建
			for(int i = 0;i<=taskItemID.Length-1;i++){
				if(taskItemID[i]!=""&&taskItemID[i]!="0"){
					GameObject taskItemObj = (GameObject)Instantiate(Obj_UI_TaskTrophyList);
					taskItemObj.transform.SetParent(UI_ItemTrophySet.transform);
					taskItemObj.transform.localScale = new Vector3(1,1,1);
					taskItemObj.transform.localPosition = new Vector3(i*70,0,0);
					taskItemObj.GetComponent<UI_TaskTrophyList>().ItemID = taskItemID[i];
					taskItemObj.GetComponent<UI_TaskTrophyList>().ItemNum = taskItemNum[i];
				}
			}
		}
    }

	//写入快捷任务
	public void WriteMainUITask(){
		string nowTaskID = Game_PublicClassVar.Get_game_PositionVar.NowTaskID;
        if (nowTaskID != "" && nowTaskID != "0") {
            if (!Game_PublicClassVar.Get_function_Task.WriteMainUITaskID(nowTaskID))
            {
                Debug.Log("最多只能保存3个快捷目标");
            }
            updateTaskData();
        }
	}

	//删除快捷任务
	public void DeleteMainUITask(){
		string nowTaskID = Game_PublicClassVar.Get_game_PositionVar.NowTaskID;
        if (nowTaskID != "" && nowTaskID != "0")
        {
            if (!Game_PublicClassVar.Get_function_Task.DeleteMainUITaskID(nowTaskID))
            {
                Debug.Log("删除快捷任务失败");
            }
            updateTaskData();
        }
	}

	//放弃自身携带的任务
	public void DeleteRoseTask(){


        string nowTaskID = Game_PublicClassVar.Get_game_PositionVar.NowTaskID;

        //判定是否是主线任务
        string taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", nowTaskID, "Task_Template");
        if (taskType == "1") {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("主线任务,无法放弃喔!");
            return;
        }

        if (nowTaskID != "") {
			Game_PublicClassVar.Get_function_Task.DeleteRoseTaskID(nowTaskID);
		}

	}

    //追踪任务
    public void RunTaskTarget() {

        string now_TaskID = Game_PublicClassVar.Get_game_PositionVar.NowTaskID;
        if (now_TaskID != "" && now_TaskID != "0") {
            Game_PublicClassVar.Get_function_Task.Btn_TaskMove(Game_PublicClassVar.Get_game_PositionVar.NowTaskID);
        }

        //关闭界面
        Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseTask);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().RoseTask_Status = false;
    }
}
