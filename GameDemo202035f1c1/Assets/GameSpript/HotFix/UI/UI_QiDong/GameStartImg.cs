using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartImg : MonoBehaviour
{

    public GameObject[] Obj_StartImg;
    public int nowShowImgNum;

    public float startShowEnterSum;     //进入时间
    public bool startShowEnterStatus;   //进入状态
    public float startShowSum;              //停留时间
    public bool startShowStatus;            //停留状态
    public float startShowTimeSum;          //消失时间
    public bool startShowStopStatus;        //下一次输出现状态
    public float startShowStopTimeSum;      //下一次启动图出现时间

    public GameObject Obj_BackMusic;

	// Use this for initialization
	void Start () {
        nowShowImgNum = 0;
        Obj_StartImg[nowShowImgNum].SetActive(true);

	}
	
	// Update is called once per frame
	void Update () {

        //渐变进入
        if (!startShowEnterStatus)
        {
            startShowEnterSum = startShowEnterSum + Time.deltaTime;
            float touMingImgValue = startShowEnterSum / 1;
            if (touMingImgValue > 1)
            {
                touMingImgValue = 1;
            }
            
            Obj_StartImg[nowShowImgNum].GetComponent<Image>().color = new Color(1, 1, 1, touMingImgValue);

            if (startShowEnterSum > 1)
            {
                startShowEnterStatus = true;
            }
        }

        //停留时间
        if (startShowEnterStatus)
        {
            startShowSum = startShowSum + Time.deltaTime;
            if (startShowSum > 1.5)
            {
                startShowStatus = true;
            }
        }

        //渐变消失
        if (startShowStatus) {

            startShowTimeSum = startShowTimeSum + Time.deltaTime;
            float touMingImgValue = startShowTimeSum / 1;
            if (touMingImgValue > 1)
            {
                touMingImgValue = 1;
            }

            Obj_StartImg[nowShowImgNum].GetComponent<Image>().color = new Color(1, 1, 1, 1 - touMingImgValue);
            Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_StartImg[nowShowImgNum]);

            if (touMingImgValue >= 1)
            {
                startShowStopStatus = true;
            }
        }

        if (startShowStopStatus) {
            startShowStopTimeSum = startShowStopTimeSum + Time.deltaTime;
        }

        //下次出现间隔
        if (startShowStopTimeSum>=1.0f)
        {
            if (nowShowImgNum >= Obj_StartImg.Length - 1)
            {
                Destroy(this.gameObject);
            }
            else {

                Obj_StartImg[nowShowImgNum].SetActive(false);
                nowShowImgNum = nowShowImgNum + 1;
                Obj_StartImg[nowShowImgNum].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                Obj_StartImg[nowShowImgNum].SetActive(true);

                startShowEnterStatus = false;
                startShowEnterSum = 0;
                startShowSum = 0;
                startShowStatus = false;
                startShowTimeSum = 0;
                startShowStopStatus = false;
                startShowStopTimeSum = 0;
            }
        }
	}

    public void click() {
        Debug.Log("点击取消!" + Time.time);
        if (Time.time >= 5f) {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Obj_BackMusic != null)
        {
            Obj_BackMusic.SetActive(true);
        }
    }
}
