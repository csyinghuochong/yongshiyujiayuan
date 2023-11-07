using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_DayTaskList : MonoBehaviour
{
    public string dayTaskID;
    public GameObject Obj_dayTaskName;
    public GameObject Obj_dayTaskIcon;
    public GameObject Obj_dayTaskDes;
    public GameObject Obj_dayTaskReward;
    public GameObject Obj_dayTaskValuePro;
    public GameObject Obj_DayTaskRewardSet;
    public GameObject ObJ_DayTaskRewardParSet;
    public GameObject Obj_DayTaskHuoYueDu;
    public GameObject Obj_DayTaskType_1;
    public GameObject Obj_DayTaskType_2;

    private ObscuredInt dayTaskNum;
    private ObscuredInt dayTaskNumMax;
    private ObscuredString dayTaskRewardType;
    private ObscuredInt dayTaskRewardValue;

    // Use this for initialization
    void Start()
    {
        //显示图标
        ObscuredString dayTaskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", dayTaskID, "TaskCountry_Template");

        //显示类型
        Obj_DayTaskType_1.SetActive(false);
        Obj_DayTaskType_2.SetActive(false);
        if (dayTaskID.Substring(0, 1) == "1") {
            Obj_DayTaskType_1.SetActive(true);
        }
        if (dayTaskID.Substring(0, 1) == "2")
        {
            Obj_DayTaskType_2.SetActive(true);
        }

        ObscuredString dayTaskIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskIcon", "ID", dayTaskID, "TaskCountry_Template");

        object obj = Resources.Load("EveryDayTaskIcon/" + dayTaskIcon, typeof(Sprite));
        Sprite taskIcon = obj as Sprite;
        Obj_dayTaskIcon.GetComponent<Image>().sprite = taskIcon;

        string[] dayTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
        string[] dayTaskValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');

        for (int i = 0; i <= dayTaskIDList.Length - 1; i++) {
            if (dayTaskIDList[i] == dayTaskID) {
                dayTaskNum = int.Parse(dayTaskValue[i]);
            }
        }
        dayTaskNumMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", dayTaskID, "TaskCountry_Template"));

        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("进度值");
        if (dayTaskNumMax != 0)
        {
            Obj_dayTaskValuePro.GetComponent<Text>().text = langStr + "：" + "" + dayTaskNum + "/" + dayTaskNumMax + "";
        }
        else {
            Obj_dayTaskValuePro.GetComponent<Text>().text = langStr + "：" + ""  + "1/1" + "";
        }

        //显示描述
        ObscuredString dayTaskDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskDes", "ID", dayTaskID, "TaskCountry_Template");
        Obj_dayTaskDes.GetComponent<Text>().text =  dayTaskDes;

        //显示名称
        ObscuredString dayTaskName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskName", "ID", dayTaskID, "TaskCountry_Template");
        Obj_dayTaskName.GetComponent<Text>().text = dayTaskName;

        //显示活跃度
        ObscuredString everyTaskRewardNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EveryTaskRewardNum", "ID", dayTaskID, "TaskCountry_Template");
        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("活跃度");
        Obj_DayTaskHuoYueDu.GetComponent<Text>().text = langStr + " X " + everyTaskRewardNum;

        //其他奖励显示
        ObscuredString rewardItem = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RewardItem", "ID", dayTaskID, "TaskCountry_Template");
        dayTaskRewardType = rewardItem;

        //清空奖励显示
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(ObJ_DayTaskRewardParSet);

        //显示奖励
        string[] dayTaskRewardTypeList = dayTaskRewardType.ToString().Split(';');
        for (int i = 0; i < dayTaskRewardTypeList.Length; i++) {

            string[] dayTaskRewardList = dayTaskRewardTypeList[i].Split(',');
            if (dayTaskRewardList.Length >= 2) {

                string rewardItemID = dayTaskRewardList[0];
                int rewardItemNum = int.Parse(dayTaskRewardList[1]);

                //金币和经验特殊处理
                switch (rewardItemID) {

                    //金币
                    case "1":
                        rewardItemNum = GetTaskRewardValue(rewardItemID, rewardItemNum);
                        break;

                    //经验
                    case "2":
                        rewardItemNum = GetTaskRewardValue(rewardItemID, rewardItemNum);
                        break;

                }

                //显示奖励
                GameObject rewardObj = (GameObject)Instantiate(Obj_DayTaskRewardSet);
                rewardObj.transform.SetParent(ObJ_DayTaskRewardParSet.transform);
                rewardObj.transform.localScale = new Vector3(1, 1, 1);
                rewardObj.GetComponent<UI_DayTaskRewardSet>().ItemID = rewardItemID;
                rewardObj.GetComponent<UI_DayTaskRewardSet>().ItemNum = rewardItemNum.ToString();
                rewardObj.GetComponent<UI_DayTaskRewardSet>().ShowItem();

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Btn_TaskReward() {

        if (dayTaskNum >= dayTaskNumMax)
        {
            //发送奖励
            string[] dayTaskRewardTypeList = dayTaskRewardType.ToString().Split(';');
            //检测背包空余位置
            if (Game_PublicClassVar.Get_function_Rose.BagNullNum() < dayTaskRewardTypeList.Length) {

                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_152");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_153");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + dayTaskRewardTypeList.Length + langStrHint_2);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包剩余格子数量不足!请预留至少" + dayTaskRewardTypeList.Length +"个位置");
                return;
            }

            for (int i = 0; i < dayTaskRewardTypeList.Length; i++)
            {

                string[] dayTaskRewardList = dayTaskRewardTypeList[i].Split(',');
                if (dayTaskRewardList.Length >= 2)
                {

                    string rewardItemID = dayTaskRewardList[0];
                    int rewardItemNum = int.Parse(dayTaskRewardList[1]);

                    //金币和经验特殊处理
                    switch (rewardItemID)
                    {

                        //金币
                        case "1":
                            rewardItemNum = GetTaskRewardValue(rewardItemID, rewardItemNum);
                            break;

                        //经验
                        case "2":
                            rewardItemNum = GetTaskRewardValue(rewardItemID, rewardItemNum);
                            break;

                    }

                    //发送奖励
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardItemID, rewardItemNum,"0",0,"0",true,"11");

                }
            }

            /*
            switch (dayTaskRewardType)
            {
                //角色经验
                case "1":
                    Game_PublicClassVar.Get_function_Rose.AddExp(dayTaskRewardValue);
                    break;
                //金币
                case "2":
                    Game_PublicClassVar.Get_function_Rose.SendReward("1", dayTaskRewardValue.ToString());
                    break;
                //国家繁荣度
                case "3":
                    Game_PublicClassVar.Get_function_Country.addCoutryExp(dayTaskRewardValue);
                    break;
                //荣誉
                case "4":
                    Game_PublicClassVar.Get_function_Country.AddCountryHonor(dayTaskRewardValue);
                    break;
            }
            */



            //Debug.Log("2");
            //删除任务更新列表
            Destroy(this.gameObject);
            //删除对应任务数据
            string write_dayTaskIDList = "";
            string write_dayTaskValue = "";
            //Debug.Log("4");
            string[] dayTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
            string[] dayTaskValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
            for (int i = 0; i <= dayTaskIDList.Length - 1; i++)
            {
                //实例化任务列表
                if (dayTaskIDList[i] != "" && dayTaskIDList[i] != "0")
                {
                    if (dayTaskIDList[i] != dayTaskID)
                    {
                        write_dayTaskIDList = write_dayTaskIDList + dayTaskIDList[i] + ";";
                        write_dayTaskValue = write_dayTaskValue + dayTaskValue[i] + ";";
                    }
                }
            }
            //Debug.Log("5");
            if (write_dayTaskIDList != "")
            {
                write_dayTaskIDList = write_dayTaskIDList.Substring(0, write_dayTaskIDList.Length - 1);
                write_dayTaskValue = write_dayTaskValue.Substring(0, write_dayTaskValue.Length - 1);
            }
            //Debug.Log("6");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskID", write_dayTaskIDList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskValue", write_dayTaskValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

            //增加活跃点数
            string everyTaskRewardNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EveryTaskRewardNum", "ID", dayTaskID, "TaskCountry_Template");
            string roseHuoYueValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskHuoYueValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            if (roseHuoYueValue == ""|| roseHuoYueValue == null) {
                roseHuoYueValue = "0";
            }

            //
            int writeValue = int.Parse(roseHuoYueValue) + int.Parse(everyTaskRewardNum);
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskHuoYueValue", writeValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

            //刷新界面显示
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseDayTask.GetComponent<UI_DayTask>().ShowHuoYueNum();

        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_384");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("领取奖励条件不足");
        }
    }

    //计算任务的经验和金币值
    public int GetTaskRewardValue(string type,int value) {

        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        int getValue = 0;
        switch (type) {

            //金币
            case "1":
                getValue = 10000 + value * roseLv;
                break;

            //经验
            case "2":
                int addValue = 1000;
                float addpro = 1;
                if (roseLv <= 15) {
                    addpro = roseLv / 15;
                }
                if (addpro >= 2) {
                    addpro = 2;
                }
                getValue = addValue + (int)(value * roseLv * addpro);
                break;
        }

        return getValue;
    }
}