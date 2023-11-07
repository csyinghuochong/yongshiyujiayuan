using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Data;

public class UI_StoreHouse : MonoBehaviour {

    public GameObject Obj_BagSpace;                 //动态创建的格子
    public Transform Tra_BagSpaceList;              //动态创建格子的绑点
	public GameObject Obj_BagGoldNum;				//背包金币
	public GameObject Obj_BagFuzhong;				//背包当前负重
    private float bagSpacePosition_X;               //动态创建格子的X轴位置
    private float bagSpacePosition_Y;               //动态创建格子的Y轴位置
    public GameObject Obj_RoseBag;
    public bool BagMoveStatus;                      //背包移动状态
    public GameObject Obj_Scelect_1;                //移动选中
    public GameObject Obj_Scelect_2;                //移动选中
    public GameObject Obj_CangKuTitle_1;            
    public GameObject Obj_CangKuTitle_2;
    public GameObject Obj_CangKuTitle_3;
	public GameObject Obj_CangKuTitle_4;

    public GameObject Obj_TitleWeiJiHuo_1;
    public GameObject Obj_TitleWeiJiHuo_2;
    public GameObject Obj_TitleWeiJiHuo_3;
    public GameObject Obj_TitleWeiJiHuo_4;

    public GameObject Obj_BtnCangKu_1;
    public GameObject Obj_BtnCangKu_2;
    public GameObject Obj_BtnCangKu_3;
    public GameObject Obj_BtnCangKu_4;

    private bool zhengLiStatus;

    //public int OpenYeShu;

    public GameObject Obj_StoreHouseSet;            //适配UI


    //private int creatSpaceNum;                      //动态创建的格子数
	// Use this for initialization
	void Start () {

        //缓存一下背包数据
        /*
        DataTable dataTable;
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseBag.xml", "RoseBag");
        Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables.Remove("RoseBag");   //先将背包数据移除在进行添加
        Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables.Add(dataTable);
        */
		//更新背包货币数据
        //this.gameObject.active = false;
		UpdataBagMoney ();
        BagMoveStatus = true;
       	
		//默认打开第一个仓库切页
		//showSpace(0);
		Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu = 1;
        showBagNum(1);
        //展示激活状态
        ShowYeShuStatus();
        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_StoreHouseSet);

