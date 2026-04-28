using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_ChouKaNumRewardList : MonoBehaviour {

    public ObscuredString ChouKaRewardListID;
    public ObscuredInt Reward_Min;
    public ObscuredInt Reward_Max;
    public ObscuredString Reward_Name;
    public ObscuredInt NeedChouKaNum;
    public GameObject Obj_ChouKaRewardName;
    public GameObject Obj_ChouKaRewardValue;

    public ObscuredBool LingQuStatus;
    public GameObject Obj_ChouKaRewardLingQuImg;
    public GameObject Obj_ChouKaRewardLingQuBtn;

    public GameObject Obj_ItemShow;
    public ObscuredString RewardItemID;
    public ObscuredInt RewardItemNum;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init() {

        clearnShow();

        string dayChouKaNumReward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ChouKaNumReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

        if (dayChouKaNumReward.Contains(ChouKaRewardListID))
        {
            LingQuStatus = true;
        }
        else {
            LingQuStatus = false;
        }

        if (LingQuStatus)
        {
            Obj_ChouKaRewardLingQuImg.SetActive(true);
        }
        else {
            Obj_ChouKaRewardLingQuBtn.SetActive(true);
        }
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("探宝次数达到");
        Reward_Name = langStr;

        switch (ChouKaRewardListID) {

            case "1":
                NeedChouKaNum = 30;
                Reward_Min = 100;
                Reward_Max = 1000;
                RewardItemID = "10000024";
                RewardItemNum = 1;
                break;

            case "2":
                NeedChouKaNum = 60;
                Reward_Min = 200;
                Reward_Max = 1000;
                RewardItemID = "10000016";
                RewardItemNum = 3;
                break;

            case "3":
                NeedChouKaNum = 100;
                Reward_Min = 300;
                Reward_Max = 1500;
                RewardItemID = "10000017";
                RewardItemNum = 1;
                //RewardItemID = "1";
                //RewardItemNum = 300000;
                break;

            case "4":
                NeedChouKaNum = 150;
                Reward_Min = 400;
                Reward_Max = 2000;
                RewardItemID = "12001003";
                RewardItemNum = 10;

                //如果坐骑已经激活,则改变奖励
                if (Game_PublicClassVar.Get_function_Pasture.IfHaveZuoQi("10003")) {
                    RewardItemID = "1";
                    RewardItemNum = 500000;
                }

                break;

            case "5":
                NeedChouKaNum = 250;
                Reward_Min = 500;
                Reward_Max = 2500;
                RewardItemID = "10010026";
                RewardItemNum = 1;
                break;

        }

        //显示奖励
        Reward_Name = Reward_Name + NeedChouKaNum + "次";
        Obj_ChouKaRewardName.GetComponent<Text>().text = Reward_Name;
        Obj_ChouKaRewardValue.GetComponent<Text>().text = Reward_Min + "-" + Reward_Max;

        Obj_ItemShow.GetComponent<UI_CommonItemShow_1>().ItemID = RewardItemID;
        Obj_ItemShow.GetComponent<UI_CommonItemShow_1>().ItemNum = RewardItemNum.ToString();
        Obj_ItemShow.GetComponent<UI_CommonItemShow_1>().IfHideName = true;
        Obj_ItemShow.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        Obj_ItemShow.GetComponent<UI_CommonItemShow_1>().IfChangeSize = true;
    }

    private void clearnShow() {

        Obj_ChouKaRewardLingQuImg.SetActive(false);
        Obj_ChouKaRewardLingQuBtn.SetActive(false);

    }

    //领取奖励
    public void Btn_LingQuReward() {

        if (LingQuStatus) {
            return;
        }

        string langStr;
        //判断自身道具格子
        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() < 1) {
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_84");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            return;
        }


        //判定是否符合要求
        string dayChouKaNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ChouKaNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (dayChouKaNum == "")
        {
            dayChouKaNum = "0";
        }
        if (int.Parse(dayChouKaNum) < NeedChouKaNum) {
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_437");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            return;
        }


        //发送对应奖励
        Game_PublicClassVar.Get_function_Rose.SendRewardToBag(RewardItemID,RewardItemNum,"0",0,"0",true,"54");

        //随机钻石
        int addValue = Reward_Min + (int)((Reward_Max - Reward_Min) * Random.value);
        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_438");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr + addValue);
        Game_PublicClassVar.Get_function_Rose.SendReward("2", addValue.ToString(), "54");

        //记录领取数据
        string dayChouKaNumReward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ChouKaNumReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        dayChouKaNumReward = dayChouKaNumReward + ";" + ChouKaRewardListID;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ChouKaNumReward", dayChouKaNumReward, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        clearnShow();
        Obj_ChouKaRewardLingQuImg.SetActive(true);

    }
}
