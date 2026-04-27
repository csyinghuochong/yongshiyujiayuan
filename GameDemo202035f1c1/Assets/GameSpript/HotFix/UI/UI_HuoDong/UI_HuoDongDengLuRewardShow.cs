using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HuoDongDengLuRewardShow : MonoBehaviour {

    public string ActiveID;
    public GameObject Obj_CommonItemShow;
    public int RandomValue;     //随机概率
    public string IfLingQuStatusStr; //是否被领取(0,表示未领取 1,表示已经领取)
    public bool UpdateShowStatus;
    public bool UpdateXuanZhongStatus;
    public GameObject Obj_XuanZhongImg;
    public GameObject Obj_YiJingQuImg;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (UpdateShowStatus) {
            UpdateShowStatus = false;
            ShowReward();
        }

        //更新选中框
        if (UpdateXuanZhongStatus)
        {
            Obj_XuanZhongImg.SetActive(true);
        }
        else {
            Obj_XuanZhongImg.SetActive(false);
        }

	}

    //显示奖励
    public void ShowReward() {

        string itemIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Par_2", "ID", ActiveID, "Activity_Template");
        Debug.Log("更新显示 ActiveID = " + ActiveID + "itemIDStr = " + itemIDStr);
        string itemID = itemIDStr.Split(',')[0];
        string itemNum = itemIDStr.Split(',')[1];

        Obj_CommonItemShow.GetComponent<UI_Common_ItemIcon>().ItemID = itemID;
        Obj_CommonItemShow.GetComponent<UI_Common_ItemIcon>().NeedItemNum = int.Parse(itemNum);
        Obj_CommonItemShow.GetComponent<UI_Common_ItemIcon>().UpdateStatus = true;

        if (IfLingQuStatusStr == "0")
        {
            Obj_YiJingQuImg.SetActive(false);
        }
        else {
            Obj_YiJingQuImg.SetActive(true);
            //置灰
            object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
            Material huiMaterial = huiObj as Material;
            Obj_CommonItemShow.GetComponent<UI_Common_ItemIcon>().Obj_NeedItemIcon.GetComponent<Image>().material = huiMaterial;
            //Obj_CommonItemShow.GetComponent<UI_Common_ItemIcon>().Obj_NeedItemIcon.GetComponent<Image>().material = huiMaterial;
            Obj_CommonItemShow.GetComponent<UI_Common_ItemIcon>().Obj_NeedItemName.GetComponent<Text>().color = new Color(0.55f, 0.55f, 0.55f, 1);
        }
    }

}
