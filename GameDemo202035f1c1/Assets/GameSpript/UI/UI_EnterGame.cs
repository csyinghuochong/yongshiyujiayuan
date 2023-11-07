using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Weijing;

public class UI_EnterGame : MonoBehaviour {

    private AsyncOperation mAsyn;
    private bool enterGameStatus;
    public GameObject Obj_Loading;
    private GameObject obj_loading;
    private bool loadStatus;
    private float loadingSumTime;               //加载延迟显示时间
    private float loadingSumTimeSum;
    private UI_Set ui_set;
    public GameObject Obj_RoseEnterGameSet;     //点击创建角色时,隐藏进入游戏的对应按钮
    public GameObject Obj_CreateRoseSet;        //点击创建角色时,显示的创角相关界面
    public string SceneTransferID;
    public bool roseObjUpdataStatus;
    private bool enterBuildGameStatus;
    public GameObject[] KeepObj;
    private bool DestroyKeepObj;
    public GameObject Obj_UpdataDataText;
    public string EnterSceneID;
    private bool clearnStatus;              //清理UI状态
    private bool chengJiuStatus;
    private float chengJiuShowSum;
    public Vector3 MapRosePositionVec3;     //玩家进入地图的初始位置

    private bool bReturnMainCity;


	// Use this for initialization
	void Start () {

        loadingSumTime = 0.5f;

        //开启选择角色状态
        //Game_PublicClassVar.Get_game_PositionVar.EnterGameStatus = true;

        //隐藏主界面和任务血条
        if (Application.loadedLevelName != "StartGame") {
            ui_set = GameObject.FindWithTag("UI_Set").GetComponent<UI_Set>();
            ui_set.Obj_MainUI.SetActive(false);
            //ui_set.Obj_UI_RoseHp.SetActive(false);
            //更新角色当前属性
            //Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty();
        }
	}

	
	// Update is called once per frame
	void Update () {

        try
        {
            //开启Loding状态
            if (enterGameStatus)
            {
                //Debug.Log("设置坐标点10");
                loadingSumTimeSum = loadingSumTimeSum + Time.deltaTime;
                //Debug.Log("loadingSumTimeSum = " + loadingSumTimeSum + "        loadingSumTime = " + loadingSumTime);
                if (loadingSumTimeSum >= loadingSumTime)
                {
                    //Debug.Log("准备执行异步加载");
                    if (loadStatus)
                    {
                        //Debug.Log("开始执行异步加载");
                        loadStatus = false;
                        loadingSumTimeSum = 0;      //确保不会执行第二次
                        //enterGameStatus = false;
                        //Debug.Log("this.gameObject.activeSelf = " + this.gameObject.activeSelf);
                        Game_PublicClassVar.Get_wwwSet.MapEnterStatus = true;
                        Game_PublicClassVar.Get_wwwSet.MapEnterTimeSum = 0;

                        StartCoroutine("Load");
                        //Debug.Log("异步加载执行完毕");
                    }
                }
                if (mAsyn != null && !mAsyn.isDone)
                {
                    //Debug.Log("下载不为空准备设置坐标点");
                    float value = (float)mAsyn.progress / 2.0f;
                    //Debug.Log(value);
                    if (value != 0)
                    {
                        //Debug.Log("设置坐标点3");
                        obj_loading.GetComponent<UI_Loading>().LoadingValue = value + loadingSumTime + 0.1f;
                        //Debug.Log("Loding:" + obj_loading.GetComponent<UI_Loading>().LoadingValue);
                        if (mAsyn.progress >= 0.9f)
                        {
                            obj_loading.GetComponent<UI_Loading>().LoadingValue = 1;
                            mAsyn.allowSceneActivation = true;
                            //Debug.Log("下载不为空准备设置坐标点123");
                            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.SetActive(false);
                            //测试
                            if (!roseObjUpdataStatus)
                            {
                                //死亡进入地图时设置血量
                                if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow <= 0)
                                {
                                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hp;
                                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";
                                }
                                //设置角色状态
                                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";


                                GameTimer timer = new GameTimer(0.1f, 1, delegate ()
                                {
                                    float transfer_X = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapTransfer_X", "ID", SceneTransferID, "SceneTransfer_Template"));
                                    float transfer_Y = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapTransfer_Y", "ID", SceneTransferID, "SceneTransfer_Template"));
                                    float transfer_Z = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapTransfer_Z", "ID", SceneTransferID, "SceneTransfer_Template"));

                                    //transfer_Y = 32.0f;
                                    //设置角色坐标
                                    if (MapRosePositionVec3 != Vector3.zero && MapRosePositionVec3 != null)
                                    {
                                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position = MapRosePositionVec3;
                                        //Debug.Log("设置坐标点 MapRosePositionVec3 = " + MapRosePositionVec3);
                                        //MapRosePositionVec3 = Vector3.zero;
                                    }
                                    else
                                    {
                                        //Debug.Log("设置坐标点 SceneTransferID = " + SceneTransferID + ";transfer_X = " + transfer_X);
                                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position = new Vector3(transfer_X, transfer_Y, transfer_Z);
                                    }
                                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.SetActive(false);
                                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.SetActive(true);


                                    //设置宠物坐标
                                    Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
                                    for (int i = 0; i <= roseStatus.RosePetObj.Length - 1; i++)
                                    {
                                        GameObject go = roseStatus.RosePetObj[i];
                                        if (go != null)
                                        {
                                            go.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
                                            go.GetComponent<AIPet>().AI_Hp_Status = false;
                                            go.SetActive(false);
                                            go.GetComponent<AIPet>().updateStarMoveStatus = true;
                                            go.SetActive(true);
                                        }
                                    }

                                    //设置召唤物坐标
                                    for (int i = 0; i < Game_PublicClassVar.Get_game_PositionVar.Obj_RoseCreatePetSet.transform.childCount; i++)
                                    {
                                        GameObject go = Game_PublicClassVar.Get_game_PositionVar.Obj_RoseCreatePetSet.transform.GetChild(i).gameObject;
                                        if (go != null)
                                        {
                                            go.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
                                            go.GetComponent<AIPet>().AI_Hp_Status = false;
                                            go.SetActive(false);
                                            go.GetComponent<AIPet>().updateStarMoveStatus = true;
                                            go.SetActive(true);
                                        }
                                    }

                                });
                                timer.Start();

                                if (!clearnStatus)
                                {
                                    ClearnUI();
                                    clearnStatus = true;
                                }

                                //主界面适配
                                Game_PublicClassVar.Get_function_UI.ShiPei_MainUI();
                                //更新主界面经验显示
                                //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseExp.GetComponent<UI_MainUIRoseExp>().UpdataRoseExp = true;

                                //成就显示状态开启
                                chengJiuStatus = true;

                            }
                        }
                    }
                }
                else
                {
                    if (loadingSumTimeSum < loadingSumTime)         //加判定是为了防止在最后一帧执行这一步,结果会是0.66
                    {
                        if (loadingSumTimeSum >= obj_loading.GetComponent<UI_Loading>().LoadingValue)
                        {
                            obj_loading.GetComponent<UI_Loading>().LoadingValue = loadingSumTimeSum;
                            //Debug.Log("LoadingValue333 = " + loadingSumTimeSum + "loadingSumTimeSum = " + loadingSumTimeSum);
                        }
                    }
                }

                //下载完毕关闭进入开关
                if (mAsyn != null)
                {
                    //Debug.Log("下载完成!");
                    if (mAsyn.isDone)
                    {
                        System.GC.Collect();
                        Resources.UnloadUnusedAssets();

                        //Debug.Log("下载完成123!");
                        enterGameStatus = false;
                        Destroy(obj_loading, 1);     //延迟1秒关闭,防止穿帮
                        Game_PublicClassVar.Get_game_PositionVar.EnterScenceStatus = true;
                        //更新场景名称
                        string sceneName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", Application.loadedLevelName, "Scene_Template");

                        //大秘境特殊处理
                        if (Application.loadedLevelName == "100002")
                        {
                            string nowDaMiJingLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            if (nowDaMiJingLv == "")
                            {
                                nowDaMiJingLv = "1";
                            }
                            int damijingLv = int.Parse(nowDaMiJingLv);
                            sceneName = sceneName + damijingLv.ToString() + "层";
                        }

                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MapName.GetComponent<Text>().text = sceneName;
                        //Game_PublicClassVar.Get_game_PositionVar.EnterScenceID = EnterSceneID;       //设置地图ID
                        //设置怪物
                        Game_PublicClassVar.Get_game_PositionVar.MonsterSet = GameObject.Find("Monster");
                        clearnStatus = false;       //重置清空状态

                        //重置地图击杀数据
                        Game_PublicClassVar.Get_game_PositionVar.MapKillMonsterNum = 0;
                        Game_PublicClassVar.Get_game_PositionVar.MapKillBossID = "";

                        //显示难度
                        Game_PublicClassVar.Get_function_UI.ShowMainUINanDuImg();

                        //显示回城按钮
                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ReturnBuilding.SetActive(true);

                        //增加进入地图次数（遇到宝宝相关）
                        Game_PublicClassVar.Get_function_Rose.Rose_AddEnterMapNum(1);

                        //清理UI
                        ClearnUI();

                        //显示墓碑
                        string RoseDeathMuBeiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMuBeiStr", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        if (RoseDeathMuBeiStr != "")
                        {
                            string[] muBeiList = RoseDeathMuBeiStr.Split('|');
                            for (int i = 0; i < muBeiList.Length; i++)
                            {
                                if (muBeiList[i] != "")
                                {
                                    string mubeiMap = muBeiList[i].Split(';')[0];
                                    string mubeiMapPosi = muBeiList[i].Split(';')[1];
                                    if (mubeiMap == Application.loadedLevelName)
                                    {
                                        //设置墓碑
                                        GameObject mubeiObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_RoseDeathMuBei);
                                        mubeiObj.transform.localScale = new Vector3(1, 1, 1);

                                        float posi_X = float.Parse(mubeiMapPosi.Split(',')[0]);
                                        float posi_Y = float.Parse(mubeiMapPosi.Split(',')[1]);
                                        float posi_Z = float.Parse(mubeiMapPosi.Split(',')[2]);

                                        Vector3 vec3 = new Vector3(posi_X, posi_Y, posi_Z);
                                        mubeiObj.transform.position = vec3;
                                    }
                                }
                            }
                        }

                        //显示地图名称
                        GameObject enterMapName = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIEnterMapShowName);
                        enterMapName.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                        enterMapName.transform.localScale = new Vector3(1, 1, 1);
                        enterMapName.GetComponent<UI_MapNameShow>().mapName = sceneName;
                    }
                }
            }
            //Debug.Log("Loding:"+obj_loading.GetComponent<UI_Loading>().LoadingValue);

            if (enterBuildGameStatus)
            {
                loadingSumTimeSum = loadingSumTimeSum + Time.deltaTime / 1;
                if (loadingSumTimeSum > 1)
                {
                    loadingSumTimeSum = 1;
                }

                //设置进度条的值,如果不加0.2f 加载条读不到100% 会提前加载完成
                float loadingProValue = loadingSumTimeSum + 0.2f;
                if (loadingProValue >= 1)
                {
                    loadingProValue = 1;
                }

                obj_loading.GetComponent<UI_Loading>().LoadingValue = loadingProValue;

                //加载完成延迟1秒注销Loding
                if (loadingSumTimeSum >= 1)
                {
                    bReturnMainCity = true;
                    //Destroy(obj_loading, 1);     //延迟1秒关闭,防止穿帮
                    enterBuildGameStatus = false;
                    DestroyKeepObj = true;
                    //Application.LoadLevel("EnterGame"); //加载场景
                    Debug.Log("进入主城开始...");
                    SceneManager.LoadScene(1);
                    Debug.Log("进入主城结束...");
                }
            }

            if (chengJiuStatus)
            {
                chengJiuShowSum = chengJiuShowSum + Time.deltaTime;
                if (chengJiuShowSum >= 3.0f)
                {
                    chengJiuStatus = false;
                    chengJiuShowSum = 0;
                    //写入成就(进入地图)
                    string mapID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", SceneTransferID, "SceneTransfer_Template");
                    Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("101", mapID, "1");
                }
            }
        }
        catch (Exception ex) {
            Debug.LogError("加载地图报错:" + ex);
        }
    }

    void LateUpdate()
    {
        if (DestroyKeepObj)
        {
            DestroyKeepObj = false;
            //保证切换场景以下预设体不消失
            for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.Obj_Keep.Length; i++)
            {
                Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_Keep[i - 1]);
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log("开始销毁：EnterGame");
        //if (bReturnMainCity != true)
        //{
        //    EventHandle.CheckCreateRole();
        //}
        //bReturnMainCity = false;
    }

    //进入建筑界面
    public void EnterBuildGame() {

        //实例化一个Loding界面
        string mapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowMapName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        obj_loading = (GameObject)Instantiate(Obj_Loading);
        Debug.Log("返回主城...." + mapName);
        //更换Loding背景图
        if (mapName == "EnterGame")
        {
            object obj = Resources.Load("GameUI/Back/Loding_1", typeof(Sprite));
            Sprite itemIcon = obj as Sprite;
            obj_loading.GetComponent<UI_Loading>().Obj_ImgBack.GetComponent<Image>().sprite = itemIcon;
        }
        obj_loading.transform.SetParent(this.transform);
        //obj_loading.transform.localScale = new Vector3(1, 1, 1);
        //obj_loading.transform.get = new Vector3(0, 0, 0);
        obj_loading.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        obj_loading.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        enterBuildGameStatus = true;
        obj_loading.transform.localScale = new Vector3(1, 1, 1);
        //Application.LoadLevel("EnterGame"); //加载场景
        //DontDestroyOnLoad(this.gameObject);
        loadingSumTimeSum = 0;
        EnterSceneID = mapName;
    }

    //进入游戏
    public void EnterGame() {

        //Debug.Log("进入地图...");

        if (Game_PublicClassVar.Get_wwwSet.MapEnterStatus)
        {
            Debug.LogError("当前正在加载地图2222... 进入地图失效!");
            return;
        }

        Game_PublicClassVar.Get_wwwSet.MapEnterStatus = true;

        if (Obj_UpdataDataText != null) {
            Obj_UpdataDataText.SetActive(false);
        }
        
        mAsyn = null;
        enterGameStatus = true;
        
        //加载Loding的UI界面
        string mapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowMapName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        obj_loading = (GameObject)Instantiate(Obj_Loading);

        //更换Loding背景图
        if (mapName == "EnterGame")
        {
            object obj = Resources.Load("GameUI/Back/Loding_1", typeof(Sprite));
            Sprite itemIcon = obj as Sprite;
            obj_loading.GetComponent<UI_Loading>().Obj_ImgBack.GetComponent<Image>().sprite = itemIcon;

            //隐藏UI
            /*
            ui_set.Obj_MainFunctionUI.SetActive(false);
            ui_set.Obj_MapName.SetActive(false);
            ui_set.Obj_RoseTask.SetActive(false);
            */
        }
        else {
            int ranIntValue = Game_PublicClassVar.Get_function_Rose.ReturnEquipRamdomValue(2, 5);
            object obj = Resources.Load("GameUI/Back/Loding_" + ranIntValue, typeof(Sprite));
            Sprite itemIcon = obj as Sprite;
            obj_loading.GetComponent<UI_Loading>().Obj_ImgBack.GetComponent<Image>().sprite = itemIcon;
        }

        obj_loading.transform.SetParent(this.transform);
        obj_loading.transform.localScale = new Vector3(1, 1, 1);
        obj_loading.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        obj_loading.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        loadStatus = true;
        this.transform.Find("EnterGameSet").gameObject.SetActive(false);

        //清空UI
        if (ui_set != null) {
            ui_set.Obj_BuildingMainUISet.SetActive(false);        //隐藏建筑UI
            ui_set.Obj_BuildingNameSet.SetActive(false);        //隐藏建筑名称UI
            ui_set.Obj_BuildingHuoBiSet.SetActive(false);               //隐藏货币
            ui_set.Obj_MainFunctionUI.SetActive(true);
            ui_set.Obj_UIMapName.SetActive(true);
            ui_set.Obj_RoseTask.SetActive(true);
            ui_set.Obj_RightDownSet.SetActive(false);
        }

        //取消目标战斗状态
        if (Application.loadedLevelName != "StartGame") {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightStatus = false;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightMonsterActList.Clear();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().TansuoRosePosiVec3 = Vector3.zero;
        }

        //Debug.Log("进入主界面！！！！！！！！！！！！！！！！！！！！！！！！");

        //Debug.Log("Game_PublicClassVar.Get_wwwSet.RoseID = " + Game_PublicClassVar.Get_wwwSet.RoseID);
        //Debug.Log("mapName = " + mapName);

        EnterSceneID = mapName;
        if (mapName != "EnterGame")
        {
            //清理UI
            ClearnUI();
        }
    }

    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    IEnumerator Load()
    {
        //获取角色当前进入地图
        string mapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowMapName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string mapPositionName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowMapPositionName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_game_PositionVar.EnterScencePositionName = mapPositionName;
        //yield return new WaitForEndOfFrame();
        //mAsyn = Application.LoadLevelAsync(mapName);
        //mapName = "EnterGame";
        //Debug.Log("mapName = " + mapName);
        //Debug.Log("mapPositionName = " + mapPositionName);
        mAsyn = SceneManager.LoadSceneAsync(mapName);
        //Debug.Log("开始加载地图 = " + mapName);
        mAsyn.allowSceneActivation = false;
        //进入游戏界面恢复主界面正常显示
        Game_PublicClassVar.Get_game_PositionVar.EnterRoseCameraDrawStatus = false;
        if (ui_set != null) {
            ui_set.Obj_MainUI.SetActive(true);
            ui_set.Obj_UI_RoseHp.SetActive(true);
            //更新UI显示的技能
            Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
        }
        //Debug.Log("开始载入游戏地图");
        yield return mAsyn;
    }


    //创建角色
    public void CreateRoseID() { 
        //判定是否输入角色名称
        Obj_RoseEnterGameSet.SetActive(false);
        Obj_CreateRoseSet.SetActive(true);
    }

    //清空UI
    public void ClearnUI() {

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
            //Debug.Log("清空掉落");
        }
        //清空名称显示
        GameObject aiHpSet = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet;
        for (int i = 0; i < aiHpSet.transform.childCount; i++)
        {
            GameObject go = aiHpSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }
        
        //清空任务显示
        GameObject npcTaskSet = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcTaskSet;
        for (int i = 0; i < npcTaskSet.transform.childCount; i++)
        {
            GameObject go = npcTaskSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        //重置摇杆状态
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_YaoGan.GetComponent<UI_GameYaoGan>().Exit();
    }

}
