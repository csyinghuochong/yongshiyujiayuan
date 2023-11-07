using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class AI_Property : MonoBehaviour {

	//定义角色属性，方便其他脚本调用
    public ObscuredString AI_Type;              //怪物属性类型,  1表示怪物  2表示宠物
    public ObscuredString AI_TypeSon;           //怪物属性子类    当Type为1时( 0:普通 1:虚弱 2：强壮 3：头领)  
    public ObscuredString AI_MonsterType;       //怪物类型      (1：普通   2：精英	3：BOSS	4：怪物召唤)
    public ObscuredString AI_MonsterCreateType;     //怪物创建类型  （0: 默认  1：表示大秘境 2: 封印之塔创建）
    private ObscuredBool ai_NanDuStatus = true;
    private ObscuredBool AI_UpdateOnlyStatus;   //只更新一次状态,表示属性仅仅在创建的时候更新一次

    public ObscuredString AI_Name;              		//怪物名称
	public ObscuredInt AI_Lv;                   //怪物等级
	public ObscuredInt AI_HpMax;                        //怪物血量上限
	public ObscuredInt AI_Hp;                   //怪物血量
	public ObscuredInt AI_Act;                  //怪物攻击
	public ObscuredInt AI_MageAct;              //怪物魔法攻击
	public ObscuredInt AI_Def;                  //怪物物防
	public ObscuredInt AI_Adf;                  //怪物魔防
	public ObscuredFloat AI_Cri;                //怪物暴击
	public ObscuredFloat AI_Res;                //怪物韧性
	public ObscuredFloat AI_Hit;                //怪物命中
	public ObscuredFloat AI_Dodge;              //怪物闪避
	public ObscuredFloat AI_DefAdd;             //怪物物理免伤
	public ObscuredFloat AI_AdfAdd;             //怪物魔法免伤
	public ObscuredFloat AI_DamgeAdd;           //怪物伤害免伤
	public ObscuredFloat AI_ActDamgeAdd;        //怪物攻击加成
	public ObscuredFloat AI_MoveSpeed;          //怪物移动速度
	public ObscuredFloat AI_ActSpeed;           //怪物攻击速度

    public ObscuredFloat AI_HuiXuePro;              //回血
    public ObscuredFloat AI_XiXieValue;           	//AI吸血值
    public ObscuredFloat AI_XingYunPro;				//幸运
    public ObscuredFloat AI_DiXiaoPro;				//概率抵消本次伤害
    public ObscuredFloat AI_ChouHenValue;			//仇恨值
    public ObscuredFloat AI_FuHuoPro;				//复活概率

    public ObscuredFloat AI_resistance_1 = 0;                //光抗性
    public ObscuredFloat AI_resistance_2 = 0;                //暗抗性
    public ObscuredFloat AI_resistance_3 = 0;                //火抗性
    public ObscuredFloat AI_resistance_4 = 0;                //水抗性
    public ObscuredFloat AI_resistance_5 = 0;                //电抗性
    public ObscuredFloat AI_raceDamge_1 = 0;            //野兽攻击伤害
    public ObscuredFloat AI_raceDamge_2 = 0;            //人物攻击伤害
    public ObscuredFloat AI_raceDamge_3 = 0;            //恶魔攻击伤害
    public ObscuredFloat AI_raceResistance_1 = 0;            //野兽攻击抗性
    public ObscuredFloat AI_raceResistance_2 = 0;            //人物攻击抗性
    public ObscuredFloat AI_raceResistance_3 = 0;            //恶魔攻击抗性
    public ObscuredFloat AI_SkillActDefPro;           //技能抗性
    public ObscuredFloat AI_SkillActDodgePro;         //技能闪避
    public ObscuredFloat AI_MonsterActDameDefPro;          //怪物普通攻击减免
    public ObscuredFloat AI_MonsterSkillDameDefPro;        //怪物技能攻击减免

    //被动技能加成相关
    public ObscuredFloat AI_Act_Skill;
    public ObscuredFloat AI_MagAct_Skill;
    public ObscuredFloat AI_Def_Skill;
    public ObscuredFloat AI_Adf_Skill;
    public ObscuredFloat AI_HpMax_Skill;
    public ObscuredFloat AI_Cri_Skill;                //怪物暴击
    public ObscuredFloat AI_Res_Skill;                //怪物韧性
    public ObscuredFloat AI_Hit_Skill;                //怪物命中
    public ObscuredFloat AI_Dodge_Skill;              //怪物闪避
    public ObscuredFloat AI_DefAdd_Skill;             //怪物物理免伤
    public ObscuredFloat AI_AdfAdd_Skill;             //怪物魔法免伤
    public ObscuredFloat AI_DamgeAdd_Skill;           //怪物伤害免伤
    public ObscuredFloat AI_MoveSpeed_Skill;          //怪物移动速度
    public ObscuredFloat ActMul_Skill;                //攻击加成
    public ObscuredFloat MageActMul_Skill;            //技能加成
    public ObscuredFloat DefMul_Skill;                //物防加成
    public ObscuredFloat AdfMul_Skill;                //魔防加成
    public ObscuredFloat HpMaxMul_Skill;              //血量加成

    public ObscuredString AI_ID;

    public ObscuredFloat ActAdd;                    //攻击加成
    public ObscuredFloat DefAdd;                    //防御加成
    public ObscuredFloat AdfAdd;                    //魔防加成
    public ObscuredFloat MageActAdd;                //魔法加成

    public ObscuredFloat AI_Cri_Add;                //怪物暴击
    public ObscuredFloat AI_Res_Add;                //怪物韧性
    public ObscuredFloat AI_Hit_Add;                //怪物命中
    public ObscuredFloat AI_Dodge_Add;              //怪物闪避
    public ObscuredFloat AI_ActDamgeAdd_Add;        //伤害加成

    public ObscuredFloat ActMul;                    //攻击加成
    public ObscuredFloat MageActMul;                //魔法攻击加成
    public ObscuredFloat DefMul;                    //物防加成
    public ObscuredFloat AdfMul;                    //魔防加成

    public ObscuredFloat DamgeAddMul;               //怪物伤害免伤加成
    public ObscuredFloat MoveSpeedMul;              //移动速度加成

    public ObscuredFloat AI_HuiXuePro_Add;              //回血
    public ObscuredFloat AI_XiXieValue_Add;           	//AI吸血值
    public ObscuredFloat AI_XingYunPro_Add;				//幸运
    public ObscuredFloat AI_DiXiaoPro_Add;				//概率抵消本次伤害
    public ObscuredFloat AI_ChouHenValue_Add;			//仇恨值
    public ObscuredFloat AI_FuHuoPro_Add;				//复活概率

    public ObscuredFloat AI_resistance_1_Add;                //光抗性
    public ObscuredFloat AI_resistance_2_Add;                //暗抗性
    public ObscuredFloat AI_resistance_3_Add;                //火抗性
    public ObscuredFloat AI_resistance_4_Add;                //水抗性
    public ObscuredFloat AI_resistance_5_Add;                //电抗性

    public ObscuredFloat AI_raceDamge_1_Add;                //野兽攻击伤害
    public ObscuredFloat AI_raceDamge_2_Add;                //人物攻击伤害
    public ObscuredFloat AI_raceDamge_3_Add;                //恶魔攻击伤害

    public ObscuredFloat AI_raceResistance_1_Add;            //野兽攻击抗性
    public ObscuredFloat AI_raceResistance_2_Add;            //人物攻击抗性
    public ObscuredFloat AI_raceResistance_3_Add;            //恶魔攻击抗性

    public ObscuredInt base_HpMax;
    public ObscuredInt base_Act;
    public ObscuredInt base_MageAct;
    public ObscuredInt base_Def;
    public ObscuredInt base_Adf;
    public ObscuredFloat base_Cri;                      //怪物暴击
    public ObscuredFloat base_Res;                      //怪物韧性
    public ObscuredFloat base_Hit;                      //怪物命中
    public ObscuredFloat base_Dodge;                    //怪物闪避
    public ObscuredFloat base_DefAdd;                   //怪物物理免伤
    public ObscuredFloat base_AdfAdd;                   //怪物魔法免伤
    public ObscuredFloat base_DamgeAdd;
    public ObscuredFloat base_MoveSpeed;



    public string[] SkillIDList;

    private Animator AI_Animator;
    private ObscuredFloat DestroyTime;
    public GameObject DropGameObject;                   //掉落物体
    public ObscuredBool UpdataAIPropertyStatus;         //打开
    public ObscuredBool StartAIPropertyStatus;      	//第一次加载的AI的属性

    private ObscuredInt LastHp;                         //用于生命UI显示
    private ObscuredFloat ai_HealHpTimeSum;             //恢复血量的时间累计

    public ObscuredFloat AI_SummonPropertyHpPro;        //召唤属性系数
    public ObscuredFloat AI_SummonPropertyPro;          //召唤属性系数
    public ObscuredFloat AI_SummonPropertyActPro;        //召唤属性系数
    public ObscuredFloat AI_SummonPropertyDefPro;        //召唤属性系数

    public bool NoUpdateProperty;                     //不更新血量和攻击属性

    //怪物难度设定QQ
    public ObscuredFloat nanduValue_HP = 1;
    public ObscuredFloat nanduValue_Other = 1;

    //绑点专用
    private GameObject gameStartVar;
    private Game_PositionVar game_PositionVar;
    private AI_1 ai_1;
    private AIPet aiPet;
	// Use this for initialization

	void Start () {

        //初始化怪物属性
        if (this.gameObject.GetComponent<AI_1>() != null)
        {
            AI_ID = this.gameObject.GetComponent<AI_1>().AI_ID.ToString();
        }
        else {
            AI_ID = this.gameObject.GetComponent<AIPet>().AI_ID.ToString();
        }

        //默认为怪物
        if (AI_Type == "" || AI_Type == "0") {
            AI_Type = "1";
        }

        if (AI_Type == "1") {

            AI_Name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", AI_ID, "Monster_Template");
            if(AI_MonsterCreateType == "2"){
                AI_Name = "被封印的" + AI_Name;
            }

            if (NoUpdateProperty == false) {
                AI_Hp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Hp", "ID", AI_ID, "Monster_Template"));
                AI_Act = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Act", "ID", AI_ID, "Monster_Template"));
                AI_MageAct = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MageAct", "ID", AI_ID, "Monster_Template"));
                AI_Lv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", AI_ID, "Monster_Template"));
            }

            AI_Def = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Def", "ID", AI_ID, "Monster_Template"));
            AI_Adf = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Adf", "ID", AI_ID, "Monster_Template"));
            AI_MoveSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MoveSpeed", "ID", AI_ID, "Monster_Template"));
            AI_Cri = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Cri", "ID", AI_ID, "Monster_Template"));
            AI_Res = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Res", "ID", AI_ID, "Monster_Template"));
            AI_Hit = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Hit", "ID", AI_ID, "Monster_Template"));
            AI_Dodge = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Dodge", "ID", AI_ID, "Monster_Template"));
            AI_DefAdd = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DefAdd", "ID", AI_ID, "Monster_Template"));
            AI_DamgeAdd = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeAdd", "ID", AI_ID, "Monster_Template"));
            AI_ActSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActSpeed", "ID", AI_ID.ToString(), "Monster_Template"));
            AI_MonsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", AI_ID, "Monster_Template");


            //备份初级数据
            base_HpMax = AI_Hp;
            base_Act = AI_Act;
            base_MageAct = AI_MageAct;
            base_Def = AI_Def;
            base_Adf = AI_Adf;
            base_Cri = AI_Cri;
            base_Res = AI_Res;
            base_Hit = AI_Hit;
            base_Dodge = AI_Dodge;
            base_DamgeAdd = AI_DamgeAdd;
            base_MoveSpeed = AI_MoveSpeed;
        }


        //更新自身属性
        //updateAIProprety();

        //获取绑点
        gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
        game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();

        StartAIPropertyStatus = true;
        if (this.gameObject.GetComponent<AIPet>() != null) {
            this.gameObject.GetComponent<AIPet>().LastHp = LastHp;
        }

        //获取脚本
        if (this.GetComponent<AI_1>() != null)
        {
            ai_1 = this.GetComponent<AI_1>();

            UpdateMonsterType();

            //创建怪物特殊处理
            float mijingValue_HP = 1;
            float mijingValue_Act = 1;
            float mijingValue_Def = 1;

            switch (AI_MonsterCreateType)
            {

                //大秘境怪物
                case "1":
                    //读取大秘境的层数
                    string daMiJingLvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    if (daMiJingLvStr == "")
                    {
                        daMiJingLvStr = "1";
                    }

                    float damijingLv = float.Parse(daMiJingLvStr);
                    mijingValue_HP = Mathf.Pow(1.065f, damijingLv);
                    mijingValue_Act = Mathf.Pow(1.05f, damijingLv);
                    mijingValue_Def = Mathf.Pow(1.04f, damijingLv);

                    //防御特殊处理
                    if (mijingValue_Def > 3.5f)
                    {
                        mijingValue_Def = 3.5f;
                    }

                    AI_Lv = (int)(damijingLv);
                    AI_Hp = (int)(AI_Hp * mijingValue_HP);
                    AI_Act = (int)(AI_Act * mijingValue_Act);
                    AI_MageAct = (int)(AI_MageAct * mijingValue_Act);
                    AI_Def = (int)(AI_Def * mijingValue_Def);
                    AI_Adf = (int)(AI_Adf * mijingValue_Def);
                    AI_HpMax = AI_Hp;
                    ai_NanDuStatus = false;
                    NoUpdateProperty = true;
                    break;

                //封印之塔创建
                case "2":
                    ai_NanDuStatus = false;
                    break;
            }

            //活动场景没有怪物
            if (Application.loadedLevelName == "100001" || Application.loadedLevelName == "100002")
            {
                ai_NanDuStatus = false;
            }

            //难度(只有BOSS才会有难度)
            string ai_monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", AI_ID, "Monster_Template");
            if (ai_monsterType != "3") {
                ai_NanDuStatus = false;
            }

            //难度(副本BOSS没有难度)
            string ai_MonsterSonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterSonType", "ID", AI_ID, "Monster_Template");
            /*
            if (ai_MonsterSonType == "")
            {
                ai_NanDuStatus = false;
            }
            */

            if (ai_NanDuStatus)
            {
                ObscuredFloat nandu_hp_1 = 1;
                ObscuredFloat nandu_other_1 = 1;
                ObscuredFloat nandu_hp_2 = 2;
                ObscuredFloat nandu_other_2 = 1.5f;
                ObscuredFloat nandu_hp_3 = 4f;
                ObscuredFloat nandu_other_3 = 2.5f;

                switch (Game_PublicClassVar.Get_game_PositionVar.GameNanduValue)
                {
                    case "1":
                        nanduValue_HP = nanduValue_HP * nandu_hp_1;
                        nanduValue_Other = nanduValue_Other * nandu_other_1;
                        break;


                    case "2":
                        nanduValue_HP = nanduValue_HP * nandu_hp_2;
                        nanduValue_Other = nanduValue_Other * nandu_other_2;
                        string langstr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("挑战");
                        AI_Name = AI_Name + "("+ langstr + ")";
                        break;


                    case "3":
                        nanduValue_HP = nanduValue_HP * nandu_hp_3;
                        nanduValue_Other = nanduValue_Other * nandu_other_3;
                        langstr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("地狱");
                        AI_Name = AI_Name + "("+ langstr + ")";
                        break;
                }
            }

            if (NoUpdateProperty == false) {
                AI_Hp = (int)(nanduValue_HP * AI_Hp);
                //赋值其他属性
                AI_HpMax = AI_Hp;
                //Debug.Log("AI_HpMax222222222 = " + AI_HpMax);
                LastHp = AI_Hp;
                AI_Act = (int)(nanduValue_Other * AI_Act);
            }

            //Debug.Log("AI_HpMax = " + AI_HpMax + "AI_Hp = " + AI_Hp);

        }

        if (this.GetComponent<AIPet>() != null)
        {
            aiPet = this.GetComponent<AIPet>();
            //
        }


        if (this.GetComponent<AI_1>() != null) {
            if (this.GetComponent<AI_1>().AI_MonsterDeathStatus) {
                AI_Hp = 0;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

        //更新属性
        if (UpdataAIPropertyStatus) {
            UpdataAIPropertyStatus = false;
            updateAIProprety();
        }

        switch (AI_Type) { 
            case "1":
                //回血
                if (ai_1 != null)
                {
                    if (ai_1.AI_Status != "8")
                    {
                        huiXue();
                    }
                }
                break;

            case "2":
                //回血
                if (aiPet != null)
                {
                    if (aiPet.AI_Status != "8")
                    {
                        huiXue_Pet();
                    }
                }
                break;
        }



        /*
		AI_Animator = GetComponent<Animator>();

		//当AI死亡时
		if (AI_Hp <= 0) {
		
			AI_Animator.Play("Death");

			//创建掉落GameObject
			//GameObject drop = (GameObject)Instantiate(DropGameObject);
			//drop.transform.parent=game_PositionVar.GameObject_AI_Drop.transform;
			//drop.transform.position = this.transform.position;

			//30秒后销毁尸体
			Destroy(this.gameObject, 30);
			Destroy(this);

			//怪物死后创建一个可以拾取物品的碰撞体



            //怪物死后创建的一个掉落（此掉落为掉落2）
            Game_PublicClassVar.Get_function_AI.AI_MonsterDrop(AI_ID, this.transform.position);


            //怪物死亡给人物增加对应的经验值
            Game_PublicClassVar.Get_function_Rose.AddRoseExp("100");


            //是否为任务要求的怪物
            Function_Task function_Task = new Function_Task();
            function_Task.TaskMonsterNum("100001", 1);

            //卸载碰撞脚本
            //Destroy(GetComponent<CapsuleCollider>());
            //Destroy(GetComponent<CharacterController>());
            //Destroy(GetComponent<AI_Status>());
            //Destroy(GetComponent<AI_Property>());
            //Destroy(GetComponent<AI_1>(),0.05f);
            //GetComponent<CapsuleCollider>().enabled = false;
            //GetComponent<CharacterController>().enabled = false;
            GetComponent<AI_1>().enabled = false;
		}
        */
	}


    private void huiXue() {
        //目标死亡不回血
        if (AI_Hp <= 0) {
            return;
        }

        if (AI_Hp < AI_HpMax)
        {
            //正常回血
            ai_HealHpTimeSum = ai_HealHpTimeSum + Time.deltaTime;
            if (ai_HealHpTimeSum >= 5)
            {
                ObscuredFloat costValue = 0.01f;
                ObscuredInt healValue = (ObscuredInt)(AI_HpMax * (costValue + AI_HuiXuePro));
                AI_Hp = AI_Hp + healValue;
                if (AI_Hp > AI_HpMax)
                {
                    AI_Hp = AI_HpMax;
                }
                ai_HealHpTimeSum = 0;
            }
        }
    }

	//宠物回血
	private void huiXue_Pet() {
		//目标死亡不回血
		if (AI_Hp <= 0) {
			return;
		}

		if (AI_Hp < AI_HpMax)
		{
			//正常回血
			ai_HealHpTimeSum = ai_HealHpTimeSum + Time.deltaTime;
			if (ai_HealHpTimeSum >= 5)
			{
				int healValue = (int)(AI_HpMax * (0.05f + AI_HuiXuePro));
				AI_Hp = AI_Hp + healValue;
				if (AI_Hp > AI_HpMax)
				{
					AI_Hp = AI_HpMax;
				}
				ai_HealHpTimeSum = 0;

				//存储当前血量
				string rosePet_ID = this.GetComponent<AIPet>().RosePet_ID;
				Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetNowHp", AI_Hp.ToString(),"ID", rosePet_ID, "RosePet");
				Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");
                //更新主界面
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().PetUpHp();
            }
		}
	}

    //刷新属性
    public void updateAIProprety() {

        switch (AI_Type) { 
            //怪物属性更新
            case "1":
            if (SkillIDList.Length != 0) {
                //获取被动技能
                if (SkillIDList[0] != "" && SkillIDList[0] != "0")
                {
                    for (int i = 0; i < SkillIDList.Length; i++)
                    {
                        //Debug.Log("有技能进入循环，技能ID:" + SkillIDList[i]);
                        string gameObjectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectName", "ID", SkillIDList[i], "Skill_Template");
                        //Debug.Log("gameObjectName = " + gameObjectName);
                        //gameObjectName = "AddAIProPrety";
                        if (gameObjectName == "AddAIProprety")
                        {
                            //Debug.Log("AddAIProprety");
                            string[] gameObjectParameterList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", SkillIDList[i], "Skill_Template").Split(';');
                            for (int y = 0; y < gameObjectParameterList.Length; y++)
                            {
                                //获取增加宠物属性
                                //Debug.Log("Y = " + y + ";gameObjectParameterList[y] = " + gameObjectParameterList[y]);
                                string[] addPropretyList = gameObjectParameterList[y].Split(',');
                                string addPropretyType = addPropretyList[0];
                                string addProPretyValue = addPropretyList[1];
                                //Debug.Log("开始增加属性");
                                switch (addPropretyType)
                                {
                                    //循环获取怪物增加属性
                                    //攻击固定值
                                    case "11":
                                        AI_Act_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //攻击百分比
                                    case "12":
                                        ActMul_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //魔法攻击固定值
                                    case "15":
                                        AI_MagAct_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //魔法攻击百分比
                                    case "16":
                                        MageActMul_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //物防固定值
                                    case "21":
                                        AI_Def_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //物防百分比
                                    case "22":
                                        DefMul_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //魔防固定值
                                    case "31":
                                        AI_Adf_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //魔防百分比
                                    case "32":
                                        AdfMul_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //血量固定值
                                    case "41":
                                        AI_HpMax_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //血量百分比
                                    case "42":
                                        ActMul_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //暴击
                                    case "101":
                                        AI_Cri_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //韧性
                                    case "102":
                                        AI_Res_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //命中
                                    case "103":
                                        AI_Hit_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //闪避
                                    case "104":
                                        AI_Dodge_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //移动速度
                                    case "105":
                                        AI_MoveSpeed_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //物理免伤
                                    case "106":
                                        AI_DefAdd_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //魔法免伤
                                    case "107":
                                        AI_AdfAdd_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //伤害减免
                                    case "108":
                                        AI_DamgeAdd_Skill = float.Parse(addProPretyValue);
                                        break;
                                    //伤害减免
                                    case "109":
                                        AI_ActDamgeAdd_Add = float.Parse(addProPretyValue);
                                        break;
                                    //吸血值
                                    case "110":
                                        AI_XiXieValue_Add = float.Parse(addProPretyValue);
                                        break;
                                    //回血值
                                    case "111":
                                        AI_HuiXuePro_Add = float.Parse(addProPretyValue);
                                        break;
                                    //幸运
                                    case "112":
                                        AI_XingYunPro_Add = float.Parse(addProPretyValue);
                                        break;
                                    //仇恨值
                                    case "113":
                                        AI_ChouHenValue_Add = float.Parse(addProPretyValue);
                                        break;
                                    //概率抵消本次伤害
                                    case "114":
                                        AI_DiXiaoPro_Add = float.Parse(addProPretyValue);
                                        break;
                                    //复活概率
                                    case "115":
                                        AI_FuHuoPro_Add = float.Parse(addProPretyValue);
                                        break;
                                    //野兽攻击
                                    case "121":
                                        AI_raceDamge_1_Add = float.Parse(addProPretyValue);
                                        break;
                                    //野兽攻击
                                    case "122":
                                        AI_raceDamge_2_Add = float.Parse(addProPretyValue);
                                        break;
                                    //野兽攻击
                                    case "123":
                                        AI_raceDamge_3_Add = float.Parse(addProPretyValue);
                                        break;
                                    //野兽攻击
                                    case "124":
                                        AI_raceResistance_1_Add = float.Parse(addProPretyValue);
                                        break;
                                    //野兽攻击
                                    case "125":
                                        AI_raceResistance_2_Add = float.Parse(addProPretyValue);
                                        break;
                                    //野兽攻击
                                    case "126":
                                        AI_raceResistance_3_Add = float.Parse(addProPretyValue);
                                        break;
                                    //光抗性
                                    case "131":
                                        AI_resistance_1_Add = float.Parse(addProPretyValue);
                                        break;
                                    //暗抗性
                                    case "132":
                                        AI_resistance_2_Add = float.Parse(addProPretyValue);
                                        break;
                                    //火抗性
                                    case "133":
                                        AI_resistance_3_Add = float.Parse(addProPretyValue);
                                        break;
                                    //水抗性
                                    case "134":
                                        AI_resistance_4_Add = float.Parse(addProPretyValue);
                                        break;
                                    //电抗性
                                    case "135":
                                        AI_resistance_5_Add = float.Parse(addProPretyValue);
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            //显示属性
            AI_Adf = (int)((base_Adf + AI_Adf_Skill + AdfAdd) * (1 + AdfMul_Skill + AdfMul));
            AI_Def = (int)((base_Def + AI_Def_Skill + DefAdd) * (1 + DefMul_Skill + DefMul));
            AI_Cri = base_Cri + AI_Cri_Skill + AI_Cri_Add;
            AI_Res = base_Res + AI_Res_Skill + AI_Res_Add;
            AI_Hit = base_Hit + AI_Hit_Skill + AI_Hit_Add;
            AI_Dodge = base_Dodge + AI_Dodge_Skill + AI_Dodge_Add;
            AI_DefAdd = base_DefAdd + AI_Def_Skill;

            if (NoUpdateProperty == false)
            {
                AI_Act = (int)((base_Act + AI_Act_Skill + ActAdd) * (1 + ActMul_Skill + ActMul));
                AI_MageAct = (int)((base_MageAct + AI_MagAct_Skill + MageActAdd) * (1 + MageActMul_Skill + MageActMul));
                AI_HpMax = (int)((base_HpMax + AI_HpMax_Skill) * (1 + HpMaxMul_Skill));
            }

            AI_DamgeAdd = base_DamgeAdd  + DamgeAddMul;
            AI_ActDamgeAdd = AI_ActDamgeAdd_Add;
            AI_MoveSpeed = base_MoveSpeed * (1 + AI_MoveSpeed_Skill + MoveSpeedMul);


            AI_HuiXuePro =  AI_HuiXuePro_Add;                 //回血
            AI_XiXieValue = AI_XiXieValue_Add;           	//AI吸血值
            AI_XingYunPro = AI_XingYunPro_Add;				//幸运
            AI_DiXiaoPro = AI_DiXiaoPro_Add;				    //概率抵消本次伤害
            AI_ChouHenValue = AI_ChouHenValue_Add;		//仇恨值
            AI_FuHuoPro = AI_FuHuoPro_Add;                  //复活概率
            //Debug.Log("AI_FuHuoPro = " + AI_FuHuoPro);
            AI_resistance_1 = AI_resistance_1_Add;                        //光抗性
            AI_resistance_2 = AI_resistance_2_Add;                        //暗抗性
            AI_resistance_3 = AI_resistance_3_Add;                        //火抗性
            AI_resistance_4 = AI_resistance_4_Add;                        //水抗性
            AI_resistance_5 = AI_resistance_5_Add;                        //电抗性
            AI_raceDamge_1 = AI_raceDamge_1_Add;                           //野兽攻击伤害
            AI_raceDamge_2 = AI_raceDamge_2_Add;                           //人物攻击伤害
            AI_raceDamge_3 =  AI_raceDamge_3_Add;                           //恶魔攻击伤害
            AI_raceResistance_1 = AI_raceResistance_1_Add;            //野兽攻击抗性
            AI_raceResistance_2 = AI_raceResistance_2_Add;            //人物攻击抗性
            AI_raceResistance_3 = AI_raceResistance_3_Add;            //恶魔攻击抗性

            //更新难度属性
            if (NoUpdateProperty == false) {
                AI_HpMax = (int)(nanduValue_HP * AI_HpMax);
                AI_Act = (int)(nanduValue_Other * AI_Act);
            }

            

            if (this.GetComponent<AI_1>() != null) {
                this.GetComponent<AI_1>().AI_MoveSpeed = AI_MoveSpeed;
                this.GetComponent<AI_1>().ai_NavMesh.speed = AI_MoveSpeed;
            }

            break;

            case "2":
                //更新宠物信息
                if (this.GetComponent<AIPet>() != null) {
                    if (this.GetComponent<AIPet>().IfSkillCreatePet_ProTeShu == false)
                    {
                        //Debug.Log("更新属性222222");
                        Game_PublicClassVar.Get_function_AI.UpdatePetProperty(this.gameObject, this.GetComponent<AIPet>().PetType);
                    }
                    else {
                        //Debug.Log("更新属性333333");
                        Game_PublicClassVar.Get_function_AI.UpdatePetProperty(this.gameObject, "4");
                    }

                }

            break;

        }


        if (AI_Hp >= AI_HpMax)
        {
            AI_Hp = AI_HpMax;
        }

    }

    //更新怪物属性
    public void UpdateMonsterType() {

        if (AI_MonsterType != "3" && AI_MonsterType != "4")
        {
            AI_Name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", AI_ID, "Monster_Template");

            ObscuredFloat nanduHp_1 = 0.5f;
            ObscuredFloat nanduHp_2 = 1.25f;
            ObscuredFloat nanduHp_3 = 1.5f;

            ObscuredFloat nanduRand_1 = 0.02f;
            ObscuredFloat nanduRand_2 = 0.03f;
            ObscuredFloat nanduRand_3 = 0.04f;


            //设定怪物值
            ObscuredFloat randValue = Random.value;
            //虚弱的
            if (randValue <= nanduRand_1)
            {
                string langstr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("虚弱的");
                AI_Name = langstr + AI_Name;
                nanduValue_HP = nanduValue_HP * nanduHp_1;
                AI_TypeSon = "1";
            }

            //强壮的
            if (randValue > nanduRand_1 && randValue <= nanduRand_2)
            {
                string langstr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("强壮的");
                AI_Name = langstr + AI_Name;
                nanduValue_HP = nanduValue_HP * nanduHp_2;
                AI_TypeSon = "2";
            }

            //头领的
            if (randValue > nanduRand_2 && randValue <= nanduRand_3)
            {
                string langstr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("头领");
                AI_Name = AI_Name + langstr;
                nanduValue_HP = nanduValue_HP * nanduHp_3;
                nanduValue_Other = nanduValue_Other * nanduHp_3;
                AI_TypeSon = "3";
            }
        }
    }
}
