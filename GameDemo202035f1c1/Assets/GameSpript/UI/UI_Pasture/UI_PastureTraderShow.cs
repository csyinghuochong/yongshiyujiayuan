using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureTraderShow : MonoBehaviour {

    public ObscuredString PastureTraderID;
    public GameObject Obj_PastureTraderHeadIcon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //初始化
    public void Init() {

        string[] pastureTraderList = Game_PublicClassVar.Get_function_Pasture.GetPastureTraderIDData(int.Parse(PastureTraderID));
        string npcHeadIconID = pastureTraderList[0];
        object obj = Resources.Load("HeadIcon/PastureTraderIcon/" + npcHeadIconID, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_PastureTraderHeadIcon.GetComponent<Image>().sprite = itemIcon;
    }

    //打开
    public void Btn_Open() {

        //牧场商人出售
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_PastureNpcBuySet_Open != null)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_PastureNpcBuySet_Open.GetComponent<UI_PastureNpcBuySet>().ClearnShow();
            Game_PublicClassVar.Get_game_PositionVar.Obj_PastureNpcBuySet_Open.GetComponent<UI_PastureNpcBuySet>().PastureTraderID = PastureTraderID;
            Game_PublicClassVar.Get_game_PositionVar.Obj_PastureNpcBuySet_Open.GetComponent<UI_PastureNpcBuySet>().ShowNpcBuyData();
        }
    }
    
}
