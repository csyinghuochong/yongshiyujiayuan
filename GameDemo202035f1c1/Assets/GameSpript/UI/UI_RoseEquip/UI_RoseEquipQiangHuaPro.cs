using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_RoseEquipQiangHuaPro : MonoBehaviour {

    public ObscuredString QiangHuaProStr;
    public GameObject Obj_QiangHuaProText;
    public bool UpdateProStatus;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (UpdateProStatus) {
            UpdateProStatus = false;
            Obj_QiangHuaProText.GetComponent<Text>().text = QiangHuaProStr;
        }
        
	}
}
