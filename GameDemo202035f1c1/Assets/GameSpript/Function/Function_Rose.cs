using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
#if UNITY_ANDROID
using Umeng;
#endif

public struct XiLianChenjiu
{
    public int chenjiuType;       //1活跃任务  2成就任务
    public string chenjiuParam;   //任务
}

public struct XiLianResult
{
    public ObscuredInt index;
    public ObscuredString equipItemId;
    public ObscuredString hideProListStr;
    public List<XiLianChenjiu> taskList; //每一次洗练可能完成多次活跃任何和成就
}

public class Function_Rose {

    private Game_PositionVar game_PositionVar;
    private Rose_Proprety rose_Proprety;
    private string selectPropetyStr;
    private List<XiLianResult> mXiLianResults;
    public event System.Action<string> SelectPropetyBack;
    private ObscuredInt OXilianNumber = 0;
    private ObscuredInt OXilianNumber100 = 100;
    private ObscuredInt OXilianNumber500 = 500;
    private ObscuredInt OXilianNumber1000 = 1000;
    private ObscuredInt OXilianNumber1500 = 1500;

    // Use this for initialization
    void Start () {
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    //获取一个随机账号ID
    public string GetZhangHuID() {

        string zhangHaoID = "";
        for (int i = 0; i <= 7; i++)
        {
            int randomValue = (int)((Random.value-0.001f)*10);
            zhangHaoID = zhangHaoID + randomValue.ToString();

        }
        return zhangHaoID;
    }
		
    //获取当前钻石
    public ObscuredInt GetRoseRMB() {

        ObscuredInt rmb = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        return rmb;
    }

    //获取当前体力
    public int GetRoseHuoLi()
    {
        int huoLi = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuoLi", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        //Debug.Log("HuoLi = " + huoLi);
        return huoLi;
    }

    //获取当前体力
    public int GetRoseTili()
    {
        int tili = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Tili", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        //Debug.Log("TiLi = " + tili);
        return tili;
    }

    public void CostRoseTiLi(int costTili) {
        //获取进入场景消耗体力
        int tili = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Tili", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        tili = tili - costTili;
        if (tili < 0) {
            tili = 0;
        }
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TiLi", tili.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

		//更新主界面
		Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().RoseUpdate_TiLi();

    }

    //获取当前角色金币
    public ObscuredLong GetRoseMoney()
    {

        long money = long.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        return money;
    }

    //获取角色当前等级
    //获取当前钻石
    public ObscuredInt GetRoseLv()
    {
        ObscuredInt roseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        return roseLv;
    }

    //获取当前牧场资金
    public ObscuredInt GetRosePastureGold()
    {
        ObscuredInt money = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureGold", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData"));
        return money;
    }

    //扣除指定的钻石
    public bool CostRMB(ObscuredInt rmb) {
        //获取自身的货币
        ObscuredInt myRmb = GetRoseRMB();
        if (myRmb >= rmb)
        {
            myRmb = myRmb - rmb;
            //写入数据表
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMB", myRmb.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            //记录值
            GameCostSaveValue("2", rmb);

            //写入活跃任务
            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "121", rmb.ToString());

            return true;
        }
        else {
            return false;
        }
    }

    //增加指定的钻石
    public bool AddRMB(ObscuredInt rmb)
    {
        //获取自身的货币
        ObscuredInt myRmb = GetRoseRMB();
            myRmb = myRmb + rmb;
            //写入数据表
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMB", myRmb.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            //如果单次增加钻石过多 请求查询数据
            if (rmb >= 100000) {
                Pro_ComStr_3 comstr_3 = new Pro_ComStr_3();
                comstr_3.str_1 = "5";
                comstr_3.str_2 = myRmb.ToString();
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001201, comstr_3);
            }

            //发送记录
            GameGetSaveValue("2", rmb);

        return true;
    }

    //增加指定的充值额度
    public bool AddPayValue(float payValue,string sendType = "0")
    {

        string nowPayValueStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (nowPayValueStr == ""|| nowPayValueStr == null) {
            nowPayValueStr = "0";
        }
        float nowPayValue = float.Parse(nowPayValueStr);
        nowPayValue = nowPayValue + payValue;

        //写入数据表
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMBPayValue", nowPayValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //冒险等级积分
        AddMaoXianExp(payValue * 10,"50");

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        string dayPayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (dayPayValue == "" || dayPayValue == null)
        {
            dayPayValue = "0";
        }
        float nowDayPayValue = float.Parse(dayPayValue);
        nowDayPayValue = nowDayPayValue + payValue;
        //写入每日付费数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayPayValue", nowDayPayValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        ServerMsg_SendItemToBag("5","5",payValue.ToString(),sendType);

        return true;
    }

    public bool AddMaoXianExp(float addValue,string sendType = "0") {

        string nowMaoXianExpStr =Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MaoXianJiaExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (nowMaoXianExpStr == "" || nowMaoXianExpStr == null) {
            nowMaoXianExpStr = "0";
        }

        //冒险等级积分
        float nowMaoXianExp = float.Parse(nowMaoXianExpStr);
        nowMaoXianExp = nowMaoXianExp + addValue;

        //写入数据表
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MaoXianJiaExp", nowMaoXianExp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        ServerMsg_SendItemToBag("4", "4", addValue.ToString(), sendType);

        return true;
    }

    //增加指定的活力值
    public bool AddHuoLi(int addHuoLi)
    {
        //获取自身体力
        int nowHuoLi = GetRoseHuoLi();
        nowHuoLi = nowHuoLi + addHuoLi;
        //体力不能超过最大值
        if (nowHuoLi > 100)
        {
            nowHuoLi = 100;
        }
        //写入数据表
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HuoLi", nowHuoLi.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        //更新显示
        if (Application.loadedLevelName != "StartGame")
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().UpdataRoseStatus = true;
            //更新主界面
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().RoseUpdate_HuoLi();
        }

        return true;
    }

    //增加指定的活力值
    public bool CostHuoLi(int costHuoLi)
    {
        //获取自身体力
        int nowHuoLi = GetRoseHuoLi();
        if (nowHuoLi < costHuoLi) {
            return false;
        }
        nowHuoLi = nowHuoLi - costHuoLi;
        //体力不能超过最大值
        if (nowHuoLi < 0)
        {
            nowHuoLi = 0;
        }
        //写入数据表
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HuoLi", nowHuoLi.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        //更新显示
        if (Application.loadedLevelName != "StartGame")
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().UpdataRoseStatus = true;
            //更新主界面
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().RoseUpdate_HuoLi();
        }

        return true;
    }

    //增加指定的体力值
    public bool AddTili(int tili) { 
        //获取自身体力
        int nowTili = GetRoseTili();
        nowTili = nowTili + tili;
        //体力不能超过最大值
        if (nowTili > 100) {
            nowTili = 100;
        }
        //写入数据表
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Tili", nowTili.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        //更新显示
        if (Application.loadedLevelName != "StartGame") {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().UpdataRoseStatus = true;
            //更新主界面
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().RoseUpdate_TiLi();
        }

        return true;
    }

    //增加指定的经验值,expShowType参数传入非0的值将不显示飘字
    public bool AddExp(ObscuredInt exp,string expShowType = "0",bool ifChuBeiStatus = false)
    {
        if (exp == 0) {
            return true;
        }

        if (ifChuBeiStatus) {
            int nowChuBeiExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChuBeiExp","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            if (nowChuBeiExp < exp)
            {
                exp = exp + nowChuBeiExp;
                nowChuBeiExp = 0;
            }
            else {
                exp = exp + exp;
                nowChuBeiExp = nowChuBeiExp - exp;
            }
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChuBeiExp", nowChuBeiExp.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        }

        //获取自身经验
        Function_DataSet dataSet = Game_PublicClassVar.Get_function_DataSet;
        Rose_Proprety rose_Proprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();

        //判定是否升级
        int Rose_Lv = int.Parse(dataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        //等级超过70级不获得任何经验
        if (Rose_Lv >= 70)
        {
            return true;
        }
        
        float addExpValue = (float)(exp) * (1 + rose_Proprety.Rose_Exp_AddPro + Game_PublicClassVar.Get_wwwSet.WorldExpProAdd) + rose_Proprety.Rose_Exp_AddValue;
        long nowExp = (long)(long.Parse(dataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData")) + (long)(addExpValue));
		//Debug.Log ("exp = " + exp + "nowExp = " + nowExp);
        //写入数据表
        dataSet.DataSet_WriteData("RoseExpNow", nowExp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        dataSet.DataSet_SetXml("RoseData");

        long Rose_Exp = long.Parse(dataSet.DataSet_ReadData("RoseUpExp", "RoseLv", Rose_Lv.ToString(), "RoseExp_Template"));
        long Rose_ExpNow = long.Parse(dataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

        //判断当前是否超过世界等级
        bool worldLvStatus = false;

        //是否读取到世界等级
        if (Game_PublicClassVar.Get_wwwSet.WorldLv >= 20) {
            if (Rose_Lv < Game_PublicClassVar.Get_wwwSet.WorldLv) {
                worldLvStatus = true;
            }
        }

        if (Rose_Lv <= 19) {
            worldLvStatus = true;
        }

        //是否完成觉醒任务
        if (Rose_Lv >= 60) {
            if (Rose_ExpNow >= Rose_Exp)
            {
                string jihuoIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (jihuoIDStr.Contains("10001") == false)
                {
                    worldLvStatus = false;
                }
            }
        }

        if (worldLvStatus) {

            if (Rose_ExpNow >= Rose_Exp)
            {
                Rose_Lv = Rose_Lv + 1;
                dataSet.DataSet_WriteData("Lv", Rose_Lv.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                //多余的经验自动转移到下一级
                long expValue = Rose_ExpNow - Rose_Exp;
                Rose_ExpNow = expValue;
                dataSet.DataSet_WriteData("RoseExpNow", expValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                //写入当前天赋SP值,大于10级显示天赋
                if (Rose_Lv >= 10)
                {
                    int spValue = int.Parse(dataSet.DataSet_ReadData("SP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                    dataSet.DataSet_WriteData("SP", (spValue + 1).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                }

                //写入技能SP
                int skillSPValue = int.Parse(dataSet.DataSet_ReadData("SkillSP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                dataSet.DataSet_WriteData("SkillSP", (skillSPValue + 1).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                //获取当前最大值
                rose_Proprety.Rose_HpNow = rose_Proprety.Rose_Hp;
                dataSet.DataSet_WriteData("RoseHpNow", rose_Proprety.Rose_HpNow.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                dataSet.DataSet_SetXml("RoseData");

                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseUpLv = true;
                rose_Proprety.updataOnly = false;           //更新血量
                Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true; //升级更新任务

                //升级生成备份存档(大于5级每次升级自动存档)
                if (Rose_Lv >= 5)
                {
                    Game_PublicClassVar.Get_wwwSet.IfSaveRoseData = true;      //开启储备数据
                }

                //存储角色通用数据
                Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "Lv", Rose_Lv.ToString());

                //更新当前等级宠物最大数量
                Game_PublicClassVar.Get_function_AI.Pet_UpdateLvMaxNum();

                //更新主界面
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().RoseUpdate_Lv();

                //写入成就(等级次数)
                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("201", "0", Rose_Lv.ToString());

                Rose_OpenFunctionHint();

                //新手引导部分
                //技能
                if (Rose_Lv == 6)
                {
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_RoseSkill();
                }

                //制造
                if (Rose_Lv == 9)
                {
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_RoseMake();
                }

                //设置
                if (Rose_Lv == 10)
                {
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_RoseSetting();
                }

                //每次等级提升都会上传数据
                if (Rose_Lv >= 10)
                {
                    //获取唯一ID
                    //zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    //每次进入游戏上传一次玩家数据
                    string[] saveList = new string[] { "", "2", "预留设备号位置", "6" };
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001006, saveList);
                }

                //开启后面章节
                if (Rose_Lv == 55)
                {
                    UpdataPVEChapter("6;1");
                }

                if (Rose_Lv == 57)
                {
                    UpdataPVEChapter("6;2");
                }

                if (Rose_Lv == 59)
                {
                    UpdataPVEChapter("6;3");
                }
            }
        }

        rose_Proprety.Rose_GetExp = true;

		//更新主界面显示
		Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set> ().Obj_UI_RoseExp.GetComponent<UI_MainUIRoseExp>().UpdataRoseExp = true;
        //如果当前角色在建筑界面更新经验显示
        if (Application.loadedLevelName == "EnterGame") {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().UpdataRoseStatus = true;
        }

        //弹出获取提示
        UI_RoseGetItemHint hint = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseGetItemHint.GetComponent<UI_RoseGetItemHint>();
        hint.UpdataHintText = "获得" + (int)(addExpValue) + "点经验";
        if (rose_Proprety.Rose_Exp_AddPro > 0) {
            hint.UpdataHintText = hint.UpdataHintText + "(+" +((rose_Proprety.Rose_Exp_AddPro + Game_PublicClassVar.Get_wwwSet.WorldExpProAdd) * 100) + "%经验加成)";
        }
        hint.UpdataHint = true;

        //通用快捷提示框
        if (expShowType == "0") {
            Game_PublicClassVar.Get_function_UI.GameGirdHint(hint.UpdataHintText);
        }
        try
        {
            //记录玩家等级数据
            if (Rose_ExpNow >= Rose_Exp)
            {
#if UNITY_ANDROID
                GA.SetUserLevel(Rose_Lv);
#endif

            }
        }
        catch {
            //Debug.Log("Umeng等级有报错");
        }

        //经验暂时不处理
        //ServerMsg_SendItemToBag("2", "2", exp.ToString(), sendType);

        return true;
    }

    public void Rose_OpenFunctionHint() {

        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

        string returnHintStr = "";

        //4级开启装备制造
        if (roseLv >= 1) {
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("4级开启");
            string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("装备制造");
            returnHintStr = langStr_1 + "\n" + langStr_2;
        }

        //8级开启宠物系统
        if (roseLv >= 4)
        {
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("8级开启");
            string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("宠物系统");
            returnHintStr = langStr_1 + "\n" + langStr_2;
        }

        //10级开启天赋系统
        if (roseLv >= 8)
        {
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("10级开启");
            string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("天赋系统");
            returnHintStr = langStr_1 + "\n" + langStr_2;
        }

        //13级开启家园系统
        if (roseLv >= 10)
        {
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("13级开启");
            string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("家园系统");
            returnHintStr = langStr_1 + "\n" + langStr_2;
        }

        //20级开启宠物合成
        if (roseLv >= 13)
        {
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("20级开启");
            string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("宠物合成");
            returnHintStr = langStr_1 + "\n" + langStr_2;
        }

        if (roseLv >= 20)
        {
            returnHintStr = "";
        }

        if (returnHintStr != "")
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_OpenFunctionHint.SetActive(true);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_OpenFunctionHintText.GetComponent<Text>().text = returnHintStr;
        }
        else {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_OpenFunctionHint.SetActive(false);
        }
    }

    //花费钻石换取体力
    public bool RMBtoTili(int rmb, int tili) {

        if (CostRMB(rmb)) {
            AddTili(tili);
            return true;
        }
        else
        {
            return false;
        }
    }

    //增加觉醒经验
    public void AddJueXingExp(int addValue) {

        string juexingExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (juexingExp == "" || juexingExp == null)
        {
            juexingExp = "0";
        }

        //觉醒经验大于1万不增长
        if (int.Parse(juexingExp) >= 10000)
        {
            return;
        }

        int nowExp = int.Parse(juexingExp) + addValue;

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JueXingExp", nowExp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

    }

    
    //根据类型发送对应奖励
    public bool SendReward(string type, string value,string getType = "0") {

		//更新背包立即显示
		Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        Function_DataSet dataSet = Game_PublicClassVar.Get_function_DataSet;
        Rose_Proprety rose_Proprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();

        switch (type) { 
            
            //发送金币
            case "1":
                //写入对应的值
                ObscuredLong goldValue = long.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                goldValue = goldValue + long.Parse(value);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GoldNum", goldValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                //更新通用界面显示
                Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet();

                //写入成就(金币累计)
                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("105", "0", value);

                //如果单次增加过多 请求查询数据
                if (int.Parse(value) >= 1000000)
                {
                    Pro_ComStr_3 comstr_3 = new Pro_ComStr_3();
                    comstr_3.str_1 = "2";
                    comstr_3.str_2 = goldValue.ToString();
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001201, comstr_3);
                }

                //记录
                ServerMsg_SendItemToBag(type, type, value, getType);

                GameGetSaveValue(type, int.Parse(value));

                return true;
            break;

            //发送钻石
            case "2":
                //写入对应的值
                ObscuredInt rmbValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                rmbValue = rmbValue + int.Parse(value);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMB", rmbValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                //更新通用界面显示
                Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet();
                //记录
                ServerMsg_SendItemToBag(type, type, value, getType);

                GameGetSaveValue(type, int.Parse(value));
                return true;
            break;

            //发送牧场资金
            case "5":
                //写入对应的值
                string PastureGoldStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureGold", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                if (PastureGoldStr == "" || PastureGoldStr == null) {
                    PastureGoldStr = "0";
                }
                ObscuredInt pastureGold = int.Parse(PastureGoldStr);
                pastureGold = pastureGold + int.Parse(value);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureGold", pastureGold.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
                //更新通用界面显示
                Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet();
                //记录
                ServerMsg_SendItemToBag(type, type, value, getType);

                GameGetSaveValue(type, int.Parse(value));

                //更新牧场显示
                Game_PublicClassVar.Get_function_Pasture.UpdatePastureShowData();

                Game_PublicClassVar.Get_function_UI.GameHint("牧场资金增加" + value);

                //写入成就
                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("225", "0", value);

                return true;
                break;
        }
        return false;
    }

	//根据类型扣除对应货币
	public ObscuredBool CostReward(string type, ObscuredString value) {

		//更新背包立即显示
		Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

		switch (type) { 
			
		//扣除金币
		case "1":
			//写入对应的值
            ObscuredLong goldValue = long.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

            ObscuredInt costValue = int.Parse(value);

            ObscuredLong nowGoldValue = goldValue - costValue;

            goldValue = nowGoldValue;

            if (goldValue>=0){
				Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GoldNum", goldValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                //更新通用界面显示
                Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet();
                //记录
                GameCostSaveValue(type, int.Parse(value));
                return true;
			}else{
				return false;
			}
			break;
			
		//扣除钻石
		case "2":
			//写入对应的值
            ObscuredInt rmbValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
			rmbValue = rmbValue - int.Parse(value);
			if(rmbValue>=0){
				Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMB", rmbValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                //更新通用界面显示
                Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet();

                //写入活跃任务
                Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "121", value);

                //记录
                GameCostSaveValue(type, int.Parse(value));
                return true;
			}else{
				return false;
			}

			break;

        //扣除牧场资金
        case "5":
            //写入对应的值
            ObscuredInt rongyuValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureGold", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData"));
            rongyuValue = rongyuValue - int.Parse(value);
            if (rongyuValue >= 0)
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureGold", rongyuValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
                //更新牧场显示
                Game_PublicClassVar.Get_function_Pasture.UpdatePastureShowData();
                return true;
            }
            else
            {
                return false;
            }

            break;

        }


		return false;
	}


    //记录获取数据
    public void GameGetSaveValue(string getType,int getValue) {

        switch (getType) {

            //金币
            case "1":
                string GetGoldSumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GetGoldSum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

                if (GetGoldSumStr == "" || GetGoldSumStr == null) {
                    GetGoldSumStr = "0";
                }


                int sumGold = int.Parse(GetGoldSumStr);
                sumGold = sumGold + getValue;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GetGoldSum", sumGold.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                break;

            //钻石
            case "2":

                string GetZuanShiSumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GetZuanShiSum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

                if (GetZuanShiSumStr == "" || GetZuanShiSumStr == null)
                {
                    GetZuanShiSumStr = "0";
                }

                int sumZuanShi = int.Parse(GetZuanShiSumStr);
                sumZuanShi = sumZuanShi + getValue;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GetZuanShiSum", sumZuanShi.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                break;
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //SendGameHuoBiJiLu();

    }

    //记录获取数据
    public void GameCostSaveValue(string getType, int getValue)
    {

        switch (getType)
        {
            //金币
            case "1":
                string costGoldSumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostGoldSum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                if (costGoldSumStr == ""|| costGoldSumStr == null) {
                    costGoldSumStr = "0";
                }
                int sumGold = int.Parse(costGoldSumStr);
                sumGold = sumGold + getValue;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CostGoldSum", sumGold.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                break;

            //钻石
            case "2":
                string costZuanShiSumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostZuanShiSum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                if (costZuanShiSumStr == "" || costZuanShiSumStr == null)
                {
                    costZuanShiSumStr = "0";
                }
                int sumZuanShi = int.Parse(costZuanShiSumStr);
                sumZuanShi = sumZuanShi + getValue;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CostZuanShiSum", sumZuanShi.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                break;
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //SendGameHuoBiJiLu();
    }

    //发送道具到背包,支持金币(发送道具ID,发送数量,是否广播,装备隐藏属性掉落概率,装备隐藏属性ID,装备是否进行宝石开孔)
    public bool SendRewardToBag(string dropID, int dropNum, string broadcastType = "0", float hideProValue = 0, string equipHideID = "0", bool gemStatus = true, string sendType = "0",string itemPar = "")
    {
        //Debug.Log("hideProValue = " + hideProValue);
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
		//更新背包立即显示
		Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

        bool ifGold = false;
        //掉落为空
        if (dropID == "0") {
            return true;
        }

        Rose_Proprety rose_Proprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();

        //判定掉落是否为金币
        if (dropID == "1")
        {
            //增加经验有经验加成
            int goldNum = (int)(dropNum + rose_Proprety.Rose_Gold_AddValue * (1 + rose_Proprety.Rose_Gold_AddPro));
            Game_PublicClassVar.Get_function_Rose.SendReward("1", goldNum.ToString(),sendType);
            ifGold = true;

            //弹窗提示
            //string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
            switch (broadcastType)
            {
                //广播
                case "0":
                    string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_421");
                    string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_422");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + dropNum.ToString() + langStrHint_2);
                    //Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "金币");
                    break;
                //不广播
                case "1":
                    break;
            }

            //更新道具任务显示
            //Game_PublicClassVar.Get_function_Task.updataTaskItemID();
            
            return true;
        }
        else
        {
            ifGold = false;
        }
        //经验
        if (dropID == "2") {
            AddExp(dropNum);

            return true;
            //ifGold = true;
        }
        //钻石
        if (dropID == "3")
        {
            Game_PublicClassVar.Get_function_Rose.SendReward("2", dropNum.ToString(),sendType);
            //弹窗提示
            //string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
            switch (broadcastType)
            {
                //广播
                case "0":
                    string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_421");
                    string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_423");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + dropNum.ToString() + langStrHint_2);
                    //Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "钻石");
                    break;
                //不广播
                case "1":
                    break;
            }

            return true;
            //ifGold = true;
        }

        //冒险家经验
        if (dropID == "4")
        {
            AddMaoXianExp(dropNum,sendType);
            return true;
        }

        //牧场资金
        if (dropID == "5")
        {

            switch (broadcastType)
            {
                //广播
                case "0":
                    string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_421");
                    string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_459");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + dropNum.ToString() + langStrHint_2);
                    //Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "钻石");
                    break;
                //不广播
                case "1":
                    break;
            }

            SendReward("5", dropNum.ToString(), sendType);
            return true;
        }

        if (!ifGold)
        {
            Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow=true; //拾取道具更新任务
            //Debug.Log("dropID = " + dropID);
            string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", dropID, "Item_Template");
            string itemSubType = function_DataSet.DataSet_ReadData("ItemSubType", "ID", dropID, "Item_Template");
            //牧场道具直接发送到牧场背包内
            if (itemType == "6" && itemSubType == "1") {
                Game_PublicClassVar.function_Pasture.SendPastureBag(dropID,dropNum,broadcastType);
                return true;
            }

            //将掉落的道具ID添加到背包内
            for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
            {
                //获得当前背包内对应格子的道具ID
                string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
                //Rdate = "0";
                //寻找背包内有没有相同的道具ID
                if (dropID == Rdate)
                {

                    //读取当前道具数量
                    string itemValue = function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseBag");
                    
                    //读取当前道具的堆叠数量的最大值
                    string itemPileSum = function_DataSet.DataSet_ReadData("ItemPileSum", "ID", dropID, "Item_Template");
                    int itemNum = int.Parse(itemValue) + dropNum; //将数量累加（此处没有顾忌到自己背包格子满的处理方式，以后添加）
                    //当满足堆叠数量,执行道具捡取
                    if (int.Parse(itemPileSum) >= itemNum)
                    {
                        //添加获得的道具数量
                        function_DataSet.DataSet_WriteData("ItemNum", itemNum.ToString(), "ID", i.ToString(), "RoseBag");
                        function_DataSet.DataSet_SetXml("RoseBag");
                        //弹窗提示
                        string itemName = function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
                        switch (broadcastType) { 
                            //广播
                            case "0":
                                //Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "个" + itemName);
                                //Game_PublicClassVar.Get_function_UI.GameHint("你获得 "+"<color=#32CD32ff>" + dropNum.ToString() + "</color> "+"个" +"<color=#FF6347ff>" + itemName + "</color>");
                                //获取道具品质
                                string itemQuality = function_DataSet.DataSet_ReadData("ItemQuality", "ID", dropID, "Item_Template");
                                string qualityStr = Game_PublicClassVar.Get_function_UI.QualityReturnColorText(itemQuality);
                                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_421");
                                Game_PublicClassVar.Get_function_UI.GameHint(langStrHint_1 +" "+ qualityStr + itemName + " * " + dropNum.ToString() + "</color>" + " !");
                                //Game_PublicClassVar.Get_function_UI.GameHint("你获得 " + qualityStr + itemName + " * " + dropNum.ToString() + "</color>" + " !");
                                break;
                            //不广播
                            case "1":
							
                            break;
                        }
                        
                        //ai_dorpitem.IfRoseTake = true;      //注销拾取的道具
						//更新道具任务显示
						//Game_PublicClassVar.Get_function_Task.updataTaskItemID();
                        if (itemType != "3")
                        {
                            Game_PublicClassVar.Get_game_PositionVar.UpdatTaskStatus = true;
                        }
                        //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
                        
                        if (itemType == "1")
                        {
                            Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
                        }
                        return true;
                        break;
                    }
                }
                //发现背包格子为空，将数据直接塞进空的格子中（从前面排序）
                if (Rdate == "0")
                {
                    function_DataSet.DataSet_WriteData("ItemID", dropID, "ID", i.ToString(), "RoseBag");
                    function_DataSet.DataSet_WriteData("ItemNum", dropNum.ToString(), "ID", i.ToString(), "RoseBag");
                    function_DataSet.DataSet_WriteData("HideID", equipHideID, "ID", i.ToString(), "RoseBag");



                    //藏宝图获取随机坐标值
                    if (itemType == "1")
                    {
                        //藏宝图
                        if (itemSubType == "29")
                        {
                            string cangBaoTuStr = GetCangBaoTuStr();
                            function_DataSet.DataSet_WriteData("ItemPar", cangBaoTuStr, "ID", i.ToString(), "RoseBag");
                        }

                        //藏宝图(地图)
                        if (itemSubType == "32")
                        {
                            string cangBaoTuStr = GetCangBaoTuMapStr();
                            function_DataSet.DataSet_WriteData("ItemPar", cangBaoTuStr, "ID", i.ToString(), "RoseBag");
                        }

                    }
                    
                    //弹窗提示
                    string itemName = function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
                    switch (broadcastType)
                    {
                        //广播
                        case "0":
                            //Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "个" + itemName);
                            //Game_PublicClassVar.Get_function_UI.GameHint("你获得 "+"<color=#DD36E7FF>" + dropNum.ToString() + "</color> "+"个" +"<color=#FF6347ff>" + itemName + "</color>");
                            //获取道具品质
                            string itemQuality = function_DataSet.DataSet_ReadData("ItemQuality", "ID", dropID, "Item_Template");
                            string qualityStr = Game_PublicClassVar.Get_function_UI.QualityReturnColorText(itemQuality);
                            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_421");
                            Game_PublicClassVar.Get_function_UI.GameHint(langStrHint_1 + " " + qualityStr + itemName + " * " + dropNum.ToString() + "</color>" + " !");
                            //Game_PublicClassVar.Get_function_UI.GameHint("你获得 " + qualityStr + itemName + " * " + dropNum.ToString() + "</color>" + " !");
                            break;
                        //不广播
                        case "1":

                            break;
                    }

					//更新道具任务显示
                    if (itemType != "3") {
                        Game_PublicClassVar.Get_game_PositionVar.UpdatTaskStatus = true;
                    }
                    
					//Game_PublicClassVar.Get_function_Task.updataTaskItemID();
                    //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
                    //string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", dropID, "Item_Template");
                    if (itemType == "1") {
                        Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
                    }

                    //获取当前道具是否增加极品属性  
                    if (itemType == "3"&& equipHideID=="0")
                    {
                        //极品概率
                        //if (Random.value < hideProValue){

                            string hideID = "0";
                            try
                            {
                                hideID = ReturnHidePropertyID(dropID);
                            }
                            catch {
                                //重新介入当前隐藏技能ID
                                hideID = "0";
                                int hideNumID = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseEquipHideNumID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig"));
                                hideNumID = hideNumID + 1000;
                                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseEquipHideNumID", hideNumID.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                            }
                            
                            //Debug.Log("hideID = " + hideID + "dropID = "+dropID);
                            if (hideID != "0")
                            {
                                //写入极品属性ID
                                function_DataSet.DataSet_WriteData("HideID", hideID, "ID", i.ToString(), "RoseBag");
                                Game_PublicClassVar.Get_game_PositionVar.ChouKaHindIdStr = hideID;
                                //Debug.Log("极品属性写入成功");
                            }
                            else
                            {
                                //清空极品属性字段
                                function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseBag");
                            }
                        //}

                        if (gemStatus) {

                            //增加宝石孔位
                            string holeStr = "";
                            string gemStr = "";

                            //设置一个宝石最多有多少宝石孔位
                            int equipGemMaxNum = 4;

                            //获取装备上的孔位信息
                            string equipGem = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", dropID, "Equip_Template");
                            int equipGemNum = 0;
                            if (equipGem != "" && equipGem != "0") {
                                equipGemNum = equipGem.Split(',').Length;
                                holeStr = equipGem;
                                for (int z = 0; z < equipGemNum; z++)
                                {
                                    gemStr = gemStr + "0,";
                                }
                            }

                            if (holeStr != "") {
                                holeStr = holeStr + ",";
                            }


                            //获取当前开孔的附加概率
                            float holePro = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_GemHole_AddPro;
                            holePro = 1 + holePro;


                            //随机出现孔位的数量
                            string itemQuality = function_DataSet.DataSet_ReadData("ItemQuality", "ID", dropID, "Item_Template");
                            int randowHoleNum = ReturnHoleNum(itemQuality);

                            //橙色装备必出3-4个孔位,低于50的
                            string itemlv = function_DataSet.DataSet_ReadData("ItemLv", "ID", dropID, "Item_Template");
                            if (int.Parse(itemQuality) >= 5) {
                                if (randowHoleNum == 0 || randowHoleNum == 1 || randowHoleNum == 2)
                                {
                                    randowHoleNum = 2;
                                }
                                else {
                                    randowHoleNum = 4;
                                }
                            }

                            //紫色最多3孔
                            if (int.Parse(itemQuality) <= 3)
                            {
                                if (randowHoleNum == 4)
                                {
                                    randowHoleNum = 3;
                                }
                            }


                            //饰品有概率出一个多彩
                            if (itemSubType == "5" && Random.value <= 0.25f)
                            {
                                holeStr = holeStr + "110,";
                                gemStr = gemStr + "0,";
                                randowHoleNum = randowHoleNum - 1;
                            }


                            if (itemSubType != "" && itemSubType != null) {

                                //十二生肖不会有槽位
                                if (int.Parse(itemSubType)>=101 && int.Parse(itemSubType) <= 112) {
                                    randowHoleNum = 0;
                                }

                                //宠物装备不会有槽位
                                if (int.Parse(itemSubType) >= 201 && int.Parse(itemSubType) <= 204)
                                {
                                    randowHoleNum = 0;
                                }
                            }

                            //循环出现孔位
                            for (int y = 0; y < randowHoleNum; y++ )
                            {
                                if (equipGemNum < equipGemMaxNum) {
                                    holeStr = holeStr + ReturnHoleColoer() + ",";
                                    gemStr = gemStr + "0,";
                                    equipGemNum = equipGemNum + 1;
                                }
                            }

                            /*
                            //第一个孔位概率
                            if (equipGemNum < equipGemMaxNum) {

                                if (Random.value < 0.05f * holePro)
                                {
                                    holeStr = holeStr + ReturnHoleColoer() + ",";
                                    gemStr = gemStr + "0,";
                                    equipGemNum = equipGemNum + 1;
                                }
                            }

                            //第二个孔位概率
                            if (equipGemNum < equipGemMaxNum)
                            {

                                if (Random.value < 0.05f * holePro)
                                {
                                    holeStr = holeStr + ReturnHoleColoer() + ",";
                                    gemStr = gemStr + "0,";
                                    equipGemNum = equipGemNum + 1;
                                }
                            }

                            //第三个孔位概率
                            if (equipGemNum < equipGemMaxNum)
                            {
                                if (Random.value < 0.05f * holePro)
                                {
                                    holeStr = holeStr + ReturnHoleColoer() + ",";
                                    gemStr = gemStr + "0,";
                                    equipGemNum = equipGemNum + 1;
                                }
                            }

                            //第四个孔位概率
                            if (equipGemNum < equipGemMaxNum)
                            {
                                if (Random.value < 0.05f * holePro)
                                {
                                    holeStr = holeStr + ReturnHoleColoer() + ",";
                                    gemStr = gemStr + "0,";
                                    equipGemNum = equipGemNum + 1;
                                }
                            }
                            */
                            //修正孔位信息
                            if (holeStr == "")
                            {
                                holeStr = "0";
                                gemStr = "0";
                            }
                            else {
                                holeStr = holeStr.Substring(0, holeStr.Length-1);
                                gemStr = gemStr.Substring(0, gemStr.Length - 1);
                            }

                            function_DataSet.DataSet_WriteData("GemHole", holeStr, "ID", i.ToString(), "RoseBag");
                            function_DataSet.DataSet_WriteData("GemID", gemStr, "ID", i.ToString(), "RoseBag");
                        }

                        //添加收藏
                        AddShouJiItem(dropID);

                    }
                    else {
                        //除装备外,其他道具添加时HideID均为0
                        //function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseBag");
                    }

                    if (itemPar != "") {
                        function_DataSet.DataSet_WriteData("ItemPar", itemPar, "ID", i.ToString(), "RoseBag");
                    }

                    function_DataSet.DataSet_SetXml("RoseBag");
                    ServerMsg_SendItemToBag("0",dropID,dropNum.ToString(),sendType);

                    return true;
                    break;  //跳出循环
                }
                //在结束循环的最后判定道具如果没有被拾取,判定为背包满了
                if (i == Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum)
                {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_301");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint("背包已满,请及时清理背包！");
                    return false;
                }
            }
        }

        return false;

    }

    //发送信息奖励
    private void ServerMsg_SendItemToBag(string type, string itemID,string itemNum,string SendType) {

        //检测服务器是否需要发送
        if (Game_PublicClassVar.Get_gameLinkServerObj.IfSendGetItemData) {

            Pro_GameMsg proGameMsg = new Pro_GameMsg();
            proGameMsg.Des = "";
            proGameMsg.Type = type;
            proGameMsg.TypeSon = SendType;
            proGameMsg.TargetID = itemID;
            proGameMsg.TargetNum = itemNum;


            //获取新战区时间
            //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001016, proGameMsg);   弃用
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000115, proGameMsg);

        }

    }


    //发送信息奖励
    public void ServerMsg_SendMsg(string des)
    {
        //检测服务器是否需要发送
        if (Game_PublicClassVar.Get_gameLinkServerObj.IfSendGetItemData)
        {
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000116, des);
        }
    }


    //获取孔位颜色
    public string ReturnHoleColoer() {

        float ranValue = Random.value;

        //红色
        if (ranValue <= 0.2f) {
            return "101";
        }

        //紫色
        if (ranValue <= 0.45f)
        {
            return "102";
        }

        //蓝色
        if (ranValue <= 0.7f)
        {
            return "103";
        }

        //绿色
        if (ranValue <= 0.95f)
        {
            return "104";
        }

        //白色
        if (ranValue <= 1.0f)
        {
            return "105";
        }
        //所有可能都排除
        Debug.Log("宝石随机值不为100%");
        return "101";
    }

    //获取孔位颜色
    public int ReturnHoleNum(string quality)
    {

        float ranValue = Random.value;

        if (quality == "5" || quality == "6")
        {

            if (ranValue <= 0.3f)
            {
                return 1;
            }

            if (ranValue <= 0.6f)
            {
                return 2;
            }

            if (ranValue <= 0.8f)
            {
                return 3;
            }

            if (ranValue <= 1.0f)
            {
                return 4;
            }
        }

        if (ranValue <= 0.5f)
        {
            return 0;
        }

        if (ranValue <= 0.7f)
        {
            return 1;
        }

        if (ranValue <= 0.85f)
        {
            return 2;
        }

        if (ranValue <= 0.95f)
        {
            return 3;
        }

        if (ranValue <= 1.0f)
        {
            return 4;
        }


        //所有可能都排除
        return 0;
    }


    public bool ReturnNeedBagItemNum(string itemID,int needNum)
    {
        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(itemID);
        //当某一材料未达成合成显示失败
        if (itemNum >= needNum)
        {
            return true;
        }
        else {
            return false;
        }
    }

	//获取一个道具在背包内的数量
	public int ReturnBagItemNum(string itemID){

		//获取金币
		if(itemID == "1"){

			return (int)(GetRoseMoney());
		}

		//获取钻石
		if(itemID == "3"){

			return GetRoseRMB();
		}

		int num = 0;
        string bagItemID = "0";
        string bagItemNum = "0";
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
		//获取道具ID
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {

			//获取当前背包格子的道具ID和数量；
            bagItemID = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
			if(bagItemID!="0"){
                bagItemNum = function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseBag");
				if(itemID == bagItemID){
					num = num+ int.Parse(bagItemNum);
				}
			}
		}
		return num;
	}

    //消耗背包指定位置的道具数量
    public bool CostBagSpaceItem(string itemID, int itemNum, string bagSpace) {
        //判定扣除的道具是否和背包内的一致
        string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpace, "RoseBag");
        if (itemID == bagItemID) {
            string bagItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", bagSpace, "RoseBag");
            int bagItemNum_Int = int.Parse(bagItemNum);
            if (bagItemNum_Int >= itemNum) {

                int nowNum = bagItemNum_Int - itemNum;
                if (nowNum >= 1)
                {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", nowNum.ToString(), "ID", bagSpace, "RoseBag");
                }
                else {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", bagSpace, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", nowNum.ToString(), "ID", bagSpace, "RoseBag");
                }
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");

                //更新背包立即显示
                Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

                //更新道具任务显示
                Game_PublicClassVar.Get_function_Task.updataTaskItemID();

                //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
                if (itemType == "1")
                {
                    Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
                }

                return true;
            }
        }
        return false;
    }

	//消耗一个道具在背包内的数量
	public bool CostBagItem(string itemID,int itemNum){

        if (itemID == "1") {
            return CostReward(itemID, itemNum.ToString());
        }

		//获取当前道具ID拥有数量
		int value = ReturnBagItemNum(itemID);
		if (value >= itemNum) {
			//获取道具ID
            for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
            {
				//获取当前背包格子的道具ID和数量；
				string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemID", "ID", i.ToString (), "RoseBag");
				if (bagItemID != "0") {
					string bagItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemNum", "ID", i.ToString (), "RoseBag");
					if (itemID == bagItemID) {
						int num = 0;
						if (int.Parse (bagItemNum) >= itemNum) {
							num = int.Parse (bagItemNum) - itemNum;
							Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData ("ItemNum", num.ToString (), "ID", i.ToString (), "RoseBag");
                            //如果道具为0,则清空道具
                            if (num == 0) {
                                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", i.ToString(), "RoseBag");
                            }
							Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml ("RoseBag");
							itemNum = 0;
                            //跳出循环后面不执行
                            break;

						} else {
							itemNum = itemNum - int.Parse (bagItemNum);
							num = 0;
							Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData ("ItemID", "0", "ID", i.ToString (), "RoseBag");
							Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData ("ItemNum", num.ToString (), "ID", i.ToString (), "RoseBag");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseBag");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", i.ToString(), "RoseBag");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", i.ToString(), "RoseBag");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", i.ToString(), "RoseBag");
							Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml ("RoseBag");
						}
					}
				}
			}

            //更新背包立即显示
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

            //更新道具任务显示
            Game_PublicClassVar.Get_function_Task.updataTaskItemID();

            //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
            if (itemType == "1")
            {
                Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
            }
			return true;
		} else {
			return false;
		}

		//return num;
	}

    //消耗一个道具在背包内指定位置的数量  （道具ID 道具数量 格子位置, 是否扣除全部）
    public bool CostBagSpaceNumItem(string itemID, int itemNum,string spaceNum,bool costAllSpace)
    {
        //
        string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceNum, "RoseBag");
        string bagItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceNum, "RoseBag");

        //是否扣除全部
        if (costAllSpace) {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceNum, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceNum, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceNum, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", spaceNum, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", spaceNum, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", spaceNum, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            return true;
        }

        //
        if (itemID == bagItemID)
        {
            int otherValue = int.Parse(bagItemNum) - itemNum;
            if (otherValue > 0)
            {
                //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", i.ToString(), "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", otherValue.ToString(), "ID", spaceNum, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
            }
            else {
                
                if (otherValue < 0) {
                    Game_PublicClassVar.Get_function_UI.GameHint("匹配数据错误");
                    return false;
                }
                
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceNum, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceNum, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceNum, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", spaceNum, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", spaceNum, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
            }
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            return true;
        }
        else {
            return false;
        }
    }

    //获取当前背包内空置位置的数量
    public int BagNullNum()
    {
        int nullNum = 0;
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            //获取当前背包格子的道具ID和数量；
            string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (bagItemID == "0")
            {
                nullNum = nullNum + 1;
            }
        }
        return nullNum;
    }


    //获取当前仓库内空置位置的数量
    public int StoreHouseNullNum()
    {
        int nullNum = 0;
        for (int i = Game_PublicClassVar.Get_game_PositionVar.RoseHouseStartNum; i <= Game_PublicClassVar.Get_game_PositionVar.RoseHouseEndNum; i++)
        {
            //获取当前背包格子的道具ID和数量；
            string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseStoreHouse");
            if (bagItemID == "0")
            {
                nullNum = nullNum + 1;
            }
        }
        return nullNum;
    }

    //检查背包内是否有足够数量的位置
    public bool IfBagNullNum(int needNullNum)
    {
        int nullNum = 0;
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            //获取当前背包格子的道具ID和数量；
            string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (bagItemID == "0")
            {
                nullNum = nullNum + 1;
            }
        }

        if (nullNum >= needNullNum)
        {
            return true;
        }
        else {
            return false;
        }
        
    }


    //获取当前背包内第一个空置的位置
    public string BagFirstNullNum() {

        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            //获取当前背包格子的道具ID和数量；
            string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (bagItemID == "0")
            {
                return i.ToString();
            }
        }
        return "-1";
    }


    //获取当前背包内第一个指定道具的位置
    public string ReturnBagFirstSpace(string itemID)
    {

        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            //获取当前背包格子的道具ID和数量；
            string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (bagItemID == itemID)
            {
                return i.ToString();
            }
        }

        return "-1";
    }


    /*
	//消耗一个道具在背包内的数量，并增加该道具售卖价格的金币
	public bool CostBagItemMoney(string itemID,int itemNum){
        if (CostBagItem(itemID, itemNum))
        {
            //增加制定的金币
            //string itemMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemID", "ID",itemID, "Item_Template");
            string itemMoney = "100";
            SendRewardToBag("1", int.Parse(itemMoney));
            return true;
        }
        else {
            return false;
        }
	}
    */
    //出售制定背包格子的道具
    public bool SellBagSpaceItemToMoney(string spaceID) {
        string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
        if(itemID!="0"){
            string itemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceID, "RoseBag");
            string itemHide = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", spaceID, "RoseBag");
            string itemMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", itemID, "Item_Template");

            float itemMoneyValue = float.Parse(itemMoney);
            //判定是否有珍宝属性
            float hintSkillProValue = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideSkillValue(itemHide, "901");
            if (hintSkillProValue!=0)
            {
                itemMoneyValue = int.Parse(itemMoney) * hintSkillProValue;
            }

            int sellValue = (int)(itemMoneyValue) * int.Parse(itemNum);

            //删除背包内的道具
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData ("ItemID", "0", "ID", spaceID, "RoseBag");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData ("ItemNum","0", "ID", spaceID, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceID, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", spaceID, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", spaceID, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", spaceID, "RoseBag");
			Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml ("RoseBag");

            //发送货币
            SendRewardToBag("1", sellValue,"0",0,"0",true,"17");

            //记录出售数据
            string StoreSellItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (StoreSellItemListText == "")
            {
                StoreSellItemListText = itemID + "," + itemNum + "," + itemHide;
            }
            else {
                StoreSellItemListText = itemID + "," + itemNum + "," + itemHide + ";" + StoreSellItemListText;
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemID", StoreSellItemListText, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            
            //更新回购界面UI
            Game_PublicClassVar.Get_game_PositionVar.UpdataSellUIStatus = true;

            //更新背包立即显示
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

            //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
            if (itemType == "1")
            {
                Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
            }
            return true;
        }else{
            return false;
        }
    }


    //出售制定背包格子的道具
    public bool DeleteBagSpaceItem(string spaceID)
    {
        string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
        if (itemID != "0")
        {
            
            string itemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceID, "RoseBag");
            string itemHide = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", spaceID, "RoseBag");
            /*
            string itemMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", itemID, "Item_Template");
            int sellValue = int.Parse(itemMoney) * int.Parse(itemNum);
            */
            //删除背包内的道具
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceID, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceID, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceID, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", spaceID, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", spaceID, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", spaceID, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");

            //发送货币
            //SendRewardToBag("1", sellValue);

            //记录出售数据
            string StoreSellItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (StoreSellItemListText == "")
            {
                StoreSellItemListText = itemID + "," + itemNum + "," + itemHide;
            }
            else
            {
                StoreSellItemListText = itemID + "," + itemNum + "," + itemHide + ";" + StoreSellItemListText;
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemID", StoreSellItemListText, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            //更新回购界面UI
            Game_PublicClassVar.Get_game_PositionVar.UpdataSellUIStatus = true;

            //更新背包立即显示
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

            //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
            if (itemType == "1")
            {
                Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }



    //出售制定背包格子的指定数量的
    public bool DeleteBagSpaceItem_Num(string spaceID,int saleNum)
    {
        string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
        string itemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceID, "RoseBag");
        if (int.Parse(itemNum) < saleNum) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_303");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("删除指定数量不足！");
            return false;
        }

        if (itemID != "0")
        {
            string itemHide = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", spaceID, "RoseBag");

            int nowItemNum = int.Parse(itemNum) - saleNum;
            if (nowItemNum <0) {
                nowItemNum = 0;
            }

            //删除背包内的道具
            if (nowItemNum == 0)
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceID, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceID, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceID, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", spaceID, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", spaceID, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", spaceID, "RoseBag");
            }
            else {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", nowItemNum.ToString(), "ID", spaceID, "RoseBag");
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");

            //记录出售数据
            string StoreSellItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (StoreSellItemListText == "")
            {
                StoreSellItemListText = itemID + "," + saleNum + "," + itemHide;
            }
            else
            {
                StoreSellItemListText = itemID + "," + saleNum + "," + itemHide + ";" + StoreSellItemListText;
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemID", StoreSellItemListText, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            //更新回购界面UI
            Game_PublicClassVar.Get_game_PositionVar.UpdataSellUIStatus = true;

            //更新背包立即显示
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

            //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
            if (itemType == "1")
            {
                Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }



    //一键出售背包对应类型的道具
    public void SellBagYiJianItemToMoney(string sellType)
    {

        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            string spaceID = i.ToString();
            //获取道具ID
            string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
            if (itemID != "0") {
                //获取道具类型
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
                //获取道具品质
                string itemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", itemID, "Item_Template");

                bool ifSellStatus = false;
                //判断出售类型
                if (itemType == sellType)
                {
                    //判断出售品质
                    if (int.Parse(itemQuality) <= 2)
                    {
                        ifSellStatus = true;
                    }

                    if (itemType == "1") {
                        string ItemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
                        if (ItemSubType != "5") {
                            ifSellStatus = false;
                        }
                    }
                }

                if (ifSellStatus) {
                    string itemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceID, "RoseBag");
                    string itemHide = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", spaceID, "RoseBag");
                    string itemMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", itemID, "Item_Template");
                    int sellValue = int.Parse(itemMoney) * int.Parse(itemNum);
                    //删除背包内的道具
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceID, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceID, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceID, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", spaceID, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", spaceID, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", spaceID, "RoseBag");
                    
                    //发送货币
                    SendRewardToBag("1", sellValue,"0",0,"0",true,"19");

                    //记录出售数据
                    string StoreSellItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    if (StoreSellItemListText == "")
                    {
                        StoreSellItemListText = itemID + "," + itemNum + "," + itemHide;
                    }
                    else
                    {
                        StoreSellItemListText = itemID + "," + itemNum + "," + itemHide + ";" + StoreSellItemListText;
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemID", StoreSellItemListText, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    

                    //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
                    if (itemType == "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
                    }
                }
            }
        }

        //更新回购界面UI
        Game_PublicClassVar.Get_game_PositionVar.UpdataSellUIStatus = true;
        //更新背包立即显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        //存储角色
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

    }

    //消除指定回购道具里的值,参数填入文本,例如1000001.20
    public bool RemoveSellItemID(string removeItemText) {
        string StoreSellItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] StoreSellItemList = StoreSellItemListText.Split(';');
        string writeText = "";
        for (int i = 0; i <= StoreSellItemList.Length - 1; i++) {
            if (StoreSellItemList[i] == removeItemText)
            {

            }
            else {
                writeText = writeText + StoreSellItemList[i]+";";
            }
        }

        if (writeText != "") {
            writeText = writeText.Substring(0, writeText.Length - 1);
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemID", writeText, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

        return true;

    }


	//更新当前角色属性(是否提示属性变化)
	public bool  UpdateRoseProperty(bool ifHint = false) {
        //Debug.LogError("更新了数据更新了数据更新了数据更新了数据更新了数据更新了数据更新了数据更新了数据更新了数据更新了数据更新了数据更新了数据");
        //System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
        //Debug.LogError(st.GetFrame(1).GetMethod().Name.ToString());
        int rose_Hp = 0;                    //初始化血量
		int rose_ActMin = 0;                //初始化最低攻击
		int rose_ActMax = 0;                //初始化最大攻击
        int rose_MagActMin = 0;             //初始化最低攻击
        int rose_MagActMax = 0;             //初始化最大攻击
		int rose_DefMin = 0;                //初始化最小物防
		int rose_DefMax = 0;                //初始化最大物防
		int rose_AdfMin = 0;                //初始化最小魔防
		int rose_AdfMax = 0;                //初始化最大魔防
        float rose_MoveSpeed = 0.0f;        //初始化角色速度

        int rose_CriRating = 0;             //初始暴击等级
        int rose_ResRating = 0;             //初始韧性等级
        int rose_HitRating = 0;             //初始命中等级
        int rose_DodgeRating = 0;           //初始闪避等级

        float rose_CriRatingToPro = 0;      //附加暴击等级转换的暴击概率
        float rose_ResRatingToPro = 0;      //附加韧性等级转换的韧性概率
        float rose_HitRatingToPro = 0;      //附加命中等级转换的命中概率
        float rose_DodgeRatingToPro = 0;    //附加闪避等级转换的闪避概率

		float rose_Cri = 0.0f;              //初始暴击值
        float rose_Resilience = 0.0f;       //初始韧性值
        float rose_Hit = 0.0f;              //初始命中值
		float rose_Dodge = 0.0f;            //初始闪避值
        float rose_MagicRebound = 0.0f;     //法术反击值
        float rose_ActRebound = 0.0f;       //攻击反击

        float rose_Resistance_1 = 0;                //光抗性
        float rose_Resistance_2 = 0;                //暗抗性
        float rose_Resistance_3 = 0;                //火抗性
        float rose_Resistance_4 = 0;                //水抗性
        float rose_Resistance_5 = 0;                //电抗性

        float rose_RaceResistance_1 = 0;            //野兽攻击抗性
        float rose_RaceResistance_2 = 0;            //人物攻击抗性
        float rose_RaceResistance_3 = 0;            //恶魔攻击抗性
		float rose_RaceDamge_1 = 0;            		//野兽攻击抗性
		float rose_RaceDamge_2 = 0;            		//人物攻击抗性
		float rose_RaceDamge_3 = 0;            		//恶魔攻击抗性

        float rose_Boss_ActAdd;                     //Boss普通攻击加成
        float rose_Boss_SkillAdd;                   //Boss技能攻击加成
        float rose_Boss_ActHitCost;                 //受到Boss普通攻击减免
        float rose_Boss_SkillHitCost;               //受到Boss技能攻击减免
        float rose_PetActAdd;                           //宠物攻击加成
        float rose_PetActHitCost;                       //宠物受伤减免
        float rose_SkillCDTimePro;                      //技能冷却时间缩减
        float rose_BuffTimeAddPro;                      //自身buff效果延长
        float rose_DeBuffTimeCostPro;                   //Debuff时间缩短
        float rose_DodgeAddHpPro;                       //闪避恢复血量

        int rose_GeDangValue = 0;                    //格挡值
        float rose_ZhongJiPro = 0.0f;                //重击概率
        int rose_ZhongJiValue = 0;                   //重击附加伤害值
        int rose_GuDingValue = 0;                    //每次普通攻击附加的伤害值
        int rose_HuShiDefValue = 0;                  //忽视目标防御值_固定值                       
        int rose_HuShiAdfValue = 0;                  //忽视目标魔防值_固定值
        float rose_HuShiDefValuePro = 0;             //忽视目标防御值_百分比                       
        float rose_HuShiAdfValuePro = 0;             //忽视目标魔防值_百分比
        float rose_XiXuePro = 0.0f;                  //吸血概率
        float rose_ActAddPro = 0.0f;                 //普通伤害加成


		float rose_DefAdd = 0.0f;                   //初始物理免伤
		float rose_AdfAdd = 0.0f;                   //初始魔法免伤
		float rose_DamgeSub = 0.0f;                 //初始总免伤
        float rose_DamgeAdd = 0.0f;                 //初始总免伤
        float rose_Lucky = 0;                       //幸运

        float rose_Exp_AddPro = 0.0f;               //经验加成
        int rose_Exp_AddValue = 0;                  //经验固定加成
        float rose_Gold_AddPro = 0.0f;              //金币加成
        int rose_Gold_AddValue = 0;                 //金币固定加成

        float rose_Blessing_AddPro = 0.0f;          //洗炼极品掉落
        float rose_HidePro_AddPro = 0.0f;           //隐藏属性出现概率
        float rose_GemHole_AddPro = 0.0f;           //装备上的宝石槽位出现概率

        int rose_YaoJiValue = 0;                    //药剂类熟练度
        int rose_DuanZaoValue = 0;				    //锻造类熟练度 

		float rose_FuHuoPro = 0.0f;          		//复活
		float rose_ActWuShi = 0.0f; 				//攻击无视防御
		float rose_ShenNong = 0.0f;              	//神农
		float rose_DropExtra = 0.0f;          	    //额外掉落
		float rose_WeiZhuang = 0.0f;           	    //伪装  +增大发现范围   -缩小范围
		float rose_ZaiNanValue = 0.0f;              //灾难
		float rose_ShiXuePro = 0.0f;				//嗜血概率

        int rose_HealHpValue = 0;                   //角色恢复的固定值
        float rose_HealHpPro = 0.0f;                //角色恢复的百分比加成固定值
        float rose_HealHpFightPro = 0.0f;           //角色战斗时恢复额外恢复血量的百分比
        float rose_AITuoZhanDisValue = 0.0f;		//怪物脱战距离
        float rose_ZhuanZhuPro = 0;					//专注概率


        float rose_BiZhongPro = 0.0f;		        //怪物脱战距离
        float rose_YaoJiCirPro = 0;					//生产药剂暴击概率
        float rose_BuZhuoPro = 0;                   //生产药剂暴击概率

        float rose_LanValueMaxAdd;                       //魔法量附加
        float rose_SummonAIPropertyAddPro;               //召唤生物属性加成
        float rose_HuDunValueAddPro;                     //护盾属性附加
        float rose_SummonAIHpPropertyAddPro;                //召唤生物属性血量加成
        float rose_SummonAIActPropertyAddPro;               //召唤生物属性攻击加成
        float rose_SummonAIDefPropertyAddPro;               //召唤生物属性防御加成

        Rose_Proprety rose_Proprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
		Function_DataSet functionDataSet = Game_PublicClassVar.Get_function_DataSet;
		WWWSet wwwSet = Game_PublicClassVar.Get_wwwSet;

		//读取角色名称
		string rose_Name = functionDataSet.DataSet_ReadData("Name","ID", wwwSet.RoseID,"RoseData");
		rose_Proprety.Rose_Name = rose_Name;
		
		//获取角色上次保存等级
		int rose_Lv = int.Parse(functionDataSet.DataSet_ReadData("Lv","ID", wwwSet.RoseID,"RoseData"));
		rose_Proprety.Rose_Lv = rose_Lv;

        //清除技能附加值
        SkilllAddValueClean();
		
		//------------------------------------更新血量
		//获取职业属性
		string 	rose_Occupation = functionDataSet.DataSet_ReadData("RoseOccupation", "ID", wwwSet.RoseID,"RoseData");
		rose_Proprety.Rose_Occupation = rose_Occupation;

		//获取装备属性
		int hp_Equip = 0;
		int act_EquipMin = 0;
		int act_EquipMax = 0;
        int magact_EquipMin = 0;
        int magact_EquipMax = 0;
		int def_EquipMin = 0; 
		int def_EquipMax = 0;
		int adf_EquipMin = 0;
		int adf_EquipMax = 0;
        float cir_Equip = 0;
        float hit_Equip = 0;
        float dodge_Equip = 0;
        float damgeAdd_Equip = 0;
        float damgeSub_Equip = 0;
        float speed_Equip = 0;
        int lucky_Equip = 0;

        float defAdd_Equip = 0;
        float adfAdd_Equip = 0;
        float hpPro_Equip = 0;
        float actPro_Equip = 0;
        float magactPro_Equip = 0;
        float defPro_Equip = 0;
        float adfPro_Equip = 0;

        int geDangValue_Equip = 0;                    //格挡值
        float zhongJiPro_Equip = 0.0f;                //重击概率
        int zhongJiValue_Equip = 0;                   //重击附加伤害值
        int guDingValue_Equip = 0;                    //每次普通攻击附加的伤害值
        int huShiDefValue_Equip = 0;                  //忽视目标防御值                       
        int huShiAdfValue_Equip = 0;                  //忽视目标魔防值
        float huShiDefValuePro_Equip = 0;             //忽视目标防御值                       
        float huShiAdfValuePro_Equip = 0;             //忽视目标魔防值
        float xiXuePro_Equip = 0.0f;                  //吸血概率

        int criRating_Equip = 0;          		     //初始暴击等级
        int resilienceRating_Equip = 0;   		     //初始韧性等级
        int hitRating_Equip = 0;          		     //初始命中等级
        int dodgeRating_Equip = 0;        		     //初始闪避等级
        float resilience_Equip = 0.0f;       	     //初始韧性值
        float magicRebound_Equip = 0.0f;     	     //法术反击值
        float actRebound_Equip = 0.0f;       	     //攻击反击
        float resistance_1_Equip = 0;                //光抗性
        float resistance_2_Equip = 0;                //暗抗性
        float resistance_3_Equip = 0;                //火抗性
        float resistance_4_Equip = 0;                //水抗性
        float resistance_5_Equip = 0;                //电抗性
        float raceResistance_1_Equip = 0;            //野兽攻击抗性
        float raceResistance_2_Equip = 0;            //人物攻击抗性
        float raceResistance_3_Equip = 0;            //恶魔攻击抗性
		float raceDamge_1_Equip = 0;            	 //野兽攻击抗性
		float raceDamge_2_Equip = 0;            	 //人物攻击抗性
		float raceDamge_3_Equip = 0;                 //恶魔攻击抗性
        float rose_Boss_ActAdd_Equip = 0;                      //Boss普通攻击加成
        float rose_Boss_SkillAdd_Equip = 0;                    //Boss技能攻击加成
        float rose_Boss_ActHitCost_Equip = 0;                  //受到Boss普通攻击减免
        float rose_Boss_SkillHitCost_Equip = 0;                //受到Boss技能攻击减免
        float rose_PetActAdd_Equip = 0;                        //宠物攻击加成
        float rose_PetActHitCost_Equip = 0;                    //宠物受伤减免
        float rose_SkillCDTimePro_Equip = 0;                      //技能冷却时间缩减
        float rose_BuffTimeAddPro_Equip = 0;                      //自身buff效果延长
        float rose_DeBuffTimeCostPro_Equip = 0;                   //Debuff时间缩短
        float rose_DodgeAddHpPro_Equip = 0;                       //闪避恢复血量

        float exp_AddPro_Equip = 0.0f;               //经验加成
        int exp_AddValue_Equip = 0;                  //经验固定加成
        float gold_AddPro_Equip = 0.0f;              //金币加成
        int gold_AddValue_Equip = 0;                 //金币固定加成
        float blessing_AddPro_Equip = 0.0f;          //洗炼极品掉落
        float hidePro_AddPro_Equip = 0.0f;           //隐藏属性出现概率
        float gemHole_AddPro_Equip = 0.0f;           //装备上的宝石槽位出现概率

        int rose_YaoJiValue_Equip = 0;                      //药剂类熟练度
        int rose_DuanZaoValue_Equip = 0;				    //锻造类熟练度 
        float rose_FuHuoPro_Equip = 0.0f;          		    //复活
        float rose_ActWuShi_Equip = 0.0f; 				    //攻击无视防御
        float rose_ShenNong_Equip = 0.0f;              	    //神农
        float rose_DropExtra_Equip = 0.0f;          	    //额外掉落
        float rose_WeiZhuang_Equip = 0.0f;           	    //伪装  +增大发现范围   -缩小范围
        float rose_ZaiNanValue_Equip = 0.0f;                //灾难
        float rose_ShiXuePro_Equip = 0.0f;				    //嗜血概率

        int rose_HealHpValue_Equip = 0;                   //角色恢复的固定值
        float rose_HealHpPro_Equip = 0.0f;                //角色恢复的百分比加成固定值
        float rose_HealHpFightPro_Equip = 0.0f;           //角色战斗时恢复额外恢复血量的百分比
        float rose_AITuoZhanDisValue_Equip = 0.0f;		  //怪物脱战距离
        float rose_ZhuanZhuPro_Equip = 0;				  //专注概率
        float rose_BiZhongPro_Equip = 0.0f;		          //怪物脱战距离
        float rose_YaoJiCirPro_Equip = 0;			      //生产药剂暴击概率
        float rose_BuZhuoPro_Equip = 0;                   //捕捉概率

        float rose_LanValueMaxAdd_Equip = 0;                       //魔法量附加
        float rose_SummonAIPropertyAddPro_Equip = 0;               //召唤生物属性加成
        float rose_HuDunValueAddPro_Equip = 0;                     //护盾属性附加

        //套装字符串
        string equipSuitIDStr = "";

		//循环自身的装备 和十二生肖
		for (int i = 1; i <= 25; i++) {

			string equipID_1 = "0";
            if (i <= 13) { 
                equipID_1 = functionDataSet.DataSet_ReadData("EquipItemID", "ID", i.ToString(), "RoseEquip");
            }

            if (i >= 14 && i <= 25) {

                string EquipIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShengXiaoSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                //Debug.Log("EquipIDStr = " + EquipIDStr + ";");
                if (EquipIDStr != "" && EquipIDStr != "0" && EquipIDStr != null)
                {
                    string[] EquipIDList = EquipIDStr.Split(';');
                    equipID_1 = EquipIDList[i - 14];
                    //Debug.Log("EquipID = " + EquipID);
                }

            }

            if (equipID_1!="0"&& equipID_1 != "")
            {

				string equipID = functionDataSet.DataSet_ReadData("ItemEquipID", "ID", equipID_1,"Item_Template");
                int hp_Equip_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Hp", "ID", equipID, "Equip_Template"));
				int act_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAct", "ID", equipID, "Equip_Template"));
				int act_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAct", "ID", equipID, "Equip_Template"));
                int magact_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinMagAct", "ID", equipID, "Equip_Template"));
                int magact_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxMagAct", "ID", equipID, "Equip_Template"));
				int def_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinDef", "ID", equipID, "Equip_Template"));
				int def_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxDef", "ID", equipID, "Equip_Template"));
				int adf_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAdf", "ID", equipID, "Equip_Template"));
				int adf_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAdf", "ID", equipID, "Equip_Template"));
                float cir_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Cri", "ID", equipID, "Equip_Template"));
                float hit_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Hit", "ID", equipID, "Equip_Template"));
                float dodge_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Dodge", "ID", equipID, "Equip_Template"));
                float damgeAdd_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeAdd", "ID", equipID, "Equip_Template"));
                float damgeSub_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeSub", "ID", equipID, "Equip_Template"));
                float speed_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Speed", "ID", equipID, "Equip_Template"));
                int lucky_Equip_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Lucky", "ID", equipID, "Equip_Template"));

                float defAdd_Equip_value = 0;
                float adfAdd_Equip_value = 0;
                float hpPro_Equip_value = 0;
                float actPro_Equip_value = 0;
                float magactPro_Equip_value = 0;
                float defPro_Equip_value = 0;
                float adfPro_Equip_value = 0;

                int geDangValue_Equip_value = 0;                    //格挡值
                float zhongJiPro_Equip_value = 0.0f;                //重击概率
                int zhongJiValue_Equip_value = 0;                   //重击附加伤害值
                int guDingValue_Equip_value = 0;                    //每次普通攻击附加的伤害值
                int huShiDefValue_Equip_value = 0;                  //忽视目标防御值                       
                int huShiAdfValue_Equip_value = 0;                  //忽视目标魔防值
                float huShiDefValuePro_Equip_value = 0;             //忽视目标防御值                       
                float huShiAdfValuePro_Equip_value = 0;             //忽视目标魔防值
                float xiXuePro_Equip_value = 0.0f;                  //吸血概率

                int criRating_Equip_value = 0;                      //暴击等级
                int resilienceRating_Equip_value = 0;               //韧性等级
                int hitRating_Equip_value = 0;                      //命中等级
                int dodgeRating_Equip_value = 0;                    //闪避等级
                float resilience_Equip_value = 0.0f;                //韧性概率
                float magicRebound_Equip_value = 0.0f;              //法术反击值
                float actRebound_Equip_value = 0.0f;                //攻击反击
                float resistance_1_Equip_value = 0;                 //光抗性
                float resistance_2_Equip_value = 0;                 //暗抗性
                float resistance_3_Equip_value = 0;                 //火抗性
                float resistance_4_Equip_value = 0;                 //水抗性
                float resistance_5_Equip_value = 0;                 //电抗性
                float raceResistance_1_Equip_value = 0;             //野兽攻击抗性
                float raceResistance_2_Equip_value = 0;             //人物攻击抗性
                float raceResistance_3_Equip_value = 0;             //恶魔攻击抗性
				float raceDamge_1_Equip_value = 0;                  //野兽攻击抗性
				float raceDamge_2_Equip_value = 0;                  //人物攻击抗性
				float raceDamge_3_Equip_value = 0;                  //恶魔攻击抗性

                float rose_Boss_ActAdd_Equip_value = 0;                      //Boss普通攻击加成
                float rose_Boss_SkillAdd_Equip_value = 0;                    //Boss技能攻击加成
                float rose_Boss_ActHitCost_Equip_value = 0;                  //受到Boss普通攻击减免
                float rose_Boss_SkillHitCost_Equip_value = 0;                //受到Boss技能攻击减免
                float rose_PetActAdd_Equip_value = 0;                        //宠物攻击加成
                float rose_PetActHitCost_Equip_value = 0;                    //宠物受伤减免
                float rose_SkillCDTimePro_Equip_value = 0;                      //技能冷却时间缩减
                float rose_BuffTimeAddPro_Equip_value = 0;                      //自身buff效果延长
                float rose_DeBuffTimeCostPro_Equip_value = 0;                   //Debuff时间缩短
                float rose_DodgeAddHpPro_Equip_value = 0;                       //闪避恢复血量

                float exp_AddPro_Equip_value = 0.0f;               //经验加成
                float gold_AddPro_Equip_value = 0.0f;              //金币加成
                int exp_AddValue_Equip_value = 0;                  //经验加成
                int gold_AddValue_Equip_value = 0;                 //金币加成
                float blessing_AddPro_Equip_value = 0.0f;          //洗炼极品掉落
                float hidePro_AddPro_Equip_value = 0.0f;           //隐藏属性出现概率
                float gemHole_AddPro_Equip_value = 0.0f;           //装备上的宝石槽位出现概率

                int rose_YaoJiValue_Equip_value = 0;                    //药剂类熟练度
                int rose_DuanZaoValue_Equip_value = 0;				    //锻造类熟练度 
                float rose_FuHuoPro_Equip_value = 0.0f;          		//复活
                float rose_ActWuShi_Equip_value = 0.0f; 				//攻击无视防御
                float rose_ShenNong_Equip_value = 0.0f;              	//神农
                float rose_DropExtra_Equip_value = 0.0f;          	    //额外掉落
                float rose_WeiZhuang_Equip_value = 0.0f;           	    //伪装  +增大发现范围   -缩小范围
                float rose_ZaiNanValue_Equip_value = 0.0f;              //灾难
                float rose_ShiXuePro_Equip_value = 0.0f;				//嗜血概率

                int rose_HealHpValue_Equip_value = 0;                   //角色恢复的固定值
                float rose_HealHpPro_Equip_value = 0.0f;                //角色恢复的百分比加成固定值
                float rose_HealHpFightPro_Equip_value = 0.0f;           //角色战斗时恢复额外恢复血量的百分比
                float rose_AITuoZhanDisValue_Equip_value = 0.0f;		//怪物脱战距离
                float rose_ZhuanZhuPro_Equip_value = 0;					//专注概率
                float rose_BiZhongPro_Equip_value = 0.0f;		        //怪物脱战距离
                float rose_YaoJiCirPro_Equip_value = 0;					//生产药剂暴击概率
                float rose_BuZhuoPro_Equip_value = 0;                   //捕捉概率

                float rose_LanValueMaxAdd_Equip_value = 0;                       //魔法量附加
                float rose_SummonAIPropertyAddPro_Equip_value = 0;               //召唤生物属性加成
                float rose_HuDunValueAddPro_Equip_value = 0;                     //护盾属性附加

                //获取极品ID
                string equipHideID = functionDataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseEquip");

                //判定装备是否有极品和虚弱属性
                float hintSkillProValue_JiPin = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideSkillValue(equipHideID, "903");
                float hintSkillProValue_XuRuo = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideSkillValue(equipHideID, "906");
                float hintSkillProValue_Quanneng = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideSkillValue(equipHideID, "910");

                float hintSkillProValue = hintSkillProValue_Quanneng - hintSkillProValue_XuRuo;

                if (hintSkillProValue != 0|| hintSkillProValue_JiPin!=0)
                {
                    hintSkillProValue = 1 + hintSkillProValue;

                    //属性成长
                    hp_Equip_value = (int)(hp_Equip_value * hintSkillProValue);
                    act_EquipMin_value = (int)(act_EquipMin_value * (hintSkillProValue + hintSkillProValue_JiPin));
                    act_EquipMax_value = (int)(act_EquipMax_value * (hintSkillProValue + hintSkillProValue_JiPin));
                    magact_EquipMin_value = (int)(magact_EquipMin_value * (hintSkillProValue + hintSkillProValue_JiPin));
                    magact_EquipMax_value = (int)(magact_EquipMax_value * (hintSkillProValue + hintSkillProValue_JiPin));
                    def_EquipMin_value = (int)(def_EquipMin_value * hintSkillProValue);
                    def_EquipMax_value = (int)(def_EquipMax_value * hintSkillProValue);
                    adf_EquipMin_value = (int)(adf_EquipMin_value * hintSkillProValue);
                    adf_EquipMax_value = (int)(adf_EquipMax_value * hintSkillProValue);

                }

                //判定装备是否有胜算属性
                float hintSkillProValue_ShengSuan = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideSkillValue(equipHideID, "904");
                if (hintSkillProValue_ShengSuan != 0)
                {
                    act_EquipMin_value = act_EquipMax_value;
                    magact_EquipMin_value = magact_EquipMax_value;
                    def_EquipMin_value = def_EquipMax_value;
                    adf_EquipMin_value = adf_EquipMax_value;
                }

                //获取其他属性
                string AddPropreListStrValue = functionDataSet.DataSet_ReadData("AddPropreListStr", "ID", equipID, "Equip_Template");
                //Debug.Log("AddPropreListStrValue = " + AddPropreListStrValue + ";equipID = " + equipID);

                if (AddPropreListStrValue != "" && AddPropreListStrValue != "0") {

                    string[] AddPropreListStr = AddPropreListStrValue.Split(';');
                    //Debug.Log("AddPropreListStr = " + AddPropreListStr.Length + "; equipID = " + equipID);
                    if (AddPropreListStr.Length > 0)
                    {
                        for (int y = 0; y < AddPropreListStr.Length; y++)
                        {
                            string proprety = AddPropreListStr[y].Split(',')[0];
                            string propretyValue = AddPropreListStr[y].Split(',')[1];
                            switch (proprety)
                            {

                                //血量百分比
                                case "50":
                                    hpPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //物理攻击(百分比)
                                case "51":
                                    actPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //魔法攻击(百分比)
                                case "52":
                                    magactPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //物理防御(百分比)
                                case "53":
                                    defPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //魔法防御(百分比)
                                case "54":
                                    adfPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //格挡值
                                case "101":
                                    geDangValue_Equip_value = int.Parse(propretyValue);
                                    break;

                                //重击概率
                                case "111":
                                    zhongJiPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //重击附加伤害值
                                case "112":
                                    zhongJiValue_Equip_value = int.Parse(propretyValue);
                                    break;

                                //每次普通攻击附加的伤害值
                                case "121":
                                    guDingValue_Equip_value = int.Parse(propretyValue);
                                    break;

                                //忽视目标防御值
                                case "131":
                                    huShiDefValue_Equip_value = int.Parse(propretyValue);
                                    break;

                                //忽视目标魔防值
                                case "132":
                                    huShiAdfValue_Equip_value = int.Parse(propretyValue);
                                    break;

                                //忽视目标防御值
                                case "133":
                                    huShiDefValuePro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //忽视目标魔防值
                                case "134":
                                    huShiAdfValuePro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //吸血概率
                                case "141":
                                    xiXuePro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //法术反击
                                case "151":
                                    magicRebound_Equip_value = float.Parse(propretyValue);
                                    break;

                                //攻击反击
                                case "152":
                                    actRebound_Equip_value = float.Parse(propretyValue);
                                    break;

                                //韧性概率
                                case "161":
                                    resilience_Equip_value = float.Parse(propretyValue);
                                    break;

                                //回血百分比
                                case "171":
                                    rose_HealHpPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //回血固定值
                                case "172":
                                    rose_HealHpValue_Equip_value = int.Parse(propretyValue);
                                    break;

                                //战斗回血比例
                                case "173":
                                    rose_HealHpFightPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //暴击等级
                                case "201":
                                    criRating_Equip_value = int.Parse(propretyValue);
                                    break;

                                //韧性等级
                                case "202":
                                    resilienceRating_Equip_value = int.Parse(propretyValue);
                                    break;

                                //命中等级
                                case "203":
                                    hitRating_Equip_value = int.Parse(propretyValue);
                                    break;

                                //闪避等级
                                case "204":
                                    dodgeRating_Equip_value = int.Parse(propretyValue);
                                    break;

                                //光抗性
                                case "301":
                                    resistance_1_Equip_value = float.Parse(propretyValue);
                                    break;

                                //暗抗性
                                case "302":
                                    resistance_2_Equip_value = float.Parse(propretyValue);
                                    break;

                                //火抗性
                                case "303":
                                    resistance_3_Equip_value = float.Parse(propretyValue);
                                    break;

                                //水抗性
                                case "304":
                                    resistance_4_Equip_value = float.Parse(propretyValue);
                                    break;

                                //电抗性
                                case "305":
                                    resistance_5_Equip_value = float.Parse(propretyValue);
                                    break;

                                //野兽攻击抗性
                                case "321":
                                    raceResistance_1_Equip_value = float.Parse(propretyValue);
                                    break;

                                //人物攻击抗性
                                case "322":
                                    raceResistance_2_Equip_value = float.Parse(propretyValue);
                                    break;

                                //恶魔攻击抗性
                                case "323":
                                    raceResistance_3_Equip_value = float.Parse(propretyValue);
                                    break;

								//野兽攻击抗性
								case "331":
									raceDamge_1_Equip_value = float.Parse(propretyValue);
									break;

								//人物攻击抗性
								case "332":
									raceDamge_2_Equip_value = float.Parse(propretyValue);
									break;

								//恶魔攻击抗性
								case "333":
									raceDamge_3_Equip_value = float.Parse(propretyValue);
									break;

                                //Boss普通攻击加成
                                case "341":
                                    rose_Boss_ActAdd_Equip_value = float.Parse(propretyValue);
                                    break;

                                //Boss技能攻击加成
                                case "342":
                                    rose_Boss_SkillAdd_Equip_value = float.Parse(propretyValue);
                                    break;
                                
                                //受到Boss普通攻击减免
                                case "343":
                                    rose_Boss_ActHitCost_Equip_value = float.Parse(propretyValue);
                                    break;
                                
                                //受到Boss技能攻击减免
                                case "344":
                                    rose_Boss_SkillHitCost_Equip_value = float.Parse(propretyValue);
                                    break;

                                //宠物攻击加成
                                case "345":
                                    rose_PetActAdd_Equip_value = float.Parse(propretyValue);
                                    break;

                                //宠物受伤减免
                                case "346":
                                    rose_PetActHitCost_Equip_value = float.Parse(propretyValue);
                                    break;

                                //技能冷却时间缩减
                                case "347":
                                    rose_SkillCDTimePro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //自身buff效果延长
                                case "348":
                                    rose_BuffTimeAddPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //Debuff时间缩短
                                case "349":
                                    rose_DeBuffTimeCostPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //闪避恢复血量
                                case "350":
                                    rose_DodgeAddHpPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //魔法量附加
                                case "351":
                                    rose_LanValueMaxAdd_Equip_value = float.Parse(propretyValue);
                                    break;

                                //召唤生物属性加成
                                case "352":
                                    rose_SummonAIPropertyAddPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //护盾属性附加
                                case "353":
                                    rose_HuDunValueAddPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //经验加成
                                case "401":
                                    exp_AddPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //金币加成
                                case "402":
                                    gold_AddPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //洗炼极品掉落（祝福值）
                                case "403":
                                    blessing_AddPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //装备隐藏属性出现概率
                                case "404":
                                    hidePro_AddPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //装备上的宝石槽位出现概率
                                case "405":
                                    gemHole_AddPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //经验加成固定
                                case "406":
                                    exp_AddValue_Equip_value = int.Parse(propretyValue);
                                    break;

                                //金币加成固定
                                case "407":
                                    gold_AddValue_Equip_value = int.Parse(propretyValue);
                                    break;
                                //药剂类熟练度
                                case "408":
                                    rose_YaoJiValue_Equip_value = int.Parse(propretyValue);                      
                                    break;
                                //锻造类熟练度
                                case "409":
                                    rose_DuanZaoValue_Equip_value = int.Parse(propretyValue);				    
                                    break;
                                //复活
                                case "411":
                                    rose_FuHuoPro_Equip_value = float.Parse(propretyValue);          		    
                                    break;
                                //攻击无视防御
                                case "412":
                                    rose_ActWuShi_Equip_value = float.Parse(propretyValue); 				    
                                    break;
                                //神农
                                case "413":
                                    rose_ShenNong_Equip_value = float.Parse(propretyValue);              	    
                                    break;
                                //额外掉落
                                case "414":
                                    rose_DropExtra_Equip_value = float.Parse(propretyValue);          	    
                                    break;
                                //伪装  +增大发现范围   -缩小范围
                                case "415":
                                    rose_WeiZhuang_Equip_value = float.Parse(propretyValue);           	    
                                    break;
                                //灾难
                                case "416":
                                    rose_ZaiNanValue_Equip_value = float.Parse(propretyValue);                
                                    break;
                                //嗜血概率
                                case "417":
                                    rose_ShiXuePro_Equip_value = float.Parse(propretyValue);				    
                                    break;

                                //怪物脱战距离
                                case "418":
                                    rose_AITuoZhanDisValue_Equip_value = float.Parse(propretyValue);
                                    break;

                                //专注概率
                                case "419":
                                    rose_ZhuanZhuPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //必中
                                case "420":
                                    rose_BiZhongPro_Equip_value = float.Parse(propretyValue);
                                    break;

                                //生产药剂暴击概率
                                case "421":
                                    rose_YaoJiCirPro_Equip_value = float.Parse(propretyValue);
                                    break;
                                //捕捉概率
                                case "422":
                                    rose_BuZhuoPro_Equip_value = float.Parse(propretyValue);
                                    break;
                            }
                        }
                    } 
                }



                //获取极品属性
                string hideID = functionDataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseEquip");
                if (hideID != "0") {

                    string hideProperListStr = functionDataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");

                    //替换字符 FoMo,
                    hideProperListStr = hideProperListStr.Replace("FuMo,", "");
                    string[] hideProperty = hideProperListStr.Split(';');

                    //隐藏属性
                    /*
                    1:血量上限
                    2:物理攻击最大值
                    3:物理防御最大值
                    4:魔法防御最大值
                    */
                    //循环加入各个隐藏属性
                    if (hideProperListStr != "")
                    {
                        for (int y = 0; y <= hideProperty.Length - 1; y++)
                        {
                            string[] hidePropertyList = hideProperty[y].Split(',');
                            if (hidePropertyList.Length >= 2) {
                                string hidePropertyType = hideProperty[y].Split(',')[0];
                                string hidePropertyValue = hideProperty[y].Split(',')[1];

                                switch (hidePropertyType)
                                {
                                    //血量上限
                                    case "1":
                                        hp_Equip_value = hp_Equip_value + int.Parse(hidePropertyValue);
                                        break;
                                    //物理攻击最大值
                                    case "2":
                                        act_EquipMax_value = act_EquipMax_value + int.Parse(hidePropertyValue);
                                        break;
                                    case "3":
                                        //物理防御最大值
                                        def_EquipMax_value = def_EquipMax_value + int.Parse(hidePropertyValue);
                                        break;
                                    //魔法防御最大值
                                    case "4":
                                        adf_EquipMax_value = adf_EquipMax_value + int.Parse(hidePropertyValue);
                                        break;
                                    //魔法攻击最大值
                                    case "5":
                                        magact_EquipMax_value = magact_EquipMax_value + int.Parse(hidePropertyValue);
                                        break;
                                    //血量
                                    case "10":
                                        hp_Equip_value = hp_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //物理最小攻击
                                    case "11":
                                        act_EquipMax_value = act_EquipMax_value + int.Parse(hidePropertyValue);
                                        break;

                                    //魔法攻击
                                    case "14":
                                        magact_EquipMax_value = magact_EquipMax_value + int.Parse(hidePropertyValue);
                                        break;

                                    //物理防御
                                    case "17":
                                        def_EquipMax_value = def_EquipMax_value + int.Parse(hidePropertyValue);
                                        break;

                                    //魔法防御
                                    case "20":
                                        adf_EquipMax_value = adf_EquipMax_value + int.Parse(hidePropertyValue);
                                        break;

                                    //暴击
                                    case "30":
                                        cir_Equip_value = cir_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //命中
                                    case "31":
                                        hit_Equip_value = hit_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //闪避
                                    case "32":
                                        dodge_Equip_value = dodge_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //物理免伤
                                    case "33":
                                        defAdd_Equip_value = defAdd_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //魔法免伤
                                    case "34":
                                        adfAdd_Equip_value = adfAdd_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //速度
                                    case "35":
                                        speed_Equip_value = speed_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //伤害免伤
                                    case "36":
                                        damgeSub_Equip_value = damgeSub_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //血量百分比
                                    case "50":
                                        hpPro_Equip_value = hpPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //物理攻击(百分比)
                                    case "51":
                                        actPro_Equip_value = actPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //魔法攻击(百分比)
                                    case "52":
                                        magactPro_Equip_value = magactPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //物理防御(百分比)
                                    case "53":
                                        defPro_Equip_value = defPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //魔法防御(百分比)
                                    case "54":
                                        adfPro_Equip_value = adfPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;


                                    //幸运值
                                    case "100":
                                        lucky_Equip_value = lucky_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //格挡值
                                    case "101":
                                        geDangValue_Equip_value = geDangValue_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //重击概率
                                    case "111":
                                        zhongJiPro_Equip_value = zhongJiPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //重击附加伤害值
                                    case "112":
                                        zhongJiValue_Equip_value = zhongJiValue_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //每次普通攻击附加的伤害值
                                    case "121":
                                        guDingValue_Equip_value = guDingValue_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //忽视目标防御值
                                    case "131":
                                        huShiDefValue_Equip_value = huShiDefValue_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //忽视目标魔防值
                                    case "132":
                                        huShiAdfValue_Equip_value = huShiAdfValue_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //忽视目标防御值
                                    case "133":
                                        huShiDefValuePro_Equip_value = huShiDefValuePro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //忽视目标魔防值
                                    case "134":
                                        huShiAdfValuePro_Equip_value = huShiAdfValuePro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //吸血概率
                                    case "141":
                                        xiXuePro_Equip_value = xiXuePro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //法术反击
                                    case "151":
                                        magicRebound_Equip_value = magicRebound_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //攻击反击
                                    case "152":
                                        actRebound_Equip_value = actRebound_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //韧性概率
                                    case "161":
                                        resilience_Equip_value = resilience_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //回血百分比
                                    case "171":
                                        rose_HealHpPro_Equip_value = rose_HealHpPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //回血固定值
                                    case "172":
                                        rose_HealHpValue_Equip_value = rose_HealHpValue_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //战斗回血比例
                                    case "173":
                                        rose_HealHpFightPro_Equip_value = rose_HealHpFightPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //暴击等级
                                    case "201":
                                        criRating_Equip_value = criRating_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //韧性等级
                                    case "202":
                                        resilienceRating_Equip_value = resilienceRating_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //命中等级
                                    case "203":
                                        hitRating_Equip_value = hitRating_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //闪避等级
                                    case "204":
                                        dodgeRating_Equip_value = dodgeRating_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //光抗性
                                    case "301":
                                        resistance_1_Equip_value = resistance_1_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //暗抗性
                                    case "302":
                                        resistance_2_Equip_value = resistance_2_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //火抗性
                                    case "303":
                                        resistance_3_Equip_value = resistance_3_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //水抗性
                                    case "304":
                                        resistance_4_Equip_value = resistance_4_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //电抗性
                                    case "305":
                                        resistance_5_Equip_value = resistance_5_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //野兽攻击抗性
                                    case "321":
                                        raceResistance_1_Equip_value = raceResistance_1_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //人物攻击抗性
                                    case "322":
                                        raceResistance_2_Equip_value = raceResistance_2_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //恶魔攻击抗性
                                    case "323":
                                        raceResistance_3_Equip_value = raceResistance_3_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //野兽攻击抗性
                                    case "331":
                                        raceDamge_1_Equip_value = raceDamge_1_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //人物攻击抗性
                                    case "332":
                                        raceDamge_2_Equip_value = raceDamge_2_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //恶魔攻击抗性
                                    case "333":
                                        raceDamge_3_Equip_value = raceDamge_3_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //Boss普通攻击加成
                                    case "341":
                                        rose_Boss_ActAdd_Equip_value = rose_Boss_ActAdd_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //Boss技能攻击加成
                                    case "342":
                                        rose_Boss_SkillAdd_Equip_value = rose_Boss_SkillAdd_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //受到Boss普通攻击减免
                                    case "343":
                                        rose_Boss_ActHitCost_Equip_value = rose_Boss_ActHitCost_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //受到Boss技能攻击减免
                                    case "344":
                                        rose_Boss_SkillHitCost_Equip_value = rose_Boss_SkillHitCost_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //宠物攻击加成
                                    case "345":
                                        rose_PetActAdd_Equip_value = rose_PetActAdd_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //宠物受伤减免
                                    case "346":
                                        rose_PetActHitCost_Equip_value = rose_PetActHitCost_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //技能冷却时间缩减
                                    case "347":
                                        rose_SkillCDTimePro_Equip_value = rose_SkillCDTimePro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //自身buff效果延长
                                    case "348":
                                        rose_BuffTimeAddPro_Equip_value = rose_BuffTimeAddPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //Debuff时间缩短
                                    case "349":
                                        rose_DeBuffTimeCostPro_Equip_value = rose_DeBuffTimeCostPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //闪避恢复血量
                                    case "350":
                                        rose_DodgeAddHpPro_Equip_value = rose_DodgeAddHpPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //魔法量附加
                                    case "351":
                                        rose_LanValueMaxAdd_Equip_value = rose_LanValueMaxAdd_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //召唤生物属性加成
                                    case "352":
                                        rose_SummonAIPropertyAddPro_Equip_value = rose_SummonAIPropertyAddPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //护盾属性附加
                                    case "353":
                                        rose_HuDunValueAddPro_Equip_value = rose_HuDunValueAddPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //经验加成
                                    case "401":
                                        exp_AddPro_Equip_value = exp_AddPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //金币加成
                                    case "402":
                                        gold_AddPro_Equip_value = gold_AddPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //洗炼极品掉落（祝福值）
                                    case "403":
                                        blessing_AddPro_Equip_value = blessing_AddPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //装备隐藏属性出现概率
                                    case "404":
                                        hidePro_AddPro_Equip_value = hidePro_AddPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //装备上的宝石槽位出现概率
                                    case "405":
                                        gemHole_AddPro_Equip_value = gemHole_AddPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //经验加成固定
                                    case "406":
                                        exp_AddValue_Equip_value = exp_AddValue_Equip_value + int.Parse(hidePropertyValue);
                                        break;

                                    //金币加成固定
                                    case "407":
                                        gold_AddValue_Equip_value = gold_AddValue_Equip_value + int.Parse(hidePropertyValue);
                                        break;
                                    //药剂类熟练度
                                    case "408":
                                        rose_YaoJiValue_Equip_value = rose_YaoJiValue_Equip_value + int.Parse(hidePropertyValue);
                                        break;
                                    //锻造类熟练度
                                    case "409":
                                        rose_DuanZaoValue_Equip_value = rose_DuanZaoValue_Equip_value + int.Parse(hidePropertyValue);
                                        break;
                                    //复活
                                    case "411":
                                        rose_FuHuoPro_Equip_value = rose_FuHuoPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;
                                    //攻击无视防御
                                    case "412":
                                        rose_ActWuShi_Equip_value = rose_ActWuShi_Equip_value + float.Parse(hidePropertyValue);
                                        break;
                                    //神农
                                    case "413":
                                        rose_ShenNong_Equip_value = rose_ShenNong_Equip_value + float.Parse(hidePropertyValue);
                                        break;
                                    //额外掉落
                                    case "414":
                                        rose_DropExtra_Equip_value = rose_DropExtra_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //伪装  +增大发现范围   -缩小范围
                                    case "415":
                                        rose_WeiZhuang_Equip_value = rose_WeiZhuang_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //灾难
                                    case "416":
                                        rose_ZaiNanValue_Equip_value = rose_ZaiNanValue_Equip_value + float.Parse(hidePropertyValue);
                                        break;
                                    //嗜血概率
                                    case "417":
                                        rose_ShiXuePro_Equip_value = rose_ShiXuePro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //怪物脱战距离
                                    case "418":
                                        rose_AITuoZhanDisValue_Equip_value = rose_AITuoZhanDisValue_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //专注概率
                                    case "419":
                                        rose_ZhuanZhuPro_Equip_value = rose_ZhuanZhuPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //必中
                                    case "420":
                                        rose_BiZhongPro_Equip_value = rose_BiZhongPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //生产药剂暴击概率
                                    case "421":
                                        rose_YaoJiCirPro_Equip_value = rose_YaoJiCirPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                    //捕捉概率
                                    case "422":
                                        rose_BuZhuoPro_Equip_value = rose_BuZhuoPro_Equip_value + float.Parse(hidePropertyValue);
                                        break;

                                }
                            }
                        }
                    }
                }


                //获取当前装备携带的宝石属性
                string gemID = functionDataSet.DataSet_ReadData("GemID", "ID", i.ToString(), "RoseEquip");
                //gemID = "101,101";
                if (gemID != "" && gemID!="0")
                {
                    string[] gemStrList = gemID.Split(',');
                    int gemNum = gemStrList.Length;

                    for (int z = 0; z < gemNum; z++) { 
                    
                        //获取当前宝石的属性
                        string gemIDStr = functionDataSet.DataSet_ReadData("ItemUsePar", "ID", gemStrList[z], "Item_Template");
                        //string gemIDStr = "10,10000";

                        string[] gemIDStrList = gemIDStr.Split(';');
                        for (int gemStrNum = 0; gemStrNum < gemIDStrList.Length; gemStrNum++) {

                            if (gemIDStrList[gemStrNum] != "" && gemIDStrList[gemStrNum] != "0")
                            {
                                string gemType = gemIDStrList[gemStrNum].Split(',')[0];
                                string gemValue = gemIDStrList[gemStrNum].Split(',')[1];

                                if (gemValue == "" || gemValue == null)
                                {
                                    gemValue = "0";
                                }

                                switch (gemType)
                                {

                                    //血量
                                    case "10":
                                        hp_Equip_value = hp_Equip_value + int.Parse(gemValue);
                                        break;

                                    //物理最小攻击
                                    case "11":
                                        act_EquipMax_value = act_EquipMax_value + int.Parse(gemValue);
                                        break;

                                    //魔法攻击
                                    case "14":
                                        magact_EquipMax_value = magact_EquipMax_value + int.Parse(gemValue);
                                        break;

                                    //物理防御
                                    case "17":
                                        def_EquipMax_value = def_EquipMax_value + int.Parse(gemValue);
                                        break;

                                    //魔法防御
                                    case "20":
                                        adf_EquipMax_value = adf_EquipMax_value + int.Parse(gemValue);
                                        break;

                                    //暴击
                                    case "30":
                                        cir_Equip_value = cir_Equip_value + float.Parse(gemValue);
                                        break;

                                    //命中
                                    case "31":
                                        hit_Equip_value = hit_Equip_value + float.Parse(gemValue);
                                        break;

                                    //闪避
                                    case "32":
                                        dodge_Equip_value = dodge_Equip_value + float.Parse(gemValue);
                                        break;

                                    //物理免伤
                                    case "33":
                                        defAdd_Equip_value = defAdd_Equip_value + float.Parse(gemValue);
                                        break;

                                    //魔法免伤
                                    case "34":
                                        adfAdd_Equip_value = adfAdd_Equip_value + float.Parse(gemValue);
                                        break;

                                    //速度
                                    case "35":
                                        speed_Equip_value = speed_Equip_value + float.Parse(gemValue);
                                        break;

                                    //伤害免伤
                                    case "36":
                                        damgeSub_Equip_value = damgeSub_Equip_value + float.Parse(gemValue);
                                        break;

                                    //物理伤害
                                    case "37":
                                        damgeAdd_Equip_value = damgeAdd_Equip_value + float.Parse(gemValue);
                                        break;

                                    //血量百分比
                                    case "50":
                                        hpPro_Equip_value = hpPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //物理攻击(百分比)
                                    case "51":
                                        actPro_Equip_value = actPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //魔法攻击(百分比)
                                    case "52":
                                        magactPro_Equip_value = magactPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //物理防御(百分比)
                                    case "53":
                                        defPro_Equip_value = defPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //魔法防御(百分比)
                                    case "54":
                                        adfPro_Equip_value = adfPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //幸运
                                    case "100":
                                        lucky_Equip_value = lucky_Equip_value + int.Parse(gemValue);
                                        break;

                                    //格挡值
                                    case "101":
                                        geDangValue_Equip_value = geDangValue_Equip_value + int.Parse(gemValue);
                                        break;

                                    //重击概率
                                    case "111":
                                        zhongJiPro_Equip_value = zhongJiPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //重击附加伤害值
                                    case "112":
                                        zhongJiValue_Equip_value = zhongJiValue_Equip_value + int.Parse(gemValue);
                                        break;

                                    //每次普通攻击附加的伤害值
                                    case "121":
                                        guDingValue_Equip_value = guDingValue_Equip_value + int.Parse(gemValue);
                                        break;

                                    //忽视目标防御值
                                    case "131":
                                        huShiDefValue_Equip_value = huShiDefValue_Equip_value + int.Parse(gemValue);
                                        break;

                                    //忽视目标魔防值
                                    case "132":
                                        huShiAdfValue_Equip_value = huShiAdfValue_Equip_value + int.Parse(gemValue);
                                        break;

                                    //忽视目标防御值
                                    case "133":
                                        huShiDefValuePro_Equip_value = huShiDefValuePro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //忽视目标魔防值
                                    case "134":
                                        huShiAdfValuePro_Equip_value = huShiAdfValuePro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //吸血概率
                                    case "141":
                                        xiXuePro_Equip_value = xiXuePro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //法术反击
                                    case "151":
                                        magicRebound_Equip_value = magicRebound_Equip_value + float.Parse(gemValue);
                                        break;

                                    //攻击反击
                                    case "152":
                                        actRebound_Equip_value = actRebound_Equip_value + float.Parse(gemValue);
                                        break;

                                    //韧性概率
                                    case "161":
                                        resilience_Equip_value = resilience_Equip_value + float.Parse(gemValue);
                                        break;

                                    //回血百分比
                                    case "171":
                                        rose_HealHpPro_Equip_value = rose_HealHpPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //回血固定值
                                    case "172":
                                        rose_HealHpValue_Equip_value = rose_HealHpValue_Equip_value + int.Parse(gemValue);
                                        break;

                                    //战斗回血比例
                                    case "173":
                                        rose_HealHpFightPro_Equip_value = rose_HealHpFightPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //暴击等级
                                    case "201":
                                        criRating_Equip_value = criRating_Equip_value + int.Parse(gemValue);
                                        break;

                                    //韧性等级
                                    case "202":
                                        resilienceRating_Equip_value = resilienceRating_Equip_value + int.Parse(gemValue);
                                        break;

                                    //命中等级
                                    case "203":
                                        hitRating_Equip_value = hitRating_Equip_value + int.Parse(gemValue);
                                        break;

                                    //闪避等级
                                    case "204":
                                        dodgeRating_Equip_value = dodgeRating_Equip_value + int.Parse(gemValue);
                                        break;

                                    //光抗性
                                    case "301":
                                        resistance_1_Equip_value = resistance_1_Equip_value + float.Parse(gemValue);
                                        break;

                                    //暗抗性
                                    case "302":
                                        resistance_2_Equip_value = resistance_2_Equip_value + float.Parse(gemValue);
                                        break;

                                    //火抗性
                                    case "303":
                                        resistance_3_Equip_value = resistance_3_Equip_value + float.Parse(gemValue);
                                        break;

                                    //水抗性
                                    case "304":
                                        resistance_4_Equip_value = resistance_4_Equip_value + float.Parse(gemValue);
                                        break;

                                    //电抗性
                                    case "305":
                                        resistance_5_Equip_value = resistance_5_Equip_value + float.Parse(gemValue);
                                        break;

                                    //野兽攻击抗性
                                    case "321":
                                        raceResistance_1_Equip_value = raceResistance_1_Equip_value + float.Parse(gemValue);
                                        break;

                                    //人物攻击抗性
                                    case "322":
                                        raceResistance_2_Equip_value = raceResistance_2_Equip_value + float.Parse(gemValue);
                                        break;

                                    //恶魔攻击抗性
                                    case "323":
                                        raceResistance_3_Equip_value = raceResistance_3_Equip_value + float.Parse(gemValue);
                                        break;

                                    //野兽攻击抗性
                                    case "331":
                                        raceDamge_1_Equip_value = raceDamge_1_Equip_value + float.Parse(gemValue);
                                        break;

                                    //人物攻击抗性
                                    case "332":
                                        raceDamge_2_Equip_value = raceDamge_2_Equip_value + float.Parse(gemValue);
                                        break;

                                    //恶魔攻击抗性
                                    case "333":
                                        raceDamge_3_Equip_value = raceDamge_3_Equip_value + float.Parse(gemValue);
                                        break;

                                    //Boss普通攻击加成
                                    case "341":
                                        rose_Boss_ActAdd_Equip_value = rose_Boss_ActAdd_Equip_value + float.Parse(gemValue);
                                        break;

                                    //Boss技能攻击加成
                                    case "342":
                                        rose_Boss_SkillAdd_Equip_value = rose_Boss_SkillAdd_Equip_value + float.Parse(gemValue);
                                        break;

                                    //受到Boss普通攻击减免
                                    case "343":
                                        rose_Boss_ActHitCost_Equip_value = rose_Boss_ActHitCost_Equip_value + float.Parse(gemValue);
                                        break;

                                    //受到Boss技能攻击减免
                                    case "344":
                                        rose_Boss_SkillHitCost_Equip_value = rose_Boss_SkillHitCost_Equip_value + float.Parse(gemValue);
                                        break;

                                    //宠物攻击加成
                                    case "345":
                                        rose_PetActAdd_Equip_value = rose_PetActAdd_Equip_value + float.Parse(gemValue);
                                        break;

                                    //宠物受伤减免
                                    case "346":
                                        rose_PetActHitCost_Equip_value = rose_PetActHitCost_Equip_value + float.Parse(gemValue);
                                        break;

                                    //技能冷却时间缩减
                                    case "347":
                                        rose_SkillCDTimePro_Equip_value = rose_SkillCDTimePro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //自身buff效果延长
                                    case "348":
                                        rose_BuffTimeAddPro_Equip_value = rose_BuffTimeAddPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //Debuff时间缩短
                                    case "349":
                                        rose_DeBuffTimeCostPro_Equip_value = rose_DeBuffTimeCostPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //闪避恢复血量
                                    case "350":
                                        rose_DodgeAddHpPro_Equip_value = rose_DodgeAddHpPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //魔法量附加
                                    case "351":
                                        rose_LanValueMaxAdd_Equip_value = rose_LanValueMaxAdd_Equip_value + float.Parse(gemValue);
                                        break;

                                    //召唤生物属性加成
                                    case "352":
                                        rose_SummonAIPropertyAddPro_Equip_value = rose_LanValueMaxAdd_Equip_value + float.Parse(gemValue);
                                        break;

                                    //护盾属性附加
                                    case "353":
                                        rose_HuDunValueAddPro_Equip_value = rose_LanValueMaxAdd_Equip_value + float.Parse(gemValue);
                                        break;

                                    //经验加成
                                    case "401":
                                        exp_AddPro_Equip_value = exp_AddPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //金币加成
                                    case "402":
                                        gold_AddPro_Equip_value = gold_AddPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //洗炼极品掉落（祝福值）
                                    case "403":
                                        blessing_AddPro_Equip_value = blessing_AddPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //装备隐藏属性出现概率
                                    case "404":
                                        hidePro_AddPro_Equip_value = hidePro_AddPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //装备上的宝石槽位出现概率
                                    case "405":
                                        gemHole_AddPro_Equip_value = gemHole_AddPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //经验加成固定
                                    case "406":
                                        exp_AddValue_Equip_value = exp_AddValue_Equip_value + int.Parse(gemValue);
                                        break;

                                    //金币加成固定
                                    case "407":
                                        gold_AddValue_Equip_value = gold_AddValue_Equip_value + int.Parse(gemValue);
                                        break;
                                    //药剂类熟练度
                                    case "408":
                                        rose_YaoJiValue_Equip_value = rose_YaoJiValue_Equip_value + int.Parse(gemValue);
                                        break;
                                    //锻造类熟练度
                                    case "409":
                                        rose_DuanZaoValue_Equip_value = rose_DuanZaoValue_Equip_value + int.Parse(gemValue);
                                        break;
                                    //复活
                                    case "411":
                                        rose_FuHuoPro_Equip_value = rose_FuHuoPro_Equip_value + float.Parse(gemValue);
                                        break;
                                    //攻击无视防御
                                    case "412":
                                        rose_ActWuShi_Equip_value = rose_ActWuShi_Equip_value + float.Parse(gemValue);
                                        break;
                                    //神农
                                    case "413":
                                        rose_ShenNong_Equip_value = rose_ShenNong_Equip_value + float.Parse(gemValue);
                                        break;
                                    //额外掉落
                                    case "414":
                                        rose_DropExtra_Equip_value = rose_DropExtra_Equip_value + float.Parse(gemValue);
                                        break;
                                    //伪装  +增大发现范围   -缩小范围
                                    case "415":
                                        rose_WeiZhuang_Equip_value = rose_WeiZhuang_Equip_value + float.Parse(gemValue);
                                        break;
                                    //灾难
                                    case "416":
                                        rose_ZaiNanValue_Equip_value = rose_ZaiNanValue_Equip_value + float.Parse(gemValue);
                                        break;
                                    //嗜血概率
                                    case "417":
                                        rose_ShiXuePro_Equip_value = rose_ShiXuePro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //怪物脱战距离
                                    case "418":
                                        rose_AITuoZhanDisValue_Equip_value = rose_AITuoZhanDisValue_Equip_value + float.Parse(gemValue);
                                        break;

                                    //专注概率
                                    case "419":
                                        rose_ZhuanZhuPro_Equip_value = rose_ZhuanZhuPro_Equip_value + float.Parse(gemValue);
                                        break;

                                    //必中
                                    case "420":
                                        rose_BiZhongPro_Equip_value = float.Parse(gemValue);
                                        break;

                                    //生产药剂暴击概率
                                    case "421":
                                        rose_YaoJiCirPro_Equip_value = float.Parse(gemValue);
                                        break;
                                    //捕捉概率
                                    case "422":
                                        rose_BuZhuoPro_Equip_value = rose_BuZhuoPro_Equip_value + float.Parse(gemValue);
                                        break;
                                }
                            }
                        }
                    }
                }

                //获取强化部位对应的强化属性加成EquipQiangHua_Template
                string qiangHuaID = functionDataSet.DataSet_ReadData("QiangHuaID", "ID", i.ToString(), "RoseEquip");
                float equipPropreAdd = float.Parse(functionDataSet.DataSet_ReadData("EquipPropreAdd", "ID", qiangHuaID, "EquipQiangHua_Template"));

                //累加属性
                hp_Equip = hp_Equip + (int)(hp_Equip_value * (1 + equipPropreAdd));
				act_EquipMin = act_EquipMin + (int)(act_EquipMin_value * (1 + equipPropreAdd));
				act_EquipMax = act_EquipMax + (int)(act_EquipMax_value * (1 + equipPropreAdd));
                magact_EquipMin = magact_EquipMin + (int)(magact_EquipMin_value * (1 + equipPropreAdd));
                magact_EquipMax = magact_EquipMax + (int)(magact_EquipMax_value * (1 + equipPropreAdd));
				def_EquipMin = def_EquipMin + (int)(def_EquipMin_value * (1 + equipPropreAdd));
				def_EquipMax = def_EquipMax + (int)(def_EquipMax_value * (1 + equipPropreAdd));
				adf_EquipMin = adf_EquipMin + (int)(adf_EquipMin_value * (1 + equipPropreAdd));
                adf_EquipMax = adf_EquipMax + (int)(adf_EquipMax_value * (1 + equipPropreAdd));
                cir_Equip = cir_Equip + cir_Equip_value;
                hit_Equip = hit_Equip + hit_Equip_value;
                dodge_Equip = dodge_Equip + dodge_Equip_value;
                damgeAdd_Equip = damgeAdd_Equip + damgeAdd_Equip_value;
                damgeSub_Equip = damgeSub_Equip + damgeSub_Equip_value;
                speed_Equip = speed_Equip + speed_Equip_value;
                lucky_Equip = lucky_Equip + lucky_Equip_value;
                defAdd_Equip = defAdd_Equip + defAdd_Equip_value;
                adfAdd_Equip = adfAdd_Equip + adfAdd_Equip_value;
                hpPro_Equip = hpPro_Equip + hpPro_Equip_value;
                actPro_Equip = actPro_Equip + actPro_Equip_value;
                magactPro_Equip = magactPro_Equip + magactPro_Equip_value;
                defPro_Equip = defPro_Equip + defPro_Equip_value;
                adfPro_Equip = adfPro_Equip + adfPro_Equip_value;

                geDangValue_Equip = geDangValue_Equip + geDangValue_Equip_value;                        //格挡值
                zhongJiPro_Equip = zhongJiPro_Equip + zhongJiPro_Equip_value;                           //重击概率
                zhongJiValue_Equip = zhongJiValue_Equip + zhongJiValue_Equip_value;                     //重击附加伤害值
                guDingValue_Equip = guDingValue_Equip + guDingValue_Equip_value;                        //每次普通攻击附加的伤害值
                huShiDefValue_Equip = huShiDefValue_Equip + huShiDefValue_Equip_value;                  //忽视目标防御值                       
                huShiAdfValue_Equip = huShiAdfValue_Equip + huShiAdfValue_Equip_value;                  //忽视目标魔防值
                huShiDefValuePro_Equip = huShiDefValuePro_Equip + huShiDefValuePro_Equip_value;         //忽视目标防御值                       
                huShiAdfValuePro_Equip = huShiAdfValuePro_Equip + huShiAdfValuePro_Equip_value;         //忽视目标魔防值
                xiXuePro_Equip = xiXuePro_Equip + xiXuePro_Equip_value;                                 //吸血概率

                criRating_Equip = criRating_Equip + criRating_Equip_value;                                  //暴击等级
                resilienceRating_Equip = resilienceRating_Equip + resilienceRating_Equip_value;             //韧性等级
                hitRating_Equip = hitRating_Equip + hitRating_Equip_value;                                  //命中等级
                dodgeRating_Equip = dodgeRating_Equip + dodgeRating_Equip_value;                            //闪避等级
                resilience_Equip = resilience_Equip + resilience_Equip_value;                               //韧性概率
                magicRebound_Equip = magicRebound_Equip + magicRebound_Equip_value;                         //法术反击值
                actRebound_Equip = actRebound_Equip + actRebound_Equip_value;                               //攻击反击
                resistance_1_Equip = resistance_1_Equip + resistance_1_Equip_value;                         //光抗性
                resistance_2_Equip = resistance_2_Equip + resistance_2_Equip_value;                         //暗抗性
                resistance_3_Equip = resistance_3_Equip + resistance_3_Equip_value;                         //火抗性
                resistance_4_Equip = resistance_4_Equip + resistance_4_Equip_value;                         //水抗性
                resistance_5_Equip = resistance_5_Equip + resistance_5_Equip_value;                         //电抗性
                raceResistance_1_Equip = raceResistance_1_Equip + raceResistance_1_Equip_value;             //野兽攻击抗性
                raceResistance_2_Equip = raceResistance_2_Equip + raceResistance_2_Equip_value;             //人物攻击抗性
                raceResistance_3_Equip = raceResistance_3_Equip + raceResistance_3_Equip_value;             //恶魔攻击抗性
                rose_Boss_ActAdd_Equip = rose_Boss_ActAdd_Equip + rose_Boss_ActAdd_Equip_value;                             //Boss普通攻击加成
                rose_Boss_SkillAdd_Equip = rose_Boss_SkillAdd_Equip + rose_Boss_SkillAdd_Equip_value;                       //Boss技能攻击加成
                rose_Boss_ActHitCost_Equip = rose_Boss_ActHitCost_Equip + rose_Boss_ActHitCost_Equip_value;                 //受到Boss普通攻击减免
                rose_Boss_SkillHitCost_Equip = rose_Boss_SkillHitCost_Equip + rose_Boss_SkillHitCost_Equip_value;           //受到Boss技能攻击减免
                rose_PetActAdd_Equip = rose_PetActAdd_Equip + rose_PetActAdd_Equip_value;                                   //宠物攻击加成
                rose_PetActHitCost_Equip = rose_PetActHitCost_Equip + rose_PetActHitCost_Equip_value;                       //宠物受伤减免
                rose_SkillCDTimePro_Equip = rose_SkillCDTimePro_Equip + rose_SkillCDTimePro_Equip_value;                    //技能冷却时间缩减
                rose_BuffTimeAddPro_Equip = rose_BuffTimeAddPro_Equip + rose_BuffTimeAddPro_Equip_value;                    //自身buff效果延长
                rose_DeBuffTimeCostPro_Equip = rose_DeBuffTimeCostPro_Equip + rose_DeBuffTimeCostPro_Equip_value;           //Debuff时间缩短
                rose_DodgeAddHpPro_Equip = rose_DodgeAddHpPro_Equip + rose_DodgeAddHpPro_Equip_value;                       //闪避恢复血量

                exp_AddPro_Equip = exp_AddPro_Equip + exp_AddPro_Equip_value;                               //经验加成
                gold_AddPro_Equip = gold_AddPro_Equip + gold_AddPro_Equip_value;                            //金币加成
                exp_AddValue_Equip = exp_AddValue_Equip + exp_AddValue_Equip_value;                         //经验加成固定值
                gold_AddValue_Equip = gold_AddValue_Equip + gold_AddValue_Equip_value;                      //金币加成固定值
                blessing_AddPro_Equip = blessing_AddPro_Equip + blessing_AddPro_Equip_value;                //洗炼极品掉落
                hidePro_AddPro_Equip = hidePro_AddPro_Equip + hidePro_AddPro_Equip_value;                   //隐藏属性出现概率
                gemHole_AddPro_Equip = gemHole_AddPro_Equip + gemHole_AddPro_Equip_value;                   //装备上的宝石槽位出现概率


                rose_YaoJiValue_Equip = rose_YaoJiValue_Equip + rose_YaoJiValue_Equip_value;                    //药剂类熟练度
                rose_DuanZaoValue_Equip = rose_DuanZaoValue_Equip + rose_DuanZaoValue_Equip_value;				//锻造类熟练度 
                rose_FuHuoPro_Equip = rose_FuHuoPro_Equip + rose_FuHuoPro_Equip_value;          		        //复活
                rose_ActWuShi_Equip = rose_ActWuShi_Equip + rose_ActWuShi_Equip_value; 				            //攻击无视防御
                rose_ShenNong_Equip = rose_ShenNong_Equip + rose_ShenNong_Equip_value;              	        //神农
                rose_DropExtra_Equip = rose_DropExtra_Equip + rose_DropExtra_Equip_value;          	            //额外掉落
                rose_WeiZhuang_Equip = rose_WeiZhuang_Equip + rose_WeiZhuang_Equip_value;           	        //伪装  +增大发现范围   -缩小范围
                rose_ZaiNanValue_Equip = rose_ZaiNanValue_Equip + rose_ZaiNanValue_Equip_value;                 //灾难
                rose_ShiXuePro_Equip = rose_ShiXuePro_Equip + rose_ShiXuePro_Equip_value;				        //嗜血概率


                rose_HealHpValue_Equip = rose_HealHpValue_Equip + rose_HealHpValue_Equip_value;              	    //角色恢复的固定值
                rose_HealHpPro_Equip = rose_HealHpPro_Equip + rose_HealHpPro_Equip_value;          	                //角色恢复的百分比加成固定值
                rose_HealHpFightPro_Equip = rose_HealHpFightPro_Equip + rose_HealHpFightPro_Equip_value;           	//角色战斗时恢复额外恢复血量的百分比
                rose_AITuoZhanDisValue_Equip = rose_AITuoZhanDisValue_Equip + rose_AITuoZhanDisValue_Equip_value;   //怪物脱战距离
                rose_YaoJiCirPro_Equip = rose_YaoJiCirPro_Equip + rose_YaoJiCirPro_Equip_value;				        //生产药剂暴击概率
                rose_BuZhuoPro_Equip = rose_BuZhuoPro_Equip + rose_BuZhuoPro_Equip_value;                           //生产药剂暴击概率

                rose_LanValueMaxAdd_Equip = rose_LanValueMaxAdd_Equip + rose_LanValueMaxAdd_Equip_value;                                //魔法量附加
                rose_SummonAIPropertyAddPro_Equip = rose_SummonAIPropertyAddPro_Equip + rose_SummonAIPropertyAddPro_Equip_value;        //召唤生物属性加成
                rose_HuDunValueAddPro_Equip = rose_HuDunValueAddPro_Equip + rose_HuDunValueAddPro_Equip_value;				            //护盾属性附加

                //循环自身装备所附带的技能ID
                //Game_PublicClassVar.Get_function_Skill.EquipCostSkillID(equipID_1);

                //循环自身装备套装的ID
                string equipSuitID = functionDataSet.DataSet_ReadData("EquipSuitID", "ID", equipID, "Equip_Template");
                if (equipSuitID != "0") {
                    if (equipSuitIDStr == "")
                    {
                        equipSuitIDStr = equipSuitIDStr + equipSuitID;
                    }
                    else
                    {
                        //循环判定,防止重复的套装ID
                        string[] equipSuitStrID = equipSuitIDStr.Split(';');
                        bool addStatus = true;
                        for (int y = 0; y <= equipSuitStrID.Length - 1; y++)
                        {
                            if (equipSuitID == equipSuitStrID[y])
                            {
                                addStatus = false;
                            }
                        }

                        if (addStatus)
                        {
                            equipSuitIDStr = equipSuitIDStr + ";" + equipSuitID;
                        }
                    }
                }
			}
		}


        //套装子ID
        ArrayList equipSuitPropertyList = new ArrayList();
        //Debug.Log("equipSuitIDStr = " + equipSuitIDStr);
        if (equipSuitIDStr != "") {
            string[] equipSuitStrID = equipSuitIDStr.Split(';');
            for (int i = 0; i <= equipSuitStrID.Length - 1; i++) { 
                //获得子套装属性
                string[] needEquipIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedEquipID", "ID", equipSuitStrID[i], "EquipSuit_Template").Split(';');
                string[] needEquipNumSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedEquipNum", "ID", equipSuitStrID[i], "EquipSuit_Template").Split(';');
                string[] suitPropertyIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SuitPropertyID", "ID", equipSuitStrID[i], "EquipSuit_Template").Split(';');
                //获取自身套装数量
                int equipSuitNum = Game_PublicClassVar.Get_function_Rose.returnEquipSuitNum(needEquipIDSet, needEquipNumSet);
                for (int y = 0; y <= suitPropertyIDSet.Length - 1; y++)
                {
                    //Debug.Log("equipSuitStrID[i] = " + equipSuitStrID[i]);
                    string triggerSuitNum = suitPropertyIDSet[y].Split(',')[0];
                    string triggerSuitPropertyID = suitPropertyIDSet[y].Split(',')[1];
                    //满足条件套装
                    if (equipSuitNum >= int.Parse(triggerSuitNum))
                    {
                        equipSuitPropertyList.Add(triggerSuitPropertyID);

                        //写入成就(套装效果)
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("204", triggerSuitPropertyID, "1");
                    }
                }
            }
        }


        //获取装备套装属性
        int hp_EquipSuit = 0;
        int act_EquipSuitMin = 0;
        int act_EquipSuitMax = 0;
        int magact_EquipSuitMin = 0;
        int magact_EquipSuitMax = 0;
        int def_EquipSuitMin = 0;
        int def_EquipSuitMax = 0;
        int adf_EquipSuitMin = 0;
        int adf_EquipSuitMax = 0;
        float cir_EquipSuit = 0;
        float hit_EquipSuit = 0;
        float dodge_EquipSuit = 0;
        float damgeAdd_EquipSuit = 0;
        float damgeSub_EquipSuit = 0;
        float speed_EquipSuit = 0;
        int lucky_EquipSuit = 0;

        int geDangValue_EquipSuit = 0;                    //格挡值
        float zhongJiPro_EquipSuit = 0.0f;                //重击概率
        int zhongJiValue_EquipSuit = 0;                   //重击附加伤害值
        int guDingValue_EquipSuit = 0;                    //每次普通攻击附加的伤害值
        int huShiDefValue_EquipSuit = 0;                  //忽视目标防御值                       
        int huShiAdfValue_EquipSuit = 0;                  //忽视目标魔防值
        float huShiDefValuePro_EquipSuit = 0;             //忽视目标防御值                       
        float huShiAdfValuePro_EquipSuit = 0;             //忽视目标魔防值
        float xiXuePro_EquipSuit = 0.0f;                  //吸血概率


        int criRating_EquipSuit = 0;                    //暴击等级
        int resilienceRating_EquipSuit = 0;             //韧性等级
        int hitRating_EquipSuit = 0;                    //命中等级
        int dodgeRating_EquipSuit = 0;                  //闪避等级
        float resilience_EquipSuit = 0.0f;              //韧性概率
        float magicRebound_EquipSuit = 0.0f;            //法术反击值
        float actRebound_EquipSuit = 0.0f;              //攻击反击
        float resistance_1_EquipSuit = 0;                //光抗性
        float resistance_2_EquipSuit = 0;                //暗抗性
        float resistance_3_EquipSuit = 0;                //火抗性
        float resistance_4_EquipSuit = 0;                //水抗性
        float resistance_5_EquipSuit = 0;                //电抗性
        float raceResistance_1_EquipSuit = 0;            //野兽攻击抗性
        float raceResistance_2_EquipSuit = 0;            //人物攻击抗性
        float raceResistance_3_EquipSuit = 0;            //恶魔攻击抗性
		float raceDamge_1_EquipSuit = 0;            //野兽攻击抗性
		float raceDamge_2_EquipSuit = 0;            //人物攻击抗性
		float raceDamge_3_EquipSuit = 0;            //恶魔攻击抗性
        float rose_Boss_ActAdd_EquipSuit = 0;                      //Boss普通攻击加成
        float rose_Boss_SkillAdd_EquipSuit = 0;                    //Boss技能攻击加成
        float rose_Boss_ActHitCost_EquipSuit = 0;                  //受到Boss普通攻击减免
        float rose_Boss_SkillHitCost_EquipSuit = 0;                //受到Boss技能攻击减免
        float rose_PetActAdd_EquipSuit = 0;                        //宠物攻击加成
        float rose_PetActHitCost_EquipSuit = 0;                    //宠物受伤减免
        float rose_SkillCDTimePro_EquipSuit = 0;                      //技能冷却时间缩减
        float rose_BuffTimeAddPro_EquipSuit = 0;                      //自身buff效果延长
        float rose_DeBuffTimeCostPro_EquipSuit = 0;                   //Debuff时间缩短
        float rose_DodgeAddHpPro_EquipSuit = 0;                       //闪避恢复血量

        float exp_AddPro_EquipSuit = 0.0f;               //经验加成
        float gold_AddPro_EquipSuit = 0.0f;              //金币加成
        int exp_AddValue_EquipSuit = 0;             //经验加成
        int gold_AddValue_EquipSuit = 0;            //金币加成
        float blessing_AddPro_EquipSuit = 0.0f;          //洗炼极品掉落
        float hidePro_AddPro_EquipSuit = 0.0f;           //隐藏属性出现概率
        float gemHole_AddPro_EquipSuit = 0.0f;           //装备上的宝石槽位出现概率

        int rose_YaoJiValue_EquipSuit = 0;                      //药剂类熟练度
        int rose_DuanZaoValue_EquipSuit = 0;				    //锻造类熟练度 
        float rose_FuHuoPro_EquipSuit = 0.0f;          		    //复活
        float rose_ActWuShi_EquipSuit = 0.0f; 				    //攻击无视防御
        float rose_ShenNong_EquipSuit = 0.0f;              	    //神农
        float rose_DropExtra_EquipSuit = 0.0f;          	    //额外掉落
        float rose_WeiZhuang_EquipSuit = 0.0f;           	    //伪装  +增大发现范围   -缩小范围
        float rose_ZaiNanValue_EquipSuit = 0.0f;                //灾难
        float rose_ShiXuePro_EquipSuit = 0.0f;				    //嗜血概率

        int rose_HealHpValue_EquipSuit = 0;                   //角色恢复的固定值
        float rose_HealHpPro_EquipSuit = 0.0f;                //角色恢复的百分比加成固定值
        float rose_HealHpFightPro_EquipSuit = 0.0f;           //角色战斗时恢复额外恢复血量的百分比
        float rose_AITuoZhanDisValue_EquipSuit = 0.0f;		  //怪物脱战距离
        float rose_ZhuanZhuPro_EquipSuit = 0;					//专注概率
        float rose_BiZhongPro_EquipSuit = 0.0f;		            //怪物脱战距离
        float rose_YaoJiCirPro_EquipSuit = 0;					//生产药剂暴击概率
        float rose_BuZhuoPro_EquipSuit = 0;                     //捕捉概率
        float rose_LanValueMaxAdd_EquipSuit = 0;                       //魔法量附加
        float rose_SummonAIPropertyAddPro_EquipSuit = 0;                //召唤生物属性加成
        float rose_HuDunValueAddPro_EquipSuit = 0;                     //护盾属性附加


        string equipSuitSkillIDStr = "";
        foreach (string equipSuitPropertyID in equipSuitPropertyList) {

            //循环添加套装属性
            int hp_EquipSuit_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Hp", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int act_EquipSuitMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAct", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int act_EquipSuitMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAct", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int magact_EquipSuitMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinMagAct", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int magact_EquipSuitMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxMagAct", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int def_EquipSuitMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinDef", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int def_EquipSuitMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxDef", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int adf_EquipSuitMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAdf", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int adf_EquipSuitMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAdf", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float cir_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Cri", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float hit_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Hit", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float dodge_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Dodge", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float damgeAdd_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeAdd", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float damgeSub_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeSub", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float speed_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Speed", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int lucky_EquipSuit_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Lucky", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));


            int geDangValue_EquipSuit_value = 0;                    //格挡值
            float zhongJiPro_EquipSuit_value = 0.0f;                //重击概率
            int zhongJiValue_EquipSuit_value = 0;                   //重击附加伤害值
            int guDingValue_EquipSuit_value = 0;                    //每次普通攻击附加的伤害值
            int huShiDefValue_EquipSuit_value = 0;                  //忽视目标防御值                       
            int huShiAdfValue_EquipSuit_value = 0;                  //忽视目标魔防值
            float huShiDefValuePro_EquipSuit_value = 0;               //忽视目标防御值                       
            float huShiAdfValuePro_EquipSuit_value = 0;               //忽视目标魔防值
            float xiXuePro_EquipSuit_value = 0.0f;                  //吸血概率

            int criRating_EquipSuit_value = 0;                    //暴击等级
            int resilienceRating_EquipSuit_value = 0;             //韧性等级
            int hitRating_EquipSuit_value = 0;                    //命中等级
            int dodgeRating_EquipSuit_value = 0;                  //闪避等级
            float resilience_EquipSuit_value = 0.0f;              //韧性概率
            float magicRebound_EquipSuit_value = 0.0f;            //法术反击值
            float actRebound_EquipSuit_value = 0.0f;              //攻击反击
            float resistance_1_EquipSuit_value = 0;                //光抗性
            float resistance_2_EquipSuit_value = 0;                //暗抗性
            float resistance_3_EquipSuit_value = 0;                //火抗性
            float resistance_4_EquipSuit_value = 0;                //水抗性
            float resistance_5_EquipSuit_value = 0;                //电抗性
            float raceResistance_1_EquipSuit_value = 0;            //野兽攻击抗性
            float raceResistance_2_EquipSuit_value = 0;            //人物攻击抗性
            float raceResistance_3_EquipSuit_value = 0;            //恶魔攻击抗性
			float raceDamge_1_EquipSuit_value = 0;            //野兽攻击抗性
			float raceDamge_2_EquipSuit_value = 0;            //人物攻击抗性
			float raceDamge_3_EquipSuit_value = 0;            //恶魔攻击抗性
            float rose_Boss_ActAdd_EquipSuit_value = 0;                      //Boss普通攻击加成
            float rose_Boss_SkillAdd_EquipSuit_value = 0;                    //Boss技能攻击加成
            float rose_Boss_ActHitCost_EquipSuit_value = 0;                  //受到Boss普通攻击减免
            float rose_Boss_SkillHitCost_EquipSuit_value = 0;                //受到Boss技能攻击减免
            float rose_PetActAdd_EquipSuit_value = 0;                        //宠物攻击加成
            float rose_PetActHitCost_EquipSuit_value = 0;                    //宠物受伤减免
            float rose_SkillCDTimePro_EquipSuit_value = 0;                      //技能冷却时间缩减
            float rose_BuffTimeAddPro_EquipSuit_value = 0;                      //自身buff效果延长
            float rose_DeBuffTimeCostPro_EquipSuit_value = 0;                   //Debuff时间缩短
            float rose_DodgeAddHpPro_EquipSuit_value = 0;                       //闪避恢复血量

            float exp_AddPro_EquipSuit_value = 0.0f;               //经验加成
            float gold_AddPro_EquipSuit_value = 0.0f;              //金币加成
            int exp_AddValue_EquipSuit_value = 0;               //经验加成固定
            int gold_AddValue_EquipSuit_value = 0;              //金币加成固定
            float blessing_AddPro_EquipSuit_value = 0.0f;          //洗炼极品掉落
            float hidePro_AddPro_EquipSuit_value = 0.0f;           //隐藏属性出现概率
            float gemHole_AddPro_EquipSuit_value = 0.0f;           //装备上的宝石槽位出现概率

            int rose_YaoJiValue_EquipSuit_value = 0;                      //药剂类熟练度
            int rose_DuanZaoValue_EquipSuit_value = 0;				    //锻造类熟练度 
            float rose_FuHuoPro_EquipSuit_value = 0.0f;          		    //复活
            float rose_ActWuShi_EquipSuit_value = 0.0f; 				    //攻击无视防御
            float rose_ShenNong_EquipSuit_value = 0.0f;              	    //神农
            float rose_DropExtra_EquipSuit_value = 0.0f;          	    //额外掉落
            float rose_WeiZhuang_EquipSuit_value = 0.0f;           	    //伪装  +增大发现范围   -缩小范围
            float rose_ZaiNanValue_EquipSuit_value = 0.0f;                //灾难
            float rose_ShiXuePro_EquipSuit_value = 0.0f;				    //嗜血概率

            int rose_HealHpValue_EquipSuit_value = 0;                       //角色恢复的固定值
            float rose_HealHpPro_EquipSuit_value = 0.0f;                    //角色恢复的百分比加成固定值
            float rose_HealHpFightPro_EquipSuit_value = 0.0f;               //角色战斗时恢复额外恢复血量的百分比
            float rose_AITuoZhanDisValue_EquipSuit_value = 0.0f;		    //怪物脱战距离
            float rose_ZhuanZhuPro_EquipSuit_value = 0;					    //专注概率
            float rose_BiZhongPro_EquipSuit_value = 0.0f;		                    //怪物脱战距离
            float rose_YaoJiCirPro_EquipSuit_value = 0;					        //生产药剂暴击概率
            float rose_BuZhuoPro_EquipSuit_value = 0;                       //捕捉概率

            float rose_LanValueMaxAdd_EquipSuit_value = 0;                        //魔法量附加
            float rose_SummonAIPropertyAddPro_EquipSuit_value = 0;                //召唤生物属性加成
            float rose_HuDunValueAddPro_EquipSuit_value = 0;                      //护盾属性附加

            //获取其他属性
            string AddPropreListStrValue = functionDataSet.DataSet_ReadData("AddPropreListStr", "ID", equipSuitPropertyID, "EquipSuitProperty_Template");
            if (AddPropreListStrValue != "" && AddPropreListStrValue != "0")
            {
                string[] AddPropreListStr = AddPropreListStrValue.Split(';');
                if (AddPropreListStr.Length > 0)
                {
                    for (int y = 0; y < AddPropreListStr.Length; y++)
                    {
                        string proprety = AddPropreListStr[y].Split(',')[0];
                        string propretyValue = AddPropreListStr[y].Split(',')[1];
                        switch (proprety)
                        {
                            //格挡值
                            case "101":
                                geDangValue_EquipSuit_value = int.Parse(propretyValue);
                                break;

                            //重击概率
                            case "111":
                                zhongJiPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //重击附加伤害值
                            case "112":
                                zhongJiValue_EquipSuit_value = int.Parse(propretyValue);
                                break;

                            //每次普通攻击附加的伤害值
                            case "121":
                                guDingValue_EquipSuit_value = int.Parse(propretyValue);
                                break;

                            //忽视目标防御值
                            case "131":
                                huShiDefValue_EquipSuit_value = int.Parse(propretyValue);
                                break;

                            //忽视目标魔防值
                            case "132":
                                huShiAdfValue_EquipSuit_value = int.Parse(propretyValue);
                                break;

                            //忽视目标防御值
                            case "133":
                                huShiDefValuePro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //忽视目标魔防值
                            case "134":
                                huShiAdfValuePro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //吸血概率
                            case "141":
                                xiXuePro_EquipSuit_value = float.Parse(propretyValue);
                                break;
                            //法术反击
                            case "151":
                                magicRebound_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //攻击反击
                            case "152":
                                actRebound_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //韧性概率
                            case "161":
                                resilience_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //回血百分比
                            case "171":
                                rose_HealHpPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //回血固定值
                            case "172":
                                rose_HealHpValue_EquipSuit_value = int.Parse(propretyValue);
                                break;

                            //战斗回血比例
                            case "173":
                                rose_HealHpFightPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //暴击等级
                            case "201":
                                criRating_EquipSuit_value = int.Parse(propretyValue);
                                break;

                            //韧性等级
                            case "202":
                                resilienceRating_EquipSuit_value = int.Parse(propretyValue);
                                break;

                            //命中等级
                            case "203":
                                hitRating_EquipSuit_value = int.Parse(propretyValue);
                                break;

                            //闪避等级
                            case "204":
                                dodgeRating_EquipSuit_value = int.Parse(propretyValue);
                                break;

                            //光抗性
                            case "301":
                                resistance_1_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //暗抗性
                            case "302":
                                resistance_2_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //火抗性
                            case "303":
                                resistance_3_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //水抗性
                            case "304":
                                resistance_4_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //电抗性
                            case "305":
                                resistance_5_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //野兽攻击抗性
                            case "321":
                                raceResistance_1_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //人物攻击抗性
                            case "322":
                                raceResistance_2_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //恶魔攻击抗性
                            case "323":
                                raceResistance_3_EquipSuit_value = float.Parse(propretyValue);
                                break;

							//野兽攻击伤害
							case "331":
								raceDamge_1_EquipSuit_value = float.Parse(propretyValue);
								break;

							//人物攻击伤害
							case "332":
								raceDamge_2_EquipSuit_value = float.Parse(propretyValue);
								break;

							//恶魔攻击伤害
							case "333":
								raceDamge_3_EquipSuit_value = float.Parse(propretyValue);
								break;

                            //Boss普通攻击加成
                            case "341":
                                rose_Boss_ActAdd_EquipSuit_value =  float.Parse(propretyValue);
                                break;

                            //Boss技能攻击加成
                            case "342":
                                rose_Boss_SkillAdd_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //受到Boss普通攻击减免
                            case "343":
                                rose_Boss_ActHitCost_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //受到Boss技能攻击减免
                            case "344":
                                rose_Boss_SkillHitCost_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //宠物攻击加成
                            case "345":
                                rose_PetActAdd_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //宠物受伤减免
                            case "346":
                                rose_PetActHitCost_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //技能冷却时间缩减
                            case "347":
                                rose_SkillCDTimePro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //自身buff效果延长
                            case "348":
                                rose_BuffTimeAddPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //Debuff时间缩短
                            case "349":
                                rose_DeBuffTimeCostPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //闪避恢复血量
                            case "350":
                                rose_DodgeAddHpPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //魔法量附加
                            case "351":
                                rose_LanValueMaxAdd_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //召唤生物属性加成
                            case "352":
                                rose_SummonAIPropertyAddPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //护盾属性附加
                            case "353":
                                rose_HuDunValueAddPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //经验加成
                            case "401":
                                exp_AddPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //金币加成
                            case "402":
                                gold_AddPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //洗炼极品掉落（祝福值）
                            case "403":
                                blessing_AddPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //装备隐藏属性出现概率
                            case "404":
                                hidePro_AddPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //装备上的宝石槽位出现概率
                            case "405":
                                gemHole_AddPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //经验加成固定
                            case "406":
                                exp_AddValue_EquipSuit_value = int.Parse(propretyValue);
                                break;

                            //金币加成固定
                            case "407":
                                gold_AddValue_EquipSuit_value = int.Parse(propretyValue);
                                break;
                            //药剂类熟练度
                            case "408":
                                rose_YaoJiValue_EquipSuit_value = int.Parse(propretyValue);
                                break;
                            //锻造类熟练度
                            case "409":
                                rose_DuanZaoValue_EquipSuit_value = int.Parse(propretyValue);
                                break;
                            //复活
                            case "411":
                                rose_FuHuoPro_EquipSuit_value = float.Parse(propretyValue);
                                break;
                            //攻击无视防御
                            case "412":
                                rose_ActWuShi_EquipSuit_value = float.Parse(propretyValue);
                                break;
                            //神农
                            case "413":
                                rose_ShenNong_EquipSuit_value = float.Parse(propretyValue);
                                break;
                            //额外掉落
                            case "414":
                                rose_DropExtra_EquipSuit_value = float.Parse(propretyValue);
                                break;
                            //伪装  +增大发现范围   -缩小范围
                            case "415":
                                rose_WeiZhuang_EquipSuit_value = float.Parse(propretyValue);
                                break;
                            //灾难
                            case "416":
                                rose_ZaiNanValue_EquipSuit_value = float.Parse(propretyValue);
                                break;
                            //嗜血概率
                            case "417":
                                rose_ShiXuePro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //怪物脱战距离
                            case "418":
                                rose_AITuoZhanDisValue_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //专注概率
                            case "419":
                                rose_ZhuanZhuPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //必中
                            case "420":
                                rose_BiZhongPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //生产药剂暴击概率
                            case "421":
                                rose_YaoJiCirPro_EquipSuit_value = float.Parse(propretyValue);
                                break;

                            //捕捉
                            case "422":
                                rose_BuZhuoPro_EquipSuit_value = float.Parse(propretyValue);
                                break;
                        }
                    }
                }
            }


            //累加属性
            hp_EquipSuit = hp_EquipSuit + hp_EquipSuit_value;
            act_EquipSuitMin = act_EquipSuitMin + act_EquipSuitMin_value;
            act_EquipSuitMax = act_EquipSuitMax + act_EquipSuitMax_value;
            magact_EquipSuitMin = magact_EquipSuitMin + magact_EquipSuitMin_value;
            magact_EquipSuitMax = magact_EquipSuitMax + magact_EquipSuitMax_value;
            def_EquipSuitMin = def_EquipSuitMin + def_EquipSuitMin_value;
            def_EquipSuitMax = def_EquipSuitMax + def_EquipSuitMax_value;
            adf_EquipSuitMin = adf_EquipSuitMin + adf_EquipSuitMin_value;
            adf_EquipSuitMax = adf_EquipSuitMax + adf_EquipSuitMax_value;
            cir_EquipSuit = cir_EquipSuit + cir_EquipSuit_value;
            hit_EquipSuit = hit_EquipSuit + hit_EquipSuit_value;
            dodge_EquipSuit = dodge_EquipSuit + dodge_EquipSuit_value;
            damgeAdd_EquipSuit = damgeAdd_EquipSuit + damgeAdd_EquipSuit_value;
            damgeSub_EquipSuit = damgeSub_EquipSuit + damgeSub_EquipSuit_value;
            speed_EquipSuit = speed_EquipSuit + speed_EquipSuit_value;
            lucky_EquipSuit = lucky_EquipSuit + lucky_EquipSuit_value;


            geDangValue_EquipSuit = geDangValue_EquipSuit + geDangValue_EquipSuit_value;                        //格挡值
            zhongJiPro_EquipSuit = zhongJiPro_EquipSuit + zhongJiPro_EquipSuit_value;                           //重击概率
            zhongJiValue_EquipSuit = zhongJiValue_EquipSuit + zhongJiValue_EquipSuit_value;                     //重击附加伤害值
            guDingValue_EquipSuit = guDingValue_EquipSuit + guDingValue_EquipSuit_value;                        //每次普通攻击附加的伤害值
            huShiDefValue_EquipSuit = huShiDefValue_EquipSuit + huShiDefValue_EquipSuit_value;                  //忽视目标防御值                       
            huShiAdfValue_EquipSuit = huShiAdfValue_EquipSuit + huShiAdfValue_EquipSuit_value;                  //忽视目标魔防值
            huShiDefValuePro_EquipSuit = huShiDefValuePro_EquipSuit + huShiDefValue_EquipSuit_value;            //忽视目标防御值                       
            huShiAdfValuePro_EquipSuit = huShiAdfValuePro_EquipSuit + huShiAdfValue_EquipSuit_value;            //忽视目标魔防值
            xiXuePro_EquipSuit = xiXuePro_EquipSuit + xiXuePro_EquipSuit_value;                                 //吸血概率



            criRating_EquipSuit = criRating_EquipSuit + criRating_EquipSuit_value;                                  //暴击等级
            resilienceRating_EquipSuit = resilienceRating_EquipSuit + resilienceRating_EquipSuit_value;             //韧性等级
            hitRating_EquipSuit = hitRating_EquipSuit + hitRating_EquipSuit_value;                                  //命中等级
            dodgeRating_EquipSuit = dodgeRating_EquipSuit + dodgeRating_EquipSuit_value;                            //闪避等级
            resilience_EquipSuit = resilience_EquipSuit + resilience_EquipSuit_value;                               //韧性概率
            magicRebound_EquipSuit = magicRebound_EquipSuit + magicRebound_EquipSuit_value;                         //法术反击值
            actRebound_EquipSuit = actRebound_EquipSuit + actRebound_EquipSuit_value;                               //攻击反击
            resistance_1_EquipSuit = resistance_1_EquipSuit + resistance_1_EquipSuit_value;                         //光抗性
            resistance_2_EquipSuit = resistance_2_EquipSuit + resistance_2_EquipSuit_value;                         //暗抗性
            resistance_3_EquipSuit = resistance_3_EquipSuit + resistance_3_EquipSuit_value;                         //火抗性
            resistance_4_EquipSuit = resistance_4_EquipSuit + resistance_4_EquipSuit_value;                         //水抗性
            resistance_5_EquipSuit = resistance_5_EquipSuit + resistance_5_EquipSuit_value;                         //电抗性
            raceResistance_1_EquipSuit = raceResistance_1_EquipSuit + raceResistance_1_EquipSuit_value;             //野兽攻击抗性
            raceResistance_2_EquipSuit = raceResistance_2_EquipSuit + raceResistance_2_EquipSuit_value;             //人物攻击抗性
            raceResistance_3_EquipSuit = raceResistance_3_EquipSuit + raceResistance_3_EquipSuit_value;             //恶魔攻击抗性
			raceDamge_1_EquipSuit = raceDamge_1_EquipSuit + raceDamge_1_EquipSuit_value;                            //野兽攻击抗性
			raceDamge_2_EquipSuit = raceDamge_2_EquipSuit + raceDamge_2_EquipSuit_value;                            //人物攻击抗性
			raceDamge_3_EquipSuit = raceDamge_3_EquipSuit + raceDamge_3_EquipSuit_value;                            //恶魔攻击抗性
            rose_Boss_ActAdd_EquipSuit = rose_Boss_ActAdd_EquipSuit + rose_Boss_ActAdd_EquipSuit_value;                            //Boss普通攻击加成
            rose_Boss_SkillAdd_EquipSuit = rose_Boss_SkillAdd_EquipSuit + rose_Boss_SkillAdd_EquipSuit_value;                      //Boss技能攻击加成
            rose_Boss_ActHitCost_EquipSuit = rose_Boss_ActHitCost_EquipSuit + rose_Boss_ActHitCost_EquipSuit_value;                //受到Boss普通攻击减免
            rose_Boss_SkillHitCost_EquipSuit = rose_Boss_SkillHitCost_EquipSuit + rose_Boss_SkillHitCost_EquipSuit_value;          //受到Boss技能攻击减免
            rose_PetActAdd_EquipSuit = rose_PetActAdd_EquipSuit + rose_PetActAdd_EquipSuit_value;                                  //宠物攻击加成
            rose_PetActHitCost_EquipSuit = rose_PetActHitCost_EquipSuit + rose_PetActHitCost_EquipSuit_value;                      //宠物受伤减免
            rose_SkillCDTimePro_EquipSuit = rose_SkillCDTimePro_EquipSuit + rose_SkillCDTimePro_EquipSuit_value;                    //技能冷却时间缩减
            rose_BuffTimeAddPro_EquipSuit = rose_BuffTimeAddPro_EquipSuit + rose_BuffTimeAddPro_EquipSuit_value;                    //自身buff效果延长
            rose_DeBuffTimeCostPro_EquipSuit = rose_DeBuffTimeCostPro_EquipSuit + rose_DeBuffTimeCostPro_EquipSuit_value;           //Debuff时间缩短
            rose_DodgeAddHpPro_EquipSuit = rose_DodgeAddHpPro_EquipSuit + rose_DodgeAddHpPro_EquipSuit_value;                       //闪避恢复血量


            exp_AddPro_EquipSuit = exp_AddPro_EquipSuit + exp_AddPro_EquipSuit_value;                               //经验加成
            gold_AddPro_EquipSuit = gold_AddPro_EquipSuit + gold_AddPro_EquipSuit_value;                            //金币加成
            exp_AddValue_EquipSuit = exp_AddValue_EquipSuit + exp_AddValue_EquipSuit_value;                         //经验加成
            gold_AddValue_EquipSuit = gold_AddValue_EquipSuit + gold_AddValue_EquipSuit_value;                      //金币加成
            blessing_AddPro_EquipSuit = blessing_AddPro_EquipSuit + blessing_AddPro_EquipSuit_value;                //洗炼极品掉落
            hidePro_AddPro_EquipSuit = hidePro_AddPro_EquipSuit + hidePro_AddPro_EquipSuit_value;                   //隐藏属性出现概率
            gemHole_AddPro_EquipSuit = gemHole_AddPro_EquipSuit + gemHole_AddPro_EquipSuit_value;                   //装备上的宝石槽位出现概率

            rose_YaoJiValue_EquipSuit = rose_YaoJiValue_EquipSuit + rose_YaoJiValue_EquipSuit_value;                        //药剂类熟练度
            rose_DuanZaoValue_EquipSuit = rose_DuanZaoValue_EquipSuit + rose_DuanZaoValue_EquipSuit_value;				    //锻造类熟练度 
            rose_FuHuoPro_EquipSuit = rose_FuHuoPro_EquipSuit + rose_FuHuoPro_EquipSuit_value;          		            //复活
            rose_ActWuShi_EquipSuit = rose_ActWuShi_EquipSuit + rose_ActWuShi_EquipSuit_value; 				                //攻击无视防御
            rose_ShenNong_EquipSuit = rose_ShenNong_EquipSuit + rose_ShenNong_EquipSuit_value;              	            //神农
            rose_DropExtra_EquipSuit = rose_DropExtra_EquipSuit + rose_DropExtra_EquipSuit_value;          	                //额外掉落
            rose_WeiZhuang_EquipSuit = rose_WeiZhuang_EquipSuit + rose_WeiZhuang_EquipSuit_value;           	            //伪装  +增大发现范围   -缩小范围
            rose_ZaiNanValue_EquipSuit = rose_ZaiNanValue_EquipSuit + rose_ZaiNanValue_EquipSuit_value;                     //灾难
            rose_ShiXuePro_EquipSuit = rose_ShiXuePro_EquipSuit + rose_ShiXuePro_EquipSuit_value;				            //嗜血概率

            rose_HealHpValue_EquipSuit = rose_HealHpValue_EquipSuit + rose_HealHpValue_EquipSuit_value;              	        //角色恢复的固定值
            rose_HealHpPro_EquipSuit = rose_HealHpPro_EquipSuit + rose_HealHpPro_EquipSuit_value;          	                    //角色恢复的百分比加成固定值
            rose_HealHpFightPro_EquipSuit = rose_HealHpFightPro_EquipSuit + rose_HealHpFightPro_EquipSuit_value;           	    //角色战斗时恢复额外恢复血量的百分比
            rose_AITuoZhanDisValue_EquipSuit = rose_AITuoZhanDisValue_EquipSuit + rose_AITuoZhanDisValue_EquipSuit_value;       //怪物脱战距离
            rose_ZhuanZhuPro_EquipSuit = rose_ZhuanZhuPro_EquipSuit + rose_ZhuanZhuPro_EquipSuit_value;				            //专注概率
            rose_BiZhongPro_EquipSuit = rose_BiZhongPro_EquipSuit + rose_BiZhongPro_EquipSuit_value;                            //怪物脱战距离
            rose_YaoJiCirPro_EquipSuit = rose_YaoJiCirPro_EquipSuit + rose_YaoJiCirPro_EquipSuit_value;				            //生产药剂暴击概率
            rose_BuZhuoPro_EquipSuit = rose_BuZhuoPro_EquipSuit + rose_BuZhuoPro_EquipSuit_value;                               //捕捉概率

            rose_LanValueMaxAdd_EquipSuit = rose_LanValueMaxAdd_EquipSuit + rose_LanValueMaxAdd_EquipSuit_value;                                //魔法量附加
            rose_SummonAIPropertyAddPro_EquipSuit = rose_SummonAIPropertyAddPro_EquipSuit + rose_SummonAIPropertyAddPro_EquipSuit_value;        //召唤生物属性加成
            rose_HuDunValueAddPro_EquipSuit = rose_HuDunValueAddPro_EquipSuit + rose_HuDunValueAddPro_EquipSuit_value;				            //护盾属性附加

            //Debug.Log("子套装ID");
            string equipSuitSkillID = functionDataSet.DataSet_ReadData("EquipSuitSkillID", "ID", equipSuitPropertyID, "EquipSuitProperty_Template");
            if (equipSuitSkillID != "0") {
                //写入套装ID技能
                if (equipSuitSkillIDStr == "")
                {
                    equipSuitSkillIDStr = equipSuitSkillID;
                }
                else
                {
                    equipSuitSkillIDStr = equipSuitSkillIDStr + "," + equipSuitSkillID;
                }
            }
        }

        //写入套装技能
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipSuitSkillID", equipSuitSkillIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //更新装备技能（放在这里需要上面先写入套装技能ID后再进行更新）
        Game_PublicClassVar.Get_function_Skill.UpdataEquipSkillID();

        //更新收集装备
        int hp_ShouJiItem = 0;
        float hpPro_ShouJiItem = 0;
        int act_ShouJiItemMin = 0;
        int act_ShouJiItemMax = 0;
        float actPro_ShouJiItemMin = 0;
        float actPro_ShouJiItemMax = 0;
        int def_ShouJiItemMin = 0;
        int def_ShouJiItemMax = 0;
        float defPro_ShouJiItemMin = 0;
        float defPro_ShouJiItemMax = 0;
        int adf_ShouJiItemMin = 0;
        int adf_ShouJiItemMax = 0;
        float adfPro_ShouJiItemMin = 0;
        float adfPro_ShouJiItemMax = 0;

        float cir_ShouJiItem = 0;
        float hit_ShouJiItem = 0;
        float dodge_ShouJiItem = 0;
        float defAdd_ShouJiItem = 0.0f;       //初始物理免伤
        float adfAdd_ShouJiItem = 0.0f;       //初始魔法免伤
        float damgeAdd_ShouJiItem = 0;
        float damgeSub_ShouJiItem = 0;
        float speed_ShouJiItem = 0;
        int lucky_ShouJiItem = 0;

        string startListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_6", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (startListStr != "" && startListStr != "0")
        {
            string[] shouJiStartList = startListStr.Split(';');
            string[] ShouJiItemList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ShouJiItemList", "GameMainValue").Split(';');

            //循环章节星数
            for (int i = 0; i < shouJiStartList.Length; i++)
            {

                //循环章节里面3个难度
                for (int y = 1; y <= 3; y++)
                {

                    string souSuoStr_1 = "ProList" + y + "_StartNum";
                    string souSuoStr_2 = "ProList" + y + "_Value";
                    int nowStarNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(souSuoStr_1, "ID", ShouJiItemList[i], "ShouJiItemPro_Template"));
                    //对当前拥有的星数大于要求的星数就激活属性
                    if (int.Parse(shouJiStartList[i]) >= nowStarNum)
                    {

                        string ShouJipropretyStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(souSuoStr_2, "ID", ShouJiItemList[i], "ShouJiItemPro_Template");
                        //Debug.Log("ShouJiItemList[i] = " + ShouJiItemList[i]);
                        //Debug.Log("ShouJipropretyStr = " + ShouJipropretyStr);
                        string[] ShouJiproprety = ShouJipropretyStr.Split(',');

                        string propretyShouJiType = ShouJiproprety[0];
                        string propretyShouJiAddType = ShouJiproprety[1];
                        string propretyShouJiValue = ShouJiproprety[2];

                        switch (propretyShouJiType)
                        {
                            //血量
                            case "10":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        hp_ShouJiItem = hp_ShouJiItem + int.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        hpPro_ShouJiItem = hpPro_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //物理最大攻击
                            case "11":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        act_ShouJiItemMax = act_ShouJiItemMax + int.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        actPro_ShouJiItemMax = actPro_ShouJiItemMax + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //物理防御
                            case "17":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        def_ShouJiItemMax = def_ShouJiItemMax + int.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        defPro_ShouJiItemMax = defPro_ShouJiItemMax + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //魔法防御
                            case "20":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        adf_ShouJiItemMax = adf_ShouJiItemMax + int.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        adfPro_ShouJiItemMax = adfPro_ShouJiItemMax + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;


                            //暴击
                            case "30":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        cir_ShouJiItem = cir_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        cir_ShouJiItem = cir_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //命中
                            case "31":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        hit_ShouJiItem = hit_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        hit_ShouJiItem = hit_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //闪避
                            case "32":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        dodge_ShouJiItem = dodge_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        dodge_ShouJiItem = dodge_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //物理免伤
                            case "33":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        defAdd_ShouJiItem = defAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        defAdd_ShouJiItem = defAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //魔法免伤
                            case "34":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        adfAdd_ShouJiItem = adfAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        adfAdd_ShouJiItem = adfAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //速度
                            case "35":
                                //Debug.Log("开始出发加速效果");
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        speed_ShouJiItem = speed_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        speed_ShouJiItem = speed_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //伤害免伤
                            case "36":
                                //Debug.Log("Rose_DamgeSubtractMul_1 == " + roseProprety.Rose_DamgeSubtractMul_1 + "buffValue_add = " + buffValue_add);
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        damgeSub_ShouJiItem = damgeSub_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        damgeSub_ShouJiItem = damgeSub_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //伤害加成
                            case "37":
                                //Debug.Log("Rose_DamgeSubtractMul_1 == " + roseProprety.Rose_DamgeSubtractMul_1 + "buffValue_add = " + buffValue_add);
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        damgeAdd_ShouJiItem = damgeAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        damgeAdd_ShouJiItem = damgeAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;


                            //幸运
                            case "100":
                                //Debug.Log("Rose_DamgeSubtractMul_1 == " + roseProprety.Rose_DamgeSubtractMul_1 + "buffValue_add = " + buffValue_add);
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        lucky_ShouJiItem = lucky_ShouJiItem + int.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        lucky_ShouJiItem = lucky_ShouJiItem + int.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
        }

        //写入天赋加成
        int hp_TianFu = 0;
        float hpPro_TianFu = 0.0f;
        int act_TianFuMin = 0;
        int act_TianFuMax = 0;
        float actPro_Tianfu = 0.0f;
        int magact_TianFuMin = 0;
        int magact_TianFuMax = 0;
        float magactPro_TianFu = 0.0f;
        int def_TianFuMin = 0;
        int def_TianFuMax = 0;
        float defPro_TianFu = 0;
        int adf_TianFuMin = 0;
        int adf_TianFuMax = 0;
        float adfPro_TianFu = 0;
        float cir_TianFu = 0;
        float hit_TianFu = 0;
        float dodge_TianFu = 0;
        float damgeAdd_TianFu = 0;
        float damgeSub_TianFu = 0;
        float defAdd_TianFu = 0.0f;
        float adfAdd_TianFu = 0.0f;
        float speed_TianFu = 0;
        int lucky_TianFu = 0;

        int geDangValue_TianFu = 0;                    //格挡值
        float zhongJiPro_TianFu = 0.0f;                //重击概率
        int zhongJiValue_TianFu = 0;                   //重击附加伤害值
        int guDingValue_TianFu = 0;                    //每次普通攻击附加的伤害值
        int huShiDefValue_TianFu = 0;                  //忽视目标防御值                       
        int huShiAdfValue_TianFu = 0;                  //忽视目标魔防值
        float huShiDefValuePro_TianFu = 0;             //忽视目标防御值                       
        float huShiAdfValuePro_TianFu = 0;             //忽视目标魔防值
        float xiXuePro_TianFu = 0.0f;                  //吸血概率

        int criRating_TianFu = 0;                    //暴击等级
        int resilienceRating_TianFu = 0;             //韧性等级
        int hitRating_TianFu = 0;                    //命中等级
        int dodgeRating_TianFu = 0;                  //闪避等级

        float resilience_TianFu = 0.0f;              //韧性概率
        float magicRebound_TianFu = 0.0f;            //法术反击值
        float actRebound_TianFu = 0.0f;              //攻击反击

        float resistance_1_TianFu = 0;                //光抗性
        float resistance_2_TianFu = 0;                //暗抗性
        float resistance_3_TianFu = 0;                //火抗性
        float resistance_4_TianFu = 0;                //水抗性
        float resistance_5_TianFu = 0;                //电抗性

        float raceResistance_1_TianFu = 0;            //野兽攻击抗性
        float raceResistance_2_TianFu = 0;            //人物攻击抗性
        float raceResistance_3_TianFu = 0;            //恶魔攻击抗性
		float raceDamge_1_TianFu = 0;            	  //野兽攻击伤害
		float raceDamge_2_TianFu = 0;            	  //人物攻击伤害
		float raceDamge_3_TianFu = 0;                 //恶魔攻击伤害
        float rose_Boss_ActAdd_TianFu = 0;                      //Boss普通攻击加成
        float rose_Boss_SkillAdd_TianFu = 0;                    //Boss技能攻击加成
        float rose_Boss_ActHitCost_TianFu = 0;                  //受到Boss普通攻击减免
        float rose_Boss_SkillHitCost_TianFu = 0;                //受到Boss技能攻击减免
        float rose_PetActAdd_TianFu = 0;                        //宠物攻击加成
        float rose_PetActHitCost_TianFu = 0;                    //宠物受伤减免
        float rose_SkillCDTimePro_TianFu = 0;                      //技能冷却时间缩减
        float rose_BuffTimeAddPro_TianFu = 0;                      //自身buff效果延长
        float rose_DeBuffTimeCostPro_TianFu = 0;                   //Debuff时间缩短
        float rose_DodgeAddHpPro_TianFu = 0;                       //闪避恢复血量

        float exp_AddPro_TianFu = 0.0f;               //经验加成
        float gold_AddPro_TianFu = 0.0f;              //金币加成
        int exp_AddValue_TianFu = 0;                  //经验加成
        int gold_AddValue_TianFu = 0;                 //金币加成
        float blessing_AddPro_TianFu = 0.0f;          //洗炼极品掉落
        float hidePro_AddPro_TianFu = 0.0f;           //隐藏属性出现概率
        float gemHole_AddPro_TianFu = 0.0f;           //装备上的宝石槽位出现概率

        int rose_YaoJiValue_TianFu = 0;                      //药剂类熟练度
        int rose_DuanZaoValue_TianFu = 0;				     //锻造类熟练度 
        float rose_FuHuoPro_TianFu = 0.0f;          		 //复活
        float rose_ActWuShi_TianFu = 0.0f; 				     //攻击无视防御
        float rose_ShenNong_TianFu = 0.0f;              	 //神农
        float rose_DropExtra_TianFu = 0.0f;          	     //额外掉落
        float rose_WeiZhuang_TianFu = 0.0f;           	     //伪装  +增大发现范围   -缩小范围
        float rose_ZaiNanValue_TianFu = 0.0f;                //灾难
        float rose_ShiXuePro_TianFu = 0.0f;				     //嗜血概率

        int rose_HealHpValue_TianFu = 0;                    //角色恢复的固定值
        float rose_HealHpPro_TianFu = 0.0f;                 //角色恢复的百分比加成固定值
        float rose_HealHpFightPro_TianFu = 0.0f;            //角色战斗时恢复额外恢复血量的百分比
        float rose_AITuoZhanDisValue_TianFu = 0.0f;		    //怪物脱战距离
        float rose_ZhuanZhuPro_TianFu = 0;				    //专注概率
        float rose_BiZhongPro_TianFu = 0.0f;		        //怪物脱战距离
        float rose_YaoJiCirPro_TianFu = 0;					//生产药剂暴击概率
        float rose_BuZhuoPro_TianFu = 0;                    //捕捉概率

        float rose_LanValueMaxAdd_TianFu = 0;                       //魔法量附加
        float rose_SummonAIPropertyAddPro_TianFu = 0;                //召唤生物属性加成
        float rose_HuDunValueAddPro_TianFu = 0;                     //护盾属性附加

        float rose_SummonAIHpPropertyAddPro_TianFu = 0;
        float rose_SummonAIActPropertyAddPro_TianFu = 0;
        float rose_SummonAIDefPropertyAddPro_TianFu = 0;


        string LearnTianFuIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianFuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] LearnTianFuID = LearnTianFuIDStr.Split(';');

        //循环每个天赋的加成属性信息
        for (int i = 0; i <= LearnTianFuID.Length - 1; i++)
        {
            if (LearnTianFuID[i].Split(',').Length >= 2)
            {

                string nowTianFuID = LearnTianFuID[i].Split(',')[0];
                int nowTianFuLv = int.Parse(LearnTianFuID[i].Split(',')[1]);

                string addPropreListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropreListStr", "ID", nowTianFuID, "Talent_Template");
                if (addPropreListStr != "" && addPropreListStr != "0")
                {
                    string[] addPropreList = addPropreListStr.Split('|');
                    if (nowTianFuLv > 0)
                    {
                        nowTianFuLv = nowTianFuLv - 1;
                        if (nowTianFuLv <= 0)
                        {
                            nowTianFuLv = 0;
                        }

                        //Debug.Log("nowTianFuID = " + nowTianFuID + ";nowTianFuLv = " + nowTianFuLv);
                        if (nowTianFuLv <= addPropreList.Length - 1)
                        {
                            if (addPropreList[nowTianFuLv] != "" && addPropreList[nowTianFuLv] != "0")
                            {
                                //Debug.Log("addPropreList[nowTianFuLv] = " + addPropreList[nowTianFuLv]);
                                string[] addProprtValue = addPropreList[nowTianFuLv].Split(';');
                                for (int y = 0; y < addProprtValue.Length; y++)
                                {
                                    //增加对应属性
                                    string addPropretyType = addProprtValue[y].Split(',')[0];
                                    string addPropretyTypeValue = addProprtValue[y].Split(',')[1];

                                    switch (addPropretyType)
                                    {
                                        //10.Hp
                                        case "10":
                                            hp_TianFu = hp_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //11.Hp百分比加成
                                        case "11":
                                            hpPro_TianFu = hpPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //12.物理最小攻击
                                        case "12":
                                            act_TianFuMin = act_TianFuMin + int.Parse(addPropretyTypeValue);
                                            break;

                                        //13.物理最大攻击
                                        case "13":
                                            act_TianFuMax = act_TianFuMax + int.Parse(addPropretyTypeValue);
                                            break;

                                        //14.物理伤害加成
                                        case "14":
                                            actPro_Tianfu = actPro_Tianfu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //15.魔法最小攻击
                                        case "15":
                                            magact_TianFuMin = magact_TianFuMin + int.Parse(addPropretyTypeValue);
                                            break;

                                        //16.魔法最大攻击
                                        case "16":
                                            magact_TianFuMax = magact_TianFuMax + int.Parse(addPropretyTypeValue);
                                            break;

                                        //17.魔法伤害加成
                                        case "17":
                                            magactPro_TianFu = magactPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //18.物理最小防御
                                        case "18":
                                            def_TianFuMin = def_TianFuMin + int.Parse(addPropretyTypeValue);
                                            break;

                                        //19.物理最大防御
                                        case "19":
                                            def_TianFuMax = def_TianFuMax + int.Parse(addPropretyTypeValue);
                                            break;

                                        //20 物理防御加成
                                        case "20":
                                            defPro_TianFu = defPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //21.魔法最小防御
                                        case "21":
                                            adf_TianFuMin = adf_TianFuMin + int.Parse(addPropretyTypeValue);
                                            break;

                                        //22.魔法最大防御
                                        case "22":
                                            adf_TianFuMax = adf_TianFuMax + int.Parse(addPropretyTypeValue);
                                            break;

                                        //23.魔法防御加成
                                        case "23":
                                            adfPro_TianFu = adfPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //30.暴击
                                        case "30":
                                            cir_TianFu = cir_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //31.命中
                                        case "31":
                                            hit_TianFu = hit_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //32.闪避
                                        case "32":
                                            dodge_TianFu = dodge_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //33.物理免伤
                                        case "33":
                                            defAdd_TianFu = defAdd_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //34.魔法免伤
                                        case "34":
                                            adfAdd_TianFu = adfAdd_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //35.移动速度
                                        case "35":
                                            speed_TianFu = speed_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //36.伤害减免
                                        case "36":
                                            damgeSub_TianFu = damgeSub_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //37.伤害加成
                                        case "37":
                                            damgeAdd_TianFu = damgeAdd_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //血量百分比
                                        case "50":
                                            hpPro_TianFu = hpPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //物理攻击(百分比)
                                        case "51":
                                            actPro_Tianfu = actPro_Tianfu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //魔法攻击(百分比)
                                        case "52":
                                            magactPro_TianFu = magactPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //物理防御(百分比)
                                        case "53":
                                            defPro_TianFu = defPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //魔法防御(百分比)
                                        case "54":
                                            adfPro_TianFu = adfPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //100.幸运值
                                        case "100":
                                            lucky_TianFu = lucky_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //101：格挡值
                                        case "101":
                                            geDangValue_TianFu = geDangValue_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //111：重击概率
                                        case "111":
                                            zhongJiPro_TianFu = zhongJiPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //112:  重击附加伤害值
                                        case "112":
                                            zhongJiValue_TianFu = zhongJiValue_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //121:  每次普通攻击附加的伤害值
                                        case "121":
                                            guDingValue_TianFu = guDingValue_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //131:  忽视目标防御值   
                                        case "131":
                                            huShiDefValue_TianFu = huShiDefValue_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //132:  忽视目标魔防值
                                        case "132":
                                            huShiAdfValue_TianFu = huShiAdfValue_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //131:  忽视目标防御值   
                                        case "133":
                                            huShiDefValuePro_TianFu = huShiDefValuePro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //132:  忽视目标魔防值
                                        case "134":
                                            huShiAdfValuePro_TianFu = huShiAdfValuePro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //141:  吸血概率
                                        case "141":
                                            xiXuePro_TianFu = xiXuePro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //法术反击
                                        case "151":
                                            magicRebound_TianFu = magicRebound_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //攻击反击
                                        case "152":
                                            actRebound_TianFu = actRebound_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //韧性概率
                                        case "161":
                                            resilience_TianFu = resilience_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //回血百分比
                                        case "171":
                                            rose_HealHpPro_TianFu = rose_HealHpPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //回血固定值
                                        case "172":
                                            rose_HealHpValue_TianFu = rose_HealHpValue_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //战斗回血比例
                                        case "173":
                                            rose_HealHpFightPro_TianFu = rose_HealHpFightPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //暴击等级
                                        case "201":
                                            criRating_TianFu = criRating_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //韧性等级
                                        case "202":
                                            resilienceRating_TianFu = resilienceRating_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //命中等级
                                        case "203":
                                            hitRating_TianFu = hitRating_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //闪避等级
                                        case "204":
                                            dodgeRating_TianFu = dodgeRating_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //光抗性
                                        case "301":
                                            resistance_1_TianFu = resistance_1_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //暗抗性
                                        case "302":
                                            resistance_2_TianFu = resistance_2_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //火抗性
                                        case "303":
                                            resistance_3_TianFu = resistance_3_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //水抗性
                                        case "304":
                                            resistance_4_TianFu = resistance_4_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //电抗性
                                        case "305":
                                            resistance_5_TianFu = resistance_5_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //野兽攻击抗性
                                        case "321":
                                            raceResistance_1_TianFu = raceResistance_1_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //人物攻击抗性
                                        case "322":
                                            raceResistance_2_TianFu = raceResistance_2_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //恶魔攻击抗性
                                        case "323":
                                            raceResistance_3_TianFu = raceResistance_3_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //野兽攻击抗性
                                        case "331":
                                            raceDamge_1_TianFu = raceDamge_1_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //人物攻击抗性
                                        case "332":
                                            raceDamge_2_TianFu = raceDamge_2_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //恶魔攻击抗性
                                        case "333":
                                            raceDamge_3_TianFu = raceDamge_3_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //Boss普通攻击加成
                                        case "341":
                                            rose_Boss_ActAdd_TianFu = rose_Boss_ActAdd_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //Boss技能攻击加成
                                        case "342":
                                            rose_Boss_SkillAdd_TianFu = rose_Boss_SkillAdd_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //受到Boss普通攻击减免
                                        case "343":
                                            rose_Boss_ActHitCost_TianFu = rose_Boss_ActHitCost_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //受到Boss技能攻击减免
                                        case "344":
                                            rose_Boss_SkillHitCost_TianFu = rose_Boss_SkillHitCost_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //宠物攻击加成
                                        case "345":
                                            rose_PetActAdd_TianFu = rose_PetActAdd_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //宠物受伤减免
                                        case "346":
                                            rose_PetActHitCost_TianFu = rose_PetActHitCost_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //技能冷却时间缩减
                                        case "347":
                                            rose_SkillCDTimePro_TianFu = rose_SkillCDTimePro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //自身buff效果延长
                                        case "348":
                                            rose_BuffTimeAddPro_TianFu = rose_BuffTimeAddPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //Debuff时间缩短
                                        case "349":
                                            rose_DeBuffTimeCostPro_TianFu = rose_DeBuffTimeCostPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //闪避恢复血量
                                        case "350":
                                            rose_DodgeAddHpPro_TianFu = rose_DodgeAddHpPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //魔法量附加
                                        case "351":
                                            rose_LanValueMaxAdd_TianFu = float.Parse(addPropretyTypeValue);
                                            break;

                                        //召唤生物属性加成
                                        case "352":
                                            rose_SummonAIPropertyAddPro_TianFu = float.Parse(addPropretyTypeValue);
                                            break;

                                        //护盾属性附加
                                        case "353":
                                            rose_HuDunValueAddPro_TianFu = float.Parse(addPropretyTypeValue);
                                            break;

                                        //召唤生物血量属性加成（天赋唯一）
                                        case "361":
                                            rose_SummonAIHpPropertyAddPro_TianFu = float.Parse(addPropretyTypeValue);
                                            break;

                                        //召唤生物攻击属性加成（天赋唯一）
                                        case "362":
                                            rose_SummonAIActPropertyAddPro_TianFu = float.Parse(addPropretyTypeValue);
                                            break;

                                        //召唤生物防御属性加成（天赋唯一）
                                        case "363":
                                            rose_SummonAIDefPropertyAddPro_TianFu = float.Parse(addPropretyTypeValue);
                                            break;

                                        //经验加成
                                        case "401":
                                            exp_AddPro_TianFu = exp_AddPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //金币加成
                                        case "402":
                                            gold_AddPro_TianFu = gold_AddPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //洗炼极品掉落（祝福值）
                                        case "403":
                                            blessing_AddPro_TianFu = blessing_AddPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //装备隐藏属性出现概率
                                        case "404":
                                            hidePro_AddPro_TianFu = hidePro_AddPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //装备上的宝石槽位出现概率
                                        case "405":
                                            gemHole_AddPro_TianFu = gemHole_AddPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //经验加成固定
                                        case "406":
                                            exp_AddValue_TianFu = exp_AddValue_TianFu + int.Parse(addPropretyTypeValue);
                                            break;

                                        //金币加成固定
                                        case "407":
                                            gold_AddValue_TianFu = gold_AddValue_TianFu + int.Parse(addPropretyTypeValue);
                                            break;
                                        //药剂类熟练度
                                        case "408":
                                            rose_YaoJiValue_TianFu = rose_YaoJiValue_TianFu + int.Parse(addPropretyTypeValue);
                                            break;
                                        //锻造类熟练度
                                        case "409":
                                            rose_DuanZaoValue_TianFu = rose_DuanZaoValue_TianFu + int.Parse(addPropretyTypeValue);
                                            break;
                                        //复活
                                        case "411":
                                            rose_FuHuoPro_TianFu = rose_FuHuoPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;
                                        //攻击无视防御
                                        case "412":
                                            rose_ActWuShi_TianFu = rose_ActWuShi_TianFu + float.Parse(addPropretyTypeValue);
                                            break;
                                        //神农
                                        case "413":
                                            rose_ShenNong_TianFu = rose_ShenNong_TianFu + float.Parse(addPropretyTypeValue);
                                            break;
                                        //额外掉落
                                        case "414":
                                            rose_DropExtra_TianFu = rose_DropExtra_TianFu + float.Parse(addPropretyTypeValue);
                                            break;
                                        //伪装  +增大发现范围   -缩小范围
                                        case "415":
                                            rose_WeiZhuang_TianFu = rose_WeiZhuang_TianFu + float.Parse(addPropretyTypeValue);
                                            break;
                                        //灾难
                                        case "416":
                                            rose_ZaiNanValue_TianFu = rose_ZaiNanValue_TianFu + float.Parse(addPropretyTypeValue);
                                            break;
                                        //嗜血概率
                                        case "417":
                                            rose_ShiXuePro_TianFu = rose_ShiXuePro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //怪物脱战距离
                                        case "418":
                                            rose_AITuoZhanDisValue_TianFu = rose_AITuoZhanDisValue_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //专注概率
                                        case "419":
                                            rose_ZhuanZhuPro_TianFu = rose_ZhuanZhuPro_TianFu + float.Parse(addPropretyTypeValue);
                                            break;

                                        //必中
                                        case "420":
                                            rose_BiZhongPro_TianFu = float.Parse(addPropretyTypeValue);
                                            break;

                                        //生产药剂暴击概率
                                        case "421":
                                            rose_YaoJiCirPro_TianFu = float.Parse(addPropretyTypeValue);
                                            break;

                                        //生产药剂暴击概率
                                        case "422":
                                            rose_BuZhuoPro_TianFu = float.Parse(addPropretyTypeValue);
                                            break;

                                        //技能附加
                                        case "SkillAdd":
                                            string skillAddValueStr = addProprtValue[y].Substring(9);
                                            //Debug.Log("skillAddValueStr = " + skillAddValueStr);
                                            //添加技能
                                            SkillAddValue(skillAddValueStr);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }



        //更新装备技能（放在这里需要上面先写入套装技能ID后再进行更新）
        Game_PublicClassVar.Get_function_Skill.UpdataEquipSkillID();

		//更新技能附加属性
		int hp_EquipSkill = 0;
		float hpPro_EquipSkill = 0.0f;
		int act_EquipSkillMin = 0;
		int act_EquipSkillMax = 0;
		float actPro_EquipSkill = 0.0f;
		int magact_EquipSkillMin = 0;
		int magact_EquipSkillMax = 0;
		float magactPro_EquipSkill = 0.0f;
		int def_EquipSkillMin = 0;
		int def_EquipSkillMax = 0;
		float defPro_EquipSkill = 0;
		int adf_EquipSkillMin = 0;
		int adf_EquipSkillMax = 0;
        float adfPro_EquipSkill = 0;
		float cir_EquipSkill = 0;
		float hit_EquipSkill = 0;
		float dodge_EquipSkill = 0;
		float damgeAdd_EquipSkill = 0;
		float damgeSub_EquipSkill = 0;
		float defAdd_EquipSkill = 0.0f;
		float adfAdd_EquipSkill = 0.0f;
		float speed_EquipSkill = 0;
		int lucky_EquipSkill = 0;

		int geDangValue_EquipSkill = 0;                    //格挡值
		float zhongJiPro_EquipSkill = 0.0f;                //重击概率
		int zhongJiValue_EquipSkill = 0;                   //重击附加伤害值
		int guDingValue_EquipSkill = 0;                    //每次普通攻击附加的伤害值
		int huShiDefValue_EquipSkill = 0;                  //忽视目标防御值                       
		int huShiAdfValue_EquipSkill = 0;                  //忽视目标魔防值
		float huShiDefValuePro_EquipSkill = 0;             //忽视目标防御值                       
        float huShiAdfValuePro_EquipSkill = 0;             //忽视目标魔防值
		float xiXuePro_EquipSkill = 0.0f;                  //吸血概率

		int criRating_EquipSkill = 0;                    //暴击等级
		int resilienceRating_EquipSkill = 0;             //韧性等级
		int hitRating_EquipSkill = 0;                    //命中等级
		int dodgeRating_EquipSkill = 0;                  //闪避等级

		float resilience_EquipSkill = 0.0f;              //韧性概率
		float magicRebound_EquipSkill = 0.0f;            //法术反击值
		float actRebound_EquipSkill = 0.0f;              //攻击反击

		float resistance_1_EquipSkill = 0;                //光抗性
		float resistance_2_EquipSkill = 0;                //暗抗性
		float resistance_3_EquipSkill = 0;                //火抗性
		float resistance_4_EquipSkill = 0;                //水抗性
		float resistance_5_EquipSkill = 0;                //电抗性

		float raceResistance_1_EquipSkill = 0;            //野兽攻击抗性
		float raceResistance_2_EquipSkill = 0;            //人物攻击抗性
		float raceResistance_3_EquipSkill = 0;            //恶魔攻击抗性
		float raceDamge_1_EquipSkill = 0;            	//野兽攻击伤害
		float raceDamge_2_EquipSkill = 0;            	//人物攻击伤害
		float raceDamge_3_EquipSkill = 0;               //恶魔攻击伤害
        float rose_Boss_ActAdd_EquipSkill = 0;                  //Boss普通攻击加成
        float rose_Boss_SkillAdd_EquipSkill = 0;                //Boss技能攻击加成
        float rose_Boss_ActHitCost_EquipSkill = 0;              //受到Boss普通攻击减免
        float rose_Boss_SkillHitCost_EquipSkill = 0;            //受到Boss技能攻击减免
        float rose_PetActAdd_EquipSkill = 0;                        //宠物攻击加成
        float rose_PetActHitCost_EquipSkill = 0;                    //宠物受伤减免
        float rose_SkillCDTimePro_EquipSkill = 0;                      //技能冷却时间缩减
        float rose_BuffTimeAddPro_EquipSkill = 0;                      //自身buff效果延长
        float rose_DeBuffTimeCostPro_EquipSkill = 0;                   //Debuff时间缩短
        float rose_DodgeAddHpPro_EquipSkill = 0;                       //闪避恢复血量

        float exp_AddPro_EquipSkill = 0.0f;               //经验加成
		float gold_AddPro_EquipSkill = 0.0f;              //金币加成
        int exp_AddValue_EquipSkill = 0;               //经验加成固定
        int gold_AddValue_EquipSkill = 0;              //金币加成固定
		float blessing_AddPro_EquipSkill = 0.0f;          //洗炼极品掉落
		float hidePro_AddPro_EquipSkill = 0.0f;           //隐藏属性出现概率
		float gemHole_AddPro_EquipSkill = 0.0f;           //装备上的宝石槽位出现概率

        int rose_YaoJiValue_EquipSkill = 0;                      //药剂类熟练度
        int rose_DuanZaoValue_EquipSkill = 0;				    //锻造类熟练度 

		float FuHuoPro_EquipSkill = 0.0f;          		//复活
		float ActWuShi_EquipSkill = 0.0f; 				//攻击无视防御
		float ShenNong_EquipSkill = 0.0f;              	//神农
		float DropExtra_EquipSkill = 0.0f;          	//额外掉落
		float WeiZhuang_EquipSkill = 0.0f;           	//伪装  +增大发现范围   -缩小范围
		float ZaiNanValue_EquipSkill = 0.0f;            //灾难
		float ShiXuePro_EquipSkill = 0.0f;				//嗜血概率

        int rose_HealHpValue_EquipSkill = 0;                   //角色恢复的固定值
        float rose_HealHpPro_EquipSkill = 0.0f;                //角色恢复的百分比加成固定值
        float rose_HealHpFightPro_EquipSkill = 0.0f;           //角色战斗时恢复额外恢复血量的百分比
        float rose_AITuoZhanDisValue_EquipSkill = 0.0f;		  //怪物脱战距离
        float rose_ZhuanZhuPro_EquipSkill = 0;					  //专注概率

        float rose_BiZhongPro_EquipSkill = 0.0f;		        //怪物脱战距离
        float rose_YaoJiCirPro_EquipSkill = 0;					//生产药剂暴击概率
        float rose_BuZhuoPro_EquipSkill = 0;                    //捕捉概率

        float rose_LanValueMaxAdd_EquipSkill = 0;                       //魔法量附加
        float rose_SummonAIPropertyAddPro_EquipSkill = 0;                //召唤生物属性加成
        float rose_HuDunValueAddPro_EquipSkill = 0;                     //护盾属性附加

        string EquipSkillIDSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		string[] EquipSkillIDSetlist = EquipSkillIDSetStr.Split(',');
		//循环每个天赋的加成属性信息
		for (int i = 0; i <= EquipSkillIDSetlist.Length - 1; i++) {
			//读取技能类型为5
			string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", EquipSkillIDSetlist[i], "Skill_Template");

            //增加属性
			if (skillType == "5") {

				string[] equipSkillParList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", EquipSkillIDSetlist[i], "Skill_Template").Split(';');

				for(int y = 0; y<equipSkillParList.Length;y++){

					string addEquipSkillProType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", EquipSkillIDSetlist[i], "Skill_Template").Split(',')[0];
					string addEquipSkillProValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", EquipSkillIDSetlist[i], "Skill_Template").Split(',')[1];

					switch (addEquipSkillProType) {
						
						//10.血量
					case "10":
						hp_EquipSkill = hp_EquipSkill + int.Parse(addEquipSkillProValue);
						break;

						//11.Hp百分比加成
					case "11":
						hpPro_EquipSkill = hpPro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//12.物理最小攻击
					case "12":
						act_EquipSkillMin = act_EquipSkillMin + int.Parse(addEquipSkillProValue);
						break;

						//13.物理最大攻击
					case "13":
						act_EquipSkillMax = act_EquipSkillMax + int.Parse(addEquipSkillProValue);
						break;

						//14.物理伤害加成
					case "14":
						actPro_EquipSkill = actPro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//15.魔法最小攻击
					case "15":
						magact_EquipSkillMin = magact_EquipSkillMin + int.Parse(addEquipSkillProValue);
						break;

						//16.魔法最大攻击
					case "16":
						magact_EquipSkillMax = magact_EquipSkillMax + int.Parse(addEquipSkillProValue);
						break;

						//17.魔法伤害加成
					case "17":
						magactPro_EquipSkill = magactPro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//18.物理最小防御
					case "18":
						def_EquipSkillMin = def_EquipSkillMin + int.Parse(addEquipSkillProValue);
						break;

						//19.物理最大防御
					case "19":
						def_EquipSkillMax = def_EquipSkillMax + int.Parse(addEquipSkillProValue);
						break;

						//20 物理防御加成
					case "20":
						defPro_EquipSkill = defPro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//21.魔法最小防御
					case "21":
						adf_EquipSkillMin = adf_EquipSkillMin + int.Parse(addEquipSkillProValue);
						break;

						//22.魔法最大防御
					case "22":
						adf_EquipSkillMax = adf_EquipSkillMax + int.Parse(addEquipSkillProValue);
						break;

						//23.魔法防御加成
					case "23":
						adfPro_EquipSkill = adfPro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//30.暴击
					case "30":
						cir_EquipSkill = cir_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//31.命中
					case "31":
						hit_EquipSkill = hit_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//32.闪避
					case "32":
						dodge_EquipSkill = dodge_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//33.物理免伤
					case "33":
						defAdd_EquipSkill = defAdd_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//34.魔法免伤
					case "34":
						adfAdd_EquipSkill = adfAdd_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//35.移动速度
					case "35":
						speed_EquipSkill = speed_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//36.伤害减免
					case "36":
						damgeSub_EquipSkill = damgeSub_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//37.伤害加成
					case "37":
						damgeAdd_EquipSkill = damgeAdd_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//100.幸运值
					case "100":
						lucky_EquipSkill = lucky_EquipSkill + int.Parse(addEquipSkillProValue);
						break;

						//101：格挡值
					case "101":
						geDangValue_EquipSkill = geDangValue_EquipSkill + int.Parse(addEquipSkillProValue);
						break;

						//111：重击概率
					case "111":
						zhongJiPro_EquipSkill = zhongJiPro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//112:  重击附加伤害值
					case "112":
						zhongJiValue_EquipSkill = zhongJiValue_EquipSkill + int.Parse(addEquipSkillProValue);
						break;

						//121:  每次普通攻击附加的伤害值
					case "121":
						guDingValue_EquipSkill = guDingValue_EquipSkill + int.Parse(addEquipSkillProValue);
						break;

						//131:  忽视目标防御值   
					case "131":
						huShiDefValue_EquipSkill = huShiDefValue_EquipSkill + int.Parse(addEquipSkillProValue);
						break;

						//132:  忽视目标魔防值
					case "132":
						huShiAdfValue_EquipSkill = huShiAdfValue_EquipSkill + int.Parse(addEquipSkillProValue);
						break;

						//131:  忽视目标防御值   
					case "133":
						huShiDefValuePro_EquipSkill = huShiDefValuePro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//132:  忽视目标魔防值
					case "134":
						huShiAdfValuePro_EquipSkill = huShiAdfValuePro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//141:  吸血概率
					case "141":
						xiXuePro_EquipSkill = xiXuePro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//法术反击
					case "151":
						magicRebound_EquipSkill = magicRebound_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//攻击反击
					case "152":
						actRebound_EquipSkill = actRebound_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//韧性概率
					case "161":
						resilience_EquipSkill = resilience_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

                    //回血百分比
                    case "171":
                        rose_HealHpPro_EquipSkill = rose_HealHpPro_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //回血固定值
                    case "172":
                        rose_HealHpValue_EquipSkill = rose_HealHpValue_EquipSkill + int.Parse(addEquipSkillProValue);
                        break;

                    //战斗回血比例
                    case "173":
                        rose_HealHpFightPro_EquipSkill = rose_HealHpFightPro_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

						//暴击等级
					case "201":
						criRating_EquipSkill = criRating_EquipSkill + int.Parse(addEquipSkillProValue);
						break;

						//韧性等级
					case "202":
						resilienceRating_EquipSkill = resilienceRating_EquipSkill + int.Parse(addEquipSkillProValue);
						break;

						//命中等级
					case "203":
						hitRating_EquipSkill = hitRating_EquipSkill + int.Parse(addEquipSkillProValue);
						break;

						//闪避等级
					case "204":
						dodgeRating_EquipSkill = dodgeRating_EquipSkill + int.Parse(addEquipSkillProValue);
						break;

						//光抗性
					case "301":
						resistance_1_EquipSkill = resistance_1_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//暗抗性
					case "302":
						resistance_2_EquipSkill = resistance_2_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//火抗性
					case "303":
						resistance_3_EquipSkill = resistance_3_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//水抗性
					case "304":
						resistance_4_EquipSkill = resistance_4_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//电抗性
					case "305":
						resistance_5_EquipSkill = resistance_5_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//野兽攻击抗性
					case "321":
						raceResistance_1_EquipSkill = raceResistance_1_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//人物攻击抗性
					case "322":
						raceResistance_2_EquipSkill = raceResistance_2_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//恶魔攻击抗性
					case "323":
						raceResistance_3_EquipSkill = raceResistance_3_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//野兽攻击伤害
					case "331":
						raceDamge_1_EquipSkill = raceDamge_1_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//人物攻击伤害
					case "332":
						raceDamge_2_EquipSkill = raceDamge_2_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

						//恶魔攻击伤害
					case "333":
						raceDamge_3_EquipSkill = raceDamge_3_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

                    //Boss普通攻击加成
                    case "341":
                        rose_Boss_ActAdd_EquipSkill = rose_Boss_ActAdd_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //Boss技能攻击加成
                    case "342":
                        rose_Boss_SkillAdd_EquipSkill = rose_Boss_SkillAdd_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //受到Boss普通攻击减免
                    case "343":
                        rose_Boss_ActHitCost_EquipSkill = rose_Boss_ActHitCost_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //受到Boss技能攻击减免
                    case "344":
                        rose_Boss_SkillHitCost_EquipSkill = rose_Boss_SkillHitCost_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //受到Boss普通攻击减免
                    case "345":
                        rose_PetActAdd_EquipSkill = rose_PetActAdd_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //受到Boss技能攻击减免
                    case "346":
                        rose_PetActHitCost_EquipSkill = rose_PetActHitCost_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //技能冷却时间缩减
                    case "347":
                        rose_SkillCDTimePro_EquipSkill = rose_SkillCDTimePro_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //自身buff效果延长
                    case "348":
                        rose_BuffTimeAddPro_EquipSkill = rose_BuffTimeAddPro_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //Debuff时间缩短
                    case "349":
                        rose_DeBuffTimeCostPro_EquipSkill = rose_DeBuffTimeCostPro_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //闪避恢复血量
                    case "350":
                        rose_DodgeAddHpPro_EquipSkill = rose_DodgeAddHpPro_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //魔法量附加
                    case "351":
                        rose_LanValueMaxAdd_EquipSkill = rose_LanValueMaxAdd_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //召唤生物属性加成
                    case "352":
                        rose_SummonAIPropertyAddPro_EquipSkill = rose_LanValueMaxAdd_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //护盾属性附加
                    case "353":
                        rose_HuDunValueAddPro_EquipSkill = rose_LanValueMaxAdd_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //经验加成
                    case "401":
					exp_AddPro_EquipSkill = exp_AddPro_EquipSkill + float.Parse(addEquipSkillProValue);
					break;

					//金币加成
					case "402":
                        gold_AddPro_EquipSkill = gold_AddPro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;
						
					//洗炼极品掉落（祝福值）
					case "403":
						blessing_AddPro_EquipSkill = blessing_AddPro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

					//装备隐藏属性出现概率
					case "404":
						hidePro_AddPro_EquipSkill = hidePro_AddPro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

					//装备上的宝石槽位出现概率
					case "405":
						gemHole_AddPro_EquipSkill = gemHole_AddPro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

                    //经验加成固定
                    case "406":
                        exp_AddValue_EquipSkill = exp_AddValue_EquipSkill + int.Parse(addEquipSkillProValue);
                        break;

                    //金币加成固定
                    case "407":
                        gold_AddValue_EquipSkill = gold_AddValue_EquipSkill + int.Parse(addEquipSkillProValue);
                        break;
                    //药剂类熟练度
                    case "408":
                        rose_YaoJiValue_EquipSkill = rose_YaoJiValue_EquipSkill + int.Parse(addEquipSkillProValue);
                        break;
                    //锻造类熟练度
                    case "409":
                        rose_DuanZaoValue_EquipSkill = rose_DuanZaoValue_EquipSkill + int.Parse(addEquipSkillProValue);
                        break;

					//复活
					case "411":
						FuHuoPro_EquipSkill = FuHuoPro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

					//攻击无视防御
					case "412":
						ActWuShi_EquipSkill = ActWuShi_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

					//神农
					case "413":
						ShenNong_EquipSkill = ShenNong_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

					//额外掉落
					case "414":
						DropExtra_EquipSkill = DropExtra_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

					//伪装
					case "415":
						WeiZhuang_EquipSkill = WeiZhuang_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

					//灾难
					case "416":
						ZaiNanValue_EquipSkill = ZaiNanValue_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

					//嗜血
					case "417":
						ShiXuePro_EquipSkill = ShiXuePro_EquipSkill + float.Parse(addEquipSkillProValue);
						break;

                    //怪物脱战距离
                    case "418":
                        rose_AITuoZhanDisValue_EquipSkill = rose_AITuoZhanDisValue_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //专注概率
                    case "419":
                        rose_ZhuanZhuPro_EquipSkill = rose_ZhuanZhuPro_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //必中
                    case "420":
                        rose_BiZhongPro_EquipSkill = rose_BiZhongPro_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;

                    //生产药剂暴击概率
                    case "421":
                        rose_YaoJiCirPro_EquipSkill = rose_YaoJiCirPro_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;
                    //捕捉概率
                    case "422":
                        rose_BuZhuoPro_EquipSkill = rose_BuZhuoPro_EquipSkill + float.Parse(addEquipSkillProValue);
                        break;
					}
				}
			}
		}


        //--------------精灵加成-----------------
        int hp_JingLing = 0;
        int act_JingLingMin = 0;
        int act_JingLingMax = 0;
        int magact_JingLingMin = 0;
        int magact_JingLingMax = 0;
        int def_JingLingMin = 0;
        int def_JingLingMax = 0;
        int adf_JingLingMin = 0;
        int adf_JingLingMax = 0;
        float cir_JingLing = 0;
        float hit_JingLing = 0;
        float dodge_JingLing = 0;
        float damgeAdd_JingLing = 0;
        float damgeSub_JingLing = 0;
        float speed_JingLing = 0;
        int lucky_JingLing = 0;

        float defAdd_JingLing = 0;
        float adfAdd_JingLing = 0;
        float hpPro_JingLing = 0;
        float actPro_JingLing = 0;
        float magactPro_JingLing = 0;
        float defPro_JingLing = 0;
        float adfPro_JingLing = 0;

        int geDangValue_JingLing = 0;                    //格挡值
        float zhongJiPro_JingLing = 0.0f;                //重击概率
        int zhongJiValue_JingLing = 0;                   //重击附加伤害值
        int guDingValue_JingLing = 0;                    //每次普通攻击附加的伤害值
        int huShiDefValue_JingLing = 0;                  //忽视目标防御值                       
        int huShiAdfValue_JingLing = 0;                  //忽视目标魔防值
        float huShiDefValuePro_JingLing = 0;             //忽视目标防御值                       
        float huShiAdfValuePro_JingLing = 0;             //忽视目标魔防值
        float xiXuePro_JingLing = 0.0f;                  //吸血概率

        int criRating_JingLing = 0;                      //暴击等级
        int resilienceRating_JingLing = 0;               //韧性等级
        int hitRating_JingLing = 0;                      //命中等级
        int dodgeRating_JingLing = 0;                    //闪避等级
        float resilience_JingLing = 0.0f;                //韧性概率
        float magicRebound_JingLing = 0.0f;              //法术反击值
        float actRebound_JingLing = 0.0f;                //攻击反击
        float resistance_1_JingLing = 0;                 //光抗性
        float resistance_2_JingLing = 0;                 //暗抗性
        float resistance_3_JingLing = 0;                 //火抗性
        float resistance_4_JingLing = 0;                 //水抗性
        float resistance_5_JingLing = 0;                 //电抗性
        float raceResistance_1_JingLing = 0;             //野兽攻击抗性
        float raceResistance_2_JingLing = 0;             //人物攻击抗性
        float raceResistance_3_JingLing = 0;             //恶魔攻击抗性
        float raceDamge_1_JingLing = 0;                  //野兽攻击抗性
        float raceDamge_2_JingLing = 0;                  //人物攻击抗性
        float raceDamge_3_JingLing = 0;                  //恶魔攻击抗性
        float rose_Boss_ActAdd_JingLing = 0;                      //Boss普通攻击加成
        float rose_Boss_SkillAdd_JingLing = 0;                    //Boss技能攻击加成
        float rose_Boss_ActHitCost_JingLing = 0;                  //受到Boss普通攻击减免
        float rose_Boss_SkillHitCost_JingLing = 0;                //受到Boss技能攻击减免
        float rose_PetActAdd_JingLing = 0;                        //宠物攻击加成
        float rose_PetActHitCost_JingLing = 0;                    //宠物受伤减免
        float rose_SkillCDTimePro_JingLing = 0;                      //技能冷却时间缩减
        float rose_BuffTimeAddPro_JingLing = 0;                      //自身buff效果延长
        float rose_DeBuffTimeCostPro_JingLing = 0;                   //Debuff时间缩短
        float rose_DodgeAddHpPro_JingLing = 0;                       //闪避恢复血量

        float exp_AddPro_JingLing = 0.0f;               //经验加成
        float gold_AddPro_JingLing = 0.0f;              //金币加成
        int exp_AddValue_JingLing = 0;                  //经验加成
        int gold_AddValue_JingLing = 0;                 //金币加成
        float blessing_AddPro_JingLing = 0.0f;          //洗炼极品掉落
        float hidePro_AddPro_JingLing = 0.0f;           //隐藏属性出现概率
        float gemHole_AddPro_JingLing = 0.0f;           //装备上的宝石槽位出现概率

        int rose_YaoJiValue_JingLing = 0;                     //药剂类熟练度
        int rose_DuanZaoValue_JingLing = 0;                   //锻造类熟练度 
        float rose_FuHuoPro_JingLing = 0.0f;                  //复活
        float rose_ActWuShi_JingLing = 0.0f;                  //攻击无视防御
        float rose_ShenNong_JingLing = 0.0f;                  //神农
        float rose_DropExtra_JingLing = 0.0f;                 //额外掉落
        float rose_WeiZhuang_JingLing = 0.0f;                 //伪装  +增大发现范围   -缩小范围
        float rose_ZaiNanValue_JingLing = 0.0f;               //灾难
        float rose_ShiXuePro_JingLing = 0.0f;                 //嗜血概率

        int rose_HealHpValue_JingLing = 0;                    //角色恢复的固定值
        float rose_HealHpPro_JingLing = 0.0f;                 //角色恢复的百分比加成固定值
        float rose_HealHpFightPro_JingLing = 0.0f;            //角色战斗时恢复额外恢复血量的百分比
        float rose_AITuoZhanDisValue_JingLing = 0.0f;         //怪物脱战距离
        float rose_ZhuanZhuPro_JingLing = 0;                  //专注概率
        float rose_BiZhongPro_JingLing = 0.0f;                //怪物脱战距离
        float rose_YaoJiCirPro_JingLing = 0;                  //生产药剂暴击概率
        float rose_BuZhuoPro_JingLing = 0;                    //捕捉概率

        float rose_LanValueMaxAdd_JingLing = 0;                       //魔法量附加
        float rose_SummonAIPropertyAddPro_JingLing = 0;                //召唤生物属性加成
        float rose_HuDunValueAddPro_JingLing = 0;                     //护盾属性附加

        //精灵属性
        string jingLingIDSet = functionDataSet.DataSet_ReadData("JingLingIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //gemID = "101,101";
        if (jingLingIDSet != "" && jingLingIDSet != "0")
        {
            string[] jingLingStrList = jingLingIDSet.Split(';');
            for (int z = 0; z < jingLingStrList.Length; z++)
            {
                //获取当前宝精灵的属性
                string jingLingIDStr = functionDataSet.DataSet_ReadData("Proprety", "ID", jingLingStrList[z], "Spirit_Template");
                //只检测被动,不检测主动
                string jingLingType = functionDataSet.DataSet_ReadData("Type", "ID", jingLingStrList[z], "Spirit_Template");
                if (jingLingType == "2") {
                    string[] jingLingIDStrList = jingLingIDStr.Split(';');
                    for (int gemStrNum = 0; gemStrNum < jingLingIDStrList.Length; gemStrNum++)
                    {
                        if (jingLingIDStrList[gemStrNum] != "" && jingLingIDStrList[gemStrNum] != "0")
                        {
                            string gemType = jingLingIDStrList[gemStrNum].Split(',')[0];
                            string gemValue = jingLingIDStrList[gemStrNum].Split(',')[1];

                            if (gemValue == "" || gemValue == null)
                            {
                                gemValue = "0";
                            }

                            switch (gemType)
                            {

                                //血量
                                case "10":
                                    hp_JingLing = hp_JingLing + int.Parse(gemValue);
                                    break;

                                //物理最小攻击
                                case "11":
                                    act_JingLingMax = act_JingLingMax + (int)(float.Parse(gemValue));
                                    break;

                                //魔法攻击
                                case "14":
                                    magact_JingLingMax = magact_JingLingMax + int.Parse(gemValue);
                                    break;

                                //物理防御
                                case "17":
                                    def_JingLingMax = def_JingLingMax + int.Parse(gemValue);
                                    break;

                                //魔法防御
                                case "20":
                                    adf_JingLingMax = adf_JingLingMax + int.Parse(gemValue);
                                    break;

                                //暴击
                                case "30":
                                    cir_JingLing = cir_JingLing + float.Parse(gemValue);
                                    break;

                                //命中
                                case "31":
                                    hit_JingLing = hit_JingLing + float.Parse(gemValue);
                                    break;

                                //闪避
                                case "32":
                                    dodge_JingLing = dodge_JingLing + float.Parse(gemValue);
                                    break;

                                //物理免伤
                                case "33":
                                    defAdd_JingLing = defAdd_JingLing + float.Parse(gemValue);
                                    break;

                                //魔法免伤
                                case "34":
                                    adfAdd_JingLing = adfAdd_JingLing + float.Parse(gemValue);
                                    break;

                                //速度
                                case "35":
                                    speed_JingLing = speed_JingLing + float.Parse(gemValue);
                                    break;

                                //伤害免伤
                                case "36":
                                    damgeSub_JingLing = damgeSub_JingLing + float.Parse(gemValue);
                                    break;

                                //37.伤害加成
                                case "37":
                                    damgeAdd_JingLing = damgeAdd_JingLing + float.Parse(gemValue);
                                    break;

                                //血量百分比
                                case "50":
                                    hpPro_JingLing = hpPro_JingLing + float.Parse(gemValue);
                                    break;

                                //物理攻击(百分比)
                                case "51":
                                    actPro_JingLing = actPro_JingLing + float.Parse(gemValue);
                                    break;

                                //魔法攻击(百分比)
                                case "52":
                                    magactPro_JingLing = magactPro_JingLing + float.Parse(gemValue);
                                    break;

                                //物理防御(百分比)
                                case "53":
                                    defPro_JingLing = defPro_JingLing + float.Parse(gemValue);
                                    break;

                                //魔法防御(百分比)
                                case "54":
                                    adfPro_JingLing = adfPro_JingLing + float.Parse(gemValue);
                                    break;

                                //幸运
                                case "100":
                                    lucky_JingLing = lucky_JingLing + int.Parse(gemValue);
                                    break;

                                //格挡值
                                case "101":
                                    geDangValue_JingLing = geDangValue_JingLing + int.Parse(gemValue);
                                    break;

                                //重击概率
                                case "111":
                                    zhongJiPro_JingLing = zhongJiPro_JingLing + float.Parse(gemValue);
                                    break;

                                //重击附加伤害值
                                case "112":
                                    zhongJiValue_JingLing = zhongJiValue_JingLing + int.Parse(gemValue);
                                    break;

                                //每次普通攻击附加的伤害值
                                case "121":
                                    guDingValue_JingLing = guDingValue_JingLing + int.Parse(gemValue);
                                    break;

                                //忽视目标防御值
                                case "131":
                                    huShiDefValue_JingLing = huShiDefValue_JingLing + int.Parse(gemValue);
                                    break;

                                //忽视目标魔防值
                                case "132":
                                    huShiAdfValue_JingLing = huShiAdfValue_JingLing + int.Parse(gemValue);
                                    break;

                                //忽视目标防御值
                                case "133":
                                    huShiDefValuePro_JingLing = huShiDefValuePro_JingLing + float.Parse(gemValue);
                                    break;

                                //忽视目标魔防值
                                case "134":
                                    huShiAdfValuePro_JingLing = huShiAdfValuePro_JingLing + float.Parse(gemValue);
                                    break;

                                //吸血概率
                                case "141":
                                    xiXuePro_JingLing = xiXuePro_JingLing + float.Parse(gemValue);
                                    break;

                                //法术反击
                                case "151":
                                    magicRebound_JingLing = magicRebound_JingLing + float.Parse(gemValue);
                                    break;

                                //攻击反击
                                case "152":
                                    actRebound_JingLing = actRebound_JingLing + float.Parse(gemValue);
                                    break;

                                //韧性概率
                                case "161":
                                    resilience_JingLing = resilience_JingLing + float.Parse(gemValue);
                                    break;

                                //回血百分比
                                case "171":
                                    rose_HealHpPro_JingLing = rose_HealHpPro_JingLing + float.Parse(gemValue);
                                    break;

                                //回血固定值
                                case "172":
                                    rose_HealHpValue_JingLing = rose_HealHpValue_JingLing + int.Parse(gemValue);
                                    break;

                                //战斗回血比例
                                case "173":
                                    rose_HealHpFightPro_JingLing = rose_HealHpFightPro_JingLing + float.Parse(gemValue);
                                    break;

                                //暴击等级
                                case "201":
                                    criRating_JingLing = criRating_JingLing + int.Parse(gemValue);
                                    break;

                                //韧性等级
                                case "202":
                                    resilienceRating_JingLing = resilienceRating_JingLing + int.Parse(gemValue);
                                    break;

                                //命中等级
                                case "203":
                                    hitRating_JingLing = hitRating_JingLing + int.Parse(gemValue);
                                    break;

                                //闪避等级
                                case "204":
                                    dodgeRating_JingLing = dodgeRating_JingLing + int.Parse(gemValue);
                                    break;

                                //光抗性
                                case "301":
                                    resistance_1_JingLing = resistance_1_JingLing + float.Parse(gemValue);
                                    break;

                                //暗抗性
                                case "302":
                                    resistance_2_JingLing = resistance_2_JingLing + float.Parse(gemValue);
                                    break;

                                //火抗性
                                case "303":
                                    resistance_3_JingLing = resistance_3_JingLing + float.Parse(gemValue);
                                    break;

                                //水抗性
                                case "304":
                                    resistance_4_JingLing = resistance_4_JingLing + float.Parse(gemValue);
                                    break;

                                //电抗性
                                case "305":
                                    resistance_5_JingLing = resistance_5_JingLing + float.Parse(gemValue);
                                    break;

                                //野兽攻击抗性
                                case "321":
                                    raceResistance_1_JingLing = raceResistance_1_JingLing + float.Parse(gemValue);
                                    break;

                                //人物攻击抗性
                                case "322":
                                    raceResistance_2_JingLing = raceResistance_2_JingLing + float.Parse(gemValue);
                                    break;

                                //恶魔攻击抗性
                                case "323":
                                    raceResistance_3_JingLing = raceResistance_3_JingLing + float.Parse(gemValue);
                                    break;

                                //野兽攻击抗性
                                case "331":
                                    raceDamge_1_JingLing = raceDamge_1_JingLing + float.Parse(gemValue);
                                    break;

                                //人物攻击抗性
                                case "332":
                                    raceDamge_2_JingLing = raceDamge_2_JingLing + float.Parse(gemValue);
                                    break;

                                //恶魔攻击抗性
                                case "333":
                                    raceDamge_3_JingLing = raceDamge_3_JingLing + float.Parse(gemValue);
                                    break;

                                //Boss普通攻击加成
                                case "341":
                                    rose_Boss_ActAdd_JingLing = rose_Boss_ActAdd_JingLing + float.Parse(gemValue);
                                    break;

                                //Boss技能攻击加成
                                case "342":
                                    rose_Boss_SkillAdd_JingLing = rose_Boss_SkillAdd_JingLing + float.Parse(gemValue);
                                    break;

                                //受到Boss普通攻击减免
                                case "343":
                                    rose_Boss_ActHitCost_JingLing = rose_Boss_ActHitCost_JingLing + float.Parse(gemValue);
                                    break;

                                //受到Boss技能攻击减免
                                case "344":
                                    rose_Boss_SkillHitCost_JingLing = rose_Boss_SkillHitCost_JingLing + float.Parse(gemValue);
                                    break;

                                //宠物攻击加成
                                case "345":
                                    rose_PetActAdd_JingLing = rose_PetActAdd_JingLing + float.Parse(gemValue);
                                    break;

                                //宠物受伤减免
                                case "346":
                                    rose_PetActHitCost_JingLing = rose_PetActHitCost_JingLing + float.Parse(gemValue);
                                    break;

                                //技能冷却时间缩减
                                case "347":
                                    rose_SkillCDTimePro_JingLing = rose_SkillCDTimePro_JingLing + float.Parse(gemValue);
                                    break;

                                //自身buff效果延长
                                case "348":
                                    rose_BuffTimeAddPro_JingLing = rose_BuffTimeAddPro_JingLing + float.Parse(gemValue);
                                    break;

                                //Debuff时间缩短
                                case "349":
                                    rose_DeBuffTimeCostPro_JingLing = rose_DeBuffTimeCostPro_JingLing + float.Parse(gemValue);
                                    break;

                                //闪避恢复血量
                                case "350":
                                    rose_DodgeAddHpPro_JingLing = rose_DodgeAddHpPro_JingLing + float.Parse(gemValue);
                                    break;

                                //魔法量附加
                                case "351":
                                    rose_LanValueMaxAdd_JingLing = rose_LanValueMaxAdd_JingLing + float.Parse(gemValue);
                                    break;

                                //召唤生物属性加成
                                case "352":
                                    rose_SummonAIPropertyAddPro_JingLing = rose_LanValueMaxAdd_JingLing + float.Parse(gemValue);
                                    break;

                                //护盾属性附加
                                case "353":
                                    rose_HuDunValueAddPro_JingLing = rose_LanValueMaxAdd_JingLing + float.Parse(gemValue);
                                    break;

                                //经验加成
                                case "401":
                                    exp_AddPro_JingLing = exp_AddPro_JingLing + float.Parse(gemValue);
                                    break;

                                //金币加成
                                case "402":
                                    gold_AddPro_JingLing = gold_AddPro_JingLing + float.Parse(gemValue);
                                    break;

                                //洗炼极品掉落（祝福值）
                                case "403":
                                    blessing_AddPro_JingLing = blessing_AddPro_JingLing + float.Parse(gemValue);
                                    break;

                                //装备隐藏属性出现概率
                                case "404":
                                    hidePro_AddPro_JingLing = hidePro_AddPro_JingLing + float.Parse(gemValue);
                                    break;

                                //装备上的宝石槽位出现概率
                                case "405":
                                    gemHole_AddPro_JingLing = gemHole_AddPro_JingLing + float.Parse(gemValue);
                                    break;

                                //经验加成固定
                                case "406":
                                    exp_AddValue_JingLing = exp_AddValue_JingLing + int.Parse(gemValue);
                                    break;

                                //金币加成固定
                                case "407":
                                    gold_AddValue_JingLing = gold_AddValue_JingLing + int.Parse(gemValue);
                                    break;
                                //药剂类熟练度
                                case "408":
                                    rose_YaoJiValue_JingLing = rose_YaoJiValue_JingLing + int.Parse(gemValue);
                                    break;
                                //锻造类熟练度
                                case "409":
                                    rose_DuanZaoValue_JingLing = rose_DuanZaoValue_JingLing + int.Parse(gemValue);
                                    break;
                                //复活
                                case "411":
                                    rose_FuHuoPro_JingLing = rose_FuHuoPro_JingLing + float.Parse(gemValue);
                                    break;
                                //攻击无视防御
                                case "412":
                                    rose_ActWuShi_JingLing = rose_ActWuShi_JingLing + float.Parse(gemValue);
                                    break;
                                //神农
                                case "413":
                                    rose_ShenNong_JingLing = rose_ShenNong_JingLing + float.Parse(gemValue);
                                    break;
                                //额外掉落
                                case "414":
                                    rose_DropExtra_JingLing = rose_DropExtra_JingLing + float.Parse(gemValue);
                                    break;
                                //伪装  +增大发现范围   -缩小范围
                                case "415":
                                    rose_WeiZhuang_JingLing = rose_WeiZhuang_JingLing + float.Parse(gemValue);
                                    break;
                                //灾难
                                case "416":
                                    rose_ZaiNanValue_JingLing = rose_ZaiNanValue_JingLing + float.Parse(gemValue);
                                    break;
                                //嗜血概率
                                case "417":
                                    rose_ShiXuePro_JingLing = rose_ShiXuePro_JingLing + float.Parse(gemValue);
                                    break;

                                //怪物脱战距离
                                case "418":
                                    rose_AITuoZhanDisValue_JingLing = rose_AITuoZhanDisValue_JingLing + float.Parse(gemValue);
                                    break;

                                //专注概率
                                case "419":
                                    rose_ZhuanZhuPro_JingLing = rose_ZhuanZhuPro_JingLing + float.Parse(gemValue);
                                    break;

                                //必中
                                case "420":
                                    rose_BiZhongPro_JingLing = float.Parse(gemValue);
                                    break;

                                //生产药剂暴击概率
                                case "421":
                                    rose_YaoJiCirPro_JingLing = float.Parse(gemValue);
                                    break;
                                //捕捉概率
                                case "422":
                                    rose_BuZhuoPro_JingLing = rose_BuZhuoPro_JingLing + float.Parse(gemValue);
                                    break;
                            }
                        }
                    }
                }
            }
        }


        //洗炼大师
        //--------------精灵加成-----------------
        int hp_XiLianDaShi = 0;
        int act_XiLianDaShiMin = 0;
        int act_XiLianDaShiMax = 0;
        int magact_XiLianDaShiMin = 0;
        int magact_XiLianDaShiMax = 0;
        int def_XiLianDaShiMin = 0;
        int def_XiLianDaShiMax = 0;
        int adf_XiLianDaShiMin = 0;
        int adf_XiLianDaShiMax = 0;
        float cir_XiLianDaShi = 0;
        float hit_XiLianDaShi = 0;
        float dodge_XiLianDaShi = 0;
        float damgeAdd_XiLianDaShi = 0;
        float damgeSub_XiLianDaShi = 0;
        float speed_XiLianDaShi = 0;
        int lucky_XiLianDaShi = 0;

        float defAdd_XiLianDaShi = 0;
        float adfAdd_XiLianDaShi = 0;
        float hpPro_XiLianDaShi = 0;
        float actPro_XiLianDaShi = 0;
        float magactPro_XiLianDaShi = 0;
        float defPro_XiLianDaShi = 0;
        float adfPro_XiLianDaShi = 0;

        int geDangValue_XiLianDaShi = 0;                    //格挡值
        float zhongJiPro_XiLianDaShi = 0.0f;                //重击概率
        int zhongJiValue_XiLianDaShi = 0;                   //重击附加伤害值
        int guDingValue_XiLianDaShi = 0;                    //每次普通攻击附加的伤害值
        int huShiDefValue_XiLianDaShi = 0;                  //忽视目标防御值                       
        int huShiAdfValue_XiLianDaShi = 0;                  //忽视目标魔防值
        float huShiDefValuePro_XiLianDaShi = 0;             //忽视目标防御值                       
        float huShiAdfValuePro_XiLianDaShi = 0;             //忽视目标魔防值
        float xiXuePro_XiLianDaShi = 0.0f;                  //吸血概率

        int criRating_XiLianDaShi = 0;                      //暴击等级
        int resilienceRating_XiLianDaShi = 0;               //韧性等级
        int hitRating_XiLianDaShi = 0;                      //命中等级
        int dodgeRating_XiLianDaShi = 0;                    //闪避等级
        float resilience_XiLianDaShi = 0.0f;                //韧性概率
        float magicRebound_XiLianDaShi = 0.0f;              //法术反击值
        float actRebound_XiLianDaShi = 0.0f;                //攻击反击
        float resistance_1_XiLianDaShi = 0;                 //光抗性
        float resistance_2_XiLianDaShi = 0;                 //暗抗性
        float resistance_3_XiLianDaShi = 0;                 //火抗性
        float resistance_4_XiLianDaShi = 0;                 //水抗性
        float resistance_5_XiLianDaShi = 0;                 //电抗性
        float raceResistance_1_XiLianDaShi = 0;             //野兽攻击抗性
        float raceResistance_2_XiLianDaShi = 0;             //人物攻击抗性
        float raceResistance_3_XiLianDaShi = 0;             //恶魔攻击抗性
        float raceDamge_1_XiLianDaShi = 0;                  //野兽攻击抗性
        float raceDamge_2_XiLianDaShi = 0;                  //人物攻击抗性
        float raceDamge_3_XiLianDaShi = 0;                  //恶魔攻击抗性
        float rose_Boss_ActAdd_XiLianDaShi = 0;                      //Boss普通攻击加成
        float rose_Boss_SkillAdd_XiLianDaShi = 0;                    //Boss技能攻击加成
        float rose_Boss_ActHitCost_XiLianDaShi = 0;                  //受到Boss普通攻击减免
        float rose_Boss_SkillHitCost_XiLianDaShi = 0;                //受到Boss技能攻击减免
        float rose_PetActAdd_XiLianDaShi = 0;                        //宠物攻击加成
        float rose_PetActHitCost_XiLianDaShi = 0;                    //宠物受伤减免
        float rose_SkillCDTimePro_XiLianDaShi = 0;                      //技能冷却时间缩减
        float rose_BuffTimeAddPro_XiLianDaShi = 0;                      //自身buff效果延长
        float rose_DeBuffTimeCostPro_XiLianDaShi = 0;                   //Debuff时间缩短
        float rose_DodgeAddHpPro_XiLianDaShi = 0;                       //闪避恢复血量

        float exp_AddPro_XiLianDaShi = 0.0f;               //经验加成
        float gold_AddPro_XiLianDaShi = 0.0f;              //金币加成
        int exp_AddValue_XiLianDaShi = 0;             //经验加成
        int gold_AddValue_XiLianDaShi = 0;            //金币加成
        float blessing_AddPro_XiLianDaShi = 0.0f;          //洗炼极品掉落
        float hidePro_AddPro_XiLianDaShi = 0.0f;           //隐藏属性出现概率
        float gemHole_AddPro_XiLianDaShi = 0.0f;           //装备上的宝石槽位出现概率

        int rose_YaoJiValue_XiLianDaShi = 0;                     //药剂类熟练度
        int rose_DuanZaoValue_XiLianDaShi = 0;                   //锻造类熟练度 
        float rose_FuHuoPro_XiLianDaShi = 0.0f;                  //复活
        float rose_ActWuShi_XiLianDaShi = 0.0f;                  //攻击无视防御
        float rose_ShenNong_XiLianDaShi = 0.0f;                  //神农
        float rose_DropExtra_XiLianDaShi = 0.0f;                 //额外掉落
        float rose_WeiZhuang_XiLianDaShi = 0.0f;                 //伪装  +增大发现范围   -缩小范围
        float rose_ZaiNanValue_XiLianDaShi = 0.0f;                //灾难
        float rose_ShiXuePro_XiLianDaShi = 0.0f;                 //嗜血概率

        int rose_HealHpValue_XiLianDaShi = 0;                   //角色恢复的固定值
        float rose_HealHpPro_XiLianDaShi = 0.0f;                //角色恢复的百分比加成固定值
        float rose_HealHpFightPro_XiLianDaShi = 0.0f;           //角色战斗时恢复额外恢复血量的百分比
        float rose_AITuoZhanDisValue_XiLianDaShi = 0.0f;     //怪物脱战距离
        float rose_ZhuanZhuPro_XiLianDaShi = 0;                  //专注概率
        float rose_BiZhongPro_XiLianDaShi = 0.0f;                //怪物脱战距离
        float rose_YaoJiCirPro_XiLianDaShi = 0;                  //生产药剂暴击概率
        float rose_BuZhuoPro_XiLianDaShi = 0;                 //捕捉概率

        float rose_LanValueMaxAdd_XiLianDaShi = 0;                       //魔法量附加
        float rose_SummonAIPropertyAddPro_XiLianDaShi = 0;                //召唤生物属性加成
        float rose_HuDunValueAddPro_XiLianDaShi = 0;                     //护盾属性附加
        float rose_ActAddPro_XiLianDaShi = 0;                           //普通攻击加成

        //精灵属性
        string XiLianDaShiIDSet = functionDataSet.DataSet_ReadData("XiLianDaShiIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //根据当前洗炼值返回洗炼家的ID
        string XiLianAddIDStr = RetuenXiLianJiaLvSet();
        if (XiLianAddIDStr != "") {
            if (XiLianDaShiIDSet == "")
            {
                XiLianDaShiIDSet = XiLianDaShiIDSet + XiLianAddIDStr;
            }
            else {
                XiLianDaShiIDSet = XiLianDaShiIDSet  + ";" + XiLianAddIDStr;
            }
        }

        //gemID = "101,101";
        if (XiLianDaShiIDSet != "" && XiLianDaShiIDSet != "0")
        {
            string[] XiLianDaShiStrList = XiLianDaShiIDSet.Split(';');
            for (int z = 0; z < XiLianDaShiStrList.Length; z++)
            {
                string[] XiLianDaShiStrSonList = XiLianDaShiStrList[z].Split(',');
                for (int y = 0; y < XiLianDaShiStrSonList.Length; y++) {

                    //获取当前宝石的属性
                    string XiLianDaShiIDStr = functionDataSet.DataSet_ReadData("XiLianPerproty", "ID", XiLianDaShiStrSonList[y], "EquipXiLianDaShi_Template");

                    string[] XiLianDaShiIDStrList = XiLianDaShiIDStr.Split(';');
                    for (int gemStrNum = 0; gemStrNum < XiLianDaShiIDStrList.Length; gemStrNum++)
                    {

                        if (XiLianDaShiIDStrList[gemStrNum] != "" && XiLianDaShiIDStrList[gemStrNum] != "0"&& XiLianDaShiIDStrList[gemStrNum].Split(',').Length>=2)
                        {
                            string gemType = XiLianDaShiIDStrList[gemStrNum].Split(',')[0];
                            string gemValue = XiLianDaShiIDStrList[gemStrNum].Split(',')[1];

                            if (gemValue == "" || gemValue == null)
                            {
                                gemValue = "0";
                            }

                            switch (gemType)
                            {

                                //血量
                                case "10":
                                    hp_XiLianDaShi = hp_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //物理最小攻击
                                case "11":
                                    //Debug.Log("gemValue = " + gemValue);
                                    act_XiLianDaShiMax = act_XiLianDaShiMax + int.Parse(gemValue);
                                    break;

                                //魔法攻击
                                case "14":
                                    magact_XiLianDaShiMax = magact_XiLianDaShiMax + int.Parse(gemValue);
                                    break;

                                //物理防御
                                case "17":
                                    def_XiLianDaShiMax = def_XiLianDaShiMax + int.Parse(gemValue);
                                    break;

                                //魔法防御
                                case "20":
                                    adf_XiLianDaShiMax = adf_XiLianDaShiMax + int.Parse(gemValue);
                                    break;

                                //暴击
                                case "30":
                                    cir_XiLianDaShi = cir_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //命中
                                case "31":
                                    hit_XiLianDaShi = hit_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //闪避
                                case "32":
                                    dodge_XiLianDaShi = dodge_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //物理免伤
                                case "33":
                                    defAdd_XiLianDaShi = defAdd_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //魔法免伤
                                case "34":
                                    adfAdd_XiLianDaShi = adfAdd_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //速度
                                case "35":
                                    speed_XiLianDaShi = speed_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //伤害免伤
                                case "36":
                                    damgeSub_XiLianDaShi = damgeSub_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //伤害免伤
                                case "37":
                                    damgeAdd_XiLianDaShi = damgeAdd_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //血量百分比
                                case "50":
                                    hpPro_XiLianDaShi = hpPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //物理攻击(百分比)
                                case "51":
                                    actPro_XiLianDaShi = actPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //魔法攻击(百分比)
                                case "52":
                                    magactPro_XiLianDaShi = magactPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //物理防御(百分比)
                                case "53":
                                    defPro_XiLianDaShi = defPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //魔法防御(百分比)
                                case "54":
                                    adfPro_XiLianDaShi = adfPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //幸运
                                case "100":
                                    lucky_XiLianDaShi = lucky_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //格挡值
                                case "101":
                                    geDangValue_XiLianDaShi = geDangValue_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //重击概率
                                case "111":
                                    zhongJiPro_XiLianDaShi = zhongJiPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //重击附加伤害值
                                case "112":
                                    zhongJiValue_XiLianDaShi = zhongJiValue_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //每次普通攻击附加的伤害值
                                case "121":
                                    guDingValue_XiLianDaShi = guDingValue_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //忽视目标防御值
                                case "131":
                                    huShiDefValue_XiLianDaShi = huShiDefValue_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //忽视目标魔防值
                                case "132":
                                    huShiAdfValue_XiLianDaShi = huShiAdfValue_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //忽视目标防御值
                                case "133":
                                    huShiDefValuePro_XiLianDaShi = huShiDefValuePro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //忽视目标魔防值
                                case "134":
                                    huShiAdfValuePro_XiLianDaShi = huShiAdfValuePro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //吸血概率
                                case "141":
                                    xiXuePro_XiLianDaShi = xiXuePro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //法术反击
                                case "151":
                                    magicRebound_XiLianDaShi = magicRebound_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //攻击反击
                                case "152":
                                    actRebound_XiLianDaShi = actRebound_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //韧性概率
                                case "161":
                                    resilience_XiLianDaShi = resilience_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //回血百分比
                                case "171":
                                    rose_HealHpPro_XiLianDaShi = rose_HealHpPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //回血固定值
                                case "172":
                                    rose_HealHpValue_XiLianDaShi = rose_HealHpValue_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //战斗回血比例
                                case "173":
                                    rose_HealHpFightPro_XiLianDaShi = rose_HealHpFightPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //暴击等级
                                case "201":
                                    criRating_XiLianDaShi = criRating_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //韧性等级
                                case "202":
                                    resilienceRating_XiLianDaShi = resilienceRating_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //命中等级
                                case "203":
                                    hitRating_XiLianDaShi = hitRating_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //闪避等级
                                case "204":
                                    dodgeRating_XiLianDaShi = dodgeRating_XiLianDaShi + int.Parse(gemValue);
                                    break;

                                //光抗性
                                case "301":
                                    resistance_1_XiLianDaShi = resistance_1_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //暗抗性
                                case "302":
                                    resistance_2_XiLianDaShi = resistance_2_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //火抗性
                                case "303":
                                    resistance_3_XiLianDaShi = resistance_3_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //水抗性
                                case "304":
                                    resistance_4_XiLianDaShi = resistance_4_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //电抗性
                                case "305":
                                    resistance_5_XiLianDaShi = resistance_5_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //野兽攻击抗性
                                case "321":
                                    raceResistance_1_XiLianDaShi = raceResistance_1_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //人物攻击抗性
                                case "322":
                                    raceResistance_2_XiLianDaShi = raceResistance_2_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //恶魔攻击抗性
                                case "323":
                                    raceResistance_3_XiLianDaShi = raceResistance_3_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //野兽攻击抗性
                                case "331":
                                    raceDamge_1_XiLianDaShi = raceDamge_1_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //人物攻击抗性
                                case "332":
                                    raceDamge_2_XiLianDaShi = raceDamge_2_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //恶魔攻击抗性
                                case "333":
                                    raceDamge_3_XiLianDaShi = raceDamge_3_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //Boss普通攻击加成
                                case "341":
                                    rose_Boss_ActAdd_XiLianDaShi = rose_Boss_ActAdd_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //Boss技能攻击加成
                                case "342":
                                    rose_Boss_SkillAdd_XiLianDaShi = rose_Boss_SkillAdd_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //受到Boss普通攻击减免
                                case "343":
                                    rose_Boss_ActHitCost_XiLianDaShi = rose_Boss_ActHitCost_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //受到Boss技能攻击减免
                                case "344":
                                    rose_Boss_SkillHitCost_XiLianDaShi = rose_Boss_SkillHitCost_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //受到Boss普通攻击减免
                                case "345":
                                    rose_PetActAdd_XiLianDaShi = rose_PetActAdd_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //受到Boss技能攻击减免
                                case "346":
                                    rose_PetActHitCost_XiLianDaShi = rose_PetActHitCost_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //技能冷却时间缩减
                                case "347":
                                    rose_SkillCDTimePro_XiLianDaShi = rose_SkillCDTimePro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //自身buff效果延长
                                case "348":
                                    rose_BuffTimeAddPro_XiLianDaShi = rose_BuffTimeAddPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //Debuff时间缩短
                                case "349":
                                    rose_DeBuffTimeCostPro_XiLianDaShi = rose_DeBuffTimeCostPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //闪避恢复血量
                                case "350":
                                    rose_DodgeAddHpPro_XiLianDaShi = rose_DodgeAddHpPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //魔法量附加
                                case "351":
                                    rose_LanValueMaxAdd_XiLianDaShi = rose_LanValueMaxAdd_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //召唤生物属性加成
                                case "352":
                                    rose_SummonAIPropertyAddPro_XiLianDaShi = rose_LanValueMaxAdd_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //护盾属性附加
                                case "353":
                                    rose_HuDunValueAddPro_XiLianDaShi = rose_LanValueMaxAdd_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //经验加成
                                case "401":
                                    exp_AddPro_XiLianDaShi = exp_AddPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //金币加成
                                case "402":
                                    gold_AddPro_XiLianDaShi = gold_AddPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //洗炼极品掉落（祝福值）
                                case "403":
                                    blessing_AddPro_XiLianDaShi = blessing_AddPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //装备隐藏属性出现概率
                                case "404":
                                    hidePro_AddPro_XiLianDaShi = hidePro_AddPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //装备上的宝石槽位出现概率
                                case "405":
                                    gemHole_AddPro_XiLianDaShi = gemHole_AddPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //经验加成固定
                                case "406":
                                    exp_AddValue_XiLianDaShi = (int)(exp_AddValue_XiLianDaShi + float.Parse(gemValue));
                                    break;

                                //金币加成固定
                                case "407":
                                    gold_AddValue_XiLianDaShi = gold_AddValue_XiLianDaShi + int.Parse(gemValue);
                                    break;
                                //药剂类熟练度
                                case "408":
                                    rose_YaoJiValue_XiLianDaShi = rose_YaoJiValue_XiLianDaShi + int.Parse(gemValue);
                                    break;
                                //锻造类熟练度
                                case "409":
                                    rose_DuanZaoValue_XiLianDaShi = rose_DuanZaoValue_XiLianDaShi + int.Parse(gemValue);
                                    break;
                                //复活
                                case "411":
                                    rose_FuHuoPro_XiLianDaShi = rose_FuHuoPro_XiLianDaShi + float.Parse(gemValue);
                                    break;
                                //攻击无视防御
                                case "412":
                                    rose_ActWuShi_XiLianDaShi = rose_ActWuShi_XiLianDaShi + float.Parse(gemValue);
                                    break;
                                //神农
                                case "413":
                                    rose_ShenNong_XiLianDaShi = rose_ShenNong_XiLianDaShi + float.Parse(gemValue);
                                    break;
                                //额外掉落
                                case "414":
                                    rose_DropExtra_XiLianDaShi = rose_DropExtra_XiLianDaShi + float.Parse(gemValue);
                                    break;
                                //伪装  +增大发现范围   -缩小范围
                                case "415":
                                    rose_WeiZhuang_XiLianDaShi = rose_WeiZhuang_XiLianDaShi + float.Parse(gemValue);
                                    break;
                                //灾难
                                case "416":
                                    rose_ZaiNanValue_XiLianDaShi = rose_ZaiNanValue_XiLianDaShi + float.Parse(gemValue);
                                    break;
                                //嗜血概率
                                case "417":
                                    rose_ShiXuePro_XiLianDaShi = rose_ShiXuePro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //怪物脱战距离
                                case "418":
                                    rose_AITuoZhanDisValue_XiLianDaShi = rose_AITuoZhanDisValue_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //专注概率
                                case "419":
                                    rose_ZhuanZhuPro_XiLianDaShi = rose_ZhuanZhuPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                                //必中
                                case "420":
                                    rose_BiZhongPro_XiLianDaShi = float.Parse(gemValue);
                                    break;

                                //生产药剂暴击概率
                                case "421":
                                    rose_YaoJiCirPro_XiLianDaShi = float.Parse(gemValue);
                                    break;
                                //捕捉概率
                                case "422":
                                    rose_BuZhuoPro_XiLianDaShi = rose_BuZhuoPro_XiLianDaShi + float.Parse(gemValue);
                                    break;
                            }
                        }
                    }

                }

            }
        }



        //坐骑进阶属性
        //string XiLianDaShiIDSet = functionDataSet.DataSet_ReadData("XiLianDaShiIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string nowZuoQiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        //获取当前属性
        string XiLianDaShiStrSonListStr = "";
        if (nowZuoQiID != "" && nowZuoQiID != "0" && nowZuoQiID != null) {

            string ZuoQiIDStr = "10001;10002;10003;10004;10005;10006;10007;10008;10009;10010";         //设置坐骑ID，懒直接写死
            string[] zuoQiIDList = ZuoQiIDStr.Split(';');
            for (int i = 0; i < zuoQiIDList.Length;i++) {

                string nowAddProperty = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddProperty", "ID", zuoQiIDList[i], "ZuoQi_Template");
                if (nowAddProperty != "0" && nowAddProperty != "" && nowAddProperty != null) {
                    if (XiLianDaShiStrSonListStr == "")
                    {
                        XiLianDaShiStrSonListStr = nowAddProperty;
                    }
                    else
                    {
                        XiLianDaShiStrSonListStr = XiLianDaShiStrSonListStr + ";" + nowAddProperty;
                    }
                }

                if (nowZuoQiID == zuoQiIDList[i])
                {
                    i = zuoQiIDList.Length;
                    break;
                }

            }
        }



        //称谓属性
        //string XiLianDaShiIDSet = functionDataSet.DataSet_ReadData("XiLianDaShiIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string ChengHaoIDSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengHaoIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //获取当前属性
        string ChengHaoIDSetStrListStr = "";
        if (ChengHaoIDSetStr != "" && ChengHaoIDSetStr != "0" && ChengHaoIDSetStr != null)
        {

            //string ZuoQiIDStr = "10001;10002;10003;10004;10005;10006;10007;10008;10009;10010";         //设置坐骑ID，懒直接写死
            string[] ChengHaoIDList = ChengHaoIDSetStr.Split(';');
            for (int i = 0; i < ChengHaoIDList.Length; i++)
            {

                //if (int.Parse(nowZuoQiID)>=int.Parse) { }

                string nowAddProperty = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddProperty", "ID", ChengHaoIDList[i], "ChengHao_Template");
                if (nowAddProperty != "0" && nowAddProperty != "" && nowAddProperty != null)
                {
                    if (ChengHaoIDSetStrListStr == "")
                    {
                        ChengHaoIDSetStrListStr = nowAddProperty;
                    }
                    else
                    {
                        ChengHaoIDSetStrListStr = ChengHaoIDSetStrListStr + ";" + nowAddProperty;
                    }
                }

            }
        }

        if (ChengHaoIDSetStrListStr != "")
        {
            XiLianDaShiStrSonListStr = XiLianDaShiStrSonListStr + ";" + ChengHaoIDSetStrListStr;
        }

        //注灵属性
        string ZhuLingIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhuLingIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //获取当前属性
        string ZhuLingIDSetStrListStr = "";
        if (ZhuLingIDStr != "" && ZhuLingIDStr != "0" && ZhuLingIDStr != null)
        {
            string[] ZhuLingIDList = ZhuLingIDStr.Split(';');
            for (int i = 0; i < ZhuLingIDList.Length; i++)
            {
                string nowAddProperty = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddProperty", "ID", ZhuLingIDList[i], "ZhuLing_Template");
                if (nowAddProperty != "0" && nowAddProperty != "" && nowAddProperty != null)
                {
                    if (ZhuLingIDSetStrListStr == "")
                    {
                        ZhuLingIDSetStrListStr = nowAddProperty;
                    }
                    else
                    {
                        ZhuLingIDSetStrListStr = ZhuLingIDSetStrListStr + ";" + nowAddProperty;
                    }
                }
            }
        }

        if (ZhuLingIDSetStrListStr != "") {
            XiLianDaShiStrSonListStr = XiLianDaShiStrSonListStr + ";" + ZhuLingIDSetStrListStr;
        }


        //gemID = "101,101";
        if (XiLianDaShiStrSonListStr != "" && XiLianDaShiStrSonListStr != "0")
        {
            string[] XiLianDaShiStrSonList = XiLianDaShiStrSonListStr.Split(';');
            for (int y = 0; y < XiLianDaShiStrSonList.Length; y++)
            {
                if (XiLianDaShiStrSonList[y].Split(',').Length >= 2)
                {

                        string gemType = XiLianDaShiStrSonList[y].Split(',')[0];
                        string gemValue = XiLianDaShiStrSonList[y].Split(',')[1];

                        if (gemValue == "" || gemValue == null)
                        {
                            gemValue = "0";
                        }

                        switch (gemType)
                        {
                            //血量
                            case "10":
                                hp_XiLianDaShi = hp_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //物理最小攻击
                            case "11":
                                //Debug.Log("gemValue = " + gemValue);
                                act_XiLianDaShiMax = act_XiLianDaShiMax + int.Parse(gemValue);
                                break;

                            //魔法攻击
                            case "14":
                                magact_XiLianDaShiMax = magact_XiLianDaShiMax + int.Parse(gemValue);
                                break;

                            //物理防御
                            case "17":
                                def_XiLianDaShiMax = def_XiLianDaShiMax + int.Parse(gemValue);
                                break;

                            //魔法防御
                            case "20":
                                adf_XiLianDaShiMax = adf_XiLianDaShiMax + int.Parse(gemValue);
                                break;

                            //暴击
                            case "30":
                                cir_XiLianDaShi = cir_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //命中
                            case "31":
                                hit_XiLianDaShi = hit_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //闪避
                            case "32":
                                dodge_XiLianDaShi = dodge_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //物理免伤
                            case "33":
                                defAdd_XiLianDaShi = defAdd_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //魔法免伤
                            case "34":
                                adfAdd_XiLianDaShi = adfAdd_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //速度
                            case "35":
                                speed_XiLianDaShi = speed_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //伤害免伤
                            case "36":
                                damgeSub_XiLianDaShi = damgeSub_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //伤害免伤
                            case "37":
                                damgeAdd_XiLianDaShi = damgeAdd_XiLianDaShi + float.Parse(gemValue);
                            break;

                            //血量百分比
                            case "50":
                                hpPro_XiLianDaShi = hpPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //物理攻击(百分比)
                            case "51":
                                actPro_XiLianDaShi = actPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //魔法攻击(百分比)
                            case "52":
                                magactPro_XiLianDaShi = magactPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //物理防御(百分比)
                            case "53":
                                defPro_XiLianDaShi = defPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //魔法防御(百分比)
                            case "54":
                                adfPro_XiLianDaShi = adfPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //幸运
                            case "100":
                                lucky_XiLianDaShi = lucky_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //格挡值
                            case "101":
                                geDangValue_XiLianDaShi = geDangValue_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //重击概率
                            case "111":
                                zhongJiPro_XiLianDaShi = zhongJiPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //重击附加伤害值
                            case "112":
                                zhongJiValue_XiLianDaShi = zhongJiValue_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //每次普通攻击附加的伤害值
                            case "121":
                                guDingValue_XiLianDaShi = guDingValue_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //忽视目标防御值
                            case "131":
                                huShiDefValue_XiLianDaShi = huShiDefValue_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //忽视目标魔防值
                            case "132":
                                huShiAdfValue_XiLianDaShi = huShiAdfValue_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //忽视目标防御值
                            case "133":
                                huShiDefValuePro_XiLianDaShi = huShiDefValuePro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //忽视目标魔防值
                            case "134":
                                huShiAdfValuePro_XiLianDaShi = huShiAdfValuePro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //吸血概率
                            case "141":
                                xiXuePro_XiLianDaShi = xiXuePro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //法术反击
                            case "151":
                                magicRebound_XiLianDaShi = magicRebound_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //攻击反击
                            case "152":
                                actRebound_XiLianDaShi = actRebound_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //韧性概率
                            case "161":
                                resilience_XiLianDaShi = resilience_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //回血百分比
                            case "171":
                                rose_HealHpPro_XiLianDaShi = rose_HealHpPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //回血固定值
                            case "172":
                                rose_HealHpValue_XiLianDaShi = rose_HealHpValue_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //战斗回血比例
                            case "173":
                                rose_HealHpFightPro_XiLianDaShi = rose_HealHpFightPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //暴击等级
                            case "201":
                                criRating_XiLianDaShi = criRating_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //韧性等级
                            case "202":
                                resilienceRating_XiLianDaShi = resilienceRating_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //命中等级
                            case "203":
                                hitRating_XiLianDaShi = hitRating_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //闪避等级
                            case "204":
                                dodgeRating_XiLianDaShi = dodgeRating_XiLianDaShi + int.Parse(gemValue);
                                break;

                            //光抗性
                            case "301":
                                resistance_1_XiLianDaShi = resistance_1_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //暗抗性
                            case "302":
                                resistance_2_XiLianDaShi = resistance_2_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //火抗性
                            case "303":
                                resistance_3_XiLianDaShi = resistance_3_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //水抗性
                            case "304":
                                resistance_4_XiLianDaShi = resistance_4_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //电抗性
                            case "305":
                                resistance_5_XiLianDaShi = resistance_5_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //野兽攻击抗性
                            case "321":
                                raceResistance_1_XiLianDaShi = raceResistance_1_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //人物攻击抗性
                            case "322":
                                raceResistance_2_XiLianDaShi = raceResistance_2_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //恶魔攻击抗性
                            case "323":
                                raceResistance_3_XiLianDaShi = raceResistance_3_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //野兽攻击抗性
                            case "331":
                                raceDamge_1_XiLianDaShi = raceDamge_1_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //人物攻击抗性
                            case "332":
                                raceDamge_2_XiLianDaShi = raceDamge_2_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //恶魔攻击抗性
                            case "333":
                                raceDamge_3_XiLianDaShi = raceDamge_3_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //Boss普通攻击加成
                            case "341":
                                rose_Boss_ActAdd_XiLianDaShi = rose_Boss_ActAdd_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //Boss技能攻击加成
                            case "342":
                                rose_Boss_SkillAdd_XiLianDaShi = rose_Boss_SkillAdd_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //受到Boss普通攻击减免
                            case "343":
                                rose_Boss_ActHitCost_XiLianDaShi = rose_Boss_ActHitCost_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //受到Boss技能攻击减免
                            case "344":
                                rose_Boss_SkillHitCost_XiLianDaShi = rose_Boss_SkillHitCost_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //受到Boss普通攻击减免
                            case "345":
                                rose_PetActAdd_XiLianDaShi = rose_PetActAdd_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //受到Boss技能攻击减免
                            case "346":
                                rose_PetActHitCost_XiLianDaShi = rose_PetActHitCost_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //技能冷却时间缩减
                            case "347":
                                rose_SkillCDTimePro_XiLianDaShi = rose_SkillCDTimePro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //自身buff效果延长
                            case "348":
                                rose_BuffTimeAddPro_XiLianDaShi = rose_BuffTimeAddPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //Debuff时间缩短
                            case "349":
                                rose_DeBuffTimeCostPro_XiLianDaShi = rose_DeBuffTimeCostPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //闪避恢复血量
                            case "350":
                                rose_DodgeAddHpPro_XiLianDaShi = rose_DodgeAddHpPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //魔法量附加
                            case "351":
                                rose_LanValueMaxAdd_XiLianDaShi = rose_LanValueMaxAdd_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //召唤生物属性加成
                            case "352":
                                rose_SummonAIPropertyAddPro_XiLianDaShi = rose_LanValueMaxAdd_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //护盾属性附加
                            case "353":
                                rose_HuDunValueAddPro_XiLianDaShi = rose_LanValueMaxAdd_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //角色普通攻击加成
                            case "361":
                                rose_ActAddPro_XiLianDaShi = rose_ActAddPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //经验加成
                            case "401":
                                    exp_AddPro_XiLianDaShi = exp_AddPro_XiLianDaShi + float.Parse(gemValue);
                                    break;

                            //金币加成
                            case "402":
                                gold_AddPro_XiLianDaShi = gold_AddPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //洗炼极品掉落（祝福值）
                            case "403":
                                blessing_AddPro_XiLianDaShi = blessing_AddPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //装备隐藏属性出现概率
                            case "404":
                                hidePro_AddPro_XiLianDaShi = hidePro_AddPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //装备上的宝石槽位出现概率
                            case "405":
                                gemHole_AddPro_XiLianDaShi = gemHole_AddPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //经验加成固定
                            case "406":
                                exp_AddValue_XiLianDaShi = (int)(exp_AddValue_XiLianDaShi + float.Parse(gemValue));
                                break;

                            //金币加成固定
                            case "407":
                                gold_AddValue_XiLianDaShi = gold_AddValue_XiLianDaShi + int.Parse(gemValue);
                                break;
                            //药剂类熟练度
                            case "408":
                                rose_YaoJiValue_XiLianDaShi = rose_YaoJiValue_XiLianDaShi + int.Parse(gemValue);
                                break;
                            //锻造类熟练度
                            case "409":
                                rose_DuanZaoValue_XiLianDaShi = rose_DuanZaoValue_XiLianDaShi + int.Parse(gemValue);
                                break;
                            //复活
                            case "411":
                                rose_FuHuoPro_XiLianDaShi = rose_FuHuoPro_XiLianDaShi + float.Parse(gemValue);
                                break;
                            //攻击无视防御
                            case "412":
                                rose_ActWuShi_XiLianDaShi = rose_ActWuShi_XiLianDaShi + float.Parse(gemValue);
                                break;
                            //神农
                            case "413":
                                rose_ShenNong_XiLianDaShi = rose_ShenNong_XiLianDaShi + float.Parse(gemValue);
                                break;
                            //额外掉落
                            case "414":
                                rose_DropExtra_XiLianDaShi = rose_DropExtra_XiLianDaShi + float.Parse(gemValue);
                                break;
                            //伪装  +增大发现范围   -缩小范围
                            case "415":
                                rose_WeiZhuang_XiLianDaShi = rose_WeiZhuang_XiLianDaShi + float.Parse(gemValue);
                                break;
                            //灾难
                            case "416":
                                rose_ZaiNanValue_XiLianDaShi = rose_ZaiNanValue_XiLianDaShi + float.Parse(gemValue);
                                break;
                            //嗜血概率
                            case "417":
                                rose_ShiXuePro_XiLianDaShi = rose_ShiXuePro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //怪物脱战距离
                            case "418":
                                rose_AITuoZhanDisValue_XiLianDaShi = rose_AITuoZhanDisValue_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //专注概率
                            case "419":
                                rose_ZhuanZhuPro_XiLianDaShi = rose_ZhuanZhuPro_XiLianDaShi + float.Parse(gemValue);
                                break;

                            //必中
                            case "420":
                                rose_BiZhongPro_XiLianDaShi = float.Parse(gemValue);
                                break;

                            //生产药剂暴击概率
                            case "421":
                                rose_YaoJiCirPro_XiLianDaShi = float.Parse(gemValue);
                                break;
                            //捕捉概率
                            case "422":
                                rose_BuZhuoPro_XiLianDaShi = rose_BuZhuoPro_XiLianDaShi + float.Parse(gemValue);
                                break;
                        }
                    
                }
            }
        }



        //获取坐骑能力属性
        string ZuoQiNengLiProStr = "";
        if (nowZuoQiID != "" && nowZuoQiID != "0" && nowZuoQiID != null)
        {
            //string ZuoQiIDStr = "10001;10002;10003;10004;10005;10006;10007;10008;10009;100010";         //设置坐骑ID，懒直接写死
            //string[] zuoQiIDList = ZuoQiIDStr.Split(';');
            for (int i = 1; i <= 4; i++)
            {
                string nowNengLiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                if (nowNengLiID != "" && nowNengLiID != "0" && nowNengLiID != null) {
                    string nowAddProperty = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddProperty", "ID", nowNengLiID, "ZuoQiNengLi_Template");

                    //根据资质计算值
                    string nowZuoQiZiZhi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiZiZhi_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                    if (nowZuoQiZiZhi != "" || nowZuoQiZiZhi != null) {

                        float ziZhiInt = float.Parse(nowZuoQiZiZhi);
                        //读取饱食度
                        float addPro = 1.0f;
                        /*
                        string baoshiduStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoShiDu", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                        if (baoshiduStr == "" || baoshiduStr == null)
                        {
                            baoshiduStr = "0";
                        }

                        float addProFloat = float.Parse(baoshiduStr);
                        if (addProFloat <= 60) {
                            addPro = addProFloat / 60;
                            if (addPro <= 0)
                            {
                                addPro = 0;
                            }
                        }
                        */
                        //坐骑资质计算
                        float ziZhiPro = (0.4f + (ziZhiInt / 2000) * 0.8f) * addPro;
                        string[] nowAddPropertyList = nowAddProperty.Split(';');
                        for (int y = 0; y < nowAddPropertyList.Length; y++) {
                            string[] addProList = nowAddPropertyList[y].Split(',');
                            if (addProList.Length >= 2) {
                                string proType = addProList[0];
                                string proValue = addProList[1];
                                proValue = ((int)(float.Parse(addProList[1]) * ziZhiPro)).ToString();
                                if (nowAddProperty == "" || nowAddProperty == "0")
                                {
                                    nowAddProperty = proType + "," + proValue;
                                }
                                else {
                                    nowAddProperty = nowAddProperty + ";" + proType + "," + proValue;
                                }
                            }
                        }
                    }

                    if (ZuoQiNengLiProStr == "")
                    {
                        ZuoQiNengLiProStr = nowAddProperty;
                    }
                    else
                    {
                        ZuoQiNengLiProStr = ZuoQiNengLiProStr + ";" + nowAddProperty;
                    }

                    /*
                    if (nowZuoQiID == zuoQiIDList[i])
                    {
                        i = zuoQiIDList.Length;
                        break;
                    }
                    */
                }
            }
        }

        //ZuoQiNengLiProStr  增加坐骑皮肤属性
        string nowZuoQiPiFuSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiPiFuSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] nowZuoQiPiFuList = nowZuoQiPiFuSet.Split(';');

        for (int i = 0; i < nowZuoQiPiFuList.Length; i++) {
            if (nowZuoQiPiFuList[i] != "" && nowZuoQiPiFuList[i] != "0" && nowZuoQiPiFuList[i] != null) {
                string nowAddProperty = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddProperty", "ID", nowZuoQiPiFuList[i], "ZuoQiShow_Template");
                //Debug.Log("坐骑属性:" + nowAddProperty);
                if (ZuoQiNengLiProStr == "")
                {
                    ZuoQiNengLiProStr = nowAddProperty;
                }
                else
                {
                    ZuoQiNengLiProStr = ZuoQiNengLiProStr + ";" + nowAddProperty;
                }

            }
        }

        //ZuoQiNengLiProStr  增加觉醒
        string nowJueXingJiHuoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] nowJueXingJiHuoIDList = nowJueXingJiHuoID.Split(';');
        for (int i = 0; i < nowJueXingJiHuoIDList.Length; i++)
        {
            if (nowJueXingJiHuoIDList[i] != "" && nowJueXingJiHuoIDList[i] != "0" && nowJueXingJiHuoIDList[i] != null)
            {
                string nowAddProperty = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProList", "ID", nowJueXingJiHuoIDList[i], "JueXing_Template");
                if (ZuoQiNengLiProStr == "")
                {
                    ZuoQiNengLiProStr = nowAddProperty;
                }
                else
                {
                    ZuoQiNengLiProStr = ZuoQiNengLiProStr + ";" + nowAddProperty;
                }
            }
        }


        //gemID = "101,101";
        if (ZuoQiNengLiProStr != "" && ZuoQiNengLiProStr != "0")
        {
            string[] XiLianDaShiStrSonList = ZuoQiNengLiProStr.Split(';');
            for (int y = 0; y < XiLianDaShiStrSonList.Length; y++)
            {
                if (XiLianDaShiStrSonList[y].Split(',').Length >= 2) {

                    string gemType = XiLianDaShiStrSonList[y].Split(',')[0];
                    string gemValue = XiLianDaShiStrSonList[y].Split(',')[1];

                    if (gemValue == "" || gemValue == null)
                    {
                        gemValue = "0";
                    }

                    switch (gemType)
                    {

                        //血量
                        case "10":
                            hp_XiLianDaShi = hp_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //物理最小攻击
                        case "11":
                            //Debug.Log("gemValue = " + gemValue);
                            act_XiLianDaShiMax = act_XiLianDaShiMax + int.Parse(gemValue);
                            break;

                        //魔法攻击
                        case "14":
                            magact_XiLianDaShiMax = magact_XiLianDaShiMax + int.Parse(gemValue);
                            break;

                        //物理防御
                        case "17":
                            def_XiLianDaShiMax = def_XiLianDaShiMax + int.Parse(gemValue);
                            break;

                        //魔法防御
                        case "20":
                            adf_XiLianDaShiMax = adf_XiLianDaShiMax + int.Parse(gemValue);
                            break;

                        //暴击
                        case "30":
                            cir_XiLianDaShi = cir_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //命中
                        case "31":
                            hit_XiLianDaShi = hit_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //闪避
                        case "32":
                            dodge_XiLianDaShi = dodge_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //物理免伤
                        case "33":
                            defAdd_XiLianDaShi = defAdd_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //魔法免伤
                        case "34":
                            adfAdd_XiLianDaShi = adfAdd_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //速度
                        case "35":
                            speed_XiLianDaShi = speed_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //伤害免伤
                        case "36":
                            damgeSub_XiLianDaShi = damgeSub_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //伤害加成
                        case "37":
                            damgeAdd_XiLianDaShi = damgeAdd_XiLianDaShi + float.Parse(gemValue);
                            //Debug.Log("伤害加成:" + damgeAdd_XiLianDaShi);
                            break;

                        //血量百分比
                        case "50":
                            hpPro_XiLianDaShi = hpPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //物理攻击(百分比)
                        case "51":
                            actPro_XiLianDaShi = actPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //魔法攻击(百分比)
                        case "52":
                            magactPro_XiLianDaShi = magactPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //物理防御(百分比)
                        case "53":
                            defPro_XiLianDaShi = defPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //魔法防御(百分比)
                        case "54":
                            adfPro_XiLianDaShi = adfPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //幸运
                        case "100":
                            lucky_XiLianDaShi = lucky_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //格挡值
                        case "101":
                            geDangValue_XiLianDaShi = geDangValue_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //重击概率
                        case "111":
                            zhongJiPro_XiLianDaShi = zhongJiPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //重击附加伤害值
                        case "112":
                            zhongJiValue_XiLianDaShi = zhongJiValue_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //每次普通攻击附加的伤害值
                        case "121":
                            guDingValue_XiLianDaShi = guDingValue_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //忽视目标防御值
                        case "131":
                            huShiDefValue_XiLianDaShi = huShiDefValue_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //忽视目标魔防值
                        case "132":
                            huShiAdfValue_XiLianDaShi = huShiAdfValue_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //忽视目标防御值
                        case "133":
                            huShiDefValuePro_XiLianDaShi = huShiDefValuePro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //忽视目标魔防值
                        case "134":
                            huShiAdfValuePro_XiLianDaShi = huShiAdfValuePro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //吸血概率
                        case "141":
                            xiXuePro_XiLianDaShi = xiXuePro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //法术反击
                        case "151":
                            magicRebound_XiLianDaShi = magicRebound_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //攻击反击
                        case "152":
                            actRebound_XiLianDaShi = actRebound_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //韧性概率
                        case "161":
                            resilience_XiLianDaShi = resilience_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //回血百分比
                        case "171":
                            rose_HealHpPro_XiLianDaShi = rose_HealHpPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //回血固定值
                        case "172":
                            rose_HealHpValue_XiLianDaShi = rose_HealHpValue_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //战斗回血比例
                        case "173":
                            rose_HealHpFightPro_XiLianDaShi = rose_HealHpFightPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //暴击等级
                        case "201":
                            criRating_XiLianDaShi = criRating_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //韧性等级
                        case "202":
                            resilienceRating_XiLianDaShi = resilienceRating_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //命中等级
                        case "203":
                            hitRating_XiLianDaShi = hitRating_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //闪避等级
                        case "204":
                            dodgeRating_XiLianDaShi = dodgeRating_XiLianDaShi + int.Parse(gemValue);
                            break;

                        //光抗性
                        case "301":
                            resistance_1_XiLianDaShi = resistance_1_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //暗抗性
                        case "302":
                            resistance_2_XiLianDaShi = resistance_2_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //火抗性
                        case "303":
                            resistance_3_XiLianDaShi = resistance_3_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //水抗性
                        case "304":
                            resistance_4_XiLianDaShi = resistance_4_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //电抗性
                        case "305":
                            resistance_5_XiLianDaShi = resistance_5_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //野兽攻击抗性
                        case "321":
                            raceResistance_1_XiLianDaShi = raceResistance_1_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //人物攻击抗性
                        case "322":
                            raceResistance_2_XiLianDaShi = raceResistance_2_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //恶魔攻击抗性
                        case "323":
                            raceResistance_3_XiLianDaShi = raceResistance_3_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //野兽攻击抗性
                        case "331":
                            raceDamge_1_XiLianDaShi = raceDamge_1_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //人物攻击抗性
                        case "332":
                            raceDamge_2_XiLianDaShi = raceDamge_2_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //恶魔攻击抗性
                        case "333":
                            raceDamge_3_XiLianDaShi = raceDamge_3_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //Boss普通攻击加成
                        case "341":
                            rose_Boss_ActAdd_XiLianDaShi = rose_Boss_ActAdd_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //Boss技能攻击加成
                        case "342":
                            rose_Boss_SkillAdd_XiLianDaShi = rose_Boss_SkillAdd_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //受到Boss普通攻击减免
                        case "343":
                            rose_Boss_ActHitCost_XiLianDaShi = rose_Boss_ActHitCost_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //受到Boss技能攻击减免
                        case "344":
                            rose_Boss_SkillHitCost_XiLianDaShi = rose_Boss_SkillHitCost_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //受到Boss普通攻击减免
                        case "345":
                            rose_PetActAdd_XiLianDaShi = rose_PetActAdd_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //受到Boss技能攻击减免
                        case "346":
                            rose_PetActHitCost_XiLianDaShi = rose_PetActHitCost_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //技能冷却时间缩减
                        case "347":
                            rose_SkillCDTimePro_XiLianDaShi = rose_SkillCDTimePro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //自身buff效果延长
                        case "348":
                            rose_BuffTimeAddPro_XiLianDaShi = rose_BuffTimeAddPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //Debuff时间缩短
                        case "349":
                            rose_DeBuffTimeCostPro_XiLianDaShi = rose_DeBuffTimeCostPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //闪避恢复血量
                        case "350":
                            rose_DodgeAddHpPro_XiLianDaShi = rose_DodgeAddHpPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //魔法量附加
                        case "351":
                            rose_LanValueMaxAdd_XiLianDaShi = rose_LanValueMaxAdd_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //召唤生物属性加成
                        case "352":
                            rose_SummonAIPropertyAddPro_XiLianDaShi = rose_LanValueMaxAdd_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //护盾属性附加
                        case "353":
                            rose_HuDunValueAddPro_XiLianDaShi = rose_LanValueMaxAdd_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //经验加成
                        case "401":
                            exp_AddPro_XiLianDaShi = exp_AddPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //金币加成
                        case "402":
                            gold_AddPro_XiLianDaShi = gold_AddPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //洗炼极品掉落（祝福值）
                        case "403":
                            blessing_AddPro_XiLianDaShi = blessing_AddPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //装备隐藏属性出现概率
                        case "404":
                            hidePro_AddPro_XiLianDaShi = hidePro_AddPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //装备上的宝石槽位出现概率
                        case "405":
                            gemHole_AddPro_XiLianDaShi = gemHole_AddPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //经验加成固定
                        case "406":
                            exp_AddValue_XiLianDaShi = (int)(exp_AddValue_XiLianDaShi + float.Parse(gemValue));
                            break;

                        //金币加成固定
                        case "407":
                            gold_AddValue_XiLianDaShi = gold_AddValue_XiLianDaShi + int.Parse(gemValue);
                            break;
                        //药剂类熟练度
                        case "408":
                            rose_YaoJiValue_XiLianDaShi = rose_YaoJiValue_XiLianDaShi + int.Parse(gemValue);
                            break;
                        //锻造类熟练度
                        case "409":
                            rose_DuanZaoValue_XiLianDaShi = rose_DuanZaoValue_XiLianDaShi + int.Parse(gemValue);
                            break;
                        //复活
                        case "411":
                            rose_FuHuoPro_XiLianDaShi = rose_FuHuoPro_XiLianDaShi + float.Parse(gemValue);
                            break;
                        //攻击无视防御
                        case "412":
                            rose_ActWuShi_XiLianDaShi = rose_ActWuShi_XiLianDaShi + float.Parse(gemValue);
                            break;
                        //神农
                        case "413":
                            rose_ShenNong_XiLianDaShi = rose_ShenNong_XiLianDaShi + float.Parse(gemValue);
                            break;
                        //额外掉落
                        case "414":
                            rose_DropExtra_XiLianDaShi = rose_DropExtra_XiLianDaShi + float.Parse(gemValue);
                            break;
                        //伪装  +增大发现范围   -缩小范围
                        case "415":
                            rose_WeiZhuang_XiLianDaShi = rose_WeiZhuang_XiLianDaShi + float.Parse(gemValue);
                            break;
                        //灾难
                        case "416":
                            rose_ZaiNanValue_XiLianDaShi = rose_ZaiNanValue_XiLianDaShi + float.Parse(gemValue);
                            break;
                        //嗜血概率
                        case "417":
                            rose_ShiXuePro_XiLianDaShi = rose_ShiXuePro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //怪物脱战距离
                        case "418":
                            rose_AITuoZhanDisValue_XiLianDaShi = rose_AITuoZhanDisValue_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //专注概率
                        case "419":
                            rose_ZhuanZhuPro_XiLianDaShi = rose_ZhuanZhuPro_XiLianDaShi + float.Parse(gemValue);
                            break;

                        //必中
                        case "420":
                            rose_BiZhongPro_XiLianDaShi = float.Parse(gemValue);
                            break;

                        //生产药剂暴击概率
                        case "421":
                            rose_YaoJiCirPro_XiLianDaShi = float.Parse(gemValue);
                            break;
                        //捕捉概率
                        case "422":
                            rose_BuZhuoPro_XiLianDaShi = rose_BuZhuoPro_XiLianDaShi + float.Parse(gemValue);
                            break;
                    }

                }

            }

        }



        //最终统计数据
        Rose_Proprety roseProprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();

		//血量上限
		int hp_Base = int.Parse(functionDataSet.DataSet_ReadData("BaseHp", "ID", rose_Occupation, "Occupation_Template"));
        float hp_LvUp = float.Parse(functionDataSet.DataSet_ReadData("LvUpHp", "ID", rose_Occupation, "Occupation_Template"));
		//hp_Equip = 0; //装备属性，以后添加
		//血量总和计算
		rose_Hp = (int)((hp_Base + hp_LvUp * rose_Lv + hp_Equip + hp_EquipSuit + hp_TianFu + hp_JingLing + hp_XiLianDaShi + hp_EquipSkill + roseProprety.Rose_HpAdd_1 + hp_ShouJiItem) * (1.0f + roseProprety.Rose_HpMul_1 + hpPro_Equip + hpPro_XiLianDaShi + hpPro_TianFu + hpPro_JingLing + hpPro_EquipSkill + hpPro_ShouJiItem) + roseProprety.Rose_HpAdd_2);
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_Hp = rose_Hp;

		int rose_Hp_ShiLi = (int)((hp_Base + hp_LvUp * rose_Lv + hp_Equip + hp_EquipSuit + hp_JingLing + hp_XiLianDaShi + hp_EquipSkill + roseProprety.Rose_HpAdd_1 + hp_ShouJiItem) * (1.0f + hpPro_Equip + hpPro_XiLianDaShi + hpPro_TianFu + hpPro_JingLing + hpPro_EquipSkill + hpPro_ShouJiItem));
        int rose_Hp_Base = (int)((hp_Base + hp_LvUp * rose_Lv + hp_Equip + hp_EquipSuit + hp_TianFu + hp_JingLing + hp_XiLianDaShi + hp_EquipSkill + roseProprety.Rose_HpAdd_1 + hp_ShouJiItem) * (1.0f + hpPro_Equip + hpPro_XiLianDaShi + hpPro_TianFu + hpPro_JingLing + hpPro_EquipSkill + hpPro_ShouJiItem));

        rose_Proprety.Rose_Hp_PropertySum = rose_Hp_Base;

        //------------------------------------更新最小攻击
        float act_BaseMin = float.Parse(functionDataSet.DataSet_ReadData("BaseMinAct", "ID", rose_Occupation, "Occupation_Template"));
        float act_LvUpMin = float.Parse(functionDataSet.DataSet_ReadData("LvUpMinAct", "ID", rose_Occupation, "Occupation_Template"));
		//act_EquipMin = 0;  //装备属性预留
		//最小攻击总和计算
		rose_ActMin = (int)((act_BaseMin + act_LvUpMin * rose_Lv + act_EquipMin + act_EquipSuitMin + act_TianFuMin + act_JingLingMin + act_XiLianDaShiMin + act_EquipSkillMin + roseProprety.Rose_ActMinAdd_1) * (1.0f + actPro_Tianfu + actPro_XiLianDaShi + actPro_JingLing + actPro_EquipSkill + roseProprety.Rose_ActMinMul_1) + roseProprety.Rose_ActMinAdd_2);
		//弹出属性提示
        if (rose_ActMin > rose_Proprety.Rose_ActMin) {
            if (ifHint) {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("攻击下限")+ "+"+ (rose_ActMin - rose_Proprety.Rose_ActMin), "7FFF00FF");
            }
        }
        //结算出结果后进行属性赋值
		rose_Proprety.Rose_ActMin = rose_ActMin;

		int rose_ActMin_ShiLi = (int)((act_BaseMin + act_LvUpMin * rose_Lv + act_EquipMin + act_EquipSuitMin + act_JingLingMin + act_XiLianDaShiMin + act_EquipSkillMin + roseProprety.Rose_ActMinAdd_1) * (1.0f + actPro_Tianfu + actPro_XiLianDaShi + actPro_JingLing + actPro_EquipSkill));
        int rose_ActMin_Base = (int)((act_BaseMin + act_LvUpMin * rose_Lv + act_EquipMin + act_EquipSuitMin + act_TianFuMin + act_JingLingMin + act_XiLianDaShiMin + act_EquipSkillMin + roseProprety.Rose_ActMinAdd_1) * (1.0f + actPro_Tianfu + actPro_XiLianDaShi + actPro_JingLing + actPro_EquipSkill));
        rose_Proprety.Rose_ActMin_PropertySum = rose_ActMin_Base;

		//------------------------------------更新最大攻击
        float act_BaseMax = float.Parse(functionDataSet.DataSet_ReadData("BaseMaxAct", "ID", rose_Occupation, "Occupation_Template"));
        float act_LvUpMax = float.Parse(functionDataSet.DataSet_ReadData("LvUpMaxAct", "ID", rose_Occupation, "Occupation_Template"));
		//act_EquipMax = 0;  //装备属性预留

		//最大攻击总和计算
        rose_ActMax = (int)((act_BaseMax + act_LvUpMax * rose_Lv + act_EquipMax + act_EquipSuitMax + act_TianFuMax + act_JingLingMax + act_XiLianDaShiMax + act_EquipSkillMax + roseProprety.Rose_ActMaxAdd_1 + actPro_Equip + act_ShouJiItemMax) * (1.0f + actPro_Tianfu + actPro_XiLianDaShi + actPro_JingLing + actPro_EquipSkill + roseProprety.Rose_ActMaxMul_1 + actPro_ShouJiItemMax) + roseProprety.Rose_ActMaxAdd_2);
        //rose_ActMax = 999999;
        //弹出属性提示
        if (rose_ActMax > rose_Proprety.Rose_ActMax)
        {
            if (ifHint){
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("攻击上限")+"+" + (rose_ActMax - rose_Proprety.Rose_ActMax), "7FFF00FF");
            }
        }

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_ActMax = rose_ActMax;

		int rose_ActMax_ShiLi = (int)((act_BaseMax + act_LvUpMax * rose_Lv + act_EquipMax + act_EquipSuitMax + act_JingLingMax + act_XiLianDaShiMax + act_EquipSkillMax + roseProprety.Rose_ActMaxAdd_1 + actPro_Equip + act_ShouJiItemMax) * (1.0f + actPro_Tianfu + actPro_XiLianDaShi + actPro_JingLing + actPro_EquipSkill + actPro_ShouJiItemMax));
        int rose_ActMax_Base = (int)((act_BaseMax + act_LvUpMax * rose_Lv + act_EquipMax + act_EquipSuitMax + act_TianFuMax + act_JingLingMax + act_XiLianDaShiMax + act_EquipSkillMax + roseProprety.Rose_ActMaxAdd_1 + actPro_Equip + act_ShouJiItemMax) * (1.0f + actPro_Tianfu + actPro_XiLianDaShi + actPro_JingLing + actPro_EquipSkill + actPro_ShouJiItemMax));
        rose_Proprety.Rose_ActMax_PropertySum = rose_ActMax_Base;

        //------------------------------------更新最小魔攻
        float magact_BaseMin = float.Parse(functionDataSet.DataSet_ReadData("BaseMinMagAct", "ID", rose_Occupation, "Occupation_Template"));
        float magact_LvUpMin = float.Parse(functionDataSet.DataSet_ReadData("LvUpMinMagAct", "ID", rose_Occupation, "Occupation_Template"));
        //act_EquipMin = 0;  //装备属性预留
        //最小攻击总和计算
		rose_MagActMin = (int)((magact_BaseMin + magact_LvUpMin * rose_Lv + magact_EquipMin + magact_EquipSuitMin + magact_TianFuMin + magact_JingLingMin + magact_XiLianDaShiMin + magact_EquipSkillMin + roseProprety.Rose_MagActMinAdd_1) * (1.0f + magactPro_TianFu + magactPro_XiLianDaShi + magactPro_JingLing + magactPro_EquipSkill + roseProprety.Rose_MagActMinMul_1) + roseProprety.Rose_MagActMinAdd_2);
        //弹出属性提示
        if (rose_MagActMin > rose_Proprety.Rose_MagActMin)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("法术下限")+"+" + (rose_MagActMin - rose_Proprety.Rose_MagActMin), "7FFF00FF");
            }
        }
        //结算出结果后进行属性赋值
        rose_Proprety.Rose_MagActMin = rose_MagActMin;

		int rose_MagActMin_ShiLi = (int)((magact_BaseMin + magact_LvUpMin * rose_Lv + magact_EquipMin + magact_EquipSuitMin + magact_JingLingMin + magact_XiLianDaShiMin + magact_EquipSkillMin + roseProprety.Rose_MagActMinAdd_1) * (1.0f + magactPro_TianFu + magactPro_XiLianDaShi + magactPro_JingLing + magactPro_EquipSkill));
        int rose_MagActMin_Base = (int)((magact_BaseMin + magact_LvUpMin * rose_Lv + magact_EquipMin + magact_EquipSuitMin + magact_TianFuMin + magact_JingLingMin + magact_XiLianDaShiMin + magact_EquipSkillMin + roseProprety.Rose_MagActMinAdd_1) * (1.0f + magactPro_TianFu + magactPro_XiLianDaShi + magactPro_JingLing + magactPro_EquipSkill));
        rose_Proprety.Rose_MagActMin_PropertySum = rose_MagActMin_Base;

        //------------------------------------更新最大魔攻
        float magact_BaseMax = float.Parse(functionDataSet.DataSet_ReadData("BaseMaxMagAct", "ID", rose_Occupation, "Occupation_Template"));
        float magact_LvUpMax = float.Parse(functionDataSet.DataSet_ReadData("LvUpMinMagAct", "ID", rose_Occupation, "Occupation_Template"));
        //magact_BaseMax = 0;
        //act_EquipMax = 0;  //装备属性预留

        //最大攻击总和计算
        rose_MagActMax = (int)((magact_BaseMax + magact_LvUpMax * rose_Lv + magact_EquipMax + magact_EquipSuitMax + magact_TianFuMax + magact_JingLingMax + magact_XiLianDaShiMax + magact_EquipSkillMax + roseProprety.Rose_MagActMaxAdd_1) * (1.0f + magactPro_Equip + magactPro_XiLianDaShi + magactPro_TianFu + magactPro_JingLing + magactPro_EquipSkill + roseProprety.Rose_MagActMaxMul_1) + roseProprety.Rose_MagActMaxAdd_2);
        //弹出属性提示
        if (rose_ActMax > rose_Proprety.Rose_ActMax)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("法术上限")+"+" + (rose_MagActMax - rose_Proprety.Rose_MagActMax), "7FFF00FF");
            }
        }
        //结算出结果后进行属性赋值
        rose_Proprety.Rose_MagActMax = rose_MagActMax;

		int rose_MagActMax_ShiLi = (int)((magact_BaseMax + magact_LvUpMax * rose_Lv + magact_EquipMax + magact_EquipSuitMax + magact_JingLingMax + magact_XiLianDaShiMax + magact_EquipSkillMax + roseProprety.Rose_MagActMaxAdd_1) * (1.0f + magactPro_Equip + magactPro_XiLianDaShi + magactPro_TianFu + magactPro_JingLing + magactPro_EquipSkill));
        int rose_MagActMax_Base = (int)((magact_BaseMax + magact_LvUpMax * rose_Lv + magact_EquipMax + magact_EquipSuitMax + magact_TianFuMax + magact_JingLingMax + magact_XiLianDaShiMax + magact_EquipSkillMax + roseProprety.Rose_MagActMaxAdd_1) * (1.0f + magactPro_Equip + magactPro_XiLianDaShi + magactPro_TianFu + magactPro_JingLing + magactPro_EquipSkill));
        rose_Proprety.Rose_MagActMax_PropertySum = rose_MagActMax_Base;

        //------------------------------------更新最小物防
        float def_BaseMin = float.Parse(functionDataSet.DataSet_ReadData("BaseMinDef", "ID", rose_Occupation, "Occupation_Template"));
        float def_LvUpMin = float.Parse(functionDataSet.DataSet_ReadData("LvUpMinDef", "ID", rose_Occupation, "Occupation_Template"));
		//def_EquipMin = 0;  //装备属性预留
		//最大攻击总和计算
		rose_DefMin = (int)((def_BaseMin + def_LvUpMin * rose_Lv + def_EquipMin + def_EquipSuitMin + def_TianFuMin + def_JingLingMin + def_XiLianDaShiMin + def_EquipMin + roseProprety.Rose_DefMinAdd_1) * (1.0f + defPro_Equip + defPro_XiLianDaShi + defPro_TianFu + defPro_JingLing + defPro_EquipSkill + roseProprety.Rose_DefMinMul_1) + roseProprety.Rose_DefMinAdd_2);
        //rose_DefMin = 999999;
        //弹出属性提示
        if (rose_DefMin > rose_Proprety.Rose_DefMin)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("防御下限")+"+" + (rose_DefMin - rose_Proprety.Rose_DefMin), "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_DefMin = rose_DefMin;

		int rose_DefMin_ShiLi = (int)((def_BaseMin + def_LvUpMin * rose_Lv + def_EquipMin + def_EquipSuitMin + def_JingLingMin + def_XiLianDaShiMin + def_EquipMin + roseProprety.Rose_DefMinAdd_1) * (1.0f + defPro_Equip + defPro_XiLianDaShi + defPro_TianFu + defPro_JingLing + defPro_EquipSkill));
        int rose_DefMin_Base = (int)((def_BaseMin + def_LvUpMin * rose_Lv + def_EquipMin + def_EquipSuitMin + def_TianFuMin + def_JingLingMin + def_XiLianDaShiMin + def_EquipMin + roseProprety.Rose_DefMinAdd_1) * (1.0f + defPro_Equip + defPro_XiLianDaShi + defPro_TianFu + defPro_JingLing + defPro_EquipSkill));
        rose_Proprety.Rose_DefMin_PropertySum = rose_DefMin_Base;

        //------------------------------------更新最大物防
        float def_BaseMax = float.Parse(functionDataSet.DataSet_ReadData("BaseMaxDef", "ID", rose_Occupation, "Occupation_Template"));
        float def_LvUpMax = float.Parse(functionDataSet.DataSet_ReadData("LvUpMaxDef", "ID", rose_Occupation, "Occupation_Template"));
		//def_EquipMax = 0;  //装备属性预留
		//最大攻击总和计算
		rose_DefMax = (int)((def_BaseMax + def_LvUpMax * rose_Lv + def_EquipMax + def_EquipSuitMax + def_TianFuMax + def_JingLingMax + def_XiLianDaShiMax + def_EquipSkillMax + roseProprety.Rose_DefMaxAdd_1 + def_ShouJiItemMax) * (1.0f + defPro_Equip + defPro_XiLianDaShi + defPro_TianFu + defPro_JingLing + defPro_EquipSkill + roseProprety.Rose_DefMaxMul_1 + defPro_ShouJiItemMax) + roseProprety.Rose_DefMaxAdd_2);
        //rose_DefMax = 99999;
        //弹出属性提示
        if (rose_DefMax > rose_Proprety.Rose_DefMax)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("防御上限")+"+"+ (rose_DefMax - rose_Proprety.Rose_DefMax), "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_DefMax = rose_DefMax;

		int rose_DefMax_ShiLi = (int)((def_BaseMax + def_LvUpMax * rose_Lv + def_EquipMax + def_EquipSuitMax + def_JingLingMax + def_XiLianDaShiMax + def_EquipSkillMax + roseProprety.Rose_DefMaxAdd_1 + def_ShouJiItemMax) * (1.0f + defPro_Equip + defPro_XiLianDaShi + defPro_TianFu + defPro_JingLing + defPro_EquipSkill + defPro_ShouJiItemMax));
        int rose_DefMax_Base = (int)((def_BaseMax + def_LvUpMax * rose_Lv + def_EquipMax + def_EquipSuitMax + def_TianFuMax + def_JingLingMax + def_XiLianDaShiMax + def_EquipSkillMax + roseProprety.Rose_DefMaxAdd_1 + def_ShouJiItemMax) * (1.0f + defPro_Equip + defPro_XiLianDaShi + defPro_TianFu + defPro_JingLing + defPro_EquipSkill + defPro_ShouJiItemMax));
        rose_Proprety.Rose_DefMax_PropertySum = rose_DefMax_Base;

        //------------------------------------更新最小魔防
        float adf_BaseMin = float.Parse(functionDataSet.DataSet_ReadData("BaseMinAdf", "ID", rose_Occupation, "Occupation_Template"));
        float adf_LvUpMin = float.Parse(functionDataSet.DataSet_ReadData("LvUpMinAdf", "ID", rose_Occupation, "Occupation_Template"));
		//adf_EquipMin = 0;  //装备属性预留
		//最大攻击总和计算
		rose_AdfMin = (int)((adf_BaseMin + adf_LvUpMin * rose_Lv + adf_EquipMin + adf_EquipSuitMin + adf_TianFuMin + adf_JingLingMin + adf_EquipMin + roseProprety.Rose_AdfMinAdd_1) * (1.0f + adfPro_Equip + adfPro_XiLianDaShi + adfPro_TianFu + adfPro_JingLing + adfPro_EquipSkill + roseProprety.Rose_AdfMinMul_1) + roseProprety.Rose_AdfMinAdd_2);
        //弹出属性提示
        if (rose_AdfMin > rose_Proprety.Rose_AdfMin)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("魔防下限")+"+" + (rose_AdfMin - rose_Proprety.Rose_AdfMin), "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_AdfMin = rose_AdfMin;

		int rose_AdfMin_ShiLi = (int)((adf_BaseMin + adf_LvUpMin * rose_Lv + adf_EquipMin + adf_EquipSuitMin + adf_JingLingMin + adf_XiLianDaShiMin + adf_EquipMin + roseProprety.Rose_AdfMinAdd_1) * (1.0f + adfPro_Equip + adfPro_XiLianDaShi + adfPro_TianFu + adfPro_JingLing + adfPro_EquipSkill));
        int rose_AdfMin_Base = (int)((adf_BaseMin + adf_LvUpMin * rose_Lv + adf_EquipMin + adf_EquipSuitMin + adf_TianFuMin + adf_JingLingMin + adf_XiLianDaShiMin + adf_EquipMin + roseProprety.Rose_AdfMinAdd_1) * (1.0f + adfPro_Equip + adfPro_XiLianDaShi + adfPro_TianFu + adfPro_JingLing + adfPro_EquipSkill));
        rose_Proprety.Rose_AdfMin_PropertySum = rose_AdfMin_Base;

        //------------------------------------更新最大魔防
        float adf_BaseMax = float.Parse(functionDataSet.DataSet_ReadData("BaseMaxAdf", "ID", rose_Occupation, "Occupation_Template"));
        float adf_LvUpMax = float.Parse(functionDataSet.DataSet_ReadData("LvUpMaxAdf", "ID", rose_Occupation, "Occupation_Template"));
		//adf_EquipMax = 0;  //装备属性预留

		//最大攻击总和计算
		rose_AdfMax = (int)((adf_BaseMax + adf_LvUpMax * rose_Lv + adf_EquipMax + adf_EquipSkillMax + adf_EquipSuitMax + adf_TianFuMax + adf_JingLingMax + adf_XiLianDaShiMax + roseProprety.Rose_AdfMaxAdd_1 + adf_ShouJiItemMax) * (1.0f + adfPro_Equip + adfPro_XiLianDaShi + adfPro_TianFu + adfPro_JingLing + adfPro_EquipSkill + roseProprety.Rose_AdfMaxMul_1 + adfPro_ShouJiItemMax) + roseProprety.Rose_AdfMaxAdd_2);
        //弹出属性提示
        if (rose_AdfMax > rose_Proprety.Rose_AdfMax)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("魔防上限")+"+" + (rose_AdfMax - rose_Proprety.Rose_AdfMax), "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_AdfMax = rose_AdfMax;

		//最大攻击总和计算
		int rose_AdfMax_ShiLi = (int)((adf_BaseMax + adf_LvUpMax * rose_Lv + adf_EquipMax + adf_EquipSkillMax + adf_EquipSuitMax + adf_JingLingMax + adf_XiLianDaShiMax + roseProprety.Rose_AdfMaxAdd_1 + adf_ShouJiItemMax) * (1.0f + adfPro_Equip + adfPro_XiLianDaShi + adfPro_TianFu + adfPro_JingLing + adfPro_EquipSkill + adfPro_ShouJiItemMax));
        int rose_AdfMax_Base = (int)((adf_BaseMax + adf_LvUpMax * rose_Lv + adf_EquipMax + adf_EquipSkillMax + adf_EquipSuitMax + adf_TianFuMax + adf_JingLingMax + adf_XiLianDaShiMax + roseProprety.Rose_AdfMaxAdd_1 + adf_ShouJiItemMax) * (1.0f + adfPro_Equip + adfPro_XiLianDaShi + adfPro_TianFu + adfPro_JingLing + adfPro_EquipSkill + adfPro_ShouJiItemMax));
        rose_Proprety.Rose_AdfMax_PropertySum = rose_AdfMax_Base;

        //------------------------------------更新移动速度
        rose_MoveSpeed = float.Parse(functionDataSet.DataSet_ReadData("BaseMoveSpeed", "ID", rose_Occupation, "Occupation_Template"));
		//rose_MoveSpeed = (rose_MoveSpeed + speed_Equip + speed_EquipSuit + speed_TianFu + speed_JingLing+ speed_XiLianDaShi +  speed_EquipSkill + roseProprety.Rose_MoveSpeedAdd_1) * (1.0f + roseProprety.Rose_MoveSpeedMul_1 + speed_ShouJiItem) + roseProprety.Rose_MoveSpeedAdd_2;
        //提示
        if (rose_MoveSpeed > rose_Proprety.Rose_MoveSpeed)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("移动速度提升") + (rose_MoveSpeed - rose_Proprety.Rose_MoveSpeed)*100+"%", "7FFF00FF");
            }
        }


        //结算出结果后进行属性赋值
        float rose_MoveSpeed_ShiLi = (rose_MoveSpeed + speed_Equip + speed_EquipSuit + speed_JingLing + speed_XiLianDaShi + speed_EquipSkill + roseProprety.Rose_MoveSpeedAdd_1) * (1.0f + speed_ShouJiItem);
        float rose_MoveSpeed_Base = (rose_MoveSpeed + speed_Equip + speed_EquipSuit + speed_TianFu + speed_JingLing + speed_XiLianDaShi + speed_EquipSkill + roseProprety.Rose_MoveSpeedAdd_1) * (1.0f + speed_ShouJiItem);
        rose_Proprety.Rose_MoveSpeed_PropertySum = rose_MoveSpeed_Base;
        rose_Proprety.Rose_MoveSpeed = rose_MoveSpeed_Base;
        rose_Proprety.rose_LastMoveSpeed = rose_Proprety.Rose_MoveSpeed;

        //------------------------------------暴击等级
        rose_CriRating = criRating_Equip + criRating_EquipSuit + criRating_TianFu + criRating_JingLing + criRating_XiLianDaShi + criRating_EquipSkill;

        //提示
        if (rose_CriRating > rose_Proprety.Rose_CriRating)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("暴击等级")+"+" + (rose_CriRating - rose_Proprety.Rose_CriRating), "7FFF00FF");
            }
        }

        ObscuredInt pro_1 = 10000;
        ObscuredInt pro_2 = 100;

        //暴击等级换算暴击率
        rose_CriRatingToPro = (float)(rose_CriRating) / (pro_1 + rose_Lv * pro_2);
        //Debug.Log("暴击等级：" + rose_CriRatingToPro  + "rose_CriRating = " + rose_CriRating);

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_CriRating = rose_CriRating;

		int rose_CriRating_ShiLi = criRating_Equip + criRating_EquipSuit + criRating_JingLing + criRating_XiLianDaShi + criRating_EquipSkill;
        int rose_CriRating_Base = criRating_Equip + criRating_EquipSuit + criRating_TianFu + criRating_JingLing + criRating_XiLianDaShi + criRating_EquipSkill;
        rose_Proprety.Rose_CriRating_PropertySum = rose_CriRating_Base;

        //------------------------------------韧性等级
        rose_ResRating = resilienceRating_Equip + resilienceRating_EquipSuit + resilienceRating_TianFu + resilienceRating_JingLing + resilienceRating_XiLianDaShi + resilienceRating_EquipSkill;

        //提示
        if (rose_ResRating > rose_Proprety.Rose_ResilienceRating)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("韧性等级")+"+" + (rose_ResRating - rose_Proprety.Rose_ResilienceRating), "7FFF00FF");
            }
        }

        //暴击等级换算暴击率
        rose_ResRatingToPro = (float)(rose_ResRating) / (pro_1 + rose_Lv * pro_2);

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_ResilienceRating = rose_ResRating;

		int rose_ResRating_ShiLi = resilienceRating_Equip + resilienceRating_EquipSuit + resilienceRating_JingLing + resilienceRating_XiLianDaShi + resilienceRating_EquipSkill;
        int rose_ResRating_Base = resilienceRating_Equip + resilienceRating_EquipSuit + resilienceRating_TianFu + resilienceRating_JingLing + resilienceRating_XiLianDaShi + resilienceRating_EquipSkill;
        rose_Proprety.Rose_ResilienceRating_PropertySum = rose_ResRating_Base;

        //------------------------------------命中等级
        rose_HitRating = hitRating_Equip + hitRating_EquipSuit + hitRating_TianFu + hitRating_JingLing + hitRating_XiLianDaShi + hitRating_EquipSkill;

        //提示
        if (rose_HitRating > rose_Proprety.Rose_HitRating)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("命中等级")+"+" + (rose_HitRating - rose_Proprety.Rose_HitRating), "7FFF00FF");
            }
        }

        //暴击等级换算暴击率
        rose_HitRatingToPro = (float)(rose_HitRating) / (pro_1 + rose_Lv * pro_2);

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_HitRating = rose_HitRating;

		int rose_HitRating_ShiLi = hitRating_Equip + hitRating_EquipSuit + hitRating_JingLing + hitRating_XiLianDaShi + hitRating_EquipSkill;
        int rose_HitRating_Base = hitRating_Equip + hitRating_EquipSuit + hitRating_TianFu + hitRating_JingLing + hitRating_XiLianDaShi + hitRating_EquipSkill;
        rose_Proprety.Rose_HitRating_PropertySum = rose_HitRating_Base;

        //------------------------------------闪避等级
        rose_DodgeRating = dodgeRating_Equip + dodgeRating_EquipSuit + dodgeRating_TianFu + dodgeRating_JingLing + dodgeRating_XiLianDaShi + dodgeRating_EquipSkill;

        //提示
        if (rose_DodgeRating > rose_Proprety.Rose_DodgeRating)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("闪避等级")+"+" + (rose_DodgeRating - rose_Proprety.Rose_DodgeRating), "7FFF00FF");
            }
        }

        //暴击等级换算暴击率
        rose_DodgeRatingToPro = (float)(rose_DodgeRating) / (pro_1 + rose_Lv * pro_2);

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_DodgeRating = rose_DodgeRating;

		int rose_DodgeRating_ShiLi = dodgeRating_Equip + dodgeRating_EquipSuit + dodgeRating_JingLing + dodgeRating_XiLianDaShi + dodgeRating_EquipSkill;
        int rose_DodgeRating_Base = dodgeRating_Equip + dodgeRating_EquipSuit + dodgeRating_TianFu + dodgeRating_JingLing + dodgeRating_XiLianDaShi + dodgeRating_EquipSkill;
        rose_Proprety.Rose_DodgeRating_PropertySum = rose_DodgeRating_Base;

        //------------------------------------更新暴击值
        float rose_Cri_Base = float.Parse(functionDataSet.DataSet_ReadData("BaseCri", "ID", rose_Occupation, "Occupation_Template"));
		rose_Cri = rose_Cri_Base + cir_Equip + cir_EquipSuit + cir_TianFu + cir_JingLing + cir_XiLianDaShi + cir_EquipSkill + roseProprety.Rose_CriMul_1 + cir_ShouJiItem + rose_CriRatingToPro;

        //提示
        if (rose_Cri > rose_Proprety.Rose_Cri)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("暴击率")+"+" + (rose_Cri - rose_Proprety.Rose_Cri) * 100 + "%", "7FFF00FF");
            }
        }

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_Cri = rose_Cri;
        
		float rose_Cri_ShiLi = rose_Cri_Base + cir_Equip + cir_EquipSuit + cir_JingLing + cir_XiLianDaShi + cir_EquipSkill + cir_ShouJiItem;
        rose_Proprety.Rose_Cri_PropertySum = rose_Cri;

        //------------------------------------更新命中值
        float rose_Hit_Base = float.Parse(functionDataSet.DataSet_ReadData("BaseHit", "ID", rose_Occupation, "Occupation_Template"));
		rose_Hit = rose_Hit_Base + hit_Equip + hit_EquipSuit + hit_TianFu + hit_JingLing + hit_XiLianDaShi + hit_EquipSkill + rose_Proprety.Rose_HitMul_1 + hit_ShouJiItem + rose_HitRatingToPro;

        //提示
        if (rose_Hit > rose_Proprety.Rose_Hit)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("命中率") + "+" + (rose_Hit - rose_Proprety.Rose_Hit) * 100 + "%", "7FFF00FF");
            }
        }

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_Hit = rose_Hit;

		float rose_Hit_ShiLi = hit_Equip + hit_EquipSuit + hit_JingLing + hit_XiLianDaShi + hit_EquipSkill + hit_ShouJiItem;
        rose_Proprety.Rose_Hit_PropertySum = rose_Hit;

        //------------------------------------更新闪避值
        float rose_Dodge_base = float.Parse(functionDataSet.DataSet_ReadData("BaseDodge", "ID", rose_Occupation, "Occupation_Template"));
		rose_Dodge = rose_Dodge_base + dodge_Equip + dodge_EquipSuit + dodge_TianFu + dodge_JingLing + dodge_XiLianDaShi + dodge_EquipSkill + dodge_EquipSkill + rose_Proprety.Rose_DodgeMul_1 + dodge_ShouJiItem + rose_DodgeRatingToPro;

        //提示
        if (rose_Dodge > rose_Proprety.Rose_Dodge)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("闪避率")+"+" + (rose_Dodge - rose_Proprety.Rose_Dodge) * 100 + "%", "7FFF00FF");
            }
        }
			
        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Dodge = rose_Dodge;

		float rose_Dodge_ShiLi =  dodge_Equip + dodge_EquipSuit + dodge_JingLing + dodge_XiLianDaShi + dodge_EquipSkill + dodge_EquipSkill + dodge_ShouJiItem;
        rose_Proprety.Rose_Dodge_PropertySum = rose_Dodge;

        //------------------------------------更新格挡值
        rose_GeDangValue = geDangValue_Equip + geDangValue_EquipSuit + geDangValue_TianFu + geDangValue_JingLing + geDangValue_XiLianDaShi;

        //提示
        if (rose_GeDangValue > rose_Proprety.Rose_GeDangValue)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("格挡值")+"+" + (rose_GeDangValue - rose_Proprety.Rose_GeDangValue), "7FFF00FF");
            }
        }
		
		//结算出结果后进行属性赋值
        rose_Proprety.Rose_GeDangValue = rose_GeDangValue;

		int rose_GeDangValue_ShiLi = geDangValue_Equip + geDangValue_EquipSuit + geDangValue_JingLing + geDangValue_XiLianDaShi;
        int rose_GeDangValue_Base = geDangValue_Equip + geDangValue_EquipSuit + geDangValue_TianFu + geDangValue_JingLing + geDangValue_XiLianDaShi;
        rose_Proprety.Rose_GeDangValue_PropertySum = rose_GeDangValue_Base;


        //------------------------------------更新重击概率
        rose_ZhongJiPro = zhongJiPro_Equip + zhongJiPro_EquipSuit + zhongJiPro_TianFu + zhongJiPro_JingLing + zhongJiPro_XiLianDaShi + zhongJiPro_EquipSkill;

        //提示
        if (rose_ZhongJiPro > rose_Proprety.Rose_ZhongJiPro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("重击概率")+"+" + (rose_ZhongJiPro - rose_Proprety.Rose_ZhongJiPro) * 100 + "%", "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_ZhongJiPro = rose_ZhongJiPro;

		float rose_ZhongJiPro_ShiLi = zhongJiPro_Equip + zhongJiPro_EquipSuit + zhongJiPro_JingLing + zhongJiPro_XiLianDaShi + zhongJiPro_EquipSkill;
        float rose_ZhongJiPro_Base = zhongJiPro_Equip + zhongJiPro_EquipSuit + zhongJiPro_TianFu + zhongJiPro_JingLing + zhongJiPro_XiLianDaShi + zhongJiPro_EquipSkill;
        rose_Proprety.Rose_ZhongJiPro_PropertySum = rose_ZhongJiPro_Base;

        //------------------------------------更新重击额外值
        rose_ZhongJiValue = zhongJiValue_Equip + zhongJiValue_EquipSuit + zhongJiValue_TianFu + zhongJiValue_JingLing + zhongJiValue_XiLianDaShi + zhongJiValue_EquipSkill;

        //提示
        if (rose_ZhongJiValue > rose_Proprety.Rose_ZhongJiValue)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("重击附加伤害")+"+" + (rose_ZhongJiValue - rose_Proprety.Rose_ZhongJiValue), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_ZhongJiValue = rose_ZhongJiValue;

		int rose_ZhongJiValue_ShiLi = zhongJiValue_Equip + zhongJiValue_EquipSuit + zhongJiValue_JingLing + zhongJiValue_XiLianDaShi + zhongJiValue_EquipSkill;
        int rose_ZhongJiValue_Base = zhongJiValue_Equip + zhongJiValue_EquipSuit + zhongJiValue_TianFu + zhongJiValue_JingLing + zhongJiValue_XiLianDaShi + zhongJiValue_EquipSkill;
        rose_Proprety.Rose_ZhongJiValue_PropertySum = rose_ZhongJiValue_Base;

        //------------------------------------更新攻击附加伤害额外值
        rose_GuDingValue = guDingValue_Equip + guDingValue_EquipSuit + guDingValue_TianFu + guDingValue_JingLing + guDingValue_XiLianDaShi + guDingValue_EquipSkill;

        //提示
        if (rose_GuDingValue > rose_Proprety.Rose_GuDingValue)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("攻击附加伤害")+"+" + (rose_GuDingValue - rose_Proprety.Rose_GuDingValue), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_GuDingValue = rose_GuDingValue;

		int rose_GuDingValue_ShiLi = guDingValue_Equip + guDingValue_EquipSuit + guDingValue_JingLing + guDingValue_XiLianDaShi + guDingValue_EquipSkill;
        int rose_GuDingValue_Base = guDingValue_Equip + guDingValue_EquipSuit + guDingValue_TianFu + guDingValue_JingLing + guDingValue_XiLianDaShi + guDingValue_EquipSkill;
        rose_Proprety.Rose_GuDingValue_PropertySum = rose_GuDingValue_Base;

        //------------------------------------忽视目标防御值
        rose_HuShiDefValue = huShiDefValue_Equip + huShiDefValue_EquipSuit + huShiDefValue_TianFu + huShiDefValue_JingLing + huShiDefValue_XiLianDaShi + huShiDefValue_EquipSkill;

        //提示
        if (rose_HuShiDefValue > rose_Proprety.Rose_HuShiDefValue)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("忽视目标防御值")+"+" + (rose_HuShiDefValue - rose_Proprety.Rose_HuShiDefValue), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_HuShiDefValue = rose_HuShiDefValue;

		int rose_HuShiDefValue_ShiLi = huShiDefValue_Equip + huShiDefValue_EquipSuit + huShiDefValue_JingLing + huShiDefValue_XiLianDaShi + huShiDefValue_EquipSkill;
        int rose_HuShiDefValue_Base = huShiDefValue_Equip + huShiDefValue_EquipSuit + huShiDefValue_TianFu + huShiDefValue_JingLing + huShiDefValue_XiLianDaShi + huShiDefValue_EquipSkill;
        rose_Proprety.Rose_HuShiDefValue_PropertySum = rose_HuShiDefValue_Base;

        //------------------------------------忽视目标魔防值
        rose_HuShiAdfValue = huShiAdfValue_Equip + huShiAdfValue_EquipSuit + huShiAdfValue_TianFu + huShiAdfValue_JingLing + huShiAdfValue_XiLianDaShi + huShiAdfValue_EquipSkill;

        //提示
        if (rose_HuShiAdfValue > rose_Proprety.Rose_HuShiAdfValue)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("忽视目标魔防值")+"+" + (rose_HuShiAdfValue - rose_Proprety.Rose_HuShiAdfValue), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_HuShiAdfValue = rose_HuShiAdfValue;

		int rose_HuShiAdfValue_ShiLi = huShiAdfValue_Equip + huShiAdfValue_EquipSuit + huShiAdfValue_JingLing + huShiAdfValue_XiLianDaShi + huShiAdfValue_EquipSkill;
        int rose_HuShiAdfValue_Base = huShiAdfValue_Equip + huShiAdfValue_EquipSuit + huShiAdfValue_TianFu + huShiAdfValue_JingLing + huShiAdfValue_XiLianDaShi + huShiAdfValue_EquipSkill;
        rose_Proprety.Rose_HuShiAdfValue_PropertySum = rose_HuShiAdfValue_Base;

        //------------------------------------忽视目标防御值百分比
        rose_HuShiDefValuePro = huShiDefValuePro_Equip + huShiDefValuePro_EquipSuit + huShiDefValuePro_TianFu + huShiDefValuePro_JingLing + huShiDefValuePro_XiLianDaShi + huShiDefValuePro_EquipSkill;

        //提示
        if (rose_HuShiDefValuePro > rose_Proprety.Rose_HuShiDefValuePro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("忽视目标防御值")+"+" + (rose_HuShiDefValuePro - rose_Proprety.Rose_HuShiDefValuePro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_HuShiDefValuePro = rose_HuShiDefValuePro;

		float rose_HuShiDefValuePro_ShiLi = huShiDefValuePro_Equip + huShiDefValuePro_EquipSuit + huShiDefValuePro_JingLing + huShiDefValuePro_XiLianDaShi + huShiDefValuePro_EquipSkill;
        float rose_HuShiDefValuePro_Base = huShiDefValuePro_Equip + huShiDefValuePro_EquipSuit + huShiDefValuePro_TianFu + huShiDefValuePro_JingLing + huShiDefValuePro_XiLianDaShi + huShiDefValuePro_EquipSkill;
        rose_Proprety.Rose_HuShiDefValuePro_PropertySum = rose_HuShiDefValuePro_Base;

        //------------------------------------忽视目标魔防值百分比
        rose_HuShiAdfValuePro = huShiAdfValuePro_Equip + huShiAdfValuePro_EquipSuit + huShiAdfValuePro_TianFu + huShiAdfValuePro_JingLing + huShiAdfValuePro_XiLianDaShi + huShiAdfValuePro_EquipSkill;

        //提示
        if (rose_HuShiAdfValuePro > rose_Proprety.Rose_HuShiAdfValuePro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("忽视目标魔防值")+"+" + (rose_HuShiAdfValuePro - rose_Proprety.Rose_HuShiAdfValuePro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_HuShiAdfValuePro = rose_HuShiAdfValuePro;

		float rose_HuShiAdfValuePro_ShiLi = huShiAdfValuePro_Equip + huShiAdfValuePro_EquipSuit + huShiAdfValuePro_JingLing + huShiAdfValuePro_XiLianDaShi + huShiAdfValuePro_EquipSkill;
        float rose_HuShiAdfValuePro_Base = huShiAdfValuePro_Equip + huShiAdfValuePro_EquipSuit + huShiAdfValuePro_TianFu + huShiAdfValuePro_JingLing + huShiAdfValuePro_XiLianDaShi + huShiAdfValuePro_EquipSkill;
        rose_Proprety.Rose_HuShiAdfValuePro_PropertySum = rose_HuShiAdfValuePro_Base;

        //------------------------------------吸血值
        rose_XiXuePro = xiXuePro_Equip + xiXuePro_EquipSuit + xiXuePro_TianFu + xiXuePro_JingLing + xiXuePro_XiLianDaShi + xiXuePro_EquipSkill;

        //提示
        if (rose_XiXuePro > rose_Proprety.Rose_XiXuePro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("吸血")+"+" + (rose_XiXuePro - rose_Proprety.Rose_XiXuePro) * 100 + "%", "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_XiXuePro = rose_XiXuePro;

		float rose_XiXuePro_ShiLi = xiXuePro_Equip + xiXuePro_EquipSuit + xiXuePro_JingLing + xiXuePro_XiLianDaShi + xiXuePro_EquipSkill;
        rose_Proprety.Rose_XiXuePro_PropertySum = rose_XiXuePro;

        //------------------------------------更新物理免伤
        float rose_DefAdd_Base = float.Parse(functionDataSet.DataSet_ReadData("BaseDefAdd", "ID", rose_Occupation, "Occupation_Template"));
		rose_DefAdd = rose_DefAdd_Base + defAdd_Equip + defAdd_TianFu + defAdd_JingLing + defAdd_XiLianDaShi + defAdd_EquipSkill + rose_Proprety.Rose_DefMul_1 + defAdd_ShouJiItem;

        //提示
        if (rose_DefAdd > rose_Proprety.Rose_DefAdd)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("物理免伤提升") + (rose_DefAdd - rose_Proprety.Rose_DefAdd) * 100 + "%", "7FFF00FF");
            }
        }

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_DefAdd = rose_DefAdd;

		float rose_DefAdd_ShiLi = defAdd_Equip + defAdd_JingLing + defAdd_XiLianDaShi + defAdd_EquipSkill + defAdd_ShouJiItem;
        rose_Proprety.Rose_DefAdd_PropertySum = rose_DefAdd;

        //------------------------------------更新魔法免伤
        float rose_AdfAdd_Base = float.Parse(functionDataSet.DataSet_ReadData("BaseAdfAdd", "ID", rose_Occupation, "Occupation_Template"));
		rose_AdfAdd = rose_AdfAdd_Base + adfAdd_Equip + adfAdd_TianFu + adfAdd_JingLing + adfAdd_XiLianDaShi + adfAdd_EquipSkill + rose_Proprety.Rose_AdfMul_1 + adfAdd_ShouJiItem;
        //提示
        if (rose_AdfAdd > rose_Proprety.Rose_AdfAdd)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("魔法免伤提升") + (rose_AdfAdd - rose_Proprety.Rose_AdfAdd) * 100 + "%", "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_AdfAdd = rose_AdfAdd;

		float rose_AdfAdd_ShiLi = adfAdd_Equip + adfAdd_JingLing + adfAdd_XiLianDaShi + adfAdd_EquipSkill + adfAdd_ShouJiItem;
        rose_Proprety.Rose_AdfAdd_PropertySum = rose_AdfAdd;

        //------------------------------------更新总免伤
        float rose_DamgeSub_Base = float.Parse(functionDataSet.DataSet_ReadData("DamgeAdd", "ID", rose_Occupation, "Occupation_Template"));
		rose_DamgeSub = rose_DamgeSub_Base + damgeSub_Equip + damgeSub_EquipSuit + damgeSub_ShouJiItem + damgeSub_EquipSkill + damgeSub_TianFu + damgeSub_JingLing + damgeSub_XiLianDaShi;
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_DamgeSub = rose_DamgeSub;

		float rose_DamgeSub_ShiLi = damgeSub_Equip + damgeSub_EquipSuit + damgeSub_ShouJiItem + damgeSub_EquipSkill + damgeSub_JingLing + damgeSub_XiLianDaShi;
        rose_Proprety.Rose_DamgeSub_PropertySum = rose_DamgeSub;

        //------------------------------------更新伤害加成
        float rose_DamgeAdd_Base = 0;
		rose_DamgeAdd = rose_DamgeAdd_Base + damgeAdd_Equip + damgeAdd_EquipSuit + damgeAdd_ShouJiItem + damgeAdd_TianFu + damgeAdd_JingLing + damgeAdd_XiLianDaShi + damgeAdd_EquipSkill;
        //结算出结果后进行属性赋值
        rose_Proprety.Rose_DamgeAdd = rose_DamgeAdd;

		float rose_DamgeAdd_ShiLi = rose_DamgeAdd_Base + damgeAdd_Equip + damgeAdd_EquipSuit + damgeAdd_ShouJiItem + damgeAdd_JingLing + damgeAdd_XiLianDaShi + damgeAdd_EquipSkill;
        rose_Proprety.Rose_DamgeAdd_PropertySum = rose_DamgeAdd;

        //------------------------------------更新幸运值
        float rose_Lucky_Base = 0;
		rose_Lucky = rose_Lucky_Base + lucky_Equip + lucky_EquipSuit + lucky_ShouJiItem + lucky_TianFu + lucky_JingLing + lucky_XiLianDaShi + lucky_EquipSkill;
        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Lucky = rose_Lucky;

		float rose_Lucky_ShiLi = rose_Lucky_Base + lucky_Equip + lucky_EquipSuit + lucky_ShouJiItem + lucky_JingLing + lucky_XiLianDaShi + lucky_EquipSkill;
        float rose_LuckyBase = rose_Lucky_Base + lucky_Equip + lucky_EquipSuit + lucky_ShouJiItem + lucky_TianFu + lucky_JingLing + lucky_XiLianDaShi + lucky_EquipSkill;
        rose_Proprety.Rose_Lucky_PropertySum = rose_LuckyBase;
        //最大为9
        if (rose_Lucky_ShiLi > 9) {
            rose_Lucky_ShiLi = 9;
        }

        //------------------------------------韧性
        rose_Resilience = resilience_Equip + resilience_EquipSuit + resilience_TianFu + resilience_JingLing + resilience_XiLianDaShi + rose_ResRatingToPro + resilience_EquipSkill + rose_Proprety.Rose_ResilienceRatingMul_1;

        //提示
        if (rose_Resilience > rose_Proprety.Rose_Res)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("韧性")+ "+" + (rose_Resilience - rose_Proprety.Rose_Res), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Res = rose_Resilience;

		float rose_Resilience_ShiLi = resilience_Equip + resilience_EquipSuit + resilience_JingLing + resilience_XiLianDaShi + resilience_EquipSkill;
        float rose_Resilience_Base = resilience_Equip + resilience_EquipSuit + resilience_TianFu + resilience_JingLing + resilience_XiLianDaShi + resilience_EquipSkill;
        rose_Proprety.Rose_Res_PropertySum = rose_Resilience_Base + rose_ResRatingToPro;

        //------------------------------------法术反击
        rose_MagicRebound = magicRebound_Equip + magicRebound_EquipSuit + magicRebound_TianFu + magicRebound_JingLing + magicRebound_XiLianDaShi + magicRebound_EquipSkill;

        //提示
        if (rose_MagicRebound > rose_Proprety.Rose_MagicRebound)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("法术反击")+"+" + (rose_MagicRebound - rose_Proprety.Rose_MagicRebound), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_MagicRebound = rose_MagicRebound;

		float rose_MagicRebound_ShiLi = magicRebound_Equip + magicRebound_EquipSuit + magicRebound_JingLing + magicRebound_XiLianDaShi + magicRebound_EquipSkill;
        float rose_MagicRebound_Base = magicRebound_Equip + magicRebound_EquipSuit + magicRebound_TianFu + magicRebound_JingLing + magicRebound_XiLianDaShi + magicRebound_EquipSkill;
        rose_Proprety.Rose_MagicRebound_PropertySum = rose_MagicRebound_Base;

        //------------------------------------攻击反击
        rose_ActRebound = actRebound_Equip + actRebound_EquipSuit + actRebound_TianFu + actRebound_JingLing + actRebound_XiLianDaShi + actRebound_EquipSkill;

        //提示
        if (rose_ActRebound > rose_Proprety.Rose_ActRebound)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("攻击反击")+"+" + (rose_ActRebound - rose_Proprety.Rose_ActRebound), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_ActRebound = rose_ActRebound;

		float rose_ActRebound_ShiLi = actRebound_Equip + actRebound_EquipSuit + actRebound_JingLing + actRebound_XiLianDaShi + actRebound_EquipSkill;
        float rose_ActRebound_Base = actRebound_Equip + actRebound_EquipSuit + actRebound_TianFu + actRebound_JingLing + actRebound_XiLianDaShi + actRebound_EquipSkill;
        rose_Proprety.Rose_ActRebound_PropertySum = rose_ActRebound_Base;

        //------------------------------------回血固定值
        rose_HealHpValue = rose_HealHpValue_Equip + rose_HealHpValue_EquipSuit + rose_HealHpValue_TianFu + rose_HealHpValue_JingLing + rose_HealHpValue_XiLianDaShi + rose_HealHpValue_EquipSkill;

        //提示
        if (rose_HealHpValue > rose_Proprety.Rose_HealHpValue)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("回血值")+"+" + (rose_HealHpValue - rose_Proprety.Rose_HealHpValue), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_HealHpValue = rose_HealHpValue;

		int rose_HealHpValue_ShiLi = rose_HealHpValue_Equip + rose_HealHpValue_EquipSuit + rose_HealHpValue_JingLing + rose_HealHpValue_XiLianDaShi + rose_HealHpValue_EquipSkill;
        int rose_HealHpValue_Base = rose_HealHpValue_Equip + rose_HealHpValue_EquipSuit + rose_HealHpValue_TianFu + rose_HealHpValue_JingLing + rose_HealHpValue_XiLianDaShi + rose_HealHpValue_EquipSkill;
        rose_Proprety.Rose_HealHpValue_PropertySum = rose_HealHpValue_Base;

        //------------------------------------回血百分比
        rose_HealHpPro = rose_HealHpPro_Equip + rose_HealHpPro_EquipSuit + rose_HealHpPro_TianFu + rose_HealHpPro_JingLing + rose_HealHpPro_XiLianDaShi + rose_HealHpPro_EquipSkill;

        //提示
        if (rose_HealHpPro > rose_Proprety.Rose_HealHpPro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("回血百分比")+"+" + (rose_HealHpPro - rose_Proprety.Rose_HealHpPro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_HealHpPro = rose_HealHpPro;

		float rose_HealHpPro_ShiLi = rose_HealHpPro_Equip + rose_HealHpPro_EquipSuit + rose_HealHpPro_JingLing + rose_HealHpPro_XiLianDaShi + rose_HealHpPro_EquipSkill;
        float rose_HealHpPro_Base = rose_HealHpPro_Equip + rose_HealHpPro_EquipSuit + rose_HealHpPro_TianFu + rose_HealHpPro_JingLing + rose_HealHpPro_XiLianDaShi + rose_HealHpPro_EquipSkill;
        rose_Proprety.Rose_HealHpPro_PropertySum = rose_HealHpPro_Base;

        //------------------------------------战斗回血百分比
        rose_HealHpFightPro = rose_HealHpFightPro_Equip + rose_HealHpFightPro_EquipSuit + rose_HealHpFightPro_TianFu + rose_HealHpFightPro_JingLing + rose_HealHpFightPro_XiLianDaShi + rose_HealHpFightPro_EquipSkill;


        //最大值限制
        if (rose_HealHpFightPro > 2f)
        {
            rose_HealHpFightPro = 2f;
        }

        //提示
        if (rose_HealHpFightPro > rose_Proprety.Rose_HealHpFightPro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("战斗回血百分比")+"+" + (rose_HealHpFightPro - rose_Proprety.Rose_HealHpFightPro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_HealHpFightPro = rose_HealHpFightPro;

		float rose_HealHpFightPro_ShiLi = rose_HealHpFightPro_Equip + rose_HealHpFightPro_EquipSuit + rose_HealHpFightPro_JingLing + rose_HealHpFightPro_XiLianDaShi + rose_HealHpFightPro_EquipSkill;
        float rose_HealHpFightPro_Base = rose_HealHpFightPro_Equip + rose_HealHpFightPro_EquipSuit + rose_HealHpFightPro_TianFu + rose_HealHpFightPro_JingLing + rose_HealHpFightPro_XiLianDaShi + rose_HealHpFightPro_EquipSkill;
        rose_Proprety.Rose_HealHpFightPro_PropertySum = rose_HealHpFightPro_Base;

        //------------------------------------光抗性
        rose_Resistance_1 = resistance_1_Equip + resistance_1_EquipSuit + resistance_1_TianFu + resistance_1_JingLing + resistance_1_XiLianDaShi + resistance_1_EquipSkill;

        //提示
        if (rose_Resistance_1 > rose_Proprety.Rose_Resistance_1)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("光抗性")+"+" + (rose_Resistance_1 - rose_Proprety.Rose_Resistance_1), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Resistance_1 = rose_Resistance_1;

		float rose_Resistance_1_ShiLi = resistance_1_Equip + resistance_1_EquipSuit + resistance_1_JingLing + resistance_1_XiLianDaShi + resistance_1_EquipSkill;
        float rose_Resistance_1_Base = resistance_1_Equip + resistance_1_EquipSuit + resistance_1_TianFu + resistance_1_JingLing + resistance_1_XiLianDaShi + resistance_1_EquipSkill;
        rose_Proprety.Rose_Resistance_1_PropertySum = rose_Resistance_1_Base;

        //------------------------------------暗抗性
        rose_Resistance_2 = resistance_2_Equip + resistance_2_EquipSuit + resistance_2_TianFu + resistance_2_JingLing + resistance_2_XiLianDaShi + resistance_2_EquipSkill;

        //提示
        if (rose_Resistance_2 > rose_Proprety.Rose_Resistance_2)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("暗抗性")+"+" + (rose_Resistance_2 - rose_Proprety.Rose_Resistance_2), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Resistance_2 = rose_Resistance_2;

		float rose_Resistance_2_ShiLi = resistance_2_Equip + resistance_2_EquipSuit + resistance_2_JingLing + resistance_2_XiLianDaShi + resistance_2_EquipSkill;
        float rose_Resistance_2_Base = resistance_2_Equip + resistance_2_EquipSuit + resistance_2_TianFu + resistance_2_JingLing + resistance_2_XiLianDaShi + resistance_2_EquipSkill;
        rose_Proprety.Rose_Resistance_2_PropertySum = rose_Resistance_2_Base;

        //------------------------------------火抗性
        rose_Resistance_3 = resistance_3_Equip + resistance_3_EquipSuit + resistance_3_TianFu + resistance_3_JingLing + resistance_3_XiLianDaShi + resistance_3_EquipSkill;

        //提示
        if (rose_Resistance_3 > rose_Proprety.Rose_Resistance_3)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("火抗性")+"+" + (rose_Resistance_3 - rose_Proprety.Rose_Resistance_3), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Resistance_3 = rose_Resistance_3;

		float rose_Resistance_3_ShiLi = resistance_3_Equip + resistance_3_EquipSuit + resistance_3_JingLing + resistance_3_XiLianDaShi + resistance_3_EquipSkill;
        float rose_Resistance_3_Base = resistance_3_Equip + resistance_3_EquipSuit + resistance_3_TianFu + resistance_3_JingLing + resistance_3_XiLianDaShi + resistance_3_EquipSkill;
        rose_Proprety.Rose_Resistance_3_PropertySum = rose_Resistance_3_Base;

        //------------------------------------水抗性
        rose_Resistance_4 = resistance_4_Equip + resistance_4_EquipSuit + resistance_4_TianFu + resistance_4_JingLing + resistance_4_XiLianDaShi + resistance_4_EquipSkill;

        //提示
        if (rose_Resistance_4 > rose_Proprety.Rose_Resistance_4)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("水抗性")+"+" + (rose_Resistance_4 - rose_Proprety.Rose_Resistance_4), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Resistance_4 = rose_Resistance_4;

		float rose_Resistance_4_ShiLi = resistance_4_Equip + resistance_4_EquipSuit + resistance_4_JingLing + resistance_4_XiLianDaShi + resistance_4_EquipSkill;
        float rose_Resistance_4_Base = resistance_4_Equip + resistance_4_EquipSuit + resistance_4_TianFu + resistance_4_JingLing + resistance_4_XiLianDaShi + resistance_4_EquipSkill;
        rose_Proprety.Rose_Resistance_4_PropertySum = rose_Resistance_4_Base;

        //------------------------------------电抗性
        rose_Resistance_5 = resistance_5_Equip + resistance_5_EquipSuit + resistance_5_TianFu + resistance_5_JingLing + resistance_5_XiLianDaShi + resistance_5_EquipSkill;

        //提示
        if (rose_Resistance_5 > rose_Proprety.Rose_Resistance_5)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("电抗性")+"+" + (rose_Resistance_5 - rose_Proprety.Rose_Resistance_5), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Resistance_5 = rose_Resistance_5;

		float rose_Resistance_5_ShiLi = resistance_5_Equip + resistance_5_EquipSuit + resistance_5_JingLing + resistance_5_XiLianDaShi + resistance_5_EquipSkill;
        float rose_Resistance_5_Base = resistance_5_Equip + resistance_5_EquipSuit + resistance_5_TianFu + resistance_5_JingLing + resistance_5_XiLianDaShi + resistance_5_EquipSkill;
        rose_Proprety.Rose_Resistance_5_PropertySum = rose_Resistance_5_Base;

        //------------------------------------野兽攻击抗性
        rose_RaceResistance_1 = raceResistance_1_Equip + raceResistance_1_EquipSuit + raceResistance_1_TianFu + raceResistance_1_JingLing + raceResistance_1_XiLianDaShi + raceResistance_1_EquipSkill;

        //提示
        if (rose_RaceResistance_1 > rose_Proprety.Rose_RaceResistance_1)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("野兽攻击抗性")+"+" + (rose_RaceResistance_1 - rose_Proprety.Rose_RaceResistance_1), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_RaceResistance_1 = rose_RaceResistance_1;

		float rose_RaceResistance_1_ShiLi = raceResistance_1_Equip + raceResistance_1_EquipSuit + raceResistance_1_JingLing + raceResistance_1_XiLianDaShi + raceResistance_1_EquipSkill;
        float rose_RaceResistance_1_Base = raceResistance_1_Equip + raceResistance_1_EquipSuit + raceResistance_1_TianFu + raceResistance_1_JingLing + raceResistance_1_XiLianDaShi + raceResistance_1_EquipSkill;
        rose_Proprety.Rose_RaceResistance_1_PropertySum = rose_RaceResistance_1_Base;

        //------------------------------------人形攻击抗性
        rose_RaceResistance_2 = raceResistance_2_Equip + raceResistance_2_EquipSuit + raceResistance_2_TianFu + raceResistance_2_JingLing + raceResistance_2_XiLianDaShi + raceResistance_2_EquipSkill;

        //提示
        if (rose_RaceResistance_2 > rose_Proprety.Rose_RaceResistance_2)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("人形攻击抗性")+"+" + (rose_RaceResistance_2 - rose_Proprety.Rose_RaceResistance_2), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_RaceResistance_2 = rose_RaceResistance_2;

		float rose_RaceResistance_2_ShiLi = raceResistance_2_Equip + raceResistance_2_EquipSuit + raceResistance_2_JingLing + raceResistance_2_XiLianDaShi + raceResistance_2_EquipSkill;
        float rose_RaceResistance_2_Base = raceResistance_2_Equip + raceResistance_2_EquipSuit + raceResistance_2_TianFu + raceResistance_2_JingLing + raceResistance_2_XiLianDaShi + raceResistance_2_EquipSkill;
        rose_Proprety.Rose_RaceResistance_2_PropertySum = rose_RaceResistance_2_Base;

        //------------------------------------恶魔攻击抗性
        rose_RaceResistance_3 = raceResistance_3_Equip + raceResistance_3_EquipSuit + raceResistance_3_TianFu + raceResistance_3_JingLing + raceResistance_3_XiLianDaShi + raceResistance_3_EquipSkill;

        //提示
        if (rose_RaceResistance_3 > rose_Proprety.Rose_RaceResistance_3)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("恶魔攻击抗性")+"+" + (rose_RaceResistance_3 - rose_Proprety.Rose_RaceResistance_3), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_RaceResistance_3 = rose_RaceResistance_3;

		float rose_RaceResistance_3_ShiLi = raceResistance_3_Equip + raceResistance_3_EquipSuit + raceResistance_3_JingLing + raceResistance_3_XiLianDaShi + raceResistance_3_EquipSkill;
        float rose_RaceResistance_3_Base = raceResistance_3_Equip + raceResistance_3_EquipSuit + raceResistance_3_TianFu + raceResistance_3_JingLing + raceResistance_3_XiLianDaShi + raceResistance_3_EquipSkill;
        rose_Proprety.Rose_RaceResistance_3_PropertySum = rose_RaceResistance_3_Base;

        //------------------------------------野兽攻击伤害
        rose_RaceDamge_1 = raceDamge_1_Equip + raceDamge_1_EquipSuit + raceDamge_1_TianFu + raceDamge_1_JingLing + raceDamge_1_XiLianDaShi + raceDamge_1_EquipSkill;

		//提示
		if (rose_RaceDamge_1 > rose_Proprety.Rose_RaceDamge_1)
		{
			if (ifHint)
			{
				Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("野兽攻击伤害")+"+" + (rose_RaceDamge_1 - rose_Proprety.Rose_RaceDamge_1), "7FFF00FF");
			}
		}

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_RaceDamge_1 = rose_RaceDamge_1;

		float rose_RaceDamge_1_ShiLi = raceDamge_1_Equip + raceDamge_1_EquipSuit + raceDamge_1_JingLing + raceDamge_1_XiLianDaShi + raceDamge_1_EquipSkill;
        float rose_RaceDamge_1_Base = raceDamge_1_Equip + raceDamge_1_EquipSuit + raceDamge_1_TianFu + raceDamge_1_JingLing + raceDamge_1_XiLianDaShi + raceDamge_1_EquipSkill;
        rose_Proprety.Rose_RaceDamge_1_PropertySum = rose_RaceDamge_1_Base;

        //------------------------------------人形攻击伤害
        rose_RaceDamge_2 = raceDamge_2_Equip + raceDamge_2_EquipSuit + raceDamge_2_TianFu + raceDamge_2_JingLing + raceDamge_2_XiLianDaShi + raceDamge_2_EquipSkill;

		//提示
		if (rose_RaceDamge_2 > rose_Proprety.Rose_RaceDamge_2)
		{
			if (ifHint)
			{
				Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("人形攻击伤害")+"+" + (rose_RaceDamge_2 - rose_Proprety.Rose_RaceDamge_2), "7FFF00FF");
			}
		}

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_RaceDamge_2 = rose_RaceDamge_2;

		float rose_RaceDamge_2_ShiLi = raceDamge_2_Equip + raceDamge_2_EquipSuit + raceDamge_2_JingLing + raceDamge_2_XiLianDaShi + raceDamge_2_EquipSkill;
        float rose_RaceDamge_2_Base = raceDamge_2_Equip + raceDamge_2_EquipSuit + raceDamge_2_TianFu + raceDamge_2_JingLing + raceDamge_2_XiLianDaShi + raceDamge_2_EquipSkill;
        rose_Proprety.Rose_RaceDamge_2_PropertySum = rose_RaceDamge_2_Base;


        //------------------------------------恶魔攻击伤害
        rose_RaceDamge_3 = raceDamge_3_Equip + raceDamge_3_EquipSuit + raceDamge_3_TianFu + raceDamge_3_JingLing + raceDamge_3_XiLianDaShi + raceDamge_3_EquipSkill + rose_Proprety.Rose_RaceDamge_3_Add;

		//提示
		if (rose_RaceDamge_3 > rose_Proprety.Rose_RaceDamge_3)
		{
			if (ifHint)
			{
				Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("恶魔攻击伤害")+"+" + (rose_RaceDamge_3 - rose_Proprety.Rose_RaceDamge_3), "7FFF00FF");
			}
		}

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_RaceDamge_3 = rose_RaceDamge_3;

		float rose_RaceDamge_3_ShiLi = raceDamge_3_Equip + raceDamge_3_EquipSuit + raceDamge_3_JingLing + raceDamge_3_XiLianDaShi + raceDamge_3_EquipSkill;
        float rose_RaceDamge_3_Base = raceDamge_3_Equip + raceDamge_3_EquipSuit + raceDamge_3_TianFu + raceDamge_3_JingLing + raceDamge_3_XiLianDaShi + raceDamge_3_EquipSkill;
        rose_Proprety.Rose_RaceDamge_3_PropertySum = rose_RaceDamge_3_Base;


        //------------------------------------Boss普通攻击加成
        rose_Boss_ActAdd = rose_Boss_ActAdd_Equip + rose_Boss_ActAdd_EquipSuit + rose_Boss_ActAdd_TianFu + rose_Boss_ActAdd_JingLing + rose_Boss_ActAdd_XiLianDaShi + rose_Boss_ActAdd_EquipSkill;

        //提示
        if (rose_Boss_ActAdd > rose_Proprety.Rose_Boss_ActAdd)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("BOSS伤害加成")+"+" + (rose_Boss_ActAdd - rose_Proprety.Rose_Boss_ActAdd), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Boss_ActAdd = rose_Boss_ActAdd;

        float rose_Boss_ActAdd_ShiLi = rose_Boss_ActAdd_Equip + rose_Boss_ActAdd_EquipSuit + rose_Boss_ActAdd_JingLing + rose_Boss_ActAdd_XiLianDaShi + rose_Boss_ActAdd_EquipSkill;
        float rose_Boss_ActAdd_Base = rose_Boss_ActAdd_Equip + rose_Boss_ActAdd_EquipSuit + rose_Boss_ActAdd_TianFu + rose_Boss_ActAdd_JingLing + rose_Boss_ActAdd_XiLianDaShi + rose_Boss_ActAdd_EquipSkill;
        rose_Proprety.Rose_Boss_ActAdd_PropertySum = rose_Boss_ActAdd_Base;


        //------------------------------------Boss技能攻击加成
        rose_Boss_SkillAdd = rose_Boss_SkillAdd_Equip + rose_Boss_SkillAdd_EquipSuit + rose_Boss_SkillAdd_TianFu + rose_Boss_SkillAdd_JingLing + rose_Boss_SkillAdd_XiLianDaShi + rose_Boss_SkillAdd_EquipSkill;

        //提示
        if (rose_Boss_SkillAdd > rose_Proprety.Rose_Boss_SkillAdd)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("Boss技能攻击加成")+"+" + (rose_Boss_SkillAdd - rose_Proprety.Rose_Boss_SkillAdd), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Boss_SkillAdd = rose_Boss_SkillAdd;

        float rose_Boss_SkillAdd_ShiLi = rose_Boss_SkillAdd_Equip + rose_Boss_SkillAdd_EquipSuit + rose_Boss_SkillAdd_JingLing + rose_Boss_SkillAdd_XiLianDaShi;
        float rose_Boss_SkillAdd_Base = rose_Boss_SkillAdd_Equip + rose_Boss_SkillAdd_EquipSuit + rose_Boss_SkillAdd_TianFu + rose_Boss_SkillAdd_JingLing + rose_Boss_SkillAdd_XiLianDaShi;
        rose_Proprety.Rose_Boss_SkillAdd_PropertySum = rose_Boss_SkillAdd_Base;


        //------------------------------------受到Boss普通攻击减免
        rose_Boss_ActHitCost = rose_Boss_ActHitCost_Equip + rose_Boss_ActHitCost_EquipSuit + rose_Boss_ActHitCost_TianFu + rose_Boss_ActHitCost_JingLing + rose_Boss_ActHitCost_XiLianDaShi + rose_Boss_ActHitCost_EquipSkill + rose_Proprety.Rose_Boss_ActHitCost_Add;

        //提示
        if (rose_Boss_ActHitCost > rose_Proprety.Rose_Boss_ActHitCost)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("受到Boss普通攻击减免")+"+" + (rose_Boss_ActHitCost - rose_Proprety.Rose_Boss_ActHitCost), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Boss_ActHitCost = rose_Boss_ActHitCost;

        float rose_Boss_ActHitCost_ShiLi = rose_Boss_ActHitCost_Equip + rose_Boss_ActHitCost_EquipSuit + rose_Boss_ActHitCost_JingLing + rose_Boss_ActHitCost_XiLianDaShi + rose_Boss_ActHitCost_EquipSkill;
        float rose_Boss_ActHitCost_Base = rose_Boss_ActHitCost_Equip + rose_Boss_ActHitCost_EquipSuit + rose_Boss_ActHitCost_TianFu + rose_Boss_ActHitCost_JingLing + rose_Boss_ActHitCost_XiLianDaShi + rose_Boss_ActHitCost_EquipSkill;
        rose_Proprety.Rose_Boss_ActHitCost_PropertySum = rose_Boss_ActHitCost_Base;


        //------------------------------------受到Boss技能攻击减免
        rose_Boss_SkillHitCost = rose_Boss_SkillHitCost_Equip + rose_Boss_SkillHitCost_EquipSuit + rose_Boss_SkillHitCost_TianFu + rose_Boss_SkillHitCost_JingLing + rose_Boss_SkillHitCost_XiLianDaShi + rose_Boss_SkillHitCost_EquipSkill;

        //提示
        if (rose_Boss_SkillHitCost > rose_Proprety.Rose_Boss_SkillHitCost)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("受到Boss技能攻击减免")+"+" + (rose_Boss_SkillHitCost - rose_Proprety.Rose_Boss_SkillHitCost), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Boss_SkillHitCost = rose_Boss_SkillHitCost;

        float rose_Boss_SkillHitCost_ShiLi = rose_Boss_SkillHitCost_Equip + rose_Boss_SkillHitCost_EquipSuit + rose_Boss_SkillHitCost_JingLing + rose_Boss_SkillHitCost_XiLianDaShi + rose_Boss_SkillHitCost_EquipSkill;
        float rose_Boss_SkillHitCost_Base = rose_Boss_SkillHitCost_Equip + rose_Boss_SkillHitCost_EquipSuit + rose_Boss_SkillHitCost_TianFu + rose_Boss_SkillHitCost_JingLing + rose_Boss_SkillHitCost_XiLianDaShi + rose_Boss_SkillHitCost_EquipSkill;
        rose_Proprety.Rose_Boss_SkillHitCost_PropertySum = rose_Boss_SkillHitCost_Base;


        //------------------------------------宠物攻击加成
        rose_PetActAdd = rose_PetActAdd_Equip + rose_PetActAdd_EquipSuit + rose_PetActAdd_TianFu + rose_PetActAdd_JingLing + rose_PetActAdd_XiLianDaShi + rose_PetActAdd_EquipSkill + rose_Proprety.Rose_PetActAdd_Add;

        //提示
        if (rose_PetActAdd > rose_Proprety.Rose_PetActAdd)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("宠物攻击加成")+"+" + (rose_PetActAdd - rose_Proprety.Rose_PetActAdd), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_PetActAdd = rose_PetActAdd;

        float rose_PetActAdd_ShiLi = rose_PetActAdd_Equip + rose_PetActAdd_EquipSuit + rose_PetActAdd_JingLing + rose_PetActAdd_XiLianDaShi + rose_PetActAdd_EquipSkill;
        float rose_PetActAdd_Base = rose_PetActAdd_Equip + rose_PetActAdd_EquipSuit + rose_PetActAdd_TianFu + rose_PetActAdd_JingLing + rose_PetActAdd_XiLianDaShi + rose_PetActAdd_EquipSkill;
        rose_Proprety.Rose_PetActAdd_PropertySum = rose_PetActAdd_Base;


        //------------------------------------宠物受伤减免
        rose_PetActHitCost = rose_PetActHitCost_Equip + rose_PetActHitCost_EquipSuit + rose_PetActHitCost_TianFu + rose_PetActHitCost_JingLing + rose_PetActHitCost_XiLianDaShi + rose_PetActHitCost_EquipSkill;

        //提示
        if (rose_PetActHitCost > rose_Proprety.Rose_PetActHitCost)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("宠物受伤减免")+"+" + (rose_PetActHitCost - rose_Proprety.Rose_PetActHitCost), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_PetActHitCost = rose_PetActHitCost;

        float rose_PetActHitCost_ShiLi = rose_PetActHitCost_Equip + rose_PetActHitCost_EquipSuit + rose_PetActHitCost_JingLing + rose_PetActHitCost_XiLianDaShi + rose_PetActHitCost_EquipSkill;
        float rose_PetActHitCost_Base = rose_PetActHitCost_Equip + rose_PetActHitCost_EquipSuit + rose_PetActHitCost_TianFu + rose_PetActHitCost_JingLing + rose_PetActHitCost_XiLianDaShi + rose_PetActHitCost_EquipSkill;
        rose_Proprety.Rose_PetActHitCost_PropertySum = rose_PetActHitCost_Base;



        //------------------------------------技能冷却时间缩减
        rose_SkillCDTimePro = rose_SkillCDTimePro_Equip + rose_SkillCDTimePro_EquipSuit + rose_SkillCDTimePro_TianFu + rose_SkillCDTimePro_JingLing + rose_SkillCDTimePro_XiLianDaShi + rose_SkillCDTimePro_EquipSkill;

        //提示
        if (rose_SkillCDTimePro > rose_Proprety.Rose_SkillCDTimePro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("技能冷却时间缩减")+"+" + (rose_SkillCDTimePro - rose_Proprety.Rose_SkillCDTimePro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_SkillCDTimePro = rose_SkillCDTimePro;

        float rose_SkillCDTimePro_ShiLi = rose_SkillCDTimePro_Equip + rose_SkillCDTimePro_EquipSuit + rose_SkillCDTimePro_JingLing + rose_SkillCDTimePro_XiLianDaShi + rose_SkillCDTimePro_EquipSkill;
        float rose_SkillCDTimePro_Base = rose_SkillCDTimePro_Equip + rose_SkillCDTimePro_EquipSuit + rose_SkillCDTimePro_TianFu + rose_SkillCDTimePro_JingLing + rose_SkillCDTimePro_XiLianDaShi + rose_SkillCDTimePro_EquipSkill;
        rose_Proprety.Rose_SkillCDTimePro_PropertySum = rose_SkillCDTimePro_Base;



        //------------------------------------自身buff效果延长
        rose_BuffTimeAddPro = rose_BuffTimeAddPro_Equip + rose_BuffTimeAddPro_EquipSuit + rose_BuffTimeAddPro_TianFu + rose_BuffTimeAddPro_JingLing + rose_BuffTimeAddPro_XiLianDaShi + rose_BuffTimeAddPro_EquipSkill;

        //提示
        if (rose_BuffTimeAddPro > rose_Proprety.Rose_BuffTimeAddPro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("自身buff效果延长")+"+" + (rose_BuffTimeAddPro - rose_Proprety.Rose_BuffTimeAddPro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_BuffTimeAddPro = rose_BuffTimeAddPro;

        float rose_BuffTimeAddPro_ShiLi = rose_BuffTimeAddPro_Equip + rose_BuffTimeAddPro_EquipSuit + rose_BuffTimeAddPro_JingLing + rose_BuffTimeAddPro_XiLianDaShi + rose_BuffTimeAddPro_EquipSkill;
        float rose_BuffTimeAddPro_Base = rose_BuffTimeAddPro_Equip + rose_BuffTimeAddPro_EquipSuit + rose_BuffTimeAddPro_TianFu + rose_BuffTimeAddPro_JingLing + rose_BuffTimeAddPro_XiLianDaShi + rose_BuffTimeAddPro_EquipSkill;

        rose_Proprety.Rose_BuffTimeAddPro_PropertySum = rose_BuffTimeAddPro_Base;



        //------------------------------------Debuff时间缩短
        rose_DeBuffTimeCostPro = rose_DeBuffTimeCostPro_Equip + rose_DeBuffTimeCostPro_EquipSuit + rose_DeBuffTimeCostPro_TianFu + rose_DeBuffTimeCostPro_JingLing + rose_DeBuffTimeCostPro_XiLianDaShi + rose_DeBuffTimeCostPro_EquipSkill;

        //提示
        if (rose_DeBuffTimeCostPro > rose_Proprety.Rose_DeBuffTimeCostPro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("Debuff时间缩短")+"+" + (rose_DeBuffTimeCostPro - rose_Proprety.Rose_DeBuffTimeCostPro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_DeBuffTimeCostPro = rose_DeBuffTimeCostPro;

        float rose_DeBuffTimeCostPro_ShiLi = rose_DeBuffTimeCostPro_Equip + rose_DeBuffTimeCostPro_EquipSuit + rose_DeBuffTimeCostPro_JingLing + rose_DeBuffTimeCostPro_XiLianDaShi + rose_DeBuffTimeCostPro_EquipSkill;
        float rose_DeBuffTimeCostPro_Base = rose_DeBuffTimeCostPro_Equip + rose_DeBuffTimeCostPro_EquipSuit + rose_DeBuffTimeCostPro_TianFu + rose_DeBuffTimeCostPro_JingLing + rose_DeBuffTimeCostPro_XiLianDaShi + rose_DeBuffTimeCostPro_EquipSkill;
        rose_Proprety.Rose_DeBuffTimeCostPro_PropertySum = rose_DeBuffTimeCostPro_Base;



        //------------------------------------闪避恢复血量
        rose_DodgeAddHpPro = rose_DodgeAddHpPro_Equip + rose_DodgeAddHpPro_EquipSuit + rose_DodgeAddHpPro_TianFu + rose_DodgeAddHpPro_JingLing + rose_DodgeAddHpPro_XiLianDaShi + rose_DodgeAddHpPro_EquipSkill + rose_Proprety.Rose_DodgeAddHpPro_Add;

        //提示
        if (rose_DodgeAddHpPro > rose_Proprety.Rose_DodgeAddHpPro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("闪避恢复血量")+"+" + (rose_DodgeAddHpPro - rose_Proprety.Rose_DodgeAddHpPro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_DodgeAddHpPro = rose_DodgeAddHpPro;

        float rose_DodgeAddHpPro_ShiLi = rose_DodgeAddHpPro_Equip + rose_DodgeAddHpPro_EquipSuit + rose_DodgeAddHpPro_JingLing + rose_DodgeAddHpPro_XiLianDaShi + rose_DodgeAddHpPro_EquipSkill;
        float rose_DodgeAddHpPro_Base = rose_DodgeAddHpPro_Equip + rose_DodgeAddHpPro_EquipSuit + rose_DodgeAddHpPro_TianFu + rose_DodgeAddHpPro_JingLing + rose_DodgeAddHpPro_XiLianDaShi + rose_DodgeAddHpPro_EquipSkill;
        rose_Proprety.Rose_DodgeAddHpPro_PropertySum = rose_DodgeAddHpPro_Base;






        //------------------------------------经验加成
        rose_Exp_AddPro = exp_AddPro_Equip + exp_AddPro_EquipSuit + exp_AddPro_TianFu + exp_AddPro_JingLing + exp_AddPro_XiLianDaShi + exp_AddPro_EquipSkill + rose_Proprety.Rose_Exp_AddPro_Add;

        //提示
        if (rose_Exp_AddPro > rose_Proprety.Rose_Exp_AddPro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("经验加成")+"+" + (rose_Exp_AddPro - rose_Proprety.Rose_Exp_AddPro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Exp_AddPro = rose_Exp_AddPro;


        //------------------------------------经验固定加成
        rose_Exp_AddValue = exp_AddValue_Equip + exp_AddValue_EquipSuit + exp_AddValue_TianFu + exp_AddValue_JingLing + exp_AddValue_XiLianDaShi + exp_AddValue_EquipSkill;

        //提示
        if (rose_Exp_AddValue > rose_Proprety.Rose_Exp_AddValue)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("经验加成")+"+" + (rose_Exp_AddValue - rose_Proprety.Rose_Exp_AddValue), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Exp_AddValue = rose_Exp_AddValue;



        //------------------------------------金币加成
        rose_Gold_AddPro = gold_AddPro_Equip + gold_AddPro_EquipSuit + gold_AddPro_TianFu + gold_AddPro_JingLing + gold_AddPro_XiLianDaShi + gold_AddPro_EquipSkill + rose_Proprety.Rose_Gold_AddPro_Add;

        //提示
        if (rose_Gold_AddPro > rose_Proprety.Rose_Gold_AddPro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("金币加成")+"+" + (rose_Gold_AddPro - rose_Proprety.Rose_Gold_AddPro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Gold_AddPro = rose_Gold_AddPro;


        //------------------------------------金币固定加成
        rose_Gold_AddValue = gold_AddValue_Equip + gold_AddValue_EquipSuit + gold_AddValue_TianFu + gold_AddValue_JingLing + gold_AddValue_XiLianDaShi + gold_AddValue_EquipSkill + rose_Proprety.Rose_Gold_AddValue_Add;

        //提示
        if (rose_Gold_AddValue > rose_Proprety.Rose_Gold_AddValue)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("金币加成")+"+" + (rose_Gold_AddValue - rose_Proprety.Rose_Gold_AddValue), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Gold_AddValue = rose_Gold_AddValue;


        //------------------------------------洗炼极品掉落
        rose_Blessing_AddPro = blessing_AddPro_Equip + blessing_AddPro_EquipSuit + blessing_AddPro_TianFu + blessing_AddPro_JingLing + blessing_AddPro_XiLianDaShi + blessing_AddPro_EquipSkill + rose_Proprety.Rose_Blessing_AddPro_Add;

        //提示
        if (rose_Blessing_AddPro > rose_Proprety.Rose_Blessing_AddPro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("洗炼极品掉落") + "+" + (rose_Blessing_AddPro - rose_Proprety.Rose_Blessing_AddPro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Blessing_AddPro = rose_Blessing_AddPro;
        //Debug.Log("rose_Proprety.Rose_Blessing_AddPro  = " + rose_Proprety.Rose_Blessing_AddPro);

        //------------------------------------装备隐藏属性出现概率
        rose_HidePro_AddPro = hidePro_AddPro_Equip + hidePro_AddPro_EquipSuit + hidePro_AddPro_TianFu + hidePro_AddPro_JingLing + hidePro_AddPro_XiLianDaShi + hidePro_AddPro_EquipSkill + rose_Proprety.Rose_HidePro_AddPro_Add;

        //提示
        if (rose_HidePro_AddPro > rose_Proprety.Rose_HidePro_AddPro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("装备隐藏属性出现概率")+"+" + (rose_HidePro_AddPro - rose_Proprety.Rose_HidePro_AddPro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_HidePro_AddPro = rose_HidePro_AddPro;
        //Debug.Log("rose_Proprety.Rose_HidePro_AddPro  = " + rose_Proprety.Rose_HidePro_AddPro);

        //------------------------------------装备上的宝石槽位出现概率
        rose_GemHole_AddPro = gemHole_AddPro_Equip + gemHole_AddPro_EquipSuit + gemHole_AddPro_TianFu + gemHole_AddPro_JingLing + gemHole_AddPro_XiLianDaShi + gemHole_AddPro_EquipSkill + rose_Proprety.Rose_GemHole_AddPro_Add;

        //提示
        if (rose_GemHole_AddPro > rose_Proprety.Rose_GemHole_AddPro)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("装备上的宝石槽位出现概率") +"+" + (rose_GemHole_AddPro - rose_Proprety.Rose_GemHole_AddPro), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_GemHole_AddPro = rose_GemHole_AddPro;


        //------------------------------------药剂熟练固定加成
        rose_YaoJiValue = rose_YaoJiValue_Equip + rose_YaoJiValue_EquipSuit + rose_YaoJiValue_TianFu + rose_YaoJiValue_JingLing + rose_YaoJiValue_XiLianDaShi + rose_YaoJiValue_EquipSkill + rose_Proprety.Rose_YaoJiShuLian_Value_Add;

        //提示
        if (rose_YaoJiValue > rose_Proprety.Rose_YaoJiShuLian_Value)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("药剂熟练度增加")+"+" + (rose_YaoJiValue - rose_Proprety.Rose_YaoJiShuLian_Value), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_YaoJiShuLian_Value = rose_YaoJiValue;


        //------------------------------------锻造熟练固定加成
        rose_DuanZaoValue = rose_DuanZaoValue_Equip + rose_DuanZaoValue_EquipSuit + rose_DuanZaoValue_TianFu + rose_DuanZaoValue_JingLing + rose_DuanZaoValue_XiLianDaShi + rose_DuanZaoValue_EquipSkill + rose_Proprety.Rose_DuanZuaoShuLian_Value_Add;

        //提示
        if (rose_DuanZaoValue > rose_Proprety.Rose_DuanZuaoShuLian_Value)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint(Game_PublicClassVar.gameSettingLanguge.LoadLocalization("锻造熟练度增加")+"+"+ (rose_DuanZaoValue - rose_Proprety.Rose_DuanZuaoShuLian_Value), "7FFF00FF");
            }
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_DuanZuaoShuLian_Value = rose_DuanZaoValue;



		//------------------------------------复活概率
        rose_FuHuoPro = rose_FuHuoPro_Equip + rose_FuHuoPro_EquipSuit + rose_FuHuoPro_TianFu + rose_FuHuoPro_JingLing + rose_FuHuoPro_XiLianDaShi + FuHuoPro_EquipSkill + rose_Proprety.Rose_FuHuoPro_Add;

		//最大值限制
		if (rose_FuHuoPro > 0.2f) {
			rose_FuHuoPro = 0.2f;
		}

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_FuHuoPro = rose_FuHuoPro;

		//------------------------------------攻击无视防御
        rose_ActWuShi = rose_ActWuShi_Equip + rose_ActWuShi_EquipSuit + rose_ActWuShi_TianFu + rose_ActWuShi_JingLing + rose_ActWuShi_XiLianDaShi + ActWuShi_EquipSkill + rose_Proprety.Rose_ActWuShi_Add;

		//最大值限制
		if (rose_ActWuShi > 0.2f) {
			rose_ActWuShi = 0.2f;
		}
		//结算出结果后进行属性赋值
        rose_Proprety.Rose_ActWuShi = rose_ActWuShi;

		//------------------------------------神农
        rose_ShenNong = rose_ShenNong_Equip + rose_ShenNong_EquipSuit + rose_ShenNong_TianFu + rose_ShenNong_JingLing + rose_ShenNong_XiLianDaShi + ShenNong_EquipSkill + rose_Proprety.Rose_ShenNong_Add;

		//最大值限制
		if (rose_ShenNong > 1f) {
			rose_ShenNong = 1f;
		}
		//结算出结果后进行属性赋值
        rose_Proprety.Rose_ShenNong = rose_ShenNong;

		//------------------------------------额外掉落
        rose_DropExtra = rose_DropExtra_Equip + rose_DropExtra_EquipSuit + rose_DropExtra_TianFu + rose_DropExtra_JingLing + rose_DropExtra_XiLianDaShi + rose_DropExtra + rose_Proprety.Rose_DropExtra_Add;

		//最大值限制
		if (rose_DropExtra > 1f) {
			rose_DropExtra = 1f;
		}

		//结算出结果后进行属性赋值
        rose_Proprety.Rose_DropExtra = rose_DropExtra;

		//------------------------------------伪装  +增大发现范围   -缩小范围
        rose_WeiZhuang = rose_WeiZhuang_Equip + rose_WeiZhuang_EquipSuit + rose_WeiZhuang_TianFu + rose_WeiZhuang_JingLing + rose_WeiZhuang_XiLianDaShi + WeiZhuang_EquipSkill + rose_Proprety.Rose_WeiZhuang_Add;

		//最大值限制
		if (rose_WeiZhuang > 5) {
			rose_WeiZhuang = 5;
		}

		//结算出结果后进行属性赋值
        rose_Proprety.Rose_WeiZhuang = rose_WeiZhuang;

		//------------------------------------灾难
        rose_ZaiNanValue = rose_ZaiNanValue_Equip + rose_ZaiNanValue_EquipSuit + rose_ZaiNanValue_TianFu + rose_ZaiNanValue_JingLing + rose_ZaiNanValue_XiLianDaShi + ZaiNanValue_EquipSkill + rose_Proprety.Rose_ZaiNanValue_Add;

		//最大值限制
		if (rose_ZaiNanValue > 1f) {
			rose_ZaiNanValue = 1f;
		}

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_ZaiNanValue = rose_ZaiNanValue;

		//------------------------------------嗜血
        rose_ShiXuePro = rose_ShiXuePro_Equip + rose_ShiXuePro_EquipSuit + rose_ShiXuePro_TianFu + rose_ShiXuePro_JingLing + rose_ShiXuePro_XiLianDaShi + ShiXuePro_EquipSkill + rose_Proprety.Rose_ShiXuePro_Add;

		//最大值限制
		if (rose_ShiXuePro > 0.2f) {
			rose_ShiXuePro = 0.2f;
		}

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_ShiXuePro = rose_ShiXuePro;


        //------------------------------------怪物脱战距离
        rose_AITuoZhanDisValue = rose_AITuoZhanDisValue_Equip + rose_AITuoZhanDisValue_EquipSuit + rose_AITuoZhanDisValue_TianFu + rose_AITuoZhanDisValue_JingLing + rose_AITuoZhanDisValue_XiLianDaShi + rose_AITuoZhanDisValue_EquipSkill + rose_Proprety.Rose_YaoJiShuLian_Value_Add;

        //最大值限制
        if (rose_AITuoZhanDisValue > 5.0f)
        {
            rose_AITuoZhanDisValue = 5.0f;
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_AITuoZhanDisValue = rose_AITuoZhanDisValue;

        //------------------------------------专注概率
        rose_ZhuanZhuPro = rose_ZhuanZhuPro_Equip + rose_ZhuanZhuPro_EquipSuit + rose_ZhuanZhuPro_TianFu + rose_ZhuanZhuPro_JingLing + rose_ZhuanZhuPro_XiLianDaShi + rose_ZhuanZhuPro_EquipSkill + rose_Proprety.Rose_YaoJiShuLian_Value_Add;

        //最大值限制
        if (rose_ZhuanZhuPro > 1f)
        {
            rose_ZhuanZhuPro = 1f;
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_ZhuanZhuPro = rose_ZhuanZhuPro;

        //------------------------------------怪物脱战距离
        rose_BiZhongPro = rose_BiZhongPro_Equip + rose_BiZhongPro_EquipSuit + rose_BiZhongPro_TianFu + rose_BiZhongPro_JingLing + rose_BiZhongPro_XiLianDaShi + rose_BiZhongPro_EquipSkill + rose_Proprety.Rose_YaoJiShuLian_Value_Add;

        //最大值限制
        if (rose_BiZhongPro > 1f)
        {
            rose_BiZhongPro = 1f;
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_BiZhongPro = rose_BiZhongPro;

        //------------------------------------生产药剂暴击概率
        rose_YaoJiCirPro = rose_YaoJiCirPro_Equip + rose_YaoJiCirPro_EquipSuit + rose_YaoJiCirPro_TianFu + rose_YaoJiCirPro_JingLing + rose_YaoJiCirPro_XiLianDaShi + rose_YaoJiCirPro_EquipSkill + rose_Proprety.Rose_YaoJiShuLian_Value_Add;

        //最大值限制
        if (rose_YaoJiCirPro > 1f)
        {
            rose_YaoJiCirPro = 1f;
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_YaoJiCirPro = rose_YaoJiCirPro;

        //------------------------------------捕捉概率
        rose_BuZhuoPro = rose_BuZhuoPro_Equip + rose_BuZhuoPro_EquipSuit + rose_BuZhuoPro_TianFu + rose_BuZhuoPro_JingLing + rose_BuZhuoPro_XiLianDaShi + rose_BuZhuoPro_EquipSkill + rose_Proprety.Rose_YaoJiShuLian_Value_Add;

        //最大值限制
        if (rose_BuZhuoPro > 1f)
        {
            rose_BuZhuoPro = 1f;
        }

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_BuZhuoPro = rose_BuZhuoPro;

        //------------------------------------魔法上限
        rose_LanValueMaxAdd = rose_LanValueMaxAdd_Equip + rose_LanValueMaxAdd_EquipSuit + rose_LanValueMaxAdd_TianFu + rose_LanValueMaxAdd_JingLing + rose_LanValueMaxAdd_XiLianDaShi + rose_LanValueMaxAdd_EquipSkill + rose_Proprety.Rose_YaoJiShuLian_Value_Add;


        //结算出结果后进行属性赋值
        rose_Proprety.Rose_LanValueMaxAdd_PropertySum = rose_LanValueMaxAdd;

        //------------------------------------召唤属性上限
        rose_SummonAIPropertyAddPro = rose_SummonAIPropertyAddPro_Equip + rose_SummonAIPropertyAddPro_EquipSuit + rose_SummonAIPropertyAddPro_TianFu + rose_SummonAIPropertyAddPro_JingLing + rose_SummonAIPropertyAddPro_XiLianDaShi + rose_SummonAIPropertyAddPro_EquipSkill + rose_Proprety.Rose_YaoJiShuLian_Value_Add;

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_SummonAIPropertyAddPro_PropertySum = rose_SummonAIPropertyAddPro;

        //其他部位属性（目前只存在天赋 后期需要需要补充）
        rose_SummonAIHpPropertyAddPro = rose_SummonAIHpPropertyAddPro_TianFu;
        rose_Proprety.Rose_SummonAIHpPropertyAddPro_PropertySum = rose_SummonAIHpPropertyAddPro;
        rose_SummonAIActPropertyAddPro = rose_SummonAIActPropertyAddPro_TianFu;
        rose_Proprety.Rose_SummonAIActPropertyAddPro_PropertySum = rose_SummonAIActPropertyAddPro;
        rose_SummonAIDefPropertyAddPro = rose_SummonAIDefPropertyAddPro_TianFu;
        rose_Proprety.Rose_SummonAIDefPropertyAddPro_PropertySum = rose_SummonAIDefPropertyAddPro;

        //------------------------------------护盾加成
        rose_HuDunValueAddPro = rose_HuDunValueAddPro_Equip + rose_HuDunValueAddPro_EquipSuit + rose_HuDunValueAddPro_TianFu + rose_HuDunValueAddPro_JingLing + rose_HuDunValueAddPro_XiLianDaShi + rose_HuDunValueAddPro_EquipSkill + rose_Proprety.Rose_YaoJiShuLian_Value_Add;

        //结算出结果后进行属性赋值
        rose_Proprety.Rose_HuDunValueAddPro_PropertySum = rose_HuDunValueAddPro;

        //-----------------------------------普通攻击加成
        rose_Proprety.Rose_ActAddPro_PropertySum = rose_ActAddPro_XiLianDaShi;

        //**********计算战力*********
        //攻击部分
        //Debug.Log("rose_ActAddPro_XiLianDaShi = " + rose_ActAddPro_XiLianDaShi);
        float ShiLi_GongJi = rose_ActMin_ShiLi + rose_ActMax_ShiLi + rose_MagActMin_ShiLi + rose_MagActMax_ShiLi;
		float ShiLi_GongJi_Pro = rose_Hit_ShiLi + rose_Cri_ShiLi + rose_DamgeAdd_ShiLi + (rose_ZhongJiPro_ShiLi + rose_HuShiDefValuePro_ShiLi + rose_HuShiAdfValuePro_ShiLi + rose_XiXuePro_ShiLi) * 0.5f + (rose_RaceDamge_1_ShiLi + rose_RaceDamge_2_ShiLi + rose_RaceDamge_3_ShiLi) * 0.3f + rose_ActAddPro_XiLianDaShi * 0.15f;

        //防御部分
        float ShiLi_FangYu = rose_Hp_ShiLi * 0.3f + rose_DefMin_ShiLi + rose_DefMax_ShiLi + rose_AdfMin_ShiLi + rose_AdfMax_ShiLi + rose_HealHpValue_ShiLi;
		float ShiLi_FangYu_Pro = rose_Dodge_ShiLi + rose_Resilience_ShiLi + rose_DamgeSub_ShiLi + (rose_MagicRebound_ShiLi + rose_ActRebound_ShiLi + rose_HealHpFightPro_ShiLi * 0.0025f + rose_HealHpPro_ShiLi * 0.0025f + rose_AdfAdd_ShiLi + rose_DefAdd_ShiLi) * 0.5f + (rose_Resistance_1_ShiLi + rose_Resistance_2_ShiLi + rose_Resistance_3_ShiLi + rose_Resistance_4_ShiLi + rose_Resistance_5_ShiLi + rose_RaceResistance_1_ShiLi + rose_RaceResistance_2_ShiLi + rose_RaceResistance_3_ShiLi) * 0.3f;

		//其他部分
		float ShiLi_QiTa = (rose_HitRating_ShiLi + rose_DodgeRating_ShiLi + rose_CriRating_ShiLi + rose_ResRating_ShiLi)* 0.25f + rose_GeDangValue_ShiLi + rose_ZhongJiValue_ShiLi + rose_GuDingValue_ShiLi + rose_HuShiDefValue_ShiLi + rose_HuShiAdfValue_ShiLi + rose_HealHpValue * 3;

		//幸运部分
		float Shili_Lucky = rose_Lucky_ShiLi * 0.25f / (10 - rose_Lucky_ShiLi + 4);
		if (Shili_Lucky <= 0)
		{
			Shili_Lucky = 0;
		}

        //觉醒部分
        string juexingSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] juexingList = juexingSet.Split(';');
        int ShiLi_JueXing = 0;
        for (int i = 0; i < juexingList.Length; i++) {
            if (juexingList[i] != "" && juexingList[i] != null) {
                int addValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddShiLi", "ID", juexingList[i], "JueXing_Template"));
                ShiLi_JueXing = ShiLi_JueXing + addValue;
            }
        }

        //战力总和计算(加入等级系数)
        ObscuredInt ShiLiSum = (int)(ShiLi_GongJi * (1 + ShiLi_GongJi_Pro + Shili_Lucky) + ShiLi_FangYu * (1 + ShiLi_FangYu_Pro) + ShiLi_QiTa + rose_Lv * 75 + rose_Lv * GetLvShiLiAddPro()) + ShiLi_JueXing;

		//发送信息
		if (ShiLiSum >= rose_Proprety.Rose_ShiLiValue) {
			string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            string sendShiLi = Game_PublicClassVar.Get_xmlScript.Encrypt(ShiLiSum.ToString());
            string[] sendStrList = new string[] { zhanghaoID, sendShiLi.ToString(), "", "" };
			Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001030, sendStrList);

			//获取新战区时间
			//Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001034, zhanghaoID);
		}

		rose_Proprety.Rose_ShiLiValue = ShiLiSum;

        //调整完属性时,更新当前角色属性
        Game_PublicClassVar.Get_game_PositionVar.UpdataRoseBuffProperty = true;

        return true;
		
	}


    //更新角色的buff属性
    public void UpdateRoseBuffProperty() {

        Rose_Proprety roseProprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        roseProprety.Rose_Hp = (int)(roseProprety.Rose_Hp_PropertySum * (1 + roseProprety.Rose_HpMul_1) + roseProprety.Rose_HpAdd_2);
        roseProprety.Rose_ActMin = (int)(roseProprety.Rose_ActMin_PropertySum * (1 + roseProprety.Rose_ActMinMul_1) + roseProprety.Rose_ActMinAdd_2);
        roseProprety.Rose_ActMax = (int)(roseProprety.Rose_ActMax_PropertySum * (1 + roseProprety.Rose_ActMaxMul_1) + roseProprety.Rose_ActMaxAdd_2);
        roseProprety.Rose_MagActMax = (int)(roseProprety.Rose_MagActMax_PropertySum * (1 + roseProprety.Rose_MagActMaxMul_1) + roseProprety.Rose_MagActMaxAdd_2);
        roseProprety.Rose_MagActMax = (int)(roseProprety.Rose_MagActMax_PropertySum * (1 + roseProprety.Rose_MagActMaxMul_1) + roseProprety.Rose_MagActMaxAdd_2);
        roseProprety.Rose_DefMin = (int)(roseProprety.Rose_DefMin_PropertySum * (1 + roseProprety.Rose_DefMinMul_1) + roseProprety.Rose_DefMinAdd_2);
        roseProprety.Rose_DefMax = (int)(roseProprety.Rose_DefMax_PropertySum * (1 + roseProprety.Rose_DefMaxMul_1) + roseProprety.Rose_DefMaxAdd_2);
        roseProprety.Rose_AdfMin = (int)(roseProprety.Rose_AdfMin_PropertySum * (1 + roseProprety.Rose_AdfMinMul_1) + roseProprety.Rose_AdfMinAdd_2);
        roseProprety.Rose_AdfMax = (int)(roseProprety.Rose_AdfMax_PropertySum * (1 + roseProprety.Rose_AdfMaxMul_1) + roseProprety.Rose_AdfMaxAdd_2);

        roseProprety.Rose_MoveSpeed = roseProprety.Rose_MoveSpeed_PropertySum * (1 + roseProprety.Rose_MoveSpeedMul_1) + roseProprety.Rose_MoveSpeedAdd_2;
        roseProprety.rose_LastMoveSpeed = roseProprety.Rose_MoveSpeed;
        roseProprety.Rose_Cri = roseProprety.Rose_Cri_PropertySum + roseProprety.Rose_CriRating_Add + roseProprety.Rose_CriMul_1;
        roseProprety.Rose_Hit = roseProprety.Rose_Hit_PropertySum + roseProprety.Rose_HitRating_Add + roseProprety.Rose_HitMul_1;
        roseProprety.Rose_Dodge = roseProprety.Rose_Dodge_PropertySum + roseProprety.Rose_DodgeRating_Add + roseProprety.Rose_DodgeMul_1;
        roseProprety.Rose_GeDangValue = roseProprety.Rose_GeDangValue_PropertySum + roseProprety.Rose_GeDangValue_Add;
        roseProprety.Rose_ZhongJiPro = roseProprety.Rose_ZhongJiPro_PropertySum + roseProprety.Rose_ZhongJiPro_Add;
        roseProprety.Rose_ZhongJiValue = roseProprety.Rose_ZhongJiValue_PropertySum + roseProprety.Rose_ZhongJiValue_Add;
        roseProprety.Rose_GuDingValue = roseProprety.Rose_GuDingValue_PropertySum + roseProprety.Rose_GuDingValue_Add;
        roseProprety.Rose_HuShiDefValue = roseProprety.Rose_HuShiDefValue_PropertySum + roseProprety.Rose_HuShiDefValue_Add;
        roseProprety.Rose_HuShiAdfValue = roseProprety.Rose_HuShiAdfValue_PropertySum + roseProprety.Rose_HuShiAdfValue_Add;
        roseProprety.Rose_HuShiDefValuePro = roseProprety.Rose_HuShiDefValuePro_PropertySum + roseProprety.Rose_HuShiDefValuePro_Add;
        roseProprety.Rose_HuShiAdfValuePro = roseProprety.Rose_HuShiAdfValuePro_PropertySum + roseProprety.Rose_HuShiAdfValuePro_Add;
        roseProprety.Rose_XiXuePro = roseProprety.Rose_XiXuePro_PropertySum + roseProprety.Rose_XiXuePro_Add;
        //Debug.Log("吸血Rose_XiXuePro_PropertySum = " + roseProprety.Rose_XiXuePro_PropertySum + "roseProprety.Rose_XiXuePro_Add = " + roseProprety.Rose_XiXuePro_Add);
        roseProprety.Rose_DefAdd = roseProprety.Rose_DefAdd_PropertySum + roseProprety.Rose_DefMul_1;
        roseProprety.Rose_AdfAdd = roseProprety.Rose_AdfAdd_PropertySum + roseProprety.Rose_AdfMul_1;
        roseProprety.Rose_DamgeSub = roseProprety.Rose_DamgeSub_PropertySum + roseProprety.Rose_DamgeSubtractMul_1;
        roseProprety.Rose_DamgeAdd = roseProprety.Rose_DamgeAdd_PropertySum + roseProprety.Rose_DamgeAddMul_1;
        roseProprety.Rose_Lucky = roseProprety.Rose_Lucky_PropertySum + roseProprety.Rose_Luck_Add;
        roseProprety.Rose_Res = roseProprety.Rose_Res_PropertySum + roseProprety.Rose_ResilienceRatingMul_1;
        roseProprety.Rose_MagicRebound = roseProprety.Rose_MagicRebound_PropertySum + roseProprety.Rose_MagicRebound_Add;
        roseProprety.Rose_ActRebound = roseProprety.Rose_ActRebound_PropertySum + roseProprety.Rose_ActRebound_Add;
        roseProprety.Rose_HealHpValue = roseProprety.Rose_HealHpValue_PropertySum + roseProprety.Rose_HealHpValue_Add;
        roseProprety.Rose_HealHpPro = roseProprety.Rose_HealHpPro_PropertySum + roseProprety.Rose_HealHpPro_Add;
        roseProprety.Rose_HealHpFightPro = roseProprety.Rose_HealHpFightPro_PropertySum + roseProprety.Rose_HealHpFightPro_Add;
        roseProprety.Rose_Resistance_1 = roseProprety.Rose_Resistance_1_PropertySum + roseProprety.Rose_Resistance_1_Add;
        roseProprety.Rose_Resistance_2 = roseProprety.Rose_Resistance_2_PropertySum + roseProprety.Rose_Resistance_2_Add;
        roseProprety.Rose_Resistance_3 = roseProprety.Rose_Resistance_3_PropertySum + roseProprety.Rose_Resistance_3_Add;
        roseProprety.Rose_Resistance_4 = roseProprety.Rose_Resistance_4_PropertySum + roseProprety.Rose_Resistance_4_Add;
        roseProprety.Rose_Resistance_5 = roseProprety.Rose_Resistance_5_PropertySum + roseProprety.Rose_Resistance_5_Add;
        roseProprety.Rose_RaceResistance_1 = roseProprety.Rose_RaceResistance_1_PropertySum + roseProprety.Rose_RaceResistance_1_Add;
        roseProprety.Rose_RaceResistance_2 = roseProprety.Rose_RaceResistance_2_PropertySum + roseProprety.Rose_RaceResistance_2_Add;
        roseProprety.Rose_RaceResistance_3 = roseProprety.Rose_RaceResistance_3_PropertySum + roseProprety.Rose_RaceResistance_3_Add;
        roseProprety.Rose_RaceDamge_1 = roseProprety.Rose_RaceDamge_1_PropertySum + roseProprety.Rose_RaceDamge_1_Add;
        roseProprety.Rose_RaceDamge_2 = roseProprety.Rose_RaceDamge_2_PropertySum + roseProprety.Rose_RaceDamge_2_Add;
        roseProprety.Rose_RaceDamge_3 = roseProprety.Rose_RaceDamge_3_PropertySum + roseProprety.Rose_RaceDamge_3_Add;
        roseProprety.Rose_Boss_ActAdd = roseProprety.Rose_Boss_ActAdd_PropertySum + roseProprety.Rose_Boss_ActAdd_Add;
        roseProprety.Rose_Boss_SkillAdd = roseProprety.Rose_Boss_SkillAdd_PropertySum + roseProprety.Rose_Boss_SkillAdd_Add;
        roseProprety.Rose_Boss_ActHitCost = roseProprety.Rose_Boss_ActHitCost_PropertySum + roseProprety.Rose_Boss_ActHitCost_Add;
        roseProprety.Rose_Boss_SkillHitCost = roseProprety.Rose_Boss_SkillHitCost_PropertySum + roseProprety.Rose_Boss_SkillHitCost_Add;
        roseProprety.Rose_PetActAdd = roseProprety.Rose_PetActAdd_PropertySum + roseProprety.Rose_PetActAdd_Add;
        roseProprety.Rose_PetActHitCost = roseProprety.Rose_PetActHitCost_PropertySum + roseProprety.Rose_PetActHitCost_Add;
        roseProprety.Rose_SkillCDTimePro = roseProprety.Rose_SkillCDTimePro_PropertySum + roseProprety.Rose_SkillCDTimePro_Add;
        roseProprety.Rose_BuffTimeAddPro = roseProprety.Rose_BuffTimeAddPro_PropertySum + roseProprety.Rose_BuffTimeAddPro_Add;
        roseProprety.Rose_DeBuffTimeCostPro = roseProprety.Rose_DeBuffTimeCostPro_PropertySum + roseProprety.Rose_DeBuffTimeCostPro_Add;
        roseProprety.Rose_DodgeAddHpPro = roseProprety.Rose_DodgeAddHpPro_PropertySum + roseProprety.Rose_DodgeAddHpPro_Add;

        int LanMaxValue = 100;
        if (roseProprety.Rose_Occupation == "2")
        {
            LanMaxValue = 100;
        }
        if (roseProprety.Rose_Occupation == "3")
        {
            LanMaxValue = 5;
        }

        roseProprety.Rose_LanValueMax = LanMaxValue + (int)(roseProprety.Rose_LanValueMaxAdd_PropertySum);
        roseProprety.Rose_SummonAIPropertyAddPro = roseProprety.Rose_SummonAIPropertyAddPro_PropertySum;
        roseProprety.Rose_HuDunValueAddPro = roseProprety.Rose_HuDunValueAddPro_PropertySum;
        roseProprety.Rose_ActAddPro = roseProprety.Rose_ActAddPro_PropertySum;

        roseProprety.Rose_SummonAIHpPropertyAddPro = roseProprety.Rose_SummonAIHpPropertyAddPro_PropertySum;
        roseProprety.Rose_SummonAIActPropertyAddPro = roseProprety.Rose_SummonAIActPropertyAddPro_PropertySum;
        roseProprety.Rose_SummonAIDefPropertyAddPro = roseProprety.Rose_SummonAIDefPropertyAddPro_PropertySum;

        roseProprety.Rose_ChouHenValue = roseProprety.Rose_ChouHenValue_Add;
    }

    //更新附加技能效果
    public void SkillAddValue(string addStr) {

        if (addStr == "" && addStr == "0") { 
            return;
        }

        //Debug.Log("addStr =" + addStr);
        string[] addSkillList = addStr.Split(',');
        if (addSkillList.Length >= 3)
        {
            Rose_Status Rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
            Rose_Status.SkillIAddValue skillAddValue = new Rose_Status.SkillIAddValue();
            skillAddValue.SkillID = addSkillList[0];
            skillAddValue.AddType = addSkillList[1];
            skillAddValue.AddValue = addSkillList[2];

            //添加数据
            Rose_Status.Rose_SkillIAddValueList.Add(skillAddValue);
        }
        else {
            Debug.Log("天赋数据错误:" + addStr);
        }

    }

    //清空附加效果
    public void SkilllAddValueClean() {
        Rose_Status Rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        Rose_Status.Rose_SkillIAddValueList.Clear();
    }


    //增加角色固定生命值（参数一：增加血量  参数二：1.技能加血 2.药物加血）
	public bool addRoseHp(int addHp,string addType = "1",bool ifPlayFly = true,bool ifdeathAdd = true) {

        Game_PositionVar game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        //玩家死亡后不能通过光坏技能回血
        if (game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatus) {
            if (ifdeathAdd == false) {
                return false;
            }
        }

        //神农增加额外属性
        ObscuredInt pro_1 = 1;
		if (addType == "2") {
			addHp = (int)(addHp * (pro_1 + rose_Proprety.Rose_ShenNong));
		}
        rose_Proprety.Rose_HpNow = rose_Proprety.Rose_HpNow + addHp;
        //防止过量加血
        if (rose_Proprety.Rose_HpNow >= rose_Proprety.Rose_Hp) {
            rose_Proprety.Rose_HpNow = rose_Proprety.Rose_Hp;
        }
        //game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseIfHit = true;

        if (addHp != 0) {
            //飘字
            if (ifPlayFly)
            {
                Game_PublicClassVar.Get_function_UI.Fight_FlyText("1", addHp.ToString(), "1", game_PositionVar.Obj_Rose, "", "");
            }
        }

        return true;
    }


    //减少角色固定生命值
    public bool costRoseHp(int costHp)
    {

        Game_PositionVar game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        int costValue = rose_Proprety.Rose_HpNow - costHp;
        //防止扣血过量
        if (costValue <= 0)
        {
            rose_Proprety.Rose_HpNow = 0;
        }
        else {
            rose_Proprety.Rose_HpNow = costValue;
        }
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseIfHit = true;
        //飘字
        Game_PublicClassVar.Get_function_UI.Fight_FlyText("1", costHp.ToString(), "0", game_PositionVar.Obj_Rose, "", "");
        return true;
    }

    //增加角色当前故事模式状态值
    public void UpdataRoseStoryStatus() {

        string roseStoryStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //roseStoryStatus = roseStoryStatus + 1;
        //获取下一级
        string nextStoryID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextStoryID", "ID", roseStoryStatus, "GameStory_Template");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("StoryStatus", nextStoryID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //Game_PublicClassVar.Get_function_DataSet
        
    }

    //传入值更新当前关卡
    public void UpdataPVEChapter(string pveChapter)
    {
        //Debug.Log("pveChapter = " + pveChapter);
        //获取自己的关卡和章节,验证要开启的关卡是否大于当前开启的开关
        string[] pve1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PVEChapter", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
        string[] pve2 = pveChapter.Split(';');
        //Debug.Log("pve1[0] = " + pve1[0] + "pve2[1]" + pve1[1]);
        if (int.Parse(pve2[0]) >= int.Parse(pve1[0]))
        {
            //判断如果大章节大则直接覆盖不必判断子章节
            if (int.Parse(pve2[0]) > int.Parse(pve1[0])) {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PVEChapter", pveChapter, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

                string[] chapterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneIDSet", "ID", pve2[0], "Chapter_Template").Split(';');
                string chapterSonName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterSonName", "ID", chapterName[int.Parse(pve2[1]) - 1], "ChapterSon_Template");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_425");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + chapterSonName);
                //Game_PublicClassVar.Get_function_UI.GameHint("新的章节开启成功!!!  " + chapterSonName);
                return;

            }

            //判断子关卡
            if (int.Parse(pve2[1]) >= int.Parse(pve1[1]))
            {
                //开启新关卡
                //Debug.Log("pveChapter22222 = " + pveChapter);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PVEChapter", pveChapter, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                //Debug.Log("数据存储完毕");
                //提示
                //Debug.Log("pve2[0] = " + pve2[0]);
                string[] chapterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneIDSet", "ID", pve2[0], "Chapter_Template").Split(';');
                //Debug.Log("chapterName[int.Parse(pve2[1])] = " + chapterName[int.Parse(pve2[1])]);
                string chapterSonName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterSonName", "ID", chapterName[int.Parse(pve2[1])-1], "ChapterSon_Template");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_426");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + chapterSonName);
                //Game_PublicClassVar.Get_function_UI.GameHint("关卡快捷传送激活成功!!!  " + chapterSonName);

            }
            else {
                Debug.Log("开启关卡失败！");
            }
        }
        else {
            Debug.Log("开启关卡失败！");
        }
    }

    //传入套装ID返回当前套装满足数量 装备数组
    public int returnEquipSuitNum(string[] equipIDStr, string[] equipNumStr) {

        int returnEquipSuitNum = 0;
        //循环判定当前身上携带的套装装备数量
        for (int i = 0; i <= equipIDStr.Length - 1; i++) {
            //获取传入装备的数量
            int wearEquipNum = IfWearEquipID(equipIDStr[i]);
            //如果穿戴的装备大于套装要求的数量就会返回套装要求的数量
            if (wearEquipNum >= int.Parse(equipNumStr[i]))
            {
                returnEquipSuitNum = returnEquipSuitNum + int.Parse(equipNumStr[i]);
            }
            else {
                returnEquipSuitNum = returnEquipSuitNum + wearEquipNum;
            }
        }
        return returnEquipSuitNum;
    }

    //传入道具ID判定当前是否装备传入ID 返回当前装备的数量
    public int IfWearEquipID(string equipIDPar) {

        Function_DataSet functionDataSet = Game_PublicClassVar.Get_function_DataSet;
        int returnEquipNum = 0;
        //获取当前身上装备ID
        for (int i = 1; i <= 13; i++)
        {
            string equipID_1 = functionDataSet.DataSet_ReadData("EquipItemID", "ID", i.ToString(), "RoseEquip");
            if (equipID_1 == equipIDPar)
            {
                returnEquipNum = returnEquipNum + 1;
            }
        }

        //获取十二生肖身上的ID
        string EquipIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShengXiaoSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //Debug.Log("EquipIDStr = " + EquipIDStr);
        if (EquipIDStr != "" && EquipIDStr != "0" || EquipIDStr != null)
        {
            string[] EquipIDList = EquipIDStr.Split(';');
            for (int i = 0; i < EquipIDList.Length; i++) {
                if (EquipIDList[i] == equipIDPar&& EquipIDList[i] != ""&& EquipIDList[i] != "0"&& EquipIDList[i] != null) {
                    returnEquipNum = returnEquipNum + 1;
                }
            }
        }

        return returnEquipNum;
    }

    //传入子ID查看套装条件是否满足
    /*
    public void ifEquipSuitProperty(string suitPropertyID) {
        //获取自身套装数量
        int equipSuitNum = Game_PublicClassVar.Get_function_Rose.returnEquipSuitNum(needEquipIDSet, needEquipNumSet);


            string triggerSuitNum = suitPropertyIDSet[i].Split(',')[0];
            string triggerSuitPropertyID = suitPropertyIDSet[i].Split(',')[1];
            //显示套装属性
            GameObject propertyObj = (GameObject)Instantiate(Obj_EquipSuitPropertyText);
            propertyObj.transform.SetParent(Obj_UIEquipSuit.transform);
            propertyObj.transform.localScale = new Vector3(1, 1, 1);
            string equipSuitDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSuitDes", "ID", triggerSuitPropertyID, "EquipSuitProperty_Template");
            propertyObj.GetComponent<Text>().text = triggerSuitNum + "件套：" + equipSuitDes;
            float suitShowTextNum = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowTextNum", "ID", triggerSuitPropertyID, "EquipSuitProperty_Template"));
            suitShowTextNumSum = suitShowTextNumSum + suitShowTextNum;
            propertyObj.transform.localPosition = new Vector3(10, -30 - 25 * suitShowTextNumSum, 0);

    
    }
    */


    //消耗一个道具在仓库内指定位置的数量  （道具ID 道具数量 格子位置, 是否扣除全部）
    public bool CostStoreHouseSpaceNumItem(string itemID, int itemNum, string spaceNum, bool costAllSpace)
    {
        string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceNum, "RoseStoreHouse");
        string bagItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceNum, "RoseStoreHouse");
        //是否扣除全部
        if (costAllSpace)
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceNum, "RoseStoreHouse");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceNum, "RoseStoreHouse");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceNum, "RoseStoreHouse");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            return true;
        }

        if (itemID == bagItemID)
        {
            int otherValue = int.Parse(bagItemNum) - itemNum;
            if (otherValue > 0)
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", otherValue.ToString(), "ID", spaceNum, "RoseStoreHouse");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
            }
            else
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceNum, "RoseStoreHouse");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceNum, "RoseStoreHouse");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceNum, "RoseStoreHouse");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
            }
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    //发送道具到仓库
    public bool SendRewardToStoreHouse(string dropID, int dropNum, string broadcastType ="0",string hideID = "0")
    {

        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        //更新背包立即显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

        bool ifGold = false;
        //掉落为空
        if (dropID == "0")
        {
            return true;
        }

        //判定掉落是否为金币
        if (dropID == "1")
        {
            int goldNum = dropNum;
            Game_PublicClassVar.Get_function_Rose.SendReward("1", goldNum.ToString(), "49");
            ifGold = true;

            //弹窗提示
            //string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
            
            switch(broadcastType)
            {

                //广播
                case "0":
                    string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_421");
                    string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_422");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + dropNum.ToString() + langStrHint_2);
                    //Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "金币");
                    break;

                //不广播
                case "1":
                    break;

            }
            //更新道具任务显示
            Game_PublicClassVar.Get_function_Task.updataTaskItemID();
            return true;
        }
        else
        {
            ifGold = false;
        }

        if (!ifGold)
        {
			//获取当前页数
			int startSpace = 1;
			int endSpace = 49;

			switch(Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu){
				
				case 1:
					startSpace = 1;
					endSpace = 49;
					break;
				case 2:
					startSpace = 50;
					endSpace = 98;
					break;
				case 3:
					startSpace = 99;
					endSpace = 147;
					break;
                case 4:
                    startSpace = 148;
                    endSpace = 196;
                    break;
            }

            //将掉落的道具ID添加到背包内
			for (int i = startSpace; i <= endSpace; i++)
            {
                //获得当前背包内对应格子的道具ID
                string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseStoreHouse");
                //Rdate = "0";
                //寻找背包内有没有相同的道具ID
                if (dropID == Rdate)
                {

                    //读取当前道具数量
                    string itemValue = function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseStoreHouse");
                    //读取当前道具的堆叠数量的最大值
                    string itemPileSum = function_DataSet.DataSet_ReadData("ItemPileSum", "ID", dropID, "Item_Template");
                    int itemNum = int.Parse(itemValue) + dropNum; //将数量累加（此处没有顾忌到自己背包格子满的处理方式，以后添加）
                    //当满足堆叠数量,执行道具捡取
                    if (int.Parse(itemPileSum) >= itemNum)
                    {
                        //添加获得的道具数量
                        function_DataSet.DataSet_WriteData("ItemNum", itemNum.ToString(), "ID", i.ToString(), "RoseStoreHouse");
                        function_DataSet.DataSet_SetXml("RoseStoreHouse");
                        //弹窗提示
                        string itemName = function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
                        
                        switch (broadcastType)
                        {
                            //广播
                            case "0":
                                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_421");
                                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_427");
                                Game_PublicClassVar.Get_function_UI.GameHint(langStrHint_1 + dropNum.ToString() + langStrHint_2 + itemName);
                                //Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "个" + itemName);
                                break;
                            //不广播
                            case "1":
                                break;
                        }
                        //ai_dorpitem.IfRoseTake = true;      //注销拾取的道具
                        //更新道具任务显示
                        Game_PublicClassVar.Get_function_Task.updataTaskItemID();
                        //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
                        string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", dropID, "Item_Template");
                        if (itemType == "1")
                        {
                            Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
                        }
                        return true;
                        break;
                    }
                }

                //发现背包格子为空，将数据直接塞进空的格子中（从前面排序）
                if (Rdate == "0")
                {
                    function_DataSet.DataSet_WriteData("ItemID", dropID, "ID", i.ToString(), "RoseStoreHouse");
                    function_DataSet.DataSet_WriteData("ItemNum", dropNum.ToString(), "ID", i.ToString(), "RoseStoreHouse");
                    function_DataSet.DataSet_WriteData("HideID", hideID, "ID", i.ToString(), "RoseStoreHouse");
                    function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    //弹窗提示
                    string itemName = function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
                    
                    switch (broadcastType)
                    {
                        //广播
                        case "0":
                            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_421");
                            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_427");
                            Game_PublicClassVar.Get_function_UI.GameHint(langStrHint_1 + dropNum.ToString() + langStrHint_2 + itemName);
                            //Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "个" + itemName);
                            break;
                        //不广播
                        case "1":
                            break;
                    }
                    //更新道具任务显示
                    Game_PublicClassVar.Get_function_Task.updataTaskItemID();
                    //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
                    string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", dropID, "Item_Template");
                    if (itemType == "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
                    }
                    return true;
                    break;  //跳出循环
                }

                //在结束循环的最后判定道具如果没有被拾取,判定为背包满了
                if (i == 70)
                {
                    return false;
                }
            }
        }
        return false;
    }

    //获取仓库对应标签页空的位置
    public string StoreHouse_ReturnNullSpaceNum(string storeHouseTitle) {

        int spaceNum_Start = 0;
        int spaceNum_End = 0;

        switch (storeHouseTitle) {

            case "1":
                spaceNum_Start = 1;
                spaceNum_End = 49;
                break;

            case "2":
                spaceNum_Start = 50;
                spaceNum_End = 98;
                break;

            case "3":
                spaceNum_Start = 99;
                spaceNum_End = 147;
                break;

            case "4":
                spaceNum_Start = 148;
                spaceNum_End = 196;
                break;

        }

        if (spaceNum_Start != 0 && spaceNum_End != 0) {

            for (int i = spaceNum_Start; i <= spaceNum_End; i++)
            {
                //获取当前背包格子的道具ID和数量；
                string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseStoreHouse");
                if (bagItemID == "0")
                {
                    return i.ToString();
                }
            }
            return "-1";

        }

        return "-1";
    }

    //整理背包
    public void RoseArrangeBag()
    {
        bool use_old = false;
        if (use_old)
        {
            this.RoseArrangeBagOld( );
        }
        else
        {
            this.RoseArrangeBagEx( );
        }
    }

   
    public void RoseArrangeBagOld() {

        //Debug.Log("Time111:" + getTime());

        //检索移动不成功的条件
        if (Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus)
        {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_340");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("洗练装备时禁止整理背包！");
            return;
        }

        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        string bagItemIDStr = "";
        string bagItemNumStr = "";
        string bagItemHideIDStr = "";
        string bagItemParStr = "";
        string bagItemGemHoleStr = "";
        string bagItemGemIDStr = "";

        //将掉落的道具ID添加到背包内
        int itemValue = 0;
        string hideID = "";
        string itemPar = "";
        string item_GemHole = "";
        string item_GemID = "";
        //Debug.Log("Time222:" + getTime());

        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (Rdate != "" && Rdate != "0")
            {
                //读取当前道具数量
                itemValue = int.Parse(function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseBag"));
                hideID = function_DataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseBag");
                itemPar = function_DataSet.DataSet_ReadData("ItemPar", "ID", i.ToString(), "RoseBag");
                item_GemHole = function_DataSet.DataSet_ReadData("GemHole", "ID", i.ToString(), "RoseBag");
                if (item_GemHole != "" && item_GemHole != "0") {
                    item_GemID = function_DataSet.DataSet_ReadData("GemID", "ID", i.ToString(), "RoseBag");
                }


                //防止整理背包出错,如果道具ID不为0,默认道具数量为1
                if (itemValue == 0) {
                    itemValue = 1;
                }

                //获得背包道具数量
                //获取道具背包最大堆叠数量
                //Debug.Log("Rdate = " + Rdate);
                string itemID_DuiBi = "";
                int itemPileSum = int.Parse(function_DataSet.DataSet_ReadData("ItemPileSum", "ID", Rdate, "Item_Template"));
                if (itemPileSum > itemValue) { 
                    //向背包后面道具查询数量
                    for (int y = i + 1; y <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; y++)
                    { 
                        //对比道具ID
                        itemID_DuiBi = function_DataSet.DataSet_ReadData("ItemID", "ID", y.ToString(), "RoseBag");
                        if (Rdate == itemID_DuiBi) {
                            //道具ID相同获取数量
                            int itemNum_DuiBi = int.Parse(function_DataSet.DataSet_ReadData("ItemNum", "ID", y.ToString(), "RoseBag"));

                            //数量相加判定是否满足前面堆叠数量
                            int itemSunNum_DuiBi = itemValue + itemNum_DuiBi;
                            if (itemSunNum_DuiBi >= itemPileSum)
                            {
                                int itemCostNum_DuiBi = itemSunNum_DuiBi - itemPileSum;
                                itemValue = itemPileSum;
                                //当满足上一次的堆叠数量自己还有剩余数量时进行记录
                                if (itemCostNum_DuiBi > 0) { 
                                    function_DataSet.DataSet_WriteData("ItemNum", itemCostNum_DuiBi.ToString(),"ID", y.ToString(), "RoseBag");
                                }else{
                                    function_DataSet.DataSet_WriteData("ItemNum", "0","ID", y.ToString(), "RoseBag");
                                    function_DataSet.DataSet_WriteData("ItemID", "0", "ID", y.ToString(), "RoseBag");

                                }

                                if (itemCostNum_DuiBi >= 0) {
                                    y = Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; //跳出循环
                                }
                            }
                            else {
                                itemValue = itemValue + itemNum_DuiBi;
                                function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", y.ToString(), "RoseBag");
                                function_DataSet.DataSet_WriteData("ItemID", "0", "ID", y.ToString(), "RoseBag");
                            }
                        }
                    }
                }

                //背包整理字符串累计
                bagItemIDStr = bagItemIDStr + Rdate + ",";
                bagItemNumStr = bagItemNumStr + itemValue + ",";
                bagItemHideIDStr = bagItemHideIDStr + hideID + ",";
                bagItemParStr = bagItemParStr + itemPar + "#";
                bagItemGemHoleStr = bagItemGemHoleStr + item_GemHole +  "#";
                bagItemGemIDStr = bagItemGemIDStr + item_GemID + "#";

                //清空当前格子数据
                /*
                function_DataSet.DataSet_WriteData("ItemID", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("GemHole", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("GemID", "0", "ID", i.ToString(), "RoseBag");
                */
            }
        }
        //Debug.Log("Time333:" + getTime());
        if (bagItemIDStr != "") {
            //Debug.Log("bagItemIDStr1 = " + bagItemIDStr);
            bagItemIDStr = bagItemIDStr.Substring(0, bagItemIDStr.Length - 1);
            bagItemNumStr = bagItemNumStr.Substring(0, bagItemNumStr.Length - 1);
            //Debug.Log("bagItemIDStr2 = " + bagItemIDStr);
        }

        //存储当前背包道具ID和数量
        string[] bagItemID_Now;
        string[] bagItemNum_Now;
        string[] bagItemHide_Now;
        string[] bagItemPar_Now;
        string[] bagItemGemHole_Now;
        string[] bagItemGemID_Now;

        if (bagItemIDStr != "")
        {
            bagItemID_Now = bagItemIDStr.Split(',');
            bagItemNum_Now = bagItemNumStr.Split(',');
            bagItemHide_Now = bagItemHideIDStr.Split(',');
            bagItemPar_Now = bagItemParStr.Split('#');
            bagItemGemHole_Now = bagItemGemHoleStr.Split('#');
            bagItemGemID_Now = bagItemGemIDStr.Split('#');
        }
        else
        {
            //Debug.Log("当前背包没有道具,不需要整理");
            return;
        }
        //整理当前背包道具数量
        //--消耗性道具
        string bagItemIDType1_Str_Q1 = "";
        string bagItemIDType1_Str_Q2 = "";
        string bagItemIDType1_Str_Q3 = "";
        string bagItemIDType1_Str_Q4 = "";
        string bagItemIDType1_Str_Q5 = "";
        string bagItemNumType1_Str_Q1 = "";
        string bagItemNumType1_Str_Q2 = "";
        string bagItemNumType1_Str_Q3 = "";
        string bagItemNumType1_Str_Q4 = "";
        string bagItemNumType1_Str_Q5 = "";
        string bagHideIDType1_Str_Q1 = "";
        string bagHideIDType1_Str_Q2 = "";
        string bagHideIDType1_Str_Q3 = "";
        string bagHideIDType1_Str_Q4 = "";
        string bagHideIDType1_Str_Q5 = "";
        string bagItemParType1_Str_Q1 = "";
        string bagItemParType1_Str_Q2 = "";
        string bagItemParType1_Str_Q3 = "";
        string bagItemParType1_Str_Q4 = "";
        string bagItemParType1_Str_Q5 = "";
        string bagGemHoleType1_Str_Q1 = "";
        string bagGemHoleType1_Str_Q2 = "";
        string bagGemHoleType1_Str_Q3 = "";
        string bagGemHoleType1_Str_Q4 = "";
        string bagGemHoleType1_Str_Q5 = "";
        string bagGemIDType1_Str_Q1 = "";
        string bagGemIDType1_Str_Q2 = "";
        string bagGemIDType1_Str_Q3 = "";
        string bagGemIDType1_Str_Q4 = "";
        string bagGemIDType1_Str_Q5 = "";

        //--材料道具
        string bagItemIDType2_Str_Q1 = "";
        string bagItemIDType2_Str_Q2 = "";
        string bagItemIDType2_Str_Q3 = "";
        string bagItemIDType2_Str_Q4 = "";
        string bagItemIDType2_Str_Q5 = "";
        string bagItemNumType2_Str_Q1 = "";
        string bagItemNumType2_Str_Q2 = "";
        string bagItemNumType2_Str_Q3 = "";
        string bagItemNumType2_Str_Q4 = "";
        string bagItemNumType2_Str_Q5 = "";
        string bagHideIDType2_Str_Q1 = "";
        string bagHideIDType2_Str_Q2 = "";
        string bagHideIDType2_Str_Q3 = "";
        string bagHideIDType2_Str_Q4 = "";
        string bagHideIDType2_Str_Q5 = "";
        string bagItemParType2_Str_Q1 = "";
        string bagItemParType2_Str_Q2 = "";
        string bagItemParType2_Str_Q3 = "";
        string bagItemParType2_Str_Q4 = "";
        string bagItemParType2_Str_Q5 = "";
        string bagGemHoleType2_Str_Q1 = "";
        string bagGemHoleType2_Str_Q2 = "";
        string bagGemHoleType2_Str_Q3 = "";
        string bagGemHoleType2_Str_Q4 = "";
        string bagGemHoleType2_Str_Q5 = "";
        string bagGemIDType2_Str_Q1 = "";
        string bagGemIDType2_Str_Q2 = "";
        string bagGemIDType2_Str_Q3 = "";
        string bagGemIDType2_Str_Q4 = "";
        string bagGemIDType2_Str_Q5 = "";

        //--装备道具
        string bagItemIDType3_Str_Q1 = "";
        string bagItemIDType3_Str_Q2 = "";
        string bagItemIDType3_Str_Q3 = "";
        string bagItemIDType3_Str_Q4 = "";
        string bagItemIDType3_Str_Q5 = "";
        string bagItemNumType3_Str_Q1 = "";
        string bagItemNumType3_Str_Q2 = "";
        string bagItemNumType3_Str_Q3 = "";
        string bagItemNumType3_Str_Q4 = "";
        string bagItemNumType3_Str_Q5 = "";
        string bagHideIDType3_Str_Q1 = "";
        string bagHideIDType3_Str_Q2 = "";
        string bagHideIDType3_Str_Q3 = "";
        string bagHideIDType3_Str_Q4 = "";
        string bagHideIDType3_Str_Q5 = "";
        string bagItemParType3_Str_Q1 = "";
        string bagItemParType3_Str_Q2 = "";
        string bagItemParType3_Str_Q3 = "";
        string bagItemParType3_Str_Q4 = "";
        string bagItemParType3_Str_Q5 = "";
        string bagGemHoleType3_Str_Q1 = "";
        string bagGemHoleType3_Str_Q2 = "";
        string bagGemHoleType3_Str_Q3 = "";
        string bagGemHoleType3_Str_Q4 = "";
        string bagGemHoleType3_Str_Q5 = "";
        string bagGemIDType3_Str_Q1 = "";
        string bagGemIDType3_Str_Q2 = "";
        string bagGemIDType3_Str_Q3 = "";
        string bagGemIDType3_Str_Q4 = "";
        string bagGemIDType3_Str_Q5 = "";

        //--宝石道具
        string bagItemIDType4_Str_Q1 = "";
        string bagItemIDType4_Str_Q2 = "";
        string bagItemIDType4_Str_Q3 = "";
        string bagItemIDType4_Str_Q4 = "";
        string bagItemIDType4_Str_Q5 = "";
        string bagItemNumType4_Str_Q1 = "";
        string bagItemNumType4_Str_Q2 = "";
        string bagItemNumType4_Str_Q3 = "";
        string bagItemNumType4_Str_Q4 = "";
        string bagItemNumType4_Str_Q5 = "";
        string bagHideIDType4_Str_Q1 = "";
        string bagHideIDType4_Str_Q2 = "";
        string bagHideIDType4_Str_Q3 = "";
        string bagHideIDType4_Str_Q4 = "";
        string bagHideIDType4_Str_Q5 = "";
        string bagItemParType4_Str_Q1 = "";
        string bagItemParType4_Str_Q2 = "";
        string bagItemParType4_Str_Q3 = "";
        string bagItemParType4_Str_Q4 = "";
        string bagItemParType4_Str_Q5 = "";
        string bagGemHoleType4_Str_Q1 = "";
        string bagGemHoleType4_Str_Q2 = "";
        string bagGemHoleType4_Str_Q3 = "";
        string bagGemHoleType4_Str_Q4 = "";
        string bagGemHoleType4_Str_Q5 = "";
        string bagGemIDType4_Str_Q1 = "";
        string bagGemIDType4_Str_Q2 = "";
        string bagGemIDType4_Str_Q3 = "";
        string bagGemIDType4_Str_Q4 = "";
        string bagGemIDType4_Str_Q5 = "";

        //--被动技能道具
        string bagItemIDType5_Str_Q1 = "";
        string bagItemIDType5_Str_Q2 = "";
        string bagItemIDType5_Str_Q3 = "";
        string bagItemIDType5_Str_Q4 = "";
        string bagItemIDType5_Str_Q5 = "";
        string bagItemNumType5_Str_Q1 = "";
        string bagItemNumType5_Str_Q2 = "";
        string bagItemNumType5_Str_Q3 = "";
        string bagItemNumType5_Str_Q4 = "";
        string bagItemNumType5_Str_Q5 = "";
        string bagHideIDType5_Str_Q1 = "";
        string bagHideIDType5_Str_Q2 = "";
        string bagHideIDType5_Str_Q3 = "";
        string bagHideIDType5_Str_Q4 = "";
        string bagHideIDType5_Str_Q5 = "";
        string bagItemParType5_Str_Q1 = "";
        string bagItemParType5_Str_Q2 = "";
        string bagItemParType5_Str_Q3 = "";
        string bagItemParType5_Str_Q4 = "";
        string bagItemParType5_Str_Q5 = "";
        string bagGemHoleType5_Str_Q1 = "";
        string bagGemHoleType5_Str_Q2 = "";
        string bagGemHoleType5_Str_Q3 = "";
        string bagGemHoleType5_Str_Q4 = "";
        string bagGemHoleType5_Str_Q5 = "";
        string bagGemIDType5_Str_Q1 = "";
        string bagGemIDType5_Str_Q2 = "";
        string bagGemIDType5_Str_Q3 = "";
        string bagGemIDType5_Str_Q4 = "";
        string bagGemIDType5_Str_Q5 = "";

        string itemType = "";
        string itemQuality = "";
        for (int i = 0; i <= bagItemID_Now.Length - 1; i++) {
            //获取道具类型和品质
            itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", bagItemID_Now[i], "Item_Template");
            itemQuality = function_DataSet.DataSet_ReadData("ItemQuality", "ID", bagItemID_Now[i], "Item_Template");
            switch (itemType) { 
                //消耗性道具
                case "1":
                    switch (itemQuality) { 
                        //品质-1
                        case "1":
                            bagItemIDType1_Str_Q1 = bagItemIDType1_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q1 = bagItemNumType1_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q1 = bagHideIDType1_Str_Q1 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q1 = bagItemParType1_Str_Q1 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q1 = bagGemHoleType1_Str_Q1 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q1 = bagGemIDType1_Str_Q1 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-2
                        case "2":
                            bagItemIDType1_Str_Q2 = bagItemIDType1_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q2 = bagItemNumType1_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q2 = bagHideIDType1_Str_Q2 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q2 = bagItemParType1_Str_Q2 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q2 = bagGemHoleType1_Str_Q2 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q2 = bagGemIDType1_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType1_Str_Q3 = bagItemIDType1_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q3 = bagItemNumType1_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q3 = bagHideIDType1_Str_Q3 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q3 = bagItemParType1_Str_Q3 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q3 = bagGemHoleType1_Str_Q3 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q3 = bagGemIDType1_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType1_Str_Q4 = bagItemIDType1_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q4 = bagItemNumType1_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q4 = bagHideIDType1_Str_Q4 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q4 = bagItemParType1_Str_Q4 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q4 = bagGemHoleType1_Str_Q4 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q4 = bagGemIDType1_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType1_Str_Q5 = bagItemIDType1_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q5 = bagItemNumType1_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q5 = bagHideIDType1_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q5 = bagItemParType1_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q5 = bagGemHoleType1_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q5 = bagGemIDType1_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;

                        //品质-5
                        case "6":
                            bagItemIDType1_Str_Q5 = bagItemIDType1_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q5 = bagItemNumType1_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q5 = bagHideIDType1_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q5 = bagItemParType1_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q5 = bagGemHoleType1_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q5 = bagGemIDType1_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                break;
                //材料
                case "2":
                    switch (itemQuality)
                    {
                        //品质-1
                        case "1":
                            bagItemIDType2_Str_Q1 = bagItemIDType2_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q1 = bagItemNumType2_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q1 = bagHideIDType2_Str_Q1 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q1 = bagItemParType2_Str_Q1 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q1 = bagGemHoleType2_Str_Q1 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q1 = bagGemIDType2_Str_Q1 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-2
                        case "2":
                            bagItemIDType2_Str_Q2 = bagItemIDType2_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q2 = bagItemNumType2_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q2 = bagHideIDType2_Str_Q2 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q2 = bagItemParType2_Str_Q2 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q2 = bagGemHoleType2_Str_Q2 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q2 = bagGemIDType2_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType2_Str_Q3 = bagItemIDType2_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q3 = bagItemNumType2_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q3 = bagHideIDType2_Str_Q3 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q3 = bagItemParType2_Str_Q3 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q3 = bagGemHoleType2_Str_Q3 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q3 = bagGemIDType2_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType2_Str_Q4 = bagItemIDType2_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q4 = bagItemNumType2_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q4 = bagHideIDType2_Str_Q4 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q4 = bagItemParType2_Str_Q4 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q4 = bagGemHoleType2_Str_Q4 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q4 = bagGemIDType2_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType2_Str_Q5 = bagItemIDType2_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q5 = bagItemNumType2_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q5 = bagHideIDType2_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q5 = bagItemParType2_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q5 = bagGemHoleType2_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q5 = bagGemIDType2_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;

                        //品质-5
                        case "6":
                            bagItemIDType2_Str_Q5 = bagItemIDType2_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q5 = bagItemNumType2_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q5 = bagHideIDType2_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q5 = bagItemParType2_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q5 = bagGemHoleType2_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q5 = bagGemIDType2_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                break;
                //装备
                case "3":

                    switch (itemQuality)
                    {
                        //品质-1
                        case "1":
                            bagItemIDType3_Str_Q1 = bagItemIDType3_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q1 = bagItemNumType3_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q1 = bagHideIDType3_Str_Q1 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q1 = bagItemParType3_Str_Q1 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q1 = bagGemHoleType3_Str_Q1 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q1 = bagGemIDType3_Str_Q1 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-2
                        case "2":
                            bagItemIDType3_Str_Q2 = bagItemIDType3_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q2 = bagItemNumType3_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q2 = bagHideIDType3_Str_Q2 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q2 = bagItemParType3_Str_Q2 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q2 = bagGemHoleType3_Str_Q2 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q2 = bagGemIDType3_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType3_Str_Q3 = bagItemIDType3_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q3 = bagItemNumType3_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q3 = bagHideIDType3_Str_Q3 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q3 = bagItemParType3_Str_Q3 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q3 = bagGemHoleType3_Str_Q3 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q3 = bagGemIDType3_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType3_Str_Q4 = bagItemIDType3_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q4 = bagItemNumType3_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q4 = bagHideIDType3_Str_Q4 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q4 = bagItemParType3_Str_Q4 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q4 = bagGemHoleType3_Str_Q4 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q4 = bagGemIDType3_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType3_Str_Q5 = bagItemIDType3_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q5 = bagItemNumType3_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q5 = bagHideIDType3_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q5 = bagItemParType3_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q5 = bagGemHoleType3_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q5 = bagGemIDType3_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;

                        //品质-5
                        case "6":
                            bagItemIDType3_Str_Q5 = bagItemIDType3_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q5 = bagItemNumType3_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q5 = bagHideIDType3_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q5 = bagItemParType3_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q5 = bagGemHoleType3_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q5 = bagGemIDType3_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                break;

                //宝石
                case "4":

                switch (itemQuality)
                {
                    //品质-1
                    case "1":
                        bagItemIDType4_Str_Q1 = bagItemIDType4_Str_Q1 + bagItemID_Now[i] + ",";
                        bagItemNumType4_Str_Q1 = bagItemNumType4_Str_Q1 + bagItemNum_Now[i] + ",";
                        bagHideIDType4_Str_Q1 = bagHideIDType4_Str_Q1 + bagItemHide_Now[i] + ",";
                        bagItemParType4_Str_Q1 = bagItemParType4_Str_Q1 + bagItemPar_Now[i] + "#";
                        bagGemHoleType4_Str_Q1 = bagGemHoleType4_Str_Q1 + bagItemGemHole_Now[i] + "#";
                        bagGemIDType4_Str_Q1 = bagGemIDType4_Str_Q1 + bagItemGemID_Now[i] + "#";
                            break;
                    //品质-2
                    case "2":
                        bagItemIDType4_Str_Q2 = bagItemIDType4_Str_Q2 + bagItemID_Now[i] + ",";
                        bagItemNumType4_Str_Q2 = bagItemNumType4_Str_Q2 + bagItemNum_Now[i] + ",";
                        bagHideIDType4_Str_Q2 = bagHideIDType4_Str_Q2 + bagItemHide_Now[i] + ",";
                        bagItemParType4_Str_Q2 = bagItemParType4_Str_Q2 + bagItemPar_Now[i] + "#";
                        bagGemHoleType4_Str_Q2 = bagGemHoleType4_Str_Q2 + bagItemGemHole_Now[i] + "#";
                        bagGemIDType4_Str_Q2 = bagGemIDType4_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                    //品质-3
                    case "3":
                        bagItemIDType4_Str_Q3 = bagItemIDType4_Str_Q3 + bagItemID_Now[i] + ",";
                        bagItemNumType4_Str_Q3 = bagItemNumType4_Str_Q3 + bagItemNum_Now[i] + ",";
                        bagHideIDType4_Str_Q3 = bagHideIDType4_Str_Q3 + bagItemHide_Now[i] + ",";
                        bagItemParType4_Str_Q3 = bagItemParType4_Str_Q3 + bagItemPar_Now[i] + "#";
                        bagGemHoleType4_Str_Q3 = bagGemHoleType4_Str_Q3 + bagItemGemHole_Now[i] + "#";
                        bagGemIDType4_Str_Q3 = bagGemIDType4_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                    //品质-4
                    case "4":
                        bagItemIDType4_Str_Q4 = bagItemIDType4_Str_Q4 + bagItemID_Now[i] + ",";
                        bagItemNumType4_Str_Q4 = bagItemNumType4_Str_Q4 + bagItemNum_Now[i] + ",";
                        bagHideIDType4_Str_Q4 = bagHideIDType4_Str_Q4 + bagItemHide_Now[i] + ",";
                        bagItemParType4_Str_Q4 = bagItemParType4_Str_Q4 + bagItemPar_Now[i] + "#";
                        bagGemHoleType4_Str_Q4 = bagGemHoleType4_Str_Q4 + bagItemGemHole_Now[i] + "#";
                        bagGemIDType4_Str_Q4 = bagGemIDType4_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                    //品质-5
                    case "5":
                        bagItemIDType4_Str_Q5 = bagItemIDType4_Str_Q5 + bagItemID_Now[i] + ",";
                        bagItemNumType4_Str_Q5 = bagItemNumType4_Str_Q5 + bagItemNum_Now[i] + ",";
                        bagHideIDType4_Str_Q5 = bagHideIDType4_Str_Q5 + bagItemHide_Now[i] + ",";
                        bagItemParType4_Str_Q5 = bagItemParType4_Str_Q5 + bagItemPar_Now[i] + "#";
                        bagGemHoleType4_Str_Q5 = bagGemHoleType4_Str_Q5 + bagItemGemHole_Now[i] + "#";
                        bagGemIDType4_Str_Q5 = bagGemIDType4_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;

                        //品质-5
                        case "6":
                            bagItemIDType4_Str_Q5 = bagItemIDType4_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType4_Str_Q5 = bagItemNumType4_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType4_Str_Q5 = bagHideIDType4_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType4_Str_Q5 = bagItemParType4_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType4_Str_Q5 = bagGemHoleType4_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType4_Str_Q5 = bagGemIDType4_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                break;

                //被动技能
                case "5":
                switch (itemQuality)
                {
                    //品质-1
                    case "1":
                        bagItemIDType5_Str_Q1 = bagItemIDType5_Str_Q1 + bagItemID_Now[i] + ",";
                        bagItemNumType5_Str_Q1 = bagItemNumType5_Str_Q1 + bagItemNum_Now[i] + ",";
                        bagHideIDType5_Str_Q1 = bagHideIDType5_Str_Q1 + bagItemHide_Now[i] + ",";
                        bagItemParType5_Str_Q1 = bagItemParType5_Str_Q1 + bagItemPar_Now[i] + "#";
                        bagGemHoleType5_Str_Q1 = bagGemHoleType5_Str_Q1 + bagItemGemHole_Now[i] + "#";
                        bagGemIDType5_Str_Q1 = bagGemIDType5_Str_Q1 + bagItemGemID_Now[i] + "#";

                            break;
                    //品质-2
                    case "2":
                        bagItemIDType5_Str_Q2 = bagItemIDType5_Str_Q2 + bagItemID_Now[i] + ",";
                        bagItemNumType5_Str_Q2 = bagItemNumType5_Str_Q2 + bagItemNum_Now[i] + ",";
                        bagHideIDType5_Str_Q2 = bagHideIDType5_Str_Q2 + bagItemHide_Now[i] + ",";
                        bagItemParType5_Str_Q2 = bagItemParType5_Str_Q2 + bagItemPar_Now[i] + "#";
                        bagGemHoleType5_Str_Q2 = bagGemHoleType5_Str_Q2 + bagItemGemHole_Now[i] + "#";
                        bagGemIDType5_Str_Q2 = bagGemIDType5_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                    //品质-3
                    case "3":
                        bagItemIDType5_Str_Q3 = bagItemIDType5_Str_Q3 + bagItemID_Now[i] + ",";
                        bagItemNumType5_Str_Q3 = bagItemNumType5_Str_Q3 + bagItemNum_Now[i] + ",";
                        bagHideIDType5_Str_Q3 = bagHideIDType5_Str_Q3 + bagItemHide_Now[i] + ",";
                        bagItemParType5_Str_Q3 = bagItemParType5_Str_Q3 + bagItemPar_Now[i] + "#";
                        bagGemHoleType5_Str_Q3 = bagGemHoleType5_Str_Q3 + bagItemGemHole_Now[i] + "#";
                        bagGemIDType5_Str_Q3 = bagGemIDType5_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                    //品质-4
                    case "4":
                        bagItemIDType5_Str_Q4 = bagItemIDType5_Str_Q4 + bagItemID_Now[i] + ",";
                        bagItemNumType5_Str_Q4 = bagItemNumType5_Str_Q4 + bagItemNum_Now[i] + ",";
                        bagHideIDType5_Str_Q4 = bagHideIDType5_Str_Q4 + bagItemHide_Now[i] + ",";
                        bagItemParType5_Str_Q4 = bagItemParType5_Str_Q4 + bagItemPar_Now[i] + "#";
                        bagGemHoleType5_Str_Q4 = bagGemHoleType5_Str_Q4 + bagItemGemHole_Now[i] + "#";
                        bagGemIDType5_Str_Q4 = bagGemIDType5_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                    //品质-5
                    case "5":
                        bagItemIDType5_Str_Q5 = bagItemIDType5_Str_Q5 + bagItemID_Now[i] + ",";
                        bagItemNumType5_Str_Q5 = bagItemNumType5_Str_Q5 + bagItemNum_Now[i] + ",";
                        bagHideIDType5_Str_Q5 = bagHideIDType5_Str_Q5 + bagItemHide_Now[i] + ",";
                        bagItemParType5_Str_Q5 = bagItemParType5_Str_Q5 + bagItemPar_Now[i] + "#";
                        bagGemHoleType5_Str_Q5 = bagGemHoleType5_Str_Q5 + bagItemGemHole_Now[i] + "#";
                        bagGemIDType5_Str_Q5 = bagGemIDType5_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;

                        //品质-5
                        case "6":
                            bagItemIDType5_Str_Q5 = bagItemIDType5_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q5 = bagItemNumType5_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q5 = bagHideIDType5_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q5 = bagItemParType5_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q5 = bagGemHoleType5_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q5 = bagGemIDType5_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                break;


                //家园
                case "6":
                    switch (itemQuality)
                    {
                        //品质-1
                        case "1":
                            bagItemIDType5_Str_Q1 = bagItemIDType5_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q1 = bagItemNumType5_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q1 = bagHideIDType5_Str_Q1 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q1 = bagItemParType5_Str_Q1 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q1 = bagGemHoleType5_Str_Q1 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q1 = bagGemIDType5_Str_Q1 + bagItemGemID_Now[i] + "#";

                            break;
                        //品质-2
                        case "2":
                            bagItemIDType5_Str_Q2 = bagItemIDType5_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q2 = bagItemNumType5_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q2 = bagHideIDType5_Str_Q2 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q2 = bagItemParType5_Str_Q2 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q2 = bagGemHoleType5_Str_Q2 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q2 = bagGemIDType5_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType5_Str_Q3 = bagItemIDType5_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q3 = bagItemNumType5_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q3 = bagHideIDType5_Str_Q3 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q3 = bagItemParType5_Str_Q3 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q3 = bagGemHoleType5_Str_Q3 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q3 = bagGemIDType5_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType5_Str_Q4 = bagItemIDType5_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q4 = bagItemNumType5_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q4 = bagHideIDType5_Str_Q4 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q4 = bagItemParType5_Str_Q4 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q4 = bagGemHoleType5_Str_Q4 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q4 = bagGemIDType5_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType5_Str_Q5 = bagItemIDType5_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q5 = bagItemNumType5_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q5 = bagHideIDType5_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q5 = bagItemParType5_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q5 = bagGemHoleType5_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q5 = bagGemIDType5_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;

                        //品质-5
                        case "6":
                            bagItemIDType5_Str_Q5 = bagItemIDType5_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q5 = bagItemNumType5_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q5 = bagHideIDType5_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q5 = bagItemParType5_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q5 = bagGemHoleType5_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q5 = bagGemIDType5_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                    break;
            }
        }
        //Debug.Log("Time444:" + getTime());
        //拼接整理后的字符串
        string bagItemID_ArrangeStr;
        string bagItemNum_ArrangeStr;
        string bagItemHideID_ArrangeStr;
        string bagItemPar_ArrangeStr;
        string bagItemGemHole_ArrangeStr;
        string bagItemGemID_ArrangeStr;

        string[] bagItemID_Arrange;
        string[] bagItemNum_Arrange;
        string[] bagItemHideID_Arrange;
        string[] bagItemPar_Arrange;
        string[] bagItemGemHole_Arrange;
        string[] bagItemGemID_Arrange;

        bagItemID_ArrangeStr = bagItemIDType1_Str_Q5 + bagItemIDType1_Str_Q4 + bagItemIDType1_Str_Q3 + bagItemIDType1_Str_Q2 + bagItemIDType1_Str_Q1 + bagItemIDType2_Str_Q5 + bagItemIDType2_Str_Q4 + bagItemIDType2_Str_Q3 + bagItemIDType2_Str_Q2 + bagItemIDType2_Str_Q1 + bagItemIDType3_Str_Q5 + bagItemIDType3_Str_Q4 + bagItemIDType3_Str_Q3 + bagItemIDType3_Str_Q2 + bagItemIDType3_Str_Q1 + bagItemIDType4_Str_Q5 + bagItemIDType4_Str_Q4 + bagItemIDType4_Str_Q3 + bagItemIDType4_Str_Q2 + bagItemIDType4_Str_Q1 + bagItemIDType5_Str_Q5 + bagItemIDType5_Str_Q4 + bagItemIDType5_Str_Q3 + bagItemIDType5_Str_Q2 + bagItemIDType5_Str_Q1;
        bagItemNum_ArrangeStr = bagItemNumType1_Str_Q5 + bagItemNumType1_Str_Q4 + bagItemNumType1_Str_Q3 + bagItemNumType1_Str_Q2 + bagItemNumType1_Str_Q1 + bagItemNumType2_Str_Q5 + bagItemNumType2_Str_Q4 + bagItemNumType2_Str_Q3 + bagItemNumType2_Str_Q2 + bagItemNumType2_Str_Q1 + bagItemNumType3_Str_Q5 + bagItemNumType3_Str_Q4 + bagItemNumType3_Str_Q3 + bagItemNumType3_Str_Q2 + bagItemNumType3_Str_Q1 + bagItemNumType4_Str_Q5 + bagItemNumType4_Str_Q4 + bagItemNumType4_Str_Q3 + bagItemNumType4_Str_Q2 + bagItemNumType4_Str_Q1 + bagItemNumType5_Str_Q5 + bagItemNumType5_Str_Q4 + bagItemNumType5_Str_Q3 + bagItemNumType5_Str_Q2 + bagItemNumType5_Str_Q1;
        bagItemHideID_ArrangeStr = bagHideIDType1_Str_Q5 + bagHideIDType1_Str_Q4 + bagHideIDType1_Str_Q3 + bagHideIDType1_Str_Q2 + bagHideIDType1_Str_Q1 + bagHideIDType2_Str_Q5 + bagHideIDType2_Str_Q4 + bagHideIDType2_Str_Q3 + bagHideIDType2_Str_Q2 + bagHideIDType2_Str_Q1 + bagHideIDType3_Str_Q5 + bagHideIDType3_Str_Q4 + bagHideIDType3_Str_Q3 + bagHideIDType3_Str_Q2 + bagHideIDType3_Str_Q1 + bagHideIDType4_Str_Q5 + bagHideIDType4_Str_Q4 + bagHideIDType4_Str_Q3 + bagHideIDType4_Str_Q2 + bagHideIDType4_Str_Q1 + bagHideIDType5_Str_Q5 + bagHideIDType5_Str_Q4 + bagHideIDType5_Str_Q3 + bagHideIDType5_Str_Q2 + bagHideIDType5_Str_Q1;
        bagItemPar_ArrangeStr = bagItemParType1_Str_Q5 + bagItemParType1_Str_Q4 + bagItemParType1_Str_Q3 + bagItemParType1_Str_Q2 + bagItemParType1_Str_Q1 + bagItemParType2_Str_Q5 + bagItemParType2_Str_Q4 + bagItemParType2_Str_Q3 + bagItemParType2_Str_Q2 + bagItemParType2_Str_Q1 + bagItemParType3_Str_Q5 + bagItemParType3_Str_Q4 + bagItemParType3_Str_Q3 + bagItemParType3_Str_Q2 + bagItemParType3_Str_Q1 + bagItemParType4_Str_Q5 + bagItemParType4_Str_Q4 + bagItemParType4_Str_Q3 + bagItemParType4_Str_Q2 + bagItemParType4_Str_Q1 + bagItemParType5_Str_Q5 + bagItemParType5_Str_Q4 + bagItemParType5_Str_Q3 + bagItemParType5_Str_Q2 + bagItemParType5_Str_Q1;
        bagItemGemHole_ArrangeStr = bagGemHoleType1_Str_Q5 + bagGemHoleType1_Str_Q4 + bagGemHoleType1_Str_Q3 + bagGemHoleType1_Str_Q2 + bagGemHoleType1_Str_Q1 + bagGemHoleType2_Str_Q5 + bagGemHoleType2_Str_Q4 + bagGemHoleType2_Str_Q3 + bagGemHoleType2_Str_Q2 + bagGemHoleType2_Str_Q1 + bagGemHoleType3_Str_Q5 + bagGemHoleType3_Str_Q4 + bagGemHoleType3_Str_Q3 + bagGemHoleType3_Str_Q2 + bagGemHoleType3_Str_Q1 + bagGemHoleType4_Str_Q5 + bagGemHoleType4_Str_Q4 + bagGemHoleType4_Str_Q3 + bagGemHoleType4_Str_Q2 + bagGemHoleType4_Str_Q1 + bagGemHoleType5_Str_Q5 + bagGemHoleType5_Str_Q4 + bagGemHoleType5_Str_Q3 + bagGemHoleType5_Str_Q2 + bagGemHoleType5_Str_Q1;
        bagItemGemID_ArrangeStr = bagGemIDType1_Str_Q5 + bagGemIDType1_Str_Q4 + bagGemIDType1_Str_Q3 + bagGemIDType1_Str_Q2 + bagGemIDType1_Str_Q1 + bagGemIDType2_Str_Q5 + bagGemIDType2_Str_Q4 + bagGemIDType2_Str_Q3 + bagGemIDType2_Str_Q2 + bagGemIDType2_Str_Q1 + bagGemIDType3_Str_Q5 + bagGemIDType3_Str_Q4 + bagGemIDType3_Str_Q3 + bagGemIDType3_Str_Q2 + bagGemIDType3_Str_Q1 + bagGemIDType4_Str_Q5 + bagGemIDType4_Str_Q4 + bagGemIDType4_Str_Q3 + bagGemIDType4_Str_Q2 + bagGemIDType4_Str_Q1 + bagGemIDType5_Str_Q5 + bagGemIDType5_Str_Q4 + bagGemIDType5_Str_Q3 + bagGemIDType5_Str_Q2 + bagGemIDType5_Str_Q1;

        if (bagItemID_ArrangeStr != "")
        {
            //去掉背包隐藏属性ID
            //Debug.Log("bagItemID_ArrangeStr1 = " + bagItemID_ArrangeStr);
            bagItemID_ArrangeStr = bagItemID_ArrangeStr.Substring(0, bagItemID_ArrangeStr.Length - 1);
            bagItemNum_ArrangeStr = bagItemNum_ArrangeStr.Substring(0, bagItemNum_ArrangeStr.Length - 1);
            bagItemHideID_ArrangeStr = bagItemHideID_ArrangeStr.Substring(0, bagItemHideID_ArrangeStr.Length - 1);
            bagItemPar_ArrangeStr = bagItemPar_ArrangeStr.Substring(0, bagItemPar_ArrangeStr.Length - 1);
            bagItemGemHole_ArrangeStr = bagItemGemHole_ArrangeStr.Substring(0, bagItemGemHole_ArrangeStr.Length - 1);
            bagItemGemID_ArrangeStr = bagItemGemID_ArrangeStr.Substring(0, bagItemGemID_ArrangeStr.Length - 1);

            //转换成数组
            //Debug.Log("bagItemID_ArrangeStr2 = " + bagItemID_ArrangeStr);
            bagItemID_Arrange = bagItemID_ArrangeStr.Split(',');
            bagItemNum_Arrange = bagItemNum_ArrangeStr.Split(',');
            bagItemHideID_Arrange = bagItemHideID_ArrangeStr.Split(',');
            bagItemPar_Arrange = bagItemPar_ArrangeStr.Split('#');
            bagItemGemHole_Arrange = bagItemGemHole_ArrangeStr.Split('#');
            bagItemGemID_Arrange = bagItemGemID_ArrangeStr.Split('#');

            //Debug.Log("Lenght = " + bagItemID_Arrange.Length);
        }
        else {
            //Debug.Log("背包没有东西需要整理——2");
            return;
        }
        //Debug.Log("Time555:" + getTime());
        //循环写入背包数据
        for (int i = 1; i <= bagItemID_Arrange.Length; i++) {
            //Debug.Log("bagItemID_Arrange[i] = " + i+ bagItemID_Arrange[i]);
            function_DataSet.DataSet_WriteData("ItemID", bagItemID_Arrange[i-1], "ID", i.ToString(), "RoseBag");
            function_DataSet.DataSet_WriteData("ItemNum", bagItemNum_Arrange[i-1], "ID", i.ToString(), "RoseBag");
            function_DataSet.DataSet_WriteData("HideID", bagItemHideID_Arrange[i-1], "ID", i.ToString(), "RoseBag");
            function_DataSet.DataSet_WriteData("ItemPar", bagItemPar_Arrange[i-1], "ID", i.ToString(), "RoseBag");
            function_DataSet.DataSet_WriteData("GemHole", bagItemGemHole_Arrange[i-1], "ID", i.ToString(), "RoseBag");
            function_DataSet.DataSet_WriteData("GemID", bagItemGemID_Arrange[i-1], "ID", i.ToString(), "RoseBag");
            //Debug.Log("i = " + i);
        }
        //清空其他位置的数据
        if (bagItemID_Arrange.Length < Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum) {
            for (int i = bagItemID_Arrange.Length + 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
            {
                //清空当前格子数据
                function_DataSet.DataSet_WriteData("ItemID", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("GemHole", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("GemID", "0", "ID", i.ToString(), "RoseBag");
                //Debug.Log("i = " + i);
            }
        }

        //Debug.Log("Time666:" + getTime());
        function_DataSet.DataSet_SetXml("RoseBag");
        //Debug.Log("Time777:" + getTime());
        //Debug.Log("整理背包成功");
        Game_PublicClassVar.Get_function_UI.PlaySource("10004", "1");
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
    }

    //整理背包
    public void RoseArrangeBagEx()
    {
        //检索移动不成功的条件
        if (Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_340");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("洗练装备时禁止整理背包！");
            return;
        }

        Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_Status = false;        //整理背包的时候不要存东西

        //将掉落的道具ID添加到背包内
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        List<ItemModel> itemList = new List<ItemModel>();
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (Rdate == "" || Rdate == "0")
            {
                continue;
            }

            //读取当前道具数量 //防止整理背包出错,如果道具ID不为0,默认道具数量为1
            int itemNum = int.Parse(function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseBag"));
            itemNum = (itemNum == 0) ? 1 : itemNum;
            string hideID = function_DataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseBag");
            string itemPar = function_DataSet.DataSet_ReadData("ItemPar", "ID", i.ToString(), "RoseBag");
            string gemHole = function_DataSet.DataSet_ReadData("GemHole", "ID", i.ToString(), "RoseBag");
            string gemID = "";
            if (gemHole != "" && gemHole != "0")
            {
                gemID = function_DataSet.DataSet_ReadData("GemID", "ID", i.ToString(), "RoseBag");
            }

            //获得背包道具数量
            //获取道具背包最大堆叠数量
            //Debug.Log("Rdate = " + Rdate);
            int itemPileSum = int.Parse(function_DataSet.DataSet_ReadData("ItemPileSum", "ID", Rdate, "Item_Template"));

            bool addItemCell = false;
            ItemModel notFullCell = null;
            for ( int k = 0; k < itemList.Count; k++ )
            {
                if (itemList[k].ItemID == Rdate && itemList[k].ItemNum < itemPileSum)
                {
                    notFullCell = itemList[k];
                }
            }

            //若已存在相同ID则合并
            if (notFullCell != null)
            {
                int totalNumber = notFullCell.ItemNum + itemNum;
                if (totalNumber <= itemPileSum)
                {
                    notFullCell.ItemNum = totalNumber;
                }
                else
                {
                    notFullCell.ItemNum = itemPileSum;
                    itemNum = totalNumber - itemPileSum;
                    addItemCell = true;
                }
            }
            else
            {
                addItemCell = true;
            }

            if (addItemCell)
            {
                ItemModel itemNode = new ItemModel();
                itemNode.ItemID = Rdate;
                itemNode.ItemNum = itemNum;
                itemNode.HideID = hideID;
                itemNode.ItemPar = itemPar;
                itemNode.GemHole = gemHole;
                itemNode.GemID = gemID;
                itemNode.SetTypeAndQuality();
                itemList.Add(itemNode);
            }
        }
        itemList.Sort(delegate (ItemModel a, ItemModel b) {
            int a_type = a.GetItemType();
            int b_type = b.GetItemType();
            int a_quality = a.GetItemQuality();
            int b_quliaty = b.GetItemQuality();
            if (a_type == b_type)
            {
                if (a_quality == b_quliaty)
                {
                    return int.Parse(a.ItemID) - int.Parse(b.ItemID);
                }
                else
                {
                    return b_quliaty - a_quality;
                }
                
            }
            else
            {
                return a_type - b_type;
            } 
        });

        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            if ( i <= itemList.Count)
            {
                ItemModel itemNode = itemList[i - 1];
                function_DataSet.DataSet_WriteData("ItemID", itemNode.ItemID, "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("ItemNum", itemNode.ItemNum.ToString(), "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("HideID", itemNode.HideID, "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("ItemPar", itemNode.ItemPar, "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("GemHole", itemNode.GemHole, "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("GemID", itemNode.GemID, "ID", i.ToString(), "RoseBag");
            }
            else
            {
                //清空当前格子数据
                function_DataSet.DataSet_WriteData("ItemID", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("GemHole", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("GemID", "0", "ID", i.ToString(), "RoseBag");
            }
        }

        function_DataSet.DataSet_SetXml("RoseBag");
        //Debug.Log("Time777:" + getTime());
        //Debug.Log("整理背包成功");
        Game_PublicClassVar.Get_function_UI.PlaySource("10004", "1");
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
    }

    private string getTime() {
        string timeStr = ((System.DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
        return timeStr;
    }

    //整理仓库
    public void RoseArrangeStoreHouse(int storeHouseMin, int storeHouseMax)
    {
        bool useOld = false;
        if (useOld)
        {
            this.RoseArrangeStoreHouseOld(storeHouseMin, storeHouseMax);
        }
        else
        {
            this.RoseArrangeStoreHouseEx(storeHouseMin, storeHouseMax);
        }
    }

    public void RoseArrangeStoreHouseEx(int storeHouseMin, int storeHouseMax)
    {
        //检索移动不成功的条件
        if (Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_341");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("洗练装备时禁止整理仓库！");
            return;
        }
        
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        List<ItemModel> itemList = new List<ItemModel>();

        //将掉落的道具ID添加到背包内[]
        for (int i = storeHouseMin; i <= storeHouseMax; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseStoreHouse");
            if (Rdate == "" || Rdate == "0")
            {
                continue;
            }
            //读取当前道具数量
            int itemValue = int.Parse(function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseStoreHouse"));
            itemValue = (itemValue == 0) ? 1 : itemValue;
            string hideID = function_DataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseStoreHouse");
            string itemPar = function_DataSet.DataSet_ReadData("ItemPar", "ID", i.ToString(), "RoseStoreHouse");
            string item_GemHole = function_DataSet.DataSet_ReadData("GemHole", "ID", i.ToString(), "RoseStoreHouse");
            string item_GemID = function_DataSet.DataSet_ReadData("GemID", "ID", i.ToString(), "RoseStoreHouse");


            //获得背包道具数量
            //获取道具背包最大堆叠数量
            //Debug.Log("Rdate = " + Rdate);
            bool addCell = false;
            int itemPileSum = int.Parse(function_DataSet.DataSet_ReadData("ItemPileSum", "ID", Rdate, "Item_Template"));
            //能合并就合并，不能合并就新增
            ItemModel sameItem = null;
            for (int k = 0; k < itemList.Count; k++)
            {
                if (itemList[k].ItemID == Rdate && itemList[k].ItemNum < itemPileSum)
                {
                    sameItem = itemList[k];
                    break;
                }
            }

            if (sameItem != null)
            {
                int totalNumer = itemValue + sameItem.ItemNum;
                if (totalNumer <= itemPileSum)
                {
                    sameItem.ItemNum = totalNumer;
                }
                else
                {
                    sameItem.ItemNum = itemPileSum;
                    itemValue = totalNumer - itemPileSum;
                    addCell = true;
                }
            }
            else
            {
                addCell = true;
            }

            if (addCell)
            {
                ItemModel itemCell = new ItemModel();
                itemCell.ItemID = Rdate;
                itemCell.ItemNum = itemValue;
                itemCell.HideID = hideID;
                itemCell.ItemPar = itemPar;
                itemCell.GemHole = item_GemHole;
                itemCell.GemID = item_GemID;
                itemCell.SetTypeAndQuality();
                itemList.Add(itemCell);
            }
        }

        if (itemList.Count == 0)
        {
            return;
        }

        itemList.Sort(delegate (ItemModel a, ItemModel b) {
            int a_type = a.GetItemType();
            int b_type = b.GetItemType();
            int a_quality = a.GetItemQuality();
            int b_quliaty = b.GetItemQuality();
            if (a_type == b_type)
            {
                if (a_quality == b_quliaty)
                {
                    return int.Parse(a.ItemID) - int.Parse(b.ItemID);
                }
                else
                {
                    return b_quliaty - a_quality;
                }
            }
            else
            {
                return a_type - b_type;
            }
        });
        for (int i = storeHouseMin; i <= storeHouseMax; i++)
        {
            int index = i - storeHouseMin;
            if (index < itemList.Count)
            {
                function_DataSet.DataSet_WriteData("ItemID", itemList[index].ItemID, "ID",i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("ItemNum", itemList[index].ItemNum.ToString(), "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("HideID", itemList[index].HideID, "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("ItemPar", itemList[index].ItemPar, "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("GemHole", itemList[index].GemHole, "ID", i.ToString().ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("GemID", itemList[index].GemID, "ID", i.ToString().ToString(), "RoseStoreHouse");
            }
            else
            {
                function_DataSet.DataSet_WriteData("ItemID", "0", "ID", i.ToString().ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("GemHole", "0", "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("GemID", "0", "ID", i.ToString(), "RoseStoreHouse");
            }
        }

        function_DataSet.DataSet_SetXml("RoseStoreHouse");
        //Debug.Log("整理背包成功");
        Game_PublicClassVar.Get_function_UI.PlaySource("10004", "1");
        //Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
    }

    //整理仓库
    public void RoseArrangeStoreHouseOld(int storeHouseMin,int storeHouseMax)
    {

        //检索移动不成功的条件
        if (Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_341");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("洗练装备时禁止整理仓库！");
            return;
        }

        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        string bagItemIDStr = "";
        string bagItemNumStr = "";
        string bagItemHideIDStr = "";
        string bagItemParStr = "";
        string bagItemGemHoleStr = "";
        string bagItemGemIDStr = "";

        //将掉落的道具ID添加到背包内
        for (int i = storeHouseMin; i <= storeHouseMax; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseStoreHouse");
            if (Rdate != "" && Rdate != "0")
            {
                //读取当前道具数量
                int itemValue = int.Parse(function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseStoreHouse"));
                string hideID = function_DataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseStoreHouse");
                string itemPar = function_DataSet.DataSet_ReadData("ItemPar", "ID", i.ToString(), "RoseStoreHouse");
                string item_GemHole = function_DataSet.DataSet_ReadData("GemHole", "ID", i.ToString(), "RoseStoreHouse");
                string item_GemID = function_DataSet.DataSet_ReadData("GemID", "ID", i.ToString(), "RoseStoreHouse");


                //防止整理背包出错,如果道具ID不为0,默认道具数量为1
                if (itemValue == 0)
                {
                    itemValue = 1;
                }

                //获得背包道具数量
                //获取道具背包最大堆叠数量
                //Debug.Log("Rdate = " + Rdate);
                int itemPileSum = int.Parse(function_DataSet.DataSet_ReadData("ItemPileSum", "ID", Rdate, "Item_Template"));
                if (itemPileSum > itemValue)
                {
                    //向背包后面道具查询数量
                    for (int y = i + 1; y <= storeHouseMax; y++)
                    {
                        //对比道具ID
                        string itemID_DuiBi = function_DataSet.DataSet_ReadData("ItemID", "ID", y.ToString(), "RoseStoreHouse");
                        if (Rdate == itemID_DuiBi)
                        {
                            //道具ID相同获取数量
                            int itemNum_DuiBi = int.Parse(function_DataSet.DataSet_ReadData("ItemNum", "ID", y.ToString(), "RoseStoreHouse"));

                            //数量相加判定是否满足前面堆叠数量
                            int itemSunNum_DuiBi = itemValue + itemNum_DuiBi;
                            if (itemSunNum_DuiBi >= itemPileSum)
                            {
                                int itemCostNum_DuiBi = itemSunNum_DuiBi - itemPileSum;
                                itemValue = itemPileSum;
                                //当满足上一次的堆叠数量自己还有剩余数量时进行记录
                                if (itemCostNum_DuiBi > 0)
                                {
                                    function_DataSet.DataSet_WriteData("ItemNum", itemCostNum_DuiBi.ToString(), "ID", y.ToString(), "RoseStoreHouse");
                                }
                                else
                                {
                                    function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", y.ToString(), "RoseStoreHouse");
                                    function_DataSet.DataSet_WriteData("ItemID", "0", "ID", y.ToString(), "RoseStoreHouse");
                                }

                                if (itemCostNum_DuiBi >= 0)
                                {
                                    y = storeHouseMax; //跳出循环
                                }
                            }
                            else
                            {
                                itemValue = itemValue + itemNum_DuiBi;
                                function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", y.ToString(), "RoseStoreHouse");
                                function_DataSet.DataSet_WriteData("ItemID", "0", "ID", y.ToString(), "RoseStoreHouse");
                            }
                        }
                    }
                }

                //背包整理字符串累计
                bagItemIDStr = bagItemIDStr + Rdate + ",";
                bagItemNumStr = bagItemNumStr + itemValue + ",";
                bagItemHideIDStr = bagItemHideIDStr + hideID + ",";
                bagItemParStr = bagItemParStr + itemPar + "#";
                bagItemGemHoleStr = bagItemGemHoleStr + item_GemHole + "#";
                bagItemGemIDStr = bagItemGemIDStr + item_GemID + "#";

                //清空当前格子数据
                function_DataSet.DataSet_WriteData("ItemID", "0", "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("GemHole", "0", "ID", i.ToString(), "RoseStoreHouse");
                function_DataSet.DataSet_WriteData("GemID", "0", "ID", i.ToString(), "RoseStoreHouse");
            }
        }
        if (bagItemIDStr != "")
        {
            //Debug.Log("bagItemIDStr1 = " + bagItemIDStr);
            bagItemIDStr = bagItemIDStr.Substring(0, bagItemIDStr.Length - 1);
            bagItemNumStr = bagItemNumStr.Substring(0, bagItemNumStr.Length - 1);
            //Debug.Log("bagItemIDStr2 = " + bagItemIDStr);
        }
        //存储当前背包道具ID和数量
        string[] bagItemID_Now;
        string[] bagItemNum_Now;
        string[] bagItemHide_Now;
        string[] bagItemPar_Now;
        string[] bagItemGemHole_Now;
        string[] bagItemGemID_Now;

        if (bagItemIDStr != "")
        {
            bagItemID_Now = bagItemIDStr.Split(',');
            bagItemNum_Now = bagItemNumStr.Split(',');
            bagItemHide_Now = bagItemHideIDStr.Split(',');
            bagItemPar_Now = bagItemParStr.Split('#');
            bagItemGemHole_Now = bagItemGemHoleStr.Split('#');
            bagItemGemID_Now = bagItemGemIDStr.Split('#');
        }
        else
        {
            //Debug.Log("当前背包没有道具,不需要整理");
            return;
        }
        //整理当前背包道具数量
        //--消耗性道具
        string bagItemIDType1_Str_Q1 = "";
        string bagItemIDType1_Str_Q2 = "";
        string bagItemIDType1_Str_Q3 = "";
        string bagItemIDType1_Str_Q4 = "";
        string bagItemIDType1_Str_Q5 = "";
        string bagItemNumType1_Str_Q1 = "";
        string bagItemNumType1_Str_Q2 = "";
        string bagItemNumType1_Str_Q3 = "";
        string bagItemNumType1_Str_Q4 = "";
        string bagItemNumType1_Str_Q5 = "";
        string bagHideIDType1_Str_Q1 = "";
        string bagHideIDType1_Str_Q2 = "";
        string bagHideIDType1_Str_Q3 = "";
        string bagHideIDType1_Str_Q4 = "";
        string bagHideIDType1_Str_Q5 = "";
        string bagItemParType1_Str_Q1 = "";
        string bagItemParType1_Str_Q2 = "";
        string bagItemParType1_Str_Q3 = "";
        string bagItemParType1_Str_Q4 = "";
        string bagItemParType1_Str_Q5 = "";
        string bagGemHoleType1_Str_Q1 = "";
        string bagGemHoleType1_Str_Q2 = "";
        string bagGemHoleType1_Str_Q3 = "";
        string bagGemHoleType1_Str_Q4 = "";
        string bagGemHoleType1_Str_Q5 = "";
        string bagGemIDType1_Str_Q1 = "";
        string bagGemIDType1_Str_Q2 = "";
        string bagGemIDType1_Str_Q3 = "";
        string bagGemIDType1_Str_Q4 = "";
        string bagGemIDType1_Str_Q5 = "";

        //--材料道具
        string bagItemIDType2_Str_Q1 = "";
        string bagItemIDType2_Str_Q2 = "";
        string bagItemIDType2_Str_Q3 = "";
        string bagItemIDType2_Str_Q4 = "";
        string bagItemIDType2_Str_Q5 = "";
        string bagItemNumType2_Str_Q1 = "";
        string bagItemNumType2_Str_Q2 = "";
        string bagItemNumType2_Str_Q3 = "";
        string bagItemNumType2_Str_Q4 = "";
        string bagItemNumType2_Str_Q5 = "";
        string bagHideIDType2_Str_Q1 = "";
        string bagHideIDType2_Str_Q2 = "";
        string bagHideIDType2_Str_Q3 = "";
        string bagHideIDType2_Str_Q4 = "";
        string bagHideIDType2_Str_Q5 = "";
        string bagItemParType2_Str_Q1 = "";
        string bagItemParType2_Str_Q2 = "";
        string bagItemParType2_Str_Q3 = "";
        string bagItemParType2_Str_Q4 = "";
        string bagItemParType2_Str_Q5 = "";
        string bagGemHoleType2_Str_Q1 = "";
        string bagGemHoleType2_Str_Q2 = "";
        string bagGemHoleType2_Str_Q3 = "";
        string bagGemHoleType2_Str_Q4 = "";
        string bagGemHoleType2_Str_Q5 = "";
        string bagGemIDType2_Str_Q1 = "";
        string bagGemIDType2_Str_Q2 = "";
        string bagGemIDType2_Str_Q3 = "";
        string bagGemIDType2_Str_Q4 = "";
        string bagGemIDType2_Str_Q5 = "";

        //--装备道具
        string bagItemIDType3_Str_Q1 = "";
        string bagItemIDType3_Str_Q2 = "";
        string bagItemIDType3_Str_Q3 = "";
        string bagItemIDType3_Str_Q4 = "";
        string bagItemIDType3_Str_Q5 = "";
        string bagItemNumType3_Str_Q1 = "";
        string bagItemNumType3_Str_Q2 = "";
        string bagItemNumType3_Str_Q3 = "";
        string bagItemNumType3_Str_Q4 = "";
        string bagItemNumType3_Str_Q5 = "";
        string bagHideIDType3_Str_Q1 = "";
        string bagHideIDType3_Str_Q2 = "";
        string bagHideIDType3_Str_Q3 = "";
        string bagHideIDType3_Str_Q4 = "";
        string bagHideIDType3_Str_Q5 = "";
        string bagItemParType3_Str_Q1 = "";
        string bagItemParType3_Str_Q2 = "";
        string bagItemParType3_Str_Q3 = "";
        string bagItemParType3_Str_Q4 = "";
        string bagItemParType3_Str_Q5 = "";
        string bagGemHoleType3_Str_Q1 = "";
        string bagGemHoleType3_Str_Q2 = "";
        string bagGemHoleType3_Str_Q3 = "";
        string bagGemHoleType3_Str_Q4 = "";
        string bagGemHoleType3_Str_Q5 = "";
        string bagGemIDType3_Str_Q1 = "";
        string bagGemIDType3_Str_Q2 = "";
        string bagGemIDType3_Str_Q3 = "";
        string bagGemIDType3_Str_Q4 = "";
        string bagGemIDType3_Str_Q5 = "";

        //--宝石道具
        string bagItemIDType4_Str_Q1 = "";
        string bagItemIDType4_Str_Q2 = "";
        string bagItemIDType4_Str_Q3 = "";
        string bagItemIDType4_Str_Q4 = "";
        string bagItemIDType4_Str_Q5 = "";
        string bagItemNumType4_Str_Q1 = "";
        string bagItemNumType4_Str_Q2 = "";
        string bagItemNumType4_Str_Q3 = "";
        string bagItemNumType4_Str_Q4 = "";
        string bagItemNumType4_Str_Q5 = "";
        string bagHideIDType4_Str_Q1 = "";
        string bagHideIDType4_Str_Q2 = "";
        string bagHideIDType4_Str_Q3 = "";
        string bagHideIDType4_Str_Q4 = "";
        string bagHideIDType4_Str_Q5 = "";
        string bagItemParType4_Str_Q1 = "";
        string bagItemParType4_Str_Q2 = "";
        string bagItemParType4_Str_Q3 = "";
        string bagItemParType4_Str_Q4 = "";
        string bagItemParType4_Str_Q5 = "";
        string bagGemHoleType4_Str_Q1 = "";
        string bagGemHoleType4_Str_Q2 = "";
        string bagGemHoleType4_Str_Q3 = "";
        string bagGemHoleType4_Str_Q4 = "";
        string bagGemHoleType4_Str_Q5 = "";
        string bagGemIDType4_Str_Q1 = "";
        string bagGemIDType4_Str_Q2 = "";
        string bagGemIDType4_Str_Q3 = "";
        string bagGemIDType4_Str_Q4 = "";
        string bagGemIDType4_Str_Q5 = "";

        //--被动技能道具
        string bagItemIDType5_Str_Q1 = "";
        string bagItemIDType5_Str_Q2 = "";
        string bagItemIDType5_Str_Q3 = "";
        string bagItemIDType5_Str_Q4 = "";
        string bagItemIDType5_Str_Q5 = "";
        string bagItemNumType5_Str_Q1 = "";
        string bagItemNumType5_Str_Q2 = "";
        string bagItemNumType5_Str_Q3 = "";
        string bagItemNumType5_Str_Q4 = "";
        string bagItemNumType5_Str_Q5 = "";
        string bagHideIDType5_Str_Q1 = "";
        string bagHideIDType5_Str_Q2 = "";
        string bagHideIDType5_Str_Q3 = "";
        string bagHideIDType5_Str_Q4 = "";
        string bagHideIDType5_Str_Q5 = "";
        string bagItemParType5_Str_Q1 = "";
        string bagItemParType5_Str_Q2 = "";
        string bagItemParType5_Str_Q3 = "";
        string bagItemParType5_Str_Q4 = "";
        string bagItemParType5_Str_Q5 = "";
        string bagGemHoleType5_Str_Q1 = "";
        string bagGemHoleType5_Str_Q2 = "";
        string bagGemHoleType5_Str_Q3 = "";
        string bagGemHoleType5_Str_Q4 = "";
        string bagGemHoleType5_Str_Q5 = "";
        string bagGemIDType5_Str_Q1 = "";
        string bagGemIDType5_Str_Q2 = "";
        string bagGemIDType5_Str_Q3 = "";
        string bagGemIDType5_Str_Q4 = "";
        string bagGemIDType5_Str_Q5 = "";


        for (int i = 0; i <= bagItemID_Now.Length - 1; i++)
        {
            //获取道具类型和品质
            string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", bagItemID_Now[i], "Item_Template");
            string itemQuality = function_DataSet.DataSet_ReadData("ItemQuality", "ID", bagItemID_Now[i], "Item_Template");
            switch (itemType)
            {
                //消耗性道具
                case "1":
                    switch (itemQuality)
                    {
                        //品质-1
                        case "1":
                            bagItemIDType1_Str_Q1 = bagItemIDType1_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q1 = bagItemNumType1_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q1 = bagHideIDType1_Str_Q1 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q1 = bagItemParType1_Str_Q1 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q1 = bagGemHoleType1_Str_Q1 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q1 = bagGemIDType1_Str_Q1 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-2
                        case "2":
                            bagItemIDType1_Str_Q2 = bagItemIDType1_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q2 = bagItemNumType1_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q2 = bagHideIDType1_Str_Q2 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q2 = bagItemParType1_Str_Q2 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q2 = bagGemHoleType1_Str_Q2 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q2 = bagGemIDType1_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType1_Str_Q3 = bagItemIDType1_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q3 = bagItemNumType1_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q3 = bagHideIDType1_Str_Q3 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q3 = bagItemParType1_Str_Q3 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q3 = bagGemHoleType1_Str_Q3 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q3 = bagGemIDType1_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType1_Str_Q4 = bagItemIDType1_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q4 = bagItemNumType1_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q4 = bagHideIDType1_Str_Q4 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q4 = bagItemParType1_Str_Q4 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q4 = bagGemHoleType1_Str_Q4 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q4 = bagGemIDType1_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType1_Str_Q5 = bagItemIDType1_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q5 = bagItemNumType1_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q5 = bagHideIDType1_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q5 = bagItemParType1_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q5 = bagGemHoleType1_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q5 = bagGemIDType1_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;

                        //品质-5
                        case "6":
                            bagItemIDType1_Str_Q5 = bagItemIDType1_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q5 = bagItemNumType1_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q5 = bagHideIDType1_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType1_Str_Q5 = bagItemParType1_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType1_Str_Q5 = bagGemHoleType1_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType1_Str_Q5 = bagGemIDType1_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                    break;
                //材料
                case "2":
                    switch (itemQuality)
                    {
                        //品质-1
                        case "1":
                            bagItemIDType2_Str_Q1 = bagItemIDType2_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q1 = bagItemNumType2_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q1 = bagHideIDType2_Str_Q1 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q1 = bagItemParType2_Str_Q1 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q1 = bagGemHoleType2_Str_Q1 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q1 = bagGemIDType2_Str_Q1 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-2
                        case "2":
                            bagItemIDType2_Str_Q2 = bagItemIDType2_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q2 = bagItemNumType2_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q2 = bagHideIDType2_Str_Q2 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q2 = bagItemParType2_Str_Q2 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q2 = bagGemHoleType2_Str_Q2 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q2 = bagGemIDType2_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType2_Str_Q3 = bagItemIDType2_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q3 = bagItemNumType2_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q3 = bagHideIDType2_Str_Q3 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q3 = bagItemParType2_Str_Q3 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q3 = bagGemHoleType2_Str_Q3 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q3 = bagGemIDType2_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType2_Str_Q4 = bagItemIDType2_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q4 = bagItemNumType2_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q4 = bagHideIDType2_Str_Q4 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q4 = bagItemParType2_Str_Q4 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q4 = bagGemHoleType2_Str_Q4 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q4 = bagGemIDType2_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType2_Str_Q5 = bagItemIDType2_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q5 = bagItemNumType2_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q5 = bagHideIDType2_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q5 = bagItemParType2_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q5 = bagGemHoleType2_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q5 = bagGemIDType2_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "6":
                            bagItemIDType2_Str_Q5 = bagItemIDType2_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q5 = bagItemNumType2_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q5 = bagHideIDType2_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType2_Str_Q5 = bagItemParType2_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType2_Str_Q5 = bagGemHoleType2_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType2_Str_Q5 = bagGemIDType2_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                    break;
                //装备
                case "3":

                    switch (itemQuality)
                    {
                        //品质-1
                        case "1":
                            bagItemIDType3_Str_Q1 = bagItemIDType3_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q1 = bagItemNumType3_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q1 = bagHideIDType3_Str_Q1 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q1 = bagItemParType3_Str_Q1 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q1 = bagGemHoleType3_Str_Q1 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q1 = bagGemIDType3_Str_Q1 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-2
                        case "2":
                            bagItemIDType3_Str_Q2 = bagItemIDType3_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q2 = bagItemNumType3_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q2 = bagHideIDType3_Str_Q2 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q2 = bagItemParType3_Str_Q2 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q2 = bagGemHoleType3_Str_Q2 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q2 = bagGemIDType3_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType3_Str_Q3 = bagItemIDType3_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q3 = bagItemNumType3_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q3 = bagHideIDType3_Str_Q3 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q3 = bagItemParType3_Str_Q3 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q3 = bagGemHoleType3_Str_Q3 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q3 = bagGemIDType3_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType3_Str_Q4 = bagItemIDType3_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q4 = bagItemNumType3_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q4 = bagHideIDType3_Str_Q4 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q4 = bagItemParType3_Str_Q4 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q4 = bagGemHoleType3_Str_Q4 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q4 = bagGemIDType3_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType3_Str_Q5 = bagItemIDType3_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q5 = bagItemNumType3_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q5 = bagHideIDType3_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q5 = bagItemParType3_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q5 = bagGemHoleType3_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q5 = bagGemIDType3_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "6":
                            bagItemIDType3_Str_Q5 = bagItemIDType3_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q5 = bagItemNumType3_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q5 = bagHideIDType3_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType3_Str_Q5 = bagItemParType3_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType3_Str_Q5 = bagGemHoleType3_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType3_Str_Q5 = bagGemIDType3_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                    break;

                //宝石
                case "4":

                    switch (itemQuality)
                    {
                        //品质-1
                        case "1":
                            bagItemIDType4_Str_Q1 = bagItemIDType4_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType4_Str_Q1 = bagItemNumType4_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType4_Str_Q1 = bagHideIDType4_Str_Q1 + bagItemHide_Now[i] + ",";
                            bagItemParType4_Str_Q1 = bagItemParType4_Str_Q1 + bagItemPar_Now[i] + "#";
                            bagGemHoleType4_Str_Q1 = bagGemHoleType4_Str_Q1 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType4_Str_Q1 = bagGemIDType4_Str_Q1 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-2
                        case "2":
                            bagItemIDType4_Str_Q2 = bagItemIDType4_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType4_Str_Q2 = bagItemNumType4_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType4_Str_Q2 = bagHideIDType4_Str_Q2 + bagItemHide_Now[i] + ",";
                            bagItemParType4_Str_Q2 = bagItemParType4_Str_Q2 + bagItemPar_Now[i] + "#";
                            bagGemHoleType4_Str_Q2 = bagGemHoleType4_Str_Q2 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType4_Str_Q2 = bagGemIDType4_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType4_Str_Q3 = bagItemIDType4_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType4_Str_Q3 = bagItemNumType4_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType4_Str_Q3 = bagHideIDType4_Str_Q3 + bagItemHide_Now[i] + ",";
                            bagItemParType4_Str_Q3 = bagItemParType4_Str_Q3 + bagItemPar_Now[i] + "#";
                            bagGemHoleType4_Str_Q3 = bagGemHoleType4_Str_Q3 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType4_Str_Q3 = bagGemIDType4_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType4_Str_Q4 = bagItemIDType4_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType4_Str_Q4 = bagItemNumType4_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType4_Str_Q4 = bagHideIDType4_Str_Q4 + bagItemHide_Now[i] + ",";
                            bagItemParType4_Str_Q4 = bagItemParType4_Str_Q4 + bagItemPar_Now[i] + "#";
                            bagGemHoleType4_Str_Q4 = bagGemHoleType4_Str_Q4 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType4_Str_Q4 = bagGemIDType4_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType4_Str_Q5 = bagItemIDType4_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType4_Str_Q5 = bagItemNumType4_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType4_Str_Q5 = bagHideIDType4_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType4_Str_Q5 = bagItemParType4_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType4_Str_Q5 = bagGemHoleType4_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType4_Str_Q5 = bagGemIDType4_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "6":
                            bagItemIDType4_Str_Q5 = bagItemIDType4_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType4_Str_Q5 = bagItemNumType4_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType4_Str_Q5 = bagHideIDType4_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType4_Str_Q5 = bagItemParType4_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType4_Str_Q5 = bagGemHoleType4_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType4_Str_Q5 = bagGemIDType4_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                    break;

                //被动技能
                case "5":

                    switch (itemQuality)
                    {
                        //品质-1
                        case "1":
                            bagItemIDType5_Str_Q1 = bagItemIDType5_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q1 = bagItemNumType5_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q1 = bagHideIDType5_Str_Q1 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q1 = bagItemParType5_Str_Q1 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q1 = bagGemHoleType5_Str_Q1 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q1 = bagGemIDType5_Str_Q1 + bagItemGemID_Now[i] + "#";

                            break;
                        //品质-2
                        case "2":
                            bagItemIDType5_Str_Q2 = bagItemIDType5_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q2 = bagItemNumType5_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q2 = bagHideIDType5_Str_Q2 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q2 = bagItemParType5_Str_Q2 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q2 = bagGemHoleType5_Str_Q2 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q2 = bagGemIDType5_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType5_Str_Q3 = bagItemIDType5_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q3 = bagItemNumType5_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q3 = bagHideIDType5_Str_Q3 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q3 = bagItemParType5_Str_Q3 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q3 = bagGemHoleType5_Str_Q3 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q3 = bagGemIDType5_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType5_Str_Q4 = bagItemIDType5_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q4 = bagItemNumType5_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q4 = bagHideIDType5_Str_Q4 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q4 = bagItemParType5_Str_Q4 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q4 = bagGemHoleType5_Str_Q4 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q4 = bagGemIDType5_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType5_Str_Q5 = bagItemIDType5_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q5 = bagItemNumType5_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q5 = bagHideIDType5_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q5 = bagItemParType5_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q5 = bagGemHoleType5_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q5 = bagGemIDType5_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "6":
                            bagItemIDType5_Str_Q5 = bagItemIDType5_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q5 = bagItemNumType5_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q5 = bagHideIDType5_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q5 = bagItemParType5_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q5 = bagGemHoleType5_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q5 = bagGemIDType5_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                    break;


                //被动技能
                default:

                    switch (itemQuality)
                    {
                        //品质-1
                        case "1":
                            bagItemIDType5_Str_Q1 = bagItemIDType5_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q1 = bagItemNumType5_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q1 = bagHideIDType5_Str_Q1 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q1 = bagItemParType5_Str_Q1 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q1 = bagGemHoleType5_Str_Q1 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q1 = bagGemIDType5_Str_Q1 + bagItemGemID_Now[i] + "#";

                            break;
                        //品质-2
                        case "2":
                            bagItemIDType5_Str_Q2 = bagItemIDType5_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q2 = bagItemNumType5_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q2 = bagHideIDType5_Str_Q2 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q2 = bagItemParType5_Str_Q2 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q2 = bagGemHoleType5_Str_Q2 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q2 = bagGemIDType5_Str_Q2 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType5_Str_Q3 = bagItemIDType5_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q3 = bagItemNumType5_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q3 = bagHideIDType5_Str_Q3 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q3 = bagItemParType5_Str_Q3 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q3 = bagGemHoleType5_Str_Q3 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q3 = bagGemIDType5_Str_Q3 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType5_Str_Q4 = bagItemIDType5_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q4 = bagItemNumType5_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q4 = bagHideIDType5_Str_Q4 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q4 = bagItemParType5_Str_Q4 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q4 = bagGemHoleType5_Str_Q4 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q4 = bagGemIDType5_Str_Q4 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType5_Str_Q5 = bagItemIDType5_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q5 = bagItemNumType5_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q5 = bagHideIDType5_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q5 = bagItemParType5_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q5 = bagGemHoleType5_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q5 = bagGemIDType5_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                        //品质-5
                        case "6":
                            bagItemIDType5_Str_Q5 = bagItemIDType5_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType5_Str_Q5 = bagItemNumType5_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType5_Str_Q5 = bagHideIDType5_Str_Q5 + bagItemHide_Now[i] + ",";
                            bagItemParType5_Str_Q5 = bagItemParType5_Str_Q5 + bagItemPar_Now[i] + "#";
                            bagGemHoleType5_Str_Q5 = bagGemHoleType5_Str_Q5 + bagItemGemHole_Now[i] + "#";
                            bagGemIDType5_Str_Q5 = bagGemIDType5_Str_Q5 + bagItemGemID_Now[i] + "#";
                            break;
                    }
                    break;
            }
        }

        //拼接整理后的字符串
        string bagItemID_ArrangeStr;
        string bagItemNum_ArrangeStr;
        string bagItemHideID_ArrangeStr;
        string bagItemPar_ArrangeStr;
        string bagItemGemHole_ArrangeStr;
        string bagItemGemID_ArrangeStr;

        string[] bagItemID_Arrange;
        string[] bagItemNum_Arrange;
        string[] bagItemHideID_Arrange;
        string[] bagItemPar_Arrange;
        string[] bagItemGemHole_Arrange;
        string[] bagItemGemID_Arrange;

        bagItemID_ArrangeStr = bagItemIDType1_Str_Q5 + bagItemIDType1_Str_Q4 + bagItemIDType1_Str_Q3 + bagItemIDType1_Str_Q2 + bagItemIDType1_Str_Q1 + bagItemIDType2_Str_Q5 + bagItemIDType2_Str_Q4 + bagItemIDType2_Str_Q3 + bagItemIDType2_Str_Q2 + bagItemIDType2_Str_Q1 + bagItemIDType3_Str_Q5 + bagItemIDType3_Str_Q4 + bagItemIDType3_Str_Q3 + bagItemIDType3_Str_Q2 + bagItemIDType3_Str_Q1 + bagItemIDType4_Str_Q5 + bagItemIDType4_Str_Q4 + bagItemIDType4_Str_Q3 + bagItemIDType4_Str_Q2 + bagItemIDType4_Str_Q1 + bagItemIDType5_Str_Q5 + bagItemIDType5_Str_Q4 + bagItemIDType5_Str_Q3 + bagItemIDType5_Str_Q2 + bagItemIDType5_Str_Q1;
        bagItemNum_ArrangeStr = bagItemNumType1_Str_Q5 + bagItemNumType1_Str_Q4 + bagItemNumType1_Str_Q3 + bagItemNumType1_Str_Q2 + bagItemNumType1_Str_Q1 + bagItemNumType2_Str_Q5 + bagItemNumType2_Str_Q4 + bagItemNumType2_Str_Q3 + bagItemNumType2_Str_Q2 + bagItemNumType2_Str_Q1 + bagItemNumType3_Str_Q5 + bagItemNumType3_Str_Q4 + bagItemNumType3_Str_Q3 + bagItemNumType3_Str_Q2 + bagItemNumType3_Str_Q1 + bagItemNumType4_Str_Q5 + bagItemNumType4_Str_Q4 + bagItemNumType4_Str_Q3 + bagItemNumType4_Str_Q2 + bagItemNumType4_Str_Q1 + bagItemNumType5_Str_Q5 + bagItemNumType5_Str_Q4 + bagItemNumType5_Str_Q3 + bagItemNumType5_Str_Q2 + bagItemNumType5_Str_Q1;
        bagItemHideID_ArrangeStr = bagHideIDType1_Str_Q5 + bagHideIDType1_Str_Q4 + bagHideIDType1_Str_Q3 + bagHideIDType1_Str_Q2 + bagHideIDType1_Str_Q1 + bagHideIDType2_Str_Q5 + bagHideIDType2_Str_Q4 + bagHideIDType2_Str_Q3 + bagHideIDType2_Str_Q2 + bagHideIDType2_Str_Q1 + bagHideIDType3_Str_Q5 + bagHideIDType3_Str_Q4 + bagHideIDType3_Str_Q3 + bagHideIDType3_Str_Q2 + bagHideIDType3_Str_Q1 + bagHideIDType4_Str_Q5 + bagHideIDType4_Str_Q4 + bagHideIDType4_Str_Q3 + bagHideIDType4_Str_Q2 + bagHideIDType4_Str_Q1 + bagHideIDType5_Str_Q5 + bagHideIDType5_Str_Q4 + bagHideIDType5_Str_Q3 + bagHideIDType5_Str_Q2 + bagHideIDType5_Str_Q1;
        bagItemPar_ArrangeStr = bagItemParType1_Str_Q5 + bagItemParType1_Str_Q4 + bagItemParType1_Str_Q3 + bagItemParType1_Str_Q2 + bagItemParType1_Str_Q1 + bagItemParType2_Str_Q5 + bagItemParType2_Str_Q4 + bagItemParType2_Str_Q3 + bagItemParType2_Str_Q2 + bagItemParType2_Str_Q1 + bagItemParType3_Str_Q5 + bagItemParType3_Str_Q4 + bagItemParType3_Str_Q3 + bagItemParType3_Str_Q2 + bagItemParType3_Str_Q1 + bagItemParType4_Str_Q5 + bagItemParType4_Str_Q4 + bagItemParType4_Str_Q3 + bagItemParType4_Str_Q2 + bagItemParType4_Str_Q1 + bagItemParType5_Str_Q5 + bagItemParType5_Str_Q4 + bagItemParType5_Str_Q3 + bagItemParType5_Str_Q2 + bagItemParType5_Str_Q1;
        bagItemGemHole_ArrangeStr = bagGemHoleType1_Str_Q5 + bagGemHoleType1_Str_Q4 + bagGemHoleType1_Str_Q3 + bagGemHoleType1_Str_Q2 + bagGemHoleType1_Str_Q1 + bagGemHoleType2_Str_Q5 + bagGemHoleType2_Str_Q4 + bagGemHoleType2_Str_Q3 + bagGemHoleType2_Str_Q2 + bagGemHoleType2_Str_Q1 + bagGemHoleType3_Str_Q5 + bagGemHoleType3_Str_Q4 + bagGemHoleType3_Str_Q3 + bagGemHoleType3_Str_Q2 + bagGemHoleType3_Str_Q1 + bagGemHoleType4_Str_Q5 + bagGemHoleType4_Str_Q4 + bagGemHoleType4_Str_Q3 + bagGemHoleType4_Str_Q2 + bagGemHoleType4_Str_Q1 + bagGemHoleType5_Str_Q5 + bagGemHoleType5_Str_Q4 + bagGemHoleType5_Str_Q3 + bagGemHoleType5_Str_Q2 + bagGemHoleType5_Str_Q1;
        bagItemGemID_ArrangeStr = bagGemIDType1_Str_Q5 + bagGemIDType1_Str_Q4 + bagGemIDType1_Str_Q3 + bagGemIDType1_Str_Q2 + bagGemIDType1_Str_Q1 + bagGemIDType2_Str_Q5 + bagGemIDType2_Str_Q4 + bagGemIDType2_Str_Q3 + bagGemIDType2_Str_Q2 + bagGemIDType2_Str_Q1 + bagGemIDType3_Str_Q5 + bagGemIDType3_Str_Q4 + bagGemIDType3_Str_Q3 + bagGemIDType3_Str_Q2 + bagGemIDType3_Str_Q1 + bagGemIDType4_Str_Q5 + bagGemIDType4_Str_Q4 + bagGemIDType4_Str_Q3 + bagGemIDType4_Str_Q2 + bagGemIDType4_Str_Q1 + bagGemIDType5_Str_Q5 + bagGemIDType5_Str_Q4 + bagGemIDType5_Str_Q3 + bagGemIDType5_Str_Q2 + bagGemIDType5_Str_Q1;

        if (bagItemID_ArrangeStr != "")
        {
            //去掉背包隐藏属性ID
            //Debug.Log("bagItemID_ArrangeStr1 = " + bagItemID_ArrangeStr);
            bagItemID_ArrangeStr = bagItemID_ArrangeStr.Substring(0, bagItemID_ArrangeStr.Length - 1);
            bagItemNum_ArrangeStr = bagItemNum_ArrangeStr.Substring(0, bagItemNum_ArrangeStr.Length - 1);
            bagItemHideID_ArrangeStr = bagItemHideID_ArrangeStr.Substring(0, bagItemHideID_ArrangeStr.Length - 1);
            bagItemPar_ArrangeStr = bagItemPar_ArrangeStr.Substring(0, bagItemPar_ArrangeStr.Length - 1);
            bagItemGemHole_ArrangeStr = bagItemGemHole_ArrangeStr.Substring(0, bagItemGemHole_ArrangeStr.Length - 1);
            bagItemGemID_ArrangeStr = bagItemGemID_ArrangeStr.Substring(0, bagItemGemID_ArrangeStr.Length - 1);

            //转换成数组
            //Debug.Log("bagItemID_ArrangeStr2 = " + bagItemID_ArrangeStr);
            bagItemID_Arrange = bagItemID_ArrangeStr.Split(',');
            bagItemNum_Arrange = bagItemNum_ArrangeStr.Split(',');
            bagItemHideID_Arrange = bagItemHideID_ArrangeStr.Split(',');
            bagItemPar_Arrange = bagItemPar_ArrangeStr.Split('#');
            bagItemGemHole_Arrange = bagItemGemHole_ArrangeStr.Split('#');
            bagItemGemID_Arrange = bagItemGemID_ArrangeStr.Split('#');

            //Debug.Log("Lenght = " + bagItemID_Arrange.Length);
        }
        else
        {
            //Debug.Log("背包没有东西需要整理——2");
            return;
        }

        //循环写入背包数据
        for (int i = 0; i <= bagItemID_Arrange.Length - 1; i++)
        {
            //Debug.Log("bagItemID_Arrange[i] = " + i+ bagItemID_Arrange[i]);
            function_DataSet.DataSet_WriteData("ItemID", bagItemID_Arrange[i], "ID", (storeHouseMin + i).ToString(), "RoseStoreHouse");
            function_DataSet.DataSet_WriteData("ItemNum", bagItemNum_Arrange[i], "ID", (storeHouseMin + i).ToString(), "RoseStoreHouse");
            function_DataSet.DataSet_WriteData("HideID", bagItemHideID_Arrange[i], "ID", (storeHouseMin + i).ToString(), "RoseStoreHouse");
            function_DataSet.DataSet_WriteData("ItemPar", bagItemPar_Arrange[i], "ID", (storeHouseMin  + i).ToString(), "RoseStoreHouse");
            function_DataSet.DataSet_WriteData("GemHole", bagItemGemHole_Arrange[i], "ID", (storeHouseMin + i).ToString(), "RoseStoreHouse");
            function_DataSet.DataSet_WriteData("GemID", bagItemGemID_Arrange[i], "ID", (storeHouseMin + i).ToString(), "RoseStoreHouse");
        }
        function_DataSet.DataSet_SetXml("RoseStoreHouse");
        //Debug.Log("整理背包成功");
        Game_PublicClassVar.Get_function_UI.PlaySource("10004", "1");
        //Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
    }

    //判断一个道具是否有隐藏属性（目前只支持判断背包内）   如果有返回技能名称
    public string IfEquipHintSkill(string bagSpace) {

        string ItemHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagSpace, "RoseBag");
        if (ItemHideID != "" && ItemHideID != "0" && ItemHideID != null) {
            string hidePropertyStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", ItemHideID, "RoseEquipHideProperty");
            string[] hideProperty = hidePropertyStr.Split(';');

            //获取和显示隐藏技能
            for (int i = 0; i < hideProperty.Length; i++)
            {
                string proprety = hideProperty[i].Split(',')[0];
                string propretyValue = hideProperty[i].Split(',')[1];
                string textShow = "";
                switch (proprety)
                {
                    case "10001":
                        //获取技能名称
                        string skillName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillName", "ID", propretyValue, "Skill_Template");
                        return skillName;
                        break;

                    case "100":
                        //获取技能名称
                        return "幸运";
                        break;
                }
            }
        }
        return "";
    }


    //获取附魔相关字符串
    public string GetHideFuMoValue(string hideIDStr) {

        string fuMoStr = "";
        if (hideIDStr != "" && hideIDStr != "0" && hideIDStr != null)
        {
            string prepeotyListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideIDStr, "RoseEquipHideProperty");
            if (prepeotyListStr.Contains("FuMo"))
            {
                string[] prepeotyList = prepeotyListStr.Split(';');
                for (int i = 0; i < prepeotyList.Length; i++)
                {
                    if (prepeotyList[i].Contains("FuMo"))
                    {
                        fuMoStr = fuMoStr + prepeotyList[i] + ";";
                    }
                }

                if (fuMoStr != "")
                {
                    fuMoStr = fuMoStr.Substring(0, fuMoStr.Length - 1);
                }
            }
        }
        return fuMoStr;
    }

    //获取隐藏属性里非附魔的值 
    public string GetHideNoFuMoValue(string hideIDStr)
    {

        string nofuMoStr = "";
        if (hideIDStr != "" && hideIDStr != "0" && hideIDStr != null)
        {
            string prepeotyListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideIDStr, "RoseEquipHideProperty");
            if (prepeotyListStr.Contains("FuMo"))
            {
                string[] prepeotyList = prepeotyListStr.Split(';');
                for (int i = 0; i < prepeotyList.Length; i++)
                {
                    if (prepeotyList[i].Contains("FuMo") == false)
                    {
                        nofuMoStr = nofuMoStr + prepeotyList[i] + ";";
                    }
                }

                if (nofuMoStr != "")
                {
                    nofuMoStr = nofuMoStr.Substring(0, nofuMoStr.Length - 1);
                }
            }
            else {
                return prepeotyListStr;
            }
        }
        return nofuMoStr;
    }

    private string WriteEquipHideNumID(XiLianResult result)
    {
        string hideProperListStr = result.hideProListStr;
        string roseEquipHideNumID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseEquipHideNumID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string roseEquipHideNumID_Now = (int.Parse(roseEquipHideNumID) + 1).ToString();
        string hidePropertyID = "0";

        if (hideProperListStr != "")
        {
            Game_PublicClassVar.Get_function_DataSet.AddRoseEquipHidePropertyXml(roseEquipHideNumID_Now, hideProperListStr);
            hidePropertyID = roseEquipHideNumID_Now;
            //存储当前极品ID
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseEquipHideNumID", roseEquipHideNumID_Now, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            //Debug.Log("roseEquipHideNumID_Now = " + roseEquipHideNumID_Now);
        }
        return hidePropertyID;
    }

    private void OnClickConfimButton(XiLianResult result)
    {
        string hidePropertyID = WriteEquipHideNumID(result);
        SelectPropetyBack(hidePropertyID);
    }

    //随机获取装备极品属性ID(itemID：装备ID,hideID:是否洗炼技能,是否广播,背包位置)
    public string ReturnHidePropertyID(string itemID, string hideID = "",bool IfGuangBo = false,string bagPosi = "")
    {

        XiLianResult result = RandomGeneratePropers(itemID, hideID, IfGuangBo, bagPosi);
        return WriteEquipHideNumID( result );

        #region
        ////读取附魔属性,附魔属性不能重置
        //string fuMoStr = "";
        //if (bagPosi != "") {
        //    string hideIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagPosi, "RoseBag");
        //    fuMoStr = GetHideFuMoValue(hideIDStr);
        //}

        ////获取装备等级和装备类型
        //string equipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemEquipID", "ID", itemID, "Item_Template");
        //string hidePropertyID = "0";
        //if (equipID != "0")
        //{
        //    string HideType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideType", "ID", equipID, "Equip_Template");

        //    string hideProperListStr = "";          //特殊属性字符串
        //    float hideShowPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideShowPro", "ID", equipID, "Equip_Template"));
            
        //    //获取装备是否有锻造大师属性
        //    float hintSkillProValue = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideSkillValue(hideID, "905");
        //    if (hintSkillProValue != 0)
        //    {
        //        hideShowPro = hideShowPro * (1 + hintSkillProValue);
        //    }
            
        //    //float hideShowPro = 0.25f;              //每个特殊属性出现的概率
        //    string roseEquipHideNumID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseEquipHideNumID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //    string roseEquipHideNumID_Now = (int.Parse(roseEquipHideNumID) + 1).ToString();
        //    /*
        //    1:血量上限
        //    2:物理攻击最大值
        //    3:物理防御最大值
        //    4:魔法防御最大值
        //    5:魔法攻击最大值
        //    */
        //    switch (HideType)
        //    { 
        //        //可出现随机属性
        //        case "1":
        //            for (int i = 2; i <= 4; i++) { 
        //                //检测概率是否触发随机概率
        //                if (Random.value <= hideShowPro) { 
        //                    //获取随机范围,并随机获取一个值
        //                    string hideMaxStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideMax", "ID", equipID, "Equip_Template");
        //                    int addValue = ReturnEquipRamdomValue(1, int.Parse(hideMaxStr),hideID);
        //                    hideProperListStr = hideProperListStr + i.ToString() + "," + addValue.ToString() + ";";
        //                }
        //            }
        //            if (hideProperListStr != "") {
        //                hideProperListStr = hideProperListStr.Substring(0, hideProperListStr.Length - 1);
        //            }
        //        break;

        //        //可出现随机属性
        //        case "2":
        //        for (int i = 1; i <= 4; i++)
        //        {
        //            //检测概率是否触发随机概率
        //            if (Random.value <= hideShowPro)
        //            {
        //                //获取随机范围,并随机获取一个值
        //                string hideMaxStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideMax", "ID", equipID, "Equip_Template");
        //                //血量属性翻5倍
        //                if (i == 1) {
        //                    hideMaxStr = (int.Parse(hideMaxStr) * 5).ToString();
        //                }
        //                int addValue = ReturnEquipRamdomValue(1, int.Parse(hideMaxStr), hideID);
        //                hideProperListStr = hideProperListStr + i.ToString() + "," + addValue.ToString() + ";";
        //            }
        //        }
        //        if (hideProperListStr != "")
        //        {
        //            hideProperListStr = hideProperListStr.Substring(0, hideProperListStr.Length - 1);
        //        }
        //        break;

        //        //可出现随机属性
        //        case "3":
        //        for (int i = 1; i <= 1; i++)
        //        {
        //            //检测概率是否触发随机概率
        //            if (Random.value <= hideShowPro)
        //            {
        //                //获取随机范围,并随机获取一个值
        //                string hideMaxStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideMax", "ID", equipID, "Equip_Template");
        //                //血量属性翻5倍
        //                if (i == 1)
        //                {
        //                    hideMaxStr = (int.Parse(hideMaxStr) * 5).ToString();
        //                }
        //                int addValue = ReturnEquipRamdomValue(1, int.Parse(hideMaxStr), hideID);
        //                hideProperListStr = hideProperListStr + i.ToString() + "," + addValue.ToString() + ";";
        //            }
        //        }
        //        if (hideProperListStr != "")
        //        {
        //            hideProperListStr = hideProperListStr.Substring(0, hideProperListStr.Length - 1);
        //        }
        //        break;
        //    }


        //    //宠物装备不会有其他极品属性
        //    ObscuredBool hideSkillStatus = true;
        //    ObscuredBool hidePetEquipStatus = false;
        //    ObscuredString itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
        //    if (int.Parse(itemSubType) >= 201 && int.Parse(itemSubType) <= 204)
        //    {
        //        hidePetEquipStatus = true;
        //    }


        //    if (hideSkillStatus)
        //    {

        //        //附加幸运值(100属性类型表示幸运值)
        //        ObscuredFloat luckProValue = 0.99f;
        //        //如果是掉落,概率降低10倍
        //        if (hideID == "")
        //        {
        //            luckProValue = 0.999f;
        //        }

        //        if (Random.value >= luckProValue && hidePetEquipStatus == false)
        //        {
        //            int addValue = 1;

        //            if (hideProperListStr != "")
        //            {
        //                hideProperListStr = hideProperListStr + ";100" + "," + addValue.ToString();
        //            }
        //            else
        //            {
        //                hideProperListStr = hideProperListStr + "100" + "," + addValue.ToString();
        //            }

        //            //获取玩家名称
        //            string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //            if (IfGuangBo)
        //            {
        //                //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "恭喜玩家" + roseName + "洗炼装备时!意外在装备里发现了幸运属性！");
        //                //Debug.Log("恭喜玩家" + roseName + "洗炼装备时!意外在装备里发现了幸运属性！");
        //            }
        //        }

        //        //附加额外的极品属性
        //        ObscuredFloat equipJiPinPro = 0.11f;

        //        //如果是掉落,概率降低10倍
        //        if (hideID == "")
        //        {
        //            equipJiPinPro = 0.05f;
        //        }

        //        if (hidePetEquipStatus)
        //        {
        //            equipJiPinPro = 0.3f;     //临时测试
        //        }

        //        if (Random.value <= equipJiPinPro)
        //        {

        //            string nextID = "0";
        //            string hintProListID = "1001";
        //            //获取隐藏条最大目数
        //            int hintJiPinMaxNum = 3;
        //            int hintJiPinMaxNumSum = 0;

        //            if (hidePetEquipStatus) {
        //                hintProListID = "3001";
        //            }

        //            do
        //            {
        //                //获取单条触发概率
        //                float triggerPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerPro", "ID", hintProListID, "HintProList_Template"));
        //                //判定当条属性位置是否激活
        //                //string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
        //                string[] equipSpaceList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSpace", "ID", hintProListID, "HintProList_Template").Split(',');
        //                bool equipStatus = false;
        //                if (equipSpaceList[0] != "" && equipSpaceList[0] != "0")
        //                {
        //                    for (int i = 0; i < equipSpaceList.Length; i++)
        //                    {
        //                        if (itemSubType == equipSpaceList[i])
        //                        {
        //                            equipStatus = true;
        //                        }
        //                    }
        //                }

        //                if (!equipStatus)
        //                {
        //                    break;
        //                }

        //                //触发隐藏属性
        //                if (Random.value < triggerPro)
        //                {
        //                    //读取隐藏属性类型和对应随机值
        //                    string hintProType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyType", "ID", hintProListID, "HintProList_Template");
        //                    float propertyValueMin = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyValueMin", "ID", hintProListID, "HintProList_Template"));
        //                    float propertyValueMax = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyValueMax", "ID", hintProListID, "HintProList_Template"));

        //                    //判定是随着等级变动
        //                    string ifEquipLvUp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfEquipLvUp", "ID", hintProListID, "HintProList_Template");
        //                    if (ifEquipLvUp == "1")
        //                    {

        //                        //获取等级
        //                        int itemlv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", itemID, "Item_Template"));
        //                        if (itemlv < 10)
        //                        {
        //                            itemlv = 10;
        //                        }
        //                        int itemNum = (int)(itemlv / 10);
        //                        itemNum = itemNum - 1;
        //                        if (itemNum < 1)
        //                        {
        //                            itemNum = 1;
        //                        }
        //                        //获取属性
        //                        propertyValueMax = propertyValueMax + propertyValueMax / 2 * itemNum;
        //                    }

        //                    //隐藏属性值得类型
        //                    string hintProValueType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HintProValueType", "ID", hintProListID, "HintProList_Template");
        //                    float hintProVlaue = 0;
        //                    if (hintProValueType == "1")
        //                    {
        //                        //表示整数
        //                        hintProVlaue = ReturnEquipRamdomValue((int)(propertyValueMin), (int)(propertyValueMax), hideID);
        //                        if (hintProVlaue <= 0)
        //                        {
        //                            hintProVlaue = propertyValueMin;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //表示浮点数
        //                        hintProVlaue = ReturnEquipRamdomValue_float(propertyValueMin, propertyValueMax);
        //                        if (hintProVlaue <= 0)
        //                        {
        //                            hintProVlaue = propertyValueMin;
        //                        }
        //                    }

        //                    //取随机值
        //                    //float hintProVlaue = propertyValueMin + (propertyValueMax - propertyValueMin)* Random.value;
        //                    //取小数点的后两位
        //                    //hintProVlaue = (float)(System.Math.Round(hintProVlaue, 2));

        //                    if (hideProperListStr != "")
        //                    {
        //                        hideProperListStr = hideProperListStr + ";" + hintProType + "," + hintProVlaue.ToString();
        //                    }
        //                    else
        //                    {
        //                        hideProperListStr = hintProType + "," + hintProVlaue.ToString();
        //                    }

        //                    //写入活跃任务
        //                    Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "102", "1");

        //                    hintJiPinMaxNumSum = hintJiPinMaxNumSum + 1;
        //                    if (hintJiPinMaxNumSum >= hintJiPinMaxNum)
        //                    {
        //                        //立即跳出循环
        //                        break;
        //                    }
        //                }

        //                nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NtxtID", "ID", hintProListID, "HintProList_Template");
        //                hintProListID = nextID;

        //            } while (nextID != "0");
        //        }

        //        //判定是否需要写入特殊技能
        //        if (hideID != "")
        //        {
        //            //附加特殊技能
        //            ObscuredFloat equipJiPinSkillPro = 0.0151f;
        //            //如果是掉落,概率降低10倍
        //            if (hideID == "")
        //            {
        //                equipJiPinSkillPro = 0.001f;
        //            }

        //            //洗炼大师附加
        //            string xilianJiaID = RetuenXiLianJiaLv();
        //            if (xilianJiaID != "" && xilianJiaID != null) { 
        //                string xiLianValueStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddXiLianPro", "ID", xilianJiaID, "EquipXiLianDaShi_Template");
        //                if (xiLianValueStr!=""&& xiLianValueStr!=null) {
        //                    float xilianAddValue = float.Parse(xiLianValueStr);
        //                    equipJiPinSkillPro = equipJiPinSkillPro + xilianAddValue;
        //                }
        //            }

        //            if (Random.value <= equipJiPinSkillPro)
        //            {
        //                ObscuredString nextID = "0";
        //                ObscuredString hintProListID = "2001";
        //                ObscuredString hintSkillType = "10001";
        //                //获取隐藏条最大目数
        //                ObscuredInt hintJiPinMaxNum = 3;
        //                ObscuredInt hintJiPinMaxNumSum = 0;
        //                do
        //                {
        //                    //获取单条触发概率
        //                    ObscuredFloat triggerPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerPro", "ID", hintProListID, "HintProList_Template"));
        //                    //判定当条属性位置是否激活
        //                    //string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
        //                    string[] equipSpaceList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSpace", "ID", hintProListID, "HintProList_Template").Split(',');
        //                    bool equipStatus = false;
        //                    if (equipSpaceList[0] != "" && equipSpaceList[0] != "0")
        //                    {
        //                        for (int i = 0; i < equipSpaceList.Length; i++)
        //                        {
        //                            if (itemSubType == equipSpaceList[i])
        //                            {
        //                                equipStatus = true;
        //                            }
        //                        }
        //                    }

        //                    if (equipStatus)
        //                    {
        //                        //触发隐藏属性
        //                        if (Random.value < triggerPro)
        //                        {
        //                            //读取隐藏属性类型和对应随机值
        //                            string hideSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyType", "ID", hintProListID, "HintProList_Template");
        //                            float propertyValueMin = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyValueMin", "ID", hintProListID, "HintProList_Template"));
        //                            float propertyValueMax = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyValueMax", "ID", hintProListID, "HintProList_Template"));

        //                            if (hideProperListStr != "")
        //                            {
        //                                hideProperListStr = hideProperListStr + ";" + hintSkillType + "," + hideSkillID;
        //                            }
        //                            else
        //                            {
        //                                hideProperListStr = hintSkillType + "," + hideSkillID;
        //                            }

        //                            //写入成就(特殊效果)
        //                            Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("205", hintProListID, "1");

        //                            //写入活跃任务
        //                            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "102", "1");

        //                            //发送广播
        //                            /*
        //                            string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //                            string hintName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", hintProListID, "HintProList_Template");

        //                            if (IfGuangBo) {
        //                                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "恭喜玩家" + roseName + "洗炼装备时!意外在装备里发现了隐藏技能:" + hintName + "！");
        //                            }
        //                            */

        //                            hintJiPinMaxNumSum = hintJiPinMaxNumSum + 1;
        //                            if (hintJiPinMaxNumSum >= hintJiPinMaxNum)
        //                            {
        //                                //立即跳出循环
        //                                break;
        //                            }
        //                        }
        //                    }

        //                    nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NtxtID", "ID", hintProListID, "HintProList_Template");
        //                    hintProListID = nextID;
        //                } while (nextID != "0");

        //            }
        //        }
        //        else
        //        {

        //            //获取当前装备的技能属性进行叠加
        //            if (hideID != "0" && hideID != "" && hideID != null)
        //            {
        //                string hideProListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");
        //                string[] hideProperty = hideProListStr.Split(';');

        //                //循环加入之前的隐藏属性
        //                if (hideProListStr != "")
        //                {
        //                    for (int y = 0; y <= hideProperty.Length - 1; y++)
        //                    {
        //                        string hidePropertyType = hideProperty[y].Split(',')[0];
        //                        string hidePropertyValue = hideProperty[y].Split(',')[1];

        //                        if (hidePropertyType == "10001")
        //                        {

        //                            if (hideProperListStr != "")
        //                            {
        //                                hideProperListStr = hideProperListStr + ";" + hidePropertyType + "," + hidePropertyValue;
        //                            }
        //                            else
        //                            {
        //                                hideProperListStr = hideProperListStr + hidePropertyType + "," + hidePropertyValue;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    //查询是否有附魔值
        //    if (fuMoStr != "" && fuMoStr != "0" && fuMoStr != null) {
        //        if (hideProperListStr != "") {
        //            hideProperListStr = hideProperListStr + ";" + fuMoStr;
        //        }
        //        else {
        //            hideProperListStr = fuMoStr;
        //        }
        //    }

        //    //Debug.Log("hideProperListStr = "+ hideProperListStr);
        //    //写入极品装备数据
        //    if (hideProperListStr != "") {
        //        Game_PublicClassVar.Get_function_DataSet.AddRoseEquipHidePropertyXml(roseEquipHideNumID_Now, hideProperListStr);
        //        hidePropertyID = roseEquipHideNumID_Now;
        //        //存储当前极品ID
        //        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseEquipHideNumID", roseEquipHideNumID_Now, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //        //Debug.Log("roseEquipHideNumID_Now = " + roseEquipHideNumID_Now);
        //    }

        //}
        //return hidePropertyID;
        #endregion
    }


    public string ReturnHidePropertyIDEx(string itemID, string hideID = "", System.Action<string> selectp = null, bool IfGuangBo = false, string bagPosi = "" )
    {
        SelectPropetyBack = selectp;
        mXiLianResults = new List<XiLianResult>();

        ObscuredInt numer = 10;
        while (mXiLianResults.Count < numer)
        {
            XiLianResult result = this.RandomGeneratePropers(itemID, hideID, IfGuangBo, bagPosi);
            if (result.hideProListStr != "")
            {
                result.index = mXiLianResults.Count;
                mXiLianResults.Add(result);
            }
        }

        GameObject uiXinlianList = MonoBehaviour.Instantiate((GameObject)Resources.Load("UGUI/UISet/RoseEquip/UI_EquipXiLianList", typeof(GameObject)));
        uiXinlianList.GetComponent<UI_EquipXiLianList>().InitData(mXiLianResults, this.OnClickConfimButton);
        uiXinlianList.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiXinlianList.transform.localPosition = Vector3.zero;
        uiXinlianList.transform.localScale = new Vector3(1, 1, 1);

        return "1";
    }

    //随机获取装备极品属性ID(itemID：装备ID,hideID:是否洗炼技能,是否广播,背包位置)
    public XiLianResult RandomGeneratePropers(string itemID, string hideID = "", bool IfGuangBo = false, string bagPosi = "")
    {

        string xiLianNumber = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (xiLianNumber == "")
            xiLianNumber = "0";
        OXilianNumber = int.Parse(xiLianNumber);

        //特殊属性字符串
        string hideProperListStr = "";
        List<XiLianChenjiu> xilianChenjiuList = new List<XiLianChenjiu>();
        XiLianResult xilianResult = new XiLianResult();
        xilianResult.equipItemId = itemID;

        //读取附魔属性,附魔属性不能重置
        string fuMoStr = "";
        if (bagPosi != "")
        {
            string hideIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagPosi, "RoseBag");
            fuMoStr = GetHideFuMoValue(hideIDStr);
        }

        //获取装备等级和装备类型
        string equipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemEquipID", "ID", itemID, "Item_Template");
        if (equipID != "0")
        {
            string HideType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideType", "ID", equipID, "Equip_Template");

            float hideShowPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideShowPro", "ID", equipID, "Equip_Template"));

            //获取装备是否有锻造大师属性
            float hintSkillProValue = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideSkillValue(hideID, "905");
            if (hintSkillProValue != 0)
            {
                hideShowPro = hideShowPro * (1 + hintSkillProValue);
            }

            //float hideShowPro = 0.25f;              //每个特殊属性出现的概率
            string roseEquipHideNumID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseEquipHideNumID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            string roseEquipHideNumID_Now = (int.Parse(roseEquipHideNumID) + 1).ToString();
            /*
            1:血量上限
            2:物理攻击最大值
            3:物理防御最大值
            4:魔法防御最大值
            5:魔法攻击最大值
            */
            switch (HideType)
            {
                //可出现随机属性
                case "1":
                    ObscuredInt numer1 = 2;
                    ObscuredInt numer2 = 4;
                    for (int i = numer1; i <= numer2; i++)
                    {
                        //检测概率是否触发随机概率
                        if (Random.value <= hideShowPro)
                        {
                            //获取随机范围,并随机获取一个值
                            string hideMaxStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideMax", "ID", equipID, "Equip_Template");
                            int addValue = ReturnEquipRamdomValue(1, int.Parse(hideMaxStr), hideID);
                            hideProperListStr = hideProperListStr + i.ToString() + "," + addValue.ToString() + ";";
                        }
                    }
                    if (hideProperListStr != "")
                    {
                        hideProperListStr = hideProperListStr.Substring(0, hideProperListStr.Length - 1);
                    }
                    break;

                //可出现随机属性
                case "2":
                    ObscuredInt numer21 = 1;
                    ObscuredInt numer22 = 4;
                    for (int i = numer21; i <= numer22; i++)
                    {
                        //检测概率是否触发随机概率
                        if (Random.value <= hideShowPro)
                        {
                            //获取随机范围,并随机获取一个值
                            string hideMaxStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideMax", "ID", equipID, "Equip_Template");
                            //血量属性翻5倍
                            if (i == 1)
                            {
                                hideMaxStr = (int.Parse(hideMaxStr) * 5).ToString();
                            }
                            int addValue = ReturnEquipRamdomValue(1, int.Parse(hideMaxStr), hideID);
                            hideProperListStr = hideProperListStr + i.ToString() + "," + addValue.ToString() + ";";
                        }
                    }
                    if (hideProperListStr != "")
                    {
                        hideProperListStr = hideProperListStr.Substring(0, hideProperListStr.Length - 1);
                    }
                    break;

                //可出现随机属性
                case "3":
                    ObscuredInt numer31 = 1;
                    for (int i = numer31; i <= numer31; i++)
                    {
                        //检测概率是否触发随机概率
                        if (Random.value <= hideShowPro)
                        {
                            //获取随机范围,并随机获取一个值
                            string hideMaxStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideMax", "ID", equipID, "Equip_Template");
                            //血量属性翻5倍
                            if (i == 1)
                            {
                                hideMaxStr = (int.Parse(hideMaxStr) * 5).ToString();
                            }
                            int addValue = ReturnEquipRamdomValue(1, int.Parse(hideMaxStr), hideID);
                            hideProperListStr = hideProperListStr + i.ToString() + "," + addValue.ToString() + ";";
                        }
                    }
                    if (hideProperListStr != "")
                    {
                        hideProperListStr = hideProperListStr.Substring(0, hideProperListStr.Length - 1);
                    }
                    break;
            }


            //宠物装备不会有其他极品属性
            ObscuredBool hideSkillStatus = true;
            ObscuredBool hidePetEquipStatus = false;
            ObscuredString itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
            if (int.Parse(itemSubType) >= 201 && int.Parse(itemSubType) <= 204)
            {
                hidePetEquipStatus = true;
            }

            if (hideSkillStatus)
            {
                //附加幸运值(100属性类型表示幸运值)
                ObscuredFloat luckProValue = 0.99f;
                //如果是掉落,概率降低10倍
                if (hideID == "")
                {
                    luckProValue = 0.999f;
                }

                if (Random.value >= luckProValue && hidePetEquipStatus == false)
                {
                    int addValue = 1;

                    if (hideProperListStr != "")
                    {
                        hideProperListStr = hideProperListStr + ";100" + "," + addValue.ToString();
                    }
                    else
                    {
                        hideProperListStr = hideProperListStr + "100" + "," + addValue.ToString();
                    }

                    //获取玩家名称
                    string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    if (IfGuangBo)
                    {
                        //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "恭喜玩家" + roseName + "洗炼装备时!意外在装备里发现了幸运属性！");
                        //Debug.Log("恭喜玩家" + roseName + "洗炼装备时!意外在装备里发现了幸运属性！");
                    }
                }

                //附加额外的极品属性
                ObscuredFloat equipJiPinPro = 0.1f;

                //如果是掉落,概率降低10倍
                if (hideID == "")
                {
                    equipJiPinPro = 0.05f;
                }

                if (hidePetEquipStatus)
                {
                    equipJiPinPro = 0.3f;     //临时测试
                }

                if (Random.value <= equipJiPinPro)
                {

                    string nextID = "0";
                    string hintProListID = "1001";
                    //获取隐藏条最大目数
                    int hintJiPinMaxNum = 3;
                    int hintJiPinMaxNumSum = 0;

                    if (hidePetEquipStatus)
                    {
                        hintProListID = "3001";
                    }

                    do
                    {
                        //获取单条触发概率
                        float triggerPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerPro", "ID", hintProListID, "HintProList_Template"));
                        //判定当条属性位置是否激活
                        //string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
                        string[] equipSpaceList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSpace", "ID", hintProListID, "HintProList_Template").Split(',');
                        bool equipStatus = false;
                        if (equipSpaceList[0] != "" && equipSpaceList[0] != "0")
                        {
                            for (int i = 0; i < equipSpaceList.Length; i++)
                            {
                                if (itemSubType == equipSpaceList[i])
                                {
                                    equipStatus = true;
                                }
                            }
                        }

                        if (!equipStatus)
                        {
                            break;
                        }

                        //触发隐藏属性
                        if (Random.value < triggerPro)
                        {
                            //读取隐藏属性类型和对应随机值
                            string hintProType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyType", "ID", hintProListID, "HintProList_Template");
                            float propertyValueMin = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyValueMin", "ID", hintProListID, "HintProList_Template"));
                            float propertyValueMax = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyValueMax", "ID", hintProListID, "HintProList_Template"));

                            //判定是随着等级变动
                            string ifEquipLvUp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfEquipLvUp", "ID", hintProListID, "HintProList_Template");
                            if (ifEquipLvUp == "1")
                            {

                                //获取等级
                                int itemlv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", itemID, "Item_Template"));
                                if (itemlv < 10)
                                {
                                    itemlv = 10;
                                }
                                int itemNum = (int)(itemlv / 10);
                                itemNum = itemNum - 1;
                                if (itemNum < 1)
                                {
                                    itemNum = 1;
                                }
                                //获取属性
                                propertyValueMax = propertyValueMax + propertyValueMax / 2 * itemNum;
                            }

                            //隐藏属性值得类型
                            string hintProValueType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HintProValueType", "ID", hintProListID, "HintProList_Template");
                            float hintProVlaue = 0;
                            if (hintProValueType == "1")
                            {
                                //表示整数
                                hintProVlaue = ReturnEquipRamdomValue((int)(propertyValueMin), (int)(propertyValueMax), hideID);
                                if (hintProVlaue <= 0)
                                {
                                    hintProVlaue = propertyValueMin;
                                }
                            }
                            else
                            {
                                //表示浮点数
                                hintProVlaue = ReturnEquipRamdomValue_float(propertyValueMin, propertyValueMax);
                                if (hintProVlaue <= 0)
                                {
                                    hintProVlaue = propertyValueMin;
                                }
                            }

                            //取随机值
                            //float hintProVlaue = propertyValueMin + (propertyValueMax - propertyValueMin)* Random.value;
                            //取小数点的后两位
                            //hintProVlaue = (float)(System.Math.Round(hintProVlaue, 2));

                            if (hideProperListStr != "")
                            {
                                hideProperListStr = hideProperListStr + ";" + hintProType + "," + hintProVlaue.ToString();
                            }
                            else
                            {
                                hideProperListStr = hintProType + "," + hintProVlaue.ToString();
                            }

                            //写入活跃任务
                            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "102", "1");

                            hintJiPinMaxNumSum = hintJiPinMaxNumSum + 1;
                            if (hintJiPinMaxNumSum >= hintJiPinMaxNum)
                            {
                                //立即跳出循环
                                break;
                            }
                        }

                        nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NtxtID", "ID", hintProListID, "HintProList_Template");
                        hintProListID = nextID;

                    } while (nextID != "0");
                }

                //判定是否需要写入特殊技能
                if (hideID != "")
                {
                    //附加特殊技能
                    ObscuredFloat equipJiPinSkillPro = 0.0108f;
                    //如果是掉落,概率降低10倍
                    if (hideID == "")
                    {
                        equipJiPinSkillPro = 0.001f;
                    }

                    //洗炼大师附加
                    string xilianJiaID = RetuenXiLianJiaLv();
                    if (xilianJiaID != "" && xilianJiaID != null)
                    {
                        string xiLianValueStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddXiLianPro", "ID", xilianJiaID, "EquipXiLianDaShi_Template");
                        if (xiLianValueStr != "" && xiLianValueStr != null)
                        {
                            float xilianAddValue = float.Parse(xiLianValueStr);
                            equipJiPinSkillPro = equipJiPinSkillPro + xilianAddValue;
                        }
                    }

                    bool ishaveSkill = IsHideTriggerSkillID(OXilianNumber);

                    if (Random.value <= equipJiPinSkillPro || ishaveSkill)
                    {
                        ObscuredString nextID = "0";
                        ObscuredString hintProListID = "2001";
                        ObscuredString hintSkillType = "10001";
                        //获取隐藏条最大目数
                        ObscuredInt hintJiPinMaxNum = 3;
                        ObscuredInt hintJiPinMaxNumSum = 0;

                        //洗练到一定次数必得隐藏技能ID
                        string teShuHintProListID = GetHideTriggerSkillID(OXilianNumber);

                        if (teShuHintProListID != "")
                        {
                            hintProListID = teShuHintProListID;
                        }

                        do
                        {
                            //获取单条触发概率
                            ObscuredFloat triggerPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerPro", "ID", hintProListID, "HintProList_Template"));
                            //判定当条属性位置是否激活
                            //string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
                            string[] equipSpaceList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSpace", "ID", hintProListID, "HintProList_Template").Split(',');
                            bool equipStatus = false;
                            if (equipSpaceList[0] != "" && equipSpaceList[0] != "0")
                            {
                                for (int i = 0; i < equipSpaceList.Length; i++)
                                {
                                    if (itemSubType == equipSpaceList[i])
                                    {
                                        equipStatus = true;
                                    }
                                }
                            }

                            if (equipStatus)
                            {

                                //触发隐藏属性
                                if (Random.value < triggerPro || teShuHintProListID != "" )
                                {

                                    //读取隐藏属性类型和对应随机值
                                    string hideSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyType", "ID", hintProListID, "HintProList_Template");

                                    float propertyValueMin = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyValueMin", "ID", hintProListID, "HintProList_Template"));
                                    float propertyValueMax = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyValueMax", "ID", hintProListID, "HintProList_Template"));

                                    if (hideProperListStr != "")
                                    {
                                        hideProperListStr = hideProperListStr + ";" + hintSkillType + "," + hideSkillID;
                                    }
                                    else
                                    {
                                        hideProperListStr = hintSkillType + "," + hideSkillID;
                                    }

                                    //写入成就(特殊效果)
                                    Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("205", hintProListID, "1");

                                    //写入活跃任务
                                    Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "102", "1");

                                    //发送广播
                                    /*
                                    string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                                    string hintName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", hintProListID, "HintProList_Template");

                                    if (IfGuangBo) {
                                        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "恭喜玩家" + roseName + "洗炼装备时!意外在装备里发现了隐藏技能:" + hintName + "！");
                                    }
                                    */

                                    hintJiPinMaxNumSum = hintJiPinMaxNumSum + 1;
                                    if (hintJiPinMaxNumSum >= hintJiPinMaxNum)
                                    {
                                        //立即跳出循环
                                        break;
                                    }
                                }
                            }

                            nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NtxtID", "ID", hintProListID, "HintProList_Template");
                            hintProListID = nextID;

                            if (teShuHintProListID != "") {
                                break;
                            }

                        } while (nextID != "0");

                    }
                }
                else
                {

                    //获取当前装备的技能属性进行叠加
                    if (hideID != "0" && hideID != "" && hideID != null)
                    {
                        string hideProListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");
                        string[] hideProperty = hideProListStr.Split(';');

                        //循环加入之前的隐藏属性
                        if (hideProListStr != "")
                        {
                            for (int y = 0; y <= hideProperty.Length - 1; y++)
                            {
                                string hidePropertyType = hideProperty[y].Split(',')[0];
                                string hidePropertyValue = hideProperty[y].Split(',')[1];

                                if (hidePropertyType == "10001")
                                {

                                    if (hideProperListStr != "")
                                    {
                                        hideProperListStr = hideProperListStr + ";" + hidePropertyType + "," + hidePropertyValue;
                                    }
                                    else
                                    {
                                        hideProperListStr = hideProperListStr + hidePropertyType + "," + hidePropertyValue;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //查询是否有附魔值
            if (fuMoStr != "" && fuMoStr != "0" && fuMoStr != null)
            {
                if (hideProperListStr != "")
                {
                    hideProperListStr = hideProperListStr + ";" + fuMoStr;
                }
                else
                {
                    hideProperListStr = fuMoStr;
                }
            }

        }
        xilianResult.taskList = xilianChenjiuList;
        xilianResult.hideProListStr = hideProperListStr;

        if (hideProperListStr != "")
        {
            OXilianNumber = OXilianNumber + 1;
        }
        if (OXilianNumber > OXilianNumber1500)
        {
            OXilianNumber = 0;
            xiLianNumber = "0";
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XiLianNum", OXilianNumber.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

        return xilianResult;           
    }

    private bool IsHideTriggerSkillID(ObscuredInt xilianNum)
    {
        xilianNum = xilianNum + 1;
        return xilianNum % 100 == 0;
        //return xilianNum == OXilianNumber100 || xilianNum == OXilianNumber500 || xilianNum == OXilianNumber1000 || xilianNum == OXilianNumber1500;
    }

    private string GetHideTriggerSkillID(ObscuredInt xilianNum)
    {
        xilianNum = xilianNum + 1;
        if (xilianNum % 100 != 0)
            return "";

        if (xilianNum == OXilianNumber500 || xilianNum == OXilianNumber1000 || xilianNum == OXilianNumber1500)
        {
            string[] XiLianSkillIDSet_List = new string[0];
            switch (xilianNum)
            {
                case 500:
                    XiLianSkillIDSet_List = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "XiLianSkillIDSet_1", "GameMainValue").Split(';');
                    break;
                case 1000:
                    XiLianSkillIDSet_List = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "XiLianSkillIDSet_2", "GameMainValue").Split(';');
                    break;
                case 1500:
                    XiLianSkillIDSet_List = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "XiLianSkillIDSet_3", "GameMainValue").Split(';');
                    break;
                default:
                    break;
            }
            if (XiLianSkillIDSet_List.Length == 0)
                return "";

            int random = Mathf.FloorToInt(Random.Range(0, XiLianSkillIDSet_List.Length));
            return XiLianSkillIDSet_List[random];
        }
        else
        {
            return "";
        }
    }

    //随机获取装备极品属性ID(itemID：装备ID,hideID:是否洗炼技能,是否广播,背包位置)
    public string ReturnNullHidePropertyID()
    {
        string roseEquipHideNumID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseEquipHideNumID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string roseEquipHideNumID_Now = (int.Parse(roseEquipHideNumID) + 1).ToString();
        Game_PublicClassVar.Get_function_DataSet.AddRoseEquipHidePropertyXml(roseEquipHideNumID_Now, "");
        string hidePropertyID = roseEquipHideNumID_Now;

        //存储当前极品ID
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseEquipHideNumID", roseEquipHideNumID_Now, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        return hidePropertyID;
    }




    //固定写入装备极品属性获取ID
    public string ReturnHidePropertyID_GuDing(string hideStr) {

		//获取装备等级和装备类型
		string hidePropertyID = "0";
		string hideProperListStr = hideStr;          //特殊属性字符串
		string roseEquipHideNumID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseEquipHideNumID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		string roseEquipHideNumID_Now = (int.Parse(roseEquipHideNumID) + 1).ToString();

		//写入极品装备数据
		if (hideProperListStr != "") {
			Game_PublicClassVar.Get_function_DataSet.AddRoseEquipHidePropertyXml(roseEquipHideNumID_Now, hideProperListStr);
			hidePropertyID = roseEquipHideNumID_Now;
			//存储当前极品ID
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseEquipHideNumID", roseEquipHideNumID_Now, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
			Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
		}

		return hidePropertyID;
	}

    //获取附魔属性
    public string ReturnFuMoProperty(string WriteBagPosi,string equipBagSpace,float minValue,float maxValue) {

        //获取装备信息
        string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", WriteBagPosi, "RoseBag");
        int itemLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", itemID, "Item_Template"));

        //根据等级设定最高上限
        string equipItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", equipBagSpace, "RoseBag");
        int equipitemLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", equipItemID, "Item_Template"));
        int maxInt = (int)((float)(equipitemLv) / 10.0f);
        int actMax = 10 + maxInt * 10;
        int defMax = 5 + maxInt * 5;
        int hpMax = 30 + maxInt * 20;

        string returnStr = "";

        //根据道具ID定义不同的偏向性
        int act_pro = 25;
        int mage_pro = 25;
        int def_pro = 25;
        int hp_pro = 25;
        int randomSum = 0;

        //攻击类型
        if (itemID == "11001001") {
            act_pro = 100;
            mage_pro = 0;
            def_pro = 0;
            hp_pro = 0;
        }

        //法术类型
        if (itemID == "11001002")
        {
            act_pro = 0;
            mage_pro = 100;
            def_pro = 0;
            hp_pro = 0;
        }

        //防御类型
        if (itemID == "11001003")
        {
            act_pro = 0;
            mage_pro = 0;
            def_pro = 100;
            hp_pro = 0;
        }

        //血量类型
        if (itemID == "11001004")
        {
            act_pro = 0;
            mage_pro = 0;
            def_pro = 0;
            hp_pro = 100;
        }

        bool ramdonStatus = false;
        float ranValue = Random.value * 100;
        string fomoType = "3";

        randomSum = randomSum + act_pro;
        if (ranValue < randomSum && ramdonStatus == false)
        {
            fomoType = "1";
            ramdonStatus = true;
        }

        randomSum = randomSum + mage_pro;
        if (ranValue < randomSum && ramdonStatus == false)
        {
            fomoType = "2";
            ramdonStatus = true;
        }

        randomSum = randomSum + def_pro;
        if (ranValue < randomSum && ramdonStatus == false)
        {
            fomoType = "3";
            ramdonStatus = true;
        }

        randomSum = randomSum + hp_pro;
        if (ranValue < hp_pro && ramdonStatus == false)
        {
            fomoType = "4";
            ramdonStatus = true;
        }

        //根据附魔类型随机值写入值
        int value = 0;
        string writeFuMoValue = "";

        switch (fomoType) {

            //攻击
            case "1":
                float ranProValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(minValue, maxValue);
                value = (int)((float)actMax * ranProValue);
                writeFuMoValue = "FuMo,"+"11," + value;
                break;

            //法术
            case "2":
                ranProValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(minValue, maxValue);
                value = (int)((float)actMax * ranProValue);
                writeFuMoValue = "FuMo," + "14," + value;
                break;

            //双防
            case "3":
                ranProValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(minValue, maxValue);
                value = (int)((float)defMax * ranProValue);
                writeFuMoValue = "FuMo," + "17," + value;
                ranProValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(minValue, maxValue);
                value = 0;
                value = (int)((float)defMax * ranProValue);
                writeFuMoValue = writeFuMoValue + ";" + "FuMo," + "20," + value;
                break;

            //血量
            case "4":
                ranProValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(minValue, maxValue);
                value = (int)((float)hpMax * ranProValue);
                writeFuMoValue = "FuMo," + "10," + value;
                break;
        }

        return writeFuMoValue;
    }



    //固定写入装备附魔属性获取ID
    public string WriteHidePropertyFuMoID(string WriteBagPosi,string fuMoStr)
    {

        //fuMoStr = "FuMo,1,10";

        //获取装备等级和装备类型
        string EquiphideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", WriteBagPosi, "RoseBag");
        string hidePropertyID = "0";
        //string hideProperListStr = fuMoStr;          //特殊属性字符串
        string roseEquipHideNumID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseEquipHideNumID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string roseEquipHideNumID_Now = (int.Parse(roseEquipHideNumID) + 1).ToString();

        //写入极品装备数据
        if (fuMoStr != "")
        {

            if (EquiphideID == "" || EquiphideID == "0" || EquiphideID == null)
            {
                //新建隐藏属性
                Game_PublicClassVar.Get_function_DataSet.AddRoseEquipHidePropertyXml(roseEquipHideNumID_Now, fuMoStr);
                hidePropertyID = roseEquipHideNumID_Now;
                //存储当前极品ID
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseEquipHideNumID", roseEquipHideNumID_Now, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            }
            else {
                //读取隐藏属性
                string nowPrepeotyList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", EquiphideID, "RoseEquipHideProperty");
                if (nowPrepeotyList != "" && nowPrepeotyList != "0" && nowPrepeotyList != null)
                {
                    fuMoStr = nowPrepeotyList + ";" + fuMoStr;
                }
                //写入隐藏属性
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PrepeotyList", fuMoStr,"ID", EquiphideID, "RoseEquipHideProperty");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquipHideProperty");
            }
        }

        return hidePropertyID;
    }



    //传入值获取当前隐藏属性的某个值属性
    public string ReturnEquipHideTypeValue(string itemID, string spaceType,string hideType,string spaceNum)
    {
        string returnTypeValue = "";
        string hideID = "";
        switch (spaceType) { 
            
            //背包
            case "1":
                hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", spaceNum, "RoseBag");
                break;

            //装备
            case "2":
                hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", spaceNum, "RoseEquip");
                break;
        }

        //获取当前装备的各项属性
        string equip_ID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemEquipID", "ID", itemID, "Item_Template");
        string equip_Hp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Hp", "ID", equip_ID, "Equip_Template");
        string equip_MaxAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MaxAct", "ID", equip_ID, "Equip_Template");
        string equip_MaxDef = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MaxDef", "ID", equip_ID, "Equip_Template");
        string equip_MaxAdf = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MaxAdf", "ID", equip_ID, "Equip_Template");
        string hidePropertyStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");

        string hideValue_Hp = "0";
        string hideValue_Act = "0";
        string hideValue_Def = "0";
        string hideValue_Adf = "0";

        //Debug.Log("equip_MaxAct = " + equip_MaxAct);
        if (hidePropertyStr != "" && hidePropertyStr != "0") {
            string[] hideProperty = hidePropertyStr.Split(';');
            //隐藏属性
            /*
            1:血量上限
            2:物理攻击最大值
            3:物理防御最大值
            4:魔法防御最大值
            */
            //循环加入各个隐藏属性
            if (hidePropertyStr != "")
            {
                for (int i = 0; i <= hideProperty.Length - 1; i++)
                {
                    string hidePropertyType = hideProperty[i].Split(',')[0];
                    string hidePropertyValue = hideProperty[i].Split(',')[1];

                    switch (hidePropertyType)
                    {
                        //血量上限
                        case "0":
                            if (hideType == hidePropertyType)
                            {
                                hideValue_Hp = hidePropertyValue;
                            }
                            break;
                        //物理攻击最大值
                        case "2":
                            if (hideType == hidePropertyType)
                            {
                                //Debug.Log("hidePropertyValue = " + hidePropertyValue);
                                hideValue_Act = hidePropertyValue;
                            }
                            break;
                        case "3":
                            //物理防御最大值
                            if (hideType == hidePropertyType)
                            {
                                hideValue_Def = hidePropertyValue;
                            }
                            break;
                        //魔法防御最大值
                        case "4":
                            if (hideType == hidePropertyType)
                            {
                                hideValue_Adf = hidePropertyValue;
                            }
                            break;
                    }
                }
            }
        }

        switch (hideType)
        {
            //血量上限
            case "0":
                returnTypeValue = (int.Parse(equip_Hp) + int.Parse(hideValue_Hp)).ToString(); ;
                break;
            //物理攻击最大值
            case "2":
                //Debug.Log("returnTypeValue = " + returnTypeValue);
                returnTypeValue = (int.Parse(equip_MaxAct) + int.Parse(hideValue_Act)).ToString();
                //Debug.Log("returnTypeValue = " + returnTypeValue);
                break;
            case "3":
                returnTypeValue = (int.Parse(equip_MaxDef) + int.Parse(hideValue_Def)).ToString();
                break;
            //魔法防御最大值
            case "4":
                returnTypeValue = (int.Parse(equip_MaxAdf) + int.Parse(hideValue_Adf)).ToString();
                break;
        }

        return returnTypeValue;
    }

    //传入值获取当前隐藏属性的某个值属性
    public string ReturnHideTypeValue(string hideID ,string hideType) { 
        
        string hideValue = "0";
        string hidePropertyStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");
        string[] hideProperty = hidePropertyStr.Split(';');
        //隐藏属性
        /*
        1:血量上限
        2:物理攻击最大值
        3:物理防御最大值
        4:魔法防御最大值
        */
        //循环加入各个隐藏属性
        if (hidePropertyStr != "")
        {
            for (int i = 0; i <= hideProperty.Length - 1; i++)
            {
                string hidePropertyType = hideProperty[i].Split(',')[0];
                string hidePropertyValue = hideProperty[i].Split(',')[1];

                switch (hidePropertyType)
                {
                    //血量上限
                    case "1":
                        if (hideType == hidePropertyType)
                        {
                            hideValue = hidePropertyValue;
                        }
                        break;
                    //物理攻击最大值
                    case "2":
                        if (hideType == hidePropertyType)
                        {
                            hideValue = hidePropertyValue;
                        }
                        break;
                    case "3":
                    //物理防御最大值
                        if (hideType == hidePropertyType)
                        {
                            hideValue = hidePropertyValue;
                        }
                        break;
                    //魔法防御最大值
                    case "4":
                        if (hideType == hidePropertyType)
                        {
                            hideValue = hidePropertyValue;
                        }
                        break;
                }
            }
        }
        return hideValue;
    }

    //传入随机范围,生成一个随机数,越到后面的随机数获取概率越低
    public int ReturnEquipRamdomValue(int randomMinValue, int randomMaxValue, string hideID = "")
    {

        int randomChaValue = randomMaxValue - randomMinValue;
        //随机4次,获得取值范围
        /*
        0-25%       0.5
        26%-50%     0.3
        51%-75%     0.15
        76%-100%    0.05
        */
        float randomRangeValue = Random.value;
        float randomRangeValue_Now =0;
        if (randomRangeValue <= 0.5f)
        {
            //0-0.25f
            randomRangeValue_Now = Random.value / 4;

        }
        if (randomRangeValue > 0.5f && randomRangeValue <= 0.8f)
        {
            //0.25-0.5
            randomRangeValue_Now = Random.value / 4+0.25f;
        }
        if (randomRangeValue > 0.8f && randomRangeValue <= 0.95f)
        {
            //0.5-0.75
            randomRangeValue_Now = Random.value / 4+0.5f;
        }
        if (randomRangeValue > 0.95f && randomRangeValue <= 1f)
        {
            //0.75-1
            randomRangeValue_Now = Random.value / 4+0.75f;
        }

        //极品大师
        if (hideID != "") {
            float hintSkillProValue = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideSkillValue(hideID, "908");
            if(hintSkillProValue!=0){
                randomRangeValue_Now = randomRangeValue_Now * (1 + hintSkillProValue);
            }
        }
        if (randomRangeValue_Now > 1)
        {
            randomRangeValue_Now = 1;
        }

        //计算最终值
        int retunrnValue = (int)(randomMinValue + randomChaValue * randomRangeValue_Now);
        return retunrnValue;
    }

    //传入随机范围,生成一个随机数,越到后面的随机数获取概率越低
    public float ReturnEquipRamdomValue_float(float randomMinValue, float randomMaxValue, string hideID = "")
    {

        float randomChaValue = randomMaxValue - randomMinValue;
        //随机4次,获得取值范围
        /*
        0-25%       0.5
        26%-50%     0.3
        51%-75%     0.15
        76%-100%    0.05
        */
        float randomRangeValue = Random.value;
        float randomRangeValue_Now = 0;
        if (randomRangeValue <= 0.5f)
        {
            //0-0.25f
            randomRangeValue_Now = Random.value / 4;

        }
        if (randomRangeValue > 0.5f && randomRangeValue <= 0.8f)
        {
            //0.25-0.5
            randomRangeValue_Now = Random.value / 4 + 0.25f;
        }
        if (randomRangeValue > 0.8f && randomRangeValue <= 0.95f)
        {
            //0.5-0.75
            randomRangeValue_Now = Random.value / 4 + 0.5f;
        }
        if (randomRangeValue > 0.95f && randomRangeValue <= 1f)
        {
            //0.75-1
            randomRangeValue_Now = Random.value / 4 + 0.75f;
        }

        //极品大师
        if (hideID != "")
        {
            float hintSkillProValue = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideSkillValue(hideID, "908");
            if (hintSkillProValue != 0)
            {
                randomRangeValue_Now = randomRangeValue_Now * (1 + hintSkillProValue);
            }
        }
        if (randomRangeValue_Now > 1)
        {
            randomRangeValue_Now = 1;
        }

        //计算最终值
        float retunrnValue = (float)(randomMinValue + randomChaValue * randomRangeValue_Now);
        retunrnValue = (float)(System.Math.Round(retunrnValue, 3));
        return retunrnValue;
    }


    //传入随机范围,生成一个随机数(平均概率)
    public float ReturnRamdomValue_Float(float randomMinValue, float randomMaxValue)
    {

        float randomChaValue = randomMaxValue - randomMinValue;
        float randomRangeValue_Now = Random.value;

        //计算最终值
        float retunrnValue = randomMinValue + randomChaValue * randomRangeValue_Now;
        return retunrnValue;
    }

    //传入随机范围,生成一个随机数(平均概率)
    public int ReturnRamdomValue_Int(int randomMinValue, int randomMaxValue)
    {

        int randomChaValue = randomMaxValue - randomMinValue;
        float randomRangeValue_Now = Random.value;
        //System.Math.Round
        //计算最终值
        int retunrnValue = (int)(System.Math.Round(randomMinValue + randomChaValue * randomRangeValue_Now));
        return retunrnValue;
    }

    public float ReturnEquipHideSkillValue(string hideID, string skillID)
    {
        //获取当前装备的技能属性进行叠加
        if (hideID != "0" && hideID != "")
        {
            string hideProListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");
            string[] hideProperty = hideProListStr.Split(';');

            //循环加入之前的隐藏属性
            if (hideProListStr != "")
            {
                for (int y = 0; y <= hideProperty.Length - 1; y++)
                {
                    string[] hidePropertyList = hideProperty[y].Split(',');
                    if (hidePropertyList.Length >= 2) {
                        string hidePropertyType = hidePropertyList[0];
                        string hidePropertyValue = hidePropertyList[1];

                        if (hidePropertyType == "10001")
                        {
                            //获取当前
                            string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", hidePropertyValue, "Skill_Template");
                            if (skillType == "5")
                            {
                                string gameObjectParameter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", hidePropertyValue, "Skill_Template");
                                string[] hintSkillProList = gameObjectParameter.Split(';');

                                for (int z = 0; z < hintSkillProList.Length; z++)
                                {
                                    string hintSkillProType = hintSkillProList[z].Split(',')[0];
                                    string hintSkillProValue = hintSkillProList[z].Split(',')[1];

                                    if (hintSkillProType == skillID)
                                    {
                                        return float.Parse(hintSkillProValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return 0 ;
    }


    
    //获取角色当前某个对应的属性值
    public int ReturnRosePropertyValue(string propertyType) {

        switch (propertyType) { 
            //获取最大攻击力
            case "1":
                return Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMax;
            break;
        }
        return 0;
    }

    //获取某个收集道具是否被收集
    public bool ifShouJiItem(string itemID)
    {
        string shoujiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_5", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] shoujiList = shoujiStr.Split(',');
        for (int i = 0; i <= shoujiList.Length - 1; i++)
        {

            if (itemID == shoujiList[i])
            {
                return true;
            }
        }
        return false;
    }

    //收集值
    public bool AddShouJiItem(string itemID)
    {
        //BeiYong
        string shoujiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_5", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] shoujiList = shoujiStr.Split(',');

        //先判定
        bool ifShouJiItem = false;
        int startNum = 0;
        int chapter = 0;
        //获取当前
        string[] ShouJiItemList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ShouJiItemList", "GameMainValue").Split(';');

        for (int i = 0; i < ShouJiItemList.Length; i++)
        {
            //Debug.Log(" ShouJiItemList[i] = " + ShouJiItemList[i]);
            //循环判定ID
            string itemListID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemListID", "ID", ShouJiItemList[i], "ShouJiItemPro_Template");
            string nextID = itemListID;
            //Debug.Log("nextID = " + nextID);
            //获取下级ID
            do
            {
                //新建兑换道具
                string duibiItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", nextID, "ShouJiItem_Template");
                if (duibiItemID == itemID)
                {
                    ifShouJiItem = true;
                    startNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StartNum", "ID", nextID, "ShouJiItem_Template"));
                    chapter = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterNum", "ID", ShouJiItemList[i], "ShouJiItemPro_Template"));
                    //nextID = "0";   //立即跳出循环
                    Debug.Log("ID一致");
                    break;
                }
                else
                {
                    nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "ShouJiItem_Template");
                }

            }
            while (nextID != "0");

            if (ifShouJiItem)
            {
                break;
            }

        }

        //添加收集ID
        if (ifShouJiItem)
        {
            bool ifGetItem = true;
            for (int i = 0; i <= shoujiList.Length - 1; i++)
            {
                if (itemID == shoujiList[i])
                {
                    //已存在收藏ID不用重复激活
                    ifGetItem = false;
                }
            }

            if (ifGetItem)
            {
                if (shoujiStr == "")
                {
                    shoujiStr = itemID;
                }
                else
                {
                    shoujiStr = shoujiStr + "," + itemID;
                }
                //Debug.Log("shoujiStr = " + shoujiStr);
                //获取当前星数进行记录
                string startListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_6", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                if (startListStr == "0" || startListStr == "")
                {
                    startListStr = "0;0;0;0;0;0";
                    //Debug.Log("startListStr = " + startListStr);
                }
                string[] startList = startListStr.Split(';');
                if (chapter != 0)
                {
                    int nowChapStarNum = int.Parse(startList[chapter - 1]);
                    startList[chapter - 1] = (nowChapStarNum + startNum).ToString();
                    startListStr = "";
                    for (int i = 0; i < startList.Length; i++)
                    {
                        //Debug.Log("i = " + startList[i]);
                        if (startListStr == "")
                        {
                            startListStr = startList[i];
                            //Debug.Log(" startListStrPPPPP = " + startListStr);
                        }
                        else
                        {
                            startListStr = startListStr + ";" + startList[i];
                            //Debug.Log(" startListStrPPPPP = " + startListStr);
                        }
                    }
                }

                string shouJiItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", itemID, "Item_Template");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_342");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(shouJiItemName + langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint(shouJiItemName + "激活收藏道具成功！");
                //Debug.Log("startListStr = " + startListStr);
                //添加记录的道具ID
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_5", shoujiStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_6", startListStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            }
            //Debug.Log("添加道具");
            return true;
        }
        else
        {
            //Debug.Log("添加道具未收藏");
        }
        return true;
    }


    //核对收集
    public bool ShouJiJianYan()
    {

        //已拥有道具列表10101
        Debug.Log("Get_wwwSet.RoseID = " + Game_PublicClassVar.Get_wwwSet.RoseID);
        string shoujiItemListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_5", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] shoujiItemStrList = shoujiItemListStr.Split(',');

        //收藏的星数
        string shoujiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_6", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (shoujiStr == "0")
        {
            shoujiStr = "0;0;0;0;0;0";
        }

        Debug.Log("shoujiStr = " + shoujiStr);
        string[] shoujiList = shoujiStr.Split(';');

        //收藏的道具
        string shoujiItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ShouJiItemList", "GameMainValue");
        string[] shoujiItemList = shoujiItemStr.Split(';');
        Debug.Log("shoujiItemStr = " + shoujiItemStr);
        string chapterStr = "";
        for (int i = 0; i < shoujiList.Length; i++)
        {

            //循环判定ID
            string itemListID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemListID", "ID", shoujiItemList[i], "ShouJiItemPro_Template");
            string nextID = itemListID;
            int startNum_Chapter = 0;

            //Debug.Log("nextID = " + nextID);
            int iii = 0;
            //获取下级ID
            do
            {
                //新建兑换道具
                string duibiItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", nextID, "ShouJiItem_Template");
                //Debug.Log("nextID = " + nextID);

                for (int y = 0; y < shoujiItemStrList.Length; y++)
                {

                    if (shoujiItemStrList[y] == duibiItemID)
                    {
                        //获取星数
                        int startNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StartNum", "ID", nextID, "ShouJiItem_Template"));
                        startNum_Chapter = startNum_Chapter + startNum;
                        //Debug.Log("获得星数startNum_Chapter = " + startNum_Chapter + " nextID = " + nextID + "duibiItemID = " + duibiItemID);
                    }
                    else
                    {
                        //nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "ShouJiItem_Template");

                    }
                }
                nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "ShouJiItem_Template");
                iii = iii + 1;
                if (iii >= 50)
                {
                    nextID = "0";
                    Debug.Log("太多");
                }
            }
            while (nextID != "0");


            if (chapterStr != "")
            {
                chapterStr = chapterStr + ";" + startNum_Chapter.ToString();
                //Debug.Log("123 = " + chapterStr);
            }
            else
            {
                chapterStr = startNum_Chapter.ToString();
                //Debug.Log("456 = " + chapterStr);
            }
            //Debug.Log("chapterStr = " + chapterStr);
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_6", chapterStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        }
        return true;
    }


    //验证序列号是否正确
    public bool IfTrueXuLieHao(string[] xuliehaoList)
    {
        //验证账号信息
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (zhanghaoID != xuliehaoList[3])
        {
            Debug.Log("账号信息验证错误！");
            return false;
        }
        //验证时间
        /*
        if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus)
        {
            //string worldTime = Game_PublicClassVar.Get_wwwSet.GetWorldTime();
            string worldTime = Game_PublicClassVar.Get_wwwSet.time();
            if (worldTime != "0000000000" && worldTime != "1 = 0000000000")
            {
                Debug.Log("worldTime = " + worldTime);
                int chazhi = int.Parse(worldTime) - int.Parse(xuliehaoList[0]);
                if (chazhi <= 86400)
                {
                    return true;
                }
                else
                {
                    Game_PublicClassVar.Get_function_UI.GameHint("此序列号已过期");
                    Debug.Log("此序列号已过期");
                }

            }
            else
            {
                Game_PublicClassVar.Get_function_UI.GameHint("请链接网络在输入序列号!");
            }
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameHint("请链接网络在输入序列号");
        }
        */
        return false;
    }

    //获取某个序列号是否领取
    public bool ifGetXuLieHao(string xuLieHaoStr)
    {
        string[] xuLieHao = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XuLieHaoSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig").Split(';');
        for (int i = 0; i <= xuLieHao.Length - 1; i++)
        {
            if (xuLieHaoStr == xuLieHao[i])
            {
                return true;
            }
        }
        return false;
    }

    //写入序列号是否领取
    public void WriteXuLieHao(string xuLieHaoStr)
    {
        string xuLieHao = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XuLieHaoSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (xuLieHao == "")
        {
            xuLieHao = xuLieHaoStr;
        }
        else
        {
            xuLieHao = xuLieHao + ";" + xuLieHaoStr;
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XuLieHaoSet", xuLieHao, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
    }


    //写入交易号
    public void WritePayID(string payID,string payValue)
    {
        string payStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

        if (payStr != "")
        {
            payStr = payStr + ";" +payID + "," + payValue;
        }
        else {
            payStr = payID + "," + payValue;
        }

        //存入支付ID
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_1", payStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
    }

    //删除支付ID
    public void DeletePayID(string payID)
    {
        string payStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] payStrList = payStr.Split(';');
        string savePayStr = "";
        for (int i = 0; i < payStrList.Length; i++)
        {
            if (payStrList[i] != "0" && payStrList[i] != "")
            {
                string[] payList = payStrList[i].Split(',');
                if (payList[0] == payID)
                {

                }
                else
                {
                    if (savePayStr != "")
                    {
                        savePayStr = savePayStr + ";" + payStrList[i];
                    }
                    else
                    {
                        savePayStr = payStrList[i];
                    }
                }
            }
        }

        //存入支付ID
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_1", savePayStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
    }

    //根据订单获取充值额度
    public string DingDanReturnPayValue(string payID)
    {
        string payStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] payStrList = payStr.Split(';');
        for (int i = 0; i < payStrList.Length; i++)
        {
            if (payStrList[i] != "0" && payStrList[i] != "")
            {
                string[] payList = payStrList[i].Split(',');
                if (payList[0] == payID)
                {
                    return payList[1];
                }
            }
        }
        return "0";
    }

    //根据充值额度发送对应钻石
    public void DingDanSendPayValue(string payValue) { 

        int returnZuanShiValue = 0;
        switch (payValue) {

            case "9.8":
                returnZuanShiValue = 1000;
                //Game_PublicClassVar.Get_function_UI.GameGirdHint("匹配到了9.8");
                break;
            case "49.8":
                returnZuanShiValue = 6000;
                break;
            case "99.8":
                returnZuanShiValue = 13000;
                break;
            case "498":
                returnZuanShiValue = 75000;
                break;
            case "888":
                returnZuanShiValue = 145000;
                break;
        }

        //累计当前充值额度,发送指定钻石奖励
        Game_PublicClassVar.Get_function_Rose.AddRMB(returnZuanShiValue);
        Game_PublicClassVar.Get_function_Rose.AddPayValue(float.Parse(payValue),"51");
    }


    //召唤宠物（参数1：是否广播,宠物类型）
    public void RosePetCreate(int rosePetID, bool ifSpeak = true,int PetType = 0) {

        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        //判定当前宠物是否出战
        if(roseStatus.RosePetObj[rosePetID-1] != null){

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_304");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前宠物已经出战");
            return;
        }
        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", rosePetID.ToString(), "RosePet");
        string petModel = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetModel", "ID", petID, "Pet_Template");
        GameObject monsterSetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_RosePetSet;

        //获取怪物并设置位置
        if (petModel != "")
        {
            GameObject monsterObj = MonoBehaviour.Instantiate((GameObject)Resources.Load("PetSet/" + petModel, typeof(GameObject)));
            monsterObj.SetActive(false);
            monsterObj.transform.SetParent(monsterSetObj.transform);
            Vector3 CreateVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
            monsterObj.transform.position = CreateVec3;
            monsterObj.GetComponent<AIPet>().RosePet_ID = rosePetID.ToString();
            roseStatus.RosePetObj[rosePetID - 1] = monsterObj;
            addRosePetPositionObj(monsterObj);
            monsterObj.GetComponent<AIPet>().PetType = "0";
            monsterObj.GetComponent<AIPet>().AI_ID = long.Parse(petID);
            monsterObj.SetActive(true);
            //记录当前出战
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetStatus", "1", "ID", rosePetID.ToString(), "RosePet");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

            if (ifSpeak) {

                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_428");
                Game_PublicClassVar.Get_function_UI.GameHint(langStrHint_1);
                //Game_PublicClassVar.Get_function_UI.GameHint("召唤宠物成功！");

                //实例化一个特效
                GameObject zhaoHuanEffect = (GameObject)MonoBehaviour.Instantiate((GameObject)Resources.Load("Effect/Skill/Eff_Skill_ZhaoHuan_1", typeof(GameObject)));        //实例化特效
                zhaoHuanEffect.transform.position = CreateVec3;
                zhaoHuanEffect.transform.localScale = new Vector3(1, 1, 1);
            }

			//更新主界面
			Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().IfUpdatePetData = true;

            //设置宠物当前血量

            //设置出战冷却时间
            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ZhaoHuanCDStatus = true;
            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ZhaoHuanCDTime = 120;
            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ZhaoHuanCDSumTime = 0;


            //发送武器显示
            if (Application.loadedLevelName == "EnterGame")
            {
                MapThread_PlayerDataChange mapThread_PlayerDataChange = new MapThread_PlayerDataChange();
                mapThread_PlayerDataChange.ChangType = "2";
                string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", rosePetID.ToString(), "RosePet");
                string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", rosePetID.ToString(), "RosePet");
                mapThread_PlayerDataChange.ChangValue = petID + "," + petName  + "," + petLv;
                mapThread_PlayerDataChange.MapName = Application.loadedLevelName;
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20000204, mapThread_PlayerDataChange);
            }

            //触发出战无敌buff
            float nowPetWuDiCD = Game_PublicClassVar.Get_function_AI.Pet_GetZhaoHuanWuDiCD(rosePetID.ToString());
            if (nowPetWuDiCD == 0)
            {
                //给自身释放无敌buff
                GameObject nowChuZhanObj = Game_PublicClassVar.Get_function_AI.Pet_ReturnChuZhanObj();
                Game_PublicClassVar.Get_function_Skill.SkillBuff("90043010", nowChuZhanObj);
                Game_PublicClassVar.Get_function_Skill.SkillBuff("90043011", nowChuZhanObj);
                if (ifSpeak)
                {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_305");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("召唤宠物成功!激活15秒无敌时间!1分钟内只能激活一次!");
                }
                //Game_PublicClassVar.Get_function_Skill.SkillBuff(buffID, actObj.gameObject);
                //添加cd
                Game_PublicClassVar.Get_function_AI.Pet_AddZhaoHuanWuDiCD(rosePetID.ToString());
            }

        }
        else
        {
            Debug.Log("实例化的宠物为空");
        }
    }

    //删除出战宠物 （ifDestury表示 销毁模型）
    public void RosePetDelete(int rosePetID,bool ifDestury = true) {

        //Debug.Log("取消出战 rosePetID = " + rosePetID);
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        if (roseStatus.RosePetObj[rosePetID - 1] != null) {
            //删除宠物位置信息
            deleteRosePetPositionObj(roseStatus.RosePetObj[rosePetID - 1]);
            MonoBehaviour.Destroy(roseStatus.RosePetObj[rosePetID - 1].GetComponent<AIPet>().UI_Hp);
            if (ifDestury)
            {
                MonoBehaviour.Destroy(roseStatus.RosePetObj[rosePetID - 1]);
            }
            else {
                roseStatus.RosePetObj[rosePetID - 1] = null;
            }
            
            roseStatus.RosePetObj[rosePetID - 1] = null;
        }

        //记录取消出战
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetStatus", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

        //更新主界面
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().IfUpdatePetData = true;
    }

    //放生清理宠物
    public void RosePetClearn(int rosePetID)
    {
        RosePetDelete(rosePetID);
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetStatus", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetID", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetLv", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetExp", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetName", "", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetPingFen", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Hp", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Act", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Def", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Adf", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_ActSpeed", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_ChengZhang", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetSkill", "0", "ID", rosePetID.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");
        //更新当前任务
        Game_PublicClassVar.Get_function_Task.updataTaskItemID();
    }

    //获取当前出战的宠物数量
    public int GetRosePetFightNum() {

        int fightNum = 0;
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        for (int i = 0; i <= roseStatus.RosePetObj.Length - 1; i++)
        {
            if (roseStatus.RosePetObj[i] != null)
            {
                fightNum = fightNum + 1;
            }
        }
        return fightNum;
    }

    //获取当前宠物总数
    public int GetRosePetNum() {

        int nowPetNum = 0;

        //显示宠物列表
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePetMaxNum; i++)
        {
            string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", i.ToString(), "RosePet");
            if (petID != "0")
            {
                nowPetNum = nowPetNum + 1;
            }
        }

        return nowPetNum;

    }

    //获取当前第一个出战的宠物ID
    public int GetRosePetFightFirstID()
    {
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        for (int i = 0; i <= roseStatus.RosePetObj.Length - 1; i++)
        {
            if (roseStatus.RosePetObj[i] != null)
            {
                return i + 1;
            }
        }
        return -1;
    }

    //获取当前出战宠物的Obj
    public GameObject GetRosePetFightObj() {
        GameObject RoseChuZhanPet_NowObj = null;
        int petFightSpaceID = GetRosePetFightFirstID();
        if (petFightSpaceID != -1)
        {
            RoseChuZhanPet_NowObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj[petFightSpaceID-1];
        }
        return RoseChuZhanPet_NowObj;
    }

    //获取当前出战召唤宠物的Obj
    public GameObject[] GetRoseZhaoHuanFightObjList() {

        GameObject petSet = Game_PublicClassVar.Get_game_PositionVar.Obj_RoseCreatePetSet;
        GameObject[] objList = new GameObject[petSet.transform.childCount];
        for (int i = 0; i < petSet.transform.childCount; i++)
        {
            GameObject go = petSet.transform.GetChild(i).gameObject;
            objList[i] = go;
        }
        //Debug.Log("petSet.transform.childCount = " + petSet.transform.childCount);
        return objList;

    }

    //清理全部
    public void RosePetClearnAll() {

        GameObject monsterSetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_RosePetSet;
        //循环删除宠物
        for (int i = 0; i < monsterSetObj.transform.childCount; i++)
        {
            GameObject go = monsterSetObj.transform.GetChild(i).gameObject;
            //清空AI血条显示
            MonoBehaviour.Destroy(go.GetComponent<AIPet>().UI_Hp);
            MonoBehaviour.Destroy(go);
        }
    }

    //添加宠物跟随坐标点
    public void addRosePetPositionObj(GameObject addPetObj) { 

        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        for(int i = 0; i < roseStatus.RosePetPositionObj.Length;i++){
            if (roseStatus.RosePetPositionObj[i] == null) {
                addPetObj.GetComponent<AIPet>().StartPositionObj = roseStatus.RosePetfollowPositionObj[i];
                roseStatus.RosePetPositionObj[i] = addPetObj;
                return;
            }
        }

        addPetObj.GetComponent<AIPet>().StartPositionObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;
    }

    //删除宠物跟随坐标点
    public void deleteRosePetPositionObj(GameObject deletePetObj) {

        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        for (int i = 0; i < roseStatus.RosePetPositionObj.Length; i++)
        {
            if (roseStatus.RosePetPositionObj[i] == deletePetObj)
            {
                roseStatus.RosePetPositionObj[i] = null;
                return;
            }
        }
    }

    //存储当前角色信息GameConfig.Xml
    public void SaveGameConfig_Rose(string roseID,string key,string value)
    {

        string set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName + "/";
        //Debug.Log("set_XmlPath = " + set_XmlPath);
        Game_PublicClassVar.Get_xmlScript.Xml_SetDate(key, value, "ID", roseID, set_XmlPath + "GameConfig.xml");
        //Debug.Log("设置Name");
    }

    //序列号检测
    public void JianCeShouJi()
    {

        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;

        //将掉落的道具ID添加到背包内
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseStoreHouse");
            if (Rdate != "0" && Rdate != "")
            {
                AddShouJiItem(Rdate);
            }
        }

        //检测背包内的
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (Rdate != "0" && Rdate != "")
            {
                AddShouJiItem(Rdate);
            }
        }

        //检测当前装备的EquipItemID
        for (int i = 1; i <= 13; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("EquipItemID", "ID", i.ToString(), "RoseEquip");
            if (Rdate != "0" && Rdate != "")
            {
                AddShouJiItem(Rdate);
            }
        }
    }


    //扣除指定SP值
    public bool CostRoseSpValue(int costSPValue)
    { 
        //获取自己拥有的SP
        int RoseSP = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        //扣除SP,学习对应的技能
        if (RoseSP >= costSPValue)
        {
            RoseSP = RoseSP - costSPValue;
        }
        else {
            return false;
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SP", RoseSP.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        return true;
    }


    //增加制造相关的熟练度
    public void AddMakeProficiencyValue(string addType,int addValue) {

        //类型为4不加熟练度
        if (addType == "4") {
            return;
        }

        int nowficiencyValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Proficiency_" + addType, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData"));
        addValue = nowficiencyValue + addValue;
        int proficiencyTypeValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "MakeItemMaxValue", "GameMainValue"));
        if (addValue > proficiencyTypeValue)
        {
            addValue = proficiencyTypeValue;
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Proficiency_" + addType, addValue.ToString(), "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        switch (addType) {

            //增加
            case "1":
                //写入成就(熟练度)
                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("202", "0", addValue.ToString());
                break;
            case "2":
                //写入成就(熟练度)
                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("203", "0", addValue.ToString());
                break;
        }
    }

    //增加制造相关的制造ID
    public void AddMakeProficiencyID(string addValue)
    {
        string addValueID = "";
        string[] addValueList = addValue.Split(',');
        for (int i = 0; i < addValueList.Length; i++) {
            addValueID = addValueList[i];

            string proficiencyType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyType", "ID", addValueID, "EquipMake_Template");
            string nowficiencyID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_" + proficiencyType, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");

            if (nowficiencyID != "" && nowficiencyID != "0") {
                addValueID = nowficiencyID + "," + addValueID;
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ProficiencyID_" + proficiencyType, addValueID.ToString(), "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");
        }
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
    }

    //获取当前制造ID是否已经学习
    public bool GetMakeProficiencyIDStatus(string addValue)
    {
        string addValueID = "";
        string[] addValueList = addValue.Split(',');
        for (int i = 0; i < addValueList.Length; i++)
        {
            addValueID = addValueList[i];

            string proficiencyType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyType", "ID", addValueID, "EquipMake_Template");
            string nowficiencyID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_" + proficiencyType, "ID", Game_PublicClassVar.Get_game_PositionVar.RoseID, "RoseData");

            string[] nowficiencyIDList = nowficiencyID.Split(',');
            for (int z = 0; z < nowficiencyIDList.Length; z++) {
                if (nowficiencyIDList[z] == addValue) {
                    return true;
                }
            }
        }
        return false;
    }


    //进入角色隐身状态
    public void EnterRoseInvisible() {

        Rose_Status rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        Rose_Bone rose_Bone = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>();

        if (!rose_Status.RoseInvisibleStatus)
        {
            rose_Status.RoseInvisibleStatus = true;
            //设置隐身效果
            switch (rose_Status.RoseOcc)
            {
                //战士
                case "1":
                    for (int i = 0; i < rose_Bone.ZhanShi_Skin.Length; i++)
                    {
                        if (i<rose_Bone.ZhanShi_InvisibleShader.Length ) {
                            rose_Bone.ZhanShi_Skin[i].material = rose_Bone.ZhanShi_InvisibleShader[i];
                        }

                    }
                    break;

                //法师
                case "2":
                    for (int i = 0; i < rose_Bone.FaShi_Skin.Length; i++)
                    {
                        if (i < rose_Bone.FaShi_InvisibleShader.Length)
                        {
                            rose_Bone.FaShi_Skin[i].material = rose_Bone.FaShi_InvisibleShader[i];
                        }
                    }
                    break;

                //法师
                case "3":
                    for (int i = 0; i < rose_Bone.LieRen_Skin.Length; i++)
                    {
                        if (i < rose_Bone.LieRen_InvisibleShader.Length)
                        {
                            rose_Bone.LieRen_Skin[i].material = rose_Bone.LieRen_InvisibleShader[i];
                        }
                    }
                    break;
            }
        }
    }

    //退出角色隐身状态
    public void ExitRoseInvisible() {

        Rose_Status rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        Rose_Bone rose_Bone = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>();

        if (rose_Status.RoseInvisibleStatus)
        {
            rose_Status.RoseInvisibleStatus = false;
            //还原隐身效果
            switch (rose_Status.RoseOcc)
            {
                //战士
                case "1":
                    for (int i = 0; i < rose_Bone.ZhanShi_Skin.Length; i++)
                    {
                        if (i < rose_Bone.ZhanShi_SelfShader.Length)
                        {
                            rose_Bone.ZhanShi_Skin[i].material = rose_Bone.ZhanShi_SelfShader[i];
                        }
                    }
                    break;

                //法师
                case "2":
                    for (int i = 0; i < rose_Bone.FaShi_Skin.Length; i++)
                    {
                        if (i < rose_Bone.FaShi_SelfShader.Length)
                        {
                            rose_Bone.FaShi_Skin[i].material = rose_Bone.FaShi_SelfShader[i];
                        }
                    }
                    break;

                //猎人
                case "3":
                    for (int i = 0; i < rose_Bone.LieRen_Skin.Length; i++)
                    {
                        if (i < rose_Bone.LieRen_SelfShader.Length)
                        {
                            rose_Bone.LieRen_Skin[i].material = rose_Bone.LieRen_SelfShader[i];
                        }
                    }
                    break;
            }
        }
    }

    //更换武器模型
    public void RoseWeaponModel(string showWeaponID) {
        
        //Debug.Log("123321=RoseWeaponModelRoseWeaponModelRoseWeaponModelRoseWeaponModel");

        //清除多余武器
        GameObject targetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseWeaponPosi.gameObject;
        for (int i = 0; i < targetObj.transform.childCount; i++)
        {
            GameObject go = targetObj.transform.GetChild(i).gameObject;
            if (go != Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseWeaponBaseModel)
            {
                MonoBehaviour.Destroy(go);
            }
        }

        //清除多余武器（UI模型显示）
        GameObject targetModelObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponPosi.gameObject;
        for (int i = 0; i < targetModelObj.transform.childCount; i++)
        {
            GameObject go = targetModelObj.transform.GetChild(i).gameObject;
            if (go != Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponBaseModel)
            {
                MonoBehaviour.Destroy(go);
            }
        }

        //显示角色武器
        GameObject weaponObj = (GameObject)Resources.Load("3DModel/WeaponModel/" + showWeaponID, typeof(GameObject));
        if (weaponObj == null)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseWeaponBaseModel.SetActive(true);
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponBaseModel.SetActive(true);
            return;
        }

        GameObject SkillObject_p = (GameObject)MonoBehaviour.Instantiate(weaponObj);
        SkillObject_p.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseWeaponBaseModel.SetActive(false);
        SkillObject_p.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseWeaponPosi.transform);
  
        //设置技能位置
        SkillObject_p.transform.localRotation = Quaternion.Euler(90, 0, 0);
        SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);
        SkillObject_p.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        //获取当前职业
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseOcc == "1")
        {
            SkillObject_p.transform.localScale = new Vector3(2f, 2f, 2f);
        }

        SkillObject_p.SetActive(true);



        //设置角色UI的武器模型
        if (weaponObj == null) {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponBaseModel.SetActive(true);
            return;
        }

        GameObject SkillObject_UI = (GameObject)MonoBehaviour.Instantiate(weaponObj);
        SkillObject_UI.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponBaseModel.SetActive(false);
        SkillObject_UI.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponPosi.transform);

        //设置技能位置
        SkillObject_UI.transform.localRotation = Quaternion.Euler(90, 0, 0);
        SkillObject_UI.transform.localPosition = new Vector3(0, 0, 0);
        SkillObject_UI.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        SkillObject_UI.SetActive(true);
        SkillObject_UI.layer = 16;

    }

    //更换武器模型（展示其他玩家的）
    public void PlayerRoseWeaponModelUIShow(string showWeaponID)
    {
        //清除多余武器（UI模型显示）
        GameObject targetModelObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShowOtherPlayerWeapon_Posi.gameObject;
        for (int i = 0; i < targetModelObj.transform.childCount; i++)
        {
            GameObject go = targetModelObj.transform.GetChild(i).gameObject;
            if (go != Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShowOtherPlayerWeapon_Model)
            {
                MonoBehaviour.Destroy(go);
            }
        }

        //设置角色UI的武器模型
        GameObject weaponObj = (GameObject)Resources.Load("3DModel/WeaponModel/" + showWeaponID, typeof(GameObject));
        if (weaponObj == null)
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShowOtherPlayerWeapon_Model.SetActive(true);
            return;
        }

        GameObject SkillObject_UI = (GameObject)MonoBehaviour.Instantiate(weaponObj);
        SkillObject_UI.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShowOtherPlayerWeapon_Model.SetActive(false);
        SkillObject_UI.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShowOtherPlayerWeapon_Posi.transform);

        //设置位置
        SkillObject_UI.transform.localRotation = Quaternion.Euler(90, 0, 0);
        SkillObject_UI.transform.localPosition = new Vector3(0, 0, 0);
        SkillObject_UI.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        SkillObject_UI.SetActive(true);
        SkillObject_UI.layer = 16;

    }

    //获取每日礼包的ID
    public string GetMeiRiLiBao() {

        //获取等级
        string libaoXuHao = "";
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv >= 1) {
            libaoXuHao = "1";
        }
        if (roseLv >= 18)
        {
            libaoXuHao = "2";
        }
        if (roseLv >= 30)
        {
            libaoXuHao = "3";
        }
        if (roseLv >= 40)
        {
            libaoXuHao = "4";
        }
        if (roseLv >= 50)
        {
            libaoXuHao = "5";
        }

        if (libaoXuHao != "")
        {
            string libaoStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "MeiRiLiBao_" + libaoXuHao, "GameMainValue");
            //Debug.Log("libaoStr = " + libaoStr);
            //string libaoStr = "2001;2002;2003;2004;2005;2006;2007;2008";
            string saveStr = "";
            string[] libaoStrList = libaoStr.Split(';');
            ArrayList list = new ArrayList();
            for (int i = 0; i < libaoStrList.Length; i++)
            {
                list.Add(libaoStrList[i]);
            }
            //Debug.Log("list = " + list.Count);
            for (int i = 1; i <= 6; i++) {

                int ranValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, list.Count-1);
                saveStr = saveStr + list[ranValue] + ",0;";
                list.Remove(list[ranValue]);
            }

            if (saveStr!="") {
                saveStr = saveStr.Substring(0, saveStr.Length - 1);
            }
            return saveStr;
        }

        return "";
    }

    public bool IfLvLiBao(string libaoID) {

        string libaoStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LvLiBao", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        string[] libaoList = libaoStr.Split(';');
        for (int i = 0; i < libaoList.Length;i++ )
        {
            if (libaoList[i] == libaoID) {
                return true;
            }
        }

        return false;
    }

    //只有场景ID的藏宝图
    public string GetCangBaoTuMapStr()
    {

        //获取当前关卡ID
        string chapterStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PVEChapter", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        int zhangjieQianStr = 10000;
        string guanka = chapterStr.Split(';')[0];
        guanka = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, int.Parse(guanka)).ToString();
        switch (guanka) {
            case "1":
                zhangjieQianStr = 10000;
                break;
            case "2":
                zhangjieQianStr = 20000;
                break;
            case "3":
                zhangjieQianStr = 30000;
                break;
            case "4":
                zhangjieQianStr = 40000;
                break;
            case "5":
                zhangjieQianStr = 50000;
                break;
        }

        int cangbaotu = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 6);
        zhangjieQianStr = zhangjieQianStr + cangbaotu;

        return zhangjieQianStr.ToString();

    }


    //带坐标的藏宝图
    public string GetCangBaoTuStr() {

        string[] mapList = new string[8];
        mapList[0] = "10001,20,55,110";
        mapList[1] = "10001,20,112,110";
        mapList[2] = "10001,30,-15,136";
        mapList[3] = "10001,25,-6,63";
        mapList[4] = "10001,20,-5,-13";
        mapList[5] = "10001,30,52,6";
        mapList[6] = "10001,10,102,6";
        mapList[7] = "10001,10,128,16";

        //随机获取一个坐标点
        int cangbaotu = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, 7);
        string[] cangbaotuList = mapList[cangbaotu].Split(',');
        int fanweiValue = int.Parse(cangbaotuList[1]);
        int tu_X = int.Parse(cangbaotuList[2]);
        int tu_Y = int.Parse(cangbaotuList[3]);
        int random_Tu_X = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(tu_X - fanweiValue, tu_X + fanweiValue);
        int random_Tu_Y = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(tu_Y - fanweiValue, tu_Y + fanweiValue);
        string cangBaoTuStr = cangbaotuList[0] + "," +random_Tu_X + "," + random_Tu_Y;
        return cangBaoTuStr;

    }

    //移动到指定地图
    public void RoseMoveTargetMap(string ScenceID) {
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Status = false;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", ScenceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        string SceneTransferID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ScenceTransferID", "ID", ScenceID, "Scene_Template");
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = SceneTransferID;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();
    }

    public void AddMiJingValue(int addValue) {

        Rose_Proprety rose_Proprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        rose_Proprety.Rose_MijingValue = rose_Proprety.Rose_MijingValue + addValue;

    }

    //获取背包的指定格子位置是否有宝石
    public bool GetBagGemStatus(string bagSpaceNum ) {
        //判定装备是否有宝石
        string ItemGemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", bagSpaceNum, "RoseBag");
        if (ItemGemID != "" && ItemGemID != "0")
        {
            string[] gemList = ItemGemID.Split(',');
            for (int y = 0; y < gemList.Length; y++)
            {
                if (gemList[y] != "0" && gemList[y] != "")
                {
                    return true;
                }
            }
        }
        return false;
    }

    //添加称号
    public void ChengHao_Add(string chenghaoID) {
        if (!ChengHao_Get(chenghaoID)) {
            string chengHaoIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengHaoIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            if (chengHaoIDSet == "")
            {
                chengHaoIDSet = chenghaoID;
            }
            else {
                chengHaoIDSet = chenghaoID + ";" + chengHaoIDSet;
            }
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengHaoIDSet", chengHaoIDSet,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_306");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("添加称号成功！请到设置界面查看称号列表并使用");
        }
    }

    //获取是否有此称号
    public bool ChengHao_Get(string chenghaoID)
    {
        string chengHaoIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengHaoIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        if (Game_PublicClassVar.Get_gameLinkServerObj.ServerPaiHangChengHao != "") {
            if (chengHaoIDSet != "")
            {
                chengHaoIDSet = chengHaoIDSet + ";" + Game_PublicClassVar.Get_gameLinkServerObj.ServerPaiHangChengHao;
            }
            else {
                chengHaoIDSet = Game_PublicClassVar.Get_gameLinkServerObj.ServerPaiHangChengHao;
            }
        }

        if (chengHaoIDSet != "")
        {
            string[] chengHaoIDList = chengHaoIDSet.Split(';');
            for (int i = 0;i< chengHaoIDList.Length;i++) {
                if (chengHaoIDList[i] == chenghaoID) {
                    return true;
                }
            }
        }
        return false;
    }

    //使用称号
    public void ChengHao_Use(string chenghaoID)
    {
        if (ChengHao_Get(chenghaoID))
        {
            string chenghaoName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", chenghaoID, "ChengHao_Template");
            string chenghaoIamge = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Iamge", "ID", chenghaoID, "ChengHao_Template");
            if (chenghaoIamge != "" && chenghaoIamge != null && chenghaoIamge != "0")
            {
                object obj = Resources.Load("ChengWeiIcon/" + chenghaoIamge, typeof(Sprite));
                Sprite itemIcon = obj as Sprite;
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Obj_ChengWei_Img.GetComponent<Image>().sprite = itemIcon;
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Obj_ChengWeiNameSet.SetActive(false);
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Obj_ChengWei_Img.GetComponent<Image>().SetNativeSize();
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Obj_ChengWei_Img.SetActive(true);
            }
            else {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Obj_ChengWeiNameSet.SetActive(true);
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Obj_ChengWei_Img.SetActive(false);
            }

            //称号称谓
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Rose_ChengWei = chenghaoName;
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().RoseChengWeiStatus = true;
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().ChengWeiID = chenghaoID;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowChengHaoID", chenghaoID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            //发送称谓显示
            if (Application.loadedLevelName == "EnterGame") {
                MapThread_PlayerDataChange mapThread_PlayerDataChange = new MapThread_PlayerDataChange();
                mapThread_PlayerDataChange.ChangType = "3";
                mapThread_PlayerDataChange.ChangValue = chenghaoID;
                mapThread_PlayerDataChange.MapName = Application.loadedLevelName;
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20000204, mapThread_PlayerDataChange);
            }
        }
    }

    //称号为空
    public void ChengHao_Null()
    {
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Rose_ChengWei = "";
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().RoseChengWeiStatus = true;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowChengHaoID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
    }

    //添加精灵
    public void JingLing_Add(string jingLingID) {

        string jingLingStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JingLingIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        bool addStatus = true;
        if (jingLingStr!="") {
            string[] jingLingIDList = jingLingStr.Split(';');
            for (int i = 0; i < jingLingIDList.Length; i++) {
                if (JingLing_JianCe(jingLingID)) {
                    addStatus = false;
                }
            }
        }

        if (addStatus) {

            if (jingLingStr != "")
            {
                jingLingStr = jingLingStr + ";" + jingLingID;
            }
            else {
                jingLingStr = jingLingID;
            }

            //存储
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JingLingIDSet", jingLingStr,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            //提示激活
            string jinglingName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", jingLingID, "Spirit_Template");

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_307");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + jinglingName);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你!激活精灵:" + jinglingName);

            GameObject hintObj = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_JingLingHintObj);
            hintObj.GetComponent<UI_JingLingHintObj>().JingLingID = jingLingID;
            hintObj.GetComponent<UI_JingLingHintObj>().ShowJingLing();
            hintObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet.transform);
            hintObj.transform.localScale = new Vector3(1, 1, 1);
            hintObj.transform.localPosition = new Vector3(683, 150, 0);
        }
    }

    //检测精灵是否存在
    public bool JingLing_JianCe(string jingLingID) {

        string jingLingStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JingLingIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        if (jingLingStr != "")
        {
            string[] jingLingIDList = jingLingStr.Split(';');
            for (int i = 0; i < jingLingIDList.Length; i++) {
                if (jingLingID == jingLingIDList[i])
                {
                    return true;
                }
            }
        }

        return false;
    }


    //装备精灵(目前只支持1个精灵装备，需要调整多个精灵要改代码xxxx;xxxx;xxx即可)
    public bool JingLing_Equip(string jingLingID) {

        string nowJingLingStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JingLingEquipID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (nowJingLingStr.Contains(jingLingID)) {
            //当前已经装备了此精灵
            return false;
        }

        if (JingLing_JianCe(jingLingID))
        {
            string jingLingSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkill","ID", jingLingID, "Spirit_Template");
            if (jingLingSkill != "0" && jingLingSkill != "")
            {
                Game_PublicClassVar.Get_function_Skill.UpdataMainUIEquipSkillID(jingLingSkill);

                //判定技能栏是否打开,如果打开就关闭,防止出现BUG（装备技能卸下重装会叠加skillADDID）
                if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseSkill_Status)
                {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseSkill();
                }

                //更新玩家主动和被动技能
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().UpdataPassiveSkillStatus = true;
                Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;

                //更新装备技能（放在这里需要上面先写入套装技能ID后再进行更新）
                Game_PublicClassVar.Get_function_Skill.UpdataEquipSkillID();

                //提示
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_168");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("激活精灵属性成功!");

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JingLingEquipID", jingLingID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

                return true;

            }
            else {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_169");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("被动精灵无法激活!");
                return false;
            }

        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_170");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("此精灵暂未拥有!");
            return false;
        }
    }



	public void SavePlayAllData(Pro_PlayAllData playData)
	{
		//读取RoseData
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Lv", playData.Write_Lv,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Name", playData.Write_Name,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhangHaoID", playData.Write_ZhangHaoID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GoldNum", playData.Write_GoldNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMB", playData.Write_RMB,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMBPayValue", playData.Write_RMBPayValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MaoXianJiaExp", playData.Write_MaoXianJiaExp, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YueKa", playData.Write_YueKa, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YueKaDayStatus", playData.Write_YueKaDayStatus,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

		//--
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HuoLi", playData.Write_HuoLi,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		//--

		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TiLi", playData.Write_TiLi,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MaxTiLi", playData.Write_MaxTiLi,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

		//--
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SkillSP", playData.Write_SkillSP,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		//--

		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SP", playData.Write_SP,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseExpNow", playData.Write_RoseExpNow,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseHpNow", playData.Write_RoseHpNow,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseOccupation", playData.Write_RoseOccupation,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("StoryStatus",playData.Write_StoryStatus, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", playData.Write_NowMapName,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapPositionName", playData.Write_NowMapPositionName,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("OffGameTime", playData.Write_OffGameTime,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskID", playData.Write_AchievementTaskID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", playData.Write_AchievementTaskValue,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

		//--
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiMenTaskNum", playData.Write_ShiMenTaskNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiMenNextTaskID", playData.Write_ShiMenNextTaskID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		//--


		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CompleteTaskID", playData.Write_CompleteTaskID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillID", playData.Write_LearnSkillID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipSkillID", playData.Write_EquipSkillID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipSuitSkillID", playData.Write_EquipSuitSkillID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillIDSet", playData.Write_LearnSkillIDSet,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipSkillIDSet", playData.Write_EquipSkillIDSet,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");


		//--
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnTianFuID", playData.Write_LearnTianFuID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnTianSkillID", playData.Write_LearnTianSkillID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //--


        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SceneItemID", playData.Write_SceneItemID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PVEChapter", playData.Write_PVEChapter,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");


		//--
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TanSuoListID", playData.Write_TanSuoListID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		//--

		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SpecialEventTime", playData.Write_SpecialEventTime,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SpecialEventOpenTime", playData.Write_SpecialEventOpenTime,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

		//--
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Proficiency_1", playData.Write_Proficiency_1,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Proficiency_2", playData.Write_Proficiency_2,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Proficiency_3", playData.Write_Proficiency_3,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ProficiencyID_1", playData.Write_ProficiencyID_1,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ProficiencyID_2", playData.Write_ProficiencyID_2,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ProficiencyID_3", playData.Write_ProficiencyID_3,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QiTianDengLuStatus", playData.Write_QiTianDengLuStatus,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QiTianDengLu", playData.Write_QiTianDengLu,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LvLiBao", playData.Write_LvLiBao,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("StroeHouseMaxNum", playData.Write_StroeHouseMaxNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetAddMaxNum", playData.Write_PetAddMaxNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJingLv", playData.Write_DaMiJingLv,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJingLvKillTime", playData.Write_DaMiJingLvKillTime, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJingRewardLv", playData.Write_DaMiJingRewardLv,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianTaskNum", playData.Write_ShiLianTaskNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianTaskStatus", playData.Write_ShiLianTaskStatus,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiLianNextTaskID", playData.Write_ShiLianNextTaskID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengHaoIDSet", playData.Write_ChengHaoIDSet,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowChengHaoID", playData.Write_NowChengHaoID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JingLingIDSet", playData.Write_JingLingIDSet,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JingLingEquipID", playData.Write_JingLingEquipID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XiLianDaShiShuLian", playData.Write_XiLianDaShiShuLian, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XiLianDaShiIDSet", playData.Write_XiLianDaShiIDSet, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LingPaiRewardID", playData.Write_LingPaiRewardID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetXiuLian", playData.Write_PetXiuLian, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_ShangHaiHightID", playData.Write_FuBen_ShangHaiHightID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_ShangHaiRewardID", playData.Write_FuBen_ShangHaiRewardID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_ShangHaiLvRewardSet", playData.Write_FuBen_ShangHaiLvRewardSet, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JueXingExp", playData.Write_JueXingExp, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JueXingJiHuoID", playData.Write_JueXingJiHuoID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanSeIDSet", playData.Write_YanSeIDSet, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowYanSeID", playData.Write_NowYanSeID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowYanSeHairID", playData.Write_NowYanSeHairID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShengXiaoSet", playData.Write_ShengXiaoSet, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhenYing", playData.Write_ZhenYing, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ProficiencyID_4", playData.Write_ProficiencyID_4, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhuLingIDSet", playData.Write_ZhuLingIDSet, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        
        //--

        //存储数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

		//读取RoseConfig
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainUITaskID", playData.Write_MainUITaskID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseEquipHideNumID", playData.Write_RoseEquipHideNumID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DengLuReward", playData.Write_DengLuReward, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DengLuDayStatus", playData.Write_DengLuDayStatus,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhiChiZuoZheID", playData.Write_ZhiChiZuoZheID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", playData.Write_DeathMonsterID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FirstEnterGame", playData.Write_FirstEnterGame,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_2", playData.Write_BeiYong_2,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_5", playData.Write_BeiYong_5,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_6", playData.Write_BeiYong_6,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_7", playData.Write_BeiYong_7,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_8", playData.Write_BeiYong_8,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_9", playData.Write_BeiYong_9,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GetGoldSum", playData.Write_GetGoldSum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GetZuanShiSum", playData.Write_GetZuanShiSum, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CostGoldSum", playData.Write_CostGoldSum, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CostZuanShiSum", playData.Write_CostZuanShiSum, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

        //存储数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

        //读取RoseDayReward
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryExp", playData.Write_CountryExp, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryHonor", playData.Write_CountryHonor, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryLv", playData.Write_CountryLv, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ExpNum", playData.Write_Day_ExpNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ExpTime", playData.Write_Day_ExpTime,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_GoldNum", playData.Write_Day_GoldNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_GoldTime", playData.Write_Day_GoldTime,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", playData.Write_ChouKaTime_One,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", playData.Write_ChouKaTime_Ten,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_FuBenNum", playData.Write_Day_FuBenNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_1", playData.Write_FenXiang_1,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_3", playData.Write_FenXiang_3,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_2", playData.Write_FenXiang_2,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskID", playData.Write_DayTaskID,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskValue", playData.Write_DayTaskValue,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskHuoYueValue", playData.Write_DayTaskHuoYueValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskCommonHuoYueRewardID", playData.Write_DayTaskCommonHuoYueRewardID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");


        //--
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MeiRiLiBao", playData.Write_MeiRiLiBao,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MeiRiCangBaoNum", playData.Write_MeiRiCangBaoNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MeiRiCangBaoTrueChestNum", playData.Write_MeiRiCangBaoTrueChestNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FuBen_1_DayNum", playData.Write_FuBen_1_DayNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DaMiJing_DayNum", playData.Write_DaMiJing_DayNum,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");


        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayPayValue", playData.Write_DayPayValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Com", playData.Write_QianDaoNum_Com, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Pay", playData.Write_QianDaoNum_Pay, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Com_Day", playData.Write_QianDaoNum_Com_Day, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QianDaoNum_Pay_Day", playData.Write_QianDaoNum_Pay_Day, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //--

        //存储数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

		//存储成就
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ComChengJiuID", playData.Write_ChengJiuID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ComChengJiuRewardID", playData.Write_ChengJiuRewardID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_1", playData.Write_ChengJiu_1, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_2", playData.Write_ChengJiu_2, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_3", playData.Write_ChengJiu_3, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_4", playData.Write_ChengJiu_4, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_5", playData.Write_ChengJiu_5, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_6", playData.Write_ChengJiu_6, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_7", playData.Write_ChengJiu_7, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_101", playData.Write_ChengJiu_101, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_102", playData.Write_ChengJiu_102, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_103", playData.Write_ChengJiu_103, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_104", playData.Write_ChengJiu_104, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_105", playData.Write_ChengJiu_105, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_106", playData.Write_ChengJiu_106, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_107", playData.Write_ChengJiu_107, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_108", playData.Write_ChengJiu_108, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_109", playData.Write_ChengJiu_109, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_110", playData.Write_ChengJiu_110, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_111", playData.Write_ChengJiu_111, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_201", playData.Write_ChengJiu_201, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_202", playData.Write_ChengJiu_202, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_203", playData.Write_ChengJiu_203, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_204", playData.Write_ChengJiu_204, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_205", playData.Write_ChengJiu_205, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_206", playData.Write_ChengJiu_206, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_207", playData.Write_ChengJiu_207, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_208", playData.Write_ChengJiu_208, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_209", playData.Write_ChengJiu_209, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_210", playData.Write_ChengJiu_210, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_211", playData.Write_ChengJiu_211, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_212", playData.Write_ChengJiu_212, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_220", playData.Write_ChengJiu_220, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_221", playData.Write_ChengJiu_221, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_222", playData.Write_ChengJiu_222, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_223", playData.Write_ChengJiu_223, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_224", playData.Write_ChengJiu_224, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_225", playData.Write_ChengJiu_225, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_226", playData.Write_ChengJiu_226, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_227", playData.Write_ChengJiu_227, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_228", playData.Write_ChengJiu_228, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_229", playData.Write_ChengJiu_229, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChengJiu_250", playData.Write_ChengJiu_250, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");


        //存储数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseChengJiu");

        //存储牧场相关
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureLv", playData.Write_PastureLv, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureExp", playData.Write_PastureExp, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureGold", playData.Write_PastureGold, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiLv", playData.Write_ZuoQiLv, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiPiFuSet", playData.Write_ZuoQiPiFuSet, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_1", playData.Write_ZuoQiZiZhi_1, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_2", playData.Write_ZuoQiZiZhi_2, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_3", playData.Write_ZuoQiZiZhi_3, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_4", playData.Write_ZuoQiZiZhi_4, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiLv_1", playData.Write_ZuoQi_NengLiLv_1, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiExp_1", playData.Write_ZuoQi_NengLiExp_1, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiLv_2", playData.Write_ZuoQi_NengLiLv_2, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiExp_2", playData.Write_ZuoQi_NengLiExp_2, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiLv_3", playData.Write_ZuoQi_NengLiLv_3, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiExp_3", playData.Write_ZuoQi_NengLiExp_3, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiLv_4", playData.Write_ZuoQi_NengLiLv_4, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiExp_4", playData.Write_ZuoQi_NengLiExp_4, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowZuoQiShowID", playData.Write_NowZuoQiShowID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiBaoShiDu", playData.Write_ZuoQiBaoShiDu, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiBaoStatus", playData.Write_ZuoQiBaoStatus, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiJieDuanLv", playData.Write_ZuoQiJieDuanLv, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiXianJiExp", playData.Write_ZuoQiXianJiExp, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureDuiHuanID", playData.Write_PastureDuiHuanID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

        Debug.Log("playData.Write_BagItemIDList.Length = " + playData.Write_BagItemIDList.Length);

		//存储背包
		for (int i = 0; i < playData.Write_BagItemIDList.Length; i++) {
			int sum = i + 1;
			Debug.Log("sum = " + sum);
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", playData.Write_BagItemIDList[i], "ID", sum.ToString(), "RoseBag");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", playData.Write_BagItemNumList[i], "ID", sum.ToString(), "RoseBag");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", playData.Write_BagItemHideList[i], "ID", sum.ToString(), "RoseBag");

			//--
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", playData.Write_BagItemParList[i], "ID", sum.ToString(), "RoseBag");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", playData.Write_BagGemHoleList[i], "ID", sum.ToString(), "RoseBag");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", playData.Write_BagGemIDList[i], "ID", sum.ToString(), "RoseBag");
			//--

		}
		//存储数据
		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");

		//存储仓库
		for (int i = 0; i < playData.Write_StoreHouseItemIDList.Length; i++)
		{
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", playData.Write_StoreHouseItemIDList[i], "ID", (i + 1).ToString(), "RoseStoreHouse");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", playData.Write_StoreHouseItemNumList[i], "ID", (i + 1).ToString(), "RoseStoreHouse");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", playData.Write_StoreHouseItemHideList[i], "ID", (i + 1).ToString(), "RoseStoreHouse");
		
			//--
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", playData.Write_StoreHouseItemParList[i], "ID", (i + 1).ToString(), "RoseStoreHouse");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", playData.Write_StoreHouseGemHoleList[i], "ID", (i + 1).ToString(), "RoseStoreHouse");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", playData.Write_StoreHouseGemIDList[i], "ID", (i + 1).ToString(), "RoseStoreHouse");
			//--
		
		}
		//存储数据
		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");

		//存储角色
		for (int i = 0; i < playData.Write_EquipItemIDList.Length; i++)
		{
			
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipItemID", playData.Write_EquipItemIDList[i], "ID", (i + 1).ToString(), "RoseEquip");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", playData.Write_EquipHideIDList[i], "ID", (i + 1).ToString(), "RoseEquip");

			//--
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", playData.Write_EquipGemHoleList[i], "ID", (i + 1).ToString(), "RoseEquip");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", playData.Write_EquipGemIDList[i], "ID", (i + 1).ToString(), "RoseEquip");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QiangHuaID", playData.Write_EquipQiangHuaIDList[i], "ID", (i + 1).ToString(), "RoseEquip");
			//--
		}
		//存储数据
		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");

		//清空隐藏属性表
		Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables["RoseEquipHideProperty"].Clear();
		Game_PublicClassVar.Get_function_DataSet.AddRoseEquipHidePropertyXml("1001", "3,1");

		//存储隐藏属性
		foreach (string hideID in playData.Write_AllHideIDList.Keys)
		{
			string addID = hideID;
			string addProperty = playData.Write_AllHideIDList[hideID];
			Game_PublicClassVar.Get_function_DataSet.AddRoseEquipHidePropertyXml(addID, addProperty);
			/*
            try
            {
                DataRow rows = Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables["RoseEquipHideProperty"].Rows.Find(hideID);
                rows["PrepeotyList"] = addProperty;
            }
            catch {

                //新建行数据
                DataTable dataTable = Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables["RoseEquipHideProperty"];
                DataRow row = dataTable.NewRow();
                //设置数据
                row["ID"] = addID;
                row["PrepeotyList"] = addProperty;
                Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables["RoseEquipHideProperty"].Rows.Add(row);
            }
            */
		}


		//存储宠物
		for (int i = 0; i < playData.Write_PetStatus_List.Length; i++) {

			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetStatus", playData.Write_PetStatus_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetID", playData.Write_PetID_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetLv", playData.Write_PetLv_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetNowHp", playData.Write_PetNowHp_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetMaxHp", playData.Write_PetMaxHp_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetExp", playData.Write_PetExp_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetName", playData.Write_PetName_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("IfBaby", playData.Write_IfBaby_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyNum", playData.Write_AddPropretyNum_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyValue", playData.Write_AddPropretyValue_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetPingFen", playData.Write_PetPingFen_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Hp", playData.Write_ZiZhi_Hp_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Act", playData.Write_ZiZhi_Act_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_MageAct", playData.Write_ZiZhi_MageAct_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Def", playData.Write_ZiZhi_Def_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Adf", playData.Write_ZiZhi_Adf_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_ActSpeed", playData.Write_ZiZhi_ActSpeed_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_ChengZhang", playData.Write_ZiZhi_ChengZhang_List[i], "ID", (i + 1).ToString(), "RosePet");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetSkill", playData.Write_PetSkill_List[i], "ID", (i + 1).ToString(), "RosePet");
            if (playData.Write_PetEquipID_1 != null)
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipID_1", playData.Write_PetEquipID_1[i], "ID", (i + 1).ToString(), "RosePet");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIDHide_1", playData.Write_PetEquipIDHide_1[i], "ID", (i + 1).ToString(), "RosePet");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipID_2", playData.Write_PetEquipID_2[i], "ID", (i + 1).ToString(), "RosePet");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIDHide_2", playData.Write_PetEquipIDHide_2[i], "ID", (i + 1).ToString(), "RosePet");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipID_3", playData.Write_PetEquipID_3[i], "ID", (i + 1).ToString(), "RosePet");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIDHide_3", playData.Write_PetEquipIDHide_3[i], "ID", (i + 1).ToString(), "RosePet");
                if (playData.Write_PetEquipID_4 != null)
                {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipID_4", playData.Write_PetEquipID_4[i], "ID", (i + 1).ToString(), "RosePet");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIDHide_4", playData.Write_PetEquipIDHide_4[i], "ID", (i + 1).ToString(), "RosePet");
                }
            }
        }

		//存储数据
		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

        //重新写入验证机制
        //Game_PublicClassVar.Get_wwwSet.WriteFileYanZheng();

        //存储登录界面显示
        try
        {
            //存储角色通用数据
            Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "Name", playData.Write_Name);
            Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "Lv", playData.Write_Lv);
        }
        catch
        {
            Debug.Log("角色数据错误！");
        }

        //关闭游戏
        Debug.Log("下载完成强制关闭游戏！");
        Application.Quit();

	}


	public void GamePay(ObscuredString rmbValue, ObscuredString rmbDingDan)
	{

        Debug.Log("支付信息打印: rmbValue:" + rmbValue + " rmbDingDan:" + rmbDingDan);

		//安卓调用
		#if UNITY_ANDROID
		if (rmbDingDan == "" || rmbDingDan == "0" || rmbDingDan == null)
		{
			Game_PublicClassVar.Get_function_UI.GameHint("支付错误,错误提示111！");
		}

		//获取订单号是否为空
		if (rmbDingDan != Game_PublicClassVar.Get_game_PositionVar.PayDingDanIDNow) {
			Game_PublicClassVar.Get_function_UI.GameHint("支付错误,错误提示222！");
			return;
		}

		//获取订单支付状态是否为2
		if (Game_PublicClassVar.Get_game_PositionVar.PayDingDanStatus==2)
		{
			Game_PublicClassVar.Get_function_UI.GameHint("支付错误,错误提示3333！");
			return;
		}
		#endif

		string payStr = "支付状态开启:" + Game_PublicClassVar.Get_game_PositionVar.PayStr;
		string payStatusType = Game_PublicClassVar.Get_game_PositionVar.PayStr.ToString().Split(';')[0];
		string payStatusStr = Game_PublicClassVar.Get_game_PositionVar.PayStr.ToString().Split(';')[1];
		int rmbToZuanShiValue = GameReturnPayValue(rmbValue);
		float rmbPayValue = float.Parse(rmbValue);

		//通知UI
		UI_RmbStore ui_rmbStore = null;
		if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>() != null)
		{
			ui_rmbStore = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>();
		}

		switch (payStatusType)
		{
		//支付状态
		case "0":
			payStr = "支付中……" + payStatusStr;
			break;

		//支付成功
		case "1":

			//删除充值记录
			Game_PublicClassVar.Get_function_Rose.DeletePayID(Game_PublicClassVar.Get_game_PositionVar.PayDingDanIDNow);
			payStr = rmbPayValue + "支付成功！" + payStatusStr;
			//累计当前充值额度,发送指定钻石奖励
			Game_PublicClassVar.Get_function_Rose.AddRMB(rmbToZuanShiValue);
			Game_PublicClassVar.Get_function_Rose.AddPayValue(rmbPayValue,"52");
			//Game_PublicClassVar.Get_function_UI.GameGirdHint("支付成功:" + rmbPayValue + "元");


			//清理支付值
			float youmengRmbValue = rmbPayValue;
            int nowRmbPay = (int)(rmbPayValue);
			rmbPayValue = 0;


			//发送友盟支付信息
			try
			{
				//GA.Pay(youmengRmbValue, GA.PaySource.Source10, rmbToZuanShiValue);
			}
			catch
			{
				Debug.Log("充值报错！");
			}

			rmbToZuanShiValue = 0;

            //更新通用界面显示
            Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet();


            //通知UI
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>() != null)
            {
                int zuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().Obj_HuoBi_Rmb.GetComponent<Text>().text = zuanShi.ToString();
                //ui_rmbStore.ClearnPayValue();       //清理支付值
            }

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_171");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你,支付成功!钻石已成功到账,请查收");

            //如果是未成年,记录当前的付费值
            if (Game_PublicClassVar.Get_wwwSet.IfWeiChengNianStatus) {

                int nowPay = PlayerPrefs.GetInt("FangChenMi_Pay");          //获取当前时间
                nowPay = nowPay + nowRmbPay;
                PlayerPrefs.SetInt("FangChenMi_Pay", nowPay);

            }

            break;

		//支付失败
		case "2":
			payStr = "支付失败！" + payStatusStr;
			rmbToZuanShiValue = 0;
			rmbPayValue = 0;

			#if UNITY_ANDROID

			//防止支付宝第一次调用错误
			int ZhiFuBaoOpenNum = PlayerPrefs.GetInt("ZhiFuBaoOpenNum");
			if (ZhiFuBaoOpenNum == 0)
			{
				PlayerPrefs.SetInt("ZhiFuBaoOpenNum", 1);
				if (Game_PublicClassVar.Get_game_PositionVar.PayValueNow != "")
				{
					//通知UI
					if (ui_rmbStore != null)
					{
						ui_rmbStore.Btn_BuyZuanShi(Game_PublicClassVar.Get_game_PositionVar.PayValueNow);
					}
				}
			}
			#endif
			break;

		//其他原因
		case "3":
			payStr = "支付未知原因！" + payStatusStr;
			//通知UI
			if (ui_rmbStore != null)
			{
				//Game_PublicClassVar.Get_game_PositionVar.PayStatus = false;
				//Game_PublicClassVar.Get_game_PositionVar.PayStr = "";
				//ui_rmbStore.ClearnPayValue();       //清理支付值
			}
			break;

		default:
			payStr = "支付default" + payStatusStr;
			//通知UI
			if (ui_rmbStore != null)
			{
				//Game_PublicClassVar.Get_game_PositionVar.PayStatus = false;
				//Game_PublicClassVar.Get_game_PositionVar.PayStr = "";
				//ui_rmbStore.ClearnPayValue();       //清理支付值
			}
			break;

			Debug.Log("payStr: " + payStr);
			Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(payStr);
		}
	}

	//根据支付额度返回应获取的额度
	private int GameReturnPayValue(string payValue)
	{
		int returnZuanShiValue = 0;
		switch (payValue)
		{
            /*
                case "0.03":
                      returnZuanShiValue = 6000;
                      Debug.Log("匹配到了");
                      break;


                  case "6":
                      returnZuanShiValue = 5000;
                      break;
                  */

            case "0":
            case "0.01":
                returnZuanShiValue = 600;
                Debug.Log("匹配到了");
                break;

            //新付费
            case "6":
                returnZuanShiValue = 600;
                break;
            case "30":
                returnZuanShiValue = 3300;
                break;
            case "50":
                returnZuanShiValue = 6000;
                break;
            case "98":
                returnZuanShiValue = 11000;
                break;
            case "198":
                returnZuanShiValue = 22688;
                break;
            case "298":
                returnZuanShiValue = 34688;
                break;
            case "488":
                returnZuanShiValue = 57688;
                break;
            case "648":
                returnZuanShiValue = 77688;
                break;
        }
        return returnZuanShiValue;
	}


    //增加洗炼大师经验
    public void AddXiLianDaShiExp(string addType,int addValue) {

        if (addValue == 0) {
            return;
        }

        string hintTitleStr = "";
        string xilianID = "10001";
        switch (addType) {
            case "1":
                hintTitleStr = "英勇洗炼";
                xilianID = "10001";
                break;
            case "2":
                hintTitleStr = "海神洗炼";
                xilianID = "20001";
                break;
            case "3":
                hintTitleStr = "漠灵洗炼";
                xilianID = "30001";
                break;
            case "4":
                hintTitleStr = "光辉洗炼";
                xilianID = "40001";
                break;
            case "5":
                hintTitleStr = "圣光洗炼";
                xilianID = "50001";
                break;
        }

        hintTitleStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(hintTitleStr);

        string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_174");
        string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_175");
        string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_176");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + addValue + langStrHint_2 + hintTitleStr + langStrHint_3);

        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("增加" + addValue + "点" + hintTitleStr + "熟练度!");

        string xiLianDaShiIDSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianDaShiIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (xiLianDaShiIDSetStr == "" || xiLianDaShiIDSetStr == "0")
        {
            xiLianDaShiIDSetStr = "0;0;0;0;0";
        }

        string[] xiLianDaShiIDSetList = xiLianDaShiIDSetStr.Split(';');

        string xiLianDaShiShuLianStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianDaShiShuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (xiLianDaShiShuLianStr == ""|| xiLianDaShiShuLianStr=="0") {
            xiLianDaShiShuLianStr = "0;0;0;0;0";
        }

        string[] xilianDaShiShuLianList = xiLianDaShiShuLianStr.Split(';');

        bool ifWriteXiLianDaShiID = false;

        if (xiLianDaShiShuLianStr.Length >= 5) {

            int num = int.Parse(addType);
            num = num - 1;
            if (num >= 0) {
                int nowValue = int.Parse(xilianDaShiShuLianList[num]);
                nowValue = nowValue + addValue;
                xilianDaShiShuLianList[num] = nowValue.ToString();

                //获取当前洗炼大师ID
                string nextXiLianDaShiID = "";
                if (xiLianDaShiIDSetList[num] != "" && xiLianDaShiIDSetList[num] != "0")
                {
                    int xulie = xiLianDaShiIDSetList[num].Split(',').Length - 1;
                    xilianID = xiLianDaShiIDSetList[num].Split(',')[xulie];
                    nextXiLianDaShiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", xilianID, "EquipXiLianDaShi_Template");
                }
                else {
                    nextXiLianDaShiID = xilianID;
                }
                
                if(nextXiLianDaShiID!=""&& nextXiLianDaShiID != "0")
                {
                    string needXiLianValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedXiLianValue", "ID", nextXiLianDaShiID, "EquipXiLianDaShi_Template");
                    if (nowValue >= int.Parse(needXiLianValue)) {

                        string langStrHint_11 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_178");
                        string langStrHint_22 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_179");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + hintTitleStr + langStrHint_2);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你!激活了新的"+ hintTitleStr + "试炼等级！");

                        if (xiLianDaShiIDSetList[num] == "0" || xiLianDaShiIDSetList[num] == "")
                        {
                            xiLianDaShiIDSetList[num] = xilianID;
                        }
                        else {
                            //写入值
                            xiLianDaShiIDSetList[num] = xiLianDaShiIDSetList[num] + "," + nextXiLianDaShiID;
                        }

                        ifWriteXiLianDaShiID = true;
                    }
                    else
                    {

                    }
                }
            }
        }

        string writeStr = "";
        for (int i = 0; i < xilianDaShiShuLianList.Length; i++) {
            writeStr = writeStr + xilianDaShiShuLianList[i] + ";";
        }
        if (writeStr != "") {
            writeStr = writeStr.Substring(0, writeStr.Length - 1);
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XiLianDaShiShuLian", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        }

        if (ifWriteXiLianDaShiID) {
            string writeIDStr = "";
            for (int i = 0; i < xiLianDaShiIDSetList.Length; i++)
            {
                writeIDStr = writeIDStr + xiLianDaShiIDSetList[i] + ";";
            }
            if (writeIDStr != "")
            {
                writeIDStr = writeIDStr.Substring(0, writeIDStr.Length - 1);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XiLianDaShiIDSet", writeIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            }
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
    }


    //添加进入地图,跟遇到宝宝次数有关系
    public void Rose_AddEnterMapNum(int addValue) {
        
        string enterMapBaby = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EnterMapBaby", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (enterMapBaby == "") {
            enterMapBaby = "0";
        }
        int writeValue = int.Parse(enterMapBaby) + addValue;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EnterMapBaby", writeValue.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
    }


    //计算世界等级地图差值
    public void GetWorldLvExpPro() {

        //计算等级差值
        int nowRoseLv = GetRoseLv();
        int chaLv = Game_PublicClassVar.Get_wwwSet.WorldPlayerLv - nowRoseLv;
        if (chaLv >= 1)
        {
            //经验加成,没级增加0.05f
            float nowPro = chaLv * 0.05f;
            if (nowPro >= 0.5f) {
                nowPro = 0.5f;
            }
            Game_PublicClassVar.Get_wwwSet.WorldExpProAdd = nowPro;
        }
        else {
            Game_PublicClassVar.Get_wwwSet.WorldExpProAdd = 0;
        }

        //等级超过50不再享受世界等级经验加成
        if (nowRoseLv >= 50) {
            Game_PublicClassVar.Get_wwwSet.WorldExpProAdd = 0;
        }

    }


    //发送封号
    public void SendFengHao(string hint) {

        Pro_ComStr_3 comstr_3 = new Pro_ComStr_3();
        comstr_3.str_1 = "999";
        comstr_3.str_2 = hint;
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001201, comstr_3);

    }

    //获取爆率倍率
    public ObscuredFloat GetBaoLvProValue(int BaoLvType)
    {
        ObscuredFloat proValue = 1;
        switch (BaoLvType)
        {
            case 1:
                proValue = 1;
                break;
            case 2:
                proValue = 1.1f;
                break;
            case 3:
                proValue = 1.2f;
                break;
            case 4:
                proValue = 1.3f;
                break;
            case 5:
                proValue = 1.5f;
                break;
            case 6:
                proValue = 2f;
                break;
            case 7:
                proValue = 2.5f;
                break;
            case 8:
                proValue = 3f;
                break;
        }

        return proValue;

    }

    //获取名称
    public string GetBaoLvName(int BaoLvType)
    {
        string name = "";
        switch (BaoLvType)
        {

            case 1:
                name = "普普通通";
                break;
            case 2:
                name = "小赚一笔";
                break;
            case 3:
                name = "幸运玩家";
                break;
            case 4:
                name = "百里挑一";
                break;
            case 5:
                name = "千载难逢";
                break;
            case 6:
                name = "万众瞩目";
                break;
            case 7:
                name = "绝世耀眼";
                break;
            case 8:
                name = "谁与争锋";
                break;

        }

        name = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(name);

        return name;

    }

    public void AddChouKaNum(int addNum) {

        string dayChouKaNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ChouKaNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (dayChouKaNum == "")
        {
            dayChouKaNum = "0";
        }

        dayChouKaNum = (int.Parse(dayChouKaNum) + addNum).ToString();
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ChouKaNum", dayChouKaNum, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

    }

    //存储离线储备经验和点数
    public void AddOffLinkReward(int lixianValue) {

        ObscuredInt addValue = (int)(lixianValue / 60);

        //最多增加8小时的储备
        if (addValue > 480) {
            addValue = 480;
        }

        if (addValue >= 1) {
            int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
            string OffLinkExpStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffLinkExp", "ID", roseLv.ToString(), "RoseExp_Template");

            float addfolat = (float)(addValue) / 1440.0f;
            int addExp = (int)(float.Parse(OffLinkExpStr) * addfolat);

            int chubeiExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChuBeiExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            chubeiExp = chubeiExp + addExp;
            if (chubeiExp > int.Parse(OffLinkExpStr)) {
                chubeiExp = int.Parse(OffLinkExpStr);
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChuBeiExp", chubeiExp.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            int ChuBeiNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChuBeiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            ChuBeiNum = ChuBeiNum + addValue;
            if (ChuBeiNum >= 10000) {
                ChuBeiNum = 10000;
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChuBeiNum", ChuBeiNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            //弹出界面提示 (超过30分钟才会提示)
            if (addValue >= 30)
            {
                UI_FunctionOpen f_open = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>();
                GameObject xilianObj = f_open.FunctionInstantiate(f_open.Obj_LiXianShouYi, "Obj_LiXianShouYi");
                if (xilianObj != null)
                {
                    xilianObj.transform.SetParent(f_open.UISet);
                    xilianObj.transform.localScale = new Vector3(1, 1, 1);
                    xilianObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                    xilianObj.GetComponent<UI_LiXianShouYi>().ChuBeiLiXianNum = addValue;
                    xilianObj.GetComponent<UI_LiXianShouYi>().ChuBeiTime = addValue;
                    xilianObj.GetComponent<UI_LiXianShouYi>().ChuBeiExpNum = addExp;
                    xilianObj.GetComponent<UI_LiXianShouYi>().Init();
                }
            }
        }
    }
    //发送货币记录
    public void SendGameHuoBiJiLu() {

        Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();

        comStr_4.str_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GetGoldSum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        comStr_4.str_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GetZuanShiSum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        comStr_4.str_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostGoldSum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        comStr_4.str_4 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostZuanShiSum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001701, comStr_4);
    }
    //发送记录1
    public void SendGameJiLu_1()
    {

        Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();

        comStr_4.str_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("KillBossTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001702, comStr_4);
    }

    //增加魔法值
    public void RoseLanAdd(int addValue) {

        int nowLan = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_LanValue;
        nowLan = nowLan + addValue;
        if (nowLan>= Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_LanValueMax) {
            nowLan = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_LanValueMax;
        }
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_LanValue = nowLan;
    }

    //减去魔法值
    public void RoseLanCost(int costValue)
    {

        int nowLan = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_LanValue;
        nowLan = nowLan - costValue;
        if (nowLan <= 0)
        {
            nowLan = 0;
        }
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_LanValue = nowLan;
    }

    //返回当前职业   1：战士  2:法师
    public string GetRoseOcc() {

        string occ = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        return occ;
    }

    //是否开启觉醒状态
    public void Rose_JueXingStatus() {

        string jihuoIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (jihuoIDStr.Contains("10001"))
        {
            Game_PublicClassVar.Get_game_PositionVar.JueXingStatus = true;
        }

    }

    //发送连续奖励
    public void Rose_SendRewardStr(string LingQuRewardStr) {

        //创建
        string[] rewardStrList = LingQuRewardStr.Split(';');
        for (int i = 0; i < rewardStrList.Length; i++)
        {
            string[] itemList = rewardStrList[i].Split(',');
            if (itemList.Length >= 2)
            {
                SendRewardToBag(itemList[0], int.Parse(itemList[1]));
            }
        }

    }

    //增加狩猎次数
    public void RoseAddShouLieNum(int addNum) {

        if (Game_PublicClassVar.gameLinkServer.HuoDong_ShouLie_Status == false) {
            return;
        }

        //显示其他信息
        string KillNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuoDong_ShouLie_KillNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (KillNum == "" || KillNum == null)
        {
            KillNum = "0";
        }

        int writeNum = int.Parse(KillNum) + addNum;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HuoDong_ShouLie_KillNum", writeNum.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

    }

    //返回是否已经觉醒
    public bool ReturnJueXingStatus() {

        //获取当前觉醒ID是否已经激活
        string jihuoIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (jihuoIDStr.Contains("10008"))
        {
            return true;
        }
        else {
            return false;
        }

    }


    //返回是否已经完成觉醒任务
    public bool ReturnJueXingTaskStatus()
    {

        //获取当前觉醒ID是否已经激活
        string jihuoIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (jihuoIDStr.Contains("10001"))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void RoseModelChangeValue() {

        string nowYanSeID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowYanSeID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (nowYanSeID == "" || nowYanSeID == "0" || nowYanSeID == null) {
            RoseModelChange("1", "", false, "1");
        }

        string nowNowYanSeHairID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowYanSeHairID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (nowNowYanSeHairID == "" || nowNowYanSeHairID == "0" || nowNowYanSeHairID == null)
        {
            RoseModelChange("1", "", false, "2");
        }

        string nowYanSe = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeID", "ID", nowYanSeID, "RanSe_Template");
        RoseModelChange(nowYanSe, nowYanSeID,false,"1");

        string nowYanSeHair = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeID", "ID", nowNowYanSeHairID, "RanSe_Template");
        RoseModelChange(nowYanSeHair, nowYanSeID,false,"2");

        //获取特效
        string nowEffectID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectID", "ID", nowYanSeID, "RanSe_Template");
        if (nowEffectID != "" && nowEffectID != "0" && nowEffectID != null)
        {
            GameObject effect = (GameObject)Resources.Load("Effect/Skill/" + nowEffectID, typeof(GameObject));
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().RoseModelEffect = (GameObject)MonoBehaviour.Instantiate(effect);
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().RoseModelEffect.transform.localScale = new Vector3(1, 1, 1);
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().RoseModelEffect.GetComponent<SkillEffectPosition>().TargetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Low;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().RoseModelEffect.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.gameObject.transform);
        }
        else {
            MonoBehaviour.DestroyObject(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().RoseModelEffect);
        }
    }

    //改变自身衣服颜色(1:表示默认  2：标书觉醒  黄色)(Position = 0 表示全部改变 1表示身体 2表示头部)
    public void RoseModelChange(string showType,string nowYanSeID = "", bool ifShowModelUI = false,string Position="0",GameObject playerObj = null,string playerOcc = "1") {

        //获取职业
        string occ = GetRoseOcc();

        if (playerObj != null) {
            if (playerObj.GetComponent<Player_Status>() != null)
            {
                occ = playerObj.GetComponent<Player_Status>().OccType;
            }
            else {
                occ = playerOcc;
            }
        }

        switch (occ) {

            //战士相关
            case "1":
                string waiguanName = "PiFu_" + showType;
                Texture2D img = (Texture2D)Resources.Load("3DModel/RoseModel/Occ_ZhanShi/PiFu/" + waiguanName);
                if (img == null)
                {
                    return;
                }

                //头发
                string waiguanHairName = "PiFuHair_" + showType;
                Texture2D HairImg = (Texture2D)Resources.Load("3DModel/RoseModel/Occ_ZhanShi/PiFu/" + waiguanHairName);
                if (HairImg == null)
                {
                    return;
                }

                if (ifShowModelUI == false)
                {
                    if (Position == "0" || Position == "1") {

                        if (playerObj == null)
                        {
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().ZhanShi_Skin[0].material.mainTexture = img;
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().ZhanShi_Skin[1].material.mainTexture = img;
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().ZhanShi_Skin[5].material.mainTexture = img;
                        }
                        else {
                            playerObj.GetComponent<Player_Bone>().ZhanShi_Skin[0].material.mainTexture = img;
                            playerObj.GetComponent<Player_Bone>().ZhanShi_Skin[1].material.mainTexture = img;
                            playerObj.GetComponent<Player_Bone>().ZhanShi_Skin[5].material.mainTexture = img;
                        }
                    }

                    if (Position == "0" || Position == "2")
                    {
                        if (playerObj == null)
                        {
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().ZhanShi_Skin[3].material.mainTexture = HairImg;
                        }
                        else {
                            playerObj.GetComponent<Player_Bone>().ZhanShi_Skin[3].material.mainTexture = HairImg;
                        }
                    }
                }

                if (Position == "0" || Position == "1")
                {
                    if (playerObj == null)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelZhanShi_Skin[0].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelZhanShi_Skin[1].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelZhanShi_Skin[5].material.mainTexture = img;
                    }
                    else {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelZhanShi_Skin[0].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelZhanShi_Skin[1].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelZhanShi_Skin[5].material.mainTexture = img;
                    }
                }
                if (Position == "0" || Position == "2")
                {
                    if (playerObj == null)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelZhanShi_Skin[3].material.mainTexture = HairImg;
                    }
                    else
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelZhanShi_Skin[3].material.mainTexture = HairImg;
                    }
                }
                break;

            //法师相关
            case "2":
                waiguanName = "PiFu_" + showType;
                img = (Texture2D)Resources.Load("3DModel/RoseModel/Occ_MoFaShi/PiFu/" + waiguanName);
                if (img == null)
                {
                    return;
                }

                //头发变色
                string colorStr = "#FFFFFF";

                switch (showType)
                {
                    case "2":
                        colorStr = "#02C0FF";
                        break;

                    case "3":
                        colorStr = "#16D206";
                        break;

                    case "101":
                        colorStr = "#FFE302";
                        break;

                    case "102":
                        colorStr = "#85E1FF";
                        break;
                        
                }

                Color nowColor;
                ColorUtility.TryParseHtmlString(colorStr, out nowColor);


                if (ifShowModelUI == false)
                {
                    if (Position == "0" || Position == "1")
                    {
                        if (playerObj == null)
                        {
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().FaShi_Skin[0].material.mainTexture = img;
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().FaShi_Skin[1].material.mainTexture = img;
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().FaShi_Skin[5].material.mainTexture = img;
                        }
                        else {
                            playerObj.GetComponent<Player_Bone>().FaShi_Skin[0].material.mainTexture = img;
                            playerObj.GetComponent<Player_Bone>().FaShi_Skin[1].material.mainTexture = img;
                            playerObj.GetComponent<Player_Bone>().FaShi_Skin[5].material.mainTexture = img;
                        }
                    }
                    if (Position == "0" || Position == "2") {
                        if (playerObj == null)
                        {
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().FaShi_Skin[3].material.color = nowColor;
                        }
                        else {
                            playerObj.GetComponent<Player_Bone>().FaShi_Skin[3].material.color = nowColor;
                        }  
                    }
                }

                if (Position == "0" || Position == "1")
                {
                    if (playerObj == null)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelFaShi_Skin[0].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelFaShi_Skin[1].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelFaShi_Skin[5].material.mainTexture = img;
                    }
                    else {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelFaShi_Skin[0].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelFaShi_Skin[1].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelFaShi_Skin[5].material.mainTexture = img;
                    }
                }
                if (Position == "0" || Position == "2") {
                    if (playerObj == null)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelFaShi_Skin[3].material.color = nowColor;
                    }
                    else {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelFaShi_Skin[3].material.color = nowColor;
                    }
                }
                break;


            //法师相关
            case "3":
                waiguanName = "PiFu_" + showType;

                img = (Texture2D)Resources.Load("3DModel/RoseModel/Occ_LieRen/PiFu/" + waiguanName);
                if (img == null)
                {
                    return;
                }

                //头发变色
                colorStr = "#FFFFFF";

                switch (showType)
                {
                    case "2":
                        colorStr = "#02C0FF";
                        break;

                    case "3":
                        colorStr = "#16D206";
                        break;

                    case "101":
                        colorStr = "#FFE302";
                        break;
                }

                ColorUtility.TryParseHtmlString(colorStr, out nowColor);


                if (ifShowModelUI == false)
                {
                    if (Position == "0" || Position == "1")
                    {
                        if (playerObj == null)
                        {
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().LieRen_Skin[0].material.mainTexture = img;
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().LieRen_Skin[1].material.mainTexture = img;
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().LieRen_Skin[5].material.mainTexture = img;
                        }
                        else
                        {
                            playerObj.GetComponent<Player_Bone>().LieRen_Skin[0].material.mainTexture = img;
                            playerObj.GetComponent<Player_Bone>().LieRen_Skin[1].material.mainTexture = img;
                            playerObj.GetComponent<Player_Bone>().LieRen_Skin[5].material.mainTexture = img;
                        }
                    }
                    if (Position == "0" || Position == "2")
                    {
                        if (playerObj == null)
                        {
                            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().LieRen_Skin[3].material.color = nowColor;
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().LieRen_Skin[6].material.mainTexture = img;
                        }
                        else
                        {
                            playerObj.GetComponent<Player_Bone>().LieRen_Skin[3].material.color = nowColor;
                        }
                    }
                }

                if (Position == "0" || Position == "1")
                {
                    if (playerObj == null)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelLieRen_Skin[0].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelLieRen_Skin[1].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelLieRen_Skin[5].material.mainTexture = img;
                    }
                    else
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelLieRen_Skin[0].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelLieRen_Skin[1].material.mainTexture = img;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelLieRen_Skin[5].material.mainTexture = img;
                    }
                }
                if (Position == "0" || Position == "2")
                {
                    if (playerObj == null)
                    {
                        //Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelLieRen_Skin[3].material.color = nowColor;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ModelLieRen_Skin[6].material.mainTexture = img;
                    }
                    else
                    {
                        //Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelLieRen_Skin[3].material.color = nowColor;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PlayerModelLieRen_Skin[6].material.mainTexture = img;
                    }
                }
                break;

        }

        //注意 如果是特效不显示可能是层级的问题，把特效的层级改变为RoseEquip
        //获取特效
        string nowEffectID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectID", "ID", nowYanSeID, "RanSe_Template");
        if (nowEffectID != "" && nowEffectID != "0" && nowEffectID != null)
        {
            GameObject effect = (GameObject)Resources.Load("Effect/Skill/" + nowEffectID, typeof(GameObject));
            Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_EquipModel_Posi_ModelEffect = (GameObject)MonoBehaviour.Instantiate(effect);
            Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_EquipModel_Posi_ModelEffect.transform.localScale = new Vector3(1, 1, 1);
            Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_EquipModel_Posi_ModelEffect.GetComponent<SkillEffectPosition>().TargetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_EquipModel_Posi_Low;
            Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_EquipModel_Posi_ModelEffect.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.gameObject.transform);
        }
        else {
            MonoBehaviour.Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_EquipModel_Posi_ModelEffect);
        }
    }

    //激活颜色id
    public void RoseYanSeAdd(string addID) {

        string roseYanSeIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (roseYanSeIDSet.Contains(addID))
        {
            return;
        }
        string writeValue = "";
        if (roseYanSeIDSet == "" || roseYanSeIDSet == null)
        {
            writeValue = addID;
        }
        else {
            writeValue = roseYanSeIDSet + ";" + addID;
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanSeIDSet", writeValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //写入成就
        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("226", "0", "1");
    }


    //穿戴十二生肖装备
    public void RoseEquip_ShengXiao_Wear(string bagSpaceNum) {

        string EquipIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShengXiaoSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (EquipIDStr == "" || EquipIDStr == "0") {
            EquipIDStr = ";;;;;;;;;;;";
        }

        //背包内的十二生肖
        string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");

        string equipType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", bagItemID, "Item_Template");
        if (equipType != "3") {
            return;
        }

        if (bagItemID != "" && bagItemID != "0" && bagItemID != null) {

            string equipSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", bagItemID, "Item_Template");
            int posi = int.Parse(equipSubType) - 100 - 1;

            //扣掉装备
            Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(bagItemID, 1, bagSpaceNum);

            //获取当前身上装备的十二生肖
            string[] EquipIDList = EquipIDStr.Split(';');
            string EquipID = EquipIDList[posi];
            if (EquipID == "0" || EquipID == "")
            {
                //直接装备
                EquipIDList[posi] = bagItemID;

            }
            else {
                //发送装备
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(EquipIDList[posi],1);
                //交换装备
                EquipIDList[posi] = bagItemID;
            }

            //写入生肖
            string writeStr = "";
            int listNum = EquipIDList.Length;
            if (listNum > 12) {
                listNum = 12;
            }

            for (int i = 0; i < listNum; i++) {
                writeStr = writeStr + EquipIDList[i] + ";";
            }

            if (EquipIDList.Length < 12) {
                int cha = 12 - EquipIDList.Length;
                for (int i = 0; i < cha; i++)
                {
                    writeStr = writeStr + ";";
                }
            }

            writeStr = writeStr.Substring(0, writeStr.Length - 1);

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShengXiaoSet", writeStr,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty(true);
        }
    }


    //穿戴十二生肖装备
    public void RoseEquip_ShengXiao_Take(string ShengXiaoSpaceNum)
    {

        string EquipIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShengXiaoSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (EquipIDStr == "" || EquipIDStr == "0")
        {
            EquipIDStr = ";;;;;;;;;;;";
        }


        //获取当前身上装备的十二生肖
        string[] EquipIDList = EquipIDStr.Split(';');
        int posi = int.Parse(ShengXiaoSpaceNum) - 1;
        string EquipID = EquipIDList[posi];
        if (EquipID == "0" || EquipID == "")
        {
            //为空不需要脱装备
        }
        else
        {

            //脱下装备
            string sendItemID = EquipIDList[posi];
            EquipIDList[posi] = "";

            //写入生肖
            string writeStr = "";
            int listNum = EquipIDList.Length;
            if (listNum > 12)
            {
                listNum = 12;
            }

            for (int i = 0; i < listNum; i++)
            {
                writeStr = writeStr + EquipIDList[i] + ";";
            }

            if (EquipIDList.Length < 12)
            {
                int cha = 12 - EquipIDList.Length;
                for (int i = 0; i < cha; i++)
                {
                    writeStr = writeStr + ";";
                }
            }

            writeStr = writeStr.Substring(0, writeStr.Length - 1);

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShengXiaoSet", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            //发送装备
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(sendItemID, 1);

            Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty(true);
        }

    }

    //获取指定位置12生肖的装备
    public string RoseEquip_ReturnShengXiaoID(int spaceID) {

        string EquipIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShengXiaoSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (EquipIDStr == "" || EquipIDStr == "0")
        {
            EquipIDStr = ";;;;;;;;;;;";
        }

        //获取当前身上装备的十二生肖
        string[] EquipIDList = EquipIDStr.Split(';');

        return EquipIDList[spaceID];
    }

    //穿戴宠物装备
    public void PetEquip_Wear(string bagSpaceNum, string nowPetID,string petEquipSpace) {

        string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
        if (bagItemID != "" && bagItemID != "0" && bagItemID != null)
        {
            string equipSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", bagItemID, "Item_Template");
            if (equipSubType == "201" || equipSubType == "202" || equipSubType == "203" || equipSubType == "204") {

                //判断部位是否正确
                bool wearStatus = false;
                if (petEquipSpace == "1" && equipSubType == "201") {
                    wearStatus = true;
                }

                if (petEquipSpace == "2" && equipSubType == "202")
                {
                    wearStatus = true;
                }

                if (petEquipSpace == "3" && equipSubType == "203")
                {
                    wearStatus = true;
                }

                if (petEquipSpace == "4" && equipSubType == "204")
                {
                    wearStatus = true;
                }

                if (wearStatus == false) {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("装备部位不符!");
                    return;
                }

                //交换装备
                string nowEquipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_" + petEquipSpace, "ID", nowPetID, "RosePet");
                string nowEquipIDHide = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipIDHide_" + petEquipSpace, "ID", nowPetID, "RosePet");

                string bagHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagSpaceNum, "RoseBag");

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipID_" + petEquipSpace, bagItemID, "ID", nowPetID, "RosePet");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIDHide_" + petEquipSpace, bagHideID, "ID", nowPetID, "RosePet");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

                if (nowEquipID == "" || nowEquipID == "0" || nowEquipID == null)
                {
                    //背包穿戴到身上
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", bagSpaceNum, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", bagSpaceNum, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "", "ID", bagSpaceNum, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", bagSpaceNum, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", bagSpaceNum, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID ", "0", "ID", bagSpaceNum, "RoseBag");
                }
                else {
                    //交换
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", nowEquipID, "ID", bagSpaceNum, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "1", "ID", bagSpaceNum, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "", "ID", bagSpaceNum, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", nowEquipIDHide, "ID", bagSpaceNum, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", bagSpaceNum, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID ", "0", "ID", bagSpaceNum, "RoseBag");
                }

                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");

                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().UpdatePetEquipShowStatus = true;
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().UpdateShowStatus = true;
            }
        }

    }

    //脱下宠物装备
    public void PetEquip_Take(string nowPetID, string petEquipSpace)
    {

        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() <= 0) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包已满,请先清理背包!");
            return;
        }

        string nowEquipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_" + petEquipSpace, "ID", nowPetID, "RosePet");
        string nowEquipIDHide = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipIDHide_" + petEquipSpace, "ID", nowPetID, "RosePet");

        if (nowEquipID != "" && nowEquipID != "0" && nowEquipID != null)
        {

            //获取一个空位
            if (nowEquipIDHide == "0") {
                nowEquipIDHide = "";
            }

            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(nowEquipID, 1, "1", 0, nowEquipIDHide, false);

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipID_" + petEquipSpace, "", "ID", nowPetID, "RosePet");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIDHide_" + petEquipSpace, "", "ID", nowPetID, "RosePet");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().UpdatePetEquipShowStatus = true;
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().UpdateShowStatus = true;
        }

    }

    //返回装备是否为装备类型
    public string ReturnEquipSubType(string itemID) {

        string itemType = Game_PublicClassVar.function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
        
        if (itemType == "3")
        {
            string itemSubType = Game_PublicClassVar.function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");

            //清理生肖显示
            if (int.Parse(itemSubType) >= 101 && int.Parse(itemSubType) <= 112)
            {
                return "2";
            }
            //清理宠物装备显示
            if (int.Parse(itemSubType) >= 201 && int.Parse(itemSubType) <= 204)
            {
                return "3";
            }

            return "1";     //其余默认装备
        }

        return "-1";
    }


    //根据当前洗炼家点数返回等级
    public string RetuenXiLianJiaLvSet() {


        int NowXiLianExp = 0;
        string XiLianDaShiShuLianStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianDaShiShuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (XiLianDaShiShuLianStr != "" && XiLianDaShiShuLianStr != "") {
            string[] xiLianNumList = XiLianDaShiShuLianStr.Split(';');
            for (int i = 0; i < xiLianNumList.Length;i++) {
                if (xiLianNumList[i] != "" && xiLianNumList[i] != null) {
                    NowXiLianExp = NowXiLianExp + int.Parse(xiLianNumList[i]);
                }
                if (i >= 5) {
                    break;
                }
            }
        }

        string returnStr = "";
        string intID = "90001";
        bool doStatus = false;
        do {
            string nowNeedNum = Game_PublicClassVar.function_DataSet.DataSet_ReadData("NeedXiLianValue", "ID", intID, "EquipXiLianDaShi_Template");
            if (nowNeedNum != ""&& nowNeedNum != null) {
                if (NowXiLianExp >= int.Parse(nowNeedNum))
                {
                    /*
                    string nowProStr = Game_PublicClassVar.function_DataSet.DataSet_ReadData("XiLianPerproty", "ID", intID, "EquipXiLianDaShi_Template");
                    if (nowProStr != "" && nowProStr != null)
                    {
                        returnStr = returnStr + nowProStr + ";";
                    }
                    */

                    returnStr = returnStr + intID + ";";

                }
                else {
                    doStatus = true;
                }
            }

            string NextID = Game_PublicClassVar.function_DataSet.DataSet_ReadData("NextID", "ID", intID, "EquipXiLianDaShi_Template");
            if (NextID != "" && NextID != "0" && NextID != null)
            {
                intID = NextID;
            }
            else {
                doStatus = true;
            }

        } while (doStatus == false);

        if (returnStr != ""&& returnStr != "0"&& returnStr != null) {
            returnStr = returnStr.Substring(0, returnStr.Length - 1);
        }

        return returnStr;

    }


    //根据当前洗炼家点数返回等级
    public string RetuenXiLianJiaLv()
    {

        int NowXiLianExp = 0;
        string XiLianDaShiShuLianStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianDaShiShuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (XiLianDaShiShuLianStr != "" && XiLianDaShiShuLianStr != "")
        {
            string[] xiLianNumList = XiLianDaShiShuLianStr.Split(';');
            for (int i = 0; i < xiLianNumList.Length; i++)
            {
                if (xiLianNumList[i] != "" && xiLianNumList[i] != null)
                {
                    NowXiLianExp = NowXiLianExp + int.Parse(xiLianNumList[i]);
                }
                if (i >= 5)
                {
                    break;
                }
            }
        }

        string intID = "90001";
        bool doStatus = false;
        do
        {
            string nowNeedNum = Game_PublicClassVar.function_DataSet.DataSet_ReadData("NeedXiLianValue", "ID", intID, "EquipXiLianDaShi_Template");
            if (nowNeedNum != "" && nowNeedNum != null)
            {
                if (NowXiLianExp >= int.Parse(nowNeedNum))
                {

                }
                else
                {
                    doStatus = true;
                }
            }

            string NextID = Game_PublicClassVar.function_DataSet.DataSet_ReadData("NextID", "ID", intID, "EquipXiLianDaShi_Template");
            if (NextID != "" && NextID != "0" && NextID != null)
            {
                intID = NextID;
            }
            else
            {
                doStatus = true;
            }

        } while (doStatus == false);


        return intID;

    }

    public int ReturnXiLianNum() {

        int NowXiLianExp = 0;
        string XiLianDaShiShuLianStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianDaShiShuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (XiLianDaShiShuLianStr != "" && XiLianDaShiShuLianStr != "")
        {
            string[] xiLianNumList = XiLianDaShiShuLianStr.Split(';');
            for (int i = 0; i < xiLianNumList.Length; i++)
            {
                if (xiLianNumList[i] != "" && xiLianNumList[i] != null)
                {
                    NowXiLianExp = NowXiLianExp + int.Parse(xiLianNumList[i]);
                }
                if (i >= 5)
                {
                    break;
                }
            }
        }

        return NowXiLianExp;

    }

    //获取黑市商店刷新次数上限
    public int ReturnHeiShiUpdateNum() {

        string nowZhiChiZuoZheID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhiChiZuoZheID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (nowZhiChiZuoZheID != null && nowZhiChiZuoZheID != "") {
            int numMax = int.Parse(nowZhiChiZuoZheID) - 10000;
            return numMax;
        }

        return 0;

    }

    public void FangChenMiYear() 
    {


        int nowYear = PlayerPrefs.GetInt("FangChenMi_Year");

        if (nowYear < 18)
        {

            //获取身份证年龄
            string roseShenFenID = PlayerPrefs.GetString("FangChenMi_ID");

            if (roseShenFenID == "" || roseShenFenID == null || roseShenFenID == "0")
            {
                return;
            }

            int year = int.Parse(roseShenFenID.Substring(6, 4));
            int month = int.Parse(roseShenFenID.Substring(10, 2));
            int day = int.Parse(roseShenFenID.Substring(12, 2));
            //身份证为15的开启验证
            if (roseShenFenID.Length == 15)
            {
                year = 18;
            }

            System.DateTime t1 = new System.DateTime(year, month, day);
            System.DateTime t2 = System.DateTime.Now;

            if (Game_PublicClassVar.Get_wwwSet.DataTime != null)
            {
                t2 = Game_PublicClassVar.Get_wwwSet.DataTime;
            }

            System.TimeSpan chaSpan = t2 - t1;
            int chayear = (int)(chaSpan.Days / 365);
            Debug.Log("当前年龄:" + chayear);
            PlayerPrefs.SetInt("FangChenMi_Year", chayear);
        }
    }

    //获取系数
    public int GetLvShiLiAddPro() {

        int roseLv = GetRoseLv();

        int returnInt = 0;
        if (roseLv >= 1)
        {
            returnInt = 30;
        }

        if (roseLv >= 5)
        {
            returnInt = 40;
        }

        if (roseLv >= 10)
        {
            returnInt = 50;
        }

        if (roseLv >= 15)
        {
            returnInt = 60;
        }

        if (roseLv >= 20)
        {
            returnInt = 70;
        }

        if (roseLv >= 25)
        {
            returnInt = 80;
        }

        if (roseLv >= 30)
        {
            returnInt = 90;
        }

        if (roseLv >= 35)
        {
            returnInt = 110;
        }

        if (roseLv >= 40)
        {
            returnInt = 120;
        }

        if (roseLv >= 45)
        {
            returnInt = 135;
        }

        if (roseLv >= 50)
        {
            returnInt = 150;
        }

        return returnInt;

    }


    //添加注灵信息
    public void AddRoseZhuLing(string ZhuLingID) {

        string ZhuLingIDSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhuLingIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (ZhuLingIDSetStr.Contains(ZhuLingID) == false) {

            //添加注灵信息
            string addWriteStr = "";
            if (ZhuLingIDSetStr == "" || ZhuLingIDSetStr == null)
            {
                addWriteStr = ZhuLingID;
            }
            else {
                addWriteStr = ZhuLingIDSetStr + ";" + ZhuLingID;
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhuLingIDSet", addWriteStr,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        }
    }

    //是否注灵
    public bool IfRoseZhuLing(string ZhuLingID) {

        string ZhuLingIDSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhuLingIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (ZhuLingIDSetStr.Contains(ZhuLingID) == false)
        {
            return false;
        }
        else
        {
            return true;
        }

            
    
    }

}
