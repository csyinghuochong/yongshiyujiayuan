using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PetAddpProperty : MonoBehaviour {

    public ObscuredString RosePetID;

    public GameObject Obj_AddPropretyValue_LiLiang;
    public GameObject Obj_AddPropretyValue_ZhiLi;
    public GameObject Obj_AddPropretyValue_TiZhi;
    public GameObject Obj_AddPropretyValue_NaiLi;

    public GameObject Obj_ShengYuNum;
    public GameObject Obj_AddPropertyShow;

    //当前点数
    public ObscuredInt NowShengYuNum;
    public ObscuredInt NowPropertyValue_LiLiang;
    public ObscuredInt NowPropertyValue_ZhiLi;
    public ObscuredInt NowPropertyValue_TiZhi;
    public ObscuredInt NowPropertyValue_NaiLi;

    public ObscuredInt NowAddPropertyValue_LiLiang;
    public ObscuredInt NowAddPropertyValue_ZhiLi;
    public ObscuredInt NowAddPropertyValue_TiZhi;
    public ObscuredInt NowAddPropertyValue_NaiLi;

    //其他状态
    private bool BtnDownStatus;
    private ObscuredFloat BtnDownSum;

    private bool Down_PropreStatus;
    private ObscuredString DownType;
    private ObscuredString DownPropretyType;

	// Use this for initialization
	void Start () {
        UpdateShow();
        ShowAddProperty();
	}
	
	// Update is called once per frame
	void Update () {

        //属性增加按下状态
        if (Down_PropreStatus) {
            //表示增加属性
            if (DownType == "1") {
                if (NowShengYuNum <= 0)
                {
                    return;
                }
                if (!BtnDownStatus)
                {
                    BtnDownSum = BtnDownSum + Time.deltaTime;
                    if (BtnDownSum >= 0.5f)
                    {
                        BtnDownStatus = true;
                        BtnDownSum = 0;
                    }
                }
                else
                {
                    BtnDownSum = BtnDownSum + Time.deltaTime;
                    //1秒加10点属性
                    if (BtnDownSum >= 0.1f)
                    {
                        Btn_AddProprety(DownPropretyType);
                    }
                }
            }
            //表示减少属性
            if (DownType == "2") {
                if (!BtnDownStatus)
                {
                    BtnDownSum = BtnDownSum + Time.deltaTime;
                    if (BtnDownSum >= 1)
                    {
                        BtnDownStatus = true;
                        BtnDownSum = 0;
                    }
                }
                else
                {
                    BtnDownSum = BtnDownSum + Time.deltaTime;
                    //1秒减10点属性
                    if (BtnDownSum >= 0.1f)
                    {
                        Btn_CostProprety(DownPropretyType);
                    }
                }
            }
        }

	}


    //初始化当前宠物属性显示
    public void UpdateShow() {

        string addPropretyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyValue", "ID", RosePetID, "RosePet");
        NowShengYuNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyNum", "ID", RosePetID, "RosePet"));

        if (addPropretyValue != "")
        {
            string[] addPropertyList = addPropretyValue.Split(';');
            NowPropertyValue_LiLiang = int.Parse(addPropertyList[0]);
            NowPropertyValue_ZhiLi = int.Parse(addPropertyList[1]);
            NowPropertyValue_TiZhi = int.Parse(addPropertyList[2]);
            NowPropertyValue_NaiLi = int.Parse(addPropertyList[3]);
        }
        else {
            NowPropertyValue_LiLiang = 0;
            NowPropertyValue_ZhiLi = 0;
            NowPropertyValue_TiZhi = 0;
            NowPropertyValue_NaiLi = 0;
        }

        NowAddPropertyValue_LiLiang = 0;
        NowAddPropertyValue_ZhiLi = 0;
        NowAddPropertyValue_TiZhi = 0;
        NowAddPropertyValue_NaiLi = 0;

        //更新显示当前属性
        Obj_AddPropertyShow.GetComponent<UI_AddPropertyShow>().PetID = RosePetID;
        Obj_AddPropertyShow.GetComponent<UI_AddPropertyShow>().showPetProperty();
    }

    public void ShowAddProperty()
    {
        Debug.Log("更新显示");
        //显示属性
        Obj_ShengYuNum.GetComponent<Text>().text = NowShengYuNum.ToString();
        ObscuredString liLiangValue = ShowPropertyOne(NowPropertyValue_LiLiang, NowAddPropertyValue_LiLiang);
        ObscuredString zhiLiValue = ShowPropertyOne(NowPropertyValue_ZhiLi, NowAddPropertyValue_ZhiLi);
        ObscuredString tiLiValue = ShowPropertyOne(NowPropertyValue_TiZhi, NowAddPropertyValue_TiZhi);
        ObscuredString naiLiValue = ShowPropertyOne(NowPropertyValue_NaiLi, NowAddPropertyValue_NaiLi);

        Obj_AddPropretyValue_LiLiang.GetComponent<Text>().text = liLiangValue;
        Obj_AddPropretyValue_ZhiLi.GetComponent<Text>().text = zhiLiValue;
        Obj_AddPropretyValue_TiZhi.GetComponent<Text>().text = tiLiValue;
        Obj_AddPropretyValue_NaiLi.GetComponent<Text>().text = naiLiValue;

        if (NowAddPropertyValue_LiLiang != 0)
        {
            Obj_AddPropretyValue_LiLiang.GetComponent<Text>().color = Color.green;
        }
        else {
            Obj_AddPropretyValue_LiLiang.GetComponent<Text>().color = Color.white;
        }

        if (NowAddPropertyValue_ZhiLi != 0)
        {
            Obj_AddPropretyValue_ZhiLi.GetComponent<Text>().color = Color.green;
        }
        else
        {
            Obj_AddPropretyValue_ZhiLi.GetComponent<Text>().color = Color.white;
        }
        //Debug.Log("NowPropertyValue_TiZhi = " + NowPropertyValue_TiZhi + ";tiLiValue = " + tiLiValue);
        if (NowAddPropertyValue_TiZhi != 0)
        {
            Obj_AddPropretyValue_TiZhi.GetComponent<Text>().color = Color.green;
        }
        else
        {
            Obj_AddPropretyValue_TiZhi.GetComponent<Text>().color = Color.white;
        }

        if (NowAddPropertyValue_NaiLi != 0)
        {
            Obj_AddPropretyValue_NaiLi.GetComponent<Text>().color = Color.green;
        }
        else
        {
            Obj_AddPropretyValue_NaiLi.GetComponent<Text>().color = Color.white;
        }
    }

    private string ShowPropertyOne(ObscuredInt propertyValue, ObscuredInt propertyAddValue)
    {
        string showStr = "";
        if (propertyAddValue >= 1)
        {
            showStr = propertyValue + "+" + propertyAddValue.ToString();
        }
        else
        {
            showStr = propertyValue.ToString();
        }

        return showStr;
    }

    //长按增加属性
    public void BtnDown_AddProprety(string propreType)
    {
        Down_PropreStatus = true;
        DownType = "1";
        DownPropretyType = propreType;
    }
    //长按减少属性
    public void BtnDown_CostProprety(string propreType)
    {
        Down_PropreStatus = true;
        DownType = "2";
        DownPropretyType = propreType;
    }

    //松开清除状态
    public void BtnUp_Proprety() {
        BtnDownSum = 0;
        BtnDownStatus = false;
        Down_PropreStatus = false;
    }

    public void Btn_AddProprety(string addType) {
        //Debug.Log("点击加点");
        //获取当前点数
        if (NowShengYuNum<=0)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_300");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前没有可分配的点数");
            return;
        }

        switch (addType)
        {
            //力量
            case "1":
                NowAddPropertyValue_LiLiang = NowAddPropertyValue_LiLiang + 1;
                NowShengYuNum = NowShengYuNum - 1;
                break;
            //智力
            case "2":
                NowAddPropertyValue_ZhiLi = NowAddPropertyValue_ZhiLi + 1;
                NowShengYuNum = NowShengYuNum - 1;
                break;
            //体质
            case "3":
                NowAddPropertyValue_TiZhi = NowAddPropertyValue_TiZhi + 1;
                NowShengYuNum = NowShengYuNum - 1;
                break;
            //耐力
            case "4":
                NowAddPropertyValue_NaiLi = NowAddPropertyValue_NaiLi + 1;
                NowShengYuNum = NowShengYuNum - 1;
                break;
        }
        //Debug.Log("NowShengYuNum = " + NowShengYuNum);

        ShowAddProperty();
    }

    //减去技能点数
    public void Btn_CostProprety(string costType)
    {
        switch (costType)
        {
            //力量
            case "1":
                if (NowAddPropertyValue_LiLiang >= 1) {
                    NowAddPropertyValue_LiLiang = NowAddPropertyValue_LiLiang - 1;
                    NowShengYuNum = NowShengYuNum + 1;
                }
                break;
            //智力
            case "2":
                if (NowAddPropertyValue_ZhiLi >= 1)
                {
                    NowAddPropertyValue_ZhiLi = NowAddPropertyValue_ZhiLi - 1;
                    NowShengYuNum = NowShengYuNum + 1;
                }
                break;
            //体质
            case "3":
                if (NowAddPropertyValue_TiZhi >= 1)
                {
                    NowAddPropertyValue_TiZhi = NowAddPropertyValue_TiZhi - 1;
                    NowShengYuNum = NowShengYuNum + 1;
                }
                break;
            //耐力
            case "4":
                if (NowAddPropertyValue_NaiLi >= 1)
                {
                    NowAddPropertyValue_NaiLi = NowAddPropertyValue_NaiLi - 1;
                    NowShengYuNum = NowShengYuNum + 1;
                }
                break;
        }                                                       

        ShowAddProperty();
    }

    public void Btn_JiaDian() {

        ObscuredInt save_Liliang = NowPropertyValue_LiLiang + NowAddPropertyValue_LiLiang;
        ObscuredInt save_ZhiLi = NowPropertyValue_ZhiLi + NowAddPropertyValue_ZhiLi;
        ObscuredInt save_TiZhi = NowPropertyValue_TiZhi + NowAddPropertyValue_TiZhi;
        ObscuredInt save_NaiLi = NowPropertyValue_NaiLi + NowAddPropertyValue_NaiLi;

        ObscuredString saveAddPropretyValue = save_Liliang + ";" + save_ZhiLi + ";" + save_TiZhi + ";" + save_NaiLi;

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyValue",saveAddPropretyValue, "ID", RosePetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyNum", NowShengYuNum.ToString(), "ID", RosePetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

        NowPropertyValue_LiLiang = save_Liliang;
        NowPropertyValue_ZhiLi = save_ZhiLi;
        save_TiZhi = NowPropertyValue_TiZhi;
        save_NaiLi = NowPropertyValue_NaiLi;

        NowAddPropertyValue_LiLiang = 0;
        NowAddPropertyValue_ZhiLi = 0;
        NowAddPropertyValue_TiZhi = 0;
        NowAddPropertyValue_NaiLi = 0;
        UpdateShow();
        ShowAddProperty();
    }

    public void CloseUI() {
        this.gameObject.SetActive(false);
        //更新属性
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().UpdateShowStatus = true;
    }
}
