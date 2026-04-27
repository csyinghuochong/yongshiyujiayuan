using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Rose_PaiMaiHangList_Show : MonoBehaviour {

    public Transform UIPoint_TaskName;
    public GameObject UI_TaskTypeName;
    public string PaiMaiHangType;                 //0:表示通用  1-3 表示战斗 收集 实力
    public GameObject UITog_TaskListShow;
    public GameObject UIImg_TaskListShow;
    public GameObject UIImg_TaskListShow_2;

    //拖拽相关
    private bool DragStatus;
    private float BeginDragPosi_Y;
    private float BeginTaskListPosi_Y;
    private float ScrollbarValue;
    private UI_PaiMaiHangSet rosePaiMaiHangList;

    // Use this for initialization
    void Start () {
        rosePaiMaiHangList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>();
    }
	
	// Update is called once per frame
	void Update () {

	
	}

    public void ifShowSonTask()
    {
		//Debug.Log ("我惦记了ifShowSonTaskifShowSonTaskifShowSonTask");
        if (PaiMaiHangType != null) {

            switch (PaiMaiHangType) {

                //第一章BOSS
                case "0":

				if (rosePaiMaiHangList.OpenPaiMaiHangZhangJie_1)
                    {
						rosePaiMaiHangList.OpenPaiMaiHangZhangJie_1 = false;
                    }
                    else
                    {
						clearnShowType ();
						rosePaiMaiHangList.OpenPaiMaiHangZhangJie_1 = true;
						rosePaiMaiHangList.ShowFirstObjStatus = true;
                    }
					
					//更新首杀列表
					rosePaiMaiHangList.Rose_PaiMaiHangList_Update = true;


                    break;

				//第二章BOSS
                case "1":

				if (rosePaiMaiHangList.OpenPaiMaiHangZhangJie_2)
                    {
						rosePaiMaiHangList.OpenPaiMaiHangZhangJie_2 = false;
                    }
                    else {
					clearnShowType ();
						rosePaiMaiHangList.OpenPaiMaiHangZhangJie_2 = true;
                        rosePaiMaiHangList.ShowFirstObjStatus = true;
                    }

					//更新首杀列表
					rosePaiMaiHangList.Rose_PaiMaiHangList_Update = true;

                    break;

				//第三章BOSS
                case "2":

				if (rosePaiMaiHangList.OpenPaiMaiHangZhangJie_3)
                    {
						rosePaiMaiHangList.OpenPaiMaiHangZhangJie_3 = false;
                    }
                    else {
					clearnShowType ();
						rosePaiMaiHangList.OpenPaiMaiHangZhangJie_3 = true;
                        rosePaiMaiHangList.ShowFirstObjStatus = true;
                    }

					//更新首杀列表
					rosePaiMaiHangList.Rose_PaiMaiHangList_Update = true;

                    break;
				//第四章BOSS
                case "3":

				if (rosePaiMaiHangList.OpenPaiMaiHangZhangJie_4)
                    {
						rosePaiMaiHangList.OpenPaiMaiHangZhangJie_4 = false;
                    }
                    else {
					clearnShowType ();
						rosePaiMaiHangList.OpenPaiMaiHangZhangJie_4 = true;
                        rosePaiMaiHangList.ShowFirstObjStatus = true;
                    }

					//更新首杀列表
					rosePaiMaiHangList.Rose_PaiMaiHangList_Update = true;

                    break;

				//第五章BOSS
				case "4":

				if (rosePaiMaiHangList.OpenPaiMaiHangZhangJie_5)
					{
						rosePaiMaiHangList.OpenPaiMaiHangZhangJie_5 = false;
					}
					else {
					clearnShowType ();
						rosePaiMaiHangList.OpenPaiMaiHangZhangJie_5 = true;
						rosePaiMaiHangList.ShowFirstObjStatus = true;
					}

					//更新首杀列表
					rosePaiMaiHangList.Rose_PaiMaiHangList_Update = true;
					
					break;
            }
        }
    }

	public void clearnShowType(){

		rosePaiMaiHangList.OpenPaiMaiHangZhangJie_1 = false;
		rosePaiMaiHangList.OpenPaiMaiHangZhangJie_2 = false;
		rosePaiMaiHangList.OpenPaiMaiHangZhangJie_3 = false;
		rosePaiMaiHangList.OpenPaiMaiHangZhangJie_4 = false;
		rosePaiMaiHangList.OpenPaiMaiHangZhangJie_5 = false;

	}

    //开始拖拽
    public void BeginDragTaskList()
    {
        DragStatus = true;

        BeginDragPosi_Y = Input.mousePosition.y;
        BeginTaskListPosi_Y = rosePaiMaiHangList.UIPoint_PaiMaiHangType.transform.localPosition.y;
		ScrollbarValue = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>().Obj_PaiMaiHangTypePro.GetComponent<Scrollbar>().value;

    }

    //拖拽中
    public void DragTaskList()
    {

        float mousePosition_Y = Input.mousePosition.y;
        float chaValue = BeginDragPosi_Y - mousePosition_Y;
        float maskValue = BeginTaskListPosi_Y - chaValue;

        if (maskValue < 17)
        {
            maskValue = 17;
        }

        //float nowBarValue = Rose_TaskBar.GetComponent<Scrollbar>().value;
		rosePaiMaiHangList.UIPoint_PaiMaiHangType.transform.localPosition = new Vector3(rosePaiMaiHangList.UIPoint_PaiMaiHangType.transform.localPosition.x, maskValue, rosePaiMaiHangList.UIPoint_PaiMaiHangType.transform.localPosition.z);
        //300.0f * nowBarValue
        //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseTask.GetComponent<Rose_TaskList>().Rose_TaskBar.GetComponent<Scrollbar>().value = barValue;

        //Debug.Log("拖拽中:" + "mousePosition_Y = " + mousePosition_Y + ";chaValue = " + chaValue);

    }

    //结束拖拽
    public void EndDragTaskList()
    {
        //Debug.Log("结束拖拽");
        DragStatus = false;

    }

}
