using UnityEngine;
using System.Collections;

//主要用于打开GameObje的技能开关和传递对应参数,和SkillObject的区别是绑定在技能开始脚本和触发技能脚本
public class SkillObjBase : MonoBehaviour {

    public bool SkillOpen;                      //当此开关打开后，执行对应的绑定技能
    public string UseSkillObjType;              //使用技能的类型   (0:表示通用,1:角色,2:表示宠物,3:表示宠物)
    public MonoBehaviour Obj_FirstSkill;        //第一个执行的技能脚本
    public string SkillID;                      //当前Skill的ID
    public float SkillLiveTime;                 //当前技能时间
    public Vector3 SkillTargetPoint;            //技能释放目标点
    public GameObject SkillTargetObj;           //技能释放的目标Obj  （对目标单体伤害时用）
    public GameObject MonsterSkillObj;          //技能释放技能时,才会用到此字段,表示此技能是哪个怪物释放的
    //public Vector3 SkillTar
    private int skillUseNum;                    //当前技能使用次数
    public float AddDelaySkillTime;             //额外增加的延迟时间
    public float DelaySkillTime;                //延迟技能释放时间
    private float delaySkillTimeSum;            //延迟技能释放时间累计值
    private GameObject HintSkillEffect;         //提示技能特效
    private bool hintSkillEffectStatus;         //提示技能状态
    public bool RoseSkillZhiShiStatus;          //角色技能指示状态
    private float SkillRigidity;               //怪物静止状态时间
    private float skillRigiditySum;

    //施法中吟唱
    public bool IfSkillSingStatus;              //技能施法中吟唱状态
    private float skillSingTime;                //技能施法中的吟唱时间

    private string nowSkillParent;

    //private float ;

    // Use this for initialization
    void Start()
    {

        //Debug.Log("nowSkillObject_p.transform999 = " + this.transform.localRotation);
        //初始化技能使用次数
        skillUseNum = 0;
        //Debug.Log("当前触发技能：" + SkillID);
        DelaySkillTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterDelayTime", "ID", SkillID, "Skill_Template"));
        SkillRigidity = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillRigidity", "ID", SkillID, "Skill_Template"));
        DelaySkillTime = DelaySkillTime + AddDelaySkillTime;
        if (MonsterSkillObj != null)
        {
            //怪物
            if (MonsterSkillObj.GetComponent<AI_1>() != null) {
                MonsterSkillObj.GetComponent<AI_1>().AIStopMoveStatus = true;
                MonsterSkillObj.GetComponent<AI_1>().StopMoveTime = SkillRigidity;
                MonsterSkillObj.GetComponent<AI_1>().StopMoveTimeSum = 0;
            }
            //宠物
            if (MonsterSkillObj.GetComponent<AIPet>() != null)
            {
                MonsterSkillObj.GetComponent<AIPet>().AIStopMoveStatus = true;
                MonsterSkillObj.GetComponent<AIPet>().StopMoveTime = SkillRigidity;
                MonsterSkillObj.GetComponent<AIPet>().StopMoveTimeSum = 0;
            }

        }
        //Debug.Log("nowSkillObject_p.transform888AAA = " + this.transform.localRotation);
        //设置怪物是否可以移动
        //MonsterSkillObj.GetComponent<AI_Property>().AI_MoveSpeed = 0;
        //设置技能范围
        Game_PublicClassVar.Get_function_Skill.AddSkillRange(this.gameObject.GetComponent<SkillObjBase>().SkillID, this.gameObject, MonsterSkillObj);
        //Debug.Log("nowSkillObject_p.transform888BBB = " + this.transform.localRotation);
        string ifLookAtTarget = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfLookAtTarget", "ID", SkillID, "Skill_Template");
        if (ifLookAtTarget == "1")
        {
            if (MonsterSkillObj != null)
            {
                //怪物
                if (MonsterSkillObj.GetComponent<AI_1>() != null) {
                    this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AIStopLookTargetStatus = true;
                    string ifLookAtTatgetTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfLookAtTatgetTime", "ID", SkillID, "Skill_Template");
                    this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AIStopLookTargetTime = float.Parse(ifLookAtTatgetTime);
                    this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AIStopLookTargetTimeSum = 0;
                }
                //宠物
                if (MonsterSkillObj.GetComponent<AIPet>() != null)
                {
                    this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AIPet>().AIStopLookTargetStatus = true;
                    string ifLookAtTatgetTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfLookAtTatgetTime", "ID", SkillID, "Skill_Template");
                    this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AIPet>().AIStopLookTargetTime = float.Parse(ifLookAtTatgetTime);
                    this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AIPet>().AIStopLookTargetTimeSum = 0;
                }

            }
        }
        //Debug.Log("nowSkillObject_p.transform888CCC = " + this.transform.localRotation);
        /*
        if (SkillID == "62003004")
        {
            //提示
            Game_PublicClassVar.Get_function_UI.GameGirdHint(DelaySkillTime + "秒后开启狂暴状态,请尽快击杀!");
        }
        */
        //技能施法中是否需要持续吟唱
        //Debug.Log("SkillID = " + SkillID);
        //string sss = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillSingTime", "ID", SkillID, "Skill_Template");
        //Debug.Log("sss = " + sss);
        skillSingTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillSingTime", "ID", SkillID, "Skill_Template"));
        //skillSingTime = float.Parse("0");
        if (skillSingTime > 0) {
            IfSkillSingStatus = true;
            //Debug.Log("开启施法吟唱!");
        }

        nowSkillParent = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParent", "ID", SkillID, "Skill_Template");

        //Debug.Log("nowSkillObject_p.transform888 = " + this.transform.localRotation);
    }
	
