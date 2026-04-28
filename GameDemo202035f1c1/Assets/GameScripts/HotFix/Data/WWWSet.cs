using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Text;
using System.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CodeStage.AntiCheat.ObscuredTypes;
using System.IO;
#if UNITY_ANDROID
using Umeng;
#endif
using System.Runtime.InteropServices;
using System.Configuration;

using I2.Loc;
using System.IO.Compression;
using System.Security.Cryptography;
using Weijing;
using UnityEngine.AI;

public class WWWSet:MonoBehaviour{

    //读取的XML
    public WWW WWW_Monster_Template;
    public WWW WWW_Drop_Template;
    public WWW WWW_Item_Template;
    public WWW WWW_Skill_Template;
    public WWW WWW_Task_Template;
    public WWW WWW_Npc_Template;
    public WWW WWW_RoseExp_Template;
	public WWW WWW_Equip_Template;
    public WWW WWW_EquipSuit_Template;
    public WWW WWW_EquipSuitProperty_Template;
	public WWW WWW_Occupation_Template;
    public WWW WWW_SceneItem_Template;
    public WWW WWW_SkillBuff_Template;
    public WWW WWW_GameStory_Template;
    public WWW WWW_Scene_Template;
    public WWW WWW_SceneTransfer_Template;
    public WWW WWW_Building_Template;
    public WWW WWW_Chapter_Template;
    public WWW WWW_ChapterSon_Template;
    public WWW WWW_TaskMovePosition_Template;
    public WWW WWW_EquipMake_Template;
    public WWW WWW_GameMainValue;
    public WWW WWW_SpecialEvent_Template;
    public WWW WWW_TaskCountry_Template;
    public WWW WWW_TakeCard_Template;
    public WWW WWW_HonorStore_Template;
    public WWW WWW_Country_Template;
    public WWW WWW_Pet_Template;
    public WWW WWW_Talent_Template;
    public WWW WWW_ShouJiItem_Template;
    public WWW WWW_ShouJiItemPro_Template;
    public WWW WWW_EquipQiangHua_Template;
    public WWW WWW_HintProList_Template;
    public WWW WWW_SceneTanSuo_Template;
    public WWW WWW_Activity_Template;
    public WWW WWW_ChengHao_Template;
    public WWW WWW_Spirit_Template;
    public WWW WWW_ChengJiu_Template;
    public WWW WWW_ChengJiuAll_Template;
    public WWW WWW_ChengJiuReward_Template;
    public WWW WWW_EquipXiLianDaShi_Template;
    public WWW WWW_QianDao_Template;
    public WWW WWW_LingPai_Template;
    public WWW WWW_PetXiuLian_Template;
    public WWW WWW_Pasture_Template;
    public WWW WWW_PastureUpLv_Template;
    public WWW WWW_PastureDuiHuanStore_Template;
    public WWW WWW_ZuoQi_Template;
    public WWW WWW_ZuoQiNengLi_Template;
    public WWW WWW_ZuoQiShow_Template;
    public WWW WWW_FuBenShangHai_Template;
    public WWW WWW_JueXing_Template;
    public WWW WWW_RanSe_Template;
    public WWW WWW_HuoDong_Tower_Template;
    public WWW WWW_ZhuLing_Template;

    //读写的XML
    public WWW WWWSet_RoseData;
    public WWW WWWSet_RoseBag;
    public WWW WWWSet_RoseEquip;
	public WWW WWWSet_RoseConfig;
    public WWW WWWSet_RoseBuilding;
    public WWW WWWSet_RoseStoreHouse;
    public WWW WWWSet_RoseEquipHideProperty;
    public WWW WWWSet_RoseDayReward;
    public WWW WWWSet_RosePet;
    public WWW WWWSet_RoseOtherData;
    public WWW WWWSet_RoseChengJiu;
    public WWW WWWSet_RosePasture;
    public WWW WWWSet_RosePastureData;
    public WWW WWWSet_RosePastureBag;

    //角色创建的XML
    public WWW WWWSet_GameConfig_1;     //战士初始化
    public WWW WWWSet_GameConfig_2;     //法师初始化
    public WWW WWWSet_GameCreate;

    //路径
    public string Get_XmlPath;
    public string Set_XmlPath;
    public string DeleteAll_XmlPath;
    public string Assets_XmlFileName;       
    public DataSet DataSetXml;          //初始化读取数据
    public DataSet DataWriteXml;        //初始化写入xml

    public bool IfUpdata;               //是否初始化数据
    public string RoseID;
    public ObscuredBool IfAddKey;
    private bool IfUpdateWorldTime;     //是否初始化更新世界时间（测试期间关闭此选项要不会卡）

    private bool updataStatus;
    public int updataNum;               //更新数量
    public int updataNumSum;            //更新数量

    public bool DataUpdataStatus;       //数据更新状态,如果数据读取完毕,打开此开关
    public bool DataUpdataStatusOnce;                    //第一次加载数据时调用此字段为True
    public bool UpdatePlayerDataToServer;                //是否上传数据到游戏服务器

    public bool WorldTimeStatus;            //世界时间状态
    private float wordldTimeStatus;         //如果未连接网络多久间隔自动测试一次网络是否连接
    public bool GameOffResourceStatus;      //离线收益状态, True 表示当前游戏的离线资源已发放
    public DateTime DataTime;               //时间变量
    public DateTime DataTime_Last;          //上一次时间变量
    //public string DataTime_Str;           //时间变量时间戳
    public DateTime LastOffGameTime;        //上一次离开游戏的时间
    private bool lastOffGameTimeStatus;     //获取上一次离线时间的状态
    //public DateTime NowWorldTime;         //获取当前世界时间
    public string enterGameTimeStamp;       //进入游戏第一次打开网络的时间戳
    private float saveOffGameTimeSum;       //保存离线数据累计值
    public bool updataOffGameTimeStatus;    //离线时间
    private float worldTimeOnceSum;         //请求世界时间间隔
    public ObscuredBool DayUpdataStatus;            //第二天凌晨更新
    public bool dayUpdataOne;
    private float updateTime_Min;           //分累计,用来执行每分钟才做

    public float dayUpdataTime;             //第二天剩余时间
    public bool upXmlDataStatus;            //更新游戏数据

    public bool IfSaveXmlStatus;            //是否覆盖当前数据内的XML
    private int xmlVersion;                 //XML存档版本号
    public int UpdateGameID;                //强制更新游戏ID

    private bool IfSendYouMengData;         //是否向友盟发送了数据
    public bool IfHindMainBtn;              //是否隐藏主功能区按钮
    public bool IfUpdataGameWaiGua;
    public bool IfSaveRoseData;             //是否存储游戏数据
    public bool IfSaveGetRoseData;          //是否从存储的数据中获取角色数据

    public bool GameIfRunBack;              //游戏是否运行在后台(true:表示后台 false表示前台)
    public float GameEnterBackTime;         //游戏进入后台时间
    public bool BackEnterGameOnly;          //后台进入前台调用,执行一次
    private int backEnterGameOnlySum;       //后台进入前台调用,技术

    private float getDianLiangTimeSum;      //获取电量时间
    private bool firstEnterGame;            //第一次进入游戏状态
    public bool gameOffResourceStatus;      //第一次发送离线奖励状态
    private bool firstWriteTimeStatus;      //第一次写入离线时间

    public bool UpdateMonsterDeathTimeStatus;   //更新怪物死亡时间

    //public string RoseDeathMuBeiStr;          //角色墓碑

    public WWW WWW_xml;                     //外网有打不开游戏的

    //创建角色相关
    public bool IfChangeOcc;                //更改职业
    public bool IfChangeOccStatus;  
    public string CreateRoseNameStr;        //创建角色名称字符
    public bool CreateRoseStatus;           //创建角色状态
    public int CreateRoseDataNum;
    public string CreateRoseOcc;            //创建职业
    public string NowSelectFileName;        //当前选择的角色文件名称

    //游戏暂存信息
    public string RoseChengJiuSetID;        //玩家成就集合


    //其余杂类相关
    public Dictionary<string, float> RosePetZhaoHuanCD = new Dictionary<string, float>();        //角色宠物战斗召唤CD      注意：重启游戏后会重置
    public Dictionary<string, float> RosePetZhaoHuanWuDiCD = new Dictionary<string, float>();     //角色宠物战斗无敌CD      注意：重启游戏后会重置   召唤出战会有一定无敌时间
    public string RoseBuffStr;                                                                //玩家加载场景存储的buff

    //UI类
    public GameObject Obj_BeiFenData;               //备份UI
    public GameObject Obj_CommonHintHint;           //提示UI
    public GameObject Obj_CommonHint_1;             //提示UI
    public GameObject Obj_GameUpdate;               //游戏强制更新

    //网络连接相关
    public bool TryWorldTimeStatus;             //尝试连接网络时间
	public string GameServerVersionStr;         //游戏版本
	public bool TiaoShiStatus;                  //调试状态,打开表示开启调试
	public string QQqunID;                      //QQ群ID
    public string QQLnkStr = "https://qm.qq.com/cgi-bin/qm/qr?k=jEIipUWc01969100WwAkosKOcCG8cN4R&jump_from=webap";      //QQ群超链接
    public string QQErWeiMaStr = "https://qun.qq.com/qrcode/index?data=https%3A%2F%2Fqm.qq.com%2Fcgi-bin%2Fqm%2Fqr%3Fk%3DjEIipUWc01969100WwAkosKOcCG8cN4R%26jump_from%3Dwebapi%26qr%3D1";    //qq群二维码
    public bool IfFengHaoStatus;                //是否修改游戏(服务器接收)
	public string ServerRoseThreadID;           //自身服务器线程ID
	public string ServerID;                     //服务器ID
	public string ServerName;                   //服务器名称
	public string ServerStart;                  //服务器开始时间戳
    public ObscuredString fileSize;             //xml文件大小
    public ObscuredString gameCount;            //文件数量
    public bool SendfileStatus;                 //发送状态
    public ObscuredInt WorldLv;                         //世界等级
    public ObscuredInt WorldPlayerLv;                   //世界等级玩家等级
    public string WorldPlayerName;                      //世界等级玩家名称
    public ObscuredFloat WorldExpProAdd;                //世界等级加成

    //挂机时间验证
    public ObscuredInt guaJiYanZhengTime;
    public bool WriteFileYanZhengStatus;
    public bool WriteFileYanZhengStatusQie;
    public ObscuredBool UpdateXmlNoYanZhengFile;

    //退出游戏
    public ObscuredBool forceExitGameStatus;
    public ObscuredFloat forceExitGameTimeSum;

    //本地初始加载变量
    public ObscuredInt BaoLvID;
    public ObscuredInt BaoLvFailNum;

    //对比DataSet
    public string DataSetXml_BiDui_1_michi;
    public float DataSetXml_BiDuiTime;

    public string DataWriteXml_BiDui_1_michi;
    public ObscuredBool DataWriteXml_BiDui_1_Status;
    public List<string> DataWriteXml_BiDui_1_michi_List;

    public float DataWriteXml_BiDui_1_timeSum;
    private float writeYanZhengTime;
    //public ObscuredBool YanZhengExitGameStatus;

    //充值状态
    public bool GamePayQueryStatus;

    //加速验证
    private string FirstTimeStr;

    //攻击验证
    public int YanZheng_RoseActMaxValue;
    public int YanZheng_PetActMaxValue;
    public bool YanZheng_ActMaxSaveStatus;
    private float YanZheng_ActMaxSaveTime;

    //

    //其他验证
    public ObscuredBool ExitGame_Obs_Status;
    public string YanZheng_AddGold;
    public string YanZheng_AddZuanShi;
    public string YanZheng_CostGold;
    public string YanZheng_CostZuanShi;
    public Pro_SheBeiData PlayerSheBeiData;
    public ObscuredString GameMD5Str;
    public ObscuredString IfRootStatus;
    public ObscuredString CheckApkNameStr;
    public ObscuredString CheckApkNameStrAdd;
    public ObscuredBool GameRootStatus;
    public ObscuredBool RootIfExitGame;
    public ObscuredBool XmlYanZhengPassStatus;
    public ObscuredBool RootShiQuExitGame;
    //private ObscuredInt offTiem;

    //找回角色
    public bool FindRoseStatus;
    public ObscuredString FindRoseZhangHaoID;
    public ObscuredString FindRosePassword;
    public bool QiangZhiDownRose;
    public bool StartFindRoseStatus;

    public ObscuredString NowZhangHaoID;

    //地图加载
    public bool MapEnterStatus;
    public float MapEnterTimeSum;

    //本地化
    public SetLanguage GameSetLanguage;         //本地化组件
    public ObscuredBool IfGooglePay;
    public GameObject Obj_RenZhengSet;
    public GameObject RenZhengSetObj;

    //打印log
    public StringBuilder GameTiaoshiStr = new StringBuilder();
    public int LogInt;
    public bool logStatus;
    public bool logNoKeyStatus;
    public bool logStatus_HuiDang;
    public StringBuilder GameTiaoshiStr_NoKey = new StringBuilder();
    public ObscuredBool GengHuanMiChiStatus;

    //防沉迷相关
    public bool IfWeiChengNianStatus;               //是否未成年
    public bool IfYouKeStatus;                        //游客状态
    public bool ShiMingHintStatus;

    public ObscuredBool useNewArchives;             //使用新存档

    public string EnterGameTimeStamp;               //进入时间戳

    void Awake() {


        string name = PlayerPrefs.GetString("FangChenMi_Name");
        string shenfenID = PlayerPrefs.GetString("FangChenMi_ID");

        Debug.Log("namestartwww = " + name + " shenfenID = " + shenfenID);

        //获取第一次进入游戏的时间戳
        if (FirstTimeStr == "" || FirstTimeStr == null) {
            FirstTimeStr = GetTimeStamp();
        }

        //重置状态
        Game_PublicClassVar.Get_wwwSet = this.GetComponent<WWWSet>();
        Game_PublicClassVar.gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
        Game_PublicClassVar.function_AI = Game_PublicClassVar.Get_wwwSet.GetComponent<Function_AI>();
        Game_PublicClassVar.gameLinkServerObj = GameObject.FindWithTag("Tag_WWWSet");
        Game_PublicClassVar.gameLinkServer = Game_PublicClassVar.gameLinkServerObj.GetComponent<GameLinkServer>();
        Game_PublicClassVar.gameServerMessageObj = GameObject.FindWithTag("Tag_WWWSet");
        Game_PublicClassVar.gameServerMessage = Game_PublicClassVar.gameServerMessageObj.GetComponent<GameServerMessage>();
        Game_PublicClassVar.tag_WWWSet = GameObject.FindWithTag("Tag_WWWSet");
        Game_PublicClassVar.gameServerObj = Game_PublicClassVar.tag_WWWSet.GetComponent<GameServerObj>();
        Game_PublicClassVar.gameGetSignature = Game_PublicClassVar.tag_WWWSet.GetComponent<GetSignature>();
        Game_PublicClassVar.Var_XmlScript = null;
        Game_PublicClassVar.function_DataSet = null;
        Game_PublicClassVar.function_UI = null;
        Game_PublicClassVar.fight_Formult = null;
        Game_PublicClassVar.function_Rose = null;
        Game_PublicClassVar.function_AI = null;
        Game_PublicClassVar.function_task = null;
        Game_PublicClassVar.function_skill = null;
        Game_PublicClassVar.function_monsterSkill = null;
        Game_PublicClassVar.function_Building = null;
        Game_PublicClassVar.function_Country = null;

        //设置屏幕自动旋转， 并置支持的方向
        Screen.orientation = ScreenOrientation.AutoRotation;
        //Screen.autorotateToLandscapeLeft = true;
        //Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        logStatus = false;              //log状态开启
        logNoKeyStatus = false;

#if UNITY_EDITOR
        IfAddKey = false;               //默认False不加密文件
		IfUpdateWorldTime = false;      //关闭此选项,不会链接网络 要不会卡
		TiaoShiStatus = false;          //开启调试状态,正式打包要关闭此状态(打包调整为:False)
#else
        IfAddKey = true;                //默认False不加密文件
        IfUpdateWorldTime = true;       //关闭此选项,不会链接网络 要不会卡
		TiaoShiStatus = false;          //调试状态
#endif

        if (IfAddKey)
        {
            Assets_XmlFileName = "Xml_JM";
        }
        else {
            //Assets_XmlFileName = "Xml";
            Assets_XmlFileName = "../../GameXml/Xml";
        }

        RoseID = "10001";
        dayUpdataTime = 86400;
        wordldTimeStatus = 4;
        Get_XmlPath = Application.persistentDataPath + "/GameData/Xml/Get_Xml/";
        Set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + RoseID + "/";
        DeleteAll_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/";

        Debug.Log("Set_XmlPath = " + Set_XmlPath);
        QQqunID = "930374250";

        DontDestroyOnLoad(this.gameObject);

        UpdateGameID = 1;
        xmlVersion = 522;                                       //只要此值比之前存储的值大就会覆盖XML数据（角色数据不会覆盖）
        WorldLv = 20;                                           //默认世界等级

        //默认设置语言
        PlayerPrefs.SetString("GameLanguageType", "0");         //0表示中文 1表示英文个

        //默认设置是否谷歌
        IfGooglePay = false;                                    //此值为谷歌服,false为国服
        if (IfGooglePay) {
            PlayerPrefs.SetString("GameLanguageType", "1");     //0表示中文 1表示英文
        }

        int xmlVersionNum = PlayerPrefs.GetInt("XmlVersionNum_WeiJing2");
        if (xmlVersion > xmlVersionNum)
        {
            Debug.Log("开始覆盖更新数据");
            //PlayerPrefs.SetInt("XmlVersionNum_WeiJing2", xmlVersion);
            //PlayerPrefs.Save();
            IfSaveXmlStatus = true; //是否覆盖配置文件

            //初始化日志
            PlayerPrefs.SetInt("LogNum", 0);
            PlayerPrefs.Save();

            //初始化不进来
            if (xmlVersionNum <= 433 && xmlVersionNum>=10) {
                //GengHuanMiChiStatus = true;
                ClearnGengXinKey();
            }
        }

        PlayerPrefs.SetString("ChangeKey", "0");         //0表示中文 1表示英文个

        //Debug.Log("时区：" + TimeZoneInfo.Local.ToString());

        //设置程序后台执行
        //Application.runInBackground = true;

        Debug.Log("开始缓存读取的数据表");    //必须加这个Debug  要不读取文件会报错,原因不明
        //Game_PublicClassVar.Get_function_DataSet.DataSet_AllReadXml()

        //协同加载时间状态
        //this.StartCoroutine(LoadWorldTime());
        
        //WorldTimeStatus = true;

        //加载数据

        this.StartCoroutine(Set_GameCreate());

        //展示公告
        //Show_GameGongGao("本次篝火测试须知！\n1.本次篝火测试需要联网才可以进入游戏！(正式测试后单机也可以进入游戏!)\n2.本次篝火测试时间为11.30-12.7日,期间可以正常登陆游戏,此时间超过后将无法登陆游戏,但是游戏存档会予以保留。");

        //初始化设置语言
        string typeStr = PlayerPrefs.GetString("GameLanguageType");
        if (typeStr == "")
        {
            typeStr = "0";          //初始化0是中文，1是英文
        }

        //设置本地语言类型
        switch (typeStr) {
            case "0":
                //设置中文
                Game_PublicClassVar.Get_gameSettingLanguge.SetLanguage("Chinese");
                Debug.Log("默认中文版本");
                break;

            case "1":
                //设置英文
                Game_PublicClassVar.Get_gameSettingLanguge.SetLanguage("English");
                Debug.Log("默认英文版本");
                break;
        }

        //初始化报错验证
        PlayerPrefs.GetString("RootStatusError","0");
        //Debug.Log("版本 = " + UnityEditor.PlayerSettings.Android.bundleVersionCode.ToString());
        LogInt =  PlayerPrefs.GetInt("LogNum", 0);
        LogInt = LogInt + 1;
        PlayerPrefs.SetInt("LogNum", LogInt);
        PlayerPrefs.Save();

        //默认设置为30帧
        PlayerPrefs.SetString("GameHuaMian", "0");
        Application.targetFrameRate = 30;

        DataWriteXml_BiDui_1_michi_List = new List<string>();
        useNewArchives = IfAddKey;

        //PlayerPrefs.SetString("FangChenMi_Name",null);
        //PlayerPrefs.SetString("FangChenMi_ID", null);
        //PlayerPrefs.SetString("FangChenMi_Year", null);
        //PlayerPrefs.SetString("YinSi", null);


    }

	void Start () {

        /*
        ZhuXiaoZhangHaoObj.SetActive(false);
        if (Application.platform == RuntimePlatform.Android) {
            ZhuXiaoZhangHaoObj.SetActive(false);
        }
        */
        //配置表总数
        updataNum = 69;

        if(!EventHandle.IsHuiWeiChannel())
            Game_PublicClassVar.Get_getSignature.excuteCheckAction();

        //Game_PublicClassVar.Get_getSignature.ReqGetChannel();
        
        //Game_PublicClassVar.gameLinkServer.IfEnterGameStatus = true;
        //Game_PublicClassVar.gameLinkServer.ServerLinkStopSendStatus = true;
        //Game_PublicClassVar.gameServerObj.OpenLinkStatus = true;
        
    }


