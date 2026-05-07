using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using I2.Loc;
using Substance.Game;
using UnityEngine;

public static class LanguageType
{
    public const string Chinese = "Chinese";
    public const string English = "English";
    public const string Japanese = "Japanese";
}

public class LanguageManager
{
    private static LanguageManager _instance;

    public static LanguageManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new LanguageManager();
            return _instance;
        }
    }

    // 多语言插件
    public LanguageSource LanguageSource;
    public LanguageSourceData LanguageSourceData => this.LanguageSource.SourceData;

    public List<string> AllLanguage = new List<string>();

    public bool UseRuntimeModule = false; //模拟平台运行时 编辑器资源不加载

    public string DefaultLanguage;

    public string CurrentLanguage;

    public void OnInitL2Localization()
    {
        DefaultLanguage = PlayerPrefs.GetString("Localization", LanguageType.Chinese);

        GameObject go = UnityEngine.Object.Instantiate(new GameObject());
        UnityEngine.Object.DontDestroyOnLoad(go);
        go.name = "[I2LocalizeMgr]";
        go.AddComponent<LanguageSource>();
        LanguageSource = go.GetComponent<LanguageSource>();

        if (Define.IsEditor)
        {
            if (!UseRuntimeModule)
            {
                LocalizationManager.RegisterSourceInEditor();
                UpdateAllLanguages();
                SetLanguage(DefaultLanguage);
            }
            else
            {
                LanguageSourceData.Awake();
                LoadLanguage(DefaultLanguage, true).Forget();
            }
        }
        else
        {
            LanguageSourceData.Awake();
            LoadLanguage(DefaultLanguage, true).Forget();
        }
    }

    private void UpdateAllLanguages()
    {
        AllLanguage.Clear();
        foreach (var language in LocalizationManager.GetAllLanguages())
        {
            var newLanguage = Regex.Replace(language, @"[\r\n]", "");
            AllLanguage.Add(newLanguage);
        }
    }

    public bool CheckLanguage(string language)
    {
        return AllLanguage.Contains(language);
    }

    //运行时注意 需要提前加载你需要的所有语言
    public bool SetLanguage(string language, bool load = false)
    {
        if (!CheckLanguage(language))
        {
            if (load)
            {
                LoadLanguage(language, true).Forget();
                return true;
            }

            Log.Error($"当前没有这个语言无法切换到此语言 {language}");
            return false;
        }

        if (CurrentLanguage == language)
        {
            return true;
        }

        Debug.Log($"设置当前语言 = {language}");
        LocalizationManager.CurrentLanguage = language;
        CurrentLanguage = language;
        return true;
    }

    //根据需求可提前加载语言
    public async UniTask LoadLanguage(string language, bool setCurrent = false)
    {
        if (Define.IsEditor)
        {
            if (!UseRuntimeModule)
            {
                Log.Error($"禁止在此模式下 动态加载语言 {language}");
                return;
            }
        }

        if (CheckLanguage(language))
        {
            Log.Error($"当前语言已存在 请勿重复加载 {language}");
            return;
        }

        var assetName = GetLanguageAssetName(language);

        var assetTextAsset = await ResourcesManager.Instance.LoadAssetAsync<TextAsset>(assetName);
        if (assetTextAsset == null)
        {
            Log.Error($"没有加载到目标语言资源 {language}");
            return;
        }

        Debug.Log($"加载语言成功 {language}");

        UseLocalizationCSV(assetTextAsset.text, !setCurrent);
        if (setCurrent)
        {
            SetLanguage(language);
        }

        //语言加载完毕后就可以释放资源了
        // YIUILoadHelper.Release(assetTextAsset);
    }

    private string GetLanguageAssetName(string language)
    {
        return $"Assets/Bundles/Text/{I2LocalizeHelper.I2ResAssetNamePrefix}{language}.csv";
    }

    private void UseLocalizationCSV(string text, bool isLocalizeAll = false)
    {
        LanguageSourceData.Import_CSV(string.Empty, text, eSpreadsheetUpdateMode.Replace, ',');
        if (isLocalizeAll)
        {
            LocalizationManager.LocalizeAll(); // 强制使用新数据本地化所有启用的标签/精灵
        }

        UpdateAllLanguages();
    }

    public string LoadLocalization(string getString)
    {
        var translation = LocalizationManager.GetTranslation(getString);
        return string.IsNullOrEmpty(translation) ? getString : translation;
    }

    public string LoadLocalizationHint(string getString)
    {
        return LoadLocalization(getString);
    }
}