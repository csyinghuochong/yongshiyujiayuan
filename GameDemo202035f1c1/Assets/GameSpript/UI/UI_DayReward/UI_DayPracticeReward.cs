using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_DayPracticeReward : MonoBehaviour {

    public GameObject Obj_ExpText;
    public GameObject Obj_ExpNum;
    public GameObject Obj_ExpTime;
    public GameObject Obj_ExpBtnText;
    public GameObject Obj_GoldText;
    public GameObject Obj_GoldNum;
    public GameObject Obj_GoldTime;
    public GameObject Obj_GoldBtnText;

    private string expNumMax;
    private string expNum;
    private string expTime;
    private int expValue;
    private string goldNumMax;
    private string goldNum;
    private string goldTime;
    private int goldValue;

    private float secUpdataTimeSum; //秒更新时间累计
    private float getTime;
	// Use this for initialization
	void Start () {
        getTime = 1800.0f; //半个小时领取一次
        UpdataShowData();
	}
	
	// Update is called once per frame
	void Update () {
        //每秒执行一次
        secUpdataTimeSum = secUpdataTimeSum + Time.deltaTime;
        if (secUpdataTimeSum >= 1)
        {
            //写入更新数据
            expTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ExpTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            goldTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_GoldTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            updataTime();
            //清空值
            secUpdataTimeSum = 0;
        }
	}


    void UpdataShowData() { 
        //获取领取次数
        expNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "Day_ExpNum", "GameMainValue");
        expNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ExpNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        goldNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "Day_GoldNum", "GameMainValue");
        goldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

        if (expNum == "0")
        {
            expTime = getTime.ToString();
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ExpTime", expTime, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
        }
        else {
            expTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ExpTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        }

        if (goldNum == "0")
        {
            goldTime = getTime.ToString();
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_GoldTime", goldTime, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
        }
        else {
            goldTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_GoldTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        }
        
        //获取经验值
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        float expPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "Day_ExpPro", "GameMainValue"));
        float goldPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "Day_GoldPro", "GameMainValue"));
        float roseExpPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpPro", "RoseLv", roseLv.ToString(), "RoseExp_Template"));
        float roseGoldPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseGoldPro", "RoseLv", roseLv.ToString(), "RoseExp_Template"));

        expValue = (int)(expPro * roseExpPro);
        goldValue = (int)(goldPro * roseGoldPro);

        //展示数据
        Obj_ExpText.GetComponent<Text>().text = "当前可提取修炼经验：" + expValue;
        Obj_GoldText.GetComponent<Text>().text = "当前可提取修炼金币：" + goldValue;

        Obj_ExpNum.GetComponent<Text>().text = "领取次数：" + expNum + "/" + expNumMax;
        Obj_GoldNum.GetComponent<Text>().text = "领取次数：" + goldNum + "/" + goldNumMax;
        updataTime();       //更新时间显示
    }

    //更新时间显示
    void updataTime() {

        //获取时间
        int minValue = (int)((getTime - float.Parse(expTime)) / 60.0f);
        int secValue = (int)((getTime - float.Parse(expTime)) % 60.0f);
        
        Obj_ExpTime.GetComponent<Text>().text = minValue + "分" + secValue + "秒";
        if (minValue <= 0 && secValue <= 0)
        {
            Obj_ExpTime.GetComponent<Text>().text = "可领取";
        }
        else {
            //Obj_ExpTime.GetComponent<Text>().text = "今日领取次数已用完";
        }

        minValue = (int)((getTime - float.Parse(goldTime)) / 60.0f);
        secValue = (int)((getTime - float.Parse(goldTime)) % 60.0f);
        Obj_GoldTime.GetComponent<Text>().text = minValue + "分" + secValue + "秒";

        if (minValue <= 0 && secValue <= 0)
        {
            Obj_GoldTime.GetComponent<Text>().text = "可领取";
        }

        //检测当前领取次数
        if (int.Parse(expNum) >= int.Parse(expNumMax))
        {
            Obj_ExpTime.GetComponent<Text>().text = "今日领取次数已用完";
        }

        //检测当前领取次数
        if (int.Parse(goldNum) >= int.Parse(goldNumMax))
        {
            Obj_GoldTime.GetComponent<Text>().text = "今日领取次数已用完";
        }
    }


    //获取经验
    public void Btn_GetExp() { 
        //检测当前领取次数
        if (int.Parse(expNum) < int.Parse(expNumMax))
        {
            int addExpValue = expValue;
            string hintText = "获得经验：";
            //检测时间
            if(float.Parse(expTime)>=getTime){
                //触发领取,检测领取暴击
                if (Random.value <= 0.2f) {
                    addExpValue = expValue * 2;
                    hintText = "幸运奖励！获得双倍经验：";
                }
                //给予经验,写入领取次数
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ExpTime", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                int writeNum = int.Parse(expNum)+ 1;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ExpNum", writeNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                Game_PublicClassVar.Get_function_Rose.AddExp(addExpValue, "1");
                UpdataShowData();
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(hintText + addExpValue);
                //更新任务
                Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "3", "1");

            }else{
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_139");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请等待倒计时结束后领取！");
                //Debug.Log("请等待倒计时结束后领取！");
            }
        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_140");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日已达领取次数已达上限,请明天再来领取！");
            //Debug.Log("今日已达领取次数已达上限,请明天再来领取！");
        }
    }

    //获取经验
    public void Btn_GetExpRMB()
    {
        int Rmb = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID","5","GameMainValue"));
        if (!Game_PublicClassVar.Get_function_Rose.CostReward("2", Rmb.ToString())) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_86");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足！");
            return;
        }
        string hintText = "获得经验：";
        int addExpValue = expValue;
        //触发领取,检测领取暴击
        if (Random.value <= 0.2f)
        {
            addExpValue = expValue * 2;
            hintText = "幸运奖励！获得双倍经验：";
        }
        //给予经验,写入领取次数
        Game_PublicClassVar.Get_function_Rose.AddExp(addExpValue,"1");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(hintText + addExpValue);
        //更新任务
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "3", "1");
    }

    //获取金币
    public void Btn_GetGold()
    {
        //检测当前领取次数
        if (int.Parse(goldNum) < int.Parse(goldNumMax))
        {
            int addGoldValue = goldValue;
            string hintText = "获得金币：";
            //触发领取,检测领取暴击
            if (Random.value <= 0.2f)
            {
                addGoldValue = goldValue * 2;
                hintText = "幸运奖励！获得双倍金币：";
            }
            //检测时间
            if (float.Parse(goldTime) >= getTime)
            {
                //触发领取
                //给予经验,写入领取次数
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_GoldTime", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                int writeNum = int.Parse(goldNum) + 1;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_GoldNum", writeNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                Game_PublicClassVar.Get_function_Rose.SendReward("1",addGoldValue.ToString(),"47");
                UpdataShowData();
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(hintText + addGoldValue);
                //更新任务
                Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "4", "1");
            }
            else
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_141");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请等待倒计时结束后领取！");
                //Debug.Log("请等待倒计时结束后领取！");
            }
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_142");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日已达领取次数已达上限,请明天再来领取！");
            //Debug.Log("今日已达领取次数已达上限,请明天再来领取！");
        }
    }


    //获取金币
    public void Btn_GetGoldRMB()
    {

        int Rmb = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "10", "GameMainValue"));
        if (!Game_PublicClassVar.Get_function_Rose.CostReward("2", Rmb.ToString()))
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_86");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足！");
            return;
        }

        string hintText = "获得金币：";
        int addGoldValue = goldValue;
        //触发领取,检测领取暴击
        if (Random.value <= 0.2f)
        {
            addGoldValue = goldValue * 2;
            hintText = "幸运奖励！获得双倍金币：";
        }
        Game_PublicClassVar.Get_function_Rose.SendRewardToBag("1",addGoldValue);
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(hintText + addGoldValue);
        //更新任务
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "4", "1");
    }

    //关闭
    public void Btn_Close() {
        
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
        Destroy(this.gameObject);
    }
    //if()
}
