using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_XiLianDaShiListShow : MonoBehaviour {

    public ObscuredString XiLianDaShiID;
    public ObscuredBool ifJIHuoStatus;
    public ObscuredInt XiLianXuHao;

    public GameObject[] Obj_JiHuoList;
    public GameObject Obj_JiHuoImg;
    public GameObject Obj_JiHuoHintImg;
    public GameObject Obj_XiLianDaShiStr;
    public GameObject Obj_XiLianDaShiDes;
    public GameObject Obj_WeiJiHuo;
    public GameObject Obj_Name;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //展示数据
    public void showData() {

        //显示名称
        string xiLianName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", XiLianDaShiID, "EquipXiLianDaShi_Template");
        Obj_Name.GetComponent<Text>().text = xiLianName;

        //隐藏
        for (int i = 0; i < Obj_JiHuoList.Length; i++) {
            Obj_JiHuoList[i].SetActive(false);
        }

        //显示
        Obj_JiHuoImg = Obj_JiHuoList[XiLianXuHao];
        Obj_JiHuoImg.SetActive(true);
        if (ifJIHuoStatus)
        {
            Obj_JiHuoHintImg.SetActive(true);
            Obj_WeiJiHuo.SetActive(false);
        }
        else {
            Obj_JiHuoHintImg.SetActive(false);
            //变灰
            object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
            Material huiMaterial = huiObj as Material;
            Obj_JiHuoImg.GetComponent<Image>().material = huiMaterial;
            Obj_JiHuoImg.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            Obj_XiLianDaShiDes.GetComponent<Text>().color = new Color(0.55f,0.55f,0.39f);
            Obj_WeiJiHuo.SetActive(true);
        }

        string needXiLianValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedXiLianValue", "ID", XiLianDaShiID, "EquipXiLianDaShi_Template");
        string langString = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("熟练度达到");
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("点");
        Obj_XiLianDaShiStr.GetComponent<Text>().text = langString + "<color=#2D882EFF>" + needXiLianValue + langStr + "</color>";

        string desStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", XiLianDaShiID, "EquipXiLianDaShi_Template");
        Obj_XiLianDaShiDes.GetComponent<Text>().text = desStr;

    }
}
