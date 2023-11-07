using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_FuBenDropItemObj : MonoBehaviour
{

    public string ItemID;
    public GameObject Obj_ItemIcon;
    public GameObject Obj_ItemName;
    public GameObject Obj_ItemQuality;
    public bool ifshow;
    private GameObject obj_ItemTips;


    // Use this for initialization
    void Start()
    {
        //ItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", HonorStoreID, "HonorStore_Template");
        //显示道具图标
        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
        string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");


        //显示名称
        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        Obj_ItemName.GetComponent<Text>().text = itemName;
        //显示兑换价格
        /*
        buyPrice = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuyPrice", "ID", HonorStoreID, "HonorStore_Template");
        Obj_BuyPrice.GetComponent<Text>().text = buyPrice + "荣誉";
        */
        if (!ifshow)
        {
            object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
            Material huiMaterial = huiObj as Material;
            //Obj_BtnImg.GetComponent<Image>().material = huiMaterial;
            Obj_ItemName.SetActive(false);
            //Obj_BuyPrice.GetComponent<Text>().text = "未激活道具";
            //Obj_BuyPrice.SetActive(false);
        }
        else {
            //显示道具Icon
            object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
            Sprite itemIcon = obj as Sprite;
            Obj_ItemIcon.GetComponent<Image>().sprite = itemIcon;

            //显示品质
            object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
            Sprite itemQuality = obj2 as Sprite;
            Obj_ItemQuality.GetComponent<Image>().sprite = itemQuality;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //鼠标点击触发
    public void Mouse_Click()
    {
        if (!ifshow) {
            return;
        }
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
                obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(ItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");

                //获取目标是否是装备
                if (itemType == "3")
                {
                    obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "3";
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

    public void Mouse_Up()
    {
        if (obj_ItemTips != null)
        {
            Destroy(obj_ItemTips);
        }
    }

}