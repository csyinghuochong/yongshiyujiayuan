using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Npc_DuiHuanShenShou : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Btn_DuiHuanShenShou() {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);

        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_2");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, DuiHuanShenShou, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否消耗100神兽碎片兑换神兽？", DuiHuanShenShou, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    //兑换神兽
    public void DuiHuanShenShou() {

        //判断当前是否有宠物空位
        int nullPetNum = Game_PublicClassVar.Get_function_AI.Pet_ReturnPetFirstNull();
        if (nullPetNum == -1)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_5");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物位置已满！");
            return;
        }

        //判定自身是否有神兽碎片
        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10010087");
        if (itemNum >= 100)
        {
            if (Game_PublicClassVar.Get_function_Rose.CostBagItem("10010087", 100)) {
                //发送神兽
                Game_PublicClassVar.Get_function_AI.Pet_Create(nullPetNum.ToString(), "30000002", "1");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_6");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你获得传说神兽!!!");
            }
        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_7");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("神兽碎片不足！");
        }
    }


    public void Btn_DuiHuanShenShou_LongLong()
    {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);

        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_2");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, DuiHuanShenShou_LongLong, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否消耗100神兽碎片兑换神兽？", DuiHuanShenShou, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    //兑换神兽
    public void DuiHuanShenShou_LongLong()
    {

        //判断当前是否有宠物空位
        int nullPetNum = Game_PublicClassVar.Get_function_AI.Pet_ReturnPetFirstNull();
        if (nullPetNum == -1)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_5");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物位置已满！");
            return;
        }

        //判定自身是否有神兽碎片
        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10010087");
        if (itemNum >= 100)
        {
            if (Game_PublicClassVar.Get_function_Rose.CostBagItem("10010087", 100))
            {
                //发送神兽
                Game_PublicClassVar.Get_function_AI.Pet_Create(nullPetNum.ToString(), "30000003", "1");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_6");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你获得传说神兽!!!");
            }
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_7");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("神兽碎片不足！");
        }
    }

    public void Btn_DuiHuanShenShou_HuaHua()
    {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);

        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_2");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, DuiHuanShenShou_HuanHuan, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否消耗100神兽碎片兑换神兽？", DuiHuanShenShou, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    //兑换神兽
    public void DuiHuanShenShou_HuanHuan()
    {

        //判断当前是否有宠物空位
        int nullPetNum = Game_PublicClassVar.Get_function_AI.Pet_ReturnPetFirstNull();
        if (nullPetNum == -1)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_5");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物位置已满！");
            return;
        }

        //判定自身是否有神兽碎片
        int itemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10010087");
        if (itemNum >= 100)
        {
            if (Game_PublicClassVar.Get_function_Rose.CostBagItem("10010087", 100))
            {
                //发送神兽
                Game_PublicClassVar.Get_function_AI.Pet_Create(nullPetNum.ToString(), "30000004", "1");
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_6");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你获得传说神兽!!!");
            }
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_7");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("神兽碎片不足！");
        }
    }
}
