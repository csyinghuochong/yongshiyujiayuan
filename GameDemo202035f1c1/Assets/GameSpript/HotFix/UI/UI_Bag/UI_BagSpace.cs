using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_BagSpace: MonoBehaviour
{

    public string BagPosition;                  //背包位置
    public ObscuredString SpaceType;                    //格子类型（移动道具位置的时候用的，1：背包 2：仓库 3：英雄栏）
    public GameObject UI_ItemIcon;              //UI绑点—道具图标
    public GameObject UI_ItemNum;               //UI绑点—道具数量
    public GameObject UI_ItemQuility;           //UI绑点—道具品质
    public GameObject UI_ItemEffect;            //UI绑点—道具特效
    public GameObject UI_SclectImg;             //UI选中
    public bool UpdataItemShow;                 //更新道具显示
    private ObscuredString ItemID;                      //道具ID
    private ObscuredString ItemNum;                     //道具数量
    private string ItemIcon;                    //道具Icon
    private string ItemQuality;                 //道具品质
    private GameObject obj_ItemTips;            //实例化的的道具Tips
    private GameObject moveIconObj;             //移动道具显示的图标
    private Sprite itemIcon;                    //道具Icon
    private Game_PositionVar game_positionVar;  //变量
    public GameObject SellItemObj;              //出售道具Set
    public GameObject SellItemTextObj;          //出售价格 
    public bool MoveBagStatus = true;           //移动背包状态是否开启  True可以上下移动背包格子位置,不是移动道具图标
    private float lastMovePosition_Y;           //上一次Y坐标
    private Function_DataSet function_DataSet;
    private string lastItemID;                  //上一次道具ID
    //public GameObject MoveItemObj;            //移动道具出现的误触用的透明墙

	// Use this for initialization
	void Start () {
        game_positionVar = Game_PublicClassVar.Get_game_PositionVar;
        function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        //MoveBagStatus = true;
        //更新道具显示
        updateItem();
	}
	
	// Update is called once per frame
	void Update () {
        //交换物品后,检测是否为自己的格子道具发生变化,发生变化清空值
        if (game_positionVar.UpdataRoseItem) {
            if (game_positionVar.ItemMoveType_End == "1") {
                if (game_positionVar.ItemMoveValue_End == BagPosition) {
                    updateItem();
                }
            }
        }

		//更新道具显示
		if (game_positionVar.UpdataBagAll) {
			updateItem();
		}

        //单独更新显示
        if (UpdataItemShow) {
            UpdataItemShow = false;
            updateItem();
        }

        //更新道具选中框
        if (game_positionVar.RoseBagSpaceSelectStatus) {
            //Debug.Log("1111111111111111bag:" + BagPosition);
            if (game_positionVar.RoseBagSpaceSelectType == "1")
            {
                //Debug.Log("22222222222222222");
                if (BagPosition != game_positionVar.RoseBagSpaceSelect)
                {
                    //Debug.Log("33333333333333333");
                    UI_SclectImg.SetActive(false);
                }
                else
                {
                    UI_SclectImg.SetActive(true);
                }
            }
            else {
                UI_SclectImg.SetActive(false);
            }
        }

        //获取是否为出售状态
        if (Game_PublicClassVar.Get_game_PositionVar.SellItemStatus)
        {
            if (ItemID != "0")
            {
                //获取出售单价
                string price = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", ItemID, "Item_Template");
                int priceSum = int.Parse(price) * int.Parse(ItemNum);
                SellItemTextObj.GetComponent<Text>().text = "售价\n" + priceSum;
                //SellItemObj.SetActive(true);
            }
        }
        else {
            //SellItemObj.SetActive(false);
        }
	}


    //更新道具显示
    void updateItem() {

        //根据背包位置ID读取当前背包对应的道具信息
        //Debug.Log("BagPosition = " + BagPosition);
        ItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", BagPosition, "RoseBag");
        //是否重新加载一次图片
        bool ifShowImg = true;
        if (ItemID == lastItemID) {
            ifShowImg = false;
        }
        lastItemID = ItemID;
        //当前道具ID不为空时 显示对应的道具信息
        if (ItemID != "0")
        {
            //显示道具
            UI_ItemQuility.SetActive(true);
            UI_ItemQuility.SetActive(true);
            UI_ItemIcon.SetActive(true);
            UI_ItemNum.SetActive(true);
            
            ItemNum = function_DataSet.DataSet_ReadData("ItemNum", "ID", BagPosition, "RoseBag");
            //显示道具数量
            UI_ItemNum.GetComponent<Text>().text = ItemNum;

            //获取道具的Icon和品质信息
            if (ifShowImg) {
                ItemIcon = function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
                ItemQuality = function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
                string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");

                //显示道具Icon
                object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
                itemIcon = obj as Sprite;
                UI_ItemIcon.GetComponent<Image>().sprite = itemIcon;

                //如果显示是装备则不显示道具数量
                if (itemType == "3")
                {
                    UI_ItemNum.GetComponent<Text>().text = "";
                }

                //显示道具品质
                string itemQuality = Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality);
                obj = Resources.Load(itemQuality, typeof(Sprite));
                Sprite itemQuility = obj as Sprite;
                UI_ItemQuility.GetComponent<Image>().sprite = itemQuility;
            }

            //如果物品错误,则自动删除此道具
            string ItemName = function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
            if (ItemName == "0" || ItemName == "" || ItemName == null) {
                Game_PublicClassVar.Get_function_Rose.DeleteBagSpaceItem(BagPosition);
            }
        }
        else {
            UI_ItemQuility.SetActive(false);
            UI_ItemQuility.SetActive(false);
            UI_ItemIcon.SetActive(false);
            UI_ItemNum.SetActive(false);
        }
    }

    //鼠标指向道具,显示道具Tips
    public void ItemTips_Show()
    {
        //如果移动装备开关打开放入对应的装备
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus) {
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = SpaceType;
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = BagPosition;
            //触发移动
            //Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
        }
    }

    //注销道具Tips
    public void ItemTips_Destroy()
    {
        /*
        //如果移动装备开关打开放入对应的装备
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
        }
        */
    }

    //移动道具按下调用
    public void StartMoveItem()
    {
        //移动背包栏位状态不触发移动
        if (MoveBagStatus) {
            return;
        }

		//拖拽时注销Tips
		if (obj_ItemTips != null) {
			Destroy(obj_ItemTips);
		}

        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus)
        {
            return;
        }

        //道具图标交换
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus = true;                 //开启道具移动状态
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial = SpaceType;
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial = BagPosition;

        //实例化道具图标
        if (ItemID != "0") {
            moveIconObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UIMoveIcon);
            moveIconObj.GetComponent<UI_MoveItemIcon>().itemIconSprite = itemIcon;      //传入图标精灵
            moveIconObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet.transform);
            moveIconObj.transform.localScale = new Vector3(1, 1, 1);
        }

        //道具放入快捷栏
        Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus = true;
        Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial = ItemID;

    }

    //移动道具松开调用
    public void EndMoveItem() {

        //Debug.Log("我松开了");
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus)
        {
            /*
            Debug.Log("zzzzz = " + Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial);
            if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "1")
            {
                if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "3")
                {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint("请点击存入按钮存入仓库！");
                    Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus = false;
                    //更新拖拽道具显示
                    updateItem();

                    //注销移动的Icon
                    if (moveIconObj != null)
                    {
                        Destroy(moveIconObj);
                    }
                    Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus = false;
                    return;
                }
            }
            */
            //发送涉嫌
            //检测是否触碰到UI上,安卓或IOS可能要换一下
#if UNITY_IPHONE
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)){
#else
            if (!EventSystem.current.IsPointerOverGameObject())
            {
#endif
                Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
                Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
            }

            if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End != "") {
                //执行交换
                Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
                //更新拖拽道具显示
                updateItem();
                Game_PublicClassVar.Get_game_PositionVar.UpdataRoseItem = true;
            }

            Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus = false;
        }

        //道具放入快捷栏执行交换
        if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus) {
            Game_PublicClassVar.Get_function_UI.UI_MoveToMainSkill("0",false);
        }

        //注销移动的Icon
        if (moveIconObj != null) {
            Destroy(moveIconObj);
        }
    }
	//鼠标按下 显示Tips
	public void Mouse_Down(){
        /*
		//调用方法显示UI的Tips
		if (obj_ItemTips == null) {
			obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(ItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
		}
        */


        //更新选中
        if (ItemID != "" && ItemID != "0") {
            game_positionVar.RoseBagSpaceSelectType = "1";
            game_positionVar.RoseBagSpaceSelect = BagPosition;
            game_positionVar.RoseBagSpaceSelectStatus = true;
            UI_SclectImg.SetActive(true);
        }

        //
        string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");   
        //获取目标是否是装备
        if (itemType == "3")
        { 
			if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().RoseEquip_Status) {
				//设置为宝石镶嵌选中的装备
				if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipGemHoleSet.activeSelf)
				{
					//获取当前选中镶嵌的位置
					UI_EquipGemHoleSet ui_EquipGemHoleSet = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipGemHoleSet.GetComponent<UI_EquipGemHoleSet>();
                    ui_EquipGemHoleSet.equipSpaceType = "1";
					ui_EquipGemHoleSet.equipSpace = BagPosition;
					ui_EquipGemHoleSet.UpdateEquipGemStatus = true;
                    
					//展示Tips
					Mouse_Click();
				}
			}
        }

        //宠物洗炼
        if (Game_PublicClassVar.Get_game_PositionVar.PetXiLianStatus)
        {
            UI_PetXiLian ui_PetXiLian = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_XiLianSet.GetComponent<UI_PetXiLian>();
            ui_PetXiLian.XiLianNeedItemID = ItemID;
            ui_PetXiLian.XiLianNeedItemNum = "1";
            ui_PetXiLian.XiLianNeedItemStatus = true;

            //判定放入的道具是技能书还是宠物洗炼的道具
            //string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");
            //宠物添加技能
            if (itemType == "5")
            {
                ui_PetXiLian.XiLianType = "1";
            }
            //宠物洗炼
            if (itemType == "1")
            {
                string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");
                if (itemSubType == "22")
                {
                    ui_PetXiLian.XiLianType = "2";
                }

                if (itemSubType == "23")
                {
                    ui_PetXiLian.XiLianType = "3";
                }
                //提高资质
                if (itemSubType == "27")
                {
                    ui_PetXiLian.XiLianType = "4";
                }
                //提高成长
                if (itemSubType == "28")
                {
                    ui_PetXiLian.XiLianType = "5";
                }
                //宝宝宠物点数洗炼
                if (itemSubType == "36")
                {
                    ui_PetXiLian.XiLianType = "6";
                }
            }

            //更新选中
            /*
            game_positionVar.RoseBagSpaceSelectType = "1";
            game_positionVar.RoseBagSpaceSelect = BagPosition;
            game_positionVar.RoseBagSpaceSelectStatus = true;
            UI_SclectImg.SetActive(true);
            */
            //展示Tips
            Mouse_Click();
            return;
        }

        //装备洗炼
        if(Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus){

            UI_EquipXiLian ui_EquipXiLian = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_EquipXiLian.GetComponent<UI_EquipXiLian>();
            ui_EquipXiLian.bagSpaceNum = BagPosition;
            ui_EquipXiLian.UpdateXiLianItem();

            //展示Tips
            Mouse_Click();
            return;
        }


        //装备洗炼
        if (Game_PublicClassVar.Get_game_PositionVar.NPCGiveStatus)
        {
            UI_GiveNPC ui_GiveNPC = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_GiveNPC.GetComponent<UI_GiveNPC>();
            ui_GiveNPC.bagSpaceNum = BagPosition;
            ui_GiveNPC.UpdateXiLianItem();

            //展示Tips
            Mouse_Click();
            return;
        }

		//回收界面
        if(Game_PublicClassVar.Get_game_PositionVar.HuiShouItemStatus){

            //UI_HuiShouItem ui_HuiShouItem = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_HuiShouItem.GetComponent<UI_HuiShouItem>();
            Game_PublicClassVar.Get_function_UI.HuiShouItem_Add(BagPosition);
            //Game_PublicClassVar.Get_function_UI.DestoryTipsUI();    //删除所有Tips
            //展示Tips
            Mouse_Click();
        }

		//拍卖行
		if(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RosePaiMaiHang_Status){

			Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet> ().UpdateChuShouItemNumStatus = true;
			//展示Tips
			Mouse_Click();
		}



        //装备洗炼
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiNengLiSet_Open!=null&& Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiNengLiSet_Open.active==true)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiNengLiSet_Open.GetComponent<UI_ZuoQiNengLiSet>().BagSpacePosition = BagPosition;

            //展示Tips
            Mouse_Click();
            return;
        }


        //装备附魔
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_PastureFuMoSet_Open != null && Game_PublicClassVar.Get_game_PositionVar.Obj_PastureFuMoSet_Open.active == true)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_PastureFuMoSet_Open.GetComponent<UI_PastureFuMoSet>().PutSpaceID(BagPosition);
            //展示Tips
            Mouse_Click();
            return;
        }

    }

	//鼠标松开注销Tips
	public void Mouse_Up(){
        
		//Debug.Log ("注销Tips注销Tips注销Tips注销Tips注销Tips");

		if (obj_ItemTips != null) {
			Destroy(obj_ItemTips);
			//Debug.Log ("注销Tips注销Tips注销Tips注销Tips注销Tips123123123");
		}

        //松开时清空移动值
        lastMovePosition_Y = 0;
        //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_Bag.GetComponent<UI_Bag>().Tra_BagSpaceList.parent.GetComponent<ScrollRect>().vertical = true;
        this.transform.parent.transform.parent.GetComponent<ScrollRect>().vertical = true;
	    
    }

    //鼠标点击触发
    public void Mouse_Click() {

        //如果道具为空,点击清空Tips
        if (ItemID == "0") {
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
        }

        //出售选定道具
        if (Game_PublicClassVar.Get_game_PositionVar.SellItemStatus)
        {
            //Game_PublicClassVar.Get_function_Rose.SellBagSpaceItemToMoney(BagPosition);
            //SellItemObj.SetActive(false);
        }

        //判定栏内是否有道具
        if (ItemID != "0")
        {
            //获取当前Tips栏内是否有Tips,如果有就清空处理
            GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
            for (int i = 0; i < parentObj.transform.childCount; i++)
            {
                GameObject go = parentObj.transform.GetChild(i).gameObject;
                Destroy(go);
            }

            //调用方法显示UI的Tips
            if (obj_ItemTips == null)
            {
                obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(ItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");
                if (obj_ItemTips == null) {
                    return;
                }
                //获取目标是否是装备
                if (itemType == "3")
                {
                    obj_ItemTips.GetComponent<UI_EquipTips>().UIBagSpaceNum = BagPosition;
                    obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "1";
					//回收界面打开
                    if (Game_PublicClassVar.Get_game_PositionVar.HuiShouItemStatus)
                    {
                        obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "6";
                    }
					//拍卖界面打开
					if(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RosePaiMaiHang_Status){
						obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "0";
					}

                    //获取极品属性
                    string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", BagPosition, "RoseBag");
                    obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;

                    //获取宝石属性
                    string itemGemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", BagPosition, "RoseBag");
                    string itemGemHole = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", BagPosition, "RoseBag");
                    //Debug.Log("宝石属性itemGemID = " + itemGemID + ";BagPosition = " + BagPosition);
                    obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemID = itemGemID;
                    obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemHole = itemGemHole;
                }
                else
                {
                    //其余默认为道具,如果其他道具需做特殊处理
                    obj_ItemTips.GetComponent<UI_ItemTips>().UIBagSpaceNum = BagPosition;
                    obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = "1";
					//回收界面打开
                    if (Game_PublicClassVar.Get_game_PositionVar.HuiShouItemStatus)
                    {
                        obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = "6";
                    }
					//拍卖界面打开
					if(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RosePaiMaiHang_Status){
						obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = "0";
					}
                }
            }
            else
            {
                Destroy(obj_ItemTips);
                //关闭UI背景图片
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);
            }
        }
    }

    //移动背包栏位
    public void Mouse_Drag() {
        if (MoveBagStatus)
        {
            //移动
            if (lastMovePosition_Y == 0)
            {
                lastMovePosition_Y = Input.mousePosition.y;
            }
            float move_Y = Input.mousePosition.y - lastMovePosition_Y;
            //Transform tra_BagSpaceList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_Bag.GetComponent<UI_Bag>().Tra_BagSpaceList;
            Transform tra_BagSpaceList = this.transform.parent.transform;
            tra_BagSpaceList.GetComponent<RectTransform>().anchoredPosition = new Vector2(tra_BagSpaceList.GetComponent<RectTransform>().anchoredPosition.x, tra_BagSpaceList.GetComponent<RectTransform>().anchoredPosition.y + move_Y/2);
            lastMovePosition_Y = Input.mousePosition.y;
            //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_Bag.GetComponent<UI_Bag>().Tra_BagSpaceList.parent.GetComponent<ScrollRect>().vertical = false;
            this.transform.parent.transform.parent.GetComponent<ScrollRect>().vertical = false;
        }
    }
}
