using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_TianFuIcon : MonoBehaviour {

    public string TianFuID;
    public bool UpdateTianFuIconStatus;
    public GameObject Obj_Fuji;

    public GameObject Obj_TianFuLv;
    public GameObject Obj_TianFuXuanZhong;
    public GameObject Obj_TianFuIcon;

	// Use this for initialization
	void Start () {
        UpdateTianFuIconStatus = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (UpdateTianFuIconStatus) {
            UpdateTianFuIconStatus = false;
            UpdateTianFuIcon();
        }
	}



    //更新天赋Icon
    void UpdateTianFuIcon() {

        //选中框更新
        if (Obj_Fuji.GetComponent<UI_TianFu>().TianFuXuanZhongID == TianFuID)
        {
            Obj_TianFuXuanZhong.SetActive(true);
        }
        else {
            Obj_TianFuXuanZhong.SetActive(false);
        }

        string tianFuIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", TianFuID, "Talent_Template");

        //更新天赋Icon
        object obj = Resources.Load("TianFuIcon/" + tianFuIcon, typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_TianFuIcon.GetComponent<Image>().sprite = img;
        
        //更新技能信息
        string tianFuNowLv = Game_PublicClassVar.Get_function_UI.RetrunTianFuNowLv(TianFuID);
        string tianFuMaxLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TianFuLv", "ID", TianFuID, "Talent_Template");

        //判定当前已用天赋总数
        int needAllSpValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedAllSpValue", "ID", TianFuID, "Talent_Template"));
        string tianFuType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Type", "ID", TianFuID, "Talent_Template");
        int nowUseSP = Game_PublicClassVar.Get_function_UI.GetTianFuUseNum(tianFuType);

        //天赋等级未激活或未达到指定天赋总数
        if (tianFuNowLv == "-1"|| nowUseSP < needAllSpValue)
        {
            tianFuNowLv = "0";
            Obj_TianFuLv.GetComponent<Text>().color = new Color(0.6f, 0.6f, 0.6f);
            //灰化Icon
            object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
            Material huiMaterial = huiObj as Material;
            Obj_TianFuIcon.GetComponent<Image>().material = huiMaterial;
        }
        else {
            Obj_TianFuIcon.GetComponent<Image>().material = null;
            Obj_TianFuLv.GetComponent<Text>().color = new Color(1f, 0.933f, 0.211f);
        }

        string tianFuLv = tianFuNowLv + "/" + tianFuMaxLv;
        Obj_TianFuLv.GetComponent<Text>().text = tianFuLv;
    }

    public void Btn_TianFuIcon()
    {

        string tianFuName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", TianFuID, "Talent_Template");
        string tianFuDesStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("talentDes", "ID", TianFuID, "Talent_Template");
        string tianFuMaxLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TianFuLv", "ID", TianFuID, "Talent_Template");
        string tianFuNowLv = Game_PublicClassVar.Get_function_UI.RetrunTianFuNowLv(TianFuID);

        //处理一下天赋描述中换行的问题
        string tianFuDes = "";
        string nextTianFuDes = "";
        if (tianFuDesStr != "" && tianFuDesStr != "0") {
            string[] tianFuDesList = tianFuDesStr.Split(';');
            int nextTianFuLv = 0;
            if (int.Parse(tianFuNowLv) >= int.Parse(tianFuMaxLv))
            {
                nextTianFuLv = int.Parse(tianFuMaxLv)-1;
                tianFuDes = tianFuDesList[nextTianFuLv];
            }
            else {
                nextTianFuLv = int.Parse(tianFuNowLv)-1;
                if (nextTianFuLv < 0)
                {
                    nextTianFuLv = 0;
                }
                else {
                    //Debug.Log("nextTianFuLv = " + nextTianFuLv);
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("下一等级");
                    //显示下一等级
                    nextTianFuDes = "\n\n" + langStr + "\n" + tianFuDesList[nextTianFuLv + 1];
                }
                tianFuDes = tianFuDesList[nextTianFuLv];
            }

            if (tianFuNowLv != "") {
                //查询激活的父级
                string needLastTianFuName = TianFuQianZhiStr(TianFuID);
                if (needLastTianFuName != "")
                {
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("前置激活条件");
                    nextTianFuDes = nextTianFuDes + "\n\n" + langStr + "：" + "\n";
                    nextTianFuDes = nextTianFuDes + needLastTianFuName + "\n";
                }
                //查询需要总点数
                string needAllSpValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedAllSpValue", "ID", TianFuID, "Talent_Template");
                if (needAllSpValue != "0")
                {
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("前置激活条件");
                    if (nextTianFuDes.Contains(langStr) == false) {
                        nextTianFuDes = nextTianFuDes + "\n\n" + langStr + "：" + "\n";
                    }
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("使用天赋点数");
                    nextTianFuDes = nextTianFuDes + langStr + ">=" + needAllSpValue;
                }
            }
        }


        if (tianFuNowLv == "-1")
        {
            tianFuNowLv = "0";
        }
        string tianFuLv = "LV " + tianFuNowLv + "/" + tianFuMaxLv;

        Obj_Fuji.GetComponent<UI_TianFu>().Obj_TianFuName.GetComponent<Text>().text = tianFuName;
        Obj_Fuji.GetComponent<UI_TianFu>().Obj_TianFuDes.GetComponent<Text>().text = tianFuDes + nextTianFuDes;
        Obj_Fuji.GetComponent<UI_TianFu>().Obj_TianFuLv.GetComponent<Text>().text = tianFuLv;

        //设置当前选中天赋
        Obj_Fuji.GetComponent<UI_TianFu>().TianFuXuanZhongID = TianFuID;
        Obj_Fuji.GetComponent<UI_TianFu>().TianFuXuanZhongObj = this.gameObject;

        //更新当前天赋
        Obj_Fuji.GetComponent<UI_TianFu>().UpdateTianFuAll();

    }

    //查询天赋激活前置
    public string TianFuQianZhiStr(string qua_TianFuID) {

        GameObject[] TianFuObjList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseSkill.GetComponent<UI_RoseSkill>().Obj_SkillTianFuSet.GetComponent<UI_TianFu>().TianFuObj;
        for (int i = 0; i < TianFuObjList.Length; i++) {
            if (TianFuObjList[i] != null) {
                string nowTianFuID = TianFuObjList[i].GetComponent<UI_TianFuIcon>().TianFuID;
                string nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nowTianFuID, "Talent_Template");
                string nextOpenLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextOpenLv", "ID", nowTianFuID, "Talent_Template");
                if (nextOpenLv == "0") {
                    nextOpenLv = "1";
                }
                if (nextID != "0" && nextID != "") {
                    if (nextID.Contains(qua_TianFuID)) {
                        string lastTianName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", nowTianFuID, "Talent_Template");
                        string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("天赋达到");
                        string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("级");
                        return lastTianName + langStr_1 + nextOpenLv + langStr_2;
                    }
                }
            }
        }

        return "";
    }

}
