using UnityEngine;
using System.Collections;

public class UI_GameGirdHint : MonoBehaviour {

    public GameObject[] GameHintObj;
    public bool HintStatus;
    public float HintInterval;       //2次提示的间隔
    private float HintIntervalSum;   //2次提示间隔的计数器
    private int nowHintNum;         //当前提示的第几个
    private bool HintOpenStatus;        //单条提示打开状态
    
	// Use this for initialization
	void Start () {
        HintInterval = 0.3f;
        nowHintNum = 0;
	}
	
	// Update is called once per frame
	void Update () {

        //提示状态开启时开始弹出提示
        if (HintStatus) {

            if (nowHintNum < GameHintObj.Length &&  GameHintObj[nowHintNum] != null)
            {
                if (HintOpenStatus)
                {
                    //触发单条提示
                    GameHintObj[nowHintNum].SetActive(true);
                    GameHintObj[nowHintNum].GetComponent<UI_GameGirdHintSingle>().HintStatus = true;
                    nowHintNum = nowHintNum + 1;
                    HintOpenStatus = false;
                    //Debug.Log("开始飘");
                    //删除对应数据
                }

                HintIntervalSum = HintIntervalSum + Time.deltaTime;
                if (HintIntervalSum >= HintInterval)
                {
                    HintOpenStatus = true;
                    HintIntervalSum = 0.0f;  //清空数据
                }
            }
            else { 
                //清空所有数据
                for (int i = 0; i <= GameHintObj.Length - 1; i++) {
                    GameHintObj[i] = null;
                }
                HintStatus = false;
                nowHintNum = 0;
            }
        }
	}
}
