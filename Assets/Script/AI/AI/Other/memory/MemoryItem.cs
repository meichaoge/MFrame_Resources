using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.AI
{
    /// <summary>
    /// 记忆感知的对象
    /// </summary>
    public class MemoryItem
    {
        public GameObject m_Go; //感知到的对象
        public float m_LastMemoryTime;  //最近感知的时间
        public float m_MemoryTimeLeft; //还能留存多久
        public Sensor.SensorType m_SensorType; //被感知到的方式

        public MemoryItem(GameObject go,float time,float timeleft,Sensor.SensorType type)
        {
            m_Go = go;
            m_LastMemoryTime = time;
            m_MemoryTimeLeft = timeleft;
            m_SensorType = type;
        }


    }
}