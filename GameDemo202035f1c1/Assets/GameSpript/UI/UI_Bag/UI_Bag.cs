using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Data;
using System.Collections.Generic;

public class UI_Bag : MonoBehaviour {

    public GameObject Obj_BagSpace;                 //动态创建的格子
    public Transform Tra_BagSpaceList;              //动态创建格子的绑点
	public GameObject Obj_BagGoldNum;				//背包金币
	public GameObject Obj_BagFuzhong;				//背包当前负重
    public GameObject Obj_BagZuanShi;               //背包当前钻石
    private float bagSpacePosition_X;               //动态创建格子的X轴位置
    private float bagSpacePosition_Y;               //动态创建格子的Y轴位置
    private int creatSpaceNum;                      //动态创建的格子数
    public GameObject Obj_YiJianSellItem;           //一键出售Obj
    private GameObject yiJianSellItemObj;           //一键出售
    public bool BagMoveStatus;                      //背包移动状态
    public GameObject Obj_Scelect_1;                //移动选中
    public GameObject Obj_Scelect_2;                //移动选中

    public bool ZhengLiStatus;
    // Use this for initialization
    void Start () {

        //缓存一下背包数据
        DataTable dataTable;
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseBag.xml", "RoseBag");
        Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables.Remove("RoseBag");   //先将背包数据移除在进行添加
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables.Add(dataTable);

        BagMoveStatus = false;

		//更新背包货币数据
		UpdataBagMoney ();
        
        //动态创建格子行
        //for (int z = 1; z <= 10; z++) {

            //动态创建每行中的格子
            for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
            {
                //开始创建背包格子
                GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
                bagSpace.transform.SetParent(Tra_BagSpaceList);
				bagSpace.transform.localScale = new Vector3(1,1,1);
                //bagSpace.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(bagSpacePosition_X, bagSpacePosition_Y, 0);
                //bagSpacePosition_X = bagSpacePosition_X + 86.0f;
                //creatSpaceNum = creatSpaceNum + 1;
                bagSpace.GetComponent<UI_BagSpace>().BagPosition = i.ToString();
                bagSpace.GetComponent<UI_BagSpace>().SpaceType = "1";   //设置格子为背包属性
            }

            //每行创建完数据清0
            //bagSpacePosition_Y = bagSpacePosition_Y - 86.0f;
            //bagSpacePosition_X = 0.0f;
        
        //}

        //更新状态
        Btn_MoveBagStatus();

        //进入主城重置参数
        Game_PublicClassVar.Get_game_PositionVar.UseItemStatus = false;

    }	
	
	// Update is called once per frame
	void Update () {
		
		if (Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll) {
			//更新背包货币数据
			UpdataBagMoney();
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
		Destroy (this.gameObject);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status = false;
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
	}

	public void UpdataBagMoney(){
		//获取当前金币
		string gold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Obj_BagGoldNum.GetComponent<Text>().text = gold;
        string rmb = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_BagZuanShi.GetComponent<Text>().text = rmb;
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

        if (Game_PublicClassVar.Get_game_PositionVar.UseItemStatus) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前正在整理道具,请稍后点击..");
            return;
        }

        if (ZhengLiStatus == false)
        {
            ZhengLiStatus = true;

            Game_PublicClassVar.Get_function_Rose.RoseArrangeBag();
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前正在整理背包,请稍后点击..");
            return;
        }

        ZhengLiStatus = false;

        //Dictionary<string, string> dic = new Dictionary<string, string>();
        //dic.Add("zhengli: ", "10");
        //dic.Add("chushou: ", "20");
        //dic.Add("shiyong: ", "30");
        //UmengController.Instance.Event( "bagaction: ", dic);
    }

    //快捷出售
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
                if (bagSpaceObj.GetComponent<UI_BagSpace>() != null) {
                    bagSpaceObj.GetComponent<UI_BagSpace>().MoveBagStatus = false;
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
                if (bagSpaceObj.GetComponent<UI_BagSpace>() != null)
                {
                    bagSpaceObj.GetComponent<UI_BagSpace>().MoveBagStatus = true;
                }
            }
            if (Obj_Scelect_1 != null)
            {
                Obj_Scelect_1.SetActive(false);
                Obj_Scelect_2.SetActive(true);
            }
        }

        if (Game_PublicClassVar.Get_game_PositionVar.EquipPropertyMoveStatus|| Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus) {

            for (int i = 0; i < Tra_BagSpaceList.transform.childCount; i++)
            {
                GameObject bagSpaceObj = Tra_BagSpaceList.transform.GetChild(i).gameObject;
                if (bagSpaceObj.GetComponent<UI_BagSpace>() != null)
                {
                    bagSpaceObj.GetComponent<UI_BagSpace>().MoveBagStatus = false;
                    string ItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceObj.GetComponent<UI_BagSpace>().BagPosition, "RoseBag");
                    string itemType = Game_PublicClassVar.function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");
                    if (itemType != "3")
                    {
                        Destroy(bagSpaceObj);
                        //Debug.Log("删除...");
                    }
                    else {
                        string itemSubType = Game_PublicClassVar.function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");
                        //清理生肖显示
                        if (int.Parse(itemSubType) >= 101 && int.Parse(itemSubType) <= 112) {
                            Destroy(bagSpaceObj);
                        }
                        //清理宠物装备显示
                        if (int.Parse(itemSubType) >= 201 && int.Parse(itemSubType) <= 203)
                        {
                            Destroy(bagSpaceObj);
                        }
                    }
                }
            }
        }
    
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
