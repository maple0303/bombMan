
using LuaInterface;
using System.IO;
using UnityEngine;
public class CLuaFunction
{
    public static string LoadXml(string strXmlPath)
    {
        return LuaXml.LoadXml(strXmlPath);
    }
    public static string LoadXmlForText(string strTextContent)
    {
        return LuaXml.LoadXmlForText(strTextContent);
    }
    public static void LogWarning(object str)
    {
        Debugger.LogWarning(str);
    }
    public static void Log(object str)
    {
        Debugger.Log(str);
    }
    public static void LogError(object str)
    {
        Debugger.LogError(str);
    }
    //判断是否存在某个lua文件
    public static bool LuaFileExists(string path)
    {
        if (File.Exists(Application.dataPath + "/Script/Lua/" + path))
        {
            return true;
        }
        if (File.Exists(AppConst.DataPath + "/LuaScript/" + path))
        {
            return true;
        }
        //Debug.Log("判断存在文件路径  " + AppConst.DataPath + "lua/" + path);
        //Debug.Log(File.Exists(AppConst.DataPath + "lua/" + path));
        //Debug.Log(File.Exists(AppConst.DataPath + "lua/" + path + ".bytes"));
        if (File.Exists(AppConst.DataPath + "lua/" + path))
        {
            return true;
        }
        if (LuaFileUtils.Instance.ReadFile(path) != null)
        {
            return true;
        }
		Debug.Log("无次文件  " + AppConst.DataPath + "lua/" + path);
        return false;
    }
    //创建目录
    public static void CreateFile(string path)
    {
        Directory.CreateDirectory(path);
    }
    //判断目录是否存在
    public static bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

}
