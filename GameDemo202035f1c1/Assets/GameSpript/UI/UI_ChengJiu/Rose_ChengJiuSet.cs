using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.Data;
using System;
using CodeStage.AntiCheat.ObscuredTypes;

//此脚本主要绑定在角色身上,当玩家打开任务列表时显示此脚本内容

public class Rose_ChengJiuSet : MonoBehaviour {

    //开启可更新任务界面
    public GameObject Obj_Rose_ChengJiuList;
    public bool Rose_ChengJiuList_Update;

    private GameObject UI_RoseChengJiuType;     		//任务日志类型标题控件
    public GameObject Obj_UIChengJiuList_Type;
    public GameObject Obj_UIChengJiuList_TaskRow;

    public GameObject Rose_RoseChengJiuBar;             //任务列表显示下拉进度条
    public Transform UIPoint_ChengJiuType;              //任务列表总绑点
    public Transform UIPoint_ChengJiuType_Main;         //主成就首页按钮
    public Transform UIPoint_ChengJiuType_Fight;        //战斗成就
    public Transform UIPoint_ChengJiuType_ShouJi;       //收集成就
    public Transform UIPoint_ChengJiuType_TanSuo;       //探索成就

    List<string> task_Fight = new List<string>();           //主线任务
    List<string> task_ShouJi = new List<string>();          //支线任务
    List<string> task_TanSuo = new List<string>();          //每日任务

    public GameObject UI_CommonHuoBiSetPosi;

    public GameObject Obj_ChengJiuDataShow;
    public GameObject Obj_ChengJiuDataSet;
    public GameObject Obj_ChengJiuDataSetList;

    public GameObject Obj_ChengJiuShouYeShow;
    public bool ShowFirstObjStatus;

    //成就奖励相关
    public GameObject Obj_ChengJiuSet;
    public GameObject Obj_ChengJiuRewardShowSet;            //成就奖励展示界面
    public GameObject Obj_ChengJiuRewardNum;
    public GameObject Obj_ChengJiuRewardNumSet;
    public GameObject Obj_XuanZhongRewardNum;
    public bool UpdateXuanZhongRewardStatus;
    private int updateXuanZhongRewardStatusNum;
    public GameObject Obj_ChengJiuAllNum;

    //精灵相关系数
    public GameObject Obj_JingLingSet;
    public GameObject Obj_XuanZhongJingLing;
    public bool UpdateXuanZhongJingLingStatus;
    private int updateXuanZhongJingLingStatusNum;
    public GameObject Obj_JingLingZhangJieShow;
    public GameObject Obj_JingLingZhangJieShowSet;
    public GameObject Obj_JingLingShow;
    public GameObject Obj_JingLingShowSet;

    //首杀相关系数
    public string ShouShaMonsterID;
    public GameObject Obj_ShouShaSet;
	public GameObject Obj_UIShouShaList_Type;
	public GameObject Obj_UIShouShaZhangJie_Row;
	public bool OpenShouShaZhangJie_1;
	public bool OpenShouShaZhangJie_2;
	public bool OpenShouShaZhangJie_3;
	public bool OpenShouShaZhangJie_4;
	public bool OpenShouShaZhangJie_5;
	public GameObject[] OpenShouShaZhangJieList;
	public bool Rose_ShouShaList_Update;
	public bool shouShaXuanZhongStatus;
	private int shouShaXuanZhongStatusNum;
	public GameObject shouShaXuanZhongObj;
	public GameObject Obj_ShouShaTypePro;
	public GameObject UIPoint_ShouShaType;

	public GameObject Obj_ShouShaShow_Name;
	public GameObject Obj_ShouShaShow_Icon;
	public GameObject Obj_ShouShaShow_Des;
	public GameObject Obj_ShouShaShow_DropSet;
	public GameObject Obj_ShouShaShow_Race;
	public GameObject Obj_ShouShaShow_DropItem;

	public GameObject Obj_ShouShaName_NanDu_1;
	public GameObject Obj_ShouShaName_NanDu_2;
	public GameObject Obj_ShouShaName_NanDu_3;
    public GameObject Obj_ShouShaShowReward;
    private GameObject shouShaShowRewardObj;
    public GameObject Obj_ShouShaShowRewardSet;
    public GameObject Obj_ShouShaShowRewardBtn;
    public GameObject Obj_BossShuaXinShow;
    public Pro_ShouShaNameList pro_ShouShaNameList;

    private bool bUseNewChengjiu;

    //右侧按钮变化
    public GameObject Obj_Btn_ChengJiu_Img;
    public GameObject Obj_Btn_ChengJiu_Text;
    public GameObject Obj_Btn_JingLing_Img;
    public GameObject Obj_Btn_JingLing_Text;
	public GameObject Obj_Btn_ShouSha_Img;
	public GameObject Obj_Btn_ShouSha_Text;
    // Use this for initialization
    void Start () {

        //打开更新成就界面开关
        bUseNewChengjiu = true;
        Rose_ChengJiuList_Update = true;
        ShowFirstObjStatus = true;
        //Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_1 = "1";      //默认打开战斗成就
        //显示成就
        Btn_ChengJiuSetShow();

        //清理展示列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ChengJiuDataSet);
        Obj_ChengJiuShouYeShow.SetActive(true);
        OpenShouYe();

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_Rose_ChengJiuList);

