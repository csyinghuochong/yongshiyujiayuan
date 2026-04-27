using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using System;

public class UITerritory : MonoBehaviour
{

    public string BuildingType;         //建筑类型
    public string BuildingID;           //建筑ID
    public bool BuildingUpdataStatus;   //建筑更新状态
    public bool BuildingUpdataUIStatus;   //建筑更新UI状态
    //UI部分
    public GameObject Obj_BuildingName;         //建筑名字
    public GameObject Obj_BuildingLv;           //建筑等级
    public GameObject Obj_BuildingResouce;      //建筑资源
    public GameObject Obj_BuildingDes;          //建筑描述

    public GameObject Obj_NextBuildingResouce;      //下一级资源产量
    public GameObject Obj_NextBuildingWorker;      //下一级工人数量
    public GameObject Obj_NowWorkerNum;             //当前工人数量

    /*
    public GameObject Obj_UpLvCost1_ValueType;  //建筑升级消耗的第一个资源图标
    public GameObject Obj_UpLvCost1_Value;      //建筑升级消耗的第一个资源量
    public GameObject Obj_UpLvCost2_ValueType;  //建筑升级消耗的第二个资源图标
    public GameObject Obj_UpLvCost2_Value;      //建筑升级消耗的第二个资源量
    public GameObject Obj_UpLvCost3_ValueType;  //建筑升级消耗的第三个资源图标
    public GameObject Obj_UpLvCost3_Value;      //建筑升级消耗的第三个资源量
    */

    private Game_PositionVar game_PositionVar;

	// Use this for initialization
	void Start () {
        /*
        DateTime dataTime_Now = System.DateTime.Now;        //获取当前时间
        DateTime dataTime_Complate = dataTime_Now.AddMinutes(30);
        if (DateTime.Compare(dataTime_Complate, dataTime_Now) > 0) { 
            //领取奖励

        }
         */
        //Debug.Log("datatime = " + datatime);
	}
	
	// Update is called once per frame
	void Update () {
        //更新建筑
        if (!BuildingUpdataStatus)
        {
            BuildingUpdataStatus = true;
            updataBuildingID();     //更新ID

        }
        //更新建筑UI状态
        if (!BuildingUpdataUIStatus) {
            BuildingUpdataUIStatus = true;
            updataBuildingUI();     //更新UI
        }
	}

    void LateUpdate() {

    }

    //更新建筑ID
    void updataBuildingID() {

        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;

        switch (BuildingType)
        {

            //市政厅等级
            case "1":
                BuildingID = game_PositionVar.GovernmentLvID;
                break;

            //民居
            case "2":
                BuildingID = game_PositionVar.ResidentHouseID;
                break;

            //农田
            case "3":
                BuildingID = game_PositionVar.FarmlandID;
                break;

            //伐木场
            case "4":
                BuildingID = game_PositionVar.LoggingFieldID;
                break;

            //采石场
            case "5":
                BuildingID = game_PositionVar.StonePitID;
                break;

            //冶炼厂
            case "6":
                BuildingID = game_PositionVar.SmeltingPlantID;
                break;

            //学院
            case "7":
                BuildingID = game_PositionVar.CollegeID;
                break;
        }
    }

    /*
    public void Btn_BuildingUp() {

        string returnBuildingID = Game_PublicClassVar.Get_function_Building.BuildingUpLv(BuildingID);
        Debug.Log("returnBuildingID = " + returnBuildingID);
        //不为""表示升级成功
        if (returnBuildingID != "") {
            BuildingID = returnBuildingID;
            BuildingUpdataUIStatus = false;       //刷新UI
        }

    }
    */

