using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// 声音触发器
    /// </summary>
    public class SoundTrigger : TriggerLimitedLiftTime
    {
        public float m_SoundStrength = 100; //声音的强度  会随着距离而递减

        protected override void Start()
        {
            m_LiftTime = 3;
            base.Start();
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }


        /// <summary>
        /// 检测感知体能否听到声音触发器发出的声音
        /// </summary>
        /// <param name="s"></param>
        public override void Try(Sensor s)
        {
            if (IsTouchingTrigger(s))
                s.Notify(this);
        }

        protected override bool IsTouchingTrigger(Sensor s)
        {
            if (s.m_SensorType != Sensor.SensorType.Sound) return false;
            float distance = (s.gameObject.transform.position - transform.position).magnitude;
         //   Debug.Log("distance=" + distance + " strength=" + m_SoundStrength / distance);
            if(distance<=(s as SoundSensor).m_MaxHearingDistance)
            {
                //如果声音强度大于能被感知的强度
                if ((m_SoundStrength / distance) >= (s as SoundSensor).m_MinThreshold)
                    return true;
            } //在能听到的范围之内
            return false;
        }




    }
}
