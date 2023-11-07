using UnityEngine;
using System.Collections;

public class Btn_GanXieMingDan : MonoBehaviour {

    public GameObject obj;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //点击感谢按钮
    public void Btn_GanXie()
    {
        if (obj.active == false)
        {
            obj.SetActive(true);
        }
        else {
            obj.SetActive(false);
        }
    }

    //DebugLog
    public void Btn_DebugLog() {
        GameObject logObj = (GameObject)Resources.Load("UGUI/UISet/Other/UI_ErrorLog", typeof(GameObject));
        GameObject errorLogObj = (GameObject)Instantiate(logObj);
        errorLogObj.transform.SetParent(GameObject.Find("Canvas").transform);
        errorLogObj.transform.localPosition = Vector3.zero;
        errorLogObj.transform.localScale = new Vector3(1, 1, 1);
    }
}
