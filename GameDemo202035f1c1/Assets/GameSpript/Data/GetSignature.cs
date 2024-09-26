using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ICSharpCode.SharpZipLib.Zip;
using System.Net.NetworkInformation;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;


public class GetSignature : MonoBehaviour
{

    Pro_SheBeiData pro_SheBeiData = new Pro_SheBeiData();
    public ObscuredString MD5Str;

    private ObscuredString checkPackages = "de.robv.android.xposed.installer&_&com.topjohnwu.magisk"; //要检测的包名,用&_&分割
    public ObscuredInt JianCeNum = 0;
    public int batteryLevel = 100;
    public string ChannelId = "1";

#if UNITY_IPHONE && !UNITY_EDITOR
     [DllImport("__Internal")]
     private static extern void CheckIphoneYueyu( string str );
#endif

    //public string[] texts = new string[100];


    // Use this for initialization
    void Start()
    {
        //Debug.Log("OpenZipOpenZipOpenZipOpenZip");
        JianCeNum = 0;
        batteryLevel = 100;
        NativeController.Instance.Init();
        try
        {
            OpenZip();
        }
        catch (Exception ex)
        {
            Debug.Log("读取信息异常111" + ex);
        }

        try
        {
            //texts = new string[100];
            if (EventHandle.IsHuiWeiChannel() == false) {
                if (PlayerPrefs.GetInt("GameYinSi") == 1)
                {
                    GetDeviceInformation();
                }
            }

            /*
            foreach(Text ttt in texts)
            {
                Debug.Log("读取设备信息:" + ttt.text.ToString());
            }
            */
        }
        catch (Exception ex)
        {
            Debug.Log("读取信息异常222" + ex);
        }
        //Debug.Log("EndEndEnd...");

        Game_PublicClassVar.Get_wwwSet.PlayerSheBeiData = pro_SheBeiData;


        try
        {
            //读取MD5值
            MD5Str = GetSignatureMD5Hash();
            MD5Str = MD5Str.ToString().Replace(":", "");
            Game_PublicClassVar.Get_wwwSet.GameMD5Str = MD5Str;
        }
        catch (Exception ex)
        {
            Debug.Log("读取信息异常3333" + ex);
        }

#if UNITY_ANDROID

    #if QQ
        ChannelId = "1";
    #endif

    #if Share
        ChannelId = "2";
    #endif
    #if TapTap
        ChannelId = "101";
    #endif

    #if Plat4399
            ChannelId = "102";
    #endif

    #if WanDouJia
            ChannelId = "103";
    #endif

    #if YingYongBao
            ChannelId = "104";
    #endif

    #if HuaWei
            ChannelId = "201";
    #endif

    #if XiaoMi
            ChannelId = "202";
    #endif

#endif

        Debug.Log("unity: ChannelId " + ChannelId);

    }

    public void SetChannelID( string channel_id )
    {
        ChannelId = channel_id;
    }

    void Update()
    {

        if (JianCeNum >= 2)
        {

            if (Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus)
            {
                if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
                {
                    Game_PublicClassVar.Get_gameLinkServerObj.CheckAndroid();
                }
            }
        }
    }