    // Update is called once per frame
    void Update()
    {

        //Game_PublicClassVar.gameLinkServer.ServerLinkStatus = true;

        /*
        if (DataSetXml_BiDui_1_michi != "") {
            DataSetXml_BiDuiTime = DataSetXml_BiDuiTime + Time.deltaTime;
            if (DataSetXml_BiDuiTime>=10) {
                Debug.Log("开始比对...");
                DataSetXml_BiDuiTime = 0;
                DataSetYanZheng();
            }
        }
        */

        /*
        Debug.Log("Time.deltaTime = " + Time.deltaTime);
        if (Time.deltaTime >= 1) {
            Debug.LogError("检测到游戏时间有异常:" + Time.deltaTime);
        }
        */

        /*
        if (Time.timeScale == 1) {
            NativeController.Instance.CheckFpsTime();
            //NativeController.Instance.LoseFocusTime = Time.realtimeSinceStartup;
        }

        NativeController.Instance.CheckFpsTimeSet();
        */

        GameTimerManager.Instance.Execute();

        //测试加载10002
        if (IfChangeOcc == true) {
            IfChangeOcc = false;
            IfChangeOccStatus = true;
            //清空数据
            //RoseID = "10002";
            upXmlDataStatus = false;
            updataNumSum = 0;
            Game_PublicClassVar.Get_game_PositionVar.RoseID = RoseID;
            //Set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + RoseID + "/";
            Set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + NowSelectFileName + "/";
        }

        //加载不加密的数据
        if (CreateRoseDataNum == 0) {
            this.StartCoroutine(Game_PublicClassVar.Get_wwwSet.Set_GameConfig());
        }

        if (IfChangeOccStatus)
        {
            //开始协同
            if (!upXmlDataStatus)
            {
                upXmlDataStatus = true;        //更新下一个数据表
                //Debug.Log("updataNumSum = " + updataNumSum);
                switch (updataNumSum)
                {
                    case 0:
                        this.StartCoroutine(LoadMonster_Template());
                        break;
                    case 1:
                        this.StartCoroutine(LoadDrop_Template());
                        break;
                    case 2:
                        this.StartCoroutine(LoadItem_Template());
                        break;
                    case 3:
                        this.StartCoroutine(LoadSkill_Template());
                        break;
                    case 4:
                        this.StartCoroutine(LoadTask_Template());
                        break;
                    case 5:
                        this.StartCoroutine(LoadNpc_Template());
                        break;
                    case 6:
                        this.StartCoroutine(LoadRoseExp_Template());
                        break;
                    case 7:
                        this.StartCoroutine(LoadEquip_Template());
                        break;
                    case 8:
                        this.StartCoroutine(LoadEquipSuit_Template());
                        break;
                    case 9:
                        this.StartCoroutine(LoadEquipSuitProperty_Template());
                        break;
                    case 10:
                        this.StartCoroutine(LoadOccupation_Template());
                        break;
                    case 11:
                        this.StartCoroutine(LoadSceneItem_Template());
                        break;
                    case 12:
                        this.StartCoroutine(LoadSkillBuff_Template());
                        break;
                    case 13:
                        this.StartCoroutine(LoadGameStory_Template());
                        break;
                    case 14:
                        this.StartCoroutine(LoadScene_Template());
                        break;
                    case 15:
                        this.StartCoroutine(LoadSceneTransfer_Template());
                        break;
                    case 16:
                        this.StartCoroutine(LoadBuilding_Template());
                        break;
                    case 17:
                        this.StartCoroutine(LoadChapter_Template());
                        break;
                    case 18:
                        this.StartCoroutine(LoadChapterSon_Template());
                        break;
                    case 19:
                        this.StartCoroutine(LoadTaskMovePosition_Template());
                        break;
                    case 20:
                        this.StartCoroutine(LoadEquipMake_Template());
                        break;
                    case 21:
                        this.StartCoroutine(LoadGameMainValue());
                        break;
                    case 22:
                        this.StartCoroutine(LoadSpecialEvent_Template());
                        break;
                    case 23:
                        this.StartCoroutine(LoadTaskCountry_Template());
                        break;
                    case 24:
                        this.StartCoroutine(LoadTakeCard_Template());
                        break;
                    case 25:
                        this.StartCoroutine(LoadHonorStore_Template());
                        break;
                    case 26:
                        this.StartCoroutine(LoadCountry_Template());
                        break;
                    //开始添加能记录的XML文件
                    case 27:
                        this.StartCoroutine(Set_RoseData());
                        break;
                    case 28:
                        this.StartCoroutine(Set_RoseBag());
                        break;
                    case 29:
                        this.StartCoroutine(Set_RoseEquip());
                        break;
                    case 30:
                        this.StartCoroutine(Set_RoseConfig());
                        break;
                    case 31:
                        this.StartCoroutine(Set_RoseBuilding());
                        break;
                    case 32:
                        this.StartCoroutine(Set_RoseStoreHouse());
                        break;
                    case 33:
                        this.StartCoroutine(Set_RoseEquipHideProperty());
                        break;
                    case 34:
                        this.StartCoroutine(Set_RoseDayReward());
                        break;
                    case 35:
                        this.StartCoroutine(Set_RosePet());
                        break;
                    case 36:
                        this.StartCoroutine(LoadPet_Template());
                        break;
                    case 37:
                        this.StartCoroutine(LoadTalent_Template());
                        break;
                    case 38:
                        this.StartCoroutine(LoadShouJiItem_Template());
                        break;
                    case 39:
                        this.StartCoroutine(LoadShouJiItemPro_Template());
                        break;
                    case 40:
                        this.StartCoroutine(LoadEquipQiangHua_Template());
                        break;
                    case 41:
                        this.StartCoroutine(LoadHintProList_Template());
                        break;
                    case 42:
                        this.StartCoroutine(Set_RoseOtherData());
                    break;
                    case 43:
                    this.StartCoroutine(LoadScenceTanSuo_Template());
                    break;

                    case 44:
                    this.StartCoroutine(LoadActivity_Template());
                    break;

                    case 45:
                    this.StartCoroutine(LoadChengHao_Template());
                    break;

                    case 46:
                    this.StartCoroutine(Set_RoseChengJiu());
                    break;

                    case 47:
                    this.StartCoroutine(LoadSpirit_Template());
                    break;
                    case 48:
                    this.StartCoroutine(LoadChengJiu_Template());
                    break;
                    case 49:
                    this.StartCoroutine(LoadChengJiuAll_Template());
                    break;
                    case 50:
                    this.StartCoroutine(LoadChengJiuReward_Template());
                    break;
                    case 51:
                    this.StartCoroutine(LoadEquipXiLianDaShi_Template());
                    break;
                    case 52:
                    this.StartCoroutine(LoadQianDao_Template());
                    break;
                    case 53:
                    this.StartCoroutine(LoadLingPai_Template());
                    break;
                    case 54:
                        this.StartCoroutine(LoadPetXiuLian_Template());
                    break;
                    case 55:
                        this.StartCoroutine(LoadPasture_Template());
                    break;
                    case 56:
                        this.StartCoroutine(Set_RosePasture());
                    break;
                    case 57:
                        this.StartCoroutine(Set_RosePastureData()); 
                    break;

                    case 58:
                        this.StartCoroutine(LoadPastureUpLv_Template());
                    break;

                    case 59:
                        this.StartCoroutine(LoadPastureDuiHuanStore_Template());
                        break;

                    case 60:
                        this.StartCoroutine(Set_RosePastureBag());
                        break;

                    case 61:
                        this.StartCoroutine(LoadZuoQi_Template());
                        break;

                    case 62:
                        this.StartCoroutine(LoadZuoQiNengLi_Template());
                        break;

                    case 63:
                        this.StartCoroutine(Load_ZuoQiShow_Template());
                        break;

                    case 64:
                        this.StartCoroutine(Load_FuBenShangHai_Template());
                        break;

                    case 65:
                        this.StartCoroutine(Load_JueXing_Template());
                        break;

                    case 66:
                        this.StartCoroutine(Load_RanSe_Template());
                        break;

                    case 67:
                        this.StartCoroutine(Load_HuoDong_Tower_Template());
                        break;
                    case 68:
                        this.StartCoroutine(Load_ZhuLing_Template());
                        break;
                }
            }
        }



        //外网有打不开游戏的生成文件
        /*
        this.StartCoroutine(addXmlJiaMi());
        */



        if (!updataStatus)
        {
            //Debug.Log("updataNumSum = " + updataNumSum);
            //if (updataNumSum >= updataNum|| updataNumSum==0)
            if (updataNumSum >= updataNum)
            {
                //Debug.Log("开始缓存所有表,当前updataNumSum值为：" + updataNumSum);
                AddShowLog("开始加载游戏内的数据...");
                //初始化缓存所有表
                if (!IfUpdata)
                {
                    Debug.Log("开始缓存所有数据！！！！！");
                    try
                    {
                        //验证存储文件是否被替换过
                        ObscuredString yanZhengFileStr = PlayerPrefs.GetString("YanZhengFileStr_" + NowSelectFileName);
                        //yanZhengFileStr = "999";
                        //防止意外,次值
                        if (yanZhengFileStr == "999")
                        {
                            XmlYanZhengPassStatus = true;
                        }

                        //GengHuanMiChiStatus = true;
                        Game_PublicClassVar.Get_function_DataSet.DataSet_AllReadXml();

                        if (GengHuanMiChiStatus) {

                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseChengJiu");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquipHideProperty");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseOtherData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePasture");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureBag");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                            PlayerPrefs.SetString("ChangeKey_" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName, "1");        //表示完成

                        }

                        GengHuanMiChiStatus = false;
                        Debug.Log("更新数据完成！");
                        AddShowLog("游戏数据加载完成...");

                        //IfSaveRoseData = true;      //开启储备数据

                        //大于1级账号才开始验证
                        ObscuredBool yanzhengStatus = true;
                        ObscuredBool writeYanZhengStatus = true;

                        /*
                        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() <= 1) {
                            yanzhengStatus = false;
                        }
                        */

                        AddShowLog("获取到文件验证码：" + yanZhengFileStr);

                        /*
                        if (IfSaveXmlStatus) {
                            yanZhengFileStr = "999";
                            AddShowLog("存档文件覆盖..." );
                        }
                        */

                        //防止意外,次值
                        if (yanZhengFileStr == "999") {
                            yanzhengStatus = false;
                            AddShowLog("通过检测数据...");
                        }

#if UNITY_EDITOR
                        //yanzhengStatus = false;
#endif

                        if (yanzhengStatus == false) {

                            //发送服务器效验
                            Pro_ComStr_4 com4 = new Pro_ComStr_4();
                            string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            com4.str_1 = Game_PublicClassVar.Get_xmlScript.Encrypt(zhanghaoID);
                            Debug.Log("AAAAAAAAAAAAAAAAAAA");
                            com4.str_2 = Game_PublicClassVar.Get_xmlScript.Encrypt(SystemInfo.deviceUniqueIdentifier);
                            com4.str_3 = Game_PublicClassVar.Get_xmlScript.Encrypt("TongGuo:" + yanZhengFileStr);
                            com4.str_4 = Game_PublicClassVar.Get_xmlScript.Encrypt(NowSelectFileName);
                            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000153, com4);

                        }


                        try
                        {
                            if (yanzhengStatus)
                            {
                                AddShowLog("正在进行检测数据...");
                                if (yanZhengFileStr != "" && yanZhengFileStr != null)
                                {
                                    //验证文件
                                    string yanZhengFileStr_roseData = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", RoseID, "RoseData");
                                    string yanZhengFileStr_roseBag = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RoseBag");
                                    string yanZhengFileStr_rosePet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RosePet");
                                    string yanZhengFileStr_roseStoreHouse = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RoseStoreHouse");
                                    string yanZhengFileStr_roseEquip = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RoseEquip");

                                    string yanZhengFileStr_rosePastureData = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", RoseID, "RosePastureData");
                                    string yanZhengFileStr_rosePastureBag = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RosePastureBag");
                                    string yanZhengFileStr_rosePasture = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RosePasture");

                                    string yanZhengFileStr_roseChengJiu = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", RoseID, "RoseChengJiu");
                                    string yanZhengFileStr_roseConfig = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr",  "ID", RoseID, "RoseConfig");
                                    string yanZhengFileStr_roseDayReward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", RoseID, "RoseDayReward");

                                    AddShowLog("检测结果..." + "yanZhengFileStr_roseData = " + yanZhengFileStr_roseData + "yanZhengFileStr_roseBag = " + yanZhengFileStr_roseBag + "yanZhengFileStr_rosePet = " + yanZhengFileStr_rosePet + "yanZhengFileStr_roseStoreHouse = " + yanZhengFileStr_roseStoreHouse + "yanZhengFileStr_roseEquip = " + yanZhengFileStr_roseEquip + "yanZhengFileStr_rosePastureData = " + yanZhengFileStr_rosePastureData + "yanZhengFileStr_rosePastureData = " + yanZhengFileStr_rosePastureBag + "yanZhengFileStr_rosePasture = " + yanZhengFileStr_rosePasture + "yanZhengFileStr_roseChengJiu = " + yanZhengFileStr_roseChengJiu + "yanZhengFileStr_roseConfig = " + yanZhengFileStr_roseConfig + "yanZhengFileStr_roseDayReward = " + yanZhengFileStr_roseDayReward);

                                    if (yanZhengFileStr == yanZhengFileStr_roseData && yanZhengFileStr == yanZhengFileStr_roseBag && yanZhengFileStr == yanZhengFileStr_rosePet && yanZhengFileStr == yanZhengFileStr_roseStoreHouse && yanZhengFileStr == yanZhengFileStr_roseEquip && yanZhengFileStr == yanZhengFileStr_rosePastureData && yanZhengFileStr == yanZhengFileStr_rosePastureBag && yanZhengFileStr == yanZhengFileStr_rosePasture && yanZhengFileStr == yanZhengFileStr_roseChengJiu && yanZhengFileStr == yanZhengFileStr_roseConfig && yanZhengFileStr == yanZhengFileStr_roseDayReward)
                                    {
                                        //验证通过
                                        //Debug.Log("验证通过!!!验证不通过!!!验证通过!!!验证通过!!!验证通过!!!");
                                        AddShowLog("验证通过!!!验证通过!!!验证通过!!!验证通过!!!验证通过!!!");
                                    }
                                    else
                                    {

                                        bool NoTongGuo = true;

                                        //特殊情况处理
                                        /*
                                        if (yanZhengFileStr_roseData == "" && yanZhengFileStr_roseBag == "" && yanZhengFileStr_rosePet == "" && yanZhengFileStr_roseStoreHouse == "" && yanZhengFileStr_roseEquip == "")
                                        {
                                            NoTongGuo = false;
                                        }

                                        if (yanZhengFileStr_roseData == "" && yanZhengFileStr_roseBag == "" && yanZhengFileStr_rosePet == "" && yanZhengFileStr_roseStoreHouse == "" && yanZhengFileStr_roseEquip == "")
                                        {
                                            NoTongGuo = false;
                                        }

                                        if ((yanZhengFileStr_rosePastureData == ""|| yanZhengFileStr_rosePastureData == "0" )&& (yanZhengFileStr_rosePastureBag == "" || yanZhengFileStr_rosePastureBag == "0" )&& (yanZhengFileStr_rosePasture == "" || yanZhengFileStr_rosePasture == "0"))
                                        {
                                            //防止删掉上面几个文件篡改
                                            if (yanZhengFileStr == yanZhengFileStr_roseData && yanZhengFileStr == yanZhengFileStr_roseBag && yanZhengFileStr == yanZhengFileStr_rosePet && yanZhengFileStr == yanZhengFileStr_roseStoreHouse && yanZhengFileStr == yanZhengFileStr_roseEquip)
                                            {
                                                NoTongGuo = false;
                                            }
                                        }

                                        if ((yanZhengFileStr_roseChengJiu == "" || yanZhengFileStr_roseChengJiu == "0") && (yanZhengFileStr_roseConfig == "" || yanZhengFileStr_roseConfig == "0") && (yanZhengFileStr_roseDayReward == "" || yanZhengFileStr_roseDayReward == "0"))
                                        {
                                            //防止删掉上面几个文件篡改
                                            if (yanZhengFileStr == yanZhengFileStr_roseData && yanZhengFileStr == yanZhengFileStr_roseBag && yanZhengFileStr == yanZhengFileStr_rosePet && yanZhengFileStr == yanZhengFileStr_roseStoreHouse && yanZhengFileStr == yanZhengFileStr_roseEquip)
                                            {
                                                NoTongGuo = false;
                                            }
                                        }
                                        */
                                        AddShowLog("验证未通过..." + "yanZhengFileStr_roseData = " + yanZhengFileStr_roseData + "yanZhengFileStr_roseBag = " + yanZhengFileStr_roseBag + "yanZhengFileStr_rosePet = " + yanZhengFileStr_rosePet + "yanZhengFileStr_roseStoreHouse = " + yanZhengFileStr_roseStoreHouse + "yanZhengFileStr_roseEquip = " + yanZhengFileStr_roseEquip + "yanZhengFileStr_rosePastureData = " + yanZhengFileStr_rosePastureData + "yanZhengFileStr_rosePastureData = " + yanZhengFileStr_rosePastureBag + "yanZhengFileStr_rosePasture = " + yanZhengFileStr_rosePasture + "yanZhengFileStr_roseChengJiu = " + yanZhengFileStr_roseChengJiu + "yanZhengFileStr_roseConfig = " + yanZhengFileStr_roseConfig + "yanZhengFileStr_roseDayReward = " + yanZhengFileStr_roseDayReward);

                                        //更新的时候是不进行检测文件的
                                        if (UpdateXmlNoYanZhengFile) {


                                            /*
                                            string nowYanZhengFileNameStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileNameStr", "ID", RoseID, "RoseData");

                                            if (yanZhengFileStr_roseChengJiu == "0" || yanZhengFileStr_roseChengJiu == "") {

                                                if (nowYanZhengFileNameStr.Contains("RoseChengJiu") == false) {
                                                    NoTongGuo = false;
                                                    AddShowLog("nowYanZhengFileNameStr!!!" + nowYanZhengFileNameStr);
                                                }
                                            }

                                            if (yanZhengFileStr_roseConfig == "0" || yanZhengFileStr_roseConfig == "")
                                            {

                                                if (nowYanZhengFileNameStr.Contains("RoseConfig") == false)
                                                {
                                                    NoTongGuo = false;
                                                    AddShowLog("yanZhengFileStr_roseConfig!!!" + nowYanZhengFileNameStr);
                                                }

                                            }

                                            if (yanZhengFileStr_roseDayReward == "0" || yanZhengFileStr_roseDayReward == "")
                                            {

                                                if (nowYanZhengFileNameStr.Contains("RoseDayReward") == false)
                                                {
                                                    NoTongGuo = false;
                                                    AddShowLog("yanZhengFileStr_roseDayReward!!!" + nowYanZhengFileNameStr);
                                                }

                                            }

                                            if (yanZhengFileStr_rosePastureData == "0" || yanZhengFileStr_rosePastureData == "")
                                            {

                                                if (nowYanZhengFileNameStr.Contains("RosePastureData") == false)
                                                {
                                                    NoTongGuo = false;
                                                    AddShowLog("yanZhengFileStr_rosePastureData!!!" + nowYanZhengFileNameStr);
                                                }

                                            }

                                            if (yanZhengFileStr_rosePastureBag == "0" || yanZhengFileStr_rosePastureBag == "")
                                            {

                                                if (nowYanZhengFileNameStr.Contains("RosePastureBag") == false)
                                                {
                                                    NoTongGuo = false;
                                                    AddShowLog("yanZhengFileStr_rosePastureBag!!!" + nowYanZhengFileNameStr);
                                                }

                                            }

                                            if (yanZhengFileStr_rosePasture == "0" || yanZhengFileStr_rosePasture == "")
                                            {

                                                if (nowYanZhengFileNameStr.Contains("RosePasture") == false)
                                                {
                                                    NoTongGuo = false;
                                                    AddShowLog("yanZhengFileStr_rosePasture!!!" + nowYanZhengFileNameStr);
                                                }

                                            }
                                            */

                                            /*
                                            //防止删掉上面几个文件篡改
                                            if ((yanZhengFileStr == yanZhengFileStr_roseData) && (yanZhengFileStr == yanZhengFileStr_roseBag) && (yanZhengFileStr == yanZhengFileStr_rosePet) && (yanZhengFileStr == yanZhengFileStr_roseStoreHouse) && (yanZhengFileStr == yanZhengFileStr_roseEquip))
                                            {
                                                NoTongGuo = false;
                                            }
                                            */

                                        }



                                        /*
                                        //本地文件为空表示异常
                                        if (yanZhengFileStr_roseData != "" && yanZhengFileStr_roseBag != "" && yanZhengFileStr_rosePet != "" && yanZhengFileStr_roseStoreHouse != "" && yanZhengFileStr_roseEquip != "") {
                                            NoTongGuo = false;
                                        }
                                        */

                                        /*
                                        //如果本地文件的存储时间大于自身存储,表示不可能修改
                                        if (long.Parse(yanZhengFileStr_roseData) >= long.Parse(yanZhengFileStr)) {
                                            NoTongGuo = false;
                                        }
                                        */

                                        if (NoTongGuo)
                                        {


                                            string fileErrorStr = "文件验证失败！请把原始文件恢复...  如需帮助,请加群联系管理!";

                                            if (yanZhengFileStr == "1001") {
                                                fileErrorStr = "文件验证失效！请把原始文件恢复...  如需帮助,请加群联系管理!";
                                            }



                                            //AddShowLog("检测验证不通过!!!No!!!No!!!No!!!");
                                            //验证不通过
                                            //Debug.Log("检测验证不通过!!!检测验证不通过!!!检测验证不通过!!!检测验证不通过!!!检测验证不通过!!!");
                                            writeYanZhengStatus = false;
                                            Time.timeScale = 0;
                                            //Debug.Log("yanZhengFileStr = " + yanZhengFileStr + ";yanZhengFileStr_roseData = " + yanZhengFileStr_roseData + ";NowSelectFileName = " + NowSelectFileName);
                                            //Show_YanZhengError(yanZhengFileStr +";"+ yanZhengFileStr_roseData + " = " + yanZhengFileStr_rosePastureData  + ";" + yanZhengFileStr_rosePastureBag + ";" + yanZhengFileStr_rosePasture);
                                            Show_YanZhengError(fileErrorStr);

                                            Game_PublicClassVar.Get_wwwSet.forceExitGameStatus = true;
                                            Game_PublicClassVar.Get_wwwSet.forceExitGameTimeSum = 0;

                                            //发送服务器效验
                                            Pro_ComStr_4 com4 = new Pro_ComStr_4();
                                            string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                                            com4.str_1 = Game_PublicClassVar.Get_xmlScript.Encrypt(zhanghaoID);
                                            Debug.Log("AAAAAAAAAAAAAAAAAAA");
                                            com4.str_2 = Game_PublicClassVar.Get_xmlScript.Encrypt(SystemInfo.deviceUniqueIdentifier);
                                            com4.str_3 = Game_PublicClassVar.Get_xmlScript.Encrypt(yanZhengFileStr);
                                            com4.str_4 = Game_PublicClassVar.Get_xmlScript.Encrypt(yanZhengFileStr_roseData +";" + yanZhengFileStr_roseBag + ";" + yanZhengFileStr_rosePet + ";" + yanZhengFileStr_roseStoreHouse + ";" + yanZhengFileStr_roseEquip + ";" + yanZhengFileStr_rosePastureData + ";" + yanZhengFileStr_rosePastureBag + ";" + yanZhengFileStr_rosePasture + ";" + yanZhengFileStr_roseChengJiu + ";" + yanZhengFileStr_roseConfig + ";" + yanZhengFileStr_roseDayReward);
                                            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000153, com4);
                                        }
                                    }
                                }
                                else {

                                    //AddShowLog("初始化文件中....");
                                    //第一次进入判定这些是否为空
                                    //判断初始file文件是否为空
                                    string yanZhengFileStr_roseData = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", RoseID, "RoseData");
                                    string yanZhengFileStr_roseBag = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RoseBag");
                                    string yanZhengFileStr_rosePet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RosePet");
                                    string yanZhengFileStr_roseStoreHouse = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RoseStoreHouse");
                                    string yanZhengFileStr_roseEquip = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RoseEquip");

                                    string yanZhengFileStr_rosePastureData = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", RoseID, "RosePastureData");
                                    string yanZhengFileStr_rosePastureBag = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RosePastureBag");
                                    string yanZhengFileStr_rosePasture = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", "1", "RosePasture");

                                    string yanZhengFileStr_roseChengJiu = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", RoseID, "RoseChengJiu");
                                    string yanZhengFileStr_roseConfig = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", RoseID, "RoseConfig");
                                    string yanZhengFileStr_roseDayReward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanZhengFileStr", "ID", RoseID, "RoseDayReward");

                                    if (yanZhengFileStr_roseData == "" && yanZhengFileStr_roseBag == "" && yanZhengFileStr_rosePet == "" && yanZhengFileStr_roseStoreHouse == "" && yanZhengFileStr_roseEquip == ""&& yanZhengFileStr_rosePastureData == ""&& yanZhengFileStr_rosePastureBag == ""&& yanZhengFileStr_rosePasture == ""&& yanZhengFileStr_roseChengJiu == "" && yanZhengFileStr_roseConfig == "" && yanZhengFileStr_roseDayReward == "")
                                    {
                                        AddShowLog("初始化通过....");
                                    }
                                    else {
                                        AddShowLog("初始化文件未通过1111....");
                                        //if (yanZhengFileStr_roseData != yanZhengFileStr_roseBag && yanZhengFileStr_roseData != yanZhengFileStr_rosePet && yanZhengFileStr_roseData != yanZhengFileStr_roseStoreHouse && yanZhengFileStr_roseData != yanZhengFileStr_rosePastureData && yanZhengFileStr_roseData != yanZhengFileStr_roseChengJiu && yanZhengFileStr_roseData != yanZhengFileStr_roseConfig && yanZhengFileStr_roseData != yanZhengFileStr_roseDayReward) {
                                            //AddShowLog("初始化文件未通过222...." + "yanZhengFileStr_roseData = " + yanZhengFileStr_roseData + "yanZhengFileStr_roseStoreHouse = " + yanZhengFileStr_roseStoreHouse);
                                            Show_YanZhengError("初始化文件错误,请重新登陆游戏！如果需要也可联系管理解决...");
                                            writeYanZhengStatus = false;
                                            Time.timeScale = 0;

                                            //发送服务器效验
                                            Pro_ComStr_4 com4 = new Pro_ComStr_4();
                                            string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                                            com4.str_1 = Game_PublicClassVar.Get_xmlScript.Encrypt(zhanghaoID);
                                            Debug.Log("AAAAAAAAAAAAAAAAAAA");
                                            com4.str_2 = Game_PublicClassVar.Get_xmlScript.Encrypt(SystemInfo.deviceUniqueIdentifier);
                                            com4.str_3 = Game_PublicClassVar.Get_xmlScript.Encrypt("Init:" + yanZhengFileStr);
                                            com4.str_4 = Game_PublicClassVar.Get_xmlScript.Encrypt(yanZhengFileStr_roseData + ";" + yanZhengFileStr_roseBag + ";" + yanZhengFileStr_rosePet + ";" + yanZhengFileStr_roseStoreHouse + ";" + yanZhengFileStr_roseEquip + ";" + yanZhengFileStr_rosePastureData + ";" + yanZhengFileStr_rosePastureBag + ";" + yanZhengFileStr_rosePasture + ";" + yanZhengFileStr_roseChengJiu + ";" + yanZhengFileStr_roseConfig + ";" + yanZhengFileStr_roseDayReward);
                                            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000153, com4);

                                            return;
                                        //}
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //AddShowLog("重新写入验证文件,防止异常....");
                            //设置重新写入验证文件，防止异常
                            PlayerPrefs.SetString("YanZhengFileStr_" + NowSelectFileName, "999");
                            PlayerPrefs.Save();
                        }

                        try
                        {
                            //设置新的随机验证文件,提供给下次验证
                            if (writeYanZhengStatus)
                            {
                                //AddShowLog("验证成功,重新写入数据....");
                                WriteFileYanZheng();
                            }
                        }
                        catch (Exception ex) {
                            //AddShowLog("重新写入验证文件,文件异常...." + ex);
                            //设置重新写入验证文件，防止异常
                            PlayerPrefs.SetString("YanZhengFileStr_" + NowSelectFileName, "999");
                            PlayerPrefs.Save();
                        }


                        int xmlVersionNum = PlayerPrefs.GetInt("XmlVersionNum_WeiJing2");
                        if (xmlVersion > xmlVersionNum)
                        {
                            //Debug.Log("开始覆盖更新数据");
                            PlayerPrefs.SetInt("XmlVersionNum_WeiJing2", xmlVersion);
                            PlayerPrefs.Save();
                            //IfSaveXmlStatus = true; //是否覆盖配置文件
                        }

                    }
                    catch(Exception ex) {

                        /*
                        IfSaveGetRoseData = true;
                        Debug.Log("账号数据异常,从备份中获取数据 = " + ex);
                        Time.timeScale = 0;
                        GameObject beifenObj = (GameObject)Instantiate(Obj_BeiFenData);
                        beifenObj.transform.SetParent(GameObject.Find("Canvas").transform);
                        beifenObj.transform.localScale = new Vector3(1, 1, 1);
                        beifenObj.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1, 1, 1);
                        */

                        Show_YanZhengError("数据存储异常,请重新下载存档！如果需要也可联系管理解决...",false,true,ex.ToString());

                    }

                    IfUpdata = true;        //表示只执行一次

                    if (DataUpdataStatus == false) {

                        AddShowLog("数据文件读取错误....");

                        Game_PublicClassVar.Get_gameLinkServerObj.HintMsgStatus_Exit = true;
                        Game_PublicClassVar.Get_gameLinkServerObj.HintMsgText_Exit = "数据文件读取错误!如果需要请加首页群联系管理..";

                        //强制退出游戏
                        Game_PublicClassVar.Get_wwwSet.forceExitGameStatus = true;
                        Game_PublicClassVar.Get_wwwSet.forceExitGameTimeSum = 0;

                        return;

                    }


                    //Debug.Log("数据加载完毕");
                    //设置当前账号不是第一次创建登录
                    Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "FirstGame", "1");

                    //DataUpdataStatus = true;

                    //第一次进游戏随机一个8为ID给角色
                    string zhangHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", RoseID, "RoseData");
                    //Debug.Log("zhangHaoID = " + zhangHaoID);
                    //因为17915632是第一次生成的时候获得加密文件的值T.T 好囧
                    if (zhangHaoID == "17915632" || zhangHaoID == "0")
                    {
                        //Debug.Log("ssssss");
                        try
                        {
                            zhangHaoID = Game_PublicClassVar.Get_function_Rose.GetZhangHuID();
                            //写入账号ID
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhangHaoID", zhangHaoID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            //清空初始化数据  
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("OffGameTime", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                            //接取初始任务
                            Game_PublicClassVar.Get_function_Task.GetTask("30100000");

                            //存储角色名称,第一次创建角色调用  根据不同职业写入不同值
                            if (CreateRoseStatus)
                            {
                                AddShowLog("新建角色第一次写入数据....");
                                //创建法师,写入法师基础值
                                if (CreateRoseOcc == "2")
                                {

                                    //写入技能
                                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillID", "60011011,60011020,60011030,60011040,60011050,60011060", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                                    //写入天赋
                                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnTianFuID", "201101,0;211101,0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                                    //写入职业
                                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseOccupation", "2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                                    Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "Occ", CreateRoseOcc);

                                    //写入快捷施法
                                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainSkillUI_5", "60011011", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

                                }

                                //创建猎手,写入猎手基础值
                                if (CreateRoseOcc == "3")
                                {

                                    //写入技能
                                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillID", "60012011,60012020,60012030,60012040,60012050,60012060", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                                    //写入天赋
                                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnTianFuID", "301101,0;311101,0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                                    //写入职业
                                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseOccupation", "3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                                    Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "Occ", CreateRoseOcc);

                                    //写入快捷施法
                                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainSkillUI_5", "60012011", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");


                                }
                            }

                            //存储文件
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("报错！！！第一次进入游戏报错" + ex);
                        }
                    }
                    else {

                        //读取当前是否已经绑定身份信息
                        string fangchenmi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShenFenBangDing", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        if (fangchenmi == "" || fangchenmi == "0" || fangchenmi == null) {
                            //超过20级自动绑定身份信息
                            if (Game_PublicClassVar.Get_function_Rose.GetRoseLv()>=20) {
                                string shenfenID = PlayerPrefs.GetString("FangChenMi_ID");
                                if (shenfenID != "" && shenfenID != "0" && shenfenID != null) {
                                    Pro_ComStr_4 com_4 = new Pro_ComStr_4();
                                    Debug.Log("AAAAAAAAAAAAAAAAAAA");
                                    com_4.str_1 = SystemInfo.deviceUniqueIdentifier;
                                    com_4.str_2 = shenfenID;
                                    com_4.str_3 = zhangHaoID;
                                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000133, com_4);
                                }
                            }
                        }

                    }


                    Game_PublicClassVar.Get_function_Rose.FangChenMiYear();

                    //防沉迷认证
                    int nowYearNum = PlayerPrefs.GetInt("FangChenMi_Year");
                    if (nowYearNum <= 17) {
                        IfWeiChengNianStatus = true;
                    }

                    //表示游客登录
                    string shenfenIDStr = PlayerPrefs.GetString("FangChenMi_ID");
                    if (shenfenIDStr.Length <= 5)
                    {
                        IfYouKeStatus = true;
                    }

                    //存储当前账号ID
                    NowZhangHaoID = zhangHaoID;

                    //存储角色名称,第一次创建角色调用
                    if (CreateRoseStatus)
                    {
                        //CreateRoseStatus = false;
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Name", CreateRoseNameStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        //Debug.Log("CreateRoseNameStr = " + CreateRoseNameStr);

                        //存储角色通用数据
                        Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "Name", CreateRoseNameStr);

                        //创建牧场的小动物
                        Game_PublicClassVar.Get_function_Pasture.CreatePastureAI("10001");
                        Game_PublicClassVar.Get_function_Pasture.CreatePastureAI("10002");
                    }

                    bool updateInitData = true;

                    //每次大版本更新用到的更新值
                    if (updateInitData)
                    {
                        //更新当前章节
                        int nowRoseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

                        //开启后面章节
                        if (nowRoseLv >= 55 && nowRoseLv < 57)
                        {
                            Game_PublicClassVar.Get_function_Rose.UpdataPVEChapter("6;1");
                        }

                        if (nowRoseLv >= 57 && nowRoseLv < 59)
                        {
                            Game_PublicClassVar.Get_function_Rose.UpdataPVEChapter("6;2");
                        }

                        if (nowRoseLv >= 59 && nowRoseLv < 62)
                        {
                            Game_PublicClassVar.Get_function_Rose.UpdataPVEChapter("6;3");
                        }

                        if (nowRoseLv >= 62)
                        {
                            Game_PublicClassVar.Get_function_Rose.UpdataPVEChapter("6;4");
                        }
                    }

                    if (WorldTimeStatus)
                    {
                        try
                        {
#if UNITY_ANDROID && !UNITY_EDITOR
                            //设置Umeng Appkey
                            GameTimer timer = new GameTimer(0.1f, 1, delegate ()
                           {
                               GA.StartWithAppKeyAndChannelId("5de0ff374ca357ddf00003ef", "App Store");        //设置appID信息

                               //调试时开启日志
                               GA.SetLogEnabled(false);            //不输出Logo信息
                               GA.SetLogEncryptEnabled(true);      //加密传输信息
                                                                   //GA.ProfileSignIn(zhangHaoID);     //写入账号ID
                               IfSendYouMengData = true;           //开启友盟发送
                               Debug.Log("unity : 初始化友盟参数");
                           });
                           timer.Start();
#endif
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("友盟有报错,PC端忽略此问题" + ex.Message);
                        }
                    }
                    else 
                    {

                        Debug.Log("unity : WorldTimeStatus = false");
                    }


