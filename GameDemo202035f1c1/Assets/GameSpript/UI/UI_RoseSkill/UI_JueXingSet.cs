using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_JueXingSet : MonoBehaviour {

    public ObscuredString nowJueXingID;

    public GameObject Obj_JueXingJiHuoIconShow;
    public GameObject[] Obj_JueXingJiHuoIconShowPositionList;

    public GameObject Obj_JueXingMiaoShuList;
    public GameObject Obj_JueXingMiaoShuListSet;

    public GameObject Obj_JueXingExpImg;
    public GameObject Obj_JueXingExpText;

    public GameObject Obj_JueXingNeedItemNum;
    public GameObject Obj_JueXingNeedGoldNum;

    // Use this for initialization
    void Start () {

        nowJueXingID = "10002";
        Btn_SelectJueXing();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //显示指定的觉醒信息
    public void Btn_SelectJueXing() {

        string juexingIDStr = "10002;10003;10004;10005;10006;10007;10008";
        string[] juexingIDList = juexingIDStr.Split(';');

        for (int i = 0; i < Obj_JueXingJiHuoIconShowPositionList.Length; i++) {
            //删除
            Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_JueXingJiHuoIconShowPositionList[i]);
            //新建
            GameObject obj = (GameObject)Instantiate(Obj_JueXingJiHuoIconShow);
            obj.transform.SetParent(Obj_JueXingJiHuoIconShowPositionList[i].transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = Vector3.zero;
            obj.GetComponent<UI_JueXingJiHuoIconSet>().Obj_Par = this.gameObject;
            obj.GetComponent<UI_JueXingJiHuoIconSet>().JueXingID = juexingIDList[i];
        }

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_JueXingMiaoShuListSet);

        string desStrOcc = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProListDes", "ID", nowJueXingID, "JueXing_Template");
        string desStr = desStrOcc.Split('@')[0];
        if (desStrOcc.Split('@').Length>=2) {
            switch (Game_PublicClassVar.Get_function_Rose.GetRoseOcc())
            {
                case "1":
                    desStr = desStrOcc.Split('@')[0];
                    break;
                case "2":
                    desStr = desStrOcc.Split('@')[1];
                    break;
                case "3":
                    desStr = desStrOcc.Split('@')[2];
                    break;
            }
        }

        string[] desStrList = desStr.Split(';');
        for (int i = 0; i < desStrList.Length; i++) {

            GameObject juexingObjList = (GameObject)Instantiate(Obj_JueXingMiaoShuList);
            juexingObjList.transform.SetParent(Obj_JueXingMiaoShuListSet.transform);
            juexingObjList.transform.localScale = new Vector3(1, 1, 1);
            juexingObjList.transform.GetComponent<UI_JueXingProDes>().Obj_ProDes.GetComponent<Text>().text = desStrList[i];

        }

        //显示经验
        string juexingExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (juexingExp == ""|| juexingExp==null) {
            juexingExp = "0";
        }
        string upExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Exp", "ID", nowJueXingID, "JueXing_Template");
        Obj_JueXingExpImg.GetComponent<Image>().fillAmount = float.Parse(juexingExp) / float.Parse(upExp);
        Obj_JueXingExpText.GetComponent<Text>().text = juexingExp + "/" + upExp;

        //显示道具数量
        string costItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostItemNum", "ID", nowJueXingID, "JueXing_Template");
        int nowItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10000032");
        Obj_JueXingNeedItemNum.GetComponent<Text>().text = nowItemNum + "/" + costItemNum;

        if (nowItemNum >= int.Parse(costItemNum))
        {
            Obj_JueXingNeedItemNum.GetComponent<Text>().color = new Color(0, 0.5f, 0);
        }
        else
        {
            Obj_JueXingNeedItemNum.GetComponent<Text>().color = Color.red;
        }


        //显示金币
        string costGoldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostGoldNum", "ID", nowJueXingID, "JueXing_Template");
        long nowGoldNum = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
        string costGoldNumStr = costGoldNum.ToString();
        string nowGoldNumStr = nowGoldNum.ToString();

        if (long.Parse(nowGoldNumStr) >= long.Parse(costGoldNumStr))
        {
            Obj_JueXingNeedGoldNum.GetComponent<Text>().color = new Color(0, 0.5f, 0);
        }
        else {
            Obj_JueXingNeedGoldNum.GetComponent<Text>().color = Color.red;
        }

        if (int.Parse(costGoldNum) >= 1000000) {
            costGoldNumStr = (long)(long.Parse(costGoldNum) / 10000) + "万";
        }

        if (nowGoldNum >= 100000) {
            nowGoldNumStr = (long)(long.Parse(nowGoldNumStr) / 10000) + "万";
        }

        Obj_JueXingNeedGoldNum.GetComponent<Text>().text = nowGoldNumStr + "/" + costGoldNumStr;

    }


    //升级觉醒
    public void Btn_JueXingUpLv() {

        //获取当前觉醒ID是否已经激活
        string jihuoIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (jihuoIDStr.Contains(nowJueXingID)) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前觉醒已经激活!");
            return;
        }

        //获取前置
        string jihuoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JiHuoID", "ID", nowJueXingID, "JueXing_Template");
        if (jihuoID != "" && jihuoID != "0" && jihuoID != null)
        {
            if (jihuoIDStr.Contains(jihuoID) == false)
            {
                string name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", jihuoID, "JueXing_Template");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要先激活:" + name);
                return;
            }
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先完成系列任务");
        }

        //获取
        //显示经验
        string juexingExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (juexingExp == "" || juexingExp == null)
        {
            juexingExp = "0";
        }
        string upExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Exp", "ID", nowJueXingID, "JueXing_Template");
        int writeExp = 0;
        if (int.Parse(juexingExp) >= int.Parse(upExp))
        {
            writeExp = int.Parse(juexingExp) - int.Parse(upExp);
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("觉醒经验不足!");
            return;
        }

        //显示道具数量
        string costItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostItemNum", "ID", nowJueXingID, "JueXing_Template");
        int nowItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10000032");
        int writeItemNum = 0;
        if (nowItemNum >= int.Parse(costItemNum))
        {
            writeItemNum = nowItemNum - int.Parse(costItemNum);
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("觉醒道具不足!");
            return;
        }

        //显示金币
        string costGoldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostGoldNum", "ID", nowJueXingID, "JueXing_Template");
        long nowGoldNum = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
        long writeGoldNum = 0;
        if (nowGoldNum >= long.Parse(costGoldNum))
        {
            writeGoldNum = nowGoldNum - long.Parse(costGoldNum);
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("金币不足!");
            return;
        }

        //扣除相关道具
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JueXingExp", writeExp.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_Rose.CostBagItem("10000032", int.Parse(costItemNum));
        Game_PublicClassVar.Get_function_Rose.CostReward("1", costGoldNum);

        //激活
        if (jihuoIDStr == "" || jihuoIDStr == null || jihuoIDStr == "0")
        {
            jihuoIDStr = nowJueXingID;
        }
        else {
            jihuoIDStr = jihuoIDStr + ";" + nowJueXingID;
        }

        //写入成就
        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("250", nowJueXingID, "1");

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("JueXingJiHuoID", jihuoIDStr,"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //激活外观显示
        if (jihuoIDStr.Contains("10008")) {
            Game_PublicClassVar.Get_function_Rose.RoseYanSeAdd("20001");
            Game_PublicClassVar.Get_function_Rose.RoseYanSeAdd("40001");
        }

        //刷新显示
        Btn_SelectJueXing();

        //刷新属性
        Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty(true);

    }

}
