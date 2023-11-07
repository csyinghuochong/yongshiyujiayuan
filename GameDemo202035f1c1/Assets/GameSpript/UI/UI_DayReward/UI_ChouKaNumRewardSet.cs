using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChouKaNumRewardSet : MonoBehaviour {

    public GameObject Obj_ChouKaNumStr;

    public GameObject Obj_ChouKaShowList;
    public GameObject Obj_ChouKaShowListSet;

	// Use this for initialization
	void Start () {

        Init();
    }

    public void Init() {

        //读取当前抽卡次数  Day_ChouKaNumReward
        string dayChouKaNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ChouKaNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (dayChouKaNum == "")
        {
            dayChouKaNum = "0";
        }
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("今日抽卡次数");
        Obj_ChouKaNumStr.GetComponent<Text>().text = langStr + ":" + dayChouKaNum;

        //清理显示
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ChouKaShowListSet);

        //循环列表
        for (int i = 1; i <= 5; i++)
        {
            GameObject obj = (GameObject)Instantiate(Obj_ChouKaShowList);
            obj.transform.SetParent(Obj_ChouKaShowListSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_ChouKaNumRewardList>().ChouKaRewardListID = i.ToString();
            obj.GetComponent<UI_ChouKaNumRewardList>().Init();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CloseUI() {

        this.gameObject.SetActive(false);

    }
}
