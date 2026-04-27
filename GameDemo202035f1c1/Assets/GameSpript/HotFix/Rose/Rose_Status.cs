using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;

public class Rose_Status : MonoBehaviour
{

    //角色当前状态的脚本
    //里面存放角色当前的各种的状态，例如施法状态等等
    public GameObject Obj_RoseModel;                    //角色Model
    public UnityEngine.AI.NavMeshAgent ai_nav;
    public Vector3 Move_Target_Position;
    public ObscuredBool Move_Target_Status;
    public bool Move_Target_SaveStatus;                 //存储,当前是否是移动状态
    public ObscuredString RoseStatus;                   //角色当前状态   1：表带待机  2：表示跑动 3:攻击 4:暴击攻击 5:释放技能 6:吟唱状态 7：眩晕状态 8:死亡状态               //9:坐骑状态
    public bool ZuoQiStatus;                            //上下马状态
    private ObscuredFloat zuoQiAddSpeed;                //坐骑移动速度加成
    public bool ZuoQiZiDongStatus;                      //坐骑自动上下马
    public GameObject Obj_RoseShowModel;
    public GameObject Obj_ZuoQiShowModel;
    public GameObject Obj_ZuoQiShowPosition;
    public GameObject Obj_ZuoQiShowSet;
    //private bool roseRunStatus;                       //角色奔跑状态
    public Animator roseAnimator;                       //角色当前动画状态机
    public GameObject Obj_RoseMoveEffect;               //角色移动点击特效
    private ObscuredInt roseMoveEffectStatus;          	//角色移动点击特效状态
    private Vector3 lastRosePosition;                   //角色上一帧位置            当角色处于移动状态时,上一帧和当前帧位置一样，则移动失败
    private ObscuredFloat lastRosePositionTime;
    public GameObject Obj_RunEffect;                        //角色奔跑产生的烟雾
    public GameObject Obj_RunEffectPath;        	        //角色奔跑产生的烟雾
    public ObscuredBool RoseInvisibleStatus;                //角色隐身状态
    public ObscuredBool RoseSingStatus;                     //角色吟唱状态
    public ObscuredBool RoseSingStopStatus;                 //角色吟唱停止状态          此开关打开吟唱状态立即停止
    private ObscuredBool roseSingUIStatus;                  //角色吟唱UI状态
    public ObscuredFloat RoseSingTime;                      //角色吟唱时间
    private ObscuredFloat roseSingTimeSum;                  //角色吟唱时间累计值
    public ObscuredString RoseSingType;                     //角色吟唱类型            0：不执行    1：施法前置吟唱    2：施法吟唱   
    private ObscuredBool ifRoseSingWin;                     //角色吟唱是否成功
    public GameObject RoseSingSkillObj;             //吟唱的技能
    private GameObject roseSingUIObj;               //角色吟唱状态UI
    public GameObject[] RosePetObj;                 //角色宠物Obj
    public GameObject[] RosePetfollowPositionObj;   //角色宠物跟随的坐标点Obj
    public GameObject[] RosePetPositionObj;         //角色宠物跟随的坐标点Obj
    public ObscuredBool RoseMapMoveStatus;                  //角色小地图移动
    public ObscuredBool RoseMapMoveNowStatus;                  //角色小地图移动
    public Vector3 RoseMapMoveVec3;                 //角色小地图移动坐标
    public Transform RoseWeaponPosi;                //角色武器位置
    public GameObject RoseWeaponBaseModel;          //角色武器模型
    public GameObject Obj_RoseWeaponEffectTrail;    //角色武器路径特效
    private GameObject roseWeaponEffectTrail;
    public Transform RoseModelWeaponPosi;           //角色UI界面模型武器位置
    public GameObject RoseModelWeaponBaseModel;     //角色UI界面模型武器模型

    //施法状态
    public ObscuredBool SkillStatus;                        //是否在释放技能
    public ObscuredBool SkillAnimationStatus;               //是否在播放技能动作
    public ObscuredString RoseNowUseSkillID;                //角色当前使用的技能ID
    public ObscuredString RoseNowUseSkillAnimationName;     //角色当前使用技能的动作名称
                                                            //public float RoseNowUseSkillDelayTime;        //角色当前使用的技能延迟
    public ObscuredBool BagStatus;                          //背包是否打开
    public ObscuredBool Rose_TaskList_Status;               //角色任务
    public ObscuredBool RoseEquipStatus;                    //当前打开角色界面
    public ObscuredBool RoseIfHit;                          //角色是否受到攻击
                                                            //public string RoseHitText;					//角色受击飘的文字
                                                            //public string RoseHitTextlater;				//角色受击飘的最后文字
    public ObscuredBool RoseIfXiXue;                        //角色是否吸血
    public ObscuredBool RpseIfDodge;                        //角色是否闪避
    //public bool RoseIfHitEffect;                  //角色受到攻击特效
    public ObscuredBool RoseUpLv;                           //角色升级时打开
    public ObscuredBool RoseStorySpeakStatus;               //角色故事对话状态是否打开
    public ObscuredBool RoseDeathStatus;                    //角色死亡状态
    public ObscuredBool RoseDeathStatusOnce;                //角色死亡状态,执行一次
    private GameObject roseDeathObj;                //角色死亡UI
    public ObscuredBool RoseFightStatus;                    //角色战斗状态
    public ObscuredBool RoseFightFirstStatus;               //角色第一次进入战斗后触发
    public List<GameObject> RoseFightMonsterActList;        //角色战斗中怪物攻击角色的怪物列表
    private float FightExitZuoQiTime;                       //退出战斗后 3秒上坐骑
    private bool FightExitZuoQiStatus;
    public GameObject HitObject;                //飘字OBJ
    public GameObject HitEffectObj;             //特效OBJ
    public GameObject Eff_RoseUp;               //角色升级特效
    public GameObject Obj_ActTarget;            //攻击目标
    public GameObject Obj_SceneTanSuo;          //探索Obj

    public ObscuredInt rose_LastHp;                     //角色上一次血量
    private ObscuredInt rose_HitHp;                     //每次飘字显示的血量
    private Rose_Proprety roseProperty;         //角色属性
    private ObscuredBool roseActStatus;                 //角色普通攻击状态
    private ObscuredBool roseMoveToAct;                 //角色跑动攻击状态
    private ObscuredBool roseCriActStatus;              //角色暴击状态
    public ObscuredString RoseOcc;                      //角色职业
    public int RoseZuoQiBaoShiDu;            //坐骑饱食度

    private UI_DropName ai_dorpitem;                    //声明一个掉落脚本
    private ObscuredBool ifGold;                        //捡取物品是否为货币
    private ObscuredBool roseMoveDrop;                  //捡取物品距离不够需要移动
    public ObscuredBool roseMoveNpc;                    //移动至Npc开启对话
    public GameObject obj_NpcTarget;                    //当前选中的NPC
    public GameObject npcTaskUI;                       //当前打开的Npc对话框
    public ObscuredBool roseMoveGether;                 //获取采集类的道具
    public UI_GetherItem ui_GetherItem;		            //采集宝箱脚本
    public ObscuredBool RoseGetherItemStatus;           //角色采集道具状态
    public ObscuredFloat SkillAnimationTime;            //技能动作持续时间（期间停止角色播放其他动作,并禁止移动）
    private ObscuredFloat skillAnimationTimeSum;        //技能动作持续时间累计值
    private Vector3 rosePosiVec3;                       //自身每帧当前坐标点(因为做法师技能去自己坐标点有时候会取到下方,找不到问题)
    public ObscuredString skillIDBeiDong;               //被动技能ID
    public ObscuredBool updatTaskStatus;                //任务更新状态
    public int shiquTimeSum;                            //拾取时间累计
    public int shiquNullNum;                            //拾取空道具的次数

    public ObscuredBool XuanYunStatus;                  //眩晕状态
    public ObscuredFloat XuanYunTime;                   //眩晕时间
    public ObscuredFloat xuanYunTimeSum;                //眩晕时间累计

    public ObscuredString TaskRunStatus;                //追踪任务状态   0：默认(无任务追踪)  1：触发任务追踪   2：任务追踪状态中  
    public Vector3 TaskRunPositionVec3;                 //任务追踪移动的坐标点
    private ObscuredFloat taskRunAddSpeed;              //任务加成移动速度
    private ObscuredFloat taskRunAddTimeSum;

    private GameObject Obj_UIAIPropertyShow;

    private ObscuredFloat actDistance;                          //当前攻击距离

    //翻滚状态
    public ObscuredBool FanGunStatus;                   //翻滚状态

    //摇杆状态
    public bool YaoGan_Status;
    public Vector3 YaoGanPositionVec3;                  //任务追踪移动的坐标点
    public float YaoGanStopMoveTime;
    //public float YaoGanJiaoDu;                        //摇杆角度

    //自动战斗
    public ObscuredBool AutomaticStatus;            //自动攻击开启此状态
    private ObscuredBool AutomaticStatusOnce;       //首次执行
    private ObscuredInt AutomaticStatusSum;
    public float AutomaticActTimeSum;               //自动攻击触发间隔时间
    public ObscuredBool ifAutomatic;
    public ObscuredFloat NextAutomaticMonsterDis;
    public GameObject NextAutomaticMonsterObj;
    public Dictionary<GameObject, ActTargetList> AutomaticMonsterObjList = new Dictionary<GameObject, ActTargetList>();

    //挂机状态
    public ObscuredBool AutomaticGuaJiStatus;           //挂机状态
    public Vector3 AutomaticGuaJiStatusInitPosi;        //初始点挂机坐标
    public ObscuredInt AutomaticGuaJiNum;               //挂机数量

    //原地攻击状态
    public bool RoseActActionStatus;
    public float RoseActActionTime;
    public GameObject SkillRangeEffect;

    //自动拾取
    //public bool AutomaticDropStatus;            //自动攻击开启此状态
    //private bool AutomaticDropStatusOnce;       //首次执行
    //private int AutomaticDropStatusSum;
    public ObscuredBool IfAutomaticTake;        //自动拾取开启
    public ObscuredFloat NextAutomaticDropDis;
    public GameObject NextAutomaticDropObj;
    private ObscuredInt fanjiStatusNum;

    //被动技能相关参数
    private string[] passiveSkillID;                 //被动技能
    private string[] passiveSkillType;               //技能触发类型
    private string[] passiveSkillPro;                //技能触发类型参数
    private string[] passiveSkillTriggerOnce;        //技能是否只触发一次参数
    private string[] passiveSkillTriggerTimeOnce;    //技能是否只触发一次参数
    private string[] passiveSkillTriggerTime;        //技能触发时间类型
    private string[] ifSkillTrigger;                 //技能是否触发,当次字段的数组为1是,对应的技能将不能再次释放
    private ObscuredFloat triggerTimeOnceSum;
    public ObscuredBool UpdataPassiveSkillStatus;

    public List<SkillIAddValue> Rose_SkillIAddValueList = new List<SkillIAddValue>();    //技能附加结构体,用于存储技能的附加效果,例如某个技能持续时间延长

    //技能指示器
    public GameObject Obj_RoseSkillZhiShi;          //技能指示器

    //定义一个技能附加值结构体,用于存储技能的附加效果
    public struct SkillIAddValue
    {
        public ObscuredString SkillID;
        public ObscuredString AddType;
        public ObscuredString AddValue;
    }

    //服务器相关
    public bool MovePositionSendStatus;             //自身坐标发送服务器状态
    public string MovePositionSendType;             // 1.表示精准发送   2.表示模糊发送
    public string MovePositionSendServerType;       // 1.全服移动       2.单服务器移动
    public Vector3 MovePositionVec3_YaoGan;                 //摇杆的移动
    public ObscuredString EnterMapServerName;               //进入服务器名称
    private float moveSendTime;

    private ObscuredFloat mLastAttackTime;
    private ObscuredFloat mLastCriAckTime;
    private ObscuredFloat mMaxIntervalTime;
    private ObscuredFloat mAnimationSpeed;


    // Use this for initialization
    void Start()
    {

        mLastAttackTime = 0f;
        mLastCriAckTime = 0f;
        mMaxIntervalTime = 0.2f;

        roseProperty = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        ai_nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        roseAnimator = Obj_RoseModel.GetComponent<Animator>();
        mAnimationSpeed = roseAnimator.speed;

        //设置主角质量最大,用于穿怪
        ai_nav.avoidancePriority = 0;

        RoseSingTime = 10.0f; //测试  吟唱时间
        skillIDBeiDong = "";
        //Game_PublicClassVar.Get_function_Skill.InitializePassiveSkill();        //初始化被动技能
        UpdataPassiveSkillStatus = true;

        RoseOcc = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //OpenEmenyAct();     //更新打开建筑攻击

        //Game_PublicClassVar.Get_function_Rose.RosePetCreate(false);
        /*
        string ifPetChuZhan = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (ifPetChuZhan == "1") {
            Debug.Log("创建宠物");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_3", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

            //临时,后期要调整
            Game_PublicClassVar.Get_function_Rose.RosePetCreate(1,false);
        }
        */

        //召唤默认出战的宠物
        for (int i = 1; i <= RosePetObj.Length; i++)
        {
            string petStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", i.ToString(), "RosePet");
            if (petStatus == "1")
            {
                Game_PublicClassVar.Get_function_Rose.RosePetCreate(i, false);
            }
        }

        //初始化当前武器模型
        //更换武器样子
        string weaponItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", "1", "RoseEquip");
        //如果格子为空则显示原始武器
        if (weaponItemID == "0" || weaponItemID == "")
        {
            Game_PublicClassVar.Get_function_Rose.RoseWeaponModel("");
        }
        else
        {
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", weaponItemID, "Item_Template");
            if (itemType == "3")
            {
                string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", weaponItemID, "Item_Template");
                if (itemSubType == "1")
                {
                    string ItemMondel = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemMondel", "ID", weaponItemID, "Item_Template");
                    //临时
                    if (ItemMondel == "yaoshui")
                    {
                        ItemMondel = "";
                    }
                    Game_PublicClassVar.Get_function_Rose.RoseWeaponModel(ItemMondel);
                }
            }
        }

        //添加保存的buff
        if (Game_PublicClassVar.Get_wwwSet.RoseBuffStr != "")
        {

            //刷新属性
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseProperty = true;

            string[] saveBuffList = Game_PublicClassVar.Get_wwwSet.RoseBuffStr.Split(';');
            Game_PublicClassVar.Get_wwwSet.RoseBuffStr = "";

            for (int i = 0; i < saveBuffList.Length; i++)
            {
                if (saveBuffList[i] != "")
                {
                    string addSkillBuffID = saveBuffList[i].Split(',')[0];
                    string addSkillBuffTime = saveBuffList[i].Split(',')[1];
                    Game_PublicClassVar.Get_function_Skill.BuffTypeReturnScriptName(this.gameObject, addSkillBuffID, null, float.Parse(addSkillBuffTime));
                }
            }
        }


        //显示坐骑
        Game_PublicClassVar.Get_function_Pasture.ZuoQi_RoseShow();

        //更新攻击距离
        GetActDis();
    }

