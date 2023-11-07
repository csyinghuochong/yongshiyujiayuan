using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class Rose_PaiMaiHangList_row_UIPoint : MonoBehaviour {

    public ObscuredString PaiMaiHangID;

    public GameObject UI_TaskName;
    public GameObject UI_TaskNum;
    public GameObject UI_TaskIfFinish;
    public GameObject UIImg_SelectStatus;

    public ObscuredString UI_TaskID;

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

		if (rosePaiMaiHangList.PaiMaiHangXuanZhongStatus) {

			if (rosePaiMaiHangList.PaiMaiHangXuanZhongObj != this.gameObject) {
                UIImg_SelectStatus.SetActive(false);
            }
        }
	}


    //显示当前成就列表
    public void UpdatePaiMaiHangListShow() {

		//显示首杀信息
		//Debug.Log("显示首杀信息！！！");
		if (rosePaiMaiHangList == null) {
			rosePaiMaiHangList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>();
		}
		//rosePaiMaiHangList.ShowPaiMaiHangUI();
		return;
		/*
        rosePaiMaiHangList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<Rose_PaiMaiHangSet>();
        string chengJiuStr = Game_PublicClassVar.Get_function_Task.PaiMaiHang_GetTargetZiDuanPaiMaiHangID(PaiMaiHangTypeID, PaiMaiHangZiDuan);
        string chengJiuStr_YiWanCheng = Game_PublicClassVar.Get_function_Task.PaiMaiHang_GetTargetZiDuanPaiMaiHangID_YiWanCheng(PaiMaiHangTypeID, PaiMaiHangZiDuan);
        Debug.Log("chengJiuStr ==" + chengJiuStr + ";chengJiuStr_YiWanCheng = " + chengJiuStr_YiWanCheng);
		//显示成就列表
        rosePaiMaiHangList.ShowPaiMaiHangList(chengJiuStr, chengJiuStr_YiWanCheng);

        //设置列表长度
        int leng = chengJiuStr.Split(';').Length + chengJiuStr_YiWanCheng.Split(';').Length;
        float LengValue = 500 + leng * 100;
        rosePaiMaiHangList.Obj_PaiMaiHangDataSet.GetComponent<RectTransform>().sizeDelta = new Vector2(200, LengValue);
        rosePaiMaiHangList.Obj_PaiMaiHangDataSet.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -1* LengValue/2, 0);
		*/
    }


    public void UI_SelectTask() {
        //Debug.Log("点击状态");

        //请求拍卖行的商品信息
        Pro_ComStr_2 com_str_2 = new Pro_ComStr_2();
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        com_str_2.str_1 = zhanghaoID;
        com_str_2.str_2 = PaiMaiHangID;
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001081, com_str_2);

        UpdatePaiMaiHangListShow();
        //更改任务名称的颜色及显示选中的任务
        UIImg_SelectStatus.SetActive(true);

        //修改当前选中的任务ID
		if(rosePaiMaiHangList==null){
			rosePaiMaiHangList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>();
		}
		rosePaiMaiHangList.PaiMaiHangXuanZhongObj = this.gameObject;
		rosePaiMaiHangList.PaiMaiHangXuanZhongStatus = true;
    }


    //开始拖拽
    public void BeginDragTaskList() {
        
        DragStatus = true;

        BeginDragPosi_Y = Input.mousePosition.y;
        BeginTaskListPosi_Y = rosePaiMaiHangList.UIPoint_PaiMaiHangType.transform.localPosition.y;
		ScrollbarValue = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>().Obj_PaiMaiHangTypePro.GetComponent<Scrollbar>().value;

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
		if(rosePaiMaiHangList==null){
			rosePaiMaiHangList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>();
		}

		rosePaiMaiHangList.UIPoint_PaiMaiHangType.transform.localPosition = new Vector3(rosePaiMaiHangList.UIPoint_PaiMaiHangType.transform.localPosition.x, maskValue, rosePaiMaiHangList.UIPoint_PaiMaiHangType.transform.localPosition.z);
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
