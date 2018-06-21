using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// 事件管理其 维护所有的触发器和感知器列表 并维护其状态
    /// </summary>
    public class TriggerSystemManager : MonoBehaviour
    {
        List<Sensor> m_CurrentSensors = new List<Sensor>();  //感知器列表
        List<Trigger> m_CurrentTriggers = new List<Trigger>();  //触发器列表

        List<Sensor> m_ToBeRemoveSensors = new List<Sensor>();//需要移除的感知器
        List<Trigger> m_ToBeRemoveTriggers = new List<Trigger>(); //需要移除的触发器


        private void Update()
        {
            UpdateTriggers();  //更新所有的触发器的状态
            TryTriggers();  //迭代所有的感知器和出发器并做出相应的行为
        }


        /// <summary>
        /// 更新维护触发器的状态
        /// </summary>
        void UpdateTriggers()
        {
            m_ToBeRemoveTriggers.Clear();
            foreach (var t in m_CurrentTriggers)
            {
                if (t.m_NeedToBeRemove)
                {
                    m_ToBeRemoveTriggers.Add(t);   //记录需要移除的触发器
                }
                else
                {
                    t.Updateme();  //触发器更新状态
                }
            }

            foreach (var t in m_ToBeRemoveTriggers)
            {
                m_CurrentTriggers.Remove(t);
            }  //移除相关触发器

        }
        /// <summary>
        /// 迭代所有的感知器和出发器并做出相应的行为
        /// </summary>
        void TryTriggers()
        {
            foreach (var s in m_CurrentSensors)
            {
                if(s.gameObject!=null)
                {
                    foreach (var t in m_CurrentTriggers)
                    {
                        t.Try(s);  //检测S感知器是否在作用范围 并做出相应的响应
                    }
                } //如果感知器所在的感知体还存在
                else
                {
                    m_ToBeRemoveSensors.Add(s);  //移除感知器
                }
            }

            foreach (var  s in m_ToBeRemoveSensors)
            {
                m_CurrentSensors.Remove(s);  //移除
            }
            m_ToBeRemoveSensors.Clear();

        }


        /// <summary>
        /// 注册触发器
        /// </summary>
        /// <param name="t"></param>
        public void RegisterTrigger(Trigger t)
        {
            m_CurrentTriggers.Add(t);
        }

        /// <summary>
        /// 注册感知器
        /// </summary>
        /// <param name="s"></param>
        public void RegisterSensor(Sensor s)
        {
            m_CurrentSensors.Add(s);
        }

    }
}