using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MFramework
{
    public class EventCenter : Singleton_Mono<EventCenter>
    {
        public enum UpdateRate
        {
            NormalFrame = 1,
            DelayOneFrame = 2,
            DelayTwooFrame = 3,
        }

        [HideInInspector]
        public Transform m_UIRoot = null;  //UI
        [HideInInspector]
        public Transform m_UICanvasTrans = null;

        public delegate void UpdateHandle();
        List<UpdateHandle> m_AllUpdateEvent_Normal = new List<UpdateHandle>();
        List<UpdateHandle> m_AllUpdateEvent_OneFrameDelay = new List<UpdateHandle>();
        List<UpdateHandle> m_AllUpdateEvent_TwoFrameDelay = new List<UpdateHandle>();

        //FixedUpodate
        List<UpdateHandle> m_AllFixedUpdateEvent_OneFrameDelay = new List<UpdateHandle>();
        List<UpdateHandle> m_AllFixedUpdateEvent_TwoFrameDelay = new List<UpdateHandle>();


        protected override void Awake()
        {
            base.Awake();
            Loom.GetInstance();
        }

        #region UpdateEvent
        int countTime = 0;
        void Update()
        {
            if (m_AllUpdateEvent_Normal.Count > 0)
            {//Normal
                ExcuteEvent(ref m_AllUpdateEvent_Normal);
            }//if

            ++countTime;
            if (countTime % 2 == 0)
            {//OneFrame Dlaye
                if (m_AllUpdateEvent_OneFrameDelay.Count != 0)
                    ExcuteEvent(ref m_AllUpdateEvent_OneFrameDelay);
            }
            else if (countTime % 3 == 0)
            {//Tworame Dlaye
                if (m_AllUpdateEvent_TwoFrameDelay.Count != 0)
                    ExcuteEvent(ref m_AllUpdateEvent_TwoFrameDelay);
            }
        }

        void ExcuteEvent(ref List<UpdateHandle> listData)
        {
            if (listData == null || listData.Count == 0) return;

            for (int _dex = 0; _dex < listData.Count; ++_dex)
            {
                if (listData[_dex] != null)
                    listData[_dex]();
            }
        }

        public void AddUpdateEvent(UpdateHandle handle, UpdateRate _rate)
        {
            if (_rate == UpdateRate.NormalFrame)
            {
                if (m_AllUpdateEvent_Normal.Contains(handle))
                {
                    //  Debug.Log("Regist Again " + handle.GetHashCode());
                    return;
                }
                //  Debug.Log("Regist " + handle.GetHashCode());
                m_AllUpdateEvent_Normal.Add(handle);
            }//if
            else if (_rate == UpdateRate.DelayOneFrame)
            {
                if (m_AllUpdateEvent_OneFrameDelay.Contains(handle)) return;
                m_AllUpdateEvent_OneFrameDelay.Add(handle);
            }//if
            else if (_rate == UpdateRate.DelayTwooFrame)
            {
                if (m_AllUpdateEvent_TwoFrameDelay.Contains(handle)) return;
                m_AllUpdateEvent_TwoFrameDelay.Add(handle);
            }//if
        }

        public void RemoveUpdateEvent(UpdateHandle handle, UpdateRate _rate)
        {
            if (m_AllUpdateEvent_Normal.Contains(handle))
            {
                //  Debug.Log("Cancle Regist " + handle.GetHashCode());
                m_AllUpdateEvent_Normal.Remove(handle);
                return;
            }
            //Debug.Log("Cancle  22 Regist    ���� " + handle.GetHashCode());
            if (m_AllUpdateEvent_OneFrameDelay.Contains(handle))
            {
                m_AllUpdateEvent_OneFrameDelay.Remove(handle);
                return;
            }

            if (m_AllUpdateEvent_TwoFrameDelay.Contains(handle))
            {
                m_AllUpdateEvent_TwoFrameDelay.Remove(handle);
                return;
            }

        }

        #endregion

        #region FixedUpdate
        int fixedCountTime = 0;
        private void FixedUpdate()
        {
            ++fixedCountTime;
            if (fixedCountTime % 2 == 0)
            {//OneFrame Dlaye
                if (m_AllFixedUpdateEvent_OneFrameDelay.Count != 0)
                    ExcuteEvent(ref m_AllFixedUpdateEvent_OneFrameDelay);
            }
            else if (fixedCountTime % 3 == 0)
            {//Tworame Dlaye
                if (m_AllFixedUpdateEvent_TwoFrameDelay.Count != 0)
                    ExcuteEvent(ref m_AllFixedUpdateEvent_TwoFrameDelay);
            }
        }


        public void AddFixedUpdateEvent(UpdateHandle handle, UpdateRate _rate)
        {
            if (_rate == UpdateRate.NormalFrame)
            {
                return;
            }//if

            if (_rate == UpdateRate.DelayOneFrame)
            {
                if (m_AllFixedUpdateEvent_OneFrameDelay.Contains(handle)) return;
                m_AllFixedUpdateEvent_OneFrameDelay.Add(handle);
                return;
            }//if

            if (_rate == UpdateRate.DelayTwooFrame)
            {
                if (m_AllFixedUpdateEvent_TwoFrameDelay.Contains(handle)) return;
                m_AllFixedUpdateEvent_TwoFrameDelay.Add(handle);
                return;
            }//if
        }

        public void RemoveFixedUpdateEvent(UpdateHandle handle, UpdateRate _rate)
        {
            if (m_AllUpdateEvent_Normal.Contains(handle))
            {
                return;
            }

            if (m_AllFixedUpdateEvent_OneFrameDelay.Contains(handle))
            {
                m_AllFixedUpdateEvent_OneFrameDelay.Remove(handle);
                return;
            }

            if (m_AllFixedUpdateEvent_TwoFrameDelay.Contains(handle))
            {
                m_AllFixedUpdateEvent_TwoFrameDelay.Remove(handle);
                return;
            }

        }

        #endregion


        #region Enumerator Action
        /// <summary>
        /// �ӳ�һ��ʱ��֮��ִ�в���
        /// </summary>
        /// <param name="time"></param>
        /// <param name="action"></param>
        public void DelayDoEnumerator(float time, System.Action action)
        {
            StartCoroutine(DelayDoAction(time, action));
        }
         IEnumerator DelayDoAction(float time, System.Action action)
        {
            yield return new WaitForSeconds(time);
            if (action != null)
                action();
        }



        /// <summary>
        /// ��ǰ֡���ʱ��ִ�в���
        /// Action will do At The Last of The Frame  
        /// </summary>
        /// <param name="action"></param>
        public void CurrentFameLastDoAction(System.Action action)
        {
            StartCoroutine(FramLastDoAction(action));
        }
         IEnumerator FramLastDoAction(System.Action action)
        {
            yield return new WaitForEndOfFrame();
            if (action != null)
                action();
        }

        

        /// <summary>
        /// ��һִ֡�в���
        /// </summary>
        /// <param name="action"></param>
        public void NextFrameDoAction(System.Action action)
        {
            StartCoroutine(NextFramDoAction(action));
        }
        IEnumerator NextFramDoAction(System.Action action)
        {
            yield return null;
            if (action != null)
                action();
        }




        /// <summary>
        /// DownLoad Texture Action
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <param name="finishDownAct"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator DownLoadAction(string url, string filePath, Action<string, string> finishDownAct, DownLoadTetureHandler action)
        {
            //++TextureDownLoadHepler.CurrentTaskCount;
            yield return null;
            WWW ww = new WWW(url);
            yield return ww;
            //  --TextureDownLoadHepler.CurrentTaskCount;
            if (ww.isDone && string.IsNullOrEmpty(ww.error))
            {
                Debug.Log("WWW DownLoad Success");
                byte[] data;
                yield return data = ww.texture.EncodeToJPG(); ;

                if (action != null)
                    action(true, url, filePath, data, finishDownAct);
                yield break;
            }

            Debug.LogError("Texture DownLoad Fail!!! url=" + url + "   error=" + ww.error);
            if (action != null)
                action(false, url, filePath, null, finishDownAct);

        }


        #endregion



    }
}
