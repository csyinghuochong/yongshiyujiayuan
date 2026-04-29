using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class ResourcesLoaderComponent
{
    private static ResourcesLoaderComponent _instance;

    public static ResourcesLoaderComponent Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ResourcesLoaderComponent();
            return _instance;
        }
    }

    private ResourcePackage Package;

    public Dictionary<string, HandleBase> Handlers = new Dictionary<string, HandleBase>();

    public void Awake()
    {
        Package = YooAssets.GetPackage("DefaultPackage");
    }

    public T LoadAssetSync<T>(string location) where T : UnityEngine.Object
    {
        AssetHandle handle = YooAssets.LoadAssetSync<T>(location);

        return (T)(handle.AssetObject);
    }

    public Object LoadAssetSync(string location, System.Type type)
    {
        AssetHandle handle = YooAssets.LoadAssetSync(location, type);

        return handle.AssetObject;
    }

    public T LoadAssetSync<T>(params string[] locations) where T : UnityEngine.Object
    {
        foreach (string location in locations)
        {
            if (string.IsNullOrEmpty(location))
                continue;

            if (!YooAssets.CheckLocationValid(location))
                continue;

            AssetHandle handle = YooAssets.LoadAssetSync<T>(location);
            if (handle.AssetObject != null)
                return (T)handle.AssetObject;
        }
        
        Debug.LogError($"资源路径错误：{locations[0]}");

        return null;
    }

    public Object LoadAssetSync(string[] locations, System.Type type)
    {
        foreach (string location in locations)
        {
            if (string.IsNullOrEmpty(location))
                continue;

            if (!YooAssets.CheckLocationValid(location))
                continue;

            AssetHandle handle = YooAssets.LoadAssetSync(location, type);
            if (handle.AssetObject != null)
                return handle.AssetObject;
        }

        return null;
    }

    public T LoadIconSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>(BuildPaths("Assets/Bundles/Icon/" + relativePath, ".png", ".PNG", ".jpg", ".JPG"));
    }

    public T LoadEffectSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>(BuildPaths("Assets/Bundles/Effect/" + relativePath, GetExtensions(typeof(T))));
    }

    public T LoadUnitSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>(BuildPaths("Assets/Bundles/Unit/" + relativePath, GetExtensions(typeof(T))));
    }

    public T LoadTextureSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>(BuildPaths("Assets/Bundles/Texture/" + relativePath, GetExtensions(typeof(T))));
    }

    public T LoadAudioSync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>(BuildPaths("Assets/Bundles/Audio/" + relativePath, ".mp3", ".ogg", ".wav"));
    }

    public T LoadUGUISync<T>(string relativePath) where T : UnityEngine.Object
    {
        return LoadAssetSync<T>("Assets/Bundles/UI/" + relativePath + ".prefab");
    }

    public async UniTask<T> LoadAssetAsync<T>(string location) where T : UnityEngine.Object
    {
        AssetHandle handle = YooAssets.LoadAssetAsync<T>(location);
        await handle.Task;

        return (T)(handle.AssetObject);
    }

    public async UniTask LoadSceneAsync(string location)
    {
        SceneHandle sceneHandle = YooAssets.LoadSceneAsync(location);
        await sceneHandle.Task;
    }

    private static string[] BuildPaths(string rootPath, params string[] extensions)
    {
        if (extensions == null || extensions.Length == 0)
            return new[] { rootPath };

        string[] locations = new string[extensions.Length];
        for (int i = 0; i < extensions.Length; i++)
            locations[i] = rootPath + extensions[i];

        return locations;
    }

    private static string[] GetExtensions(System.Type type)
    {
        if (type == typeof(GameObject))
            return new[] { ".prefab" };

        if (type == typeof(Material))
            return new[] { ".mat" };

        if (type == typeof(RenderTexture))
            return new[] { ".renderTexture" };

        if (type == typeof(Texture) || type == typeof(Texture2D))
            return new[] { ".png", ".PNG", ".jpg", ".JPG", ".tga", ".TGA", ".psd", ".PSD", ".renderTexture" };

        if (type == typeof(AudioClip))
            return new[] { ".mp3", ".ogg", ".wav" };

        return new[] { ".prefab", ".png", ".PNG", ".jpg", ".JPG", ".mat", ".renderTexture", ".tga", ".TGA", ".psd", ".PSD" };
    }
}
