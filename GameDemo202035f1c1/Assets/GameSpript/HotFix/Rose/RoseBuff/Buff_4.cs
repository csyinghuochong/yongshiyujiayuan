using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

//增加和减少属性的BUFF
public class Buff_4 : MonoBehaviour {

    public ObscuredString BuffID;                   //BuffID
    private GameObject SkillEffect;         //技能特效
    private ObscuredFloat buffTime;                 //技能特效播放时间
    public ObscuredFloat buffTimeSum;              //技能持续时间累计值
    private ObscuredFloat damgePro;                 //攻击转换百分百
    private ObscuredInt damgeValue;                 //固定值
    private string[] continuedTime;            //2次持续扣血时间的间隔
    private ObscuredFloat continuedTimeSum;         //2次间隔时间的累计时间
    private ObscuredString ifImmediatelyUse;        //是否立即释放
    private ObscuredString targetType;              //目标类型
    private ObscuredString buffClass;
    private GameObject effect;
    private Rose_Proprety roseProprety;
    private Rose_Status roseStatus;

    public ObscuredString propretyType;            //属性类型
    public ObscuredString propretyAddType;         //属性加成类型  1：固定值 2：百分比
    public ObscuredString propretyAddValue;        //改变属性的值,支持负号
    public ObscuredString propretyAddValueSave;
    private ObscuredInt propretyValue_1;
    private ObscuredInt propretyValue_2;
    private ObscuredString buffBenefitType;
    public ObscuredFloat buffcheckTime;
    //绑点专用
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {




        //BuffID = "90010004";
        //获取绑点
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;

        roseProprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        roseStatus = game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();

        //Debug.Log("Buff4触发的ID：" + BuffID);
        //获取Buff参数
        continuedTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffParameter", "ID", BuffID, "SkillBuff_Template").Split(',');
        propretyType = continuedTime[0];
        propretyAddType = continuedTime[1];
        propretyAddValue = continuedTime[2];
        propretyAddValueSave = propretyAddValue;
        //获取治疗值
        damgePro = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgePro", "ID", BuffID, "SkillBuff_Template"));
        damgeValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", BuffID, "SkillBuff_Template"));

        //获取Buff持续时间
        buffTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffTime", "ID", BuffID, "SkillBuff_Template"));

        //获取技能增减益类型,设定buff时间
        buffBenefitType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffBenefitType", "ID", BuffID, "SkillBuff_Template");

        buffClass = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffAddClass", "ID", BuffID, "SkillBuff_Template");

        //获取技能目标
        targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", BuffID, "SkillBuff_Template");

        //增益
        if (buffBenefitType == "1") {
            buffTime = buffTime * (1 + roseProprety.Rose_BuffTimeAddPro);
        }

        //减益 且 必须是对玩家释放
        if (buffBenefitType == "2" && targetType== "1") {
            buffTime = buffTime * (1 - roseProprety.Rose_DeBuffTimeCostPro);
        }

        if (buffTime < 0) {
            buffTime = 0;
        }

        //buffTime = 100;
        //获取Buff是否立即释放
        ifImmediatelyUse = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfImmediatelyUse", "ID", BuffID, "SkillBuff_Template");



        //播放Buff特效
        string ifPlayEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfPlayEffect", "ID", BuffID, "SkillBuff_Template");
        if (ifPlayEffect == "1")
        {
            //获取特效名称
            string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", BuffID, "SkillBuff_Template");
            //获取播放点
            string effectPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectPosition", "ID", BuffID, "SkillBuff_Template");
            //实例化技能特效
            if (effectName != "0") {
                //Debug.Log("开始实例化技能");
                GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
                effect = (GameObject)Instantiate(SkillEffect);
                effect.SetActive(false);
                //根据Buff目标绑定不同的位置
                //玩家
                if (targetType == "1")
                {
                    Rose_Bone bone = this.GetComponent<Rose_Bone>();
                    Transform effectTra = bone.BoneSet.transform.Find(effectPosition).transform;
                    effect.transform.parent = effectTra;
                }
                //怪物
                if (targetType == "2")
                {
                    AI_1 bone = this.GetComponent<AI_1>();
                    Transform effectTra = bone.BoneSet.transform.Find(effectPosition).transform;
                    effect.transform.parent = effectTra;
                }
                //宠物
                if (targetType == "3")
                {
                    //Debug.Log("BuffID：" + BuffID);
                    AIPet bone = this.GetComponent<AIPet>();
                    Transform effectTra = bone.BoneSet.transform.Find(effectPosition).transform;
                    effect.transform.parent = effectTra;
                }

                effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                effect.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
                effect.SetActive(true);
            }
        }

        //触发一次Buff
        buffUse();
		//显示Icon
		string ifShowIconTips = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfShowIconTips", "ID", BuffID, "SkillBuff_Template");
		if(ifShowIconTips == "1"){
			//触发显示
			//Debug.Log("吃吃吃吃吃吃吃吃吃吃吃吃吃吃吃吃吃吃吃");
			Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuffListShowSet.GetComponent<UI_BuffShowListSet>().AddBuff(BuffID);
            //Debug.Log("光环1111111111111111111！！！！！！！");
		}
        checkBuff();
    }
	
