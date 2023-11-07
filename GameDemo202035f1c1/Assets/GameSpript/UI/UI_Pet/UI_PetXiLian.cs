using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PetXiLian : MonoBehaviour {

    //宠物洗炼相关
    public GameObject Obj_BagSpace;                 //动态创建的背包格子
    public GameObject Obj_BagSpaceListSet;
    public GameObject Obj_PetShow;

    public ObscuredString XiLianType;                   //1.技能打技能书  2.金柳露洗炼  3.超级金柳露洗练   4.提高宠物资质   5.提高宠物成长    6.宠物宝宝清洗点数
    public ObscuredString XiLianNeedItemID;
    public ObscuredString XiLianNeedItemNum;
    public GameObject Obj_XiLianNeedItem;
    public GameObject Obj_XiLianNeedNull;
    public ObscuredBool XiLianNeedItemStatus;
    private ObscuredInt nowXiLianNum;

    // Use this for initialization
    void Start () {
        //显示宠物的信息
        Obj_PetShow.GetComponent<UI_PetXiLianShow>().PetID = "1";
        //显示背包道具
        BagItemSkillBtnList();
        Game_PublicClassVar.Get_game_PositionVar.PetXiLianStatus = true;
        Obj_XiLianNeedNull.SetActive(true);
        Obj_XiLianNeedItem.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(XiLianNeedItemStatus){
            Obj_XiLianNeedNull.SetActive(false);
            Obj_XiLianNeedItem.SetActive(true);
            XiLianNeedItemStatus = false;
            Obj_XiLianNeedItem.GetComponent<UI_Common_ItemIcon_2>().ItemID = XiLianNeedItemID;
            Obj_XiLianNeedItem.GetComponent<UI_Common_ItemIcon_2>().NeedItemNum = int.Parse(XiLianNeedItemNum);
            Obj_XiLianNeedItem.GetComponent<UI_Common_ItemIcon_2>().ItemUpdateStatus = true;
            Obj_XiLianNeedItem.GetComponent<UI_Common_ItemIcon_2>().ItemShowType = "2";
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

				if (itemType == "1"||itemType == "5")
                {
                    string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
					if (itemType == "5"|| itemSubType == "22" || itemSubType == "23" || itemSubType == "27" || itemSubType == "28" || itemSubType == "36")
                    {
                        //判定是否有技能
                        //string itemSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", itemID, "Item_Template");
                        //if (itemSkillID != "0")
                        //{
                            //开始创建背包格子
                            GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
                            bagSpace.transform.SetParent(Obj_BagSpaceListSet.transform);
                            bagSpace.GetComponent<UI_BagSpace>().BagPosition = i.ToString();
                            bagSpace.GetComponent<UI_BagSpace>().SpaceType = "1";   //设置格子为背包属性
                            bagSpace.transform.localScale = new Vector3(1, 1, 1);
                            bagSpace.GetComponent<UI_BagSpace>().MoveBagStatus = true;
                        //}
                    }
                }

                //宠物技能
                if (itemType == "5")
                {
                    //判定是否有技能
                    string itemSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", itemID, "Item_Template");
                    if (itemSkillID != "0")
                    {
                        //开始创建背包格子
                        GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
                        bagSpace.transform.SetParent(Obj_BagSpaceListSet.transform);
                        bagSpace.GetComponent<UI_BagSpace>().BagPosition = i.ToString();
                        bagSpace.GetComponent<UI_BagSpace>().SpaceType = "1";   //设置格子为背包属性
                        bagSpace.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
            }
        }
    }

    //洗炼按钮
    public void Btn_XiLian() {


        //检测服务器网络
        if (Game_PublicClassVar.gameLinkServer.ServerLinkStatus == false)
        {
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_58");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1);
            return;
        }

        //Debug.Log("点击洗炼按钮  XiLianType = " + XiLianType);

        //检测道具
        if (XiLianNeedItemID != "" && XiLianNeedItemID != "0" && XiLianNeedItemID != null)
        {
            if (Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(XiLianNeedItemID) <= 0)
            {

                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_222");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("道具不足!");
                return;
            }
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("道具错误");
            return;
        }

        string petSpaceID = Obj_PetShow.GetComponent<UI_PetXiLianShow>().PetID;
        string nowPetID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", petSpaceID, "RosePet");
        if (nowPetID == "0"|| nowPetID == "")
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_449");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            return;
        }

        //神兽不支持洗炼
        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", petSpaceID, "RosePet");
        string petType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petID, "Pet_Template");
        if (petType == "2" && XiLianType != "6") {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("神兽宠物拥有神圣抗体,无法对其进行洗炼!");
            return;
        }

        switch (XiLianType) { 
            
            //宠物打技能书
            case "1":

                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_223");
                string addSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", XiLianNeedItemID, "Item_Template");
                if (Game_PublicClassVar.Get_function_AI.Pet_AddSkill(petSpaceID, addSkillID))
                {
                    //消耗当前的道具,刷新对应的栏位显示
                    if (Game_PublicClassVar.Get_function_Rose.CostBagItem(XiLianNeedItemID, int.Parse(XiLianNeedItemNum)))
                    {
                        BagItemSkillBtnList();
                    }
                    
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你的技能发挥了作用！");

                    Obj_PetShow.GetComponent<UI_PetXiLianShow>().showPetProperty();
					
                    //写入成就(宠物打书)
                    Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("210", "0", "1");
                    nowXiLianNum = nowXiLianNum + 1;
                }
                else {
                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_224");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你的宠物已经有了相同的技能！");
                }
                break;

            //宠物自身洗炼
            case "2":
                //string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", petSpaceID, "RosePet");

                //string petType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petID, "Pet_Template");

                if (petType != "0")
                {
                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_225");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("洗炼变异宠物请使用更强大的道具！");
                    return;
                }

                if (petID != "30000002") {
                    //3%概率变成变异宠物
                    ObscuredFloat bianyiPro = 0.032f;
                    if (Random.value <= bianyiPro)
                    {
                        string petBianYiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetBianYiID", "ID", petID, "Pet_Template");
                        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", petID, "Pet_Template");

                        if (petBianYiID != "" && petID != "0")
                        {
                            petID = petBianYiID;
                        }

                        //获取玩家名称
                        string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "恭喜玩家" + roseName + "洗炼宠物时一不小心打翻了药坛子,宠物不小心变了一个颜色!");
                        Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
                        comStr_4.str_1 = "2";
                        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010191, comStr_4);
                    }
                }

                nowXiLianNum = nowXiLianNum + 1;

                Game_PublicClassVar.Get_function_AI.Pet_Create(petSpaceID, petID,"1");

                langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_226");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你的宠物被一股强大的能力赋予了！");
                Obj_PetShow.GetComponent<UI_PetXiLianShow>().showPetProperty();
                //消耗当前的道具,刷新对应的栏位显示
                if (Game_PublicClassVar.Get_function_Rose.CostBagItem(XiLianNeedItemID, int.Parse(XiLianNeedItemNum)))
                {
                    BagItemSkillBtnList();
                    nowXiLianNum = nowXiLianNum + 1;
                }
                break;

            //超级金柳露洗炼
            case "3":
                petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", petSpaceID, "RosePet");
                Game_PublicClassVar.Get_function_AI.Pet_Create(petSpaceID, petID,"1");

                langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_226");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你的宠物被一股强大的能力赋予了！");
                Obj_PetShow.GetComponent<UI_PetXiLianShow>().showPetProperty();
                //消耗当前的道具,刷新对应的栏位显示
                if (Game_PublicClassVar.Get_function_Rose.CostBagItem(XiLianNeedItemID, int.Parse(XiLianNeedItemNum)))
                {
                    BagItemSkillBtnList();
                    nowXiLianNum = nowXiLianNum + 1;
                }
                break;

            //资质提高
            case "4":
                petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", petSpaceID, "RosePet");
                if (Game_PublicClassVar.Get_function_AI.Pet_AddRandomZiZhi(petSpaceID)) {

                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_227");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("一股神秘的力量让你的宠物资质提高了！");
                    Obj_PetShow.GetComponent<UI_PetXiLianShow>().showPetProperty();
                    //消耗当前的道具,刷新对应的栏位显示
                    if (Game_PublicClassVar.Get_function_Rose.CostBagItem(XiLianNeedItemID, int.Parse(XiLianNeedItemNum)))
                    {
                        BagItemSkillBtnList();
                        nowXiLianNum = nowXiLianNum + 1;
                    }
                }
                break;

            //成长提高
            case "5":
                petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", petSpaceID, "RosePet");
                if (Game_PublicClassVar.Get_function_AI.Pet_AddRandomChengZhang(petSpaceID))
                {

                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_228");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("一股神秘的力量让你的宠物成长提高了！");
                    Obj_PetShow.GetComponent<UI_PetXiLianShow>().showPetProperty();
                    //消耗当前的道具,刷新对应的栏位显示
                    if (Game_PublicClassVar.Get_function_Rose.CostBagItem(XiLianNeedItemID, int.Parse(XiLianNeedItemNum)))
                    {
                        BagItemSkillBtnList();
                        nowXiLianNum = nowXiLianNum + 1;
                    }
                }
                break;

            //宝宝属性点数重置
            case "6":
                
                //判定目标是否为宝宝
                string ifBaby = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", petSpaceID, "RosePet");
                if (ifBaby == "0") {

                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_229");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("不能给野生宠物使用此道具！");
                    break;
                }

                //读取宠物技能点数
                string addPropretyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyValue", "ID", petSpaceID, "RosePet");
                string addPropretyNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyNum", "ID", petSpaceID, "RosePet");
                string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", petSpaceID, "RosePet");
                string[] addPropretyValueList = addPropretyValue.Split(';');
                int nowNum = 0;
                for (int i = 0; i < addPropretyValueList.Length; i++) {
                    nowNum = nowNum + int.Parse(addPropretyValueList[i]);
                }

                int nowChongZhiNumOne = 15 + (int.Parse(petLv) - 1) * 1;

                if (nowNum >= nowChongZhiNumOne * 4)
                {
                    nowNum = nowNum - nowChongZhiNumOne * 4;
                }
                else {
                    //宠物属性使用失败,当前加点总数必须大于一定值。
                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_230");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("重置失败,点数不符！");
                    break;
                }

                //消耗当前的道具,刷新对应的栏位显示
                if (Game_PublicClassVar.Get_function_Rose.CostBagItem(XiLianNeedItemID, int.Parse(XiLianNeedItemNum)))
                {
                    nowNum = nowNum + int.Parse(addPropretyNum);

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyValue", nowChongZhiNumOne +";"+ nowChongZhiNumOne + ";"+ nowChongZhiNumOne + ";"+ nowChongZhiNumOne, "ID", petSpaceID, "RosePet");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyNum", nowNum.ToString(),"ID", petSpaceID, "RosePet");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_231");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("一股神秘的力量让你的宠物属性点数重置了！");
                    nowXiLianNum = nowXiLianNum + 1;
                }

                break;
        }

        //每洗炼10次 收集一次数据
        if (nowXiLianNum >= 10)
        {
            nowXiLianNum = 0;
            string[] saveList = new string[] { "", "2", "预留设备号位置","3" };
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001006, saveList);
        }

        if (Game_PublicClassVar.Get_function_AI.Pet_ReturnRoseListFirst() == petSpaceID) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_38");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

        }

    }
}
