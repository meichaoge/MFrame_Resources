using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// AI群体行为：与群体中邻居朝向一致 :队列
    /// </summary>
    public class SteeringForAlignment : Steering
    {
        private Radar m_Radar;  //当前AI 角色的雷达组件
        private Vehicle m_Vehicle;

        private void Start()
        {
            m_Radar = GetComponent<Radar>();
            m_Vehicle = GetComponent<Vehicle>();
        }

        /// <summary>
        /// 群体队列中操控力 为邻居的平均方向 减去自己方向
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetForce()
        {
            if (m_Radar.m_AllNeighbors.Count == 0)
                return Vector3.zero;   //没有邻居则按照自己方向前进


            Vector3 neighborForward = Vector3.zero;
            foreach (var neighbor in m_Radar.m_AllNeighbors.Values)
            {
                neighborForward += neighbor.transform.forward;
            }

            neighborForward = neighborForward / m_Radar.m_AllNeighbors.Count;
            return neighborForward - transform.forward;  //使得AI 朝向一致的操控力
                                                         //获得邻居的平均朝向
        }


    }
}