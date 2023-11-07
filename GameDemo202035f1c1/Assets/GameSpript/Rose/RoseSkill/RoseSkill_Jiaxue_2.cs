using UnityEngine;
using System.Collections;
//药物加血技能
public class RoseSkill_Jiaxue_2 : MonoBehaviour
{

	private GameObject SkillEffect;     //技能特效
	//private GameObject skillEffect;   //脚本中实例化的特效ID
    public float SkillTime;             //技能特效播放时间
    private int startAddHpValue;        //初始加血的值
    private int continuedAddHpValue;    //持续的加血值
    private int continuedTime;          //2次持续加血时间的间隔
    

    //绑点专用
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {

        //获取绑点
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        
        //获取治疗值
        startAddHpValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue","ID",this.GetComponent<SkillObjBase>().SkillID,"Skill_Template"));
        Game_PublicClassVar.Get_function_Rose.addRoseHp(startAddHpValue,"2");

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

        //触发技能Buff
        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (buffID != "0" && buffID != "")
        {
            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, game_PositionVar.Obj_Rose);
        }

        //获取技能参数
        string skillPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (skillPar != "0") {
            string[] skillParValue = skillPar.Split(';');
            int hudunValue = 0; //初始化护盾值
            switch (skillParValue[0]) { 
                //护盾
                case "1":
                    switch (skillParValue[1]) {
                        //最大生命值
                        case "1":
                            hudunValue = (int)(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hp * float.Parse(skillParValue[2]));
                        break;
                        //最大攻击
                        case "2":
                            hudunValue = (int)(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMax * float.Parse(skillParValue[2]));
                        break;
                    }
                    Debug.Log("hudunValue = " + hudunValue + " Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hp = " + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hp + " skillParValue[1] = " + skillParValue[1]);
                    hudunValue = hudunValue + int.Parse(skillParValue[3]);
                    Debug.Log("角色附加护盾：护盾值 = " + hudunValue + "持续时间 = " + skillParValue[4]);
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunStatus = true;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunValue = hudunValue;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunTime = int.Parse(skillParValue[4]);

                    //添加护盾特效
                    GameObject hudunEffect = (GameObject)Resources.Load("Effect/Skill/" + skillParValue[5], typeof(GameObject));
                    GameObject effect = (GameObject)Instantiate(hudunEffect);
                    effect.SetActive(false);
                    //effect.transform.parent = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Center.transform;
                    effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    effect.SetActive(true);
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunEffect = effect;
                break;
            }
        }



        
        //播放施法动作
        //________Game_PublicClassVar.Get_game_PositionVar.Rose_IfSkill = true;



        //延迟时间注销
        Destroy(this.gameObject, SkillTime);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
