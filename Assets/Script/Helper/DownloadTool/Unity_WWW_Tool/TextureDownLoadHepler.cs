using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MFramework
{

    /// <summary>
    /// DownLoad CallHandler
    /// </summary>
    /// <param name="isSuccess">is WWW Down Success</param>
    /// <param name="url"></param>
    /// <param name="filePath">file Store Path</param>
    /// <param name="data"></param>
    /// <param name="downLoadAct"></param>
    public delegate void DownLoadTetureHandler(bool isSuccess, string url, string filePath, byte[] data, Action<string, string> downLoadAct);
    public class TextureDownLoadHepler : Singleton_Static<TextureDownLoadHepler>
    {
        [System.Serializable]
        /// <summary>
        /// ��������
        /// </summary>
        public class TextureDownLoadHeplerCacheData
        {
            public string Url;
            public Action<string, string> DownLoadAct;

            public TextureDownLoadHeplerCacheData(string url, Action<string, string> action)
            {
                Url = url;
                DownLoadAct = action;
            }
        }

        protected override void InitialSingleton() { }

        private int CurrentTaskCount = 0; //��ǰ��������������
        private int MaxTaskCount = 5;  //���������������

        /// <summary>
        /// �Ѿ���������ݺ�����
        /// </summary>
        private Dictionary<string, string> m_AllLocalCacheTexturesDic = new Dictionary<string, string>(); //Cache Dic
        //�����������򻺴�����    ���еȴ����ص�����
        private Queue<TextureDownLoadHeplerCacheData> m_WaitingList = new Queue<TextureDownLoadHeplerCacheData>();



        /// <summary>
        /// ����ͼƬ�ӿ�
        /// </summary>
        /// <param name="url"></param>
        /// <param name="downLoadAct">���سɹ�����ʧ�ܺ�Ļص�</param>
        public void DownLoadTexture(string url, Action<string, string> downLoadAct)
        {
            #region Url ���

            if (string.IsNullOrEmpty(url))
            {
                if (downLoadAct != null) downLoadAct("", url);
                Debug.LogError("Url Should Not Be Null or Empty ");
                return;
            }

            if (url.StartsWith("http") == false) url = "http://" + url;  //�ֻ��˱����� http:// ��ʼ�����޷�ʶ���ַ
            #endregion

            if (m_AllLocalCacheTexturesDic.ContainsKey(url))
            {
//#if UNITY_EDITOR
//                DataShow_TextureDownLoad.Insance.FlushView();
//#endif
                Debug.Log("The File Is Exit ,Load Cache Image >>>  " + url);
                if (downLoadAct != null) downLoadAct(m_AllLocalCacheTexturesDic[url], url);  //�����Ѿ�������
                return;
            }

            if (CurrentTaskCount >= MaxTaskCount)
            {
                //#if UNITY_EDITOR
                //                DataShow_TextureDownLoad.Insance.FlushView();
                //#endif
                Debug.Log("Delay DownLoad .... DelayList Count=" + m_WaitingList.Count);
                m_WaitingList.Enqueue(new TextureDownLoadHeplerCacheData(url, downLoadAct));
                return;
            } //�������������ʱ�� ��������

            //DownLoad Texture
            string fileFullPath = FileDirHelper.GetUrlFileDir(url);
            fileFullPath = GetLocalCachePath(fileFullPath);
            Debug.Log("Start DownLoad " + url + "     fileFullPath=" + fileFullPath);

            ++CurrentTaskCount;
            EventCenter.GetInstance().StartCoroutine(EventCenter.GetInstance().DownLoadAction(url, fileFullPath, downLoadAct, DownLoad));

        }

        private void DownLoad(bool isSuccess, string url, string filePath, byte[] data, Action<string, string> downLoadAct)
        {
//#if UNITY_EDITOR
//            DataShow_TextureDownLoad.Insance.FlushView();
//#endif
            --CurrentTaskCount;
            try
            {
                if (false == isSuccess)
                {
                    if (downLoadAct != null)
                        downLoadAct("", url);
                    return;
                }

                if (m_AllLocalCacheTexturesDic.ContainsKey(url) == false)
                {//Save To Dictionary
                    Debug.Log("Save To Local Cache " + url);
                    m_AllLocalCacheTexturesDic.Add(url, filePath);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    System.IO.File.WriteAllBytes(filePath, data);
                }

                if (downLoadAct != null) downLoadAct(filePath, url);
            }
            catch (Exception e) { Debug.LogError("DownLoad Fail ,Exception: " + e.Message); }
            finally
            {
                Debug.Log("final .... downPreviousData Count=" + m_WaitingList.Count);
                if (m_WaitingList.Count > 0)
                {
                    if (CurrentTaskCount < MaxTaskCount)
                    {
                        TextureDownLoadHeplerCacheData down = m_WaitingList.Dequeue();
                        DownLoadTexture(down.Url, down.DownLoadAct);  //���ػ��������
                    }//if
                }//if
            }//finally
        }


        //Save The File To Cache File
        string GetLocalCachePath(string filePath)
        {
            string storePath = ConstDefine.TextureCachePath + System.IO.Path.GetDirectoryName(filePath);
            Debug.Log("filePath=" + filePath + "    storePath=" + storePath);
            if (System.IO.Directory.Exists(storePath) == false)
                System.IO.Directory.CreateDirectory(storePath);

            return ConstDefine.TextureCachePath + filePath;
        }


        #region Unity Editor Test

#if UNITY_EDITOR
        public  List<TextureDownLoadHeplerCacheData> GetAllWaitingList()
        {
            List<TextureDownLoadHeplerCacheData> result = new List<TextureDownLoadHeplerCacheData>();
            foreach (var item in m_WaitingList)
                result.Add(item);
            return result;
        }

        public  List<string> GetAllDownLoadedList()
        {
            List<string> result = new List<string>();
            foreach (var item in m_AllLocalCacheTexturesDic)
                result.Add(item.Key);

            Debug.Log("GetAllDownLoadedList..... " + result.Count);
            return result;
        }

#endif

        #endregion



    }
}


