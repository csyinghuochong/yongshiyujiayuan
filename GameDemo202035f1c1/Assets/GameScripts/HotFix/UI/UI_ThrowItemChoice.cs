using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ThrowItemChoice : MonoBehaviour {
    public string ItemID;
    public GameObject Obj_ThrowItemName;
    public string BagSpaceNum;
    public GameObject Obj_SellGoldHint;
	// Use this for initialization
	void Start () {
        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        Obj_ThrowItemName.GetComponent<Text>().text = itemName;
        //显示出售价格

        string sellMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("sellMoney", "ID", ItemID, "Item_Template");
        if (sellMoney != "" || sellMoney != "0" || sellMoney != null)
        {
            Obj_SellGoldHint.GetComponent<Text>().text = "出售单价：" + sellMoney + "金币/1个";
        }
        else {
            Obj_SellGoldHint.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //丢弃一个
    public void ThrowOne() {
        Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(ItemID, 1, BagSpaceNum, false);
        //获取道具的出售金额
        string sellValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", ItemID, "Item_Template");
        Game_PublicClassVar.Get_function_Rose.SendRewardToBag("1", int.Parse(sellValue),"0",0,"0",true,"23");
        //Game_PublicClassVar.Get_function_Rose.SellBagSpaceItemToMoney(UIBagSpaceNum);
        //获取当前格子剩余数量
        string spaceNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", BagSpaceNum, "RoseBag");
        if (int.Parse(spaceNum) <= 0) {
            //获取当前Tips栏内是否有Tips,如果有就清空处理
            GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
            for (int i = 0; i < parentObj.transform.childCount; i++)
            {
                GameObject go = parentObj.transform.GetChild(i).gameObject;
                Destroy(go);
            }
        }

        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;   //更新背包数据

    }

    //丢弃全部个
    public void ThrowAll() {
        //Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(ItemID, 1, BagSpaceNum, true);
        Game_PublicClassVar.Get_function_Rose.SellBagSpaceItemToMoney(BagSpaceNum);
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;   //更新背包数据
    }
}
