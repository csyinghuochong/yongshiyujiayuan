using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_GetherItem : MonoBehaviour {

    public ObscuredString SceneItemOnlyID;              //场景物唯一ID,用于存储只显示一次的宝箱,手工配置
    public ObscuredString SceneItmeID;
    public ObscuredString SceneItemPastureID;
    public ObscuredFloat SceneItemPastureQiangZhuangValue;
    public ObscuredString SceneItemType;                //拾取道具类型（1：表示普通宝箱/掉落  2：表示喜从天降活动宝箱  3: 主城宝箱活动）
    public ObscuredInt ChestID;                         //活动宝箱ID
    public GameObject DropNamePosition;
	//public string DropItemID;                         //掉落的道具ID
	//public int DropItemNum;                           //掉落的道具数据,一般默认为1 像金币或其他货币会用的上
	public bool IfRoseTake;                             //道具是否被拾取
	private bool DropNameUpdateStatus;                  //掉落更新 表示执行一次
	private GameObject obj_dropName;                    //掉落物体的UI
	private Vector3 vec3_droName;                       //掉落道具名字在UI中的位置
	private GameObject dropItemModel;			        //3D道具模型
	private bool openStatus;					        //打开状态
	public float openTime;						        //打开时间
	public float openTimeSum;					        //打开时间累计
	public bool openStatusOnce;				            //打开宝箱第一次实例化UI
	public GameObject mainUIGetherItem;		            //主界面打开进度条UI
	public ObscuredString CostItemID;					//开启宝箱消耗的道具ID
	public ObscuredString CostItemNum;					//开启宝箱消耗的道具数量
	public bool IfNeedItem;						        //开启宝箱是否消耗道具
    private ObscuredString sceneItemName;               //宝箱名称
    private ObscuredString sceneItemQuality;            //宝箱品质
    private ObscuredString sceneItemType;               //场景物类型
    private ObscuredInt sceneItemApearPro;              //场景物出现的概率
    public GameObject[] sceneItemCreateMonsterList;     //触发场景物出现怪物
    public ObscuredString CreateMonsterID;              //创建怪物
    private ObscuredString sceneItemStr;
    public ObscuredFloat ScenceItemReviveTime;          //场景物自身重生时间
    private ObscuredFloat scenceItemReviceTimeSum;
    public bool ScenceItemReviveStatus;                 //复活中 开启此状态
    public GameObject ScenceItemShowObj;

    // Use this for initialization
    void Start () {

        //根据概率设置是否显示
        //Debug.Log("SceneItmeID = " + SceneItmeID);
        string appearPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AppearPro", "ID", SceneItmeID, "SceneItem_Template");
        float gailv = float.Parse(appearPro) / 10000;
        float randomValue = Random.value;
        //Debug.Log("value = " + randomValue + ";gailv = " + gailv + " appearPro = " + appearPro);
        if (randomValue > gailv) {
            //Debug.Log("删除1111!");
            Destroy(this.gameObject);
        }

        //获取场景物的相关属性
        GetHerItemUpdateData();


        if (sceneItemType == "7")
        {
            //Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>()
            float dis = Vector3.Distance(Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().PastureInitVec3, this.transform.position);
            if (dis >= 10)
            {
                float addDis_x = Random.value * 10f - 5f;
                float addDis_z = Random.value * 10f - 5f;

                float pos_x = -8f + addDis_x;
                float pos_z = 0 + addDis_z;

                this.transform.position = new Vector3(pos_x, 1.1f, pos_z);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		//判定与角色相距30米进行名称显示
		float distance = Vector3.Distance(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position, this.gameObject.transform.position);
		if (distance <= 30.0f)
		{
			//修正物体在屏幕中的位置
			vec3_droName = Camera.main.WorldToViewportPoint(DropNamePosition.transform.position);
			vec3_droName = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(vec3_droName);

			//第一次显示
			if (DropNameUpdateStatus == true && ScenceItemReviveStatus == false)
			{
				DropNameUpdateStatus = false;
                //实例化UI
                if (Game_PublicClassVar.Get_game_PositionVar.Obj_UICaiJiName != null)
                {
                    obj_dropName = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICaiJiName);
                    obj_dropName.GetComponent<UI_CaiJiItem>().Obj_Par = this.gameObject;

                    obj_dropName.SetActive(false);
                    obj_dropName.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_DropItemSet.transform);
                    obj_dropName.transform.localPosition = new Vector3(vec3_droName.x, vec3_droName.y, 0);
                    obj_dropName.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    obj_dropName.SetActive(true);
                    //显示名称
                    obj_dropName.transform.Find("Lab_DropName").transform.GetComponent<Text>().color = Game_PublicClassVar.Get_function_UI.QualityReturnColor(sceneItemQuality);
                    obj_dropName.transform.Find("Lab_DropName").transform.GetComponent<Text>().text = sceneItemName;
                }
			}
			//修正掉落物体的位置
            if (obj_dropName!=null) {
                obj_dropName.transform.localPosition = new Vector3(vec3_droName.x, vec3_droName.y, 0);
            }
			
		}
		else {
			//不显示掉落名称（清理不需要显示的内容防止卡）
			DropNameUpdateStatus = true;
            
			MonoBehaviour.Destroy(obj_dropName);
		}

        //重生状态 无法开启
        if (ScenceItemReviveStatus)
        {
            IfRoseTake = false;
        }

        if (IfRoseTake)
        {
            if (sceneItemType == "7")
            {
                //判断农场格子有位置
                if (Game_PublicClassVar.Get_function_Pasture.ReturnNullSpaceNum() == "-1")
                {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("家园仓库已满!");
                    IfRoseTake = false;
                }
            }
        }

        //开启拾取状态
        if (IfRoseTake) {
            GameObject playSourceObj = null;
			if(!openStatusOnce){
				//实例化打开UI
				mainUIGetherItem = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIGetherItem);
				mainUIGetherItem.transform.Find("Lab_OpenText").transform.GetComponent<Text>().text = "开启中……";
				mainUIGetherItem.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_GetherItemSet.transform);
				mainUIGetherItem.transform.localPosition = new Vector3(0,0,0);
				mainUIGetherItem.transform.localScale = new Vector3(1,1,1);
				openStatusOnce = true;
                //播放开启音效
                playSourceObj = Game_PublicClassVar.Get_function_UI.PlaySource("20006", "2", openTime);
			}

			//显示当前打开的进度值
			openTimeSum = openTimeSum + Time.deltaTime;
			float value = openTimeSum/openTime;
			//Debug.Log("value = "+value);

            if (mainUIGetherItem != null) {
                mainUIGetherItem.transform.Find("Img_GetherPro").GetComponent<Image>().fillAmount = value;
            }

			if(openTimeSum>openTime){
				IfRoseTake = false;
				openTimeSum = 0;
                Destroy(mainUIGetherItem);
                //开启宝箱是否需要道具(场景普通掉落或场景宝箱)
                if (SceneItemType == "1" || SceneItemType == "" || SceneItemType == "3")
                {
                    if (IfNeedItem)
                    {
                        if (Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(CostItemID)>= int.Parse(CostItemNum))
                        {
                            Game_PublicClassVar.Get_function_Rose.CostBagItem(CostItemID, int.Parse(CostItemNum));
                            openStatus = true;
                        }
                        else
                        {
                            string costItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", CostItemID, "Item_Template");
                            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_430");
                            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_431");
                            string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_432");

                            Game_PublicClassVar.Get_function_UI.GameHint(langStrHint_1 + CostItemNum + langStrHint_2 + costItemName + "," + langStrHint_3);
                            //Game_PublicClassVar.Get_function_UI.GameHint("需要" + CostItemNum + "个" + costItemName + ",才能开启");
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseGetherItemStatus = false;




                        }
                    }
                    else
                    {
                        openStatus = true;
                    }

                }
                else{
					openStatus = true;
				}
			}else{
				if(openTimeSum>0.1f){
					//如果开启的过程中触发移动，则开启失败
                    bool roseStopTakeStatus = false;
					if(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus !="1"){
                        roseStopTakeStatus = true;
					}

                    //如果角色受到伤害也停止开锁
                    /*
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseIfHit) {
                        roseStopTakeStatus = true;
                    }
                    */
                    //取消拾取状态
                    if (roseStopTakeStatus)
                    {
                        //初始化自身变量
                        IfRoseTake = false;
                        openTimeSum = 0;
                        openStatus = false;
                        Destroy(mainUIGetherItem);
                        openStatusOnce = false;

                        //销毁音效
                        if (playSourceObj != null)
                        {
                            Destroy(playSourceObj);
                        }

                        //取消拾取状态
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseGetherItemStatus = false;
                    }
				}
			}
		}



		//如果道具被拾取，注销对应的3D模型和UI
		if (openStatus) {

            openStatus = false;

            //取消拾取状态
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseGetherItemStatus = false;

            //场景普通掉落或场景宝箱
            if (SceneItemType == "1" || SceneItemType == "" || SceneItemType == "3") {

                Game_PublicClassVar.Get_function_UI.PlaySource("20007", "2");

                //触发掉落
                //获取掉落ID
                string dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropID", "ID", SceneItmeID, "SceneItem_Template");
                if (dropID != "0")
                {
                    Game_PublicClassVar.Get_function_AI.DropIDToDropItem(dropID, this.transform.position);
                }

                MonoBehaviour.Destroy(obj_dropName);              //销毁UI

                if (ScenceItemReviveTime == 0)
                {
                    //Debug.Log("删除22222!");
                    MonoBehaviour.Destroy(this.gameObject);           //销毁3D模型
                }
                else {
                    //隐藏自身下面的全部物体
                    ItemModelShowStatus(this.gameObject, false);
                    ScenceItemReviveStatus = true;
                }


                //创建怪物
                if (sceneItemCreateMonsterList.Length != 0)
                {
                    appearMonster();        //显示隐藏怪物
                }

                //开启宝箱显示图片
                if (sceneItemType == "3")
                {
                    showWord();
                    //游戏暂停
                    Time.timeScale = 0;
                }

                //藏宝洞窟宝箱ID
                if (sceneItemType == "5") {

                    //随机给目标附加一个BUFFID
                    string buffIDStr = "95001002";
                    if (Random.value >= 0.5f)
                    {
                        buffIDStr = "95001002";     //减速Buff
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_125");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你碰到了一个被腐蚀的宝箱,似乎对你产生了不好的作用!");
                    }
                    else {
                        buffIDStr = "95001003";     //眩晕Buff
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_126");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你碰到了一个被腐蚀的宝箱,似乎对你产生了不好的作用!");
                    }
                    //触发BUff
                    if (buffIDStr != "0" && buffIDStr != "")
                    {
                       Game_PublicClassVar.Get_function_Skill.SkillBuff(buffIDStr, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose);
                    }
                }

                //创建指定怪物
                if (sceneItemType == "6") {
                    //开始创建怪物
                    Game_PublicClassVar.Get_function_AI.AI_CreatMonster(CreateMonsterID, this.transform.position);
                    /*
                    GameObject createObj = (GameObject)Instantiate(CreateMonsterObj);
                    createObj.transform.position = this.transform.position;
                    createObj.transform.localScale = new Vector3(1, 1, 1);
                    */
                }

                //拾取牧场掉落
                if (sceneItemType == "7") {

                    Game_PublicClassVar.function_Pasture.PastureDeleteDrop(SceneItemPastureID);

                    Game_PublicClassVar.Get_function_UI.PlaySource("20007", "2");
                    //触发掉落
                    //获取掉落ID
                    dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropID", "ID", SceneItmeID, "SceneItem_Template");
                    if (dropID != "0")
                    {
                        //Debug.Log("删除33333!");
                        MonoBehaviour.Destroy(this.gameObject);           //销毁3D模型
                        MonoBehaviour.Destroy(obj_dropName);              //销毁UI

                        string[] dropItmeList = dropID.Split(',');
                        if (dropItmeList.Length >= 3) {
                            int dropNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(int.Parse(dropItmeList[1]), int.Parse(dropItmeList[2]));
                            Game_PublicClassVar.Get_function_Pasture.SendPastureBag(dropItmeList[0], dropNum,"0",0, SceneItemPastureQiangZhuangValue);
                        }
                    }

                }

                //场景道具唯一ID处理
                if (SceneItemOnlyID != "" && SceneItemOnlyID != "0")
                {
                    sceneItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    if (sceneItemStr != "")
                    {
                        sceneItemStr = sceneItemStr + ";" + SceneItemOnlyID;
                    }
                    else
                    {
                        sceneItemStr = SceneItemOnlyID;
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SceneItemID", sceneItemStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                }

                //写入成就
                switch (SceneItmeID) {

                    //第一章黄金宝箱
                    case "41001012":
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("102", "1", "1");
                        break;
                    case "41001023":
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("102", "1", "1");
                        break;

                    //第二章黄金宝箱
                    case "41002013":
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("102", "2", "1");
                        break;

                    //第三章黄金宝箱
                    case "41003013":
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("102", "3", "1");
                        break;

                    //第四章黄金宝箱
                    case "41004013":
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("102", "4", "1");
                        break;

                    //第五章黄金宝箱
                    case "41005013":
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("102", "5", "1");
                        break;
                }

            }

            if (SceneItemType == "3") {

                if (Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang != null) {
                    Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang.GetComponent<UI_HuoDong_BaoXiang>().ChestOpen(ChestID);
                }

                if (SceneItmeID == "42101003" || SceneItmeID == "42102003" || SceneItmeID == "42103003" || SceneItmeID == "42104003" || SceneItmeID == "42105003")
                {
                    string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "天降宝箱!恭喜玩家" + roseName + "在主城开启散落的物资箱时发现了意外收获!");
                    Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
                    comStr_4.str_1 = "6";
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010191, comStr_4);
                }

                if (SceneItmeID == "42101008"|| SceneItmeID == "42101009"|| SceneItmeID == "42102008"|| SceneItmeID == "42102009"|| SceneItmeID == "42103008"|| SceneItmeID == "42103009"|| SceneItmeID == "42104008"|| SceneItmeID == "42104009"|| SceneItmeID == "42105008"|| SceneItmeID == "42105009") {
                    string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "天降宝箱!恭喜玩家" + roseName + "在主城发现了大宝箱!");
                    Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
                    comStr_4.str_1 = "5";
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010191, comStr_4);
                }

            }

            //喜从天降宝箱活动
            /*
            if (SceneItemType == "2") {
                Game_PublicClassVar.Get_function_UI.PlaySource("20007", "2");

                //触发掉落
                Pro_ComStr_2 pro_ComStr_2 = new Pro_ComStr_2();
                pro_ComStr_2.str_1 = Game_PublicClassVar.Get_wwwSet.ServerRoseThreadID;
                pro_ComStr_2.str_2 = ChestID.ToString();
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20001012, pro_ComStr_2);
                //ChestReward();
            }
            */
		}

        //开启复活计时
        if (ScenceItemReviveStatus) {
            scenceItemReviceTimeSum = scenceItemReviceTimeSum + Time.deltaTime;
            if (scenceItemReviceTimeSum >= ScenceItemReviveTime) {
                //显示模型
                ItemModelShowStatus(this.gameObject, true);
                //重置数据
                GetHerItemUpdateData();
                //清空复活相关数据
                ScenceItemReviveStatus = false;
                scenceItemReviceTimeSum = 0;
            }
        }

    }


    //删除
    private void OnDestroy()
    {
        if (IfRoseTake)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_123");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你的宝箱已经先手被别人抢走了！ T.T");
        }
        //销毁标名称UI
        if (mainUIGetherItem != null) {
            Destroy(mainUIGetherItem);
        }

        if (obj_dropName != null) {
            Destroy(obj_dropName);
        }

    }

    //初始化加载
    public void ChestInitia()
    {

        //初始化数据
        DropNameUpdateStatus = true;

        //获取场景物的相关属性
        openTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OpenTime", "ID", SceneItmeID, "SceneItem_Template"));
        CostItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OpenItemID", "ID", SceneItmeID, "SceneItem_Template");
        if (CostItemID == "0")
        {
            IfNeedItem = false;
        }
        else
        {
            IfNeedItem = true;
            //获取数量
            CostItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OpenValue", "ID", SceneItmeID, "SceneItem_Template");
        }

        sceneItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", SceneItmeID, "SceneItem_Template");
        sceneItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Quality", "ID", SceneItmeID, "SceneItem_Template");

    }

    //触发宝箱掉落
    public void ChestReward()
    {
        //喜从天降宝箱
        if (SceneItemType == "2" )
        {
            if (IfNeedItem)
            {
                if (Game_PublicClassVar.Get_function_Rose.CostBagItem(CostItemID, int.Parse(CostItemNum)))
                {
                    Debug.Log("扣除开箱道具成功！");
                }
                else
                {
                    string costItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", CostItemID, "Item_Template");
                    string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_127");
                    string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_128");
                    string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_129");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + CostItemNum + langStrHint_2 + costItemName + langStrHint_3 );
                    //Game_PublicClassVar.Get_function_UI.GameHint("需要" + CostItemNum + "个" + costItemName + ",才能开启");
                    return;
                }
            }
        }

        //获取掉落ID
        string dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropID", "ID", SceneItmeID, "SceneItem_Template");
        if (dropID != "0")
        {
            Game_PublicClassVar.Get_function_AI.DropIDToDropItem(dropID, this.transform.position);
            MonoBehaviour.Destroy(obj_dropName);              //销毁UI
            //Debug.Log("删除44444!");
            MonoBehaviour.Destroy(this.gameObject);           //销毁3D模型
        }
    }


    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.layer == 14)
        {
            if (sceneItemType == "3")
            {
                //Debug.Log("主角碰到了");
                appearMonster();
                deleteScenceItem();
            }
        }
    }

    //显示隐藏怪物
    void appearMonster() {
        
        for (int i = 0; i <= sceneItemCreateMonsterList.Length - 1; i++)
        {
            if (sceneItemCreateMonsterList[i] != null) {
                sceneItemCreateMonsterList[i].SetActive(true);      //显示出隐藏怪物
            }
        }
    }

    void deleteScenceItem() {
        MonoBehaviour.Destroy(obj_dropName);              //销毁UI
        //Debug.Log("删除55555!");
        MonoBehaviour.Destroy(this.gameObject);           //销毁3D模型
    }

    void showWord() {

        GameObject obj_storyBack = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_StoreTextBack);
        obj_storyBack.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_StorySpeakSet.transform);
        obj_storyBack.transform.localPosition = new Vector3(0, 0, 0);
        obj_storyBack.transform.localScale = new Vector3(1, 1, 1);
        UI_StoreTextBack ui_StoreTextBack = obj_storyBack.GetComponent<UI_StoreTextBack>();
        ui_StoreTextBack.ExitType = "0";        //默认显示点击取消
        string showText = "12345678901234567890123456789012345678901234567890";
        ui_StoreTextBack.StoreText = showText;

    }

    public void GetHerItemUpdateData() {

        //初始化数据
        DropNameUpdateStatus = true;

        //测试场景物出现的概率
        sceneItemApearPro = 10000;
        sceneItemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Type", "ID", SceneItmeID, "SceneItem_Template");
        if ((int)(Random.value * 10000) >= sceneItemApearPro)
        {
            deleteScenceItem();
        }

        //判定是否一次性宝箱的唯一ID
        sceneItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //Debug.Log("sceneItemStr = "+ sceneItemStr);
        if (sceneItemStr != "")
        {
            string[] sceneStr = sceneItemStr.ToString().Split(';');
            for (int i = 0; i <= sceneStr.Length - 1; i++)
            {
                if (SceneItemOnlyID != "0" && SceneItemOnlyID != "")
                {
                    //如果此宝箱唯一ID被开启过将立刻注销此宝箱
                    if (sceneStr[i] == SceneItemOnlyID)
                    {
                        //Debug.Log("删除6666!");
                        deleteScenceItem();
                    }
                }
            }
        }


        //获取场景物的相关属性
        openTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OpenTime", "ID", SceneItmeID, "SceneItem_Template"));
        CostItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OpenItemID", "ID", SceneItmeID, "SceneItem_Template");
        if (CostItemID == "0")
        {
            IfNeedItem = false;
        }
        else
        {
            IfNeedItem = true;
            //获取数量
            CostItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OpenValue", "ID", SceneItmeID, "SceneItem_Template");
        }

        sceneItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", SceneItmeID, "SceneItem_Template");
        sceneItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Quality", "ID", SceneItmeID, "SceneItem_Template");

        //读取重生时间
        ScenceItemReviveTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ReviveTime", "ID", SceneItmeID, "SceneItem_Template"));

    }


    public void ItemModelShowStatus(GameObject targetObj,bool hideStatus) {
        //隐藏自身下面的全部物体
        for (int i = 0; i < targetObj.transform.childCount; i++)
        {
            GameObject go = targetObj.transform.GetChild(i).gameObject;
            go.SetActive(hideStatus);
        }
    }
}