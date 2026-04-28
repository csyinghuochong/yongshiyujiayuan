using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ShouJiItemObj : MonoBehaviour
{

    public string HonorStoreID;
    private string ItemID;
    public GameObject Obj_ItemIcon;
    public GameObject Obj_ItemName;
    public GameObject Obj_ItemQuality;
    public GameObject Obj_BuyPrice;
    public GameObject Obj_BtnImg;
    public GameObject Obj_ImgWeiYongYou;
    private string buyPrice;
    public bool ifshow;
    private GameObject obj_ItemTips;


    // Use this for initialization
    void Start()
    {
        ItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", HonorStoreID, "ShouJiItem_Template");
        //显示道具图标
        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
        string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");


        //显示名称
        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        Obj_ItemName.GetComponent<Text>().text = itemName;
        //显示兑换价格
        buyPrice = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StartNum", "ID", HonorStoreID, "ShouJiItem_Template");
        Obj_BuyPrice.GetComponent<Text>().text = buyPrice;

        //获取道具是否拥有
        bool shoujiStatus = Game_PublicClassVar.Get_function_Rose.ifShouJiItem(ItemID);


        //显示道具Icon
        object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ItemIcon.GetComponent<Image>().sprite = itemIcon;
        //Obj_ItemIcon.GetComponent<Image>().sprite = Sprite.Create((Texture2D)obj, new Rect(0, 0, 80, 80), new Vector2(0.5f, 0.5f));
        //显示品质
        object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
        Sprite itemQuality = obj2 as Sprite;
        Obj_ItemQuality.GetComponent<Image>().sprite = itemQuality;
        //Obj_ItemIcon.GetComponent<Image>().sprite = Sprite.Create((Texture2D)obj2, new Rect(0, 0, 80, 80), new Vector2(0.5f, 0.5f));

        if (shoujiStatus)
        {
            Obj_ImgWeiYongYou.GetComponent<Text>().text = "(已拥有)";
            Obj_ImgWeiYongYou.GetComponent<Text>().color = Color.green;
            Obj_ItemName.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1);
        }
        else {
            //置灰
            object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
            Material huiMaterial = huiObj as Material;
            Obj_ItemIcon.GetComponent<Image>().material = huiMaterial;
            Obj_ItemQuality.GetComponent<Image>().material = huiMaterial;
            Obj_ItemName.GetComponent<Text>().color = new Color(0.55f,0.55f,0.55f,1);
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    //关闭UI
    public void Btn_Buy() {
        if (!ifshow) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_200");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请升级繁荣等级开启此道具奖励！"); 
            return;
        }

        //检测背包格子
        int bagNum = Game_PublicClassVar.Get_function_UI.BagSpaceNullNum();
        if (bagNum < 1)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_201");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("背包已满,请预留至少1个位置！");
            return;
        }

        //判定当前自身的荣誉
        if (Game_PublicClassVar.Get_function_Rose.CostReward("3", buyPrice)) {
            //发送道具
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(ItemID, 1,"0",0,"0",true,"15");
            //更新任务
            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "5", "1");
        }
        else
        {
            //兑换失败
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_202");
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