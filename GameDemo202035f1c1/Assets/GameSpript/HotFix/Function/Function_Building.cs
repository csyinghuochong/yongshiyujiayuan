using UnityEngine;
using System.Collections;

public class Function_Building{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //建筑升级
    public string BuildingUpLv(string buildingID)
    {

        Game_PositionVar game_PositionVar;
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;

        //获取需要的资源
        string resources_TypeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpCostType", "ID", buildingID, "Building_Template");
        string resources_ValueStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpCostValue", "ID", buildingID, "Building_Template");

        string[] resources_Type = resources_TypeStr.Split(';');
        string[] resources_Value = resources_ValueStr.Split(';');

        bool buildingIfUp = true;

        //获取升级建筑的等级以及市政厅的等级
        string buildingLv_Now = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingLv", "ID", buildingID, "Building_Template");
        string MainbuildingID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GovernmentLvID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        string MainbuildingLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingLv", "ID", MainbuildingID, "Building_Template");
        //判定升级的建筑是否为市政厅
        if (buildingID != MainbuildingID) {
            //判定是否超过主建筑等级,超过直接返回空
            if (int.Parse(MainbuildingLv) <= int.Parse(buildingLv_Now))
            {
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先将市政厅等级提升至" + int.Parse(MainbuildingLv)+1 + "级,此建筑等级不能超过政厅等级！");
                return "";
            }
        }

        //循环判定当前升级资源是否足够
        for (int i = 0; i <= resources_Type.Length - 1; i++)
        {
            switch (resources_Type[i])
            {
                //建筑金币
                case "1":

                    //获取当前角色拥有资源
                    int resourcesValue = game_PositionVar.BuildingGold;
                    //开始对比
                    if (resources_ValueStr[i] > resourcesValue)
                    {
                        buildingIfUp = false;
                    }

                    break;
                //农民
                case "2":

                    //获取当前角色拥有资源
                    resourcesValue = game_PositionVar.Farm;
                    //开始对比
                    if (resources_ValueStr[i] > resourcesValue)
                    {
                        buildingIfUp = false;
                    }

                    break;
                //粮食
                case "3":

                    //获取当前角色拥有资源
                    resourcesValue = game_PositionVar.Food;
                    //开始对比
                    if (resources_ValueStr[i] > resourcesValue)
                    {
                        buildingIfUp = false;
                    }

                    break;
                //木材
                case "4":


                    //获取当前角色拥有资源
                    resourcesValue = game_PositionVar.Wood;
                    //开始对比
                    if (resources_ValueStr[i] > resourcesValue)
                    {
                        buildingIfUp = false;
                    }

                    break;
                //石头
                case "5":

                    //获取当前角色拥有资源
                    resourcesValue = game_PositionVar.Stone;
                    //开始对比
                    if (resources_ValueStr[i] > resourcesValue)
                    {
                        buildingIfUp = false;
                    }

                    break;
                //钢铁
                case "6":

                    //获取当前角色拥有资源
                    resourcesValue = game_PositionVar.Iron;
                    //开始对比
                    if (resources_ValueStr[i] > resourcesValue)
                    {
                        buildingIfUp = false;
                    }

                    break;

            }

            //只要某一材料不足,立即跳出循环
            if (buildingIfUp == false)
            {
                Debug.Log("升级材料不足,缺失材料代号为：" + resources_Type[i]);
                break;
            }
        }

        //升级成功
        if (buildingIfUp)
        {
            Debug.Log("升级建筑成功,建筑代号：" + buildingID);

            //获取下一级建筑
            string nextBuildingID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", buildingID, "Building_Template");
            if (nextBuildingID != "0" && nextBuildingID != "")
            {
                //消耗对应材料
                CostBuildingResource(buildingID);
                //设置升级后的建筑等级
                buildingID = nextBuildingID;
                //Debug.Log("升级成功,下一级ID:" + buildingID);
                //获取建筑名称和等级
                string buildingName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingName", "ID", buildingID, "Building_Template");
                string buildingLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingLv", "ID", buildingID, "Building_Template");

                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("升级成功！ "+buildingName+"提升至"+buildingLv+"级");

                //播放音效
                Game_PublicClassVar.Get_function_UI.PlaySource("10009", "1");

                //获取建筑类型,写入下一级建筑ID
                if (nextBuildingID != "0") {
                    //Debug.Log("写入了升级建筑ID");
                    string buildingType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingType", "ID", buildingID, "Building_Template");
                    switch (buildingType)
                    { 
                        //市政大厅
                        case "1":
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GovernmentLvID", nextBuildingID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
                            break;
                        //市政大厅
                        case "2":
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ResidentHouseID", nextBuildingID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
                            break;
                        //市政大厅
                        case "3":
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FarmlandID", nextBuildingID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
                            break;
                        //市政大厅
                        case "4":
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LoggingFieldID", nextBuildingID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
                            break;
                        //市政大厅
                        case "5":
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("StonePitID", nextBuildingID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
                            break;
                        //市政大厅
                        case "6":
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SmeltingPlantID", nextBuildingID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
                            break;
                        //市政大厅
                        case "7":
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CollegeID", nextBuildingID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
                            break;
                        //市政大厅
                        case "8":
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FortressID", nextBuildingID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
                            break;
                        //市政大厅
                        case "9":
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TrainCampID", nextBuildingID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
                            break;

                    }

                    //更新建筑分钟产出
                    UpdataMinuteResource();

                }
                return buildingID;

            }

        }

        return "";
    }

    //消耗建筑升级资源
    public void CostBuildingResource(string buildingID)
    {

        Game_PositionVar game_PositionVar;
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        //获取需要的资源
        string resources_TypeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpCostType", "ID", buildingID, "Building_Template");
        string resources_ValueStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpCostValue", "ID", buildingID, "Building_Template");

        string[] resources_Type = resources_TypeStr.Split(';');
        string[] resources_Value = resources_ValueStr.Split(';');

        //循环判定当前升级资源是否足够
        for (int i = 0; i <= resources_Type.Length - 1; i++)
        {
            Debug.Log("扣钱");
            switch (resources_Type[i])
            {

                //建筑金币
                case "1":

                    //获取当前角色拥有资源
                    int resourcesValue = game_PositionVar.BuildingGold;
                    resourcesValue = resourcesValue - int.Parse(resources_Value[i]);
                    game_PositionVar.BuildingGold = resourcesValue;
                    Debug.Log("扣钱金币");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BuildingGold", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                    break;

                //农民
                case "2":

                    //获取当前角色拥有资源
                    resourcesValue = game_PositionVar.Farm;
                    resourcesValue = resourcesValue - int.Parse(resources_Value[i]);
                    game_PositionVar.Farm = resourcesValue;
                    Debug.Log("扣钱农民");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Farm", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                    break;

                //粮食
                case "3":

                    //获取当前角色拥有资源
                    resourcesValue = game_PositionVar.Food;
                    resourcesValue = resourcesValue - int.Parse(resources_Value[i]);
                    game_PositionVar.Food = resourcesValue;
                    Debug.Log("扣钱粮食");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Food", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                    break;

                //木材
                case "4":

                    //获取当前角色拥有资源
                    resourcesValue = game_PositionVar.Wood;
                    resourcesValue = resourcesValue - int.Parse(resources_Value[i]);
                    game_PositionVar.Wood = resourcesValue;
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Wood", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                    break;

                //石头
                case "5":

                    //获取当前角色拥有资源
                    resourcesValue = game_PositionVar.Stone;
                    resourcesValue = resourcesValue - int.Parse(resources_Value[i]);
                    game_PositionVar.Stone = resourcesValue;
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Stone", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                    break;

                //钢铁
                case "6":

                    //获取当前角色拥有资源
                    resourcesValue = game_PositionVar.Iron;
                    resourcesValue = resourcesValue - int.Parse(resources_Value[i]);
                    game_PositionVar.Iron = resourcesValue;
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Iron", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                    break;

            }

            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
        }

    }



    //传入类型返回类型名称
    public string ReturnResourceName (string resouceType)
    {
        switch (resouceType)
        {

            //建筑金币
            case "1":
                return "金币";
            break;

            //农民
            case "2":
                return "农民";
            break;

            //粮食
            case "3":
                return "粮食";
            break;

            //木材
            case "4":
                return "木材";
            break;

            //石头
            case "5":
                return "石头";
            break;

            //钢铁
            case "6":
                return "钢铁";
            break;

            //主角经验
            case "7":
                return "主角经验";
            break;

        }
        return "";
    }


    //传入类型返回当前类型建筑的工人数量
    public string ReturnWorkerNum(string resouceType)
    {
        switch (resouceType)
        {

            //市政大厅
            case "1":
                string nowFarmValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GovernmentWorker", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                return nowFarmValue;
                break;

            //民居
            case "2":
                nowFarmValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ResidentHouseWorker", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                return nowFarmValue;
                break;

            //农田
            case "3":
                nowFarmValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmlandWorker", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                return nowFarmValue;
                break;

            //伐木场
            case "4":
                nowFarmValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LoggingFieldWorker", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                return nowFarmValue;
                break;

            //采石场
            case "5":
                nowFarmValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StonePitWorker", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                return nowFarmValue;
                break;

            //冶炼厂
            case "6":
                nowFarmValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SmeltingPlantWorker", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                return nowFarmValue;
                break;

            //学院
            case "7":
                nowFarmValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CollegeWorker", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                return nowFarmValue;
                break;

        }
        return "";
    }


    //传入资源类型返回资源数量
    public int RetuenTypeResource(string ResourceType)
    {

        Game_PositionVar game_PositionVar;
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;

            switch (ResourceType)
            {

                //建筑金币
                case "1":

                    //获取当前角色拥有资源
                    return game_PositionVar.BuildingGold;
                    
                    break;

                //农民
                case "2":

                    //获取当前角色拥有资源
                    return game_PositionVar.Farm;

                    break;

                //粮食
                case "3":

                    //获取当前角色拥有资源
                    return game_PositionVar.Food;

                    break;

                //木材
                case "4":

                    //获取当前角色拥有资源
                    return game_PositionVar.Wood;

                    break;

                //石头
                case "5":

                    //获取当前角色拥有资源
                    return game_PositionVar.Stone;

                    break;

                //钢铁
                case "6":

                    //获取当前角色拥有资源
                    return game_PositionVar.Iron;

                    break;

            }

            return -1;

    }


    //传入资源类型发送对应资源
    public void SendResource(string ResourceType,int ResourceValue)
    {

        Game_PositionVar game_PositionVar;
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;

        switch (ResourceType)
        {

            //建筑金币
            case "1":

                //获取当前角色拥有资源
                int resourcesValue = game_PositionVar.BuildingGold;
                resourcesValue = resourcesValue + ResourceValue;
                //数额小于0,此值等于0
                if (resourcesValue <= 0)
                {
                    resourcesValue = 0;
                }
                game_PositionVar.BuildingGold = resourcesValue;

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BuildingGold", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                Game_PublicClassVar.Get_game_PositionVar.BuildingGold = resourcesValue;
                break;

            //农民
            case "2":

                //获取当前角色拥有资源
                resourcesValue = game_PositionVar.Farm;
                resourcesValue = resourcesValue + ResourceValue;
                //数额小于0,此值等于0
                if (resourcesValue <= 0)
                {
                    resourcesValue = 0;
                }
                game_PositionVar.Farm = resourcesValue;
                Debug.Log("扣钱农民");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Farm", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                Game_PublicClassVar.Get_game_PositionVar.Farm = resourcesValue;
                break;

            //粮食
            case "3":

                //获取当前角色拥有资源
                resourcesValue = game_PositionVar.Food;
                resourcesValue = resourcesValue + ResourceValue;
                //数额小于0,此值等于0
                if (resourcesValue <= 0)
                {
                    resourcesValue = 0;
                }
                game_PositionVar.Food = resourcesValue;
                Debug.Log("扣钱粮食");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Food", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                Game_PublicClassVar.Get_game_PositionVar.Food = resourcesValue;
                break;

            //木材
            case "4":

                //获取当前角色拥有资源
                resourcesValue = game_PositionVar.Wood;
                resourcesValue = resourcesValue + ResourceValue;
                //数额小于0,此值等于0
                if (resourcesValue <= 0)
                {
                    resourcesValue = 0;
                }
                game_PositionVar.Wood = resourcesValue;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Wood", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                Game_PublicClassVar.Get_game_PositionVar.Wood = resourcesValue;
                break;

            //石头
            case "5":

                //获取当前角色拥有资源
                resourcesValue = game_PositionVar.Stone;
                resourcesValue = resourcesValue + ResourceValue;
                game_PositionVar.Stone = resourcesValue;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Stone", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                Game_PublicClassVar.Get_game_PositionVar.Stone = resourcesValue;
                break;

            //钢铁
            case "6":

                //获取当前角色拥有资源
                resourcesValue = game_PositionVar.Iron;
                resourcesValue = resourcesValue + ResourceValue;
                //数额小于0,此值等于0
                if (resourcesValue <= 0)
                {
                    resourcesValue = 0;
                }
                game_PositionVar.Iron = resourcesValue;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Iron", resourcesValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                Game_PublicClassVar.Get_game_PositionVar.Iron = resourcesValue;
                break;

        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");



    }

    //更新分钟产出资源
    public void UpdataMinuteResource() {

        //  ResidentHouseID="2001" FarmlandID="3001" LoggingFieldID="4001" StonePitID="5001" SmeltingPlantID="6001" CollegeID="7001" FortressID="8001" TrainCampID="9001"

        //获取当前建筑ID
        Game_PositionVar game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        WWWSet wwwSet = Game_PublicClassVar.Get_wwwSet;

        string ResidentHouseID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ResidentHouseID", "ID", wwwSet.RoseID, "RoseBuilding");      //产出建筑资金
        string FarmlandID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmlandID", "ID", wwwSet.RoseID, "RoseBuilding");    //产出农田
        string LoggingFieldID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LoggingFieldID", "ID", wwwSet.RoseID, "RoseBuilding");    //产出木材
        string StonePitID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StonePitID", "ID", wwwSet.RoseID, "RoseBuilding");        //产出矿石
        string SmeltingPlantID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SmeltingPlantID", "ID", wwwSet.RoseID, "RoseBuilding");  //产出钢铁
        string CollegeID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CollegeID", "ID", wwwSet.RoseID, "RoseBuilding");     //产出主角经验
        //string FortressID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FortressID", "ID", RoseID, "RoseBuilding");
        string TrainCampID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TrainCampID", "ID", wwwSet.RoseID, "RoseBuilding");

        string BuildingGold_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", ResidentHouseID, "Building_Template");            //建筑金币
        string Farm_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", TrainCampID, "Building_Template");           	    //农民,暂定
        string Food_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", FarmlandID, "Building_Template");         //粮食
        string Wood_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", LoggingFieldID, "Building_Template");              //木材
        string Stone_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", StonePitID, "Building_Template");                //石头
        string Iron_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", SmeltingPlantID, "Building_Template");	                //钢铁
        string RoseExp_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", CollegeID, "Building_Template");                //角色经验


        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MinuteBuildingGold", BuildingGold_Minute, "ID", wwwSet.RoseID, "RoseBuilding");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MinuteFarm", Farm_Minute, "ID", wwwSet.RoseID, "RoseBuilding");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MinuteFood", Food_Minute, "ID", wwwSet.RoseID, "RoseBuilding");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MinuteWood", Wood_Minute, "ID", wwwSet.RoseID, "RoseBuilding");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MinuteStone", Stone_Minute, "ID", wwwSet.RoseID, "RoseBuilding");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MinuteIron", Iron_Minute, "ID", wwwSet.RoseID, "RoseBuilding");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MinuteRoseExp", RoseExp_Minute, "ID", wwwSet.RoseID, "RoseBuilding");

        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");

