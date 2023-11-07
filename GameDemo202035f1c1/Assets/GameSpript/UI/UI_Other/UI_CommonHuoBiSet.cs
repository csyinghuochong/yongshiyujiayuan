 using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_CommonHuoBiSet : MonoBehaviour {

    public GameObject Obj_Parent;
    public GameObject Obj_ZuanShiValue;
    public GameObject Obj_JinBiValue;
    public GameObject Obj_TitleImg;
    public GameObject Obj_TitleHintText;
    public string TitleImgType;
    public bool UpdateStatus;

    //额外显示一种货币、
    public string OutheItemID;
    public GameObject Obj_Other_Icon;
    public GameObject Obj_Other_Value;

    //动效
    public float EffectNum_Y;

    // Use this for initialization
    void Start () {
        if (OutheItemID != "") {
            Obj_Other_Icon.SetActive(false);
            Obj_Other_Value.SetActive(false);
        }

        UpdateStatus = true;
        EffectNum_Y = 100;

    }
	
	// Update is called once per frame
	void Update () {

        if (UpdateStatus) {
            UpdateStatus = false;
            UpdateHuoBi();
        }

        //源UI为空时,销毁自己
        if (Obj_Parent == null)
        {
            Destroy(this.gameObject);
        }

        //动效
        if (EffectNum_Y > 0)
        {
            EffectNum_Y = EffectNum_Y - Time.deltaTime * 250;
            if (EffectNum_Y <= 0)
            {
                EffectNum_Y = 0;
            }
            this.gameObject.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, EffectNum_Y, 0);
        }

    }


    public void UpdateHuoBi() {
		//Debug.Log ("TitleImgType = " + TitleImgType);
        //获取当前货币数量
        string goldValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string zuanShiValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //获取当前
        Obj_ZuanShiValue.GetComponent<Text>().text = zuanShiValue;
        Obj_JinBiValue.GetComponent<Text>().text = goldValue;

        string path = "GameUI/Img_ComTitle/";

        if (Game_PublicClassVar.Get_wwwSet.GameSetLanguage._Language == "English") {
            path = "GameUI_EN/Img_ComTitle/";
        }

        //根据类型显示当前图片
        if (Obj_TitleImg != null) {
            Obj_TitleImg.SetActive(true);
            switch (TitleImgType)
            {

                case "":
                    break;

                //显示为空
                case "0":
                    //Debug.Log("NULLLLLLLLLLLLLLLLL");
                    Obj_TitleImg.SetActive(false);
                    break;

                //显示角色
                case "101":
                    object obj = Resources.Load(path + "Img_101", typeof(Sprite));
                    Sprite img = obj as Sprite;
                    Obj_TitleImg.GetComponent<Image>().sprite = img;
                    break;

                //显示宠物列表
                case "201":
                    obj = Resources.Load(path + "Img_201", typeof(Sprite));
                    img = obj as Sprite;
                    Obj_TitleImg.GetComponent<Image>().sprite = img;
                    break;

                //显示宠物合成
                case "202":

                    obj = Resources.Load(path + "Img_202", typeof(Sprite));
                    img = obj as Sprite;
                    Obj_TitleImg.GetComponent<Image>().sprite = img;
                    break;

                //显示任务
                case "301":
                    obj = Resources.Load(path + "Img_301", typeof(Sprite));
                    img = obj as Sprite;
                    Obj_TitleImg.GetComponent<Image>().sprite = img;

                    break;

                //显示技能             
                case "401":

                    obj = Resources.Load(path + "Img_401", typeof(Sprite));
                    img = obj as Sprite;
                    Obj_TitleImg.GetComponent<Image>().sprite = img;

                    break;

                //显示天赋             
                case "402":
                    obj = Resources.Load(path + "Img_402", typeof(Sprite));
                    img = obj as Sprite;
                    Obj_TitleImg.GetComponent<Image>().sprite = img;

                    break;

				//显示制作              
				case "501":
                    obj = Resources.Load(path + "Img_501", typeof(Sprite));
                    img = obj as Sprite;
                    Obj_TitleImg.GetComponent<Image>().sprite = img;

					break;

                //显示商店2
                case "601":

                    obj = Resources.Load(path + "Img_601", typeof(Sprite));
                    img = obj as Sprite;
                    Obj_TitleImg.GetComponent<Image>().sprite = img;
                    break;

                //显示钻石商店
                case "602":
     
                    obj = Resources.Load(path + "Img_601", typeof(Sprite));
                    img = obj as Sprite;
                    Obj_TitleImg.GetComponent<Image>().sprite = img;
                    Obj_TitleHintText.GetComponent<Text>().text = "提示:每天8点、12点、20点、24点刷新道具";
                    break;

				//显示成就
				case "701":
					obj = Resources.Load(path + "Img_701", typeof(Sprite));
					img = obj as Sprite;
					Obj_TitleImg.GetComponent<Image>().sprite = img;
					break;

				//显示商店2
				case "801":
					obj = Resources.Load(path + "Img_801", typeof(Sprite));
					img = obj as Sprite;
					Obj_TitleImg.GetComponent<Image>().sprite = img;
					break;

                default:
                    obj = Resources.Load(path + "Img_"+ TitleImgType, typeof(Sprite));
                    img = obj as Sprite;
                    Obj_TitleImg.GetComponent<Image>().sprite = img;
                    break;
            }

            Obj_TitleImg.GetComponent<Image>().SetNativeSize();
        }

        //显示额外货币
        if (OutheItemID != "" && OutheItemID != "0" && OutheItemID != null) {
            Obj_Other_Icon.SetActive(true);
            Obj_Other_Value.SetActive(true);
            //获取道具图标
            string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", OutheItemID, "Item_Template");
            object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
            Sprite itemIcon = obj as Sprite;
            Obj_Other_Icon.GetComponent<Image>().sprite = itemIcon;

            int nowItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(OutheItemID);
            Obj_Other_Value.GetComponent<Text>().text = nowItemNum.ToString();
        }
    }

    public void Click_AddRmb() {
        Debug.Log("我点击了增加钻石,请弹出商城界面");
    }

    public void Btn_GoToRmbStore() {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_11");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, GoToRmbStore, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否前往充值界面？", GoToRmbStore, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    public void GoToRmbStore() {
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing == null) {
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().ClearnOpenUI();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_HuoDongDaTing();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().IfInitStatus = false;
        }
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().Btn_GamePay();
    }


    public void Btn_GoToDuiHuanGold()
    {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_12");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, GoToGoToDuiHuanGold, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否前往兑换金币界面？", GoToGoToDuiHuanGold, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    public void GoToGoToDuiHuanGold()
    {
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang == null) {
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().ClearnOpenUI();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenGamePaiMaiHang();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>().IfInitStatus = false;
        }
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>().Btn_DuiHuanSetShow();
    }

}
