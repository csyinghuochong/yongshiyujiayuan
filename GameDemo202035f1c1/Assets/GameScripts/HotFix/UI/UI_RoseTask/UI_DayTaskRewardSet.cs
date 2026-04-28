using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DayTaskRewardSet : MonoBehaviour {

    public string ItemID;
    public string ItemNum;

    public GameObject Obj_ItemIcon;
    public GameObject Obj_ItemNum;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowItem() {
        //显示道具Icon
        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
        //string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
        object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ItemIcon.GetComponent<Image>().sprite = itemIcon;
        Obj_ItemNum.GetComponent<Text>().text = ItemNum;
        //经验设置为原始大小
        if (ItemID == "2") {
            Obj_ItemIcon.GetComponent<Image>().SetNativeSize();
            Obj_ItemNum.transform.localPosition = new Vector3(Obj_ItemNum.transform.localPosition.x + 10.0f, Obj_ItemNum.transform.localPosition.y, Obj_ItemNum.transform.localPosition.z);
        }

    }
}
