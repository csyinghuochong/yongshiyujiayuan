using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_RoseEquipQiangHua : MonoBehaviour {

    public ObscuredString QiangHuaID;
    public ObscuredString EquipSpace;
    public bool UpdateStatus;

    //UI相关
    public GameObject Obj_CommonItemIconShow;
    public GameObject CostItemObjSet;               //消耗道具的位置
    public GameObject Obj_QiangHuapProShow;
    public GameObject QiangHuaProObjSet;            //强化属性的位置
    public GameObject Obj_QiangHuaStartIconShow;
    public GameObject QiangHuaStartIconShowSet;
    public GameObject Obj_QiangHuaGold;             //强化金币属性
    public GameObject Obj_QiangHuaSuccessPro;             //强化成功概率


    public GameObject[] QianHuaEquipList;
    public ObscuredString NowQiangHuaEquipSpaceID;
    public bool UpdateSelectStatus;
    public GameObject Obj_EquipSpaceName;           //强化位置的名字


	// Use this for initialization
	void Start () {
        //默认选中第一个装备
        NowQiangHuaEquipSpaceID = "1";
        QiangHuaID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaID", "ID", NowQiangHuaEquipSpaceID, "RoseEquip");
        UpdateStatus = true;
        QianHuaEquipList[0].GetComponent<UI_RoseEquipQiangHuaIcon>().Mouse_Click();
        QianHuaEquipList[0].GetComponent<UI_RoseEquipQiangHuaIcon>().UpdataStatus = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (UpdateStatus) {
            UpdateStatus = false;
            updataQiangHua();
        }

        if (UpdateSelectStatus) {
            UpdateSelectStatus = false;
            for (int i = 0; i < QianHuaEquipList.Length; i++) {
                QianHuaEquipList[i].GetComponent<UI_RoseEquipQiangHuaIcon>().UpdataStatus = true;
            }
        }
	}

    void updataQiangHua() {

        QiangHuaID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaID", "ID", NowQiangHuaEquipSpaceID, "RoseEquip");

        //清空属性和道具
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(QiangHuaProObjSet);
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(CostItemObjSet);
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(QiangHuaStartIconShowSet);

        //QiangHuaID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaID", "ID", EquipSpace, "RoseEquip");

        //显示强化名称
        string qianghuaName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSpaceName", "ID", QiangHuaID, "EquipQiangHua_Template");
        Obj_EquipSpaceName.GetComponent<Text>().text = qianghuaName;
        
        //显示强化消耗的道具
        string CostItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostItem", "ID", QiangHuaID, "EquipQiangHua_Template");
        Game_PublicClassVar.Get_function_UI.Common_2_CreateSonObj(Obj_CommonItemIconShow, CostItemObjSet, CostItemStr);

        //显示强化属性
        float equipPropreAdd = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipPropreAdd", "ID", QiangHuaID, "EquipQiangHua_Template"));
        GameObject qianghuaObj = (GameObject)Instantiate(Obj_QiangHuapProShow);
        qianghuaObj.transform.SetParent(QiangHuaProObjSet.transform);
        string nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", QiangHuaID, "EquipQiangHua_Template");
        if (nextID != "99999")
        {
            float nextEquipPropreAdd = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipPropreAdd", "ID", nextID, "EquipQiangHua_Template"));

            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("下级部位属性提升");
            qianghuaObj.GetComponent<UI_RoseEquipQiangHuaPro>().QiangHuaProStr = langStr + "：" + nextEquipPropreAdd * 100 + "%" + "<color=#059600> (+" +  ((nextEquipPropreAdd - equipPropreAdd)*100).ToString("F1") + "%)</color>";
        }
        else {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("当前部位属性提升");
            qianghuaObj.GetComponent<UI_RoseEquipQiangHuaPro>().QiangHuaProStr = langStr + "：" + equipPropreAdd * 100 + "%";
        }

        qianghuaObj.GetComponent<UI_RoseEquipQiangHuaPro>().UpdateProStatus = true;
        qianghuaObj.transform.localScale = new Vector3(1, 1, 1);

        //显示强化金币
        string costGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostGold", "ID", QiangHuaID, "EquipQiangHua_Template");
        long selfGold = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
        if (selfGold < int.Parse(costGold))
        {
            Obj_QiangHuaGold.GetComponent<Text>().color = Color.red;
            //Obj_QiangHuaGold.GetComponent<Text>().text = costGold + "(金币不足)";
            Obj_QiangHuaGold.GetComponent<Text>().text = costGold;
        }
        else {
            Obj_QiangHuaGold.GetComponent<Text>().color = Color.white;
            Obj_QiangHuaGold.GetComponent<Text>().text = costGold;
        }

        //显示强化概率
        string successPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SuccessPro", "ID", QiangHuaID, "EquipQiangHua_Template");
        string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("强化成功概率");
        Obj_QiangHuaSuccessPro.GetComponent<Text>().text = langStr_1 + "：" + (int)(float.Parse(successPro) * 100) + "%";

        //显示属性
        /*
        string addPropreListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropreListStr", "ID", QiangHuaID, "EquipQiangHua_Template");
        string[] addPropreList = addPropreListStr.Split(';');
        if(addPropreList[0]!=""&&addPropreList[0]!=""){
            for (int i = 0; i < addPropreList.Length; i++ )
            {
                string GetProStr = ShowPro(addPropreList[i]);
                GameObject qianghuaObj = (GameObject)Instantiate(Obj_QiangHuapProShow);
                qianghuaObj.transform.SetParent(QiangHuaProObjSet.transform);
                qianghuaObj.GetComponent<UI_RoseEquipQiangHuaPro>().QiangHuaProStr = GetProStr;
                qianghuaObj.GetComponent<UI_RoseEquipQiangHuaPro>().UpdateProStatus = true;
            }
        }
        */

        //显示更新等级图标
        string qiangHuaLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaLv", "ID", QiangHuaID, "EquipQiangHua_Template");
        for (int i = 0; i < int.Parse(qiangHuaLv); i++)
        {
            GameObject qiangHuaObj = (GameObject)Instantiate(Obj_QiangHuaStartIconShow);
            qiangHuaObj.transform.SetParent(QiangHuaStartIconShowSet.transform);
            qiangHuaObj.GetComponent<UI_QiangHuaStartIconShow>().QiangHuaLvStr = "1";
            qiangHuaObj.GetComponent<UI_QiangHuaStartIconShow>().UpdateStatus = true;
            //Debug.Log("qiangHuaLv = " + qiangHuaLv);
        }

        int exNum = 0;
        while (nextID != "99999") {

            //强化角色等级限制
            int upLvLimit =int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpLvLimit", "ID", nextID, "EquipQiangHua_Template"));
            if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= upLvLimit)
            {
                //角色可以升级
                GameObject qiangHuaObj = (GameObject)Instantiate(Obj_QiangHuaStartIconShow);
                qiangHuaObj.transform.SetParent(QiangHuaStartIconShowSet.transform);
                qiangHuaObj.GetComponent<UI_QiangHuaStartIconShow>().QiangHuaLvStr = "2";
                qiangHuaObj.GetComponent<UI_QiangHuaStartIconShow>().UpdateStatus = true;
            }
            else {
                //强化未达到角色等级要求
                GameObject qiangHuaObj = (GameObject)Instantiate(Obj_QiangHuaStartIconShow);
                qiangHuaObj.transform.SetParent(QiangHuaStartIconShowSet.transform);
                qiangHuaObj.GetComponent<UI_QiangHuaStartIconShow>().QiangHuaLvStr = "3";
                qiangHuaObj.GetComponent<UI_QiangHuaStartIconShow>().UpdateStatus = true;
            }
            nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "EquipQiangHua_Template");
            //防止因为BUG让程序加速
            exNum = exNum + 1;
            //此处最多循环100次
            if (exNum > 100) {
                break;
            }
        }
    }

    //根据传入的属性进行显示
    public string ShowPro(string getProStr) {

        string proStr = "";

        if (getProStr != "" && getProStr != "0") {
            string addProType = getProStr.Split(',')[0];
            string addProValue = getProStr.Split(',')[1];
            proStr = Game_PublicClassVar.Get_function_UI.GetProName(addProType) + addProValue;
        }

        return proStr;
    }


    //开始强化
    public void Btn_QiangHua() {

        bool ifQiangHuaSuccess = Game_PublicClassVar.Get_function_UI.EquipQiangHua(EquipSpace);
        //强化成功
        if (ifQiangHuaSuccess)
        {
            UpdateStatus = true;
            UpdateSelectStatus = true;
            //Debug.Log("强化成功");

            //写入成就 获取最低等级
            int minLv = 0;
            for (int i = 1; i <= 13; i++)
            {
                string qianghuaID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaID", "ID", i.ToString(), "RoseEquip");
                string qianghuaLvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaLv", "ID", qianghuaID, "EquipQiangHua_Template");
                if (qianghuaLvStr == "")
                {
                    qianghuaLvStr = "0";
                }

                if (i == 1)
                {
                    minLv = int.Parse(qianghuaLvStr);
                }

                if (int.Parse(qianghuaLvStr) < minLv)
                {
                    minLv = int.Parse(qianghuaLvStr);
                }
            }

            //写入成就(强化等级)
            Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("206", "0", minLv.ToString());

            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你!强化装备成功");

        }
        else {
            UpdateStatus = true;            //更新显示
        }
    }
}
