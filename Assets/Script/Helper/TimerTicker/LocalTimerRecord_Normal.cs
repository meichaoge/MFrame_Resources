using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework
{
    /// <summary>
    /// 正常的计时器
    /// </summary>
    public class LocalTimerRecord_Normal : LocalTimerRecordBase
    {
        public float m_StartRecordTime = 0;  //注册时候的时间
        public float m_CurrentTime = 0;    //计时过去多久
        public float m_LastRecordTime = 0; //上一帧时候记录距离上一次到达m_SpaceTime 间隔过去的时间

        public override  void InitialTimer()
        {
            base.InitialTimer();
            m_CurrentTime = 0;
            m_LastRecordTime = 0;
        }

        /// <summary>
        /// 每一帧调用
        /// </summary>
        public override void TimeTicked()
        {
            m_CurrentTime += Time.deltaTime;
            m_LastRecordTime += Time.deltaTime;

            if (m_LastRecordTime >= m_SpaceTime)
            {
                if (m_CallbackAc != null)
                    m_CallbackAc(m_CurrentTime, m_HashCode);

                m_LastRecordTime -= m_SpaceTime;
            }
        }
    }
}