using UnityEngine;
using System.Collections;
//加血技能
public class RoseSkill_ZhanSha_1 : MonoBehaviour
{
	private GameObject SkillEffect;     //技能受击特效
    public float SkillTime;             //技能特效播放时间
    private int Value_1;                //初始加血的值
    private int Value_2;                //持续的加血值
    private int Value_3;                //2次持续加血时间的间隔
    

    //绑点专用
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {

        //获取绑点
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        
        //对目标造成伤害
        //获取攻击目标
        GameObject actObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
        if (actObj != null) {
            actObj.GetComponent<AI_1>().HitStatus = true;      //播放受击特效

            //获取特效名称
            string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
            if (effectName != "0") {
                actObj.GetComponent<AI_1>().HitEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName);
                actObj.GetComponent<AI_1>().IfHitEffect = true;
            }
            
            //获取技能特效名称
            string acteffectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
            if (acteffectName != "" && acteffectName != "0") {
                //实例化技能特效
                GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + acteffectName, typeof(GameObject));
                GameObject effect = (GameObject)Instantiate(SkillEffect);
                effect.SetActive(false);
                string positionName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                //effect.transform.parent = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().BoneSet.transform.Find(positionName).transform;
                effect.transform.parent = actObj.transform;
                effect.transform.localPosition = Vector3.zero;
                effect.transform.localRotation = Quaternion.Euler(Vector3.zero);
                effect.SetActive(true);
            }

            //宠物触发重击
            Game_PublicClassVar.Get_fight_Formult.RoseActMonster(this.GetComponent<SkillObjBase>().SkillID, actObj, false);
            //触发斩杀
            if (actObj.GetComponent<AI_Property>().AI_Hp / actObj.GetComponent<AI_Property>().AI_HpMax <= 0.5f) {
                //Debug.Log("触发了斩杀");
                Game_PublicClassVar.Get_fight_Formult.RoseActMonster(this.GetComponent<SkillObjBase>().SkillID, actObj, false);
            }
        }

        //触发技能Buff
        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (buffID != "0" && buffID != "")
        {
            //Debug.Log("触发BUFF:" + buffID);
            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget);
        }


        //获取技能时间
        //Value_2 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));

        //实例化技能特效
        /*
        GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
        GameObject effect = (GameObject)Instantiate(SkillEffect);
        effect.SetActive(false);
        effect.transform.parent = this.transform;
        effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        effect.SetActive(true);
        */
        
        //播放施法动作
        //________Game_PublicClassVar.Get_game_PositionVar.Rose_IfSkill = true;



        //延迟时间注销
        Destroy(this.gameObject, SkillTime);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
