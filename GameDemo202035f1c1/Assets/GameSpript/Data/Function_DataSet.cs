using UnityEngine;
using System.Collections;
using System.Data;
using System.Xml;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

//本表为缓存Xml数据的表,在游戏初始化的时候将全部表缓存
public class Function_DataSet{

    //private DataSet dataSet; 
    private WWWSet wwwSet_p = Game_PublicClassVar.Get_wwwSet;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    

    //缓存XML数据表
    public DataTable DataSet_ReadXml(string xmlPath,string datatableName) {

        if (File.Exists(xmlPath))
        {

            bool xiufuStatus = false;
            string xmlPathOld = xmlPath;
            MemoryStream newms = null;
            try
            {
                //开始解密文件
                if (Game_PublicClassVar.Get_wwwSet.IfAddKey)
                {
                    //Debug.Log("加密文件：xmlPath" + xmlPath + ";datatableName = " + datatableName);
                    //判定是否上一次退出文件出错
                    //string yanZhengFileStr = PlayerPrefs.GetString("YanZhengFileStr_" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName);
                    if (Game_PublicClassVar.Get_wwwSet.XmlYanZhengPassStatus)
                    {

                        //尝试解密一次失败就进入到修复文件
                        try
                        {
                            if (Game_PublicClassVar.Get_wwwSet.useNewArchives)
                                newms = Game_PublicClassVar.Get_xmlScript.CostKeyEx(xmlPath, datatableName);
                            else
                                xmlPath = Game_PublicClassVar.Get_xmlScript.CostKey(xmlPath, datatableName);
                        }
                        catch (Exception ex) {

                            string destFile = xmlPath.Replace(".xml", "_AddJie.xml");
                            if (File.Exists(destFile))
                            {
                                xmlPath = destFile;
                                xiufuStatus = true;
                                Debug.LogError("修复文件:" + xmlPathOld);
                            }
                            else
                            {
                                if (Game_PublicClassVar.Get_wwwSet.useNewArchives)
                                    newms = Game_PublicClassVar.Get_xmlScript.CostKeyEx(xmlPath, datatableName);
                                else
                                    xmlPath = Game_PublicClassVar.Get_xmlScript.CostKey(xmlPath, datatableName);
                            }
                        }
                    }
                    else
                    {
                        if (Game_PublicClassVar.Get_wwwSet.useNewArchives)
                            newms = Game_PublicClassVar.Get_xmlScript.CostKeyEx(xmlPath, datatableName);
                        else
                            xmlPath = Game_PublicClassVar.Get_xmlScript.CostKey(xmlPath, datatableName);
                    }
                }

                //读取XML
                DataSet dateSet = new DataSet();

                if (Game_PublicClassVar.Get_wwwSet.useNewArchives)
                {
                    MemoryStream ms = new MemoryStream(newms.ToArray());
                    dateSet.ReadXml(ms);
                }
                else
                {
                    dateSet.ReadXml(xmlPath);

                }


                DataTable dataTable = new DataTable(datatableName);
                dataTable = dateSet.Tables[0].Copy();
                dataTable.TableName = datatableName;

                if (xiufuStatus) {
                    Game_PublicClassVar.Get_xmlScript.setKey(xmlPath, xmlPathOld);
                }

                //删除指定加密文件
                if (Game_PublicClassVar.Get_wwwSet.IfAddKey && !Game_PublicClassVar.Get_wwwSet.useNewArchives)
                {
                    File.Delete(xmlPath);
                }

                if (Game_PublicClassVar.Get_wwwSet.XmlYanZhengPassStatus)
                {
                    string destFile = xmlPathOld.Replace(".xml", "_JieMi.xml");
                    if (File.Exists(destFile))
                    {
                        File.Delete(destFile);
                    }
                }

                return dataTable;
            }
            catch(Exception ex) {
                Debug.LogError("加载文件：" + xmlPath + "错误！ ex = " + ex.Message);
                Game_PublicClassVar.Get_wwwSet.AddShowLog_NoKey("加载文件：" + xmlPath + "错误！ ex = " + ex.Message);

                /*
                if (xiufuStatus) {

                    string destFile = xmlPathOld.Replace(".xml", "_AddJie.xml");
                    if (File.Exists(destFile))
                    {
                        File.Delete(destFile);
                    }

                }
                */

                return null;
            }
        }
        else
        {
            Debug.Log(datatableName + "文件不存在！");
            Game_PublicClassVar.Get_wwwSet.AddShowLog_NoKey(datatableName + "文件不存在！");
            return null;
        }
    }


