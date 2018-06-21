using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.Re
{
    /// <summary>
    /// 用于管理AssetBundle 资源热更新
    /// </summary>
    public class AssetUpdateManager_AssetBundle : AssetUpdateManagerBase
    {
        protected override void Awake()
        {
            base.Awake();
            gameObject.name ="AssetbundleUpdate(manager)";
        }

        protected override void InitialState()
        {
            m_ServerAseetPath = "file://" + @"E:/My_WorkSpace/AssetBundle_Test/";
            m_AssetConfigurePath = ConstDefine.AssetBundleConfigurePath;
            m_AssetSaveTopPath = ConstDefine.ABundleTopPath;

            base.InitialState();
        }

        /// <summary>
        /// 从下载完成回调中获取当前  的相对路径名以便于记录
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected override string GetAssetRelativePathByUrl(string url)
        {
            string abundleDwonPath = m_ServerAseetPath + ConstDefine.ABundleTopFileNameOfPlatformat + "/";
            int index = url.IndexOf(abundleDwonPath);
            if (index == -1)
            {
                Debug.LogError("GetAssetRelativePathByUrl Fail url=" + url);
                return "";
            }

            //Debug.Log(url.Substring(index + abundleDwonPath.Length));
            return url.Substring(index + abundleDwonPath.Length);
        }

        /// <summary>
        ///  根据 Asset 名获取下载时候的路径
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        protected override string GetAssetDownLoadPath(string assetName)
        {
            return m_ServerAseetPath + ConstDefine.ABundleTopFileNameOfPlatformat + "/" + assetName;
        }


    }
}