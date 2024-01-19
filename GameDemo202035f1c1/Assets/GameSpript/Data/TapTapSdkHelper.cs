using System;
using TapTap.Bootstrap;
using TapTap.Common;
using TapTap.Login;
using UnityEngine;
using TapTap.AntiAddiction;
using TapTap.AntiAddiction.Model;


public class TapTapSdkHelper : MonoBehaviour
{
    private string clientId = "yrvsirol93o27hydc7";
    private string clientToken = "4lTeqdbgS3FtR5i4UliTIUaAvN1Ga4AtirdUeGTB";
    private string serverUrl = "https://yrvsirol.cloud.tds1.tapapis.cn";
    private RegionType regionType = RegionType.CN;
    private bool showAntiAddictionSwitchAccount; //showAntiAddictionSwitchAccount 参数为 bool 类型，表示是否显示切换账号

    // Start is called before the first frame update
    void Start()
    {
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

        AntiAddictionConfig config_2 = new AntiAddictionConfig()
        {
            gameId = clientId,      // TapTap 开发者中心对应 Client ID
            showSwitchAccount = false,      // 是否显示切换账号按钮
        };

        Action<int, string> callback = (code, errorMsg) => {
            // code == 500;   // 登录成功
            // code == 1000;  // 用户登出
            // code == 1001;  // 切换账号
            // code == 1030;  // 用户当前无法进行游戏
            // code == 1050;  // 时长限制
            // code == 9002;  // 实名过程中点击了关闭实名窗
            //UnityEngine.Debug.LogFormat($"code: {code} error Message: {errorMsg}");
            this.AntiAddictionHandler( code, errorMsg );
        };

        AntiAddictionUIKit.Init(config_2, callback);
        // 如果是 PC 平台还需要额外设置一下 gameId
        TapTap.AntiAddiction.TapTapAntiAddictionManager.AntiAddictionConfig.gameId = clientId;


        ///System.Guid.NewGuid();  系统方法生成唯一id
        // 注意唯一标识参数值长度不能超过 64 字符
        string userIdentifier = SystemInfo.deviceUniqueIdentifier;
        AntiAddictionUIKit.Startup(userIdentifier);
    }

    private void AntiAddictionHandler(int code, string errorMsg)
    {
        // code == 500;   // 登录成功
        // code == 1000;  // 用户登出
        // code == 1001;  // 切换账号
        // code == 1030;  // 用户当前无法进行游戏
        // code == 1050;  // 时长限制
        // code == 9002;  // 实名过程中点击了关闭实名窗

        UnityEngine.Debug.LogFormat($"code: {code} error Message: {errorMsg}");
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
