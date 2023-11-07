using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor.Build.Reporting;

public class MyEditorScript
{
	static string[] SCENES = FindEnabledEditorScenes();
	[MenuItem("Custom/Build Android QQ")]
	static void PerformAndroidQQBuild()
	{
		BulidTarget("QQ", "Android");
	}

	[MenuItem("Custom/Build Android Share")]
	static void PerformAndroidShareBuild()
	{
		BulidTarget("Share", "Android");
	}

	[MenuItem("Custom/Build Android TapTap")]
	static void PerformAndroidTapTapBuild()
	{
		BulidTarget("TapTap", "Android");
	}

	[MenuItem("Custom/Build Android MuBao")]
	static void PerformAndroidHuaWeiBuild()
	{
		BulidTarget("MuBao", "Android");
	}

	[MenuItem("Custom/Build Android ALL")]
	static void PerformAndroidALLBuild()
	{
		BulidTarget("QQ", "Android");
		BulidTarget("Share", "Android");
		BulidTarget("TapTap", "Android");
		//BulidTarget("MuBao", "Android");
	}
	//[MenuItem("Custom/Build iPhone QQ")]
	static void PerformiPhoneQQBuild()
	{
		BulidTarget("QQ", "IOS");
	}

    private static string formPath = @"E:\WeijingGame_2\Android\";//原路径   [guanfang\mubao]

	private static string targetPath = Application.dataPath + @"\Plugins\Android\libs_alipay"; //目标路径   ../表示当前项目文件的父路径
	private static string mainfestFile = Application.dataPath + @"\Plugins\Android\AndroidManifest"; //目标路径   ../表示当前项目文件的父路径

	private static bool isNull = false;


	private static void CopyLibs(string path)
	{
		isNull = false;

		CopyDirectory(formPath + path, Application.dataPath + @"\Plugins\Android");
		if (!isNull)
		{
			Debug.Log("目录文件导入成功！！");
		}
	}

	/// <summary>
	/// 拷贝文件
	/// </summary>
	/// <param name="srcDir">起始文件夹</param>
	/// <param name="tgtDir">目标文件夹</param>
	public static void CopyDirectory(string srcDir, string tgtDir)
	{
		DirectoryInfo source = new DirectoryInfo(srcDir);
		DirectoryInfo target = new DirectoryInfo(tgtDir);

		if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
		{
			throw new Exception("父目录不能拷贝到子目录！");
		}

		if (!source.Exists)
		{
			return;
		}

		if (!target.Exists)
		{
			target.Create();
		}

		FileInfo[] files = source.GetFiles();
		DirectoryInfo[] dirs = source.GetDirectories();
		if (files.Length == 0 && dirs.Length == 0)
		{
			isNull = true;
			return;
		}
		for (int i = 0; i < files.Length; i++)
		{
			File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
		}
		for (int j = 0; j < dirs.Length; j++)
		{
			CopyDirectory(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
		}
	}

	//删除目标文件夹下面所有文件
	public static void CleanDirectory(string dir)
	{
		foreach (string subdir in Directory.GetDirectories(dir))
		{
			Directory.Delete(subdir, true);
		}

		foreach (string subFile in Directory.GetFiles(dir))
		{
			File.Delete(subFile);
		}
	}

	static void BulidTarget(string name, string target)
	{

		if (Directory.Exists(targetPath))
		{
			CleanDirectory(targetPath);
		}
		if (File.Exists(mainfestFile))
		{
			File.Delete(mainfestFile);
		}

		//渠道包不需要阿里支付插件
		if (name == "MuBao")
		{
			CopyLibs("mubao");
		}
		else
		{
			CopyLibs("guanfang");
		}

		string app_name = "WeiJing_" + name;
		string target_dir = Application.dataPath + "/TargetAndroid";
		string target_name = app_name + ".apk";
		BuildTargetGroup targetGroup = BuildTargetGroup.Android;
		BuildTarget buildTarget = BuildTarget.Android;
		string applicationPath = Application.dataPath.Replace("/Assets", "");

		if (target == "Android")
		{
			target_dir = applicationPath + "/TargetAndroid";
			target_name = app_name + ".apk";
			targetGroup = BuildTargetGroup.Android;
		}
		if (target == "IOS")
		{
			target_dir = applicationPath + "/TargetIOS";
			target_name = app_name;
			targetGroup = BuildTargetGroup.iOS;
			buildTarget = BuildTarget.iOS;
		}


		if (Directory.Exists(target_dir))
		{
			if (File.Exists(target_name))
			{
				File.Delete(target_name);
			}
		}
		else
		{
			Directory.CreateDirectory(target_dir);
		}

		PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, "SUBSTANCE_PLUGIN_ENABLED;" + name);

		//PlayerSettings.applicationIdentifier = "com.game.uc";
		//PlayerSettings.bundleVersion = "v0.0.1";
		//PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, "SUBSTANCE_PLUGIN_ENABLED;UC");
		//==================�����ǱȽ���Ҫ�Ķ���=======================

		GenericBuild(SCENES, target_dir + "/" + target_name, buildTarget, targetGroup, BuildOptions.None);

	}

	private static string[] FindEnabledEditorScenes()
	{
		List<string> EditorScenes = new List<string>();
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		{
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}

	static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildTargetGroup target_grooup, BuildOptions build_options)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(target_grooup, build_target);
		BuildReport br = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
		if (br.summary.result == BuildResult.Failed)
		{
			throw new Exception("BuildPlayer failure: " + br);
		}
	}

}