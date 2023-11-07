using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WeChatComponent : MonoBehaviour {

    public string WXAppID = "wx0f24f5e538739f0d";
    public string WXAppSecret = "dd075324e0a4ab44bd49d972efcffedc";

    public bool isRegisterToWechat = false;

    AndroidJavaClass javaClass;
    AndroidJavaObject javaActive;
    public string javaClassStr ="com.unity3d.player.UnityPlayer";
    public string javaActiveStr = "currentActivity";

    public string WeChatCallObjName = "GameVar";

    //事件原型
    //public delegate void WeChatLogonCallback(WeChatUserData userData);//微信登录回调 userData:null表示登陆了失败 否则表示成功,携带用户公开的信息
    //public delegate void WeChatShareCallback(string result);//分享结果 result:ERR_OK(由于业务调整,现在始终返回分享成功)
    public delegate void WeChatPayCallback(int state);//微信充值回调  result 0:成功 -1失败 -2取消

    //public WeChatLogonCallback weChatLogonCallback;
    //public WeChatShareCallback weChatShareTextCallback, weChatShareImageCallback, weChatShareWebPageCallback;
    public WeChatPayCallback weChatPayCallback;


    void Start () {
        if (EventHandle.IsQudaoPackage())
            return;

#if UNITY_EDITOR
#elif UNITY_ANDROID
        //初始化 获得项目对应的MainActivity
        javaClass = new AndroidJavaClass(javaClassStr);
        javaActive = javaClass.GetStatic<AndroidJavaObject>(javaActiveStr);
#endif
        RegisterAppWechat();
    }

    [OPS.Obfuscator.Attribute.DoNotRenameAttribute]
    /// <summary> 微信初始化:注册ID </summary>
    public void RegisterAppWechat()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        //安卓端已经在java层做了 这里忽略 
        if (!isRegisterToWechat)
        {
            javaActive.Call("WechatInit", WXAppID);
        }
        isRegisterToWechat=true;
#endif
    }

    /// <summary> 是否安装了微信 </summary>
    public bool IsWechatInstalled()
    {

#if UNITY_EDITOR
        return false;
#elif UNITY_ANDROID
        return javaActive.Call<bool>("IsWechatInstalled");
#endif
		return false;
    }

    /// <summary>微信登录 </summary>
    public void WeChatLogon( string state)
    {

#if UNITY_EDITOR
#elif UNITY_ANDROID
        object[] objs = new object[] { WXAppID, state, WeChatCallObjName, "LogonCallback" };
        javaActive.Call("LoginWechat", objs);
#endif
    }

    /// <summary>微信充值</summary>
    public void WeChatPay(string appid, string mchid, string prepayid, string noncestr, string timestamp, string sign)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        //将服务器返回的参数 封装到object数组里 分别是:会话ID,随机字符串,时间戳,签名,支付结果通知回调的物体,物体上的某个回调函数名称
        object[] objs = new object[] { appid, mchid, prepayid, noncestr, timestamp, sign, WeChatCallObjName, "WechatPayCallback" };
        //调用安卓层的WeiChatPayReq方法 进行支付
        javaActive.Call("WeChatPayReq", objs);
#endif
    }

    /// <summary> 微信支付回调 </summary>
    public void WechatPayCallback(string retCode)
    {
        Debug.Log("微信支付回调微信支付回调微信支付回调:" + retCode);
        int state = int.Parse(retCode); 
        switch (state)
        {
            case -2:
                Debug.Log("支付取消");

                break;
            case -1:
                Debug.Log("支付失败");
             
                break;
            case 0:
                Debug.Log("支付成功");
          
                break;
        }
        weChatPayCallback(state);
    }
}
