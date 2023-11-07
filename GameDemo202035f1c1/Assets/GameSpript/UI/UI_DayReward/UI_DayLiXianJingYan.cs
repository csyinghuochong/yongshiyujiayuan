using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class UI_DayLiXianJingYan : MonoBehaviour {

    private ObscuredString ChuBeiExp;
    public ObscuredInt ChuBeiNum;

    public GameObject Obj_ChuBeiExp_Text;
    public GameObject Obj_ChuBeiExp_Img;
    public GameObject Obj_ChuBeiNum;
    public GameObject Obj_HuoYueRewardHint;
    private GameObject HuoYueRewardHintObj;

    // Use this for initialization
    void Start () {
        Init();
    }

    //初始化显示
    private void Init() {

        //读取当前的离线经验和离线点数
        ChuBeiExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChuBeiExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        string offLinkExpMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffLinkExp", "ID", roseLv.ToString(), "RoseExp_Template");

        int ChuBeiInt = int.Parse(ChuBeiExp);
        int offLinkExpMaxInt = int.Parse(offLinkExpMax);
        string ChuBeiExpStr = ChuBeiInt.ToString();
        string offLinkExpMaxStr = offLinkExpMaxInt.ToString();

        if (ChuBeiInt >= 10000) {
            ChuBeiExpStr = ((int)(ChuBeiInt / 10000)).ToString() + "w";
        }
        if (offLinkExpMaxInt >= 10000)
        {
            offLinkExpMaxStr = ((int)(offLinkExpMaxInt / 10000)).ToString() + "w";
        }
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("剩余储备经验");
        Obj_ChuBeiExp_Text.GetComponent<Text>().text = langStr + ":" + ChuBeiExpStr + "/" + offLinkExpMaxStr;
        Obj_ChuBeiExp_Img.GetComponent<Image>().fillAmount = float.Parse(ChuBeiExp) / float.Parse(offLinkExpMax);

        //显示离线点数
        ObscuredString ChuBeiNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChuBeiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (ChuBeiNumStr == "") {
            ChuBeiNumStr = "0";
        }

        ChuBeiNum = int.Parse(ChuBeiNumStr);
        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("当前离线点数");
        Obj_ChuBeiNum.GetComponent<Text>().text = langStr + ":" + ChuBeiNum.ToString();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //消耗钻石
    public void Btn_Reward(string type) {

        //判定背包是否有1个空格
        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() < 1) {
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_1");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr_1);
            return;
        }

        ObscuredString dropID = "";
        ObscuredInt costNum = 0;

        switch (type) {

            case "1":
                dropID = "50090310";
                costNum = 480;
                break;

            case "2":
                dropID = "50090320";
                costNum = 960;
                break;

            case "3":
                dropID = "50090330";
                costNum = 1440;
                break;
        }

        //显示离线点数
        ObscuredString ChuBeiNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChuBeiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (ChuBeiNumStr == "")
        {
            ChuBeiNumStr = "0";
        }

        ChuBeiNum = int.Parse(ChuBeiNumStr);
        if (ChuBeiNum >= costNum)
        {
            ChuBeiNum = ChuBeiNum - costNum;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChuBeiNum", ChuBeiNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            Game_PublicClassVar.Get_function_AI.DropIDToDropItem(dropID, Vector3.zero, "79999999");

            //刷新显示
            Init();
        }
        else {
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_452");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr_1);
        }
    }



    public void Btn_RearwdShow(string huoyueDuValue)
    {

        bool ifShowTips = true;
        
        //奖励
        ObscuredString rewardStr = "";
        ObscuredString rewardTitle = "";
        ObscuredString rewardDes = "";


        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("离线宝箱奖励");

        switch (huoyueDuValue)
        {

            //普通离线宝箱
            case "1":

                rewardStr = "10010041,5;1,100000;10000016,1;10010033,1";
 
                break;

            //60活跃度
            case "2":

                rewardStr = "10010041,10;1,200000;10010083,1;10010033,1;10000017,1";

                break;

            //90活跃度
            case "3":

                rewardStr = "10010041,15;1,300000;10010083,1;10000017,1;10000015,1;10010026,1";

                break;

        }

        //显示奖励
        string[] rewardStrList = rewardStr.ToString().Split(';');
        for (int i = 0; i < rewardStrList.Length; i++)
        {
            string[] sendRewardList = rewardStrList[i].Split(',');
            if (sendRewardList.Length >= 2)
            {
                string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", sendRewardList[0], "Item_Template");
                rewardDes = rewardDes + itemName + " X " + sendRewardList[1] + "\n";
 
            }
        }

        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("以下道具随机获得一种");
        rewardDes = langStr + "\n" + rewardDes;

        if (ifShowTips)
        {
            //展示Tips
            HuoYueRewardHintObj = (GameObject)Instantiate(Obj_HuoYueRewardHint);
            HuoYueRewardHintObj.transform.SetParent(this.transform);
            HuoYueRewardHintObj.transform.localScale = new Vector3(1, 1, 1);
            HuoYueRewardHintObj.transform.position = new Vector3(Input.mousePosition.x - 220, Input.mousePosition.y - 50);
            HuoYueRewardHintObj.GetComponent<UI_DayTaskRearwdHint>().Obj_DayTaskRewardTitleHint.GetComponent<Text>().text = langStr;
            HuoYueRewardHintObj.GetComponent<UI_DayTaskRearwdHint>().Obj_DayTaskRewardHint.GetComponent<Text>().text = rewardDes;
            HuoYueRewardHintObj.GetComponent<UI_DayTaskRearwdHint>().Obj_ImgDi.GetComponent<RectTransform>().sizeDelta = new Vector2(320, 280);
            HuoYueRewardHintObj.GetComponent<UI_DayTaskRearwdHint>().Obj_ImgDi.GetComponent<RectTransform>().localPosition = new Vector3(HuoYueRewardHintObj.GetComponent<UI_DayTaskRearwdHint>().Obj_ImgDi.GetComponent<RectTransform>().localPosition.x, -60, HuoYueRewardHintObj.GetComponent<UI_DayTaskRearwdHint>().Obj_ImgDi.GetComponent<RectTransform>().localPosition.z);
        }
    }

}
