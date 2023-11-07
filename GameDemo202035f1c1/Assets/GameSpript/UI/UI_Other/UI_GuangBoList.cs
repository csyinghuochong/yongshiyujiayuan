using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuangBoList: MonoBehaviour {
    
    public int GuangBoSumNum;
    public Dictionary<int, string> GuangboTextStr = new Dictionary<int, string>();
    public int GuangBoNowNum;
    public GameObject Obj_GuangBo;            //掉落物体的UI
    public GameObject Obj_GuangBoSet;
    public GameObject Obj_Back;


    public float PlayTimeSum;

    public bool test;
    // Use this for initialization
    void Start () {
        PlayTimeSum = 5;
        Game_PublicClassVar.Get_gameServerObj.Obj_GuangBoList = this.gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        PlayTimeSum = PlayTimeSum + Time.deltaTime;

        //判定是否有广播内容
        if (GuangboTextStr.Count >= 1)
        {
            Obj_Back.SetActive(true);
            if (PlayTimeSum >= 5) {
                PlayTimeSum = 0;
                GuangBoNowNum = GuangBoNowNum + 1;
                if (GuangboTextStr.ContainsKey(GuangBoNowNum)) {
                    string guangboText = GuangboTextStr[GuangBoNowNum];
                    GameObject guangboObj = (GameObject)Instantiate(Obj_GuangBo);
                    guangboObj.transform.SetParent(Obj_GuangBoSet.transform);
                    guangboObj.transform.localPosition = new Vector3(1800, 1, 1);
                    guangboObj.transform.localScale = new Vector3(1, 1, 1);
                    guangboObj.GetComponent<Text>().text = guangboText;
                    GuangboTextStr.Remove(GuangBoNowNum);
                }
            }
        }
        else
        {
            if (Obj_Back.transform.childCount <= 0) {
                Obj_Back.SetActive(false);
            }
        }

        if (test) {
            AddGuangBo("我是一条小小广播");
            test = false;
        }


    }

    //添加广播内容
    public void AddGuangBo(string guangboText) {
        GuangBoSumNum = GuangBoSumNum + 1;
        GuangboTextStr.Add(GuangBoSumNum, guangboText);
    }

    public void IfHide() {
        
    }
}
