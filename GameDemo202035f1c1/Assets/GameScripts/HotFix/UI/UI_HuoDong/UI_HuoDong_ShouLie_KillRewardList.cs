using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_HuoDong_ShouLie_KillRewardList : MonoBehaviour {

    public ObscuredString ShouLieRewardID;
    public ObscuredString NowKillNum;
    public ObscuredString NeddKillNum;
    public ObscuredString LingQuNum;
    public ObscuredString LingQuMax;
    public ObscuredString LingQuRewardStr;
    public GameObject Obj_NeddKillNum;
    public GameObject Obj_LingQuNum;
    public GameObject Obj_RewardList;
    public GameObject Obj_RewardListSet;
    public GameObject Obj_YiLingQu;
    public GameObject Obj_LingQuBtn;

	// Use this for initialization
	void Start () {
        Init();
    }
	
	// Update is called once per frame
	void Update () {
		

	}


    public void Init() {
        NowKillNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuoDong_ShouLie_KillNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (NowKillNum == "" || NowKillNum == null)
        {
            NowKillNum = "0";
        }

        //显示领取人数
        Obj_NeddKillNum.GetComponent<Text>().text = NowKillNum + "/" + NeddKillNum;
        Obj_LingQuNum.GetComponent<Text>().text = LingQuNum + "/" + LingQuMax;

        //显示奖励
        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_RewardListSet);
        //创建
        string[] rewardStrList = LingQuRewardStr.ToString().Split(';');
        for (int i = 0; i < rewardStrList.Length; i++)
        {
            GameObject obj = (GameObject)Instantiate(Obj_RewardList);
            obj.transform.SetParent(Obj_RewardListSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            string[] itemList = rewardStrList[i].Split(',');
            if (itemList.Length >= 2)
            {
                obj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                obj.GetComponent<UI_Common_ItemIcon>().ItemID = itemList[0];
                obj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = int.Parse(itemList[1]);
            }
        }

        //检测自身是否领取
        string rewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuoDong_ShouLie_Reward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (rewardStr.Contains(ShouLieRewardID))
        {
            Obj_YiLingQu.SetActive(true);
            Obj_LingQuBtn.SetActive(false);
        }
        else
        {
            Obj_YiLingQu.SetActive(false);
            Obj_LingQuBtn.SetActive(true);
        }
    }

    //领取
    public void Btn_LingQu() {

        if (ShouLieRewardID == "" || ShouLieRewardID == "0"|| ShouLieRewardID==null) {
            return;
        }
        
        if (NeddKillNum == ""|| NeddKillNum == "0"|| NeddKillNum == null) {
            return;
        }
        
        string rewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuoDong_ShouLie_Reward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (rewardStr.Contains(ShouLieRewardID)) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("此奖励已领取");
            return;
        }

        //判断人数
        if (int.Parse(LingQuNum) >= int.Parse(LingQuMax)) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("次奖励已领取完毕");
            return;
        }

        //检测背包数据
        string[] rewardStrList = LingQuRewardStr.ToString().Split(';');
        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() < rewardStrList.Length) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请背包预留" + rewardStrList.Length + "个位置");
            return;
        }

        //判定角色击杀数
        NowKillNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuoDong_ShouLie_KillNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (NowKillNum == "" || NowKillNum == null)
        {
            NowKillNum = "0";
        }

        if (int.Parse(NowKillNum) >= int.Parse(NeddKillNum))
        {

            if (rewardStr == "" || ShouLieRewardID == "0" || ShouLieRewardID == null)
            {
                rewardStr = ShouLieRewardID;
            }
            else
            {
                rewardStr = rewardStr + ";" + ShouLieRewardID;
            }

            //存储数据
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HuoDong_ShouLie_Reward", rewardStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

            //发送奖励
            Game_PublicClassVar.Get_function_Rose.Rose_SendRewardStr(LingQuRewardStr);
            //刷新显示
            Init();

            //发送服务器消息
            Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
            comStr_4.str_1 = ShouLieRewardID;
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001903,comStr_4);

        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未达到指定击败次数");
        }

    }
}
