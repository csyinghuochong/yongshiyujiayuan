using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_FuBen_ShangHaiShow : MonoBehaviour {

    public ObscuredString nowShangHaiID;
    public ObscuredString nowShangHaiRewardID;
    public ObscuredString nowShangHaiIDValue;
    private ObscuredFloat fightTimeSum;
    public ObscuredBool FightStatus;
    public ObscuredFloat FightTime;
    public GameObject Obj_Par;
    public GameObject Obj_MapTime;
    public GameObject Obj_ShangHaiSumValue;
    public GameObject Obj_ShangHaiSumValue_Rose;
    public GameObject Obj_ShangHaiSumValue_Pet;
    public GameObject Obj_ShangHaiMiaoValue;
    public GameObject Obj_ZhanDouTime;
    public GameObject Obj_ZhanDouRewardLv;
    public GameObject Obj_ZhanDouNextNeedDamge;

    public float UpdateTimeSum;

	// Use this for initialization
	void Start () {

        fightTimeSum = 90;         //设置每次战斗时间
        initShow();
        //UpdateShow();
        //适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);


        //写入活跃任务
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "133", "1");
    }
	
	// Update is called once per frame
	void Update () {

        if (FightStatus) {

            UpdateTimeSum = UpdateTimeSum + Time.deltaTime;

            if (UpdateTimeSum >= 0.1f)
            {
                UpdateTimeSum = 0;
                UpdateShow();
            }


            FightTime = FightTime + Time.deltaTime;
            if (FightTime >= fightTimeSum) {
                FightTime = 0;
                //战斗结束
                Obj_Par.GetComponent<Main_100005>().EndFuBen();
            }
        }

	}

    void UpdateShow() {

        Game_PositionVar gameVar = Game_PublicClassVar.Get_game_PositionVar;

        if (Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHai_Status) {

            ObscuredLong fubenValue = gameVar.FuBen_ShangHaiValue_Rose + gameVar.FuBen_ShangHaiValue_Pet;
            int showCostSec = (int)(fightTimeSum - FightTime);
            if (showCostSec <= 0)
            {
                showCostSec = 0;
            }

            if (Obj_Par != null) {
                if (Obj_Par.GetComponent<Main_100005>().CreateSkillMonsterStatus == true) {
                    Obj_MapTime.GetComponent<Text>().text = "剩余战斗时间:" + showCostSec + "秒";
                }
            }
            
            Obj_ShangHaiSumValue.GetComponent<Text>().text = "总体伤害:" + fubenValue.ToString();
            Obj_ShangHaiSumValue_Rose.GetComponent<Text>().text = "玩家伤害:" + gameVar.FuBen_ShangHaiValue_Rose;
            Obj_ShangHaiSumValue_Pet.GetComponent<Text>().text = "宠物伤害:" + gameVar.FuBen_ShangHaiValue_Pet;
            Obj_ZhanDouTime.GetComponent<Text>().text = "战斗时间:" + (int)FightTime + "秒";
            Obj_ShangHaiMiaoValue.GetComponent<Text>().text = "总体秒伤:" + (int)((gameVar.FuBen_ShangHaiValue_Rose + gameVar.FuBen_ShangHaiValue_Pet) / FightTime);

            //显示伤害等级
            if (fubenValue >= long.Parse(nowShangHaiIDValue))
            {
                //获取下一等级
                if (nowShangHaiID == "" || nowShangHaiID == "0" || nowShangHaiID == null)
                {
                    nowShangHaiID = "10001";
                }
                nowShangHaiRewardID = nowShangHaiID;

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_ShangHaiEveryRewardID", nowShangHaiID.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                string NextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nowShangHaiID, "FuBenShangHai_Template");
                if (NextID != "0" && NextID != "" && NextID != null)
                {
                    nowShangHaiIDValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", NextID, "FuBenShangHai_Template");
                    nowShangHaiID = NextID;
                }

                string nowDamgeLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowShangHaiRewardID, "FuBenShangHai_Template");
                Obj_ZhanDouRewardLv.GetComponent<Text>().text = "伤害奖励等级:" + nowDamgeLv + "级";

                int nextDamgeID = int.Parse(nowShangHaiRewardID) + 1;
                if(nextDamgeID<= 10100)
                {
                    string needDamgeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", nextDamgeID.ToString(), "FuBenShangHai_Template");
                    Obj_ZhanDouNextNeedDamge.GetComponent<Text>().text = "下级伤害目标:" + needDamgeStr;
                }


                //写入当前达到的最高伤害等级
                string hightID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiHightID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (hightID == "" || hightID == null)
                {
                    hightID = "0";
                }
                if (int.Parse(nowShangHaiRewardID) > int.Parse(hightID))
                {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_ShangHaiHightID", nowShangHaiRewardID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                }

            }
        }
    }

    //初始化显示
    void initShow()
    {

        Game_PositionVar gameVar = Game_PublicClassVar.Get_game_PositionVar;

        Obj_MapTime.GetComponent<Text>().text = "战斗准备开始";
        Obj_ShangHaiSumValue.GetComponent<Text>().text = "总体伤害:" + 0;
        Obj_ShangHaiSumValue_Rose.GetComponent<Text>().text = "玩家伤害:" + 0;
        Obj_ShangHaiSumValue_Pet.GetComponent<Text>().text = "宠物伤害:" + 0;
        Obj_ZhanDouTime.GetComponent<Text>().text = "战斗时间:" + 0;
        Obj_ShangHaiMiaoValue.GetComponent<Text>().text = "总体秒伤:" + 0;

        //读取初始化ID值
        nowShangHaiIDValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", "10001", "FuBenShangHai_Template");

        Obj_ZhanDouRewardLv.GetComponent<Text>().text = "伤害达到" + nowShangHaiIDValue + "开启奖励";
        Obj_ZhanDouNextNeedDamge.GetComponent<Text>().text = "下级伤害目标:" + nowShangHaiIDValue;

        nowShangHaiID = "10001";
        nowShangHaiRewardID = nowShangHaiID;

        //统计状态清空
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Rose = 0;
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Pet = 0;
    }

    //重置
    public void Btn_ChongZhi() {

        if (Obj_Par.GetComponent<Main_100005>() != null) {

            Obj_Par.GetComponent<Main_100005>().EndFuBen();
            Obj_Par.GetComponent<Main_100005>().InitFuBen();
        }

        initShow();

    }

    //重置
    public void Btn_End()
    {

        Obj_Par.GetComponent<Main_100005>().EndFuBen();
        FightStatus = false;

    }

    //领取奖励
    /*
    public void Btn_LingQuReward() {

        if (nowShangHaiRewardID == "" || nowShangHaiRewardID == "0" || nowShangHaiRewardID == null) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要伤害达到" + nowShangHaiIDValue + "领取副本奖励!");
            return;
        }

        //读取今天是否领取
        ObscuredString FuBen_ShangHaiNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (FuBen_ShangHaiNumStr == "" || FuBen_ShangHaiNumStr == null)
        {
            FuBen_ShangHaiNumStr = "0";
        }
        if (int.Parse(FuBen_ShangHaiNumStr) >= 1) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日奖励已领取,请明天再来!");
            return;
        }

        //写入当前达到的最高伤害等级
        string hightID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiHightID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (int.Parse(nowShangHaiRewardID) > int.Parse(hightID))
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_ShangHaiHightID", nowShangHaiRewardID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        }

        //读取当前等级
        //int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

        //读取当前奖励金币
        ObscuredString sendRewardGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldReward", "ID", nowShangHaiRewardID, "FuBenShangHai_Template");
        ObscuredString sendRewardLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowShangHaiRewardID, "FuBenShangHai_Template");

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
        string langStrHint = "是否领取进入伤害奖励! 当前伤害奖励等级：" + sendRewardLv + "级,是否领取?\n提示:每天只有一次领取机会,伤害奖励越高,奖励越高.";
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, SendReward, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    //领取奖励
    public void SendReward() {

        //FuBen_ShangHaiNum
        ObscuredString FuBen_ShangHaiNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (FuBen_ShangHaiNumStr == "" || FuBen_ShangHaiNumStr == null) {
            FuBen_ShangHaiNumStr = "0";
        }

        if (int.Parse(FuBen_ShangHaiNumStr) >= 1)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日奖励已领取,请明天再来!");
            return;
        }

        int writeValue = int.Parse(FuBen_ShangHaiNumStr) + 1;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_ShangHaiNum", writeValue.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");


        //读取当前奖励金币
        ObscuredString sendRewardGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldReward", "ID", nowShangHaiRewardID, "FuBenShangHai_Template");
        Game_PublicClassVar.Get_function_Rose.SendReward("1", sendRewardGold);

    }
    */

}
