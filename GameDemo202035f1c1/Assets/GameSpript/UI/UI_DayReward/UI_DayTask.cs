using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_DayTask : MonoBehaviour
{
    public GameObject Obj_DayTaskSet;
    public GameObject Obj_DayTaskListSet;
    public GameObject Obj_DayTaskList;
    private float taskNum;
    private string[] dayTaskIDList;
    private string[] dayTaskValue;

    public GameObject Obj_HuoYueValue;
    public GameObject Obj_HuoYueExpPro;
    public GameObject Obj_HuoYueRewardHint;
    private GameObject HuoYueRewardHintObj;

    public GameObject[] Obj_RewardLingQu;

    public GameObject[] Obj_RewardOpen;

    public GameObject Obj_BaoLvShowSet;
    public GameObject Obj_DayTaskShowSet;
    public GameObject Obj_DayLiXianJingYan;
    public GameObject Obj_OlineHuoDongSet;

    public bool IfNoInit;

    // Use this for initialization
    void Start()
    {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_DayTaskSet);

        //清空UI
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_DayTaskListSet);

        dayTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
        dayTaskValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
        for (int i = 0; i <= dayTaskIDList.Length - 1; i++) { 
            //实例化任务列表
            if (dayTaskIDList[i] != "" && dayTaskIDList[i] != "0") {
                GameObject dayTaskObj = (GameObject)Instantiate(Obj_DayTaskList);
                dayTaskObj.transform.SetParent(Obj_DayTaskListSet.transform);
                dayTaskObj.transform.localScale = new Vector3(1, 1, 1);
                dayTaskObj.GetComponent<UI_DayTaskList>().dayTaskID = dayTaskIDList[i];

            }
        }

        if (IfNoInit == false)
        {
            //初始化显示
            Btn_EveryActive();

            //展示活跃点数
            ShowHuoYueNum();

            //展示宝箱状态
            showRewardChest();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Btn_CloseUI() {
        //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenErveryDayTask();
        Destroy(this.gameObject);
    }

    public void ShowHuoYueNum() {
        //读取活跃点数
        ObscuredString dayTaskHuoYueValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskHuoYueValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (dayTaskHuoYueValue == "" || dayTaskHuoYueValue == null)
        {
            dayTaskHuoYueValue = "0";
        }
        Obj_HuoYueValue.GetComponent<Text>().text = dayTaskHuoYueValue;
        //显示进度条
        float nowPro = float.Parse(dayTaskHuoYueValue) / 120;
        Obj_HuoYueExpPro.GetComponent<Image>().fillAmount = nowPro;
    }

    //展示宝箱
    public void showRewardChest() {

        for (int i = 0; i < Obj_RewardLingQu.Length; i++) {
            ObscuredString nowRewardID = "30";
            switch (i) {
                case 0:
                    nowRewardID = "30";
                    break;

                case 1:
                    nowRewardID = "60";
                    break;

                case 2:
                    nowRewardID = "90";
                    break;

                case 3:
                    nowRewardID = "120";
                    break;

            }

            //检测奖励是否已经领取
            string dayTaskCommonHuoYueRewardID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskCommonHuoYueRewardID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            if (dayTaskCommonHuoYueRewardID.Contains(nowRewardID))
            {
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("奖励已领取,请勿重新领取！");
                Obj_RewardLingQu[i].SetActive(false);
                Obj_RewardOpen[i].SetActive(true);
            }
            else {
                Obj_RewardLingQu[i].SetActive(true);
                Obj_RewardOpen[i].SetActive(false);
            }


        }



    }

    public void Btn_Rearwd(string huoyueDuValue) {

        bool ifShowTips = true;
        ObscuredString dayTaskCommonHuoYueRewardID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskCommonHuoYueRewardID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //奖励
        ObscuredString rewardStr = "";
        ObscuredString rewardTitle = "";
        ObscuredString rewardDes = "";
        ObscuredString writeRewardID = dayTaskCommonHuoYueRewardID + huoyueDuValue + ";";

        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("点活跃度奖励");

        switch (huoyueDuValue) {

            //30活跃度
            case "30":

                rewardStr = "10010041,5;4,2;10010085,5";
                rewardTitle = huoyueDuValue + langStr;

                break;

            //60活跃度
            case "60":

                rewardStr = "10010041,5;4,2;10000024,1;10000060,1";
                rewardTitle = huoyueDuValue + langStr;

                break;

            //90活跃度
            case "90":

                rewardStr = "10010041,5;4,3;10000016,1;10000060,1";
                rewardTitle = huoyueDuValue + langStr;

                break;

            //120活跃度
            case "120":

                rewardStr = "10010041,5;4,5;10000016,1;10010083,1";
                rewardTitle = huoyueDuValue + langStr;

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

        if (huoyueDuValue == "120") {

            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("概率获得神兽碎片");
            rewardDes = rewardDes + langStr_1;
        }

        //判断是展示Tips还是领取奖励
        ObscuredString dayTaskHuoYueValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskHuoYueValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (dayTaskHuoYueValue == "" || dayTaskHuoYueValue == null)
        {
            dayTaskHuoYueValue = "0";
        }
        if (int.Parse(dayTaskHuoYueValue) >= int.Parse(huoyueDuValue)) {
            //检测奖励是否已经领取

            if (dayTaskCommonHuoYueRewardID.ToString().Contains(huoyueDuValue))
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_154");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("奖励已领取,请勿重新领取！");
                return;
            }
            else {
                ifShowTips = false;
            }
        }

        if (ifShowTips)
        {
            //展示Tips
            HuoYueRewardHintObj = (GameObject)Instantiate(Obj_HuoYueRewardHint);
            HuoYueRewardHintObj.transform.SetParent(this.transform);
            HuoYueRewardHintObj.transform.localScale = new Vector3(1, 1, 1);
            HuoYueRewardHintObj.transform.position = new Vector3(Input.mousePosition.x-220, Input.mousePosition.y-50);
            HuoYueRewardHintObj.GetComponent<UI_DayTaskRearwdHint>().Obj_DayTaskRewardTitleHint.GetComponent<Text>().text = rewardTitle;
            HuoYueRewardHintObj.GetComponent<UI_DayTaskRearwdHint>().Obj_DayTaskRewardHint.GetComponent<Text>().text = rewardDes;
        }
        else {

            //检测背包格子
            if (Game_PublicClassVar.Get_function_Rose.BagNullNum() < rewardStrList.Length) {
                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_156");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_157");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + rewardStrList.Length + langStrHint_2);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包空间不足！请预留至少" + rewardStrList.Length + "个位置领取奖励");
                return;
            }

            //领取奖励,并记录
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskCommonHuoYueRewardID", writeRewardID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

            //发送奖励
            for (int i = 0; i < rewardStrList.Length; i++) {
                string[] sendRewardList = rewardStrList[i].Split(',');
                if (sendRewardList.Length >= 2) {
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(sendRewardList[0], int.Parse(sendRewardList[1]),"0",0,"0",true,"13");
                }
            }

            //有一定概率奖励神兽碎片,20%概率
            if (huoyueDuValue == "120") {
                if (Random.value <= 0.2f) {
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010087", 1,"0",0,"0",true,"14");
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_158");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("天降幸运！恭喜你完成活跃任务时获得了神兽碎片！");
                    string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "福气降临！"+"恭喜玩家" + roseName + "完成活跃任务时获得了神兽碎片！");

                    Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
                    comStr_4.str_1 = "9";
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010191, comStr_4);
                }
            }

            //展示宝箱状态
            showRewardChest();
        }
    }


    //点击活跃
    public void Btn_EveryActive() {
        clearnShow();
        Obj_DayTaskShowSet.SetActive(true);
    }


    //超爽大爆
    public void Btn_BaoLvShow() {
        clearnShow();
        Obj_BaoLvShowSet.SetActive(true);
    }

    //离线经验
    public void Btn_LiXianJingYan()
    {
        clearnShow();
        Obj_DayLiXianJingYan.SetActive(true);
    }

    //离线经验
    public void Btn_OlineHuoDong()
    {
        clearnShow();
        Obj_OlineHuoDongSet.SetActive(true);
    }

    //清理显示
    private void clearnShow() {
        Obj_DayTaskShowSet.SetActive(false);
        Obj_BaoLvShowSet.SetActive(false);
        Obj_DayLiXianJingYan.SetActive(false);
        Obj_OlineHuoDongSet.SetActive(false);
    }

}