using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PetHeChengList : MonoBehaviour {

    public ObscuredString PetID;
    public ObscuredString HeChengWeiZhi;

    public GameObject Obj_HeChengXuanZe;

    public GameObject Obj_PetName;
    public GameObject Obj_PetLv;
    //public GameObject Obj_PingFen;
    public GameObject Obj_PetIcon;
    public GameObject Obj_PetTitle;

    //宠物资质相关
    public GameObject Obj_ZiZhiValue_Hp;
    public GameObject Obj_ZiZhiValue_Act;
    public GameObject Obj_ZiZhiValue_MageAct;
    public GameObject Obj_ZiZhiValue_Def;
    public GameObject Obj_ZiZhiValue_Adf;
    public GameObject Obj_ZiZhiValue_ActSpeed;
    public GameObject Obj_ZiZhiValue_ChengZhang;

    //宠物技能相关
    public GameObject Obj_SkillListSet;
    public GameObject Obj_PetSkillIcon;

	// Use this for initialization
	void Start () {
        showPetProperty();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //展示宠物信息
    public void showPetProperty() {


        //获取宠物信息
        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", PetID, "RosePet");
        string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", PetID, "RosePet");
        string petTemplateID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", PetID, "RosePet");
        string headIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeadIcon", "ID", petTemplateID, "Pet_Template");

        if(PetID=="0"){
            petName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("点击放置合成宠物");
            //Obj_PingFen.SetActive(false);
            Obj_PetIcon.SetActive(false);
        }else{
            //Obj_PingFen.SetActive(true);
            Obj_PetIcon.SetActive(true);
        }
            
        //显示宠物头像
        object obj = Resources.Load("PetHeadIcon/" + headIcon, typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_PetIcon.GetComponent<Image>().sprite = img;

        //显示宠物属性
        Obj_PetName.GetComponent<Text>().text = petName;
        Obj_PetLv.GetComponent<Text>().text = petLv + "级";

        //当前资质
        string zizhiNow_Hp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp", "ID", PetID, "RosePet");
        string zizhiNow_Act = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act", "ID", PetID, "RosePet");
        string zizhiNow_Def = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def", "ID", PetID, "RosePet");
        string zizhiNow_Adf = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf", "ID", PetID, "RosePet");
        string zizhiNow_ActSpeed = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed", "ID", PetID, "RosePet");
        string zizhiNow_ChengZhang = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang", "ID", PetID, "RosePet");
        string zizhiNow_MageAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct", "ID", PetID, "RosePet");

        //显示宠物资质
        Obj_ZiZhiValue_Hp.GetComponent<Text>().text = zizhiNow_Hp;
        Obj_ZiZhiValue_Act.GetComponent<Text>().text = zizhiNow_Act;
        Obj_ZiZhiValue_MageAct.GetComponent<Text>().text = zizhiNow_MageAct;
        Obj_ZiZhiValue_Def.GetComponent<Text>().text = zizhiNow_Def;
        Obj_ZiZhiValue_Adf.GetComponent<Text>().text = zizhiNow_Adf;
        Obj_ZiZhiValue_ActSpeed.GetComponent<Text>().text = zizhiNow_ActSpeed;
        Obj_ZiZhiValue_ChengZhang.GetComponent<Text>().text = zizhiNow_ChengZhang;


        //显示宠物评分

        //显示宠物技能
        string petSkillListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetSkill", "ID", PetID, "RosePet");
        string[] petSkillList = petSkillListStr.Split(';');

        //清空父节点
        for (int i = 0; i < Obj_SkillListSet.transform.childCount; i++)
        {
            GameObject go = Obj_SkillListSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        //Debug.Log("petSkillList.Length = " + petSkillList.Length + "petSkillList[0] =" + petSkillList[0]);
        if (petSkillList[0] != "" && petSkillList[0] != "0")
        {
            //循环显示宠物每个技能
            for (int i = 0; i <= petSkillList.Length - 1; i++)
            {
                //实例化一个宠物列表控件
                GameObject petSkillListObj = (GameObject)Instantiate(Obj_PetSkillIcon);
                petSkillListObj.transform.SetParent(Obj_SkillListSet.transform);
                petSkillListObj.transform.localScale = new Vector3(1, 1, 1);
                petSkillListObj.GetComponent<PetSkillIcon>().PetSkillID = petSkillList[i];
            }
        }

        //填补空位
        Game_PublicClassVar.Get_function_AI.Pet_SkillShowNull(petSkillList, Obj_PetSkillIcon, Obj_SkillListSet);
        
        //显示主副宠标签
        switch (HeChengWeiZhi) { 
            case "1":
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("主宠");
                Obj_PetTitle.GetComponent<Text>().text = langStr;
                break;
            case "2":
                langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("副宠");
                Obj_PetTitle.GetComponent<Text>().text = langStr;
                break;
        }
    }


    //点击选择宠物
    public void Btn_XuanZePet()
    {
        GameObject HeChengXuanZeObj = (GameObject)Instantiate(Obj_HeChengXuanZe);
        HeChengXuanZeObj.transform.SetParent(this.gameObject.transform.parent.transform.parent.transform);
        HeChengXuanZeObj.transform.localPosition = new Vector3(0, 0, 0);
        HeChengXuanZeObj.transform.localScale = new Vector3(1, 1, 1);
        HeChengXuanZeObj.GetComponent<UI_PetHeChengXuanZe>().HeChengWeiZhi = HeChengWeiZhi;
        HeChengXuanZeObj.GetComponent<UI_PetHeChengXuanZe>().FuJiObj = this.gameObject;
    }
}
