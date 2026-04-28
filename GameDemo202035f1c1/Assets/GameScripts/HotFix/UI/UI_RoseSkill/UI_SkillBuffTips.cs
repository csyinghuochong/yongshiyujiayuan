using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//此脚本用在显示主界面的技能描述
public class UI_SkillBuffTips : MonoBehaviour {

    public string SkillBuffID;
    public GameObject Obj_SkillIcon;    //技能Icon
    public GameObject Obj_SkillName;    //技能名称
    public GameObject Obj_SkillDec;     //技能描述
    public GameObject Obj_SkillCDTime;  //技能冷却CD
    //public GameObject Obj_SkillLv;      //技能等级
    public GameObject Obj_SkillUseMp;   //技能使用魔法
    //public GameObject Obj_SkillBack1;   //背景图1  根据技能描述长度调整大小时用到
    public GameObject Obj_SkillBack2;   //背景图2  根据技能描述长度调整大小时用到
    public GameObject Obj_SkillBottom;  //底层Obj  根据技能描述长度调整大小时用到

    private Sprite skillIcon;            //技能Icon
    private string skillCDTime;
    // Use this for initialization
    void Start () {

		//显示技能Icon
		string skillBuffIconID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffIcon", "ID", SkillBuffID, "SkillBuff_Template");
		object obj = Resources.Load("SkillIcon/BuffIcon/" + skillBuffIconID, typeof(Sprite));

		skillIcon = obj as Sprite;
		Obj_SkillIcon.GetComponent<Image>().sprite = skillIcon;

		//显示技能名字
		string skillName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffName", "ID", SkillBuffID,"SkillBuff_Template");
		Obj_SkillName.GetComponent<Text>().text = skillName;

		//显示技能冷却时间
		skillCDTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffTime", "ID", SkillBuffID, "SkillBuff_Template");
		Obj_SkillCDTime.GetComponent<Text>().text = skillCDTime+"秒";



		//显示技能描述
		string skillDec = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffDescribe", "ID", SkillBuffID, "SkillBuff_Template");

        //爆率显示特殊处理
        if (skillDec.Contains("#baoLv#")) {

            string RankBaoLvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RankBaoLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            if (RankBaoLvStr == "" || RankBaoLvStr == null)
            {
                RankBaoLvStr = "0";
            }

            int baolv_Int = (int)(float.Parse(RankBaoLvStr) * 100);
            skillDec = skillDec.Replace("#baoLv#", "<color=#00ff00>" + baolv_Int.ToString()+ "</color>");

            Obj_SkillName.GetComponent<Text>().text = skillName + "<color=#00ff00>(+" + baolv_Int + "%)</color>";
        }

        Obj_SkillDec.GetComponent<Text>().text = skillDec;
		decLenghtToUISize(skillDec);

		//显示buff剩余时间
		int buffRemainTime = 0;
        //显示buff_4
		Buff_4[] buff4List = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponents<Buff_4>();
		if (buff4List.Length >= 1) {
			for (int i = 0; i < buff4List.Length; i++) {
				if (buff4List[i].BuffID == SkillBuffID) {
					buffRemainTime =(int)(float.Parse(skillCDTime) - buff4List[i].buffTimeSum);
				}
			}
		}
        //显示buff_1
        Buff_1[] buff1List = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponents<Buff_1>();
        if (buff1List.Length >= 1)
        {
            for (int i = 0; i < buff1List.Length; i++)
            {
                if (buff1List[i].BuffID == SkillBuffID)
                {
                    buffRemainTime = (int)(float.Parse(skillCDTime) - buff1List[i].buffTimeSum);
                }
            }
        }

        Obj_SkillUseMp.GetComponent<Text>().text = buffRemainTime.ToString()+"秒";

        //特殊处理
        if (skillCDTime == "999") {
            Obj_SkillCDTime.GetComponent<Text>().text = "永久";
            Obj_SkillUseMp.GetComponent<Text>().text = "永久";
        }

        //特殊处理
        if (skillCDTime == "86400")
        {
            Obj_SkillCDTime.GetComponent<Text>().text = "1天";
            Obj_SkillUseMp.GetComponent<Text>().text = "1天";
        }

        
    }
	
	// Update is called once per frame
	void Update () {

        //实时显示时间
        int buffRemainTime = 0;
        //显示buff_4
        Buff_4[] buff4List = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponents<Buff_4>();
        if (buff4List.Length >= 1)
        {
            for (int i = 0; i < buff4List.Length; i++)
            {
                if (buff4List[i].BuffID == SkillBuffID)
                {
                    buffRemainTime = (int)(float.Parse(skillCDTime) - buff4List[i].buffTimeSum);
                }
            }
        }
        //显示buff_1
        Buff_1[] buff1List = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponents<Buff_1>();
        if (buff1List.Length >= 1)
        {
            for (int i = 0; i < buff1List.Length; i++)
            {
                if (buff1List[i].BuffID == SkillBuffID)
                {
                    buffRemainTime = (int)(float.Parse(skillCDTime) - buff1List[i].buffTimeSum);
                }
            }
        }

        if (skillCDTime != "999" && skillCDTime != "99999" && skillCDTime != "86400") {
            Obj_SkillUseMp.GetComponent<Text>().text = buffRemainTime.ToString() + "秒";
        }

    }

    //根据描述文字的长度对当前UI的大小进行调整
    private void decLenghtToUISize(string dec) {

        //获取需要显示的行数
        int decNum = dec.Length;
        int decRowNum = decNum / 20 + 1;
        Debug.Log("decRowNum= " + decRowNum);

        //Obj_SkillBack1.GetComponent<RectTransform>().sizeDelta = new Vector2(290, 134 + decRowNum*22);
        //Obj_SkillBack1.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 35+decRowNum*-11, 0);
        Obj_SkillBack2.GetComponent<RectTransform>().sizeDelta = new Vector2(290, 124 + decRowNum * 22);
        Obj_SkillBack2.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 35+decRowNum *-11, 0);

        Obj_SkillBottom.transform.localPosition = new Vector3(0, 70-decRowNum * 22, 0);

    }

	public void CloseUI(){
		Destroy (this.gameObject);
	}
}
