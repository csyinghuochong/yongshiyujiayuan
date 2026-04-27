using UnityEngine;
using System.Collections;

public class Scence_TanSuo : MonoBehaviour
{

    //private float disRose;      //距离主角的距离
    public string TanSuoIDOne;
    public string TanSuoID;      //故事ID

    //public GameObject MoveTypeObj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //判定当前探索ID是否已经存在
        string tansuoListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TanSuoListID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (tansuoListStr != "" && tansuoListStr != "0")
        {
            string[] tansuoListID = tansuoListStr.Split(';');
            for (int i = 0; i < tansuoListID.Length; i++)
            {
                if (tansuoListID[i] == TanSuoIDOne)
                {
                    Destroy(this.gameObject);
                }
            }
        }
	}


    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("OnTriggerEnterOnTriggerEnterOnTriggerEnter !!!!");
        //当碰撞的是主角
        if (collider.gameObject.layer == 14)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_SceneTanSuo = this.gameObject;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        //Debug.Log("OnTriggerExitOnTriggerExitOnTriggerExit !!!!");
        //当碰撞的是主角
        if (collider.gameObject.layer == 14)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_SceneTanSuo = null;
        }
    }



    public void RoseTanSuo() {
        //添加唯一探索ID
        AddTanSuo(TanSuoIDOne);
        //根据探索添加奖励
        string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RewardItemID", "ID", TanSuoID, "SceneTanSuo_Template");
        int itemNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RewardItemNum", "ID", TanSuoID, "SceneTanSuo_Template"));
        Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID, itemNum,"1",0,"0",true,"4");

        //提示文字
        string hintStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HintStr", "ID", TanSuoID, "SceneTanSuo_Template");
        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", itemID, "Item_Template");
        Game_PublicClassVar.Get_function_UI.GameHint(hintStr + itemName + " * " + itemNum.ToString());
        //销毁自身
        Destroy(this.gameObject);
    }

    void AddTanSuo(string addTanSuoID) {

        string tansuoListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TanSuoListID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (tansuoListStr == "" && tansuoListStr == "0")
        {
            tansuoListStr = addTanSuoID;
        }
        else {
            tansuoListStr = tansuoListStr + ";" + addTanSuoID;
        }
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TanSuoListID", tansuoListStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
    }
}
