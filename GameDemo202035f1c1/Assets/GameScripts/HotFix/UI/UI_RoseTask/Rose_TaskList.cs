using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//此脚本主要绑定在角色身上,当玩家打开任务列表时显示此脚本内容

public class Rose_TaskList : MonoBehaviour {

    //开启可更新任务界面
    public GameObject Obj_Rose_TaskList;
    public bool Rose_TaskList_Update;

    private bool Rose_TaskType_Main;        //主线
    private bool Rose_TaskType_Branch;      //支线
    private bool Rose_TaskType_EveryDay;    //每日任务

    private GameObject UI_RoseTaskType;     //任务日志类型标题控件

    public GameObject Rose_TaskBar;         //任务列表显示下拉进度条
    public Transform UIPoint_TaskType;              //任务列表总绑点
    public Transform UIPoint_TaskType_Main;         //主线任务绑定
    public Transform UIPoint_TaskType_Branch;       //支线任务绑点
    public Transform UIPoint_TaskType_Everyday;     //每日任务绑点

    List<string> task_Main = new List<string>();        //主线任务
    List<string> task_Branch = new List<string>();      //支线任务
    List<string> task_Everyday = new List<string>();    //每日任务

    public GameObject UI_CommonHuoBiSetPosi;

	// Use this for initialization
	void Start () {

        //打开更新任务日志界面开关
        Rose_TaskList_Update = true;

        //获取当前选中任务
        string now_TaskID = Game_PublicClassVar.Get_game_PositionVar.NowTaskID;
        //获取当前选中任务列表
        bool showStatus = false;
        string[] taskStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        for(int i = 0; i<=taskStr.Length-1; i++){
            if (taskStr[i] != "" && taskStr[i]!="0")
            {
                if (taskStr[i] == now_TaskID)
                {
                    showStatus = true;
                }
            }
        }

        if (!showStatus) {
            now_TaskID = taskStr[0];
        }

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_Rose_TaskList);

