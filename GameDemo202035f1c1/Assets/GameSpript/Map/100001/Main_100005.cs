using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class Main_100005 : MonoBehaviour {

    //创建怪物的列表
    public GameObject[] MonsterListSet;


    //创建怪物
    public Transform monsterCreatePosition;
    public bool CreateBossStatus;
    private ObscuredFloat CreateBossTime;
    private ObscuredFloat CreateBossTimeSum;

    //创建技能
    public GameObject CreateSkillMonster;
    private ObscuredFloat CreateSkillMonsterTime;
    private ObscuredFloat CreateSkillMonsterTimeSum;
    public bool CreateSkillMonsterStatus;
    public GameObject CreateMonsterEffectObj;

    private ObscuredInt roseLv;


    //时间
    private ObscuredFloat MapTime;
    private ObscuredFloat MapTimeSum;
    private bool MapExitStatus;     //地图退出状态
    private ObscuredFloat OneTimeSum;       //1秒执行1次

    //UI类
    public GameObject Obj_FuBen_ShangHaiShow;
    private GameObject FuBen_ShangHaiShowObj;


    // Use this for initialization
    void Start () {

        roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        CreateSkillMonster.SetActive(false);


        //设定地图时间
        //MapTime = 360;
        FuBen_ShangHaiShowObj = (GameObject)Instantiate(Obj_FuBen_ShangHaiShow);
        FuBen_ShangHaiShowObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        FuBen_ShangHaiShowObj.transform.localPosition = new Vector3(0, 0, 0);
        FuBen_ShangHaiShowObj.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
        FuBen_ShangHaiShowObj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        FuBen_ShangHaiShowObj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        FuBen_ShangHaiShowObj.transform.localScale = new Vector3(1, 1, 1);
        FuBen_ShangHaiShowObj.GetComponent<UI_FuBen_ShangHaiShow>().Obj_Par = this.gameObject;


        //创建追踪怪物的时间
        CreateBossTime = 10.0f;


        //初始化,开启伤害统计状态
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHai_Status = true;

        //隐藏任务栏
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseTask.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {


        //创建技能怪物
        if (!CreateSkillMonsterStatus) {
            CreateSkillMonsterTimeSum = CreateSkillMonsterTimeSum + Time.deltaTime;

            FuBen_ShangHaiShowObj.GetComponent<UI_FuBen_ShangHaiShow>().Obj_MapTime.GetComponent<Text>().text = "战斗马上开启:" + (int)(CreateBossTime- CreateSkillMonsterTimeSum) + "秒";

            if (CreateSkillMonsterTimeSum >= CreateBossTime)
            {
                CreateSkillMonsterTimeSum = 0;
                CreateSkillMonsterStatus = true;
                CreateSkillMonster.SetActive(true);
                if (FuBen_ShangHaiShowObj != null) {
                    FuBen_ShangHaiShowObj.GetComponent<UI_FuBen_ShangHaiShow>().FightTime = 0;
                    FuBen_ShangHaiShowObj.GetComponent<UI_FuBen_ShangHaiShow>().FightStatus = true;
                    Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Pet = 0;
                    Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Rose = 0;

                    //播放特效
                    //GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill/" + "Eff_Skill_ZhaoHuan_4", typeof(GameObject));
                    GameObject SkillObject_p = (GameObject)Instantiate(CreateMonsterEffectObj);

                    //SkillObject_p.transform.position = monsterObj.transform.position;
                    SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
                    SkillObject_p.transform.SetParent(CreateSkillMonster.transform);
                    SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);

                    Destroy(SkillObject_p, 2);

                    if (CreateSkillMonster.GetComponent<AI_1>().UI_Hp != null)
                    {
                        CreateSkillMonster.GetComponent<AI_1>().UI_Hp.SetActive(true);
                        CreateSkillMonster.GetComponent<AI_Property>().AI_Hp = CreateSkillMonster.GetComponent<AI_Property>().AI_HpMax;
                    }
                }
            }
        }

        //显示地图时间
        MapTimeSum = MapTimeSum + Time.deltaTime;

        //地图超过时间回到主城
        /*
        if (MapTimeSum >= MapTime)
        {
            //删除怪物
            for (int i = 0; i < monsterCreatePosition.transform.childCount; i++)
            {
                GameObject go = monsterCreatePosition.transform.GetChild(i).gameObject;
                MonoBehaviour.Destroy(go);
            }

            //地图时间结束,将玩家返回地图
            if (!MapExitStatus) {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ReturnBuilding.GetComponent<UI_StartGame>().Btn_RetuenBuilding();
            }

            MapExitStatus = true;
        }
        */
    }

    //注销时调用
    private void OnDestroy()
    {
        //初始化,开启伤害统计状态
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHai_Status = false;
        //关闭统计状态
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Rose = 0;
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Pet = 0;

        Destroy(FuBen_ShangHaiShowObj);
    }

    //初始化战斗
    public void InitFuBen() {

        CreateSkillMonster.SetActive(false);
        CreateSkillMonsterTimeSum = 0;
        CreateBossTime = 5;
        CreateSkillMonsterStatus = false;
        Debug.Log("初始化了战斗...");

        //不计入伤害
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHai_Status = true;
        //统计状态清空
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Rose = 0;
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHaiValue_Pet = 0;
    }

    //隐藏副本
    public void EndFuBen()
    {
        CreateSkillMonster.SetActive(false);
        if (CreateSkillMonster.GetComponent<AI_1>().UI_Hp != null) {
            CreateSkillMonster.GetComponent<AI_1>().UI_Hp.SetActive(false);
        }


        //清空宠物
        for (int i = 0; i < Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj.Length; i++) {
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj[i] != null) {
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj[i].GetComponent<AIPet>().AI_Target = null;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj[i].GetComponent<AIPet>().ai_IfReturn = true;
            }
        }

        //设置玩家状态
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget != null) {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget = null;
        }
        

        for (int i = 0; i < Game_PublicClassVar.Get_game_PositionVar.Obj_RoseCreatePetSet.transform.childCount; i++)
        {
            GameObject go = Game_PublicClassVar.Get_game_PositionVar.Obj_RoseCreatePetSet.transform.GetChild(i).gameObject;

            if (go != null)
            {
                if (go.GetComponent<AIPet>() != null) {
                    go.GetComponent<AIPet>().AI_Target = null;
                    go.GetComponent<AIPet>().ai_IfReturn = true;
                }

            }
        }

        //不计入伤害
        Game_PublicClassVar.Get_game_PositionVar.FuBen_ShangHai_Status = false;

    }


}
