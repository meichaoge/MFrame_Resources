using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// AI角色的操作行为: 随机徘徊 
    /// </summary>
    public class SteeringForWander : Steering
    {
        //************m_WanderRadius 与m_WanderDistance 值比较小时候会明显出现徘徊效果

        [Range(0.01f,5)]
        public float m_WanderRadius=1; //随机徘徊半径
        [Range(0.01f, 5)]
        public float m_WanderDistance=5; //徘徊距离  及徘徊元出现在AI角色前面的距离
        [Range(0.1f, 5)]
        public float m_WanderJitter=1; //每秒加到目标的随机位移的最大值

        private Vector3 m_DesiredVelocity; //预期速度
        private Vehicle m_Vehicle;  //被控制的AI角色  以便于获取相关信息


        private Vector3 m_CircleTarget;  //圆上的点  这个圆时相对于当前AI角色
        private Vector3 m_WanderTarget; //AI角色徘徊的目标位置

        private void Start()
        {
            m_Vehicle = GetComponent<Vehicle>();
            //选择圆圈上一个点作为初始点
             m_CircleTarget = new Vector3(m_WanderRadius * 0.7f, 0, m_WanderRadius * 0.7f);
        }

        public override Vector3 GetForce()
        {
            Vector3 randomDisplacement = new Vector3((Random.value - 0.5f) * 2 ,  (Random.value - 0.5f) * 2 , (Random.value - 0.5f) * 2 )* m_WanderJitter;  //计算随机位移

            if (m_Vehicle.m_IsPlanar)
                randomDisplacement.y = 0;

            m_CircleTarget += randomDisplacement; //新的目标位置
            m_CircleTarget = m_WanderRadius * m_CircleTarget.normalized; //新的位置可能不在圆上 需要投影到圆周上

            m_WanderTarget = m_Vehicle.m_Velocity.normalized * m_WanderDistance + m_CircleTarget + transform.position; //徘徊的目标位置

            m_DesiredVelocity = (m_WanderTarget - transform.position).normalized * m_Vehicle.m_MaxSpeed; //预计速度

            return (m_DesiredVelocity - m_Vehicle.m_Velocity);  //操作向量 即预期速度与当前速度差
        }


    }
}