    //缓存XML数据表
    public DataTable DataSet_ReadXml_NoKey(string xmlPath, string datatableName)
    {

        if (File.Exists(xmlPath))
        {
            try
            {

                //读取XML
                DataSet dateSet = new DataSet();
                dateSet.ReadXml(xmlPath);
                DataTable dataTable = new DataTable(datatableName);
                dataTable = dateSet.Tables[0].Copy();
                dataTable.TableName = datatableName;

                return dataTable;
            }
            catch (Exception ex)
            {
                Debug.Log("加载文件：" + xmlPath + "错误！ ex = " + ex.Message);
                return null;
            }
        }
        else
        {
            Debug.Log(datatableName + "文件不存在！");
            return null;
        }
    }


    //写入全部XML数据表（游戏存档调用的比较多）
    public bool DataSet_AllWriteXml() {

        //Game_PublicClassVar.Get_game_PositionVar.DataSetXml.Tables["Item_Template"].WriteXml(Game_PublicClassVar.Get_game_PositionVar.Set_XmlPath + "Item_Template.xml");
        return true;
    
    }


    //单独将xml缓存的写入对应的XML文档
    public bool DataSet_SetXml(string xmlName)
    {
        /*
        if (Game_PublicClassVar.Get_gameLinkServerObj.HintMsgStatus_Exit) {
            return false;
        }
        */

        try
        {
#if !UNITY_EDITOR
            if (Game_PublicClassVar.Get_wwwSet.forceExitGameStatus) {
                return false;
            }
#endif
            if (Game_PublicClassVar.Get_wwwSet.IfAddKey)
            {

                if (Game_PublicClassVar.Get_wwwSet.useNewArchives)
                {
                    MemoryStream ss = new MemoryStream();
                    Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables[xmlName].WriteXml(ss);
                    //加密
                    //Debug.Log("开始加密存储文件");
                    Game_PublicClassVar.Get_xmlScript.setKeyEx(ss.ToArray(), Game_PublicClassVar.Get_wwwSet.Set_XmlPath + xmlName + ".xml");
                }
                else
                {
                    Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables[xmlName].WriteXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + xmlName + "_AddJie.xml");
                    Game_PublicClassVar.Get_xmlScript.setKey(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + xmlName + "_AddJie.xml", Game_PublicClassVar.Get_wwwSet.Set_XmlPath + xmlName + ".xml");
                }
            }
            else
            {
                Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables[xmlName].WriteXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + xmlName + ".xml");
            }
        }

        catch(Exception ex) {

            Debug.LogError("储存Xml报错111:" + xmlName  + "EX:" + ex);
            Game_PublicClassVar.Get_wwwSet.AddShowLog_NoKey("储存Xml报错111:" + xmlName + "EX:" + ex);
            //防止文件为只读属性
            if (ex.ToString().Contains("System.UnauthorizedAccessException"))
            {

                //防止修改存档属性
                if (Game_PublicClassVar.Get_wwwSet.WriteFileYanZhengStatus) {
                    PlayerPrefs.SetString("YanZhengFileStr_" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName, "1001");
                    PlayerPrefs.Save();
                }

                Game_PublicClassVar.Get_wwwSet.AddShowLog_NoKey("储存Xml报错222:" + xmlName + "EX:" + ex);
                Debug.LogError("储存Xml报错222:" + xmlName + "EX:" + ex);

                Application.Quit();
                Game_PublicClassVar.Get_gameLinkServerObj.HintMsgStatus_Exit = true;
                Game_PublicClassVar.Get_gameLinkServerObj.HintMsgText_Exit = "数据异常!请稍后再试";

                //强制退出游戏
                Game_PublicClassVar.Get_wwwSet.forceExitGameStatus = true;
                Game_PublicClassVar.Get_wwwSet.forceExitGameTimeSum = 0;

                if (Game_PublicClassVar.Get_wwwSet.WriteFileYanZhengStatus)
                {
                    Game_PublicClassVar.Get_gameLinkServerObj.setXmlErrorWriteYanZhengStatus = true;        //防止写入
                }

            }
            else {
                if (Game_PublicClassVar.Get_wwwSet.WriteFileYanZhengStatus)
                {
                    Game_PublicClassVar.Get_gameLinkServerObj.setXmlErrorWriteYanZhengStatus = true;
                }
            }

            Game_PublicClassVar.Get_gameLinkServerObj.serverFileYanZhengStatus = true;
        }
        return true;
    }

    //缓存全部XML数据表
    public bool DataSet_AllReadXml() {

        //获取绑点
        //GameObject gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
        //Game_PositionVar game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();
        try
        {
            //初始化数据
            Function_DataSet function_DataSet = new Function_DataSet();
            DataSet dateSet = new DataSet();
            DataTable dataTable = new DataTable();

            //缓存道具表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Item_Template.xml", "Item_Template");
            dataTable.PrimaryKey = new DataColumn[]{dataTable.Columns["ID"]};       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item1");
            //缓存掉落表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Drop_Template.xml", "Drop_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item1a");
            //缓存掉落表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Monster_Template.xml", "Monster_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item11");
		    //缓存技能表
		    dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Skill_Template.xml", "Skill_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
		    dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item111");
            //缓存任务表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Task_Template.xml", "Task_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //缓存NPC表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Npc_Template.xml", "Npc_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //缓存角色经验表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "RoseExp_Template.xml", "RoseExp_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["RoseLv"] };       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item222");
		    //缓存装备表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "EquipSuit_Template.xml", "EquipSuit_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
		    dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item333");
            //缓存装备套装表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "EquipSuitProperty_Template.xml", "EquipSuitProperty_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //缓存装备套装属性表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Equip_Template.xml", "Equip_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item444");
		    //缓存职业表
		    dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Occupation_Template.xml", "Occupation_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
		    dateSet.Tables.Add(dataTable);

            //缓存场景道具表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "SceneItem_Template.xml", "SceneItem_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item555");
            //Debug.Log("开始缓存数据Item2");
            //缓存Buff表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "SkillBuff_Template.xml", "SkillBuff_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item3");
            //缓存故事对话表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "GameStory_Template.xml", "GameStory_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item666");
            //缓存场景表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Scene_Template.xml", "Scene_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item777");
            //缓存场景传送表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "SceneTransfer_Template.xml", "SceneTransfer_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //缓存建筑表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Building_Template.xml", "Building_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //缓存章节表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Chapter_Template.xml", "Chapter_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //缓存章节子表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ChapterSon_Template.xml", "ChapterSon_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //缓存任务追踪坐标表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "TaskMovePosition_Template.xml", "TaskMovePosition_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //装备合成表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "EquipMake_Template.xml", "EquipMake_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //主参数配置表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "GameMainValue.xml", "GameMainValue");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //特殊事件
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "SpecialEvent_Template.xml", "SpecialEvent_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //每日任务
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "TaskCountry_Template.xml", "TaskCountry_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //抽卡
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "TakeCard_Template.xml", "TakeCard_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //荣誉大厅
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "HonorStore_Template.xml", "HonorStore_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //王国表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Country_Template.xml", "Country_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //宠物表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Pet_Template.xml", "Pet_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //天赋表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Talent_Template.xml", "Talent_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //收集表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ShouJiItem_Template.xml", "ShouJiItem_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //收集表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ShouJiItemPro_Template.xml", "ShouJiItemPro_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //装备格子强化
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "EquipQiangHua_Template.xml", "EquipQiangHua_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //隐藏装备属性
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "HintProList_Template.xml", "HintProList_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //场景探索表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "SceneTanSuo_Template.xml", "SceneTanSuo_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //游戏活动表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Activity_Template.xml", "Activity_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //游戏称号表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ChengHao_Template.xml", "ChengHao_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //游戏精灵表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Spirit_Template.xml", "Spirit_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //游戏成就表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ChengJiu_Template.xml", "ChengJiu_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //成就总表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ChengJiuAll_Template.xml", "ChengJiuAll_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //成就奖励表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ChengJiuReward_Template.xml", "ChengJiuReward_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //洗炼大师表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "EquipXiLianDaShi_Template.xml", "EquipXiLianDaShi_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("aaaaaaaaaaaaaaaaaaa111111111");
            //签到表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "QianDao_Template.xml", "QianDao_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("aaaaaaaaaaaaaaaaaaa");
            //令牌表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "LingPai_Template.xml", "LingPai_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);
            //Debug.Log("aaaaaaaaaaaaaaaaaaa2222222222");
            //宠物修炼
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "PetXiuLian_Template.xml", "PetXiuLian_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //牧场表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Pasture_Template.xml", "Pasture_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //牧场等级表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "PastureUpLv_Template.xml", "PastureUpLv_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //牧场兑换表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "PastureDuiHuanStore_Template.xml", "PastureDuiHuanStore_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //坐骑能力表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ZuoQiNengLi_Template.xml", "ZuoQiNengLi_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //坐骑表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ZuoQi_Template.xml", "ZuoQi_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //坐骑展示表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ZuoQiShow_Template.xml", "ZuoQiShow_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //坐骑展示表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "FuBenShangHai_Template.xml", "FuBenShangHai_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //坐骑觉醒表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "JueXing_Template.xml", "JueXing_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //染色表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "RanSe_Template.xml", "RanSe_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //活动爬塔
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "HuoDong_Tower_Template.xml", "HuoDong_Tower_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //注灵
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ZhuLing_Template.xml", "ZhuLing_Template");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);


            //用作对比
            //Game_PublicClassVar.Get_wwwSet.DataSetXml_BiDui_1 = dateSet.Copy();
            //更新DateSet
            Game_PublicClassVar.Get_wwwSet.DataSetXml = dateSet.Copy();

            //清理重新生成
            dateSet.Clear();


            //Game_PublicClassVar.Get_wwwSet.GengHuanMiChiStatus = true;
            string ChangeKey = PlayerPrefs.GetString("ChangeKey_" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName);
            if (ChangeKey == "change") {
                Game_PublicClassVar.Get_wwwSet.GengHuanMiChiStatus = true;
            }


            //缓存角色表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseData.xml", "RoseData");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            //效验数据
            DataTable wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RoseData, "RoseData");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);

            //缓存背包表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseBag.xml", "RoseBag");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            //效验数据
            wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RoseBag, "RoseBag");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);

            //缓存装备表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseEquip.xml", "RoseEquip");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            //效验数据
            wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RoseEquip, "RoseEquip");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);


            //缓存装备表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseConfig.xml", "RoseConfig");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            //效验数据
            wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RoseConfig, "RoseConfig");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);

            //缓存建筑表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseBuilding.xml", "RoseBuilding");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            //效验数据
            wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RoseBuilding, "RoseBuilding");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);

            //缓存仓库表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseStoreHouse.xml", "RoseStoreHouse");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            //效验数据
            wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RoseStoreHouse, "RoseStoreHouse");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);

            //缓存装备极品表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseEquipHideProperty.xml", "RoseEquipHideProperty");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //缓存角色每日奖励
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseDayReward.xml", "RoseDayReward");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            //效验数据
            wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RoseDayReward, "RoseDayReward");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);
            //Debug.Log("开始缓存数据Item111A");

            //缓存角色宠物表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RosePet.xml", "RosePet");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            //效验数据
            wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RosePet, "RosePet");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);

            //缓存角色其他配置表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseOtherData.xml", "RoseOtherData");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            dateSet.Tables.Add(dataTable);

            //缓存角色成就表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseChengJiu.xml", "RoseChengJiu");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键

            //效验数据
            wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RoseChengJiu, "RoseChengJiu");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);

            //牧场表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RosePasture.xml", "RosePasture");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
            
            //效验数据
            wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RosePasture, "RosePasture");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);


            //牧场表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RosePastureData.xml", "RosePastureData");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键

            //效验数据
            wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RosePastureData, "RosePastureData");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);

            //牧场仓库表
            dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RosePastureBag.xml", "RosePastureBag");
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键

            //效验数据
            wwwTable = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_RosePastureBag, "RosePastureBag");
            dataTable = CompareDataTable(wwwTable, dataTable);
            dateSet.Tables.Add(dataTable);




            //Debug.Log("开始缓存数据Item111B");
            //更新DateSet
            //Game_PublicClassVar.Get_wwwSet.DataSetXml = dateSet.Copy();
            Game_PublicClassVar.Get_wwwSet.DataWriteXml = dateSet.Copy();
            Game_PublicClassVar.Get_wwwSet.DataUpdataStatus = true;

            //DataSet_SetXml("RosePasture");

            //Game_PublicClassVar.Get_wwwSet.NpcHaXiInt = Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables["Npc_Template"].GetHashCode();

            //经测试,检测一次大约消耗350毫秒
            //string time =((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
            //Debug.Log("time = " + time);

            byte[] dateByte = Game_PublicClassVar.Get_wwwSet.DataSetToByte(Game_PublicClassVar.Get_wwwSet.DataSetXml);
            MD5 md5 = MD5.Create();
            byte[] encryptionBytes = md5.ComputeHash(dateByte);
            string EncryptionStr = Convert.ToBase64String(encryptionBytes);
            Game_PublicClassVar.Get_wwwSet.DataSetXml_BiDui_1_michi = EncryptionStr;
            //Debug.Log("md5TestStr = " + EncryptionStr);
            //time = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
            //Debug.Log("time = " + time);

            //Debug.Log("缓存所有表成功！");



            //比对角色周边配置表（不加密的）
            DataSet dateSet_NoKey = new DataSet();
            DataTable dataTable_NoKey = new DataTable();
            dataTable_NoKey = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml_NoKey(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "GameConfig.xml", "GameConfig");
            dataTable_NoKey.PrimaryKey = new DataColumn[] { dataTable_NoKey.Columns["ID"] };       //设置主键
            //效验数据
            DataTable wwwTableNoKey = XmlTextToDataTable(Game_PublicClassVar.Get_wwwSet.WWWSet_GameConfig_1, "GameConfig",true);
            dataTable_NoKey = CompareDataTable(wwwTableNoKey, dataTable_NoKey);
            //Debug.Log("wwwTableNoKey = " + wwwTableNoKey + ";dataTable_NoKey = " + dataTable_NoKey);
            //Game_PublicClassVar.Get_xmlScript.Xml_SetXmlDate(dataTable.ToString(), Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "GameConfig.xml");
            dateSet_NoKey.Tables.Add(dataTable_NoKey);
            dateSet_NoKey.Tables["GameConfig"].WriteXml(Application.persistentDataPath + "/GameData/" + "Xml" + "/Set_Xml/" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName + "/" + "GameConfig.xml");

            //开启储备数据
            Game_PublicClassVar.Get_wwwSet.IfSaveRoseData = true;
            Game_PublicClassVar.Get_wwwSet.AddShowLog("开启备份数据状态...");

        }
        catch (Exception ex)
        {
            Debug.LogError("配置表错误：ex = " + ex);
            Game_PublicClassVar.Get_wwwSet.AddShowLog_NoKey("账号数据异常, 从备份中获取数据 = " + ex);
            Game_PublicClassVar.Get_wwwSet.IfSaveGetRoseData = true;
            Debug.Log("账号数据异常,从备份中获取数据 = " + ex);
            Time.timeScale = 0;
            GameObject beifenObj = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_BeiFenData);
            beifenObj.transform.SetParent(GameObject.Find("Canvas").transform);
            beifenObj.transform.localScale = new Vector3(1, 1, 1);
            beifenObj.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1, 1, 1);

        }
        //判定DataSet内的数据是否有错误
        if (Game_PublicClassVar.Get_wwwSet.DataSetXml.HasErrors){
            return false;
        }
        else {
            return true;
        }
    }

    //本地化读取
    public string LoadReadLanguage(string readKey,string readXml) {
        string readStr = "";
        bool ifAdd = false;

        if (Game_PublicClassVar.Get_wwwSet.GameSetLanguage._Language == "Chinese") {
            return readKey;
        }

        if (readXml == "Scene_Template") {
            switch (readKey)
            {
                case "SceneName":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "Npc_Template")
        {
            switch (readKey)
            {
                case "NpcName":
                    ifAdd = true;
                    break;
                case "SpeakText":
                    ifAdd = true;
                    break;
                case "NpcHeadSpeakText":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "HintProList_Template")
        {
            switch (readKey)
            {
                case "Name":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "Monster_Template")
        {
            switch (readKey)
            {
                case "MonsterName":
                    ifAdd = true;
                    break;
                case "Des":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "Spirit_Template")
        {
            switch (readKey)
            {
                case "Name":
                    ifAdd = true;
                    break;
                case "Des":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "Pet_Template")
        {
            switch (readKey)
            {
                case "PetName":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "Activity_Template")
        {
            switch (readKey)
            {
                case "Par_4":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "QianDao_Template")
        {
            switch (readKey)
            {
                case "ShowText":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "ChapterSon_Template")
        {
            switch (readKey)
            {
                case "ChapterSonName":
                    ifAdd = true;
                    break;
                case "ChapterSonNameOut":
                    ifAdd = true;
                    break;
            }
        }


        if (readXml == "EquipQiangHua_Template")
        {
            switch (readKey)
            {
                case "EquipSpaceName":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "Chapter_Template")
        {
            switch (readKey)
            {
                case "ChapterName":
                    ifAdd = true;
                    break;
                case "ChapterNameOut":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "ChengJiuReward_Template")
        {
            switch (readKey)
            {
                case "Name":
                    ifAdd = true;
                    break;
                case "Des":
                    ifAdd = true;
                    break;
            }
        }


        if (readXml == "ChengJiuAll_Template")
        {
            switch (readKey)
            {
                case "Name":
                    ifAdd = true;
                    break;
                case "Des":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "EquipSuitProperty_Template")
        {
            switch (readKey)
            {
                case "EquipSuitName":
                    ifAdd = true;
                    break;
                case "EquipSuitDes":
                    ifAdd = true;
                    break;
            }
        }


        if (readXml == "EquipSuit_Template")
        {
            switch (readKey)
            {
                case "Name":
                    ifAdd = true;
                    break;
            }
        }


        if (readXml == "ChengHao_Template")
        {
            switch (readKey)
            {
                case "Name":
                    ifAdd = true;
                    break;
                case "Des":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "SceneItem_Template")
        {
            switch (readKey)
            {
                case "Name":
                    ifAdd = true;
                    break;
            }
        }


        if (readXml == "SceneTanSuo_Template")
        {
            switch (readKey)
            {
                case "HintStr":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "Skill_Template")
        {
            switch (readKey)
            {
                case "SkillName":
                    ifAdd = true;
                    break;
                case "SkillDescribe":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "Talent_Template")
        {
            switch (readKey)
            {
                case "Name":
                    ifAdd = true;
                    break;
                case "talentDes":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "TaskCountry_Template")
        {
            switch (readKey)
            {
                case "TaskName":
                    ifAdd = true;
                    break;
                case "TaskDes":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "Item_Template")
        {
            switch (readKey)
            {
                case "ItemName":
                    ifAdd = true;
                    break;
                case "ItemDes":
                    ifAdd = true;
                    break;
                case "ItemBlackDes":
                    ifAdd = true;
                    break;
            }
        }


        if (readXml == "SkillBuff_Template")
        {
            switch (readKey)
            {
                case "BuffName":
                    ifAdd = true;
                    break;
                case "BuffDescribe":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "ShouJiItemPro_Template")
        {
            switch (readKey)
            {
                case "ChapterDes":
                    ifAdd = true;
                    break;
                case "ProList1_Des":
                    ifAdd = true;
                    break;
                case "ProList2_Des":
                    ifAdd = true;
                    break;
                case "ProList3_Des":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "ChengJiu_Template")
        {
            switch (readKey)
            {
                case "Name":
                    ifAdd = true;
                    break;
                case "Des":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "Task_Template")
        {
            switch (readKey)
            {
                case "TaskName":
                    ifAdd = true;
                    break;
                case "TaskDes":
                    ifAdd = true;
                    break;
            }
        }


        if (readXml == "EquipXiLianDaShi_Template")
        {
            switch (readKey)
            {
                case "Name":
                    ifAdd = true;
                    break;
                case "Des":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "SceneTransfer_Template")
        {
            switch (readKey)
            {
                case "TransferName":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "GameStory_Template")
        {
            switch (readKey)
            {
                case "StorySpeakChooseText":
                    ifAdd = true;
                    break;
                case "StorySpeakTextNpc_True":
                    ifAdd = true;
                    break;
                case "StorySpeakTextNpc_False":
                    ifAdd = true;
                    break;
                case "StorySpeakTextRose_True":
                    ifAdd = true;
                    break;
                case "StorySpeakTextRose_False":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "TaskMovePosition_Template")
        {
            switch (readKey)
            {
                case "MapName":
                    ifAdd = true;
                    break;
            }
        }

        if (readXml == "PetXiuLian_Template")
        {
            switch (readKey)
            {
                case "Des":
                    ifAdd = true;
                    break;
            }
        }

        //添加状态
        if (ifAdd) {
            readKey = readKey + "_EN";
        }

        return readKey;

    }

    private string GetReadData(string xmlName) {

        switch (xmlName) {
            case "RoseBag":
                return "2";
                break;
            case "RoseBuilding":
                return "2";
                break;
            case "RoseChengJiu":
                return "2";
                break;
            case "RoseConfig":
                return "2";
                break;
            case "RoseData":
                return "2";
                break;
            case "RoseDayReward":
                return "2";
                break;
            case "RoseEquip":
                return "2";
                break;
            case "RoseEquipHideProperty":
                return "2";
                break;
            case "RoseOtherData":
                return "2";
                break;
            case "RosePet":
                return "2";
                break;
            case "RoseStoreHouse":
                return "2";
                break;

            case "RoseStorehouse":
                return "2";
            case "RosePasture":
                return "2";
                break;
            case "RosePastureData":
                return "2";

            case "RosePastureBag":
                return "2";
                break;
        }

        //读取类返回1，存储类返回2
        return "1";

    }


    //读取对应的DataSet数据值
    public string DataSet_ReadData(string seclectKey, string primaryKey, string primaryValue, string xmlName,string getOcc = "")
    {
        try {
            //DataRow[] rows = wwwSet_p.DataSetXml.Tables[xmlName].Select(primaryKey + " = " + "'" + primaryValue + "'");
            //string seclectValue = rows[0][seclectKey].ToString();

            //加载本地化处理
            seclectKey = LoadReadLanguage(seclectKey, xmlName);
            if (GetReadData(xmlName) == "1")
            {
                DataRow rows = wwwSet_p.DataSetXml.Tables[xmlName].Rows.Find(primaryValue);
                string seclectValue = rows[seclectKey].ToString();
                seclectValue = ReturnOccStr(seclectValue, seclectKey, xmlName, getOcc);     //检测职业
                return seclectValue;
            }
            else {
                DataRow rows = wwwSet_p.DataWriteXml.Tables[xmlName].Rows.Find(primaryValue);
                string seclectValue = rows[seclectKey].ToString();
                seclectValue = ReturnOccStr(seclectValue, seclectKey, xmlName, getOcc);     //检测职业
                return seclectValue;
            }

        }catch(Exception ex){

            string logStr = "报错读取数据：" + seclectKey + "," + primaryKey + "," + primaryValue + "," + xmlName;

            //Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr = logStr + "\n" + Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr;

            Game_PublicClassVar.Get_wwwSet.AddShowLog(logStr + " ex = " + ex);

            return "0";
        }

    }

    public bool DataSet_WriteData(string writeKey, string writeValue, string primaryKey, string primaryValue, string xmlName)
    {
        //Debug.Log("写入值" + xmlName + "writeValue = " + writeValue  + "writeKey = " + writeKey);
        try
        {
            /*
            DataRow[] rows = Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables[xmlName].Select(primaryKey + " = " + "'" + primaryValue + "'");
            DataRow row = rows[0];
            row[writeKey] = writeValue;
            */

#if !UNITY_EDITOR
            if (Game_PublicClassVar.Get_wwwSet.forceExitGameStatus) {
                return false;
            }
#endif

            if (writeValue == null) {
                writeValue = "";
            }
            DataRow rows = Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables[xmlName].Rows.Find(primaryValue);
            rows[writeKey] = writeValue;

            
            //Debug.Log("写入值" + xmlName + "writeValue = " + writeValue + "writeKey = " + writeKey);
            if (Game_PublicClassVar.Get_wwwSet.DataWriteXml_BiDui_1_Status) {
                //Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("写入值" + xmlName + "writeValue = " + writeValue + "writeKey = " + writeKey);
                Game_PublicClassVar.Get_wwwSet.DataWriteSave();
            }
            

            //Game_PublicClassVar.Get_wwwSet.DataWriteSave();

            return true;
        }catch (Exception ex)
        {
            string logStr = "报错写入数据：" + writeKey + "," + writeValue + "," + primaryKey + "," + primaryValue + "," + xmlName;
            //Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr = logStr + "\n" + Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr;
            Game_PublicClassVar.Get_wwwSet.AddShowLog(logStr + " ex = " +ex);
           return false;
        }
    }
    //新增装备极品属性
    public void AddRoseEquipHidePropertyXml(string addID, string addProperty)
    {

        //新建行数据
        DataTable dataTable = Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables["RoseEquipHideProperty"];
        DataRow row = dataTable.NewRow();
        //设置数据
        row["ID"] = addID;
        row["PrepeotyList"] = addProperty;
        //存储数据
        Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables["RoseEquipHideProperty"].Rows.Add(row);
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquipHideProperty");
    }

    //新增随机商店属性
    public void AddRandomStoreXml(string addID, string addProperty)
    {

        //Debug.Log("addProperty = " + addProperty);

        //新建行数据
        DataTable dataTable = Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables["RoseOtherData"];
        DataRow row = dataTable.NewRow();

        //设置数据
        row["ID"] = "Store_" + addID;
        row["Value_1"] = addProperty;
        //Debug.Log("Game_PublicClassVar.Get_wwwSet.enterGameTimeStamp = " + Game_PublicClassVar.Get_wwwSet.enterGameTimeStamp + ";Time.realtimeSinceStartup = " + Time.realtimeSinceStartup);
        int timeTmp = 0;
        if (Game_PublicClassVar.Get_wwwSet.enterGameTimeStamp != "")
        {
            timeTmp = int.Parse(Game_PublicClassVar.Get_wwwSet.enterGameTimeStamp) + (int)(Time.realtimeSinceStartup);
        }

        row["Value_2"] = timeTmp.ToString();
        
        //存储数据
        Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables["RoseOtherData"].Rows.Add(row);
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseOtherData");
    }


    //新增随机商店属性
    public void UpdateRandomStoreXml(string addID, string addProperty)
    {

        //新建行数据
        DataTable dataTable = Game_PublicClassVar.Get_wwwSet.DataWriteXml.Tables["RoseOtherData"];
        DataRow[] rows = dataTable.Select("ID" + " = " + "'" + "Store_" + addID + "'");
        DataRow row = rows[0];

        row["Value_1"] = addProperty;

		string timeStr = Game_PublicClassVar.Get_wwwSet.enterGameTimeStamp;
		//连接不上服务器,显示本地时间戳
		if (timeStr == "") {
			timeStr = Game_PublicClassVar.Get_function_DataSet.GetTimeStamp ();
			Debug.Log ("timeStr = " + timeStr);
		}

		int timeTmp = int.Parse(timeStr) + (int)(Time.realtimeSinceStartup);
        row["Value_2"] = timeTmp.ToString();

        //存储数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseOtherData");
    }


    //文本变成DataTable
    private DataTable XmlTextToDataTable(WWW www, string tableName,bool NoKeyXml = false) {

        string xmlText = www.text;
        //如果为加密则先解密
        if (Game_PublicClassVar.Get_wwwSet.IfAddKey&& NoKeyXml==false) {
            //Debug.Log("解锁名称:" + tableName);
            xmlText = Game_PublicClassVar.Get_xmlScript.ReadCostKey(www);
        }

        DataSet dateSetTest = new DataSet();
        StringReader StrStream = new StringReader(xmlText);
        XmlTextReader Xmlrdr = new XmlTextReader(StrStream);
        dateSetTest.ReadXml(Xmlrdr);
        DataTable dataTableTest = new DataTable(tableName);
        dataTableTest = dateSetTest.Tables[0].Copy();
        dataTableTest.TableName = tableName;

        return dataTableTest;

    }


    //核对2个Data数据
    public DataTable CompareDataTable(DataTable data_1, DataTable data_2) {

        //循环比较1有2没有的字段
        for (int i = 0; i < data_1.Columns.Count; i++) {
            string ziduanName_1 = data_1.Columns[i].ColumnName;
            bool ifHaveStatus = false;
            for (int y = 0; y<data_2.Columns.Count;y++) {
                string ziduanName_2 = data_2.Columns[y].ColumnName;
                if (ziduanName_1 == ziduanName_2) {
                    ifHaveStatus = true;
                    break;
                }
            }

            if (!ifHaveStatus)
            {
                //Debug.Log("对比发现缺少字段:" + ziduanName_1);

                //获取默认值
                string ziduanName_1_Value = data_1.Rows[0][ziduanName_1].ToString();
                //Debug.Log("ziduanName_1_Value = " + ziduanName_1_Value);

                //添加列
                if (ziduanName_1_Value == null) {
                    ziduanName_1_Value = "";
                }
                DataColumn dc1 = new DataColumn(ziduanName_1, typeof(string));
                dc1.DefaultValue = ziduanName_1_Value;
                dc1.AllowDBNull = false;
                dc1.ColumnMapping = MappingType.Attribute;      //设置生成XML的类型
                data_2.Columns.Add(dc1);
                
                for (int z = 0; z < data_2.Rows.Count; z++) {
                    DataRow nowRow = data_2.Rows[z];
                    //nowRow.BeginEdit();
                    nowRow[ziduanName_1] = ziduanName_1_Value;
                    //nowRow.EndEdit();
                }

                if (ziduanName_1 == "YanZhengFileStr")
                {
                    Game_PublicClassVar.Get_wwwSet.UpdateXmlNoYanZhengFile = true;
                    //Debug.Log("进入不效验文件模式");
                }
            }
        }

        //判断是否要增加列
        if (data_1.Rows.Count > data_2.Rows.Count) {
            for (int i = data_2.Rows.Count; i < data_1.Rows.Count; i++) {
                DataRow nowRow = data_1.Rows[i];
                data_2.ImportRow(nowRow);
            }
        }

        return data_2;
    }

	//获取本地时间戳
	public string GetTimeStamp()
	{
		TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return Convert.ToInt64(ts.TotalSeconds).ToString();
	}

    //检测职业相关显示字段
    private string ReturnOccStr(string valueStr, string selectName,string XmlName,string getOcc="") {

        if (XmlName == "Item_Template") {

            bool ifReturn = false;

            if (selectName == "ItemName"|| selectName == "ItemIcon"|| selectName == "ItemMondel") {
                ifReturn = true;
            }

            if (ifReturn) {
                string[] valueList = valueStr.Split(';');
                if (valueList.Length >= 2) {

                    //获取当前职业
                    string occ = Game_PublicClassVar.Get_function_Rose.GetRoseOcc();
                    if (getOcc != "" && getOcc != null) {
                        occ = getOcc;
                    }

                    switch (occ) {
                        case "1":
                            if (valueList.Length >= 1) {
                                return valueList[0];
                            }
                            break;

                        case "2":
                            if (valueList.Length >= 2)
                            {
                                return valueList[1];
                            }
                            break;

                        case "3":
                            if (valueList.Length >= 3)
                            {
                                return valueList[2];
                            }
                            break;
                    }
                }
            }
        }

        return valueStr;
    }

}
