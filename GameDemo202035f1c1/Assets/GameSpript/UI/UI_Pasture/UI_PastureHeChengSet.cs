using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_PastureHeChengSet : MonoBehaviour {

    public string[] SpaceListStr;
    public GameObject[] Obj_SpaceShowList;

	// Use this for initialization
	void Start () {
        Game_PublicClassVar.Get_game_PositionVar.Obj_PastureHeChengSet_Open = this.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
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

        string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RosePastureBag");
        string nowItemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", nowItemID, "Item_Template");
        string nowItemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", nowItemID, "Item_Template");
        if (nowItemType != "6" && nowItemSubType != "1") {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("放入道具不符,请放入可以合成的原材料");
            return false;
        }

        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            if (SpaceListStr[i] == "0" || SpaceListStr[i] == "")
            {
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


    //清理显示
    public void ClearnShow()
    {

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
            Game_PublicClassVar.Get_function_Pasture.CostPastureBagSpaceNumItem(Obj_SpaceShowList[i].GetComponent<UI_Common_PastureItemIcon>().ItemID, 1, SpaceListStr[i], false);
        }

        ClearnShow();

        Game_PublicClassVar.Get_game_PositionVar.UpdatePastureBagAll = true;

    }

    //合成按钮
    public void Btn_HeCheng() {

        //判断背包格子有位置
        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() <= 0) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("角色背包已满!");
            return;
        }
        //判断农场格子有位置
        if (Game_PublicClassVar.Get_function_Pasture.ReturnNullSpaceNum() == "-1")
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("家园仓库已满!");
            return;
        }



        //获取道具
        int hechengNum = 0;
        int nowItemParSum = 0;          //品质总和
        int nowItemUseParSum = 0;       //参数总和
        int nownowItemLvSum = 0;        //等级总和
        bool ifXiangTong = false;

        string lastItemID = "";

        List<int> pinZhi = new List<int>();

        for (int i = 0; i < SpaceListStr.Length; i++)
        {
            if (SpaceListStr[i] !=""&& SpaceListStr[i] != "0"&& SpaceListStr[i] != null)
            {
                string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", SpaceListStr[i], "RosePastureBag");
                if (nowItemID != "" && nowItemID != "0" && nowItemID != null) {
                    string nowItemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", SpaceListStr[i], "RosePastureBag");
                    string nowItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", nowItemID, "Item_Template");
                    string nowItemLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", SpaceListStr[i], "RosePastureBag");
                    nowItemParSum = nowItemParSum + int.Parse(nowItemPar);
                    nowItemUseParSum = nowItemUseParSum + int.Parse(nowItemUsePar);
                    nownowItemLvSum = nownowItemLvSum + int.Parse(nowItemLv);

                    if (nowItemID == lastItemID)
                    {
                        ifXiangTong = true;
                    }

                    if (ifXiangTong)
                    {
                        if (nowItemID != lastItemID)
                        {
                            ifXiangTong = false;
                        }
                    }

                    if (lastItemID == "")
                    {
                        lastItemID = nowItemID;
                    }

                    pinZhi.Add(int.Parse(nowItemPar));

                    hechengNum = hechengNum + 1;
                }

            }
        }

        //获取当前品质最小和最大
        int pinZhi_Min = pinZhi.Min();
        int pinZhi_Max = pinZhi.Max();

        if (hechengNum <= 1) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("合成至少需要2个材料");
            return;
        }

        //清理道具删除道具
        CostPastureBagItem();

        //写入活跃任务
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "136", "1");

        //随机道具权重
        int guoshiPro_1 = 25;       
        int guoshiPro_2 = 50;
        int guoshiPro_3 = 75;
        int guoshiPro_4 = 100;

        int qualityValue = 0;
        //判断是否是一样的道具
        float ranPro = 0.9f;
        string sendItemID = "0";
        if (ifXiangTong)
        {

            //2个道具相同 10%概率才会出果实   3个道具相同20%概率出果实
            if (hechengNum <= 2)
            {
                ranPro = 0.9f;
            }
            else
            {
                ranPro = 0.8f;
            }
            if (Random.value <= ranPro)
            {
                sendItemID = lastItemID;

                //qualityValue = Game_PublicClassVar.Get_function_Pasture.GetPastureItemQuality((int)((float)nowItemParSum / (float)hechengNum));
                qualityValue = Game_PublicClassVar.Get_function_Pasture.GetPastureItemQualityHeCheng(pinZhi_Min, pinZhi_Max, hechengNum);
                //获取品质上限
                /*
                string nowItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", sendItemID, "Item_Template");
                if (qualityValue >= int.Parse(nowItemUsePar)) {
                    qualityValue = int.Parse(nowItemUsePar);
                }
                */
                Game_PublicClassVar.Get_function_Pasture.SendPastureBag(sendItemID, 1, "0", qualityValue);
                return;
            }
        }
        else {
            //2个道具不相同 20%概率才会出果实   3个道具相同30%概率出果实
            ranPro = 0.8f;
            if (hechengNum <= 2)
            {
                ranPro = 0.8f;
            }
            else
            {
                ranPro = 0.7f;
            }

            if (Random.value <= ranPro)
            {
                int pingjunLv = (int)((float)nownowItemLvSum / (float)hechengNum);
                if (pingjunLv <= 0)
                {
                    pingjunLv = 1;
                }
                if (pingjunLv >= 11)
                {
                    pingjunLv = 10;
                }

                string randomSendListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "PastureHeChengLv_" + pingjunLv, "GameMainValue");
                string[] randomSendList = randomSendListStr.Split(';');
                int randValueInt = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, randomSendList.Length - 1);
                //qualityValue = Game_PublicClassVar.Get_function_Pasture.GetPastureItemQuality((int)((float)nowItemParSum / (float)hechengNum));
                qualityValue = Game_PublicClassVar.Get_function_Pasture.GetPastureItemQualityHeCheng(pinZhi_Min, pinZhi_Max, hechengNum);
                //获取品质上限
                /*
                string nowItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", sendItemID, "Item_Template");
                if (qualityValue >= int.Parse(nowItemUsePar))
                {
                    qualityValue = int.Parse(nowItemUsePar);
                }
                */
                Game_PublicClassVar.Get_function_Pasture.SendPastureBag(randomSendList[randValueInt], 1, "0", qualityValue);
                return;
            }
        }


        //根据参数生成权重(根据尾数生成结果)
        int ranValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 100);
        string nowItemUseParSumStr = nowItemUseParSum.ToString();
        if (nowItemUseParSumStr.Length <= 1) {
            nowItemUseParSumStr = "0" + nowItemUseParSumStr;
        }
        string lastNumStr = nowItemUseParSumStr.Substring(nowItemUseParSumStr.Length-1, 1);
        if (lastNumStr == "1" || lastNumStr == "5")
        {
            guoshiPro_1 = 20;
            guoshiPro_2 = 40;
            guoshiPro_3 = 60;
            guoshiPro_4 = 100;
        }

        if (lastNumStr == "2" || lastNumStr == "6")
        {
            guoshiPro_1 = 20;
            guoshiPro_2 = 40;
            guoshiPro_3 = 80;
            guoshiPro_4 = 100;
        }

        if (lastNumStr == "3" || lastNumStr == "7")
        {
            guoshiPro_1 = 20;
            guoshiPro_2 = 60;
            guoshiPro_3 = 80;
            guoshiPro_4 = 100;
        }

        if (lastNumStr == "4" || lastNumStr == "8")
        {
            guoshiPro_1 = 40;
            guoshiPro_2 = 60;
            guoshiPro_3 = 80;
            guoshiPro_4 = 100;
        }


        //根据结果判定
        float randomValue = Random.value;
        bool randStatus = true;
        if (randomValue <= 0.8f)
        {
            sendItemID = "11001001";
            ranValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 100);

            if (ranValue < guoshiPro_1 && randStatus == true)
            {
                sendItemID = "11001001";
                randStatus = false;
            }

            if (ranValue < guoshiPro_2 && randStatus == true)
            {
                sendItemID = "11001002";
                randStatus = false;
            }

            if (ranValue < guoshiPro_3 && randStatus == true)
            {
                sendItemID = "11001003";
                randStatus = false;
            }

            if (ranValue < guoshiPro_4 && randStatus == true)
            {
                sendItemID = "11001004";
                randStatus = false;
            }

        }

        if (randomValue <= 0.9f && randStatus == true)
        {
            sendItemID = "10000051";
            randStatus = false;
        }

        if (randomValue <= 1f && randStatus == true)
        {
            sendItemID = "10000052";
            randStatus = false;
        }

        //qualityValue = Game_PublicClassVar.Get_function_Pasture.GetPastureItemQuality((int)((float)nowItemParSum / (float)hechengNum), nownowItemLvSum);
        qualityValue = Game_PublicClassVar.Get_function_Pasture.GetPastureItemQualityHeCheng(pinZhi_Min, pinZhi_Max, hechengNum);
        Game_PublicClassVar.Get_function_Rose.SendRewardToBag(sendItemID, 1, "0", 0, "0", true, "0", qualityValue.ToString());
        


    }

    public void Btn_Close() {
        Destroy(this.gameObject);
    }
}
