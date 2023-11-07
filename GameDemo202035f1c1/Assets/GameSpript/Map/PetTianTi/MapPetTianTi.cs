using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class MapPetTianTi : MonoBehaviour {


    //时间
    private ObscuredFloat MapTime;
    private ObscuredFloat MapTimeSum;
    private bool MapExitStatus;             //地图退出状态
    private ObscuredFloat OneTimeSum;       //1秒执行1次

    //UI类
    public GameObject Obj_MapTime;
    private GameObject mapTimeObj;
    private GameObject mapTimeShowObj;

    public Transform[] CreatePosition;
    public Transform[] CreatePosition_Act;

    public GameObject[] ObjPetList_Self;
    public GameObject[] ObjPetList_Act;

    private bool LveDuoStatus;
    private ObscuredString FightStatus;             // "0"或空表示 正在战斗  1表示己方胜利   2表示己方输了
    private ObscuredBool fightSendHint;

    private ObscuredBool fightStartStatus;
    private ObscuredFloat fightStartTime;
    private ObscuredFloat hintYanChiTime;

    // Use this for initialization
    void Start () {


        //设定地图时间
        MapTime = 360;
        mapTimeObj = (GameObject)Instantiate(Obj_MapTime);
        mapTimeObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        mapTimeObj.transform.localPosition = new Vector3(0, 0, 0);
        mapTimeObj.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
        mapTimeObj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        mapTimeObj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        mapTimeObj.transform.localScale = new Vector3(1, 1, 1);

        if (mapTimeObj != null) {
            mapTimeShowObj = mapTimeObj.GetComponent<UI_MapTime>().Obj_MapTime;
            string langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("地图剩余时间");
            string langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("秒");
            mapTimeShowObj.GetComponent<Text>().text = langstr_1 + ":" + MapTime + langstr_2;
            mapTimeObj.GetComponent<UI_MapTime>().Obj_MapChestNum.SetActive(false);
        }

        //测试召唤
        string petTiaoZhanTeam = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanTeam", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Debug.Log("petTiaoZhanTeam = " + petTiaoZhanTeam);
        string[] petTiaoZhanTeamList = petTiaoZhanTeam.Split(';');
        for (int i = 0; i < petTiaoZhanTeamList.Length; i++) {
            if (petTiaoZhanTeamList[i] != "" && petTiaoZhanTeamList[i] != null) {
                Debug.Log("petTiaoZhanTeam111 = " + i);
                Pet_ZhaoHuan(petTiaoZhanTeamList[i], "1", i);
            }
        }

        /*
        Pet_ZhaoHuan("1", "1", 0);
        Pet_ZhaoHuan("2", "1", 1);
        Pet_ZhaoHuan("3", "1", 2);
        */

        //召唤攻击目标
        //string actTargetPetDataStr = "0@10001080@51@10561@12205@378@血色软泥怪@1@5@13;4;300;11@0@3520@1320@1180@1200@1320@3000@1.05@64000001|0@10001080@45@-311@14769@810@血色软泥怪@1@15@4;26;252;6@0@3653@1320@1220@1200@884@1600@0.99@64000108;64000001|0@10003020@1@1094@1094@0@沙漠烈犬@1@20@24;3;17;19@0@2906@1496@1275@750@1161@1600@0.98@64000104";
        string actTargetPetDataStr = Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData.PetData;
        string[] actTargetPetDataList = actTargetPetDataStr.Split('|');

        //Debug.Log("actTargetPetDataStr = " + actTargetPetDataStr);

        for (int i = 0; i < actTargetPetDataList.Length; i++) {
            RosePetCreate(1, "2", actTargetPetDataList[i], i);
        }


        //隐藏不需要显示的UI和预制件
        //隐藏角色
        /*
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_RoseModel.SetActive(false);
        //隐藏角色宠物
        Game_PublicClassVar.Get_game_PositionVar.Obj_RosePetSet.SetActive(false);
        //隐藏技能栏
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI_BtnFightingSet.SetActive(false);
        //隐藏左下方的功能按钮
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(false);
        //隐藏摇杆
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_YaoGan.SetActive(false);
        //隐藏右上方的功能按钮
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI_RightSet.SetActive(false);
        //隐藏任务界面
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseTask.SetActive(false);
        //隐藏血条
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.SetActive(false);
        */

        ClearnShow(false);

        //清理自己的召唤物
        Game_PublicClassVar.function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.Obj_RoseCreatePetSet);
    }

    // Update is called once per frame
    void Update () {

        if (fightStartStatus == false) {
            fightStartTime = fightStartTime + Time.deltaTime;
            if (fightStartTime >= 3) {
                fightStartStatus = true;
                Init();
            }
        }

        //隐藏角色
        //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_RoseModel.SetActive(false);
        //隐藏角色宠物
        Game_PublicClassVar.Get_game_PositionVar.Obj_RosePetSet.SetActive(false);

        //显示地图时间
        MapTimeSum = MapTimeSum + Time.deltaTime;
        //1秒计时
        OneTimeSum = OneTimeSum + Time.deltaTime;

        if (OneTimeSum >= 1) {
            OneTimeSum = 0;

            int nowMapTime = (int)(MapTime - MapTimeSum);
            if (mapTimeShowObj != null)
            {
                if (nowMapTime >= 0)
                {
                    string langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("地图剩余时间");
                    string langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("秒");
                    mapTimeShowObj.GetComponent<Text>().text = langstr_1 + nowMapTime + langstr_2;
                }
                else
                {
                    string langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("正在退出地图");
                    mapTimeShowObj.GetComponent<Text>().text = langstr_2;
                }
            }
        }

        //地图超过时间回到主城
        if (MapTimeSum >= MapTime)
        {
            //地图时间结束,将玩家返回地图
            if (!MapExitStatus) {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ReturnBuilding.GetComponent<UI_StartGame>().Btn_RetuenBuilding();
            }

            MapExitStatus = true;
        }

        if (fightStartStatus) {
            //敌人获取自己
            int actDeathNum = 0;
            for (int i = 0; i < ObjPetList_Act.Length; i++)
            {
                if (ObjPetList_Act[i] != null)
                {
                    if (ObjPetList_Act[i].GetComponent<AIPet>().AI_Target == null || ObjPetList_Act[i].GetComponent<AIPet>().AI_Target.GetComponent<AIPet>().AI_Status == "5")
                    {
                        GameObject targetObj = getActTarget(ObjPetList_Act[i], "2");
                        ObjPetList_Act[i].GetComponent<AIPet>().AI_Target = targetObj;
                    }

                    if (ObjPetList_Act[i].GetComponent<AIPet>().AI_Status == "5")
                    {
                        actDeathNum = actDeathNum + 1;
                    }

                }
                else
                {
                    actDeathNum = actDeathNum + 1;
                }

            }

            if (actDeathNum >= ObjPetList_Act.Length)
            {
                //表示己方胜利
                FightStatus = "1";
            }


            //循环检测宠物对战的宠物目标是否为空
            int selfDeathNum = 0;
            for (int i = 0; i < ObjPetList_Self.Length; i++)
            {
                if (ObjPetList_Self[i] != null)
                {
                    if (ObjPetList_Self[i].GetComponent<AIPet>().AI_Target == null || ObjPetList_Self[i].GetComponent<AIPet>().AI_Target.GetComponent<AIPet>().AI_Status == "5")
                    {
                        GameObject targetObj = getActTarget(ObjPetList_Self[i], "1");
                        ObjPetList_Self[i].GetComponent<AIPet>().AI_Target = targetObj;
                    }

                    //判断胜利还是失败
                    if (ObjPetList_Self[i].GetComponent<AIPet>().AI_Status == "5")
                    {
                        selfDeathNum = selfDeathNum + 1;
                    }
                }
                else
                {
                    selfDeathNum = selfDeathNum + 1;
                }
            }

            if (selfDeathNum >= ObjPetList_Self.Length)
            {
                //表示己方输了
                FightStatus = "2";
            }

            if (!fightSendHint)
            {
                if (FightStatus == "1")
                {
                    hintYanChiTime = hintYanChiTime + Time.deltaTime;
                    if (hintYanChiTime >= 2) {
                        hintYanChiTime = 0;
                        fightSendHint = true;
                        fightSendRewardHint();
                    }

                }
                if (FightStatus == "2")
                {
                    hintYanChiTime = hintYanChiTime + Time.deltaTime;
                    if (hintYanChiTime >= 2)
                    {
                        hintYanChiTime = 0;
                        fightSendHint = true;
                        fightSendRewardHint();
                    }
                }
            }
        }
    }


    //初始化设定
    private void Init() {
        //设置攻击目标
        for (int i = 0; i <= 2; i++)
        {
            if (ObjPetList_Self[i] != null)
            {
                if (ObjPetList_Act[i] != null)
                {
                    ObjPetList_Self[i].GetComponent<AIPet>().AI_Target = ObjPetList_Act[i];
                }
                else
                {
                    //强制设定第一个怪物为目标
                    ObjPetList_Self[i].GetComponent<AIPet>().AI_Target = ObjPetList_Act[0];
                }
            }
        }

        for (int i = 0; i <= 2; i++)
        {
            if (ObjPetList_Act[i] != null)
            {
                if (ObjPetList_Self[i] != null)
                {
                    ObjPetList_Act[i].GetComponent<AIPet>().AI_Target = ObjPetList_Self[i];
                }
                else
                {
                    //强制设定第一个怪物为目标
                    ObjPetList_Act[i].GetComponent<AIPet>().AI_Target = ObjPetList_Self[0];
                }
            }
        }
    }

    //注销时调用
    private void OnDestroy()
    {
        //存储当前寻找的宝箱数
        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MeiRiCangBaoTrueChestNum", (ChestTrueNum - NowChestTrueNum).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        Game_PublicClassVar.Get_gameLinkServerObj.LveDuoStatus = false;
        Game_PublicClassVar.Get_gameLinkServerObj.LveDuoGoldNum = 0;

    }

    //战斗结束后发送奖励提示
    public void fightSendRewardHint() {

        string hintStr = "";

        if (FightStatus == "1")
        {
            //Debug.Log("己方胜利");
            hintStr = "comhint_35";


            if (Game_PublicClassVar.Get_gameLinkServerObj.LveDuoStatus == true)
            {
                //发送服务器消息
                Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
                comStr_4.str_1 = "2";
                comStr_4.str_2 = Game_PublicClassVar.Get_gameLinkServerObj.LveDuoKuangSpace.ToString();
                comStr_4.str_3 = Game_PublicClassVar.Get_gameLinkServerObj.LvDuoKuangZhangHaoID;
                comStr_4.str_4 = Game_PublicClassVar.Get_gameLinkServerObj.LvDuoKuangType + ";" + Game_PublicClassVar.Get_gameLinkServerObj.LvDuoKuangPetSpaceStr;

                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002013, comStr_4);

                int sendValue = Game_PublicClassVar.Get_gameLinkServerObj.LveDuoGoldNum;
                hintStr = "恭喜你!掠夺目标矿脉资源成功!\n获胜奖励：金币*" + sendValue;

                Game_PublicClassVar.Get_function_Rose.SendRewardToBag("1", sendValue);

            }
            else {

                //发送服务器结果
                Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
                comStr_4.str_1 = Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData.RankPetID;
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001503, comStr_4);
                //重新刷新当前匹配天梯数据
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001502, "1");

                //发送挑战成功奖励
                int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                int sendValue = 30000 + roseLv * 600;
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag("1", sendValue);

                hintStr = "恭喜你!赢得了今天天梯对战的胜利\n获胜奖励：金币*" + sendValue;

            }
        }

        if (FightStatus == "2")
        {
            //Debug.Log("己方输了");
            hintStr = "comhint_36";

            if (Game_PublicClassVar.Get_gameLinkServerObj.LveDuoStatus == true)
            {
                int sendValue = 0;
                hintStr = "很抱歉!掠夺目标矿脉资源失败!";
            }
            else {
                //重新刷新当前匹配天梯数据
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001502, "1");

                //发送失败奖励
                int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                int sendValue = 15000 + roseLv * 200;
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag("1", sendValue);

                hintStr = "很抱歉,你没有击败对方宠物战队\n失败奖励：金币*" + sendValue;

            }
        }

        LveDuoStatus = Game_PublicClassVar.Get_gameLinkServerObj.LveDuoStatus;

        Game_PublicClassVar.Get_gameLinkServerObj.LveDuoStatus = false;
        Game_PublicClassVar.Get_gameLinkServerObj.LveDuoGoldNum = 0;

        //弹出战斗结果提示 点击提示按钮返回主城
        GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
        string langStrHint1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint(hintStr);
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint1, returnEnterGame, returnEnterGame,"系统提示","确定","取消", returnEnterGame);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
    }

    //回到主城
    public void returnEnterGame() {

        if (LveDuoStatus)
        {
            ClearnShow(true);
            if (mapTimeObj != null)
            {
                Destroy(mapTimeObj);
            }
            Game_PublicClassVar.Get_function_Rose.RoseMoveTargetMap("JiaYuan");
            return;
        }
        else {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ReturnBuilding.GetComponent<UI_StartGame>().Btn_RetuenBuilding();
        }

    }


    //获取举例自己最近的单位设置为攻击目标
    public GameObject getActTarget(GameObject petObj, string type) {

        //Debug.Log("寻找新的攻击目标");
        int xuhaoID = 0;
        float disShort = 0;

        //自己获取敌人
        if (type == "1")
        {
            for (int i = 0; i < ObjPetList_Act.Length; i++)
            {
                if (ObjPetList_Act[i] != null)
                {
                    if (ObjPetList_Act[i].GetComponent<AIPet>().AI_Status != "5") {
                        //获取距离
                        float nowDis = Vector3.Distance(petObj.transform.position, ObjPetList_Act[i].transform.position);
                        //比较距离
                        bool saveStatus = false;
                        if (disShort == 0)
                        {
                            saveStatus = true;
                        }
                        else
                        {
                            if (nowDis < disShort)
                            {
                                saveStatus = true;
                            }
                        }
                        if (saveStatus)
                        {
                            disShort = nowDis;
                            xuhaoID = i;
                        }
                    }
                }
            }

            return ObjPetList_Act[xuhaoID];
        }


        //敌人获取自己
        if (type == "2")
        {
            for (int i = 0; i < ObjPetList_Self.Length; i++)
            {
                if (ObjPetList_Self[i] != null)
                {
                    if (ObjPetList_Self[i].GetComponent<AIPet>().AI_Status != "5")
                    {
                        //获取距离
                        float nowDis = Vector3.Distance(petObj.transform.position, ObjPetList_Self[i].transform.position);
                        //比较距离
                        bool saveStatus = false;
                        if (disShort == 0)
                        {
                            saveStatus = true;
                        }
                        else
                        {
                            if (nowDis < disShort)
                            {
                                saveStatus = true;
                            }
                        }
                        if (saveStatus)
                        {
                            disShort = nowDis;
                            xuhaoID = i;
                        }
                    }
                }
            }

            return ObjPetList_Self[xuhaoID];
        }

        return null;
    }


    //召唤出战宠物
    public void Pet_ZhaoHuan(string NowSclectPetID,string type,int posiType)
    {

        //Debug.Log("我点击了出战按钮");
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();

        //判定出战
        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", NowSclectPetID, "RosePet");
        if (petID == "0" || petID == "") {
            return;
        }
        int fightLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FightLv", "ID", petID, "Pet_Template"));
        if (roseStatus.GetComponent<Rose_Proprety>().Rose_Lv < fightLv)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_192");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            return;
        }

        //出战
        //Game_PublicClassVar.Get_function_Rose.RosePetClearn(int.Parse(NowSclectPetID));
        RosePetCreate(int.Parse(NowSclectPetID), type, "",posiType);

    }


    //召唤宠物（参数1：是否广播,宠物类型）
    public void RosePetCreate(int rosePetID, string type, string petDataStr="", int posiType = 0, bool ifSpeak = true, int PetType = 0)
    {

        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        string petID = "";
        string petModel = "";

        if (type=="1") {
            petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", rosePetID.ToString(), "RosePet");
            petModel = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetModel", "ID", petID, "Pet_Template");
        }

        if (type == "2") {
            string[] petDataStrList = petDataStr.Split('@');
            petID = petDataStrList[1];
            petModel = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetModel", "ID", petID, "Pet_Template");
        }

        if (petID == "0" || petID == "")
        {
            return;
        }

        GameObject monsterSetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_RosePetSet;

        //获取怪物并设置位置
        if (petModel != "")
        {

            //Debug.Log("petModel = " + petModel);
            GameObject monsterObj = MonoBehaviour.Instantiate((GameObject)Resources.Load("PetSet/" + petModel, typeof(GameObject)));
            monsterObj.SetActive(false);
            monsterObj.transform.SetParent(monsterSetObj.transform);
 
            
            //roseStatus.RosePetObj[rosePetID - 1] = monsterObj;

            //设置宠物的初始位置
            //addRosePetPositionObj(monsterObj);
            if (type == "1") {
                monsterObj.transform.SetParent(CreatePosition[posiType]);
                ObjPetList_Self[posiType] = monsterObj;
                monsterObj.GetComponent<AIPet>().PetTianTiType = "1";
                monsterObj.layer = 18;                                      //设置己方为pet层级
                monsterObj.GetComponent<AIPet>().PetType = "2";
                monsterObj.GetComponent<AIPet>().RosePet_ID = rosePetID.ToString();
            }

            if (type == "2") {

                Debug.Log("CreatePosition_Act.Length = " + CreatePosition_Act.Length  + "posiType = " + posiType);
                monsterObj.transform.SetParent(CreatePosition_Act[posiType]);
                monsterObj.GetComponent<AIPet>().PetTianTiDataStr = petDataStr;
                //Debug.Log("设置完成PetTianTiDataStr：" + petDataStr);
                string actTargetPetXiuLian = Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData.PetXiuLian;
                monsterObj.GetComponent<AIPet>().PetTianTiXiuLianStr = actTargetPetXiuLian;
                monsterObj.GetComponent<AIPet>().roseEquipHideStr = Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData.EuqipHideStr;
                monsterObj.GetComponent<AIPet>().EquipHideID();

                ObjPetList_Act[posiType] = monsterObj;
                monsterObj.GetComponent<AIPet>().PetTianTiType = "2";       //设置对战类型
                monsterObj.GetComponent<AIPet>().PetType = "2";

            }

            //Vector3 CreateVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
            Vector3 CreateVec3 = new Vector3(0, 0, 0);
            monsterObj.transform.localPosition = CreateVec3;
            //monsterObj.GetComponent<AIPet>().PetType = "0";
            
            monsterObj.GetComponent<AIPet>().AI_ID = long.Parse(petID);
            monsterObj.SetActive(true);
            monsterObj.GetComponent<AIPet>().StartPositionObj = monsterObj;

            //Debug.Log("pet:" + monsterObj.transform.localPosition);

            //记录当前出战
            //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetStatus", "1", "ID", rosePetID.ToString(), "RosePet");
            //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

            if (ifSpeak)
            {
                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_428");
                Game_PublicClassVar.Get_function_UI.GameHint(langStrHint_1);
                //Game_PublicClassVar.Get_function_UI.GameHint("召唤宠物成功！");

                //实例化一个特效
                GameObject zhaoHuanEffect = (GameObject)MonoBehaviour.Instantiate((GameObject)Resources.Load("Effect/Skill/Eff_Skill_ZhaoHuan_1", typeof(GameObject)));        //实例化特效
                zhaoHuanEffect.transform.position = CreateVec3;
                zhaoHuanEffect.transform.localScale = new Vector3(1, 1, 1);
            }

            //更新主界面
            /*
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().IfUpdatePetData = true;


            //发送武器显示
            if (Application.loadedLevelName == "EnterGame")
            {
                MapThread_PlayerDataChange mapThread_PlayerDataChange = new MapThread_PlayerDataChange();
                mapThread_PlayerDataChange.ChangType = "2";
                string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", rosePetID.ToString(), "RosePet");
                string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", rosePetID.ToString(), "RosePet");
                mapThread_PlayerDataChange.ChangValue = petID + "," + petName + "," + petLv;
                mapThread_PlayerDataChange.MapName = Application.loadedLevelName;
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20000204, mapThread_PlayerDataChange);
            }
            */

            //触发出战无敌buff
            if (monsterObj != null)
            {
                Game_PublicClassVar.Get_function_Skill.SkillBuff("90045010", monsterObj);
                Game_PublicClassVar.Get_function_Skill.SkillBuff("90045020", monsterObj);
            }
            /*
            if (ifSpeak)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_305");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            }
            */
        }
        else
        {
            Debug.Log("实例化的宠物为空");
        }
    }


    private void ClearnShow(bool status)
    {

        //隐藏角色
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.SetActive(status);
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_RoseModel.SetActive(status);
        //隐藏角色宠物
        Game_PublicClassVar.Get_game_PositionVar.Obj_RosePetSet.SetActive(status);
        //隐藏技能栏
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI_BtnFightingSet.SetActive(status);
        //隐藏左下方的功能按钮
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(status);
        //隐藏摇杆
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_YaoGan.SetActive(status);
        //隐藏右上方的功能按钮
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI_RightSet.SetActive(status);
        //隐藏任务界面
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseTask.SetActive(status);
        //隐藏血条
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.SetActive(status);

    }

    //添加宠物跟随坐标点
    /*
    public void addRosePetPositionObj(GameObject addPetObj)
    {

        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        for (int i = 0; i < roseStatus.RosePetPositionObj.Length; i++)
        {
            if (roseStatus.RosePetPositionObj[i] == null)
            {
                addPetObj.GetComponent<AIPet>().StartPositionObj = roseStatus.RosePetfollowPositionObj[i];
                roseStatus.RosePetPositionObj[i] = addPetObj;
                return;
            }
        }
    }
    */



}