    /// <summary>   
    /// 解压功能(解压压缩文件到指定目录)   
    /// </summary>   
    /// <param name="fileToUnZip">待解压的文件</param>   
    /// <param name="zipedFolder">指定解压目标目录</param>   
    /// <param name="password">密码</param>   
    /// <returns>解压结果</returns>   
    public bool UnZip(string fileToUnZip, string zipedFolder, string password)
    {
        bool result = true;
        FileStream fs = null;
        ZipInputStream zipStream = null;
        ZipEntry ent = null;
        string fileName;

        if (!File.Exists(fileToUnZip))
            return false;

        if (!Directory.Exists(zipedFolder))
            Directory.CreateDirectory(zipedFolder);

        try
        {
            zipStream = new ZipInputStream(File.OpenRead(fileToUnZip));
            if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
            while ((ent = zipStream.GetNextEntry()) != null)
            {
                if (!string.IsNullOrEmpty(ent.Name))
                {
                    fileName = Path.Combine(zipedFolder, ent.Name);
                    fileName = fileName.Replace('/', '\\');//change by Mr.HopeGi   

                    if (fileName.EndsWith("\\"))
                    {
                        Directory.CreateDirectory(fileName);
                        continue;
                    }

                    fs = File.Create(fileName);
                    int size = 2048;
                    byte[] data = new byte[size];
                    while (true)
                    {
                        size = zipStream.Read(data, 0, data.Length);
                        if (size > 0)
                            fs.Write(data, 0, data.Length);
                        else
                            break;
                    }
                }
            }
        }
        catch
        {
            result = false;
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
                fs.Dispose();
            }
            if (zipStream != null)
            {
                zipStream.Close();
                zipStream.Dispose();
            }
            if (ent != null)
            {
                ent = null;
            }
            GC.Collect();
            GC.Collect(1);
        }
        return result;
    }

    public void OpenZipFile()
    {

        //万物皆对象
        //一个压缩文件看成一个对象zf.GetHashCode = -1955568384

        ZipFile zf = new ZipFile(Application.dataPath + "/META-INF/");

        //一个压缩文件内，包括多个被压缩的文件
        foreach (ZipEntry entry in zf)
        {
            //一个被压缩文件,称为一个条目
            //Debug.Log("压缩包内文件名:" + entry.Name);
            //Debug.Log("压缩包大小:" + entry.Size);
            //Debug.Log("GetHashCode:" + entry.GetHashCode());

            //解压出被压缩的文件

            FileStream fs = new FileStream(entry.Name, FileMode.Create);

            //获取从压缩包中读取数据的流
            Stream input = zf.GetInputStream(entry);

            byte[] buffer = new byte[10 * 1024];

            int length = 0;
            while ((length = input.Read(buffer, 0, 10 * 1024)) > 0)
            {
                fs.Write(buffer, 0, length);

            }
            fs.Close();
            input.Close();
        }

        //Console.Read();


    }




    public void OpenZip()
    {

        //万物皆对象
        //一个压缩文件看成一个对象

        ZipFile zf = new ZipFile(Application.dataPath);

        //Debug.Log("zf.Size = " + zf.Size);
        //Debug.Log("zf.BufferSize = " + zf.BufferSize);
        //Debug.Log("zf.GetHashCode = " + zf.GetHashCode());
        //Debug.Log("zf.Count = " + zf.Count);

        Game_PublicClassVar.Get_wwwSet.gameCount = zf.Count.ToString();

        //一个压缩文件内，包括多个被压缩的文件
        foreach (ZipEntry entry in zf)
        {
            //一个被压缩文件,称为一个条目
            //Debug.Log("压缩包内文件名:" + entry.Name);
            //Debug.Log("压缩包大小:" + entry.Size);

            if (entry.Name == "AndroidManifest.xml")
            {
                Game_PublicClassVar.Get_wwwSet.fileSize = entry.Size.ToString();
            }

            /*
            //解压出被压缩的文件
            string pathttt = Path.Combine(Application.dataPath, entry.Name);
            Debug.Log("pathttt = " + pathttt);
            FileStream fs = new FileStream(pathttt, FileMode.Create);
            Debug.Log("pathttt111 = " + pathttt);
            //获取从压缩包中读取数据的流
            Stream input = zf.GetInputStream(entry);
            Debug.Log("pathttt2222 = " + pathttt);
            byte[] buffer = new byte[10 * 1024];

            int length = 0;
            while ((length = input.Read(buffer, 0, 10 * 1024)) > 0)
            {
                fs.Write(buffer, 0, length);

            }
            fs.Close();
            input.Close();
            */
        }

        //Console.Read();

    }



    private string GetSignatureMD5Hash()
    {
        //Debug.Log ("GetSignatureMD5Hash");
        var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var activity = player.GetStatic<AndroidJavaObject>("currentActivity");
        //Debug.Log("md5_111111111111111111111111");
        var PackageManager = new AndroidJavaClass("android.content.pm.PackageManager");
        //Debug.Log("md5_222222222222222222222222");
        var packageName = activity.Call<string>("getPackageName");
        //Debug.Log("md5_33333333333333333333333333");
        var GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
        var packageManager = activity.Call<AndroidJavaObject>("getPackageManager");
        var packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
        var signatures = packageInfo.Get<AndroidJavaObject[]>("signatures");
        //Debug.Log("md5_44444444444444444");
        if (signatures != null && signatures.Length > 0)
        {
            //Debug.Log("md5_5555555555555555555555555:" + signatures.Length);
            byte[] bytes = signatures[0].Call<byte[]>("toByteArray");
            //Debug.Log("md5_666666666666666666");
            var md5String = GetMD5Hash(bytes);
            md5String = md5String.ToUpper();
            //Debug.Log("md5_7777777777777777777777");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5String.Length; ++i)
            {
                if (i > 0 && i % 2 == 0)
                {
                    sb.Append(':');
                }
                sb.Append(md5String[i]);
            }
            //Debug.Log("md5_="+ sb);
            return sb.ToString();

        }

        return null;
    }

    private string GetMD5Hash(byte[] bytedata)
    {
        //Debug.Log ("GetMD5Hash");
        try
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(bytedata);



            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5Hash() fail,error:" + ex.Message);
        }
    }


    public string fileMD5(string filePath)
    {
        try
        {
            byte[] retVal;
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                retVal = md5.ComputeHash(file);
            }
            string write = ToHex(retVal, "x2");

            return write;
        }
        catch (Exception ex)
        {
            Debug.Log("ex = " + ex);
        }
        return "报错路径:" + "filePath = " + filePath;
    }


    public string ToHex(byte[] bytes, string format)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in bytes)
        {
            stringBuilder.Append(b.ToString(format));
        }
        return stringBuilder.ToString();
    }


    //利用滑动条滚动条实现吧
    public void GetDeviceInformation()
    {
        /*
        texts[0] = "设备模型：" + SystemInfo.deviceModel;
        Debug.Log(texts[0]);
        texts[1] = "设备名称：" + SystemInfo.deviceName;
        Debug.Log(texts[1]);
        texts[2] = "设备类型：" + SystemInfo.deviceType;
        Debug.Log(texts[2]);
        texts[3] = "设备唯一标识符：" + SystemInfo.deviceUniqueIdentifier;
        Debug.Log(texts[3]);
        texts[4] = "是否支持纹理复制：" + SystemInfo.copyTextureSupport;
        Debug.Log(texts[4]);
        texts[5] = "显卡ID：" + SystemInfo.graphicsDeviceID;
        Debug.Log(texts[5]);
        texts[6] = "显卡名称：" + SystemInfo.graphicsDeviceName;
        texts[7] = "显卡类型：" + SystemInfo.graphicsDeviceType;
        texts[8] = "显卡供应商：" + SystemInfo.graphicsDeviceVendor;
        texts[9] = "显卡供应商ID：" + SystemInfo.graphicsDeviceVendorID;
        texts[10] = "显卡版本号：" + SystemInfo.graphicsDeviceVersion;
        texts[11] = "显存大小（单位：MB）：" + SystemInfo.graphicsMemorySize;
        texts[12] = "是否支持多线程渲染：" + SystemInfo.graphicsMultiThreaded;
        texts[13] = "支持的渲染目标数量：" + SystemInfo.supportedRenderTargetCount;
        texts[14] = "系统内存大小（单位：MB）：" + SystemInfo.systemMemorySize;
        texts[15] = "操作系统：" + SystemInfo.operatingSystem;
        Debug.Log(texts[15]);
        */

        pro_SheBeiData.SheBei_deviceModel = SystemInfo.deviceModel;
        pro_SheBeiData.SheBei_deviceName = SystemInfo.deviceName;
        pro_SheBeiData.SheBei_deviceType = SystemInfo.deviceType.ToString();
        pro_SheBeiData.SheBei_deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
        pro_SheBeiData.SheBei_graphicsDeviceID = SystemInfo.graphicsDeviceID.ToString();
        pro_SheBeiData.SheBei_graphicsDeviceName = SystemInfo.graphicsDeviceName;
        pro_SheBeiData.SheBei_graphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();
        pro_SheBeiData.SheBei_graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;
        pro_SheBeiData.SheBei_graphicsDeviceVendorID = SystemInfo.graphicsDeviceVendorID.ToString();
        pro_SheBeiData.SheBei_graphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;
        pro_SheBeiData.SheBei_graphicsMemorySize = SystemInfo.graphicsMemorySize.ToString();
        pro_SheBeiData.SheBei_systemMemorySize = SystemInfo.systemMemorySize.ToString();
        pro_SheBeiData.SheBei_operatingSystem = SystemInfo.operatingSystem;


        try
        {
            GetMacAddress();
        }
        catch (Exception ex)
        {
            Debug.LogError("获取Mac报错！" + ex);
        }

        try
        {
            if ( !EventHandle.IsHuiWeiChannel())
            {
                GetDeviceIMEI();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("获取唯一编码报错！" + ex);
        }
    }

    /// <summary>
    /// 获取mac地址
    /// </summary>
    public void GetMacAddress()
    {
        NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
        List<string> strList = new List<string>();
        foreach (NetworkInterface ni in nis)
        {
            //Debug.Log("Name = " + ni.Name);
            //Debug.Log("Des = " + ni.Description);
            //Debug.Log("Type = " + ni.NetworkInterfaceType.ToString());
            //Debug.Log("Mac地址 = " + ni.GetPhysicalAddress().ToString());
            //texts[16] += "   mac地址：" + ni.GetPhysicalAddress().ToString();
            string macDiZhi = ni.GetPhysicalAddress().ToString();
            if (macDiZhi != "" && macDiZhi != "0" && macDiZhi != null)
            {
                strList.Add(ni.Name + "/" + ni.Description + "/" + ni.NetworkInterfaceType.ToString() + "/" + macDiZhi);
            }
        }
        pro_SheBeiData.SheBei_MacDiZhi = strList;
        //Debug.Log("数量：" + pro_SheBeiData.SheBei_MacDiZhi.Count);
    }




    /// <summary>
    /// 手机序列号是IMEI码的俗称。
    /// IMEI为TAC + FAC + SNR + SP。
    /// IMEI(International Mobile Equipment Identity)是国际移动设备身份码的缩写，
    /// 国际移动装备辨识码，是由15位数字组成的"电子串号"，
    /// 它与每台移动电话机一一对应，而且该码是全世界唯一的。
    /// </summary>
    #region 获得安卓手机上的IMEI号
    string imei0 = "";
    string imei1 = "";
    string meid = "";
    public void GetDeviceIMEI()
    {
        //Debug.Log("开始获取IMEI");
        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        var telephoneyManager = context.Call<AndroidJavaObject>("getSystemService", "phone");
        imei0 = telephoneyManager.Call<string>("getImei", 0);//如果手机双卡 双待  就会有两个MIEI号
        imei1 = telephoneyManager.Call<string>("getImei", 1);
        meid = telephoneyManager.Call<string>("getMeid");//电信的手机 是MEID
                                                         //texts[17] = "IMEI0:" + imei0;
                                                         //texts[18] = "IMEI1:" + imei1;
                                                         //texts[19] = "MEID:" + meid;
                                                         //Debug.Log(texts[17]);
                                                         //Debug.Log(texts[18]);
                                                         //Debug.Log(texts[19]);
        pro_SheBeiData.SheBei_imei0 = imei0;
        pro_SheBeiData.SheBei_imei1 = imei1;
        pro_SheBeiData.SheBei_meid = meid;
    }
    #endregion


    /// <summary>
    /// ios检测
    /// </summary>
    /// <param name="yueyu"></param>
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]

    public void OnRecvYueyu(string yueyu)
    {
        Game_PublicClassVar.Get_wwwSet.IfRootStatus = yueyu;
        JianCeNum = JianCeNum + 1;
    }

    /// <summary>
    /// ios检测
    /// </summary>
    /// <param name="yueyu"></param>
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]

    public void OnRecvIosSignature(string sign)
    {
        UnityEngine.Debug.Log("OnRecvIosSignature: " + sign);
    }

    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    public void OnRecvBundleids(string bundles)
    {
        Game_PublicClassVar.Get_wwwSet.CheckApkNameStr = bundles;
        JianCeNum = JianCeNum + 1;
    }

    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    public void onRecvRoot(string root)
    {
        UnityEngine.Debug.Log($"onRecvRoot:{root}");
        //分别取出个十百位的值，拼接成字符串 "0_1_1";
        int rootPermisson = int.Parse(root);
        string a1 = (rootPermisson / 10000 % 10).ToString();
        string a2 = (rootPermisson / 1000 % 10).ToString();
        string a3 = (rootPermisson / 100 % 10).ToString();
        string a4 = (rootPermisson / 10 % 10).ToString();
        string a5 = (rootPermisson / 1 % 10).ToString();
        Game_PublicClassVar.Get_wwwSet.IfRootStatus = a1 + "_" + a2 + "_" + a3 + "_" + a4 + "_" + a5;
        JianCeNum = JianCeNum + 1;
    }

    /// <summary>
    /// 包名&&是否存在
    /// </summary>
    /// <param name="packNamesExist"></param>
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    public void onCheckPackage(string packNamesExist)
    {
        string newStr = "";
        string[] packList = Regex.Split(packNamesExist, "&_&", RegexOptions.IgnoreCase);
        for (int i = 0; i < packList.Length; i++)
        {
            if (i % 2 == 0)
            {
                newStr = newStr + "&_&" + packList[i];
            }
            else
            {
                newStr = newStr + "&x&" + packList[i];
            }
        }
        if (newStr.Length > 3)
        {
            newStr = newStr.Substring(3, newStr.Length - 3);
        }
        Game_PublicClassVar.Get_wwwSet.CheckApkNameStr = newStr;
        JianCeNum = JianCeNum + 1;

    }

    //获取系统系统
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    public void onRecvSysTime(string vv)
    {
        NativeController.Instance.onRecvSysTime(vv);
    }

    //获取渠道包
    public void OnRecvChannelID(string channel)
    {
        if (channel != null && channel.Length > 0)
            ChannelId = channel;

        Debug.Log("unity : OnRecvChannelID" + channel);
    }

    //获取电池电量
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    public void onRecvBattery(string level)
    {
        batteryLevel = Mathf.CeilToInt(float.Parse(level) * 100);
    }

    public int GetBatteryLevel()
    {
        return batteryLevel;
    }

    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    public void CheckIosSignature(string strparam)
    {
#if UNITY_IPHONE  && !UNITY_EDITOR                               
        CheckIosSignature(strparam);
#endif
    }

    /// <summary>
    /// 开始检测root 和  包名
    /// </summary>
    /// 
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    public void excuteCheckAction()
    {
        if (Game_PublicClassVar.Get_wwwSet.CheckApkNameStrAdd != "" && Game_PublicClassVar.Get_wwwSet.CheckApkNameStrAdd != null)
        {
            checkPackages = "de.robv.android.xposed.installer&_&com.topjohnwu.magisk&_&" + Game_PublicClassVar.Get_wwwSet.CheckApkNameStrAdd;
        }

        JianCeNum = 0;
        string strparam = this.checkPackages.ToString();

#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                jo.Call("excuteCheckAction", strparam );
            }
        }
