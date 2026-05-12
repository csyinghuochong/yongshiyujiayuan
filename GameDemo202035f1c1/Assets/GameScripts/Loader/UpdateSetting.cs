using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Hotfix/UpdateSetting", fileName = "UpdateSetting")]
public class UpdateSetting : ScriptableObject
{
    public bool Enable
    {
        get
        {
#if ENABLE_HYBRIDCLR
            return true;
#else
            return false;
#endif
        }
    }

    [Header("热更新服务器地址")]
    public string HostServerURL = "http://127.0.0.1";
    
    [Header("Auto sync with [HybridCLRGlobalSettings]")]
    public List<string> HotUpdateAssemblies = new List<string>() { "Game.Hotfix.dll" };

    [Header("Need manual setting!")] public List<string> AOTMetaAssemblies = new List<string>()
        { "mscorlib.dll", "System.dll", "System.Core.dll", "Game.Loader.dll", "UniTask.dll", "YooAsset.dll" };

    /// <summary>
    /// Dll of main business logic assembly
    /// </summary>
    public string LogicMainDllName = "Game.Hotfix.dll";

    /// <summary>
    /// 程序集文本资产打包Asset后缀名
    /// </summary>
    public string AssemblyTextAssetExtension = ".bytes";

    /// <summary>
    /// 程序集文本资产资源目录
    /// </summary>
    public string AssemblyTextAssetPath = "Bundles/Code";
}