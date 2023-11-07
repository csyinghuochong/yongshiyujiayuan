using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_JingLingHintObj : MonoBehaviour {
    public string JingLingID;
    public GameObject Obj_JingLingName;
    public GameObject Obj_JingLingDes;
    public GameObject Obj_JingLingIcon;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //展示成就
    public void ShowJingLing() {

        string JingLingName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", JingLingID, "Spirit_Template");
        string JingLingDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", JingLingID, "Spirit_Template");
        string icon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", JingLingID, "Spirit_Template");

        Obj_JingLingName.GetComponent<Text>().text = "激活精灵:" + JingLingName;
        Obj_JingLingDes.GetComponent<Text>().text = JingLingDes;

        //显示Icon
        object obj = Resources.Load("JingLingIcon/" + icon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_JingLingIcon.GetComponent<Image>().sprite = itemIcon;


        //展示5秒
        Destroy(this.gameObject,5);
    }

}