        //设置分钟产出
        game_PositionVar.MinuteBuildingGold = int.Parse(BuildingGold_Minute);
        game_PositionVar.MinuteFarm = int.Parse(Farm_Minute);
        game_PositionVar.MinuteFood = int.Parse(Food_Minute);
        game_PositionVar.MinuteWood = int.Parse(Wood_Minute);
        game_PositionVar.MinuteStone = int.Parse(Stone_Minute);
        game_PositionVar.MinuteIron = int.Parse(Iron_Minute);
        game_PositionVar.MinuteRoseExp = int.Parse(RoseExp_Minute);

    }

    //传入类型获取当前类型建筑的每分钟产出
    public string retrunBuildingMinute(string buildingType) {

        WWWSet wwwSet = Game_PublicClassVar.Get_wwwSet;
        switch (buildingType) { 
            
                            //建筑金币
                case "1":

                    //获取当前角色拥有资源
                    string ResidentHouseID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ResidentHouseID", "ID", wwwSet.RoseID, "RoseBuilding");      //产出建筑资金
                    string BuildingGold_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", ResidentHouseID, "Building_Template");            //建筑金币
                    return BuildingGold_Minute;
                    
                    break;

                //农民
                case "2":

                    //获取当前角色拥有资源
                    /*
                    string FarmlandID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmlandID", "ID", wwwSet.RoseID, "RoseBuilding");    //产出农田
                    string Farm_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", TrainCampID, "Building_Template");
                    return Farm_Minute;
                    */
                    break;

                //粮食
                case "3":

                    //获取当前角色拥有资源
                    string FarmlandID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmlandID", "ID", wwwSet.RoseID, "RoseBuilding");    //产出农田
                    string Food_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", FarmlandID, "Building_Template");         //粮食
                    return Food_Minute;

                    break;

                //木材
                case "4":

                    //获取当前角色拥有资源
                    string LoggingFieldID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LoggingFieldID", "ID", wwwSet.RoseID, "RoseBuilding");    //产出木材
                    string Wood_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", LoggingFieldID, "Building_Template"); 
                    return Wood_Minute;

                    break;

                //石头
                case "5":

                    //获取当前角色拥有资源
                    string StonePitID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StonePitID", "ID", wwwSet.RoseID, "RoseBuilding");        //产出矿石
                    string Stone_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", StonePitID, "Building_Template");                //石头
                    return Stone_Minute;

                    break;

                //钢铁
                case "6":

                    //获取当前角色拥有资源
                    string SmeltingPlantID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SmeltingPlantID", "ID", wwwSet.RoseID, "RoseBuilding");  //产出钢铁
                    string Iron_Minute = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteValue", "ID", SmeltingPlantID, "Building_Template");	                //钢铁
                    return Iron_Minute;

                    break;


            }

        return "0";

        }
}
