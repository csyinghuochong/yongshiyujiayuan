using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ServerListShow : MonoBehaviour {


    public GameObject Obj_ServerName;
    public GameObject Obj_ServerTitle;
    public GameObject Obj_ServerStatus;
    public GameObject Obj_ServerTime;

    public string ServerName;
    public string ServerStatus;
    public string ServerTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init() {

        Obj_ServerName.GetComponent<Text>().text = ServerName;

        Color nowColor = new Color(0,0,0);

        string nowServerTypeDes = "";
        string nowServerTitle = "";
        string nowServerTimeDes = "";

        switch (ServerStatus) {
            //默认老服
            case "0":
                nowColor = new Color(1, 1, 1);
                nowServerTypeDes = "正常开放";
                nowServerTimeDes = "正常开放";
                nowServerTitle = "";
                break;
            //当前最新服务器
            case "1":
                nowColor = new Color(0, 1, 0);
                nowServerTypeDes = "正常开放";
                nowServerTimeDes = "新服开启";
                nowServerTitle = "新服";
                break;
            //预计开启服务器
            case "2":
                nowColor = new Color(0.7f, 0.7f, 0.7f);
                DateTime nowData = Game_PublicClassVar.Get_wwwSet.GetTime(ServerTime);
                string nowMinstr = nowData.Minute.ToString();
                if (nowMinstr == "0") {
                    nowMinstr = "00";
                }
                nowServerTimeDes = nowData.Month + "月" + nowData.Day + "日" + nowData.Hour + ":" + nowMinstr + "点";
                nowServerTypeDes = "暂未开放";
                nowServerTitle = "预告";
                break;
        }

        Obj_ServerTitle.GetComponent<Text>().text = nowServerTitle;

        Obj_ServerStatus.GetComponent<Text>().text = nowServerTypeDes;
        Obj_ServerTime.GetComponent<Text>().text = nowServerTimeDes;

        Obj_ServerName.GetComponent<Text>().color = nowColor;
        Obj_ServerTitle.GetComponent<Text>().color = nowColor;
        Obj_ServerStatus.GetComponent<Text>().color = nowColor;
        Obj_ServerTime.GetComponent<Text>().color = nowColor;
    }
}
