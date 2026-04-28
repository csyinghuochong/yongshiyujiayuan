using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class UI_FuBen_ShangHaiRewadSet : MonoBehaviour {

    public ObscuredString NowShangHaiID;
    public GameObject Obj_ShangHaiShowSet;
    public GameObject Obj_JiHuoLvShow;
    public GameObject Obj_JiHuoTiaoJian;
    public GameObject Obj_JiHuoRewardSet;
    public GameObject Obj_ShangHaiShowListSet;
    public GameObject Obj_ShangHaiShowList;
    public GameObject Obj_JiHuoRewardObj;
    public GameObject Obj_Btn_YiLingQu;
    public GameObject Obj_Btn_LingQu;
    public GameObject Obj_Btn_ZuiGaoShiLianLv;

    // Use this for initialization
    void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_ShangHaiShowSet);

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ShangHaiShowListSet);

        string showIDStr = "10001;10005;10010;10015;10020;10025;10030;10035;10040;10045;10050;10055;10060;10065;10070;10075;10080;10085;10090;10095;10100;10105;10110;10115;10120;10125;10130";
        string[] showIDList = showIDStr.Split(';');
        for (int i = 0; i < showIDList.Length; i++) {
            GameObject obj = (GameObject)Instantiate(Obj_ShangHaiShowList);
            obj.transform.SetParent(Obj_ShangHaiShowListSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_ShangHaiRewardShowList>().ShangHaiID = showIDList[i];
            obj.GetComponent<UI_ShangHaiRewardShowList>().Obj_Par = this.gameObject;
        }

        //默认显示第一个,后面可以调整为默认显示第一个未领取的
        NowShangHaiID = showIDList[0];

        ShowRewardShow();

    }
	
	// Update is called once per frame
	void Update () {
		

	}


    public void ShowRewardShow() {

        string lvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", NowShangHaiID, "FuBenShangHai_Template");
        Obj_JiHuoLvShow.GetComponent<Text>().text = "试炼等级:" + lvStr;

        string damgeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", NowShangHaiID, "FuBenShangHai_Template");
        Obj_JiHuoTiaoJian.GetComponent<Text>().text = "需要试炼总伤害达到:" + damgeStr + "激活";

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_JiHuoRewardSet);
        string lvRewadStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LvRewad", "ID", NowShangHaiID, "FuBenShangHai_Template");
        if (lvRewadStr != "" && lvRewadStr != "0") {
            string[] rewardItemList = lvRewadStr.Split(';');
            for (int i = 0; i < rewardItemList.Length; i++)
            {
                //显示奖励
                string[] itemList = rewardItemList[i].Split(',');
                GameObject itemObj = (GameObject)Instantiate(Obj_JiHuoRewardObj);
                itemObj.transform.SetParent(Obj_JiHuoRewardSet.transform);
                itemObj.transform.localScale = new Vector3(1, 1, 1);
                itemObj.GetComponent<UI_Common_ItemIcon>().ItemID = itemList[0];
                itemObj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = int.Parse(itemList[1]);
                itemObj.GetComponent<UI_Common_ItemIcon>().Obj_NeedItemNum.active = true;
                itemObj.GetComponent<UI_Common_ItemIcon>().UpdateItem();
            }
        }

        for (int i = 0; i < Obj_ShangHaiShowListSet.transform.childCount; i++)
        {
            GameObject go = Obj_ShangHaiShowListSet.transform.GetChild(i).gameObject;
            if (go.GetComponent<UI_ShangHaiRewardShowList>() != null) {
                go.GetComponent<UI_ShangHaiRewardShowList>().Show();
            }
        }

        //显示是否已领取
        string rewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiLvRewardSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        if (rewardStr.Contains(NowShangHaiID))
        {
            Obj_Btn_YiLingQu.SetActive(true);
            Obj_Btn_LingQu.SetActive(false);
        }
        else
        {
            Obj_Btn_YiLingQu.SetActive(false);
            Obj_Btn_LingQu.SetActive(true);
        }

        string nowHightShangHaiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiHightID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (nowHightShangHaiID == "" || nowHightShangHaiID == "0" || nowHightShangHaiID == null)
        {
            Obj_Btn_ZuiGaoShiLianLv.GetComponent<Text>().text = "未达到试炼1级";
        }
        else {
            string nowLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowHightShangHaiID, "FuBenShangHai_Template");
            Obj_Btn_ZuiGaoShiLianLv.GetComponent<Text>().text = "当前最高试炼等级：" + nowLv;
        }
        
    }


    public void Btn_GetReward() {

        //获取最高
        string nowHightShangHaiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiHightID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (nowHightShangHaiID == "" || nowHightShangHaiID == null) {
            nowHightShangHaiID = "0";
        }
        if (int.Parse(nowHightShangHaiID) < int.Parse(NowShangHaiID)) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你还未达到此试炼等级!");
            return;
        }

        string lvRewadStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LvRewad", "ID", NowShangHaiID, "FuBenShangHai_Template");
        string[] rewardItemList = lvRewadStr.Split(';');



        string rewardStr =  Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiLvRewardSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        if (rewardStr.Contains(NowShangHaiID))
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("已经领取此奖励!");
            return;
        }
        else {

            if (Game_PublicClassVar.Get_function_Rose.BagNullNum() < rewardItemList.Length)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请预留出至少" + rewardItemList.Length + "个背包位置");
                return;
            }

            string writeValue = "";
            if (rewardStr != "" && rewardStr != "0")
            {
                writeValue = rewardStr + ";" + NowShangHaiID;
            }
            else {
                writeValue = NowShangHaiID;
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_ShangHaiLvRewardSet", writeValue,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        }


        if (lvRewadStr != "" && lvRewadStr != "0")
        {
            for (int i = 0; i < rewardItemList.Length; i++)
            {
                //奖励
                string[] itemList = rewardItemList[i].Split(',');
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemList[0], int.Parse(itemList[1]));
            }
        }

        //刷新显示
        ShowRewardShow();

    }

    public void Btn_Close() {
        Destroy(this.gameObject);
    }
}
