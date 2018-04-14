using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Packager {
    public static string platform = string.Empty;
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();
    static List<AssetBundleBuild> maps = new List<AssetBundleBuild>();

    ///-----------------------------------------------------------
    static string[] exts = { ".txt", ".xml", ".lua", ".assetbundle", ".json" };
    static bool CanCopy(string ext) {   //能不能复制
        foreach (string e in exts) {
            if (ext.Equals(e)) return true;
        }
        return false;
    }

    /// <summary>
    /// 载入素材
    /// </summary>
    static UnityEngine.Object LoadAsset(string file) {
        if (file.EndsWith(".lua")) file += ".txt";
        return AssetDatabase.LoadMainAssetAtPath("Assets/LuaFramework/Examples/Builds/" + file);
    }

    [MenuItem("LuaFramework/Build iPhone Resource", false, 100)]
    public static void BuildiPhoneResource() {
        BuildTarget target;
#if UNITY_5
        target = BuildTarget.iOS;
#else
        target = BuildTarget.iPhone;
#endif
        BuildAssetResource(target);
    }

    [MenuItem("LuaFramework/Build Android Resource", false, 101)]
    public static void BuildAndroidResource() {
        BuildAssetResource(BuildTarget.Android);
    }

    [MenuItem("LuaFramework/Build Windows Resource", false, 102)]
    public static void BuildWindowsResource() {
        BuildAssetResource(BuildTarget.StandaloneWindows);
    }

    /// <summary>
    /// 生成绑定素材
    /// </summary>
    public static void BuildAssetResource(BuildTarget target) 
    {
        // 清空之前的lua加密文件
        //string streamPath = Application.streamingAssetsPath + "/LuaScript/";
        //if (Directory.Exists(streamPath))
        //{
        //    Directory.Delete(streamPath, true);
        //}
        //Directory.CreateDirectory(streamPath);
        //streamPath = Application.streamingAssetsPath + "/ToLua/";
        //if (Directory.Exists(streamPath))
        //{
        //    Directory.Delete(streamPath, true);
        //}
        //Directory.CreateDirectory(streamPath);
        AssetDatabase.Refresh();

        maps.Clear();
        //if (AppConst.LuaBundleMode) 
        //{
            HandleLuaBundle();
        //} 
        //else 
        //{
        //    HandleLuaFile();
        //}
        string resPath = "Assets/" + AppConst.AssetDir;
        BuildPipeline.BuildAssetBundles(resPath, maps.ToArray(), BuildAssetBundleOptions.None, target);
        BuildFileIndex();

        string streamDir = Application.dataPath + "/" + AppConst.LuaTempDir;
        if (Directory.Exists(streamDir)) Directory.Delete(streamDir, true);
        AssetDatabase.Refresh();
    }

    static void AddBuildMap(string bundleName, string pattern, string path) 
    {
        string[] files = Directory.GetFiles(path, pattern);
        if (files.Length == 0) return;

        for (int i = 0; i < files.Length; i++) 
        {
            files[i] = files[i].Replace('\\', '/');
        }
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = bundleName;
        build.assetNames = files;
        maps.Add(build);
    }

    /// <summary>
    /// 处理Lua代码包
    /// </summary>
    static void HandleLuaBundle() 
    {
        string streamDir = Application.dataPath + "/" + AppConst.LuaTempDir;
        if (!Directory.Exists(streamDir)) Directory.CreateDirectory(streamDir);

        string[] srcDirs = { Application.dataPath + "/Script/Lua/", Application.dataPath + "/ToLua/Lua" };
        for (int i = 0; i < srcDirs.Length; i++) 
        {
            string sourceDir = srcDirs[i];
            string[] files = Directory.GetFiles(sourceDir, "*.lua", SearchOption.AllDirectories);
            int len = sourceDir.Length;

            if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\') 
            {
                --len;
            }

            if (AppConst.LuaByteMode) 
            {
                for (int j = 0; j < files.Length; j++)
                {
                    string str = files[j].Remove(0, len);
                    string dest = streamDir + str + ".bytes";
                    string filePath = Path.GetDirectoryName(dest);
                    filePath = filePath.Replace('\\', '/');
                    string fileName = Path.GetFileName(dest);
                    string dir = "";
                    if (filePath.IndexOf("/message") != -1)
                    {
                        dir = streamDir + "message";
                    }
                    else if (filePath.IndexOf("/Template") != -1)
                    {
                        dir = streamDir + "template";
                    }
                    else
                    {
                        dir = streamDir + "script";
                    }
                    Directory.CreateDirectory(dir);
                    string outPath = dir + "/" + fileName;
                    EncodeLuaFile(files[j], outPath);
                }    
            } 
            else 
            {
                //ToLuaMenu.CopyLuaBytesFiles(srcDirs[i], streamDir);
                for (int k = 0; k < files.Length; k++)
                {
                    string strFile = files[k].Remove(0, len);
                    string destStr = streamDir + strFile + ".bytes";
                    string filePathStr = Path.GetDirectoryName(destStr);
                    filePathStr = filePathStr.Replace('\\', '/');
                    string fileNameStr = Path.GetFileName(destStr);
                    string dirStr = "";
                    if (filePathStr.IndexOf("/message") != -1)
                    {
                        dirStr = streamDir + "message";
                    }
                    else if (filePathStr.IndexOf("/Template") != -1)
                    {
                        dirStr = streamDir + "template";
                    }
                    else
                    {
                        dirStr = streamDir + "script";
                    }
                    Directory.CreateDirectory(dirStr);
                    string outPathStr = dirStr + "/" + fileNameStr;
                    File.Copy(files[k], outPathStr, true);
                }
            }
        }
        string[] dirs = Directory.GetDirectories(streamDir, "*", SearchOption.AllDirectories);
        for (int i = 0; i < dirs.Length; i++) 
        {
            string name = dirs[i].Replace(streamDir, string.Empty);
            name = name.Replace('\\', '_').Replace('/', '_');
            name = "lua/lua_" + name.ToLower() + AppConst.ExtName;

            string path = "Assets" + dirs[i].Replace(Application.dataPath, "");
            AddBuildMap(name, "*.bytes", path);
        }
        AddBuildMap("lua/lua" + AppConst.ExtName, "*.bytes", "Assets/" + AppConst.LuaTempDir);

        //-------------------------------处理非Lua文件----------------------------------
        string luaPath = AppDataPath + "/StreamingAssets/lua/";
        for (int i = 0; i < srcDirs.Length; i++) 
        {
            paths.Clear(); files.Clear();
            string luaDataPath = srcDirs[i].ToLower();
            Recursive(luaDataPath);
            foreach (string f in files) {
                if (f.EndsWith(".meta") || f.EndsWith(".lua")) continue;
                string newfile = f.Replace(luaDataPath, "");
                string path = Path.GetDirectoryName(luaPath + newfile);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                string destfile = path + "/" + Path.GetFileName(f);
                File.Copy(f, destfile, true);
            }
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 处理Lua文件
    /// </summary>
    static void HandleLuaFile() {
        string[] copyLuaPaths ={ Application.streamingAssetsPath + "/LuaScript/",
                                 Application.streamingAssetsPath + "/ToLua/"};


        string[] luaPaths = { Application.dataPath + "/Script/Lua/", 
                              Application.dataPath + "/ToLua/Lua/"};

        for (int i = 0; i < luaPaths.Length; i++) 
        {
            paths.Clear(); files.Clear();
            string luaDataPath = luaPaths[i];
            Recursive(luaDataPath);
            int n = 0;
            foreach (string f in files) 
            {
                if (f.EndsWith(".meta")) continue;
                string newfile = f.Replace(luaDataPath, "");
                string newpath = copyLuaPaths[i] + newfile;
                string path = Path.GetDirectoryName(newpath);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                if (File.Exists(newpath)) 
                {
                    File.Delete(newpath);
                }
                if (AppConst.LuaByteMode) 
                {
                    EncodeLuaFile(f, newpath);
                } 
                else 
                {
                    File.Copy(f, newpath, true);
                }
                UpdateProgress(n++, files.Count, newpath);
            } 
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static void BuildFileIndex() {
        string resPath = AppDataPath + "/StreamingAssets/";
        ///----------------------创建文件列表-----------------------
        string newFilePath = resPath + "/files.txt";
        if (File.Exists(newFilePath)) File.Delete(newFilePath);

        paths.Clear(); files.Clear();
        Recursive(resPath);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Count; i++) {
            string file = files[i];
            string ext = Path.GetExtension(file);
            if (file.EndsWith(".meta") || file.Contains(".DS_Store")) continue;

            string md5 = md5file(file);
            string value = file.Replace(resPath, string.Empty);
            sw.WriteLine(value + "|" + md5);
        }
        sw.Close(); fs.Close();
    }

    /// <summary>
    /// 数据目录
    /// </summary>
    static string AppDataPath {
        get { return Application.dataPath.ToLower(); }
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path) {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names) 
        {
            string ext = Path.GetExtension(filename);
            if (!ext.Equals(".lua")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs) 
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }
    //copy 进度
    static void UpdateProgress(int progress, int progressMax, string desc) {
        string title = "Processing...[" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }
    //加密成二进制文件
    public static void EncodeLuaFile(string srcFile, string outFile) {
        if (!srcFile.ToLower().EndsWith(".lua")) {
            File.Copy(srcFile, outFile, true);
            return;
        }
        bool isWin = true; 
        string luaexe = string.Empty;
        string args = string.Empty;
        string exedir = string.Empty;
        string currDir = Directory.GetCurrentDirectory();
        if (Application.platform == RuntimePlatform.WindowsEditor) {
            isWin = true;
            luaexe = "luajit.exe";
            args = "-b " + srcFile + " " + outFile;
            exedir = Application.dataPath;
			exedir = exedir.Replace("Assets", "LuaEncoder/luajit/");
        } else if (Application.platform == RuntimePlatform.OSXEditor) {
            isWin = false;
            luaexe = "./luajit";
            args = "-b " + srcFile + " " + outFile;
            exedir = Application.dataPath;
			exedir = exedir.Replace("Assets", "LuaEncoder/luajit_mac/");
        }
        Directory.SetCurrentDirectory(exedir);
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = luaexe;
        info.Arguments = args;
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = isWin;
        info.ErrorDialog = true;
        UnityEngine.Debug.Log(info.FileName + " " + info.Arguments);

        Process pro = Process.Start(info);
        pro.WaitForExit();
		pro.Close ();
        Directory.SetCurrentDirectory(currDir);
    }
    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
}