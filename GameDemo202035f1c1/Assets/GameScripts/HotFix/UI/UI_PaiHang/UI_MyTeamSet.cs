using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MyTeamSet : MonoBehaviour {

    public string PetIDStr;
    public int PetXuanZhongType;
    public string PetXuanZhongID;
    public string[] PetIDList;

    public GameObject[] Obj_TeamIconList;
    public GameObject[] Obj_TeamNameList;
    public GameObject[] Obj_TeamLvList;

    public GameObject Obj_HeChengXuanZe;
    public GameObject Obj_TeamName;

    private bool UpdateDataStatus;

    // Use this for initialization
    void Start () {
        Init();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init() {

        PetIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanTeam", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (PetIDStr == "" || PetIDStr == "0") {
            PetIDStr = ";;";
        }

        string[] petIDList = PetIDStr.Split(';');

        for (int i = 0; i < petIDList.Length; i++) {

            PetIDList[i] = petIDList[i];

            if (PetIDList[i] != "0" && PetIDList[i] != "" && PetIDList[i] != null)
            {
                //获取头像ID和名称
                string nowPetID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", petIDList[i], "RosePet");
                if (nowPetID != "0" && nowPetID != "")
                {

                    string nowHeadIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeadIcon", "ID", nowPetID, "Pet_Template");

                    //获取宠物等级
                    string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", petIDList[i], "RosePet");
                    //获取宠物名称
                    string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", petIDList[i], "RosePet");

                    //显示宠物信息
                    Obj_TeamNameList[i].GetComponent<Text>().text = petName;
                    Obj_TeamLvList[i].GetComponent<Text>().text = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级") +" " + petLv;

                    //显示底图
                    Object obj = Resources.Load("PetHeadIcon/" + nowHeadIcon, typeof(Sprite));
                    Sprite img = obj as Sprite;
                    Obj_TeamIconList[i].GetComponent<Image>().sprite = img;
                }
            }
            else {
                Obj_TeamNameList[i].GetComponent<Text>().text = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("点击放入上阵宠物");
                Obj_TeamLvList[i].GetComponent<Text>().text = "";
                //Obj_TeamIconList[i].GetComponent<Image>().sprite = null;
            }
        }

        //显示战队挑战名称
        string teamName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanTeamName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Obj_TeamName.GetComponent<InputField>().text = teamName;
    }

    //修改战队名称
    public void Btn_XiuGaiTeamName() {

        string teamName = Obj_TeamName.GetComponent<InputField>().text;
        if (teamName.Length > 7)
        {
            teamName = teamName.Substring(0, 7);
        }
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetTiaoZhanTeamName", teamName,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

    }

    //修改显示
    public void AddPetTianTi() {

        PetIDList[PetXuanZhongType] = PetXuanZhongID;
        SaveSetting();
        Init();
    }


    //保存配置
    public void SaveSetting() {

        //点击上传一次玩家数据
        if (UpdateDataStatus == false) {
            UpdateDataStatus = true;
            string[] saveList = new string[] { "", "2", "预留设备号位置","5" };
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001006, saveList);
        }

        string writeStr = PetIDList[0] +";" + PetIDList[1] + ";" + PetIDList[2];
        Debug.Log("writeStr = " + writeStr);
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetTiaoZhanTeam", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_439");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);


        //上传至服务器
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiHangBang.GetComponent<UI_PaiHang>().Obj_PetTianTiSet.GetComponent<UI_PetTianTiSet>().Btn_UpdateTeam();

    }

    //选择宠物
    public void Btn_SelectPetList(int type) {
        PetXuanZhongType = type;
        GameObject HeChengXuanZeObj = (GameObject)Instantiate(Obj_HeChengXuanZe);
        HeChengXuanZeObj.transform.SetParent(this.gameObject.transform.parent.transform);
        HeChengXuanZeObj.transform.localPosition = new Vector3(0, 0, 0);
        HeChengXuanZeObj.transform.localScale = new Vector3(1, 1, 1);
        //HeChengXuanZeObj.GetComponent<UI_PetHeChengXuanZe>().HeChengWeiZhi = HeChengWeiZhi;
        HeChengXuanZeObj.GetComponent<UI_PetHeChengXuanZe>().XuanZeType = 1;
        HeChengXuanZeObj.GetComponent<UI_PetHeChengXuanZe>().FuJiObj = this.gameObject;
    }

    //关闭按钮
    public void Btn_Close() {
        this.gameObject.SetActive(false);
    }
}
