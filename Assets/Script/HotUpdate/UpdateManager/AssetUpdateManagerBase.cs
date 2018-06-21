using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MFramework.Re
{
    /// <summary>
    /// 资源更新的基类 处理如何更新资源
    /// </summary>
    public abstract class AssetUpdateManagerBase : MonoBehaviour
    {
        protected HotAssetRecordInfor m_LocalAssetRecord = null; //本地配置的 资源 信息
        protected HotAssetRecordInfor m_ServerAssetRecord = null;//服务器配置的 资源 信息

        protected string m_ServerConfigureStr = ""; //服务器的资源配置表 会写到本地去

        protected List<string> m_AllNeedUpdateAssetPath = new List<string>(); //所有需要下载的 资源 包名  也包含下载失败时候 缓存的需要重新下载的资源列表
        protected Dictionary<string, int> m_AllDownLoadFailAssetRecord = new Dictionary<string, int>();    //记录所有下载失败的案例    //Valua 记录下载的次数
        protected Dictionary<string, bool> m_AllCompleteDownLoadAssetDic = new Dictionary<string, bool>(); //所有已经正确下载保存到本地的子  资源 。value标识是否已经被更新到本地的配置文件中


        protected int m_AllNeedDownLoadAssetCount = 0; //所有需要下载的资源总数
        protected int m_CurrentDownLoadCount = 0;//当前下载的AB资源数量
        protected int m_TotalDownLoadAssetCount = 0;   //下载成功的资源数量
        protected static int S_DownLoadCoutToRecord = 5;  //当下载的资源数量是5的整数倍则开始保存记录

        protected int m_MaxTryDownLoadTime = 3; //下载失败最大尝试次数

        public bool m_IsBegingAssetUpdate { protected set; get; }  //标识是否已经开始更新当前种类的资源

        protected bool m_IsUpdateRecorded = false;  //标识是否已经更新过记录文件
        public bool m_IsCompleteUpdate { private set; get; }   //只有当所有的资源下载完成时候才是True
        public bool m_IsCompleteMD5 { private set; get; }   //只有当所有的资源md5 码比对完成
        public bool m_IsDownLoadServerConfigure { private set; get; }   //是否成功下载了服务器的配置文件 


        public Action m_OnDownLoadServerConfigureFailAct = null;  //当下载服务器的配置文件失败时候
        public Action<int> m_CompleteCheckAssetStateAct = null;  //比对完服务器和本地  资源的版本
        public Action<List<string>> m_OnDownLoadAssetFailAct = null;  //当下载资源失败时候调用
        public Action<bool> m_OnAssetUpdateDone = null; //当资源已经下载完成时  参数标识是否完成整个更新
        public Action<int> m_OnDownLoadAssetSuccessAct = null; //下载保存资源成功  主要用于返回下载速度

        protected virtual string m_ServerAseetPath { get; set; }  //服务器  资源 下载的顶层目录
        protected virtual string m_AssetConfigurePath { get; set; }  //从服务器下载的资源的相对路径  ConstDefine.AssetBundleConfigurePath
        protected virtual string m_AssetSaveTopPath { get; set; }  //从服务器下载的资源的到本地的顶层目录  ConstDefine.ABundleTopPath


        private Dictionary<string, int> m_TestDownloadRecordDic = new Dictionary<string, int>();

        protected virtual void Awake()
        {
            m_IsBegingAssetUpdate = false;
            InitialState();
        }

        protected virtual void OnDestroy()
        {
            if (m_IsUpdateRecorded == false)
                UpdateLocalRecordConfigureText(true);  //避免某些情况下 没有刷新本地的配置文件


            m_OnDownLoadServerConfigureFailAct = null;
            m_CompleteCheckAssetStateAct = null;
            m_OnDownLoadAssetFailAct = null;
            m_OnAssetUpdateDone = null;

            InitialState();
        }

        public void CheckLoadAssetUpdateState()
        {
            m_IsBegingAssetUpdate = true;
            InitialState();
            GetLocalAseetConfigureRecordText();
            StartCoroutine(GetServerAssetConfigureRecordText(CheckLocalAbundleNeedUpdateRecord));
        }


        /// <summary>
        /// 重置状态
        /// </summary>
        protected virtual void InitialState()
        {
            m_TotalDownLoadAssetCount = 0;
            m_AllNeedDownLoadAssetCount = m_CurrentDownLoadCount = 0;

            m_AllDownLoadFailAssetRecord.Clear();
            m_AllNeedUpdateAssetPath.Clear();
            m_AllCompleteDownLoadAssetDic.Clear();

            m_LocalAssetRecord = m_ServerAssetRecord = null;

            m_IsCompleteMD5 = m_IsDownLoadServerConfigure = false;
            m_IsCompleteUpdate = false;
            m_IsUpdateRecorded = false;

            if (Directory.Exists(m_AssetSaveTopPath) == false)
                Directory.CreateDirectory(m_AssetSaveTopPath);  //ABundle 资源目录初始化
        }

        #region  读取本地配置文件

        /// <summary>
        /// 加载本地的AB资源配置文件
        /// </summary>
        protected void GetLocalAseetConfigureRecordText()
        {
            string assetText = "";
            string filePath = m_AssetSaveTopPath + m_AssetConfigurePath + ".txt";
            //****首先读取外部的配置文件
            if (File.Exists(filePath))
            {
                Debug.LogInfor("GetLocalAseetConfigureRecordText  is OutStorage directory");
                assetText = File.ReadAllText(filePath);
            }
            else
            {
                Debug.LogInfor("GetLocalAseetConfigureRecordText  is Resources directory");
                TextAsset textAsset = Resources.Load<TextAsset>(m_AssetConfigurePath);
                if (textAsset == null)
                {
                    Debug.LogInfor("Resource Configure Not Exit");
                    m_LocalAssetRecord = null;
                    return;
                }
                assetText = textAsset.text;
            }//随包的  配置文件

            m_LocalAssetRecord = JsonMapper.ToObject<HotAssetRecordInfor>(assetText);
        }
        #endregion


        #region  下载并解析服务器的配置文件
        /// <summary>
        /// 服务器的AssetBundle 配置
        /// </summary>
        protected virtual IEnumerator GetServerAssetConfigureRecordText(System.Action callback)
        {
            Debug.LogInfor("GetServerABundleRecordText");
            WWW ww = new WWW(m_ServerAseetPath + m_AssetConfigurePath + ".txt");
            yield return ww;
            if (string.IsNullOrEmpty(ww.error) == false)
            {
                Debug.LogError("GetServerABundleRecordText Fail  Error: " + ww.error);
                OnDownLoadServerConfigFail();
                yield break;
            }
            m_ServerConfigureStr = ww.text;  //保存配置文件
            m_ServerAssetRecord = JsonMapper.ToObject<HotAssetRecordInfor>(ww.text);
            if (m_ServerAssetRecord == null)
            {
                Debug.LogError("Server ABInfor Can't Identify");
                OnDownLoadServerConfigFail();
                yield break;
            }
            if (callback != null) callback();
        }

        /// <summary>
        /// 当下载服务器的配置文件失败时候
        /// </summary>
        /// <param name="ww"></param>
        protected virtual void OnDownLoadServerConfigFail()
        {
            Debug.LogError("OnDownLoadServerConfigFail......");
            m_IsDownLoadServerConfigure = false;
            if (m_OnDownLoadServerConfigureFailAct != null)
                m_OnDownLoadServerConfigureFailAct();
        }


        #endregion


        #region 检查  Asset 是否需要更新
        /// <summary>
        /// 对比MD5 获取那些文件需要更新下载
        /// </summary>
        protected virtual void CheckLocalAbundleNeedUpdateRecord()
        {
            Debug.LogInfor("GetNeedUpdateRecourRecord");
            int totalNeedDownLoadSize = 0;
            m_IsDownLoadServerConfigure = true;

            HotAssetInfor checkABInfor = null;
            foreach (var item in m_ServerAssetRecord.m_AllAssetRecordsDic)
            {
                if (m_LocalAssetRecord != null && m_LocalAssetRecord.m_AllAssetRecordsDic.TryGetValue(item.Key, out checkABInfor))
                {
                    if (item.Value.m_MD5Code != checkABInfor.m_MD5Code)
                    {
                     //   Debug.Log(" Need Update::  " + item.Key + "   MD5=" + item.Value.m_MD5Code + ":::" + checkABInfor.m_MD5Code + "  size==" + item.Value.m_ByteSize);
                        RecordNeedUpdateAssetState(GetAssetDownLoadPath(item.Key), true);
                        totalNeedDownLoadSize += item.Value.m_ByteSize;
                        continue;
                    }  //资源MD5不一致
                }
                else
                {
                    //Debug.Log(" Need DownLoad:: " + item.Key+"  size=="+item.Value.m_ByteSize);
                    RecordNeedUpdateAssetState(GetAssetDownLoadPath(item.Key), true);
                    totalNeedDownLoadSize += item.Value.m_ByteSize;
                }//新的资源
            }

            m_IsCompleteMD5 = true;

            if (m_CompleteCheckAssetStateAct != null)
                m_CompleteCheckAssetStateAct(totalNeedDownLoadSize);

        }


        /// <summary>
        /// 根据 Asset 名获取下载时候的路径
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        protected virtual string GetAssetDownLoadPath(string assetName)
        {
            return "";
            //  return m_ServerABundlePath + ConstDefine.ABundleTopFileNameOfPlatformat + "/" + assetBundleName;
        }
        #endregion


        #region 开始更新下载资源

        /// <summary>
        /// 开始更新下载资源
        /// </summary>
        public virtual void BeginUpdateAsset()
        {
            if (m_IsCompleteMD5 == false || m_IsDownLoadServerConfigure == false)
            {
                Debug.LogError("BeginUpdateAsset Fail " + (m_IsDownLoadServerConfigure == false));
                return;
            }

            if (m_AllNeedUpdateAssetPath.Count == 0)
            {
                OnFinishDownLoadAsset();
                return;
            } //所有的资源已经是最新的
            OnTryDownLoadAsset(OnDownloadAssetCallBack);
        }

        /// <summary>
        /// 下载 Asset 资源回调
        /// </summary>
        /// <param name="ww"></param>
        /// <param name="url"></param>
        protected virtual void OnDownloadAssetCallBack(WWW ww, string url)
        {
            OnTrySaveDownloadAsset(ww, url);
            if (ww != null)
            {
                m_AllCompleteDownLoadAssetDic.Add(GetAssetRelativePathByUrl(url), false);  //记录当前 AssetBundle 已经下载完成
            }
            CheckWhetherNeedReLoad();
        }

        /// <summary>
        /// 从下载完成回调中获取当前  的相对路径名以便于记录
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected virtual string GetAssetRelativePathByUrl(string url)
        {
            //string abundleDwonPath = m_ServerABundlePath + ConstDefine.ABundleTopFileNameOfPlatformat + "/";
            //int index = url.IndexOf(abundleDwonPath);
            //if (index == -1)
            //{
            //    Debug.LogError("GetABundlePathByUrl Fail url=" + url);
            //    return "";
            //}

            ////Debug.Log(url.Substring(index + abundleDwonPath.Length));
            //return url.Substring(index + abundleDwonPath.Length);

            return "";
        }

        /// <summary>
        /// 检测是否需要重新下载部分资源
        /// </summary>
        protected virtual void CheckWhetherNeedReLoad()
        {
            if (m_CurrentDownLoadCount != m_AllNeedDownLoadAssetCount) return;  //等待本次下载任务完成

            Debug.LogInfor("CheckWhetherNeedReLoad...." + m_AllDownLoadFailAssetRecord.Count);
            if (m_AllDownLoadFailAssetRecord.Count == 0)
            {
                OnFinishDownLoadAsset();
                Debug.Log("CheckWhetherNeedReLoad  Complete");
                return;
            }

            List<string> allFailDownloadAsset = new List<string>();
            foreach (var item in m_AllDownLoadFailAssetRecord)
            {
                if (item.Value >= m_MaxTryDownLoadTime)
                {
                    allFailDownloadAsset.Add(item.Key);
                }
                else
                {
                    Debug.LogInfor(">>下载子ABundle 资源失败 ，正在重新下载 " + item.Key);
                    RecordNeedUpdateAssetState(GetAssetDownLoadPath(item.Key), true);
                }
            }

            if (allFailDownloadAsset.Count != 0)
            {
                Debug.LogError("OnDownLoadMainABundleCallBack  有资源下载多次失败 本次下载失败");
                UpdateLocalRecordConfigureText(true);  //这里强制写入一次配置文件 避免由于部分资源下载失败而没有记录的问题
                if (m_OnDownLoadAssetFailAct != null)
                    m_OnDownLoadAssetFailAct(allFailDownloadAsset);

                return;
            }


            if (m_AllNeedUpdateAssetPath.Count > 0)
                OnTryDownLoadAsset(OnDownloadAssetCallBack);
        }

        #endregion



        #region 下载 Asset 以及回调处理  保存下载的资源
        /// <summary>
        /// 下载新的资源
        /// </summary>
        protected void OnTryDownLoadAsset(Action<WWW, string> downLoadCallback)
        {
            Debug.LogInfor("OnTryDownLoadAsset Count=" + m_AllNeedUpdateAssetPath.Count);
            m_AllNeedDownLoadAssetCount = m_AllNeedUpdateAssetPath.Count;
            m_CurrentDownLoadCount = 0; //重置标识位

            for (int index = 0; index < m_AllNeedUpdateAssetPath.Count; ++index)
            {
                DownLoadHelper_WWW.DownLoadWithOutSaveLocal(m_AllNeedUpdateAssetPath[index], downLoadCallback, false);
            }
            m_AllNeedUpdateAssetPath.Clear();
        }

        /// <summary>
        /// 处理下载回调  保存下载的资源
        /// </summary>
        /// <param name="ww"></param>
        /// <param name="url"></param>
        protected void OnTrySaveDownloadAsset(WWW ww, string url)
        {
            ++m_CurrentDownLoadCount;
            if (ww==null|| string.IsNullOrEmpty(ww.error) == false)
            {
                string path = GetAssetRelativePathByUrl(url);
                //Debug.Log("AAAAAAAAAAAAAAAAA PATH=" + path);
                Debug.LogError(string.Format("下载失败URL {0}", path) );
                RecordNeedUpdateAssetState(path, false);
                return;
            }
            int index = url.IndexOf(m_ServerAseetPath);
            if (index == -1)
            {
                Debug.LogError("OnTrySaveDownloadAsset  Fail,无法解析的地址 " + url);
                return;
            }

            try
            {
                ++m_TotalDownLoadAssetCount;
                string Relativepath = url.Substring(index + m_ServerAseetPath.Length);  //获取资源的相对路径
                string path = m_AssetSaveTopPath + Relativepath;

                Debug.LogInfor(Relativepath + "    本地保存的Asset  path=" + path);
                if (File.Exists(path))
                    File.Delete(path);   //删除旧的文件

                string DictionaryPath = Path.GetDirectoryName(path);
                if (Directory.Exists(DictionaryPath) == false)
                    Directory.CreateDirectory(DictionaryPath);  //创建路径

                File.WriteAllBytes(path, ww.bytes);  //保存资源

                OnDownLoadSuccess(GetAssetRelativePathByUrl(url), ww.bytes.Length); //下载成功   去除下载失败的记录
                UpdateLocalRecordConfigureText(false);

                m_TestDownloadRecordDic.Add(GetAssetRelativePathByUrl(url), ww.bytes.Length);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("OnDownLoadCallBack Exception:  " + ex.ToString());
            }
        }
        #endregion

        #region 更新本地配置文件
        /// <summary>
        /// 更新本地的配置文件  防止由于意外下载完成一半而导致部分 Asset 已经更新但是配置文件没有更新的问题
        /// </summary>
        /// <param name="forceRecord">是否忽略下载的文件个数限制</param>
        protected virtual void UpdateLocalRecordConfigureText(bool forceRecord)
        {
            if (m_IsCompleteUpdate || m_AllCompleteDownLoadAssetDic.Count == 0)
            {
                //Debug.LogInfor("UpdateLocalRecordConfigureText   No Need");
                return;
            }//已经完成正常的更新流程

            if (m_ServerAssetRecord == null)
            {
                Debug.LogError("UpdateLocalRecordConfigureText Fail ,Server Configure is Null");
                return;
            }//服务器的配置文件不存在

            if (forceRecord == false && m_TotalDownLoadAssetCount % S_DownLoadCoutToRecord != 0)
                return;   //避免频繁写入文件

            m_IsUpdateRecorded = true;

            if (m_LocalAssetRecord == null)
            {
                m_LocalAssetRecord = new HotAssetRecordInfor();
            }

            #region  对比更新本地配置文件数据
            HotAssetInfor bundleInfor = null;
            List<string> allNewRecordKeys = new List<string>();
            foreach (var abundleRecord in m_AllCompleteDownLoadAssetDic)
            {
                if (abundleRecord.Value)
                    continue;  //已经被记录到本地了

                if (m_LocalAssetRecord.m_AllAssetRecordsDic.TryGetValue(abundleRecord.Key, out bundleInfor))
                {
                    if (bundleInfor.m_MD5Code != m_ServerAssetRecord.m_AllAssetRecordsDic[abundleRecord.Key].m_MD5Code)
                    {
                        //   Debug.LogInfor("Need Update AssetBundle :" + abundleRecord.Key);
                        m_LocalAssetRecord.m_AllAssetRecordsDic[abundleRecord.Key] = m_ServerAssetRecord.m_AllAssetRecordsDic[abundleRecord.Key];
                        allNewRecordKeys.Add(abundleRecord.Key);
                        continue;
                    }
                }
                else
                {
                    //Debug.LogInfor("New Add AssetBundle  :: " + abundleRecord.Key);
                    if (m_ServerAssetRecord.m_AllAssetRecordsDic.ContainsKey(abundleRecord.Key))
                    {
                        m_LocalAssetRecord.m_AllAssetRecordsDic.Add(abundleRecord.Key, m_ServerAssetRecord.m_AllAssetRecordsDic[abundleRecord.Key]);
                        allNewRecordKeys.Add(abundleRecord.Key);
                    }
                    continue;
                }
            }

            for (int dex = 0; dex < allNewRecordKeys.Count; ++dex)
            {
                m_AllCompleteDownLoadAssetDic[allNewRecordKeys[dex]] = true;
            }
            allNewRecordKeys.Clear();

            #endregion

            //***********更新本地配置文件
            if (File.Exists(m_AssetSaveTopPath + m_AssetConfigurePath))
                File.Delete(m_AssetSaveTopPath + m_AssetConfigurePath);
            File.WriteAllText(m_AssetSaveTopPath + m_AssetConfigurePath + ".txt", JsonMapper.ToJson(m_LocalAssetRecord));  //更新配置文件

        }
        #endregion


        #region 更新完成

        /// <summary>
        /// 所有的资源下载完成 
        /// </summary>
        protected virtual void OnFinishDownLoadAsset()
        {
            Debug.LogInfor("OnFinishDownLoadAB");
            m_IsCompleteUpdate = true;
            string filePath = m_AssetSaveTopPath + m_AssetConfigurePath + ".txt";
            if (File.Exists(filePath))
                File.Delete(filePath);

            File.WriteAllText(filePath, m_ServerConfigureStr);  //更新配置文件
            m_AllCompleteDownLoadAssetDic.Clear();

//#if UNITY_EDITOR
//            Debug.Log("______________________________________Begin Download Check");
//            foreach (var item in m_ServerAssetRecord.m_AllAssetRecordsDic)
//            {
//                if(m_TestDownloadRecordDic.ContainsKey(item.Key)==false)
//                {
//                    Debug.LogError("ddddddddddddddddddddddd " + item.Key);
//                }
//                else
//                {
//                    if(item.Value.m_ByteSize!= m_TestDownloadRecordDic[item.Key])
//                    {
//                        Debug.LogError("ccccccccccccccccccccc  " + item.Key+"  Old ="+item.Value.m_ByteSize+"  New="+ m_TestDownloadRecordDic[item.Key]);
//                    }
//                }
//            }

//            Debug.Log("______________________________________End");

//#endif

            if (m_OnAssetUpdateDone != null)
                m_OnAssetUpdateDone(m_IsCompleteUpdate);
        }
        #endregion

        #region 记录下载的文件的状态

        /// <summary>
        /// 检测当前资源是否能够加入到下载队列
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected void RecordNeedUpdateAssetState(string path, bool isBeginDownOrDownLoadFail)
        {
            if (isBeginDownOrDownLoadFail == false)
            {
                if (m_AllDownLoadFailAssetRecord.ContainsKey(path))
                    ++m_AllDownLoadFailAssetRecord[path];
                else
                    m_AllDownLoadFailAssetRecord.Add(path, 1);

                return;
            } //下载失败的时候记录状态

            if (m_AllDownLoadFailAssetRecord.ContainsKey(path))
            {
                if (m_AllDownLoadFailAssetRecord[path] < m_MaxTryDownLoadTime)
                {
                    m_AllNeedUpdateAssetPath.Add(path);
                    return;
                }
                else
                {
                    Debug.Log(string.Format("RecordNeedUpdateAssetState  资源 {0},已经下载失败{1}次", path, m_MaxTryDownLoadTime));
                    return;
                }
            } //当前资源已经下载失败过

            m_AllNeedUpdateAssetPath.Add(path);
        }
        protected void OnDownLoadSuccess(string path,int assetSize)
        {
            //Debug.Log("AAAAAAA  path" + path + "          assetSize= " + assetSize);
            if (m_OnDownLoadAssetSuccessAct != null)
                m_OnDownLoadAssetSuccessAct(assetSize);

            if (m_AllDownLoadFailAssetRecord.ContainsKey(path))
            {
                m_AllDownLoadFailAssetRecord.Remove(path);
                Debug.Log("有一些资源重新下载成功 " + path);
            }
        }
        #endregion


    }
}