using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Weijing;

public class DoorWay : MonoBehaviour {

    public string DoorWayID;
    private AsyncOperation mAsyn;
    private bool enterGameStatus;
    public GameObject Obj_Loading;
    public GameObject Obj_UIDoorWayNamePosition;        //传送门名称显示位置
    public GameObject Obj_UIDoorWayName;        //传送门名称Obj
    private GameObject objUIDoorWatName;        //实例化的传送门名称Obj
    private GameObject obj_loading;
    private bool loadStatus;
    private float loadingSumTime;       //加载延迟显示时间
    private float loadingSumTimeSum;
    private Vector2 vec3_NpcName;
    private string NextMapName;
    private string NextMapPosition;
    private bool doorWayNameUpdateStatus;       //保证名字显示只执行一次
    private string doorWayName;


    //此加载为假的,默认加载1秒钟，之后等待进入
	// Use this for initialization
	void Start () {

        
        loadingSumTime = 1.0f;
        //NextMapName = "555";
        NextMapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", DoorWayID, "SceneTransfer_Template");
        //NextMapPosition = "RosePosition_1";
        NextMapPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapTransferName", "ID", DoorWayID, "SceneTransfer_Template");
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("传送");
        doorWayName = langStr + ":" + Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TransferName", "ID", DoorWayID, "SceneTransfer_Template");
        doorWayNameUpdateStatus = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (enterGameStatus)
        {
            loadingSumTimeSum = loadingSumTimeSum + Time.deltaTime;
            if (loadingSumTimeSum > 1.0f) {
                loadingSumTimeSum = 1;
            }
            obj_loading.GetComponent<UI_Loading>().LoadingValue = loadingSumTimeSum;            //显示加载条
            if (loadingSumTimeSum >= loadingSumTime) {
                if (mAsyn.progress >= 0.9f)
                {
                    //清理Npc名称UI
                    for (int i = 0; i < Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcNameSet.transform.childCount; i++)
                    {
                        GameObject go = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcNameSet.transform.GetChild(i).gameObject;
                        Destroy(go);
                    }

                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.SetActive(false);
                    GameTimer timer = new GameTimer(0.1f, 1, delegate ()
                    {
                        Debug.Log("DoorWayID = " + DoorWayID);
                        float transfer_X = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapTransfer_X", "ID", DoorWayID, "SceneTransfer_Template"));
                        float transfer_Y = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapTransfer_Y", "ID", DoorWayID, "SceneTransfer_Template"));
                        float transfer_Z = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapTransfer_Z", "ID", DoorWayID, "SceneTransfer_Template"));
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position = new Vector3(transfer_X, transfer_Y, transfer_Z);
                        Debug.Log("地图加载完毕" + "obj_Position.transform.position = " + new Vector3(transfer_X, transfer_Y, transfer_Z));
                    });
                    timer.Start();

                    Destroy(obj_loading, 1);     //延迟1秒注销界面,防止穿帮
                    enterGameStatus = false;
                    Game_PublicClassVar.Get_game_PositionVar.EnterScenceStatus = true;
                    mAsyn.allowSceneActivation = true;
                    Debug.Log("*********设置角色坐标为：" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                    Debug.Log("mAsyn.isDone = " + mAsyn.isDone);
                }
            }
        }



        //显示传送门名称
        //判定与角色相距20米进行名称显示
        float distance = Vector3.Distance(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position, this.gameObject.transform.position);
        if (distance <= 20.0f)
        {
            //修正物体在屏幕中的位置
            vec3_NpcName = Camera.main.WorldToViewportPoint(Obj_UIDoorWayNamePosition.transform.position);
            vec3_NpcName = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(vec3_NpcName);

            //第一次显示
            if (doorWayNameUpdateStatus == true)
            {
                doorWayNameUpdateStatus = false;
                //实例化UI
                objUIDoorWatName = (GameObject)MonoBehaviour.Instantiate(Obj_UIDoorWayName);
                objUIDoorWatName.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcNameSet.transform);
                objUIDoorWatName.transform.localPosition = new Vector3(vec3_NpcName.x, vec3_NpcName.y, 0);
                objUIDoorWatName.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                //显示Npc名称
                //string doorWayName = "前往矿洞";        //暂时，以后读取配置
                objUIDoorWatName.transform.Find("Lab_DoorWayName").transform.GetComponent<Text>().text = doorWayName;
            }
            //修正掉落物体的位置
            if (objUIDoorWatName != null) {
                objUIDoorWatName.transform.localPosition = new Vector3(vec3_NpcName.x, vec3_NpcName.y, 0);
            }
            
        }
	}

