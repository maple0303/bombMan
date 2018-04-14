using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles/StandaloneWindows")]
    static void BuildAllAssetBundlesWindows()
    {
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
    }
    [MenuItem("Assets/Build AssetBundles/iOS")]
    static void BuildAllAssetBundlesIos()
    {
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
    }
    [MenuItem("Assets/Build AssetBundles/Android")]
    static void BuildAllAssetBundlesAndroid()
    {
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
    }
    //[MenuItem("Assets/Build Select AssetBundles/StandaloneWindows")]
    //static void BuildSelectAssetBundlesWindows()
    //{
    //    List<AssetBundleBuild> maps = new List<AssetBundleBuild>();
    //    foreach(UnityEngine.Object ob in Selection.objects)
    //    {
    //        AssetBundleBuild build = new AssetBundleBuild();
    //        build.assetBundleName = ob.name;
    //        string[] paths = new string[1];
    //        paths[0] = AssetDatabase.GetAssetPath(ob);
    //        build.assetNames = paths;
    //        maps.Add(build);
    //    }
    //    BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", maps.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
    //}
    [MenuItem("Assets/Get AssetBundle names")]
    static void GetNames()
    {
        var names = AssetDatabase.GetAllAssetBundleNames();
        foreach (var name in names)
            Debug.Log("AssetBundle: " + name);
    }
}
public class MyPostprocessor : AssetPostprocessor
{
    void OnPostprocessAssetbundleNameChanged(string path,
            string previous, string next)
    {
        Debug.Log("AB: " + path + " old: " + previous + " new: " + next);
    }
}