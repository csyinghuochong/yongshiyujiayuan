using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ServerListShowSet : MonoBehaviour {

    public Pro_ServerListDataSet ProServerListDataSet;
    public string GameServerStr;
    public GameObject Obj_ServerListSet;
    public GameObject Obj_ServerList;
    // Use this for initialization
    void Start () {

        //申请获取服务器列表
        if (Game_PublicClassVar.gameServerObj.ProServerListDataSet == null) {
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000120, "");
        }

        Game_PublicClassVar.Get_gameServerObj.Obj_ServerListShow = this.gameObject;
        //Init();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init() {

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ServerListSet);

        ProServerListDataSet = Game_PublicClassVar.gameServerObj.ProServerListDataSet;

        for (int i = 0; i < ProServerListDataSet.ProServerListData.Count; i++)
        {
            GameObject serverObj = (GameObject)Instantiate(Obj_ServerList);
            serverObj.transform.SetParent(Obj_ServerListSet.transform);
            serverObj.transform.localScale = new Vector3(1, 1, 1);
            serverObj.GetComponent<UI_ServerListShow>().ServerName = ProServerListDataSet.ProServerListData[i.ToString()].ServerName;
            serverObj.GetComponent<UI_ServerListShow>().ServerStatus = ProServerListDataSet.ProServerListData[i.ToString()].ServerStatus;
            serverObj.GetComponent<UI_ServerListShow>().ServerTime = ProServerListDataSet.ProServerListData[i.ToString()].ServerTime;
            serverObj.GetComponent<UI_ServerListShow>().Init();
        }

    }

    //关闭隐藏自己
    public void Btn_Close() {

        this.gameObject.SetActive(false);

    }
}
