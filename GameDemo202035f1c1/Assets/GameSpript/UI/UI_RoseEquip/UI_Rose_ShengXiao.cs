using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Rose_ShengXiao : MonoBehaviour {

    public bool UpdateStatus;
    public GameObject[] ObjShengXiaoList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (UpdateStatus) {
            UpdateStatus = false;
            for (int i = 0; i < ObjShengXiaoList.Length; i++) {
                if (ObjShengXiaoList[i] != null) {
                    ObjShengXiaoList[i].GetComponent<UI_RoseEquipShow>().UpdataStatus = true;
                }
            }
        }

	}

    public void Btn_CloseUI() {
        this.gameObject.SetActive(false);
    }
}
