using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HuoDong_TowerShow : MonoBehaviour {

    public ObscuredString nowShangHaiID;
    public ObscuredString nowShangHaiRewardID;
    public ObscuredString nowShangHaiIDValue;
    public ObscuredFloat fightTimeSum;
    public ObscuredBool FightStatus;
    public ObscuredFloat FightTime;
    public GameObject Obj_Par;
    public GameObject Obj_MapTime;
    public GameObject Obj_UpdateMonsterTime;
    public GameObject Obj_Ceng;

    public float UpdateTimeSum;

    // Use this for initialization
    void Start()
    {

        //fightTimeSum = 180;         //设置每次战斗时间
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        //初始化显示
        initShow();

    }

    // Update is called once per frame
    void Update()
    {
        if (FightStatus)
        {
            UpdateTimeSum = UpdateTimeSum + Time.deltaTime;

            if (UpdateTimeSum >= 0.1f)
            {
                UpdateTimeSum = 0;
                UpdateShow();
            }

            FightTime = FightTime + Time.deltaTime;
            if (FightTime >= fightTimeSum)
            {
                //战斗结束
                Obj_Par.GetComponent<Main_100006>().EndFuBen();
            }
        }
    }

    void UpdateShow()
    {
        Game_PositionVar gameVar = Game_PublicClassVar.Get_game_PositionVar;
        ObscuredLong fubenValue = gameVar.FuBen_ShangHaiValue_Rose + gameVar.FuBen_ShangHaiValue_Pet;
        int costNum = (int)(fightTimeSum - FightTime);
        if (costNum >= 0)
        {
            Obj_MapTime.GetComponent<Text>().text = "剩余战斗时间:" + (int)(fightTimeSum - FightTime) + "秒";
        }
        else {
            Obj_MapTime.GetComponent<Text>().text = "即将离开地图!";
        }
       
    }

    //初始化显示
    void initShow()
    {

        Game_PositionVar gameVar = Game_PublicClassVar.Get_game_PositionVar;
        Obj_MapTime.GetComponent<Text>().text = "战斗准备开始";
    }

    //重置
    public void Btn_ChongZhi()
    {

        if (Obj_Par.GetComponent<Main_100006>() != null)
        {
            Obj_Par.GetComponent<Main_100006>().InitFuBen();
        }

        initShow();

    }

    //重置
    public void Btn_End()
    {

        Obj_Par.GetComponent<Main_100006>().EndFuBen();
        FightStatus = false;

    }
}
