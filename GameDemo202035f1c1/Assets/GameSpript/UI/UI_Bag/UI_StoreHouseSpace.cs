using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_StoreHouseSpace: MonoBehaviour
{

    public string BagPosition;                  //背包位置
    public string SpaceType;                    //格子类型（移动道具位置的时候用的，1：背包 2：装备栏 3：仓库）
    public GameObject UI_ItemIcon;              //UI绑点—道具图标
    public GameObject UI_ItemNum;               //UI绑点—道具数量
    public GameObject UI_ItemQuility;           //UI绑点—道具品质
    public GameObject UI_ItemEffect;            //UI绑点—道具特效
    public GameObject UI_SclectImg;             //UI选中
    public bool UpdataItemShow;                 //更新道具显示
    private string ItemID;                      //道具ID
    private string ItemNum;                     //道具数量
    private string ItemIcon;                    //道具Icon
    private string ItemQuality;                 //道具品质
    private GameObject obj_ItemTips;            //实例化的的道具Tips
    private GameObject moveIconObj;             //移动道具显示的图标
    private Sprite itemIcon;                    //道具Icon
    private Game_PositionVar game_positionVar;  //变量
    public bool MoveBagStatus;                  //移动背包状态是否开启
    private float lastMovePosition_Y;           //上一次Y坐标
    private string lastItemID;                  //上一次道具ID

    // Use this for initialization
    void Start () {
        game_positionVar = Game_PublicClassVar.Get_game_PositionVar;
        MoveBagStatus = true;
        //更新道具显示
        updateItem();
	}
	
	// Update is called once per frame
	void Update () {

        //交换物品后,检测是否为自己的格子道具发生变化,发生变化清空值
        if (game_positionVar.UpdataRoseItem) {
            if (game_positionVar.ItemMoveType_End == "3") {
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
        if (game_positionVar.RoseBagSpaceSelectStatus)
        {
            if (game_positionVar.RoseBagSpaceSelectType == "2")
            {
                if (BagPosition != game_positionVar.RoseBagSpaceSelect)
                {
                    UI_SclectImg.SetActive(false);
                }
                else {
                    UI_SclectImg.SetActive(true);
                }
            }
            else {
                UI_SclectImg.SetActive(false);
            }
        }
	}


    //更新道具显示
    void updateItem() {

        //根据背包位置ID读取当前背包对应的道具信息
        ItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", BagPosition, "RoseStoreHouse");

        //是否重新加载一次图片
        bool ifShowImg = true;
        if (ItemID == lastItemID)
        {
            ifShowImg = false;
        }
        lastItemID = ItemID;

        //当前道具ID不为空时 显示对应的道具信息
        if (ItemID != "0")
        {
            //显示道具
            /*
            UI_ItemQuility.active = true;
            UI_ItemQuility.active = true;
            UI_ItemIcon.active = true;
            UI_ItemNum.active = true;
            */
            ItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", BagPosition, "RoseStoreHouse");
            //获取道具的Icon和品质信息
			ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
			ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");

            //显示道具Icon
            object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
            itemIcon = obj as Sprite;
            UI_ItemIcon.GetComponent<Image>().sprite = itemIcon;

            //显示道具数量
            UI_ItemNum.GetComponent<Text>().text = ItemNum;
            //如果显示是装备则不显示道具数量
            if (itemType == "3") {
                UI_ItemNum.GetComponent<Text>().text = "";
            }

            //显示道具品质
            string itemQuality = Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality);
            obj = Resources.Load(itemQuality, typeof(Sprite));
            Sprite itemQuility = obj as Sprite;
            UI_ItemQuility.GetComponent<Image>().sprite = itemQuility;

            UI_ItemQuility.SetActive(true);
            UI_ItemIcon.SetActive(true);
            UI_ItemNum.SetActive(true);
        }
        else {
            //UI_ItemQuility.active = false;
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
		//拖拽时注销Tips
		if (obj_ItemTips != null) {
			Destroy(obj_ItemTips);
		}

        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus) {
            return;
        }

        //道具图标交换
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus = true; //开启道具移动状态
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
            if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End != "") {
                /*
                Debug.Log("vvvvv = " + Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial);
                if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "3")
                {
                    if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "1")
                    {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint("请点击取出按钮取出仓库道具！");
                        Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus = false;
                        Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus = false;
                        //更新拖拽道具显示
                        updateItem();
                        //注销移动的Icon
                        if (moveIconObj != null)
                        {
                            Destroy(moveIconObj);
                        }
                        return;
                    }
                }
                */
                //执行交换
                Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
                //更新拖拽道具显示
                updateItem();
            }
            
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus = false;
        }

        //道具放入快捷栏执行交换
        if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus) {
            Game_PublicClassVar.Get_function_UI.UI_MoveToMainSkill("0");
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
	}
	//鼠标松开注销Tips
	public void Mouse_Up(){
        /*
		if (obj_ItemTips != null) {
			Destroy(obj_ItemTips);
		}
        */
        //松开时清空移动值
        lastMovePosition_Y = 0;
        //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_Bag.GetComponent<UI_Bag>().Tra_BagSpaceList.parent.GetComponent<ScrollRect>().vertical = true;
        this.transform.parent.transform.parent.GetComponent<ScrollRect>().vertical = true;
	}

    //鼠标点击触发
    public void Mouse_Click() {

        //如果道具为空,点击清空Tips
        if (ItemID == "0")
        {
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
        }

        //出售选定道具
        if (Game_PublicClassVar.Get_game_PositionVar.SellItemStatus)
        {
            Game_PublicClassVar.Get_function_Rose.SellBagSpaceItemToMoney(BagPosition);
        }
        else {
            //判定栏内是否有道具
            if (ItemID != "0") {
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
                    
                    //获取目标是否是装备
                    if (itemType == "3")
                    {
                        obj_ItemTips.GetComponent<UI_EquipTips>().UIBagSpaceNum = BagPosition;
                        obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "4";

                        //获取极品属性
                        string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", BagPosition, "RoseStorehouse");
                        obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;

                        //获取宝石属性
                        string itemGemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", BagPosition, "RoseStorehouse");
                        string itemGemHole = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", BagPosition, "RoseStorehouse");
                        obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemID = itemGemID;
                        obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemHole = itemGemHole;
                    }
                    else { 
                        //其余默认为道具,如果其他道具需做特殊处理
                        obj_ItemTips.GetComponent<UI_ItemTips>().UIBagSpaceNum = BagPosition;
                        obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = "4";
                    }
                }
                else
                {
                    Destroy(obj_ItemTips);
                }

                //更新选中
                game_positionVar.RoseBagSpaceSelectType = "2";
                game_positionVar.RoseBagSpaceSelect = BagPosition;
                game_positionVar.RoseBagSpaceSelectStatus = true;
                UI_SclectImg.SetActive(true);

            }
        }
    }


    //移动背包栏位
    public void Mouse_Drag()
    {
		//屏蔽背包移动
		return;

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
            tra_BagSpaceList.GetComponent<RectTransform>().anchoredPosition = new Vector2(tra_BagSpaceList.GetComponent<RectTransform>().anchoredPosition.x, tra_BagSpaceList.GetComponent<RectTransform>().anchoredPosition.y + move_Y / 2);
            lastMovePosition_Y = Input.mousePosition.y;
            //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_Bag.GetComponent<UI_Bag>().Tra_BagSpaceList.parent.GetComponent<ScrollRect>().vertical = false;
            this.transform.parent.transform.parent.GetComponent<ScrollRect>().vertical = false;
        }
    }
}
