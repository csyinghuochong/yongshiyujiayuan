using System;



using TapTap.Bootstrap;
using TapTap.Common;
using TapTap.Login;
using UnityEngine;
using TapTap.AntiAddiction;
using TapTap.AntiAddiction.Model;


public class TapTapSdkHelper : MonoBehaviour
{
    public static string clientId = "yfbkmzv4zafmyq8nzb";// "yrvsirol93o27hydc7";
    private string clientToken = "UAmCBcwjj6NPxQbOk2PRxlHWfFaSUblIxXOz7J8Q";// "4lTeqdbgS3FtR5i4UliTIUaAvN1Ga4AtirdUeGTB";
    private string serverUrl = "j7pl8yru0wLrdC4cwbdS6bzaDTByr9RQ";// "https://yrvsirol.cloud.tds1.tapapis.cn";
    private RegionType regionType = RegionType.CN;
    private bool showAntiAddictionSwitchAccount; //showAntiAddictionSwitchAccount 参数为 bool 类型，表示是否显示切换账号

    public Action<string> TapLoingHandler;
    public Action<int, string> AntiAddictionHandler;


    public GameObject ButtonInit;
    public GameObject ButtonLogin;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    //从 3.27.0 版本开始，防沉迷初始化有两种方式，使用 TapBootstrap 模块 和 单独调用防沉迷接口初始化，游戏根据需要选择一种即可，
    //3.27.0 之前的版本只支持单独调用防沉迷接口初始化。
    //TapBootstrap 初始化（不推荐)
    public void TapInit_1()
    {
#if !UNITY_EDITOR
        var config = new TapConfig.Builder()
      .ClientID(clientId)
      .ClientToken(clientToken)
      .ServerURL(serverUrl)
      .RegionType(regionType);
        //.AntiAddictionConfig(showAntiAddictionSwitchAccount);

        TapBootstrap.Init(config.ConfigBuilder());

        // 适用于中国大陆
        TapLogin.Init(clientId);
        // 适用于其他国家或地区
        //TapLogin.Init(clientId, true, true);

#endif
        UnityEngine.Debug.Log("TapTap Start");
    }

    //防沉迷接口单独初始化（推荐）
    public void TapInit_2()
    {
        UnityEngine.Debug.Log("TapInit");
        AntiAddictionConfig config_2 = new AntiAddictionConfig()
        {
            gameId = clientId,      // TapTap 开发者中心对应 Client ID
            showSwitchAccount = false,      // 是否显示切换账号按钮
        };


        //初始化完成后，需要注册监听防沉迷消息的回调，示例如下：
        Action<int, string> callback = (code, errorMsg) =>
        {
            //实名认证回调
            // code == 500;   // 玩家未受到限制，正常进入游戏
            // code == 1000;  // 退出防沉迷认证及检查，当开发者调用 Exit 接口时或用户认证信息无效时触发，游戏应返回到登录页
            // code == 1001;  // 用户点击切换账号，游戏应返回到登录页
            // code == 1030;  // 用户当前时间无法进行游戏，此时用户只能退出游戏或切换账号
            // code == 1050;  // 用户无可玩时长，此时用户只能退出游戏或切换账号
            // code == 1100;  // 当前用户因触发应用设置的年龄限制无法进入游戏
            // code == 1200;  // 数据请求失败，游戏需检查当前设置的应用信息是否正确及判断当前网络连接是否正常
            // code == 9002;  // 实名过程中点击了关闭实名窗，游戏可重新开始防沉迷认证
            UnityEngine.Debug.LogFormat($"code: {code} error Message: {errorMsg}");

            if (this.AntiAddictionHandler != null)
            {
                this.AntiAddictionHandler(code, errorMsg);
            }
        };

        TapLogin.Init(clientId);

        //初始化防沉迷 UI 模块，包括设置启动防沉迷功能的配置、注册防沉迷的消息监听。
        AntiAddictionUIKit.Init(config_2, callback);

        // 如果是 PC 平台还需要额外设置一下 gameId
        TapTap.AntiAddiction.TapTapAntiAddictionManager.AntiAddictionConfig.gameId = clientId;


        AntiAddiction();
        //开发者可调用如下接口获取玩家所处年龄段：上例中的 ageRange 是一个整数，表示玩家所处年龄段的下限（最低年龄）。
        //int ageRange = AntiAddictionUIKit.AgeRange;

        //获取玩家当前剩余时长：
        //int remainingTimeInSeconds = AntiAddictionUIKit.RemainingTime;  
        //int remainingTimeInMinutes = AntiAddictionUIKit.RemainingTimeInMinutes;
    }

