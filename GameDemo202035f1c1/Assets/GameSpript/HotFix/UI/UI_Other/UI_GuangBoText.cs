using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GuangBoText : MonoBehaviour {

    public float StopTime;
    public float StopTimeSum;
    public float MoveTimeSum;
	// Use this for initialization
	void Start () {
        //StopTime = 3.0f;
    }
	
	// Update is called once per frame
	void Update () {

        StopTimeSum = StopTimeSum + Time.deltaTime;
        if (StopTimeSum >= StopTime) {
            MoveTimeSum = MoveTimeSum + Time.deltaTime;
            //开始出发上移
            this.transform.localPosition = new Vector3(this.transform.localPosition.x - 200 * Time.deltaTime, this.transform.localPosition.y, this.transform.localPosition.z);
            //this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 2 * MoveTimeSum, this.transform.localPosition.z);
        }
        //
        if (StopTimeSum >= 12) {
            Destroy(this.gameObject);
        }

    }
}
