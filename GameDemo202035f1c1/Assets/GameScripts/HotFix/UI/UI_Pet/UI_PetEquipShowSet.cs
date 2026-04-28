using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PetEquipShowSet : MonoBehaviour {

    //宠物洗炼相关
    public GameObject Obj_BagSpace;                 //动态创建的背包格子
    public GameObject Obj_BagSpaceListSet;
    public GameObject Obj_PetEquipShowSet;

    public string SelectPetID;
    public string SelectPetEquipSpace;

    public bool UpdatePetEquipShowStatus;
    public GameObject[] Obj_PetEquipSpaceList;

    // Use this for initialization
    void Start () {

        //显示背包道具
        BagItemSkillBtnList();

	}
	
	// Update is called once per frame
	void Update () {

        //更新装备栏显示
        if (UpdatePetEquipShowStatus) {
            UpdatePetEquipShowStatus = false;
            for (int i = 0; i < Obj_PetEquipSpaceList.Length; i++) {
                Obj_PetEquipSpaceList[i].GetComponent<UI_RoseEquipShow>().UpdataStatus = true;
            }
        }
	}

    //注销时设置
    void OnDestroy()
    {
        Game_PublicClassVar.Get_game_PositionVar.PetXiLianStatus = false;
    }

    //点击背包道具按钮
    public void BagItemSkillBtnList()
    {
        //Debug.Log("点击了道具");

        string nowItemSubType = "0";
        switch (SelectPetEquipSpace) {

            case "1":
                nowItemSubType = "201";
                break;

            case "2":
                nowItemSubType = "202";
                break;

            case "3":
                nowItemSubType = "203";
                break;

            case "4":
                nowItemSubType = "204";
                break;
        }

        //清空道具列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_BagSpaceListSet);
        //将自身的所有消耗品显示
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        //Debug.Log("RoseBagMaxNum = " + Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum);
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            string itemID = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            //Debug.Log("itemID = " + itemID);
            if (itemID != "" && itemID != "0")
            {
                string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");

				if (itemType == "3")
                {
                    string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
                    //if (itemSubType == "201" || itemSubType == "202" || itemSubType == "203") {
                    if (itemSubType == nowItemSubType)
                    {
                        //开始创建背包格子
                        GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
                        bagSpace.transform.SetParent(Obj_BagSpaceListSet.transform);
                        bagSpace.GetComponent<UI_BagSpace>().BagPosition = i.ToString();
                        bagSpace.GetComponent<UI_BagSpace>().SpaceType = "1";   //设置格子为背包属性
                        bagSpace.transform.localScale = new Vector3(1, 1, 1);
                        bagSpace.GetComponent<UI_BagSpace>().MoveBagStatus = true;
                    }
                }
            }
        }
    }

    public void Btn_Click() {

        string bagSpace = Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect;
        if (bagSpace != null && bagSpace != "0" && bagSpace != "")
        {
            //判断宠物装备等级
            string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpace, "RoseBag");
            string itemlv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", itemID, "Item_Template");
            string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", SelectPetID, "RosePet");
            if (int.Parse(petLv) >= int.Parse(itemlv))
            {
                Game_PublicClassVar.Get_function_Rose.PetEquip_Wear(bagSpace, SelectPetID, SelectPetEquipSpace);
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().Obj_PetEquipShowSet.SetActive(false);
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_LeftSet.SetActive(true);
                UpdatePetEquipShowStatus = true;
            }
            else
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("等级不足,无法装备");
            }
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请选择需要装备的宠物装备");
        }

    }

    public void Btn_Close() {

        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().Obj_PetEquipShowSet.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_LeftSet.SetActive(true);

    }
}
