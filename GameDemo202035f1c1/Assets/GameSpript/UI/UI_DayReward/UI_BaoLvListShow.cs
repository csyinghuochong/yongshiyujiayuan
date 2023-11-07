using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_BaoLvListShow : MonoBehaviour {

    public ObscuredInt BaoLvID;

    public GameObject Obj_BaoLvName;
    public GameObject Obj_BaoLvProValue;
    public GameObject UI_ShowSelect;
    public GameObject UI_ShowParObj;
    public GameObject UI_ShowBaoLvIcon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateShow() {

        if (UI_ShowParObj.GetComponent<UI_BaoLvShowSet>().BaoLvID == BaoLvID)
        {
            //Debug.Log("BaoLvID = " + BaoLvID);
            UI_ShowSelect.SetActive(true);
            UI_ShowBaoLvIcon.GetComponent<Image>().material = null;
        }
        else{

            UI_ShowSelect.SetActive(false);

            //置灰
            object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
            Material huiMaterial = huiObj as Material;
            UI_ShowBaoLvIcon.GetComponent<Image>().material = huiMaterial;
            //Obj_ItemName.GetComponent<Text>().color = new Color(0.55f, 0.55f, 0.55f, 1);
        }
    }
}
