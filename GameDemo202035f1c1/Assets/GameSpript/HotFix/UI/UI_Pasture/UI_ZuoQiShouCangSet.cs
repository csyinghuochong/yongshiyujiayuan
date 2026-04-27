using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ZuoQiShouCangSet : MonoBehaviour {

    public GameObject Obj_ZuoQiShouCangShowListSet;
    public GameObject Obj_ZuoQiShouCangShowList;

    // Use this for initialization
    void Start () {

        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ZuoQiModelShow.SetActive(true);

        Init();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ZuoQiModelShow.SetActive(false);
    }

    //初始化
    public void Init() {

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ZuoQiShouCangShowListSet);

        string showListStr = "10001,10007,10002,10003,10004,10005,10006,10008,10009";
        string[] showList = showListStr.Split(',');

        for (int i = 0; i < showList.Length; i++) {
            GameObject obj = (GameObject)Instantiate(Obj_ZuoQiShouCangShowList);
            obj.transform.SetParent(Obj_ZuoQiShouCangShowListSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_ZuoQiShouCangShowList>().ZuoQiShowID = showList[i];
            obj.GetComponent<UI_ZuoQiShouCangShowList>().Init();
        }

    }
}
