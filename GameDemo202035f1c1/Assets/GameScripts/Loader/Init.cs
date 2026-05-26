using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Substance.Game;
#if ENABLE_HYBRIDCLR
using HybridCLR;
#endif
using UnityEngine;
using UniFramework.Event;
using YooAsset;

public class Init : MonoBehaviour
{
    public static Init Instance { get; private set; }

    private UpdateSetting updateSetting;

    public static UpdateSetting UpdateSetting
    {
        get
        {
#if UNITY_EDITOR
            if (Instance == null)
            {
                string[] guids = UnityEditor.AssetDatabase.FindAssets("t:UpdateSetting");
                if (guids.Length >= 1)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                    return UnityEditor.AssetDatabase.LoadAssetAtPath<UpdateSetting>(path);
                }
            }
#endif

            return Instance.updateSetting;
        }
    }

    /// <summary>
    /// 资源系统运行模式
    /// </summary>
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        updateSetting = Resources.Load<UpdateSetting>("UpdateSetting");

        Debug.Log($"资源系统运行模式：{PlayMode}");
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);
        
        SetUIPatch(false);
    }

    void Start()
    {
        // 游戏管理器
        GameManager.Instance.Behaviour = this;

        // 初始化事件系统
        UniEvent.Initalize();

        // 初始化资源系统
        YooAssets.Initialize();

        // 加载更新页面
        SetUIPatch(true);

        StartCoroutine(StartPatch());
    }
    
    private void Update()
    {
        CheckLoadAssembly();
    }

    public void SetUIPatch(bool enable)
    {
        transform.Find("UI/UIPatch")?.gameObject.SetActive(enable);
    }

    public IEnumerator StartPatch()
    {
        // 开始补丁更新流程
        var operation = new PatchOperation("DefaultPackage", PlayMode);
        YooAssets.StartOperation(operation);
        yield return operation;

        // 设置默认的资源包
        var gamePackage = YooAssets.GetPackage("DefaultPackage");
        YooAssets.SetDefaultPackage(gamePackage);

        // 切换到主页面场景
        // PatchEventDefine.LoadAssembly.SendEventMessage();
        LoadAssembly().Forget();
    }

    # region 热更程序集

    private bool _enableAddressable = false;
    private int _loadAssetCount;
    private int _loadMetadataAssetCount;
    private int _failureAssetCount;
    private int _failureMetadataAssetCount;
    private bool _startLoadAssembly;
    private bool _loadAssemblyComplete;
    private bool _loadMetadataAssemblyComplete;
    private bool _loadAssemblyWait;
    private bool _loadMetadataAssemblyWait;
    private Assembly _mainLogicAssembly;
    private List<Assembly> _hotfixAssemblyList;

    private async UniTaskVoid LoadAssembly()
    {
        _startLoadAssembly = true;
        _loadAssemblyComplete = false;
        _hotfixAssemblyList = new List<Assembly>();

        //AOT Assembly加载原始metadata
        if (UpdateSetting.Enable)
        {
#if !UNITY_EDITOR
            _loadMetadataAssemblyComplete = false;
            LoadMetadataForAOTAssembly();
#else
            _loadMetadataAssemblyComplete = true;
#endif
        }
        else
        {
            _loadMetadataAssemblyComplete = true;
        }

        if (!UpdateSetting.Enable || PlayMode == EPlayMode.EditorSimulateMode)
        {
            _mainLogicAssembly = GetMainLogicAssembly();
        }
        else
        {
            if (UpdateSetting.Enable)
            {
                foreach (string hotUpdateDllName in UpdateSetting.HotUpdateAssemblies)
                {
                    var assetLocation = hotUpdateDllName;
                    if (!_enableAddressable)
                    {
                        assetLocation = GetRegularPath(Path.Combine("Assets", UpdateSetting.AssemblyTextAssetPath, $"{hotUpdateDllName}{UpdateSetting.AssemblyTextAssetExtension}"));
                    }

                    Debug.Log($"LoadAsset: [ {assetLocation} ]");
                    _loadAssetCount++;

                    AssetHandle handle = YooAssets.LoadAssetAsync<TextAsset>(assetLocation);
                    await handle.Task;
                    TextAsset result = (TextAsset)handle.AssetObject;

                    LoadAssetSuccess(result);
                }

                _loadAssemblyWait = true;
            }
            else
            {
                _mainLogicAssembly = GetMainLogicAssembly();
            }
        }

        if (_loadAssetCount == 0)
        {
            _loadAssemblyComplete = true;
        }
    }

    private void CheckLoadAssembly()
    {
        if (!_startLoadAssembly)
        {
            return;
        }
        
        if (!_loadAssemblyComplete)
        {
            return;
        }

        if (!_loadMetadataAssemblyComplete)
        {
            return;
        }

        _startLoadAssembly = false;
        
        AllAssemblyLoadComplete();
    }

    private void AllAssemblyLoadComplete()
    {
#if UNITY_EDITOR
        _mainLogicAssembly = GetMainLogicAssembly();
#endif
        if (_mainLogicAssembly == null)
        {
            Debug.Log($"Main logic assembly missing. Please check \'ENABLE_HYBRIDCLR\' is defined in Player Settings And check the file of {UpdateSetting.LogicMainDllName}.bytes is exits.");
            return;
        }

        var appType = _mainLogicAssembly.GetType("GameApp");
        if (appType == null)
        {
            Debug.Log($"Main logic type 'GameMain' missing.");
            return;
        }

        var entryMethod = appType.GetMethod("Entrance");
        if (entryMethod == null)
        {
            Debug.Log($"Main logic entry method 'Entrance' missing.");
            return;
        }

        object[] objects = new object[] { new object[] { _hotfixAssemblyList } };
        entryMethod.Invoke(appType, objects);
    }

    private Assembly GetMainLogicAssembly()
    {
        _hotfixAssemblyList.Clear();
        Assembly mainLogicAssembly = null;
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (string.Compare(UpdateSetting.LogicMainDllName, $"{assembly.GetName().Name}.dll", StringComparison.Ordinal) == 0)
            {
                mainLogicAssembly = assembly;
            }

            foreach (var hotUpdateDllName in UpdateSetting.HotUpdateAssemblies)
            {
                if (hotUpdateDllName == $"{assembly.GetName().Name}.dll")
                {
                    _hotfixAssemblyList.Add(assembly);
                }
            }

            if (mainLogicAssembly != null && _hotfixAssemblyList.Count == UpdateSetting.HotUpdateAssemblies.Count)
            {
                break;
            }
        }

        return mainLogicAssembly;
    }

    /// <summary>
    /// 加载代码资源成功回调。
    /// </summary>
    /// <param name="textAsset">代码资产。</param>
    private void LoadAssetSuccess(TextAsset textAsset)
    {
        _loadAssetCount--;
        if (textAsset == null)
        {
            Log.Warning($"Load Assembly failed.");
            return;
        }

        var assetName = textAsset.name;
        Debug.Log($"LoadAssetSuccess, assetName: [ {assetName} ]");

        try
        {
            var assembly = Assembly.Load(textAsset.bytes);
            if (string.Compare(UpdateSetting.LogicMainDllName, assetName, StringComparison.Ordinal) == 0)
            {
                _mainLogicAssembly = assembly;
            }

            _hotfixAssemblyList.Add(assembly);
            Debug.Log($"Assembly [ {assembly.GetName().Name} ] loaded");
        }
        catch (Exception e)
        {
            _failureAssetCount++;
            Debug.Log(e);
            throw;
        }
        finally
        {
            _loadAssemblyComplete = _loadAssemblyWait && 0 == _loadAssetCount;
        }
    }

    /// <summary>
    /// 为Aot Assembly加载原始metadata， 这个代码放Aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行。
    /// </summary>
    public void LoadMetadataForAOTAssembly()
    {
        // 可以加载任意aot assembly的对应的dll。但要求dll必须与unity build过程中生成的裁剪后的dll一致，而不能直接使用原始dll。
        // 我们在BuildProcessor_xxx里添加了处理代码，这些裁剪后的dll在打包时自动被复制到 {项目目录}/HybridCLRData/AssembliesPostIl2CppStrip/{Target} 目录。

        // 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        // 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        if (UpdateSetting.AOTMetaAssemblies.Count == 0)
        {
            _loadMetadataAssemblyComplete = true;
            return;
        }

        foreach (string aotDllName in UpdateSetting.AOTMetaAssemblies)
        {
            var assetLocation = aotDllName;
            if (!_enableAddressable)
            {
                assetLocation = GetRegularPath(Path.Combine("Assets", UpdateSetting.AssemblyTextAssetPath,
                    $"{aotDllName}{UpdateSetting.AssemblyTextAssetExtension}"));
            }


            Debug.Log($"LoadMetadataAsset: [ {assetLocation} ]");
            _loadMetadataAssetCount++;

            AssetHandle handle = YooAssets.LoadAssetAsync<TextAsset>(assetLocation);
            handle.Completed += assetHandle => LoadMetadataAssetSuccess((TextAsset)assetHandle.AssetObject);
        }

        _loadMetadataAssemblyWait = true;
    }

    /// <summary>
    /// 加载元数据资源成功回调。
    /// </summary>
    /// <param name="textAsset">代码资产。</param>
    private void LoadMetadataAssetSuccess(TextAsset textAsset)
    {
        _loadMetadataAssetCount--;
        if (null == textAsset)
        {
            Debug.Log($"LoadMetadataAssetSuccess:Load Metadata failed.");
            return;
        }

        string assetName = textAsset.name;
        Debug.Log($"LoadMetadataAssetSuccess, assetName: [ {assetName} ]");
        try
        {
            byte[] dllBytes = textAsset.bytes;
#if ENABLE_HYBRIDCLR
            // 加载assembly对应的dll，会自动为它hook。一旦Aot泛型函数的native函数不存在，用解释器版本代码
            HomologousImageMode mode = HomologousImageMode.SuperSet;
            LoadImageErrorCode err = (LoadImageErrorCode)HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{assetName}. mode:{mode} ret:{err}");
#endif
        }
        catch (Exception e)
        {
            _failureMetadataAssetCount++;
            Debug.Log(e.Message);
            throw;
        }
        finally
        {
            _loadMetadataAssemblyComplete = _loadMetadataAssemblyWait && 0 == _loadMetadataAssetCount;
        }
    }

    /// <summary>
    /// 获取规范的路径。
    /// </summary>
    /// <param name="path">要规范的路径。</param>
    /// <returns>规范的路径。</returns>
    private static string GetRegularPath(string path)
    {
        if (path == null)
        {
            return null;
        }

        return path.Replace('\\', '/');
    }

    #endregion
}