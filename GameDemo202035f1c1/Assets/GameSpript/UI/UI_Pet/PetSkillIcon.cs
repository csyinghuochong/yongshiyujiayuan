using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PetSkillIcon : MonoBehaviour {

    public string PetSkillID;
    public GameObject PetSkillIconObj;
    private GameObject obj_SkillTips;

	// Use this for initialization
	void Start () {
        showSkillIcon();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //展示技能图标
    void showSkillIcon() {

        //为空不显示技能Icon
        if (PetSkillID == "0" || PetSkillID == "")
        {
            PetSkillIconObj.SetActive(false);
            return;
        }

        string skillIconID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillIcon", "ID", PetSkillID, "Skill_Template");
        //Debug.Log("skillIconID = " + skillIconID);

        //显示底图
        Object obj = Resources.Load("SkillIcon/PetSkill/" + skillIconID, typeof(Sprite));
        Sprite img = obj as Sprite;
        PetSkillIconObj.GetComponent<Image>().sprite = img;

    }


    //点击图标显示图标Tips
    public void ClickSkillIcon()
    {

        //Debug.Log("我点击了宠物技能Tips");
        

    }


    //鼠标按下 显示Tips
    public void Mouse_Down()
    {
        //调用方法显示UI的Tips
        if (obj_SkillTips == null)
        {
            obj_SkillTips = Game_PublicClassVar.Get_function_UI.UI_SkillTips(PetSkillID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet,"2");
        }
    }
    //鼠标松开注销Tips
    public void Mouse_Up()
    {
        if (obj_SkillTips != null)
        {
            Destroy(obj_SkillTips);
            //Debug.Log("B");
        }
    }


}
