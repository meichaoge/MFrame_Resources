using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

namespace MFramework.Re
{
    /// <summary>
    /// 加载资源的方式
    /// </summary>
    public enum LoadResourceEum
    {
        LoadResources,   //加载本地的资源
        LoadAssetBundle, //优先加载AssetBudle 资源
    }


    //整个程序的入口 负责Lua 的启动和其他的模块
    public class APPEngineManager : Singleton_Mono<APPEngineManager>
    {

        private static LuaEnv _luaEnv = null; //all lua behaviour shared one luaenv only!
        public static LuaEnv LuaEngine
        {
            get
            {
                if (_luaEnv == null)
                    _luaEnv = new LuaEnv();
                return _luaEnv;
            }
        } //全局共享的lua 虚拟机
        private static float lastGCTime = 0;
        private const float GCInterval = 1;//1 second 
        [Header("加载优先加载资源的方式")]
        public LoadResourceEum m_LoadResourceEum = LoadResourceEum.LoadResources;

        public System.Action OnAssetUpdateDoneAct = null; //资源完成更新

        public bool m_IsAssetUpdating { private set; get; }   //标识当前是否实在更新资源状态

        public int m_TotalNeedDownLoadSize { private set; get; }

        private AssetUpdateManager_AssetBundle m_AssetUpdateManager_AssetBundleScript;
        private AssetUpdateManager_Lua m_AssetUpdateManager_LuaScript;

        private UITestAssetUpdateView m_UITestAssetUpdateView;//显示加载进度的界面


        #region Unity Frame
        protected override void Awake()
        {
            base.Awake();
            m_TotalNeedDownLoadSize = 0;
            GameObject.DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Debug.Log("Application.persistentDataPath=" + Application.persistentDataPath);
            m_IsAssetUpdating = true;
            ShowAssetUpdateUI();
            BeginHotUpdateAsset();
        }

        private void Update()
        {
            if (Time.time - lastGCTime > GCInterval)
            {
                LuaEngine.Tick();
                lastGCTime = Time.time;
            }
        }

        protected override void OnDestroy()
        {
            LuaEngine.Dispose();
            base.OnDestroy();
        }

        #endregion

        #region 资源和lua 脚本热更新模块
        private void BeginHotUpdateAsset()
        {
            #region Lua  管理器初始化
            GameObject luaObj = new GameObject();
            m_AssetUpdateManager_LuaScript = luaObj.GetAddComponent<AssetUpdateManager_Lua>();
            m_AssetUpdateManager_LuaScript.m_OnAssetUpdateDone += OnLuaAssetUpdateComplete;
            m_AssetUpdateManager_LuaScript.m_CompleteCheckAssetStateAct += OnLuaAssetCompareComplete;
            m_AssetUpdateManager_LuaScript.m_OnDownLoadServerConfigureFailAct += OnDownLoadLuaServerConfigureFail;
            m_AssetUpdateManager_LuaScript.m_OnDownLoadAssetFailAct += OnDownloadLuaAssetOutOfTime;
            m_AssetUpdateManager_LuaScript.m_OnDownLoadAssetSuccessAct += OnLuaAssetDownLoadSuccess;
            #endregion

            #region AssetBundle 管理器初始化
            GameObject AssetBundleObj = new GameObject();
            m_AssetUpdateManager_AssetBundleScript = AssetBundleObj.GetAddComponent<AssetUpdateManager_AssetBundle>();
            m_AssetUpdateManager_AssetBundleScript.m_OnAssetUpdateDone += OnAssetBundleUpdateComplete;
            m_AssetUpdateManager_AssetBundleScript.m_CompleteCheckAssetStateAct += OnAssetBundleCompareComplete;
            m_AssetUpdateManager_AssetBundleScript.m_OnDownLoadServerConfigureFailAct += OnDownLoadAssetBundleServerConfigureFail;
            m_AssetUpdateManager_AssetBundleScript.m_OnDownLoadAssetFailAct += OnDownloadAssetBundleAssetOutOfTime;
            m_AssetUpdateManager_AssetBundleScript.m_OnDownLoadAssetSuccessAct += OnAssetBundleAssetDownLoadSuccess;

            #endregion

            Debug.Log("先检测lua 脚本资源状态 再更新AssetBundle");
            m_UITestAssetUpdateView.ShowAssetStateInfor("正在检测资源版本");
            m_AssetUpdateManager_LuaScript.CheckLoadAssetUpdateState();
        }

