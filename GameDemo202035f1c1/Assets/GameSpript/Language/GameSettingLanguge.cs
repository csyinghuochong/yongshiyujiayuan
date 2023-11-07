using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameSettingLanguge : MonoBehaviour {

    public bool LangSetting_Chinese;
    public bool LangSetting_English;

    public Dictionary<string, LangugeType> LangugeList = new Dictionary<string, LangugeType>();

    public struct LangugeType{
        public string cn;
        public string en;

    }

    public bool langLoadStatus;             //本地化语言加载状态 

    //随机名称
    public int ranNameNum;
    public string[] randomName_xing;
    public string[] randomName_name;

    // Use this for initialization
    void Start () {

        //读取本地化内容并赋值
        StartCoroutine("LoadWWW");
        StartCoroutine("LoadWWW_Xing");
        StartCoroutine("LoadWWW_Name");
        
    }
	
	// Update is called once per frame
	void Update () {

        if (LangSetting_Chinese) {
            LangSetting_Chinese = false;
            SetLanguage("Chinese");
        }

        if (LangSetting_English) {
            LangSetting_English = false;
            SetLanguage("English");
        }

	}

    public void SetLanguage(string language)
    {
        Game_PublicClassVar.Get_wwwSet.GameSetLanguage._Language = language;
        Game_PublicClassVar.Get_wwwSet.GameSetLanguage.ApplyLanguage();
    }


    public string LoadLocalization(string getString) {

        //string nowGetString = get_uft8(getString);

        if (langLoadStatus)
        {
            if (LangugeList.ContainsKey(getString))
            {
                LangugeType lang = LangugeList[getString];
                string en_str = "";
                switch (Game_PublicClassVar.Get_wwwSet.GameSetLanguage._Language) {
                    case "Chinese":
                        en_str = lang.cn;
                        break;

                    case "English":
                        en_str = lang.en;
                        break;
                }


                //string en_str = LangugeList[getString].en;
                return en_str;
            }
            else {
                //Debug.Log(getString + "没有本地化资源...");
                return getString;
            }
        }
        else {
            //Debug.Log(getString + "本地化资源加载失败...");
            return getString;
        }
    }


    public string LoadLocalizationHint(string getString) {
        return LoadLocalization(getString);
    }


    public string get_uft8(string unicodeString)
    {
        UTF8Encoding utf8 = new UTF8Encoding();
        Byte[] encodedBytes = utf8.GetBytes(unicodeString);
        String decodedString = utf8.GetString(encodedBytes);
        return decodedString;
    }


    /// <summary>
    /// 使用一个协程来进行文件读取
    /// </summary>
    /// <returns></returns>
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    IEnumerator LoadWWW()
    {
        WWW www;
        //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
        if (Application.platform == RuntimePlatform.Android)
        {

            www = new WWW(Application.streamingAssetsPath + "/" + "Localization.txt");
        }
        else
        {
            //Debug.Log("开始加载字11111");
            www = new WWW("file://" + Application.streamingAssetsPath + "/" + "Localization.txt");
            //Debug.Log("开始加载字22222" + www.bytes.Length);
        }
        yield return www;

        if (!(www.Equals("") || www.Equals(null)))
        {
            //Debug.Log("开始加载屏蔽字33333");
            //LocalizationDebug.Log(www.text);

            string wwwStr = www.text;
            wwwStr = wwwStr.Replace("\r", "");
            wwwStr = wwwStr.Replace("\n", "");

            //将读取到的字符串进行分割后存储到定义好的数组中
            string[] zuList = wwwStr.Split('@');
            for (int i = 0; i < zuList.Length; i++) {
                string[] List = zuList[i].Split('#');
                if (List.Length >= 3) {
                    LangugeType langType = new LangugeType();
                    langType.cn = List[1];
                    langType.en = List[2];
                    if (LangugeList.ContainsKey(List[0]) == false)
                    {
                        LangugeList.Add(List[0], langType);
                    }
                    else {
                        //Debug.Log("本地化语言包有重复项目:" + List[0]);
                    }

                }
            }

            langLoadStatus = true;
        }
    }


    /// <summary>
    /// 使用一个协程来进行文件读取
    /// </summary>
    /// <returns></returns>
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    IEnumerator LoadWWW_Xing()
    {
        WWW www;
        //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
        if (Application.platform == RuntimePlatform.Android)
        {

            www = new WWW(Application.streamingAssetsPath + "/" + "RandName_Xing.txt");
        }
        else
        {
            //Debug.Log("开始加载字11111");
            www = new WWW("file://" + Application.streamingAssetsPath + "/" + "RandName_Xing.txt");
            //Debug.Log("开始加载字22222" + www.bytes.Length);
        }
        yield return www;

        if (!(www.Equals("") || www.Equals(null)))
        {
            //Debug.Log("开始加载屏蔽字33333");
            //LocalizationDebug.Log(www.text);

            string wwwStr = www.text;
            wwwStr = wwwStr.Replace("\r", "");
            wwwStr = wwwStr.Replace("\n", "");

            //将读取到的字符串进行分割后存储到定义好的数组中
            randomName_xing = wwwStr.Split('@');

            ranNameNum = ranNameNum + 1;
        }
    }


    /// <summary>
    /// 使用一个协程来进行文件读取
    /// </summary>
    /// <returns></returns>
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    IEnumerator LoadWWW_Name()
    {
        WWW www;
        //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
        if (Application.platform == RuntimePlatform.Android)
        {

            www = new WWW(Application.streamingAssetsPath + "/" + "RandName_Name.txt");
        }
        else
        {
            //Debug.Log("开始加载字11111");
            www = new WWW("file://" + Application.streamingAssetsPath + "/" + "RandName_Name.txt");
            //Debug.Log("开始加载字22222" + www.bytes.Length);
        }
        yield return www;

        if (!(www.Equals("") || www.Equals(null)))
        {
            //Debug.Log("开始加载屏蔽字33333");
            //LocalizationDebug.Log(www.text);

            string wwwStr = www.text;
            wwwStr = wwwStr.Replace("\r", "");
            wwwStr = wwwStr.Replace("\n", "");

            //将读取到的字符串进行分割后存储到定义好的数组中
            randomName_name = wwwStr.Split('@');

            ranNameNum = ranNameNum + 1;
        }
    }


}
