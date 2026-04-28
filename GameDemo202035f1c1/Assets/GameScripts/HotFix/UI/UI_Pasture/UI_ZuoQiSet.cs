using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ZuoQiSet : MonoBehaviour {

    public GameObject Obj_FunctionBtnSet;
    public GameObject Obj_ZuoQiXianJiSet;
    public GameObject Obj_ZuoQiShowSet;
    public GameObject Obj_ZuoQiNengLiSet;
    public GameObject Obj_ZuoWeiShiSet;
    public GameObject Obj_ZuoQiShouCangSet;

    public GameObject Obj_EquipBtnText_1;
    public GameObject Obj_EquipBtnText_2;
    public GameObject Obj_EquipBtnText_3;
    public GameObject Obj_EquipBtnText_4;

    public GameObject ObjBtn_XinXi;
    public GameObject ObjBtn_NengLi;
    public GameObject ObjBtn_WeiShi;
    public GameObject ObjBtn_ShouCang;

    public GameObject Obj_PastureGold;
    public GameObject Obj_RoseRmb;
    public GameObject Obj_RoseGold;
    public ObscuredBool UpdateShow;
    private ObscuredFloat UpdateShowTime;


    // Use this for initialization
    void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);
        Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiSetNow = this.gameObject;
        //初始化购买显示
        Btn_OpenXianJi();

    }
	
	// Update is called once per frame
	void Update () {

        UpdateShowTime = UpdateShowTime + Time.deltaTime;
        if (UpdateShowTime >= 0.1f)
        {
            UpdateShow = true;
        }

        if (UpdateShow)
        {
            UpdateShow = false;
            string nowPastureGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureGold", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Obj_PastureGold.GetComponent<Text>().text = nowPastureGold;
            int nowRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
            Obj_RoseRmb.GetComponent<Text>().text = nowRmb.ToString();
            long nowGold = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
            Obj_RoseGold.GetComponent<Text>().text = nowGold.ToString();
        }

    }


    //献祭
    public void Btn_OpenXianJi()
    {
        try
        {

            ClearnShow();

            //如果献祭满了,则显示坐骑信息

            string nowZuoQiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            if (nowZuoQiLv == "" || nowZuoQiLv == null || nowZuoQiLv == "0")
            {
                Obj_ZuoQiXianJiSet.SetActive(true);
                Obj_FunctionBtnSet.SetActive(false);
            }
            else
            {
                Btn_OpenShowData();
                Obj_FunctionBtnSet.SetActive(true);
            }
        }
        catch (Exception ex) {
            Debug.LogError("坐骑献祭错误2222:" + ex);
        }

    }

    //展示信息
    public void Btn_OpenShowData()
    {
        ClearnShow();
        Obj_ZuoQiShowSet.SetActive(true);
        Obj_ZuoQiShowSet.GetComponent<UI_ZuoQiShowSet>().Init();

        Show_Table("1");
    }

    //展示坐骑能力
    public void Btn_OpenShowNengLi()
    {
        ClearnShow();
        Obj_ZuoQiNengLiSet.SetActive(true);
        Obj_ZuoQiNengLiSet.GetComponent<UI_ZuoQiNengLiSet>().Init();

        Show_Table("2");
    }

    //展示信息
    public void Btn_OpenWeiShi()
    {
        ClearnShow();
        Obj_ZuoWeiShiSet.SetActive(true);
        Obj_ZuoWeiShiSet.GetComponent<UI_ZuoWeiShiSet>().Init();

        Show_Table("3");
    }

    //展示信息
    public void Btn_OpenShouCang()
    {
        ClearnShow();
        Obj_ZuoQiShouCangSet.SetActive(true);
        Obj_ZuoQiShouCangSet.GetComponent<UI_ZuoQiShouCangSet>().Init();

        Show_Table("4");
    }

    public void ClearnShow()
    {
        Obj_ZuoQiXianJiSet.SetActive(false);
        Obj_ZuoQiShowSet.SetActive(false);
        Obj_ZuoQiNengLiSet.SetActive(false);
        Obj_ZuoWeiShiSet.SetActive(false);
        Obj_ZuoQiShouCangSet.SetActive(false);
    }


    //关闭UI
    public void CloseUI() {
        Destroy(this.gameObject);
    }


    //显示标签
    public void Show_Table(string type)
    {

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        ObjBtn_XinXi.GetComponent<Image>().sprite = img;
        ObjBtn_NengLi.GetComponent<Image>().sprite = img;
        ObjBtn_WeiShi.GetComponent<Image>().sprite = img;
        ObjBtn_ShouCang.GetComponent<Image>().sprite = img;

        switch (type)
        {
            //牧场升级
            case "1":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_XinXi.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;

            //牧场购买
            case "2":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_NengLi.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;

            //牧场购买
            case "3":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_WeiShi.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;

            //牧场购买
            case "4":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_ShouCang.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;
        }

    }


    //增加钻石
    public void Btn_AddZuanShi()
    {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_12");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否前往购买钻石界面", GoToGoToDuiHuanGold, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否前往兑换金币界面？", GoToGoToDuiHuanGold, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }


    public void GoToGoToDuiHuanGold()
    {
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing == null)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().ClearnOpenUI();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_HuoDongDaTing();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().IfInitStatus = false;
        }
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().Btn_GamePay();
    }


    //兑换
    public void Btn_DuiHuan()
    {

        //获取数据
        string PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        int duiHuanCostZuanShi = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanCostZuanShi", "ID", PastureID, "PastureUpLv_Template"));
        int duiHuanGetPastureGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DuiHuanGetPastureGold", "ID", PastureID, "PastureUpLv_Template"));
        //Obj_DuiHuanShow.GetComponent<Text>().text = "";

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否消耗" + duiHuanCostZuanShi + "钻石兑换" + duiHuanGetPastureGold + "家园资金", Game_PublicClassVar.Get_function_Pasture.DuiHuanPastureGold, Game_PublicClassVar.Get_function_Pasture.DuiHuanPastureGold_Ten, "系统提示", "兑换一次", "兑换十次", null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

}
