using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//冲击波技能 可以配置移动速度和受到的伤害,此脚本会使怪物360°旋转一圈
public class MonsterSkill_FireWall_2 : MonoBehaviour
{

	//定义技能状态
	//public bool SkillStatus;
	public float SkillEffectDistance;   //技能每秒移动距离
    public float SkillTime;             //技能时间（特效和效果共用）
    public Vector3 SkillTargetPoint;    //技能移动到的点，根据此点判定移动的方向

    public float DamgeFrequency;            //2次伤害时间的间隔
    private float DamgeFrequencySum;        //2次伤害时间的间隔累加，用来累计时间
    private float SkillTimeSum;             //技能时间累计值，用来在时间到时清空怪物列表
    private GameObject AI_Collider;         //碰撞的怪物
    public List<GameObject> colliderList = new List<GameObject>();  //碰撞到的怪物列表
    private int cillider_Num;               //碰撞体的怪物数量
    private string AI_HitPosition;          //AI受到伤害特效显示的位置
    private string AI_IfHitEffect;          //是否播放受击特效

    private float XuanZhuanTime;
    private float XuanZhuanTimeSum;
    private float XuanZhuanValue;

	// Use this for initialization
	void Start () {

        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //获取特效时间
        SkillTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));

        //技能两次触发时间间隔
        DamgeFrequency = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));

        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //实例化技能特效
        GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
        if (SkillEffect != null)
        {
            GameObject effect = (GameObject)Instantiate(SkillEffect);
            effect.SetActive(false);
            effect.transform.parent = this.transform;
            effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            effect.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            effect.SetActive(true);
        }
        else
        {
            //Debug.Log("技能" + skillID + "缺少技能特效");
        }

        //设置技能位置
        SkillTargetPoint = this.gameObject.GetComponent<SkillObjBase>().SkillTargetPoint;
        this.gameObject.transform.localPosition = new Vector3(SkillTargetPoint.x, SkillTargetPoint.y, SkillTargetPoint.z);

        Destroy(this.gameObject, SkillTime);  //技能时间到注销此物体

        this.GetComponent<SkillObjBase>().MonsterSkillObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //DamgeFrequencySum = DamgeFrequency;
        //Debug.Log("DamgeFrequency = " + DamgeFrequency);
	}
	
	// Update is called once per frame
	void Update () {
        //累计技能时间
        DamgeFrequencySum = DamgeFrequencySum + Time.deltaTime;
        SkillTimeSum = SkillTimeSum + Time.deltaTime;
        //Debug.Log("DamgeFrequencySum = " + DamgeFrequencySum);
        //技能结束清空怪物泛型
        if (SkillTimeSum >= SkillTime-0.2f) {
            //colliderList.Clear();
            SkillTimeSum = 0.0f;
        }

        XuanZhuanTimeSum = XuanZhuanTimeSum + Time.deltaTime;
        XuanZhuanValue = XuanZhuanTimeSum * 60.0f;  //6秒旋转一圈
        this.GetComponent<SkillObjBase>().MonsterSkillObj.transform.rotation = Quaternion.Euler(new Vector3(0, XuanZhuanValue, 0));
        //转一圈后清空数据
        if (XuanZhuanValue >= 360.0f) {
            XuanZhuanTimeSum = 0;
        }

	}


	//第一次碰撞调用
	void OnTriggerEnter (Collider collider){
        //将进入技能范围的怪物加入进一个集合中
        if (collider.gameObject.layer == 14 || collider.gameObject.layer == 18)
        {
            if (collider.gameObject != null)
            {
                //当碰撞体不是自己时触发
                if (collider.gameObject != null)
                {
                    //Debug.Log("正确添加Rose");
                    colliderList.Add(collider.gameObject);
                }
            }
        }
        else
        {
            bool addStatus = true;
            foreach (GameObject nowObj in colliderList)
            {
                //等于怪物层级
                if (collider.gameObject.layer == 12)
                {

                    if (nowObj.gameObject == collider.gameObject)
                    {
                        addStatus = false;
                    }

                }
            }

            if (addStatus)
            {
                colliderList.Add(collider.gameObject);
            }
        }
    }


	//碰撞范围内调用
	void OnTriggerStay(Collider collider){
        //Debug.Log("DamgeFrequencySum = " + DamgeFrequencySum);
        if (DamgeFrequencySum >= DamgeFrequency)
        {
            //Debug.Log("进来了");
            //Debug.Log("colliderList = " + colliderList.Count);
            DamgeFrequencySum = 0.0f;
            foreach (GameObject nowObj in colliderList) {
                //Debug.Log("AAAAA");
                AI_Collider = nowObj;

                    if (AI_Collider != this.gameObject)
                    {

                        //碰撞Rose
                        if (AI_Collider.gameObject.layer == 14)
                        {

                            Rose_Status ai = AI_Collider.gameObject.GetComponent<Rose_Status>();
                            Rose_Proprety ai_property = AI_Collider.gameObject.GetComponent<Rose_Proprety>();

                            if (ai_property != null)
                            {

                                //是否播放受击特效
                                switch (AI_IfHitEffect)
                                {
                                    case "0":
                                        //AI_Collider.gameObject.GetComponent<Rose_Status>().IfHitEffect = false;   //不播放
                                        //AI_Collider.gameObject.GetComponent<Rose_Status>().HitStatus = true;
                                        break;
                                    case "1":
                                        //AI_Collider.gameObject.GetComponent<Rose_Status>().IfHitEffect = true;    //播放
                                        //AI_Collider.gameObject.GetComponent<Rose_Status>().HitStatus = true;
                                        //AI_Collider.gameObject.GetComponent<Rose_Status>().HitEffectt_Position = AI_Collider.gameObject.GetComponent<AI_1>().BoneSet.transform.Find(AI_HitPosition).gameObject;  //设置播放位置
                                        break;
                                }
                                DamgeFrequencySum = 0.0f;       //清空累计时间
                                ai.RoseIfHit = true;            //受击特效
                                //发送攻击消息
                                //Debug.Log("触发群体伤害");
                                Game_PublicClassVar.Get_fight_Formult.MonsterActRose(this.gameObject.GetComponent<SkillObjBase>().SkillID, this.gameObject.GetComponent<SkillObjBase>().MonsterSkillObj, AI_Collider);

                                //触发技能Buff
                                string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                                if (buffID != "0" && buffID != "")
                                {
                                    //Debug.Log("开始触发怪物BUFF:" + buffID);
                                    Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, AI_Collider.gameObject, this.gameObject);
                                }

                            }
                        }

                        //碰撞怪物
                        if (AI_Collider.gameObject.layer == 12)
                        {

                            //触发技能Buff
                            string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                            if (buffID != "0" && buffID != "")
                            {
                                Debug.Log("怪物开始触发怪物BUFF:" + buffID);
                                Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, AI_Collider.gameObject);
                            }
                        }


                        //碰撞宠物
                        if (AI_Collider.layer == 18)
                        {
                            AI_Collider.GetComponent<AIPet>().HitStatus = true;      //播放受击特效
                            Game_PublicClassVar.Get_fight_Formult.MonsterActRose(this.gameObject.GetComponent<SkillObjBase>().SkillID, this.gameObject.GetComponent<SkillObjBase>().MonsterSkillObj, AI_Collider);

                            //触发技能Buff
                            string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                            if (buffID != "0" && buffID != "")
                            {
                                Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
                            }
                        }

                }

                //}

            }
        }
    }


	//离开碰撞调用
	void OnTriggerExit (Collider collider){

            colliderList.Remove(collider.gameObject);
            //Debug.Log(collider.name+"离开的碰撞体");

	}


}
