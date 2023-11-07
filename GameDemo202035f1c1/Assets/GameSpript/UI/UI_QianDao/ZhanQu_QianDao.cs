using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class ZhanQu_QianDao : MonoBehaviour {

    public GameObject Obj_QianDaoSet;
    public GameObject Obj_QianDao;
    public GameObject Obj_ComReward_ShowSet;
    public GameObject Obj_PayReward_ShowSet;
    public GameObject Obj_ComReward_LingQuImg;
    public GameObject Obj_PayReward_LingQuImg;
    public GameObject Obj_CommonItemShow;
    public ObscuredString NowXuanZhongID;
    public ObscuredBool UpdateStatus;
    public ObscuredInt UpdateNum;

    // Use this for initialization
    void Start () {

        Init();
    }

    public void Init() {

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_QianDaoSet);

        //循环显示签到
        for (int i = 1; i <= 30; i++)
        {
            string qianDaoID = (10000 + i).ToString();
            GameObject qiandaoObj = (GameObject)Instantiate(Obj_QianDao);
            qiandaoObj.transform.SetParent(Obj_QianDaoSet.transform);
            qiandaoObj.transform.localScale = new Vector3(1, 1, 1);
            qiandaoObj.GetComponent<QianDaoItemShow>().Obj_QianDaoParObj = this.gameObject;
            qiandaoObj.GetComponent<QianDaoItemShow>().QianDaoID = qianDaoID;
        }

        //默认显示签到
        string qianDaoNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string qianDaoNum_Day = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com_Day", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (int.Parse(qianDaoNum) < 30 && qianDaoNum_Day == "0")
        {
            string showQianDaoID = (10000 + int.Parse(qianDaoNum) + 1).ToString();
            ShowQianDaoReward(showQianDaoID);
        }
        else {
            string showQianDaoID = (10000 + int.Parse(qianDaoNum)).ToString();
            ShowQianDaoReward(showQianDaoID);
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (UpdateStatus) {
            if (UpdateNum >= 1) {
                UpdateStatus = false;
                UpdateNum = 0;
            }
            UpdateNum = UpdateNum + 1;
        }
	}

    //展示奖励
    public void ShowQianDaoReward(string qianDaoID) {

        //只能查询今天和之前的奖励,保留神秘感
        string qianDaoNum_Com = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string qianDaoNum_Com_Day = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com_Day", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (qianDaoNum_Com_Day == "1")
        {
            //今日已经领取
            if (int.Parse(qianDaoID) > int.Parse(qianDaoNum_Com) + 10000)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_246");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("到当天指定天数才可查看奖励喔!");
                return;
            }
        }
        else {
            if (int.Parse(qianDaoID) > int.Parse(qianDaoNum_Com) + 1 + 10000)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_246");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("到当天指定天数才可查看奖励喔!");
                return;
            }
        }

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ComReward_ShowSet);
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PayReward_ShowSet);

        NowXuanZhongID = qianDaoID;

        //获取普通签到和付费签到的奖励
        string qianDao_Com = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDao_Com", "ID", NowXuanZhongID, "QianDao_Template");
        string qianDao_Pay = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDao_Pay", "ID", NowXuanZhongID, "QianDao_Template");

        //如果真身有猩猩则更换礼包    
        string ZuoQiPiFuSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiPiFuSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (ZuoQiPiFuSetStr.Contains("10005")) {
            qianDao_Pay = qianDao_Pay.Replace("10000025", "10000082"); 
        }
        
        string[] qianDao_Com_List = qianDao_Com.Split(';');
        string[] qianDao_Pay_List = qianDao_Pay.Split(';');

        //显示普通签到
        for (int i = 0; i < qianDao_Com_List.Length; i++)
        {
            //显示奖励
            GameObject RewardObj = (GameObject)Instantiate(Obj_CommonItemShow);
            RewardObj.transform.SetParent(Obj_ComReward_ShowSet.transform);

            string itemID = qianDao_Com_List[i].Split(',')[0];
            string itemNum = qianDao_Com_List[i].Split(',')[1];

            RewardObj.GetComponent<UI_CommonItemShow_1>().ItemID = itemID;
            RewardObj.GetComponent<UI_CommonItemShow_1>().ItemNum = itemNum;
            RewardObj.GetComponent<UI_CommonItemShow_1>().IfHideName = true;
        }

        //显示赞助签到
        for (int i = 0; i < qianDao_Pay_List.Length; i++)
        {
            //显示奖励
            GameObject RewardObj = (GameObject)Instantiate(Obj_CommonItemShow);
            RewardObj.transform.SetParent(Obj_PayReward_ShowSet.transform);

            string itemID = qianDao_Pay_List[i].Split(',')[0];
            string itemNum = qianDao_Pay_List[i].Split(',')[1];

            RewardObj.GetComponent<UI_CommonItemShow_1>().ItemID = itemID;
            RewardObj.GetComponent<UI_CommonItemShow_1>().ItemNum = itemNum;
            RewardObj.GetComponent<UI_CommonItemShow_1>().IfHideName = true;
        }


        if (qianDaoNum_Com_Day == "1")
        {
            Obj_ComReward_LingQuImg.SetActive(true);
        }
        else {
            Obj_ComReward_LingQuImg.SetActive(false);
        }

        string qianDaoDay = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoDay", "ID", NowXuanZhongID, "QianDao_Template");
        string qianDaoNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string qianDaoNum_Pay_Day = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Pay_Day", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (qianDaoNum_Pay_Day == "1")
        {
            Obj_PayReward_LingQuImg.SetActive(true);
        }
        else {
            if (int.Parse(qianDaoDay) < int.Parse(qianDaoNum))
            {
                Obj_PayReward_LingQuImg.SetActive(true);
            }
            else {
                Obj_PayReward_LingQuImg.SetActive(false);
            }
        }
    }

    //领取签到奖励
    public void Btn_LingQuQianDaoReward_Com() {

        if (NowXuanZhongID == "") {
            return;
        }

        ObscuredString qianDaoNum_Com_Day = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com_Day", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (qianDaoNum_Com_Day == "1") {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_247");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日签到奖励已领取,请明日再来!");
            return;
        }

        //是否按顺序领取奖励
        ObscuredString qianDaoDay = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoDay", "ID", NowXuanZhongID, "QianDao_Template");
        ObscuredString qianDaoNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //ObscuredString qianDaoNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Pay", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        
        if (qianDaoDay!= (int.Parse(qianDaoNum)+1).ToString()) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_248");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请按顺领取签到奖励!");
            return;
        }

        //获取普通签到和付费签到的奖励
        string qianDao_Com = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDao_Com", "ID", NowXuanZhongID, "QianDao_Template");
        string[] qianDao_Com_List = qianDao_Com.Split(';');

        //获取当前是否签到
        if (int.Parse(qianDaoDay) > int.Parse(qianDaoNum))
        {
            //发送奖励
            for (int i = 0; i < qianDao_Com_List.Length; i++)
            {
                //检测背包是否足够
                if (Game_PublicClassVar.Get_function_Rose.BagNullNum() >= qianDao_Com_List.Length)
                {
                    //记录签到时间
                    int writeValue = int.Parse(qianDaoNum) + 1;
                    if (writeValue >= 31)
                    {
                        writeValue = 0;
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Com", writeValue.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Com_Day", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                    //显示奖励
                    string itemID = qianDao_Com_List[i].Split(',')[0];
                    string itemNum = qianDao_Com_List[i].Split(',')[1];
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID,int.Parse(itemNum), "0", 0, "0", true, "26");

                    //显示已领取的标识
                    Obj_ComReward_LingQuImg.SetActive(true);
                }
                else {

                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_249");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包剩余空间不足!");
                }
            }
        }
        
        else {
            //当前已经签到
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_250");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("签到奖励已经领取!");
        }
        
    }


    //领取付费签到奖励
    public void Btn_LingQuQianDaoReward_Pay()
    {
        if (NowXuanZhongID == "")
        {
            return;
        }

        ObscuredString qianDaoNum_Pay_Day = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Pay_Day", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (qianDaoNum_Pay_Day == "1")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_251");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日赞助签到奖励已领取,请明日再来!");
            return;
        }

        //获取今日是否付费
        ObscuredString dayPayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (dayPayValue == ""|| dayPayValue == "0") {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_252");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("领取失败,每天赞助任意金额即可领取此奖励!");
            return;
        }

        //是否按顺序领取奖励
        ObscuredString qianDaoDay = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoDay", "ID", NowXuanZhongID, "QianDao_Template");
        ObscuredString qianDaoNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Pay", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        /*
        if (qianDaoDay != (int.Parse(qianDaoNum) + 1).ToString())
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请按顺领取签到奖励!");
            return;
        }
        */

        //获取普通签到和付费签到的奖励
        string qianDao_Pay = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDao_Pay", "ID", NowXuanZhongID, "QianDao_Template");

        //如果真身有猩猩则更换礼包    
        string ZuoQiPiFuSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiPiFuSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (ZuoQiPiFuSetStr.Contains("10005"))
        {
            qianDao_Pay = qianDao_Pay.Replace("10000025", "10000082");
        }

        string[] qianDao_Pay_List = qianDao_Pay.Split(';');

        //获取当前是否签到
        //if (int.Parse(qianDaoDay) > int.Parse(qianDaoNum))
        //{

            //检测背包是否足够
            if (Game_PublicClassVar.Get_function_Rose.BagNullNum() >= qianDao_Pay_List.Length)
            {

                //记录签到时间
                int writeValue = int.Parse(qianDaoNum) + 1;
                if (writeValue >= 31)
                {
                    writeValue = 0;
                }

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Pay", writeValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Pay_Day", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                //发送奖励
                for (int i = 0; i < qianDao_Pay_List.Length; i++)
                {
                        //显示奖励
                        string itemID = qianDao_Pay_List[i].Split(',')[0];
                        string itemNum = qianDao_Pay_List[i].Split(',')[1];
                        Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID, int.Parse(itemNum), "0", 0, "0", true, "27");

                        //显示已领取的标识
                        Obj_PayReward_LingQuImg.SetActive(true);
                }

                ObscuredString qianDaoNum_com = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                if (int.Parse(qianDaoNum)> int.Parse(qianDaoNum_com))
                {
                    //有人付费领不到，当签到次数为空的时时候
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Pay", writeValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                }

            }
            else
                {

                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_253");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包剩余空间不足!");
            }
            /*
        }
        else
        {
            //当前已经签到
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_254");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("签到奖励已经领取!");
        }
        */
    }
}
