using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// ʹ��Unity ���õ�WWW���� ���ҿ���������������������
    /// </summary>
    public class DownLoadHelper_WWW
    {
        /// <summary>
        /// ��������
        /// </summary>
        private class DownLoadHelper_WWWCacheData
        {
            public string Url;
            public bool IsNeedCheckUrl = false;
            public Action<WWW, string> downLoadAct;

            public DownLoadHelper_WWWCacheData(string url, Action<WWW, string> action,bool isNeedCheck)
            {
                Url = url;
                downLoadAct = action;
                IsNeedCheckUrl = isNeedCheck;
            }
        }

        //�����������򻺴�����    ���еȴ����ص�����
        private static Queue<DownLoadHelper_WWWCacheData> m_WaitingList = new Queue<DownLoadHelper_WWWCacheData>();


        public static int CurrentTaskCount = 0;
        private static int MaxTaskCount = 2;   //ͬһʱ������������������

        /// <summary>
        /// ������Դ 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <param name="needCheckUrl">�Ƿ���Ҫ���Url �ԡ�http://�� ��ͷ</param>
        public static void DownLoadWithOutSaveLocal(string url, Action<WWW, string> callback,bool needCheckUrl)
        {
            if (string.IsNullOrEmpty(url))
            {
                if (callback != null) callback(null, url);

                Debug.LogError("Url Should Not Be Null ");
                return;
            }

            if (needCheckUrl&&url.StartsWith("http") == false)
                url = "http://" + url;

            if (CurrentTaskCount >= MaxTaskCount)
            {
                //Debug.Log("Delay DownLoad .... DelayList Count=" + m_WaitingList.Count);
                m_WaitingList.Enqueue(new DownLoadHelper_WWWCacheData(url, callback, needCheckUrl));
                return;
            }

            EventCenter.GetInstance().StartCoroutine(DownLoad(url, callback));
        }


        static IEnumerator DownLoad(string url, Action<WWW, string> callback)
        {
            ++CurrentTaskCount;
            WWW ww = new WWW(url);
            yield return ww;
            if (string.IsNullOrEmpty(ww.error) == false)
            {
                Debug.LogError("DownLoad Fail " + ww.error+ "    url="+ url);
                if (callback != null) callback(null, url);
                yield return null;
                ww.Dispose();
                --CurrentTaskCount;
                DownCacheTask();
                yield break;
            }

            //Debug.Log("DownLoad success  " + ww.url);
            if (callback != null) callback(ww, url);
            yield return null;
            ww.Dispose();

            --CurrentTaskCount;
            DownCacheTask();
            yield break;

        }


        static void DownCacheTask()
        {
            if (m_WaitingList.Count != 0)
            {
                DownLoadHelper_WWWCacheData down = m_WaitingList.Dequeue();
                //Debug.Log("Down Cache Data " + m_WaitingList.Count + "    down.Url=" + down.Url);
                DownLoadWithOutSaveLocal(down.Url, down.downLoadAct,down.IsNeedCheckUrl);  //���ػ��������
            }
        }


    }
}
