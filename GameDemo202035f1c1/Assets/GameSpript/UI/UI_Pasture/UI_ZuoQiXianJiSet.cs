using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public class UI_ZuoQiXianJiSet : MonoBehaviour {

    public string[] SpaceListStr;
    public GameObject[] Obj_SpaceShowList;

    public GameObject Obj_XianJiExpShowText;
    public GameObject Obj_XianJiExpShowPro;
    private ObscuredInt maxXianJiValue;
    public ObscuredInt DayXianJiMaxNum;
    public GameObject Obj_XianJiNumShow;
    public bool CloseXianJiStatus;
    //public GameObject Obj_XianJiGoldNum;
    //private ObscuredInt xianJiCostGold;
    // Use this for initialization
    void Start () {

        //判定如果当前没有坐骑则自动创建一个坐骑数据
        //Game_PublicClassVar.Get_function_Pasture.CreateZuoQi();
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ZuoQiModelShow.SetActive(true);

        Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiJiHuoSet_Open = this.gameObject;
        DayXianJiMaxNum = 10;
        InitShow();
    }
	
	// Update is called once per frame
	void Update () {

        if (CloseXianJiStatus) {
            CloseXianJiStatus = false;
            //Debug.LogError("激活坐骑成功");
            Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiSetNow.GetComponent<UI_ZuoQiSet>().Btn_OpenXianJi();
            //Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiSet.GetComponent<UI_ZuoQiSet>().CloseUI();
        }

	}

    private void OnDestroy()
    {
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ZuoQiModelShow.SetActive(false);
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

        //显示进度值
        string nowXianJiExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiXianJiExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowXianJiExp == "" || nowXianJiExp == null) {
            nowXianJiExp = "0";
        }
        maxXianJiValue = 1000;
        Obj_XianJiExpShowText.GetComponent<Text>().text = nowXianJiExp + "/" + maxXianJiValue;
        Obj_XianJiExpShowPro.GetComponent<Image>().fillAmount = float.Parse(nowXianJiExp) / (float)(maxXianJiValue);

        //显示次数
        int nowZuoQiXianJiNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiXianJiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
        Obj_XianJiNumShow.GetComponent<Text>().text = nowZuoQiXianJiNum + "/" + DayXianJiMaxNum;

        //显示消耗金币
        //int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        //xianJiCostGold = 100000 + roseLv * 10000;

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


    public void Btn_XianJiGold() {


    }


    //献祭按钮
    public void Btn_XianJi() {
        try
        {
            //获取当前道具的品质总和
            int qualityValue = 0;
            for (int i = 0; i < SpaceListStr.Length; i++)
            {
                if (SpaceListStr[i] != null && SpaceListStr[i] != "" && SpaceListStr[i] != "0")
                {
                    string itemPar = Game_PublicClassVar.function_DataSet.DataSet_ReadData("ItemPar", "ID", SpaceListStr[i], "RosePastureBag");
                    qualityValue = qualityValue + int.Parse(itemPar);
                }
            }

            //献祭经验超过最大值,表示献祭成功
            string zuoQiExpStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiXianJiExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            if (zuoQiExpStr == "" || zuoQiExpStr == null)
            {
                zuoQiExpStr = "0";
            }

            int zuoQiExp = int.Parse(zuoQiExpStr);
            if (zuoQiExp >= maxXianJiValue)
            {
                Game_PublicClassVar.Get_function_Pasture.CreateZuoQi();
                Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiSetNow.GetComponent<UI_ZuoQiSet>().Btn_OpenXianJi();
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你激活坐骑成功!");
            }

            if (qualityValue <= 0)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请放入献祭的道具");
                return;
            }

            //清理道具删除道具
            CostPastureBagItem();


            //触发献祭
            XianJi(SpaceListStr.Length);
        }
        catch (Exception ex) {
            Debug.LogError("献祭ex=" + ex);
        }
    }

    private void XianJi(int num) {

        //添加次数
        int nowZuoQiXianJiNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiXianJiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
        if (nowZuoQiXianJiNum >= DayXianJiMaxNum)
        {
            return;
        }
        else
        {
            nowZuoQiXianJiNum = nowZuoQiXianJiNum + 1;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZuoQiXianJiNum", nowZuoQiXianJiNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
        }

        //添加当前献祭经验
        //qualityValue = (int)((float)(qualityValue) * (0.8f + 0.4f * Random.value));
        int addExp = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(20,50);
        float addPro = 1f;
        switch (num) {
            case 1:
                addPro = 1f;
                break;
            case 2:
                addPro = 1.5f;
                break;
            case 3:
                addPro = 2f;
                break;
        }

        addExp = (int)(addExp * addPro);
        Game_PublicClassVar.Get_function_Pasture.ZuoQiAddXianJiExp(addExp);

        //献祭经验超过最大值,表示献祭成功
        string zuoQiExpStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiXianJiExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (zuoQiExpStr == "" || zuoQiExpStr == null)
        {
            zuoQiExpStr = "0";
        }

        int zuoQiExp = int.Parse(zuoQiExpStr);
        if (zuoQiExp >= maxXianJiValue)
        {
            Game_PublicClassVar.Get_function_Pasture.CreateZuoQi();
            Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiSetNow.GetComponent<UI_ZuoQiSet>().Btn_OpenXianJi();
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你激活坐骑成功!");
        }
        else
        {
            //初始化显示
            InitShow();
        }
    }

    public void Btn_TiQianOpen() {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_12");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("冒险家5级可以提前免费开启坐骑献祭", TiQianOpen, OpenMaoXianJia, "系统提示","免费开启","查看冒险家",null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否前往兑换金币界面？", GoToGoToDuiHuanGold, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    public void TiQianOpen() {

        //判断冒险家等级
        string maoxianValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MaoXianJiaExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (maoxianValue == "" || maoxianValue == null) {
            maoxianValue = "0";
        }
        if (int.Parse(maoxianValue) >= 3000)
        {
            Game_PublicClassVar.Get_function_Pasture.ZuoQiAddXianJiExp(maxXianJiValue);
            Game_PublicClassVar.Get_function_Pasture.CreateZuoQi();
            //Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiSet.GetComponent<UI_ZuoQiSet>().Btn_OpenXianJi();
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你激活坐骑成功!");


            CloseXianJiStatus = true;
            //Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiSet.GetComponent<UI_ZuoQiSet>().Btn_OpenXianJi();
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("冒险家等级不足!");
            return;
        }
    }

    public void OpenMaoXianJia() {

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing == null)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().ClearnOpenUI();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_HuoDongDaTing();
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().IfInitStatus = false;
            Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().IfInitZhiChiStatus = false;
        }

        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().ZuiHouLiBaoID_Max = "10016";
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().ZuiHouLiBaoID_Min = "10001";
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_RoseHuoDongDaTing.GetComponent<UI_HuoDongDaTing>().Btn_ZhiChiZuoZheSet();

    }

    public void Btn_Close() {
        Destroy(this.gameObject);
    }
}
