using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CaiJiItem : MonoBehaviour {

    public GameObject Obj_Par;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Obj_Par.GetComponent<UI_GetherItem>() == null) {
            if (Obj_Par.GetComponent<UI_GetherItem>().ScenceItemReviveStatus) {
                Destroy(this.gameObject);
            }
        }

        
        if (Obj_Par == null)
        {
            Destroy(this.gameObject);
        }
        
	}
}
