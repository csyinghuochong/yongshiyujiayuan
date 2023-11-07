using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class UI_FindRoseList : MonoBehaviour {

    public GameObject Obj_Name;
    public GameObject Obj_Lv;
    public GameObject Obj_Occ;
    public GameObject Obj_ServerName;

    public ObscuredString Name;
    public ObscuredString Lv;
    public ObscuredString Occ;
    public ObscuredString ServerName;
    public ObscuredString ZhangHaoID;
    public ObscuredString Password;

    public ObscuredString ShenFenName;
    public ObscuredString ShenFenID;


    // Use this for initialization
    void Start () {
		
	}

    //初始化
    public void Init() {

        Obj_Name.GetComponent<Text>().text = Name;
        Obj_Lv.GetComponent<Text>().text = Lv;
        Obj_ServerName.GetComponent<Text>().text = ServerName;

        switch (Occ) {
            case "1":
                Obj_Occ.GetComponent<Text>().text = "战士";
                break;
            case "2":
                Obj_Occ.GetComponent<Text>().text = "魔法师";
                break;
            case "3":
                Obj_Occ.GetComponent<Text>().text = "狩猎者";
                break;

        }


    }

    // Update is called once per frame
    void Update () {
		
	}

    //找回角色
    public void Btn_FindRose() {

        object obj = Resources.Load("UGUI/UISet/Common/UI_CommonHint", typeof(GameObject));
        GameObject nowObj = obj as GameObject;
        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(nowObj);      //Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint
        //string jieshaoStr = "1.玩家需要在限定的时间内击杀小怪收集秘境值,当秘境值达到500点后即可召唤秘境领主BOSS.\n2.击杀秘境BOSS后则挑战当前秘境层级成功,并激活下一层级大秘境,怪物实力随着秘境层级越高！\n3.大秘境内均会掉落秘境碎片可以在隔壁的同学处兑换奖励！\n4.大秘境成功退出地图后可以连续挑战,如果挑战失败只能等明日再来！";
        string jieshaoStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_20");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, FindRose, null, "找回须知", "我已阅读", "取 消");
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_gameServerObj.Obj_FindRose.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    //找回角色
    public void FindRose()
    {

        //获取当前列表内是否还可以创建角色
        //显示角色列表
        string createXmlPath = Application.persistentDataPath + "/GameData/" + "Xml" + "/Set_Xml/";
        string createIDListStr = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("CreateIDList", "ID", "1", createXmlPath + "GameCreate.xml");
        //Debug.Log("createIDListStr = " + createIDListStr);
        //显示当前选择
        //Game_PublicClassVar.Get_wwwSet.NowSelectFileName = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("CreateNowID", "ID", "1", createXmlPath + "GameCreate.xml");

        string[] createIDList = createIDListStr.Split(';');
        if (createIDList.Length >= 5)
        {
            Debug.Log("当前拥有的角色超过5个,无法创建新角色");
            Game_PublicClassVar.gameLinkServer.HintMsgStatus_StartGame = true;
            Game_PublicClassVar.gameLinkServer.HintMsgText_StartGame = "当前拥有的角色超过5个,无法创建新角色";
            return;
        }

        if (Game_PublicClassVar.Get_wwwSet.StartFindRoseStatus)
        {
            Game_PublicClassVar.gameLinkServer.HintMsgStatus_StartGame = true;
            Game_PublicClassVar.gameLinkServer.HintMsgText_StartGame = "请不要重复点击下载数据！\n(提示:新区角色7天内不支持下载,角色30天内只能开放1次下载!如有特殊需求,请联系群管理解决!)";
            if (Game_PublicClassVar.Get_wwwSet.updataNumSum >= 1) {
                Game_PublicClassVar.gameLinkServer.HintMsgText_StartGame = "当前正在下载数据+" + Game_PublicClassVar.Get_wwwSet.updataNumSum + "/" + Game_PublicClassVar.Get_wwwSet.updataNum + "...等下载完成后游戏会自动闪退,再重新登录即可恢复角色!";
            }
            
            return;
        }


        //获取当前列表是否有相同的角色
        if (createIDList[0] != "")
        {
            //循环显示
            for (int i = 0; i < createIDList.Length; i++)
            {
                string[] createRoseIDList = createIDList[i].Split(',');
                string saveFileName = createRoseIDList[0];
                string saveFileRoseID = createRoseIDList[1];


                string set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + saveFileName + "/";
                //Debug.Log("set_XmlPath = " + set_XmlPath + ";roseID = " + roseID);
                string roseName = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Name", "ID", saveFileRoseID, set_XmlPath + "GameConfig.xml");

                if (roseName == Name)
                {
                    Debug.Log("当前列表内存在需要下载的角色");
                    Game_PublicClassVar.gameLinkServer.HintMsgStatus_StartGame = true;
                    Game_PublicClassVar.gameLinkServer.HintMsgText_StartGame = "当前角色列表内存在需要下载的角色,无法重复下载";
                    return;
                }
            }
        }


        //申请当前角色是否可以创建
        Game_PublicClassVar.Get_wwwSet.FindRoseZhangHaoID = ZhangHaoID;
        Game_PublicClassVar.Get_wwwSet.FindRosePassword = Password;
        Pro_FindRoseListDataDown ProFindRoseListDataDown = new Pro_FindRoseListDataDown();
        ProFindRoseListDataDown.ZhangHaoID = ZhangHaoID;
        ProFindRoseListDataDown.SheBeiID = SystemInfo.deviceUniqueIdentifier;
        ProFindRoseListDataDown.Name = ShenFenName;
        ProFindRoseListDataDown.ShenFenID = ShenFenID;
        ProFindRoseListDataDown.GameVsion = Application.version;

        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010072, ProFindRoseListDataDown);
        Game_PublicClassVar.Get_wwwSet.StartFindRoseStatus = true;
    }
}