#if UNITY_ANDROID
                    string roseId = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    BuglyAgent.ConfigDebugMode(false);
                    //BuglyAgent.SetUserId(roseId);
                    Debug.Log("unity:roseId " + roseId);
                    BuglyAgent.ConfigDefault(null, Application.version, roseId, 0);
                    BuglyAgent.InitWithAppId("0865861615");
                    BuglyAgent.EnableExceptionHandler();
#endif

                    //GA.ProfileSignIn(zhangHaoID, "app strore");

                    //print("GA.ProfileSignOff();");

                    //GA.ProfileSignOff();


                    //测试
                    //Game_PublicClassVar.Get_function_Rose.UpdataPVEChapter("5;5");

                }

                //Debug.Log(Application.persistentDataPath);

                //开启主界面追踪UI显示
                Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;

                updataStatus = true;            //表示只执行一次

                //updataOffGameTimeStatus = true;     //更新离线时间

                //加载初始化的资源变量
                string BaoLvIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BaoLvID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (BaoLvIDStr == "")
                {
                    BaoLvIDStr = "0";
                }

                BaoLvID = int.Parse(BaoLvIDStr);

                //初始化设定挂机时间，如果今天上线大于挂机验证时间 则一上线就会弹出提示
                string expTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayLinkTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                if (expTime == "")
                {
                    expTime = "0";
                }
                if (int.Parse(expTime) >= 240) {
                    Game_PublicClassVar.Get_wwwSet.guaJiYanZhengTime = 58;
                }

                //时间效验
                XiuGaiTimeYanZheng();

                //伤害验证赋值
                string roseActMaxValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseActMaxValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                if (roseActMaxValue!=""&& roseActMaxValue != null) {
                    YanZheng_RoseActMaxValue = int.Parse(roseActMaxValue);
                }

                string petActMaxValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetActMaxValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                if (petActMaxValue != "" && petActMaxValue != null)
                {
                    YanZheng_PetActMaxValue = int.Parse(petActMaxValue);
                }
            }
        }

        if (DataUpdataStatus)
        {
            //修改检测
            if (!IfUpdataGameWaiGua)
            {
                //Debug.Log("检测角色数据");
                IfUpdataGameWaiGua = true;

                string payValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string roseRmb = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                //充值少于100
                if (float.Parse(payValue) <= 100)
                {
                    //检测钻石
                    if (float.Parse(roseRmb) >= float.Parse(payValue) * 1000 + 900000)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame = true;
                        //Debug.Log("检测角色修改了数据");
                    }

                    //检测攻击(等级小于30级,攻击大于998判定为数据篡改)
                    if (Application.loadedLevelName == "EnterGame")
                    {
                        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                        if (roseLv < 30)
                        {
                            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMax > 99998)
                            {
                                Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame = true;
                            }
                        }
                    }
                }

                ObscuredString beiyong9 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_9", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                if (beiyong9 == "1") {
                    IfFengHaoStatus = true;
                    Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame = true;
                }
            }
        }

        //如果当前进入游戏时没有链接到网络,则不断尝试的进行网络连接
        if (!WorldTimeStatus)
        {
            wordldTimeStatus = wordldTimeStatus + Time.deltaTime;
            if (wordldTimeStatus >= 5.0f)
            {
                //协同加载时间状态
                //Debug.Log("获取时间aaaaaaa");
                try
                {
                    //wangluoTestStr = "开始连接网络";
                    if (IfUpdateWorldTime) {
                        //this.StartCoroutine(LoadWorldTime());
                        //获取时间戳
                        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000101, "");
                    }
                }
                catch
                {
                    Debug.Log("连接网络读取时间异常");
                    //wangluoTestStr = "连接网络读取时间异常";
                }

                wordldTimeStatus = 0;
            }
        }
        else
        {
            /*
            //判定是否读取到了表
            if (DataUpdataStatus)
            {
                //每分钟请求一次时间
                worldTimeOnceSum = worldTimeOnceSum + Time.deltaTime;

                if (!IfSendYouMengData)
                {
                    try
                    {
                        //if (worldTimeOnceSum >= 60) {
                        //worldTimeOnceSum = 0;
                        //GetNowWorldTime();
                        //设置Umeng Appkey

                        //Debug.Log("更新账号信息");
                        //string zhangHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", RoseID, "RoseData");
                        GA.StartWithAppKeyAndChannelId("58ef5a47c895761626001bc0", "App Store");        //设置appID信息
                        //Debug.Log("更新账号信息2222");
                        //调试时开启日志
                        GA.SetLogEnabled(false);            //不输出Logo信息
                        //Debug.Log("更新账号信息3333");
                        GA.SetLogEncryptEnabled(true);      //加密传输信息
                        //Debug.Log("更新账号信息4444");
                        //GA.ProfileSignIn(zhangHaoID);       //写入账号ID
                        IfSendYouMengData = true;           //开启友盟发送
                        //Debug.Log("更新账号信息5555");

                        //}
                    }
                    catch
                    {
                        Debug.Log("World友盟有报错");
                    }
                }
            }
            */
        }

        //设置距离第二天的离线时间
        if (WorldTimeStatus)
        {
            if (dayUpdataTime > 0)
            {
                float updataDayTime = dayUpdataTime - Time.realtimeSinceStartup;
                if (updataDayTime <= 0)
                {
                    //GetNowWorldTime();
                    //DayUpdataStatus = true; //设置更新
                    Debug.Log("第二天到了！！！");
                    dayUpdataTime = 86400;
                }
            }
        }
        else
        {
            //Debug.Log("时间未连接");
        }


        //每分钟请求一次时间
        saveOffGameTimeSum = saveOffGameTimeSum + Time.deltaTime;
        if (DataUpdataStatus)
        {
            if (saveOffGameTimeSum >= 10.0f)
            {
                if (lastOffGameTimeStatus) {
                    writeOffGameTime(saveOffGameTimeSum);
                    saveOffGameTimeSum = 0;
                }
            }
        }


        //获取上次离线时间
        if (updataOffGameTimeStatus)
        {
            updataOffGameTime();
            updataOffGameTimeStatus = false;
        }

        //第一次进入游戏触发
        if (!firstEnterGame)
        {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus == true)
            {

                firstEnterGame = true;

                //更新进攻时间
                //emenyActTime = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EnemyTime", "ID", RoseID, "RoseBuilding"));

                //检测当前是否第一次登陆游戏,清空每日奖励数据
                /*
                if (ifOneGameToDay) {
                    Debug.Log("今天第一次登陆游戏！");
                    dayClearnGameData();    //清空每日数据
                }
                */
                /*
                if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus)
                {
                    Debug.Log("ssssss123s");
                    Game_PublicClassVar.Get_game_PositionVar.dayClearnGameData();    //清空每日数据
                }
                */

                //检测当前首次进入游戏
                string oneEnterGame = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FirstEnterGame", "ID", RoseID, "RoseConfig");
                if (oneEnterGame == "0")
                {
                    try
                    {
                        //Debug.Log("ssssss123sSSSSS");
                        Game_PublicClassVar.Get_game_PositionVar.dayClearnGameData(true);    //清空每日数据
                    }
                    catch
                    {
                        Debug.Log("清空每日数据报错");
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FirstEnterGame", "1", "ID", RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                }
                //更新每日奖励领取状态
                string expNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "Day_ExpNum", "GameMainValue");
                string expNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ExpNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string goldNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "Day_GoldNum", "GameMainValue");
                string goldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

                //如果今日领取次数满了则不在触发时间累计
                if (int.Parse(expNum) >= int.Parse(expNumMax) && int.Parse(goldNum) >= int.Parse(goldNumMax))
                {
                    Game_PublicClassVar.Get_game_PositionVar.RoseDayPracticeRewardStatus = true;
                }

                //清空出售道具的数据
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

                //写入登录每日任务
                //Debug.Log("触发登录任务");
                Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "6", "1");

                //读取离线时间
                updataOffGameTime();
            }
        }

        //从当前数据拷贝备份数据
        if (IfSaveRoseData) {

            IfSaveRoseData = false;
            
            string beifenFileName = Set_XmlPath.Substring(0,Set_XmlPath.Length-1)+"_beifen/";
            //删除备份文件
            /*
            if (Directory.Exists(beifenFileName))
            {
                //删除文件夹
                Directory.Delete(beifenFileName, true);
            }
            */
            /*
            AddShowLog("储存备份文件中...");
            //备份文件
            Game_PublicClassVar.Get_xmlScript.CopyFile(new DirectoryInfo(Set_XmlPath), beifenFileName);
            AddShowLog("储存备份完成...");
            */
        }

        //从备份获取数据替换当前数据
        if (IfSaveGetRoseData) {

            IfSaveGetRoseData = false;
            
            AddShowLog("从储存备份文件获取存档...");

            string beifenFileName = Set_XmlPath.Substring(0, Set_XmlPath.Length - 1) + "_beifen/";
            //Debug.Log("beifenFileName = " + beifenFileName);
            //检测是否有存档文件
            if (Directory.Exists(beifenFileName))
            {
                Game_PublicClassVar.Get_xmlScript.CopyFile(new DirectoryInfo(beifenFileName), Set_XmlPath);
                AddShowLog("强制退出!1111...");
                //Game_PublicClassVar.Get_function_UI.Btn_ReturnRose();
                Application.Quit(); //退出游戏,以后要改成重启需要调用安卓方法
                //Debug.Log("强制退出!1111");
                //ExitGame();
            }

            AddShowLog("从储存备份文件获取存档完成!...");
            
        }

        
        if (IfFengHaoStatus)
        {
            IfFengHaoStatus = false;
            Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame = true;
            forceExitGameStatus = true;

            //弹出提示
            GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_CommonHintHint);
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_19");
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("系统检测数据异常,已断开与服务器连接!!!", Game_PublicClassVar.Get_wwwSet.ExitGame, Game_PublicClassVar.Get_wwwSet.ExitGame, "系统提示", "确定", "取消", Game_PublicClassVar.Get_wwwSet.ExitGame);

            if (GameObject.Find("Canvas/GameGongGaoSet") != null)
            {
                uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }

            //强制退出游戏
            Game_PublicClassVar.Get_wwwSet.forceExitGameStatus = true;
            Game_PublicClassVar.Get_wwwSet.forceExitGameTimeSum = 0;

        }

        //后台进入前台重置状态
        if (BackEnterGameOnly)
        {
            if (backEnterGameOnlySum >= 1)
            {
                BackEnterGameOnly = false;
                backEnterGameOnlySum = 0;
            }
            backEnterGameOnlySum = backEnterGameOnlySum + 1;
        }

        //读取电量,每秒检测电量
        getDianLiangTimeSum = getDianLiangTimeSum + Time.deltaTime;
        if (getDianLiangTimeSum >= 60) {
            getDianLiangTimeSum = 0;
            //检测一次电量
            JianCeDianLiang();
        }

        //检测强制退出游游戏
        if (forceExitGameStatus)
        {
            forceExitGameTimeSum = forceExitGameTimeSum + Time.deltaTime;
            if (forceExitGameTimeSum >= 5) {
                forceExitGameTimeSum = 0;
                //Debug.Log("强制退出！开关打开");
                ExitGame();
            }
        }

        //存储伤害记录
        if (YanZheng_ActMaxSaveStatus) {
            YanZheng_ActMaxSaveTime = YanZheng_ActMaxSaveTime + Time.deltaTime;
            if (YanZheng_ActMaxSaveTime >= 30) {
                YanZheng_ActMaxSaveTime = 0;
                YanZheng_ActMaxSaveStatus = false;
                if (DataUpdataStatus) {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseActMaxValue", YanZheng_RoseActMaxValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetActMaxValue", YanZheng_PetActMaxValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                }
            }
        }

        //上传玩家数据
        if (UpdatePlayerDataToServer) {
            if (DataUpdataStatus) {

                UpdatePlayerDataToServer = false;
                //每次进入游戏上传一次玩家数据(临时屏蔽一下吧,好像有bug，创建新号不会上传角色数据,直接默认上传吧)
                //string[] saveList = new string[] { "", "2", "预留设备号位置" };
                //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001006, saveList);
            }
        }

        //找回角色
        if (FindRoseStatus == true && DataUpdataStatus == true) {
            FindRoseStatus = false;
            Debug.Log("允许下载数据！");
            Debug.Log("AAAAAAAAAAAAAAAAAAA");
            string shebeiStr = SystemInfo.deviceUniqueIdentifier;
            string[] saveList = new string[] { FindRoseZhangHaoID, FindRosePassword, shebeiStr };
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010071, saveList);
            
        }

        if (RootIfExitGame) {
            if (IfRootStatus.ToString().Contains("1") || CheckApkName()==true) {
                //强制退出
                forceExitGameStatus = true;
                Debug.LogError("检测非法程序,退出游戏进程");
            }
        }

        //地图检测
        if (MapEnterStatus) {
            MapEnterTimeSum = MapEnterTimeSum + Time.deltaTime;
            if (MapEnterTimeSum >= 10) {
                MapEnterTimeSum = 0;
                MapEnterStatus = false;
            }
        }

        if (DataUpdataStatus) {
            if (DataWriteXml_BiDui_1_Status)
            {
                if (Application.loadedLevelName != "StartGame")
                {
                    DataWriteXml_BiDui_1_timeSum = DataWriteXml_BiDui_1_timeSum + Time.deltaTime;
                    if (DataWriteXml_BiDui_1_timeSum >= 5) {
                        DataWriteXml_BiDui_1_Status = false;
                        DataWriteXml_BiDui_1_timeSum = 0;
                    }
                }
            }
        }

