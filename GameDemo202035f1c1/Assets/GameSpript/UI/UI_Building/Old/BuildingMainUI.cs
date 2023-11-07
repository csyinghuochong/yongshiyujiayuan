using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BuildingMainUI : MonoBehaviour {

    public GameObject Obj_Building;
    public GameObject Obj_Territory;
    public GameObject Obj_MainUISet;
    public GameObject Obj_StarPve;
    public GameObject Obj_TrainCamp;
    public GameObject Obj_Fortress;
    public bool ifHideMainUI;        //是否隐藏主界面UI
    private bool showNameUI;
    private float showNameTimeSum;
    private GameObject obj_GameSetting;

    //主界面
    public GameObject Obj_RoseName;
    public GameObject Obj_RoseLv;
    public GameObject Obj_RoseGold;
    public GameObject Obj_RoseRmb;
    public GameObject Obj_RoseTiLiText;
    public GameObject Obj_RoseTiLiImg;
    public GameObject Obj_RoseExpText;
    public GameObject Obj_RoseExpImg;
    public bool UpdataRoseStatus;

    public GameObject Obj_BuildingGold;
    public GameObject Obj_Food;
    public GameObject Obj_Wood;
    public GameObject Obj_Stone;
    public GameObject Obj_Iron;


    public GameObject Obj_Gold;
    public GameObject Obj_Rmb;
    public GameObject Obj_Honor;
    //public bool UpdataMainUIStatus;

    private float exitTimeSum;

	// Use this for initialization
	void Start () {
        UpdataRoseStatus = true;
	    
        //初始化背景音乐是否播放
        //Debug.Log("一一一一一一一一一一一一一一:" + Game_PublicClassVar.Get_game_PositionVar.GameSourceValue);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingBGM.GetComponent<AudioSource>().volume = Game_PublicClassVar.Get_game_PositionVar.GameSourceValue;

        return;
        //updataMainUI();
    }
	
	// Update is called once per frame
	void Update () {
        return;
        //打开子界面是否隐藏主建筑UI
        if (ifHideMainUI)
        {
            //Obj_MainUISet.active = false;
        }
        else {
            //Obj_MainUISet.active = true;
        }

        //延迟0.5秒显示建筑名称
        if (showNameUI) {
            showNameTimeSum = showNameTimeSum + Time.deltaTime;
            if (showNameTimeSum >= 0.5f) {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingNameSet.SetActive(true);
                showNameTimeSum = 0.0f;
                showNameUI = false;
            }
        }

        /*
        if (UpdataRoseStatus) { 
            
            //读取当前角色经验和最大值
            string roseExp_Now = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            string roseExp_Max = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseUpExp", "RoseLv", roseLv, "RoseExp_Template");

            Obj_RoseLv.GetComponent<Text>().text = "等级:" + Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Obj_RoseName.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Obj_RoseGold.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Obj_RoseRmb.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

            //读取当前体力值
            string tiLi_Now = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TiLi", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

            Obj_RoseTiLiText.GetComponent<Text>().text = "体力：" + tiLi_Now + "/100";
            float chaValue = float.Parse(roseExp_Now) / float.Parse(roseExp_Max);
            Obj_RoseExpText.GetComponent<Text>().text = "经验：" + (int)((chaValue)*100)+"%";
            Obj_RoseTiLiImg.GetComponent<Image>().fillAmount = float.Parse(tiLi_Now)/100;
            Obj_RoseExpImg.GetComponent<Image>().fillAmount = chaValue;

            UpdataRoseStatus = false;
        }
        */

        updataMainUI();


        //检测是否修改数据
        if (Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint("系统监测你修改了数据,请加群联系管理解决,不思悔改将立即删除账号信息！");
            exitTimeSum = exitTimeSum + Time.deltaTime;
            if (exitTimeSum >= 5.0f) {
                Application.Quit();
            }
        }
	}

    //点击市政厅按钮
    public void Btn_ClickBuilding() {

        //检测是否修改数据
        if (Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame)
        {
            return;
        }

        if (Obj_Building.active)
        {
            Obj_Building.active = false;
            ifHideMainUI = false;
            Camera.main.GetComponent<CameraAI>().BuildEnterStatus = false;
            Camera.main.GetComponent<CameraAI>().BuildExitStatus = true;
            //Debug.Log("退出");
            showNameUI = true;

        }
        else {
            Obj_Building.active = true;
            ifHideMainUI = true;
        }
    }

    //点击市领到按钮
    public void Btn_ClickTerritory()
    {
        //检测是否修改数据
        if (Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame)
        {
            return;
        }

        if (Obj_Territory.active)
        {
            Obj_Territory.active = false;
            ifHideMainUI = false;
            //退出时,镜头切换
            Camera.main.GetComponent<CameraAI>().BuildEnterStatus = false;
            Camera.main.GetComponent<CameraAI>().BuildExitStatus = true;
            //Debug.Log("退出");
            showNameUI = true;
        }
        else
        {
            Obj_Territory.active = true;
            ifHideMainUI = true;
        }
    }

    //点击市领到按钮
    public void Btn_ClickFortress()
    {
        //检测是否修改数据
        if (Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame)
        {
            return;
        }

        if (Obj_Fortress.active)
        {
            Obj_Fortress.active = false;
            ifHideMainUI = false;
            //退出时,镜头切换
            Camera.main.GetComponent<CameraAI>().BuildEnterStatus = false;
            Camera.main.GetComponent<CameraAI>().BuildExitStatus = true;
            //Debug.Log("退出");
            showNameUI = true;
        }
        else
        {
            Obj_Fortress.active = true;
            ifHideMainUI = true;
            Obj_Fortress.GetComponent<UI_Fortress>().UpdataShowStatus = true;
        }
    }

    //点击地图按钮
    public void Btn_ClickStarPVE() {


        //检测是否修改数据
        if (Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame) {
            return;
        }

        //取消新手引导
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().Obj_YinDaoSet_MaoXian.SetActive(false);

        if (Obj_StarPve.active)
        {
            Obj_StarPve.active = false;
            ifHideMainUI = false;
            //showNameUI = true;
        }
        else
        {
            Obj_StarPve.active = true;
            ifHideMainUI = true;
            Obj_StarPve.GetComponent<UI_EnterPVESet>().UpdataZhangJieStatus = true;
            Game_PublicClassVar.Get_function_UI.PlaySource("10001", "1");
        }
    }

    //点击训练营地按钮
    public void Btn_ClickTrainCamp()
    {
        //检测是否修改数据
        if (Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame)
        {
            return;
        }

        if (Obj_TrainCamp.active)
        {
            Obj_TrainCamp.active = false;
            ifHideMainUI = false;
            //退出时,镜头切换
            Camera.main.GetComponent<CameraAI>().BuildEnterStatus = false;
            Camera.main.GetComponent<CameraAI>().BuildExitStatus = true;
            //Debug.Log("退出");
            showNameUI = true;
        }
        else
        {
            Obj_TrainCamp.active = true;
            ifHideMainUI = true;
            //Obj_TrainCamp.GetComponent<UI_EnterPVESet>().UpdataZhangJieStatus = true;
        }
    }


    void updataMainUI() {
        /*
        Obj_BuildingGold.GetComponent<Text>().text = Game_PublicClassVar.Get_game_PositionVar.BuildingGold.ToString();
        Obj_Food.GetComponent<Text>().text = Game_PublicClassVar.Get_game_PositionVar.Food.ToString();
        Obj_Wood.GetComponent<Text>().text = Game_PublicClassVar.Get_game_PositionVar.Wood.ToString();
        Obj_Stone.GetComponent<Text>().text = Game_PublicClassVar.Get_game_PositionVar.Stone.ToString();
        Obj_Iron.GetComponent<Text>().text = Game_PublicClassVar.Get_game_PositionVar.Iron.ToString();
        */

        Obj_Gold.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_Rmb.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_Honor.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryHonor", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
    }

    //游戏设置
    public void OpenGameSetting() {
        if (obj_GameSetting == null) {
            obj_GameSetting = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_GameSetting);
                obj_GameSetting.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.transform);
                obj_GameSetting.transform.localPosition = Vector3.zero;
                obj_GameSetting.transform.localScale = new Vector3(1,1,1);
        }
    }

    //进入家园
    public void Btn_GotoJiaYuan() {

        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= 13)
        {
            Game_PublicClassVar.Get_function_Rose.RoseMoveTargetMap("JiaYuan");
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("13级开启家园功能!");
        }

    }

    //通用关闭
    public void Close_UI() {

        ifHideMainUI = false;
        //退出时,镜头切换
        Camera.main.GetComponent<CameraAI>().BuildEnterStatus = false;
        Camera.main.GetComponent<CameraAI>().BuildExitStatus = true;
        //Debug.Log("退出");
        showNameUI = true;
      
        if (Application.loadedLevelName == "EnterGame")
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RightDownSet.SetActive(true);
        }

        //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_HeadSet.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(true);
        //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUISet.SetActive(true);
    }
}
