using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PastureSellSet : MonoBehaviour {

    public GameObject PastureSellListObjSet;
    public GameObject PastureSellListObj;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init()
    {
        Debug.Log("初始化...");

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(PastureSellListObjSet);

        string pastutreSetStr = "10001;10002;10003;10004;10005;10006;10007;10008;10009;10010;10011;10012";
        string[] pastutreSetStrList = pastutreSetStr.Split(';');
        for (int i = 0; i < pastutreSetStrList.Length; i++) {

            //实例化
            GameObject nowObj = (GameObject)Instantiate(PastureSellListObj);
            nowObj.transform.SetParent(PastureSellListObjSet.transform);
            nowObj.transform.localScale = new Vector3(1, 1, 1);
            nowObj.GetComponent<UI_PastureSellList>().PastureID = pastutreSetStrList[i];
            nowObj.GetComponent<UI_PastureSellList>().Init();
        }

    }
}
