using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Mail : MonoBehaviour {

	public bool MailUpdateStatus;
	public Pro_Mail pro_Mail;

    private string zhanghaoID;
	public string SelectMailID;

	public GameObject Obj_MailList;
	public GameObject Obj_MailListSet;
    public GameObject Obj_MailRewardSet;

	public GameObject Obj_MailName;
	public GameObject Obj_MailDes;
	public GameObject Obj_MailRewardObj;
	public GameObject Obj_MailRewardListSet;

    public GameObject Obj_MailNullHint;
    public GameObject Obj_ShowMailSet;

    private bool LingQuMailStatus;
	// Use this for initialization
	void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        Game_PublicClassVar.Get_gameServerObj.Obj_Mail = this.gameObject;
        /*
		pro_Mail = new Pro_Mail();
		Pro_MailStrList sss;
		sss.str_1 = "游戏维护福利";
		sss.str_2 = "游戏例行维护,感谢你对游戏一直的支持,下面是奖励,请你查收！";
        sss.str_3 = "1|100|0#10030101|1|3,2;4,2";
		pro_Mail.MailData.Add ("1001", sss);
		sss.str_1 = "测试邮箱名称222";
		pro_Mail.MailData.Add ("1002", sss);
		sss.str_1 = "测试邮箱名称333";
		pro_Mail.MailData.Add ("1003", sss);

		//邮件更新
		MailUpdateStatus = true;
        */

        //初始化不显示邮件内容
        Obj_ShowMailSet.SetActive(false);
        Obj_MailNullHint.SetActive(true);

        zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001040, zhanghaoID);      //老
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010401, zhanghaoID);            //新

        //关闭邮件提示
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MailHint.SetActive(false);
        Game_PublicClassVar.Get_gameLinkServerObj.MailHintStatus = false;

    }
	
	// Update is called once per frame
	void Update () {
	
		if(MailUpdateStatus){
			MailUpdateStatus = false;
			//设置默认显示第一个


			//更新邮件信息
			ShowMail();
		}
	}

	//显示邮件列表
	public void ShowMail(){
        Debug.Log("更新邮件");

        LingQuMailStatus = false;

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_MailListSet);

		foreach (string mailID in pro_Mail.MailData.Keys) {
			//Debug.Log ("mailID = "+ mailID);
			//创建邮件列表
			GameObject mailList = (GameObject)Instantiate(Obj_MailList);
			mailList.transform.SetParent(Obj_MailListSet.transform);
			mailList.transform.localScale = new Vector3 (1, 1, 1);
			mailList.GetComponent<UI_MailList>().MailID = mailID;
			mailList.GetComponent<UI_MailList>().Obj_MailName.GetComponent<Text>().text = pro_Mail.MailData[mailID].str_1;

			//设置当前选中的邮件id
			if (SelectMailID == "") {
				SelectMailID = mailID;
			}

        }

        //邮件数量大于1在显示邮件
        if (pro_Mail.MailData.Keys.Count > 0)
        {
            //显示邮件内容
            Btn_ShowMailData();
        }
        else {
            //不显示邮件内容
            Obj_ShowMailSet.SetActive(false);
            Obj_MailNullHint.SetActive(true);
        }
	}

	//显示邮件内容
	public void Btn_ShowMailData(){

		Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_MailRewardListSet);

		string mailName = pro_Mail.MailData[SelectMailID].str_1;
		string mailDes = pro_Mail.MailData[SelectMailID].str_2;
		string mailRewardStr = getMailReward(pro_Mail.MailData[SelectMailID].str_3);
		string[] mailRewardList = mailRewardStr.Split ('#');

		Obj_MailName.GetComponent<Text> ().text = mailName;

        //替换内部道具ID变成道具名称
        //获取道具起始位置
        string showDes = mailDes;
        int startNum = mailDes.IndexOf("<ItemID>");
        int endNum = mailDes.IndexOf("</ItemID>");
        Debug.Log("startNum = " + startNum + "endNum = " + endNum);

        if (startNum != -1) {
            string itemID = mailDes.Substring(startNum + 8, endNum - startNum - 8);
            Debug.Log("itemID ====" + itemID);
            string Name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", itemID, "Item_Template");
            showDes = mailDes.Substring(0, startNum) + Name + mailDes.Substring(endNum + 9, mailDes.Length - endNum - 9);
        }

        Obj_MailDes.GetComponent<Text> ().text = showDes;

        if (mailRewardList.Length <= 0) {
            Obj_MailRewardSet.SetActive(false);
        }

		for (int i = 0; i < mailRewardList.Length; i++) {
			//显示奖励
			GameObject mailRewardObj = (GameObject)Instantiate(Obj_MailRewardObj);
			mailRewardObj.transform.SetParent(Obj_MailRewardListSet.transform);
            mailRewardObj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

			string itemID = mailRewardList [i].Split('|')[0];
            string itemNum = mailRewardList[i].Split('|')[1];
            string itemHideStr = mailRewardList[i].Split('|')[2];

			mailRewardObj.GetComponent<UI_CommonItemShow_1>().ItemID = itemID;
			mailRewardObj.GetComponent<UI_CommonItemShow_1>().ItemNum = itemNum;
			mailRewardObj.GetComponent<UI_CommonItemShow_1>().HideStr = itemHideStr;
            mailRewardObj.GetComponent<UI_CommonItemShow_1>().Obj_ItemName.SetActive(false);
		}

        //显示邮件内容
        Obj_ShowMailSet.SetActive(true);
        Obj_MailNullHint.SetActive(false);
	}

	//领取邮箱
	public void Btn_LingQuMail(){

        if (LingQuMailStatus) {
            return;
        }

        LingQuMailStatus = true;

        //判定背包格子
        string mailRewardStr = getMailReward(pro_Mail.MailData[SelectMailID].str_3);
        string[] mailRewardList = mailRewardStr.Split('#');
		int bangNullNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
		if (bangNullNum < mailRewardList.Length) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_301");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("背包已满,请保持背包内有足够的空位！");
            LingQuMailStatus = false;
            return;

		}

        //发送奖励
        /*
        for (int i = 0; i < mailRewardList.Length; i++) {

            string[] rewardStr = mailRewardList[i].Split('|');
            if (rewardStr.Length >= 2) {
                string sendItemID = rewardStr[0];
                string sendItemNum = rewardStr[1];
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(sendItemID, int.Parse(sendItemNum));
            }

        }
        */

        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001043, SelectMailID);


        //重新刷新邮件列表
        //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001040, zhanghaoID);

        //LingQuMailStatus = false;
    }

    public void SendMailReward() {

        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001044, SelectMailID);

        //判定背包格子
        string mailRewardStr = getMailReward(pro_Mail.MailData[SelectMailID].str_3);
        string[] mailRewardList = mailRewardStr.Split('#');

        //获取邮件奖励
        for (int i = 0; i < mailRewardList.Length; i++)
        {
            if (mailRewardList[i].Split('|').Length >= 3) {
                string itemID = mailRewardList[i].Split('|')[0];
                int itemNum = int.Parse(mailRewardList[i].Split('|')[1]);
                string itemHideStr = mailRewardList[i].Split('|')[2];
                if (itemHideStr != "" && itemHideStr != "0")
                {
                    string addHideID = Game_PublicClassVar.Get_function_Rose.ReturnHidePropertyID_GuDing(itemHideStr);
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID, itemNum, "0", 0, addHideID, true, "32");
                }
                else
                {
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID, itemNum, "0", 0, "0", true, "32");
                    Debug.Log("邮箱发送道具:" + itemID);
                }
            }
        }

        //清空当前邮件id
        if (pro_Mail.MailData.ContainsKey(SelectMailID)) {
            pro_Mail.MailData.Remove(SelectMailID);
        }

        SelectMailID = "";

        //刷新邮件
        ShowMail();

    }

	void Ondestroy() {
		Game_PublicClassVar.Get_gameServerObj.Obj_Mail = null;
	}

	//关闭
	public void CloseUI()
	{
		Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
		Destroy(this.gameObject);
	}

    //解密
    private string getMailReward(string rewardStr) {

        //Debug.Log("rewardStr = " + rewardStr);
        //return Game_PublicClassVar.Get_xmlScript.Decrypt(rewardStr);        //老
        return Game_PublicClassVar.Get_xmlScript.Decrypt_DongTai(rewardStr);
    }

}
