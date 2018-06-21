using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// 跟随领队的行为  ：设置跟随行为的目标点在领队的速度方向反方向一定距离
    /// </summary>
    [RequireComponent(typeof(SteeringForArrive))]
    public class SteeringForLeaderFollowing : Steering
    {
        public GameObject m_LeaderObj; //领队
        [Range(1,100)]
        public float m_MaxBehindLeader = 10;  //最远距离领队多远

        private Vehicle m_LeaderVehicle;  //领队
        private SteeringForArrive m_SteeringForArrive;  //到达

        private Vector3 m_ArriveTargetPos;  //跟随的目标位置
        GameObject target;

        private void Start()
        {
            m_LeaderVehicle = m_LeaderObj.GetComponent<Vehicle>();
            m_SteeringForArrive = GetComponent<SteeringForArrive>();
            target= new GameObject("ArriveTarget_"+gameObject.name);
            target.hideFlags = HideFlags.HideAndDontSave;
            m_SteeringForArrive.m_Target = target; //设置跟随目标
            m_SteeringForArrive.m_Target.transform.position = m_LeaderVehicle.transform.position;
        }

        public override Vector3 GetForce()
        {
            m_ArriveTargetPos = m_LeaderObj.transform.position + (-1*m_LeaderVehicle.m_Velocity.normalized) * m_MaxBehindLeader;

            target.transform.position= m_ArriveTargetPos;
            //AI 跟随领队的位置在领队速度方向的反方向一定距离
            m_SteeringForArrive.m_Target.transform.position = m_ArriveTargetPos;
            return Vector3.zero;
        }





    }
}