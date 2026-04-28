using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_HuoDongRewardLvList : MonoBehaviour
{

    public int DengLuRewardDay;
    public string DengLuRewardStr;
    public int NeedValue;
    public string RewardSonType;

    public int RewardNum_Now;
    public int RewardNum_Sum;

    public GameObject Obj_RewardNumStr;

    public GameObject ItemParentSet;
    public GameObject ItemObj;
    public GameObject DengLuRewardBtn;
    //public GameObject DengLuRewardDayObj;
    public bool ifItemIconHui;
    public GameObject Obj_LabYiLingQu;
    private string lingQuStr;
    private string[] lingQuList;
    public GameObject Obj_LabMan;
    public GameObject Obj_TitleLab;

    // Use this for initialization
    void Start () {
        updataDengLuReward();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    //更新登陆奖励
    public void updataDengLuReward() {

        //显示标题
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("需要等级达到");
        Obj_TitleLab.GetComponent<Text>().text = langStr + ":" + NeedValue;

        //清除显示
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(ItemParentSet);

        //显示领取奖励
        //DengLuRewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "EveryDayReward_" + DengLuRewardDay, "GameMainValue");
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

    }

    //点击领取奖励
    public void Btn_RewardLv()
    {
        //判定当前等级是否大于25级
        int roseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        if (roseLv < 20)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_355");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("请将等级提升至指定等级后领取!");
            return;
        }

        //判定人数
        if (RewardNum_Now >= RewardNum_Sum) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_356");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("当前领取人数已满!");
            return;
        }

        //判定是否连接服务器
        //判断是否连接网络
        if (!Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_346");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("服务器未连接!");
            return;
        }

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

        /*
        //发送奖励
        for (int i = 0; i <= dengLuRewardValue.Length - 1; i++)
        {
            string[] rewardStr = dengLuRewardValue[i].Split(',');
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardStr[0], int.Parse(rewardStr[1]));
        }
        */

        //奖励已经领取隐藏领取按钮
        //DengLuRewardBtn.SetActive(false);
        //Obj_LabYiLingQu.SetActive(true);

        //发送请求
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string getRoseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv().ToString();
        Pro_ComStr_4 pro_ComStr_4 = new Pro_ComStr_4() { str_1= zhanghaoID, str_2 = "1",str_3 = RewardSonType, str_4 = getRoseLv};
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001061, pro_ComStr_4);


        //更新奖励
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001060, "1");

    }

}
