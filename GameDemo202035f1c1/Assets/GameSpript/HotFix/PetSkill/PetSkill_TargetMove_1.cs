using UnityEngine;
using System.Collections;

//怪物远程攻击技能
public class PetSkill_TargetMove_1 : MonoBehaviour
{

	//定义技能状态
	public float SkillEffectDistance;   //技能每秒移动距离
    public float SkillTime;             //技能时间（特效和效果共用）
    public int DamgeValue_Fixed;        //技能伤害
	public GameObject ActTargetObj;     
    public Vector3 SkillTargetPoint;    //技能移动到的点，根据此点判定移动的方向

    private GameObject AI_Collider;     //碰撞的怪物
    private string AI_HitPosition;      //AI受到伤害特效显示的位置
    private string AI_IfHitEffect;      //是否播放受击特效
    public GameObject SkillActObj;     //技能攻击目标
    private string ParameterValue;      //参数值

    private float SkillMoveTimeSum;     //技能时间

    //设置技能出生点
    public Vector3 Vec3_Start;
    public Vector3 Vec3_End;

	// Use this for initialization
	void Start () {
        //Debug.Log("触发：火球术！！！！！！！！！！！！！！！！");
        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID,  "Skill_Template");
        //技能时间
        SkillTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        //获取受击特效播放特效
        //AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取技能
        SkillEffectDistance = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillMoveSpeed", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        
        //设置技能范围
        Game_PublicClassVar.Get_function_Skill.AddSkillRange(this.gameObject.GetComponent<SkillObjBase>().SkillID, this.gameObject);

        //获取技能攻击目标
        SkillActObj = this.GetComponent<SkillObjBase>().SkillTargetObj;

        //设置技能位置
        ParameterValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (ParameterValue != "0" && ParameterValue != "" && ParameterValue != "-1")
        {
            string[] parValue = ParameterValue.Split(';');
            switch (parValue[0]) { 
                //从A点移动到B点
                case "1":
                    string[] startVec3 = parValue[1].Split(',');
                    string[] endVec3 = parValue[2].Split(',');
                    Vec3_Start.x = float.Parse(startVec3[0]);
                    Vec3_Start.y = float.Parse(startVec3[1]);
                    Vec3_Start.z = float.Parse(startVec3[2]);
                    Vec3_End.x = float.Parse(endVec3[0]);
                    Vec3_End.y = float.Parse(endVec3[1]);
                    Vec3_End.z = float.Parse(endVec3[2]);
                    break;
                //从A点移动到攻击目标处
                case "2":
                    startVec3 = parValue[1].Split(',');
                    Vec3_Start.x = float.Parse(startVec3[0]);
                    Vec3_Start.y = float.Parse(startVec3[1]);
                    Vec3_Start.z = float.Parse(startVec3[2]);
                    Vec3_End = SkillActObj.transform.position;
                    Vec3_End.y = Vec3_Start.y;
                    break;

                case "3":

                    Vec3_Start = new Vector3(SkillActObj.transform.position.x + (Random.value - 0.5f) * 10.0f, SkillActObj.transform.position.y, SkillActObj.transform.position.z + (Random.value - 0.5f) * 10.0f);

                    if (Vector3.Distance(Vec3_Start, SkillActObj.transform.position) <= 3.0f) {
                        Vec3_Start = new Vector3(SkillActObj.transform.position.x + 3.0f, SkillActObj.transform.position.y, SkillActObj.transform.position.z + 3.0f);
                    }

                    Vec3_End = new Vector3(SkillActObj.transform.position.x + (Random.value - 0.5f) * 5.0f, SkillActObj.transform.position.y, SkillActObj.transform.position.z + (Random.value - 0.5f) * 5.0f);
                    Vec3_End.y = Vec3_Start.y;
                    break;
            }

            //设置技能初始位置
            this.gameObject.transform.position = Vec3_Start;
            this.gameObject.transform.LookAt(Vec3_End);
        }

        //实例化技能特效
        //Game_PublicClassVar.Get_function_Skill.PlayActSkillEffect(this.gameObject.GetComponent<SkillObjBase>().SkillID, this.gameObject);


        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (effectName != "" && effectName != "0")
        {

            //实例化技能特效
            GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
            GameObject effect = (GameObject)MonoBehaviour.Instantiate(SkillEffect);
            effect.SetActive(false);
            effect.transform.parent = this.gameObject.transform;
            string playStartPoisition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
            this.transform.position = this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AIPet>().BoneSet.transform.Find(playStartPoisition).transform.position;
            effect.transform.localPosition = Vector3.zero;
            effect.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
            //effect.transform.LookAt(SkillActObj.transform);
            effect.SetActive(true);


            if (ParameterValue != "0" && ParameterValue != "" && ParameterValue != "-1")
            {
                //设置技能初始位置
                effect.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
                this.gameObject.transform.position = Vec3_Start;
                this.gameObject.transform.LookAt(Vec3_End);
            }
        }

        
        //获取技能是否有释放目标点
        //string ifSelectSkillRange = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfSelectSkillRange", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (SkillActObj != null)
        {
            //设置一定方向及位置
            Transform hitPosition = this.transform;
            if (SkillActObj.GetComponent<AI_1>() != null) {
                hitPosition = SkillActObj.GetComponent<AI_1>().BoneSet.transform.Find("Center").transform;        //假设命中怪物,后期如果怪物打人需要此处调整
            }
            if (SkillActObj.GetComponent<AIPet>() != null)
            {
                hitPosition = SkillActObj.GetComponent<AIPet>().BoneSet.transform.Find("Center").transform;        //假设命中怪物,后期如果怪物打人需要此处调整
            }
            this.gameObject.transform.LookAt(hitPosition);                      //修正技能施放方向
            this.gameObject.transform.localRotation = new Quaternion(0, this.gameObject.transform.localRotation.y, this.gameObject.transform.localRotation.z, this.gameObject.transform.localRotation.w);
        }

