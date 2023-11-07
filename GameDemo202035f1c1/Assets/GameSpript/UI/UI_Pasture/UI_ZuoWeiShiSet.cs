using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_ZuoWeiShiSet : MonoBehaviour {

    public GameObject[] Obj_ZiZhiListText;
    public GameObject[] Obj_ZiZhiListImgPro;
    public GameObject Obj_BaoShiDu;
    public GameObject Obj_ZhuangTai;
    public GameObject Obj_BagSpaceListSet;
    public GameObject Obj_BagSpace;

    public GameObject Obj_SelectItemID;
    public GameObject Obj_ZuoQiJinJieShow;
    public GameObject Obj_JinBiWeiShi;
    private ObscuredString jinbiWeiShiGold;

    public ObscuredString BagSpacePosition;     //当前选中的背包格子
    // Use this for initialization
    void Start () {

        Game_PublicClassVar.Get_game_PositionVar.Obj_ZuoQiWeiShiSet_Open = this.gameObject;


        Init();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init() {

        //读取资质
        string nowZuoQiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string zizhiMaxValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhiMax", "ID", nowZuoQiID, "ZuoQi_Template");
        for (int i = 1; i <= Obj_ZiZhiListText.Length; i++)
        {
            string zizhiValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiZiZhi_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Obj_ZiZhiListText[i - 1].GetComponent<Text>().text = zizhiValue + "/" + zizhiMaxValue;
            Obj_ZiZhiListImgPro[i - 1].GetComponent<Image>().fillAmount = float.Parse(zizhiValue) / float.Parse(zizhiMaxValue);
        }

        //显示状态
        string nowZuoQiBaoShiDu = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoShiDu", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nowZuoQiBaoStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

        Obj_BaoShiDu.GetComponent<Text>().text = nowZuoQiBaoShiDu;
        string statusName = Game_PublicClassVar.Get_function_Pasture.ReturnStatusName(nowZuoQiBaoStatus);
        Obj_ZhuangTai.GetComponent<Text>().text = statusName;


        //更新背包显示
        UpdateBag();

        //显示阶段
        string nowLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiJieDuanLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        object obj = Resources.Load("GameUI/Img/ZuoQiJieDuanLv_" + nowLv, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ZuoQiJinJieShow.GetComponent<Image>().sprite = itemIcon;

        if (BagSpacePosition == "" || BagSpacePosition == "0" || BagSpacePosition == null)
        {
            Obj_SelectItemID.SetActive(false);
        }
        else {
            Obj_SelectItemID.SetActive(true);
        }

        jinbiWeiShiGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpZiZhiCostGold", "ID", nowZuoQiID, "ZuoQi_Template");
        Obj_JinBiWeiShi.GetComponent<Text>().text = jinbiWeiShiGold;
    }

    //更新背包
    public void UpdateBag()
    {

        Debug.Log("点击了道具");
        //清空道具列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_BagSpaceListSet);

        //将自身的所有消耗品显示
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePastureBagMaxNum; i++)
        {
            string itemID = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RosePastureBag");

            if (itemID != "" && itemID != "0")
            {
                string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
                string itemTypeSon = function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");

                if (itemType == "6" && itemTypeSon == "1")
                {
                    //Debug.Log("开始创建！");
                    //开始创建背包格子
                    GameObject bagSpace = (GameObject)Instantiate(Obj_BagSpace);
                    bagSpace.transform.SetParent(Obj_BagSpaceListSet.transform);
                    bagSpace.GetComponent<UI_PastureBagSpace>().BagPosition = i.ToString();
                    bagSpace.GetComponent<UI_PastureBagSpace>().SpaceType = "1";   //设置格子为背包属性   
                    bagSpace.GetComponent<UI_PastureBagSpace>().MoveBagStatus = false;
                    bagSpace.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }

    //显示放置
    public void PutSpaceID(string bagPosition) {

        BagSpacePosition = bagPosition;

        //读取图标
        string nowItem = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagPosition, "RosePastureBag");
        string nowItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", nowItem, "Item_Template");

        Obj_SelectItemID.SetActive(true);

        //显示道具Icon
        object obj = Resources.Load("ItemIcon/" + nowItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_SelectItemID.GetComponent<Image>().sprite = itemIcon;

    }


    //喂食
    public void Btn_WeiShi() {

        if (BagSpacePosition == "" || BagSpacePosition == null)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请放入要喂食的道具");
            return;
        }

        string nowZuoQiBaoShiDu = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoShiDu", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (int.Parse(nowZuoQiBaoShiDu) >= 100)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("饱食度已满,不需要喂食");
            return;
        }


        //扣除背包道具（指定位置的）
        string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", BagSpacePosition, "RosePastureBag");

        //获取品质系数
        string nowItemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", BagSpacePosition, "RosePastureBag");

        //销毁道具
        Game_PublicClassVar.Get_function_Pasture.CostPastureBagSpaceNumItem(nowItemID, 1, BagSpacePosition,false);

        //增加饱食度（随机增加2-8点）
        int addBaoShiDuValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(2,8);
        Game_PublicClassVar.Get_function_Pasture.ZuoQiAddBaoShiDu(addBaoShiDuValue);

        //5%概率增加资质
        if (Random.value<=0.05f) {
            Game_PublicClassVar.Get_function_Pasture.AddZuoQiZiZhi();
        }

        //清空放置
        BagSpacePosition = "";

        //更新背包显示
        UpdateBag();

        Init();

    }


    //金币喂食
    public void Btn_WeiShiGold()
    {

        //防止出错
        if (int.Parse(jinbiWeiShiGold) <= 1000) {
            return;
        }

        //消耗金币
        long selfGold = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
        if (selfGold < int.Parse(jinbiWeiShiGold))
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("金币不足!");
            return;
        }

        string nowZuoQiBaoShiDu = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoShiDu", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (int.Parse(nowZuoQiBaoShiDu) < 100)
        {
            //增加饱食度（随机增加2-8点）
            int addBaoShiDuValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(2, 8);
            Game_PublicClassVar.Get_function_Pasture.ZuoQiAddBaoShiDu(addBaoShiDuValue);
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("饱食度已满,但会有概率增加坐骑资质!");
        }

        //销毁道具
        Game_PublicClassVar.Get_function_Rose.CostReward("1", jinbiWeiShiGold);

        //10%概率增加资质
        if (Random.value <= 0.15f)
        {
            Game_PublicClassVar.Get_function_Pasture.AddZuoQiZiZhi();
        }

        Init();

    }


    //交谈
    public void Btn_JiaoTan() {

        Debug.Log("我点击了交谈");
    }
}
