using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_HuoDongDaTing : MonoBehaviour {

    public GameObject Obj_YueKaSet;
    public GameObject Obj_XueLieHaoSet;
    public GameObject Obj_DengLuSet;
    public GameObject Obj_ZhiChiZuoZheSet;
    public GameObject Obj_MeiRiRewardSet;

    public GameObject Obj_XuLieHaoID;       //序列号Obj
    public GameObject Obj_XuLieHaoQQ;

    public ObscuredBool IfInitStatus = true;
    public ObscuredBool IfInitZhiChiStatus = true;

    //月卡相关
    public GameObject Obj_YueKa_DayNum;
    public GameObject Obj_YueKa_BuyText;
    public GameObject Obj_YueKa_Buy;
    public GameObject Obj_YueKa_Get;
    public GameObject Obj_YueKaRewardItemObjSet;
    public GameObject Obj_YueKaRewardItemObj;

    //支持作者
    public GameObject Obj_ZhiChiZuoZhe_BuyText;
    public GameObject Obj_ZhiChiZuoZheItemObjSet;
    public GameObject Obj_ZhiChiZuoZheItemObj;
    public GameObject Obj_ZhiChiZuoZheExpObj;
    public GameObject Obj_ZhiChiZuoZheExpProObj;
    public ObscuredString nowZhiChiZuoZheID;
    public GameObject Obj_ZhiChiZuoZheNameObj;
    public GameObject Obj_ZhiChiZuoZheLeft;
    public GameObject Obj_ZhiChiZuoZheRight;
    public GameObject Obj_ZhiChiZuoZheBtn;
    public GameObject Obj_ZhiChiZuoZheLingQuImg;

    //登陆
    public GameObject Obj_DengLuRewardDayObj;
    public GameObject Obj_DengLuRewardDayObjSet;

    //每日奖励
    public GameObject Obj_MeiRiPayValue;

    //七天登录
    public GameObject Obj_QiTianDengLu;
    public GameObject Obj_QiTianBtn;

    //每日礼包
    public GameObject Obj_MeiRiLiBao;

    //等级礼包
    public GameObject Obj_LvLiBao;

    //等级礼包
    public GameObject Obj_LvLiBaoMianFei;

    //游戏充值
    //public GameObject Obj_GamePay;
    public GameObject Obj_RmbStore;
	private GameObject rmbStroeObj;
	public GameObject Obj_RmbStoreSet;

    //货币显示
    public GameObject Obj_HuoBi_Rmb;
    public GameObject Obj_HuoBi_Gold;

    //按钮集合
    public GameObject Obj_Btn_QiRi;
    public GameObject Obj_Btn_MeiRi;
    public GameObject Obj_Btn_ZhouKa;
    public GameObject Obj_Btn_lvLiBao;
    public GameObject Obj_Btn_lvLiBaoMianFei;
    public GameObject Obj_Btn_MaoXian;
    public GameObject Obj_Btn_Pay;

    public GameObject Obj_Btn_QiRi_Text;
    public GameObject Obj_Btn_MeiRi_Text;
    public GameObject Obj_Btn_ZhouKa_Text;
    public GameObject Obj_Btn_lvLiBao_Text;
    public GameObject Obj_Btn_lvLiBaoMianFei_Text;
    public GameObject Obj_Btn_MaoXian_Text;
    public GameObject Obj_Btn_Pay_Text;

    public ObscuredString ZuiHouLiBaoID_Max;
    public ObscuredString ZuiHouLiBaoID_Min;
    private string ZuiHouLiBaoID_MaxBase;

    //战区活动
    public GameObject Obj_ZhanQu_RewardLv;
	public GameObject Obj_ZhanQu_RewardShiLi;
	public GameObject Obj_Btn_ZhanQuRewardLv;
	public GameObject Obj_Btn_ZhanQuRewardShiLi;



	// Use this for initialization
	void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        if (IfInitZhiChiStatus) {
            ZuiHouLiBaoID_Max = "10021";
            ZuiHouLiBaoID_Min = "10001";
            
        }
        ZuiHouLiBaoID_MaxBase = "10021";

        if (IfInitStatus) {

            AllHide();
            //默认显示月卡
            Btn_YueKaSet();

            //更新显示七天登录
            bool showQiTianStatus = false;
            string saveLingQuValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiTianDengLu", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            string[] saveLingQuValueList = saveLingQuValue.Split(';');
            for (int i = 0; i < saveLingQuValueList.Length; i++)
            {
                if (saveLingQuValueList[i] == "0")
                {
                    showQiTianStatus = true;
                    break;
                }
            }
            if (showQiTianStatus)
            {
                //显示七天登录按钮
                Obj_QiTianBtn.SetActive(true);
            }
            else
            {
                //隐藏
                Obj_QiTianBtn.SetActive(false);
            }
        }

        //显示货币
        updateHuoBi();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //显示
    public void Btn_YueKaSet() {
        //清空列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_YueKaRewardItemObjSet);
        AllHide();
        Obj_YueKaSet.SetActive(true);

        //按钮选中显示
        Btn_XuanZhongShow(Obj_Btn_ZhouKa, Obj_Btn_ZhouKa_Text);

        //显示月卡内容
        string yuekaValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YueKa", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        int yuekaValueInt = int.Parse(yuekaValue) - 1;
        //如果月卡状态为0,则置为月卡天数为0
        if (yuekaValueInt < 0) {
            yuekaValueInt = 0;
        }

        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("周卡剩余领取次数");
        Obj_YueKa_DayNum.GetComponent<Text>().text = langStr + "：" + yuekaValueInt.ToString() + "/7";
        if (yuekaValue == "0")
        {
            int buyValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "YueKaBuyValue", "GameMainValue"));
            Obj_YueKa_BuyText.SetActive(true);
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("开启周卡需消耗");
            string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("钻石");
            Obj_YueKa_BuyText.GetComponent<Text>().text = langStr + "：" + buyValue + langStr_2;
            Obj_YueKa_Buy.SetActive(true);
            Obj_YueKa_Get.SetActive(false);
        }
        else {
            Obj_YueKa_Buy.SetActive(false);
            Obj_YueKa_Get.SetActive(true);
            Obj_YueKa_BuyText.SetActive(false);
        }

        string[] rewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "YueKaReward", "GameMainValue").Split(';');
        //显示奖励
        for (int i = 0; i <= rewardStr.Length - 1; i++)
        {
            string[] rewardYueKaStr = rewardStr[i].Split(',');
            GameObject itemObj = (GameObject)Instantiate(Obj_YueKaRewardItemObj);
            itemObj.transform.SetParent(Obj_YueKaRewardItemObjSet.transform);
            itemObj.transform.localPosition = new Vector3(100 * i-100, 0, 0);
            itemObj.transform.localScale = new Vector3(1, 1, 1);
            itemObj.GetComponent<UI_ChouKaItemObj>().ItemID = rewardYueKaStr[0];
            itemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = rewardYueKaStr[1];
        }
    }
    //显示序列号
    public void Btn_XueLieHaoSet()
    {
        AllHide();
        Obj_XueLieHaoSet.SetActive(true);


        //获取加群序列号是否领取
        if (Game_PublicClassVar.Get_function_Rose.ifGetXuLieHao("weijing666"))
        {
            Obj_XuLieHaoQQ.SetActive(false);
        }
        else {
            Obj_XuLieHaoQQ.SetActive(true);
        }
    }

    //显示登陆
    public void Btn_DengLuSet()
    {

        //清空列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_DengLuRewardDayObjSet);
        Obj_YueKaSet.SetActive(false);
        Obj_XueLieHaoSet.SetActive(false);
        Obj_DengLuSet.SetActive(true);
        Obj_ZhiChiZuoZheSet.SetActive(false);
        Obj_MeiRiRewardSet.SetActive(false);

        for (int i = 1; i <= 7; i++) { 
            string dengLuRewardValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DengLu_" + i, "GameMainValue");
            //实例化控件
            GameObject obj = (GameObject)Instantiate(Obj_DengLuRewardDayObj);
            obj.transform.SetParent(Obj_DengLuRewardDayObjSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_HuoDongDaTingDengLu>().DengLuRewardStr = dengLuRewardValue;
            obj.GetComponent<UI_HuoDongDaTingDengLu>().DengLuRewardDay = i.ToString();
        }
    }


    public void Btn_ZhiChiZuoZheSet() {
        nowZhiChiZuoZheID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhiChiZuoZheID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        ShowZhiChiZuoZheBtn();
        Btn_ZhiChiZuoZheShow(nowZhiChiZuoZheID);

        //设置最多多看1页支持作者
        int zhichiIDNum = int.Parse(nowZhiChiZuoZheID);
        zhichiIDNum = zhichiIDNum + 2;
        if (int.Parse(ZuiHouLiBaoID_Max) > zhichiIDNum)
        {
            ZuiHouLiBaoID_Max = zhichiIDNum.ToString();
        }
    }

    public void Btn_ZhiChiZuoZhe_Left()
    {
        if (nowZhiChiZuoZheID == ZuiHouLiBaoID_Min)
        {
            return;
        }
        nowZhiChiZuoZheID = (int.Parse(nowZhiChiZuoZheID) - 1).ToString();
        Btn_ZhiChiZuoZheShow(nowZhiChiZuoZheID);
        ShowZhiChiZuoZheBtn();
    }

    public void Btn_ZhiChiZuoZhe_Right()
    {
        if (nowZhiChiZuoZheID == ZuiHouLiBaoID_Max)
        {
            return;
        }
        nowZhiChiZuoZheID = (int.Parse(nowZhiChiZuoZheID) + 1).ToString();
        Btn_ZhiChiZuoZheShow(nowZhiChiZuoZheID);
        ShowZhiChiZuoZheBtn();
    }

    public void ShowZhiChiZuoZheBtn() {

        Obj_ZhiChiZuoZheLeft.SetActive(true);
        Obj_ZhiChiZuoZheRight.SetActive(true);

        if (nowZhiChiZuoZheID == ZuiHouLiBaoID_Min)
        {
            Obj_ZhiChiZuoZheLeft.SetActive(false);
        }

        if (nowZhiChiZuoZheID == ZuiHouLiBaoID_Max)
        {
            Obj_ZhiChiZuoZheRight.SetActive(false);
        }
    }



    //显示支持作者
    public void Btn_ZhiChiZuoZheShow(string zhiChiZuoZheID)
    {

        Debug.Log("显示列表");
        //清空列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ZhiChiZuoZheItemObjSet);
        AllHide();
        Obj_ZhiChiZuoZheSet.SetActive(true);

        //按钮选中显示
        Btn_XuanZhongShow(Obj_Btn_MaoXian, Obj_Btn_MaoXian_Text);

        //显示名称
        int showLv = int.Parse(zhiChiZuoZheID.Substring(3, 2));
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("级冒险家");
        Obj_ZhiChiZuoZheNameObj.GetComponent<Text>().text = showLv + langStr;

        //读取当前奖励
        //string zhiChiZuoZheID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhiChiZuoZheID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        /*
        if (zhiChiZuoZheID == ZuiHouLiBaoID_Max)
        {
            Game_PublicClassVar.Get_function_UI.GameHint("感谢你为作者做出的赞助,已经够多了,作者在此感谢你！");
            Obj_ZhiChiZuoZheItemObjSet.SetActive(false);
            Obj_ZhiChiZuoZhe_BuyText.GetComponent<Text>().text = "感谢你为作者做出的赞助,已经够多了,作者在此感谢你！";
            return;
        }
        */


        string[] zhiChiZuoZheValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ZhiChiZuoZhe_" + zhiChiZuoZheID, "GameMainValue").Split(';');
        //float nowPayValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        float nowPayValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MaoXianJiaExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));


        //显示奖励
        for (int i = 1; i <= zhiChiZuoZheValue.Length - 1; i++)
        {
            string[] rewardStr = zhiChiZuoZheValue[i].Split(',');
            GameObject itemObj = (GameObject)Instantiate(Obj_ZhiChiZuoZheItemObj);
            itemObj.transform.SetParent(Obj_ZhiChiZuoZheItemObjSet.transform);
            itemObj.transform.localPosition = new Vector3(100 * i, 0, 0);
            itemObj.transform.localScale = new Vector3(1, 1, 1);

            //特殊处理
            if (rewardStr[0] == "10000053") {
                //检测自身坐骑是否激活
                string nowZuoQiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                if (nowZuoQiLv != "" && nowZuoQiLv != null && nowZuoQiLv != "0")
                {
                    rewardStr[0] = "10000052";
                    rewardStr[1] = "10";
                }
            }

            itemObj.GetComponent<UI_ChouKaItemObj>().ItemID = rewardStr[0];
            itemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = rewardStr[1];
        }

        //显示进度值
        Obj_ZhiChiZuoZheExpObj.GetComponent<Text>().text = nowPayValue + "/" + zhiChiZuoZheValue[0];
        Obj_ZhiChiZuoZheExpProObj.GetComponent<Image>().fillAmount =  nowPayValue/float.Parse(zhiChiZuoZheValue[0]);
        
        //显示赞助额度
        float needValue = float.Parse(zhiChiZuoZheValue[0]) - nowPayValue;
        string needValueStr = "";
        if (needValue <= 0)
        {
            needValue = 0;
            needValueStr = needValue.ToString();
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("点击领取作者回馈礼包");
            Obj_ZhiChiZuoZhe_BuyText.GetComponent<Text>().text = langStr;
        }
        else {
            //向下取整显示
            //needValueStr = needValue.ToString("0.0");   //保留小数点后一位
            float showJiaGeValue = needValue / 10;
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("冒险家积分不足");
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("赞助");
            string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("元可直接开启礼包");
            Obj_ZhiChiZuoZhe_BuyText.GetComponent<Text>().text = langStr + "!" + langStr_1 + showJiaGeValue.ToString() + langStr_2;
        }

        //显示按钮
        string nowZhiChiZuoZheID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhiChiZuoZheID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (int.Parse(nowZhiChiZuoZheID) > int.Parse(zhiChiZuoZheID))
        {
            //隐藏按钮
            Obj_ZhiChiZuoZheBtn.SetActive(false);
            Obj_ZhiChiZuoZheLingQuImg.SetActive(true);
        }
        else {
            //显示按钮
            Obj_ZhiChiZuoZheBtn.SetActive(true);
            Obj_ZhiChiZuoZheLingQuImg.SetActive(false);
        }


    }

    //显示每日奖励
    public void Btn_MeiRiRewardSet()
    {
        //清空列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_DengLuRewardDayObjSet);

        AllHide();
        Obj_MeiRiRewardSet.SetActive(true);

        string payValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_8", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("你今日已赞助");
        string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("元");
        string langStr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("此额度每日0点清空");
        Obj_MeiRiPayValue.GetComponent<Text>().text = langStr_1 + payValue + langStr_2 + "，"+ langStr_3;

    }


    //月卡购买
    public void Btn_YueKaBuy() { 
    

        //获取钻石当前值
        int buyValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "YueKaBuyValue", "GameMainValue"));
        //获取当前月卡状态
        string yuekaValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YueKa", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (yuekaValue == "0") { 
            bool openStatus = Game_PublicClassVar.Get_function_Rose.CostRMB(buyValue);
            if(openStatus){
                //开启月卡状态
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YueKa", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                //Game_PublicClassVar.Get_function_UI.GameHint("开启周卡成功!");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_411");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

                //更新UI显示状态
                Btn_YueKaSet();

                //更新活动界面的货币
                Game_PublicClassVar.Get_function_UI.HuoDongHuoBiUpdate();

                //发送服务器记录消息
                Game_PublicClassVar.Get_function_Rose.ServerMsg_SendMsg("购买周卡成功");
                //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000116, "购买周卡成功");

            }
            else{
                //Game_PublicClassVar.Get_function_UI.GameHint("钻石不足!");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_399");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            }
        }
        
    }


    //月卡领取
    public void Btn_YueKaGet() {

        //查看月卡进入是否领取
        string yueKaDayStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YueKaDayStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (yueKaDayStatus == "1") {
            //Game_PublicClassVar.Get_function_UI.GameHint("今日周卡已经领取");
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_412");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            return;
        }

        //获取当前月卡状态
        string yuekaValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YueKa", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (yuekaValue != "0")
        {
            //开启月卡状态
            int yuekaNum = int.Parse(yuekaValue) + 1;
            //如果月卡>=7天则重置其状态
            if (yuekaNum > 7) {
                yuekaNum = 0;
                Debug.Log("yuekaNum1111 = " + yuekaNum);
            }

            //发送月卡奖励
            string[] rewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "YueKaReward", "GameMainValue").Split(';');
            //检测背包格子
            if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(rewardStr.Length))
            {
                Debug.Log("yuekaNum = " + yuekaNum);
                //写入月卡每日领取数据
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YueKaDayStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YueKa", yuekaNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

                //发送奖励
                for (int i = 0; i <= rewardStr.Length - 1; i++)
                {
                    string[] rewardYueKaStr = rewardStr[i].Split(',');
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardYueKaStr[0], int.Parse(rewardYueKaStr[1]),"0",0,"0",true,"16");
                }

                //更新UI显示状态
                Btn_YueKaSet();

                //更新活动界面的货币
                Game_PublicClassVar.Get_function_UI.HuoDongHuoBiUpdate();
            }
            else {
                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_152");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_153");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + rewardStr.Length + langStrHint_2);
                //Game_PublicClassVar.Get_function_UI.GameHint("背包请预留至少" + rewardStr.Length +"个位置!");
            }
        }
    }

    //序列号按钮
    public void Btn_XuelieHao() {
        string xulieHaoID = Obj_XuLieHaoID.GetComponent<InputField>().text;
        //Debug.Log("输入值:" + xulieHaoID);
        if (xulieHaoID == "") {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_413");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("请输入序列号!");
            return;
        }
        if (Game_PublicClassVar.Get_function_Rose.ifGetXuLieHao(xulieHaoID))
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_414");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("序列号已被领取!");
            return;
        }

        switch (xulieHaoID) { 
            case "weijing666":
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_415");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameHint("领取组织福利成功,奖励已发往背包,请点击查看！");
                //检测背包格子
                if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(4))
                {

                    //写入序列号ID
                    Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示

                    //Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010061", 5);     //发送经验卷轴5个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010071", 5);       //遗失的金币袋子
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010011", 20);      //发送小型止血药20个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010082", 2);       //发送繁荣度勋章2个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010102", 1);       //发送僵尸护符1个
                    
                    //更新UI显示状态
                    Btn_XueLieHaoSet();
                }

                break;

            case "zuozhezhenshuai":
                langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_415");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameHint("领取组织福利成功,奖励已发往背包,请点击查看！");

                int roseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                if (roseLv >= 12) {
                    //检测背包格子
                    if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1))
                    {
                        //写入序列号ID
                        Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                        Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示

                        Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010041", 5);       //经验木桩
                        Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010026", 1);       //BOSS冷却卷轴
                        //更新UI显示状态
                        Btn_XueLieHaoSet();
                    }
                }

                break;

            case "weijingweibo":
                langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_415");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameHint("领取组织福利成功,奖励已发往背包,请点击查看！");

                roseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

                //检测背包格子
                if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1))
                {
                    //写入序列号ID
                    Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010041", 10);       //经验木桩
                    //更新UI显示状态
                    Btn_XueLieHaoSet();
                }

                break;

            case "20180405":

                Game_PublicClassVar.Get_function_UI.GameHint("我曾踏月而来,只因你在山中");
                
                break;


            case "wangzirui":

                //Game_PublicClassVar.Get_function_UI.GameHint("领取组织福利成功,奖励已发往背包,请点击查看！");

                DateTime oldDate = new DateTime(2018, 4, 5);
                DateTime newDate = DateTime.Now;
                TimeSpan ts = newDate - oldDate;
                int differenceInDays = ts.Days;
                Game_PublicClassVar.Get_function_UI.GameHint("今天是我们在一起的第" + differenceInDays + "天");
                /*

                    //检测背包格子
                    if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1))
                    {
                        //写入序列号ID
                        Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                        Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示

                        //Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010041", 5);       //经验木桩
                        Game_PublicClassVar.Get_function_Rose.SendRewardToBag("3", 500);       //500钻石
                        //更新UI显示状态
                        Btn_XueLieHaoSet();
                    }
                */
                break;

            case "shouji":
                Game_PublicClassVar.Get_function_Rose.JianCeShouJi();
            break;
                

            case "log":
                GameObject logObj = (GameObject)Resources.Load("UGUI/UISet/Other/UI_ErrorLog", typeof(GameObject));
                GameObject errorLogObj = (GameObject)Instantiate(logObj);
                errorLogObj.transform.SetParent(GameObject.Find("Canvas").transform);
                errorLogObj.transform.localPosition = Vector3.zero;
                errorLogObj.transform.localScale = new Vector3(1, 1, 1);
                break;

            case "chongwu":
                
                //获得当前充值额度
                float rosePayValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                if (rosePayValue < 30)
                {
                    Game_PublicClassVar.Get_function_UI.GameHint("领取宠物失败,赞助额度不足30！");
                    return;
                }

                //检测背包格子
                if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1))
                {
                    //写入序列号ID
                    Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010077", 1);       //遗失的金币袋子
                    //更新UI显示状态
                    Btn_XueLieHaoSet();
                    Game_PublicClassVar.Get_function_UI.GameHint("领取宠物成功,祝你游戏愉快！");
                }
                else {
                    Game_PublicClassVar.Get_function_UI.GameHint("请预留1个背包格子！");
                }
                break;

            default:

                //验证密齿
                string xuliehao = Game_PublicClassVar.Get_xmlScript.costStr(xulieHaoID);
                Debug.Log("xuliehao = " + xuliehao);
                if (xulieHaoID != "-1")
                {
                    bool sendReward = false;
                    string[] xuliehaoList = xuliehao.Split(';');
                    if (xuliehaoList.Length < 3) {
                        langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_416");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameHint("序列号不正确!");
                        return;
                    }
                    //验证序列号
                    sendReward = Game_PublicClassVar.Get_function_Rose.IfTrueXuLieHao(xuliehaoList);

                    if (!sendReward)
                    {
                        langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_417");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameHint("序列号密匙不正确!");
                        return;
                    }
                    switch (xuliehaoList[1]) { 
                        //发送道具
                        case "101":
                            

                            //检测背包格子
                            if (!Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1)) {
                                langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_201");
                                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                                //Game_PublicClassVar.Get_function_UI.GameHint("请背包预留至少1个位置!");
                                return;
                            }

                            //获取发送道具
                            if (sendReward)
                            {
                                string[] sendItem = xuliehaoList[2].Split(',');
                                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(sendItem[0], int.Parse(sendItem[1]),"0",0,"0",true,"33");
                                //写入序列号ID
                                Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                            }
                            else {
                                langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_418");
                                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                                //Game_PublicClassVar.Get_function_UI.GameHint("序列号不正确,请检查后在进行输入!");
                            }
                            break;

                        //设置当前关卡
                        case "102":
                            //开启关卡
                            string[] guankaList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_Rose.UpdataPVEChapter(guankaList[0] + ";" + guankaList[1]);
                            Game_PublicClassVar.Get_function_UI.GameHint("开启关卡成功！");
                            break;

                        //充值指定额度(钻石,额度)
                        case "103":
                            //充值指定额度(钻石,额度)
                            string[] payValue = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_Rose.SendRewardToBag("3", int.Parse(payValue[0]));
                            Game_PublicClassVar.Get_function_Rose.AddPayValue(float.Parse(payValue[1]),"33");
                            Game_PublicClassVar.Get_function_UI.GameHint("领取奖励成功！");
                            //写入序列号ID
                            Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                            break;

                        //直接销毁当前身上某个部位的装备
                        case"201":
                            string[] destoryEquip = xuliehaoList[2].Split(',');
                            //Debug.Log("destoryEquip[0] = " + destoryEquip[0] + "destoryEquip[1] = " + destoryEquip[1]);
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipItemID", destoryEquip[1], "ID", destoryEquip[0], "RoseEquip");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipQuality", "0", "ID", destoryEquip[0], "RoseEquip");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIcon", "0", "ID", destoryEquip[0], "RoseEquip");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", destoryEquip[0], "RoseEquip");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");
                            break;

                        //直接设置国家等级
                        case "202":
                            string[] counrtyList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryLv", counrtyList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryExp", counrtyList[1], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                            break;

                        //直接设置离线时间戳
                        case "203":
                            string[] offTimeList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("OffGameTime", offTimeList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryExp", counrtyList[1], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            break;
                        //清空抽卡时间
                        case "204":
                            string[] chouKaList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", chouKaList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", chouKaList[1], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                            break;

                        //清空BOSS刷新时间
                        case "205":
                            //string[] chouKaList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                            break;

                        //设置剧情ID
                        case "206":
                            string[] storyIDList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("StoryStatus", storyIDList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            break;

                        //设置当前任务
                        case "207":
                            string[] taskIDList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskID", taskIDList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", "100,0,0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            break;

                        //清空已完成的任务记录
                        case "208":
                            string[] taskSaveList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CompleteTaskID", taskSaveList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            break;

                        //设置快捷任务显示
                        case "209":
                            string[] maintaskSaveList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainUITaskID", maintaskSaveList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                            break;
                    }
                }
                else {
                    string langStrHint_c = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_416");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_c);
                    //Game_PublicClassVar.Get_function_UI.GameHint("序列号不正确!");
                }

                Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示
                //更新UI显示状态
                Btn_XueLieHaoSet();
                //更新活动界面的货币
                Game_PublicClassVar.Get_function_UI.HuoDongHuoBiUpdate();
                break;
        }
    }

    //点击支持作者按钮
    public void Btn_ZhiChiZuoZhe() {

        string zhiChiZuoZheID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhiChiZuoZheID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        
        if (zhiChiZuoZheID == ZuiHouLiBaoID_MaxBase)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_419");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("感谢你为作者做出的赞助,已经够多了,作者在此感谢你！");
            return;
        }
        
        string[] zhiChiZuoZheValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ZhiChiZuoZhe_" + zhiChiZuoZheID, "GameMainValue").Split(';');
        //float nowPayValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        float nowPayValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MaoXianJiaExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

        //检测背包位置是否足够
        int spaceNullNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
        if (zhiChiZuoZheValue.Length> spaceNullNum) {
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_152");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_153");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + (zhiChiZuoZheValue.Length - 1).ToString() + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameHint("请预留" + (zhiChiZuoZheValue.Length).ToString()+"个背包空位置！");
            return;
        }


        //检测充值额度是否足够
        //Debug.Log("nowPayValue = " + nowPayValue + "||" + "zhiChiZuoZheValue[0] = " + zhiChiZuoZheValue[0]);
        if (nowPayValue >= float.Parse(zhiChiZuoZheValue[0]))
        {
            //循环发送奖励
            for (int i = 1; i <= zhiChiZuoZheValue.Length - 1; i++)
            {
                string[] rewardStr = zhiChiZuoZheValue[i].Split(',');

                //特殊处理
                if (rewardStr[0] == "10000053")
                {
                    //检测自身坐骑是否激活
                    string nowZuoQiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                    if (nowZuoQiLv != "" && nowZuoQiLv != null && nowZuoQiLv != "0")
                    {
                        rewardStr[0] = "10000052";
                        rewardStr[1] = "10";
                    }
                }

                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardStr[0], int.Parse(rewardStr[1]), "0", 0, "0", true, "37");
            }

            //写入礼包数据
            zhiChiZuoZheID = (int.Parse(zhiChiZuoZheID) + 1).ToString();
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhiChiZuoZheID", zhiChiZuoZheID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            Btn_ZhiChiZuoZheShow(zhiChiZuoZheID);     //更新显示

            //更新活动界面的货币
            Game_PublicClassVar.Get_function_UI.HuoDongHuoBiUpdate();
        }
        else {
            string hintTextStr = Obj_ZhiChiZuoZhe_BuyText.GetComponent<Text>().text;
            //Game_PublicClassVar.Get_function_UI.GameHint("赞助金额不足!   " + hintTextStr);
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_420");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + hintTextStr);
        }
    }

    //点击游戏充值
    public void Btn_GamePay() {
        //Debug.Log("ccccc");
        AllHide();
        //按钮选中显示
        Btn_XuanZhongShow(Obj_Btn_Pay, Obj_Btn_Pay_Text);
        //Obj_GamePay.SetActive(true);

		Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_RmbStoreSet);

		if (rmbStroeObj != null) {
			Destroy (rmbStroeObj);
		}
		rmbStroeObj = (GameObject)Instantiate (Obj_RmbStore);
		rmbStroeObj.transform.SetParent(Obj_RmbStoreSet.transform);
		rmbStroeObj.transform.localPosition = Vector3.zero;
		rmbStroeObj.transform.localScale = new Vector3 (1, 1, 1);
    }

    /*
    //点击领取登陆
    public void Btn_DengLu() {
        string dengLuReward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DengLuReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //获取领取状态
        string dengLuDayStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DengLuDayStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (dengLuDayStatus == "1") {
            Game_PublicClassVar.Get_function_UI.GameHint("今日登陆奖励已领取!");
        }

        //获取领取数据
        string[] dengLuRewardValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DengLu_" + dengLuReward, "GameMainValue").Split(';');
        //检测背包位置是否足够
        int spaceNullNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
        if (dengLuRewardValue.Length - 1 > spaceNullNum)
        {
            Game_PublicClassVar.Get_function_UI.GameHint("请预留" + (dengLuRewardValue.Length - 1).ToString() + "个背包空位置！");
            return;
        }

        //发送奖励
        for (int i = 0; i <= dengLuRewardValue.Length - 1; i++) {
            string[] rewardStr = dengLuRewardValue[i].Split(',');
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardStr[0], int.Parse(rewardStr[1]));
        }
    }
    */

    //全部隐藏
    public void AllHide() {
        Obj_YueKaSet.SetActive(false);
        Obj_XueLieHaoSet.SetActive(false);
        Obj_DengLuSet.SetActive(false);
        Obj_ZhiChiZuoZheSet.SetActive(false);
        Obj_MeiRiRewardSet.SetActive(false);
        Obj_QiTianDengLu.SetActive(false);
        Obj_MeiRiLiBao.SetActive(false);
        Obj_LvLiBao.SetActive(false);
        Obj_LvLiBaoMianFei.SetActive(false);

        //if (Obj_GamePay.active) {
        //关闭连接

        //}

        //关闭商城
        //Obj_GamePay.SetActive(false);
        if (rmbStroeObj != null) {
			Destroy (rmbStroeObj);
		}

        //重置按钮状态
        Btn_ChongZhi();
    }

    //七天登录
    public void Btn_QiTianDengLu() {

        AllHide();
        //按钮选中显示
        Btn_XuanZhongShow(Obj_Btn_QiRi, Obj_Btn_QiRi_Text);

        Obj_QiTianDengLu.SetActive(true);
        Obj_QiTianDengLu.GetComponent<UI_QiTianDengLu>().ChuShiHua();
    }

    public void Btn_MeiRiLiBao() {
        AllHide();
        //按钮选中显示
        Btn_XuanZhongShow(Obj_Btn_MeiRi, Obj_Btn_MeiRi_Text);
        Obj_MeiRiLiBao.SetActive(true);
        Obj_MeiRiLiBao.GetComponent<UI_MeiRiLiBao>().ChuShiHua();
    }

    //礼包
    public void Btn_LvLiBao() {
        AllHide();
        //按钮选中显示
        Btn_XuanZhongShow(Obj_Btn_lvLiBao, Obj_Btn_lvLiBao_Text);
        Obj_LvLiBao.SetActive(true);
        Obj_LvLiBao.GetComponent<UI_LvLiBao>().ChuShiHua();
    }

    //免费礼包
    public void Btn_LvLiBaoMianFei()
    {
        AllHide();
        //按钮选中显示
        Btn_XuanZhongShow(Obj_Btn_lvLiBaoMianFei, Obj_Btn_lvLiBaoMianFei_Text);
        Obj_LvLiBaoMianFei.SetActive(true);
        Obj_LvLiBaoMianFei.GetComponent<UI_LvLiBao_MianFei>().ChuShiHua();
    }

    //点击前往充值
    public void GoToPay() {
		/*
        Debug.Log("打开充值界面");
        CloseUI();
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_RmbStore();
		*/
		Btn_GamePay ();
	}

    public void CloseUI() {
        Debug.Log("关闭界面");
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_HuoDongDaTing();
    }

    public void updateHuoBi()
    {
        //显示货币
        int roseRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        long roseGold = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
        Obj_HuoBi_Rmb.GetComponent<Text>().text = roseRmb.ToString();
        Obj_HuoBi_Gold.GetComponent<Text>().text = roseGold.ToString();
    }

    //重置按钮状态
    public void Btn_ChongZhi() {

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_57_1", typeof(Sprite));
        Sprite img = obj as Sprite;

        //重置按钮状态
        Obj_Btn_QiRi.GetComponent<Image>().sprite = img; ;
        Obj_Btn_MeiRi.GetComponent<Image>().sprite = img; ;
        Obj_Btn_ZhouKa.GetComponent<Image>().sprite = img; ;
        Obj_Btn_lvLiBao.GetComponent<Image>().sprite = img; ;
        Obj_Btn_lvLiBaoMianFei.GetComponent<Image>().sprite = img; ;
        Obj_Btn_MaoXian.GetComponent<Image>().sprite = img; ;
        Obj_Btn_Pay.GetComponent<Image>().sprite = img;


        Obj_Btn_QiRi_Text.GetComponent<Text>().color = new Color(0.93f, 0.85f, 0.68f);
        Obj_Btn_MeiRi_Text.GetComponent<Text>().color = new Color(0.93f, 0.85f, 0.68f);
        Obj_Btn_ZhouKa_Text.GetComponent<Text>().color = new Color(0.93f, 0.85f, 0.68f);
        Obj_Btn_lvLiBao_Text.GetComponent<Text>().color = new Color(0.93f, 0.85f, 0.68f);
        Obj_Btn_lvLiBaoMianFei_Text.GetComponent<Text>().color = new Color(0.93f, 0.85f, 0.68f);
        Obj_Btn_MaoXian_Text.GetComponent<Text>().color = new Color(0.93f, 0.85f, 0.68f);
        Obj_Btn_Pay_Text.GetComponent<Text>().color = new Color(0.93f, 0.85f, 0.68f);


    }

    //选中按钮展示
    public void Btn_XuanZhongShow(GameObject obj,GameObject obj_Text) {
        //显示按钮
        object imgObj = Resources.Load("GameUI/" + "Btn/Btn_57_2", typeof(Sprite));
        Sprite img = imgObj as Sprite;
        //重置按钮状态
        obj.GetComponent<Image>().sprite = img;
        obj_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);
    }


	public void Btn_ZhanQuRewardLv()
	{
		ClearnShowBtnUI();
		Obj_ZhanQu_RewardLv.SetActive(true);
	}
	public void Btn_ZhanQuRewardShiLi()
	{
		ClearnShowBtnUI();
		Obj_ZhanQu_RewardShiLi.SetActive(true);
	}

	public void ClearnShowBtnUI() {
		Obj_YueKaSet.SetActive(false);
		Obj_XueLieHaoSet.SetActive(false);
		Obj_DengLuSet.SetActive(false);
		Obj_ZhiChiZuoZheSet.SetActive(false);
		Obj_MeiRiRewardSet.SetActive(false);
		Obj_ZhanQu_RewardLv.SetActive(false);
		Obj_ZhanQu_RewardShiLi.SetActive(false);
	}


    public void Btn_GoToDuiHuanGold()
    {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);

        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_8");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, GoToGoToDuiHuanGold, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否前往兑换金币界面？", GoToGoToDuiHuanGold, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    public void GoToGoToDuiHuanGold()
    {
        CloseUI();
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenGamePaiMaiHang();
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>().IfInitStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>().Btn_DuiHuanSetShow();
    }
}
