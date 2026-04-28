using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_ZhanQuJinPai : MonoBehaviour {

    public GameObject Obj_LingPaiSet;
    public GameObject Obj_LingPai;

    public GameObject Obj_LingPaiShow_Gold;
    public GameObject Obj_LingPaiShow_ZuanShi;

	// Use this for initialization
	void Start () {

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_LingPaiSet);

        //循环令牌
        for (int i = 1; i <= 30; i++)
        {
            string qianDaoID = (10000 + i).ToString();
            GameObject lingPaiObj = (GameObject)Instantiate(Obj_LingPai);
            lingPaiObj.transform.SetParent(Obj_LingPaiSet.transform);
            lingPaiObj.transform.localScale = new Vector3(1, 1, 1);
            lingPaiObj.GetComponent<LingPai_RewardShow>().LingPaiID = qianDaoID;
        }

        //显示令牌已经激活
        ObscuredString rmbValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (rmbValue == "") {
            rmbValue = "0";
        }
        int rmb = int.Parse(rmbValue);
        if (rmb >= 98) {
            Obj_LingPaiShow_Gold.SetActive(false);
        }
        if (rmb >= 198)
        {
            Obj_LingPaiShow_ZuanShi.SetActive(false);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
