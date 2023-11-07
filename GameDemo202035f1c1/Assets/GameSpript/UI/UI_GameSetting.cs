using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_GameSetting : MonoBehaviour {

    public GameObject Obj_RoseName;
    public GameObject Obj_RoseNameText;
    public GameObject Obj_RoseMusicText;
    public GameObject Obj_RoseMusicText_YinXiao;
    public GameObject Obj_RoseDanJiImg;
    public GameObject Obj_Bgm;
    public GameObject Obj_ZhangHaoIDStr;
    //public GameObject Obj_YaoGanBtnText;
    public GameObject Obj_ShiJianChuo;
    public GameObject Obj_BanBenText;
    public GameObject Obj_ServerGameVersion;

    public GameObject Obj_YaoGanImg_1;
    public GameObject Obj_YaoGanImg_2;
    public GameObject Obj_YaoGanImg_3;

    public GameObject Obj_PetMoShiImg_1;
    public GameObject Obj_PetMoShiImg_2;
    public GameObject Obj_PetMoShiImg_3;

    public GameObject Obj_NanDuImg_1;
    public GameObject Obj_NanDuImg_2;
    public GameObject Obj_NanDuImg_3;

    public GameObject Obj_Language_0;       //中文勾选
    public GameObject Obj_Language_1;       //英文勾选

    public GameObject Obj_ShuaXin_0;       //流畅
    public GameObject Obj_ShuaXin_1;       //高级

    public GameObject Obj_ChengHaoXuanZe;

	public GameObject Obj_UpdatePlay;
	public GameObject Obj_SavePlay;
	public GameObject Obj_ServerObj;
    private bool serverNameStatus;

    public GameObject Obj_RenZhengObj;
    public GameObject RenZhengObj;

    public GameObject Obj_SettingSet;

    public GameObject Obj_ShenFenXinXiSet;
    public GameObject Obj_BtnYinSi;

    public GameObject Obj_GuaJiSet;

    public GameObject ObjBtn_Up;
    public GameObject ObjBtn_Down;
    public GameObject ObjBtn_RenZheng;

	// Use this for initialization
	void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        //获取游戏名称并显示
        string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_RoseNameText.GetComponent<Text>().text = roseName;
        //显示声音状态
        if (Game_PublicClassVar.Get_game_PositionVar.GameSourceValue == 1)
        {
            //Obj_RoseMusicText.GetComponent<Text>().text = "关";
			Obj_RoseMusicText.SetActive(true);
        }
        else {
            //Obj_RoseMusicText.GetComponent<Text>().text = "开";
            Obj_RoseMusicText.SetActive(false);
        }

        //显示声音状态
        if (Game_PublicClassVar.Get_game_PositionVar.GameSourceValue_YinXiao == 1)
        {
            //Obj_RoseMusicText.GetComponent<Text>().text = "关";
            Obj_RoseMusicText_YinXiao.SetActive(true);
        }
        else
        {
            //Obj_RoseMusicText.GetComponent<Text>().text = "开";
            Obj_RoseMusicText_YinXiao.SetActive(false);
        }

        //显示联网模式
        if (Game_PublicClassVar.Get_game_PositionVar.GameDanJiStatus == 0)
        {
            //Obj_RoseMusicText.GetComponent<Text>().text = "关";
            Obj_RoseDanJiImg.SetActive(false);
        }
        else
        {
            //Obj_RoseMusicText.GetComponent<Text>().text = "开";
            Obj_RoseDanJiImg.SetActive(true);
        }

        //显示账号ID
        ZhangHaoID_Show();

        //显示时间
        //string shiJianChuo = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffGameTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //DateTime DataTime = Game_PublicClassVar.Get_wwwSet.GetTime(shiJianChuo);

        //记录时间LastOffGameTime
        if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus)
        {
            Obj_ShiJianChuo.GetComponent<Text>().text = "上次离线时间:" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Month + "月" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Day + "日" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Hour + "时" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Minute + "分" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Second + "秒";
        }
        else {
            Obj_ShiJianChuo.GetComponent<Text>().text = "未连接网络！";
        }

        //显示版本号
        string payValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_BanBenText.GetComponent<Text>().text = Application.version + " --" + payValue;

        //显示勾选项
        Btn_ShowNanDuImg();
        Btn_ShowYaoImg();
        if (Game_PublicClassVar.Get_game_PositionVar.GameSourceValue == 1)
        {
            Obj_RoseMusicText.SetActive(true);
        }
        else {
            Obj_RoseMusicText.SetActive(false);
        }

        //显示服务器版本
        //显示版本号
        Obj_BanBenText.GetComponent<Text>().text = Application.version;
        if (Game_PublicClassVar.Get_wwwSet.GameServerVersionStr != "")
        {
            //if (Application.version != Game_PublicClassVar.Get_wwwSet.GameServerVersionStr)
            if(GameVersionBiJiao(Application.version, Game_PublicClassVar.Get_wwwSet.GameServerVersionStr)==false)
            {
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("最新版本");
                Obj_ServerGameVersion.GetComponent<Text>().text = langStr + ":" + Game_PublicClassVar.Get_wwwSet.GameServerVersionStr;
            }
            else
            {
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("当前版本为最新版本");
                Obj_ServerGameVersion.GetComponent<Text>().text = langStr;
            }
        }
        else
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("未连接服务器获取最新版本");
            Obj_ServerGameVersion.GetComponent<Text>().text = langStr;
        }

        //显示语言
        Obj_Language_0.SetActive(false);
        Obj_Language_1.SetActive(false);

        //初始化设置语言
        string typeStr = PlayerPrefs.GetString("GameLanguageType");
        if (typeStr == "")
        {
            typeStr = "0";          //初始化0是中文，1是英文
        }

        //设置本地语言类型
        switch (typeStr)
        {
            case "0":
                //设置中文
                Obj_Language_0.SetActive(true);
                break;

            case "1":
                //设置英文
                Obj_Language_1.SetActive(true);
                break;
        }


        //设置本地语言类型
        string GameHuaMianStr = PlayerPrefs.GetString("GameHuaMian");
        if (GameHuaMianStr == "")
        {
            GameHuaMianStr = "0";          //初始化0是中文，1是英文
        }

        Obj_ShuaXin_0.SetActive(false);
        Obj_ShuaXin_1.SetActive(false);
        switch (GameHuaMianStr)
        {
            case "0":
                //设置中文
                Obj_ShuaXin_0.SetActive(true);
                break;

            case "1":
                //设置英文
                Obj_ShuaXin_1.SetActive(true);
                break;
        }

        //默认显示设置
        Btn_SettingShow();

        IfShowYinSiBtn();

        Btn_ShowPetMoShiImg();

        //华为隐藏
        if (EventHandle.IsHuiWeiChannel()) {
            ObjBtn_Up.SetActive(false);
            ObjBtn_Down.SetActive(false);
            ObjBtn_RenZheng.SetActive(false);
        }

    }
	
	// Update is called once per frame
	void Update () {
        //this.GetComponent<>

        if (serverNameStatus == false) {

            //显示区服
            if (Game_PublicClassVar.Get_wwwSet.ServerName != "" && Game_PublicClassVar.Get_wwwSet.ServerName != null)
            {
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("游戏战区");
                Obj_ServerObj.GetComponent<Text>().text = langStr + ":" + Game_PublicClassVar.Get_wwwSet.ServerName;
                serverNameStatus = true;
            }
        }
	}


    //开关音乐
    public void Btn_GameMusic() {
        if (Game_PublicClassVar.Get_game_PositionVar.GameSourceValue == 1)
        {
            Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 0;
            //Obj_RoseMusicText.GetComponent<Text>().text = "开";
			Obj_RoseMusicText.SetActive(false);
            //设置背景音乐音量为空
            if (Application.loadedLevelName == "EnterGame")
            {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingBGM.GetComponent<AudioSource>().volume = 0;
            }
            else {
                Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceSet);
            }

            //存储声音状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SourceSize", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        }
        else {
            Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 1;
            //Obj_RoseMusicText.GetComponent<Text>().text = "关";
			Obj_RoseMusicText.SetActive(true);
            if (Application.loadedLevelName == "EnterGame")
            {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingBGM.GetComponent<AudioSource>().volume = Game_PublicClassVar.Get_game_PositionVar.GameSourceValue;
            }
            else {
                //获取当前场景名称
                string sceneName = Application.loadedLevelName;
                //Debug.Log("sceneName = " + sceneName);
                if (sceneName != "StartGame")
                {
                    string sceneBGM = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneBGM", "ID", sceneName, "Scene_Template");
                    Game_PublicClassVar.Get_function_UI.PlaySource(sceneBGM, "3");
                }
            }
            //存储声音状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SourceSize", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        }
    }


    //开关音乐
    public void Btn_GameMusic_YinXiao()
    {
        if (Game_PublicClassVar.Get_game_PositionVar.GameSourceValue_YinXiao == 1)
        {
            Game_PublicClassVar.Get_game_PositionVar.GameSourceValue_YinXiao = 0;
            //Obj_RoseMusicText.GetComponent<Text>().text = "开";
            Obj_RoseMusicText_YinXiao.SetActive(false);
            //设置背景音乐音量为空
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingBGM.GetComponent<AudioSource>().volume = 0;
            //存储声音状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SourceSize_YinXiao", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        }
        else
        {
            Game_PublicClassVar.Get_game_PositionVar.GameSourceValue_YinXiao = 1;
            //Obj_RoseMusicText.GetComponent<Text>().text = "关";
            Obj_RoseMusicText_YinXiao.SetActive(true);
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingBGM.GetComponent<AudioSource>().volume = Game_PublicClassVar.Get_game_PositionVar.GameSourceValue;
            //存储声音状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SourceSize_YinXiao", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        }
    }

    //单机模式
    public void Btn_GameDanJi()
    {

        if (Game_PublicClassVar.Get_game_PositionVar.GameDanJiStatus == 1)
        {
            Game_PublicClassVar.Get_game_PositionVar.GameDanJiStatus = 0;
            Obj_RoseDanJiImg.SetActive(false);

            //显示UI
            if (Application.loadedLevelName == "EnterGame")
            {
                Game_PublicClassVar.gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().PlayerObjSet.SetActive(true);
                //Game_PublicClassVar.Get_function_UI.HintTargetObj(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet, true);
            }
            //存储状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GameModel", Game_PublicClassVar.Get_game_PositionVar.GameDanJiStatus.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        }
        else
        {
            Game_PublicClassVar.Get_game_PositionVar.GameDanJiStatus = 1;
            Obj_RoseDanJiImg.SetActive(true);

            //隐藏UI
            if (Application.loadedLevelName == "EnterGame") {

                Game_PublicClassVar.gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().PlayerObjSet.SetActive(false);
                Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().PlayerObjSet);
                Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet);

            }

            //存储状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GameModel", Game_PublicClassVar.Get_game_PositionVar.GameDanJiStatus.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        }
    }

    //开关摇杆
    public void Btn_YaoGanStatus()
    {
        //摇杆状态  1：表示开  0:表示关
        string yaoganStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YaoGanStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (yaoganStatus == "1")
        {
            //Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 0;
            //Obj_YaoGanBtnText.GetComponent<Text>().text = "开";
            Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = false;
            //存储状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            //Game_PublicClassVar.Get_function_UI.GameHint("摇杆操作已关闭");
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_372");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        }
        else
        {
            //Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 1;
            //Obj_YaoGanBtnText.GetComponent<Text>().text = "关";
            Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(true);
            Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = true;
            //存储状态
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            //Game_PublicClassVar.Get_function_UI.GameHint("摇杆操作已开启");
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_373");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        }
        Btn_ShowYaoImg();
    }


    //开关摇杆
    public void Btn_YaoGanStatus_0()
    {
        //Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 1;
        //Obj_YaoGanBtnText.GetComponent<Text>().text = "开";
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = false;
        //存储状态
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //Game_PublicClassVar.Get_function_UI.GameHint("点击移动模式开启");
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_374");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        Btn_ShowYaoImg();
        //设置摇杆状态
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.GetComponent<UI_GameYaoGan>().yaoganStatus = "0";
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.GetComponent<UI_GameYaoGan>().SettingYaoGanType();
    }

    //开关摇杆
    public void Btn_YaoGanStatus_1()
    {
        //Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 1;
        //Obj_YaoGanBtnText.GetComponent<Text>().text = "关";
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = true;
        //存储状态
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //Game_PublicClassVar.Get_function_UI.GameHint("摇杆(移动)操作已开启");
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_375");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        Btn_ShowYaoImg();
        //设置摇杆状态
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.GetComponent<UI_GameYaoGan>().yaoganStatus = "1";
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.GetComponent<UI_GameYaoGan>().SettingYaoGanType();
    }

    //开关摇杆
    public void Btn_YaoGanStatus_2()
    {
        //Game_PublicClassVar.Get_game_PositionVar.GameSourceValue = 1;
        //Obj_YaoGanBtnText.GetComponent<Text>().text = "关";
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = true;
        //存储状态
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //Game_PublicClassVar.Get_function_UI.GameHint("摇杆(固定)操作已开启");
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_376");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        Btn_ShowYaoImg();
        //设置摇杆状态
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.GetComponent<UI_GameYaoGan>().yaoganStatus = "2";
        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.GetComponent<UI_GameYaoGan>().SettingYaoGanType();
    }


    //开关摇杆
    public void Btn_PetModelStatus_0()
    {
        Game_PublicClassVar.Get_game_PositionVar.PetActMoShi = 0;
        //存储状态
        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //Game_PublicClassVar.Get_function_UI.GameHint("点击移动模式开启");
        //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_374");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("更改宠物模式为:攻击模式");
        Btn_ShowPetMoShiImg();
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().PetActModel();
    }

    //开关摇杆
    public void Btn_PetModelStatus_1()
    {
        Game_PublicClassVar.Get_game_PositionVar.PetActMoShi = 1;
        //存储状态
        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //Game_PublicClassVar.Get_function_UI.GameHint("摇杆(移动)操作已开启");
        //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_375");
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("更改宠物模式为:跟随模式");
        Btn_ShowPetMoShiImg();
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().PetActModel();
    }

    //开关摇杆
    public void Btn_PetModelStatus_2()
    {
        Game_PublicClassVar.Get_game_PositionVar.PetActMoShi = 2;
        //存储状态
        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //Game_PublicClassVar.Get_function_UI.GameHint("摇杆(固定)操作已开启");
        //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_376");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("更改宠物模式为:守护模式");
        Btn_ShowPetMoShiImg();
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().PetActModel();
    }


    public void Btn_GameName() {

        string roseName = Obj_RoseName.GetComponent<InputField>().text;

        //检测名称是否有特殊符号
        bool ifteshu = Game_PublicClassVar.Get_function_UI.IfTeShuStr(roseName);
        if (ifteshu)
        {
            Debug.Log("存在特殊字符");
            //强制改名
            roseName = "";
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("不能起带有特殊字符和数字的名字呦~");
            return;
        }

        //判定是否存在数字
        string numStr = "1;2;3;4;5;6;7;8;9;0;一;二;三;四;五;六;七;八;九;零";
        string[] numStrList = numStr.Split(';');
        for (int i = 0; i < numStrList.Length; i++)
        {
            if (roseName.Contains(numStrList[i])) {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("不能起带有特殊字符和数字的名字呦~");
                roseName = "";
                return;
            }
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Name",roseName, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        //修改主界面显示的名称
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Obj_RoseName.GetComponent<Text>().text = roseName;
        //Game_PublicClassVar.Get_function_UI.GameHint("你的昵称已修改为：" + roseName);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_377");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + roseName);
        //存储角色通用数据
        Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "Name", roseName);
		//更新主界面
		Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().RoseUpdate_Name();
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().ShowName();

    }

    /*
    //复制账号ID
    public void ZhangHaoID_Copy() {
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_ZhangHaoIDStr.GetComponent<Text>().text = zhanghaoID;

    }
    */

    //切换游戏难度（1.普通  2.挑战 3.地狱）
    public void ChangGameNanDu(string nanduType)
    {

        //更改难度只能在城镇更改
        if (Application.loadedLevelName != "EnterGame")
        {
            //Game_PublicClassVar.Get_function_UI.GameHint("请进入城镇使用更改游戏难度");
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_378");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            return;
        }

        switch (nanduType)
        {
            //切换普通
            case "1":
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_4", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_311");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint("切换普通模式成功!");
                Game_PublicClassVar.Get_game_PositionVar.GameNanduValue = "1";
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                break;
            //切换挑战
            case "2":
                if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= 10)
                {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_4", "2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_312");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint("切换挑战模式成功！Boss属性增强,更有大概率掉落道具！");
                    Game_PublicClassVar.Get_game_PositionVar.GameNanduValue = "2";
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                }
                else
                {
                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_313");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint("等级提升至10级后开启挑战模式！");
                }

                break;
            //切换地狱
            case "3":
                if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= 15)
                {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_4", "3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_314");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint("切换地狱模式成功！Boss属性大大增强,更有超大概率掉落道具！");
                    Game_PublicClassVar.Get_game_PositionVar.GameNanduValue = "3";
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                }
                else
                {
                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_315");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint("等级提升至15级后开启地狱模式！");
                }
                break;
        }

        //更新右上角模式图标显示
        Game_PublicClassVar.Get_function_UI.ShowMainUINanDuImg();

        Btn_ShowNanDuImg();
    }


    //显示账号ID
    public void ZhangHaoID_Show()
    {
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_ZhangHaoIDStr.GetComponent<Text>().text = zhanghaoID;
    }

    public void Btn_CloseUI() {
        Destroy(this.gameObject);
    }

    //关闭游戏
    public void Btn_CloseGame() {
        Application.Quit();
    }

    //展示摇杆
    public void Btn_ShowYaoImg() {

        Obj_YaoGanImg_1.SetActive(false);
        Obj_YaoGanImg_2.SetActive(false);
        Obj_YaoGanImg_3.SetActive(false);

        string yaoganStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YaoGanStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        switch (yaoganStatus)
        {
            //移动状态
            case "0":
                Obj_YaoGanImg_1.SetActive(true);
            break;

            case "1":
                Obj_YaoGanImg_2.SetActive(true);
            break;

            case "2":
                Obj_YaoGanImg_3.SetActive(true);
            break;
        }
    }

    //展示摇杆
    public void Btn_ShowPetMoShiImg()
    {

        Obj_PetMoShiImg_1.SetActive(false);
        Obj_PetMoShiImg_2.SetActive(false);
        Obj_PetMoShiImg_3.SetActive(false);

        //string yaoganStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YaoGanStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        switch (Game_PublicClassVar.Get_game_PositionVar.PetActMoShi)
        {
            //移动状态
            case 0:
                Obj_PetMoShiImg_1.SetActive(true);
                break;

            case 1:
                Obj_PetMoShiImg_2.SetActive(true);
                break;

            case 2:
                Obj_PetMoShiImg_3.SetActive(true);
                break;
        }

    }

    public void Btn_ShowNanDuImg() {

        Obj_NanDuImg_1.SetActive(false);
        Obj_NanDuImg_2.SetActive(false);
        Obj_NanDuImg_3.SetActive(false);

        string nandu = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_4", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        switch (nandu)
        {
			//难度1
			case "0":
				Obj_NanDuImg_1.SetActive(true);
			break;

            //难度1
            case "1":
                Obj_NanDuImg_1.SetActive(true);
                break;

            //难度2
            case "2":
                Obj_NanDuImg_2.SetActive(true);
                break;

            //难度3
            case "3":
                Obj_NanDuImg_3.SetActive(true);
                break;
        }
    }

    public void Btn_UseChengHao() {

        //测试
        //Game_PublicClassVar.Get_function_Rose.ChengHao_Use("100001");

        GameObject HeChengXuanZeObj = (GameObject)Instantiate(Obj_ChengHaoXuanZe);
        HeChengXuanZeObj.transform.SetParent(this.gameObject.transform.parent.transform.parent.transform);
        HeChengXuanZeObj.transform.localPosition = new Vector3(0, 0, 0);
        HeChengXuanZeObj.transform.localScale = new Vector3(1, 1, 1);
        HeChengXuanZeObj.GetComponent<UI_ChengHaoXuanZe>().FuJiObj = this.gameObject;

    }


	//上传存档
	public void Btn_UpdatePlay() {
        ClearnShow();
        Obj_UpdatePlay.SetActive(true);
	}

	//下载存档
	public void Btn_SavePlay()
	{
        ClearnShow();
        Obj_SavePlay.SetActive(true);

	}

    //设置显示
    public void Btn_SettingShow()
    {
        ClearnShow();
        Obj_SettingSet.SetActive(true);

    }

    //清空显示
    private void ClearnShow() {
        Obj_UpdatePlay.SetActive(false);
        Obj_SavePlay.SetActive(false);
        Obj_SettingSet.SetActive(false);
        Obj_ShenFenXinXiSet.SetActive(false);
        Obj_GuaJiSet.SetActive(false);
    }



    //返回角色
    public void Btn_ReturnRose()
    {
        Game_PublicClassVar.ReturnDengLuStatus = true;
        for (int i = 0; i < Game_PublicClassVar.Get_game_PositionVar.Obj_Keep.Length; i++)
        {
            Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_Keep[i]);
        }
        Destroy(Game_PublicClassVar.Get_wwwSet.gameObject.GetComponent<WWWSet>());
        Destroy(Game_PublicClassVar.Get_wwwSet.gameObject);
        SceneManager.LoadScene("StartGame");        //加载场景
    }

    //打开测试帧率
    public void ShowTestXinXi() {

        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_TestXinXi.SetActive(true);
    }


    //返回角色
    public void Btn_UpdateGame()
    {

        string hintText = "";
        //|| GameVersionBiJiao(Application.version, Game_PublicClassVar.Get_wwwSet.GameServerVersionStr)
        if (Game_PublicClassVar.Get_wwwSet.GameServerVersionStr == Application.version)
        {
            hintText = "当前版本为最新版本!";
        }
        else {
            //"当前游戏版本:" + Application.version + "\n最新游戏版本:" + Game_PublicClassVar.Get_wwwSet.GameServerVersionStr + 
            hintText = "是否更新到当前最新版本?\n提示: 点击更新后会弹出浏览器点击下载最新版本游戏,下载完成后自行覆盖安装游戏即可。";
        }
        
        //弹出提示框
        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(hintText, UpdateGame, null,"游戏更新","更新游戏");
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
        uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 26;
        uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

    }

    //实名认证
    public void Btn_ShiMing() {

        //初始化认证自身是否绑定身份证
        string name = PlayerPrefs.GetString("FangChenMi_Name");
        string shenfenID = PlayerPrefs.GetString("FangChenMi_ID");

        if (name != "" && name != null && shenfenID != "" && shenfenID != null)
        {
            //不需要进行防沉迷验证,已经验证成功
            ClearnShow();
            Obj_ShenFenXinXiSet.SetActive(true);
        }
        else
        {

            if (RenZhengObj != null) {
                Destroy(RenZhengObj);
            }

            RenZhengObj = (GameObject)Instantiate(Obj_RenZhengObj);
            RenZhengObj.transform.SetParent(this.transform.parent);
            RenZhengObj.transform.localScale = new Vector3(1, 1, 1);
            RenZhengObj.transform.localPosition = new Vector3(0, 0, 0);
        }

    }

    //打开挂机
    public void Btn_GuaJi()
    {
        ClearnShow();
        Obj_GuaJiSet.SetActive(true);
    }


    //版本比对（1的版本比2大,返回True,否则返回False）
    public bool GameVersionBiJiao(string banBenStr_1, string banBenStr_2)
    {

        string[] verList_1 = banBenStr_1.Split('.');
        string[] verList_2 = banBenStr_2.Split('.');

        if (verList_1.Length < 3 || verList_2.Length < 3)
        {
            return false;
        }

        //比较第一个版本
        if (int.Parse(verList_1[0]) >= int.Parse(verList_2[0]))
        {

            if (int.Parse(verList_1[0]) > int.Parse(verList_2[0]))
            {
                return true;
            }

            //比较第二个版本
            if (int.Parse(verList_1[1]) >= int.Parse(verList_2[1]))
            {
                if (int.Parse(verList_1[1]) > int.Parse(verList_2[1]))
                {
                    return true;
                }

                //比较第二个版本
                if (int.Parse(verList_1[2]) >= int.Parse(verList_2[2]))
                {
                    return true;
                }
            }
        }

        return false;
    }


    //更新游戏
    public void UpdateGame() {

        try
        {

            if (GameVersionBiJiao(Application.version, Game_PublicClassVar.Get_wwwSet.GameServerVersionStr))
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前版本为最新版本,无需更新!");
                return;
            }

#if UNITY_ANDROID
                string upGameStr = "https://l.taptap.com/T28gNaHb";
            if (Game_PublicClassVar.gameLinkServer.UpdateGame_Android != "" && Game_PublicClassVar.gameLinkServer.UpdateGame_Ios != null && Game_PublicClassVar.gameLinkServer.UpdateGame_Ios != null)
            {
                upGameStr = Game_PublicClassVar.gameLinkServer.UpdateGame_Android;
            }
            Application.OpenURL(upGameStr);
#endif
#if UNITY_IPHONE
            string upGameStr = "https://itunes.apple.com/cn/app/id1510177862";
            if (Game_PublicClassVar.gameLinkServer.UpdateGame_Android != "" && Game_PublicClassVar.gameLinkServer.UpdateGame_Ios != null && Game_PublicClassVar.gameLinkServer.UpdateGame_Ios != null) {
                upGameStr = Game_PublicClassVar.gameLinkServer.UpdateGame_Android;
            }
            Application.OpenURL(upGameStr);
#endif
        }
        catch
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请到TapTap下载更新游戏,感谢！");
        }

    }

    //设置语言   0:表示中文  1表示英文
    public void SetLanguage(string langType)
    {

        string typeStr = PlayerPrefs.GetString("GameLanguageType");
        if (typeStr == "") {
            typeStr = "0";
        }

        Obj_Language_0.SetActive(false);
        Obj_Language_1.SetActive(false);

        switch (langType) {
            //切换中文
            case "0":
                Game_PublicClassVar.Get_gameSettingLanguge.SetLanguage("Chinese");
                PlayerPrefs.SetString("GameLanguageType","0");
                Obj_Language_0.SetActive(true);
                break;

            //切换英文
            case "1":
                Game_PublicClassVar.gameSettingLanguge.SetLanguage("English");
                PlayerPrefs.SetString("GameLanguageType", "1");
                Obj_Language_1.SetActive(true);
                break;
        }

        IfShowYinSiBtn();

        //发送当前语言
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001021, "");
    }

    public void IfShowYinSiBtn() {

        if (PlayerPrefs.GetString("GameLanguageType") == "0")
        {
            Obj_BtnYinSi.SetActive(false);
        }
        else {
            Obj_BtnYinSi.SetActive(true);
        }
    }

    public void GameHuaMian(string langType)
    {

        Obj_ShuaXin_0.SetActive(false);
        Obj_ShuaXin_1.SetActive(false);

        switch (langType)
        {
            //切换中文
            case "0":
                Application.targetFrameRate = 30;
                PlayerPrefs.SetString("GameHuaMian", "0");
                PlayerPrefs.Save();
                Obj_ShuaXin_0.SetActive(true);
                break;

            //切换英文
            case "1":
                Application.targetFrameRate = 60;
                PlayerPrefs.SetString("GameHuaMian", "1");
                PlayerPrefs.Save();
                Obj_ShuaXin_1.SetActive(true);
                break;
        }
    }

    //点击挂机按钮
    public void Btn_OpenGuaJi() {


        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();

        //判断等级
        if (roseStatus.GetComponent<Rose_Proprety>().Rose_Lv < 20) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("等级达到20后才可以进行挂机!");
            return;
        }

        if (roseStatus.AutomaticGuaJiStatus == false)
        {
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("开启挂机模式");
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().GuaJiBtn();
            Obj_GuaJiSet.GetComponent<UI_GuaJiSetting>().Obj_GuaJiNumShowText.GetComponent<Text>().text = "取消挂机";
        }
        else {
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("结束挂机模式");
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().GuaJiBtn();
            Obj_GuaJiSet.GetComponent<UI_GuaJiSetting>().Obj_GuaJiNumShowText.GetComponent<Text>().text = "开始挂机";
        }

    }


    //打开隐私
    public void Btn_OpenYinSi() {
        Application.OpenURL("https://sgcssg.lofter.com/post/31e753e0_1c9040b5b");
    }

    public void Btn_CopyID() {
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        GUIUtility.systemCopyBuffer = zhanghaoID;
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("账号ID已复制在粘贴板!");
    }


    //string createIDListStr = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("CreateIDList", "ID", "1", createXmlPath + "GameCreate.xml");

}
