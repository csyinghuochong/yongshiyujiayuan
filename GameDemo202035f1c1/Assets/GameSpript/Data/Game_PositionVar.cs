using UnityEngine;
using System.Collections;
using System.Data;
using CodeStage.AntiCheat.ObscuredTypes;
using System;


//此脚本用于存放游戏内的通用的Transform和GameObject，方便其他程序调用，统一规范化管理
public class Game_PositionVar : MonoBehaviour {

    public DataSet DataSetXml;              //XML缓存集
    public string RoseID;                   //角色ID
    public GameObject Obj_Rose;             //角色Obj
    public string Get_XmlPath;              //只读Xml的位置
    public string Set_XmlPath;              //可读可存Xml的位置
    public GameObject[] Obj_Keep;           //切换场景保存的Obj
    public GameObject Obj_GameSourceSet;    //游戏音效的Obj
    public float SourceSize;                //游戏音效声音大小值
    public GameObject Obj_UIItemTips;       //UI道具Tips的源文件
    public GameObject Obj_UIItemTipsUse;    //UI道具Tips使用的OBJ
    public GameObject Obj_UIThrowItemChoice;    //丢弃道具时选择丢弃数量的UI
	public GameObject Obj_UIEquipTips;		//UI装备Tips的源文件
    public GameObject Obj_UIEquipMake;      //装备制造源Obj
    public GameObject Obj_UIHeZi;           //经验盒子源Obj
    public GameObject Obj_UICunMinDeXin;    //村民的信源Obj
    public GameObject Obj_UISkillTips;      //UI技能Tips的源文件
    public GameObject Obj_UIPetSkillTips;   //UI技能Tips的源文件
    public GameObject OBJ_UIMoveIcon;       //UI移动时显示的ICON
    public GameObject OBJ_UI_Set;           //UITips的集合
	public GameObject Obj_UI_FunctionOpen;	//UI功能开启
    public GameObject Obj_Model_Drop;       //掉落Obj
    public Transform Tr_Drop;               //掉落的父级
    public GameObject Obj_UIDropName;       //掉落道具的UI
    public GameObject Obj_UICaiJiName;       //采集道具的UI
    public bool UpdataRoseItem;             //更新角色道具
    private int updataRoseItemSum;
    public GameObject Obj_UINpcName;        //Npc名称
	public GameObject Obj_UINpcStoreShow;	//商店界面
    public GameObject Obj_UINpcStoreShow_2;	//商店2界面
    public GameObject Obj_UIItemMakeLearn;  //学习制造界面
	public GameObject Obj_UIMail;           //邮箱界面
    public GameObject Obj_UIDaMiJing;			//大秘境界面
    public GameObject Obj_StoreTextBack;    //黑屏显示故事文字
    public GameObject Obj_StorySpeakSet;    //对话UI
    public GameObject Obj_PastureMain;      //牧场升级集合
    public GameObject Obj_PastureDuiHuanDaTing;     //牧场升级集合
    public GameObject Obj_PastureBag;               //牧场仓库
    public GameObject Obj_PastureHeChengSet;        //牧场合成
    public GameObject Obj_PastureHeChengSet_Open;        //牧场合成(实例化)
    public GameObject Obj_PastureNpcBuySet_Open;         //牧场商人(实例化)
    public GameObject Obj_PastureFuMoSet_Open;           //牧场附魔(实例化)
    public GameObject Obj_PastureGongGaoSet;             //牧场公告

    public GameObject Obj_ZuoQiSet;                     //牧场合成
    public GameObject Obj_ZuoQiSetNow;                     //牧场合成
    public GameObject Obj_RanSeSet;                     //牧场染色
    public GameObject Obj_ZuoQiJiHuoSet_Open;           //坐骑激活
    public GameObject Obj_ZuoQiNengLiSet_Open;          //坐骑能力升级
    public GameObject Obj_ZuoQiWeiShiSet_Open;          //坐骑喂食

    public bool UpdatePastureBagAll;                //更新牧场背包显示
    public bool UpdataBagAll;				//背包内容全部更新
	private int UpdataBagAllSum;			//背包内容更新累计值
    public string RoseBagSpaceSelectType;   //背包选中类型
    public string RoseBagSpaceSelect;       //背包选中格子
    public bool RoseBagSpaceSelectStatus;   //背包选中格子更新状态
    public int RoseBagSpaceSelectUpdateNum; //背包选中更新累计
    public int RoseBagMaxNum;               //背包最大的格子数
    //public int RoseHouseMaxNum;           //仓库最大的格子数
    public int RosePastureBagMaxNum;        //牧场仓库最大格子数
    public int RoseHouseStartNum;           //仓库当前打开的开始格子数
    public int RoseHouseEndNum;             //仓库当前打开的结束格子数
    public int RosePetMaxNum;               //宠物最大数量
    public int RosePetLvMaxNum;             //宠物最大的等级数量
    public bool UpdataRoseEquip;            //更新当前角色角色界面显示的属性值
    public GameObject Obj_BuildingName;     //建筑名称的OBJ
    public GameObject Obj_RosePetSet;       //宠物父级
    public GameObject Obj_RoseCreatePetSet; //召唤宠物父级
    public GameObject Obj_MapCamera;        //小地图摄像机
    public GameObject Obj_CommonHuoBiSet;       //通用货币UI
    public GameObject Obj_CommonHuoBiSetObj;    //通用货币UI实力化后的,确保只有一个通用货币
    public GameObject Obj_TuoZhanEffect;        //脱战范围特效
    public int RosePastureMaxNum;               //牧场宠物最大数量

    //public string GameStatusValue;                //0：进入游戏界面  1： 创建角色界面  2：正式在游戏中
    //UI道具交换使用的变量
    public bool ItemMoveStatus;                     //是否在进行移动道具
    public string ItemMoveType_Initial;             //1：代表背包  2：代表英雄栏位
    public string ItemMoveValue_Initial;
    public string ItemMoveType_End;                 //1：代表背包  2：代表英雄栏位
    public string ItemMoveValue_End;
	public int HouseBagYeShu;

    //UI技能上交换
    public bool SkillMoveStatus;                    //是否在进行技能交换
    public string SkillMoveValue_Initial;                    
    public string SkillMoveValue_End;               
    public bool Rose_TaskListUpdata;                //是否更新任务列表
    public GameObject Obj_UITaskList_TaskType;      //任务列表UI
    public GameObject Obj_UITaskList_TaskRow;       //任务行UI
    public string NowTaskID;                        //当前选中任务ID
    public bool Rose_TaskDataUpdata;                //更新任务详细数据状态
    public string RoseTaskListShow_1;               //任务列表是否展开——主线任务
    public string RoseTaskListShow_2;               //任务列表是否展开——支线任务
    public string RoseTaskListShow_3;               //任务列表是否展开——其他任务
    public GameObject Obj_UI_NpcTaskList;           //接取任务用到的条
    public GameObject Obj_UI_NpcGetTask;            //接取任务用到的UI
    public GameObject Obj_UI_NpcCompleteTask;       //完成任务用到的UI
    public GameObject Obj_UI_NpcTask;               //NPC显示任务及对话的界面
    public string NpcTaskUpdataStatus;              //Npc显示的任务更新状态,0：表位未更新  1 - 2 表示执行到的步骤
	public bool MainUITaskUpdata;					//主界面任务显示更新
	public bool NpcTaskMainUIShow;					//主界面Npc头顶完成任务图标显示
	private int NpcTaskMainUIShowSum;				//主界面Npc头顶完成任务图标显示累计值，用于执行完当前帧重置其状态
	public bool UpdataRoseProperty;                 //开启时更新当前角色属性
    public bool UpdataRoseBuffProperty;             //开启时更新当前角色属性
    public GameObject Obj_UIGameHint;				//主界面通用UI提示
    public GameObject Obj_UIGameGirdHintSing;		//主界面通用UI提示(组提示)
    public GameObject Obj_UICommonHint;             //通用提示,带选项(是否)
    public GameObject Obj_UICommonHint_2;           //通用提示,带选项(是,仅仅提示作用)
    public GameObject Obj_UICommonHintTips_1;       //通用提示
    public GameObject Obj_UIGetherItem;             //打开道具的进度条UI
    public GameObject Obj_UISkillSing;				//技能吟唱
    public GameObject Obj_GameSourceObj;            //游戏音效Obj
    public bool EnterScenceStatus;                  //切换场景
    public string EnterScencePositionName;          //切换场景的坐标名称
    public bool SellItemStatus;                     //出售道具时开启此状态
    public bool PetXiLianStatus;                    //洗炼宠物时开启此状态
    public bool StoreHouseStatus;                   //打开仓库时开启此状态
    public bool EquipXiLianStatus;                  //打开洗练时开启此状态
	public bool HuiShouItemStatus;                  //打开回收时开启此状态
    public bool EquipPropertyMoveStatus;            //打开装备属性转移时开启此状态
    public bool NPCGiveStatus;                      //NPC给与状态
    public bool UpdataSelectedStatus;               //开启后,所有怪物更新当前选中目标是不是自己,如果不是吧自身的选中状态去掉
    private int UpdataSelectedStatusSum;            //开启怪物更新选中状态的计数器
    public bool UpdataSellUIStatus;                 //开启更新出售道具后,回购的列表
    public bool EnterRoseCameraDrawStatus;          //进入角色拉近摄像机
    public bool Rose_PublicSkillCDStatus;           //角色技能公共CD状态
    public float Rose_PublicSkillCDTime;            //角色技能公共CD时间
    public float Rose_PublicSkillCDTimeSum;         //角色技能公共CD累加值
    public bool UpdataMainSkillUI;                  //开启后更新主界面上的技能IconUI
    private int UpdataMainSkillUISum;               //开启后更新主界面上的技能IconUI的累加值,保证在下一帧执行一次
    private float doorWayNextDelayTime;             //延迟一帧设置玩家坐标，要不玩家会掉下去
    private float deathMonsterTimeSum;              //怪物死亡时间累计
    public GameObject Obj_MonsterDeathTime;         //怪物死亡倒计时显示UI
    public GameObject Obj_GameSetting;              //游戏设置UI
    public float GameSourceValue;                   //游戏声音值 1：表示正常  0表示静音
    public float GameSourceValue_YinXiao;           //游戏音效值 1：表示正常  0表示静音
    public int GameDanJiStatus;                     //游戏音效值 1：表示单机模式  0表示弱联网模式
    public GameObject Obj_YaoGanSet;                //摇杆相关控件
    public bool YaoGanStatus;                       //摇杆状态    
    public int PetActMoShi;                         //宠物模式
    public GameObject Obj_MonsterModelSheXiangJi;   //怪物模型摄像机
    public GameObject Obj_RoseModelSheXiangJi;      //角色摄像机
    public GameObject Obj_PetModlePosition;         //宠物模型摄像机绑点
    //public bool roseOpenOnlyUI;                   //角色打开唯一UI
    public string GameNanduValue;                   //怪物难度
    public bool Fight_CriStatus;                    //战斗暴击状态（临时存储）
    public bool ChengJiuJianCeStatus;               //成就检测状态
    public float ChengJiuJianCeTimeSum;             //成就检测时间
    public string ChengJiuJianCeChengJiuIDStr;      //成就id值

