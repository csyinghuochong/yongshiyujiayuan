using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//此脚本用在显示主界面的技能描述
public class UI_SkillTips : MonoBehaviour {

    public string SkillID;
    public string SkillType;            //技能类型  1:表示角色技能  2表示宠物技能
    public GameObject Obj_SkillIcon;    //技能Icon
    public GameObject Obj_SkillName;    //技能名称
    public GameObject Obj_SkillDec;     //技能描述
    public GameObject Obj_SkillCDTime;  //技能冷却CD
    public GameObject Obj_SkillLv;      //技能等级
    public GameObject Obj_SkillUseMp;   //技能使用魔法
    public GameObject Obj_SkillBack1;   //背景图1  根据技能描述长度调整大小时用到
    public GameObject Obj_SkillBack2;   //背景图2  根据技能描述长度调整大小时用到
    public GameObject Obj_SkillBottom;  //底层Obj  根据技能描述长度调整大小时用到

    private Sprite skillIcon;            //技能Icon

	// Use this for initialization
	void Start () {

        //SkillID = "60010002";

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        string skillIDFirst = SkillID.Substring(0,1);
        switch (skillIDFirst)
        {
            //道具描述
            case "1":

            break;

            case "6":
                
                //显示技能Icon
                string skillIconID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillIcon", "ID", SkillID, "Skill_Template");
                object obj = null;
                switch (SkillType) {
                        //玩家技能
                    case "1":
                        obj = Resources.Load("SkillIcon/" + skillIconID, typeof(Sprite));
                        break;
                        //宠物技能
                    case "2":
                        obj = Resources.Load("SkillIcon/PetSkill/" + skillIconID, typeof(Sprite));
                        break;
                }

                skillIcon = obj as Sprite;
                Obj_SkillIcon.GetComponent<Image>().sprite = skillIcon;

                //显示技能名字
                string skillName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillName", "ID", SkillID,"Skill_Template");
                Obj_SkillName.GetComponent<Text>().text = skillName;

                //显示技能等级
                string skillLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLv", "ID", SkillID,"Skill_Template");
                Obj_SkillLv.GetComponent<Text>().text = skillLv;

                //显示技能冷却时间
                string skillCDTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", SkillID, "Skill_Template");
                Obj_SkillCDTime.GetComponent<Text>().text = skillCDTime+"秒";

                //显示技能描述
                string skillDec = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillDescribe", "ID", SkillID, "Skill_Template");
                Obj_SkillDec.GetComponent<Text>().text = skillDec;
                decLenghtToUISize(skillDec);

                //显示技能消耗魔法
                string skillUseMp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillUseMP", "ID", SkillID, "Skill_Template");
                Obj_SkillUseMp.GetComponent<Text>().text = skillUseMp;
                
            break;
            
        }



    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //根据描述文字的长度对当前UI的大小进行调整
    private void decLenghtToUISize(string dec) {

        //获取需要显示的行数
        int decNum = dec.Length;
        int decRowNum = decNum / 20+1;
        //Debug.Log("decRowNum= " + decRowNum);

        Obj_SkillBack1.GetComponent<RectTransform>().sizeDelta = new Vector2(270, 130 + decRowNum*22);
        Obj_SkillBack1.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 35+decRowNum*-11, 0);
        //Obj_SkillBack2.GetComponent<RectTransform>().sizeDelta = new Vector2(290, 124 + decRowNum * 22);
        //Obj_SkillBack2.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 35+decRowNum *-11, 0);

        Obj_SkillBottom.transform.localPosition = new Vector3(0, 70-decRowNum * 22, 0);

    }
}
