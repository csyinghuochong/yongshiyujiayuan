using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_RanSeList : MonoBehaviour {

	public ObscuredString RanSeID;
	public GameObject Obj_Par;
	public GameObject Obj_YanSeIconShow;
	public GameObject Obj_YanSeNameShow;
    public GameObject Obj_YanSeSelectImg;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (Obj_Par.GetComponent<UI_RanSeSet>().NowSelectRanSeID == RanSeID)
        {
            Obj_YanSeSelectImg.SetActive(true);
        }
        else {
            Obj_YanSeSelectImg.SetActive(false);
        }

	}

    //初始化
	public void Init() {

		string ranSeName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", RanSeID, "RanSe_Template");
		string ranSeIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeIcon", "ID", RanSeID, "RanSe_Template");

		Obj_YanSeNameShow.GetComponent<Text>().text = ranSeName;

        //测试
        //ranSeIcon = "10001";

        object obj = Resources.Load("OtherIcon/YanSe/" + ranSeIcon, typeof(Sprite));
		Sprite itemIcon = obj as Sprite;
		Obj_YanSeIconShow.GetComponent<Image>().sprite = itemIcon;

	}

    public void Btn_Select() {

        Obj_Par.GetComponent<UI_RanSeSet>().NowSelectRanSeID = RanSeID;
        Obj_Par.GetComponent<UI_RanSeSet>().SelectRanSe();

    }
}
