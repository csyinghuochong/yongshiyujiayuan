using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureFuMoSet : MonoBehaviour
{

    public string[] SpaceListStr;
    public GameObject[] Obj_SpaceShowList;
    public GameObject Obj_BagSpace;
    public GameObject Obj_BagSpaceListSet;

    public ObscuredString EquipSpacePosition;
    public GameObject Obj_EquipShow;

    // Use this for initialization
    void Start()
    {
        Game_PublicClassVar.Get_game_PositionVar.Obj_PastureFuMoSet_Open = this.gameObject;

        //UpdateBag();
        InitShow();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitShow()
    {
        /*
        //循环删除道具
        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            Game_PublicClassVar.function_Rose.CostBagSpaceNumItem(Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().ItemID, 1, SpaceListStr[i], false);
        }

        ClearnShow();

        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        */

        UpdateBag();
    }


    //放置材料ID
    public void PutSpaceID(string spaceID)
    {

        string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
        string nowItemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", nowItemID, "Item_Template");
        string nowItemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", nowItemID, "Item_Template");
        if (nowItemType == "6" && nowItemSubType == "2")
        {
            PutSpaceItemID(spaceID);
            return;
        }
        if (nowItemType == "3")
        {
            if (int.Parse(nowItemSubType) <= 100) {
                PutSpaceEquipID(spaceID);
                return;
            }
        }
    }


    //放置装备ID
    public bool PutSpaceEquipID(string spaceID)
    {

        string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
        string nowItemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", nowItemID, "Item_Template");
        string nowItemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", nowItemID, "Item_Template");

        if (nowItemType != "3")
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("放入道具不符,请放入可以合成的原材料");
            return false;
        }

        if (EquipSpacePosition == "0" || EquipSpacePosition == "")
        {
            EquipSpacePosition = spaceID;
            Obj_EquipShow.GetComponent<UI_Common_ItemIcon>().BagPosition = spaceID;
            Obj_EquipShow.GetComponent<UI_Common_ItemIcon>().ItemID = nowItemID;
            Obj_EquipShow.GetComponent<UI_Common_ItemIcon>().UpdateItem();
            return true;
        }
        else
        {
            if (EquipSpacePosition == spaceID)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经放置了此道具,请务重复放置");
                return false;
            }
        }
        

        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("放置栏位已满!");
        return false;

    }


    //放置材料ID
    public bool PutSpaceItemID(string spaceID)
    {

        string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
        string nowItemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", nowItemID, "Item_Template");
        string nowItemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", nowItemID, "Item_Template");
        if (nowItemType != "6" && nowItemSubType != "2")
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("放入道具不符,请放入可以合成的原材料");
            return false;
        }

        //检测是否放入了相同的果实
        string nowPutItemID = "";
        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            //检测当前道具栏内是否有道具
            if (SpaceListStr[i] != "0" && SpaceListStr[i] != "")
            {
                nowPutItemID = Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().ItemID;
            }
        }

        if (nowPutItemID != "") {
            if (nowPutItemID != nowItemID) {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请放入相同的果实!");
                return false;
            }
        }

        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            if (SpaceListStr[i] == spaceID)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经放置了此道具,请务重复放置");
                return false;
            }
        }


        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            if (SpaceListStr[i] == "0" || SpaceListStr[i] == "")
            {
                SpaceListStr[i] = spaceID;
                Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().BagPosition = spaceID;
                Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().ItemID = nowItemID;
                Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().UpdateItem();
                return true;
            }
        }

        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("放置栏位已满!");
        return false;

    }

    public void CancleSpaceID(string spaceID)
    {

        string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
        string nowItemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", nowItemID, "Item_Template");
        string nowItemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", nowItemID, "Item_Template");
        if (nowItemType == "6" && nowItemSubType == "2") {
            for (int i = 0; i < SpaceListStr.Length; i++)
            {
                if (SpaceListStr[i] == spaceID)
                {
                    SpaceListStr[i] = "0";
                    Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().BagPosition = "";
                    Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().ItemID = "";
                    Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().UpdateItem();
                }
            }
        }

        if (nowItemType == "3") {
            EquipSpacePosition = "0";
            Obj_EquipShow.GetComponent<UI_Common_ItemIcon>().BagPosition = "";
            Obj_EquipShow.GetComponent<UI_Common_ItemIcon>().ItemID = "";
            Obj_EquipShow.GetComponent<UI_Common_ItemIcon>().UpdateItem();
        }
    }


    //清理显示
    public void ClearnShow()
    {

        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            SpaceListStr[i] = "0";
            Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().BagPosition = "0";
            Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().ItemID = "0";
            Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().UpdateItem();
        }

    }

    //清理牧场仓库道具
    
    public void CostBagItem()
    {
        //循环删除道具
        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            //Debug.Log("Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().ItemID = " + Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().ItemID + "SpaceListStr[i] = " + SpaceListStr[i]);
            if (Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().ItemID!=null&& Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().ItemID!= "0" && Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().ItemID!="") {
                Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(Obj_SpaceShowList[i].GetComponent<UI_Common_ItemIcon>().ItemID, 1, SpaceListStr[i], false);
            }
        }

        //ClearnShow();

        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

    }
    

    //附魔按钮
    public void Btn_FuMo()
    {

        //判断背包格子有位置
        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() <= 0)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("角色背包已满!");
            return;
        }

        if (EquipSpacePosition == ""|| EquipSpacePosition=="0"|| EquipSpacePosition==null) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先放置装备...");
            return;
        }

        //获取放入道具的数量和总品质
        int hechengNum = 0;
        int nowItemParSum = 0;          //品质总和

        string randomItemID ="";        //设置随机道具ID

        string lastItemID = "";

        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            if (SpaceListStr[i] != "" && SpaceListStr[i] != "0" && SpaceListStr[i] != null)
            {
                hechengNum = hechengNum + 1;
                string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", SpaceListStr[i], "RoseBag");
                string nowItemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", SpaceListStr[i], "RoseBag");
                string nowItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", nowItemID, "Item_Template");
                
                nowItemParSum = nowItemParSum + int.Parse(nowItemPar);

                if (randomItemID == "")
                {
                    randomItemID = SpaceListStr[i];
                }
                else {
                    if (Random.value <= 0.4f) {
                        randomItemID = SpaceListStr[i];
                    }
                }
            }
        }

        if (hechengNum<1) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请放入附魔道具!");
            return;
        }

        float randMin = 0;
        float randMax = 1;

        //品质控制范围
        float pingJunValue = (float)nowItemParSum / (float)hechengNum / 100;
        pingJunValue = pingJunValue + (hechengNum-1) * 0.15f;

        randMin = pingJunValue - 0.2f;
        randMax = pingJunValue + 0.2f;

        if (randMin <= 0) {
            randMin = 0;
        }


        //数量控制上限
        float shangXianMax = 0;
        switch (hechengNum) {
            case 1:
                shangXianMax = 0.5f;
                break;

            case 2:
                shangXianMax = 0.75f;
                break;

            case 3:
                shangXianMax = 1f;
                break;
        }

        randMax = randMax * shangXianMax;

        //形成最终随机值
        if (randMax >= shangXianMax)
        {
            randMax = shangXianMax;
        }

        if (randMin >= randMax - 0.2f)
        {
            randMin = randMax - 0.2f;
        }

        if (randMin <= 0)
        {
            randMin = 0;
        }

        /*
        if (hechengNum <= 1)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("合成至少需要2个材料");
            return;
        }
        */

        //获取附魔属性,根据放入的材料
        /*
        string fumoValue = Game_PublicClassVar.Get_function_Rose.ReturnFuMoProperty(randomItemID, EquipSpacePosition,randMin, randMax);

        //添加附魔属性
        string nowHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", EquipSpacePosition, "RoseBag");
        if (nowHideID == "" || nowHideID == "0" || nowHideID == null)
        {
            //新建一个隐藏属性
            nowHideID = Game_PublicClassVar.Get_function_Rose.ReturnNullHidePropertyID();
        }

        //string PrepeotyList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", nowHideID, "RoseEquipHideProperty");
        string PrepeotyList = Game_PublicClassVar.Get_function_Rose.GetHideNoFuMoValue(nowHideID);                      //如果有附魔属性清除附魔属性
        if (PrepeotyList != "" && PrepeotyList != "0")
        {
            PrepeotyList = PrepeotyList + ";" + fumoValue;
        }
        else {
            PrepeotyList = fumoValue;
        }



        //添加附魔属性
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", nowHideID,"ID", EquipSpacePosition, "RoseBag");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PrepeotyList", PrepeotyList, "ID", nowHideID, "RoseEquipHideProperty");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquipHideProperty");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");

        //显示
        ClearnShow();
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("附魔属性成功!");

        //更新装备属性
        Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;
        */

        EquipFoMo(randomItemID, EquipSpacePosition, randMin, randMax);

        //清理道具删除道具
        CostBagItem();
        //显示
        ClearnShow();

    }

    private void EquipFoMo(string randomItemID, string EquipSpacePosition,float randMin,float randMax) {

        //获取附魔属性,根据放入的材料
        string fumoValue = Game_PublicClassVar.Get_function_Rose.ReturnFuMoProperty(randomItemID, EquipSpacePosition, randMin, randMax);

        //添加附魔属性
        string nowHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", EquipSpacePosition, "RoseBag");
        if (nowHideID == "" || nowHideID == "0" || nowHideID == null)
        {
            //新建一个隐藏属性
            nowHideID = Game_PublicClassVar.Get_function_Rose.ReturnNullHidePropertyID();
        }

        //string PrepeotyList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", nowHideID, "RoseEquipHideProperty");
        string PrepeotyList = Game_PublicClassVar.Get_function_Rose.GetHideNoFuMoValue(nowHideID);                      //如果有附魔属性清除附魔属性
        if (PrepeotyList != "" && PrepeotyList != "0")
        {
            PrepeotyList = PrepeotyList + ";" + fumoValue;
        }
        else
        {
            PrepeotyList = fumoValue;
        }

        //添加附魔属性
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", nowHideID, "ID", EquipSpacePosition, "RoseBag");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PrepeotyList", PrepeotyList, "ID", nowHideID, "RoseEquipHideProperty");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquipHideProperty");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");

        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("附魔属性成功!");

        //更新装备属性
        Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;
    }


    //更新背包
    public void UpdateBag()
    {

        Debug.Log("点击了道具");
        //清空道具列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_BagSpaceListSet);

        //将自身的所有消耗品显示
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++)
        {
            string itemID = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            //Debug.Log("itemID = " + itemID);
            if (itemID != "" && itemID != "0")
            {
                string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
                string itemTypeSon = function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");

                if (itemTypeSon == "" || itemTypeSon == null)
                {
                    itemTypeSon = "0";
                }

                if (itemType == "6" && itemTypeSon == "2"|| itemType == "3")
                {
                    if (int.Parse(itemTypeSon) <= 100)
                    {
                        //开始创建背包格子
                        GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
                        bagSpace.transform.SetParent(Obj_BagSpaceListSet.transform);
                        bagSpace.GetComponent<UI_BagSpace>().BagPosition = i.ToString();
                        bagSpace.GetComponent<UI_BagSpace>().SpaceType = "1";   //设置格子为背包属性   
                        bagSpace.GetComponent<UI_BagSpace>().MoveBagStatus = false;
                        bagSpace.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
            }
        }
    }

    //钻石附魔
    public void Btn_ZuanShiFuMo() {

        if (EquipSpacePosition == "" || EquipSpacePosition == "0" || EquipSpacePosition == null)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先放置装备...");
            return;
        }

        //判断钻石是否足够
        if (Game_PublicClassVar.Get_function_Rose.GetRoseRMB() >= 350)
        {

            bool ifChengGong = Game_PublicClassVar.Get_function_Rose.CostRMB(350);

            if (ifChengGong)
            {
                string randomItemID = "11001001";
                float randValue = Random.value;

                if (randValue <= 0.25f)
                {
                    randomItemID = "11001001";
                }
                if (randValue > 0.25f && randValue <= 0.5f)
                {
                    randomItemID = "11001002";
                }
                if (randValue > 0.5f && randValue <= 0.75f)
                {
                    randomItemID = "11001003";
                }
                if (randValue > 0.75f && randValue <= 1f)
                {
                    randomItemID = "11001004";
                }

                EquipFoMo(randomItemID, EquipSpacePosition, 0.1f, 1f);

                //显示
                ClearnShow();
            }
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足");
        }

    }


    public void Btn_Close()
    {
        Destroy(this.gameObject);
    }
}
