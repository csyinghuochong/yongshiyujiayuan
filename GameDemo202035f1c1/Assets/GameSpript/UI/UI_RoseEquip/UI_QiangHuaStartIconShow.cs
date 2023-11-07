using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_QiangHuaStartIconShow : MonoBehaviour {

    public ObscuredString QiangHuaLvStr;
    
    public GameObject QiangHuaLvIcon_1;     //已经强化
    public GameObject QiangHuaLvIcon_2;     //未强化
    public GameObject QiangHuaLvIcon_3;     //未达到等级

    public bool UpdateStatus;


	// Use this for initialization
	void Start () {
        UpdateStatus = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (UpdateStatus)
        {
            UpdateStatus = false;
            showQiangHua();
        }
	}

    void showQiangHua() {

        QiangHuaLvIcon_1.SetActive(false);
        QiangHuaLvIcon_2.SetActive(false);
        QiangHuaLvIcon_3.SetActive(false);

        switch (QiangHuaLvStr) { 
            
            case "1":
                QiangHuaLvIcon_1.SetActive(true);
                break;

            case "2":
                QiangHuaLvIcon_2.SetActive(true);
                break;

            case "3":
                QiangHuaLvIcon_3.SetActive(true);
                break;

        }

    }
}
