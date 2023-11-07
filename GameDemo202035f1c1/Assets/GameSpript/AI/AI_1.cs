using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using TMPro;

//此脚本用于怪物本身的智能


public class AI_1 : MonoBehaviour {
	
	//定义AI的属性
    public string AI_ID_Only;                       //世界生成的AI的唯一ID序号
    public long AI_ID;                              //世界生成的AI的ID序号
    public ObscuredLong AI_ID_Obs;
	public ObscuredBool AI_Selected_Status;                 //是否被选中
    public ObscuredString AI_Status;                        //AI状态                      0：巡逻  1：发现目标（但不追击）2：追击 3：攻击 4：返回（攻击目标太远） 5：死亡 6：释放技能 7：跟随主角模式 8:眩晕
    public ObscuredInt AI_Type;                             //AI的类型                    0或空: 表示普通怪物   1：表示宝宝   2：表示变异宝宝
    public ObscuredString AI_PetID;                         //宠物ID
    public ObscuredInt AI_BuZhuoNum;                        //当前宠物捕捉次数
    public GameObject AI_Target;                            //当前怪物目前注视的目标
    public GameObject Obj_AIModel;                          //AI模型
    public SkinnedMeshRenderer ModelMesh;                   //AI材质
    private ObscuredFloat hitMeshTime;                      //受击播放材质时间
    private ObscuredBool hitMeshStatus;                     //受击播放材质状态
    public ObscuredBool IfRosePet;                          //当前AI是否为玩家的召唤的宠物
    public GameObject MonsterCreateObj;             //怪物召唤的父级
    public ObscuredBool AIStopMoveStatus;                   //怪物移动状态
    public ObscuredBool AIStopLookTargetStatus;             //AI停止看目标状态
    public ObscuredBool IfDeathDestoryModel;                //怪物死亡时是否立即销毁尸体
    public ObscuredString CreateEWaoDropID;                 //额外的掉落ID

    //AI巡逻
    private ObscuredBool ai_PatrolStatus;                   //AI是否在巡逻
    private ObscuredBool ai_FindNextPatrol;                 //AI是否寻找下一个巡逻点
    private ObscuredFloat ai_WalkSpeed;                     //AI走路速度（用于巡逻）
    private ObscuredFloat ai_PatrolRange;                   //AI巡逻范围
    private ObscuredFloat ai_PatrolRestTime;                //AI巡逻到达指定地点休息时间
    private ObscuredFloat ai_PatrolGuardTime;               //AI巡逻保护时间
    private ObscuredFloat ai_actRunRange;                   //怪物攻击范围
    private Vector3 ai_StarPosition;                        //AI出生坐标点
    public UnityEngine.AI.NavMeshAgent ai_NavMesh;          //AI自动寻路控制器
    private Vector3 targetPosition;                         //目标的坐标点
    private Vector3 walkPosition;                           //巡逻移动的目标点
    public ObscuredFloat ai_chaseRange;                     //AI追击范围
    //private float ai_PartolTime;                          //AI两次寻路间隔时间;
    private ObscuredBool actRoseStatus;                     //攻击角色状态
    private Vector3 aiPosi_Last;                            //ai上一次的位置
	
	
	//AI属性声明
	private ObscuredString AI_Name;
	private ObscuredInt  AI_Hp_Max;                         //AI血量上限
    public ObscuredFloat AI_MoveSpeed;                      //怪物移动速度
    private ObscuredFloat AI_MoveSpeedBase;                      //怪物移动速度
    private ObscuredBool AI_IfWalk;                         //怪物是否来回走
    private ObscuredBool ai_IfReturn;                       //开启后表示怪事是在超出自身距离后返回
    private ObscuredBool ai_IfChase;                        //当前ai是否处于追击状态
    private ObscuredFloat ai_ActDistance;                   //当前ai攻击距离
    public ObscuredString ai_monsterType;                   //ai类型
    private ObscuredFloat actSpeed;                         //攻击速度
    private ObscuredFloat actSpped_Sum;                     //攻击速度统计值（累加值,用来判定当前时间是否满足下次攻击）
    private ObscuredBool NextActStatus;                     //下一次攻击状态,如果为true表示可以立即进行下一次攻击
    private ObscuredBool ActStatus;                         //攻击状态
    public ObscuredFloat monsterRebirthTime;                //怪物复活时间（单位/秒）
    public ObscuredFloat monsterReirthTimeSum;              //怪物复活时间累计值 
    public ObscuredFloat monsterOffLineTime;
    public ObscuredBool ai_IfDeath;                         //怪物是否死亡
    private ObscuredBool ai_IfDeathTimeStatus;              //怪物是为死亡复活状态
    public ObscuredFloat monsterDestoryTime;                //怪物销毁模型时间
    //private float ai_DeathTimeSum;                        //怪物死亡复活时间累计
    private string[] skillID;                               //怪物自身的技能
    private string[] passiveSkillType;                      //技能触发类型
    private string[] passiveSkillPro;                       //技能触发类型参数
    private string[] passiveSkillTriggerOnce;               //技能是否只触发一次参数
    private string[] passiveSkillTriggerTime;               //技能触发时间类型
    private string[] ifSkillTrigger;                        //技能是否触发,当次字段的数组为1是,对应的技能将不能再次释放
    private string[] passiveSkillTriggerTimeCD;             //技能CD
    private string[] passiveSkillTriggerTimeCDSum;          //技能CD计时
    private ObscuredBool triggerSkillStatus;                //怪物触发技能状态
    private ObscuredString triggerSkillID;                  //怪物触发技能ID
    //private bool actStatus;                               //每次攻击打开此状态判定是否释放技能
    private ObscuredFloat passiveSkillTriggerTimeSum;       //在Updata里每隔多少时间监测一次是否触发被动
    public ObscuredBool AI_UpdateProStatus;

	//当前AI变量
	public ObscuredFloat AI_Distance;                       //AI距离目标的距离
    private ObscuredFloat AI_RoseDis;                       //AI距离角色的距离
    private ObscuredBool AI_Hp_Status;                      //如果当前AI显示血条，则为True
    private ObscuredBool AI_MonsterDeathUIStatus;           //怪物死亡复活状态
    public ObscuredBool AI_MonsterDeathStatus;     //怪物死亡复活状态
	public GameObject Selected_Effect;              //选中特效
	public Transform Selected_Effect_Position;      //选中特效坐标点
	private ObscuredBool Selected_Effect_Status;    //特效是否已经开启
	private GameObject effect;                      //当前实例化的光圈特效
	public Transform Ai_HpShowPosition;             //当前显示血条的坐标点
    private Transform ai_DeathTimePosition;         //怪物复活UI坐标点
	public GameObject UI_GameObject;                //UI的GameObject
    public GameObject UI_Hp;                        //AI血条的UI位置
    private GameObject monsterDeathTimeObj;         //AI死亡复活时间显示UI
	private AI_Property ai_property;                //AI当前属性
	private ObscuredBool MaxHp_Status;              //是否取到最大生命值


    public ObscuredBool HitStatus;                          //是否受到伤害
	public ObscuredInt HitDamge;                            //受到的伤害值
    public ObscuredBool HitStatus_LuanShe;                  //收到乱射伤害
    private float HitStatus_LuanSheTime;
	//public string AIHitText;						//AI受击飘的文字
	//public string AIHitTextlater;					//AI受击飘的最后文字
	public GameObject HitObject;                    //伤害飘字的显示UI
    public ObscuredBool HitCriStatus;                       //是否受到暴击伤害
    public ObscuredBool ZhongJiStatus;                      //是否受到重击伤害
    public ObscuredBool ActReboundStatus;                   //物理反击
    public ObscuredBool MageReboundStatus;                  //法术反击
	private ObscuredInt LastHp;                             //上一次的生命值,飘字用的
    public ObscuredBool IfHitEffect;                        //是否播放受伤特效
    public ObscuredBool IfDodge;                            //是否受到闪避
    public GameObject HitEffect;                    //受击特效
    public GameObject HitEffectt_Position;          //受击特效播放点
    public GameObject BoneSet;                      //骨骼绑点
    private AI_Status ai_status;                    //AI状态脚本
    private Rose_Status rose_status;
    private Rose_Proprety roseProprety;
    private Game_PositionVar game_positionVar;
    private GameObject obj_Rose;                    //主角的Obj
    private ObscuredBool SkillStatus;
    //public bool petMoveStatus;                    //宠物跟随主角移动状态
    public ObscuredFloat StopMoveTime;
    public ObscuredFloat StopMoveTimeSum;
    public ObscuredFloat AIStopLookTargetTime;
    public ObscuredFloat AIStopLookTargetTimeSum;
    private GameObject Obj_TuoZhanShow;             //脱战范围显示
    private ObscuredFloat fightTimeSum;                     //战斗时间
    private ObscuredBool fightStatus;                       //战斗状态
    //public float AIStop

    public ObscuredBool XuanYunStatus;                      //眩晕状态
    public ObscuredFloat XuanYunTime;                       //眩晕时间
    public ObscuredFloat xuanYunTimeSum;                    //眩晕时间累计
    private ObscuredBool XuanYunFlyTextStatus;              //眩晕飘字状态

    public ObscuredBool ChenMoStatus;
    public ObscuredFloat ChenMoTime;                       //沉默时间
    public ObscuredFloat ChenMoTimeSum;                    //沉默时间累计
    //private Animator animator;

    //为了无限刷怪的BUG,修正掉落
    public ObscuredBool dropStatus;
    private ObscuredFloat dropNum;
    private ObscuredFloat dropNumSum;

    // Use this for initialization
    void Start () {

        AI_ID_Obs = AI_ID;

        //获取怪物出现概率
        float monsterShowPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterShowPro", "ID", AI_ID.ToString(), "Monster_Template"));
        //float monsterShowPro = 0f;
        if (Random.value > monsterShowPro) {
            //Debug.Log("删除了怪 AI_ID = " + AI_ID);
            //Destroy(this.gameObject);
        }

        //初始化玩家属性
        roseProprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        rose_status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        game_positionVar = Game_PublicClassVar.Get_game_PositionVar;
        obj_Rose = game_positionVar.Obj_Rose;


        //初始化AI的UI显示
        ai_property = this.gameObject.GetComponent<AI_Property>();
        ai_property.AI_Type = "1";              //设置属性为怪物属性
        LastHp = ai_property.AI_Hp;             //显示AI血量

        //初始化AI巡逻机制
        ai_PatrolStatus = true;     //开启AI巡逻
        ai_FindNextPatrol = true;   //开启AI下一次攻击
        ai_StarPosition = this.gameObject.transform.position;       //获取AI的出生点
        ai_NavMesh = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        ai_status = this.gameObject.GetComponent<AI_Status>();
        ai_status.AI_StatusValue = 0;

        //设置怪物穿透
        ai_NavMesh.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;


        //初始化怪物属性
        AI_MoveSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MoveSpeed", "ID", AI_ID.ToString(), "Monster_Template"));
        AI_MoveSpeedBase = AI_MoveSpeed;
        //怪物走路速度
        ai_WalkSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("WalkSpeed", "ID", AI_ID.ToString(), "Monster_Template"));             
        //设置攻击速度
        actSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActSpeed", "ID", AI_ID.ToString(), "Monster_Template"));
        //设置攻击距离  （远程单位不要超过10.0f,如果想改成超过10.0f需要手动修改追击10.0f的值才会触发）
        ai_ActDistance = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActDistance", "ID", AI_ID.ToString(), "Monster_Template"));
        //设置追击范围
        ai_chaseRange = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChaseRange", "ID", AI_ID.ToString(), "Monster_Template"));
        //ai_chaseRange = 5.0f;
        ai_actRunRange = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActRunRange", "ID", AI_ID.ToString(), "Monster_Template"));
        //赋值巡逻范围
        ai_PatrolRange = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PatrolRange", "ID", AI_ID.ToString(), "Monster_Template"));
        ai_monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");

