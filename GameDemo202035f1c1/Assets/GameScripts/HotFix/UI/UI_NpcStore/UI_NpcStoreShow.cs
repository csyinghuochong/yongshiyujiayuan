using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_NpcStoreShow : MonoBehaviour {

	public string NpcID;					//NpcID
	public GameObject Obj_StoreList;		//商品的Obj
	public GameObject Obj_StoreListNum;		//商品页数的Obj
    public GameObject Obj_SellStoreList;    //赎回商品的OBJ
	public Transform Tra_StoreListSet;
    public GameObject Obj_MoneyValue;       //显示自身金币
    public bool UpdataMoneyValue;           //更新金币显示
	private string StoreItemListText;		//商品ID的整个Text
	private string[] StoreItemList;			//商品ID数组
    private string StoreSellItemListText;   //出售商品ID的整个Text
    private string[] StoreSellItemList;     //出售商品ID数组
	private float StoreListPosition_X;		//商品显示的X坐标
	private float StoreListPosition_Y;		//商品显示的Y坐标
	private int nowStoreListNum;			//当前商品页数
	private int sumStoreListNum;			//商品总页数

	// Use this for initialization
	void Start () {

        StoreItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShopValue", "ID", NpcID, "Npc_Template");
        string ShopType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShopType", "ID", NpcID, "Npc_Template");
        switch (ShopType) { 
            //普通,配置什么出什么
            case "0":
                StoreItemList = StoreItemListText.Split(';');
            break;
            //随机出现
            case "1":
            /*
                StoreItemList = StoreItemListText.Split(';');
                StoreItemListText = "";
                for (int i = 0; i <= StoreItemList.Length - 1; i++) {
                    string[] storeItemID = StoreItemList[i].Split(',');
                    //获取概率
                    if (Random.value <= float.Parse(storeItemID[1])) {
                        StoreItemListText = StoreItemListText + storeItemID[0] + ";";
                    }
                }
                if (StoreItemListText != "") {
                    StoreItemListText = StoreItemListText.Substring(0, StoreItemListText.Length - 1);
                }
                Debug.Log("StoreItemListText = " + StoreItemListText);
                StoreItemList = StoreItemListText.Split(';');
             */
            break;
        }
        
		
		//创建第1页显示商品
		nowStoreListNum = 1;
		createStoreList (nowStoreListNum);
		//显示商品页数
		if (StoreItemList.Length <= 12) {
			sumStoreListNum = (int)StoreItemList.Length / 12;
		} else {
			sumStoreListNum = (int)StoreItemList.Length / 12+1;
		}

		Obj_StoreListNum.GetComponent<Text> ().text = "1/" + sumStoreListNum.ToString();
        UpdataMoneyValue = true;

	}
	
	// Update is called once per frame
	void Update () {

		//触发移动注销此界面
		if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status> ().RoseStatus != "1") {
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
            Btn_CloseUI();
		}

        //更新回购UI
        if (Game_PublicClassVar.Get_game_PositionVar.UpdataSellUIStatus) {
            SellItemUI();
            Game_PublicClassVar.Get_game_PositionVar.UpdataSellUIStatus = false;
            UpdataMoneyValue = true;
        }

        if (UpdataMoneyValue) {
            UpdataMoneyValue = false;
            //显示自身剩余多少金币
            Obj_MoneyValue.GetComponent<Text>().text = Game_PublicClassVar.Get_function_Rose.GetRoseMoney().ToString();
        }
	}

    void OnDestroy(){
        Game_PublicClassVar.Get_game_PositionVar.SellItemStatus = false;
        //关闭背包
        GameObject functionOpen = GameObject.FindWithTag("UI_FunctionOpen");
        if (functionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status)
        {
            functionOpen.GetComponent<UI_FunctionOpen>().OpenBag();
        }
        
    }
	//循环创建对应页数的子商品
	public void createStoreList(int storeNum){

		//循环阐述子控件
		for(int i = 0;i<Tra_StoreListSet.transform.childCount;i++)
		{
			GameObject go = Tra_StoreListSet.transform.GetChild(i).gameObject;
			Destroy(go);
		}

		int rowSum = 0;
		int createSum = 0;
		StoreListPosition_X = -180.0f;
		StoreListPosition_Y = 220.0f;
		//循环创建
		for(int i = (storeNum-1)*12;i<=StoreItemList.Length-1;i++){
			if(StoreItemList[i]!=""){
				rowSum = rowSum +1;
				
				//实例化商店栏
				GameObject obj_NpcStore = (GameObject)MonoBehaviour.Instantiate(Obj_StoreList);
				obj_NpcStore.transform.SetParent(Tra_StoreListSet);
				obj_NpcStore.transform.localPosition = new Vector3(StoreListPosition_X, StoreListPosition_Y, 0);
				obj_NpcStore.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                string[] storeItemListStr = StoreItemList[i].Split(',');
                if (storeItemListStr.Length >= 2)
                {
                    obj_NpcStore.GetComponent<UI_StoreList>().ItemID = storeItemListStr[0];
                    obj_NpcStore.GetComponent<UI_StoreList>().ItemNum = storeItemListStr[1];
                }
                else {
                    obj_NpcStore.GetComponent<UI_StoreList>().ItemID = StoreItemList[i];
                    obj_NpcStore.GetComponent<UI_StoreList>().ItemNum = "1";
                }
				
				
				StoreListPosition_X = 50.0f;
				//设置下个生成的坐标点
				if(rowSum==2){
					StoreListPosition_X = -180.0f;
					StoreListPosition_Y = StoreListPosition_Y -77.0f;
					rowSum = 0;
				}
			}
			
			//循环创建12个，满足12个立即跳出当前循环
			createSum = createSum + 1;
			if(createSum>=12){
				i = StoreItemList.Length;	//立即跳出循环
			}
		}
	}

    //循环创建回购道具
    public void createSellList(int storeNum)
    {

        //循环阐述子控件
        for (int i = 0; i < Tra_StoreListSet.transform.childCount; i++)
        {
            GameObject go = Tra_StoreListSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        int rowSum = 0;
        int createSum = 0;
        StoreListPosition_X = -180.0f;
        StoreListPosition_Y = 220.0f;
        //循环创建
        for (int i = (storeNum - 1) * 12; i <= StoreSellItemList.Length - 1; i++)
        {
            string[] StoreSellItem = StoreSellItemList[i].Split(',');

            if (StoreSellItem[0] != "")
            {
                rowSum = rowSum + 1;

                //实例化商店栏
                GameObject obj_NpcStore = (GameObject)MonoBehaviour.Instantiate(Obj_SellStoreList);
                obj_NpcStore.transform.SetParent(Tra_StoreListSet);
                obj_NpcStore.transform.localPosition = new Vector3(StoreListPosition_X, StoreListPosition_Y, 0);
                obj_NpcStore.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                obj_NpcStore.GetComponent<UI_SellStoreList>().ItemID = StoreSellItem[0];
                obj_NpcStore.GetComponent<UI_SellStoreList>().SellNum = StoreSellItem[1];
                obj_NpcStore.GetComponent<UI_SellStoreList>().HideID = StoreSellItem[2];

                StoreListPosition_X = 50.0f;
                //设置下个生成的坐标点
                if (rowSum == 2)
                {
                    StoreListPosition_X = -180.0f;
                    StoreListPosition_Y = StoreListPosition_Y - 77.0f;
                    rowSum = 0;
                }
            }

            //循环创建12个，满足12个立即跳出当前循环
            createSum = createSum + 1;
            if (createSum >= 12)
            {
                i = StoreSellItemList.Length;	//立即跳出循环
            }
        }
    }

	//显示上一页
	public void StoreList_Up(){
		if (nowStoreListNum > 1) {
			nowStoreListNum = nowStoreListNum - 1;
			createStoreList(nowStoreListNum);
			Obj_StoreListNum.GetComponent<Text> ().text = nowStoreListNum + "/" + sumStoreListNum.ToString();
		}
	}

	//显示下一页
	public void StoreList_Down(){
		if (nowStoreListNum < sumStoreListNum){
			nowStoreListNum = nowStoreListNum + 1;
			createStoreList(nowStoreListNum);
			Obj_StoreListNum.GetComponent<Text> ().text = nowStoreListNum + "/" + sumStoreListNum.ToString();
		}
	}

    //点击出售按钮
    public void SellItemUI() {


        //读取当前卖的道具列表
        StoreSellItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        StoreSellItemList = StoreSellItemListText.Split(';');

        //显示商品页数
        if (StoreSellItemList.Length <= 12)
        {
            sumStoreListNum = (int)StoreItemList.Length / 12;
        }
        else
        {
            sumStoreListNum = (int)StoreItemList.Length / 12 + 1;
        }

        Obj_StoreListNum.GetComponent<Text>().text = "1/" + sumStoreListNum.ToString();

        createSellList(nowStoreListNum);
        //Game_PublicClassVar.Get_game_PositionVar.SellItemStatus = true;

        //打开背包
        GameObject functionOpen = GameObject.FindWithTag("UI_FunctionOpen");
        if (!functionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status) {
            functionOpen.GetComponent<UI_FunctionOpen>().OpenBag();
        }

    }

    //点击商品列表
    public void BuyItemUI() {

        //关闭出售状态
        Game_PublicClassVar.Get_game_PositionVar.SellItemStatus = false;

        StoreItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShopValue", "ID", NpcID, "Npc_Template");
        StoreItemList = StoreItemListText.Split(';');

        //创建第1页显示商品
        nowStoreListNum = 1;
        createStoreList(nowStoreListNum);
        //显示商品页数
        if (StoreItemList.Length <= 12)
        {
            sumStoreListNum = (int)StoreItemList.Length / 12;
        }
        else
        {
            sumStoreListNum = (int)StoreItemList.Length / 12 + 1;
        }
        Obj_StoreListNum.GetComponent<Text>().text = "1/" + sumStoreListNum.ToString();
    }

    public void Btn_CloseUI() {
        Game_PublicClassVar.Get_function_UI.PlaySource("10002", "1");

        //退出时,镜头切换
        string ifCameraLaJin = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfCameraLaJin", "ID", NpcID, "Npc_Template");
        if (ifCameraLaJin == "1")
        {
            Camera.main.GetComponent<CameraAI>().BuildEnterStatus = false;
            Camera.main.GetComponent<CameraAI>().BuildExitStatus = true;
        }
        Destroy(this.gameObject);
    }
}
