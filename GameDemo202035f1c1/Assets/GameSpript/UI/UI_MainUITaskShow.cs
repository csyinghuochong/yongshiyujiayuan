using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_MainUITaskShow : MonoBehaviour {

	public string TaskID;
	public bool UpdataTask;
	public GameObject Obj_TaskName;
	public GameObject Obj_TaskTarget;
	public GameObject Obj_TaskType_1;
	public GameObject Obj_TaskType_2;
	public GameObject Obj_TaskType_3;
    public GameObject Obj_TaskType_4;

    // Use this for initialization
    void Start () {

        //界面适配
        //Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject.transform.parent.gameObject);

    }
	
	// Update is called once per frame
	void Update () {

		//更新显示任务
		if (UpdataTask) {
			UpdataTask = false;

			//清理图表显示
			clearnTaskTypeShow ();

			string taskName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskName", "ID", TaskID, "Task_Template");
			string taskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", TaskID, "Task_Template");
			string taskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", TaskID, "Task_Template");
			string taskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", TaskID, "Task_Template");
			string taskTargetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", TaskID, "Task_Template");
            string taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", TaskID, "Task_Template");
            string taskSonType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskSonType", "ID", TaskID, "Task_Template");

            //名称特殊处理
            switch (taskType) {

                //试炼任务
                case "2":
                    if (taskSonType == "1") {
                        string shilianNumStr =  Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        if (shilianNumStr == "") {
                            shilianNumStr = "0";
                        }
                        int shilianNum = int.Parse(shilianNumStr);
                        shilianNum = shilianNum + 1;
                        string langStr_1 = Game_PublicClassVar.gameSettingLanguge.LoadLocalization("第");
                        string langStr_2 = Game_PublicClassVar.gameSettingLanguge.LoadLocalization("环");
                        taskName = taskName + "(" + langStr_1 + shilianNum + langStr_2 + ")";
						Obj_TaskType_2.SetActive (true);
                    }
                break;

                //日常任务
                case "3":
                    string shimenNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMenTaskNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    if (shimenNumStr == "")
                    {
                        shimenNumStr = "0";
                    }
                    int shiMenNum = int.Parse(shimenNumStr);
                    shiMenNum = shiMenNum + 1;
                    string langStr_11 = Game_PublicClassVar.gameSettingLanguge.LoadLocalization("第");
                    string langStr_22 = Game_PublicClassVar.gameSettingLanguge.LoadLocalization("环");
                    taskName = taskName + "(" + langStr_11 + shiMenNum + langStr_22 + ")";
					Obj_TaskType_3.SetActive (true);
                    break;

				default:
					Obj_TaskType_1.SetActive (true);
					break;
            }

			//获取任务当前目标文本
			string taskText = "";
			string value1 = "";
			string value2 = "";
			string value3 = "";

			if(taskTarget1!="0"){
				value1= returnTaskTagetText(taskTarget1,taskTargetType,"1");
			}
			if(taskTarget2!="0"){
				value2= returnTaskTagetText(taskTarget2,taskTargetType,"2");
			}
			if(taskTarget3!="0"){
				value3= returnTaskTagetText(taskTarget3,taskTargetType,"3");
			}

			taskText = value1 + value2 + value3;

            //显示找人任务
            if (taskTargetType == "3")
            {
                //获取寻找人的名称
                string completeNpcID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteNpcID", "ID", TaskID, "Task_Template");
                string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", completeNpcID, "Npc_Template");
                string langStr = Game_PublicClassVar.gameSettingLanguge.LoadLocalization("寻找");
                taskText = langStr + "：" + npcName;
            }

			//显示任务名称
			Obj_TaskName.GetComponent<Text>().text = taskName;
			//显示任务进度
			Obj_TaskTarget.GetComponent<Text>().text = taskText;
		}
	}

	//返回任务目标描述文本，参数1：任务目标ID  参数2：任务目标类型  参数3：当前第几个目标
	private string returnTaskTagetText(string targetID,string targetType,string targetNum){
		string taskText = "";
		//根据任务目标类型返回任务前缀的文字、
		string targetText = "";
		string taskTargetName = "";
		string taskTargetValue = "";
		string taskNowTargetValue = "";
		if (targetID != "0") {
			//获取任务完成度
			taskTargetValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue" + targetNum, "ID", TaskID, "Task_Template");
            taskNowTargetValue = Game_PublicClassVar.Get_function_Task.TaskReturnValue(TaskID, targetNum);
            if (taskNowTargetValue == "") {
                taskNowTargetValue = "0";
            }
            //Debug.Log("targetID = " + targetID + ";targetType = " + targetType + ";taskTargetValue = " + taskTargetValue + ";taskNowTargetValue = " + taskNowTargetValue);
			//根据任务目标类型到各个表中获取任务名称
			switch (targetType) {
			case "1":
				targetText = "击杀";
				taskTargetName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("MonsterName", "ID", targetID, "Monster_Template");
				break;

			case "2":
				targetText = "获得";
				taskTargetName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemName", "ID", targetID, "Item_Template");
				break;

			case "3":
				targetText = "对话";
				taskTargetName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("NpcName", "ID", targetID, "Npc_Template");
				break;

            case "4":
                targetText = "等级";
                taskTargetName = "提升至";
                taskTargetName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(taskTargetName);
                break;


            case "5":
                targetText = "击杀";
                taskTargetName = "击杀任意怪物";
                taskTargetName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(taskTargetName);

                break;

            case "6":
                targetText = "击杀BOSS";
                taskTargetName = "击杀任意BOSS";
                taskTargetName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(taskTargetName);
                break;

            case "7":
                targetText = "捕捉宠物";
                taskTargetName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", targetID, "Pet_Template");
                break;

            case "11":
                targetText = "寻找";
                string EquipNeedProType = targetID.Split(',')[0];
                int EquipNeedValue = int.Parse(targetID.Split(',')[1]);
                string langstr_1 = "";
                string langstr_2 = "";
                switch (EquipNeedProType)
                {
                    //攻击
                    case "1":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("攻击大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("点的装备");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "攻击大于" + EquipNeedValue + "点的装备";
                            break;
                    //物防
                    case "2":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("物防大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("点的装备");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "物防大于" + EquipNeedValue + "点的装备";
                            break;
                    //魔防
                    case "3":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("魔防大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("点的装备");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "魔防大于" + EquipNeedValue + "点的装备";
                            break;
                    //等级
                    case "4":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("点的装备");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "点的装备";
                            break;
                }

                break;

            case "12":
                targetText = "寻找";
                EquipNeedProType = targetID.Split(',')[0];
                EquipNeedValue = int.Parse(targetID.Split(',')[1]);
                switch (EquipNeedProType)
                {
                    //白色
                    case "1":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("白色品质的装备");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "白色品质的装备";
                        break;
                    //绿色
                    case "2":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("绿色品质的装备");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "绿色品质的装备";
                        break;
                    //蓝色
                    case "3":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("蓝色品质的装备");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "蓝色品质的装备";
                        break;
                    //紫色
                    case "4":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("紫色品质的装备");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "紫色品质的装备";
                        break;
                    //橙色
                    case "5":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("橙色品质的装备");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "橙色品质的装备";
                        break;
                }
                break;

            case "13":

                targetText = "寻找";
                EquipNeedProType = targetID.Split(',')[0];
                EquipNeedValue = int.Parse(targetID.Split(',')[1]);
                switch (EquipNeedProType)
                {
                    case "1":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的武器");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "的武器";
                        break;

                    case "2":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的衣服");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "的衣服";
                        break;

                    case "3":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的护符");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "的护符";
                        break;

                    case "4":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的戒指");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "的戒指";
                        break;

                    case "5":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的饰品");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "的饰品";
                        break;

                    case "6":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的鞋子");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "的鞋子";
                        break;

                    case "7":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的裤子");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "的裤子";
                        break;

                    case "8":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的腰带");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "的腰带";
                        break;

                    case "9":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的手套");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "的手套";
                        break;
 
                    case "10":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的头盔");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "的头盔";
                        break;

                    case "11":
                            langstr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("等级大于");
                            langstr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的项链");
                            taskTargetName = langstr_1 + EquipNeedValue + langstr_2;
                            //taskTargetName = "等级大于" + EquipNeedValue + "的项链";
                        break;
                }
                break;

			}

            //本地化翻译
            targetText = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(targetText);



            if (targetText != "") {
                

				taskText = targetText + taskTargetName + "(" + taskNowTargetValue + "/" + taskTargetValue + ")" + "\n";
                //这里报错 因为达到要求后给与任务的是2个参数,所以这里报错
                //Debug.Log("taskNowTargetValue = " + taskNowTargetValue + "taskTargetValue = " + taskTargetValue);
                if (int.Parse(taskNowTargetValue) >= int.Parse(taskTargetValue)) {
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("已完成");
                    taskText = targetText + taskTargetName + "(" + taskNowTargetValue + "/" + taskTargetValue + ")" + "<color=#00ff00ff>" + "（"+ langStr + "）</color>" + "\n";
                }
                
                if (targetType == "4")
                {
                    string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("级");
                    taskText = targetText + taskTargetName + taskTargetValue + langStr + "\n";
                    /*
                    int roseLv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
                    if (roseLv >= int.Parse(taskTargetValue))
                    { 
                        
                    }
                    */
                    //

                }
			}
		}
		return taskText;
	}

    //点击任务移动按钮
    public void Btn_TaskMove(){

        if (TaskID == "30100000" && Application.loadedLevelName == "EnterGame") {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_151");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.function_UI.GameGirdHint_Front("请点击右下角 [开始冒险] 进入冒险地图完成任务!");
            return;
        }

        Game_PublicClassVar.Get_function_Task.Btn_TaskMove(TaskID);

    }

	public void clearnTaskTypeShow(){
		Obj_TaskType_1.SetActive (false);
		Obj_TaskType_2.SetActive (false);
		Obj_TaskType_3.SetActive (false);
        Obj_TaskType_4.SetActive (false);

    }
}