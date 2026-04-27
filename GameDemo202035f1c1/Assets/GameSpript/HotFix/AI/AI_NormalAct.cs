using UnityEngine;
using System.Collections;

public class AI_NormalAct : MonoBehaviour {

    private float actSpeed;             //攻击速度
    private float actSpped_Sum;          //攻击速度统计值
    public bool NextActStatus;          //下一次攻击状态,如果为true表示可以立即进行下一次攻击
	private int Damage;
	private GameObject ActTarger;
	private bool ActStatus;
	private Animator animator;

	void Start () {

        //设置攻击速度
        actSpeed = 2.0f;
        NextActStatus = false;

		animator = GetComponent<Animator>();
		ActStatus = false;

	}
	
	// Update is called once per frame
	void Update () {

		//当与主角距离少于3时，开始触发普通攻击
		AI_1 ai = GetComponent<AI_1>();

        actSpped_Sum = actSpped_Sum + Time.deltaTime;

		if (ai.AI_Distance <= 3.5f) {

			AI_Property aiproperty = GetComponent<AI_Property>();

			if(aiproperty!=null){

                //Rose_Proprety roseproperty = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
				AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

                if (actSpped_Sum >= actSpeed)
                {

					ActStatus = true;

                    actSpped_Sum = 0.0f;
                    NextActStatus = false;

				}

				if(ActStatus){

                    //设置AI为攻击状态
                    this.gameObject.GetComponent<AI_Status>().AI_StatusValue = 3;
                    this.gameObject.GetComponent<AI_Status>().IfUpdateStatus = true;
                    /****
					Fight_Formult formult = new Fight_Formult();
					Damage = formult.FightHurt(aiproperty.AI_Act,roseproperty.Rose_Def);

                    Game_PublicClassVar.Get_game_PositionVar.GameObject_Rose.GetComponent<Rose_Status>().RoseIfHit = true;
                    Game_PublicClassVar.Get_fight_Formult.MonsterActRose("60020001",this.gameObject);
					*/
                    ActStatus = false;
				
				}
		    }

		}

        if (actSpped_Sum >= actSpeed) {
            NextActStatus = true;
        }

	}
}
