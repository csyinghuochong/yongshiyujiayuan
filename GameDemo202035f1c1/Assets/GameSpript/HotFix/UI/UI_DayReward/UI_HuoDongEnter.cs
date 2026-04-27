using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_HuoDongEnter : MonoBehaviour
{

    public GameObject Obj_FuBen;
    public GameObject Obj_FnBenListSet;
    public GameObject Obj_FuBenNumStr;

    public GameObject Obj_ExpShow_1;
    public GameObject Obj_ExpShow_2;
    public GameObject Obj_ExpShow_3;
    public GameObject Obj_ExpShow_4;
    public GameObject Obj_ExpShow_5;

    public GameObject Obj_LianZhanNum;

    private int KillBossNum;
    // Use this for initialization
    void Start()
    {
        /*
        float show_X = 0;
        float show_Y = 0;
        string[] DayFunBen_ID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DayFunBen_ID", "GameMainValue").Split(';');
        for (int i = 0; i <= DayFunBen_ID.Length - 1; i++) {
            if (DayFunBen_ID[i] != "") {
                GameObject fubenListObj = (GameObject)Instantiate(Obj_FuBen);
                fubenListObj.transform.SetParent(Obj_FnBenListSet.transform);
                fubenListObj.transform.localPosition = new Vector3(show_X, show_Y, 0);
                fubenListObj.transform.localScale = new Vector3(1, 1, 1);
                fubenListObj.GetComponent<UI_FubenList>().ScenceID = DayFunBen_ID[i];
                show_Y = show_Y + 300;
            }
        }
        */



        //获取次数
        string now_FuBenNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_FuBenNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string FuBenNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DayFunBen_Num", "GameMainValue");
        Obj_FuBenNumStr.GetComponent<Text>().text = "每日进入副本次数：" + now_FuBenNum + "/" + FuBenNumMax;

        Obj_ExpShow_1.GetComponent<Text>().color = new Color(0.61f,0.61f,0.61f);
        Obj_ExpShow_2.GetComponent<Text>().color = new Color(0.61f, 0.61f, 0.61f);
        Obj_ExpShow_3.GetComponent<Text>().color = new Color(0.61f, 0.61f, 0.61f);
        Obj_ExpShow_4.GetComponent<Text>().color = new Color(0.61f, 0.61f, 0.61f);
        Obj_ExpShow_5.GetComponent<Text>().color = new Color(0.61f, 0.61f, 0.61f);
        
        //获取当前BOSS连斩次数
        KillBossNum = PlayerPrefs.GetInt("FuBenKillBossNum" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName);

        Obj_LianZhanNum.GetComponent<Text>().text = KillBossNum.ToString();

        switch (KillBossNum)
        {
            case 0:
                //Obj_ExpShow_1.GetComponent<Text>().color = new Color(0.61f, 0.61f, 0.61f);
                break;
            case 1:
                Obj_ExpShow_1.GetComponent<Text>().color = new Color(1, 1, 1);
                break;
            case 2:
                Obj_ExpShow_2.GetComponent<Text>().color = new Color(1, 1, 1);
                break;
            case 3:
                Obj_ExpShow_3.GetComponent<Text>().color = new Color(1, 1, 1);
                break;
            case 4:
                Obj_ExpShow_4.GetComponent<Text>().color = new Color(1, 1, 1);
                break;
            case 5:
                Obj_ExpShow_5.GetComponent<Text>().color = new Color(1, 1, 1);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Btn_CloseUI()
    {
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
        Destroy(this.gameObject);
    }

    /*
    //发送经验奖励
    public void SendEexp() {


        //效验次数
        int LingQuFuBenExp = PlayerPrefs.GetInt("LingQuFuBenExp");
        if (LingQuFuBenExp >= 1) {
            Game_PublicClassVar.Get_function_UI.GameHint("今日已经领取");
            return;
        }
        if (KillBossNum < 1) {
            Game_PublicClassVar.Get_function_UI.GameHint("今天未击杀BOSS不能领取！");
            return;
        }
        PlayerPrefs.SetInt("LingQuFuBenExp",1);

        //获取等级对应的经验
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        int Rose_Exp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseUpExp", "RoseLv", roseLv.ToString(), "RoseExp_Template"));

        int addexp = 0;

        switch (KillBossNum)
        {
            case 0:
                //Obj_ExpShow_1.GetComponent<Text>().color = new Color(0.61f, 0.61f, 0.61f);
                break;
            case 1:
                addexp = (int)(Rose_Exp * 0.05f); 
                break;
            case 2:
                addexp = (int)(Rose_Exp * 0.1f); 
                break;
            case 3:
                addexp = (int)(Rose_Exp * 0.2f); 
                break;
            case 4:
                addexp = (int)(Rose_Exp * 0.3f); 
                break;
            case 5:
                addexp = (int)(Rose_Exp * 0.5f); 
                break;
        }

        //Debug.Log("增加：" + Rose_Exp);
        Game_PublicClassVar.Get_function_Rose.AddExp(addexp);

    }
    */


}