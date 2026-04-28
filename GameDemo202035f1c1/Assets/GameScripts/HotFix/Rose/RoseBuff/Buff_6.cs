using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

//加血BUFF
public class Buff_6 : MonoBehaviour {

    public ObscuredString BuffID;               //BuffID

    private GameObject SkillEffect;     //技能特效
    public ObscuredFloat buffTime;             //技能特效播放时间
    public ObscuredFloat buffTimeSum;          //技能持续时间累计值
    /*
    private float damgePro;             //攻击转换百分百
    private int damgeValue;             //固定值
    private float continuedTime;          //2次持续加血时间的间隔
    private float continuedTimeSum;         //2次间隔时间的累计时间
    */
    private ObscuredString ifImmediatelyUse;        //是否立即释放
    private ObscuredString targetType;              //目标类型
    private GameObject effect;
    //绑点专用
    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {

        //BuffID = "90010001";

        //获取绑点
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();

        //获取Buff间隔时间
        //continuedTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffParameter", "ID", BuffID, "SkillBuff_Template"));

        //获取治疗值
        //damgePro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgePro", "ID", BuffID, "SkillBuff_Template"));
        //damgeValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", BuffID, "SkillBuff_Template"));

        //获取Buff持续时间
        buffTime = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffTime", "ID", BuffID, "SkillBuff_Template"));

        //获取Buff是否立即释放
        ifImmediatelyUse = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfImmediatelyUse", "ID", BuffID, "SkillBuff_Template");

        //获取技能目标
        targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", BuffID, "SkillBuff_Template");

        //播放Buff特效
        string ifPlayEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfPlayEffect", "ID", BuffID, "SkillBuff_Template");
        if (ifPlayEffect == "1")
        {
            //获取特效名称
            string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", BuffID, "SkillBuff_Template");
            //获取播放点
            string effectPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectPosition", "ID", BuffID, "SkillBuff_Template");
            //effectName = "Eff_Buff_JiaXue_1";
            //实例化技能特效
            if (effectName != "" && effectName!="0")
            {
                GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
                effect = (GameObject)Instantiate(SkillEffect);
                effect.SetActive(false);
                //根据Buff目标绑定不同的位置
                if (targetType == "1")
                {
                    Rose_Bone bone = this.GetComponent<Rose_Bone>();
                    Transform effectTra = bone.BoneSet.transform.Find(effectPosition).transform;
                    effect.transform.parent = effectTra;
                }
                if (targetType == "2")
                {
                    AI_1 bone = this.GetComponent<AI_1>();
                    Transform effectTra = bone.BoneSet.transform.Find(effectPosition).transform;
                    effect.transform.parent = effectTra;
                }
                if (targetType == "3")
                {
                    AIPet bone = this.GetComponent<AIPet>();
                    Transform effectTra = bone.BoneSet.transform.Find(effectPosition).transform;
                    effect.transform.parent = effectTra;
                }

                effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                effect.SetActive(true);
            }

        }
        /*
        if (ifImmediatelyUse == "0") {
            continuedTimeSum = continuedTime;
        }
        */

        //触发技能效果
        buffUse();
	}
	
	// Update is called once per frame
	void Update () {
        
        buffTimeSum = buffTimeSum + Time.deltaTime;
        /*
        continuedTimeSum = continuedTimeSum + Time.deltaTime;
        if (continuedTimeSum >= continuedTime) {
            continuedTimeSum = 0;
            //触发一次Buff
            buffUse();
        }
        */
        //注销自己的脚本
        if (buffTimeSum > buffTime) {
            //注销特效
            if (effect != null)
            {
                Destroy(effect);
            }
            //注销自己  
            Destroy(this);
        }

        //获取绑定对象的生命是否为0，为0删除自身脚本及特效
        bool ifdeath = Game_PublicClassVar.Get_function_Skill.ifDeath(targetType, this.gameObject);
        if (ifdeath)
        {
            Destroy(effect);
            Destroy(this);
        }
	}

    //触发一次Buff效果
    void buffUse() {
        //玩家
        if (targetType == "1")
        {
            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseInvisibleStatus = true;
            Game_PublicClassVar.Get_function_Rose.EnterRoseInvisible();
        }
        /*
        //怪物加血
        if (targetType == "2") {
            Game_PublicClassVar.Get_function_AI.AI_addHp(this.gameObject, damgeValue);
        }
        //宠物加血
        if (targetType == "3")
        {
            Game_PublicClassVar.Get_function_AI.AI_addHp(this.gameObject, damgeValue);
        }
        */
    }

    //销毁脚本时调用
    void OnDestroy()
    {
        //玩家
        if (targetType == "1")
        {
            //自身只有一个buff_6（也就是隐身buff）时才可以将自身的隐身设置为false
            if (this.GetComponents<Buff_6>().Length == 1) {
                Game_PublicClassVar.Get_function_Rose.ExitRoseInvisible();
            }
        }
    }
}
