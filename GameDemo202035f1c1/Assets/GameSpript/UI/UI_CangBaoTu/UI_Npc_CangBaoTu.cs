using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Npc_CangBaoTu : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //进入藏宝图地图
    public void Btn_EnterCangBaoTu() {

        //获取玩家等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv < 15) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_242");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("15级可以进入藏宝洞窟！");
            return;
        }

        string meiRiCangBaoNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MeiRiCangBaoNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (meiRiCangBaoNum == "")
        {
            meiRiCangBaoNum = "0";
        }
        int cangbaoNum = int.Parse(meiRiCangBaoNum);
        //cangbaoNum = 0;
        if (cangbaoNum <= 0)
        {
            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_16");
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, EnterCangBaoTu, null);
            //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入藏宝洞窟？\n提示:每天只有一次进入机会！", EnterCangBaoTu, null);
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
        }
        else {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_243");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今天已经进入藏宝洞窟一次了,请明天再来！");
        }
    }

    public void EnterCangBaoTu() {

        //Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000016",1);
        //接取任务
        //string nowTask = "31001049";
        //Game_PublicClassVar.Get_function_Task.GetTask(nowTask);

        //记录次数
        string meiRiCangBaoNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MeiRiCangBaoNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (meiRiCangBaoNum == "")
        {
            meiRiCangBaoNum = "0";
        }
        int cangbaoNum = int.Parse(meiRiCangBaoNum);
        cangbaoNum = cangbaoNum + 1;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MeiRiCangBaoNum", cangbaoNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        //设置满血状态
        Rose_Proprety roseProprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        int roseHp = roseProprety.Rose_Hp;
        roseProprety.Rose_HpNow = roseProprety.Rose_Hp;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseHpNow", roseHp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //写入藏宝库每日任务
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "8", "1");

        //进入藏宝洞穴
        Game_PublicClassVar.Get_function_Rose.RoseMoveTargetMap("100001");

    }

    //兑换高级藏宝图地图
    public void Btn_DuiHuanCangBaoTu()
    {
        string itemName_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", "10000018", "Item_Template");
        string itemName_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", "10000019", "Item_Template");
        string itemName_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", "10000020", "Item_Template");

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_21");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint + itemName_1  + "x1 "+ itemName_2 + "x1 " + itemName_3 + "x1 ", DuiHuanGaoJiCangBaoTu, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否消耗材料兑换一个高级藏宝图？\n消耗道具:" + itemName_1 + "x1 " + itemName_2 + "x1 " + itemName_3 + "x1 ", DuiHuanGaoJiCangBaoTu, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
    }


    //兑换高级藏宝图
    public void DuiHuanGaoJiCangBaoTu() {

        //判断背包是否有材料
        int item_1 = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10000018");
        int item_2 = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10000019");
        int item_3 = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10000020");
        if (item_1 >= 1 && item_2 >= 1 && item_3 >= 1)
        {
            //扣除道具,发送高级藏宝图
            Game_PublicClassVar.Get_function_Rose.CostBagItem("10000018", 1);
            Game_PublicClassVar.Get_function_Rose.CostBagItem("10000019", 1);
            Game_PublicClassVar.Get_function_Rose.CostBagItem("10000020", 1);
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000017", 1,"0",0,"0",true,"30");
        }
        else {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_244");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要材料不足");
        }
    }

    //藏宝图地图玩法介绍
    public void CangBaoTuJieShao() {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
        //string jieshaoStr = "1.地图随机刷新出真假宝箱,触碰到真的宝箱可以获得真正的奖励,地图内会出现6个真正的藏宝箱\n2.地图只有5分钟时间提供给玩家寻找宝箱,超过时间会自动传送出地图！\n3.地图内不能使用任何技能和道具";
        string jieshaoStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_22");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, null, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }


    //领取藏宝图地图奖励
    public void CangBaoTuReward()
    {
        string chestNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MeiRiCangBaoTrueChestNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (chestNum == "-1")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_245");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日已经领取,请明日再来！");
            return;
        }
        //弹出提示
        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_23");
        string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_24");
        //string jieshaoStr = "恭喜你!今天你寻找到了" + chestNum + "个真实宝箱！\n寻找1-3个宝箱获得1张藏宝图 \n寻找4-5个宝箱获得2张藏宝图 \n寻找6个宝箱获得3张藏宝图";
        string jieshaoStr = langStrHint_1 + chestNum + langStrHint_2;
        string langStrHint_4 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_10");
        string langStrHint_5 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_11");
        string langStrHint_6 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_12");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, SendCangBaoTuReward, null, langStrHint_4, langStrHint_5, langStrHint_6);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
    }

    //发送藏宝图奖励
    public void SendCangBaoTuReward() {

        int chestNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MeiRiCangBaoTrueChestNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
        if (chestNum == -1) {
            return;
        }

        if (chestNum >= 1) {
            //空的背包
            int bagNullNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
            if (bagNullNum >= 1) {

                //计算发送的数量  1-3发送1个  4-5 发送2个 6发送3个
                int sendNum = 1;
                if (chestNum >= 1) {
                    sendNum = 1;
                }
                if (chestNum >= 4)
                {
                    sendNum = 2;
                    if (chestNum >= 6) {
                        sendNum = 3;
                    }
                }

                //发送奖励
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000016", sendNum, "0", 0, "0", true, "31");
                //记录发送
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MeiRiCangBaoTrueChestNum", "-1","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
            }
        }
    }

}
