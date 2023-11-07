using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose_ShowShouShaReward : MonoBehaviour {

    public GameObject ShouShaRewardObj;
    public GameObject ShouShaRewardObjSet;
    public string ShouShaRewardStr;
    // Use this for initialization
    void Start () {
        //ShowReward();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowReward() {

        if (ShouShaRewardStr == "") {
            return;
        }

        string[] ShouShaRewardList = ShouShaRewardStr.Split('|');
        if (ShouShaRewardList.Length < 3) {
            return;
        }

        //清除
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(ShouShaRewardObjSet);

        //实例化3个难度
        for (int i = 1;i<=3;i++) {
            GameObject rewardObj = (GameObject)Instantiate(ShouShaRewardObj);
            rewardObj.transform.SetParent(ShouShaRewardObjSet.transform);
            rewardObj.transform.localScale = new Vector3(1, 1, 1);
            rewardObj.GetComponent<Rose_ShowShouShaNanDuRewardList>().NanDu = i.ToString();
            rewardObj.GetComponent<Rose_ShowShouShaNanDuRewardList>().RewardStr = ShouShaRewardList[i-1];
        }
    }

    public void CloseUI() {
        Destroy(this.gameObject);
    }
}
