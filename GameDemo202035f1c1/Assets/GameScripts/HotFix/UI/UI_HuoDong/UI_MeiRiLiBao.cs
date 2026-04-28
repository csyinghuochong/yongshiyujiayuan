using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MeiRiLiBao : MonoBehaviour {

    public string MeiRiLiBaoStr;
    public GameObject Obj_MeiRiLiBao;
    public GameObject Obj_MeiRiListSet;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    

    //初始化
    public void ChuShiHua() {

        /*
        string sss =  Game_PublicClassVar.Get_function_Rose.GetMeiRiLiBao();
        Debug.Log("每日礼包：" + sss);
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MeiRiLiBao", sss,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
        */

        MeiRiLiBaoStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MeiRiLiBao", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string[] meiRiStrList = MeiRiLiBaoStr.Split(';');

        //清空显示
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_MeiRiListSet);
        
        for (int i = 0; i < meiRiStrList.Length; i++)
        {
            string activityID = meiRiStrList[i].Split(',')[0];
            string activityStatus = meiRiStrList[i].Split(',')[1];

            GameObject obj_MeiRiLiBao = (GameObject)Instantiate(Obj_MeiRiLiBao);
            obj_MeiRiLiBao.transform.SetParent(Obj_MeiRiListSet.transform);
            obj_MeiRiLiBao.transform.localScale = new Vector3(1, 1, 1);
            obj_MeiRiLiBao.GetComponent<UI_MeiRiLiBaoReward>().ActiveID = activityID;
            obj_MeiRiLiBao.GetComponent<UI_MeiRiLiBaoReward>().ActivityStatus = activityStatus;
            obj_MeiRiLiBao.GetComponent<UI_MeiRiLiBaoReward>().UpdateShowStatus = true;
        }
    }

}
