using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.AI
{
    /// <summary>
    /// AI排队 :如果在正前方有其他的AI 则自己减速
    /// </summary>
    public class SteeringForQueue : Steering
    {
        [Range(1,20)]
        public float m_MaxQueueAhead = 2f;  //检测正前方多远的AI
        [Range(1, 20)]
        public float m_MaxQueueRadiu = 2f;  //检测正前方范围
        private Vehicle m_Vehicle;
        private Collider[] m_Colliders;
        public LayerMask m_OtherAILayers; //AI所在的层

        private void Start()
        {
            m_Vehicle = GetComponent<Vehicle>();
        }


        public override Vector3 GetForce()
        {
            Vector3 ahead = transform.position + m_Vehicle.m_Velocity.normalized * m_MaxQueueAhead;  //AI前面的点
            m_Colliders = Physics.OverlapSphere(ahead, m_MaxQueueRadiu, m_OtherAILayers);
            Vehicle otherVehicle = null;
            if (m_Colliders.Length>0)
            {
                foreach (var go in m_Colliders)
                {
                    if (go.gameObject == gameObject) continue;

                    otherVehicle = go.gameObject.GetComponent<Vehicle>();
                    if (m_Vehicle.m_Velocity.sqrMagnitude> otherVehicle.m_Velocity.sqrMagnitude)
                        m_Vehicle.m_Velocity *= 0.2f;  //当前AI减速等待其他AI；
                    else
                    {
                        otherVehicle.m_Velocity *= 0.3f;  //其他AI减速
                    }

                    break;
                }
            }
            return Vector3.zero;
        }


    }
}