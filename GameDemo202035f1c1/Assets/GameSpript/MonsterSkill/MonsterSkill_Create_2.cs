using UnityEngine;
using System.Collections;
//召唤怪物技能(在指定地方创建N个同类怪物,不支持创建多个)
public class MonsterSkill_Create_2 : MonoBehaviour
{

	private GameObject SkillEffect;     //技能特效
	//private GameObject skillEffect;   //脚本中实例化的特效ID
    public float SkillTime;             //技能特效播放时间
    private float delayTime;            //延迟时间
    private float delayTimeSum;         //延迟时间累计
    /*
    private int startAddHpValue;        //初始加血的值
    private int continuedAddHpValue;    //持续的加血值
    private int continuedTime;          //2次持续加血时间的间隔
    */

    //绑点专用
    private Game_PositionVar game_PositionVar;
    private Object monsterObj;

	// Use this for initialization
	void Start () {

        //获取绑点
        /*
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        
        //获取治疗值
        startAddHpValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue","ID",this.GetComponent<SkillObjBase>().SkillID,"Skill_Template"));

        Game_PublicClassVar.Get_function_MonsterSkill.MonsterAddHp(this.gameObject.transform.parent.gameObject, this.GetComponent<SkillObjBase>().SkillID);

        //获取特效名称
        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        */
        
        //获取技能时间
        SkillTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        delayTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillDelayTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));

            //Game_PublicClassVar.Get_function_AI.AI_CreatMonster("70001004", Vec3);
            //Debug.Log("召唤怪物");

        /*
        //实例化技能特效
        GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
        GameObject effect = (GameObject)Instantiate(SkillEffect);
        effect.SetActive(false);
        effect.transform.parent = this.transform;
        effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        effect.SetActive(true);
        */
        
        //播放施法动作
        //________Game_PublicClassVar.Get_game_PositionVar.Rose_IfSkill = true;


        //延迟时间注销
        //Destroy(this.gameObject, SkillTime);
        //获取召唤列表
        string[] createList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template").Split(';');
        if (createList.Length > 0)
        {
            string createID = createList[0].Split(',')[0];
            monsterObj = Resources.Load("CreateMonster/" + createID, typeof(GameObject));
        }


        //封印之塔里的怪物不进行召唤
        if (this.GetComponent<SkillObjBase>().MonsterSkillObj!=null) {
            if (this.GetComponent<SkillObjBase>().MonsterSkillObj.GetComponent<AI_Property>().AI_MonsterCreateType == "2") {
                Destroy(this.gameObject);
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {

        //延迟召唤
        delayTimeSum = delayTimeSum + Time.deltaTime;
        if (delayTimeSum > delayTime) {
            createMonster();
        }

	}

    void createMonster() {
        //测试
        Vector3 Vec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        Vec3 = new Vector3(Vec3.x + Random.value, Vec3.y + 0.5f, Vec3.z + Random.value);

        //获取召唤列表
        string[] createList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template").Split(';');

        if (createList.Length > 1)
        {
            string createID = createList[0].Split(',')[0];
            int createNum = int.Parse(createList[0].Split(',')[1]);
            for (int x = 0; x <= createNum - 1; x++)
            {

                string[] Vec3Position = createList[1].Split(',');
                Vec3 = new Vector3(float.Parse(Vec3Position[0]), float.Parse(Vec3Position[1]), float.Parse(Vec3Position[2]));
                //Debug.Log("Vec3 = " + Vec3 + "this.GetComponent<SkillObjBase>().SkillID = " + this.GetComponent<SkillObjBase>().SkillID);
                monsterObj = Resources.Load("CreateMonster/" + createID, typeof(GameObject));
                Game_PublicClassVar.Get_function_AI.AI_CreatMonster(createID, Vec3, this.GetComponent<SkillObjBase>().MonsterSkillObj, monsterObj);

                /*
                //播放传送特效
                GameObject SkillObj = (GameObject)Resources.Load("Effect/Rose/" + "Rose_MoveScene", typeof(GameObject));
                GameObject SkillObject_p = (GameObject)Instantiate(SkillObj);

                GameObject effectObj = (GameObject)Instantiate((GameObject)Resources.Load("Effect/Skill/Rose_CriAct"));
                SkillObject_p.transform.position = Vec3;
                SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
                */

            }
        }

        //延迟时间注销
        Destroy(this.gameObject);
    }
}
