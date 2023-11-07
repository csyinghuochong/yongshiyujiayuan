using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rose_ChengJiuList_Show : MonoBehaviour {

    public Transform UIPoint_TaskName;
    public GameObject UI_TaskTypeName;
    public string ChengJiuType;                 //0:表示通用  1-3 表示战斗 收集 实力
    public GameObject UITog_TaskListShow;
    public GameObject UIImg_TaskListShow;
    public GameObject UIImg_TaskListShow_2;

    //拖拽相关
    private bool DragStatus;
    private float BeginDragPosi_Y;
    private float BeginTaskListPosi_Y;
    private float ScrollbarValue;
    private Rose_ChengJiuSet roseChengJiuList;

    // Use this for initialization
    void Start () {
        roseChengJiuList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>();
        if (ChengJiuType == "0") {
            //首页不显示箭头
            UIImg_TaskListShow.SetActive(false);
            UIImg_TaskListShow_2.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {

	
	}

    public void ifShowSonTask()
    {
        roseChengJiuList.Obj_ChengJiuShouYeShow.SetActive(false);

        if (ChengJiuType != null) {

            switch (ChengJiuType) {

                //选择通用成就按钮
                case "0":

                    if (Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_0 == "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_0 = "0";
                    }
                    else
                    {
                        Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_0 = "0";
                    }

                    //打开
                    Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_1 = "0";
                    Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_2 = "0";
                    Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_3 = "0";
                    //更新任务日志
                    Game_PublicClassVar.Get_game_PositionVar.Rose_ChengJiuListUpdata = true;

                    //展示成就首页界面
                    roseChengJiuList.Obj_ChengJiuShouYeShow.SetActive(true);
                    roseChengJiuList.Obj_ChengJiuDataSetList.SetActive(false);

                    //Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ChengJiuDataSet);
                    break;

                //选择战斗成就
                case "1":

                    if (Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_1 == "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_1 = "0";
                    }
                    else {
                        Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_1 = "1";
                        roseChengJiuList.ShowFirstObjStatus = true;
                    }

                    //更新任务日志
                    Game_PublicClassVar.Get_game_PositionVar.Rose_ChengJiuListUpdata = true;

                    break;

                //选择探索类成就
                case "2":

                    if (Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_2== "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_2 = "0";
                    }
                    else {
                        Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_2 = "1";
                        roseChengJiuList.ShowFirstObjStatus = true;
                    }

                    //更新任务日志
                    Game_PublicClassVar.Get_game_PositionVar.Rose_ChengJiuListUpdata = true;

                    break;
                //选择实力类类成就
                case "3":

                    if (Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_3 == "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_3 = "0";
                    }
                    else {
                        Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_3 = "1";
                        roseChengJiuList.ShowFirstObjStatus = true;
                    }

                    //更新任务日志
                    Game_PublicClassVar.Get_game_PositionVar.Rose_ChengJiuListUpdata = true;

                    break;
            }
        }
    }

    //开始拖拽
    public void BeginDragTaskList()
    {

        DragStatus = true;

        BeginDragPosi_Y = Input.mousePosition.y;
        BeginTaskListPosi_Y = roseChengJiuList.UIPoint_ChengJiuType.transform.localPosition.y;
        ScrollbarValue = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>().Rose_RoseChengJiuBar.GetComponent<Scrollbar>().value;

    }

    //拖拽中
    public void DragTaskList()
    {

        float mousePosition_Y = Input.mousePosition.y;
        float chaValue = BeginDragPosi_Y - mousePosition_Y;
        float maskValue = BeginTaskListPosi_Y - chaValue * 1;

        if (maskValue < 17)
        {
            maskValue = 17;
        }

        //float nowBarValue = Rose_TaskBar.GetComponent<Scrollbar>().value;
        roseChengJiuList.UIPoint_ChengJiuType.transform.localPosition = new Vector3(roseChengJiuList.UIPoint_ChengJiuType.transform.localPosition.x, maskValue, roseChengJiuList.UIPoint_ChengJiuType.transform.localPosition.z);
        //300.0f * nowBarValue
        //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseTask.GetComponent<Rose_TaskList>().Rose_TaskBar.GetComponent<Scrollbar>().value = barValue;

        Debug.Log("拖拽中:" + "mousePosition_Y = " + mousePosition_Y + ";chaValue = " + chaValue);

    }

    //结束拖拽
    public void EndDragTaskList()
    {
        Debug.Log("结束拖拽");
        DragStatus = false;

    }

}
