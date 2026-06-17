using System;
using UnityEngine;

// 炽魂爆发 进入爆发状态
// GameObjectParameter：消耗x%生命值;每秒损失x%生命值;普攻恢复x%;普攻伤害增加x%
public class RoseSkill_BaoFa : MonoBehaviour
{
    private float SkillTime; //技能时间
    private float triggerTime;

    //绑点专用
    private Game_PositionVar game_PositionVar;
    private Rose_Proprety roseProprety;
    private Rose_Bone roseBone;

    private float value_1;
    private float value_2;
    private float value_3;
    
    void Start()
    {
        SkillTime = this.GetComponent<SkillObjBase>().SkillLiveTime;
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        roseProprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        roseBone = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>();
        
        string skillPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (skillPar != "0")
        {
            string[] skillParValue = skillPar.Split(';');
            value_1 = float.Parse(skillParValue[1]);
            value_2 = float.Parse(skillParValue[2]);
            value_3 = float.Parse(skillParValue[3]);
        }
        
        //触发技能Buff
        // string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        // if (buffID != "0" && buffID != "")
        // {
        //     Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, game_PositionVar.Obj_Rose);
        // }

        roseProprety.Rose_XiXuePro_Add += value_2;
        roseProprety.Rose_ActAddPro_Add += value_3;

        Game_PublicClassVar.Get_game_PositionVar.UpdataRoseBuffProperty = true;
        
        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        
        //实例化技能特效
        if (effectName != "0")
        {
            GameObject skillEffect = (GameObject)ResourcesManager.Instance.LoadEffectSync<GameObject>("Skill/" + effectName);
            GameObject effect = (GameObject)Instantiate(skillEffect, this.transform, true);
            effect.SetActive(false);
            effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            effect.SetActive(true);
        }

        Destroy(this.gameObject, SkillTime);
    }

    private void Update()
    {
        if (roseBone != null)
        {
            this.transform.position = roseBone.Bone_Center.transform.position;
        }
        
        triggerTime += Time.deltaTime;

        if (triggerTime >= 1f)
        {
            triggerTime = 0;
            
            int hpNow = roseProprety.Rose_HpNow;
            int hp = roseProprety.Rose_Hp;
            int value = (int)(hp * value_1);
            
            if (hpNow <= value)
            {
                // 生命值不足取消爆发状态
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Game_PublicClassVar.Get_function_Rose.costRoseHp(value);
            }
        }
    }

    private void OnDestroy()
    {
        Debug.LogWarning("爆发技能结束！！！！！");
        roseProprety.Rose_XiXuePro_Add -= value_2;
        roseProprety.Rose_ActAddPro_Add -= value_3;
        
        Game_PublicClassVar.Get_game_PositionVar.UpdataRoseBuffProperty = true;
    }
}