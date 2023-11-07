using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingModel : MonoBehaviour {

    public string BuildingID;
    public GameObject Obj_NamePosition;
    public GameObject Obj_CameraPosition;
    private GameObject Obj_BuildingName;
    private Vector3 vec3_BuildingName;
    private string buildingName;

	// Use this for initialization
	void Start () {

        //修正物体在屏幕中的位置
        vec3_BuildingName = Camera.main.WorldToViewportPoint(Obj_NamePosition.transform.position);
        vec3_BuildingName = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(vec3_BuildingName);

        Obj_BuildingName = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_BuildingName);
        Obj_BuildingName.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingNameSet.transform);
        Obj_BuildingName.transform.localPosition = new Vector3(vec3_BuildingName.x, vec3_BuildingName.y, 0);
        Obj_BuildingName.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

        switch (BuildingID) { 
            case "1":
                buildingName = "国王大厅";
                int openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_GuoWang", "GameMainValue"));
                if (roseLv < openLv)
                {
                    //buildingName = "10级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.GetComponent<Text>().text = openLv + "级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = new Color(0.65f, 0.65f, 0.65f);
                }
                else {
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = Color.green;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.SetActive(false);
                }
                break;

            case "2":
                buildingName = "修炼中心";
                openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_XiuLian", "GameMainValue"));
                if (roseLv < openLv)
                {
                    //buildingName = "10级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.GetComponent<Text>().text = openLv + "级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = new Color(0.65f, 0.65f, 0.65f);
                }
                else {
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = Color.green;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.SetActive(false);
                }
                break;

            case "3":
                buildingName = "荣誉大厅";
                openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_RongYu", "GameMainValue"));
                if (roseLv < openLv)
                {
                    //buildingName = "10级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.GetComponent<Text>().text = openLv + "级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = new Color(0.65f, 0.65f, 0.65f);
                }
                else {
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = Color.green;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.SetActive(false);
                }
                break;

            case "4":
                buildingName = "试炼之塔";
                openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_ShiLian", "GameMainValue"));
                if (roseLv < openLv)
                {
                    //buildingName = "10级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.GetComponent<Text>().text = openLv + "级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = new Color(0.65f,0.65f,0.65f);
                }
                else {
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = Color.green;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.SetActive(false);
                }
                break;

            case "5":
                buildingName = "幸运殿堂";
                openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_XingYunDianTang", "GameMainValue"));
                if (roseLv < openLv)
                {
                    //buildingName = "10级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.GetComponent<Text>().text = openLv + "级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = new Color(0.65f, 0.65f, 0.65f);
                }
                else {
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = Color.green;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.SetActive(false);
                }
                break;

            case "6":
                buildingName = "每日任务";
                openLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "OpenLv_DayTask", "GameMainValue"));
                if (roseLv < openLv)
                {
                    //buildingName = "10级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.GetComponent<Text>().text = openLv + "级开启";
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = new Color(0.65f, 0.65f, 0.65f);
                }
                else {
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().fontSize = 20;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().color = Color.green;
                    Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildLv.SetActive(false);
                }
                break;
        }

        //显示建筑名称
        Obj_BuildingName.GetComponent<UI_BuildingName>().Obj_BuildName.GetComponent<Text>().text = buildingName;

	}
	
	// Update is called once per frame
	void Update () {




	}


}
