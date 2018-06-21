using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// 所有操作行为的基类 包含操作行为共有的基类和方法 
    /// 操作AI角色的寻找、逃跑、追逐、躲避、徘徊、分离、队列、聚集等都是由此类派生
    /// </summary>
    public abstract class Steering : MonoBehaviour
    {
        [Range(0,1)]
        public float m_Weight = 1; //表示每个操作力的权重

        /// <summary>
        /// 计算操作力的方法 由派生类实现
        /// </summary>
        /// <returns></returns>
        public abstract Vector3 GetForce();


    }
}