        //设定怪物复活时间
        monsterRebirthTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ReviveTime", "ID", AI_ID.ToString(), "Monster_Template"));
        monsterOffLineTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffLineReviveTime", "ID", AI_ID.ToString(), "Monster_Template"));

        //monsterRebirthTime = 5.0f;
        //monsterReirthTimeSum = Game_PublicClassVar.Get_function_AI.GetMonsterDeathTime(AI_ID_Only);

        //防止无限刷怪BUG,强制两次掉落时间
        dropStatus = false;
        dropNum = 1800;     //为防止下面执行错误等于0
        dropNum = monsterRebirthTime/2;

        if (AI_ID_Only != "" && AI_ID_Only != "0") {
            //更新在线刷新时间
            float rebirthTime = Game_PublicClassVar.Get_function_AI.GetMonsterDeathTime(AI_ID_Only);
            if (rebirthTime != 0) {
                monsterRebirthTime = rebirthTime;
            }
            //更新离线刷新时间
            float offLineTime = Game_PublicClassVar.Get_function_AI.GetMonsterDeathOffLineTime(AI_ID_Only);
            if (offLineTime != 0) {
                monsterOffLineTime = offLineTime;
            }
        }
        //monsterRebirthTime = 60.0f;
        //获取怪物技能
        skillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", AI_ID.ToString(), "Monster_Template").Split(';');

        //获取技能的其他参数
        if (skillID[0] != "0")
        {
            string PassiveSkillTypeStr = "";        //技能类型参数
            string PassiveSkillProStr = "";         //技能参数
            string PassiveSkillTriggerOnceStr = "";  //技能只执行一次参数
            string PassiveSkillTriggerTimeStr = "";     //技能触发时机
            string IfPassiveSkillTrigger = "";          //技能触发状态
            string passiveSkillTriggerTimeCDStr = "";          //技能触发状态
            string passiveSkillTriggerTimeCDSumStr = "";          //技能触发状态
            for (int i = 0; i <= skillID.Length - 1; i++)
            {
                string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillType", "ID", skillID[i], "Skill_Template");
                string skillPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillPro", "ID", skillID[i], "Skill_Template");
                string skillTriggerOnce = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillTriggerOnce", "ID", skillID[i], "Skill_Template");
                string skillTriggerTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillTriggerTime", "ID", skillID[i], "Skill_Template");
                string skillTriggerTimeCD = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", skillID[i], "Skill_Template");
                PassiveSkillTypeStr = PassiveSkillTypeStr + skillType + ";";
                PassiveSkillProStr = PassiveSkillProStr + skillPro + ";";
                PassiveSkillTriggerOnceStr = PassiveSkillTriggerOnceStr + skillTriggerOnce + ";";
                PassiveSkillTriggerTimeStr = PassiveSkillTriggerTimeStr + skillTriggerTime + ";";
                IfPassiveSkillTrigger = IfPassiveSkillTrigger + "0;";
                passiveSkillTriggerTimeCDStr = passiveSkillTriggerTimeCDStr + skillTriggerTimeCD + ";";
                passiveSkillTriggerTimeCDSumStr = passiveSkillTriggerTimeCDSumStr + "0;";
            }
            passiveSkillType = PassiveSkillTypeStr.Split(';');
            passiveSkillPro = PassiveSkillProStr.Split(';');
            passiveSkillTriggerOnce = PassiveSkillTriggerOnceStr.Split(';');
            passiveSkillTriggerTime = PassiveSkillTriggerTimeStr.Split(';');
            ifSkillTrigger = IfPassiveSkillTrigger.Split(';');
            passiveSkillTriggerTimeCD = passiveSkillTriggerTimeCDStr.Split(';');
            passiveSkillTriggerTimeCDSum = passiveSkillTriggerTimeCDSumStr.Split(';');

        }

        //初始化怪物默认目标
        if (AI_Target == null) {
            AI_Target = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;
        }



        //怪物复活时间存储
        /*
        if (AI_ID_Only == "1000301") {
            string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            //Debug.Log("deathMonsterIDListStr = " + deathMonsterIDListStr);
        }
        */


        float deathTimeNow = Game_PublicClassVar.Get_function_AI.GetMonsterDeathTime(AI_ID_Only);
        if (deathTimeNow != 0) {
            //this.gameObject.SetActive(false);
            //Debug.Log("怪物设置为死亡状态!!!" + AI_ID.ToString());
            this.GetComponent<AI_Property>().AI_Hp = 0;
            //Debug.Log("怪物设置为死亡状态!!!血量" + this.GetComponent<AI_Property>().AI_Hp);
            AI_MonsterDeathStatus = true;
            //AI_MonsterDeathUIStatus = true;
            //ai_IfDeath = true;
            //showMonsterDeathTimeUI();       //怪物复活时间显示
        }

        //ai_DeathTimePosition = BoneSet.transform.Find("Center").transform;

        //在狩猎塔不进行召唤召唤类型的怪物  100006
        if (Application.loadedLevelName == "100006" || Application.loadedLevelName == "100007")
        {
            string monsterTypeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", AI_ID.ToString(), "Monster_Template");
            if (monsterTypeStr == "4")
            {
                //Debug.Log("创建怪物销毁啦!!! 地图id:" + Application.loadedLevelName);
                Destroy(this.gameObject);
            }
        }

        //更新属性
        AI_UpdateProStatus = true;

        //设置自身离寻路最近的坐标点,要不离地面检测区太远会报错
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(this.gameObject.transform.position, out hit, 10.0f, 1);
        try
        {
            //Debug.Log("hit = " + hit.position);
            ai_NavMesh.Warp(hit.position);
            walkPosition.y = hit.position.y;
            ai_NavMesh.SetDestination(walkPosition);
            //设置AI的初始坐标，要不返回时会报错
            ai_StarPosition = this.transform.position;
        }
        catch
        {
            //Debug.Log("移动报错！怪物ID：" + AI_ID + "name = " + AI_Name);
        }

        if (ai_NavMesh.isOnNavMesh == false)
        {
            string monsterTypeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", AI_ID.ToString(), "Monster_Template");
            if (monsterTypeStr == "4") {
                //Debug.LogError("创建怪物销毁啦!!!" + AI_ID +  " 地图id:" + Application.loadedLevelName);
                Destroy(this.gameObject);
            }
        }

        string enterMapBaby = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EnterMapBaby", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (enterMapBaby == "")
        {
            enterMapBaby = "0";
        }

        //检测自身是否为宝宝(一定时间内进入游戏12次以后才有可能出现宝宝和变异)
        if (int.Parse(enterMapBaby) <= 12) {
            ifbaby();
        }

        //设置黑底的位置
        //隐藏阴影底
        try
        {
            if (Obj_AIModel.transform.Find("BackDi") != null)
            {
                GameObject backObj = Obj_AIModel.transform.Find("BackDi").gameObject;
                if (backObj != null)
                {
                    backObj.transform.position = BoneSet.transform.Find("Di").transform.position;
                }
            }
        }
        catch {
            Debug.Log("底图未找到！");
        }