    //主城相关任务
    public GameObject Obj_DayPracticeReward;
    private GameObject dayPracticeRewardObj;        //修炼中心
    public GameObject Obj_GuoWangDaTing;
    private GameObject guoWangDaTingObj;            //国王大厅
    public GameObject Obj_ChouKa;
    private GameObject chouKaObj;                   //抽卡
    public GameObject Obj_ShiLianZhiTa;
    private GameObject shiLianZhiTaObj;             //试炼之塔
    public GameObject Obj_DuiHuanDaTing;
    private GameObject duiHuanDaTingObj;            //兑换大厅
    public GameObject Obj_DayTask;
    private GameObject dayTaskObj;                  //每日任务
    public GameObject Obj_ShouJiDaTing;
    private GameObject shouJiDaTingObj;             //每日任务

    //public bool DestroyKeepObj;                   //当次开关打开是注销Keep保存的Obj,此处用在战斗界面切换建筑界面中

    //建筑相关参数
    public bool UpdataBuildingIDStatus;                 //开启后更新建筑ID
    private bool BuildingHintGirdStatus;                //开启时播放资源获取提示
    private float Minute_TimeSum;                       //单次奖励时间累积
    private float HintShowIntervalTime;                 //提示间隔时间
    //private bool gameOffResourceStatus;               //游戏离线资源是否发放  true 表示已发放
    private float minuteValue;
    public GameObject Obj_OffGameResource;              //离线资源显示的Obj
    public GameObject Obj_UIFortressBuildReward;        //要塞战斗结果Obj

    private float hour_TimeSum;             //小时计时器
    private float tiLi_TimeSum;             //体力计时器

    public string GovernmentLvID;   	//市政厅等级
    public string ResidentHouseID;  	//民居
    public string FarmlandID;      		//农田
    public string LoggingFieldID;		//伐木场
    public string StonePitID;		    //采石场
    public string SmeltingPlantID;		//冶炼厂
    public string CollegeID;   		    //学院
    public string FortressID;           //要塞
    public string TrainCampID;          //训练场

    public int BuildingGold;            //建筑金币
    public int Farm;             	    //农民
    public int Food;	                //粮食
    public int Wood;	                //木材
    public int Stone;	                //石头
    public int Iron;	                //钢铁

    public int BuildingGold_Add;            //建筑金币
    public int Farm_Add;             	    //农民
    public int Food_Add;	                //粮食
    public int Wood_Add;	                //木材
    public int Stone_Add;	                //石头
    public int Iron_Add;	                //钢铁
    public int RoseExp_Add;                 //角色经验

    public int MinuteBuildingGold;            //建筑金币 (分钟收益)
    public int MinuteFarm;             	      //农民 (分钟收益)
    public int MinuteFood;	                  //粮食 (分钟收益)
    public int MinuteWood;	                  //木材 (分钟收益)
    public int MinuteStone;	                  //石头 (分钟收益)
    public int MinuteIron;	                  //钢铁 (分钟收益)
    public int MinuteRoseExp;                 //角色经验
    public int MinuteRoseTiLi;                //角色体力

    public int CountryExp;                      //每分钟国家经验
    public int CountryHonor;                    //每分钟荣誉产出
    public int MinuteCountryExp;                //每分钟国家经验
    public int MinuteCountryHonor;              //每分钟荣誉产出
    public int CountryExp_Add;              
    public int CountryHonor_Add;

    public bool emenyActStatus;            //此状态为True时,表示触发了要塞防御
    private float emenyActTimeSum;
    public float emenyActTime;
    private bool firstEnterGame;            //第一次进入游戏获取怪物进攻时间
    public bool UpdatTaskStatus;            //任务更新状态

    public bool OpenEmenyActStatus;
    private float secUpdataTimeSum;                 //每10秒执行一次
    private float secUpdateTimeSum_One;             //每1秒执行一次
    public bool RoseDayPracticeRewardStatus;        //每日领经验和金币状态
    private bool ifOneGameToDay;                    //为True表示今天第一次登陆游戏
    private int clearnBabyNum;                      //清理遇到宝宝的次数（每45分钟清理一次）

    //村民的信
    private float CunMinEmailSum;
    private float CunMinSaveSum;
    private bool CunMinStatus;

    //抽卡调用
    public bool ChouKaUIOpenStatus;
    public bool ChouKaStatus;
    public string ChouKaStr;
    public string ChouKaHindIdStr;

    //成就相关参数
    public string RoseChengJiuListShow_0;               //成就列表是否展开——成就首页
    public string RoseChengJiuListShow_1;               //成就列表是否展开——战斗成就
    public string RoseChengJiuListShow_2;               //成就列表是否展开——收集成就
    public string RoseChengJiuListShow_3;               //成就列表是否展开——探索成就
    public bool Rose_ChengJiuListUpdata;                //是否更新任务列表
    public GameObject Obj_ChengJiuXuanZhong;
    public bool Rose_ChengJiuXuanZhongStatus;
    private int rose_ChengJiuXuanZhongNum;
    public GameObject Obj_ChengJiuHintObj;
    //觉醒相关
    public bool JueXingStatus;                          //激活当前觉醒状态

    //精灵相关
    public GameObject Obj_JingLingHintObj;              //精灵提示

    //牧场相关
    public GameObject NowPastureSetObj;

    //副本相关参数
    public ObscuredBool FuBen_ShangHai_Status;          //进入伤害副本开启
    public ObscuredLong FuBen_ShangHaiValue_Rose;       //角色总伤害
    public ObscuredLong FuBen_ShangHaiValue_Pet;        //宠物总伤害

    //狩猎
    public bool HuoDong_ShowLie_YanChiShowStatus;
    public float HuoDong_ShowLie_RewardTime;

    //挂机验证
    public GameObject GuaJiObj;
    public string GuaJiShiQuType_1;
    public string GuaJiShiQuType_2;

    //加速效验
    private long JiaSuYanZheng_LastTime;
    private ObscuredInt jiaSuYanZhengAddNum;

    //其他参数
    public bool NpcShowValueStatus;
    private int npcShowValueNum;
    public int MapKillMonsterNum;
    public string MapKillBossID;
    public int DayKillMonsterNum;
    public bool UseItemStatus;          //背包使用道具状态


    public GameObject Obj_RoseDeathMuBei;

    //支付调用
    /*
    public bool PayStatus;               //交易状态
    public string PayStr;
    public string PayValueNow;           //当前交易值
    public string PayDingDanIDNow;       //当前交易订单
    public bool PayQueryStatus;          //交易订单查询状态
    public string PayStrQueryStatus;     //交易订单查询字符串
    private string testStrPay;
	*/

    public string PayValueNow;                  //当前交易值
	public string PayStrQueryStatus;            //交易订单查询字符串
	private string testStrPay;
	public bool PayQueryStatus;                 //交易订单查询状态

    //安卓支付
	public ObscuredBool PayStatus;              //交易状态
	public ObscuredString PayStr;               //交易字符串
	public ObscuredString PayDingDanIDNow;      //当前交易订单
	public ObscuredInt PayDingDanStatus;        //当前订单交易
	public ObscuredString PayDingDanLast;       //上一次订单账号
    //IOS支付
    public ObscuredBool PayStatusIOS;           //Ios支付状态
    //public ObscuredString PayIOSYanZhengStr;      //ios不用暂时取消本地二次验证
    public ObscuredString PayIOS_Str;

    public GameObject MonsterSet;
    public bool testFunction;           //测试功能
    public bool ifXiuGaiGame;           //是否修改游戏
    public string ErrorLogStr;          //错误日志

    public bool TestWeiTuo;
    public bool test111Status;



