using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_ZhenYingShouLieList : MonoBehaviour
{
    public ObscuredString ShouLieRewardID;
    public ObscuredInt ShouLieNum;
    public ObscuredInt ShouLieRewardNum;
    public GameObject Obj_ShouLieNumDes;
    public GameObject Obj_ShouLieRewardNumDes;
    public GameObject Obj_Btn_LingQu;
    public GameObject Obj_Img_LingQu;
    private ObscuredString SendIDStr;

    // Start is called before the first frame update
    void Start()
    {
        SendIDStr = "10000057";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //初始化
    public void Init() {

        Obj_ShouLieNumDes.GetComponent<Text>().text = "今日狩猎任意怪物数量达到" + ShouLieNum + "只";
        Obj_ShouLieRewardNumDes.GetComponent<Text>().text = ShouLieRewardNum.ToString();

        //显示是否已经领取
        string ZhengYingRewardFuLiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhengYingRewardFuLi", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (ZhengYingRewardFuLiStr.Contains(ShouLieRewardID))
        {
            Obj_Btn_LingQu.SetActive(false);
            Obj_Img_LingQu.SetActive(true);
        }
        else {
            Obj_Btn_LingQu.SetActive(true);
            Obj_Img_LingQu.SetActive(false);
        }

    }

    //领取奖励
    public void Btn_LingQu() {

        ObscuredInt nowLingQuNum = Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum;      //获取击杀怪物次数
        if (nowLingQuNum >= ShouLieNum)
        {
            //检测背包是否有1个位置的空位
            if (!Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1))
            {

                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_301");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                return;
            }

            string ZhengYingRewardFuLiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhengYingRewardFuLi", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            if (ZhengYingRewardFuLiStr.Contains(ShouLieRewardID) == false)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("领取奖励成功!");
                ZhengYingRewardFuLiStr = ZhengYingRewardFuLiStr + ";" + ShouLieRewardID;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhengYingRewardFuLi", ZhengYingRewardFuLiStr,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                //发送奖励
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(SendIDStr, ShouLieRewardNum);
            }
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未达到领取条件!");
        }

        //刷新显示
        Init();

    }
}
