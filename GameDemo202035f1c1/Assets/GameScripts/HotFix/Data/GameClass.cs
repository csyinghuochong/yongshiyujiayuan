using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
public class GameClass : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

//新建一个拍卖行类（加密）
public class Pro_PaiMaiDataAdd
{

    public ObscuredString PaiMaiID;             //拍买ID

    public ObscuredString PaiMaiType;           //拍买类型

    public ObscuredString PaiMaiItemID;         //拍买道具ID

    public ObscuredString PaiMaiItemNum;        //拍买道具数量

    public ObscuredString GoldType;             //购买金币类型

    public ObscuredString GoldValue;            //购买金币值

    public ObscuredFloat GoldBianHuaValue;      //购买价格浮动值

    public ObscuredString GoldBianHuaValueStr;      //购买价格浮动值

}

//拍卖行购买信息

public class Pro_PaiMai_PlayerSellAdd
{

    public ObscuredString PaiMaiItemID;         //拍买道具ID

    public ObscuredString PaiMaiItemNum;        //拍买道具数量

    public ObscuredString GoldType;             //出售金币类型

    public ObscuredString GoldValue;            //出售金币值

    public ObscuredString SellTime;             //出售时间

    public ObscuredString SellID;             	//出售ID
}
