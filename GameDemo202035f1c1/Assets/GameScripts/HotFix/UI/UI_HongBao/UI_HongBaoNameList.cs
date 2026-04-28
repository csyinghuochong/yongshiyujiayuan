using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HongBaoNameList : MonoBehaviour {

    public string HongBaoName;
    public string HongBaoValue;

    public GameObject Obj_HongBaoName;
    public GameObject Obj_HongBaoValue;

    // Use this for initialization
    void Start () {
        HongBaoName = "玩家*****";
        Obj_HongBaoName.GetComponent<Text>().text = HongBaoName;
        //string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_422");
        Obj_HongBaoValue.GetComponent<Text>().text = HongBaoValue;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
