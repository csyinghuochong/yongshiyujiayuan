using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PetHeCheng : MonoBehaviour {

    public GameObject Obj_HeChengShow;
    public GameObject Obj_HeChengListSet;

    public ObscuredString PetID_List_1;
    public ObscuredString PetID_List_2;

    public GameObject obj_HeCheng_1;
    public GameObject obj_HeCheng_2;

    public ObscuredString HeChengType;          //1.宠物合成  2.技能合成


    // Use this for initialization
    void Start() {

        //测试数据
        PetID_List_1 = "0";
        PetID_List_2 = "0";
        HeChengType = "1";      //默认为宠物合成
        showAllHeChengList();

    }

    // Update is called once per frame
    void Update() {

    }

    //显示宠物技能
    public void showPetSkillList() {

        HeChengType = "2";

    }

    //显示宠物自身
    public void showPetList() {

    }

    //展示合成宠物信息
    public void showAllHeChengList() {

        //清空父节点
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_HeChengListSet);

        //实例化节点
        obj_HeCheng_1 = (GameObject)Instantiate(Obj_HeChengShow);
        obj_HeCheng_1.transform.SetParent(Obj_HeChengListSet.transform);
        obj_HeCheng_1.transform.localPosition = Vector3.zero;
        obj_HeCheng_1.transform.localScale = new Vector3(1, 1, 1);
        obj_HeCheng_1.GetComponent<UI_PetHeChengList>().PetID = PetID_List_1;
        obj_HeCheng_1.GetComponent<UI_PetHeChengList>().HeChengWeiZhi = "1";

        obj_HeCheng_2 = (GameObject)Instantiate(Obj_HeChengShow);
        obj_HeCheng_2.transform.SetParent(Obj_HeChengListSet.transform);
        obj_HeCheng_2.transform.localPosition = new Vector3(643, 0, 0);
        obj_HeCheng_2.transform.localScale = new Vector3(1, 1, 1);
        obj_HeCheng_2.GetComponent<UI_PetHeChengList>().PetID = PetID_List_2;
        obj_HeCheng_2.GetComponent<UI_PetHeChengList>().HeChengWeiZhi = "2";

        //货币通用栏
        Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet("202");
        //obj_HeCheng_1.SetActive(false);
        //obj_HeCheng_2.SetActive(false);
    }

    //我点击了合成
    public void Btn_HeCheng() {

        //检测服务器网络
        if (Game_PublicClassVar.gameLinkServer.ServerLinkStatus == false)
        {
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_58");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1);
            return;
        }

        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv < 20)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_149");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("需要达到20级后开启!");
            return;
        }

        PetID_List_1 = obj_HeCheng_1.GetComponent<UI_PetHeChengList>().PetID;
        PetID_List_2 = obj_HeCheng_2.GetComponent<UI_PetHeChengList>().PetID;

        //判断合成是否佩戴装备
        bool ifWearEquip = false;

        for (int i = 1; i <= 3; i++) {

            string EquipID_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_" + i, "ID", PetID_List_1, "RosePet");
            string EquipID_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_" + i, "ID", PetID_List_2, "RosePet");
            if (EquipID_1 != "" && EquipID_1 != "0" && EquipID_1 != null)
            {
                ifWearEquip = true;
            }

            if (EquipID_2 != "" && EquipID_2 != "0" && EquipID_2 != null)
            {
                ifWearEquip = true;
            }

        }

        if (ifWearEquip) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint("召唤兽身上佩戴装备,请先卸下再进行合成!");
            return;
        }

        //野生和变异无法合成
        string baby_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", PetID_List_1, "RosePet");
        string baby_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", PetID_List_2, "RosePet");
        if (baby_1 == "0" || baby_2 == "0")
        {
            //判断其中一个是否为变异
            string NowPetID_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", PetID_List_1, "RosePet");
            string NowPetID_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", PetID_List_2, "RosePet");
            string ifBianYi_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", NowPetID_1, "Pet_Template");
            string ifBianYi_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", NowPetID_2, "Pet_Template");

            if (ifBianYi_1 == "1" || ifBianYi_2 == "1") {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("野生宠物不能与变异宠物进行合成!");
                return;
            }
        }

        if (HeChengType == "1"){


            if (PetID_List_1 == "" || PetID_List_1 == null || PetID_List_2 == "" || PetID_List_2 == null) {
                return;
            }

            if (Game_PublicClassVar.Get_function_AI.Pet_HeCheng(PetID_List_1, PetID_List_2)) {

                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_148");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物合成成功!请到宠物列表中进行查看");
                //清空数据
                PetID_List_1 = "0";
                PetID_List_2 = "0";
                showAllHeChengList();
            }
        }

        /*
        if(HeChengType == "2"){
            PetID_List_1 = obj_HeCheng_1.GetComponent<UI_PetHeChengList>().PetID;
            string addSkill = "100001";
            Game_PublicClassVar.Get_function_AI.Pet_AddSkill(PetID_List_1, addSkill);
        }
        */
    }
}
