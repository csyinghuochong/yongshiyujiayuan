using UnityEngine;
using System.Collections;

public class Function_MonsterSkill{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //加血,恢复当前血量上限
    public void MonsterAddHp(GameObject monsterObj, string skillID) {

        if (monsterObj == null) {
            return;
        }

        if (monsterObj.GetComponent<AI_Property>() != null) {
            AI_Property ai_property = monsterObj.GetComponent<AI_Property>();
            //Debug.Log("AI的前血量 = " + ai_property.AI_Hp);
            //获取本次治疗血量
            string actDamge = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActDamge", "ID", skillID, "Skill_Template");
            string damgeValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", skillID, "Skill_Template");
            //Debug.Log("ai_property = " + ai_property.name);
            //Debug.Log("ai_property.AI_Hp = " + ai_property.AI_Hp + " actDamge = " + actDamge + "damgeValue = " + damgeValue + "ai_property.AI_HpMax = " + ai_property.AI_HpMax);
            ai_property.AI_Hp = ai_property.AI_Hp + int.Parse(damgeValue) + (int)(float.Parse(actDamge) * (float)(ai_property.AI_HpMax));
        }
    }

    //怪物附加技能
    public void AddSkillID(string skillID,GameObject MonsterObj) {

        //1表示AI，2表示玩家自身
        string targetType = "1";
        if (MonsterObj == Game_PublicClassVar.Get_game_PositionVar.Obj_Rose) {
            targetType = "2";
        }

        //触发BUFF
        string skillObjName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectName", "ID", skillID, "Skill_Template");
        GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill_Obj/" + skillObjName, typeof(GameObject));
        if (SkillObj == null) {
            //Debug.Log("技能GameObjectName为空，skillID = " + skillID);
            return;
        }
        GameObject SkillObject_p = (GameObject)MonoBehaviour.Instantiate(SkillObj);
        if (targetType == "1") {
            SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = MonsterObj.GetComponent<AI_1>().AI_Target;
        }
        if (targetType == "2") {
            SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = MonsterObj.GetComponent<Rose_Status>().Obj_ActTarget;
        }

        string skillParent = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParent", "ID", skillID, "Skill_Template");
        //Debug.Log("间接触发技能+" + skillID);
        /*
        if (skillID[i] == "62000103") {
            Debug.Log("触发召唤技能");
        }
        */
        //skillParent = "2";      //测试无绑定点用的,后面需删掉
        //Debug.Log("skillParent = " + skillParent);
        switch (skillParent)
        {
            //绑定在身上
            case "0":
                //目前只支持对自己附加
                //Debug.Log("技能挂在自己身上+" + skillID[i]);
                string skillParentPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID, "Skill_Template");
                if (targetType == "1") {
                    SkillObject_p.transform.SetParent(MonsterObj.GetComponent<AI_1>().BoneSet.transform.Find(skillParentPosition));
                }
                if (targetType == "2") {
                    SkillObject_p.transform.SetParent(MonsterObj.GetComponent<Rose_Bone>().BoneSet.transform.Find(skillParentPosition));
                }

                SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);
                SkillObject_p.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID;
                SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = MonsterObj;
                //triggerSkillID = skillID;
                //SkillStatus = true;         //开启技能状态
                break;
            //无绑定点
            case "1":
                //目前只支持对攻击目标区域释放
                //获取攻击目标位置
                Vector3 skillPosition = new Vector3(0,0,0);
                if (targetType == "1")
                {
                    skillPosition = MonsterObj.GetComponent<AI_1>().AI_Target.transform.position;
                }
                if (targetType == "2")
                {
                    skillPosition = MonsterObj.GetComponent<Rose_Status>().Obj_ActTarget.transform.position;
                }

                SkillObject_p.transform.position = skillPosition;
                //skillID[i] = "60090002";
                SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID;
                SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = MonsterObj;

                if (skillID == "62002106")
                {
                    Debug.Log("SkillObject_p = " + SkillObject_p.transform.localRotation);
                }

                //SkillStatus = true;         //开启技能状态
                //triggerSkillID = skillID[i];
                break;

            //无绑定点,释放起始位置位于AI中心
            case "2":
                //目前只支持对攻击目标区域释放
                //获取攻击目标位置
                skillPosition = new Vector3(0, 0, 0);
                if (targetType == "1")
                {
                    skillPosition = MonsterObj.GetComponent<AI_1>().AI_Target.transform.position;
                }
                if (targetType == "2")
                {
                    skillPosition = MonsterObj.GetComponent<Rose_Status>().Obj_ActTarget.transform.position;
                }

                string playStartPoisition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID, "Skill_Template");
                if (targetType == "1")
                {
                    SkillObject_p.transform.position = MonsterObj.GetComponent<AI_1>().BoneSet.transform.Find(playStartPoisition).transform.position;
                }
                if (targetType == "2")
                {
                    SkillObject_p.transform.position = MonsterObj.GetComponent<Rose_Bone>().BoneSet.transform.Find(playStartPoisition).transform.position;
                }

                //SkillObject_p.transform.position = new Vector3(SkillObject_p.transform.position.x + Random.value * 5 - 2.5f, SkillObject_p.transform.position.y, SkillObject_p.transform.position.z + Random.value * 5 - 2.5f);

                //skillID[i] = "60090002";
                SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID;
                SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = MonsterObj;
                SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = MonsterObj.GetComponent<AI_1>().AI_Target;



                //SkillStatus = true;         //开启技能状态
                //triggerSkillID = skillID[i];
                break;

            //目标随机绑点
            //无绑定点,释放起始位置位于AI中心
            case "3":
                //目前只支持对攻击目标区域释放
                //获取攻击目标位置
                if (targetType == "1")
                {
                    skillPosition = MonsterObj.GetComponent<AI_1>().AI_Target.transform.position;
                }
                if (targetType == "2")
                {
                    skillPosition = MonsterObj.GetComponent<Rose_Status>().Obj_ActTarget.transform.position;
                }
                playStartPoisition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID, "Skill_Template");
                //SkillObject_p.transform.position = MonsterObj.GetComponent<AI_1>().BoneSet.transform.Find(playStartPoisition).transform.position;
                if (targetType == "1")
                {
                    SkillObject_p.transform.position = MonsterObj.GetComponent<AI_1>().BoneSet.transform.Find(playStartPoisition).transform.position;
                }
                if (targetType == "2")
                {
                    SkillObject_p.transform.position = MonsterObj.GetComponent<Rose_Bone>().BoneSet.transform.Find(playStartPoisition).transform.position;
                }
                skillPosition = new Vector3(SkillObject_p.transform.position.x + Random.value * 10 - 5f, SkillObject_p.transform.position.y, SkillObject_p.transform.position.z + Random.value * 10 - 5f);
               
                //skillID[i] = "60090002";
                SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID;
                SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = MonsterObj;
                SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = MonsterObj.GetComponent<AI_1>().AI_Target;
                break;

        }
    
    }
}
