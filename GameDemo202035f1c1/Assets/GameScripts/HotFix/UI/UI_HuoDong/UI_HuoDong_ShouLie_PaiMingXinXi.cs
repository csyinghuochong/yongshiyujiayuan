using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_HuoDong_ShouLie_PaiMingXinXi : MonoBehaviour {

    public ObscuredString PaiMingXinXiData;
    public ObscuredString PaiMingValue;
    public GameObject Obj_PaiMingRank;
    public GameObject Obj_PaiMingName;
    public GameObject Obj_PaiMingKillNum;
	// Use this for initialization
	void Start () {



    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init() {

        Debug.Log("PaiMingXinXiData = " + PaiMingXinXiData);
        if (PaiMingXinXiData != "" && PaiMingXinXiData != null && PaiMingXinXiData != "0")
        {
            string[] paimingList = PaiMingXinXiData.ToString().Split(',');
            Obj_PaiMingRank.GetComponent<Text>().text = PaiMingValue;
            Obj_PaiMingName.GetComponent<Text>().text = paimingList[1];
            if (paimingList[2] != "")
            {
                Obj_PaiMingKillNum.GetComponent<Text>().text = paimingList[2];
            }
            else {
                Obj_PaiMingKillNum.GetComponent<Text>().text = "暂无数据";
            }
            
        }

    }
}
