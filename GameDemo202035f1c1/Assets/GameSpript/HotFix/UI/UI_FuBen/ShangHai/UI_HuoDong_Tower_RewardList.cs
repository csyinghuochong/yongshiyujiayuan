using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_HuoDong_Tower_RewardList : MonoBehaviour {

    public ObscuredString NowCengNum;
    public ObscuredString NowRewardStr;
    public GameObject Obj_Ceng;
    public GameObject Obj_RewardSet;
    public GameObject Obj_CommonItemShow;
    public GameObject Obj_BtnLingQu;
    public GameObject Obj_ImgYiLingQu;

    // Use this for initialization
    void Start() {


    }

    // Update is called once per frame
    void Update() {


    }

    public void Init() {

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_RewardSet);

        //获取当前奖励
        string[] NowRewardList = NowRewardStr.ToString().Split(';');
        for (int i = 0; i < NowRewardList.Length; i++)
        {
            string[] rewardList = NowRewardList[i].Split(',');
            if (rewardList.Length >= 2)
            {
                //显示奖励
                GameObject itemObj = (GameObject)Instantiate(Obj_CommonItemShow);
                itemObj.transform.SetParent(Obj_RewardSet.transform);
                itemObj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                itemObj.GetComponent<UI_Common_ItemIcon>().ItemID = rewardList[0];
                itemObj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = int.Parse(rewardList[1]);
                itemObj.GetComponent<UI_Common_ItemIcon>().UpdateItem();
            }
        }

        //表示当前是否已经领取
        string TowerCengRewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TowerCengReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (TowerCengRewardStr.Contains("Ceng_" + NowCengNum))
        {
            //已经领取
            Obj_BtnLingQu.SetActive(false);
            Obj_ImgYiLingQu.SetActive(true);
        }
        else
        {
            //未领取
            Obj_BtnLingQu.SetActive(true);
            Obj_ImgYiLingQu.SetActive(false);
        }

        Obj_Ceng.GetComponent<Text>().text = NowCengNum + "层";

    }

    //领取奖励
    public void Btn_LingQu() {

        //判断当前是否满足层数
        string TowerCengStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TowerCeng", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

        if (TowerCengStr == "" || TowerCengStr == null)
        {
            TowerCengStr = "0";
        }

        if (int.Parse(NowCengNum) > int.Parse(TowerCengStr)) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请达到指定层数!");
            return;
        }

        //表示当前是否已经领取
        string TowerCengRewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TowerCengReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (TowerCengRewardStr.Contains("Ceng_" + NowCengNum))
        {
            //已经领取
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("奖励已领取!");
            return;
        }

        string[] NowRewardList = NowRewardStr.ToString().Split(';');
        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() < NowRewardList.Length) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先清理背包!");
            return;
        }

        for (int i = 0; i < NowRewardList.Length; i++)
        {
            string[] rewardList = NowRewardList[i].Split(',');
            if (rewardList.Length >= 2)
            {
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardList[0], int.Parse(rewardList[1]));
            }
        }

        string writeStr = "Ceng_" + NowCengNum;
        TowerCengRewardStr = TowerCengRewardStr + ";" + writeStr;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TowerCengReward", TowerCengRewardStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        //显示已经领取
        Obj_BtnLingQu.SetActive(false);
        Obj_ImgYiLingQu.SetActive(true);
    }
}
