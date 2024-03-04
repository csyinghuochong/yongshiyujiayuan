using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class UILoginHelper
{
    public static void SetParent(GameObject son, GameObject parent)
    {
        if (son == null || parent == null)
            return;
        son.transform.SetParent(parent.transform);
        son.transform.localPosition = Vector3.zero;
        son.transform.localScale = Vector3.one;
    }

    public static string GetHongHuText()
    {
        try
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] pageData = MyWebClient.DownloadData("http://verification.weijinggame.com/weijing/yinsi3.txt"); //从指定网站下载数据
            string pageHtml = Encoding.UTF8.GetString(pageData);
            return pageHtml;
        }

        catch (WebException webEx)
        {
            UnityEngine.Debug.Log(webEx.ToString());
        }
        return "服务器维护中！";
    }

    public static string GetYingSiText()
    {
        try
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] pageData = MyWebClient.DownloadData("http://verification.weijinggame.com/weijing/yinsi4.txt"); //从指定网站下载数据
            string pageHtml = Encoding.UTF8.GetString(pageData);
            return pageHtml;
        }

        catch (WebException webEx)
        {
            UnityEngine.Debug.Log(webEx.ToString());
        }
        return "服务器维护中！";
    }

    public static void ShowTextList(string pageHtml, GameObject textItem)
    {
        string tempstr = string.Empty;
        string leftValue = pageHtml;
        int indexlist = pageHtml.IndexOf('\n');
        int whileNumber = 0;

        List<string> allString = new List<string>();

        while (indexlist != -1)
        {
            whileNumber++;
            if (whileNumber >= 1000)
            {
                break;
            }

            tempstr = leftValue.Substring(0, indexlist);
            allString.Add(tempstr);

            indexlist += 1;
            leftValue = leftValue.Substring(indexlist, leftValue.Length - indexlist);

            indexlist = leftValue.IndexOf('\n');

            if (indexlist == -1)
            {
                allString.Add(leftValue);
            }
        }

        string lineStr = string.Empty;

        GameObject parentobject = textItem.transform.parent.gameObject;
        int totalLength = allString.Count;
        for (int i = 0; i < totalLength; i++)
        {
            lineStr += allString[i] + '\n';

            if (lineStr.Length > 1000 || i == totalLength - 1)
            {
                lineStr = lineStr.Substring(0, lineStr.Length - 1);

                GameObject textGo = GameObject.Instantiate(textItem);
                SetParent(textGo, parentobject);

                Text text = textGo.GetComponent<Text>();

                text.text = lineStr;

                text.GetComponent<RectTransform>().sizeDelta = new Vector2(1200, text.preferredHeight);

                text.gameObject.SetActive(false);
                text.gameObject.SetActive(true);

                lineStr = string.Empty;
            }


        }
    }
}

public class YinSi : MonoBehaviour
{
    // Start is called before the first frame update
    public Button ButtonRefuse;
    public Button ButtonAgree;
    public Button Text_Button_1;
    public Button Text_Button_2;
    public GameObject YinSiXieYi;
    public GameObject YongHuXieYi;
    public GameObject TextYinSi;

    public static string PlayerPrefsYinSi = "YinSi0105";

    void Start()
    {
        TextYinSi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnText_Button_1()
    {
        YongHuXieYi.SetActive(true);
    }

    public void OnText_Button_2()
    {
        YinSiXieYi.SetActive(true);
    }

    public void OnCloseXieYi()
    {
        YongHuXieYi.SetActive(false);
        YinSiXieYi.SetActive(false);
    }

    /// <summary>
    /// 同意权限
    /// </summary>
    public void btnYes() 
    {
        //申请权限
        UnityEngine.Debug.Log("unitycall.btnYes");
        Game_PublicClassVar.Get_getSignature.QuDaoRequestPermissions();
    }

    /// <summary>
    /// 拒绝权限，退出游戏
    /// </summary>
    public void btnNo() 
    {
        Application.Quit();
    }

    //����˽
    public void Btn_OpenYinSi()
    {
        //Application.OpenURL("http://verification.weijinggame.com/yinsi/");

        Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc.GetComponent<UI_StartGameFunc>().Btn_OpenYinSi();
    }

    //注销账户数据
    public void Btn_ZhuXiao() {

        //弹出提示
        GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_CommonHintHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_19");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否注销自身帐号的所有数据，注销后自身所有数据将自动删除，如果你确定要注销帐号数据请点击下方确认按钮！", ZhuXiao_True, ZhuXiao_Flase, "系统提示", "确定", "取消", ZhuXiao_Flase);
        //DontDestroyOnLoad(uiCommonHint);
        if (GameObject.Find("Canvas/GameGongGaoSet") != null)
        {
            uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }

    //注销确认
    public void ZhuXiao_True() {

        //清空实名信息
        PlayerPrefs.SetInt("FangChenMi_Type", 0);
        PlayerPrefs.SetString("FangChenMi_Name", "");
        PlayerPrefs.SetString("FangChenMi_ID", "");

        //请求验证当前是否
        Pro_ComStr_3 proComStr_3 = new Pro_ComStr_3();
        proComStr_3.str_1 = SystemInfo.deviceUniqueIdentifier;
        proComStr_3.str_2 = "zhuxiao";
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000132, proComStr_3);

        //PlayerPrefs.SetString("FangChenMi_Name",null);
        //PlayerPrefs.SetString("FangChenMi_ID", null);
        //PlayerPrefs.SetString("FangChenMi_Year", null);
        //PlayerPrefs.SetString("YinSi", null);


        string createXmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/";
        string createIDListStr = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("CreateIDList", "ID", "1", createXmlPath + "GameCreate.xml");
        string deleteFileID = "";
        string[] createIDList = createIDListStr.Split(';');
        for (int i = 0; i < createIDList.Length; i++) {
            if (createIDList[i] != "") {
                string[] list = createIDList[i].Split(',');
                if (list.Length >= 2) {
                    
                    PlayerPrefs.SetString("YanZhengFileStr_" + list[1], "");
                }
            }
        }

        PlayerPrefs.Save();

        //删除所有数据
        Game_PublicClassVar.Get_wwwSet.DeleteAllDate();

        //退出游戏
        Game_PublicClassVar.Get_wwwSet.ExitGame();
    }

    //注销取消
    public void ZhuXiao_Flase() { 
        


    }

}
