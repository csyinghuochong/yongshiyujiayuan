using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rose_TaskList_row_UIPoint : MonoBehaviour {

    public GameObject UI_TaskName;
    public GameObject UI_TaskLv;
    public GameObject UI_TaskIfFinish;
    public GameObject UIImg_SelectStatus;

    public string UI_TaskID;

    private bool DragStatus;
    private float BeginDragPosi_Y;
    private float BeginTaskListPosi_Y;
    private float ScrollbarValue;
    private Rose_TaskList roseTaskList;

	// Use this for initialization
	void Start () {

        //判定当前任务目标是否已达成

        bool taskStatus = Game_PublicClassVar.Get_function_Task.TaskComplete(UI_TaskID);

        if (taskStatus)
        {
            UI_TaskIfFinish.SetActive(true);
        }
        else
        {
            UI_TaskIfFinish.SetActive(false);
        }

        roseTaskList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseTask.GetComponent<Rose_TaskList>();

	}
	
	// Update is called once per frame
	void Update () {



	}

    public void UI_SelectTask() {
        Debug.Log("点击状态");
        if (DragStatus) {

            return;
        }
        
        //更改任务名称的颜色及显示选中的任务
        //UI_TaskLv.GetComponent<Text>().color = new Color(1, 1, 1, 1);
        //UI_TaskName.GetComponent<Text>().color = new Color(1, 1, 1, 1);
        UIImg_SelectStatus.SetActive(true);

        //修改当前选中的任务ID
        Game_PublicClassVar.Get_game_PositionVar.NowTaskID = UI_TaskID;
        //Debug.Log("当前任务ID" + UI_TaskID);

        //更新任务日志列表
        Game_PublicClassVar.Get_game_PositionVar.Rose_TaskListUpdata = true;
        //更新任务信息
        Game_PublicClassVar.Get_game_PositionVar.Rose_TaskDataUpdata = true;

        //显示当前UI

        //判定当前任务目标是否已达成
        
        bool taskStatus = Game_PublicClassVar.Get_function_Task.TaskComplete(UI_TaskID);
        if (taskStatus)
        {

            UI_TaskIfFinish.SetActive(true);

        }
        else {

            UI_TaskIfFinish.SetActive(false);
        
        }
            
    }


    //开始拖拽
    public void BeginDragTaskList() {
        
        DragStatus = true;

        BeginDragPosi_Y = Input.mousePosition.y;
        BeginTaskListPosi_Y = roseTaskList.UIPoint_TaskType.transform.localPosition.y;
        ScrollbarValue = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseTask.GetComponent<Rose_TaskList>().Rose_TaskBar.GetComponent<Scrollbar>().value;

    }

    //拖拽中
    public void DragTaskList() {

        float mousePosition_Y = Input.mousePosition.y;
        float chaValue = BeginDragPosi_Y - mousePosition_Y;
        float maskValue = BeginTaskListPosi_Y - chaValue * 2;

        if (maskValue < 17)
        {
            maskValue = 17;
        }
        
                //float nowBarValue = Rose_TaskBar.GetComponent<Scrollbar>().value;
        roseTaskList.UIPoint_TaskType.transform.localPosition = new Vector3(roseTaskList.UIPoint_TaskType.transform.localPosition.x, maskValue, roseTaskList.UIPoint_TaskType.transform.localPosition.z);
            //300.0f * nowBarValue
        //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseTask.GetComponent<Rose_TaskList>().Rose_TaskBar.GetComponent<Scrollbar>().value = barValue;

        Debug.Log("拖拽中:" + "mousePosition_Y = " + mousePosition_Y + ";chaValue = " + chaValue);
    
    }

    //结束拖拽
    public void EndDragTaskList(){
        Debug.Log("结束拖拽");
        DragStatus = false;

    }


}
