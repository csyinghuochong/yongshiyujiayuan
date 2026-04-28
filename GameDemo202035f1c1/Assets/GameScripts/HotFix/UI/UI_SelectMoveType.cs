using UnityEngine;
using System.Collections;

public class UI_SelectMoveType : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Btn_DianJi() {

        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = false;
        //存储状态
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        Btn_CloseUI();
    }

    public void Btn_YaoGan()
    {

        Game_PublicClassVar.Get_game_PositionVar.Obj_YaoGanSet.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus = true;
        //存储状态
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YaoGanStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        Btn_CloseUI();

    }

    //关闭
    public void Btn_CloseUI()
    {
        Destroy(this.gameObject);
    }

}
