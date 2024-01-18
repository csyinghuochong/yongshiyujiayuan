using System;
//using TapTap.Bootstrap;
using TapTap.Common;
using UnityEngine;
using TapTap.AntiAddiction;
using TapTap.AntiAddiction.Model;

public class TapTapSdkHelper : MonoBehaviour
{

    private string clientId = "";
    private string clientToken = "";
    private string serverUrl = "";
    private RegionType regionType;
    private bool showAntiAddictionSwitchAccount;

    // Start is called before the first frame update
    void Start()
    {
        var config =  new TapConfig.Builder()
        .ClientID(clientId)
        .ClientToken(clientToken)
        .ServerURL(serverUrl)
        .RegionType(regionType)
        // showAntiAddictionSwitchAccount 参数为 bool 类型，表示是否显示切换账号   
        .AntiAddictionConfig(showAntiAddictionSwitchAccount);

        //TapBootstrap.Init(config);

        Action<int, string> callback = (code, errorMsg) => {
            // code == 500;   // 登录成功
            // code == 1000;  // 用户登出
            // code == 1001;  // 切换账号
            // code == 1030;  // 用户当前无法进行游戏
            // code == 1050;  // 时长限制
            // code == 9002;  // 实名过程中点击了关闭实名窗
            UnityEngine.Debug.LogFormat($"code: {code} error Message: {errorMsg}");
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
