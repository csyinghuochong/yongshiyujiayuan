using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rose_ChengJiuList_row_UIPoint : MonoBehaviour {

    public string ChengJiuTypeID;
    public string ChengJiuZiDuan;

    public GameObject UI_TaskName;
    public GameObject UI_TaskNum;
    public GameObject UI_TaskIfFinish;
    public GameObject UIImg_SelectStatus;

    public string UI_TaskID;

    private bool DragStatus;
    private float BeginDragPosi_Y;
    private float BeginTaskListPosi_Y;
    private float ScrollbarValue;
    private Rose_ChengJiuSet roseChengJiuList;

	// Use this for initialization
	void Start () {

        //

        

	}
	
	// Update is called once per frame
	void Update () {

        if (Game_PublicClassVar.Get_game_PositionVar.Rose_ChengJiuXuanZhongStatus) {

            if (Game_PublicClassVar.Get_game_PositionVar.Obj_ChengJiuXuanZhong != this.gameObject) {
                UIImg_SelectStatus.SetActive(false);
            }
        }
	}


    //显示当前成就列表
    public void UpdateChengJiuListShow() {

        roseChengJiuList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>();
        string chengJiuStr = Game_PublicClassVar.Get_function_Task.ChengJiu_GetTargetZiDuanChengJiuID(ChengJiuTypeID, ChengJiuZiDuan);
        string chengJiuStr_YiWanCheng = Game_PublicClassVar.Get_function_Task.ChengJiu_GetTargetZiDuanChengJiuID_YiWanCheng(ChengJiuTypeID, ChengJiuZiDuan);
        //Debug.Log("chengJiuStr ==" + chengJiuStr + ";chengJiuStr_YiWanCheng = " + chengJiuStr_YiWanCheng);
		//显示成就列表
        roseChengJiuList.ShowChengJiuList(chengJiuStr, chengJiuStr_YiWanCheng);

        //设置列表长度
        int leng = chengJiuStr.Split(';').Length + chengJiuStr_YiWanCheng.Split(';').Length;
        float LengValue = 500 + leng * 140;
        roseChengJiuList.Obj_ChengJiuDataSet.GetComponent<RectTransform>().sizeDelta = new Vector2(200, LengValue);
        roseChengJiuList.Obj_ChengJiuDataSet.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -1* LengValue/2, 0);

    }


    public void UI_SelectTask() {
        //Debug.Log("点击状态");
        if (DragStatus) {

            //return;
        }
        UpdateChengJiuListShow();
        //更改任务名称的颜色及显示选中的任务
        UIImg_SelectStatus.SetActive(true);

        //修改当前选中的任务ID
        Game_PublicClassVar.Get_game_PositionVar.Obj_ChengJiuXuanZhong = this.gameObject;
        Game_PublicClassVar.Get_game_PositionVar.Rose_ChengJiuXuanZhongStatus = true;

        //Debug.Log("当前任务ID" + UI_TaskID);

        //更新任务日志列表
        //Game_PublicClassVar.Get_game_PositionVar.Rose_TaskListUpdata = true;
        //更新任务信息
        //Game_PublicClassVar.Get_game_PositionVar.Rose_TaskDataUpdata = true;

    }


    //开始拖拽
    public void BeginDragTaskList() {
        
        DragStatus = true;

        BeginDragPosi_Y = Input.mousePosition.y;
        if (roseChengJiuList != null)
        {
            BeginTaskListPosi_Y = roseChengJiuList.UIPoint_ChengJiuType.transform.localPosition.y;
        }
        ScrollbarValue = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>().Rose_RoseChengJiuBar.GetComponent<Scrollbar>().value;
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
		if(roseChengJiuList==null){
			roseChengJiuList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>();
		}

        roseChengJiuList.UIPoint_ChengJiuType.transform.localPosition = new Vector3(roseChengJiuList.UIPoint_ChengJiuType.transform.localPosition.x, maskValue, roseChengJiuList.UIPoint_ChengJiuType.transform.localPosition.z);
        //300.0f * nowBarValue
        //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseTask.GetComponent<Rose_TaskList>().Rose_TaskBar.GetComponent<Scrollbar>().value = barValue;

        //Debug.Log("拖拽中:" + "mousePosition_Y = " + mousePosition_Y + ";chaValue = " + chaValue);
    
    }

    //结束拖拽
    public void EndDragTaskList(){
        //Debug.Log("结束拖拽");
        DragStatus = false;

    }


}
