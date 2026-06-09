using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//嘲讽技能
public class RoseSkill_LaJin : MonoBehaviour
{
    //定义技能状态
    public float SkillTime; //技能时间（特效和效果共用）

    private GameObject AI_Collider; //碰撞的怪物
    private string AI_HitPosition; //AI受到伤害特效显示的位置
    private string AI_IfHitEffect; //是否播放受击特效
    private string AI_HitEffect; //AI受击特效

    private List<GameObject> collockList = new List<GameObject>();
    private float collockStatusTime;

    // Use this for initialization
    void Start()
    {
        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID",
            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID",
            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //设置技能范围
        Game_PublicClassVar.Get_function_Skill.AddSkillRange(this.gameObject.GetComponent<SkillObjBase>().SkillID,
            this.gameObject);

        //实例化技能特效
        Game_PublicClassVar.Get_function_Skill.PlayActSkillEffect(this.gameObject.GetComponent<SkillObjBase>().SkillID,
            this.gameObject);

        //技能时间到注销此物体
        Destroy(this.gameObject, SkillTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //第一次碰撞调用
    void OnTriggerEnter(Collider collider)
    {
        
        AI_Collider = collider.gameObject;


        if (collockList.Contains(AI_Collider))
        {
            return;
        }


        if (AI_Collider != this.gameObject && AI_Collider.layer == 12)
        {
            //Debug.Log("碰撞体：" + collider.name);
            AI_1 ai = collider.gameObject.GetComponent<AI_1>();
            AI_Property ai_property = collider.gameObject.GetComponent<AI_Property>();

            //聚怪
            collider.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;

            if (ai_property != null)
            {
                //是否播放受击特效
                switch (AI_IfHitEffect)
                {
                    case "0":
                        Debug.Log("collider = " + collider.name);
                        collider.gameObject.GetComponent<AI_1>().IfHitEffect = false; //不播放
                        ai.HitStatus = true; //受击特效
                        break;
                    case "1":
                        //获取受击特效播放特效
                        AI_HitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID",
                            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                        if (AI_HitEffect != "" && AI_HitEffect != "0")
                        {
                            if (collider.gameObject != null && collider.gameObject.GetComponent<AI_1>() != null)
                            {
                                collider.gameObject.GetComponent<AI_1>().IfHitEffect = true; //播放
                                //实例化技能特效
                                GameObject SkillEffect =
                                    (GameObject)ResourcesManager.Instance.LoadEffectSync<GameObject>("Skill/" +
                                        AI_HitEffect);
                                GameObject effect = (GameObject)MonoBehaviour.Instantiate(SkillEffect);
                                collider.gameObject.GetComponent<AI_1>().HitEffect = effect;
                                collider.gameObject.GetComponent<AI_1>().HitEffectt_Position = collider.gameObject
                                    .GetComponent<AI_1>().BoneSet.transform.Find(AI_HitPosition).gameObject; //设置播放位置
                            }
                        }

                        break;
                }

                Game_PublicClassVar.Get_fight_Formult.RoseActMonster(
                    this.gameObject.GetComponent<SkillObjBase>().SkillID, collider.gameObject, false);

                //触发技能Buff
                string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID",
                    this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                if (buffID != "0" && buffID != "")
                {
                    Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
                }
            }
        }


        collockList.Add(AI_Collider);
    }
}
