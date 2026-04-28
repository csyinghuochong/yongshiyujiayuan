using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

//public class Fight_Formult : MonoBehaviour {

//战斗公式

public class Fight_Formult {

    private bool criStatus;
    private ObscuredFloat roseAct_param1;
    private ObscuredFloat roseAct_param2;

    public Fight_Formult()
    {
        roseAct_param1 = 0.8f;
        roseAct_param2 = 0.4f;
    }

    public int FightHurt(int act,int def){

		int FightHurt = (int)((act-def)* (100+Random.value*20-10)/100);

        //最低伤害
        if (FightHurt <= 1) {
            FightHurt = 1;
        }
		return FightHurt;
    }

    //宠物攻击,传入技能ID和怪物ID计算角色攻击怪物的伤害值
    public bool PetActMonster(GameObject actObj, string skillID, GameObject monsterObj, bool ifCri)
    {

        if (monsterObj == null)
        {
            return false;
        }

        //判定攻击目标是怪物还是宠物
        string actTargetType = "1";

        if (monsterObj.GetComponent<AI_1>() != null)
        {
            actTargetType = "1";
        }

        if (monsterObj.GetComponent<AIPet>() != null)
        {
            actTargetType = "2";
        }

        //伤害飘字的两个变量
        string flyValueFrontStr = "";
        string flyValueLastStr = "";

        //读取技能属性
        float actDamge = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActDamge", "ID", skillID, "Skill_Template"));
        int damgeValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", skillID, "Skill_Template"));
        int damgeType = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeType", "ID", skillID, "Skill_Template"));

        if (actDamge == 0) {
            return false;
        }

        //读取角色攻击值
        int roseLv = actObj.GetComponent<AI_Property>().AI_Lv;
        int roseAct = actObj.GetComponent<AI_Property>().AI_Act;
        int roseMageAct = actObj.GetComponent<AI_Property>().AI_MageAct;
        //roseAct = (int)(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Act*0.1f);
        float roseCri = actObj.GetComponent<AI_Property>().AI_Cri;
        float roseRes = actObj.GetComponent<AI_Property>().AI_Res;
        float roseHit = actObj.GetComponent<AI_Property>().AI_Hit;
        float roseDodge = actObj.GetComponent<AI_Property>().AI_Dodge;
        float roseDefAdd = actObj.GetComponent<AI_Property>().AI_DefAdd;
        float roseAdfAdd = actObj.GetComponent<AI_Property>().AI_AdfAdd;
        //float roseDamgeAdd = actObj.GetComponent<AI_Property>().AI_DamgeSub;

        //宠物强制命中等于
        //roseHit = 0.75f;
        roseAct = (int)(roseAct * (roseAct_param1 + Random.value * roseAct_param2));

        //设置必中
        string ifMustAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfMustAct", "ID", skillID, "Skill_Template");
        //string ifMustAct = "1";
        if (ifMustAct == "1")
        {
            roseHit = 99;
        }
        //Debug.Log("roseHit = " + roseHit);
        //读取怪物属性
        int monsterLv = monsterObj.GetComponent<AI_Property>().AI_Lv;
        int monsterDef = monsterObj.GetComponent<AI_Property>().AI_Def;
        int monsterAdf = monsterObj.GetComponent<AI_Property>().AI_Adf;
        float monsteCri = monsterObj.GetComponent<AI_Property>().AI_Cri;
        float monsteRes = monsterObj.GetComponent<AI_Property>().AI_Res;
        float monsteHit = monsterObj.GetComponent<AI_Property>().AI_Hit;
        float monsteDodge = monsterObj.GetComponent<AI_Property>().AI_Dodge;
        float monsteDefAdd = monsterObj.GetComponent<AI_Property>().AI_DefAdd;
        float monsteAdfAdd = monsterObj.GetComponent<AI_Property>().AI_AdfAdd;
        float monsteDamgeAdd = monsterObj.GetComponent<AI_Property>().AI_DamgeAdd;

        int damge = 0;

        damge = fightdamge(roseLv, monsterLv, roseHit, roseCri, monsteRes,monsteDodge, roseAct, roseMageAct,damgeValue, monsterDef, monsterAdf, actDamge, damgeType, monsteDefAdd, monsteAdfAdd, monsteDamgeAdd);

        //判定是否未命中
        if (damge == -1)
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪避");
            Game_PublicClassVar.Get_function_UI.Fight_FlyText("3", langStr, "2",monsterObj, "", "");
            return true;
        }


        if (Game_PublicClassVar.Get_game_PositionVar.Fight_CriStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.Fight_CriStatus = false;
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("暴击");
            flyValueFrontStr = langStr;
            if (actTargetType == "1") {
                monsterObj.GetComponent<AI_1>().HitCriStatus = true;
            }
            if (actTargetType == "2") {
                monsterObj.GetComponent<AIPet>().HitCriStatus = true;
            }

        }


        //伤害浮动(0.9-1.1的浮动)
        ObscuredFloat actPro_1 = 1.1f;
        ObscuredFloat actPro_2 = 0.9f;
        ObscuredFloat actPro_3 = 0.2f;
        ObscuredFloat pro_1 = 1.0f;         //固定值1


        if (Random.value < actObj.GetComponent<AI_Property> ().AI_XingYunPro) {
			damge = (int)(damge * actPro_1);
		} else {
			damge = (int)(damge *(actPro_2 + Random.value * actPro_3));
		}

