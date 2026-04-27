using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QianDaoItemShow : MonoBehaviour {

    public string QianDaoID;
    public GameObject Obj_QianDaoShowImg_1;
    public GameObject Obj_QianDaoShowImg_2;
    public GameObject Obj_QianDaoShowImg_3;
    public GameObject Obj_QianDaoShowImg_4;
    public GameObject Obj_QianDaoShowImg_YilingQu;
    public GameObject Obj_QianDaoShowText;
    public GameObject Obj_XuanZhongImg;
    public GameObject Obj_QianDaoParObj;        //签到父级

    // Use this for initialization
    void Start() {

        Init();

    }

    public void Init() {
        //显示签到信息
        string qianDaoText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowText", "ID", QianDaoID, "QianDao_Template");
        Obj_QianDaoShowText.GetComponent<Text>().text = qianDaoText;

        //清理显示
        Obj_QianDaoShowImg_1.SetActive(false);
        Obj_QianDaoShowImg_2.SetActive(false);
        Obj_QianDaoShowImg_3.SetActive(false);
        Obj_QianDaoShowImg_4.SetActive(false);

        //显示签到图片
        string qianDaoType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowType", "ID", QianDaoID, "QianDao_Template");
        switch (qianDaoType)
        {
            case "1":
                Obj_QianDaoShowImg_1.SetActive(true);
                break;
            case "2":
                Obj_QianDaoShowImg_2.SetActive(true);
                break;
            case "3":
                Obj_QianDaoShowImg_3.SetActive(true);
                break;
            case "4":
                Obj_QianDaoShowImg_4.SetActive(true);
                break;
        }

        //显示当前是否已经签到
        Obj_QianDaoShowImg_YilingQu.SetActive(false);
        /*
        string qianDaoDay = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoDay", "ID", QianDaoID, "QianDao_Template");
        string qianDaoNum_Com_Day = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com_Day", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string qianDaoNum_Pay_Day = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Pay_Day", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

        if (qianDaoNum_Com_Day=="1"&& qianDaoNum_Pay_Day=="1")
        {
            Obj_QianDaoShowImg_YilingQu.SetActive(true);
        }
        else
        {
            Obj_QianDaoShowImg_YilingQu.SetActive(false);
        }
        */

        string qianDaoDay = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoDay", "ID", QianDaoID, "QianDao_Template");
        string qianDaoNum_Com = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string qianDaoNum_Com_Day = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com_Day", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (qianDaoNum_Com_Day == "0") {
            if (int.Parse(qianDaoDay) - 1 > int.Parse(qianDaoNum_Com))
            {
                object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
                Material huiMaterial = huiObj as Material;
                Obj_QianDaoShowImg_1.GetComponent<Image>().material = huiMaterial;
                Obj_QianDaoShowImg_2.GetComponent<Image>().material = huiMaterial;
                Obj_QianDaoShowImg_3.GetComponent<Image>().material = huiMaterial;
                Obj_QianDaoShowImg_4.GetComponent<Image>().material = huiMaterial;
                Obj_QianDaoShowText.GetComponent<Text>().material = huiMaterial;
            }
        }

        if (qianDaoNum_Com_Day == "1")
        {
            if (int.Parse(qianDaoDay) > int.Parse(qianDaoNum_Com))
            {
                object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
                Material huiMaterial = huiObj as Material;
                Obj_QianDaoShowImg_1.GetComponent<Image>().material = huiMaterial;
                Obj_QianDaoShowImg_2.GetComponent<Image>().material = huiMaterial;
                Obj_QianDaoShowImg_3.GetComponent<Image>().material = huiMaterial;
                Obj_QianDaoShowImg_4.GetComponent<Image>().material = huiMaterial;
                Obj_QianDaoShowText.GetComponent<Text>().material = huiMaterial;
            }
        }


    }

    // Update is called once per frame
    void Update () {
        if (Obj_QianDaoParObj.GetComponent<ZhanQu_QianDao>().NowXuanZhongID == QianDaoID) {
            Obj_XuanZhongImg.SetActive(true);
        }
        else
        {
            Obj_XuanZhongImg.SetActive(false);
        }

        if (Obj_QianDaoParObj.GetComponent<ZhanQu_QianDao>().UpdateStatus) {
            Init();
        }
	}

    public void Btn_QianDaoShow() {

        Obj_QianDaoParObj.GetComponent<ZhanQu_QianDao>().ShowQianDaoReward(QianDaoID);
    }
}