    // Update is called once per frame
    void Update()
    {

        rosePosiVec3 = this.gameObject.transform.position;

        RoseStatus = "1";           //默认为待机

        /*
        if (RoseActActionStatus)
        {
            RoseActActionTime = RoseActActionTime - Time.deltaTime;
            if (RoseActActionTime <= 0) {
                RoseActActionStatus = false;
            }
 
        }
        */

        //实例化奔跑特效
        if (Obj_RunEffect == null) {
            if (Obj_RunEffectPath != null)
            {
                GameObject nowObj = (GameObject)Instantiate(Obj_RunEffectPath);
                if (nowObj.GetComponent<RunEff>() != null)
                {
                    Obj_RunEffect = nowObj.GetComponent<RunEff>().runEffectObj;
                }
            }
        }

        if (Move_Target_SaveStatus)
        {
            Move_Target_SaveStatus = false;
            //Debug.Log("Move_Target_SaveStatus");
            Move_Target_Status = true;
        }


        //查询角色是否在坐骑上
        if (ZuoQiStatus)
        {
            zuoQiAddSpeed = 1.5f;

            if (RoseZuoQiBaoShiDu <= 0)
            {

                string baoshiduStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoShiDu", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                if (baoshiduStr == "" || baoshiduStr == null)
                {
                    baoshiduStr = "0";
                }
                RoseZuoQiBaoShiDu = int.Parse(baoshiduStr);
            }

            float zuoqiPro = (float)(RoseZuoQiBaoShiDu) / 60;
            if (zuoqiPro > 1)
            {
                zuoqiPro = 1;
            }

            if (zuoqiPro < 1)
            {
                zuoqiPro = 0;
            }

            zuoQiAddSpeed = zuoQiAddSpeed * zuoqiPro;

        }
        else
        {
            zuoQiAddSpeed = 0;
        }

        ai_nav.speed = roseProperty.Rose_MoveSpeed + zuoQiAddSpeed + taskRunAddSpeed;     //设置角色速度

        //角色死亡不能执行任何操作
        if (RoseDeathStatus)
        {
            //播放动画
            RoseStatus = "8";
            ai_nav.speed = 0;
            roseAnimatorOpen(RoseStatus);
            //执行一次
            if (!RoseDeathStatusOnce)
            {
                RoseDeathStatusOnce = true;
                Move_Target_Status = false;
                TaskRunStatus = "0";
                if (roseDeathObj == null)
                {
                    roseDeathObj = (GameObject)Instantiate(Resources.Load("UGUI/UISet/Other/UI_RoseDeath", typeof(GameObject)));
                    roseDeathObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameGirdHint_Front.transform);
                    roseDeathObj.transform.localPosition = new Vector3(0, 0, 0);
                    roseDeathObj.transform.localScale = new Vector3(1, 1, 1);
                }

                //清空自身所有的毒BUFF
                Buff_2[] buffList = this.GetComponents<Buff_2>();
                if (buffList != null)
                {
                    for (int i = 0; i <= buffList.Length - 1; i++)
                    {
                        Destroy(buffList[i]);
                    }
                }

                //清空任务栏
                GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcTaskSet;
                for (int i = 0; i < parentObj.transform.childCount; i++)
                {
                    GameObject go = parentObj.transform.GetChild(i).gameObject;
                    MonoBehaviour.Destroy(go);
                }

                //设置墓碑
                GameObject mubeiObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_RoseDeathMuBei);
                mubeiObj.transform.localScale = new Vector3(1, 1, 1);
                mubeiObj.transform.position = this.gameObject.transform.position;


                string RoseDeathMuBeiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMuBeiStr", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                //设置墓碑
                string addStr = Application.loadedLevelName + ";" + this.gameObject.transform.position.x + "," + this.gameObject.transform.position.y + "," + this.gameObject.transform.position.z;
                if (RoseDeathMuBeiStr != "")
                {
                    RoseDeathMuBeiStr = RoseDeathMuBeiStr + "|" + addStr;
                }
                else
                {
                    RoseDeathMuBeiStr = addStr;
                }

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMuBeiStr", RoseDeathMuBeiStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            }

            return;
        }

        //眩晕状态,不执行任和操作
        if (XuanYunStatus)
        {
            xuanYunTimeSum = xuanYunTimeSum + Time.deltaTime;
            Move_Target_Status = false; //禁止移动
            if (xuanYunTimeSum >= XuanYunTime)
            {
                XuanYunStatus = false;
                xuanYunTimeSum = 0.0f;
            }

            //播放动画
            RoseStatus = "1";
            roseAnimatorOpen(RoseStatus);

            return;
        }

