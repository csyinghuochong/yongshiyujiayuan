using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

/// <summary>
/// 游戏资源加载入口。已加载资源由本类持有 YooAsset 句柄，调用释放接口时归还。
/// </summary>
public sealed class ResourcesManager
{
    private const string IconRoot = "Assets/Bundles/Icon/";
    private const string EffectRoot = "Assets/Bundles/Effect/";
    private const string UnitRoot = "Assets/Bundles/Unit/";
    private const string TextureRoot = "Assets/Bundles/Texture/";
    private const string AudioRoot = "Assets/Bundles/Audio/";
    private const string UguiRoot = "Assets/Bundles/UI/";

    private static readonly ResourcesManager instance = new ResourcesManager();
    private readonly Dictionary<string, AssetHandle> assetHandles = new Dictionary<string, AssetHandle>();
    private readonly HashSet<string> loadingAssets = new HashSet<string>();

    public static ResourcesManager Instance => instance;

    private ResourcesManager()
    {
    }

    public T LoadAssetSync<T>(string location) where T : UnityEngine.Object
    {
        if (!CheckLocation(location))
            return null;

        if (TryGetCachedAsset(location, out T cachedAsset))
            return cachedAsset;

        AssetHandle handle = YooAssets.LoadAssetSync<T>(location);
        return CacheLoadedAsset<T>(location, handle);
    }

    public async UniTask<T> LoadAssetAsync<T>(string location) where T : UnityEngine.Object
    {
        if (!CheckLocation(location))
            return null;

        if (TryGetCachedAsset(location, out T cachedAsset))
            return cachedAsset;

        // 同一资源正在加载时等待首个请求，避免创建重复句柄。
        if (loadingAssets.Contains(location))
        {
            await UniTask.WaitUntil(() => !loadingAssets.Contains(location));
            return TryGetCachedAsset(location, out cachedAsset) ? cachedAsset : null;
        }

        loadingAssets.Add(location);
        AssetHandle handle = null;
        try
        {
            handle = YooAssets.LoadAssetAsync<T>(location);
            await handle.Task;
            return CacheLoadedAsset<T>(location, handle);
        }
        catch (Exception exception)
        {
            handle?.Dispose();
            Debug.LogError($"加载资源失败：{location}\n{exception}");
            return null;
        }
        finally
        {
            loadingAssets.Remove(location);
        }
    }

    public async UniTask LoadSceneAsync(string location)
    {
        SceneHandle sceneHandle = LoadSceneAsyncHandle(location);
        if (sceneHandle == null)
            return;

        try
        {
            await sceneHandle.Task;
            if (sceneHandle.Status != EOperationStatus.Succeed)
            {
                Debug.LogError($"加载场景失败：{location}，{sceneHandle.LastError}");
                sceneHandle.Dispose();
            }
        }
        catch (Exception exception)
        {
            sceneHandle.Dispose();
            Debug.LogError($"加载场景失败：{location}\n{exception}");
        }
    }

    /// <summary>异步加载场景并返回句柄，供调用方读取加载进度。</summary>
    public SceneHandle LoadSceneAsyncHandle(string location)
    {
        if (!CheckLocation(location))
            return null;

        return YooAssets.LoadSceneAsync(location);
    }

    /// <summary>同步加载场景并返回句柄。</summary>
    public SceneHandle LoadSceneSync(string location)
    {
        if (!CheckLocation(location))
            return null;

        SceneHandle sceneHandle = YooAssets.LoadSceneSync(location);
        if (sceneHandle.Status != EOperationStatus.Succeed)
        {
            Debug.LogError($"加载场景失败：{location}，{sceneHandle.LastError}");
            sceneHandle.Dispose();
        }

        return sceneHandle;
    }

    /// <summary>释放指定资源的持有句柄。</summary>
    public bool ReleaseAsset(string location)
    {
        if (string.IsNullOrWhiteSpace(location) || !assetHandles.TryGetValue(location, out AssetHandle handle))
            return false;

        assetHandles.Remove(location);
        handle.Dispose();
        return true;
    }

    /// <summary>释放本管理器持有的全部资源句柄。</summary>
    public void ReleaseAllAssets()
    {
        foreach (AssetHandle handle in assetHandles.Values)
            handle.Dispose();
        assetHandles.Clear();
    }

    /// <summary>释放零引用资源包；应在释放资源句柄之后按需调用。</summary>
    public async UniTask UnloadUnusedAssetsAsync()
    {
        ResourcePackage package = YooAssets.GetPackage("DefaultPackage");
        if (package == null)
        {
            Debug.LogError("默认资源包尚未初始化。");
            return;
        }

        await package.UnloadUnusedAssetsAsync();
    }

    public T LoadIconSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>(CombineLocation(IconRoot, relativePath));
    }

    public UniTask<T> LoadIconAssetAsync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetAsync<T>(CombineLocation(IconRoot, relativePath));
    }

    public T LoadEffectSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>(CombineLocation(EffectRoot, relativePath));
    }

    public UniTask<T> LoadEffectAsync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetAsync<T>(CombineLocation(EffectRoot, relativePath));
    }

    public T LoadUnitSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>(CombineLocation(UnitRoot, relativePath));
    }

    public T LoadTextureSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>(CombineLocation(TextureRoot, relativePath));
    }

    public T LoadAudioSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>(CombineLocation(AudioRoot, relativePath));
    }

    public T LoadUGUISync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>(CombineLocation(UguiRoot, relativePath));
    }

    private static string CombineLocation(string root, string relativePath)
    {
        return root + (relativePath ?? string.Empty).TrimStart('/', '\\');
    }

    private static bool CheckLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            Debug.LogError("资源路径不能为空。");
            return false;
        }

        if (YooAssets.CheckLocationValid(location))
            return true;

        Debug.LogError($"资源路径无效：{location}");
        return false;
    }

    private bool TryGetCachedAsset<T>(string location, out T asset) where T : UnityEngine.Object
    {
        asset = null;
        if (!assetHandles.TryGetValue(location, out AssetHandle handle))
            return false;

        if (handle == null || !handle.IsValid || handle.AssetObject == null)
        {
            assetHandles.Remove(location);
            handle?.Dispose();
            return false;
        }

        asset = handle.AssetObject as T;
        if (asset == null)
            Debug.LogError($"资源类型不匹配：{location}，请求类型 {typeof(T).Name}，实际类型 {handle.AssetObject.GetType().Name}");
        return asset != null;
    }

    private T CacheLoadedAsset<T>(string location, AssetHandle handle) where T : UnityEngine.Object
    {
        if (handle == null || handle.Status != EOperationStatus.Succeed || handle.AssetObject == null)
        {
            string error = handle == null ? "未创建加载句柄" : handle.LastError;
            Debug.LogError($"加载资源失败：{location}，{error}");
            handle?.Dispose();
            return null;
        }

        T asset = handle.AssetObject as T;
        if (asset == null)
        {
            Debug.LogError($"资源类型不匹配：{location}，请求类型 {typeof(T).Name}，实际类型 {handle.AssetObject.GetType().Name}");
            handle.Dispose();
            return null;
        }

        if (assetHandles.TryGetValue(location, out AssetHandle oldHandle) && oldHandle != handle)
            oldHandle.Dispose();
        assetHandles[location] = handle;
        return asset;
    }
}
