using System;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public static class ABAtlasTypes
{
    public const string ItemIcon = "ItemIcon";
    public const string ItemQualityIcon = "ItemQualityIcon";
    public const string HeroIcon = "HeroIcon";
    public const string SkillIcon = "SkillIcon";
    public const string MonsterIcon = "MonsterIcon";
    public const string TaskIcon = "TaskIcon";
    public const string OtherIcon = "OtherIcon";
}

public static class ABUnitType
{
    public const string Hero = "Hero";
    public const string Monster = "Monster";
    public const string DropItem = "DropItem";
    public const string NPC = "NPC";
}

public static class ABPathHelper
{
    public static string GetAnimFbxPath(string fileName)
    {
        return string.Format("Assets/Bundles/Animation/{0}.fbx", fileName);
    }

    public static string GetMaterialPath(string fileName)
    {
        return string.Format("Assets/Bundles/Material/{0}.mat", fileName);
    }

    public static string GetTexturePath(string fileName)
    {
        return string.Format("Assets/Bundles/Altas/{0}.prefab", fileName);
    }

    public static string GetUGUIPath(string name)
    {
        return string.Format("Assets/Bundles/UI/{0}.prefab", name);
    }

    public static string GetConfigPath(string fileName)
    {
        return string.Format("Assets/Bundles/Config/{0}.bytes", fileName);
    }

    public static string GetMapConfigPath(string fileName)
    {
        return string.Format("Assets/Bundles/MapConfig/{0}.bytes", fileName);
    }

    public static string GetNormalConfigPath(string fileName)
    {
        return string.Format("Assets/Bundles/Independent/{0}.prefab", fileName);
    }

    public static string GetAudioPath(string fileName)
    {
        return string.Format("Assets/Bundles/Audio/{0}.mp3", fileName);
    }

    public static string GetAudioOggPath(string fileName)
    {
        return string.Format("Assets/Bundles/Audio/{0}.ogg", fileName);
    }

    public static string GetSoundPath(string fileName)
    {
        return string.Format("Assets/Bundles/Sound/{0}.prefab", fileName);
    }

    public static string GetUnitPath(string path, string fileName)
    {
        return string.Format("Assets/Bundles/Unit/{0}/{1}.prefab", path, fileName);
    }

    public static string GetUnitPath(string relativePath)
    {
        return string.Format("Assets/Bundles/Unit/{0}.prefab", relativePath);
    }

    public static string GetUIUnitPath(string path, string fileName)
    {
        return string.Format("Assets/Bundles/UI/Spine/{0}/{1}.prefab", path, fileName);
    }

    public static string GetSceneUnitPath(string fileName)
    {
        return string.Format("Assets/Bundles/Unit/Scene/{0}.prefab", fileName);
    }

    public static string GetItemPath(string fileName)
    {
        return string.Format("Assets/Bundles/Unit/ItemModel/{0}.prefab", fileName);
    }

    public static string GetScenePath(string fileName)
    {
        return string.Format("Assets/Bundles/Scenes/{0}.unity", fileName);
    }

    public static string GetEffectPath(string fileName)
    {
        return string.Format("Assets/Bundles/Effect/{0}.prefab", fileName);
    }

    public static string GetSkillEffectPath(string fileName)
    {
        return string.Format("Assets/Bundles/Effect/SkillEffect/{0}.prefab", fileName);
    }

    public static string GetSkillIndicatorPath(string fileName)
    {
        return string.Format("Assets/Bundles/Effect/SkillIndicator/{0}.prefab", fileName);
    }

    public static string GetSkillHitEffectPath(string fileName)
    {
        return string.Format("Assets/Bundles/Effect/SkillHitEffect/{0}.prefab", fileName);
    }

    public static string GetIconPath(string fileName)
    {
        return string.Format("Assets/Bundles/Icon/{0}.png", fileName);
    }

    public static string GetTextureBundlePath(string relativePath, string extension)
    {
        return string.Format("Assets/Bundles/Texture/{0}{1}", relativePath, extension);
    }

    public static string GetAudioBundlePath(string relativePath, string extension)
    {
        return string.Format("Assets/Bundles/Audio/{0}{1}", relativePath, extension);
    }

    public static string GetUnitAssetPath(string relativePath, string extension)
    {
        return string.Format("Assets/Bundles/Unit/{0}{1}", relativePath, extension);
    }

    public static string GetEffectAssetPath(string relativePath, string extension)
    {
        return string.Format("Assets/Bundles/Effect/{0}{1}", relativePath, extension);
    }

    public static string GetAtlasPath_2(string path, string name)
    {
        return string.Format("Assets/Bundles/Icon/{0}/{1}.png", path, name);
    }

    public static string GetAtlasPath(string path)
    {
        return string.Format("Assets/Bundles/Atlas/{0}.prefab", path);
    }

    public static string GetJpgPath(string path)
    {
        return string.Format("Assets/Bundles/Jpg/{0}.jpg", path);
    }

    public static string GetTextPath(string text)
    {
        return string.Format("Assets/Bundles/Text/{0}.txt", text);
    }

    public static string GetRecastPath(string text)
    {
        return string.Format("Assets/Bundles/Recast/{0}.bytes", text);
    }
}
