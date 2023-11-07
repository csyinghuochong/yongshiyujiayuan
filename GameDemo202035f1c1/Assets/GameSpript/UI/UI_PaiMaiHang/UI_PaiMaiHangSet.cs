using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PaiMaiHangSet : MonoBehaviour {

	public GameObject Obj_PaiMaiHangSet;
	public GameObject Obj_PaiMaiHangListSet;
	public GameObject Obj_PaiMaiHangSaleSet;
	public GameObject Obj_PaiMaiHangDuiHuanSet;
	public GameObject Obj_PaiMaiHangItemShow;
	public GameObject Obj_UIPaiMaiHangList_Type;
	public GameObject Obj_UIPaiMaiHangZhangJie_Row;
	public bool OpenPaiMaiHangZhangJie_1;
	public bool OpenPaiMaiHangZhangJie_2;
	public bool OpenPaiMaiHangZhangJie_3;
	public bool OpenPaiMaiHangZhangJie_4;
	public bool OpenPaiMaiHangZhangJie_5;
	public GameObject[] OpenPaiMaiHangZhangJieList;
	public bool Rose_PaiMaiHangList_Update;
	public bool PaiMaiHangXuanZhongStatus;
	private ObscuredInt PaiMaiHangXuanZhongStatusNum;
	public GameObject PaiMaiHangXuanZhongObj;
	public GameObject Obj_PaiMaiHangTypePro;
	public GameObject UIPoint_PaiMaiHangType;

	public GameObject Obj_Btn_Buy_Img;
	public GameObject Obj_Btn_Buy_Text;
	public GameObject Obj_Btn_Sale_Img;
	public GameObject Obj_Btn_Sale_Text;
	public GameObject Obj_Btn_DuiHuan_Img;
	public GameObject Obj_Btn_DuiHuan_Text;

	public bool ShowFirstObjStatus;
	public GameObject Obj_PaiMaiItemShow;
	public Dictionary<string, Pro_PaiMaiDataAdd> PaimaiItemShow = new Dictionary<string, Pro_PaiMaiDataAdd>();

    public bool IfInitStatus = true;

	public ObscuredInt BuyItemNum;
	public ObscuredInt BuyItemAllGold;

	public GameObject Obj_BuyItemNum;
	public GameObject Obj_BuyItemAllGold;

	public bool PaiMaiHangXuanZhongItemListStatus;
	public GameObject Obj_PaiMaiHangXuanZhongItemList;
	private ObscuredInt PaiMaiHangXuanZhongItemListNum;

	public ObscuredString nowSeleBuyItemID;
    private ObscuredString paimaiID_ChuShi;                 //初始选中的拍卖ID
    private ObscuredFloat paimaiShowList_Y_ChuShi;

	//出售
	public GameObject Obj_BagSpace;                 //动态创建的背包格子
	public GameObject Obj_BagSpaceListSet;
	public GameObject Obj_ChuShouItemNum;
	public ObscuredInt ChuShouItemNum;
	//public string LastBagSpaceNum;
	public bool UpdateChuShouItemNumStatus;

	public GameObject Obj_ChuShouListSet;
	public GameObject Obj_ChuShouListItemShow;
	public GameObject Obj_ItemShengYuTime;

	public bool ChuShouXuanZhongItemListStatus;
	public GameObject Obj_ChuShouXuanZhongItemList;
	private ObscuredInt ChuShouXuanZhongItemListNum;

	public GameObject Obj_ChuShouItemShow;
	public GameObject Obj_ChuShouItemShowSet;

	public Dictionary<string, Pro_PaiMai_PlayerSellAdd> ChuShouItemShow = new Dictionary<string, Pro_PaiMai_PlayerSellAdd>();
    public Dictionary<string, GameObject> ChuShouItemShowObj = new Dictionary<string, GameObject>();

    public bool XiaJiaStatus;

	//兑换相关
	public GameObject Obj_DuiHuanRmbNum;
	public GameObject Obj_DuiHuanGetGoldNum;
	public GameObject Obj_DuiHuanDes;
	public ObscuredInt DuiHuanRMB;
	public ObscuredInt DuiHuanRmbValuePro;
	public ObscuredInt DuiHuanRmbValuePro_Last;
	public ObscuredInt DuiHuanGetGold;

	// Use this for initialization
	void Start () {

		//界面适配
		Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_PaiMaiHangSet);

		//显示通用UI
		Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject,"801");

		//服务器
		Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang = this.gameObject;

        //初始长度
        paimaiShowList_Y_ChuShi = Obj_PaiMaiHangItemShow.transform.localPosition.y;

        //请求拍卖行的商品信息
        paimaiID_ChuShi = "100";
        Pro_ComStr_2 com_str_2 = new Pro_ComStr_2();
		string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		com_str_2.str_1 = zhanghaoID;
		com_str_2.str_2 = paimaiID_ChuShi;

		Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001081, com_str_2);

		//请求自身拍卖数据
		//Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001084, zhanghaoID);

		//请求自身拍卖兑换金币数据
		//Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001086, zhanghaoID);

		//测试信息
		/*
		Pro_PaiMaiData paiList = new Pro_PaiMaiData();
		paiList.PaiMaiID = "1";
		paiList.PaiMaiType = "1";
		paiList.PaiMaiItemID = "10000021";
		paiList.PaiMaiItemNum = "10";
		paiList.GoldType = "1";
		paiList.GoldValue = "100";
		paiList.GoldBianHuaValue = 0.1f;

		PaimaiItemShow.Add ("1", paiList);
		PaimaiItemShow.Add ("2", paiList);
		PaimaiItemShow.Add ("3", paiList);
		*/

		nowSeleBuyItemID = "1";

        //默认打开出售
        if (IfInitStatus) {
            Btn_BuySetShow();
        }

        PaiMaiHangUpdate ();
	}
	
	// Update is called once per frame
	void Update () {
		
		//更新选中状态
		if (PaiMaiHangXuanZhongStatus) {
			PaiMaiHangXuanZhongStatusNum = PaiMaiHangXuanZhongStatusNum + 1;

            if (PaiMaiHangXuanZhongStatusNum > 1) {
				PaiMaiHangXuanZhongStatusNum = 0;
				PaiMaiHangXuanZhongStatus = false;
			}
		}

		//更新列表
		if(Rose_PaiMaiHangList_Update){
			Rose_PaiMaiHangList_Update = false;
			CleanSonGameObject();
			PaiMaiHangUpdate ();
		}

		//选中拍卖行商品
		if (PaiMaiHangXuanZhongItemListStatus) {
			PaiMaiHangXuanZhongItemListNum = PaiMaiHangXuanZhongItemListNum + 1;
			if (PaiMaiHangXuanZhongItemListNum > 1) {
				PaiMaiHangXuanZhongItemListNum = 0;
				PaiMaiHangXuanZhongItemListStatus = false;
			}
		}

		//选中出售商品
		//选中商品
		if (ChuShouXuanZhongItemListStatus) {
			ChuShouXuanZhongItemListNum = ChuShouXuanZhongItemListNum + 1;
			if (ChuShouXuanZhongItemListNum > 1) {
				ChuShouXuanZhongItemListNum = 0;
				ChuShouXuanZhongItemListStatus = false;
			}
		}


		//查看当前选中的背包格子是否发生改变
		/*
		if(LastBagSpaceNum !=Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect){
			LastBagSpaceNum = Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect;
			if (LastBagSpaceNum == "-1") {
				ChuShouItemNum = 0;
			} else {
				//获取最大数量
				int spaceNum = int.Parse (Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect);
				//读取背包数据
				Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
				string sapceItemNum = function_DataSet.DataSet_ReadData ("ItemNum", "ID", spaceNum.ToString (), "RoseBag");
				ChuShouItemNum = int.Parse (sapceItemNum);
			}
			ShowSaleItemNum ();
		}
		*/

		if (UpdateChuShouItemNumStatus) {
			UpdateChuShouItemNumStatus = false;
            //获取最大数量
            if (Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect != ""&& Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect!=null) {
			    int spaceNum = int.Parse (Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect);
			    //读取背包数据
			    Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
			    string sapceItemNum = function_DataSet.DataSet_ReadData ("ItemNum", "ID", spaceNum.ToString (), "RoseBag");
			    ChuShouItemNum = int.Parse (sapceItemNum);
			    ShowSaleItemNum ();
            }
        }

        if (Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus == false) {
            CloseUI_TaskList();
        }
	}


	//成就更新
	private void PaiMaiHangUpdate()
	{
		float hight = 0.0f;
		int zhangJieNum = 5;        //章节数量
		//Debug.Log("hight000= " + hight);
		hight = ShowPaiMaiHangZhangJieList("0", hight, zhangJieNum);
		//Debug.Log("hight111= " + hight);
		hight = ShowPaiMaiHangZhangJieList("1", hight, zhangJieNum);
		//Debug.Log("hight222= " + hight);
		hight = ShowPaiMaiHangZhangJieList("2", hight, zhangJieNum);
		//Debug.Log("hight333= " + hight);
		//hight = ShowPaiMaiHangZhangJieList("3", hight, zhangJieNum);
		//Debug.Log("hight333= " + hight);
	}



	//打开章节列表
	public float ShowPaiMaiHangZhangJieList(string PaiMaiHangType, float hight,int zhangJieNum) {

		//实例化界面
		GameObject UI_RosePaiMaiHangType = (GameObject)Instantiate(Obj_UIPaiMaiHangList_Type);

		//给实例化的脚本指定任务类型
		//UI_RoseChengJiuType.GetComponent<Rose_ChengJiuList_Show>().ChengJiuType = chengJiuType;

		string PaiMaiHangIDSet = "";

		//获取当前任务类型对应的文字
		string ChengJiuTypeName = "";
		switch (PaiMaiHangType)
		{

		//材料
		case "0":
			PaiMaiHangIDSet = "100,101,102,103,104,105";
			zhangJieNum = PaiMaiHangIDSet.Split (',').Length;
			ChengJiuTypeName = "材料";
			UI_RosePaiMaiHangType.transform.parent = OpenPaiMaiHangZhangJieList[int.Parse(PaiMaiHangType)].transform;
			UI_RosePaiMaiHangType.transform.localPosition = new Vector3 (0, hight, 0);
			UI_RosePaiMaiHangType.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			//hight = -55.0f;
			if (OpenPaiMaiHangZhangJie_1) {
				hight = hight - zhangJieNum * 50.0f - 55.0f;
			} else {
				hight = hight - 55.0f;
			}

			break;

		//宠物用品
		case "1":
			PaiMaiHangIDSet = "200,201,202";
			zhangJieNum = PaiMaiHangIDSet.Split (',').Length;
			ChengJiuTypeName = "宠物";
			UI_RosePaiMaiHangType.transform.parent = OpenPaiMaiHangZhangJieList[int.Parse(PaiMaiHangType)].transform;
			UI_RosePaiMaiHangType.transform.localPosition = new Vector3(0, hight, 0);
			UI_RosePaiMaiHangType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

			//计算列表当前高度
			if (OpenPaiMaiHangZhangJie_2)
			{
				hight = hight - zhangJieNum * 50.0f - 55.0f;
			}
			else
			{
				hight = hight - 55.0f;
			}
			break;

		//宝石
		case "2":
			//PaiMaiHangIDSet = "300";
            PaiMaiHangIDSet = "301,302,303,304,305,306";
            zhangJieNum = PaiMaiHangIDSet.Split (',').Length;
			ChengJiuTypeName = "宝石";
			UI_RosePaiMaiHangType.transform.parent = OpenPaiMaiHangZhangJieList[int.Parse(PaiMaiHangType)].transform;
			UI_RosePaiMaiHangType.transform.localPosition = new Vector3(0, hight, 0);
			UI_RosePaiMaiHangType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

			//计算列表当前高度
			if (OpenPaiMaiHangZhangJie_3)
			{
				hight = hight - zhangJieNum * 50.0f - 55.0f;
			}
			else
			{
				hight = hight - 55.0f;
			}
			break;

		//装备
		/*
		case "3":
			PaiMaiHangIDSet = "0,1,2,3,4,5";
			zhangJieNum = PaiMaiHangIDSet.Split (',').Length;
			ChengJiuTypeName = "宝石";
			UI_RosePaiMaiHangType.transform.parent = OpenPaiMaiHangZhangJieList[int.Parse(PaiMaiHangType)].transform;
			UI_RosePaiMaiHangType.transform.localPosition = new Vector3(0, hight, 0);
			UI_RosePaiMaiHangType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

			//计算列表当前高度
			if (OpenPaiMaiHangZhangJie_3)
			{
				hight = hight - zhangJieNum * 50.0f - 55.0f;
			}
			else
			{
				hight = hight - 55.0f;
			}
			break;
		*/
		}


		//检测当前角色任务是否展开
		bool ifShowList = false;
		switch (PaiMaiHangType)
		{
			case "0":
				ifShowList = OpenPaiMaiHangZhangJie_1;
				break;
			case "1":
				ifShowList = OpenPaiMaiHangZhangJie_2;
				break;
			case "2":
				ifShowList = OpenPaiMaiHangZhangJie_3;
				break;
			case "3":
				ifShowList = OpenPaiMaiHangZhangJie_4;
				break;
		}

        //本地化
        ChengJiuTypeName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(ChengJiuTypeName);

        if (ifShowList)
		{
			//展开列表
			//更新任务类型名称
			Rose_PaiMaiHangList_Show taskList_Show = UI_RosePaiMaiHangType.GetComponent<Rose_PaiMaiHangList_Show>();
			taskList_Show.UI_TaskTypeName.GetComponent<Text>().text = ChengJiuTypeName;

			//更新图标显示
			taskList_Show.UIImg_TaskListShow.SetActive(true);
			taskList_Show.UIImg_TaskListShow_2.SetActive(false);

			//显示章节
			LoadPaiMaiHangZhangJie(PaiMaiHangIDSet,UI_RosePaiMaiHangType);

		}
		else
		{
			//收缩列表
			Rose_PaiMaiHangList_Show taskList_Show = UI_RosePaiMaiHangType.GetComponent<Rose_PaiMaiHangList_Show>();
			taskList_Show.UI_TaskTypeName.GetComponent<Text>().text = ChengJiuTypeName;

			//更新图标显示
			taskList_Show.UIImg_TaskListShow.SetActive(false);
			taskList_Show.UIImg_TaskListShow_2.SetActive(true);
		}

		UI_RosePaiMaiHangType.GetComponent<Rose_PaiMaiHangList_Show>().PaiMaiHangType = PaiMaiHangType;
		return hight;
	}


	public void LoadPaiMaiHangZhangJie(string paimaiListIDSet,GameObject obj_PaiMaiHangType)
	{
		Debug.Log ("展示拍卖行列表");
		string[] paimaiIDList = paimaiListIDSet.Split (',');
		//实例化任务UI
		int zhangJieNum = 0;
		string ziDuanName = "";

		for (int i = 0; i<paimaiIDList.Length; i++) {

			//设置名称
			string showName = "";
			switch (paimaiIDList [i]) {
				
			case "0":
				showName = "通用";
				break;

			case "1":
				showName = "第一章";
				break;

			case "2":
				showName = "第二章";
				break;

			case "3":
				showName = "第三章";
				break;

			case "4":
				showName = "第四章";
				break;

			case "5":
				showName = "第五章";
				break;


			//材料
			case "100":
				showName = "通用";
				break;

			case "101":
				showName = "第一章";
				break;

			case "102":
				showName = "第二章";
				break;

			case "103":
				showName = "第三章";
				break;

			case "104":
				showName = "第四章";
				break;

			case "105":
				showName = "第五章";
				break;

			//宠物
			case "200":
				showName = "通用";
				break;
            //宠物
            case "201":
                showName = "低级技能";
                break;
            //宠物
            case "202":
                showName = "高级技能";
                break;
            //宝石
                case "300":
				showName = "通用";
				break;
            case "301":
                showName = "红色插槽";
                break;
            case "302":
                showName = "紫色插槽";
                break;
            case "303":
                showName = "蓝色插槽";
                break;
            case "304":
                showName = "绿色插槽";
                break;
            case "305":
                showName = "白色插槽";
                break;
            case "306":
                showName = "抗性宝石";
                break;
            }

			zhangJieNum = zhangJieNum + 1;
			GameObject UI_RoseChengJiuListName = (GameObject)Instantiate(Obj_UIPaiMaiHangZhangJie_Row);
			Rose_PaiMaiHangList_Show rose_TaskList_Show = obj_PaiMaiHangType.GetComponent<Rose_PaiMaiHangList_Show>();
			UI_RoseChengJiuListName.transform.parent = rose_TaskList_Show.UIPoint_TaskName;
			UI_RoseChengJiuListName.transform.localPosition = new Vector3(0, zhangJieNum * -50.0f, 0);
			UI_RoseChengJiuListName.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

			//显示章节名称
			Rose_PaiMaiHangList_row_UIPoint uiPoint = UI_RoseChengJiuListName.GetComponent<Rose_PaiMaiHangList_row_UIPoint>();
			Text textName = uiPoint.UI_TaskName.GetComponent<Text>();

            //本地化
            showName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(showName);
            textName.text = showName;

			//收集id
			uiPoint.PaiMaiHangID = paimaiIDList[i];

			//点击列表默认显示第一个
			if (i==0) {
				if (ShowFirstObjStatus) {
					ShowFirstObjStatus = false;
					uiPoint.UI_SelectTask();
				}
			}
		}
	}

	//展示信息
	public void ShowPaiMaiHangUI(){

        //Debug.Log("展示价格浮动！");
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("价格浮动");

		//清空列表信息
		Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PaiMaiHangItemShow);
		int firstNum = 0;
		foreach(string listID in PaimaiItemShow.Keys) {

			Pro_PaiMaiDataAdd paimaiShowList = PaimaiItemShow[listID];

			//判定道具ID是否存在
			string itemPileSum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", paimaiShowList.PaiMaiItemID, "Item_Template");
			if (itemPileSum != "0" && itemPileSum != "") {

                string typeStr = paimaiID_ChuShi;
                if (PaiMaiHangXuanZhongObj != null) {
                    typeStr = PaiMaiHangXuanZhongObj.GetComponent<Rose_PaiMaiHangList_row_UIPoint>().PaiMaiHangID;
                }

                if (paimaiShowList.PaiMaiType == typeStr) {
				    //创建列表
				    GameObject paimaiItemShowObj = (GameObject)Instantiate(Obj_PaiMaiItemShow);
                    paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList> ().PaimaiListID = listID;
				    paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().itemID = paimaiShowList.PaiMaiItemID;
				    paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().itemGoldNum = paimaiShowList.GoldValue;
                    float fudongValue = paimaiShowList.GoldBianHuaValue - 1;
                    if (fudongValue >= 0)
                    {
                        fudongValue = fudongValue * 100;
                        string fudongStr = fudongValue.ToString();
                        //保留小数点后两位
                        int result = fudongStr.Length - fudongStr.IndexOf(".") - 1;
                        if (result >= 2)
                        {
                            fudongStr = string.Format("{0:F}", fudongValue);
                        }
                        //设置浮动价格
                        //paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().Obj_ItemNumDes.GetComponent<Text>().color = Color.green;
                        paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().GoldNumDes = langStr + ":" + fudongStr + "%";
                    }
                    else {
                        fudongValue = fudongValue * 100;
                        string fudongStr = fudongValue.ToString();
                        //保留小数点后两位
                        int result = fudongStr.Length - fudongStr.IndexOf(".") - 1;
                        if (result >= 2)
                        {
                            fudongStr = string.Format("{0:F}", fudongValue);
                        }
                        //设置浮动价格
                        paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().Obj_ItemNumDes.GetComponent<Text>().color = Color.red;
                        paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().GoldNumDes = langStr + ":" + fudongStr + "%";
                    }

				    
				    paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().ShowItem ();
				    paimaiItemShowObj.transform.SetParent(Obj_PaiMaiHangItemShow.transform);
                    paimaiItemShowObj.transform.localScale = new Vector3(1, 1, 1);

                    //增加缓存
                    if (ChuShouItemShowObj.ContainsKey(paimaiShowList.PaiMaiItemID) == true)
                    {
                        ChuShouItemShowObj.Remove(paimaiShowList.PaiMaiItemID);
                    }
                    ChuShouItemShowObj.Add(paimaiShowList.PaiMaiItemID, paimaiItemShowObj);

                    //默认显示第一个选中
                    if (firstNum == 0){
					    paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList> ().Btn_PaiMaiListID();
				    }

				    firstNum = firstNum + 1;



                }
            }
		}

        //设置位置
        float hightValue = (int)(PaimaiItemShow.Count / 4) * 240 + 240;
        if (hightValue <= 600)
        {
            hightValue = 600;
        }

        Obj_PaiMaiHangItemShow.GetComponent<RectTransform>().sizeDelta = new Vector2(Obj_PaiMaiHangItemShow.GetComponent<RectTransform>().sizeDelta.x, hightValue);
        Obj_PaiMaiHangItemShow.transform.localPosition = new Vector3(Obj_PaiMaiHangItemShow.transform.localPosition.x, hightValue / 2 * -1, Obj_PaiMaiHangItemShow.transform.localPosition.z);

    }


    //展示信息
    public void ShowPaiMaiHangUI_Jie()
    {

        Debug.Log("展示价格浮动！");
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("价格浮动");

        //清空列表信息
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PaiMaiHangItemShow);
        int firstNum = 0;
        foreach (string listID in PaimaiItemShow.Keys)
        {

            Pro_PaiMaiDataAdd paimaiShowList = PaimaiItemShow[listID];

            //判定道具ID是否存在
            string itemPileSum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", paimaiShowList.PaiMaiItemID, "Item_Template");
            if (itemPileSum != "0" && itemPileSum != "")
            {

                string typeStr = paimaiID_ChuShi;
                if (PaiMaiHangXuanZhongObj != null)
                {
                    typeStr = PaiMaiHangXuanZhongObj.GetComponent<Rose_PaiMaiHangList_row_UIPoint>().PaiMaiHangID;
                }

                if (paimaiShowList.PaiMaiType == typeStr)
                {
                    //创建列表
                    GameObject paimaiItemShowObj = (GameObject)Instantiate(Obj_PaiMaiItemShow);
                    paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().PaimaiListID = listID;
                    paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().itemID = paimaiShowList.PaiMaiItemID;
                    paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().itemGoldNum = paimaiShowList.GoldValue;
                    float fudongValue = paimaiShowList.GoldBianHuaValue - 1;
                    if (fudongValue >= 0)
                    {
                        fudongValue = fudongValue * 100;
                        string fudongStr = fudongValue.ToString();
                        //保留小数点后两位
                        int result = fudongStr.Length - fudongStr.IndexOf(".") - 1;
                        if (result >= 2)
                        {
                            fudongStr = string.Format("{0:F}", fudongValue);
                        }
                        //设置浮动价格
                        //paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().Obj_ItemNumDes.GetComponent<Text>().color = Color.green;
                        paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().GoldNumDes = langStr + ":" + fudongStr + "%";
                    }
                    else
                    {
                        fudongValue = fudongValue * 100;
                        string fudongStr = fudongValue.ToString();
                        //保留小数点后两位
                        int result = fudongStr.Length - fudongStr.IndexOf(".") - 1;
                        if (result >= 2)
                        {
                            fudongStr = string.Format("{0:F}", fudongValue);
                        }
                        //设置浮动价格
                        paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().Obj_ItemNumDes.GetComponent<Text>().color = Color.red;
                        paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().GoldNumDes = langStr + ":" + fudongStr + "%";
                    }


                    paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().ShowItem();
                    paimaiItemShowObj.transform.SetParent(Obj_PaiMaiHangItemShow.transform);
                    paimaiItemShowObj.transform.localScale = new Vector3(1, 1, 1);


                    //增加缓存
                    if (ChuShouItemShowObj.ContainsKey(paimaiShowList.PaiMaiItemID) == true) {
                        ChuShouItemShowObj.Remove(paimaiShowList.PaiMaiItemID);
                    }
                    ChuShouItemShowObj.Add(paimaiShowList.PaiMaiItemID, paimaiItemShowObj);

                    //默认显示第一个选中
                    if (firstNum == 0)
                    {
                        paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList>().Btn_PaiMaiListID();
                    }

                    firstNum = firstNum + 1;
                }
            }
        }

        //设置位置
        float hightValue = (int)(PaimaiItemShow.Count / 4) * 240 + 240;
        if (hightValue <= 600)
        {
            hightValue = 600;
        }

        Obj_PaiMaiHangItemShow.GetComponent<RectTransform>().sizeDelta = new Vector2(Obj_PaiMaiHangItemShow.GetComponent<RectTransform>().sizeDelta.x, hightValue);
        Obj_PaiMaiHangItemShow.transform.localPosition = new Vector3(Obj_PaiMaiHangItemShow.transform.localPosition.x, hightValue / 2 * -1, Obj_PaiMaiHangItemShow.transform.localPosition.z);

    }


    //清理子物体
    private void CleanSonGameObject() {

		for (int y = 0; y < OpenPaiMaiHangZhangJieList.Length; y++) {
			//清空子物体
			for (int i = 0; i < OpenPaiMaiHangZhangJieList[y].transform.childCount; i++)
			{
				GameObject go = OpenPaiMaiHangZhangJieList[y].transform.GetChild(i).gameObject;
				Destroy(go);
			}
		}
	}

	//点击购买列表按钮
	public void Btn_BuySetShow() {
		clearnListBtn();
		Obj_PaiMaiHangListSet.SetActive(true);
		Obj_PaiMaiHangSaleSet.SetActive(false);
		Obj_PaiMaiHangDuiHuanSet.SetActive (false);
		//显示底图
		object obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
		Sprite img = obj as Sprite;
		Obj_Btn_Buy_Img.GetComponent<Image>().sprite = img;
		//更新通用图标显示
		Obj_Btn_Buy_Text.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

		ShowPaiMaiHangUI();

	}


	//点击出售列表按钮
	public void Btn_SaleSetShow() {
		clearnListBtn();
		Obj_PaiMaiHangListSet.SetActive(false);
		Obj_PaiMaiHangSaleSet.SetActive(true);
		Obj_PaiMaiHangDuiHuanSet.SetActive (false);

		//显示底图
		object obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
		Sprite img = obj as Sprite;
		Obj_Btn_Sale_Img.GetComponent<Image>().sprite = img;
		//更新通用图标显示
		Obj_Btn_Sale_Text.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

		//请求自身拍卖数据
		string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001084, zhanghaoID);

		//展示背包列表
		BagItemListShow ();
		//展示出售列表
		ShowSaleItemList();
		//清空选择背包位置
		Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect = "-1";
		ChuShouItemNum = 0;
		ShowSaleItemNum ();
	}

	//点击兑换列表按钮
	public void Btn_DuiHuanSetShow() {

		clearnListBtn();
		Obj_PaiMaiHangListSet.SetActive(false);
		Obj_PaiMaiHangSaleSet.SetActive(false);
		Obj_PaiMaiHangDuiHuanSet.SetActive (true);

		//显示底图
		object obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
		Sprite img = obj as Sprite;
		Obj_Btn_DuiHuan_Img.GetComponent<Image>().sprite = img;
		//更新通用图标显示
		Obj_Btn_DuiHuan_Text.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

		DuiHuanShow ();
	}


	//增加购买数量
	public void Btn_BuyItemNum_Add(int buyNum){

		BuyItemNum = BuyItemNum + buyNum;
        //获取道具堆叠数量
        Pro_PaiMaiDataAdd PaimaiShowList = PaimaiItemShow[nowSeleBuyItemID];
		string buyItemID = PaimaiShowList.PaiMaiItemID;
		string itemPileSum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPileSum", "ID", buyItemID, "Item_Template");
		if (itemPileSum == "") {
			itemPileSum = "1";
		}
		if (BuyItemNum > int.Parse(itemPileSum)) {
			BuyItemNum = int.Parse(itemPileSum);
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_155");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + int.Parse(itemPileSum));
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("购买单组数量最大为"+int.Parse(itemPileSum));
		}
		ShowGold ();
	}


	//减少购买数量
	public void Btn_BuyItemNum_Cost(int costNum){

		BuyItemNum = BuyItemNum - costNum;
		if (BuyItemNum < 0) {
			BuyItemNum = 0;
		}
		ShowGold();
	}

	//显示购买金额
	public void ShowGold(){

        //获取商品单价
        Pro_PaiMaiDataAdd PaimaiShowList = PaimaiItemShow[nowSeleBuyItemID];
		//int danjia = int.Parse(PaimaiShowList.GoldValue);

        if (ChuShouItemShowObj.ContainsKey(PaimaiShowList.PaiMaiItemID)) {

            ObscuredInt danjia = int.Parse(ChuShouItemShowObj[PaimaiShowList.PaiMaiItemID].GetComponent<PaiMaiHangItemShowList>().itemGoldNum);
            BuyItemAllGold = BuyItemNum * danjia;
            Obj_BuyItemNum.GetComponent<Text>().text = BuyItemNum.ToString();
            Obj_BuyItemAllGold.GetComponent<Text>().text = BuyItemAllGold.ToString();

        }

    }


	public void Btn_BuyItem(){

        if (BuyItemNum <= 0) {
            return;
        }

        if (BuyItemAllGold <= 0) {
            return;
        }

		int pipeiValue = int.Parse(Obj_BuyItemAllGold.GetComponent<Text>().text);
		if (BuyItemAllGold < pipeiValue)
		{
			BuyItemAllGold = pipeiValue;
		}

		//Debug.Log ("我点击了购买道具");
		if (Game_PublicClassVar.gameLinkServer.ServerLinkStatus)
		{
			Pro_PaiMaiDataAdd PaimaiShowList = PaimaiItemShow[nowSeleBuyItemID];
			string buyItemID = PaimaiShowList.PaiMaiItemID;
			long roseGold = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
			if (roseGold >= BuyItemAllGold && BuyItemNum >= 1)
			{

				//判定背包格子是否足够
				if (Game_PublicClassVar.Get_function_Rose.BagNullNum() >= 1)
				{

					//发送道具
					Game_PublicClassVar.Get_function_Rose.CostReward("1", BuyItemAllGold.ToString());
					Game_PublicClassVar.Get_function_Rose.SendRewardToBag(buyItemID, BuyItemNum, "0", 0, "0", true, "10");

					//服务器发送购买信息
					Pro_PaiMai_Buy pro_PaiMai_Buy = new Pro_PaiMai_Buy();
					string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
					pro_PaiMai_Buy.ZhangHaoID = zhanghaoID;
					pro_PaiMai_Buy.PaiMaiItemID = buyItemID;
					pro_PaiMai_Buy.PaiMaiItemNum = BuyItemNum.ToString();
					pro_PaiMai_Buy.GoldType = "1";
					pro_PaiMai_Buy.GoldValue = BuyItemAllGold.ToString();
					Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001072, pro_PaiMai_Buy);

				}
				else
				{
					string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_84");
					Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
					//Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包已满");
					return;
				}
			}
			else
			{
				string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_138");
				Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
				//Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("金币不足！");
				return;
			}

		}
		else {
			Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未连接服务器,无法购买!");
		}
	}


	//展示出售背包
	public void BagItemListShow() {
		Debug.Log("点击了道具");
		//清空道具列表
		Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_BagSpaceListSet);
		//将自身的所有消耗品显示
		Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
		for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++) {
			string itemID = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
			//Debug.Log("itemID = " + itemID);
			if (itemID != "" && itemID!="0")
			{
				string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
				if (itemType == "1"|| itemType == "2"|| itemType == "4"|| itemType == "5") {
					//判定是否有技能
					//string itemSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", itemID, "Item_Template");
					//if (itemSkillID != "0")
					//{
						Debug.Log("开始创建！");
						//开始创建背包格子
						GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
						bagSpace.transform.SetParent(Obj_BagSpaceListSet.transform);
						bagSpace.GetComponent<UI_BagSpace>().BagPosition = i.ToString();
						bagSpace.GetComponent<UI_BagSpace>().SpaceType = "1";   //设置格子为背包属性   
						bagSpace.GetComponent<UI_BagSpace>().MoveBagStatus = true;
						bagSpace.transform.localScale = new Vector3(1, 1, 1);
					//}
				}
			}
		}
	}


	//展示出售列表
	public void ShowSaleItemList(){

        XiaJiaStatus = false;

		//清空列表信息
		Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ChuShouListSet);
		int firstNum = 0;
		foreach(string listID in ChuShouItemShow.Keys) {

			Pro_PaiMai_PlayerSellAdd paimaiShowList = ChuShouItemShow[listID];
			//创建列表
			GameObject paimaiItemShowObj = (GameObject)Instantiate(Obj_ChuShouListItemShow);
			paimaiItemShowObj.GetComponent<PaiMaiSaleItemShow>().SaleListID = listID;
			paimaiItemShowObj.GetComponent<PaiMaiSaleItemShow>().ItemID = paimaiShowList.PaiMaiItemID;
			paimaiItemShowObj.GetComponent<PaiMaiSaleItemShow>().ItemSaleGold = paimaiShowList.GoldValue;
			paimaiItemShowObj.GetComponent<PaiMaiSaleItemShow>().ItemSaleNum = int.Parse(paimaiShowList.PaiMaiItemNum);
			paimaiItemShowObj.GetComponent<PaiMaiSaleItemShow>().ItemSaleTime = int.Parse(paimaiShowList.SellTime);
			paimaiItemShowObj.GetComponent<PaiMaiSaleItemShow>().ServerSellID = paimaiShowList.SellID;
			paimaiItemShowObj.GetComponent<PaiMaiSaleItemShow>().ShowSaleItem();
			paimaiItemShowObj.transform.SetParent(Obj_ChuShouListSet.transform);
            paimaiItemShowObj.transform.localScale = new Vector3(1, 1, 1);


            //默认显示第一个选中
            /*
			if(firstNum == 0){
				paimaiItemShowObj.GetComponent<PaiMaiHangItemShowList> ().Btn_PaiMaiListID();
			}
			firstNum = firstNum + 1;
			*/
        }

    }

	//上架道具_增加数量
	public void BtnSaleItemNum_Add(int num){

		int spaceNum = int.Parse (Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect);
		//读取背包数据
		Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
		string sapceItemNum = function_DataSet.DataSet_ReadData ("ItemNum", "ID", spaceNum.ToString (), "RoseBag");
		int nowspaceNum = int.Parse (sapceItemNum);
		ChuShouItemNum = ChuShouItemNum + num;
		if(ChuShouItemNum > nowspaceNum){
			ChuShouItemNum = nowspaceNum;
		}
		ShowSaleItemNum ();
	}

	//上架道具_减少数量
	//出售最低为1
	public void BtnSaleItemNum_Cost(int num){

		ChuShouItemNum = ChuShouItemNum - num;
		if(ChuShouItemNum <0){
			ChuShouItemNum = 0;
		}
		ShowSaleItemNum ();
	}

	//显示出售道具
	private void ShowSaleItemNum(){

		Obj_ChuShouItemNum.GetComponent<Text> ().text = ChuShouItemNum.ToString ();
	}


	//点击上架道具
	public void Btn_SaleItem_Up(){

		if (ChuShouItemNum <= 0) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_159");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("出售道具的数量不能为0");
			return;
		}

        //未连接服务器不能上架
        if (Game_PublicClassVar.gameLinkServer.ServerLinkStatus == false) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_160");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未连接至服务器");
            return;
        }

        //未连接服务器不能上架
        if (Game_PublicClassVar.Get_wwwSet.ServerName == "")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_160");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未连接至服务器");
            return;
        }

        //获取当前出售道具的数量最多只能上架8个
        if (ChuShouItemShow.Count >= 8) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_161");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("最多只能上架8个道具");
            return;
        }

        int spaceNum = int.Parse (Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect);
		//读取背包数据
		Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
		string sapceItemID = function_DataSet.DataSet_ReadData ("ItemID", "ID", spaceNum.ToString (), "RoseBag");
		string sapceItemNum = function_DataSet.DataSet_ReadData ("ItemNum", "ID", spaceNum.ToString (), "RoseBag");
		if (sapceItemID == "0" && sapceItemID == "") {
			return;
		}

        //兽决类道具需要角色大于20级才可以出售
        string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", sapceItemID, "Item_Template");
        if (itemType == "5") {
            if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < 20) {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_162");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("出售宠物技能书类的道具需要角色等级>=20级");
                return;
            }
        }


        //设置属性
        if (Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelectType == "1" &&Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect !="-1") {

			Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ChuShouItemShowSet);

            //发送出售数据
            Pro_PaiMaiSellData pro_PaiMaiSellData = new Pro_PaiMaiSellData();
            pro_PaiMaiSellData.BagSpaceNum = spaceNum;
            pro_PaiMaiSellData.PaiMaiItemID = sapceItemID;
            pro_PaiMaiSellData.PaiMaiItemNum = ChuShouItemNum.ToString();
            pro_PaiMaiSellData.GoldType = "1";
            string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            pro_PaiMaiSellData.ZhangHaoID = zhanghaoID;
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001077, pro_PaiMaiSellData);

			//写入活跃任务
			Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "12", "1");

            //弹出上架提示框10001077
            /*
            GameObject saleObj = (GameObject)Instantiate(Obj_ChuShouItemShow);
			saleObj.transform.SetParent(Obj_ChuShouItemShowSet.transform);
			saleObj.transform.localPosition = Vector3.zero;

			int sellValue = 250;	//后期需要修改,读取服务器信息
            
			saleObj.GetComponent<UI_PaiMaiChuShouShow> ().BagSpace = spaceNum;
			saleObj.GetComponent<UI_PaiMaiChuShouShow> ().Sale_ItemID = sapceItemID;
			saleObj.GetComponent<UI_PaiMaiChuShouShow> ().sale_ItemNum = int.Parse(sapceItemNum);
			saleObj.GetComponent<UI_PaiMaiChuShouShow> ().Sale_Gold = sellValue;		
			saleObj.GetComponent<UI_PaiMaiChuShouShow> ().ShowSale();
            */


        } else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_163");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("请先选择要出售的道具");
			ChuShouItemNum = 0;
			ShowSaleItemNum ();
		}
	}


	//点击下架道具
	public void Btn_SaleItem_Down(){

		SaleItemDown ();

	}

	//出售道具下架
	public void SaleItemDown(){

        if (XiaJiaStatus) {

            return;
        }

		//获取对应的选择
		if(Obj_ChuShouXuanZhongItemList != null){

            XiaJiaStatus = true;

            string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
			Pro_ComStr_2 com_str_2 = new Pro_ComStr_2 ();
			com_str_2.str_1 = zhanghaoID;
			com_str_2.str_2 = Obj_ChuShouXuanZhongItemList.GetComponent<PaiMaiSaleItemShow>().ServerSellID;
            //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001075, com_str_2);
            string addStr = Game_PublicClassVar.Get_xmlScript.Encrypt(com_str_2.str_2);
            com_str_2.str_2 = addStr;
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001078, com_str_2);
            //请求自身拍卖数据
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001084, zhanghaoID);

            if (Obj_ChuShouXuanZhongItemList.GetComponent<PaiMaiSaleItemShow>().ItemSaleTime <= 600) {
                //计入出售次数
                string sellnum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellItemNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                if (sellnum == "")
                {
                    sellnum = "0";
                }

                int writeNum = int.Parse(sellnum) + 1;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemNum", writeNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
            }
		}
	}


	//兑换界面
	public void DuiHuanShow(){

		//请求自身拍卖兑换金币数据
		string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001086, zhanghaoID);

		//DuiHuanRmbValuePro = 100;	//默认钻石兑换金币额度
		//Obj_DuiHuanDes.GetComponent<Text>().text = "1钻石可兑换"+ DuiHuanRmbValuePro+ "金币";
        Obj_DuiHuanDes.GetComponent<Text>().text = (DuiHuanRmbValuePro * 100).ToString();
        //默认兑换100钻石
        Btn_DuiHuanNum_Add (100);


	}


	//增加购买数量
	public void Btn_DuiHuanNum_Add(int buyNum){

		DuiHuanRMB = DuiHuanRMB + buyNum;
		//获取道具堆叠数量
		int roseRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
		if (DuiHuanRMB > roseRmb) {
			DuiHuanRMB = roseRmb;
			//Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("购买单组数量最大为"+int.Parse(itemPileSum));
		}
		ShowDuiHuanNum ();
	}


	//减少购买数量
	public void Btn_DuiHuanNum_Cost(int costNum){

		DuiHuanRMB = DuiHuanRMB - costNum;
		if (DuiHuanRMB < 0) {
			DuiHuanRMB = 0;
		}
		ShowDuiHuanNum();
	}

	//显示兑换货币数量
	public void ShowDuiHuanNum(){

		Obj_DuiHuanRmbNum.GetComponent<Text>().text = DuiHuanRMB.ToString ();

		//显示自己可以获得多少金币
		DuiHuanGetGold = DuiHuanRMB * DuiHuanRmbValuePro;
		Obj_DuiHuanGetGoldNum.GetComponent<Text>().text = DuiHuanGetGold.ToString ();

        //显示今天金币是上涨还是下降
        string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("上涨价格");
        string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("下降价格");

        float proValue = (float)(DuiHuanRmbValuePro)/(float)(DuiHuanRmbValuePro_Last);
		string showStr = "";
		if(proValue>=1){
			proValue = proValue -1;
			showStr = langStr_1 + ":" + proValue*100 + "%";
		}else{
			proValue = 1- proValue;
			showStr = langStr_2 + ":" + proValue*100 + "%";
		}

		Obj_DuiHuanDes.GetComponent<Text>().text = (DuiHuanRmbValuePro * 100).ToString();
	}

	public void Btn_DuiHuanGold(){

		if (DuiHuanRMB <= 0) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_164");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("兑换钻石数量不能为0");
			return;
		}

        if (DuiHuanGetGold <= 0) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_165");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("您可能未连接至服务器！兑换金币数量不能为0");
            return;
        }

		int roseRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
		if (DuiHuanRMB <= roseRmb) {
			//扣出钻石
			Game_PublicClassVar.Get_function_Rose.CostRMB(DuiHuanRMB);
			//发送金币
			Game_PublicClassVar.Get_function_Rose.SendReward("1",DuiHuanGetGold.ToString(),"44");

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_166");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_167");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + DuiHuanGetGold.ToString() + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("兑换成功！你兑换了" + DuiHuanGetGold.ToString()+"金币");
            //发送服务器消息
            //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000116, DuiHuanRMB + "钻石兑换金币");  
            Game_PublicClassVar.Get_function_Rose.ServerMsg_SendMsg(DuiHuanRMB + "钻石兑换金币");
        }
	}

	//关闭界面时调用
	public void CloseUI_TaskList() {
		Destroy(this.gameObject);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RosePaiMaiHang_Status = false;
	}

	public void clearnListBtn()
	{

		Obj_Btn_Buy_Text.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
		Obj_Btn_Sale_Text.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
		Obj_Btn_DuiHuan_Text.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
		//显示按钮
		object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
		Sprite img = obj as Sprite;
		Obj_Btn_Buy_Img.GetComponent<Image>().sprite = img;
		Obj_Btn_Sale_Img.GetComponent<Image>().sprite = img;
		Obj_Btn_DuiHuan_Img.GetComponent<Image>().sprite = img;
	}

}
