using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TapSDK.Core;
using TapSDK.Compliance;
using TapSDK.Login;
using UnityEngine;

public class TapTapManager
{
    private static TapTapManager _instance;

    public static TapTapManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new TapTapManager();
            return _instance;
        }
    }

    private const string clientId = "yrvsirol93o27hydc7";
    private const string clientToken = "4lTeqdbgS3FtR5i4UliTIUaAvN1Ga4AtirdUeGTB";

    private bool hasInit = false;

    public void Init()
    {
        // 核心配置
        TapTapSdkOptions coreOptions = new TapTapSdkOptions
        {
            // 客户端 ID，开发者后台获取
            clientId = clientId,
            // 客户端令牌，开发者后台获取
            clientToken = clientToken,
            // 地区，CN 为国内，Overseas 为海外
            region = TapTapRegionType.CN,
            // 客户端 PC 平台公钥，开发者后台获取，仅接入 TapTap PC 客户端需要
            clientPublicKey = "pubKey",
            // 屏幕方向：0-竖屏 1-横屏，仅移动端生效
            screenOrientation = 1,
            // 是否开启日志，Release 版本请设置为 false
            enableLog = true
        };

        // 合规认证配置
        TapTapComplianceOption complianceOption = new TapTapComplianceOption
        {
            showSwitchAccount = true, // 是否显示切换账号按钮
            useAgeRange = false // 游戏是否需要获取真实年龄段信息
        };
        //数据分析相关配置
        TapTapEventOptions eventOptions = new TapTapEventOptions
        {
            // 渠道，如 AppStore、GooglePlay
            channel = "AppStore",
            // 初始化时传入的自定义参数，会在初始化时上报到 device_login 事件
            propertiesJson = "{\"device_login_custom_key\": \"这是初始化的时候传入的数据，会上报到 device_login 事件\"}",
            // 是否能够覆盖内置参数，默认为 false
            overrideBuiltInParameters = false,
            // 是否开启自动上报 IAP 事件
            enableAutoIAPEvent = true,
            // 是否禁用自动上报设备登录事件，默认为 false, 仅 Android 端生效
            disableAutoLogDeviceLogin = false,
            // CAID，仅国内 iOS
            caid = "000-000-0000-00000",
            // 是否开启广告商 ID 收集，默认为 false
            enableAdvertiserIDCollection = true,
            // OAID证书, 仅 Android，用于上报 OAID 仅 [TapTapRegion.CN] 生效
            oaidCert = "",
            // 是否禁用 OAID 反射，默认为 true
            disableReflectionOAID = true
        };
        // 当需要添加其他模块的初始化配置项，例如合规认证、成就等， 请使用如下 API
        TapTapSdkBaseOptions[] otherOptions = new TapTapSdkBaseOptions[]
        {
            // 其他模块配置项
            complianceOption,
            eventOptions
        };

        // TapSDK 初始化
        TapTapSDK.Init(coreOptions, otherOptions);

        // 合规认证回调
        TapTapCompliance.RegisterComplianceCallback(OnCompliance);

        hasInit = true;

        Debug.Log("TapTap Start");
    }

    // code	回调类型	触发逻辑
    // 500	LOGIN_SUCCESS	玩家未受到限制，正常进入游戏
    // 1000	EXITED	退出防沉迷认证及检查，当开发者调用 Exit 接口时或用户认证信息无效时触发，游戏应返回到登录页
    // 1001	SWITCH_ACCOUNT	用户点击切换账号，游戏应返回到登录页
    // 1030	PERIOD_RESTRICT	用户当前时间无法进行游戏，此时用户只能退出游戏或切换账号
    // 1050	DURATION_LIMIT	用户无可玩时长，此时用户只能退出游戏或切换账号
    // 1100	AGE_LIMIT	当前用户因触发应用设置的年龄限制无法进入游戏，该回调的优先级高于 1030，触发该回调无弹窗提示
    // 1200	INVALID_CLIENT_OR_NETWORK_ERROR	数据请求失败，游戏需检查当前设置的应用信息是否正确及判断当前网络连接是否正常
    // 9002	REAL_NAME_STOP	实名过程中点击了关闭实名窗，游戏可重新开始防沉迷认证
    private void OnCompliance(int code, string s)
    {
        if (code == 1050)
        {
            Debug.Log("用户无可玩时长，此时用户只能退出游戏或切换账号");
            return;
        }

        if (code != 500)
        {
            Debug.Log("实名认证失败");
            return;
        }

        //获取年龄
        TapTapManager.Instance.GetAgeRange().Forget();
        //获取剩余游戏时长
        TapTapManager.Instance.GetRemainingTime().Forget();
    }

    // 类型数值	含义
    // -1	未知
    // 0	0 到 7 岁
    // 8	8 到 15 岁
    // 16	16 到 17 岁
    // 18	成年玩家
    public async UniTask<int> GetAgeRange()
    {
        int ageRange = await TapTapCompliance.GetAgeRange();

        return ageRange;
    }

    /// <summary>
    /// 游戏剩余时长
    /// </summary>
    public async UniTask<int> GetRemainingTime()
    {
        int time = await TapTapCompliance.GetRemainingTime(); // 单位:秒

        return time;
    }

    /// <summary>
    /// TapTap实名认证
    /// </summary>
    public void Compliance(string userid)
    {
        if (!hasInit)
        {
            Init();
        }

        TapTapCompliance.Startup(userid);
    }

    # region 数据分析

    // 设置账号 ID
    public void SetUserID(string userId)
    {
        // 自定义属性
        var dict = new Dictionary<string, string>();
        // dict.Add(key, value);
        // dict.Add(key2, value2);
        string properties = dict.toJson();

        // 设置用户 ID 及账号登录事件属性
        TapTapEvent.SetUserID(userId, properties);
    }

    // 清除账号 ID
    public void ClearUser()
    {
        TapTapEvent.ClearUser();
    }

    /// <summary>
    /// 上报充值记录
    /// </summary>
    /// <param name="orderID">订单 ID</param>
    /// <param name="productName">产品名称</param>
    /// <param name="amount">充值金额（单位分，即无论什么币种，都需要乘以 100）</param>
    /// <param name="currencyType">货币类型，遵循 ISO 4217 标准。参考：人民币 CNY，美元 USD；欧元 EUR</param>
    /// <param name="paymentMethod">支付方式，如：支付宝</param>
    /// <param name="properties">充值（ charge ）的事件属性</param>
    public void LogPurchasedEvent(string orderID, string productName, long amount, string currencyType, string paymentMethod, string properties)
    {
        TapTapEvent.LogPurchasedEvent(
            orderID: orderID,
            productName: productName,
            amount: amount,
            currencyType: currencyType,
            paymentMethod: paymentMethod,
            properties: properties
        );
    }

    # endregion
}