using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ZuoQiShowSet : MonoBehaviour {

    public GameObject[] Obj_NengLiLvList;
    public GameObject[] Obj_ZiZhiListText;
    public GameObject[] Obj_ZiZhiListImgPro;
    public GameObject Obj_BaoShiDu;
    public GameObject Obj_ZhuangTai;
    public GameObject[] Obj_FuJiaProList;
    public GameObject[] Obj_JinJieShowImage;
    public GameObject Obj_UI_CommonHintlTips_1;
    public GameObject Obj_ZuoQiJinJieShow;
    public GameObject Obj_ZuoQiImgShow;
    // Use this for initialization
    void Start () {

        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ZuoQiModelShow.SetActive(true);
        Init();
    }
	
	// Update is called once per frame
	void Update () {
		


	}

    private void OnDestroy()
    {
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ZuoQiModelShow.SetActive(false);
    }

    //初始化
    public void Init() {

        string nowZuoQiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

        //读取能力等级
        for (int i = 1; i <= Obj_NengLiLvList.Length; i++) {

            string nengLiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            string nengLiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nengLiID, "ZuoQiNengLi_Template");
            Obj_NengLiLvList[i - 1].GetComponent<Text>().text = nengLiLv + "级";

        }

        //读取资质
        string zizhiMaxValue =   Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhiMax", "ID", nowZuoQiID, "ZuoQi_Template");
        for (int i = 1; i <= Obj_ZiZhiListText.Length;i++) {

            string zizhiValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiZiZhi_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

            Debug.Log("zizhiValue = " + zizhiValue + " zizhiMaxValue = " + zizhiMaxValue);

            Obj_ZiZhiListText[i - 1].GetComponent<Text>().text = zizhiValue + "/" + zizhiMaxValue;
            Obj_ZiZhiListImgPro[i - 1].GetComponent<Image>().fillAmount = float.Parse(zizhiValue) / float.Parse(zizhiMaxValue);

            //读取属性
            string nowZuoQiNengLiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + i.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            string proStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddProperty", "ID", nowZuoQiNengLiID, "ZuoQiNengLi_Template");
            if (proStr != "" && proStr != "0") {
                string[] proStrList = proStr.Split(';')[0].Split(',');
                Obj_FuJiaProList[i-1].GetComponent<Text>().text = proStrList[1];
            }
        }

        //显示阶段属性加成
        object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
        Material huiMaterial = huiObj as Material;
        Obj_JinJieShowImage[0].GetComponent<Image>().material = huiMaterial;
        Obj_JinJieShowImage[1].GetComponent<Image>().material = huiMaterial;
        Obj_JinJieShowImage[2].GetComponent<Image>().material = huiMaterial;
        Obj_JinJieShowImage[3].GetComponent<Image>().material = huiMaterial;
        Obj_JinJieShowImage[4].GetComponent<Image>().material = huiMaterial;

        string nowZuoQiJieDuanLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiJieDuanLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowZuoQiJieDuanLv == "" || nowZuoQiJieDuanLv == null) {
            nowZuoQiJieDuanLv = "0";
        }
        int nowZuoQiJieDuanLvInt = int.Parse(nowZuoQiJieDuanLv);
        if (nowZuoQiJieDuanLvInt >= 2)
        {
            Obj_JinJieShowImage[0].GetComponent<Image>().material = null;
        }
        if (nowZuoQiJieDuanLvInt >= 4)
        {
            Obj_JinJieShowImage[1].GetComponent<Image>().material = null;
        }
        if (nowZuoQiJieDuanLvInt >= 6)
        {
            Obj_JinJieShowImage[2].GetComponent<Image>().material = null;
        }
        if (nowZuoQiJieDuanLvInt >= 8)
        {
            Obj_JinJieShowImage[3].GetComponent<Image>().material = null;
        }
        if (nowZuoQiJieDuanLvInt >= 10)
        {
            Obj_JinJieShowImage[4].GetComponent<Image>().material = null;
        }

        //显示状态
        string nowZuoQiBaoShiDu = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoShiDu", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nowZuoQiBaoStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

        Obj_BaoShiDu.GetComponent<Text>().text = nowZuoQiBaoShiDu;
        string statusName = Game_PublicClassVar.Get_function_Pasture.ReturnStatusName(nowZuoQiBaoStatus);
        Obj_ZhuangTai.GetComponent<Text>().text = statusName;

        //显示阶段
        string nowLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiJieDuanLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowLv == "0" || nowLv == "")
        {
            Obj_ZuoQiJinJieShow.SetActive(false);
        }
        else {
            Obj_ZuoQiJinJieShow.SetActive(true);
            Debug.Log("nowLv = " + nowLv);
            object obj = Resources.Load("GameUI/Img/ZuoQiJieDuanLv_" + nowLv, typeof(Sprite));
            Sprite itemIcon = obj as Sprite;
            Obj_ZuoQiJinJieShow.GetComponent<Image>().sprite = itemIcon;
        }

        //显示外观
        if (Obj_ZuoQiImgShow != null)
        {
            string nowZuoQiShowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowZuoQiShowID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            if (nowZuoQiShowID == "" || nowZuoQiShowID == "0" || nowZuoQiShowID == null) {
                nowZuoQiShowID = "10001";
            }
            object obj = Resources.Load("CameraImage/ZuoQi_" + nowZuoQiShowID, typeof(Texture));
            Texture itemIcon = obj as Texture;
            Obj_ZuoQiImgShow.GetComponent<RawImage>().texture = itemIcon;
        }

    }

    //坐骑进阶
    public void Btn_JinJie() {

        bool ifUpdate = Game_PublicClassVar.Get_function_Pasture.ZuoQiJinJie();
        if (ifUpdate == true) {
            Init();
        }

    }

    public void Btn_ShowZuoQiJinJie_Down(string zuoqiID) {

        if (zuoqiID == "" || zuoqiID == "0" || zuoqiID == null) {
            return;
        }

        string name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JieName", "ID", zuoqiID, "ZuoQi_Template");
        string des = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", zuoqiID, "ZuoQi_Template");

        Game_PublicClassVar.Get_function_UI.ShowCommonTips_1(name,des);

    }

    public void Btn_ShowZuoQiJinJie_Up()
    {
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
    }


    public void Btn_ShowBaoShiDuHint() {

        string name = "饱食度说明";
        string des = "饱食度低于60附加角色的坐骑加速效果会开始降低。";
        Game_PublicClassVar.Get_function_UI.ShowCommonTips_1(name, des);
    }

    public void Btn_CloseHint() {
        Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
    }

}
