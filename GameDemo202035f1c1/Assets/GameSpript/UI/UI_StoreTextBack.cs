using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_StoreTextBack : MonoBehaviour
{

    public string ExitType;                 //0: 点击退出  1:固定时间退出  2:
    public float UIShowTime;                //ExitType为1时调用,表示停顿多少秒后退出
    private float uiShowTimeSum;            //退出时间累计值
    public GameObject Obj_StoreText;
    public string StoreText;

    private int StoreTextSum;
    private int StoreTextLength;
    private bool StoreTextUpdata;
    private float StoreTextUpdataSum;

	// Use this for initialization
	void Start () {

        //ExitType = "1";
        UIShowTime = 3;
        StoreTextLength = StoreText.Length;
        //显示文字
        if (StoreText != "") {
            //Debug.Log("StoreText = " + StoreText);
            Obj_StoreText.GetComponent<Text>().text = StoreText;
            //RectTransform rect = new Vector3(0.5f,0.5f,0);
            //Obj_StoreText.GetComponent<Text>().alignment.
        }
        
	}
	
	// Update is called once per frame
	void Update () {

        switch (ExitType) { 
        
            //点击退出
            case "0":

                //鼠标点击,隐藏此界面
                if (Input.GetMouseButton(0)) {
                    recoveryTime();     //恢复暂停的游戏
                    Destroy(this.gameObject);
                }

            break;

            //固定时间退出
            case "1":

                //等待时间,结束暂停游戏
                uiShowTimeSum = uiShowTimeSum + Time.deltaTime;
                if (uiShowTimeSum >= UIShowTime) {
                    recoveryTime();     //恢复暂停的游戏
                    Destroy(this.gameObject);
                }
                    
            break;

            //逐个蹦字
            case "2":
                if (!StoreTextUpdata) {
                    if (StoreTextUpdataSum >= 0.05f)
                    {
                        StoreTextUpdataSum = 0;
                        StoreTextSum = StoreTextSum + 1;
                        Obj_StoreText.GetComponent<Text>().text = StoreText.Substring(0, StoreTextSum);
                        //Debug.Log(StoreTextSum + "长度" + StoreTextLength + "TEXT = " + StoreText.Substring(1, StoreTextSum));
                    }
                    else {

                        StoreTextUpdataSum = Time.deltaTime + StoreTextUpdataSum;
                    }

                }
                else
                {
                    //鼠标点击,隐藏此界面
                    if (Input.GetMouseButton(0))
                    {
                        Destroy(this.gameObject);

                        //更新故事章节

                    }
                }

                if (StoreTextSum >= StoreTextLength-1) {
                    StoreTextUpdata = true;
                    StoreTextSum = 0;
                }

            break;
        
        }
	}

    //判定当前游戏暂停的话,将重新激活游戏
    void recoveryTime() {

        if (Time.timeScale == 0) {
            Time.timeScale = 1;
        }
    }
}
