using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

//嘲讽技能
public class RoseSkill_FanGun_1 : MonoBehaviour
{

	private GameObject SkillEffect;     //技能特效
	//private GameObject skillEffect;   //脚本中实例化的特效ID
    public ObscuredFloat SkillTime;             //技能特效播放时间
    private string skillHitEffectName;      //技能受击特效
    private string AI_HitPosition;          //技能受击特效播放点
    private string AI_IfHitEffect;          //播放技能特效
    //private int startAddHpValue;        //初始加血的值
    //private int continuedAddHpValue;    //持续的加血值
    //private int continuedTime;          //2次持续加血时间的间隔
    
    //存储朝向
    private ObscuredFloat chaoXiang_Y;
    public ObscuredFloat FanGunSpeedValue;
    public ObscuredFloat chiXuTime;

    //绑点专用
    private Game_PositionVar game_PositionVar;
    public ObscuredBool MoveStatus;
	// Use this for initialization
	void Start () {
        //Debug.Log("触发翻滚");
        //获取绑点
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        //是否播放特效
        //AI_IfHitEffect = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfHitEffect", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效名称
        //skillHitEffectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        //获取受击特效播放位置
        //AI_HitPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitPosition", "ID", this.gameObject.GetComponent<SkillObjBase>().SkillID, "Skill_Template");

        /*
        //实例化技能特效
        GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
        GameObject effect = (GameObject)Instantiate(SkillEffect);
        effect.SetActive(false);
        effect.transform.parent = this.transform;
        effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        effect.SetActive(true);
        
        GameObject actObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
        actObj.GetComponent<AI_1>().HitStatus = true;

        //触发BUFF
        //触发技能Buff
        string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (buffID != "0" && buffID != "")
        {
            Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, actObj.gameObject);
        }

        Game_PublicClassVar.Get_fight_Formult.RoseActMonster(this.gameObject.GetComponent<SkillObjBase>().SkillID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget, false);

        //播放受击特效
        if (AI_IfHitEffect == "1") {
            Debug.Log("嘲讽播放受击特效skillHitEffectName = " + skillHitEffectName);
            GameObject collider = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
            collider.gameObject.GetComponent<AI_1>().IfHitEffect = true;    //播放
            collider.gameObject.GetComponent<AI_1>().HitEffectt_Position = collider.gameObject.GetComponent<AI_1>().BoneSet.transform.Find(AI_HitPosition).gameObject;  //设置播放位置
            if (skillHitEffectName != "" && skillHitEffectName != "0")
            {
                Debug.Log("嘲讽播放受击特效222");
                collider.GetComponent<AI_1>().HitEffect = (GameObject)Resources.Load("Effect/Skill/" + skillHitEffectName);
            }
        }

        */
        //延迟时间注销
        //Destroy(this.gameObject, SkillTime);

        //SkillTime = 1.0f;
        Destroy(this, SkillTime);

        //获取自身朝向
        chaoXiang_Y = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.localRotation.y;

        //设置移动到的区域
        Vector3 vec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.localPosition;

        //给自己添加一个加速Buff

        //让玩家在移动区间不能控制轮盘
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().FanGunStatus = true;

        //Debug.Log("Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = " + Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus);
        //设置移动方向
        if (Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus&& Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_YaoGan.GetComponent<UI_GameYaoGan>().yaoganDragStatus) {
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().YaoGanPositionVec3 != null) {
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.LookAt(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().YaoGanPositionVec3);
            }
        }

        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ifAutomatic = false;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Status = false;
    }

    // Update is called once per frame
    void Update () {

        if (MoveStatus)
        {
            //持续向前移动
            if (chiXuTime <= 0.25f)
            {
                if (chiXuTime <= 0.2f)
                {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<UnityEngine.AI.NavMeshAgent>().angularSpeed = 0;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
                    chiXuTime = chiXuTime + Time.deltaTime;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * FanGunSpeedValue);
                }
                else {
                    //翻滚缓冲
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<UnityEngine.AI.NavMeshAgent>().angularSpeed = 0;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
                    chiXuTime = chiXuTime + Time.deltaTime;
                    float chiXuTime_Value = 0.25f - chiXuTime;
                    if (chiXuTime_Value < 0) {
                        chiXuTime_Value = 0;
                    }
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * FanGunSpeedValue * (chiXuTime_Value / 0.05f));
                }
            }
            else
            {
                MoveStatus = false;
                chiXuTime = 0;
                //立即想自身移动一下
                //Debug.Log(" Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.localPosition = " + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.localPosition);
                //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
                //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Position = new Vector3(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.localPosition.x + 5f, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.localPosition.y, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.localPosition.z);
                //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Status = true;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ai_nav.SetDestination(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().roseAnimator.Play("Idle");
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<UnityEngine.AI.NavMeshAgent>().angularSpeed = 360;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 4;
                //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Status = false;
                Destroy(this);
            }
        }
        else {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().FanGunStatus = false;
        }
	}

    //卸载时调用
    void OnDestroy()
    {
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().FanGunStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
    }
}
