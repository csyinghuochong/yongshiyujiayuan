using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ZhanQu_RewardLv : MonoBehaviour {
    public string ZhanQuRewardNumStr;
    public string ZhanQuRewardOnlyID;
    public bool UpdateStatus;
    public GameObject[] ObjList_RewardNum;
    public GameObject Obj_MyLv;

	// Use this for initialization
	void Start () {
        Init();
    }

    public void Init()
    {
        Game_PublicClassVar.Get_gameServerObj.Obj_ZhanQu_RewardLv = this.gameObject;
        UpdateRewardPlayerNum();
        //显示自身等级
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("我的等级");
        Obj_MyLv.GetComponent<Text>().text = langStr + ":" + Game_PublicClassVar.Get_function_Rose.GetRoseLv().ToString();
        this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (UpdateStatus) {
            UpdateStatus = false;
            string[] zhanquRewardNumList = ZhanQuRewardNumStr.Split(';');
            for (int i = 0; i < ObjList_RewardNum.Length;i++) {
                string rewardNum = "";
                switch (i) {
                    case 0:
                        rewardNum = "100";
                        break;
                    case 1:
                        rewardNum = "50";
                        break;
                    case 2:
                        rewardNum = "30";
                        break;
                    case 3:
                        rewardNum = "10";
                        break;
                    case 4:
                        rewardNum = "5";
                        break;
                }


                string[] readRewardStrList = zhanquRewardNumList[i].Split(',');
                int nowNum = readRewardStrList.Length;
                if (zhanquRewardNumList[i] == "") {
                    nowNum = 0;
                }
                
                ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().Obj_RewardNumStr.GetComponent<Text>().text = nowNum + "/" + rewardNum;
                ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().RewardNum_Now = nowNum;
                ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().RewardNum_Sum = int.Parse(rewardNum);

                //显示是否已经领取
                bool ifLingQuStatus = false;
                for (int y = 0; y< readRewardStrList.Length; y++) {
                    if (readRewardStrList[y] == ZhanQuRewardOnlyID) {
                        ifLingQuStatus = true;
                    }
                }

                //满名额
                if (nowNum >= int.Parse(rewardNum))
                {
                    //奖励已经领取隐藏领取按钮
                    ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().DengLuRewardBtn.SetActive(false);
                    ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().Obj_LabYiLingQu.SetActive(false);
                    ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().Obj_LabMan.SetActive(true);
                    ObjList_RewardNum[i].SetActive(false);
                }
                else {
                    ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().DengLuRewardBtn.SetActive(true);
                    ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().Obj_LabYiLingQu.SetActive(false);
                    ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().Obj_LabMan.SetActive(false);
                }

                //已领取
                if (ifLingQuStatus) {
                    //奖励已经领取隐藏领取按钮
                    ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().DengLuRewardBtn.SetActive(false);
                    ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().Obj_LabYiLingQu.SetActive(true);
                    ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().Obj_LabMan.SetActive(false);
                }

                //ObjList_RewardNum[i].GetComponent<UI_HuoDongRewardLvList>().
                //ObjList_RewardNum[i].GetComponent<Text>().text = "剩余数量："+ zhanquRewardNumList[i] + "/" + rewardNum;
            }
        }
	}

    void UpdateRewardPlayerNum() {

        //获取自身排名
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001060, "1");

    }
}
