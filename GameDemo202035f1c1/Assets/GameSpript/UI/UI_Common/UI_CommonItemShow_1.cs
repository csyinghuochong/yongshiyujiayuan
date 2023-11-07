using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_CommonItemShow_1 : MonoBehaviour
{
    public ObscuredString ItemID;
    public ObscuredString ItemNum;
    public ObscuredString HindID;
	public ObscuredString HideStr;   			//显示隐藏属性,有了此属性HindId不生效
    public GameObject Obj_ItemIcon;
    public GameObject Obj_ItemName;
    public GameObject Obj_ItemQuality;
    public GameObject Obj_ItemNum;
    private float objSize;
    private GameObject obj_ItemTips;
    public bool IfHideName;
    public bool IfChangeSize;

    // Use this for initialization
    void Start()
    {
        //显示道具图标
        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
        string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
        //显示道具Icon
        object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ItemIcon.GetComponent<Image>().sprite = itemIcon;
        //显示品质
        object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
        Sprite itemQuality = obj2 as Sprite;
        Obj_ItemQuality.GetComponent<Image>().sprite = itemQuality;
        //显示名称
        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        Obj_ItemName.GetComponent<Text>().text = itemName;
        string showNum = ItemNum.ToString();
        if (ItemNum != "") {
            int itemNumInt = int.Parse(ItemNum);
            if (itemNumInt >= 10000) {
                int wanValue = (int)(itemNumInt / 10000);
                float yushu = itemNumInt / 10000;
                if (yushu == 0)
                {
                    showNum = wanValue + "W";
                }
                else {
                    showNum = wanValue + "W+";
                }
            }
        }
        //显示数量
        Obj_ItemNum.GetComponent<Text>().text = showNum;
        if (IfHideName) {
            Obj_ItemName.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IfChangeSize) {
            //逐步变大
            objSize = objSize + Time.deltaTime * 4;
            if (objSize >= 1)
            {
                objSize = 1;
            }
            this.gameObject.transform.localScale = new Vector3(objSize, objSize, objSize);
        }

    }

    //鼠标点击触发
    public void Mouse_Click()
    {
        //判定栏内是否有道具
        if (ItemID != "0")
        {
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
                obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(ItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet, false, HindID);
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");

                //获取目标是否是装备
                if (itemType == "3")
                {
                    obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "3";
					//是否特殊显示隐藏属性
					if(HideStr!=""){
						obj_ItemTips.GetComponent<UI_EquipTips>().ItemHide_PaiHangBangShowHideStr = HideStr;
						//obj_ItemTips.GetComponent<UI_EquipTips>().Obj_UIRoseEquipShow = this.gameObject;
						//obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "3";
					}
                }
                else
                {
                    obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = "3";
                }
            }
            else
            {
                //Destroy(obj_ItemTips);
            }

        }
    }

    public void Mouse_Up() {
        if (obj_ItemTips != null)
        {
            Destroy(obj_ItemTips);
        }
    }

}