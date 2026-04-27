using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_AddPropertyShow : MonoBehaviour
{

    public string PetID;

    public GameObject Obj_PetName;
    public GameObject Obj_PetLv;
    public GameObject Obj_PingFen;
    public GameObject Obj_PetIcon;

    //宠物资质相关
    public GameObject Obj_ZiZhiValue_Hp;
    public GameObject Obj_ZiZhiValue_Act;
    public GameObject Obj_ZiZhiValue_Def;
    public GameObject Obj_ZiZhiValue_Adf;
    public GameObject Obj_ZiZhiValue_ActSpeed;
    public GameObject Obj_ZiZhiValue_ChengZhang;

    //宠物技能相关
    public GameObject Obj_SkillListSet;
    public GameObject Obj_PetSkillIcon;

	// Use this for initialization
	void Start () {
        //showPetProperty();
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
            petName = "点击放置合成宠物";
            Obj_PingFen.SetActive(false);
            Obj_PetIcon.SetActive(false);
        }else{
            Obj_PingFen.SetActive(true);
            Obj_PetIcon.SetActive(true);
        }
            
        //显示宠物头像
        object obj = Resources.Load("PetHeadIcon/" + headIcon, typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_PetIcon.GetComponent<Image>().sprite = img;

        //显示宠物属性
        Obj_PetName.GetComponent<Text>().text = petName;
        Obj_PetLv.GetComponent<Text>().text = "LV" + petLv;

        //当前资质
        string zizhiNow_Hp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp", "ID", PetID, "RosePet");
        string zizhiNow_Act = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act", "ID", PetID, "RosePet");
        string zizhiNow_Def = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def", "ID", PetID, "RosePet");
        string zizhiNow_Adf = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf", "ID", PetID, "RosePet");
        string zizhiNow_ActSpeed = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed", "ID", PetID, "RosePet");
        string zizhiNow_ChengZhang = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang", "ID", PetID, "RosePet");

        //显示宠物资质
        Obj_ZiZhiValue_Hp.GetComponent<Text>().text = zizhiNow_Hp;
        Obj_ZiZhiValue_Act.GetComponent<Text>().text = zizhiNow_Act;
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
    }
}
