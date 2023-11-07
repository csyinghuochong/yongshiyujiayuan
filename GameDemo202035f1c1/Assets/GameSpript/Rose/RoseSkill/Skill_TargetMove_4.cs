using UnityEngine;
using System.Collections;

//冲击波技能 可以配置移动速度和受到的伤害
public class Skill_TargetMove_4 : MonoBehaviour
{

	//定义技能状态
	public float SkillEffectDistance;   //技能每秒移动距离
    public float SkillTime;             //技能时间（特效和效果共用）
    public int DamgeValue_Fixed;        //技能伤害
    public Vector3 SkillTargetPoint;    //技能移动到的点，根据此点判定移动的方向

    private GameObject AI_Collider;     //碰撞的怪物
    private string skillHitEffectName;  //技能特效
    private string AI_HitPosition;      //AI受到伤害特效显示的位置
    private string AI_IfHitEffect;      //是否播放受击特效
    private GameObject SkillActObj;     //技能攻击目标
    private string SkillPar;            //技能参数
    private float skillLiveTime;        //技能存在时间
    public bool destroyStatus;          //销毁状态

	// Use this for initialization
	void Start () {
        //Debug.Log("技能创建！");
        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID,  "Skill_Template");
        //获取受击特效播放特效
        //AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取技能
        SkillEffectDistance = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillMoveSpeed", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        //获取受击特效名称
        skillHitEffectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        
        //设置技能范围
        Game_PublicClassVar.Get_function_Skill.AddSkillRange(this.gameObject.GetComponent<SkillObjBase>().SkillID, this.gameObject);
        
        //实例化技能特效
        Game_PublicClassVar.Get_function_Skill.PlayActSkillEffect(this.gameObject.GetComponent<SkillObjBase>().SkillID,this.gameObject);

        //获取技能攻击目标
        SkillActObj = this.GetComponent<SkillObjBase>().SkillTargetObj;

        //获取技能是否有释放目标点
        //string ifSelectSkillRange = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfSelectSkillRange", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (SkillActObj != null)
        {
            //设置一定方向及位置
            Transform hitPosition = SkillActObj.GetComponent<AI_1>().BoneSet.transform.Find("Center").transform;        //假设命中怪物,后期如果怪物打人需要此处调整
            this.gameObject.transform.LookAt(hitPosition);         //修正技能施放方向
            this.gameObject.transform.localRotation = new Quaternion(0, this.gameObject.transform.localRotation.y, this.gameObject.transform.localRotation.z, this.gameObject.transform.localRotation.w);
        }

        //获取技能参数
        SkillPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //技能时间到注销此物体
        //Destroy(this.gameObject,SkillTime);

        //技能参数为1时,将始终跟随目标移动
        if (SkillPar == "1")
        {
            if (this.gameObject.GetComponent<SkillObjBase>().SkillTargetObj != null)
            {
                Vector3 vec3 = new Vector3(this.gameObject.GetComponent<SkillObjBase>().SkillTargetObj.transform.position.x, this.gameObject.GetComponent<SkillObjBase>().SkillTargetObj.transform.position.y + 1.0f, this.gameObject.GetComponent<SkillObjBase>().SkillTargetObj.transform.position.z);
                transform.LookAt(vec3);
            }
        }


        //添加翻滚冲击效果
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.AddComponent<RoseSkill_FanGun_1>();
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().FanGunSpeedValue = -20;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().SkillTime = 1;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().MoveStatus = true;

    }

    // Update is called once per frame
    void Update () {


		//物体移动
		transform.Translate(Vector3.forward*SkillEffectDistance*Time.deltaTime);


        //技能存在20秒会自动销毁
        skillLiveTime = skillLiveTime + Time.deltaTime;
        if (skillLiveTime >= 3.0f) {
            Destroy(this.gameObject);
        }


        if (destroyStatus) {
            if (skillLiveTime > 0.1f)
            {
                Destroy(this.gameObject);
                destroyStatus = false;
            }

        }

        //防止意外
        /*
        if (this.GetComponent<SphereCollider>() != null) {
            this.GetComponent<SphereCollider>().isTrigger = true;
        }
        */  
	}

	//第一次碰撞调用
	void OnTriggerEnter (Collider collider){

        Debug.Log("collider = " + collider.name);

        //碰撞体碰到目标后给予技能伤害,并销毁自己
		AI_Collider = collider.gameObject;
        //SkillActObj = this.GetComponent<SkillObjBase>().SkillTargetObj;
        //Debug.Log("AI_Collider = " + AI_Collider.name + "SkillActObj = " + SkillActObj);
        if (collider.gameObject.layer == 12)
        {
            Debug.Log("进来了1");
            AI_1 ai = collider.gameObject.GetComponent<AI_1>();
            AI_Property ai_property = collider.gameObject.GetComponent<AI_Property>();

            if (ai_property != null)
            {
                //Debug.Log("进来了2");
                //是否播放受击特效
                switch (AI_IfHitEffect)
                {
                    case "0":
                        collider.gameObject.GetComponent<AI_1>().IfHitEffect = false;   //不播放
                        break;
                    case "1":
                        collider.gameObject.GetComponent<AI_1>().IfHitEffect = true;    //播放
                        collider.gameObject.GetComponent<AI_1>().HitEffectt_Position = collider.gameObject.GetComponent<AI_1>().BoneSet.transform.Find(AI_HitPosition).gameObject;  //设置播放位置
                        if (skillHitEffectName != "" && skillHitEffectName != "0")
                        {
                            AI_Collider.GetComponent<Collider>().gameObject.GetComponent<AI_1>().HitEffect = (GameObject)Resources.Load("Effect/Skill/" + skillHitEffectName);
                        }
                        break;
                }
                //Debug.Log("进来了3");
                ai.HitStatus = true; //受击特效
                Game_PublicClassVar.Get_fight_Formult.RoseActMonster(this.gameObject.GetComponent<SkillObjBase>().SkillID, collider.gameObject, false);

                //触发技能Buff
                string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                if (buffID != "0" && buffID != "") {
                    Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
                }
            }

            //Debug.Log("进来了4");
            //碰撞到目标后注销自己
            //destroyStatus = true;         //屏蔽这里是移动的时候不会蹦到目标销毁自身
            //Debug.Log("进来了5");
        }
	}

	//碰撞范围内调用
	void OnTriggerStay(Collider collider){


        //Debug.Log("collider123 = " + collider.name);

    }


	//离开碰撞调用
	void OnTriggerExit (Collider collider){
	
	
	}


}
