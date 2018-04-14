//#define ASSETBUNDLE_ENABLE

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
public class CAssetManager : MonoBehaviour
{
    // Class with the AssetBundle reference, url and version
    private class AssetBundleRef
    {
        public AssetBundle assetBundle = null;
        public float timeStamp = 0;
        public string url;
        public AssetBundleRef(string strUrlIn)
        {
            url = strUrlIn;
            timeStamp = Time.time;
        }
    };

    // A dictionary to hold the AssetBundle references
    static private Dictionary<string, AssetBundleData> dictAssetBundleRefs = null;
    static private Dictionary<string, string> dicAbName = null;
    static private Dictionary<string, List<string>> m_DependencyTable = null; //依赖关系表
    static private Dictionary<string, Dictionary<string, Sprite>> m_dicSpriteAtlas = new Dictionary<string, Dictionary<string, Sprite>>(); // 图集资源表

    private bool m_bIsLoading = false;
    private List<LoadTaskData> m_listLoadData = new List<LoadTaskData>();
    private Coroutine m_coroutine;

    void Awake()
    {
#if !UNITY_EDITOR || ASSETBUNDLE_ENABLE 
        dictAssetBundleRefs = new Dictionary<string, AssetBundleData>();
        dicAbName = new Dictionary<string, string>();
        m_DependencyTable = new Dictionary<string, List<string>>();
        m_dicSpriteAtlas = new Dictionary<string, Dictionary<string, Sprite>>();
        AssetBundle mainBundle = null;
        // 获得更新包路径
        string strPersistentDataPath = string.Empty;
        if (Application.isMobilePlatform)
        {
            strPersistentDataPath = Application.persistentDataPath;
        }
        // 获得原始数据路径
        string strRawPath = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                strRawPath = "jar:file://" + Application.dataPath + "!/assets";
                break;
            case RuntimePlatform.IPhonePlayer:
                strRawPath = Application.dataPath + "/Raw";
                break;
            default:
                strRawPath = Application.streamingAssetsPath;
                break;
        }
        if (strPersistentDataPath != string.Empty && File.Exists(Application.persistentDataPath + "/StreamingAssets"))
        {
            mainBundle = AssetBundle.LoadFromFile(strPersistentDataPath + "/StreamingAssets");
        }
        else
        {
            // 没有新版的bundle主文件，则加载安装包里的bundle主文件
            mainBundle = AssetBundle.LoadFromFile(strRawPath + "/StreamingAssets");
        }
        // 构建资源依赖关系
        AssetBundleManifest manifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] arrStrBundle = manifest.GetAllAssetBundles();
        foreach (string strBundleName in arrStrBundle)
        {
            AssetBundleRef abRef = new AssetBundleRef(strBundleName);
            AssetBundleData assetBundleData = new AssetBundleData();
            assetBundleData.listUseBundleName = new List<string>();
            assetBundleData.assetBundleRef = abRef;
            if (strPersistentDataPath != string.Empty && File.Exists(Application.persistentDataPath + "/" + strBundleName))
            {
                assetBundleData.strAssetPath = strPersistentDataPath + "/" + strBundleName;
            }
            else
            {
                // 没有新版的bundle主文件，则加载安装包里的bundle主文件
                assetBundleData.strAssetPath = strRawPath + "/" + strBundleName;
            }
            abRef.assetBundle = AssetBundle.LoadFromFile(assetBundleData.strAssetPath);

            string[] arrStrName = abRef.assetBundle.GetAllAssetNames();
            string[] arrDependenciesName = manifest.GetAllDependencies(abRef.assetBundle.name);
            assetBundleData.arrDependenciesName = new string[arrDependenciesName.Length];
            for (int i = 0; i < arrDependenciesName.Length; i++)
            {
                assetBundleData.arrDependenciesName[i] = arrDependenciesName[i];
            }
            
            foreach (string strAssetName in arrStrName)
            {
                if (!dicAbName.ContainsKey(abRef.assetBundle.name))
                {
                    dicAbName.Add(abRef.assetBundle.name, strAssetName);
                }
                dictAssetBundleRefs.Add(strAssetName, assetBundleData);
            }
            abRef.assetBundle.Unload(false);
        }
        mainBundle.Unload(false);
