using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_TianFu : MonoBehaviour {

    public ObscuredString LearnTianFuIDStr;

    public GameObject Obj_TianFuName;
    public GameObject Obj_TianFuDes;
    public GameObject Obj_TianFuLv;
    public GameObject Obj_SPValue;
    public GameObject Obj_UseSPValue;

    public GameObject Obj_TianFu;
    public GameObject Obj_TianFuSet;


    public GameObject[] TianFuObj;

    public ObscuredString TianFuXuanZhongID;
    public GameObject TianFuXuanZhongObj;


    public GameObject Obj_TianFuShowType_1;
    public GameObject Obj_TianFuShowType_2;
    public GameObject Obj_TianFuBtnType_1;
    public GameObject Obj_TianFuBtnType_2;
    public GameObject Obj_TianFuTextType_1;
    public GameObject Obj_TianFuTextType_2;

    // Use this for initialization
    void Start () {

        //初始化互数据
        Init();
        //默认显示第一个天赋切页
        Btn_TianFuType("1");

        //显示天赋名称
        if (Game_PublicClassVar.Get_function_Rose.GetRoseOcc() == "2") {
            string name = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("魔法天赋");
            Obj_TianFuTextType_1.GetComponent<Text>().text = name;
            name = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("召唤天赋");
            Obj_TianFuTextType_2.GetComponent<Text>().text = name;
        }

        //显示天赋名称
        if (Game_PublicClassVar.Get_function_Rose.GetRoseOcc() == "3")
        {
            string name = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("射击天赋");
            Obj_TianFuTextType_1.GetComponent<Text>().text = name;
            name = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("生存天赋");
            Obj_TianFuTextType_2.GetComponent<Text>().text = name;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void Init() {
        LearnTianFuIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianFuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] LearnTianFuID = LearnTianFuIDStr.ToString().Split(';');


        string roseSP = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_SPValue.GetComponent<Text>().text = roseSP;
        int nowUseSP = Game_PublicClassVar.Get_function_UI.GetTianFuUseNum();
        Obj_UseSPValue.GetComponent<Text>().text = nowUseSP.ToString();
        //Debug.Log("nowUseSP =" + nowUseSP + ";roseSP = " + roseSP);
        //Obj_SPValue.GetComponent<Text>().text = "<color=#04af00>" + nowUseSP + "</color>" + "/" + (nowUseSP + int.Parse(roseSP)).ToString();
        //天赋
        for (int i = 0; i <= TianFuObj.Length - 1; i++)
        {
            /*
            TianFuObj[i] = (GameObject)Instantiate(Obj_TianFu);
            TianFuObj[i].transform.SetParent(Obj_TianFuSet.transform);
            TianFuObj[i].transform.localPosition = getSkillPosition((i + 1).ToString(),TianFuObj[i]);
            TianFuObj[i].transform.localScale = new Vector3(1, 1, 1);
            */
            if (TianFuObj[i] != null) {
                TianFuObj[i].GetComponent<UI_TianFuIcon>().Obj_Fuji = this.gameObject;
                GameObject tianFuObj = TianFuObj[i];

                string occStr = Game_PublicClassVar.Get_function_Rose.GetRoseOcc();

                if (occStr == "1" || occStr == "0" || occStr == "" || occStr == null||occStr == "2" || occStr == "3") {

                    //战士天赋
                    switch (i + 1)
                    {
                        //第一天赋
                        case 1:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "101101";
                            break;
                        case 2:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "102101";
                            break;
                        case 3:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "102201";
                            break;
                        case 4:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "102301";
                            break;
                        case 5:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "103101";
                            break;
                        case 6:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "103201";
                            break;
                        case 7:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "104301";
                            break;
                        case 8:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "104201";
                            break;
                        case 9:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "104101";
                            break;
                        case 10:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "104401";
                            break;
                        case 11:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "104501";
                            break;
                        case 12:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "105101";
                            break;
                        case 13:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "105201";
                            break;
                        case 14:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "106301";
                            break;
                        case 15:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "106201";
                            break;
                        case 16:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "106101";
                            break;
                        case 17:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "106401";
                            break;
                        case 18:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "106501";
                            break;
                        case 19:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "107201";
                            break;
                        case 20:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "107101";
                            break;
                        case 21:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "107301";
                            break;
                        case 22:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "108101";
                            break;


                        //第二天赋
                        case 42:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "111101";
                            break;
                        case 43:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "112101";
                            break;
                        case 44:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "112201";
                            break;
                        case 45:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "112301";
                            break;
                        case 46:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "113101";
                            break;
                        case 47:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "113201";
                            break;
                        case 48:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "114301";
                            break;
                        case 49:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "114201";
                            break;
                        case 50:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "114101";
                            break;
                        case 51:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "114401";
                            break;
                        case 52:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "114501";
                            break;
                        case 53:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "115101";
                            break;
                        case 54:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "115201";
                            break;
                        case 55:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "116301";
                            break;
                        case 56:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "116201";
                            break;
                        case 57:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "116101";
                            break;
                        case 58:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "116401";
                            break;
                        case 59:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "116501";
                            break;
                        case 60:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "117201";
                            break;
                        case 61:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "117101";
                            break;
                        case 62:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "117301";
                            break;
                        case 63:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "118101";
                            break;
                    }
                }

                //法师天赋
                if (occStr == "2")
                {

                    switch (i + 1)
                    {
                        //第一天赋
                        case 1:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "201101";
                            break;
                        case 2:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "202101";
                            break;
                        case 3:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "202201";
                            break;
                        case 4:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "202301";
                            break;
                        case 5:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "203101";
                            break;
                        case 6:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "203201";
                            break;
                        case 7:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "204301";
                            break;
                        case 8:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "204201";
                            break;
                        case 9:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "204101";
                            break;
                        case 10:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "204401";
                            break;
                        case 11:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "204501";
                            break;
                        case 12:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "205101";
                            break;
                        case 13:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "205201";
                            break;
                        case 14:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "206301";
                            break;
                        case 15:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "206201";
                            break;
                        case 16:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "206101";
                            break;
                        case 17:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "206401";
                            break;
                        case 18:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "206501";
                            break;
                        case 19:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "207201";
                            break;
                        case 20:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "207101";
                            break;
                        case 21:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "207301";
                            break;
                        case 22:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "208101";
                            break;


                        //第二天赋
                        case 42:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "211101";
                            break;
                        case 43:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "212101";
                            break;
                        case 44:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "212201";
                            break;
                        case 45:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "212301";
                            break;
                        case 46:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "213101";
                            break;
                        case 47:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "213201";
                            break;
                        case 48:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "214301";
                            break;
                        case 49:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "214201";
                            break;
                        case 50:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "214101";
                            break;
                        case 51:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "214401";
                            break;
                        case 52:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "214501";
                            break;
                        case 53:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "215101";
                            break;
                        case 54:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "215201";
                            break;
                        case 55:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "216301";
                            break;
                        case 56:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "216201";
                            break;
                        case 57:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "216101";
                            break;
                        case 58:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "216401";
                            break;
                        case 59:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "216501";
                            break;
                        case 60:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "217201";
                            break;
                        case 61:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "217101";
                            break;
                        case 62:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "217301";
                            break;
                        case 63:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "218101";
                            break;
                    }
                }


                //猎人天赋
                if (occStr == "3")
                {

                    switch (i + 1)
                    {
                        //第一天赋
                        case 1:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "301101";
                            break;
                        case 2:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "302101";
                            break;
                        case 3:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "302201";
                            break;
                        case 4:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "302301";
                            break;
                        case 5:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "303101";
                            break;
                        case 6:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "303201";
                            break;
                        case 7:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "304301";
                            break;
                        case 8:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "304201";
                            break;
                        case 9:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "304101";
                            break;
                        case 10:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "304401";
                            break;
                        case 11:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "304501";
                            break;
                        case 12:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "305101";
                            break;
                        case 13:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "305201";
                            break;
                        case 14:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "306301";
                            break;
                        case 15:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "306201";
                            break;
                        case 16:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "306101";
                            break;
                        case 17:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "306401";
                            break;
                        case 18:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "306501";
                            break;
                        case 19:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "307201";
                            break;
                        case 20:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "307101";
                            break;
                        case 21:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "307301";
                            break;
                        case 22:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "308101";
                            break;


                        //第二天赋
                        case 42:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "311101";
                            break;
                        case 43:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "312101";
                            break;
                        case 44:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "312201";
                            break;
                        case 45:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "312301";
                            break;
                        case 46:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "313101";
                            break;
                        case 47:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "313201";
                            break;
                        case 48:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "314301";
                            break;
                        case 49:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "314201";
                            break;
                        case 50:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "314101";
                            break;
                        case 51:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "314401";
                            break;
                        case 52:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "314501";
                            break;
                        case 53:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "315101";
                            break;
                        case 54:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "315201";
                            break;
                        case 55:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "316301";
                            break;
                        case 56:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "316201";
                            break;
                        case 57:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "316101";
                            break;
                        case 58:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "316401";
                            break;
                        case 59:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "316501";
                            break;
                        case 60:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "317201";
                            break;
                        case 61:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "317101";
                            break;
                        case 62:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "317301";
                            break;
                        case 63:
                            tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "318101";
                            break;
                    }
                }

            }
        }

        //默认选中第一个
        TianFuObj[0].GetComponent<UI_TianFuIcon>().Btn_TianFuIcon();
    }

    //传入位置和物体 写入ID
    /*
    private Vector3 getSkillPosition(string iconNum,GameObject tianFuObj) {
        switch (iconNum) { 
            
            case "1":
                tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "10001";
                return new Vector3(-530f, -23.1f, 0);
                break;
            case "2":
                tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "10003";
                return new Vector3(-399.1f, 97.9f, 0);
                break;
            case "3":
                tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "10002";
                return new Vector3(-420f, -23.1f, 0);
                break;
            case "4":
                tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "10004";
                return new Vector3(-399.1f, -142.1f, 0);
                break;
            case "5":
                tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "10005";
                return new Vector3(-300.2f, 39f, 0);
                break;
            case "6":
                tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "10006";
                return new Vector3(-300.2f, -80.8f, 0);
                break;
            case "7":
                tianFuObj.GetComponent<UI_TianFuIcon>().TianFuID = "10007";
                return new Vector3(-199.9f, -21.1f, 0);
                break;
            case "8":
                return new Vector3(-140.9f, 91.9f, 0);
                break;
            case "9":
                return new Vector3(-89.5f, -21.5f, 0);
                break;
            case "10":
                return new Vector3(-139.5f, -132.1f, 0);
                break;
            case "11":
                return new Vector3(-88.8f, 174.4f, 0);
                break;
            case "12":
                return new Vector3(20.8f, -23.1f, 0);
                break;
            case "13":
                return new Vector3(-90.39f, -222.4f, 0);
                break;
            case "14":
                return new Vector3(89.7f, 149, 0);
                break;
            case "15":
                return new Vector3(139.9f, -22.3f, 0);
                break;
            case "16":
                return new Vector3(89.2f, -191.4f, 0);
                break;
        }

        return new Vector3(0f, 0f, 0f);
    }
    */

    public void Btn_TianFuUpLv()
    {
        //Debug.Log("我点击了天赋Icon升级");

        Game_PublicClassVar.Get_function_UI.UpTianFuNowLv(TianFuXuanZhongID);
    
        string roseSP = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //int nowUseSP = Game_PublicClassVar.Get_function_UI.GetTianFuUseNum();
        //Obj_SPValue.GetComponent<Text>().text = "<color=#04af00>" + nowUseSP + "</color>" + "/" + (nowUseSP + int.Parse(roseSP)).ToString();

        Obj_SPValue.GetComponent<Text>().text = roseSP;
        TianFuXuanZhongObj.GetComponent<UI_TianFuIcon>().Btn_TianFuIcon();

        //更新属性
        Game_PublicClassVar.Get_game_PositionVar.UpdataRoseProperty = true;

        //UpdateTianFuAll();

    }


    //更新全部天赋
    public void UpdateTianFuAll() {
        for (int i = 0; i <= TianFuObj.Length - 1; i++)
        {
            if (TianFuObj[i] != null) {
                TianFuObj[i].GetComponent<UI_TianFuIcon>().UpdateTianFuIconStatus = true;
            }
        }
    }

    //天赋重置
    public void Btn_ChongZhiTianFu() {


        //计算消耗的金币



        //弹出提示
        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        //string jieshaoStr = "是否消耗600钻石重置当前天赋！";
        string jieshaoStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_14");
        string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_1");
        string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_2");
        string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_3");

        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, Game_PublicClassVar.Get_function_Skill.TianFuChongZhi, null, langStrHint_1, langStrHint_2, langStrHint_3);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    //天赋切换
    public void Btn_TianFuType(string type) {

        //初始化按钮状态
        object obj = Resources.Load("GameUI/" + "Btn/Btn_69_1", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_TianFuBtnType_1.GetComponent<Image>().sprite = img;
        Obj_TianFuTextType_1.GetComponent<Text>().color = new Color(1f, 0.88f, 0.76f);
        Obj_TianFuBtnType_2.GetComponent<Image>().sprite = img;
        Obj_TianFuTextType_2.GetComponent<Text>().color = new Color(1f, 0.88f, 0.76f);

        //按钮变换
        switch (type) {

            case "1":
                Obj_TianFuShowType_1.SetActive(true);
                Obj_TianFuShowType_2.SetActive(false);

                //按钮变换
                obj = Resources.Load("GameUI/" + "Btn/Btn_69_2", typeof(Sprite));
                img = obj as Sprite;
                Obj_TianFuBtnType_1.GetComponent<Image>().sprite = img;
                Obj_TianFuTextType_1.GetComponent<Text>().color = new Color(0.71f, 0.35f, 0f);

                break;
            case "2":
                Obj_TianFuShowType_1.SetActive(false);
                Obj_TianFuShowType_2.SetActive(true);

                //按钮变换
                //按钮变换
                obj = Resources.Load("GameUI/" + "Btn/Btn_69_2", typeof(Sprite));
                img = obj as Sprite;
                Obj_TianFuBtnType_2.GetComponent<Image>().sprite = img;
                Obj_TianFuTextType_2.GetComponent<Text>().color = new Color(0.71f, 0.35f, 0f);

                break;
        }
    }
}
