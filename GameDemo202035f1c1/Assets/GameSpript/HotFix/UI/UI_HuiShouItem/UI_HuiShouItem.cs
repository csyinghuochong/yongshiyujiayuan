using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_HuiShouItem : MonoBehaviour {

    public string NpcID;					//NpcID
	public string[] HuiShouBagNumList;		//背包格子
	public string[] HuiShouItemIDList;		//背包道具
	public GameObject[] HuiShouItemList;	//回收道具显示的Obj

	private string huiShouGetItemSet;
	public GameObject HuiShouGetItemSet;
	public GameObject CommonItemIconShow;

	public ObscuredString HuiShouGetItemIDStr;
	public ObscuredString HuiShouGetItemNumStr;

	public int HuiShouQuality;
	public GameObject HuiShouQualityImgObj;

	public bool UpdateStatus;
    public GameObject Obj_HuiShouSet;

    private bool huiShouStatus;

	// Use this for initialization
	void Start () {
        /*
		for (int i = 0; i < HuiShouItemList.Length; i++) {
			HuiShouItemList[i].SetActive (false);
		}
        */

        ClearnPut();

        Game_PublicClassVar.Get_game_PositionVar.HuiShouItemStatus = true;
		//默认回收品质为蓝色的
		HuiShouQuality = 3;
		HuiShouQualityImgObj.SetActive (false);
		//UpdateStatus = true;

		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_HuiShouItem = this.gameObject;

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_HuiShouSet);

    }
	
	// Update is called once per frame
	void Update () {
		if (UpdateStatus) {
			UpdateStatus = false;
			updateHuiShouItem();
		}
	}

	void OnDisable(){
		Game_PublicClassVar.Get_game_PositionVar.HuiShouItemStatus = false;

        //退出时,镜头切换
        string ifCameraLaJin = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfCameraLaJin", "ID", NpcID, "Npc_Template");
        if (ifCameraLaJin == "1")
        {
            Camera.main.GetComponent<CameraAI>().BuildEnterStatus = false;
            Camera.main.GetComponent<CameraAI>().BuildExitStatus = true;
        }
	}

	public void updateHuiShouItem(string noUpdate = ""){

        /*
		for (int i = 0; i < HuiShouItemList.Length; i++) {
			HuiShouItemList[i].SetActive (false);
		}
        */

		//Debug.Log ("noUpdate = " + noUpdate);
		huiShouGetItemSet = "";
		//显示回收装备
		if(noUpdate != "0"){
			HuiShouItemListShow(0);
		}
		if(noUpdate != "1"){
			HuiShouItemListShow(1);
		}
		if(noUpdate != "2"){
			HuiShouItemListShow(2);
		}
		if(noUpdate != "3"){
			HuiShouItemListShow(3);
		}
		if(noUpdate != "4"){
			HuiShouItemListShow(4);
		}
		if(noUpdate != "5"){
			HuiShouItemListShow(5);
		}

		//Debug.Log ("huiShouGetItemSet = " + huiShouGetItemSet);

		//清空回收奖励缓存列表
		HuiShouGetItemIDStr = "";
		HuiShouGetItemNumStr = "";
		//清空奖励obj
		Game_PublicClassVar.Get_function_UI.DestoryTargetObj (HuiShouGetItemSet);

		//回收获取道具ID列表
		//string huishouGetItemIDStr = "";
		string[] huishouGetItemStrList = huiShouGetItemSet.Split(';');
		if (huishouGetItemStrList[0] != "" && huishouGetItemStrList[0] != "0") {
			for (int i = 0; i < huishouGetItemStrList.Length; i++) {
				//bool ifaddStatus = true;
				//回收获取道具的ID
				string huishouGetItemID = huishouGetItemStrList[i].Split(',')[0];
				string huishouGetItemNum = huishouGetItemStrList[i].Split(',')[1];
				addHuiShouGetItem(huishouGetItemID, huishouGetItemNum);
			}
		}
			
		//Debug.Log ("HuiShouGetItemIDStr = " + HuiShouGetItemIDStr);
		//Debug.Log ("HuiShouGetItemNumStr = " + HuiShouGetItemNumStr);

		//列表显示奖励
		string[] huiShouGetItemIDStrList = HuiShouGetItemIDStr.ToString().Split(',');
		string[] huiShouGetItemNumStrList = HuiShouGetItemNumStr.ToString().Split(',');

		if (huiShouGetItemIDStrList [0] != "" && huiShouGetItemIDStrList [0] != "0") {
			for (int i = 0; i < huiShouGetItemIDStrList.Length; i++) {
				GameObject huishouObj = (GameObject)Instantiate (CommonItemIconShow);
				huishouObj.GetComponent<UI_Common_ItemIcon>().ItemID = huiShouGetItemIDStrList[i];
				huishouObj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = int.Parse(huiShouGetItemNumStrList[i]);
				huishouObj.GetComponent<UI_Common_ItemIcon>().UpdateStatus = true;
				huishouObj.transform.SetParent(HuiShouGetItemSet.transform);
			}
		}
	}


	//添加收集奖励ID
	public void addHuiShouGetItem(string addID,string addValue){
		//Debug.Log ("addID = " + addID + " addValue = " + addValue);
		string[] huiShouGetItemIDStrList = HuiShouGetItemIDStr.ToString().Split(',');
		string[] huiShouGetItemNumStrList = HuiShouGetItemNumStr.ToString().Split(',');

		bool addStatus = true;

		//查看添加的又没有重复的id
		if (huiShouGetItemIDStrList[0] != "" && huiShouGetItemIDStrList [0] != "0") {
			for (int i = 0; i < huiShouGetItemIDStrList.Length; i++) {
				if (addID == huiShouGetItemIDStrList[i]) {
					addStatus = false;
					//添加数量
					huiShouGetItemNumStrList[i] = (int.Parse(huiShouGetItemNumStrList[i]) + int.Parse(addValue)).ToString();
					//清空一下数据重新生成数量数据
					HuiShouGetItemNumStr = "";
					//Debug.Log ("huiShouGetItemNumStrList.lenght =" + huiShouGetItemNumStrList.Length);
					//Debug.Log ("huiShouGetItemNumStrList[0] = " + huiShouGetItemNumStrList[0]);
					//Debug.Log ("huiShouGetItemNumStrList[1] = " + huiShouGetItemNumStrList[1]);

					for (int y = 0; y < huiShouGetItemNumStrList.Length; y++) {
						//Debug.Log ("y = " + y +";"+ huiShouGetItemNumStrList [y]);
						if (HuiShouGetItemNumStr == "" || HuiShouGetItemNumStr == "0") {
							HuiShouGetItemNumStr = huiShouGetItemNumStrList[y];
						} else {
							HuiShouGetItemNumStr = HuiShouGetItemNumStr + "," + huiShouGetItemNumStrList[y];
						}
					}
					//Debug.Log ("HuiShouGetItemNumStr = " + HuiShouGetItemNumStr);
				}
			}
		}


		if (addStatus) {
			if (HuiShouGetItemIDStr == "" || HuiShouGetItemIDStr == "0") {
				HuiShouGetItemIDStr = addID;
				HuiShouGetItemNumStr = addValue;
			} else {
				HuiShouGetItemIDStr = HuiShouGetItemIDStr + "," + addID;
				HuiShouGetItemNumStr = HuiShouGetItemNumStr + "," + addValue;
				//Debug.Log ("HuiShouGetItemNumStr123 = " + HuiShouGetItemNumStr);
			}
		}
		//Debug.Log ("HuiShouGetItemNumStr456 = " + HuiShouGetItemNumStr);
	}

	public void yijianFangRu(){

        //清空放入数据
        /*
		for(int i = 0;i<HuiShouBagNumList.Length;i++){
			HuiShouBagNumList[i] = "";
		}
        */
        ClearnPut();

        int huishouBagNum = 0;

		for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++) {
			string spaceID = i.ToString ();
			//获取道具ID
			string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemID", "ID", spaceID, "RoseBag");
			if (itemID != "0") {
				//获取道具类型
				string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemType", "ID", itemID, "Item_Template");
				//获取道具品质
				string itemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemQuality", "ID", itemID, "Item_Template");
				//判断类型为装备
				if (itemType == "3") {
					//判断出售品质
					if (int.Parse (itemQuality) <= HuiShouQuality) {
						HuiShouBagNumList [huishouBagNum] = spaceID;
						//到达每次回收上限
						if (huishouBagNum >= 5) {
							UpdateStatus = true;
							return;
						}
						huishouBagNum = huishouBagNum + 1;
					}
				}
			}
		}

		UpdateStatus = true;
		if (HuiShouBagNumList [0] == "" || HuiShouBagNumList [0] == "0") {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_397");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint ("当前没有符合要求的装备放入!");

            ClearnPut();

        }
	}

	public void CloseUI(){
		Destroy (this.gameObject);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().Obj_HuiShouItem = null;
	}


    private void ClearnPut() {
        //Debug.Log("开始清理....");
        //清理显示
        HuiShouItemListShowClearn(0);
        HuiShouItemListShowClearn(1);
        HuiShouItemListShowClearn(2);
        HuiShouItemListShowClearn(3);
        HuiShouItemListShowClearn(4);
        HuiShouItemListShowClearn(5);

        //清理回收数据
        HuiShouGetItemIDStr = "";
        HuiShouGetItemNumStr = "";

        //清空奖励obj
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(HuiShouGetItemSet);

        //Debug.Log("清理完毕....");
    }

	public void ClickQuality(){
		if (HuiShouQualityImgObj.activeSelf) {
			HuiShouQualityImgObj.SetActive (false);
			HuiShouQuality = 3;
		} else {
			HuiShouQualityImgObj.SetActive (true);
			HuiShouQuality = 4;
		}
	}

	//显示回收装备
	public void HuiShouItemListShow(int spaceNum){
		if (HuiShouBagNumList[spaceNum] != "0" && HuiShouBagNumList[spaceNum] != "") {
			HuiShouItemIDList[spaceNum] = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemID", "ID", HuiShouBagNumList[spaceNum], "RoseBag");
            if (HuiShouItemIDList[spaceNum] == "0" || HuiShouItemIDList[spaceNum] == "" || HuiShouItemIDList[spaceNum] == null) {
                HuiShouItemListShowClearn(spaceNum);
                return;
            }
            int itemNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", HuiShouBagNumList[spaceNum], "RoseBag"));
            HuiShouItemList[spaceNum].SetActive(true);
			HuiShouItemList[spaceNum].GetComponent<UI_Common_ItemIcon>().ItemID = HuiShouItemIDList[spaceNum];
			HuiShouItemList[spaceNum].GetComponent<UI_Common_ItemIcon>().UpdateStatus = true;
			HuiShouItemList[spaceNum].GetComponent<UI_Common_ItemIcon>().ItemEquipTipsType = "7";
			HuiShouItemList[spaceNum].GetComponent<UI_Common_ItemIcon>().BagPosition = HuiShouBagNumList[spaceNum];
			string huishouGetItem =  Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("HuishouGetItem", "ID", HuiShouItemIDList[spaceNum], "Item_Template");
            for (int i = 1; i <= itemNum; i++) {
                if (huishouGetItem != "0" && huishouGetItem != "")
                {
                    if (huiShouGetItemSet != "")
                    {
                        huiShouGetItemSet = huiShouGetItemSet + ";" + huishouGetItem;
                    }
                    else
                    {
                        huiShouGetItemSet = huishouGetItem;
                    }
                }
            }
		}	
	}

	//清理回收装备

	public void HuiShouItemListShowClearn(int spaceNum){
		//Debug.Log ("spaceNum = " + spaceNum);
		//显示回收装备
		if (HuiShouBagNumList[spaceNum] != "0" && HuiShouBagNumList[spaceNum] != "") {
			//HuiShouItemIDList[spaceNum] = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", HuiShouBagNumList[spaceNum], "RoseBag");
            HuiShouItemIDList[spaceNum] = "";
            HuiShouBagNumList[spaceNum] = "";
            HuiShouItemList[spaceNum].GetComponent<UI_Common_ItemIcon>().ItemID = "";
			HuiShouItemList[spaceNum].GetComponent<UI_Common_ItemIcon>().UpdateStatus = true;
			HuiShouItemList[spaceNum].GetComponent<UI_Common_ItemIcon>().ItemEquipTipsType = "";
			//HuiShouItemList[spaceNum].SetActive(false);
		}
	}


	public void ClickHuiShou(){
		//Debug.Log ("huishou");

        if (huiShouStatus) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_205");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("正在处理上次回收数据!请稍后...");
            return;
        }


        if (HuiShouGetItemIDStr == ""|| HuiShouGetItemNumStr=="") {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_206");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请放入要回收的装备!");
            //清理显示
            ClearnPut();
            return;
        }

        //判断当前道具奖励的种类是否满足背包剩余格子的需求；
        string[] huiShouGetItemIDStrList = HuiShouGetItemIDStr.ToString().Split(',');
        string[] huiShouGetItemNumStrList = HuiShouGetItemNumStr.ToString().Split(',');
        if (Game_PublicClassVar.Get_function_Rose.BagNullNum() < huiShouGetItemIDStrList.Length)
        {

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_207");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_208");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + huiShouGetItemIDStrList.Length + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你的背包需要剩余最少" + huiShouGetItemIDStrList.Length + "个位置!");
            //清理显示
            ClearnPut();
            return;
        }

        //无法回收装备等级高于自己的超过10级的装备
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        for (int i = 0; i < HuiShouBagNumList.Length; i++)
        {
            if (HuiShouBagNumList[i] != "" && HuiShouBagNumList[i] != "0")
            {
                //判定装备是否有橙色
                string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", HuiShouBagNumList[i], "RoseBag");
                string itemLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", itemID, "Item_Template");
                if (itemLv == "" || itemLv == null) {
                    itemLv = "0";
                }

                int lvCha = int.Parse(itemLv) - roseLv;
                if (lvCha >= 15) {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_209");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("回收物品超过自身等级15级,无法回收!");
                    return;
                }

                if (itemID == "" || itemID == null || itemID == "0") {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("回收熔炉过热,请重新打开放入回收装备");
                    //清理显示
                    ClearnPut();
                    return;
                }
            }
        }


        //回收状态
        huiShouStatus = true;

        //判定回收到道具是否有宝石镶嵌
        bool huishouHint = false;

        //删除回收的装备
        for (int i = 0; i < HuiShouBagNumList.Length; i++)
        {
            if (HuiShouBagNumList[i] != "" && HuiShouBagNumList[i] != "0")
            {
                //判定装备是否有橙色HuiShouBagNumList[i]
                string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", HuiShouBagNumList[i], "RoseBag");
                string equipQualityStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", itemID, "Item_Template");
                if (equipQualityStr != null && equipQualityStr != "") {
                    int equipQuality = int.Parse(equipQualityStr);
                    if (equipQuality >= 5)
                    {
                        huishouHint = true;
                        break;
                    }
                }

                //判定装备是否有宝石
                if (Game_PublicClassVar.Get_function_Rose.GetBagGemStatus(HuiShouBagNumList[i])) {
                    huishouHint = true;
                    break;
                }
            }
        }


        if (huishouHint)
        {
            //关闭UI背景图片
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);
            //弹出提示
            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
            //string jieshaoStr = "是否确认回收这些装备！\n(提示:当前装备上附带宝石或回收的装备为橙色级别以上装备!)";
            string jieshaoStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_15");
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_4");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_5");
            string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_6");
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, HuiShou, null, langStrHint_1, langStrHint_2, langStrHint_3);
            //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, HuiShou, null, "回收确认", "回收", "取消");
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
        }
        else {

            HuiShou();
        }

        huiShouStatus = false;
    }

    //触发回收
    public void HuiShou() {

        //判断当前道具奖励的种类是否满足背包剩余格子的需求；
        string[] huiShouGetItemIDStrList = HuiShouGetItemIDStr.ToString().Split(',');
        string[] huiShouGetItemNumStrList = HuiShouGetItemNumStr.ToString().Split(',');

        //删除回收的装备
        for (int i = 0; i < HuiShouBagNumList.Length; i++)
        {
            if (HuiShouBagNumList[i] != "" && HuiShouBagNumList[i] != "0")
            {
                Game_PublicClassVar.Get_function_Rose.DeleteBagSpaceItem(HuiShouBagNumList[i]);
            }
        }

        //发送回收道具
        if (huiShouGetItemIDStrList[0] != "" && huiShouGetItemIDStrList[0] != "0")
        {
            for (int i = 0; i < huiShouGetItemIDStrList.Length; i++)
            {
                //发送道具
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(huiShouGetItemIDStrList[i], int.Parse(huiShouGetItemNumStrList[i]),"0",0,"0",true,"24");
            }
        }

        ClearnPut();



    }
}
