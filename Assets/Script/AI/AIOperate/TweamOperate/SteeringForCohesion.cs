using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// AI群体行为：与群体中聚集——聚集 
    /// 测试发现效果不稳定
    /// </summary>
    public class SteeringForCohesion : Steering
    {
        private Radar m_Radar;  //当前AI 角色的雷达组件
        private Vehicle m_Vehicle;

        private Vector3 m_DesiredVelocity = Vector3.zero;

        private void Start()
        {
            m_Radar = GetComponent<Radar>();
            m_Vehicle = GetComponent<Vehicle>();
        }

        /// <summary>
        /// 求出邻居的平均位置 然后以当前AI到平均位置的方向求出预期速度
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetForce()
        {
            if (m_Radar.m_AllNeighbors.Count == 0)
                return Vector3.zero;   //没有邻居则按照自己方向前进

            Vector3 neighborsPos = Vector3.zero;
            m_DesiredVelocity = Vector3.zero;
            foreach (var neighbor in m_Radar.m_AllNeighbors.Values)
            {
                neighborsPos += neighbor.transform.position;
            }
            neighborsPos = neighborsPos / m_Radar.m_AllNeighbors.Count;  //平均位置

            m_DesiredVelocity = (neighborsPos - transform.position).normalized * m_Vehicle.m_MaxSpeed;
            if (m_Vehicle.m_IsPlanar)
                m_DesiredVelocity.y = 0;

            return m_DesiredVelocity - m_Vehicle.m_Velocity;
        }

    }
}