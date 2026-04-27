using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ZuoQiShouCangShowList : MonoBehaviour {

    public string ZuoQiShowID;
    public GameObject Obj_ZuoQiShowImage;
    public GameObject Obj_ZuoQiName;
    public GameObject Obj_ZuoQiStatusName;
    public GameObject Obj_ZuoQiDes;
    public GameObject Obj_ZuoQiJiHuoImage;
    public GameObject Obj_ZuoQiShowBtn;
    public GameObject Obj_ZuoQiShouHuiBtn;
    public GameObject Obj_ZuoQiHuoQuSet;
    public GameObject Obj_ZuoQiHuoQuStr;
    public GameObject Obj_ZuoQiJiHuo;
    public GameObject Obj_ZuoQiWeiJiHuo;
    public GameObject[] Obj_ZuoQiQuaList;


    // Use this for initialization
    void Start () {

        Init();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    //初始化显示
    public void Init() {

        string NameStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", ZuoQiShowID, "ZuoQiShow_Template");
        string IconStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", ZuoQiShowID, "ZuoQiShow_Template");
        string ModelIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ModelID", "ID", ZuoQiShowID, "ZuoQiShow_Template");
        string DesStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", ZuoQiShowID, "ZuoQiShow_Template");
        string AddPropertyStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddProperty", "ID", ZuoQiShowID, "ZuoQiShow_Template");

        //控件显示
        Obj_ZuoQiName.GetComponent<Text>().text = NameStr;
        Obj_ZuoQiDes.GetComponent<Text>().text = DesStr;

        int nowQua = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Quality", "ID", ZuoQiShowID, "ZuoQiShow_Template"));
        Color nowColor = Game_PublicClassVar.Get_function_UI.QualityReturnColor(nowQua.ToString());
        Obj_ZuoQiName.GetComponent<Text>().color = nowColor;
        for (int i = 1; i <= 5; i++) {
            if (nowQua >= i)
            {
                Obj_ZuoQiQuaList[i - 1].SetActive(true);
            }
            else {
                Obj_ZuoQiQuaList[i - 1].SetActive(false);
            }
        }


        //显示模型
        object obj = Resources.Load("CameraImage/ZuoQi_" + ModelIDStr, typeof(Texture));
        Texture objImg = obj as Texture;
        Obj_ZuoQiShowImage.GetComponent<RawImage>().texture = objImg;

        //是否拥有此坐骑
        bool ifHave = Game_PublicClassVar.Get_function_Pasture.IfHaveZuoQi(ZuoQiShowID);
        if (ifHave)
        {
            //显示激活按钮,显示使用按钮
            Obj_ZuoQiJiHuoImage.SetActive(true);
            Obj_ZuoQiShowBtn.SetActive(true);
            Obj_ZuoQiHuoQuSet.SetActive(false);

            Obj_ZuoQiJiHuo.SetActive(false);
            Obj_ZuoQiWeiJiHuo.SetActive(false);

        }
        else {

            Obj_ZuoQiJiHuoImage.SetActive(false);
            Obj_ZuoQiShowBtn.SetActive(false);
            Obj_ZuoQiHuoQuSet.SetActive(true);

            Obj_ZuoQiJiHuo.SetActive(false);
            Obj_ZuoQiWeiJiHuo.SetActive(true);

            //显示阶段属性加成
            object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
            Material huiMaterial = huiObj as Material;
            Obj_ZuoQiShowImage.GetComponent<RawImage>().material = huiMaterial;

            //显示获取途径
            string nowGetDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GetDes", "ID", ZuoQiShowID, "ZuoQiShow_Template");
            Obj_ZuoQiHuoQuStr.GetComponent<Text>().text = nowGetDes;
        }

        string nowZuoQiShowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowZuoQiShowID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowZuoQiShowID == ZuoQiShowID)
        {
            //当前使用的坐骑,不显示使用按钮
            Obj_ZuoQiShowBtn.SetActive(false);
            Obj_ZuoQiShouHuiBtn.SetActive(true);
        }
        else {
            Obj_ZuoQiShouHuiBtn.SetActive(false);
        }
    }

    //点击出战按钮
    public void Btn_Use() {

        if (ZuoQiShowID == "" || ZuoQiShowID == "0" || ZuoQiShowID == null) {
            return;
        }

        Game_PublicClassVar.Get_function_Pasture.ZuoQiTiHuanShow(ZuoQiShowID);
        Init();

    }

    //收回
    public void Btn_ShouHui() {

        Game_PublicClassVar.Get_function_Pasture.ZuoQi_RoseShowShouHui();
        Init();
    }
}
