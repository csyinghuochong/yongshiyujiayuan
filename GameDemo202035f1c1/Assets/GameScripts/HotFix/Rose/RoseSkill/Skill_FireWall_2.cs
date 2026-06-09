using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Skill_FireWall_2 : MonoBehaviour
{ 
    //定义技能状态
	public float SkillEffectDistance;   //技能每秒移动距离
    public float SkillTime;             //技能时间（特效和效果共用）
    public Vector3 SkillTargetPoint;    //技能移动到的点，根据此点判定移动的方向

    public float DamgeFrequency;            //2次伤害时间的间隔
    private float DamgeFrequencySum;        //2次伤害时间的间隔累加，用来累计时间
    private float SkillTimeSum;             //技能时间累计值，用来在时间到时清空怪物列表
    private GameObject AI_Collider;         //碰撞的怪物
    private List<GameObject> colliderList   = new List<GameObject>();  //碰撞到的怪物列表
    private int cillider_Num;               //碰撞体的怪物数量
    private string AI_HitPosition;          //AI受到伤害特效显示的位置
    private string AI_IfHitEffect;          //是否播放受击特效
    private string triggerSkillID;          //第一次伤害触发的技能ID
    private GameObject effect;
    private string skillHitEffectName;
    // Use this for initialization
    void Start () {

        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效名称
        skillHitEffectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //技能间隔时间
        string gameObjectParameter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取技能附加
        SkillTime = this.GetComponent<SkillObjBase>().SkillLiveTime;

        if (gameObjectParameter != "0")
        {
            string[] parameter = gameObjectParameter.Split(';');
            DamgeFrequency = float.Parse(parameter[0]);
            if (parameter.Length >= 2)
            {
                triggerSkillID = parameter[1];
            }
        }
        else {
            triggerSkillID = "0";
        }

        //实例化技能特效
        if (effectName != "" && effectName != null && effectName != "0") {

            GameObject SkillEffect = (GameObject)ResourcesManager.Instance.LoadEffectSync<GameObject>("Skill/" + effectName);
            if (SkillEffect != null)
            {
                effect = (GameObject)Instantiate(SkillEffect);
                effect.SetActive(false);
                effect.transform.parent = this.transform;
                effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                effect.SetActive(true);

            }
            else
            {
                //Debug.Log("技能" + effectName + "缺少技能特效");
            }

        }
        
        Destroy(this.gameObject, SkillTime);  //技能时间到注销此物体
        DamgeFrequencySum = DamgeFrequency;

    }
	
	// Update is called once per frame
	void Update () {

        //累计技能时间
        DamgeFrequencySum = DamgeFrequencySum + Time.deltaTime;
        SkillTimeSum = SkillTimeSum + Time.deltaTime;

        //技能结束清空怪物泛型
        if (SkillTimeSum >= SkillTime-0.2f) {
            colliderList.Clear();
            SkillTimeSum = 0.0f;
        }
	}

    private void OnDestroy()
    {
        //离开时销毁特效,主要针对燃烧特效
        Destroy(effect);
    }


    //第一次碰撞调用
    void OnTriggerEnter (Collider collider)
    {
        
        updataSkillData();                      //如果进入比Start方法早,需要读取一下技能配置

        //将进入技能范围的怪物加入进一个集合中
        if (collider.name != "Rose")
        {
            if (collider.gameObject.layer == 12)
            {
                if (collider.gameObject != null)
                {
                    //当碰撞体不是自己时触发
                    if (collider.gameObject != null)
                    {
                        colliderList.Add(collider.gameObject);

                        //触发技能Buff
                        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                        if (buffID != "0" && buffID != "")
                        {
                            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
                            
                            Game_PublicClassVar.Get_fight_Formult.RoseActMonster(this.gameObject.GetComponent<SkillObjBase>().SkillID, collider.gameObject,false);
                        }

                        //技能附加值（附加额外Buff）
                        Game_PublicClassVar.Get_function_Skill.SkillAddValue_Buff(this.GetComponent<SkillObjBase>().SkillID, collider.gameObject);
                    }
                }
            }

            if (collider.name == "Rose")
            {
                
                string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                if (buffID != "0" && buffID != "")
                {
                    
                    //Debug.Log("触发BUFF:" + buffID);
                    Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose);

                }
            }
        }
    }


	//碰撞范围内调用
	void OnTriggerStay(Collider collider)
    {
        
        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (buffID != "0" && buffID != "")
        {
            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
        }

        foreach (GameObject nowObj in colliderList)
        {

            AI_Collider = nowObj;

            if (AI_Collider != this.gameObject && AI_Collider != null)
            {
                Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, nowObj.gameObject);
            }
        }
    }


	//离开碰撞调用
	void OnTriggerExit (Collider collider)
    {
            colliderList.Remove(collider.gameObject);
	}

    void updataSkillData() {

        //读取首次触发的技能配置
        string gameObjectParameter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        if (gameObjectParameter != "0")
        {
            string[] parameter = gameObjectParameter.Split(';');
            DamgeFrequency = float.Parse(parameter[0]);
            if (parameter.Length >= 2)
            {
                triggerSkillID = parameter[1];
            }
        }
    }
}
