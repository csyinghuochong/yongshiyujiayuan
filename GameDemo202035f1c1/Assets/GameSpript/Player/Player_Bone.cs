using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bone : MonoBehaviour {

    public GameObject BoneSet;
    public GameObject Bone_Low;
    public GameObject Bone_Center;
    public GameObject Bone_Head;
    public GameObject Bone_Hand;
    //public GameObject Bone_HandObj;
    public GameObject PetPositionSet;
    public GameObject PetPositionSet_Posi_1;

    //战士
    public RuntimeAnimatorController ZhanShi_Animator;
    public Avatar ZhanShi_Avatar;
    public GameObject ZhanShi_Obj;
    public GameObject ZhanShi_WuQiObj;
    public GameObject ZhanShi_WuQiObj_Base;
    public SkinnedMeshRenderer[] ZhanShi_Skin;

    //法师
    public RuntimeAnimatorController FaShi_Animator;
    public Avatar FaShi_Avatar;
    public GameObject FaShi_Obj;
    public GameObject FaShi_WuQiObj;
    public GameObject FaShi_WuQiObj_Base;
    public SkinnedMeshRenderer[] FaShi_Skin;

    //猎人
    public RuntimeAnimatorController LieRen_Animator;
    public Avatar LieRen_Avatar;
    public GameObject LieRen_Obj;
    public GameObject LieRen_WuQiObj;
    public GameObject LieRen_WuQiObj_Base;
    public SkinnedMeshRenderer[] LieRen_Skin;

    // Use this for initialization
    void Start () {

        //RoseModelChange("1");           //后面此处需要协助服务器一起处理传输,跟做彩果系统一样

        /*
        //显示外观
        string nowYanSeID = "10002";
        string nowYanSe = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeID", "ID", nowYanSeID, "RanSe_Template");
        Game_PublicClassVar.Get_function_Rose.RoseModelChange(nowYanSe, nowYanSeID, false, "1", this.gameObject);

        string nowNowYanSeHairID = "30002";
        string nowYanSeHair = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeID", "ID", nowNowYanSeHairID, "RanSe_Template");
        Game_PublicClassVar.Get_function_Rose.RoseModelChange(nowYanSeHair, nowNowYanSeHairID, false, "2", this.gameObject);
        */


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /*
    //改变自身衣服颜色(1:表示默认  2：标书觉醒  黄色)
    public void RoseModelChange(string showType)
    {

        //获取职业
        string occ = this.gameObject.GetComponent<Player_Status>().OccType;
        switch (occ)
        {
            //战士相关
            case "1":
                string waiguanName = "PiFu_" + showType;

                Texture2D img = (Texture2D)Resources.Load("3DModel/RoseModel/Occ_ZhanShi/PiFu/" + waiguanName);
                if (img == null) {
                    return;
                }

                ZhanShi_Skin[0].material.mainTexture = img;
                ZhanShi_Skin[1].material.mainTexture = img;
                ZhanShi_Skin[5].material.mainTexture = img;
                break;

            //法师相关
            case "2":
                waiguanName = "PiFu_" + showType;
                img = (Texture2D)Resources.Load("3DModel/RoseModel/Occ_MoFaShi/PiFu/" + waiguanName);
                if (img == null)
                {
                    return;
                }
                waiguanName = "PiFu_" + showType;
                FaShi_Skin[0].material.mainTexture = img;
                FaShi_Skin[1].material.mainTexture = img;
                FaShi_Skin[5].material.mainTexture = img;
                break;
        }

    }
    */
}
