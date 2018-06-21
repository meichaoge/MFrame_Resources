using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// 跟随者在领队前面时候如果挡住了则需要开启逃避行为
    /// </summary>
    public class EvadeController : MonoBehaviour
    {
        public GameObject m_LeaderObj;
        private Vehicle m_LeaderVehicle;

        private SteeringForLeaderFollowing m_SteeringForLeaderFollowing;
        private SteeringForEvade m_SteeringForEvade;
        private Vehicle m_Vehicle;
        [Range(1f, 100)]
        public float m_SqrEvadeDistance;  //规避的距离 的平方
        [Range(0.02f,5)]
        public float m_CheckTime = 1f;  //检测时间
        private float m_Timer;
        // Use this for initialization
        void Start()
        {
            m_LeaderVehicle = m_LeaderObj.GetComponent<Vehicle>();
            m_Vehicle = GetComponent<Vehicle>();
            m_SteeringForLeaderFollowing = GetComponent<SteeringForLeaderFollowing>();
            m_SteeringForEvade = GetComponent<SteeringForEvade>();
            m_SteeringForEvade.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            m_Timer += Time.deltaTime;
            if(m_Timer> m_CheckTime)
            {
                m_Timer = 0;

                Vector3 leaderAheadPos = m_LeaderObj.transform.position + m_LeaderVehicle.m_Velocity.normalized * 
                        m_SteeringForLeaderFollowing.m_MaxBehindLeader;   //领队前方距离上的点

                Vector3 distance = leaderAheadPos - transform.position;
                if (m_Vehicle.m_IsPlanar)
                    distance.y = 0;

                if(distance.sqrMagnitude< m_SqrEvadeDistance)
                {
                    m_SteeringForEvade.enabled = true;
                    UnityEngine.Debug.DrawLine(transform.position, m_LeaderObj.transform.position);
                } //如果当前AI在领队前方一定的距离的一个范围内 则需要开启逃避行为
                else
                {
                    m_SteeringForEvade.enabled = false;
                }
            }

        }




    }
}