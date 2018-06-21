using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    ///  AI角色的操作行为: 抵达目标位置  (当靠近目标位置时候会减速)
    ///  当AI 到达边界时候初始速度过大也会出现穿过情况
    /// </summary>
    public class SteeringForArrive : Steering
    {
        public GameObject m_Target;
       // public float m_ArrivalDistance = 0.3f;
       [Range(0.1f,20)]
        public float m_SlowDownDistance=2;   //当与目标小于这个距离时候开始减速

        private Vector3 m_DesiredVelocity; //预期速度
        private Vehicle m_Vehicle;  //被控制的AI角色  以便于获取相关信息

        private void Start()
        {
            m_Vehicle = GetComponent<Vehicle>();
        }


        /// <summary>
        /// 在向目标抵达的操作行为上 获得的力为操作向量
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetForce()
        {
            if (m_Target == null || m_Vehicle == null)
                return Vector3.zero;

            Vector3 toTarget = m_Target.transform.position - transform.position;
            if (m_Vehicle.m_IsPlanar)
                toTarget.y = 0;

            //与目标距离大于减速距离则使用类似于靠近的行为策略
            if (toTarget.magnitude > m_SlowDownDistance)
            {
                m_DesiredVelocity = toTarget.normalized * m_Vehicle.m_MaxSpeed; //计算预期速度
            }
            else
            {
                m_DesiredVelocity = toTarget - m_Vehicle.m_Velocity;
            } //进入减速带

            return (m_DesiredVelocity - m_Vehicle.m_Velocity);  //操作向量 即预期速度与当前速度差
        }


        private void OnDrawGizmos()
        {
            if (m_Target == null) return;
            Gizmos.DrawWireSphere(m_Target.transform.position, m_SlowDownDistance);
        }


    }
}