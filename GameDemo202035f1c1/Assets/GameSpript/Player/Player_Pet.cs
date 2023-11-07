using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Pet : MonoBehaviour {

    public GameObject Obj_Player;
    public string PetName;
    public string PetLv;
    public AI_Status aiStatus;
    public UnityEngine.AI.NavMeshAgent ai_NavMesh;      //AI自动寻路控制器
    public bool moveStatus;
    //public GameObject Obj_PetUI;
    public GameObject Obj_PetNameUI;
    private GameObject petNameUIObj;
    public Transform Ai_HpShowPosition;
    public SkinnedMeshRenderer ModelMesh;               //AI材质
    public bool ifCanMove;                              //是否可以移动 有时候在寻路上报错就将其设置为不可移动

    // Use this for initialization
    void Start () {
        aiStatus = this.GetComponent<AI_Status>();
        ai_NavMesh = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        ifCanMove = true;
        //设置怪物穿透
        ai_NavMesh.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
        ai_NavMesh.speed = 4;
        this.gameObject.transform.LookAt(Obj_Player.transform);

        //创建血条
        Vector3 hpPosi = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y+3.0f, this.gameObject.transform.position.z);
        hpPosi = Ai_HpShowPosition.transform.position;
        Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(hpPosi);
        Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);

        //实例化UI
        petNameUIObj = (GameObject)Instantiate(Obj_PetNameUI);

        //显示UI,并对其相应的属性修正
        petNameUIObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet.transform);
        petNameUIObj.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
        petNameUIObj.transform.localScale = new Vector3(1f, 1f, 1f);
        petNameUIObj.GetComponent<UI_PlayerPetHp>().Obj_Name.GetComponent<Text>().text ="Lv." + PetLv + " " +PetName;

    }
	
	// Update is called once per frame
	void Update () {

        if (ai_NavMesh.isOnNavMesh == false)
        {
            ifCanMove = false;
            //Debug.Log("报错！！！isOnNavMesh = " + ai_NavMesh.isOnNavMesh);
            this.gameObject.SetActive(false);
        }


        if (ifCanMove) {
            //判定跟随的目标
            if (Obj_Player != null)
            {

                if (Obj_Player.GetComponent<Player_Bone>() != null)
                {

                    float dis = Vector3.Distance(Obj_Player.transform.position, this.gameObject.transform.position);
                    if (dis >= 2)
                    {
                        //开始移动
                        try
                        {
                            ai_NavMesh.SetDestination(Obj_Player.GetComponent<Player_Bone>().PetPositionSet_Posi_1.transform.position);
                        }
                        catch
                        {
                            Debug.Log("宠物移动数据错误,销毁数据！");
                            ifCanMove = false;
                        }

                        //Debug.Log("Obj_Player.GetComponent<Player_Bone>().PetPositionSet_Posi_1.transform.position = " + Obj_Player.GetComponent<Player_Bone>().PetPositionSet_Posi_1.transform.position);
                        //播放动作
                        aiStatus.AI_StatusValue = 1;
                        aiStatus.IfUpdateStatus = true;

                        moveStatus = true;
                    }
                }
            }
            else
            {
                //如果跟随的玩家消失,那么宠物也消失
                Destroy(this.gameObject);
                Destroy(petNameUIObj);
            }

            //移动状态
            if (moveStatus)
            {
                if (Obj_Player != null)
                {
                    if (Obj_Player.GetComponent<Player_Bone>() != null)
                    {
                        float dis = Vector3.Distance(Obj_Player.GetComponent<Player_Bone>().PetPositionSet_Posi_1.transform.position, this.gameObject.transform.position);
                        if (dis <= 1.0f)
                        {
                            //Debug.Log("停止移动！！！！！！！！！！！！！！！！！！！");
                            moveStatus = false;

                            //开始移动
                            try
                            {
                                ai_NavMesh.SetDestination(this.gameObject.transform.position);  //停止移动
                                                                                                //播放动作
                                aiStatus.AI_StatusValue = 0;
                                aiStatus.IfUpdateStatus = true;

                            }
                            catch
                            {
                                Debug.Log("宠物移动数据错误,销毁数据！");
                                ifCanMove = false;
                            }
                        }
                    }
                }
            }
        }
 

        //血条移动
        if (petNameUIObj != null) {
            Vector3 hpPosi = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 3.0f, this.gameObject.transform.position.z);
            hpPosi = Ai_HpShowPosition.transform.position;
            Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(hpPosi);
            Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);
            petNameUIObj.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
        }
	}

    void OnDestroy()
    {
        Destroy(petNameUIObj);
    }
}
