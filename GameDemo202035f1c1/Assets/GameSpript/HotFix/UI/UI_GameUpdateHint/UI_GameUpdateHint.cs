using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameUpdateHint : MonoBehaviour {

    public string ShowUpdateText;
    public string ShowUpdateUrl;
    public GameObject Obj_ShowText;

	// Use this for initialization
	void Start () {

        ShowUpdateText = System.Text.RegularExpressions.Regex.Unescape(ShowUpdateText);       //有“/n”的处理为换行符
        Obj_ShowText.GetComponent<Text>().text = ShowUpdateText;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //点击前往更新
    public void Btn_GoUpdate() {

        Application.OpenURL(ShowUpdateUrl);

    }
}
