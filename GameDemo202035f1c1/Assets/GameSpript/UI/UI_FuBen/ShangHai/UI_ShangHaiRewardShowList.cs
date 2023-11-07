using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShangHaiRewardShowList : MonoBehaviour {

    public string ShangHaiID;
    public GameObject Obj_ShangHaiIcon;
    public GameObject Obj_ShangHaiLv;
    public GameObject Obj_ShangHaiSelect;
    public GameObject Obj_Par;
    public GameObject Obj_YiLingQu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show() {

        string iconStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", ShangHaiID, "FuBenShangHai_Template");
        object obj = Resources.Load("OtherIcon/FuBen/" + iconStr, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ShangHaiIcon.GetComponent<Image>().sprite = itemIcon;

        string lvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", ShangHaiID, "FuBenShangHai_Template");
        Obj_ShangHaiLv.GetComponent<Text>().text = "试炼等级:" + lvStr;

        //显示当前是否选中
        if (Obj_Par != null) {
            if (Obj_Par.GetComponent<UI_FuBen_ShangHaiRewadSet>().NowShangHaiID == ShangHaiID)
            {
                Obj_ShangHaiSelect.SetActive(true);
            }
            else
            {
                Obj_ShangHaiSelect.SetActive(false);
            }
        }

        //显示当前领取状态
        //显示是否已领取
        string rewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiLvRewardSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (rewardStr.Contains(ShangHaiID))
        {
            Obj_YiLingQu.SetActive(true);
        }
        else
        {
            Obj_YiLingQu.SetActive(false);

            //检测当前是否可以领取,不可以领取为灰色
            string nowHightShangHaiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiHightID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            if (nowHightShangHaiID == "" || nowHightShangHaiID == null)
            {
                nowHightShangHaiID = "0";
            }
            if (int.Parse(nowHightShangHaiID) < int.Parse(ShangHaiID))
            {
                object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
                Material huiMaterial = huiObj as Material;
                Obj_ShangHaiIcon.GetComponent<Image>().material = huiMaterial;
            }
        }
    }

    public void Btn_Select() {

        Obj_Par.GetComponent<UI_FuBen_ShangHaiRewadSet>().NowShangHaiID = ShangHaiID;
        Obj_Par.GetComponent<UI_FuBen_ShangHaiRewadSet>().ShowRewardShow();

    }
}
