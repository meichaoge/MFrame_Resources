using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework
{
    /// <summary>
    /// 计时器基类
    /// </summary>
    public class LocalTimerRecordBase 
    {
        public System.Action<float,int> m_CallbackAc = null;  //回调 当前的计时以及Hashcode
        public int m_HashCode { protected set; get; }
        public float m_SpaceTime ;  //间隔

        public virtual void InitialTimer()
        {
            m_HashCode= GetHashCode();
        }

        /// <summary>
        /// 每一帧调用
        /// </summary>
        public virtual void TimeTicked()
        {
          
        }
    }
}