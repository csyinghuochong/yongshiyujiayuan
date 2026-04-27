using UnityEngine;
using System.Collections;
//加血技能
public class PetSkill_LianJi : MonoBehaviour
{
	private GameObject SkillEffect;     //技能受击特效
    public float SkillTime;             //技能特效播放时间
    private int Value_1;                //初始加血的值
    private int Value_2;                //持续的加血值
    private int Value_3;                //2次持续加血时间的间隔

    public float skillTimeSum;
    private bool chufaStatus;

    //绑点专用
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {
        //Debug.Log("触发宠物连击！！！！！！！！！！");
        //获取绑点
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        //Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        
        //对目标造成伤害
        //获取攻击目标
        AIPet aiPet = this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AIPet>();
        //aiPet.actSpped_Sum = aiPet.gameObject.GetComponent<AI_Property>().AI_ActSpeed-0.5f;

        //aiPet.gameObject.GetComponent<AI_Status>().AI_Animator.Play("Attack_1");

        //aiPet.gameObject.GetComponent<AIPet>().AIAct();
        //Game_PublicClassVar.Get_fight_Formult.PetActMonster(aiPet.gameObject, "62000001", aiPet.gameObject.GetComponent<AIPet>().AI_Target, false);         //战斗

        //触发技能Buff
        /*
        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (buffID != "0" && buffID != "")
        {
            Debug.Log("触发BUFF:" + buffID);
            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget);
        }
        */

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


        if (SkillTime <= 1) {
            SkillTime = 1;
        }
        //延迟时间注销
        Destroy(this.gameObject, SkillTime);
	}
	
	// Update is called once per frame
	void Update () {

        if (chufaStatus==false) {
            skillTimeSum = skillTimeSum + Time.deltaTime;
            if (skillTimeSum >= 0.25f)
            {
                skillTimeSum = 0;
                chufaStatus = true;
                if (this.GetComponent<SkillObjBase>() != null)
                {
                    if (this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AIPet>() != null)
                    {
                        AIPet aiPet = this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AIPet>();
                        if (aiPet != null)
                        {
                            Game_PublicClassVar.Get_fight_Formult.PetActMonster(aiPet.gameObject, "62000001", aiPet.gameObject.GetComponent<AIPet>().AI_Target, false);         //战斗
                        }
                    }
                }
            }
        }

    }
}
