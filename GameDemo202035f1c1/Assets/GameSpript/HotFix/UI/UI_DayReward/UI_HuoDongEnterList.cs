using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_HuoDongEnterList : MonoBehaviour
{

    public string ScenceID;
    public GameObject Obj_SceneName;
    //public GameObject Obj_SceneDropShow;
    public GameObject OBJ_SceneEnter;
    private string[] sceneDropItemList;
    public GameObject Obj_DropItemShowSet;
    public GameObject Obj_DropItemObj;
    public GameObject Obj_DropHintText;
    private int enterSceneLv;
    private int enterCountryLv;
    public GameObject Obj_FuBen;
    //private bool ifJing

    // Use this for initialization
    void Start()
    {
        //场景名称
        /*
        Obj_SceneName.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", ScenceID, "Scene_Template");
        enterSceneLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseLv", "ID", ScenceID, "Scene_Template"));
        enterCountryLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", ScenceID, "Scene_Template"));
        if (enterCountryLv == 0)
        {
            OBJ_SceneEnter.GetComponent<Text>().text = "角色等级：" + enterSceneLv + "级";
        }
        else {
            OBJ_SceneEnter.GetComponent<Text>().text = "繁荣等级：" + enterSceneLv + "级";
        }

        Obj_DropHintText.GetComponent<Text>().text = "副本开启后显示掉落道具";
        //监测玩家等级
        bool showItemStatus = false;
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        //监测国王等级
        int countryLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
        if (roseLv >= enterSceneLv)
        {
            if (countryLv >= enterCountryLv)
            {
                showItemStatus = true;
                //Obj_DropHintText.GetComponent<Text>().text = "副本掉落道具:";
            }
        }
        */

        //存储服务器Obj
        Game_PublicClassVar.Get_gameServerObj.Obj_MapEnterUI_HuoDong_1 = this.gameObject;

        //显示道具
        string dropItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemID", "ID", ScenceID, "Scene_Template");
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

        if (roseLv >= 1)
        {

            dropItemStr = "10030301;10030303;10030305;10030307;10030310;10030311;10030313;10030315;10030316;10030317;10030320;10010041;10010083;1;3";
        }

        if (roseLv >= 30)
        {
            dropItemStr = "10030401;10030403;10030405;10030407;10030409;10030411;10030413;10030415;10030416;10030418;10030420;10010041;10010083;1;3";
        }

        if (roseLv >= 40)
        {
            dropItemStr = "10030501;10030503;10030505;10030507;10030509;10030511;10030513;10030515;10030516;10030518;10030520;10010041;10010083;1;3";
        }

        if (roseLv >= 50)
        {
            dropItemStr = "10030601;10030603;10030605;10030607;10030609;10030611;10030613;10030615;10030616;10030618;10030620;10010041;10010083;1;3";
        }

        sceneDropItemList = dropItemStr.Split(';');
        float show_X = 0;
        int lieNum = 6;
        int lieNumSum = 0;
        int hangNumSum = 0;
        for (int i = 0; i <= sceneDropItemList.Length - 1; i++) {
            if (sceneDropItemList[i] != "") {

                lieNumSum = lieNumSum + 1;
                if (lieNumSum >= lieNum)
                {
                    lieNumSum = 1;
                    hangNumSum = hangNumSum + 1;
                }

                GameObject dropItemID = (GameObject)Instantiate(Obj_DropItemObj);
                dropItemID.transform.SetParent(Obj_DropItemShowSet.transform);
                dropItemID.transform.localScale = new Vector3(1, 1, 1);
                dropItemID.transform.localPosition = new Vector3(show_X + 80 * lieNumSum, hangNumSum * -80, 0);
                
                dropItemID.GetComponent<UI_FuBenDropItemObj>().ItemID = sceneDropItemList[i];
                dropItemID.GetComponent<UI_FuBenDropItemObj>().ifshow = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Btn_EnterScene() {

        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("封测版本不开启此功能！");
        //return;

        //判定服务器是否连接
        if (Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus == false) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_104");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未连接至服务器！");
            return;
        }

        //监测玩家等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

        //监测国王等级
        int countryLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
        if (roseLv >= enterSceneLv)
        {
            //发送进入请求
            //EnterScence();
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20001004, "");
        }
        else {

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_216");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_215");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + enterSceneLv + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("角色等级不足！请提升至：" + enterSceneLv + "级");
        }
    }

    public void EnterScence() {

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", ScenceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", "50003", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //注销
        Destroy(Obj_FuBen);

        string sceneTransferID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ScenceTransferID", "ID", ScenceID, "Scene_Template");
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = sceneTransferID;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();
        //播放音效
        Game_PublicClassVar.Get_function_UI.PlaySource("10003", "1");

    }

}