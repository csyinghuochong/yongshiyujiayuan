using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_YaoGan : MonoBehaviour {

    private Vector2 starVec2;
    private Vector2 moveVec2;
    private Rose_Status rose_Status;
    private GameObject roseObj;
    public GameObject MoveObj;
    public GameObject MoveObj_JianCe;
    public GameObject MoveIconObj;
    public GameObject MoveDiObj;
    private bool moveStatus;
    private int chuMoNum;
    private int chuMoStatusOnce;        //触发是触发一次此状态
    private int yaoganChuMoID;          //摇杆自己的触摸ID
    private Rect rect;
    private bool chuMoStatus;
    private bool yaoganDragStatus;      //摇杆拖拽状态
    private string yaoganStatus;
    // Use this for initialization
    void Start () {
        //MoveObj = this.transform.Find("Move").gameObject;
        //Debug.Log("调用了摇杆开始");
        //starVec2 = new Vector2(MoveObj.transform.localPosition.x, MoveObj.transform.localPosition.y);
        starVec2 = new Vector2(MoveObj.transform.position.x, MoveObj.transform.position.y);
        //Debug.Log("starVec2 = " + starVec2);
        rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        roseObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;

        BaseUIStatus();
        
        float x = Game_PublicClassVar.Get_function_UI.ReturnScreen_X(180);
        float y = Game_PublicClassVar.Get_function_UI.ReturnScreen_Y(212);
        float w = Game_PublicClassVar.Get_function_UI.ReturnScreen_X(450);
        float h = Game_PublicClassVar.Get_function_UI.ReturnScreen_Y(450);

        rect = new Rect(x,y,w,h);
        //摇杆状态  1：表示开  0:表示关
        string yaoganStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YaoGanStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
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

        //开始设置摇杆位置
        yaoganStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YaoGanStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("当前触摸ID  chuMoNum = " + chuMoNum + "触摸总数：" + Input.touchCount + " yaoganChuMoID = " + yaoganChuMoID  + " yaoganDragStatus = " + yaoganDragStatus);
        
        //Debug.Log("调用了摇杆_111");
        if (moveStatus) {
            //Debug.Log("调用了摇杆_222");
            AnXia();
        }
        //Debug.Log("调用了摇杆_333");
        int inputNum = Input.touchCount;
        //Debug.Log("调用了摇杆_444");
        if (inputNum >= 2) {
            //Debug.Log("调用了摇杆_555");
            for (int i = 0; i <= inputNum - 1; i++)
            {
                //Debug.Log("调用了摇杆_666");
                //检测触碰区域在右边平且触碰点大于等于2时
                if (Input.GetTouch(i).position.x < Screen.width / 2 && inputNum >= 2)
                {
                    //Debug.Log("调用了摇杆_777");
                    if (rect.Contains(Input.GetTouch(i).position))
                    {
                        //Debug.Log("调用了摇杆_888");
                        chuMoNum = i;
                        //Debug.Log("调用了摇杆_999 chuMoNum = " + chuMoNum);
                        chuMoStatus = true;
                        //Debug.Log("调用了摇杆_101010");
                        DragUI();
                        //Debug.Log("调用了摇杆_111111");
                    }
                }
            }
        }

        //Debug.Log("调用了摇杆_121212");
        //开启触摸状态
        if (chuMoStatus) {
            //Debug.Log("调用了摇杆_131313");
            if (Input.touchCount <= 1) {
                //Debug.Log("调用了摇杆_141414");
                chuMoNum = 0;
            }
            //Debug.Log("调用了摇杆_151515");
                DragUI();

            //Debug.Log("调用了摇杆_161616");
        }

        //Debug.Log("调用了摇杆_171717");
        //停止移动退出
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //Debug.Log("调用了摇杆_181818");
        }
        else {
            //Debug.Log("调用了摇杆_191919");
            //触摸状态
            if (chuMoStatus) {
                //触摸停止时调用
                if (Input.GetTouch(yaoganChuMoID).phase == TouchPhase.Ended)
                {
                    //Debug.Log("清空摇杆数据！！！");
                    Exit();
                    //Debug.Log("调用了摇杆_212121");
                }
            }
        }

        //Debug.Log("2222当前触摸IDchuMoNum = " + chuMoNum + "触摸总数：" + Input.touchCount);
    }


    public void Exit()
    {
        //Debug.Log("我离开了摇杆");
        moveStatus = false;
        chuMoStatus = false;
        rose_Status.YaoGan_Status = false;
        rose_Status.YaoGanStopMoveTime = 0;
        Vector3 movePositionVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        rose_Status.YaoGanPositionVec3 = movePositionVec3;
        //重置UI状态
        BaseUIStatus();
        //this.gameObject.transform.localPosition = new Vector3(210,260,0);
        this.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(177, 228, 0);
        //设置角色停止移动
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        roseStatus.Move_Target_Position = roseStatus.gameObject.transform.position;
        //roseStatus.ai_nav.SetDestination(roseStatus.gameObject.transform.position);
        //rose_Status.Move_Target_Status = false;
        yaoganDragStatus = false;
        yaoganChuMoID = 0;
    }

    public void AnXia()
    {

        //获取当前是否为死亡状态
        //if(Game_PublicClassVar.)
        //死亡状态不能触发按钮
        if (rose_Status != null)
        {
            if (rose_Status.RoseDeathStatus)
            {
                return;
            }
        }

        //moveVec2 = new Vector2(MoveObj.transform.localPosition.x, MoveObj.transform.localPosition.y);
        //moveVec2 = Input.mousePosition;

		if (Application.platform == RuntimePlatform.WindowsEditor||Application.platform == RuntimePlatform.OSXEditor)
        {
            moveVec2 = Input.mousePosition;
        }
        else
        {
            moveVec2 = Input.GetTouch(yaoganChuMoID).position;
        }

        //Debug.Log("moveVec2 = " + moveVec2);
        //计算摇杆角度
        /*
        float yaoganjiaodu = Vector2.Angle(new Vector2(0, 1), moveVec2);

        if (moveVec2.x < 0) {
            yaoganjiaodu = 180 + 180 - yaoganjiaodu;
        }
        rose_Status.YaoGanJiaoDu = yaoganjiaodu;        //设置摇杆角度
        */

        //触发移动
        //获取当前目标
        Vector3 movePositionVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        Vector3 moveObjVec3 = MoveObj.transform.position;
        //差值
        float cha_X = moveObjVec3.x - starVec2.x;
        float cha_Y = moveObjVec3.y - starVec2.y;

        /*
        if (cha_X >= 1&& cha_X <= 40)
        {
            cha_X = cha_X * 5;
        }
        if (cha_X <= -1 && cha_X >= -40)
        {
            cha_X = cha_X * 5;
        }
        if (cha_Y >= 1&& cha_Y <= 40)
        {
            cha_Y = cha_Y * 5;
        }
        if (cha_Y <= -1&& cha_Y >= -40)
        {
            cha_Y = cha_Y * 5;
        }
        */
        /*
        if (cha_X >= 110) {
            cha_X = 110;
        }
        if (cha_X <= -110) {
            cha_X = -110;
        }
        if (cha_Y >= 110)
        {
            cha_Y = 110;
        }
        if (cha_Y <= -110) {
            cha_Y = -110;
        }
        */

        float add_X = cha_X / 55;
        float add_Y = cha_Y / 55;

        //摇杆坐标修正
        if (Math.Abs(add_X) >= Math.Abs(add_Y))
        {
            add_Y = add_Y / Math.Abs(add_X);

            if (add_X >= 0)
            {
                add_X = 1;
            }
            else
            {
                add_X = -1;
            }
        }
        else {
            add_X = add_X / Math.Abs(add_Y);

            if (add_Y >= 0)
            {
                add_Y = 1;
            }
            else
            {
                add_Y = -1;
            }
        }

        //Debug.Log("add_X = " + add_X + ";add_Y = " + add_Y);

        float move_X = movePositionVec3.x + add_X;
        float move_Y = movePositionVec3.z + add_Y;

        float speed_x = Math.Abs(cha_X / 110);
        float speed_y = Math.Abs(cha_Y / 110);
        float speed = System.Math.Max(speed_x, speed_y);
        
        rose_Status.YaoGan_Status = true;
        rose_Status.YaoGanPositionVec3.x = move_X;
        rose_Status.YaoGanPositionVec3.z = move_Y;
        rose_Status.YaoGanPositionVec3.y = roseObj.transform.position.y;

        //发送服务器摇杆单独设定
        if (rose_Status.MovePositionSendStatus) {

            float server_move_X = movePositionVec3.x + add_X * 3.0f;
            float server_move_Y = movePositionVec3.z + add_Y * 3.0f;

            rose_Status.YaoGan_Status = true;
            rose_Status.MovePositionVec3_YaoGan.x = server_move_X;
            rose_Status.MovePositionVec3_YaoGan.z = server_move_Y;
            rose_Status.MovePositionVec3_YaoGan.y = roseObj.transform.position.y;

        }

        speed = speed * 3;

        if (speed >= 1)
        {
            speed = 1;
        }

        speed = 1;
        rose_Status.GetComponent<Rose_Proprety>().Rose_MoveSpeedYaoGan = speed;

        //创建一个特效,测试用
        /*
        GameObject moveEffect = (GameObject)Instantiate(rose_Status.Obj_RoseMoveEffect);
        moveEffect.transform.localScale = new Vector3(1, 1, 1);
        moveEffect.transform.position = rose_Status.YaoGanPositionVec3;
        */

    }

    //拖拽调用
    public void DragUI() {

        //Debug.Log("Input.touchCount = " + Input.touchCount);
        /*
        if (Input.GetTouch(chuMoNum).position.x >= Screen.width / 2)
        {
            //Game_PublicClassVar.Get_function_UI.GameHint("超出屏幕,结束拖拽！ " + Input.GetTouch(chuMoNum).position.x);
            Exit();
            return;
        }
        */
        //开启移动状态
        /*
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            MoveObj_JianCe.transform.position = Input.mousePosition;
        }
        else {
            MoveObj_JianCe.transform.position = Input.GetTouch(chuMoNum).position;
        }
        */


        //移动摇杆（防止2次点击拖拽显示）
        /*
        if (yaoganStatus != "2")
        {
            //显示摇杆
            if (!yaoganDragStatus) {
            Debug.Log("yaoganDragStatusyaoganDragStatus");
            return;
            }
        }
        */
        /*
        if (rose_Status.YaoGanStopMoveTime > 0) {
            return;
        }
        */

        int inputNum = Input.touchCount;
        //Debug.Log("触摸调用了摇杆_444");
        if (inputNum >= 2)
        {
            if (!yaoganDragStatus)
            {
                //Debug.Log("触摸调用了摇杆_555");
                for (int i = 0; i <= inputNum - 1; i++)
                {
                    //Debug.Log("触摸调用了摇杆_666");
                    //检测触碰区域在右边平且触碰点大于等于2时
                    if (Input.GetTouch(i).position.x < Screen.width / 2 && inputNum >= 2)
                    {
                        //Debug.Log("触摸调用了摇杆_777");
                        if (rect.Contains(Input.GetTouch(i).position))
                        {
                            //Debug.Log("点击到了指定区域！1111");
                            chuMoNum = i;
                            //Debug.Log("触摸调用了摇杆触屏 chuMoNum = " + chuMoNum);
                        }
                    }
                }
            }
            else {
                chuMoNum = yaoganChuMoID;
            }
        }
        else {
            //Debug.Log("1个触摸调用了摇杆_1111");
            if (inputNum == 1)
            {
                //Debug.Log("1个触摸调用了摇杆_22222");
                if (Input.GetTouch(0).position.x < Screen.width / 2 && inputNum >= 2)
                {
                    //Debug.Log("1个触摸调用了摇杆_3333");
                    if (rect.Contains(Input.GetTouch(0).position))
                    {
                        //Debug.Log("点击到了指定区域！2222");
                        chuMoNum = 0;
                    }
                }
                chuMoNum = 0;
            }
        }

        yaoganChuMoID = chuMoNum;

        moveStatus = true;
        //获取拖拽到的坐标点
        Vector2 dragVec2 = new Vector2(MoveObj_JianCe.transform.localPosition.x, MoveObj_JianCe.transform.localPosition.y);
        //获取到原点的距离
        float dis = Vector2.Distance(moveVec2, starVec2);
        //监测是否超出移动距离
        if (dis >= 110)
        {
            //获取两个坐标的角度
            //float jiaodu = Vector2.Angle(starVec2, moveVec2);
            Vector2 aaa = Vector2.Lerp(starVec2, moveVec2, 110 / dis);
            MoveObj.transform.position = new Vector3(aaa.x, aaa.y, MoveObj.transform.position.z);
        }
        else {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                MoveObj.transform.position = Input.mousePosition;
            }
            else
            {
                MoveObj.transform.position = Input.GetTouch(yaoganChuMoID).position;
            }
        }


        MoveDiObj.SetActive(true);
        MoveIconObj.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    //开始拖拽
    public void StarDragUI() {

        //Debug.Log("开始拖拽 Input.touchCount = " + Input.touchCount);
        chuMoNum = Input.touchCount - 1;
        int inputNum = Input.touchCount;
        //Debug.Log("开始拖拽 chuMoNum = " + chuMoNum);

        //如果多个手指同时按住,那个在屏幕左边那个就是摇杆的触碰ID
        /*
        if (inputNum >= 2)
        {
            //Debug.Log("调用了摇杆_555");
            for (int i = 0; i <= inputNum - 1; i++)
            {
                //Debug.Log("调用了摇杆_666");
                //检测触碰区域在右边平且触碰点大于等于2时
                if (Input.GetTouch(i).position.x < Screen.width / 2 && inputNum >= 2)
                {
                    //Debug.Log("调用了摇杆_777");
                    //if (rect.Contains(Input.GetTouch(i).position))
                    //{
                        //Debug.Log("调用了摇杆_888");
                        chuMoNum = i;
                    //}
                }
            }
        }
        */



        //Debug.Log("开始拖拽22222 chuMoNum = " + chuMoNum);
        //开始设置摇杆位置
        yaoganStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YaoGanStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (yaoganStatus != "2") {
            if (yaoganDragStatus == false) {
                yaoganChuMoID = chuMoNum;
                //this.gameObject.transform.position = Input.mousePosition;
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    this.gameObject.transform.position = Input.mousePosition;
                }
                else {
                    this.gameObject.transform.position = Input.GetTouch(yaoganChuMoID).position;
                }

            }
        }
        //Debug.Log("触摸触摸触摸chuMoNum = " + chuMoNum);
        yaoganDragStatus = true;
        MoveObj.transform.localPosition = Vector3.zero;
        starVec2 = new Vector2(MoveObj.transform.position.x, MoveObj.transform.position.y);

        if (rose_Status.RoseStatus != "3")
        {
            //rose_Status.YaoGanStopMoveTime = 0;
        }
        else {
            //rose_Status.YaoGanStopMoveTime = 0.2f;
            //if(rose_Status.roseAnimator.GetCurrentAnimatorStateInfo["Act"].NOW)
        }

    }

    void BaseUIStatus() {

        MoveObj.transform.localPosition = Vector3.zero;
        MoveDiObj.SetActive(false);
        MoveIconObj.GetComponent<Image>().color = new Color(1, 1, 1, 0.15f);
        chuMoStatusOnce = 0;

    }

    //手指按下,获取当前按下位置将摇杆防止在此处
    public void MouseAnXia() {

        /*
        if (Input.GetTouch(chuMoNum).position.x >= Screen.width / 2)
        {
            Game_PublicClassVar.Get_function_UI.GameHint("超出屏幕,结束拖拽！ " + Input.GetTouch(chuMoNum).position.x);
            Exit();
            return;
        }
        */
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            this.gameObject.transform.position = Input.mousePosition;
        }
        else
        {
            this.gameObject.transform.position = Input.GetTouch(chuMoNum).position;
        }

        //this.gameObject.transform.position = Input.mousePosition;
        //AnXia();
        StarDragUI();
        DragUI();
    }

    //移送时调用一次
    /*
    void moveUIStatus() {
        if (chuMoStatusOnce == 1)
        {
            chuMoStatusOnce = 2;
            //获取当前触摸数量
            chuMoNum = Input.touchCount-1;
            Debug.Log("触摸触摸触摸chuMoNum = " + chuMoNum);
        }
    }
    */
}
