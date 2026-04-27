using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_RoseEquip : MonoBehaviour {

    public GameObject Obj_RoseEquipSet;
    public Transform EquipList;
    public string RoseEquipStatus;     // 1.背包(未制作)  2.属性(未制作)  3.镶嵌(未制作)  4.强化
    //子级的控件列表
    private GameObject obj_Equip_Weapon;
    private GameObject obj_Equip_Clothes;
    private GameObject obj_Equip_Amulet;
    private GameObject obj_Equip_GodsStone;
    private GameObject obj_Equip_Ornament3;
    private GameObject obj_Equip_Ornament2;
    private GameObject obj_Equip_Ornament1;
    private GameObject obj_Equip_Shoes;
    private GameObject obj_Equip_Pants;
    private GameObject obj_Equip_Belt;
    private GameObject obj_Equip_Bracelet;
    private GameObject obj_Equip_Helmet;
    private GameObject obj_Equip_Necklace;
    private GameObject EquipQuality;
    private GameObject EquipIcon;
    private GameObject EquipDi;
    public GameObject Btn_ShengXiao;

    private string str_EquipItemID;
    private string str_EquipQuality;
    private string str_EquipIcon;

    private GameObject gameStartVar;
    private Game_PositionVar game_PositionVar;

    //UI部分
    public GameObject Obj_RoseEquipDiSet;
    public GameObject Obj_Bag;
    public GameObject Obj_Property;
    public GameObject Obj_EquipGemHoleSet;
    public GameObject Obj_EquipQiangHuaSet;
    public GameObject Obj_FunctionSetBtn;
    public GameObject Obj_RoseModelImg;
    public GameObject Obj_EquipQiangHuaSpaceSet;
    public GameObject Obj_EquipQiangHuaProSet;
    public GameObject Obj_EquipQiangHuaDiSet;
    public GameObject Obj_EquipShengXiaoSet;

    public GameObject Obj_EquipBtnText_1;
    public GameObject Obj_EquipBtnText_2;
    public GameObject Obj_EquipBtnText_3;
    public GameObject Obj_EquipBtnText_4;

    public Transform Obj_RoseProMoveTra;
    public Transform Obj_RoseProMask;

    public GameObject ObjBtn_Bag;
    public GameObject ObjBtn_Property;
    public GameObject ObjBtn_Gem;
    public GameObject ObjBtn_QiangHua;

    private float scalePro;
    private float lastMovePosition_Y;

	//属性_基础部分
	public GameObject Obj_RoseHp;
	public GameObject Obj_RoseAct;
	public GameObject Obj_RoseDef;
    public GameObject Obj_RoseAdf;
    public GameObject Obj_RoseHuoLi;
    public GameObject Obj_RoseTiLi;

    //属性_特殊属性
	public GameObject Obj_RoseHit;
	public GameObject Obj_RoseCri;
	public GameObject Obj_RoseDodge;
    public GameObject Obj_RoseRenXing;
    public GameObject Obj_RoseXiXue;
    public GameObject Obj_RoseGeDang;
    public GameObject Obj_ActChuanTou;
    public GameObject Obj_MageActChuanTou;
    public GameObject Obj_ZhongJi;
    public GameObject Obj_ZhenShiDamge;
    public GameObject Obj_ActFanTan;
    public GameObject Obj_MageFanTan;
    public GameObject Obj_HuShiDef;
    public GameObject Obj_HuShiAdf;
	public GameObject Obj_RoseDamgeAdd;
	public GameObject Obj_RoseDamgeMinu;
	public GameObject Obj_RoseMoveSpeed;
    public GameObject Obj_RoseLucky;
    public GameObject Obj_RoseYeShouAdd;
    public GameObject Obj_RoseRenXingAdd;
    public GameObject Obj_RoseEMoAdd;
    public GameObject Obj_RoseMage;

    //属性_角色信息
    public GameObject Obj_RoseKangXing_ShenSheng;
    public GameObject Obj_RoseKangXing_AnYing;
    public GameObject Obj_RoseKangXing_BingShuang;
    public GameObject Obj_RoseKangXing_HuoYan;
    public GameObject Obj_RoseKangXing_ShanDian;
    public GameObject Obj_RoseKangXing_YeShou;
    public GameObject Obj_RoseKangXing_RenWu;
    public GameObject Obj_RoseKangXing_EMo;
    
    //属性_角色信息
    public GameObject Obj_RoseName;
    public GameObject Obj_RoseLv;
    public GameObject Obj_RoseExp;


	// Use this for initialization
	void Start () {

        //找到对应绑点
        obj_Equip_Weapon = EquipList.Find("Equip_1").gameObject;
        obj_Equip_Clothes = EquipList.Find("Equip_2").gameObject;
        obj_Equip_Amulet = EquipList.Find("Equip_3").gameObject;
        obj_Equip_GodsStone = EquipList.Find("Equip_4").gameObject;
        obj_Equip_Ornament3 = EquipList.Find("Equip_5").gameObject;
        obj_Equip_Ornament2 = EquipList.Find("Equip_6").gameObject;
        obj_Equip_Ornament1 = EquipList.Find("Equip_7").gameObject;
        obj_Equip_Shoes = EquipList.Find("Equip_8").gameObject;
        obj_Equip_Pants = EquipList.Find("Equip_9").gameObject;
        obj_Equip_Belt = EquipList.Find("Equip_10").gameObject;
        obj_Equip_Bracelet = EquipList.Find("Equip_11").gameObject;
        obj_Equip_Helmet = EquipList.Find("Equip_12").gameObject;
        obj_Equip_Necklace = EquipList.Find("Equip_13").gameObject;
        
        //更新装备数据
        UpdateAllRoseEquip();
        //因为刚才已经更新,所以防止二次更新
        if (Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip) {
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = false;
        }

        //打开通用UI
        Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject, "101");

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_RoseEquipSet);

        //更新名称、等级、经验
        Obj_RoseName.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_RoseLv.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        long nowExp = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ExpNow;
        long sumExp = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Exp;
        Obj_RoseExp.GetComponent<Text>().text = nowExp.ToString() + "/" + sumExp.ToString();


        /*
        scalePro = Game_PublicClassVar.Get_function_UI.ReturnScreenScalePro();
        Obj_RoseEquipSet.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        Obj_RoseEquipSet.transform.GetComponent<RectTransform>().anchoredPosition3D = Obj_RoseEquipSet.transform.GetComponent<RectTransform>().anchoredPosition3D * scalePro;
        */
        /*
        Debug.Log("EquipList.transform.localPosition = " + EquipList.transform.GetComponent<RectTransform>().anchoredPosition3D);
        Debug.Log("EquipList.transform.localPosition = " + EquipList.transform.GetComponent<RectTransform>().anchoredPosition3D);
        EquipList.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        EquipList.transform.GetComponent<RectTransform>().anchoredPosition3D = EquipList.transform.GetComponent<RectTransform>().anchoredPosition3D * scalePro;

        //Obj_RoseEquipSet.transform.

        Obj_RoseEquipDiSet.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        Obj_RoseEquipDiSet.transform.GetComponent<RectTransform>().anchoredPosition3D = Obj_RoseEquipDiSet.transform.GetComponent<RectTransform>().anchoredPosition3D * scalePro;

        //背包_界面适配
        Obj_Bag.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        Obj_Bag.transform.GetComponent<RectTransform>().anchoredPosition3D = Obj_Bag.transform.GetComponent<RectTransform>().anchoredPosition3D / scalePro;
        //属性_界面适配
        Obj_Property.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        Obj_Property.transform.GetComponent<RectTransform>().anchoredPosition3D = Obj_Property.transform.GetComponent<RectTransform>().anchoredPosition3D * scalePro;
        //装备宝石_界面适配
        Obj_EquipGemHoleSet.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        Obj_EquipGemHoleSet.transform.GetComponent<RectTransform>().anchoredPosition3D = Obj_EquipGemHoleSet.transform.GetComponent<RectTransform>().anchoredPosition3D * scalePro;
        //装备强化_界面适配
        //Obj_EquipQiangHuaSet.transform.localScale = new Vector3(scalePro, scalePro, scalePro);

        Obj_EquipQiangHuaSpaceSet.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        //Vector3 vec3 = new Vector3(1, scalePro,1);
        Obj_EquipQiangHuaSpaceSet.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(Obj_EquipQiangHuaSpaceSet.transform.GetComponent<RectTransform>().anchoredPosition3D.x, Obj_EquipQiangHuaSpaceSet.transform.GetComponent<RectTransform>().anchoredPosition3D.y * scalePro);
        Obj_EquipQiangHuaProSet.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        Obj_EquipQiangHuaProSet.transform.GetComponent<RectTransform>().anchoredPosition3D = Obj_EquipQiangHuaProSet.transform.GetComponent<RectTransform>().anchoredPosition3D * scalePro;
        //功能按钮_界面适配
        Obj_FunctionSetBtn.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        //Obj_FunctionSetBtn.transform.GetComponent<RectTransform>().anchoredPosition3D = Obj_FunctionSetBtn.transform.GetComponent<RectTransform>().anchoredPosition3D / scalePro;
        */
        //updataRoseProprety();

        //默认打开背包
        Click_Type("1");

        //初始化标签按钮
        if (Game_PublicClassVar.Get_wwwSet.GameSetLanguage._Language != "Chinese") {
            Obj_EquipBtnText_1.GetComponent<Text>().fontSize = 20;
            Obj_EquipBtnText_2.GetComponent<Text>().fontSize = 20;
            Obj_EquipBtnText_3.GetComponent<Text>().fontSize = 20;
            Obj_EquipBtnText_4.GetComponent<Text>().fontSize = 20;
        }

        //打开摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_EquipRoseSet.SetActive(true);


        //获取等级
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= 50)
        {
            Btn_ShengXiao.SetActive(true);
        }
        else {
            Btn_ShengXiao.SetActive(false);
        }

    }
	
	// Update is called once per frame
	void Update () {
        //更新属性
        if (Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip) {
            updataRoseProprety();
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = false;
        }
	}

    private void OnDestroy()
    {
        //关闭摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_EquipRoseSet.SetActive(false);
    }

    void UpdateAllRoseEquip()
    {
        UpdateRoseEquipDate(obj_Equip_Weapon, 1);
        UpdateRoseEquipDate(obj_Equip_Clothes, 2);
        UpdateRoseEquipDate(obj_Equip_Amulet, 3);
        UpdateRoseEquipDate(obj_Equip_GodsStone, 4);
        UpdateRoseEquipDate(obj_Equip_Ornament3, 5);
        UpdateRoseEquipDate(obj_Equip_Ornament2, 6);
        UpdateRoseEquipDate(obj_Equip_Ornament1, 7);
        UpdateRoseEquipDate(obj_Equip_Shoes, 8);
        UpdateRoseEquipDate(obj_Equip_Pants, 9);
        UpdateRoseEquipDate(obj_Equip_Belt, 10);
        UpdateRoseEquipDate(obj_Equip_Bracelet, 11);
        UpdateRoseEquipDate(obj_Equip_Helmet, 12);
        UpdateRoseEquipDate(obj_Equip_Necklace, 13);
    }

    //显示装备数据
    void UpdateRoseEquipDate(GameObject RoseEquipSpaceName, int bagSpaceNum)
    {
        //获取装备底层显示的文字
        //EquipDi = RoseEquipSpaceName.transform.Find("Img_EquipBackText").gameObject;
        //获取道具数据
        str_EquipItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", bagSpaceNum.ToString(), "RoseEquip");

        RoseEquipSpaceName.GetComponent<UI_RoseEquipShow>().EquipID = str_EquipItemID;
        RoseEquipSpaceName.GetComponent<UI_RoseEquipShow>().UpdataStatus = true;

        /*
        //绑定Tips脚本
        UI_ItemTips ui_ItemTips = RoseEquipSpaceName.transform.Find("Btn_Equip").gameObject.AddComponent<UI_ItemTips>();

        //将信息赋值给脚本
        ui_ItemTips.ItemID = str_EquipItemID;
        ui_ItemTips.ItemType = "2";
        ui_ItemTips.ItemTypeValue = bagSpaceNum.ToString();
        */
        //判定当前装备栏位是否有装备道具
        if (str_EquipItemID != "0")
        {

            //str_EquipQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", str_EquipItemID, "Item_Template");
            //str_EquipIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", str_EquipItemID, "Item_Template");

            //Debug.Log("当前道具序号：" + bagSpaceNum + "当前道具品质" + str_EquipQuality + "当前道具ID" + str_EquipItemID);

            //EquipDi.active = false;

            //显示数据
            //显示道具图标
            /*
            UITexture equipIconTexture = RoseEquipSpaceName.transform.Find("EquipIcon").GetComponent<UITexture>();
            Function_UI function_ui = new Function_UI();
            equipIconTexture.mainTexture = (Texture2D)Resources.Load("ItemIcon/" + str_EquipIcon);
            

            //显示道具品质
            UISprite itemQualitySprite = RoseEquipSpaceName.transform.Find("EquipQuality").GetComponent<UISprite>();
            itemQualitySprite.spriteName = "ItemQuality_" + str_EquipQuality;
            */
        }
        else
        {

            //当被格子没有到道具清空显示信息
            /*
            UITexture equipIconTexture = RoseEquipSpaceName.transform.Find("EquipIcon").GetComponent<UITexture>();
            equipIconTexture.mainTexture = (Texture2D)Resources.Load("");
            UISprite uispriteQuality = RoseEquipSpaceName.transform.Find("EquipQuality").GetComponent<UISprite>();
            uispriteQuality.spriteName = "";
            */
            //EquipDi.active = true;
        }

    }

    //更新角色属性
    void updataRoseProprety() {

        //Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty();    打开会卡,所以此处不进行实时属性更新

        Function_Rose function_Rose = Game_PublicClassVar.Get_function_Rose;
        Rose_Proprety rose_Proprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        //显示基础属性
        Obj_RoseHp.GetComponent<Text>().text = rose_Proprety.Rose_HpNow.ToString() + "/" + rose_Proprety.Rose_Hp.ToString();
        Obj_RoseAct.GetComponent<Text>().text = rose_Proprety.Rose_ActMin.ToString() + "-" + rose_Proprety.Rose_ActMax.ToString();
        Obj_RoseDef.GetComponent<Text>().text = rose_Proprety.Rose_DefMin.ToString() + "-" + rose_Proprety.Rose_DefMax.ToString();
        Obj_RoseAdf.GetComponent<Text>().text = rose_Proprety.Rose_AdfMin.ToString() + "-" + rose_Proprety.Rose_AdfMax.ToString();
        Obj_RoseHuoLi.GetComponent<Text>().text = function_Rose.GetRoseHuoLi() + "/100";
        Obj_RoseTiLi.GetComponent<Text>().text = function_Rose.GetRoseTili() + "/100";

        //显示特殊属性
        Obj_RoseHit.GetComponent<Text>().text = (rose_Proprety.Rose_Hit * 100).ToString("0.##") + "%";
        Obj_RoseCri.GetComponent<Text>().text = (rose_Proprety.Rose_Cri * 100).ToString("0.##") + "%";
        Obj_RoseDodge.GetComponent<Text>().text = (rose_Proprety.Rose_Dodge * 100).ToString("0.##") + "%";
        Obj_RoseRenXing.GetComponent<Text>().text = (rose_Proprety.Rose_Res * 100).ToString("0.##") + "%";
        Obj_RoseXiXue.GetComponent<Text>().text = (rose_Proprety.Rose_XiXuePro * 100).ToString("0.##") + "%";
        Obj_RoseGeDang.GetComponent<Text>().text = (rose_Proprety.Rose_GeDangValue).ToString();
        Obj_ActChuanTou.GetComponent<Text>().text = (rose_Proprety.Rose_HuShiDefValuePro * 100).ToString("0.##") + "%";
        Obj_MageActChuanTou.GetComponent<Text>().text = (rose_Proprety.Rose_HuShiAdfValuePro * 100).ToString("0.##") + "%";
        Obj_ZhongJi.GetComponent<Text>().text = (rose_Proprety.Rose_ZhongJiPro * 100).ToString("0.##") + "%";
        Obj_ZhenShiDamge.GetComponent<Text>().text = (rose_Proprety.Rose_GuDingValue).ToString("0.##");
        Obj_ActFanTan.GetComponent<Text>().text = (rose_Proprety.Rose_ActRebound * 100).ToString("0.##") + "%";
        Obj_MageFanTan.GetComponent<Text>().text = (rose_Proprety.Rose_MagicRebound * 100).ToString("0.##") + "%";
        Obj_RoseDamgeAdd.GetComponent<Text>().text = (rose_Proprety.Rose_DamgeAdd * 100).ToString("0.##") + "%";
        Obj_RoseDamgeMinu.GetComponent<Text>().text = (rose_Proprety.Rose_DamgeSub * 100).ToString("0.##") + "%";
        Obj_HuShiDef.GetComponent<Text>().text = (rose_Proprety.Rose_HuShiDefValue).ToString();
        Obj_HuShiAdf.GetComponent<Text>().text = (rose_Proprety.Rose_HuShiAdfValue).ToString();
        Obj_RoseYeShouAdd.GetComponent<Text>().text = (rose_Proprety.Rose_RaceDamge_1 * 100).ToString("0.##") + "%";
        Obj_RoseRenXingAdd.GetComponent<Text>().text = (rose_Proprety.Rose_RaceDamge_2 * 100).ToString("0.##") + "%";
        Obj_RoseEMoAdd.GetComponent<Text>().text = (rose_Proprety.Rose_RaceDamge_3 * 100).ToString("0.##") + "%";
        Obj_RoseMage.GetComponent<Text>().text = rose_Proprety.Rose_MagActMax.ToString();
        Obj_RoseMoveSpeed.GetComponent<Text>().text = rose_Proprety.Rose_MoveSpeed.ToString();
        Obj_RoseLucky.GetComponent<Text>().text = rose_Proprety.Rose_Lucky.ToString() + "/9";

        //显示角色抗性
        Obj_RoseKangXing_ShenSheng.GetComponent<Text>().text = "+" + (rose_Proprety.Rose_Resistance_1 * 100).ToString("0.##") + "%";
        Obj_RoseKangXing_AnYing.GetComponent<Text>().text = "+" + (rose_Proprety.Rose_Resistance_2 * 100).ToString("0.##") + "%";
        Obj_RoseKangXing_BingShuang.GetComponent<Text>().text = "+" + (rose_Proprety.Rose_Resistance_3 * 100).ToString("0.##") + "%";
        Obj_RoseKangXing_HuoYan.GetComponent<Text>().text = "+" + (rose_Proprety.Rose_Resistance_4 * 100).ToString("0.##") + "%";
        Obj_RoseKangXing_ShanDian.GetComponent<Text>().text = "+" + (rose_Proprety.Rose_Resistance_5 * 100).ToString("0.##") + "%";
        Obj_RoseKangXing_YeShou.GetComponent<Text>().text = "+" + (rose_Proprety.Rose_RaceResistance_1 * 100).ToString("0.##") + "%";
        Obj_RoseKangXing_RenWu.GetComponent<Text>().text = "+" + (rose_Proprety.Rose_RaceResistance_2 * 100).ToString("0.##") + "%";
        Obj_RoseKangXing_EMo.GetComponent<Text>().text = "+" + (rose_Proprety.Rose_RaceResistance_3 * 100).ToString("0.##") + "%";



    }

	public void CloseUI(){
        Destroy (this.gameObject);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseEquip_Status = false;
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
	}

    public void UpdataEquipOne(string updataEquipNum) {
        //更新装备
        switch (updataEquipNum) { 
            
            case "1":
                UpdateRoseEquipDate(obj_Equip_Weapon, 1);
                break;

            case "2":
                UpdateRoseEquipDate(obj_Equip_Clothes, 2);
                break;
                    
            case "3":
                UpdateRoseEquipDate(obj_Equip_Amulet, 3);
                break;
                
            case "4":
                UpdateRoseEquipDate(obj_Equip_GodsStone, 4);
                break;

            case "5":
                UpdateRoseEquipDate(obj_Equip_Ornament3, 5);
                break;

            case "6":
                UpdateRoseEquipDate(obj_Equip_Ornament2, 6);
                break;

            case "7":
                UpdateRoseEquipDate(obj_Equip_Ornament1, 7);
                break;

            case "8":
                UpdateRoseEquipDate(obj_Equip_Shoes, 8);
                break;
                
            case "9":
                UpdateRoseEquipDate(obj_Equip_Pants, 9);
                break;
  
            case "10":
                UpdateRoseEquipDate(obj_Equip_Belt, 10);
                break;

            case "11":
                UpdateRoseEquipDate(obj_Equip_Bracelet, 11);
                break;

            case "12":
                UpdateRoseEquipDate(obj_Equip_Helmet, 12);
                break;

            case "13":
                UpdateRoseEquipDate(obj_Equip_Necklace, 13);
                break;

        }
    }


    //点击按钮
    public void Click_Type(string type)
    {
        Obj_Bag.SetActive(false);
        Obj_Property.SetActive(false);
        Obj_EquipGemHoleSet.SetActive(false);
        Obj_EquipQiangHuaSet.SetActive(false);
        Obj_RoseModelImg.SetActive(false);

        Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);
        Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.66f, 0.44f, 0.266f);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_12_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        ObjBtn_Bag.GetComponent<Image>().sprite = img;
        ObjBtn_Property.GetComponent<Image>().sprite = img;
        ObjBtn_Gem.GetComponent<Image>().sprite = img;
        ObjBtn_QiangHua.GetComponent<Image>().sprite = img;

        switch (type)
        {
            //角色
            case "1":
                Obj_Bag.SetActive(true);
                Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
                Obj_RoseModelImg.SetActive(true);
                RoseEquipStatus = "1";
                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_Bag.GetComponent<Image>().sprite = img;
                //更新通用图标显示
                Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet("101");
                Obj_EquipBtnText_1.GetComponent<Text>().color = new Color(0.415f,0.25f,0.1f);
                break;
                
            //属性
            case "2":

                //更新下属性
                updataRoseProprety();

                Obj_Property.SetActive(true);
                Obj_RoseModelImg.SetActive(true);
                RoseEquipStatus = "2";
                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_Property.GetComponent<Image>().sprite = img;
                //更新通用图标显示
                Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet("101");
                Obj_EquipBtnText_2.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;

            //宝石
            case "3":
                Obj_EquipGemHoleSet.SetActive(true);
                Obj_Bag.SetActive(true);
                RoseEquipStatus = "3";
                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_Gem.GetComponent<Image>().sprite = img;
                //更新通用图标显示
                Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet("101");
                Obj_EquipBtnText_3.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                //清空选中道具
                Obj_EquipGemHoleSet.GetComponent<UI_EquipGemHoleSet>().Init();

                break;

            //强化
            case "4":
                Obj_EquipGemHoleSet.SetActive(false);
                Obj_EquipQiangHuaSet.SetActive(true);
                RoseEquipStatus = "4";
                //显示底图
                obj = Resources.Load("GameUI/" + "Btn/Btn_12_1", typeof(Sprite));
                img = obj as Sprite;
                ObjBtn_QiangHua.GetComponent<Image>().sprite = img;
                //更新通用图标显示
                Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet("101");
                Obj_EquipBtnText_4.GetComponent<Text>().color = new Color(0.415f, 0.25f, 0.1f);

                break;
        }
    }


    //移动属性栏位
    public void Mouse_Drag()
    {
        //Debug.Log("开始拖动！！！！！");
        //移动
        if (lastMovePosition_Y == 0)
        {
            lastMovePosition_Y = Input.mousePosition.y;
        }
        float move_Y = Input.mousePosition.y - lastMovePosition_Y;
        //Transform tra_BagSpaceList = this.transform.parent.transform.parent.transform;
        Transform tra_BagSpaceList = Obj_RoseProMoveTra;
        tra_BagSpaceList.GetComponent<RectTransform>().anchoredPosition = new Vector2(tra_BagSpaceList.GetComponent<RectTransform>().anchoredPosition.x, tra_BagSpaceList.GetComponent<RectTransform>().anchoredPosition.y + move_Y / 2);
        lastMovePosition_Y = Input.mousePosition.y;
        //this.transform.parent.transform.parent.transform.parent.GetComponent<ScrollRect>().vertical = false;
        Obj_RoseProMask.GetComponent<ScrollRect>().vertical = false;
    }

    //鼠标松开注销Tips
    public void Mouse_Up()
    {
        //Debug.Log("停止拖动！！！！！");
        //松开时清空移动值
        lastMovePosition_Y = 0;
        //this.transform.parent.transform.parent.transform.parent.GetComponent<ScrollRect>().vertical = true;
        Obj_RoseProMask.GetComponent<ScrollRect>().vertical = true;
    }


    //打开生肖
    public void Btn_OpenShengXiao() {

        //获取等级
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() <= 55)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("56级开启生肖系统!");
        }

        Obj_EquipShengXiaoSet.SetActive(true);
        Obj_EquipShengXiaoSet.GetComponent<UI_Rose_ShengXiao>().UpdateStatus = true;
    }

}
