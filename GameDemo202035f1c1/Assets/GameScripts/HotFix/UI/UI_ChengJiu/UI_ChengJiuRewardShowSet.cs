using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_ChengJiuRewardShowSet : MonoBehaviour {

    public ObscuredString ChengJiuRewardID;
    public GameObject Obj_ChengJiuIcon;
    public GameObject Obj_ChengJiuNum;
    public GameObject Obj_ChengJiuRewardName;
    public GameObject Obj_ChengJiuRewardDes;
    public GameObject Obj_ChengJiuRewardItemSet;
    public GameObject Obj_ChengJiuRewardItem;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowChengJiuRewardShow()
    {
        //读取信息
        string name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", ChengJiuRewardID, "ChengJiuReward_Template");
        string icon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", ChengJiuRewardID, "ChengJiuReward_Template");
        string chengJiuNeedNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiuNeedNum", "ID", ChengJiuRewardID, "ChengJiuReward_Template");
        string reward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Reward", "ID", ChengJiuRewardID, "ChengJiuReward_Template");
        string Des = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", ChengJiuRewardID, "ChengJiuReward_Template");

        //显示对应信息
        Obj_ChengJiuRewardName.GetComponent<Text>().text = name;
        Obj_ChengJiuRewardDes.GetComponent<Text>().text = Des;

        //显示Icon
        object obj = Resources.Load("ChengJiuIcon/" + icon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_ChengJiuIcon.GetComponent<Image>().sprite = itemIcon;

        //清除
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ChengJiuRewardItemSet);

        //成就奖励
        if (reward != "") {
            string[] rewardItemList = reward.Split(';');
            for (int i = 0; i < rewardItemList.Length; i++) {

                //显示奖励
                string[] itemList = rewardItemList[i].Split(',');
                GameObject itemObj = (GameObject)Instantiate(Obj_ChengJiuRewardItem);
                itemObj.transform.SetParent(Obj_ChengJiuRewardItemSet.transform);
                itemObj.transform.localScale = new Vector3(1, 1, 1);
                itemObj.GetComponent<UI_Common_ItemIcon>().ItemID = itemList[0];
                itemObj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = int.Parse(itemList[1]);
                itemObj.GetComponent<UI_Common_ItemIcon>().UpdateItem();
            }
        }
    }


    //领取奖励
    public void Btn_LingQuReward() {



    }
}
