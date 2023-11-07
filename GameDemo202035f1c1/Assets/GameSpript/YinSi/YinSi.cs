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

    public static string GetYingSiText()
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

    public static void ShowTextList(GameObject textItem)
    {
        string pageHtml = GetYingSiText();

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

}
