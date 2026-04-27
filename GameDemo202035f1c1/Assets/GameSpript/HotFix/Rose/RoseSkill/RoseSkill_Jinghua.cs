using UnityEngine;
using System.Collections;
//加血技能
public class RoseSkill_Jinghua : MonoBehaviour
{

	private GameObject SkillEffect;     //技能特效
	//private GameObject skillEffect;   //脚本中实例化的特效ID
    public float SkillTime;             //技能特效播放时间
    private int startAddHpValue;        //初始加血的值
    private int continuedAddHpValue;    //持续的加血值
    private float continuedTime;          //2次持续加血时间的间隔
    

    //绑点专用
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {
        Debug.Log("触发净化！");
        //获取绑点
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        
        //获取治疗值
        startAddHpValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue","ID",this.GetComponent<SkillObjBase>().SkillID,"Skill_Template"));
        Game_PublicClassVar.Get_function_Rose.addRoseHp(startAddHpValue);

        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //获取技能时间
        SkillTime = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));

        //实例化技能特效
        if (effectName != "0")
        {
            GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
            GameObject effect = (GameObject)Instantiate(SkillEffect);
            effect.SetActive(false);
            effect.transform.parent = this.transform;
            effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            effect.SetActive(true);
        }

        clearnBuff();

        //播放施法动作
        //________Game_PublicClassVar.Get_game_PositionVar.Rose_IfSkill = true;

        //延迟时间注销
        Destroy(this.gameObject, SkillTime);
	}
	
	// Update is called once per frame
	void Update () {

        continuedTime = continuedTime + Time.deltaTime;
        //间隔1秒触发一次净化效果
        if (continuedTime >= 0.1f) {
            continuedTime = 0;
            clearnBuff();
        }
	}

    void clearnBuff() {
        //获取并清除角色身上的所有buff
        GameObject roseObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;
        Buff_2[] buffList_2 = roseObj.GetComponents<Buff_2>();
        if (buffList_2.Length > 0)
        {
            for (int i = 0; i < buffList_2.Length; i++)
            {
                Destroy(buffList_2[i]);
            }
        }
        Buff_3[] buffList_3 = roseObj.GetComponents<Buff_3>();
        if (buffList_3.Length > 0)
        {
            for (int i = 0; i < buffList_3.Length; i++)
            {
                Destroy(buffList_3[i]);
            }
        }
        Buff_4[] buffList_4 = roseObj.GetComponents<Buff_4>();
        if (buffList_4.Length > 0)
        {
            for (int i = 0; i < buffList_4.Length; i++)
            {
                string buffID_4 = buffList_4[i].BuffID;
                string buffBenefitType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffBenefitType", "ID", buffID_4, "SkillBuff_Template");
                if (buffBenefitType == "2")
                {
                    Destroy(buffList_4[i]);
                }
            }
        }


        //触发技能Buff
        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (buffID != "0" && buffID != "")
        {
            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, game_PositionVar.Obj_Rose);
        }
    }
}
