using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_PaiHangRoseEquipItemShow : MonoBehaviour
{

    public bool UpdataStatus;   //开启后更新数据
    public GameObject Obj_EquipQuility;
    public GameObject Obj_EquipIcon;   
    public string EquipID;
    public string HideStr;
	public string ItemParStr;
	public string GemIDStr;
	public string GemHoleStr;
    public string roseOcc;

    public string EquipSpaceNum;
    private string EquipIcon;
    private string EquipQuality;
    private Game_PositionVar game_positionVar;
	private GameObject obj_ItemTips;            //实例化的的道具Tips
	private GameObject moveIconObj;             //移动道具显示的图标
	private Sprite itemIcon;
    private GameObject EquipDi;


	// Use this for initialization
	void Start () {

        game_positionVar = Game_PublicClassVar.Get_game_PositionVar;
		EquipDi = this.transform.Find("Img_EquipBackText").gameObject;
		if (EquipID != "0") {
			EquipDi.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
        
        if (UpdataStatus) {
            UpdataStatus = false;
            //EquipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", EquipSpaceNum, "RoseEquip");
            if (EquipID != "0") {
                //根据装备ID显示对应的数据
                EquipIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", EquipID, "Item_Template", roseOcc);
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
                //EquipDi.SetActive(false);
            }
            else
            {
                //如果为0显示空数据
                Obj_EquipQuility.GetComponent<Image>().sprite = null;
                Obj_EquipIcon.GetComponent<Image>().sprite = null;
				Obj_EquipQuility.SetActive(false);
				Obj_EquipIcon.SetActive(false);
                //EquipDi.SetActive(true);
            }
        }

        //交换物品后,检测是否为自己的格子道具发生变化,发生变化清空值
        /*
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
        */
        //更新底框
        if (EquipID != "0")
        {
            EquipDi.SetActive(false);
        }
        else
        {
            EquipDi.SetActive(true);
        }
        
	}
    /*
    //进入显示装备Tips
    public void EnterEquipTips() {
        Debug.Log("显示Tips");

        //显示装备Tips

        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus) {
            Debug.Log("开始拖拽装备");
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "2";
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = EquipSpaceNum;
        }
    }

    //退出显示装备Tips
    public void ExitEquipTips() {
        Debug.Log("离开Tips");
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
    */
	//鼠标按下 显示Tips
	public void Mouse_Down(){
		//调用方法显示UI的Tips
        /*
		if (obj_ItemTips == null) {
			obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(EquipID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
		}
         */
	}

	//鼠标松开注销Tips
	public void Mouse_Up(){
		if (obj_ItemTips != null) {
			Destroy(obj_ItemTips);
		}
	}

    public void Mouse_Click() {

        if (EquipID == "0") {
            //Debug.Log("当前栏位没有装备");
            return;
        }

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
            obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(EquipID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet,false);
            obj_ItemTips.GetComponent<UI_EquipTips>().ItemHide_PaiHangBangShowHideStr = HideStr;
			obj_ItemTips.GetComponent<UI_EquipTips> ().ItemGemHole = GemHoleStr;
			obj_ItemTips.GetComponent<UI_EquipTips> ().ItemGemID = GemIDStr;
            obj_ItemTips.GetComponent<UI_EquipTips>().Obj_UIRoseEquipShow = this.gameObject;
            obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "3";
            obj_ItemTips.GetComponent<UI_EquipTips>().PaiHangShowShowOcc = roseOcc;
            //Debug.Log("HideStr = " + HideStr);
            /*
            //获取极品属性
            string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", EquipSpaceNum, "RoseEquip");
            obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;
            */
        }
        else {
            Destroy(obj_ItemTips);
        }
    }
}
