using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_RoseEquipQiangHuaIcon : MonoBehaviour
{

    public bool UpdataStatus;               //开启后更新数据
    public GameObject Obj_EquipSelect;
    public GameObject Obj_EquipIcon;   
    public string EquipSpaceNum;
    private ObscuredString EquipSelect;
    private ObscuredString qiangHuaID;
    public GameObject Obj_QiangHuaLv;


	// Use this for initialization
	void Start () {

        UpdataStatus = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (UpdataStatus)
        {
            UpdataStatus = false;
            qiangHuaID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaID", "ID", EquipSpaceNum, "RoseEquip");
            string qiangHuaLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaLv", "ID", qiangHuaID, "EquipQiangHua_Template");
            Obj_QiangHuaLv.GetComponent<Text>().text = "+" + qiangHuaLv;

            //更新选中显示
            string nowQiangHuaEquipSpaceID = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipQiangHuaSet.GetComponent<UI_RoseEquipQiangHua>().NowQiangHuaEquipSpaceID;
            if (nowQiangHuaEquipSpaceID == EquipSpaceNum)
            {
                SelectEquipIcon();
            }
            else {
                Obj_EquipSelect.SetActive(false);
            }
        }
	}

    public void Mouse_Click() {

        qiangHuaID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaID", "ID", EquipSpaceNum, "RoseEquip");

        UI_RoseEquipQiangHua ui_RoseEquipQiangHua = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().Obj_EquipQiangHuaSet.GetComponent<UI_RoseEquipQiangHua>();
        //更新右边强化界面显示
        ui_RoseEquipQiangHua.QiangHuaID = qiangHuaID;
        ui_RoseEquipQiangHua.UpdateStatus = true;
        //更新选中框
        ui_RoseEquipQiangHua.NowQiangHuaEquipSpaceID = EquipSpaceNum;
        ui_RoseEquipQiangHua.UpdateSelectStatus = true;

        ui_RoseEquipQiangHua.EquipSpace = EquipSpaceNum;
    }

    //选中状态取消
    public void SelectStatusCancle() {
        Obj_EquipSelect.SetActive(false);
    }

    //选中状态
    public void SelectEquipIcon() {
        //显示选中框
        Obj_EquipSelect.SetActive(true);
        /*
        string itemQuality = Game_PublicClassVar.Get_function_UI.ItemQualiytoPath("4");
        object Equipobj = Resources.Load(itemQuality, typeof(Sprite));
        Sprite itemQuility = Equipobj as Sprite;
        Obj_EquipSelect.GetComponent<Image>().sprite = itemQuility;
        */
    }

}