        //鼠标按下移动
        if (Input.GetMouseButton(0))
        {
            //Debug.Log("鼠标移动!!!YaoGan_Status = " + YaoGan_Status);
            //检测当前道具Icon、技能Icon是否在移动
            if (!Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus && SkillStatus != true && !Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus)
            {
                //检测是否触碰到UI上,安卓或IOS可能要换一下

#if UNITY_EDITOR
                if (!EventSystem.current.IsPointerOverGameObject())
#else
                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif

                /*
                #if UNITY_IPHONE
                                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                #else
                                    if (!EventSystem.current.IsPointerOverGameObject())
                #endif
                */

                {

                    bool dianjiUI = false;
#if UNITY_EDITOR
                    if (EventSystem.current.IsPointerOverGameObject())
#else
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
                    {
                        dianjiUI = true;
                    }
                    else
                    {
                        if (RoseMapMoveNowStatus)
                        {
                            RoseMapMoveNowStatus = false;
                        }
                    }

                    //摇杆移动的时候无法触发点击事件
                    if (!dianjiUI && YaoGan_Status == false)
                    {
                        //从摄像机处向点击的目标处发送一条射线
                        Ray Move_Target_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        //声明一个光线触碰器
                        RaycastHit Move_Target_Hit;
                        LayerMask mask = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 12) | (1 << 13) | (1 << 20) | (1 << 22) | (1 << 25) | (1 << 18);

                        //检测射线是否碰触到对象
                        //标记OUT变量在传入参数进去后可能发生改变，和ref类似，但是ref需要给他一个初始值
                        //第一个参数射线变量  第二个参数光线触碰器的反馈变量
                        if (Physics.Raycast(Move_Target_Ray, out Move_Target_Hit, 100, mask.value))
                        {
                            //检测当前是否打开Npc对话框,打开的话点击其他地方对话框会消失
                            if (npcTaskUI != null)
                            {
                                float dis = Vector3.Distance(Move_Target_Hit.point, npcTaskUI.GetComponent<UI_NpcTask>().Obj_Npc.transform.position);
                                ObscuredFloat disValue = 2;
                                if (dis > disValue)
                                {
                                    //Debug.Log("跑动销毁啦啦啦啦 dis = " + dis);
                                    Destroy(npcTaskUI);
                                }
                            }

                            //清空播放选中Npc特效
                            if (obj_NpcTarget != null)
                            {
                                obj_NpcTarget.GetComponent<AI_NPC>().IfSeclectNpc = false;
                                obj_NpcTarget = null;       //清空选中的NPC
                            }

                            //关闭自动挂机
                            if (AutomaticGuaJiStatus)
                            {
                                AutomaticGuaJiStatus = false;

                                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_255");
                                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("已关闭挂机状态");
                            }

                            //获取碰撞地面
                            if (Move_Target_Hit.collider.gameObject.layer == 10)
                            {
                                if (Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus == false)
                                {
                                    if (YaoGanStopMoveTime <= 0)
                                    {
                                        //将碰触的三维坐标进行赋值
                                        Move_Target_Position = Move_Target_Hit.point;
                                        //Debug.Log("碰撞:1");

                                        //自动移动开关打开
                                        Move_Target_Status = true;
                                        roseMoveDrop = false;
                                        roseActStatus = false;
                                        roseMoveNpc = false;
                                        roseMoveGether = false;

                                        //转身
                                        Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
                                        this.transform.LookAt(v31);

                                        //创建移动特效(等于0表示只执行一次)
                                        if (roseMoveEffectStatus == 0)
                                        {
                                            GameObject moveEffect = (GameObject)Instantiate(Obj_RoseMoveEffect);
                                            moveEffect.transform.localScale = new Vector3(1, 1, 1);
                                            moveEffect.transform.position = new Vector3(Move_Target_Position.x, Move_Target_Position.y + 0.1f, Move_Target_Position.z);
                                            roseMoveEffectStatus = roseMoveEffectStatus + 1;
                                        }
                                    }
                                }
                            }

                            //获取选中怪物目标,触发攻击
                            if (Move_Target_Hit.collider.gameObject.layer == 12)
                            {
                                //选定目标
                                Obj_ActTarget = Move_Target_Hit.collider.gameObject;
                                Move_Target_Position = Obj_ActTarget.transform.position;

                                //对选定的目标显示选中特效
                                Obj_ActTarget.GetComponent<AI_1>().AI_Selected_Status = true;
                                Move_Target_Status = true;
                                roseMoveToAct = true;
                                roseMoveDrop = false;
                                roseMoveNpc = false;
                                roseMoveGether = false;

                                //开启更新怪物选中状态
                                Game_PublicClassVar.Get_game_PositionVar.UpdataSelectedStatus = true;

                                //获取目标是否是BOSS
                                string ifBoss = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBoss", "ID", Obj_ActTarget.GetComponent<AI_1>().AI_ID.ToString(), "Monster_Template");
                                if (ifBoss == "1")
                                {
                                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_BossHp.gameObject.GetComponent<UI_BossHp>().UpdataMonster = true;
                                }
                                else
                                {
                                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_BossHp.gameObject.GetComponent<UI_BossHp>().CloseStatus = true;
                                }

                                //转身
                                Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
                                this.transform.LookAt(v31);
                            }

                            //拾取道具
                            if (Move_Target_Hit.collider.gameObject.layer == 8)
                            {
                                //取得鼠标点击的位置为释放技能的范围中心点
                                Move_Target_Position = Move_Target_Hit.point;
                                ai_dorpitem = Move_Target_Hit.collider.gameObject.GetComponent<UI_DropName>();
                                //开启移动状态，移动到目标处捡取物品
                                roseMoveDrop = true;
                                roseMoveNpc = false;
                                Move_Target_Status = true;
                                roseMoveGether = false;

                                //转身
                                Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
                                this.transform.LookAt(v31);
                            }

                            //拾取采集类道具
                            if (Move_Target_Hit.collider.gameObject.layer == 13)
                            {
                                //取得鼠标点击的位置为释放技能的范围中心点
                                Move_Target_Position = Move_Target_Hit.point;
                                ui_GetherItem = Move_Target_Hit.collider.gameObject.GetComponent<UI_GetherItem>();
                                //开启移动状态，移动到目标处捡取物品
                                roseMoveGether = true;
                                roseMoveDrop = false;
                                roseMoveNpc = false;
                                Move_Target_Status = true;

                                //转身
                                Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
                                this.transform.LookAt(v31);
                            }

                            //点击NPC
                            //if (YaoGan_Status == false) {

                            //点击NPC
                            //if (Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus == false)
                            //{

                            if (Move_Target_Hit.collider.gameObject.layer == 9)
                            {

                                //Debug.Log("Input.mousePosition = " + Input.mousePosition);
                                //左下角区域不选中NPC
                                if (Input.mousePosition.x >= 450 || Input.mousePosition.y >= 350)
                                {
                                    //取得鼠标点击的位置为释放技能的范围中心点
                                    Move_Target_Position = Move_Target_Hit.point;
                                    ai_dorpitem = Move_Target_Hit.collider.gameObject.GetComponent<UI_DropName>();
                                    //开启移动状态，移动到目标处捡取物品
                                    roseMoveDrop = false;
                                    roseMoveNpc = true;
                                    Move_Target_Status = true;
                                    roseMoveGether = false;

                                    //设置点击的NPC
                                    obj_NpcTarget = Move_Target_Hit.collider.gameObject;
                                    //播放选中Npc特效
                                    if (obj_NpcTarget != null)
                                    {
                                        obj_NpcTarget.GetComponent<AI_NPC>().IfSeclectNpc = true;
                                    }

                                    //转身
                                    Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
                                    this.transform.LookAt(v31);
                                    //Debug.Log("点击NPC!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + YaoGan_Status);
                                }

                            }



                            //}

                            //}

                            //点击玩家
                            //判定点击是否和摇杆区域重叠
                            if (Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus)
                            {
                                Vector2 yaoganVec2 = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_YaoGan.GetComponent<UI_GameYaoGan>().MoveObj.transform.position;
                                float chaVec2Value = Vector2.Distance(Input.mousePosition, yaoganVec2);
                                if (chaVec2Value >= 200)
                                {
                                    if (Move_Target_Hit.collider.gameObject.layer == 20)
                                    {
                                        //取得鼠标点击的位置为释放技能的范围中心点
                                        Move_Target_Position = Move_Target_Hit.point;
                                        //转身
                                        Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
                                        this.transform.LookAt(v31);
                                        //Debug.Log("点击玩家!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                        Move_Target_Hit.collider.gameObject.GetComponent<Player_Status>().ClickPlayer();
                                    }
                                }
                            }


                            //点击牧场动物
                            if (Move_Target_Hit.collider.gameObject.layer == 22)
                            {
                                //取得鼠标点击的位置为释放技能的范围中心点
                                Move_Target_Position = Move_Target_Hit.point;
                                //转身
                                Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
                                this.transform.LookAt(v31);
                                //Debug.Log("点击牧场动物!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                Move_Target_Hit.collider.gameObject.GetComponent<PastureAI>().ClickPlayer();
                            }


                            //点击牧场动物
                            //Debug.Log("Move_Target_Hit.collider.gameObject.layer = " + Move_Target_Hit.collider.gameObject.layer);
                            if (Move_Target_Hit.collider.gameObject.layer == 25)
                            {
                                //取得鼠标点击的位置为释放技能的范围中心点
                                Move_Target_Position = Move_Target_Hit.point;
                                //转身
                                Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
                                this.transform.LookAt(v31);
                                //Debug.Log("点击牧场动物!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                Move_Target_Hit.collider.gameObject.GetComponent<PastureKuangSet>().Click();
                            }



                            //点击牧场动物
                            if (Move_Target_Hit.collider.gameObject.layer == 18)
                            {
                                /*
                                //取得鼠标点击的位置为释放技能的范围中心点
                                Move_Target_Position = Move_Target_Hit.point;
                                //转身
                                Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
                                this.transform.LookAt(v31);
                                Debug.Log("点击牧场动物!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                Move_Target_Hit.collider.gameObject.GetComponent<PastureAI>().ClickPlayer();
                                */
                                if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().YaoGan_Status == false)
                                {
                                    if (Move_Target_Hit.collider.gameObject.GetComponent<AIPet>().PetType == "1" && RoseFightStatus == false)
                                    {

                                        if (Obj_UIAIPropertyShow != null)
                                        {
                                            Destroy(Obj_UIAIPropertyShow);
                                        }

                                        Obj_UIAIPropertyShow = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_AIPropertyShow);
                                        Obj_UIAIPropertyShow.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                                        Obj_UIAIPropertyShow.transform.localScale = new Vector3(1, 1, 1);
                                        Obj_UIAIPropertyShow.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                                        Obj_UIAIPropertyShow.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                                        Obj_UIAIPropertyShow.GetComponent<UI_AIPropertyShow>().Obj_Show = Move_Target_Hit.collider.gameObject;
                                        Obj_UIAIPropertyShow.GetComponent<UI_AIPropertyShow>().Init();
                                    }
                                }
                            }

                            //判定是否在任务追踪
                            if (TaskRunStatus == "2")
                            {
                                TaskRunStatus = "0";
                                taskRunAddSpeed = 0;
                                //TaskRunPositionVec3 = Vector3
                                //Debug.Log("取消追踪状态");
                            }
                        }
                    }
                }
            }



        }


        //获取碰撞地面
        if (RoseMapMoveStatus == true)
        {

            //将碰触的三维坐标进行赋值
            Move_Target_Position = RoseMapMoveVec3;


            //自动移动开关打开
            Move_Target_Status = true;
            roseMoveDrop = false;
            roseActStatus = false;
            roseMoveNpc = false;
            roseMoveGether = false;
            RoseMapMoveStatus = false;

            RoseStatus = "2";       //设置跑动

            //转身
            Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
            this.transform.LookAt(v31);

            //创建移动特效(等于0表示只执行一次)
            if (roseMoveEffectStatus == 0)
            {
                GameObject moveEffect = (GameObject)Instantiate(Obj_RoseMoveEffect);
                moveEffect.transform.localScale = new Vector3(1, 1, 1);
                moveEffect.transform.position = new Vector3(Move_Target_Position.x, Move_Target_Position.y + 0.1f, Move_Target_Position.z);
                roseMoveEffectStatus = roseMoveEffectStatus + 1;
            }

            //检测当前是否打开Npc对话框,打开的话点击其他地方对话框会消失
            if (npcTaskUI != null)
            {
                Destroy(npcTaskUI);
            }
        }

        //鼠标松开,重置播放点击特效
        if (Input.GetMouseButtonDown(0))
        {
            roseMoveEffectStatus = 0;
        }

        if (AutomaticStatus)
        {
            //首次执行
            if (!AutomaticStatusOnce)
            {
                ifAutomatic = true;
                AutomaticStatusOnce = true;
            }

            //目标为空时立即寻扎
            if (Obj_ActTarget == null)
            {
                if (AutomaticStatusSum > 0)
                {
                    ifAutomatic = true;
                }
                AutomaticStatusSum = AutomaticStatusSum + 1;        //停留1帧 让其他怪物读取到与主角的距离
            }
            else
            {
                AutomaticStatusSum = 0; // 清空
            }
        }
        else
        {
            //清空首次执行
            AutomaticStatusOnce = false;
        }

        //自动战斗
        if (ifAutomatic)
        {

            ifAutomatic = false;

            if (!FanGunStatus)
            {
                //选定目标
                if (NextAutomaticMonsterObj != null)
                {

                    //获取当前职业,魔法师职业不自动切换最近目标
                    string roseOcc = RoseOcc;
                    //float autoActDis = 5.0f;     // 如果需要供给最近目标,就在此处设置1.0即可

                    switch (roseOcc)
                    {
                        //战士攻击最近目标
                        case "1":

                            if (Obj_ActTarget == null)
                            {
                                Obj_ActTarget = NextAutomaticMonsterObj;
                            }
                            else
                            {
                                //if (Vector3.Distance(Obj_ActTarget.transform.position, NextAutomaticMonsterObj.transform.position) >= autoActDis)
                                if (Vector3.Distance(Obj_ActTarget.transform.position, this.gameObject.transform.position) >= actDistance)
                                {
                                    Obj_ActTarget = NextAutomaticMonsterObj;
                                }
                            }

                            break;
                        //魔法师不执行任何操作
                        case "2":

                            if (Obj_ActTarget == null)
                            {
                                Obj_ActTarget = NextAutomaticMonsterObj;
                            }
                            else
                            {
                                //if (Vector3.Distance(Obj_ActTarget.transform.position, NextAutomaticMonsterObj.transform.position) >= actDistance)
                                if (Vector3.Distance(Obj_ActTarget.transform.position, this.gameObject.transform.position) >= actDistance)
                                {
                                    Obj_ActTarget = NextAutomaticMonsterObj;
                                }
                            }

                            break;

                        //猎人不执行任何操作
                        case "3":

                            if (Obj_ActTarget == null)
                            {
                                Obj_ActTarget = NextAutomaticMonsterObj;
                            }
                            else
                            {
                                //if (Vector3.Distance(Obj_ActTarget.transform.position, NextAutomaticMonsterObj.transform.position) >= actDistance)
                                if (Vector3.Distance(Obj_ActTarget.transform.position, this.gameObject.transform.position) >= actDistance)
                                {
                                    Obj_ActTarget = NextAutomaticMonsterObj;
                                }
                            }

                            break;
                    }
                    //Obj_ActTarget = NextAutomaticMonsterObj;
                    Move_Target_Position = Obj_ActTarget.transform.position;
                    //对选定的目标显示选中特效
                    Obj_ActTarget.GetComponent<AI_1>().AI_Selected_Status = true;
                    Move_Target_Status = true;
                    roseMoveToAct = true;
                    roseMoveDrop = false;
                    roseMoveNpc = false;
                    roseMoveGether = false;

                    //开启更新怪物选中状态
                    Game_PublicClassVar.Get_game_PositionVar.UpdataSelectedStatus = true;

                    //获取目标是否是BOSS
                    string ifBoss = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBoss", "ID", Obj_ActTarget.GetComponent<AI_1>().AI_ID.ToString(), "Monster_Template");
                    if (ifBoss == "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_BossHp.gameObject.GetComponent<UI_BossHp>().UpdataMonster = true;
                    }
                    else
                    {
                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_BossHp.gameObject.GetComponent<UI_BossHp>().CloseStatus = true;
                    }

                    //转身
                    Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
                    this.transform.LookAt(v31);

                    //设置不触发摇杆的时间
                    if (YaoGan_Status)
                    {
                        switch (roseOcc)
                        {
                            case "1":
                                YaoGanStopMoveTime = 1f;
                                break;
                            case "2":
                                YaoGanStopMoveTime = 0.3f;
                                break;
                            case "3":
                                YaoGanStopMoveTime = 0.3f;
                                break;
                        }
                    }

                }
                else
                {
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前范围内没有可攻击的怪物！");
                }
            }
        }

        //拾取道具
        /*
        if (AutomaticDropStatus)
        {
            //首次执行
            if (!AutomaticDropStatusOnce)
            {
                IfAutomaticTake = true;
                AutomaticDropStatusOnce = true;
            }

            //目标为空时立即寻扎
            if (NextAutomaticDropObj == null)
            {
                if (AutomaticDropStatusSum > 0)
                {
                    IfAutomaticTake = true;
                }
                AutomaticDropStatusSum = AutomaticDropStatusSum + 1;        //停留1帧 让其他怪物读取到与主角的距离
            }
            else
            {
                AutomaticDropStatusSum = 0; // 清空
            }
        }
        else
        {
            //清空首次执行
            AutomaticDropStatusOnce = false;
        }
        */

        //拾取道具
        if (IfAutomaticTake)
        {
            shiquTimeSum = shiquTimeSum + 1;
            ObscuredFloat shiquDis = 15;
            if (shiquTimeSum <= shiquDis)
            {
                //取得鼠标点击的位置为释放技能的范围中心点
                if (NextAutomaticDropObj != null)
                {

                    Move_Target_Position = NextAutomaticDropObj.transform.position;
                    ai_dorpitem = NextAutomaticDropObj.GetComponent<UI_DropName>();
                    //开启移动状态，移动到目标处捡取物品
                    roseMoveDrop = true;
                    roseMoveNpc = false;
                    Move_Target_Status = true;
                    roseMoveGether = false;

                    //转身
                    Vector3 v31 = new Vector3(Move_Target_Position.x, this.transform.position.y, Move_Target_Position.z);
                    this.transform.LookAt(v31);

                    shiquNullNum = 0;
                }
                else
                {
                    shiquNullNum = shiquNullNum + 1;
                    if (shiquNullNum >= 2)
                    {
                        IfAutomaticTake = false;
                        shiquTimeSum = 0;
                        shiquNullNum = 0;
                    }
                }
            }
            else
            {
                IfAutomaticTake = false;
                shiquTimeSum = 0;
                shiquNullNum = 0;
            }
        }


        //任务寻路状态开启
        if (TaskRunStatus == "1")
        {
            TaskRunStatus = "2";
            //自动移动开关打开
            Move_Target_Status = true;
            roseMoveDrop = false;
            roseActStatus = false;
            roseMoveNpc = false;
            roseMoveGether = false;

            //设置移动坐标
            Move_Target_Position = TaskRunPositionVec3;
            Vector3 lookAtVec3 = new Vector3(TaskRunPositionVec3.x, this.transform.position.y, TaskRunPositionVec3.z);

            //转身
            this.transform.LookAt(lookAtVec3);

            //给自己添加加速BUFF
            //Game_PublicClassVar.Get_function_Skill.SkillBuff("90010004", this.gameObject);

            //创建移动特效(等于0表示只执行一次)
            if (roseMoveEffectStatus == 0)
            {
                GameObject moveEffect = (GameObject)Instantiate(Obj_RoseMoveEffect);
                moveEffect.transform.localScale = new Vector3(1, 1, 1);
                moveEffect.transform.position = new Vector3(Move_Target_Position.x, Move_Target_Position.y + 0.1f, Move_Target_Position.z);
                roseMoveEffectStatus = roseMoveEffectStatus + 1;
            }
        }

        //每1秒移动速度增加0.5 ,上限速度为1.5
        if (TaskRunStatus == "2")
        {
            if (taskRunAddSpeed < 1.5f)
            {
                taskRunAddTimeSum = taskRunAddTimeSum + Time.deltaTime;
                if (taskRunAddTimeSum >= 1.0f)
                {
                    taskRunAddSpeed = taskRunAddSpeed + 0.5f;
                    taskRunAddTimeSum = 0;
                }
            }

            RoseStatus = "2";           //默认跑动

            //判定当前距离目标有多少距离
            float dis = Vector3.Distance(this.gameObject.transform.position, Move_Target_Position);
            if (dis <= 20.0f)
            {
                taskRunAddSpeed = 1;
                //移动至目标区域调整移动状态
                if (dis <= 2.0f)
                {
                    TaskRunStatus = "0";
                    //检测附近有无交任务的Npc
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().AutomaticNpc(true);
                    RoseMapMoveNowStatus = false;
                }
            }
        }

        //Debug.Log("YaoGanStopMoveTime = " + YaoGanStopMoveTime);

        //摇杆移动
        if (YaoGan_Status)
        {
            if (!FanGunStatus)
            {
                //Debug.Log("YaoGanStopMoveTime = " + YaoGanStopMoveTime);
                if (YaoGanStopMoveTime <= 0)
                {
                    //自动移动开关打开
                    Move_Target_Status = true;
                    //roseMoveDrop = false;
                    roseActStatus = false;
                    roseMoveNpc = false;
                    roseMoveGether = false;

                    //获取目标点的角度
                    /*
                    if (Mathf.Abs(this.transform.eulerAngles.y - YaoGanJiaoDu) > 90)
                    {
                        Debug.Log("移动有偏差！！！" + "this.transform.localRotation.y = " + this.transform.eulerAngles + ",YaoGanJiaoDu = " + YaoGanJiaoDu);
                    }
                    */

                    //设置移动坐标
                    Move_Target_Position = YaoGanPositionVec3;
                    Vector3 lookAtVec3 = new Vector3(YaoGanPositionVec3.x, this.transform.position.y, YaoGanPositionVec3.z);

                    //转身
                    this.transform.LookAt(lookAtVec3);

                    //关闭自动挂机
                    if (AutomaticGuaJiStatus)
                    {
                        AutomaticGuaJiStatus = false;
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_255");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("已关闭挂机状态");
                    }

                    //使用摇杆将 关闭自动拾取
                    if (shiquTimeSum >= 3)
                    {
                        IfAutomaticTake = false;
                    }

                }
                /*
                else
                {
                    YaoGanStopMoveTime = YaoGanStopMoveTime - Time.deltaTime;
                    if (YaoGanStopMoveTime <= 0)
                    {
                        YaoGanStopMoveTime = 0;
                    }
                }
                */
            }
        }

        //摇杆停止时间
        if (YaoGanStopMoveTime > 0)
        {
            YaoGanStopMoveTime = YaoGanStopMoveTime - Time.deltaTime;
            if (YaoGanStopMoveTime <= 0)
            {
                YaoGanStopMoveTime = 0;
            }
        }
        /*
        else {
            YaoGanJiaoDu = 0;
        }
        */

        //开启拾取
        if (roseMoveDrop)
        {
            //判定当前距离目标有多少距离
            float dis = Vector3.Distance(this.gameObject.transform.position, Move_Target_Position);

            //道具跳的太高,这里判定
            if (dis <= 6f)
            {
                //道具高度超过角色1米距离自动进行拾取
                float disHight = System.Math.Abs(Move_Target_Position.y - this.gameObject.transform.position.y);
                if (disHight >= 0.6f)
                {
                    //强制拾取对应道具到背包内
                    dis = 0;
                }
            }

            //正常拾取道具
            if (dis <= 2.5f)
            {
                //拾取对应道具到背包内
                roseMoveDrop = false;
                Move_Target_Status = false;
                ai_nav.SetDestination(this.gameObject.transform.position);  //拾取到物体，停止移动
                if (ai_dorpitem != null)
                {
                    bool sendRewardStatus = Game_PublicClassVar.Get_function_Rose.SendRewardToBag(ai_dorpitem.DropItemID, ai_dorpitem.DropItemNum, "0", ai_dorpitem.HideDropPro, "0", true, "29");   //拾趣道具极品
                    if (sendRewardStatus)
                    {
                        ai_dorpitem.IfRoseTake = true;      //注销拾取的道具
                    }
                }
            }
        }

        //开启拾取采集类道具
        if (roseMoveGether)
        {
            //判定当前距离目标有多少距离
            float dis = Vector3.Distance(this.gameObject.transform.position, Move_Target_Position);
            if (dis <= 1.5f)
            {
                //拾取对应道具到背包内
                roseMoveGether = false;
                Move_Target_Status = false;
                ai_nav.SetDestination(this.gameObject.transform.position);  //拾取到物体，停止移动
                if (ui_GetherItem != null)
                {
                    if (!ui_GetherItem.ScenceItemReviveStatus)
                    {
                        RoseGetherItemStatus = true;
                    }
                    ui_GetherItem.IfRoseTake = true;      //注销拾取的道具
                }
            }
        }


        //移动至Npc处
        if (roseMoveNpc)
        {
            //判定当前距离目标有多少距离
            float dis = Vector3.Distance(this.gameObject.transform.position, Move_Target_Position);
            //float dis = Vector3.Distance(this.gameObject.transform.position, obj_NpcTarget.transform.position);
            //Debug.Log ("roseMoveNpc = " + roseMoveNpc + ";dis = " + dis + "yaogan="+ YaoGan_Status);
            ObscuredFloat npcDis = 2.5f;
            if (dis <= npcDis)
            {
                //移动至Npc附近
                roseMoveNpc = false;
                Move_Target_Status = false;
                ai_nav.SetDestination(this.gameObject.transform.position);  //拾取到物体，停止移动
                //故事对话状态是否打开,确保下面代码只执行一次
                if (!RoseStorySpeakStatus)
                {
                    bool isStory = false;
                    //获取当前选中的Npc是否有剧情对话
                    if (obj_NpcTarget != null)
                    {
                        string storyIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakID", "ID", obj_NpcTarget.GetComponent<AI_NPC>().NpcID, "Npc_Template");
                        if (storyIDStr != "0")
                        {
                            string[] storyID = (storyIDStr).Split(';');
                            for (int i = 0; i <= storyID.Length - 1; i++)
                            {
                                if (storyID[i] == Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"))
                                {
                                    isStory = true;
                                    RoseStorySpeakStatus = true;
                                }
                            }
                        }
                    }


                    if (isStory)
                    {
                        //实例化一个剧情UI
                        GameObject obj_StorySpeakSet = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_StorySpeakSet);
                        obj_StorySpeakSet.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_StorySpeakSet.transform);
                        obj_StorySpeakSet.transform.localPosition = Vector3.zero;
                        obj_StorySpeakSet.transform.localScale = new Vector3(1, 1, 1);
                        obj_StorySpeakSet.GetComponent<UI_StorySpeakSet>().GameStoryID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        if (obj_NpcTarget.GetComponent<AI_NPC>().NpcID != "" && obj_NpcTarget.GetComponent<AI_NPC>().NpcID != "0")
                        {
                            obj_StorySpeakSet.GetComponent<UI_StorySpeakSet>().NpcID = obj_NpcTarget.GetComponent<AI_NPC>().NpcID;
                            //隐藏主界面
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI.SetActive(false);
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcTaskSet.SetActive(false);
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(false);
                            //Debug.Log("隐藏对话");
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.SetActive(false);
                        }
                    }
                }

                //触发打开任务UI
                if (npcTaskUI == null)
                {
                    //Debug.Log ("npcTaskUInpcTaskUInpcTaskUI = ");
                    npcTaskUI = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_NpcTask);
                    npcTaskUI.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcTaskSet.transform);
                    //根据分辨率获取此界面的缩放值
                    //npcTaskUI.transform.localPosition = new Vector3(-350, 0, 0);   //暂时的位置,后期需要调整
                    npcTaskUI.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-350, 0, 0);
                    float scalePro = Game_PublicClassVar.Get_function_UI.ReturnScreenScalePro();
                    npcTaskUI.transform.localScale = new Vector3(scalePro, scalePro, scalePro);

                    //传入点击的NpcID
                    if (npcTaskUI.GetComponent<UI_NpcTask>() != null && obj_NpcTarget != null)
                    {
                        npcTaskUI.GetComponent<UI_NpcTask>().NpcID = obj_NpcTarget.GetComponent<AI_NPC>().NpcID;
                        npcTaskUI.GetComponent<UI_NpcTask>().CompleteTaskID = obj_NpcTarget.GetComponent<AI_NPC>().CompleteTaskID;      //传入完成的任务
                        npcTaskUI.GetComponent<UI_NpcTask>().Obj_Npc = obj_NpcTarget;
                    }
                }

                //播放音效,如果有多个音效随机播放一个
                if (obj_NpcTarget != null)
                {
                    if (obj_NpcTarget.GetComponent<AI_NPC>().NpcID != null && obj_NpcTarget.GetComponent<AI_NPC>().NpcID != "")
                    {
                        string sourceIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SourceID", "ID", obj_NpcTarget.GetComponent<AI_NPC>().NpcID, "Npc_Template");
                        if (sourceIDStr != "" && sourceIDStr != "0")
                        {
                            string[] sourceIDList = sourceIDStr.Split(';');
                            int nowListNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, sourceIDList.Length - 1);
                            if (sourceIDList[nowListNum] != "" && sourceIDList[nowListNum] != "0")
                            {
                                Game_PublicClassVar.Get_function_UI.PlaySource(sourceIDList[nowListNum], "2");
                            }
                        }
                    }
                }
            }
        }

        //更新传入的NPC任务
        if (Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus == "2")
        {
            if (npcTaskUI != null)
            {
                if (npcTaskUI.GetComponent<UI_NpcTask>() != null)
                {
                    npcTaskUI.GetComponent<UI_NpcTask>().CompleteTaskID = obj_NpcTarget.GetComponent<AI_NPC>().CompleteTaskID;      //传入完成的任务
                    npcTaskUI.GetComponent<UI_NpcTask>().UpdataStatus = true;
                }
            }
            Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus = "0";
        }

        //普通攻击
        if (roseActStatus)
        {
            if (!roseCriActStatus)
            {
                RoseStatus = "3";
                if (Obj_ActTarget != null)
                {
                    this.transform.LookAt(Obj_ActTarget.transform);     //转向目标
                }
            }
            else
            {
                RoseStatus = "4";
                if (Obj_ActTarget != null)
                {
                    this.transform.LookAt(Obj_ActTarget.transform);     //转向目标
                }
            }

            //获取目标血量是否为0
            if (Obj_ActTarget != null)
            {
                if (Obj_ActTarget.GetComponent<AI_Property>().AI_Hp <= 0)
                {
                    roseActStatus = false;
                    Obj_ActTarget = null;
                }
            }
            else
            {
                roseActStatus = false;
            }
        }

        //播放技能动作
        if (SkillAnimationStatus)
        {

            //Debug.Log("播放技能动作RoseSingStatus = " + RoseSingStatus + ";Move_Target_Status = " + Move_Target_Status + ";Move_Target_SaveStatus = " + Move_Target_SaveStatus);


            if (Move_Target_Status)
            {
                if (RoseSingStatus == false)
                {
                    //Debug.Log("开始移动,停止施法!");
                    Move_Target_SaveStatus = true;
                    Move_Target_Status = false;
                }
                else
                {
                    Move_Target_Status = false;
                }
            }

            /*
            if (RoseMapMoveStatus) {
                if (RoseSingStatus == false)
                {
                    Move_Target_SaveStatus = true;
                }
            }
            */

            if (RoseMapMoveNowStatus == true && RoseSingStatus == true)
            {
                RoseMapMoveNowStatus = false;
            }

            if (RoseMapMoveNowStatus == true && RoseSingStatus == false)
            {
                Move_Target_SaveStatus = true;
            }
            /*
            if (ZuoQiStatus) {
                Move_Target_Status = false;
            }
            */
            skillAnimationTimeSum = skillAnimationTimeSum + Time.deltaTime;
            if (skillAnimationTimeSum > SkillAnimationTime)
            {
                //播放完毕,清空各个值
                skillAnimationTimeSum = 0;
                SkillAnimationStatus = false;
                SkillAnimationTime = 0;
                //Move_Target_Status = true;
            }
            else
            {
                //播放技能僵直动作
                RoseStatus = "5";
                ai_nav.speed = 0;
                //Debug.Log("准备播放技能动作2");
            }
        }


        //判定角色是否在吟唱状态
        if (RoseSingStatus)
        {

            if (!roseSingUIStatus)
            {
                roseSingUIStatus = true;
                //实例化一个吟唱读条UI
                //实例化打开UI
                roseSingUIObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UISkillSing);
                //roseSingUIObj.transform.Find("Lab_OpenText").transform.GetComponent<Text>().text = "施法中……";
                roseSingUIObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_GetherItemSet.transform);
                roseSingUIObj.transform.localScale = new Vector3(1, 1, 1);
                roseSingUIObj.transform.localPosition = new Vector3(0, 30, 0);

                //停止移动
                ai_nav.SetDestination(this.gameObject.transform.position);  //拾取到物体，停止移动
                Move_Target_Status = false;


                /*
                if (RoseSingType == "2") {
                    roseSingUIObj.transform.Find("Img_GetherPro").transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                }
                */
            }

            //吟唱时间累加
            float value = roseSingTimeSum / RoseSingTime;
            //施法中的时候进度条为从1降至0
            if (RoseSingType == "2")
            {
                value = 1 - value;
            }

            roseSingUIObj.transform.Find("Img_GetherPro").GetComponent<Image>().fillAmount = value;
            roseSingTimeSum = roseSingTimeSum + Time.deltaTime;
            RoseStatus = "6";
            //Debug.Log("roseSingTimeSum = " + roseSingTimeSum);
            if (roseSingTimeSum > RoseSingTime)
            {
                //Debug.Log("施法时间结束");
                //施法结束
                RoseSingStopStatus = true;
                ifRoseSingWin = true;
            }

            //关闭吟唱状态
            if (RoseSingStopStatus)
            {
                switch (RoseSingType)
                {
                    //角色施法前状态
                    case "1":
                        //Debug.Log("施法1111111 RoseSingSkillObj = " + RoseSingSkillObj.name + "  ifRoseSingWin = " + ifRoseSingWin);
                        //如果技能释放成功
                        if (ifRoseSingWin)
                        {
                            if (RoseSingSkillObj != null)
                            {
                                RoseSingSkillObj.GetComponent<RoseSkill_Sing_1>().IfWaitSkillFrontSing = true;
                                //Debug.Log("技能施法成功");
                            }
                        }
                        else
                        {
                            if (RoseSingSkillObj != null)
                            {
                                RoseSingSkillObj.GetComponent<RoseSkill_Sing_1>().IfSkillFrontFail = true;
                                //Debug.Log("技能施法失败");
                            }
                        }
                        break;

                    //角色施法中状态
                    case "2":
                        Destroy(RoseSingSkillObj);      //删除技能OBJ
                        break;
                }
                //清空属性
                roseSingTimeSum = 0;
                RoseSingType = "0";
                RoseSingStatus = false;
                roseSingUIStatus = false;
                RoseSingStopStatus = false;
                ifRoseSingWin = false;
                Destroy(roseSingUIObj);         //删除吟唱读条UI

            }
        }
        else
        {

        }

        //翻滚状态 不进行移动
        if (FanGunStatus)
        {
            Move_Target_Status = false;
        }


        //移动状态
        if (Move_Target_Status)
        {

            //开启普通攻击
            if (Obj_ActTarget != null)
            {
                if (roseMoveToAct)
                {
                    /*
                    //攻击距离
                    actDistance = 3.0f;
                    switch(RoseOcc){
                        //战士攻击距离
                        case "1":
                            actDistance = 3.0f;
                            break;
                        //法师攻击距离
                        case "2":
                            actDistance = 8.0f;
                            break;
                    }
                    */

                    //更新攻击距离
                    if (actDistance <= 0)
                    {
                        GetActDis();
                    }

                    //到达指定目标开启攻击
                    if (Vector3.Distance(Obj_ActTarget.transform.position, this.transform.position) <= actDistance)
                    {
                        Move_Target_Status = false;
                        roseActStatus = true;
                        Move_Target_Position = this.transform.position;
                        roseMoveToAct = false;
                    }
                }
            }

            //向服务器发送坐标信息
            if (MovePositionSendStatus)
            {

                moveSendTime = moveSendTime + Time.deltaTime;

                //3秒发送一次
                ObscuredFloat sendTime = 0.3f;
                if (moveSendTime >= sendTime)
                {
                    moveSendTime = 0;

                    //Pro_Fight_MoveList proFight_Move = new Pro_Fight_MoveList();
                    //Vector3 vector3 = this.gameObject.transform.position;
                    //proFight_Move.MapName = "";
                    //proFight_Move.Move_X = (int)(vector3.x * 100);
                    //proFight_Move.Move_Y = (int)(vector3.y * 100);
                    //proFight_Move.Move_Z = (int)(vector3.z * 100);
                    //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20000002, proFight_Move);

                    //判定当前不是特殊地图则取消发送状态
                    if (Application.loadedLevelName != "EnterGame")
                    {
                        MovePositionSendStatus = false;
                    }

                    switch (MovePositionSendType)
                    {

                        //精准发送
                        case "1":
                            MapThread_PlayerMove mapThread_PlayerMove = new MapThread_PlayerMove();
                            Vector3 vector3 = this.gameObject.transform.position;
                            //mapThread_PlayerMove.MapName = "Map_" + Application.loadedLevelName.ToString();
                            mapThread_PlayerMove.MapName = EnterMapServerName;
                            mapThread_PlayerMove.Move_X = (int)(vector3.x * 100);
                            mapThread_PlayerMove.Move_Y = (int)(vector3.y * 100);
                            mapThread_PlayerMove.Move_Z = (int)(vector3.z * 100);
                            mapThread_PlayerMove.MoveStatus = true;
                            switch (MovePositionSendServerType)
                            {
                                case "1":
                                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20000103, mapThread_PlayerMove);
                                    break;
                                case "2":
                                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20000203, mapThread_PlayerMove);
                                    break;
                            }

                            break;

                        //模糊发送
                        case "2":
                            mapThread_PlayerMove = new MapThread_PlayerMove();
                            if (YaoGan_Status)
                            {
                                vector3 = MovePositionVec3_YaoGan;
                            }
                            else
                            {
                                vector3 = Move_Target_Position;
                            }
                            //mapThread_PlayerMove.MapName = "Map_" + Application.loadedLevelName.ToString();
                            mapThread_PlayerMove.MapName = EnterMapServerName;
                            mapThread_PlayerMove.Move_X = (int)(vector3.x * 100);
                            mapThread_PlayerMove.Move_Y = (int)(vector3.y * 100);
                            mapThread_PlayerMove.Move_Z = (int)(vector3.z * 100);
                            mapThread_PlayerMove.MoveStatus = true;
                            switch (MovePositionSendServerType)
                            {
                                case "1":
                                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20000103, mapThread_PlayerMove);
                                    break;
                                case "2":
                                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20000203, mapThread_PlayerMove);
                                    break;
                            }

                            //Debug.Log("移动发送坐标" + mapThread_PlayerMove.Move_X + ";" + mapThread_PlayerMove.Move_Z);

                            break;
                    }
                }
            }

            //防止寻路错误
            try
            {
                ai_nav.SetDestination(Move_Target_Position);
            }
            catch
            {
                Debug.LogError("寻路报错");
            }

            RoseStatus = "2";       //设置跑动

            //要不会卡探索
            if (RoseGetherItemStatus)
            {
                RoseGetherItemStatus = false;
            }

            //检测当前是否处于技能吟唱状态
            if (RoseSingStatus)
            {
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("移动操作取消吟唱状态");
                RoseSingStopStatus = true;
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_256");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Debug.Log("技能施法状态中断1111！");
            }

            //移动到目标后关闭移动状态;
            Vector3 dis1 = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            Vector3 dis2 = new Vector3(Move_Target_Position.x, 0, Move_Target_Position.z);
            if (Vector3.Distance(dis1, dis2) <= 0.2f)
            {
                Move_Target_Status = false;
            }

            //获取和上一帧的距离，如果相同移动停止
            if (this.transform.position == lastRosePosition)
            {
                //原地停留3秒判定为不移动
                lastRosePositionTime = lastRosePositionTime + Time.deltaTime;

                if (lastRosePositionTime >= 10)
                {
                    //Debug.Log("停止移动！！！！");
                    Move_Target_Status = false;
                    lastRosePositionTime = 0;
                }
                else
                {
                    //lastRosePositionTime = 0;
                }
            }
            else
            {
                lastRosePositionTime = 0;
            }
            //获取上一帧自己的位置
            lastRosePosition = this.transform.position;

        }

        //Debug.Log(RoseStatus);

        //播放动画
        roseAnimatorOpen(RoseStatus);

        //当自身除移动以外的操作时,不能打开唯一的UI
        /*
        if (RoseStatus != "1") {
            Game_PublicClassVar.Get_game_PositionVar.roseOpenOnlyUI = false;
        }
        */

        //当角色受到攻击
        if (RoseIfHit)
        {

            RoseIfHit = false;

            //记录飘字血量和上一次血量
            rose_HitHp = rose_LastHp - roseProperty.Rose_HpNow;
            rose_LastHp = roseProperty.Rose_HpNow;

            //受到攻击触发技能
            if (passiveSkillID[0] != "0" && passiveSkillID[0] != "")
            {
                for (int i = 0; i <= passiveSkillID.Length - 1; i++)
                {
                    //获取技能触发类型
                    string skillType = passiveSkillType[i];
                    switch (skillType)
                    {
                        //受到攻击触发
                        case "3":
                            if (Random.value <= float.Parse(passiveSkillPro[i]))
                            {
                                triggerSkill(i);        //触发怪物技能  
                            }
                            break;
                    }
                }
            }

            /*
			GameObject HitObject_p = (GameObject)Instantiate(HitObject);
			//RoseHitTextlater = "<color=#D2D2D2><size=18> (黑暗抵抗)</size></color>";
			//RoseHitText = "<color=#D2D2D2><size=18>抵抗 </size></color>";
			HitObject_p.GetComponent<Text> ().text = RoseHitText + "-" + rose_HitHp.ToString() + RoseHitTextlater;

			//恢复血量飘字为绿色
			if (rose_HitHp <= 0) {
				rose_HitHp = rose_HitHp * -1;
				if (RoseIfXiXue) {
					RoseIfXiXue = false;
					HitObject_p.GetComponent<Text> ().text = "吸血+" + rose_HitHp.ToString ();
				} else {
					HitObject_p.GetComponent<Text> ().text = RoseHitText + "+" + rose_HitHp.ToString () + RoseHitTextlater;
				}
                
				HitObject_p.GetComponent<Text> ().color = Color.green;
			}
			*/
            //播放受击特效
            if (HitEffectObj != null)
            {
                GameObject hitEffectObj = (GameObject)Instantiate(HitEffectObj);
                hitEffectObj.transform.SetParent(this.GetComponent<Rose_Bone>().Bone_Center.transform);
                hitEffectObj.transform.localPosition = new Vector3(0, 0, 0);
                hitEffectObj.transform.localScale = new Vector3(1, 1, 1);
                //清空
                HitEffectObj = null;
            }

            //伤害为0表示闪避            
            if (rose_HitHp == 0)
            {
                //判定当前是否在护盾状态
                /*
                if (this.GetComponent<Rose_Proprety> ().HuDunStatus) {
                    HitObject_p.GetComponent<Text> ().text = "护盾抵消！";
                } else {
                    if (RpseIfDodge) {
                        RpseIfDodge = false;
                        HitObject_p.GetComponent<Text> ().text = "闪避";
                    }
                }
                */
            }
            else
            {
                //判断当前是否在施法中,如果在施法中每次受到攻击减少20%施法时间
                if (RoseSingStatus)
                {
                    float costValue = RoseSingTime * 0.2f;
                    switch (RoseSingType)
                    {
                        case "1":
                            roseSingTimeSum = roseSingTimeSum - costValue;
                            break;
                        case "2":
                            roseSingTimeSum = roseSingTimeSum + costValue;
                            break;
                    }

                    //如果值小于0判定施法状态结束
                    if (roseSingTimeSum <= 0)
                    {
                        roseSingTimeSum = 0;
                        RoseSingStopStatus = true;
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_258");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("技能施法状态中断2222！");
                    }
                }

                //受到伤害每次有5%概率击落下马
                if (Random.value <= 0.1f)
                {
                    if (ZuoQiStatus)
                    {
                        ZuoQiStatus = false;
                        roseAnimator.Play("Idle");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("怪物用力将你从马上击落...");
                    }
                }
            }
            /*
            HitObject_p.transform.SetParent (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set> ().Obj_UI_RoseHp.transform);
            HitObject_p.transform.localPosition = new Vector3 (0, 40, 0);
            HitObject_p.transform.localScale = new Vector3 (1, 1, 1);
            */

            if (rose_HitHp > 0)
            {

                if (ui_GetherItem != null)
                {
                    if (ui_GetherItem.IfRoseTake)
                    {
                        Destroy(ui_GetherItem.mainUIGetherItem);
                        ui_GetherItem.IfRoseTake = false;
                        ui_GetherItem.openTimeSum = 0;
                        ui_GetherItem.openStatusOnce = false;
                    }
                }
            }


            //触发角色反击
            if (RoseStatus == "1")
            {
                if (fanjiStatusNum >= 4)
                {
                    //触发反击
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().AutomaticFight(false);
                    //清空反击
                    fanjiStatusNum = 0;

                    //反击修改角色状态
                    if (ZuoQiStatus)
                    {
                        ZuoQiStatus = false;
                    }
                }
                fanjiStatusNum = fanjiStatusNum + 1;
            }

            //Debug.Log("触发受击技能效果！");
            actTriggerSkill("2", "3");  //受到伤害触发被动技能

        }

        //当角色升级
        if (RoseUpLv)
        {
            RoseUpLv = false;
            //播放升级特效
            GameObject effRoseUp = (GameObject)Instantiate(Eff_RoseUp);
            effRoseUp.transform.SetParent(GetComponent<Rose_Bone>().Bone_Low.transform);
            effRoseUp.transform.localPosition = new Vector3(0, 0, 0);
            effRoseUp.transform.localScale = new Vector3(1, 1, 1);
            Destroy(effRoseUp, 2);
            Game_PublicClassVar.Get_function_Task.updataTaskItemID();         //更新等级类任务
            OpenEmenyAct();     //更新打开建筑攻击
            //播放音效
            Game_PublicClassVar.Get_function_UI.PlaySource("20010", "2");
        }

        //更新被动技能
        if (UpdataPassiveSkillStatus)
        {
            updataPassiveSkill();
            UpdataPassiveSkillStatus = false;
        }
        //每秒扣除被动技能的冷却时间
        if (ifSkillTrigger != null)
        {
            triggerTimeOnceSum = triggerTimeOnceSum + Time.deltaTime;
            if (triggerTimeOnceSum >= 1.0f)
            {
                //每秒更新
                for (int i = 0; i <= ifSkillTrigger.Length - 1; i++)
                {
                    if (ifSkillTrigger[i] == "1")
                    {
                        //获取被动技能间隔时间
                        float triggerTimeOnceValue = float.Parse(passiveSkillTriggerTimeOnce[i]);
                        triggerTimeOnceValue = triggerTimeOnceValue - 1;
                        //Debug.Log("triggerTimeOnceValue = " + triggerTimeOnceValue);
                        if (triggerTimeOnceValue <= 0)
                        {
                            ifSkillTrigger[i] = "0";
                            if (i < passiveSkillID.Length)
                            {
                                string skillCD = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", passiveSkillID[i], "Skill_Template");
                                //Debug.Log("技能ID" + passiveSkillID[i] + "技能CD结束" + skillCD);
                                passiveSkillTriggerTimeOnce[i] = skillCD;
                                //Debug.Log("清空");
                            }
                        }
                        else
                        {
                            passiveSkillTriggerTimeOnce[i] = triggerTimeOnceValue.ToString();
                            //Debug.Log("triggerTimeOnceValue = " + passiveSkillTriggerTimeOnce[i]);
                        }
                        triggerTimeOnceSum = 0;
                    }
                }
                actTriggerSkill("2", "2");  //被动技能
            }
        }
        //跑动声音时间累计
        //runSourceTime = runSourceTime + Time.deltaTime;


        //如果选中目标离自己超过10的距离则不能选中当前目标
        if (Obj_ActTarget != null)
        {
            ObscuredFloat targetDis = 10;
            if (Vector3.Distance(Obj_ActTarget.transform.position, this.gameObject.transform.position) > targetDis)
            {
                if (Obj_ActTarget.transform.GetComponent<AI_1>().AI_Status == "4")
                {
                    Obj_ActTarget.transform.GetComponent<AI_1>().AI_Selected_Status = false;
                    Obj_ActTarget = null;
                }
            }
        }


        //进入和退出战斗
        if (RoseFightStatus)
        {
            if (!RoseFightFirstStatus)
            {
                RoseFightFirstStatus = true;
                //Debug.Log("进入战斗");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_259");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("进入战斗");
            }

            //攻击列表为空退出战斗状态
            bool ifnull = false;
            if (RoseFightMonsterActList.Count == 0)
            {
                ifnull = true;
            }
            else
            {
                //攻击列表为空
                ifnull = true;
                for (int i = 0; i <= RoseFightMonsterActList.Count - 1; i++)
                {
                    if (RoseFightMonsterActList[i] != null)
                    {
                        ifnull = false;
                    }
                }
            }

            if (ifnull)
            {
                RoseFightStatus = false;
                FightExitZuoQiStatus = true;
            }
        }
        else
        {
            if (RoseFightFirstStatus)
            {
                RoseFightFirstStatus = false;
                //Debug.Log("退出战斗");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_260");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("退出战斗");

                //清空宠物CD
                Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD.Clear();

            }

            //攻击目标距离较远则取消目标
            if (Obj_ActTarget != null)
            {
                float dis = Vector3.Distance(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget.transform.position);
                if (dis >= 25.0f)
                {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget.GetComponent<AI_1>().AI_Selected_Status = false;
                    //开启更新怪物选中状态
                    Game_PublicClassVar.Get_game_PositionVar.UpdataSelectedStatus = true;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget = null;
                }
            }
        }

        //自动挂机
        AutomaticActTimeSum = AutomaticActTimeSum + Time.deltaTime;
        if (AutomaticGuaJiStatus && AutomaticActTimeSum >= 1)
        {

            //判定当前挂机数量
            if (AutomaticGuaJiNum >= 2000) {
                AutomaticGuaJiStatus = false;
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日挂机数量已满!每日凌晨重置数量..");
                return;
            }

            AutomaticActTimeSum = 0;        //每秒触发一次
            if (Obj_ActTarget == null)
            {
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().AutomaticGuaJi();
            }
            else
            {
                if (RoseStatus != "3")
                {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().AutomaticGuaJi();
                }
            }
        }

        if (FightExitZuoQiStatus)
        {
            FightExitZuoQiTime = FightExitZuoQiTime + Time.deltaTime;
            if (FightExitZuoQiTime >= 3)
            {
                FightExitZuoQiStatus = false;
                FightExitZuoQiTime = 0;

                if (ZuoQiZiDongStatus)
                {
                    if (Obj_ZuoQiShowModel != null)
                    {
                        ZuoQiStatus = true;
                        Obj_ZuoQiShowModel.GetComponent<ZuoQiShowModel>().PlayStartEffect();
                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuffListShowSet.GetComponent<UI_BuffShowListSet>().UpdateZuoQiBuffStatus = true;
                    }
                }
            }
        }
    }

    //普通攻击
    public void RoseAct()
    {
        if (RoseSingStatus)
        {
            return;
        }

        if (Obj_ActTarget != null)
        {
            if (Obj_ActTarget.GetComponent<AI_1>().AI_Status == "4")
            {
                RoseStatus = "1";
                roseActStatus = false;
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_261");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("怪物已脱离,无法攻击");
                return;
            }
        }

        if (Time.time - mLastAttackTime < mMaxIntervalTime)
        {
            return;
        }
        mLastAttackTime = Time.time;
        //主动攻击会下马
        if (ZuoQiStatus)
        {
            ZuoQiStatus = false;
        }

        switch (RoseOcc)
        {

            //战士普通攻击
            case "1":

                //Debug.Log("我攻击了");
                //判断与目标的距离,小于3就不进行攻击,设置为待机
                /*
                if (Obj_ActTarget != null) {
                    float actDis = Vector3.Distance(this.gameObject.transform.position, Obj_ActTarget.transform.position);
                    Debug.Log("actDis = " + actDis);
                    if (actDis >= 3)
                    {
                        RoseStatus = "1"; //设置为待机状态
                        roseActStatus = false;
                        break;
                    }
                }
                */

                //设置受击特效
                if (Obj_ActTarget != null)
                {
                    Obj_ActTarget.GetComponent<AI_1>().HitStatus = true;      //播放受击特效
                    Obj_ActTarget.GetComponent<AI_1>().HitEffect = (GameObject)Resources.Load("Effect/Skill/Rose_Act_2");
                    Obj_ActTarget.GetComponent<AI_1>().HitEffect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    Obj_ActTarget.GetComponent<AI_1>().IfHitEffect = true;
                    Game_PublicClassVar.Get_fight_Formult.RoseActMonster("62000001", Obj_ActTarget, false, true);
                    if (Random.value < this.GetComponent<Rose_Proprety>().Rose_Cri)
                    {
                        roseCriActStatus = true;
                        //Debug.Log("我开启了暴击");
                    }

                    rosePassiveSkill(); //被动技能

                    actTriggerSkill("2", "1");  //被动技能

                    //播放音效
                    Game_PublicClassVar.Get_function_UI.PlaySource("20001", "2");
                }

                break;

            //法师普通攻击
            case "2":

                //设置实例化技能
                string fashi_SkillID = "60011001";
                string skillObjName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectName", "ID", fashi_SkillID, "Skill_Template");
                GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill_Obj/" + skillObjName, typeof(GameObject));
                GameObject SkillObject_p = (GameObject)Instantiate(SkillObj);
                SkillObject_p.SetActive(false);
                //设置技能位置
                SkillObject_p.transform.localRotation = this.transform.rotation;
                SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
                SkillObject_p.transform.position = new Vector3(rosePosiVec3.x, rosePosiVec3.y + 0.5f, rosePosiVec3.z);
                SkillObject_p.SetActive(true);
                //传递技能对应的值
                SkillObjBase skillStatus = SkillObject_p.transform.GetComponent<SkillObjBase>();
                //skillStatus.SkillTargetPoint = SkillTargetPoint;        //技能目标点
                skillStatus.SkillID = fashi_SkillID;
                skillStatus.SkillOpen = true;   //开启技能，需要在设置完值后开启技能
                skillStatus.SkillTargetObj = Obj_ActTarget;

                rosePassiveSkill(); //被动技能

                actTriggerSkill("2", "1");  //被动技能

                //播放音效
                Game_PublicClassVar.Get_function_UI.PlaySource("20015", "2");
                break;

            //猎人普通攻击
            case "3":

                //设置实例化技能
                fashi_SkillID = "60012001";
                skillObjName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectName", "ID", fashi_SkillID, "Skill_Template");
                SkillObj = (GameObject)Resources.Load("Effect/Skill_Obj/" + skillObjName, typeof(GameObject));
                SkillObject_p = (GameObject)Instantiate(SkillObj);
                SkillObject_p.SetActive(false);
                //设置技能位置
                SkillObject_p.transform.localRotation = this.transform.rotation;
                SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
                SkillObject_p.transform.position = new Vector3(rosePosiVec3.x, rosePosiVec3.y + 0.5f, rosePosiVec3.z);
                SkillObject_p.SetActive(true);
                //传递技能对应的值
                skillStatus = SkillObject_p.transform.GetComponent<SkillObjBase>();
                //skillStatus.SkillTargetPoint = SkillTargetPoint;        //技能目标点
                skillStatus.SkillID = fashi_SkillID;
                skillStatus.SkillOpen = true;   //开启技能，需要在设置完值后开启技能
                skillStatus.SkillTargetObj = Obj_ActTarget;

                rosePassiveSkill(); //被动技能

                actTriggerSkill("2", "1");  //被动技能

                //播放音效
                Game_PublicClassVar.Get_function_UI.PlaySource("20015", "2");
                break;
        }

        //退出隐身状态
        Game_PublicClassVar.Get_function_Rose.ExitRoseInvisible();
    }

    //暴击攻击
    public void RoseCriAct()
    {
        if (Time.time - mLastCriAckTime < mMaxIntervalTime)
            return;
        mLastCriAckTime = Time.time;
        //Debug.Log("我暴击了");
        if (Obj_ActTarget != null)
        {
            Obj_ActTarget.GetComponent<AI_1>().HitStatus = true;      //播放受击特效
            Game_PublicClassVar.Get_fight_Formult.RoseActMonster("62000001", Obj_ActTarget, true);
            //播放音效
            Game_PublicClassVar.Get_function_UI.PlaySource("20002", "2");
            //actTriggerSkill("2", "4");  //暴击时触发被动技能
        }

        //Obj_ActTarget.GetComponent<AI_1>().HitEffect = null;
        //Obj_ActTarget.GetComponent<AI_1>().HitEffect = (GameObject)Resources.Load("Effect/Skill/Rose_CriAct");
        //Obj_ActTarget.GetComponent<AI_1>().IfHitEffect = true;

        roseCriActStatus = false;

        GameObject effectObj = (GameObject)Instantiate((GameObject)Resources.Load("Effect/Skill/Rose_CriAct"));
        if (Obj_ActTarget != null)
        {
            effectObj.transform.SetParent(Obj_ActTarget.GetComponent<AI_1>().BoneSet.transform.Find("Di").transform);
            effectObj.transform.localPosition = new Vector3(0, 0.1f, 0);
            effectObj.transform.localScale = new Vector3(1, 1, 1);
        }

        /*
        //角色爆击时,进入战斗状态
        if (!RoseFightStatus)
        {
            RoseFightStatus = true;
        }
        */

        //退出隐身状态
        Game_PublicClassVar.Get_function_Rose.ExitRoseInvisible();

    }

    public void RoseActStopMove_Start()
    {
        //设置不触发摇杆的时间
        if (Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus)
        {
            switch (RoseOcc)
            {
                case "1":
                    YaoGanStopMoveTime = 0.3f;
                    break;
                case "2":
                    YaoGanStopMoveTime = 0.3f;
                    break;
                case "3":
                    YaoGanStopMoveTime = 1f;
                    break;
            }
        }

        //Debug.Log("YaoGanStopMoveTime = " + YaoGanStopMoveTime);
    }

    public void RoseActStopMove_End()
    {
        //设置不触发摇杆的时间
        if (Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus)
        {
            YaoGanStopMoveTime = 0.0f;
        }

        YaoGanStopMoveTime = 0.0f;

        //Debug.Log("YaoGanStopMoveTime = " + YaoGanStopMoveTime);
    }

    public void RoseRunSource()
    {

        Game_PublicClassVar.Get_function_UI.PlaySource("20013", "2");
    }


    //触发被动技能
    void rosePassiveSkill()
    {

        //获取自己的被动技能
        string[] beidongSkillIDStr = skillIDBeiDong.ToString().Split(';');
        for (int i = 0; i <= beidongSkillIDStr.Length - 1; i++)
        {
            if (beidongSkillIDStr[i] != "")
            {
                string[] beidongSkillID = beidongSkillIDStr[i].Split(',');
                if (beidongSkillID[0] != "")
                {
                    //根据触发概率判定是否触发被动技能
                    if (Random.value <= float.Parse(beidongSkillID[1]))
                    {
                        //Debug.Log("触发被动技能ID：" + beidongSkillID[0]);
                    }
                }
            }
        }
    }


    //触发被动技能,triggerTime表示触发时间,1 表示起手动作前 ,2表示起手动作后    triggerType表示触发类型 1：表示攻击时触发  2：表示每帧都监测一次
    public void actTriggerSkill(string triggerTime, string triggerType)
    {
        //判定触发被动技能
        if (passiveSkillID[0] != "0" && passiveSkillID[0] != "")
        {
            //Debug.Log("准备触发技能 = " + passiveSkillID[0]);
            for (int i = 0; i <= passiveSkillID.Length - 1; i++)
            {
                //获取技能触发类型
                string skillType = passiveSkillType[i];
                switch (skillType)
                {
                    //每次攻击概率触发
                    case "1":
                        //如果当前技能不是1直接跳出
                        if (triggerType != "1")
                        {
                            break;
                        }
                        switch (passiveSkillTriggerTime[i])
                        {
                            //随时触发,无限定条件
                            case "0":
                                if (Random.value <= float.Parse(passiveSkillPro[i]))
                                {
                                    //if (actStatus)
                                    //{
                                    triggerSkill(i);        //触发怪物技能
                                    //}
                                }
                                break;
                            //每次有攻击行为时触发
                            case "1":
                                if (passiveSkillTriggerTime[i] == triggerTime)
                                {

                                    //if (actStatus)
                                    //{
                                    //actStatus = false;
                                    if (Random.value <= float.Parse(passiveSkillPro[i]))
                                    {
                                        triggerSkill(i);        //触发怪物技能
                                    }
                                    //}
                                }

                                break;
                            //每次有攻击动作时触发
                            case "2":
                                if (passiveSkillTriggerTime[i] == triggerTime)
                                {

                                    if (Random.value <= float.Parse(passiveSkillPro[i]))
                                    {
                                        triggerSkill(i);        //触发怪物技能
                                    }
                                }

                                break;
                        }

                        break;
                    //血量低于多少百分比触发
                    case "2":
                        //如果当前技能不是2直接跳出
                        if (triggerType != "2")
                        {
                            break;
                        }

                        //获取自身生命值
                        if (GetComponent<Rose_Proprety>().Rose_HpNow <= GetComponent<Rose_Proprety>().Rose_Hp * float.Parse(passiveSkillPro[i]))
                        {
                            if (ifSkillTrigger[i] == "0")
                            {
                                triggerSkill(i);            //触发技能
                            }
                        }

                        break;

                    //受到攻击触发
                    case "3":
                        //如果当前技能不是3直接跳出
                        if (triggerType != "3")
                        {
                            break;
                        }

                        if (Random.value <= float.Parse(passiveSkillPro[i]))
                        {
                            if (ifSkillTrigger[i] == "0")
                            {
                                triggerSkill(i);        //触发技能
                            }
                        }
                        break;

                    //暴击时触发
                    case "4":
                        //如果当前技能不是4直接跳出
                        if (triggerType != "4")
                        {
                            break;
                        }

                        if (Random.value <= float.Parse(passiveSkillPro[i]))
                        {
                            if (ifSkillTrigger[i] == "0")
                            {
                                Debug.Log("暴击触发技能");
                                triggerSkill(i);        //触发技能
                            }
                        }
                        break;

                    //闪避时触发
                    case "5":
                        //如果当前技能不是5直接跳出
                        if (triggerType != "5")
                        {
                            break;
                        }

                        if (Random.value <= float.Parse(passiveSkillPro[i]))
                        {
                            if (ifSkillTrigger[i] == "0")
                            {
                                triggerSkill(i);        //触发技能
                            }
                        }
                        break;
                }
            }
        }
        //清除状态
        //actStatus = false;
    }
    //参数 i 当前触发第几个技能
    void triggerSkill(int i)
    {

        if (ifSkillTrigger[i] == "0")
        {
            //Debug.Log("passiveSkillID[i] = " + passiveSkillID[i]);
            //防止技能执行多次
            if (passiveSkillTriggerOnce[i] == "1")
            {
                ifSkillTrigger[i] = "1";
                string skillCD = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", passiveSkillID[i], "Skill_Template");
                //读取冷却技能缩小时间
                skillCD = (float.Parse(skillCD) * (1 - Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_SkillCDTimePro)).ToString();

                //技能附加值,减少冷却CD
                int skillAddValue_skillCD = 0;
                string skillAddValueStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(passiveSkillID[i], "4");
                if (skillAddValueStr != "" && skillAddValueStr != "0")
                {
                    string[] skillAddValueList = skillAddValueStr.Split(',');
                    for (int y = 0; y < skillAddValueList.Length; y++)
                    {
                        skillAddValue_skillCD = skillAddValue_skillCD + int.Parse(skillAddValueList[y]);
                    }
                }

                float nowSkillCD = float.Parse(skillCD) - skillAddValue_skillCD;
                if (nowSkillCD < 1)
                {
                    nowSkillCD = 1;
                }
                passiveSkillTriggerTimeOnce[i] = nowSkillCD.ToString();
            }
            else
            {
                ifSkillTrigger[i] = "1";
            }

            //触发BUFF
            string skillObjName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectName", "ID", passiveSkillID[i], "Skill_Template");
            if (skillObjName != "" && skillObjName != "0")
            {

                GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill_Obj/" + skillObjName, typeof(GameObject));
                GameObject SkillObject_p = (GameObject)Instantiate(SkillObj);

                SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = Obj_ActTarget;
                string skillParent = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParent", "ID", passiveSkillID[i], "Skill_Template");
                //skillParent = "0";
                switch (skillParent)
                {
                    //绑定在身上
                    case "0":

                        //目前只支持对自己附加
                        //Debug.Log("技能挂在自己身上+" + passiveSkillID[i]);
                        string skillParentPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", passiveSkillID[i], "Skill_Template");
                        SkillObject_p.transform.SetParent(this.GetComponent<Rose_Bone>().BoneSet.transform.Find(skillParentPosition));
                        SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);
                        SkillObject_p.GetComponent<SkillObjBase>().SkillID = passiveSkillID[i];
                        SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                        SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;
                        //SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = this.gameObject.transform.position;
                        //SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;
                        //triggerSkillID = passiveSkillID[i];
                        //SkillStatus = true;         //开启技能状态

                        break;
                    //无绑定点
                    case "1":
                        //目前只支持对攻击目标区域释放
                        //获取攻击目标位置
                        if (Obj_ActTarget != null)
                        {
                            Vector3 skillPosition = Obj_ActTarget.transform.position;
                            SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                            SkillObject_p.transform.position = skillPosition;
                        }
                        else
                        {
                            Vector3 skillPosition = this.gameObject.transform.position;
                            SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                            SkillObject_p.transform.position = skillPosition;
                        }

                        SkillObject_p.GetComponent<SkillObjBase>().SkillID = passiveSkillID[i];
                        SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                        SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;

                        //SkillStatus = true;         //开启技能状态
                        //triggerSkillID = passiveSkillID[i];
                        break;

                    //无绑定点,释放起始位置位于AI中心
                    case "2":
                        //目前只支持对攻击目标区域释放
                        //获取攻击目标位置
                        if (Obj_ActTarget != null)
                        {
                            Vector3 skillPosition = Obj_ActTarget.transform.position;
                            string playStartPoisition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", passiveSkillID[i], "Skill_Template");
                            SkillObject_p.transform.position = this.GetComponent<Rose_Bone>().BoneSet.transform.Find(playStartPoisition).transform.position;
                            SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                        }
                        else
                        {
                            Vector3 skillPosition = this.gameObject.transform.position;
                            SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                            SkillObject_p.transform.position = skillPosition;
                        }

                        SkillObject_p.GetComponent<SkillObjBase>().SkillID = passiveSkillID[i];
                        SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                        SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;
                        SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = Obj_ActTarget;

                        //SkillStatus = true;         //开启技能状态
                        //triggerSkillID = passiveSkillID[i];
                        break;

                    //无绑定点
                    case "3":
                        //目前只支持对攻击目标区域释放
                        //获取攻击目标位置
                        if (Obj_ActTarget != null)
                        {
                            Vector3 skillPosition = Obj_ActTarget.transform.position;
                            SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                            SkillObject_p.transform.position = this.GetComponent<Rose_Bone>().Bone_Center.transform.position;
                        }
                        else
                        {
                            Vector3 skillPosition = this.gameObject.transform.position;
                            SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                            SkillObject_p.transform.position = this.GetComponent<Rose_Bone>().Bone_Center.transform.position;
                        }

                        SkillObject_p.GetComponent<SkillObjBase>().SkillID = passiveSkillID[i];
                        SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                        SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;

                        //SkillStatus = true;         //开启技能状态
                        //triggerSkillID = passiveSkillID[i];
                        break;
                }
            }
        }
    }


    public void roseAnimatorOpen(string openAnimator)
    {

        /*
        if (RoseActActionStatus) {
            if (openAnimator == "3") {
                Debug.Log("##################################");
            }
            return;
        }
        */

        //初始化所有值
        roseAnimator.SetBool("Idle", false);
        roseAnimator.SetBool("Run", false);
        roseAnimator.SetBool("Act_1", false);
        roseAnimator.SetBool("Act_2", false);
        roseAnimator.SetBool("CriAct", false);
        roseAnimator.SetBool("Skill_1", false);
        roseAnimator.SetBool("Skill_2", false);
        roseAnimator.SetBool("Skill_3", false);

        roseAnimator.speed = mAnimationSpeed;
        //RoseWeaponEffectTrail.SetActive(false);
        Obj_RunEffect.GetComponent<ParticleSystem>().Stop();

        bool ifShowZuoQi = false;
        switch (openAnimator)
        {
            case "1":

                if (ZuoQiStatus)
                {
                    roseAnimator.Play("ZuoQi");
                    if (Obj_ZuoQiShowModel != null)
                    {
                        Obj_RoseShowModel.transform.position = Obj_ZuoQiShowPosition.transform.position;
                        Obj_RoseShowModel.transform.rotation = Obj_ZuoQiShowPosition.transform.rotation;
                        Obj_ZuoQiShowModel.GetComponent<ZuoQiShowModel>().ZuoQiStatus = openAnimator;
                        Obj_ZuoQiShowModel.GetComponent<ZuoQiShowModel>().ZuoQiPlay();
                        ifShowZuoQi = true;
                    }
                }
                else
                {
                    //if (YaoGanStopMoveTime <= 0)
                    //{
                    if (YaoGan_Status)
                    {
                        if (YaoGanStopMoveTime <= 0)
                        {

                            roseAnimator.SetBool("Idle", true);
                            //YaoGanStopMoveTime = 0;
                        }
                    }
                    else
                    {
                        roseAnimator.SetBool("Idle", true);
                        //YaoGanStopMoveTime = 0;
                    }


                    //销毁挥动武器特效
                    if (roseWeaponEffectTrail != null)
                    {
                        Destroy(roseWeaponEffectTrail);
                    }
                    //RoseWeaponEffectTrail.SetActive(false);
                    //}
                }

                break;
            case "2":

                //if (ZuoQiStatus && RoseFightStatus == false)
                if (ZuoQiStatus)
                {
                    roseAnimator.Play("ZuoQi");
                    if (Obj_ZuoQiShowModel != null)
                    {
                        Obj_RoseShowModel.transform.position = Obj_ZuoQiShowPosition.transform.position;
                        Obj_RoseShowModel.transform.rotation = Obj_ZuoQiShowPosition.transform.rotation;
                        Obj_ZuoQiShowModel.GetComponent<ZuoQiShowModel>().ZuoQiStatus = openAnimator;
                        Obj_ZuoQiShowModel.GetComponent<ZuoQiShowModel>().ZuoQiPlay();
                        ifShowZuoQi = true;
                    }

                    //根据不同的坐骑,拖尾效果不同？
                    Obj_RunEffect.GetComponent<ParticleSystem>().Pause(true);
                    Obj_RunEffect.GetComponent<ParticleSystem>().Play();

                }
                else
                {
                    roseAnimator.Play("Run");
                    Obj_RunEffect.GetComponent<ParticleSystem>().Pause(true);
                    Obj_RunEffect.GetComponent<ParticleSystem>().Play();
                    /*
                    if (YaoGan_Status)
                    {
                        if (YaoGanStopMoveTime <= 0)
                        {
                            roseAnimator.SetBool("Run", true);
                            //YaoGanStopMoveTime = 0;
                            Obj_RunEffect.GetComponent<ParticleSystem>().Pause(true);
                            Obj_RunEffect.GetComponent<ParticleSystem>().Play();
                        }
                    }
                    else
                    {
                        roseAnimator.SetBool("Run", true);
                        Obj_RunEffect.GetComponent<ParticleSystem>().Pause(true);
                        Obj_RunEffect.GetComponent<ParticleSystem>().Play();
                        //YaoGanStopMoveTime = 0;
                    }
                    */
                    //跑动销毁刀光
                    if (roseWeaponEffectTrail != null)
                    {
                        Destroy(roseWeaponEffectTrail);
                    }

                    //播放声音
                    /*
                    if (runSourceTime >= 0.5f) {
                        Game_PublicClassVar.Get_function_UI.PlaySource("20013","2");
                        runSourceTime = 0;
                    }
                    */
                    //}

                    //清空反击
                    fanjiStatusNum = 0;
                }

                break;
            case "3":

                float value = Random.value;
                if (value <= 0.5f)
                {
                    roseAnimator.SetBool("Act_1", true);
                    roseAnimator.SetBool("Act_2", false);

                }
                else
                {
                    roseAnimator.SetBool("Act_1", false);
                    roseAnimator.SetBool("Act_2", true);

                }

                //RoseWeaponEffectTrail.SetActive(true);

                //增加挥动武器特效
                if (roseWeaponEffectTrail == null)
                {
                    if (RoseOcc != "3")
                    {
                        roseWeaponEffectTrail = (GameObject)Instantiate(Obj_RoseWeaponEffectTrail);
                        roseWeaponEffectTrail.SetActive(false);
                        roseWeaponEffectTrail.transform.parent = RoseWeaponPosi.transform;
                        roseWeaponEffectTrail.transform.localPosition = new Vector3(-0.0036f, -0.00429f, 0.549f);
                        roseWeaponEffectTrail.transform.localScale = new Vector3(1, 1, 1);
                        roseWeaponEffectTrail.SetActive(true);
                    }
                }

                if (ZuoQiStatus)
                {
                    //ifShowZuoQi = true;
                    ZuoQiStatus = false;
                    roseAnimator.Play("Act_1");
                }

                break;
            case "4":
                roseAnimator.SetBool("CriAct", true);
                break;

            case "5":
                //Debug.Log("重击动作");
                //Debug.Log("播放技能动作1111");
                roseAnimator.SetBool(RoseNowUseSkillAnimationName, true);
                //Debug.Log("播放技能动作2222");
                roseAnimator.Play(RoseNowUseSkillAnimationName);
                break;
            //吟唱状态
            case "6":
                //Debug.Log("进入吟唱状态");
                roseAnimator.SetBool("Skill_3", true);
                break;
            case "8":
                roseAnimator.Play("Death");
                break;

            //坐骑状态
            case "9":
                roseAnimator.Play("ZuoQi");
                break;
        }
        /*
        if (openAnimator != "2") {
            runSourceTime = 0.0f;
        }
         */
        /*
        if (ifShowZuoQi == false) {
            //Debug.Log("openAnimator = " + openAnimator);
        }
        */
        if (Obj_ZuoQiShowModel != null)
        {
            Obj_ZuoQiShowModel.SetActive(ifShowZuoQi);
            if (ifShowZuoQi == false)
            {
                Obj_RoseShowModel.transform.localPosition = Vector3.zero;
                Obj_RoseShowModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                ZuoQiStatus = false;
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuffListShowSet.GetComponent<UI_BuffShowListSet>().UpdateZuoQiBuffStatus = true;
            }
        }
    }

    //更新当前角色携带被动技能的状态
    void updataPassiveSkill()
    {
        string[] passiveSkillID_Save;
        string[] passiveSkillTriggerTimeOnce_Save;
        string[] ifSkillTrigger_Save;
        //更新时先将之前的数据保存
        if (passiveSkillID == null)
        {
            string[] zeroValue = { "0" };
            passiveSkillID_Save = zeroValue;
            passiveSkillTriggerTimeOnce_Save = zeroValue;
            ifSkillTrigger_Save = zeroValue;
            //Debug.Log("开始更新被动技能1   passiveSkillID_Save = " + passiveSkillID_Save[0]);
        }
        else
        {
            passiveSkillID_Save = passiveSkillID;                                  			  //被动技能
            //string[] passiveSkillType_Save = passiveSkillType;                              //技能触发类型
            //string[] passiveSkillPro_Save = passiveSkillPro;                                //技能触发类型参数
            //string[] passiveSkillTriggerOnce_Save = passiveSkillTriggerOnce;                //技能是否只触发一次参数
            passiveSkillTriggerTimeOnce_Save = passiveSkillTriggerTimeOnce;        //技能是否只触发一次参数
            //string[] passiveSkillTriggerTime_Save = passiveSkillTriggerTime;                //技能触发时间类型
            ifSkillTrigger_Save = ifSkillTrigger;                                  //技能是否触发,当次字段的数组为1是,对应的技能将不能再次释放
            //Debug.Log("开始更新被动技能2 passiveSkillID = " + passiveSkillID[0] + "   passiveSkillID_Save = " + passiveSkillID_Save[0]);
        }

        //获取当前携带的被动技能
        string passiveSkillIDStr_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string passiveSkillIDStr_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string passiveSkillIDStr = "";
        if (passiveSkillIDStr_1 != "" && passiveSkillIDStr_2 != "")
        {
            passiveSkillIDStr = passiveSkillIDStr_1 + "," + passiveSkillIDStr_2;
        }
        if (passiveSkillIDStr_1 == "" || passiveSkillIDStr_2 == "")
        {
            passiveSkillIDStr = passiveSkillIDStr_1 + passiveSkillIDStr_2;
        }

        //获取当前携带的精灵技能(被动)
        string JingLingIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JingLingIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] jingLingIDList = JingLingIDSet.Split(';');
        for (int i = 0; i < jingLingIDList.Length; i++)
        {
            string jingLingSkillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Type", "ID", jingLingIDList[i], "Spirit_Template");
            if (jingLingSkillType == "2")
            {
                string jingLingSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkill", "ID", jingLingIDList[i], "Spirit_Template");
                if (passiveSkillIDStr != "" && jingLingSkillStr != "" && jingLingSkillStr != "0")
                {
                    passiveSkillIDStr = passiveSkillIDStr + "," + jingLingSkillStr;
                }
            }
        }

        //获取当前携带的精灵技能(主动被动)
        string jingLingEquipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JingLingEquipID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] jingLingEquipIDList = jingLingEquipID.Split(';');
        for (int i = 0; i < jingLingEquipIDList.Length; i++)
        {
            string jingLingSkillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Type", "ID", jingLingEquipIDList[i], "Spirit_Template");
            if (jingLingSkillType == "1")
            {
                string jingLingSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkill", "ID", jingLingEquipIDList[i], "Spirit_Template");
                if (passiveSkillIDStr != "" && jingLingSkillStr != "" && jingLingSkillStr != "0")
                {
                    passiveSkillIDStr = passiveSkillIDStr + "," + jingLingSkillStr;
                }
            }
        }


        //获取当前携带的觉醒技能(被动)
        string JueXingJiHuoIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] jueXingJiHuoIDList = JueXingJiHuoIDSet.Split(';');
        for (int i = 0; i < jueXingJiHuoIDList.Length; i++)
        {
            string jingLingSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddSkill", "ID", jueXingJiHuoIDList[i], "JueXing_Template");
            //获取职业
            string roseOcc = Game_PublicClassVar.Get_function_Rose.GetRoseOcc();
            string[] SkillStrList = jingLingSkillStr.Split('@');
            if (SkillStrList.Length >= 2)
            {
                switch (roseOcc)
                {
                    case "1":
                        jingLingSkillStr = SkillStrList[0];
                        break;

                    case "2":
                        jingLingSkillStr = SkillStrList[1];
                        break;
                }
            }

            if (passiveSkillIDStr != "" && jingLingSkillStr != "" && jingLingSkillStr != "0")
            {
                passiveSkillIDStr = passiveSkillIDStr + "," + jingLingSkillStr;
            }
        }


        //获取当前携带的天赋技能
        string learnTianSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (learnTianSkillID != "")
        {
            passiveSkillIDStr = passiveSkillIDStr + "," + learnTianSkillID;
        }


        //装备隐藏技能
        string writeEquipHintSkillStr = "";
        for (int i = 1; i <= 13; i++)
        {
            string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseEquip");
            if (hideID != "0")
            {
                string hideProperListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");
                string[] hideProperty = hideProperListStr.Split(';');
                //循环加入各个隐藏属性
                if (hideProperListStr != "")
                {
                    for (int y = 0; y <= hideProperty.Length - 1; y++)
                    {

                        string[] hidePropertyList = hideProperty[y].Split(',');
                        if (hidePropertyList.Length >= 2)
                        {
                            string hidePropertyType = hideProperty[y].Split(',')[0];
                            string hidePropertyValue = hideProperty[y].Split(',')[1];
                            switch (hidePropertyType)
                            {
                                //额外技能
                                case "10001":
                                    string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", hidePropertyValue, "Skill_Template");

                                    //增加被动
                                    if (skillType == "2")
                                    {
                                        if (writeEquipHintSkillStr != "")
                                        {
                                            writeEquipHintSkillStr = writeEquipHintSkillStr + "," + hidePropertyValue;
                                        }
                                        else
                                        {
                                            writeEquipHintSkillStr = hidePropertyValue;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        if (writeEquipHintSkillStr != "")
        {
            passiveSkillIDStr = passiveSkillIDStr + "," + writeEquipHintSkillStr;
            //Debug.Log("添加装备隐藏技能 writeEquipHintSkillStr = " + writeEquipHintSkillStr);
        }


        //初始化技能
        passiveSkillID = passiveSkillIDStr.Split(',');
        //再次初始化技能字符串  passiveSkillIDStr
        passiveSkillIDStr = "";
        for (int i = 0; i <= passiveSkillID.Length - 1; i++)
        {
            //获取技能类型
            if (passiveSkillID[i] != "" && passiveSkillID[i] != "0")
            {
                //Debug.Log("passiveSkillID[i] = " + passiveSkillID[i]);
                string type = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", passiveSkillID[i], "Skill_Template");
                if (type == "2")
                {
                    if (passiveSkillIDStr != "")
                    {
                        passiveSkillIDStr = passiveSkillIDStr + "," + passiveSkillID[i];
                    }
                    else
                    {
                        passiveSkillIDStr = passiveSkillIDStr + passiveSkillID[i];
                    }
                }
            }
        }

        //给被动技能赋值
        passiveSkillID = passiveSkillIDStr.Split(',');
        //Debug.Log("被动技能列表_2：" + passiveSkillIDStr);
        //获取技能的其他参数
        if (passiveSkillID[0] != "0" && passiveSkillID[0] != "")
        {
            string PassiveSkillTypeStr = "";        //技能类型参数
            string PassiveSkillProStr = "";         //技能参数
            string PassiveSkillTriggerOnceStr = "";  //技能只执行一次参数
            string PassiveSkillTriggerOnceTimeStr = "";  //技能只执行一次参数
            string PassiveSkillTriggerTimeStr = "";     //技能触发时机
            string IfPassiveSkillTrigger = "";          //技能触发状态

            for (int i = 0; i <= passiveSkillID.Length - 1; i++)
            {
                //循环获取技能数据
                string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillType", "ID", passiveSkillID[i], "Skill_Template");
                string skillPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillPro", "ID", passiveSkillID[i], "Skill_Template");
                string skillTriggerOnce = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillTriggerOnce", "ID", passiveSkillID[i], "Skill_Template");
                string skillTriggerTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillTriggerTime", "ID", passiveSkillID[i], "Skill_Template");
                PassiveSkillTypeStr = PassiveSkillTypeStr + skillType + ";";
                PassiveSkillProStr = PassiveSkillProStr + skillPro + ";";
                PassiveSkillTriggerOnceStr = PassiveSkillTriggerOnceStr + skillTriggerOnce + ";";
                PassiveSkillTriggerTimeStr = PassiveSkillTriggerTimeStr + skillTriggerTime + ";";

                bool saveStatus = false;

                //监测技能是否存在CD
                if (passiveSkillID_Save[0] != "0")
                {
                    for (int y = 0; y <= passiveSkillID_Save.Length - 1; y++)
                    {
                        if (passiveSkillID_Save[y] == passiveSkillID[i])
                        {
                            if (ifSkillTrigger_Save[y] == "1")
                            {
                                string skillCD = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", passiveSkillID[i], "Skill_Template");
                                PassiveSkillTriggerOnceTimeStr = PassiveSkillTriggerOnceTimeStr + skillCD + ";";              //默认5分钟被动技能冷却时间,后期可读配置,上面还有一处要改
                                IfPassiveSkillTrigger = IfPassiveSkillTrigger + ifSkillTrigger_Save[y] + ";";
                                saveStatus = true;
                                //Debug.Log("更新装备时,我保存了数据");
                            }
                        }
                    }
                }

                if (!saveStatus)
                {
                    //Debug.Log("passiveSkillID[i] = " + passiveSkillID[i]);
                    string skillCD = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", passiveSkillID[i], "Skill_Template");
                    PassiveSkillTriggerOnceTimeStr = PassiveSkillTriggerOnceTimeStr + skillCD + ";";              //默认5分钟被动技能冷却时间,后期可读配置,上面还有一处要改
                    IfPassiveSkillTrigger = IfPassiveSkillTrigger + "0;";
                }
            }

            passiveSkillType = PassiveSkillTypeStr.Split(';');
            passiveSkillPro = PassiveSkillProStr.Split(';');
            passiveSkillTriggerOnce = PassiveSkillTriggerOnceStr.Split(';');
            passiveSkillTriggerTime = PassiveSkillTriggerTimeStr.Split(';');
            passiveSkillTriggerTimeOnce = PassiveSkillTriggerOnceTimeStr.Split(';');
            ifSkillTrigger = IfPassiveSkillTrigger.Split(';');

        }
    }

    //等级开启要塞
    public void OpenEmenyAct()
    {
        int roseLv = this.GetComponent<Rose_Proprety>().Rose_Lv;
        if (roseLv >= 15)
        {
            Game_PublicClassVar.Get_game_PositionVar.OpenEmenyActStatus = true;
        }
    }


    //碰撞范围内调用
    void OnCollisionEnter(Collider collider)
    {

        //Debug.Log("碰撞体111：" + collider.gameObject.name);

    }

    void OnControllerColliderHit(Collider collider)
    {

        //Debug.Log("碰撞体222：" + collider.gameObject.name);

    }

    public void WeaPonEffectStar()
    {
        //销毁挥动武器特效
        if (roseWeaponEffectTrail != null)
        {
            Destroy(roseWeaponEffectTrail);
        }
    }


    public void WeaPonEffectExit()
    {
        //销毁挥动武器特效
        if (roseWeaponEffectTrail != null)
        {
            Destroy(roseWeaponEffectTrail);
        }
    }

    //创建一个攻击范围的指示圈
    public void CreateActRangEffect()
    {

        if (SkillRangeEffect != null)
        {
            SkillRangeEffect.SetActive(true);
            return;
        }

        //实例化玩家释法范围
        GameObject skillRangeEffect = (GameObject)Resources.Load("Effect/Rose/Rose_SkillRange_2", typeof(GameObject));        //实例化范围特效
        SkillRangeEffect = (GameObject)Instantiate(skillRangeEffect);
        Vector3 playerVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        SkillRangeEffect.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform);
        SkillRangeEffect.transform.localPosition = Vector3.zero;
        float sizeFloat = actDistance * 0.15f;
        SkillRangeEffect.transform.localScale = new Vector3(sizeFloat, sizeFloat, 1);

    }

    //获取攻击距离
    private void GetActDis()
    {

        //攻击距离
        actDistance = 3.0f;
        switch (RoseOcc)
        {
            //战士攻击距离
            case "1":
                actDistance = 3.0f;
                break;
            //法师攻击距离
            case "2":
                actDistance = 8.0f;
                break;
            //猎人攻击距离
            case "3":
                actDistance = 8.0f;
                break;
        }

    }

    //添加目标
    public void AddActTargetList(GameObject targetObj)
    {

        //Debug.Log("加入id：" + targetObj.GetComponent<AI_1>().AI_ID + " 加入名字：" + targetObj.name);
        if (AutomaticMonsterObjList.ContainsKey(targetObj) == false)
        {

            ActTargetList actList = new ActTargetList();
            actList.ActObj = targetObj;

            AutomaticMonsterObjList.Add(targetObj, actList);
        }

    }

    //删除目标
    public void DelActTargetList(GameObject targetObj)
    {
        if (AutomaticMonsterObjList.ContainsKey(targetObj) == true)
        {
            AutomaticMonsterObjList.Remove(targetObj);
        }

    }

}

public class ActTargetList
{
    public GameObject ActObj;
    public float Dis;
    public bool ifXuanZhong;
}
