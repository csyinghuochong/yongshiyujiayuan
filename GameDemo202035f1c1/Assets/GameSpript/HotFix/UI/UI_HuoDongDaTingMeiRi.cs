using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_HuoDongDaTingMeiRi : MonoBehaviour
{

    public int DengLuRewardDay;
    public string DengLuRewardStr;
    public GameObject ItemParentSet;
    public GameObject ItemObj;
    public GameObject DengLuRewardBtn;
    //public GameObject DengLuRewardDayObj;
    public bool ifItemIconHui;
    public GameObject Obj_LabYiLingQu;
    private string lingQuStr;
    private string[] lingQuList;

	// Use this for initialization
	void Start () {
        updataDengLuReward();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //更新登陆奖励
    public void updataDengLuReward() {

        /*
        DengLuRewardDayObj.GetComponent<Text>().text = "第" + DengLuRewardDay + "天";
        //获取当前领取状态
        string dengLuReward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DengLuReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (int.Parse(dengLuReward) > int.Parse(DengLuRewardDay))
        {
            //奖励已经领取隐藏领取按钮
            DengLuRewardBtn.SetActive(false);
            ifItemIconHui = true;
            Obj_LabYiLingQu.SetActive(true);
        }
        */
        //初始化领取状态
        lingQuStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_7", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (lingQuStr == "0" || lingQuStr == "") {
            lingQuStr = "0;0;0;0;0;0;0";
        }

        lingQuList = lingQuStr.Split(';');

        //显示领取奖励
        DengLuRewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "EveryDayReward_" + DengLuRewardDay, "GameMainValue");
        string[] dengLuRewardItemStr = DengLuRewardStr.Split(';');

        for (int i = 0; i <= dengLuRewardItemStr.Length - 1; i++) {
            string[] dengLuRewardItemValue = dengLuRewardItemStr[i].Split(',');
            GameObject itemObj = (GameObject)Instantiate(ItemObj);
            itemObj.transform.SetParent(ItemParentSet.transform);
            itemObj.transform.localPosition = new Vector3(100 * i + 100, 0, 0);
            itemObj.transform.localScale = new Vector3(1, 1, 1);
            itemObj.GetComponent<UI_ChouKaItemObj>().ItemID = dengLuRewardItemValue[0];
            itemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = dengLuRewardItemValue[1];
            if (ifItemIconHui) {
                //变灰
                object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
                Material huiMaterial = huiObj as Material;
                itemObj.GetComponent<UI_ChouKaItemObj>().Obj_ItemIcon.GetComponent<Image>().material = huiMaterial;
            }
        }

        //显示按钮
        //Debug.Log("lingQuList[DengLuRewardDay] = " + lingQuList[DengLuRewardDay]);
        if (lingQuList[DengLuRewardDay] != "0")
        {
            //奖励已经领取隐藏领取按钮
            DengLuRewardBtn.SetActive(false);
            Obj_LabYiLingQu.SetActive(true);
        }
    }

    //点击领取奖励
    public void Btn_MeiRiReward()
    {

        //初始化领取状态
        lingQuStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_7", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (lingQuStr == "0" || lingQuStr == "")
        {
            lingQuStr = "0;0;0;0;0;0;0";
        }

        lingQuList = lingQuStr.Split(';');

        if (lingQuList[DengLuRewardDay] != "0")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_362");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("奖励已领取");
            return;
        }

        //判定当前等级是否大于12级
        if (DengLuRewardDay == 0) {
            int roseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            if (roseLv < 12)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_363");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameHint("请将等级提升至12级后领取!");
                return;
            }
        }


        //读取今天付费额度
        float nowDayRmbValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_8", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig"));
        float needDayRmbValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "EveryDayRMB_" + DengLuRewardDay, "GameMainValue"));
        if (nowDayRmbValue < needDayRmbValue)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_364");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("赞助额度不足请稍后再试!");
            return;
        }

        //读取奖励
        //string dengLuReward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EveryDayReward_" + DengLuRewardDay, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

        /*
        if (dengLuReward != DengLuRewardDay) {
            Game_PublicClassVar.Get_function_UI.GameHint("请按照登陆奖励依次领取!");
            return;
        }

        //获取领取状态
        string dengLuDayStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DengLuDayStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (dengLuDayStatus == "1")
        {
            Game_PublicClassVar.Get_function_UI.GameHint("今日登陆奖励已领取!");
            return;
        }
        */
        //获取领取数据
        string[] dengLuRewardValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "EveryDayReward_" + DengLuRewardDay, "GameMainValue").Split(';');
        //检测背包位置是否足够
        int spaceNullNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
        if (dengLuRewardValue.Length - 1 > spaceNullNum)
        {
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_152");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_153");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + (dengLuRewardValue.Length - 1).ToString() + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameHint("请预留" + (dengLuRewardValue.Length - 1).ToString() + "个背包空位置！");
            return;
        }

        //发送奖励
        for (int i = 0; i <= dengLuRewardValue.Length - 1; i++)
        {
            string[] rewardStr = dengLuRewardValue[i].Split(',');
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardStr[0], int.Parse(rewardStr[1]),"0",0,"0",true,"5");
        }

        //奖励已经领取隐藏领取按钮
        DengLuRewardBtn.SetActive(false);
        Obj_LabYiLingQu.SetActive(true);

        //设置今日领取状态
        lingQuList[DengLuRewardDay] = "1";
        for (int i = 0; i < lingQuList.Length; i++) {
            if (i == 0)
            {
                lingQuStr = lingQuList[i];
            }
            else {
                lingQuStr = lingQuStr + ";" + lingQuList[i];
            }
        }

        //Debug.Log("lingQuStr = " + lingQuStr);
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_7", lingQuStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

        //设置登陆领取
        //dengLuReward = (int.Parse(dengLuReward) + 1).ToString();
        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DengLuReward", dengLuReward, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
    }

}