    //在Start方法之前执行
    void Awake() {
        
        //保证切换场景以下预设体不消失
        for (int i = 1; i <= Obj_Keep.Length; i++) {
            DontDestroyOnLoad(Obj_Keep[i-1]);
        }
        
        //设置路径
        Get_XmlPath = Application.dataPath + "/GameData/Xml/Get_Xml/";        //只读Xml的位置
        Set_XmlPath = Application.dataPath + "/GameData/Xml/Set_Xml/";        //可读可存Xml的位置

        //加载游戏首次缓存数据集
        //Game_PublicClassVar.Get_function_DataSet.DataSet_AllReadXml();

        //临时数据
        //初始化角色ID
        RoseID = Game_PublicClassVar.Get_wwwSet.RoseID;
        SourceSize = 1.0f;                                               //设置音效的大小值
        Rose_PublicSkillCDTime = 1;                                      //设置技能公共冷却CD为1秒
        RoseBagMaxNum = 90;                                              //暂定背包格子为72个
        RosePastureBagMaxNum = 60;
        //RoseHouseMaxNum = 70;                                          //仓库背包的格子
        RosePetMaxNum = 15;                                              //宠物最大数量
        Game_PublicClassVar.Get_function_AI.Pet_UpdateLvMaxNum();        //更新当前等级宠物数量

        RosePastureMaxNum = 30;

        //Debug.Log("RosePetLvMaxNum = " + RosePetLvMaxNum);
        OBJ_UI_Set = GameObject.FindWithTag("UI_Set");
		Obj_UI_FunctionOpen = GameObject.FindWithTag("UI_FunctionOpen");
        if (GameObject.FindWithTag("DropItemSet") != null) {
            Tr_Drop = GameObject.FindWithTag("DropItemSet").transform;
        }

        //初始化进入界面
        //EnterGameStatus = true;
        
        //初始化任务列表是打开的
        RoseTaskListShow_1 = "1";
        RoseTaskListShow_2 = "1";
        RoseTaskListShow_3 = "1";

        //设定建筑资源获取间隔时间
        HintShowIntervalTime = 10.0f;

        GameSourceValue = 0;  //默认声音正常,以后需要添加配置修改这里即可

        if (Application.loadedLevelName == "EnterGame") {
            //读取当前声音状态
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
            {
                GameSourceValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SourceSize", "ID", RoseID, "RoseConfig"));
                GameSourceValue_YinXiao = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SourceSize_YinXiao", "ID", RoseID, "RoseConfig"));
                //GameDanJiStatus = 0;    //每次进游戏默认为弱联网模式  
                string GameDanJiStatusStr =Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameModel", "ID", RoseID, "RoseConfig");
                if (GameDanJiStatusStr == "") {
                    GameDanJiStatusStr = "0";
                }
                GameDanJiStatus = int.Parse(GameDanJiStatusStr);

                //觉醒状态
                Game_PublicClassVar.Get_function_Rose.Rose_JueXingStatus();
            }
        }

        //初始化支付状态
        PayStr = "";
        PayStatus = false;

        //默认难度
        GameNanduValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_4", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

    }

	// Use this for initialization
	void Start () {

        //测试数据（建筑相关）
        //Minute_Gold = 10.0f;    //每分钟金币产出,测试数据

        //获取离线发送资源状态
        //gameOffResourceStatus = Game_PublicClassVar.Get_wwwSet.GameOffResourceStatus;

        //检测外挂
        Game_PublicClassVar.Get_wwwSet.IfUpdataGameWaiGua = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (TestWeiTuo) {
            TestWeiTuo = false;
            GameObject uiCommonHint = (GameObject)Instantiate(Obj_UICommonHint);
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("进入委托", testWeiTuoA, null);
        }


        if (test111Status) {
            test111Status = false;
            //test111();
            //发送时间效验
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000004, "");
        }

        //更新NPC状态
        if (NpcShowValueStatus) {
            if (npcShowValueNum>=1)
            {
                npcShowValueNum = 0;
                NpcShowValueStatus = false;
                //测试
                //MapKillMonsterNum = 0;
            }
            npcShowValueNum = npcShowValueNum + 1;
        }
        /*
        if (MapKillMonsterNum >= 5)
        {
            NpcShowValueStatus = true;

        }
        */
        if (!Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
        {
            //Debug.Log("Updata读取数据表中……");
            return;
        }
		//当前帧执行完重置其状态
		if (NpcTaskMainUIShow) {
			NpcTaskMainUIShowSum = NpcTaskMainUIShowSum +1;
			if(NpcTaskMainUIShowSum > 1){
				NpcTaskMainUIShow=false;
				NpcTaskMainUIShowSum =0;
			}
		}

		//当前背包内容全更新重置其状态
		if (UpdataBagAll) {
            //Debug.Log("更新！！！");
			UpdataBagAllSum = UpdataBagAllSum +1;
			if(UpdataBagAllSum > 2){
				UpdataBagAll = false;
				UpdataBagAllSum = 0;
			}
		}

        if (UpdataRoseItem) {
            //Debug.Log("更新！！！");
            updataRoseItemSum = updataRoseItemSum + 1;
            if (updataRoseItemSum > 1)
            {
                UpdataRoseItem = false;
                updataRoseItemSum = 0;
                Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
                Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
            }
        }

        //切换场景,更新自身的位置
        if (EnterScenceStatus) {

            doorWayNextDelayTime = doorWayNextDelayTime + Time.deltaTime;

            if (doorWayNextDelayTime > 1)
            {
                Obj_Rose.SetActive(true);
                EnterScenceStatus = false;

                /*
                GameObject scenceRosePosition = GameObject.FindWithTag("ScenceRosePosition");
                GameObject obj_Position = scenceRosePosition.transform.Find(EnterScencePositionName).gameObject;
                 
                //Debug.Log("obj_Position = " + obj_Position.transform.position);
                Obj_Rose.transform.position = obj_Position.transform.position;
                 
                //Debug.Log("设置的位置为" + obj_Position.transform.position + ",角色的坐标为：" + Obj_Rose.transform.position);
                EnterScencePositionName = "";       //清空
                */

                doorWayNextDelayTime = 0;
            }

        }
        if (Obj_Rose != null) {
            //Debug.Log("角色的坐标为：" + Obj_Rose.transform.position);
        }


        //更新建筑ID
        if (!UpdataBuildingIDStatus)
        {
            UpdataBuildingIDStatus = true;
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus == true) {
                try
                {
                    updateBuildingID();
                }
                catch {
                    Debug.Log("报错！！！更新建筑报错");
                }
            }
        }

        //更新背包选中状态
        if (RoseBagSpaceSelectStatus) {
            if (RoseBagSpaceSelectUpdateNum >= 1)
            {
                RoseBagSpaceSelectStatus = false;
                RoseBagSpaceSelectUpdateNum = 0;
            }
            else {
                RoseBagSpaceSelectUpdateNum = RoseBagSpaceSelectUpdateNum + 1;
            }
        }

        //每分钟在线收益   Minute_TimeSum此值初始值一定要为0！！！要不服务器效验加速会封号
        Minute_TimeSum = Minute_TimeSum + Time.deltaTime;
        if (Minute_TimeSum >= 60)
        {
            Minute_TimeSum = 0.0f;

            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {

                //写入活跃任务
                Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "10", "1");

                //每分钟执行一次
                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("109", "0", "1");

                //验证防挂机
                addLinkTime();

                //存储牧场时间
                Game_PublicClassVar.Get_function_Pasture.SavePastureTime(60);

                //存储牧场商人时间
                Game_PublicClassVar.Get_function_Pasture.CostTimePastureTrader(60);

                //存储矿区订单
                Game_PublicClassVar.Get_function_Pasture.PastureKuang_AddTime(1);

                //发送时间效验
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000004, "");

                //牧场交互时间降低
                Game_PublicClassVar.Get_function_Pasture.CostPastureJiaoHuTime(60);
                Game_PublicClassVar.Get_function_Pasture.CostPastureGongGaoUpdateTime(60);

                //Debug.Log("当前时间:" + DateTime.Now.ToString());
                //Debug.Log("当前时间戳:" + WWWSet.GetTimeStamp());

                if (JiaSuYanZheng_LastTime > 10000) {

                    string nowTimeStr = WWWSet.GetTimeStamp();
                    long nowTimeLong = long.Parse(nowTimeStr);
                    long chaValue = nowTimeLong - JiaSuYanZheng_LastTime;
                    //Debug.Log("chaValue = " + chaValue);
                    int yanzhengNum = 45;

                    if (chaValue <= yanzhengNum)
                    {
                        Debug.Log("怀疑开启加速");

                        jiaSuYanZhengAddNum = jiaSuYanZhengAddNum + 1;
                    }
                    else {
                        jiaSuYanZhengAddNum = 0;
                    }
                }

                /*
                if () { 
                    

                
                }
                */

                JiaSuYanZheng_LastTime = long.Parse(WWWSet.GetTimeStamp());

