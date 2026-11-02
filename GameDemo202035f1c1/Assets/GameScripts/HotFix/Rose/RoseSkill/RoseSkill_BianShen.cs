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
    
    // GameObjectParameter：伤害间隔;体型增幅;变化时间
    // 增幅1表示原体型的2倍；旧配置只有伤害间隔时，默认增幅1、变化0.5秒。
    private float sizeValue = 1f;
    private float sizeTime = 0.5f;
    private Transform changeObj;
    private Vector3 originalScale;
    private Vector3 enlargedScale;
    private static RoseSkill_BianShen activeTransformation;
    
    private void BeginModelSize()
    {
        changeObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose
            .GetComponent<Rose_Status>().Obj_RoseModel.transform;

        // 重复变身替换前一个实例，避免倍率叠乘以及旧实例提前恢复新变身。
        if (activeTransformation != null && activeTransformation != this)
        {
            RoseSkill_BianShen previous = activeTransformation;
            previous.EndModelSize();
            previous.enabled = false;
            Destroy(previous.gameObject);
        }

        originalScale = changeObj.localScale;
        enlargedScale = originalScale * (1f + sizeValue);
        activeTransformation = this;
        IsBianShen = true;
        changeObj.localScale = sizeTime <= 0 ? enlargedScale : originalScale;
    }

    private void UpdateModelSize()
    {
        if (activeTransformation != this || changeObj == null) return;

        // 短时技能限制过渡长度，避免放大和缩小时段重叠。
        float transitionTime = Mathf.Min(sizeTime, SkillTime * 0.5f);
        if (transitionTime <= 0)
        {
            changeObj.localScale = enlargedScale;
            return;
        }

        float remainingTime = SkillTime - SkillTimeSum;
        if (remainingTime < transitionTime)
        {
            float t = Mathf.Clamp01(remainingTime / transitionTime);
            changeObj.localScale = Vector3.Lerp(originalScale, enlargedScale, t);
        }
        else
        {
            float t = Mathf.Clamp01(SkillTimeSum / transitionTime);
            changeObj.localScale = Vector3.Lerp(originalScale, enlargedScale, t);
        }
    }

    private void EndModelSize()
    {
        if (activeTransformation != this) return;
        if (changeObj != null) changeObj.localScale = originalScale;
        activeTransformation = null;
        IsBianShen = false;
    }

    private void OnDisable()
    {
        EndModelSize();
    }

    // Use this for initialization
    void Start()
    {
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
        
        
        string[] parameters = damgeFrequency.Split(';');
        DamgeFrequency = Mathf.Max(0f, float.Parse(parameters[0]));
        if (parameters.Length > 1) sizeValue = Mathf.Max(0f, float.Parse(parameters[1]));
        if (parameters.Length > 2) sizeTime = Mathf.Max(0f, float.Parse(parameters[2]));
        
        BeginModelSize();

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

        UpdateModelSize();

        DamgeFrequencySum = DamgeFrequencySum + Time.deltaTime;
    }

    private void OnDestroy()
    {

        EndModelSize();
        colliderList.Clear();
        // 使用成员变量保存特效引用，避免局部同名变量遮蔽。
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

