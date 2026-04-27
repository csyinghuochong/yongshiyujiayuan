using UnityEngine;
using System.Collections;

public class UI_BeiFenData : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Btn_Click() { 

        Game_PublicClassVar.Get_wwwSet.IfSaveGetRoseData = true;
    }
}
