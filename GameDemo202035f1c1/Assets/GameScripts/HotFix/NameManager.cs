using Cysharp.Threading.Tasks;
using UnityEngine;

public class NameManager
{
    private static NameManager _instance;

    public static NameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new NameManager();
            return _instance;
        }
    }
    
    public int ranNameNum;
    public string[] randomName_xing = new string[0];
    public string[] randomName_name = new string[0];
    private bool _randomNameLoadingStarted;
    
    public void StartLoadRandomNameData()
    {
        if (_randomNameLoadingStarted)
        {
            return;
        }

        _randomNameLoadingStarted = true;
        LoadRandomNameData().Forget();
    }

    private async UniTask LoadRandomNameData()
    {
        var xingList = await LoadRandomNameList("RandName_Xing");
        if (xingList != null)
        {
            randomName_xing = xingList;
            ranNameNum += 1;
        }

        var nameList = await LoadRandomNameList("RandName_Name");
        if (nameList != null)
        {
            randomName_name = nameList;
            ranNameNum += 1;
        }
    }

    private async UniTask<string[]> LoadRandomNameList(string fileName)
    {
        var text =
            await ResourcesLoaderComponent.Instance.LoadAssetAsync<TextAsset>(ABPathHelper.GetTextPath(fileName));
        if (string.IsNullOrEmpty(text.text))
        {
            return null;
        }

        return NormalizeStreamingAssetText(text.text).Split('@');
    }

    private string NormalizeStreamingAssetText(string text)
    {
        return text.Replace("\r", string.Empty).Replace("\n", string.Empty);
    }
}