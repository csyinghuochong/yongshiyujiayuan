using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GiveNPC : MonoBehaviour
{

    public GameObject Obj_EquipItem;            //放入的装备图标
    public GameObject Obj_EquipQuality;         //放入的装备图标
    public GameObject Obj_EquipXiLianGold;      //金币洗练消耗金币
    public GameObject Obj_GiveDes;              //给与描述
    public bool UpdateXiLianItemStatus;
    public ArrayList taskIDList;                //任务ID
    public string bagSpaceNum;                  //背包格子
    public string moveItemID;                   //移动道具ID
    public string xiLianItemID;                 //洗练道具ID
    private int xiLianNeedGold;                 //洗练金币
    private string[] xiLianNeedItem;            //洗练需要道具
    private GameObject obj_ItemTips;            //道具Tips

    public GameObject Obj_BagSpaceListSet;      //背包格子
    public GameObject Obj_BagSpace;             //动态创建的背包格子
    // Use this for initialization
    void Start () {
	    
        //默认打开背包
        GameObject functionOpen = GameObject.FindWithTag("UI_FunctionOpen");
        Obj_EquipXiLianGold.GetComponent<Text>().text = "消耗金币：0";

        Game_PublicClassVar.Get_game_PositionVar.NPCGiveStatus = true;      //打开装备给与状态
        XiLian_Item();  //默认显示道具洗练

        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_GiveNPC = this.gameObject;

        //显示任务描述
        foreach (string taskID in taskIDList)
        {
            string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", taskID, "Task_Template");
            if(targetType=="11"|| targetType == "12"|| targetType == "13")
            {
                string taskDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskDes", "ID", taskID, "Task_Template");
                Obj_GiveDes.GetComponent<Text>().text = taskDes;
                break;
            }
        }

        //显示背包
        ShowBagItem();

    }
	
	// Update is called once per frame
	void Update () {
        //触发移动注销此界面
        /*
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus != "1")
        {
            Destroy(this.gameObject);
            //关闭背包
            if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status == true)
            {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenBag();
            }
        }
        */
        if (UpdateXiLianItemStatus)
        {
            UpdateXiLianItemStatus = false;
        }
	}

    //被销毁时调用
    void OnDisable()
    {
        Game_PublicClassVar.Get_game_PositionVar.NPCGiveStatus = false;      //关闭装备洗练状态
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_GiveNPC = null;
    }

    public void MouseEnter() {

        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "1") {
            UpdateXiLianItem();
        }
    }

    public void UpdateXiLianItem() {

        moveItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
        if (moveItemID != "")
        {
            //判定ID是否为装备
            if (moveItemID[0] == '1')
            {
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItemID, "Item_Template");
                if (itemType == "3")
                {

                    //显示道具Icon
                    string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", moveItemID, "Item_Template");
                    object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
                    Sprite itemIcon = obj as Sprite;
                    Obj_EquipItem.GetComponent<Image>().sprite = itemIcon;

                    //显示品质
                    string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", moveItemID, "Item_Template");
                    object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
                    Sprite itemQuality = obj2 as Sprite;
                    Obj_EquipQuality.GetComponent<Image>().sprite = itemQuality;

                    //显示洗练金币
                    string xiLianMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianMoney", "ID", moveItemID, "Item_Template");
                    //Debug.Log("xiLianMoney = " + xiLianMoney + ";moveItemID = " + moveItemID);
                    xiLianNeedGold = int.Parse(xiLianMoney);
                    Obj_EquipXiLianGold.GetComponent<Text>().text = "消耗金币：" + xiLianNeedGold;

                }
            }
        }
    }

    public void Btn_GiveNPCItem()
    {
        //不显示Tips
        if (obj_ItemTips != null)
        {
            Destroy(obj_ItemTips);
        }


        if (bagSpaceNum != "" && bagSpaceNum != "0")
        {
            xiLianItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
            string xilianHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagSpaceNum, "RoseBag");
            if (xiLianItemID != "")
            {
                //判定ID是否为装备
                if (xiLianItemID[0] == '1')
                {
                    string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", xiLianItemID, "Item_Template");
                    if (itemType == "3")
                    {
                        if (xiLianItemID == moveItemID)
                        {
                            //检测放入装备
                            bool ifGiveNpcStatus = false;
                            foreach (string taskID in taskIDList)
                            {
                                ifGiveNpcStatus = Game_PublicClassVar.Get_function_Task.TaskEquipNum(bagSpaceNum, 1, taskID);
                            }
                            if (ifGiveNpcStatus)
                            {
                                Btn_Close();
                            }
                        }
                    }
                }
            }
        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_326");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("请放入将要给予的装备");
        }
        return;
    }

    //显示装备ID
    public void Btn_Click() {
        if (obj_ItemTips == null)
        {
            if (moveItemID != "" && moveItemID != "0")
            {
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItemID, "Item_Template");
                obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(moveItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
                //获取目标是否是装备
                if (itemType == "3")
                {
                    obj_ItemTips.GetComponent<UI_EquipTips>().UIBagSpaceNum = bagSpaceNum;
                    obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "1";
                    //获取极品属性
                    string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagSpaceNum, "RoseBag");
                    obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;
                    //获取宝石属性
                    string itemGemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", bagSpaceNum, "RoseBag");
                    string itemGemHole = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", bagSpaceNum, "RoseBag");
                    obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemID = itemGemID;
                    obj_ItemTips.GetComponent<UI_EquipTips>().ItemGemHole = itemGemHole;
                }
            }
        }
        else {
            Destroy(obj_ItemTips);
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
        }
    }

    /*
    //点击洗练金币
    public void XiLian_Gold() {
        Obj_EquipXiLianBtn_Gold.SetActive(true);
        Obj_EquipXiLianBtn_Item.SetActive(false);
        Obj_EquipXiLianGold.SetActive(true);
        Obj_EquipXiLianItem.SetActive(false);
    }
    */

    //点击洗练道具
    public void XiLian_Item()
    {
        Obj_EquipXiLianGold.SetActive(false);
    }

    public void Btn_Close() {
        Destroy(obj_ItemTips);
        Destroy(this.gameObject);
    }

    public void ShowBagItem() {
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
                if (ifItemType(itemID)) {
                    //开始创建背包格子
                    GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
                    bagSpace.transform.SetParent(Obj_BagSpaceListSet.transform);
                    bagSpace.GetComponent<UI_BagSpace>().BagPosition = i.ToString();
                    bagSpace.GetComponent<UI_BagSpace>().SpaceType = "1";   //设置格子为背包属性   
                    bagSpace.GetComponent<UI_BagSpace>().MoveBagStatus = true;
                    bagSpace.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }

    public bool ifItemType(string itemID) {

        bool returnValue = false;

        foreach (string taskID in taskIDList)
        {
            //Debug.Log("taskID = " + taskID);
            string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", taskID, "Task_Template");
            //给与道具
            if (targetType == "2") {
                string target1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", taskID, "Task_Template");
                string needItemType = "";
                if (target1 != "")
                {
                    needItemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", target1, "Item_Template");
                }
                Debug.Log("target1 = " + target1 + ";needItemType = " + needItemType);
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");

                if (itemType == needItemType)
                {
                    returnValue = true;
                }
            }

            //给与装备
            if (targetType == "11"|| targetType=="12"|| targetType=="13")
            {
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
                if (itemType == "3")
                {
                    returnValue = true;
                }
            }
        }

        return returnValue;
    }
}
