using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class PastureAI : MonoBehaviour {

    public ObscuredString RosePastureID;        
    private ObscuredString PastureID;            //
    private ObscuredString PastureTime;          //存在时间
    private ObscuredString AI_Status;            //0表示巡逻
    public int AI_Animator_Status;   //0:待机  1：移动  2：跑动 3：吃草  6:死亡
    public bool ai_FindNextPatrol;
    private bool ai_IfDeath;
    private float ai_PatrolRange;
    public Vector3 walkPosition;
    public Vector3 ai_StarPosition;
    public UnityEngine.AI.NavMeshAgent ai_NavMesh;  //AI自动寻路控制器
    private float ai_WalkSpeed;
    private Vector3 aiPosi_Last;
    public float ai_PatrolRestTime;
    private float ai_PatrolGuardTime;
    private Animator AI_Animator;
    private int UpStatus;            //成长中的状态,表示在成长中的那个时期  0:幼崽期  1：成长期 2：成熟期 3：衰老期 4：死亡

    private float AI_Distance;
    private bool AI_Hp_Status;
    private GameObject UI_Hp;

    private Transform Ai_HpShowPosition;

    private GameObject pastureListObj;

    //宠物信息
    private string PastureName;

    // Use this for initialization
    void Start () {

        AI_Animator = GetComponent<Animator>();
        AI_Status = "0";
        ai_WalkSpeed = 0.8f;
        ai_NavMesh = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        ai_NavMesh.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
        ai_StarPosition = this.gameObject.transform.position;
        ai_PatrolRange = 2;
        Ai_HpShowPosition = this.transform.Find("NamePosi");

        //初始化吃草动作
        AI_Animator_Status = 3;

        //读取数据
        PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureID", "ID", RosePastureID, "RosePasture");
        PastureName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", PastureID, "Pasture_Template");        //显示姓名
        PastureTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Time", "ID", RosePastureID, "RosePasture");
        string PasturUpLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PasturUpLv", "ID", RosePastureID, "RosePasture");
        PastureName = PastureName + "  " + Game_PublicClassVar.function_Pasture.GetUpLvStatusName(PasturUpLv);


        //根据阶段进行缩放模型大小
        float sizeValue = 1;
        if (PasturUpLv == "0") {
            sizeValue = 0.6f;
        }
        if (PasturUpLv == "1")
        {
            sizeValue = 0.8f;
        }

        this.gameObject.transform.localScale = new Vector3(sizeValue, sizeValue, sizeValue);

    }
	
	// Update is called once per frame
	void Update () {
        //根据不同的AI状态触发不同的AI机制
        switch (AI_Status)
        {

            //巡逻
            case "0":

                //获取一个巡逻目标点
                if (ai_FindNextPatrol)
                {
                    //随机一个范围
                    float random_x = (Random.value - 0.5f) * ai_PatrolRange * 3;
                    float random_z = (Random.value - 0.5f) * ai_PatrolRange * 3;
                    walkPosition = new Vector3(ai_StarPosition.x + random_x, ai_StarPosition.y, ai_StarPosition.z + random_z);

                    //判断目标点离起始点距离过远则回原点
                    float dis = Vector3.Distance(Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().PastureInitVec3, walkPosition);
                    if (dis >= 15) {
                        walkPosition = Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().PastureInitVec3;
                    }

                    //注视目标
                    transform.LookAt(new Vector3(walkPosition.x, transform.position.y, walkPosition.z));
                    //移动目标区域
                    ai_NavMesh.speed = ai_WalkSpeed;

                    //设置自身离寻路最近的坐标点,要不离地面检测区太远会报错

                    UnityEngine.AI.NavMeshHit hit;
                    UnityEngine.AI.NavMesh.SamplePosition(this.gameObject.transform.position, out hit, 10.0f, 1);
                    try
                    {
                        ai_NavMesh.Warp(hit.position);
                        walkPosition.y = hit.position.y;
                        ai_NavMesh.SetDestination(walkPosition);
                    }
                    catch
                    {
                        Debug.Log("移动报错！");
                    }

                    ai_FindNextPatrol = false;
                    //设置AI状态&播放对应动作
                    //ai_status.IfUpdateStatus = true;
                    //ai_status.AI_StatusValue = 1;
                    aiPosi_Last = this.gameObject.transform.position;

                    AI_Animator_Status = 1;

                }

                //获取自己与目标点的距离(抛弃Y轴的高低差)
                Vector3 dis_1 = new Vector3(walkPosition.x, 0, walkPosition.z);
                Vector3 dis_2 = new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z);
                float distance = Vector3.Distance(dis_1, dis_2);

                //到达目标点,休息3秒
                if (distance <= 0.25f)
                {
                    //休息时间累积
                    ai_PatrolRestTime = ai_PatrolRestTime + Time.deltaTime;
                    //设置AI状态&播放对应动作
                    //ai_status.IfUpdateStatus = true;
                    //ai_status.AI_StatusValue = 0;
                    //ai_NavMesh.speed = 0;
                    ai_PatrolGuardTime = 0.0f;

                    if (ai_PatrolRestTime >= 5.0f)
                    {
                        //清空数据
                        ai_FindNextPatrol = true;
                        ai_PatrolRestTime = 0.0f;
                    }

                    if (ai_PatrolRestTime >= 1.0f)
                    {
                        AI_Animator_Status = 3;
                    }

                        
                }
                else
                {
                    //移动保护时间
                    ai_PatrolGuardTime = ai_PatrolGuardTime + Time.deltaTime;
                }

                //每次移动超过10秒再次寻找下一个巡逻目标点
                if (ai_PatrolGuardTime >= 10.0f)
                {
                    ai_FindNextPatrol = true;
                    ai_PatrolGuardTime = 0.0f;
                }

                //防止原地踏步,强制休息 0.2秒内还是在原地 表示寻路地点无法到达
                if (ai_PatrolGuardTime >= 0.2f)
                {
                    if (aiPosi_Last == this.gameObject.transform.position)
                    {
                        //Debug.Log("原地踏步！");
                        ai_FindNextPatrol = true;
                        ai_PatrolGuardTime = 0.0f;
                    }
                }

                break;
        }


        //人走过去,动物会跑开？直到撞墙？


        //显示UI
        ShowNameUI();

        //播放对应动作
        PlayDongZuo();
    }

    void OnDestroy()
    {
        Destroy(UI_Hp);
        PastureSaveData();
    }


    //创建名称UI
    private void ShowNameUI() {

        AI_Distance =  Vector3.Distance(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position, transform.position);

        //当角色离AI 20米内,AI显示血条
        if (AI_Distance < 20)
        {
            if (!AI_Hp_Status)
            {
                //显示UI后，表示为true;
                AI_Hp_Status = true;


                Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(Ai_HpShowPosition.position);
                Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);

                //实例化UI
                UI_Hp = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_PastrueName);

                //显示UI,并对其相应的属性修正
                UI_Hp.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet.transform);
                UI_Hp.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
                UI_Hp.transform.localScale = new Vector3(1f, 1f, 1f);
                UI_Hp.GetComponent<UI_AIHp>().Obj_AI = this.gameObject;

                //取得界面控件
                GameObject UI_Name = UI_Hp.transform.Find("Lal_Name").gameObject;

                //显示当前UI名称
                Text UIname = UI_Name.GetComponent<Text>();
                UIname.text = PastureName;
            }
            else
            {
                //后台进入前台重新载入一次时间
                if (Game_PublicClassVar.Get_wwwSet.BackEnterGameOnly)
                {
                    //判定怪物是否死亡
                    if (ai_IfDeath)
                    {
                        
                    }
                }
            }

            /*   以后作为成长值显示
            if (UI_Hp != null)
            {
                //UI显示当前血量
                Image HpValue = UI_Hp.GetComponent<UI_AIHp>().Obj_aiImgValue.GetComponent<Image>();
                HpValue.fillAmount = (float)ai_property.AI_Hp / AI_Hp_Max;
            }
            */
        }
        else
        {
            //删除怪物名称
            Destroy(UI_Hp);
            AI_Hp_Status = false;
        }

        //出现血条后,不断修正血条位置
        if (AI_Hp_Status)
        {

            if (UI_Hp != null)
            {

                Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(Ai_HpShowPosition.position);
                Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);

                //血条位置修正（根据分辨率的变化而变化）
                UI_Hp.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
            }
        }

    }


    //存储数据
    public void PastureSaveData() {

        string position = this.transform.localPosition.x.ToString("F2")  + ","+ this.transform.localPosition.y.ToString("F2") + ","+this.transform.localPosition.z.ToString("F2");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Position", position,"ID", RosePastureID, "RosePasture");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePasture");

    }


    public void PlayDongZuo() {

        AI_Animator.SetBool("idle", false);
        AI_Animator.SetBool("eat", false);
        AI_Animator.SetBool("walk", false);
        AI_Animator.SetBool("run", false);

        switch (AI_Animator_Status)
        {

            //待机
            case 0:
                AI_Animator.SetBool("idle", true);
                break;

            //走动
            case 1:
                AI_Animator.SetBool("walk", true);
                break;

            //跑动
            case 2:
                AI_Animator.SetBool("run", true);
                break;

            //吃吃吃
            case 3:
                AI_Animator.SetBool("eat", true);
                break;

            //死亡状态
            case 6:
                AI_Animator.Play("Death");
                break;

        }
    }

    //点击牧场动物
    public void ClickPlayer()
    {
        if (pastureListObj != null) {
            Destroy(pastureListObj);
        }

        Debug.Log("我被戳了一下!");
        pastureListObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_PastrueDataShow);
        //pastureListObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet.transform);
        pastureListObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        pastureListObj.transform.localScale = new Vector3(1, 1, 1);

        pastureListObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        //pastureListObj.GetComponent<Player_ShowDataListSet>().Player_ZhangHaoID = Player_ZhangHaoID;


        //设置位置
        Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(Ai_HpShowPosition.position);
        Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);
        //Debug.Log("Hp_show_position = " + Hp_show_position);
        pastureListObj.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(Hp_show_position.x - 1366/2, Hp_show_position.y - 768/2, 0);
        //Debug.Log("Hp_show_position = " + Hp_show_position);

        pastureListObj.GetComponent<UI_PastureShowDataListSet>().RosePastureID = RosePastureID;
        pastureListObj.GetComponent<UI_PastureShowDataListSet>().PastureID = PastureID;
        pastureListObj.GetComponent<UI_PastureShowDataListSet>().Obj_Pasture = this.gameObject;
        //pastureListObj.GetComponent<UI_PastureShowDataListSet>().PastureUpLv = UpStatus;
        pastureListObj.GetComponent<UI_PastureShowDataListSet>().Init();

    }
}
