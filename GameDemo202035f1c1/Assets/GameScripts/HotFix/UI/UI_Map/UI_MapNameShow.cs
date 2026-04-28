using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MapNameShow : MonoBehaviour {

    public GameObject Obj_MapName;
    public string mapName;
    private float moveDis;
    private float nowDis;
    private float timeSum;
    private Vector3 chushiVec3;
    private bool stopMoveStatus;
    private bool stopMoveStart;
    public bool stopMoveEnd;

    // Use this for initialization
    void Start () {
        Obj_MapName.GetComponent<Text>().text = mapName;
        moveDis = 1000;
        this.gameObject.transform.localPosition = new Vector3(-1*moveDis, 0, 0);
        chushiVec3 = this.gameObject.transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        if (stopMoveStart)
        {
            if (!stopMoveStatus)
            {
                timeSum = timeSum + Time.deltaTime * 1.5f;
                nowDis = chushiVec3.x + moveDis * timeSum;

                if (!stopMoveEnd) {
                    if (nowDis>=0) {
                        nowDis = 0;
                    }
                }
                this.gameObject.transform.localPosition = new Vector3(nowDis, 0, 0);
                if (timeSum >= 1)
                {
                    stopMoveStatus = true;
                    timeSum = 0;
                }
            }
            else
            {
                stopMoveEnd = true;
                timeSum = timeSum + Time.deltaTime * 1.5f;
                //停顿3秒
                if (timeSum >= 2.5f)
                {
                    stopMoveStatus = false;
                    timeSum = 0;
                    chushiVec3 = this.gameObject.transform.localPosition;
                    Destroy(this.gameObject, 1);        //2秒后销毁
                }
            }
        }
        else {
            //实例化后 延迟2秒显示
            timeSum = timeSum + Time.deltaTime;
            if (timeSum >= 2) {
                timeSum = 0;
                stopMoveStart = true;
            }
        }

    }
}
