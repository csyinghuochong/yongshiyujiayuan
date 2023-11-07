using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UI_PaiHang : MonoBehaviour
{
    public bool ReceiveDataStatus;
    public GameObject Obj_PaiHangSet;
    public Pro_PaiHang pro_ProList;                     //实力排行榜数据

    public GameObject Obj_WeiLianJieImg;
    public GameObject Obj_LinkHintText;
    public GameObject Obj_ShiLiStr;
    public GameObject Obj_TimeStr;
    public GameObject Obj_TimeNewStr;
    public GameObject Obj_PaiHangTableText;
    public float UpdateDataTime;
    public GameObject Obj_PaiHangUpdateTimeHint;
    private bool UpdateRankStatus;
    private float updateSecSum;
    public GameObject Obj_PaiHangListSet;
    private bool sendAgainStatus;
    public GameObject Obj_PaiHangShow;
    public GameObject Obj_PaiHangReward;
    public GameObject Obj_PaiHangRewardEvery;
    public GameObject Obj_PaiHangHeQuReward;
    public GameObject Obj_PaiHangHeQuTianTiReward;


    public GameObject Obj_PaiHangRankValue;         //自身排名
    public GameObject Obj_PaiHangRewardTime;

    public GameObject Obj_RewardEveryDayPaiHang_RankValue;
    public GameObject Obj_RewardPetPaiHang_RankValue;
    public GameObject Obj_HeQuPaiHang_RankValue;
    public GameObject Obj_HeQuPetPaiHang_RankValue;

    private string zhanghaoID;

    public GameObject Obj_PaiHangListShow;                  //实力排行榜列表Obj

    //标题
    public GameObject Obj_Title_PaiHang;
    public GameObject Obj_Title_DaMiJing;

    //大秘境相关
    public Pro_PaiHang_DaMiJing pro_PaiHang_DaMiJing;       //大秘境排行数据
    public GameObject Obj_PaiHangDaMiJingListShow;          //大秘境排行榜列表Obj
    public bool UpdateDaMiJingStatus;

    //宠物排行相关
    public GameObject Obj_PetTianTiSet;                     //宠物天梯

    //奖励发放时间
    public GameObject Obj_SendRewardTime;
    public GameObject Obj_SendHeQuRewardTime;
    public GameObject Obj_SendHeQuPetRewardTime;

    //排名奖励按钮
    public GameObject Obj_Btn_RankReward;
    public GameObject Obj_Btn_RankRewardHeQu;
    public GameObject Obj_Btn_RankRewardHeQuTianTi;
    // Use this for initialization
    void Start () {

        //打开通用UI
        Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject, "805");

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_PaiHangSet);

        Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang = this.gameObject;
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PaiHangListSet);

        //判断是否连接网络
        if (Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus)
        {
            Obj_WeiLianJieImg.SetActive(false);
            Obj_LinkHintText.SetActive(true);
        }
        else {
            Obj_WeiLianJieImg.SetActive(true);
            Obj_LinkHintText.SetActive(false);
        }
        
        //默认选择综合排行榜
        zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //Debug.Log("发送时间 = "+ DateTime.Now.ToString());
        Btn_ZongHePaiHang();

        //显示自身的实力值
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("当前自身的实力值");
        string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("点");

        Obj_ShiLiStr.GetComponent<Text>().text = langStr + ":" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ShiLiValue + langStr_2;
        Obj_TimeStr.SetActive(false);

        //默认显示
        //ShowRewardRank("2");

        //发送自身实力值
        string[] sendStrList_2 = new string[] { zhanghaoID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ShiLiValue.ToString(),"",""};

        //发送账号ID
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001030, sendStrList_2);

        //获取自身排名
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001032, zhanghaoID);

        //获取新战区时间
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001033, zhanghaoID);

        //获取显示排名奖励还是合区奖励
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001037, zhanghaoID);

        //请求天梯排名
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001504, "");

        //请求排行榜显示领取时间
        ///10001037


    }

    // Update is called once per frame
    void Update () {
        //接收到服务器数据更新数据
        if (ReceiveDataStatus)
        {
            Debug.Log("发送时间222 = " + DateTime.Now.ToString());
            ReceiveDataStatus = false;
            showPaiHang();
        }

        if (UpdateDaMiJingStatus) {
            UpdateDaMiJingStatus = false;
            showPaiHang_DaMiJing();
        }

        UpdateDataTime = UpdateDataTime - Time.deltaTime;
        if (UpdateRankStatus)
        {
            updateSecSum = updateSecSum + Time.deltaTime;
            if (updateSecSum >= 1)
            {
                updateSecSum = 0;

                int showMin = (int)(UpdateDataTime / 60);
                int showSec = (int)(UpdateDataTime % 60);
                string showTime = "";
                if (showMin == 0)
                {
                    showTime = showSec + "秒";
                }
                else
                {
                    showTime = showMin + "分" + showSec + "秒";
                }
                int lastTimeUpdate = (int)((1800 - UpdateDataTime) / 60);
                if (lastTimeUpdate >= 1)
                {
                    Obj_PaiHangUpdateTimeHint.GetComponent<Text>().text = "排行数据于" + lastTimeUpdate + "分钟前刷新,下次刷新时间:" + showTime;
                }
                else {
                    int lastTimeUpdateSec = (int)((1800 - UpdateDataTime) % 60);
                    Obj_PaiHangUpdateTimeHint.GetComponent<Text>().text = "排行数据于" + lastTimeUpdateSec + "秒前刷新,下次刷新时间:" + showTime;
                }
                

                if (sendAgainStatus == false) {
                    sendAgainStatus = true;
                    if (UpdateDataTime < 0)
                    {
                        //发送自身实力值
                        string[] sendStrList_2 = new string[] { zhanghaoID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ShiLiValue.ToString(), "", "" };
                        //发送账号ID
                        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001030, sendStrList_2);
                    }
                }
            }
        }
        else {
            Obj_PaiHangUpdateTimeHint.GetComponent<Text>().text = "";
        }

    }

    private void clrearnTitle() {
        Obj_Title_PaiHang.SetActive(false);
        Obj_Title_DaMiJing.SetActive(false);
    }

    public void showPaiHang() {

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PaiHangListSet);
        clrearnTitle();
        Obj_Title_PaiHang.SetActive(true);

        //取排名前10
        for (int i = 1; i <= pro_ProList.PaiHangData.Count; i++) {
            //从1-10名顺序取玩家信息
            //如果玩家的ID为0则表示没有玩家
            if (pro_ProList.PaiHangData.ContainsKey(i.ToString()))
            {
                //显示对应玩家
                Pro_PaiHangStrList pro_PaiHangStrList = pro_ProList.PaiHangData[i.ToString()];
                GameObject paiHangListShow = (GameObject)Instantiate(Obj_PaiHangListShow);
                paiHangListShow.transform.SetParent(Obj_PaiHangListSet.transform);
                paiHangListShow.GetComponent<UI_PaiHangListShow>().rosePaiHangValue = i.ToString();
                paiHangListShow.GetComponent<UI_PaiHangListShow>().roseEquipStr = pro_PaiHangStrList.str_1;
                paiHangListShow.GetComponent<UI_PaiHangListShow>().roseEquipHideStr = pro_PaiHangStrList.str_2;
                paiHangListShow.GetComponent<UI_PaiHangListShow>().roseName = pro_PaiHangStrList.str_3;
                paiHangListShow.GetComponent<UI_PaiHangListShow>().roseLv = pro_PaiHangStrList.str_4;
                paiHangListShow.GetComponent<UI_PaiHangListShow>().roseShiLiValue = pro_PaiHangStrList.str_5;
                paiHangListShow.GetComponent<UI_PaiHangListShow>().roseOcc = pro_PaiHangStrList.str_6;
				paiHangListShow.GetComponent<UI_PaiHangListShow>().Server_PetDataStr = pro_PaiHangStrList.str_7;
                paiHangListShow.GetComponent<UI_PaiHangListShow>().roseNowYanSeID = pro_PaiHangStrList.NowYanSeID;
                paiHangListShow.GetComponent<UI_PaiHangListShow>().roseNowNowYanSeHairID = pro_PaiHangStrList.NowNowYanSeHairID;
                //刷新显示
                paiHangListShow.GetComponent<UI_PaiHangListShow>().ShowPlayListData();
                paiHangListShow.transform.localScale = new Vector3(1, 1, 1);
                //Debug.Log();

            }
            else {

                //显示空
                GameObject paiHangListShow = (GameObject)Instantiate(Obj_PaiHangListShow);
                paiHangListShow.transform.SetParent(Obj_PaiHangListSet.transform);

                //刷新显示
                paiHangListShow.GetComponent<UI_PaiHangListShow>().IfNullStatus = true;
                paiHangListShow.GetComponent<UI_PaiHangListShow>().rosePaiHangValue = i.ToString();
                paiHangListShow.GetComponent<UI_PaiHangListShow>().ShowPlayListData();
            }
        }

        //显示时间
        if (pro_ProList.UpdateTime == "" || pro_ProList.UpdateTime == null)
        {
            UpdateDataTime = 0;
        }
        else {
            UpdateDataTime = float.Parse(pro_ProList.UpdateTime);
        }

        UpdateRankStatus = true;

        /*
        Obj_TimeStr.GetComponent<Text>().text = "数据来源:" + pro_ProList.ShowServerName + "(" + pro_ProList.ShowTimeMin.Month + "月" + pro_ProList.ShowTimeMin.Day + "日-" + pro_ProList.ShowTimeMax.Month + "月" + (pro_ProList.ShowTimeMax.Day).ToString() + "日" + ")";
        Obj_TimeStr.SetActive(true);
        Debug.Log("发送时间333 = " + DateTime.Now.ToString());
        */

        //Obj_LinkHintText.GetComponent<Text>().text = "";

        //显示
        //Obj_PaiHangRewardTime.GetComponent<Text>().text = pro_ProList.ShowTimeMin.AddDays(7).Month + "月" + pro_ProList.ShowTimeMin.AddDays(7).Day +"日0:00点发送排行奖励,邮件查收!";

        //Debug.Log("接受排行榜数据3333" + DateTime.Now.ToString());
    }



    public void showPaiHang_DaMiJing()
    {

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PaiHangListSet);
        clrearnTitle();
        Obj_Title_DaMiJing.SetActive(true);

        if (Obj_PaiHangDaMiJingListShow != null)
        {
            //取排名前10
            for (int i = 1; i <= pro_PaiHang_DaMiJing.PaiHangData.Count; i++)
            {
                //从1-10名顺序取玩家信息
                //如果玩家的ID为0则表示没有玩家
                if (pro_PaiHang_DaMiJing.PaiHangData.ContainsKey(i.ToString()))
                {
                    //显示对应玩家
                    Pro_PaiHangDaMiJingStrList pro_PaiHangStrList = pro_PaiHang_DaMiJing.PaiHangData[i.ToString()];
                    GameObject paiHangListShow = (GameObject)Instantiate(Obj_PaiHangDaMiJingListShow);
                    paiHangListShow.transform.SetParent(Obj_PaiHangListSet.transform);
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().rosePaiHangValue = i.ToString();
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().roseEquipStr = pro_PaiHangStrList.str_1;
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().roseEquipHideStr = pro_PaiHangStrList.str_2;
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().roseName = pro_PaiHangStrList.str_3;
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().roseLv = pro_PaiHangStrList.str_4;
                    //paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().roseShiLiValue = pro_PaiHangStrList.str_5;
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().roseOcc = pro_PaiHangStrList.str_6;
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().Server_PetDataStr = pro_PaiHangStrList.str_7;
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().roseDaMiJingLvValue = pro_PaiHangStrList.str_8;
                    if (pro_PaiHangStrList.str_9 == "" || pro_PaiHangStrList.str_9 == null)
                    {
                        paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().roseDaMiJingLvTime = 0;
                    }
                    else
                    {
                        paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().roseDaMiJingLvTime = float.Parse(pro_PaiHangStrList.str_9);
                    }
                    //刷新显示
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().ShowPlayListData();
                    paiHangListShow.transform.localScale = new Vector3(1, 1, 1);
                    //Debug.Log();

                }
                else
                {

                    //显示空
                    GameObject paiHangListShow = (GameObject)Instantiate(Obj_PaiHangDaMiJingListShow);
                    paiHangListShow.transform.SetParent(Obj_PaiHangListSet.transform);

                    //刷新显示
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().IfNullStatus = true;
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().rosePaiHangValue = i.ToString();
                    paiHangListShow.GetComponent<UI_PaiHangListShow_DaMiJing>().ShowPlayListData();
                }
            }
        }

        //显示时间
        /*
        Obj_TimeStr.GetComponent<Text>().text = "数据来源:" + pro_ProList.ShowServerName + "(" + pro_ProList.ShowTimeMin.Month + "月" + pro_ProList.ShowTimeMin.Day + "日-" + pro_ProList.ShowTimeMax.Month + "月" + (pro_ProList.ShowTimeMax.Day).ToString() + "日" + ")";
        Obj_TimeStr.SetActive(true);
        Debug.Log("发送时间333 = " + DateTime.Now.ToString());
        */

        Obj_LinkHintText.GetComponent<Text>().text = "";

        //显示
        Obj_PaiHangRewardTime.GetComponent<Text>().text = pro_ProList.ShowTimeMin.AddDays(7).Month + "月" + pro_ProList.ShowTimeMin.AddDays(7).Day + "日0:00点发送排行奖励,邮件查收!";
    }


    public void SendShiLiValue() {
        //发送链接请求
        int shiliValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ShiLiValue;
        //Debug.Log("shiliValue = " + shiliValue);
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] sendStrList = new string[] { zhanghaoID, shiliValue.ToString(), "", "" };
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001030, sendStrList);
    }

    void Ondestroy() {
        Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang = null;
    }

    //关闭
    public void CloseUI()
    {
        //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RosePaiHangBang_Status = false;
		Destroy(this.gameObject);
    }

    //战士排行榜
    public void Btn_ZhanShiPaiHang() {

        ClearnObj();
        Obj_PaiHangShow.SetActive(true);

        string[] sendStrList_1 = new string[] { zhanghaoID, "1", "", "" };
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001031, sendStrList_1);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_219");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("战士排行榜已刷新！");
        Obj_PaiHangTableText.GetComponent<Text>().text = "战士排行榜";
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PaiHangListSet);
        Obj_LinkHintText.GetComponent<Text>().text = "数据加载中,请稍等...";

    }

    //法师排行榜
    public void Btn_FaShiPaiHang(){

        ClearnObj();
        Obj_PaiHangShow.SetActive(true);

        string[] sendStrList_1 = new string[] { zhanghaoID, "2", "", "" };
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001031, sendStrList_1);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("法师排行榜已刷新！");
        Obj_PaiHangTableText.GetComponent<Text>().text = "法师排行榜";
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PaiHangListSet);
        Obj_LinkHintText.GetComponent<Text>().text = "数据加载中,请稍等...";
    }

    //综合排行榜
    public void Btn_ZongHePaiHang()
    {
        ClearnObj();
        Obj_PaiHangShow.SetActive(true);

        string[] sendStrList_1 = new string[] { zhanghaoID, "0", "", "" };
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001031, sendStrList_1);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("综合排行榜已刷新！");
        Obj_PaiHangTableText.GetComponent<Text>().text = "综合排行榜";
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PaiHangListSet);
        Obj_LinkHintText.GetComponent<Text>().text = "数据加载中,请稍等...";
    }

    //综合排行榜
    public void Btn_DaMiJingPaiHang()
    {
        ClearnObj();
        Obj_PaiHangShow.SetActive(true);

        string[] sendStrList_1 = new string[] { zhanghaoID, "0", "", "" };
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001035, sendStrList_1);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("大秘境排行榜已刷新！");
        Obj_PaiHangTableText.GetComponent<Text>().text = "大秘境排行榜";
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PaiHangListSet);
        Obj_LinkHintText.GetComponent<Text>().text = "数据加载中,请稍等...";
    }

    //点击排名
    public void Btn_RankReward() {
        //ClearnObj();
        Obj_PaiHangReward.SetActive(true);
    }

    //点击每日奖励
    public void Btn_RankRewardEveryDay() {

        Obj_PaiHangRewardEvery.SetActive(true);
    }

    //点击合区排名奖励
    public void Btn_RankHeQuReward()
    {
        Obj_PaiHangHeQuReward.SetActive(true);
    }

    //点击合区排名奖励
    public void Btn_RankHeQuTianTiReward()
    {
        Obj_PaiHangHeQuTianTiReward.SetActive(true);
    }

    //点击合区排名奖励
    public void Close_RankHeQuReward()
    {
        Obj_PaiHangHeQuReward.SetActive(false);
    }

    //点击合区排名奖励
    public void Close_RankHeQuTianTiReward()
    {
        Obj_PaiHangHeQuTianTiReward.SetActive(false);
    }

    //点击每日奖励
    public void Close_RankRewardEveryDay()
    {

        Obj_PaiHangRewardEvery.SetActive(false);

    }

    //清理
    public void ClearnObj() {
        Obj_PaiHangShow.SetActive(false);
        Obj_PaiHangReward.SetActive(false);
        Obj_PetTianTiSet.SetActive(false);
    }

    //关闭奖励提示
    public void Close_Reward() {
        Obj_PaiHangReward.SetActive(false);
    }

    public void Btn_OpenPetTianTi() {
        ClearnObj();
        Obj_PetTianTiSet.SetActive(true);
    }

    //显示排行奖励类型
    public void ShowRewardRank(string type,string startTime,string hequTime) {

        Obj_Btn_RankReward.SetActive(false);
        Obj_Btn_RankRewardHeQu.SetActive(false);
        Obj_Btn_RankRewardHeQuTianTi.SetActive(false);
        //空表示显示默认,0表示显示开区排行,1表示显示合区排行,2表示都不显示
        //显示类型
        switch (type) {
            case "0":
                Obj_Btn_RankReward.SetActive(true);
                break;

            case "1":
                Obj_Btn_RankRewardHeQu.SetActive(true);
                Obj_Btn_RankRewardHeQuTianTi.SetActive(true);
                break;

            case "2":
                break;
        }

        string lang_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_455");
        string lang_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_456");

        string startShowStr = "";
        DateTime date = new DateTime();
        if (startTime != "" && startTime != "0" && startTime != null) {
            if (startTime.Length > 10) {
                startTime = startTime.Substring(0, 10);
            }
            date = Game_PublicClassVar.Get_wwwSet.GetTime(startTime);
            if (date != null) {
                startShowStr = date.AddDays(7).Month + lang_2 + date.AddDays(7).Day + lang_1;
            }
        }
        //显示
        Obj_SendRewardTime.GetComponent<Text>().text = startShowStr;

        string hequShowStr = "";
        date = new DateTime();
        if (hequTime != "" && hequTime != "0" && hequTime != null)
        {
            if (hequTime.Length > 10)
            {
                hequTime = hequTime.Substring(0, 10);
            }
            date = Game_PublicClassVar.Get_wwwSet.GetTime(hequTime);
            if (date != null)
            {
                startShowStr = date.AddDays(7).Month + lang_2 + date.AddDays(7).Day + lang_1;
            }
        }
        //显示
        Obj_SendHeQuRewardTime.GetComponent<Text>().text = startShowStr;
        Obj_SendHeQuPetRewardTime.GetComponent<Text>().text = startShowStr;
    }
}
