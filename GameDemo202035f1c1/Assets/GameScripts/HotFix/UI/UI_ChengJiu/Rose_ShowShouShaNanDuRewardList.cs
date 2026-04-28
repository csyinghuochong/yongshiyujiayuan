using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rose_ShowShouShaNanDuRewardList : MonoBehaviour {

    public string NanDu;                //难度  1普通 2挑战 3困难
    //public string MonsterID;
    public string RewardStr;            //奖励列表
    public GameObject RewardShowObj;
    public string KillZhangHaoID;       //击杀账号ID
    public GameObject ShowRewardSet;
    public GameObject Obj_ShowNanDuName;


	// Use this for initialization
	void Start () {
        ShowReward();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //展示奖励
    public void ShowReward() {

        //string monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", MonsterID, "Monster_Template");

        string nanduStr = "";
        switch (NanDu) {
            case "1":
                nanduStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("普通难度首胜奖励");
                break;
            case "2":
                nanduStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("挑战难度首胜奖励");
                break;
            case "3":
                nanduStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("地狱难度首胜奖励");
                break;
        }
        Obj_ShowNanDuName.GetComponent<Text>().text = nanduStr;

        //清除
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(ShowRewardSet);

        //成就奖励
        if (RewardStr != "")
        {
            string[] rewardItemList = RewardStr.Split(';');
            Debug.Log("rewardItemList的长度 = " + rewardItemList);
            for (int i = 0; i < rewardItemList.Length; i++)
            {
                //显示奖励
                string[] itemList = rewardItemList[i].Split(',');
                GameObject itemObj = (GameObject)Instantiate(RewardShowObj);
                itemObj.transform.SetParent(ShowRewardSet.transform);
                itemObj.transform.localScale = new Vector3(1, 1, 1);
                itemObj.GetComponent<UI_Common_ItemIcon>().ItemID = itemList[0];
                itemObj.GetComponent<UI_Common_ItemIcon>().NeedItemNum = int.Parse(itemList[1]);
                itemObj.GetComponent<UI_Common_ItemIcon>().UpdateItem();
            }

            //设置列表长度
            if (rewardItemList.Length > 10)
            {
                int num = rewardItemList.Length - 10;
                float shousha_x = 1000 + num * 100;
                ShowRewardSet.GetComponent<RectTransform>().sizeDelta = new Vector2(shousha_x, 80);
                ShowRewardSet.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(shousha_x / 2, ShowRewardSet.GetComponent<RectTransform>().anchoredPosition3D.y, ShowRewardSet.GetComponent<RectTransform>().anchoredPosition3D.z);
            }
        }
    }

    //领取奖励
    public void Btn_LingQuReward() {

        //判断是否是自己

    }
}