        fightTimeSum = 0;   //设置战斗时间

    }
	
	// Update is called once per frame
	void Update () {


        //防止刷怪BUG做的掉落检测
        if (dropStatus) {
            dropNumSum = dropNumSum + Time.deltaTime;
            if(dropNumSum>=dropNum){
                dropStatus = false;
                dropNumSum = 0;
            }
        }

        //设置AI选中状态
        if (game_positionVar.UpdataSelectedStatus) {
        //if (Game_PublicClassVar.Get_game_PositionVar.UpdataSelectedStatus)
            if (rose_status.Obj_ActTarget != null)
            {
                if (this.gameObject!= rose_status.Obj_ActTarget)
                {
                    AI_Selected_Status = false;
                }
            }
            else
            {
                AI_Selected_Status = false;
            }
        }
        
        //怪物停止
        if (AIStopMoveStatus) {
            //ai_NavMesh.ResetPath();     //清理寻路,停止移动
            AI_MoveSpeed = 0;
            StopMoveTimeSum = StopMoveTimeSum + Time.deltaTime;
            if (StopMoveTimeSum >= StopMoveTime) {
                AIStopMoveStatus = false;
                //AI_MoveSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MoveSpeed", "ID", AI_ID.ToString(), "Monster_Template"));
                AI_MoveSpeed = AI_MoveSpeedBase;
            }
        }

        //怪物停止注释
        if (AIStopLookTargetStatus) {
            AIStopLookTargetTimeSum = AIStopLookTargetTimeSum + Time.deltaTime;
            if (AIStopLookTargetTimeSum >= AIStopLookTargetTime) {
                //清空数据
                AIStopLookTargetStatus = false;
                AIStopLookTargetTimeSum = 0;
                AIStopLookTargetTime = 0;
            }
        }

        //控制AI的攻击间隔,当AI刚攻击结束一次后必须满足下次攻击状态开启时才能追击目标
        actSpped_Sum = actSpped_Sum + Time.deltaTime;

        if (actSpped_Sum >= actSpeed/2)
        {
            NextActStatus = true;
        }

        //特殊受击
        if (HitStatus_LuanShe) {
            HitStatus_LuanSheTime = HitStatus_LuanSheTime + Time.deltaTime;
            if (HitStatus_LuanSheTime >= 0.1f) {
                HitStatus_LuanSheTime = 0;
                HitStatus_LuanShe = false;
            }
        }

        
        //-------------------------------------------------设置AI状态-----------------------------------------------

        if (AI_Target != null) {
            if (AI_Target.active == false)
            {
                AI_Target = null;
            }
        }

        //初始化怪物默认目标
        if (AI_Target == null)
        {
            AI_Target = obj_Rose;
        }
        else {
            //攻击角色
            if (AI_Target.layer == 14)
            {
                //获取目标是否死亡
            }
            //攻击宠物
            if (AI_Target.layer == 18)
            {
                //获取目标是否死亡
                if (AI_Target.GetComponent<AIPet>().AI_Status == "5") {
                    AI_Target = obj_Rose;
                }
            }
        }
        
        //判定角色离AI的距离
        targetPosition = AI_Target.gameObject.transform.position;
        AI_Distance = Vector3.Distance(targetPosition, this.gameObject.transform.position);
        
        //获取角色当前自动攻击
        if (ai_property.AI_Hp >0)
        {
            
            float AI_RoseDis = Vector3.Distance(obj_Rose.transform.position, this.gameObject.transform.position);
            float roseDis = rose_status.NextAutomaticMonsterDis;
            if (rose_status.NextAutomaticMonsterObj == null)
            {
                rose_status.NextAutomaticMonsterObj = this.gameObject;
                rose_status.NextAutomaticMonsterDis = AI_RoseDis;
            }

            if (rose_status.NextAutomaticMonsterObj == this.gameObject) {
                rose_status.NextAutomaticMonsterDis = AI_RoseDis;
            }
            
            if (AI_RoseDis <= roseDis)
            {
                //获取当前职业,魔法师职业不自动切换最近目标
                string roseOcc = rose_status.RoseOcc;
                switch (roseOcc)
                {
                    //战士攻击最近目标
                    case "1":
                        rose_status.NextAutomaticMonsterObj = this.gameObject;
                        rose_status.NextAutomaticMonsterDis = AI_RoseDis;
                        break;
                    //魔法师不执行任何操作
                    case "2":
                        rose_status.NextAutomaticMonsterObj = this.gameObject;
                        rose_status.NextAutomaticMonsterDis = AI_RoseDis;
                        break;
                    //猎人不执行任何操作
                    case "3":
                        rose_status.NextAutomaticMonsterObj = this.gameObject;
                        rose_status.NextAutomaticMonsterDis = AI_RoseDis;
                        break;
                }

            }
            
            //切换目标的距离,如果根据职业 修改此值即可
            if (AI_RoseDis <= 20.0) {

                //加入到攻击列表中
                rose_status.AddActTargetList(this.gameObject);
            }
            else
            {
                //移除攻击列表
                rose_status.DelActTargetList(this.gameObject);
            }
        }

        //装备技能额外
        float roseWeiZhuang = roseProprety.Rose_WeiZhuang;
        float now_aichaseRange = ai_chaseRange + roseWeiZhuang;
        //Debug.Log("AI_Distance = " + AI_Distance);
        //与目标点的距离大于ai_chaseRange时,判定为处于巡逻状态
        if (AI_Distance >= now_aichaseRange + 2.0f)   //加2.0是表示怪物在追击范围+0.2的范围内是要注释玩家的
        {
            AI_Status = "0";        //设置为巡逻
            //判定怪物是否为玩家的宠物
            if (IfRosePet)
            {
                AI_Status = "7";            //设定怪物为宠物
            }
        }
        else {

            //获取目标是否为隐身状态
            bool ifInvisible = false;
            if (AI_Target == obj_Rose) {
                ifInvisible = rose_status.RoseInvisibleStatus;
            }

            //判定角色是否隐身
            if (!ifInvisible)
            {
                AI_Status = "1";        //设置为注视目标

                if (AI_Distance < ai_chaseRange + roseWeiZhuang)
                {
                    AI_Status = "2";    //设置为追击模式
                    ai_IfChase = true;

                    if (AI_Distance <= ai_ActDistance)
                    {
                        AI_Status = "3";    //设置为攻击模式
                    }

                    //获取目标是否已经死亡
                    if (rose_status.RoseDeathStatus)
                    {
                        AI_Status = "0";        //设置为巡逻
                    }
                }
            }
            else {
                if (AI_Status == "2" || AI_Status == "3")
                {
                    //设置为返回状态
                    ai_IfReturn = true;
                }
                //如果目标为隐身状态,则设置自身为巡逻状态
                AI_Status = "0";        //设置为巡逻
            }
        }
        
        //如果受到攻击则直接开启追击模式
        if (HitStatus) {
            ai_IfChase = true;
        }

        //打开追击模式
        if (ai_IfChase) {
            if (AI_Status == "0") {
                AI_Status = "2";
                //获取目标是否已经死亡
                if (rose_status.RoseDeathStatus)
                {
                    AI_Status = "0";        //设置为巡逻
                }
            }
            if (AI_Status == "1") {
                AI_Status = "2";
            }
        }
        
        //每隔1秒监测一次被动技能
        passiveSkillTriggerTimeSum = passiveSkillTriggerTimeSum + Time.deltaTime;
        if (passiveSkillTriggerTimeSum >= 1.0f) {
            //怪物未死亡
            if (!ai_IfDeath)
            {
                actTriggerSkill("1", "2");       //尝试触发被动技能
            }

            //更新被动技能CD
            if (skillID[0] != "0") {
                for (int i = 0; i <= skillID.Length - 1; i++)
                {
                    if (passiveSkillTriggerOnce[i] == "0")
                    {
                        //更新CD
                        if (passiveSkillTriggerTimeCD[i] != "0")
                        {
                            float value = float.Parse(passiveSkillTriggerTimeCDSum[i]) + passiveSkillTriggerTimeSum;
                            passiveSkillTriggerTimeCDSum[i] = value.ToString();
                            //清空CD
                            //Debug.Log("更新被动技能时间:" + value.ToString());
                            if (value > float.Parse(passiveSkillTriggerTimeCD[i]))
                            {
                                //Debug.Log("清空技能CD,i = " + i);
                                ifSkillTrigger[i] = "0";
                                passiveSkillTriggerTimeCDSum[i] = "0";
                            }
                        }
                    }
                }
            }
            //清空累计数
            passiveSkillTriggerTimeSum = 0;
        }

        //怪物超过出生地一定距离后返回出生地
        float safetyDistance = Vector3.Distance(this.gameObject.transform.position, ai_StarPosition);
        float returnDis = ai_actRunRange + roseProprety.Rose_AITuoZhanDisValue;
        //血量低于90% 返回场景为2倍
        if (ai_monsterType != "3") {
            //if (ai_property.AI_HpMax != 0) {
                if (ai_property.AI_Hp / ai_property.AI_HpMax <= 0.9f)
                {
                    returnDis = returnDis * 2;
                }
            //}
        }

        if (safetyDistance >= returnDis)
        {
            //判定是否为宠物
            if (!IfRosePet) {
                AI_Status = "4";
                ai_IfReturn = true; //打开返回开关强制返回
            }
        }

        if (ai_IfReturn) {
            AI_Status = "4";
            //当自身的坐标点和出生点一致时,不再强制等于返回状态
            if (safetyDistance < 1.0f) {
                ai_IfReturn = false;
                ai_IfChase = false;
                //AI_Status = "0";
            }
            //血量恢复至最大生命
            ai_property.AI_Hp = AI_Hp_Max;
        }

        //释放状态
        if (SkillStatus) {
            AI_Status = "6";
            SkillStatus = false;
        }

        //眩晕状态,不执行任和操作
        if (XuanYunStatus)
        {
            //判定当前是否是返回状态
            if (AI_Status != "4")
            {
                //眩晕飘字
                if (!XuanYunFlyTextStatus)
                {
                    XuanYunFlyTextStatus = true;
                    Game_PublicClassVar.Get_function_UI.SpecialFlyText(HitObject, UI_Hp, "1");
                }

                xuanYunTimeSum = xuanYunTimeSum + Time.deltaTime;
                if (xuanYunTimeSum >= XuanYunTime)
                {
                    XuanYunStatus = false;
                    xuanYunTimeSum = 0;
                    //Debug.Log(AI_Name + "正在眩晕中");
                    XuanYunFlyTextStatus = false;       //眩晕完毕 修正飘字状态
                    //return;
                }
                AI_Status = "8";
            }
            else {
                XuanYunStatus = false;
            }

            //return;
        }

        //沉默状态
        if (ChenMoStatus) {
            ChenMoTimeSum = ChenMoTimeSum + Time.deltaTime;
            if (ChenMoTimeSum>=ChenMoTime) {
                ChenMoTimeSum = 0;
                ChenMoStatus = false;
            }
        }

        //获取持久化数据
        /*
        if (AI_ID_Only != "0")
        {
            if (PlayerPrefs.GetInt(AI_ID_Only) != 0)
            {
                ai_property.AI_Hp = 0;
                //更新死亡复活时间

            }
        }
        */

        //如果生命低于0,判定为死亡
        
        if (ai_property.AI_Hp <= 0)
        {
            AI_Status = "5";
        }

        //ai_NavMesh.speed = ai_WalkSpeed;
        
        //根据不同的AI状态触发不同的AI机制
        switch (AI_Status) { 

            //巡逻
            case "0":
                
                ai_IfDeath = false; //重置死亡状态
                fightStatus = false;
                actRoseStatus = false;

                if (rose_status.RoseFightStatus == false)
                {
                    fightTimeSum = 0;   //设置战斗时间
                }

                //获取一个巡逻目标点
                if (ai_FindNextPatrol)
                {
                    //随机一个范围
                    float random_x = (Random.value - 0.5f) * ai_PatrolRange * 2;
                    float random_z = (Random.value - 0.5f) * ai_PatrolRange * 2;
                    walkPosition = new Vector3(ai_StarPosition.x + random_x, ai_StarPosition.y, ai_StarPosition.z + random_z);
                    //注视目标
                    transform.LookAt(new Vector3(walkPosition.x, transform.position.y, walkPosition.z));
                    //移动目标区域
                    ai_NavMesh.speed = ai_WalkSpeed;

                    //设置自身离寻路最近的坐标点,要不离地面检测区太远会报错

                    UnityEngine.AI.NavMeshHit hit;
                    UnityEngine.AI.NavMesh.SamplePosition(this.gameObject.transform.position, out hit, 10.0f, 1);
                    try
                    {
                        //if (ai_NavMesh.hasPath) { }
                        ai_NavMesh.Warp(hit.position);
                        walkPosition.y = hit.position.y;
                        ai_NavMesh.SetDestination(walkPosition);
                    }
                    catch
                    {
                        //Debug.Log("移动报错！怪物ID：" + AI_ID + "name = " + AI_Name);
                    }

                    ai_FindNextPatrol = false;
                    //设置AI状态&播放对应动作
                    ai_status.IfUpdateStatus = true;
                    ai_status.AI_StatusValue = 1;
                    aiPosi_Last = this.gameObject.transform.position;

                }

                //获取自己与目标点的距离(抛弃Y轴的高低差)
                Vector3 dis_1 = new Vector3(walkPosition.x, 0, walkPosition.z);
                Vector3 dis_2 = new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z);
                float distance = Vector3.Distance(dis_1, dis_2);

                //到达目标点,休息3秒
                if (distance <= 0.25f)
                {
                    //休息时间累积
                    ai_PatrolRestTime = ai_PatrolRestTime + Time.deltaTime;
                    //设置AI状态&播放对应动作
                    ai_status.IfUpdateStatus = true;
                    ai_status.AI_StatusValue = 0;
                    //ai_NavMesh.speed = 0;
                    ai_PatrolGuardTime = 0.0f;

                    if (ai_PatrolRestTime >= 3.0f)
                    {
                        //清空数据
                        ai_FindNextPatrol = true;
                        ai_PatrolRestTime = 0.0f;
                    }
                }
                else
                {
                    //移动保护时间
                    ai_PatrolGuardTime = ai_PatrolGuardTime + Time.deltaTime;
                }

                //每次移动超过10秒再次寻找下一个巡逻目标点
                if (ai_PatrolGuardTime >= 10.0f)
                {
                    ai_FindNextPatrol = true;
                    ai_PatrolGuardTime = 0.0f;
                }

                //防止原地踏步,强制休息 0.2秒内还是在原地 表示寻路地点无法到达
                if (ai_PatrolGuardTime >= 0.2f)
                {
                    if (aiPosi_Last == this.gameObject.transform.position)
                    {
                        //Debug.Log("原地踏步！");
                        //distance = 0;
                        ai_FindNextPatrol = true;
                        ai_PatrolGuardTime = 0.0f;
                    }
                }
                
                break;

            //注视
            case "1":
                    
                //旋转到主角
                if (ai_property != null)
                {
                    //检测自身是否已经死亡
                    if (ai_property.AI_Hp > 0)
                    {
                        //注视目标
                        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
                        ai_NavMesh.SetDestination(this.transform.position);
                        //ai_NavMesh.speed = 0;
                        ai_status.IfUpdateStatus = true;
                        ai_status.AI_StatusValue = 0;
                    }
                }

            break;

            //追击
            case "2":

                //注视目标
                if (!AIStopLookTargetStatus)
                {
                    transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
                
                //if (this.gameObject.GetComponent<AI_NormalAct>().NextActStatus)
                    if (NextActStatus)
                    {
                        //停止巡逻
                        ai_PatrolStatus = false;
                        ai_NavMesh.speed = AI_MoveSpeed;
                        ai_NavMesh.SetDestination(AI_Target.gameObject.transform.position);
                        //更新动作
                        ai_status.IfUpdateStatus = true;
                        ai_status.AI_StatusValue = 2;
                    }
                }

                //怪物攻击角色时,进入战斗状态
                if (AI_Target == Game_PublicClassVar.Get_game_PositionVar.Obj_Rose)
                {
                    if (!rose_status.RoseFightStatus)
                    {
                        rose_status.RoseFightStatus = true;
                    }
                }

                //角色进入战斗状态
                addMonsterActRoseList();

                //显示范围
                TuoZhan_Show();

            break;
            //普通攻击
            case "3":

                ai_NavMesh.ResetPath(); //停止移动目标
                //注视目标
                if (!AIStopLookTargetStatus) {
                    transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
                }
                
                //当与主角距离少于3时，开始触发普通攻击
                if (AI_Distance <= ai_ActDistance)
                {
                    AI_Property aiproperty = GetComponent<AI_Property>();
                    if (aiproperty != null)
                    {
                        //Rose_Proprety roseproperty = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
                        //AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                        //判定是否满足攻击间隔时间
                        if (actSpped_Sum >= actSpeed)
                        {
                            ActStatus = true;
                            actSpped_Sum = 0.0f;
                            NextActStatus = false;
                            //actStatus = true;
                        }

                        //开启攻击状态
                        if (ActStatus)
                        {
                            //设置AI为攻击状态
                            this.gameObject.GetComponent<AI_Status>().AI_StatusValue = 3;
                            this.gameObject.GetComponent<AI_Status>().IfUpdateStatus = true;

                            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseIfHit = true;      //播放受击特效
                            //Game_PublicClassVar.Get_fight_Formult.MonsterActRose("60020001", this.gameObject);           //战斗
                            ActStatus = false;

                        }
                    }
                }

                //怪物攻击角色时,进入战斗状态
                if (AI_Target == Game_PublicClassVar.Get_game_PositionVar.Obj_Rose)
                {
                    if (!Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightStatus)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightStatus = true;
                    }
                }

                //角色进入战斗状态
                addMonsterActRoseList();

            break;
            //返回
            case "4":

                //获取目标是否为隐身状态
                bool ifInvisible = false;
                if (AI_Target == Game_PublicClassVar.Get_game_PositionVar.Obj_Rose) {
                    ifInvisible = AI_Target.GetComponent<Rose_Status>().RoseInvisibleStatus;
                }
                //当距离出生地30码时,怪物自动返回营地
                if (safetyDistance >= 15.0f || ifInvisible == true)
                {
                    //注视目标
                    transform.LookAt(new Vector3(ai_StarPosition.x, transform.position.y, ai_StarPosition.z));
                    ai_NavMesh.speed = AI_MoveSpeed;
                    ai_NavMesh.SetDestination(ai_StarPosition);

                    //设置AI状态&播放对应动作
                    ai_status.IfUpdateStatus = true;
                    ai_status.AI_StatusValue = 2;

                    ai_FindNextPatrol = true;   //将巡逻状态打开

                    //重置自身技能
                    initialSkill();

                    //退出攻击角色列表
                    costMonsterActRoseList();

                    actRoseStatus = false;

                    //销毁脱战光圈
                    TuoZhan_YinCang();
                }

                break;

            //死亡
            case "5":

                if (!ai_IfDeath) {

                    //隐藏范围显示
                    //TuoZhan_Show();

                    //移除攻击列表
                    rose_status.DelActTargetList(this.gameObject);

                    //退出攻击角色列表
                    costMonsterActRoseList();
                    actRoseStatus = false;

                    monsterDestoryTime = 5.0f;        //设置怪物初始销毁模型时间
                    //清空角色自动攻击的目标
                    if (rose_status.NextAutomaticMonsterObj == this.gameObject) {
                        rose_status.NextAutomaticMonsterObj = null;
                        rose_status.NextAutomaticMonsterDis = 0;
                    }

                    //隐藏碰撞体
                    this.GetComponent<CharacterController>().enabled = false;
                    ai_IfDeath = true;
                    ai_status.IfUpdateStatus = true;
                    ai_status.AI_StatusValue = 6;
                    ai_NavMesh.height = 0;  //隐藏碰撞高

                    //每次死亡默认刷新一下当前宠物的属性
                    GetComponent<AI_Property>().UpdateMonsterType();

                    //清空捕捉次数
                    AI_BuZhuoNum = 0;

                    if (AI_ID_Obs != AI_ID) {
                        AI_ID = long.Parse(this.GetComponent<AI_Property>().AI_ID);
                    }

                    if (AI_ID.ToString() != this.GetComponent<AI_Property>().AI_ID)
                    {
                        AI_ID = long.Parse(this.GetComponent<AI_Property>().AI_ID);
                    }

                    //显示复活时间UI
                    //判定怪物是否死亡
                    if (ai_IfDeath)
                    {
                        //判定怪物的唯一ID
                        if (AI_ID_Only != "" && AI_ID_Only != "0" && this.GetComponent<AI_Property>().AI_MonsterCreateType!="2")
                        {
                            //检测存储的复活数据里有没有相同的怪物ID
                            bool showDeathTimeStatus = false;
                            string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                            string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
                            if (deathMonsterIDListStr != "")
                            {
                                for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
                                {
                                    string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                                    if (deathMonsterID[0] == AI_ID_Only)
                                    {
                                        //Debug.Log("deathMonsterID[0] == " + deathMonsterID[0] + "   AI_ID_Only = " + AI_ID_Only);
                                        showDeathTimeStatus = true;
                                    }
                                }
                            }
                            //如果没有相同的怪物ID则现实复活时间
                            if (!showDeathTimeStatus)
                            {
                                showMonsterDeathTimeUI();       //怪物复活时间显示
                            }
                            monsterDestoryTime = 99999;
                        }
                    }
                    string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");
                    string monsterSonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterSonType", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");
                    //存储唯一怪物ID
                    if (AI_ID_Only != "" && AI_ID_Only != "0")
                    {
                        //存储复活时间
                        Game_PublicClassVar.Get_function_AI.SaveMonsterDeathTime(AI_ID_Only, monsterRebirthTime.ToString(), monsterOffLineTime.ToString(),AI_ID.ToString());
                    }
                    
                    //取消选中状态;
                    AI_Selected_Status = false;
                    //清空目标
                    AI_Target = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;

                    ObscuredFloat dropProVlaue = 0;  //掉落倍率
                    ObscuredFloat dropValue = 1;

                    //触发怪物掉落及任务相关规则
                    if (!AI_MonsterDeathStatus) {
                        AI_MonsterDeathStatus = true;   //保证执行一次

                        //怪物死后创建的一个掉落
                        if (!dropStatus)
                        {
                            dropStatus = true;
                            //Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(GetComponent<AI_Property>().AI_ID, this.transform.position);     //此代码消耗大
                            dropProVlaue = dropProVlaue + dropValue;

                            //装备属性额外触发掉落
                            if (Random.value <= Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_DropExtra){
								//Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(GetComponent<AI_Property>().AI_ID, this.transform.position);     //此代码消耗大
                                dropProVlaue = dropProVlaue + dropValue;
                            }

                            //获取当前角色等级和怪物等级,绝对值小于10的才能享受难度奖励的加成
                            int roseLv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
                            int aiLv = this.GetComponent<AI_Property>().AI_Lv;
                            int lvChaRoseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                            //最低保障前两章是双倍掉率
                            if (lvChaRoseLv >= 55) {
                                lvChaRoseLv = 55;
                            }
                            int lvCha = lvChaRoseLv - aiLv;

                            int chaLvValue = aiLv - roseLv;
                            if (Mathf.Abs(chaLvValue) <= 100)         //玩家真凶我,老抱怨我就改啦
                            {
                                if (monsterType == "3") {

                                    //等级差超过15级不进行爆率加成
                                    if (lvCha <= 15)
                                    {
                                        switch (Game_PublicClassVar.Get_game_PositionVar.GameNanduValue)
                                        {
                                            //难度2执行有概率执行掉落
                                            case "2":
                                                if (Random.value >= 0.5f)
                                                {
                                                    //Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(GetComponent<AI_Property>().AI_ID, this.transform.position);     //此代码消耗大
                                                    dropProVlaue = dropProVlaue + dropValue;
                                                    //Debug.Log("执行了2次掉率");
                                                }
                                                break;

                                            //难度3执行有概率执行2次掉落
                                            case "3":
                                                //Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(GetComponent<AI_Property>().AI_ID, this.transform.position);     //此代码消耗大
                                                dropProVlaue = dropProVlaue + dropValue;
                                                //Debug.Log("执行了地狱掉率");
                                                break;
                                        }
                                    }
                                    else {
                                        string hintStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_460");
                                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("击败比自己等级低15级的BOSS不触发难度爆率加成");
                                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(hintStr);
                                    }

                                    //超爽大爆
                                    float addValue = Game_PublicClassVar.Get_function_Rose.GetBaoLvProValue(Game_PublicClassVar.Get_wwwSet.BaoLvID);
                                    if (addValue >= 1) {
                                        addValue = addValue - dropValue;
                                    }

                                    //排行榜爆率（判断等级差）
                                    string baolvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RankBaoLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                                    if (baolvStr == "") {
                                        baolvStr = "0";
                                    }
                                    if (float.Parse(baolvStr) >= 1) {
                                        baolvStr = "1";
                                    }

                                    //等级差超过15级不进行爆率加成
                                    if (lvCha >= 15) {
                                        baolvStr = "0";
                                    }

                                    addValue = float.Parse(baolvStr) + addValue;

                                    //提示并初始化
                                    if (addValue >= 0) {
                                        string lang_str_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_433");
                                        string lang_str_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_434");
                                        Game_PublicClassVar.Get_function_UI.GameHint(lang_str_1 + Game_PublicClassVar.Get_function_Rose.GetBaoLvName(Game_PublicClassVar.Get_wwwSet.BaoLvID) + lang_str_2 + "！");
                                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你触发了" + Game_PublicClassVar.Get_function_Rose.GetBaoLvName(Game_PublicClassVar.Get_wwwSet.BaoLvID) + "爆率效果！");
                                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BaoLvID", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                                        Game_PublicClassVar.Get_wwwSet.BaoLvID = 1;
                                    }

                                    dropProVlaue = dropProVlaue + addValue;
                                }
                            }

                            //怪物属性掉落  
                            if (GetComponent<AI_Property>().AI_MonsterType == "1" && monsterSonType == "0") {

                                switch (GetComponent<AI_Property>().AI_TypeSon)
                                {
                                    //精英(额外掉落)
                                    case "3":
                                        //Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(GetComponent<AI_Property>().AI_ID, this.transform.position);     //此代码消耗大
                                        //Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(GetComponent<AI_Property>().AI_ID, this.transform.position);     //此代码消耗大
                                        dropProVlaue = dropProVlaue + 3;  //额外掉落5次,如果代码开销大,需要降低次数
                                        break;
                                }


                                //每日小怪掉落(防挂机)
                                float nowdropProVlaue = dropProVlaue;
                                if (Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum >= 1000)
                                {
                                    dropProVlaue = nowdropProVlaue * 0.9f;
                                }
                                if (Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum >= 2000)
                                {
                                    dropProVlaue = nowdropProVlaue * 0.8f;
                                }
                                if (Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum >= 2500)
                                {
                                    dropProVlaue = nowdropProVlaue * 0.7f;
                                }
                                if (Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum >= 3000)
                                {
                                    dropProVlaue = nowdropProVlaue * 0.6f;
                                }
                                if (Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum >= 4000)
                                {
                                    dropProVlaue = nowdropProVlaue * 0.5f;
                                }
                                if (Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum >= 5000)
                                {
                                    dropProVlaue = nowdropProVlaue * 0.4f;
                                }
                                if (Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum >= 6000)
                                {
                                    dropProVlaue = nowdropProVlaue * 0.3f;
                                }
                                if (Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum >= 7000)
                                {
                                    dropProVlaue = nowdropProVlaue * 0.2f;
                                }
                                if (Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum >= 8000)
                                {
                                    dropProVlaue = nowdropProVlaue * 0.15f;
                                }
                            }

                            //封印塔的爆率始终为1,没有任何爆率加成
                            if (this.GetComponent<AI_Property>().AI_MonsterCreateType == "2") {
                                dropProVlaue = dropValue;
                                //并触发额外掉落
                                if (CreateEWaoDropID != "" && CreateEWaoDropID != "0" && CreateEWaoDropID != null)
                                {
                                    Game_PublicClassVar.Get_function_AI.DropIDToDropItem(CreateEWaoDropID, this.transform.position, GetComponent<AI_Property>().AI_ID);
                                }
                            }

                            //触发掉落
                            ObscuredInt maxDropNum = 6;
                            if (dropProVlaue > 0) {

                                //防止意外,最多6倍爆率
                                if (dropProVlaue >= maxDropNum) {
                                    dropProVlaue = maxDropNum;
                                }

                                //Debug.Log("当前爆率:" + dropProVlaue + "name:" + this.gameObject.name);

                                Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(GetComponent<AI_Property>().AI_ID, this.transform.position, dropProVlaue);     //此代码消耗大
                            }

                            //写入成就
                            Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("1", AI_ID.ToString(), "1");
                            Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("2", "0", "1");

                            //判定是否为领主级怪物
                            if (monsterType == "3"&& this.GetComponent<AI_Property>().AI_MonsterCreateType != "2")
                            {
                                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("3", "0", "1");
                                //判定难度
                                switch (Game_PublicClassVar.Get_game_PositionVar.GameNanduValue)
                                {
                                    //普通难度
                                    case "0":
                                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("4", AI_ID.ToString(), "1");
                                        break;
                                    //普通难度
                                    case "1":
                                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("4", AI_ID.ToString(), "1");
                                        break;
                                    //挑战难度
                                    case "2":
                                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("5", AI_ID.ToString(), "1");
                                        break;
                                    //地狱难度
                                    case "3":
                                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("6", AI_ID.ToString(), "1");
                                        //180秒内激活
                                        if (fightTimeSum <= 180)
                                        {
                                            Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("7", AI_ID.ToString(), "1");
                                        }
                                        break;
                                }

                                //写入记录值
                                string writeNanDu = Game_PublicClassVar.Get_game_PositionVar.GameNanduValue;
                                if (writeNanDu == "0") {
                                    writeNanDu = "1";
                                }

                                if (fightStatus) {
                                    Game_PublicClassVar.Get_function_AI.SaveKillBossTime(AI_ID.ToString() + "_" + writeNanDu, (int)(fightTimeSum));
                                }
                            }

                            //存储成就
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseChengJiu");

                            //判定是否为领主级怪物
                            if (this.GetComponent<AI_Property>().AI_MonsterCreateType != "2")
                            {
                                //触发精灵
                                string spiritPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpiritPro", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");
                                if (spiritPro != "")
                                {
                                    string spiritID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpiritID", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");
                                    if (Random.value < float.Parse(spiritPro))
                                    {
                                        Game_PublicClassVar.Get_function_Rose.JingLing_Add(spiritID);
                                    }
                                }
                            }

                            //判断当前是否挂机
                            AddGuaJiNum();
                        }

                        //获取怪物是否有任务需求掉落
                        string[] dropTaskItem = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropTaskItem", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template").Split(';');
                        if (dropTaskItem[0] != "0")
                        {
                            for (int i = 0; i <= dropTaskItem.Length - 1; i++)
                            {
                                string[] taskDropID = dropTaskItem[i].Split(',');
                                string taskStatus = Game_PublicClassVar.Get_function_Task.TaskReturnStatus(taskDropID[0]);
                                if (taskStatus == "3")
                                {
                                    //当返回的任务状态为3,也就是任务已接取,未完成则触发一次掉落
                                    Game_PublicClassVar.Get_function_AI.DropIDToDropItem(taskDropID[1], this.transform.position, GetComponent<AI_Property>().AI_ID);
                                    //Debug.Log("我执行了一次任务掉落");
                                }
                            }
                        }
                        
                        //怪物死亡给人物增加对应的经验值
                        ObscuredString monsterExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Exp", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");
                        Game_PublicClassVar.Get_function_Rose.AddExp(int.Parse(monsterExp),"0",true);
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_GetExp = true;

                        //给宠物添加经验值
                        Game_PublicClassVar.Get_function_AI.Pet_AddExp(int.Parse(monsterExp));

                        //触发一次死亡技能
                        //Debug.Log("触发死亡技能");
                        //actTriggerSkill("1", "2");       //尝试触发被动技能

                        /*
                        if (AI_ID == 70002006) { 
                            //死亡创建一个怪物
                            Game_PublicClassVar.Get_function_AI.AI_CreatMonster("70002007", this.transform.position);
                        }
                        */

                        //地图击杀
                        Game_PublicClassVar.Get_game_PositionVar.MapKillMonsterNum = Game_PublicClassVar.Get_game_PositionVar.MapKillMonsterNum + 1;

                        //今日击杀
                        Game_PublicClassVar.Get_function_AI.DayKillMonsterNum();

                        //是否为任务要求的怪物
                        Game_PublicClassVar.Get_function_Task.TaskMonsterNum(GetComponent<AI_Property>().AI_ID, 1);
                        
                        //更新任务
                        //Game_PublicClassVar.Get_function_Country.UpdataTaskValue("2", "1", "1");
                        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "1", "1");

                        //获取当前是否为BOSS
                        if (monsterType == "3" && this.GetComponent<AI_Property>().AI_MonsterCreateType != "2") {
                            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "2", "1");
                            //Game_PublicClassVar.Get_function_Country.UpdataTaskValue("3", "2", "1");
                            //Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "4", AI_ID.ToString(), AI_ID.ToString());       //传入隐藏任务指定BOSS的ID(会引发日常任务全部完成的问题)

                            //记录地图值
                            Game_PublicClassVar.Get_game_PositionVar.MapKillBossID = AI_ID.ToString();

                            //发送服务器BOSS记录值
                            bool ifSendBossKill = true;
                            if (Game_PublicClassVar.Get_wwwSet.GameRootStatus == true) {
                                if (PlayerPrefs.GetString("RootStatusError") == "1") {
                                    ifSendBossKill = false;
                                }
                            }

                            if (ifSendBossKill) {

                                string nanduStr = Game_PublicClassVar.Get_game_PositionVar.GameNanduValue;
                                if (nanduStr == "0" || nanduStr == "")
                                {
                                    nanduStr = "1";
                                }

                                Pro_ComStr_3 comStr_3 = new Pro_ComStr_3();
                                string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                                comStr_3.str_1 = zhanghaoID;
                                comStr_3.str_2 = GetComponent<AI_Property>().AI_ID;
                                comStr_3.str_3 = nanduStr;
                                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001102, comStr_3);

                            }
                        }

                        //大秘境的怪需要增加大秘境值
                        
                        if (monsterSonType == "1")
                        {
                            int addValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(3,7);
                            Game_PublicClassVar.Get_function_Rose.AddMiJingValue(addValue);
                        }

                        //更新隐藏NPC
                        Game_PublicClassVar.Get_game_PositionVar.NpcShowValueStatus = true;

                        //添加觉醒经验
                        if (Game_PublicClassVar.Get_game_PositionVar.JueXingStatus) {
                            if (Random.value <= 1f) {
                                int addJueXingExp = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 5);
                                Game_PublicClassVar.Get_function_Rose.AddJueXingExp(addJueXingExp);
                            }
                        }

                        //增加狩猎数量
                        if (Game_PublicClassVar.gameLinkServer.HuoDong_ShouLie_Status)
                        {
                            //获取当前等级差
                            string monsterLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");
                            if (monsterLv == "" || monsterLv == null) {
                                monsterLv = "0";
                            }

                            //狩猎发送
                            int bijiaoLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv() - 9;
                            if (int.Parse(monsterLv) >= bijiaoLv || int.Parse(monsterLv) >= 50) {
                                Game_PublicClassVar.gameLinkServer.HuoDong_ShouLie_SendNum = Game_PublicClassVar.gameLinkServer.HuoDong_ShouLie_SendNum + 1;
                            }
                        }
                    }

                    //停止移动
                    ai_NavMesh.SetDestination(this.transform.position);
                    ai_IfChase = false;         //追击模式取消

                    //隐藏阴影底
                    try
                    {
                        GameObject backObj = Obj_AIModel.transform.parent.transform.Find("BackDi").gameObject;
                        if (backObj == null)
                        {
                            backObj = Obj_AIModel.transform.Find("BackDi").gameObject;
                        }
                        if (backObj != null)
                        {
                            backObj.SetActive(false);
                        }
                    }
                    catch {
                        //Debug.Log(this.gameObject.name + "没有BackDi");
                    }
                    
                    //播放怪物死亡音效
                    Game_PublicClassVar.Get_function_UI.PlaySource("20014", "2");
					
                    //更新玩家剧情状态
                    string TriggerStoryID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerStoryID", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");

                    if (TriggerStoryID != "0"){
                        //获取玩家当前剧情状态值
                        string roseStoryStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                        if (roseStoryStatus == TriggerStoryID)
                        {
                            Game_PublicClassVar.Get_function_Rose.UpdataRoseStoryStatus();
                            //Debug.Log("玩家完成指定任务,更新了剧情值");
                        }
                    }

                    //获取怪物是否需要重生
                    string ifRevive = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfRevive", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");

                    //判定目标是否为宝宝,如果是宝宝,则不会复活
                    if (AI_Type == 1 || AI_Type == 2) {
                        ifRevive = "0";
                    }

                    //封印之塔召唤不复活
                    if (this.GetComponent<AI_Property>().AI_MonsterCreateType == "2") {
                        ifRevive = "0";
                    }

                    //不复活,直接销毁目标
                    if (ifRevive == "0") {
                        monsterDestoryTime = 5.0f;     //5秒尸体消失
                        Destroy(this.gameObject, monsterDestoryTime);
                    }

                    //复活
                    /*
                    if (ifRevive == "1")
                    {
                        monsterDestoryTime = 5.0f;     //5秒尸体消失
                    }
                    */

                    //怪物死亡注销模型
                    if (IfDeathDestoryModel){
                        monsterDestoryTime = 0f;
                    }

                    //怪物死亡注销角色当前目标
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget == this.gameObject) {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget = null;
                    }


                }

                //超过一定时间秒尸体消失
                if (monsterReirthTimeSum >= monsterDestoryTime)
                {
                    if (Obj_AIModel != null) {
                        Obj_AIModel.SetActive(false);  //模型隐藏
                    }

                    //宝宝或者变异直接删除
                    switch (AI_Type)
                    {
                        case 1:
                            string nowPetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", AI_PetID, "Pet_Template");
                            //普通
                            if (nowPetType == "0")
                            {
                                Destroy(this.gameObject);
                            }
                            //变异
                            if (nowPetType == "1")
                            {
                                Destroy(this.gameObject);
                            }
                            break;
                    }
                    //Destroy(this.gameObject);
                    //Destroy(monsterDeathTimeObj);
                    //Debug.Log("隐藏模型");
                }
                //Debug.Log("AI重生");
                //AI重生
                if (ai_monsterType != "3") {
                    monsterReirthTimeSum = monsterReirthTimeSum + Time.deltaTime;
                }
                //Debug.Log("monsterReirthTimeSum = " + monsterReirthTimeSum + "      monsterRebirthTime = " + monsterRebirthTime);
                if (monsterReirthTimeSum >= monsterRebirthTime) {
                    
                    Obj_AIModel.SetActive(true); //模型显示
                    ai_property.AI_Hp = ai_property.AI_HpMax;
                    //Debug.Log("怪物重生：恢复血量 = "+AI_Hp_Max);
                    ai_IfDeath = false;
                    monsterReirthTimeSum = 0.0f;
                    AI_Hp_Status = false;
                    //将出生点重置
                    this.gameObject.transform.position = ai_StarPosition;
                    ai_IfChase = false;         //追击模式取消
                    ai_FindNextPatrol = true;   //AI巡逻重置
                    AI_Status = "0";
                    ifbaby();       //更新一次宝宝
					this.GetComponent<AI_Status>().AI_Animator.Play("Idle");
                    //显示碰撞体
                    this.GetComponent<CharacterController>().enabled = true;
                    //隐藏阴影底
                    if (Obj_AIModel.transform.Find("BackDi") != null) {
                        GameObject backObj = Obj_AIModel.transform.Find("BackDi").gameObject;
                        if (backObj != null)
                        {
                            backObj.SetActive(true);
                        }
                    }

                    //设置碰撞体高
                    ai_NavMesh.height = 2;
                    Destroy(monsterDeathTimeObj);
                    AI_MonsterDeathUIStatus = false;
                    
                    AI_MonsterDeathStatus = false;
                    ai_IfDeathTimeStatus = false;    //设置怪物死亡复活状态
                    //设置怪物复活时间
                    monsterRebirthTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ReviveTime", "ID", AI_ID.ToString(), "Monster_Template"));
                    monsterOffLineTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffLineReviveTime", "ID", AI_ID.ToString(), "Monster_Template"));

                    //重生怪物一律设置为普通,方式蹲点刷尸体
                    GetComponent<AI_Property>().AI_TypeSon = "0";

                    fightTimeSum = 0;   //设置战斗时间
                }

                //销毁脱战光圈
                if (Obj_TuoZhanShow != null)
                {
                    Destroy(Obj_TuoZhanShow);
                }

                break;

            //释放技能
            case "6":
                string skillAnimation = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillAnimation", "ID", triggerSkillID, "Skill_Template");
                if (skillAnimation != "" && skillAnimation != "0")
                {
                    try
                    {
                        //Debug.Log("triggerSkillID = " + triggerSkillID + "没有对应的技能动作！动作名称：" + skillAnimation);
                        this.GetComponent<AI_Status>().AI_Animator.Play(skillAnimation);
                    }
				catch( System.Exception ex) {
					//Debug.Log("triggerSkillID = " + triggerSkillID + "没有对应的技能动作！" + "Exception = " + ex);
                    }
                }
                break;

            //宠物模式
            case "7":

                //获取玩家当前位置与自己的距离
                float roseDistance = Vector3.Distance(this.gameObject.transform.position, obj_Rose.transform.position);

                //判定与玩家的距离,如果大于多少距离则进行移动
                if (roseDistance >= 2.0f) {
                    /*
                    //获取玩家当前朝向
                    //Debug.Log("主角的角度：" + obj_Rose.transform.localEulerAngles.y);
                    float roseTargetAngle = obj_Rose.transform.localEulerAngles.y;
                    //获取玩家当前坐标
                    Vector3 rosePositionVec3 = obj_Rose.transform.position;
                    float petMove_X;    //宠物要移动的X坐标点
                    float petMove_Z;    //宠物要移动的Z坐标点

                    //朝向右,X轴为前
                    if (roseTargetAngle <= 180)
                    {
                        //2.5范围内随机一个值
                        petMove_X = obj_Rose.transform.position.x + Random.value * 2.5f;

                    }
                    //朝向左,X轴为后
                    else { 
                       
                        //2.5范围内随机一个值
                        petMove_X = obj_Rose.transform.position.x - Random.value * 2.5f;
                    
                    }

                    //朝向上,Y轴为上
                    if (roseTargetAngle <= 90 || roseTargetAngle >= 270)
                    {
                        //2.5范围内随机一个值
                        petMove_Z = obj_Rose.transform.position.z - Random.value * 2.5f;
                    }
                    //朝向下,Y轴为下
                    else
                    {
                        //2.5范围内随机一个值
                        petMove_Z = obj_Rose.transform.position.z + Random.value * 2.5f;
                    }
                    */

            
                        //if (!petMoveStatus) {
                            //petMoveStatus = true;
                            //开始跟随移动
                            ai_NavMesh.speed = AI_MoveSpeed;
                            //Vector3 petMovePosition = new Vector3(petMove_X, obj_Rose.transform.position.y, petMove_Z);
                            //petMovePosition = new Vector3(obj_Rose.gameObject.transform.position.x + Random.value * 2.5f, obj_Rose.gameObject.transform.position.y, obj_Rose.transform.position.z + Random.value * 2.5f);
                            //ai_NavMesh.SetDestination(petMovePosition);

                            ai_NavMesh.SetDestination(obj_Rose.transform.position);
                            //更新动作
                            ai_status.IfUpdateStatus = true;
                            ai_status.AI_StatusValue = 2;
                        //}

                    

                }

                if (roseDistance <= 1.0f)
                {
                    ai_NavMesh.speed = 0;
                    //petMoveStatus = false;

                }
                
                //随机获取玩家身后的一个坐标点

                //向移动点移动

                break;

            case "8":

                ai_NavMesh.speed = 0;

            break;
        }

        //判定当前AI是否处于返回状态
        if (!ai_IfReturn) {
            //判定角色离AI的距离
            if (AI_Target != null) {
                AI_Distance = Vector3.Distance(AI_Target.transform.position, transform.position);
            }
        }

        //取当前AI最大生命值,用来UI上的显示（此脚本只会执行一次,用来取当前怪物的最大血量，方便UI进度条的显示，放在star里面担心和ai属性脚本冲突,ai脚本是在Updata读取属性的）
        if (ai_property != null)
        {
            if (!MaxHp_Status)
            {
                AI_Hp_Max = ai_property.AI_HpMax;
                MaxHp_Status = true;
            }
        }

        
		//当角色离AI 20米内,AI显示血条
        if (AI_Distance < 20)
        {

            //判定怪物是否死亡
            if (ai_IfDeath && monsterDeathTimeObj == null)
            {
                //判定怪物的唯一ID
                if (AI_ID_Only != "" && AI_ID_Only != "0" && this.GetComponent<AI_Property>().AI_MonsterCreateType != "2")
                {
                    showMonsterDeathTimeUI();
                }
            }

            //怪物未死亡血条消失的清空
            if (UI_Hp == null && ai_IfDeath == false)
            {
                AI_Hp_Status = false;
            }

            //Debug.Log("距离内准备显示血条...");
            if (!AI_Hp_Status)
            {
                //显示UI后，表示为true;
                AI_Hp_Status = true;
                AI_Name = ai_property.AI_Name;          //显示AI姓名

                Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(Ai_HpShowPosition.position);
                Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);

                //实例化UI
                UI_Hp = (GameObject)Instantiate(UI_GameObject);

                //显示UI,并对其相应的属性修正
                UI_Hp.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet.transform);
                UI_Hp.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
                UI_Hp.transform.localScale = new Vector3(1f, 1f, 1f);
                UI_Hp.GetComponent<UI_AIHp>().Obj_AI = this.gameObject;


                //取得界面控件
                GameObject UI_Name = UI_Hp.transform.Find("Lal_Name").gameObject;

                //text.GetComponent<TMP_Text>().text = "112112";
                //显示当前UI名称
                TMP_Text UIname = UI_Name.GetComponent<TMP_Text>();
                switch (AI_Type)
                {
                    //ai_property.AI_Lv + "级" + "  " + 
                    case 0:
                        UIname.text = AI_Name;
                        break;

                    case 1:
                        string nowPetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", AI_PetID, "Pet_Template");
                        //普通
                        if (nowPetType == "0")
                        {
                            UIname.text = AI_Name + "宝宝";
                            UIname.color = Color.green;
                        }
                        //变异
                        if (nowPetType == "1")
                        {
                            UIname.text = AI_Name + "变异宝宝";
                            UIname.color = new Color(1, 0, 1, 1);
                        }
                        break;
                }

                UI_Hp.GetComponent<UI_AIHp>().Obj_aiLv.GetComponent<TMP_Text>().text = ai_property.AI_Lv.ToString();

                //显示复活时间
                //判定怪物是否死亡
                if (ai_IfDeath && monsterDeathTimeObj == null)
                {
                    //判定怪物的唯一ID
                    if (AI_ID_Only != "" && AI_ID_Only != "0" && this.GetComponent<AI_Property>().AI_MonsterCreateType != "2")
                    {
                        showMonsterDeathTimeUI();
                    }
                }
            }
            else {
                //后台进入前台重新载入一次时间
                if (Game_PublicClassVar.Get_wwwSet.BackEnterGameOnly)
                {
                    //判定怪物是否死亡
                    if (ai_IfDeath)
                    {
                        //判定怪物的唯一ID
                        if (AI_ID_Only != "" && AI_ID_Only != "0")
                        {
                            //showMonsterDeathTimeUI();
                            //Debug.Log("火速展示222！");
                        }
                    }
                }
            }

            if (UI_Hp != null)
            {
                //UI显示当前血量
                Image HpValue = UI_Hp.GetComponent<UI_AIHp>().Obj_aiImgValue.GetComponent<Image>();
                HpValue.fillAmount = (float)ai_property.AI_Hp / AI_Hp_Max;
            }
        }
        else {
            
            Destroy(UI_Hp);
            AI_Hp_Status = false;
        }
		
		//出现血条后,不断修正血条位置
		if (AI_Hp_Status) {
            
			if(UI_Hp!=null){
			
				Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(Ai_HpShowPosition.position);
				Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);
				
				//血条位置修正（根据分辨率的变化而变化）
				UI_Hp.transform.localPosition = new Vector3(Hp_show_position.x,Hp_show_position.y,0);
			}
        }
        
        //修正怪物复活显示位置
        //Debug.Log("a1");
        if (AI_MonsterDeathUIStatus) {
            //Debug.Log("a2");
            if ( monsterDeathTimeObj != null)
            {

                Vector3 DeathTime_position = Camera.main.WorldToViewportPoint(Ai_HpShowPosition.position);
                //Vector3 DeathTime_position = Camera.main.WorldToViewportPoint(ai_DeathTimePosition.position);
                DeathTime_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(DeathTime_position);

                //血条位置修正（根据分辨率的变化而变化）
                monsterDeathTimeObj.transform.localPosition = new Vector3(DeathTime_position.x, DeathTime_position.y, 0);
                //Debug.Log("a3");
            }
        }
		
		//当和AI距离超过20后，不显示其UI
		if(AI_Distance >= 20) {

		    AI_Hp_Status = false;

            Destroy(UI_Hp);
            Destroy(monsterDeathTimeObj);
            AI_MonsterDeathUIStatus = false;
            //回归原来位置,以后添加
        }

		//选中状态触发时播放特效
		if (AI_Selected_Status) {

			//此次判定为了只实例化一次
			if (!Selected_Effect_Status) {

				if(Selected_Effect==null){

			    //Debug.Log("怪物选中特效为空");
				}else{
                
                }

				effect = (GameObject)Instantiate (Selected_Effect);
                effect.transform.SetParent(Selected_Effect_Position.transform);
                Selected_Effect_Status = true;

                //获取选中大小
                float selectedEffectSize = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SelectSize", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template"));
                //float selectedEffectSize = 2.0f;
                effect.GetComponent<Rose_SelectTarget>().SelectEffectSize = selectedEffectSize * 1.5f;

				//重置特效位置
				effect.transform.localPosition = new Vector3 (0, 0.05f, 0);

                //选中描红边
                //ModelMesh.material.SetColor("_OutlineColor", Color.red);     //设置受击变色

                //新手引导
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_ZhuaPu();

            }

		} else {

			if(effect){
			    Destroy(effect);
			    Selected_Effect_Status = false;
                //选中描红边
                if (ModelMesh != null) {
                    ModelMesh.material.SetColor("_OutlineColor", Color.black);     //设置受击变色
                }
			}
		}

		//死亡时注销生命条
		if (ai_property != null) {

			if (ai_property.AI_Hp < 1) {

                Destroy (UI_Hp);
			}


        }
        
        //战斗飘字
        if (HitStatus) {

			if (skillID [0] != "0"&&skillID [0] != "") {
				for (int i = 0; i <= skillID.Length - 1; i++) {
					//获取技能触发类型
					string skillType = passiveSkillType [i];
					switch (skillType) {
					//受到攻击触发
					case "3":
						if (Random.value <= float.Parse (passiveSkillPro [i])) {
							triggerSkill (i);        //触发怪物技能  
						}
						break;
					}
				}
			}

            hitMeshStatus = true;
            //防止死亡到底也变成红色
            if (ai_property.AI_Hp > 0) {
                if (ModelMesh!=null)
                {
                    ModelMesh.material.SetColor("_Color", Color.red);     //设置受击变色
                }
            }
            //播放特效
            if (IfHitEffect) {
                if (HitEffect != null)
                {
                    GameObject effect = (GameObject)Instantiate(HitEffect);
                    effect.transform.SetParent(HitEffectt_Position.transform);

                    //重置特效位置
                    effect.transform.localPosition = new Vector3(0, 0, 0);
                    HitStatus = true;

                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose != null) {
                        effect.transform.LookAt(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform);
                    }

                }

                //清空数据
                IfHitEffect = false;
            }

            //战斗飘字
			int damgeValue = LastHp - ai_property.AI_Hp;
            LastHp = LastHp - damgeValue;

			/*
			GameObject HitObject_p = (GameObject)Instantiate(HitObject);
			Text label = HitObject_p.GetComponent<Text>();

            //label.text = "-" + damgeValue.ToString();
			label.text = AIHitText + "-" + damgeValue.ToString() + AIHitTextlater;

            if (ZhongJiStatus) {
                ZhongJiStatus = false;
				label.text = "重击  -" + damgeValue.ToString() + AIHitTextlater;
            }

            if (HitCriStatus)
            {
                HitCriStatus = false;
				label.text = "暴击  -" + damgeValue.ToString() + AIHitTextlater;
            }

            if (ActReboundStatus) { 
                HitCriStatus = false;
				label.text = "攻击反击  -" + damgeValue.ToString() + AIHitTextlater;
            }

            if (MageReboundStatus)
            {
                HitCriStatus = false;
				label.text = "法术反击  -" + damgeValue.ToString() + AIHitTextlater;
            }

            //伤害为0表示闪避
            if (damgeValue == 0) {
                if (IfDodge) {
                    IfDodge = false;
                    label.text = "闪避";
                }
            }

            if (UI_Hp != null) {
                //HitObject_p.transform.parent = UI_Hp.transform;
                HitObject_p.transform.SetParent(UI_Hp.transform);
                HitObject_p.transform.localPosition = new Vector3(0, 40, 0);
                HitObject_p.transform.localScale = new Vector3(1, 1, 1);
            }
            */
			HitStatus=false;
		}
        
        if (hitMeshStatus)
        {
            hitMeshTime = hitMeshTime + Time.deltaTime;
            if (hitMeshTime > 0.15f) {
                if (ModelMesh != null) {
                    ModelMesh.material.SetColor("_Color", Color.white);     //设置受击变色
                }
                
                hitMeshStatus = false;
                hitMeshTime = 0;
            }
        }

        //判定自己是否为召唤物,如果父级为返回或死亡状态,则删除自己
        if (MonsterCreateObj != null) {
            string ai_statusValue = MonsterCreateObj.GetComponent<AI_1>().AI_Status;
            if (ai_statusValue == "1" || ai_statusValue == "4" || ai_statusValue == "5") {
                this.GetComponent<AI_Property>().AI_Hp = 0;
            }
        }

        //计算战斗时间
        if (fightStatus) {
            fightTimeSum = fightTimeSum + Time.deltaTime;
        }
	}

    //AI近战攻击
	public void AIAct(){

        //判定与当前目标的攻击距离
        if (AI_Target != null) {
            float dis = Vector3.Distance(this.gameObject.transform.position, AI_Target.transform.position);
            if (dis > ai_ActDistance) {
                //攻击距离大于3,攻击无效
                return;
            }
        }

        //攻击距离超过6判定为远程
        if (ai_ActDistance >= 6) {
            actTriggerSkill("2", "1");
            return;
        }
        

        //攻击角色
        if (AI_Target.layer == 14)
        {
        	Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseIfHit = true;      	//播放受击特效
            Game_PublicClassVar.Get_fight_Formult.MonsterActRose("62000001", this.gameObject, AI_Target,true);        	//战斗
            actTriggerSkill("2", "1");
			//Debug.Log ("ai普通攻击" +  "ai_ActDistance = " + ai_ActDistance + ";AI_Distance = " + AI_Distance);
        }

        //攻击宠物
        if (AI_Target.layer == 18)
        {
            AI_Target.GetComponent<AIPet>().HitStatus = true;      //播放受击特效
            Game_PublicClassVar.Get_fight_Formult.MonsterActRose("62000001", this.gameObject, AI_Target, true);                      //战斗
            actTriggerSkill("2", "1");
        }
    }

    //AI远程攻击
    public void AIAct_Remote() {
        actTriggerSkill("2","1");
    }

    //触发被动技能,triggerTime表示触发时间,1 表示起手动作前 ,2表示起手动作后    triggerType表示触发类型 1：表示攻击时触发  2：表示每帧都监测一次
    void actTriggerSkill( string triggerTime,string triggerType ) {

        

        //判定触发被动技能
        if (skillID[0] != "0")
        {
            for (int i = 0; i <= skillID.Length - 1; i++)
            {
                //Debug.Log("循环技能" + skillID[i]);
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

                        switch (passiveSkillTriggerTime[i]) {
                            //每次有攻击行为时,无限定条件
                            case "0":
                                if (Random.value <= float.Parse(passiveSkillPro[i]))
                                {
                                    /*
                                    if (skillID[i] == "62000201")
                                    {
                                        Debug.Log("620000201 = " + passiveSkillTriggerTime[i]);
                                    }
                                    */
                                    //if (actStatus)
                                    //{
                                        triggerSkill(i);        //触发怪物技能
                                    //}
                                }
                            break;
                            //每次有攻击行为时触发，必须与传入的值一样,才会触发
                            case "1":
                                if (passiveSkillTriggerTime[i] == triggerTime) {

                                    //if (actStatus)
                                    //{
                                        //actStatus = false;
                                        if (Random.value <= float.Parse(passiveSkillPro[i]))
                                        {
                                            triggerSkill(i);        //触发怪物技能
                                            //Debug.Log("触发怪物攻击 + " + i);
                                        }
                                    //}
                                }

                            break;
                            //每次有攻击动作时触发，必须与传入的值一样,才会触发
                            case "2":
        
                                if (passiveSkillTriggerTime[i] == triggerTime) {

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
                        if (ai_property.AI_Hp <= ai_property.AI_HpMax * float.Parse(passiveSkillPro[i]))
                        {
                            if (ifSkillTrigger[i] == "0")
                            {
                                triggerSkill(i);            //触发怪物技能
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
    void triggerSkill(int i) {

        bool triggerSkillStatus = false;
        if (ifSkillTrigger[i] == "0")
        {
            //防止技能执行多次
            if (passiveSkillTriggerOnce[i] == "1")
            {
                ifSkillTrigger[i] = "1";
            }

            //判定技能是否有CD,有CD设定不能触发
            if (passiveSkillTriggerTimeCD[i] != "0")
            {
                ifSkillTrigger[i] = "1";
                passiveSkillTriggerTimeCDSum[i] = "0";  //清空技能CD累计时间
            }

            triggerSkillStatus = true;
        }

        //判定当前状态是否眩晕
        if (XuanYunStatus) {
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseOcc == "1") {
                triggerSkillStatus = false;
            }
        }

        if (triggerSkillStatus) {

            //触发BUFF
            //string skillObjName = "Monster_FireWall_1";
            string skillObjName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectName", "ID", skillID[i], "Skill_Template");
            //skillObjName = "Monster_FireWall_1";
            //Debug.Log("触发技能skillID[i] = " + skillID[i]);
            GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill_Obj/" + skillObjName, typeof(GameObject));
            GameObject SkillObject_p = (GameObject)Instantiate(SkillObj);
            SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = AI_Target;
            string skillParent = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParent", "ID", skillID[i], "Skill_Template");
            //Debug.Log("触发技能+" + skillID[i]);
            /*
            if (skillID[i] == "62000103") {
                Debug.Log("触发召唤技能");
            }
            */
            //skillParent = "2";      //测试无绑定点用的,后面需删掉
            switch (skillParent)
            {
                //绑定在身上
                case "0":
                    //目前只支持对自己附加
                    //Debug.Log("技能挂在自己身上+" + skillID[i]);
                    string skillParentPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID[i], "Skill_Template");
                    SkillObject_p.transform.SetParent(this.GetComponent<AI_1>().BoneSet.transform.Find(skillParentPosition));
                    SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);
                    SkillObject_p.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID[i];
                    SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                    SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;
                    triggerSkillID = skillID[i];
                    SkillStatus = true;         //开启技能状态
                    break;
                //无绑定点
                case "1":
                    //目前只支持对攻击目标区域释放
                    //获取攻击目标位置
                    Vector3 skillPosition = AI_Target.transform.position;
                    SkillObject_p.transform.position = skillPosition;
                    //skillID[i] = "60090002";
                    SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID[i];
                    SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                    SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                    SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;

                    SkillStatus = true;         //开启技能状态
                    triggerSkillID = skillID[i];
                    break;

                //无绑定点,释放起始位置位于AI中心
                case "2":
                    //目前只支持对攻击目标区域释放
                    //获取攻击目标位置
                    skillPosition = AI_Target.transform.position;
                    string playStartPoisition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID[i], "Skill_Template");
                    SkillObject_p.transform.position = BoneSet.transform.Find(playStartPoisition).transform.position;
                    //skillPosition = new Vector3(SkillObject_p.transform.position.x + Random.value * 5 - 2.5f, SkillObject_p.transform.position.y, SkillObject_p.transform.position.z + Random.value * 5 - 2.5f);
               
                    //skillID[i] = "60090002";
                    SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID[i];
                    SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                    SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                    SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;
                    SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = AI_Target;

                    SkillStatus = true;         //开启技能状态
                    triggerSkillID = skillID[i];
                    break;


                //无绑定点,释放起始位置位于AI中心
                case "3":
                    //目前只支持对攻击目标区域释放
                    //获取攻击目标位置
                    //Debug.Log("AI_Target = " + AI_Target.name);
                    skillPosition = AI_Target.transform.position;
                    playStartPoisition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID[i], "Skill_Template");
                    SkillObject_p.transform.position = BoneSet.transform.Find(playStartPoisition).transform.position;
                    skillPosition = new Vector3(SkillObject_p.transform.position.x + Random.value * 5 - 2.5f, skillPosition.y, SkillObject_p.transform.position.z + Random.value * 5 - 2.5f);

                    //skillID[i] = "60090002";
                    SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID[i];
                    SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                    SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                    SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;
                    SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = AI_Target;

                    SkillStatus = true;         //开启技能状态
                    triggerSkillID = skillID[i];
                    break;


                //无绑定点,释放起始位置位于AI目标中心
                case "4":
                    //目前只支持对攻击目标区域释放
                    //获取攻击目标位置
                    //Debug.Log("AI_Target = " + AI_Target.name);
                    skillPosition = AI_Target.transform.position;
                    //playStartPoisition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID[i], "Skill_Template");
                    //SkillObject_p.transform.position = BoneSet.transform.Find(playStartPoisition).transform.position;
                    skillPosition = new Vector3(skillPosition.x + Random.value * 5 - 2.5f, skillPosition.y, skillPosition.z + Random.value * 5 - 2.5f);

                    SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID[i];
                    SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                    SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                    SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = this.gameObject;
                    SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = AI_Target;

                    SkillStatus = true;         //开启技能状态
                    triggerSkillID = skillID[i];
                    break;
            }
        }
    }

    //出现怪物复活时间条
    void showMonsterDeathTimeUI() {

        if (monsterDeathTimeObj != null)
        {
            //Debug.Log("复活条1111...");
            Destroy(monsterDeathTimeObj);
        }
        //Debug.Log("复活条...");
        Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(Ai_HpShowPosition.position);
        //Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(ai_DeathTimePosition.position);
        monsterDeathTimeObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_MonsterDeathTime);
        monsterDeathTimeObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet.transform);
        monsterDeathTimeObj.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
        monsterDeathTimeObj.transform.localScale = new Vector3(1f, 1f, 1f);
        ObscuredFloat deathTime = 3600;
        if (AI_ID_Only != "" && AI_ID_Only != "0")
        {
            //更新在线刷新时间
            ObscuredFloat rebirthTime = Game_PublicClassVar.Get_function_AI.GetMonsterDeathTime(AI_ID_Only);
            if (rebirthTime != 0)
            {
                deathTime = rebirthTime;
                //Debug.Log("*************************************************************************");
                //monsterRebirthTime = rebirthTime;
                monsterDeathTimeObj.GetComponent<UI_MonsterDeathTime>().DeathRebirthTimeSum = monsterRebirthTime - rebirthTime;
            }
            //更新离线刷新时间
            ObscuredFloat offLineTime = Game_PublicClassVar.Get_function_AI.GetMonsterDeathOffLineTime(AI_ID_Only);
            if (offLineTime != 0)
            {
                monsterOffLineTime = offLineTime;
            }
        }

        monsterDeathTimeObj.GetComponent<UI_MonsterDeathTime>().DeathTime = monsterRebirthTime;
        monsterDeathTimeObj.GetComponent<UI_MonsterDeathTime>().deathMonsterName = AI_Name;
        monsterDeathTimeObj.GetComponent<UI_MonsterDeathTime>().Obj_MonsterObj = this.gameObject;
        monsterDeathTimeObj.GetComponent<UI_MonsterDeathTime>().OffLineTime = monsterOffLineTime;
        AI_MonsterDeathUIStatus = true;     //更新显示
        //AI_MonsterDeathStatus = true;     
        ai_IfDeathTimeStatus = true;        //设置怪物死亡复活状态
    
    }

    private void ifbaby() {

        //随机判定自身是否为宝宝
        float babyPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BabyPro", "ID", AI_ID.ToString(), "Monster_Template"));
        AI_PetID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", AI_ID.ToString(), "Monster_Template");
        if (AI_PetID == "0" || AI_PetID == "" || AI_PetID == null) {
            return;
        }

        if (babyPro > 0)
        {
            if (Random.value <= babyPro)
            {
                //当前宠物为宝宝
                AI_Type = 1;
            }
        }

        //变异宝宝
        float bianYiBabyPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BianYiBabyPro", "ID", AI_ID.ToString(), "Monster_Template"));
        //bianYiBabyPro = 1;
        if (bianYiBabyPro > 0)
        {
            if (Random.value <= bianYiBabyPro)
            {
                AI_Type = 1;
                AI_PetID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BianYiPetID", "ID", AI_ID.ToString(), "Monster_Template");
                Game_PublicClassVar.Get_function_AI.AI_AddBianYiTieTu(this.gameObject, AI_PetID);
            }
        }

        
        //万分之一的概率变为泡泡
        float paopaoPro = 0.000025f;
        if(Random.value <= paopaoPro){
            Game_PublicClassVar.Get_function_AI.AI_CreatMonster("70009901",this.gameObject.transform.position);
            this.gameObject.SetActive(false);
        }
    }

    //重置自身技能
    private void initialSkill() {

        //重置技能
        if (skillID[0] != "0")
        {
            string PassiveSkillTypeStr = "";        //技能类型参数
            string PassiveSkillProStr = "";         //技能参数
            string PassiveSkillTriggerOnceStr = "";  //技能只执行一次参数
            string PassiveSkillTriggerTimeStr = "";     //技能触发时机
            string IfPassiveSkillTrigger = "";          //技能触发状态
            string passiveSkillTriggerTimeCDStr = "";          //技能触发状态
            string passiveSkillTriggerTimeCDSumStr = "";          //技能触发状态
            for (int i = 0; i <= skillID.Length - 1; i++)
            {
                //Debug.Log("skillID" + i + " = " +skillID[i]);
                //循环获取技能数据
                string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillType", "ID", skillID[i], "Skill_Template");
                //Debug.Log("skillType = " + skillType + "skillID[i] = " + skillID[i]);
                string skillPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillPro", "ID", skillID[i], "Skill_Template");
                string skillTriggerOnce = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillTriggerOnce", "ID", skillID[i], "Skill_Template");
                string skillTriggerTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillTriggerTime", "ID", skillID[i], "Skill_Template");
                string skillTriggerTimeCD = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", skillID[i], "Skill_Template");

                //根据难度对技能CD进行缩减
                if (skillTriggerTimeCD != "" && skillTriggerTimeCD != "0" && skillTriggerTimeCD != null) {
                    float timeCD_float = float.Parse(skillTriggerTimeCD);
                    switch (Game_PublicClassVar.Get_game_PositionVar.GameNanduValue) {
                        case "2":
                            timeCD_float = timeCD_float * 0.8f;
                            break;

                        case "3":
                            timeCD_float = timeCD_float * 0.6f;
                            break;
                    }

                    skillTriggerTimeCD = timeCD_float.ToString();
                    /*
                    if (timeCD_float < 0) {
                        timeCD_float = 0;
                    }
                    */
                }


                PassiveSkillTypeStr = PassiveSkillTypeStr + skillType + ";";
                PassiveSkillProStr = PassiveSkillProStr + skillPro + ";";
                PassiveSkillTriggerOnceStr = PassiveSkillTriggerOnceStr + skillTriggerOnce + ";";
                PassiveSkillTriggerTimeStr = PassiveSkillTriggerTimeStr + skillTriggerTime + ";";
                IfPassiveSkillTrigger = IfPassiveSkillTrigger + "0;";
                passiveSkillTriggerTimeCDStr = passiveSkillTriggerTimeCDStr + skillTriggerTimeCD + ";";
                passiveSkillTriggerTimeCDSumStr = passiveSkillTriggerTimeCDSumStr + "0;";
            }

            //Debug.Log("技能类型"+PassiveSkillTypeStr);

            passiveSkillType = PassiveSkillTypeStr.Split(';');
            passiveSkillPro = PassiveSkillProStr.Split(';');
            passiveSkillTriggerOnce = PassiveSkillTriggerOnceStr.Split(';');
            passiveSkillTriggerTime = PassiveSkillTriggerTimeStr.Split(';');
            ifSkillTrigger = IfPassiveSkillTrigger.Split(';');
            passiveSkillTriggerTimeCD = passiveSkillTriggerTimeCDStr.Split(';');
            passiveSkillTriggerTimeCDSum = passiveSkillTriggerTimeCDSumStr.Split(';');

        }
    
    }

    //进入角色攻击
    private void addMonsterActRoseList() {

        //防止二次调用
        if (actRoseStatus) {
            return;
        }

        //角色进入战斗状态
        List<GameObject> roseFightMonsterActList = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightMonsterActList;
        bool addStatus = true;
        for (int i = 0; i < roseFightMonsterActList.Count; i++)
        {
            if (roseFightMonsterActList[i] == this.gameObject)
            {
                addStatus = false;
            }
        }
        if (addStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightMonsterActList.Add(this.gameObject);
            actRoseStatus = true;
        }

        //自身也进入战斗状态
        fightStatus = true;
    }


    //退出角色攻击
    private void costMonsterActRoseList()
    {

        //防止二次调用
        if (!actRoseStatus)
        {
            return;
        }

        //角色进入战斗状态
        List<GameObject> roseFightMonsterActList = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightMonsterActList;
        for (int i = 0; i < roseFightMonsterActList.Count; i++)
        {
            if (roseFightMonsterActList[i] == this.gameObject)
            {
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightMonsterActList.Remove(roseFightMonsterActList[i]);
                actRoseStatus = false;
            }
        }

    }

    //创建怪物脱战范围
    public void TuoZhan_Show() {

        if (Obj_TuoZhanShow == null) {
            //获取当前是否为BOSS
            string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", GetComponent<AI_Property>().AI_ID, "Monster_Template");
            if (monsterType == "3")
            {
                Obj_TuoZhanShow = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_TuoZhanEffect);
                Obj_TuoZhanShow.transform.position = ai_StarPosition;
                Obj_TuoZhanShow.transform.localScale = new Vector3(ai_actRunRange, ai_actRunRange, ai_actRunRange);
                Obj_TuoZhanShow.GetComponent<MonsterActRange>().RangeSize = ai_actRunRange;
            }
        }
    }

    //脱战隐藏
    public void TuoZhan_YinCang() {
        if (Obj_TuoZhanShow != null)
        {
            Destroy(Obj_TuoZhanShow);
        }
    }

    //增加挂机数量
    private void AddGuaJiNum() {

        if (rose_status.AutomaticGuaJiStatus) {

            string num = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayGuaJiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (num == "" || num == null)
            {
                num = "0";
            }

            int nowNum = int.Parse(num) + 1;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayGuaJiNum", nowNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            rose_status.GetComponent<Rose_Status>().AutomaticGuaJiNum = nowNum;
        }

    }
}
