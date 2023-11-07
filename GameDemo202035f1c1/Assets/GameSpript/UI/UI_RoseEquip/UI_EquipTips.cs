using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_EquipTips : MonoBehaviour {

	public ObscuredString ItemID;
    public ObscuredString ItemHideID;
    public ObscuredString ItemGemID;
    public ObscuredString ItemGemHole;
	public ObscuredString ItemHide_PaiHangBangShowHideStr;          //排行榜查看装备展示的
    public ObscuredString PaiHangShowShowOcc;                                  //展示职业
	public GameObject Obj_Imgback;
	public GameObject Obj_EquipIcon;
	public GameObject Obj_EquipQuality;
	public GameObject Obj_EquipName;
	public GameObject Obj_EquipType;
	public GameObject Obj_EquipProperty;
	public GameObject Obj_EquipWearNeed;
	public GameObject Obj_EquipDes;
    public GameObject Obj_EquipWearNeedProperty;

    public GameObject Obj_HideObj;
    public GameObject Obj_HideProperty;

	public GameObject Obj_EquiBase;
	public GameObject Obj_EquipNeed;
	public GameObject Obj_EquipBottom;
    public GameObject Obj_BtnSet;
    public GameObject Obj_EquipPropertyText;
    public GameObject Obj_BagOpenSet;
    public GameObject Obj_RoseEquipOpenSet;
    public GameObject Obj_SaveStoreHouse;
    public GameObject Obj_Diu;
    public GameObject Obj_Btn_StoreHouseSet;
	public GameObject Obj_Btn_HuiShou;
	public GameObject Obj_Btn_HuiShouCancle;

    public string UIBagSpaceNum;
    public GameObject Obj_UIBagSpace;
    public GameObject Obj_UIRoseEquipShow;
    public GameObject Obj_UIOrnamentChoice;

    public GameObject Obj_UIEquipGemHoleSet;
    public GameObject Obj_UIEquipGemHole_1;
    public GameObject Obj_UIEquipGemHole_2;
    public GameObject Obj_UIEquipGemHole_3;
    public GameObject Obj_UIEquipGemHole_4;

    public GameObject Obj_UIEquipGemHoleText_1;
    public GameObject Obj_UIEquipGemHoleText_2;
    public GameObject Obj_UIEquipGemHoleText_3;
    public GameObject Obj_UIEquipGemHoleText_4;

    public GameObject Obj_UIEquipGemHoleIcon_1;
    public GameObject Obj_UIEquipGemHoleIcon_2;
    public GameObject Obj_UIEquipGemHoleIcon_3;
    public GameObject Obj_UIEquipGemHoleIcon_4;

    public GameObject Obj_UIEquipSuit;              //套装组建
    public GameObject Obj_UIEquipSuitName;          //套装名称
    public GameObject Obj_EquipSuitPropertyText;

    public GameObject Obj_UISuitEquipName;
    public GameObject EquipSuitShowSet_Right;
    public GameObject EquipSuitShowSet_Lelt;

    public ObscuredString EquipTipsType;                    //1.背包打开  2.装备栏打开 3.无任何按钮显示,点击商店列表弹出  4.仓库  5.对比查看  6.回收
    public ObscuredString EquipQiangHuaID;                  //装备强化ID
    public ObscuredInt EquipType;                        //0表示装备   1表示十二生肖  2表示宠物装备
	private GameObject itemTips_1;
    private GameObject showEquipObj;
    public GameObject Obj_ImgYiChuanDai;

    private string equipSuitID;
    private int properShowNum;                      //属性列表展示次数

    private bool clickBtnStatus;                    //点击按钮状态

	// Use this for initialization
	void Start () {
        //Debug.Log("打开套装");
		string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template", PaiHangShowShowOcc);
		string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
		
		//获取当前装备的各项属性
		string equip_ID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemEquipID", "ID", ItemID, "Item_Template");
		string equipName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template",PaiHangShowShowOcc);
		string equipLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", ItemID, "Item_Template");
		string ItemBlackDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemBlackDes", "ID", ItemID, "Item_Template");
		string equip_Hp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Hp", "ID", equip_ID, "Equip_Template");
		string equip_MinAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MinAct", "ID", equip_ID, "Equip_Template");
		string equip_MaxAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MaxAct", "ID", equip_ID, "Equip_Template");
        string equip_MinMagAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MinMagAct", "ID", equip_ID, "Equip_Template");
        string equip_MaxMagAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MaxMagAct", "ID", equip_ID, "Equip_Template");
        string equip_MinDef = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MinDef", "ID", equip_ID, "Equip_Template");
		string equip_MaxDef = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MaxDef", "ID", equip_ID, "Equip_Template");
		string equip_MinAdf = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MinAdf", "ID", equip_ID, "Equip_Template");
		string equip_MaxAdf = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MaxAdf", "ID", equip_ID, "Equip_Template");
		string equip_Cri = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Cri", "ID", equip_ID, "Equip_Template");
		string equip_Hit = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Hit", "ID", equip_ID, "Equip_Template");
		string equip_Dodge = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Dodge", "ID", equip_ID, "Equip_Template");
		string equip_DamgeAdd = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_DamgeAdd", "ID", equip_ID, "Equip_Template");
		string equip_DamgeSub = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_DamgeSub", "ID", equip_ID, "Equip_Template");
		string equip_Speed = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Speed", "ID", equip_ID, "Equip_Template");
		string equip_Lucky = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Lucky", "ID", equip_ID, "Equip_Template");


        //初始化当前是什么装备类型
        string equipType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");

        EquipType = 0;
        if (int.Parse(equipType) >= 101 && int.Parse(equipType) <= 112)
        {
            EquipType = 1;
        }

        if (int.Parse(equipType) >= 201 && int.Parse(equipType) <= 204)
        {
            EquipType = 2;
        }

        //获取极品装备属性
        //string hidePropertyStr = "1,1;4,1;5,1";         //临时
        string hidePropertyStr = "";
        string[] hideProperty;
		bool showHideStatus = false;
        //ItemHideID = "1001";

        if (ItemHideID != "0" && ItemHideID!="")
        {
            hidePropertyStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", ItemHideID, "RoseEquipHideProperty");
            
			showHideStatus = true;

        }

		//排行榜装备显示
		if (ItemHide_PaiHangBangShowHideStr != "")
		{
			//Debug.Log("有属性");
			hidePropertyStr = ItemHide_PaiHangBangShowHideStr;
			showHideStatus = true;
		}

		if (showHideStatus) {
		
			hideProperty = hidePropertyStr.Split(';');
			//隐藏属性
			/*
            1:血量上限
            2:物理攻击最大值
            3:物理防御最大值
            4:魔法防御最大值
            */
			//循环加入各个隐藏属性
			if (hidePropertyStr != "")
			{
				for (int i = 0; i <= hideProperty.Length - 1; i++)
				{
                    string[] hidePropertyList = hideProperty[i].Split(',');
                    if (hidePropertyList.Length >= 2) {
                        string hidePropertyType = hideProperty[i].Split(',')[0];
                        string hidePropertyValue = hideProperty[i].Split(',')[1];

                        switch (hidePropertyType)
                        {
                            //血量上限
                            case "1":
                                equip_Hp = (int.Parse(equip_Hp) + int.Parse(hidePropertyValue)).ToString();
                                break;
                            //物理攻击最大值
                            case "2":
                                equip_MaxAct = (int.Parse(equip_MaxAct) + int.Parse(hidePropertyValue)).ToString();
                                break;
                            case "3":
                            //物理防御最大值
                            equip_MaxDef = (int.Parse(equip_MaxDef) + int.Parse(hidePropertyValue)).ToString();
                            break;
                            //魔法防御最大值
                            case "4":
                                equip_MaxAdf = (int.Parse(equip_MaxAdf) + int.Parse(hidePropertyValue)).ToString();
                                break;
                            //魔法攻击最大值
                            case "5":
                                equip_MaxMagAct = (int.Parse(equip_MaxMagAct) + int.Parse(hidePropertyValue)).ToString();
                                break;
                                
                            //幸运值
                            case "100":
                                //equip_Lucky = (int.Parse(equip_Lucky) + int.Parse(hidePropertyValue)).ToString();
                                equip_Lucky = (int.Parse(equip_Lucky)).ToString();
                                break;
                        }
                    }
				}
			}
		}
		else {
			hideProperty = hidePropertyStr.Split(';');      //防止下面读取不到数据报错
		}


		//判定各项属性是否为0，为0的不显示
		string textShow = "";
        string langStr = "";
        //血量
        if (equip_Hp != "0")
		{
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("血量");
			textShow = langStr + " ： " + equip_Hp ;
			//textNum = textNum + 1;
            //ShowPropertyText(textShow);
            bool ifHideProperty = false;
            //显示极品属性
            if (hidePropertyStr != "") {
                for (int i = 0; i <= hideProperty.Length - 1; i++)
                {
                    if (hideProperty[i].Split(',').Length >= 2) {
                        string hidePropertyType = hideProperty[i].Split(',')[0];
                        string hidePropertyValue = hideProperty[i].Split(',')[1];
                        if (hidePropertyType == "1")
                        {
                            textShow = langStr + " ： " + equip_Hp + "(+" + hidePropertyValue + ")";
                            ifHideProperty = true;
                        }
                    }
                }
            }

            if (ifHideProperty)
            {
                ShowPropertyText(textShow,"1");
            }
            else {
                ShowPropertyText(textShow);                
            }
		}
		//攻击
		if (equip_MinAct != "0"||equip_MaxAct != "0")
		{
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击");
                textShow = langStr + " ： " + equip_MinAct + " - " + equip_MaxAct ;
				//textNum = textNum + 1;
                //ShowPropertyText(textShow);
                bool ifHideProperty = false;
                //显示极品属性
                if (hidePropertyStr != "")
                {
                    for (int i = 0; i <= hideProperty.Length - 1; i++)
                    {
                        if (hideProperty[i].Split(',').Length >= 2) {
                            string hidePropertyType = hideProperty[i].Split(',')[0];
                            string hidePropertyValue = hideProperty[i].Split(',')[1];
                            if (hidePropertyType == "2")
                            {
                                textShow = langStr + " ： " + equip_MinAct + " - " + equip_MaxAct + "(+" + hidePropertyValue + ")";
                                ifHideProperty = true;
                            }
                        }
                    }
                }
                if (ifHideProperty)
                {
                    ShowPropertyText(textShow, "1");
                }
                else
                {
                    ShowPropertyText(textShow);
                }
		}


        //防御
        if (equip_MinDef != "0"||equip_MaxDef != "0")
		{
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("防御");
                textShow = langStr + " ： " + equip_MinDef + " - " + equip_MaxDef ;
				//textNum = textNum + 1;
                //ShowPropertyText(textShow);
                bool ifHideProperty = false;
                //显示极品属性
                if (hidePropertyStr != "")
                {
                    for (int i = 0; i <= hideProperty.Length - 1; i++)
                    {
                        if (hideProperty[i].Split(',').Length >= 2)
                        {
                            string hidePropertyType = hideProperty[i].Split(',')[0];
                            string hidePropertyValue = hideProperty[i].Split(',')[1];
                            if (hidePropertyType == "3")
                            {
                                textShow = langStr + " ： " + equip_MinDef + " - " + equip_MaxDef + "(+" + hidePropertyValue + ")";
                                ifHideProperty = true;
                            }
                        }
                    }
                }
                if (ifHideProperty)
                {
                    ShowPropertyText(textShow, "1");
                }
                else
                {
                    ShowPropertyText(textShow);
                }
		}
		//魔防
		if (equip_MinAdf != "0"||equip_MaxAdf != "0")
		{
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("魔防");
                textShow = langStr + " ： " + equip_MinAdf + " - " + equip_MaxAdf ;
				//textNum = textNum + 1;
                //ShowPropertyText(textShow);
                bool ifHideProperty = false;
                //显示极品属性
                if (hidePropertyStr != "")
                {
                    for (int i = 0; i <= hideProperty.Length - 1; i++)
                    {
                        if (hideProperty[i].Split(',').Length >= 2) {
                            string hidePropertyType = hideProperty[i].Split(',')[0];
                            string hidePropertyValue = hideProperty[i].Split(',')[1];
                            if (hidePropertyType == "4")
                            {
                                textShow = langStr + " ： " + equip_MinAdf + " - " + equip_MaxAdf + "(+" + hidePropertyValue + ")";
                                ifHideProperty = true;
                            }
                        }

                    }
                }
                if (ifHideProperty)
                {
                    ShowPropertyText(textShow, "1");
                }
                else
                {
                    ShowPropertyText(textShow);
                }
		}


        //攻击
        if (equip_MinMagAct != "0" || equip_MaxMagAct != "0")
        {
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("技能伤害");
            textShow = langStr + " ： " + equip_MinMagAct + " - " + equip_MaxMagAct;
            //textNum = textNum + 1;
            //ShowPropertyText(textShow);
            bool ifHideProperty = false;
            //显示极品属性
            if (hidePropertyStr != "")
            {
                for (int i = 0; i <= hideProperty.Length - 1; i++)
                {
                    if (hideProperty[i].Split(',').Length >= 2)
                    {
                        string hidePropertyType = hideProperty[i].Split(',')[0];
                        string hidePropertyValue = hideProperty[i].Split(',')[1];
                        if (hidePropertyType == "5")
                        {
                            textShow = langStr + " ： " + equip_MinMagAct + " - " + equip_MaxMagAct + "(+" + hidePropertyValue + ")";
                            ifHideProperty = true;
                        }
                    }
                }
            }
            if (ifHideProperty)
            {
                ShowPropertyText(textShow, "1");
            }
            else
            {
                ShowPropertyText(textShow);
            }
        }

        //暴击
        if (equip_Cri != "0")
		{
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("暴击");
            textShow = langStr + " ： " + float.Parse(equip_Cri) * 100 + "%\n";
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		//命中
		if (equip_Hit != "0")
		{
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("命中");
            textShow = langStr + " ： " + float.Parse(equip_Hit)*100 + "%\n";
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		//闪避
		if (equip_Dodge != "0")
		{
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪避");
            textShow = langStr + " ： " + float.Parse(equip_Dodge)*100 + "%\n";
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		//伤害加成
		if (equip_DamgeAdd != "0")
		{
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("伤害加成");
            textShow = langStr + " ： " + float.Parse(equip_DamgeAdd)*100 + "%\n";
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		//伤害减免
		if (equip_DamgeSub != "0")
		{
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("伤害减免");
            textShow = langStr + " ： " + float.Parse(equip_DamgeSub)*100 + "%\n";
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		//速度
		if (equip_Speed != "0")
		{
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("移动速度");
            textShow = langStr + " ： " + equip_Dodge ;
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}

		//幸运值
		if (equip_Lucky != "0")
		{
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("幸运值");
            textShow = langStr + " ： " + equip_Lucky ;
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}

        //法术
        /*
        if (equip_MinMagAct != "0" || equip_MaxMagAct != "0")
        {
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("技能伤害");
            textShow = langStr + " ： " + equip_MinMagAct + " - " + equip_MaxMagAct;
            ShowPropertyText(textShow);
            //textNum = textNum + 1;
            //ShowPropertyText(textShow);
        }
        */

        /*
        int geDangValue_EquipSuit_value = 0;                    //格挡值
        float zhongJiPro_EquipSuit_value = 0.0f;                //重击概率
        int zhongJiValue_EquipSuit_value = 0;                   //重击附加伤害值
        int guDingValue_EquipSuit_value = 0;                    //每次普通攻击附加的伤害值
        int huShiDefValue_EquipSuit_value = 0;                  //忽视目标防御值                       
        int huShiAdfValue_EquipSuit_value = 0;                  //忽视目标魔防值
        float xiXuePro_EquipSuit_value = 0.0f;                  //吸血概率
        */

        //获取其他属性
        string AddPropreListStrValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropreListStr", "ID", equip_ID, "Equip_Template");
        if (AddPropreListStrValue != "" && AddPropreListStrValue != "0")
        {

            string[] AddPropreListStr = AddPropreListStrValue.Split(';');
            if (AddPropreListStr.Length > 0)
            {
                for (int y = 0; y < AddPropreListStr.Length; y++)
                {
                    string proStr = Game_PublicClassVar.Get_function_UI.ShowHintPro(AddPropreListStr[y],EquipType);
                    if (proStr != "" && proStr != "0") {
                        ShowPropertyText(proStr);
                    }
                }
            }
        }

        //ShowPropertyText("附魔效果:攻击提升1%");
        //Debug.Log("hidePropertyStr = " + hidePropertyStr);
        if (hidePropertyStr != "0" && hidePropertyStr != "")
        {
            //获取和显示隐藏属性
            for (int i = 0; i < hideProperty.Length; i++)
            {
                //Debug.Log("hideProperty = " + hideProperty[i]);
                string proStr = Game_PublicClassVar.Get_function_UI.ShowHintPro(hideProperty[i], EquipType);
                if (proStr != "" && proStr != "0")
                {
                    ShowPropertyText(proStr, "1");
                }
            }

            //获取和显示隐藏技能
            for (int i = 0; i < hideProperty.Length; i++)
            {
                string proStr = Game_PublicClassVar.Get_function_UI.ShowHintSkill(hideProperty[i]);
                if (proStr != "" && proStr != "0")
                {
                    ShowPropertyText(proStr, "2");
                }
            }
        }

        //展示装备强化
        if(EquipTipsType=="2"){
			//EquipQiangHuaID = 
			if (EquipQiangHuaID != "0" && EquipQiangHuaID !="") {
				//Debug.Log ("EquipQiangHuaID = " + EquipQiangHuaID);
                float equipPropreAdd = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipPropreAdd", "ID", EquipQiangHuaID, "EquipQiangHua_Template"));
                string qiangHuaLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaLv", "ID", EquipQiangHuaID, "EquipQiangHua_Template");
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("强化 装备属性提升");
                ShowPropertyText("+" + qiangHuaLv + langStr + equipPropreAdd * 100 + "%", "4");
            }
        }

		//最后一行去掉/n
		if (textShow != "")
		{
			textShow = textShow.Substring(0, textShow.Length - 1);
		}
		
		//判定当前装备是什么部位
		string textEquipType = "";
		string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");
		switch (itemSubType)
		{
		    case "1":
			    textEquipType = "武器";
			    break;
			
		    case "2":
			    textEquipType = "衣服";
			    break;
			
		    case "3":
			    textEquipType = "护符";
			    break;
			
		    case "4":
			    textEquipType = "戒指";
			    break;
			
		    case "5":
			    textEquipType = "饰品";
			    break;
			
		    case "6":
			    textEquipType = "鞋子";
			    break;
			
		    case "7":
			    textEquipType = "裤子";
			    break;
			
		    case "8":
			    textEquipType = "腰带";
			    break;
			
		    case "9":
			    textEquipType = "手套";
			    break;
			
		    case "10":
			    textEquipType = "头盔";
			    break;
			
		    case "11":
			    textEquipType = "项链";
			    break;

            case "101":
                textEquipType = "生肖装备";
                break;

            case "102":
                textEquipType = "生肖装备";
                break;

            case "103":
                textEquipType = "生肖装备";
                break;

            case "104":
                textEquipType = "生肖装备";
                break;

            case "105":
                textEquipType = "生肖装备";
                break;

            case "106":
                textEquipType = "生肖装备";
                break;

            case "107":
                textEquipType = "生肖装备";
                break;

            case "108":
                textEquipType = "生肖装备";
                break;

            case "109":
                textEquipType = "生肖装备";
                break;

            case "110":
                textEquipType = "生肖装备";
                break;

            case "111":
                textEquipType = "生肖装备";
                break;

            case "112":
                textEquipType = "生肖装备";
                break;

            case "201":
                textEquipType = "宠物项圈";
                break;

            case "202":
                textEquipType = "宠物铠甲";
                break;

            case "203":
                textEquipType = "宠物护腕";
                break;

            case "204":
                textEquipType = "宠物披风";
                break;
        }

        textEquipType = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(textEquipType);

        //显示宝石插槽属性
        float showGemTextNum = 0;
        //ItemGemHole = "101,102,102,102";
        //ItemGemID = "99990001,99990002,0,0";

        //Debug.Log("ItemGemHole = " + ItemGemHole);
        //Debug.Log("ItemGemID = " + ItemGemID);

        //装备显示
        switch (EquipTipsType)
        {
            //背包显示
            case "1":
                break;

            //角色栏显示
            case "2":
                break;

            case "4":
                break;
        }

        if (ItemGemHole != "" && ItemGemHole != "0")
        {

            string[] gemHoleList = ItemGemHole.ToString().Split(',');
            string[] gemIDList = ItemGemID.ToString().Split(',');
            showGemTextNum = gemHoleList.Length * 22;
            //Debug.Log("ItemGemID = " + ItemGemID);
            //Debug.Log("showGemTextNum = " + showGemTextNum);
            if (gemHoleList.Length >= 1)
            {
                switch (gemHoleList.Length)
                {
                    case 1:
                        TipsShowEquipGem(Obj_UIEquipGemHoleIcon_1, Obj_UIEquipGemHoleText_1, gemIDList[0], gemHoleList[0]);
                        Obj_UIEquipGemHole_1.SetActive(true);
                        break;

                    case 2:
                        TipsShowEquipGem(Obj_UIEquipGemHoleIcon_1, Obj_UIEquipGemHoleText_1, gemIDList[0], gemHoleList[0]);
                        TipsShowEquipGem(Obj_UIEquipGemHoleIcon_2, Obj_UIEquipGemHoleText_2, gemIDList[1], gemHoleList[1]);
                        Obj_UIEquipGemHole_1.SetActive(true);
                        Obj_UIEquipGemHole_2.SetActive(true);
                        break;

                    case 3:
                        TipsShowEquipGem(Obj_UIEquipGemHoleIcon_1, Obj_UIEquipGemHoleText_1, gemIDList[0], gemHoleList[0]);
                        TipsShowEquipGem(Obj_UIEquipGemHoleIcon_2, Obj_UIEquipGemHoleText_2, gemIDList[1], gemHoleList[1]);
                        TipsShowEquipGem(Obj_UIEquipGemHoleIcon_3, Obj_UIEquipGemHoleText_3, gemIDList[2], gemHoleList[2]);
                        Obj_UIEquipGemHole_1.SetActive(true);
                        Obj_UIEquipGemHole_2.SetActive(true);
                        Obj_UIEquipGemHole_3.SetActive(true);
                        break;

                    case 4:
                        TipsShowEquipGem(Obj_UIEquipGemHoleIcon_1, Obj_UIEquipGemHoleText_1, gemIDList[0], gemHoleList[0]);
                        TipsShowEquipGem(Obj_UIEquipGemHoleIcon_2, Obj_UIEquipGemHoleText_2, gemIDList[1], gemHoleList[1]);
                        TipsShowEquipGem(Obj_UIEquipGemHoleIcon_3, Obj_UIEquipGemHoleText_3, gemIDList[2], gemHoleList[2]);
                        TipsShowEquipGem(Obj_UIEquipGemHoleIcon_4, Obj_UIEquipGemHoleText_4, gemIDList[3], gemHoleList[3]);
                        Obj_UIEquipGemHole_1.SetActive(true);
                        Obj_UIEquipGemHole_2.SetActive(true);
                        Obj_UIEquipGemHole_3.SetActive(true);
                        Obj_UIEquipGemHole_4.SetActive(true);
                        break;
                }

                Vector2 equipGemHole_vec2 = new Vector2(150.0f, -110 - 22 * properShowNum);
                Obj_UIEquipGemHoleSet.transform.GetComponent<RectTransform>().anchoredPosition = equipGemHole_vec2;
            }
        }



        //显示隐藏技能
        float HintTextNum = 0;
        string skillIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", ItemID, "Item_Template");
        string[] nowskillID = skillIDStr.Split(',');

        //职业判定
        if (nowskillID.Length >= 3) {
            string occType = Game_PublicClassVar.Get_function_Rose.GetRoseOcc();
            switch (occType) {

                case "1":
                    skillIDStr = nowskillID[0];
                    break;

                case "2":
                    skillIDStr = nowskillID[1];
                    break;

                case "3":
                    skillIDStr = nowskillID[2];
                    break;
            }
        }

        if (skillIDStr != "0")
        {
            //标题长度
            HintTextNum = HintTextNum + 50.0f;
            //获取技能ID列表
            string[] skillID = skillIDStr.Split(',');
            //设置位置
            //Obj_HideObj.transform.localPosition = new Vector3(8, -20 - 11 * textNum, 0);
            Vector2 hint_vec2 = new Vector2(150.0f, -110 - 22 * properShowNum - HintTextNum - showGemTextNum);
            Obj_HideObj.transform.GetComponent<RectTransform>().anchoredPosition = hint_vec2;


            //逐个显示
            string showHintTxt = "";
            for (int i = 0; i <= skillID.Length - 1; i++) {
                //每增加一个隐藏属性,长度增加22
                HintTextNum = HintTextNum + 20.0f;
                //获取技能名称
                //Debug.Log("skillID[i] = " + skillID[i]);
                //skillID[i] = "60091201";        //测试,配置完成后时候屏蔽即可
                string skillName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillName", "ID", skillID[i], "Skill_Template");
                showHintTxt = showHintTxt + skillName + "\n";
                //Debug.Log("showHintTxt = " + showHintTxt + "skillIDStr = " + skillIDStr + "skillName = " + skillName);
            }

            Obj_HideProperty.GetComponent<Text>().text = showHintTxt;
            Obj_HideObj.SetActive(true);
            Obj_HideProperty.SetActive(true);
        }
        else {
            Obj_HideObj.SetActive(false);
            Obj_HideProperty.SetActive(false);
        }


        //显示套装属性
        float equipSuitTextNum = 0;
        float suitShowTextNumSum = 0;
        equipSuitID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSuitID", "ID", equip_ID, "Equip_Template");
        if (equipSuitID != "0")
        {
            //设置套装显示位置
            //Debug.Log("properShowNum = " + properShowNum);
            Vector2 equipSuit_vec2 = new Vector2(150.0f, -160 - 22 * properShowNum - HintTextNum - showGemTextNum);
            Obj_UIEquipSuit.transform.GetComponent<RectTransform>().anchoredPosition = equipSuit_vec2;
            Obj_UIEquipSuit.SetActive(true);
            equipSuitTextNum = equipSuitTextNum + 50.0f;

            //获取套装名称
            string equipSuitName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", equipSuitID, "EquipSuit_Template");

            //获取套装ID
            string[] needEquipIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedEquipID", "ID", equipSuitID, "EquipSuit_Template").Split(';');
            string[] needEquipNumSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedEquipNum", "ID", equipSuitID, "EquipSuit_Template").Split(';');
            string[] suitPropertyIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SuitPropertyID", "ID", equipSuitID, "EquipSuit_Template").Split(';');

            //获取自身套装数量
            int equipSuitNum = Game_PublicClassVar.Get_function_Rose.returnEquipSuitNum(needEquipIDSet, needEquipNumSet);
            //显示套装名称及拥有数量    + "                    查看部件"
            Obj_UIEquipSuitName.GetComponent<Text>().text = equipSuitName + "(" + equipSuitNum + "/" + needEquipIDSet.Length + ")";
            
            for(int i = 0; i<=suitPropertyIDSet.Length-1;i++){
                string triggerSuitNum = suitPropertyIDSet[i].Split(',')[0];
                string triggerSuitPropertyID = suitPropertyIDSet[i].Split(',')[1];
                //显示套装属性
                GameObject propertyObj = (GameObject)Instantiate(Obj_EquipSuitPropertyText);
                propertyObj.transform.SetParent(Obj_UIEquipSuit.transform);
                propertyObj.transform.localScale = new Vector3(1, 1, 1);
                string equipSuitDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSuitDes", "ID", triggerSuitPropertyID, "EquipSuitProperty_Template");
                string ifShowSuitNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ifShowSuitNum", "ID", triggerSuitPropertyID, "EquipSuitProperty_Template");
                if (ifShowSuitNum == "0")
                {
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("件套");
                    propertyObj.GetComponent<Text>().text = triggerSuitNum + langStr + "：" + equipSuitDes;
                }
                else {
                    propertyObj.GetComponent<Text>().text = "            " + equipSuitDes;
                }
                
                //float suitShowTextNum = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowTextNum", "ID", triggerSuitPropertyID, "EquipSuitProperty_Template"));
                float suitShowTextNum = 1;
                suitShowTextNumSum = suitShowTextNumSum + suitShowTextNum;
                propertyObj.transform.localPosition = new Vector3(-12, -30 - 25 * suitShowTextNumSum, 0);
                //propertyObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 25.0f * suitShowTextNumSum);
                //满足条件显示绿色
                if (equipSuitNum >= int.Parse(triggerSuitNum)) {
                    propertyObj.GetComponent<Text>().color = Color.green;
                }
            }

            //累计获取套装显示占用的长度
            equipSuitTextNum = equipSuitTextNum + suitShowTextNumSum * 25.0f+10.0f;
            //Debug.Log("没有套装！");
        }
        else {
            Obj_UIEquipSuit.SetActive(false);
            //Debug.Log("有套装！");
        }


		//显示对应装备属性
		Obj_EquipName.GetComponent<Text>().text = equipName;
		Obj_EquipName.GetComponent<Text>().color = Game_PublicClassVar.Get_function_UI.QualityReturnColor(ItemQuality);
        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("部位");
        Obj_EquipType.GetComponent<Text>().text = langStr + "：" +textEquipType;

        Vector2 equipNeedvec2 = new Vector2(150.0f, -175 - 22 * properShowNum - HintTextNum - showGemTextNum - equipSuitTextNum);
        Obj_EquipNeed.transform.GetComponent<RectTransform>().anchoredPosition = equipNeedvec2;
        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级");
        Obj_EquipWearNeed.GetComponent<Text>().text = langStr + " ： " + equipLv;

        //获取角色等级
        int roseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        if (roseLv < int.Parse(equipLv)) {
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级不足");
            Obj_EquipWearNeed.GetComponent<Text>().text = langStr + " ： " + equipLv + "<color=#ff0000ff>  ("+ langStr_1 + ")</color>";
        }
        int EquipNeedTextNum = 30;

        //此处后面需补充判定某种属性的条件
        //string[] needProperty = "1,40".Split(',');
        string propertyLimit = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyLimit", "ID", ItemID, "Item_Template");
        string[] needProperty = propertyLimit.Split(',');

        if (needProperty[0] != "0")
        {
            //根据属性值返回属性名称
            string propertyName = Game_PublicClassVar.Get_function_UI.ReturnEquipNeedPropertyName(needProperty[0]);
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("需要");
            string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("达到");
            string langStr_4 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(propertyName);
            Obj_EquipWearNeedProperty.GetComponent<Text>().text = langStr_1 + langStr_4 + langStr_2 + " ： " + needProperty[1];

            switch (needProperty[0])
            { 
                //最大攻击
                case "1":
                    int value = Game_PublicClassVar.Get_function_Rose.ReturnRosePropertyValue("1");
                    if (value < int.Parse(needProperty[1])) {
                        //Obj_EquipWearNeedProperty.GetComponent<Text>().color = Color.red;
                        string langStr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击力不足");
                        Obj_EquipWearNeedProperty.GetComponent<Text>().text = langStr_1 + langStr_4 + langStr_2 + " ： " + needProperty[1] + "<color=#ff0000ff>  ("+ langStr_3 + ")</color>";
                    }
                break;
                //后期属性在补充
            }

            //显示值
            Obj_EquipWearNeedProperty.SetActive(true);

            //设置长度
            EquipNeedTextNum = EquipNeedTextNum + 22;
        }
        else {
            Obj_EquipWearNeedProperty.SetActive(false);
        }
		

        //显示道具来源描述文字
        int ItemBlackNum = 0;
        float EquipBottomTextNum = 0;
        //Debug.Log("ItemBlackDes = " + ItemBlackDes);
        if (ItemBlackDes != "0" && ItemBlackDes != "")
        {
            ItemBlackNum = (int)((ItemBlackDes.Length - 16) / 16) + 1;
            Vector2 equipBottomvec2 = new Vector2(150.0f, -170 - 20 * properShowNum - HintTextNum - showGemTextNum - EquipNeedTextNum - ItemBlackNum * 8 - equipSuitTextNum);
            Obj_EquipBottom.transform.GetComponent<RectTransform>().anchoredPosition = equipBottomvec2;
            EquipBottomTextNum = EquipBottomTextNum + 50.0f;
            Obj_EquipDes.GetComponent<Text>().text = ItemBlackDes;
        }
        else
        {
            Obj_EquipBottom.SetActive(false);
        }
        if (ItemBlackDes.Length > 32)
        {
            ItemBlackNum = (int)((ItemBlackDes.Length - 32) / 16) + 1;
            Obj_EquipDes.GetComponent<RectTransform>().sizeDelta = new Vector2(240.0f, 40.0f + 16.0f * ItemBlackNum);
            Obj_EquipDes.GetComponent<Text>().text = ItemBlackDes;
            EquipBottomTextNum = EquipBottomTextNum + 16 * ItemBlackNum;
        }


		Obj_Btn_HuiShou.SetActive(false);
		Obj_Btn_HuiShouCancle.SetActive (false);

        //显示按钮
        switch (EquipTipsType)
        {
			//不显示任何按钮
			case "":
				Obj_BagOpenSet.SetActive(false);
				Obj_RoseEquipOpenSet.SetActive(false);
				Obj_Btn_StoreHouseSet.SetActive(false);
				EquipBottomTextNum = 0;
			break;
			//不显示任何按钮
			case "0":
				Obj_BagOpenSet.SetActive(false);
				Obj_RoseEquipOpenSet.SetActive(false);
				Obj_Btn_StoreHouseSet.SetActive(false);
				EquipBottomTextNum = 0;
				break;

            //背包打开显示对应功能按钮
            case "1":
                Obj_BagOpenSet.SetActive(true);
                Obj_RoseEquipOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(false);
                //判定当前是否打开仓库
                if (Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus)
                {
                    Obj_SaveStoreHouse.SetActive(true);
                    Obj_Diu.SetActive(false);
                }
                else {
                    Obj_SaveStoreHouse.SetActive(false);
                    Obj_Diu.SetActive(true);
                }
                break;
            //角色栏打开显示对应功能按钮
            case "2":
                Obj_BagOpenSet.SetActive(false);
                Obj_RoseEquipOpenSet.SetActive(true);
                Obj_Btn_StoreHouseSet.SetActive(false);
                break;
            //商店查看属性
            case "3":
                Obj_BagOpenSet.SetActive(false);
                Obj_RoseEquipOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(false);
                EquipBottomTextNum = 0;
                break;
            //仓库
            case "4":
                Obj_BagOpenSet.SetActive(false);
                Obj_RoseEquipOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(true);
                //EquipBottomTextNum = 0;
                break;

            //对比查看
            case "5":
                Obj_BagOpenSet.SetActive(false);
                Obj_RoseEquipOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(false);
                //EquipBottomTextNum = 0;
                break;

			//回收背包打开
		    case "6":
				Obj_BagOpenSet.SetActive (true);
				Obj_RoseEquipOpenSet.SetActive (false);
				Obj_Btn_StoreHouseSet.SetActive (false);
				Obj_SaveStoreHouse.SetActive (false);
				Obj_Diu.SetActive(true);
				Obj_Btn_HuiShou.SetActive(true);
				break;

			//回收功能显示
			case "7":
				Obj_BagOpenSet.SetActive(false);
				Obj_RoseEquipOpenSet.SetActive(false);
				Obj_Btn_StoreHouseSet.SetActive(false);
				Obj_Btn_HuiShouCancle.SetActive (true);

				break;
        }

        //设置底图长度
        float DiHight = 250 + 20 * properShowNum + HintTextNum + showGemTextNum + EquipNeedTextNum + EquipBottomTextNum + equipSuitTextNum;
        Obj_Imgback.GetComponent<RectTransform>().sizeDelta = new Vector2(301.0f, DiHight);
		//显示道具Icon
		object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
		Sprite itemIcon = obj as Sprite;
		Obj_EquipIcon.GetComponent<Image>().sprite = itemIcon;

		//显示品质
		object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
		Sprite itemQuality = obj2 as Sprite;
		Obj_EquipQuality.GetComponent<Image>().sprite = itemQuality;

        //监测UI是否超过底部显示
        float screen_higeValue = 768 * Game_PublicClassVar.Get_function_UI.ReturnScreenScalePro();
        float UIHeadValue = screen_higeValue - this.transform.localPosition.y - DiHight/2;            //UI和顶部的距离
        //float UIHeadValue = (screen_higeValue - DiHight) / 2;            //UI和顶部的距离
        //float UIHightValue = Mathf.Abs(UIHeadValue) + DiHight;
        float UIHightValue = DiHight;
        //监测UI是否超过了顶部显示

        if (UIHeadValue <= 30)
        {
            //Debug.Log("UI触顶了");
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, screen_higeValue - DiHight / 2, 0);
            //this.transform.localPosition = new Vector3(this.transform.localPosition.x, screen_higeValue/ 2, 0);
        }


        if (Mathf.Abs(UIHeadValue) + DiHight + 45 >= screen_higeValue) {
            //Debug.Log("UI触底了");
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, DiHight / 2 + 45f, 0);
            //this.transform.localPosition = new Vector3(this.transform.localPosition.x, screen_higeValue / 2, 0);
        }

        //如果太长就缩放
        if (UIHightValue + 50 >= screen_higeValue * 0.9f) {
            float suofangSize = (UIHightValue)/ screen_higeValue;
            suofangSize = 1 / suofangSize;
            suofangSize = 1 - (1-suofangSize) * 0.8f;
            if (suofangSize >= 1) {
                suofangSize = 1;
            }
            if (suofangSize <= 0.85f) {
                suofangSize = 0.85f;
            }
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, screen_higeValue / 2 + 45.0f, 0);
            this.transform.localScale = new Vector3(suofangSize, suofangSize, suofangSize);
        }


    }
	
	// Update is called once per frame
	void Update () {



	}

    //穿戴装备
    public void UseEquip()
    {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        //Debug.Log("我穿了一件装备！");

        Game_PositionVar game_PublicClassVar = Game_PublicClassVar.Get_game_PositionVar;
        //获取装备类型
        string equipType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");
        string equipSpaceType =  Game_PublicClassVar.Get_function_UI.ReturnEquipSpaceNum(equipType);

        //获取此装备的类型,如果是饰品需要弹出选择界面
        if (equipSpaceType == "5")
        {
            GameObject ornamentObj = (GameObject)Instantiate(Obj_UIOrnamentChoice);
            ornamentObj.transform.SetParent(this.gameObject.transform);
            ornamentObj.transform.position = new Vector3(Screen.width/2,Screen.height/2,0);        //在中心,后期分辨率不一样可能需要调整
            ornamentObj.transform.localScale = new Vector3(1, 1, 1);
            ornamentObj.GetComponent<UI_OrnamentChoice>().UIBagSpaceNum = UIBagSpaceNum;
        }
        else {

            //装备
            if (EquipType == 0)
            {
                game_PublicClassVar.ItemMoveValue_Initial = UIBagSpaceNum;
                game_PublicClassVar.ItemMoveType_Initial = "1";
                game_PublicClassVar.ItemMoveValue_End = equipSpaceType;
                game_PublicClassVar.ItemMoveType_End = "2";
                Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
            }

            //十二生肖
            if (EquipType == 1)
            {
                Game_PublicClassVar.Get_function_Rose.RoseEquip_ShengXiao_Wear(UIBagSpaceNum);

                //更新12生肖
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipShengXiaoSet.GetComponent<UI_Rose_ShengXiao>().UpdateStatus = true;

            }

            if (EquipType == 2) {

                if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet != null)
                {
                    string bagSpace = Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect;
                    if (bagSpace != null && bagSpace != "0" && bagSpace != "")
                    {
                        string nowPetID = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().NowSclectPetID;
                        //判断宠物装备等级
                        string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpace, "RoseBag");
                        string itemlv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", itemID, "Item_Template");
                        string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", nowPetID, "RosePet");
                        if (int.Parse(petLv) >= int.Parse(itemlv))
                        {
                            Game_PublicClassVar.Get_function_Rose.PetEquip_Wear(bagSpace, nowPetID, Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().SelectPetEquipSpace);
                            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().Obj_PetEquipShowSet.SetActive(false);
                            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_LeftSet.SetActive(true);
                        }
                        else {
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("等级不足,无法装备");
                        }
                    }
                    else
                    {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请选择需要装备的宠物装备");
                    }
                }
                else {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请打开宠物界面选择需要装备的宠物");
                }
            }

            //删除Tips
            Destroy(this.gameObject);
            //关闭Tips
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
            //更新背包
            //Obj_UIBagSpace.GetComponent<UI_BagSpace>().UpdataItemShow = true;
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            //角色装备更新指定某个装备
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().UpdataEquipOne(equipSpaceType);
            //更新装备属性
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;
            


        }
    }

    //丢弃道具按钮
    public void ThrowItem()
    {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        string equipName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        string equipQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
        string jieshaoStr = "";
        string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_27");
        //有宝石镶嵌时提示
        bool hintStatus = false;
        if (ItemGemID != "" && ItemGemID != "0") {
            string[] gemList = ItemGemID.ToString().Split(',');
            for (int i = 0; i < gemList.Length; i++) {
                if (gemList[i] != "0" && gemList[i] != "") {
                    hintStatus = true;
                    string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_28");
                    jieshaoStr = langStrHint_1 + equipName + langStrHint_2;
                    //jieshaoStr = "是否出售装备:" + equipName + "\n(提示:当前装备上附带宝石!)";
                }
            }
        }

        //出售品质为蓝色,且等级小于15级进行提示
        if (int.Parse(equipQuality) >= 3) {
            int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
            if (roseLv <= 20)
            {
                hintStatus = true;
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_29");
                jieshaoStr = langStrHint_1 + equipName + langStrHint_2;
                //jieshaoStr = "是否出售装备:" + equipName + "\n(提示:在主城回收商人处回收可以获得经验和装备碎片!)";
            }
        }

        //出售品质为蓝色,且等级小于15级进行提示
        if (int.Parse(equipQuality) >= 4)
        {
            hintStatus = true;
            string sellValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", ItemID, "Item_Template");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_30");
            jieshaoStr = langStrHint_1 + equipName + langStrHint_2 + sellValue;
            //jieshaoStr = "是否出售装备:" + equipName + "\n(提示:在主城回收商人处回收可以获得经验和装备碎片!)\n本次出售可以获得金币:" + sellValue;
        }


        //出售提示提示
        if (hintStatus)
        {
            this.gameObject.SetActive(false);
            //关闭UI背景图片
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);
            //弹出提示
            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
            string langStrHint_4 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_13");
            string langStrHint_5 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_14");
            string langStrHint_6 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_15");
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, throwItem, null, langStrHint_4, langStrHint_5, langStrHint_6);
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

        }
        else
        {
            throwItem();
        }

    }


    public void throwItem()
    {
        //如果打开回收界面 出售要即时显示
        if (Game_PublicClassVar.Get_game_PositionVar.HuiShouItemStatus)
        {
            string[] huiShouBagNumList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_HuiShouItem.GetComponent<UI_HuiShouItem>().HuiShouBagNumList;
            for (int i = 0; i < huiShouBagNumList.Length; i++)
            {
                if (huiShouBagNumList[i] == UIBagSpaceNum)
                {
                    //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().Obj_HuiShouItem.GetComponent<UI_HuiShouItem> ().HuiShouItemListShowClearn(i);
                    Game_PublicClassVar.Get_function_UI.HuiShouItem_Cancle(UIBagSpaceNum);
                }
            }
        }

        //Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(ItemID, 1, UIBagSpaceNum, true);
        Game_PublicClassVar.Get_function_Rose.SellBagSpaceItemToMoney(UIBagSpaceNum);
        CloseUI();

        clickBtnStatus = false;  //防止因为卡顿二次执行

    }




    public void CloseUI()
    {
        Destroy(this.gameObject);
			
    }

    //实例化属性文本（showText：展示的文字,showType：展示的颜色（0 白色 1 绿色 2 黄色 3 红色 4 蓝色））
    public void ShowPropertyText(string showText,string showType = "0") {
 
        //实例化一个Obj
        GameObject propertyObj = (GameObject)Instantiate(Obj_EquipPropertyText);
        propertyObj.transform.SetParent(Obj_EquiBase.transform);
        propertyObj.transform.localScale = new Vector3(1, 1, 1);
        propertyObj.GetComponent<Text>().text = showText;
        propertyObj.transform.localPosition = new Vector3(-12, -30 - 22 * properShowNum, 0);

        properShowNum = properShowNum + 1;

        switch (showType) {
            //绿色
            case "1":
                propertyObj.GetComponent<Text>().color = Color.green;
                break;
            //黄色（隐藏技能）
            case "2":
                propertyObj.GetComponent<Text>().color = new Color(1,0.31f,0);
                break;
            //红色
            case "3":
                propertyObj.GetComponent<Text>().color = Color.red;
                break;
            //蓝色
            case "4":
                propertyObj.GetComponent<Text>().color = new Color(0.49f,0.89f,1);
                break;
        }
        //Debug.Log("实例化坐标：" + properShowNum);
    }

    //卸下装备
    public void Btn_TakeEquip() {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        //判断背包是否已满
        string bagFirstNullNum = Game_PublicClassVar.Get_function_Rose.BagFirstNullNum();
        if (bagFirstNullNum != "-1")
        {
            //获取装备类型
            if (EquipType == 0)
            {
                string equipType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");
                string equipSpaceType = Game_PublicClassVar.Get_function_UI.ReturnEquipSpaceNum(equipType);
                Game_PositionVar game_PublicClassVar = Game_PublicClassVar.Get_game_PositionVar;
                game_PublicClassVar.ItemMoveValue_Initial = Obj_UIRoseEquipShow.GetComponent<UI_RoseEquipShow>().EquipSpaceNum;
                game_PublicClassVar.ItemMoveType_Initial = "2";
                game_PublicClassVar.ItemMoveValue_End = bagFirstNullNum;
                game_PublicClassVar.ItemMoveType_End = "1";
                Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
            }
            
            //十二生肖
            if (EquipType == 1)
            {
                Game_PublicClassVar.Get_function_Rose.RoseEquip_ShengXiao_Take(Obj_UIRoseEquipShow.GetComponent<UI_RoseEquipShow>().EquipSpaceNum);
            }

            if (EquipType == 2)
            {
                string nowPetID = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().NowSclectPetID;
                Game_PublicClassVar.Get_function_Rose.PetEquip_Take(nowPetID,Obj_UIRoseEquipShow.GetComponent<UI_RoseEquipShow>().EquipSpaceNum);
            }

            //删除Tips
            Destroy(this.gameObject);
            //关闭UI背景图片
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);
            //更新背包
            //Obj_UIBagSpace.GetComponent<UI_BagSpace>().UpdataItemShow = true;
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            //角色装备更新指定某个装备
            Obj_UIRoseEquipShow.GetComponent<UI_RoseEquipShow>().UpdataStatus = true;
            //更新装备属性
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;

            //获取装备是否携带技能
            string equip_skillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", ItemID, "Item_Template");
            if (equip_skillID != "0" && equip_skillID != "") {
                Game_PublicClassVar.Get_function_Skill.UpdataMainUIEquipSkillID(equip_skillID);
            }

            //判定技能栏是否打开,如果打开就关闭,防止出现BUG（装备技能卸下重装会叠加skillADDID）
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseSkill_Status) {
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseSkill();
            }

        }
    }

    //展示装备套装
    public void Btn_ShowEquipSuit() {
        if (showEquipObj == null)
        {
            showEquipObj = (GameObject)Instantiate(Obj_UISuitEquipName);

            //判定自身是在左边还是右边
            if (this.gameObject.transform.position.x >= Screen.width / 2)
            {
                //左边显示左侧
                showEquipObj.transform.SetParent(EquipSuitShowSet_Lelt.transform);
                showEquipObj.transform.localScale = new Vector3(1, 1, 1);
                showEquipObj.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20, -106);
                showEquipObj.GetComponent<UI_SuitNeedEquipName>().SuitShowDirection = "1";
                //Debug.Log("在左边");
            }
            else
            {
                //左面显示右侧
                showEquipObj.transform.SetParent(EquipSuitShowSet_Right.transform);
                showEquipObj.transform.localScale = new Vector3(1, 1, 1);
                showEquipObj.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(27, -106);
                showEquipObj.GetComponent<UI_SuitNeedEquipName>().SuitShowDirection = "2";
                //Debug.Log("在右边");
            }
            showEquipObj.GetComponent<UI_SuitNeedEquipName>().SuitID = equipSuitID;
        }
        else {
            Destroy(showEquipObj);
        }
    }

    //存入仓库
    public void Btn_SaveStoreHouse() {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行


        //获取仓库是否已经满了
        int nullNum = Game_PublicClassVar.Get_function_Rose.StoreHouseNullNum();
        if (nullNum <= 0)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_82");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("仓库已满！");
            return;
        }

        //获取存入道具的数据
        //string save_ItemID = ItemID;
        //获取隐藏属性ID
        //string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", UIBagSpaceNum, "RoseBag");
        //删除指定道具
        //Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(save_ItemID, 1, UIBagSpaceNum, true);
        //添加指定道具到仓库
        //Debug.Log("UIBagSpaceNum = " + UIBagSpaceNum + ";   hideID = " + hideID);
        //Game_PublicClassVar.Get_function_Rose.SendRewardToStoreHouse(save_ItemID, 1, "1", hideID);

        if (Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu == 0) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_83");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("仓库未打开！");
            clickBtnStatus = false;
            return;
        }

        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial = "1";
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial = UIBagSpaceNum;

        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "3";
        string nullspace = Game_PublicClassVar.Get_function_Rose.StoreHouse_ReturnNullSpaceNum(Game_PublicClassVar.Get_game_PositionVar.HouseBagYeShu.ToString());
        if (nullspace == "-1") {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_82");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("仓库已满!");
            clickBtnStatus = false;
            return;
        }
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = nullspace;

        //执行交换
        Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();

        //更新显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        Destroy(this.gameObject);
        Game_PublicClassVar.Get_function_UI.DestoryTipsUI();    //删除所有Tips
    }

    //取出仓库
    public void Btn_TaskStoreHouse() {

        if (clickBtnStatus) {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        //获取存入道具的数据
        //string save_ItemID = ItemID;
        //获取隐藏属性ID
        //string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", UIBagSpaceNum, "RoseStoreHouse");
        //删除指定道具
        //Game_PublicClassVar.Get_function_Rose.CostStoreHouseSpaceNumItem(save_ItemID, 1, UIBagSpaceNum, true);
        //添加指定道具到背包
        //Game_PublicClassVar.Get_function_Rose.SendRewardToBag(save_ItemID, 1, "1",0, hideID);


        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial = "3";
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial = UIBagSpaceNum;

        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "1";
        string nullspace = Game_PublicClassVar.Get_function_Rose.BagFirstNullNum();
        if (nullspace == "-1")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_82");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("仓库已满!");
            clickBtnStatus = false;
            return;
        }
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = nullspace;

        //执行交换
        Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();


        //更新显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        Destroy(this.gameObject);
        Game_PublicClassVar.Get_function_UI.DestoryTipsUI();    //删除所有Tips
    }

    //显示宝石
    public void TipsShowEquipGem(GameObject gemIconObj,GameObject gemTextObj,string gemItemID,string gemHoleID) {

        //Debug.Log("gemItemID = " + gemItemID + "gemHoleID = " + gemHoleID);

        //获取道具图标和名称
        if (gemItemID != "" && gemItemID != "0")
        {
            //显示宝石
            string gemItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", gemItemID, "Item_Template");
            string gemItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", gemItemID, "Item_Template");
            string gemItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", gemItemID, "Item_Template");
            
            object obj = Resources.Load("ItemIcon/" + gemItemIcon, typeof(Sprite));
            Sprite itemIcon = obj as Sprite;
            gemIconObj.GetComponent<Image>().sprite = itemIcon;
            gemTextObj.GetComponent<Text>().text = gemItemName;
            gemTextObj.GetComponent<Text>().color = Game_PublicClassVar.Get_function_UI.QualityReturnColor(gemItemQuality);

        }else
        {
            //表示空的宝石槽位
            switch (gemHoleID) { 
                //紫色宝石
                case "101":
                    //显示名称
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("红色插槽");
                    gemTextObj.GetComponent<Text>().text = langStr;
                    //显示空图标
                    object obj = Resources.Load("GemHoleDi/" + gemHoleID, typeof(Sprite));
                    Sprite itemIcon = obj as Sprite;
                    gemIconObj.GetComponent<Image>().sprite = itemIcon;
                    break;
                //红色宝石
                case "102":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("紫色插槽");
                    gemTextObj.GetComponent<Text>().text = langStr;
                    //显示空图标
                    obj = Resources.Load("GemHoleDi/" + gemHoleID, typeof(Sprite));
                    itemIcon = obj as Sprite;
                    gemIconObj.GetComponent<Image>().sprite = itemIcon;
                    break;
                //蓝色宝石
                case "103":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("蓝色插槽");
                    gemTextObj.GetComponent<Text>().text = langStr;
                    //显示空图标
                    obj = Resources.Load("GemHoleDi/" + gemHoleID, typeof(Sprite));
                    itemIcon = obj as Sprite;
                    gemIconObj.GetComponent<Image>().sprite = itemIcon;
                    break;
                //绿色孔位
                case "104":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("绿色插槽");
                    gemTextObj.GetComponent<Text>().text = langStr;
                    //显示空图标
                    obj = Resources.Load("GemHoleDi/" + gemHoleID, typeof(Sprite));
                    itemIcon = obj as Sprite;
                    gemIconObj.GetComponent<Image>().sprite = itemIcon;
                    break;
                //白色孔位
                case "105":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("白色插槽");
                    gemTextObj.GetComponent<Text>().text = langStr;
                    //显示空图标
                    obj = Resources.Load("GemHoleDi/" + gemHoleID, typeof(Sprite));
                    itemIcon = obj as Sprite;
                    gemIconObj.GetComponent<Image>().sprite = itemIcon;
                    break;
                //多彩插槽
                case "110":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("多彩插槽");
                    gemTextObj.GetComponent<Text>().text = langStr;
                    //显示空图标
                    obj = Resources.Load("GemHoleDi/" + gemHoleID, typeof(Sprite));
                    itemIcon = obj as Sprite;
                    gemIconObj.GetComponent<Image>().sprite = itemIcon;
                    break;
            }
        }
    }

    //展示隐藏技能
    public string ShowHintSkill(string AddPropreListStr)
    {
        string proprety = AddPropreListStr.Split(',')[0];
        string propretyValue = AddPropreListStr.Split(',')[1];
        string textShow = "";
        switch (proprety)
        { 
            case "10001":
                //获取技能名称
                string skillName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillName", "ID", propretyValue, "Skill_Template");
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("隐藏属性");
                textShow = langStr + "：" + skillName;
                break;
        }
        return textShow;
    }



    //展示隐藏属性
    public string ShowHintPro(string AddPropreListStr)
    {
        string proprety = AddPropreListStr.Split(',')[0];
        string propretyValue = AddPropreListStr.Split(',')[1];
        string textShow = "";

        if (proprety == "FuMo") {

            proprety = AddPropreListStr.Split(',')[1];
            propretyValue = AddPropreListStr.Split(',')[2];
            textShow = "附魔效果：";
        }

        if (EquipType == 0 || EquipType == 1)
        {
            switch (proprety)
            {

                //血量
                case "10":
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("血量");
                    textShow = textShow + langStr + propretyValue;
                    break;

                //物理最小攻击
                case "11":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击提高");
                    textShow = textShow + langStr + propretyValue;
                    break;

                //魔法攻击
                case "14":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("法术提高");
                    textShow = textShow + langStr + propretyValue;
                    break;

                //物理防御
                case "17":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("物防提高");
                    textShow = textShow + langStr + propretyValue;
                    break;

                //魔法防御
                case "20":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("魔防提高");
                    textShow = textShow + langStr + propretyValue;
                    break;

                //暴击
                case "30":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("暴击概率提升");
                    textShow = langStr + " ：" + float.Parse(propretyValue) * 100 + "%";
                    break;

                //命中
                case "31":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("命中概率提高");
                    textShow = langStr + " ：" + float.Parse(propretyValue) * 100 + "%";
                    break;

                //闪避
                case "32":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪避概率提高");
                    textShow = langStr + " ：" + float.Parse(propretyValue) * 100 + "%";
                    break;

                //物理免伤
                case "33":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("受到攻击伤害降低");
                    textShow = langStr + "：" + float.Parse(propretyValue) * 100 + "%";
                    break;

                //魔法免伤
                case "34":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("受到法术伤害降低");
                    textShow = langStr + "：" + float.Parse(propretyValue) * 100 + "%";
                    break;

                //速度
                case "35":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("速度提高");
                    textShow = langStr + " ： " + propretyValue;
                    break;

                //伤害免伤
                case "36":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("受到伤害降低");
                    textShow = langStr + " ： " + float.Parse(propretyValue) * 100 + "%";
                    break;

                //血量百分比
                case "50":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("血量上限提升");
                    textShow = langStr + " ： " + float.Parse(propretyValue) * 100 + "%";
                    break;

                //物理攻击(百分比)
                case "51":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击伤害提升");
                    textShow = langStr + " ： " + float.Parse(propretyValue) * 100 + "%";
                    break;

                //魔法攻击(百分比)
                case "52":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("法术伤害提升");
                    textShow = langStr + " ： " + float.Parse(propretyValue) * 100 + "%";
                    break;

                //物理防御(百分比)
                case "53":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("物防提升");
                    textShow = langStr + " ： " + float.Parse(propretyValue) * 100 + "%";
                    break;

                //魔法防御(百分比)
                case "54":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("魔防提升");
                    textShow = langStr + " ： " + float.Parse(propretyValue) * 100 + "%";
                    break;

                //幸运
                case "100":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("幸运值");
                    textShow = langStr + " ： " + propretyValue;
                    break;

                //格挡值
                case "101":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("格挡值额外提高");
                    textShow = langStr + propretyValue;
                    break;

                //重击概率
                case "111":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("重击概率额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //重击附加伤害值
                case "112":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("重击伤害额外提高");
                    textShow = langStr + propretyValue;
                    break;

                //每次普通攻击附加的伤害值
                case "121":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("固定伤害额外提高");
                    textShow = langStr + propretyValue;
                    break;

                //忽视目标防御值
                case "131":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("忽视目标防御额外提高");
                    textShow = langStr + propretyValue;
                    break;

                //忽视目标魔防值
                case "132":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("忽视目标魔防额外提高");
                    textShow = langStr + propretyValue;
                    break;

                //忽视目标防御值
                case "133":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击穿透");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //忽视目标魔防值
                case "134":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("法术穿透");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //吸血概率
                case "141":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("吸血率额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //法术反击
                case "151":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("法术反弹额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //攻击反击
                case "152":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击反弹额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //韧性概率
                case "161":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("韧性概率额外提高");
                    textShow = langStr + " ：" + float.Parse(propretyValue) * 100 + "%";
                    break;

                //暴击等级
                case "201":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("暴击等级额外提高");
                    textShow = langStr + int.Parse(propretyValue);
                    break;

                //韧性等级
                case "202":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("韧性等级额外提高");
                    textShow = langStr + int.Parse(propretyValue);
                    break;

                //命中等级
                case "203":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("命中等级额外提高");
                    textShow = langStr + int.Parse(propretyValue);
                    break;

                //闪避等级
                case "204":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪避等级额外提高");
                    textShow = langStr + int.Parse(propretyValue);
                    break;

                //光抗性
                case "301":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("神圣抗性额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //暗抗性
                case "302":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("暗影抗性额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //火抗性
                case "303":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("火焰抗性额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //水抗性
                case "304":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("冰霜抗性额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //电抗性
                case "305":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪电抗性额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //野兽攻击抗性
                case "321":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("野兽攻击抗性额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //人物攻击抗性
                case "322":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("人形攻击抗性额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //恶魔攻击抗性
                case "323":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("恶魔攻击抗性额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //经验加成
                case "401":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("经验收益额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //金币加成
                case "402":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("金币收益额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //洗炼极品掉落（祝福值）
                case "403":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("极品值额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //装备隐藏属性出现概率
                case "404":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("祝福值额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //装备上的宝石槽位出现概率
                case "405":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("运气值额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //经验加成固定
                case "406":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("经验收益额外提高");
                    textShow = langStr + propretyValue;
                    break;

                //金币加成固定
                case "407":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("金币收益额外提高");
                    textShow = langStr + propretyValue;
                    break;
                //药剂类熟练度
                case "408":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("药剂类熟练度额外提高");
                    textShow = langStr + propretyValue;
                    break;
                //锻造类熟练度
                case "409":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("锻造类熟练度额外提高");
                    textShow = langStr + propretyValue;
                    break;
                //复活
                case "411":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("复活几率额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;
                //攻击无视防御
                case "412":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("无视防御额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;
                //神农
                case "413":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("神农值额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;
                //额外掉落
                case "414":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("财富值额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;
                //伪装  +增大发现范围   -缩小范围
                case "415":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("伪装值额外提高");
                    textShow = langStr + propretyValue;
                    break;
                //灾难
                case "416":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("灾难值额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;
                //嗜血概率
                case "417":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("嗜血几率额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //怪物脱战距离
                case "418":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("怪物脱战距离额外提高");
                    textShow = langStr + propretyValue;
                    break;

                //专注概率
                case "419":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("专注额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //怪物脱战距离
                case "420":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("必中额外提高");
                    textShow = langStr + propretyValue;
                    break;

                //生产药剂暴击概率
                case "421":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("生产药剂暴击概率额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;
            }
        }

        if (EquipType == 2)
        {
            switch (proprety)
            {

                //血量
                case "11":
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击附加伤害+");
                    textShow = textShow + langStr + propretyValue;
                    break;
                //血量
                case "15":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("魔法附加伤害+");
                    textShow = textShow + langStr + propretyValue;
                    break;
                //血量
                case "21":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("物伤伤害抵抗+");
                    textShow = textShow + langStr + propretyValue;
                    break;
                //血量
                case "31":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("魔防伤害抵抗+");
                    textShow = textShow + langStr + propretyValue;
                    break;
                //血量
                case "41":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("血量附加+");
                    textShow = textShow + langStr + propretyValue;
                    break;

                case "101":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("暴击率+");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                case "104":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("闪避率+");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                case "103":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("命中率+");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                case "102":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("暴击抵抗+");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                case "16":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("魔攻增加+");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                case "42":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("血量增加+");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                case "12":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("物攻增加+");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                case "22":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("物防增加+");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                case "32":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("魔防增加+");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                case "141":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("技能抵抗增加+");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;
            }
        }
        return textShow;
    }


	//点击回收按钮
	public void Btn_HuiShou(){
		Game_PublicClassVar.Get_function_UI.HuiShouItem_Add(UIBagSpaceNum);
		Destroy(this.gameObject);
		Game_PublicClassVar.Get_function_UI.DestoryTipsUI();    //删除所有Tips
	}

	//点击取消回收按钮
	public void Btn_HuiShouCancle(){
		Game_PublicClassVar.Get_function_UI.HuiShouItem_Cancle(UIBagSpaceNum);
		Destroy(this.gameObject);
		Game_PublicClassVar.Get_function_UI.DestoryTipsUI();    //删除所有Tips
	}
}
