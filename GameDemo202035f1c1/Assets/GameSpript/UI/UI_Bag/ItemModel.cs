using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel  {


	public string ItemID;			//物品ID
	public int ItemNum;			//物品数量
	public string ItemPar;		//物品参数
	public string HideID;			//物品隐藏ID
	public string GemHole;			//宝石
	public string GemID;            //宝石ID

	private string ItemType = "";
	private string ItemQuality = "";

	public ItemModel()
	{
		ItemID = "";
		ItemNum = 0;
		ItemPar = "";
		HideID = "";
		GemHole = "";
		GemID = "";
		ItemType = "0";
		ItemQuality = "0";
	}

	public void SetTypeAndQuality()
	{
		if (ItemID == "" || ItemID == "0")
		{
			ItemType = "0";
			ItemQuality = "0";
			return;
		}
		Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
		ItemType = function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");
		ItemQuality = function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
	}

	public int GetItemType() 
	{
		return int.Parse(ItemType);
	}

	public int GetItemQuality()
	{
		return int.Parse(ItemQuality);
	}
}