#endif

    }

    static CAssetManager()
    {
    }

    public static UnityEngine.Object GetAsset(string strAssetUrl, Type type)
    {
        string keyName = "assets/" + strAssetUrl.ToLower();
#if UNITY_EDITOR && !ASSETBUNDLE_ENABLE
        return AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(keyName);
#else
        AssetBundleData assetBundleData;
        if (!dictAssetBundleRefs.TryGetValue(keyName, out assetBundleData))
        {
            return null;
        }
        if (assetBundleData.assetBundleRef.assetBundle == null)
        {
            assetBundleData.assetBundleRef.assetBundle = AssetBundle.LoadFromFile(assetBundleData.strAssetPath);
        }

        return assetBundleData.assetBundleRef.assetBundle.LoadAsset(keyName, type);
#endif
    }
    // Get an AssetBundle
    public static UnityEngine.Object GetAsset(string strAssetUrl)
    {
        string keyName = "assets/" + strAssetUrl.ToLower();
#if UNITY_EDITOR && !ASSETBUNDLE_ENABLE
        return AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(keyName);
#else
        AssetBundleData assetBundleData;
        if (!dictAssetBundleRefs.TryGetValue(keyName, out assetBundleData))
        {
            Debug.Log("没加载到  " + keyName);
            return null;
        }
        //加载依赖的bundle
        foreach (string strDependenciesName in assetBundleData.arrDependenciesName)
        {
            LoadDependenciesBundle(dicAbName[strDependenciesName], keyName);
        }
        if (assetBundleData.assetBundleRef.assetBundle == null)
        {
            assetBundleData.assetBundleRef.assetBundle = AssetBundle.LoadFromFile(assetBundleData.strAssetPath);
        }
        return assetBundleData.assetBundleRef.assetBundle.LoadAsset(keyName);
#endif
    }

    //加载依赖的bundle文件
    private static void LoadDependenciesBundle(string keyName, string useName)
    {
        AssetBundleData abData;
        if (!dictAssetBundleRefs.TryGetValue(keyName, out abData))
        {
            Debug.Log("没加载到  " + keyName);
            return;
        }
        if (abData.assetBundleRef.assetBundle == null)
        {
            abData.assetBundleRef.assetBundle = AssetBundle.LoadFromFile(abData.strAssetPath);
        }

        //将依赖这个资源的bundle名字，加入到使用者列表中
        if (abData.listUseBundleName.IndexOf(useName) == -1)
        {
            abData.listUseBundleName.Add(useName);
        }
        //加载依赖的bundle
        foreach (string strDependenciesName in abData.arrDependenciesName)
        {
            LoadDependenciesBundle(dicAbName[strDependenciesName], keyName);
        }
    }
    public static void UnloadAsset(string strAssetUrl)
    {
#if !UNITY_EDITOR || ASSETBUNDLE_ENABLE
        string keyName = "assets/" + strAssetUrl.ToLower();
        AssetBundleData assetBundleData;
        if (!dictAssetBundleRefs.TryGetValue(keyName, out assetBundleData))
        {
            Debug.Log("没加载到  " + keyName);
            return;
        }
        //加载依赖的bundle
        foreach (string strDependenciesName in assetBundleData.arrDependenciesName)
        {
            UnloadDependenciesBundle(dicAbName[strDependenciesName], keyName);
        }
        if (assetBundleData.assetBundleRef.assetBundle != null)
        {
            assetBundleData.assetBundleRef.assetBundle.Unload(true);
            Debug.Log("卸载" + strAssetUrl);
        }
#endif
    }
    //卸载依赖的bundle文件
    private static void UnloadDependenciesBundle(string keyName, string useName)
    {
        AssetBundleData abData;
        if (!dictAssetBundleRefs.TryGetValue(keyName, out abData))
        {
            return;
        }

        //将依赖这个资源的bundle名字，从使用者列表中移除
        int nIndex = abData.listUseBundleName.IndexOf(useName);
        if (nIndex >= 0)
        {
            abData.listUseBundleName.RemoveAt(nIndex);
        }
        //资源处于空闲状态下，卸载bundle(使用者数量为0)
        if (abData.listUseBundleName.Count == 0)
        {
            if (abData.assetBundleRef.assetBundle != null)
            {
                abData.assetBundleRef.assetBundle.Unload(true);
                Debug.Log("卸载" + keyName);
            }
        }
        //加载依赖的bundle
        foreach (string strDependenciesName in abData.arrDependenciesName)
        {
            UnloadDependenciesBundle(dicAbName[strDependenciesName], keyName);
        }
    }

    public static Sprite GetAssetSprite(string strAssetUrl, string strAssetAtlas = "")
    {
        string keyName = "assets/" + strAssetUrl.ToLower();
        if (strAssetAtlas != "")
        {
            keyName = "assets/" + strAssetAtlas.ToLower();
        }

#if UNITY_EDITOR && !ASSETBUNDLE_ENABLE
         if (strAssetAtlas != "")
        {
            Dictionary<string, Sprite> dicSprite = null;
            m_dicSpriteAtlas.TryGetValue(keyName, out dicSprite);
            if(dicSprite == null)
            {
                dicSprite = new Dictionary<string, Sprite>();
                UnityEngine.Object[] arrObj = AssetDatabase.LoadAllAssetsAtPath(keyName);
                for (int i = 0; i < arrObj.Length; i++)
                {
                    if(arrObj[i].GetType() == typeof(UnityEngine.Sprite))
                    {
                        dicSprite[arrObj[i].name] = arrObj[i] as Sprite;
                    }
                }
                m_dicSpriteAtlas[keyName] = dicSprite;
            }
            Sprite sprite = null;
            dicSprite.TryGetValue(strAssetUrl, out sprite);
            return sprite;
        }

        return AssetDatabase.LoadAssetAtPath<Sprite>(keyName);
#else
        AssetBundleData assetBundleData;
        if (!dictAssetBundleRefs.TryGetValue(keyName, out assetBundleData))
        {
            return null;
        }
        //加载依赖的bundle
        foreach (string strDependenciesName in assetBundleData.arrDependenciesName)
        {
            LoadDependenciesBundle(dicAbName[strDependenciesName], keyName);
        }
        if (assetBundleData.assetBundleRef.assetBundle == null)
        {
            assetBundleData.assetBundleRef.assetBundle = AssetBundle.LoadFromFile(assetBundleData.strAssetPath);
        }
        if (strAssetAtlas != "")
        {
            Dictionary<string, Sprite> dicSprite = null;
            m_dicSpriteAtlas.TryGetValue(keyName, out dicSprite);
            if(dicSprite == null)
            {
                dicSprite = new Dictionary<string, Sprite>();
                Sprite[] arrSprites = assetBundleData.assetBundleRef.assetBundle.LoadAllAssets<Sprite>();
                for (int i = 0; i < arrSprites.Length; i++)
                {
                    dicSprite[arrSprites[i].name] = arrSprites[i];
                }
                m_dicSpriteAtlas[keyName] = dicSprite;
            }
            return dicSprite[strAssetUrl];
            
        }
        return assetBundleData.assetBundleRef.assetBundle.LoadAsset<Sprite>(keyName);
#endif
    }

    public void GetAssetSpriteAsync(string strAssetUrl, string strAssetAtlas = "", Action<UnityEngine.Object> callBack = null)
    {
        string keyName = "assets/" + strAssetUrl.ToLower();
        if (strAssetAtlas != "")
        {
            keyName = "assets/" + strAssetAtlas.ToLower();
        }
        LoadResAsync(strAssetAtlas, callBack, true);
    }

    public void LoadResAsync(string url, Action<UnityEngine.Object> callBack, bool bSprite)
    {
        if (m_bIsLoading)
        {
            LoadTaskData data = new LoadTaskData();
            data.url = url;
            data.func = callBack;
            data.bSprite = bSprite;
            m_listLoadData.Add(data);
        }
        else
        {
            if (bSprite)
            {
                m_coroutine = StartCoroutine(GetAssetAsync<Sprite>(url, callBack));
            }
            else
            {
                m_coroutine = StartCoroutine(GetAssetAsync<UnityEngine.Object>(url, callBack));
            }
        }
    }

    public void ExeCoroutineTask(Action startFunc, Action<object> callBack, object pararm)
    {
        if (m_bIsLoading)
        {
            LoadTaskData data = new LoadTaskData();
            data.startFunc = startFunc;
            data.callBack = callBack;
            data.pararm = pararm;
            m_listLoadData.Add(data);
        }
        else
        {
            m_coroutine = StartCoroutine(DoCoroutineTask(startFunc, callBack, pararm));
        }
    }

    public void ClearAsyncLoadingTask()
    {
        m_bIsLoading = false;
        StopCurAsyncLoading();
        m_listLoadData.Clear();
    }

    public void StopCurAsyncLoading()
    {
        if (m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
        }
    }

    private void CheckAsyncLoad()
    {
        if (m_listLoadData.Count == 0)
        {
            return;
        }
        LoadTaskData data = m_listLoadData[0];
        m_listLoadData.RemoveAt(0);
        if (string.IsNullOrEmpty(data.url))
        {
            m_coroutine = StartCoroutine(DoCoroutineTask(data.startFunc, data.callBack, data.pararm));
            //ExeCoroutineTask(data.startFunc, data.callBack, data.pararm);
        }
        else
        {
            m_coroutine = StartCoroutine(GetAssetAsync<UnityEngine.Object>(data.url, data.func));
            //LoadResAsync(data.url, data.func);
        }
    }

    public IEnumerator DoCoroutineTask(Action startFunc, Action<object> callBack, object pararm)
    {
        m_bIsLoading = true;
        if (startFunc == null)
        {
            m_bIsLoading = false;
            yield break;
        }
        startFunc();

        if (callBack != null)
        {
            callBack(pararm);
            yield return new WaitForSeconds(0.1f);
        }
        m_bIsLoading = false;
        this.CheckAsyncLoad();
    }

    // 协程实现
    public IEnumerator GetAssetAsync<T>(string strAssetUrl, Action<UnityEngine.Object> callBack) where T : UnityEngine.Object 
    {
        m_bIsLoading = true;

        string keyName = "assets/" + strAssetUrl.ToLower();
#if UNITY_EDITOR && !ASSETBUNDLE_ENABLE
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<T>(keyName);
        yield return obj;
        m_bIsLoading = false;
        callBack(obj);
        this.CheckAsyncLoad();
#else
        AssetBundleData assetBundleData;
        if (!dictAssetBundleRefs.TryGetValue(keyName, out assetBundleData))
        {
            Debug.Log("没加载到  " + keyName);
            m_bIsLoading = false;
            this.CheckAsyncLoad();
            yield break;
        }
        int nSum = 0;
        //加载依赖的bundle
        foreach (string strDependenciesName in assetBundleData.arrDependenciesName)
        {
            nSum++;
            StartCoroutine(LoadAsyncDependenciesBundle(dicAbName[strDependenciesName], keyName,
                delegate() { nSum--; }));
        }
        yield return new WaitUntil(() => nSum == 0);

        if (assetBundleData.assetBundleRef.assetBundle == null)
        {
            AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(assetBundleData.strAssetPath);
            yield return new WaitUntil(() => abcr.isDone == true);

            assetBundleData.assetBundleRef.assetBundle = abcr.assetBundle;
        }
        AssetBundleRequest assetLoadRequest = assetBundleData.assetBundleRef.assetBundle.LoadAssetAsync<T>(keyName);
        yield return new WaitUntil(() => assetLoadRequest.isDone == true);
        UnityEngine.Object obj = assetLoadRequest.asset;
        m_bIsLoading = false;
        callBack(obj);
        this.CheckAsyncLoad();
#endif
    }
    //加载依赖的bundle文件
    private IEnumerator LoadAsyncDependenciesBundle(string keyName, string useName, Action callBack)
    {
        AssetBundleData abData;
        if (!dictAssetBundleRefs.TryGetValue(keyName, out abData))
        {
            Debug.Log("没加载到  " + keyName);
            if (callBack != null)
            {
                callBack();
            }
            yield break;
        }
        if (abData.assetBundleRef.assetBundle == null)
        {
            AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(abData.strAssetPath);
            yield return new WaitUntil(() => abcr.isDone == true);
            abData.assetBundleRef.assetBundle = abcr.assetBundle;
        }

        //将依赖这个资源的bundle名字，加入到使用者列表中
        if (abData.listUseBundleName.IndexOf(useName) == -1)
        {
            abData.listUseBundleName.Add(useName);
        }
        int nSum = 0;
        //加载依赖的bundle
        foreach (string strDependenciesName in abData.arrDependenciesName)
        {
            nSum++;
            LoadAsyncDependenciesBundle(dicAbName[strDependenciesName], keyName,
                delegate() { nSum--; });
        }
        yield return new WaitUntil(() => nSum == 0);
        if (callBack != null)
        {
            callBack();
        }
    }

    struct AssetBundleData
    {
        public AssetBundleRef assetBundleRef;
        public string[] arrDependenciesName;
        public string strAssetPath;
        public List<string> listUseBundleName;
    }

    struct LoadTaskData
    {
        public string url;
        public Action<UnityEngine.Object> func;
        public Action startFunc;
        public Action<object> callBack;
        public object pararm;
        public bool bSprite;
    }
}