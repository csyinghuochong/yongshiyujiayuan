using UnityEngine;
using System.Collections;

public class Function_Skill{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //根据BuffID触发对应的技能  (参数1：BuffID组成的字符串，参数2：附加Buff的Obj，参数3：技能物体的Obj)
    public bool SkillBuff(string buffIDStr, GameObject obj,GameObject skillObj = null)
    {

        if (obj == null) {
            return false;
        }

        //Debug.Log("进入触发BUFF = " + buffIDStr);
        //循环获取BuffID
        string[] buffID = buffIDStr.Split(',');
        //Debug.Log("进入触发BUFF[0] = " + buffID[0]);
        for (int i = 0; i <= buffID.Length - 1; i++)
        {
            //Debug.Log("进入触发BUFF0000000 = " + buffID[i]);
            if (buffID[i] != "") { 
                //获取Buff释放目标
                //ebug.Log("可能出错的BUFFID = " + buffID[i]);
                string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", buffID[i], "SkillBuff_Template");
                //Debug.Log("进入触发targetType = " + targetType);
                switch (targetType) { 
                    //玩家自身
                    case "1":
                        //判定传入obj是否相符
                        //Debug.Log("obj = " + obj.gameObject.name);
                        if (obj.gameObject.name == "Rose") {
                            //Debug.Log("进入触发BUFF22222222222 = " + buffIDStr);
                            BuffTypeReturnScriptName(obj, buffID[i], skillObj);
                        }

                        break;
                    //怪物
                    case "2":
                        //判定传入obj是否相符
                        //Debug.Log("怪物触发BUFF,obj.gameObject.name = " + obj.gameObject.name);
                        //if (obj.gameObject.name != "Rose")
                        if (obj.gameObject.layer == 12)
                        {
                            //Debug.Log("怪物触发BUFF:" + buffID[i]);
                            BuffTypeReturnScriptName(obj, buffID[i], skillObj);
                        }

                        break;

                    //宠物
                    case "3":
                        //判定传入obj是否相符
                        //Debug.Log("怪物触发BUFF,obj.gameObject.name = " + obj.gameObject.name);
                        if (obj != null) {
                            if (obj.gameObject.layer == 18)
                            {
                                //Debug.Log("怪物触发BUFF:" + buffID[i]);
                                BuffTypeReturnScriptName(obj, buffID[i], skillObj);
                            }
                        }

                        break;
                }
            }
        }

        return true;
        
    }

