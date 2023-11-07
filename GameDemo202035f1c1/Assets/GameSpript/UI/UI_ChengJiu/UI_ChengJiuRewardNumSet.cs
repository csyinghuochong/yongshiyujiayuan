using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_ChengJiuRewardNumSet : MonoBehaviour {

    public ObscuredString ChengJiuRewardID;
    public GameObject Obj_ChengJiuIcon;
    public GameObject Obj_ChengJiuNum;
    public GameObject Obj_XuanZhongImg;
    private Rose_ChengJiuSet roseChengJiuList;

    // Use this for initialization
    void Start () {
       roseChengJiuList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>();

    }

    // Update is called once per frame
    void Update () {

        if (roseChengJiuList.UpdateXuanZhongRewardStatus) {
            
            if (roseChengJiuList.Obj_XuanZhongRewardNum == this.gameObject)
            {
                Obj_XuanZhongImg.SetActive(true);
            }
            else {
                Obj_XuanZhongImg.SetActive(false);
            }
        }
	}

    public void ShowChengJiuRewardNum() {

        //读取信息
        string name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", ChengJiuRewardID, "ChengJiuReward_Template");
        string icon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", ChengJiuRewardID, "ChengJiuReward_Template");
        string chengJiuNeedNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiuNeedNum", "ID", ChengJiuRewardID, "ChengJiuReward_Template");
        string reward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Reward", "ID", ChengJiuRewardID, "ChengJiuReward_Template");
        string Des = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", ChengJiuRewardID, "ChengJiuReward_Template");

        //显示对应信息
        Obj_ChengJiuNum.GetComponent<Text>().text = chengJiuNeedNum;
        //显示Icon
        object obj = Resources.Load("ChengJiuIcon/" + icon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ChengJiuIcon.GetComponent<Image>().sprite = itemIcon;

    }

    //选中奖励
    public void SelectReward() {

        Rose_ChengJiuSet roseChengJiuList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>();
        roseChengJiuList.Obj_ChengJiuRewardShowSet.GetComponent<UI_ChengJiuRewardShowSet>().ChengJiuRewardID = ChengJiuRewardID;
        roseChengJiuList.Obj_ChengJiuRewardShowSet.GetComponent<UI_ChengJiuRewardShowSet>().ShowChengJiuRewardShow();
        roseChengJiuList.Obj_XuanZhongRewardNum = this.gameObject;
        roseChengJiuList.UpdateXuanZhongRewardStatus = true;

    }
}