        #endregion

        private void ShowAssetUpdateUI()
        {
            ResourcesMgr.GetInstance().LoadResourcesAsset("Prefabs/UI/TestAssetUpdatePanel", (go) =>
            {
                if (go == null)
                {
                    Debug.LogError("ShowAssetUpdateUI  Fail,Not Exit");
                    return;
                }
                GameObject uiPrefab = GameObject.Instantiate(go) as GameObject;
                m_UITestAssetUpdateView = uiPrefab.GetAddComponent<UITestAssetUpdateView>();
            });
        }

        #region Lua 更新
        /// <summary>
        /// 下载lua 服务器配置失败
        /// </summary>
        private void OnDownLoadLuaServerConfigureFail()
        {
            Debug.LogInfor("OnDownLoadLuaServerConfigureFail");
        }

        private void OnLuaAssetCompareComplete(int size)
        {
            Debug.Log("OnLuaAssetCompareComplete size=" + size);
            OnLuaOrABundleCompareComplete(size);
        }

        /// <summary>
        /// lua资源下载成功
        /// </summary>
        /// <param name="assetsize"></param>
        private void OnLuaAssetDownLoadSuccess(int assetsize)
        {
            m_UITestAssetUpdateView.OnDownLoadAssetCallBack(assetsize);

        }
        private void OnLuaAssetUpdateComplete(bool iscomplete)
        {
            if (iscomplete)
            {
                Debug.LogInfor("<<<lua 资源更新完成，卸载这个模块" + iscomplete);
                GameObject.Destroy(m_AssetUpdateManager_LuaScript.gameObject);
                m_AssetUpdateManager_LuaScript = null;

                OnAssetUpdateComplete();
            }
            else
            {
                Debug.LogError("OnLuaAssetUpdateComplete lua更新失败");
            }
        }

        /// <summary>
        /// 部分资源多次下载都失败了
        /// </summary>
        /// <param name="allFailAsset"></param>
        private void OnDownloadLuaAssetOutOfTime(List<string> allFailAsset)
        {
            Debug.LogError("部分lua 资源多次下载都失败了");
            //for (int dex = 0; dex < allFailAsset.Count; ++dex)
            //{
            //    Debug.Log("LuaAsset ::: " + allFailAsset[dex]);
            //}

            m_UITestAssetUpdateView.ShowAssetStateInfor("资源更新失败，请检查网络后重新启动");
            m_UITestAssetUpdateView.StopUpdateProcess(false);
        }

        #endregion

        #region AssetBundle

        private void OnDownLoadAssetBundleServerConfigureFail()
        {
            Debug.LogInfor("OnDownLoadAssetBundleServerConfigureFail");
        }

        /// <summary>
        /// 部分资源多次下载都失败了
        /// </summary>
        /// <param name="allFailAsset"></param>
        private void OnDownloadAssetBundleAssetOutOfTime(List<string> allFailAsset)
        {
            Debug.LogError("部分 AssetBundle 资源多次下载都失败了");
            //for (int dex = 0; dex < allFailAsset.Count; ++dex)
            //{
            //    Debug.Log("AssetBundle ::: " + allFailAsset[dex]);
            //}
            m_UITestAssetUpdateView.ShowAssetStateInfor("资源更新失败，请检查网络后重新启动");
            m_UITestAssetUpdateView.StopUpdateProcess(false);
        }

        /// <summary>
        /// AssetBundle 资源下载成功
        /// </summary>
        /// <param name="assetsize"></param>
        private void OnAssetBundleAssetDownLoadSuccess(int assetsize)
        {
            m_UITestAssetUpdateView.OnDownLoadAssetCallBack(assetsize);
        }

        private void OnAssetBundleCompareComplete(int size)
        {
            Debug.Log("OnAssetBundleCompareComplete size=" + size);
            OnLuaOrABundleCompareComplete(size);
        }
        private void OnAssetBundleUpdateComplete(bool iscomplete)
        {
            Debug.LogInfor("<<<OnAssetBundleUpdateComplete" + iscomplete);
            if (iscomplete)
            {
                GameObject.Destroy(m_AssetUpdateManager_AssetBundleScript.gameObject);
                m_AssetUpdateManager_AssetBundleScript = null;
                OnAssetUpdateComplete();
                Debug.LogInfor(">>更新AssetBundle 完成 卸载更新模块<<");
            }
            else
            {
                Debug.LogError("OnLuaAssetUpdateComplete AssetBundle 更新失败");
            }
        }
        #endregion

