using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_RoseEquipShow : MonoBehaviour {

    public bool UpdataStatus;   //开启后更新数据
    public GameObject Obj_EquipQuility;
    public GameObject Obj_EquipIcon;   
    public ObscuredString EquipID;
    public string EquipSpaceNum;
    private ObscuredString EquipIcon;
    private ObscuredString EquipQuality;
    private Game_PositionVar game_positionVar;
	private GameObject obj_ItemTips;            //实例化的的道具Tips
	private GameObject moveIconObj;             //移动道具显示的图标
	private Sprite itemIcon;
    private GameObject EquipDi;
    public int EquipType;                    //"0"表示装备  1表示十二生肖  2表示宠物装备

	// Use this for initialization
	void Start () {

        game_positionVar = Game_PublicClassVar.Get_game_PositionVar;
		EquipDi = this.transform.Find("Img_EquipBackText").gameObject;
		if (EquipID != "0") {
			EquipDi.SetActive(false);
		}

        //初始化更新
        UpdataStatus = true;

    }
	
	// Update is called once per frame
	void Update () {

        if (UpdataStatus) {
            UpdataStatus = false;

            if (EquipType == 0) {
                EquipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", EquipSpaceNum, "RoseEquip");
            }
            if (EquipType == 1)
            {
                string EquipIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShengXiaoSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                //Debug.Log("EquipIDStr = " + EquipIDStr);
                if (EquipIDStr != "" && EquipIDStr != "0" || EquipIDStr != null)
                {
                    string[] EquipIDList = EquipIDStr.Split(';');
                    EquipID = EquipIDList[int.Parse(EquipSpaceNum) - 1];
                    //Debug.Log("EquipID = " + EquipID);
                }
            }
            if (EquipType == 2) {
                if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet != null)
                {
                    string nowPetID = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().NowSclectPetID;
                    EquipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_" + EquipSpaceNum, "ID", nowPetID, "RosePet");
                }
            }

            if (EquipID != "0" && EquipID != "") {
                //根据装备ID显示对应的数据
                EquipIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", EquipID, "Item_Template");
                EquipQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", EquipID, "Item_Template");
                //显示道具品质
                string itemQuality = Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(EquipQuality);
                object Equipobj = Resources.Load(itemQuality, typeof(Sprite));
				Sprite itemQuility = Equipobj as Sprite;
                Obj_EquipQuility.GetComponent<Image>().sprite = itemQuility;
                //显示道具Icon
                string equipIcon = Game_PublicClassVar.Get_function_UI.EquipIconToPath(EquipIcon);
				Equipobj = Resources.Load(equipIcon, typeof(Sprite));
				itemIcon = Equipobj as Sprite;
                Obj_EquipIcon.GetComponent<Image>().sprite = itemIcon;
				Obj_EquipQuility.SetActive(true);
				Obj_EquipIcon.SetActive(true);

                GameObject backObj = this.transform.Find("Img_EquipBack").gameObject;
                if (backObj != null) {
                    backObj.SetActive(false);
                }

                //Img_EquipBack
                //EquipDi.SetActive(false);
            }
            else
            {
                //如果为0显示空数据
                Obj_EquipQuility.GetComponent<Image>().sprite = null;
                Obj_EquipIcon.GetComponent<Image>().sprite = null;
				Obj_EquipQuility.SetActive(false);
				Obj_EquipIcon.SetActive(false);

                GameObject backObj = this.transform.Find("Img_EquipBack").gameObject;
                if (backObj != null)
                {
                    backObj.SetActive(true);
                }
                //EquipDi.SetActive(true);
            }
        }

        //交换物品后,检测是否为自己的格子道具发生变化,发生变化清空值
        if (game_positionVar.UpdataRoseItem)
        {
            if (game_positionVar.ItemMoveType_End == "2")
            {
                if (game_positionVar.ItemMoveValue_End == EquipSpaceNum)
                {
                    UpdataStatus = true;
                    //game_positionVar.UpdataRoseItem = false;
                    Debug.Log("装备触发了更新事件");

                }
            }
        }

        //更新底框
        if (EquipID != "0")
        {
            EquipDi.SetActive(false);
        }
        else
        {
            EquipDi.SetActive(false);
        }
	}

    //进入显示装备Tips
    public void EnterEquipTips() {
        //Debug.Log("显示Tips");
        //显示装备Tips

        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus) {
            Debug.Log("开始拖拽装备");
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "2";
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = EquipSpaceNum;
        }
    }

    //退出显示装备Tips
    public void ExitEquipTips() {
        //Debug.Log("离开Tips");
        //如果移动装备开关打开放入对应的装备
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
        }
    }

    //拖动装备
    public void StartDragEquipIcon() {
		//拖拽时注销Tips
		if (obj_ItemTips != null) {
			Destroy(obj_ItemTips);
		}
        Debug.Log("开始拖拽装备");
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus = true; //开启道具移动状态
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial = "2";
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial = EquipSpaceNum;

		//实例化道具图标
		moveIconObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UIMoveIcon);
		moveIconObj.GetComponent<UI_MoveItemIcon>().itemIconSprite = itemIcon;      //传入图标精灵
		moveIconObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet.transform);
		moveIconObj.transform.localScale = new Vector3(1, 1, 1);
    }

    //结束拖拽装备
    public void EndDragEquipIcon() {
        Debug.Log("结束拖拽装备");
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus)
        {
            if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End != "")
            {
                //执行交换
                Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
                //更新装备
                UpdataStatus = true;
            }
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus = false;
        }

		//注销移动的Icon
		if (moveIconObj != null) {
			Destroy(moveIconObj);
		}
    }

	//鼠标按下 显示Tips
	public void Mouse_Down(){

        //调用方法显示UI的Tips
        /*
		if (obj_ItemTips == null) {
			obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(EquipID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
		}
        */

        //设置为宝石镶嵌选中的装备
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip != null) {
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipGemHoleSet.activeSelf)
            {

                //获取当前选中镶嵌的位置
                UI_EquipGemHoleSet ui_EquipGemHoleSet = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipGemHoleSet.GetComponent<UI_EquipGemHoleSet>();
                ui_EquipGemHoleSet.equipSpaceType = "2";
                ui_EquipGemHoleSet.equipSpace = EquipSpaceNum;
                ui_EquipGemHoleSet.UpdateEquipGemStatus = true;
                //展示Tips
                Mouse_Click();
            }
        }


        //判定当前是否在强化
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip != null)
        {
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().RoseEquipStatus == "4")
            {
                Debug.Log("我点击了强化！");
            }
        }

        //更新背包选中框
        //更新选中
        game_positionVar.RoseBagSpaceSelect = "-1";     //不显示任何背包选中
        game_positionVar.RoseBagSpaceSelectStatus = true;
        //Debug.Log("sssssssssssssssssssssssssssssssssssssssssssssssssssssssss");
        //UI_SclectImg.SetActive(true);
	}
	//鼠标松开注销Tips
	public void Mouse_Up(){
		if (obj_ItemTips != null) {
			Destroy(obj_ItemTips);
		}
	}

    public void Mouse_Click() {

        if (EquipID == "0"|| EquipID == "") {
            Debug.Log("当前栏位没有装备");

            if (EquipType == 2)
            {
                if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet != null)
                {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().Obj_PetEquipShowSet.SetActive(true);
                    string nowPetID = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().NowSclectPetID;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().SelectPetID = nowPetID;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().SelectPetEquipSpace = EquipSpaceNum;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_LeftSet.SetActive(false);
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().BagItemSkillBtnList();
                }
            }

            return;
        }

        //清空装备Tips显示
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        //调用方法显示UI的Tips
        if (obj_ItemTips == null)
        {

            obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(EquipID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet, false);
            obj_ItemTips.GetComponent<UI_EquipTips>().Obj_UIRoseEquipShow = this.gameObject;
            obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "2";

            //角色装备
            if (EquipType == 0)
            {
                //获取极品属性
                string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", EquipSpaceNum, "RoseEquip");
                obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;
                //获取宝石属性
                string itemGemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", EquipSpaceNum, "RoseEquip");
                string itemGemHole = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", EquipSpaceNum, "RoseEquip");
                obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemID = itemGemID;
                obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemHole = itemGemHole;
                //显示强化
                string itemQiangHuaID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaID", "ID", EquipSpaceNum, "RoseEquip");
                obj_ItemTips.GetComponent<UI_EquipTips>().EquipQiangHuaID = itemQiangHuaID;
            }

            //宠物装备
            if (EquipType == 2)
            {
                //获取极品属性
                if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet != null)
                {
                    string nowPetID = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().NowSclectPetID;
                    string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipIDHide_" + EquipSpaceNum, "ID", nowPetID, "RosePet");
                    obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;
                }
            }

        }
        else {
            Destroy(obj_ItemTips);
        }
    }

    /*
    private void showEquipTips() {

        //Debug.Log("我点击了装备");
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
        //调用方法显示UI的Tips
        if (obj_ItemTips == null)
        {
            obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(EquipID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet, false);
            obj_ItemTips.GetComponent<UI_EquipTips>().Obj_UIRoseEquipShow = this.gameObject;
            obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "2";
            //获取极品属性
            string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", EquipSpaceNum, "RoseEquip");
            obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;

        }
        else
        {
            Destroy(obj_ItemTips);
        }

    }
     */
}
