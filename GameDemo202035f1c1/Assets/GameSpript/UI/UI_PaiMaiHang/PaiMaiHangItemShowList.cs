using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class PaiMaiHangItemShowList : MonoBehaviour {

	public ObscuredString PaimaiListID;
	public ObscuredString itemID;
	public ObscuredString itemGoldNum;
	public ObscuredString GoldNumDes;
    private ObscuredFloat lastMovePosition_Y;           //上一次Y坐标

    public GameObject Obj_Item;
	public GameObject Obj_ItemSet;
	public GameObject Obj_ItemGoldNum;
	public GameObject Obj_ItemNumDes;
	public GameObject Obj_ItemName;
	public GameObject Obj_XuanZhongImg;

	public UI_PaiMaiHangSet paimaiUI;

	// Use this for initialization
	void Start () {

		paimaiUI = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>();

	}
	
	// Update is called once per frame
	void Update () {
		if (paimaiUI.PaiMaiHangXuanZhongItemListStatus) {
			if (paimaiUI.Obj_PaiMaiHangXuanZhongItemList != this.gameObject) {
				Obj_XuanZhongImg.SetActive (false);
			} else {
				Obj_XuanZhongImg.SetActive (true);
			}
		}
	}

	public void ShowItem(){
		Obj_ItemGoldNum.GetComponent<Text> ().text = itemGoldNum;
		Obj_ItemNumDes.GetComponent<Text> ().text = GoldNumDes;

		Game_PublicClassVar.Get_function_UI.DestoryTargetObj (Obj_ItemSet);

		//展示道具
		GameObject itemObj = (GameObject)Instantiate(Obj_Item);
		itemObj.transform.SetParent(Obj_ItemSet.transform);
		itemObj.transform.localScale = new Vector3(1, 1, 1);
		itemObj.GetComponent<UI_Common_ItemIcon>().ItemID = itemID;
		itemObj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = 0;
		itemObj.GetComponent<UI_Common_ItemIcon>().UpdateItem();
		itemObj.transform.localPosition = Vector3.zero;

		//获取道具名称
		string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", itemID, "Item_Template");
		Obj_ItemName.GetComponent<Text> ().text = itemName;
	}

	//拍卖行
	public void Btn_PaiMaiListID(){
		//Debug.Log ("错错错错错错错错错错错");
		paimaiUI = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>();
		paimaiUI.nowSeleBuyItemID = PaimaiListID;
		paimaiUI.Obj_PaiMaiHangXuanZhongItemList = this.gameObject;
		paimaiUI.PaiMaiHangXuanZhongItemListStatus = true;
		paimaiUI.BuyItemNum = 1;	//点击后默认显示购买1个
		paimaiUI.ShowGold();
		Obj_XuanZhongImg.SetActive (true);
	}

    //显示拍卖行
    public void PaiMaiDrag() {

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
        //this.transform.parent.transform.parent.GetComponent<ScrollRect>().vertical = false;

    }

}
