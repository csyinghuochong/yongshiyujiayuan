using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainRosePastureData : MonoBehaviour {

    public GameObject Obj_PastureLv;
    public GameObject Obj_PastureRenKou;
    public GameObject Obj_PastureGold;

    // Use this for initialization
    void Start () {
        ShowData();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //展示信息
    public void ShowData() {

        string nowPastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nowPastureLvName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", nowPastureLv, "PastureUpLv_Template");
        string nowPastureGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureGold", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        int nowRenKou = Game_PublicClassVar.Get_function_Pasture.GetPasturePeopleNum();
        string nowPeopleNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PeopleNumMax", "ID", nowPastureLv, "PastureUpLv_Template");
        Obj_PastureLv.GetComponent<Text>().text = nowPastureLvName;
        Obj_PastureRenKou.GetComponent<Text>().text = nowRenKou.ToString() + "/" + nowPeopleNumMax;
        Obj_PastureGold.GetComponent<Text>().text = nowPastureGold;

    }
}