	// Update is called once per frame
	void Update () {
        
        buffTimeSum = buffTimeSum + Time.deltaTime;
        /*
        continuedTimeSum = continuedTimeSum + Time.deltaTime;
        if (continuedTimeSum >= continuedTime) {
            continuedTimeSum = 0;
            //触发一次Buff
            buffUse();
        }
        */

        //循环检自身是否有同类效果,有同类效果 自己值为0
        /*
        if (buffClass == "3") {

            Buff_4[] buff_4 = this.GetComponents<Buff_4>();
            bool ifAddBuffStatus = false;
            //Debug.Log("buff_4长度" + buff_4.Length);
            
            for (int i = 0; i <= buff_4.Length - 1; i++)
            {
                string buffAddClass = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffAddClass", "ID", buff_4[i].BuffID, "SkillBuff_Template");
                //if(buffAddClass!=null){
                if (buff_4[i].propretyType == propretyType)
                {
                    if (buff_4[i] != this) {
                        ifAddBuffStatus = true;
                    }
                }
            }
            
            if (ifAddBuffStatus)
            {
                propretyAddValueSave = propretyAddValue;
                propretyAddValue = "0";
            }
            else {
                propretyAddValue = propretyAddValueSave;
            }
        }
        */




        //获取绑定对象的生命是否为0，为0删除自身脚本及特效
        bool ifdeath = Game_PublicClassVar.Get_function_Skill.ifDeath(targetType,this.gameObject);
        //角色死亡不销毁的buff(宠物光环)
        string guanghuaiBuffIDStr = "90040210,90040220,90040230,90041210,90041220,90041230";
        string[] guanghuaiBuffIDList = guanghuaiBuffIDStr.Split(',');
        for (int i = 0; i < guanghuaiBuffIDList.Length; i++)
        {
            if (BuffID == guanghuaiBuffIDList[i])
            {
                ifdeath = false;
            }
        }

        //注销自己的脚本
        if (buffTimeSum > buffTime)
        {
            /*
            //注销特效
            if (effect != null)
            {
                Destroy(effect);
            }
            //注销自己  
            Destroy(this);
            */
            ifdeath = true;
        }


        if (ifdeath)
        {
            if (effect != null)
            {
                Destroy(effect);
            }
            Destroy(this);
        }


        //检测角色是否脱离战斗，如果脱离战斗减益buff将取消
        if (buffBenefitType == "2") {
            if (!roseStatus.RoseFightStatus) {
                Destroy(this);
            }
        }
        //在主城的时候销毁减益buff
        if (buffBenefitType == "2" && Application.loadedLevelName == "EnterGame") {
            Destroy(this);
        }

        //1秒检测一次
        buffcheckTime = buffcheckTime + Time.deltaTime;
        if (buffcheckTime >= 1) {
            buffcheckTime = 0;
            checkBuff();
        }

    }

    //销毁脚本时调用
    void OnDestroy() {

        //Debug.Log("销毁BUFF...");

        //恢复属性
        propretyBuff(propretyType, propretyAddType, propretyAddValue, targetType, true);
        //注销特效
        if (effect != null) {
            Destroy(effect);
            //更新AI属性
            AI_Property aiProprety = this.gameObject.GetComponent<AI_Property>();
            if (aiProprety != null) {
                aiProprety.UpdataAIPropertyStatus = true;
            }
        }
        //Debug.Log(this.gameObject.name + "脚本销毁了！");

        //销毁Buff
        try
        {
            if (Game_PublicClassVar.Get_game_PositionVar != null && Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>() != null && Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuffListShowSet != null)
            {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuffListShowSet.GetComponent<UI_BuffShowListSet>().DelBuff(BuffID);
                //Debug.Log("光环222222222222222222222222！！！！！！！");
            }
        }
        catch {
            Debug.Log("销毁buff_4报错...");
        }

    }

