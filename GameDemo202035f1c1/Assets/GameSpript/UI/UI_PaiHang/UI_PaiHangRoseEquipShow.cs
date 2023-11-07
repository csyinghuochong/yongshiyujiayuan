using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_PaiHangRoseEquipShow : MonoBehaviour
{

    public string roseName;
    public string roseLv;
    public string roseOcc;              //1表示战士  2:表示法师
    public string nowYanSeID;
    public string nowNowYanSeHairID;

    public string[] roseEquipIDList;
    //public Dictionary<string, string> roseEquipIDDic = new Dictionary<string, string>();            //装备ID,隐藏属性ID
    public Dictionary<string, string> roseEquipHideDic = new Dictionary<string, string>();          //隐藏属性ID,隐藏值

    public Transform EquipList;

    //子级的控件列表
    public GameObject[] EquipShowList;

    private GameObject gameStartVar;
    private Game_PositionVar game_PositionVar;

    public GameObject Obj_RoseName;
    public GameObject Obj_RoseLv;
    public GameObject Obj_RoseExp;



	// Use this for initialization
	void Start () {

        for (int i = 1; i <= 13; i++) {
            EquipShowList[i - 1] = EquipList.Find("Equip_" + i).gameObject;
        }

        //打开摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ShowPlayerRoseSet.SetActive(true);

    }
	
	// Update is called once per frame
	void Update () {

	}

    private void OnDestroy()
    {
        //关闭摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_ShowPlayerRoseSet.SetActive(false);
    }

    //显示装备模块
    public void ShowEquip() { 

        Obj_RoseName.GetComponent<Text>().text = roseName;
        Obj_RoseLv.GetComponent<Text>().text = "Lv." + roseLv;
        for (int i = 1; i <= 13; i++) {
            string equipID = roseEquipIDList[i-1].Split('@')[0];
            //Debug.Log("equipID = " + equipID);
            if (equipID != "0" && equipID!="")
            {
                string equipHideID = roseEquipIDList[i - 1].Split('@')[1];
                //Debug.Log("equipHideID = " + equipHideID);
                string equipHideStr = "";
                if (equipHideID != "0" && equipHideID != "") {
                    if (roseEquipHideDic.ContainsKey(equipHideID))
                    {
                        equipHideStr = roseEquipHideDic[equipHideID];
                    }
                    else {
                        //Debug.Log("查询不到值");
                    }
                }

                //获取宝石信息
                if (roseEquipIDList[i - 1].Split('@').Length >= 5) {
                    string gemHoleStr = roseEquipIDList[i - 1].Split('@')[2];
                    string gemIDStr = roseEquipIDList[i - 1].Split('@')[3];
                    string equipQiangHuaID = roseEquipIDList[i - 1].Split('@')[4];
                    //获取宝石信息并显示
                    UpdateRoseEquipDate(EquipShowList[i - 1], equipID, equipHideStr, gemIDStr, gemHoleStr,roseOcc);
                }

                //显示武器
                if (i == 1) {
                    string itemMondel = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemMondel", "ID", equipID, "Item_Template",roseOcc);
                    Game_PublicClassVar.Get_function_Rose.PlayerRoseWeaponModelUIShow(itemMondel);
                }
            }
        }

        //显示模型
        //Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().showOccModel(roseOcc);

        //显示外观
        //nowYanSeID = "10002";
        string nowYanSe = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeID", "ID", nowYanSeID, "RanSe_Template");
        Game_PublicClassVar.Get_function_Rose.RoseModelChange(nowYanSe, nowYanSeID, true, "1", this.gameObject, roseOcc);
        //nowNowYanSeHairID = "30003";
        string nowYanSeHair = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeID", "ID", nowNowYanSeHairID, "RanSe_Template");
        Game_PublicClassVar.Get_function_Rose.RoseModelChange(nowYanSeHair, nowYanSeID, true, "2", this.gameObject, roseOcc);
    }


    //显示装备数据
	void UpdateRoseEquipDate(GameObject RoseEquipSpaceName, string equipID,string showEquipHideStr,string gemIDStr,string gemHoleStr,string roseOcc)
    {
        RoseEquipSpaceName.GetComponent<UI_PaiHangRoseEquipItemShow>().EquipID = equipID;
        RoseEquipSpaceName.GetComponent<UI_PaiHangRoseEquipItemShow>().HideStr = showEquipHideStr;
		RoseEquipSpaceName.GetComponent<UI_PaiHangRoseEquipItemShow>().GemIDStr = gemIDStr;
		RoseEquipSpaceName.GetComponent<UI_PaiHangRoseEquipItemShow>().GemHoleStr = gemHoleStr;
        RoseEquipSpaceName.GetComponent<UI_PaiHangRoseEquipItemShow>().UpdataStatus = true;
        RoseEquipSpaceName.GetComponent<UI_PaiHangRoseEquipItemShow>().roseOcc = roseOcc;
    }

	public void CloseUI(){
        Destroy (this.gameObject);
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
	}

    void OnDestory() {
        //显示模型
        //Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.GetComponent<CameraModel>().showModel();
        //Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.SetActive(false);
    }

    public void ClearnTips() {
        Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
    }
}
