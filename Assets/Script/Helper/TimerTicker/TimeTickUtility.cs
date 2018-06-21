
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework
{

    /// <summary>
    /// 全局的计时器
    /// </summary>
    public class TimeTickUtility : Singleton_Mono<TimeTickUtility>
    {

        private Dictionary<int, LocalTimerRecordBase> m_AllTimerCallback = new Dictionary<int, LocalTimerRecordBase>();
        private List<LocalTimerRecordBase> m_AllDeadTimers = new List<LocalTimerRecordBase>();  //需要被注销的计时器

        private void Update()
        {
            if (m_AllTimerCallback.Count == 0) return;
            if (m_AllDeadTimers.Count != 0)
            {
                for (int dex = 0; dex < m_AllDeadTimers.Count; ++dex)
                {
                    if (m_AllTimerCallback.ContainsKey(m_AllDeadTimers[dex].m_HashCode))
                        m_AllTimerCallback.Remove(m_AllDeadTimers[dex].m_HashCode);
                }
                m_AllDeadTimers.Clear();
            } //清理过时计时器

            foreach (var item in m_AllTimerCallback.Values)
            {
                item.TimeTicked();
            }
        }

        /// <summary>
        ///注册计时器
        /// </summary>
        /// <param name="spaceTime"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public int RegisterTimer(float spaceTime, System.Action<float,int> callback)
        {
            LocalTimerRecord_Normal recordInfor = new LocalTimerRecord_Normal();
            recordInfor.m_SpaceTime = spaceTime;
            recordInfor.m_CallbackAc = callback;
            recordInfor.m_StartRecordTime = Time.time;

            recordInfor.InitialTimer();

            if (m_AllTimerCallback.ContainsKey(recordInfor.m_HashCode))
            {
                Debug.LogError("RegisterTimer  Fail");
                return 0;
            }
            m_AllTimerCallback.Add(recordInfor.m_HashCode, recordInfor);
            return recordInfor.m_HashCode;
        }


        /// <summary>
        /// 注册倒计时计时器
        /// </summary>
        /// <param name="spaceTime"></param>
        /// <param name="deadTime">倒计时时长</param>
        /// <param name="callback"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int RegisterCountDownTimer(float spaceTime, float deadTime, System.Action<float, int> callback)
        {
            LocalTimerRecord_CountDown recordInfor = new LocalTimerRecord_CountDown();
            recordInfor.m_SpaceTime = spaceTime;
            recordInfor.m_CallbackAc = callback;
            recordInfor.m_DeadTime = deadTime;

            recordInfor.InitialTimer();
            if (m_AllTimerCallback.ContainsKey(recordInfor.m_HashCode))
            {
                Debug.LogError("RegisterCountDownTimer  Fail");
                return 0;
            }
            m_AllTimerCallback.Add(recordInfor.m_HashCode, recordInfor);
            return recordInfor.m_HashCode;
        }


        public bool UnRegisterTimer(int hashcode)
        {
            if (hashcode == 0)
                Debug.LogError("UnRegisterTimer  注意可能不存在这个计时器");

            if (m_AllTimerCallback.ContainsKey(hashcode))
            {
                m_AllTimerCallback.Remove(hashcode);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 标记为要删除的计时器
        /// </summary>
        /// <param name="timer"></param>
        public void UnRegisterTimer_Delay(LocalTimerRecordBase timer)
        {
            m_AllDeadTimers.Add(timer);
        }

    }
}