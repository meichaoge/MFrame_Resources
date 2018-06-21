using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFramework;
using System;
using System.IO;
using MFramework.Re;

/// <summary>
/// 资源管理的类 所有需要使用Resources 的地方使用这个
/// </summary>
public class ResourcesMgr : Singleton_Static<ResourcesMgr>
{
    private Dictionary<string, AssetBundle> m_AllLoadedABundle = new Dictionary<string, AssetBundle>();
    private AssetBundleManifest m_MainFest = null;
    public AssetBundleManifest Manifest
    {
        get
        {
            if (m_MainFest == null)
            {
                GetAssetBundleManifest();
            }
            return m_MainFest;
        }
        private set { m_MainFest = value; }
    }


    Dictionary<string, LocalFileBase> m_AllLoadLocalFileDic = new Dictionary<string, LocalFileBase>();  //记录加载的TextAsset文件




    protected override void InitialSingleton() { }

    /// <summary>
    /// 加载主的ABundle 文件
    /// </summary>
    void GetAssetBundleManifest()
    {
        string manifestPath = ConstDefine.ABundleTopPath + ConstDefine.ABundleTopFileNameOfPlatformat + "/" + ConstDefine.ABundleTopFileNameOfPlatformat;
        AssetBundle mainAssetBundle = AssetBundle.LoadFromFile(manifestPath);
        if (mainAssetBundle == null)
        {
            Debug.LogError("mainAssetBundle is Null");
            return;
        }
        m_MainFest = mainAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        if (m_MainFest == null)
        {
            Debug.LogError("GetAssetBundleManifest Fail");
            return;
        }
      
    }


    #region 公共的资源管理接口

    /// <summary>
    /// 加载资源的统一入口 当外部AssetBundle 资源不存在时候 会尝试加载Resources中的资源 但是可能会出现异常情况
    /// </summary>
    /// <param name="assetPath">资源相对于Reources的路径</param>
    /// <param name="callback"></param>
    public void LoadAsset(string assetPath, Action<UnityEngine.Object> callback)
    {
        UnityEngine.Object result = null;
        Debug.Log("APPEngineManager.GetInstance().m_IsAssetUpdating=" + APPEngineManager.GetInstance().m_IsAssetUpdating);
        if (APPEngineManager.GetInstance().m_IsAssetUpdating||Manifest == null)
        {
            Debug.LogInfor("如果不存在AssetBundle 则尝试加载本地Resources中资源");
            GetLocalResourcesAsset(assetPath, ref result);  //如果不存在AssetBundle 则尝试加载本地Resources中资源
            if (callback != null) callback(result);
            return;
        }  //Manifest 加载失败
        string abundlePath = ConstDefine.ABundleTopPath + ConstDefine.ABundleTopFileNameOfPlatformat + "/" + assetPath + ConstDefine.ABundleObjExtensitonName;
        Debug.LogInfor("abundlePath=" + abundlePath);
        if (File.Exists(abundlePath))
        {
            result = LoadAssetByPath(assetPath);
        }//只有当外部存储内存中存在该资源时读取 否则使用Reources中的资源

        if (result != null)
        {
            if (callback != null) callback(result);
            return;
        }
        GetLocalResourcesAsset(assetPath, ref result);  //如果不存在AssetBundle 则尝试加载本地Resources中资源
        if (callback != null) callback(result);
    }

    /// <summary>
    /// 直接加载本地的资源
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="callback"></param>
    public void LoadResourcesAsset(string assetPath, Action<UnityEngine.Object> callback)
    {
        UnityEngine.Object result = null;
        GetLocalResourcesAsset(assetPath, ref result);  //如果不存在AssetBundle 则尝试加载本地Resources中资源
        if (callback != null) callback(result);
        return;
    }

