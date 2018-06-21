using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.Re
{
    /// <summary>
    /// 用于管理 Lua 脚本的更新
    /// </summary>
    public class AssetUpdateManager_Lua : AssetUpdateManagerBase
    {

        protected override void Awake()
        {
            base.Awake();
            gameObject.name = "LuaAssetUpdate(manager)";
        }

        protected override void InitialState()
        {
            m_ServerAseetPath = "file://" + @"   E:/My_WorkSpace/LuaAsset_ServerTest/";
            m_AssetConfigurePath = ConstDefine.LuaConfigureFileName;
            m_AssetSaveTopPath = ConstDefine.XluaAssetTopPath;

            base.InitialState();
        }

        /// <summary>
        /// 从下载完成回调中获取当前  的相对路径名以便于记录
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected override string GetAssetRelativePathByUrl(string url)
        {
            int index = url.IndexOf(m_ServerAseetPath);
            if (index == -1)
            {
                Debug.LogError("GetLuaAssetPathByUrl Fail url=" + url);
                return "";
            }

            Debug.Log(url.Substring(index + m_ServerAseetPath.Length));
            return url.Substring(index + m_ServerAseetPath.Length);
        }

        /// <summary>
        ///  根据 Asset 名获取下载时候的路径
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        protected override string GetAssetDownLoadPath(string assetName)
        {
            return m_ServerAseetPath + assetName;
        }






    }
}