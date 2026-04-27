using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Common_PastureItemIcon : MonoBehaviour {

    public string ItemID;
    public int NeedItemNum;
	public string BagPosition;
	public string ItemEquipTipsType;
    public GameObject Obj_NeedItemName;         //制造书名称
    public GameObject Obj_NeedItemQuality;      //制造道具品质显示
    public GameObject Obj_NeedItemIcon;         //制造道具图标显示
    public GameObject Obj_NeedItemNum;          //制造道具图标显示
    private GameObject obj_ItemTips;         	//道具的Tips
    public GameObject Obj_NeedItemPinZhi;       //道具品质
	public bool UpdateStatus;
	public bool IfShowWenHao;
	// Use this for initialization
	void Start () {
		UpdateStatus = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(UpdateStatus){
			UpdateStatus = false;
			UpdateItem();
		}
	}


	public void UpdateItem(){

        if (BagPosition == "" || BagPosition == null || BagPosition == "0")
        {
            Obj_NeedItemName.SetActive(false);
            Obj_NeedItemQuality.SetActive(false);
            Obj_NeedItemIcon.SetActive(false);
            Obj_NeedItemNum.SetActive(false);
            if (Obj_NeedItemPinZhi != null)
            {
                Obj_NeedItemPinZhi.SetActive(false);
            }
            return;
        }
        else {
            Obj_NeedItemName.SetActive(true);
            Obj_NeedItemQuality.SetActive(true);
            Obj_NeedItemIcon.SetActive(true);
            Obj_NeedItemNum.SetActive(true);
            if (Obj_NeedItemPinZhi != null)
            {
                Obj_NeedItemPinZhi.SetActive(true);
            }
        }


        ItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", BagPosition, "RosePastureBag");

        //读取数据
        if (!IfShowWenHao) {
			string needItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemName", "ID", ItemID, "Item_Template");
			string needItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemIcon", "ID", ItemID, "Item_Template");
			string needItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemQuality", "ID", ItemID, "Item_Template");

			//显示需求道具
			Obj_NeedItemName.GetComponent<Text> ().text = needItemName;

			//显示道具Icon
			object obj = Resources.Load ("ItemIcon/" + needItemIcon, typeof(Sprite));
			Sprite itemIcon = obj as Sprite;
			Obj_NeedItemIcon.GetComponent<Image> ().sprite = itemIcon;

			//显示品质
			object obj2 = Resources.Load (Game_PublicClassVar.Get_function_UI.ItemQualiytoPath (needItemQuality), typeof(Sprite));
			Sprite itemQuality = obj2 as Sprite;
			Obj_NeedItemQuality.GetComponent<Image> ().sprite = itemQuality;

			//显示数量
			if (NeedItemNum != 0) {
				Obj_NeedItemNum.GetComponent<Text> ().text = NeedItemNum.ToString ();
			} else {
				Obj_NeedItemNum.GetComponent<Text> ().text = "";
			}

            //显示品质
            if (Obj_NeedItemPinZhi != null) {
                string itemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", BagPosition, "RosePastureBag");
                Obj_NeedItemPinZhi.GetComponent<Text>().text = "品质:" + itemPar;
            }

		} else {

			//显示道具Icon
			object obj = Resources.Load ("GameUI/Img/" + "UI_Image_179", typeof(Sprite));
			Sprite itemIcon = obj as Sprite;
			Obj_NeedItemIcon.GetComponent<Image> ().sprite = itemIcon;

			//显示品质
			object obj2 = Resources.Load (Game_PublicClassVar.Get_function_UI.ItemQualiytoPath("1"), typeof(Sprite));
			Sprite itemQuality = obj2 as Sprite;
			Obj_NeedItemQuality.GetComponent<Image> ().sprite = itemQuality;

			//显示数量
			Obj_NeedItemNum.GetComponent<Text> ().text = "";

            //显示品质
            if (Obj_NeedItemPinZhi != null)
            {
                Obj_NeedItemPinZhi.GetComponent<Text>().text = "";
            }
        }

	}
	/*
    //显示制造道具的Tips
    public void Btn_NeedItemTips()
    {
        if (obj_ItemTips == null)
        {
            //获取当前Tips栏内是否有Tips,如果有就清空处理

            GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
            for (int i = 0; i < parentObj.transform.childCount; i++)
            {
                GameObject go = parentObj.transform.GetChild(i).gameObject;
                Destroy(go);
            }

            //实例化Tips
            obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(ItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");

            //获取目标是否是装备
            if (itemType == "3")
            {
                //obj_ItemTips.GetComponent<UI_EquipTips>().UIBagSpaceNum = BagPosition;
                obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "3";
                //获取极品属性
                //string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", BagPosition, "RoseBag");
                //obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;
            }
            else
            {
                //其余默认为道具,如果其他道具需做特殊处理
                //obj_ItemTips.GetComponent<UI_ItemTips>().UIBagSpaceNum = BagPosition;
                obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = "3";
            }
        }
        else
        {
            Destroy(obj_ItemTips);
        }
    }
	*/
	//鼠标点击触发
	public void Mouse_Click() {

        if (IfShowWenHao) {
            Debug.Log("问号不显示Tips");
            return;
        }

		//如果道具为空,点击清空Tips
		if (ItemID == "0") {
			Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
		}

        //取消放置
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_PastureHeChengSet_Open != null && Game_PublicClassVar.Get_game_PositionVar.Obj_PastureHeChengSet_Open.active == true) {
            Game_PublicClassVar.Get_game_PositionVar.Obj_PastureHeChengSet_Open.GetComponent<UI_PastureHeChengSet>().CancleSpaceID(BagPosition);
            return;
        }


        //取消放置
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_PastureNpcBuySet_Open != null && Game_PublicClassVar.Get_game_PositionVar.Obj_PastureNpcBuySet_Open.active == true)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_PastureNpcBuySet_Open.GetComponent<UI_PastureNpcBuySet>().CancleSpaceID(BagPosition);
            return;
        }

        //取消放置
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiJiHuoSet_Open != null && Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiJiHuoSet_Open.active == true)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiJiHuoSet_Open.GetComponent<UI_ZuoQiXianJiSet>().CancleSpaceID(BagPosition);
            return;
        }

        //出售选定道具
        if (Game_PublicClassVar.Get_game_PositionVar.SellItemStatus)
		{
			//Game_PublicClassVar.Get_function_Rose.SellBagSpaceItemToMoney(BagPosition);
			//SellItemObj.SetActive(false);
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
						obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = ItemEquipTipsType;
						//获取极品属性
						string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", BagPosition, "RosePastureBag");
						obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;
                        //获取宝石属性
                        string itemGemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", BagPosition, "RosePastureBag");
                        string itemGemHole = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", BagPosition, "RosePastureBag");
                        obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemID = itemGemID;
                        obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemHole = itemGemHole;
					}
					else { 
						//其余默认为道具,如果其他道具需做特殊处理
						obj_ItemTips.GetComponent<UI_ItemTips>().UIBagSpaceNum = BagPosition;
						obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = ItemEquipTipsType;
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
	}
}
