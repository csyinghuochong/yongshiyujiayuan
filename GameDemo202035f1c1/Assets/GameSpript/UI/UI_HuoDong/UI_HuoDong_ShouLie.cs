using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_HuoDong_ShouLie : MonoBehaviour {

    public ObscuredString RankValue;
    public ObscuredFloat UpdateDataTime;
    public ObscuredString KillNum;
    public ObscuredFloat HuoDongTime;

    public ObscuredString ShouLieStr;
    public ObscuredString ShouLieDataStr;
    public GameObject Obj_HuoDongRankListSet;
    public GameObject Obj_HuoDongRankList;

    public GameObject Obj_HuoDongRewardListSet;
    public GameObject Obj_HuoDongRewardList;

    public GameObject Obj_RankValue;
    public GameObject Obj_KillNum;
    public GameObject Obj_HuoDongTime;
    public GameObject Obj_UpdateDataTime;

    // Use this for initialization
    void Start () {

        Game_PublicClassVar.gameServerObj.Obj_HuoDong_ShouLie = this.gameObject;
        //请求活动数据
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001902, "");
        //RankValue = "25";
        //Init();     //回头需要屏蔽
    }
	
	// Update is called once per frame
	void Update () {

        if (HuoDongTime > 0)
        {
            HuoDongTime = HuoDongTime - Time.deltaTime;
            UpdateDataTime = UpdateDataTime - Time.deltaTime;
        }


        if (UpdateDataTime > 0)
        {
            UpdateDataTime = UpdateDataTime - Time.deltaTime;
        }


        ShowTime();

        if (Game_PublicClassVar.gameLinkServer.HuoDong_ShouLie_Status) {
            if (HuoDongTime <= 0)
            {
                Debug.Log("HuoDongTime = " + HuoDongTime);
                //Game_PublicClassVar.gameLinkServer.HuoDong_ShouLie_Status = false;
                Obj_UpdateDataTime.GetComponent<Text>().text = "请重新打开界面刷新数据!";
                Obj_UpdateDataTime.GetComponent<Text>().fontSize = 18;
            }
        }


        if (UpdateDataTime <= 0) {
            //重新申请排行数据
            //UpdateDataTime = 30;
            //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001902, "");
        }
    }

    public void Init() {

        //HuoDongTime = 750;
        //ShouLieStr = "1,测试玩家1,500;2,测试玩家1,500;3,测试玩家1,500;4,测试玩家1,500;5,测试玩家1,500";
        //ShouLieDataStr = "500@750@0@1,10000;1,10000;1,10000#500@750@1000@1,10000;1,10000;1,10000";

        //Debug.Log("ShouLieStr = " + ShouLieStr);
        //Debug.Log("ShouLieDataStr = " + ShouLieDataStr);

        //显示排名信息
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_HuoDongRankListSet);
        string[] shouLieStrList = ShouLieStr.ToString().Split(';');
        for (int i = 0; i < shouLieStrList.Length; i++)
        {
            GameObject obj = (GameObject)Instantiate(Obj_HuoDongRankList);
            obj.transform.SetParent(Obj_HuoDongRankListSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_HuoDong_ShouLie_PaiMingXinXi>().PaiMingValue = (i + 1).ToString();
            obj.GetComponent<UI_HuoDong_ShouLie_PaiMingXinXi>().PaiMingXinXiData = shouLieStrList[i];
            obj.GetComponent<UI_HuoDong_ShouLie_PaiMingXinXi>().Init();
        }

        //显示奖励信息
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_HuoDongRewardListSet);
        string[] shouLieRewardList = ShouLieDataStr.ToString().Split('#');
        for (int i = 0; i < shouLieRewardList.Length; i++)
        {

            GameObject obj = (GameObject)Instantiate(Obj_HuoDongRewardList);
            obj.transform.SetParent(Obj_HuoDongRewardListSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            string[] rewardList = shouLieRewardList[i].Split('@');
            if (rewardList.Length >= 4)
            {
                obj.GetComponent<UI_HuoDong_ShouLie_KillRewardList>().ShouLieRewardID = "1000" + i;
                obj.GetComponent<UI_HuoDong_ShouLie_KillRewardList>().LingQuRewardStr = rewardList[3];
                obj.GetComponent<UI_HuoDong_ShouLie_KillRewardList>().LingQuNum = rewardList[0];
                obj.GetComponent<UI_HuoDong_ShouLie_KillRewardList>().LingQuMax = rewardList[1];
                obj.GetComponent<UI_HuoDong_ShouLie_KillRewardList>().NeddKillNum = rewardList[2];
            }
        }

        //显示其他信息
        //KillNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuoDong_ShouLie_KillNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (KillNum == "" || KillNum == null)
        {
            KillNum = "0";
        }



        //Obj_RankValue.GetComponent<Text>().text = RankValue;
        Obj_KillNum.GetComponent<Text>().text = KillNum;

        //检测时间
        ShowTime();

        //如果本地击杀数据和服务器不一致,将保持服务器的击杀数据
        string localKillNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuoDong_ShouLie_KillNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (localKillNum != KillNum) {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HuoDong_ShouLie_KillNum", KillNum, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
        }
    }

    private void ShowTime()
    {

        //检测时间
        if (HuoDongTime >= 60)
        {
            int min = (int)((int)HuoDongTime / 60);
            int sec = (int)HuoDongTime % 60;
            Obj_HuoDongTime.GetComponent<Text>().text = min + "分" + sec + "秒";
        }
        else {
            Obj_HuoDongTime.GetComponent<Text>().text = HuoDongTime + "秒";
        }

        if (Game_PublicClassVar.gameLinkServer.HuoDong_ShouLie_Status == false) {
            Obj_HuoDongTime.GetComponent<Text>().text = "活动已结束";
        }

        //刷新时间
        Obj_UpdateDataTime.GetComponent<Text>().text = "刷新数据:" + (int)(UpdateDataTime) + "秒";

    }

    public void Btn_Close() {

        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenGameShouLie();

    }
}
