using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DaMiJing_Reward : MonoBehaviour {

    private bool daMiJingRewardStatus;
    public string ShouLieDataStr;
    public GameObject Obj_Ceng;
    public GameObject Obj_DaMiJingRewardListSet;
    public GameObject Obj_DaMiJingRewardList;

    // Use this for initialization
    void Start () {

        Init();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Init() {

        string daMiJingLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (daMiJingLv == "" || daMiJingLv == null) {
            daMiJingLv = "0";
        }
        Obj_Ceng.GetComponent<Text>().text = daMiJingLv;

        //显示奖励信息
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_DaMiJingRewardListSet);
        //string[] shouLieRewardList = ShouLieDataStr.Split('#');
        string daMiJingRewardLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingRewardLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (daMiJingRewardLv == "" || daMiJingRewardLv == null)
        {
            daMiJingRewardLv = "0";
        }
        int daMiJingLvInt = int.Parse(daMiJingRewardLv); 

        for (int i = 0; i < 5; i++)
        {
            if (daMiJingLvInt == 0) {
                daMiJingLvInt = daMiJingLvInt + 1;
            }
            
            if (daMiJingLvInt <= 100)
            {
                GameObject obj = (GameObject)Instantiate(Obj_DaMiJingRewardList);
                obj.transform.SetParent(Obj_DaMiJingRewardListSet.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.GetComponent<UI_DaMiJing_RewardList>().RewardLv = daMiJingLvInt.ToString();
                obj.GetComponent<UI_DaMiJing_RewardList>().LingQuRewardStr = RetuenLvReward(daMiJingLvInt);
            }

            if (daMiJingLvInt >= 1) {
                daMiJingLvInt = daMiJingLvInt + 1;
            }
        }
    }


    //进入大秘境
    public void Btn_DaMiJing_Enter()
    {

        string meiRiCangBaoNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJing_DayNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (meiRiCangBaoNum == "")
        {
            meiRiCangBaoNum = "0";
        }

        int cangbaoNum = int.Parse(meiRiCangBaoNum);

        //cangbaoNum = 0;       //测试用的
        if (cangbaoNum <= 0)
        {
            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, Enter_DaMiJing, null);
            //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_38");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今天已经进入副本一次了,请明天再来！");
        }
    }

    public void Enter_DaMiJing()
    {

        //Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000016",1);
        //接取任务
        //string nowTask = "31001049";
        //Game_PublicClassVar.Get_function_Task.GetTask(nowTask);

        //记录次数
        string damijingNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJing_DayNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (damijingNum == "")
        {
            damijingNum = "0";
        }
        int num = int.Parse(damijingNum) + 1;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJing_DayNum", num.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        //写入每日任务
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "9", "1");

        //进入藏宝洞穴
        Game_PublicClassVar.Get_function_Rose.RoseMoveTargetMap("100002");

    }


    //大秘境地图奖励领取
    public void Btn_DaMiJingReward()
    {
        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);

        //获取当前秘境层级
        string daMiJingLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string daMiJingRewardLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingRewardLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (daMiJingLv != "")
        {
            if (daMiJingRewardLv == "" || daMiJingRewardLv == null) {
                daMiJingRewardLv = "0";
            }

            int daMiJingLv_int = int.Parse(daMiJingLv);
            int daMiJingRewardLv_int = int.Parse(daMiJingRewardLv);

            if (daMiJingLv_int > daMiJingRewardLv_int)
            {
                //可以领取礼包
                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_6");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_7");
                daMiJingRewardLv_int = daMiJingRewardLv_int + 1;
                string jieshaoStr = langStrHint_1 + daMiJingRewardLv_int + langStrHint_2;
                //string jieshaoStr = "是否领取通关大秘境第"+ daMiJingRewardLv_int+ "层奖励？";
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, DaMiJingReward, null);
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                if (daMiJingLv_int > 1)
                {
                    //string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_39");
                    //string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_40");
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + (daMiJingLv_int - 1).ToString() + langStrHint_2);
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先完成对应层数的挑战！");
                }
                else
                {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_41");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你还未通关大秘境第1层,无法领取奖励！");
                }
            }
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_41");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你还未通关大秘境第1层,无法领取奖励！");
        }
    }

    public void DaMiJingReward()
    {

        if (daMiJingRewardStatus)
        {
            return;
        }

        daMiJingRewardStatus = true;
        string daMiJingRewardLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingRewardLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        if (daMiJingRewardLv == "" || daMiJingRewardLv == null)
        {
            daMiJingRewardLv = "0";
        }

        if (daMiJingRewardLv != "")
        {

            if (int.Parse(daMiJingRewardLv) >= 100) {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("奖励已经领完!");
                return;
            }

            //检测背包
            int bagNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
            if (bagNum >= 2)
            {
                //发送奖励
                string sendStr = RetuenLvReward(int.Parse(daMiJingRewardLv) + 1);
                Game_PublicClassVar.Get_function_Rose.Rose_SendRewardStr(sendStr);
                //Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010089", 1, "0", 0, "0", true, "2");

                //记录数据
                int daMiJingRewardLv_int = int.Parse(daMiJingRewardLv);
                daMiJingRewardLv_int = daMiJingRewardLv_int + 1;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJingRewardLv", daMiJingRewardLv_int.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                //重新刷新
                Init();
            }
            else
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包预留位置不足2个!");
            }

        }

        daMiJingRewardStatus = false;

    }

    public void Btn_Close() {

        Destroy(this.gameObject);

    }

    //根据层数返回奖励
    private string RetuenLvReward(int rewardLv) {

        int gold = 50000 + (int)(rewardLv / 10) * 50000;

        return "1," + gold + ";10010088,20;10010089,1";

    }

}
