using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_BaoLvShowSet : MonoBehaviour {


    public ObscuredInt BaoLvID;
    public ObscuredInt BaoLvIDLast;
    public ObscuredBool UpdateStatus;
    public GameObject Obj_BaoLvListObj;
    public GameObject Obj_BaoLvListObjPar;
    public GameObject[] BaoLvList;
    public GameObject Obj_BaoLvShowText;
    public GameObject Obj_CostZuanShi;
    public GameObject Obj_CostGold;

    // Use this for initialization
    void Start() {

        BaoLvID = Game_PublicClassVar.Get_wwwSet.BaoLvID;

        if (BaoLvID == 0) {
            BaoLvID = 1;
        }

        //初始化显示
        BaoLvList = new GameObject[8];
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_BaoLvListObjPar);
        for (int i = 1; i <= 8; i++) {
            GameObject obj = (GameObject)Instantiate(Obj_BaoLvListObj);
            obj.transform.SetParent(Obj_BaoLvListObjPar.transform);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_BaoLvListShow>().BaoLvID = i;
            obj.GetComponent<UI_BaoLvListShow>().UI_ShowParObj = this.gameObject;
            obj.GetComponent<UI_BaoLvListShow>().UpdateShow();
            BaoLvList[i - 1] = obj;

            //显示信息
            obj.GetComponent<UI_BaoLvListShow>().Obj_BaoLvName.GetComponent<Text>().text = Game_PublicClassVar.Get_function_Rose.GetBaoLvName(i);
            string lang_str_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("倍爆率");
            obj.GetComponent<UI_BaoLvListShow>().Obj_BaoLvProValue.GetComponent<Text>().text = "(" + Game_PublicClassVar.Get_function_Rose.GetBaoLvProValue(i).ToString("0.0") + lang_str_2 + ")";
        }

        BaoLvIDLast = BaoLvID;

        showBaoLvShow();

        ShowZuanShi();
    }

    private void ShowZuanShi() {

        //显示钻石
        int roseZuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        Obj_CostZuanShi.GetComponent<Text>().text = roseZuanShi + "/" + 120;
        if (roseZuanShi < 120) {
            Obj_CostZuanShi.GetComponent<Text>().color = Color.red;
        }
        
        //显示金币
        long roseMoney = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
        int costMoney = getBaoLvGold();

        string showRoseMoneyStr = roseMoney.ToString();
        if (roseMoney >= 3000000) {
            string lang_str_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("万");
            showRoseMoneyStr = ((int)(roseMoney / 10000)).ToString() + lang_str_2;
        }

        Obj_CostGold.GetComponent<Text>().text = showRoseMoneyStr + "/" + ((int)(costMoney/10000)).ToString()+"万";
        if (roseMoney < costMoney)
        {
            Obj_CostGold.GetComponent<Text>().color = Color.red;
        }

    }

    // Update is called once per frame
    void Update() {

        //根据等级消耗不同的金币


        //更新显示
        if (UpdateStatus) {
            UpdateStatus = false;
            for (int i = 0; i < BaoLvList.Length; i++)
            {
                BaoLvList[i].GetComponent<UI_BaoLvListShow>().UpdateShow();
            }
        }
    }


    //消耗金币
    public void Btn_CostGold()
    {
        if (BaoLvID >= 8) {
            return;
        }

        int costMoney = getBaoLvGold();
        if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() >= costMoney)
        {
            Game_PublicClassVar.Get_function_Rose.CostReward("1",costMoney.ToString());
            Btn_UpdateBaoLv("1");
        }
        else
        {
            string str = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_138");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(str);
        }
    }


    //消耗钻石
    public void Btn_CostZuanShi() {

        if (BaoLvID >= 8)
        {
            return;
        }

        if (Game_PublicClassVar.Get_function_Rose.GetRoseRMB() >= 180)
        {
            Game_PublicClassVar.Get_function_Rose.CostRMB(180);
            Btn_UpdateBaoLv("2");
        }
        else {
            string str = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_86");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(str);
        }

    }

    //检测是否达到上限

    //刷新爆率
    public void Btn_UpdateBaoLv(string upType = "1") {


        //获取当前爆率
        int randomInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 100);
        randomInt = (int)(randomInt * 0.5f);             //整体概率降低50%
        Debug.Log("randomInt = " + randomInt);

        if (randomInt >= 1 && randomInt <= 50)
        {
            //设置爆率
            BaoLvID = 2;
        }

        //获取当前爆率
        if (randomInt >= 51 && randomInt <= 75)
        {
            //设置爆率
            BaoLvID = 3;
        }

        //获取当前爆率
        if (randomInt >= 76 && randomInt <= 86)
        {
            //设置爆率
            BaoLvID = 4;
        }

        //获取当前爆率
        if (randomInt >= 87 && randomInt <= 91)
        {
            //设置爆率
            BaoLvID = 5;
        }

        //获取当前爆率
        if (randomInt >= 92 && randomInt <= 95)
        {
            //设置爆率
            BaoLvID = 6;
        }

        //获取当前爆率
        if (randomInt >= 96 && randomInt <= 98)
        {
            //设置爆率
            BaoLvID = 7;
        }

        //获取当前爆率
        if (randomInt >= 99 && randomInt <= 100)
        {
            //设置爆率
            BaoLvID = 8;
        }

        bool ifUpdate = false;
        //如果随机到的爆率等级比之前低,则爆率不降低
        if (BaoLvIDLast >= BaoLvID)
        {
            BaoLvID = BaoLvIDLast;
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_436");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);

            //累计失败5次必须升级成功.
            Game_PublicClassVar.Get_wwwSet.BaoLvFailNum = Game_PublicClassVar.Get_wwwSet.BaoLvFailNum + 1;
            int lvNumMax = 5;
            if (upType == "1")
            {
                lvNumMax = 10;          //金币升级为10次
            }
            if (upType == "2") {
                lvNumMax = 5;          //钻石升级为10次
            }
            if (Game_PublicClassVar.Get_wwwSet.BaoLvFailNum >= lvNumMax)
            {
                Game_PublicClassVar.Get_wwwSet.BaoLvFailNum = 0;
                BaoLvID = BaoLvID + 1;
                if (BaoLvID >= 8) {
                    BaoLvID = 8;
                }

                BaoLvIDLast = BaoLvID;
                ifUpdate = true;
            }
        }
        else {

            BaoLvIDLast = BaoLvID;
            ifUpdate = true;
        }

        //升级成功刷新显示
        if (ifUpdate)
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_435");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);

            UpdateStatus = true;

            //存储当前爆率等级
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BaoLvID", BaoLvID.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            Game_PublicClassVar.Get_wwwSet.BaoLvID = BaoLvID;

            showBaoLvShow();

        }

        ShowZuanShi();

    }

    private void showBaoLvShow() {

        string lang_str_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("当前爆率");
        string lang_str_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("倍爆率");

        Obj_BaoLvShowText.GetComponent<Text>().text = lang_str_1 + "：" + Game_PublicClassVar.Get_function_Rose.GetBaoLvName(BaoLvID) + "(" + Game_PublicClassVar.function_Rose.GetBaoLvProValue(BaoLvID).ToString("0.0") + lang_str_2 + ")";
        //Obj_BaoLvShowText.GetComponent<Text>().text = "当前爆率：" + Game_PublicClassVar.Get_function_Rose.GetBaoLvName(BaoLvID) + "(" + Game_PublicClassVar.function_Rose.GetBaoLvProValue(BaoLvID).ToString("0.0") + "倍爆率)";

    }


    //根据等级获得消耗金币
    private int getBaoLvGold() {

        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

        if (roseLv < 20) {
            return 80000;
        }

        if (roseLv < 30)
        {
            return 100000;
        }

        if (roseLv < 40)
        {
            return 120000;
        }

        if (roseLv < 50)
        {
            return 150000;
        }

        return 200000;

    }

}