#if UNITY_EDITOR
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose && Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.activeInHierarchy)
        {
            NavMeshAgent agent = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<UnityEngine.AI.NavMeshAgent>();
            Vector3 pos = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
            agent.speed = 5;
            if (Input.GetKey(KeyCode.W))
            {
                agent.SetDestination(new Vector3(pos.x, pos.y, pos.z + 1));
            }
            if (Input.GetKey(KeyCode.A))
            {
                agent.SetDestination(new Vector3(pos.x - 1, pos.y, pos.z));
            }
            if (Input.GetKey(KeyCode.D))
            {
                agent.SetDestination(new Vector3(pos.x + 1, pos.y, pos.z));
            }
            if (Input.GetKey(KeyCode.S))
            {
                agent.SetDestination(new Vector3(pos.x, pos.y, pos.z - 1));
            }
        }
#endif
    }

    //注销时调用,保存上次时间
    void OnDestory(){
        //Debug.Log("写入离线数据");
        //writeOffGameTime(0);
    }


    //游戏进入后台时执行该方法 pause为true 切换回前台时pause为false  （正常进退）
    void OnApplicationPause(bool pause)
    {
        /*
        //失去焦点在进入显示服务器断开连接
        if (Application.loadedLevelName == "StartGame") {
            if (Game_PublicClassVar.Get_gameLinkServerObj != null)
            {
                Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus = false;
            }
        }
        */

        if (pause)
        {

            NativeController.Instance.GetFocusTime = Time.realtimeSinceStartup;
            NativeController.Instance.CheckFocusTime();

            Time.timeScale = 0;

            //切换到后台时执行
            GameIfRunBack = true;
            //Debug.Log("正常进!进入后台！");

            //记录后台运行时间
            GameEnterBackTime = Time.realtimeSinceStartup;
            //Debug.Log("进入后台！GameEnterBackTime = " + GameEnterBackTime);
            //重新写入验证
            //AddShowLog("OnApplicationPause" + "切屏,进入后台！");
            //WriteFileYanZheng();
            //AddShowLog("OnApplicationPause" + "切屏,进入后台完成！");
            //AddShowLogSava_NoKey("进入后台完成！");

            DataWriteSave();

        }
        else
        {

            //AddShowLogSava_NoKey("获得前台完成！");

            Time.timeScale = 1;

            //检测是否修改了文件
            if (Application.loadedLevelName != "StartGame")
            {
                DataWriteXml_BiDui_1_Status = true;
                DataWriteYanZheng();
            }

            if (GameIfRunBack)
            {
                GameIfRunBack = false;
                //重新请求一次喜从天降宝箱数据
                //Debug.Log("进来了后台 Time.realtimeSinceStartup = " + Time.realtimeSinceStartup);
                //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20001014, "");
                //重新计算BOSS刷新时间
                if (Time.realtimeSinceStartup >= GameEnterBackTime)
                {
                    //计算后台运行时间
                    float nowBackTime = Time.realtimeSinceStartup - GameEnterBackTime;
                    //防止出错
                    if (nowBackTime <= 0) {
                        nowBackTime = 0;
                    }

                    //Debug.Log("nowBackTime = " + nowBackTime);

                    //后台进入前台执行一次
                    BackEnterGameOnly = true;
                    backEnterGameOnlySum = 0;

                    //超过半个小时直接强制退出游戏
                    if (nowBackTime >= 1800)
                    {
                        //Application.Quit();
                        ExitGame();
                    }

                    //如果没有连接网络,离线时间大于在线时间就会重启
                    if (Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus == false) {
                        if (nowBackTime >= Time.time) {
                            //Application.Quit();
                            ExitGame();
                        }
                    }

                    //后台时间待机超过半个小时回到主城
                    if (Application.loadedLevelName != "EnterGame" && Application.loadedLevelName != "StartGame")
                    {
                        //超过半个小时直接强制退出游戏
                        if (nowBackTime >= 1800)
                        {
                            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ReturnBuilding.GetComponent<UI_StartGame>().Btn_RetuenBuilding();
                            //Application.Quit();
                            ExitGame();
                        }
                    }

                    //如果未读取数据就取消
                    if (!Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
                    {
                        //Debug.Log("Updata读取数据表中……");
                        return;
                    }

                    if (Application.loadedLevelName != "StartGame")
                    {
                        //BOSS刷新扣除后台运行时间
                        //Debug.Log("后台时间：" + nowBackTime);
                        Game_PublicClassVar.function_AI.UpdataMonsterDeathOffLineTime(nowBackTime);
                        UpdateMonsterDeathTimeStatus = true;
                        //更新抽卡时间
                        Game_PublicClassVar.Get_function_UI.UpdateChouKaTime(nowBackTime);
                        //更新家园时间
                        Game_PublicClassVar.Get_function_Pasture.SavePastureTime((int)nowBackTime);
                        Game_PublicClassVar.Get_function_Pasture.CostTimePastureTrader((int)nowBackTime);
                    }
                    else {
                        //如果在登录界面就强制踢下线
                        if (nowBackTime >= 1800)
                        {
                            ExitGame();
                        }
                    }

                    if (Application.loadedLevelName == "EnterGame") {

                        if (Game_PublicClassVar.gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj != null) {
                            Game_PublicClassVar.gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().SendEnterMap();
                        }
                    }

                    if (DataUpdataStatus) {

                        //增加体力,每5分钟增加一点体力
                        int addTiLi = (int)(nowBackTime / 300);
                        if (addTiLi > 0)
                        {
                            Game_PublicClassVar.Get_function_Rose.AddTili(addTiLi);
                        }

                        //增加体力,每5分钟增加一点活力
                        if (addTiLi > 0)
                        {
                            Game_PublicClassVar.Get_function_Rose.AddHuoLi(addTiLi);
                        }

                    }

                    if (Application.loadedLevelName != "StartGame")
                    {
                        //判定是否开启加速
                        WriteTimeYanZheng(nowBackTime);
                    }
                }
            }

            //Debug.Log("111111");
            if (Application.loadedLevelName != "StartGame") {

                //重新写入验证（暂时屏蔽）
                //AddShowLog("OnApplicationPause" + "切屏,进入前台！");
                if (WriteFileYanZhengStatusQie == false) {
                    WriteFileYanZhengStatusQie = true;
                    WriteFileYanZheng();
                    WriteFileYanZhengStatusQie = false;
                }
                
                //AddShowLog("OnApplicationPause" + "切屏,进入前台完成！");
                //重新对比数据
                DataSetYanZheng();



                //game ("进来了后台 Time.realtimeSinceStartup = " + Time.realtimeSinceStartup);
            }

            NativeController.Instance.LoseFocusTime = Time.realtimeSinceStartup;
        }
    }

    //游戏失去焦点也就是进入后台时 focus为false 切换回前台时 focus为true   (Home进退)  
    void OnApplicationFocus(bool focus)
    {
        /*
        //失去焦点在进入显示服务器断开连接
        if (Application.loadedLevelName == "StartGame")
        {
            if (Game_PublicClassVar.Get_gameLinkServerObj != null)
            {
                Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus = false;
            }
        }
        */
        //AddShowLogSava_NoKey("进入焦点方法！");
        //AddShowLogSava_NoKey("进入焦点方法游戏时间Time.time:" + Time.time);
        //AddShowLogSava_NoKey("进入焦点方法游戏时间Time.realtimeSinceStartup:" + Time.realtimeSinceStartup);

        if (focus)
        {

            //AddShowLogSava_NoKey("进入焦点 = " + Time.realtimeSinceStartup);
            NativeController.Instance.GetFocusTime = Time.realtimeSinceStartup;
            NativeController.Instance.CheckFocusTime();

            Time.timeScale = 1;

            //切换到前台时执行，游戏启动时执行一次
            GameIfRunBack = false;
            //AddShowLogSava_NoKey("获得焦点完成！");
            //AddShowLogSava_NoKey("获得焦点完成游戏时间:" + Time.time);
            //AddShowLogSava_NoKey("获得焦点完成游戏时间.realtimeSinceStartup:" + Time.realtimeSinceStartup);
            //Debug.Log("Home进!进入前台！");
            //if(GAME)

            if (Application.loadedLevelName != "StartGame")
            {
                //重新对比数据
                DataSetYanZheng();
                //DataWriteSave();
                //重新写入验证
                //WriteFileYanZheng();
                //DataWriteSaveYanZheng();
            }

            //检测是否修改了文件
            if (Application.loadedLevelName != "StartGame")
            {
                //AddShowLog("OnApplicationFocus!失去焦点" + "进入前台！");
                DataWriteXml_BiDui_1_Status = true;
                DataWriteYanZheng();
                //AddShowLog("OnApplicationFocus完成!失去焦点" + "进入前台！");
            }
        }
        else
        {

            //AddShowLogSava_NoKey("失去焦点！");
            //AddShowLogSava_NoKey("失去焦点完成游戏时间:" + Time.time);
            //AddShowLogSava_NoKey("获得焦点完成游戏时间.realtimeSinceStartup:" + Time.realtimeSinceStartup);
            DataWriteSave();

            Time.timeScale = 0;

            //切换到后台时执行
            GameIfRunBack = true;
            //Debug.Log("Home进进!进入后台！");
            //记录后台运行时间
            GameEnterBackTime = Time.realtimeSinceStartup;
            //重新写入验证
            //AddShowLog("OnApplicationFocus完成!失去焦点" + "进入后台！");
            //WriteFileYanZheng();

            //Debug.Log("22222");
            //AddShowLog("OnApplicationFocus完成!失去焦点" + "进入后台！");
            //Debug.Log("OnApplicationFocus完成!失去焦点" + "进入后台！");

            if (WriteFileYanZhengStatusQie == false)
            {
                WriteFileYanZhengStatusQie = true;
                WriteFileYanZheng();
                WriteFileYanZhengStatusQie = false;
            }

            //失去焦点直接退出
            if (RootShiQuExitGame)
            {
                if (IfRootStatus.ToString().Contains("1") || CheckApkName() == true)
                {
                    //强制退出
                    forceExitGameStatus = true;
                    Application.Quit();
                    //Debug.LogError("检测失去程序,退出游戏进程");
                }
            }

            //AddShowLogSava_NoKey("失去焦点 = " + Time.realtimeSinceStartup);
            NativeController.Instance.LoseFocusTime = Time.realtimeSinceStartup;

        }
    }


    //写入离线时间
    public void writeOffGameTime(float addTimeValue) {

        //Debug.Log("准备存储离线时间...");

        //切后台的时候不记录时间
        if (GameIfRunBack) {
            //Debug.Log("准备存储离线时间...在后台");
            return;
        }

        //加载完离线时间才能写入离线时间
        if (Game_PublicClassVar.Get_wwwSet.GameOffResourceStatus == false) {
            //Debug.Log("准备存储离线时间...GameOffResourceStatus为false");
            return;
        }


        //Debug.Log("写入离线时间:" + Time.realtimeSinceStartup);
        if (WorldTimeStatus)
        {
            //Debug.Log("aaaaaaaa = " + float.Parse(enterGameTimeStamp));
            //string value = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffGameTime", "ID", RoseID, "RoseData");
            //ObscuredInt gameTime = (ObscuredInt)(Time.realtimeSinceStartup);
            //ObscuredInt gameTime = (ObscuredInt)(addTimeValue);
            //ObscuredInt offTiem = int.Parse(enterGameTimeStamp) + gameTime;
            //Debug.Log(" offTiem = " + offTiem);
            //ObscuredInt offTimeInt = (ObscuredInt)(offTiem);
            /*
            if (offTimeInt < 0) {
                offTimeInt = 0;
            }
            */

            /*
            ObscuredInt offTiem = 0;
            ObscuredInt offTimeInt = 0;
            ObscuredInt gameTime = (ObscuredInt)(addTimeValue);

            if (firstWriteTimeStatus == false) {

                offTiem = int.Parse(enterGameTimeStamp) + gameTime;
                offTimeInt = (ObscuredInt)(offTiem);
            }
            */
            string value = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffGameTime", "ID", RoseID, "RoseData");
            if (firstWriteTimeStatus == false)
            {
                value = enterGameTimeStamp;
                firstWriteTimeStatus = true;
            }

            ObscuredInt gameTime = (ObscuredInt)(addTimeValue);
            ObscuredInt offTiem = int.Parse(value) + gameTime;
            ObscuredInt offTimeInt = (ObscuredInt)(offTiem);

            //存储离线时间
            //Debug.Log("存储离线时间：" + offTimeInt);
            //每分钟写入角色数据,顺便写入当前血量的数据
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("OffGameTime", offTimeInt.ToString(), "ID", RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        }
        else {
            Debug.Log("准备存储离线时间...3333333");
        }
    }

    //开始协同怪物表
    private IEnumerator LoadMonster_Template()
    {
        //Debug.Log("Application.dataPath = " + Application.dataPath);
        //Debug.Log("测试读取表");
        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Monster_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/"+ Assets_XmlFileName + "/Get_Xml/" + "Monster_Template.xml");
            yield return WWW_Monster_Template;
            //Debug.Log("测试读取表成功");

        }
        if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            //Debug.Log("PC读取怪物表成功！进行中……");
            WWW_Monster_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Monster_Template.xml");
            yield return WWW_Monster_Template;
            //Debug.Log("PC读取怪物表成功！");

        }

        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Monster_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Monster_Template.xml");
            yield return WWW_Monster_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Monster_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Monster_Template.xml");
            yield return WWW_Monster_Template;
        }

        if (WWW_Monster_Template.isDone)
        {

            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Monster_Template.xml", WWW_Monster_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表

            //Debug.Log("读取怪物表成功！执行下一个表");
            //string sourceFile = Application.persistentDataPath + "/GameData/Xml/Get_Xml/AAAA.xml";
            //byte[] btFile = File.ReadAllBytes(sourceFile);
            //string aaa = Encoding.UTF8.GetString(btFile);
            //Debug.Log("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZWWW_Monster_Template = " + aaa);

        }

    }


    //开始协同道具表
    private IEnumerator LoadItem_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Item_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Item_Template.xml");
            yield return WWW_Item_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Item_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Item_Template.xml");
            yield return WWW_Item_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Item_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Item_Template.xml");
            yield return WWW_Item_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Item_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Item_Template.xml");
            yield return WWW_Item_Template;
        }


        if (WWW_Item_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Item_Template.xml", WWW_Item_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取道具表成功！");
        }

    }


    //开始协同道具表
    private IEnumerator LoadSkill_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Skill_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Skill_Template.xml");
            yield return WWW_Skill_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Skill_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Skill_Template.xml");
            yield return WWW_Skill_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Skill_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Skill_Template.xml");
            yield return WWW_Skill_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Skill_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Skill_Template.xml");
            yield return WWW_Skill_Template;
        }


        if (WWW_Skill_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Skill_Template.xml", WWW_Skill_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取技能表成功！");
        }

    }

    //开始协同掉落表
    private IEnumerator LoadDrop_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Drop_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Drop_Template.xml");
            yield return WWW_Drop_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Drop_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Drop_Template.xml");
            yield return WWW_Drop_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Drop_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Drop_Template.xml");
            yield return WWW_Drop_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Drop_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Drop_Template.xml");
            yield return WWW_Drop_Template;
        }


        if (WWW_Drop_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Drop_Template.xml", WWW_Drop_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取掉落表成功！");
        }

    }

    //开始协同任务表
    private IEnumerator LoadTask_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Task_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Task_Template.xml");
            yield return WWW_Task_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Task_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Task_Template.xml");
            yield return WWW_Task_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Task_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Task_Template.xml");
            yield return WWW_Task_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Task_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Task_Template.xml");
            yield return WWW_Task_Template;
        }


        if (WWW_Task_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Task_Template.xml", WWW_Task_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取任务表成功！");
        }

    }

    //开始协同NPC表
    private IEnumerator LoadNpc_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Npc_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Npc_Template.xml");
            yield return WWW_Npc_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Npc_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Npc_Template.xml");
            yield return WWW_Npc_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Npc_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Npc_Template.xml");
            yield return WWW_Npc_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Npc_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Npc_Template.xml");
            yield return WWW_Npc_Template;
        }


        if (WWW_Npc_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Npc_Template.xml", WWW_Npc_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取NPC表成功！");
        }

    }

    //开始协同角色经验表
    private IEnumerator LoadRoseExp_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_RoseExp_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "RoseExp_Template.xml");
            yield return WWW_RoseExp_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_RoseExp_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "RoseExp_Template.xml");
            yield return WWW_RoseExp_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_RoseExp_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "RoseExp_Template.xml");
            yield return WWW_RoseExp_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_RoseExp_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "RoseExp_Template.xml");
            yield return WWW_RoseExp_Template;
        }


        if (WWW_RoseExp_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "RoseExp_Template.xml", WWW_RoseExp_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取角色经验表成功！");
        }
    }

	//开始协同装备表
	private IEnumerator LoadEquip_Template()
	{
		//安卓读取
		if (Application.platform == RuntimePlatform.Android)
		{
			WWW_Equip_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Equip_Template.xml");
			yield return WWW_Equip_Template;
		}
        if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			//PC读取
			WWW_Equip_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Equip_Template.xml");
			yield return WWW_Equip_Template;
		}


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Equip_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Equip_Template.xml");
            yield return WWW_Equip_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Equip_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Equip_Template.xml");
            yield return WWW_Equip_Template;
        }


        if (WWW_Equip_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Equip_Template.xml", WWW_Equip_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取装备表成功！");
        }
		
	}

    //开始协套装备表
    private IEnumerator LoadEquipSuit_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_EquipSuit_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipSuit_Template.xml");
            yield return WWW_EquipSuit_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_EquipSuit_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "EquipSuit_Template.xml");
            yield return WWW_EquipSuit_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_EquipSuit_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipSuit_Template.xml");
            yield return WWW_EquipSuit_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_EquipSuit_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipSuit_Template.xml");
            yield return WWW_EquipSuit_Template;
        }


        if (WWW_EquipSuit_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "EquipSuit_Template.xml", WWW_EquipSuit_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取装备表成功！");
        }

    }

    //开始协套装属性表
    private IEnumerator LoadEquipSuitProperty_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_EquipSuitProperty_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipSuitProperty_Template.xml");
            yield return WWW_EquipSuitProperty_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_EquipSuitProperty_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "EquipSuitProperty_Template.xml");
            yield return WWW_EquipSuitProperty_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_EquipSuitProperty_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipSuitProperty_Template.xml");
            yield return WWW_EquipSuitProperty_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_EquipSuitProperty_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipSuitProperty_Template.xml");
            yield return WWW_EquipSuitProperty_Template;
        }


        if (WWW_EquipSuitProperty_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "EquipSuitProperty_Template.xml", WWW_EquipSuitProperty_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取装备表成功！");
        }

    }

	//开始协同职业表
	private IEnumerator LoadOccupation_Template()
	{
		
		
		//安卓读取
		if (Application.platform == RuntimePlatform.Android)
		{
			WWW_Occupation_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Occupation_Template.xml");
			yield return WWW_Occupation_Template;
		}
        if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			//PC读取
			WWW_Occupation_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Occupation_Template.xml");
			yield return WWW_Occupation_Template;

		}


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Occupation_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Occupation_Template.xml");
            yield return WWW_Occupation_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Occupation_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Occupation_Template.xml");
            yield return WWW_Occupation_Template;
        }


        if (WWW_Occupation_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Occupation_Template.xml", WWW_Occupation_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取职业表成功！");
        }
		
	}


    //开始协同场景道具表
    private IEnumerator LoadSceneItem_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_SceneItem_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SceneItem_Template.xml");
            yield return WWW_SceneItem_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_SceneItem_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "SceneItem_Template.xml");
            yield return WWW_SceneItem_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_SceneItem_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SceneItem_Template.xml");
            yield return WWW_SceneItem_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_SceneItem_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SceneItem_Template.xml");
            yield return WWW_SceneItem_Template;
        }


        if (WWW_SceneItem_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "SceneItem_Template.xml", WWW_SceneItem_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取道具表成功！");
        }

    }


    //开始协同场景Buff表
    private IEnumerator LoadSkillBuff_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_SkillBuff_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SkillBuff_Template.xml");
            yield return WWW_SkillBuff_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_SkillBuff_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "SkillBuff_Template.xml");
            yield return WWW_SkillBuff_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_SkillBuff_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SkillBuff_Template.xml");
            yield return WWW_SkillBuff_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_SkillBuff_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SkillBuff_Template.xml");
            yield return WWW_SkillBuff_Template;
        }


        if (WWW_SkillBuff_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "SkillBuff_Template.xml", WWW_SkillBuff_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //开始协同故事对话表
    private IEnumerator LoadGameStory_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_GameStory_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "GameStory_Template.xml");
            yield return WWW_GameStory_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_GameStory_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "GameStory_Template.xml");
            yield return WWW_GameStory_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_GameStory_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "GameStory_Template.xml");
            yield return WWW_GameStory_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_GameStory_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "GameStory_Template.xml");
            yield return WWW_GameStory_Template;
        }


        if (WWW_GameStory_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "GameStory_Template.xml", WWW_GameStory_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取故事对话表表成功！");
        }

    }


    //开始协同故事对话表
    private IEnumerator LoadScene_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Scene_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Scene_Template.xml");
            yield return WWW_Scene_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Scene_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Scene_Template.xml");
            yield return WWW_Scene_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Scene_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Scene_Template.xml");
            yield return WWW_Scene_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Scene_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Scene_Template.xml");
            yield return WWW_Scene_Template;
        }


        if (WWW_Scene_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Scene_Template.xml", WWW_Scene_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //开始协同故事对话表
    private IEnumerator LoadSceneTransfer_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_SceneTransfer_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SceneTransfer_Template.xml");
            yield return WWW_SceneTransfer_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_SceneTransfer_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "SceneTransfer_Template.xml");
            yield return WWW_SceneTransfer_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_SceneTransfer_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SceneTransfer_Template.xml");
            yield return WWW_SceneTransfer_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_SceneTransfer_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SceneTransfer_Template.xml");
            yield return WWW_SceneTransfer_Template;
        }


        if (WWW_SceneTransfer_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "SceneTransfer_Template.xml", WWW_SceneTransfer_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //建筑表
    private IEnumerator LoadBuilding_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Building_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Building_Template.xml");
            yield return WWW_Building_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Building_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Building_Template.xml");
            yield return WWW_Building_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Building_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Building_Template.xml");
            yield return WWW_Building_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Building_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Building_Template.xml");
            yield return WWW_Building_Template;
        }


        if (WWW_Building_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Building_Template.xml", WWW_Building_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //建筑表
    private IEnumerator LoadChapter_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Chapter_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Chapter_Template.xml");
            yield return WWW_Chapter_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Chapter_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Chapter_Template.xml");
            yield return WWW_Chapter_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Chapter_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Chapter_Template.xml");
            yield return WWW_Chapter_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Chapter_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Chapter_Template.xml");
            yield return WWW_Chapter_Template;
        }


        if (WWW_Chapter_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Chapter_Template.xml", WWW_Chapter_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    private IEnumerator LoadChapterSon_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ChapterSon_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChapterSon_Template.xml");
            yield return WWW_ChapterSon_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ChapterSon_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "ChapterSon_Template.xml");
            yield return WWW_ChapterSon_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ChapterSon_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChapterSon_Template.xml");
            yield return WWW_ChapterSon_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ChapterSon_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChapterSon_Template.xml");
            yield return WWW_ChapterSon_Template;
        }


        if (WWW_ChapterSon_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ChapterSon_Template.xml", WWW_ChapterSon_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }


    private IEnumerator LoadTaskMovePosition_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_TaskMovePosition_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "TaskMovePosition_Template.xml");
            yield return WWW_TaskMovePosition_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_TaskMovePosition_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "TaskMovePosition_Template.xml");
            yield return WWW_TaskMovePosition_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_TaskMovePosition_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "TaskMovePosition_Template.xml");
            yield return WWW_TaskMovePosition_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_TaskMovePosition_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "TaskMovePosition_Template.xml");
            yield return WWW_TaskMovePosition_Template;
        }


        if (WWW_TaskMovePosition_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "TaskMovePosition_Template.xml", WWW_TaskMovePosition_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    private IEnumerator LoadEquipMake_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_EquipMake_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipMake_Template.xml");
            yield return WWW_EquipMake_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_EquipMake_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "EquipMake_Template.xml");
            yield return WWW_EquipMake_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_EquipMake_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipMake_Template.xml");
            yield return WWW_EquipMake_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_EquipMake_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipMake_Template.xml");
            yield return WWW_EquipMake_Template;
        }


        if (WWW_EquipMake_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "EquipMake_Template.xml", WWW_EquipMake_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    private IEnumerator LoadGameMainValue()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_GameMainValue = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "GameMainValue.xml");
            yield return WWW_GameMainValue;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_GameMainValue = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "GameMainValue.xml");
            yield return WWW_GameMainValue;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_GameMainValue = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "GameMainValue.xml");
            yield return WWW_GameMainValue;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_GameMainValue = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "GameMainValue.xml");
            yield return WWW_GameMainValue;
        }


        if (WWW_GameMainValue.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "GameMainValue.xml", WWW_GameMainValue);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    private IEnumerator LoadSpecialEvent_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_SpecialEvent_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SpecialEvent_Template.xml");
            yield return WWW_SpecialEvent_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_SpecialEvent_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "SpecialEvent_Template.xml");
            yield return WWW_SpecialEvent_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_SpecialEvent_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SpecialEvent_Template.xml");
            yield return WWW_SpecialEvent_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_SpecialEvent_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SpecialEvent_Template.xml");
            yield return WWW_SpecialEvent_Template;
        }


        if (WWW_SpecialEvent_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "SpecialEvent_Template.xml", WWW_SpecialEvent_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //每日任务表
    private IEnumerator LoadTaskCountry_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_TaskCountry_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "TaskCountry_Template.xml");
            yield return WWW_TaskCountry_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_TaskCountry_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "TaskCountry_Template.xml");
            yield return WWW_TaskCountry_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_TaskCountry_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "TaskCountry_Template.xml");
            yield return WWW_TaskCountry_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_TaskCountry_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "TaskCountry_Template.xml");
            yield return WWW_TaskCountry_Template;
        }


        if (WWW_TaskCountry_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "TaskCountry_Template.xml", WWW_TaskCountry_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //抽卡
    private IEnumerator LoadTakeCard_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_TakeCard_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "TakeCard_Template.xml");
            yield return WWW_TakeCard_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_TakeCard_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "TakeCard_Template.xml");
            yield return WWW_TakeCard_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_TakeCard_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "TakeCard_Template.xml");
            yield return WWW_TakeCard_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_TakeCard_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "TakeCard_Template.xml");
            yield return WWW_TakeCard_Template;
        }


        if (WWW_TaskCountry_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "TakeCard_Template.xml", WWW_TakeCard_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //荣誉兑换
    private IEnumerator LoadHonorStore_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_HonorStore_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "HonorStore_Template.xml");
            yield return WWW_HonorStore_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_HonorStore_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "HonorStore_Template.xml");
            yield return WWW_HonorStore_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_HonorStore_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "HonorStore_Template.xml");
            yield return WWW_HonorStore_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_HonorStore_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "HonorStore_Template.xml");
            yield return WWW_HonorStore_Template;
        }


        if (WWW_HonorStore_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "HonorStore_Template.xml", WWW_HonorStore_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //抽卡
    private IEnumerator LoadCountry_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Country_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Country_Template.xml");
            yield return WWW_Country_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Country_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Country_Template.xml");
            yield return WWW_Country_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Country_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Country_Template.xml");
            yield return WWW_Country_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Country_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Country_Template.xml");
            yield return WWW_Country_Template;
        }


        if (WWW_Country_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Country_Template.xml", WWW_Country_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //宠物表
    private IEnumerator LoadPet_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Pet_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Pet_Template.xml");
            yield return WWW_Pet_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Pet_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Pet_Template.xml");
            yield return WWW_Pet_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Pet_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Pet_Template.xml");
            yield return WWW_Pet_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Pet_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Pet_Template.xml");
            yield return WWW_Pet_Template;
        }


        if (WWW_Pet_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Pet_Template.xml", WWW_Pet_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //宠物表
    private IEnumerator LoadTalent_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Talent_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Talent_Template.xml");
            yield return WWW_Talent_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Talent_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Talent_Template.xml");
            yield return WWW_Talent_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Talent_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Talent_Template.xml");
            yield return WWW_Talent_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Talent_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Talent_Template.xml");
            yield return WWW_Talent_Template;
        }


        if (WWW_Talent_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Talent_Template.xml", WWW_Talent_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //收集大厅
    private IEnumerator LoadShouJiItem_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ShouJiItem_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ShouJiItem_Template.xml");
            yield return WWW_ShouJiItem_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ShouJiItem_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "ShouJiItem_Template.xml");
            yield return WWW_ShouJiItem_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ShouJiItem_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ShouJiItem_Template.xml");
            yield return WWW_ShouJiItem_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ShouJiItem_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ShouJiItem_Template.xml");
            yield return WWW_ShouJiItem_Template;
        }


        if (WWW_ShouJiItem_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ShouJiItem_Template.xml", WWW_ShouJiItem_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //收集大厅
    private IEnumerator LoadShouJiItemPro_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ShouJiItemPro_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ShouJiItemPro_Template.xml");
            yield return WWW_ShouJiItemPro_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ShouJiItemPro_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "ShouJiItemPro_Template.xml");
            yield return WWW_ShouJiItemPro_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ShouJiItemPro_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ShouJiItemPro_Template.xml");
            yield return WWW_ShouJiItemPro_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ShouJiItemPro_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ShouJiItemPro_Template.xml");
            yield return WWW_ShouJiItemPro_Template;
        }


        if (WWW_ShouJiItem_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ShouJiItemPro_Template.xml", WWW_ShouJiItemPro_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //装备格子强化
    private IEnumerator LoadEquipQiangHua_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_EquipQiangHua_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipQiangHua_Template.xml");
            yield return WWW_EquipQiangHua_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_EquipQiangHua_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "EquipQiangHua_Template.xml");
            yield return WWW_EquipQiangHua_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_EquipQiangHua_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipQiangHua_Template.xml");
            yield return WWW_EquipQiangHua_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_EquipQiangHua_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipQiangHua_Template.xml");
            yield return WWW_EquipQiangHua_Template;
        }


        if (WWW_EquipQiangHua_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "EquipQiangHua_Template.xml", WWW_EquipQiangHua_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //洗炼属性表
    private IEnumerator LoadHintProList_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_HintProList_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "HintProList_Template.xml");
            yield return WWW_HintProList_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_HintProList_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "HintProList_Template.xml");
            yield return WWW_HintProList_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_HintProList_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "HintProList_Template.xml");
            yield return WWW_HintProList_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_HintProList_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "HintProList_Template.xml");
            yield return WWW_HintProList_Template;
        }


        if (WWW_HintProList_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "HintProList_Template.xml", WWW_HintProList_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //场景探索表
    private IEnumerator LoadScenceTanSuo_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_SceneTanSuo_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SceneTanSuo_Template.xml");
            yield return WWW_SceneTanSuo_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_SceneTanSuo_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "SceneTanSuo_Template.xml");
            yield return WWW_SceneTanSuo_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_SceneTanSuo_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SceneTanSuo_Template.xml");
            yield return WWW_SceneTanSuo_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_SceneTanSuo_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "SceneTanSuo_Template.xml");
            yield return WWW_SceneTanSuo_Template;
        }


        if (WWW_SceneTanSuo_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "SceneTanSuo_Template.xml", WWW_SceneTanSuo_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //游戏活动表WWW_ChengHao_Template
    private IEnumerator LoadActivity_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Activity_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Activity_Template.xml");
            yield return WWW_Activity_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Activity_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Activity_Template.xml");
            yield return WWW_Activity_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Activity_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Activity_Template.xml");
            yield return WWW_Activity_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Activity_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Activity_Template.xml");
            yield return WWW_Activity_Template;
        }


        if (WWW_Activity_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Activity_Template.xml", WWW_Activity_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //游戏活动表WWW_ChengHao_Template
    private IEnumerator LoadChengHao_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ChengHao_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengHao_Template.xml");
            yield return WWW_ChengHao_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ChengHao_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "ChengHao_Template.xml");
            yield return WWW_ChengHao_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ChengHao_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengHao_Template.xml");
            yield return WWW_ChengHao_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ChengHao_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengHao_Template.xml");
            yield return WWW_ChengHao_Template;
        }


        if (WWW_ChengHao_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ChengHao_Template.xml", WWW_ChengHao_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //游戏活动表WWW_ChengHao_Template
    private IEnumerator LoadSpirit_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Spirit_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Spirit_Template.xml");
            yield return WWW_Spirit_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Spirit_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Spirit_Template.xml");
            yield return WWW_Spirit_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Spirit_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Spirit_Template.xml");
            yield return WWW_Spirit_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Spirit_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Spirit_Template.xml");
            yield return WWW_Spirit_Template;
        }


        if (WWW_Spirit_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Spirit_Template.xml", WWW_Spirit_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }



    //游戏活动表WWW_ChengHao_Template
    private IEnumerator LoadChengJiu_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ChengJiu_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengJiu_Template.xml");
            yield return WWW_ChengJiu_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ChengJiu_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "ChengJiu_Template.xml");
            yield return WWW_ChengJiu_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ChengJiu_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengJiu_Template.xml");
            yield return WWW_ChengJiu_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ChengJiu_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengJiu_Template.xml");
            yield return WWW_ChengJiu_Template;
        }


        if (WWW_ChengHao_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ChengJiu_Template.xml", WWW_ChengJiu_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }



    //游戏活动表WWW_ChengHao_Template
    private IEnumerator LoadChengJiuAll_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ChengJiuAll_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengJiuAll_Template.xml");
            yield return WWW_ChengJiuAll_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ChengJiuAll_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "ChengJiuAll_Template.xml");
            yield return WWW_ChengJiuAll_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ChengJiuAll_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengJiuAll_Template.xml");
            yield return WWW_ChengJiuAll_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ChengJiuAll_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengJiuAll_Template.xml");
            yield return WWW_ChengJiuAll_Template;
        }


        if (WWW_ChengJiuAll_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ChengJiuAll_Template.xml", WWW_ChengJiuAll_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //游戏活动表WWW_ChengHao_Template
    private IEnumerator LoadChengJiuReward_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ChengJiuReward_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengJiuReward_Template.xml");
            yield return WWW_ChengJiuReward_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ChengJiuReward_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "ChengJiuReward_Template.xml");
            yield return WWW_ChengJiuReward_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ChengJiuReward_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengJiuReward_Template.xml");
            yield return WWW_ChengJiuReward_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ChengJiuReward_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ChengJiuReward_Template.xml");
            yield return WWW_ChengJiuReward_Template;
        }


        if (WWW_ChengJiuReward_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ChengJiuReward_Template.xml", WWW_ChengJiuReward_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //洗炼属性表
    private IEnumerator LoadEquipXiLianDaShi_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_EquipXiLianDaShi_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipXiLianDaShi_Template.xml");
            yield return WWW_EquipXiLianDaShi_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_EquipXiLianDaShi_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "EquipXiLianDaShi_Template.xml");
            yield return WWW_EquipXiLianDaShi_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_EquipXiLianDaShi_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipXiLianDaShi_Template.xml");
            yield return WWW_EquipXiLianDaShi_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_EquipXiLianDaShi_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "EquipXiLianDaShi_Template.xml");
            yield return WWW_EquipXiLianDaShi_Template;
        }


        if (WWW_EquipXiLianDaShi_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "EquipXiLianDaShi_Template.xml", WWW_EquipXiLianDaShi_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //签到表  
    private IEnumerator LoadQianDao_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_QianDao_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "QianDao_Template.xml");
            yield return WWW_QianDao_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_QianDao_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "QianDao_Template.xml");
            yield return WWW_QianDao_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_QianDao_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "QianDao_Template.xml");
            yield return WWW_QianDao_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_QianDao_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "QianDao_Template.xml");
            yield return WWW_QianDao_Template;
        }


        if (WWW_QianDao_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "QianDao_Template.xml", WWW_QianDao_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //令牌表
    private IEnumerator LoadLingPai_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_LingPai_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "LingPai_Template.xml");
            yield return WWW_LingPai_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_LingPai_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "LingPai_Template.xml");
            yield return WWW_LingPai_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_LingPai_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "LingPai_Template.xml");
            yield return WWW_LingPai_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_LingPai_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "LingPai_Template.xml");
            yield return WWW_LingPai_Template;
        }


        if (WWW_LingPai_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "LingPai_Template.xml", WWW_LingPai_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }


    //令牌表
    private IEnumerator LoadPetXiuLian_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_PetXiuLian_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "PetXiuLian_Template.xml");
            yield return WWW_PetXiuLian_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_PetXiuLian_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "PetXiuLian_Template.xml");
            yield return WWW_PetXiuLian_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_PetXiuLian_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "PetXiuLian_Template.xml");
            yield return WWW_PetXiuLian_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_PetXiuLian_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "PetXiuLian_Template.xml");
            yield return WWW_PetXiuLian_Template;
        }


        if (WWW_PetXiuLian_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "PetXiuLian_Template.xml", WWW_PetXiuLian_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }


    
    //牧场表
    private IEnumerator LoadPasture_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Pasture_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Pasture_Template.xml");
            yield return WWW_Pasture_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Pasture_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "Pasture_Template.xml");
            yield return WWW_Pasture_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Pasture_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Pasture_Template.xml");
            yield return WWW_Pasture_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Pasture_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "Pasture_Template.xml");
            yield return WWW_Pasture_Template;
        }


        if (WWW_Pasture_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Pasture_Template.xml", WWW_Pasture_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }


    //牧场升级表
    private IEnumerator LoadPastureUpLv_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_PastureUpLv_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "PastureUpLv_Template.xml");
            yield return WWW_PastureUpLv_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_PastureUpLv_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "PastureUpLv_Template.xml");
            yield return WWW_PastureUpLv_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_PastureUpLv_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "PastureUpLv_Template.xml");
            yield return WWW_PastureUpLv_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_PastureUpLv_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "PastureUpLv_Template.xml");
            yield return WWW_PastureUpLv_Template;
        }


        if (WWW_PastureUpLv_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "PastureUpLv_Template.xml", WWW_PastureUpLv_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }


    //牧场兑换表
    private IEnumerator LoadPastureDuiHuanStore_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_PastureDuiHuanStore_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "PastureDuiHuanStore_Template.xml");
            yield return WWW_PastureDuiHuanStore_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_PastureDuiHuanStore_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "PastureDuiHuanStore_Template.xml");
            yield return WWW_PastureDuiHuanStore_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_PastureDuiHuanStore_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "PastureDuiHuanStore_Template.xml");
            yield return WWW_PastureDuiHuanStore_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_PastureDuiHuanStore_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "PastureDuiHuanStore_Template.xml");
            yield return WWW_PastureDuiHuanStore_Template;
        }


        if (WWW_PastureDuiHuanStore_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "PastureDuiHuanStore_Template.xml", WWW_PastureDuiHuanStore_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }


    //牧场兑换表
    private IEnumerator LoadZuoQi_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ZuoQi_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZuoQi_Template.xml");
            yield return WWW_ZuoQi_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ZuoQi_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "ZuoQi_Template.xml");
            yield return WWW_ZuoQi_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ZuoQi_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZuoQi_Template.xml");
            yield return WWW_ZuoQi_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ZuoQi_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZuoQi_Template.xml");
            yield return WWW_ZuoQi_Template;
        }


        if (WWW_ZuoQi_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ZuoQi_Template.xml", WWW_ZuoQi_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }



    //牧场兑换表
    private IEnumerator LoadZuoQiNengLi_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ZuoQiNengLi_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZuoQiNengLi_Template.xml");
            yield return WWW_ZuoQiNengLi_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ZuoQiNengLi_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "ZuoQiNengLi_Template.xml");
            yield return WWW_ZuoQiNengLi_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ZuoQiNengLi_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZuoQiNengLi_Template.xml");
            yield return WWW_ZuoQiNengLi_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ZuoQiNengLi_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZuoQiNengLi_Template.xml");
            yield return WWW_ZuoQiNengLi_Template;
        }


        if (WWW_ZuoQiNengLi_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ZuoQiNengLi_Template.xml", WWW_ZuoQiNengLi_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }


    //牧场兑换表
    private IEnumerator Load_ZuoQiShow_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ZuoQiShow_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZuoQiShow_Template.xml");
            yield return WWW_ZuoQiShow_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ZuoQiShow_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "ZuoQiShow_Template.xml");
            yield return WWW_ZuoQiShow_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ZuoQiShow_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZuoQiShow_Template.xml");
            yield return WWW_ZuoQiShow_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ZuoQiShow_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZuoQiShow_Template.xml");
            yield return WWW_ZuoQiShow_Template;
        }


        if (WWW_ZuoQiShow_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ZuoQiShow_Template.xml", WWW_ZuoQiShow_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }



    //牧场兑换表
    private IEnumerator Load_FuBenShangHai_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_FuBenShangHai_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "FuBenShangHai_Template.xml");
            yield return WWW_FuBenShangHai_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_FuBenShangHai_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "FuBenShangHai_Template.xml");
            yield return WWW_FuBenShangHai_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_FuBenShangHai_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "FuBenShangHai_Template.xml");
            yield return WWW_FuBenShangHai_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_FuBenShangHai_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "FuBenShangHai_Template.xml");
            yield return WWW_FuBenShangHai_Template;
        }


        if (WWW_FuBenShangHai_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "FuBenShangHai_Template.xml", WWW_FuBenShangHai_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }



    //牧场兑换表
    private IEnumerator Load_JueXing_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_JueXing_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "JueXing_Template.xml");
            yield return WWW_JueXing_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_JueXing_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "JueXing_Template.xml");
            yield return WWW_JueXing_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_JueXing_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "JueXing_Template.xml");
            yield return WWW_JueXing_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_JueXing_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "JueXing_Template.xml");
            yield return WWW_JueXing_Template;
        }


        if (WWW_JueXing_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "JueXing_Template.xml", WWW_JueXing_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }


    //牧场兑换表
    private IEnumerator Load_RanSe_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_RanSe_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "RanSe_Template.xml");
            yield return WWW_RanSe_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_RanSe_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "RanSe_Template.xml");
            yield return WWW_RanSe_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_RanSe_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "RanSe_Template.xml");
            yield return WWW_RanSe_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_RanSe_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "RanSe_Template.xml");
            yield return WWW_RanSe_Template;
        }


        if (WWW_RanSe_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "RanSe_Template.xml", WWW_RanSe_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }


    //牧场兑换表
    private IEnumerator Load_HuoDong_Tower_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_HuoDong_Tower_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "HuoDong_Tower_Template.xml");
            yield return WWW_HuoDong_Tower_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_HuoDong_Tower_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "HuoDong_Tower_Template.xml");
            yield return WWW_HuoDong_Tower_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_HuoDong_Tower_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "HuoDong_Tower_Template.xml");
            yield return WWW_HuoDong_Tower_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_HuoDong_Tower_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "HuoDong_Tower_Template.xml");
            yield return WWW_HuoDong_Tower_Template;
        }


        if (WWW_HuoDong_Tower_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "HuoDong_Tower_Template.xml", WWW_HuoDong_Tower_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }

    //牧场兑换表
    private IEnumerator Load_ZhuLing_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ZhuLing_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZhuLing_Template.xml");
            yield return WWW_ZhuLing_Template;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ZhuLing_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Get_Xml\\" + "ZhuLing_Template.xml");
            yield return WWW_ZhuLing_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ZhuLing_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZhuLing_Template.xml");
            yield return WWW_ZhuLing_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ZhuLing_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Get_Xml/" + "ZhuLing_Template.xml");
            yield return WWW_ZhuLing_Template;
        }


        if (WWW_ZhuLing_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ZhuLing_Template.xml", WWW_ZhuLing_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }


    /*
    //开始协同Buff表
    private IEnumerator LoadSkillBuff_Template()
    {

        //Debug.Log("开始读取Buff表");
        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_SkillBuff_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + " SkillBuff_Template.xml");
            //Debug.Log("开始读取Buff表(安卓)");
            yield return WWW_SkillBuff_Template;
        }
        else
        {
            //PC读取
            WWW_SkillBuff_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "SkillBuff_Template.xml");
            yield return WWW_SkillBuff_Template;

        }

        if (WWW_SkillBuff_Template.isDone)
        {
            //Debug.Log("读取Buff表完成");
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "SkillBuff_Template.xml", WWW_SkillBuff_Template);
            updataNumSum = updataNumSum + 1;
            //Debug.Log("读取Buff表成功！");
        }

    }
    */

    //开始初始化主角数据表
    private IEnumerator Set_RoseData()
    {
        //Debug.Log("Application.dataPath = " + Application.dataPath);

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseData = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseData.xml");
            yield return WWWSet_RoseData;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseData = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RoseData.xml");
            yield return WWWSet_RoseData;
        }

        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseData = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseData.xml");
            yield return WWWSet_RoseData;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseData = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseData.xml");
            yield return WWWSet_RoseData;
        }


        if (WWWSet_RoseData.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseData.xml", WWWSet_RoseData);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取主数据表成功！");

            //LastOffGameTime = GetTime("0000000000");        //测试
            
        }

    }

    //开始初始化主角背包数据
    private IEnumerator Set_RoseBag()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseBag = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseBag.xml");
            yield return WWWSet_RoseBag;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseBag = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RoseBag.xml");
            yield return WWWSet_RoseBag;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseBag = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseBag.xml");
            yield return WWWSet_RoseBag;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseBag = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseBag.xml");
            yield return WWWSet_RoseBag;
        }


        //复制RoseBag表
        if (WWWSet_RoseBag.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseBag.xml", WWWSet_RoseBag);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取背包表成功！");
        }

    }


    //开始初始化主角装备数据
    private IEnumerator Set_RoseEquip()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseEquip = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseEquip.xml");
            yield return WWWSet_RoseEquip;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseEquip = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RoseEquip.xml");
            yield return WWWSet_RoseEquip;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseEquip = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseEquip.xml");
            yield return WWWSet_RoseEquip;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseEquip = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseEquip.xml");
            yield return WWWSet_RoseEquip;
        }


        if (WWWSet_RoseEquip.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseEquip.xml", WWWSet_RoseEquip);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取主角装备表成功！");
        }

    }


    //游戏配置WWWSet_GameCreate
    public IEnumerator Set_GameCreate()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_GameCreate = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + "GameCreate.xml");
            yield return WWWSet_GameCreate;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_GameCreate = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + "GameCreate.xml");
            yield return WWWSet_GameCreate;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_GameCreate = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + "GameCreate.xml");
            yield return WWWSet_GameCreate;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_GameCreate = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + "GameCreate.xml");
            yield return WWWSet_GameCreate;
        }


        if (WWWSet_GameCreate.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/" + "Xml" + "/Set_Xml/" + "GameCreate.xml", WWWSet_GameCreate);
            //CreateRoseDataNum = CreateRoseDataNum + 1;
            //updataNumSum = updataNumSum + 1;
            //upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }


    //游戏配置WWWSet_GameConfig_1
    public IEnumerator Set_GameConfig()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_GameConfig_1 = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_GameConfig_1 = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_GameConfig_1 = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_GameConfig_1 = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }


        if (WWWSet_GameConfig_1.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/"+ "Xml" + "/Set_Xml/" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName + "/" + "GameConfig.xml", WWWSet_GameConfig_1);
            CreateRoseDataNum = CreateRoseDataNum + 1;
            //updataNumSum = updataNumSum + 1;
            //upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }

    
	//开始初始化主角配置数据
	private IEnumerator Set_RoseConfig()
	{
		//安卓读取
		if (Application.platform == RuntimePlatform.Android)
		{
			WWWSet_RoseConfig = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseConfig.xml");
			yield return WWWSet_RoseConfig;
		}
        if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			//PC读取
			WWWSet_RoseConfig = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RoseConfig.xml");
			yield return WWWSet_RoseConfig;
		}


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseConfig = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseConfig.xml");
            yield return WWWSet_RoseConfig;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseConfig = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseConfig.xml");
            yield return WWWSet_RoseConfig;
        }


        if (WWWSet_RoseConfig.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseConfig.xml", WWWSet_RoseConfig);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取游戏配置表成功！");
        }
	}
    

    //开始初始化建筑相关数据
    private IEnumerator Set_RoseBuilding()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseBuilding = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseBuilding.xml");
            yield return WWWSet_RoseBuilding;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseBuilding = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RoseBuilding.xml");
            yield return WWWSet_RoseBuilding;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseBuilding = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseBuilding.xml");
            yield return WWWSet_RoseBuilding;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseBuilding = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseBuilding.xml");
            yield return WWWSet_RoseBuilding;
        }


        if (WWWSet_RoseBuilding.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseBuilding.xml", WWWSet_RoseBuilding);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取游戏配置表成功！");
        }

    }

    //开始初始化仓库相关数据
    private IEnumerator Set_RoseStoreHouse()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseStoreHouse = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseStoreHouse.xml");
            yield return WWWSet_RoseStoreHouse;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseStoreHouse = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RoseStoreHouse.xml");
            yield return WWWSet_RoseStoreHouse;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseStoreHouse = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseStoreHouse.xml");
            yield return WWWSet_RoseStoreHouse;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseStoreHouse = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseStoreHouse.xml");
            yield return WWWSet_RoseStoreHouse;
        }


        if (WWWSet_RoseStoreHouse.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseStoreHouse.xml", WWWSet_RoseStoreHouse);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取游戏配置表成功！" + updataNumSum);
        }

    }

    //初始化极品装备
    private IEnumerator Set_RoseEquipHideProperty()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseEquipHideProperty = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseEquipHideProperty.xml");
            yield return WWWSet_RoseEquipHideProperty;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseEquipHideProperty = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RoseEquipHideProperty.xml");
            yield return WWWSet_RoseEquipHideProperty;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseEquipHideProperty = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseEquipHideProperty.xml");
            yield return WWWSet_RoseEquipHideProperty;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseEquipHideProperty = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseEquipHideProperty.xml");
            yield return WWWSet_RoseEquipHideProperty;
        }


        if (WWWSet_RoseEquipHideProperty.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseEquipHideProperty.xml", WWWSet_RoseEquipHideProperty);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取游戏配置表成功！" + updataNumSum);
        }

    }

    //初始化每日奖励
    private IEnumerator Set_RoseDayReward()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseDayReward = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseDayReward.xml");
            yield return WWWSet_RoseDayReward;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseDayReward = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RoseDayReward.xml");
            yield return WWWSet_RoseDayReward;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseDayReward = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseDayReward.xml");
            yield return WWWSet_RoseDayReward;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseDayReward = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseDayReward.xml");
            yield return WWWSet_RoseDayReward;
        }


        if (WWWSet_RoseDayReward.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseDayReward.xml", WWWSet_RoseDayReward);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }
    }



    //游戏配置WWWSet_GameConfig_1
    private IEnumerator Set_RosePet()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RosePet = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePet.xml");
            //Debug.Log("WWWSet_RosePet1 = " + WWWSet_RosePet.text);
            yield return WWWSet_RosePet;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RosePet = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RosePet.xml");
            //Debug.Log("WWWSet_RosePet2 = " + WWWSet_RosePet.text);
            yield return WWWSet_RosePet;
        }

        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RosePet = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePet.xml");
            yield return WWWSet_RosePet;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RosePet = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePet.xml");
            yield return WWWSet_RosePet;
        }


        if (WWWSet_RosePet.isDone)
        {
            //Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + NowSelectFileName + "/" + "RosePet.xml", WWWSet_RosePet);
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RosePet.xml", WWWSet_RosePet);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }


    //游戏配置WWWSet_GameConfig_1
    private IEnumerator Set_RoseOtherData()
    {
        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseOtherData = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseOtherData.xml");
            //Debug.Log("WWWSet_RosePet1 = " + WWWSet_RosePet.text);
            yield return WWWSet_RoseOtherData;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseOtherData = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RoseOtherData.xml");
            //Debug.Log("WWWSet_RosePet2 = " + WWWSet_RosePet.text);
            yield return WWWSet_RoseOtherData;
        }

        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseOtherData = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseOtherData.xml");
            yield return WWWSet_RoseOtherData;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseOtherData = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseOtherData.xml");
            yield return WWWSet_RoseOtherData;
        }


        if (WWWSet_RoseOtherData.isDone)
        {
            //Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + NowSelectFileName + "/" + "RoseOtherData.xml", WWWSet_RoseOtherData);
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseOtherData.xml", WWWSet_RoseOtherData);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }


    //游戏配置WWWSet_GameConfig_1
    private IEnumerator Set_RoseChengJiu()
    {
        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseChengJiu = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseChengJiu.xml");
            //Debug.Log("WWWSet_RosePet1 = " + WWWSet_RosePet.text);
            yield return WWWSet_RoseChengJiu;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseChengJiu = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RoseChengJiu.xml");
            //Debug.Log("WWWSet_RosePet2 = " + WWWSet_RosePet.text);
            yield return WWWSet_RoseChengJiu;
        }

        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseChengJiu = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseChengJiu.xml");
            yield return WWWSet_RoseChengJiu;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseChengJiu = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RoseChengJiu.xml");
            yield return WWWSet_RoseChengJiu;
        }


        if (WWWSet_RoseChengJiu.isDone)
        {
            //Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + NowSelectFileName + "/" + "RosePet.xml", WWWSet_RosePet);
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseChengJiu.xml", WWWSet_RoseChengJiu);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }


    //游戏配置WWWSet_GameConfig_1
    private IEnumerator Set_RosePasture()
    {
        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RosePasture = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePasture.xml");
            //Debug.Log("WWWSet_RosePet1 = " + WWWSet_RosePet.text);
            yield return WWWSet_RosePasture;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RosePasture = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RosePasture.xml");
            //Debug.Log("WWWSet_RosePet2 = " + WWWSet_RosePet.text);
            yield return WWWSet_RosePasture;
        }

        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RosePasture = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePasture.xml");
            yield return WWWSet_RosePasture;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RosePasture = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePasture.xml");
            yield return WWWSet_RosePasture;
        }


        if (WWWSet_RosePasture.isDone)
        {
            //Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + NowSelectFileName + "/" + "RosePet.xml", WWWSet_RosePet);
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RosePasture.xml", WWWSet_RosePasture);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }


    //游戏配置WWWSet_GameConfig_1
    private IEnumerator Set_RosePastureData()
    {
        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RosePastureData = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePastureData.xml");
            //Debug.Log("WWWSet_RosePet1 = " + WWWSet_RosePet.text);
            yield return WWWSet_RosePastureData;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RosePastureData = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RosePastureData.xml");
            //Debug.Log("WWWSet_RosePet2 = " + WWWSet_RosePet.text);
            yield return WWWSet_RosePastureData;
        }

        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RosePastureData = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePastureData.xml");
            yield return WWWSet_RosePastureData;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RosePastureData = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePastureData.xml");
            yield return WWWSet_RosePastureData;
        }


        if (WWWSet_RosePastureData.isDone)
        {
            //Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + NowSelectFileName + "/" + "RosePet.xml", WWWSet_RosePet);
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RosePastureData.xml", WWWSet_RosePastureData);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }

    //牧场仓库
    private IEnumerator Set_RosePastureBag()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RosePastureBag = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePastureBag.xml");
            yield return WWWSet_RosePastureBag;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RosePastureBag = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\" + Assets_XmlFileName + "\\Set_Xml\\" + RoseID + "\\" + "RosePastureBag.xml");
            yield return WWWSet_RosePastureBag;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RosePastureBag = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePastureBag.xml");
            yield return WWWSet_RosePastureBag;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RosePastureBag = new WWW("file://" + Application.dataPath + "/Raw/GameData/" + Assets_XmlFileName + "/Set_Xml/" + RoseID + "/" + "RosePastureBag.xml");
            yield return WWWSet_RosePastureBag;
        }


        if (WWWSet_RosePastureBag.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RosePastureBag.xml", WWWSet_RosePastureBag);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
                                            //Debug.Log("读取主数据表成功！");

            //LastOffGameTime = GetTime("0000000000");        //测试

        }

    }


    /*
    //游戏配置WWWSet_GameConfig_1
    private IEnumerator Set_GameConfig_1()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_GameConfig_1 = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + "10001" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_GameConfig_1 = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + "10001" + "\\" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_GameConfig_1 = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + "10001" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_GameConfig_1 = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + "10001" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }


        if (WWWSet_GameConfig_1.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/Xml/Set_Xml/10001/" + "GameConfig.xml", WWWSet_GameConfig_1);
            CreateRoseDataNum = CreateRoseDataNum + 1;
            //updataNumSum = updataNumSum + 1;
            //upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }

    //游戏配置WWWSet_GameConfig_1
    private IEnumerator Set_GameConfig_2()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_GameConfig_2 = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + "10002" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_2;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_GameConfig_2 = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + "10002" + "\\" + "GameConfig.xml");
            yield return WWWSet_GameConfig_2;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_GameConfig_2 = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + "10002" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_2;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_GameConfig_2 = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + "10002" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_2;
        }


        if (WWWSet_GameConfig_2.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/Xml/Set_Xml/10002/" + "GameConfig.xml", WWWSet_GameConfig_2);
            CreateRoseDataNum = CreateRoseDataNum + 1;
            //updataNumSum = updataNumSum + 1;
            //upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }
    */


    //外网有打不开的游戏的,需要手动加密Xml
    private IEnumerator addXmlJiaMi()
    {
        WWW_xml = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Test\\" + "aaa.xml");
        yield return WWW_xml;
    }


    private void updataOffGameTime(){
    
        //假设一个上一次时间,持久化数据OffGameTime
        //Debug.Log("RoseID = " + RoseID);
        string value = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffGameTime", "ID", RoseID, "RoseData");

        //Debug.Log("OffValue = " + value);
        if (value == "0")
        {
            //初次登陆设置时间为1970,会被识别为第一次登陆
            //PlayerPrefs.SetString("LastOffTime", "0000000000");
            LastOffGameTime = GetTime("0000000000");
            lastOffGameTimeStatus = true;       //获取离线时间戳值
        }
        else
        {
            LastOffGameTime = GetTime(value);
            lastOffGameTimeStatus = true;       //获取离线时间戳值
            //Debug.Log("LastOffGameTime = " + LastOffGameTime);
        }
    }


    //协同程序 获取当前时间
    private IEnumerator LoadWorldTime()
    {
        enterGameTimeStamp = "0";
        //wangluoTestStr = "网络加载中……";
        //Debug.Log("aaaaaaaaaaaaaaaa");
        string str = GetWebClient("http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=2");
        string timeStamp = str.Split('=')[1].Substring(0, 10);  //网页获取的数据长度超了，所以要裁剪

        Debug.Log("timeStamp = " + timeStamp);

        bool ifGetWorldTimeStr = true;
        //Debug.Log("str = " + str);
        //Debug.Log("timeStamp = " + timeStamp);
        //string str = GetWebClient("http://shijianchuo.911cha.com/");
        if (str == "0000000000")
        {
            //wangluoTestStr = "未连接网络!";
            Debug.Log("未连接网络");
            yield return WorldTimeStatus;
        }
        else {
            /*
            wangluoTestStr = "连接网络中!";
            string[] strShuZu = str.Split(new string[] { "→" }, StringSplitOptions.RemoveEmptyEntries);
            wangluoTestStr = "连接数据：1";
            Debug.Log("wangluoTestStr = " + wangluoTestStr);
            Debug.Log("wangluoTestStr111 = " + wangluoTestStr);
            bool ifGetWorldTimeStr = false;
            
            try
            {
                if (strShuZu.Length >= 2)
                {
                    wangluoTestStr = "ssssssss报错了！" + str;
                    ifGetWorldTimeStr = true;
                }
                else {
                    wangluoTestStr = "没啥！";
                }
            }
            catch {
                wangluoTestStr = "strShuZu报错了！" + str;
                Debug.Log("wangluoTestStr = " + wangluoTestStr);
                ifGetWorldTimeStr = false;
            }
            */
            /*
            Debug.Log("wangluoTestStr222 = " + wangluoTestStr);
            if (ifGetWorldTimeStr) {
                if (strShuZu.Length >= 2)
                {
                    Debug.Log("wangluoTestStr = " + wangluoTestStr);
                    wangluoTestStr = "连接数据：2";
                    string timeStamp = strShuZu[1].Substring(1, 10);  //网页获取的数据长度超了，所以要裁剪
                    wangluoTestStr = "连接数据：3";
                    Debug.Log("timeStamp = " + timeStamp);

                    //保证时间戳都是第一次连接获得
                    if (enterGameTimeStamp == "")
                    {
                        enterGameTimeStamp = timeStamp;
                    }
                    wangluoTestStr = "连接数据：4";
                    DataTime = GetTime(timeStamp);
                    wangluoTestStr = "连接数据：5";
                    //记录时间
                    Debug.Log((int)DataTime.Year + "年" + (int)DataTime.Month + "月" + (int)DataTime.Day + "日" + (int)DataTime.Hour + "时" + (int)DataTime.Minute + "分" + (int)DataTime.Second + "秒");//输出时间
                    dayUpdataTime = 86400 - ((int)DataTime.Hour * 3600 + (int)DataTime.Minute * 60 + (int)DataTime.Second);
                    wangluoTestStr = "连接数据：6";
                    if ((int)DataTime.Year > 2015)
                    {
                        //Debug.Log("Data = " + DataTime);
                        wangluoTestStr = "连接数据：7";
                        dayUpdataOne = true;
                        WorldTimeStatus = true;
                        yield return WorldTimeStatus;
                    }
                    wangluoTestStr = "连接数据：7---------" + timeStamp;
                }
            }
            */
            //wangluoTestStr = "连接数据：1";
            //Debug.Log("timeStamp = " + timeStamp);

            //保证时间戳都是第一次连接获得
            if (enterGameTimeStamp == "")
            {
                enterGameTimeStamp = timeStamp;
            }
            //wangluoTestStr = "连接数据：4";
            DataTime = GetTime(timeStamp);
            //wangluoTestStr = "连接数据：5";
            //记录时间
            Debug.Log((int)DataTime.Year + "年" + (int)DataTime.Month + "月" + (int)DataTime.Day + "日" + (int)DataTime.Hour + "时" + (int)DataTime.Minute + "分" + (int)DataTime.Second + "秒");//输出时间
            dayUpdataTime = 86400 - ((int)DataTime.Hour * 3600 + (int)DataTime.Minute * 60 + (int)DataTime.Second);
            //dayUpdataTime = 1950 - ((int)DataTime.Hour * 3600 + (int)DataTime.Minute * 60 + (int)DataTime.Second);
            //wangluoTestStr = "连接数据：6";

            Debug.Log("DataWorldTimeStatus22222 = " + DataTime);
            if ((int)DataTime.Year > 2015)
            {
                Debug.Log("DataWorldTimeStatus333333 = " + DataTime);
                //wangluoTestStr = "连接数据：7";
                dayUpdataOne = true;
                //WorldTimeStatus = true;
                yield return WorldTimeStatus;
            }
        }
    }



    //获取当前时间（用于其他地方调用）
    public string GetWorldTime()
    {
        //Debug.Log("aaaaaaaaaaaaaaaa");
        //string str = GetWebClient("http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=2");
        //string timeStamp = str.Split('=')[1].Substring(0, 10);  //网页获取的数据长度超了，所以要裁剪
        string str = GetWebClient("http://shijianchuo.911cha.com/");
        if (str == "0000000000" && str != "1 = 0000000000")
        {
            Debug.Log("未连接网络");
            return str;
        }
        else
        {
            string[] strShuZu = str.Split(new string[] { "→" }, StringSplitOptions.RemoveEmptyEntries);
            if (strShuZu.Length >= 2)
            {
                string timeStamp = strShuZu[1].Substring(1, 10);  //网页获取的数据长度超了，所以要裁剪
                Debug.Log("timeStamp = " + timeStamp);
                return timeStamp;
                /*
                //保证时间戳都是第一次连接获得
                if (enterGameTimeStamp == "")
                {
                    enterGameTimeStamp = timeStamp;
                }

                DataTime = GetTime(timeStamp);

                //记录时间
                Debug.Log((int)DataTime.Year + "年" + (int)DataTime.Month + "月" + (int)DataTime.Day + "日" + (int)DataTime.Hour + "时" + (int)DataTime.Minute + "分" + (int)DataTime.Second + "秒");//输出时间
                dayUpdataTime = 86400 - ((int)DataTime.Hour * 3600 + (int)DataTime.Minute * 60 + (int)DataTime.Second);

                if ((int)DataTime.Year > 2015)
                {
                    //Debug.Log("Data = " + DataTime);
                    dayUpdataOne = true;
                    WorldTimeStatus = true;
                    
                }
                */
            }
        }

        return str;
    }


    private static string GetWebClient(string url)
    {
        string strHTML = "";
        WebClient myWebClient = new WebClient();
        try
        {
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHTML = sr.ReadToEnd();
            myStream.Close();
        }
        catch(Exception ex) {
            ////Debug.Log("报错信息" + ex + "详细信息" + ex.Message);
            strHTML = "1=0000000000";
        }

        return strHTML;
    }

    /// <summary>
    /// 获取时间戳
    /// 本方法只是为了测试时间戳样式
    /// </summary>
    /// <returns></returns>
    public static string GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }

    /// <summary>
    /// 时间戳转为C#格式时间
    /// </summary>
    /// <param name="timeStamp">Unix时间戳格式</param>
    /// <returns>C#格式时间</returns>
    public DateTime GetTime(string timeStamp)
    {
        //验证时间戳格式是否正确
        int errorValue=999;
        bool timeValue = int.TryParse(timeStamp,out errorValue);
        if (!timeValue) {
            Debug.Log("时间戳验证错误!");
            timeStamp = "0000000000";   //强制等于1970的时间
        }

        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc));
        //dtStart.data
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }


    //检测电量,低于1%自动退出游戏。
    public void JianCeDianLiang() {
#if  UNITY_ANDROID && !UNITY_EDITOR
        int dianLiangValue = GetBatteryLevel();
        Debug.Log("dianLiangValue: " + dianLiangValue);
        if (dianLiangValue <= 2 && dianLiangValue!= -2)
        {
            //退出游戏
            ShowAndroidToastMessage("电量低于1%时,游戏将自动退出!防止游戏因为突然关机产生数据错误...");
            //Application.Quit();
            Debug.Log("强制退出!电量原因");
            ExitGame();
        }
#endif
    }


