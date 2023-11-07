using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PastureXinXiSet : MonoBehaviour {

    public GameObject Obj_XinXiShowListSet;
    public GameObject Obj_XinXiShowList;


    // Use this for initialization
    void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_XinXiShowListSet);

        //展示信息
        string PastureGongGao = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GongGao", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] gonggaoList = PastureGongGao.Split(';');

        for (int i = 0; i < gonggaoList.Length; i++)
        {
            if (gonggaoList[i] != "") {
                GameObject obj = (GameObject)Instantiate(Obj_XinXiShowList);
                obj.transform.SetParent(Obj_XinXiShowListSet.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.GetComponent<UI_PastureXinXiList>().desXinXi = gonggaoList[i];
                obj.GetComponent<UI_PastureXinXiList>().Init();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CloseUI() {
        Destroy(this.gameObject);
    }
}
