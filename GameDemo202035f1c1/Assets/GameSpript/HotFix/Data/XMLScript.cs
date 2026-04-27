using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System;

public class XMLScript
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    //读取对应的XML   成功返回字符串，失败返回"-1"字符串
    //FindStr       查询的Key
    //QueryName     匹配的Key
    //QueryValue    匹配的值
    //FilePath      文件路径
    public string Xml_WWWGetDate(string FindStr, string QueryName, string QueryValue, WWW www)
    {

        //判定XML文件是否存在
        //if (File.Exists(FilePath))
        //{

        //WWW www;

        //开始协同
        //this.StartCoroutine(BeginLoad(XmlName));

        ///if (www.isDone) {

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(www.text);

        XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("DateArea").ChildNodes;

        //便利找匹配的字段
        foreach (XmlElement xe in xmlNodeList)
        {

            if (xe.GetAttribute(QueryName) == QueryValue)
            {

                return xe.GetAttribute(FindStr);

            }
        }

        //}


        //}
        //else {

        //return "-1";

        //}

        return "-1";

    }


    //读取对应的XML   成功返回字符串，失败返回"-1"字符串
    //FindStr       查询的Key
    //QueryName     匹配的Key
    //QueryValue    匹配的值
    //FilePath      文件路径
    public string Xml_GetDate(string FindStr, string QueryName, string QueryValue, string FilePath)
    {
 
        //判定XML文件是否存在
        if (File.Exists(FilePath))
        {

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(FilePath);

            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("NewDataSet").ChildNodes;

            //便利找匹配的字段
            foreach (XmlElement xe in xmlNodeList)
            {

                if (xe.GetAttribute(QueryName) == QueryValue)
                {

                    return xe.GetAttribute(FindStr);

                }
            }
        }
        else
        {
            return "-1";
        }

        return "-1";
    }

    //存储对应XML的值   成功返回true
    //SetKey        存储的Key
    //SetValue      存储的值
    //QueryName     匹配的Key
    //QueryValue    匹配的值
    //FilePath      文件路径
    public bool Xml_SetDate(string SetKey, string SetValue, string QueryName, string QueryValue, string FilePath)
    {
        //Debug.Log("FilePath1111 = " + FilePath);
        //判定XML文件是否存在
        if (File.Exists(FilePath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(FilePath);
            XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("NewDataSet").ChildNodes;

            //便利找匹配的字段
            foreach (XmlElement xe in xmlNodeList)
            {
                //找到对应的节点
                if (xe.GetAttribute(QueryName) == QueryValue)
                {
                    //修改对应的值
                    xe.SetAttribute(SetKey, SetValue);
                    //保存文件
                    //Debug.Log("FilePath2222 = " + FilePath);
                    xmlDoc.Save(FilePath);
                    return true;
                }
            }
        }
        else
        {
            return false;

        }

        return false;

    }


    public bool Xml_SetXmlDate(string setXmlText, string FilePath)
    {
        //Debug.Log("FilePath1111 = " + FilePath);
        //判定XML文件是否存在
        if (File.Exists(FilePath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(FilePath);
            xmlDoc.InnerText = setXmlText;
            //XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("NewDataSet").ChildNodes;

            xmlDoc.Save(FilePath);
        }
        else
        {
            return false;

        }

        return false;

    }


    public bool Xml_Create(string FilePath, WWW www)
    {

        //Debug.Log("正在创建" + FilePath);
        if (Game_PublicClassVar.Get_wwwSet.IfSaveXmlStatus) {

            //bool deleteStatus = false;
            //判定是否为存储文件夹,存储文件夹不予删除
            if (FilePath.IndexOf(Game_PublicClassVar.Get_wwwSet.NowSelectFileName) == -1)
            {
                if (File.Exists(FilePath))
                {
                    //删除此文件
                    File.Delete(FilePath);
                    //Debug.Log("覆盖文件:" + FilePath);
                }
            }
            else {
                //Debug.Log("不可覆盖的文件:" + FilePath);
            }
        }



        //判定XML文件是否存在
        if (!File.Exists(FilePath))
        {
            string tmepFile = FilePath.Replace(".xml", "_temp.xml");
            if (File.Exists(tmepFile))
            {
                UnityEngine.Debug.LogError("替换文档1:  " + "__" + tmepFile);
                File.Move(tmepFile, FilePath);
                return true;
            }

            if (Game_PublicClassVar.Get_wwwSet.IfAddKey)
            {
                //Debug.Log("开始创建加密文件：" + FilePath);
                //需要加密配置文件时解开此处
                //Debug.Log("www = " + www.text);
                //string addStr = Game_PublicClassVar.Get_xmlScript.CostKey(FilePath, datatableName);
                //没有文件目录创建文件夹

                string path = FilePath.Substring(0, FilePath.LastIndexOf("/"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Debug.Log("创建文件....");
                Game_PublicClassVar.Get_wwwSet.GengHuanMiChiStatus = false;

                //加密文件(老版,读的是明码文件)
                //addKey(www, FilePath);

                //文件解密(新版,读的是加密文件)
                //Debug.Log("加密文件文本：" + www);
                string xmlPath = Game_PublicClassVar.Get_xmlScript.CostKey_2(FilePath, www);
                //Debug.Log("创建加密文件成功：" + FilePath);
            }
            else {
                //Debug.Log("开始创建文件：" + FilePath);
                //Debug.Log("1");
                XmlDocument xmlDoc = new XmlDocument();
                //Debug.Log("2");
                //Debug.Log("www.text = " + www.text);
                xmlDoc.LoadXml(www.text);
                //Debug.Log("3");
                //创建子目录文件夹
                string path = FilePath.Substring(0, FilePath.LastIndexOf("/"));
                //Debug.Log("4");
                Directory.CreateDirectory(path);
                //Debug.Log("5");
                xmlDoc.Save(FilePath);
                //Debug.Log("创建文件成功：" + FilePath);
            }
            return true;
        }
        else
        {
            //Debug.Log("创建文件失败：" + FilePath);
            return false;
        }
        
    }


    //此处为创建XML但是不进行任何加密,用于读取创角数据
    public bool Xml_CreateNoKey(string FilePath, WWW www,bool ifSave=false)
    {

        //Debug.Log("正在创建" + FilePath);
        if (Game_PublicClassVar.Get_wwwSet.IfSaveXmlStatus)
        {
            //bool deleteStatus = false;
            //判定是否为存储文件夹,存储文件夹不予删除
            if (FilePath.IndexOf(Game_PublicClassVar.Get_wwwSet.NowSelectFileName) == -1)
            {
                if (File.Exists(FilePath))
                {
                    //删除此文件
                    File.Delete(FilePath);
                    //Debug.Log("覆盖文件:" + FilePath);
                }
            }
            else
            {
                //Debug.Log("不可覆盖的文件:" + FilePath);
            }

        }

        //判定XML文件是否存在
        if (!File.Exists(FilePath))
        {

            //Debug.Log("开始创建文件：" + FilePath);
            //Debug.Log("1");
            XmlDocument xmlDoc = new XmlDocument();
            //Debug.Log("2");
            xmlDoc.LoadXml(www.text);
            //Debug.Log("3");
            //创建子目录文件夹
            string path = FilePath.Substring(0, FilePath.LastIndexOf("/"));
            //Debug.Log("4");
            Directory.CreateDirectory(path);
            //Debug.Log("5");
            xmlDoc.Save(FilePath);
            //Debug.Log("创建文件成功：" + FilePath);

            return true;
        }
        else
        {
            //Debug.Log("创建文件失败：" + FilePath);
            return false;
        }

    }


    //此处为创建XML但是不进行任何加密,用于读取创角数据
    public void Xml_CreateFile(string FilePath, string str)
    {
        //Debug.LogError("进入文件...");
        if (File.Exists(FilePath))
        {
            //删除此文件
            //File.Delete(FilePath);
            //Debug.Log("覆盖文件:" + FilePath);
        }
        else {
            //File.Create(FilePath);
        }

        try
        {
            FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("utf-8"));            //54936是编码格式 表示简体中文 (GB18030)
            sw.Write(str);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        catch (Exception E)
        {
            Debug.Log("记录文件：" + E.Message);
        }

    }


    //加密字符串
    //初始化时,将所有数据加密
    public bool addStr(string str)
    {
        string destFile = "D:/1.txt";

        string Key = @"P@+#wG+Z";       //私匙
        string IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量

        //转换对应密匙到字节
        byte[] btKey = Encoding.Default.GetBytes(Key);
        byte[] btIV = Encoding.Default.GetBytes(IV);

        //声明一个加密类
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //打开一个文件，将文件的内容读入一个字符串，然后关闭该文件
        byte[] btFile = Encoding.UTF8.GetBytes(str);
        //捕捉异常
        using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
        {
            try
            {
                using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                {
                    //Debug.Log(btFile);

                    cs.Write(btFile, 0, btFile.Length);
                    cs.FlushFinalBlock();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        byte[] bytes = File.ReadAllBytes(destFile);
        //byte[] btFile = Encoding.UTF8.GetBytes(str);
        string aaa = "";
        for (int i = 0; i <= bytes.Length - 1; i++)
        {
            aaa = aaa + bytes[i] + "-";
            //Debug.Log(btFile[i]);
        }
        aaa = aaa.Substring(0, aaa.Length - 1);
        Debug.Log("aaa = " + aaa);

        return true;
    }


    //解密字符串(序列号)
    public string costStr(string str)
    {
        string destFile = Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "xuliehao.txt";

        string Key = @"P@+#wG+Z";       //私匙
        string IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量

        //转换对应密匙到字节
        byte[] btKey = Encoding.Default.GetBytes(Key);
        byte[] btIV = Encoding.Default.GetBytes(IV);

        //声明一个加密类
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //打开一个文件，将文件的内容读入一个字符串，然后关闭该文件
        //byte[] btFile = File.ReadAllBytes(str);
        string[] byteStrList = str.Split('-');
        byte[] btFile = new byte[byteStrList.Length];
        if (byteStrList.Length > 1)
        {
            for (int i = 0; i <= btFile.Length - 1; i++)
            {
                btFile[i] = byte.Parse(byteStrList[i]);
            }
        }
        else {
            Debug.Log("序列号错误！");
            return "-1";
        }


        //byte[] btFile = Encoding.UTF8.GetBytes(str);
        /*
        string aaa = "";
        for (int i = 0; i <= btFile.Length - 1; i++) {
            aaa = aaa +";"+ btFile[i];
            //Debug.Log(btFile[i]);
        }
        Debug.Log("aaa = " + aaa);
        */
        //byte[] btFile = File.ReadAllBytes(str);
        //string michi = System.Text.Encoding.UTF8.GetString(btFile);
        //Debug.Log("michi = " + michi);
        //michi = ASCIIEncoding.ASCII.GetString(btFile);
        //Debug.Log("ASSmichi = " + michi);

        //捕捉异常
        using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
        {
            try
            {
                using (CryptoStream cs = new CryptoStream(fs, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                {
                    cs.Write(btFile, 0, btFile.Length);
                    cs.FlushFinalBlock();
                }
            }
            catch
            {
                //验证失败在此报错
                Debug.Log("序列号错误！");
                return "-1";
                throw;
            }
            finally
            {
                //不论验证成功与否都执行
                fs.Close();
            }
        }

        string xuliehao = System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(destFile));
        //Debug.Log("xuliehao = " + xuliehao);
        //验证每个序列号是否都为正常字符
        for(int i = 0;i<=xuliehao.Length-1;i++){
            bool ifStrTrue = false;
            //验证字符串是否等于0-9
            for (int shuzi = 0; shuzi <= 9; shuzi++) {
                if (shuzi.ToString() == xuliehao.Substring(i, 1)) {
                    ifStrTrue = true;
                }
            }
            //验证字符串是否等于";"和","
            if (xuliehao.Substring(i, 1) == ",") {
                ifStrTrue = true;
            }
            if (xuliehao.Substring(i, 1) == ";")
            {
                ifStrTrue = true;
            }
            if (!ifStrTrue) {
                Debug.Log("序列号字符串有错误！");
                return "-1";
            }
        }

        File.Delete(destFile);
        return xuliehao;
    }


    //初始化时,将所有数据加密
    public bool addKey(WWW www, string FilePath)
    {


        string Key = @"P@+#wG+Z";       //私匙
        string IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量

        string destFile = FilePath;
        //转换对应密匙到字节
        byte[] btKey = Encoding.Default.GetBytes(Key);
        byte[] btIV = Encoding.Default.GetBytes(IV);

        //声明一个加密类
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //打开一个文件，将文件的内容读入一个字符串，然后关闭该文件
        byte[] btFile = Encoding.UTF8.GetBytes(www.text);
        //捕捉异常
        using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
        {
            try
            {
                using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                {
                    cs.Write(btFile, 0, btFile.Length);
                    cs.FlushFinalBlock();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
        return true;
    }

    public bool setKeyEx(byte[] btFile, string FilePath)
    {
        //Debug.Log("sourceFile = " + sourceFile + "    ********* FilePath = " + FilePath);
        //string Key = @"P@+#wG+Z";       //私匙
        //string IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量

        string Key = @"P@+#wG+C";               //私匙
        string IV = @"L%n67}G\Mk@k%:~H";        //加密偏移量

        string tmepFile = FilePath.Replace(".xml", "_temp.xml");
        //转换对应密匙到字节
        byte[] btKey = Encoding.Default.GetBytes(Key);
        byte[] btIV = Encoding.Default.GetBytes(IV);

        //声明一个加密类
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //打开一个文件，将文件的内容读入一个字符串，然后关闭该文件
        //byte[] btFile = Encoding.UTF8.GetBytes(www.text);
        //byte[] btFile = File.ReadAllBytes(sourceFile);

        if (File.Exists(tmepFile))
        {
            File.Delete(tmepFile);
        }

        //捕捉异常
        using (FileStream fs = new FileStream(tmepFile, FileMode.Create, FileAccess.Write))
        {
            try
            {
                using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                {

                    //Debug.Log(sourceFile + " 长度:" + btFile.Length);
                    if (btFile.Length > 10)
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        //cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                        cs.Dispose();

                        //删除旧的
                        if (File.Exists(FilePath))
                        {
                            File.Delete(FilePath);
                        }

                        File.Move(tmepFile, FilePath);
                        File.Delete(tmepFile);

                    }
                    else
                    {
                        Debug.LogError("写入存储加密文件字节较小setKey:" + FilePath.ToString());

                        Game_PublicClassVar.Get_gameLinkServerObj.HintMsgStatus_Exit = true;
                        Game_PublicClassVar.Get_gameLinkServerObj.HintMsgText_Exit = "写入存储文件异常!请及时联系管理...";

                        //Game_PublicClassVar.Get_wwwSet.logStatus_HuiDang = true;
                        Game_PublicClassVar.Get_wwwSet.AddShowLog_NoKey("写入存储加密文件字节较小setKey:" + FilePath.ToString() + "btFile.Length = " + btFile.Length);
                        //Game_PublicClassVar.Get_wwwSet.logStatus_HuiDang = false;

                    }

                }
            }
            catch (Exception ex)
            {
                Debug.LogError("加密文件报错:" + ex.ToString());
                //Game_PublicClassVar.Get_wwwSet.logStatus_HuiDang = true;
                Game_PublicClassVar.Get_wwwSet.AddShowLog_NoKey("加密文件报错:" + "sourceFile:" + FilePath + " " + ex.ToString());
                //Game_PublicClassVar.Get_wwwSet.logStatus_HuiDang = false;
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        //删除指定加密文件
        //File.Delete(sourceFile);
        return true;
    }


    //"_JieMi.xml"           ".xml"
    //对于用户存储数据进行加密
    public bool setKey(string sourceFile, string FilePath)
    {
        //Debug.Log("sourceFile = " + sourceFile + "    ********* FilePath = " + FilePath);
        //string Key = @"P@+#wG+Z";       //私匙
        //string IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量

        string Key = @"P@+#wG+C";               //私匙
        string IV = @"L%n67}G\Mk@k%:~H";        //加密偏移量

        string tmepFile = FilePath.Replace(".xml", "_temp.xml" );


        //转换对应密匙到字节
        byte[] btKey = Encoding.Default.GetBytes(Key);
        byte[] btIV = Encoding.Default.GetBytes(IV);

        //声明一个加密类
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //打开一个文件，将文件的内容读入一个字符串，然后关闭该文件
        //byte[] btFile = Encoding.UTF8.GetBytes(www.text);
        byte[] btFile = File.ReadAllBytes(sourceFile);

        if (File.Exists(tmepFile))
        {
            File.Delete(tmepFile);
        }

        //捕捉异常
        using (FileStream fs = new FileStream(tmepFile, FileMode.Create, FileAccess.Write))
        {
            try
            {
                using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                {

                    //Debug.Log(sourceFile + " 长度:" + btFile.Length);
                    if (btFile.Length > 10)
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        //cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                        cs.Dispose();

                        //删除旧的
                        if (File.Exists(FilePath))
                        {
                            File.Delete(FilePath);
                        }

                        File.Move(tmepFile, FilePath);
                        File.Delete(tmepFile);

                    }
                    else {
                        Debug.LogError("写入存储加密文件字节较小setKey:" + sourceFile.ToString());

                        Game_PublicClassVar.Get_gameLinkServerObj.HintMsgStatus_Exit = true;
                        Game_PublicClassVar.Get_gameLinkServerObj.HintMsgText_Exit = "写入存储文件异常!请及时联系管理...";

                        //Game_PublicClassVar.Get_wwwSet.logStatus_HuiDang = true;
                        Game_PublicClassVar.Get_wwwSet.AddShowLog_NoKey("写入存储加密文件字节较小setKey:" + sourceFile.ToString() + "btFile.Length = " + btFile.Length);
                        //Game_PublicClassVar.Get_wwwSet.logStatus_HuiDang = false;

                    }
                    
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("加密文件报错:" + ex.ToString());
                //Game_PublicClassVar.Get_wwwSet.logStatus_HuiDang = true;
                Game_PublicClassVar.Get_wwwSet.AddShowLog_NoKey("加密文件报错:" + "sourceFile:" + sourceFile + " " + ex.ToString());
                //Game_PublicClassVar.Get_wwwSet.logStatus_HuiDang = false;
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        //删除指定加密文件
        File.Delete(sourceFile);
        return true;
    }

    public MemoryStream CostKeyEx(string filePath, string fileNameText)
    {
        string Key = @"P@+#wG+C";               //私匙
        string IV = @"L%n67}G\Mk@k%:~H";        //加密偏移量

        if (Game_PublicClassVar.Get_wwwSet.GengHuanMiChiStatus)
        {
            Debug.Log("进来了");
            Key = @"P@+#wG+Z";               //私匙
            IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量
        }

        string sourceFile = filePath;
        //string[] destFileArr = filePath.Split('.');
        //string destFile = destFileArr[0] + "_JieMi." + destFileArr[1];
        string destFile = filePath.Replace(".xml", "_JieMi.xml");
        //转换对应密匙到字节
        byte[] btKey = Encoding.Default.GetBytes(Key);
        byte[] btIV = Encoding.Default.GetBytes(IV);

        //声明一个加密类
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //打开一个文件，将文件的内容读入一个字符串，然后关闭该文件
        byte[] btFile = File.ReadAllBytes(sourceFile);

        byte[] btFilenew = new byte[btFile.Length];
        MemoryStream ms = new MemoryStream();
        ms.Position = 0;


        //using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
        {
            try
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                {
                    cs.Write(btFile, 0, btFile.Length);
                    cs.FlushFinalBlock();


                    cs.Close();
                }
                }
            catch (Exception ex)
            {
                Game_PublicClassVar.Get_wwwSet.AddShowLog_NoKey("解密文件报错:" + "sourceFile:" + sourceFile + " " + ex.ToString());
                throw;
            }
            finally
            {
               //cs.Close();
            }
        }
        //Debug.Log("destFile = " + destFile);
        return ms;
    }

    //初始化解密指定XML文件
    public string CostKey(string filePath, string fileNameText)
    {

        string Key = @"P@+#wG+C";               //私匙
        string IV = @"L%n67}G\Mk@k%:~H";        //加密偏移量

        if (Game_PublicClassVar.Get_wwwSet.GengHuanMiChiStatus) {
            Debug.Log("进来了");
            Key = @"P@+#wG+Z";               //私匙
            IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量
        }

        string sourceFile = filePath;
        //string[] destFileArr = filePath.Split('.');
        //string destFile = destFileArr[0] + "_JieMi." + destFileArr[1];
        string destFile = filePath.Replace(".xml", "_JieMi.xml");
        //转换对应密匙到字节
        byte[] btKey = Encoding.Default.GetBytes(Key);
        byte[] btIV = Encoding.Default.GetBytes(IV);

        //声明一个加密类
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //打开一个文件，将文件的内容读入一个字符串，然后关闭该文件
        byte[] btFile = File.ReadAllBytes(sourceFile);
        
        using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
        {
            try
            {
                using (CryptoStream cs = new CryptoStream(fs, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                {
                    cs.Write(btFile, 0, btFile.Length);
                    cs.FlushFinalBlock();
                }
            }
            catch (Exception ex)
            {
                Game_PublicClassVar.Get_wwwSet.AddShowLog_NoKey("解密文件报错:" + "sourceFile:" + sourceFile + " " + ex.ToString());
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
        //Debug.Log("destFile = " + destFile);
        return destFile;
    }


    //读取加密文件中的字符串
    public string ReadCostKey(WWW www)
    {

        //string Key = @"P@+#wG+Z";               //私匙
        //string IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量

        string Key = @"P@+#wG+C";               //私匙
        string IV = @"L%n67}G\Mk@k%:~H";        //加密偏移量

        //Key = @"P@+#wG+Z";               //私匙
        //IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量

        /*
        if (Game_PublicClassVar.Get_wwwSet.GengHuanMiChiStatus)
        {
            Debug.Log("进来了");
            Key = @"P@+#wG+Z";               //私匙
            IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量
        }
        */

        //string sourceFile = filePath;
        //string[] destFileArr = filePath.Split('.');
        string destFile = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + "/" + "SetXmlLinShiJieMi." + "xml";

        //Debug.Log("destFile = " + destFile);
        //Debug.Log("www =  " + www.text);
        //如果文件存在需要删除文件
        if (File.Exists(destFile)) {
            File.Delete(destFile);
        }

        //转换对应密匙到字节
        byte[] btKey = Encoding.Default.GetBytes(Key);
        byte[] btIV = Encoding.Default.GetBytes(IV);

        //声明一个加密类
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //打开一个文件，将文件的内容读入一个字符串，然后关闭该文件
        //byte[] btFile = File.ReadAllBytes(sourceFile);
        byte[] btFile = www.bytes;
        //byte[] btFile = System.Text.Encoding.Default.GetBytes(AddText);

        using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
        {
            try
            {
                using (CryptoStream cs = new CryptoStream(fs, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                {
                    cs.Write(btFile, 0, btFile.Length);
                    //string jiemiStr111 = System.Text.Encoding.Default.GetString(btFile);
                    //Debug.Log("jiemiStr111 = " + jiemiStr111);
                    cs.FlushFinalBlock();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        //返回解密后的字符串
        string jiemiStr = File.ReadAllText(destFile);
        //string jiemiStr = System.Text.Encoding.Default.GetString(btFile);
        //Debug.Log("jiemiStr = " + jiemiStr);
        //删除解密后的文件
        File.Delete(destFile);
        return jiemiStr;
    }


    //初始化解密指定XML文件
    public string CostKey_2(string filePath, WWW www)
    {
        //Debug.Log("CostKey_2 = " + filePath);
        //string Key = @"P@+#wG+Z";       //私匙
        //string IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量

        string Key = @"P@+#wG+C";               //私匙
        string IV = @"L%n67}G\Mk@k%:~H";        //加密偏移量

        //filePath = "D:/sss/RoseData_AddJie.xml";
        string sourceFile = filePath;
        //string[] destFileArr = filePath.Split('.');
        //string destFile = destFileArr[0] + "_JieMi." + destFileArr[1];
        string destFile = filePath.Replace(".xml", "_JieMi.xml");
        //Debug.Log("CostKey_2 = " + destFile);
        //转换对应密匙到字节
        byte[] btKey = Encoding.Default.GetBytes(Key);
        byte[] btIV = Encoding.Default.GetBytes(IV);

        //声明一个加密类
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //打开一个文件，将文件的内容读入一个字符串，然后关闭该文件
        //byte[] btFile = File.ReadAllBytes(costStr);
        //byte[] btFile = Encoding.UTF8.GetBytes(costStr);
        byte[] btFile = www.bytes;

        using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
        {
            try
            {
                using (CryptoStream cs = new CryptoStream(fs, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                {
                    cs.Write(btFile, 0, btFile.Length);
                    cs.FlushFinalBlock();
                    cs.Dispose();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        Game_PublicClassVar.Get_xmlScript.setKey(destFile, filePath);

        return destFile;
    }


    //拷贝文件（需要拷贝的目录,拷贝到的目录）
    public void CopyFile(System.IO.DirectoryInfo path, string desPath)
    {
        //System.IO.DirectoryInfo path_1 = desPath

        //获取资源路径是否存在
        if (!Directory.Exists(desPath))
        {
            //创建此文件夹
            Directory.CreateDirectory(desPath);
        }
        else {
            //删除文件夹
            Directory.Delete(desPath,true);
            //创建此文件夹
            Directory.CreateDirectory(desPath);
        }


        //循环复制文件
        string sourcePath = path.FullName;
        System.IO.FileInfo[] files = path.GetFiles();
        foreach (System.IO.FileInfo file in files)
        {
            string sourceFileFullName = file.FullName;
            string destFileFullName = sourceFileFullName.Replace(sourcePath, desPath);
            file.CopyTo(destFileFullName, true);
        }
    }

    //删除指定文件
    public void FileDelete(string desPath)
    {
        Debug.Log("删除文件" + desPath);
        //获取资源路径是否存在
        if (!Directory.Exists(desPath))
        {
            Debug.Log("删除文件111" + desPath);
            //删除文件夹
            Directory.Delete(desPath, true);
            Debug.Log("删除文件222" + desPath);
        }
    }


    //复制一个指定的文件
    public void CopyOneFile(string CopyPath, string CopyFilePath){
        
        FileInfo fileinfo = new FileInfo(CopyPath);
        FileInfo fileinfo_2 = new FileInfo(CopyFilePath);
        if (!fileinfo_2.Exists)
        {
            //Debug.Log("CopyPath = " + CopyPath + ";CopyFilePath = " + CopyFilePath);
            fileinfo.CopyTo(CopyFilePath,false);        //不覆盖原有文件
        }
    }



    //对游戏进不去的时候对文件加密
    public void addFileXml() {
        string FilePath = "D:/sss/sss.xml";         //指定要加密的文件
        //sss();
        //WWW WWW_xml = new WWW("D:/sss/RoseData_AddJie.xml");

        string Key = @"P@+#wG+Z";       //私匙
        string IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量

        string destFile = FilePath;
        //转换对应密匙到字节
        byte[] btKey = Encoding.Default.GetBytes(Key);
        byte[] btIV = Encoding.Default.GetBytes(IV);

        //声明一个加密类
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //打开一个文件，将文件的内容读入一个字符串，然后关闭该文件
        //Debug.Log("www = " + Game_PublicClassVar.Get_wwwSet.WWW_xml.text);
        byte[] btFile = Encoding.UTF8.GetBytes(Game_PublicClassVar.Get_wwwSet.WWW_xml.text);
        //捕捉异常
        using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
        {
            try
            {
                using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                {
                    cs.Write(btFile, 0, btFile.Length);
                    cs.FlushFinalBlock();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
        //return true;
        Debug.Log("加密完成");
    }



    //*************服务器加密********************
    //加密字符串
    public string Encrypt(string str)
    {

        try
        {
            string encryptKey = "/7}G";

            byte[] key = Encoding.Unicode.GetBytes(encryptKey);//密钥
            byte[] data = Encoding.Unicode.GetBytes(str);//待加密字符串

            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();//加密、解密对象
            MemoryStream MStream = new MemoryStream();//内存流对象

            //用内存流实例化加密流对象
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);//向加密流中写入数据
            CStream.FlushFinalBlock();//将数据压入基础流
            byte[] temp = MStream.ToArray();//从内存流中获取字节序列
            CStream.Close();//关闭加密流
            MStream.Close();//关闭内存流

            return Convert.ToBase64String(temp);//返回加密后的字符串
        }
        catch
        {
            return str;
        }
    }

    //解密字符串
    public string Decrypt(string str)
    {

        try
        {
            string encryptKey = "/7}G";

            byte[] key = Encoding.Unicode.GetBytes(encryptKey);//密钥
            byte[] data = Convert.FromBase64String(str);//待解密字符串

            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();//加密、解密对象
            MemoryStream MStream = new MemoryStream();//内存流对象

            //用内存流实例化解密流对象
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);//向加密流中写入数据
            CStream.FlushFinalBlock();//将数据压入基础流
            byte[] temp = MStream.ToArray();//从内存流中获取字节序列
            CStream.Close();//关闭加密流
            MStream.Close();//关闭内存流

            return Encoding.Unicode.GetString(temp);//返回解密后的字符串
        }
        catch
        {
            return str;
        }
    }



    //*************服务器加密********************
    //加密字符串
    public string Encrypt_DongTai(string str)
    {
        try
        {

            string encryptKey = Game_PublicClassVar.Get_gameLinkServerObj.EncryptKey_DongTai;

            byte[] key = Encoding.Unicode.GetBytes(encryptKey);//密钥
            byte[] data = Encoding.Unicode.GetBytes(str);//待加密字符串

            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();//加密、解密对象
            MemoryStream MStream = new MemoryStream();//内存流对象

            //用内存流实例化加密流对象
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);//向加密流中写入数据
            CStream.FlushFinalBlock();//将数据压入基础流
            byte[] temp = MStream.ToArray();//从内存流中获取字节序列
            CStream.Close();//关闭加密流
            MStream.Close();//关闭内存流

            return Convert.ToBase64String(temp);//返回加密后的字符串
        }
        catch
        {
            return str;
        }
    }

    //解密字符串
    public string Decrypt_DongTai(string str)
    {

        try
        {
            string encryptKey = Game_PublicClassVar.Get_gameLinkServerObj.EncryptKey_DongTai;
            byte[] key = Encoding.Unicode.GetBytes(encryptKey);//密钥
            byte[] data = Convert.FromBase64String(str);//待解密字符串

            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();//加密、解密对象
            MemoryStream MStream = new MemoryStream();//内存流对象

            //用内存流实例化解密流对象
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);//向加密流中写入数据
            CStream.FlushFinalBlock();//将数据压入基础流
            byte[] temp = MStream.ToArray();//从内存流中获取字节序列
            CStream.Close();//关闭加密流
            MStream.Close();//关闭内存流

            return Encoding.Unicode.GetString(temp);//返回解密后的字符串
        }
        catch
        {
            return str;
        }
    }


}