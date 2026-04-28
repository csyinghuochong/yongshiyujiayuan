using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_FortressBuildReward : MonoBehaviour {

    public bool ActResult;                  //战斗结果,1表示赢， 2表示输
    public string ResultTypeTextStr;        //结果资源类型显示
    public string ResultValueTextStr;       //结果资源值显示

    public string SelfDeathNum;             //己方死亡人数
    public string EnemyDeathNum;            //敌人死亡人数
    public GameObject Obj_UIRewardGird;
    public GameObject Obj_UIRewardGirdParent;       //RewardGird的父级
    public GameObject Obj_UIActResult;              //战斗文本显示
    public GameObject Obj_ImgWin;                   //胜利
    public GameObject Obj_ImgFail;                  //失败
    public GameObject Obj_EmenyActSet;


	// Use this for initialization
	void Start () {

        if (ResultTypeTextStr != "") {
            string[] resultTypeText = ResultTypeTextStr.Split(',');
            string[] resultValueText = ResultValueTextStr.Split(',');
            float startPosition = (resultTypeText.Length - 1) * -60.0f;

            //循环实例化奖励
            for (int i = 0; i <= resultTypeText.Length-1; i++) {

                GameObject rewardGirdObj = (GameObject)Instantiate(Obj_UIRewardGird);
                rewardGirdObj.transform.SetParent(Obj_UIRewardGirdParent.transform);
                rewardGirdObj.transform.localScale = new Vector3(1, 1, 1);
                rewardGirdObj.transform.localPosition = new Vector3(startPosition + i * 120.0f, 0, 0);
                Debug.Log("resultTypeText[i] = " + resultTypeText[i]);
                rewardGirdObj.GetComponent<UI_RewardGird>().RewardType = resultTypeText[i];
                rewardGirdObj.GetComponent<UI_RewardGird>().RewardTypeValue = resultValueText[i];

            }
        }

        if (ActResult)
        {
            Obj_UIActResult.GetComponent<Text>().text = "要塞防御成功！,敌方死亡" + EnemyDeathNum + "人,己方死亡" + SelfDeathNum + "这是你获得的战利品.";
            Obj_ImgWin.SetActive(true);
            Obj_ImgFail.SetActive(false);
        }
        else {
            Obj_UIActResult.GetComponent<Text>().text = "要塞防御失败！,敌方死亡" + EnemyDeathNum + "人,己方死亡" + SelfDeathNum + "这是你损失的要塞资源.";
            Obj_ImgWin.SetActive(false);
            Obj_ImgFail.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CloseUI() {
        //Destroy(Obj_EmenyActSet);
        Destroy(this.gameObject);
    }
}
