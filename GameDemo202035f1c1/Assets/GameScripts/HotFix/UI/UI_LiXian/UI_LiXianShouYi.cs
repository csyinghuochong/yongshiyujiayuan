using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class UI_LiXianShouYi : MonoBehaviour
{
    public ObscuredInt ChuBeiTime;
    public ObscuredInt ChuBeiExpNum;
    public ObscuredInt ChuBeiLiXianNum;

    public GameObject Obj_ChuBeiTime;
    public GameObject Obj_ChuBeiExp;
    public GameObject Obj_ChuBeiLiXianNum;

    // Start is called before the first frame update
    void Start()
    {
        //Init();
    }

    //初始化
    public void Init() {

        int hour = (ChuBeiTime / 60);
        int min = ChuBeiTime - hour * 60;
        if (hour == 0)
        {
            Obj_ChuBeiTime.GetComponent<Text>().text = "离线时间:" + min + "分钟";
        }
        else
        {
            Obj_ChuBeiTime.GetComponent<Text>().text = "离线时间:" + hour + "小时" + min + "分钟";
        }

        //Obj_ChuBeiTime.GetComponent<Text>().text = "离线时间:" + "小时";
        Obj_ChuBeiExp.GetComponent<Text>().text = "离线经验:" + ChuBeiExpNum;
        Obj_ChuBeiLiXianNum.GetComponent<Text>().text = "储备点数:" + ChuBeiLiXianNum;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Btn_LiXian() {

        Destroy(this.gameObject);

    }

    public void Btn_ChaKan() {

        Btn_LiXian();

        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenErveryDayTask();
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseDayTask.GetComponent<UI_DayTask>().IfNoInit = true;
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseDayTask.GetComponent<UI_DayTask>().Btn_LiXianJingYan();

    }

}
