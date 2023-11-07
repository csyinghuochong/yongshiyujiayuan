using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_PaiHangPetList : MonoBehaviour {

    public string PetOnlyID;
    public GameObject Obj_FuJiObj;
    public GameObject Obj_XuanZhong;
    public GameObject Obj_PetName;
    public GameObject Obj_PetLv;
    public GameObject Obj_PetIcon;
    public GameObject Obj_PetQuality;
    public GameObject Obj_CanZhan;
    public bool UpdateStatus;

	public string[] ServerPetDataList;
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
		string petName = ServerPetDataList[6];
        //Debug.Log("petName = " + petName);

        //获取宠物等级
		string petLv = ServerPetDataList[2];

        //获取宠物头像Icon
		string petID = ServerPetDataList[1];
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
		if (Obj_FuJiObj.GetComponent<UI_PaiHangShowPet>().NowSclectPetID != PetOnlyID) {
            //取消选中状态
            Obj_XuanZhong.SetActive(false);
        }

        //获取宠物参战ID进行显示PetFightID
        string petCanZhanID = "2";
        if (petCanZhanID == "1")
        {
            Obj_CanZhan.SetActive(true);
        }
        else {
            Obj_CanZhan.SetActive(false);
        }
    }


    //点击选中按钮
    public void Click_XuanZhong() {

        //Debug.Log("点击选中" + PetOnlyID);
		Obj_FuJiObj.GetComponent<UI_PaiHangShowPet>().NowSclectPetID = PetOnlyID;
		Obj_FuJiObj.GetComponent<UI_PaiHangShowPet>().UpdateShowStatus = true;
        //显示选中状态
        Obj_XuanZhong.SetActive(true);
    }

}
