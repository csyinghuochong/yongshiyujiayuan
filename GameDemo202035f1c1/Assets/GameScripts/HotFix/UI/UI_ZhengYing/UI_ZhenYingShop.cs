using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_ZhenYingShop : MonoBehaviour
{
    public GameObject Obj_ZhenYingShopList;
    public GameObject Obj_ZhenYingShopPar;
    public ObscuredString RewardStr;
    // Start is called before the first frame update
    void Start()
    {
        RewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ZhenYingShop", "GameMainValue");
        //RewardStr = "10000027,1,15;10000028,1,100;10000029,1,100;10000030,1,100;10000061,1,30;10000062,1,100;10000063,1,100;10000064,1,100;10000065,1,30;10000066,1,5";
        Init();
    }

    //初始化
    public void Init() {

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ZhenYingShopPar);

        string[] RewardStrList = RewardStr.ToString().Split(';');

        //实例化
        for (int i = 0; i < RewardStrList.Length; i++)
        {
            string[] RewardIDStrList = RewardStrList[i].ToString().Split(',');
            GameObject obj = (GameObject)Instantiate(Obj_ZhenYingShopList);
            obj.transform.SetParent(Obj_ZhenYingShopPar.transform);
            obj.transform.localScale = new Vector3(1f, 1f, 1f);
            obj.GetComponent<UI_StoreList_2>().ItemID = RewardIDStrList[0].ToString();
            obj.GetComponent<UI_StoreList_2>().ItemNum = RewardIDStrList[1].ToString();
            obj.GetComponent<UI_StoreList_2>().BuyUseItem = "10000057";
            obj.GetComponent<UI_StoreList_2>().BuyUseItemNum = int.Parse(RewardIDStrList[2].ToString());
            obj.GetComponent<UI_StoreList_2>().BuyType = "3";

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
