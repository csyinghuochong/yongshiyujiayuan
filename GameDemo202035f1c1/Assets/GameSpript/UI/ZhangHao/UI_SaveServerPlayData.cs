using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SavePlayData : MonoBehaviour
{

    public GameObject SaveMiMaObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Btn_SavePlayData(){
        
        string mima = SaveMiMaObj.GetComponent<InputField>().text;
        if (mima != "") { 
            //上传数据


        }
    }
}
