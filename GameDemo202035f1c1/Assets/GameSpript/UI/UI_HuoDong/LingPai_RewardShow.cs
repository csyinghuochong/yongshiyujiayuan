using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class LingPai_RewardShow : MonoBehaviour {

    public ObscuredString LingPaiID;
    public GameObject Obj_LingPaiLv;
    public GameObject Obj_LingPaiItemShow;
    public GameObject Obj_LingPai_Com_Set;
    public GameObject Obj_LingPai_Gold_Set;
    public GameObject Obj_LingPai_ZuanShi_Set;

    public GameObject Obj_LingQuHint_Com;
    public GameObject Obj_LingQuHint_Gold;
    public GameObject Obj_LingQuHint_ZuanShi;

    public GameObject Obj_LingQuBtn_Com;
    public GameObject Obj_LingQuBtn_Gold;
    public GameObject Obj_LingQuBtn_ZuanShi;

    private ObscuredString reward_Com;
    private ObscuredString reward_Gold;
    private ObscuredString reward_ZuanShi;

    // Use this for initialization
    void Start () {

        //显示奖励
        reward_Com = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Reward_Com", "ID", LingPaiID, "LingPai_Template");
        reward_Gold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Reward_Gold", "ID", LingPaiID, "LingPai_Template");
        reward_ZuanShi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Reward_ZuanShi", "ID", LingPaiID, "LingPai_Template");
        ShowReward(reward_Com, Obj_LingPai_Com_Set);
        ShowReward(reward_Gold, Obj_LingPai_Gold_Set);
        ShowReward(reward_ZuanShi, Obj_LingPai_ZuanShi_Set);

        //显示等级
        string lv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", LingPaiID, "LingPai_Template");
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("级");
        Obj_LingPaiLv.GetComponent<Text>().text = lv + langStr;

        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= int.Parse(lv))
        {
            Obj_LingQuBtn_Com.SetActive(true);
            Obj_LingQuBtn_Gold.SetActive(true);
            Obj_LingQuBtn_ZuanShi.SetActive(true);
        }
        else {
            Obj_LingQuBtn_Com.SetActive(false);
            Obj_LingQuBtn_Gold.SetActive(false);
            Obj_LingQuBtn_ZuanShi.SetActive(false);
        }

        //显示是否已经领取
        //普通领取
        if (IfLingQuLingPai(LingPaiID + "_Com"))
        {
            Obj_LingQuHint_Com.SetActive(true);
            Obj_LingQuBtn_Com.SetActive(false);
        }
        else {
            Obj_LingQuHint_Com.SetActive(false);
        }
        //黄金领取
        if (IfLingQuLingPai(LingPaiID + "_Gold"))
        {
            Obj_LingQuHint_Gold.SetActive(true);
            Obj_LingQuBtn_Gold.SetActive(true);
        }
        else
        {
            Obj_LingQuHint_Gold.SetActive(false);
        }
        //钻石领取
        if (IfLingQuLingPai(LingPaiID + "_ZuanShi"))
        {
            Obj_LingQuHint_ZuanShi.SetActive(true);
            Obj_LingQuBtn_ZuanShi.SetActive(true);
        }
        else
        {
            Obj_LingQuHint_ZuanShi.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowReward(string reward_Com,GameObject Obj_LingPai_Set) {

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_LingPai_Set);

        string[] reward_List = reward_Com.Split(';');
        for (int i = 0; i < reward_List.Length; i++)
        {

            //显示奖励
            GameObject RewardObj = (GameObject)Instantiate(Obj_LingPaiItemShow);
            RewardObj.transform.SetParent(Obj_LingPai_Set.transform);

            string itemID = reward_List[i].Split(',')[0];
            string itemNum = reward_List[i].Split(',')[1];

            RewardObj.GetComponent<UI_CommonItemShow_1>().ItemID = itemID;
            RewardObj.GetComponent<UI_CommonItemShow_1>().ItemNum = itemNum;

        }
    }

    //领取道具奖励
    public void Btn_LingQuReward(string LingQuType) {

        //检测是否满足开启条件
        string rmbValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (rmbValue == "")
        {
            rmbValue = "0";
        }
        int rmb = int.Parse(rmbValue);
        if (LingQuType == "1")
        {
            if (rmb < 98)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_27");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("无法领取!未达到黄金令牌");
                return;
            }
        }

        if (LingQuType == "2")
        {
            if (rmb < 198)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_28");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("无法领取!未达到钻石令牌");
                return;
            }
        }

        //检测背包是否有1个孔位
        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() <= 1) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_29");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("领取奖励需要预留1个道具位置");
            return;
        }

        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        string lv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", LingPaiID, "LingPai_Template");
        int needLv = int.Parse(lv);
        if (roseLv >= needLv) {
            //发送奖励
            switch (LingQuType) {
                //普通
                case "0":
                    //写入令牌奖励
                    if (WriteLingPai(LingPaiID + "_Com")) {
                        string[] reward_Com_List = reward_Com.ToString().Split(';');
                        for (int i = 0; i < reward_Com_List.Length; i++)
                        {
                            string itemID = reward_Com_List[i].Split(',')[0];
                            string itemNum = reward_Com_List[i].Split(',')[1];
                            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID, int.Parse(itemNum), "0", 0, "0", true, "39");
                        }
                        Obj_LingQuHint_Com.SetActive(true);
                    }
                    break;
                //黄金令牌
                case "1":
                    //写入令牌奖励
                    if (WriteLingPai(LingPaiID + "_Gold")) {
                        string[] reward_Gold_List = reward_Gold.ToString().Split(';');
                        for (int i = 0; i < reward_Gold_List.Length; i++)
                        {
                            string itemID = reward_Gold_List[i].Split(',')[0];
                            string itemNum = reward_Gold_List[i].Split(',')[1];
                            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID, int.Parse(itemNum), "0", 0, "0", true, "40");
                        }
                        Obj_LingQuHint_Gold.SetActive(true);
                    }
                    break;
                //钻石令牌
                case "2":
                    //写入令牌奖励
                    if (WriteLingPai(LingPaiID + "_ZuanShi")) {
                        string[] reward_ZuanShi_List = reward_ZuanShi.ToString().Split(';');
                        for (int i = 0; i < reward_ZuanShi_List.Length; i++)
                        {
                            string itemID = reward_ZuanShi_List[i].Split(',')[0];
                            string itemNum = reward_ZuanShi_List[i].Split(',')[1];
                            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID, int.Parse(itemNum), "0", 0, "0", true, "41");
                        }
                        Obj_LingQuHint_ZuanShi.SetActive(true);
                    }
                    break;
            }
        }
    }

    //写入令牌
    public bool WriteLingPai(string writeStr) {

        string lingPaiRewardID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LingPaiRewardID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (lingPaiRewardID.Contains(writeStr) == true) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_33");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前奖励已领取!");
            return false;
        }

        if (lingPaiRewardID == "") {
            lingPaiRewardID = writeStr;
        }
        else
        {
            lingPaiRewardID = lingPaiRewardID + ";" + writeStr;
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LingPaiRewardID", lingPaiRewardID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        return true;

    }

    //检测是否领取
    public bool IfLingQuLingPai(string lingPaiID) {
        string lingPaiRewardID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LingPaiRewardID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (lingPaiRewardID.Contains(lingPaiID) == true){
            return true;
        }
        else {
            return false;
        }
    }
}
