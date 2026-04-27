using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LvLiBao : MonoBehaviour
{

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

        //MeiRiLiBaoStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MeiRiLiBao", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //MeiRiLiBaoStr = "3001;3002;3003;3004;3005";
        //MeiRiLiBaoStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LvLiBao", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "GameMainValue");
        

        //根据等级显示对应礼包
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        string libaoXuHao = "";
        if (roseLv >= 1) {
            libaoXuHao = "1";
        }
        if (roseLv >= 18)
        {
            libaoXuHao = "2";
        }
        if (roseLv >= 30)
        {
            libaoXuHao = "3";
        }
        if (roseLv >= 40)
        {
            libaoXuHao = "4";
        }
        if (roseLv >= 50)
        {
            libaoXuHao = "5";
        }

        MeiRiLiBaoStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "LvLiBao", "GameMainValue");

        string[] meiRiStrList = MeiRiLiBaoStr.Split(';');

        //清空显示
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_MeiRiListSet);
        
        for (int i = 0; i < meiRiStrList.Length; i++)
        {
            string activityID = meiRiStrList[i];
            //判定当前礼包ID是否已经购买
            string activityStatus = "0";
            if (Game_PublicClassVar.Get_function_Rose.IfLvLiBao(activityID)) {
                activityStatus = "1";
            }

            GameObject obj_MeiRiLiBao = (GameObject)Instantiate(Obj_MeiRiLiBao);
            obj_MeiRiLiBao.transform.SetParent(Obj_MeiRiListSet.transform);
            obj_MeiRiLiBao.transform.localScale = new Vector3(1, 1, 1);
            obj_MeiRiLiBao.GetComponent<UI_LvLiBaoReward>().ActiveID = activityID;
            obj_MeiRiLiBao.GetComponent<UI_LvLiBaoReward>().ActivityStatus = activityStatus;
            obj_MeiRiLiBao.GetComponent<UI_LvLiBaoReward>().UpdateShowStatus = true;
        }
    }

}
