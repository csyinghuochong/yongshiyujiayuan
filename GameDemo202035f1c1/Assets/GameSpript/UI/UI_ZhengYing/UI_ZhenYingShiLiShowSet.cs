using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ZhenYingShiLiShowSet : MonoBehaviour {

    public GameObject Obj_ZhengYingRoseList;
    public GameObject Obj_ZhenYingShow_Zheng;
    public GameObject Obj_ZhenYingShow_Xie;

    public GameObject Obj_ZhenYingShiLi_Zheng;
    public GameObject Obj_ZhenYingShiLi_Xie;

    public Pro_ZhenYingDataList ProZhenYingDataList;

    // Use this for initialization
    void Start () {

        UpdateData();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //更新数据
    public void UpdateData() {

        //测试数据
        /*
        Pro_ZhenYingRoseData proZhenYingRoseData = new Pro_ZhenYingRoseData();
        proZhenYingRoseData.RankNum = 1;
        proZhenYingRoseData.RoseName = "幸福的瞬间";
        proZhenYingRoseData.RoseOcc = "3";
        proZhenYingRoseData.RoseShiLiValue = 111111;
        */

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ZhenYingShow_Zheng);
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ZhenYingShow_Xie);

        if (ProZhenYingDataList == null)
        {
            return;
        }

        //添加正方阵营
        //int shiliSum_zheng = 0;
        for (int i = 1; i <= 10; i++)
        {

            if (ProZhenYingDataList.ProZhenYingRoseData_Zheng.Count >= 1)
            {
                if (ProZhenYingDataList.ProZhenYingRoseData_Zheng.ContainsKey(i))
                {
                    GameObject obj = (GameObject)Instantiate(Obj_ZhengYingRoseList);
                    obj.transform.SetParent(Obj_ZhenYingShow_Zheng.transform);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.GetComponent<UI_ZhenYingRoseList>().proZhenYingRoseData = ProZhenYingDataList.ProZhenYingRoseData_Zheng[i];
                    obj.GetComponent<UI_ZhenYingRoseList>().UpdateData();
                    //shiliSum_zheng = shiliSum_zheng + proZhenYingRoseData.RoseShiLiValue;
                }
            }
            
        }

        Obj_ZhenYingShiLi_Zheng.GetComponent<Text>().text = ProZhenYingDataList.ZhenYingNum_Zheng.ToString();

        //添加邪方正营
        //int shiliSum_xie = 0;
        for (int i = 1; i <= 10; i++)
        {
            if (ProZhenYingDataList.ProZhenYingRoseData_Xie.Count >= 1)
            {
                if (ProZhenYingDataList.ProZhenYingRoseData_Xie.ContainsKey(i))
                {
                    GameObject xieObj = (GameObject)Instantiate(Obj_ZhengYingRoseList);
                    xieObj.transform.SetParent(Obj_ZhenYingShow_Xie.transform);
                    xieObj.transform.localScale = new Vector3(1, 1, 1);
                    xieObj.GetComponent<UI_ZhenYingRoseList>().proZhenYingRoseData = ProZhenYingDataList.ProZhenYingRoseData_Xie[i];
                    xieObj.GetComponent<UI_ZhenYingRoseList>().UpdateData();
                    //shiliSum_xie = shiliSum_xie + proZhenYingRoseData.RoseShiLiValue;
                }
            }
        }

        Obj_ZhenYingShiLi_Xie.GetComponent<Text>().text = ProZhenYingDataList.ZhenYingNum_Xie.ToString();

    }



}
