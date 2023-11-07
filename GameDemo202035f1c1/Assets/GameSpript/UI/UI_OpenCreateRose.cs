using UnityEngine;
using System.Collections;
using System;

public class UI_OpenCreateRose : MonoBehaviour {

    public GameObject Obj_CreateRose;
    public GameObject Obj_EnterGameSet;
    public GameObject Obj_GanXieBtn;

    EventHandle mEventHandle;

    // Use this for initialization
    void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_EnterGameSet);

        mEventHandle = GameObject.Find("WWW_Set").GetComponent<EventHandle>();
        mEventHandle.onLoginSuccessAction = onLoginSuccess;
        mEventHandle.onLoginFailAction = onLoginFail;
        mEventHandle.onInitSuccessAction = OnInitSuccess;

        //PlayerPrefs.SetString("FangChenMi_Name", null);
        //PlayerPrefs.SetString("FangChenMi_ID",null);

    }
	
	// Update is called once per frame
	void Update () {

        //判定是否为返回登录界面
        if (Game_PublicClassVar.ReturnDengLuStatus)
        {
            Game_PublicClassVar.ReturnDengLuStatus = false;
            //点击返回按钮直接打开登选角界面
            Obj_CreateRose.SetActive(true);
        }

    }

    public void Open_CreateRose() 
    {

        //判定当前是否连接网络
        if (!Game_PublicClassVar.gameLinkServer.ServerLinkStatus)
        {
            //Debug.Log("检测到未连接网络!无法进入");
            //Game_PublicClassVar.Get_wwwSet.Show_GameHint("检测到未连接网络!无法进入\n (本次篝火测试需要联网才可以进入游戏)因为是第一次测试,所以需要玩家进行联网进行测试资格验证！\n(游戏正式上线后单机也可以进入游戏!)");
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_104");
            Game_PublicClassVar.Get_wwwSet.Show_GameHint(langStr);
            return;
        }

        //
        if (!Game_PublicClassVar.gameLinkServer.IfEnterGameStatus)
        {

            if (Game_PublicClassVar.Get_wwwSet.IfGooglePay)
            {
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_104");
                Game_PublicClassVar.Get_wwwSet.Show_GameHint(langStr);
            }
            else
            {
                if (EventHandle.IsHuiWeiChannel())
                {
                    Game_PublicClassVar.Get_wwwSet.Show_GameHint("服务器链接中,请稍后再次点击进入游戏!");
                }
                else {
                    Game_PublicClassVar.Get_wwwSet.Show_GameHint("服务器链接失败,当前可能正在维护!!! 玩家QQ群:" + Game_PublicClassVar.Get_wwwSet.QQqunID + " 获取最新资讯!");
                }
                
            }

            //Game_PublicClassVar.Get_wwwSet.Show_GameHint("测试已经结束,感谢你的支持!您的所有数据都已保存在服务器内,等待下次开放可以直接使用本次测试的游戏角色继续游戏,相信不久就会迎来新的测试!!!玩家QQ群:"+Game_PublicClassVar.Get_wwwSet.QQqunID);
            return;
        }

        //判断是否实名制
        if (EventHandle.IsQudaoPackage())
        {
            EventHandle.reInit();
        }
        else
        {
            if (Game_PublicClassVar.Get_gameLinkServerObj.QiangZhiShiMingStatus)
            {
                string shenfenIDStr = PlayerPrefs.GetString("FangChenMi_ID");
                if (shenfenIDStr.Length <= 5)
                {
                    Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc.GetComponent<UI_StartGameFunc>().Btn_OpenRenZheng();
                    Game_PublicClassVar.Get_wwwSet.Show_GameHint("请先进行实名认证,未实名和未成年用户禁止登陆游戏!");
                    return;
                }

                if (PlayerPrefs.GetInt("FangChenMi_Year") <= 17)
                {
                    //判定时间
                    bool hintShiMing = true;
                    string dt = DateTime.Today.DayOfWeek.ToString();
                    //Debug.Log("时间:" + dt);

                    if (dt.Contains("Friday") || dt.Contains("Saturday") || dt.Contains("Sunday"))
                    {
                        if (DateTime.Now.Hour == 21)
                        {
                            hintShiMing = false;
                        }
                    }

                    if (hintShiMing)
                    {
                        Game_PublicClassVar.Get_wwwSet.Show_GameHint("根据国家相关规定,未成年用户游戏只能在周五 六 日的21:00-22:00进行游戏!");
                        return;
                    }
                }
            }

            onLoginSuccess();
        }

        /*
        if (Game_PublicClassVar.Get_wwwSet.CreateRoseDataNum < 2)
        {
            Debug.Log("角色GameConfig未生成完毕,请稍后再次点击");
        }
        else {
            if (Obj_CreateRose != null)
            {
                Obj_CreateRose.SetActive(true);
            }
        }
        */
    }

    private void OnInitSuccess()
    { 
        mEventHandle.onLogin();
    }

    private void onLoginSuccess()
    {
        //判断是否实名制
        if (EventHandle.IsHuiWeiChannel())
        {
            //华为渠道包由平台检测

            //华为获取设备信息
            try
            {
                //获取设备信息
                Game_PublicClassVar.Get_wwwSet.GetComponent<GetSignature>().GetDeviceInformation();
            }
            catch (Exception ex)
            {
                Debug.Log("华为获取信息错误:" + ex);
            }

            if (Game_PublicClassVar.Get_gameLinkServerObj.QiangZhiShiMingStatus)
            {
                /*
                string shenfenIDStr = PlayerPrefs.GetString("FangChenMi_ID");
                if (shenfenIDStr.Length <= 5)
                {
                    Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc.GetComponent<UI_StartGameFunc>().Btn_OpenRenZheng();
                    Game_PublicClassVar.Get_wwwSet.Show_GameHint("请先进行实名认证,未实名和未成年用户禁止登陆游戏!");
                    return;
                }
                */

                if (PlayerPrefs.GetInt("FangChenMi_Year") == 0)
                {
                    Debug.Log("GetInt:==0");
                    Game_PublicClassVar.Get_wwwSet.Show_GameHint("请先进行实名认证,未实名和未成年用户禁止登陆游戏!");
                    return;
                }

                if (PlayerPrefs.GetInt("FangChenMi_Year") <= 17)
                {
                    //判定时间
                    bool hintShiMing = true;

                    DateTime dtime = Game_PublicClassVar.Get_wwwSet.GetTime(Game_PublicClassVar.Get_wwwSet.EnterGameTimeStamp);
                    string dt = dtime.DayOfWeek.ToString();

                    //string dt = DateTime.Today.DayOfWeek.ToString();
                    if (dt.Contains("Friday") || dt.Contains("Saturday") || dt.Contains("Sunday"))
                    {
                        if (DateTime.Now.Hour == 21)
                        {
                            hintShiMing = false;
                        }
                    }

                    if (hintShiMing)
                    {
                        Game_PublicClassVar.Get_wwwSet.Show_GameHint("根据国家相关规定,未成年用户游戏只能在周五 六 日的21:00-22:00进行游戏!");
                        return;
                    }
                }
            }
        }
        else if (EventHandle.IsQudaoPackage())
        {
            //其他渠道包暂时不做处理;
        }


        Obj_GanXieBtn.SetActive(false);
        Obj_CreateRose.SetActive(true);

        if (EventHandle.IsHuiWeiChannel())
        {
            //渠道的检测放在用户点击获取权限之后
            Game_PublicClassVar.Get_getSignature.excuteCheckAction();
        }
    }

    private void onLoginFail()
    {
        Game_PublicClassVar.Get_wwwSet.Show_GameHint("登录失败，请重新登录！");
    }

}