        //打开背包
        /*
        if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status == false)
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenBag();
        }
        */
    }
	
	// Update is called once per frame
	void Update () {
        //this.gameObject.active = true;
		if (Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll) {
			//更新背包货币数据
			UpdataBagMoney();
		}


        //触发移动注销此界面
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus != "1"){
            Destroy(this.gameObject);
            //关闭仓库状态
            Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus = false;
			/*
            //关闭背包
            if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status == true)
            {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenBag();
            }
			*/
        }
	}

    //被销毁时调用
    void OnDisable() {
        //此处会有一个错误,当开启背包UI，强制关闭游戏时,此处会报错，因为找不到对象
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose != null) {
            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_UIStatus>().RoseBag_Status = false;
        }

        Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu = 0;

        //清空Tips显示
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);

        //更新任务
        Game_PublicClassVar.Get_game_PositionVar.UpdatTaskStatus = true;

        Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus = false;

    }

	public void CloseUI(){
		Destroy (this.gameObject);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status = false;
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++){
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        //关闭仓库状态
        Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus = false;
	}

	public void UpdataBagMoney(){
		//获取当前金币
		string gold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Obj_BagGoldNum.GetComponent<Text>().text = gold;
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

	//初始创建的背包格子数量
	void showSpace(int creatSpaceNum){

		//清理需求道具
		for (int i = 0; i < Tra_BagSpaceList.transform.childCount; i++)
		{
			GameObject go = Tra_BagSpaceList.transform.GetChild(i).gameObject;
			Destroy (go);
		}

		//动态创建格子行
		//for (int z = 1; z <= 8; z++) {

			//动态创建每行中的格子
			for (int i = 1; i <= 49; i++)
			{
				//开始创建背包格子
				GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
				bagSpace.transform.SetParent(Tra_BagSpaceList);
				bagSpace.transform.localScale = new Vector3(1,1,1);
				//bagSpace.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(bagSpacePosition_X, bagSpacePosition_Y, 0);
				//bagSpacePosition_X = bagSpacePosition_X + 59.0f;
				creatSpaceNum = creatSpaceNum + 1;
                bagSpace.GetComponent<UI_StoreHouseSpace>().BagPosition = creatSpaceNum.ToString();
				bagSpace.GetComponent<UI_StoreHouseSpace>().SpaceType = "3";   //设置格子为背包属性 
			}

			//每行创建完数据清0
			//bagSpacePosition_Y = bagSpacePosition_Y - 59.0f;
			//bagSpacePosition_X = 0.0f;
		//}
		//bagSpacePosition_Y = 0;
	}

	public void showBagNum(int yeshuNum){

		switch (yeshuNum) {
			
		    case 1:
			    showSpace (0);
			    Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu = yeshuNum;
                ClearnYeShuShow();
                Obj_CangKuTitle_1.SetActive(true);

                Game_PublicClassVar.Get_game_PositionVar.RoseHouseStartNum = 1;
                Game_PublicClassVar.Get_game_PositionVar.RoseHouseEndNum = 49;

                break;

		    case 2:

                //判定
                int stroeHouseMaxNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StroeHouseMaxNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                if (stroeHouseMaxNum <= 1) {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_143");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要冒险家等级提升至3级开启此仓库栏位！");
                    return;
                }
			    showSpace(49);
			    Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu = yeshuNum;
                ClearnYeShuShow();
                Obj_CangKuTitle_2.SetActive(true);
            
                Game_PublicClassVar.Get_game_PositionVar.RoseHouseStartNum = 50;
                Game_PublicClassVar.Get_game_PositionVar.RoseHouseEndNum = 98;

                break;

		    case 3:
                //判定
                stroeHouseMaxNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StroeHouseMaxNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                if (stroeHouseMaxNum <= 2)
                {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_144");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要冒险家等级提升至6级开启此仓库栏位！");
                    return;
                }
			    showSpace(98);
			    Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu = yeshuNum;
                ClearnYeShuShow();
                Obj_CangKuTitle_3.SetActive(true);

                Game_PublicClassVar.Get_game_PositionVar.RoseHouseStartNum = 99;
                Game_PublicClassVar.Get_game_PositionVar.RoseHouseEndNum = 147;

                break;

		    case 4:
			    //判定
			    stroeHouseMaxNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StroeHouseMaxNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
			    if (stroeHouseMaxNum <= 3)
			    {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_145");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要冒险家等级提升至8级开启此仓库栏位！");
				    return;
			    }
			    showSpace(147);
			    Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu = yeshuNum;
			    ClearnYeShuShow();
			    Obj_CangKuTitle_4.SetActive(true);

                Game_PublicClassVar.Get_game_PositionVar.RoseHouseStartNum = 148;
                Game_PublicClassVar.Get_game_PositionVar.RoseHouseEndNum = 196;

                break;

		}
	}


    //仓库整理
    public void StoreZhengLi() {

        if (zhengLiStatus) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_146");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("正在整理,请稍后再试！");
            return;
        }

        zhengLiStatus = true;

        Debug.Log("触发仓库整理!");

        Game_PublicClassVar.Get_function_Rose.RoseArrangeStoreHouse(Game_PublicClassVar.Get_game_PositionVar.RoseHouseStartNum, Game_PublicClassVar.Get_game_PositionVar.RoseHouseEndNum);
        showBagNum(Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu);
        zhengLiStatus = false;

    }


    public void ClearnYeShuShow() {

        Obj_CangKuTitle_1.SetActive(false);
        Obj_CangKuTitle_2.SetActive(false);
        Obj_CangKuTitle_3.SetActive(false);
		Obj_CangKuTitle_4.SetActive(false);

    }

    //检测页数激活状态
    public void ShowYeShuStatus() {

        Obj_TitleWeiJiHuo_1.SetActive(true);
        Obj_TitleWeiJiHuo_2.SetActive(true);
        Obj_TitleWeiJiHuo_3.SetActive(true);
        Obj_TitleWeiJiHuo_4.SetActive(true);

        int stroeHouseMaxNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StroeHouseMaxNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        switch (stroeHouseMaxNum) {
            case 1:
                Obj_TitleWeiJiHuo_1.SetActive(false);
                Obj_TitleWeiJiHuo_3.SetActive(false);
                Obj_TitleWeiJiHuo_4.SetActive(false);
                Obj_BtnCangKu_3.SetActive(false);
                Obj_BtnCangKu_4.SetActive(false);

                break;
            case 2:
                Obj_TitleWeiJiHuo_1.SetActive(false);
                Obj_TitleWeiJiHuo_2.SetActive(false);


                Obj_TitleWeiJiHuo_4.SetActive(false);
                Obj_BtnCangKu_4.SetActive(false);
                break;
            case 3:
                Obj_TitleWeiJiHuo_1.SetActive(false);
                Obj_TitleWeiJiHuo_2.SetActive(false);
                Obj_TitleWeiJiHuo_3.SetActive(false);
                break;
            case 4:
                Obj_TitleWeiJiHuo_1.SetActive(false);
                Obj_TitleWeiJiHuo_2.SetActive(false);
                Obj_TitleWeiJiHuo_3.SetActive(false);
                Obj_TitleWeiJiHuo_4.SetActive(false);
                break;
            default:
                Obj_TitleWeiJiHuo_1.SetActive(false);
                Obj_TitleWeiJiHuo_2.SetActive(false);
                Obj_TitleWeiJiHuo_3.SetActive(false);
                Obj_TitleWeiJiHuo_4.SetActive(false);
                break;
        }
    }

    //背包移动状态
    public void Btn_MoveBagStatus()
    {
        if (BagMoveStatus)
        {
            //背包不能移动
            BagMoveStatus = false;

            for (int i = 0; i < Tra_BagSpaceList.transform.childCount; i++)
            {
                GameObject bagSpaceObj = Tra_BagSpaceList.transform.GetChild(i).gameObject;
                if (bagSpaceObj.GetComponent<UI_StoreHouseSpace>() != null)
                {
                    bagSpaceObj.GetComponent<UI_StoreHouseSpace>().MoveBagStatus = false;
                }
            }

            Obj_Scelect_1.SetActive(true);
            Obj_Scelect_2.SetActive(false);
            //背包更新
            Obj_RoseBag.GetComponent<UI_Bag>().Btn_MoveBagStatus();
        }
        else
        {
            //背包移动
            BagMoveStatus = true;

            for (int i = 0; i < Tra_BagSpaceList.transform.childCount; i++)
            {
                GameObject bagSpaceObj = Tra_BagSpaceList.transform.GetChild(i).gameObject;
                if (bagSpaceObj.GetComponent<UI_StoreHouseSpace>() != null)
                {
                    bagSpaceObj.GetComponent<UI_StoreHouseSpace>().MoveBagStatus = true;
                }
            }

            Obj_Scelect_1.SetActive(false);
            Obj_Scelect_2.SetActive(true);
            //背包更新
            Obj_RoseBag.GetComponent<UI_Bag>().Btn_MoveBagStatus();
        }
    }
}
