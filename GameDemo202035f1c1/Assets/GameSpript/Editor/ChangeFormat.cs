using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
public class ChangeFormat : EditorWindow
{
    TextureImporterFormat format = TextureImporterFormat.AutomaticCompressed;
    //TextureImporterFormat format = TextureImporterFormat.AutomaticCrunched;
    int size = 512;
    string platform = "Android";
    bool autoSize = false;
    [MenuItem("Image/ChangeFormat")]
    static void SetReadWriteTrue()
    {
        ChangeFormat changeFormat = EditorWindow.GetWindow(typeof(ChangeFormat)) as ChangeFormat;
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("格式：");
        format = (TextureImporterFormat)EditorGUILayout.EnumPopup(format);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("贴图大小：");
        size = EditorGUILayout.IntField(size);
        GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();  
        //GUILayout.Label("使用贴图原始大小：");  
        //autoSize = EditorGUILayout.Toggle(autoSize);  
        //GUILayout.EndHorizontal();  

        GUILayout.BeginHorizontal();
        GUILayout.Label("平台：");
        platform = EditorGUILayout.TextField(platform);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("开始"))
        {
            Execute();
        }

    }


    void Execute()
    {
        Debug.LogWarning("开始");
        UnityEngine.Object[] selectedAsset = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        int currentSize = size;
        for (int i = 0; i < selectedAsset.Length; i++)
        {
            Debug.Log("i=" + i + selectedAsset[i].name);
            Texture2D tex = selectedAsset[i] as Texture2D;
            string path = AssetDatabase.GetAssetPath(tex);
            //TextureImporter ti = TextureImporter.GetAtPath(path) as TextureImporter;
            //TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
            TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(tex));

            //ti.textureFormat = changeTextureImporterFormat;

            //if (autoSize) {  
            //    currentSize = ti.maxTextureSize;  
            //}  
            ti.SetPlatformTextureSettings(platform, currentSize, format);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(tex));
        }
        Debug.LogWarning("结束");
    }
}
