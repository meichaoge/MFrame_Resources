using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.AI
{
    /// <summary>
    /// 所有触发器的基类 包含所有触发器的共有属性
    /// </summary>
    public class Trigger : MonoBehaviour
    {
        protected TriggerSystemManager m_Manager;  //管理中心对象

        protected Vector3 m_Position;  //触发器的位置
        public int m_Radius; //触发器半径
        public bool m_NeedToBeRemove = false;  //是否需要被移除



        protected void Awake()
        {
            m_Manager = FindObjectOfType<TriggerSystemManager>(); 
        }

        protected virtual void Start()
        {
            m_NeedToBeRemove = false;
            m_Manager.RegisterTrigger(this);
        }



        /// <summary>
        /// 检测感知器S是否在触发器的作用范围 如果是 则需要采取相关行动
        /// </summary>
        public virtual void Try(Sensor s)
        {

        }

        /// <summary>
        /// 检测感知器S是否在触发器的作用范围 如果是 返回true 否则返回false 由Try()调用
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsTouchingTrigger(Sensor s)
        {
            return false;
        }

        /// <summary>
        /// 触发器更新内部的状态
        /// </summary>
        public virtual void Updateme() { }




    
    }
}