using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_OrnamentChoice : MonoBehaviour {

    public GameObject Obj_Choice_1;
    public GameObject Obj_Choice_2;
    public GameObject Obj_Choice_3;
    public string UIBagSpaceNum;

	// Use this for initialization
	void Start () {

        //获取当前饰品
        string oranmentID_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", "5", "RoseEquip");
        string oranmentID_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", "6", "RoseEquip");
        string oranmentID_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", "7", "RoseEquip"); 

        //获取名字
        string oranmentName_1 = "无";
        string oranmentName_2 = "无";
        string oranmentName_3 = "无";
        if (oranmentID_1 != "0") {
            oranmentName_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", oranmentID_1, "Item_Template");
        }
        if (oranmentID_2 != "0") {
            oranmentName_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", oranmentID_2, "Item_Template");
        }
        if (oranmentID_3 != "0") {
            oranmentName_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", oranmentID_3, "Item_Template");
        }
        

        Obj_Choice_1.GetComponent<Text>().text = "当前饰品：" + oranmentName_1 + "(点击更换)";
        Obj_Choice_2.GetComponent<Text>().text = "当前饰品：" + oranmentName_2 + "(点击更换)";
        Obj_Choice_3.GetComponent<Text>().text = "当前饰品：" + oranmentName_3 + "(点击更换)";

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //更换饰品1
    public void BtnChoice_1() 
    {
        choiceOrnament("5");
    }

    //更换饰品2
    public void BtnChoice_2()
    {
        choiceOrnament("6");
    }
        
    //更换饰品3
    public void BtnChoice_3()
    {
        choiceOrnament("7");
    }

    void choiceOrnament(string ornamentValue) {

        if (UIBagSpaceNum != "-1") { 
            Game_PositionVar game_PublicClassVar = Game_PublicClassVar.Get_game_PositionVar;
            game_PublicClassVar.ItemMoveValue_Initial = UIBagSpaceNum;
            game_PublicClassVar.ItemMoveType_Initial = "1";
            game_PublicClassVar.ItemMoveValue_End = ornamentValue;
            game_PublicClassVar.ItemMoveType_End = "2";
            bool ifMove = Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
            //删除Tips
            Destroy(this.gameObject);
            //更新背包
            //Obj_UIBagSpace.GetComponent<UI_BagSpace>().UpdataItemShow = true;
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            //角色装备更新指定某个装备
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().UpdataEquipOne(ornamentValue);
            //Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;
            //更新装备属性
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;

            if (ifMove) {

                //获取当前Tips栏内是否有Tips,如果有就清空处理
                GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
                for (int i = 0; i < parentObj.transform.childCount; i++)
                {
                    GameObject go = parentObj.transform.GetChild(i).gameObject;
                    Destroy(go);
                }
            }
        }
    }
}
