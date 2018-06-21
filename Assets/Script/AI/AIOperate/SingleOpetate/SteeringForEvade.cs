using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// AI角色的操作行为: 逃避追捕
    /// </summary>
    public class SteeringForEvade : Steering
    {
        public GameObject m_Target;  //需要寻找的目标物体

        private Vector3 m_DesiredVelocity; //预期速度
        private Vehicle m_Vehicle;  //被控制的AI角色  以便于获取相关信息
        private Vehicle m_TargetVehicle;  //追逐的AI角色  以便于获取相关信息


        private void Start()
        {
            m_Vehicle = GetComponent<Vehicle>();
            if (m_Target != null)
                m_TargetVehicle = m_Target.GetComponent<Vehicle>();
        }

        public override Vector3 GetForce()
        {
            if (m_Target == null || m_Vehicle == null)
                return Vector3.zero;

            Vector3 toTarget = m_Target.transform.position - transform.position;

            float lookAheadTime = toTarget.magnitude / (m_Vehicle.m_MaxSpeed + m_TargetVehicle.m_Velocity.magnitude);
            Vector3 m_TargetDesiredPos = m_Target.transform.position + m_TargetVehicle.m_Velocity * lookAheadTime; //追逐者预计的位置
            m_DesiredVelocity = ( transform.position- m_TargetDesiredPos ).normalized * m_Vehicle.m_MaxSpeed;  //朝着追逐者预计达到位置到当前位置方向逃离

            if (m_Vehicle.m_IsPlanar)
                m_DesiredVelocity.y = 0;  //如果只在平面运动

            return (m_DesiredVelocity - m_Vehicle.m_Velocity);  //操作向量 即预期速度与当前速度差
        }


    }
}