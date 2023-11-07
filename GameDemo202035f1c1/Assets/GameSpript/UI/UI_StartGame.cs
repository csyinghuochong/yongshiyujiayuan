using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_StartGame : MonoBehaviour {
    public bool DestroyKeepObj;                     //当次开关打开是注销Keep保存的Obj,此处用在战斗界面切换建筑界面中
    public GameObject EnterGameObj;
    public GameObject Obj_ReturnBuilding;
    public GameObject Obj_StartBGM;                 //开始界面的BGM      //只在初始界面配置播放即可
    public bool EnterGameStatus;                    //进入游戏状态
    public GameObject Obj_InputName;                //输入名字

    public GameObject Obj_CreateRose;
	// Use this for initialization
	void Start () {
        if (Obj_StartBGM != null) {
            float sourceSize = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SourceSize", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig"));
            Obj_StartBGM.GetComponent<AudioSource>().volume = sourceSize;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (!Game_PublicClassVar.Get_wwwSet.DataUpdataStatusOnce)
        {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
            {
                this.transform.parent.transform.gameObject.SetActive(false);        //隐藏创建角色界面
                Game_PublicClassVar.Get_wwwSet.DataUpdataStatusOnce = true;
                //Debug.Log("创建角色进入场景!");
                try
                {
                    Btn_EnterGame();
                }
                catch(Exception ex) {
                    Debug.LogError("报错！！！进入场景" + ex);
                }
            }
        }
	}

    public void Btn_EnterGame() {

        //Application.LoadLevel("EnterGame");

        //写入场景
        //ScenceID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", SceneTransferID, "SceneTransfer_Template");
        //获取当前血量
        if (Application.loadedLevelName != "StartGame")
        {
            
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseHpNow", Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        }

        //Debug.Log("存储！！！！！！！");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", "EnterGame", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        //GameObject.Find("DoorWay").gameObject.GetComponent<DoorWay>().DoorWayID = "1";
        //GameObject.Find("DoorWay").gameObject.GetComponent<DoorWay>().EnterGame();
        //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = SceneTransferID;
        EnterGameObj.GetComponent<UI_EnterGame>().roseObjUpdataStatus = true;
        EnterGameObj.GetComponent<UI_EnterGame>().EnterGame();
        
    }

    public void Btn_RetuenBuilding() {

        //存储自身Buff
        string saveBuffStr = "";
        Buff_4[] buff4List = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponents<Buff_4>();
        
        if (buff4List.Length != 0)
        {
            for (int i = 0; i < buff4List.Length; i++)
            {
                string buffID = buff4List[i].BuffID;
                float buffTime = buff4List[i].buffTimeSum;
                saveBuffStr = saveBuffStr + buffID + "," + buffTime + ";";
            }
            if (saveBuffStr != "")
            {
                saveBuffStr = saveBuffStr.Substring(0, saveBuffStr.Length - 1);
                Game_PublicClassVar.Get_wwwSet.RoseBuffStr = saveBuffStr;
            }
        }
        
        /*
        for (int i = 0; i < buff4List.Length; i++) {
            if (buff4List[i] != null)
            {
                Destroy(buff4List[i]);
            }
        }
        */

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", "EnterGame", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        string writeHp = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow.ToString();
        if (writeHp != "" && writeHp != "0" && writeHp != null) {
            if (int.Parse(writeHp) > 0) {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseHpNow", writeHp, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            }
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        EnterGameObj.GetComponent<UI_EnterGame>().roseObjUpdataStatus = true;
        //EnterGameObj.GetComponent<UI_EnterGame>().EnterBuildGame();
        //EnterGameObj.GetComponent<UI_EnterGame>().KeepObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Keep;
        EnterGameObj.GetComponent<UI_EnterGame>().EnterBuildGame();
        //Application.LoadLevel("EnterGame");
        //DestroyKeepObj = true;
    }

    public void Btn_ReturnBuildingUI() {
        GameObject obj = (GameObject)Instantiate(Obj_ReturnBuilding);
        obj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        obj.GetComponent<UI_ReturnBuilding>().ReturnBuildingObj = this.gameObject;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(1, 1, 1);
    }

    void LateUpdate()
    {
        if (DestroyKeepObj) {
            DestroyKeepObj = false;
            //保证切换场景以下预设体不消失
            for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.Obj_Keep.Length; i++)
            {
                Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_Keep[i - 1]);
            }
        }
    }

    //创建角色
    public void Btn_CreateRose()
    {
        
        //查询网络是否连接
        if (Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus == false) {
            //Debug.Log("网络未连接...");
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_104");
            Game_PublicClassVar.Get_wwwSet.Show_GameHint(langStr);
            return;
        }
        
        if (Obj_CreateRose.GetComponent<UI_CreateRose>().sclectOcctype == "0")
        {
            return;
        }

        if (Obj_InputName != null) {
            Obj_InputName.SetActive(false);
        }

        GameObject createRoseObj = Obj_CreateRose;
        string createName = Obj_CreateRose.GetComponent<UI_CreateRose>().UI_InputName.GetComponent<InputField>().text;
        createRoseObj.GetComponent<UI_CreateRose>().Obj_DeleteRoseBtn.SetActive(false);
        createRoseObj.GetComponent<UI_CreateRose>().Obj_FindRoseBtn.SetActive(false);

        createName = createName.Replace("*", "");
        //判断角色名称是否符合要求（不能有特殊符号）
        bool ifteshu = Game_PublicClassVar.Get_function_UI.IfTeShuStr(createName);
        if (ifteshu)
        {
            Debug.Log("存在特殊字符");
            //强制改名
            createName = "";
        }

        //修改角色名称
        string roseID = "10001";
        switch (createRoseObj.GetComponent<UI_CreateRose>().sclectOcctype)
        {
            case "1":
                roseID = "10001";
                break;

            case "2":
                roseID = "10002";
                break;
                
            case "3":
                roseID = "10003";
                break;
        }

        //设置存储文件
        string createXmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/";

        //为空表示创建角色
        if (Game_PublicClassVar.Get_wwwSet.NowSelectFileName == "")
        {
            string createIDListStr = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("CreateIDList", "ID", "1", createXmlPath + "GameCreate.xml");
            string[] createIDList = createIDListStr.Split(';');
            if (createIDList[0] == "")
            {
                //创建第一个账号
                Game_PublicClassVar.Get_wwwSet.NowSelectFileName = "10001";
                Game_PublicClassVar.Get_xmlScript.Xml_SetDate("CreateIDList", Game_PublicClassVar.Get_wwwSet.NowSelectFileName + "," + Game_PublicClassVar.Get_wwwSet.RoseID, "ID", "1", createXmlPath + "GameCreate.xml");
                Game_PublicClassVar.Get_xmlScript.Xml_SetDate("CreateNowID", Game_PublicClassVar.Get_wwwSet.NowSelectFileName, "ID", "1", createXmlPath + "GameCreate.xml");
            }
            else
            {
                //创建其他账号
                string[] createRoseIDList = createIDList[createIDList.Length - 1].Split(',');
                int saveFileName = int.Parse(createRoseIDList[0]);
                saveFileName = saveFileName + 1;
                Game_PublicClassVar.Get_wwwSet.NowSelectFileName = saveFileName.ToString();

                Game_PublicClassVar.Get_xmlScript.Xml_SetDate("CreateIDList", createIDListStr + ";" + saveFileName.ToString() + "," + Game_PublicClassVar.Get_wwwSet.RoseID, "ID", "1", createXmlPath + "GameCreate.xml");
                Game_PublicClassVar.Get_xmlScript.Xml_SetDate("CreateNowID", Game_PublicClassVar.Get_wwwSet.NowSelectFileName, "ID", "1", createXmlPath + "GameCreate.xml");
            }

            //存储文件
            this.StartCoroutine(Game_PublicClassVar.Get_wwwSet.Set_GameConfig());
            
            //第一次创建角色开启创建角色状态
            Game_PublicClassVar.Get_wwwSet.CreateRoseStatus = true;

            //默认初始进入游戏名称
            switch (createRoseObj.GetComponent<UI_CreateRose>().sclectOcctype)
            {
                case "1":
                    Game_PublicClassVar.Get_wwwSet.CreateRoseOcc = "1";
                    break;

                case "2":
                    Game_PublicClassVar.Get_wwwSet.CreateRoseOcc = "2";
                    break;

                case "3":
                    Game_PublicClassVar.Get_wwwSet.CreateRoseOcc = "3";
                    break;
            }

            //更新创建角色的名称
            if (createName == "")
            {
                string set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName + "/";
                createName = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Name", "ID", roseID, set_XmlPath + "GameConfig.xml");
                Debug.Log("createName = " + createName);
                //默认初始进入游戏名称
                switch (createRoseObj.GetComponent<UI_CreateRose>().sclectOcctype)
                {
                    case "1":
                        createName = "小小战士";
                        break;

                    case "2":
                        createName = "小小法师";
                        break;

                    case "3":
                        createName = "小小猎手";
                        break;
                }

                if (PlayerPrefs.GetString("GameLanguageType") == "1")
                {
                    createName = "Little adventurer";
                }

                Game_PublicClassVar.Get_xmlScript.Xml_SetDate("Name", createName,"ID", "10001", set_XmlPath + "GameConfig.xml");
            }
        }
        else
        {
            //存储当前选中的角色文件夹
            Game_PublicClassVar.Get_xmlScript.Xml_SetDate("CreateNowID", Game_PublicClassVar.Get_wwwSet.NowSelectFileName, "ID", "1", createXmlPath + "GameCreate.xml");
        }
        Game_PublicClassVar.Get_wwwSet.CreateRoseNameStr = createName;
        Game_PublicClassVar.Get_wwwSet.IfChangeOcc = true;
        this.gameObject.transform.position = new Vector3(10000, 0, 0);  //移除屏幕外  UI_InputName

        createRoseObj.GetComponent<UI_CreateRose>().UI_SetObj_SclectCreateName.SetActive(false);


    }

}