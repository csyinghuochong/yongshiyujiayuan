using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
public class YanZheng2 : MonoBehaviour {

    private const String host = "https://naidcard.market.alicloudapi.com";
    private const String path = "/nidCard";
    private const String method = "GET";
    private const String appcode = "d59fefe68cf644f6a8f54dd039c3806f";

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
	/// 获取当前时间戳  
	/// </summary>
	/// <param name="bflag"></param>为真时获取10位时间戳,为假时获取13位时间戳.bool bflag = true</param>  
	/// <returns></returns>
	public string GetTimeStamp(bool bflag)
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        string ret = string.Empty;
        if (bflag)
            ret = Convert.ToInt64(ts.TotalSeconds).ToString();
        else
            ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();
        return ret;
    }

    public bool YanZhengShenFen_New(string Name, string CardID)
    {
        Pro_PlayerYanZheng cc  = new Pro_PlayerYanZheng();
        cc.Name = Name;
        cc.SheBeiID = SystemInfo.deviceUniqueIdentifier; 
        cc.ShenFenID = CardID;
       
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(30000001, cc);
        return true;
    }

    public bool YanZhengShenFen_Old(string Name, string CardID)
    {
        Debug.Log("1111111111111111");

        string querys = "idCard=" + CardID + "&name=" + Name;
        string bodys = "";
        string url = host + path;
        HttpWebRequest httpRequest = null;
        HttpWebResponse httpResponse = null;

        if (0 < querys.Length)
        {
            url = url + "?" + querys;
        }

        if (host.Contains("https://"))
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
        }
        else
        {
            httpRequest = (HttpWebRequest)WebRequest.Create(url);
        }
        httpRequest.Method = method;
        httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
        if (0 < bodys.Length)
        {
            byte[] data = Encoding.UTF8.GetBytes(bodys);
            using (Stream stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }
        try
        {
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        }
        catch (WebException ex)
        {
            httpResponse = (HttpWebResponse)ex.Response;
        }

        Debug.Log(httpResponse.StatusCode);
        Debug.Log(httpResponse.Method);
        Debug.Log(httpResponse.Headers);
        Stream st = httpResponse.GetResponseStream();
        StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));

        //测试解析

        string xinxi = reader.ReadToEnd().ToString();
        Newtonsoft.Json.Linq.JArray jsonArr = GetToJsonList("["+ xinxi + "]");
        string status = jsonArr[0]["status"].ToString();
        //Debug.Log("status = " + status);

        switch (status)
        {
            //实名认证通过
            case "01":
                Debug.Log("实名认证通过");
                return true;
                break;
            //实名认证不通过
            case "02":
                Debug.Log("实名认证未通过！");
                return false;
                break;
            //其他
            default:
                Debug.Log("实名认证未通过！");
                return false;
                break;
        }
    }

    public bool YanZhengShenFen(string Name, string CardID)
    {
        bool value = true;

        if (value)
            return YanZhengShenFen_New(Name, CardID);
        else
            return YanZhengShenFen_Old(Name, CardID);
    }

    public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
        return true;
    }

    public static Newtonsoft.Json.Linq.JArray GetToJsonList(string json)
    {
        Newtonsoft.Json.Linq.JArray jsonArr = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(json);
        return jsonArr;
    }


}


