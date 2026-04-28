using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ZhengYingFuLi : MonoBehaviour
{
    public GameObject Obj_UIPar;
    public GameObject Obj_ZhenYingShowText;
    public GameObject Obj_ZhenYingZhiWeiShowText;

    public GameObject Obj_ZhenYingReward_1;
    public GameObject Obj_ZhenYingReward_2;
    public GameObject Obj_ZhenYingReward_Sum;

    private ObscuredString SendIDStr;
    private ObscuredInt SendNum_Win;
    private ObscuredInt SendNum_Fail;
    private ObscuredInt SendRewadNum;
    private ObscuredInt SendRewardNumSum;
    private ObscuredInt SendZhiWeiRewardNum;
    private ObscuredBool InitStatus;
    // Start is called before the first frame update
    void Start()
    {
        SendIDStr = "10000057";
        SendNum_Win = 10;
        SendNum_Fail = 5;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //初始化
    private void Init()
    {

        string zhenying = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhenYing", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        if (zhenying == "" || zhenying == "0" || zhenying == null)
        {
            Obj_ZhenYingShowText.GetComponent<Text>().text = "未选择阵营";
        }

        if (zhenying == "1")
        {
            Obj_ZhenYingShowText.GetComponent<Text>().text = "正派";
        }

        if (zhenying == "2")
        {
            Obj_ZhenYingShowText.GetComponent<Text>().text = "邪派";
        }

        string zhenyingName = Game_PublicClassVar.Get_function_UI.ReturnZhiWeiName(Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().SelfGuanZhi);

        if (zhenyingName != "" && zhenyingName != "0" && zhenyingName != null)
        {
            Obj_ZhenYingZhiWeiShowText.GetComponent<Text>().text = zhenyingName;
        }

        //刷新主角界面显示
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().UpdateZhenYing();

        //显示货币
        if (Obj_UIPar.GetComponent<UI_ZhenYingSet>().ProZhenYingDataList.ZhenYingNum_Zheng >= Obj_UIPar.GetComponent<UI_ZhenYingSet>().ProZhenYingDataList.ZhenYingNum_Xie)
        {
            if (zhenying == "1")
            {
                //发送胜利方奖励
                Obj_ZhenYingReward_1.GetComponent<Text>().text = "胜者奖励:" + SendNum_Win;
                SendRewadNum = SendNum_Win;
            }
            else
            {
                //发送失败方奖励
                Obj_ZhenYingReward_1.GetComponent<Text>().text = "负者奖励:" + SendNum_Fail;
                SendRewadNum = SendNum_Fail;
            }
        }
        else
        {

            if (zhenying == "2")
            {
                //发送胜利方奖励
                Obj_ZhenYingReward_1.GetComponent<Text>().text = "胜者奖励:" + SendNum_Win;
                SendRewadNum = SendNum_Win;
            }
            else
            {
                //发送失败方奖励
                Obj_ZhenYingReward_1.GetComponent<Text>().text = "负者奖励:" + SendNum_Fail;
                SendRewadNum = SendNum_Fail;
            }
        }

        if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet != null) { 

            SendZhiWeiRewardNum = Game_PublicClassVar.Get_function_UI.ReturnZhiWeiRewardNum(Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().SelfGuanZhi);
        }
        Obj_ZhenYingReward_2.GetComponent<Text>().text = "职位奖励:" + SendZhiWeiRewardNum;

        SendRewardNumSum = SendZhiWeiRewardNum + SendRewadNum;

        Obj_ZhenYingReward_Sum.GetComponent<Text>().text = "累计领取:" + SendRewardNumSum.ToString();

        InitStatus = true;
    }


    //发送
    public void Btn_SendReward() {

        Debug.Log("点击了领取按钮...");

        if (InitStatus == false) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("初始化未完成...");
            return;
        }

        string zhenying = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhenYing", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        if (zhenying == "" || zhenying == "0" || zhenying == null)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先选择阵营!");
            return;
        }

        string ZhengYingEveryRewardFuLiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhengYingEveryRewardFuLi", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (ZhengYingEveryRewardFuLiStr != "1")
        {
            //记录
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhengYingEveryRewardFuLi", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

            //获取阵营的胜利方还是失败方
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(SendIDStr, SendRewardNumSum);

            /*
            if (Obj_UIPar.GetComponent<UI_ZhenYingSet>().ProZhenYingDataList.ZhenYingNum_Zheng >= Obj_UIPar.GetComponent<UI_ZhenYingSet>().ProZhenYingDataList.ZhenYingNum_Xie)
            {
                if (zhenying == "1")
                {
                    //发送胜利方奖励
                    //发送奖励
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(SendIDStr, SendNum_Win);
                }
                else
                {
                    //发送失败方奖励
                    //发送奖励
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(SendIDStr, SendNum_Fail);
                }
            }
            else
            {

                if (zhenying == "2")
                {
                    //发送胜利方奖励
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(SendIDStr, SendNum_Win);
                }
                else
                {
                    //发送失败方奖励
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(SendIDStr, SendNum_Fail);
                }
            }
            */
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日已领取奖励,请明日再来!");
        }
    }
}
