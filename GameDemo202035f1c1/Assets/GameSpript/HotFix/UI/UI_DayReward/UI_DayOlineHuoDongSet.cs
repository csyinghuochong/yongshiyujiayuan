using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DayOlineHuoDongSet : MonoBehaviour {

    public GameObject Obj_JieShao_HongBao;
    public GameObject Obj_JieShao_BaoXiang;
    public GameObject Obj_JieShao_ShouLie;
    public GameObject Obj_JieShao_Tower;

    public GameObject Obj_Select_HongBao;
    public GameObject Obj_Select_BaoXiang;
    public GameObject Obj_Select_ShouLie;
    public GameObject Obj_Select_Tower;

    // Use this for initialization
    void Start () {

        ClickType("1");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //点击类型
    public void ClickType(string type) {

        Clearn();

        switch (type) {

            //点击红包雨活动
            case "1":
                Obj_JieShao_HongBao.SetActive(true);
                Obj_Select_HongBao.SetActive(true);
                break;

            //点击宝箱活动
            case "2":
                Obj_JieShao_BaoXiang.SetActive(true);
                Obj_Select_BaoXiang.SetActive(true);
                break;

            //点击狩猎活动
            case "3":
                Obj_JieShao_ShouLie.SetActive(true);
                Obj_Select_ShouLie.SetActive(true);
                break;

            //点击魔塔活动
            case "4":
                Obj_JieShao_Tower.SetActive(true);
                Obj_Select_Tower.SetActive(true);
                break;

        }

    }

    public void Clearn() {

        Obj_JieShao_HongBao.SetActive(false);
        Obj_JieShao_BaoXiang.SetActive(false);
        Obj_Select_HongBao.SetActive(false);
        Obj_Select_BaoXiang.SetActive(false);
        Obj_JieShao_ShouLie.SetActive(false);
        Obj_Select_ShouLie.SetActive(false);
        Obj_JieShao_Tower.SetActive(false);
        Obj_Select_Tower.SetActive(false);

    }
}
