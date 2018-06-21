using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.AI
{
    /// <summary>
    /// 听觉感知器
    /// </summary>
    public class SoundSensor : Sensor
    {
        public float m_MaxHearingDistance = 30f;
        public float m_MinThreshold = 30;  //最低能被听到的声音大小

        protected override void Start()
        {
            m_SensorType = Sensor.SensorType.Sound;
            base.Start();
        }

        /// <summary>
        /// 感知到声音
        /// </summary>
        /// <param name="t"></param>
        public override void Notify(Trigger t)
        {
            Debug.Log("I Hear From " + t.gameObject.name);

        }


    }
}