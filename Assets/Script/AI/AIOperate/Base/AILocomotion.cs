using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// 派生自Vehicle 用于真正控制AI角色的移动 包括计算每次移动的距离 播放动画
    /// </summary>
    public class AILocomotion :Vehicle
    {
        private CharacterController m_Controller;  //AI 角色控制器
        private Rigidbody m_TheRigidbody; //AI的橘色Rigidbody
        private Animation m_Animation;  //AI角色的动画组件
        private Vector3 m_MoveDistance;  //AI角色每次移动的距离

        protected override void Start()
        {
            m_Controller = GetComponent<CharacterController>();
            m_TheRigidbody = GetComponent<Rigidbody>();
            m_Animation = GetComponent<Animation>();
            m_MoveDistance = Vector3.zero;

            base.Start();
        }


        /// <summary>
        /// 物理相关操作更新
        /// </summary>
        private void FixedUpdate()
        {
            m_Velocity += m_Acceleration * Time.fixedDeltaTime; //计算速度
            if (m_Velocity.sqrMagnitude > m_SqrMaxSpeed)
                m_Velocity = Vector3.ClampMagnitude(m_Velocity, m_MaxSpeed); //限制速度
            m_MoveDistance = m_Velocity * Time.fixedDeltaTime;  //计算移动的距离

            if(m_IsPlanar)
            {
                m_Velocity.y = 0;
                m_MoveDistance.y = 0;
            } //如果只在二位平面运动

            if (m_Controller != null)
            {
                m_Controller.SimpleMove(m_Velocity);  //如果有角色控制器 则利用角色控制器
            }
            else if (m_TheRigidbody != null && m_TheRigidbody.isKinematic)
            {
                transform.position += m_MoveDistance;
            } //如果AI有Rigidbody 但是由动力学的方式控制运动
            else
            {
                if (m_TheRigidbody != null)
                    m_TheRigidbody.MovePosition(m_TheRigidbody.position + m_MoveDistance);  //使用Rigidbody控制
            }

            //更新朝向 如果速度大于一直阈值(防止抖动)
            if(m_Velocity.sqrMagnitude>0.01)
            {
                //通过当前朝向与速度方向进行插值 计算新的朝向
                Vector3 newForward = Vector3.Slerp(transform.forward, m_Velocity, m_Damping * Time.fixedDeltaTime);

                if (m_IsPlanar)
                    newForward.y = 0;

                transform.forward = newForward; //设置AI角色的新朝向
            }

            //播放行走动画
            if (m_Animation != null)
                m_Animation.Play("walk");

        }


    }
}