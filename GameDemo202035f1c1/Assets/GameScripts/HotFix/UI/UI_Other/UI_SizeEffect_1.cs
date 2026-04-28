using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SizeEffect_1 : MonoBehaviour {

    private float size = 1;
    public float fangdaValue;
    private float sizeTimeSum;
    private bool sizeStatus;   //true表示缩小  false表示放大
    // Use this for initialization
    void Start()
    {
        //fangdaValue = 1.5f;

    }

    // Update is called once per frame
    void Update()
    {
        if (sizeStatus == false)
        {
            sizeTimeSum = sizeTimeSum + Time.deltaTime;
            if(sizeTimeSum>= 1)
            {
                sizeTimeSum = 1;
                sizeStatus = true;
            }
        }
        else {
            sizeTimeSum = sizeTimeSum - Time.deltaTime;
            if (sizeTimeSum <= 0)
            {
                sizeTimeSum = 0;
                sizeStatus = false;
            }
        }


        size = 1 + sizeTimeSum * (fangdaValue-1);
        this.gameObject.transform.localScale = new Vector3(size, size, size);
    }
}
