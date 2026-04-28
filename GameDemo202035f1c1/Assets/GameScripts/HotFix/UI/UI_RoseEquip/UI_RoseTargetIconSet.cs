using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoseTargetIconSet : MonoBehaviour {

    public bool IfUpdateTarget;

    public GameObject TargetObjSet;
    public GameObject obj_RoseActTarget;
    public GameObject Obj_MonsterName;
    public GameObject Obj_MonsterLv;
    public GameObject Obj_BossHeadIcon;
    public GameObject Obj_MonsterHp;
    // Use this for initialization
    void Start () {
        IfUpdateTarget = true;

    }
	
	// Update is called once per frame
	void Update () {

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget != null && Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget != obj_RoseActTarget)
        {
            IfUpdateTarget = true;
        }

        if (IfUpdateTarget) {

            IfUpdateTarget = false;

            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget != null&& Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget.GetComponent<AI_1>()!=null)
            {
                TargetObjSet.SetActive(true);
                obj_RoseActTarget = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
                //读取头像和名称血量
                string monsterID = obj_RoseActTarget.GetComponent<AI_1>().AI_ID.ToString();
                string monsterName = obj_RoseActTarget.GetComponent<AI_Property>().AI_Name;

                string ifBoss = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBoss", "ID", monsterID, "Monster_Template");
                if (ifBoss == "1")
                {
                    TargetObjSet.SetActive(false);
                }
                else {
                    //怪物类型
                    string monsterRace = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterRace", "ID", monsterID, "Monster_Template");
                    switch (monsterRace)
                    {
                        case "0":
                            monsterName = "[" + "通用" + "] " + monsterName;
                            break;

                        case "1":
                            monsterName = "[" + "野兽系" + "] " + monsterName;
                            break;

                        case "2":
                            monsterName = "[" + "人形系" + "] " + monsterName;
                            break;

                        case "3":
                            monsterName = "[" + "恶魔系" + "] " + monsterName;
                            break;
                    }

                    string monsterLv = obj_RoseActTarget.GetComponent<AI_Property>().AI_Lv.ToString();

                    Obj_MonsterName.GetComponent<Text>().text = monsterName;
                    Obj_MonsterLv.GetComponent<Text>().text = monsterLv + "级";

                    //显示BOSS头像
                    object obj = Resources.Load("HeadIcon/BossIcon/" + monsterID, typeof(Sprite));
                    if (obj != null)
                    {
                        Sprite Icon = obj as Sprite;
                        Obj_BossHeadIcon.GetComponent<Image>().sprite = Icon;
                    }
                }
            }
            else {
                TargetObjSet.SetActive(false);
            }
        }

        //更新血量
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget != null && Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget.GetComponent<AI_1>() != null)
        {
            Obj_MonsterHp.GetComponent<Image>().fillAmount = (float)obj_RoseActTarget.GetComponent<AI_Property>().AI_Hp / (float)obj_RoseActTarget.GetComponent<AI_Property>().AI_HpMax;
        }
        else
        {
            TargetObjSet.SetActive(false);
        }

    }
}
