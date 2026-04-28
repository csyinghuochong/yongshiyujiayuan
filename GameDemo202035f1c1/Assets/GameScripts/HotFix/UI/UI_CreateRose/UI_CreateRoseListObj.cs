using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CreateRoseListObj : MonoBehaviour {

    public string RoseFileName;
    public string roseID;
    public bool SelectStatus;
    public string roseOcc;
    public GameObject Obj_CreatePar;
    public GameObject Obj_CreateRoseHint;
    public GameObject Obj_CreateRoseSet;
    public GameObject Obj_CreateRoseName;
    public GameObject Obj_CreateRoseLv;
    public GameObject Obj_CreateRoseOcc;
    public GameObject Obj_CreateRoseServerName;
    public GameObject Obj_SelectImg;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (SelectStatus)
        {
            Obj_SelectImg.SetActive(true);
        }
        else {
            Obj_SelectImg.SetActive(false);
        }
	}

    public void updateShow() {

        if (RoseFileName == "" || RoseFileName == "0")
        {
            Obj_CreateRoseHint.SetActive(true);
            Obj_CreateRoseSet.SetActive(false);
        }
        else {

            string set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + RoseFileName + "/";
            //Debug.Log("set_XmlPath = " + set_XmlPath + ";roseID = " + roseID);
            string roseName = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Name", "ID", roseID, set_XmlPath + "GameConfig.xml");
            //Debug.Log("roseName = " + roseName);
            string roseLv = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Lv", "ID", roseID, set_XmlPath + "GameConfig.xml");
            //Debug.Log("roseLv = " + roseLv);
            string roseOcc = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Occ", "ID", roseID, set_XmlPath + "GameConfig.xml");

            switch (roseOcc) { 
                case "1":

                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("战士");
                    roseOcc = langStr;
                    break;

                case "2":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("魔法师");
                    roseOcc = langStr;
                    break;

                case "3":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("狩猎者");
                    roseOcc = langStr;
                    break;

                default:

                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("战士");
                    roseOcc = langStr;
                    break;
            }

            //显示
            Obj_CreateRoseName.GetComponent<Text>().text = roseName;
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级");
            Obj_CreateRoseLv.GetComponent<Text>().text = langStr_1 +" " + roseLv;
            Obj_CreateRoseOcc.GetComponent<Text>().text = roseOcc;
            Obj_CreateRoseHint.SetActive(false);

            //显示区服
            try
            {
                string serverName = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("ServerName", "ID", roseID, set_XmlPath + "GameConfig.xml");
                if (serverName != "" && serverName != "0" && serverName != null)
                {
                    Obj_CreateRoseServerName.GetComponent<Text>().text = serverName;
                }
                else
                {
                    Obj_CreateRoseServerName.GetComponent<Text>().text = "进入游戏更新";
                }
            }
            catch {
                Obj_CreateRoseServerName.GetComponent<Text>().text = "无";
            }

        }
    }

    public void Btn_Select() {

        if (Obj_CreatePar.GetComponent<UI_CreateRose>().MoveStatus) {
            //Debug.Log("角色正在移动不触发选择..");
            return;
        }

        //正在进入游戏不能触发
        if (Game_PublicClassVar.Get_wwwSet.IfChangeOccStatus) {
            //Debug.Log("角色正在加载无法切换角色..");
            return;
        }

        //Debug.Log("RoseFileName = " + RoseFileName + ";roseID = " + roseID);
        //显示当前选择
        if (RoseFileName != "")
        {
            Game_PublicClassVar.Get_wwwSet.NowSelectFileName = RoseFileName;
            Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_ZhanShi.SetActive(true);
            Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_MoFaShi.SetActive(true);
            Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_LieRen.SetActive(true);
            Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_DeleteRoseBtn.SetActive(true);
            //切换界面角色显示
            //用于播放位置来回跑动
            /*
            switch (roseID) { 
                case "10001":
                    Obj_CreatePar.GetComponent<UI_CreateRose>().CreateRose("1");
                    //隐藏模型
                    Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_MoFaShi.SetActive(false);
                    break;
                case "10002":
                    Obj_CreatePar.GetComponent<UI_CreateRose>().CreateRose("2");
                    //隐藏模型
                    Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_ZhanShi.SetActive(false);
                    break;
            }
            */
            string set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + RoseFileName + "/";
            string roseOcc = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Occ", "ID", roseID, set_XmlPath + "GameConfig.xml");
            switch (roseOcc)
            {
                case "1":
                    Obj_CreatePar.GetComponent<UI_CreateRose>().CreateRose("1");
                    //隐藏模型
                    Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_MoFaShi.SetActive(false);
                    Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_LieRen.SetActive(false);
                    break;

                case "2":
                    Obj_CreatePar.GetComponent<UI_CreateRose>().CreateRose("2");
                    //隐藏模型
                    Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_ZhanShi.SetActive(false);
                    Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_LieRen.SetActive(false);
                    break;

                case "3":
                    Obj_CreatePar.GetComponent<UI_CreateRose>().CreateRose("3");
                    //隐藏模型
                    Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_ZhanShi.SetActive(false);
                    Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_MoFaShi.SetActive(false);
                    break;

                default:
                    Obj_CreatePar.GetComponent<UI_CreateRose>().CreateRose("1");
                    //隐藏模型
                    Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_MoFaShi.SetActive(false);
                    Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_LieRen.SetActive(false);
                    break;
            }

            //显示中间玩家名称
            Game_PublicClassVar.Get_wwwSet.RoseID = roseID;
            //string set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + RoseFileName + "/";
            string roseName = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Name", "ID", roseID, set_XmlPath + "GameConfig.xml");
            string roseLv = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Lv", "ID", roseID, set_XmlPath + "GameConfig.xml");
            //Debug.Log("名字：" + "Lv." + roseLv + "  " + roseName);
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级");
            Obj_CreatePar.GetComponent<UI_CreateRose>().UI_NameObj.GetComponent<Text>().text = langStr_1 + " " + roseLv + "  " + roseName;
            //Obj_CreatePar.GetComponent<UI_CreateRose>().MoveStatus = true;
            //选中角色隐藏后面两个的UI显示
            Obj_CreatePar.GetComponent<UI_CreateRose>().UI_SetObj_MoFaShi.SetActive(false);
            Obj_CreatePar.GetComponent<UI_CreateRose>().UI_SetObj_ZhanShi.SetActive(false);
            Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_ShowText_LieRen_1.SetActive(false);
            Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_ShowText_LieRen_2.SetActive(false);
            Obj_CreatePar.GetComponent<UI_CreateRose>().UI_SetObj_SclectShowName.SetActive(false);

            Obj_CreatePar.GetComponent<UI_CreateRose>().UI_SetObj_SclectCreateName.SetActive(false);
            //Obj_CreatePar.GetComponent<UI_CreateRose>().UI_SetObj_SclectShowName.SetActive(true);

        }
        else {

            if (Obj_CreatePar.GetComponent<UI_CreateRose>().sclectOcctype != "1") {

                //默认创建一个战士
                Obj_CreatePar.GetComponent<UI_CreateRose>().CreateRose("1");
                Obj_CreatePar.GetComponent<UI_CreateRose>().MoveStatus = true;

            }

            Game_PublicClassVar.Get_wwwSet.NowSelectFileName = "";

            //显示模型
            Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_MoFaShi.SetActive(true);
            Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_ZhanShi.SetActive(true);
            Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_LieRen.SetActive(true);
            Obj_CreatePar.GetComponent<UI_CreateRose>().updateNameShow();

            Obj_CreatePar.GetComponent<UI_CreateRose>().Obj_DeleteRoseBtn.SetActive(false);

            Obj_CreatePar.GetComponent<UI_CreateRose>().UI_SetObj_SclectCreateName.SetActive(true);
            Obj_CreatePar.GetComponent<UI_CreateRose>().UI_SetObj_SclectShowName.SetActive(false);

            //显示进入服务器名称
            Obj_CreatePar.GetComponent<UI_CreateRose>().ShowCreateEnterServerName();
            //随机名称
            Obj_CreatePar.GetComponent<UI_CreateRose>().Btn_RandomName();
            //申请服务器名字显示
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000121, "");
        }

        //隐藏其他列表的状态
        //if (updateOtherList) {
            Obj_CreatePar.GetComponent<UI_CreateRose>().UpdateSelect(this.gameObject);
        //}

    }
}
