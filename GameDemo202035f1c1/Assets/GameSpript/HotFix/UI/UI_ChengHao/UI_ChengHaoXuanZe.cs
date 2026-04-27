using UnityEngine;
using System.Collections;

public class UI_ChengHaoXuanZe : MonoBehaviour {

    public string ShowPetIDListStr;
    public GameObject Obj_PetHeChengXuanZeList;
    public GameObject Obj_PetHeChengXuanZeListSet;

    public GameObject FuJiObj;
    public string HeChengWeiZhi;        //合成位置
    public string XuanZhongPetID;

	public GameObject Obj_ChengHaoNull;		//空的称号

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

        ShowPetIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengHaoIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //添加天下第一称号
        //string first_ChengHao = "10014";
        string first_ChengHao = Game_PublicClassVar.Get_gameLinkServerObj.ServerPaiHangChengHao;
        if (first_ChengHao != "") {
            if (ShowPetIDListStr != "")
            {
                ShowPetIDListStr = first_ChengHao + ";" + ShowPetIDListStr;
            }
            else {
                ShowPetIDListStr = first_ChengHao;
            }
        }

        Debug.Log ("ShowPetIDListStr = " + ShowPetIDListStr);
		if (ShowPetIDListStr == "" || ShowPetIDListStr == "0") {
			Obj_ChengHaoNull.SetActive (true);
			return;
		} else {
			Obj_ChengHaoNull.SetActive (false);
		}

        string[] ShowPetIDList = ShowPetIDListStr.Split(';');
        if (ShowPetIDList[0] != ""&& ShowPetIDList[0] != "0") {
            for (int i = 0; i <= ShowPetIDList.Length - 1; i++)
            {
                if (ShowPetIDList[i] != "" && ShowPetIDList[i] != "0") {
                    Debug.Log("显示：" + i);
                    //实例化一个宠物列表控件
                    GameObject petSkillListObj = (GameObject)Instantiate(Obj_PetHeChengXuanZeList);
                    petSkillListObj.transform.SetParent(Obj_PetHeChengXuanZeListSet.transform);
                    petSkillListObj.transform.localScale = new Vector3(1, 1, 1);
                    petSkillListObj.GetComponent<UI_ChengHaoXuanZeList>().ChengHaoID = ShowPetIDList[i];
                    petSkillListObj.GetComponent<UI_ChengHaoXuanZeList>().UpdateStatus = true;
                    petSkillListObj.GetComponent<UI_ChengHaoXuanZeList>().Obj_FuJiObj = this.gameObject;
                }
            }
        }
    }

    /*
    public void XuanZeHeChengPet() {

        //设置合成宠物
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

        Destroy(this.gameObject);
    }
    */

    public void UI_Close() {
        Debug.Log("点击了关闭按钮");
        Destroy(this.gameObject);
    }
}