    /// <summary>
    /// 加载Sprite 资源 由于所有的Sprite 按照文件夹打包的，需要单独处理
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="callback"></param>
    public void LoadSprite(string assetPath, System.Action<UnityEngine.Object> callback)
    {
        UnityEngine.Object result = null;
        if ( APPEngineManager.GetInstance().m_IsAssetUpdating|| Manifest == null )
        {
            Debug.LogInfor("如果不存在AssetBundle 则尝试加载本地Resources中资源");
            GetLocalResourcesAsset(assetPath, ref result);  //如果不存在AssetBundle 则尝试加载本地Resources中资源
            if (callback != null) callback(result);
            return;
        }  //Manifest 加载失败
        string parentDirectoryName = System_IO_Path_Ex.GetPathParentDirectoryName(assetPath);//获取一个文件下打包的Sprite 
        Debug.LogInfor("LoadSprite  parentDirectoryName=" + parentDirectoryName);


        //Debug.LogInfor("abundlePath=" + abundlePath);
        string assetDirectoryPath = System.IO.Path.GetDirectoryName(assetPath);
        string spriteAssetPath = assetDirectoryPath + "/" + parentDirectoryName;

        string spriteAbundlepath = ConstDefine.ABundleTopPath + ConstDefine.ABundleTopFileNameOfPlatformat + "/" + spriteAssetPath + ConstDefine.ABundleObjExtensitonName;
        Debug.LogInfor("spriteAbundlepath=" + spriteAbundlepath);
        if (System.IO.File.Exists(spriteAbundlepath))
        {
            result = LoadSpriteAssetByPath(spriteAssetPath,System.IO.Path.GetFileName(assetPath));
        }//只有当外部存储内存中存在该资源时读取 否则使用Reources中的资源

        if (result != null)
        {
            if (callback != null) callback(result);
            return;
        }
        GetLocalResourcesAsset(assetPath, ref result);  //如果不存在AssetBundle 则尝试加载本地Resources中资源
        if (callback != null) callback(result);
    }

    /// <summary>
    /// 加载TextAsset资源文件（仅仅适用于特定格式的Json 文件）
    /// </summary>
    /// <param name="assetPath">资源相对于Resource的路径</param>
    /// <param name="fileType">配置文件的类型</param>
    /// <param name="callback"></param>
    public void LoadTextAsset(string assetPath,System.Action<LocalFileBase> callback, LocalFileTypeEnum fileType= LocalFileTypeEnum.ObjectSimpleVale)
    {
        if (m_AllLoadLocalFileDic.ContainsKey(assetPath))
        {
            if (callback != null)
                callback(m_AllLoadLocalFileDic[assetPath]);
            return;
        }

        LoadAsset(assetPath, (obj) => {
            if(obj ==null)
            {
                if (callback != null)
                    callback(null);
                return;
            }

            TextAsset asset = obj as TextAsset;
            if(asset==null)
            {
                Debug.LogError("加载TextAsset 失败 " + assetPath);
                if (callback != null)
                    callback(null);
                return;
            }
            LocalFileBase localFile = LocalFileFactory.GetInstance().GetLocalFileBase(fileType, assetPath, asset.text);
            if (localFile == null)
            {
                Debug.LogError("生成本地文件失败 " + assetPath);
                if (callback != null)
                    callback(null);
                return;
            }
            m_AllLoadLocalFileDic.Add(assetPath, localFile);
            if (callback != null)
                callback(localFile);
        });
    }





    /// <summary>
    /// 加载本地Resource中的资源
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="result"></param>
    private  void GetLocalResourcesAsset(string assetPath, ref UnityEngine.Object result)
    {
        Debug.LogInfor("Load Resource Asset " + assetPath);
        //if(System.IO.Path.GetExtension(assetPath)==".txt")
        //    result = Resources.Load<TextAsset>(assetPath); //如果该资源不存在 则会返回 null
        //else
         result = Resources.Load(assetPath); //如果该资源不存在 则会返回 null
        if(result==null)
        {
            Debug.LogError("GetLocalResourcesAsset Fail,Not Exit " + assetPath);
        }
    }

    /// <summary>
    /// 根据路径加载资源
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    private UnityEngine.Object LoadAssetByPath(string assetPath)
    {
        AssetBundle bundle = null;
        UnityEngine.Object result = null;
        if (assetPath.Contains(ConstDefine.ABundleObjExtensitonName) == false)
            assetPath += ConstDefine.ABundleObjExtensitonName;  //如果路径中不包含打包设置的ABundle 后缀名 则添加
        assetPath = assetPath.ToLower(); //将相对于Resources 路径转换成一个配置路径名  并且由于Unity 所有的 AssetBundle 扩展名都是小写这里也需要小写

        //   string assetName = GetAssetNameFromPath(assetPath);//获取需要加载的真是资源名
        string assetName = System.IO.Path.GetFileName(assetPath);//获取需要加载的真实资源名
        Debug.LogInfor("name=" + assetPath + "               >>assetName=" + assetName);

        if (m_AllLoadedABundle.TryGetValue(assetPath, out bundle))
        {
            result = bundle.LoadAsset<UnityEngine.Object>(AssetNameWithOutExtensiton(assetName));
        }//如果已经加载过这个 AssetBundle 则直接获取这个 AssetBundle
        else
        {
            string[] dependences = Manifest.GetAllDependencies(assetPath); //**这里需要特别注意 搜索的AssetBundle 需要包含路径 以便于搜索到资源
            AssetBundle dependenceAB = null;
            string path = ConstDefine.ABundleTopPath + ConstDefine.ABundleTopFileNameOfPlatformat + "/"; //本地所有下载的AssetBundle 的顶层目录
            foreach (var item in dependences)
            {
                if (m_AllLoadedABundle.TryGetValue(item, out dependenceAB)) //item 中分隔符是“/” 这里需要转换
                {
                    dependenceAB.LoadAsset(item);  //加载依赖的资源
                }
                else
                {
                    LoadAssetByPath(item);  //循环获取所有当前资源依赖的资源 并加载出来
                }
            }//foreach
            try
            {
                //    string apath= @"C:\Users\client2\AppData\LocalLow\DefaultCompany\MFrameWork_Arrange\\ABundle\Window\prefabs\slider.unity3d";
                bundle = AssetBundle.LoadFromFile(path + assetPath);  //根据AssetBundle 的绝对路径加载资源
                m_AllLoadedABundle.Add(assetPath, bundle); //保存该AssetBudle 的引用
                result = bundle.LoadAsset<UnityEngine.Object>(AssetNameWithOutExtensiton(assetName));  //LoadAsset 中参数name 不能包含扩展名 
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
            } //存在可能性 当前需要加载的资源依赖的某些资源确实
        }
        return result;
    }

