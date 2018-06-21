using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MFramework
{
    /// <summary>
    /// 精灵帧动画管理器
    /// </summary>
    public class SpriteSequenceUtility : Singleton_Mono<SpriteSequenceUtility>
    {
        private Dictionary<string, BaseSpriteSequenceInfor> m_AllSpriteSequenceDic = new Dictionary<string, BaseSpriteSequenceInfor>(); //所有的帧动画
        private List<SpriteSequenceEvent> m_AllSequence = new List<SpriteSequenceEvent>();

        #region 单例
        //public static SpriteSequenceUtility Instance { private set; get; }
        //private void Awake()
        //{
        //    if (Instance != null)
        //        Debug.LogError("SpriteSequenceUtility is Not Signal");
        //    Instance = this;
        //}

        protected override void Awake()
        {
            base.Awake();
            if (GetInstance(false) == null)
                Debug.LogError("SpriteSequenceUtility Not Exit");
            else
            {
                GameObject.DontDestroyOnLoad(gameObject);
            }
        }
        #endregion

        #region 帧动画

        /// <summary>
        /// 将参数的序列帧动画加入到管理器中
        /// </summary>
        /// <param name="infor"></param>
        public void RecordSpriteSequence(BaseSpriteSequenceInfor infor)
        {
            if (infor == null)
            {
                Debug.LogError("RecordSpriteSequence Fail, Animation is Null");
                return;
            }
            if (m_AllSpriteSequenceDic.ContainsKey(infor.AnimationName))
            {
                Debug.Log("This SpriteSequence is Exit,Will overlap Previous " + infor.AnimationName);
                m_AllSpriteSequenceDic[infor.AnimationName] = infor;
            }
            else
            {
                m_AllSpriteSequenceDic.Add(infor.AnimationName, infor);
            }
        }

        public void StartSpriteSequence(BaseSpriteSequenceInfor infor, UnityEngine.UI.Image img)
        {
            if (infor != null)
                StartSpriteSequence(infor.AnimationName, img);
        }

        /// <summary>
        /// 启动序列帧动画
        /// </summary>
        /// <param name="name"></param>
        /// <param name="img"></param>
        public void StartSpriteSequence(string name, UnityEngine.UI.Image img)
        {
            if (m_AllSpriteSequenceDic.ContainsKey(name) == false)
            {
                Debug.LogError("This SpriteSequence Not Exit " + name);
                return;
            }

            SpriteSequenceEvent sequence = new SpriteSequenceEvent();
            sequence.m_Img = img;
            sequence.m_HashCode = sequence.GetHashCode();
            sequence.m_SequenceInfor = m_AllSpriteSequenceDic[name];
            AddSequence(sequence);
        }

        void AddSequence(SpriteSequenceEvent sequence)
        {
            if (sequence == null)
            {
                Debug.LogError("SpriteSequenceEvent is Null");
                return;
            }
            int times = 0;
            for (int dex = 0; dex < m_AllSequence.Count; ++dex)
            {
                if (m_AllSequence[dex].m_HashCode == sequence.m_HashCode)
                    ++times;
            }

            if (times != 0)
                Debug.LogError("AddSequence There are already Exit " + times + " times " + sequence.m_Img.name);

            m_AllSequence.Add(sequence);

        }

        void RemoveSequence(UnityEngine.UI.Image img)
        {
            int Index = -1;
            for (int dex = 0; dex < m_AllSequence.Count; ++dex)
            {
                if (m_AllSequence[dex].m_Img == img)
                {
                    Index = dex;
                    break;
                }
            }

            if (Index != -1)
            {
                m_AllSequence.RemoveAt(Index);
            }
            else
            {
                Debug.LogError("Sequence Not Exit " + img);
            }
        }
        #endregion

        #region  序列帧执行循环
        int timeRecord = 0;
        private void FixedUpdate()
        {
            if (m_AllSequence.Count == 0) return;
            ++timeRecord;
            if (timeRecord % 2 == 0) return;
       
            for (int dex = 0; dex < m_AllSequence.Count; ++dex)
            {
                m_AllSequence[dex].DoSequence();
            }//for
        }
        #endregion

        #region 继续序列帧和重新开始序列帧接口 、暂停、停止移除
        /// <summary>
        /// 继续一个暂停的序列帧 。只对非Loop 模式的有效
        /// </summary>
        /// <param name="sequenceInfor"></param>
        public void ContinueSequence(BaseSpriteSequenceInfor sequenceInfor)
        {
            if (sequenceInfor == null) return;
            for (int dex = 0; dex < m_AllSequence.Count; ++dex)
            {
                if (m_AllSequence[dex].m_SequenceInfor == sequenceInfor)
                {
                    sequenceInfor.ContinueSequence();
                    return;
                }
            }
            Debug.LogError("ContinueSequence Fail,Not Exit");

        }
        /// <summary>
        /// 重启
        /// </summary>
        /// <param name="sequenceInfor"></param>
        public void RestartSequence(BaseSpriteSequenceInfor sequenceInfor)
        {
            if (sequenceInfor == null) return;
            for (int dex = 0; dex < m_AllSequence.Count; ++dex)
            {
                if (m_AllSequence[dex].m_SequenceInfor == sequenceInfor)
                {
                    sequenceInfor.ReStartSequence();
                    return;
                }
            }
            Debug.LogError("RestartSequence Fail,Not Exit");
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="sequenceInfor"></param>
        public void ForcePauseSequence(BaseSpriteSequenceInfor sequenceInfor) {
            if (sequenceInfor == null) return;
            for (int dex = 0; dex < m_AllSequence.Count; ++dex)
            {
                if (m_AllSequence[dex].m_SequenceInfor == sequenceInfor)
                {
                    sequenceInfor.ForcePauseSequence();
                    return;
                }
            }
            Debug.LogError("ForcePauseSequence Fail,Not Exit");
        }

        /// <summary>
        /// 强制停止和移除
        /// </summary>
        /// <param name="sequenceInfor"></param>
        public void ForceStopSequence(BaseSpriteSequenceInfor sequenceInfor)
        {
            if (sequenceInfor == null) {
                Debug.LogError("ForceStopSequence Fail");
                return;
            }
            int index = -1;
            for (int dex = 0; dex < m_AllSequence.Count; ++dex)
            {
                if (m_AllSequence[dex].m_SequenceInfor == sequenceInfor)
                {
                    index = dex;
                    break;
                }
            }
            if(index!=-1)
            {
                m_AllSequence.RemoveAt(index);
                sequenceInfor.StopAndClearSequence(); //清理资源
                Debug.Log("Stop And Remove Sequence Success  "+ sequenceInfor.AnimationName);
            }
            //else
            //{
            //    Debug.LogError("Can't Find This Sequence " + sequenceInfor.AnimationName);
            //}
        }
        #endregion



    }
}

