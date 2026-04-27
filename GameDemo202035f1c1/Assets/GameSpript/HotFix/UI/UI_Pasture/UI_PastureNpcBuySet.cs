using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureNpcBuySet : MonoBehaviour {

    public string[] SpaceListStr;
    public GameObject[] Obj_SpaceShowList;

    public GameObject Obj_PastureTraderShow;
    public GameObject Obj_PastureTraderShowListSet;
    public ObscuredBool ifHaveDingDan;
    public ObscuredString PastureTraderID;
    public GameObject Obj_NpcBuyData;
    public GameObject Obj_ShangRenHeadIcon;
    public GameObject Obj_ShangRenName;
    public GameObject Obj_PastureItemName;
    public GameObject Obj_PastureItemQuality;
    public GameObject Obj_PastureItemNum;
    public GameObject Obj_PastureItemJiaGe;
    public GameObject Obj_PastureItemTime;
    public GameObject Obj_PastureItemIconShow;
    public GameObject Obj_PastureTraderNullHint;

	// Use this for initialization
	void Start () {
        Game_PublicClassVar.Get_game_PositionVar.Obj_PastureNpcBuySet_Open = this.gameObject;
        Init();
    }


	
	// Update is called once per frame
	void Update () {
		
	}

    //初始化显示
    public void Init() {

        ShowNpcNeedPastrueItemList();

        PastureTraderID = "0";
        string nowPastureTraderID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureTraderID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowPastureTraderID != "" && nowPastureTraderID != "0" && nowPastureTraderID != null)
        {
            ShowNpcBuyData();
            Obj_PastureTraderNullHint.SetActive(false);
        }
        else {
            Obj_PastureTraderNullHint.SetActive(true);
            Obj_NpcBuyData.SetActive(false);
            ifHaveDingDan = false;
        }
    }

    public void InitShow() {

        for (int i = 0; i < SpaceListStr.Length; i++) {
            if (SpaceListStr[i] != null && SpaceListStr[i] != "" && SpaceListStr[i] != "0")
            {
                Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().BagPosition = SpaceListStr[i];
                Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().UpdateItem();
            }
            else {
                Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().UpdateItem();
            }
        }
    }

    public bool PutSpaceID(string spaceID) {

        if (ifHaveDingDan == false) {
            return false;
        }
        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            if (SpaceListStr[i] == spaceID)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("此道具已经放置!");
                return false;
            }
        }
            


        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            if (SpaceListStr[i] == "0" || SpaceListStr[i] == "")
            {

                //判断当前剩余的数量
                int shengXiaNum = Game_PublicClassVar.Get_function_Pasture.GetPastureTraderNeedItemNum(int.Parse(PastureTraderID));
                int putNum = GetPutNum();
                if (putNum >= shengXiaNum) {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你提交的数量已经满足商人的需求!");
                    return false;
                }

                //判定是否符合放入要求
                bool ifPut = Game_PublicClassVar.Get_function_Pasture.IfPastureTraderItem(int.Parse(PastureTraderID), int.Parse(spaceID));
                SpaceListStr[i] = spaceID;
                Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().BagPosition = spaceID;
                Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().UpdateItem();
                return true;
            }
            else {
                if (SpaceListStr[i] == spaceID) {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经放置了此道具,请务重复放置");
                    return false;
                }
            }
        }

        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("放置栏位已满!");
        return false;

    }

    public void CancleSpaceID(string spaceID)
    {
        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            if (SpaceListStr[i] == spaceID)
            {
                SpaceListStr[i] = "0";
                Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().BagPosition = "0";
                Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().UpdateItem();
            }
        }
    }


    //展示NPC列表
    public void ShowNpcNeedPastrueItemList() {

        //清理显示
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PastureTraderShowListSet);

        //显示列表
        string nowPastureTraderID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureTraderID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] nowPastureTraderIDList = nowPastureTraderID.Split(';');

        for (int i = 0; i < nowPastureTraderIDList.Length; i++) {
            if (nowPastureTraderIDList[i].Split(',').Length >= 8) {
                GameObject obj = (GameObject)Instantiate(Obj_PastureTraderShow);
                obj.transform.SetParent(Obj_PastureTraderShowListSet.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.GetComponent<UI_PastureTraderShow>().PastureTraderID = i.ToString();
                obj.GetComponent<UI_PastureTraderShow>().Init();
            }
        }
    }


    public int GetPutNum() {
        int num = 0;
        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            if (SpaceListStr[i] != "0"&& SpaceListStr[i] != ""&& SpaceListStr[i] != null) {
                num = num + 1;
            }
        }
        return num;
    }

    //清理显示
    public void ClearnShow() {

        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            SpaceListStr[i] = "0";
            Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().BagPosition = "0";
            Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().UpdateItem();
        }

    }

    //清理牧场仓库道具
    public void CostPastureBagItem()
    {
        //循环删除道具
        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            Game_PublicClassVar.Get_function_Pasture.CostPastureBagSpaceNumItem(Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().ItemID,1, SpaceListStr[i],false);
        }

        ClearnShow();
        Game_PublicClassVar.Get_game_PositionVar.UpdatePastureBagAll = true;
    }

    //更新游戏
    public void ShowNpcBuyData() {

        string[] pastureTraderList = Game_PublicClassVar.Get_function_Pasture.GetPastureTraderIDData(int.Parse(PastureTraderID));
        if (pastureTraderList == null)
        {
            Obj_NpcBuyData.SetActive(false);
            Debug.Log("读取数据错误");
            return;
        }


        //判断当前是否有订单
        if (pastureTraderList.Length>=1) {
            Obj_NpcBuyData.SetActive(true);
            ifHaveDingDan = true;
        }
        else {
            Obj_NpcBuyData.SetActive(false);
            return;
        }

        /*
        string npcBuyName = "来自乌尔的商人";
        string pastureItemID = "11000001";
        string npcNeedItemQuality = "60";
        string npcNeedItemNum = "10";
        string npcNeedItemJiaGe = "10050";
        string npcNeedItemTime = "99999";
        */

        //头像ID,Npc名称,需要道具,需要品质,自己完成数量,要求的数量,剩余时间,收购价格
        string npcHeadIconID = pastureTraderList[0];
        string npcBuyName = pastureTraderList[1];
        string pastureItemID = pastureTraderList[2];
        string npcNeedItemQuality = pastureTraderList[3];
        string npcNeedItemNum = pastureTraderList[4];
        string npcNeedItemNeedNum = pastureTraderList[5];
        string npcNeedItemJiaGe = pastureTraderList[7];
        string npcNeedItemTime = pastureTraderList[6];

        object obj = Resources.Load("HeadIcon/PastureTraderIcon/" + npcHeadIconID, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ShangRenHeadIcon.GetComponent<Image>().sprite = itemIcon;
        Obj_ShangRenName.GetComponent<Text>().text = npcBuyName;
        string ItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", pastureItemID, "Item_Template");
        Obj_PastureItemName.GetComponent<Text>().text = ItemName;
        Obj_PastureItemQuality.GetComponent<Text>().text = npcNeedItemQuality + "以上";
        Obj_PastureItemNum.GetComponent<Text>().text = npcNeedItemNum + "/" + npcNeedItemNeedNum;
        Obj_PastureItemJiaGe.GetComponent<Text>().text = npcNeedItemJiaGe + "/1个";

        int minSum = (int)(float.Parse(npcNeedItemTime) / 60);
        int hour = (int)(minSum / 60);
        int min = minSum - (60 * hour);
        Obj_PastureItemTime.GetComponent<Text>().text = hour + "小时" + min + "分钟";

        //显示Icon
        string itemIconStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", pastureItemID, "Item_Template");
        obj = Resources.Load("ItemIcon/" + itemIconStr, typeof(Sprite));
        itemIcon = obj as Sprite;
        Obj_PastureItemIconShow.GetComponent<Image>().sprite = itemIcon;
        

    }


    //出售按钮
    public void Btn_Sell() {

        bool ifSellStatus = false;
        int sellNum = 0;
        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            if (SpaceListStr[i] != "0" && SpaceListStr[i] != "" && SpaceListStr[i] != null) {
                ifSellStatus = true;
                sellNum = sellNum + 1;
            }
        }

        if (ifSellStatus == false) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请放入要出售的道具");
            return;
        }

        //判定放入商品是否符合要求
        bool ifFuHe = Game_PublicClassVar.Get_function_Pasture.IfCompletePastureTrader(PastureTraderID, SpaceListStr);
        if (ifFuHe == false)
        {
            return;
        }
        else {
            //写入成就
            Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("229", "0", "1");
            //写入活跃任务
            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "137", "1");
        }

        if (ifSellStatus)
        {
            //清理背包道具
            CostPastureBagItem();
            bool ifStatus = Game_PublicClassVar.Get_function_Pasture.CompletePastureTrader(PastureTraderID, sellNum);
            //刷新任务显示
            if (ifStatus)
            {
                //重新刷新列表显示
                Init();
            }
            else
            {
                //刷新此任务数量
                ShowNpcBuyData();
            }

            //更新背包
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        }
   
    }

    //放弃订单
    public void Btn_FangQiDingDan() {
        Game_PublicClassVar.Get_function_Pasture.CanclePastureTrader(PastureTraderID);
        //重新刷新列表显示
        Init();
    }

    public void Btn_Close() {
        //Destroy(this.gameObject);
    }
}
