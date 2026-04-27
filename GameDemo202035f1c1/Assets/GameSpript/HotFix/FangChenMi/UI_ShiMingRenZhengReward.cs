using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShiMingRenZhengReward : MonoBehaviour
{

    public GameObject Obj_LingQuBtn;
    public GameObject Obj_LingQuImg;
    public GameObject[] ObjRewardList;

    // Start is called before the first frame update
    void Start()
    {
        //判断当前奖励是否已经领取
        string ShiMingRewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMingReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (ShiMingRewardStr == "" || ShiMingRewardStr == "0")
        {
            Obj_LingQuBtn.SetActive(true);
            Obj_LingQuImg.SetActive(false);
        }
        else
        {
            Obj_LingQuBtn.SetActive(false);
            Obj_LingQuImg.SetActive(true);
        }

        /*
        for (int i = 0; i < 3; i++) {
            ObjRewardList[i].GetComponent<UI_Common_ItemIcon_2>().ItemID = "1";
            ObjRewardList[i].GetComponent<UI_Common_ItemIcon_2>().NUM
        }
        */

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_ShiMingRenZhengReward() {

        //判断当前是否已经认证
        string name = PlayerPrefs.GetString("FangChenMi_Name");
        string shenfenID = PlayerPrefs.GetString("FangChenMi_ID");

        if (name != "" && name != null && shenfenID != "" && shenfenID != null)
        {

            //判断当前奖励是否已经领取
            string ShiMingRewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMingReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (ShiMingRewardStr == "" || ShiMingRewardStr == "0")
            {
                //判定背包是否足够
                //触发领取
                Game_PublicClassVar.Get_function_UI.GameHint("领取福利成功,奖励已发往背包,请点击查看！");
                //检测背包格子
                if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(3))
                {
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("1", 50000);          //5万金币
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000016", 1);       //藏宝图
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010091", 1);       //宠物


                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShiMingReward", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

                    Obj_LingQuBtn.SetActive(false);
                    Obj_LingQuImg.SetActive(true);

                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShiMingBtn.SetActive(false);

                }
                else
                {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请预留至少4个背包位置");
                }

            }
            else {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("奖励已领取..");
            }
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先在右侧进行实名认证后,即可领取");
        }

    }

    public void Close() {

        if (Game_PublicClassVar.Get_wwwSet.ShiMingHintStatus) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先在右侧进行实名认证.认证为成年人后即可畅玩游戏！");
            return;
        }

        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Btn_ShiMingReward();
        Destroy(this.gameObject);

    }
}
