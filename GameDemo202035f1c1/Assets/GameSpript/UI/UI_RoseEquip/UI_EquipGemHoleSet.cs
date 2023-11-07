using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_EquipGemHoleSet : MonoBehaviour {

    public string equipSpaceType;
    public string equipSpace;

    public string nowSelectGemHole;

    public GameObject Obj_EquipItem;

    public GameObject Obj_EquipGemHole_1;
    public GameObject Obj_EquipGemHole_2;
    public GameObject Obj_EquipGemHole_3;
    public GameObject Obj_EquipGemHole_4;

    public GameObject[] Obj_EquipGemHoleTypeDi;
    public GameObject[] Obj_EquipGemHoleTypeLab;

    public GameObject Obj_EquipGemHoleItem_1;
    public GameObject Obj_EquipGemHoleItem_2;
    public GameObject Obj_EquipGemHoleItem_3;
    public GameObject Obj_EquipGemHoleItem_4;

    public GameObject Obj_EquipGemHoleSelectImg_1;
    public GameObject Obj_EquipGemHoleSelectImg_2;
    public GameObject Obj_EquipGemHoleSelectImg_3;
    public GameObject Obj_EquipGemHoleSelectImg_4;

    public bool UpdateEquipGemStatus;

	private string equipGemHoleStr = "";
	private string equipGemIDStr = "";
	private string equipItemID = "";
	//string equipItemEquip = "";
	private string equipItemHideID = "";
	private string equipItemNum = "";

	// Use this for initialization
	void Start () {

        //初始化
        Init();
        //nowSelectGemHole = "1";
        //SelectGemHole(nowSelectGemHole);

    }

    public void Init() {

        //初始显示装备第一个格子
        equipSpaceType = "2";
        equipSpace = "1";
        //nowSelectGemHole = "1";
        //SelectGemHole(nowSelectGemHole);
        //更新宝石孔位信息
        updateEquipGemHole();
    }
	
	// Update is called once per frame
	void Update () {
        if (UpdateEquipGemStatus) {
            UpdateEquipGemStatus = false;
            updateEquipGemHole();
            nowSelectGemHole = "1";
            SelectGemHole(nowSelectGemHole);
        }
	}

    private void updateEquipGemHole() {

        //显示当前装备的宝石
		/*
        string equipGemHoleStr = "";
        string equipGemIDStr = "";
        string equipItemID = "";
        //string equipItemEquip = "";
        string equipItemHideID = "";
        string equipItemNum = "";
		*/

        switch (equipSpaceType)
        {
            //背包
            case "1":
                equipGemHoleStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", equipSpace, "RoseBag");
                equipGemIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", equipSpace, "RoseBag");
                equipItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", equipSpace, "RoseBag");
                equipItemHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", equipSpace, "RoseBag");
                equipItemNum = "1";
                
                break;


            //装备
            case "2":
                equipGemHoleStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", equipSpace, "RoseEquip");
                equipGemIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", equipSpace, "RoseEquip");
                equipItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", equipSpace, "RoseEquip");
                equipItemHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", equipSpace, "RoseEquip");
                equipItemNum = "1";
                break;
        }


        //显示当前指定的装备
        //Debug.Log("equipItemID = " + equipItemID);
        //Debug.Log("equipGemIDStr = " + equipGemIDStr);
        Obj_EquipItem.GetComponent<UI_ItemIcomShow>().ItemID = equipItemID;
        Obj_EquipItem.GetComponent<UI_ItemIcomShow>().ItemNum = equipItemNum;
        Obj_EquipItem.GetComponent<UI_ItemIcomShow>().HindID = equipItemHideID;
        Obj_EquipItem.GetComponent<UI_ItemIcomShow>().ItemGemHole = equipGemHoleStr;
        Obj_EquipItem.GetComponent<UI_ItemIcomShow>().ItemGemID = equipGemIDStr;
        Obj_EquipItem.GetComponent<UI_ItemIcomShow>().ShowUpdateItem();
        //隐藏底
        if (equipItemID != "" && equipItemID != "0") {
            GameObject backObj = Obj_EquipItem.transform.parent.transform.Find("Img_EquipBack").gameObject;
            if (backObj != null)
            {
                backObj.SetActive(false);
            }
        }

		//清楚选中的图片
		Obj_EquipGemHoleSelectImg_1.SetActive(false);
		Obj_EquipGemHoleSelectImg_2.SetActive(false);
		Obj_EquipGemHoleSelectImg_3.SetActive(false);
		Obj_EquipGemHoleSelectImg_4.SetActive(false);

        //先清除4个格子的数据
        Obj_EquipGemHoleItem_1.SetActive(false);
        Obj_EquipGemHoleItem_2.SetActive(false);
        Obj_EquipGemHoleItem_3.SetActive(false);
        Obj_EquipGemHoleItem_4.SetActive(false);

        //隐藏宝石孔位
        Obj_EquipGemHole_1.SetActive(false);
        Obj_EquipGemHole_2.SetActive(false);
        Obj_EquipGemHole_3.SetActive(false);
        Obj_EquipGemHole_4.SetActive(false);

		Obj_EquipItem.SetActive (false);

        showHoleItemDi(Obj_EquipGemHole_1);
        showHoleItemDi(Obj_EquipGemHole_2);
        showHoleItemDi(Obj_EquipGemHole_3);
        showHoleItemDi(Obj_EquipGemHole_4);

        //如果格子为空置返回空
        if(equipGemIDStr==""){
            return;
        }

        if (equipItemID == "0" || equipItemID == "")
        {
            //Debug.Log("当前位置没有装备");
            return;
        }
        else {
            Obj_EquipItem.SetActive(true);
        }

        int holeNum = 0;
        string[] equipGemHoleList = equipGemHoleStr.Split(',');
        if(equipGemHoleStr!=""&&equipGemHoleStr!="0"){

            holeNum = equipGemHoleList.Length;
            //显示宝石不同类型的底框
            for (int i = 0; i < holeNum;i++ )
            {
                showEquipGemHoleTypeDi(Obj_EquipGemHoleTypeDi[i], Obj_EquipGemHoleTypeLab[i],equipGemHoleList[i]);
            }
        }
        switch (holeNum) { 
            case 0:
                break;
            case 1:
                Obj_EquipGemHole_1.SetActive(true);
                break;
            case 2:
                Obj_EquipGemHole_1.SetActive(true);
                Obj_EquipGemHole_2.SetActive(true);
                break;
            case 3:
                Obj_EquipGemHole_1.SetActive(true);
                Obj_EquipGemHole_2.SetActive(true);
                Obj_EquipGemHole_3.SetActive(true);
                break;
            case 4:
                Obj_EquipGemHole_1.SetActive(true);
                Obj_EquipGemHole_2.SetActive(true);
                Obj_EquipGemHole_3.SetActive(true);
                Obj_EquipGemHole_4.SetActive(true);
                break;
        }


        //清理显示
        clearnEquipGemData();


        //显示宝石
        string[] equipGemIDStrList = equipGemIDStr.Split(',');
        for (int i = 0; i < equipGemIDStrList.Length; i++) {
            //Debug.Log("equipGemIDStrList = " + equipGemIDStrList[i]);
            switch (i) { 
                
                case 0:
                    if (equipGemIDStrList[i] != "" && equipGemIDStrList[i] != "0")
                    {
                        //Obj_EquipGemHole_1.SetActive(true);
                        Obj_EquipGemHoleItem_1.SetActive(true);
                        Obj_EquipGemHoleItem_1.GetComponent<UI_ItemIcomShow>().ItemID = equipGemIDStrList[i];
                        Obj_EquipGemHoleItem_1.GetComponent<UI_ItemIcomShow>().EquipTipsType = "8";
                        Obj_EquipGemHoleItem_1.GetComponent<UI_ItemIcomShow>().ShowUpdateItem();

                        GameObject backObj = Obj_EquipGemHole_1.transform.Find("Img_EquipBack").gameObject;
                        if (backObj != null)
                        {
                            backObj.SetActive(false);
                        }
                    }
                    break;

                case 1:
                    if (equipGemIDStrList[i] != "" && equipGemIDStrList[i] != "0")
                    {
                        //Obj_EquipGemHole_2.SetActive(true);
                        Obj_EquipGemHoleItem_2.SetActive(true);
                        Obj_EquipGemHoleItem_2.GetComponent<UI_ItemIcomShow>().ItemID = equipGemIDStrList[i];
                        Obj_EquipGemHoleItem_2.GetComponent<UI_ItemIcomShow>().EquipTipsType = "8";
                        Obj_EquipGemHoleItem_2.GetComponent<UI_ItemIcomShow>().ShowUpdateItem();

                        GameObject backObj = Obj_EquipGemHole_2.transform.Find("Img_EquipBack").gameObject;
                        if (backObj != null)
                        {
                            backObj.SetActive(false);
                        }
                    }
                    break;

                case 2:
                    if (equipGemIDStrList[i] != "" && equipGemIDStrList[i]!="0")
                    {
                        //Obj_EquipGemHole_3.SetActive(true);
                        Obj_EquipGemHoleItem_3.SetActive(true);
                        Obj_EquipGemHoleItem_3.GetComponent<UI_ItemIcomShow>().ItemID = equipGemIDStrList[i];
                        Obj_EquipGemHoleItem_3.GetComponent<UI_ItemIcomShow>().EquipTipsType = "8";
                        Obj_EquipGemHoleItem_3.GetComponent<UI_ItemIcomShow>().ShowUpdateItem();

                        GameObject backObj = Obj_EquipGemHole_3.transform.Find("Img_EquipBack").gameObject;
                        if (backObj != null)
                        {
                            backObj.SetActive(false);
                        }
                    }
                    break;

                case 3:                    
                    if (equipGemIDStrList[i] != "" && equipGemIDStrList[i]!="0")
                    {
                        //Obj_EquipGemHole_4.SetActive(true);
                        Obj_EquipGemHoleItem_4.SetActive(true);
                        Obj_EquipGemHoleItem_4.GetComponent<UI_ItemIcomShow>().ItemID = equipGemIDStrList[i];
                        Obj_EquipGemHoleItem_4.GetComponent<UI_ItemIcomShow>().EquipTipsType = "8";
                        Obj_EquipGemHoleItem_4.GetComponent<UI_ItemIcomShow>().ShowUpdateItem();

                        GameObject backObj = Obj_EquipGemHole_4.transform.Find("Img_EquipBack").gameObject;
                        if (backObj != null)
                        {
                            backObj.SetActive(false);
                        }
                    }
                    break;
            }
        }
    }

    //清理数据显示
    void clearnEquipGemData() {
        Obj_EquipGemHoleItem_1.GetComponent<UI_ItemIcomShow>().ItemID = "";
        Obj_EquipGemHoleItem_1.GetComponent<UI_ItemIcomShow>().EquipTipsType = "";

        Obj_EquipGemHoleItem_2.GetComponent<UI_ItemIcomShow>().ItemID = "";
        Obj_EquipGemHoleItem_2.GetComponent<UI_ItemIcomShow>().EquipTipsType = "";

        Obj_EquipGemHoleItem_3.GetComponent<UI_ItemIcomShow>().ItemID = "";
        Obj_EquipGemHoleItem_3.GetComponent<UI_ItemIcomShow>().EquipTipsType = "";

        Obj_EquipGemHoleItem_4.GetComponent<UI_ItemIcomShow>().ItemID = "";
        Obj_EquipGemHoleItem_4.GetComponent<UI_ItemIcomShow>().EquipTipsType = "";
    }

    void showHoleItemDi(GameObject holeSet) {

        GameObject backObj = holeSet.transform.Find("Img_EquipBack").gameObject;
        if (backObj != null)
        {
            backObj.SetActive(true);
        }

    }

    public void showEquipGemHoleTypeDi(GameObject holeTypeDi, GameObject holeTypeLab, string holeType)
    {
        switch (holeType)
        {
            //红色孔位
            case "101":
                object obj = Resources.Load("GemHoleDi/" + holeType, typeof(Sprite));
                Sprite itemIcon = obj as Sprite;
                holeTypeDi.GetComponent<Image>().sprite = itemIcon;
                holeTypeLab.GetComponent<Text>().text = "红色孔位";
                break;
            //紫色孔位
            case "102":
                obj = Resources.Load("GemHoleDi/" + holeType, typeof(Sprite));
                itemIcon = obj as Sprite;
                holeTypeDi.GetComponent<Image>().sprite = itemIcon;
                holeTypeLab.GetComponent<Text>().text = "紫色孔位";
                break;
            //蓝色孔位
            case "103":
                obj = Resources.Load("GemHoleDi/" + holeType, typeof(Sprite));
                itemIcon = obj as Sprite;
                holeTypeDi.GetComponent<Image>().sprite = itemIcon;
                holeTypeLab.GetComponent<Text>().text = "蓝色孔位";
                break;
            //绿色孔位
            case "104":
                obj = Resources.Load("GemHoleDi/" + holeType, typeof(Sprite));
                itemIcon = obj as Sprite;
                holeTypeDi.GetComponent<Image>().sprite = itemIcon;
                holeTypeLab.GetComponent<Text>().text = "绿色孔位";
                break;
            //白色孔位
            case "105":
                obj = Resources.Load("GemHoleDi/" + holeType, typeof(Sprite));
                itemIcon = obj as Sprite;
                holeTypeDi.GetComponent<Image>().sprite = itemIcon;
                holeTypeLab.GetComponent<Text>().text = "白色孔位";
                break;
            //多彩插槽
            case "110":
                obj = Resources.Load("GemHoleDi/" + holeType, typeof(Sprite));
                itemIcon = obj as Sprite;
                holeTypeDi.GetComponent<Image>().sprite = itemIcon;
                holeTypeLab.GetComponent<Text>().text = "多彩插槽";
                break;
        }

    }

    public void SelectGemHole(string nowSelectValue) {
        Debug.Log("调用SelectGemHole + " + nowSelectValue);
		//如果当前装备为空则不显示
		if (equipItemID == "" || equipItemID == "0") {
			return;
		}

        //隐藏全部选中图标
        Obj_EquipGemHoleSelectImg_1.SetActive(false);
        Obj_EquipGemHoleSelectImg_2.SetActive(false);
        Obj_EquipGemHoleSelectImg_3.SetActive(false);
        Obj_EquipGemHoleSelectImg_4.SetActive(false);

        //设置选中值
        nowSelectGemHole = nowSelectValue;

        //显示道具Tps
        switch (nowSelectValue)
        {

            case "1":
                Obj_EquipGemHoleItem_1.GetComponent<UI_ItemIcomShow>().Mouse_Click();
                Obj_EquipGemHoleSelectImg_1.SetActive(true);
                break;

            case "2":
                Obj_EquipGemHoleItem_2.GetComponent<UI_ItemIcomShow>().Mouse_Click();
                Obj_EquipGemHoleSelectImg_2.SetActive(true);
                break;

            case "3":
                Obj_EquipGemHoleItem_3.GetComponent<UI_ItemIcomShow>().Mouse_Click();
                Obj_EquipGemHoleSelectImg_3.SetActive(true);
                break;

            case "4":
                Obj_EquipGemHoleItem_4.GetComponent<UI_ItemIcomShow>().Mouse_Click();
                Obj_EquipGemHoleSelectImg_4.SetActive(true);
                break;

        }
    }
}
