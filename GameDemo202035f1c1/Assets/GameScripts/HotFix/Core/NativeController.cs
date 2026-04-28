using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections.Generic;
using UnityEngine;
using Weijing;

public class NativeController : Singleton<NativeController>
{

    private bool bCheckTime = false;
    private GameTimer mBatteryTimer;
    private ObscuredInt mAllowInterval = 2000;  //暂定2000毫秒。可以修改到100以内
    private int mCheckTimes = 0;                //检测次数，至少检测两次，经测试第一次差值很大，在100以上，后面基本都是在10以内
    private int mAllowTimes = 2;
    //ObscuredInt
    private int passFrame = 0;
    private int checkIntervalFrame = 0;

    private AndroidJavaClass jc;
    private AndroidJavaObject jo;

    private float m_lasttime;

    private float m_deltaTime;
    private float m_deltaTime2;

    private float _getFocusTime;
    public float GetFocusTime {
        get { return _getFocusTime; }
        set { _getFocusTime = value; }
    }

    private float _loseFocusTime;
    public float LoseFocusTime
    {
        get { return _loseFocusTime; }
        set { _loseFocusTime = value; }
    }

    private List<float> s_timelist;
    private List<float> s_timelist_Fps;

    public NativeController()
    {

    }

    public void Init()
    {
        //mAllowInterval = 2000;          
        //mCheckTimes = 0;
        //mAllowTimes = 2;
        m_lasttime = 0f;
        m_deltaTime2 = 0f;
        s_timelist = new List<float>();
        s_timelist_Fps = new List<float>();

#if UNITY_ANDROID && !UNITY_EDITOR
        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
#endif
    }

    public void SetCheckIntervalTime(float time)
    {
        checkIntervalFrame = Mathf.CeilToInt(Application.targetFrameRate * time);
    }

    public void CheckFocusTime()
    {

        //Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("unitytime1  m_deltaTime :" + m_deltaTime + "   m_deltaTime2:" + m_deltaTime2);
        //Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("unitytime1  LoseFocusTime: " + LoseFocusTime);
        //Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("unitytime1  GetFocusTime: " + GetFocusTime);

#if UNITY_ANDROID || UNITY_EDITOR

        if (s_timelist == null || s_timelist.Count < 2)
            return;

        float timeJiange = 0;
        string xunhuanStr = "";
        s_timelist.Add(Time.realtimeSinceStartup);
        for (int i = 0; i < s_timelist.Count - 1; i++)
        {
            if (s_timelist[i + 1] - s_timelist[i] > timeJiange)
            {
                timeJiange = s_timelist[i + 1] - s_timelist[i];
                xunhuanStr = xunhuanStr + ":" + timeJiange;
            }
        }

        s_timelist.RemoveAt(s_timelist.Count - 1);

        Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("unitytime1  timeJiange: " + timeJiange);
        float jiaodianTIme = GetFocusTime - LoseFocusTime;
        if (Game_PublicClassVar.Get_wwwSet.IfRootStatus.ToString().Contains("1"))
        {
            if (timeJiange >= 2)
            {

                if (jiaodianTIme < 0.1)
                {
                    //直接闪退
                    Game_PublicClassVar.Get_wwwSet.Show_YanZhengError("游戏时间异常.." + +timeJiange * 5 + " : " + jiaodianTIme * 5);
                }
                else
                {

                    float proJianCe = timeJiange / jiaodianTIme;
                    if (proJianCe >= 10)
                    {
                        //直接闪退
                        Game_PublicClassVar.Get_wwwSet.Show_YanZhengError("游戏时间异常..." + timeJiange * 5 + " : " + jiaodianTIme * 5);
                    }
                }
            }
        }

        s_timelist.Clear();
#endif
    }

    public void Update()
    {
        //if (m_lasttime != 0 && Time.realtimeSinceStartup - m_lasttime > 1f)
        //{
        //    Debug.Log("unitytime1  Time.time - m_lasttime :" + (Time.realtimeSinceStartup - m_lasttime) );
        //    Debug.Log("unitytime1  LoseFocusTime: " + LoseFocusTime);
        //    Debug.Log("unitytime1  GetFocusTime: " + GetFocusTime);
        //}
        //else
        //{
        //    Debug.Log("unitytime2  : " + Time.time);
        //}
      
        if (s_timelist.Count > 5)
        {
            s_timelist.RemoveAt(0);
        }
        s_timelist.Add(Time.realtimeSinceStartup);

        if (passFrame % 2 == 0)
            m_deltaTime = Time.realtimeSinceStartup - m_lasttime;
        else
            m_deltaTime2 = Time.realtimeSinceStartup - m_lasttime;

        m_lasttime = Time.realtimeSinceStartup ;
        if (jc == null || jo == null)
            return;

        if (bCheckTime)
        {
            passFrame++;
            if (passFrame >= checkIntervalFrame)
            {
                passFrame = 0;
                jo.Call("ReqSystemTime", "");
            }
        }

    }



    /// <summary>
    /// 检测电池电量
    /// </summary>
    /// <param name="time"></param>
    public void BeginCheckBattery(float time)
    {
        if (jc == null)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
#endif
        }

        if (jc == null || jo == null)
            return;

        FinishCheckBattery();
        mBatteryTimer = new GameTimer(time, 0, delegate ()
        {
            jo.Call("getBatteryStatus");
        });
        mBatteryTimer.Start();
    }

    public void FinishCheckBattery()
    {
        if (mBatteryTimer != null)
            mBatteryTimer.Dispose();
        mBatteryTimer = null;
    }

    /// <summary>
    /// 加测是否加速
    /// </summary>
    /// <param name="interval">检测时间间隔</param>
    public void BeginCheckTime(float interval = 3)
    {
        if (jc == null)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
#endif
        }

        SetCheckIntervalTime(interval);
        passFrame = 0;
        mCheckTimes = 0;
        bCheckTime = true;
    }

    public void FinishCheckTime()
    {
        bCheckTime = false;
        passFrame = 0;
        mCheckTimes = 0;
    }
    public void onRecvSysTime(string vv)
    {
        long t1 = long.Parse(vv);
        TimeSpan ts2 = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        long t2 = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds(); // Convert.ToInt64(ts2.TotalMilliseconds);

        if (Math.Abs(t1 - t2) < mAllowInterval)
        {
            return;
        }

        mCheckTimes++;
        if (mCheckTimes >= mAllowTimes)
        {
            Debug.Log("unity:  onRecvSysTime:[检测到游戏速度异常] ");
            Game_PublicClassVar.Get_gameLinkServerObj.HintMsgStatus_Exit = true;
            Game_PublicClassVar.Get_gameLinkServerObj.HintMsgText_Exit = "检测到游戏速度异常!断开与游戏连接...";
            //强制退出游戏
            Game_PublicClassVar.Get_wwwSet.forceExitGameStatus = true;
            Game_PublicClassVar.Get_wwwSet.forceExitGameTimeSum = 0;
        }

    }

}
