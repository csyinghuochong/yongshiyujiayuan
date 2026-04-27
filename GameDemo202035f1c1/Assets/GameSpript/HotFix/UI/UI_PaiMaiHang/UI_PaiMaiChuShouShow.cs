using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PaiMaiChuShouShow : MonoBehaviour {

	public ObscuredString Sale_ItemID;
	public ObscuredInt BagSpace;
	public ObscuredInt Sale_Gold;
	public ObscuredInt Sale_NowGold;
	public ObscuredInt sale_ItemNum;
	public ObscuredFloat saleAddValuePro;


	public GameObject SaleItemSet;
	public GameObject Obj_SaleItem;
	public GameObject Obj_ItemName;
	public GameObject Obj_TuiJianSaleGld;
	public GameObject Obj_NowSaleGold;
	public GameObject Obj_SaleItemNum;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowSale(){
		
		//获取道具数据
		string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", Sale_ItemID, "Item_Template");
		Obj_ItemName.GetComponent<Text> ().text = itemName;
		Obj_SaleItemNum.GetComponent<Text> ().text = sale_ItemNum.ToString();
        //更新出售金币
        ShowNowSaleGold ();
		Obj_TuiJianSaleGld.GetComponent<Text> ().text = Sale_Gold.ToString();

		//显示道具
		Game_PublicClassVar.Get_function_UI.DestoryTargetObj (SaleItemSet);

		//展示道具图标
		GameObject itemObj = (GameObject)Instantiate(Obj_SaleItem);
		itemObj.transform.SetParent(SaleItemSet.transform);
		itemObj.transform.localScale = new Vector3(1, 1, 1);
		itemObj.GetComponent<UI_Common_ItemIcon>().ItemID = Sale_ItemID ;
		itemObj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = 0;
		itemObj.GetComponent<UI_Common_ItemIcon>().UpdateItem();
		itemObj.transform.localPosition = Vector3.zero;

	}

	public void ShowNowSaleGold(){
	
		//显示出售的金币
		Sale_NowGold = (int)(Sale_Gold * (1 + saleAddValuePro));
		string fuhaoStr = "";
		if (saleAddValuePro >= 0) {
			fuhaoStr = "+";
		}

		//Obj_NowSaleGold.GetComponent<Text> ().text = Sale_NowGold.ToString() + "(" + fuhaoStr + saleAddValuePro * 100 + "%)";
		Obj_NowSaleGold.GetComponent<Text> ().text = Sale_NowGold.ToString();
    }


	//增减出售金币
	public void Btn_SaleGold_Add(){

		saleAddValuePro = saleAddValuePro + 0.1f;
		if (saleAddValuePro >= 1) {
			saleAddValuePro = 1;
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_51");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("最高只能加价至100%");
		}
		ShowNowSaleGold ();
	}


	//减少出售金币
	public void Btn_SaleGold_Cost(){
		
		saleAddValuePro = saleAddValuePro - 0.1f;
		if (saleAddValuePro <= -0.9f) {
			saleAddValuePro = -0.9f;
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_52");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("最高只能1折出售");
		}
		ShowNowSaleGold ();
	}

	//出售道具按钮
	public void Btn_SaleItem(){

        //计入出售次数
        string sellnum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellItemNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (sellnum == "")
        {
            sellnum = "0";
        }
        if (int.Parse(sellnum) >= 20)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_450"));
            return;
        }


        Debug.Log ("我点击了出售按钮！");
		if (Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus) {

			//扣除道具
			bool costStatus = Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(Sale_ItemID, sale_ItemNum, BagSpace.ToString(), false);
			//扣除状态
			if (costStatus) {
				//服务器收到消息好进行发送回馈再扣除数据
				Pro_PaiMai_Sell pro_PaiMai_Sell = new Pro_PaiMai_Sell ();
				string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                /*
				pro_PaiMai_Sell.ZhangHaoID = zhanghaoID;
				pro_PaiMai_Sell.PaiMaiItemID = Sale_ItemID;
				pro_PaiMai_Sell.PaiMaiItemNum = sale_ItemNum.ToString();
				pro_PaiMai_Sell.GoldType = "1";
				pro_PaiMai_Sell.GoldValue = Sale_NowGold.ToString ();
                */

                pro_PaiMai_Sell.ZhangHaoID = Game_PublicClassVar.Get_xmlScript.Encrypt(zhanghaoID);
                pro_PaiMai_Sell.PaiMaiItemID = Game_PublicClassVar.Get_xmlScript.Encrypt(Sale_ItemID);
                pro_PaiMai_Sell.PaiMaiItemNum = Game_PublicClassVar.Get_xmlScript.Encrypt(sale_ItemNum.ToString());
                pro_PaiMai_Sell.GoldType = Game_PublicClassVar.Get_xmlScript.Encrypt("1");
                pro_PaiMai_Sell.GoldValue = Game_PublicClassVar.Get_xmlScript.Encrypt(Sale_NowGold.ToString());
                pro_PaiMai_Sell.SendTimeStr = Game_PublicClassVar.Get_xmlScript.Encrypt(WWWSet.GetTimeStamp());

                //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001073, pro_PaiMai_Sell);
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001079, pro_PaiMai_Sell);
                //关闭界面
                Destroy (this.gameObject);
				//清空选择框
				Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelect = "";
				Game_PublicClassVar.Get_game_PositionVar.RoseBagSpaceSelectStatus = true;

                //重新请求拍卖行出售列表
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001084, zhanghaoID);

                /*
                //出售价值大于50W的道具会上传自身的存档
                if (Sale_NowGold >= 500000)
                {
                    //获取唯一ID
                    //zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    //每次进入游戏上传一次玩家数据
                    string[] saveList = new string[] { "", "2", "预留设备号位置" };
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001006, saveList);

                }
                */
            } else {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_57");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
				//Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("出售数量与实际不匹配");
				//关闭界面
				Destroy(this.gameObject);
			}
		} else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_58");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
			//Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("请检查服务器是否正常连接");
			//关闭界面
			Destroy(this.gameObject);
		}




    }


	public void CloseUI(){
	
		Destroy (this.gameObject);
	}
}
