using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameYinEffect : MonoBehaviour {

    public string Type;  //1表示左右移动  2表示上下移动  
    public float ObjProMaxValue;
    private string ObjProType;
    private float ObjNowProValue;

    // Use this for initialization
    void Start () {

        ObjProType = "1";

    }
	
	// Update is called once per frame
	void Update () {
        if (ObjProType == "1")
        {
            ObjNowProValue = ObjNowProValue + Time.deltaTime * 100;
        }
        if (ObjProType == "2")
        {
            ObjNowProValue = ObjNowProValue - Time.deltaTime * 100;
        }

        if (ObjNowProValue > ObjProMaxValue)
        {
            ObjNowProValue = ObjProMaxValue;
            ObjProType = "2";
        }

        if (ObjNowProValue < 0)
        {
            ObjNowProValue = 0;
            ObjProType = "1";
        }

        if (Type == "1")
        {
            this.transform.localPosition = new Vector3(ObjNowProValue, 0, 0);
        }

        if (Type == "2")
        {
            this.transform.localPosition = new Vector3(0, ObjNowProValue, 0);
        }
    }
}
