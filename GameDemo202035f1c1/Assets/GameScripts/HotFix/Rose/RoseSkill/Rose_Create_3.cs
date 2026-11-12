using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SkillObjBase))]
public class Rose_Create_3 : MonoBehaviour
{
    // 上限|召唤物ID,数量,攻击倍率,生命倍率,防御倍率;下一组召唤物
    private readonly struct SummonEntry
    {
        public readonly long Id;
        public readonly int Count;
        public readonly float Attack;
        public readonly float Health;
        public readonly float Defense;

        public SummonEntry(long id, int count, float attack, float health, float defense)
        {
            Id = id;
            Count = count;
            Attack = attack;
            Health = health;
            Defense = defense;
        }
    }

    private IEnumerator Start()
    {
        SkillObjBase skill = GetComponent<SkillObjBase>();
        string skillId = skill.SkillID;
        GameObject owner = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;
        if (owner == null || !owner.TryGetComponent(out Rose_Proprety ownerStats))
        {
            Destroy(gameObject);
            yield break;
        }

        PlayEffect(ReadSkill("EffectName", skillId), owner.transform.position);
        if (float.TryParse(ReadSkill("SkillDelayTime", skillId), NumberStyles.Float,
                CultureInfo.InvariantCulture, out float delay) && delay > 0f)
            yield return new WaitForSeconds(delay);
        else
            yield return null;

        GameObject summonRoot = Game_PublicClassVar.Get_game_PositionVar.Obj_RoseCreatePetSet;
        if (owner != null && ownerStats != null && summonRoot != null)
            SpawnConfigured(owner, ownerStats, summonRoot.transform, skillId, skill.SkillLiveTime);

        Destroy(gameObject);
    }

