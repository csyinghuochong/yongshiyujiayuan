using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class UI_ZhenYingShouLie : MonoBehaviour
{

    public GameObject Obj_ShouLieListParSet;
    public GameObject Obj_ShouLieList;
    public ObscuredString ShouLieStr;
    public ObscuredString ShouLieRewardStr;
    public GameObject ShouLieShowNum;

    // Start is called before the first frame update
    void Start()
    {

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ShouLieListParSet);

        ShouLieStr = "500,1000,1500,2000,3000";
        ShouLieRewardStr = "5,5,5,5,5";

        string[] ShouLieList = ShouLieStr.ToString().Split(',');
        string[] ShouLieRewardList = ShouLieRewardStr.ToString().Split(',');
        //实例化
        for (int i = 0; i < ShouLieList.Length; i++) {

            GameObject obj = (GameObject)Instantiate(Obj_ShouLieList);
            obj.transform.SetParent(Obj_ShouLieListParSet.transform);
            obj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            obj.GetComponent<UI_ZhenYingShouLieList>().ShouLieRewardID = i.ToString();
            obj.GetComponent<UI_ZhenYingShouLieList>().ShouLieNum = int.Parse(ShouLieList[i]);
            obj.GetComponent<UI_ZhenYingShouLieList>().ShouLieRewardNum = int.Parse(ShouLieRewardList[i]);
            obj.GetComponent<UI_ZhenYingShouLieList>().Init();
        }

        //获取当前今日击杀值
        string dayKillNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayKillNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

        if (dayKillNumStr == "" || dayKillNumStr == null)
        {
            dayKillNumStr = "0";
        }

        int dayKillNum = int.Parse(dayKillNumStr);
        Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum = dayKillNum;
        ShouLieShowNum.GetComponent<Text>().text = "今日已狩猎:" + Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum + "只";

    }

    // Update is called once per frame
    void Update()
    {
        


    }
}