    //根据buff类型返回Buff脚本名称(参数1：需要绑定对象,参数2：BuffID,参数3:技能Obj 参数4：buff已经运行时间)             注意：此处不做是否绑定的判定,请在其他地方判定
    public void BuffTypeReturnScriptName(GameObject obj,string buffID,GameObject skillObj = null, float buffTimeSum = 0)
    {
        
        //获取Buff类型
        string buffType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffType", "ID", buffID, "SkillBuff_Template");
        //Debug.Log("进入附加BUFF,buffType = " + buffType + "buffID = " + buffID);
        switch (buffType) { 
            case "1":
                bool addBuffStatus = true;
                Buff_1[] buff_1 = obj.GetComponents<Buff_1>();
                for (int i = 0; i <= buff_1.Length - 1; i++)
                {
                    //获取Buff是否叠加
                    //检测目标是否用此Buff如果有执行替换
                    string buffAddClass = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffAddClass", "ID", buffID, "SkillBuff_Template");
                    if (buffAddClass == "0")
                    {
                        //注销buff(特殊处理,宠物光坏buff不消失)
                        if (buff_1[i].BuffID != "90041210" && buff_1[i].BuffID != "90040210" && buff_1[i].BuffID != "90000206" && buff_1[i].BuffID != "90000207")
                        {
                            MonoBehaviour.Destroy(buff_1[i]);
                        }
                        else {
                            //光环类的不会添加新buff也不会销毁,要不左上角的图标会显示不正确，因为Des 的调用顺序是低于Start 不一样
                            if (buff_1[i].BuffID == buffID)
                            {
                                addBuffStatus = false;
                            }
                        }

                        //buffID相同销毁原来的
                        /*
                        if (buff_1[i].BuffID == buffID) {
                            MonoBehaviour.Destroy(buff_1[i]);
                        }
                        */
                    }

                    //关联buff处理
                    if (buffAddClass == "2")
                    {
                        string weiYibuffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("WeiYiBuffID", "ID", buffID, "SkillBuff_Template");
                        string[] weiYiBuffIDList = weiYibuffID.Split(',');
                        for (int y = 0; y < weiYiBuffIDList.Length; y++)
                        {
                            if (buff_1[i].BuffID == weiYiBuffIDList[y])
                            {
                                //注销
                                if (buff_1[i].BuffID == "90041210" || buff_1[i].BuffID == "90040210")
                                {
                                    buff_1[i].buffTimeSum = 0;
                                    addBuffStatus = false;
                                }
                                else {
                                    MonoBehaviour.Destroy(buff_1[i]);
                                    addBuffStatus = true;
                                }
                            }
                        }
                    }
                }

                if (addBuffStatus) {
                    Buff_1 addBuff1 = obj.AddComponent<Buff_1>();
                    addBuff1.BuffID = buffID;
                }

                break;

            case "2":
                Buff_2[] buff_2 = obj.GetComponents<Buff_2>();
                for (int i = 0; i <= buff_2.Length - 1; i++)
                {
                    string[] continuedTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffParameter", "ID", buffID, "SkillBuff_Template").Split(',');
                    string propretyType = continuedTime[0];
                    //string propretyAddType = continuedTime[1];
                    //表示两个Buff的Buff效果一致,后者将替代前者的Buff
                    //获取Buff是否叠加
                    string buffAddClass = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffAddClass", "ID", buffID, "SkillBuff_Template");
                    //if(buffAddClass!=null){
                        if (buffAddClass == "0")
                        {
                            //注销
                            MonoBehaviour.Destroy(buff_2[i]);
   
                        }
                }

                //绑定脚本
                obj.AddComponent<Buff_2>().BuffID = buffID;
                //Debug.Log("绑定BUFF提示：" + obj.name + "绑定BUFF_2");
                //给BUFF传入释放技能时的攻击值  (目标怪物不支持按照怪物攻击的百分比减血,因为不知道是那个怪物放的)
                if (obj.layer == 14) {
                    //value = obj.GetComponent<AI_Property>().AI_Act;
                    //Debug.Log("攻击角色");
                }
                //攻击怪物
                if (obj.layer == 12){
                    int actValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Act;
                    //Debug.Log("触发流血 act = " + actValue);
                    obj.GetComponent<Buff_2>().ActValue = actValue;
                    //Debug.Log("攻击怪物" + actValue);
                }
                
                break;
                

                //眩晕
            case "3":
                obj.AddComponent<Buff_3>().BuffID = buffID;
                //Debug.Log("绑定眩晕BUFF");
                //obj.GetComponent<Buff_3>();
                break;

            //属性改变
            case "4":
                Buff_4[] buff_4 = obj.GetComponents<Buff_4>();
                bool ifAddBuffStatus = true;
                //Debug.Log("buff_4长度" + buff_4.Length);
                for (int i = 0; i <= buff_4.Length - 1; i++)
                {
                    //Debug.Log("IIIIIIIIIIIIIIIIIIIIIIIIIIIIII = " + i);
                    string[] continuedTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffParameter", "ID", buffID, "SkillBuff_Template").Split(',');
                    string propretyType = continuedTime[0];
                    string propretyAddType = continuedTime[1];
                    float propretyAddValue = float.Parse(continuedTime[2]);
                    //表示两个Buff的Buff效果一致,后者将替代前者的Buff
                    if (buff_4[i].propretyType == propretyType) {
                        if (buff_4[i].propretyAddType == propretyAddType) {
                            if (propretyType == "35" && propretyAddType == "2" && propretyAddValue > float.Parse(buff_4[i].propretyAddValue)&& propretyAddValue<0)       //技能效果不支持叠加  两个Buff的Buff效果一致,后者将替代前者的Buff
                            {
                                ifAddBuffStatus = false;
                            }
                            else
                            {
                                //获取Buff是否叠加
                                //Debug.Log("清除buffID = " + buffID);
                                string buffAddClass = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffAddClass", "ID", buffID, "SkillBuff_Template");

                                //目前3和1是一样的 3的区别是在1的内部
                                if (buffAddClass == "3")
                                {
                                    buffAddClass = "0";
                                }

                                //if(buffAddClass!=null){
                                if (buffAddClass == "0")
                                {

                                    //注销
                                    //MonoBehaviour.Destroy(buff_4[i]);
                                    //清空buff累计时间
                                    //Debug.Log("有相同的Buff,已经替换");
                                    buff_4[i].buffTimeSum = 0;
                                    ifAddBuffStatus = true;

                                    //宠物光环属性特殊处理
                                    string guanghuaiBuffIDStr = "90040210,90040220,90040230,90041210,90041220,90041230";
                                    string[] guanghuaiBuffIDList = guanghuaiBuffIDStr.Split(',');
                                    for (int y = 0; y < guanghuaiBuffIDList.Length; y++)
                                    {
                                        if (buffID == guanghuaiBuffIDList[y])
                                        {
                                            //注销
                                            //MonoBehaviour.Destroy(buff_4[i]);
                                            //ifAddBuffStatus = true;
                                            buff_4[i].buffTimeSum = 0;
                                            ifAddBuffStatus = false;
                                            break;
                                        }
                                    }

                                    //检测是否有相同的buffID
                                    if (buff_4[i].BuffID == buffID)
                                    {
                                        buff_4[i].buffTimeSum = 0;
                                        ifAddBuffStatus = false;
                                        break;
                                    }

                                    //移动速度特殊处理(只能附加一种减速效，取减速效果最高的)
                                    if (propretyType == "35") {
                                        MonoBehaviour.Destroy(buff_4[i]);
                                    }
                                }

                                //关联buff处理
                                if (buffAddClass == "2")
                                {
                                    string weiYibuffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("WeiYiBuffID", "ID", buffID, "SkillBuff_Template");
                                    string[] weiYiBuffIDList = weiYibuffID.Split(',');
                                    for (int y = 0; y < weiYiBuffIDList.Length; y++)
                                    {
                                        if (buff_4[i].BuffID == weiYiBuffIDList[y] && buffID != weiYiBuffIDList[y])
                                        {
                                            //注销
                                            MonoBehaviour.Destroy(buff_4[i]);
                                            ifAddBuffStatus = true;
                                        }

                                        //如果自身buff和排除buff相同则将时间设置为0   此处主要是
                                        /*
                                        if (buffID == weiYiBuffIDList[y])
                                        {
                                            buff_4[i].buffTimeSum = 0;
                                            ifAddBuffStatus = false;
                                            break;
                                        }
                                        */
                                    }
                                }
                            }
                        }
                    }
                }

                if (ifAddBuffStatus) {
                    Buff_4 addBuff4 = obj.AddComponent<Buff_4>();
                    addBuff4.BuffID = buffID;
                    if (buffTimeSum != 0)
                    {
                        addBuff4.buffTimeSum = buffTimeSum;
                    }
                }

                //Debug.Log("添加buffID = " + buffID);

                break;

            //击退效果
            case "5":
                //Debug.Log("buffID = " + buffID + "触发击退效果");
                string jituiValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffParameter", "ID", buffID, "SkillBuff_Template");

                if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>() == null)
                {
                    if (skillObj != null)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.LookAt(skillObj.transform);
                        //Debug.Log("旋转方向 " + skillObj.gameObject.name);
                    }
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.AddComponent<RoseSkill_FanGun_1>();
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().FanGunSpeedValue = int.Parse(jituiValue);
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().SkillTime = 0.5f;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().MoveStatus = true;
                }
                else {
                    MonoBehaviour.Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>());
                }
                break;

            //眩晕
            case "6":
                obj.AddComponent<Buff_6>().BuffID = buffID;
                break;
        }
        
    }

    //根据目标类型判定Buff附带的目标是否死亡
    public bool ifDeath(string targetType, GameObject targetObj) {

        switch (targetType)
        {
            //玩家
            case "1":
                if (targetObj.GetComponent<Rose_Proprety>().Rose_HpNow <= 0)
                {
                    //Debug.Log("玩家死了Buff注销");
                    return true;
                }
                break;
            //怪物
            case "2":
                if (targetObj.GetComponent<AI_Property>().AI_Hp <= 0)
                {
                    return true;
                }
                break;

            //宠物
            case "3":
                if (targetObj.GetComponent<AI_Property>().AI_Hp <= 0)
                {
                    return true;
                }
                break;
        }
        return false;
    }


    //传入技能ID,播放攻击特效(参数1：技能ID,参数2：释放技能的本体)
    public void PlayActSkillEffect(string skillID, GameObject SelfObj) {

        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", skillID, "Skill_Template");
        if (effectName != "" && effectName != "0") {

            //实例化技能特效
            GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
            GameObject effect = (GameObject)MonoBehaviour.Instantiate(SkillEffect);
            effect.SetActive(false);
            effect.transform.parent = SelfObj.transform;
            effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            effect.transform.localRotation = Quaternion.Euler( new Vector3(0.0f, 0.0f, 0.0f));
            effect.SetActive(true);

        }
    }



    //传入技能ID,播放受击特效




    //传入眩晕时间和眩晕目标,设置目标眩晕状态 (参数一：眩晕时间 参数二：眩晕目标Obj 参数三：目标类型)
    public bool XuanYunBuff(float xuanYunBuffTime, GameObject xuanYunObj, string xuanYunType,string xuanYunBuffType = "0") {

        //获取眩晕类型
        switch (xuanYunType) { 
            
            //己方
            case "1":
                //Debug.Log("进入眩晕");
                //获取目标眩晕状态
                bool xuanYunStatus = xuanYunObj.GetComponent<Rose_Status>().XuanYunStatus;
                float xuanYunTimeSum = xuanYunObj.GetComponent<Rose_Status>().xuanYunTimeSum;
                float xuanYunTime = xuanYunObj.GetComponent<Rose_Status>().XuanYunTime;
                if (xuanYunStatus)
                {
                    //获取目标眩晕时间,保持2个眩晕之间,存在一个眩晕长的
                    if (xuanYunTimeSum < xuanYunTime)
                    {
                        xuanYunObj.GetComponent<Rose_Status>().XuanYunTime = xuanYunBuffTime;
                        xuanYunObj.GetComponent<Rose_Status>().XuanYunStatus = true;
                        //Debug.Log("更新眩晕！111");
                    }
                }
                else {

                    xuanYunObj.GetComponent<Rose_Status>().XuanYunTime = xuanYunBuffTime;
                    xuanYunObj.GetComponent<Rose_Status>().XuanYunStatus = true;
                    //Debug.Log("更新眩晕！222");
                }
                break;

            //敌方
            case "2":
                //Debug.Log("XuanYunStatus");
                //获取目标眩晕状态
                xuanYunStatus = xuanYunObj.GetComponent<AI_1>().XuanYunStatus;
                xuanYunTimeSum = xuanYunObj.GetComponent<AI_1>().xuanYunTimeSum;
                xuanYunTime = xuanYunObj.GetComponent<AI_1>().XuanYunTime;
                if (xuanYunStatus)
                {
                    //获取目标眩晕时间,保持2个眩晕之间,存在一个眩晕长的
                    if (xuanYunTimeSum < xuanYunTime)
                    {
                        //xuanYunStatus = true;
                        //xuanYunTime = xuanYunBuffTime;
                        if (xuanYunBuffType == "0" || xuanYunBuffType == "" || xuanYunBuffType == null)
                        {
                            xuanYunObj.GetComponent<AI_1>().XuanYunStatus = true;
                            xuanYunObj.GetComponent<AI_1>().XuanYunTime = xuanYunBuffTime;
                        }
                        //沉默
                        if (xuanYunBuffType == "1") {
                            xuanYunObj.GetComponent<AI_1>().ChenMoStatus = true;
                            xuanYunObj.GetComponent<AI_1>().ChenMoTime = xuanYunBuffTime;
                        }
                    }
                }
                else
                {
                    //xuanYunStatus = true;
                    //xuanYunTime = xuanYunBuffTime;
                    if (xuanYunBuffType == "0" || xuanYunBuffType == "" || xuanYunBuffType == null)
                    {
                        xuanYunObj.GetComponent<AI_1>().XuanYunStatus = true;
                        xuanYunObj.GetComponent<AI_1>().XuanYunTime = xuanYunBuffTime;
                    }
                    //沉默
                    if (xuanYunBuffType == "1")
                    {
                        xuanYunObj.GetComponent<AI_1>().ChenMoStatus = true;
                        xuanYunObj.GetComponent<AI_1>().ChenMoTime = xuanYunBuffTime;
                    }

                }
                break;
        }

        return true;
    }

    //根据传入的属性类型,降低目标的值

    //设置技能范围（参数1：技能ID 参数2:技能自身的Obj 参数3：释放此技能的Obj）
    public void AddSkillRange(string skillID, GameObject skillObj,GameObject skillActObj = null) {
        

        //获取技能范围数据
        string damgeRangeType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeRangeType", "ID", skillID, "Skill_Template");
        string damgeRange = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeRange", "ID", skillID, "Skill_Template");
        string skillParent = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParent", "ID", skillID, "Skill_Template");
        if (skillParent != "7") {
            if (skillActObj == null)
            {
                skillObj.transform.localRotation = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.localRotation;
            }
            else
            {
                skillObj.transform.localRotation = skillActObj.transform.localRotation;
            }
        }

        //damgeRangeType = "5";



        //清理范围   法师初始不改变
        /*
        if (skillID != "60011001")
        {
            if (damgeRangeType != "0")
            {
                ClearnCollider(skillObj);
            }
        }
        else {
            return;
        }
        */

        if (damgeRangeType != "0")
        {
            ClearnCollider(skillObj);
        }


        string[] rang = damgeRange.Split(';');
        switch(damgeRangeType) {
            //球形
            case "1":

                SphereCollider sphereCollider = skillObj.AddComponent<SphereCollider>();
                sphereCollider.isTrigger = true;
                sphereCollider.radius = float.Parse(damgeRange);
                //Debug.Log("球形状态！" + skillObj.GetComponent<SphereCollider>().isTrigger);

                //技能附加值
                float addSkillAddValue = 0;
                string skillAddValueStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(skillID, "1");
                if (skillAddValueStr != "" && skillAddValueStr != "0")
                {
                    string[] skillAddValueList = skillAddValueStr.Split(',');
                    for (int i = 0; i < skillAddValueList.Length; i++)
                    {
                        addSkillAddValue = addSkillAddValue + float.Parse(skillAddValueList[i]);
                    }
                }

                //设定技能技能范围的位置,而不是居中位置
                if (rang.Length > 1)
                {
                    string[] p_value = rang[1].Split(',');
                    if (p_value.Length > 2)
                    {
                        float p_x = float.Parse(p_value[0]) + addSkillAddValue;
                        float p_y = float.Parse(p_value[1]) + addSkillAddValue;
                        float p_z = float.Parse(p_value[2]) + addSkillAddValue;
                        skillObj.GetComponent<SphereCollider>().center = new Vector3(p_x, p_y, p_z);
                    }
                }
                break;
            //方形
            case "2":
                
                //技能附加值
                addSkillAddValue = 0;
                skillAddValueStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(skillID, "1");
                if (skillAddValueStr != "" && skillAddValueStr != "0")
                {
                    string[] skillAddValueList = skillAddValueStr.Split(',');
                    for (int i = 0; i < skillAddValueList.Length; i++)
                    {
                        addSkillAddValue = addSkillAddValue + float.Parse(skillAddValueList[i]);
                    }
                }

                string[] value = rang[0].Split(',');
                float x = float.Parse(value[0]) + addSkillAddValue;
                float z = float.Parse(value[1]) + addSkillAddValue;

                BoxCollider boxCollider = skillObj.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                boxCollider.size = new Vector3(x, 1, z);

                //设定技能技能范围的位置,而不是居中位置
                if (rang.Length > 1)
                {
                    string[] p_value = rang[1].Split(',');
                    if (p_value.Length > 2)
                    {
                        float p_x = float.Parse(p_value[0]) + addSkillAddValue;
                        float p_y = float.Parse(p_value[1]) + addSkillAddValue;
                        float p_z = float.Parse(p_value[2]) + addSkillAddValue;
                        skillObj.GetComponent<BoxCollider>().center = new Vector3(p_x, p_y, p_z);
                    }
                }
                
                break;

            //对目标立即造成伤害
            case "3":
                SphereCollider sc = skillObj.AddComponent<SphereCollider>();
                sc.radius = 0.5f;
                sc.isTrigger = true;
                break;

            //对目标立即造成伤害
            case "4":
                skillObj.AddComponent<SphereCollider>();
                skillObj.GetComponent<SphereCollider>().radius = 1.0f;
                skillObj.GetComponent<SphereCollider>().isTrigger = true;
                break;

            //对目标立即造成伤害
            case "5":
                skillObj.AddComponent<SphereCollider>();
                skillObj.GetComponent<SphereCollider>().radius = float.Parse(damgeRange);
                skillObj.GetComponent<SphereCollider>().isTrigger = true;

                //技能附加值
                addSkillAddValue = 0;
                skillAddValueStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(skillID, "1");
                if (skillAddValueStr != "" && skillAddValueStr != "0")
                {
                    string[] skillAddValueList = skillAddValueStr.Split(',');
                    for (int i = 0; i < skillAddValueList.Length; i++)
                    {
                        addSkillAddValue = addSkillAddValue + float.Parse(skillAddValueList[i]);
                    }
                }

                //设定技能技能范围的位置,而不是居中位置
                if (rang.Length > 1)
                {
                    string[] p_value = rang[1].Split(',');
                    if (p_value.Length > 2)
                    {
                        float p_x = float.Parse(p_value[0]) + addSkillAddValue;
                        float p_y = float.Parse(p_value[1]) + addSkillAddValue;
                        float p_z = float.Parse(p_value[2]) + addSkillAddValue;

                        skillObj.GetComponent<SphereCollider>().center = new Vector3(p_x, p_y, p_z);
                    }
                }
            break;
        }

        //设置朝向
        if (skillParent != "7")
        {
            string ifLookAtTarget = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfLookAtTarget", "ID", skillID, "Skill_Template");
            if (ifLookAtTarget == "1")
            {
                if (skillActObj != null)
                {
                    skillObj.transform.LookAt(skillActObj.transform);
                }
            }
            else
            {
                if (skillObj.GetComponent<SkillObjBase>() != null)
                {
                    if (skillObj.GetComponent<SkillObjBase>().MonsterSkillObj != null)
                    {
                        skillObj.transform.rotation = skillObj.GetComponent<SkillObjBase>().MonsterSkillObj.transform.rotation;
                    }
                }
            }
        }

        
    }

    private void ClearnCollider(GameObject skillObj) {
        //清理脚本
        BoxCollider[] boxList = skillObj.GetComponents<BoxCollider>();
        for (int i = 0; i < boxList.Length; i++)
        {
            MonoBehaviour.Destroy(boxList[i]);
        }

        //清理脚本
        SphereCollider[] spList = skillObj.GetComponents<SphereCollider>();
        for (int i = 0; i < spList.Length; i++)
        {
            MonoBehaviour.Destroy(spList[i]);
        }

    }

    //返回技能附加值
    public string GetSkillAddValue(string skillID,string addType) {

        //获取角色技能附加值
        string addValueStr = "";
        Rose_Status rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        for (int i = 0; i < rose_Status.Rose_SkillIAddValueList.Count; i++) {
            if (skillID == rose_Status.Rose_SkillIAddValueList[i].SkillID) {
                if (addType == rose_Status.Rose_SkillIAddValueList[i].AddType) {
                    addValueStr = addValueStr + rose_Status.Rose_SkillIAddValueList[i].AddValue + ",";
                }
            }
        }
        if (addValueStr != "") {
            addValueStr = addValueStr.Substring(0, addValueStr.Length - 1);
        }
        else {
            addValueStr = "0";
        }
        return addValueStr;
        //for()

    }

    //技能附加额外BUFF
    public void SkillAddValue_Buff(string skillID, GameObject addObj ) {
        //技能附加值（附加额外Buff）
        string skillAddValueStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(skillID, "6");
        if (skillAddValueStr != "" && skillAddValueStr != "0")
        {
            string[] skillAddValueList = skillAddValueStr.Split(',');
            for (int i = 0; i < skillAddValueList.Length; i++)
            {
                Game_PublicClassVar.Get_function_Skill.SkillBuff(skillAddValueList[i], addObj);
            }
        }
    }


    //技能升级(参数1：技能ID , 参数2：是否删除原本技能ID(0 不删除  1 删除))
    public bool SkillUp(string skillID,string ifDelSkillID){
        
        //获取技能下一级id
        string nextSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextSkillID", "ID", skillID, "Skill_Template");
        if (nextSkillID != "0")
        {
            bool costGoldStatus = false;
            string costGoldValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostGoldValue", "ID", skillID, "Skill_Template");

            //获取学习技能需要的SP
            int costSPValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostSPValue", "ID", skillID, "Skill_Template"));
            //获取自己拥有的SP
            int RoseSP = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillSP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

            //消耗金币
            if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() >= int.Parse(costGoldValue))
            {
                costGoldStatus = true;
            }
            else {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_138");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint("金币不足,请获取足够多的金币!");
                return false;
            }

            

            //扣除SP,学习对应的技能
            if (RoseSP >= costSPValue)
            {
                RoseSP = RoseSP - costSPValue;

                string roseSkillStr = "";
                switch (ifDelSkillID)
                {

                    //不删除
                    case "0":
                        roseSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        if (roseSkillStr != "")
                        {
                            roseSkillStr = roseSkillStr + "," + nextSkillID;
                        }
                        else
                        {
                            roseSkillStr = nextSkillID;
                        }
                        break;
                    //删除
                    case "1":
                        //获取自身的技能数组
                        string[] roseSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
                        for (int i = 0; i <= roseSkill.Length - 1; i++)
                        {
                            if (roseSkill[i] == skillID)
                            {
                                roseSkillStr = roseSkillStr + nextSkillID + ",";
                            }
                            else
                            {
                                roseSkillStr = roseSkillStr + roseSkill[i] + ",";
                            }
                        }
                        //删除最后逗号
                        roseSkillStr = roseSkillStr.Substring(0, roseSkillStr.Length - 1);
                        break;
                }

                //扣除对应金币
                Game_PublicClassVar.Get_function_Rose.CostReward("1", costGoldValue);

                //写入对应数据
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SkillSP", RoseSP.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillID", roseSkillStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

                //PassiveSkill(skillID);
                //获取技能是否为被动技能
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().UpdataPassiveSkillStatus = true;
                Game_PublicClassVar.Get_function_UI.PlaySource("10008", "1");
                return true;
            }
            else {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_310");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

                //Game_PublicClassVar.Get_function_UI.GameGirdHint("SP值不足,每升一级获得1点SP值!");
                //返回升级消耗的金币
                /*
                if (costGoldStatus) {
                    Game_PublicClassVar.Get_function_Rose.SendReward("1", costGoldValue,);
                }
                */
            }
        }
        return false;
    }

    /*
    //传入技能ID,获取自身的被动技能并记录
    public bool PassiveSkill(string skillID) {

        //获取技能是否为被动技能
        string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", skillID, "Skill_Template");
        if (skillType == "2")
        {
            //获取自己的被动技能
            string aa = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().skillIDBeiDong;
            string[] beidongSkillIDStr = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().skillIDBeiDong.Split(';');
            for (int i = 0; i <= beidongSkillIDStr.Length - 1; i++)
            {
                if (beidongSkillIDStr[i] != "")
                {
                    string[] beidongSkillID = beidongSkillIDStr[i].Split(',');
                    if (beidongSkillID[0] == skillID)
                    {
                        //如果附加的被动技能和自己的拥有的被动技能ID一致,则跳出循环,什么也不发生改变
                        return true;
                    }
                }
            }

            //获取被动技能触发概率
            string passiveSkillPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillPro", "ID", skillID, "Skill_Template");
            //设置被动技能的相关值
            string passiveSkillStr = "";
            if (beidongSkillIDStr[0] != "")
            {
                passiveSkillStr = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().skillIDBeiDong + ";" + skillID + "," + passiveSkillPro;
            }
            else
            {
                passiveSkillStr = skillID + "," + passiveSkillPro;
            }
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().skillIDBeiDong = passiveSkillStr;
        }
        return true;
    }

    //初始化被动技能
    public void InitializePassiveSkill() { 
        //获取自身拥有的技能
        string[] roseSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        for (int i = 0; i <= roseSkill.Length - 1; i++) {
            if (roseSkill[i] != "" && roseSkill[i] != "0") {
                //PassiveSkill(roseSkill[i]);
                //获取技能是否为被动技能
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().UpdataPassiveSkillStatus = true;
            }
        }
    }
    */
    //更新装备技能ID
    public void UpdataEquipSkillID() {
        //Debug.Log("更新装备技能");
        Function_DataSet functionDataSet = Game_PublicClassVar.Get_function_DataSet;
        string equipSkillIDStr = "";
        string equipSkillType3_Str = "";            //装备技能类型为3的字符串

        //循环读取装备技能
        for (int i = 1; i <= 13; i++)
        {
            string equipID = functionDataSet.DataSet_ReadData("EquipItemID", "ID", i.ToString(), "RoseEquip");
            //Debug.Log("i = " + i + ";   equipID = " + equipID);
            if (equipID == "0")
            {
                continue;   //立即执行下次循环
            }
            //根据装备ID获取对应的技能ID
            string[] equipSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", equipID, "Item_Template").Split(',');
            for (int y = 0; y <= equipSkillID.Length - 1; y++) {

                if (equipSkillID[y] != "0")
                {
                    //更新装备ID字符串
                    if (equipSkillIDStr == "")
                    {
                        equipSkillIDStr = equipSkillID[y];
                    }
                    else
                    {
                        equipSkillIDStr = equipSkillIDStr + "," + equipSkillID[y];
                    }

                    //更新装备技能类型为3的字符串
                    //获取SkillID的技能类型为3则跳过循环
                    string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", equipSkillID[y], "Skill_Template");
                    if (skillType == "3")
                    {
                        if (equipSkillType3_Str == "")
                        {
                            equipSkillType3_Str = equipSkillID[y];
                        }
                        else
                        {
                            equipSkillType3_Str = equipSkillType3_Str + "," + equipSkillID[y];
                        }
                    }
                }
            }
            //Debug.Log("equipSkillID[0] = " + equipSkillID[0]);
            //Debug.Log("equipSkillIDStr = " + equipSkillIDStr);


			//写入隐藏技能数据
			string hideID = functionDataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseEquip");
            string hideProStr = functionDataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");
            //Debug.Log("hideID = " + hideID);
            string[] hideIDList = hideProStr.Split(';');
			//string hideSkillStr = "";
			//string[] hideSkillIDList = hideSkillStr.Split(',');
			for (int y = 0; y <= hideIDList.Length - 1; y++) {
                //Debug.Log("y = " + y + ";hideIDList[y] = " + hideIDList[y]);
				if (hideIDList[y] != "0" && hideIDList[y] != "")
				{
					string[] hideSkillIDList = hideIDList[y].Split(',');

					string hideType = hideSkillIDList[0];
					string hideSkillID = hideSkillIDList[1];

					if(hideType=="101"|| hideType == "10001")
                    {

						//更新装备ID字符串
						if (equipSkillIDStr == "")
						{
							equipSkillIDStr = hideSkillID;
						}
						else
						{
							equipSkillIDStr = equipSkillIDStr + "," + hideSkillID;
						}

						//更新装备技能类型为3的字符串
						//获取SkillID的技能类型为3则跳过循环
						string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", hideSkillID, "Skill_Template");
						if (skillType == "3")
						{
							if (equipSkillType3_Str == "")
							{
								equipSkillType3_Str = hideSkillID;
							}
							else
							{
								equipSkillType3_Str = equipSkillType3_Str + "," + hideSkillID;
							}
						}
					}
				}
			}
        }

        //读取天赋技能
        string LearnTianFuIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianFuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] LearnTianFuID = LearnTianFuIDStr.Split(';');
        //循环每个天赋的加成属性信息
        for (int i = 0; i <= LearnTianFuID.Length - 1; i++)
        {
            if (LearnTianFuID[i].Split(',').Length >= 2) {
                string nowTianFuID = LearnTianFuID[i].Split(',')[0];
                int nowTianFuLv = int.Parse(LearnTianFuID[i].Split(',')[1]);

                string talentSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddSkillID", "ID", nowTianFuID, "Talent_Template");
                if (talentSkillID != "" && talentSkillID != "0")
                {

                    string[] talentSkillIDList = talentSkillID.Split(';');
                    if (nowTianFuLv > 0)
                    {
                        nowTianFuLv = nowTianFuLv - 1;
                        if (nowTianFuLv <= 0)
                        {
                            nowTianFuLv = 0;
                        }

                        if (talentSkillIDList[nowTianFuLv] != "" && talentSkillIDList[nowTianFuLv] != "0")
                        {
                            string[] addProprtValue = talentSkillIDList[nowTianFuLv].Split(';');
                            for (int y = 0; y < addProprtValue.Length; y++)
                            {
                                if (addProprtValue[y] != "0")
                                {
                                    //更新装备ID字符串
                                    if (equipSkillIDStr == "")
                                    {
                                        equipSkillIDStr = addProprtValue[y];
                                    }
                                    else
                                    {
                                        equipSkillIDStr = equipSkillIDStr + "," + addProprtValue[y];
                                    }

                                    //更新装备技能类型为3的字符串
                                    //获取SkillID的技能类型为3则跳过循环
                                    string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", addProprtValue[y], "Skill_Template");
                                    if (skillType == "3")
                                    {
                                        if (equipSkillType3_Str == "")
                                        {
                                            equipSkillType3_Str = addProprtValue[y];
                                        }
                                        else
                                        {
                                            equipSkillType3_Str = equipSkillType3_Str + "," + addProprtValue[y];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //读取觉醒数据
        //获取当前携带的觉醒技能(被动)
        string JueXingJiHuoIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] jueXingJiHuoIDList = JueXingJiHuoIDSet.Split(';');
        for (int i = 0; i < jueXingJiHuoIDList.Length; i++)
        {
            string juexingSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddSkill", "ID", jueXingJiHuoIDList[i], "JueXing_Template");
            //获取职业
            string roseOcc = Game_PublicClassVar.Get_function_Rose.GetRoseOcc();
            string[] SkillStrList = juexingSkillStr.Split('@');
            if (SkillStrList.Length >= 2)
            {
                switch (roseOcc)
                {
                    case "1":
                        juexingSkillStr = SkillStrList[0];
                        break;

                    case "2":
                        juexingSkillStr = SkillStrList[1];
                        break;

                    case "3":
                        juexingSkillStr = SkillStrList[2];
                        break;

                }
            }

            if (juexingSkillStr != "" && juexingSkillStr != "0")
            {
                equipSkillIDStr = equipSkillIDStr + "," + juexingSkillStr;
            }

            //更新装备技能类型为3的字符串
            //获取SkillID的技能类型为3则跳过循环
            string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", juexingSkillStr, "Skill_Template");
            if (skillType == "3")
            {
                if (equipSkillType3_Str == "")
                {
                    equipSkillType3_Str = juexingSkillStr;
                }
                else
                {
                    equipSkillType3_Str = equipSkillType3_Str + "," + juexingSkillStr;
                }
            }

        }


        //Debug.Log("equipSkillType3_Str = " + equipSkillType3_Str);
        //写入对应数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipSkillID", equipSkillIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        
        //更新技能ID
        string learnSkillStrSet = "";
        //获取自身的技能数组
        string roseSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] roseSkill = roseSkillStr.Split(',');

        //获取类型为3的技能
        string[] equipSkillType3 = equipSkillType3_Str.Split(',');
        //Debug.Log("equipSkillType3 = " + equipSkillType3_Str);
        //循环判定自身有没有对应的技能ID
     
        for (int i = 0; i <= roseSkill.Length - 1; i++)
        {
            for (int y = 0; y <= equipSkillType3.Length - 1; y++)
            {
                //获取升级附加数据
                if (equipSkillType3[0] == "") {
                    break;
                }
                string[] equipSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkill", "ID", equipSkillType3[y], "Skill_Template").Split(';');
                for (int x = 0; x <= equipSkill.Length - 1; x++)
                {
                    string equipSkillID_1 = equipSkill[x].Split(',')[0];
                    string equipSkillID_2 = equipSkill[x].Split(',')[1];

                    if (roseSkill[i] == equipSkillID_1)
                    {
                        //Debug.Log("equipSkillID_2 = " + equipSkillID_2 + "equipSkillID_1 = " + equipSkillID_1);
                        roseSkill[i] = equipSkillID_2;      //更换升级后的技能
                        Game_PublicClassVar.Get_function_Skill.UpdataMainUISkillID(equipSkillID_1, roseSkill[i]);        //替换主界面图标
                        //Debug.Log("技能替换：更换了技能");
                        //立即跳出循环
                        break;
                    }
                }
            }

            if (learnSkillStrSet != "" && learnSkillStrSet != "0")
            {
                learnSkillStrSet = learnSkillStrSet + "," + roseSkill[i];
            }
            else
            {
                learnSkillStrSet = roseSkill[i];
            }
        }

        //Debug.Log("learnSkillStrSet = " + learnSkillStrSet);
        //写入对应数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillIDSet", learnSkillStrSet, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //装备最终数据
        string equipSkillIDStrSet = "";
        string equipSkillID_Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        
        //获取自身套装携带的技能
        string equipSuitSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSuitSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (equipSuitSkillStr != "")
        {
            if (equipSkillID_Str != "")
            {
                equipSkillID_Str = equipSkillID_Str + "," + equipSuitSkillStr;
            }
            else {
                equipSkillID_Str = equipSuitSkillStr;
            }
        }

        //Debug.Log("equipSkillID_Str = " + equipSkillID_Str);
         
        //获取自身的装备技能数组
        string[] equipSkillStrID = equipSkillID_Str.Split(',');
        //循环判定自身有没有对应的技能ID
        for (int i = 0; i <= equipSkillStrID.Length - 1; i++)
        {
            if (equipSkillStrID[0] == "") {
                break;
            }
            string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", equipSkillStrID[i], "Skill_Template");
            if (skillType == "3") {
                continue;     //立即执行下次循环
            }

            for (int y = 0; y <= equipSkillType3.Length - 1; y++)
            {
                //如果附加等级类技能为空则直接跳出循环
                if (equipSkillType3[0] == "")
                {
                    break;
                }

                //获取升级附加数据
                string[] equipSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkill", "ID", equipSkillType3[y], "Skill_Template").Split(';');
                for (int x = 0; x <= equipSkill.Length - 1; x++)
                {
                    string equipSkillID_1 = equipSkill[x].Split(',')[0];
                    string equipSkillID_2 = equipSkill[x].Split(',')[1];

                    if (equipSkillStrID[i] == equipSkillID_1)
                    {
                        equipSkillStrID[i] = equipSkillID_2;      //更换升级后的技能
                        //Debug.Log("更换了技能");
                        //立即跳出循环
                        break;
                    }
                }
            }

            //获取前面是否有相同的技能ID
            bool xiangTongID = false;
            for (int z = 0; z < i; z++)
            {
                if (equipSkillStrID[i] == equipSkillStrID[z]) {
                    xiangTongID = true;
                }
            }
            if (!xiangTongID) {
                if (equipSkillIDStrSet != "" && equipSkillIDStrSet != "0")
                {
                    equipSkillIDStrSet = equipSkillIDStrSet + "," + equipSkillStrID[i];
                }
                else
                {
                    equipSkillIDStrSet = equipSkillStrID[i];
                }
            }
        }

        //写入对应数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipSkillIDSet", equipSkillIDStrSet, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");


        //获取当前快捷技能
        //获取当前拥有的全部技能列表
        string piPeiSkillIDStr = "";
        string LearnSkillIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (LearnSkillIDSet != "")
        {
            piPeiSkillIDStr = LearnSkillIDSet + "," + equipSkillIDStrSet;
        }
        else
        {
            piPeiSkillIDStr = equipSkillIDStrSet;
        }

        //匹配精灵技能
        string jingLingSkillStr = "";
        string jingLingEquipIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JingLingEquipID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (jingLingEquipIDStr != "") {
            string[] jingLingEquipIDList = jingLingEquipIDStr.Split(';');
            for (int i = 0; i < jingLingEquipIDList.Length; i++) {
                string jingLingSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkill", "ID", jingLingEquipIDList[i], "Spirit_Template");
                jingLingSkillStr = jingLingSkillStr + jingLingSkill + ",";
            }
        }
        if (jingLingSkillStr != "") {
            jingLingSkillStr = jingLingSkillStr.Substring(0, jingLingSkillStr.Length - 1);
        }

        if (piPeiSkillIDStr != "") {
            piPeiSkillIDStr = piPeiSkillIDStr + "," + jingLingSkillStr;
        }

        //获取当前拥有的全部天赋技能
        string learnTianSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (learnTianSkillID != "")
        {
            piPeiSkillIDStr = piPeiSkillIDStr + "," + learnTianSkillID;
        }

        //对比技能ID
        string[] piPeiSkillID = piPeiSkillIDStr.Split(',');
        //Debug.Log("piPeiSkillIDStr = " + piPeiSkillIDStr);
        for (int i = 1; i <= 8; i++)
        {
            string skillIDValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (skillIDValue != "") {
                bool ifShowSkillID = false;

                if (piPeiSkillID[0] != "")
                {
                    for (int x = 0; x <= piPeiSkillID.Length - 1; x++)
                    {
                        if (skillIDValue == piPeiSkillID[x])
                        {
                            ifShowSkillID = true;
                        }
                        else { 
                            //判定装备上的技能附加属性是否需要替换
                            /*
                            if (skillIDValue != "")
                            {

                            }
                            */
                        }
                    }
                }

                //获取当前ID是否为道具
                if (skillIDValue.Substring(0, 1) == "1") {
                    ifShowSkillID = true;
                }

                //写入快捷技能
                if (!ifShowSkillID)
                {
                    //Debug.Log("更新技能：" + i);
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainSkillUI_" + i, "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                    Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;  //更新技能图标
                }
            }
        }
        //更新被动技能
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().UpdataPassiveSkillStatus = true;
        //Debug.Log("更新了装备的技能");
    }

    //传入技能ID判定并获取加成后的技能ID
    public string ReturnAddSkillID(string skillID){

        string returnSkillID = skillID;

        return skillID;

    }

    //替换主界面快捷施法技能ID
    //参数1：替换源ID   参数2：替换后ID
    public void UpdataMainUISkillID(string skillID_1,string skillID_2) {

        for (int i = 1; i <= 8; i++) {
            string mainUISkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (mainUISkillID == skillID_1) {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainSkillUI_" + i, skillID_2, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
            }
        }

    }

    //传入要卸下的装备技能ID,主界面快捷施法更换技能ID
    public void UpdataMainUIEquipSkillID(string equipSkillID) {
        //ID为空结束
        if (equipSkillID == "" && equipSkillID == "0") {
            return;
        }
        string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", equipSkillID, "Skill_Template");
        //不为被动技能结束
        if (skillType != "3")
        {
            return;     //立即执行下次循环
        }

        //检测并替换装备附加技能ID
        string[] equipSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkill", "ID", equipSkillID, "Skill_Template").Split(';');
        for (int x = 0; x <= equipSkill.Length - 1; x++)
        {
            string equipSkillID_1 = equipSkill[x].Split(',')[0];
            string equipSkillID_2 = equipSkill[x].Split(',')[1];
            for (int i = 1; i <= 8; i++)
            {
                string mainUISkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                if (mainUISkillID == equipSkillID_2)
                {
                    Game_PublicClassVar.Get_function_Skill.UpdataMainUISkillID(equipSkillID_2, equipSkillID_1);        //替换主界面图标
                }
            }
        }
    }

    //技能重置
    public void RoseReSkillSP()
    {
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_316");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint("技能重置成功!");

        //替换主界面图标
        string[] changeStringID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        for (int i = 0; i <= changeStringID.Length - 1; i++)
        {
            Debug.Log("changeStringID_" + i + " = " + changeStringID[i]);
            Game_PublicClassVar.Get_function_Skill.UpdataMainUISkillID(changeStringID[i], "");        //替换主界面图标
        }

        //更改技能数据
        //获取当前职业
        string occType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string learnSkillID = "60010101,60010200,60010300,60010400,60010500,60010600";  //默认战士
        switch (occType)
        {
            //战士初始技能
            case "1":
                learnSkillID = "60010101,60010200,60010300,60010400,60010500,60010600";
                break;
            //法师初始技能
            case "2":
                learnSkillID = "60011011,60011020,60011030,60011040,60011050,60011060";
                break;
            //法师初始技能
            case "3":
                learnSkillID = "60012011,60012020,60012030,60012040,60012050,60012060";
                break;

        }
        
        Debug.Log("learnSkillID = " + learnSkillID);
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillID", learnSkillID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillIDSet", learnSkillID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SkillSP", roseLv, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //关闭技能界面OpenRoseSkill
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseSkill();

        //更新主界面图标显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;

        //写入成就(重置天赋次数)
        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("108", "0", "1");
    }

    //天赋重置
    public void TianFuChongZhi()
    {

        if (Game_PublicClassVar.Get_function_Rose.GetRoseRMB() < 600)
        {
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_86");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足！");
            return;
        }
        else {
            Game_PublicClassVar.Get_function_Rose.CostRMB(600);
        }

        //写入重置天赋
        //获取当前职业
        string occType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string learnTianFuID = "";
        switch (occType)
        {
            //战士初始技能
            case "1":
                learnTianFuID = "101101,0;111101,0";
                break;
            //法师初始技能
            case "2":
                learnTianFuID = "201101,0;211101,0";
                break;
            //猎人初始技能
            case "3":
                learnTianFuID = "301101,0;311101,0";
                break;
        }

        //读取天赋点数
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        int nowSPNum = roseLv - 9;

        //读取天赋技能
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnTianFuID",learnTianFuID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SP", nowSPNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnTianSkillID", nowSPNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_298");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("天赋重置成功！");

        //写入成就
        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("108", "0", "1");

        //初始化天赋显示
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseSkill.GetComponent<UI_RoseSkill>().Obj_SkillTianFuSet.GetComponent<UI_TianFu>().Init();
    }

    //检测当前是否有空的快捷技能栏位
    public string GetNullMainSkillUI()
    {

        for (int i = 1; i <= 8; i++)
        {
            string skillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (skillID == "" || skillID == "0")
            {
                return i.ToString();
            }
        }

        return "-1";
    }


    //传入一个技能ID,返回该技能ID是否已经装备中
    public bool IfEquipMainSkill(string skillID) {

        for (int i = 1; i <= 8; i++)
        {
            string mainSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (mainSkillID == skillID)
            {
                return true;
            }
        }

        return false;

    }

    //传入一个目标获取此目标的类型（返回值：1.表示玩家  2.表示宠物  3.表示怪物）
    public string returnObjType(GameObject obj) {

        //角色
        if (obj.GetComponent<Rose_Status>() != null)
        {
            return "1";
        }
        //宠物
        if (obj.GetComponent<AIPet>() != null)
        {
            return "2";
        }

        //怪物
        if (obj.GetComponent<AI_1>() != null)
        {
            return "3";
        }
        return "0";

    }

    //直接触发一个技能
    public void UseSkill(string skillID,GameObject skillParObj = null) {


        if (skillID != "0")
        {
            GameObject skill_0 = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainFunctionUI.transform.Find("UI_MainRoseSkill_0").gameObject;
            skill_0.GetComponent<MainUI_SkillGrid>().SkillID = skillID;
            skill_0.GetComponent<MainUI_SkillGrid>().UseSkillID = skillID;
            skill_0.GetComponent<RoseSkill_Sing_1>().SkillParObj = skillParObj;
            skill_0.GetComponent<MainUI_SkillGrid>().updataSkill();
            skill_0.GetComponent<MainUI_SkillGrid>().cleckbutton();
        }

    }

    //获取技能的吟唱时间
    public float GetSkillSingTime(string SkillID) {

        //技能吟唱时间缩减
        float skillSingTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillFrontSingTime", "ID", SkillID, "Skill_Template"));
        string skillAddValueStrPro = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(SkillID, "11");
        if (skillAddValueStrPro == "") {
            skillAddValueStrPro = "0";
        }
        skillSingTime = skillSingTime - float.Parse(skillAddValueStrPro);

        if (skillSingTime <= 0) {
            skillSingTime = 0;
        }
        return skillSingTime;
    }

}
