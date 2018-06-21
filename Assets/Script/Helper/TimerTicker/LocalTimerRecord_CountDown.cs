using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework
{
    /// <summary>
    /// 倒计时计时器
    /// </summary>
    public class LocalTimerRecord_CountDown : LocalTimerRecordBase
    {
        public float m_DeadTime;  //倒计时时长
        public float m_LastRecordTime;

        public override void InitialTimer()
        {
            base.InitialTimer();
            m_LastRecordTime = 0;
        }

        public override void TimeTicked()
        {
            m_DeadTime -= Time.deltaTime;
            if (m_DeadTime <= 0)
            {
                if (m_CallbackAc != null)
                    m_CallbackAc(0, m_HashCode);
                TimeTickUtility.GetInstance().UnRegisterTimer_Delay(this);  //标记为要删除
                return;
            }
            m_LastRecordTime += Time.deltaTime;
            if (m_LastRecordTime >= m_SpaceTime)
            {
                if (m_CallbackAc != null)
                    m_CallbackAc(m_DeadTime, m_HashCode);
                m_LastRecordTime -= m_SpaceTime;
            }

        }

    }
}