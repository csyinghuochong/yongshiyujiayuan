using UnityEngine;
using System.Collections;

public class Rose_Bone : MonoBehaviour {

    public GameObject BoneSet;
    public GameObject Bone_Low;
    public GameObject Bone_Center;
    public GameObject Bone_Head;
    public GameObject Bone_Hand;
    public GameObject Bone_HandObj;
    public GameObject PetPositionSet;
    public GameObject RoseModelEffect;

    //战士
    public RuntimeAnimatorController ZhanShi_Animator;
    public Avatar ZhanShi_Avatar;
    public GameObject ZhanShi_Obj;
    public SkinnedMeshRenderer[] ZhanShi_Skin;
    public Material[] ZhanShi_InvisibleShader;            //角色隐身shader
    public Material[] ZhanShi_SelfShader;                 //角色本身shader
    public GameObject[] ZhanShi_ModelShowShader;            //角色本身shader(用于显示遮挡科技感)
    public Transform RoseWeaponPosi_ZhanShi;                //角色武器位置
    public Transform RoseModelWeaponPosi_ZhanShi;           //角色武器位置UI
    public GameObject RoseWeaponBaseModel_ZhanShi;
    public GameObject RoseModelWeaponBaseModel_ZhanShi;

    //法师
    public RuntimeAnimatorController FaShi_Animator;
    public Avatar FaShi_Avatar;
    public GameObject FaShi_Obj;
    public SkinnedMeshRenderer[] FaShi_Skin;
    public Material[] FaShi_InvisibleShader;            //角色隐身shader
    public Material[] FaShi_SelfShader;                 //角色本身shader
    public GameObject[] FaShi_ModelShowShader;            //角色本身shader(用于显示遮挡科技感)
    public Transform RoseWeaponPosi_FaShi;                //角色武器位置
    public Transform RoseModelWeaponPosi_FaShi;           //角色武器位置UI
    public GameObject RoseWeaponBaseModel_FaShi;          //角色法师
    public GameObject RoseModelWeaponBaseModel_FaShi;


    //猎人
    public RuntimeAnimatorController LieRen_Animator;
    public Avatar LieRen_Avatar;
    public GameObject LieRen_Obj;
    public SkinnedMeshRenderer[] LieRen_Skin;
    public Material[] LieRen_InvisibleShader;            //角色隐身shader
    public Material[] LieRen_SelfShader;                 //角色本身shader
    public GameObject[] LieRen_ModelShowShader;            //角色本身shader(用于显示遮挡科技感)
    public Transform RoseWeaponPosi_LieRen;                //角色武器位置
    public Transform RoseModelWeaponPosi_LieRen;           //角色武器位置UI
    public GameObject RoseWeaponBaseModel_LieRen;          //角色法师
    public GameObject RoseModelWeaponBaseModel_LieRen;


    // Use this for initialization
    void Awake()
    {
        string nowOcc = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        switch (nowOcc)
        {

            //战士
            case "1":

                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = ZhanShi_Animator;
                this.gameObject.GetComponent<Animator>().avatar = ZhanShi_Avatar;
                ZhanShi_Obj.SetActive(true);
                FaShi_Obj.SetActive(false);
                LieRen_Obj.SetActive(false);
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseWeaponPosi = RoseWeaponPosi_ZhanShi;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponPosi = RoseModelWeaponPosi_ZhanShi;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseWeaponBaseModel = RoseWeaponBaseModel_ZhanShi;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponBaseModel = RoseModelWeaponBaseModel_ZhanShi;

                break;

            //法师
            case "2":

                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = FaShi_Animator;
                this.gameObject.GetComponent<Animator>().avatar = FaShi_Avatar;
                FaShi_Obj.SetActive(true);
                ZhanShi_Obj.SetActive(false);
                LieRen_Obj.SetActive(false);
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseWeaponPosi = RoseWeaponPosi_FaShi;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponPosi = RoseModelWeaponPosi_FaShi;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseWeaponBaseModel = RoseWeaponBaseModel_FaShi;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponBaseModel = RoseModelWeaponBaseModel_FaShi;

                break;

            //猎人
            case "3":

                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = LieRen_Animator;
                this.gameObject.GetComponent<Animator>().avatar = LieRen_Avatar;
                FaShi_Obj.SetActive(false);
                ZhanShi_Obj.SetActive(false);
                LieRen_Obj.SetActive(true);
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseWeaponPosi = RoseWeaponPosi_LieRen;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponPosi = RoseModelWeaponPosi_LieRen;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseWeaponBaseModel = RoseWeaponBaseModel_LieRen;
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponBaseModel = RoseModelWeaponBaseModel_LieRen;

                break;
        }


        //觉醒
        /*
        if (Game_PublicClassVar.Get_function_Rose.ReturnJueXingStatus())
        {
            //外形变化
            //Game_PublicClassVar.Get_function_Rose.RoseModelChange("2");

            //添加特效
            if (jueXingEffect == null)
            {
                GameObject effect = (GameObject)Resources.Load("Effect/Skill/Effect_JueXing", typeof(GameObject));
                jueXingEffect = (GameObject)Instantiate(effect);
                jueXingEffect.transform.localScale = new Vector3(1, 1, 1);
                jueXingEffect.GetComponent<SkillEffectPosition>().TargetObj = Bone_Low;
                jueXingEffect.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.gameObject.transform);
            }

        }
        */
    }

    void Start () {

        //默认显示当前外形颜色
        Game_PublicClassVar.Get_function_Rose.RoseModelChangeValue();
    }
	
	// Update is called once per frame
	void Update () {

        Bone_Hand.transform.position = Bone_HandObj.transform.position; 
        /*
        if (testCreatMonster) {
            testCreatMonster = false;
            Game_PublicClassVar.Get_function_AI.AI_CreatMonster("70001004",Vector3.zero);
        }
	    */
	}
}
