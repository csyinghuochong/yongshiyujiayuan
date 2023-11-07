using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_ZuoQiNengLiSet : MonoBehaviour {


    private ObscuredString nengLiUpCostItemID;
    public GameObject Obj_NengLiIcon;
    public GameObject Obj_NengLiExpShow;
    public GameObject Obj_NengLiExpImgShow;
    public GameObject Obj_NengLiName;
    public GameObject Obj_CostZiJin;
    public GameObject Obj_CostGold;
    private ObscuredInt costZiJinValue;
    private ObscuredInt costGoldValue;
    private ObscuredInt addExpValue;
    public GameObject Obj_BagSpaceListSet;
    public GameObject Obj_BagSpace;
    public GameObject[] Obj_NowNengLiLv;
    public GameObject[] Obj_NengLiProSet;
    public GameObject[] Obj_NengLiPro;
    public GameObject[] Obj_NengLiNextPro;
    public GameObject[] Obj_NengLiProName;
    public GameObject[] Obj_NengLiNextProName;
    public GameObject[] Obj_NowNengLiXuanZhongList;
    public GameObject Obj_NengLiNeedItemHint;

    public ObscuredString BagSpacePosition;     //当前选中的背包格子
    public ObscuredString selectshowType;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //初始化
    public void Init() {

        Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiNengLiSet_Open = this.gameObject;

        //读取当前能力等级进行显示


        //初始化显示第一个
        Btn_ShowNengLiData("1");

        //更新背包显示
        UpdateBag();

    }


    //更新背包
    public void UpdateBag() {

        Debug.Log("点击了道具");
        //清空道具列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_BagSpaceListSet);

        int spaceNum = 0;

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

                if (itemType == "6"&& itemTypeSon=="2")
                {
                    //Debug.Log("开始创建！");
                    //开始创建背包格子
                    GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
                    bagSpace.transform.SetParent(Obj_BagSpaceListSet.transform);
                    bagSpace.GetComponent<UI_BagSpace>().BagPosition = i.ToString();
                    bagSpace.GetComponent<UI_BagSpace>().SpaceType = "1";   //设置格子为背包属性   
                    bagSpace.GetComponent<UI_BagSpace>().MoveBagStatus = false;
                    bagSpace.transform.localScale = new Vector3(1, 1, 1);

                    spaceNum = spaceNum + 1;
                }
            }
        }

        if (spaceNum >= 1)
        {
            Obj_NengLiNeedItemHint.SetActive(false);
        }
        else {
            Obj_NengLiNeedItemHint.SetActive(true);
        }
        
    }

    public void Btn_ShowNengLiData(string showType) {

        selectshowType = showType;

        string nowZuoQiNengLiLvID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + showType, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nowZuoQiNengLiExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiExp_" + showType, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

        string nengLiName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", nowZuoQiNengLiLvID, "ZuoQiNengLi_Template");
        string nengLiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowZuoQiNengLiLvID, "ZuoQiNengLi_Template");
        string nengLiExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Exp", "ID", nowZuoQiNengLiLvID, "ZuoQiNengLi_Template");
        nengLiUpCostItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpCostItemID", "ID", nowZuoQiNengLiLvID, "ZuoQiNengLi_Template");
        //显示Icon要处理
        string nengLiIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", nowZuoQiNengLiLvID, "ZuoQiNengLi_Template");
        //显示道具Icon
        object obj = Resources.Load("OtherIcon/NengLiIcon/" + nengLiIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_NengLiIcon.GetComponent<Image>().sprite = itemIcon;

        //显示经验
        Obj_NengLiExpShow.GetComponent<Text>().text = nowZuoQiNengLiExp + "/" + nengLiExp;
        Obj_NengLiExpImgShow.GetComponent<Image>().fillAmount = float.Parse(nowZuoQiNengLiExp) / float.Parse(nengLiExp);

        //显示名字
        Obj_NengLiName.GetComponent<Text>().text = nengLiName + nengLiLv + "级";

        //显示属性
        string nengLiAddProperty = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddProperty", "ID", nowZuoQiNengLiLvID, "ZuoQiNengLi_Template");
        string nextnengLiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nowZuoQiNengLiLvID, "ZuoQiNengLi_Template");
        if (nextnengLiID == "0"|| nextnengLiID == "") {
            nextnengLiID = nowZuoQiNengLiLvID;
        }
        string nextnengLiAddProperty = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddProperty", "ID", nextnengLiID, "ZuoQiNengLi_Template");
        string[] nengLiAddProList = nengLiAddProperty.Split(';');

        Obj_NengLiProSet[0].SetActive(false);
        Obj_NengLiProSet[1].SetActive(false);

        for (int i = 0; i < nengLiAddProList.Length; i++) {

            //显示当前属性
            Obj_NengLiPro[i].GetComponent<Text>().text = nengLiAddProList[i].Split(',')[1];
            //显示升级属性
            string[] nextnengLiAddList = nextnengLiAddProperty.Split(';');
            string type = nextnengLiAddList[i].Split(',')[0];
            string value = nextnengLiAddList[i].Split(',')[1];

            Obj_NengLiNextPro[i].GetComponent<Text>().text = value;
            Obj_NengLiProSet[i].SetActive(true);

            //显示名字
            string typeName = "aaa";
            if (type == "10") {
                typeName = "血量";
            }
            if (type == "11")
            {
                typeName = "攻击";
            }
            if (type == "14")
            {
                typeName = "法术";
            }
            if (type == "17")
            {
                typeName = "物防";
            }
            if (type == "20")
            {
                typeName = "魔防";
            }

            Obj_NengLiProName[i].GetComponent<Text>().text = typeName;
            Obj_NengLiNextProName[i].GetComponent<Text>().text = typeName;
        }

        //显示资金
        int costZiJinSum = 0;
        int costGoldSum = 0;
        int addExpValueSum = 0;
        for (int i = 1; i <= 4; i++)
        {
            //读取能力等级
            string nengLiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            string nengLiUpCostZiJin = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpCostZiJin", "ID", nengLiID, "ZuoQiNengLi_Template");
            string nengLiUpCostGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpCostGold", "ID", nengLiID, "ZuoQiNengLi_Template");
            string nengLiUpAddExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpAddExp", "ID", nengLiID, "ZuoQiNengLi_Template");
            costZiJinSum = costZiJinSum + int.Parse(nengLiUpCostZiJin);
            costGoldSum = costGoldSum + int.Parse(nengLiUpCostGold);
            addExpValueSum = addExpValueSum + int.Parse(nengLiUpAddExp);

            //显示等级
            string nownengLiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nengLiID, "ZuoQiNengLi_Template");
            Obj_NowNengLiLv[i-1].GetComponent<Text>().text = nownengLiLv + "级";
        }

        costZiJinValue = (int)(costZiJinSum / 4);
        costGoldValue = (int)(costGoldSum / 4);
        Obj_CostZiJin.GetComponent<Text>().text = costZiJinValue.ToString();
        Obj_CostGold.GetComponent<Text>().text = costGoldValue.ToString();

        addExpValue = (int)(addExpValueSum / 4);

        //显示选中框
        for (int i = 0; i < Obj_NowNengLiXuanZhongList.Length; i++) {
            Obj_NowNengLiXuanZhongList[i].SetActive(false);
        }

        Obj_NowNengLiXuanZhongList[int.Parse(selectshowType)-1].SetActive(true);

    }


    //使用道具
    public void Btn_Use() {

        if (selectshowType == "" || selectshowType == null) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先选中要提升的能力类型");
            return;
        }

        if (BagSpacePosition == "" || BagSpacePosition == null) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先选中要使用的道具,如没显示则表示背包内没有对应道具");
            return;
        }

        //能力ID
        string nengLiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + selectshowType, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nengLiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nengLiID, "ZuoQiNengLi_Template");
        if (int.Parse(nengLiLv) >= 120) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你当前选中的能力已达到满级!");
            return;
        }

        //扣除背包道具（指定位置的）
        string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", BagSpacePosition, "RoseBag");
        if (nowItemID == nengLiUpCostItemID|| nowItemID == "11001001" || nowItemID == "11001002" || nowItemID == "11001003" || nowItemID == "11001004") {

            //获取品质系数
            string nowItemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", BagSpacePosition, "RoseBag");

            //销毁道具
            Game_PublicClassVar.Get_function_Rose.CostBagSpaceItem(nowItemID, 1, BagSpacePosition);

            //增加经验
            Game_PublicClassVar.Get_function_Pasture.ZuoQiNengLiAddExp(selectshowType,int.Parse(nowItemPar));

            //更新背包显示
            UpdateBag();
        }

    }


    //随机提升
    public void Btn_RandomUp(string upType) {

        if (upType == "" || upType == "0" || upType == null) {
            return;
        }

        ObscuredInt costValue = 0;
        if(upType == "1")
        {
            costValue = costZiJinValue;
        }

        if (upType == "2")
        {
            costValue = costGoldValue;
        }

        //获取当前家园等级
        string nowPastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string ZuoQiNengLiLvMaxStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiNengLiLvMax", "ID", nowPastureLv, "PastureUpLv_Template");

        int maxLv = int.Parse(ZuoQiNengLiLvMaxStr)* 4;

        int lvSum = 0;
        for (int i = 1; i <= 4; i++) {
            string nowNengLiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            string nowLvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowNengLiLv, "ZuoQiNengLi_Template");
            lvSum = lvSum + int.Parse(nowLvStr);
        }

        if (lvSum >= maxLv) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("坐骑能力总和达到当前上限,请提升家园等级从而提升坐骑能力等级上限,每提升1级家园等级各个能力等级可以提升10个等级");
            return;
        }

        if (costValue <= 0) {
            return;
        }

        //消耗资金
        long nowGold = Game_PublicClassVar.Get_function_Pasture.GetPastureGold();
        string costType = "5";
        if (upType == "2")
        {
            nowGold = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
            costType = "1";
        }

        if (nowGold >= costValue)
        {
            Game_PublicClassVar.Get_function_Rose.CostReward(costType, costValue.ToString());
            string upLvType = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, 4).ToString();

            //判断当前等级是否达到100级
            string nowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + upLvType, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            int nowLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowID, "ZuoQiNengLi_Template"));
            if (nowLv >= 100) {
                //寻找当前最低的
                int lowNengLiLv = 100;
                int lowNengLiID = 0;
                for (int i = 1; i <= 4; i++) {

                    string nowNengLiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                    int nowNengLiLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowNengLiID, "ZuoQiNengLi_Template"));

                    if (nowNengLiLv < lowNengLiLv) {
                        lowNengLiID = i;
                        lowNengLiLv = nowNengLiLv;
                    }
                }

                if(lowNengLiID != 0)
                {
                    upLvType = lowNengLiID.ToString();
                }
            }

            //获取增加经验
            int addValueMin = (int)(addExpValue * 0.8f);
            int addValueMax = (int)(addExpValue * 1.2f);

            int addValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(addValueMin, addValueMax);
            Game_PublicClassVar.Get_function_Pasture.ZuoQiNengLiAddExp(upLvType, addValue);

            //刷新显示
            Btn_ShowNengLiData(upLvType);
        }
        else {

            if (upType == "1")
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("家园资金不足!");
            }

            if (upType == "2")
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("金币不足!");
            }
        }
    }
}
