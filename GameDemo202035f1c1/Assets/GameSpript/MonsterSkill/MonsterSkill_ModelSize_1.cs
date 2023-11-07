using UnityEngine;
using System.Collections;
//加血技能
public class MonsterSkill_ModelSize_1 : MonoBehaviour
{

	private GameObject SkillEffect;     //技能特效
	//private GameObject skillEffect;   //脚本中实例化的特效ID
    public float SkillTime;             //技能特效播放时间
    private int startAddHpValue;        //初始加血的值
    private int continuedAddHpValue;    //持续的加血值
    private int continuedTime;          //2次持续加血时间的间隔
    
    private string ParameterValue;      //参数值
    private float sizeValue;            //变化值
    private float now_ChangeSize;       //当前改变变化值
    private float sizeTime;             //变化时间
    private float sizeTimeSum;
    private Vector3 changeVec3;
    private GameObject changeObj;       //改变Obj
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
        */
        ParameterValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        sizeValue = float.Parse(ParameterValue.Split(',')[0]);
        sizeTime = float.Parse(ParameterValue.Split(',')[1]);
        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //获取技能时间
        SkillTime = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));

        //实例化技能特效
        if (effectName != "0" && effectName != "") {
            GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
            GameObject effect = (GameObject)Instantiate(SkillEffect);
            effect.SetActive(false);
            effect.transform.parent = this.transform;
            effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            effect.SetActive(true);
        }

        //触发技能Buff
        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (buffID != "0" && buffID != "")
        {
            Debug.Log("触发BUFF:" + buffID);
            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, this.gameObject.transform.parent.parent.parent.gameObject);
        }

        changeObj = this.gameObject.transform.parent.parent.parent.gameObject;
        changeVec3 = this.gameObject.transform.parent.gameObject.transform.localScale;
        //延迟时间注销
        Destroy(this.gameObject, SkillTime);
	}
	
	// Update is called once per frame
	void Update () {
        sizeTimeSum = sizeTimeSum + Time.deltaTime;
        now_ChangeSize = sizeTimeSum / sizeTime;
        if (now_ChangeSize >= 1)
        {
            now_ChangeSize = 1;
        }
        changeObj.transform.localScale = new Vector3(changeVec3.x * (1 + now_ChangeSize * sizeValue), changeVec3.y * (1 + now_ChangeSize * sizeValue), changeVec3.z * (1 + now_ChangeSize * sizeValue));
	}
}
