using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_NpcGetTask : MonoBehaviour {

    public string TaskID;
    public GameObject Obj_TaskDes;
    public GameObject Obj_TaskName;
    public GameObject Obj_TaskReward_Gold;
    public GameObject Obj_TaskReward_Exp;
    public GameObject UI_ItemTrophySet;				//任务道具奖励父级
    public GameObject Obj_UI_TaskTrophyList;		//任务道具奖励显示
    public bool ClickComTaskStatus;

	// Use this for initialization
	void Start () {
        //获取任务名称
        string taskName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskName", "ID", TaskID, "Task_Template");
        Obj_TaskName.GetComponent<Text>().text = taskName;

        //显示任务描述
        string taskDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskDes", "ID", TaskID, "Task_Template");
        Obj_TaskDes.GetComponent<Text>().text =taskDes;

        //获取任务奖励
        string expValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskExp", "ID", TaskID, "Task_Template");
        string moneyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskCoin", "ID", TaskID, "Task_Template");
        string taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", TaskID, "Task_Template");

        //试炼任务系数递增奖励
        if (taskType == "2")
        {
            string taskSonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskSonType", "ID", TaskID, "Task_Template");
            if (taskSonType=="1") {
                expValue = Game_PublicClassVar.Get_function_Task.EveryTaskRewardShow(expValue,"2");
                moneyValue = Game_PublicClassVar.Get_function_Task.EveryTaskRewardShow(moneyValue, "2");
            }
        }

        //每日任务按系数递增奖励
        if (taskType == "3")
        {
            expValue = Game_PublicClassVar.Get_function_Task.EveryTaskRewardShow(expValue, "1");
            moneyValue = Game_PublicClassVar.Get_function_Task.EveryTaskRewardShow(moneyValue, "1");
        }

        Obj_TaskReward_Gold.GetComponent<Text>().text = moneyValue;
        if (expValue != "0")
        {
            Obj_TaskReward_Exp.GetComponent<Text>().text = expValue;
        }
        else {
            Obj_TaskReward_Exp.SetActive(false);
        }
        
        //更新道具奖励的显示
        string[] taskItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", TaskID, "Task_Template").Split(',');
        string[] taskItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", TaskID, "Task_Template").Split(',');

        //循环创建
        if (taskItemID[0] != "0" && taskItemID[0] != "")
        {
            for (int i = 0; i <= taskItemID.Length - 1; i++)
            {
                if (taskItemID[i] != "" && taskItemID[i] != "0")
                {
                    GameObject taskItemObj = (GameObject)Instantiate(Obj_UI_TaskTrophyList);
                    taskItemObj.transform.SetParent(UI_ItemTrophySet.transform);
                    taskItemObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                    taskItemObj.transform.localPosition = new Vector3(i * 70, 0, 0);
                    taskItemObj.GetComponent<UI_TaskTrophyList>().ItemID = taskItemID[i];
                    taskItemObj.GetComponent<UI_TaskTrophyList>().ItemNum = taskItemNum[i];
                }
            }
        }
        else {
            //清空奖励显示
            Game_PublicClassVar.Get_function_UI.DestoryTargetObj(UI_ItemTrophySet);
        }

        //获取当前任务ID是否已经接取

	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void GetTask() {
        //领取任务,领取成功关闭UI
        if (Game_PublicClassVar.Get_function_Task.GetTask(TaskID)) {

            //关闭界面
            Destroy(this.gameObject);   
            Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus = "1";        //更新Npc携带的任务
        }
    }

    public void CompleteTask() {

        if (ClickComTaskStatus) {
            return;
        }

        ClickComTaskStatus = true;

        //判定背包位置是否足够
        string[] rewardItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", TaskID, "Task_Template").Split(',');
        string[] rewardItemValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", TaskID, "Task_Template").Split(',');

        //获取背包剩余格子数量，判定奖励数量是否大于格子数量
        if (Game_PublicClassVar.Get_function_UI.BagSpaceNullNum() < rewardItemID.Length)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_325");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("背包已满,请先清空背包在提交任务！");
            return;       //背包格子不足,领取奖励失败
        }

        //检测当前是否携带任务
        string AchievementTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (AchievementTaskID.Contains(TaskID) == false) {
            Debug.Log("任务不存在");
            Destroy(this.gameObject);
            Destroy(this.gameObject.transform.parent.gameObject);
            return;
        }

        //检测任务是否要求道具
        string targetTypeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", TaskID, "Task_Template");
        if (targetTypeStr == "2") {
            string target1Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", TaskID, "Task_Template");
            string targetValue1Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", TaskID, "Task_Template");
            if (targetValue1Str != "" && targetValue1Str != null) {
                int nowNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(target1Str);
                if (nowNum < int.Parse(targetValue1Str)) {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包内任务要求的道具数量不足,请检查后提交任务!");
                    return;
                }
            }
        }

        //检测任务是否要求宝宝是否存在
        targetTypeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", TaskID, "Task_Template");
        if (targetTypeStr == "7")
        {
            string target1Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", TaskID, "Task_Template");
            string targetValue1Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", TaskID, "Task_Template");
            if (targetValue1Str != "" && targetValue1Str != null)
            {
                int nowNum = Game_PublicClassVar.Get_function_Task.ReturnTaskNeedPetNum(target1Str);
                if (nowNum < int.Parse(targetValue1Str))
                {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("没有对应宠物!请将需要提交的宠物名称修改正确!");
                    return;
                }
            }
        }


        //Debug.Log("CompleteTask321321321");
        //完成任务
        if (Game_PublicClassVar.Get_function_Task.TaskWriteRoseData(TaskID))
        {
            //Debug.Log("CompleteTask123123123");
            //写入完成任务
            if (Game_PublicClassVar.Get_function_Task.TaskRewards(TaskID))
            {
                //Debug.Log("CompleteTask456456");
                Destroy(this.gameObject,0.1f);   //关闭界面(0.1的延迟是关闭时，上一级的界面还没重新生成好,需要加一个小延迟时间遮挡一下)
                Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus = "1";        //更新Npc携带的任务
                                                                                           //关闭界面
            }
        }

        ClickComTaskStatus = false;
    }
}