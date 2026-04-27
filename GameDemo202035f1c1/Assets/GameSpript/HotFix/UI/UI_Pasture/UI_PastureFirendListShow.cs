using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PastureFirendListShow : MonoBehaviour {

    public string FirendDataStr;

    public string FirendDataID;
    public GameObject Obj_HeadIcon;
    public GameObject Obj_Name;
    public GameObject Obj_LvDes;
    public GameObject Obj_Par;

    public bool JiaoHuStatus;
    public GameObject Obj_SucPro_DaSao;
    public GameObject Obj_SucPro_TanSuo;
    public GameObject Obj_SucPro_TouShi;

    public GameObject Obj_Btn_DaSao;
    public GameObject Obj_Btn_TanSuo;
    public GameObject Obj_Btn_TouShi;
    public GameObject Obj_HintObj;

    public string pastureLv;
    public string sucPro_DaSao;
    public string sucPro_TanSuo;
    public string sucPro_TouShi;


    // Use this for initialization
    void Start () {

        //读取数据
        //

        this.transform.localScale = new Vector3(1, 1, 1);

    }
	
	// Update is called once per frame
	void Update () {
		


	}

    public void Init() {

        string[] FirendDataList = FirendDataStr.Split(';');
        FirendDataID = FirendDataList[0];
        string name = FirendDataList[2];
        string occ = FirendDataList[3];
        pastureLv = FirendDataList[4];
        sucPro_DaSao = FirendDataList[5];
        sucPro_TanSuo = FirendDataList[6];
        sucPro_TouShi = FirendDataList[7];

        if (FirendDataList[1] == "0")
        {
            JiaoHuStatus = false;
        }
        else {
            JiaoHuStatus = true;
        }

        if (sucPro_DaSao == "")
        {
            sucPro_DaSao = "0";
        }
        if (sucPro_TanSuo == "")
        {
            sucPro_TanSuo = "0";
        }
        if (sucPro_TouShi == "")
        {
            sucPro_TouShi = "0";
        }

        if (pastureLv == "0" || pastureLv == "" || pastureLv == null)
        {
            pastureLv = "1";
        }
        else {
            pastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", pastureLv, "PastureUpLv_Template");
        }

        Obj_Name.GetComponent<Text>().text = name;
        Obj_LvDes.GetComponent<Text>().text = "家园" + pastureLv + "级";
        Obj_SucPro_DaSao.GetComponent<Text>().text = (float.Parse(sucPro_DaSao) * 100).ToString("F0") + "%";
        Obj_SucPro_TanSuo.GetComponent<Text>().text = (float.Parse(sucPro_TanSuo) * 100).ToString("F0") + "%";
        Obj_SucPro_TouShi.GetComponent<Text>().text = (float.Parse(sucPro_TouShi) * 100).ToString("F0") + "%";

        //显示头像
        string headIconID = "10001";
        switch (occ)
        {
            case "1":
                headIconID = "10001";
                break;

            case "2":
                headIconID = "10002";
                break;
        }
        object obj = Resources.Load("HeadIcon/PlayerIcon/" + headIconID, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_HeadIcon.GetComponent<Image>().sprite = itemIcon;
        BtnShow();

    }

    private void BtnShow() {

        //交互按钮是否显示
        if (JiaoHuStatus)
        {
            Obj_Btn_DaSao.SetActive(false);
            Obj_Btn_TanSuo.SetActive(false);
            Obj_Btn_TouShi.SetActive(false);
            Obj_HintObj.SetActive(true);


        }
        else
        {
            Obj_Btn_DaSao.SetActive(true);
            Obj_Btn_TanSuo.SetActive(true);
            Obj_Btn_TouShi.SetActive(true);
            Obj_HintObj.SetActive(false);
        }

    }

    //打扫
    public void Btn_DaSao() {

        if (pastureLv == "") {
            return;
        }

        //判定交互次数是否达到上限
        if (IfJiaoHuNum() == false) {
            return;
        }

        //增加一次交互次数
        Game_PublicClassVar.Get_function_Pasture.PastureAddJiaoHuNum(1);


        string nowPastureLvID = (10000 + int.Parse(pastureLv)).ToString();
        if (Random.value <= float.Parse(sucPro_DaSao)) {

            //随机额度进行发送
            string PastureFirendListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaSaoGold", "ID", nowPastureLvID, "PastureUpLv_Template");
            int sendValue = int.Parse(PastureFirendListStr);
            sendValue = (int)(sendValue * (0.5f + Random.value * 0.5f));        //随机值
            Game_PublicClassVar.Get_function_Rose.SendReward("5", sendValue.ToString());


        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("很遗憾!你并没有打扫成功...");
        }

        //记录当前数据不能进行交互
        Game_PublicClassVar.Get_function_Pasture.PastureWriteJiaoHuStatus(FirendDataID);


        //刷新显示
        Obj_Par.GetComponent<UI_PastureFirendListSet>().Show();
        JiaoHuStatus = true;
        BtnShow();

    }

    //探索
    public void Btn_TanSuo() {


        if (pastureLv == "")
        {
            return;
        }

        //判定交互次数是否达到上限
        if (IfJiaoHuNum() == false)
        {
            return;
        }

        //判断仓库是否满了
        if (Game_PublicClassVar.Get_function_Pasture.ReturnNullSpaceNum() == "-1")
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("家园仓库已满!");
            return;
        }

        //增加一次交互次数
        Game_PublicClassVar.Get_function_Pasture.PastureAddJiaoHuNum(1);


        string nowPastureLvID = (10000 + int.Parse(pastureLv)).ToString();
        if (Random.value <= float.Parse(sucPro_TanSuo))
        {
            //随机额度进行发送
            string danID = Game_PublicClassVar.Get_function_Pasture.PastureLvGetDanID(nowPastureLvID);

            Game_PublicClassVar.Get_function_Pasture.SendPastureBag(danID, 1);



        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("很遗憾!你并没有搜索成功...");
        }


        //记录当前数据不能进行交互
        Game_PublicClassVar.Get_function_Pasture.PastureWriteJiaoHuStatus(FirendDataID);


        //刷新显示
        Obj_Par.GetComponent<UI_PastureFirendListSet>().Show();
        JiaoHuStatus = true;
        BtnShow();

    }

    //投食
    public void Btn_TouShi() {

        if (pastureLv == "")
        {
            return;
        }

        //判定交互次数是否达到上限
        if (IfJiaoHuNum() == false)
        {
            return;
        }

        //判断仓库是否满了
        /*
        if (Game_PublicClassVar.Get_function_Pasture.GetPasturePeopleNum() == "-1")
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("家园仓库已满!");
            return;
        }
        */

        //增加一次交互次数
        Game_PublicClassVar.Get_function_Pasture.PastureAddJiaoHuNum(1);


        string nowPastureLvID = (10000 + int.Parse(pastureLv)).ToString();
        if (Random.value <= float.Parse(sucPro_TouShi))
        {
            //随机额度进行发送
            string dongwuID = Game_PublicClassVar.Get_function_Pasture.PastureLvGetDongWuID(nowPastureLvID);

            Game_PublicClassVar.Get_function_Pasture.CreatePastureAI(dongwuID);

   


            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你!投放的食物成功牵引了对方农场的动物来到你的农场!");
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("很遗憾!你的投食并没有吸引到牧场动物的注意...");
        }

        //刷新显示
        //记录当前数据不能进行交互
        Game_PublicClassVar.Get_function_Pasture.PastureWriteJiaoHuStatus(FirendDataID);

        Obj_Par.GetComponent<UI_PastureFirendListSet>().Show();
        JiaoHuStatus = true;
        BtnShow();
    }

    //是否可以交互
    public bool IfJiaoHuNum() {

        //判定交互次数是否达到上限
        string jiaohuNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JiaoHuNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (jiaohuNum == "")
        {
            jiaohuNum = "0";
        }
        if (int.Parse(jiaohuNum) >= 10)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("交互次数是否已经达到上限");
            return false;
        }

        return true;
    }
}
