using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function_Pasture {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    //创建一个牧场动物
    public string CreatePastureAI(string pastureID) {

        string nowID = GetPastureNull();
        if (nowID != "-1")
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureID", pastureID, "ID", nowID, "RosePasture");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Time", "0", "ID", nowID, "RosePasture");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Position", "0,0,0", "ID", nowID, "RosePasture");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PasturUpLv", "0", "ID", nowID, "RosePasture");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LastXiaDanTime", "0", "ID", nowID, "RosePasture");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XiaDanNum", "0", "ID", nowID, "RosePasture");

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QiangZhuang", Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 5).ToString(), "ID", nowID, "RosePasture");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FanZhi", Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 5).ToString(), "ID", nowID, "RosePasture");

            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePasture");

            //显示动物
            if (Application.loadedLevelName == "JiaYuan")
            {
                if (Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj != null)
                {
                    Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().AddPastureObj(nowID);
                }
            }

            //写入成就
            Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("223", pastureID, "1");
            //写入成就
            Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("224", "0", "1");
        }
        else {
            Debug.Log("牧场已满！");
        }
        return "";
    }


    //删除一个牧场动物
    public void DeletePastureAI(string pastureID)
    {
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureID", "0", "ID", pastureID, "RosePasture");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Time", "0", "ID", pastureID, "RosePasture");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Position", "0,0,0", "ID", pastureID, "RosePasture");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PasturUpLv", "0", "ID", pastureID, "RosePasture");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LastXiaDanTime", "0", "ID", pastureID, "RosePasture");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XiaDanNum", "0", "ID", pastureID, "RosePasture");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QiangZhuang", "0", "ID", pastureID, "RosePasture");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FanZhi", "0", "ID", pastureID, "RosePasture");

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePasture");

        if (Application.loadedLevelName == "JiaYuan") {
            if (Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj != null) {
                if (Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().NowPastureObjList.ContainsKey(pastureID)) {
                    MonoBehaviour.Destroy(Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().NowPastureObjList[pastureID]);
                    Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().NowPastureObjList.Remove(pastureID);
                }
            }
        }
    }


    //获取牧场是否有空位
    public string GetPastureNull() {

        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePastureMaxNum; i++) {
            string nowPastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureID", "ID", i.ToString(), "RosePasture");
            if (nowPastureID == "" || nowPastureID == "0")
            {
                return i.ToString();
            }
        }

        return "-1";
    }



    //获取牧场当前人口总数
    public int GetPasturePeopleNum() {
        int nowNum = 0;
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePastureMaxNum; i++)
        {
            string nowPastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureID", "ID", i.ToString(), "RosePasture");
            if (nowPastureID != "" && nowPastureID != "0")
            {
                string nowPeopleNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PeopleNum", "ID", nowPastureID, "Pasture_Template");
                if (nowPeopleNum != "") {
                    nowNum = nowNum + int.Parse(nowPeopleNum);
                }
            }
        }
        return nowNum;
    }

    //存储牧场时间(单位:秒)
    public void SavePastureTime(int timeNum)
    {

        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePastureMaxNum; i++)
        {
            string nowPastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureID", "ID", i.ToString(), "RosePasture");
            if (nowPastureID != "" && nowPastureID != "0" && nowPastureID != null)
            {
                string nowTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Time", "ID", i.ToString(), "RosePasture");
                if (nowTime == "" || nowTime == null)
                {
                    nowTime = "0";
                }

                int saveTime = int.Parse(nowTime) + timeNum;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Time", saveTime.ToString(), "ID", i.ToString(), "RosePasture");

                string nowPasturUpLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PasturUpLv", "ID", i.ToString(), "RosePasture");
                if (nowPasturUpLv == "" || nowPasturUpLv == null)
                {
                    nowPasturUpLv = "0";
                }

                //获取当前牧场动物的状态
                int endTIme = 172800;
                int nowUpLv = GetPastureUpLv(i.ToString());
                if (nowUpLv > int.Parse(nowPasturUpLv))
                {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PasturUpLv", nowUpLv.ToString(), "ID", i.ToString(), "RosePasture");
                    if (nowUpLv >= 2 && int.Parse(nowPasturUpLv) < 2)
                    {

                        string NowUpTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpTime", "ID", nowPastureID, "Pasture_Template");
                        string[] NowUpTimeListStr = NowUpTime.Split(';');
                        if (NowUpTimeListStr.Length >= 2)
                        {
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LastXiaDanTime", NowUpTimeListStr[1], "ID", i.ToString(), "RosePasture");
                        }
                        endTIme = int.Parse(NowUpTimeListStr[NowUpTimeListStr.Length - 1]);
                    }
                }

                //死亡期维持半天动物会自己消失
                if (saveTime >= endTIme + 86400 / 2) {
                    DeletePastureAI(i.ToString());
                }
            }
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePasture");

    }

    //更新下蛋数据
    public void PastureUpdateDrop() {

        string nowTimeStr = GetTimeStamp();
        string dropWriteStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiaDanData", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        int XiaDanDataInt = dropWriteStr.Split('#').Length;

        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePastureMaxNum; i++)
        {
            string nowPastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureID", "ID", i.ToString(), "RosePasture");
            if (nowPastureID != "" && nowPastureID != "0" && nowPastureID != null)
            {
                string nowTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Time", "ID", i.ToString(), "RosePasture");
                string qiangzhuang = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangZhuang", "ID", i.ToString(), "RosePasture");

                int saveTime = int.Parse(nowTime);

                string nowLastXiaDanTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LastXiaDanTime", "ID", i.ToString(), "RosePasture");
                int nowUpLv = GetPastureUpLv(i.ToString());

                //获取下蛋最后时间
                string NowUpTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpTime", "ID", nowPastureID, "Pasture_Template");
                string[] NowUpTimeListStr = NowUpTime.Split(';');
                int XiaDanMaxTime = int.Parse(NowUpTimeListStr[NowUpTimeListStr.Length - 1]);

                //只能在2-3这个阶段下蛋    nowLastXiaDanTime != "0" && 
                if (nowLastXiaDanTime != "" && nowUpLv >= 2) {

                    //计算下蛋
                    string dropTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropTime", "ID", nowPastureID, "Pasture_Template");
                    int nowLastXiaDanTimeInt = int.Parse(nowLastXiaDanTime);
                    int chaTime = saveTime - int.Parse(nowLastXiaDanTime);
                    int dropTimeInt = int.Parse(dropTime);

                    if (dropTimeInt <= XiaDanMaxTime) {

                        if (chaTime >= dropTimeInt)
                        {
                            int dropNum = (int)(chaTime / dropTimeInt);
                            //触发掉落,根据时间判定触发几次
                            for (int y = 0; y < dropNum; y++)
                            {
                                nowLastXiaDanTimeInt = nowLastXiaDanTimeInt + dropTimeInt;

                                //判断时间(超过时间不予产出鸡蛋)
                                if (nowLastXiaDanTimeInt <= XiaDanMaxTime)
                                {

                                    //获取当前概率
                                    string dropProStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GetPro", "ID", nowPastureID, "Pasture_Template");
                                    if (dropProStr == "" || dropProStr == null)
                                    {
                                        dropProStr = "0";
                                    }

                                    //判断当前下蛋数量是否已经超过50个,则不再触发掉落
                                    if (XiaDanDataInt <= 50) {

                                        //执行掉落
                                        if (float.Parse(dropProStr) <= Random.value)
                                        {
                                            //触发掉落
                                            string NowGetItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GetItemID", "ID", nowPastureID, "Pasture_Template");
                                            int NowGetItemIDNumInt = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GetItemIDNum", "ID", nowPastureID, "Pasture_Template"));
                                            for (int z = 0; z < NowGetItemIDNumInt; z++)
                                            {

                                                //随机位置
                                                string NowPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position", "ID", i.ToString(), "RosePasture");
                                                if (NowPosition == "" || NowPosition == null)
                                                {
                                                    NowPosition = "0,0,0";
                                                }
                                                string[] NowPositionListStr = NowPosition.Split(',');
                                                float addDis_x = Random.value * 5f - 2f;
                                                float addDis_z = Random.value * 5f - 2f;

                                                float pos_x = float.Parse(NowPositionListStr[0]) + addDis_x;
                                                float pos_y = float.Parse(NowPositionListStr[1]);
                                                float pos_z = float.Parse(NowPositionListStr[2]) + addDis_z;

                                                //生成唯一标识
                                                string weiyiID = i + "_" + nowTimeStr + "_" + y + "_" + z;

                                                Vector3 nowXiaDanPosi = new Vector3(pos_x, pos_y, pos_z);
                                                Vector3 chushiPosi = new Vector3(-8, 1.1f, 0);
                                                float xiaDanDis = Vector3.Distance(nowXiaDanPosi, chushiPosi);
                                                if (xiaDanDis >= 8) {
                                                    pos_x = float.Parse(NowPositionListStr[0]);
                                                    pos_y = float.Parse(NowPositionListStr[1]);
                                                    pos_z = float.Parse(NowPositionListStr[2]);
                                                }

                                                //写入值
                                                string writeQiangZhuang = "1";
                                                switch (qiangzhuang)
                                                {
                                                    case "1":
                                                        writeQiangZhuang = "0.8";
                                                        break;
                                                    case "2":
                                                        writeQiangZhuang = "0.9";
                                                        break;
                                                    case "3":
                                                        writeQiangZhuang = "1";
                                                        break;
                                                    case "4":
                                                        writeQiangZhuang = "1.1";
                                                        break;
                                                    case "5":
                                                        writeQiangZhuang = "1.2";
                                                        break;
                                                }

                                                string writePosition = weiyiID + ";" + NowGetItemID + ";" + pos_x.ToString("F2") + "," + "1.1" + "," + pos_z.ToString("F2") + ";" + writeQiangZhuang;

                                                if (dropWriteStr == "")
                                                {
                                                    dropWriteStr = writePosition;
                                                }
                                                else
                                                {
                                                    dropWriteStr = dropWriteStr + "#" + writePosition;
                                                }

                                                string xiaDanNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiaDanNum", "ID", i.ToString(), "RosePasture");
                                                if (xiaDanNumStr == "" || xiaDanNumStr == null) {
                                                    xiaDanNumStr = "0";
                                                }

                                                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XiaDanNum", (int.Parse(xiaDanNumStr) + 1).ToString(), "ID", i.ToString(), "RosePasture");

                                                XiaDanDataInt = XiaDanDataInt + 1;
                                            }
                                        }
                                    }

                                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LastXiaDanTime", nowLastXiaDanTimeInt.ToString(), "ID", i.ToString(), "RosePasture");
                                }
                            }
                        }
                    }
                }
            }
        }

        //记录时间
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XiaDanData", dropWriteStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePasture");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

    }


    public void PastureDeleteDrop(string dropID) {

        string writeDropData = "";
        string dropName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiaDanData", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] dropItemStrList = dropName.Split('#');
        if (dropItemStrList.Length >= 1) {
            for (int i = 0; i < dropItemStrList.Length; i++) {
                if (dropItemStrList[i] != "" && dropItemStrList[i] != null) {
                    string nowDropID = dropItemStrList[i].Split(';')[0];
                    if (nowDropID == dropID)
                    {
                        //不执行任何操作,表示删除
                    }
                    else {
                        if (writeDropData == "") {
                            writeDropData = dropItemStrList[i];
                        }
                        else {
                            writeDropData = writeDropData + "#" + dropItemStrList[i];
                        }
                    }
                }
            }
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XiaDanData", writeDropData, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
    }

    //根据时间获取当前牧场的状态
    public int GetPastureUpLv(string RosePastureID) {

        string PastureTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Time", "ID", RosePastureID, "RosePasture");

        //读取状态变化时间
        int UpStatus = 0;
        string PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureID", "ID", RosePastureID, "RosePasture");
        string NowUpTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpTime", "ID", PastureID, "Pasture_Template");
        string[] NowUpTimeListStr = NowUpTime.Split(';');
        for (int i = 0; i < NowUpTimeListStr.Length; i++)
        {
            if (int.Parse(PastureTime) >= int.Parse(NowUpTimeListStr[i]))
            {
                UpStatus = UpStatus + 1;
            }
        }

        if (UpStatus > 4)
        {
            UpStatus = 4;
        }
        return UpStatus;

    }

    //获取成长阶段名称
    public string GetUpLvStatusName(string UpStatus) {
        string writeUpStatusName = "";
        switch (UpStatus)
        {
            case "0":
                writeUpStatusName = "幼崽期";
                break;

            case "1":
                writeUpStatusName = "成长期";
                break;

            case "2":
                writeUpStatusName = "成熟期";
                break;

            case "3":
                writeUpStatusName = "衰老期";
                break;

            case "4":
                writeUpStatusName = "终老期";
                break;
        }

        return writeUpStatusName;

    }


    //获取成长阶段名称
    public string GetUpLvStatusNameDes(string UpStatus)
    {
        string writeUpStatusName = "";
        switch (UpStatus)
        {
            case "0":
                writeUpStatusName = "幼崽期,此阶段不会生产";
                break;

            case "1":
                writeUpStatusName = "成长期,此阶段开始生产";
                break;

            case "2":
                writeUpStatusName = "成熟期,此阶段生产旺盛";
                break;

            case "3":
                writeUpStatusName = "衰老期,此阶段建议出售";
                break;

            case "4":
                writeUpStatusName = "终老期,建议尽快出售";
                break;
        }

        return writeUpStatusName;

    }

    //获取牧场动物出售金币
    public int PastureGetSellGold(string rosePastureID) {

        string PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureID", "ID", rosePastureID, "RosePasture");
        int nowSellGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellGold", "ID", PastureID, "Pasture_Template"));
        return nowSellGold;

    }

    //传入人口数 返回是否满足要求
    public bool IfPasstureRenKou(int renkouNum) {

        int nowPeopleNum = GetPasturePeopleNum();
        int costPeopleNum = GetRenKouMaxNum() - nowPeopleNum;
        if (costPeopleNum >= renkouNum) {
            return true;
        }
        else {
            return false;
        }

    }

    //获取当前人口上限
    public int GetRenKouMaxNum() {
        string nowPastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string sumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PeopleNumMax", "ID", nowPastureLv, "PastureUpLv_Template");
        return int.Parse(sumStr);
    }

    //获取当前牧场资金
    public int GetPastureGold()
    {
        string nowPastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureGold", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        return int.Parse(nowPastureLv);
    }


    //牧场升级
    public bool PastureUpLv() {

        string nowPastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nowPastureExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowPastureExp == "" || nowPastureExp == null) {
            nowPastureExp = "0";
        }

        string needRoseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedRoseLv", "ID", nowPastureLv, "PastureUpLv_Template");
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < int.Parse(needRoseLv)) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请提升角色等级至" + needRoseLv + "开启下一级家园限制");
            return false;
        }

        string nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nowPastureLv, "PastureUpLv_Template");
        if (nextID != "" && nextID != "0" && nextID != null)
        {

            int addValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpLvAddExp", "ID", nowPastureLv, "PastureUpLv_Template"));
            addValue = (int)((float)(addValue) * (0.8f + Random.value * 0.4f - 0.2f));
            //Game_PublicClassVar.Get_function_Rose.CostReward("5", upLvNeedGold.ToString());
            int upExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpLvExp", "ID", nowPastureLv, "PastureUpLv_Template"));
            int nowExp = int.Parse(nowPastureExp) + addValue;
            if (nowExp >= upExp)
            {
                //写入值
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureExp", (nowExp - upExp).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureLv", nextID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("获得" + addValue + "点家园经验,你的家园等级获得提升!");

                //写入成就
                string nowLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nextID, "PastureUpLv_Template");
                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("220", "0", nowLv);

            }
            else {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("获得" + addValue + "点家园经验");
                //写入值
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureExp", nowExp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

            //更新显示
            UpdatePastureShowData();



            return true;



        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("您已满级!");
        }

        return false;
    }

    //更新牧场信息
    public void UpdatePastureShowData()
    {
        if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainRosePastureData != null)
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainRosePastureData.GetComponent<UI_MainRosePastureData>().ShowData();
        }

        //UpdateShow

    }

    //发送道具到背包,支持金币(发送道具ID,发送数量,是否广播,装备隐藏属性掉落概率,装备隐藏属性ID,装备是否进行宝石开孔)
    public bool SendPastureBag(string dropID, int dropNum, string broadcastType = "0", int qualityValue = 0, float qiangHuaValue = 1)
    {
        //Debug.Log("hideProValue = " + hideProValue);
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        //更新背包立即显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

        Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true; //拾取道具更新任务
        //Debug.Log("dropID = " + dropID);
        string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", dropID, "Item_Template");
        //将掉落的道具ID添加到背包内
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePastureBagMaxNum; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RosePastureBag");
            //Rdate = "0";
            //寻找背包内有没有相同的道具ID
            if (dropID == Rdate)
            {

                //读取当前道具数量
                string itemValue = function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RosePastureBag");

                //读取当前道具的堆叠数量的最大值
                string itemPileSum = function_DataSet.DataSet_ReadData("ItemPileSum", "ID", dropID, "Item_Template");
                int itemNum = int.Parse(itemValue) + dropNum; //将数量累加（此处没有顾忌到自己背包格子满的处理方式，以后添加）

                //当满足堆叠数量,执行道具捡取
                if (int.Parse(itemPileSum) >= itemNum)
                {
                    //添加获得的道具数量
                    function_DataSet.DataSet_WriteData("ItemNum", itemNum.ToString(), "ID", i.ToString(), "RosePastureBag");
                    function_DataSet.DataSet_SetXml("RosePastureBag");
                    //弹窗提示
                    string itemName = function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
                    switch (broadcastType)
                    {
                        //广播
                        case "0":
                            //Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "个" + itemName);
                            //Game_PublicClassVar.Get_function_UI.GameHint("你获得 "+"<color=#32CD32ff>" + dropNum.ToString() + "</color> "+"个" +"<color=#FF6347ff>" + itemName + "</color>");
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
                function_DataSet.DataSet_WriteData("ItemID", dropID, "ID", i.ToString(), "RosePastureBag");
                function_DataSet.DataSet_WriteData("ItemNum", dropNum.ToString(), "ID", i.ToString(), "RosePastureBag");
                //function_DataSet.DataSet_WriteData("HideID", equipHideID, "ID", i.ToString(), "RoseBag");

                string itemSubType = function_DataSet.DataSet_ReadData("ItemSubType", "ID", dropID, "Item_Template");
                string itemUsePar = function_DataSet.DataSet_ReadData("ItemUsePar", "ID", dropID, "Item_Template");

                //牧场道具
                if (itemType == "6")
                {
                    //牧场生物
                    if (itemSubType == "1")
                    {
                        //随机品质
                        int nowPinZhiValue = qualityValue;
                        if (qualityValue == 0) {
                            if (qiangHuaValue < 1) {
                                qiangHuaValue = 1;
                            }
                            nowPinZhiValue = (int)(Random.value * int.Parse(itemUsePar) * qiangHuaValue);
                        }
                        if (nowPinZhiValue <= 1) {
                            nowPinZhiValue = 1;
                        }

                        if (nowPinZhiValue >= int.Parse(itemUsePar)) {
                            nowPinZhiValue = int.Parse(itemUsePar);
                        }

                        function_DataSet.DataSet_WriteData("ItemPar", nowPinZhiValue.ToString(), "ID", i.ToString(), "RosePastureBag");
                    }
                }

                function_DataSet.DataSet_SetXml("RosePastureBag");

                //弹窗提示
                string itemName = function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
                switch (broadcastType)
                {
                    //广播
                    case "0":
                        //Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "个" + itemName);
                        //Game_PublicClassVar.Get_function_UI.GameHint("你获得 "+"<color=#32CD32ff>" + dropNum.ToString() + "</color> "+"个" +"<color=#FF6347ff>" + itemName + "</color>");
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

                //写入成就
                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("228", "0", "1");

                //写入活跃任务
                Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "135", "1");

                return true;
                break;  //跳出循环

            }
            //在结束循环的最后判定道具如果没有被拾取,判定为背包满了
            if (i == Game_PublicClassVar.Get_game_PositionVar.RosePastureBagMaxNum)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_301");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint("背包已满,请及时清理背包！");
                return false;
            }

        }

        return false;

    }


    public void PastureBagArrangeBag()
    {

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
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePastureBagMaxNum; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RosePastureBag");
            if (Rdate != "" && Rdate != "0")
            {
                //读取当前道具数量
                itemValue = int.Parse(function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RosePastureBag"));
                hideID = function_DataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RosePastureBag");
                itemPar = function_DataSet.DataSet_ReadData("ItemPar", "ID", i.ToString(), "RosePastureBag");
                item_GemHole = function_DataSet.DataSet_ReadData("GemHole", "ID", i.ToString(), "RosePastureBag");
                if (item_GemHole != "" && item_GemHole != "0")
                {
                    item_GemID = function_DataSet.DataSet_ReadData("GemID", "ID", i.ToString(), "RosePastureBag");
                }


                //防止整理背包出错,如果道具ID不为0,默认道具数量为1
                if (itemValue == 0)
                {
                    itemValue = 1;
                }

                //获得背包道具数量
                //获取道具背包最大堆叠数量
                //Debug.Log("Rdate = " + Rdate);
                string itemID_DuiBi = "";
                int itemPileSum = int.Parse(function_DataSet.DataSet_ReadData("ItemPileSum", "ID", Rdate, "Item_Template"));
                if (itemPileSum > itemValue)
                {
                    //向背包后面道具查询数量
                    for (int y = i + 1; y <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; y++)
                    {
                        //对比道具ID
                        itemID_DuiBi = function_DataSet.DataSet_ReadData("ItemID", "ID", y.ToString(), "RosePastureBag");
                        if (Rdate == itemID_DuiBi)
                        {
                            //道具ID相同获取数量
                            int itemNum_DuiBi = int.Parse(function_DataSet.DataSet_ReadData("ItemNum", "ID", y.ToString(), "RosePastureBag"));

                            //数量相加判定是否满足前面堆叠数量
                            int itemSunNum_DuiBi = itemValue + itemNum_DuiBi;
                            if (itemSunNum_DuiBi >= itemPileSum)
                            {
                                int itemCostNum_DuiBi = itemSunNum_DuiBi - itemPileSum;
                                itemValue = itemPileSum;
                                //当满足上一次的堆叠数量自己还有剩余数量时进行记录
                                if (itemCostNum_DuiBi > 0)
                                {
                                    function_DataSet.DataSet_WriteData("ItemNum", itemCostNum_DuiBi.ToString(), "ID", y.ToString(), "RosePastureBag");
                                }
                                else
                                {
                                    function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", y.ToString(), "RosePastureBag");
                                    function_DataSet.DataSet_WriteData("ItemID", "0", "ID", y.ToString(), "RosePastureBag");

                                }

                                if (itemCostNum_DuiBi >= 0)
                                {
                                    y = Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; //跳出循环
                                }
                            }
                            else
                            {
                                itemValue = itemValue + itemNum_DuiBi;
                                function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", y.ToString(), "RosePastureBag");
                                function_DataSet.DataSet_WriteData("ItemID", "0", "ID", y.ToString(), "RosePastureBag");
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

            }
        }
        //Debug.Log("Time333:" + getTime());
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

        string itemType = "";
        string itemQuality = "";
        for (int i = 0; i <= bagItemID_Now.Length - 1; i++)
        {
            //获取道具类型和品质
            itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", bagItemID_Now[i], "Item_Template");
            itemQuality = function_DataSet.DataSet_ReadData("ItemQuality", "ID", bagItemID_Now[i], "Item_Template");
            switch (itemType)
            {
                /*
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
                    }
                    break;
                */
                //消耗性道具
                case "6":
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
            bagItemID_ArrangeStr = bagItemID_ArrangeStr.Substring(0, bagItemID_ArrangeStr.Length - 1);
            bagItemNum_ArrangeStr = bagItemNum_ArrangeStr.Substring(0, bagItemNum_ArrangeStr.Length - 1);
            bagItemHideID_ArrangeStr = bagItemHideID_ArrangeStr.Substring(0, bagItemHideID_ArrangeStr.Length - 1);
            bagItemPar_ArrangeStr = bagItemPar_ArrangeStr.Substring(0, bagItemPar_ArrangeStr.Length - 1);
            bagItemGemHole_ArrangeStr = bagItemGemHole_ArrangeStr.Substring(0, bagItemGemHole_ArrangeStr.Length - 1);
            bagItemGemID_ArrangeStr = bagItemGemID_ArrangeStr.Substring(0, bagItemGemID_ArrangeStr.Length - 1);

            //转换成数组
            bagItemID_Arrange = bagItemID_ArrangeStr.Split(',');
            bagItemNum_Arrange = bagItemNum_ArrangeStr.Split(',');
            bagItemHideID_Arrange = bagItemHideID_ArrangeStr.Split(',');
            bagItemPar_Arrange = bagItemPar_ArrangeStr.Split('#');
            bagItemGemHole_Arrange = bagItemGemHole_ArrangeStr.Split('#');
            bagItemGemID_Arrange = bagItemGemID_ArrangeStr.Split('#');

        }
        else
        {
            //Debug.Log("背包没有东西需要整理——2");
            return;
        }

        //循环写入背包数据
        for (int i = 1; i <= bagItemID_Arrange.Length; i++)
        {
            //Debug.Log("bagItemID_Arrange[i] = " + i+ bagItemID_Arrange[i]);
            function_DataSet.DataSet_WriteData("ItemID", bagItemID_Arrange[i - 1], "ID", i.ToString(), "RosePastureBag");
            function_DataSet.DataSet_WriteData("ItemNum", bagItemNum_Arrange[i - 1], "ID", i.ToString(), "RosePastureBag");
            function_DataSet.DataSet_WriteData("HideID", bagItemHideID_Arrange[i - 1], "ID", i.ToString(), "RosePastureBag");
            function_DataSet.DataSet_WriteData("ItemPar", bagItemPar_Arrange[i - 1], "ID", i.ToString(), "RosePastureBag");
            function_DataSet.DataSet_WriteData("GemHole", bagItemGemHole_Arrange[i - 1], "ID", i.ToString(), "RosePastureBag");
            function_DataSet.DataSet_WriteData("GemID", bagItemGemID_Arrange[i - 1], "ID", i.ToString(), "RosePastureBag");
            //Debug.Log("i = " + i);
        }

        //清空其他位置的数据
        if (bagItemID_Arrange.Length < Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum)
        {
            for (int i = bagItemID_Arrange.Length + 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
            {
                //清空当前格子数据
                function_DataSet.DataSet_WriteData("ItemID", "0", "ID", i.ToString(), "RosePastureBag");
                function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", i.ToString(), "RosePastureBag");
                function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RosePastureBag");
                function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", i.ToString(), "RosePastureBag");
                function_DataSet.DataSet_WriteData("GemHole", "0", "ID", i.ToString(), "RosePastureBag");
                function_DataSet.DataSet_WriteData("GemID", "0", "ID", i.ToString(), "RosePastureBag");
                Debug.Log("i = " + i);
            }
        }

        //Debug.Log("Time666:" + getTime());
        function_DataSet.DataSet_SetXml("RosePastureBag");
        //Debug.Log("Time777:" + getTime());
        //Debug.Log("整理背包成功");
        Game_PublicClassVar.Get_function_UI.PlaySource("10004", "1");
        Game_PublicClassVar.Get_game_PositionVar.UpdatePastureBagAll = true;
    }


    //消耗一个道具在仓库内指定位置的数量  （道具ID 道具数量 格子位置, 是否扣除全部）
    public bool CostPastureBagSpaceNumItem(string itemID, int itemNum, string spaceNum, bool costAllSpace)
    {
        string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceNum, "RosePastureBag");
        string bagItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceNum, "RosePastureBag");
        //是否扣除全部
        if (costAllSpace)
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceNum, "RosePastureBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceNum, "RosePastureBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceNum, "RosePastureBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureBag");
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            return true;
        }

        if (itemID == bagItemID)
        {
            int otherValue = int.Parse(bagItemNum) - itemNum;
            if (otherValue > 0)
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", otherValue.ToString(), "ID", spaceNum, "RosePastureBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureBag");
            }
            else
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceNum, "RosePastureBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceNum, "RosePastureBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceNum, "RosePastureBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureBag");
            }
            //Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    //获取仓库对应标签页空的位置
    public string ReturnNullSpaceNum()
    {

        int spaceNum_Start = 1;
        int spaceNum_End = Game_PublicClassVar.Get_game_PositionVar.RosePastureBagMaxNum;

        for (int i = spaceNum_Start; i <= spaceNum_End; i++)
        {
            //获取当前背包格子的道具ID和数量；
            string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RosePastureBag");
            if (bagItemID == "0")
            {
                //Debug.Log("空:" + i.ToString());
                return i.ToString();
            }
        }
        return "-1";

    }


    //出售制定背包格子的道具
    public void SellPastureBagSpaceItemToMoney(string spaceID, bool ifSaveData = true)
    {
        string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RosePastureBag");
        if (itemID != "0")
        {
            string itemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceID, "RosePastureBag");
            string itemHide = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", spaceID, "RosePastureBag");

            int itemMoneyValue = GetPastureSellGold(spaceID);
            //获取品质

            int sellValue = itemMoneyValue * int.Parse(itemNum);

            //删除背包内的道具
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceID, "RosePastureBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceID, "RosePastureBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceID, "RosePastureBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "0", "ID", spaceID, "RosePastureBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", spaceID, "RosePastureBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", spaceID, "RosePastureBag");
            if (ifSaveData) {
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureBag");
            }


            //发送货币
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag("5", sellValue, "0", 0, "0", true, "56");

            //更新背包立即显示
            Game_PublicClassVar.Get_game_PositionVar.UpdatePastureBagAll = true;
        }

    }

    //获取出售价格
    public int GetPastureSellGold(string spaceID) {

        string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RosePastureBag");
        if (itemID != "0")
        {
            string itemMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", itemID, "Item_Template");
            string itemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", spaceID, "RosePastureBag");
            float itemMoneyValue = float.Parse(itemMoney);

            int parValue = 0;
            if (itemPar == "" || itemPar == null) {
                itemPar = "0";
            }
            parValue = int.Parse(itemPar);
            int goldValue = (int)(float.Parse(itemMoney) * (1 + ((float)(parValue) / 100.0f)));
            return goldValue;
        }
        return 0;

    }

    //更新家园订单
    public void UpdatePastureTrader() {

        string nowPastureTraderTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureTraderTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nowTime = GetTimeStamp();

        //初始化直接创建三个订单
        if (nowPastureTraderTime == "" || nowPastureTraderTime == null || nowPastureTraderTime == "0") {

            //记录时间
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureTraderTime", nowTime, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

            for (int i = 1; i <= 3; i++) {
                CreatePastureTrader();
            }

            return;
        }

        //创建其他订单
        long chaTime = long.Parse(nowTime) - long.Parse(nowPastureTraderTime);
        int hour = (int)chaTime / 3600;
        if (hour >= 1) {
            for (int i = 1; i <= hour; i++) {

                //记录时间
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureTraderTime", nowTime, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

                //每小时30%概率出订单
                if (Random.value <= 0.3f) {
                    CreatePastureTrader();
                }
            }
        }

    }


    //创建
    public void CreatePastureTrader() {

        //订单数量超过10个不进行接收新订单
        string nowPastureTraderID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureTraderID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowPastureTraderID.Split(';').Length >= 10) {
            return;
        }

        //随机头像ID
        string PastureTraderHeadID = "10000" + Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 9);

        //随机名字
        string PastureTraderName = GetPastureTraderName();

        //随机道具
        string PastureTraderRandomItemID = GetPastureTraderNeedItem();

        //随机要求品质
        string nowPastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nowQualityMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TraderNeedQualityMax", "ID", nowPastureLv, "PastureUpLv_Template");

        //不能超过本身要求品质
        string itemUseParStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", PastureTraderRandomItemID, "Item_Template");
        if (itemUseParStr != "" && itemUseParStr != null) {
            int maxVlaue = (int)(float.Parse(itemUseParStr) * 0.8f);
            if (int.Parse(nowQualityMax) >= maxVlaue) {
                nowQualityMax = maxVlaue.ToString();
                //Debug.Log("强制限制:" + nowQualityMax);
            }
        }

        int PastureTraderRandomQuality = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(10, int.Parse(nowQualityMax));

        //随机要求数量
        string nowTraderNeedNeedNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TraderNeedNeedNum", "ID", nowPastureLv, "PastureUpLv_Template");
        int PastureTraderNeedNeedNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, int.Parse(nowTraderNeedNeedNum));

        //随机时间
        string nowTime = "86400";

        //收购单价
        //读取道具售价
        string SellMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", PastureTraderRandomItemID, "Item_Template");
        float proValue = (float)(PastureTraderRandomQuality) / 100.0f / 0.1f * 0.8f + 2f * Random.value;
        int nowBuyJiaGe = (int)(float.Parse(SellMoney) * proValue);
        //int nowBuyJiaGe = 10000;

        //头像ID,Npc名称,需要道具,需要品质,自己完成数量,要求的数量,剩余时间,收购价格
        string writeStr = PastureTraderHeadID + "," + PastureTraderName + "," + PastureTraderRandomItemID + "," + PastureTraderRandomQuality + "," + "0" + "," + PastureTraderNeedNeedNum + "," + nowTime + "," + nowBuyJiaGe;


        if (nowPastureTraderID == "" || nowPastureTraderID == null || nowPastureTraderID == "0")
        {
            nowPastureTraderID = writeStr;
        }
        else {
            nowPastureTraderID = nowPastureTraderID + ";" + writeStr;
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureTraderID", nowPastureTraderID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

    }

    //放弃家园订单
    public void CanclePastureTrader(string cancleID) {

        //订单数量超过10个不进行接收新订单
        string nowPastureTraderID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureTraderID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] nowPastureTraderIDList = nowPastureTraderID.Split(';');
        string writeStr = "";
        for (int i = 0; i < nowPastureTraderIDList.Length; i++) {
            if (i.ToString() != cancleID) {
                writeStr = writeStr + nowPastureTraderIDList[i] + ";";
            }
        }

        if (writeStr != "") {
            writeStr = writeStr.Substring(0, writeStr.Length - 1);
        }
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureTraderID", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

    }


    //订单缩减时间
    public void CostTimePastureTrader(int costTime)
    {

        //订单数量超过10个不进行接收新订单
        string nowPastureTraderID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureTraderID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] nowPastureTraderIDList = nowPastureTraderID.Split(';');
        string writeStr = "";
        for (int i = 0; i < nowPastureTraderIDList.Length; i++)
        {
            //获取订单时间
            string[] dingdanList = nowPastureTraderIDList[i].Split(',');
            if (dingdanList.Length >= 8) {
                int nowTime = int.Parse(dingdanList[6]);
                nowTime = nowTime - costTime;
                if (nowTime <= 0) {
                    CanclePastureTrader(i.ToString());
                }
                else
                {
                    dingdanList[6] = nowTime.ToString();
                    string writeDingDanStr = "";
                    for (int y = 0; y < dingdanList.Length; y++) {
                        writeDingDanStr = writeDingDanStr + dingdanList[y] + ",";
                    }

                    if (writeDingDanStr != "")
                    {
                        writeDingDanStr = writeDingDanStr.Substring(0, writeDingDanStr.Length - 1);
                    }

                    writeStr = writeStr + writeDingDanStr + ";";
                }
            }
        }

        if (writeStr != "")
        {
            writeStr = writeStr.Substring(0, writeStr.Length - 1);
        }
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureTraderID", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
    }


    //提交订单（返回为True表示完成订单,返回为False,表示完成了一部分）
    public bool CompletePastureTrader(string dingdanID, int completeNum)
    {
        //bool writeStatus = false;
        //订单数量超过10个不进行接收新订单
        string nowPastureTraderID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureTraderID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] nowPastureTraderIDList = nowPastureTraderID.Split(';');
        string writeStr = "";
        for (int i = 0; i < nowPastureTraderIDList.Length; i++)
        {
            if (dingdanID == i.ToString())
            {

                //获取订单时间
                string[] dingdanList = nowPastureTraderIDList[i].Split(',');
                if (dingdanList.Length >= 8)
                {
                    int nowNum = int.Parse(dingdanList[4]);
                    int needNum = int.Parse(dingdanList[5]);

                    nowNum = nowNum + completeNum;

                    if (nowNum >= needNum)
                    {
                        //表示完成了当前所有订单,删除订单任务
                        CanclePastureTrader(i.ToString());
                        //writeStatus = true;

                        //发送奖励
                        int nowjiage = int.Parse(dingdanList[7]);
                        int nowsendValue = nowjiage * completeNum;
                        Game_PublicClassVar.Get_function_Rose.SendReward("5", nowsendValue.ToString());

                        return true;
                    }
                    else
                    {
                        dingdanList[4] = nowNum.ToString();
                        string writeDingDanStr = "";
                        for (int y = 0; y < dingdanList.Length; y++)
                        {
                            writeDingDanStr = writeDingDanStr + dingdanList[y] + ",";
                        }

                        if (writeDingDanStr != "")
                        {
                            writeDingDanStr = writeDingDanStr.Substring(0, writeDingDanStr.Length - 1);
                        }

                        writeStr = writeStr + writeDingDanStr + ";";
                    }

                    //发送奖励
                    int jiage = int.Parse(dingdanList[7]);
                    int sendValue = jiage * completeNum;
                    Game_PublicClassVar.Get_function_Rose.SendReward("5", sendValue.ToString());
                }
            }
            else {
                writeStr = writeStr + nowPastureTraderIDList[i] + ";";
            }
        }

        if (writeStr != "")
        {
            writeStr = writeStr.Substring(0, writeStr.Length - 1);
        }
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureTraderID", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

        return false;
    }


    //提交订单（返回为True表示完成订单,返回为False,表示完成了一部分）
    public bool IfCompletePastureTrader(string dingdanID, string[] SpaceListStr)
    {
        //bool writeStatus = false;
        //订单数量超过10个不进行接收新订单
        string nowPastureTraderID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureTraderID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] nowPastureTraderIDList = nowPastureTraderID.Split(';');
        string writeStr = "";
        for (int i = 0; i < nowPastureTraderIDList.Length; i++)
        {
            if (dingdanID == i.ToString())
            {

                //获取订单时间
                string[] dingdanList = nowPastureTraderIDList[i].Split(',');
                if (dingdanList.Length >= 8)
                {
                    int nowNum = int.Parse(dingdanList[4]);
                    int needNum = int.Parse(dingdanList[5]);
                    string needItemID = dingdanList[2];
                    int needQualityValue = int.Parse(dingdanList[3]);

                    for (int y = 0; y < SpaceListStr.Length; y++)
                    {
                        if (SpaceListStr[y] != "" && SpaceListStr[y] != "0" && SpaceListStr[y] != null) {

                            //读取信息
                            string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", SpaceListStr[y], "RosePastureBag");
                            string bagQualityValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", SpaceListStr[y], "RosePastureBag");

                            if (bagItemID == needItemID)
                            {
                                if (int.Parse(bagQualityValue) >= needQualityValue)
                                {
                                    //符合要求不做任何处理
                                }
                                else {
                                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("放入道具品质不符");
                                    return false;
                                }
                            }
                            else {
                                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("放入道具名称不符");
                                return false;
                            }
                        }
                    }
                }
            }
        }

        return true;
    }


    //是否符合商人要求的道具
    public bool IfPastureTraderItem(int dingdanID, int PastureSpace)
    {
        //订单数量超过10个不进行接收新订单
        string nowPastureTraderID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureTraderID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] nowPastureTraderIDList = nowPastureTraderID.Split(';');
        if (nowPastureTraderIDList.Length >= dingdanID) {
            //获取订单时间
            string[] dingdanList = nowPastureTraderIDList[dingdanID].Split(',');
            if (dingdanList.Length >= 8)
            {
                string needItem = dingdanList[2];
                int needQuality = int.Parse(dingdanList[3]);

                //获取指定位置的道具
                string itemID = Game_PublicClassVar.function_DataSet.DataSet_ReadData("ItemID", "ID", PastureSpace.ToString(), "RosePastureBag");
                string itemPar = Game_PublicClassVar.function_DataSet.DataSet_ReadData("ItemPar", "ID", PastureSpace.ToString(), "RosePastureBag");

                if (itemID == needItem) {
                    if (int.Parse(itemPar) >= needQuality) {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    public string[] GetPastureTraderIDData(int dingdanID)
    {
        //订单数量超过10个不进行接收新订单
        string nowPastureTraderID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureTraderID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] nowPastureTraderIDList = nowPastureTraderID.Split(';');
        if (nowPastureTraderIDList.Length >= dingdanID)
        {
            //获取订单时间
            string[] dingdanList = nowPastureTraderIDList[dingdanID].Split(',');
            if (dingdanList.Length >= 8)
            {
                return dingdanList;
            }
        }

        return null;
    }



    //获取家园名称
    public string GetPastureTraderName() {

        int ranValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 9);
        string returnName = "";
        switch (ranValue) {

            case 1:
                returnName = "来自地渊大厅的商人";
                break;

            case 2:
                returnName = "来自圣歌平原的商人";
                break;

            case 3:
                returnName = "来自南镇郊外的商人";
                break;

            case 4:
                returnName = "来自荒漠之地的商人";
                break;

            case 5:
                returnName = "来自风之谷的商人";
                break;

            case 6:
                returnName = "来自海巫宫殿的商人";
                break;

            case 7:
                returnName = "来自蘑菇森林的商人";
                break;

            case 8:
                returnName = "来自熔岩之地的商人";
                break;

            case 9:
                returnName = "来自沉船海滩的商人";
                break;
        }

        return returnName;
    }

    public string GetPastureTraderNeedItem() {

        //获取当前牧场等级
        string nowPastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nowTraderNeedItem = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TraderNeedItem", "ID", nowPastureLv, "PastureUpLv_Template");
        string[] nowTraderNeedItemList = nowTraderNeedItem.Split(';');
        int ranValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, nowTraderNeedItemList.Length - 1);
        return nowTraderNeedItemList[ranValue];

    }


    public int GetPastureTraderNeedItemNum(int dingdanID)
    {
        //订单数量超过10个不进行接收新订单
        string nowPastureTraderID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureTraderID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] nowPastureTraderIDList = nowPastureTraderID.Split(';');
        if (nowPastureTraderIDList.Length >= dingdanID)
        {
            //获取订单信息
            string[] dingdanList = nowPastureTraderIDList[dingdanID].Split(',');
            int nowNum = int.Parse(dingdanList[4]);
            int needNum = int.Parse(dingdanList[5]);
            int returnNum = needNum - nowNum;
            if (returnNum <= 0) {
                returnNum = 0;
            }
            return returnNum;
        }

        return 0;
    }


    //品质
    public int GetPastureItemQuality(int value, int itemlv = 0) {

        if (itemlv != 0) {
            value = (int)((float)value * (float)((float)itemlv / 10.0f / 3.0f));
        }

        int minValue = value - 20;
        int maxValue = value + 20;
        if (minValue <= 1) {
            minValue = 1;
        }
        if (maxValue >= 100) {
            maxValue = 100;
        }
        int returnInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(minValue, maxValue);
        return returnInt;
    }


    //品质
    public int GetPastureItemQualityHeCheng(int min, int max, int itemNum)
    {

        float addProValue = 1;
        if (itemNum == 2) {
            addProValue = 1.1f;
        }
        if (itemNum == 3) {
            addProValue = 1.2f;
        }

        int minValue = (int)((float)min * 0.75f * addProValue);
        int maxValue = (int)((float)max * 1.25f * addProValue);
        if (minValue <= 1)
        {
            minValue = 1;
        }
        if (maxValue >= 100)
        {
            maxValue = 100;
        }

        int returnInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(minValue, maxValue);
        return returnInt;
    }


    //创建坐骑
    public void CreateZuoQi() {

        string nowZuoQiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowZuoQiID == "" || nowZuoQiID == "0") {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiLv", "10001", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiBaoShiDu", "60", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiBaoStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiXianJiExp", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiJieDuanLv", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_1", "750", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_2", "750", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_3", "750", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_4", "750", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiLv_1", "10001", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiLv_2", "20001", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiLv_3", "30001", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiLv_4", "40001", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiExp_1", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiExp_2", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiExp_3", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiExp_4", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

            //初始化皮肤
            ZuoQi_AddPiFu("10001");
        }
    }


    //增加坐骑经验
    public void ZuoQiAddXianJiExp(int addValue) {
        string zuoQiExpStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiXianJiExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (zuoQiExpStr == "" || zuoQiExpStr == null) {
            zuoQiExpStr = "0";
        }

        int zuoQiExp = int.Parse(zuoQiExpStr);
        zuoQiExp = zuoQiExp + addValue;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiXianJiExp", zuoQiExp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("增加" + addValue + "点献祭值");

    }

    //随机增加坐骑资质
    //创建坐骑
    public void AddZuoQiZiZhi()
    {

        string nowZuoQiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowZuoQiID != "" && nowZuoQiID != "0")
        {
            int zizhiMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhiMax", "ID", nowZuoQiID, "ZuoQi_Template"));
            int ziZhi_1 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiZiZhi_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData"));
            int ziZhi_2 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiZiZhi_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData"));
            int ziZhi_3 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiZiZhi_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData"));
            int ziZhi_4 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiZiZhi_4", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData"));

            int ziZhi_1_Random = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(5, 15);
            int ziZhi_2_Random = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(5, 15);
            int ziZhi_3_Random = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(5, 15);
            int ziZhi_4_Random = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(5, 15);

            ziZhi_1 = ziZhi_1 + ziZhi_1_Random;
            ziZhi_2 = ziZhi_2 + ziZhi_2_Random;
            ziZhi_3 = ziZhi_3 + ziZhi_3_Random;
            ziZhi_4 = ziZhi_4 + ziZhi_4_Random;

            if (ziZhi_1 >= zizhiMax)
            {
                ziZhi_1 = zizhiMax;
            }

            if (ziZhi_2 >= zizhiMax)
            {
                ziZhi_2 = zizhiMax;
            }

            if (ziZhi_3 >= zizhiMax)
            {
                ziZhi_3 = zizhiMax;
            }

            if (ziZhi_4 >= zizhiMax)
            {
                ziZhi_4 = zizhiMax;
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_1", ziZhi_1.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_2", ziZhi_2.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_3", ziZhi_3.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiZiZhi_4", ziZhi_4.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你,坐骑资质获得了提升!");

            //更新装备属性
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;

        }
    }



    //坐骑进阶
    public bool ZuoQiJinJie() {

        string nowZuoQiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string zuoQiJieDuanLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowZuoQiLv, "ZuoQi_Template");
        //string zuoQiJieDuanLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiJieDuanLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nowZuoQiJieDuanLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiJieDuanLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (zuoQiJieDuanLv == "" || nowZuoQiJieDuanLv == "") {
            zuoQiJieDuanLv = "0";
        }

        int xianZhiValue = (int.Parse(zuoQiJieDuanLv) + 1) * 10;
        if (xianZhiValue >= 100) {
            xianZhiValue = 100;
        }


        bool ifUpdateLv = true;
        for (int i = 1; i <= 4; i++) {
            string nowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + i.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            string nowLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowID, "ZuoQiNengLi_Template");

            if (int.Parse(nowLv) < xianZhiValue) {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先提升坐骑能力等级至" + xianZhiValue + "级");
                ifUpdateLv = false;
                return false;
            }
        }

        int zuoQiJieDuanLvInt = int.Parse(zuoQiJieDuanLv);
        if (zuoQiJieDuanLvInt >= 10) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("坐骑阶数已满!");
            return false;
        }

        if (ifUpdateLv) {
            zuoQiJieDuanLvInt = zuoQiJieDuanLvInt + 1;

            string nowNextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nowZuoQiLv, "ZuoQi_Template");
            if (nowNextID != "0" && nowNextID != "" && nowNextID != null) {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiLv", nowNextID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiJieDuanLv", zuoQiJieDuanLvInt.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("坐骑进阶成功!当前坐骑达到" + zuoQiJieDuanLvInt + "阶");

            //更新装备属性
            Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty(true);
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;

            return true;
        }

        return false;
    }


    //坐骑能力升级
    public void ZuoQiNengLiUp(string type) {

        string nowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + type, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        int nowLvExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiExp_" + type, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData"));
        string nowLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowID, "ZuoQiNengLi_Template");
        int nowLvExpMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Exp", "ID", nowID, "ZuoQiNengLi_Template"));

        int chaValue = nowLvExp - nowLvExpMax;

        if (chaValue >= 0) {
            string nowNextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nowID, "ZuoQiNengLi_Template");
            if (nowNextID != "0" && nowNextID != "") {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiLv_" + type, nowNextID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiExp_" + type, chaValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

                //更新装备属性
                //Game_PublicClassVar.Get_game_PositionVar.UpdataRoseProperty = true;
                Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty(true);
            }
        }

        //写入坐骑成就
        int lvSum = 0;
        for (int i = 1; i <= 4; i++) {
            string nowZuoQiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + type, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            if (nowZuoQiID != "" && nowZuoQiID != "0" && nowZuoQiID != null) {
                string nowZuoQiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowZuoQiID, "ZuoQiNengLi_Template");
                lvSum = lvSum + int.Parse(nowZuoQiLv);
            }
        }

        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("222", "0", lvSum.ToString());
    }

    public void ZuoQiNengLiAddExp(string type, int addValue) {

        //如果类型为0就随机增加一种
        if (type == "0")
        {
            type = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 4).ToString();
        }

        string nowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + type, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        int nowLvExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiExp_" + type, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData"));
        string nowLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowID, "ZuoQiNengLi_Template");
        int nowLvExpMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Exp", "ID", nowID, "ZuoQiNengLi_Template"));

        nowLvExp = nowLvExp + addValue;

        int chaValue = nowLvExp - nowLvExpMax;
        if (chaValue >= 0)
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiExp_" + type, nowLvExp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            ZuoQiNengLiUp(type);
        }
        else {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQi_NengLiExp_" + type, nowLvExp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
        }
        //提示信息
        string hintName = "";
        switch (type) {

            case "1":
                hintName = "伤害能力";
                break;

            case "2":
                hintName = "法术能力";
                break;

            case "3":
                hintName = "生存能力";
                break;

            case "4":
                hintName = "防御能力";
                break;

        }

        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("坐骑" + hintName + "增加" + addValue + "点经验");
    }

    //增加饱食度
    public void ZuoQiAddBaoShiDu(int addValue)
    {
        int zuoQiExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoShiDu", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData"));
        if (zuoQiExp < 100)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("坐骑增加了" + addValue + "点饱食度");
        }

        zuoQiExp = zuoQiExp + addValue;
        if (zuoQiExp >= 100) {
            zuoQiExp = 100;
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiBaoShiDu", zuoQiExp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose != null)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseZuoQiBaoShiDu = zuoQiExp;
        }

    }

    //降低饱食度
    public void ZuoQiCostBaoShiDu(int costValue)
    {

        string nowZuoQiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowZuoQiLv == "" || nowZuoQiLv == "0" || nowZuoQiLv == null) {
            return;
        }

        //没有坐骑外观的不扣除
        string NowZuoQiShowIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowZuoQiShowID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (NowZuoQiShowIDStr == "" || NowZuoQiShowIDStr == "0" || NowZuoQiShowIDStr == null)
        {
            return;
        }

        string baoshiduStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoShiDu", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (baoshiduStr == "" || baoshiduStr == null) {
            baoshiduStr = "0";
        }
        int zuoQiExp = int.Parse(baoshiduStr);
        zuoQiExp = zuoQiExp - costValue;
        if (zuoQiExp <= 0)
        {
            zuoQiExp = 0;
        }
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiBaoShiDu", zuoQiExp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose != null)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseZuoQiBaoShiDu = zuoQiExp;
        }

        if (zuoQiExp <= 60) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("坐骑饱食度降低至60以下,坐骑移动的速度将会降低!");
        }
    }

    //根据坐骑状态值显示名称
    public string ReturnStatusName(string status) {
        string returnName = "";
        switch (status) {

            case "1":
                returnName = "状态1";
                break;

            case "2":
                returnName = "状态2";
                break;

            case "3":
                returnName = "状态3";
                break;

            case "4":
                returnName = "状态4";
                break;

            case "5":
                returnName = "状态5";
                break;
        }

        return returnName;
    }

    //是否拥有坐骑
    public bool IfHaveZuoQi(string zuoQiID) {

        string nowZuoQiPiFuSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiPiFuSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowZuoQiPiFuSet.Contains(zuoQiID))
        {
            return true;
        }
        else {
            return false;
        }
    }


    //是否拥有坐骑
    public void ZuoQiTiHuanShow(string zuoQiID)
    {

        string nowZuoQiPiFuSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiPiFuSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowZuoQiPiFuSet.Contains(zuoQiID))
        {

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowZuoQiShowID", zuoQiID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

            ZuoQi_RoseShow();
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你未拥有此坐骑,无法显示");
        }
    }


    //切换外面坐骑
    public void ZuoQi_RoseShow(bool zidongStatus = true) {

        //检测当前是否隐藏坐骑显示
        /*
        if (zidongStatus == true) {

            bool ifHindShow = true;

            if (ifHindShow) {
                return;
            }
        }
        */
        //清理坐骑显示
        string zuoqiShowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowZuoQiShowID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (zuoqiShowID == "" || zuoqiShowID == "0" || zuoqiShowID == null) {
            return;
        }

        string zuoqiModelID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ModelID", "ID", zuoqiShowID, "ZuoQiShow_Template");

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ZuoQiShowSet != null) {
            Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ZuoQiShowSet);
        }


        //zuoqiModelID = "10001";

        GameObject zuoQiObj = MonoBehaviour.Instantiate((GameObject)Resources.Load("3DModel/ZuoQiModel/" + zuoqiModelID, typeof(GameObject)));
        zuoQiObj.SetActive(false);
        zuoQiObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ZuoQiShowSet.transform);
        zuoQiObj.transform.localPosition = Vector3.zero;
        zuoQiObj.transform.localScale = new Vector3(1, 1, 1);
        zuoQiObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        zuoQiObj.GetComponent<ZuoQiShowModel>().ZuoQiType = "1";
        zuoQiObj.SetActive(true);

        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ZuoQiStatus = true;

    }


    //切换玩家坐骑
    public void ZuoQi_PlayerShow(string zuoqiShowID, GameObject PlayerObj)
    {
        //Debug.Log("坐骑显示zuoqiShowID:" + zuoqiShowID);
        //清理坐骑显示
        //string zuoqiShowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowZuoQiShowID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (zuoqiShowID == "" || zuoqiShowID == "0" || zuoqiShowID == null)
        {
            return;
        }

        string zuoqiModelID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ModelID", "ID", zuoqiShowID, "ZuoQiShow_Template");
        if (zuoqiModelID == "" || zuoqiModelID == null || zuoqiModelID == "0") {
            return;
        }

        if (PlayerObj.GetComponent<Player_Status>().Obj_ZuoQiShowSet != null)
        {
            Game_PublicClassVar.Get_function_UI.DestoryTargetObj(PlayerObj.GetComponent<Player_Status>().Obj_ZuoQiShowSet);
        }


        //zuoqiModelID = "10001";

        GameObject zuoQiObj = MonoBehaviour.Instantiate((GameObject)Resources.Load("3DModel/ZuoQiModel/" + zuoqiModelID, typeof(GameObject)));
        zuoQiObj.SetActive(false);
        //zuoQiObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ZuoQiShowSet.transform);
        zuoQiObj.transform.SetParent(PlayerObj.GetComponent<Player_Status>().Obj_ZuoQiShowSet.transform);
        zuoQiObj.transform.localPosition = Vector3.zero;
        zuoQiObj.transform.localScale = new Vector3(1, 1, 1);
        zuoQiObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        zuoQiObj.GetComponent<ZuoQiShowModel>().ZuoQiType = "2";
        zuoQiObj.GetComponent<ZuoQiShowModel>().Obj_Player = PlayerObj;
        zuoQiObj.SetActive(true);

        PlayerObj.GetComponent<Player_Status>().ZuoQiStatus = true;

    }


    //切换外面坐骑
    public void ZuoQi_RoseShowShouHui()
    {

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ZuoQiShowSet != null)
        {
            Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ZuoQiShowSet);
        }

        //清理坐骑显示
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowZuoQiShowID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ZuoQiStatus = false;

    }

    public void ZuoQi_AddPiFu(string pifuID) {

        string pifuStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiPiFuSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (pifuStr == "" || pifuStr == "0" || pifuStr == null)
        {
            pifuStr = pifuID;

            /*
            //判定坐骑是否激活
            string nowZuoQiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            if (nowZuoQiID == "" || nowZuoQiID == "0")
            {
                CreateZuoQi();
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你激活坐骑成功!");
            }
            */
        }
        else
        {
            pifuStr = pifuStr + ";" + pifuID;
        }
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiPiFuSet", pifuStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

        string name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", pifuID, "ZuoQiShow_Template");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你!激活坐骑:" + name);

        //写入坐骑成就
        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("221", pifuID, "1");



    }

    //牧场ID
    public void PastureDuiHuanAdd(string addID) {

        string duihuanIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureDuiHuanID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (duihuanIDStr.Contains(addID) == false) {
            if (duihuanIDStr == "")
            {
                duihuanIDStr = addID;
            }
            else {
                duihuanIDStr = duihuanIDStr + ";" + addID;
            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureDuiHuanID", duihuanIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
        }

    }

    public bool PastureDuiHuanIDJianCe(string duiHuanID) {
        string duihuanIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureDuiHuanID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (duihuanIDStr.Contains(duiHuanID) == true)
        {
            return true;
        }
        else {
            return false;
        }
    }


    public void DuiHuanPastureGold()
    {
        string PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        int duiHuanCostZuanShi = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanCostZuanShi", "ID", PastureID, "PastureUpLv_Template"));
        int duiHuanGetPastureGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanGetPastureGold", "ID", PastureID, "PastureUpLv_Template"));

        if (duiHuanCostZuanShi <= 0)
        {
            return;
        }

        int nowZuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        if (nowZuanShi >= duiHuanCostZuanShi)
        {
            //扣除钻石发送牧场资金
            Game_PublicClassVar.Get_function_Rose.CostRMB(duiHuanCostZuanShi);
            Game_PublicClassVar.Get_function_Rose.SendReward("5", duiHuanGetPastureGold.ToString());
        }
        else
        {
            //提示钻石不足
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足,无法兑换");
        }

    }


    public void DuiHuanPastureGold_Ten()
    {
        string PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        int duiHuanCostZuanShi = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanCostZuanShi", "ID", PastureID, "PastureUpLv_Template")) * 10;
        int duiHuanGetPastureGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanGetPastureGold", "ID", PastureID, "PastureUpLv_Template")) * 10;

        if (duiHuanCostZuanShi <= 0)
        {
            return;
        }

        int nowZuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        if (nowZuanShi >= duiHuanCostZuanShi)
        {
            //扣除钻石发送牧场资金
            Game_PublicClassVar.Get_function_Rose.CostRMB(duiHuanCostZuanShi);
            Game_PublicClassVar.Get_function_Rose.SendReward("5", duiHuanGetPastureGold.ToString());
        }
        else
        {
            //提示钻石不足
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足,无法兑换");
        }

    }


    //时间戳
    public string GetTimeStamp()
    {
        System.TimeSpan ts = System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
        return System.Convert.ToInt64(ts.TotalSeconds).ToString();
    }

    //增加公告
    public void PastureGongGaoAdd(string pastureGongGao) {

        string PastureGongGao = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GongGao", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (PastureGongGao != "" && PastureGongGao != "0")
        {
            PastureGongGao = pastureGongGao + ";" + PastureGongGao;
        }
        else {
            PastureGongGao = pastureGongGao;
        }

        string[] gonggaoList = PastureGongGao.Split(';');
        string writeValue = "";
        for (int i = 0; i < gonggaoList.Length; i++) {

            if (i >= 20) {
                //最多保留20条记录
                break;
            }
            if (gonggaoList[i] != "" && gonggaoList[i] != "0") {
                writeValue = gonggaoList[i] + ";" + writeValue;
            }
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GongGao", writeValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

    }

    //增加交互次数
    public void PastureAddJiaoHuNum(int addValue) {

        string jiaohuNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JiaoHuNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (jiaohuNum == "") {
            jiaohuNum = "0";
        }

        int nowNum = int.Parse(jiaohuNum) + addValue;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JiaoHuNum", nowNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        //写入成就
        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("227", "0", "1");
        //写入活跃任务
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "134", "1");

    }

    //新建交互牧场交互数据
    public void PastureCreateJiaoHuData(Pro_JiaoHuData proJiaoHuData) {

        //每次新建3个交互数据
        string writeValue = "";
        for (int i = 0; i < proJiaoHuData.JiaoHuData.Count; i++) {

            //像服务器请求3个玩家数据（包括玩家名字,职业和农场等级）
            //string name = "测试名字";
            //string occ = "1";
            //string pastureLv = "1";
            string name = proJiaoHuData.JiaoHuData[i.ToString()].Name;
            string occ = proJiaoHuData.JiaoHuData[i.ToString()].Occ;
            string pastureLv = proJiaoHuData.JiaoHuData[i.ToString()].PastureLv;

            //唯一ID,是否交互,名字,职业,农场等级,打扫概率,探索概率,偷摸概率
            string id = (100 + i).ToString();
            //设置概率
            float dasaoPro = 1.0f;
            float tansuoPro = Random.value * 0.6f;
            float toumoPro = Random.value * 0.3f;

            string nowData = id + ";" + "0" + ";" + name + ";" + occ + ";" + pastureLv + ";" + dasaoPro.ToString("F2") + ";" + tansuoPro.ToString("F2") + ";" + toumoPro.ToString("F2");

            if (writeValue == "")
            {
                writeValue = nowData;
            }
            else {
                writeValue = writeValue + "@" + nowData;
            }
        }

        //默认1800秒刷新一次
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FirendUpdateTime", "1800","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FirendList", writeValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");

        //刷新显示
        if (Game_PublicClassVar.Get_gameServerObj.Obj_JiaoHuListSet != null) {
            Game_PublicClassVar.Get_gameServerObj.Obj_JiaoHuListSet.GetComponent<UI_PastureFirendListSet>().Init();
        }
    }


    //减去牧场交互时间
    public void CostPastureJiaoHuTime(float costTime) {

        string firendUpdateTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FirendUpdateTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (firendUpdateTime == "" || firendUpdateTime == null) {
            firendUpdateTime = "0";
        }
        float timeSum = float.Parse(firendUpdateTime) - costTime;
        if (timeSum <= 0)
        {
            //开始刷新
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002001, "");
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FirendUpdateTime", ((int)(timeSum)).ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
    }


    //写入牧场交互数据
    public void PastureWriteJiaoHuStatus(string jiaoHuID) {

        string PastureFirendListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FirendList", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (PastureFirendListStr == "")
        {
            return;
        }

        string writeStr = "";
        string[] PastureFirendList = PastureFirendListStr.Split('@');
        for (int i = 0; i < PastureFirendList.Length; i++)
        {
            string nowStr = "";
            string[] dataList = PastureFirendList[i].Split(';');
            if (dataList[0] == jiaoHuID)
            {
                dataList[1] = "1";

                //形成数据进行写入
                for (int y = 0; y < dataList.Length; y++) {

                    if (nowStr == "")
                    {
                        nowStr = dataList[y];
                    }
                    else
                    {
                        nowStr = nowStr + ";" + dataList[y];
                    }
                }
            }
            else {
                nowStr = PastureFirendList[i];
            }

            if (writeStr == "")
            {
                writeStr = nowStr;
            }
            else {
                writeStr = writeStr + "@" + nowStr;
            }
            

        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FirendList", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
    }


    //根据牧场等级随机获取一个蛋的ID
    public string PastureLvGetDanID(string pastureID) {

        string PastureFirendListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FirendListDanStr", "ID", pastureID, "PastureUpLv_Template");
        string[] pastureFirendList = PastureFirendListStr.Split(';');
        int xuhao = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, pastureFirendList.Length - 1);
        return pastureFirendList[xuhao];
    }

    //根据牧场等级随机获取一个蛋的ID
    public string PastureLvGetDongWuID(string pastureID)
    {

        string PastureFirendListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FirendListDongWuStr", "ID", pastureID, "PastureUpLv_Template");
        string[] pastureFirendList = PastureFirendListStr.Split(';');
        int xuhao = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, pastureFirendList.Length - 1);
        return pastureFirendList[xuhao];
    }

    //减去牧场交互时间
    public void CostPastureGongGaoUpdateTime(float costTime)
    {

        string firendUpdateTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureGongGaoUpdateTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (firendUpdateTime == "") {
            firendUpdateTime = "0";
        }
        float timeSum = float.Parse(firendUpdateTime) - costTime;
        if (timeSum <= 0)
        {
            //开始刷新
            PastureGongGaoRandom();
            timeSum = 3600;     //1小时刷新一次消息

            //获取额外次数
            if (costTime >= timeSum) {
                int xunhuanNum = (int)(costTime / timeSum);
                for (int i = 0; i < xunhuanNum; i++) {
                    PastureGongGaoRandom();
                }
            }
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureGongGaoUpdateTime", ((int)(timeSum)).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
    }

    //随机显示一个交互信息
    public void PastureGongGaoRandom() {

        //写入交互
        float randvalue = Random.value;
        string thingType = "0";         //0 空  1 随机描述 2 获得资金 3 获得道具 4 获得动物 5 丢失道具  6 丢失动物
        string thingDes = "";

        bool randStatus = false;
        if (randStatus == false && randvalue <= 0.5f) {
            thingType = "0";
            randStatus = true;
        }
        if (randStatus == false && randvalue > 0.5f && randvalue <= 0.8f)
        {
            thingType = "1";
            randStatus = true;
            randStatus = true;
        }

        if (randStatus == false && randvalue > 0.8f && randvalue <= 0.9f)
        {
            thingType = "2";
            randStatus = true;
        }

        if (randStatus == false && randvalue > 0.9f && randvalue <= 0.95f)
        {
            thingType = "3";
            randStatus = true;
        }

        if (randStatus == false && randvalue > 0.95f && randvalue <= 0.96f)
        {
            thingType = "4";
            randStatus = true;
        }

        if (randStatus == false && randvalue > 0.96f && randvalue <= 0.999f)
        {
            thingType = "5";
            randStatus = true;
        }

        if (randStatus == false && randvalue > 0.999f && randvalue <= 1f)
        {
            thingType = "6";
            randStatus = true;
        }

        switch (thingType) {

            case "0":

                break;

            //随机描述事件
            case "1":
                string desSet = "你的家园刚刚被玩家打扫的非常干净.;你的家园刚刚好像有人来过,不过幸亏被及时发现,好像没有什么损失！";
                string[] desList = desSet.Split(';');
                int ranInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, desList.Length - 1);
                thingDes = desList[ranInt];
                break;

            //获得资金事件
            case "2":
                desSet = "你的家园刚刚来个收货的商人,他付了<TiHuan>资金就匆匆离开了;你的家园刚刚被人被打扫了一下，幸运的发现地上有一些东西,拿起一看竟然是<TiHuan>家园资金！";
                desList = desSet.Split(';');
                ranInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, desList.Length - 1);
                thingDes = desList[ranInt];

                //随机一个活的资金的额度
                string nowPastureLvID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string PastureFirendListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaSaoGold", "ID", nowPastureLvID, "PastureUpLv_Template");

                int sendValue = (int)((Random.value + 0.5f) * float.Parse(PastureFirendListStr));
                Game_PublicClassVar.Get_function_Rose.SendReward("5", sendValue.ToString());
                thingDes = thingDes.Replace("<TiHuan>", "<color=#3CD202>" + sendValue.ToString() + "</color>");

                break;

            //获得道具事件
            case "3":
                desSet = "你的家园刚刚来个收货的商人,他走的时候匆匆遗落了<TiHuan>,好像被你捡起来了;你的家园刚刚被人被打扫了一下，幸运的发现地上有一些东西,拿起一看竟然是<TiHuan>！";
                desList = desSet.Split(';');
                ranInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, desList.Length - 1);
                thingDes = desList[ranInt];

                //随机一个奖励
                nowPastureLvID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

                //发送奖励
                string danID = Game_PublicClassVar.Get_function_Pasture.PastureLvGetDanID(nowPastureLvID);
                Game_PublicClassVar.Get_function_Pasture.SendPastureBag(danID, 1);

                string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", danID, "Item_Template");

                thingDes = thingDes.Replace("<TiHuan>", "<color=#3CD202>" + itemName + "</color>");
                break;


            //获得动物事件
            case "4":
                desSet = "有个动物在附近走失了,你上前一看竟然是<TiHuan>,于是你赶紧把它领进了家园内！;一头<TiHuan>竟然闯进了你的家园里不走了,可能你是家园太美丽了吧！";
                desList = desSet.Split(';');
                ranInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, desList.Length - 1);
                thingDes = desList[ranInt];

                string buchongStr = "";
                string nowID = GetPastureNull();
                if (nowID == "-1")
                {
                    buchongStr = "但是你家园好像已满,它又偷偷溜走了...";
                }

                //随机一个奖励
                nowPastureLvID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

                //发送奖励
                string dongWuID = Game_PublicClassVar.Get_function_Pasture.PastureLvGetDongWuID(nowPastureLvID);
                Game_PublicClassVar.Get_function_Pasture.CreatePastureAI(dongWuID);

                string dongwuName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", dongWuID, "Pasture_Template");

                thingDes = thingDes.Replace("<TiHuan>", "<color=#3CD202>" + dongwuName + "</color>") + buchongStr;
                break;

            //丢失道具事件
            case "5":
                desSet = "刚刚一个玩家模样人来到你的家园东翻西翻的好像在找到了什么,于是你花费了<TiHuan>家园资金又把他买回来了！！;你的家园仓库好像有什么遗失了,一个好心人替你找到了它,于是给了好心人<TiHuan>家园资金表示感谢！";
                desList = desSet.Split(';');
                ranInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, desList.Length - 1);
                thingDes = desList[ranInt];

                //随机一个奖励
                nowPastureLvID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

                //随机一个活的资金的额度
                nowPastureLvID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                PastureFirendListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaSaoGold", "ID", nowPastureLvID, "PastureUpLv_Template");

                sendValue = (int)((Random.value + 0.5f) * float.Parse(PastureFirendListStr));
                Game_PublicClassVar.Get_function_Rose.CostReward("5", sendValue.ToString());

                thingDes = thingDes.Replace("<TiHuan>", "<color=#E70000>" + sendValue.ToString() + "</color>");
                break;


            //丢失动物事件
            case "6":
                desSet = "刚刚家园里的牲畜跑出去走丢了,于是你花费了<TiHuan>家园资金又把它找回来了！！;刚刚家园里的牲畜跑出去走丢了,于是你花费了<TiHuan>家园资金又把它找回来了！！;";
                desList = desSet.Split(';');
                ranInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, desList.Length - 1);
                thingDes = desList[ranInt];

                //随机一个奖励
                nowPastureLvID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

                //随机一个活的资金的额度
                nowPastureLvID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                PastureFirendListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaSaoGold", "ID", nowPastureLvID, "PastureUpLv_Template");

                sendValue = (int)((Random.value + 0.5f) * float.Parse(PastureFirendListStr)) * 5;       //找回牲畜多花5倍资金
                Game_PublicClassVar.Get_function_Rose.CostReward("5", sendValue.ToString());

                thingDes = thingDes.Replace("<TiHuan>", "<color=#E70000>" + sendValue.ToString() + "</color>");
                break;

        }
        if (thingDes != "") {
            PastureGongGaoAdd(thingDes);
        }
    }


    //去创建矿(会根据当前等级进行创建)
    public void PastureKuang_Create() {

        //矿数据
        string nowPastureKuangSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureKuangSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowPastureKuangSetStr == "" || nowPastureKuangSetStr == null|| nowPastureKuangSetStr == "0")
        {
            nowPastureKuangSetStr = "0@0@0";
        }

        //掠夺数据
        string nowKuangLvDuoDataStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("KuangLvDuoData", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowKuangLvDuoDataStr == "" || nowKuangLvDuoDataStr == null || nowKuangLvDuoDataStr == "0")
        {
            nowKuangLvDuoDataStr = "0@0@0";
        }

        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        string[] nowPastureKuangSetStrList = nowPastureKuangSetStr.Split('@');
        string[] nowKuangLvDuoDataStrList = nowKuangLvDuoDataStr.Split('@');

        int xunhuanNum = nowPastureKuangSetStrList.Length;

        if (roseLv < 35) 
        {
            xunhuanNum = 1;
        }

        if (roseLv >= 35 && roseLv < 45)
        {
            xunhuanNum = 2;
        }

        if (xunhuanNum > 3) {
            xunhuanNum = 3;
        }

        for (int i = 0; i < xunhuanNum; i++) {
            if (nowPastureKuangSetStrList[i] == "" || nowPastureKuangSetStrList[i] == "0") {
                //初始化写入数据
                string randJinKuangType = PastureKuang_GetType();      //随机获取一个矿的种类
                nowPastureKuangSetStrList[i] = randJinKuangType + ";0,0,0;0";
                nowKuangLvDuoDataStrList[i] = "0";
            }
        }

        string writeStr = "";
        for (int i = 0; i < nowPastureKuangSetStrList.Length; i++)
        {
            writeStr = writeStr + nowPastureKuangSetStrList[i] + "@";
        }
        string writeLvDuoStr = "";
        for (int i = 0; i < nowKuangLvDuoDataStrList.Length; i++)
        {
            writeLvDuoStr = writeLvDuoStr + nowKuangLvDuoDataStrList[i] + "@";
        }

        if (writeStr != "") {

            writeStr = writeStr.Substring(0, writeStr.Length - 1);

            //矿数据
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureKuangSet", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

        }

        if (writeLvDuoStr != "") {

            writeLvDuoStr = writeLvDuoStr.Substring(0, writeLvDuoStr.Length - 1);

            //掠夺数据
            Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("KuangLvDuoData", writeLvDuoStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

            //发送服务器消息
            Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
            comStr_4.str_1 = "1";
            comStr_4.str_2 = writeLvDuoStr;
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002013, comStr_4);

        }

        //存储
        if (writeStr != "" || writeLvDuoStr != "")
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
        }

    }

    //删除一个矿
    public void PastureKuang_Delete(int DeleteKuangID)
    {

        DeleteKuangID = DeleteKuangID - 1;
        string nowPastureKuangSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureKuangSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowPastureKuangSetStr == "" || nowPastureKuangSetStr == null || nowPastureKuangSetStr == "0")
        {
            nowPastureKuangSetStr = "0@0@0";
        }

        string[] nowPastureKuangSetStrList = nowPastureKuangSetStr.Split('@');
        for (int i = 0; i < nowPastureKuangSetStrList.Length; i++)
        {
            if (DeleteKuangID == i) {
                nowPastureKuangSetStrList[i] = "0";     //写入0表示清空矿
            }
        }

        string writeStr = "";
        for (int i = 0; i < nowPastureKuangSetStrList.Length; i++)
        {
            writeStr = writeStr + nowPastureKuangSetStrList[i] + "@";
        }

        if (writeStr != "")
        {
            writeStr = writeStr.Substring(0, writeStr.Length - 1);
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureKuangSet", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
        }

    }

    //矿增加分钟
    public void PastureKuang_AddTime(int AddTime)
    {
        string nowPastureKuangSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureKuangSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowPastureKuangSetStr == "" || nowPastureKuangSetStr == null || nowPastureKuangSetStr == "0")
        {
            nowPastureKuangSetStr = "0@0@0";
        }
        string[] nowPastureKuangSetStrList = nowPastureKuangSetStr.Split('@');
        for (int i = 0; i < nowPastureKuangSetStrList.Length; i++)
        {

            string[] nowPastureKuangList = nowPastureKuangSetStrList[i].Split(';');

            if (nowPastureKuangList.Length >= 3)
            {
                //判断当前是否有宠物在驻守,有才会加时间
                string[] petList = nowPastureKuangList[1].Split(',');
                bool ifAdd = false;

                for (int y = 0; y < petList.Length; y++) {
                    if (petList[y]!="" && petList[y] != "0" && petList[y] != null) {
                        ifAdd = true;
                    }
                }

                if (ifAdd)
                {
                    int nowTime = int.Parse(nowPastureKuangList[2]);
                    nowTime = nowTime + AddTime;
                    nowPastureKuangList[2] = nowTime.ToString();
                }


                string writekuangStr = "";
                for (int y = 0; y < nowPastureKuangList.Length; y++)
                {
                    writekuangStr = writekuangStr + nowPastureKuangList[y] + ";";
                }
                if (writekuangStr != "")
                {
                    writekuangStr = writekuangStr.Substring(0, writekuangStr.Length - 1);
                    nowPastureKuangSetStrList[i] = writekuangStr;
                }

            }
        }

        string writeStr = "";
        for (int i = 0; i < nowPastureKuangSetStrList.Length; i++)
        {
            writeStr = writeStr + nowPastureKuangSetStrList[i] + "@";
        }

        if (writeStr != "")
        {
            writeStr = writeStr.Substring(0, writeStr.Length - 1);
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureKuangSet", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
        }

    }

    //设置出站宠物
    public void PastureKuang_SheZhiPet(string setPetID,int setSpace,int setKuangSpace)
    {

        string nowPastureKuangSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureKuangSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowPastureKuangSetStr == "" || nowPastureKuangSetStr == null || nowPastureKuangSetStr == "0")
        {
            nowPastureKuangSetStr = "0@0@0";
        }

        string[] nowPastureKuangSetStrList = nowPastureKuangSetStr.Split('@');
        for (int i = 0; i < nowPastureKuangSetStrList.Length; i++)
        {

            string[] nowPastureKuangList = nowPastureKuangSetStrList[i].Split(';');

            if (nowPastureKuangList.Length >= 3)
            {
                //设置驻守
                string[] petList = nowPastureKuangList[1].Split(',');
                for (int y = 0; y < petList.Length; y++)
                {
                    if (setPetID == petList[y]) {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("一个宠物只能驻守一个矿区位置哦!");
                        return;
                    }
                }
            }
        }

        string[] nowPastureKuangDataList = nowPastureKuangSetStrList[setKuangSpace-1].Split(';');

        if (nowPastureKuangDataList.Length >= 3)
        {
            //设置驻守
            string[] petList = nowPastureKuangDataList[1].Split(',');
            petList[setSpace - 1] = setPetID;

            string writepetStr = "";

            for (int y = 0; y < petList.Length; y++) {
                writepetStr = writepetStr + petList[y] + ",";
            }

            if (writepetStr != "") {
                writepetStr = writepetStr.Substring(0, writepetStr.Length - 1);
                nowPastureKuangDataList[1] = writepetStr;
            }

            string writekuangStr = "";
            for (int y = 0; y < nowPastureKuangDataList.Length; y++)
            {
                writekuangStr = writekuangStr + nowPastureKuangDataList[y] + ";";
            }
            if (writekuangStr != "")
            {
                writekuangStr = writekuangStr.Substring(0, writekuangStr.Length - 1);
                nowPastureKuangSetStrList[setKuangSpace - 1] = writekuangStr;
            }
        }

        string writeStr = "";
        for (int i = 0; i < nowPastureKuangSetStrList.Length; i++)
        {
            writeStr = writeStr + nowPastureKuangSetStrList[i] + "@";
        }

        if (writeStr != "")
        {
            writeStr = writeStr.Substring(0, writeStr.Length - 1);
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureKuangSet", writeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
        }

    }

    //矿场数据获取
    public string PastuteKuang_GetKuangData(int DeleteKuangID)
    {
        string returnStr = "";
        DeleteKuangID = DeleteKuangID - 1;
        string nowPastureKuangSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureKuangSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowPastureKuangSetStr == "" || nowPastureKuangSetStr == null || nowPastureKuangSetStr == "0")
        {
            nowPastureKuangSetStr = "0@0@0";
        }
        string[] nowPastureKuangSetStrList = nowPastureKuangSetStr.Split('@');
        for (int i = 0; i < nowPastureKuangSetStrList.Length; i++)
        {
            if (DeleteKuangID == i)
            {
                returnStr = nowPastureKuangSetStrList[i];
            }
        }

        return returnStr;

    }


    //随机获取一个矿种类
    public string PastureKuang_GetType() {

        float ranf = Random.value;
        //int randI = (int)(Random.value * 100000.0f);
        string returnStr = "1";

        //铁矿
        if (ranf <= 0.3f) {
            //returnStr = "1" + "_"+ randI;
            returnStr = "1";
            return returnStr;
        }

        //铜矿
        if (ranf <= 0.6f)
        {
            //returnStr = "2" + "_" + randI;
            returnStr = "2";
            return returnStr;
        }

        //银矿
        if (ranf <= 0.8f)
        {
            //returnStr = "3" + "_" + randI;
            returnStr = "3";
            return returnStr;
        }

        //金矿
        if (ranf <= 0.9f)
        {
            //returnStr = "4" + "_" + randI;
            returnStr = "4";
            return returnStr;
        }

        //钻石矿
        if (ranf <= 1f)
        {
            //returnStr = "5" + "_" + randI;
            returnStr = "5";
            return returnStr;
        }

        return returnStr;
    }



    //每个矿种类对应的基础产出
    public int PastureKuang_GetTypeZiYuan(string type) {

        switch (type) {

            case "1":
                return 5000;
                break;

            case "2":
                return 7500;
                break;

            case "3":
                return 10000;
                break;

            case "4":
                return 15000;
                break;

            case "5":
                return 20000;
                break;

        }
        return 0;
    }


    //每个矿种类对应的基础产出
    public string PastureKuang_GetTypeName(string type)
    {

        string returnName = "";

        switch (type)
        {

            case "1":
                returnName = "铁矿脉";
                break;

            case "2":
                returnName = "青铜矿脉";
                break;

            case "3":
                returnName = "白银矿脉";
                break;

            case "4":
                returnName = "黄金矿脉";
                break;

            case "5":
                returnName = "钻石矿脉";
                break;

        }

        return returnName;

    }


    //每个矿种类对应的基础产出
    public int PastureKuang_GetTypeHourNum(string type)
    {

        int returnInt = 0;

        switch (type)
        {

            case "1":
                returnInt = 6;
                break;

            case "2":
                returnInt = 8;
                break;


            case "3":
                returnInt = 10;
                break;


            case "4":
                returnInt = 12;
                break;


            case "5":
                returnInt = 16;
                break;

        }

        return returnInt;

    }


    //矿系数
    public float PastureKuang_KuangPro() {

        string nowPastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string sumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("KuangGoldRewardPro", "ID", nowPastureLv, "PastureUpLv_Template");
        if (sumStr == ""|| sumStr == null) {
            sumStr = "1";
        }

        return float.Parse(sumStr);
    }


}