    //触发一次Buff效果
    void buffUse() {

        propretyBuff(propretyType, propretyAddType, propretyAddValue, targetType, false);
        if (damgeValue != 0) {
            //玩家自己触发伤害及眩晕
            if (targetType == "1")
            {
                Game_PublicClassVar.Get_function_Rose.costRoseHp(damgeValue);
                //Debug.Log("触发扣血");

            }
            //怪物加血
            if (targetType == "2")
            {
                Game_PublicClassVar.Get_function_AI.AI_costHp(this.gameObject, damgeValue);
                //Game_PublicClassVar.Get_function_Skill.XuanYunBuff(buffTime, this.gameObject, "2");
            }

            //宠物加血
            if (targetType == "3")
            {
                Game_PublicClassVar.Get_function_AI.AI_costHp(this.gameObject, damgeValue, "2");
                //Game_PublicClassVar.Get_function_Skill.XuanYunBuff(buffTime, this.gameObject, "2");
            }
        }
    }

    private void checkBuff() {

        Buff_4[] buff_4List = this.GetComponents<Buff_4>();
        string guanghuaiBuffIDStr = "90040210,90040211,90040220,90040221,90040230,90040231,90041210,90041211,90041220,90041221,90041230,90041231";
        string[] guanghuaiBuffIDList = guanghuaiBuffIDStr.Split(',');
        for (int i = 0; i < guanghuaiBuffIDList.Length; i++)
        {

            if (BuffID == guanghuaiBuffIDList[i])
            {

                for (int y = 0; y < buff_4List.Length; y++)
                {
                    if (buff_4List[y].BuffID == BuffID && buff_4List[y]!=this)
                    {
                        //Debug.Log("销毁相同buff");
                        Destroy(this);
                    }
                }
            }
        }

    }

