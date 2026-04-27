using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Data;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureBag : MonoBehaviour {

    public GameObject Obj_BagSpace;                 //动态创建的格子
    public Transform Tra_BagSpaceList;              //动态创建格子的绑点
    private ObscuredFloat bagSpacePosition_X;               //动态创建格子的X轴位置
    private ObscuredFloat bagSpacePosition_Y;               //动态创建格子的Y轴位置
    private ObscuredInt creatSpaceNum;                      //动态创建的格子数
    public GameObject Obj_YiJianSellItem;           //一键出售Obj
    private GameObject yiJianSellItemObj;           //一键出售
    public bool BagMoveStatus;                      //背包移动状态
    public GameObject Obj_Scelect_1;                //移动选中
    public GameObject Obj_Scelect_2;                //移动选中
    public GameObject[] Obj_SellPinZhiList;
    private int sellPinZhiValue;
    private float UpdateShowTime;
    public GameObject Obj_PastureGold;
    public GameObject Obj_RoseRmb;
    public bool UpdateShow;
    // Use this for initialization
    void Start () {

        //缓存一下背包数据
        DataTable dataTable;
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RosePastureBag.xml", "RosePastureBag");
        Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables.Remove("RosePastureBag");   //先将背包数据移除在进行添加
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables.Add(dataTable);

        BagMoveStatus = false;

		//更新背包货币数据
		UpdataBagMoney ();

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Tra_BagSpaceList.gameObject);

        //动态创建每行中的格子
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePastureBagMaxNum; i++)
        {
            //开始创建背包格子
            GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
            bagSpace.transform.SetParent(Tra_BagSpaceList);
			bagSpace.transform.localScale = new Vector3(1,1,1);
            //bagSpace.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(bagSpacePosition_X, bagSpacePosition_Y, 0);
            //bagSpacePosition_X = bagSpacePosition_X + 86.0f;
            //creatSpaceNum = creatSpaceNum + 1;
            bagSpace.GetComponent<UI_PastureBagSpace>().BagPosition = i.ToString();
            bagSpace.GetComponent<UI_PastureBagSpace>().SpaceType = "1";   //设置格子为背包属性
        }

        //更新状态
        Btn_MoveBagStatus();

        //初始化
        Btn_SellYiJianXuanZe("1");

    }	
	
	// Update is called once per frame
	void Update () {
		
		if (Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll) {
			//更新背包货币数据
			UpdataBagMoney();
		}

        UpdateShowTime = UpdateShowTime + Time.deltaTime;
        if (UpdateShowTime >= 0.1f)
        {
            UpdateShow = true;
        }

        if (UpdateShow)
        {
            UpdateShow = false;
            if (Obj_PastureGold != null) {
                string nowPastureGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureGold", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                Obj_PastureGold.GetComponent<Text>().text = nowPastureGold;
            }
            if (Obj_RoseRmb != null) {
                int nowRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
                Obj_RoseRmb.GetComponent<Text>().text = nowRmb.ToString();
            }
        }
    }

    //被销毁时调用
    void OnDisable() {
        //此处会有一个错误,当开启背包UI，强制关闭游戏时,此处会报错，因为找不到对象
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose != null) {
            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_UIStatus>().RoseBag_Status = false;
        }
    }

	public void CloseUI(){

		Destroy (this.transform.parent.parent.gameObject);
		
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
	}

	public void UpdataBagMoney(){

	}

    public void CloseItemTips() {
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
    }

    //整理背包
    public void Btn_ArrangeBag()
    {
        Game_PublicClassVar.Get_function_Pasture.PastureBagArrangeBag();
    }

    //快捷出售
    /*
    public void Btn_YiJianSellItem() 
    {
        if (yiJianSellItemObj == null)
        {
            //实例化一个一键出售OBJ
            yiJianSellItemObj = (GameObject)Instantiate(Obj_YiJianSellItem);
            yiJianSellItemObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            yiJianSellItemObj.transform.localScale = new Vector3(1, 1, 1);
            yiJianSellItemObj.transform.localPosition = new Vector3(0, 0, 0);
        }
        else {
            Destroy(yiJianSellItemObj);
        }
    }
    */

    //背包移动状态
    public void Btn_MoveBagStatus() {
        //Debug.Log("Btn_MoveBagStatusBtn_MoveBagStatusBtn_MoveBagStatus");
        if (BagMoveStatus)
        {
            //背包不能移动
            BagMoveStatus = false;

            for (int i = 0; i < Tra_BagSpaceList.transform.childCount; i++)
            {
                GameObject bagSpaceObj = Tra_BagSpaceList.transform.GetChild(i).gameObject;
                if (bagSpaceObj.GetComponent<UI_PastureBagSpace>() != null) {
                    bagSpaceObj.GetComponent<UI_PastureBagSpace>().MoveBagStatus = false;
                }
            }

            if (Obj_Scelect_1 != null) {
                Obj_Scelect_1.SetActive(true);
                Obj_Scelect_2.SetActive(false);
            }
        }
        else {
            //背包移动
            BagMoveStatus = true;

            for (int i = 0; i < Tra_BagSpaceList.transform.childCount; i++)
            {
                GameObject bagSpaceObj = Tra_BagSpaceList.transform.GetChild(i).gameObject;
                if (bagSpaceObj.GetComponent<UI_PastureBagSpace>() != null)
                {
                    bagSpaceObj.GetComponent<UI_PastureBagSpace>().MoveBagStatus = true;
                }
            }
            if (Obj_Scelect_1 != null)
            {
                Obj_Scelect_1.SetActive(false);
                Obj_Scelect_2.SetActive(true);
            }
        }

        if (Game_PublicClassVar.Get_game_PositionVar.EquipPropertyMoveStatus) {

            for (int i = 0; i < Tra_BagSpaceList.transform.childCount; i++)
            {
                GameObject bagSpaceObj = Tra_BagSpaceList.transform.GetChild(i).gameObject;
                if (bagSpaceObj.GetComponent<UI_PastureBagSpace>() != null)
                {
                    bagSpaceObj.GetComponent<UI_PastureBagSpace>().MoveBagStatus = false;
                    string ItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceObj.GetComponent<UI_BagSpace>().BagPosition, "RosePastureBag");
                    string itemType = Game_PublicClassVar.function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");
                    if (itemType != "3") {
                        Destroy(bagSpaceObj);
                        Debug.Log("删除...");
                    }
                }
            }
        }
    }
    
    //一键出售
    public void Btn_YiJianSell() {

        if (sellPinZhiValue <= 0) {
            return;
        }

        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePastureBagMaxNum; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RosePastureBag");
            if (Rdate != "" && Rdate != "0")
            {
                string pinzhiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", i.ToString(), "RosePastureBag");
                int pinZhiValue = int.Parse(pinzhiStr);
                if (pinZhiValue <= sellPinZhiValue) {
                    Game_PublicClassVar.Get_function_Pasture.SellPastureBagSpaceItemToMoney(i.ToString(), false);
                }
            }
        }

        //存储数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureBag");

    }

    //出售
    public void Btn_SellYiJianXuanZe(string target) {

        if (Obj_SellPinZhiList.Length == 0)
        {
            return;
        }

        for (int i = 0; i < Obj_SellPinZhiList.Length; i++) {
            Obj_SellPinZhiList[i].SetActive(false);
        }

        if (target == "1") {
            sellPinZhiValue = 20;
            Obj_SellPinZhiList[0].SetActive(true);
        }

        if (target == "2")
        {
            sellPinZhiValue = 50;
            Obj_SellPinZhiList[1].SetActive(true);
        }

        if (target == "3")
        {
            sellPinZhiValue = 100;
            Obj_SellPinZhiList[2].SetActive(true);
        }
    }


    //兑换
    public void Btn_DuiHuan()
    {

        //获取数据
        string PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        int duiHuanCostZuanShi = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanCostZuanShi", "ID", PastureID, "PastureUpLv_Template"));
        int duiHuanGetPastureGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanGetPastureGold", "ID", PastureID, "PastureUpLv_Template"));
        //Obj_DuiHuanShow.GetComponent<Text>().text = "";

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否消耗" + duiHuanCostZuanShi + "钻石兑换" + duiHuanGetPastureGold + "家园资金", Game_PublicClassVar.Get_function_Pasture.DuiHuanPastureGold, Game_PublicClassVar.Get_function_Pasture.DuiHuanPastureGold_Ten, "系统提示", "兑换一次", "兑换十次", null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    //增加钻石
    public void Btn_AddZuanShi()
    {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_12");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否前往购买钻石界面", GoToGoToDuiHuanGold, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否前往兑换金币界面？", GoToGoToDuiHuanGold, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }


    public void GoToGoToDuiHuanGold()
    {
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing == null)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().ClearnOpenUI();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_HuoDongDaTing();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().IfInitStatus = false;
        }
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().Btn_GamePay();
    }

    //移动清理
    /*
    public void Btn_ClearnMoveData() {
        
        Debug.Log("离开了！");
        //如果移动装备开关打开放入对应的装备
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
            //触发移动
            //Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
            Debug.Log("清空数据");
        }
        
    }
     */
}
