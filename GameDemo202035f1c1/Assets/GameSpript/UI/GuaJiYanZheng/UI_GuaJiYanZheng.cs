using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuaJiYanZheng : MonoBehaviour {

    public GameObject YanZhengSet;
    private int rand_1;
    private string trueBtnID;

    public GameObject Obj_ShowText;
    public GameObject Obj_Btn_1;
    public GameObject Obj_Btn_2;
    public GameObject Obj_Btn_3;
    public GameObject Obj_Btn_4;

    private bool YanZhengStatus;
    private float TimeSum;
    public GameObject ShengYuTime;
    public bool IfYanZhengZhuCheng;

	// Use this for initialization
	void Start () {
        YanZheng();
    }
	
	// Update is called once per frame
	void Update () {

        if (YanZhengStatus) {
            TimeSum = TimeSum + Time.deltaTime;

            if (TimeSum >= 600)
            {
                TimeSum = 0;
                ExitGame();
            }

            ShengYuTime.GetComponent<Text>().text = "剩余验证时间:" + (600 - (int)(TimeSum)).ToString() + "秒";
        }
    }


    private void ShowWenTi() {

        rand_1 = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 9);

        //找出正确按钮
        trueBtnID = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 4).ToString();

        Obj_ShowText.GetComponent<Text>().text = "验证问题:请选择下面的数字" + rand_1.ToString();

        if (trueBtnID == "1")
        {
            Obj_Btn_1.GetComponent<Text>().text = rand_1.ToString();
        }
        else {
            Obj_Btn_1.GetComponent<Text>().text = getNum();
        }

        if (trueBtnID == "2")
        {
            Obj_Btn_2.GetComponent<Text>().text = rand_1.ToString();
        }
        else
        {
            Obj_Btn_2.GetComponent<Text>().text = getNum();
        }

        if (trueBtnID == "3")
        {
            Obj_Btn_3.GetComponent<Text>().text = rand_1.ToString();
        }
        else
        {
            Obj_Btn_3.GetComponent<Text>().text = getNum();
        }

        if (trueBtnID == "4")
        {
            Obj_Btn_4.GetComponent<Text>().text = rand_1.ToString();
        }
        else
        {
            Obj_Btn_4.GetComponent<Text>().text = getNum();
        }


    }

    private string getNum() {

        int randInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 9);
        if (randInt == rand_1) {
            randInt = randInt + 1;
        }
        return randInt.ToString();
    }


    private void YanZheng() {

        YanZhengSet.SetActive(false);

        //在主城中不弹出
        if (IfYanZhengZhuCheng)
        {
            if (Application.loadedLevelName == "EnterGame")
            {
                Destroy(this.gameObject);
                return;
            }
        }


        //在登陆界面中不弹出
        if (Application.loadedLevelName == "StartGame"|| Application.loadedLevelName == "EnterGame") {
            Destroy(this.gameObject);
            return;
        }


        try
        {

            //战斗状态中判定目标是否为Boss,如果是Boss就延迟弹出
            Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
            if (roseStatus.RoseFightStatus)
            {
                if (roseStatus.Obj_ActTarget != null)
                {
                    if (roseStatus.Obj_ActTarget.GetComponent<AI_1>() != null) {
                        AI_1 ai_1 = roseStatus.Obj_ActTarget.GetComponent<AI_1>();
                        string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", ai_1.AI_ID.ToString(), "Monster_Template");
                        if (monsterType == "3")
                        {
                            //如果是Boss延迟验证弹出
                            YanZhengStatus = false;
                            Destroy(this.gameObject);
                            return;
                        }
                    }
                }
            }

            YanZhengStatus = true;
            YanZhengSet.SetActive(true);
            ShowWenTi();

        }
        catch (Exception ex) {
            Destroy(this.gameObject);
        }
    }


    //按钮点击验证
    public void BtnYanZheng(string str) {

        //回答正确有奖励
        if (str == trueBtnID)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("回答正确");
            Destroy(this.gameObject);
        }
        else {
            //踢下线，并记录
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("回答错误");
            ExitGame();
        }

    }


    //退出游戏
    private void ExitGame() {

        Application.Quit();

    }

}