        //技能时间到注销此物体
        Destroy(this.gameObject,SkillTime);
	}
	
	// Update is called once per frame
	void Update () {

        SkillMoveTimeSum = SkillMoveTimeSum + Time.deltaTime;

        if (SkillMoveTimeSum >= 5.0f) { 
            string[] parValue = ParameterValue.Split(';');
            if (parValue[0] == "3") {
                Vector3 saveVec3 = Vec3_Start;
                Vec3_Start = Vec3_End;
                Vec3_End = saveVec3;
                this.gameObject.transform.LookAt(Vec3_End);
                SkillMoveTimeSum = 0;
            } 
        }

        if (ActTargetObj != null) {
            this.gameObject.transform.LookAt(ActTargetObj.transform);
        }

        //设置移动
        transform.Translate(Vector3.forward * SkillEffectDistance * Time.deltaTime);


	}

	//第一次碰撞调用
	void OnTriggerEnter (Collider collider){
        //碰撞体碰到目标后给予技能伤害,并销毁自己
		AI_Collider = collider.gameObject;
        if (AI_Collider == SkillActObj)
        {
            //Debug.Log("AI_Collider = " + AI_Collider.name);

            if (collider.gameObject.GetComponent<AI_1>()) {

            }


            //AI_1 rose_Proprety = collider.gameObject.GetComponent<AI_1>();

            //if (rose_Proprety != null)
            //{
            //是否播放受击特效
            switch (AI_IfHitEffect)
            {
                case "0":
                    if (collider.gameObject.GetComponent<AI_1>()!=null)
                    {
                        collider.gameObject.GetComponent<AI_1>().IfHitEffect = false;   //不播放
                    }
                    if (collider.gameObject.GetComponent<AIPet>() != null)
                    {
                        collider.gameObject.GetComponent<AIPet>().IfHitEffect = false;   //不播放
                    }
                break;
                case "1":

                    //获取特效名称
                    string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                    if (collider.gameObject.GetComponent<AI_1>() != null)
                    {
                        if (effectName != "0")
                        {
                            effectName = "Eff_Hit_Fire_3";
                            collider.GetComponent<AI_1>().HitEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName);
                        }
                        collider.gameObject.GetComponent<AI_1>().IfHitEffect = true;    //播放
                        collider.gameObject.GetComponent<AI_1>().HitEffectt_Position = collider.gameObject.GetComponent<AI_1>().BoneSet.transform.Find(AI_HitPosition).gameObject;  //设置播放位置
                    }

                    if (collider.gameObject.GetComponent<AIPet>() != null)
                    {
                        if (effectName != "0")
                        {
                            effectName = "Eff_Hit_Fire_3";
                            collider.GetComponent<AIPet>().HitEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName);
                        }
                        collider.gameObject.GetComponent<AIPet>().IfHitEffect = true;    //播放
                        collider.gameObject.GetComponent<AIPet>().HitEffectt_Position = collider.gameObject.GetComponent<AIPet>().BoneSet.transform.Find(AI_HitPosition).gameObject;  //设置播放位置
                    }

                break;
            }

            //AI_1 rose_Status = collider.gameObject.GetComponent<AI_1>();
            //rose_Status.HitStatus = true; //受击特效
            if (collider.gameObject.GetComponent<AI_1>() != null)
            {
                collider.gameObject.GetComponent<AI_1>().HitStatus = true; //受击特效
            }
            if (collider.gameObject.GetComponent<AIPet>() != null)
            {
                collider.gameObject.GetComponent<AIPet>().HitStatus = true; //受击特效
            }

            //获取攻击目标
            GameObject actObj = this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AIPet>().AI_Target;
            if (actObj != null)
            {
                Game_PublicClassVar.Get_fight_Formult.PetActMonster(this.GetComponent<SkillObjBase>().MonsterSkillObj, this.GetComponent<SkillObjBase>().SkillID, actObj, false);
            }

            //触发技能Buff
            string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
            if (buffID != "0" && buffID != "") {
                Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
            }
            //}

            //碰撞到目标后注销自己
            //Debug.Log("销毁自己！！！！！！！！！！！ParameterValue = " + ParameterValue);
            if (ParameterValue == "0") {
                //Debug.Log("销毁自己222222！！！！！！！！！！！");
                Destroy(this.gameObject);
            }
        }
	}

	//碰撞范围内调用
	void OnTriggerStay(Collider collider){

        

	}


	//离开碰撞调用
	void OnTriggerExit (Collider collider){
	
	
	}


}
