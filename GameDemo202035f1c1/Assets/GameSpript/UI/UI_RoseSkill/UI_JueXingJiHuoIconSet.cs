using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_JueXingJiHuoIconSet : MonoBehaviour {

    public ObscuredString JueXingID;
    public GameObject Obj_Par;
    public GameObject Obj_JueXingIcon;
    public GameObject Obj_JueXingName;
    public GameObject Obj_JueXingSelectImg;

	// Use this for initialization
	void Start () {
        Init();

    }
	
	// Update is called once per frame
	void Update () {

        if (Obj_Par != null) {
            if (Obj_Par.GetComponent<UI_JueXingSet>().nowJueXingID == JueXingID)
            {
                Obj_JueXingSelectImg.SetActive(true);
            }
            else {
                Obj_JueXingSelectImg.SetActive(false);
            }
        }

	}

    public void Init() {

        //显示名字
        string name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", JueXingID, "JueXing_Template");
        Obj_JueXingName.GetComponent<Text>().text = name;

        //显示图标
        string iconOcc = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", JueXingID, "JueXing_Template");

        string icon = iconOcc.Split('@')[0];
        if (iconOcc.Split('@').Length >= 2)
        {
            switch (Game_PublicClassVar.Get_function_Rose.GetRoseOcc())
            {
                case "1":
                    icon = iconOcc.Split('@')[0];
                    break;
                case "2":
                    icon = iconOcc.Split('@')[1];
                    break;
                case "3":
                    icon = iconOcc.Split('@')[2];
                    break;
            }
        }

        object obj = Resources.Load("SkillIcon/" + icon, typeof(Sprite));
        Sprite skillIconSp = obj as Sprite;
        Obj_JueXingIcon.GetComponent<Image>().sprite = skillIconSp;
        //查询自身是否激活
        string jihuoIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (jihuoIDStr.Contains(JueXingID)== false)
        {
            //未激活显示灰色状态
            object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
            Material huiMaterial = huiObj as Material;
            Obj_JueXingIcon.GetComponent<Image>().material = huiMaterial;
        }
    }

    public void Btn_Click() {
        if (Obj_Par != null) {
            Obj_Par.GetComponent<UI_JueXingSet>().nowJueXingID = JueXingID;
            Obj_Par.GetComponent<UI_JueXingSet>().Btn_SelectJueXing();
        }
    }
}
