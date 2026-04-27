using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PastureItemFunctionSet : MonoBehaviour {

    public GameObject Obj_HeChengSet;
    public GameObject Obj_PastureNpcBuySet;
    public GameObject Obj_PastureFuMoSet;

    public GameObject Obj_EquipBtnText_1;
    public GameObject Obj_EquipBtnText_2;
    public GameObject Obj_EquipBtnText_3;

    public GameObject ObjBtn_HeCheng;
    public GameObject ObjBtn_ShangRen;
    public GameObject ObjBtn_FuMo;

    private float UpdateShowTime;
    public GameObject Obj_PastureGold;
    public GameObject Obj_PastureRmb;
    public bool UpdateShow;

    // Use this for initialization
    void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);
        //Game_PublicClassVar.Get_function_Pasture.CreatePastureTrader();
        //初始化
        Btn_Function("1");

    }
	
	// Update is called once per frame
	void Update () {


        UpdateShowTime = UpdateShowTime + Time.deltaTime;
        if (UpdateShowTime >= 0.1f)
        {
            UpdateShow = true;
            UpdateShowTime = 0;
        }

        if (UpdateShow)
        {
            UpdateShow = false;
            string nowPastureGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureGold", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Obj_PastureGold.GetComponent<Text>().text = nowPastureGold;
            Obj_PastureRmb.GetComponent<Text>().text = Game_PublicClassVar.Get_function_Rose.GetRoseRMB().ToString();
        }

    }

    public void Btn_Close()
    {
        Destroy(this.gameObject);
    }

    //切换功能
    public void Btn_Function(string type) {

        Obj_HeChengSet.SetActive(false);
        Obj_PastureNpcBuySet.SetActive(false);
        Obj_PastureFuMoSet.SetActive(false);

        switch (type) {

            case "1":
                Obj_HeChengSet.SetActive(true);
                break;

            case "2":
                Obj_PastureNpcBuySet.SetActive(true);
                break;

            case "3":
                Obj_PastureFuMoSet.SetActive(true);
                Obj_PastureFuMoSet.GetComponent<UI_PastureFuMoSet>().InitShow();
                break;

        }

        Show_Table(type);
    }

    public void Show_Table(string type)
    {

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        ObjBtn_HeCheng.GetComponent<Image>().sprite = img;
        ObjBtn_ShangRen.GetComponent<Image>().sprite = img;
        ObjBtn_FuMo.GetComponent<Image>().sprite = img;

        switch (type)
        {
            //牧场升级
            case "1":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_HeCheng.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;

            //牧场购买
            case "2":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_ShangRen.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;

            //牧场购买
            case "3":

                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_FuMo.GetComponent<Image>().sprite = img;
                Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;
        }

    }

}
