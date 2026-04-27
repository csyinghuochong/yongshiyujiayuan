using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoseDataSet : MonoBehaviour {

	public bool IfUpdateData;
	public bool IfUpdatePetData;
	public int UpdatePetDataNum;

	public GameObject Obj_RoseName;
	public GameObject Obj_RoseLv;
	public GameObject Obj_HuoLi;
	public GameObject Obj_HuoLiPro;
	public GameObject Obj_TiLi;
	public GameObject Obj_TiLiPro;
	public GameObject Obj_RoseIconShow;


	public GameObject Obj_PetSet;
	public GameObject Obj_PetName;
	public GameObject Obj_PetLv;
	//public GameObject Obj_PetHp;
	public GameObject Obj_PetHpPro;
	//public GameObject Obj_PetExp;
	public GameObject Obj_PetExpPro;
	public GameObject Obj_PetIconShow;

    public GameObject Obj_PetHpProText;
    //public GameObject Obj_PetExp;
    public GameObject Obj_PetExpProText;
	public GameObject Obj_PetMoShi;

    // Use this for initialization
    void Start () {
		IfUpdateData = true;
		IfUpdatePetData = true;
	}
	
	// Update is called once per frame
	void Update () {
		//更新状态
		if(IfUpdateData){
			IfUpdateData = false;
			RoseUpdateDataAll();
		}

		//更新宠物
		if(IfUpdatePetData){
			if (UpdatePetDataNum >= 3) {
				UpdatePetDataNum = 0;
				IfUpdatePetData = false;
				PetUpdateDataAll();
			}
			UpdatePetDataNum = UpdatePetDataNum + 1;
		}
	}

	public void RoseUpdateDataAll(){

		//显示名称
		RoseUpdate_Name();

		//显示等级
		RoseUpdate_Lv();

		//显示活力
		RoseUpdate_HuoLi();

        //显示活力
        RoseUpdate_TiLi();

        //头像Icon
        RoseUpdate_HeadIcon();

	}


	public void PetUpdateDataAll(){

		string id = Game_PublicClassVar.function_AI.Pet_ReturnChuZhanID();
		if (id == "") {
			Obj_PetSet.SetActive (false);
			return;
		} else {
			Obj_PetSet.SetActive (true);
		}

		//显示名称
		PetUpName();

		//显示等级
		PetUpLv();

		//显示血量
		PetUpHp();

		//更新经验显示
		PetUpExp();

		//头像Icon
		PetUpHeadIcon();

		//宠物模式
		PetActModel();

	}

	//更新名称
	public void RoseUpdate_Name(){

		string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Obj_RoseName.GetComponent<Text> ().text = roseName;

	}

	//显示等级
	public void RoseUpdate_Lv(){
		string roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv().ToString();
		Obj_RoseLv.GetComponent<Text>().text = roseLv;
        //Debug.Log("RoseUpdate_LvRoseUpdate_LvRoseUpdate_Lv roseLv = " + roseLv);
    }

	//显示活力
	public void RoseUpdate_HuoLi(){

		string roseHuoLi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuoLi", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		string roseHuoLiMax = "100";
        string langstr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("活力");
		Obj_HuoLi.GetComponent<Text> ().text = langstr + ":" + roseHuoLi + "/" + roseHuoLiMax;
		Obj_HuoLiPro.GetComponent<Image> ().fillAmount = float.Parse (roseHuoLi) / float.Parse (roseHuoLiMax);

	}

	//显示活力
	public void RoseUpdate_TiLi(){

		string roseTiLi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TiLi", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		string roseTiLiMax = "100";
        string langstr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("体力");
        Obj_TiLi.GetComponent<Text> ().text = langstr + ":" + roseTiLi + "/" + roseTiLiMax;
		Obj_TiLiPro.GetComponent<Image> ().fillAmount = float.Parse (roseTiLi) / float.Parse (roseTiLiMax);

	}

	//头像Icon
	public void RoseUpdate_HeadIcon()
	{

        string roseHeadIconId = "10001";
        string roseOcc = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        switch (roseOcc) {
            case "1":
                roseHeadIconId = "10001";
                break;

            case "2":
                roseHeadIconId = "10002";
                break;

            case "3":
                roseHeadIconId = "10003";
                break;
        }

        Object obj = Resources.Load("HeadIcon/PlayerIcon/" + roseHeadIconId, typeof(Sprite));
		Sprite img = obj as Sprite;
        Obj_RoseIconShow.GetComponent<Image>().sprite = img;
		
	}

	//获取当前出战宠物等级
	public void PetUpLv(){

		string id = Game_PublicClassVar.function_AI.Pet_ReturnChuZhanID();
		if(id==""){
			return;
		}
		string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", id, "RosePet");
		Obj_PetLv.GetComponent<Text>().text = petLv;

	}

	//宠物名称更新
	public void PetUpName(){
		string id = Game_PublicClassVar.function_AI.Pet_ReturnChuZhanID ();
		if(id==""){
			return;
		}
		string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", id, "RosePet");
		Obj_PetName.GetComponent<Text>().text = petName;
	}

	//宠物血量更新
	public void PetUpHp(){

		GameObject chuzhanObj = Game_PublicClassVar.Get_function_AI.Pet_ReturnChuZhanObj();
		//Debug.Log ("chuzhanObj = " + chuzhanObj.name);
		if (chuzhanObj != null){
			string petHp = chuzhanObj.GetComponent<AI_Property>().AI_Hp.ToString();
			string petHpMax = chuzhanObj.GetComponent<AI_Property>().AI_HpMax.ToString();
			//Debug.Log("petHp = "+ petHp + " petHpMax = " + petHpMax);
			Obj_PetHpPro.GetComponent<Image>().fillAmount = float.Parse(petHp) / float.Parse (petHpMax);
            Obj_PetHpProText.GetComponent<Text>().text = (float.Parse(petHp) / float.Parse(petHpMax) * 100).ToString("F2") + "%";

            if (int.Parse(petHp) <= 0) {
				//灰化Icon
				object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
				Material huiMaterial = huiObj as Material;
				Obj_PetIconShow.GetComponent<Image>().material = huiMaterial;
				//Obj_JingLingName.GetComponent<Text>().color = new Color(0.55f, 0.55f, 0.55f, 1);
			}else{
				Obj_PetIconShow.GetComponent<Image> ().material = null;
			}
		}
	}

	//宠物经验更新
	public void PetUpExp(){
		
		string id = Game_PublicClassVar.function_AI.Pet_ReturnChuZhanID();
		if(id==""){
			return;
		}
		
		string petExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetExp", "ID", id, "RosePet");
		string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", id, "RosePet");
		string petExpSum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetUpExp", "ID", petLv, "RoseExp_Template");
		Obj_PetExpPro.GetComponent<Image>().fillAmount = float.Parse (petExp) / float.Parse (petExpSum);
        Obj_PetExpProText.GetComponent<Text>().text = (float.Parse(petExp) / float.Parse(petExpSum)*100).ToString("F2") + "%";
        //Debug.Log("petExp = "+ petExp + ";petExpSum = " + petExpSum);
    }

	//宠物头像更新
	public void PetUpHeadIcon(){
		
		string id = Game_PublicClassVar.function_AI.Pet_ReturnChuZhanID ();
		if(id==""){
			return;
		}
		string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", id, "RosePet");
		string petHeadIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeadIcon", "ID", petID, "Pet_Template");

		//显示底图
		Object obj = Resources.Load("PetHeadIcon/" + petHeadIcon, typeof(Sprite));
		Sprite img = obj as Sprite;
		Obj_PetIconShow.GetComponent<Image>().sprite = img;

	}

	//显示
	public void PetActModel() {

		switch (Game_PublicClassVar.Get_game_PositionVar.PetActMoShi)
		{
			//移动状态
			case 0:
				Obj_PetMoShi.GetComponent<Text>().text = "模式:攻击";
				Obj_PetMoShi.GetComponent<Outline>().effectColor = new Color(0.5f, 0f, 0f);
				break;

			case 1:
				Obj_PetMoShi.GetComponent<Text>().text = "模式:跟随";
				Obj_PetMoShi.GetComponent<Outline>().effectColor = new Color(0.2f,0.5f,0f);
				break;

			case 2:
				Obj_PetMoShi.GetComponent<Text>().text = "模式:守护";
				break;
		}

	}

    //打开设置界面
    public void OpenSetting() {

        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenGameSetting();

    }

    public void OpenPetUI() {

        //打开宠物
        UI_FunctionOpen ui_FunctionOpen = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>();
        ui_FunctionOpen.OpenPet();

        //默认选择当前出战宠物
        if (ui_FunctionOpen.Obj_rosePet != null) {
            string id = Game_PublicClassVar.function_AI.Pet_ReturnChuZhanID();
            ui_FunctionOpen.Obj_rosePet.GetComponent<UI_Pet>().NowSclectPetID = id;
            ui_FunctionOpen.Obj_rosePet.GetComponent<UI_Pet>().UpdateShowStatus = true;
            ui_FunctionOpen.Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_EquipSet.GetComponent<UI_PetEquipShowSet>().UpdatePetEquipShowStatus = true;
        }
    }

}
