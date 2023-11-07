using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChengJiuHintObj : MonoBehaviour {
    public string ChengJiuID;
    public GameObject Obj_ChengJiuName;
    public GameObject Obj_ChengJiuDes;
    public GameObject Obj_ChengJiuNum;
    public GameObject Obj_ChengJiuIcon;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //展示成就
    public void ShowChengJiu() {

        string chengJiuName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", ChengJiuID, "ChengJiu_Template");
        string chengJiuDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", ChengJiuID, "ChengJiu_Template");
        string targetValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue", "ID", ChengJiuID, "ChengJiu_Template");
        string rewardNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RewardNum", "ID", ChengJiuID, "ChengJiu_Template");
        string icon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", ChengJiuID, "ChengJiu_Template");

        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("成就");
        Obj_ChengJiuName.GetComponent<Text>().text = langStr + ":" + chengJiuName;
        Obj_ChengJiuDes.GetComponent<Text>().text = chengJiuDes;

        //显示点数
        Obj_ChengJiuNum.GetComponent<Text>().text = rewardNum;

        //显示Icon
        object obj = Resources.Load("ChengJiuIcon/" + icon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ChengJiuIcon.GetComponent<Image>().sprite = itemIcon;


        //展示5秒
        Destroy(this.gameObject,5);
    }

}
