using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//冲击波技能 可以配置移动速度和受到的伤害
public class MonsterSkill_ChongZhuang_1 : MonoBehaviour
{

	//定义技能状态
	//public bool SkillStatus;
	public float SkillEffectDistance;   //技能每秒移动距离
    public float SkillTime;             //技能时间（特效和效果共用）
    public Vector3 SkillTargetPoint;    //技能移动到的点，根据此点判定移动的方向

    //public float DamgeFrequency;            //2次伤害时间的间隔
    //private float DamgeFrequencySum;        //2次伤害时间的间隔累加，用来累计时间
    private float SkillTimeSum;             //技能时间累计值，用来在时间到时清空怪物列表
    private GameObject AI_Collider;         //碰撞的怪物
    private List<GameObject> colliderList = new List<GameObject>();  //碰撞到的怪物列表
    private int cillider_Num;               //碰撞体的怪物数量
    private string AI_HitPosition;          //AI受到伤害特效显示的位置
    private string AI_IfHitEffect;          //是否播放受击特效

    private string gameObjectParameter;      //游戏参数
    private bool ifMoveStatus;
    private Quaternion monsterQuaternion;
    private Vector3 ai_StarPosition;

	// Use this for initialization
	void Start () {

        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取特效时间
        SkillTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));

        //技能两次触发时间间隔
        //DamgeFrequency = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));

        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        gameObjectParameter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        //实例化技能特效
        GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
        if (SkillEffect != null)
        {
            GameObject effect = (GameObject)Instantiate(SkillEffect);
            effect.SetActive(false);
            effect.transform.parent = this.transform;
            effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            effect.transform.localScale = new Vector3(1, 1, 1);
            effect.SetActive(true);
        }
        else
        {
            //Debug.Log("技能" + skillID + "缺少技能特效");
        }

        //设置技能位置
        /*
        SkillTargetPoint = this.gameObject.GetComponent<SkillObjBase>().SkillTargetPoint;
        this.gameObject.transform.localPosition = new Vector3(SkillTargetPoint.x, SkillTargetPoint.y, SkillTargetPoint.z);
        */
        SkillTime = 2;
        Destroy(this.gameObject, SkillTime);  //技能时间到注销此物体
        this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AIStopLookTargetStatus = true;
        monsterQuaternion = this.GetComponent<SkillObjBase>().MonsterSkillObj.transform.localRotation;

        //this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().ai_NavMesh.SetDestination(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
        //this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().ai_NavMesh.speed = 10;
        //this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>()

        //DamgeFrequencySum = DamgeFrequency;
        /*
        Debug.Log("DamgeFrequency = " + DamgeFrequency);
        DamgeFrequencySum = 1;
         */
        ai_StarPosition = this.GetComponent<SkillObjBase>().MonsterSkillObj.transform.position;       //获取AI的出生点


    }
	
	// Update is called once per frame
	void Update () {
        //累计技能时间
        //DamgeFrequencySum = DamgeFrequencySum + Time.deltaTime;
        /*
        SkillTimeSum = SkillTimeSum + Time.deltaTime;

        //技能结束清空怪物泛型
        if (SkillTimeSum >= SkillTime-0.2f) {
            //colliderList.Clear();
            SkillTimeSum = 0.0f;
        }
        */
        SkillEffectDistance = 15;    //设置奔跑速度
        //设置移动
        if (!ifMoveStatus) {
            this.GetComponent<SkillObjBase>().MonsterSkillObj.transform.Translate(Vector3.forward * SkillEffectDistance * Time.deltaTime);
            this.GetComponent<SkillObjBase>().MonsterSkillObj.transform.localRotation = monsterQuaternion;
            //时刻监测超过的逃离战斗的距离
            //怪物超过出生地一定距离后返回出生地
            float safetyDistance = Vector3.Distance(this.gameObject.transform.position, ai_StarPosition);
            if (safetyDistance >= 14.0f)
            {
                Debug.Log("冲撞超过距离");
                Destroy(this.gameObject);
            }
        }
	}

    //销毁时调用
    void OnDestroy() {
        //注释目标为否
        this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AIStopLookTargetStatus = false;
    }

	//第一次碰撞调用
	void OnTriggerEnter (Collider collider){
        //将进入技能范围的怪物加入进一个集合中
        if (collider.name == "Rose")
        {
            Debug.Log("添加Rose");
            if (collider.gameObject.layer == 14)
            {
                if (collider.gameObject != null)
                {
                    //当碰撞体不是自己时触发
                    if (collider.gameObject != null)
                    {
                        Debug.Log("正确添加Rose");
                        //colliderList.Add(collider.gameObject);
                        ifMoveStatus = true;
                        this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_1>().AIStopLookTargetStatus = false;
                        Debug.Log("停止移动");

                        Rose_Status ai = collider.gameObject.GetComponent<Rose_Status>();
                        Rose_Proprety ai_property = collider.gameObject.GetComponent<Rose_Proprety>();

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
                            //DamgeFrequencySum = 0.0f;       //清空累计时间
                            ai.RoseIfHit = true;            //受击特效
                            //发送攻击消息
                            Debug.Log("触发群体伤害");
                            Game_PublicClassVar.Get_fight_Formult.MonsterActRose(this.gameObject.GetComponent<SkillObjBase>().SkillID, this.gameObject.GetComponent<SkillObjBase>().MonsterSkillObj, collider.gameObject);

                            //触发技能Buff
                            string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                            if (buffID != "0" && buffID != "")
                            {
                                Debug.Log("开始触发怪物BUFF:" + buffID);
                                Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
                            }
                        }

                        Destroy(this.gameObject);  //技能时间到注销此物体
                    }
                }
            }
        }
        else {
            //碰撞到物体取消冲撞
            if (collider.gameObject.layer == 11) {
                Debug.Log("啊啊啊啊啊啊:"+collider.gameObject.name);
                Destroy(this.gameObject, SkillTime);  //技能时间到注销此物体
            }
        }

        Debug.Log("啊啊啊啊啊啊zzzzzz:" + collider.gameObject.name);
    }


	//碰撞范围内调用
    /*
	void OnTriggerStay(Collider collider){
        

            foreach (GameObject nowObj in colliderList) {

                AI_Collider = nowObj;

                if (AI_Collider != this.gameObject)
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
                        //DamgeFrequencySum = 0.0f;       //清空累计时间
                        ai.RoseIfHit = true;            //受击特效
                        //发送攻击消息
                        Debug.Log("触发群体伤害");
                        Game_PublicClassVar.Get_fight_Formult.MonsterActRose(this.gameObject.GetComponent<SkillObjBase>().SkillID, this.gameObject.GetComponent<SkillObjBase>().MonsterSkillObj);

                        //触发技能Buff
                        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                        if (buffID != "0" && buffID != "")
                        {
                            Debug.Log("开始触发怪物BUFF:" + buffID);
                            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
                        }
                    }
                }
            }
    }
    */

	//离开碰撞调用
	void OnTriggerExit (Collider collider){

            colliderList.Remove(collider.gameObject);
            Debug.Log(collider.name+"离开的碰撞体");

	}


}
