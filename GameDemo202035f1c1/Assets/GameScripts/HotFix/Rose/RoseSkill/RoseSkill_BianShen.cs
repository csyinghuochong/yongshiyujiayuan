using System;
using System.Collections.Generic;
using UnityEngine;


public class RoseSkill_BianShen : MonoBehaviour
{
    public static bool IsBianShen;
    //定义技能状态
    public float SkillTime; //技能时间（特效和效果共用）

    public float DamgeFrequency; //2次伤害时间的间隔
    private float DamgeFrequencySum; //2次伤害时间的间隔累加，用来累计时间
    private float SkillTimeSum; //技能时间累计值
    private GameObject AI_Collider; //碰撞的怪物
    private List<GameObject> colliderList = new List<GameObject>(); //碰撞到的怪物列表
    private string AI_HitPosition; //AI受到伤害特效显示的位置
    private string AI_IfHitEffect; //是否播放受击特效
    private GameObject effect;

    private string skillHitEffectName;

    // Use this for initialization
    void Start()
    {
        IsBianShen = true;
        
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_RoseModel.transform
            .localScale = new Vector3(2, 2, 2);

        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID",
            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID",
            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效名称
        skillHitEffectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID",
            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID",
            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //技能间隔时间
        string damgeFrequency = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID",
            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取技能附加
        SkillTime = this.GetComponent<SkillObjBase>().SkillLiveTime;

        //伤害间隔时间
        DamgeFrequency = float.Parse(damgeFrequency);

        //实例化技能特效
        if (effectName != "0")
        {
            GameObject skillEffect =
                (GameObject)ResourcesManager.Instance.LoadEffectSync<GameObject>("Skill/" + effectName);
            GameObject effect = (GameObject)Instantiate(skillEffect, this.transform, true);
            effect.SetActive(false);
            effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            effect.SetActive(true);
        }

        Destroy(this.gameObject, SkillTime);

    }

    // Update is called once per frame
    void Update()
    {
        SkillTimeSum = SkillTimeSum + Time.deltaTime;

        if (SkillTimeSum >= SkillTime - 0.2f)
        {
            IsBianShen = false;
            
            colliderList.Clear();
            SkillTimeSum = 0.0f;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_RoseModel.transform
                    .localScale =
                new Vector3(1, 1, 1);
        }

        DamgeFrequencySum = DamgeFrequencySum + Time.deltaTime;

        
    }

    private void OnDestroy()
    {
        //离开时销毁特效,主要针对燃烧特效
        Destroy(effect);
    }


    //第一次碰撞调用
    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.layer == 12)
        {
            if (collider.gameObject != null)
            {

                colliderList.Add(collider.gameObject);

                //触发技能Buff
                string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID",
                    this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                if (buffID != "0" && buffID != "")
                {
                    Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
                }


                Game_PublicClassVar.Get_fight_Formult.RoseActMonster(GetComponent<SkillObjBase>().SkillID, collider.gameObject, false);

                //技能附加值（附加额外Buff）
                Game_PublicClassVar.Get_function_Skill.SkillAddValue_Buff(this.GetComponent<SkillObjBase>().SkillID,
                    collider.gameObject);

            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (DamgeFrequencySum > DamgeFrequency)
        {
            foreach (GameObject nowObj in colliderList)
            {
                AI_Collider = nowObj;

                if (AI_Collider != this.gameObject && AI_Collider != null)
                {
                    AI_1 ai = AI_Collider.gameObject.GetComponent<AI_1>();
                    AI_Property ai_property = AI_Collider.gameObject.GetComponent<AI_Property>();

                    if (ai_property != null)
                    {
                        //是否播放受击特效
                        switch (AI_IfHitEffect)
                        {
                            case "0":
                                AI_Collider.gameObject.GetComponent<AI_1>().IfHitEffect = false; //不播放
                                AI_Collider.gameObject.GetComponent<AI_1>().HitStatus = true;
                                break;
                            case "1":
                                if (skillHitEffectName != "" && skillHitEffectName != "0")
                                {
                                    AI_Collider.GetComponent<Collider>().gameObject.GetComponent<AI_1>().HitEffect =
                                        (GameObject)ResourcesManager.Instance.LoadEffectSync<GameObject>("Skill/" +
                                            skillHitEffectName);
                                }

                                AI_Collider.gameObject.GetComponent<AI_1>().IfHitEffect = true; //播放
                                AI_Collider.gameObject.GetComponent<AI_1>().HitStatus = true;
                                AI_Collider.gameObject.GetComponent<AI_1>().HitEffectt_Position = AI_Collider.gameObject
                                    .GetComponent<AI_1>().BoneSet.transform.Find(AI_HitPosition).gameObject; //设置播放位置
                                break;
                        }

                        DamgeFrequencySum = 0.0f; //清空累计时间
                        ai.HitStatus = true; //受击特效
                        //发送攻击消息
                        Game_PublicClassVar.Get_fight_Formult.RoseActMonster(this.gameObject.GetComponent<SkillObjBase>().SkillID, nowObj.gameObject, false);

                    }
                }
            }
        }
    }


    //离开碰撞调用
    void OnTriggerExit(Collider collider)
    {
        colliderList.Remove(collider.gameObject);
    }
}

