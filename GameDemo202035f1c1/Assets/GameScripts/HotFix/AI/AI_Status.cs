using UnityEngine;
using System.Collections;


//此脚本会根据对应的值显示对应的动画
public class AI_Status : MonoBehaviour {

    //0：待机
    //1：走动
    //2：跑动
    //3：攻击
    //4: 施放技能
    //5: 吟唱状态
    //6: 死亡
    public int AI_StatusValue;
    public bool IfUpdateStatus;             //打开此开关表示更新一次AI动作
    public Animator AI_Animator;

    private void Awake()
    {
        AI_Animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {

        IfUpdateStatus = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (IfUpdateStatus) {

            //保证动作更新只执行一次
            IfUpdateStatus = false;
            
            if (AI_Animator.runtimeAnimatorController != null &&
                AI_Animator.runtimeAnimatorController.name == "Rose_QiShi")
            {
                UpdateQiShiAnimation();
                return;
            }

            switch (AI_StatusValue){
            
                //待机
                case 0:

                    //AI_Animator.Play("Idle");
                    AI_Animator.SetBool("idle", true);
                    AI_Animator.SetBool("act", false);
                    AI_Animator.SetBool("run", false);
                    AI_Animator.SetBool("walk", false);
                    
                break;

                //走动
                case 1:

                    //AI_Animator.Play("Walk");
                    AI_Animator.SetBool("walk", true);
                    AI_Animator.SetBool("idle", false);
                    AI_Animator.SetBool("run", false);
                    AI_Animator.SetBool("act", false);

                break;

                //跑动
                case 2:
                    //AI_Animator.Play("Run");
                    AI_Animator.SetBool("run", true);
                    AI_Animator.SetBool("act", false);
                    AI_Animator.SetBool("idle", false);
                    AI_Animator.SetBool("walk", false);

                break;

                //攻击
                case 3:
                    //AI_Animator.Play("Attack_1");
                    AI_Animator.SetBool("act", true);
                    AI_Animator.SetBool("run", false);
                    AI_Animator.SetBool("idle", false);
                    AI_Animator.SetBool("walk", false);
                    AI_StatusValue = 0;
                    IfUpdateStatus = true;
                    

                break;

                //释放技能
                case 4:

                break;

                //吟唱状态
                case 5:

                break;

                //死亡状态
                case 6:

                    AI_Animator.Play("Death");
                    AI_Animator.SetBool("act", false);
					AI_Animator.SetBool("run", false);
					AI_Animator.SetBool("idle", false);
					AI_Animator.SetBool("walk", false);
                break;

            }

        }
	
	}
    
    private void UpdateQiShiAnimation()
    {
        
        
        AI_Animator.fireEvents = true;

        // 技能动画由 AIPet 直接播放，这里不打断。
        if (AI_StatusValue == 4 || AI_StatusValue == 5)
        {
            return;
        }

        // 清除角色控制器中的动作参数。
        AI_Animator.SetBool("Idle", false);
        AI_Animator.SetBool("Run", false);
        AI_Animator.SetBool("CreateWalk", false);
        AI_Animator.SetBool("CreateIdle", false);
        AI_Animator.SetBool("Act_1", false);
        AI_Animator.SetBool("Act_2", false);
        AI_Animator.SetBool("CriAct", false);
        AI_Animator.SetBool("Skill_1", false);
        AI_Animator.SetBool("Skill_2", false);
        AI_Animator.SetBool("Skill_3", false);

        switch (AI_StatusValue)
        {
            case 0: // 待机
                AI_Animator.SetBool("Idle", true);
                break;

            case 1: // 走动
                AI_Animator.SetBool("CreateWalk", true);
                break;

            case 2: // 跑动
                AI_Animator.SetBool("Run", true);
                break;

            case 3: // 普通攻击
                AI_Animator.SetBool("Act_1", true);
                AI_Animator.Play("Act_1", 0, 0f);
                break;

            case 6: // 死亡
                AI_Animator.Play("Death", 0, 0f);
                break;
        }
    }
    
    // 接收现有角色动画的攻击事件，转交给宠物结算伤害。
    public void RoseAct()
    {
        if (AI_Animator == null ||
            AI_Animator.runtimeAnimatorController == null ||
            AI_Animator.runtimeAnimatorController.name != "Rose_QiShi")
        {
            return;
        }

        // 仅普通攻击结算普攻伤害，避免技能动作重复触发。
        if (!AI_Animator.GetCurrentAnimatorStateInfo(0).IsName("Act_1"))
        {
            return;
        }

        AIPet pet = GetComponent<AIPet>();
        if (pet != null && pet.AI_Target != null)
        {
            pet.AIAct();
        }
    }

// 接收角色动画自带的移动控制事件。
// AI 移动由 AIPet 管理，此处无需处理。
    public void RoseActStopMove_Start()
    {
    }

    public void RoseActStopMove_End()
    {
        if (AI_Animator == null ||
            AI_Animator.runtimeAnimatorController == null ||
            AI_Animator.runtimeAnimatorController.name != "Rose_QiShi")
        {
            return;
        }

        // 收到普攻结束事件后再恢复待机。
        if (AI_StatusValue == 3 &&
            AI_Animator.GetCurrentAnimatorStateInfo(0).IsName("Act_1"))
        {
            AI_StatusValue = 0;
            IfUpdateStatus = true;
        }
    }
}
