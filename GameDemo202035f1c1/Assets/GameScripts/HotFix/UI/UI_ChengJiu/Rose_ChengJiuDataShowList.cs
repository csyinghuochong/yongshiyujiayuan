using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rose_ChengJiuDataShowList : MonoBehaviour {

    public string ChengJiuID;
    public bool IfComChengJiu;
    public GameObject UI_TaskName;
    public GameObject UI_TaskDes;
    public GameObject UI_ProValue;
    public GameObject UI_ChengJiuNum;
    public GameObject UI_ChengJiuIcon;
    public GameObject UI_TaskIfFinish;
    public GameObject UIImg_SelectStatus;

	// Use this for initialization
	void Start () {

        //roseChengJiuList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>();

	}
	
	// Update is called once per frame
	void Update () {


	}


    //显示当前成就信息
    public void UpdateChengJiuDataShow() {

        //Debug.Log("ChengJiuID = " + ChengJiuID);
        string chengJiuName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", ChengJiuID, "ChengJiu_Template");
        string chengJiuDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", ChengJiuID, "ChengJiu_Template");
        string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", ChengJiuID, "ChengJiu_Template");
        string targetValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue", "ID", ChengJiuID, "ChengJiu_Template");
        string rewardNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RewardNum", "ID", ChengJiuID, "ChengJiu_Template");
        string icon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", ChengJiuID, "ChengJiu_Template");

        //204套装类型特殊处理
        if (targetType == "204") {
            string des_Par_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des_Par_1", "ID", ChengJiuID, "ChengJiu_Template");
            string[] des_par_1_List = des_Par_1.Split(';');
            for (int i = 0; i < des_par_1_List.Length; i++) {
                if (des_par_1_List[i] != "" && des_par_1_List[i] != "0" && des_par_1_List[i] != null) {
                    string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", des_par_1_List[i], "Item_Template");
                    chengJiuDes = chengJiuDes + itemName + "、";
                }
            }

            chengJiuDes = chengJiuDes.Substring(0,chengJiuDes.Length-1);
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("穿戴身上激活成就");
            chengJiuDes = chengJiuDes + "("+ langStr_1 + ")";
        }
        
        UI_TaskName.GetComponent<Text>().text = chengJiuName;
        UI_TaskDes.GetComponent<Text>().text = chengJiuDes;
        string nowValue = "0";
        if (IfComChengJiu)
        {
            nowValue = targetValue;
        }
        else {
            nowValue = Game_PublicClassVar.Get_function_Task.ChengJiu_ReadChengJiuIDValue(ChengJiuID);
        }

        //显示完成值
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("进度");
        UI_ProValue.GetComponent<Text>().text = langStr + ":" + nowValue +"/"+ targetValue;

        //显示点数
        UI_ChengJiuNum.GetComponent<Text>().text = rewardNum;

        //显示Icon
        object obj = Resources.Load("ChengJiuIcon/" + icon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        UI_ChengJiuIcon.GetComponent<Image>().sprite = itemIcon;

        if (IfComChengJiu) {
            //显示已完成成就
            UI_TaskIfFinish.SetActive(true);
        }
        else
        {
            //显示未完成成就
            UI_TaskIfFinish.SetActive(false);
        }
    }



    public void UI_SelectTask() {
        Debug.Log("点击状态");


    }

}
