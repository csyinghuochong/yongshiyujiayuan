using UnityEngine;
using System.Collections;
//加血技能
public class MonsterSkill_ZhongJi_1 : MonoBehaviour
{
	private GameObject SkillEffect;     //技能受击特效
    public float SkillTime;             //技能特效播放时间
    private int Value_1;                //初始加血的值
    private int Value_2;                //持续的加血值
    private int Value_3;                //2次持续加血时间的间隔
    private float delayTime;
    private float delayTimeSum;         //延迟时间累计
    //绑点专用
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {
        //Debug.Log("开始重击");
        //获取绑点
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        SkillTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        delayTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillDelayTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        //Debug.Log("重击延迟 = " + delayTime);
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

        //延迟召唤
        //Debug.Log("delayTimeSum = " + delayTimeSum);
        delayTimeSum = delayTimeSum + Time.deltaTime;
        if (delayTimeSum > delayTime)
        {
            createMonster();
            delayTime = 99999;      //只执行一次,防止执行多次
        }

	}

    void createMonster() {

        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        //对目标造成伤害
        //获取攻击目标
        //GameObject actObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
        GameObject actObj = this.gameObject.transform.parent.parent.parent.GetComponent<AI_1>().AI_Target;

        if (actObj != null)
        {
            //actObj.GetComponent<AI_1>().HitStatus = true;      //播放受击特效
            //获取特效名称
            string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
            if (effectName != "0")
            {
                //播放受击特效
                Debug.Log("播放受击特效");
                //actObj.GetComponent<Rose_Status>().HitEffectObj = (GameObject)Resources.Load("Effect/Skill/" + effectName);
                //actObj.GetComponent<Rose_Status>().RoseIfHit = true;
                GameObject hitEffectObj = (GameObject)Instantiate((GameObject)Resources.Load("Effect/Skill/" + effectName));

                //GameObject hitEffectObj = (GameObject)Instantiate(hitEffectObj);
                hitEffectObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Low.transform);
                hitEffectObj.transform.localPosition = new Vector3(0, 0.0f, 0);
                hitEffectObj.transform.localScale = new Vector3(1, 1, 1);
                
                
            }

            //获取技能特效名称
            string acteffectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
            //实例化技能特效
            if (acteffectName != "" && acteffectName != "0")
            {
                GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + acteffectName, typeof(GameObject));
                GameObject effect = (GameObject)Instantiate(SkillEffect);
                effect.SetActive(false);
                //effect.transform.parent = this.transform;
                //effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

                string positionName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                effect.transform.parent = this.gameObject.transform.parent.parent.GetComponent<AI_1>().BoneSet.transform.Find(positionName).transform;
                effect.transform.localPosition = Vector3.zero;
                effect.transform.localRotation = Quaternion.Euler(Vector3.zero);
                effect.SetActive(true);
            }

            //Debug.Log("冰裂斩"+effect.name);

            //Debug.Log("执行一次重击");
            Game_PublicClassVar.Get_fight_Formult.MonsterActRose(this.GetComponent<SkillObjBase>().SkillID, this.gameObject.transform.parent.parent.parent.gameObject, actObj);
            
        }

        //触发技能Buff
        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (buffID != "0" && buffID != "")
        {
            //Debug.Log("触发BUFF:" + buffID);
            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, game_PositionVar.Obj_Rose);
        }
    }
}
