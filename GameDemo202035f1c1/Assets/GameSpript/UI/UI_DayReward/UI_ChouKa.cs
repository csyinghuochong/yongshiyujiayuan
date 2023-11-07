using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_ChouKa : MonoBehaviour
{
    private ObscuredString takeCard_IDStr;
    private string[] takeCard_ID;
    private ObscuredString countryLv;
    private ObscuredString zuanShiNum;
    private ObscuredString zuanShiNum_Ten;
    private ObscuredString zuanShiNum_Base;
    private ObscuredString zuanShiNum_Ten_Base;
    private ObscuredString dropID;

    public GameObject ChouKaItemSet;
    public GameObject ChouKaItemObj;
    public GameObject ChouKaTitleObj;
    public GameObject ChouKaImgObj;
    public GameObject ChouKaShowItemSet;    //抽卡获得奖励的展示
    public GameObject ChouKaRewardItemObj;  //抽卡奖励展示道具Obj
    public GameObject UI_HuoBiSetObj;
    public GameObject Obj_ChouKaTime_One;
    public GameObject Obj_ChouKaTime_Ten;
    public GameObject Obj_ChouKaCost_One;
    public GameObject Obj_ChouKaCost_Ten;
    public GameObject Obj_CloseShowBtn;
    public GameObject Obj_ChouKaNumRewardSet;
    public GameObject Obj_ChouKaNumRewardShowStr;

    private ObscuredFloat updataTimeSum;
    private ObscuredFloat chouKaTime_One;
    private ObscuredFloat chouKaTime_Ten;
    public ObscuredBool chouKaTime_OneStatus;
    public ObscuredBool chouKaTime_TenStatus;

    public GameObject chouKaShowItemSet;
    public ObscuredString nowChouKaID;

    public GameObject Obj_ChouKaSet;
    public GameObject Obj_ZhangJieXuanZeObj;
    public GameObject Obj_ZhangJieXuanZeTextShow;

    private int nowChouKaNum;

    private float clickTime;
    private bool nowChouKaStatus;

    // Use this for initialization
    void Start()
    {
        chouKaTime_OneStatus = false;
        chouKaTime_TenStatus = false;
        //打开界面初始化抽取数据
        Game_PublicClassVar.Get_game_PositionVar.ChouKaStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.ChouKaStr = "";
        Game_PublicClassVar.Get_game_PositionVar.ChouKaUIOpenStatus = true;

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_ChouKaSet);

        countryLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //抽卡字符串
        takeCard_IDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "TakeCard_ID", "GameMainValue");
        takeCard_ID = takeCard_IDStr.ToString().Split(';');


        dropID = "0";
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        //循环判定掉落
        for (int i = 0; i <= takeCard_ID.Length - 1; i++)
        {
            if (takeCard_ID[i] != "")
            {
                string roseLvLimit = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseLvLimit", "ID", takeCard_ID[i], "TakeCard_Template");
                if (roseLv >= int.Parse(roseLvLimit))
                {
                    dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropID", "ID", takeCard_ID[i], "TakeCard_Template");
                    zuanShiNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuanShiNum", "ID", takeCard_ID[i], "TakeCard_Template");
                    zuanShiNum_Ten = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuanShiNum_Ten", "ID", takeCard_ID[i], "TakeCard_Template");
                    nowChouKaID = takeCard_ID[i];

                    zuanShiNum_Base = zuanShiNum;
                    zuanShiNum_Ten_Base = zuanShiNum_Ten;

                }
            }
        }

        //更新时间
        showOtherTime();

        ShowDropItem();

        //显示当前抽取章节
        ShowZhangJieXuanZeText();

        //更新显示抽卡次数
        ShowChouKaNum();

    }

    // Update is called once per frame
    void Update()
    {

        //每秒更新一次时间
        float aa = Time.realtimeSinceStartup;
        updataTimeSum = updataTimeSum + Time.deltaTime;
        if (updataTimeSum >= 1) {
            updataTimeSum = 0;
            showOtherTime();
        }

        if (Game_PublicClassVar.Get_game_PositionVar.ChouKaStatus) {
            Game_PublicClassVar.Get_game_PositionVar.ChouKaStatus = false;

            //更新货币栏
            UI_HuoBiSetObj.GetComponent<UI_CommonHuoBiSet>().UpdateStatus = true;
            UI_HuoBiSetObj.GetComponent<UI_CommonHuoBiSet>().Obj_Parent = this.gameObject;

            //隐藏道具展示栏
            ChouKaShowItemSet.SetActive(false);

            string[] dropList = Game_PublicClassVar.Get_game_PositionVar.ChouKaStr.Split(';');
            if (dropList.Length >= 10)
            {
                Debug.Log("展示十连抽！");
                //设置单抽标题
                //ChouKaImgObj.SetActive(false);
                Obj_CloseShowBtn.SetActive(false);
                ChouKaTitleObj.SetActive(true);
                chouKaShowItemSet.SetActive(false);
                ChouKaTitleObj.transform.localPosition = new Vector3(0,222,0);
                float show_X = -300;
                float show_Y = 100;
                int showNumHang = 0;
                //10连抽
                for (int i = 0; i <= dropList.Length - 1; i++)
                {
                    if (dropList[i] != "") {
                        showNumHang = showNumHang + 1;
                        //设置十连抽位置
                        string dropItemID = dropList[i].Split(',')[0];
                        string dropItemNum = dropList[i].Split(',')[1];
                        string hideID = dropList[i].Split(',')[2];

                        //实例化道具
                        GameObject chouKaItemObj = (GameObject)Instantiate(ChouKaRewardItemObj);
                        chouKaItemObj.transform.SetParent(ChouKaItemSet.transform);
                        chouKaItemObj.transform.localPosition = new Vector3(show_X, show_Y, 0);
                        chouKaItemObj.transform.localScale = new Vector3(1, 1, 1);
                        chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemID = dropItemID;
                        chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = dropItemNum;
                        chouKaItemObj.GetComponent<UI_ChouKaItemObj>().HindID = hideID;
                        show_X = show_X + 150;
                        //每5个切一行
                        if (showNumHang >= 5)
                        {
                            show_X = -300;
                            showNumHang = 0;
                            show_Y = show_Y - 150;
                        }
                    }
                }
            }
            else { 

                //设置单抽
                //ChouKaImgObj.SetActive(false);
                Obj_CloseShowBtn.SetActive(false);
                ChouKaTitleObj.SetActive(true);
                chouKaShowItemSet.SetActive(false);
                ChouKaTitleObj.transform.localPosition = new Vector3(0,222,0);
                //单抽
                string dropItemID = dropList[0].Split(',')[0];
                string dropItemNum = dropList[0].Split(',')[1];
                string hideID = dropList[0].Split(',')[2];

                GameObject chouKaItemObj = (GameObject)Instantiate(ChouKaRewardItemObj);
                chouKaItemObj.transform.SetParent(ChouKaItemSet.transform);
                chouKaItemObj.transform.localPosition = new Vector3(0, 50, 0);
                chouKaItemObj.transform.localScale = new Vector3(1, 1, 1);
                chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemID = dropItemID;
                chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = dropItemNum;
                chouKaItemObj.GetComponent<UI_ChouKaItemObj>().HindID = hideID;
            }
        }
        //监测抽卡状态开启,读取抽卡字符串进行显示

        if (nowChouKaStatus == true)
        {
            clickTime = clickTime + Time.deltaTime;
            if (clickTime >= 1) {
                clickTime = 0;
                nowChouKaStatus = false;
            }
        }

    }

    void OnDestroy() {
        Game_PublicClassVar.Get_game_PositionVar.ChouKaUIOpenStatus = false;
    }

    public void ShowDropItem() {

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(chouKaShowItemSet);

        //循环展示掉落
        string[] dropItemShowList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropShow", "ID", nowChouKaID, "TakeCard_Template").Split(';');
        int choukaItemHangNum = 0;
        float choukaItem_X = -138;
        float choukaItem_Y = 91;
        for (int i = 0; i <= dropItemShowList.Length - 1; i++)
        {

            choukaItemHangNum = choukaItemHangNum + 1;
            choukaItem_X = choukaItem_X + 60;
            if (choukaItemHangNum > 5)
            {
                choukaItem_X = -78;
                choukaItem_Y = choukaItem_Y - 63.0f;
                choukaItemHangNum = 1;
            }
            string[] chouKaItem = dropItemShowList[i].Split(',');
            GameObject chouKaItemObj = (GameObject)Instantiate(ChouKaItemObj);
            chouKaItemObj.transform.SetParent(chouKaShowItemSet.transform);
            chouKaItemObj.transform.localPosition = new Vector3(choukaItem_X, choukaItem_Y, 0);
            chouKaItemObj.transform.localScale = new Vector3(1, 1, 1);
            chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemID = chouKaItem[0];
            chouKaItemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = chouKaItem[1];
        }
    }

    public void CloseChouKaShow() {
        Debug.Log("关闭抽卡界面！");
        chouKaShowItemSet.SetActive(true);
        Obj_CloseShowBtn.SetActive(true);
        ChouKaTitleObj.SetActive(false);

    }

    void showOtherTime() {

        chouKaTime_One = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_One", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
        chouKaTime_Ten = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_Ten", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));

        //防止出错,防止出现时间很长的情况
        if (chouKaTime_One < 0)
        {
            chouKaTime_One = 86400;
        }
        if (chouKaTime_Ten < 0)
        {
            chouKaTime_Ten = 259200;
        }

        //获取时间
        chouKaTime_One = 86400 - chouKaTime_One;
        chouKaTime_Ten = 259200 - chouKaTime_Ten;

        string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("秒后免费领取");
        string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("当前可免费领取");

        if (chouKaTime_One >= 1)
        {
            //显示倒计时
            int hour = (int)(chouKaTime_One / 3600);
            int hour_value = (int)(chouKaTime_One % 3600);
            int minute = (int)(hour_value / 60);
            int second = (int)(hour_value % 60);
            Obj_ChouKaTime_One.GetComponent<Text>().text = hour + ":" + minute + ":" + second + langStr_1;
            chouKaTime_OneStatus = false;
        }
        else { 
            //显示领取
            Obj_ChouKaTime_One.GetComponent<Text>().text = langStr_2;
            chouKaTime_OneStatus = true;
        }

        if (chouKaTime_Ten >= 1)
        {
            //显示倒计时
            int hour = (int)(chouKaTime_Ten / 3600);
            int hour_value = (int)(chouKaTime_Ten % 3600);
            int minute = (int)(hour_value / 60);
            int second = (int)(hour_value % 60);
            Obj_ChouKaTime_Ten.GetComponent<Text>().text = hour + ":" + minute + ":" + second + langStr_1;
            chouKaTime_TenStatus = false;
        }
        else
        {
            //显示领取
            Obj_ChouKaTime_Ten.GetComponent<Text>().text = langStr_2;
            chouKaTime_TenStatus = true;
        }

    }

    public void Btn_ChouKa() {

        if (nowChouKaStatus == false)
        {
            nowChouKaStatus = true;

            //监测背包
            int bagNum = Game_PublicClassVar.Get_function_UI.BagSpaceNullNum();
            if (bagNum < 1)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_1");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                nowChouKaStatus = false;
                return;
            }

            //8级以下无法免费抽取
            int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
            if (roseLv < 8)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_85");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("8级以下无法抽取奖励！");
                nowChouKaStatus = false;
                return;
            }

            //开启Var内的抽卡状态
            //Debug.Log("点击抽卡按钮");
            Game_PublicClassVar.Get_game_PositionVar.ChouKaStr = "";
            bool chouKaStatus = false;
            bool chouKaStatus_MianFei = false;

            //监测钻石
            if (!chouKaStatus)
            {
                int roseRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
                if (roseRmb >= int.Parse(zuanShiNum))
                {
                    chouKaStatus = true;
                }
                else
                {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_86");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足！");
                }
            }

            //监测免费次数
            if (chouKaTime_OneStatus)
            {
                chouKaStatus = true;
                chouKaTime_OneStatus = false;
                //清空倒计时
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                chouKaStatus_MianFei = true;
            }

            if (chouKaStatus)
            {

                //扣除钻石_免费不扣钻石
                if (!chouKaStatus_MianFei)
                {
                    Game_PublicClassVar.Get_function_Rose.CostReward("2", zuanShiNum);
                }

                //抽卡
                chouKa(1);

                //清空显示
                for (int i = 0; i < ChouKaItemSet.transform.childCount; i++)
                {
                    GameObject go = ChouKaItemSet.transform.GetChild(i).gameObject;
                    Destroy(go);
                }

            }

            nowChouKaStatus = false;
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("点击速度过快,请稍后点击");
        }

    }

    public void Btn_ChouKaTen()
    {
        if (nowChouKaStatus == false)
        {
            nowChouKaStatus = true;

            //检测服务器网络
            if (Game_PublicClassVar.gameLinkServer.ServerLinkStatus == false)
            {
                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_58");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1);
                nowChouKaStatus = false;
                return;
            }

            //监测背包
            int bagNum = Game_PublicClassVar.Get_function_UI.BagSpaceNullNum();
            if (bagNum < 10)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_87");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                nowChouKaStatus = false;
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包已满,请预留至少10个位置！");
                return;
            }

            //开启Var内的抽卡状态
            //Debug.Log("点击抽卡按钮");
            Game_PublicClassVar.Get_game_PositionVar.ChouKaStr = "";

            bool chouKaStatus = false;
            bool chouKaStatus_MianFei = false;
            //监测免费次数
            if (chouKaTime_TenStatus)
            {
                chouKaStatus = true;
                chouKaTime_TenStatus = false;
                chouKaStatus_MianFei = true;
                //清空倒计时
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

            }

            //8级以下无法免费抽奖
            if (chouKaStatus_MianFei)
            {
                int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                if (roseLv < 8)
                {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_113");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("8级以下无法免费抽取！");
                    nowChouKaStatus = false;
                    return;
                }
            }

            //监测钻石
            if (!chouKaStatus)
            {

                int roseRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
                if (roseRmb >= int.Parse(zuanShiNum_Ten))
                {
                    chouKaStatus = true;
                }
                else
                {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_86");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足！");
                }

            }

            if (chouKaStatus)
            {

                //扣除钻石_免费不扣钻石
                if (!chouKaStatus_MianFei)
                {
                    Game_PublicClassVar.Get_function_Rose.CostReward("2", zuanShiNum_Ten);
                }

                //抽卡
                chouKa(10);
                //清空显示
                for (int i = 0; i < ChouKaItemSet.transform.childCount; i++)
                {
                    GameObject go = ChouKaItemSet.transform.GetChild(i).gameObject;
                    Destroy(go);
                }

                //写入成就(十连抽累计)
                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("106", "0", "1");

                if (!chouKaStatus_MianFei)
                {
                    //发送服务器记录消息
                    Game_PublicClassVar.Get_function_Rose.ServerMsg_SendMsg("十连抽");
                    //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000116, "十连抽");
                }

                //每抽5次卡 收集一次数据
                nowChouKaNum = nowChouKaNum + 1;
                if (nowChouKaNum >= 5)
                {
                    nowChouKaNum = 0;

                    string[] saveList = new string[] { "", "2", "预留设备号位置", "2" };
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001006, saveList);
                    //Game_PublicClassVar.Get_wwwSet.UpdatePlayerDataToServer = true;
                }
            }

            nowChouKaStatus = false;
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("点击速度过快,请稍后点击");
        }
    }

    void chouKa(int chouKaNum) {
        //掉落ID
        for (int i = 1; i <= chouKaNum; i++)
        {
            Game_PublicClassVar.Get_function_AI.DropIDToDropItem(dropID, Vector3.zero,"79999999");
        }

        //写入每日任务
        //Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "1", chouKaNum.ToString());
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "3", chouKaNum.ToString());

        //记录抽卡次数
        Game_PublicClassVar.Get_function_Rose.AddChouKaNum(chouKaNum);

        //更新显示抽卡次数
        ShowChouKaNum();
    }

    public void Btn_CloseUI(){
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
        Destroy(this.gameObject);
    }

    //切换抽奖章节
    public void ZhangJieQieHuan(string zhangJie) {

        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

        bool xuanZeStatus = false;
        switch (zhangJie)
        {
            case "1":
                if (roseLv >= 1) {
                    xuanZeStatus = true;
                    nowChouKaID = "1001";
                }
                break;

            case "2":
                if (roseLv >= 18)
                {
                    xuanZeStatus = true;
                    nowChouKaID = "1002";
                }
                break;

            case "3":
                if (roseLv >= 30)
                {
                    xuanZeStatus = true;
                    nowChouKaID = "1003";
                }
                break;

            case "4":
                if (roseLv >= 40)
                {
                    xuanZeStatus = true;
                    nowChouKaID = "1004";
                }
                break;

            case "5":
                if (roseLv >= 50)
                {
                    xuanZeStatus = true;
                    nowChouKaID = "1005";
                }
                break;
        }

        if (xuanZeStatus)
        {
            dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropID", "ID", nowChouKaID, "TakeCard_Template");
            //关闭章节提示
            Close_ZhangJie();
            //显示当前抽取章节
            ShowZhangJieXuanZeText();
            //抽奖显示
            ShowDropItem();

        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_124");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("等级未达到章节要求,无法切换到此章节！");
        }
    }

    public void Close_ZhangJie() {

        if (Obj_ZhangJieXuanZeObj.active == false)
        {
            Obj_ZhangJieXuanZeObj.SetActive(true);
        }
        else {
            Obj_ZhangJieXuanZeObj.SetActive(false);
        }
    }

    void ShowZhangJieXuanZeText() {

        //显示当前抽取章节
        switch (nowChouKaID)
        {

            case "1001":
                Obj_ZhangJieXuanZeTextShow.GetComponent<Text>().text = "第一章探宝";
                break;
            case "1002":
                Obj_ZhangJieXuanZeTextShow.GetComponent<Text>().text = "第二章探宝";
                break;
            case "1003":
                Obj_ZhangJieXuanZeTextShow.GetComponent<Text>().text = "第三章探宝";
                break;
            case "1004":
                Obj_ZhangJieXuanZeTextShow.GetComponent<Text>().text = "第四章探宝";
                break;
            case "1005":
                Obj_ZhangJieXuanZeTextShow.GetComponent<Text>().text = "第五章探宝";
                break;
        }

        Obj_ZhangJieXuanZeTextShow.GetComponent<Text>().text = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(Obj_ZhangJieXuanZeTextShow.GetComponent<Text>().text);

    }

    //打开抽卡奖励
    public void OpenChouKaNumReward() {

        Obj_ChouKaNumRewardSet.SetActive(true);
        Obj_ChouKaNumRewardSet.GetComponent<UI_ChouKaNumRewardSet>().Init();
    }

    //显示当前抽卡次数
    public void ShowChouKaNum() {

        string dayChouKaNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ChouKaNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("今日累计次数");
        Obj_ChouKaNumRewardShowStr.GetComponent<Text>().text = langStr + "：" + dayChouKaNum;

        //更新抽卡消耗显示
        UpdateShowChouKaCost();

    }

    //更新抽卡消耗
    public void UpdateShowChouKaCost() {

        string dayChouKaNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ChouKaNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (dayChouKaNum == "") {
            dayChouKaNum = "0";
        }
        string langStr = "";
        if (int.Parse(dayChouKaNum) >= 250)
        {
            float zhekou = 0.8f;  //抽卡次数大于250次打8折
            zuanShiNum = ((int)(int.Parse(zuanShiNum_Base) * zhekou)).ToString();
            zuanShiNum_Ten = ((int)(int.Parse(zuanShiNum_Ten_Base) * zhekou)).ToString();
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("折");
            langStr = " (" + zhekou * 10 + langStr + ")";
        }

        Obj_ChouKaCost_One.GetComponent<Text>().text = zuanShiNum + langStr;
        Obj_ChouKaCost_Ten.GetComponent<Text>().text = zuanShiNum_Ten + langStr;

    }
}