using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ShowDataListSet : MonoBehaviour {

	public string Player_ZhangHaoID;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//展示装备
	public void Btn_ShowEquip(){

		Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001091, Player_ZhangHaoID);
	}

	//展示宠物
	public void Btn_ShowPet(){

		Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001093, Player_ZhangHaoID);
	}

    //关闭界面
    public void Btn_Close() {
        Destroy(this.gameObject);
    }


}
