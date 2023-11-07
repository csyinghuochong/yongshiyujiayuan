using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ZhenYingRoseList : MonoBehaviour {

    public Pro_ZhenYingRoseData proZhenYingRoseData;
    public Sprite[] Obj_ZhenYingRankImgList;
    public GameObject Obj_ZhenYingRankImg;
    public GameObject Obj_ZhenYingRankText;
    public GameObject Obj_ZhenYingRoseHeadIcon;
    public GameObject Obj_ZhenYingRoseName;
    public GameObject Obj_ZhenYingRoseShiLiValue;
    public GameObject Obj_ZhenYingGuanZhiShow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //更新数据
    public void UpdateData() {

        //排名
        if (proZhenYingRoseData.RankNum <= 3)
        {
            Obj_ZhenYingRankImg.GetComponent<Image>().sprite = Obj_ZhenYingRankImgList[proZhenYingRoseData.RankNum - 1];
            Obj_ZhenYingRankImg.SetActive(true);
            Obj_ZhenYingRankText.SetActive(false);
        }
        else {

            Obj_ZhenYingRankText.GetComponent<Text>().text = proZhenYingRoseData.RankNum.ToString();
            Obj_ZhenYingRankImg.SetActive(false);
            Obj_ZhenYingRankText.SetActive(true);
        }

        //显示其他信息
        Obj_ZhenYingRoseName.GetComponent<Text>().text = proZhenYingRoseData.RoseName;
        Obj_ZhenYingRoseShiLiValue.GetComponent<Text>().text = proZhenYingRoseData.RoseShiLiValue.ToString();


        /*
        string roseHeadIconId = "10001";
        string roseOcc = proZhenYingRoseData.RoseOcc;

        switch (roseOcc)
        {
            case "1":
                roseHeadIconId = "10001";
                break;

            case "2":
                roseHeadIconId = "10002";
                break;

            case "3":
                roseHeadIconId = "10003";
                break;
        }

        Object obj = Resources.Load("HeadIcon/PlayerIcon/" + roseHeadIconId, typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_ZhenYingRoseHeadIcon.GetComponent<Image>().sprite = img;
        Obj_ZhenYingRoseHeadIcon.GetComponent<Image>().SetNativeSize();
        */

        //显示头像
        Game_PublicClassVar.Get_function_UI.ShowPlayerHeadIcon(proZhenYingRoseData.RoseOcc,Obj_ZhenYingRoseHeadIcon);
        //Debug.Log("(proZhenYingRoseData.RoseZhenYingGuanZhi = " + proZhenYingRoseData.RoseZhenYingGuanZhi);
        string nowZhiWei = Game_PublicClassVar.Get_function_UI.ReturnZhiWeiName(proZhenYingRoseData.RoseZhenYingGuanZhi);
        if (nowZhiWei != "" && nowZhiWei != "0" && nowZhiWei != null)
        {
            Obj_ZhenYingGuanZhiShow.GetComponent<Text>().text = "职位:" + nowZhiWei;
        }
        else {
            Obj_ZhenYingGuanZhiShow.GetComponent<Text>().text = "职位:" + "门众";
        }
    }
}
