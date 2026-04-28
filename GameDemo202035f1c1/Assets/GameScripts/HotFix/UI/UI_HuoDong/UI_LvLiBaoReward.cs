using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_LvLiBaoReward : MonoBehaviour
{

    public string ActiveID;
    public string ActivityStatus;       //是否被领取(0,表示未领取 1,表示已经领取)

    public bool UpdateShowStatus;
    public bool UpdateXuanZhongStatus;
    public GameObject Obj_LvNoShowImg;
    public GameObject Obj_YiJingQuImg;
    public GameObject Obj_BuyIcon_1;
    public GameObject Obj_BuyIcon_2;
    public GameObject Obj_BuyValue;
    public GameObject Obj_ShowSet;
    public GameObject Obj_CommonItemShow;
    public GameObject Obj_LiBaoName;
    public GameObject Obj_LiBaoLv;
    public GameObject Obj_BuyBtn;
    public ObscuredString buyType;
    public ObscuredString buyValue;
    public ObscuredInt buyLv;
	public bool LvShowStatus;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (UpdateShowStatus) {
            UpdateShowStatus = false;
            ShowReward();
        }
	}

    //显示奖励
    public void ShowReward() {

        //清空
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ShowSet);

        //显示礼包名称
        string libaoStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Par_4", "ID", ActiveID, "Activity_Template");
        Obj_LiBaoName.GetComponent<Text>().text = libaoStr;

        //显示礼包等级
        string libaoLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Par_1", "ID", ActiveID, "Activity_Template");
        string langStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Par_1", "ID", ActiveID, "UI_LiBao");
        string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("级开启购买");
        Obj_LiBaoLv.GetComponent<Text>().text = libaoLv + langStr_1;
        buyLv = int.Parse(libaoLv);

        //如果未达到等级,隐藏奖励显示
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv < int.Parse(libaoLv))
        {
            //Obj_LvNoShowImg.SetActive(true);
			//将奖励置为灰色状态

			LvShowStatus = true;

        }

        //显示购买
        string buyStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Par_2", "ID", ActiveID, "Activity_Template");
        buyType = buyStr.Split(',')[0];
        buyValue = buyStr.Split(',')[1];
        Obj_BuyIcon_1.SetActive(false);
        Obj_BuyIcon_2.SetActive(false);

        if (buyValue != "0")
        {
            if (LvShowStatus != true)
            {
                switch (buyType)
                {
                    //金币购买
                    case "1":
                        Obj_BuyIcon_1.SetActive(true);
                        break;

                    //钻石购买
                    case "2":
                        Obj_BuyIcon_2.SetActive(true);
                        break;
                }

                Obj_BuyValue.GetComponent<Text>().text = buyValue;
            }
            else {
                Obj_BuyValue.GetComponent<Text>().text = "";
                Obj_BuyBtn.SetActive(false);
            }
        }
        else {
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("到达等级免费领取");
            Obj_BuyValue.GetComponent<Text>().text = langStr;
        }

        //显示奖励列表
        string itemIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Par_3", "ID", ActiveID, "Activity_Template");
        string[] itemIDStrList = itemIDStr.Split(';');

        for (int i = 0; i < itemIDStrList.Length; i++) {

            string nowItemStr = itemIDStrList[i];
            string itemID = nowItemStr.Split(',')[0];
            string itemNum = nowItemStr.Split(',')[1];

            //实例化显示
            GameObject obj_com = (GameObject)Instantiate(Obj_CommonItemShow);
            obj_com.transform.SetParent(Obj_ShowSet.transform);
            obj_com.transform.localScale = new Vector3(1, 1, 1);
            obj_com.GetComponent<UI_Common_ItemIcon>().ItemID = itemID;
            obj_com.GetComponent<UI_Common_ItemIcon>().NeedItemNum = int.Parse(itemNum);
			if (LvShowStatus) {
				obj_com.GetComponent<UI_Common_ItemIcon> ().IfShowWenHao = true;
			}
            obj_com.GetComponent<UI_Common_ItemIcon>().UpdateStatus = true;
        }

        if (ActivityStatus == "0")
        {
            //未购买
            Obj_YiJingQuImg.SetActive(false);
            Obj_BuyValue.SetActive(true);

            //未购买隐藏购买按钮
            if (!LvShowStatus)
            {
                Obj_BuyBtn.SetActive(true);
            }
            else {
                Obj_BuyBtn.SetActive(false);
            }
        }
        else { 
            //已经购买
            Obj_YiJingQuImg.SetActive(true);
            Obj_BuyIcon_1.SetActive(false);
            Obj_BuyIcon_2.SetActive(false);
            Obj_BuyValue.SetActive(false);
            Obj_BuyBtn.SetActive(false);
        }
    }

    public void Btn_Buy() {

        //如果未达到等级,隐藏奖励显示
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv < buyLv)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_232");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("等级不足,无法购买");
            return;
        }

        //显示奖励列表
        string itemIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Par_3", "ID", ActiveID, "Activity_Template");
        string[] itemIDStrList = itemIDStr.Split(';');

        //获取背包格子
        int bagNullNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
        if (bagNullNum < itemIDStrList.Length)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_233");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包格子不足！");
            return;
        }

        
        if (Game_PublicClassVar.Get_function_Rose.CostReward(buyType, buyValue))
        {
            //发送奖励
            for (int i = 0; i < itemIDStrList.Length; i++)
            {
                string nowItemStr = itemIDStrList[i];
                string itemID = nowItemStr.Split(',')[0];
                string itemNum = nowItemStr.Split(',')[1];

                //发送道具
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID, int.Parse(itemNum), "0", 0, "0", true, "28");
            }

            //记录已经购买
            string saveMeiRi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LvLiBao", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            saveMeiRi = saveMeiRi + ActiveID + ";";
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LvLiBao", saveMeiRi, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            //显示已经购买
            ActivityStatus = "1";
            Obj_YiJingQuImg.SetActive(true);
            Obj_BuyIcon_1.SetActive(false);
            Obj_BuyIcon_2.SetActive(false);
            Obj_BuyValue.SetActive(false);
            Obj_BuyBtn.SetActive(false);

            //更新活动界面的货币
            Game_PublicClassVar.Get_function_UI.HuoDongHuoBiUpdate();

            //发送服务器消息
            //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000116, buyValue + "钻石购买等级礼包ID:" + ActiveID);
            Game_PublicClassVar.Get_function_Rose.ServerMsg_SendMsg(buyValue + "钻石购买等级礼包ID:" + ActiveID);
        }
        else {
            //货币不足
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_234");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("货币不足,无法购买！");
            return;
        }
    }

}
