using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureDuiHuanItemObj : MonoBehaviour
{

    public ObscuredString HonorStoreID;
    private ObscuredString ItemID;
    public GameObject Obj_ItemIcon;
    public GameObject Obj_ItemName;
    public GameObject Obj_ItemQuality;
    public GameObject Obj_BuyPrice;
    public GameObject Obj_ItemNum;
    public GameObject Obj_BtnImg;
    public GameObject Obj_ImgYiLingQu;
    private ObscuredString buyPrice;
    public ObscuredBool ifshow;
    private ObscuredInt buyNum;
    private GameObject obj_ItemTips;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Init() {

        ItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", HonorStoreID, "PastureDuiHuanStore_Template");
        //显示道具图标
        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
        string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");

        //显示名称
        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        Obj_ItemName.GetComponent<Text>().text = itemName;
        //显示兑换价格
        buyPrice = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuyPrice", "ID", HonorStoreID, "PastureDuiHuanStore_Template");
        Obj_BuyPrice.GetComponent<Text>().text = buyPrice;

        if (!ifshow)
        {
            object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
            Material huiMaterial = huiObj as Material;
            Obj_BtnImg.GetComponent<Image>().material = huiMaterial;
            Obj_ItemName.SetActive(false);
            //Obj_ItemIcon.SetActive(false);
            Obj_BuyPrice.GetComponent<Text>().text = "未激活道具";
            Obj_ItemNum.GetComponent<Text>().text = "";
            Obj_ImgYiLingQu.SetActive(false);
            //Obj_BuyPrice.SetActive(false);
        }
        else
        {
            //显示道具Icon
            object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
            Sprite itemIcon = obj as Sprite;
            Obj_ItemIcon.GetComponent<Image>().sprite = itemIcon;

            //显示品质
            object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
            Sprite itemQuality = obj2 as Sprite;
            Obj_ItemQuality.GetComponent<Image>().sprite = itemQuality;

            //显示数量
            buyNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuyNum", "ID", HonorStoreID, "PastureDuiHuanStore_Template"));
            string buyNumStr = buyNum.ToString();
            if (buyNum >= 100000)
            {
                buyNumStr = (int)(buyNum / 10000) + "万";
            }
            Obj_ItemNum.GetComponent<Text>().text = buyNumStr;

            //判断道具是否领取
            bool ifLingQu = Game_PublicClassVar.Get_function_Pasture.PastureDuiHuanIDJianCe(HonorStoreID);
            if (ifLingQu)
            {
                Obj_ImgYiLingQu.SetActive(true);
                Obj_BuyPrice.SetActive(false);
                Obj_BtnImg.SetActive(false);
            }
            else
            {
                Obj_ImgYiLingQu.SetActive(false);
                Obj_BuyPrice.SetActive(true);
                Obj_BtnImg.SetActive(true);
            }
        }

    }


    //关闭UI
    public void Btn_Buy() {
        if (!ifshow) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_238");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请升级繁荣等级开启此道具奖励！"); 
            return;
        }

        //检测背包格子
        int bagNum = Game_PublicClassVar.Get_function_UI.BagSpaceNullNum();
        if (bagNum < 1)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_239");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包已满,请预留至少1个位置！");
            return;
        }

        //判定当前自身的牧场资金
        if (Game_PublicClassVar.Get_function_Rose.CostReward("5", buyPrice)) {
            //发送道具
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(ItemID, buyNum, "0", 0, "0", true, "36");
            //更新任务
            //Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "5", "1");

            //记录ID
            Game_PublicClassVar.Get_function_Pasture.PastureDuiHuanAdd(HonorStoreID);

            //刷新
            Init();

        }
        else
        {
            //兑换失败
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_240");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("荣誉不足！");
        }
        /*
        string countryHonorValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryHonor", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (int.Parse(countryHonorValue) >= int.Parse(buyPrice))
        {
            //兑换成功
            if (Game_PublicClassVar.Get_function_Country.CostCountryHonor(int.Parse(buyPrice))) { 
                //发送道具
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(ItemID, 1);
                //更新任务
                Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "5", "1");
            }
        }
        else {
            //兑换失败
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("荣誉不足！");
        }
        */
    }

    //鼠标点击触发
    public void Mouse_Click()
    {
        if (!ifshow) {
            return;
        }
        //判定栏内是否有道具
        if (ItemID != "0")
        {
            //获取当前Tips栏内是否有Tips,如果有就清空处理
            GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
            for (int i = 0; i < parentObj.transform.childCount; i++)
            {
                GameObject go = parentObj.transform.GetChild(i).gameObject;
                Destroy(go);
            }

            //调用方法显示UI的Tips
            if (obj_ItemTips == null)
            {
                obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(ItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");

                //获取目标是否是装备
                if (itemType == "3")
                {
                    obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "3";
                }
                else
                {
                    obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = "3";
                }
            }
            else
            {
                //Destroy(obj_ItemTips);
            }
        }
    }

    public void Mouse_Up()
    {
        if (obj_ItemTips != null)
        {
            Destroy(obj_ItemTips);
        }
    }

}