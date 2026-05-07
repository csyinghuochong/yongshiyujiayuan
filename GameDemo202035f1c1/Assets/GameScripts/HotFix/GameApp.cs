using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameApp
{
    /// <summary>
    /// 热更域App主入口。
    /// </summary>
    /// <param name="objects"></param>
    public static void Entrance(object[] objects)
    {
        Debug.Log("热更程序集完成 开始游戏！！！！！！！！！！");

        LanguageManager.Instance.OnInitL2Localization();
        ResourcesLoaderComponent.Instance.LoadSceneAsync(ABPathHelper.GetScenePath("StartGame")).Forget();
    }
}