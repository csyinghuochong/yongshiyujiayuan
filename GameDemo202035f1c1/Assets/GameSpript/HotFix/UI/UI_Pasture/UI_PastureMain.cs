using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureMain : MonoBehaviour {

    public GameObject PastureSellSet;
    public GameObject PastureUpLvSet;
    public GameObject PastureLveDuoSet;
    public GameObject PastureKuangMaiSet;
    public GameObject Obj_PastureGold;
    public GameObject Obj_RoseRmb;
    public GameObject Obj_RoseGold;
    public ObscuredBool UpdateShow;
    private ObscuredFloat UpdateShowTime;

    public GameObject Obj_EquipBtnText_1;
    public GameObject Obj_EquipBtnText_2;
    public GameObject Obj_EquipBtnText_3;
    public GameObject Obj_EquipBtnText_4;

    public GameObject ObjBtn_Up;
    public GameObject ObjBtn_Buy;
    public GameObject ObjBtn_LveDuo;
    public GameObject ObjBtn_KuangMai;

    // Use this for initialization
    void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        //初始化购买显示
        //Btn_OpenBuy();
        Btn_OpenUpLv();

    }
	
	// Update is called once per frame
	void Update () {

        UpdateShowTime = UpdateShowTime + Time.deltaTime;
        if (UpdateShowTime >= 0.1f) {
            UpdateShow = true;
            UpdateShowTime = 0;
        }

        if (UpdateShow) {
            UpdateShow = false;
            string nowPastureGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureGold", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Obj_PastureGold.GetComponent<Text>().text = nowPastureGold;
            int nowRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
            Obj_RoseRmb.GetComponent<Text>().text = nowRmb.ToString();
            Obj_RoseGold.GetComponent<Text>().text = Game_PublicClassVar.Get_function_Rose.GetRoseMoney().ToString();
        }

	}



    //增加钻石
    public void Btn_AddZuanShi() {

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


    //升级
    public void Btn_OpenUpLv()
    {
        ClearnShow();
        PastureUpLvSet.SetActive(true);
        PastureUpLvSet.GetComponent<UI_PastureUpLvSet>().Init();
        Show_Table("1");
    }

    //购买
    public void Btn_OpenBuy()
    {
        ClearnShow();
        PastureSellSet.SetActive(true);
        PastureSellSet.GetComponent<UI_PastureSellSet>().Init();
        Show_Table("2");
    }

    //掠夺
    public void Btn_OpenLvDuo()
    {
        ClearnShow();
        PastureLveDuoSet.SetActive(true);
        //PastureLveDuoSet.GetComponent<UI_PastureSellSet>().Init();
        Show_Table("3");
    }

    //掠夺
    public void Btn_OpenKuangMai()
    {
        ClearnShow();
        PastureKuangMaiSet.SetActive(true);
        //PastureKuangMaiSet.GetComponent<UI_PastureSellSet>().Init();
        Show_Table("4");
    }

    public void Show_Table(string type) {

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        ObjBtn_Up.GetComponent<Image>().sprite = img;
        ObjBtn_Buy.GetComponent<Image>().sprite = img;
        ObjBtn_LveDuo.GetComponent<Image>().sprite = img;
        ObjBtn_KuangMai.GetComponent<Image>().sprite = img;

        switch (type)
        {
            //牧场升级
            case "1":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_Up.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;

            //牧场购买
            case "2":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_Buy.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;

            //牧场掠夺
            case "3":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_LveDuo.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;

            //牧场矿脉
            case "4":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_KuangMai.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;
        }

    }

    public void ClearnShow()
    {
        PastureSellSet.SetActive(false);
        PastureUpLvSet.SetActive(false);
        PastureLveDuoSet.SetActive(false);
        PastureKuangMaiSet.SetActive(false);
    }




    //关闭UI
    public void CloseUI() {
        Destroy(this.gameObject);
    }

}
