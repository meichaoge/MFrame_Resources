using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.AI
{
    public class DrawGizmos : MonoBehaviour
    {
        public float m_EvadeRadiu = 2; //领队正前方的检测球半径
        public float m_MaxDistance;  //检测距离

        private Vector3 m_Center;
        private Vehicle m_Vehicle;

        private void Start()
        {
            m_Vehicle = GetComponent<Vehicle>();
        }

        private void Update()
        {
            m_Center = transform.position + m_Vehicle.m_Velocity.normalized * m_MaxDistance;
        }


        private void OnDrawGizmos()
        {
            //在领队前方画一个线球框 如果AI进入这个范围则需要这个AI开启逃避
            Gizmos.DrawWireSphere(m_Center, m_EvadeRadiu);
        }
    }
}