using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.AI
{
    /// <summary>
    /// 所有感知器的基类   包含感知类型的枚举定义 和事件管理器
    /// </summary>
    public class Sensor : MonoBehaviour
    {
        /// <summary>
        /// 感知类型枚举 (这里可以扩展成标志 方便一个感知器同时感知多个)
        /// </summary>
        public enum SensorType
        {
            Sight,    //视线感知器
            Sound, //声音感知器
            Health,   //生命感知器
        }

        protected TriggerSystemManager m_Manager;  //管理中心对象

        public SensorType m_SensorType;  //感知类型
        protected void Awake()
        {
            m_Manager = FindObjectOfType<TriggerSystemManager>();
        }

        protected virtual void Start()
        {
            m_Manager.RegisterSensor(this);
        }

        public virtual void Notify(Trigger t)
        {


        }


    }
}