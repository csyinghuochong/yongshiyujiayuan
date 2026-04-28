using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServerMessage : MonoBehaviour {

    //服务器消息字符串
    public bool ServerMessageStatus;
    public string ServerMessageStr;

    //定义一个委托
    public delegate void DelegateCommon();   // 定义一个委托
    private DelegateCommon commonDel;
    public delegate void DelegateCommonString(string str);   // 定义一个委托
    private DelegateCommonString commonDelStr;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //接受服务器消息
        if (ServerMessageStatus)
        {
            ServerMessageStatus = false;
            string[] ServerMessageStrList = ServerMessageStr.Split(';');
            for (int i = 0; i < ServerMessageStrList.Length; i++) {
                if (ServerMessageStrList[i] != "") {
                    string[] serverList = ServerMessageStrList[i].Split(',');
                    string messageType = serverList[0];
                    switch (messageType)
                    {
                        //发送消息
                        case "10001":
                            Game_PublicClassVar.Get_function_UI.GameGirdHint(serverList[1]);
                            break;

                        //发送道具
                        case "10002":
                            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(serverList[1], int.Parse(serverList[2]),"0",0,"0",true,"3");
                            break;
                    }
                }
            }
            ServerMessageStr = "";
        }

        
        //循环检测


        //测试
        if (commonDelStr != null) {
            BtnWeiTuoTest123();
        }

	}

    public void BtnWeiTuoTest(DelegateCommonString del) {
        Debug.Log("掉落掉落！！！");
        //commonDel = del;
        //commonDel();

        commonDelStr = del;
        //commonDelStr("123123123");
        //commonDelStr = TestWeiTuoHint("AAAAAAAAAAAAAAAA");
    }


    public void BtnWeiTuoTest123()
    {
        //commonDel();
        commonDelStr("456456");
    }

    public void TestWeiTuoHint(string hintStr)
    {
        Debug.Log("测试委托提示");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("测试委托提示：" + hintStr);
    }
}