    private void SpawnConfigured(GameObject owner, Rose_Proprety ownerStats, Transform summonRoot,
        string skillId, float lifetime)
    {
        string parameter = ReadSkill("GameObjectParameter", skillId);
        string[] sections = (parameter ?? string.Empty).Split('|');
        if (sections.Length != 2 || !int.TryParse(sections[0], out int limit) || limit < 1)
        {
            Debug.LogWarning($"召唤技能 {skillId} 的数量上限配置无效：{parameter}");
            return;
        }

        string[] groups = sections[1].Split(';');
        var entries = new SummonEntry[groups.Length];
        for (int i = 0; i < groups.Length; i++)
        {
            string[] fields = groups[i].Split(',');
            if (fields.Length != 5 || !long.TryParse(fields[0], out long id) || id < 1 ||
                !int.TryParse(fields[1], out int count) || count < 1 ||
                !TryReadRatio(fields[2], out float attack) ||
                !TryReadRatio(fields[3], out float health) || health <= 0f ||
                !TryReadRatio(fields[4], out float defense))
            {
                Debug.LogWarning($"召唤技能 {skillId} 的召唤组配置无效：{groups[i]}");
                return;
            }
            entries[i] = new SummonEntry(id, count, attack, health, defense);
        }

        foreach (SummonEntry entry in entries)
        {
            int remaining = limit - CountLiving(summonRoot, entry.Id);
            if (remaining <= 0)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front($"最多召唤{limit}只！");
                continue;
            }

            GameObject prefab = ResourcesManager.Instance.LoadUnitSync<GameObject>("PetSet/" + entry.Id);
            if (prefab == null || !prefab.TryGetComponent(out NavMeshAgent agent) ||
                !prefab.TryGetComponent(out AIPet template) || template.AI_ID != entry.Id ||
                !prefab.TryGetComponent<AI_Property>(out _))
            {
                Debug.LogWarning($"召唤技能 {skillId}：资源 {entry.Id} 的 ID 或必要组件有误。");
                continue;
            }

            int countToSpawn = Mathf.Min(entry.Count, remaining);
            for (int i = 0; i < countToSpawn; i++)
                SpawnOne(owner, ownerStats, summonRoot, skillId, lifetime, entry, prefab, agent);
        }
    }

    private static bool TryReadRatio(string text, out float ratio)
    {
        return float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out ratio) &&
            ratio >= 0f && !float.IsInfinity(ratio);
    }

    private static int CountLiving(Transform summonRoot, long id)
    {
        int livingCount = 0;
        foreach (Transform child in summonRoot)
        {
            if (child.TryGetComponent(out AIPet existingPet) &&
                existingPet.AI_ID == id &&
                child.TryGetComponent(out AI_Property existingStats) && existingStats.AI_Hp > 0)
                livingCount++;
        }

        return livingCount;
    }

    private void SpawnOne(GameObject owner, Rose_Proprety ownerStats, Transform summonRoot,
        string skillId, float lifetime, SummonEntry entry, GameObject prefab, NavMeshAgent agent)
    {
        Vector3 ownerPosition = owner.transform.position;
        Vector3 spawnPosition = ownerPosition + new Vector3(Random.value, 0f, Random.value);
        if (ReadSkill("SkillType", skillId) == "2" && owner.TryGetComponent(out Rose_Status ownerStatus) &&
            ownerStatus.Obj_ActTarget != null)
            spawnPosition = ownerStatus.Obj_ActTarget.transform.position;

        var filter = new NavMeshQueryFilter { agentTypeID = agent.agentTypeID, areaMask = agent.areaMask };
        if (!NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 2f, filter) &&
            !NavMesh.SamplePosition(ownerPosition, out hit, 2f, filter))
        {
            Debug.LogWarning($"召唤技能 {skillId}：附近没有可寻路落点。");
            return;
        }

        GameObject summon = Instantiate(prefab, hit.position, prefab.transform.rotation, summonRoot);
        AIPet pet = summon.GetComponent<AIPet>();
        pet.PetType = "1";
        pet.AILiveTime = lifetime;
        pet.PositionObj = owner;
        pet.IfSkillCreatePet_ProTeShu = true;

        AI_Property stats = summon.GetComponent<AI_Property>();
        // 关闭旧的攻击和防御继承。沿用属性加成字段，让 Buff 重算时仍保留继承值。
        stats.AI_SummonPropertyPro = 0f;
        stats.AI_SummonPropertyActPro = 0f;
        stats.AI_SummonPropertyDefPro = 0f;
        stats.AI_SummonPropertyHpPro = entry.Health;
        stats.ActAdd += (float)ownerStats.Rose_ActMax * entry.Attack;
        stats.MageActAdd += (float)ownerStats.Rose_MagActMax * entry.Attack;
        stats.DefAdd += (float)ownerStats.Rose_DefMax * entry.Defense;
        stats.AdfAdd += (float)ownerStats.Rose_AdfMax * entry.Defense;

        // 同帧再次触发召唤时，新仆从也应计入上限。
        int inheritedHealth = Mathf.Max(1, (int)((float)ownerStats.Rose_Hp * entry.Health));
        stats.AI_HpMax = inheritedHealth;
        stats.AI_Hp = inheritedHealth;
        summon.SetActive(true);
        Game_PublicClassVar.Get_function_Rose.addRosePetPositionObj(summon);

        string effectName = ReadSkill("HitEffectName", skillId);
        if (string.IsNullOrEmpty(effectName) || effectName == "0")
            effectName = "Eff_Skill_ZhaoHuan_3";
        PlayEffect(effectName, summon.transform.position, summon.transform);
    }

    private static string ReadSkill(string column, string skillId)
    {
        return Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(column, "ID", skillId, "Skill_Template");
    }

    private void PlayEffect(string effectName, Vector3 position, Transform parent = null)
    {
        if (string.IsNullOrEmpty(effectName) || effectName == "0")
            return;

        GameObject prefab = ResourcesManager.Instance.LoadEffectSync<GameObject>("Skill/" + effectName);
        if (prefab == null)
            return;

        GameObject effect = Instantiate(prefab, position, prefab.transform.rotation, parent);
        effect.transform.localScale = Vector3.one;
        if (parent != null)
            Destroy(effect, 2f);
    }
}