        //显示通用UI
        Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject,"301");
	}
	
	// Update is called once per frame
	void Update () {

        //初次打开任务日志更新界面
        if (Rose_TaskList_Update) {
            CleanSonGameObject();
            taskUpdate();
            Rose_TaskList_Update = false;
        }

        //判定外界是否需要更新任务日志
        if (Game_PublicClassVar.Get_game_PositionVar.Rose_TaskListUpdata)
        {
            //开启更新任务
            Rose_TaskList_Update = true;
            //关闭外界更新任务开关
            Game_PublicClassVar.Get_game_PositionVar.Rose_TaskListUpdata = false;
        }
	}

    private void taskUpdate() {

        Debug.Log("更新任务显示");
        List<string> taskList = new List<string>();
        taskList = Game_PublicClassVar.Get_function_Task.GetRoseTaskID();

        //获取同类任务数据
        task_Main = Game_PublicClassVar.Get_function_Task.GetRoseTaskTypeID("1");
        task_Branch = Game_PublicClassVar.Get_function_Task.GetRoseTaskTypeID("2");
        task_Everyday = Game_PublicClassVar.Get_function_Task.GetRoseTaskTypeID("3");

        float hight = 0.0f;

        //判定主线任务类型是否有任务
        if (task_Main.Count > 0) {
            hight = loadTaskType("1", task_Main, hight);
        }

        //判定支线任务类型是否有任务
        if (task_Branch.Count > 0) {
            hight = loadTaskType("2", task_Branch, hight);
        }

        //判定每日任务类型是否有任务
        if (task_Everyday.Count > 0)
        {
            hight = loadTaskType("3", task_Everyday, hight);
        }
    }

    //加载任务类型
    private float loadTaskType(string taskType,List<string> taskID,float hight) {

        //实例化界面
        UI_RoseTaskType = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UITaskList_TaskType);

        //获取任务类型是否展开
        string ifShow_Main = Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_1;
        string ifShow_Branch = Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_2;
        string ifShow_Everyday = Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_3;

        //给实例化的脚本指定任务类型
        UI_RoseTaskType.GetComponent<Rose_TaskList_Show>().TaskType = taskType;
        
        //获取当前任务类型对应的文字
        string tasktypeName="";
        switch(taskType){
            
            case "1":
                tasktypeName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("主线任务");
                UI_RoseTaskType.transform.parent = UIPoint_TaskType_Main.transform;
                UI_RoseTaskType.transform.localPosition = new Vector3(0, 0, 0);
                UI_RoseTaskType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                
                //计算列表当前高度
                if (ifShow_Main == "1")
                {
                    hight = -task_Main.Count * 80.0f - 50.0f;
                }
                else {

                    hight = -50.0f;
                }
            break;

            case "2":
                tasktypeName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("支线任务");
                UI_RoseTaskType.transform.parent = UIPoint_TaskType_Branch.transform;
                UI_RoseTaskType.transform.localPosition = new Vector3(0, hight, 0);
                UI_RoseTaskType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                //计算列表当前高度
                if (ifShow_Branch == "1")
                {
                    hight = hight - task_Branch.Count * 80.0f - 50.0f;
                }
                else
                {
                    hight = hight - 50.0f;
                }
            break;

            case "3":
                tasktypeName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("每日任务");
                UI_RoseTaskType.transform.parent = UIPoint_TaskType_Everyday.transform;
                UI_RoseTaskType.transform.localPosition = new Vector3(0, hight, 0);
                UI_RoseTaskType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            break;
        }

        //检测当前角色任务是否展开
        //目前默认展开,没有变量存储
        string ifShowTaskList = "";
        switch (taskType) { 
            case "1":
                ifShowTaskList = Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_1;
                break;
            case "2":
                ifShowTaskList = Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_2;
                break;
            case "3":
                ifShowTaskList = Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_3;
                break;
        }
        //string ifShowTaskList = "1";
        if (ifShowTaskList == "1")
        {

            //更新任务类型名称
            Rose_TaskList_Show taskList_Show = UI_RoseTaskType.GetComponent<Rose_TaskList_Show>();
            taskList_Show.UI_TaskTypeName.GetComponent<Text>().text = tasktypeName;

            //更新图标显示
            taskList_Show.UIImg_TaskListShow.SetActive(true);
            taskList_Show.UIImg_TaskListShow_2.SetActive(false);

            //获取主线任务
            loadTaskData(taskType, taskID);

        }
        else {

            Rose_TaskList_Show taskList_Show = UI_RoseTaskType.GetComponent<Rose_TaskList_Show>();
            taskList_Show.UI_TaskTypeName.GetComponent<Text>().text = tasktypeName;

            //更新图标显示
            taskList_Show.UIImg_TaskListShow.SetActive(false);
            taskList_Show.UIImg_TaskListShow_2.SetActive(true);
        }

        return hight;

    }

    //加载任务类下的子任务
    private void loadTaskData(string taskType, List<string> taskID)
    {

        Rose_TaskList_Show rose_TaskList_Show = UI_RoseTaskType.GetComponent<Rose_TaskList_Show>();
        int TaskNum = 0;

        foreach(string nowTaskID in taskID){
            
            //获取当前任务信息
            string taskName = Game_PublicClassVar.Get_function_Task.TaskIDtoTaskName(nowTaskID);
            string taskLv = Game_PublicClassVar.Get_function_Task.TaskIDtoTaskLv(nowTaskID);

            //实例化任务UI
            TaskNum = TaskNum + 1;
            GameObject UI_RoseTaskListName = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UITaskList_TaskRow);
            UI_RoseTaskListName.transform.parent = rose_TaskList_Show.UIPoint_TaskName;
            UI_RoseTaskListName.transform.localPosition = new Vector3(400, TaskNum * -80.0f, 0);
            UI_RoseTaskListName.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            Rose_TaskList_row_UIPoint uiPoint = UI_RoseTaskListName.GetComponent<Rose_TaskList_row_UIPoint>();
            uiPoint.UI_TaskID = nowTaskID;
            //显示任务名称
            //taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", nowTaskID, "Task_Template");
            string taskSonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskSonType", "ID", nowTaskID, "Task_Template");
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第");
            string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("环");

            //名称特殊处理
            switch (taskType)
            {
                //试炼任务
                case "2":
                    if (taskSonType == "1")
                    {
                        string nowShiLianTaskNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        if (nowShiLianTaskNum == "" || nowShiLianTaskNum == null) {
                            nowShiLianTaskNum = "0";
                        }
                        int shilianNum = int.Parse(nowShiLianTaskNum);
                        shilianNum = shilianNum + 1;
                        
                        taskName = taskName + "("+ langStr_1 + shilianNum + langStr_2 + ")";
                    }
                    break;

                //日常任务
                case "3":

                    string nowShiMenTaskNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMenTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    if (nowShiMenTaskNum == "" || nowShiMenTaskNum == null)
                    {
                        nowShiMenTaskNum = "0";
                    }

                    int shiMenNum = int.Parse(nowShiMenTaskNum);
                    shiMenNum = shiMenNum + 1;
                    taskName = taskName + "(" + langStr_1 + shiMenNum + langStr_2 + ")";
                    break;
            }

            Text textName = uiPoint.UI_TaskName.GetComponent<Text>();
            textName.text = taskName;
            
            //显示任务等级
            Text textLv = uiPoint.UI_TaskLv.GetComponent<Text>();
            string langStr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("级");
            textLv.text = "[" + taskLv + langStr_3 + "]";

            //判定当前任务是否为选中的任务
            if (Game_PublicClassVar.Get_game_PositionVar.NowTaskID == nowTaskID)
            {
                //更改任务名称的颜色及显示选中的任务
                //uiPoint.UI_TaskLv.GetComponent<Text>().color = new Color(1, 1, 1, 1);
                //uiPoint.UI_TaskName.GetComponent<Text>().color = new Color(1, 1, 1, 1);
                uiPoint.UIImg_SelectStatus.SetActive(true);
            }
        }
    }


    //清理子物体
    private void CleanSonGameObject() {

        //清空子物体
        for (int i = 0; i < UIPoint_TaskType_Main.childCount; i++)
        {
            GameObject go = UIPoint_TaskType_Main.GetChild(i).gameObject;
            Destroy(go);
        }

        //清空子物体
        for (int i = 0; i < UIPoint_TaskType_Branch.childCount; i++)
        {
            GameObject go = UIPoint_TaskType_Branch.GetChild(i).gameObject;
            Destroy(go);
        }

        //清空子物体
        for (int i = 0; i < UIPoint_TaskType_Everyday.childCount; i++)
        {
            GameObject go = UIPoint_TaskType_Everyday.GetChild(i).gameObject;
            Destroy(go);
        }

    }
    
    //关闭界面时调用
    public void CloseUI_TaskList() {
        Destroy(this.gameObject);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().RoseTask_Status = false;
    }

    public void MoveUI_TaskList() {

        //移动列表
        //获取当前下拉进度条进度
        float nowBarValue = Rose_TaskBar.GetComponent<Scrollbar>().value;
        if (nowBarValue > 0.0) {
            UIPoint_TaskType.transform.localPosition = new Vector3(UIPoint_TaskType.transform.localPosition.x, 300.0f * nowBarValue, UIPoint_TaskType.transform.localPosition.z);
        }
    }
}
