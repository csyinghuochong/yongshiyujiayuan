using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MailList : MonoBehaviour {

	public string MailID;
	public GameObject Obj_MailName;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Btn_ClickMail(){
		Debug.Log ("我点击了邮箱列表");
		Game_PublicClassVar.Get_gameServerObj.Obj_Mail.GetComponent<UI_Mail>().SelectMailID = MailID;
		Game_PublicClassVar.Get_gameServerObj.Obj_Mail.GetComponent<UI_Mail>().Btn_ShowMailData();
	}
}