    //领地训练
    public void Btn_Territory() { 
    
        //获取当前建筑需要工人最大数量
        string farmMaxValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmValue", "ID", BuildingID, "Building_Template");
        //获取当前工人最大数量
        string workerNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("WorkerNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        if (int.Parse(workerNum) >= int.Parse(farmMaxValue))
        {
            int workerValue = int.Parse(workerNum) - int.Parse(farmMaxValue);       //工人剩下余数
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("WorkerNum", workerValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");

            //获取发送资源的类型
            string minuteType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteType", "ID", BuildingID, "Building_Template");
            string minuteValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", BuildingID, "Building_Template");
            int addValue = int.Parse(minuteValue) * 60;
            Game_PublicClassVar.Get_function_Building.SendResource(minuteType, addValue);
            //获取资源名称
            string resourceName = Game_PublicClassVar.Get_function_Building.ReturnResourceName(minuteType);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(farmMaxValue + "个工人经过努力的工作获得" + resourceName + "+" + addValue.ToString());
            //updataBuildingUI(); //更新界面显示
            for (int i = 2; i <= 6; i++) {
               GameObject obj= this.transform.parent.Find("Territory_" + i).gameObject;
               if (obj != null) {
                   obj.GetComponent<UITerritory>().BuildingUpdataUIStatus = false;
               }
            }
        }
        else {
            Debug.Log("工人人数不足,不能工作");
        }
    }

    //更新UI相关数据
    void updataBuildingUI() {

        //显示建筑名称
        string buildingName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingName", "ID", BuildingID, "Building_Template");
        Obj_BuildingName.GetComponent<Text>().text = buildingName;
        //显示建筑等级
        string buildingLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingLv", "ID", BuildingID, "Building_Template");
        Obj_BuildingLv.GetComponent<Text>().text = "等级：" + buildingLv;
        //显示建筑产出资源
        string resouceType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteType", "ID", BuildingID, "Building_Template");
        string resouceTypeName = Game_PublicClassVar.Get_function_Building.ReturnResourceName(resouceType);
        string resouceValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", BuildingID, "Building_Template");
        //Obj_BuildingResouce.GetComponent<Text>().text = "当前产出" + resouceTypeName + ":" + resouceValue + "/分钟";
        Obj_BuildingResouce.GetComponent<Text>().text = "";
        //显示建筑描述
        string buildingDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingDes_Farm", "ID", BuildingID, "Building_Template");
        Obj_BuildingDes.GetComponent<Text>().text = buildingDes;

        //显示下一级建筑信息
        //获取下一级建筑
        string nextBuildingID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", BuildingID, "Building_Template");
        if (nextBuildingID != "0" && nextBuildingID != "")
        {
            string nextResouceValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", nextBuildingID, "Building_Template");
            Obj_NextBuildingResouce.GetComponent<Text>().text = resouceTypeName + "产量：" + nextResouceValue;
            string nextFarmValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmValue", "ID", nextBuildingID, "Building_Template");
            Obj_NextBuildingWorker.GetComponent<Text>().text = "工人上限：" + nextFarmValue;
            string maxFarmValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmValue", "ID", BuildingID, "Building_Template");
            //获取当前工人数量
            string buildType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingType", "ID", BuildingID, "Building_Template");
            //string nowFarmValue = Game_PublicClassVar.Get_function_Building.ReturnWorkerNum(buildType);
            string nowFarmValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("WorkerNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Obj_NowWorkerNum.GetComponent<Text>().text = "当前工人：" + nowFarmValue + "/" + maxFarmValue;
            //条件不足改变UI颜色
            if (int.Parse(nowFarmValue) < int.Parse(maxFarmValue))
            {
                Obj_NowWorkerNum.GetComponent<Text>().color = Color.red;
            }
            else {
                Obj_NowWorkerNum.GetComponent<Text>().color = Color.white;
            }

        }

        //显示升级建筑消耗
        /*
        string resources_TypeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpCostType", "ID", BuildingID, "Building_Template");
        string resources_ValueStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpCostValue", "ID", BuildingID, "Building_Template");

        string[] resources_Type = resources_TypeStr.Split(';');
        string[] resources_Value = resources_ValueStr.Split(';');
        
        //循环显示资源
        for (int i = 0; i <= resources_Type.Length - 1; i++)
        {
            switch (i) { 
                
                case 0:
                    //显示第一个
                    showUpCostResouce(Obj_UpLvCost1_ValueType, resources_Type[0], Obj_UpLvCost1_Value, resources_Value[0]);
                break;

                case 1:
                    //显示第二个
                    showUpCostResouce(Obj_UpLvCost2_ValueType, resources_Type[1], Obj_UpLvCost2_Value, resources_Value[1]);
                break;

                case 2:
                    //显示第三个
                    showUpCostResouce(Obj_UpLvCost3_ValueType, resources_Type[2], Obj_UpLvCost3_Value, resources_Value[2]);
                break;
            }
        }
        */    
    
    }

    /*
    void showUpCostResouce(GameObject obj_Icon,string IconType,GameObject obj_CostValue,string costValue) { 
    
        //暂时无视图片
        obj_CostValue.GetComponent<Text>().text = costValue;

        //获取当前类型有多少货币
        int resouceValue = Game_PublicClassVar.Get_function_Building.RetuenTypeResource(IconType);
        if (int.Parse(costValue) > resouceValue)
        {
            obj_CostValue.GetComponent<Text>().color = Color.red;           //不足红色
        }
        else {
            obj_CostValue.GetComponent<Text>().color = Color.black;         //满足黑色
        }
    
    }
    */


}
