using UnityEngine;
using System.Collections.Generic;

public class PetSkill_JiaXue : MonoBehaviour
{
    private void Start()
    {
        SkillObjBase skill = GetComponent<SkillObjBase>();
        var data = Game_PublicClassVar.Get_function_DataSet;

        string skillType = data.DataSet_ReadData(
            "SkillType", "ID", skill.SkillID, "Skill_Template");

        string damageType = data.DataSet_ReadData(
            "DamgeType", "ID", skill.SkillID, "Skill_Template");

        // 其他原有治疗技能，继续交给原脚本处理。
        if (skillType != "4" || damageType != "0")
        {
            RoseSkill_Jiaxue_3 original =
                GetComponent<RoseSkill_Jiaxue_3>();

            if (original != null)
                original.enabled = true;

            enabled = false;
            return;
        }

        GameObject caster = skill.MonsterSkillObj;
        if (caster == null || caster.GetComponent<AIPet>() == null)
        {
            Destroy(gameObject);
            return;
        }

        int fixedHeal = int.Parse(data.DataSet_ReadData(
            "DamgeValue", "ID", skill.SkillID, "Skill_Template"));

        float healRatio = float.Parse(data.DataSet_ReadData(
            "ActDamge", "ID", skill.SkillID, "Skill_Template"));

        float radius = float.Parse(data.DataSet_ReadData(
            "DamgeRange", "ID", skill.SkillID, "Skill_Template"));

        var position = Game_PublicClassVar.Get_game_PositionVar;
        GameObject roseObj = position.Obj_Rose;

        if (roseObj == null)
        {
            Destroy(gameObject);
            return;
        }

        // 范围为0：只治疗主人；大于0：治疗范围内己方。
        bool ownerOnly = radius <= 0;
        float radiusSqr = radius * radius;

        if (ownerOnly ||
            (roseObj.transform.position - caster.transform.position)
            .sqrMagnitude <= radiusSqr)
        {
            Rose_Proprety rose = roseObj.GetComponent<Rose_Proprety>();

            if (rose != null && rose.Rose_HpNow > 0)
            {
                int heal = fixedHeal +
                    Mathf.FloorToInt((float)rose.Rose_Hp * healRatio);

                Game_PublicClassVar.Get_function_Rose.addRoseHp(
                    heal, "1", true, false);
            }
        }

        if (!ownerOnly)
        {
            var healed = new HashSet<GameObject>();

            GameObject[] groups =
            {
                position.Obj_RosePetSet,
                position.Obj_RoseCreatePetSet
            };

            foreach (GameObject group in groups)
            {
                if (group == null)
                    continue;

                foreach (AIPet pet in
                    group.GetComponentsInChildren<AIPet>())
                {
                    if (!healed.Add(pet.gameObject))
                        continue;

                    if ((pet.transform.position -
                         caster.transform.position).sqrMagnitude
                        > radiusSqr)
                        continue;

                    AI_Property property =
                        pet.GetComponent<AI_Property>();

                    if (property == null ||
                        property.AI_Hp <= 0 ||
                        property.AI_HpMax <= 0)
                        continue;

                    Game_PublicClassVar.Get_function_AI.AI_addHp(
                        pet.gameObject, fixedHeal, healRatio);
                }
            }
        }

        Destroy(gameObject);
    }
}