#elif UNITY_IPHONE  && !UNITY_EDITOR
        CheckIphoneYueyu( strparam ); 
#endif

    }

    public void ReqGetChannel()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                jo.Call("ReqGetChannel" );
            }
        }
#endif
    }


    public int AgreeNumber = 0;
    //隐私权限
    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    public void onRequestPermissionsResult(string permissons)
    {
        UnityEngine.Debug.Log($"onRecvPermissionsResult！ {permissons}");
        if (this.AgreeNumber >= 10)
        {
            return;
        }

        string[] values = permissons.Split('_');
        if (values[1] == "0")
        {
            //Application.Quit();
            //return;
            this.AgreeNumber = 10;
        }
        this.AgreeNumber++;
        if (this.AgreeNumber >= 2 || permissons == "1_1")
        {
            PlayerPrefs.SetString(YinSi.PlayerPrefsYinSi, YinSi.YinSiValue);
            UnityEngine.Debug.Log($"onRequestPermissionsResult: StartUpdate");
            GameObject.Find("Canvas/HuaWeiYinSi").SetActive(false);

#if UNITY_ANDROID && !UNITY_EDITOR
            GameObject.Find("WWW_Set/TapTapSdk").GetComponent<TapTapSdkHelper>().TapInit_1();
#endif
        }
        //弹出界面
        //Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc.GetComponent<UI_StartGameFunc>().QingQiuQuanXianShow();
    }

    public void QuDaoRequestPermissions()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                UnityEngine.Debug.Log("unitycall: yyyy");
                jo.Call("QuDaoRequestPermissions" );
            }
        }
#else
        onRequestPermissionsResult("1_1");
#endif
    }
}
