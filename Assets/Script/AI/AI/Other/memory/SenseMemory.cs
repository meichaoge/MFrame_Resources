using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// 记忆感知
    /// </summary>
    public class SenseMemory : MonoBehaviour
    {
        public float m_CanMemoryTime = 4f; //能记住的时间
        public List<MemoryItem> m_AllMemory = new List<MemoryItem>(); //记忆列表

        private List<MemoryItem> m_Memory_Forget = new List<MemoryItem>(); //需要遗忘的记忆

        void Update()
        {
            m_Memory_Forget.Clear();
            foreach (var memory in m_AllMemory)
            {
                if (memory.m_MemoryTimeLeft<=Time.deltaTime)
                {
                    m_Memory_Forget.Add(memory);
                } //需要遗忘的记忆
                else
                {
                    memory.m_MemoryTimeLeft -= Time.deltaTime;

                    UnityEngine.Debug.DrawLine(transform.position, memory.m_Go.transform.position,Color.blue); 
                } //更新剩余记忆时间
            }

            foreach (var item in m_Memory_Forget)
            {
                m_AllMemory.Remove(item);
            }//更新记忆
        }


        /// <summary>
        /// 添加记忆
        /// </summary>
        /// <param name="go"></param>
        /// <param name="type"></param>
        public void AddMemory(GameObject go,Sensor.SensorType type)
        {
            MemoryItem memory = null;
            foreach (var item in m_AllMemory)
            {
                if(item.m_Go==go)
                {
                    memory = item;
                    break;
                }
            }

            if(memory==null)
            {
                memory = new MemoryItem(go, Time.time, m_CanMemoryTime, type);
                m_AllMemory.Add(memory); //记住这个对象
            }
            else
            {
                memory.m_LastMemoryTime = Time.time;
                memory.m_MemoryTimeLeft = m_CanMemoryTime;
                memory.m_SensorType = type;
            }//如果已经记住过 则更新信息
        }

       
    }
}