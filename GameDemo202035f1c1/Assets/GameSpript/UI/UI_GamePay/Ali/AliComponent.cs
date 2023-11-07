using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AliComponent : MonoBehaviour {

    string aliSDKCallObjName = "GameVar";

    //是java里的类，一些静态方法可以直接通过这个调用。
    //androidjavaobject 调用的话，会生成一个对象，就和java里new一个对象一样，可以通过对象去调用里面的方法以及属性。
    AndroidJavaClass javaClass;
    AndroidJavaObject javaActive;

    //"com.mafeng.alinewsdk.AliSDKActivity"是2018.11.01日更新的版本 对应安卓工程中的alinewsdk Module
    //而"com.mafeng.aliopensdk.AliSDKActivity"是之前的版本 对应安卓工程中的aliopensdk Module
    string javaClassStr = "com.example.alinewsdk.AliSDKActivity";  //"com.mafeng.aliopensdk.AliSDKActivity";
    string javaActiveStr = "currentActivity";



    void Start()
    {
        if (EventHandle.IsQudaoPackage())
            return;

#if UNITY_EDITOR
#elif UNITY_ANDROID
        
        javaActive = new AndroidJavaObject(javaClassStr);
        //初始化 获得项目对应的MainActivity
        //javaClass = new AndroidJavaClass(javaClassStr);
        //javaActive = javaClass.GetStatic<AndroidJavaObject>(javaActiveStr);
#endif

    }



    /// <summary>
    /// 支付宝支付
    /// </summary>
    public void AliPay(string OrderInfo)
    {

#if UNITY_EDITOR
#elif UNITY_ANDROID
        object[] objs = new object[] { OrderInfo,aliSDKCallObjName, "AliPayCallback" };
        javaActive.Call("AliPay", objs);
#endif

    }

    public delegate void AliSDKAction(string result);
    public AliSDKAction aliPayCallBack;

    /// <summary>支付宝支付回调</summary>
    //这里是同步调用,由SDK反馈支付结果
    public void AliPayCallback(string result)
    {

        aliPayCallBack(result);
        //告诉服务器已经支付 等待返回结果
        //再监听结果 进行发放奖励 实际上都是独立的
        if (result == "支付成功")
        {
            Debug.Log("支付宝支付成功");

        }
        else
        {
            Debug.Log("支付宝支付失败");
           
        }
    }

}
