using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_TaskTrophyList : MonoBehaviour {

	public string ItemID;
	public string ItemNum;
	public GameObject Obj_ItemIcon;
	public GameObject Obj_ItemQuality;
	public GameObject Obj_ItemNum;
	private GameObject obj_ItemTips;

	// Use this for initialization
	void Start () {

		//当前道具ID不为空时 显示对应的道具信息
		if (ItemID != "0")
		{
			//获取道具的Icon和品质信息
			string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
			string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
			
			//显示道具Icon
			object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
			Sprite itemIcon = obj as Sprite;
			Obj_ItemIcon.GetComponent<Image>().sprite = itemIcon;
			
			//显示道具数量
			Obj_ItemNum.GetComponent<Text>().text = ItemNum;
			
			//显示道具品质
			string itemQuality = Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality);
			obj = Resources.Load(itemQuality, typeof(Sprite));
			Sprite itemQuility = obj as Sprite;
			Obj_ItemQuality.GetComponent<Image>().sprite = itemQuility;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//鼠标按下 显示Tips
	public void Mouse_Down(){
		//调用方法显示UI的Tips
		if (obj_ItemTips == null) {
			obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(ItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
		}
	}
	//鼠标松开注销Tips
	public void Mouse_Up(){
		if (obj_ItemTips != null) {
			Destroy(obj_ItemTips);
		}
	}
}
