using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QiTianDengLu : MonoBehaviour {

    public GameObject[] ShowObjList;
    //public GameObject[] now_ShowObjList;
    private int quanZhongValue;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //初始化
    public void ChuShiHua() {

        string saveLingQuValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiTianDengLu", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] saveLingQuValueList = saveLingQuValue.Split(';');
        int activityID = 10001;
        quanZhongValue = 0;
        for (int i = 0; i < ShowObjList.Length; i++)
        {

            //显示活动值
            ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().ActiveID = activityID.ToString();
            ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().UpdateShowStatus = true;

            //表示已经领取显示灰化
            if (saveLingQuValueList[i] == "0")
            {
                ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().IfLingQuStatusStr = "0";
                //显示匹配的随机值
                int randomValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Par_1", "ID", activityID.ToString(), "Activity_Template"));
                quanZhongValue = quanZhongValue + randomValue;
                ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().RandomValue = quanZhongValue;
            }
            else
            {
                ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().IfLingQuStatusStr = "1";
                ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().RandomValue = 0;
            }

            activityID = activityID + 1;
        }
    }

    public void Btn_QianDao() {

        //获取等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv < 8) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_216");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_215"); 
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + "8" + langStrHint_2);
            return;
        }

        //获取奖励
        string qiTianDengLuStatus =  Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiTianDengLuStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (qiTianDengLuStatus == "1") {
            //今日奖励已经领取
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_245");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日奖励已经领取,请明日再来!");
            return;
        }

        //随机权重值
        int nowRandom = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, quanZhongValue);
        string saveValue = "";
        bool savaStatus = false;
        for (int i = 0; i < ShowObjList.Length; i++)
        {
            if(!savaStatus){
                if (ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().IfLingQuStatusStr == "0") {
                    if (nowRandom <= ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().RandomValue)
                    {
                        //获取背包格子
                        int bagNullNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
                        if (bagNullNum < 1)
                        {
                            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_235");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包格子不足！");
                            return;
                        }

                        //发送对应奖励
                        string sendStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Par_2", "ID", ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().ActiveID.ToString(), "Activity_Template");
                        string sendID = sendStr.Split(',')[0];
                        string sendNum = sendStr.Split(',')[1];
                        Game_PublicClassVar.Get_function_Rose.SendRewardToBag(sendID, int.Parse(sendNum),"0",0,"0",true,"8");

                        //记录ID
                        ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().IfLingQuStatusStr = "1";
                        ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().UpdateXuanZhongStatus = true;
                        ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().Obj_YiJingQuImg.SetActive(true);

                        //只能获取一个道具
                        savaStatus = true;
                    }
                }
            }
            saveValue = saveValue + ShowObjList[i].GetComponent<UI_HuoDongDengLuRewardShow>().IfLingQuStatusStr + ";";
        }

        if (saveValue != "") {
            saveValue = saveValue.Substring(0, saveValue.Length - 1);
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QiTianDengLuStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QiTianDengLu", saveValue,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //刷新界面
        ChuShiHua();

        //更新活动界面的货币
        Game_PublicClassVar.Get_function_UI.HuoDongHuoBiUpdate();
    }
}
