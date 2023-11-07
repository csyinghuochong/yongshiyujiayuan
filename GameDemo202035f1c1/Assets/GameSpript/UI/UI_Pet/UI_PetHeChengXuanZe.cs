using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PetHeChengXuanZe : MonoBehaviour {

    public ObscuredString ShowPetIDListStr;
    public GameObject Obj_PetHeChengXuanZeList;
    public GameObject Obj_PetHeChengXuanZeListSet;

    public GameObject FuJiObj;
    public ObscuredString HeChengWeiZhi;        //合成位置
    public ObscuredString XuanZhongPetID;
    public int XuanZeType;             //  1 : 表示矿区
	// Use this for initialization
	void Start () {
        showList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void showList() {

        //清空
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PetHeChengXuanZeListSet);

        ShowPetIDListStr = Game_PublicClassVar.Get_function_AI.Pet_ReturnRoseListOther(Game_PublicClassVar.Get_function_AI.Pet_ReturnHeChengListStr());

        string[] ShowPetIDList = ShowPetIDListStr.ToString().Split(';');
        if (ShowPetIDList[0] != "") {
            for (int i = 0; i <= ShowPetIDList.Length - 1; i++)
            {
                //Debug.Log("显示：" + i);
                //实例化一个宠物列表控件
                GameObject petSkillListObj = (GameObject)Instantiate(Obj_PetHeChengXuanZeList);
                petSkillListObj.transform.SetParent(Obj_PetHeChengXuanZeListSet.transform);
                petSkillListObj.transform.localScale = new Vector3(1, 1, 1);
                petSkillListObj.GetComponent<UI_PetHeChengXuanZeList>().PetOnlyID = ShowPetIDList[i];
                petSkillListObj.GetComponent<UI_PetHeChengXuanZeList>().UpdateStatus = true;
                petSkillListObj.GetComponent<UI_PetHeChengXuanZeList>().Obj_FuJiObj = this.gameObject;

            }
        }
    }

    public void XuanZeHeChengPet() {

        //设置合成宠物
        if (FuJiObj == null) {
            return;
        }

        string lockStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LockStatus", "ID", XuanZhongPetID, "RosePet");
        if (lockStatus == "1" && XuanZeType == 0)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物锁定状态无法合成宠物,请先解锁!");
            return;
        }


        if (FuJiObj.GetComponent<UI_PetHeChengList>() != null) {
            switch (HeChengWeiZhi)
            {
                case "1":
                    FuJiObj.GetComponent<UI_PetHeChengList>().PetID = XuanZhongPetID;
                    FuJiObj.GetComponent<UI_PetHeChengList>().showPetProperty();
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_HeChengSet.GetComponent<UI_PetHeCheng>().PetID_List_1 = XuanZhongPetID;
                    break;

                case "2":
                    FuJiObj.GetComponent<UI_PetHeChengList>().PetID = XuanZhongPetID;
                    FuJiObj.GetComponent<UI_PetHeChengList>().showPetProperty();
                    Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_HeChengSet.GetComponent<UI_PetHeCheng>().PetID_List_2 = XuanZhongPetID;
                    break;
            }
        }

        //设置成洗炼宠物
        if (FuJiObj.GetComponent<UI_PetXiLianShow>() != null) {
            FuJiObj.GetComponent<UI_PetXiLianShow>().PetID = XuanZhongPetID;
            FuJiObj.GetComponent<UI_PetXiLianShow>().showPetProperty();
            //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_XiLianSet.GetComponent<UI_PetXiLian>()
        }

        //设置成战队宠物
        if (FuJiObj.GetComponent<UI_MyTeamSet>() != null)
        {
            //获取目标是否为宠物宝宝
            string ifBaby = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", XuanZhongPetID, "RosePet");

            //只有宠物可以上榜
            if (ifBaby == "1")
            {
                //判断自身是否已经在队伍中
                string PetIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanTeam", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string[] PetIDStrList = PetIDStr.Split(';');

                bool ifCunZai = false;

                for (int i = 0; i < PetIDStrList.Length; i++) {
                    if (XuanZhongPetID == PetIDStrList[i]) {
                        ifCunZai = true;
                    }
                }

                if (ifCunZai) {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("此宠物已经在你的战队中"));
                    return;
                }

                FuJiObj.GetComponent<UI_MyTeamSet>().PetXuanZhongID = XuanZhongPetID;
                FuJiObj.GetComponent<UI_MyTeamSet>().AddPetTianTi();
            }
            else {
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_442");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            }



            //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_XiLianSet.GetComponent<UI_PetXiLian>()

        }


        //设置成驻守宠物
        if (FuJiObj.GetComponent<UI_PastureKuangShowName>() != null)
        {
            Game_PublicClassVar.Get_function_Pasture.PastureKuang_SheZhiPet(XuanZhongPetID, FuJiObj.GetComponent<UI_PastureKuangShowName>().NowSelectID, int.Parse(FuJiObj.GetComponent<UI_PastureKuangShowName>().KuangSpaceID));
            FuJiObj.GetComponent<UI_PastureKuangShowName>().Init();
            //刷新主界面显示
            Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().ShowKuangTitleStatus = true;
        }

        Destroy(this.gameObject);

    }

    public void UI_Close() {
        Destroy(this.gameObject);
    }
}
