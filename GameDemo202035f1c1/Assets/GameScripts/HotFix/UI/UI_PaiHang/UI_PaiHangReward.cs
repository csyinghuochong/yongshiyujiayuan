using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PaiHangReward : MonoBehaviour {

    public string RewardStr;
    public string RankStr;

    public GameObject Obj_RankStr;
    public GameObject Obj_RewardList;
    public GameObject Obj_RewardItemObj;

	// Use this for initialization
	void Start () {

        ShowRewardList();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ShowRewardList() {

        //清空显示
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_RewardList);

        //显示道具
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization(RankStr);
        Obj_RankStr.GetComponent<Text>().text = langStr;
        string[] rewardList = RewardStr.Split(';');
        int obj_x = 45;
        for (int i = 0;i< rewardList.Length;i++) {
            GameObject obj = (GameObject)Instantiate(Obj_RewardItemObj);
            obj.transform.SetParent(Obj_RewardList.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = new Vector3(obj_x, 0, 0);
            //设置奖励
            string itemID = rewardList[i].Split(',')[0];
            string itemNum = rewardList[i].Split(',')[1];
            obj.GetComponent<UI_ChouKaItemObj>().ItemID = itemID;
            obj.GetComponent<UI_ChouKaItemObj>().ItemNum = itemNum;
            obj_x = obj_x + 100;
            obj.GetComponent<UI_ChouKaItemObj>().Obj_ItemName.SetActive(false);
        }
    }
}
