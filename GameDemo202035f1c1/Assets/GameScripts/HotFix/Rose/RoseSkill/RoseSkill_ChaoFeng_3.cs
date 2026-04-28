using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//冲击波技能 可以配置移动速度和受到的伤害
public class RoseSkill_ChaoFeng_3 : MonoBehaviour
{

	//定义技能状态
	//public bool SkillStatus;
	public float SkillEffectDistance;   //技能每秒移动距离
    public float SkillTime;             //技能时间（特效和效果共用）
    public Vector3 SkillTargetPoint;    //技能移动到的点，根据此点判定移动的方向

    public float DamgeFrequency;            //2次伤害时间的间隔
    private float DamgeFrequencySum;        //2次伤害时间的间隔累加，用来累计时间
    private float SkillTimeSum;             //技能时间累计值，用来在时间到时清空怪物列表
    private GameObject AI_Collider;         //碰撞的怪物
    private List<GameObject> colliderList   = new List<GameObject>();  //碰撞到的怪物列表
    private int cillider_Num;               //碰撞体的怪物数量
    private string AI_HitPosition;          //AI受到伤害特效显示的位置
    private string AI_IfHitEffect;          //是否播放受击特效
    


	// Use this for initialization
	void Start () {

        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        SkillTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        


        //实例化技能特效
        GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
        if (SkillEffect != null)
        {
            GameObject effect = (GameObject)Instantiate(SkillEffect);
            effect.SetActive(false);
            effect.transform.parent = this.transform;
            effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            effect.SetActive(true);
        }
        else
        {
            //Debug.Log("技能" + skillID + "缺少技能特效");
        }

        //设置技能位置
        SkillTargetPoint = this.gameObject.GetComponent<SkillObjBase>().SkillTargetPoint;
        this.gameObject.transform.localPosition = new Vector3(SkillTargetPoint.x, SkillTargetPoint.y, SkillTargetPoint.z);

        Destroy(this.gameObject, SkillTime);  //技能时间到注销此物体

        DamgeFrequencySum = DamgeFrequency;

	}
	
	// Update is called once per frame
	void Update () {

        //累计技能时间
        DamgeFrequencySum = DamgeFrequencySum + Time.deltaTime;
        SkillTimeSum = SkillTimeSum + Time.deltaTime;

        //技能结束清空怪物泛型
        if (SkillTimeSum >= SkillTime-0.2f) {
            colliderList.Clear();
            SkillTimeSum = 0.0f;
        }

	}


	//第一次碰撞调用
	void OnTriggerEnter (Collider collider){

        //将进入技能范围的怪物加入进一个集合中
        if (collider.name != "Rose")
        {
            if (collider.gameObject.layer == 12)
            {
                if (collider.gameObject != null)
                {
                    //当碰撞体不是自己时触发
                    if (collider.gameObject != null)
                    {
                        colliderList.Add(collider.gameObject);

                        //触发技能Buff
                        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                        if (buffID != "0" && buffID != "")
                        {
                            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
                        }

                        //释放嘲讽
                        collider.gameObject.GetComponent<AI_1>().HitStatus = true;
                    }
                }
            }
        }
    }


	//碰撞范围内调用
	void OnTriggerStay(Collider collider){

        if (DamgeFrequencySum >= DamgeFrequency)
        {
            DamgeFrequencySum = 0.0f;
            foreach (GameObject nowObj in colliderList) {

                AI_Collider = nowObj;
                //if (AI_Collider.transform.IsChildOf(Game_PublicClassVar.Get_game_PositionVar.Transform_AI_RoseNearMonster))
                //{

                    if (AI_Collider != this.gameObject)
                    {

                        AI_1 ai = AI_Collider.gameObject.GetComponent<AI_1>();
                        AI_Property ai_property = AI_Collider.gameObject.GetComponent<AI_Property>();

                        if (ai_property != null)
                        {

                            //是否播放受击特效
                            switch (AI_IfHitEffect)
                            {
                                case "0":
                                    AI_Collider.gameObject.GetComponent<AI_1>().IfHitEffect = false;   //不播放
                                    AI_Collider.gameObject.GetComponent<AI_1>().HitStatus = true;
                                    break;
                                case "1":
                                    AI_Collider.gameObject.GetComponent<AI_1>().IfHitEffect = true;    //播放
                                    AI_Collider.gameObject.GetComponent<AI_1>().HitStatus = true;
                                    AI_Collider.gameObject.GetComponent<AI_1>().HitEffectt_Position = AI_Collider.gameObject.GetComponent<AI_1>().BoneSet.transform.Find(AI_HitPosition).gameObject;  //设置播放位置
                                    break;
                            }

                            DamgeFrequencySum = 0.0f;       //清空累计时间
                            ai.HitStatus = true;            //受击特效
                            //发送攻击消息
                            Game_PublicClassVar.Get_fight_Formult.RoseActMonster(this.gameObject.GetComponent<SkillObjBase>().SkillID, nowObj.gameObject,false);

                        }

                    }

                //}

            }
        }
    }


	//离开碰撞调用
	void OnTriggerExit (Collider collider){

            colliderList.Remove(collider.gameObject);
            Debug.Log(collider.name+"离开的碰撞体");

	}


}
