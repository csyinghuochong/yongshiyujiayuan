using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameYaoGan : MonoBehaviour {

    private Vector2 starVec2;
    private Vector2 moveVec2;
    private Rose_Status rose_Status;
    private GameObject roseObj;
    public GameObject Obj_YaoGanSeting;
    public GameObject MoveObj;
    public GameObject MoveIconObj;
    public GameObject MoveDiObj;
    //private int chuMoStatusOnce;          //触发是触发一次此状态
    public bool yaoganDragStatus;           //摇杆拖拽状态
    public string yaoganStatus;             //摇杆状态

    // Use this for initialization
    void Start () {

        //设置初始位置
        starVec2 = new Vector2(MoveObj.transform.position.x, MoveObj.transform.position.y);
        //设置初始变量方便后面调用
        rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        roseObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;
        //初始摇杆虚化
        BaseUIStatus();

        //摇杆状态  1：表示开  0:表示关
        yaoganStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YaoGanStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (yaoganStatus == "1" || yaoganStatus == "2")
        {
            this.gameObject.SetActive(true);
            Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = true;
        }
        else
        {
            this.gameObject.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = false;
        }

        //设置摇杆状态
        SettingYaoGanType();
    }
	
	// Update is called once per frame
	void Update () {


    }

    //开始拖拽
    public void StarDragUI()
    {
        MoveObj.transform.localPosition = Vector3.zero;
        starVec2 = new Vector2(MoveObj.transform.position.x, MoveObj.transform.position.y);

        //显示底
        MoveDiObj.SetActive(true);
        //MoveIconObj.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        MoveIconObj.SetActive(false);
        yaoganDragStatus = true;
    }

    //拖拽调用
    public void DragUI()
    {
        AnXia();
        yaoganDragStatus = true;
    }

    public void AnXia() {

        //获取当前目标
        Vector3 movePositionVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        Vector3 moveObjVec3 = MoveObj.transform.position;
        //差值,计算摇杆距离
        float cha_X = moveObjVec3.x - starVec2.x;
        float cha_Y = moveObjVec3.y - starVec2.y;
        float add_X = cha_X / 55;
        float add_Y = cha_Y / 55;
        float move_X = movePositionVec3.x + add_X;
        float move_Y = movePositionVec3.z + add_Y;
        rose_Status.YaoGan_Status = true;
        rose_Status.YaoGanPositionVec3.x = move_X;
        rose_Status.YaoGanPositionVec3.z = move_Y;
        rose_Status.YaoGanPositionVec3.y = roseObj.transform.position.y;

        //发送服务器摇杆单独设定
        if (rose_Status.MovePositionSendStatus)
        {
            float server_move_X = movePositionVec3.x + add_X * 3.0f;
            float server_move_Y = movePositionVec3.z + add_Y * 3.0f;

            rose_Status.YaoGan_Status = true;
            rose_Status.MovePositionVec3_YaoGan.x = server_move_X;
            rose_Status.MovePositionVec3_YaoGan.z = server_move_Y;
            rose_Status.MovePositionVec3_YaoGan.y = roseObj.transform.position.y;
        }

        rose_Status.GetComponent<Rose_Proprety>().Rose_MoveSpeedYaoGan = 1;

        /*
        float speed_x = Math.Abs(cha_X / 110);
        float speed_y = Math.Abs(cha_Y / 110);
        float speed = System.Math.Max(speed_x, speed_y);
        */
    }


    public void Exit()
    {
        rose_Status.YaoGan_Status = false;
        //rose_Status.YaoGanStopMoveTime = 0;
        Vector3 movePositionVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        rose_Status.YaoGanPositionVec3 = movePositionVec3;
        //重置UI状态
        BaseUIStatus();
        this.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(177, 228, 0);
        //设置角色停止移动
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        roseStatus.Move_Target_Position = roseStatus.gameObject.transform.position;
        yaoganDragStatus = false;
    }


    //设置摇杆类型(1.拖拽  2.固定)
    public void SettingYaoGanType() {
        switch (yaoganStatus) {
            //不显示摇杆
            case "0":
                this.gameObject.SetActive(false);
                Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = false;
                break;

            //移动摇杆
            case "1":
                this.gameObject.SetActive(true);
                Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = true;
                Obj_YaoGanSeting.GetComponent<ETCJoystick>().joystickType = ETCJoystick.JoystickType.Dynamic;
                break;

            //固定摇杆
            case "2":
                this.gameObject.SetActive(true);
                Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = true;
                Obj_YaoGanSeting.GetComponent<ETCJoystick>().joystickType = ETCJoystick.JoystickType.Static;
                Obj_YaoGanSeting.GetComponent<ETCJoystick>().visible = true;
                break;
        }

    }




    //摇杆虚化
    void BaseUIStatus()
    {
        MoveObj.transform.localPosition = Vector3.zero;
        MoveIconObj.transform.localPosition = Vector3.zero;
        //MoveDiObj.SetActive(false);
        MoveIconObj.SetActive(true);
        MoveIconObj.GetComponent<Image>().enabled = true;
        MoveIconObj.GetComponent<Image>().color = new Color(1, 1, 1, 0.15f);
        //chuMoStatusOnce = 0;

    }
}