#if UNITY_ANDROID
    public static int GetBatteryLevel()
    {
        try
        {
            return Game_PublicClassVar.Get_getSignature.GetBatteryLevel();

            //if (Application.platform == RuntimePlatform.Android)
            //{
            //    float dianchiValue = SystemInfo.batteryLevel;       //返回1-100的值
            //    dianchiValue = dianchiValue * 100;
            //    int dianchiInt = (int)(dianchiValue);
            //    //Debug.Log("dianchiInt = " + dianchiInt);

            //    //string capacityString = System.IO.File.ReadAllText("/sys/class/power_supply/battery/capacity");
            //    //return int.Parse(capacityString);
            //    return dianchiInt;
            //}
            //return 100;
        }
        catch (Exception e)
        {
            if (Application.platform != RuntimePlatform.WindowsEditor) {
                Debug.Log("Failed to read battery power:" + e.Message);
            }
        }
        return -1;
    }
#endif



    /// <summary>
    /// 安卓弹出信息提示
    /// </summary>
    private void ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }

    //验证错误
    public void Show_YanZhengError(string hintText = "", bool ifQiangZhiUpdate = false, bool ifSendErr = false,string sendStr ="")
    {

        //强制退出游戏（10秒）
        forceExitGameStatus = true;
        forceExitGameTimeSum = 0;

        //传到后台报错数据
        if (ifSendErr) {
            Debug.LogError(sendStr);
        }

        if (ifQiangZhiUpdate == false)
        {
            //hintText = "";  //发布不显示
            GameObject uiCommonHint = (GameObject)Instantiate(Obj_CommonHintHint);
            //角色数据验证错误,如确认自身没有修改过角色数据,请加群联系管理验证后解开限制!
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(hintText, ExitGame, ExitGame, "系统提示", "确定", "取消", ExitGame);
            uiCommonHint.transform.SetParent(GameObject.Find("Canvas").transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        else {

            hintText = "游戏报错:" + hintText + "\n是否强制下载服务器的角色数据来解决此问题? \n提示:服务器数据有可能存在部分小范围回档!";
            //hintText = "";  //发布不显示
            GameObject uiCommonHint = (GameObject)Instantiate(Obj_CommonHint_1);
            //角色数据验证错误,如确认自身没有修改过角色数据,请加群联系管理验证后解开限制!
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(hintText, QiangZhiDownData, ExitGame, "系统提示", "下载数据", "取消", ExitGame);
            uiCommonHint.transform.SetParent(GameObject.Find("Canvas").transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1f, 1f, 1f);

        }

    }

    //检测模块退出
    public void ExitGame_Obs() {
        SetGame_Obs();

        ExitGame();

        if (IfRootStatus.ToString().Contains("1") || CheckApkName() == true) {
            ExitGame();
        }
        /*
        if (UnityEngine.Random.value <= 0.5f)
        {
            ExitGame();
        }
        */
    }

    public void SetGame_Obs() {

        if (Application.loadedLevelName == "StartGame") {
            return;
        }

        string ObsExitNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ObsExitNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (ObsExitNumStr == "" || ObsExitNumStr == null)
        {
            ObsExitNumStr = "0";
        }

        int writeExitNum = int.Parse(ObsExitNumStr) + 1;

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ObsExitNum", writeExitNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

    }


    public void ExitGame() {

        try
        {
            //存储成就信息
            if (DataUpdataStatus)
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseChengJiu");
            }
        }
        catch {

        }
        //Debug.LogError("退出游戏！退出游戏！退出游戏！退出游戏！");
        //Debug.Log("退出游戏！退出游戏！退出游戏！退出游戏！");
        Application.Quit();
    }


    //展示游戏公告
    public void Show_GameGongGao(string hintText)
    {
        GameObject uiCommonHint = (GameObject)Instantiate(Obj_CommonHintHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_19");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(hintText, null, null, "游戏公告");
        uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    //展示游戏公告
    public void Show_GameHint(string hintText)
    {
        GameObject uiCommonHint = (GameObject)Instantiate(Obj_CommonHintHint);
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(hintText, null, null);
        uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    //展示更新公告
    public void Show_GameUpdate(string hintText,string Url)
    {
        if (Application.loadedLevelName == "StartGame")
        {
            GameObject uiCommonHint = (GameObject)Instantiate(Obj_GameUpdate);
            uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1f, 1f, 1f);
            uiCommonHint.GetComponent<UI_GameUpdateHint>().ShowUpdateText = hintText;
            uiCommonHint.GetComponent<UI_GameUpdateHint>().ShowUpdateUrl = Url;
        }
        else {
            GameObject uiCommonHint = (GameObject)Instantiate(Obj_GameUpdate);
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1f, 1f, 1f);
            uiCommonHint.GetComponent<UI_GameUpdateHint>().ShowUpdateText = hintText;
            uiCommonHint.GetComponent<UI_GameUpdateHint>().ShowUpdateUrl = Url;
        }
    }

    //写入验证
    public void WriteFileYanZheng() {

        //AddShowLog("准备写入验证文件...." );
        if (Application.loadedLevelName == "StartGame")
        {
            //AddShowLog("写入失败,Star!");
            return;
        }

        try
        {
            //写入数据出错时退出
            if (WriteFileYanZhengStatus) {
                //AddShowLog("写入失败,YanZheng!");
                return;
            }

            //出错状态下退出
            if (forceExitGameStatus) {
                return;
            }

            WriteFileYanZhengStatus = true;

            if (DataUpdataStatus) {
                System.Random ran = new System.Random();
                int n = ran.Next(100, 10000);
                string writeYanZhengStr = "A" + GetTimeStamp() + "B" + n;

                //Debug.Log("写入效验码1111..." + writeYanZhengStr);

                //AddShowLog("写入效验..." + NowSelectFileName + ":writeYanZhengStr = " + writeYanZhengStr);
                PlayerPrefs.SetString("YanZhengFileStr_" + NowSelectFileName, "999");
                PlayerPrefs.Save();

                //AddShowLog("写入效验...YanZhengFileStr_" + NowSelectFileName + PlayerPrefs.GetString("YanZhengFileStr_" + NowSelectFileName) + ":writeYanZhengStr = " + writeYanZhengStr);
                //AddShowLog("写入效验1111……" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));

                bool ifwriteStatus_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", RoseID, "RoseData");
                bool ifwriteStatus_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", "1", "RoseBag");
                bool ifwriteStatus_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", "1", "RosePet");
                bool ifwriteStatus_4 = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", "1", "RoseStoreHouse");
                //AddShowLog("写入效验1111A……" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                bool ifwriteStatus_5 = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", "1", "RoseEquip");
                bool ifwriteStatus_6 = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", RoseID, "RosePastureData");
                bool ifwriteStatus_7 = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", "1", "RosePastureBag");
                bool ifwriteStatus_8 = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", "1", "RosePasture");
                //AddShowLog("写入效验1111B……" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                bool ifwriteStatus_9 = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", RoseID, "RoseChengJiu");
                //AddShowLog("写入效验1111C……" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                bool ifwriteStatus_10 = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", RoseID, "RoseConfig");
                bool ifwriteStatus_11 = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", RoseID, "RoseDayReward");

                //AddShowLog("写入效验2222……" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileNameStr", "RosePastureData;RosePastureBag;RosePasture;RoseChengJiu;RoseConfig;RoseDayReward", "ID", RoseID, "RoseData");

                //AddShowLog("写入效验3333……" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));

                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");
                //AddShowLog("写入效验5555……" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePasture");
                //AddShowLog("写入效验6666……" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseChengJiu");
                //AddShowLog("写入效验7777……" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                //AddShowLog("写入效验8888……" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");


                //AddShowLog("写入效验执行完毕....");

                if (Game_PublicClassVar.Get_gameLinkServerObj.setXmlErrorWriteYanZhengStatus == false)
                {

                    //AddShowLog("写入效验执行完毕1111....");

                    if (ifwriteStatus_1 && ifwriteStatus_2 && ifwriteStatus_3 && ifwriteStatus_4 && ifwriteStatus_5 && ifwriteStatus_6 && ifwriteStatus_7 && ifwriteStatus_8 && ifwriteStatus_9 && ifwriteStatus_10 && ifwriteStatus_11)
                    {
                        //AddShowLog("写入效验执行完毕2222....");
                        PlayerPrefs.SetString("YanZhengFileStr_" + NowSelectFileName, writeYanZhengStr);
                        PlayerPrefs.Save();
                        //Debug.Log("写入效验码2222..." + writeYanZhengStr);
                        //AddShowLog("写入效验执行完毕3333....");
                    }
                    else {
                        //AddShowLog("写入效验错误:" + ifwriteStatus_1.ToString() + ";" + ifwriteStatus_4.ToString());
                    }
                }
                else {
                    //AddShowLog("写入效验文件失败...");
                }


                if (Game_PublicClassVar.Get_gameLinkServerObj.setXmlErrorWriteYanZhengStatus)
                {
                    Game_PublicClassVar.Get_gameLinkServerObj.setXmlErrorWriteYanZhengStatus = false;
                }

                //AddShowLog("写入效验执行执行完毕...");


                //Debug.Log("写入数据...");
            }

            WriteFileYanZhengStatus = false;
            UpdateXmlNoYanZhengFile = false;

            //AddShowLog("写入效验执行执行完毕跳出方法...");

        }
        catch(Exception ex) {

            //AddShowLog("写入验证报错..." + ex);
            Debug.LogError("写入验证报错:" + ex);
            PlayerPrefs.SetString("YanZhengFileStr_" + NowSelectFileName, "999");
            PlayerPrefs.Save();
            WriteFileYanZhengStatus = false;
            UpdateXmlNoYanZhengFile = false;

            //AddShowLog("写入验证报错22222...");
        }
    }


    //写入服务器验证
    public void WriteFileServerYanZheng(string yanzhengValue) {

        try
        {
            if (DataUpdataStatus)
            {
                string writeYanZhengStr = yanzhengValue;

                    bool writeStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ServerYanZhengFileStr", writeYanZhengStr, "ID", RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

                    /*
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", "1", "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", "1", "RosePet");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", "1", "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanZhengFileStr", writeYanZhengStr, "ID", "1", "RoseEquip");

                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");
                    */

                    //Debug.Log("写入服务器数据...");
                    if (Game_PublicClassVar.Get_gameLinkServerObj.serverFileYanZhengStatus == false && writeStatus == true)
                    {
                        string serverYanZhengStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ServerYanZhengFileStr", "ID", RoseID, "RoseData");
                        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001602, serverYanZhengStr);
                    }

                    Game_PublicClassVar.Get_gameLinkServerObj.serverFileYanZhengStatus = true;

                }
            
        }
        catch
        {
            //PlayerPrefs.SetString("YanZhengFileStr_" + NowSelectFileName, "");
        }
        

    }


    //写入服务器验证
    public void GetFileServerYanZheng(string yanzhengValue)
    {

        try
        {
            if (DataUpdataStatus)
            {

                if (yanzhengValue == "999") {
                    //Debug.Log("服务器验证通过999..");
                    //请求新的文件验证记录
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001601, "");
                    return;
                }

                string serverYanZhengFileStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ServerYanZhengFileStr", "ID", RoseID, "RoseData");
                //Debug.Log("serverYanZhengFileStr = " + serverYanZhengFileStr + ";yanzhengValue == " + yanzhengValue);
                if (yanzhengValue == serverYanZhengFileStr)
                {
                    //Debug.Log("服务器验证通过..");
                    //请求新的文件验证记录
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001601, "");
                    return;
                }
                else {
                    //Debug.Log("服务器验证未通过..");
                    Show_YanZhengError("你可能在不同设备登陆过同一角色,服务器验证未通过!请重新下载角色数据或者联系客服进行重置");

                    //发送服务器效验
                    Pro_ComStr_4 com4 = new Pro_ComStr_4();
                    string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    com4.str_1 = Game_PublicClassVar.Get_xmlScript.Encrypt(zhanghaoID);
                    Debug.Log("AAAAAAAAAAAAAAAAAAA");
                    com4.str_2 = Game_PublicClassVar.Get_xmlScript.Encrypt(SystemInfo.deviceUniqueIdentifier);
                    com4.str_3 = Game_PublicClassVar.Get_xmlScript.Encrypt("ServerYanZheng");
                    com4.str_4 = Game_PublicClassVar.Get_xmlScript.Encrypt(serverYanZhengFileStr + ";" + yanzhengValue);
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000153, com4);

                }
            }
        }
        catch
        {
            //PlayerPrefs.SetString("YanZhengFileStr_" + NowSelectFileName, "");
        }
    }



    /// <summary>
    /// 获取数据压缩后的字节码
    /// </summary>
    public byte[] DataSetToByte(DataSet dt)
    {
        try
        {
            // 声明MemoryStream
            MemoryStream ms = new MemoryStream();
            // 写入DataSet中的数据到ms中
            dt.WriteXml(ms, XmlWriteMode.WriteSchema);
            // ms转换为数组序列
            byte[] bsrc = ms.ToArray();
            //关闭ms并释放资源
            ms.Close();
            ms.Dispose();

            ms = new MemoryStream();
            ms.Position = 0;
            // 压缩数组序列中的数据
            DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Compress);
            zipStream.Write(bsrc, 0, bsrc.Length);
            zipStream.Close();
            zipStream.Dispose();
            return ms.ToArray();
        }
        catch
        {
            return null;
        }
    }



    /// <summary>
    /// 获取数据压缩后的字节码
    /// </summary>
    public byte[] DataSetTableToByte(DataTableCollection dt,string dtName)
    {
        try
        {
            // 声明MemoryStream
            MemoryStream ms = new MemoryStream();
            // 写入DataSet中的数据到ms中
            dt[dtName].WriteXml(ms, XmlWriteMode.WriteSchema);
            // ms转换为数组序列
            byte[] bsrc = ms.ToArray();
            //关闭ms并释放资源
            ms.Close();
            ms.Dispose();

            ms = new MemoryStream();
            ms.Position = 0;
            // 压缩数组序列中的数据
            DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Compress);
            zipStream.Write(bsrc, 0, bsrc.Length);
            zipStream.Close();
            zipStream.Dispose();
            return ms.ToArray();
        }
        catch
        {
            return null;
        }
    }


    //对比数据
    private void DataSetYanZheng() {

        try
        {

            if (DataUpdataStatus == false)
            {
                return;
            }

            //DataWriteXml   DataSetXml
            byte[] dateByte = Game_PublicClassVar.Get_wwwSet.DataSetToByte(Game_PublicClassVar.Get_wwwSet.DataSetXml);
            MD5 md5 = MD5.Create();
            byte[] encryptionBytes = md5.ComputeHash(dateByte);
            string EncryptionStr = Convert.ToBase64String(encryptionBytes);

            //Debug.Log("EncryptionStr = " + EncryptionStr + " DataSetXml_BiDui_1_michi = " + Game_PublicClassVar.Get_wwwSet.DataSetXml_BiDui_1_michi); 

            if (EncryptionStr != Game_PublicClassVar.Get_wwwSet.DataSetXml_BiDui_1_michi)
            {
                Show_YanZhengError("匹配数据失败...");
            }

        }
        catch (Exception EX)
        {
            Debug.LogError("DataSetYanZheng报错:" + EX);
        }
    }


    //存储WriteXml
    public void DataWriteSave() {

        //Debug.Log("当前时间111:" + DateTime.Now.ToString("yyyy/MM/dd h:mm:ss.ff"));

        try
        {
            if (DataUpdataStatus == false) {
                return;
            }

            byte[] dateByte = Game_PublicClassVar.Get_wwwSet.DataSetToByte(Game_PublicClassVar.Get_wwwSet.DataWriteXml);
            //Debug.Log("当前时间1112:" + DateTime.Now.ToString("yyyy/MM/dd h:mm:ss.ff"));
            MD5 md5 = MD5.Create();
            byte[] encryptionBytes = md5.ComputeHash(dateByte);
            string EncryptionStr = Convert.ToBase64String(encryptionBytes);
            //Debug.Log("当前时间1113:" + DateTime.Now.ToString("yyyy/MM/dd h:mm:ss.ff"));
            Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_michi = EncryptionStr;
            //AddShowLogSava_NoKey("write:" + EncryptionStr);
            DataWriteXml_BiDui_1_Status = true;
            //Debug.Log("当前时间1114:" + DateTime.Now.ToString("yyyy/MM/dd h:mm:ss.ff"));
        }
        catch(Exception EX) {
            AddShowLogSava_NoKey("DataWriteSave报错:" + EX);
            DataWriteXml_BiDui_1_Status = false;
        }

        //Debug.Log("当前时间3333:" + DateTime.Now.ToString("yyyy/MM/dd h:mm:ss.ff"));
    }


    //对比数据WriteXml
    private void DataWriteYanZheng()
    {
        try
        {
            //AddShowLogSava_NoKey("执行文件效验开始!");
            if (DataWriteXml_BiDui_1_Status)
            {
                byte[] dateByte = Game_PublicClassVar.Get_wwwSet.DataSetToByte(Game_PublicClassVar.Get_wwwSet.DataWriteXml);
                MD5 md5 = MD5.Create();
                byte[] encryptionBytes = md5.ComputeHash(dateByte);
                string EncryptionStr = Convert.ToBase64String(encryptionBytes);

                //AddShowLogSava_NoKey("111 = " + EncryptionStr + " 222 = " + Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_michi);
                //Debug.Log("EncryptionStr = " + EncryptionStr + " Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_michi  = " + Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_michi);
                //Debug.Log("2222md5TestStr = " + EncryptionStr + "Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_michi = " + Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_michi);

                if (Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_michi != "")
                {
                    if (EncryptionStr != Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_michi)
                    {
                        //Debug.LogError("EncryptionStr = " + EncryptionStr + " Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_michi  = " + Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_michi);
                        if (IfRootStatus.ToString().Contains("1") || CheckApkName() == true)
                        {
                            Show_YanZhengError("匹配数据失败2222...");
                        }
                        //AddShowLogSava_NoKey("验证未通过!!!" + " 111 = " + EncryptionStr + " 222 = " + Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_michi);
                    }
                }
                DataWriteXml_BiDui_1_Status = false;
            }
        }
        catch (Exception EX)
        {
            Debug.LogError("DataWriteYanZheng:" + EX);
            AddShowLogSava_NoKey("验证报错!!!" + " ex = " + EX);
            DataWriteXml_BiDui_1_Status = false;
        }
        AddShowLogSava_NoKey("执行文件效验结束!");
    }


    //登陆时间验证
    private void XiuGaiTimeYanZheng()
    {
        //读取最后一次进入的时间戳
        string lastEnterGameTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LastEnterGameTime", "ID", RoseID, "RoseData");

        //string lastEnterGameTime = GetTimeStamp();
        if (lastEnterGameTime == "") {
            lastEnterGameTime = "0";
        }

        //*********验证本机时间************
        //获取服务器时间戳
        string nowTimeStr = GetTimeStamp();
        string severEnterTime = Game_PublicClassVar.Get_wwwSet.enterGameTimeStamp;

        Int64 nowTimeStr_Int = Int64.Parse(nowTimeStr);
        Int64 severEnterTime_Int = Int64.Parse(severEnterTime) + (Int64)(Time.time);
        Int64 chaValue = nowTimeStr_Int - severEnterTime_Int;

        if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus == false)
        {
            Show_YanZhengError("未获取游戏服务器的时间,请联网后重新打开游戏!");
            return;
        }
        else {
            if (chaValue >= 3600) {
                Show_YanZhengError("请将手机的本地时间调整为正常的北京时间!");
                return;
            }
        }

        /*
        Int64 lastEnterGameTime_Int = Int64.Parse(lastEnterGameTime);
        if (lastEnterGameTime_Int > nowTimeStr_Int) {
            Show_YanZhengError("和上一次登陆时间冲突,请稍等一会再登陆游戏!");
            return;
        }
        */

        //写入时间效验
        string nowTimeStamp = GetTimeStamp();
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LastEnterGameTime", nowTimeStamp, "ID", RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        /*
        //获取当前运行时间
        Int64 lastEnterGameTime_Int = Int64.Parse(lastEnterGameTime);

        //判断本机时间
        Int64 severEnterTime_Int = Int64.Parse(severEnterTime) + (Int64)(Time.time);

        //当最后一次进入的时间戳大于服务器的时间戳判定则不能进入游戏
        Int64 chaValue = lastEnterGameTime_Int - severEnterTime_Int;
        if (chaValue > 3600)
        {
            Show_YanZhengError("请将手机的本地时间调整为当前正常的北京时间!");
            return;
        }

        //*********验证服务器时间***********
        //获取服务器时间戳
        severEnterTime = Game_PublicClassVar.Get_wwwSet.enterGameTimeStamp;

        //获取当前运行时间
        lastEnterGameTime_Int = Int64.Parse(lastEnterGameTime);

        //判断服务器时间
        severEnterTime_Int = Int64.Parse(severEnterTime) + (Int64)(Time.time);

        //当最后一次进入的时间戳大于服务器的时间戳判定则不能进入游戏
        chaValue = lastEnterGameTime_Int - severEnterTime_Int;
        if (chaValue > 3600)
        {
            Show_YanZhengError("时间验证失败222!");
        }
        else {
            //写入时间效验
            string nowTimeStamp = GetTimeStamp();
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LastEnterGameTime", nowTimeStamp, "ID", RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        }
        */

    }



    //写入时间验证
    private void WriteTimeYanZheng(float blackTime) {


        //写入时间效验
        string nowTimeStamp = GetTimeStamp();
        /*
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LastEnterGameTime", nowTimeStamp, "ID", RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        */

        //向服务器发送时间进行验证
        Pro_ComStr_4 pro_ComStr_4 = new Pro_ComStr_4();
        pro_ComStr_4.str_1 = nowTimeStamp;
        pro_ComStr_4.str_2 = FirstTimeStr;
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000005, pro_ComStr_4);


        //********本地基础效验*********
        //获取在线时间
        //string nowTimeStamp = GetTimeStamp();
        //Int64 chaValue = Int64.Parse(nowTimeStamp) - Int64.Parse(FirstTimeStr);



        //存储时间戳

        //发送服务器时间戳进行效验,如果当前没有网络需要做个标记进行实时监测




        //获取服务器的时间戳  跟当前时间对比   取得差值参数


        //获取本地时间 和  服务器时间进行效验？



    }

    //强制下载数据
    public void QiangZhiDownData() {

        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (zhanghaoID != "" && zhanghaoID != null && zhanghaoID != "0") {
            Debug.Log("AAAAAAAAAAAAAAAAAAA");
            string shebeiStr = SystemInfo.deviceUniqueIdentifier;
            string[] saveList = new string[] { zhanghaoID, "", shebeiStr };
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010071, saveList);
        }

    }


    //清理验证
    public void ClearnFileYanZheng()
    {
        //Debug.Log("开始清理文件效验...");
        string createXmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/";
        string createIDListStr = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("CreateIDList", "ID", "1", createXmlPath + "GameCreate.xml");
        string[] createIDList = createIDListStr.Split(';');
        int initID = 10001;
        int maxID = initID;
        for (int i = 0; i < createIDList.Length; i++) {

            string[] createIDListSon = createIDList[i].Split(',');

            if (createIDListSon.Length >= 2) {
                string nowID = createIDListSon[0];
                if (nowID == "" || nowID == null)
                {
                    nowID = "0";
                }
                if (int.Parse(nowID) >= maxID) {
                    maxID = int.Parse(nowID);
                }
                
            }
        }

        int xunhuanNum = maxID - initID;
        for (int i = 0;i <= xunhuanNum;i++) {
            int now = initID + i;
            PlayerPrefs.SetString("YanZhengFileStr_" + now, "999");
            //Debug.Log("开始清理文件效验..." + "YanZhengFileStr_" + now);
        }

        PlayerPrefs.Save();
    }


    //清理验证
    public void ClearnGengXinKey()
    {
        /*
        //Debug.Log("开始清理文件效验...");
        string createXmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/";
        string createIDListStr = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("CreateIDList", "ID", "1", createXmlPath + "GameCreate.xml");
        string[] createIDList = createIDListStr.Split(';');
        int initID = 10001;
        int maxID = initID;

        for (int i = 0; i < createIDList.Length; i++)
        {

            string[] createIDListSon = createIDList[i].Split(',');

            if (createIDListSon.Length >= 2)
            {
                string nowID = createIDListSon[0];
                if (nowID == "" || nowID == null)
                {
                    nowID = "0";
                }
                if (int.Parse(nowID) >= maxID)
                {
                    maxID = int.Parse(nowID);
                }

            }
        }

        int xunhuanNum = maxID - initID;
        for (int i = 0; i <= xunhuanNum; i++)
        {
            int now = initID + i;
            PlayerPrefs.SetString("ChangeKey_" + now, "change");
            //Debug.Log("开始密尺验证..." + "ChangeKey_" + now);
        }

        PlayerPrefs.Save();
        */

    }



    public void AddShowLog(string str) {
        if (logStatus)
        {
            GameTiaoshiStr.Append("开始记录:" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss fff") + ":" + str + "\r\n");
            AddShowLogSava();
        }
    }

    //
    public void AddShowLogSava() {

        string addStr = Game_PublicClassVar.Get_xmlScript.Encrypt_DongTai(GameTiaoshiStr.ToString());

        string path = Application.persistentDataPath + "/GameData/" + "Xml" + "/Set_Xml/Log/";
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        Game_PublicClassVar.Get_xmlScript.Xml_CreateFile(Application.persistentDataPath + "/GameData/" + "Xml" + "/Set_Xml/Log/" + "gamelog" + LogInt + ".txt", addStr);
    }


    public void AddShowLog_NoKey(string str)
    {
        /*
            GameTiaoshiStr_NoKey.Append("开始记录:" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss fff") + ":" + str + "\r\n");
            AddShowLogSava_NoKey();
        */
    }

    //
    public void AddShowLogSava_NoKey(string addLog)
    {

        if (logNoKeyStatus)
        {
            Debug.Log("unity noKylog:" + addLog);
            GameTiaoshiStr_NoKey.Append("开始记录:" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss fff") + ":" + addLog + "\r\n");

            //string addStr = GameTiaoshiStr_NoKey.ToString();
            string path = Application.persistentDataPath + "/GameData/" + "Xml" + "/Set_Xml/NoKeyLog/";
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Game_PublicClassVar.Get_xmlScript.Xml_CreateFile(Application.persistentDataPath + "/GameData/" + "Xml" + "/Set_Xml/NoKeyLog/" + "gamelognokey" + LogInt + ".txt", GameTiaoshiStr_NoKey.ToString());
        }
    }



    public void LogShowLogCost(string str) {

        string costStr = Game_PublicClassVar.Get_xmlScript.Decrypt_DongTai(str);

    }


    //打开防沉迷
    public void OpenFangChenMiYanZheng()
    {


        //Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc.GetComponent<UI_StartGameFunc>().Btn_OpenRenZheng();

        if (RenZhengSetObj != null) {
            Destroy(RenZhengSetObj);
        }

        RenZhengSetObj = (GameObject)Instantiate(Obj_RenZhengSet);
        RenZhengSetObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        RenZhengSetObj.transform.localScale = new Vector3(1, 1, 1);
        RenZhengSetObj.transform.localPosition = new Vector3(0, 0, 0);



        //Debug.Log("OpenFangChenMiYanZheng111111");
        /*
        GameObject fangObj = (GameObject)Instantiate(FangChengMiYanZhengObj);
        fangObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        fangObj.transform.localScale = new Vector3(1, 1, 1);
        fangObj.transform.localPosition = Vector3.zero;
        fangObj.SetActive(true);
        */
        //Debug.Log("OpenFangChenMiYanZheng222222222");
    }


    //检测apk
    public bool CheckApkName() {

        if (CheckApkNameStr.ToString().Contains("xposed") && CheckApkNameStr.ToString().Contains("magisk"))
        {
            return true;
        }
        else {
            return false;
        }

    }


    //删除数据
    public void DeleteAllDate() {

        string FileName = Set_XmlPath.Substring(0, Set_XmlPath.Length - 15);
        Debug.Log("FileName = " + FileName);
        //删除备份文件

        if (Directory.Exists(FileName))
        {
            //删除文件夹
            Directory.Delete(FileName, true);
        }

    }



    /*
    /// <summary>
    /// 获取解压缩后的数据集
    /// </summary>
    public DataSet DeCompress(byte[] arrbts)
    {
    try
    {
        // 
        MemoryStream ms = new MemoryStream();
        ms.Write(arrbts, 0, ArrBytes.Length);
        ms.Position = 0;
        //
        DeflateStream ZipStream = new DeflateStream(ms, CompressionMode.Decompress);
        MemoryStream UnzipStream = new MemoryStream();
        byte[] sDecompressed = new byte[128];
        while (true)
        {
            int bytesRead = ZipStream.Read(sDecompressed, 0, 128);
            if (bytesRead == 0)
            {
                break;
            }
            UnzipStream.Write(sDecompressed, 0, bytesRead);
        }
        ZipStream.Close();
        ms.Close();
        UnzipStream.Position = 0;
        DataSet ds = new DataSet();
        // 读取解压后xml数据
        ds.ReadXml(UnzipStream);
        ds.AcceptChanges();     //更新所有行的状态为初始状态
        return ds;
    }
    catch
    { return null; }
    }
    */
    }