    /// <summary>
    /// taptap登录
    /// </summary>
    public async void TapTapLogin()
    {
        string tapAccount = string.Empty;
        var currentUser = TDSUser.GetCurrent();
        if (null == currentUser)
        {
            UnityEngine.Debug.Log("TapTap 当前未登录");
            // 开始登录
        }
        else
        {
            UnityEngine.Debug.Log("TapTap 已登录");
            // 进入游戏
        }
        try
        {
            //TapTapLogin333 success1:{
            //    "__type":"Pointer","className":"_User","objectId":"65f2d7f3a48919957f900840","createdAt":"2024-03-14T10:56:51.47Z","updatedAt":"2024-03-14T10:56:51.47Z","ACL":{ "*":{ "read":true},"65f2d7f3a48919957f900840":{ "write":true} },"authData":{
            //        "taptap":{
            //           "access_token":"1/D_BSEm8fSIJ4pLwciMXQBOpD1-ee3u7maifYHJVFbpb9QOJ5HN_w1a8uvuAJSxB_dO4y5f-nGxke2SZkaIwmXhiitqxxMT7J9YlbLK-GvrJ7HebMMvJOV3yL2d0AnOlvhXr_EK0k67gMtZrD2h9RzYMMRtt5os4rvFBoKPyzfV8sxrjalJtxCa5bqLOlg3MuNKixgwTLCpdNOVO3NVVw6dqcqSdm16VnQwGpVKsYdTCfjyc4m_xzuVeW16uEEXycVCgIOedzFVhhl6iDIVDCbPpiatHEs9Nb2CFzkMruFF0VoBbEn4J_-86JK07aokjpknqw4JME14HITSNBWfBNMQ","avatar":"https://img3.tapimg.com/default_avatars/1a6a73fbd7be6ac0f37a1033ac4a9251.jpg?imageMogr2/auto-orient/strip/thumbnail/!270x270r/gravity/Center/crop/270x270/format/jpg/interlace/1/quality/80","kid":"1/D_BSEm8fSIJ4pLwciMXQBOpD1-ee3u7maifYHJVFbpb9QOJ5HN_w1a8uvuAJSxB_dO4y5f-nGxke2SZkaIwmXhiitqxxMT7J9YlbLK-GvrJ7HebMMvJOV3yL2d0AnOlvhXr_EK0k67gMtZrD2h9RzYMMRtt5os4rvFBoKPyzfV8sxrjalJtxCa5bq
            //TapTapLogin444 success2: 65f2d7f3a48919957f900840

            UnityEngine.Debug.Log("TapTapLogin222");
            // 在 iOS、Android 系统下会唤起 TapTap 客户端或以 WebView 方式进行登录
            // 在 Windows、macOS 系统下显示二维码（默认）和跳转链接（需配置）
            TDSUser tdsUser = await TDSUser.LoginWithTapTap();
            UnityEngine.Debug.Log($"TapTapLogin333 success1:{tdsUser}");
            // 获取 TDSUser 属性
            var objectId = tdsUser.ObjectId;     // 用户唯一标识
            var nickname = tdsUser["nickname"];  // 昵称
            var avatar = tdsUser["avatar"];      // 头像
            UnityEngine.Debug.Log($"TapTapLogin444 success2:{objectId}");
            tapAccount = objectId;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("登录异常");
            if (e is TapException tapError)  // using TapTap.Common
            {
                UnityEngine.Debug.Log($"encounter exception:{tapError.code} message:{tapError.message}");
                if (tapError.code == (int)TapErrorCode.ERROR_CODE_BIND_CANCEL) // 取消登录
                {
                    UnityEngine.Debug.Log("登录取消");
                }
            }
        }
        TapLoingHandler?.Invoke(tapAccount);
    }

    public void  TapTapAuther()
    {
        AntiAddiction();
    }

    /// <summary>
    /// taptap实名认证
    /// </summary>
    public void AntiAddiction()
    {
        ///System.Guid.NewGuid();  系统方法生成唯一id
        // 注意唯一标识参数值长度不能超过 64 字符
        string userIdentifier = SystemInfo.deviceUniqueIdentifier;
        AntiAddictionUIKit.Startup(userIdentifier);
    }


    //新的初始化
    public void TapInit_3()
    {
        Start_3();
    }

    //新的登陆
    public void TapTapLogin_3()
    {
        OnTapLoginButtonClick();
    }

        /// <summary>
        /// 初始化 SDK 并判断本地是否已登录，已登录时开始合规认证检查，否则显示登录按钮
        /// </summary>
        async void Start_3()
    {
        // 初始化 SDK
        GameSDKManager.Instance.InitSDK();

        AccessToken currentToken = null;
        try
        {
            // 检查本地是否已存在 TapToken
            currentToken = await TapLogin.GetAccessToken();
        }
        catch (Exception e)
        {
            Debug.Log("本地无有效 token");
        }

        if (currentToken == null)
        {
            // TODO: 显示登录按钮
        }
        else
        {
            // 如果当前还未通过合规认证检查，开始认证
            if (!GameSDKManager.Instance.hasCheckedAntiAddiction)
            {
                // 开始合规认证检查
                StartAntiAddiction();
            }
        }
    }

    /// <summary>
    /// 登录按钮点击后执行 Tap 登录
    /// </summary>
    public async void OnTapLoginButtonClick()
    {
        try
        {
            // 发起 Tap 登录并获取用户信息
            var accessToken = await TapLogin.Login();

            // 开始合规认证检查
            StartAntiAddiction();
        }
        catch (Exception e)
        {
            // 登录取消或错误，提示用户重新登录
            Debug.Log("用户登录取消或错误");
        }
    }

    /// <summary>
    /// 开启合规认证检查
    /// </summary>
    public async void StartAntiAddiction()
    {
        // 获取当前已登录用户的 Profile 信息
        Profile profile = null;
        try
        {
            profile = await TapLogin.GetProfile();
        }
        catch (Exception exception)
        {
            Debug.Log($"获取 Profile 信息出现异常：{exception}");
        }
        if (profile == null)
        {
            // 无法获取 Profile 时，登出并显示登录按钮
            TapLogin.Logout();
            // TODO: 显示登录按钮
            return;
        }

        // 使用当前 Tap 用户的 unionid 作为用户标识进行合规认证检查
        string userIdentifier = profile.unionid;
        GameSDKManager.Instance.StartAntiAddiction(userIdentifier);
    }



// Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        AntiAddictionUIKit.Exit();
    }
}



