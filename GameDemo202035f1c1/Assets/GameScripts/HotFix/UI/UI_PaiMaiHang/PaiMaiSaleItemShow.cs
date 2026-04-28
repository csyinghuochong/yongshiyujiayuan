using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class PaiMaiSaleItemShow : MonoBehaviour {

	public ObscuredString SaleListID;
	public ObscuredString ServerSellID;
	public ObscuredString ItemID;
	public ObscuredString ItemSaleGold;
	public ObscuredInt ItemSaleNum;
	public ObscuredInt ItemSaleTime;

	public GameObject Obj_Item;
	public GameObject Obj_ItemSet;
	//UI类
	public GameObject Obj_ItemName;
	public GameObject Obj_ItemSaleGold;
	public GameObject Obj_XuanZhongImg;


	public UI_PaiMaiHangSet paimaiUI;

	// Use this for initialization
	void Start () {
		paimaiUI = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>();

	}
	
	// Update is called once per frame
	void Update () {
		if (paimaiUI.ChuShouXuanZhongItemListStatus) {
			if (paimaiUI.Obj_ChuShouXuanZhongItemList != this.gameObject) {
				Obj_XuanZhongImg.SetActive (false);
			} else {
				Obj_XuanZhongImg.SetActive (true);
			}
		}
	}

	public void ShowSaleItem(){

		Game_PublicClassVar.Get_function_UI.DestoryTargetObj (Obj_ItemSet);

		//展示道具
		GameObject itemObj = (GameObject)Instantiate(Obj_Item);
		itemObj.transform.SetParent(Obj_ItemSet.transform);
		itemObj.transform.localScale = new Vector3(1, 1, 1);
		itemObj.GetComponent<UI_Common_ItemIcon> ().ItemID = ItemID;
		itemObj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = ItemSaleNum;
		itemObj.GetComponent<UI_Common_ItemIcon>().UpdateItem();
		itemObj.transform.localPosition = Vector3.zero;

		//获取道具名称
		string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
		Obj_ItemName.GetComponent<Text> ().text = itemName;
		Obj_ItemSaleGold.GetComponent<Text>().text = ItemSaleGold;

	}

	//选中出售道具
	public void SelceSaleItem(){
        //剩余时间以秒为单位处理
        /*
        string nowTimeStr = WWWSet.GetTimeStamp();
        int nowTimeValue = int.Parse(nowTimeStr);
        //防止出错
        if (ItemSaleTime <= 0) {
            ItemSaleTime = nowTimeValue;
        }
        int sellTimeValue = nowTimeValue - ItemSaleTime;
        //防止出错
        if (sellTimeValue <= 0) {
            sellTimeValue = 0;
        }
        */

        int time_min = (int)(ItemSaleTime / 60);
		int time_hourse = 0;
		if (time_min >= 60) {
			time_hourse = (int)(time_min / 60);
			time_min = time_min - time_hourse * 60;
		}

        string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("已上架");
        string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("小时");
        string langStr_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("分钟");

        string showTimeStr = "";
		if (time_hourse >= 1) {
			showTimeStr = langStr_1 + time_hourse + langStr_2 + time_min + langStr_3;
		} else {
			showTimeStr = langStr_1 + time_min + langStr_3;
		}

		if (paimaiUI == null) {
			paimaiUI = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePaiMaiHang.GetComponent<UI_PaiMaiHangSet>();
		}
		paimaiUI.Obj_ItemShengYuTime.GetComponent<Text>().text = showTimeStr;

		Obj_XuanZhongImg.SetActive (true);
		paimaiUI.Obj_ChuShouXuanZhongItemList = this.gameObject;
		paimaiUI.ChuShouXuanZhongItemListStatus = true;
	}
}
