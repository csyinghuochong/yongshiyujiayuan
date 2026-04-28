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
}
