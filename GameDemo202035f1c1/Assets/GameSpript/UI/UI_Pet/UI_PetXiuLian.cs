using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PetXiuLian : MonoBehaviour {

    public int NowSeletID;
    public GameObject[] PetXiuLianNameList;
    public GameObject[] PetXiuLianLvList;
    public GameObject[] PetXiuLianXuanZhongImgList;

    //每个修炼的显示
    public GameObject Obj_XiuLianName;
    public GameObject Obj_XiuLianLv;
    private string[] XiuLianLvList;
    public GameObject[] Obj_XiuLianProList;
    public GameObject Obj_Cost_Gold;
    public GameObject Obj_Cost_Item;
    public GameObject Obj_GaiLvStr;
    public GameObject Obj_XiuLianIcon;

    // Use this for initialization
    void Start () {
        Init();
        //默认展示第一个攻击修炼
        ShowPetXiuLian(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //初始化加载
    public void Init() {

        //读取当前宠物修炼等级
        string xiuLianStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetXiuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        XiuLianLvList = xiuLianStr.Split(';');

        //显示宠物各个修炼等级
        for (int i = 0; i< XiuLianLvList.Length;i++) {
            PetXiuLianNameList[i].GetComponent<Text>().text = getXiuLianName(i);
            string nowLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", XiuLianLvList[i], "PetXiuLian_Template");
            PetXiuLianLvList[i].GetComponent<Text>().text = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级") + nowLv;
        }
    }

    //展示宠物修炼
    public void ShowPetXiuLian(int type) {

        //显示选择框
        for (int i = 0; i < PetXiuLianXuanZhongImgList.Length; i++)
        {
            PetXiuLianXuanZhongImgList[i].SetActive(false);
        }

        PetXiuLianXuanZhongImgList[type].SetActive(true);

        string proStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", XiuLianLvList[type], "PetXiuLian_Template");
        string costGoldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostMoney", "ID", XiuLianLvList[type], "PetXiuLian_Template");
        string costItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostItem", "ID", XiuLianLvList[type], "PetXiuLian_Template");
        string xiulianGaiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SuccessPro", "ID", XiuLianLvList[type], "PetXiuLian_Template");

        NowSeletID = type;

        //显示信息
        Obj_XiuLianName.GetComponent<Text>().text = getXiuLianName(type);
        string nowLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", XiuLianLvList[type], "PetXiuLian_Template");
        Obj_XiuLianLv.GetComponent<Text>().text = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级") + nowLv;

        //显示图标
        string icon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", XiuLianLvList[type], "PetXiuLian_Template");
        //显示底图
        Object obj = Resources.Load("PetXiuLian/" + icon, typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_XiuLianIcon.GetComponent<Image>().sprite = img;

        //显示增加属性
        string[] proStrList = proStr.Split(';');

        //清空显示
        for (int i = 0; i < 3; i++)
        {
            Obj_XiuLianProList[i].GetComponent<Text>().text = "";
        }

        for (int i = 0; i < proStrList.Length; i++) {
            Obj_XiuLianProList[i].GetComponent<Text>().text = proStrList[i];
        }

        //显示消耗
        long playGold = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
        string showGoldStr = playGold.ToString();
        if (playGold >= 1000000) {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("万");
            showGoldStr = ((int)(playGold / 10000)).ToString()+ langStr;
        }
        
        string[] costItemList = costItemNum.Split(',');
        int playitemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(costItemList[0]);

        Obj_Cost_Gold.GetComponent<Text>().text = showGoldStr + "/"+  costGoldNum;
        Obj_Cost_Item.GetComponent<Text>().text = playitemNum + "/" +  costItemList[1];
        Obj_GaiLvStr.GetComponent<Text>().text = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("修炼成功概率") + ":" + float.Parse(xiulianGaiLv) * 100 + "%";

        if (playGold < int.Parse( costGoldNum)) {
            Obj_Cost_Gold.GetComponent<Text>().color = Color.red;
        }

        if (playitemNum < int.Parse(costItemList[1]))
        {
            Obj_Cost_Item.GetComponent<Text>().color = Color.red;
        }


    }

    private string getXiuLianName(int i) {

        string returnStr = "";

        switch (i) {
            case 0:
                returnStr = "攻击修炼";
                break;
            case 1:
                returnStr = "防御修炼";
                break;
            case 2:
                returnStr = "伤害修炼";
                break;
            case 3:
                returnStr = "血量修炼";
                break;
        }

        return Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(returnStr);
    }

    //宠物修炼
    public void Btn_XiuLian()
    {

        string costGoldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostMoney", "ID", XiuLianLvList[NowSeletID], "PetXiuLian_Template");
        string costItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostItem", "ID", XiuLianLvList[NowSeletID], "PetXiuLian_Template");
        string xiulianGaiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SuccessPro", "ID", XiuLianLvList[NowSeletID], "PetXiuLian_Template");


        //判定宠物洗炼消耗
        string nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", XiuLianLvList[NowSeletID], "PetXiuLian_Template");
        if (nextID=="0")
        {
            string nowlangStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_444");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(nowlangStr);
            return;
        }

        //判定消耗
        string[] costItemList = costItemNum.Split(',');
        if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() >= int.Parse(costGoldNum) && Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(costItemList[0]) >= int.Parse(costItemList[1]))
        {
            //扣除消耗
            Game_PublicClassVar.Get_function_Rose.CostReward("1", costGoldNum);
            Game_PublicClassVar.Get_function_Rose.CostBagItem(costItemList[0], int.Parse(costItemList[1]));
        }
        else {
            string nowlangStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_446");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(nowlangStr);
            return;
        }

        //判定宠物洗炼成功概率
        if (Random.value>float.Parse(xiulianGaiLv)) {
            string nowlangStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_445");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(nowlangStr);
            //刷新界面
            ShowPetXiuLian(NowSeletID);
            return;
        }


        //写入洗练值
        string xiuLianStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetXiuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        XiuLianLvList = xiuLianStr.Split(';');

        string writeLv = nextID;
        if (writeLv == "0") {
            return;
        }

        XiuLianLvList[NowSeletID] = writeLv.ToString();
        string writeStr = "";
        for (int i = 0; i < XiuLianLvList.Length; i++) {
            writeStr = writeStr + XiuLianLvList[i] + ";";
        }

        writeStr = writeStr.Substring(0, writeStr.Length - 1);

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetXiuLian", writeStr,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_447");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);

        //刷新界面
        ShowPetXiuLian(NowSeletID);
        Init();
    }

    //显示Tips
    public void ShowTips(string  itemID) {


        GameObject obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(itemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
        //其余默认为道具,如果其他道具需做特殊处理
        //obj_ItemTips.GetComponent<UI_ItemTips>().UIBagSpaceNum = "0";
        //obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = "1";



    }

}
