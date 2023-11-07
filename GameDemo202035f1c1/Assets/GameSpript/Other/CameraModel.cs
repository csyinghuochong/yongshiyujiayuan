using UnityEngine;
using System.Collections;

public class CameraModel : MonoBehaviour {

    public GameObject Obj_ModelZhanShi;
    public GameObject Obj_ModelFaShi;
    public GameObject Obj_ModelLieRen;

    public GameObject Obj_EquipModel_Posi_ModelEffect;      //这个特效只能显示1个
    public GameObject Obj_EquipModel_Posi_Low;

    public SkinnedMeshRenderer[] Obj_ModelZhanShi_Skin;
    public SkinnedMeshRenderer[] Obj_ModelFaShi_Skin;
    public SkinnedMeshRenderer[] Obj_ModelLieRen_Skin;

    public GameObject Obj_ZuoQiModelShow;

    public GameObject Obj_PlayerModelZhanShi;
    public GameObject Obj_PlayerModelFaShi;
    public GameObject Obj_PlayerModelLieRen;

    public SkinnedMeshRenderer[] Obj_PlayerModelZhanShi_Skin;
    public SkinnedMeshRenderer[] Obj_PlayerModelFaShi_Skin;
    public SkinnedMeshRenderer[] Obj_PlayerModelLieRen_Skin;


    public GameObject Obj_EquipRoseSet;
    public GameObject Obj_MonsterModelistSet;
    public GameObject Obj_PetModelListSet;
    public GameObject Obj_ShowPlayerRoseSet;
    public GameObject Obj_PastureModelShowSet;

    // Use this for initialization
    void Start () {
		showModel();

        Obj_EquipRoseSet.SetActive(false);
        Obj_MonsterModelistSet.SetActive(false);
        Obj_PetModelListSet.SetActive(false);
        Obj_ShowPlayerRoseSet.SetActive(false);
        Obj_PastureModelShowSet.SetActive(false);
        Obj_ZuoQiModelShow.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void showModel() {

		//获取当前职业
		string occ = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		switch (occ)
		{
		    case "1":
			    Obj_ModelZhanShi.SetActive(true);
			    Obj_ModelFaShi.SetActive(false);
                Obj_ModelLieRen.SetActive(false);

                break;

		    case "2":
			    Obj_ModelZhanShi.SetActive(false);
			    Obj_ModelFaShi.SetActive(true);
                Obj_ModelLieRen.SetActive(false);
                break;

            case "3":
                Obj_ModelZhanShi.SetActive(false);
                Obj_ModelFaShi.SetActive(false);
                Obj_ModelLieRen.SetActive(true);
                break;
		}

	}

	public void showOccModel(string occ)
	{
		Debug.Log("showOccModelshowOccModelshowOccModel");
		switch (occ)
		{
		    case "1":
			    Obj_PlayerModelZhanShi.SetActive(true);
                Obj_PlayerModelFaShi.SetActive(false);
                Obj_PlayerModelLieRen.SetActive(false);

                break;

		    case "2":
                Obj_PlayerModelZhanShi.SetActive(false);
                Obj_PlayerModelFaShi.SetActive(true);
                Obj_PlayerModelLieRen.SetActive(false);
                break;

            case "3":
                Obj_PlayerModelZhanShi.SetActive(false);
                Obj_PlayerModelFaShi.SetActive(false);
                Obj_PlayerModelLieRen.SetActive(true);
                break;
        }
	}
}
