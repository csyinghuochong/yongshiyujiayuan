using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 护体之球 周身环绕3个旋转的球
public class Skill_OrbitBall : MonoBehaviour
{
    private GameObject AI_Collider; //碰撞的怪物
    private string AI_HitPosition; //AI受到伤害特效显示的位置
    private string AI_IfHitEffect; //是否播放受击特效
    private string AI_HitEffect; //AI受击特效
    private Rose_Bone roseBone;
    private float jiaodu;
    public float SkillTime; //技能时间
    
    private int ballNum = 3; //球的数量
    private float ballDistance = 1.5f; //球到中心的距离
    private float ballRadius = 0.5f; //球触发器的半径，特效可能得手动调整，或者把特效的配置也加到GameObjectParameter中
    private float interval = 0.5f; //每个球对每个单位的伤害间隔
    private float speed = 1f; //旋转速度
    private List<GameObject> balls = new();
    private Dictionary<(GameObject, GameObject), float> lastTrigger = new();

    void Start()
    {
        balls.Clear();
        lastTrigger.Clear();

        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        roseBone = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>();
        SkillTime = this.GetComponent<SkillObjBase>().SkillLiveTime;
        
        //获取技能参数
        string skillPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (skillPar != "0")
        {
            string[] skillParValue = skillPar.Split(';');
            ballNum = int.Parse(skillParValue[0]);
            ballDistance = float.Parse(skillParValue[1]);
            ballRadius = float.Parse(skillParValue[2]);
            interval = float.Parse(skillParValue[3]);
            speed = float.Parse(skillParValue[4]);
        }

        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        
        //实例化技能特效
        if (effectName != "0")
        {
            GameObject skillEffect = (GameObject)ResourcesManager.Instance.LoadEffectSync<GameObject>("Skill/" + effectName);

            for (int i = 0; i < ballNum; i++)
            {
                GameObject effect = (GameObject)Instantiate(skillEffect, this.transform, true);
                effect.SetActive(false);
                effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

                // 围绕中心点环形排列
                float angle = i * (360f / ballNum);
                float rad = angle * Mathf.Deg2Rad;
                float x = Mathf.Cos(rad) * ballDistance;
                float z = Mathf.Sin(rad) * ballDistance;
                effect.transform.localPosition = new Vector3(x, 0, z);

                SphereCollider sphereCollider = effect.AddComponent<SphereCollider>();
                sphereCollider.isTrigger = true;
                sphereCollider.radius = ballRadius;

                effect.AddComponent<OrbitBall>();

                effect.SetActive(true);

                balls.Add(effect);
            }
        }
        
        Destroy(this.gameObject, SkillTime);
    }


    void Update()
    {
        //始终跟随这Rose进行移动、旋转
        if (roseBone != null)
        {
            this.transform.position = roseBone.Bone_Center.transform.position;
        }

        jiaodu = jiaodu + Time.deltaTime * 60 * speed;
        if (jiaodu >= 360)
        {
            jiaodu = 0;
        }

        this.transform.localRotation = Quaternion.Euler(0, jiaodu, 0);
    }

    public void OnBallTriggerEnter(GameObject ball, Collider collider)
    {
        AI_Collider = collider.gameObject;

        if (lastTrigger.ContainsKey((this.gameObject, collider.gameObject)))
        {
            float last =  lastTrigger[(this.gameObject, collider.gameObject)];
            if (Time.time - last < interval)
            {
                return;
            }
            
            lastTrigger[(this.gameObject, collider.gameObject)] = Time.time;
        }
        else
        {
            lastTrigger.Add((this.gameObject, collider.gameObject), Time.time);
        }
        
        if (AI_Collider != this.gameObject && AI_Collider.layer == 12)
        {
            //Debug.Log("碰撞体：" + collider.name);
            AI_1 ai = collider.gameObject.GetComponent<AI_1>();
            AI_Property ai_property = collider.gameObject.GetComponent<AI_Property>();

            if (ai_property != null)
            {
                //是否播放受击特效
                switch (AI_IfHitEffect)
                {
                    case "0":
                        Debug.Log("collider = " + collider.name);
                        collider.gameObject.GetComponent<AI_1>().IfHitEffect = false; //不播放
                        ai.HitStatus = true; //受击特效
                        break;
                    case "1":
                        //获取受击特效播放特效
                        AI_HitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                        if (AI_HitEffect != "" && AI_HitEffect != "0")
                        {
                            if (collider.gameObject != null && collider.gameObject.GetComponent<AI_1>() != null)
                            {
                                collider.gameObject.GetComponent<AI_1>().IfHitEffect = true; //播放
                                //实例化技能特效
                                GameObject SkillEffect = (GameObject)ResourcesManager.Instance.LoadEffectSync<GameObject>("Skill/" + AI_HitEffect);
                                GameObject effect = (GameObject)MonoBehaviour.Instantiate(SkillEffect);
                                collider.gameObject.GetComponent<AI_1>().HitEffect = effect;
                                collider.gameObject.GetComponent<AI_1>().HitEffectt_Position = collider.gameObject.GetComponent<AI_1>().BoneSet.transform.Find(AI_HitPosition).gameObject; //设置播放位置
                            }
                        }

                        break;
                }

                Game_PublicClassVar.Get_fight_Formult.RoseActMonster(this.gameObject.GetComponent<SkillObjBase>().SkillID, collider.gameObject, false);

                //触发技能Buff
                string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                if (buffID != "0" && buffID != "")
                {
                    Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
                }
            }
        }
    }
}

public class OrbitBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.GetComponent<Skill_OrbitBall>()?.OnBallTriggerEnter(gameObject, other);
    }

    private void OnTriggerStay(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
    }
}