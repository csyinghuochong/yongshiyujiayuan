using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OpenFunctionHint : MonoBehaviour {

    private float timeValue;
    private bool gaibianStatus;
    private float nowSizePro;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!gaibianStatus) {
            timeValue = timeValue + Time.deltaTime;
            nowSizePro = 1 + timeValue / 10;
            this.gameObject.transform.localScale = new Vector3(nowSizePro, nowSizePro, nowSizePro);
            if (timeValue >= 0.5f) {
                gaibianStatus = true;
                timeValue = 0;
            }
        }
        else {
            timeValue = timeValue + Time.deltaTime;
            nowSizePro = nowSizePro - (nowSizePro -1) * (timeValue/0.5f);
            if (nowSizePro <= 1) {
                nowSizePro = 1;
            }
            this.gameObject.transform.localScale = new Vector3(nowSizePro, nowSizePro, nowSizePro);
            if (timeValue >= 0.5f)
            {
                gaibianStatus = false;
                timeValue = 0;
            }
        }
        //Debug.Log("nowSizePro = " + nowSizePro);
    }


    public void Btn_Hint() {
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_150");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你有新的邮件！请赶快前往圣光成的邮件处领取哦~");
    }
}
