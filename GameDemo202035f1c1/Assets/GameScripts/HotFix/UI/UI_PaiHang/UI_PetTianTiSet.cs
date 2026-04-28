using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PetTianTiSet : MonoBehaviour {

    public ObscuredString SelfRank;
    public GameObject Obj_SelfRank;
    public ObscuredInt MaxTiaoZhanNum;                              //最大挑战次数

    public Pro_RankPetListData proRankListData;

    public GameObject Obj_TiaoZhanNumStr;
    public GameObject Obj_PetTianTiSet;                     //宠物天梯
    public GameObject Obj_PetTianTiListShow;                //实力排行榜列表Obj

    public GameObject Obj_PetTianTiRewardSet;

    public GameObject Obj_MyTeamSet;

    public string TeamName;


    // Use this for initialization
    void Start () {

        MaxTiaoZhanNum = 5;
        Game_PublicClassVar.Get_gameServerObj.Obj_PetTianTi = this.gameObject;
        //Init();
        //像服务器发送请求
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001502, "");
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001504, "");

        //初始化值(防止奖励发错)
        Game_PublicClassVar.Get_gameLinkServerObj.LveDuoStatus = false;
        Game_PublicClassVar.Get_gameLinkServerObj.LveDuoGoldNum = 0;
    }

	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        Game_PublicClassVar.Get_gameServerObj.Obj_PetTianTi = null;
    }

    //初始化
    public void Init() {

        //测试数据
        /*
        Pro_RankPetListData test = new Pro_RankPetListData();

        Pro_PetListData proPetListData = new Pro_PetListData();
        proPetListData.PetPlayerName = "测试名称1";
        proPetListData.RankVlaue = "1";
        proPetListData.PetTeamName = "测试队伍名称1";
        proPetListData.PetData = "0@10001080@51@10561@12205@378@血色软泥怪@1@5@13;4;300;11@0@3520@1320@1180@1200@1320@3000@1.05@64000001|0@10001080@45@-311@14769@810@血色软泥怪@1@15@4;26;252;6@0@3653@1320@1220@1200@884@1600@0.99@64000108;64000001|0@10003020@1@1094@1094@0@沙漠烈犬@1@20@24;3;17;19@0@2906@1496@1275@750@1161@1600@0.98@64000104";

        test.PetRankData.Add("1", proPetListData);
        test.PetRankData.Add("2", proPetListData);
        test.PetRankData.Add("3", proPetListData);
        proRankListData = test;
        */

        //获取当前挑战次数
        string petTiaoZhanNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (petTiaoZhanNum == "") {
            petTiaoZhanNum = "0";
        }

        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("今日挑战次数");
        Obj_TiaoZhanNumStr.GetComponent<Text>().text = langStr + ":" + petTiaoZhanNum + "/" + MaxTiaoZhanNum;

        //初始化挑战列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PetTianTiSet);
        //降序排序
        for (int i = 3; i >= 1; i--) {

            if (proRankListData.PetRankData.ContainsKey(i.ToString())) {

                GameObject obj = (GameObject)Instantiate(Obj_PetTianTiListShow);
                obj.transform.SetParent(Obj_PetTianTiSet.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.GetComponent<UI_PetTianTiList>().ProPetListData = proRankListData.PetRankData[i.ToString()];
                obj.GetComponent<UI_PetTianTiList>().Init();
            }
        }

        //获取自身战队名称（暂时以玩家名字命名）
        TeamName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanTeamName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (TeamName == "") {
            TeamName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData") + Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的队伍"); ;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetTiaoZhanTeamName", TeamName, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
        }

    }

    public void ShowMyRank() {
        //显示我的排名
        Obj_SelfRank.GetComponent<Text>().text = SelfRank;
    }

    //刷新排行
    public void Btn_UpdateRank() {

        //判定消耗,扣除一定金币强制刷新
        GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_37");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint1, UpdateRank, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    public void UpdateRank() {

        if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() >= 30000) {

            Game_PublicClassVar.Get_function_Rose.CostReward("1", "30000");
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001502, "1");
        }

    }

    //打开奖励列表
    public void Btn_OpenRewardList() {

        if (Obj_PetTianTiRewardSet.active)
        {
            Obj_PetTianTiRewardSet.SetActive(false);
        }
        else {
            Obj_PetTianTiRewardSet.SetActive(true);
        }
    }

    //点击编辑队伍
    public void Btn_Team() {
        Obj_MyTeamSet.SetActive(true);
    }

    //更新战队信息
    public void Btn_UpdateTeam() {
        UpdateServerPetData();
    }

    //更新服务器战队信息
    public void UpdateServerPetData() {

        Pro_ComStr_4 ProComStr4 = new Pro_ComStr_4();

        string petTiaoZhanTeam = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanTeam", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

        ProComStr4.str_1 = petTiaoZhanTeam;
        TeamName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanTeamName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (TeamName.Length > 7) {
            TeamName = TeamName.Substring(0, 7);
        }
        ProComStr4.str_2 = TeamName;

        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001501, ProComStr4);
        //Debug.Log("更新了宠物战队的信息");

    }

    //增加挑战次数
    public void Btn_AddTiaoZhanNum() {

        GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_34");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint1, AddTiaoZhanNum, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    public void AddTiaoZhanNum() {

        //判定钻石是否足够
        if (Game_PublicClassVar.Get_function_Rose.GetRoseRMB() >= 300)
        {
            //扣除钻石
            Game_PublicClassVar.Get_function_Rose.CostReward("2", "300");

            //清空挑战次数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetTiaoZhanNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        }
        else {
            string langStrHint1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_86");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint1);
        }

        //重新刷新界面
        Init();

    }
}
