using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GuaJiSetting : MonoBehaviour
{

    public GameObject Obj_GuaJiNumShow;
    public float timeSum;
    public GameObject Obj_GuaJiNumShowText;

    public GameObject Obj_ShiQuType_1;
    public GameObject Obj_ShiQuType_2;

    // Start is called before the first frame update
    void Start()
    {
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().AutomaticGuaJiNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayGuaJiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig"));
        timeSum = 1;

        /*
        if (Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiQuType_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig") == "0")
        {
            Obj_ShiQuType_1.SetActive(true);
        }
        else {
            Obj_ShiQuType_1.SetActive(true);
        }

        if (Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiQuType_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig") == "0")
        {
            Obj_ShiQuType_2.SetActive(true);
        }
        else
        {
            Obj_ShiQuType_2.SetActive(true);
        }
        */
        updateSettingShow();

    }

    // Update is called once per frame
    void Update()
    {
        timeSum = timeSum + Time.deltaTime;
        if (timeSum >= 1)
        {
            timeSum = 0;
            Obj_GuaJiNumShow.GetComponent<Text>().text = "今日挂机数量:" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().AutomaticGuaJiNum + "/2000";

            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().AutomaticGuaJiStatus) {
                Obj_GuaJiNumShowText.GetComponent<Text>().text = "取消挂机";
            }
        }
    }

    //点击拾取按钮
    public void Btn_ShiQuType(string type) {

        if (type == "1") {

            if (Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiQuType_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig") == "0")
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiQuType_1", "1","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                //Obj_ShiQuType_1.SetActive(false);
            }
            else
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiQuType_1", "0","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                //Obj_ShiQuType_1.SetActive(true);
            }

        }

        if (type == "2")
        {
            if (Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiQuType_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig") == "0")
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiQuType_2", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                //Obj_ShiQuType_2.SetActive(false);
            }
            else
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiQuType_2", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                //Obj_ShiQuType_2.SetActive(true);
            }
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

        //更新显示
        updateSettingShow();

    }

    //更新显示
    private void updateSettingShow()
    {
        Game_PublicClassVar.Get_game_PositionVar.GuaJiShiQuType_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiQuType_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_game_PositionVar.GuaJiShiQuType_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiQuType_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

        if (Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiQuType_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig") == "0")
        {
            Obj_ShiQuType_1.SetActive(true);
        }
        else
        {
            Obj_ShiQuType_1.SetActive(false);
        }

        if (Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiQuType_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig") == "0")
        {
            Obj_ShiQuType_2.SetActive(true);
        }
        else
        {
            Obj_ShiQuType_2.SetActive(false);
        }

    }
}
