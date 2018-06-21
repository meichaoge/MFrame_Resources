using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// 操作AI角色的基类  把操作对象抽象成一个质点
    /// 包含位置/质量/速度/最大的力/最高的速度/朝向
    /// </summary>
    public class Vehicle : MonoBehaviour
    {
        //****AI的属性
        [Range(0.01f, 100)]
        public float m_MaxSpeed = 10; //这个AI角色能达到的最大速度
        [Range(0.01f, 500)]
        public float m_MaxForce = 100;  //设置能施加到这个AI角色的力的最大值
        [Range(0.01f, 500)]
        public float m_Mass = 1;  //AI角色的质量
        public Vector3 m_Velocity;  //AI角色的速度
        [Range(0.01f,10)]
        public float m_Damping = 0.9f;  //控制转向时的速度

        [Range(0.01f, 10)]
        [Header("更新计算控制力的时间间隔")]
        public float m_ComputeInterval = 0.2f;//控制力的计算时间间隔 为了达到更高的帧率 ，操作力不需要每一帧更新
        public bool m_IsPlanar = true; //是否在二维平面上 如果是 计算两个GameObject 时候忽略Y值的不同

        //计算AI中间数据
        protected Vector3 m_Acceleration=Vector3.zero;  //AI角色的加速度
        protected float m_SqrMaxSpeed;  //缓存最大速度的平方的结果
        private Vector3 m_SteeringForce= Vector3.zero;//计算得到的操作力
        private float m_Timer=0;  //计时器

        private Steering[] m_Steerings;  //这个AI角色包含的操控行为列表
        private bool m_IsFirstTime = true;  //第一次Update时候调用一次获得初始状态


        protected virtual void Start()
        {
            m_SqrMaxSpeed = m_MaxSpeed * m_MaxSpeed;
            m_Steerings = gameObject.GetComponents<Steering>(); //获得这个AI角色所包含的操控行为列表
        }


        protected void Update()
        {
            m_Timer += Time.deltaTime;
            if (m_IsFirstTime)
            {
                m_IsFirstTime = false;
                UpdateSteeringForce();  //为了能获得一个初始状态 
                return;
             //   Debug.Log("AAAAAAA " + gameObject.name);
            }
          
            if(m_Timer> m_ComputeInterval)
            {
                UpdateSteeringForce();
                m_Timer = 0;  //设置状态
            }
        }


        /// <summary>
        /// 跟新计算操控力
        /// </summary>
        void UpdateSteeringForce()
        {
            //将操控行为列表中所有的操控行为对应的操控了进行带权重的求和
            foreach (var Substeering in m_Steerings)
            {
                if (Substeering.enabled)
                    m_SteeringForce += Substeering.GetForce() * Substeering.m_Weight;
            }
            m_SteeringForce = Vector3.ClampMagnitude(m_SteeringForce, m_MaxForce);  //使得操控力不大于 m_MaxForce
            m_Acceleration = m_SteeringForce / m_Mass;  //AI的加速度
            m_SteeringForce = Vector3.zero;
        }


    }
}
