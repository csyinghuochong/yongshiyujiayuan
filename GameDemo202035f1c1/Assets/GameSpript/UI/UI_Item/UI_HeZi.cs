using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_HeZi : MonoBehaviour {

    public ObscuredString ItemID;
    public ObscuredString ItemSubType;
    public ObscuredString BagSpace;
    public GameObject Obj_ItemNum;
    public GameObject Obj_ItemIcon;
    public GameObject Obj_ItemQuality;
    public GameObject Obj_BtnZuanshiText;
    public GameObject Obj_Title1;
    public GameObject Obj_Title2;
    public GameObject Obj_Title3;
    private ObscuredInt openZuanShi;
    //经验用的
    private ObscuredInt roseExp;
    private ObscuredInt addRoseExp;

    //金币用的
    private ObscuredInt roseGold;
    private ObscuredInt addRoseGold;

    //按钮
    public GameObject BtnSet_Gold;
    public GameObject BtnSet_Exp;

    private bool ClickStatus;
    //private int openZuanShi_Gold;

	// Use this for initialization
	void Start () {

        //适配界面
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        ItemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");

        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        Obj_ItemNum.GetComponent<Text>().text = "数量:" + itemNum;

        //显示道具图标
        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
        string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
        //显示道具Icon
        object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ItemIcon.GetComponent<Image>().sprite = itemIcon;
        //显示品质
        object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
        Sprite itemQuality = obj2 as Sprite;
        Obj_ItemQuality.GetComponent<Image>().sprite = itemQuality;
        string makeItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        Obj_Title3.GetComponent<Text>().text = makeItemName;

        BtnSet_Gold.SetActive(false);
        BtnSet_Exp.SetActive(false);

        switch (ItemSubType) { 
            case "2":
                Obj_Title1.GetComponent<Text>().text = "开启经验木桩可获得大量经验！";
                Obj_Title2.GetComponent<Text>().text = "提示：钻石开启获得更多经验,更有概率触发经验暴击！";
                //显示开启钻石
                openZuanShi = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "5", "GameMainValue"));
                Obj_BtnZuanshiText.GetComponent<Text>().text = openZuanShi + "钻开启";
                //更新显示数据
                updataRoseExp();
                BtnSet_Exp.SetActive(true);
                break;
            case "3":
                Obj_Title1.GetComponent<Text>().text = "开启金币袋子可获得大量金币！";
                Obj_Title2.GetComponent<Text>().text = "提示：开启获得更多金币,更有概率触发金币暴击！";
                //显示开启钻石
                openZuanShi = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "10", "GameMainValue"));
                Obj_BtnZuanshiText.GetComponent<Text>().text = openZuanShi + "钻开启";
                //更新显示数据
                updataRoseGold();
                BtnSet_Gold.SetActive(true);
                break;
        }

        //关闭背包界面
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseEquip();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    //免费开启按钮
    public void Btn_MianFei()
    {
        if (ClickStatus) {
            return;
        }

        ClickStatus = true;

        //监测背包是否有道具
        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        //int itemNum = 100;
        if (itemNum <= 0)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_386");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("道具不足");
            ClickStatus = false;
            return;
        }
        else {
            //CostBagSpaceNumItem
            Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
            //Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(ItemID, 1, BagSpace,false);
            updataBagItemNum();
        }

        switch (ItemSubType) {
            case "2":
                updataRoseExp();    //更新获得经验
                addRoseExp = (int)(addRoseExp * 0.7f);      //特殊处理,因为改配置麻烦T.T
                Game_PublicClassVar.Get_function_Rose.AddExp(addRoseExp,"1");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_387");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + addRoseExp);
                //Game_PublicClassVar.Get_function_UI.GameHint("免费开启,获得经验：" + addRoseExp);
            break;

            case "3":
                updataRoseGold();    //更新获得金币

                //金币有10%概率触发暴击  双倍金币奖励
                if (Random.value <= 0.15f)
                {
                    addRoseGold = (int)(addRoseGold * 2);
                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_392");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + addRoseGold);
                    //Game_PublicClassVar.Get_function_UI.GameHint("触发金币暴击！获得双倍金币：" + addRoseGold);
                }
                else {
                    langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_388");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + addRoseGold);
                }

                Game_PublicClassVar.Get_function_Rose.SendReward("1", addRoseGold.ToString(),"43");

                //Game_PublicClassVar.Get_function_UI.GameHint("免费开启,获得金币：" + addRoseGold);
            break;
        }
        ClickStatus = false;
    }

    //钻石开启
    public void Btn_ZuanShi()
    {
        //判定当前是否超过肝帝等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv >= 10) {
            if (Game_PublicClassVar.Get_wwwSet.WorldPlayerLv >= 1)
            {
                if (roseLv >= Game_PublicClassVar.Get_wwwSet.WorldPlayerLv) {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_389");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameHint("已达到当前最高等级,无法使用此道具!");
                    return;
                }
            }
            else {
                if (Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus == true)
                {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_389");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameHint("已达到当前最高等级,无法使用此道具!");
                }
                else {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_346");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameHint("未连接网络,无法使用此道具!");
                }

                return;
            }
        }

        if (ClickStatus)
        {
            return;
        }
        ClickStatus = true;

        //监测背包是否有道具
        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        if (itemNum <= 0)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_386");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("道具不足");
            ClickStatus = false;
            return;
        }

        int roseRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        if (roseRmb < openZuanShi)
        {
            Game_PublicClassVar.Get_function_UI.AddRMBHint();   //钻石不足统一提示
            ClickStatus = false;
            return;
        }
        else
        {
            //扣除钻石
            Game_PublicClassVar.Get_function_Rose.CostReward("2", openZuanShi.ToString());
            //销毁道具
            Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
            updataBagItemNum();         //监测道具是否存在
        }

        switch (ItemSubType) {
            //经验盒子
            case "2":
                updataRoseExp();    //更新获得经验
                float expPro_Zuanshi = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "4", "GameMainValue"));
                //监测是否触发经验暴击
                float expPro_Cir = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "3", "GameMainValue"));
                if (Random.value <= expPro_Cir)
                {
                    addRoseExp = (int)(addRoseExp * expPro_Zuanshi * 2);
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_390");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + addRoseExp);
                    //Game_PublicClassVar.Get_function_UI.GameHint("触发经验暴击！获得大量经验：" + addRoseExp);
                }
                else {
                    addRoseExp = (int)(addRoseExp * expPro_Zuanshi);
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_391");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + addRoseExp);
                    //Game_PublicClassVar.Get_function_UI.GameHint("钻石开启！获得大量经验：" + addRoseExp);
                }
                Debug.Log("addRoseExp111:" + addRoseExp);
                //根据等级递增
                if (roseLv >= 30&& roseLv <= 59) {
                    addRoseExp = (int)(addRoseExp * (1 + (roseLv - 30) / 30));
                }
                if (roseLv >= 60)
                {
                    addRoseExp = (int)(addRoseExp * (1 + (roseLv - 30) / 15));
                }
                Debug.Log("addRoseExp222:" + addRoseExp);
                Game_PublicClassVar.Get_function_Rose.AddExp((int)(addRoseExp),"1");

            break;
            //金币盒子
            case "3":
                updataRoseGold();   //更新获得金币
                float goldPro_Zuanshi = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "9", "GameMainValue"));
                //监测是否触发经验暴击
                float goldPro_Cir = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "8", "GameMainValue"));
                if (Random.value <= goldPro_Cir)
                {
                    addRoseGold = (int)(addRoseGold * goldPro_Zuanshi * 2);
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_392");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + addRoseGold);
                    //Game_PublicClassVar.Get_function_UI.GameHint("触发金币暴击！获得大量金币：" + addRoseGold);
                }
                else {
                    addRoseGold = (int)(addRoseGold * goldPro_Zuanshi);
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_393");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + addRoseGold);
                    //Game_PublicClassVar.Get_function_UI.GameHint("钻石开启！获得大量金币：" + addRoseGold);
                }

                Game_PublicClassVar.Get_function_Rose.SendReward("1", addRoseGold.ToString(),"43");

            break;
        }

        ClickStatus = false;
    }

    //更新背包此道具数据,为0时关闭界面
    public void updataBagItemNum() {
        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        if (itemNum <= 0)
        {
            Btn_Close();
        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_394");
            Obj_ItemNum.GetComponent<Text>().text = langStrHint + ":" + itemNum;                //更新数量
        }
    }

    //更新每次获得的经验
    void updataRoseExp() {
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        roseExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpPro", "RoseLv", roseLv.ToString(), "RoseExp_Template"));
        float expPro_Min = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "1", "GameMainValue"));
        float expPro_Max = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "2", "GameMainValue"));
        addRoseExp = (int)(((expPro_Max - expPro_Min) * Random.value + expPro_Min) * roseExp);
    }

    //更新每次获得的金币
    void updataRoseGold()
    {
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        roseGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseGoldPro", "RoseLv", roseLv.ToString(), "RoseExp_Template"));
        float expPro_Min = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "6", "GameMainValue"));
        float expPro_Max = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "7", "GameMainValue"));
        addRoseGold = (int)(((expPro_Max - expPro_Min) * Random.value + expPro_Min) * roseGold);
    }

    public void Btn_Close() {
        Destroy(this.gameObject);
        //关闭背包界面
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseEquip();
    }


}
