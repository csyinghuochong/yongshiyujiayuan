using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_StorySpeakSet : MonoBehaviour {

    public string GameStoryID;
    public string NpcID;
    public GameObject Obj_RoseSpeakSet;
    public GameObject Obj_NpcSpeakSet;
    public GameObject Obj_BtnShowNextSpeak;
    public GameObject Obj_ChooseQuestSet;
    public GameObject Obj_NpcSpeakText;
    public GameObject Obj_NpcSpeakName;
    public GameObject Obj_RoseSpeakText;
    public GameObject Obj_RoseSpeakName;
    public GameObject Obj_QuestTitleText;                   //问题标题文本
    public GameObject Obj_QuestTrueText;                   //问题,是按钮名字
    public GameObject Obj_QuestFalseText;                   //问题,否按钮名字
    private string isStorySpeakChoose;                //是否触发选择模式
    private int storySpeakTriggerChoose;             //多少次兑换触发问题
 
    private string[] storySpeakChooseText;            //选择说话文本
    private string[] storySpeakTextNpc_True;          //NPC选是说话文本
    private string[] storySpeakTextNpc_False;         //NPC选否说话文本
    private string[] storySpeakTextRose_True;         //玩家选是说话文本
    private string[] storySpeakTextRose_False;        //玩家选否说话文本

    //private int storySpeakTriggerChoose;              //对话几次触发问题
    private string roseName;
    private int NpcSpeakNum;                          //NPC说话次数
    private int RoseSpeakNum;                         //Rose说话次数
    private bool NpcSpeakStatus;                          //NPC说话状态
    private bool RoseSpeakStatus;                         //Rose说话状态
    private int SpeakShowLength;                        //说话的长度
    private float SpeakShowTime;                          //单字显示时间累计
    private bool RoseChooseQuest;                       //表示玩家选择问题的答案,一开始默认为是

	// Use this for initialization
	void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        //默认开启NPC说话状态,关闭Rose说话状态
        //GameStoryID = "2";
        NpcSpeakStatus = true;
        RoseSpeakStatus = false;

        //默认选择答案选项
        RoseChooseQuest = true;

        //获取自身显示的类型
        isStorySpeakChoose = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IsStorySpeakChoose", "ID", GameStoryID, "GameStory_Template");
        storySpeakTriggerChoose = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakTriggerChoose", "ID", GameStoryID, "GameStory_Template"));
        //获取对话文本
        storySpeakChooseText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakChooseText", "ID", GameStoryID, "GameStory_Template").Split(';');
        storySpeakTextNpc_True = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakTextNpc_True", "ID", GameStoryID, "GameStory_Template").Split(';');
        storySpeakTextNpc_False = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakTextNpc_False", "ID", GameStoryID, "GameStory_Template").Split(';');
        storySpeakTextRose_True = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakTextRose_True", "ID", GameStoryID, "GameStory_Template").Split(';');
        storySpeakTextRose_False = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakTextRose_False", "ID", GameStoryID, "GameStory_Template").Split(';');
        roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //Debug.Log("storySpeakTextNpc_True的长度" + storySpeakTextNpc_True.Length);
        //Debug.Log("storySpeakTextRose_True的长度" + storySpeakTextRose_True.Length);
        //获取NPC名称
        //Debug.Log("NpcID = " + NpcID);
        if (NpcID != "" && NpcID != "0")
        {
            string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", NpcID, "Npc_Template");
            Obj_NpcSpeakName.GetComponent<Text>().text = npcName;
            Obj_RoseSpeakName.GetComponent<Text>().text = roseName;
        }


        //Debug.Log(storySpeakTextNpc_True[0] + ";" + storySpeakTextNpc_True[1]);
        //isStorySpeakChoose = "0";
        //Debug.Log(storySpeakTextNpc_True.Length + "***" + storySpeakTextRose_True.Length);
	}
	
	// Update is called once per frame
	void Update () {

        //NPC说话状态开启
        if (NpcSpeakStatus) {
            //Debug.Log("Npc开始说话!");
            //暂时隐藏界面显示
            Obj_RoseSpeakSet.SetActive(false);
            Obj_ChooseQuestSet.SetActive(false);
            Obj_NpcSpeakSet.SetActive(true);
            
            //每隔0.1秒显示出一个文字,表现文字逐个显示的效果
            if (SpeakShowTime >= 0.01f)
            {
                SpeakShowLength = SpeakShowLength + 1;
                SpeakShowTime = 0;
                if (RoseChooseQuest)
                {
                    //文字逐个出现
                    if (SpeakShowLength <= storySpeakTextNpc_True[NpcSpeakNum].Length) {
                        Obj_NpcSpeakText.GetComponent<Text>().text = storySpeakTextNpc_True[NpcSpeakNum].Substring(0, SpeakShowLength);
                    }
                }
                else
                {
                    //文字逐个出现
                    if (SpeakShowLength <= storySpeakTextNpc_False[NpcSpeakNum].Length)
                    {
                        Obj_NpcSpeakText.GetComponent<Text>().text = storySpeakTextNpc_False[NpcSpeakNum].Substring(0, SpeakShowLength);
                    }
                }
            }
            else {
                SpeakShowTime = SpeakShowTime + Time.deltaTime;
            }
        }

        //主角说话状态开启
        if (RoseSpeakStatus) {

            //Debug.Log("主角开始说话!");
            //暂时隐藏界面显示
            Obj_ChooseQuestSet.SetActive(false);
            Obj_NpcSpeakSet.SetActive(false);
            Obj_RoseSpeakSet.SetActive(true);

            //每隔0.05秒显示出一个文字,表现文字逐个显示的效果
            if (SpeakShowTime >= 0.01f)
            {
                SpeakShowLength = SpeakShowLength + 1;
                SpeakShowTime = 0;
                if (RoseChooseQuest)
                {
                    //防止出错
                    if (RoseSpeakNum > storySpeakTextRose_True.Length - 1) {
                        RoseSpeakNum = storySpeakTextRose_True.Length - 1;
                    }
                    //文字逐个出现
                    if (SpeakShowLength <= storySpeakTextRose_True[RoseSpeakNum].Length)
                    {
                        Obj_RoseSpeakText.GetComponent<Text>().text = storySpeakTextRose_True[RoseSpeakNum].Substring(0, SpeakShowLength);
                    }
                }
                else
                {
                    //防止出错
                    if (RoseSpeakNum > storySpeakTextRose_False.Length - 1)
                    {
                        RoseSpeakNum = storySpeakTextRose_False.Length - 1;
                    }
                    //文字逐个出现
                    if (SpeakShowLength <= storySpeakTextRose_False[RoseSpeakNum].Length)
                    {
                        Obj_RoseSpeakText.GetComponent<Text>().text = storySpeakTextRose_False[RoseSpeakNum].Substring(0, SpeakShowLength);
                    }
                }
            }
            else
            {
                SpeakShowTime = SpeakShowTime + Time.deltaTime;
            }
        }
	}

    public void Btn_ShowNextSpeak() {

        //Debug.Log("NpcSpeakNum = " + NpcSpeakNum);
        //Debug.Log("RoseSpeakNum = " + RoseSpeakNum);

        //识别后面没有话将跳出循环
        if (NpcSpeakNum >= storySpeakTextNpc_True.Length)
        {
            exitStorySpeakSet();
            return;
        }

        SpeakShowLength = 0;        //重置显示的文字长度

        if (NpcSpeakStatus)
        {
            if (RoseChooseQuest)
            {
                if (storySpeakTextRose_True[NpcSpeakNum] != "")
                {
                    //切换Rose说话
                    NpcSpeakStatus = false;
                    RoseSpeakStatus = true;
                    NpcSpeakNum = NpcSpeakNum + 1;
                    Obj_NpcSpeakText.GetComponent<Text>().text = "";    //清空文字显示
                }
                else
                {
                    //继续Npc说话
                    NpcSpeakNum = NpcSpeakNum + 1;
                    RoseSpeakNum = RoseSpeakNum + 1;
                    Obj_NpcSpeakText.GetComponent<Text>().text = "";    //清空文字显示
                }
            }
            else {

                if (storySpeakTextRose_False[NpcSpeakNum] != "")
                {
                    //切换Rose说话
                    NpcSpeakStatus = false;
                    RoseSpeakStatus = true;
                    NpcSpeakNum = NpcSpeakNum + 1;
                    Obj_NpcSpeakText.GetComponent<Text>().text = "";    //清空文字显示
                }
                else
                {
                    //继续Npc说话
                    NpcSpeakNum = NpcSpeakNum + 1;
                    RoseSpeakNum = RoseSpeakNum + 1;
                    Obj_NpcSpeakText.GetComponent<Text>().text = "";    //清空文字显示
                }
            }
        }
        else {

            if (RoseSpeakStatus)
            {
                if (storySpeakTextNpc_False[RoseSpeakNum] != "")
                {
                    //切换Npc说话
                    RoseSpeakStatus = false;
                    NpcSpeakStatus = true;
                    if (RoseSpeakNum < storySpeakTextNpc_True.Length-1)
                    {
                        RoseSpeakNum = RoseSpeakNum + 1;
                    }
                    Obj_RoseSpeakText.GetComponent<Text>().text = "";   //清空文字显示
                }
                else {
                    //继续Rose说话
                    if (RoseSpeakNum < storySpeakTextNpc_True.Length-1)
                    {
                        RoseSpeakNum = RoseSpeakNum + 1;
                        NpcSpeakNum = NpcSpeakNum + 1;
                    }
                    Obj_RoseSpeakText.GetComponent<Text>().text = "";   //清空文字显示
                }
            }
        }

        //判定当前是否触发问题
        if (isStorySpeakChoose == "1")
        {
            //当对话达到指定次数后弹出问题
            if (NpcSpeakNum == storySpeakTriggerChoose)
            {
                //屏蔽其他UI
                Obj_ChooseQuestSet.SetActive(true);
                Obj_NpcSpeakSet.SetActive(false);
                Obj_RoseSpeakSet.SetActive(false);
                NpcSpeakStatus = false;
                RoseSpeakStatus = false;
                //显示问题相关文本
                Obj_QuestTitleText.GetComponent<Text>().text = storySpeakChooseText[0];
                Obj_QuestTrueText.GetComponent<Text>().text = storySpeakChooseText[1];
                Obj_QuestFalseText.GetComponent<Text>().text = storySpeakChooseText[2];
                return;
            }
        }

        //确保最后一个对话,如果自己没有对话则强制关闭对话界面
        if (NpcSpeakNum >= storySpeakTextNpc_True.Length)
        {
            if (RoseChooseQuest)
            {
                if (storySpeakTextRose_True[NpcSpeakNum - 1] == "")
                {
                    exitStorySpeakSet();
                    //return;
                }
            }
            else {
                if (storySpeakTextRose_False[NpcSpeakNum - 1] == "")
                {
                    exitStorySpeakSet();
                    //return;
                }
            }
        }

        //Debug.Log("NpcSpeakNum = " + NpcSpeakNum + "    RoseSpeakNum = " + RoseSpeakNum);
    }

    //退出此UI时调用
    private void exitStorySpeakSet() {
        //显示主界面UI
        //Debug.Log("开启显示");
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcTaskSet.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(true);
        //主角故事对话状态关闭
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStorySpeakStatus = false;
        //更新主角
        Game_PublicClassVar.Get_function_Rose.UpdataRoseStoryStatus();
        //发送奖励
        sendStorySpeakReward();
        //删除自己
        Destroy(this.gameObject);
    }

    //问题回答——是
    public void Btn_QuestTrue() {

        RoseChooseQuest = true;
        
        //屏蔽其他UI
        Obj_ChooseQuestSet.SetActive(false);
        Obj_NpcSpeakSet.SetActive(true);
        Obj_RoseSpeakSet.SetActive(false);
        RoseSpeakStatus = false;
        NpcSpeakStatus = true;
        
        isStorySpeakChoose = "0";       //设置对话触发事件为false
        NpcSpeakNum = NpcSpeakNum - 1;  //因为下面要重新调用一次按钮,并不是这按,所以累计次数需要-1
        if (storySpeakTextRose_True[RoseSpeakNum] == "") {
            RoseSpeakNum = RoseSpeakNum + 1;
        }

        Btn_ShowNextSpeak();

    }
    //问题回答——否
    public void Btn_QuestFalse() {

        RoseChooseQuest = false;

        //屏蔽其他UI
        Obj_ChooseQuestSet.SetActive(false);
        Obj_NpcSpeakSet.SetActive(true);
        Obj_RoseSpeakSet.SetActive(false);
        RoseSpeakStatus = false;
        NpcSpeakStatus = true;

        isStorySpeakChoose = "0";       //设置对话触发事件为false
        NpcSpeakNum = NpcSpeakNum - 1;  //因为下面要重新调用一次按钮,并不是这按,所以累计次数需要-1
        if (storySpeakTextRose_False[RoseSpeakNum] == ""){
            RoseSpeakNum = RoseSpeakNum + 1;
        }

        Btn_ShowNextSpeak();
    
    }

    //发送对话奖励
    private void sendStorySpeakReward() {

        if (RoseChooseQuest)
        {
            //获得奖励  StorySpeakReward_True
            string[] storySpeakReward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakReward_True", "ID", GameStoryID, "GameStory_Template").Split(';');
            if (storySpeakReward[0] != "0") { 
                for(int i = 0;i<=storySpeakReward.Length-1;i++){
                    //Debug.Log("storySpeakReward[i] = " + storySpeakReward[i]);
                    string[] rewardID = storySpeakReward[i].Split(',');
                    //Debug.Log("发送奖励ID：" + rewardID[0] + "   数量：" + rewardID[1]);
                    //发送奖励
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardID[0], int.Parse(rewardID[1]),"0",0,"0",true,"25");
                }
            }

            //查询是否有任务接取
            string taskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakTask_True", "ID", GameStoryID, "GameStory_Template");
            if (taskID != "0")
            {
                Game_PublicClassVar.Get_function_Task.GetTask(taskID);
            }
        }
        else {

            //获得奖励  StorySpeakReward_True
            string[] storySpeakReward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakReward_False", "ID", GameStoryID, "GameStory_Template").Split(';');
            if (storySpeakReward[0] != "0")
            {
                for (int i = 0; i <= storySpeakReward.Length - 1; i++)
                {
                    string[] rewardID = storySpeakReward[i].Split(',');
                    //发送奖励
                    //Debug.Log("发送奖励ID：" + rewardID[0] + "   数量：" + rewardID[1]);
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardID[0], int.Parse(rewardID[1]), "0", 0, "0", true, "25");
                }
            }

            //查询是否有任务接取
            string taskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakTask_False", "ID", GameStoryID, "GameStory_Template");
            if (taskID != "0")
            {
                Game_PublicClassVar.Get_function_Task.GetTask(taskID);
            }

        }
    }
}
