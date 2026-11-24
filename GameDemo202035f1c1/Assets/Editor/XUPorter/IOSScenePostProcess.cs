#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

internal static class IOSScenePostProcess
{
    private const string DelegateName = "YongshiSceneDelegate";
    private const string DelegateFile = "Classes/" + DelegateName + ".mm";
    private const string SceneManifestKey = "UIApplicationSceneManifest";
    private const string SceneConfigurationsKey = "UISceneConfigurations";
    private const string ApplicationSceneRole = "UIWindowSceneSessionRoleApplication";
    private const string DelegateClassKey = "UISceneDelegateClassName";

    [PostProcessBuild(10001)]
    public static void OnPostProcessBuild(BuildTarget target, string exportPath)
    {
        if (target != BuildTarget.iOS)
            return;

        var templatePath = Path.Combine(Application.dataPath, "Editor/XUPorter", DelegateName + ".mm.txt");
        Configure(exportPath, templatePath);
    }

    internal static void Configure(string exportPath, string templatePath)
    {
        var plistPath = Path.Combine(exportPath, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        if (plist.root.values.TryGetValue(SceneManifestKey, out var manifestValue) &&
            manifestValue is PlistElementDict manifest &&
            manifest.values.TryGetValue(SceneConfigurationsKey, out var configurationsValue) &&
            configurationsValue is PlistElementDict configurations &&
            configurations.values.TryGetValue(ApplicationSceneRole, out var scenesValue) &&
            scenesValue is PlistElementArray scenes && scenes.values.Count > 0)
        {
            // Unity versions that export a scene delegate already own their window lifecycle.
            if (!(scenes.values[0] is PlistElementDict scene) ||
                !scene.values.TryGetValue(DelegateClassKey, out var delegateValue) ||
                !(delegateValue is PlistElementString delegateClass) || delegateClass.value != DelegateName)
                return;
        }
        else
        {
            var sceneManifest = plist.root.CreateDict(SceneManifestKey);
            sceneManifest.SetBoolean("UIApplicationSupportsMultipleScenes", false);
            var scene = sceneManifest.CreateDict(SceneConfigurationsKey).CreateArray(ApplicationSceneRole).AddDict();
            scene.SetString("UISceneClassName", "UIWindowScene");
            scene.SetString("UISceneConfigurationName", "Unity Game");
            scene.SetString(DelegateClassKey, DelegateName);
        }

        var projectPath = PBXProject.GetPBXProjectPath(exportPath);
        var project = new PBXProject();
        project.ReadFromFile(projectPath);
        File.Copy(templatePath, Path.Combine(exportPath, DelegateFile), true);

        var fileGuid = project.FindFileGuidByProjectPath(DelegateFile);
        if (string.IsNullOrEmpty(fileGuid))
            fileGuid = project.AddFile(DelegateFile, DelegateFile, PBXSourceTree.Source);

        project.AddFileToBuild(project.GetUnityFrameworkTargetGuid(), fileGuid);
        project.WriteToFile(projectPath);
        plist.WriteToFile(plistPath);
    }
}
#endif
