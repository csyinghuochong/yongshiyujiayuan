using System;
using TapTap.Login;
using TapTap.AntiAddiction;
using TapTap.AntiAddiction.Model;
using UnityEngine;

/// <summary>
/// SDK 初始化及合规认证回调处理管理类
/// </summary>
public sealed class GameSDKManager
{
    // 游戏在 TapTap 开发者中心对应的 Client ID
    private readonly string clientId = TapTapSdkHelper.clientId;

    // 是否已初始化
    private readonly bool hasInit = false;

    // 是否已通过合规认证检查
    public bool hasCheckedAntiAddiction { get; private set; }

    private static readonly Lazy<GameSDKManager> lazy
        = new Lazy<GameSDKManager>(() => new GameSDKManager());

    public static GameSDKManager Instance { get { return lazy.Value; } }

    private GameSDKManager() { }

    // 声明合规认证回调
    private readonly Action<int, string> AntiAddictionCallback = (code, errorMsg) =>
    {
        // 根据回调返回的参数 code 添加不同情况的处理
        switch (code)
        {

            case 500: // 玩家未受限制，可正常进入
                Instance.hasCheckedAntiAddiction = true;
                // TODO: 显示开始游戏按钮      
                break;

            case 1000: // 防沉迷认证凭证无效时触发
            case 1001: // 当玩家触发时长限制时，点击了拦截窗口中「切换账号」按钮
            case 9002: // 实名认证过程中玩家关闭了实名窗口
                TapLogin.Logout(); // 如果游戏有其他账户系统，此时也应执行退出
                // TODO: 切换到登录页面 例如：SceneManager.LoadScene("Login");
                break;

            case 1100: // 当前用户因触发应用设置的年龄限制无法进入游戏
                // TODO: 游戏应自行绘制适龄限制提示，并引导玩家退出游戏
                break;

            case 1200: // 数据请求失败，应用信息错误或网络连接异常  
                // TODO: 引导玩家确认网络连接是否正常，并重新调用开始认证接口
                break;

            default:
                Debug.Log("其他可选回调");
                break;
        }

    };

    /// <summary>
    /// 初始化登录与合规认证 SDK 
    /// </summary>
    public void InitSDK()
    {
        if (!hasInit)
        {
            // 初始化 TapTap 登录
            TapLogin.Init(clientId);

            // 定义合规认证模块 config
            AntiAddictionConfig config = new AntiAddictionConfig()
            {
                gameId = clientId,           // TapTap 开发者中心对应 Client ID
                showSwitchAccount = true,    // 是否显示切换账号按钮
                useAgeRange = false          // 是否使用年龄段信息
            };

            // 初始化合规认证及设置回调
            AntiAddictionUIKit.Init(config);
            AntiAddictionUIKit.SetAntiAddictionCallback(AntiAddictionCallback);
        }
    }

    /// <summary>
    /// 开始合规认证检查
    /// </summary>
    /// <param name="userIdentifier">用户唯一标识</param>
    public void StartAntiAddiction(string userIdentifier)
    {
        hasCheckedAntiAddiction = false;
        AntiAddictionUIKit.StartupWithTapTap(userIdentifier);
    }
}