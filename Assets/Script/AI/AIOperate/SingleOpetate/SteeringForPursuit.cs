using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// AI角色的操作行为: 追逐目标
    /// </summary>
    public class SteeringForPursuit : Steering
    {
        public GameObject m_Target;  //需要寻找的目标物体
        [Range(0.01f,1)]
        public float m_MaxAngleBetweenAI = 0.5f; //逃跑着与追逐着正前方夹角小于这个值时候直接向逃跑着位置移动

        private Vector3 m_DesiredVelocity; //预期速度
        private Vehicle m_Vehicle;  //被控制的AI角色  以便于获取相关信息
        private Vehicle m_TargetVehicle;  //被追逐的AI角色  以便于获取相关信息

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
            float relativeDirection = Vector3.Dot(transform.forward, m_Target.transform.forward); //计算追逐着和逃避者方向的夹角

            if(relativeDirection > m_MaxAngleBetweenAI|| m_TargetVehicle==null)
            {
                m_DesiredVelocity = toTarget.normalized * m_Vehicle.m_MaxSpeed;  //追逐者与逃跑着者朝向基本一致 则直接追上去
            }
            else
            {
                //计算预测时间 正比于两者之间的距离 反比于两者之间的速度
                float lookAheadTime = toTarget.magnitude / (m_TargetVehicle.m_Velocity.magnitude + m_Vehicle.m_MaxSpeed); //预测逃跑者位置的时间
                Vector3 m_TargetDesiredPos = m_Target.transform.position + m_TargetVehicle.m_Velocity * lookAheadTime; //逃跑者预计的位置
                m_DesiredVelocity = (m_TargetDesiredPos - transform.position).normalized * m_Vehicle.m_MaxSpeed;
            } //如果逃跑者与追逐者朝向不一致 则追逐者向着逃跑者预测的位置移动


            if (m_Vehicle.m_IsPlanar)
                m_DesiredVelocity.y = 0;  //如果只在平面运动

            return (m_DesiredVelocity - m_Vehicle.m_Velocity);  //操作向量 即预期速度与当前速度差
        }

    }
}