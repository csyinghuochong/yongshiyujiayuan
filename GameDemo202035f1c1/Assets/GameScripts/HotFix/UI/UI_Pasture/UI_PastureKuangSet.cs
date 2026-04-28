using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureKuangSet : MonoBehaviour
{

    public Pro_RankPetListData proRankListData;
    public GameObject Obj_Hint;

    public GameObject Obj_PetKuangListShow;
    public GameObject Obj_PetKuangSet;
    public GameObject Obj_TiaoZhanNumStr;

    // Start is called before the first frame update
    void Start()
    {
        Game_PublicClassVar.Get_gameServerObj.Obj_KuangSet = this.gameObject;

        //发送服务器请求
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002012, "");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Init() {

        Obj_Hint.SetActive(false);

        //获取当前挑战次数
        string petTiaoZhanNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("KuangLvDuoNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (petTiaoZhanNum == "")
        {
            petTiaoZhanNum = "0";
        }

        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("今日掠夺次数");
        Obj_TiaoZhanNumStr.GetComponent<Text>().text = langStr + ":" + petTiaoZhanNum + "/" + "5";

        //初始化挑战列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_PetKuangSet);
        //降序排序
        for (int i = 1; i <= 3; i++)
        {

            if (proRankListData.PetRankData.ContainsKey(i.ToString()))
            {

                GameObject obj = (GameObject)Instantiate(Obj_PetKuangListShow);
                obj.transform.SetParent(Obj_PetKuangSet.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.GetComponent<UI_PastureKuangListSet>().ProPetListData = proRankListData.PetRankData[i.ToString()];
                obj.GetComponent<UI_PastureKuangListSet>().Init();
                
            }
        }

        /*
        //获取自身战队名称（暂时以玩家名字命名）
        TeamName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanTeamName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (TeamName == "")
        {
            TeamName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData") + Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("的队伍"); ;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetTiaoZhanTeamName", TeamName, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
        }
        */

    }

    //重置相关数据
    public void Btn_ChongZhi() {

        string petTiaoZhanNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("KuangLvDuoChongZhiNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (petTiaoZhanNum == "")
        {
            petTiaoZhanNum = "0";
        }

        int kuangchongzhiNum = int.Parse(petTiaoZhanNum);
        if (kuangchongzhiNum >= 1) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日重置次数已用完,每日最可以重置1次!");
            return;
        }

        kuangchongzhiNum = kuangchongzhiNum + 1;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("KuangLvDuoChongZhiNum", kuangchongzhiNum.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        ChongZhi();

        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("重置成功,每日最多重置3次!");
    }


    public void ChongZhi() {

        //发送强制申请
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002012, "1");

    }
}
