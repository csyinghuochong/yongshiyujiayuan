using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class EditeScript : MonoBehaviour {

    public static string XmlPath_JM = Application.dataPath + "\\Bundles\\Config";
    //public static string XmlPath_YuanShi = Application.dataPath + "\\StreamingAssets\\GameData\\" + "Xml";
    
    public static string XmlPath_YuanShi = Application.dataPath + "\\GameXml\\" + "Xml";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [MenuItem("Tools/复制配置到Bundle")]
    public static void CopyAllConfig()
    {
        Debug.Log("复制配置到Bundle！");

        // 复制 Get_Xml 目录下所有文件
        string getXmlSrc = XmlPath_YuanShi + "\\Get_Xml\\";
        DirectoryInfo getRoot = new DirectoryInfo(getXmlSrc);
        if (getRoot.Exists)
        {
            FileInfo[] getFiles = getRoot.GetFiles();
            for (int i = 0; i < getFiles.Length; i++)
            {
                string nowFilePath = getFiles[i].ToString();
                if (nowFilePath.Substring(nowFilePath.Length - 4, 4) == "meta")
                {
                    Debug.Log("跳过meta文件：" + nowFilePath);
                    continue;
                }

                string fileName = getFiles[i].Name;
                string destPath = XmlPath_JM + "\\Get_Xml\\" + fileName;
                Debug.Log("复制文件：" + destPath);
                CopyOneFile(nowFilePath, destPath);
            }
        }
        else
        {
            Debug.LogWarning("Get_Xml 目录不存在：" + getXmlSrc);
        }

        // 复制 Set_Xml 顶层的 GameCreate.xml
        CopyOneFile(
            XmlPath_YuanShi + "\\Set_Xml\\GameCreate.xml",
            XmlPath_JM + "\\Set_Xml\\GameCreate.xml"
        );

        // 复制 Set_Xml 子目录下所有文件
        string setStr = "10001";
        string[] setStrList = setStr.Split(';');
        for (int z = 0; z < setStrList.Length; z++)
        {
            string path = XmlPath_YuanShi + "\\Set_Xml\\" + setStrList[z] + "\\";
            DirectoryInfo root = new DirectoryInfo(path);
            if (!root.Exists)
            {
                Debug.LogWarning("目录不存在：" + path);
                continue;
            }

            // 确保目标目录存在
            string destDir = XmlPath_JM + "\\Set_Xml\\" + setStrList[z] + "\\";
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            FileInfo[] files = root.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                string nowFilePath = files[i].ToString();
                if (nowFilePath.Substring(nowFilePath.Length - 4, 4) == "meta")
                {
                    Debug.Log("跳过meta文件：" + nowFilePath);
                    continue;
                }

                string fileName = files[i].Name;
                string destPath = destDir + fileName;
                Debug.Log("复制文件：" + destPath);
                CopyOneFile(nowFilePath, destPath);
            }
        }

        Debug.Log("复制完成！");
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/加密工具GetXml")]
    public static void XmlJiaMi() {
        Debug.Log("加密！");
        
        string path = XmlPath_YuanShi + "\\Get_Xml\\";
        Debug.Log("path = " + path);
        //string path_ChuShi = XmlPath_JM + "\\Get_Xml\\";
        DirectoryInfo root = new DirectoryInfo(path);
        FileInfo[] files = root.GetFiles();
        //去掉结尾是.meta的文件
        for (int i = 0; i < files.Length; i++) {
            string nowFilePath = files[i].ToString();
            string chuShiFilePath = files[i].ToString();
            if (nowFilePath.Substring(nowFilePath.Length - 4, 4) == "meta")
            {
                Debug.Log("发现meta文件！");
            }
            else {
                //获取文件名称
                string fileName = files[i].Name;
                //进行文件加密
                string jiamiFileName = XmlPath_JM + "\\Get_Xml\\"+ fileName;
                Debug.Log("生成加密文件：" + jiamiFileName);
                setKey(nowFilePath, jiamiFileName);
            }
        }
    }

    [MenuItem("Tools/加密工具SetXml")]
    public static void XmlJiaMi_SetXml()
    {
        Debug.Log("加密！");
        //复制不加密的整体配置文件
        CopyOneFile(XmlPath_YuanShi + "\\Set_Xml\\"+ "GameCreate.xml", XmlPath_JM + "\\Set_Xml\\GameCreate.xml");

        //文件名路径
        string setStr = "10001";
        string[] setStrList = setStr.Split(';');
        for (int z = 0; z < setStrList.Length; z++) {
            string path = XmlPath_YuanShi + "\\Set_Xml\\"+ setStrList[z]+"\\";
            //string path_ChuShi = XmlPath_JM + "\\Set_Xml\\" + setStrList[z] + "\\";
            DirectoryInfo root = new DirectoryInfo(path);
            FileInfo[] files = root.GetFiles();
            //去掉结尾是.meta的文件
            for (int i = 0; i < files.Length; i++)
            {
                string nowFilePath = files[i].ToString();
                if (nowFilePath.Substring(nowFilePath.Length - 4, 4) == "meta" || nowFilePath.Contains("GameConfig") || nowFilePath.Contains("GameCreate"))
                {
                    Debug.Log("发现meta文件！");
                    //直接复制文件,这俩文件是明码配置
                    if (nowFilePath.Contains("GameConfig") || nowFilePath.Contains("GameCreate")) {
                        //获取文件名称
                        string fileName = files[i].Name;
                        //进行文件加密
                        string jiamiFileName = XmlPath_JM + "\\Set_Xml\\" + setStrList[z] + "\\" + fileName;
                        Debug.Log("生成加密文件：" + jiamiFileName);
                        CopyOneFile(nowFilePath,jiamiFileName);
                    }
                }
                else
                {
                    //获取文件名称
                    string fileName = files[i].Name;
                    //进行文件加密
                    string jiamiFileName = XmlPath_JM + "\\Set_Xml\\" + setStrList[z] + "\\" + fileName;
                    Debug.Log("生成加密文件：" + jiamiFileName);
                    setKey(nowFilePath, jiamiFileName);
                }
            }
        }
    }

    //复制一个指定的文件
    public static void CopyOneFile(string CopyPath, string CopyFilePath)
    {
        string destDir = Path.GetDirectoryName(CopyFilePath);
        if (!Directory.Exists(destDir))
            Directory.CreateDirectory(destDir);

        FileInfo fileinfo = new FileInfo(CopyPath);
        fileinfo.CopyTo(CopyFilePath, true);  // true = 强制覆盖
        Debug.Log("已复制：" + CopyFilePath);
    }



    [MenuItem("Tools/解密Xml")]
    public static void XmlJieMi()
    {
        Debug.Log("加密！");

        //string path = XmlPath_YuanShi + "\\Get_Xml\\";
        string path = "C:\\1.xml";
        Debug.Log("path = " + path);
        CostKey(path);
        /*
        //string path_ChuShi = XmlPath_JM + "\\Get_Xml\\";
        DirectoryInfo root = new DirectoryInfo(path);
        FileInfo[] files = root.GetFiles();
        //去掉结尾是.meta的文件
        for (int i = 0; i < files.Length; i++)
        {
            string nowFilePath = files[i].ToString();
            string chuShiFilePath = files[i].ToString();
            if (nowFilePath.Substring(nowFilePath.Length - 4, 4) == "meta")
            {
                Debug.Log("发现meta文件！");
            }
            else
            {
                //获取文件名称
                string fileName = files[i].Name;
                //进行文件加密
                string jiamiFileName = XmlPath_JM + "\\Get_Xml\\" + fileName;
                Debug.Log("生成加密文件：" + jiamiFileName);
                setKey(nowFilePath, jiamiFileName);
            }
        }
        */
    }

    [MenuItem("Tools/解密Log")]
    public static void LogJieMi()
    {
        Debug.Log("加密！");

        //string path = XmlPath_YuanShi + "\\Get_Xml\\";
        string path = "C:\\1.txt";
        Debug.Log("path = " + path);

        /*
        //string path_ChuShi = XmlPath_JM + "\\Get_Xml\\";
        DirectoryInfo root = new DirectoryInfo(path);
        FileInfo[] files = root.GetFiles();
        //去掉结尾是.meta的文件
        for (int i = 0; i < files.Length; i++)
        {
            string nowFilePath = files[i].ToString();
            string chuShiFilePath = files[i].ToString();
            if (nowFilePath.Substring(nowFilePath.Length - 4, 4) == "meta")
            {
                Debug.Log("发现meta文件！");
            }
            else
            {
                //获取文件名称
                string fileName = files[i].Name;
                //进行文件加密
                string jiamiFileName = XmlPath_JM + "\\Get_Xml\\" + fileName;
                Debug.Log("生成加密文件：" + jiamiFileName);
                setKey(nowFilePath, jiamiFileName);
            }
        }
        */

    }


    //初始化解密指定XML文件
    public static string CostKey(string filePath)
    {

        //老的
        string Key = @"P@+#wG+Z";               //私匙
        string IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量

        //新的
        //string Key = @"P@+#wG+C";               //私匙
        //string IV = @"L%n67}G\Mk@k%:~H";        //加密偏移量

        string sourceFile = filePath;
        string[] destFileArr = filePath.Split('.');
        string destFile = destFileArr[0] + "_JieMi." + destFileArr[1];
        Debug.Log("destFile = " + destFile);
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
            catch
            {
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        return destFile;
    }



    //对于用户存储数据进行加密
    public static bool setKey(string sourceFile, string FilePath)
    {
        //Debug.Log("sourceFile = " + sourceFile + "    ********* FilePath = " + FilePath);
        //string Key = @"P@+#wG+Z";       //私匙
        //string IV = @"L%n67}G\Mk@k%:~Y";        //加密偏移量

        string Key = @"P@+#wG+C";               //私匙
        string IV = @"L%n67}G\Mk@k%:~H";        //加密偏移量

        string destFile = FilePath;
        //转换对应密匙到字节
        byte[] btKey = Encoding.Default.GetBytes(Key);
        byte[] btIV = Encoding.Default.GetBytes(IV);

        //声明一个加密类
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //打开一个文件，将文件的内容读入一个字符串，然后关闭该文件
        //byte[] btFile = Encoding.UTF8.GetBytes(www.text);
        byte[] btFile = File.ReadAllBytes(sourceFile);
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

        //删除指定加密文件
        //File.Delete(sourceFile);
        return true;
    }
}
