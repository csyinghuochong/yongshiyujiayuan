using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Data;
using System;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_NpcStoreShow_2 : MonoBehaviour
{

	public ObscuredString NpcID;					//NpcID
    public GameObject Obj_NpcStoreShow_2Set;
    public GameObject Obj_NpcStoreShowNpcImg;
    public GameObject Obj_StoreList;		        //商品的Obj
	public Transform Tra_StoreListSet;
    public GameObject Obj_MoneyValue;               //显示自身金币
    public bool UpdataMoneyValue;                   //更新金币显示
	private ObscuredString StoreItemListText;		//商品ID的整个Text
	//private string[] StoreItemList;			        //商品ID数组
    private ObscuredString StoreSellItemListText;   //出售商品ID的整个Text
    //private string[] StoreSellItemList;             //出售商品ID数组

    public GameObject Obj_NullHintText;
    public GameObject Obj_ShuaXinCostZuanShi;
    public GameObject Obj_ShuaXinSet;
    public ObscuredBool ShuaXinStatus;
    private ObscuredInt ShuaXinCostRmbNum;

	// Use this for initialization
	void Start () {

        //更新商店
        updateStore();

        //根据NPCID额外显示货币
        string otherItemID = "";
        switch (NpcID) {

            //回收商人
            case "22000008":
                otherItemID = "10010086";
                break;

            //大秘境
            case "21000023":
                otherItemID = "10010088";
                break;
        }

        //打开通用UI
        string ShopType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShopType", "ID", NpcID, "Npc_Template");
        if (ShopType == "1")
        {
            Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject, "602", otherItemID);
            Obj_ShuaXinSet.SetActive(true);
            ShuaXinCostRmbNum = 420;
            Obj_ShuaXinCostZuanShi.GetComponent<Text>().text = ShuaXinCostRmbNum.ToString();

        }
        else {
            Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject, "601", otherItemID);
            Obj_ShuaXinSet.SetActive(false);
        }


        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_NpcStoreShow_2Set);
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_NpcStoreShowNpcImg);




    }
	
	// Update is called once per frame
	void Update () {

		//触发移动注销此界面
        /*
		if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status> ().RoseStatus != "1") {
			Destroy(this.gameObject);
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
		}
        */
        if (UpdataMoneyValue) {
            UpdataMoneyValue = false;
            //显示自身剩余多少金币
            Obj_MoneyValue.GetComponent<Text>().text = Game_PublicClassVar.Get_function_Rose.GetRoseMoney().ToString();
        }
	}

    

    void OnDestroy(){

        Game_PublicClassVar.Get_game_PositionVar.SellItemStatus = false;

    }


    //重置黑市
    public void Btn_ChongZhiHeiShi() {

        //获取次数
        string nowShuaXinNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeiShiShuaXinNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (nowShuaXinNum == "" || nowShuaXinNum == null) {
            nowShuaXinNum = "0";
        }

        int nowNum = int.Parse(nowShuaXinNum);

        int numMax = Game_PublicClassVar.Get_function_Rose.ReturnHeiShiUpdateNum();

        if (nowNum >= numMax) {
            Game_PublicClassVar.Get_function_UI.GameHint("今日刷新次数已用完,提升冒险家等级可以增加次数!");
            return;
        }

        //刷新钻石
        if (Game_PublicClassVar.Get_function_Rose.GetRoseRMB() >= ShuaXinCostRmbNum) {

            Game_PublicClassVar.Get_function_Rose.CostReward("2", ShuaXinCostRmbNum.ToString());

            //设置刷新
            nowNum = nowNum + 1;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HeiShiShuaXinNum", nowNum.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            //执行刷新
            ShuaXinStatus = true;
            updateStore();
        }

    }

    //更新商店
    public void updateStore() {

        StoreItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShopValue", "ID", NpcID, "Npc_Template");
        string ShopType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShopType", "ID", NpcID, "Npc_Template");
        switch (ShopType)
        {
            //普通,配置什么出什么
            case "0":
                string[] StoreItemList = StoreItemListText.ToString().Split(';');
                break;
            //随机出现
            case "1":
                //读取当前配置
                StoreItemListText = ""; //  清空一下值
                string stroeValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value_1", "ID", "Store_" + NpcID, "RoseOtherData");
                if (stroeValue == "0")
                {
                    //表示没有数据,需要创建新的商店随机库
                    StoreItemListText = Game_PublicClassVar.Get_function_UI.CreateRandomStore(NpcID);
                    //Debug.Log("StoreItemListText111=" + StoreItemListText);
                    //创建数据
                    Game_PublicClassVar.Get_function_DataSet.AddRandomStoreXml(NpcID, StoreItemListText);
                }
                else
                {

                    bool ifUpdate = false;

                    //判定当前是否需要重置商店数据
					string timeStr = Game_PublicClassVar.Get_wwwSet.enterGameTimeStamp;
					//连接不上服务器,显示本地时间戳
					if (timeStr == "") {
						timeStr = Game_PublicClassVar.Get_function_DataSet.GetTimeStamp ();
						//Debug.Log ("timeStr = " + timeStr);
					}
					int nowTime = int.Parse(timeStr) + (int)(Time.realtimeSinceStartup);
                    DateTime nowDataTime = Game_PublicClassVar.Get_wwwSet.GetTime(nowTime.ToString());

                    string value_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value_2", "ID", "Store_" + NpcID, "RoseOtherData");
                    DateTime lastDataTime = Game_PublicClassVar.Get_wwwSet.GetTime(value_2);
					
                    //对比年份
                    if (nowDataTime.Year > lastDataTime.Year)
                    {
                        ifUpdate = true;
                    }
                    else
                    {
                        //对比月份
                        if (nowDataTime.Month > lastDataTime.Month)
                        {
                            ifUpdate = true;
                        }
                        else
                        {
                            //对比天数
                            if (nowDataTime.Day > lastDataTime.Day)
                            {
                                ifUpdate = true;
                            }
                            else
                            {
                                //获取小时
                                int nowHour = nowDataTime.Hour;
                                int lastHour = lastDataTime.Hour;
                                //8点  12点  20点   24点刷新
                                //0-7小时59分
                                if (nowHour >= 0 && nowHour < 8)
                                {
                                    if (ifDuiBiValue(lastHour, 0, 8) == false)
                                    {
                                        ifUpdate = true;
                                    }
                                }
                                //8-11小时59分
                                if (nowHour >= 8 && nowHour < 12)
                                {
                                    if (ifDuiBiValue(lastHour, 8, 12) == false)
                                    {
                                        ifUpdate = true;
                                    }
                                }
                                //12-19小时59分
                                if (nowHour >= 12 && nowHour < 20)
                                {
                                    if (ifDuiBiValue(lastHour, 12, 20) == false)
                                    {
                                        ifUpdate = true;
                                    }
                                }
                                //20-23小时59分
                                if (nowHour >= 20 && nowHour < 24)
                                {
                                    if (ifDuiBiValue(lastHour, 20, 24) == false)
                                    {
                                        ifUpdate = true;
                                    }
                                }
                            }
                        }
                    }

                    //每6小时固定刷新,额外写的,暂时不用
                    /*
                    TimeSpan timeCha = nowDataTime - lastDataTime;
                    if (timeCha.TotalHours >= 6)
                    {
                        ifUpdate = true;
                    }
                    */

                    //强制刷新
                    if (ShuaXinStatus) {
                        ifUpdate = true;
                        ShuaXinStatus = false;
                    }

                    //重置数据
                    if (ifUpdate)
                    {
                        //获取商店道具
                        StoreItemListText = Game_PublicClassVar.Get_function_UI.CreateRandomStore(NpcID);
                        //Debug.Log("StoreItemListText222=" + StoreItemListText);
                        //更新数据
                        Game_PublicClassVar.Get_function_DataSet.UpdateRandomStoreXml(NpcID, StoreItemListText);
                    }
                    else
                    {
                        //读取数据进行显示
                        if (stroeValue != "" && stroeValue != "0")
                        {
                            StoreItemListText = stroeValue;
                        }
                    }
                }

                //Debug.Log("StoreItemListText = " + StoreItemListText);
                StoreItemList = StoreItemListText.ToString().Split(';');

                break;

            //等级出现
            case "2":
                StoreItemList = StoreItemListText.ToString().Split(';');
                StoreItemListText = "";
                for (int i = 0; i <= StoreItemList.Length - 1; i++)
                {
                    string[] storeItemIDList = StoreItemList[i].Split(',');
                    //获取配置数据
                    int MinLv = int.Parse(storeItemIDList[0]);
                    int MaxLv = int.Parse(storeItemIDList[1]);
                    string storeItemID = storeItemIDList[2];
                    float storeItemNum = float.Parse(storeItemIDList[3]);
                    //float storeItemNum = 1;
                    int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                    if (MinLv <= roseLv && roseLv < MaxLv)
                    {
                        StoreItemListText = StoreItemListText + storeItemID + "," + storeItemNum + ";";
                        //Debug.Log("StoreItemListText = " + StoreItemListText);
                    }
                }
                if (StoreItemListText != "")
                {
                    StoreItemListText = StoreItemListText.ToString().Substring(0, StoreItemListText.Length - 1);
                }
                StoreItemList = StoreItemListText.ToString().Split(';');
                break;


            //等级出现
            case "3":
                StoreItemList = StoreItemListText.ToString().Split(';');
                //Debug.Log("StoreItemListText = " + StoreItemListText);
                StoreItemListText = "";
                for (int i = 0; i <= StoreItemList.Length - 1; i++)
                {
                    string[] storeItemIDList = StoreItemList[i].Split(',');
                    if (storeItemIDList.Length >= 4)
                    {
                        //获取配置数据
                        int MinLv = int.Parse(storeItemIDList[0]);
                        int MaxLv = int.Parse(storeItemIDList[1]);
                        string storeItemID = storeItemIDList[2];
                        float storeItemNum = float.Parse(storeItemIDList[3]);
                        //float storeItemNum = 1;
                        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                        if (MinLv <= roseLv && roseLv < MaxLv)
                        {
                            StoreItemListText = StoreItemListText + storeItemID + "," + storeItemNum + "," + storeItemIDList[4] + "," + storeItemIDList[5] + ";";
                        }
                    }
                }
                if (StoreItemListText != "")
                {
                    StoreItemListText = StoreItemListText.ToString().Substring(0, StoreItemListText.Length - 1);
                }
                StoreItemList = StoreItemListText.ToString().Split(';');
                break;
        }

        createStoreList();
        UpdataMoneyValue = true;

    }

	//循环创建对应页数的子商品
	public void createStoreList(){

		//循环阐述子控件
		for(int i = 0;i<Tra_StoreListSet.transform.childCount;i++)
		{
			GameObject go = Tra_StoreListSet.transform.GetChild(i).gameObject;
			Destroy(go);
		}

        Obj_NullHintText.SetActive(false);
        string[] StoreItemList = StoreItemListText.ToString().Split(';');
        if (StoreItemList.Length<=1) {
            if (StoreItemList[0] == "")
            {
                Obj_NullHintText.SetActive(true);
                Obj_NullHintText.GetComponent<Text>().text = "当前道具已售空,请耐心等待商人补货！";
            }
        }

		//循环创建
		for(int i = 0;i<StoreItemList.Length;i++){
			if(StoreItemList[i]!=""){
				//实例化商店栏
				GameObject obj_NpcStore = (GameObject)MonoBehaviour.Instantiate(Obj_StoreList);
				obj_NpcStore.transform.SetParent(Tra_StoreListSet);
                obj_NpcStore.transform.localScale = new Vector3(1, 1, 1);
                //Debug.Log("StoreItemList[i] = " + StoreItemList[i]);
                string[] storeItemListStr = StoreItemList[i].Split(',');
                if (storeItemListStr.Length >= 2)
                {
                    obj_NpcStore.GetComponent<UI_StoreList_2>().ItemID = storeItemListStr[0];
                    obj_NpcStore.GetComponent<UI_StoreList_2>().ItemNum = storeItemListStr[1];

                    //如果类型为1则有限制购买和购买类型
                    string ShopType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShopType", "ID", NpcID, "Npc_Template");
                    if (ShopType == "1") {
                        obj_NpcStore.GetComponent<UI_StoreList_2>().BuyNum = int.Parse(storeItemListStr[2]);
                        obj_NpcStore.GetComponent<UI_StoreList_2>().BuyType = storeItemListStr[3];
                        obj_NpcStore.GetComponent<UI_StoreList_2>().BuySpace = i;
                    }

                    //等级显示,消耗金币购买
                    if (ShopType == "2")
                    {
                        obj_NpcStore.GetComponent<UI_StoreList_2>().BuyType = "0";
                    }


                    //等级显示,消耗指定道具购买对应道具
                    if (ShopType == "3")
                    {
                        obj_NpcStore.GetComponent<UI_StoreList_2>().BuyType = "3";
                        obj_NpcStore.GetComponent<UI_StoreList_2>().BuyUseItem = storeItemListStr[2];
                        obj_NpcStore.GetComponent<UI_StoreList_2>().BuyUseItemNum = int.Parse(storeItemListStr[3]);
                    }
                }
                else {
                    obj_NpcStore.GetComponent<UI_StoreList_2>().ItemID = StoreItemList[i];
                    obj_NpcStore.GetComponent<UI_StoreList_2>().ItemNum = "1";
                    //如果类型为1则有限制购买和购买类型

                }
			}
		}

        Tra_StoreListSet.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (int)(StoreItemList.Length/2)*150);// StoreItemList.Length
	}

    /*
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
    */
    //点击出售按钮
    public void SellItemUI() {

        /*
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

        createSellList(nowStoreListNum);
        //Game_PublicClassVar.Get_game_PositionVar.SellItemStatus = true;

        //打开背包
        GameObject functionOpen = GameObject.FindWithTag("UI_FunctionOpen");
        if (!functionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status) {
            functionOpen.GetComponent<UI_FunctionOpen>().OpenBag();
        }
        */
    }

    //点击商品列表
    public void BuyItemUI() {

        //关闭出售状态
        Game_PublicClassVar.Get_game_PositionVar.SellItemStatus = false;

        StoreItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShopValue", "ID", NpcID, "Npc_Template");
        string[] StoreItemList = StoreItemListText.ToString().Split(';');
        StoreItemList = StoreItemListText.ToString().Split(';');

        //创建第1页显示商品
        createStoreList();
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

    //判定一个值是否在两个值得区间
    public bool ifDuiBiValue(int duibiValue, int value_1, int value_2)
    {
        if (duibiValue >= value_1) {
            if (duibiValue < value_2) {
                return true;
            }
        }
        return false;
    }
}
