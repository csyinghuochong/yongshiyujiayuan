using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ZhenYingXuanZe : MonoBehaviour {

    //
    public GameObject Obj_ZhenYingShowText;
    public GameObject Obj_ZhenYingZhiWeiShowText;
    // Use this for initialization
    void Start () {
        Init();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    //初始化
    private void Init() {

        string zhenying = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhenYing", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        if (zhenying == "" || zhenying == "0" || zhenying == null)
        {
            Obj_ZhenYingShowText.GetComponent<Text>().text = "未选择阵营";
        }

        if (zhenying == "1")
        {
            Obj_ZhenYingShowText.GetComponent<Text>().text = "正派";
        }

        if (zhenying == "2")
        {
            Obj_ZhenYingShowText.GetComponent<Text>().text = "邪派";
        }

        string zhenyingName = Game_PublicClassVar.Get_function_UI.ReturnZhiWeiName(Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().SelfGuanZhi);

        if (zhenyingName != "" && zhenyingName != "0" && zhenyingName != null) {
            Obj_ZhenYingZhiWeiShowText.GetComponent<Text>().text = zhenyingName;
        }

        //刷新主角界面显示
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp.GetComponent<UI_RoseHp>().UpdateZhenYing();
    }



    public void ZhenYingXuan_Zheng() {

        //ZhenYingXuanZe("1");

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = "是否选择加入正派？\n提示:只有1次免费机会,后续更改需要花费100万金币!";
        string zhenying = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhenYing", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (zhenying == "" || zhenying == "0" || zhenying == null)
        {

        }
        else
        {
            if (zhenying == "1")
            {

                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("相同阵营,无需改变!");
                return;
            }

            langStrHint = "是否改变阵营加入正派？\n提示:更改需要花费100万金币手续费!";
        }

        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, Btn_ZhenYingXuan_Zheng, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    public void ZhenYingXuan_Xie()
    {
        //ZhenYingXuanZe("2");

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = "是否选择加入邪派？\n提示:只有1次免费机会,后续更改需要花费100万金币";
        string zhenying = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhenYing", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (zhenying == "" || zhenying == "0" || zhenying == null)
        {

        }
        else
        {

            if (zhenying == "2") {

                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("相同阵营,无需改变!");
                return;
            }

            langStrHint = "是否改变阵营加入邪派？\n提示:更改需要花费100万金币手续费!";
        }
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, Btn_ZhenYingXuan_Xie, null);
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否进入副本？\n提示:每天只有一次进入机会！", Enter_DaMiJing, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    public void Btn_ZhenYingXuan_Zheng()
    {

        ZhenYingXuanZe("1");


    }

    public void Btn_ZhenYingXuan_Xie()
    {
        ZhenYingXuanZe("2");
    }


    private void ZhenYingXuanZe(string zhenyingStr) {

        string zhenying = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhenYing", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (zhenying == "" || zhenying == "0" || zhenying == null)
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhenYing", zhenyingStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002302, zhenyingStr);
        }
        else {

            //无法转入强势一方
            if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet != null)
            {

                Pro_ZhenYingDataList nowPro_ZhenYingDataList = Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().ProZhenYingDataList;

                if (zhenyingStr == "1") {
                    if (nowPro_ZhenYingDataList.ZhenYingNum_Zheng >= nowPro_ZhenYingDataList.ZhenYingNum_Xie) {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("无法转移胜利的一方阵营");
                        return;
                    }
                }

                if (zhenyingStr == "2")
                {
                    if (nowPro_ZhenYingDataList.ZhenYingNum_Zheng < nowPro_ZhenYingDataList.ZhenYingNum_Xie)
                    {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("无法转移胜利的一方阵营");
                        return;
                    }
                }

            }
            else {
                return;
            }


            //如果当前有阵营,调整阵营需要消费100万金币
            if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() < 1000000)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("金币不足");
                return;
            }

            int ItmePrice = 1000000;
            bool ifbuyStatus = Game_PublicClassVar.Get_function_Rose.CostReward("1", ItmePrice.ToString());
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhenYing", zhenyingStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002302, zhenyingStr);

            if (zhenyingStr == "1") {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("正派盟主欢迎您的加入!");
            }

            if (zhenyingStr == "2") {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("邪派盟主欢迎您的加入!");
            }
            
        }

        //刷新显示
        Init();
    }
}
