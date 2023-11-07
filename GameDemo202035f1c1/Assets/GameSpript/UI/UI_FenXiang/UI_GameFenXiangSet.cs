using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameFenXiangSet : MonoBehaviour {

    public GameObject Obj_FenXiangSet;
    public GameObject Obj_XuLieHaoSet;

    public GameObject Obj_Btn_FenXiang;
    public GameObject Obj_Btn_XuLieHao;

    public GameObject Obj_Btn_FenXiangText;
    public GameObject Obj_Btn_XuLieHaoText;

    public GameObject[] FenXiangObjList;

    // Use this for initialization
    void Start () {

        //默认打开是分享
        Btn_FenXiang();

        Debug.Log("包名:" + Application.identifier);
        if(Application.identifier.Contains(".mi") || Application.identifier.Contains(".mi"))
        {
            FenXiangObjList[0].SetActive(false);
            FenXiangObjList[2].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Btn_FenXiang()
    {
        Obj_FenXiangSet.SetActive(true);
        Obj_XuLieHaoSet.SetActive(false);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_23_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_Btn_FenXiang.GetComponent<Image>().sprite = img;
        Obj_Btn_FenXiangText.GetComponent<Text>().color = new Color(0.52f,0.31f,0);

        //显示按钮
        obj = Resources.Load("GameUI/" + "Btn/Btn_23_1", typeof(Sprite));
        img = obj as Sprite;
        Obj_Btn_XuLieHao.GetComponent<Image>().sprite = img;
        Obj_Btn_XuLieHaoText.GetComponent<Text>().color = new Color(1, 1, 1);
    }

    public void Btn_XuLieHao()
    {
        Obj_FenXiangSet.SetActive(false);
        Obj_XuLieHaoSet.SetActive(true);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_23_1", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_Btn_FenXiang.GetComponent<Image>().sprite = img;
        Obj_Btn_FenXiangText.GetComponent<Text>().color = new Color(1, 1, 1);
        //显示按钮
        obj = Resources.Load("GameUI/" + "Btn/Btn_23_2", typeof(Sprite));
        img = obj as Sprite;
        Obj_Btn_XuLieHao.GetComponent<Image>().sprite = img;
        Obj_Btn_XuLieHaoText.GetComponent<Text>().color = new Color(0.52f, 0.31f, 0);
    }
}
