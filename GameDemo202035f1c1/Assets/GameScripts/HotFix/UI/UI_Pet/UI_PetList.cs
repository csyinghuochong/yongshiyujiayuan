using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_PetList : MonoBehaviour {

    public string PetOnlyID;
    public GameObject Obj_FuJiObj;
    public GameObject Obj_XuanZhong;
    public GameObject Obj_PetName;
    public GameObject Obj_PetLv;
    public GameObject Obj_PetIcon;
    public GameObject Obj_PetQuality;
    public GameObject Obj_CanZhan;
    public GameObject Obj_LockImg;
    public bool UpdateStatus;


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (UpdateStatus) {
            UpdateStatus = false;
            showPetListProperty();
        }
	}

    //展示宠物列表信息
    void showPetListProperty() { 
    
        //获取宠物名称
        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", PetOnlyID, "RosePet");
        //Debug.Log("petName = " + petName);

        //获取宠物等级
        string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", PetOnlyID, "RosePet");

        //获取宠物头像Icon
        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", PetOnlyID, "RosePet");
        string petHeadIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeadIcon", "ID", petID, "Pet_Template");

        //显示宠物信息
        Obj_PetName.GetComponent<Text>().text = petName;
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("级");
        Obj_PetLv.GetComponent<Text>().text = petLv + langStr;

        //显示底图
        Object obj = Resources.Load("PetHeadIcon/" + petHeadIcon, typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_PetIcon.GetComponent<Image>().sprite = img;

        //获取自身是否被选中
        if (Obj_FuJiObj.GetComponent<UI_Pet>().NowSclectPetID != PetOnlyID)
        {
            //取消选中状态
            Obj_XuanZhong.SetActive(false);
        }
        else {
            Obj_XuanZhong.SetActive(true);
        }

        //获取宠物参战ID进行显示PetFightID
        string petCanZhanID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", PetOnlyID, "RosePet");
        if (petCanZhanID == "1")
        {
            Obj_CanZhan.SetActive(true);
        }
        else {
            Obj_CanZhan.SetActive(false);
        }

        //显示锁定状态
        string lockStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LockStatus", "ID", PetOnlyID, "RosePet");
        if (lockStatus == "" || lockStatus == "0")
        {
            Obj_LockImg.SetActive(false);
        }
        else
        {
            Obj_LockImg.SetActive(true);
        }
        
    }


    //点击选中按钮
    public void Click_XuanZhong() {

        //Debug.Log("点击选中" + PetOnlyID);
        Obj_FuJiObj.GetComponent<UI_Pet>().NowSclectPetID = PetOnlyID;
        Obj_FuJiObj.GetComponent<UI_Pet>().UpdateShowStatus = true;
        //显示选中状态
        Obj_XuanZhong.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().UpdatePetEquipShowStatus = true;

    }

}
