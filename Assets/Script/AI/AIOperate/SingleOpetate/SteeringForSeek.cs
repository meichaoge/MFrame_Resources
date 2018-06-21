using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// AI角色的操作行为: 向目标靠近
    /// </summary>
    public class SteeringForSeek : Steering
    {
        public GameObject m_Target;  //需要寻找的目标物体

        private Vector3 m_DesiredVelocity; //预期速度
        private Vehicle m_Vehicle;  //被控制的AI角色  以便于获取相关信息

        private void Start()
        {
            m_Vehicle = GetComponent<Vehicle>();  
        }


        /// <summary>
        /// 在向目标靠近的操作行为上 获得的力为操作向量
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetForce()
        {
            if (m_Target == null || m_Vehicle == null)
                return Vector3.zero;
            m_DesiredVelocity = (m_Target.transform.position - transform.position).normalized * m_Vehicle.m_MaxSpeed; //计算预期速度

            if (m_Vehicle.m_IsPlanar)
                m_DesiredVelocity.y = 0;  //如果只在平面运动

            return (m_DesiredVelocity - m_Vehicle.m_Velocity);  //操作向量 即预期速度与当前速度差
        }

    }
}