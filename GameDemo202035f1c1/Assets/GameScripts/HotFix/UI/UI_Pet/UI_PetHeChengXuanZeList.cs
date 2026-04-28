using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_PetHeChengXuanZeList : MonoBehaviour
{

    public string PetOnlyID;
    public GameObject Obj_FuJiObj;
    public GameObject Obj_PetName;
    public GameObject Obj_PetLv;
    public GameObject Obj_PetIcon;
    public GameObject Obj_PetQuality;
    public GameObject Obj_LockImg;
    public bool UpdateStatus;
    public int XuanZeType;

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
        Obj_PetLv.GetComponent<Text>().text = petLv + "级";

        //显示底图
        Object obj = Resources.Load("PetHeadIcon/" + petHeadIcon, typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_PetIcon.GetComponent<Image>().sprite = img;

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
        Obj_FuJiObj.GetComponent<UI_PetHeChengXuanZe>().XuanZhongPetID = PetOnlyID;
        Obj_FuJiObj.GetComponent<UI_PetHeChengXuanZe>().XuanZeHeChengPet();

    }

    //关闭界面
    public void Click_CloseUI() {
        Destroy(this.gameObject);
    }

}
