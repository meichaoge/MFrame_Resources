using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// AI角色的操作行为: 避开障碍物
    /// 效果一般般 会卡死
    /// </summary>
    public class SteeringForCollisionAvoidance : Steering
    {
        public float m_MaxSeeAhead = 2f;  //正前方能看到的最大距离
        public float m_MaxAvoidanceForce=5;  //避开障碍物产生的最大操控力

        private Vector3 m_DesiredVelocity; //预期速度
        private Vehicle m_Vehicle;  //被控制的AI角色  以便于获取相关信息

        private GameObject[] m_AllColliders;  //所有的障碍物

        private void Start()
        {
            m_Vehicle = GetComponent<Vehicle>();
            if (m_MaxAvoidanceForce > m_Vehicle.m_MaxForce)
                m_MaxAvoidanceForce = m_Vehicle.m_MaxForce;

            m_AllColliders = GameObject.FindGameObjectsWithTag("obstacle");  //所有障碍物
        }


        public override Vector3 GetForce()
        {
            Vector3 avoidForce = Vector3.zero;  //避开障碍物的力
            Vector3 maxSeePoint = transform.position + m_Vehicle.m_Velocity.normalized * m_MaxSeeAhead
                * (m_Vehicle.m_Velocity.magnitude / m_Vehicle.m_MaxSpeed); //最远开到的点

          UnityEngine.  Debug.DrawLine(transform.position, maxSeePoint);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, m_Vehicle.m_Velocity.normalized, out hit, m_MaxSeeAhead * (m_Vehicle.m_Velocity.magnitude / m_Vehicle.m_MaxSpeed)))
            {
                //***向AI正前方 发射一个射线检测是否会碰撞到物体
                avoidForce = maxSeePoint - hit.collider.transform.position;  //计算避免碰撞产生的操作力的方向
                avoidForce = avoidForce.normalized * m_MaxAvoidanceForce;
                if (m_Vehicle.m_IsPlanar)
                    avoidForce.y = 0;

                foreach (var item in m_AllColliders)
                {
                    if (item == hit.collider.gameObject)
                        item.GetComponent<MeshRenderer>().material.color = Color.black;
                    else
                        item.GetComponent<MeshRenderer>().material.color = Color.white;
                }//设置障碍物的颜色
            }
            else
            {
                foreach (var item in m_AllColliders)
                {
                        item.GetComponent<MeshRenderer>().material.color = Color.white;
                }//设置障碍物的颜色
            }


            return avoidForce;
        }


    }
}