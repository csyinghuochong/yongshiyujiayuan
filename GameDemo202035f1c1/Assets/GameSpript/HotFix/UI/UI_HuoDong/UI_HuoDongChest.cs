using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_HuoDongChest : MonoBehaviour {

    //public string SceneItemOnlyID;              //场景物唯一ID,用于存储只显示一次的宝箱,手工配置
    public ObscuredInt ChestID;
    public ObscuredString SceneItmeID;
	public GameObject DropNamePosition;
	//public string DropItemID;                 //掉落的道具ID
	//public int DropItemNum;                   //掉落的道具数据,一般默认为1 像金币或其他货币会用的上
	public ObscuredBool IfRoseTake;                     //道具是否被拾取
	private ObscuredBool DropNameUpdateStatus;          //掉落更新 表示执行一次
	private GameObject obj_dropName;            //掉落物体的UI
	private Vector3 vec3_droName;               //掉落道具名字在UI中的位置
	private GameObject dropItemModel;			//3D道具模型
	private ObscuredBool openStatus;					//打开状态
	public float openTime;						//打开时间
	public float openTimeSum;					//打开时间累计
	public ObscuredBool openStatusOnce;				    //打开宝箱第一次实例化UI
	public GameObject mainUIGetherItem;		    //主界面打开进度条UI
	public ObscuredString CostItemID;					//开启宝箱消耗的道具ID
	public ObscuredString CostItemNum;					//开启宝箱消耗的道具数量
	public ObscuredBool IfNeedItem;						//开启宝箱是否消耗道具
    private ObscuredString sceneItemName;               //宝箱名称
    private ObscuredString sceneItemQuality;            //宝箱品质
    private ObscuredString sceneItemType;               //场景物类型
    private ObscuredInt sceneItemApearPro;              //场景物出现的概率
    //public GameObject[] sceneItemCreateMonsterList;     //触发场景物出现怪物
    private ObscuredString sceneItemStr;
	// Use this for initialization
	void Start () {
		


        //测试场景物出现的概率
        /*
        sceneItemApearPro = 10000;
        sceneItemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Type", "ID", SceneItmeID, "SceneItem_Template");
        if ((int)(Random.value * 10000) >= sceneItemApearPro) {
            deleteScenceItem();
        }
        */
        //判定是否一次性宝箱的唯一ID
        /*
        sceneItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //Debug.Log("sceneItemStr = "+ sceneItemStr);
        if (sceneItemStr != "") {
            string[] sceneStr = sceneItemStr.Split(';');
            for (int i = 0; i <= sceneStr.Length - 1; i++)
            {
                if (SceneItemOnlyID != "0" && SceneItemOnlyID != "") {
                    //如果此宝箱唯一ID被开启过将立刻注销此宝箱
                    if (sceneStr[i] == SceneItemOnlyID)
                    {
                        deleteScenceItem();
                    }
                }
            }
        }
        */
        

		//显示道具模型
        /*
		dropItemModel = (GameObject)MonoBehaviour.Instantiate(Resources.Load("3DModel/Item/wuqi"));
		dropItemModel.transform.SetParent(this.gameObject.transform);
		dropItemModel.transform.localPosition = new Vector3(0,0,0);
		dropItemModel.transform.localRotation = new Quaternion (0, 0, 0, 0);
		dropItemModel.transform.localScale = new Vector3(0.1f, 1.0f, 0.1f);
        */

        //获取场景物出现的概率




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
			if (DropNameUpdateStatus == true)
			{
				DropNameUpdateStatus = false;
				//实例化UI
				obj_dropName = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIDropName);
				obj_dropName.SetActive(false);
				obj_dropName.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_DropItemSet.transform);
				obj_dropName.transform.localPosition = new Vector3(vec3_droName.x, vec3_droName.y, 0);
				obj_dropName.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				obj_dropName.SetActive(true);
				//显示名称
                obj_dropName.transform.Find("Lab_DropName").transform.GetComponent<Text>().color = Game_PublicClassVar.Get_function_UI.QualityReturnColor(sceneItemQuality);
                obj_dropName.transform.Find("Lab_DropName").transform.GetComponent<Text>().text = sceneItemName;
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
				//开启宝箱是否需要道具
				if(IfNeedItem){
					if(Game_PublicClassVar.Get_function_Rose.CostBagItem(CostItemID,int.Parse(CostItemNum))){
						openStatus = true;
					}else{
						string costItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", CostItemID,"Item_Template");

                        string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_365");
                        string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_366");
                        string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_367");
                        Game_PublicClassVar.Get_function_UI.GameHint(langStrHint_1 + CostItemNum+ langStrHint_2 + costItemName+ langStrHint_3);
                        //Game_PublicClassVar.Get_function_UI.GameHint("需要" + CostItemNum + "个" + costItemName + ",才能开启");
                    }
				}else{
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
                    }
				}
			}
		}

		//如果道具被拾取，注销对应的3D模型和UI
		if (openStatus) {

            Game_PublicClassVar.Get_function_UI.PlaySource("20007", "2");

            //触发掉落
            Pro_ComStr_2 pro_ComStr_2 = new Pro_ComStr_2();
            pro_ComStr_2.str_1 = Game_PublicClassVar.Get_wwwSet.ServerRoseThreadID;
            pro_ComStr_2.str_2 = ChestID.ToString();
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20001012, pro_ComStr_2);
            ChestReward();

		}
	}

    //删除
    private void OnDestroy()
    {
        if (IfRoseTake) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_123");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你的宝箱已经先手被别人抢走了！ T.T");
            Destroy(mainUIGetherItem);
        }

        if (mainUIGetherItem!=null) {
            Destroy(mainUIGetherItem);
        }
    }

    //初始化加载
    public void ChestInitia() {

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
    public void ChestReward() {
        //获取掉落ID
        string dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropID", "ID", SceneItmeID, "SceneItem_Template");
        if (dropID != "0")
        {
            Game_PublicClassVar.Get_function_AI.DropIDToDropItem(dropID, this.transform.position);
            MonoBehaviour.Destroy(obj_dropName);              //销毁UI
            MonoBehaviour.Destroy(this.gameObject);           //销毁3D模型
        }
    }

    


    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 14)
        {
            //金币
            if (sceneItemType == "3")
            {
                Debug.Log("主角碰到了");
                deleteScenceItem();
            }
        }
    }

    void deleteScenceItem() {
        MonoBehaviour.Destroy(obj_dropName);              //销毁UI
        MonoBehaviour.Destroy(this.gameObject);           //销毁3D模型
    }

    /*
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
    */
}