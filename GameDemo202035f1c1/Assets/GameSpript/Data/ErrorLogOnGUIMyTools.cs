using System.Collections.Generic;
using UnityEngine;


public class ErrorLogOnGUIMyTools : MonoBehaviour
{
    private List<string> m_logEntries = new List<string>();

    private bool m_IsVisible = false;

    private Rect m_WindowRect = new Rect(0, 0, Screen.width, Screen.height);

    private Vector2 m_scrollPositionText = Vector2.zero;

    private bool sendStatus;
    private int sendNum;

    private void Start()
    {
        Application.logMessageReceived += (string condition, string stackTrace, LogType type) =>
        {
            if (type == LogType.Exception || type == LogType.Error)
            {
                /*
                if (!m_IsVisible)
                {
                    m_IsVisible = true;
                }
                */

                bool ifSendStatus = true;
                string showErrorStr = string.Format("{0}\n{1}", condition, stackTrace);

                Game_PublicClassVar.Get_wwwSet.AddShowLogSava_NoKey("报错:" + showErrorStr);

                foreach (string entry in m_logEntries)
                {
                    if (entry == showErrorStr) {
                        ifSendStatus = false;
                    }
                }

                //检测服务器是否允许发送
                if (Game_PublicClassVar.Get_gameLinkServerObj.IfSendErrorData == false) {
                    ifSendStatus = false;
                }

                if (ifSendStatus)
                {
                    m_logEntries.Add(showErrorStr);

                    //发送错误消息
                    string zhanghaoID = "";
                    if (Application.loadedLevelName == "StartGame")
                    {
                        //不记录账号ID
                        //zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    }
                    else {
                        //获取账号ID
                        zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                    }
                    sendNum = sendNum + 1;
                    if (sendNum < 100)
                    {
                        Pro_ComStr_4 com_4 = new Pro_ComStr_4();
                        com_4.str_1 = zhanghaoID;
                        com_4.str_2 = condition;
                        com_4.str_3 = stackTrace;
                        com_4.str_4 = Application.version;
                        //Debug.Log("发送消息111!");
                        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000112, com_4);
                    }
                }
            }
        };

        /*
        string[] AAA = new string[1];
        string BBB = AAA[2];
        Debug.Log("bbb = " + BBB);



        for (int i = 0; i < 30; i++)
        {
            Debug.LogError("test error!");
        }
        */

    }

    private void Update()
    {
        if (!sendStatus)
        {
            if (Game_PublicClassVar.gameLinkServer.ServerLinkStatus) {

                sendStatus = true;
                //发送错误消息
                string zhanghaoID = "";
                if (Application.loadedLevelName == "StartGame")
                {

                }
                else
                {
                    //获取账号ID
                    zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                }

                if (Application.platform != RuntimePlatform.WindowsEditor)
                {
                    foreach (string entry in m_logEntries)
                    {
                        Pro_ComStr_4 com_4 = new Pro_ComStr_4();
                        com_4.str_1 = zhanghaoID;
                        com_4.str_3 = entry;
                        Debug.Log("发送消息222!");
                        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000110, com_4);
                    }

                    m_logEntries.Clear();
                }
            }
            
        }
    }

    void ConsoleWindow(int windowID)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear", GUILayout.MaxWidth(200), GUILayout.MaxHeight(100)))
        {
            m_logEntries.Clear();
        }
        if (GUILayout.Button("Close", GUILayout.MaxWidth(200), GUILayout.MaxHeight(100)))
        {
            m_IsVisible = false;
        }
        GUILayout.EndHorizontal();

        m_scrollPositionText = GUILayout.BeginScrollView(m_scrollPositionText);

        foreach (var entry in m_logEntries)
        {
            Color color = GUI.contentColor;
            GUI.contentColor = Color.red;
            GUILayout.TextArea(entry);
            GUI.contentColor = color;
        }
        GUILayout.EndScrollView();
    }

    private void OnGUI()
    {
        if (m_IsVisible)
        {
            m_WindowRect = GUILayout.Window(0, m_WindowRect, ConsoleWindow, "Console");
        }
    }
}
