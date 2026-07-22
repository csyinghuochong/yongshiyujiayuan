using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class ResourcesManager
{
    private static ResourcesManager _instance;

    public static ResourcesManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ResourcesManager();
            return _instance;
        }
    }

    private ResourcePackage Package;

    public Dictionary<string, AssetHandle> Handlers = new();

    public void Awake()
    {
        Package = YooAssets.GetPackage("DefaultPackage");
    }

    public T LoadAssetSync<T>(string location) where T : UnityEngine.Object
    {
        if (!YooAssets.CheckLocationValid(location))
        {
            Debug.LogError($"资源路径错误：{location}");
            return null;
        }

        AssetHandle handle;
        if (!Handlers.TryGetValue(location, out AssetHandle selfHandler))
        {
            handle = YooAssets.LoadAssetSync<T>(location);
            Handlers.Add(location, handle);
        }
        else
        {
            handle = selfHandler;
        }

        return (T)handle.AssetObject;
    }

    public async UniTask<T> LoadAssetAsync<T>(string location) where T : UnityEngine.Object
    {
        if (!YooAssets.CheckLocationValid(location))
        {
            Debug.LogError($"资源路径错误：{location}");
            return null;
        }

        AssetHandle handle;
        if (!Handlers.TryGetValue(location, out AssetHandle selfHandler))
        {
            handle = YooAssets.LoadAssetAsync<T>(location);
            Handlers.Add(location, handle);
        }
        else
        {
            handle = selfHandler;
        }

        await handle.Task;

        return (T)handle.AssetObject;
    }

    public async UniTask LoadSceneAsync(string location)
    {
        SceneHandle sceneHandle = YooAssets.LoadSceneAsync(location);
        await sceneHandle.Task;
    }


    public T LoadIconSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>("Assets/Bundles/Icon/" + relativePath);
    }

    public T LoadEffectSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>("Assets/Bundles/Effect/" + relativePath);
    }

    public T LoadUnitSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>("Assets/Bundles/Unit/" + relativePath);
    }

    public T LoadTextureSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>("Assets/Bundles/Texture/" + relativePath);
    }

    public T LoadAudioSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>("Assets/Bundles/Audio/" + relativePath);
    }

    public T LoadUGUISync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>("Assets/Bundles/UI/" + relativePath);
    }
}