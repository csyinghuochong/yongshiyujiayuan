using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Status : MonoBehaviour {

    public int PlayerStatus;                                // 0:停止    1:移动
    public bool UpdateDataStatus;                           //数据更新状态
    public Vector3 MovePosition;                            //移动的坐标点ee
    public UnityEngine.AI.NavMeshAgent Player_Nav;
    public Animator PlayerAnimator;                         //角色当前动画状态机
    public GameObject Obj_RunEffect;                        //角色跑动特效
    public GameObject PlayerPetObj;                         //宠物Obj

    //自身UI控件
    public GameObject Obj_HpNameUI;
    public GameObject Obj_PlayerDataList;
    private GameObject playerListObj;
    public string Player_ZhangHaoID;

    public List<MapThread_PlayerSkill> mapThread_PlayerSkill;


    //坐骑相关
    public string ZuoQiID;
    public bool ZuoQiStatus;                            //上下马状态
    public string OccType;
    public GameObject Obj_RoseShowModel;
    public GameObject Obj_ZuoQiShowModel;
    public GameObject Obj_ZuoQiShowPosition;
    public GameObject Obj_ZuoQiShowSet;
    private Vector3 RoseHeadZuoQiPosition;
    // Use this for initialization
    void Start () {

        PlayerAnimator = this.GetComponent<Animator>();
        //设置穿透
        Player_Nav.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

        //显示模型

    }
	
	// Update is called once per frame
	void Update () {

        if (UpdateDataStatus) {
            //Debug.Log("开启移动");
            UpdateDataStatus = false;
            /*
            //获取自身坐标点
            Vector3 vec3_Self = this.gameObject.transform.position;
            //获取目标坐标点
            MovePosition = MovePosition / 100;
            Player_Nav.SetDestination(MovePosition);
            */

            //测试技能
            //UseSkill("60010100", this.gameObject.transform.position, "3", this.gameObject);
            //UseSkill("60010200", this.gameObject.transform.position, "3", this.gameObject);
            
        }

        //时刻显示自身Hp的血条位置
        if (Obj_HpNameUI != null)
        {

            //Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(this.GetComponent<Player_Bone>().Bone_Head.transform.position);
            UpdatePosition();
            Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(RoseHeadZuoQiPosition);
            
            Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);

            //血条位置修正（根据分辨率的变化而变化）
            Obj_HpNameUI.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
        }

        float dis = Vector3.Distance(Player_Nav.destination, Player_Nav.nextPosition);
        bool ifShowZuoQi = false;
        if (ZuoQiStatus)
        {
            //播放坐骑动作
            PlayerAnimator.Play("ZuoQi");

            //非坐骑状态
            if (dis <= 0.05f)
            {

                //表示停止
                if (Obj_ZuoQiShowModel != null)
                {
                    Obj_RoseShowModel.transform.position = Obj_ZuoQiShowPosition.transform.position;
                    Obj_RoseShowModel.transform.rotation = Obj_ZuoQiShowPosition.transform.rotation;
                    Obj_ZuoQiShowModel.GetComponent<ZuoQiShowModel>().ZuoQiStatus = "1";
                    Obj_ZuoQiShowModel.GetComponent<ZuoQiShowModel>().ZuoQiPlay();
                    ifShowZuoQi = true;
                    if (Obj_RunEffect != null)
                    {
                        Obj_RunEffect.GetComponent<ParticleSystem>().Pause(false);
                        Obj_RunEffect.GetComponent<ParticleSystem>().Stop();
                    }
                }
            }
            else
            {
                //表示移动
                if (Obj_ZuoQiShowModel != null)
                {
                    Obj_RoseShowModel.transform.position = Obj_ZuoQiShowPosition.transform.position;
                    Obj_RoseShowModel.transform.rotation = Obj_ZuoQiShowPosition.transform.rotation;
                    Obj_ZuoQiShowModel.GetComponent<ZuoQiShowModel>().ZuoQiStatus = "2";
                    Obj_ZuoQiShowModel.GetComponent<ZuoQiShowModel>().ZuoQiPlay();
                    ifShowZuoQi = true;

                    if (Obj_RunEffect != null)
                    {
                        Obj_RunEffect.GetComponent<ParticleSystem>().Pause(true);
                        Obj_RunEffect.GetComponent<ParticleSystem>().Play();
                    }
                }
            }

        }
        else {

            //非坐骑状态
            if (dis <= 0.05f)
            {

                //表示停止
                PlayerAnimator.SetBool("Idle", true);
                PlayerAnimator.SetBool("Run", false);
                //Obj_RunEffect.GetComponent<ParticleSystem>().Stop();
            }
            else
            {
                PlayerAnimator.SetBool("Idle", false);
                PlayerAnimator.SetBool("Run", true);
                //Obj_RunEffect.GetComponent<ParticleSystem>().Pause(true);
                //Obj_RunEffect.GetComponent<ParticleSystem>().Play();
            }
        }


        if (Obj_ZuoQiShowModel != null)
        {
            Obj_ZuoQiShowModel.SetActive(ifShowZuoQi);
            //坐骑小时恢复角色坐标和朝向
            if (ifShowZuoQi == false)
            {
                Obj_RoseShowModel.transform.localPosition = Vector3.zero;
                Obj_RoseShowModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
                ZuoQiStatus = false;
            }
        }
    }

    //销毁时调用
    public void OnDestroy()
    {

        //注销UI
        if (Obj_HpNameUI != null)
        {
            Destroy(Obj_HpNameUI);
        }
    }

    //展示坐骑
    public void PlayerShowZuoQi() {

        //展示坐骑
        //ZuoQiID = "10001";      //测试功能后面需屏蔽
        if (ZuoQiID != "" && ZuoQiID != "0" && ZuoQiID != null)
        {
            Game_PublicClassVar.Get_function_Pasture.ZuoQi_PlayerShow(ZuoQiID, this.gameObject);
        }

    }

    //其他玩家移动
    public void PlayerMove() {
        //获取自身坐标点
        Vector3 vec3_Self = this.gameObject.transform.position;
        //获取目标坐标点
        MovePosition = MovePosition / 100;
        Player_Nav.SetDestination(MovePosition);
        //Debug.Log("接受移动时间444444：" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
    }

    public void RoseRunSource()
    {
        //音效（去掉方法会报错）
        //Game_PublicClassVar.Get_function_UI.PlaySource("20013", "2");
    }

    //玩家使用技能
    public void PlayerUseSkill()
    {
        foreach (MapThread_PlayerSkill nowMapThread_PlayerSkill in mapThread_PlayerSkill) {

            //获取技能ID
            string nowSkillID = nowMapThread_PlayerSkill.Skill_ID;
            Vector3 skillTargetVec3 = new Vector3(nowMapThread_PlayerSkill.Skill_X, nowMapThread_PlayerSkill.Skill_Y, nowMapThread_PlayerSkill.Skill_Z);
            UseSkill(nowSkillID, skillTargetVec3,"3",this.gameObject);
        }

    }


    //实例化技能
    public void UseSkill(string skillID, Vector3 SkillTargetPoint, string SkillUseType, GameObject UseSkillObj)
    {

        //设置实例化技能
        string skillObjName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectName", "ID", skillID, "Skill_Template");
        GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill_Obj/" + skillObjName, typeof(GameObject));
        GameObject SkillObject_p = (GameObject)MonoBehaviour.Instantiate(SkillObj);

        //设定父节点
        string skillParent = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParent", "ID", skillID, "Skill_Template");
        switch (skillParent)
        {
            case "0":
                //获取存放的点
                string positionName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID, "Skill_Template");
                SkillObject_p.transform.parent = UseSkillObj.GetComponent<Player_Bone>().BoneSet.transform.Find(positionName).transform;
                SkillObject_p.transform.localPosition = Vector3.zero;
                //Debug.Log("实例化特效位置");
                break;

            case "1":
                //SkillObject_p.transform.parent = this.transform;
                SkillObject_p.transform.localPosition = UseSkillObj.transform.position;
                //Debug.Log("实例化特效位置skillObjName = " + skillObjName + "SkillObject_p = " + SkillObject_p.transform.position);
                break;
        }

        //设置技能位置
        SkillObject_p.transform.localRotation = UseSkillObj.transform.rotation;
        //SkillObject_p.transform.rotation = Quaternion.Euler(Vector3.zero);
        SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
        //传递技能对应的值
        SkillObjBase skillStatus = SkillObject_p.transform.GetComponent<SkillObjBase>();
        skillStatus.SkillTargetPoint = SkillTargetPoint;        //技能目标点
        skillStatus.SkillID = skillID;
        //skillStatus.DamgeValue_Fixed = SkillDamge;              //技能伤害

        skillStatus.SkillOpen = true;   //开启技能，需要在设置完值后开启技能

        switch (SkillUseType)
        {
            //玩家施放的技能
            case "1":
                //设置释放技能的目标
                skillStatus.SkillTargetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
                break;
            //怪物释放的技能(以后怪物技能补充)
            case "2":

                break;
        }
    }

    //点击玩家
    public void ClickPlayer() {

        playerListObj = (GameObject)Instantiate(Obj_PlayerDataList);
        playerListObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        playerListObj.transform.localScale = new Vector3(1, 1, 1);
        playerListObj.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        playerListObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
        playerListObj.GetComponent<Player_ShowDataListSet>().Player_ZhangHaoID = Player_ZhangHaoID;

    }

    //更新坐标位置
    public void UpdatePosition()
    {

        if (ZuoQiStatus)
        {
            RoseHeadZuoQiPosition = new Vector3(this.GetComponent<Player_Bone>().Bone_Head.transform.position.x, this.GetComponent<Player_Bone>().Bone_Head.transform.position.y + 1.0f, this.GetComponent<Player_Bone>().Bone_Head.transform.position.z);
        }
        else
        {
            RoseHeadZuoQiPosition = new Vector3(this.GetComponent<Player_Bone>().Bone_Head.transform.position.x, this.GetComponent<Player_Bone>().Bone_Head.transform.position.y, this.GetComponent<Player_Bone>().Bone_Head.transform.position.z);
        }

    }
}