    public void EnterGame()
    {
        enterGameStatus = true;
        loadingSumTimeSum = 0;
        //加载Loding的UI界面
        obj_loading = (GameObject)Instantiate(Obj_Loading);
        obj_loading.transform.SetParent(GameObject.FindWithTag("UI_Set").transform);
        obj_loading.transform.localScale = new Vector3(1, 1, 1);
        obj_loading.transform.localPosition = new Vector3(0, 0, 0);
        loadStatus = true;
        StartCoroutine("Load");
        //注销传送门名称UI
        if (objUIDoorWatName != null)
        {
            Destroy(objUIDoorWatName);
        }
        //清空名字显示
        GameObject npcNameObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcNameSet;
        for (int i = 0; i < npcNameObj.transform.childCount; i++)
        {
            GameObject go = npcNameObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
        //清空血条显示
        GameObject aiHpObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet;
        for (int i = 0; i < aiHpObj.transform.childCount; i++)
        {
            GameObject go = aiHpObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
        //清空游戏声音
        GameObject gameSourceSet = Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceSet;
        for (int i = 0; i < gameSourceSet.transform.childCount; i++)
        {
            GameObject go = gameSourceSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }
        //清空掉落显示
        GameObject dropItemSet = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_DropItemSet;
        for (int i = 0; i < dropItemSet.transform.childCount; i++)
        {
            GameObject go = dropItemSet.transform.GetChild(i).gameObject;
            Destroy(go);
            Debug.Log("清空掉落");
        }
        //清空名称显示
        GameObject aiHpSet = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet;
        for (int i = 0; i < aiHpSet.transform.childCount; i++)
        {
            GameObject go = aiHpSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }
    }

    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    IEnumerator Load()
    {
        //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.SetActive(false);
        //获取角色当前进入地图
        Game_PublicClassVar.Get_game_PositionVar.EnterScencePositionName = NextMapPosition;
        mAsyn = Application.LoadLevelAsync(NextMapName);
        mAsyn.allowSceneActivation = false;
        yield return mAsyn;
    }

    //检测碰撞体
    void OnTriggerEnter(Collider collider)
    {

        if (Game_PublicClassVar.Get_wwwSet.MapEnterStatus) {
            Debug.LogError("当前正在加载地图... 进入地图失效!");
            return;
        }

        //Game_PublicClassVar.Get_wwwSet.MapEnterStatus = true;

        if (collider.gameObject.layer == 14)
        {

            //特殊处理
            if (DoorWayID == "100099") {
                //传送回主城
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ReturnBuilding.GetComponent<UI_StartGame>().Btn_RetuenBuilding();
                return;
            }

            //如果玩家有冲刺的技能就取消
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose != null) {
                if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>() != null) {
                    Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>());
                }
            }

            //获取当前角色等级
            int roseLv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
            //获取场景进入等级NextMapName
            int sceneEnterLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EnterLv", "ID", DoorWayID, "SceneTransfer_Template"));
            
            if (sceneEnterLv == null) {
                sceneEnterLv = 1;
            }

            if (roseLv >= sceneEnterLv)
            {
                //设置角色停止移动
                Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
                roseStatus.Move_Target_Position = roseStatus.gameObject.transform.position;

                //播放传送特效
                GameObject SkillObj = (GameObject)Resources.Load("Effect/Rose/" + "Rose_MoveScene", typeof(GameObject));
                GameObject SkillObject_p = (GameObject)Instantiate(SkillObj);
                //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Low

                GameObject effectObj =(GameObject)Instantiate((GameObject)Resources.Load("Effect/Skill/Rose_CriAct"));
                SkillObject_p.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Low.transform);
                SkillObject_p.transform.localPosition = new Vector3(0, 0.5f, 0);
                SkillObject_p.transform.localScale = new Vector3(1, 1, 1);

                //切换场景
                //EnterGame();

                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Status = false;
                string ScenceID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", DoorWayID, "SceneTransfer_Template");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", ScenceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = DoorWayID;
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();

                //播放音效
                Game_PublicClassVar.Get_function_UI.PlaySource("20008", "2");
            }
            else {

                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_317");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_3185");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + sceneEnterLv + langStrHint_2);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint("击杀场景怪物提升至" + sceneEnterLv + "级后进入！");
            }
        }
    }
}
