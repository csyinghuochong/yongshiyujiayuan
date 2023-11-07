using UnityEngine;
using System.Collections;
//指定时间删除释放此技能的怪物
public class MonsterSkill_DeleteMonster_1 : MonoBehaviour
{

	private GameObject SkillEffect;     //技能特效
	//private GameObject skillEffect;   //脚本中实例化的特效ID
    public float SkillTime;             //技能特效播放时间
    private int startAddHpValue;        //初始加血的值
    private int continuedAddHpValue;    //持续的加血值
    private int continuedTime;          //2次持续加血时间的间隔
    public float TimeSum;

    //绑点专用
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {

        //获取绑点
        /*
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        
        //获取治疗值
        startAddHpValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue","ID",this.GetComponent<SkillObjBase>().SkillID,"Skill_Template"));

        Game_PublicClassVar.Get_function_MonsterSkill.MonsterAddHp(this.gameObject.transform.parent.parent.parent.gameObject, this.GetComponent<SkillObjBase>().SkillID);

        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //获取技能时间
        SkillTime = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));

        //实例化技能特效
        if (effectName != "0") {
            GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
            GameObject effect = (GameObject)Instantiate(SkillEffect);
            effect.SetActive(false);
            effect.transform.parent = this.transform;
            effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            effect.SetActive(true);
        }

        
        
        //播放施法动作
        //________Game_PublicClassVar.Get_game_PositionVar.Rose_IfSkill = true;
        */

        //获取技能时间
        SkillTime = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        //Debug.Log("销毁召唤物");
        this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().IfDeathDestoryModel = true;
        this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_Property>().AI_Hp = 0;
        //Destroy(this.GetComponent<SkillObjBase>().MonsterSkillObj);

        //Destroy(this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().UI_Hp);

        //延迟时间注销
        Destroy(this.gameObject, SkillTime);
	}
	
	// Update is called once per frame
	void Update () {
        TimeSum = TimeSum + Time.deltaTime;
	}

    void OnDestroy() {

        
    }
    
}
