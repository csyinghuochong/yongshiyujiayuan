using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_TrainCamp : MonoBehaviour {

    public GameObject Obj_FarmerNum;
    public GameObject Obj_WorkerNum;
    public GameObject Obj_SoldierNum;
    public GameObject Obj_Farm;

    public bool UpdataShowStatus;       //更新展示状态

	// Use this for initialization
	void Start () {

        updataShow();
	
	}
	
	// Update is called once per frame
	void Update () {

        //更新显示
        if (UpdataShowStatus) {
            updataShow();
            UpdataShowStatus = false;
        }

	}

    //更新建筑显示
    void updataShow() { 
        
        //读取各个农民数量
        string farmerNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmerNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        string workerNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("WorkerNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        string soldierNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SoldierNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        string farmerNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmerNumMax", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        string farm = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Farm", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        Obj_FarmerNum.GetComponent<Text>().text = "当前数量："+ farmerNum;
        Obj_WorkerNum.GetComponent<Text>().text = "当前数量：" + workerNum;
        Obj_SoldierNum.GetComponent<Text>().text = "当前数量：" + soldierNum;
        Obj_Farm.GetComponent<Text>().text = "可招募数量：" + farm + "/" + farmerNumMax;

    }


    //召唤农民
    public void Btn_CreateFarmer() {

        string farm = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Farm", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        string farmerNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmerNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        string farmerNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmerNumMax", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        int remainFarmer = int.Parse(farmerNumMax) - int.Parse(farmerNum);
        if (int.Parse(farm) >= remainFarmer)
        {
            //计算剩余的农民
            int remainFarm = int.Parse(farm) - remainFarmer;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Farm", remainFarm.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FarmerNum", farmerNumMax.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
            //文字提示
            if (remainFarmer != 0)
            {
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("成功召唤" + remainFarmer.ToString() + "个农民");
            }
            else {
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("农民已达上限");
            }
            
        }
        else {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Farm", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            int farmerValue = int.Parse(farm) + int.Parse(farmerNum);
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FarmerNum", farmerValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
            //文字提示
            
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("成功召唤" + farm + "个农民");
        }

        updataShow();  //更新显示
    }


    //训练工人
    public void Btn_TrainWorker(){

        string farmerNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmerNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        string farmerNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmerNumMax", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        string workerNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("WorkerNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");

        int addValue = (int)(float.Parse(farmerNumMax) * 0.1f);
        if (int.Parse(farmerNum) >= addValue)
        {
            int worker_value = int.Parse(workerNum) + addValue;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("WorkerNum", worker_value.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            int farmer_value = int.Parse(farmerNum) - addValue;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FarmerNum", farmer_value.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
            //更新建筑显示
            updataShow();
            //文字提示
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("成功训练" + addValue.ToString()+"个工人");

        }

        Debug.Log("训练工人");
           
    }


    //训练士兵
    public void Btn_TrainSoldier()
    {

        string farmerNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmerNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        string farmerNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmerNumMax", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        string soldierNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SoldierNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");

        int addValue = (int)(float.Parse(farmerNumMax) * 0.1f);
        if (int.Parse(farmerNum) >= addValue)
        {
            int soldier_value = int.Parse(soldierNum) + addValue;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SoldierNum", soldier_value.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            int farmer_value = int.Parse(farmerNum) - addValue;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FarmerNum", farmer_value.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
            //更新建筑显示
            updataShow();
            //文字提示
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("成功训练" + addValue.ToString() + "个士兵");
        }

        Debug.Log("训练士兵");

    }

}
