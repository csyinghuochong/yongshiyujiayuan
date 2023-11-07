using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_FuBen_1 : MonoBehaviour {

    private bool daMiJingRewardStatus;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //进入副本地图
    public void Btn_EnterFuBen_1()
    {

        string meiRiCangBaoNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_1_DayNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (meiRiCangBaoNum == "")
        {
            meiRiCangBaoNum = "0";
        }
        int cangbaoNum = int.Parse(meiRiCangBaoNum);

        if (cangbaoNum <= 0)
        {
            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_1");

            string nanduNameStr = "普通模式";
            string nanduStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_4", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

            if (nanduStr == "2") {
                nanduNameStr = "挑战模式";
            }

            if (nanduStr == "3")
            {
                nanduNameStr = "地狱模式";
            }

            string nowStr = "<color=#FF0000FF>" + "当前副本难度 : " + nanduNameStr + "</color>" + "\n";
            langStrHint = nowStr + langStrHint;

            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, EnterFuBen_1, null);
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_36");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今天已经进入副本一次了,请明天再来！");
        }
    }

    public void EnterFuBen_1()
    {
        /*
        Debug.Log("副本暂未开启!敬请期待~");
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_37");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        return;
        */

        //Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000016",1);
        //接取任务
        //string nowTask = "31001049";
        //Game_PublicClassVar.Get_function_Task.GetTask(nowTask);

        //判断等级
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < 55) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_457");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            return;
        }


        //记录次数(测试功能暂时屏蔽)
        string nowNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_1_DayNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (nowNumStr == "") {
            nowNumStr = "0";
        }
        int nowNum = int.Parse(nowNumStr);
        if (nowNum >= 1)
        {
            /*
            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, Enter_DaMiJing, null);
            //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            */
        }
        else {
            nowNum = nowNum + 1;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_1_DayNum", nowNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

            //进入副本
            Game_PublicClassVar.Get_function_Rose.RoseMoveTargetMap("fb10001");
        }

    }


    //进入副本地图
    public void Btn_EnterFuBen_2()
    {

        string meiRiCangBaoNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_2_DayNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (meiRiCangBaoNum == "")
        {
            meiRiCangBaoNum = "0";
        }
        int cangbaoNum = int.Parse(meiRiCangBaoNum);
        //cangbaoNum = 0;     //测试
        if (cangbaoNum <= 0)
        {
            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_1");

            string nanduNameStr = "普通模式";
            string nanduStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_4", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

            if (nanduStr == "2")
            {
                nanduNameStr = "挑战模式";
            }

            if (nanduStr == "3")
            {
                nanduNameStr = "地狱模式";
            }

            string nowStr = "<color=#FF0000FF>" + "当前副本难度 : " + nanduNameStr + "</color>" + "\n";
            langStrHint = nowStr + langStrHint;

            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, EnterFuBen_2, null);
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_36");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今天已经进入副本一次了,请明天再来！");
        }
    }


    public void EnterFuBen_2()
    {
        /*
        Debug.Log("副本暂未开启!敬请期待~");
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_37");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        return;
        */

        //Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000016",1);
        //接取任务
        //string nowTask = "31001049";
        //Game_PublicClassVar.Get_function_Task.GetTask(nowTask);

        //判断等级
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < 60)
        {
            //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_457");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请提升等级至60级后进入此副本");
            return;
        }


        //记录次数(测试功能暂时屏蔽)
        string nowNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_2_DayNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (nowNumStr == "")
        {
            nowNumStr = "0";
        }
        int nowNum = int.Parse(nowNumStr);

        nowNum = nowNum + 1;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_2_DayNum", nowNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        //进入副本
        Game_PublicClassVar.Get_function_Rose.RoseMoveTargetMap("100008");
        

    }


    //进入大秘境
    public void Btn_DaMiJing_Enter() {

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
        if (damijingNum == "") {
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


    //大秘境地图玩法介绍
    public void DaMiJingJieShao()
    {
        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
        //string jieshaoStr = "1.玩家需要在限定的时间内击杀小怪收集秘境值,当秘境值达到500点后即可召唤秘境领主BOSS.\n2.击杀秘境BOSS后则挑战当前秘境层级成功,并激活下一层级大秘境,怪物实力随着秘境层级越高！\n3.大秘境内均会掉落秘境碎片可以在隔壁的同学处兑换奖励！\n4.大秘境成功退出地图后可以连续挑战,如果挑战失败只能等明日再来！";
        string jieshaoStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_5");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, null, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
    }

    //大秘境地图奖励领取
    public void Btn_DaMiJingReward()
    {
        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);

        //获取当前秘境层级
        string daMiJingLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string daMiJingRewardLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingRewardLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (daMiJingLv != "" && daMiJingRewardLv != "")
        {
            int daMiJingLv_int = int.Parse(daMiJingLv);
            int daMiJingRewardLv_int = int.Parse(daMiJingRewardLv);

            if (daMiJingLv_int > daMiJingRewardLv_int)
            {
                //可以领取礼包
                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_6");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_7");
                string jieshaoStr = langStrHint_1 + daMiJingRewardLv_int + langStrHint_2;
                //string jieshaoStr = "是否领取通关大秘境第"+ daMiJingRewardLv_int+ "层奖励？";
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, DaMiJingReward, null);
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }
            else {
                if (daMiJingLv_int > 1)
                {
                    string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_39");
                    string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_40");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + (daMiJingLv_int - 1).ToString() + langStrHint_2);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已领取通关大秘境第" + (daMiJingLv_int - 1).ToString() + "层奖励！");
                }
                else {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_41");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你还未通关大秘境第1层,无法领取奖励！");
                }
            }
        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_41");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你还未通关大秘境第1层,无法领取奖励！");
        }
    }

    public void DaMiJingReward() {

        if (daMiJingRewardStatus) {
            return;
        }

        daMiJingRewardStatus = true;
        string daMiJingRewardLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingRewardLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (daMiJingRewardLv != "")
        {

            //检测背包
            int bagNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
            if (bagNum >= 1)
            {
                //发送奖励
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010089",1,"0",0,"0",true,"2");

                //记录数据
                int daMiJingRewardLv_int = int.Parse(daMiJingRewardLv);
                daMiJingRewardLv_int = daMiJingRewardLv_int + 1;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJingRewardLv", daMiJingRewardLv_int.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
            }

        }

        daMiJingRewardStatus = false;
    }


    //进入喜从天降活动
    public void Btn_HuoDongChest_Enter()
    {
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_44");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("功能暂未开启!");
        Debug.Log("点击喜从天降按钮");

    }

    //进入伤害试炼
    public void Btn_ShangHai_Enter()
    {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, Enter_DaMiJing, null);
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入伤害试炼副本\n此副本进入次数不限,但每日只能领取一次奖励", Enter_ShangHai, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }



    //伤害试炼
    public void Enter_ShangHai()
    {

        //进入地图
        Game_PublicClassVar.Get_function_Rose.RoseMoveTargetMap("100005");

    }


    //进入伤害试炼
    public void Btn_Enter_FengYinZhiTa()
    {
        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, Enter_DaMiJing, null);
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入封印之塔？\n提示:进入前身上最好携带够足够的封印之塔的凭证哦,被封印的怪物为特殊爆率不受加成影响", Enter_FengYinZhiTa, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
    }



    //进入封印之塔
    public void Enter_FengYinZhiTa()
    {

        /*
        if (Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10000060") >= 1)
        {
            //进入地图
            Game_PublicClassVar.Get_function_Rose.RoseMoveTargetMap("100007");
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要凭证数量不足！请先确定背包内是否已有:封印之塔的凭证");
        }
        */

        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < 13) {
            Debug.Log("请将等级最少提升至13级以上才可进入");
            return;
        }

        //进入地图
        Game_PublicClassVar.Get_function_Rose.RoseMoveTargetMap("100007");

        

    }




    //打开伤害试炼界面
    public void Open_ShangHaiReward() {

        UI_FunctionOpen f_open = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>();
        GameObject obj = f_open.FunctionInstantiate(f_open.Obj_ShangHaiShiLian, "Obj_ShangHaiShiLian");
        obj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(1, 1, 1);

    }

    
    //领取奖励
    public void Btn_LingQuShangHaiEveryReward()
    {
        string nowShangHaiRewardID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiEveryRewardID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (nowShangHaiRewardID == "" || nowShangHaiRewardID == "0" || nowShangHaiRewardID == null)
        {
            string nowShangHaiIDValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", "10001", "FuBenShangHai_Template");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要伤害达到" + nowShangHaiIDValue + "领取副本奖励!");
            return;
        }

        //读取今天是否领取
        ObscuredString FuBen_ShangHaiNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (FuBen_ShangHaiNumStr == "" || FuBen_ShangHaiNumStr == null)
        {
            FuBen_ShangHaiNumStr = "0";
        }

        if (int.Parse(FuBen_ShangHaiNumStr) >= 1)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日奖励已领取,请明天再来!");
            return;
        }

        //读取当前奖励金币
        ObscuredString sendRewardGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldReward", "ID", nowShangHaiRewardID, "FuBenShangHai_Template");
        ObscuredString sendRewardLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowShangHaiRewardID, "FuBenShangHai_Template");

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
        string langStrHint = "是否领取进入伤害奖励! 当前伤害奖励等级：" + sendRewardLv + "级,是否领取?\n提示:造成的伤害奖励越高,奖励越高。每天可无限进入,取最高伤害领取当天奖励";
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, SendReward, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    //领取奖励
    public void SendReward()
    {

        //FuBen_ShangHaiNum
        ObscuredString FuBen_ShangHaiNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (FuBen_ShangHaiNumStr == "" || FuBen_ShangHaiNumStr == null)
        {
            FuBen_ShangHaiNumStr = "0";
        }

        if (int.Parse(FuBen_ShangHaiNumStr) >= 1)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日奖励已领取,请明天再来!");
            return;
        }

        int writeValue = int.Parse(FuBen_ShangHaiNumStr) + 1;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_ShangHaiNum", writeValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");


        //读取当前奖励金币
        string nowShangHaiRewardID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiHightID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        ObscuredString sendRewardGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldReward", "ID", nowShangHaiRewardID, "FuBenShangHai_Template");
        Game_PublicClassVar.Get_function_Rose.SendRewardToBag("1", int.Parse(sendRewardGold),"0",0,"0",false,"57");

    }
    
}
