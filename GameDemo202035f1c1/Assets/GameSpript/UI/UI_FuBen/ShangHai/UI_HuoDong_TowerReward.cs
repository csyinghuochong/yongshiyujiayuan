using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HuoDong_TowerReward : MonoBehaviour {

    public GameObject Obj_CengShu;

    public GameObject Obj_CengRewardListSet;
    public GameObject Obj_CengRewardList;

    // Use this for initialization
    void Start () {

        //表示当前是否已经领取
        string TowerCengStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TowerCeng", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

        if (TowerCengStr == "" || TowerCengStr == null) {
            TowerCengStr = "0";
        }

        Obj_CengShu.GetComponent<Text>().text = TowerCengStr;

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_CengRewardListSet);

        string rewardCengStr = "10;20;30;40;50;60;70;80;90;100";
        string[] rewardCengList = rewardCengStr.Split(';');

        for (int i = 0; i< rewardCengList.Length; i++)
        {
            string TowerID = (10000 + int.Parse(rewardCengList[i])).ToString();
            string rewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Reward", "ID", TowerID, "HuoDong_Tower_Template");
            GameObject obj = (GameObject)Instantiate(Obj_CengRewardList);
            obj.transform.SetParent(Obj_CengRewardListSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_HuoDong_Tower_RewardList>().NowCengNum = rewardCengList[i];
            obj.GetComponent<UI_HuoDong_Tower_RewardList>().NowRewardStr = rewardStr;
            obj.GetComponent<UI_HuoDong_Tower_RewardList>().Init();
        }
	}
	
	// Update is called once per frame
	void Update () {
		


	}


    //进入魔塔
    public void EnterMoTa() {

        
        if (Game_PublicClassVar.Get_gameLinkServerObj.HuoDong_Tower_Status == false) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("魔塔活动已经结束,请明日再来!");
            return;
        }

        string TowerCengTimeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TowerCengTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (TowerCengTimeStr == "" || TowerCengTimeStr == null)
        {
            TowerCengTimeStr = "900";
        }

        if (int.Parse(TowerCengTimeStr) <= 5) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日在魔塔内的时间已经用完,请明日再来!");
            return;
        }
        
        string ScenceID = "100006";
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", ScenceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        string sceneTransferID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ScenceTransferID", "ID", ScenceID, "Scene_Template");
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = sceneTransferID;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();

        //播放音效
        Game_PublicClassVar.Get_function_UI.PlaySource("10003", "1");

        Destroy(this.gameObject);

    }

    //关闭游戏
    public void Btn_Close() {

        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenGameTower();

    }
}
