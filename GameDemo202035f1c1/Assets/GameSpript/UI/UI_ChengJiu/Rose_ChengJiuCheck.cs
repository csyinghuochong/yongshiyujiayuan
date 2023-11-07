using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weijing;

public class Rose_ChengJiuCheck  : Singleton<Rose_ChengJiuCheck>
{

    public Dictionary<string, string> ChengJiuNextId = new Dictionary<string, string>();

    //chengJiuSonType="1;2;3;4;5;6;105;228;109;204;101"
    public string ChengJiu_JianCeTargetSonTypeChengJiuID(string chengJiuSonType, bool ifHint = false)
    {
        if (chengJiuSonType == "" || chengJiuSonType == null)
            return "";

        //读取当前已完成的成就ID
        string comChengJiuID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ComChengJiuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
        List<string> comChengJiuIDLis = new List<string>(comChengJiuID.Split(';'));

        //获取所有未完成的成就，在未完成的成就中检测达到完成条件的
        //string chengJiuStr = Game_PublicClassVar.Get_function_Task.ChengJiu_GetTargetZiDuanChengJiuID(chengJiuTypeID, ziDuanName);

        string comChengJiuSetStr = "";
        List<string> chengJiuSonTypeList = new List<string>(chengJiuSonType.Split(';'));
        string chengJiuSetStr = ChengJiu_GetAllChengJiuID(comChengJiuIDLis);
        string[] chengjiuSetList = chengJiuSetStr.Split(';');
        for (int i = 0; i < chengjiuSetList.Length; i++)
        {
            //判断成就打到完成条件没写入完成列表的
            if (Game_PublicClassVar.Get_function_Task.ChengJiu_IfComChengJiu( chengjiuSetList[i] ) )
            {
                comChengJiuSetStr = comChengJiuSetStr + chengjiuSetList[i] + ";";
            }
        }
        if (comChengJiuSetStr.Length > 0)
            comChengJiuSetStr = comChengJiuSetStr.Substring(0, comChengJiuSetStr.Length - 1);
        return comChengJiuSetStr;
    }

    public string ChengJiu_GetAllChengJiuID(List<string> comChengJiuIDList)
    {
        string chengJiuSetStr = "";
        bool getStatus = false;
        if (Game_PublicClassVar.Get_wwwSet.RoseChengJiuSetID == "" || Game_PublicClassVar.Get_wwwSet.RoseChengJiuSetID == null)
        {
            getStatus = true;
        }
        else
        {
            chengJiuSetStr = Game_PublicClassVar.Get_wwwSet.RoseChengJiuSetID;
        }

        List<string> checkType = new List<string> { "1002", "1003", "1004" };
        for (int i = 0; i < checkType.Count; i++)
        {
            string chengjiuIds = ChengJiu_GetTargerTypeChengJiuIDSet(checkType[i], comChengJiuIDList);
            if (chengjiuIds == "")
            {
                continue;
            }
            else
            {
                chengJiuSetStr = chengJiuSetStr + chengjiuIds +";";
            }
        }
        if (chengJiuSetStr != "")
            chengJiuSetStr = chengJiuSetStr.Substring(0, chengJiuSetStr.Length - 1);
        return chengJiuSetStr;
    }

    public string ChengJiu_GetTargerTypeChengJiuIDSet(string chengJiuType, List<string> comChengJiuIDList)
    {
        List<string> checkZiduan = new List<string> {
            "ChengJiuSet_Com", "ChengJiuSet_ZhangJie_1", "ChengJiuSet_ZhangJie_2", "ChengJiuSet_ZhangJie_3", "ChengJiuSet_ZhangJie_4", "ChengJiuSet_ZhangJie_5" };

        string chengJiuSetStr = "";
        for ( int i = 0; i < checkZiduan.Count; i++ )
        {
            string targetIds = ChengJiu_GetTargetZiDuanChengJiuID(chengJiuType, checkZiduan[i], comChengJiuIDList);
            if (targetIds == "")
            {
                continue;
            }
            else
            {
                chengJiuSetStr = chengJiuSetStr + targetIds + ";";
            } 
        }
        if (chengJiuSetStr != "")
            chengJiuSetStr = chengJiuSetStr.Substring(0, chengJiuSetStr.Length - 1);
        return chengJiuSetStr;
    }

    public string ChengJiu_GetTargetZiDuanChengJiuID(string chengJiuTypeID, string chengJiuZiDuan, List<string> comChengJiuIDList)
    {

        //chengJiuTypeID-1002 chengJiuZiDuan-ChengJiuSet_ZhangJie_1.改类型具体的章节所有的头条成就
        string chengJiuSetID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(chengJiuZiDuan, "ID", chengJiuTypeID, "ChengJiuAll_Template");
        string[] chengJiuSetIDList = chengJiuSetID.Split(';');

        //成就集合
        string returnChengJiuIDStr = "";
        string separationCharacter = ";";
        //把每一条成就线未完成的下一个成取出来
        for ( int i = 0; i < chengJiuSetIDList.Length; i++ )
        {
            string jianceChengJiuID = chengJiuSetIDList[i];

            int sum = 0;
            while (true)
            {
                //读取当前任务是否完成
                if (comChengJiuIDList.Contains(jianceChengJiuID))
                {
                    //获取下一级任务

                    if (ChengJiuNextId.ContainsKey(jianceChengJiuID))
                    {
                        jianceChengJiuID = ChengJiuNextId[jianceChengJiuID];
                    }
                    else
                    {
                        jianceChengJiuID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", jianceChengJiuID, "ChengJiu_Template");
                        ChengJiuNextId[chengJiuSetIDList[i]] = jianceChengJiuID;
                    }
                    if (jianceChengJiuID == "" || jianceChengJiuID == "0")
                    {
                        //没有下一级任务
                        break;
                    }
                }
                else
                {
                    //此任务未完成
                    returnChengJiuIDStr = returnChengJiuIDStr + jianceChengJiuID + separationCharacter;
                    break;
                }
                //防止死循环
                sum = sum + 1;
                if (sum >= 1000)
                {
                    break;
                }
            }
        }
        if (returnChengJiuIDStr != "")
            returnChengJiuIDStr = returnChengJiuIDStr.Substring(0, returnChengJiuIDStr.Length - 1);
        return returnChengJiuIDStr;
    }
}