                if (jiaSuYanZhengAddNum >= 1) {

                    Game_PublicClassVar.Get_gameLinkServerObj.HintMsgStatus_Exit = true;
                    Game_PublicClassVar.Get_gameLinkServerObj.HintMsgText_Exit = "游戏速度异常!断开与游戏连接...";

                    //强制退出游戏
                    Game_PublicClassVar.Get_wwwSet.forceExitGameStatus = true;
                    Game_PublicClassVar.Get_wwwSet.forceExitGameTimeSum = 0;

                    jiaSuYanZhengAddNum = 0;
                }

            }

            //minuteValue = HintShowIntervalTime /60.0f;
            //调用持久化数据
            /*
            updataPlayPrefsData();

            //添加资源
            
            CountryExp_Add = (int)(CountryExp_Add + MinuteCountryExp * minuteValue);
            CountryHonor_Add = (int)(CountryHonor_Add + MinuteCountryHonor * minuteValue);
            //持久化数据
            PlayerPrefs.SetInt("CountryExp_Add", CountryExp_Add);
            PlayerPrefs.SetInt("CountryHonor_Add", CountryHonor_Add);

            //判定当前场景是否是建筑场景
            if (Application.loadedLevelName == "EnterGame") {
                BuildingHintGirdStatus = true;
                //Debug.Log("开启存储资源数据");
            }

            ActTimeSet();       //更新怪物进攻时间,每10秒
            */
        }

        //每小时在线收益
        /*
        hour_TimeSum = hour_TimeSum + Time.deltaTime;
        if (hour_TimeSum >= 3600.0f)
        {
            hour_TimeSum = 0;

            //获取当前农民数量及上限
            string farmerNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmerNumMax", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            string farm = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Farm", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            if (int.Parse(farm) < int.Parse(farmerNumMax)) {

                //获取差值,当差值快满足上限时,只弥补差值部分
                int differenceValue = int.Parse(farmerNumMax) - int.Parse(farm);
                if (differenceValue < Farm_Add) {
                    Farm_Add = differenceValue;
                }

                Farm_Add = (int)(Farm_Add + MinuteFarm);
                PlayerPrefs.SetInt("Farm_Add", Farm_Add);

                buildingHintGird("农民", Farm_Add);
                //更新界面人数显示
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Obj_TrainCamp.GetComponent<UI_TrainCamp>().UpdataShowStatus = true;
            }
        }
        */

        //更新体力
        tiLi_TimeSum = tiLi_TimeSum + Time.deltaTime;
        //每5分钟更新1点体力
        if(tiLi_TimeSum>=300.0f)
        {
            tiLi_TimeSum = 0;
            //增加1点体力
            Game_PublicClassVar.Get_function_Rose.AddTili(1);
            //增加1点活力
            Game_PublicClassVar.Get_function_Rose.AddHuoLi(1);

            //扣除坐骑饱食度
            int costValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 2);
            if (UnityEngine.Random.value <= 0.3f) {
                Game_PublicClassVar.Get_function_Pasture.ZuoQiCostBaoShiDu(costValue);
            }
        }

        /*
        if (BuildingHintGirdStatus) {

            buildingHintGird("繁荣度", CountryExp_Add);
            buildingHintGird("荣誉", CountryHonor_Add);
            
            BuildingHintGirdStatus = false;

            //开启存储数据
            updateBuildingResource();   //更新当前存储资源

            Game_PublicClassVar.Get_function_Country.addCoutryExp(CountryExp_Add);
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryHonor", (CountryHonor_Add + CountryHonor).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

            //清空数据
            CountryExp_Add = 0;
            CountryHonor_Add = 0;
            PlayerPrefs.SetInt("CountryExp_Add", 0);
            PlayerPrefs.SetInt("CountryHonor_Add", 0);
        }
        */

         //判定离线资源是否已经发放
        if (!Game_PublicClassVar.Get_wwwSet.GameOffResourceStatus)
        {
            //判定当前场景是否是建筑场景
            if (Application.loadedLevelName != "StartGame")
            {
                //判定是否读取到世界时间
                if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus)
                {
                    UpdataOffGameResource();
                }
            }
        }


        //判定当前凌晨更新游戏数据
        if (Game_PublicClassVar.Get_wwwSet.DayUpdataStatus) {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {
                
                dayClearnGameData(false, true);    //清空每日数据
            }
        }

        //判定要塞防御状态
        /*
        if (OpenEmenyActStatus) {
            if (!emenyActStatus)
            {
                //开始倒计时
                emenyActTime = emenyActTime - Time.deltaTime;
                //if()
                if (emenyActTime <= 0.0f)
                {
                    //屏蔽要塞 emenyActStatus = true;
                }
            }
        }
        */
        //测试功能
        if (testFunction) {
            testFunction = false;
            //dayClearnGameData();
        }
	}

    void LateUpdate() {

        if (!Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
        {
            //Debug.Log("LateUpdate读取数据表中……");
            return;
        }

		//更新当前角色属性
		if (UpdataRoseProperty) {
			UpdataRoseProperty = false;
			Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty();
        }

        //更新角色buff属性
        if (UpdataRoseBuffProperty) {
            UpdataRoseBuffProperty = false;
            Game_PublicClassVar.Get_function_Rose.UpdateRoseBuffProperty();
        }

        //怪物选中状态开启后执行一个循环后关闭
        if (UpdataSelectedStatus) {
            UpdataSelectedStatusSum = UpdataSelectedStatusSum + 1;
            if (UpdataSelectedStatusSum > 1) {
                UpdataSelectedStatus = false;
                UpdataSelectedStatusSum = 0;
            }
        }

        //刷新技能公共冷却时间
        if (Rose_PublicSkillCDStatus)
        {
            Rose_PublicSkillCDTimeSum = Rose_PublicSkillCDTimeSum + Time.deltaTime;
            if (Rose_PublicSkillCDTimeSum >= Rose_PublicSkillCDTime)
            {
                Rose_PublicSkillCDTimeSum = 0.0f;
                Rose_PublicSkillCDStatus = false;
            }
        }

        //更新主界面的Icon显示
        if (UpdataMainSkillUI) {
            if (UpdataMainSkillUISum >= 2) {
                UpdataMainSkillUI = false;
                UpdataMainSkillUISum = 0;
            }
            UpdataMainSkillUISum = UpdataMainSkillUISum + 1;
        }

        //更新任务显示
        if (UpdatTaskStatus)
        {
            UpdatTaskStatus = false;
            Game_PublicClassVar.Get_function_Task.updataTaskItemID();
        }

        //更新成就选中
        if (Rose_ChengJiuXuanZhongStatus) {
            if (rose_ChengJiuXuanZhongNum >= 1)
            {
                Rose_ChengJiuXuanZhongStatus = false;
                rose_ChengJiuXuanZhongNum = 0;
            }
            else {
                rose_ChengJiuXuanZhongNum = rose_ChengJiuXuanZhongNum + 1;
            }
        }

        //更新怪物死亡时间
        /*
        deathMonsterTimeSum = deathMonsterTimeSum + Time.deltaTime;
        if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {
            //Debug.Log("读取死亡时间");
            if (deathMonsterTimeSum >= 1.0f)
            {
                deathMonsterTimeSum = 0;
                //更新怪物复活时间
                Game_PublicClassVar.Get_function_AI.UpdataMonsterDeathTime(deathMonsterTimeSum);
                //Debug.Log("读取死亡时间完毕");
            }
        }
        */
        /*
        //战斗界面切换建筑界面是注销的Obj
        if (DestroyKeepObj) {

            //保证切换场景以下预设体不消失
            for (int i = 1; i <= Obj_Keep.Length; i++)
            {
                DontDestroyOnLoad(Obj_Keep[i - 1]);
            }
        
        }
        */
        //村民的信
        /*
        if (!CunMinStatus) {
            CunMinSaveSum = CunMinSaveSum + Time.deltaTime;
            //每10秒记录一次事件
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {
                if (CunMinSaveSum > 3600.0f)
                {
                    float specialEventTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpecialEventTime", "ID", RoseID, "RoseData"));
                    specialEventTime = specialEventTime + CunMinSaveSum;
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SpecialEventTime", specialEventTime.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                    CunMinEmailSum = specialEventTime;
                    CunMinSaveSum = 0;
                }
            }
        }
        //设置时间触发
        if (CunMinEmailSum > 30) {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {
                if (Random.value <= 1)
                {
                    //触发村民的信
                    if (Application.loadedLevelName != "StartGame") {
                        if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnCunMinDeXin.GetComponent<UI_CunMinDeXinBtn>().EventStatus == false)
                        {
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnCunMinDeXin.SetActive(true);
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnCunMinDeXin.GetComponent<UI_CunMinDeXinBtn>().EventStatus = true;
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnCunMinDeXin.GetComponent<UI_CunMinDeXinBtn>().EventUpdaDataStatus = true;
                            CunMinEmailSum = 0; //清空
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SpecialEventTime", CunMinEmailSum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            //CunMinStatus = true;
                        }
                    }
                }
            }
        }
        */
        //每10秒执行一次
        secUpdataTimeSum = secUpdataTimeSum + Time.deltaTime;
        if (secUpdataTimeSum >= 10)
        {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {

                if (!RoseDayPracticeRewardStatus)
                {
                    //写入更新数据
                    string expTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ExpTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    string goldTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_GoldTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    float writeExpTime = float.Parse(expTime) + secUpdataTimeSum;
                    float writeGoldTime = float.Parse(goldTime) + secUpdataTimeSum;
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ExpTime", writeExpTime.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_GoldTime", writeGoldTime.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                }

                //写入抽卡时间
                float chouKaTime_One = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_One", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
                float chouKaTime_Ten = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_Ten", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
                chouKaTime_One = chouKaTime_One + secUpdataTimeSum;
                chouKaTime_Ten = chouKaTime_Ten + secUpdataTimeSum;

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", chouKaTime_One.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", chouKaTime_Ten.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                //Debug.Log("更新数据");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                //写入怪物死亡时间
                string deathMonsterIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string[] deathMonsterID = deathMonsterIDList.Split(';');
                string deathMonsterIDStr = "";
                if (deathMonsterIDList != "")
                {
                    //循环获取当前怪物死亡时间
                    for (int i = 0; i <= deathMonsterID.Length-1; i++)
                    {
                        if (deathMonsterID[i] != ""&& deathMonsterID[i] !="0")
                        {
                            string monsterOnlyID = deathMonsterID[i].Split(',')[0];                             
                            float monsterTime = float.Parse(deathMonsterID[i].Split(',')[1]);               //获取在线BOSS复活时间
                            float monsterOffLineTime = float.Parse(deathMonsterID[i].Split(',')[2]);        //获取离线BOSS复活时间

                            //获取怪物ID
                            string monsterID = "";
                            if (deathMonsterID[i].Split(',').Length >= 4)
                            {
                                monsterID = deathMonsterID[i].Split(',')[3];
                            }

                            //如果离线时间少于在线时间,在线时间取离线时间的值
                            if (monsterOffLineTime < monsterTime) {
                                monsterTime = monsterOffLineTime;
                            }
                            monsterTime = monsterTime - secUpdataTimeSum;
                            monsterOffLineTime = monsterOffLineTime - secUpdataTimeSum;
                            if (monsterTime <= 0)
                            {
                                monsterTime = 0;
                            }
                            else
                            {
                                deathMonsterIDStr = deathMonsterIDStr + monsterOnlyID + "," + monsterTime + ","+ monsterOffLineTime + "," + monsterID + ";";
                            }
                        }
                    }
                    //记录怪物死亡数据
                    if (deathMonsterIDStr != "")
                    {
                        deathMonsterIDStr = deathMonsterIDStr.Substring(0, deathMonsterIDStr.Length - 1);
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", deathMonsterIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                    }
                    else
                    {
                        deathMonsterIDStr = "";
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", deathMonsterIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                    }
                }

                //清空值
                secUpdataTimeSum = 0;
            }
        }

        //每秒执行一次
        secUpdateTimeSum_One = secUpdateTimeSum_One + Time.deltaTime;
        if (secUpdateTimeSum_One >= 1) {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
            {
                Game_PublicClassVar.Get_function_AI.Pet_CostZhaoHuanCD(1);
                Game_PublicClassVar.Get_function_AI.Pet_CostZhaoHuanWuDiCD(1);
                secUpdateTimeSum_One = 0;
                clearnBabyNum = clearnBabyNum + 1;

            }
        }

        if (clearnBabyNum >= 45) {
            clearnBabyNum = 0;
            //清空进入地图遇到宝宝的次数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EnterMapBaby", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        }

        //成就检测
        if (ChengJiuJianCeStatus)
        {
            ChengJiuJianCeTimeSum = ChengJiuJianCeTimeSum + Time.deltaTime;
            if (ChengJiuJianCeTimeSum >= 1) {

                //检测是否有成就完成
                string comChengJiuIDSet = Game_PublicClassVar.Get_function_Task.ChengJiu_JianCeTargetSonTypeChengJiuID(ChengJiuJianCeChengJiuIDStr);
                if (comChengJiuIDSet != "")
                {
                    //Debug.Log("comChengJiuIDSet = " + comChengJiuIDSet);
                    string[] comChengJiuIDList = comChengJiuIDSet.Split(';');
                    for (int i = 0; i < comChengJiuIDList.Length; i++)
                    {
                        //写入成就ID
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteComChengJiuID(comChengJiuIDList[i]);
                        //弹出成就提示
                        Game_PublicClassVar.Get_function_Task.ChengJiu_ComHint(comChengJiuIDList[i]);
                    }
                }

                ChengJiuJianCeTimeSum = 0;
                ChengJiuJianCeStatus = false;
                ChengJiuJianCeChengJiuIDStr = "";
            }
        }
    }


    private void addLinkTime() {
        string expTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayLinkTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (expTime == "")
        {
            expTime = "0";
        }
        int linkTime = int.Parse(expTime) + 1;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayLinkTime", linkTime.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        //每天开启超过5个小时开启验证
        if (linkTime >= 300) {
            //开启验证
            if (Application.loadedLevelName != "EnterGame") {
                Game_PublicClassVar.Get_wwwSet.guaJiYanZhengTime = Game_PublicClassVar.Get_wwwSet.guaJiYanZhengTime + 1;
            }
        }

        //每60分钟效验一次
        if (Game_PublicClassVar.Get_wwwSet.guaJiYanZhengTime >= 60) {
            Game_PublicClassVar.Get_wwwSet.guaJiYanZhengTime = 0;
            if (GuaJiObj != null) {
                if (GuaJiObj != null) {
                    GameObject obj = (GameObject)Instantiate(GuaJiObj);
                    try {
                        obj.transform.SetParent(OBJ_UI_Set.transform);
                        obj.transform.localPosition = new Vector3(0, 0, 0);
                        obj.transform.localScale = new Vector3(1, 1, 1);
                    }
                    catch (Exception ex) {
                        Destroy(obj);
                    }
                }
            }
        }

        //判定在线时间,根据不同的在线时间设定不同的奖励时间


    }
    /*
    void OnGUI() {
        //testStrPay = "111111" + "\n" + "222222";
        GUI.Box(new Rect(10, 10, 500, 670), testStrPay);
    }
    */
    //每天凌晨需要清空的游戏数据(参数1：是否第一次进入游戏  参数2：是否为当天24点进入的游戏)
    public void dayClearnGameData(bool ifFirst = false, bool ifUpdataTime24 = false)
    {
        Game_PublicClassVar.Get_wwwSet.updataOffGameTimeStatus = true;     //更新离线时间
        Game_PublicClassVar.Get_wwwSet.DayUpdataStatus = false;

        if (Game_PublicClassVar.gameLinkServer.ServerLinkStatus) {

            //清空每日次数发送给服务器
            string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Pro_ComStr_4 com4 = new Pro_ComStr_4();
            com4.str_1 = zhanghaoID;
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000111, com4);

            //清空每日奖励数据
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ExpNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_GoldNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ExpTime", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_GoldTime", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //清空分享数据
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_1", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_2", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_3", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //清空每日副本次数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_FuBenNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MeiRiCangBaoNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //清空每日任务
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskValue", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //清空每日击杀
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayKillNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //清空每日师门任务
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiMenTaskNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

            //清空七天登录
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QiTianDengLuStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

            //写入月卡每日领取数据
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YueKaDayStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            //每日礼包
            string meiRiLiBaoStr = Game_PublicClassVar.Get_function_Rose.GetMeiRiLiBao();
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MeiRiLiBao", meiRiLiBaoStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //藏宝图次数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MeiRiCangBaoTrueChestNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //副本_1 次数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_1_DayNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_2_DayNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //大秘境次数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJing_DayNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //存储每日的付费金额
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayPayValue", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //存储每日签到数据
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Com_Day", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Pay_Day", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //清理每日献祭次数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiXianJiNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            string qianDaoNum_Com = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            string qianDaoNum_Pay = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Pay", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //清理副本次数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_ShangHaiNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_ShangHaiEveryRewardID", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //清理爆率状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RankBaoLvStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            
            //清理魔塔
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TowerCeng", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TowerCengReward", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TowerCengTime", "900", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FengYinCeng", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FengYinCengNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            if (qianDaoNum_Com == "") {
                qianDaoNum_Com = "0";
            }

            if (qianDaoNum_Pay == "") {
                qianDaoNum_Pay = "0";
            }

            if (int.Parse(qianDaoNum_Com) >= 30) {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Com", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            }

            if (int.Parse(qianDaoNum_Pay) >= 30){
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Pay", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            }

            if (int.Parse(qianDaoNum_Pay) >= int.Parse(qianDaoNum_Com)) {
                qianDaoNum_Pay = qianDaoNum_Com;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Pay", qianDaoNum_Pay, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            }

            //清理每日累计抽卡
            /*
            string dayChouKaNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ChouKaNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            if (dayChouKaNum == "")
            {
                dayChouKaNum = "0";
            }
            */

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ChouKaNum", "0" , "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ChouKaNumReward", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetTiaoZhanNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RankBaoLv", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayLinkTime", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JiaoHuNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //清空活动相关
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HuoDong_ShouLie_Reward", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhengYingRewardFuLi", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhengYingEveryRewardFuLi", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("KuangLvDuoNum", "0","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("KuangLvDuoChongZhiNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //先存一下 防止下面任务出错
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

            //增加冒险家积分
            //Game_PublicClassVar.Get_function_Rose.AddMaoXianExp(10);

            //每天清空墓碑
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMuBeiStr", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

            //清空进入地图遇到宝宝的次数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EnterMapBaby", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

            //清空登陆数据
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DengLuDayStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

            //清空黑市刷新次数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HeiShiShuaXinNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

            //存入每日奖励日期
            //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_2", nowDayValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            //清空每日挂机次数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayGuaJiNum","0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

            //清空牧场相关
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureDuiHuanFreeNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

            //每天凌晨增加60体力和活力
            Game_PublicClassVar.Get_function_Rose.AddHuoLi(60);
            Game_PublicClassVar.Get_function_Rose.AddTili(60);

            //每日任务
            /*
            string[] dayTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DayTask_ID", "GameMainValue").Split(';');
            string dayTaskIDStr = "";
            string dayTaskValueStr = "";

            //随机三个任务ID
            int DayTask_Num = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DayTask_Num", "GameMainValue"));
            float taskNum = dayTaskID.Length - 0.01f;
            for (int i = 0; i <= DayTask_Num-1; i++)
            {
                int randValue = (int)(Random.value * taskNum);
                //获取任务概率
                float taskRandValue = Random.value;
                string writeTaskID = dayTaskID[i];
                string writeTaskID_Next = writeTaskID;
                float triggerPro = 0;
                int kajiNum = 0;
                do{
                    writeTaskID = writeTaskID_Next;
                    triggerPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerPro", "ID", writeTaskID, "TaskCountry_Template"));
                    //Debug.Log("任务概率：" + taskRandValue + "/" + triggerPro);
                    writeTaskID_Next = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextTask", "ID", writeTaskID, "TaskCountry_Template");

                    //防止卡机
                    if (kajiNum >= 10) {
                        taskRandValue = -1;
                    }
                    if (writeTaskID_Next == "0") {
                        taskRandValue = -1;
                    }
                } while (taskRandValue >= triggerPro);

                dayTaskIDStr = dayTaskIDStr + writeTaskID + ";";
                dayTaskValueStr = dayTaskValueStr + "0" + ";";
            }
            */

            //添加隐藏任务
            /*
            string countryLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            float hideTaskPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideTaskPro", "ID", countryLv, "Country_Template"));
            //hideTaskPro = 1;
            if (Random.value <= hideTaskPro) {
                //Debug.Log("触发隐藏任务");
                //获取隐藏任务
                string[] DayHideTaskList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DayTask_HideID", "GameMainValue").Split(';');
                float hidTaskNum = DayHideTaskList.Length - 0.01f;

                string DayHideTaskListStr = "";
                for (int i = 0; i <= 10; i++) {
                    int randValue = (int)(Random.value * hidTaskNum);
                    DayHideTaskListStr = DayHideTaskList[randValue];

                    string taskTriggerType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerType", "ID", DayHideTaskListStr, "TaskCountry_Template");
                    if (taskTriggerType == "1") {
                        int taskLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerValue", "ID", DayHideTaskListStr, "TaskCountry_Template"));
                        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= taskLv)
                        {
                            i = 11; //跳出循环
                        }
                        else {
                            DayHideTaskListStr = "";
                        }
                    }
                }
                //如果DayHideTaskListStr为0,就默认指定一个任务
                if (DayHideTaskListStr == "") {
                    DayHideTaskListStr = "30001";
                }

                dayTaskIDStr = dayTaskIDStr + DayHideTaskListStr + ";";
                dayTaskValueStr = dayTaskValueStr + "0;";
            }
            */

            //活跃任务
            string[] dayTaskID = "100001,100002,100003,100004,100005,100006,100007,100008,100009,200001,200101,200201,200301,300101,300201,300301".Split(',');
            string dayTaskIDStr = "";
            string dayTaskValueStr = "";

            for (int i = 0; i < dayTaskID.Length; i++)
            {
                //int randValue = (int)(Random.value);
                //获取任务概率
                float taskRandValue = UnityEngine.Random.value;
                string writeTaskID = dayTaskID[i];
                string writeTaskID_Next = writeTaskID;
                float triggerPro = 0;
                int kajiNum = 0;
                do
                {
                    writeTaskID = writeTaskID_Next;
                    triggerPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerPro", "ID", writeTaskID, "TaskCountry_Template"));
                    //Debug.Log("任务概率：" + taskRandValue + "/" + triggerPro);
                    writeTaskID_Next = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextTask", "ID", writeTaskID, "TaskCountry_Template");

                    //防止卡机
                    if (kajiNum >= 10)
                    {
                        taskRandValue = -1;
                    }
                    if (writeTaskID_Next == "0")
                    {
                        taskRandValue = -1;
                    }

                } while (taskRandValue >= triggerPro);

                dayTaskIDStr = dayTaskIDStr + writeTaskID + ";";
                dayTaskValueStr = dayTaskValueStr + "0" + ";";
            }


            if (dayTaskIDStr != "")
            {
                dayTaskIDStr = dayTaskIDStr.Substring(0, dayTaskIDStr.Length - 1);
            }

            if (dayTaskValueStr != "")
            {
                dayTaskValueStr = dayTaskValueStr.Substring(0, dayTaskValueStr.Length - 1);
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskID", dayTaskIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskValue", dayTaskValueStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskHuoYueValue", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskCommonHuoYueRewardID", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");


            //清除登录数据
            PlayerPrefs.SetInt("FangChenMi_Time", 0);
            PlayerPrefs.Save();

            //发送重置记录消息
            //发送服务器记录消息
            //string sendStr = "";
            //string lastday111 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            //sendStr = "上次登录时间=" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime + ";本次登录时间=" + Game_PublicClassVar.Get_wwwSet.DataTime + "; 存储今日天数= " + lastday111;
            //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001017, "重置天数:" + sendStr);
        }
        //Debug.Log("nowDayValue = " + nowDayValue);

        //请求重置今日的信息
        //请求今天第一称号
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001036, "");

        //清空爆率buff显示
        if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set != null) {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuffListShowSet.GetComponent<UI_BuffShowListSet>().DeleteBaoLvBuff();
        }

        //清空身份验证次数
        PlayerPrefs.SetInt("FangChenMi_YanZhengNum", 0);
        PlayerPrefs.Save();

    }

    void updateBuildingID() {

        //更新建筑ID
        GovernmentLvID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GovernmentLvID", "ID", RoseID, "RoseBuilding");
        ResidentHouseID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ResidentHouseID", "ID", RoseID, "RoseBuilding");
        FarmlandID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmlandID", "ID", RoseID, "RoseBuilding");
        LoggingFieldID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LoggingFieldID", "ID", RoseID, "RoseBuilding");
        StonePitID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StonePitID", "ID", RoseID, "RoseBuilding");
        SmeltingPlantID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SmeltingPlantID", "ID", RoseID, "RoseBuilding");
        CollegeID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CollegeID", "ID", RoseID, "RoseBuilding");
        FortressID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FortressID", "ID", RoseID, "RoseBuilding");
        TrainCampID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TrainCampID", "ID", RoseID, "RoseBuilding");

        /*
        BuildingGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingGold", "ID", RoseID, "RoseBuilding"));
        Farm = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Farm", "ID", RoseID, "RoseBuilding"));
        Food = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Food", "ID", RoseID, "RoseBuilding"));
        Wood = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Wood", "ID", RoseID, "RoseBuilding"));
        Stone = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Stone", "ID", RoseID, "RoseBuilding"));
        Iron = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Iron", "ID", RoseID, "RoseBuilding"));

        
        MinuteBuildingGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteBuildingGold", "ID", RoseID, "RoseBuilding"));
        MinuteFarm = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteFarm", "ID", RoseID, "RoseBuilding"));
        MinuteFood = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteFood", "ID", RoseID, "RoseBuilding"));
        MinuteWood = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteWood", "ID", RoseID, "RoseBuilding"));
        MinuteStone = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteStone", "ID", RoseID, "RoseBuilding"));
        MinuteIron = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteIron", "ID", RoseID, "RoseBuilding"));
        MinuteRoseExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteRoseExp", "ID", RoseID, "RoseBuilding"));
        */

        CountryExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryExp", "ID", RoseID, "RoseDayReward"));
        CountryHonor = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryHonor", "ID", RoseID, "RoseDayReward"));


        //更新游戏分钟产出资源
        Game_PublicClassVar.Get_function_Building.UpdataMinuteResource();
        Game_PublicClassVar.Get_function_Country.UpdataMinuteData();

    }


    //更新建筑资源
    void updateBuildingResource() {

        /*
        BuildingGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingGold", "ID", RoseID, "RoseBuilding"));
        Farm = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Farm", "ID", RoseID, "RoseBuilding"));
        Food = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Food", "ID", RoseID, "RoseBuilding"));
        Wood = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Wood", "ID", RoseID, "RoseBuilding"));
        Stone = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Stone", "ID", RoseID, "RoseBuilding"));
        Iron = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Iron", "ID", RoseID, "RoseBuilding"));
        */

        CountryExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryExp", "ID", RoseID, "RoseDayReward"));
        CountryHonor = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryHonor", "ID", RoseID, "RoseDayReward"));

    }

    //更新持久化数据
    void updataPlayPrefsData() {

        /*
        BuildingGold_Add = PlayerPrefs.GetInt("BuildingGold_Add");
        Farm_Add = PlayerPrefs.GetInt("Farm_Add");
        Food_Add = PlayerPrefs.GetInt("Food_Add");
        Wood_Add = PlayerPrefs.GetInt("Wood_Add");
        Stone_Add = PlayerPrefs.GetInt("Stone_Add");
        Iron_Add = PlayerPrefs.GetInt("Iron_Add");
        RoseExp_Add = PlayerPrefs.GetInt("RoseExp_Add");
        */
        
        CountryExp_Add = PlayerPrefs.GetInt("CountryExp_Add");
        CountryHonor_Add = PlayerPrefs.GetInt("CountryHonor_Add");
    
    }

    void buildingHintGird(string resourceName,int resourceValue) {

        if (resourceValue > 0)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint(resourceName + "+" + resourceValue.ToString());
        }
    }

    void UpdataOffGameResource() {

        //Debug.Log("Game_PublicClassVar.Get_wwwSet.RoseID1 = " + Game_PublicClassVar.Get_wwwSet.RoseID);

        //判定是否读取到世界时间
        if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus)
        {

            Game_PublicClassVar.Get_wwwSet.GameOffResourceStatus = true;
            //Debug.Log("Game_PublicClassVar.Get_wwwSet.RoseID2 = " + Game_PublicClassVar.Get_wwwSet.LastOffGameTime);
            //获取上一次离线时间,如果小于2016表示第一次登陆将直接跳转
            if (Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Year >= 2016)
            {
                //Debug.Log("Game_PublicClassVar.Get_wwwSet.RoseID3 = " + Game_PublicClassVar.Get_wwwSet.RoseID);
                //计算离线时间(最多离线收益为24小时)
                int year = Game_PublicClassVar.Get_wwwSet.DataTime.Year - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Year;
                int month = Game_PublicClassVar.Get_wwwSet.DataTime.Month - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Month;
                int day = Game_PublicClassVar.Get_wwwSet.DataTime.Day - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Day;
                int hour = Game_PublicClassVar.Get_wwwSet.DataTime.Hour - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Hour;
                int minute = Game_PublicClassVar.Get_wwwSet.DataTime.Minute - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Minute;
                int second = Game_PublicClassVar.Get_wwwSet.DataTime.Second - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Second;

                System.TimeSpan offGameTime = Game_PublicClassVar.Get_wwwSet.DataTime - Game_PublicClassVar.Get_wwwSet.LastOffGameTime;

                //判定当前是否第一次登陆
                if (year >= 1) {
                    ifOneGameToDay = true;
                }
                if (month >= 1) {
                    ifOneGameToDay = true;
                }
                if (day >= 1) {
                    ifOneGameToDay = true;
                }

                //Debug.Log("离线时间戳 = " + offGameTime.TotalSeconds);
                int offGameTimeInt = (int)(offGameTime.TotalSeconds);

                //发送资源时减去已经开启的时间
                offGameTimeInt = offGameTimeInt - (int)(Time.time);

                //防止离线时间为负数
                if (offGameTimeInt <= 0)
                {
                    offGameTimeInt = 0;
                }

                //写入抽卡时间
                float chouKaTime_One = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_One", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
                float chouKaTime_Ten = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_Ten", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
                chouKaTime_One = chouKaTime_One + offGameTimeInt;
                chouKaTime_Ten = chouKaTime_Ten + offGameTimeInt;

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", chouKaTime_One.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", chouKaTime_Ten.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                //循环减去判定怪物的离线复活时间
                Game_PublicClassVar.Get_function_AI.UpdataMonsterDeathOffLineTime(offGameTimeInt);

                Debug.Log("离线时间：" + offGameTimeInt);
                Game_PublicClassVar.Get_wwwSet.writeOffGameTime(0); //写入离线时间记录

                //发送离线获得体力和活力
                int addTili = (int)(offGameTimeInt / 300.0f);
                //Debug.Log("addTili=" + addTili);
                Game_PublicClassVar.Get_function_Rose.AddTili(addTili);
                Game_PublicClassVar.Get_function_Rose.AddHuoLi(addTili);

                //发送离线储备经验和点数
                Game_PublicClassVar.Get_function_Rose.AddOffLinkReward(offGameTimeInt);

                //存储牧场离线时间
                Game_PublicClassVar.Get_function_Pasture.SavePastureTime(offGameTimeInt);

                //存储牧场商人时间
                Game_PublicClassVar.Get_function_Pasture.CostTimePastureTrader(offGameTimeInt);

                //存储牧场交互时间
                Game_PublicClassVar.Get_function_Pasture.CostPastureJiaoHuTime(offGameTimeInt);
                Game_PublicClassVar.Get_function_Pasture.CostPastureGongGaoUpdateTime(offGameTimeInt);

                //存储矿时间
                Game_PublicClassVar.Get_function_Pasture.PastureKuang_AddTime((int)(offGameTimeInt / 60));

                //扣除宠物饱食度(每间隔半个小时扣1点)
                int costValue = (int)(offGameTimeInt / 1800.0f);
                //最多扣除25点
                if (costValue >= 10) {
                    costValue = 10;
                }
                if (costValue <= 0) {
                    costValue = 0;
                }

                Game_PublicClassVar.Get_function_Pasture.ZuoQiCostBaoShiDu(costValue);

                //限制离线收益(8小时)
                /*
                if (offGameTimeInt > 28800)
                {
                    offGameTimeInt = 28800;
                }

                if (offGameTimeInt <= 0) {
                    offGameTimeInt = 0;
                }

                int CountryExp_off = (int)(MinuteCountryExp * offGameTimeInt / 60.0f);
                int CountryHonor_off = (int)(MinuteCountryHonor * offGameTimeInt / 60.0f);


                
                //表示资源已经发放
                gameOffResourceStatus = true;
                Game_PublicClassVar.Get_wwwSet.GameOffResourceStatus = true;

                //开启存储数据
                updateBuildingResource();   //更新当前存储资源

                //写入离线数据
                Game_PublicClassVar.Get_wwwSet.writeOffGameTime(0);
                
                Game_PublicClassVar.Get_function_Country.addCoutryExp(CountryExp_off);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryHonor", (CountryHonor_off + CountryHonor).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                
                if (offGameTimeInt > 0) {
                    //弹出离线UI
                    GameObject offGameResourceObj = (GameObject)Instantiate(Obj_OffGameResource);
                    offGameResourceObj.transform.SetParent(OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.transform);
                    offGameResourceObj.transform.localPosition = Vector3.zero;
                    offGameResourceObj.transform.localScale = new Vector3(1, 1, 1);
                    offGameResourceObj.GetComponent<UI_OffGameResource>().CountryExp = CountryExp_off;
                    offGameResourceObj.GetComponent<UI_OffGameResource>().CountryHonor = CountryHonor_off;
                }
                */
            }
        }
    }
    //记录怪物进攻时间
    public void ActTimeSet()
    {
        
        //获取当前进攻时间
        int actTimeValue = (int)(Game_PublicClassVar.Get_game_PositionVar.emenyActTime);
        //actTimeValue = 10;  //测试
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EnemyTime", actTimeValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
    }



    //支付回执(IOS回执)（客户端回执）
    public void OnPayResult(string str)
    {
        if (str != ""&& str!=null)
        {
            Debug.Log("客户端回执:" + str);
            PayIOS_Str = str;
            Debug.Log("PayIOS_Str:" + PayIOS_Str);
            //PayIOSYanZhengStr = str.Substring(50, 30);
            PayStatusIOS = true;   //开启支付状态

            //存储付费数据
            SaveIosPay(PayIOS_Str);

            //记录付费返回值
            //PlayerPrefs.SetString("iosPay_" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName, PayIOS_Str);
            //PlayerPrefs.Save();

        }
        else {
            string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            //参数：协议号,账号ID,平台ID,交易金额,交易状态,IOS查询Base64位码
            string sendStr = "IOSPayNullValue," + zhanghaoID + "," + "3" + "," + Game_PublicClassVar.Get_game_PositionVar.PayValueNow + "," + "1" + "," + "未收到验证字符";
            Debug.Log("sendStr = " + sendStr);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<GamePayLinkServer>().SendToServer(sendStr);
        }

    }

    //存储IOS支付订单编号
    public void SaveIosPay(string saveStr) {

        /*
        if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {

            string nowIOSPayStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IOSPayStr", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

            string writeStr = "";
            if (nowIOSPayStr == "" || nowIOSPayStr == null || nowIOSPayStr == "0")
            {
                writeStr = saveStr;
            }
            else
            {

                //最多存储5个订单
                string[] nowIOSPayStrList = nowIOSPayStr.Split(';');
                if (nowIOSPayStrList.Length >= 5)
                {
                    writeStr = saveStr + ";" + nowIOSPayStrList[1] + ";" + nowIOSPayStrList[2] + ";" + nowIOSPayStrList[3] + ";" + nowIOSPayStrList[4];
                }
                else
                {
                    writeStr = saveStr + ";" + nowIOSPayStr;
                }
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("IOSPayStr", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            Debug.Log("IOS增加订单记录:" + writeStr);
        }
        else
        {
        }
        */
            string nowIOSPayStr = PlayerPrefs.GetString("iosPayDingDan");

            string writeStr = "";
            if (nowIOSPayStr == "" || nowIOSPayStr == null || nowIOSPayStr == "0")
            {
                writeStr = saveStr;
            }
            else
            {

                //最多存储5个订单
                string[] nowIOSPayStrList = nowIOSPayStr.Split(';');
                if (nowIOSPayStrList.Length >= 5)
                {
                    writeStr = saveStr + ";" + nowIOSPayStrList[1] + ";" + nowIOSPayStrList[2] + ";" + nowIOSPayStrList[3] + ";" + nowIOSPayStrList[4];
                }
                else
                {
                    writeStr = saveStr + ";" + nowIOSPayStr;
                }
            }

            //临时存储在这里
            PlayerPrefs.SetString("iosPayDingDan", writeStr);
            PlayerPrefs.Save();
        
 

    }

    //删除IOS支付订单
    public void DeleteIosPay(string saveStr)
    {
        /*
        if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
        {
            string nowIOSPayStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IOSPayStr", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Debug.Log("IOS移除订单记录nowIOSPayStr:" + nowIOSPayStr);
            string[] nowIOSPayStrList = nowIOSPayStr.Split(';');
            string writeStr = "";
            for (int i = 0; i < nowIOSPayStrList.Length; i++)
            {
                if (nowIOSPayStrList[i].Contains(saveStr) == false)
                {
                    if (writeStr == "")
                    {
                        writeStr = nowIOSPayStrList[i];
                        Debug.Log("订单11111:" + writeStr);
                    }
                    else
                    {
                        writeStr = writeStr + ";" + nowIOSPayStrList[i];
                        Debug.Log("订单22222:" + writeStr);
                        Debug.Log("订单33333:" + nowIOSPayStrList[i]);
                    }
                }
                else
                {
                    //去掉相同订单
                }
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("IOSPayStr", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChaXunNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            Debug.Log("IOS移除订单记录:" + writeStr);
        }
        else
        {
        }
        */

        string nowIOSPayStr = PlayerPrefs.GetString("iosPayDingDan");
        Debug.Log("IOS移除订单记录nowIOSPayStr:" + nowIOSPayStr);
        string[] nowIOSPayStrList = nowIOSPayStr.Split(';');
        string writeStr = "";
        for (int i = 0; i < nowIOSPayStrList.Length; i++)
        {
            if (nowIOSPayStrList[i].Contains(saveStr) == false)
            {
                if (writeStr == "")
                {
                    writeStr = nowIOSPayStrList[i];
                    Debug.Log("订单11111:" + writeStr);
                }
                else
                {
                    writeStr = writeStr + ";" + nowIOSPayStrList[i];
                    Debug.Log("订单22222:" + writeStr);
                    Debug.Log("订单33333:" + nowIOSPayStrList[i]);
                }
            }
            else
            {
                //去掉相同订单
            }
        }

        PlayerPrefs.SetString("iosPayDingDan", writeStr);
        PlayerPrefs.Save();

        Debug.Log("IOS移除订单记录:" + writeStr);

        

    }



    //支付回执(IOS)(服务器返回结果效验)
    public void OnPayResultReturnIOS(string str)
    {

        PayStr = str;

        if (str != "")
        {
            PayStatus = true;   //开启支付状态
            ObscuredString returnPayValue = "";

            //获取订单号
            string[] strList = str.Split(';');

            //获取额度
            if (strList.Length >= 3)
            {
                returnPayValue = strList[2];
            }

            /*
            if (strList.Length >= 4)
            {
                //判定两次返回的订单号ID号是否相同
                PayDingDanIDNow = strList[4];
                if (PayIOSYanZhengStr != "")
                {
                    if (PayIOSYanZhengStr == PayDingDanIDNow)
                    {
                        Debug.Log("验证订单完成：" + PayDingDanIDNow);
                        //return;
                    }
                }

                PayIOSYanZhengStr = PayDingDanIDNow;
            }
            */

            //删除已经成功的订单记录
            if (PayIOS_Str != "" && PayIOS_Str != null && PayIOS_Str != "0")
            {
                DeleteIosPay(PayIOS_Str);
            }

            //判定当前是否是查询状态
            if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>() != null)
            {
                if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>().buyQueryStatus) {
                    Debug.Log("PayDingDanIDNow = " + PayDingDanIDNow);
                    DeleteIosPay(PayDingDanIDNow);
                }
            }

            PayStr = "1;" + str;
            Game_PublicClassVar.Get_function_Rose.GamePay(returnPayValue, PayDingDanIDNow);

            //PlayerPrefs.SetString("iosPay_" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName, "");
            //PlayerPrefs.Save();



        }
    }





    //支付回执(安卓)
    public void OnPayResultReturn(string str)
    {
        testStrPay = testStrPay + ";" + str;
        //Debug.Log("调用了支付返回值");
        //IOS强制支付成功,因为失败会调用下面另一个函数

        PayStr = str;

        //payStr = str;
        if (str != "")
        {
            PayStatus = true;   //开启支付状态
            ObscuredString returnPayValue = "";

            //获取订单号
            string[] strList = str.Split(';');


            if (strList.Length >= 3)
            {
                returnPayValue = strList[2];
            }

            if (strList.Length >= 4)
            {
                //判定两次返回的订单号ID号是否相同
                PayDingDanIDNow = strList[3];
                if (PayDingDanLast != "")
                {
                    if (PayDingDanLast == PayDingDanIDNow)
                    {
                        Debug.Log("订单号相同 PayDingDanIDNow = " + PayDingDanIDNow);
                        return;
                    }
                }
                PayDingDanLast = PayDingDanIDNow;
            }

            Game_PublicClassVar.Get_function_Rose.GamePay(returnPayValue, PayDingDanIDNow);

            /*（修改交易方式不需要存储订单号了）
            //存储交易订单号
            if (str.Split(';')[0]=="4") {
                PayDingDanIDNow = str.Split(';')[1];
                Game_PublicClassVar.Get_function_Rose.WritePayID(PayDingDanIDNow, PayValueNow);
            }
            */

        }
    }

    //支付订单回执(支付平台;订单号;价格;支付成功/失败;交易信息)
    public void OnPayDingDanReturn(string str)
    {
        //Debug.Log("DingDan");

        string[] StrList = str.Split(';');

        string pay_PingTai = StrList[0];    
        string pay_DingDan = StrList[1];
        string pay_JiaGe = StrList[2];
        string pay_Status = StrList[3];
        string pay_Des = StrList[4];
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        PayDingDanIDNow = pay_DingDan;                  //当前交易订单
        PayDingDanStatus = int.Parse(pay_Status);       //当前订单交易

        Pro_PayList proPayList = new Pro_PayList();
        proPayList.zhanghaoID = zhanghaoID;
        proPayList.pay_PingTai = pay_PingTai;
        proPayList.pay_DingDan = pay_DingDan;
        proPayList.pay_JiaGe = pay_JiaGe;
        proPayList.pay_Status = pay_Status;
        proPayList.pay_Des = pay_Des;

        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001050, proPayList);

    }



#if UNITY_IPHONE
#endif
    //支付回执（IOS失败回执）
    public void OnPayResultReturnFail(string str)
    {
        testStrPay = "2" + ";" + str;
        Debug.Log("调用了支付返回值");
        PayStr = "2" + ";" + str;
        //payStr = str;
        if (PayStr != "")
        {
            PayStatus = true;   //开启支付状态

            /*（修改交易方式不需要存储订单号了）
            //存储交易订单号
            if (str.Split(';')[0]=="4") {
                PayDingDanIDNow = str.Split(';')[1];
                Game_PublicClassVar.Get_function_Rose.WritePayID(PayDingDanIDNow, PayValueNow);
            }
            */
        }
    }


    //查询订单回执
    public void OnPayQueryReturn(string str)
    {
        //Debug.Log("调用了支付查询值");
        PayStrQueryStatus = str;
        //testStrPay = testStrPay + ";" + str + "\n";
        //payStr = str;
        if (str != "")
        {
            PayQueryStatus = true;   //开启查询状态
        }

        string dingdanID = str.Split(';')[0];
        string dingdanStatus = str.Split(';')[1];
        if (dingdanStatus == "SUCCESS")
        {
            string dingDanValue = Game_PublicClassVar.Get_function_Rose.DingDanReturnPayValue(dingdanID);
            //删除订单记录
            Game_PublicClassVar.Get_function_Rose.DeletePayID(dingdanID);
            if (dingDanValue != "0" && dingDanValue != "")
            {
                Game_PublicClassVar.Get_function_Rose.DingDanSendPayValue(dingDanValue);
            }
        }
    }

    //安装微信插件清空点击状态
    public void ClearnWeiXinPayStatus(string str)
    {
        Debug.Log("微信清空点击状态！，当前版本：" + str);
        //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>().buyStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>().ClearnPayValue();
    }


    public void testWeiTuoA()
    {
        Debug.Log("testWeiTuoBtestWeiTuoBtestWeiTuoA");
    }
    public void testWeiTuoB() {
        Debug.Log("testWeiTuoBtestWeiTuoBtestWeiTuoB");
    }

    public void test111() {

        //Game_PublicClassVar.Get_function_UI.AddEquipGem("5","2","1",2);
        Game_PublicClassVar.Get_function_UI.UnloadEquipGem("102","2", "1", 1);
    }

    /// <summary> 微信支付回调 </summary>
    public void WechatPayCallback(string retCode)
    {
        Debug.Log("微信支付结果来了：" + retCode);

        switch (retCode)
        {
            case "-2":
                Debug.Log("支付取消");
                //清理支付信息
                PayStr = "2" + ";" + retCode;
                PayStatus = true;
                break;
            case "-1":
                Debug.Log("支付失败");
                //清理支付信息
                PayStr = "2" + ";" + retCode;
                PayStatus = true;
                break;
            case "0":
                Debug.Log("支付成功");

                break;
        }
        //weChatPayCallback(state);
    }

    /// <summary>支付宝支付回调</summary>
    //这里是同步调用,由SDK反馈支付结果
    public void AliPayCallback(string result)
    {
        Debug.Log("支付宝支付结果来了：" + result);
        //aliPayCallBack(result);
        //告诉服务器已经支付 等待返回结果
        //再监听结果 进行发放奖励 实际上都是独立的
        if (result == "支付成功")
        {
            Debug.Log("支付宝支付成功");
        }
        else
        {
            Debug.Log("支付宝支付失败");
            //清理支付信息
            PayStr = "2" + ";" + result;
            PayStatus = true;
        }
    }
}