	// Update is called once per frame
	void Update () {

        //判定是否执行技能
        if (SkillOpen)
        {
            //Debug.Log("nowSkillObject_p.transform777 = " + this.transform.localRotation);
            delaySkillTimeSum = delaySkillTimeSum + Time.deltaTime;
            if (delaySkillTimeSum >= DelaySkillTime)
            {
                Obj_FirstSkill.enabled = true;
                skillUseNum = skillUseNum + 1;
                SkillOpen = false;      //保证只执行一次

                //删除技能提示特效
                if (HintSkillEffect != null) {
                    Destroy(HintSkillEffect);
                }
                delaySkillTimeSum = 0;

                //获取该技能有没有附加的技能
                string addSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddSkillID", "ID", SkillID, "Skill_Template");
                if (addSkillID != "0"&&addSkillID !="") {
                    Game_PublicClassVar.Get_function_MonsterSkill.AddSkillID(addSkillID, MonsterSkillObj);
                }

                //技能施法中是否需要持续吟唱
                if (IfSkillSingStatus)
                {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingStatus = true;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingTime = skillSingTime;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingSkillObj = this.gameObject;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingType = "2";
                }

                //设置技能存在时间
                SkillLiveTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
                //技能附加值（附加额外Buff）
                float SkillLiveTimeAdd = 0;
                string skillAddValueStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(this.GetComponent<SkillObjBase>().SkillID, "5");
                if (skillAddValueStr != "" && skillAddValueStr != "0")
                {
                    string[] skillAddValueList = skillAddValueStr.Split(',');
                    for (int i = 0; i < skillAddValueList.Length; i++)
                    {
                        SkillLiveTimeAdd = SkillLiveTimeAdd + float.Parse(skillAddValueList[i]);
                    }
                }
                SkillLiveTime = SkillLiveTime + SkillLiveTimeAdd;

                //播放技能音效
                string skillMusic = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillMusic", "ID", SkillID, "Skill_Template");
                if (skillMusic != "" && skillMusic != "0") {
                    //播放音效
                    Game_PublicClassVar.Get_function_UI.PlaySource(skillMusic, "4");
                }

                //技能附加值（附加额外Buff）（此处仅支持给自身附加buff,如果给目标附加buff会在具体的技能中实现）
                Game_PublicClassVar.Get_function_Skill.SkillAddValue_Buff(SkillID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose);

                //查询技能附加值
                Rose_Status Rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();

                //添加数据
                string AddSkillValueStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(this.GetComponent<SkillObjBase>().SkillID, "9");
                string[] addSkillValueFenGeStrList = AddSkillValueStr.Split(',');
                /*
                if (AddSkillValueStr.Contains("60032171")) {
                    Debug.Log("123");
                }
                */
                for(int i = 0; i< addSkillValueFenGeStrList.Length; i++)
                {
                    string[] AddSkillValueList = addSkillValueFenGeStrList[i].Split('@');

                    //获取触发概率
                    if (AddSkillValueList.Length >= 2)
                    {

                        float chuFaPro = float.Parse(AddSkillValueList[0]);
                        //chuFaPro = 1;
                        if (Random.value <= chuFaPro)
                        {
                            //触发技能
                            string chuFaString = AddSkillValueList[1];
                            //Debug.Log("天赋触发技能:" + chuFaString);
                            Game_PublicClassVar.Get_function_Skill.UseSkill(chuFaString);
                        }
                    }
                }

                //Debug.Log("nowSkillObject_p.transform666 = " + this.transform.localRotation);

            }
            else {
                if (!hintSkillEffectStatus) {
                    
                    //实例化一个提示特效
                    GameObject obj = (GameObject)Resources.Load("Effect/Rose/Rose_SelectRange_Monster", typeof(GameObject));        //实例化范围特效
                    HintSkillEffect = (GameObject)Instantiate(obj);
                    HintSkillEffect.transform.position = SkillTargetPoint;
                    hintSkillEffectStatus = true;
                    //Debug.Log("实例化了一个提示特效" + HintSkillEffect.name);
                    hintSkillEffectStatus = true;
                    //获取技能范围大小
                    float rangeSize = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillRangeSize", "ID", SkillID, "Skill_Template"));
                    HintSkillEffect.GetComponent<Rose_SelectRange>().RangeSize = rangeSize;
                }

                //判断技能是否是一个指示器
                //判断目标是否眩晕
                if (this.GetComponent<SkillObjBase>().MonsterSkillObj != null)
                {
                    if (this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>() != null)
                    {
                        if (this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().ChenMoStatus)
                        {
                            if (hintSkillEffectStatus == true)
                            {
                                Debug.Log("沉默销毁技能...");
                                //注销自身
                                Destroy(this.gameObject);
                                //注销自身指示器
                                Destroy(HintSkillEffect);
                            }
                        }
                    }
                }
            }
        }

        //如果父级为返回或死亡状态,则删除自己
        if (MonsterSkillObj != null)
        {
            //怪物
            if (MonsterSkillObj.GetComponent<AI_1>() != null) {
                string ai_statusValue = MonsterSkillObj.GetComponent<AI_1>().AI_Status;
                if (ai_statusValue == "1" || ai_statusValue == "4" || ai_statusValue == "5")
                {
                    Destroy(this.gameObject);
                    Destroy(HintSkillEffect);
                }
            }
            //宠物
            if (MonsterSkillObj.GetComponent<AIPet>() != null)
            {
                string ai_statusValue = MonsterSkillObj.GetComponent<AIPet>().AI_Status;
                if (ai_statusValue == "1" || ai_statusValue == "4" || ai_statusValue == "5")
                {
                    Destroy(this.gameObject);
                    Destroy(HintSkillEffect);
                }
            }

        }

        //始终跟随玩家
        if (nowSkillParent == "5")
        {
            this.gameObject.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        }

    }

    //销毁时调用
    void OnDestroy() {
        //this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AIStopLookTargetStatus = false;
    }
} 
