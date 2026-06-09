using System.Collections.Generic;
using UnityEngine;


public class RoseSkill_Around : MonoBehaviour
{
    //定义技能状态
    public float SkillEffectDistance; //技能每秒移动距离
    public float SkillTime; //技能时间（特效和效果共用）
    public Vector3 SkillTargetPoint; //技能移动到的点，根据此点判定移动的方向

    public float DamgeFrequency; //2次伤害时间的间隔
    private float DamgeFrequencySum; //2次伤害时间的间隔累加，用来累计时间
    private float SkillTimeSum; //技能时间累计值，用来在时间到时清空怪物列表
    private GameObject AI_Collider; //碰撞的怪物
    private List<GameObject> colliderList = new List<GameObject>(); //碰撞到的怪物列表
    private int cillider_Num; //碰撞体的怪物数量
    private string AI_HitPosition; //AI受到伤害特效显示的位置
    private string AI_IfHitEffect; //是否播放受击特效
    private string triggerSkillID; //第一次伤害触发的技能ID
    private GameObject effect;
    private string skillHitEffectName;


    // 球数量
    public int ballCount = 3;

    // 环绕半径
    public float radius = 2f;

    // 旋转速度
    public float rotateSpeed = 120f;

    // 球对象列表
    private List<GameObject> ballList = new List<GameObject>();

    // 每个球的角度
    private List<float> angleList = new List<float>();

    // 每个球自己的攻击时间
    private List<float> hitTimeList = new List<float>();

    void Start()
    {

        //获取是否播放受击特效
        AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID",
            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效播放位置
        AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID",
            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效名称
        skillHitEffectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID",
            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID",
            this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //技能间隔时间
        string gameObjectParameter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter",
            "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
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
        else
        {
            triggerSkillID = "0";
        }

        //实例化技能特效
        if (effectName != "" && effectName != null && effectName != "0")
        {

            GameObject SkillEffect =
                (GameObject)ResourcesManager.Instance.LoadEffectSync<GameObject>("Skill/" + effectName);
            if (SkillEffect != null)
            {
                // effect = (GameObject)Instantiate(SkillEffect);
                // effect.SetActive(false);
                // effect.transform.parent = this.transform;
                // effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                // effect.SetActive(true);


                for (int i = 0; i < ballCount; i++)
                {

                    effect = (GameObject)Instantiate(SkillEffect);
                    effect.SetActive(false);
                    effect.transform.parent = this.transform;

                    // 初始化角度
                    float angle = i * Mathf.PI * 2 / ballCount;

                    angleList.Add(angle);

                    // 初始化攻击时间
                    hitTimeList.Add(-999f);

                    // 初始位置
                    Vector3 pos = new Vector3(
                        Mathf.Cos(angle) * radius,
                        1f,
                        Mathf.Sin(angle) * radius
                    );

                    effect.transform.localPosition = pos;

                    ballList.Add(effect);

                    effect.SetActive(true);
                }

                this.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform);
            }
        }

        Destroy(this.gameObject, SkillTime); //技能时间到注销此物体
        DamgeFrequencySum = DamgeFrequency;

    }

    // Update is called once per frame
    void Update()
    {

        //累计技能时间
        DamgeFrequencySum = DamgeFrequencySum + Time.deltaTime;
        SkillTimeSum = SkillTimeSum + Time.deltaTime;

        //技能结束清空怪物泛型
        if (SkillTimeSum >= SkillTime - 0.2f)
        {
            colliderList.Clear();
            SkillTimeSum = 0.0f;
        }


        for (int i = 0; i < ballList.Count; i++)
        {
            angleList[i] += rotateSpeed * Mathf.Deg2Rad * Time.deltaTime;

            Vector3 pos = new Vector3(
                Mathf.Cos(angleList[i]) * radius,
                1f,
                Mathf.Sin(angleList[i]) * radius
            );

            ballList[i].transform.localPosition = pos;

        }
    }

    private void OnDestroy()
    {
        //离开时销毁特效,主要针对燃烧特效
        Destroy(effect);
    }


    //第一次碰撞调用
    void OnTriggerEnter(Collider collider)
    {

        updataSkillData(); //如果进入比Start方法早,需要读取一下技能配置

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

                        for (int i = 0; i < ballList.Count; i++)
                        {
                            GameObject ball = ballList[i];

                            if (ball == null)
                                continue;

                            // 球与敌人的距离
                            float dis = Vector3.Distance(
                                ball.transform.position,
                                collider.transform.position
                            );

                            // 球碰撞范围
                            if (dis <= 1f)
                            {
                                // 判断球攻击CD
                                if (Time.time - hitTimeList[i] < DamgeFrequency)
                                {
                                    continue;
                                }

                                // 记录攻击时间
                                hitTimeList[i] = Time.time;

                                //触发技能Buff
                                string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID",
                                    "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                                if (buffID != "0" && buffID != "")
                                {
                                    Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, collider.gameObject);
                                }

                                //技能附加值（附加额外Buff）
                                Game_PublicClassVar.Get_function_Skill.SkillAddValue_Buff(
                                    this.GetComponent<SkillObjBase>().SkillID, collider.gameObject);

                                Game_PublicClassVar.Get_fight_Formult.RoseActMonster(
                                    this.gameObject.GetComponent<SkillObjBase>().SkillID, collider.gameObject, false);
                            }


                        }

                    }

                }
            }
        }
    }
    
    void updataSkillData()
    {
        //读取首次触发的技能配置
        string gameObjectParameter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter",
            "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

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