using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rose_ShouShaList_Show : MonoBehaviour {

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
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void ifShowSonTask()
    {
		Debug.Log ("我惦记了ifShowSonTaskifShowSonTaskifShowSonTask");
        if (ChengJiuType != null) {

            switch (ChengJiuType) {

                //第一章BOSS
                case "0":

				if (roseChengJiuList.OpenShouShaZhangJie_1)
                    {
						roseChengJiuList.OpenShouShaZhangJie_1 = false;
                    }
                    else
                    {
						clearnShowType ();
						roseChengJiuList.OpenShouShaZhangJie_1 = true;
						roseChengJiuList.ShowFirstObjStatus = true;
                    }
					
					//更新首杀列表
					roseChengJiuList.Rose_ShouShaList_Update = true;


                    break;

				//第二章BOSS
                case "1":

				if (roseChengJiuList.OpenShouShaZhangJie_2)
                    {
						roseChengJiuList.OpenShouShaZhangJie_2 = false;
                    }
                    else {
					clearnShowType ();
						roseChengJiuList.OpenShouShaZhangJie_2 = true;
                        roseChengJiuList.ShowFirstObjStatus = true;
                    }

					//更新首杀列表
					roseChengJiuList.Rose_ShouShaList_Update = true;

                    break;

				//第三章BOSS
                case "2":

				if (roseChengJiuList.OpenShouShaZhangJie_3)
                    {
						roseChengJiuList.OpenShouShaZhangJie_3 = false;
                    }
                    else {
					clearnShowType ();
						roseChengJiuList.OpenShouShaZhangJie_3 = true;
                        roseChengJiuList.ShowFirstObjStatus = true;
                    }

					//更新首杀列表
					roseChengJiuList.Rose_ShouShaList_Update = true;

                    break;
				//第四章BOSS
                case "3":

				if (roseChengJiuList.OpenShouShaZhangJie_4)
                    {
						roseChengJiuList.OpenShouShaZhangJie_4 = false;
                    }
                    else {
					clearnShowType ();
						roseChengJiuList.OpenShouShaZhangJie_4 = true;
                        roseChengJiuList.ShowFirstObjStatus = true;
                    }

					//更新首杀列表
					roseChengJiuList.Rose_ShouShaList_Update = true;

                    break;

				//第五章BOSS
				case "4":

				if (roseChengJiuList.OpenShouShaZhangJie_5)
					{
						roseChengJiuList.OpenShouShaZhangJie_5 = false;
					}
					else {
					clearnShowType ();
						roseChengJiuList.OpenShouShaZhangJie_5 = true;
						roseChengJiuList.ShowFirstObjStatus = true;
					}

					//更新首杀列表
					roseChengJiuList.Rose_ShouShaList_Update = true;
					
					break;
            }
        }
    }

	public void clearnShowType(){

		roseChengJiuList.OpenShouShaZhangJie_1 = false;
		roseChengJiuList.OpenShouShaZhangJie_2 = false;
		roseChengJiuList.OpenShouShaZhangJie_3 = false;
		roseChengJiuList.OpenShouShaZhangJie_4 = false;
		roseChengJiuList.OpenShouShaZhangJie_5 = false;

	}

    //开始拖拽
    public void BeginDragTaskList()
    {

        DragStatus = true;

        BeginDragPosi_Y = Input.mousePosition.y;
        BeginTaskListPosi_Y = roseChengJiuList.UIPoint_ChengJiuType.transform.localPosition.y;
		ScrollbarValue = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>().Obj_ShouShaTypePro.GetComponent<Scrollbar>().value;

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
		roseChengJiuList.UIPoint_ShouShaType.transform.localPosition = new Vector3(roseChengJiuList.UIPoint_ShouShaType.transform.localPosition.x, maskValue, roseChengJiuList.UIPoint_ShouShaType.transform.localPosition.z);
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
