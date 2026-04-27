using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureUpLvSet : MonoBehaviour {

    public GameObject Obj_PastureLvName;
    public GameObject Obj_PastureRenKou;

    public GameObject Obj_PastureRenKouNext;
    public GameObject Obj_PastureDesNext;

    public GameObject Obj_PastureCostGold;

    public GameObject Obj_Name;
    public GameObject Obj_DuiHuanShow;
    public GameObject Obj_NextRenKouShow;

    public GameObject Obj_NextShowSet;
    public GameObject Obj_ManJiLvShow;
    public GameObject Obj_NextShowNengLiDes;
    public GameObject Obj_NextKuangAddDes;

    private ObscuredInt duiHuanCostZuanShi;
    private ObscuredInt duiHuanGetPastureGold;

    public GameObject Obj_PastureUpLvGold;
    public GameObject Obj_PastureUpLvExpImg;
    public GameObject Obj_PastureUpLvExpStr;
    public GameObject Obj_PastureUpLvExpSet;
    //private ObscuredInt duiHuanCostGold;

    // Use this for initialization
    void Start () {

        string name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_Name.GetComponent<Text>().text = name;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init()
    {
        Debug.Log("初始化...");

        //获取数据
        string PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string pasName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", PastureID, "PastureUpLv_Template");
        Obj_PastureLvName.GetComponent<Text>().text = pasName;

        //人口上限
        string peopleNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PeopleNumMax", "ID", PastureID, "PastureUpLv_Template");
        Obj_PastureRenKou.GetComponent<Text>().text = peopleNumMax;

        string upLvNeedGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpLvNeedGold", "ID", PastureID, "PastureUpLv_Template");
        Obj_PastureCostGold.GetComponent<Text>().text = upLvNeedGold;

        string upLvGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpLvGold", "ID", PastureID, "PastureUpLv_Template");
        Obj_PastureUpLvGold.GetComponent<Text>().text = upLvGold;

        string upLvExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpLvExp", "ID", PastureID, "PastureUpLv_Template");
        string nowExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowExp == "" || nowExp == null) {
            nowExp = "0";
        }
        Obj_PastureUpLvExpStr.GetComponent<Text>().text = nowExp + "/" + upLvExp;
        Obj_PastureUpLvExpImg.GetComponent<Image>().fillAmount = float.Parse(nowExp) / float.Parse(upLvExp);

        string nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", PastureID, "PastureUpLv_Template");
        if (nextID != "" && nextID != "0" && nextID != null)
        {

            //人口上限
            string nextPeopleNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PeopleNumMax", "ID", nextID, "PastureUpLv_Template");
            Obj_PastureRenKouNext.GetComponent<Text>().text = nextPeopleNumMax;

            //解锁描述
            string des = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", nextID, "PastureUpLv_Template");
            Obj_PastureDesNext.GetComponent<Text>().text = des;

            //能力上限描述
            string nowZuoQiNengLiLvMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiNengLiLvMax", "ID", nextID, "PastureUpLv_Template");
            Obj_NextShowNengLiDes.GetComponent<Text>().text = nowZuoQiNengLiLvMax+"级";

            string nowKuangGoldRewardPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("KuangGoldRewardPro", "ID", nextID, "PastureUpLv_Template");
            Obj_NextKuangAddDes.GetComponent<Text>().text = "矿产资源收益率提升:" + ((float.Parse(nowKuangGoldRewardPro) - 1) * 100).ToString() + "%";

            Obj_NextShowSet.SetActive(true);

            Obj_ManJiLvShow.SetActive(false);

        }
        else {
            //Obj_PastureRenKou.GetComponent<Text>().text = "恭喜你已达到家园满级";
            Obj_PastureDesNext.GetComponent<Text>().text = "恭喜你已达到家园满级";
            Obj_NextRenKouShow.SetActive(false);
            Obj_ManJiLvShow.SetActive(true);
            Obj_NextShowSet.SetActive(false);
            Obj_PastureUpLvExpSet.SetActive(false);
        }

        duiHuanCostZuanShi = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanCostZuanShi", "ID", PastureID, "PastureUpLv_Template"));
        duiHuanGetPastureGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanGetPastureGold", "ID", PastureID, "PastureUpLv_Template"));
        //duiHuanCostGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanGetPastureGold", "ID", PastureID, "PastureUpLv_Template"));



    }

    //点击升级按钮
    public void Btn_UpLv() {

        string PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        int upLvNeedGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpLvNeedGold", "ID", PastureID, "PastureUpLv_Template"));
        if (Game_PublicClassVar.function_Rose.GetRosePastureGold() >= upLvNeedGold)
        {

            bool ifUpdateStatus = Game_PublicClassVar.Get_function_Pasture.PastureUpLv();
            if (ifUpdateStatus)
            {
                Game_PublicClassVar.Get_function_Rose.CostReward("5", upLvNeedGold.ToString());
                Init();
            }
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("牧场资金不足,无法升级!");
        }

    }


    //点击升级按钮
    public void Btn_UpLv_Gold()
    {

        string PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        int upLvNeedGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpLvGold", "ID", PastureID, "PastureUpLv_Template"));
        if (Game_PublicClassVar.function_Rose.GetRoseMoney() >= upLvNeedGold)
        {
            
            bool ifUpdateStatus = Game_PublicClassVar.Get_function_Pasture.PastureUpLv();
            if (ifUpdateStatus)
            {
                Game_PublicClassVar.Get_function_Rose.CostReward("1", upLvNeedGold.ToString());
                Init();
            }
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("金币不足,无法升级!");
        }

    }

    //兑换
    public void Btn_DuiHuan()
    {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否消耗" + duiHuanCostZuanShi + "钻石兑换" + duiHuanGetPastureGold + "家园资金", Game_PublicClassVar.Get_function_Pasture.DuiHuanPastureGold, Game_PublicClassVar.Get_function_Pasture.DuiHuanPastureGold_Ten, "系统提示","兑换一次","兑换十次",null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }


    //兑换
    public void Btn_DuiHuan_Gold()
    {

        string nowNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureDuiHuanFreeNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowNum == "" || nowNum == null) {
            nowNum = "0";
        }

        if (int.Parse(nowNum)>=1) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日已经领取,请明日再来");
            return;
        }


        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("每天可以免费领取一次家园资金,是否现在领取?", DuiHuanGold, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }


    public void DuiHuanGold() {

        string nowNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureDuiHuanFreeNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowNum == "" || nowNum == null)
        {
            nowNum = "0";
        }

        if (int.Parse(nowNum) >= 1)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日已经领取,请明日再来");
            return;
        }

        string PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

        int duiHuanGetPastureGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanGetPastureGold", "ID", PastureID, "PastureUpLv_Template"));

        Game_PublicClassVar.Get_function_Rose.SendReward("5", duiHuanGetPastureGold.ToString());

        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已领取家园资金:" + duiHuanGetPastureGold);
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("家园资金已发放!");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureDuiHuanFreeNum", (nowNum + 1).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
    }

    /*
    public void DuiHuan() {

        //

        if (duiHuanCostZuanShi <= 0) {
            return;
        }

        int nowZuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        if (nowZuanShi >= duiHuanCostZuanShi)
        {
            //扣除钻石发送牧场资金
            Game_PublicClassVar.Get_function_Rose.CostRMB(duiHuanCostZuanShi);
            Game_PublicClassVar.Get_function_Rose.SendReward("5", duiHuanGetPastureGold.ToString());
        }
        else {
            //提示钻石不足
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足,无法兑换");
        }

    }

    public void DuiHuan_Ten()
    {

        ObscuredInt nowDuiHuanCostZuanShi = duiHuanCostZuanShi * 10;

        //
        if (nowDuiHuanCostZuanShi <= 0)
        {
            return;
        }

        int nowZuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        if (nowZuanShi >= nowDuiHuanCostZuanShi)
        {
            //扣除钻石发送牧场资金
            Game_PublicClassVar.Get_function_Rose.CostRMB(nowDuiHuanCostZuanShi);
            Game_PublicClassVar.Get_function_Rose.SendReward("5", (duiHuanGetPastureGold * 10).ToString());
        }
        else
        {
            //提示钻石不足
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足,无法兑换");
        }

    }
    */
}
