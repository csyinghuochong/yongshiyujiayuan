using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PetTianTiList : MonoBehaviour {


    public ObscuredString PetID;
    public Pro_PetListData ProPetListData;

    public GameObject Obj_TeamName;
    public GameObject Obj_PlayerName;
    public GameObject Obj_RankStr;

    public string[] PetListData;
    public GameObject[] Obj_PetHeadIconList;

    public GameObject Obj_PaiHangRosePet;

    public Dictionary<string, string> roseEquipHideDic = new Dictionary<string, string>();
    public string roseEquipHideStr;

    // Use this for initialization
    void Start () {
        //Init();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //初始化
    public void Init() {

        Obj_TeamName.GetComponent<Text>().text = ProPetListData.PetTeamName;
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("拥有者");
        Obj_PlayerName.GetComponent<Text>().text = langStr + "：" + ProPetListData.PetPlayerName;
        Obj_RankStr.GetComponent<Text>().text = ProPetListData.RankVlaue;

        //显示Icon
        PetListData = ProPetListData.PetData.Split('|');
        for (int i = 0; i <= 2; i++) {

            //清空显示
            Obj_PetHeadIconList[i].GetComponent<Image>().sprite = null;

            if (PetListData.Length > i)
            {
                string[] listData = PetListData[i].Split('@');
                if (listData.Length >= 10)
                {
                    string nowPetID = listData[1];

                    //显示头像
                    string nowHeadIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeadIcon", "ID", nowPetID, "Pet_Template");

                    //显示底图
                    Object obj = Resources.Load("PetHeadIcon/" + nowHeadIcon, typeof(Sprite));
                    Sprite img = obj as Sprite;
                    Obj_PetHeadIconList[i].GetComponent<Image>().sprite = img;
                }
            }
        }

        EquipHideID();

    }

    //点击挑战
    public void Click_TiaoZhan() {


        //判断当前是否拥有队伍,没有队伍需要配置队伍
        string petTiaoZhanTeam = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanTeam", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (petTiaoZhanTeam == "" || petTiaoZhanTeam == "0" || petTiaoZhanTeam == ";;") {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_441");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            return;
        }

        //获取当前挑战次数
        string petTiaoZhanNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (petTiaoZhanNum == "")
        {
            petTiaoZhanNum = "0";
        }

        //测试功能
        //petTiaoZhanNum = "0";
        if (int.Parse(petTiaoZhanNum) >= 5) {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_440");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            return;
        }

        int writeInt = int.Parse(petTiaoZhanNum) + 1;

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetTiaoZhanNum", writeInt.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        //向服务器发送挑战信息（等服务器验证没有问题 由服务器发送消息过来进入挑战地图）


        //设置当前进入挑战信息
        Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData = ProPetListData;

        //关闭排行界面
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenPaiHangBang();

        //测试进入地图
        Game_PublicClassVar.Get_function_Rose.RoseMoveTargetMap("PetTianTi");

    }

    //展示宠物信息
    public void Btn_ShowPet()
    {
        string[] listData = PetListData[0].Split('@');
        if (listData[6] == "入侵者") {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_443");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            return;
        }

        if (PetListData.Length >= 1)
        {
            string[] petIDSetList = PetListData;

            GameObject showPetobj = (GameObject)Instantiate(Obj_PaiHangRosePet);
            showPetobj.transform.SetParent(Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.transform);
            showPetobj.GetComponent<UI_PaiHangShowPet>().SerVer_PetDataList = petIDSetList;
            showPetobj.GetComponent<UI_PaiHangShowPet>().roseEquipHideDic = roseEquipHideDic;
            showPetobj.GetComponent<UI_PaiHangShowPet>().PetXiuLianStr = ProPetListData.PetXiuLian;
            showPetobj.GetComponent<UI_PaiHangShowPet>().Inti();
            showPetobj.transform.localScale = new Vector3(1, 1, 1);
            showPetobj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            showPetobj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        }
    }


    //将玩家的隐藏属性加载到Dic中方便调用
    public void EquipHideID()
    {

        roseEquipHideStr = ProPetListData.EuqipHideStr;

        //隐藏属性ID和隐藏值
        if (roseEquipHideStr != "" && roseEquipHideStr != null)
        {
            string[] hideList = roseEquipHideStr.Split(']');
            //Debug.Log("roseEquipHideStr = " + roseEquipHideStr);
            for (int i = 0; i < hideList.Length; i++)
            {
                //Debug.Log("hideList[i] = " + hideList[i]);
                string[] hideIDPro = hideList[i].Split('[');
                //hideIDPro[0] 为0表示没有装备
                if (hideIDPro[0] != "0")
                {
                    roseEquipHideDic.Add(hideIDPro[0], hideIDPro[1]);
                }
            }
        }
    }
}