    //触发一次Buff
    void propretyBuff(string propretyBuffType, string propretyBuffAddType, string propretyBuffValue, string targetType,bool ifExitBuff) {
        
        int buffValue_add = 0;
        float buffValue_mul = 0;

        switch (propretyBuffAddType)
        {
            //固定值
            case "1":
                if (propretyBuffValue != "" && propretyBuffValue != null)
                {
                    buffValue_add = (int)float.Parse(propretyBuffValue);
                }
            break;

            //百分比
            case "2":
                if (propretyBuffValue != "" && propretyBuffValue != null)
                {
                    buffValue_mul = float.Parse(propretyBuffValue);
                }
            break;
        }

        //buffValue_ComValue 通用的值不管类型为1还是2,都读取对应的
        float buffValue_ComValue = 0;
        if (buffValue_add != 0)
        {
            buffValue_ComValue = buffValue_add;
        }
        else {
            buffValue_ComValue = buffValue_mul;
        }
        
        //退出Buff时,把参数都设置为配置的负数
        if (ifExitBuff) {
            buffValue_add = buffValue_add * -1;
            buffValue_mul = buffValue_mul * -1;
            buffValue_ComValue = buffValue_ComValue * -1;
        }

        //玩家
        if (targetType == "1")
        {
            
            roseProprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
            switch (propretyBuffType) { 

                case "10":

                    switch (propretyBuffAddType)
                    {
                        //固定值
                        case "1":
                            roseProprety.Rose_HpAdd_2 = roseProprety.Rose_HpAdd_2 + buffValue_add;
                            break;
                        //百分比
                        case "2":
                            roseProprety.Rose_HpMul_1 = roseProprety.Rose_HpMul_1 + buffValue_mul;
                            break;
                    }

                    break;

                //物理攻击
                case "11":
                    switch (propretyBuffAddType)
                    {
                        //固定值
                        case "1":
                            roseProprety.Rose_ActMinAdd_2 = roseProprety.Rose_ActMinAdd_2 + buffValue_add;
                            roseProprety.Rose_ActMaxAdd_2 = roseProprety.Rose_ActMaxAdd_2 + buffValue_add;
                        break;
                        //百分比
                        case "2":
                            roseProprety.Rose_ActMinMul_1 = roseProprety.Rose_ActMinMul_1 + buffValue_mul;
                            roseProprety.Rose_ActMaxMul_1 = roseProprety.Rose_ActMaxMul_1 + buffValue_mul;
                        break;
                    }
                break;

                //魔法攻击
                case "16":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_MagActMinAdd_2 = roseProprety.Rose_MagActMinAdd_2 + buffValue_add;
                        roseProprety.Rose_MagActMaxAdd_2 = roseProprety.Rose_MagActMaxAdd_2 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_MagActMinMul_1 = roseProprety.Rose_MagActMinMul_1 + buffValue_mul;
                        roseProprety.Rose_MagActMaxMul_1 = roseProprety.Rose_MagActMaxMul_1 + buffValue_mul;
                        break;
                }
                break;

                //物理防御
                case "17" :
                    switch (propretyBuffAddType)
                    {
                        //固定值
                        case "1":
                            roseProprety.Rose_DefMinAdd_2 = roseProprety.Rose_DefMinAdd_2 + buffValue_add;
                            roseProprety.Rose_DefMaxAdd_2 = roseProprety.Rose_DefMaxAdd_2 + buffValue_add;
                        break;
                        //百分比
                        case "2":
                            roseProprety.Rose_DefMinMul_1 = roseProprety.Rose_DefMinMul_1 + buffValue_mul;
                            roseProprety.Rose_DefMaxMul_1 = roseProprety.Rose_DefMaxMul_1 + buffValue_mul;
                        break;
                    }
                break;

                //魔法防御
                case "20":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_AdfMinAdd_2 = roseProprety.Rose_AdfMinAdd_2 + buffValue_add;
                        roseProprety.Rose_AdfMaxAdd_2 = roseProprety.Rose_AdfMaxAdd_2 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_AdfMinMul_1 = roseProprety.Rose_AdfMinMul_1 + buffValue_mul;
                        roseProprety.Rose_AdfMaxMul_1 = roseProprety.Rose_AdfMaxMul_1 + buffValue_mul;
                        break;
                }
                break;


                //暴击
                case "30":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_CriMul_1 = roseProprety.Rose_CriMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_CriMul_1 = roseProprety.Rose_CriMul_1 + buffValue_mul;
                        break;
                }
                break;