        #region 资源比对和更新后的回调

        /// <summary>
        /// 当lua 、AssetBudle 更新模块比对完成MD5 准备下载前
        /// 
        /// </summary>
        /// <param name="size"></param>
        private void OnLuaOrABundleCompareComplete(int size)
        {
            //Debug.Log("ccccccccccccccccccccccc size=" + size);
            m_TotalNeedDownLoadSize += size;
            if (m_AssetUpdateManager_AssetBundleScript.m_IsBegingAssetUpdate == false && m_AssetUpdateManager_LuaScript.m_IsBegingAssetUpdate)
            {
                m_AssetUpdateManager_AssetBundleScript.CheckLoadAssetUpdateState();
                Debug.LogInfor(">>>开始检测AssetBundle 资源状态");
                return;
            }
            if (m_AssetUpdateManager_LuaScript.m_IsBegingAssetUpdate == false || m_AssetUpdateManager_AssetBundleScript.m_IsBegingAssetUpdate == false)
            {
                Debug.LogError("OnLuaOrABundleCompareComplete  失败,lua资源状态不对");
                return;
            }
            if (m_AssetUpdateManager_LuaScript.m_IsCompleteMD5 && m_AssetUpdateManager_AssetBundleScript.m_IsCompleteMD5)
            {
                Debug.LogInfor(">>OnLuaOrABundleCompareComplete  BeginDownLoad  Total Size=" + m_TotalNeedDownLoadSize);
                m_UITestAssetUpdateView.ShowAssetStateInfor("资源版本校验完成..");
                EventCenter.GetInstance().DelayDoEnumerator(2, () =>
                {
                    m_UITestAssetUpdateView.ShowAssetStateInfor("开始更新下载资源..", m_TotalNeedDownLoadSize);
                    m_UITestAssetUpdateView.BeginUpdateProcess();

                    m_AssetUpdateManager_LuaScript.BeginUpdateAsset();
                    m_AssetUpdateManager_AssetBundleScript.BeginUpdateAsset();
                });
            }
        }
        /// <summary>
        /// 当lua 资源和 AssetBundle 资源都加载完成时候
        /// </summary>
        private void OnAssetUpdateComplete()
        {
            //Debug.LogInfor("AAAAAAAAAAAAAAAAAAAAAAA  " + (m_AssetUpdateManager_LuaScript == null) + "   " + (m_AssetUpdateManager_AssetBundleScript == null));
            if (m_AssetUpdateManager_LuaScript == null && m_AssetUpdateManager_AssetBundleScript == null)
            {
                Debug.LogInfor("OnAssetUpdateComplete  Done Asset Update>>>>>");
                m_UITestAssetUpdateView.ShowAssetStateInfor("资源已经更新完毕");
                m_UITestAssetUpdateView.StopUpdateProcess(true);
                m_IsAssetUpdating = false;

                luaHotFix();
                EventCenter.GetInstance().DelayDoEnumerator(0.5f, () =>
                {
                    GameObject.Destroy(m_UITestAssetUpdateView.gameObject);
                });
                if (OnAssetUpdateDoneAct != null)
                    OnAssetUpdateDoneAct();
            }
        }
        #endregion

        #region Lua 注入代码
        private void luaHotFix()
        {
            LuaEngine.AddLoader(luaMainCustomerLoader);
            LuaEngine.DoString(@"require 'luamain'");
            LuaEngine.DoString("main.luaMainEnter()");
        }

        /// <summary>
        /// 自定义加载luamain 文件的方法
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private byte[] luaMainCustomerLoader(ref string filepath)
        {
            byte[] data = null;
            filepath = "LuaAsset/" + filepath;
            ResourcesMgr.GetInstance().LoadAsset(filepath, (go) =>
            {
                if (go == null)
                {
                    Debug.LogError("LoadAsset Fail .....");
                    return;
                }
                TextAsset file = go as TextAsset;
                if (file != null)
                    data = file.bytes;
                else
                    data = null;
            });
            return data;
        }

        #endregion


    }
}