    private Sprite LoadSpriteAssetByPath(string parentTexturePath,string spriteName)
    {
        AssetBundle bundle = null;
        Sprite[] allSprites = new Sprite[0];
        if (parentTexturePath.Contains(ConstDefine.ABundleObjExtensitonName) == false)
            parentTexturePath += ConstDefine.ABundleObjExtensitonName;  //如果路径中不包含打包设置的ABundle 后缀名 则添加
        parentTexturePath = parentTexturePath.ToLower(); //将相对于Resources 路径转换成一个配置路径名  并且由于Unity 所有的 AssetBundle 扩展名都是小写这里也需要小写

        Debug.LogInfor("name=" + parentTexturePath + "               >>spriteName=" + spriteName);

        if (m_AllLoadedABundle.TryGetValue(parentTexturePath, out bundle))
        {
            allSprites = bundle.LoadAllAssets<Sprite>();
        }//如果已经加载过这个 AssetBundle 则直接获取这个 AssetBundle
        else
        {
            string[] dependences = Manifest.GetAllDependencies(parentTexturePath); //**这里需要特别注意 搜索的AssetBundle 需要包含路径 以便于搜索到资源
            AssetBundle dependenceAB = null;
            string path = ConstDefine.ABundleTopPath + ConstDefine.ABundleTopFileNameOfPlatformat + "/"; //本地所有下载的AssetBundle 的顶层目录
            foreach (var item in dependences)
            {
                if (m_AllLoadedABundle.TryGetValue(item, out dependenceAB)) //item 中分隔符是“/” 这里需要转换
                {
                    dependenceAB.LoadAsset(item);  //加载依赖的资源
                }
                else
                {
                    LoadAssetByPath(item);  //循环获取所有当前资源依赖的资源 并加载出来
                }
            }//foreach
            try
            {
                //    string apath= @"C:\Users\client2\AppData\LocalLow\DefaultCompany\MFrameWork_Arrange\\ABundle\Window\prefabs\slider.unity3d";
                bundle = AssetBundle.LoadFromFile(path + parentTexturePath);  //根据AssetBundle 的绝对路径加载资源
                m_AllLoadedABundle.Add(parentTexturePath, bundle); //保存该AssetBudle 的引用
                allSprites = bundle.LoadAllAssets<Sprite>();
                //    result = bundle.LoadAsset<UnityEngine.Object>(AssetNameWithOutExtensiton(assetName));  //LoadAsset 中参数name 不能包含扩展名 
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
            } //存在可能性 当前需要加载的资源依赖的某些资源确实
        }

        foreach (var sprite in allSprites)
        {
            if (sprite.name == spriteName)
                return sprite;
        }
        Debug.LogError("LoadSpriteAssetByPath Fai.Not Exit " + parentTexturePath);
        return null;
    }


    /// <summary>
    /// 去掉路径中的文件名中带的后缀 .unity3d
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private string AssetNameWithOutExtensiton(string name)
    {
        return System.IO.Path.GetFileNameWithoutExtension(name);
    }

    /// <summary>
    /// 根据路径名获取需要加载的ABundle 名
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private string GetAssetNameFromPath(string path)
    {
        string[] str = path.Split('/');
        if (str.Length == 0)
            return path;
        return str[str.Length - 1];
    }



    #endregion



}