                //命中
                case "31":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_HitMul_1 = roseProprety.Rose_HitMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_HitMul_1 = roseProprety.Rose_HitMul_1 + buffValue_mul;
                        break;
                }
                break;

                //闪避
                case "32":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_DodgeMul_1 = roseProprety.Rose_DodgeMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_DodgeMul_1 = roseProprety.Rose_DodgeMul_1 + buffValue_mul;
                        break;
                }
                break;

                //物理免伤
                case "33":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_DefMul_1 = roseProprety.Rose_DefMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_DefMul_1 = roseProprety.Rose_DefMul_1 + buffValue_mul;
                        break;
                }
                break;

                //魔法免伤
                case "34":
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_AdfMul_1 = roseProprety.Rose_AdfMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_AdfMul_1 = roseProprety.Rose_AdfMul_1 + buffValue_mul;
                        break;
                }
                break;

                //速度
                case "35":
                //Debug.Log("开始出发加速效果");
                switch (propretyBuffAddType) { 
                    //固定值
                    case "1":
                        roseProprety.Rose_MoveSpeedAdd_2 = roseProprety.Rose_MoveSpeedAdd_2 + buffValue_add;
                    break;
                    //百分比
                    case "2":
                        roseProprety.Rose_MoveSpeedMul_1 = roseProprety.Rose_MoveSpeedMul_1 + buffValue_mul;
                    break;
                }
                break;

                //伤害免伤
                case "36":
                //Debug.Log("Rose_DamgeSubtractMul_1 == " + roseProprety.Rose_DamgeSubtractMul_1 + "buffValue_add = " + buffValue_add);
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_DamgeSubtractMul_1 = roseProprety.Rose_DamgeSubtractMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_DamgeSubtractMul_1 = roseProprety.Rose_DamgeSubtractMul_1 + buffValue_mul;
                        break;
                }
                break;

                //伤害加成
                case "37":
                //Debug.Log("Rose_DamgeSubtractMul_1 == " + roseProprety.Rose_DamgeSubtractMul_1 + "buffValue_add = " + buffValue_add);
                switch (propretyBuffAddType)
                {
                    //固定值
                    case "1":
                        roseProprety.Rose_DamgeAddMul_1 = roseProprety.Rose_DamgeAddMul_1 + buffValue_add;
                        break;
                    //百分比
                    case "2":
                        roseProprety.Rose_DamgeAddMul_1 = roseProprety.Rose_DamgeAddMul_1 + buffValue_mul;
                        break;
                }
                break;


                //血量直接扣除
                case "50":
                    //Debug.Log("Rose_DamgeSubtractMul_1 == " + roseProprety.Rose_DamgeSubtractMul_1 + "buffValue_add = " + buffValue_add);
                    if (!ifExitBuff) {
                        switch (propretyBuffAddType)
                        {
                            //固定值
                            case "1":
                                roseProprety.Rose_HpNow = roseProprety.Rose_HpNow - buffValue_add;
                                break;
                            //百分比
                            case "2":
                                Debug.Log("调用buff百分比扣血~");
                                roseProprety.Rose_HpNow = (int)(roseProprety.Rose_HpNow - (roseProprety.Rose_Hp * buffValue_mul));
                                break;
                        }
                    }
                    break;


                //100.幸运值
                case "100":
                    roseProprety.Rose_Luck_Add = roseProprety.Rose_Luck_Add + (int)buffValue_ComValue;
                break;

                //101：格挡值
                case "101":
                    roseProprety.Rose_GeDangValue_Add = roseProprety.Rose_GeDangValue_Add + (int)buffValue_ComValue;
                break;

                //111：重击概率
                case "111":
                    roseProprety.Rose_ZhongJiPro_Add = roseProprety.Rose_ZhongJiPro_Add + buffValue_ComValue;
                break;

                //112:  重击附加伤害值
                case "112":
                    roseProprety.Rose_ZhongJiValue_Add = roseProprety.Rose_ZhongJiValue_Add + (int)buffValue_ComValue;
                break;

                //121:  每次普通攻击附加的伤害值
                case "121":
                    roseProprety.Rose_GuDingValue_Add = roseProprety.Rose_GuDingValue_Add + (int)buffValue_ComValue;
                break;

                //131:  忽视目标防御值   
                case "131":
                    roseProprety.Rose_HuShiDefValue_Add = roseProprety.Rose_HuShiDefValue_Add + (int)buffValue_ComValue;
                break;

                //132:  忽视目标魔防值
                case "132":
                    roseProprety.Rose_HuShiAdfValue_Add = roseProprety.Rose_HuShiAdfValue_Add + (int)buffValue_ComValue;
                break;

                //131:  忽视目标防御值   
                case "133":
                    roseProprety.Rose_HuShiDefValuePro_Add = roseProprety.Rose_HuShiDefValuePro_Add + buffValue_ComValue;
                break;

                //132:  忽视目标魔防值
                case "134":
                    roseProprety.Rose_HuShiAdfValuePro_Add = roseProprety.Rose_HuShiAdfValuePro_Add + buffValue_ComValue;
                break;

                //141:  吸血概率
                case "141":
                    roseProprety.Rose_XiXuePro_Add = roseProprety.Rose_XiXuePro_Add + buffValue_ComValue;
                break;

                //法术反击
                case "151":
                    roseProprety.Rose_MagicRebound_Add = roseProprety.Rose_MagicRebound_Add +buffValue_ComValue;
                break;

                //攻击反击
                case "152":
                    roseProprety.Rose_ActRebound_Add = roseProprety.Rose_ActRebound_Add + buffValue_ComValue;
                break;

                //韧性概率
                case "161":
                    roseProprety.Rose_ResilienceRatingMul_1 = roseProprety.Rose_ResilienceRatingMul_1 + buffValue_ComValue;
                break;

                //暴击等级
                case "201":
                    roseProprety.Rose_CriRating_Add = roseProprety.Rose_GeDangValue_Add + (int)buffValue_ComValue;
                break;

                //韧性等级
                case "202":
                    roseProprety.Rose_ResilienceRating_Add = roseProprety.Rose_ResilienceRating_Add + (int)buffValue_ComValue;
                break;

                //命中等级
                case "203":
                    roseProprety.Rose_HitRating_Add = roseProprety.Rose_HitRating_Add + (int)buffValue_ComValue;
                break;

                //闪避等级
                case "204":
                    roseProprety.Rose_DodgeRating_Add = roseProprety.Rose_DodgeRating_Add + (int)buffValue_ComValue;
                break;

                //光抗性
                case "301":
                    roseProprety.Rose_Resistance_1_Add = roseProprety.Rose_Resistance_1_Add + buffValue_ComValue;
                break;

                //暗抗性
                case "302":
                    roseProprety.Rose_Resistance_2_Add = roseProprety.Rose_Resistance_2_Add + buffValue_ComValue;
                break;

                //火抗性
                case "303":
                    roseProprety.Rose_Resistance_3_Add = roseProprety.Rose_Resistance_3_Add + buffValue_ComValue;
                break;

                //水抗性
                case "304":
                    roseProprety.Rose_Resistance_4_Add = roseProprety.Rose_Resistance_4_Add + buffValue_ComValue;
                break;

                //电抗性
                case "305":
                    roseProprety.Rose_Resistance_5_Add = roseProprety.Rose_Resistance_5_Add + buffValue_ComValue;
                break;

                //野兽攻击抗性
                case "321":
                    roseProprety.Rose_RaceResistance_1_Add = roseProprety.Rose_RaceResistance_1_Add + buffValue_ComValue;
                break;

                //人物攻击抗性
                case "322":
                    roseProprety.Rose_RaceResistance_2_Add = roseProprety.Rose_RaceResistance_2_Add + buffValue_ComValue;
                break;

                //恶魔攻击抗性
                case "323":
                    roseProprety.Rose_RaceResistance_3_Add = roseProprety.Rose_RaceResistance_3_Add + buffValue_ComValue;
                break;

                //野兽攻击伤害
                case "331":
                    roseProprety.Rose_RaceDamge_1_Add = roseProprety.Rose_RaceDamge_1_Add + buffValue_ComValue;
                break;

                //人物攻击伤害
                case "332":
                    roseProprety.Rose_RaceDamge_2_Add = roseProprety.Rose_RaceDamge_2_Add + buffValue_ComValue;
                break;

                //恶魔攻击伤害
                case "333":
                    roseProprety.Rose_RaceDamge_3_Add = roseProprety.Rose_RaceDamge_3_Add + buffValue_ComValue;
                break;

                //Boss普通攻击加成
                case "341":
                    roseProprety.Rose_Boss_ActAdd_Add = roseProprety.Rose_Boss_ActAdd_Add + buffValue_ComValue;
                break;

                //Boss技能攻击加成
                case "342":
                    roseProprety.Rose_Boss_SkillAdd_Add = roseProprety.Rose_Boss_SkillAdd_Add + buffValue_ComValue;
                break;

                //受到Boss普通攻击减免
                case "343":
                    roseProprety.Rose_Boss_ActHitCost_Add = roseProprety.Rose_Boss_ActHitCost_Add + buffValue_ComValue;
                break;

                //受到Boss技能攻击减免
                case "344":
                    roseProprety.Rose_Boss_SkillHitCost_Add = roseProprety.Rose_Boss_SkillHitCost_Add + buffValue_ComValue;
                break;

                //宠物攻击加成
                case "345":
                    roseProprety.Rose_PetActAdd_Add = roseProprety.Rose_PetActAdd_Add + buffValue_ComValue;
                break;

                //宠物受伤减免
                case "346":
                    roseProprety.Rose_PetActHitCost_Add = roseProprety.Rose_PetActHitCost_Add + buffValue_ComValue;
                break;

                //Debuff时间缩短
                case "349":
                    roseProprety.Rose_DeBuffTimeCostPro_Add = roseProprety.Rose_DeBuffTimeCostPro_Add + buffValue_ComValue;
                    break;

                //经验加成
                case "401":
                    roseProprety.Rose_Exp_AddPro_Add = roseProprety.Rose_Exp_AddPro_Add + buffValue_ComValue;
                break;

                //金币加成
                case "402":
                    roseProprety.Rose_Gold_AddPro_Add = roseProprety.Rose_Gold_AddPro_Add + buffValue_ComValue;
                break;

                //洗炼极品掉落（祝福值）
                case "403":
                    roseProprety.Rose_Blessing_AddPro_Add = roseProprety.Rose_Blessing_AddPro_Add + buffValue_ComValue;
                break;

                //装备隐藏属性出现概率
                case "404":
                    roseProprety.Rose_HidePro_AddPro_Add = roseProprety.Rose_HidePro_AddPro_Add + buffValue_ComValue;
                break;

                //装备上的宝石槽位出现概率
                case "405":
                    roseProprety.Rose_GemHole_AddPro_Add = roseProprety.Rose_GemHole_AddPro_Add + buffValue_ComValue;
                break;
                //经验加成固定
                case "406":
                    roseProprety.Rose_Exp_AddValue_Add = roseProprety.Rose_Exp_AddValue_Add + (int)buffValue_ComValue;
                break;
                //金币加成固定
                case "407":
                    roseProprety.Rose_Gold_AddValue_Add = roseProprety.Rose_Gold_AddValue_Add + (int)buffValue_ComValue;
                break;
                //药剂类熟练度
                case "408":
                    roseProprety.Rose_YaoJiShuLian_Value_Add = roseProprety.Rose_YaoJiShuLian_Value_Add + (int)buffValue_ComValue;
                break;
                //锻造类熟练度
                case "409":
                    roseProprety.Rose_DuanZuaoShuLian_Value_Add = roseProprety.Rose_DuanZuaoShuLian_Value_Add + (int)buffValue_ComValue;
                break;
                //复活
                case "411":
                    roseProprety.Rose_FuHuoPro_Add = roseProprety.Rose_FuHuoPro_Add + buffValue_ComValue;
                break;

                //攻击无视防御
                case "412":
                    roseProprety.Rose_ActWuShi_Add = roseProprety.Rose_ActWuShi_Add + buffValue_ComValue;
                break;
                //神农
                case "413":
                    roseProprety.Rose_ShenNong_Add = roseProprety.Rose_ShenNong_Add + buffValue_ComValue;
                break;
                //额外掉落
                case "414":
                    roseProprety.Rose_DropExtra_Add = roseProprety.Rose_DropExtra_Add + buffValue_ComValue;
                break;
                //伪装
                case "415":
                    roseProprety.Rose_WeiZhuang_Add = roseProprety.Rose_WeiZhuang_Add + buffValue_ComValue;
                break;
                //灾难
                case "416":
                    roseProprety.Rose_ZaiNanValue_Add = roseProprety.Rose_ZaiNanValue_Add + buffValue_ComValue;
                break;
                //嗜血
                case "417":
                    roseProprety.Rose_ShiXuePro_Add = roseProprety.Rose_ShiXuePro_Add + buffValue_ComValue;
                break;
                //怪物脱战距离
                case "418":
                roseProprety.Rose_AITuoZhanDisValue_Add = roseProprety.Rose_AITuoZhanDisValue_Add + buffValue_ComValue;
                break;

                //专注概率
                case "419":
                roseProprety.Rose_ZhuanZhuPro_Add = roseProprety.Rose_ZhuanZhuPro_Add + buffValue_ComValue;
                break;

                //必中
                case "420":
                roseProprety.Rose_BiZhongPro_Add = roseProprety.Rose_BiZhongPro_Add + buffValue_ComValue;
                break;

                //生产药剂暴击概率
                case "421":
                roseProprety.Rose_YaoJiCirPro_Add = roseProprety.Rose_YaoJiCirPro_Add + buffValue_ComValue;
                break;

                //捕捉概率
                case "422":
                roseProprety.Rose_BuZhuoPro = roseProprety.Rose_BuZhuoPro + buffValue_ComValue;
                break;

                //特殊类
                //仇恨值
                case "901":
                    roseProprety.Rose_ChouHenValue_Add = roseProprety.Rose_ChouHenValue_Add + buffValue_ComValue;
                break;
            }

            //调整完属性时,更新当前角色属性
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseBuffProperty = true;
        }
        //怪物
        if (targetType == "2")
        {
            AI_Property aiProprety = this.gameObject.GetComponent<AI_Property>();
            aiProprety.UpdataAIPropertyStatus = true;
            switch (propretyBuffType)
            {
                //攻击
                case "11":
                    aiProprety.ActMul = aiProprety.ActMul + buffValue_mul;
                    aiProprety.ActAdd = aiProprety.ActAdd + buffValue_add;
                    break;

                //魔法
                case "16":
                    aiProprety.MageActMul = aiProprety.MageActMul + buffValue_mul;
                    aiProprety.MageActAdd = aiProprety.MageActAdd + buffValue_add;
                    break;

                //物理防御
                case "17":
                    aiProprety.DefMul = aiProprety.DefMul + buffValue_mul;
                    aiProprety.DefAdd = aiProprety.DefAdd + buffValue_add;
                    break;

                //魔法防御
                case "20":
                    aiProprety.AdfMul = aiProprety.AdfMul + buffValue_mul;
                    aiProprety.DefAdd = aiProprety.DefAdd + buffValue_add;
                    break;

                //闪避
                case "29":
                    aiProprety.AI_Res_Add = aiProprety.AI_Res_Add + buffValue_mul;
                    break;

                //暴击
                case "30":
                    aiProprety.AI_Cri_Add = aiProprety.AI_Cri_Add + buffValue_mul;
                    break;

                //命中
                case "31":
                    aiProprety.AI_Hit_Add = aiProprety.AI_Hit_Add + buffValue_mul;
                    break;

                //闪避
                case "32":
                    aiProprety.AI_Dodge_Add = aiProprety.AI_Dodge_Add + buffValue_mul;
                    break;

                //物理免伤
                case "33":
                    aiProprety.DefMul = aiProprety.DefMul + buffValue_mul;
                    break;

                //魔法免伤
                case "34":
                    aiProprety.AdfMul = aiProprety.AdfMul + buffValue_mul;
                    break;

                //速度
                case "35":
                    aiProprety.MoveSpeedMul = aiProprety.MoveSpeedMul + buffValue_mul;
                    break;

                //伤害免伤
                case "36":
                    aiProprety.DamgeAddMul = aiProprety.DamgeAddMul + buffValue_mul;
                    break;
            }
        }

        //宠物
        if (targetType == "3")
        {
            AI_Property aiProprety = this.gameObject.GetComponent<AI_Property>();
            aiProprety.UpdataAIPropertyStatus = true;
            switch (propretyBuffType)
            {
                //攻击
                case "11":
                    aiProprety.ActMul = aiProprety.ActMul + buffValue_mul;
                    aiProprety.ActAdd = aiProprety.ActAdd + buffValue_add;
                    break;

                //魔法
                case "16":
                    aiProprety.MageActMul = aiProprety.MageActMul + buffValue_mul;
                    aiProprety.MageActAdd = aiProprety.MageActAdd + buffValue_add;
                    break;

                //物理防御
                case "17":
                    aiProprety.DefMul = aiProprety.DefMul + buffValue_mul;
                    aiProprety.DefAdd = aiProprety.DefAdd + buffValue_add;
                    break;

                //魔法防御
                case "20":
                    aiProprety.AdfMul = aiProprety.AdfMul + buffValue_mul;
                    aiProprety.AdfAdd = aiProprety.AdfAdd + buffValue_add;
                    break;

                //韧性
                case "29":
                    aiProprety.AI_Res_Add = aiProprety.AI_Res_Add + buffValue_mul;
                    break;

                //暴击
                case "30":
                    aiProprety.AI_Cri_Add = aiProprety.AI_Cri_Add + buffValue_mul; 
                    break;

                //命中
                case "31":
                    aiProprety.AI_Hit_Add = aiProprety.AI_Hit_Add + buffValue_mul; 
                    break;

                //闪避
                case "32":
                    aiProprety.AI_Dodge_Add = aiProprety.AI_Dodge_Add + buffValue_mul; 
                    break;

                //物理免伤
                case "33":
                    aiProprety.DefMul = aiProprety.DefMul + buffValue_mul;
                    break;

                //魔法免伤
                case "34":
                    aiProprety.AdfMul = aiProprety.AdfMul + buffValue_mul;
                    break;

                //速度
                case "35":
                    aiProprety.MoveSpeedMul = aiProprety.MoveSpeedMul + buffValue_mul;
                    break;

                //伤害免伤
                case "36":
                    aiProprety.DamgeAddMul = aiProprety.DamgeAddMul + buffValue_mul;
                    break;
            }
            //调整完宠物后
            aiProprety.UpdataAIPropertyStatus = true;
        }
    }
}

