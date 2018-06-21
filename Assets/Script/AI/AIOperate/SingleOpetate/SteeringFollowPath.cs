using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// AI角色的操作行为: 跟随路径(简单的设置几个路径关键点 最后会停在最后一个关键点上)
    /// </summary>
    public class SteeringFollowPath : Steering
    {
        public Transform[] m_WayPoint;  //路径关键点
        private Transform m_Target=null; //目标点
        [Range(0.01f,10)]
        public float m_ArriveDistance=2; //当AI位置与路径关键点距离小于这个值时候需要寻找下一个关键点

        private float m_SqrArriveDistance;
        private int m_CurrentNode=0;  //当前已经抵达的关键点的索引值
        private Vector3 m_DesiredVelocity; //预期速度
        private Vehicle m_Vehicle;  //被控制的AI角色  以便于获取相关信息

        private void Start()
        {
            m_Vehicle = GetComponent<Vehicle>();
            m_SqrArriveDistance = m_ArriveDistance * m_ArriveDistance;
            if (m_WayPoint==null|| m_WayPoint.Length==0)
            {
                m_CurrentNode = 0;
                m_Target = null;
            }
            else
            {
                m_CurrentNode = 1;
                m_Target = m_WayPoint[0];
            }
        }

        public override Vector3 GetForce()
        {
            if (m_Vehicle==null|| m_Target == null || m_CurrentNode == -1)
                return Vector3.zero;  //没有设置路径关键点

            Vector3 distance = m_Target.position - transform.position;
            if (m_Vehicle.m_IsPlanar)
                distance.y = 0;

            if(m_CurrentNode== m_WayPoint.Length)
            {
                if(distance.sqrMagnitude< m_SqrArriveDistance)
                {
                    m_DesiredVelocity = distance - m_Vehicle.m_Velocity;
                }//此时行为类似于抵达行为
                else
                {
                    m_DesiredVelocity= distance.normalized* m_Vehicle.m_MaxSpeed; //计算预期速度
                }//此时类似于靠近行为
            }//已经在驶向最后一个路径关键点
            else
            {
                if (distance.sqrMagnitude < m_SqrArriveDistance)
                {
                    ++m_CurrentNode;
                    m_Target = m_WayPoint[m_CurrentNode - 1];
                }//已经靠近一个关键路径点 则设置目标是下一个路径关键点
                m_DesiredVelocity = distance.normalized * m_Vehicle.m_MaxSpeed; //计算预期速度
            }//中间行驶过程中

            return (m_DesiredVelocity - m_Vehicle.m_Velocity);  //操作向量 即预期速度与当前速度差
        }

    }
}