using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_Fortress : MonoBehaviour {

    public string BuildingID;
    public GameObject Obj_BuildingUpLv;
    public GameObject Obj_SoilderNum;   //士兵人数
    public GameObject Obj_EnemyNum;     //敌人人数
    public GameObject Obj_ActTime;
    public GameObject Obj_HintText;
    public bool UpdataShowStatus;   //更新要塞显示
    private bool UpdataActTimeStatus;   //更新进攻时间
    private float updataActTimeSum;
    private float actTime;

	// Use this for initialization

    void Awake() {
        updataActTime();
    }

	void Start () {
        updataFortress();

        updataActTimeSum = 1;
	}
	
	// Update is called once per frame
	void Update () {

        if (UpdataShowStatus) {
            UpdataShowStatus = false;
            updataFortress();
        }

        if (UpdataActTimeStatus) {
            actTime = actTime + Time.deltaTime;
            if (actTime >= updataActTimeSum)
            {
                Debug.Log("滴答");
                updataActTime();        //每秒钟更新一次事件
                actTime = 0;
            }
        }
	}

    //更新要塞显示
    void updataFortress() { 
        
        //获取当前士兵人数
        string soldierNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SoldierNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        BuildingID = Game_PublicClassVar.Get_game_PositionVar.FortressID;
        string soldierNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmValue", "ID", BuildingID, "Building_Template");

        string EnemyNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EnemyNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        Obj_SoilderNum.GetComponent<Text>().text = "要塞守卫:" + soldierNum + "/" + soldierNumMax;
        Obj_EnemyNum.GetComponent<Text>().text = "进攻部队：" + EnemyNum + "人";
        //等级大于15级开启要塞功能
        string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (int.Parse(roseLv) >= 15) {
            UpdataActTimeStatus = true;
        }
        
        if (int.Parse(soldierNumMax) <= int.Parse(EnemyNum))
        {
            string showValue = "本次进要塞很大概率失败！\n请及时去农场招募足够多的要塞士兵来帮你防御要塞！";
            Obj_HintText.GetComponent<Text>().color = Color.red;
            Obj_HintText.GetComponent<Text>().text = showValue;
            Obj_HintText.SetActive(true);
        }
        else {
            string showValue = "你的要塞现在看起来很安全！";
            Obj_HintText.GetComponent<Text>().color = Color.green;
            Obj_HintText.GetComponent<Text>().text = showValue;
            Obj_HintText.SetActive(true);
        }
    }

    void updataActTime() {

        int EnemyTime = (int)(Game_PublicClassVar.Get_game_PositionVar.emenyActTime);
        TimeSpan ts = new TimeSpan(0, 0, EnemyTime);
        Obj_ActTime.GetComponent<Text>().text = "距离敌方进攻：" + ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds;
        
    }
}
