using UnityEngine;
using System.Collections;

public class Rose_TaskList_Show : MonoBehaviour {

    public Transform UIPoint_TaskName;
    public GameObject UI_TaskTypeName;
    public string TaskType;
    public GameObject UITog_TaskListShow;
    public GameObject UIImg_TaskListShow;
    public GameObject UIImg_TaskListShow_2;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

	
	}

    public void ifShowSonTask()
    {

        if (TaskType != null) {
            /*  暂时屏蔽打开关闭状态
            //获取当前状态
            string taskOpenStatus = Game_PublicClassVar.Get_IniSprict.InIRead("RoseTaskListShow", TaskType, Game_PublicClassVar.Get_game_PositionVar.Ini_Path + "RoseData_" + Game_PublicClassVar.Get_game_PositionVar.Rose_ID + ".ini");
            //修改当前状态
            if (taskOpenStatus == "1")
            {
                Game_PublicClassVar.Get_IniSprict.InIWrite("RoseTaskListShow", TaskType, "0", Game_PublicClassVar.Get_game_PositionVar.Ini_Path + "RoseData_" + Game_PublicClassVar.Get_game_PositionVar.Rose_ID + ".ini");
            }
            else
            {
                Game_PublicClassVar.Get_IniSprict.InIWrite("RoseTaskListShow", TaskType, "1", Game_PublicClassVar.Get_game_PositionVar.Ini_Path + "RoseData_" + Game_PublicClassVar.Get_game_PositionVar.Rose_ID + ".ini");
            }
            */

            switch (TaskType) { 
                
                case "1":

                    if (Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_1 == "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_1 = "0";
                    }
                    else {
                        Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_1 = "1";
                    }

                break;

                case "2":

                    if (Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_2== "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_2 = "0";
                    }
                    else {
                        Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_2 = "1";
                    }

                break;

                case "3":

                    if (Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_3 == "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_3 = "0";
                    }
                    else {
                        Game_PublicClassVar.Get_game_PositionVar.RoseTaskListShow_3 = "1";
                    }

                break;
            }
            //更新任务日志
            Game_PublicClassVar.Get_game_PositionVar.Rose_TaskListUpdata = true;
            
        }
    }
}
