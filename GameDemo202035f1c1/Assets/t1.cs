using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System;
using CodeStage.AntiCheat.ObscuredTypes;

public class t1 : MonoBehaviour {

    public GameObject Obj;
    static string encryptKey = "/7}G";
    
    public ObscuredInt CCC;
    // Use this for initialization
    void Start () {

        //测试解析
        /*
        string xinxi = Obj.GetComponent<Text>().text;
        Debug.Log("XINXI = " + xinxi);
        int start = xinxi.IndexOf("purchaseToken") + 22;
        int end = xinxi.IndexOf("signature")-10;

        Debug.Log("start = " + start + "end = " + end);

        xinxi = xinxi.Substring(start, end - start);

        Debug.Log("XINXI22222222 = " + xinxi);
        */
        /*
        Receipt_Google rt = JsonConvert.DeserializeObject<Receipt_Google>(xinxi);
        Debug.Log("rt.purchaseToken = " + rt.Store);
        */

        /*
        CheatInt tt11t = new CheatInt(1111);
        Debug.Log("tt11t = " + tt11t.GetData());
        tt11t.SetData(2222);
        Debug.Log("tt11t = " + tt11t.GetData());
        */


        /*
        //测试加密
        string j1 = Encrypt("123456");
        //Debug.Log("j1 = " + j1);

        string j2 = Decrypt(j1);
        //Debug.Log("j2 = " + j2);
        //Game_PublicClassVar.Get_xmlScript.costStr(xulieHaoID);

        string a1 = "a1";
        //Debug.Log("a11:" + a1.GetHashCode());
        //Debug.Log("a22:" + a1.GetHashCode());
        a1 = "a2";
        //Debug.Log("a33:" + a1.GetHashCode());
        */


        ObscuredInt aaa = 11111;
        ObscuredInt bbb = 22222;
        
        Debug.Log("aaa = " + aaa);
        Debug.Log("bbb = " + bbb);
        //ObscuredInt CCC;
        Debug.Log("CCC = " + CCC);
    }
	
	// Update is called once per frame
	void Update () {
		
	}



    //加密字符串
    public string Encrypt(string str)
    {

        try
        {
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
















    public class Receipt_Google
    {
        /// <summary>
        /// 
        /// </summary>
        public string Store { get; set; }
        /// <summary>
        //public string purchaseToken { get; set; }

    }

}
