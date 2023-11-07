using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AIPropertyShow : MonoBehaviour {


    public GameObject Obj_Show;
    public GameObject Obj_Name;
    public GameObject Obj_Property_Act;
    public GameObject Obj_Property_Mage;
    public GameObject Obj_Property_Def;
    public GameObject Obj_Property_Adf;
    public GameObject Obj_Property_Hp;
    public GameObject Obj_Property_Speed;
    public GameObject Obj_Property_Cri;
    public GameObject Obj_Property_Hit;
    public GameObject Obj_Property_ActDoge;
    public GameObject Obj_Property_MageDoge;


    // Use this for initialization
    void Start () {



    }

    public void Init() {

        Obj_Name.GetComponent<Text>().text = Obj_Show.GetComponent<AI_Property>().AI_Name;
        Obj_Property_Act.GetComponent<Text>().text = Obj_Show.GetComponent<AI_Property>().AI_Act.ToString();
        Obj_Property_Mage.GetComponent<Text>().text = Obj_Show.GetComponent<AI_Property>().AI_MageAct.ToString();
        Obj_Property_Def.GetComponent<Text>().text = Obj_Show.GetComponent<AI_Property>().AI_Def.ToString();
        Obj_Property_Adf.GetComponent<Text>().text = Obj_Show.GetComponent<AI_Property>().AI_Adf.ToString();
        Obj_Property_Hp.GetComponent<Text>().text = Obj_Show.GetComponent<AI_Property>().AI_HpMax.ToString();
        Obj_Property_Speed.GetComponent<Text>().text = Obj_Show.GetComponent<AI_Property>().AI_MoveSpeed.ToString();
        Obj_Property_Cri.GetComponent<Text>().text = (Obj_Show.GetComponent<AI_Property>().AI_Cri * 100).ToString() + "%";
        Obj_Property_Hit.GetComponent<Text>().text = (Obj_Show.GetComponent<AI_Property>().AI_Hit * 100).ToString() + "%";
        Obj_Property_ActDoge.GetComponent<Text>().text = (Obj_Show.GetComponent<AI_Property>().AI_Dodge * 100).ToString() + "%";
        Obj_Property_MageDoge.GetComponent<Text>().text = (Obj_Show.GetComponent<AI_Property>().AI_SkillActDodgePro * 100).ToString() + "%";

    }

    // Update is called once per frame
    void Update () {
		
	}

    //关闭
    public void Btn_Close() {
        Destroy(this.gameObject);
    }

}
