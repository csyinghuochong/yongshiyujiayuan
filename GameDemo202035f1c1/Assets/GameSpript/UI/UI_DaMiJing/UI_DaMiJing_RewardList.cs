using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_DaMiJing_RewardList : MonoBehaviour {

    public ObscuredString RewardLv;
    public ObscuredString LingQuRewardStr;      //奖励字符
    public GameObject Obj_Ceng;
    public GameObject Obj_RewardList;
    public GameObject Obj_RewardListSet;
    public GameObject Obj_YiLingQu;
    public GameObject Obj_LingQuBtn;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Init()
    {

        //显示奖励
        Obj_Ceng.GetComponent<Text>().text = "第" + RewardLv + "层";
        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_RewardListSet);
        //创建
        string[] rewardStrList = LingQuRewardStr.ToString().Split(';');
        for (int i = 0; i < rewardStrList.Length; i++)
        {
            GameObject obj = (GameObject)Instantiate(Obj_RewardList);
            obj.transform.SetParent(Obj_RewardListSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            string[] itemList = rewardStrList[i].Split(',');
            if (itemList.Length >= 2)
            {
                obj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                obj.GetComponent<UI_Common_ItemIcon>().ItemID = itemList[0];
                obj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = int.Parse(itemList[1]);
            }
        }

        //检测自身是否领取
        //string daMiJingLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string daMiJingRewardLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingRewardLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (RewardLv != "" && daMiJingRewardLv != "")
        {
            int daMiJingLv_int = int.Parse(RewardLv);
            int daMiJingRewardLv_int = int.Parse(daMiJingRewardLv);

            if (daMiJingLv_int > daMiJingRewardLv_int)
            {
                Obj_YiLingQu.SetActive(false);
                Obj_LingQuBtn.SetActive(true);
            }
            else
            {
                Obj_YiLingQu.SetActive(true);
                Obj_LingQuBtn.SetActive(false);
            }
        }
    }

    //领取
    /*
    public void Btn_LingQu()
    {

        if (RewardLv == "" || RewardLv == "0" || RewardLv == null)
        {
            return;
        }

        //检测背包数据
        string[] rewardStrList = LingQuRewardStr.Split(';');
        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() < rewardStrList.Length)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请背包预留" + rewardStrList.Length + "个位置");
            return;
        }

        //检测自身是否领取
        string daMiJingLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string daMiJingRewardLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingRewardLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (daMiJingLv != "" && daMiJingRewardLv != "")
        {
            int daMiJingLv_int = int.Parse(daMiJingLv);
            int daMiJingRewardLv_int = int.Parse(daMiJingRewardLv);

            if (daMiJingLv_int > daMiJingRewardLv_int)
            {
                //存储数据
                //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HuoDong_ShouLie_Reward", rewardStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                //发送奖励
                Game_PublicClassVar.Get_function_Rose.Rose_SendRewardStr(LingQuRewardStr);
                //刷新显示
                Init();
            }
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未达到指定击败次数");
        }

    }
    */
}
