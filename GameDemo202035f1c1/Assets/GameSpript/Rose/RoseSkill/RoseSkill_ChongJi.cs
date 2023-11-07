using UnityEngine;
using System.Collections;

//冲击波技能 可以配置移动速度和受到的伤害
public class RoseSkill_ChongJi : MonoBehaviour
{
	//定义技能状态
	public float SkillEffectDistance;   //技能每秒移动距离
    public float SkillTime;             //技能时间（特效和效果共用）
    public int DamgeValue_Fixed;        //技能伤害
    public Vector3 SkillTargetPoint;    //技能移动到的点，根据此点判定移动的方向

    private GameObject AI_Collider;     //碰撞的怪物
    private string AI_HitPosition;      //AI受到伤害特效显示的位置
    private string AI_IfHitEffect;      //是否播放受击特效
    private string AI_HitEffect;        //AI受击特效


	// Use this for initialization
	void Start () {
        
        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID,  "Skill_Template");

        //获取技能
        SkillEffectDistance = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillMoveSpeed", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        
        //设置技能范围
        Game_PublicClassVar.Get_function_Skill.AddSkillRange(this.gameObject.GetComponent<SkillObjBase>().SkillID, this.gameObject);
        
        //实例化技能特效
        Game_PublicClassVar.Get_function_Skill.PlayActSkillEffect(this.gameObject.GetComponent<SkillObjBase>().SkillID,this.gameObject);

        //获取技能是否有释放目标点
        string ifSelectSkillRange = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfSelectSkillRange", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (ifSelectSkillRange != "0") {
            //设置一定方向及位置
            SkillTargetPoint = this.gameObject.GetComponent<SkillObjBase>().SkillTargetPoint;
            this.gameObject.transform.LookAt(SkillTargetPoint);         //修正技能施放方向
            this.gameObject.transform.localRotation = new Quaternion(0, this.gameObject.transform.localRotation.y, this.gameObject.transform.localRotation.z, this.gameObject.transform.localRotation.w);
            //Debug.Log("进入到移动，销毁时间：" + SkillTime);
        }

        //触发技能Buff
        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (buffID != "0" && buffID != "")
        {
            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose);
        }

        //添加翻滚冲击效果
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.AddComponent<RoseSkill_FanGun_1>();
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().FanGunSpeedValue = 20;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().SkillTime = 1;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().MoveStatus = true;

        //技能时间到注销此物体
        Destroy(this.gameObject,SkillTime);
	}
	
	// Update is called once per frame
	void Update () {

			//物体移动
        //SkillEffectDistance = 10.0f;
			transform.Translate(Vector3.forward*SkillEffectDistance*Time.deltaTime);
	}

    private void OnDestroy()
    {
    }

    //第一次碰撞调用
    void OnTriggerEnter (Collider collider){
        
		AI_Collider = collider.gameObject;

        //获取碰撞
        //___gameObject MonsterSet = this.gameObject;
        //___if (AI_Collider.transform.IsChildOf(MonsterSet))
        //___{

            if (AI_Collider != this.gameObject)
            {
                //Debug.Log("碰撞体：" + collider.name);
                AI_1 ai = collider.gameObject.GetComponent<AI_1>();
                AI_Property ai_property = collider.gameObject.GetComponent<AI_Property>();

                if (ai_property != null)
                {
                    //是否播放受击特效
                    switch (AI_IfHitEffect)
                    {
                        case "0":
                            collider.gameObject.GetComponent<AI_1>().IfHitEffect = false;   //不播放
                            ai.HitStatus = true; //受击特效
                            break;
                        case "1":
                            //获取受击特效播放特效
                            AI_HitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                            if (AI_HitEffect != "" && AI_HitEffect!="0")
                            {
                                collider.gameObject.GetComponent<AI_1>().IfHitEffect = true;    //播放
                                //实例化技能特效
                                GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + AI_HitEffect, typeof(GameObject));
                                GameObject effect = (GameObject)MonoBehaviour.Instantiate(SkillEffect);
                                collider.gameObject.GetComponent<AI_1>().HitEffect = effect;
                                collider.gameObject.GetComponent<AI_1>().HitEffectt_Position = collider.gameObject.GetComponent<AI_1>().BoneSet.transform.Find(AI_HitPosition).gameObject;  //设置播放位置
                            }
                            break;
                    }

                    
                    Game_PublicClassVar.Get_fight_Formult.RoseActMonster(this.gameObject.GetComponent<SkillObjBase>().SkillID, collider.gameObject, false);

                    //触发技能Buff
                    string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                    if (buffID != "0" && buffID != "") {
                        Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
                    }
                }
            }
        //___}
		
	}

	//碰撞范围内调用
	void OnTriggerStay(Collider collider){

        

	}


	//离开碰撞调用
	void OnTriggerExit (Collider collider){
	
	
	}


}