        //显示通用UI
        Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject,"701");

		Game_PublicClassVar.gameServerObj.Obj_ChengJiu = this.gameObject;

        //检测是否有成就完成(怪物类的为每次打开界面后检查,要不每杀1个怪会就检查会卡)    "228" || writeType == "109" || writeType == "204" || writeType == "105"
       
        string comChengJiuIDSet = "";
        //bUseNewChengjiu = false;
        if (bUseNewChengjiu)
            comChengJiuIDSet = Rose_ChengJiuCheck.Instance.ChengJiu_JianCeTargetSonTypeChengJiuID("1;2;3;4;5;6;105;228;109;204;101");
        else
            comChengJiuIDSet = Game_PublicClassVar.Get_function_Task.ChengJiu_JianCeTargetSonTypeChengJiuID("1;2;3;4;5;6;105;228;109;204;101");

        Debug.Log("comChengJiuIDSet:  " + comChengJiuIDSet);

        if (comChengJiuIDSet != "")
        {
            string[] comChengJiuIDList = comChengJiuIDSet.Split(';');
            for (int i = 0; i < comChengJiuIDList.Length; i++)
            {
                //写入成就ID
                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteComChengJiuID(comChengJiuIDList[i]);
                //弹出成就提示
                //Game_PublicClassVar.Get_function_Task.ChengJiu_ComHint(comChengJiuIDList[i]);
            }
        }

        //初始化标签按钮
        if (Game_PublicClassVar.Get_wwwSet.GameSetLanguage._Language != "Chinese")
        {
            Obj_Btn_ChengJiu_Text.GetComponent<Text>().fontSize = 20;
            Obj_Btn_JingLing_Text.GetComponent<Text>().fontSize = 20;
            Obj_Btn_ShouSha_Text.GetComponent<Text>().fontSize = 20;
        }
    }
	
	// Update is called once per frame
	void Update () {

        //初次打开任务日志更新界面
        if (Rose_ChengJiuList_Update) {
            //清理列表显示
            CleanSonGameObject();
            //更新列表显示
            chengJiuUpdate();
            Rose_ChengJiuList_Update = false;
        }

        //判定外界是否需要更新任务日志
        if (Game_PublicClassVar.Get_game_PositionVar.Rose_ChengJiuListUpdata)
        {
            //开启更新任务
            Rose_ChengJiuList_Update = true;
            //关闭外界更新任务开关
            Game_PublicClassVar.Get_game_PositionVar.Rose_ChengJiuListUpdata = false;

        }

        //更新选中状态
        if (UpdateXuanZhongRewardStatus) {
            if (updateXuanZhongRewardStatusNum > 1) {
                updateXuanZhongRewardStatusNum = 0;
                UpdateXuanZhongRewardStatus = false;
            }
            updateXuanZhongRewardStatusNum = updateXuanZhongRewardStatusNum + 1;
        }

        //更新精灵选中状态
        if (UpdateXuanZhongJingLingStatus) {
            updateXuanZhongJingLingStatusNum = updateXuanZhongJingLingStatusNum + 1;
            if (updateXuanZhongJingLingStatusNum > 1) {
                updateXuanZhongJingLingStatusNum = 0;
                UpdateXuanZhongJingLingStatus = false;
            }
        }

		//更新首杀选中状态
		if (shouShaXuanZhongStatus) {
			shouShaXuanZhongStatusNum = shouShaXuanZhongStatusNum + 1;
			if (shouShaXuanZhongStatusNum > 1) {
				shouShaXuanZhongStatusNum = 0;
				shouShaXuanZhongStatus = false;
			}
		}

		//更新首杀列表
		if(Rose_ShouShaList_Update){
			Rose_ShouShaList_Update = false;
			CleanShouShaSonGameObject();
			ShouShaUpdate ();
		}

    }

    //成就更新
    private void chengJiuUpdate()
    {
        //获取各个类型的成就数据
        string chengJiuStr_Fight = Game_PublicClassVar.Get_function_Task.ChengJiu_GetTargerTypeChengJiuIDSet("1002");
        string chengJiuStr_ShouJi = Game_PublicClassVar.Get_function_Task.ChengJiu_GetTargerTypeChengJiuIDSet("1003");
        string chengJiuStr_TanSuo = Game_PublicClassVar.Get_function_Task.ChengJiu_GetTargerTypeChengJiuIDSet("1004");
        string[] chengJiuList_Fight = chengJiuStr_Fight.Split(';');
        string[] chengJiuList_ShouJi = chengJiuStr_ShouJi.Split(';');
        string[] chengJiuList_TanSuo = chengJiuStr_TanSuo.Split(';');
        task_Fight = chengJiuList_Fight.ToList();
        task_ShouJi = chengJiuList_ShouJi.ToList();
        task_TanSuo = chengJiuList_TanSuo.ToList();

        float hight = 0.0f;
        int zhangJieNum = 6;        //章节数量
        Debug.Log("hight000= " + hight);
        //显示成就首页
        hight = ShowZhangJieList("0", hight, zhangJieNum);
        //Debug.Log("hight111= " + hight);
        //判定主线任务类型是否有任务
        hight = ShowZhangJieList("1", hight, zhangJieNum);
        Debug.Log("hight222= " + hight);
        //判定支线任务类型是否有任务
        hight = ShowZhangJieList("2", hight, zhangJieNum);
        Debug.Log("hight333= " + hight);
        //判定每日任务类型是否有任务
        hight = ShowZhangJieList("3", hight, zhangJieNum);
        Debug.Log("hight444= " + hight);
    }



    //打开章节列表
    public float ShowZhangJieList(string chengJiuType, float hight,int zhangJieNum) {

        //实例化界面
        UI_RoseChengJiuType = (GameObject)Instantiate(Obj_UIChengJiuList_Type);

        //给实例化的脚本指定任务类型
        UI_RoseChengJiuType.GetComponent<Rose_ChengJiuList_Show>().ChengJiuType = chengJiuType;

        string chengJiuTypeID = "";

        //获取当前任务类型对应的文字
        string ChengJiuTypeName = "";
        switch (chengJiuType)
        {

            case "0":
                chengJiuTypeID = "1001";
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("成就首页");
                ChengJiuTypeName = langStr;
                UI_RoseChengJiuType.transform.parent = UIPoint_ChengJiuType_Main.transform;
                UI_RoseChengJiuType.transform.localPosition = new Vector3(0, hight, 0);
                UI_RoseChengJiuType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                hight = -55.0f;

                break;


            case "1":
                chengJiuTypeID = "1002";
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("成就列表");
                ChengJiuTypeName = langStr;
                UI_RoseChengJiuType.transform.parent = UIPoint_ChengJiuType_Fight.transform;
                UI_RoseChengJiuType.transform.localPosition = new Vector3(0, hight, 0);
                UI_RoseChengJiuType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                //计算列表当前高度
                if (Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_1 == "1")
                {
                    hight = hight - zhangJieNum * 50.0f - 55.0f;
                }
                else
                {
                    hight = hight - 55.0f;
                }
                break;

            case "2":
                chengJiuTypeID = "1003";
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("探索成就");
                ChengJiuTypeName = langStr;
                UI_RoseChengJiuType.transform.parent = UIPoint_ChengJiuType_ShouJi.transform;
                UI_RoseChengJiuType.transform.localPosition = new Vector3(0, hight, 0);
                UI_RoseChengJiuType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                //计算列表当前高度
                if (Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_2 == "1")
                {
                    hight = hight - zhangJieNum * 50.0f - 55.0f;
                }
                else
                {
                    hight = hight - 55.0f;
                }
                break;

            case "3":
                chengJiuTypeID = "1004";
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("收集成就");
                ChengJiuTypeName = langStr;
                UI_RoseChengJiuType.transform.parent = UIPoint_ChengJiuType_TanSuo.transform;
                UI_RoseChengJiuType.transform.localPosition = new Vector3(0, hight, 0);
                UI_RoseChengJiuType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                break;
        }


        //检测当前角色任务是否展开
        string ifShowTaskList = "";
        switch (chengJiuType)
        {
            case "0":
                ifShowTaskList = Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_0;
                break;
            case "1":
                ifShowTaskList = Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_1;
                break;
            case "2":
                ifShowTaskList = Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_2;
                break;
            case "3":
                ifShowTaskList = Game_PublicClassVar.Get_game_PositionVar.RoseChengJiuListShow_3;
                break;
        }

        if (ifShowTaskList == "1")
        {
            //展开列表
            //更新任务类型名称
            Rose_ChengJiuList_Show taskList_Show = UI_RoseChengJiuType.GetComponent<Rose_ChengJiuList_Show>();
            taskList_Show.UI_TaskTypeName.GetComponent<Text>().text = ChengJiuTypeName;

            //更新图标显示
            taskList_Show.UIImg_TaskListShow.SetActive(true);
            taskList_Show.UIImg_TaskListShow_2.SetActive(false);

            //显示章节
            LoadChengJiuZhangJie(chengJiuTypeID);

            //显示首页
            if (chengJiuType == "0")
            {
                Obj_ChengJiuShouYeShow.SetActive(true);
                OpenShouYe();
            }
            else {
                Obj_ChengJiuShouYeShow.SetActive(false);
            }
        }
        else
        {
            //收缩列表
            Rose_ChengJiuList_Show taskList_Show = UI_RoseChengJiuType.GetComponent<Rose_ChengJiuList_Show>();
            taskList_Show.UI_TaskTypeName.GetComponent<Text>().text = ChengJiuTypeName;

            //更新图标显示
            taskList_Show.UIImg_TaskListShow.SetActive(false);
            taskList_Show.UIImg_TaskListShow_2.SetActive(true);
        }
        return hight;
    }



    public void LoadChengJiuZhangJie(string chengJiuTypeID)
    {

        //实例化任务UI
        int zhangJieNum = 0;
        string ziDuanName = "";

        for (int i = 0;i<6;i++) {
            string zhangJieName = "";
            switch (i) {

                case 0:
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("通 用");
                    zhangJieName = langStr;
                    ziDuanName = "ChengJiuSet_Com";
                    break;

                case 1:
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第一章");
                    zhangJieName = langStr;
                    ziDuanName = "ChengJiuSet_ZhangJie_1";
                    break;

                case 2:
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第二章");
                    zhangJieName = langStr;
                    ziDuanName = "ChengJiuSet_ZhangJie_2";
                    break;

                case 3:
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第三章");
                    zhangJieName = langStr;
                    ziDuanName = "ChengJiuSet_ZhangJie_3";
                    break;

                case 4:
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第四章");
                    zhangJieName = langStr;
                    ziDuanName = "ChengJiuSet_ZhangJie_4";
                    break;

                case 5:
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第五章");
                    zhangJieName = langStr;
                    ziDuanName = "ChengJiuSet_ZhangJie_5";
                    break;
            }

            zhangJieNum = zhangJieNum + 1;
            GameObject UI_RoseChengJiuListName = (GameObject)Instantiate(Obj_UIChengJiuList_TaskRow);
            Rose_ChengJiuList_Show rose_TaskList_Show = UI_RoseChengJiuType.GetComponent<Rose_ChengJiuList_Show>();
            UI_RoseChengJiuListName.transform.parent = rose_TaskList_Show.UIPoint_TaskName;
            UI_RoseChengJiuListName.transform.localPosition = new Vector3(0, zhangJieNum * -50.0f, 0);
            UI_RoseChengJiuListName.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);


            //增加显示章节成就完成数量
            string comChengJiuStr = Game_PublicClassVar.Get_function_Task.ChengJiu_GetTargetZiDuanChengJiuID_YiWanCheng(chengJiuTypeID, ziDuanName);
            int chengJiuNum = Game_PublicClassVar.Get_function_Task.ChengJiu_GetComTargetStrChengJiuNum(comChengJiuStr);
            //获取当前章节总的成就点数
            string chengJiuStr = Game_PublicClassVar.Get_function_Task.ChengJiu_GetTargetZiDuanChengJiuID(chengJiuTypeID, ziDuanName);
            int weiChengJiuNum = Game_PublicClassVar.Get_function_Task.ChengJiu_GetComTargetStrChengJiuNum(chengJiuStr);

            zhangJieName = zhangJieName;

            //显示章节名称
            Rose_ChengJiuList_row_UIPoint uiPoint = UI_RoseChengJiuListName.GetComponent<Rose_ChengJiuList_row_UIPoint>();
            Text textName = uiPoint.UI_TaskName.GetComponent<Text>();
            textName.text = zhangJieName;
            //特殊处理
            if (Game_PublicClassVar.Get_wwwSet.GameSetLanguage._Language == "English") {
                textName.fontSize = 18;
            }

            uiPoint.UI_TaskNum.GetComponent<Text>().text = "(" + chengJiuNum + "/" + (chengJiuNum + weiChengJiuNum).ToString() + ")";

            //设置属性
            uiPoint.ChengJiuZiDuan = ziDuanName;
            uiPoint.ChengJiuTypeID = chengJiuTypeID;

            //点击列表默认显示第一个
            if (i==0) {
                if (ShowFirstObjStatus) {
                    ShowFirstObjStatus = false;
                    uiPoint.UI_SelectTask();
                }
            }

            /*
            //判定当前任务是否为选中的任务
            if (Game_PublicClassVar.Get_game_PositionVar.NowTaskID == nowTaskID)
            {
                //更改任务名称的颜色及显示选中的任务
                //uiPoint.UI_TaskLv.GetComponent<Text>().color = new Color(1, 1, 1, 1);
                //uiPoint.UI_TaskName.GetComponent<Text>().color = new Color(1, 1, 1, 1);
                uiPoint.UIImg_SelectStatus.SetActive(true);
            }
            */
        }
    }


    //清理子物体
    private void CleanSonGameObject() {

        //清空子物体
        for (int i = 0; i < UIPoint_ChengJiuType_Main.childCount; i++)
        {
            GameObject go = UIPoint_ChengJiuType_Main.GetChild(i).gameObject;
            Destroy(go);
        }

        //清空子物体
        for (int i = 0; i < UIPoint_ChengJiuType_Fight.childCount; i++)
        {
            GameObject go = UIPoint_ChengJiuType_Fight.GetChild(i).gameObject;
            Destroy(go);
        }

        //清空子物体
        for (int i = 0; i < UIPoint_ChengJiuType_ShouJi.childCount; i++)
        {
            GameObject go = UIPoint_ChengJiuType_ShouJi.GetChild(i).gameObject;
            Destroy(go);
        }

        //清空子物体
        for (int i = 0; i < UIPoint_ChengJiuType_TanSuo.childCount; i++)
        {
            GameObject go = UIPoint_ChengJiuType_TanSuo.GetChild(i).gameObject;
            Destroy(go);
        }
    }

	//清理子物体
	private void CleanShouShaSonGameObject() {

		//清空子物体
		for (int i = 0; i < OpenShouShaZhangJieList[0].transform.childCount; i++)
		{
			GameObject go = OpenShouShaZhangJieList[0].transform.GetChild(i).gameObject;
			Destroy(go);
		}

		//清空子物体
		for (int i = 0; i < OpenShouShaZhangJieList[1].transform.childCount; i++)
		{
			GameObject go = OpenShouShaZhangJieList[1].transform.GetChild(i).gameObject;
			Destroy(go);
		}

		//清空子物体
		for (int i = 0; i < OpenShouShaZhangJieList[2].transform.childCount; i++)
		{
			GameObject go = OpenShouShaZhangJieList[2].transform.GetChild(i).gameObject;
			Destroy(go);
		}

		//清空子物体
		for (int i = 0; i < OpenShouShaZhangJieList[3].transform.childCount; i++)
		{
			GameObject go = OpenShouShaZhangJieList[3].transform.GetChild(i).gameObject;
			Destroy(go);
		}

		//清空子物体
		for (int i = 0; i < OpenShouShaZhangJieList[4].transform.childCount; i++)
		{
			GameObject go = OpenShouShaZhangJieList[4].transform.GetChild(i).gameObject;
			Destroy(go);
		}
	}
    
    //关闭界面时调用
    public void CloseUI_TaskList() {
        Destroy(this.gameObject);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().RoseChengJiu_Status = false;
    }

    public void MoveUI_TaskList() {

        //移动列表
        //获取当前下拉进度条进度
        float nowBarValue = Rose_RoseChengJiuBar.GetComponent<Scrollbar>().value;
        if (nowBarValue > 0.0) {
            UIPoint_ChengJiuType.transform.localPosition = new Vector3(UIPoint_ChengJiuType.transform.localPosition.x, 300.0f * nowBarValue/2, UIPoint_ChengJiuType.transform.localPosition.z);
        }
    }

    //显示成就信息
    public void ShowChengJiuList(string chengJiuIDSet_WeiWanCheng,string chengJiuIDSet_YiWanCheng) {

        //Debug.Log("chengJiuIDSet_WeiWanCheng = " + chengJiuIDSet_WeiWanCheng + "；chengJiuIDSet_YiWanCheng = " + chengJiuIDSet_YiWanCheng);
        Obj_ChengJiuDataSetList.SetActive(true);

        //清理展示列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ChengJiuDataSet);

        //开始展示
        /*
        if (chengJiuIDSet_WeiWanCheng == "") {
            return;
        }
        */

        //显示未完成成就
        if (chengJiuIDSet_WeiWanCheng != "")
        {
            string[] chengJiuIDList = chengJiuIDSet_WeiWanCheng.Split(';');
            for (int i = 0; i < chengJiuIDList.Length; i++)
            {
                GameObject showObj = (GameObject)Instantiate(Obj_ChengJiuDataShow);
                showObj.transform.parent = Obj_ChengJiuDataSet.transform;
                showObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                showObj.GetComponent<Rose_ChengJiuDataShowList>().ChengJiuID = chengJiuIDList[i];
                showObj.GetComponent<Rose_ChengJiuDataShowList>().IfComChengJiu = false;
                showObj.GetComponent<Rose_ChengJiuDataShowList>().UpdateChengJiuDataShow();
            }
        }
        //显示已完成成就
        if (chengJiuIDSet_YiWanCheng != "") {
            string[] chengJiuIDList_YiWanCheng = chengJiuIDSet_YiWanCheng.Split(';');
            for (int i = 0; i < chengJiuIDList_YiWanCheng.Length; i++)
            {
                GameObject showObj = (GameObject)Instantiate(Obj_ChengJiuDataShow);
                showObj.transform.parent = Obj_ChengJiuDataSet.transform;
                showObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                showObj.GetComponent<Rose_ChengJiuDataShowList>().ChengJiuID = chengJiuIDList_YiWanCheng[i];
                showObj.GetComponent<Rose_ChengJiuDataShowList>().IfComChengJiu = true;
                showObj.GetComponent<Rose_ChengJiuDataShowList>().UpdateChengJiuDataShow();
            }
        }
    }

    void OpenShouYe() {

        //清除
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ChengJiuRewardNumSet);

        //关闭列表展示
        Obj_ChengJiuDataSetList.SetActive(false);

        //读取成就奖励
        string chengJiuRewardSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RewardIDSet", "ID", "1001", "ChengJiuAll_Template");
        string[] chengJiuRewardList = chengJiuRewardSet.Split(';');

        for (int i = 0; i < chengJiuRewardList.Length; i++) {
            GameObject obj = (GameObject)Instantiate(Obj_ChengJiuRewardNum);
            obj.transform.SetParent(Obj_ChengJiuRewardNumSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_ChengJiuRewardNumSet>().ChengJiuRewardID = chengJiuRewardList[i];
            obj.GetComponent<UI_ChengJiuRewardNumSet>().ShowChengJiuRewardNum();

            //默认打开第一个
            if (i == 0) {
                obj.GetComponent<UI_ChengJiuRewardNumSet>().SelectReward();
            }
        }

        //显示总的成就点数
        ObscuredInt chengJiuNum =  Game_PublicClassVar.Get_function_Task.ChengJiu_GetComChengJiuNum();
        Obj_ChengJiuAllNum.GetComponent<Text>().text = chengJiuNum.ToString();
    }

    public void Btn_LingQuReward() {
        Debug.Log("领取奖励");
        Game_PublicClassVar.Get_function_Task.ChengJiu_RewardLingQu(Obj_XuanZhongRewardNum.GetComponent<UI_ChengJiuRewardNumSet>().ChengJiuRewardID);
    }

    //点击成就列表
    public void Btn_ChengJiuSetShow() {
        clearnListBtn();
        Obj_ChengJiuSet.SetActive(true);
        Obj_JingLingSet.SetActive(false);
		Obj_ShouShaSet.SetActive (false);

        //显示底图
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_Btn_ChengJiu_Img.GetComponent<Image>().sprite = img;
        //更新通用图标显示
        Obj_Btn_ChengJiu_Text.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);
    }

    //点击精灵列表按钮
    public void Btn_JingLingSetShow() {
        clearnListBtn();
        Obj_ChengJiuSet.SetActive(false);
        Obj_JingLingSet.SetActive(true);
		Obj_ShouShaSet.SetActive (false);
        //显示底图
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_Btn_JingLing_Img.GetComponent<Image>().sprite = img;
        //更新通用图标显示
        Obj_Btn_JingLing_Text.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

        //展示精灵列表
        JingLingShowZhangJieList();
    }

	//点击精灵列表按钮
	public void Btn_ShouShaSetShow() {
		clearnListBtn();
		Obj_ChengJiuSet.SetActive(false);
		Obj_JingLingSet.SetActive(false);
		Obj_ShouShaSet.SetActive (true);

		//显示底图
		object obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
		Sprite img = obj as Sprite;
		Obj_Btn_ShouSha_Img.GetComponent<Image>().sprite = img;
		//更新通用图标显示
		Obj_Btn_ShouSha_Text.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

		//展示首杀列表
		ShouShaUpdate();

		//默认显示第一章最后boss
		ShowShouShaUI("70001906");
	}

    public void clearnListBtn()
    {
        Obj_Btn_ChengJiu_Text.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_Btn_JingLing_Text.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
		Obj_Btn_ShouSha_Text.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_Btn_ChengJiu_Img.GetComponent<Image>().sprite = img;
        Obj_Btn_JingLing_Img.GetComponent<Image>().sprite = img;
		Obj_Btn_ShouSha_Img.GetComponent<Image>().sprite = img;
    }


    //展示精灵章节
    public void JingLingShowZhangJieList() {
        //清空目标
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_JingLingZhangJieShowSet);

        for (int i = 1; i <= 5; i++) {
            GameObject obj = (GameObject)Instantiate(Obj_JingLingZhangJieShow);
            obj.transform.SetParent(Obj_JingLingZhangJieShowSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_JingLingZhangJie>().ZhangJieID = i.ToString();
            obj.GetComponent<UI_JingLingZhangJie>().ShowZhangJieName();

            //默认显示第一章节
            if (i == 1) {
                obj.GetComponent<UI_JingLingZhangJie>().Btn_ZhangJie();
            }
        }
    }

    //精灵
    public void JingLingShow(string zhangJieID) {

        //清空精灵显示
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_JingLingShowSet);
        Obj_JingLingShowSet.transform.localPosition = new Vector3(Obj_JingLingShowSet.transform.localPosition.x, -800, Obj_JingLingShowSet.transform.localPosition.z);

        //显示精灵ID
        string jingLingID = "10001";
        //精灵起始ID对应不同章节
        switch (zhangJieID) {

            case "1":
                jingLingID = "10001";
                break;

            case "2":
                jingLingID = "20001";
                break;

            case "3":
                jingLingID = "30001";
                break;

            case "4":
                jingLingID = "40001";
                break;

            case "5":
                jingLingID = "50001";
                break;
        }

        while (true) {

            if (jingLingID == "" || jingLingID == "0")
            {
                break;
            }
            else {
                //展示精灵ID
                GameObject jingLingObj = (GameObject)Instantiate(Obj_JingLingShow);
                jingLingObj.transform.SetParent(Obj_JingLingShowSet.transform);
                jingLingObj.transform.localScale = new Vector3(1, 1, 1);
                jingLingObj.GetComponent<UI_JingLingShow>().JingLingID = jingLingID;
                jingLingObj.GetComponent<UI_JingLingShow>().JingLingShow();
            }

            //下一个精灵ID
            jingLingID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", jingLingID, "Spirit_Template");
        }
    }









	//成就更新
	private void ShouShaUpdate()
	{

		float hight = 0.0f;
		int zhangJieNum = 5;        //章节数量
		//Debug.Log("hight000= " + hight);
		hight = ShowShouShaZhangJieList("0", hight, zhangJieNum);
		//Debug.Log("hight111= " + hight);
		hight = ShowShouShaZhangJieList("1", hight, zhangJieNum);
		//Debug.Log("hight222= " + hight);
		hight = ShowShouShaZhangJieList("2", hight, zhangJieNum);
		//Debug.Log("hight333= " + hight);
		hight = ShowShouShaZhangJieList("3", hight, zhangJieNum);
		//Debug.Log("hight444= " + hight);
		hight = ShowShouShaZhangJieList("4", hight, zhangJieNum);
		//Debug.Log("hight444= " + hight);
	}



	//打开章节列表
	public float ShowShouShaZhangJieList(string shoushaType, float hight,int zhangJieNum) {

		//实例化界面
		GameObject UI_RoseShouShaType = (GameObject)Instantiate(Obj_UIShouShaList_Type);

		//给实例化的脚本指定任务类型
		//UI_RoseChengJiuType.GetComponent<Rose_ChengJiuList_Show>().ChengJiuType = chengJiuType;

		string shouShaIDSet = "";

		//获取当前任务类型对应的文字
		string ChengJiuTypeName = "";
		switch (shoushaType)
		{

		case "0":
				shouShaIDSet = "70001901,70001902,70001903,70001904,70001905,70001906";
				zhangJieNum = shouShaIDSet.Split (',').Length;
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第一章");
                ChengJiuTypeName = langStr;
				UI_RoseShouShaType.transform.parent = OpenShouShaZhangJieList[int.Parse(shoushaType)].transform;
				UI_RoseShouShaType.transform.localPosition = new Vector3 (0, hight, 0);
				UI_RoseShouShaType.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
				//hight = -55.0f;
				if (OpenShouShaZhangJie_1) {
					hight = hight - zhangJieNum * 50.0f - 55.0f;
				} else {
					hight = hight - 55.0f;
				}

				break;


			case "1":
			    shouShaIDSet = "70002901,70002902,70002904,70002903";
			    zhangJieNum = shouShaIDSet.Split (',').Length;
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第二章");
                ChengJiuTypeName = langStr;
				UI_RoseShouShaType.transform.parent = OpenShouShaZhangJieList[int.Parse(shoushaType)].transform;
				UI_RoseShouShaType.transform.localPosition = new Vector3(0, hight, 0);
				UI_RoseShouShaType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

				//计算列表当前高度
				if (OpenShouShaZhangJie_2)
				{
					hight = hight - zhangJieNum * 50.0f - 55.0f;
				}
				else
				{
					hight = hight - 55.0f;
				}
				break;

			case "2":
			    shouShaIDSet = "70003901,70003902,70003903,70003904";
			    zhangJieNum = shouShaIDSet.Split (',').Length;
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第三章");
                ChengJiuTypeName = langStr;
				UI_RoseShouShaType.transform.parent = OpenShouShaZhangJieList[int.Parse(shoushaType)].transform;
				UI_RoseShouShaType.transform.localPosition = new Vector3(0, hight, 0);
				UI_RoseShouShaType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

				//计算列表当前高度
				if (OpenShouShaZhangJie_3)
				{
					hight = hight - zhangJieNum * 50.0f - 55.0f;
				}
				else
				{
					hight = hight - 55.0f;
				}
				break;

			case "3":
			    shouShaIDSet = "70004901,70004902,70004903,70004904";
			    zhangJieNum = shouShaIDSet.Split (',').Length;
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第四章");
                ChengJiuTypeName = langStr;
				UI_RoseShouShaType.transform.parent = OpenShouShaZhangJieList[int.Parse(shoushaType)].transform;
				UI_RoseShouShaType.transform.localPosition = new Vector3(0, hight, 0);
				UI_RoseShouShaType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				
				//计算列表当前高度
				if (OpenShouShaZhangJie_4)
				{
					hight = hight - zhangJieNum * 50.0f - 55.0f;
				}
				else
				{
					hight = hight - 55.0f;
				}
				break;

			case "4":
			    shouShaIDSet = "70005901,70005902,70005903,70005904,70005905";
			    zhangJieNum = shouShaIDSet.Split (',').Length;
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第五章");
                ChengJiuTypeName = langStr;
				UI_RoseShouShaType.transform.parent = OpenShouShaZhangJieList[int.Parse(shoushaType)].transform;
				UI_RoseShouShaType.transform.localPosition = new Vector3(0, hight, 0);
				UI_RoseShouShaType.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

				//计算列表当前高度
				if (OpenShouShaZhangJie_5)
				{
					hight = hight - zhangJieNum * 50.0f - 55.0f;
				}
				else
				{
					hight = hight - 55.0f;
				}
				break;
		}


		//检测当前角色任务是否展开
		bool ifShowList = false;
		switch (shoushaType)
		{
		case "0":
			ifShowList = OpenShouShaZhangJie_1;
			break;
		case "1":
			ifShowList = OpenShouShaZhangJie_2;
			break;
		case "2":
			ifShowList = OpenShouShaZhangJie_3;
			break;
		case "3":
			ifShowList = OpenShouShaZhangJie_4;
			break;
		case "4":
			ifShowList = OpenShouShaZhangJie_5;
			break;
		}

		if (ifShowList)
		{
			//展开列表
			//更新任务类型名称
			Rose_ShouShaList_Show taskList_Show = UI_RoseShouShaType.GetComponent<Rose_ShouShaList_Show>();
			taskList_Show.UI_TaskTypeName.GetComponent<Text>().text = ChengJiuTypeName;

			//更新图标显示
			taskList_Show.UIImg_TaskListShow.SetActive(true);
			taskList_Show.UIImg_TaskListShow_2.SetActive(false);

			//显示章节
			LoadShouShaZhangJie(shouShaIDSet,UI_RoseShouShaType);

        }
		else
		{
			//收缩列表
			Rose_ShouShaList_Show taskList_Show = UI_RoseShouShaType.GetComponent<Rose_ShouShaList_Show>();
			taskList_Show.UI_TaskTypeName.GetComponent<Text>().text = ChengJiuTypeName;

			//更新图标显示
			taskList_Show.UIImg_TaskListShow.SetActive(false);
			taskList_Show.UIImg_TaskListShow_2.SetActive(true);

        }

        UI_RoseShouShaType.GetComponent<Rose_ShouShaList_Show> ().ChengJiuType = shoushaType;
		return hight;
	}


	public void LoadShouShaZhangJie(string monsterIDSet,GameObject obj_ShouShaType)
	{
		Debug.Log ("展示首杀列表");
		string[] monsterIDList = monsterIDSet.Split (',');
		//实例化任务UI
		int zhangJieNum = 0;
		string ziDuanName = "";

		for (int i = 0; i<monsterIDList.Length; i++) {

			string monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", monsterIDList[i], "Monster_Template");

			zhangJieNum = zhangJieNum + 1;
			GameObject UI_RoseChengJiuListName = (GameObject)Instantiate(Obj_UIShouShaZhangJie_Row);
			Rose_ShouShaList_Show rose_TaskList_Show = obj_ShouShaType.GetComponent<Rose_ShouShaList_Show>();
			UI_RoseChengJiuListName.transform.parent = rose_TaskList_Show.UIPoint_TaskName;
			UI_RoseChengJiuListName.transform.localPosition = new Vector3(0, zhangJieNum * -50.0f, 0);
			UI_RoseChengJiuListName.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

			//显示章节名称
			Rose_ShouShaList_row_UIPoint uiPoint = UI_RoseChengJiuListName.GetComponent<Rose_ShouShaList_row_UIPoint>();
			Text textName = uiPoint.UI_TaskName.GetComponent<Text>();
			textName.text = monsterName;

			//收集id
			uiPoint.ShouShaID = monsterIDList [i];

			//点击列表默认显示第一个
			if (i==0) {
				if (ShowFirstObjStatus) {
					ShowFirstObjStatus = false;
					uiPoint.UI_SelectTask();
				}
			}
		}
	}

	//展示首杀信息
	public void ShowShouShaUI(string shoushaID){

		//显示怪物信息
		string monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", shoushaID, "Monster_Template");
		string monsterLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", shoushaID, "Monster_Template");
		//string jinglingID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpiritID", "ID", shoushaID, "Monster_Template");
		//string monsterIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", jinglingID, "Spirit_Template");
		string monsterIcon = shoushaID;
		string monsterMonsterRace = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterRace", "ID", shoushaID, "Monster_Template");
		string raceStr = "通用";
		switch (monsterMonsterRace) {
			
		case "1":
			raceStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("野兽");
			break;

		case "2":
			raceStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("人类");
			break;

		case "3":
			raceStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("恶魔");
			break;
		}

        string monsterDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", shoushaID, "Monster_Template").ToString();
        monsterDes = monsterDes.Replace("\\n", "\n");

        //获取掉落
        string monsterDropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropID", "ID", shoushaID, "Monster_Template");
		string monsterMainDrop = getDropItemIDSet(monsterDropID);

		//string monsterMainDrop = "1;2;3";

		//Obj_ShouShaShow_Name.GetComponent<Text> ().text = monsterName + "(Lv." + monsterLv + ")";
		Obj_ShouShaShow_Name.GetComponent<Text> ().text = monsterName;
		Obj_ShouShaShow_Des.GetComponent<Text> ().text = monsterDes;
		Obj_ShouShaShow_Race.GetComponent<Text> ().text = raceStr;
		object obj = Resources.Load("HeadIcon/BossIcon/" + monsterIcon, typeof(Sprite));
		Sprite itemIcon = obj as Sprite;
		Obj_ShouShaShow_Icon.GetComponent<Image>().sprite = itemIcon;

		//清除
		Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ShouShaShow_DropSet);

		//成就奖励
		if (monsterMainDrop != "") {
			string[] rewardItemList = monsterMainDrop.Split(';');
			//Debug.Log ("rewardItemList的长度 = " + rewardItemList);
			for (int i = 0; i < rewardItemList.Length; i++) {

				//显示奖励
				string[] itemList = rewardItemList[i].Split(',');
				GameObject itemObj = (GameObject)Instantiate(Obj_ShouShaShow_DropItem);
				itemObj.transform.SetParent(Obj_ShouShaShow_DropSet.transform);
				itemObj.transform.localScale = new Vector3(1, 1, 1);
				itemObj.GetComponent<UI_Common_ItemIcon>().ItemID = itemList[0];
				itemObj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = 0;
				itemObj.GetComponent<UI_Common_ItemIcon>().UpdateItem();
			}

			//设置列表长度
			if(rewardItemList.Length>10){
				int num = rewardItemList.Length - 10;
				float shousha_x = 1000 + num * 100;
				Obj_ShouShaShow_DropSet.GetComponent<RectTransform> ().sizeDelta = new Vector2 (shousha_x,80);
				Obj_ShouShaShow_DropSet.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (shousha_x/2,Obj_ShouShaShow_DropSet.GetComponent<RectTransform> ().anchoredPosition3D.y,Obj_ShouShaShow_DropSet.GetComponent<RectTransform> ().anchoredPosition3D.z);
			}
		}

        string langstr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("正在向服务器请求首杀数据");

        Obj_ShouShaName_NanDu_1.GetComponent<Text> ().text = langstr + "...";
		Obj_ShouShaName_NanDu_2.GetComponent<Text> ().text = langstr + "...";
		Obj_ShouShaName_NanDu_3.GetComponent<Text> ().text = langstr + "...";

		string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

		Pro_ComStr_2 pro_ComStr_2 = new Pro_ComStr_2 ();
		pro_ComStr_2.str_1 = zhanghaoID;
		pro_ComStr_2.str_2 = shoushaID;

		Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001101, pro_ComStr_2);

        ShouShaMonsterID = shoushaID;


        //显示是否已经刷新
        string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (deathMonsterIDListStr.Contains(ShouShaMonsterID))
        {
            langstr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("已击杀");
            Obj_BossShuaXinShow.GetComponent<Text>().text = "("+ langstr + ")";
            Obj_BossShuaXinShow.GetComponent<Text>().color = Color.red;
        }
        else {
            langstr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("已刷新");
            Obj_BossShuaXinShow.GetComponent<Text>().text = "("+ langstr + ")";
            Obj_BossShuaXinShow.GetComponent<Text>().color = new Color(0.25f,0.53f,0.15f);
        }
    }

    public void ShowShouShaName(){

        string langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("击杀时间");
        string langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("未击杀");

        string langstr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("普通模式");
        string langstr_4 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("困难模式");
        string langstr_5 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("地狱模式");

        Pro_ShouShaNameList shoushaList = pro_ShouShaNameList;

		if (shoushaList.ShouShaTime_1 != "" && shoushaList.ShouShaTime_1 != "0"&& shoushaList.ShouShaTime_1 != null) {
			DateTime dateTime = Game_PublicClassVar.Get_wwwSet.GetTime (shoushaList.ShouShaTime_1);
			string showTimeStr = dateTime.ToString ("yyyy-MM-dd HH:mm:ss");
            
            Obj_ShouShaName_NanDu_1.GetComponent<Text> ().text = langstr_3 + " : " + shoushaList.PlayerName_1 + " ( "+ langstr_1 + ":" + showTimeStr + " )";
		} else {
			Obj_ShouShaName_NanDu_1.GetComponent<Text> ().text = langstr_3 + " : "+ langstr_2 + "！";
		}

		if (shoushaList.ShouShaTime_2 != "" && shoushaList.ShouShaTime_2 != "0"&& shoushaList.ShouShaTime_2 != null) {

			DateTime dateTime = Game_PublicClassVar.Get_wwwSet.GetTime (shoushaList.ShouShaTime_2);
			string showTimeStr = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			Obj_ShouShaName_NanDu_2.GetComponent<Text> ().text = langstr_4 + " : " +shoushaList.PlayerName_2 + " ( "+ langstr_1 + ":"+showTimeStr + " )";
		}else {
			Obj_ShouShaName_NanDu_2.GetComponent<Text> ().text = langstr_4 + " : "+ langstr_2 + "！";
		}

		if (shoushaList.ShouShaTime_3 != "" && shoushaList.ShouShaTime_3 != "0"&& shoushaList.ShouShaTime_3 != null) {

			DateTime dateTime = Game_PublicClassVar.Get_wwwSet.GetTime (shoushaList.ShouShaTime_3);
			string showTimeStr = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			Obj_ShouShaName_NanDu_3.GetComponent<Text> ().text = langstr_5 + " : " +shoushaList.PlayerName_3 + " ( "+ langstr_1 + ":"+showTimeStr + " )";
		}else {
			Obj_ShouShaName_NanDu_3.GetComponent<Text> ().text = langstr_5 + " : "+ langstr_2 + "！";
		}

	}

	private string getDropItemIDSet(string dropID){
	
		//是否有子掉落
		bool DropSonStatus = false;
		int dropLoopNum = 0;                    //掉落循环次数
		string dropIDInitial = dropID;          //设置初始掉落
		string returnDropItemStr = "";
		//循环每行掉落数据
		do
		{
			DropSonStatus = true;
			//生成每行掉落数据
			for (int i = 1; i <= 10; i++)
			{
				string dropItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemID" + i.ToString(), "ID", dropID, "Drop_Template");
				//获取道具品质
				string quality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", dropItemID, "Item_Template");
				string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", dropItemID, "Item_Template");
				if(quality!=""&& dropItemID!="1")
                {
					//橙色品质排在前面
					if(int.Parse(quality)>=5 && itemType!="4"){
						returnDropItemStr = dropItemID + ";" + returnDropItemStr ;
					}
						
					if(int.Parse(quality)==4 && itemType!="4"){
						returnDropItemStr = returnDropItemStr + dropItemID + ";";
					}

				}
			}

			//获取子掉落
			dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropSonID", "ID", dropID,"Drop_Template");
			//没有子掉落循环取消（因为掉落ID里面可以套掉落ID）
			if (dropID == "0")
			{
				DropSonStatus = false;
				//循环100次强制结束循环
				if (dropLoopNum > 100)
				{
					DropSonStatus = false;
				}
			}
		}
		while (DropSonStatus);

		if (returnDropItemStr != "") {
			returnDropItemStr = returnDropItemStr.Substring (0, returnDropItemStr.Length - 1);
		}

		return returnDropItemStr;
	}


    //展示首杀奖励
    public void Btn_ShowShouShaReward() {

        if (shouShaShowRewardObj != null)
        {
            Destroy(shouShaShowRewardObj);
        }
        string shouShaRewardList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShouShaRewardList", "ID", ShouShaMonsterID, "Monster_Template");
        if (shouShaRewardList == "" || shouShaRewardList == "0" || shouShaRewardList == null) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_210");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("此Boss无首杀奖励！");
            return;
        }

        shouShaShowRewardObj = (GameObject)Instantiate(Obj_ShouShaShowReward);
        shouShaShowRewardObj.transform.SetParent(Obj_ShouShaShowRewardSet.transform);
        shouShaShowRewardObj.transform.localScale = new Vector3(1, 1, 1);
        shouShaShowRewardObj.transform.localPosition = new Vector3(0, 0, 0);

        //shouShaRewardList = "1,100;2,100;3,100|1,200;2,200;3,200|1,300;2,300;3,300";
        shouShaShowRewardObj.GetComponent<Rose_ShowShouShaReward>().ShouShaRewardStr = shouShaRewardList;
        shouShaShowRewardObj.GetComponent<Rose_ShowShouShaReward>().ShowReward();
    }
}
