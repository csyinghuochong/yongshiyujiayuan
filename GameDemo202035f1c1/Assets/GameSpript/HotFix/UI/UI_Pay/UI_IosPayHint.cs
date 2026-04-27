using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_IosPayHint : MonoBehaviour {

    public GameObject LodingImg;
    float value;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        value = value + 360.0f * Time.deltaTime;
        LodingImg.transform.localRotation =  Quaternion.Euler(new Vector3(0, 0, value)) ;

    }
}
