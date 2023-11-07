using UnityEngine;
using System.Collections;
//嘲讽技能
public class RoseSkill_ChaoFeng_1 : MonoBehaviour
{

	private GameObject SkillEffect;     //技能特效
	//private GameObject skillEffect;   //脚本中实例化的特效ID
    public float SkillTime;             //技能特效播放时间
    private string skillHitEffectName;      //技能受击特效
    private string AI_HitPosition;          //技能受击特效播放点
    private string AI_IfHitEffect;          //播放技能特效
    //private int startAddHpValue;        //初始加血的值
    //private int continuedAddHpValue;    //持续的加血值
    //private int continuedTime;          //2次持续加血时间的间隔
    

    //绑点专用
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {

        //获取绑点
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        //是否播放特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效名称
        skillHitEffectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        GameObject actObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
        if (actObj != null) {

            //获取特效名称
            string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
            //实例化技能特效
            if (effectName != "" && effectName != "0" && effectName != null)
            {
                GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
                GameObject effect = (GameObject)Instantiate(SkillEffect);
                effect.SetActive(false);
                effect.transform.parent = actObj.transform;
                effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                effect.SetActive(true);
            }
            actObj.GetComponent<AI_1>().HitStatus = true;
            //触发BUFF
            //触发技能Buff
            string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
            if (buffID != "0" && buffID != "")
            {
                Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, actObj.gameObject);
            }

            //技能附加值（附加额外Buff）
            Game_PublicClassVar.Get_function_Skill.SkillAddValue_Buff(this.GetComponent<SkillObjBase>().SkillID,actObj.gameObject);

            //造成伤害
            Game_PublicClassVar.Get_fight_Formult.RoseActMonster(this.gameObject.GetComponent<SkillObjBase>().SkillID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget, false);

            //播放受击特效
            if (AI_IfHitEffect == "1") {
                //Debug.Log("嘲讽播放受击特效skillHitEffectName = " + skillHitEffectName);
                GameObject collider = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
                collider.gameObject.GetComponent<AI_1>().IfHitEffect = true;    //播放
                collider.gameObject.GetComponent<AI_1>().HitEffectt_Position = collider.gameObject.GetComponent<AI_1>().BoneSet.transform.Find(AI_HitPosition).gameObject;  //设置播放位置
                if (skillHitEffectName != "" && skillHitEffectName != "0")
                {
                    //Debug.Log("嘲讽播放受击特效222");
                    collider.GetComponent<AI_1>().HitEffect = (GameObject)Resources.Load("Effect/Skill/" + skillHitEffectName);
                }
            }
        }
        SkillTime = this.GetComponent<SkillObjBase>().SkillLiveTime;
        //延迟时间注销
        Destroy(this.gameObject, SkillTime);

	}
	
	// Update is called once per frame
	void Update () {

	}
}