		float damgeResist = 0;
		//获取技能元素抵抗
		string damgeElementType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeElementType", "ID", skillID, "Skill_Template");
		//获取玩家身上抗性
		switch (damgeElementType)
		{
		case "0":
			damgeResist = 0;
			break;
		case "1":
			damgeResist = monsterObj.GetComponent<AI_Property>().AI_resistance_1;
			if(damgeResist!=0){
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("神圣抗性");
                flyValueLastStr = "<color=#D2D2D2><size=18> ("+ langStr + ")</size></color>";
			}
			break;
		case "2":
			damgeResist = monsterObj.GetComponent<AI_Property>().AI_resistance_2;
			if(damgeResist!=0){
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("黑暗抗性");
                flyValueLastStr = "<color=#D2D2D2><size=18> ("+ langStr + ")</size></color>";
			}
			break;
		case "3":
			damgeResist = monsterObj.GetComponent<AI_Property>().AI_resistance_3;
			if(damgeResist!=0){
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("火焰抗性");
                flyValueLastStr = "<color=#D2D2D2><size=18> ("+ langStr + ")</size></color>";
			}
			break;
		case "4":
			damgeResist = monsterObj.GetComponent<AI_Property>().AI_resistance_4;
			if(damgeResist!=0){
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("冰霜抗性");
                flyValueLastStr = "<color=#D2D2D2><size=18> ("+ langStr + ")</size></color>";
			}
			break;
		case "5":
			damgeResist = monsterObj.GetComponent<AI_Property>().AI_resistance_5;
			if(damgeResist!=0){
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪电抗性");
                flyValueLastStr = "<color=#D2D2D2><size=18> ("+ langStr + ")</size></color>";
			}
			break;
		}

		//伤害元素抵抗
		damge = (int)(damge * (pro_1 - damgeResist));
		damgeResist = 0;

		string monsterRace = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetRace", "ID", actObj.GetComponent<AIPet>().AI_ID.ToString(), "Pet_Template");
		switch (monsterRace)
		{
		case "0":
			damgeResist = 0;
			break;

		case "1":
			damgeResist = monsterObj.GetComponent<AI_Property>().AI_raceDamge_1;
			break;

		case "2":
			damgeResist = monsterObj.GetComponent<AI_Property>().AI_raceDamge_2;
			break;

		case "3":
			damgeResist = monsterObj.GetComponent<AI_Property>().AI_raceDamge_3;
			break;
		}

		//种族伤害
		damge = (int)(damge * (pro_1 - damgeResist));

        //宠物攻击加成
        float actAddValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_PetActAdd;
        damge = (int)(damge * (pro_1 + actAddValue));

