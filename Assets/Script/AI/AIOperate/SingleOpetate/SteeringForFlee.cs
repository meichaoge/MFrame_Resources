using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// AI角色的操作行为:远离目标
    /// </summary>
    public class SteeringForFlee : Steering
    {
        public GameObject m_Target;  //远离的目标
        [Range(1,300)]
        public float m_FearDistance = 20; //AI角色意识到危险时候开始逃跑的范围

        private Vector3 m_DesiredVelocity; //预期速度
        private Vehicle m_Vehicle;

        private void Start()
        {
            m_Vehicle = GetComponent<Vehicle>();
        }

        public override Vector3 GetForce()
        {
            if (m_Target == null || m_Vehicle == null)
                return Vector3.zero;

            Vector3 temPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 temTargetPos = new Vector3(m_Target.transform.position.x, 0, m_Target.transform.position.z);

            if (Vector3.Distance(temPos, temTargetPos) > m_FearDistance)
                return Vector3.zero;  //如果AI与目标距离大于感知距离则返回0

            //朝着背离目标方向
            m_DesiredVelocity = ( transform.position- m_Target.transform.position ).normalized * m_Vehicle.m_MaxSpeed; //计算预期速度

            if (m_Vehicle.m_IsPlanar)
                m_DesiredVelocity.y = 0;  //如果只在平面运动

            return (m_DesiredVelocity - m_Vehicle.m_Velocity);  //操作向量 即预期速度与当前速度差

        }

    }
}