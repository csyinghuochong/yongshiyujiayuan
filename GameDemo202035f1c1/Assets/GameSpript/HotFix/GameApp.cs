using UnityEngine;
using YooAsset;

public class GameApp
{
    /// <summary>
    /// 热更域App主入口。
    /// </summary>
    /// <param name="objects"></param>
    public static void Entrance(object[] objects)
    {
        Debug.Log("热更程序集完成 开始游戏！！！！！！！！！！");
        YooAssets.LoadSceneAsync("Assets/Bundles/Scenes/StartGame.unity");
    }
}