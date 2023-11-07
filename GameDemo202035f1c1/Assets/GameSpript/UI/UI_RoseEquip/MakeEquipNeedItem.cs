using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MakeEquipNeedItem : MonoBehaviour {

    public string ItemID;
    public int NeedItemNum;
    public GameObject Obj_NeedItemName;         //制造书名称
    public GameObject Obj_NeedItemQuality;      //制造道具品质显示
    public GameObject Obj_NeedItemIcon;         //制造道具图标显示
    public GameObject Obj_NeedItemNum;         //制造道具图标显示
    private GameObject obj_ItemTips;         //道具的Tips
	// Use this for initialization
	void Start () {

        //读取数据
        string needItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        string needItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
        string needItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");

        //显示需求道具
        Obj_NeedItemName.GetComponent<Text>().text = needItemName;

        //显示道具Icon
        object obj = Resources.Load("ItemIcon/" + needItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_NeedItemIcon.GetComponent<Image>().sprite = itemIcon;

        //显示品质
        object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(needItemQuality), typeof(Sprite));
        Sprite itemQuality = obj2 as Sprite;
        Obj_NeedItemQuality.GetComponent<Image>().sprite = itemQuality;

        //显示数量
        int selfNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        Obj_NeedItemNum.GetComponent<Text>().text = selfNum + "/" + NeedItemNum;
        if (selfNum >= NeedItemNum)
        {
            Obj_NeedItemNum.GetComponent<Text>().color = Color.green;
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("已达成");
            Obj_NeedItemNum.GetComponent<Text>().text = selfNum + "/" + NeedItemNum + "("+langStr+")";
        }
        else {
            Obj_NeedItemNum.GetComponent<Text>().color = Color.red;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

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
}
