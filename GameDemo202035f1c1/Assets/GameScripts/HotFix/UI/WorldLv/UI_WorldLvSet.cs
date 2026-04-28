using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_WorldLvSet : MonoBehaviour {

    public GameObject Obj_WorldLv;
    public GameObject Obj_GanDiLab;
    public GameObject Obj_MyLv;
    public GameObject Obj_MyExpAddPro;

    public GameObject Obj_ExpToGold_MyLv;
    public GameObject Obj_ExpToGold_MyExpPro;


    // Use this for initialization
    void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        //请求世界等级
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001302, "");
        Game_PublicClassVar.Get_gameServerObj.Obj_WorldLv = this.gameObject;
        Init();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //初始化
    public void Init() {

        //显示信息
        //Debug.Log("世界等级:" + Game_PublicClassVar.Get_wwwSet.WorldLv);
        Obj_WorldLv.GetComponent<Text>().text = Game_PublicClassVar.Get_wwwSet.WorldLv.ToString();
        string langStr = "";
        if (Game_PublicClassVar.Get_wwwSet.WorldPlayerName != "")
        {
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("肝帝");
            Obj_GanDiLab.GetComponent<Text>().text = langStr + ":" + Game_PublicClassVar.Get_wwwSet.WorldPlayerName.ToString() + "(" + Game_PublicClassVar.Get_wwwSet.WorldPlayerLv + Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("级") + ")";
        }
        else {
            langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第一个升级至20级的为肝帝");
            Obj_GanDiLab.GetComponent<Text>().text = langStr + "!";
        }

        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("可以获得经验加成");
        Obj_MyExpAddPro.GetComponent<Text>().text = langStr + Game_PublicClassVar.Get_wwwSet.WorldExpProAdd*100 + "%";

        //角色等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("当前的等级");
        Obj_MyLv.GetComponent<Text>().text = langStr + ":" + roseLv;
        Obj_ExpToGold_MyLv.GetComponent<Text>().text = langStr + ":" + roseLv + Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("级");
        
        float roseExpNow = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        float Rose_Exp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseUpExp", "RoseLv", roseLv.ToString(), "RoseExp_Template"));
        //Debug.Log("roseExpNow = " + roseExpNow + " Rose_Exp = " + Rose_Exp);
        float expPro = roseExpNow / Rose_Exp;
        langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("当前的经验百分比");
        Obj_ExpToGold_MyExpPro.GetComponent<Text>().text = langStr + ":" +(expPro * 100).ToString("0.00") + "%";

    }

    //点击兑换
    public void Btn_DuiHuanGold() {

        Debug.Log("我点击了兑换...");
        if (Game_PublicClassVar.Get_wwwSet.WorldLv < 10)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_106");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未连接服务器！");
            return;
        }

        //当前角色等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

        if (roseLv >= 50)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_107");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("世界等级已经完全放开,无法兑换!");
            return;
        }


        if (roseLv >= Game_PublicClassVar.Get_wwwSet.WorldLv)
        {
            //获取当前经验比例
            float roseExpNow = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            float Rose_Exp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseUpExp", "RoseLv", roseLv.ToString(), "RoseExp_Template"));

            //扣除20%经验
            int costExp = (int)(Rose_Exp * 0.2f);
            if ((int)(roseExpNow) >= costExp)
            {

                //发送金币奖励
                float duihuanGold = (float)(Game_PublicClassVar.Get_wwwSet.WorldLv) / 2.0f - 5.0f;
                if (duihuanGold <= 0)
                {
                    duihuanGold = 0;
                }
                if (duihuanGold >= 30)
                {
                    duihuanGold = 30;
                }

                int duihuanGold_Int = (int)(duihuanGold * 10000);

                //弹出通用提示框
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);

                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_19");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_20");
                string jieshaoStr = langStrHint_1 + duihuanGold_Int + langStrHint_2;
                //string jieshaoStr = "是否消耗20%经验兑换:" + duihuanGold_Int + "金币";
                string langStrHint_4 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_7");
                string langStrHint_5 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_8");
                string langStrHint_6 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_9");
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, DuiHuanGold, null, langStrHint_4, langStrHint_5, langStrHint_6);
                //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, DuiHuanGold, null, "兑换确认", "兑换", "取消");
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_108");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("经验不足,无法兑换！");
                return;
            }
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_109");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未达到世界等级！");
            return;
        }

    }


    private void DuiHuanGold() {

        //当前角色等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv >= 50)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_110");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("世界等级已经完全放开,无法兑换!");
            return;
        }

        if (roseLv >= Game_PublicClassVar.Get_wwwSet.WorldLv)
        {
            //获取当前经验比例
            float roseExpNow = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            float Rose_Exp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseUpExp", "RoseLv", roseLv.ToString(), "RoseExp_Template"));

            //扣除20%经验
            int costExp = (int)(Rose_Exp * 0.2f);
            if ((int)(roseExpNow) >= costExp)
            {
                //扣除经验
                int writeExp = (int)(roseExpNow) - costExp;
                if (writeExp <= 0)
                {
                    writeExp = 0;
                }
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseExpNow", writeExp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                //发送金币奖励
                float duihuanGold = (float)(Game_PublicClassVar.Get_wwwSet.WorldLv) / 2.0f - 5.0f;
                if (duihuanGold <= 0)
                {
                    duihuanGold = 0;
                }
                if (duihuanGold >= 30)
                {
                    duihuanGold = 30;
                }

                int duihuanGold_Int = (int)(duihuanGold * 10000);
                Game_PublicClassVar.Get_function_Rose.SendReward("1", duihuanGold_Int.ToString(),"46");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

                //更新显示
                Rose_Proprety rose_Proprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
                rose_Proprety.Rose_GetExp = true;

                //更新主界面显示
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseExp.GetComponent<UI_MainUIRoseExp>().UpdataRoseExp = true;

                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_116");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_117");
                string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_118");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + costExp + langStrHint_2 + duihuanGold_Int + langStrHint_3);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你消耗了" + costExp + "点经验兑换了" + duihuanGold_Int + "金币");

                //刷新界面
                Init();
            }
            else
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_119");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("经验不足,无法兑换！");
                return;
            }
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_120");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未达到世界等级！");
            return;
        }
    }

    //关闭界面
    public void CloseUI() {
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenWorldLv();
    }

}
