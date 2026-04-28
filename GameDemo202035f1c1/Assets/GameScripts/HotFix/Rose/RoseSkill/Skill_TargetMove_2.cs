using UnityEngine;
using System.Collections;

//立即释放技能（没有弹道） 
public class Skill_TargetMove_2 : MonoBehaviour
{

	//定义技能状态
	public float SkillEffectDistance;   //技能每秒移动距离
    public float SkillTime;             //技能时间（特效和效果共用）
    public int DamgeValue_Fixed;        //技能伤害
    public Vector3 SkillTargetPoint;    //技能移动到的点，根据此点判定移动的方向

    private GameObject AI_Collider;     //碰撞的怪物
    private string AI_HitPosition;      //AI受到伤害特效显示的位置
    private string AI_IfHitEffect;      //是否播放受击特效
    private GameObject SkillActObj;     //技能攻击目标
    private string skillHitEffectName;      //技能受击特效

	// Use this for initialization
	void Start () {
        
        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID,  "Skill_Template");
        //获取受击特效名称
        skillHitEffectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //获取技能
        SkillEffectDistance = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillMoveSpeed", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        
        //设置技能范围
        Game_PublicClassVar.Get_function_Skill.AddSkillRange(this.gameObject.GetComponent<SkillObjBase>().SkillID, this.gameObject);
        
        //实例化技能特效
        Game_PublicClassVar.Get_function_Skill.PlayActSkillEffect(this.gameObject.GetComponent<SkillObjBase>().SkillID,this.gameObject);

        //获取技能攻击目标
        SkillActObj = this.GetComponent<SkillObjBase>().SkillTargetObj;
        //触发技能
        StarSkill();
        
	}
	
	// Update is called once per frame
	void Update () {

	}


    public void StarSkill() {

        //碰撞体碰到目标后给予技能伤害,并销毁自己
        AI_Collider = SkillActObj;
        if (AI_Collider == SkillActObj)
        {
            AI_1 ai = AI_Collider.GetComponent<Collider>().gameObject.GetComponent<AI_1>();
            AI_Property ai_property = AI_Collider.GetComponent<Collider>().gameObject.GetComponent<AI_Property>();

            if (ai_property != null)
            {
                //是否播放受击特效
                switch (AI_IfHitEffect)
                {
                    case "0":
                        AI_Collider.GetComponent<Collider>().gameObject.GetComponent<AI_1>().IfHitEffect = false;   //不播放
                        break;
                    case "1":

                        AI_Collider.GetComponent<Collider>().gameObject.GetComponent<AI_1>().IfHitEffect = true;    //播放
                        AI_Collider.GetComponent<Collider>().gameObject.GetComponent<AI_1>().HitEffectt_Position = AI_Collider.GetComponent<Collider>().gameObject.GetComponent<AI_1>().BoneSet.transform.Find(AI_HitPosition).gameObject;  //设置播放位置
                        if (skillHitEffectName != "" && skillHitEffectName != "0") {
                            AI_Collider.GetComponent<Collider>().gameObject.GetComponent<AI_1>().HitEffect = (GameObject)Resources.Load("Effect/Skill/" + skillHitEffectName);
                        }
                        break;
                }

                ai.HitStatus = true; //受击特效
                Game_PublicClassVar.Get_fight_Formult.RoseActMonster(this.gameObject.GetComponent<SkillObjBase>().SkillID, AI_Collider.GetComponent<Collider>().gameObject, false);

                //触发技能Buff
                string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                if (buffID != "0" && buffID != "")
                {
                    Debug.Log("触发BUFF:" + buffID);
                    Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, AI_Collider.GetComponent<Collider>().gameObject);
                }
            }

            //碰撞到目标后注销自己
            Destroy(this.gameObject);

        }

    }








	//第一次碰撞调用
	void OnTriggerEnter (Collider collider){

	}

	//碰撞范围内调用
	void OnTriggerStay(Collider collider){

        

	}


	//离开碰撞调用
	void OnTriggerExit (Collider collider){
	
	
	}


}
