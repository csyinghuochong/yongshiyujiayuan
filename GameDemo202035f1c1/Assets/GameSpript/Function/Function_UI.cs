using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CodeStage.AntiCheat.ObscuredTypes;

public class Function_UI 
{


    public Function_UI()
    {
        AddLisListener();
    }

    public void AddLisListener()
    {
        ObscuredInt.sendError = this.SendObsErr;
        ObscuredFloat.sendError = this.SendObsErr;
        Debug.Log("我sendError.....");
    }


    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //根据道具品质返回对应的品质框
    //ItemQuality  道具品质
    public string ItemQualiytoPath(string ItemQuality) {

        string path ="";

        switch (ItemQuality)
        {
            case "1":
                path = "ItemQuality/ItemQuality_1";
            break;

            case "2":
                path = "ItemQuality/ItemQuality_2";
            break;

            case "3":
                path = "ItemQuality/ItemQuality_3";
            break;

            case "4":
                path = "ItemQuality/ItemQuality_4";
            break;

            case "5":
                path = "ItemQuality/ItemQuality_5";
            break;

            case "6":
                path = "ItemQuality/ItemQuality_6";
            break;

        }

        return path;

    }

    //根据ID返回装备ICON路径
    public string EquipIconToPath(string equipIcon) {

        string path = "";
		path = "ItemIcon/" + equipIcon;
        return path;
    }

    //展示道具Tips
    public GameObject UI_ItemTips(string itemID, GameObject parentObj,bool ifShowEquipTips = true,string equipHindID = "0")
    {

        //如果传入的值为0则直接返回空
		if (itemID == "0") {
			return null;
		}
		//获取道具类型
        string ItemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
        //打开UI背景图片
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(true);
        
		//Debug.Log ("ID:"+itemID+" 类型为"+ItemType);
        GameObject itemTips_1 = null;

        switch (ItemType)
        {
            //消耗品道具
            case "1":
                itemTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIItemTips);
                UI_ItemTips ui_ItemTipsShow_1 = itemTips_1.GetComponent<UI_ItemTips>();
                ui_ItemTipsShow_1.ItemID = itemID;
                
                //设置UI出现的位置
                Vector2 mouseVec2 = Input.mousePosition;
                itemTips_1.transform.SetParent(parentObj.transform);
				itemTips_1.transform.localScale= new Vector3(1,1,1);
				Vector3 v3 = new Vector3(mouseVec2.x,mouseVec2.y,0);
                //v3 = RetrunScreenV2(v3);
                itemTips_1.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen(v3);
				break;
            //材料道具
            case "2":

                itemTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIItemTips);
                ui_ItemTipsShow_1 = itemTips_1.GetComponent<UI_ItemTips>();
                ui_ItemTipsShow_1.ItemID = itemID;
                
                //设置UI出现的位置
                mouseVec2 = Input.mousePosition;
                itemTips_1.transform.SetParent(parentObj.transform);
				itemTips_1.transform.localScale= new Vector3(1,1,1);
				v3 = new Vector3(mouseVec2.x,mouseVec2.y,0);
                //v3 = RetrunScreenV2(v3);
                itemTips_1.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen(v3);
				
                break;

            //装备道具
            case "3":

