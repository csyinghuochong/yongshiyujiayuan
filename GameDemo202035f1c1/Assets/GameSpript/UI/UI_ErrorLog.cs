using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ErrorLog : MonoBehaviour {

    public GameObject logStrObj;
    private float jishuNum;
	// Use this for initialization
	void Start () {
        //Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr = "1\n1\n1\n1\n1\n1\n1\n1\n1\n1\n1\n1\n";
        jishuNum = 1;

	}
	
	// Update is called once per frame
	void Update () {
        jishuNum = jishuNum + Time.deltaTime;
        if (jishuNum >= 1) {
            jishuNum = 0;
            //string[] logStrList = Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr.Split(';');
            logStrObj.GetComponent<Text>().text = Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr;
            if (Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr == "") {
                logStrObj.GetComponent<Text>().text = "暂无任何Log信息！";
            }
        }
	}

    public void Close() {
        Destroy(this.gameObject);
    }
}