        //吸血效果
        int xixueValue = 0;
		float petXiXueValue = actObj.GetComponent<AI_Property>().AI_XiXieValue;
		xixueValue = (int)(damge * petXiXueValue);
        if (xixueValue > 0)
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("吸血");
            Game_PublicClassVar.Get_function_UI.Fight_FlyText("2", xixueValue.ToString(), "1", actObj, langStr, "");
        }

        xixueValue = xixueValue + actObj.GetComponent<AI_Property> ().AI_Hp;

        
		if(xixueValue>actObj.GetComponent<AI_Property>().AI_HpMax){
			xixueValue = actObj.GetComponent<AI_Property>().AI_HpMax;
            //Game_PublicClassVar.Get_function_UI.Fight_FlyText("2",  xixueValue.ToString(),"1", actObj, "吸血", "");
		}
        

		actObj.GetComponent<AI_Property>().AI_Hp = xixueValue;

        //扣血
        //monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_Hp - damge;


        int value = monsterObj.GetComponent<AI_Property>().AI_Hp - damge;

        if (value <= 0) {
            if (Random.value < monsterObj.GetComponent<AI_Property>().AI_FuHuoPro)
            {
                //触发复活
                value = monsterObj.GetComponent<AI_Property>().AI_HpMax;
            }
        }

        //设置血量
        monsterObj.GetComponent<AI_Property>().AI_Hp = value;

        //开启怪物飘字
        if (actTargetType == "1") {
            monsterObj.GetComponent<AI_1>().HitStatus = true;
            Game_PublicClassVar.Get_function_UI.Fight_FlyText("3", damge.ToString(), "0", monsterObj, flyValueFrontStr, flyValueLastStr);
        }
        if (actTargetType == "2")
        {
            monsterObj.GetComponent<AIPet>().HitStatus = true;
            Game_PublicClassVar.Get_function_UI.Fight_FlyText("2", damge.ToString(), "0", monsterObj, flyValueFrontStr, flyValueLastStr);
        }


        
        //每次攻击有概率改变怪物当前的攻击目标
		float chouHenValue = 0.2f + actObj.GetComponent<AI_Property>().AI_ChouHenValue;
		if (Random.value <= chouHenValue) {
            if (actTargetType == "1")
            {
                monsterObj.GetComponent<AI_1>().AI_Target = actObj;
            }
            if (actTargetType == "2")
            {
                monsterObj.GetComponent<AIPet>().AI_Target = actObj;
            }
        }

        //副本开启伤害统计
        if (Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHai_Status)
        {
            Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Pet = Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Pet + damge;
        }

        //记录宠物造成的最高伤害
        if (damge >= Game_PublicClassVar.Get_wwwSet.YanZheng_RoseActMaxValue)
        {
            Game_PublicClassVar.Get_wwwSet.YanZheng_RoseActMaxValue = damge;
            Game_PublicClassVar.Get_wwwSet.YanZheng_ActMaxSaveStatus = true;
        }

        return true;
    }


    //传入技能ID和怪物ID计算角色攻击怪物的伤害值
    public bool RoseActMonster(string skillID, GameObject actTargetObj,bool ifCri,bool ifComAct = false) {

        if (actTargetObj.layer == 18) {
            return false;
        }

        //伤害浮动(0.9-1.1的浮动)
        ObscuredFloat pro_1 = 1.0f;         //固定值1

        //伤害飘字的两个变量
        string flyValueFrontStr = "";
        string flyValueLastStr = "";

        //读取技能属性
        ObscuredFloat actDamge = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActDamge", "ID", skillID, "Skill_Template"));
        ObscuredInt damgeValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", skillID, "Skill_Template"));
        int damgeType = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeType", "ID", skillID, "Skill_Template"));
        //Debug.Log("1damgeValue = " + damgeValue + "skillID = " + skillID);
        //读取技能附加属性
        ObscuredFloat skillAddValue_actDamge = 0;
        //技能附加值
        string skillAddValueStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(skillID, "2");
        if (skillAddValueStr != "" && skillAddValueStr != "0")
        {
            string[] skillAddValueList = skillAddValueStr.Split(',');
            for (int i = 0; i < skillAddValueList.Length; i++)
            {
                skillAddValue_actDamge = skillAddValue_actDamge + float.Parse(skillAddValueList[i]);
            }
        }

        ObscuredInt skillAddValue_damgeValue = 0;

        //技能附加值
        skillAddValueStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(skillID, "3");
        if (skillAddValueStr != "" && skillAddValueStr != "0")
        {
            string[] skillAddValueList = skillAddValueStr.Split(',');
            for (int i = 0; i < skillAddValueList.Length; i++)
            {
                skillAddValue_damgeValue = skillAddValue_damgeValue + int.Parse(skillAddValueList[i]);
            }
        }


        //读取技能附加属性
        ObscuredFloat skillAddValue_actDamgePro = 0;
        //技能附加值
        string skillAddValueStrPro = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(skillID, "10");
        if (skillAddValueStrPro != "" && skillAddValueStrPro != "0")
        {
            string[] skillAddValueListPro = skillAddValueStrPro.Split(',');
            for (int i = 0; i < skillAddValueListPro.Length; i++)
            {
                skillAddValue_actDamgePro = skillAddValue_actDamgePro + float.Parse(skillAddValueListPro[i]);
            }
        }


        //技能附加值赋值
        actDamge = actDamge * (1 + skillAddValue_actDamgePro) + skillAddValue_actDamge;
        damgeValue = damgeValue + skillAddValue_damgeValue;
        

        //读取角色攻击值
        int roseLv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
        int roseAct = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Act;
        int roseMagAct = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_MagAct;
        float roseCri = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Cri;
        float roseRes = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Res;
        float roseHit = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hit;
        float roseDodge = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Dodge;
        float roseDefAdd = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_DefAdd;
        float roseAdfAdd = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_AdfAdd;
        float roseDamgeAdd = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_DamgeSub;
        float roseZhongJiPro = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ZhongJiPro;
        int roseZhongJiValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ZhongJiValue;
        int roseGuDingValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_GuDingValue;
        int roseHuShiDefValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HuShiDefValue;
        int roseHuShiAdfValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HuShiAdfValue;
        float roseHuShiDefValuePro = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HuShiDefValuePro;
        float roseHuShiAdfValuePro = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HuShiAdfValuePro;
        float roseXiXueProValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_XiXuePro;
        float roseActAddPro = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActAddPro;

        float skillAddValue_CriValue = 0;

        //技能暴击附加值
        skillAddValueStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(skillID, "8");
        if (skillAddValueStr != "" && skillAddValueStr != "0")
        {
            string[] skillAddValueList = skillAddValueStr.Split(',');
            for (int i = 0; i < skillAddValueList.Length; i++)
            {
                skillAddValue_CriValue = skillAddValue_CriValue + float.Parse(skillAddValueList[i]);
            }
        }
        roseCri = roseCri + skillAddValue_CriValue;


        //设置必中
        string ifMustAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfMustAct", "ID", skillID, "Skill_Template");
        //string ifMustAct = "1";
        float rose_BiZhongPro = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_BiZhongPro;
        //必中
        if (Random.value < rose_BiZhongPro) {
            ifMustAct = "1";
        }
        if (ifMustAct == "1") {
            roseHit = 99;
        }

        //读取怪物属性
        int monsterLv = actTargetObj.GetComponent<AI_Property>().AI_Lv;
        int monsterDef = actTargetObj.GetComponent<AI_Property>().AI_Def;
        int monsterAdf = actTargetObj.GetComponent<AI_Property>().AI_Adf;
        float monsteCri = actTargetObj.GetComponent<AI_Property>().AI_Cri;
        float monsteRes = actTargetObj.GetComponent<AI_Property>().AI_Res;
        float monsteHit = actTargetObj.GetComponent<AI_Property>().AI_Hit;
        float monsteDodge = actTargetObj.GetComponent<AI_Property>().AI_Dodge;
        float monsteDefAdd = actTargetObj.GetComponent<AI_Property>().AI_DefAdd;
        float monsteAdfAdd = actTargetObj.GetComponent<AI_Property>().AI_AdfAdd;
        float monsteDamgeAdd = actTargetObj.GetComponent<AI_Property>().AI_DamgeAdd;

		//触发特殊的无视防御属性
		if(Random.value <= Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActWuShi){
			monsterDef = 0;
			monsterAdf = 0;
		}
        
        int damge = 0;

        //判定是否重击
        if (Random.value <= roseZhongJiPro) {
            monsterDef = 0;
            monsterAdf = 0;
            roseAct = roseAct + roseZhongJiValue;
            actTargetObj.GetComponent<AI_1>().ZhongJiStatus = true;
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("重击");
            flyValueFrontStr = langStr;
        }

        //忽视目标防御值
        monsterDef = (int)(monsterDef - roseHuShiDefValue * (1 - roseHuShiDefValuePro));
        //忽视目标魔防值
        monsterAdf = (int)(monsterAdf - roseHuShiAdfValue * (1 - roseHuShiAdfValuePro));
        damge = fightdamge(roseLv, monsterLv, roseHit, roseCri, monsteRes, monsteDodge, roseAct, roseMagAct, damgeValue, monsterDef, monsterAdf, actDamge, damgeType, monsteDefAdd, monsteAdfAdd, monsteDamgeAdd);

        int baseDamge = damge;

        //判定是否未普通攻击  skillID == "62000001"|| skillID == "60011001"|| skillID == "60012001"
        //Debug.Log("roseActAddPro = " + roseActAddPro + " damge = " + damge + "skillID = " + skillID);
        if (skillID == "62000001"|| skillID == "60011001"|| skillID == "60012001") {
            damge = (int)(damge * (1.0f + roseActAddPro));
            //Debug.Log("damge = " + damge);
        }

        //判定是否未命中
        if (damge == -1)
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪避");
            Game_PublicClassVar.Get_function_UI.Fight_FlyText("3", langStr, "2",actTargetObj, "", "");
            return true;
        }

        //判定是否暴击
        if (Game_PublicClassVar.Get_game_PositionVar.Fight_CriStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.Fight_CriStatus = false;
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("暴击");
            flyValueFrontStr = langStr;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().actTriggerSkill("2", "4");  //暴击时触发被动技能
        }

		float damgeResist = 0;

		//获取技能元素抵抗
		string damgeElementType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("DamgeElementType", "ID", skillID, "Skill_Template");
		//获取玩家身上抗性
		switch (damgeElementType) {
		case "0":
			damgeResist = 0;
			break;
		case "1":
			damgeResist = actTargetObj.GetComponent<AI_Property>().AI_resistance_1;
			if(damgeResist!=0){
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("神圣抗性");
                flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr + ")</size></color>";
			}
			break;
		case "2":
			damgeResist = actTargetObj.GetComponent<AI_Property>().AI_resistance_2;
			if(damgeResist!=0){
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("黑暗抗性");
                flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr + ")</size></color>";
			}
			break;
		case "3":
			damgeResist = actTargetObj.GetComponent<AI_Property>().AI_resistance_3;
			if(damgeResist!=0){
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("火焰抗性");
                flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr + ")</size></color>";
			}
			break;
		case "4":
			damgeResist = actTargetObj.GetComponent<AI_Property>().AI_resistance_4;
			if(damgeResist!=0){
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("冰霜抗性");
                flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr + ")</size></color>";
			}
			break;
		case "5":
			damgeResist = actTargetObj.GetComponent<AI_Property>().AI_resistance_5;
			if(damgeResist!=0){
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪电抗性");
                flyValueLastStr = "<color=#D2D2D2><size=18> ("+ langStr + ")</size></color>";
			}
			break;
		}

		//伤害元素抵抗
		damge = (int)(damge * (pro_1 - damgeResist));
		damgeResist = 0;

		string monsterRace = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("MonsterRace", "ID", actTargetObj.GetComponent<AI_Property> ().AI_ID, "Monster_Template");
		switch (monsterRace) {
		case "0":
			damgeResist = 0;
			break;

		case "1":
			damgeResist = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_RaceDamge_1;
			break;

		case "2":
			damgeResist = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_RaceDamge_2;
			break;

		case "3":
			damgeResist = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_RaceDamge_3;
			break;
		}

		//种族伤害
		damge = (int)(damge * (pro_1 + damgeResist));

        //普通攻击Boss加成
        if (ifComAct)
        {
            float actBossAddValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Boss_ActAdd;
            damge = (int)(damge * (pro_1 + actBossAddValue));
        }
        else {
            //技能攻击加成
            float actBossAddValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Boss_SkillAdd;
            damge = (int)(damge * (pro_1 + actBossAddValue));
        }


        //每次伤害增加的
        damge = damge + roseGuDingValue;

        /*
        //判定是否暴击
        if (ifCri)
        {
            damge = damge * 2;
            actTargetObj.GetComponent<AI_1>().HitCriStatus = true;
        }
        */
        //扣血
        actTargetObj.GetComponent<AI_Property>().AI_Hp = actTargetObj.GetComponent<AI_Property>().AI_Hp - damge;

        //吸血,技能不触发吸血,只有普攻触发
        if (skillID == "62000001"|| skillID == "60011001"|| skillID == "60012001") {
            int xiXueValue = (int)(baseDamge * roseXiXueProValue);
            //Debug.Log("xiXueValue = " + xiXueValue);
            if (xiXueValue > 0)
            {
                Game_PublicClassVar.Get_function_Rose.addRoseHp(xiXueValue, "1", false);
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseIfXiXue = true;
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("吸血");
                Game_PublicClassVar.Get_function_UI.Fight_FlyText("1", xiXueValue.ToString(), "1", actTargetObj, langStr, "");
            }
        }

        //开启怪物飘字
        actTargetObj.GetComponent<AI_1>().HitStatus = true;
        Game_PublicClassVar.Get_function_UI.Fight_FlyText("3", damge.ToString(), "0",actTargetObj, flyValueFrontStr, flyValueLastStr);
        //每次攻击有概率改变怪物当前的攻击目标
        if (Random.value <= (0.1f + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ChouHenValue))
        {
            actTargetObj.GetComponent<AI_1>().AI_Target = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;
        }

        //副本开启伤害统计
        if (Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHai_Status) {
            Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Rose = Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Rose + damge;
        }

        //记录玩家造成的最高伤害
        if (damge >= Game_PublicClassVar.Get_wwwSet.YanZheng_RoseActMaxValue) {
            Game_PublicClassVar.Get_wwwSet.YanZheng_RoseActMaxValue = damge;
            Game_PublicClassVar.Get_wwwSet.YanZheng_ActMaxSaveStatus = true;
        }

        return true;
    }




    //传入技能ID和怪物ID计算怪物攻击角色的伤害值
    public bool MonsterActRose(string skillID, GameObject monsterObj, GameObject ActPetObj, bool ifComAct = false)
    {
        if (ActPetObj == null) {
            Debug.Log("攻击目标为空!");
            return false;
        }

        ObscuredFloat actPro_1 = 1.1f;
        ObscuredFloat actPro_2 = 0.9f;
        ObscuredFloat actPro_3 = 0.2f;
        ObscuredFloat pro_1 = 1.0f;         //固定值1
        ObscuredInt pro_2 = 2;         //固定值1

        //Debug.Log("造成了伤害");
        //读取技能属性
        float actDamge = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActDamge", "ID", skillID,"Skill_Template"));
        int damgeValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", skillID, "Skill_Template"));
        int damgeType = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeType", "ID", skillID, "Skill_Template"));
        string skillActType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillActType", "ID", skillID, "Skill_Template");


        //攻击目标的类型
        string actObjType = "1";
        //GameObject aiActObj = monsterObj.GetComponent<AI_1>().AI_Target;
        Rose_Proprety roseProprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        //if (aiActObj != null) {
        actObjType = Game_PublicClassVar.Get_function_Skill.returnObjType(ActPetObj);
        //}

        if (actObjType != "2") {
            //ActPetObj = null;   //表示为宠物
        }

        //伤害飘字的两个变量
        string flyValueFrontStr = "";
        string flyValueLastStr = "";

        //读取角色攻击值
        int roseLv = roseProprety.Rose_Lv;
        int roseAct = roseProprety.Rose_Act;
        int roseDef = roseProprety.Rose_Def;
        int roseAdf = roseProprety.Rose_Adf;
        float roseCri = roseProprety.Rose_Cri;
        float roseRes = roseProprety.Rose_Res;
        float roseHit = roseProprety.Rose_Hit;
        float roseDodge = roseProprety.Rose_Dodge;
        float roseDefAdd = roseProprety.Rose_DefAdd;
        float roseAdfAdd = roseProprety.Rose_AdfAdd;
        float roseDamgeAdd = roseProprety.Rose_DamgeSub;
        //float roseActDamgeAdd = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_DamgeAdd;
        int roseGeDangValue = roseProprety.Rose_GeDangValue;
		float rose_DiXiaoPro = 0;
        float roseSkillActDefPro = 0;
        float roseSkillActDodgePro = 0;
        float ai_MonsterActDameDefPro = 0;
        float ai_MonsterSkillDameDefPro = 0;

        //获取宠物的属性
        if (actObjType == "2")
        {
            roseLv = ActPetObj.GetComponent<AI_Property>().AI_Lv;
            roseAct = ActPetObj.GetComponent<AI_Property>().AI_Act;
            roseDef = ActPetObj.GetComponent<AI_Property>().AI_Def;
            roseAdf = ActPetObj.GetComponent<AI_Property>().AI_Adf;
            roseCri = ActPetObj.GetComponent<AI_Property>().AI_Cri;
            roseRes = ActPetObj.GetComponent<AI_Property>().AI_Res;
            roseHit = ActPetObj.GetComponent<AI_Property>().AI_Hit;
            roseDodge = ActPetObj.GetComponent<AI_Property>().AI_Dodge;
            roseDefAdd = ActPetObj.GetComponent<AI_Property>().AI_DefAdd;
            roseAdfAdd = ActPetObj.GetComponent<AI_Property>().AI_AdfAdd;
            roseDamgeAdd = ActPetObj.GetComponent<AI_Property>().AI_DamgeAdd;
			rose_DiXiaoPro =  ActPetObj.GetComponent<AI_Property>().AI_DiXiaoPro;
            //roseActDamgeAdd = ActPetObj.GetComponent<AI_Property>().AI_ActDamgeAdd;
            roseSkillActDefPro = ActPetObj.GetComponent<AI_Property>().AI_SkillActDefPro;
            roseSkillActDodgePro = ActPetObj.GetComponent<AI_Property>().AI_SkillActDodgePro;
            ai_MonsterActDameDefPro = ActPetObj.GetComponent<AI_Property>().AI_MonsterActDameDefPro;
            ai_MonsterSkillDameDefPro = ActPetObj.GetComponent<AI_Property>().AI_MonsterSkillDameDefPro;
            roseGeDangValue = 0;    //宠物没有格挡属性
        }

        //读取怪物属性
        int monsterLv = monsterObj.GetComponent<AI_Property>().AI_Lv;
        int monsterAct = monsterObj.GetComponent<AI_Property>().AI_Act;
        int monsterDef = monsterObj.GetComponent<AI_Property>().AI_Def;
        int monsterAdf = monsterObj.GetComponent<AI_Property>().AI_Adf;
        float monsteCri = monsterObj.GetComponent<AI_Property>().AI_Cri;
        float monsteRes = monsterObj.GetComponent<AI_Property>().AI_Res;
        float monsteHit = monsterObj.GetComponent<AI_Property>().AI_Hit;
        float monsteDodge = monsterObj.GetComponent<AI_Property>().AI_Dodge;
        float monsteDefAdd = monsterObj.GetComponent<AI_Property>().AI_DefAdd;
        float monsteAdfAdd = monsterObj.GetComponent<AI_Property>().AI_AdfAdd;
        float monsteDamgeAdd = monsterObj.GetComponent<AI_Property>().AI_DamgeAdd;
        float monsteActDamgeAdd = monsterObj.GetComponent<AI_Property>().AI_ActDamgeAdd;
        //float monsteSkillActDefPro = monsterObj.GetComponent<AI_Property>().AI_SkillActDefPro;
        //float monsteSkillActDodgePro = monsterObj.GetComponent<AI_Property>().AI_SkillActDodgePro;
        //monsterAct = 0;

        //判断技能是否被闪避
        if (skillActType == "1") {
            if (Random.value < roseSkillActDodgePro) {

                //高于90%的技能闪避 不显示  要不太频繁了
                if (roseSkillActDodgePro <= 0.9f)
                {
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("技能闪避");
                    //玩家闪避
                    if (actObjType == "1")
                    {
                        Game_PublicClassVar.Get_function_UI.Fight_FlyText("1", langStr, "2", Game_PublicClassVar.Get_game_PositionVar.Obj_Rose, "", "");
                    }
                    //宠物闪避
                    if (actObjType == "2")
                    {
                        Game_PublicClassVar.Get_function_UI.Fight_FlyText("2", langStr, "2", ActPetObj, "", "");
                    }
                }
                return true;
            }
        }


        //判定是否抵消伤害
        if (Random.value < rose_DiXiaoPro){

            //玩家闪避
            if (actObjType == "1")
            {
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("抵消伤害");
                Game_PublicClassVar.Get_function_UI.Fight_FlyText("1", langStr, "2", Game_PublicClassVar.Get_game_PositionVar.Obj_Rose, "", "");
            }
            //宠物闪避
            if (actObjType == "2")
            {
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("抵消伤害");
                Game_PublicClassVar.Get_function_UI.Fight_FlyText("2", langStr, "2", ActPetObj, "", "");
            }
			return true;
		}

        //设置必中
        string ifMustAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfMustAct", "ID", skillID, "Skill_Template");
        if (ifMustAct == "1")
        {
            monsteHit = 99;
        }

        int damge = fightdamge(monsterLv, roseLv, monsteHit, monsteCri,roseRes,roseDodge, monsterAct, 0, damgeValue, roseDef, roseAdf, actDamge, damgeType, roseDefAdd, roseAdfAdd, roseDamgeAdd);
        //Debug.Log("damge = " + damge + ";actObjType = " + actObjType);
        //判定是否未命中
        if (damge == -1 ||damge == 0) {
            string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪避");
            flyValueFrontStr = langStr1;
            //玩家闪避
            if (actObjType == "1")
            {
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().actTriggerSkill("2", "5");  //闪避时触发被动技能

                //闪避恢复血量
                if (roseProprety.Rose_DodgeAddHpPro > 0)
                {
                    int addHp = (int)(roseProprety.Rose_Hp * roseProprety.Rose_DodgeAddHpPro);
                    Game_PublicClassVar.Get_function_Rose.addRoseHp(addHp,"1",false);
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪避 血量");
                    Game_PublicClassVar.Get_function_UI.Fight_FlyText("1", langStr + "+" + addHp, "2", Game_PublicClassVar.Get_game_PositionVar.Obj_Rose, "", "");
                }
                else {
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪避");
                    Game_PublicClassVar.Get_function_UI.Fight_FlyText("1", langStr, "2", Game_PublicClassVar.Get_game_PositionVar.Obj_Rose, "", "");
                }
            }
            //宠物闪避
            if (actObjType == "2")
            {
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪避");
                Game_PublicClassVar.Get_function_UI.Fight_FlyText("2", langStr, "2",ActPetObj, "", "");
            }
            return true;
        }

        //判定是否暴击
        if (Game_PublicClassVar.Get_game_PositionVar.Fight_CriStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.Fight_CriStatus = false;
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("暴击");
            flyValueFrontStr = langStr;
        }

        //技能抵抗
        if (skillActType == "1")
        {
            damge = (int)((float)damge * (1 - roseSkillActDefPro));
        }

        //宠物伤害浮动
        if (actObjType == "2")
        {
			//伤害浮动(0.9-1.1的浮动)
			if (Random.value < monsterObj.GetComponent<AI_Property>().AI_XingYunPro) {
				damge = (int)(damge * actPro_1);
			} else {
				damge = (int)(damge * (actPro_2 + Random.value * actPro_3));
			}
		}

        //伤害加成
        damge = (int)(damge * (pro_1 + monsteActDamgeAdd));

        //-1表示闪避
        /*
        if (damge == -1) {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RpseIfDodge = true;
        }
        */

        //格挡
        damge = damge - roseGeDangValue;

		//获取宠物的属性
        if (actObjType == "2")
        {
			float damgeResist = 0;
			//获取技能元素抵抗
			string damgeElementType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("DamgeElementType", "ID", skillID, "Skill_Template");
			//获取玩家身上抗性
			switch (damgeElementType) {
			case "0":
				damgeResist = 0;
				break;
			case "1":
				damgeResist = ActPetObj.GetComponent<AI_Property> ().AI_resistance_1;
				if(damgeResist!=0){
                        //ActPetObj.GetComponent<AIPet>().AIHitTextlater = "<color=#D2D2D2><size=18> (神圣抗性)</size></color>";
                    string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("神圣抗性");
                    flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr1 + ")</size></color>";
				}
				break;
			case "2":
				damgeResist = ActPetObj.GetComponent<AI_Property>().AI_resistance_2;
				if(damgeResist!=0){
                    string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("黑暗抗性");
                    flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr1 + ")</size></color>";
				}
				break;
			case "3":
				damgeResist = ActPetObj.GetComponent<AI_Property>().AI_resistance_3;
				if(damgeResist!=0){
                    string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("火焰抗性");
                    flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr1 + ")</size></color>";
				}
				break;
			case "4":
				damgeResist = ActPetObj.GetComponent<AI_Property>().AI_resistance_4;
				if(damgeResist!=0){
                    string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("冰霜抗性");
                    flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr1 + ")</size></color>";
				}
				break;
			case "5":
				damgeResist = ActPetObj.GetComponent<AI_Property>().AI_resistance_5;
				if(damgeResist!=0){
                    string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪电抗性");
                    flyValueLastStr = "<color=#D2D2D2><size=18> ("+ langStr1 + ")</size></color>";
				}
				break;
			}

			//伤害元素抵抗
			damge = (int)(damge * (pro_1 - damgeResist));
			damgeResist = 0;

            //种族
			string monsterRace = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("MonsterRace", "ID", monsterObj.GetComponent<AI_Property> ().AI_ID, "Monster_Template");
			switch (monsterRace) {
			case "0":
				damgeResist = 0;
				break;

			case "1":
				damgeResist = ActPetObj.GetComponent<AI_Property>().AI_raceResistance_1;
				break;

			case "2":
				damgeResist = ActPetObj.GetComponent<AI_Property>().AI_raceResistance_2;
				break;

			case "3":
				damgeResist = ActPetObj.GetComponent<AI_Property>().AI_raceResistance_3;
				break;
			}

			//种族伤害
			damge = (int)(damge * (pro_1 - damgeResist));

		} else {
			float damgeResist = 0;

			//获取技能元素抵抗
			string damgeElementType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeElementType", "ID", skillID, "Skill_Template");
			//获取玩家身上抗性
			switch (damgeElementType)
			{
			case "0":
				damgeResist = 0;
				break;
			case "1":
				damgeResist = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Resistance_1;
				if(damgeResist!=0){
                        string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("神圣抗性");
                        flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr1 + ")</size></color>";
				}
				break;
			case "2":
				damgeResist = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Resistance_2;
				if(damgeResist!=0){
                        string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("黑暗抗性");
                        flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr1 + ")</size></color>";
				}
				break;
			case "3":
				damgeResist = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Resistance_3;
				if(damgeResist!=0){
                        string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("火焰抗性");
                        flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr1 + ")</size></color>";
				}
				break;
			case "4":
				damgeResist = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Resistance_4;
				if(damgeResist!=0){
                        string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("冰霜抗性");
                        flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr1 + ")</size></color>";
				}
				break;
			case "5":
				damgeResist = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Resistance_5;
				if(damgeResist!=0){
                        string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪电抗性");
                        flyValueLastStr = "<color=#D2D2D2><size=18> (" + langStr1 + ")</size></color>";
				}
				break;
			}

			//伤害元素抵抗
			damge = (int)(damge * (pro_1 - damgeResist));
			damgeResist = 0;

			string monsterRace = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterRace", "ID", monsterObj.GetComponent<AI_Property>().AI_ID, "Monster_Template");
			switch (monsterRace)
			{
			case "0":
				damgeResist = 0;
				break;

			case "1":
				damgeResist = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_RaceResistance_1;
				break;

			case "2":
				damgeResist = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_RaceResistance_2;
				break;

			case "3":
				damgeResist = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_RaceResistance_3;
				break;
			}

			//种族伤害
			damge = (int)(damge * (pro_1 - damgeResist));
		}

        //攻击玩家
        if (actObjType == "1")
        {
            //受到Boss普通攻击减免
            if (monsterObj.GetComponent<AI_1>().ai_monsterType == "3") {
                if (ifComAct)
                {
                    float actBossCostValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Boss_ActHitCost;
                    damge = (int)(damge * (pro_1 - actBossCostValue));
                }
                else
                {
                    //受到Boss技能攻击减免
                    float actBossCostValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Boss_SkillHitCost;
                    damge = (int)(damge * (pro_1 - actBossCostValue));
                }
            }
        }

        //攻击宠物
        if (actObjType == "2")
        {
            float actHitCost = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_PetActHitCost;
            damge = (int)(damge * (pro_1 - actHitCost));
            //Debug.Log("原始造成伤害!" + damge);
            //首领攻击免伤
            if (skillActType == "0")
            {
                damge = (int)(damge * (1 - ai_MonsterActDameDefPro));
            }

            //首领技能免伤
            if (skillActType == "1")
            {
                damge = (int)(damge * (1 - ai_MonsterSkillDameDefPro));
            }

            //Debug.Log("原始造成伤害!" + damge);
        }




        //防止伤害为0
        if (damge < 0)
        {
            damge = 0;
        }

        //获取当前技能为普攻还是法术攻击
        int reboundDamge = 0;
        float reboundDamgePro = 0;
        switch (damgeType) { 
            
            //普通攻击
            case 1:

                //获取攻击反击
                reboundDamgePro = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActRebound;
                break;
            //法术攻击
            case 2:

                //获取法术反击
                reboundDamgePro = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_MagicRebound;
                break;
        }

        //设置反击伤害
        reboundDamge = (int)(damge * reboundDamgePro);
        //扣除反击伤害
        if(reboundDamge>0){
            monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_Hp - reboundDamge;

            //反击伤害飘字
            string langStr1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("反击伤害");
            string hitTextLast = "<color=#D2D2D2><size=18> ("+ langStr1 + ")</size></color>";
            Game_PublicClassVar.Get_function_UI.Fight_FlyText("3", reboundDamge.ToString(), "",monsterObj, "", hitTextLast);

            switch (damgeType) {
                
                case 1:
                    monsterObj.GetComponent<AI_1>().ActReboundStatus = true;
                    monsterObj.GetComponent<AI_1>().HitStatus = true;
                    break;

                case 2:
                    monsterObj.GetComponent<AI_1>().MageReboundStatus = true;
                    monsterObj.GetComponent<AI_1>().HitStatus = true;
                    break;
            }
        }

        //获取当前角色血量
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow > 0) {

            if (actObjType == "1") {

                //检测角色是否有护盾
                if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunStatus)
                {
                    int hudunDamge = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunValue - damge;
                    if (hudunDamge > 0)
                    {
                        //Debug.Log("护盾抵消伤害:" + damge);
                        //战斗飘字
                        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("护盾抵消");
                        Game_PublicClassVar.Get_function_UI.Fight_FlyText("1", langStr, "2",ActPetObj, flyValueFrontStr, flyValueLastStr);
                        damge = 0;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunValue = hudunDamge;
                    }
                    else
                    {
                        //护盾消失,清空护盾状态
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunValue = 0;
                        damge = System.Math.Abs(hudunDamge);
                    }
                }
            }


			//装备灾难属性
			if(Random.value <= Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ZaiNanValue){
				damge = damge * pro_2;
			}


            if (actObjType == "2")
            {
                int value = ActPetObj.GetComponent<AI_Property>().AI_Hp - damge;

                if (Random.value < ActPetObj.GetComponent<AI_Property>().AI_FuHuoPro&& value <= 0)
                {
                    //触发复活
                    ActPetObj.GetComponent<AI_Property>().AI_Hp = ActPetObj.GetComponent<AI_Property>().AI_HpMax;

                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_241");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("复活成功！");

                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("复活成功啦卡卡");
                }
                else
                {
                    ActPetObj.GetComponent<AI_Property>().AI_Hp = value;
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("收到伤害剩余:"+ value);
                }
                
                //战斗飘字
                Game_PublicClassVar.Get_function_UI.Fight_FlyText("2", damge.ToString(), "0",ActPetObj,flyValueFrontStr, flyValueLastStr);
                //记录当前血量值
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetNowHp", value.ToString(), "ID", ActPetObj.GetComponent<AIPet>().RosePet_ID, "RosePet");
                //此处暂时不做存储,频率太高,到时候统计宠物冷却时间的时候回自动存一次数据
                //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

				//更新主界面
				Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().PetUpHp();

            }
            else {
                int value = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow - damge;
                if (value <= 0)
                {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow = 0;
                }
                else
                {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow = value;
                    //此处不做存储本地本地文件处理,通过其他方式保存RoseData时一起保存,减少内存消耗
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseHpNow", value.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                }
                //战斗飘字
                if (damge != 0) { 
                    Game_PublicClassVar.Get_function_UI.Fight_FlyText("1", damge.ToString(), "0",Game_PublicClassVar.Get_game_PositionVar.Obj_Rose, flyValueFrontStr, flyValueLastStr);
                }
            }
        }

        return true;
    }

    //传入对应的属性计算造成的伤害
    //参数说明
    /*
    lv                  攻击者等级
    target_lv           目标者等级
    hit                 攻击者命中值
    cri                 攻击者暴击值
    target_dodge        目标者闪避值
    act                 攻击者攻击值
    magact                 攻击者的魔法攻击值
    damgeValue          技能的附加的固定伤害
    target_def          目标的物理防御
    target_adf          目标的魔法防御
    actDamge            技能伤害来源的攻击值系数,为1时表示造成100%攻击力的伤害
    damgeType           伤害类型，分物理和魔法
    target_DefAdd       目标物理免伤值
    target_AdfAdd       目标魔法免伤值
    target_damgeAdd     目标总免伤值
    */
    private int fightdamge(int lv, int target_lv, float hit, float cri, float res, float target_dodge, int act, int magact, int damgeValue, int target_def, int target_adf, float actDamge, int damgeType, float target_DefAdd, float target_AdfAdd, float target_damgeAdd)
    {

        ObscuredFloat actPro_1 = 0.8f;
        ObscuredFloat actPro_2 = 0.05f;
        ObscuredFloat pro_1 = 1.0f;

        //判定是否攻击到目标
        ObscuredInt lvNum = lv - target_lv;
        ObscuredFloat now_RoseHit = actPro_1 + actPro_2 * lvNum + hit;
        ObscuredFloat hitChance = now_RoseHit - target_dodge;

        ObscuredInt criCof = 1;  //暴击系数

        //获取当前暴击值
        cri = cri - res;
        if (cri<0)
        {
            cri = 0;
        }

        if (Random.value <= cri)
        {
            //双倍伤害
            criCof = 2;
            //设定暴击状态 用于战斗飘字
            Game_PublicClassVar.Get_game_PositionVar.Fight_CriStatus = true;
        }
        else {
            criCof = 1;
        }

        //设置保底命中值(设置最低命中为20%)
        if (hitChance < 0.2f)
        {
            hitChance = 0.2f;
        }

        //根据随机数判定是否命中目标
        if (Random.value > hitChance)
        {
            //如果随机数大于命中值表示未命中目标
            return -1;
        }

        //重击概率和重击值



        //判定攻击类型
        switch (damgeType)
        {

            //物理攻击
            case 1:

                ObscuredInt damge = (int)((act - target_def) * actDamge * (pro_1 - target_DefAdd) * (pro_1 - target_damgeAdd));
                if (damge <= 0)
                {
                    //固定伤害
                    damge = damgeValue;
                }
                else
                {
                    damge = damge * criCof + damgeValue;
                }

                //保底伤害
                ObscuredInt baodiDamge = (int)(act * 0.1f);
                if (damge < baodiDamge)
                {
                    damge = baodiDamge;
                }
                if (damge < 1)
                {
                    damge = 1;
                }

                return damge;

                break;

            //魔法攻击
            case 2:

                damge = (int)((act + magact - target_adf) * actDamge * (pro_1 - target_AdfAdd) * (pro_1 - target_damgeAdd));
                if (damge <= 0)
                {
                    //固定伤害
                    damge = damgeValue;
                }
                else
                {
                    damge = damge * criCof + damgeValue;
                }

                //保底伤害
                baodiDamge = (int)((act + magact) * 0.1f);
                if (damge < baodiDamge)
                {
                    damge = baodiDamge;
                }
                if (damge < 1)
                {
                    damge = 1;
                }

                return damge;

                break;

        }

        return 0;

    }

}