				mouseVec2 = Input.mousePosition;
                itemTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIEquipTips);
				itemTips_1.transform.SetParent(parentObj.transform);
				itemTips_1.transform.localScale= new Vector3(1,1,1);
				itemTips_1.GetComponent<UI_EquipTips>().ItemID = itemID;
                if (equipHindID != "0" && equipHindID!="")
                {
                    itemTips_1.GetComponent<UI_EquipTips>().ItemHideID = equipHindID;
                }
                
				v3 = new Vector3(mouseVec2.x,mouseVec2.y,0);
                itemTips_1.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen(v3);

                //----------------显示对比装备---------------
                if (ifShowEquipTips)
                {
                    //获取装备位置
                    string ItemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
                    //获取当前身上对应位置的装备
                    string roseEquipID = EquipSubType(ItemSubType);
                    if (roseEquipID != "5" && roseEquipID != "0")
                    {
                        string equipID_DuiBi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", roseEquipID, "RoseEquip");
                        string equipHideID_DuiBi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", roseEquipID, "RoseEquip");

                        //十二生肖
                        if (int.Parse(roseEquipID) >= 101 && int.Parse(roseEquipID) <= 112) {
                            equipID_DuiBi = Game_PublicClassVar.Get_function_Rose.RoseEquip_ReturnShengXiaoID(int.Parse(roseEquipID) - 101);
                            equipHideID_DuiBi = "";     //生肖没有隐藏
                        }

                        //不为空展示Tips
                        if (equipID_DuiBi != "0" && equipID_DuiBi != "")
                        {

                            GameObject itemTips_2 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIEquipTips);
                            itemTips_2.transform.SetParent(parentObj.transform);
                            itemTips_2.transform.localScale = new Vector3(1, 1, 1);
                            itemTips_2.GetComponent<UI_EquipTips>().ItemID = equipID_DuiBi;
                            itemTips_2.GetComponent<UI_EquipTips>().ItemHideID = equipHideID_DuiBi;
                            v3 = new Vector3(mouseVec2.x, mouseVec2.y, 0);
                            itemTips_2.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen_DuiBi(v3);
                            itemTips_2.GetComponent<UI_EquipTips>().Obj_ImgYiChuanDai.SetActive(true);          //设置已装备图标
                            itemTips_2.GetComponent<UI_EquipTips>().EquipTipsType = "5";
                            itemTips_2.transform.SetSiblingIndex(0);

                            //获取宝石属性
                            string itemGemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", roseEquipID, "RoseEquip");
                            string itemGemHole = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", roseEquipID, "RoseEquip");
                            itemTips_2.GetComponent<UI_EquipTips>().ItemGemID = itemGemID;
                            itemTips_2.GetComponent<UI_EquipTips>().ItemGemHole = itemGemHole;

                        }

                        //
                    }
                }

				break;

            //宝石道具
            case "4":

                itemTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIItemTips);
                ui_ItemTipsShow_1 = itemTips_1.GetComponent<UI_ItemTips>();
                ui_ItemTipsShow_1.ItemID = itemID;

                //设置UI出现的位置
                mouseVec2 = Input.mousePosition;
                itemTips_1.transform.SetParent(parentObj.transform);
                itemTips_1.transform.localScale = new Vector3(1, 1, 1);
                v3 = new Vector3(mouseVec2.x, mouseVec2.y, 0);
                itemTips_1.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen(v3);

                break;

            //被动技能
            case "5":
                itemTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIItemTips);
                ui_ItemTipsShow_1 = itemTips_1.GetComponent<UI_ItemTips>();
                ui_ItemTipsShow_1.ItemID = itemID;

                //设置UI出现的位置
                mouseVec2 = Input.mousePosition;
                itemTips_1.transform.SetParent(parentObj.transform);
                itemTips_1.transform.localScale = new Vector3(1, 1, 1);
                v3 = new Vector3(mouseVec2.x, mouseVec2.y, 0);
                //v3 = RetrunScreenV2(v3);
                itemTips_1.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen(v3);
                break;


            //消耗品道具
            case "6":
                itemTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIItemTips);
                ui_ItemTipsShow_1 = itemTips_1.GetComponent<UI_ItemTips>();
                ui_ItemTipsShow_1.ItemID = itemID;

                //设置UI出现的位置
                mouseVec2 = Input.mousePosition;
                itemTips_1.transform.SetParent(parentObj.transform);
                itemTips_1.transform.localScale = new Vector3(1, 1, 1);
                v3 = new Vector3(mouseVec2.x, mouseVec2.y, 0);
                //v3 = RetrunScreenV2(v3);
                itemTips_1.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen(v3);
                break;

            default:

                return itemTips_1;

                break;
        }
        //设置父级Tips
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.GetComponent<UI_CloseTips>().TipsParent = itemTips_1;
        
        return itemTips_1;

    }

	
	//展示技能Tips
    public GameObject UI_SkillTips(string skillID, GameObject parentObj,string skillType) {

        //如果传入的值为0则直接返回空
        if (skillID == "0")
        {
            return null;
        }

        //获取道具类型
        GameObject skillTips_1 = null;
        switch (skillType) { 
            case "1":
                skillTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UISkillTips);
                break;
            case "2":
                skillTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIPetSkillTips);
                break;
        }

        UI_SkillTips ui_ItemTipsShow_1 = skillTips_1.GetComponent<UI_SkillTips>();
        ui_ItemTipsShow_1.SkillID = skillID;
        ui_ItemTipsShow_1.SkillType = skillType;

        //设置UI出现的位置
        Vector2 mouseVec2 = Input.mousePosition;
        skillTips_1.transform.SetParent(parentObj.transform);
        skillTips_1.transform.localScale = new Vector3(1, 1, 1);
        Vector3 v3 = new Vector3(mouseVec2.x, mouseVec2.y, 0);
        skillTips_1.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen(v3);

        return skillTips_1;
    
    }
	
	
    //交换用鼠标移动道具的位置的前后顺序
    public bool UI_ItemMouseMove() {

        //Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("执行道具开始222!");

        //Debug.Log("进来了");
        bool moveStatus = true;
        bool ifUpdataProperty = false;      //是否更新属性
        string sourceValue = "1";



        //检索移动不成功的条件
        if (Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_336");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("洗练装备时禁止移动道具操作！");
            return false;
        }

        //判定是需要交换的两个道具的容器类型

        string ItemMoveID_Initial ="";
        string ItemMoveNum_Initial = "";
        string ItemMovePar_Initial = "";
        string ItemMoveHideID_Initial = "";
        string ItemMoveGemHole_Initial = "";
        string ItemMoveGemID_Initial = "";


        string ItemMoveID_End = "";
        string ItemMoveNum_End = "";
        string ItemMovePar_End = "";
        string ItemMoveHideID_End = "";
        string ItemMoveGemHole_End = "";
        string ItemMoveGemID_End = "";


        //获取相关数据
        switch (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial)
        {

            //背包
            case "1":
                ItemMoveID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                ItemMoveNum_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                ItemMovePar_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                ItemMoveHideID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                ItemMoveGemHole_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                ItemMoveGemID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                break;
            //装备
            case "2":
                ItemMoveID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                ItemMoveNum_Initial = "1";
                ItemMovePar_Initial = "";
                ItemMoveHideID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                ItemMoveGemHole_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                ItemMoveGemID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                break;
            //仓库
            case "3":
                ItemMoveID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                ItemMoveNum_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                ItemMovePar_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                ItemMoveHideID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                ItemMoveGemHole_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                ItemMoveGemID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                break;

        }

        //Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("道具开始Start=:" + ItemMoveID_Initial);

        //更新选中
        if (ItemMoveID_Initial != "" && ItemMoveID_Initial != "0")
        {
            Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelectType = "1";
            Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect = "-1";
            Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelectStatus = true;
        }
        else {
            //开始不能移动空的位子
            return false;
        }

        switch (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End)
        {

            //背包
            case "1":
                ItemMoveID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                ItemMoveNum_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                ItemMovePar_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                ItemMoveHideID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                ItemMoveGemHole_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                ItemMoveGemID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                break;

            //装备
            case "2":
                ItemMoveID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                if (ItemMoveID_End != "" && ItemMoveID_End != "0")
                {
                    ItemMoveNum_End = "1";
                }
                else {
                    ItemMoveNum_End = "0";
                }
                ItemMovePar_End = "";
                ItemMoveHideID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                ItemMoveGemHole_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                ItemMoveGemID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                break;

            //仓库
            case "3":
                ItemMoveID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                ItemMoveNum_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                ItemMovePar_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                ItemMoveHideID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                ItemMoveGemHole_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                ItemMoveGemID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                break;
        }

        //Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("道具开始End=:" + ItemMoveID_End);

        //获得数据,检测不满足交换的条件
        //背包→装备
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "1" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "2")
        {
            bool ifMoveItem = bagToEquip(ItemMoveID_Initial, ItemMoveID_End, ItemMoveHideID_Initial);
            //Debug.Log("ItemMoveID_Initial = " + ItemMoveID_Initial + "     ItemMoveID_End = " + ItemMoveID_End);
            if (ifMoveItem)
            {
                //Debug.Log("可以交换");
                //如果当前打开技能界面则关闭,让他刷新一下技能界面
                if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseSkill_Status) {
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseSkill();
                }
                //更新角色数据
                //Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty();
                //Game_PublicClassVar.Get_function_UI.PlaySource("10005", "1");
                sourceValue = "2";    //切换播放音效
                ifUpdataProperty = true;        //更新属性
            }
            else {
                //Debug.Log("不可以交换");
                moveStatus = false;     //暂时关闭交换数据
            }
        }

        //装备→背包
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "2" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "1")
        {
            bool ifMoveItem = equipToBag(ItemMoveID_Initial, ItemMoveID_End);
            if (ifMoveItem)
            {
                //Debug.Log("可以交换");
                sourceValue = "3";    //切换播放音效
                //如果当前打开技能界面则关闭,让他刷新一下技能界面
                if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseSkill_Status)
                {
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseSkill();
                }
                ifUpdataProperty = true;        //更新属性
            }
            else
            {
                //Debug.Log("不可以交换");
                moveStatus = false;     //暂时关闭交换数据
            }
        }

		//背包→背包
		if(Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "1"&&Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "1"){

            if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End == Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial) {
                //Debug.Log("移动位置相同");
                return false;   
            }

            if (Game_PublicClassVar.Get_game_PositionVar.EquipPropertyMoveStatus) {
                return false;
            }

			//获取交换的道具ID检测是否为同类
			if(ItemMoveID_Initial == ItemMoveID_End){
                //交换道具为0直接结束方法
                if (ItemMoveID_Initial == "0") {
                    return false; 
                }
				//获取当前背包道具的最大堆叠数量
				int itemPileSum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPileSum", "ID",ItemMoveID_End, "Item_Template"));
				if(int.Parse(ItemMoveNum_End)<itemPileSum){
					if(itemPileSum-int.Parse(ItemMoveNum_End)>=int.Parse(ItemMoveNum_Initial)){
						//可以叠加
						int numValue = int.Parse(ItemMoveNum_End)+int.Parse(ItemMoveNum_Initial);
						ItemMoveNum_End = numValue.ToString();
						ItemMoveNum_Initial = "0";

					}else{
						//额外叠加
						int numValue = int.Parse(ItemMoveNum_Initial)-(itemPileSum-int.Parse(ItemMoveNum_End));
						ItemMoveNum_Initial = numValue.ToString();
						ItemMoveNum_End = itemPileSum.ToString();
					}

					//执行交换数据
					if(ItemMoveNum_Initial!="0"){
						Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
						Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
					}else{
						Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
						Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID ", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
					}

					Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
					Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
					Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");

                    //Debug.Log("ItemMoveGemHole_Initial = " + ItemMoveGemHole_Initial + "ItemMoveGemHole_End = " + ItemMoveGemHole_End + "ItemMoveHideID_Initial = " + ItemMoveHideID_Initial + "ItemMoveHideID_End = " + ItemMoveHideID_End);

					moveStatus = false;
				}
			}
		}

        //装备→装备
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "2" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "2") {
            //不同部位不可以交换
            string typeSon1 = EquipBagType(Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial);
            string typeSon2 = EquipBagType(Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End);
            //Debug.Log("typeSon1 = " + typeSon1 + ";typeSon2 = " + typeSon2);
            if (typeSon1 != typeSon2) {
                moveStatus = false;
            }
        }

        //仓库→装备（仓库不能直接穿戴装备）
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "3" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "2")
        {
            moveStatus = false;
        }

        //装备→仓库（装备不能直接存进仓库）
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "2" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "3")
        {
            moveStatus = false;
        }

        //仓库→背包
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "3" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "1")
        {
            //获取交换的道具ID检测是否为同类
            if (ItemMoveID_Initial == ItemMoveID_End)
            {
                if (ItemMoveID_Initial == "0")
                {
                    //return false;
                }
                //获取当前背包道具的最大堆叠数量
                int itemPileSum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPileSum", "ID", ItemMoveID_End, "Item_Template"));
                if (int.Parse(ItemMoveNum_End) < itemPileSum)
                {
                    if (itemPileSum - int.Parse(ItemMoveNum_End) >= int.Parse(ItemMoveNum_Initial))
                    {
                        //可以叠加
                        int numValue = int.Parse(ItemMoveNum_End) + int.Parse(ItemMoveNum_Initial);
                        ItemMoveNum_End = numValue.ToString();
                        ItemMoveNum_Initial = "0";

                    }
                    else
                    {
                        //额外叠加
                        int numValue = int.Parse(ItemMoveNum_Initial) - (itemPileSum - int.Parse(ItemMoveNum_End));
                        ItemMoveNum_Initial = numValue.ToString();
                        ItemMoveNum_End = itemPileSum.ToString();
                    }

                    //执行交换数据
                    if (ItemMoveNum_Initial != "0")
                    {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    }
                    else
                    {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID ", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    moveStatus = false;

                    //更新道具任务显示
                    string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemMoveID_End, "Item_Template");
                    if (itemType != "3")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.UpdatTaskStatus = true;
                    }
                }
            }
        }

        //仓库→仓库
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "3" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "3")
        {
            //Debug.Log("啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊");
            //Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

            if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End == Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial)
            {
                //Debug.Log("移动位置相同");
                return false;
            }

            //获取交换的道具ID检测是否为同类
            if (ItemMoveID_Initial == ItemMoveID_End)
            {
                if (ItemMoveID_Initial == "0") {
                    return false;
                }
                //获取当前背包道具的最大堆叠数量
                int itemPileSum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPileSum", "ID", ItemMoveID_End, "Item_Template"));
                if (int.Parse(ItemMoveNum_End) < itemPileSum)
                {
                    if (itemPileSum - int.Parse(ItemMoveNum_End) >= int.Parse(ItemMoveNum_Initial))
                    {
                        //可以叠加
                        int numValue = int.Parse(ItemMoveNum_End) + int.Parse(ItemMoveNum_Initial);
                        ItemMoveNum_End = numValue.ToString();
                        ItemMoveNum_Initial = "0";

                    }
                    else
                    {
                        //额外叠加
                        int numValue = int.Parse(ItemMoveNum_Initial) - (itemPileSum - int.Parse(ItemMoveNum_End));
                        ItemMoveNum_Initial = numValue.ToString();
                        ItemMoveNum_End = itemPileSum.ToString();
                    }

                    //执行交换数据
                    if (ItemMoveNum_Initial != "0")
                    {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    }
                    else
                    {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    moveStatus = false;
                    
                }
            }
        }

        //背包→仓库
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "1" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "3")
        {
            //获取交换的道具ID检测是否为同类
            if (ItemMoveID_Initial == ItemMoveID_End)
            {
                if (ItemMoveID_Initial == "0") {
                    return false;
                }
                //Debug.Log("ItemMoveID_Initial = " + ItemMoveID_Initial + "/ItemMoveID_End = " + ItemMoveID_End);
                //获取当前背包道具的最大堆叠数量
                int itemPileSum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPileSum", "ID", ItemMoveID_End, "Item_Template"));
                if (int.Parse(ItemMoveNum_End) < itemPileSum)
                {
                    if (itemPileSum - int.Parse(ItemMoveNum_End) >= int.Parse(ItemMoveNum_Initial))
                    {
                        //可以叠加
                        int numValue = int.Parse(ItemMoveNum_End) + int.Parse(ItemMoveNum_Initial);
                        ItemMoveNum_End = numValue.ToString();
                        ItemMoveNum_Initial = "0";
                        //Debug.Log("可以叠加");

                    }
                    else
                    {
                        //额外叠加
                        int numValue = int.Parse(ItemMoveNum_Initial) - (itemPileSum - int.Parse(ItemMoveNum_End));
                        ItemMoveNum_Initial = numValue.ToString();
                        ItemMoveNum_End = itemPileSum.ToString();
                        Debug.Log("不可以叠加");
                    }

                    //执行交换数据
                    if (ItemMoveNum_Initial != "0")
                    {
                        //Debug.Log("11111:" + ItemMoveID_Initial + "--------" + Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial + "----" + ItemMoveNum_Initial);
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    }
                    else
                    {
                        //Debug.Log("22222:" + ItemMoveID_Initial + "--------" + Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial);
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", "", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    moveStatus = false;
                }
            }
        }

        //判定移动道具的类型是否为装备

		//判定当前交换是否为

        //Debug.Log("ItemMoveNum_Initial = " + ItemMoveNum_Initial + ";" + "ItemMoveNum_End = " + ItemMoveNum_End);
        //执行交换
        if (moveStatus)
        {

            //Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("道具执行 = start:" + ItemMoveID_Initial + "end:" + ItemMoveID_End);

            //执行2的交换
            switch (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial)
            {

                case "1":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                    break;

                case "2":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");

                    //如果格子为空则显示原始武器
                    if (ItemMoveGemID_End == "0" || ItemMoveGemID_End == "")
                    {
                        Game_PublicClassVar.Get_function_Rose.RoseWeaponModel("");
                    }
                    break;
                case "3":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    break;
            }

            //执行1的交换
            switch (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End)
            {
                case "1":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                    break;

                case "2":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");
                    //更换武器样子
                    string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemMoveID_Initial, "Item_Template");
                    if (itemType == "3") {
                        string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemMoveID_Initial, "Item_Template");
                        if (itemSubType == "1") {
                            string ItemMondel = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemMondel", "ID", ItemMoveID_Initial, "Item_Template");
                            //临时
                            if (ItemMondel == "yaoshui") {
                                ItemMondel = "";
                            }
                            Game_PublicClassVar.Get_function_Rose.RoseWeaponModel(ItemMondel);

                            //发送武器显示
                            if (Application.loadedLevelName == "EnterGame")
                            {
                                MapThread_PlayerDataChange mapThread_PlayerDataChange = new MapThread_PlayerDataChange();
                                mapThread_PlayerDataChange.ChangType = "1";
                                mapThread_PlayerDataChange.ChangValue = ItemMoveID_Initial;
                                mapThread_PlayerDataChange.MapName = Application.loadedLevelName;
                                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20000204, mapThread_PlayerDataChange);
                            }
                        }
                    }

                    break;

                case "3":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemPar", ItemMovePar_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", ItemMoveGemHole_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", ItemMoveGemID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    break;
            }
        }
        else {
            //交换失败进行提示
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("交换条件不足");
            //return false;
        }

        //更新装备属性
        Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;

        //交换完毕清空数据
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial = "";
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial = "";
        /*
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
        */
        Game_PublicClassVar.Get_game_PositionVar.UpdataRoseItem = true;
        //播放UI音效
        switch (sourceValue) { 
            //交换
            case "1":
                Game_PublicClassVar.Get_function_UI.PlaySource("10007", "1");
                break;
            //穿戴
            case "2":
                Game_PublicClassVar.Get_function_UI.PlaySource("10005", "1");
                break;
            //卸下
            case "3":
                Game_PublicClassVar.Get_function_UI.PlaySource("10006", "1");
                break;
        }
        if (ifUpdataProperty) {
            Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty(true);
            //Debug.Log("更新了属性");
        }

        //Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("交换完成");

        return true;   
    }

    //根据装备栏的位置,返回对应的装备类型
    public string EquipBagType(string equipBagValue) {

        switch (equipBagValue) {
            //武器
            case "1":
                return "1";
            break;

            case "2":
                return "2";
            break;

            case "3":
            return "3";
            break;

            case "4":
            return "4";
            break;

            case "5":
            return "5";
            break;

            case "6":
            return "5";
            break;

            case "7":
            return "5";
            break;

            case "8":
            return "6";
            break;

            case "9":
            return "7";
            break;

            case "10":
            return "8";
            break;

            case "11":
            return "9";
            break;

            case "12":
            return "10";
            break;

            case "13":
            return "11";
            break;
        }
        return "0";
    }


    //根据装备栏的位置,返回对应的装备类型
    public string EquipSubType(string equipSubType)
    {

        switch (equipSubType)
        {
            //武器
            case "1":
                return "1";
                break;
            //衣服
            case "2":
                return "2";
                break;
            //护符
            case "3":
                return "3";
                break;
            //灵石
            case "4":
                return "4";
                break;
            //饰品
            case "5":
                return "5";
                break;
            //鞋子
            case "6":
                return "8";
                break;
            //裤子
            case "7":
                return "9";
                break;
            //腰带
            case "8":
                return "10";
                break;
            //手镯
            case "9":
                return "11";
                break;
            //头盔
            case "10":
                return "12";
                break;
            //项链
            case "11":
                return "13";
                break;

            default:
                return equipSubType;
                break;
        }
        return "0";
    }

    //判定是否可以交换，参数1,背包数据  参数2，装备数据
    private bool bagToEquip(string moveItem1,string moveItem2,string moveItem1_HideID) {

        string type1 = "";
        string type2 = "";
        string typeSon1 = "";
        string typeSon2 = "";
        
        //Debug.Log("1");
        //判定移动时背包内是否拖动的是空格子
        if (moveItem1 != "0")
        {
            type1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem1, "Item_Template");
            typeSon1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem1, "Item_Template");
            //Debug.Log("2");
            //判定移动到装备是否为空格子
            if (moveItem2 != "0")
            {
                type2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem2, "Item_Template");
                typeSon2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem2, "Item_Template");
                //Debug.Log("3");
            }
            else
            {
                type2 = "3";
                typeSon2 = EquipBagType(Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End);
                //Debug.Log("4");
            }
            //道具类型一样执行下一步筛选
            if (type1 == type2)
            {
                //Debug.Log("5");
                //获取装备子类,查看是否是一个子类
                if (typeSon1 == typeSon2)
                {
                    //Debug.Log("6");
                    //判定装备等级是否达到
                    int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                    int equipLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", moveItem1, "Item_Template"));

                    //获取技能是否有简易
                    float hintSkillProValue = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideSkillValue(moveItem1_HideID, "902");
                    equipLv = equipLv - (int)(hintSkillProValue);

                    //获取技能是否无级别
                    hintSkillProValue = Game_PublicClassVar.Get_function_Rose.ReturnEquipHideSkillValue(moveItem1_HideID, "905");
                    if (hintSkillProValue > 0)
                    {
                        equipLv = 0;
                    }

                    if (roseLv >= equipLv) {
                        //判定对应的属性是否达到
                        string equipLimit = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyLimit", "ID", moveItem1, "Item_Template");
                        if(equipLimit!="0"){
                            string needPropertyType = equipLimit.Split(',')[0];
                            string needPropertyValue = equipLimit.Split(',')[1];
                            switch (needPropertyType) { 
                                //目前先只支持攻击
                                case "1":
                                    //获取自身攻击力
                                    int roseAct = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMax;
                                    if (roseAct >= int.Parse(needPropertyValue)) {
                                        return true;
                                    }
                                    break;
                            }
                        }else{
                            return true;
                        }
                        
                    }
                }
                    /*
                else
                {
                    return false;     //交换失败
                }
                     */
            }
                /*
            else
            {
                return false;     //交换失败
            }
                 */
        }
            /*
        else
        {
            return false;     //交换失败
        }
             */
        return false;     //交换失败
    }


    //判定是否可以交换，参数1,装备数据  参数2，背包数据
    private bool equipToBag(string moveItem1, string moveItem2)
    {
        string type1 = "";
        string typeSon1 = "";



        string type2 = "";
        string typeSon2 = "";

        //判定移动时背包内是否拖动的是空格子
        if (moveItem1 != "0")
        {
            type1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem1, "Item_Template");
            typeSon1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem1, "Item_Template");
            //判定移动到装备是否为空格子
            if (moveItem2 != "0")
            {
                type2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem2, "Item_Template");
                typeSon2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem2, "Item_Template");
            }
            else
            {
                //检测移动装备移动背包内,背包的格子为空，则判定移入成功
                type2 = type1;
                typeSon2 = typeSon1;
            }
            //道具类型一样执行下一步筛选
            if (type1 == type2)
            {
                //获取装备子类,查看是否是一个子类
                if (typeSon1 == typeSon2)
                {
                    return true;
                }
                else
                {
                    return false;     //交换失败
                }
            }
            else
            {
                return false;     //交换失败
            }
        }
        else
        {
            return false;     //交换失败
        }

    }

    
    //判定是否可以交换，参数1,装备数据  参数2，背包数据
    private bool storeHouseToBag(string moveItem1, string moveItem2)
    {
        string type1 = "";
        string type2 = "";
        string typeSon1 = "";
        string typeSon2 = "";

        //判定移动时背包内是否拖动的是空格子
        if (moveItem1 != "0")
        {
            type1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem1, "Item_Template");
            typeSon1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem1, "Item_Template");
            //判定移动到装备是否为空格子
            if (moveItem2 != "0")
            {
                type2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem2, "Item_Template");
                typeSon2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem2, "Item_Template");
            }
            else
            {
                //检测移动装备移动背包内,背包的格子为空，则判定移入成功
                type2 = type1;
                typeSon2 = typeSon1;
            }
            //道具类型一样执行下一步筛选
            if (type1 == type2)
            {
                //获取装备子类,查看是否是一个子类
                if (typeSon1 == typeSon2)
                {
                    return true;
                }
                else
                {
                    return false;     //交换失败
                }
            }
            else
            {
                return false;     //交换失败
            }
        }
        else
        {
            return false;     //交换失败
        }

    }


    //判定道具是否移入到技能栏中  moveType(0:表示道具类型  1：表示技能类型)   ifExchange 表示检测到技能栏有相同ID是否交换,如果不交换则后者替换前者后,前者清空
    public bool UI_MoveToMainSkill(string moveType,bool ifExchange = true)
    {
        if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus) {
            bool moveStatus = true;
            //判定交换失败条件
            if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial == "0" && Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial == "")
            {
                moveStatus = false;
            }

            if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End == "") {
                moveStatus = false;
            }

            //Debug.Log("Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial = " + Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial);
            if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial == "0")
            {
                moveStatus = false;
                //清空值
                Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus = false;
                Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial = "";
                Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End = "";
                return false;
            }

            for (int i = 1; i <= 10; i++) {

                GameObject skillObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseSkillSet.transform.Find("UI_MainRoseSkill_" + i).gameObject;
                string endSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

                if (skillObj.GetComponent<MainUI_SkillGrid>().SkillID == Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial || skillObj.GetComponent<MainUI_SkillGrid>().SkillID == endSkillID)
                {
                    //获取技能是否有CD
                    if (skillObj.GetComponent<MainUI_SkillGrid>().skillCDStatus) {
                        moveStatus = false;
                        //Game_PublicClassVar.Get_function_UI.GameHint("技能CD中,不能移动技能");
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_402");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    }
                }
            }


            switch (moveType)
            {
                //判定道具是否为消耗品
                case "0":
                    string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial, "Item_Template");
                    if (itemType != "1")
                    {
                        moveStatus = false;
                    }
                    //判定是否有技能
                    string itemSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial, "Item_Template");
                    if (itemSkillID == "0")
                    {
                        moveStatus = false;
                    }
                    //格子9/10只能放道具
                    if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End!="9"&&Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End!="10")
                    {
                        moveStatus = false;
                    }
                break;

                //技能,不做处理（后期需要判定是否为被动技能）
                case "1":
                    //格子9/10只能放道具
                    if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End == "9" || Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End == "10")
                    {
                        moveStatus = false;
                    }
                    break;
            }

            //写入快捷键的值
            if (moveStatus) {

                //获取当前快捷键内是否有相同的技能或道具ID
                for (int i = 1; i <= 10; i++) {
                    string mainSkillId = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_"+ i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    if (mainSkillId == Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial) {
                        if (ifExchange)
                        {
                            string endSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainSkillUI_" + i, endSkillID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        }
                        else {
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainSkillUI_" + i, "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        }
                    }
                }

                //相同ID删除之前的ID
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainSkillUI_" + Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End, Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
            }
        }

        //清空值
        Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial = "";
        Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End = "";


        //新手引导
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseSkill != null) {
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseSkill.GetComponent<UI_RoseSkill>().Obj_YinDao_SkillItemUse.activeSelf)
            {
                if (Game_PublicClassVar.Get_function_Skill.IfEquipMainSkill("10010001"))
                {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseSkill.GetComponent<UI_RoseSkill>().Obj_YinDao_SkillItemUse.SetActive(false);
                }
            }
        }

        return true;
    }


    //获取当前背包剩余的格子数
    public int BagSpaceNullNum() {
        //暂时设定背包格子有64个,以后可能会改
        int bagNullNum = 0;
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            string bagValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID","ID",i.ToString(),"RoseBag");
            if (bagValue == "0") {
                bagNullNum = bagNullNum + 1;
            }
        }
        return bagNullNum;
    }

	//根据品质返回对应的颜色值
	public string QualityReturnColorText(string ItenQuality){
		string color = "";
		switch (ItenQuality)
		{
		case "1":
			color = "<color=#FFFFFF>" ;
			break;
		case "2":
			color = "<color=#00FF00>";
			break;
		case "3":
			color = "<color=#0CC2D8>";
			break;
		case "4":
            //color = "<color=#871F78>";
            color = "<color=#E794FF>";
            break;
		case "5":
			color = "<color=#FF7F00>";
			break;
        case "6":
            color = "<color=#CD7D30>";
            break;
        }
		return color;

	}

	//根据品质返回一个Color
	public Color QualityReturnColor(string ItenQuality){
		Color color = new Color(1,1,1);
		switch (ItenQuality)
		{
		    case "1":
			    color = new Color(1,1,1);
			    break;

		    case "2":
			    color = new Color(0,1,0);
			    break;

		    case "3":
                color = new Color(0.047f, 0.76f, 0.847f);
			    break;

		    case "4":
                color = new Color(0.937f, 0.5f, 1.0f);
			    break;

		    case "5":
                color = new Color(1, 0.49f, 0);
			    break;

            case "6":
                color = new Color(0.80f, 0.49f, 0.19f);
                break;
		}
		return color;
	}

	//输入文本弹出对应的游戏通用提示
	public void GameHint(string hintText){
		GameObject objHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIGameHint);
		objHint.transform.SetParent (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_GameHintSet.transform);
		objHint.transform.localScale = new Vector3(1,1,1);
		objHint.transform.localPosition = new Vector3(0,0,0);
		objHint.GetComponent<UI_GameHint> ().HintText = hintText;
	}

	//传入屏幕的百分比位置，显示当前对应的位置
	public Vector3 RetrunScreenV2(Vector3 v3){
		//获取当前屏幕的坐标
		int screen_X = Screen.width;
		int screen_Y = Screen.height;
        //Debug.Log("screen_X = " + screen_X + ";" + screen_Y);
		Vector3 UI_V3 = new Vector3();
        
		UI_V3.x = v3.x * 1366;
        UI_V3.y = v3.y * 1366 * screen_Y / screen_X;

		return UI_V3;
	}

    //获取当前分辨率小界面的缩放比
    public float ReturnScreenScalePro() {

        //获取当前屏幕的坐标
        float screen_X = Screen.width;
        float screen_Y = Screen.height;
        //Debug.Log("screen_X = " + screen_X + "; screen_Y = " + screen_Y);

        float scale_Y = screen_Y / 768;
        //Debug.Log("scale_Y = " + scale_Y);

        float scaleValue_X = 1366 * scale_Y;
        float scalePro = scaleValue_X / screen_X;
        //Debug.Log("scalePro = " + scalePro + "scaleValue_X = " + scaleValue_X);
        return scalePro;

    }

    //界面适配
    public void UIFitResolutionRatio(GameObject fitObj) {
        //界面适配
        float scalePro = Game_PublicClassVar.Get_function_UI.ReturnScreenScalePro();
        //仅支持缩小UI,放大UI有可能会让两边缺失
        if (scalePro <= 1) {
            fitObj.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
            fitObj.transform.GetComponent<RectTransform>().anchoredPosition3D = fitObj.transform.GetComponent<RectTransform>().anchoredPosition3D * scalePro;
        }

    }

	//传入UI坐标值,显示当前装备在左边还是右边
	public Vector3 UITipsScreen(Vector3 v2){

		Vector3 vec3 = new Vector3 ();
		float v3_X = v2.x;
        float v3_Y = v2.y;
		if (v2.x >= (Screen.width / 2)) {
            v3_X = v3_X - 250.0f * Screen.width / 1366;
		} else {
            v3_X = v3_X + 250.0f * Screen.width / 1366;
		}

        v3_X = v3_X / Screen.width * 1366;
        int screen_X = Screen.width;
        int screen_Y = Screen.height;
        //v3_Y = v3_Y / Screen.height * 1366 * screen_Y / screen_X;
        v3_Y = v3_Y / Screen.height * 768;
        //v3_Y = v3_Y / Screen.height * 768;
        vec3 = new Vector3(v3_X, v3_Y, 0);
        //vec3 = RetrunScreenV2(vec3);
		return vec3;

	}

    //传入UI坐标值,显示当前装备在左边还是右边
    public Vector3 UITipsScreen_DuiBi(Vector3 v2)
    {

        Vector3 vec3 = new Vector3();
        float v3_X = v2.x;
        float v3_Y = v2.y;
        if (v2.x >= (Screen.width / 2))
        {
            v3_X = v3_X - 550.0f * Screen.width / 1366;
        }
        else
        {
            v3_X = v3_X + 550.0f * Screen.width / 1366;
        }
        v3_X = v3_X / Screen.width * 1366;
        int screen_X = Screen.width;
        int screen_Y = Screen.height;
        v3_Y = v3_Y / Screen.height * 1366 * screen_Y / screen_X;
        //v3_Y = v3_Y / Screen.height * 768;
        vec3 = new Vector3(v3_X, v3_Y, 0);
        //vec3 = RetrunScreenV2(vec3);
        return vec3;

    }

    public Vector3 UIMoveIconPosition(float x, float y) { 
        x = x / Screen.width * 1366;
        int screen_X = Screen.width;
        int screen_Y = Screen.height;
        y = y / Screen.height *1366 * screen_Y / screen_X;
        //y = y / Screen.height * 768;
        Vector2 v3 = new Vector3(x,y,0);
        //Debug.Log("Screen.width = " + Screen.width + "Screen.height = " + Screen.height);
        return v3;
    }

    //传入X值返回实际X值
    public float ReturnScreen_X(float Value_X) {
        Value_X = Value_X / 1366 * Screen.width;
        return Value_X;
    }

    //传入Y值返回实际X值
    public float ReturnScreen_Y(float Value_Y){
        //int screen_X = Screen.width;
        //int screen_Y = Screen.height;
        //Value_Y = Value_Y / 1366 * screen_Y / screen_X * Screen.height;
        Value_Y = Value_Y / 768 * Screen.height;
        return Value_Y;
    }

    //显示故事模式以及背景字母
    public void CreateStoryBack() {

        //获取显示的内容
        string roseStoryStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string isShowBack = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IsStoryBackText", "ID", roseStoryStatus, "GameStory_Template");
        if (isShowBack == "1") {

            //实例化一个背景
            //Debug.Log("开始实例化");
            GameObject obj_storyBack = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_StoreTextBack);
            obj_storyBack.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_StorySpeakSet.transform);
            obj_storyBack.transform.localPosition = new Vector3(0, 0, 0);
            //obj_storyBack.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(683, 682);
            //obj_storyBack.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(683, 0, 0);
            //obj_storyBack.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            //获取当前分辨率
            //Debug.Log("长=" + Screen.width + "宽=" + Screen.height);
            float chang = Screen.width / 1366.0f;
            float kuan = Screen.height / 768.0f;
            if (chang < 1) {
                chang = 1;
            }
            if (kuan < 1) {
                kuan = 1;
            }
            
            //obj_storyBack.transform.localScale = new Vector3(chang, kuan, 1);
            obj_storyBack.transform.localScale = new Vector3(1, 1, 1);
            //obj_storyBack.transform.lossyScale = new(1,1,1);
            UI_StoreTextBack ui_StoreTextBack = obj_storyBack.GetComponent<UI_StoreTextBack>();
            ui_StoreTextBack.ExitType = "2";        //默认显示点击取消
            string showText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryBackText", "ID", roseStoryStatus, "GameStory_Template");
            ui_StoreTextBack.StoreText = showText;

            //更新主角故事
            Game_PublicClassVar.Get_function_Rose.UpdataRoseStoryStatus();
            //Debug.Log("结束实例化");
            //获取是否有任务需要接取
            string taskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryBackTaskID", "ID", roseStoryStatus, "GameStory_Template");
            if (taskID != "0") {
                Game_PublicClassVar.Get_function_Task.GetTask(taskID);
            }
        }
    }

    //创建通用组提示
    public void GameGirdHint(string hintText,string colorVale = "FFFFFFFF") {

        GameObject hintObj = MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIGameGirdHintSing);
        hintObj.SetActive(false);
        hintObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameGirdHint.transform);
        UI_GameGirdHint ui_gameGirdHint = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameGirdHint.GetComponent<UI_GameGirdHint>();
        //ui_gameGirdHint.GameHintObj.Length - 1
        //循环判定提示位置
        for (int i = 0; i <= 99; i++)
        {
            if (ui_gameGirdHint.GameHintObj[i] == null)
            {
                ui_gameGirdHint.GameHintObj[i] = hintObj;
                //设定位置
                
                hintObj.transform.localPosition = new Vector3(0,0,0);
                hintObj.GetComponent<UI_GameGirdHintSingle>().HintText = hintText;
                hintObj.GetComponent<UI_GameGirdHintSingle>().HintColorValue = colorVale;
                //判定提示状态是否打开
                if (!ui_gameGirdHint.HintStatus) {
                    ui_gameGirdHint.HintStatus = true;
                    //Debug.Log("状态打开！");
                }
                break;      //跳出循环
            }
        }
    }


    //创建通用组提示
    public void GameGirdHint_Front(string hintText)
    {
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UIGameGirdHintSing != null) {
            GameObject hintObj = MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIGameGirdHintSing);
            hintObj.SetActive(false);

            GameObject OBJ_UI_Set = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set;
            if (OBJ_UI_Set && OBJ_UI_Set.GetComponent<UI_Set>() && OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameGirdHint_Front && OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameGirdHint_Front.GetComponent<UI_GameGirdHint>())
            {
                hintObj.transform.SetParent(OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameGirdHint_Front.transform);
                UI_GameGirdHint ui_gameGirdHint = OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameGirdHint_Front.GetComponent<UI_GameGirdHint>();
                //ui_gameGirdHint.GameHintObj.Length - 1
                //循环判定提示位置
                for (int i = 0; i <= 99; i++)
                {
                    if (ui_gameGirdHint.GameHintObj[i] == null)
                    {
                        ui_gameGirdHint.GameHintObj[i] = hintObj;
                        //设定位置

                        hintObj.transform.localPosition = new Vector3(0, 0, 0);
                        hintObj.GetComponent<UI_GameGirdHintSingle>().HintText = hintText;
                        //判定提示状态是否打开
                        if (!ui_gameGirdHint.HintStatus)
                        {
                            ui_gameGirdHint.HintStatus = true;
                            //Debug.Log("状态打开！");
                        }
                        break;      //跳出循环
                    }
                }
            }
        }
    }

    //根据资源类型返回资源图标路径
    public string ResourceTypeReturnIconPath(string resourceType) {

        switch (resourceType) {
            //建筑金币
            case "1":
                return "ItemIcon/Resouce_1";
                break;
            //农民
            case "2":
                return "ItemIcon/Resouce_6";
                break;
            //粮食
            case "3":
                return "ItemIcon/Resouce_2";
                break;
            //木材
            case "4":
                return "ItemIcon/Resouce_3";
                break;
            //石头
            case "5":
                return "ItemIcon/Resouce_4";
                break;
            //钢铁
            case "6":
                return "ItemIcon/Resouce_5";
                break;
        }
        return "";
    }

    //特殊飘字
    //参数一：飘字的Obj  参数二：飘字的父级Obj  参数三：飘的文字类型
    public void SpecialFlyText(GameObject flyObj, GameObject flyObjParent, string flyType)
    {

        GameObject HitObject_p = (GameObject)MonoBehaviour.Instantiate(flyObj);
        Text label = HitObject_p.GetComponent<Text>();
        Outline outLine = HitObject_p.GetComponent<Outline>();
        switch (flyType) { 
            //眩晕
            case "1":
                label.text = "眩晕";
                label.fontSize = 30;
                label.color = Color.green;
                outLine.effectColor = Color.black;
                break;
        }


        if (flyObj != null && flyObjParent != null)
        {
            HitObject_p.transform.SetParent(flyObjParent.transform);
            HitObject_p.transform.localPosition = new Vector3(-30, 40, 0);
            HitObject_p.transform.localScale = new Vector3(1, 1, 1);
        }

    }

    //传入值获取属性名称
    public string ReturnEquipNeedPropertyName(string proprety) {

        string propertyName = "";
        switch (proprety) { 
            
            case "1":
                propertyName = "攻击";
                break;

            case "2":
                propertyName = "物防";
                break;

            case "3":
                propertyName = "魔防";
                break;
        }
        return propertyName;
    }

    //传入装备类型返回对应的角色装备格子
    public string ReturnEquipSpaceNum(string equipType) {

        string equipSpaceNum = "0";
        switch (equipType) { 
            //武器
            case "1":
                equipSpaceNum = "1";
                break;
            //衣服
            case "2":
                equipSpaceNum = "2";
                break;
            //护符
            case "3":
                equipSpaceNum = "3";
                break;
            //灵石
            case "4":
                equipSpaceNum = "4";
                break;
            //饰品
            case "5":
                equipSpaceNum = "5";
                break;
            //鞋子
            case "6":
                equipSpaceNum = "8";
                break;
            //裤子
            case "7":
                equipSpaceNum = "9";
                break;
            //腰带
            case "8":
                equipSpaceNum = "10";
                break;
            //手镯
            case "9":
                equipSpaceNum = "11";
                break;
            //头盔
            case "10":
                equipSpaceNum = "12";
                break;
            //项链
            case "11":
                equipSpaceNum = "13";
                break;
        
        }

        return equipSpaceNum;

    }

    //播放音效(音效名称、音效类型、播放时间)
    public GameObject PlaySource(string sourceName,string sourceType,float playTime=0) {
        try
        {
            //获取当前播放音效的组建
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceObj == null)
            {
                return null;
            }
            GameObject sourceObj = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceObj);
            AudioSource audioSource = sourceObj.GetComponent<AudioSource>();
            sourceObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceSet.transform);
            float sourceSize = Game_PublicClassVar.Get_game_PositionVar.GameSourceValue;
            float sourceSize_YinXiao = Game_PublicClassVar.Get_game_PositionVar.GameSourceValue_YinXiao;
            /*
            if (sourceSize == 0) {
                return null;
            }
            */
            switch (sourceType)
            {

                //播放UI音效
                case "1":
                    AudioClip audioClip = (AudioClip)Resources.Load("GameSource/UI/" + sourceName, typeof(AudioClip));
                    audioSource.clip = audioClip;
                    if (playTime > 0)
                    {
                        audioSource.loop = true;    //循环播放 
                    }
                    audioSource.GetComponent<GameSourceObj>().playTime = playTime;
                    audioSource.volume = 1 * sourceSize_YinXiao;
                    audioSource.Play();
                    break;

                //播放游戏音效
                case "2":
                    audioClip = (AudioClip)Resources.Load("GameSource/Game/" + sourceName, typeof(AudioClip));
                    audioSource.clip = audioClip;
                    if (playTime > 0)
                    {
                        audioSource.loop = true;    //循环播放 
                    }
                    audioSource.GetComponent<GameSourceObj>().playTime = playTime;
                    audioSource.volume = 1 * sourceSize_YinXiao;
                    audioSource.Play();
                    break;

                //播放场景背景音效
                case "3":
                    audioClip = (AudioClip)Resources.Load("GameSource/BGM/" + sourceName, typeof(AudioClip));
                    audioSource.clip = audioClip;
                    audioSource.loop = true;    //循环播放
                    audioSource.GetComponent<GameSourceObj>().playTime = 999999;
                    audioSource.volume = 0.5f * sourceSize;
                    audioSource.Play();
                    break;

                //播放技能游戏音效
                case "4":
                    audioClip = (AudioClip)Resources.Load("GameSource/Skill/" + sourceName, typeof(AudioClip));
                    audioSource.clip = audioClip;
                    if (playTime > 0)
                    {
                        audioSource.loop = true;    //循环播放 
                    }
                    audioSource.GetComponent<GameSourceObj>().playTime = playTime;
                    audioSource.volume = 1 * sourceSize_YinXiao;
                    audioSource.Play();
                    break;

            }


            return sourceObj;
        }
        catch (System.Exception ex) {
            Debug.Log("未找到音效文件：" + sourceName + "ex = " + ex);
            return null;
        }
    }

    //钻石不足提示
    public void AddRMBHint() {
        //钻石不足统一提示,以后可能在这里统一添加界面跳转
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_407");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        //GameHint("钻石不足,请点击商城充值！");
    }

    //循环删除目标物体下的所有Obj
    public void DestoryTargetObj(GameObject targetObj) {
        
        for (int i = 0; i < targetObj.transform.childCount; i++)
        {
            GameObject go = targetObj.transform.GetChild(i).gameObject;
            MonoBehaviour.Destroy(go);
        }
    }

    //循环隐藏/显示目标物体下的所有Obj
    public void HintTargetObj(GameObject targetObj,bool ifshow)
    {

        for (int i = 0; i < targetObj.transform.childCount; i++)
        {
            GameObject go = targetObj.transform.GetChild(i).gameObject;
            go.SetActive(ifshow);
        }
    }

    //循环道具Tips
    public void DestoryTipsUI()
    {
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            MonoBehaviour.Destroy(go);
        }

        //打开UI背景图片
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);
    }

    //添加通用货币提示框
    public void AddUI_CommonHuoBiSet(GameObject targetObj,string titleType = "0",string otherItemID = "") {

        //清空通用框
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_CommonHuoBiSetObj != null) {
            MonoBehaviour.Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_CommonHuoBiSetObj);
        }

        //附加
        GameObject commonObj = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_CommonHuoBiSet);
        Game_PublicClassVar.Get_game_PositionVar.Obj_CommonHuoBiSetObj = commonObj;
		//Debug.Log ("targetObj = " + targetObj);

        commonObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_CommonHuoBiSet.transform);
        commonObj.transform.localScale = new Vector3(1, 1, 1);
        commonObj.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        commonObj.GetComponent<UI_CommonHuoBiSet>().TitleImgType = titleType;
        commonObj.GetComponent<UI_CommonHuoBiSet>().Obj_Parent = targetObj;
        commonObj.GetComponent<UI_CommonHuoBiSet>().OutheItemID = otherItemID;
    }

    //更新通用货币信息
    public void UpdateUI_CommonHuoBiSet(string titleType="") {
		//Debug.Log ("打开通用货币");
        GameObject commonObj = Game_PublicClassVar.Get_game_PositionVar.Obj_CommonHuoBiSetObj;
        if (commonObj != null) {
            commonObj.GetComponent<UI_CommonHuoBiSet>().TitleImgType = titleType;
            commonObj.GetComponent<UI_CommonHuoBiSet>().UpdateStatus = true;
        }
    }

    //展示宠物模型
    public void ShowPetModel(string petModelID) {

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.Obj_PetModlePosition);

        string modelShowPosiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ModelShowPosi", "ID", petModelID, "Pet_Template");
        //Debug.Log("modelShowPosiStr = " + modelShowPosiStr + ";petModelID = " + petModelID);
        string[] modelShowPosiList = modelShowPosiStr.Split(',');
        float posi_X = 0;
        float posi_Y = 0;
        float posi_Z = 0;
        float posi_R = 0;
        if (modelShowPosiStr != "" && modelShowPosiStr != "0")
        {
            posi_X = float.Parse(modelShowPosiList[0]);
            posi_Y = float.Parse(modelShowPosiList[1]);
            posi_Z = float.Parse(modelShowPosiList[2]);
            posi_R = float.Parse(modelShowPosiList[3]);
        }
        
        //实例化
        string petModelShowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetModel", "ID", petModelID, "Pet_Template");
        GameObject obj = (GameObject)Resources.Load("PetSet/" + petModelShowID, typeof(GameObject));
        if (obj != null)
        {
            GameObject modelObj = MonoBehaviour.Instantiate(obj);
            modelObj.SetActive(false);

            //查看当前是否为变异
            string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petModelID, "Pet_Template");
            if (monsterType == "1")
            {
                Game_PublicClassVar.Get_function_AI.AI_AddBianYiTieTu(modelObj, petModelID);
            }

            //删除多余的脚本
            modelObj.GetComponent<AIPet>().IfDestoryBuff = false;
            MonoBehaviour.Destroy(modelObj.GetComponent<AIPet>());
            MonoBehaviour.Destroy(modelObj.GetComponent<AI_Property>());
            modelObj.SetActive(true);

            //设定怪物位置
            modelObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_PetModlePosition.transform);
            modelObj.transform.localScale = new Vector3(1, 1, 1);
            modelObj.transform.localRotation = Quaternion.Euler(new Vector3(0, posi_R, 0));
            modelObj.transform.localPosition = new Vector3(posi_X, posi_Y, posi_Z);

        }
        
    }


    //展示宠物模型
    public void ShowMonsterModel(string petModelID)
    {

        //清理
        //Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.Obj_PetModlePosition);

        string modelShowPosiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ModelShowPosi", "ID", petModelID, "Pet_Template");
        //Debug.Log("modelShowPosiStr = " + modelShowPosiStr + ";petModelID = " + petModelID);
        string[] modelShowPosiList = modelShowPosiStr.Split(',');
        float posi_X = 0;
        float posi_Y = 0;
        float posi_Z = 0;
        float posi_R = 0;
        if (modelShowPosiStr != "" && modelShowPosiStr != "0")
        {
            posi_X = float.Parse(modelShowPosiList[0]);
            posi_Y = float.Parse(modelShowPosiList[1]);
            posi_Z = float.Parse(modelShowPosiList[2]);
            posi_R = float.Parse(modelShowPosiList[3]);
        }

        //实例化
        string petModelShowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetModel", "ID", petModelID, "Pet_Template");
        GameObject obj = (GameObject)Resources.Load("PetSet/" + petModelShowID, typeof(GameObject));
        if (obj != null)
        {
            GameObject modelObj = MonoBehaviour.Instantiate(obj);
            modelObj.SetActive(false);

            //查看当前是否为变异
            string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petModelID, "Pet_Template");
            if (monsterType == "1")
            {
                Game_PublicClassVar.Get_function_AI.AI_AddBianYiTieTu(modelObj, petModelID);
            }

            //删除多余的脚本
            MonoBehaviour.Destroy(modelObj.GetComponent<AIPet>());
            MonoBehaviour.Destroy(modelObj.GetComponent<AI_Property>());
            modelObj.SetActive(true);

            //设定怪物位置
            modelObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_PetModlePosition.transform);
            modelObj.transform.localScale = new Vector3(1, 1, 1);
            modelObj.transform.localRotation = Quaternion.Euler(new Vector3(0, posi_R, 0));
            modelObj.transform.localPosition = new Vector3(posi_X, posi_Y, posi_Z);

        }
    }



    //获取某个天赋的ID的当前等级
    public string RetrunTianFuNowLv(string tianFuID) {

        string LearnTianFuIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianFuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] LearnTianFuID = LearnTianFuIDStr.Split(';');
        for(int i = 0;i<=LearnTianFuID.Length -1;i++){
            
            string nowTianFuID = LearnTianFuID[i].Split(',')[0];
            if(tianFuID == nowTianFuID){
                string nowTianFuLv = LearnTianFuID[i].Split(',')[1];
                return nowTianFuLv;
            } 
        }

        return "-1";
    }

    //升级某个天赋
    public void UpTianFuNowLv(string tianFuID) {
        string langStrHint = "";
        string LearnTianFuIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianFuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] LearnTianFuID = LearnTianFuIDStr.Split(';');
        for(int i = 0;i<=LearnTianFuID.Length -1;i++){
            string nowTianFuID = LearnTianFuID[i].Split(',')[0];
            if(tianFuID == nowTianFuID){

            int nowTianFuLv = int.Parse(LearnTianFuID[i].Split(',')[1]);

                //获取当前最大等级
                int maxTianFuLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TianFuLv", "ID", tianFuID, "Talent_Template"));

                if (nowTianFuLv < maxTianFuLv)
                {
                    //判断当前学习等级是否满足
                    int learnRoseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnRoseLv", "ID", tianFuID, "Talent_Template"));
                    if (learnRoseLv <= Game_PublicClassVar.Get_function_Rose.GetRoseLv())
                    {
                        //判定当前已用天赋总数
                        int needAllSpValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedAllSpValue", "ID", tianFuID, "Talent_Template"));
                        string tianFuType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Type", "ID", tianFuID, "Talent_Template");
                        int nowUseSP = GetTianFuUseNum(tianFuType);
                        //计算总的天赋点数,因为是10级才开启的天赋点数
                        if (nowUseSP >= needAllSpValue)
                        {
                            //判断升级消耗是否足够
                            int costSPValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TalentSP", "ID", tianFuID, "Talent_Template"));
                            if (Game_PublicClassVar.Get_function_Rose.CostRoseSpValue(1))
                            {
                                //技能等级+1
                                nowTianFuLv = nowTianFuLv + 1;
                                UpTianFuID(nowTianFuID, nowTianFuLv.ToString());

                                int nextOpenLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextOpenLv", "ID", tianFuID, "Talent_Template"));
                                //判断当前是否达到最大等级开启下一级0级天赋
                                if (nowTianFuLv >= nextOpenLv)
                                {
                                    //下一级ID
                                    string nextIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", tianFuID, "Talent_Template");
                                    if (nextIDStr != "0" && nextIDStr != "") {
                                        string[] nextIDList = nextIDStr.Split(';');
                                        for (int y = 0; y <= nextIDList.Length - 1; y++)
                                        {
                                            UpTianFuID(nextIDList[y], "0");
                                        }
                                    }
                                }

                                //添加天赋技能
                                string learnTianSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                                string addSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddSkillID", "ID", tianFuID, "Talent_Template");
                                if (addSkillID !="" && addSkillID != "0")
                                {
                                    string[] addSkillListID = addSkillID.Split(';');
                                    if (addSkillListID.Length >= 2) {

                                        //获取对应天赋等级的技能
                                        if (addSkillListID.Length <= nowTianFuLv) {
                                            addSkillID = addSkillListID[nowTianFuLv - 1];
                                        }

                                    }

                                    if (learnTianSkillID != "" && learnTianSkillID != "0") {
                                        learnTianSkillID = learnTianSkillID + "," + addSkillID;
                                    }
                                    else {
                                        learnTianSkillID = addSkillID;
                                    }
                                }

                                if (learnTianSkillID != "") {
                                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnTianSkillID", learnTianSkillID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                                }

                                //Debug.Log("升级天赋成功");
                                //升级成功,直接跳转出去
                                return;
                            }
                            else
                            {
                                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("SP值不足");
                                langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_279");
                                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                            }

                        }
                        else
                        {
                            //激活此天赋需要总点数
                            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_280");
                            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_281");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + needAllSpValue + langStrHint_2);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要总激活天赋点数达到:" + needAllSpValue+"点");
                            return;
                        }

                    }
                    else
                    {
                        langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_282");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("学习等级不足");
                        return;
                    }

                }
                else {
                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_283");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("SP当前天赋已达上限");
                    return;
                }
            }
        }

        langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_284");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("关联天赋未激活,请先激活关联天赋");
    }

    //获取当前消耗的天赋点数(默认获取当前全部天赋点数)
    public int GetTianFuUseNum(string nowTianFuType="0") {

        /*
        int nowSP = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        int nowUseSP = roseLv - nowSP - 9;
        if (nowUseSP < 0)
        {
            nowUseSP = 0;
        }
        //Debug.Log("当前使用天赋总点数:" + nowUseSP);

        return nowUseSP;
        */

        int nowUseSP = 0;
        string learnTianFuIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianFuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] LearnTianFuIDList = learnTianFuIDStr.Split(';');
        for (int i = 0; i < LearnTianFuIDList.Length; i++) {
            string[] nowTianFuList = LearnTianFuIDList[i].Split(',');
            if (nowTianFuList.Length>=2) {
                string nowTianFuID = nowTianFuList[0];
                int nowTianFuLv = int.Parse(nowTianFuList[1]);
                string tianFuType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Type", "ID", nowTianFuID, "Talent_Template");
                if (nowTianFuType == tianFuType|| nowTianFuType=="0") {
                    //累计天赋点数
                    nowUseSP = nowUseSP + nowTianFuLv;
                }
            }
        }
        return nowUseSP;
    }

    //添加某个天赋的ID,检测是否有重复ID
    public void UpTianFuID(string tianFuID,string tianFuLv) {

        if (tianFuID == "" || tianFuID == "0") {
            return;
        }

        string LearnTianFuIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianFuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] LearnTianFuID = LearnTianFuIDStr.Split(';');

        string nowLearnTianFuIDStr = "";
        bool UpLvStatus = false;
        //Debug.Log("进来啦");
        for (int i = 0; i <= LearnTianFuID.Length - 1; i++)
        {

            string nowLearnTianFuID = LearnTianFuID[i].Split(',')[0];
            int nowLearnTianFuLv = int.Parse(LearnTianFuID[i].Split(',')[1]);

            //检测是否有相同天赋
            //string nowTianFuID = LearnTianFuID[i].Split(',')[0];
            //Debug.Log("tianFuID = " + tianFuID + ",nowLearnTianFuID = " + nowLearnTianFuID);
            if (tianFuID == nowLearnTianFuID)
            {
                if (tianFuLv != "0") {
                    nowLearnTianFuLv = nowLearnTianFuLv + 1;
                }

                nowLearnTianFuIDStr = nowLearnTianFuIDStr + nowLearnTianFuID + "," + nowLearnTianFuLv + ";";
                UpLvStatus = true;
            }
            else {
                nowLearnTianFuIDStr = nowLearnTianFuIDStr + LearnTianFuID[i] + ";";
            }
        }


        if (UpLvStatus)
        {
            nowLearnTianFuIDStr = nowLearnTianFuIDStr.Substring(0, nowLearnTianFuIDStr.Length - 1);

            //存储值
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnTianFuID", nowLearnTianFuIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        }
        else {

            //赋予值
            if (LearnTianFuIDStr == "")
            {
                LearnTianFuIDStr = tianFuID + "," + tianFuLv;
            }
            else
            {
                LearnTianFuIDStr = LearnTianFuIDStr + ";" + tianFuID + "," + tianFuLv;
            }

            //存储值
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnTianFuID", LearnTianFuIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
    
    }


    //背包镶嵌宝石（背包内镶嵌的背包格子,镶嵌的装备位置类型1代表背包2代表装备,镶嵌装备的位置根据类型定,镶嵌装备的宝石位置）
    public bool AddEquipGem(string gemSpaceID, string equipType, string equipSpaceID, int gemNum) {

        //获取当前背包ID是否为宝石
        string bagGemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", gemSpaceID, "RoseBag");

        if (bagGemID == "0" || bagGemID == "") {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_337");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("当前没有选择需要镶嵌的宝石");
            return false;
        }


        string equipGemHoleStr = "";
        string equipGemIDStr = "";
        string equipItemID = "";
        switch (equipType) {
            
            //背包
            case "1":
                equipGemHoleStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", equipSpaceID, "RoseBag");
                equipGemIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", equipSpaceID, "RoseBag");
                equipItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", equipSpaceID, "RoseBag");
                break;

            //装备
            case "2":
                equipGemHoleStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", equipSpaceID, "RoseEquip");
                equipGemIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", equipSpaceID, "RoseEquip");
                equipItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", equipSpaceID, "RoseEquip");
                
            break;

        }

        if (equipItemID != "") {
            string ItemLvStr_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", bagGemID, "Item_Template");
            //string ItemLvStr_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", equipItemID, "Item_Template");
            string ItemLvStr_2 = Game_PublicClassVar.function_Rose.GetRoseLv().ToString();
            int ItemLvInt_1 = int.Parse(ItemLvStr_1);
            int ItemLvInt_2 = int.Parse(ItemLvStr_2);
            if (ItemLvInt_1 > ItemLvInt_2) {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请确认自身等级是否高于镶嵌宝石的等级!");
                return false;
            }
        }

        //Debug.Log("equipGemHoleStr = " + equipGemHoleStr);

        //
        string[] equipGemHoleStrList = equipGemHoleStr.Split(',');
        string[] equipGemIDStrList = equipGemIDStr.Split(',');

        //获取要镶嵌的宝石孔位
        if (equipGemHoleStrList.Length >= gemNum) { 
        
            //获取当前孔位宝石
            if (gemNum < 1) {
                gemNum = 1;
            }


            //如果当前格子已经有宝石需要弹出提示点击是否覆盖当前的宝石
            if (equipGemIDStrList[gemNum - 1] != "0" && equipGemIDStrList[gemNum - 1] != "")
            {

                //此处调用通用提示框,如果点击是则把当前宝石清空,然后再次出发一次此功能即可。
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_285");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //GameGirdHint_Front("当前插槽已有宝石,请将宝石卸下后方可重新镶嵌");
                //Debug.Log("当前插槽已有宝石,请将宝石写下后方可重新镶嵌");
                return false;

            }

            string nowEquipGemHole = equipGemHoleStrList[gemNum - 1];
            Debug.Log("nowEquipGemHole = " + nowEquipGemHole);
            if (nowEquipGemHole != "0" && nowEquipGemHole != "")
            {

                //判定当前道具是否为宝石
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", bagGemID, "Item_Template");
                if (itemType == "4") {
                    Debug.Log("2222");
                    //判定当前孔位的颜色
                    string[] itemSubTypeStrList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", bagGemID, "Item_Template").Split(',');
                    int nowItemTypeList = 0;
                    //如果一个宝石对应多个孔位颜色在这里判断
                    for (int i = 0; i < itemSubTypeStrList.Length; i++) { 
                        if (nowEquipGemHole == itemSubTypeStrList[i]){
                            nowItemTypeList = i;
                        }
                    }

                    string itemSubType = itemSubTypeStrList[nowItemTypeList];
                    if (itemSubType == nowEquipGemHole)
                    {
                        Debug.Log("3333");
                        //替换当前宝石
                        equipGemIDStrList[gemNum - 1] = bagGemID;
                        string writeEquipGemIDStr = "";
                        Debug.Log("4444");
                        //重新写入宝石
                        for (int i = 0; i < equipGemIDStrList.Length; i++)
                        {
                            writeEquipGemIDStr = writeEquipGemIDStr + equipGemIDStrList[i] + ",";
                        }
                        Debug.Log("writeEquipGemIDStr = " + writeEquipGemIDStr);
                        if (writeEquipGemIDStr != "")
                        {
                            writeEquipGemIDStr = writeEquipGemIDStr.Substring(0, writeEquipGemIDStr.Length - 1);
                        }

                        Debug.Log("writeEquipGemIDStr123 = " + writeEquipGemIDStr);



                        //根据不同类型写入数据
                        switch (equipType)
                        {

                            //背包
                            case "1":
                                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", equipGemHoleStr, "ID", equipSpaceID, "RoseBag");
                                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", writeEquipGemIDStr, "ID", equipSpaceID, "RoseBag");
                                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                                break;

                            //装备
                            case "2":
                                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", equipGemHoleStr, "ID", equipSpaceID, "RoseEquip");
                                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", writeEquipGemIDStr, "ID", equipSpaceID, "RoseEquip");
                                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");
                                break;
                        }

                        //销毁当前背包内的宝石,数量-1
                        Debug.Log("bagGemID = " + bagGemID);
                        Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(bagGemID, 1, gemSpaceID, false);
                        return true;
                    }
                    else {
                        //镶嵌失败,颜色不同
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_286");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint("镶嵌失败,请确定宝石的颜色是否与孔位一致！");
                    }

                }

            }
            else {
                //当前位置没有宝石孔位,不做任何镶嵌操作
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_287");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint("镶嵌位置错误！");
                return false;
            }
        }

        //获取当前装备上的宝石ID是否为空
        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", gemSpaceID, "RoseBag");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", gemSpaceID, "RoseBag");
        return false;
    
    }




    //装备宝石卸下（卸载宝石的颜色字符串支持多个宝石颜色,镶嵌的装备位置类型1代表背包2代表装备,镶嵌装备的位置根据类型定,镶嵌装备的宝石位置）
    public bool UnloadEquipGem(string unloadGemHoleStr, string equipType, string equipSpaceID, int gemNum)
    {

        //判定当前背包位置是否足够
        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() < 1) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_288");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //GameGirdHint_Front("背包已满,请清理背包");
            return false;
        }

        string equipGemHoleStr = "";
        string equipGemIDStr = "";

        switch (equipType)
        {

            //背包
            case "1":
                equipGemHoleStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", equipSpaceID, "RoseBag");
                equipGemIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", equipSpaceID, "RoseBag");
                break;

            //装备
            case "2":
                equipGemHoleStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", equipSpaceID, "RoseEquip");
                equipGemIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", equipSpaceID, "RoseEquip");
                break;

        }

        string[] equipGemHoleStrList = equipGemHoleStr.Split(',');
        string unloadGemID = "";
        string[] equipGemIDStrList = equipGemIDStr.Split(',');
        //获取要镶嵌的宝石孔位
        if (equipGemIDStrList.Length >= gemNum)
        {

            //获取当前孔位宝石
            if (gemNum < 1)
            {
                gemNum = 1;
            }

            string nowEquipGemHole = equipGemHoleStrList[gemNum - 1];
            Debug.Log("nowEquipGemHole = " + nowEquipGemHole);
            bool unloadStatus = false;
            if (nowEquipGemHole != "0" && nowEquipGemHole != "")
            {
                //判定当前孔位的颜色
                string[] itemSubTypeStrList = unloadGemHoleStr.Split(',');
                Debug.Log("itemSubTypeStrList = " + itemSubTypeStrList[0]);
                int nowItemTypeList = 0;
                //如果一个宝石对应多个孔位颜色在这里判断
                for (int i = 0; i < itemSubTypeStrList.Length; i++)
                {
                    if (nowEquipGemHole == itemSubTypeStrList[i])
                    {
                        unloadStatus = true;
                    }
                }
            }

            if (unloadStatus)
            {
                //替换当前宝石
                unloadGemID = equipGemIDStrList[gemNum - 1];
                equipGemIDStrList[gemNum - 1] = "0";
                string writeEquipGemIDStr = "";
                Debug.Log("4444");
                //重新写入宝石
                for (int i = 0; i < equipGemIDStrList.Length; i++)
                {
                    writeEquipGemIDStr = writeEquipGemIDStr + equipGemIDStrList[i] + ",";
                }
                Debug.Log("writeEquipGemIDStr = " + writeEquipGemIDStr);
                if (writeEquipGemIDStr != "")
                {
                    writeEquipGemIDStr = writeEquipGemIDStr.Substring(0, writeEquipGemIDStr.Length - 1);
                }

                Debug.Log("writeEquipGemIDStr123 = " + writeEquipGemIDStr);



                //根据不同类型写入数据
                switch (equipType)
                {
                    //背包
                    case "1":
                        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", equipGemHoleStr, "ID", equipSpaceID, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", writeEquipGemIDStr, "ID", equipSpaceID, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                        break;

                    //装备
                    case "2":
                        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", equipGemHoleStr, "ID", equipSpaceID, "RoseEquip");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", writeEquipGemIDStr, "ID", equipSpaceID, "RoseEquip");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");
                        break;
                }

                //发送背包
                if (unloadGemID != "" && unloadGemID != "0")
                {
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(unloadGemID, 1,"0",0,"0",true,"21");
                }

                //刷新背包和镶嵌
                UI_EquipGemHoleSet ui_EquipGemHoleSet = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipGemHoleSet.GetComponent<UI_EquipGemHoleSet>();
                ui_EquipGemHoleSet.UpdateEquipGemStatus = true;

                //更新背包立即显示（后期优化可改为更新某一个格子的数量显示）
                Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

                return true;
            }
            else {
                Debug.Log("不能使用,此道具不能卸载此槽位的宝石");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_410");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //GameGirdHint_Front("不能使用,此道具不能卸载此槽位的宝石");
            }
        }

        //获取当前装备上的宝石ID是否为空
        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemHole", "0", "ID", gemSpaceID, "RoseBag");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GemID", "0", "ID", gemSpaceID, "RoseBag");
        return false;

    }


	public void HuiShouItem_Add(string UIBagSpaceNum) {

		string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemID", "ID", UIBagSpaceNum, "RoseBag");
		string huishouGetItem =  Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("HuishouGetItem", "ID", itemID, "Item_Template");
		if(huishouGetItem == ""||huishouGetItem =="0"){
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_409");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //GameHint("当前道具不可以回收");
			return;
		}

		//获取当前回收栏位的装备是否已满
		bool iffullStatus = true;
		int huiShouSpaceNum = 0;
		GameObject[] huishouItemList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_HuiShouItem.GetComponent<UI_HuiShouItem>().HuiShouItemList;
		for(int i = 0; i<huishouItemList.Length; i++){
			if (huishouItemList[i].GetComponent<UI_Common_ItemIcon>().ItemID == "" || huishouItemList[i].GetComponent<UI_Common_ItemIcon>().ItemID == "0") {
				iffullStatus = false;
				huiShouSpaceNum = i;
				break;
			}
		}

		string[] huiShouBagNumList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_HuiShouItem.GetComponent<UI_HuiShouItem>().HuiShouBagNumList;

		//获取当前装备是否已经存在回收栏位
		for(int i = 0;i<huiShouBagNumList.Length;i++){
			if (huiShouBagNumList[i] == UIBagSpaceNum) {
				iffullStatus = true;
				break;
			}
		}

		if (!iffullStatus) {
			Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().Obj_HuiShouItem.GetComponent<UI_HuiShouItem>().HuiShouBagNumList [huiShouSpaceNum] = UIBagSpaceNum;
			Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().Obj_HuiShouItem.GetComponent<UI_HuiShouItem>().UpdateStatus = true;
		}

	}

    //回收取消
	public void HuiShouItem_Cancle(string UIBagSpaceNum){
		string[] huiShouBagNumList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().Obj_HuiShouItem.GetComponent<UI_HuiShouItem> ().HuiShouBagNumList;
		for(int i = 0;i<huiShouBagNumList.Length;i++){
            if (huiShouBagNumList[i] == UIBagSpaceNum) {
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_HuiShouItem.GetComponent<UI_HuiShouItem>().HuiShouItemListShowClearn(i);
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_HuiShouItem.GetComponent<UI_HuiShouItem>().HuiShouBagNumList[i] = "";
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_HuiShouItem.GetComponent<UI_HuiShouItem>().updateHuiShouItem(i.ToString());
                break;
            }
		}
	}

    //在一个父节点循环创建子物体（创建的物体,创建的父节点,需要生成的道具列表）
    public void Common_CreateSonObj(GameObject createObj, GameObject parObj,string itemStr) {

        string[] itemList = itemStr.Split(';');
        if (itemList[0] != "" && itemList[0] != "0") {
            for (int i = 0; i < itemList.Length; i++)
            {
                string itemID = itemList[i].Split(',')[0];
                string itemNum = itemList[i].Split(',')[1];

                GameObject huishouObj = (GameObject)MonoBehaviour.Instantiate(createObj);
                huishouObj.GetComponent<UI_Common_ItemIcon>().ItemID = itemID;
                huishouObj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = int.Parse(itemNum);
                huishouObj.GetComponent<UI_Common_ItemIcon>().UpdateStatus = true;
                huishouObj.transform.SetParent(parObj.transform);
            }
        }
    }

    //在一个父节点循环创建子物体（创建的物体,创建的父节点,需要生成的道具列表）
    public void Common_2_CreateSonObj(GameObject createObj, GameObject parObj, string itemStr,string itemShowType="1")
    {
        string[] itemList = itemStr.Split(';');
        if (itemList[0] != "" && itemList[0] != "0")
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                string itemID = itemList[i].Split(',')[0];
                string itemNum = itemList[i].Split(',')[1];

                GameObject huishouObj = (GameObject)MonoBehaviour.Instantiate(createObj);
                huishouObj.GetComponent<UI_Common_ItemIcon_2>().ItemID = itemID;
                huishouObj.GetComponent<UI_Common_ItemIcon_2>().NeedItemNum = int.Parse(itemNum);
                huishouObj.GetComponent<UI_Common_ItemIcon_2>().ItemShowType = itemShowType;
                huishouObj.transform.SetParent(parObj.transform);
                huishouObj.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }


    //开始强化
    public bool EquipQiangHua(string equipSpace)
    {
        string QiangHuaID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaID", "ID", equipSpace, "RoseEquip");
        string nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", QiangHuaID, "EquipQiangHua_Template");
        //判定当前是否已经达到顶级
        if (nextID == "99999")
        {
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("强化已达满级!");
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_289");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            return false;
        }

        //判定是否已经达到等级上限
        string qianghuaID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", QiangHuaID, "EquipQiangHua_Template");
        int needRoseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpLvLimit", "ID", qianghuaID, "EquipQiangHua_Template"));
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < needRoseLv) {

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_290");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_291");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + needRoseLv + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要等级达到" + needRoseLv + "级开启本次强化！");

            return false;
        }


        //判定当前背包是否有对应的材料
        bool itemNeedStatus = true;

        //判断金币
        string costGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostGold", "ID", QiangHuaID, "EquipQiangHua_Template");
        long selfGold = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
        if (selfGold < int.Parse(costGold))
        {
            itemNeedStatus = false;

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_292");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("强化所需金币不足！");
            return false;
        }

        string costItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostItem", "ID", QiangHuaID, "EquipQiangHua_Template");
        string[] costItemList = costItemStr.Split(';');

        for (int i = 0; i < costItemList.Length; i++)
        {
            string itemID = costItemList[i].Split(',')[0];
            int itemNum = int.Parse(costItemList[i].Split(',')[1]);

            if (Game_PublicClassVar.Get_function_Rose.ReturnNeedBagItemNum(itemID, itemNum) == false)
            {
                itemNeedStatus = false;
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_293");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("强化所需道具不足！");
                return false;
            }
        }



        //消耗道具
        for (int i = 0; i < costItemList.Length; i++)
        {
            string itemID = costItemList[i].Split(',')[0];
            int itemNum = int.Parse(costItemList[i].Split(',')[1]);

            Game_PublicClassVar.Get_function_Rose.CostBagItem(itemID, itemNum);
        }

        //扣除金币
        Game_PublicClassVar.Get_function_Rose.CostReward("1", costGold);

        //判定成功概率
        float successPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SuccessPro", "ID", QiangHuaID, "EquipQiangHua_Template"));
        if (Random.value > successPro)
        {
            itemNeedStatus = false;
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_294");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("强化失败！你的运气可能不太好");
        }

        //升级成功
        if (itemNeedStatus)
        {
            //写入升级
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("QiangHuaID", nextID, "ID", equipSpace, "RoseEquip");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");

            return true;
        }
        else {
            return false;
        }
    }


    //适配战斗UI
    public void ShiPei_MainUI() {
        float scalePro = ReturnScreenScalePro();

        //如果是Ipad 则默认为1
        if (scalePro >= 1.2f) {
            scalePro = 1;
        }

        UI_Set ui_Set = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>();
        ui_Set.Obj_MainUI_BtnFightingSet.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        ui_Set.Obj_MainUI_UI_BossHp.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        //ui_Set.Obj_MainUI_UI_RoseExp.transform.localScale = new Vector3(1, scalePro, 1);
        ui_Set.Obj_MainUI_UI_GetherItemSet.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        ui_Set.Obj_MainUI_BtnFightingSet.transform.localScale = new Vector3(scalePro, scalePro, scalePro);

        //ui_Set.Obj_MainUIBtn.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        //ui_Set.Obj_UIMapName.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        //ui_Set.Obj_MainFunctionUI.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        ui_Set.Obj_RightDownSet.transform.localScale = new Vector3(scalePro, scalePro, scalePro);
        //ui_Set.Obj_RoseTask.transform.localScale = new Vector3(scalePro, scalePro, scalePro);     //进入野外地图回引起界面改变
    }

    //战斗通用飘字
    /*
     * flyType:飘字类型
     * flyValue：飘字的内容
     * flyStatus：飘字的状态,0:表示伤害  1（非0）：表示加血
     * flyObj:飘字的物体
     * flyValueFrontStr:前缀
     * flyValueLastStr：后缀
     */

    public void Fight_FlyText(string flyType, string flyValue, string flyStatus, GameObject flyObj, string flyValueFrontStr = "", string flyValueLastStr = "")
    {

        //判定攻击目标是怪物还是宠物
        if (flyObj.GetComponent<AI_1>() != null)
        {
            flyType = "3";
        }

        if (flyObj.GetComponent<AIPet>() != null)
        {
            flyType = "2";
        }

        if (flyValueFrontStr != "")
        {
            flyValueFrontStr = flyValueFrontStr + " ";
        }

        /*
        if (flyValueLastStr != "")
        {
            flyValueLastStr = "<color=#D2D2D2><size=18> (" + flyValueLastStr + ")</size></color>";
            flyValueLastStr = "<color=#D2D2D2><size=18> (" + flyValueLastStr + ")</size></color>";
        }
        */

        GameObject HitObject_p = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().HitObject);
        switch (flyStatus)
        {
            //伤害
            case "0":
                HitObject_p.GetComponent<Text>().text = flyValueFrontStr + "-" + flyValue + flyValueLastStr;

                break;
            //恢复血量
            case "1":
                HitObject_p.GetComponent<Text>().color = Color.green;
                HitObject_p.GetComponent<Text>().text = flyValueFrontStr + "+" + flyValue + flyValueLastStr;
                break;
            //只显示文字
            case "2":
                HitObject_p.GetComponent<Text>().text = flyValue;
                HitObject_p.GetComponent<Text>().color = Color.green;
                break;
        }

        //Debug.Log("HitObject_p.GetComponent<Text>().text = " + HitObject_p.GetComponent<Text>().text);

        switch (flyType) { 
            //玩家飘字
            case "1":
                HitObject_p.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.transform);
                //Debug.Log("123123123HitObject_p.GetComponent<Text>().text = " + HitObject_p.GetComponent<Text>().text);
                break;
            //宠物飘字
            case "2":
                if (flyObj.GetComponent<AIPet>().UI_Hp != null) {
                    HitObject_p.transform.SetParent(flyObj.GetComponent<AIPet>().UI_Hp.transform);
                }
            break;
            //怪物飘字
            case "3":
                if (flyObj.GetComponent<AI_1>().UI_Hp != null) {
                    HitObject_p.transform.SetParent(flyObj.GetComponent<AI_1>().UI_Hp.transform);
                }
            break;
        }

        HitObject_p.transform.localPosition = new Vector3(0, 40, 0);
        HitObject_p.transform.localScale = new Vector3(1, 1, 1);
    
    }

    //创建随机商店
    public string CreateRandomStore(string npcID) {

        //Debug.Log("CreateRandom == " + npcID);
        string StoreItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShopValue", "ID", npcID, "Npc_Template");
        //Debug.Log("456 == " + StoreItemListText);
        if (StoreItemListText == "" || StoreItemListText == "0")
        {
            return "";
        }
        string[] StoreItemList = StoreItemListText.Split(';');
        StoreItemListText = "";
        for (int i = 0; i <= StoreItemList.Length - 1; i++) {
            string[] storeItemIDList = StoreItemList[i].Split(',');
            //获取配置数据
            //Debug.Log("StoreItemList[i] = " + StoreItemList[i] + "storeItemIDList[0] = " + storeItemIDList[0]);
            float chuxianPro = float.Parse(storeItemIDList[0]);
            string storeItemID = storeItemIDList[1];
            float storeItemNum = float.Parse(storeItemIDList[2]);
            string buyNum = storeItemIDList[3];
            string buyType = storeItemIDList[4];

            if (Random.value <= chuxianPro)
            {
                StoreItemListText = StoreItemListText + storeItemID + "," + storeItemNum + "," + buyNum + "," + buyType + ";";
            }
        }

        if (StoreItemListText != "") {
            StoreItemListText = StoreItemListText.Substring(0, StoreItemListText.Length - 1);
        }

        //Game_PublicClassVar.Get_function_DataSet.AddRandomStoreXml(npcID, StoreItemListText);
        //Debug.Log("StoreItemListText123 == " + StoreItemListText);
        //Game_PublicClassVar.Get_function_DataSet.UpdateRandomStoreXml(npcID, StoreItemListText);
        //Debug.Log("StoreItemListText = " + StoreItemListText);
        return StoreItemListText;
    }

    //修改随机商店值商品剩余次数
    public void RandomStoreItemNum(string npcID,int spaceNum,int costValue)
    { 
        //修改随机商店的值
        string stroeValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value_1", "ID", "Store_" + npcID, "RoseOtherData");
        string[] stroeValueList = stroeValue.Split(';');
        string storeValueStr = "";
        for (int i = 0; i < stroeValueList.Length; i++) {
            string addValue = "";
            if (i == spaceNum)
            {
                //次数扣除
                string[] stroeItemValueList = stroeValueList[i].Split(',');
                int itemBuyNum = int.Parse(stroeItemValueList[2]);
                itemBuyNum = itemBuyNum - costValue;
                if (itemBuyNum <= 0)
                {
                    //直接销毁
                    addValue = "";
                }
                else
                {
                    addValue = stroeItemValueList[0] + "," + stroeItemValueList[1] + "," + itemBuyNum + "," + stroeItemValueList[2];
                }
            }
            else {
                addValue = stroeValueList[i];
            }

            if (addValue != "") {
                storeValueStr = storeValueStr + addValue + ";";
            }
        }

        if (storeValueStr != "") {
            storeValueStr = storeValueStr.Substring(0, storeValueStr.Length - 1);
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Value_1",storeValueStr, "ID", "Store_" + npcID, "RoseOtherData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseOtherData");
    }

    //显示属性名称
    public string GetProName(string proType) { 
        
        string proName = "";

        switch (proType)
        {
            case "10":
                proName = "生命值";
                break;
            case "11":
                proName = "物理攻击";
                break;
            case "14":
                proName = "魔法攻击";
                break;
            case "17":
                proName = "物理防御";
                break;
            case "20":
                proName = "魔法防御";
                break;
            case "30":
                proName = "暴击";
                break;
            case "31":
                proName = "命中";
                break;
            case "32":
                proName = "闪避";
                break;
            case "33":
                proName = "物理免伤";
                break;
            case "34":
                proName = "魔法免伤";
                break;
            case "35":
                proName = "移动速度";
                break;
            case "36":
                proName = "伤害减免";
                break;
            case "50":
                proName = "血量百分比";
                break;
            case "51":
                proName = "物攻百分比";
                break;
            case "52":
                proName = "魔攻百分比";
                break;
            case "53":
                proName = "物防百分比";
                break;
            case "54":
                proName = "魔防百分比";
                break;
            case "101":
                proName = "格挡值";
                break;
            case "111":
                proName = "重击概率";
                break;
            case "112":
                proName = "重击附加伤害值";
                break;
            case "121":
                proName = "每次普通攻击附加的伤害值";
                break;
            case "131":
                proName = "忽视目标防御值";
                break;
            case "132":
                proName = "忽视目标魔防值";
                break;
            case "133":
                proName = "忽视目标百分比防御值";
                break;
            case "134":
                proName = "忽视目标百分比魔防值";
                break;
            case "141":
                proName = "吸血概率";
                break;
            case "151":
                proName = "法术反击";
                break;
            case "152":
                proName = "攻击反击";
                break;
            case "161":
                proName = "韧性";
                break;
            case "201":
                proName = "初始暴击等级";
                break;
            case "202":
                proName = "初始韧性等级";
                break;
            case "203":
                proName = "初始命中等级";
                break;
            case "204":
                proName = "初始闪避等级";
                break;
            case "301":
                proName = "光抗性";
                break;
            case "302":
                proName = "暗抗性";
                break;
            case "303":
                proName = "火抗性";
                break;
            case "304":
                proName = "水抗性";
                break;
            case "305":
                proName = "电抗性";
                break;
            case "321":
                proName = "野兽攻击抗性";
                break;
            case "322":
                proName = "人形攻击抗性";
                break;
            case "323":
                proName = "恶魔攻击抗性";
                break;
            case "401":
                proName = "经验加成";
                break;
            case "402":
                proName = "金币加成";
                break;
            case "403":
                proName = "洗炼极品掉落";
                break;
            case "404":
                proName = "隐藏属性出现概率";
                break;
            case "405":
                proName = "装备上的宝石槽位出现概率";
                break;
            
        }
        return proName;

    }


    //特殊字符检测(True表示存在)
    public bool IfTeShuStr(string createName)
    {
        bool bl_exist = false;
        foreach (char c in createName)
        {
            if ((!char.IsLetter(c)) && (!char.IsNumber(c))) //既不是字母又不是数字的就认为是特殊字符
            { bl_exist = true; }
        }

        if (bl_exist)
        {
            Debug.Log("存在特殊字符");
        }
        return bl_exist;
    }


    //更新活动界面的货币
    public void HuoDongHuoBiUpdate()
    {
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing != null) {
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().updateHuoBi();
        }
    }

    public void ShowMainUINanDuImg() {

        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img1.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img2.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img3.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img1_EN.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img2_EN.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img3_EN.SetActive(false);

        bool ifEN = false;
        if (PlayerPrefs.GetString("GameLanguageType") == "1") {
            ifEN = true;
        }

        //设置难度
        if (ifEN == false)
        {
            switch (Game_PublicClassVar.Get_game_PositionVar.GameNanduValue)
            {

                case "0":
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:普通";
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img1.SetActive(true);
                    break;

                case "1":
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:普通";
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img1.SetActive(true);
                    break;

                case "2":
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:挑战";
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img2.SetActive(true);
                    break;

                case "3":
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:地狱";
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img3.SetActive(true);
                    break;
            }
        }
        else {
            switch (Game_PublicClassVar.Get_game_PositionVar.GameNanduValue)
            {

                case "0":
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:普通";
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img1_EN.SetActive(true);
                    break;

                case "1":
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:普通";
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img1_EN.SetActive(true);
                    break;

                case "2":
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:挑战";
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img2_EN.SetActive(true);
                    break;

                case "3":
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:地狱";
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu_Img3_EN.SetActive(true);
                    break;
            }
        }


        //显示服务器名称
        if (Game_PublicClassVar.Get_wwwSet.ServerName != "" && Game_PublicClassVar.Get_wwwSet.ServerName != "0" && Game_PublicClassVar.Get_wwwSet.ServerName != null)
        {
            if (Application.loadedLevelName != "StartGame") {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIServerName.GetComponent<Text>().text = Game_PublicClassVar.Get_wwwSet.ServerName;

                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().UpdateServerNameStatus = true;
            }

        }
        else {
            //未连接服务器
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIServerName.GetComponent<Text>().text = "";
        }
    }


    //更新抽卡时间
    public void UpdateChouKaTime(float secUpdataTimeSum) {

        //写入抽卡时间
        float chouKaTime_One = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_One", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
        float chouKaTime_Ten = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_Ten", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
        chouKaTime_One = chouKaTime_One + secUpdataTimeSum;
        chouKaTime_Ten = chouKaTime_Ten + secUpdataTimeSum;

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", chouKaTime_One.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", chouKaTime_Ten.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //Debug.Log("更新数据");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

    }


    public void ShowCommonTips_1(string showName,string showDes)
    {

        GameObject hintObj = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHintTips_1);

        //设置UI出现的位置
        Vector2 mouseVec2 = Input.mousePosition;
        hintObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet.transform);
        hintObj.transform.localScale = new Vector3(1, 1, 1);
        Vector3 v3 = new Vector3(mouseVec2.x, mouseVec2.y, 0);
        //v3 = RetrunScreenV2(v3);
        hintObj.GetComponent<RectTransform>().anchoredPosition3D = Game_PublicClassVar.function_UI.UITipsScreen(v3);

        hintObj.GetComponent<UI_CommonHintlTips_1>().Obj_Name.GetComponent<Text>().text = showName;
        hintObj.GetComponent<UI_CommonHintlTips_1>().Obj_Des.GetComponent<Text>().text = showDes;

    }


    //返回角色
    public void Btn_ReturnRose()
    {
        Game_PublicClassVar.ReturnDengLuStatus = true;
        for (int i = 0; i < Game_PublicClassVar.Get_game_PositionVar.Obj_Keep.Length; i++)
        {
            MonoBehaviour.Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_Keep[i]);
        }
        MonoBehaviour.Destroy(Game_PublicClassVar.Get_wwwSet.gameObject.GetComponent<WWWSet>());
        MonoBehaviour.Destroy(Game_PublicClassVar.Get_wwwSet.gameObject);
        SceneManager.LoadScene("StartGame");        //加载场景
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
    public string ShowHintPro(string AddPropreListStr,int EquipType)
    {
        string proprety = AddPropreListStr.Split(',')[0];
        string propretyValue = AddPropreListStr.Split(',')[1];
        string textShow = "";

        if (proprety == "FuMo")
        {

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

                //韧性概率
                case "171":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("回血百分比额外提高");
                    textShow = langStr + " ：" + float.Parse(propretyValue) * 100 + "%";
                    break;

                //韧性概率
                case "172":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("回血固定值额外提高");
                    textShow = langStr + " ：" + float.Parse(propretyValue);
                    break;

                //韧性概率
                case "173":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("战斗回血额外提高");
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

                //野兽攻击抗性
                case "331":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击野兽伤害额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //人物攻击抗性
                case "332":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击人形伤害额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //恶魔攻击抗性
                case "333":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击恶魔伤害额外提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //宠物攻击加成
                case "345":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("宠物攻击加成提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //宠物受伤减免
                case "346":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("宠物受伤减免提高");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //技能冷却时间缩减
                case "347":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("技能冷却时间缩减");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //首领攻击加成
                case "341":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击首领加成");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //首领技能加成
                case "342":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击首领技能加成");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //受到首领攻击减免
                case "343":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("受到首领攻击减免");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //受到首领技能减免
                case "344":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("受到首领技能减免");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                //经验收益额外提高
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

                case "151":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("怪物攻击伤害减免");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                case "152":
                    langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("怪物技能伤害减免");
                    textShow = langStr + float.Parse(propretyValue) * 100 + "%";
                    break;

                    //
            }
        }
        return textShow;
    }


    public void GoToRmbStore()
    {
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing == null)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().ClearnOpenUI();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_HuoDongDaTing();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().IfInitStatus = false;
        }
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().Btn_GamePay();
    }


    //根据职业显示头像
    public void ShowPlayerHeadIcon(string roseOcc, GameObject Obj_HeadIcon) {

        string roseHeadIconId = "10001";

        switch (roseOcc)
        {
            case "1":
                roseHeadIconId = "10001";
                break;

            case "2":
                roseHeadIconId = "10002";
                break;

            case "3":
                roseHeadIconId = "10003";
                break;
        }

        Object obj = Resources.Load("HeadIcon/PlayerIcon/" + roseHeadIconId, typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_HeadIcon.GetComponent<Image>().sprite = img;
        Obj_HeadIcon.GetComponent<Image>().SetNativeSize();

    }


    public string ReturnZhiWeiName(string type)
    {

        string returnName = "门众";

        switch (type)
        {

            case "1":
                returnName = "领袖门主";
                break;

            case "2":
                returnName = "副门主";
                break;

            case "11":
                returnName = "永生长老";
                break;

            case "12":
                returnName = "战神长老";
                break;

            case "13":
                returnName = "灵宠长老";
                break;

            case "21":
                returnName = "征东将军";
                break;

            case "22":
                returnName = "征南将军";
                break;

            case "23":
                returnName = "征西将军";
                break;

            case "24":
                returnName = "征北将军";
                break;

            case "31":
                returnName = "战斗护法";
                break;

            case "32":
                returnName = "战斗护法";
                break;

            case "33":
                returnName = "战斗护法";
                break;

            case "34":
                returnName = "战斗护法";
                break;

            case "35":
                returnName = "战斗护法";
                break;

            case "36":
                returnName = "战斗护法";
                break;

        }

        return returnName;

    }


    public int ReturnZhiWeiRewardNum(string type)
    {

        int returnInt = 0;

        switch (type)
        {

            case "1":
                returnInt = 20;
                break;

            case "2":
                returnInt = 15;
                break;

            case "11":
                returnInt = 12;
                break;

            case "12":
                returnInt = 12;
                break;

            case "13":
                returnInt = 12;
                break;

            case "21":
                returnInt = 8;
                break;

            case "22":
                returnInt = 8;
                break;

            case "23":
                returnInt = 8;
                break;

            case "24":
                returnInt = 8;
                break;

            case "31":
                returnInt = 5;
                break;

            case "32":
                returnInt = 5;
                break;

            case "33":
                returnInt = 5;
                break;

            case "34":
                returnInt = 5;
                break;

            case "35":
                returnInt = 5;
                break;

            case "36":
                returnInt = 5;
                break;

        }

        return returnInt;

    }

    public void SendObsErr(string errStr) {

        if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
        {
            //if (tttt == false)
            //{
                //tttt = true;
                //跨线程调用(跨线程调用的值不能传出)
                //MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                //{

                    if (Game_PublicClassVar.gameLinkServer.ServerLinkStatus)
                    {

                        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        Pro_ComStr_4 com_4 = new Pro_ComStr_4();
                        com_4.str_1 = zhanghaoID;
                        com_4.str_3 = errStr;
                        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100001101, com_4);
                        
                    }

                //}));
            //}
        }
    }

}

