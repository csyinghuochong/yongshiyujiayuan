using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_Set : MonoBehaviour {

    public GameObject Obj_UI_TipsSet;
    public GameObject Obj_UI_RoseHp;
    public GameObject Obj_DropItemSet;
    public GameObject Obj_BuildingNameSet;
    public GameObject Obj_NpcNameSet;
    public GameObject Obj_NpcTaskSet;
	public GameObject Obj_RoseGetItemHint;
	public GameObject Obj_UI_BossHp;
	public GameObject Obj_UI_RoseExp;
	public GameObject Obj_UI_GameHintSet;
	public GameObject Obj_UI_GetherItemSet;
    public GameObject Obj_UI_AIHpSet;
    public GameObject Obj_UI_PastureKuangSet;
    public GameObject Obj_MainUI;
    public GameObject Obj_FunctionOpen;
    public GameObject Obj_StorySpeakSet;
    public GameObject Obj_RoseSkillSet;
    public GameObject Obj_BuildingMainUISet;
    public GameObject Obj_BuildingMainUISet_2;
    public GameObject Obj_EnterGame;
    public GameObject Obj_GameGirdHint;
    public GameObject Obj_GameGirdHint_Front;
    public GameObject Obj_BtnCunMinDeXin;
    public GameObject Obj_BuildingHuoBiSet;
    public GameObject Obj_ReturnBuilding;
    public GameObject Obj_CommonHuoBiSet;
    public GameObject Obj_HeadSet;
    public GameObject Obj_RightDownSet;
    public GameObject Obj_MainUIBtn;
    public GameObject Obj_EquipMakeSet;
    public GameObject Obj_MapName;
    public GameObject Obj_UI_CloseTips;
    public GameObject Obj_MainUISet;
    public GameObject Obj_BuildingBGM;
    public GameObject Obj_ShouSuo;
    public GameObject Obj_ShouSuoImg;
    public GameObject Obj_YaoGan;
    public GameObject Obj_RmbStore;
	public GameObject Obj_BuffListShowSet;
	public GameObject Obj_RoseDataSet;

    //public GameObject Obj_RightDownSet;
    public GameObject Obj_MainFunctionUI;
    public GameObject Obj_RoseTask;
    public GameObject Obj_UIMapName;
    public GameObject Obj_UIEnterMapShowName;
    public GameObject Obj_UIGameNanDu;
    public GameObject Obj_UIGameNanDu_Img1;
    public GameObject Obj_UIGameNanDu_Img2;
    public GameObject Obj_UIGameNanDu_Img3;
    public GameObject Obj_UIGameNanDu_Img1_EN;
    public GameObject Obj_UIGameNanDu_Img2_EN;
    public GameObject Obj_UIGameNanDu_Img3_EN;
    public GameObject Obj_UIServerName;
    public bool UpdateServerNameStatus;
    public GameObject Obj_MailHint;             //新邮件提示
    public GameObject Obj_OpenFunctionHint;     //开启新功能提示
    public GameObject Obj_OpenFunctionHintText;
    public GameObject OBJ_WaBaoPro;
    public GameObject Obj_BtnHongBao;
    public GameObject Obj_FengXiangBtn;
    public GameObject Obj_PastrueName;
    public GameObject Obj_PastrueDataShow;
    public GameObject Obj_Btn_JiaYuan;
    public GameObject Obj_PastureModelPositionSet;
    public GameObject Obj_PastureKuangShowName;
    public GameObject Obj_MainRosePastureData;
    public GameObject Obj_FightBtnSet;
    public GameObject Obj_FightPastureBtnSet;
    public GameObject Obj_RoseTargetIconSet;
    //活动按钮相关
    public GameObject Obj_HuoDongBtn_ShouLie;
    public bool HuoDong_ShowLie_YanChiShowStatus;
    public float HuoDong_ShowLie_RewardTime;
    public GameObject HuoDong_ShowLie_BtnTimeShow;

    public GameObject Obj_HuoDongBtn_Tower;
    public GameObject Obj_ShiMingBtn;
    //public bool HuoDong_ShowLie_YanChiShowStatus;
    //public float HuoDong__Tower_RewardTime;
    //public GameObject HuoDong_Tower_BtnTimeShow;

    //适配
    //主界面功能适配
    public GameObject Obj_MainUI_RoseGetItemHint;
    public GameObject Obj_MainUI_UI_BossHp;
    public GameObject Obj_MainUI_UI_RoseExp;
    public GameObject Obj_MainUI_UI_GetherItemSet;
    public GameObject Obj_MainUI_BtnFightingSet;
    public GameObject Obj_MainUI_LeftBtnSet;
    public GameObject Obj_MainUI_RightSet;

    //查看其他玩家角色信息的界面
    public Transform Obj_ShowOtherPlayerWeapon_Posi;        //角色武器模型  
    public GameObject Obj_ShowOtherPlayerWeapon_Model;      //角色武器位置

    //
    public GameObject Obj_AIPropertyShow;                   //AI属性界面

    //新手引导
    public GameObject Obj_GameYinDaoSet;

    //测试信息
    public GameObject Obj_TestXinXi;

    public GameObject Obj_ZuoQiUpDownBtn;

    public GameObject Obj_TimeHintObj;

    //防沉迷相关
    private float SecTimeSum;

    //检测网络
    private bool WangLuoJianCeStatus;

    //
    private bool JianCeFangChenMiStatus;
    //华为查询
    private bool HuaWeiPayChaXunStatus;

    //实力化
    //public GameObject ObjIn_CommonHuoBiSetPosi;

    // Use this for initialization
    void Start () {

        //更新场景名称（只有此脚本在主城才加载，所以临时存放一下,显示主城名称）
        string sceneName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", Application.loadedLevelName, "Scene_Template");
        
        if (Application.loadedLevelName == "EnterGame")
        {
            //sceneName = "圣光小镇";
            //主城隐藏返回按钮
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ReturnBuilding.SetActive(false);
        }
        

        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MapName.GetComponent<Text>().text = sceneName;
        //Debug.Log("sceneName = " + sceneName);

        //显示难度
        Game_PublicClassVar.Get_function_UI.ShowMainUINanDuImg();

        //每次进入主界面请求是否有新邮件
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        if(Game_PublicClassVar.Get_gameLinkServerObj.MailHintStatus == false){
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001042, zhanghaoID);
        }

        //显示等级开启提示
        Game_PublicClassVar.Get_function_Rose.Rose_OpenFunctionHint();

        //如果红包状态开启 就显示
        if (Game_PublicClassVar.Get_gameLinkServerObj.HongBaoStatus) {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnHongBao.SetActive(true);
        }

        //是否谷歌
        if (Game_PublicClassVar.Get_wwwSet.IfGooglePay || EventHandle.IsHuiWeiChannel())
        {
            Obj_FengXiangBtn.SetActive(false);
            Obj_ShiMingBtn.SetActive(false);
        }
        else {
            Obj_FengXiangBtn.SetActive(true);
        }

        //1级账号提示是否开启单机模式
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() == 1) {

            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_39");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_40");
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, null, null,"系统提示", langStrHint_2);
            //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        }

        //显示UI
        if(Game_PublicClassVar.Get_game_PositionVar.GameDanJiStatus == 1)
        if (Application.loadedLevelName == "EnterGame")
        {
            Game_PublicClassVar.gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().PlayerObjSet.SetActive(true);
            //Game_PublicClassVar.Get_function_UI.HintTargetObj(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet, true);
        }


        //隐藏家园,显示任务
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseTask.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainRosePastureData.SetActive(false);

        //检测当前坐骑是否开启
        string nowZuoQiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiPiFuSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowZuoQiID == "" || nowZuoQiID == "0" || nowZuoQiID == null)
        {
            Obj_ZuoQiUpDownBtn.SetActive(false);
        }
        else {
            Obj_ZuoQiUpDownBtn.SetActive(true);
        }

        //验证是否开启狩猎活动
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001900, "");



        //验证魔塔奖励是否领取
        if (Game_PublicClassVar.Get_gameLinkServerObj.HuoDong_Tower_Status == false) {
            //验证是否开启魔塔活动
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002201, "");
            if (Game_PublicClassVar.Get_gameLinkServerObj.HuoDong_Tower_EndShowTime > 10) {
                Obj_HuoDongBtn_Tower.SetActive(true);
            }

            /*
            string cengStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TowerCeng", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            if (cengStr != "" && cengStr != "0" && cengStr != null)
            {
                string TowerCengRewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TowerCengReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                if (TowerCengRewardStr.Contains("Ceng_" + NowCengNum))
                {
                    //已经领取
                    Obj_BtnLingQu.SetActive(false);
                    Obj_ImgYiLingQu.SetActive(true);
                }
            }
            */

        }
        else {
            Obj_HuoDongBtn_Tower.SetActive(true);
        }


        //觉醒提示
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv >= 60) {

            long Rose_Exp = long.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseUpExp", "RoseLv", roseLv.ToString(), "RoseExp_Template"));
            long Rose_ExpNow = long.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

            if (roseLv == 60 && Rose_ExpNow >= Rose_Exp) {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("提示:完成觉醒系列任务开启70级上限!!!");
            }

            //CompleteTaskID   AchievementTaskID
            string nowCompleteTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            string nowAchievementTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            if (nowCompleteTaskID.Contains("33000001") == false && nowAchievementTaskID.Contains("33000001")==false) {

                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
                string langStrHint = "请前往主城的觉醒使者处领取觉醒任务,完成后可开启全新的70级等级上限!";
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, null, null);
                //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
        }

        HuoDong_ShowLie_YanChiShowStatus = Game_PublicClassVar.Get_game_PositionVar.HuoDong_ShowLie_YanChiShowStatus;
        HuoDong_ShowLie_RewardTime = Game_PublicClassVar.Get_game_PositionVar.HuoDong_ShowLie_RewardTime;

        //效验一次文件
        //重新写入验证机制
        Game_PublicClassVar.Get_wwwSet.WriteFileYanZheng();
        //Debug.Log("重新写入效验文件!");

        //初始化弹出一次防沉迷
        if (!EventHandle.IsHuiWeiChannel())
        {
            FangChengMiHint();
        }
       
        //检查电量
        NativeController.Instance.BeginCheckBattery(5);
        //检查加速
        NativeController.Instance.BeginCheckTime(5);

        //判断当前奖励是否已经领取
        string ShiMingRewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMingReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (ShiMingRewardStr == "" || ShiMingRewardStr == "0")
        {
            Obj_ShiMingBtn.SetActive(true);
        }
        else {
            Obj_ShiMingBtn.SetActive(false);
        }

        if (EventHandle.IsHuiWeiChannel()) {
            Obj_ShiMingBtn.SetActive(false);
            //默认申请一次查询订单
            if (HuaWeiPayChaXunStatus == false)
            {
                HuaWeiPayChaXunStatus = true;
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(30000011, "");
            }
        }

        //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(30000011, new Pro_ComStr_4());
    }

    // Update is called once per frame
    void Update () {

        if (HuoDong_ShowLie_YanChiShowStatus == true) {

            if (HuoDong_ShowLie_RewardTime > 0)
            {
                HuoDong_ShowLie_RewardTime = HuoDong_ShowLie_RewardTime - Time.deltaTime;
                Obj_HuoDongBtn_ShouLie.SetActive(true);
                Game_PublicClassVar.Get_game_PositionVar.HuoDong_ShowLie_RewardTime = HuoDong_ShowLie_RewardTime;
            }
            else
            {
                Obj_HuoDongBtn_ShouLie.SetActive(false);
                HuoDong_ShowLie_YanChiShowStatus = false;

                Game_PublicClassVar.Get_game_PositionVar.HuoDong_ShowLie_YanChiShowStatus = false;
                Game_PublicClassVar.Get_game_PositionVar.HuoDong_ShowLie_RewardTime = 0;
            }

        }


        if (Game_PublicClassVar.Get_gameLinkServerObj.HuoDong_ShouLie_Status==true || HuoDong_ShowLie_YanChiShowStatus == true) {

            int HuoDongTime = (int)(Game_PublicClassVar.Get_gameLinkServerObj.HuoDong_ShouLie_Time);
            //检测时间
            if (HuoDongTime >= 60)
            {
                int min = (int)((int)HuoDongTime / 60);
                int sec = (int)HuoDongTime % 60;
                HuoDong_ShowLie_BtnTimeShow.GetComponent<Text>().text = min + "分" + sec + "秒";
            }
            else
            {
                HuoDong_ShowLie_BtnTimeShow.GetComponent<Text>().text = HuoDongTime + "秒";
            }

            if (Game_PublicClassVar.gameLinkServer.HuoDong_ShouLie_Status == false)
            {
                HuoDong_ShowLie_BtnTimeShow.GetComponent<Text>().text = "活动已结束";
            }

        }


        //显示服务器名称
        if (UpdateServerNameStatus == false)
        {

            if (Game_PublicClassVar.Get_wwwSet.ServerName != "" && Game_PublicClassVar.Get_wwwSet.ServerName != "0" && Game_PublicClassVar.Get_wwwSet.ServerName != null)
            {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIServerName.GetComponent<Text>().text = Game_PublicClassVar.Get_wwwSet.ServerName;
                UpdateServerNameStatus = true;
            }
            else
            {
                //未连接服务器
                //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIServerName.GetComponent<Text>().text = "";
            }

        }

        //防沉迷相关,每分钟增加相关时间  Game_PublicClassVar.Get_wwwSet.IfWeiChengNianStatus
        if (Game_PublicClassVar.Get_wwwSet.IfWeiChengNianStatus == true || Game_PublicClassVar.Get_wwwSet.IfYouKeStatus == true || JianCeFangChenMiStatus == true)
        {
            SecTimeSum = SecTimeSum + Time.deltaTime;
            if (SecTimeSum >= 60)
            {
                SecTimeSum = 0;
                int nowTime = PlayerPrefs.GetInt("FangChenMi_Time");          //获取当前时间
                nowTime = nowTime + 1;
                if (nowTime == 30 || nowTime == 45 || nowTime == 60) {
                    FangChengMiHint();      //指定时间进行提示
                }

                PlayerPrefs.SetInt("FangChenMi_Time", nowTime);

                //当前时间不是21点 也进行提示
                if (DateTime.Now.Hour != 21) {
                    FangChengMiHint();
                }
            }
        }

        NativeController.Instance.Update();

        
        if (Game_PublicClassVar.gameLinkServer.FirstLinkStatus == false) {
            if (Game_PublicClassVar.gameLinkServer.ServerLinkStatus)
            {
                //WangLuoJianCeStatus = true;
                Game_PublicClassVar.gameLinkServer.FirstLinkStatus = true;
            }
            else {
                Game_PublicClassVar.Get_wwwSet.Show_YanZhengError("请链接网络后进入游戏!");
            }
        }
        
    }


    public void FangChengMiHint()
    {

        //测试
        //Game_PublicClassVar.Get_gameLinkServerObj.QiangZhiShiMingStatus = true;
        
        if (Game_PublicClassVar.Get_gameLinkServerObj.QiangZhiShiMingStatus == false) {
            //不进行实名认证
            return;
        }

        
        //验证是否满18
        if (PlayerPrefs.GetInt("FangChenMi_Year") >= 18)
        {
            //不进行实名认证
            return;
        }
        

        //string nowTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowDayTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string nowTime = PlayerPrefs.GetInt("FangChenMi_Time").ToString();          //获取当前时间

        if (nowTime == "")
        {
            nowTime = "0";
        }

        int nowTimeSum = int.Parse(nowTime);

        //防沉迷提示
        /*
        if(PlayerPrefs.GetString("FangChenMi_ID") )
        if (PlayerPrefs.GetString("FangChenMi_ID").Length <= 10)
        {
        }
        */

        //验证登陆时间
        DateTime nowData = DateTime.Now;

        /*
        if (Game_PublicClassVar.Get_wwwSet.DataTime == null)
        {
            nowData = DateTime.Now;
        }
        else
        {
            nowData = Game_PublicClassVar.Get_wwwSet.DataTime;
        }
        */

        bool renzhengStatus = false;
        string shenfenIDStr = PlayerPrefs.GetString("FangChenMi_ID");

        if (shenfenIDStr.Length >= 10) {
            renzhengStatus = true;
        }


        // 周五 周六 周日 只给1个小时
        bool hintShiMing = true;
        string dt = DateTime.Today.DayOfWeek.ToString();

        if (dt.Contains("Friday") || dt.Contains("Saturday") || dt.Contains("Sunday"))
        {
            if (DateTime.Now.Hour == 21)
            {
                hintShiMing = false;
                JianCeFangChenMiStatus = true;
            }
        }

        //提示实名
        if (hintShiMing) {

            //弹出对应提示
            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家相关规定,未实名及未成年用户游戏只能在周五 六 日的21:00-22:00进行游戏!", Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Btn_ShiMingReward, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示",  "立即实名", "退出游戏", Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Btn_ShiMingReward);
            uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 24;
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1f, 1f, 1f);
            Game_PublicClassVar.Get_wwwSet.ShiMingHintStatus = true;
            return;

        }

        /*
        if (renzhengStatus == false)
        {
            
            if (nowData.Hour >= 22 || nowData.Hour < 8)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 24;
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,22:00-8:00不能向未成年人提供游戏服务,请在规定时间外登陆游戏,实名后数据将上传服务永久保存!", Game_PublicClassVar.Get_wwwSet.ExitGame, Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, "温馨提示", "确认", "立即实名");
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1f, 1f, 1f);
                return;
            }
            

            if (nowTimeSum < 45)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》,未实名用户游戏体验时间不得超过60分钟,立即实名获得完整的游戏体验,实名后数据将上传服务永久保存!", null, Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, "温馨提示", "稍后实名", "立即实名");
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 24;
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            if (nowTimeSum >= 45 && nowTimeSum < 60)
            {
                if (Obj_TimeHintObj != null)
                {
                    Destroy(Obj_TimeHintObj);
                }

                //弹出对应提示
                Obj_TimeHintObj = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                Obj_TimeHintObj.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》,未实名用户游戏体验时间不得超过60分钟,您已体验" + nowTimeSum + "分钟,立即实名获得完整的游戏体验,实名后数据将上传服务永久保存!", null, Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, "温馨提示", "稍后实名", "立即实名");
                Obj_TimeHintObj.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 24;
                Obj_TimeHintObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                Obj_TimeHintObj.transform.localPosition = Vector3.zero;
                Obj_TimeHintObj.transform.localScale = new Vector3(1, 1, 1);

            }


            if (nowTimeSum >= 60)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,您的体验时间已经结束,继续游戏请进行实名认证,实名后数据将上传服务永久保存!", Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "立即实名", "退出游戏");
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 24;
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1f, 1f, 1f);
            }

        }

        if (renzhengStatus == true)
        {

            //验证登陆时间
            nowData = DateTime.Now;
            if (Game_PublicClassVar.Get_wwwSet.DataTime == null)
            {
                nowData = DateTime.Now;
            }
            else
            {
                nowData = Game_PublicClassVar.Get_wwwSet.DataTime;
            }

            
            if (nowData.Hour >= 22 || nowData.Hour < 8)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 24;
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,22:00-8:00不能向未成年人提供游戏服务,请在规定时间外登陆游戏。", Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "立即认证", "确认");
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            }
            

            if (nowTimeSum == 0)
            {

                if (Obj_TimeHintObj != null)
                {
                    Destroy(Obj_TimeHintObj);
                }
                //弹出对应提示
                Obj_TimeHintObj = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                Obj_TimeHintObj.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,未成年人每日登陆时间不得超过90分钟,您已登陆" + nowTimeSum + "分钟。", null, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "确认", "退出游戏");
                Obj_TimeHintObj.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 24;
                Obj_TimeHintObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                Obj_TimeHintObj.transform.localPosition = Vector3.zero;
                Obj_TimeHintObj.transform.localScale = new Vector3(1f, 1f, 1f);

            }



            if (nowTimeSum >= 60 && nowTimeSum < 90)
            {

                if (Obj_TimeHintObj != null)
                {
                    Destroy(Obj_TimeHintObj);
                }
                //弹出对应提示
                Obj_TimeHintObj = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                Obj_TimeHintObj.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,未成年人每日登陆时间不得超过90分钟,您已登陆" + nowTimeSum + "分钟。", null, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "确认", "退出游戏");
                Obj_TimeHintObj.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 24;
                Obj_TimeHintObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                Obj_TimeHintObj.transform.localPosition = Vector3.zero;
                Obj_TimeHintObj.transform.localScale = new Vector3(1f, 1f, 1f);

            }

            if (nowTimeSum >= 90)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 24;
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,未成年人每日登陆时间不得超过90分钟,您已登陆90分钟,请合理分配游戏时间。", Game_PublicClassVar.Get_wwwSet.ExitGame, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "确认", "确认");
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            }
        }
        */
    }
}
