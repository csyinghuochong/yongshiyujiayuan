using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_EquipMakeListObj : MonoBehaviour {

    public string PetOnlyID;
    public GameObject Obj_FuJiObj;
    public GameObject Obj_XuanZhong;
    public GameObject Obj_ItemName;
    public GameObject Obj_ItemLv;
    public GameObject Obj_ItemIcon;
    public GameObject Obj_ItemQuality;
    public GameObject Obj_CanZhan;
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
		string makeItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeItemID", "ID", PetOnlyID, "EquipMake_Template");
		string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", makeItemID, "Item_Template");
        //Debug.Log("petName = " + petName);
        //获取宠物等级
		string makeLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeLv", "ID", PetOnlyID, "EquipMake_Template");

        //获取宠物头像Icon
        //string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", PetOnlyID, "RosePet");
		string itemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", makeItemID, "Item_Template");
		string itemquality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", makeItemID, "Item_Template");

        //显示宠物信息
		Obj_ItemName.GetComponent<Text>().text = itemName;
		Obj_ItemLv.GetComponent<Text>().text = makeLv + "级";

        //显示底图
		Object obj = Resources.Load("ItemIcon/" + itemIcon, typeof(Sprite));
        Sprite img = obj as Sprite;
		Obj_ItemIcon.GetComponent<Image>().sprite = img;

		//显示品质
		object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(itemquality), typeof(Sprite));
		Sprite itemQuality = obj2 as Sprite;
		Obj_ItemQuality.GetComponent<Image>().sprite = itemQuality;
		/*
		if(Obj_FuJiObj.GetComponent<UI_EquipMake>().makeItemlasterObj!=null){
			Obj_FuJiObj.GetComponent<UI_EquipMake> ().makeItemlasterObj.GetComponent<UI_EquipMakeListObj> ().UpdateStatus = true;
		}
		*/
        //获取自身是否被选中
		if (Obj_FuJiObj.GetComponent<UI_EquipMake>().nowSeclect != PetOnlyID) {
            //取消选中状态
            Obj_XuanZhong.SetActive(false);
			Obj_FuJiObj.GetComponent<UI_EquipMake> ().makeItemlasterObj = this.gameObject;
        }


		bool ifMakeStatus = true;
		//判定当前道具是否满足制造


		//判断当前制造等级是否满足
		if(Game_PublicClassVar.Get_function_Rose.GetRoseLv()<int.Parse(makeLv)){
			ifMakeStatus = false;
		}


		if (!ifMakeStatus) {

			//置灰
			object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
			Material huiMaterial = huiObj as Material;
			Obj_ItemIcon.GetComponent<Image>().material = huiMaterial;
			Obj_ItemQuality.GetComponent<Image>().material = huiMaterial;
			Obj_ItemName.GetComponent<Text>().color = new Color(0.55f,0.55f,0.55f,1);

		}

		/*
        //获取宠物参战ID进行显示PetFightID
        string petCanZhanID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", PetOnlyID, "RosePet");
        if (petCanZhanID == "1")
        {
            Obj_CanZhan.SetActive(true);
        }
        else {
            Obj_CanZhan.SetActive(false);
        }
		*/
    }


    //点击选中按钮
    public void Click_XuanZhong() {

		Obj_FuJiObj.GetComponent<UI_EquipMake>().nowSeclect = PetOnlyID;
		Obj_FuJiObj.GetComponent<UI_EquipMake>().UpdateShowStatus = true;
        //显示选中状态
        Obj_XuanZhong.SetActive(true);
    }
}
