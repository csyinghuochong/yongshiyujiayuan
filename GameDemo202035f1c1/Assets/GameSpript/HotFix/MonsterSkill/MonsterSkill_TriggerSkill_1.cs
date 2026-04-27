using UnityEngine;
using System.Collections;
//技能开始多久间隔触发一个指定的技能
public class MonsterSkill_TriggerSkill_1 : MonoBehaviour
{

	private GameObject SkillEffect;     //技能特效
	//private GameObject skillEffect;   //脚本中实例化的特效ID
    public float SkillTime;             //技能特效播放时间
    public float TriggerTime;           //触发技能间隔
    private float TriggerTimeSum;
    public string TriggerSkillID;        //触发技能ID
    /*
    private string ParameterValue;      //参数值
    private float sizeValue;            //变化值
    private float now_ChangeSize;       //当前改变变化值
    private float sizeTime;             //变化时间
    private float sizeTimeSum;
    private Vector3 changeVec3;
    private GameObject changeObj;       //改变Obj
    private string parameterList;
     */
    //绑点专用
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {
        /*
        //获取绑点
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        
        //获取治疗值
        startAddHpValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue","ID",this.GetComponent<SkillObjBase>().SkillID,"Skill_Template"));

        Game_PublicClassVar.Get_function_MonsterSkill.MonsterAddHp(this.gameObject.transform.parent.gameObject, this.GetComponent<SkillObjBase>().SkillID);

        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //获取技能时间
        SkillTime = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));

        //实例化技能特效
        GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
        GameObject effect = (GameObject)Instantiate(SkillEffect);
        effect.SetActive(false);
        effect.transform.parent = this.transform;
        effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        effect.SetActive(true);

        
        //播放施法动作
        //________Game_PublicClassVar.Get_game_PositionVar.Rose_IfSkill = true;
        */

        //获取技能参数
        string[] parameterList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template").Split(',');
        TriggerTime = float.Parse(parameterList[0]);
        TriggerSkillID = parameterList[1];
        //获取技能时间
        //Debug.Log("SkillID123123 = " + this.GetComponent<SkillObjBase>().SkillID);
        SkillTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));

        //触发技能Buff
        /*
        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (buffID != "0" && buffID != "")
        {
            Debug.Log("触发BUFF:" + buffID);
            //Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget);
            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, this.gameObject.transform.parent.parent.parent.gameObject);
        }
        */
        //SkillTime = 30;
        //延迟时间注销
        Destroy(this.gameObject, SkillTime);
	}
	
	// Update is called once per frame
	void Update () {
        //间隔时间触发技能
        TriggerTimeSum = TriggerTimeSum + Time.deltaTime;
        if (TriggerTimeSum >= TriggerTime) {
            TriggerTimeSum = 0;
            /*
            if (this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AI_Target !=null) { 
            
            }
            */
            Game_PublicClassVar.Get_function_MonsterSkill.AddSkillID(TriggerSkillID,this.GetComponent<SkillObjBase>().MonsterSkillObj);
        }

        //判断目标是否眩晕
        if (this.GetComponent<SkillObjBase>().MonsterSkillObj != null) {
            if (this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>()!=null) {
                if (this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().ChenMoStatus) {
                    //注销自身
                    Destroy(this.gameObject);
                }
            }
        }
	}
}
