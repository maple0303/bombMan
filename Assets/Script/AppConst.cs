using UnityEngine;
using System.Collections;

public class AppConst
{
    public const bool DebugMode = true;                       //调试模式-用于pc端测试，移动端设为false
    /// <summary>
    /// 如果想删掉自带的例子，那这个例子模式必须要
    /// 关闭，否则会出现一些错误。
    /// </summary>
    public const bool ResourceBundleMode = false;                       //资源AssetBundle模式模式 若为true,打包资源时会把设置好的资源打包成budnle 

    /// <summary>
    /// 如果开启更新模式，前提必须启动框架自带服务器端。
    /// 否则就需要自己将StreamingAssets里面的所有内容
    /// 复制到自己的Webserver上面，并修改下面的WebUrl。
    /// </summary>
    public const bool UpdateMode = false;                       //更新模式-默认关闭 
	public const bool LuaByteMode = true;                       //Lua字节码模式-默认关闭 
	public const bool LuaBundleMode = false;                    //Lua代码AssetBundle模式
    public const string LuaTempDir = "Lua/";                    //临时目录
    public const string ExtName = ".unity3d";                   //素材扩展名
    public const string AssetDir = "StreamingAssets";           //素材目录 
    public const string FestivalTaskDir = "Task/";              //节日任务的目录,改成和调试模式下的目录结构相同


    /// <summary>
    /// 取得数据存放目录
    /// </summary>
    public static string DataPath
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/";
            }
            
            //if (Application.platform == RuntimePlatform.OSXEditor)
            //{
           	//    int i = Application.dataPath.LastIndexOf('/');
            //    return Application.dataPath.Substring(0, i + 1) + "/";
            //}
            return  Application.dataPath + "/" + AppConst.AssetDir + "/";
        }
    }

    public static string GetRelativePath()
    {
        if (Application.isEditor)
        {
            return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/") + "/Assets/" + AppConst.AssetDir + "/";
        }
        else if (Application.isMobilePlatform || Application.isConsolePlatform)
        {
            return "file:///" + Application.persistentDataPath + "/";
        }
        else
        {
            // For standalone player.
            return "file://" + Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// 应用程序内容路径
    /// </summary>
    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = Application.dataPath + "/" + AppConst.AssetDir + "/";
                break;
        }
        return path;
    }
}
