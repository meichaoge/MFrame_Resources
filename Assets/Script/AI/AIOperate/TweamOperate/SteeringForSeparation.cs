using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// AI群体行为：与群体中邻居保持适当的距离：分离
    /// </summary>
    public class SteeringForSeparation : Steering
    {
        private Radar m_Radar;  //当前AI 角色的雷达组件
        [Range(1, 50)]
        public float m_MaxComfortDistance = 3; //可接收的最近距离
        [Range(0.02f,20)]
        public float m_MinDistanceNeighbor = 0.5f; //当前物体与邻居之间最近距离  由于出现计算当前AI与邻居距离为0的情况 为了避免这个情况此时取这个值为距离
        [Range(1, 10)]
        public float m_MultiplierInsiderComforDistance = 2;  //AI与邻居距离太近时候的惩罚因子
        private Vehicle m_Vehicle;
        private void Start()
        {
            m_Radar = GetComponent<Radar>();
            m_Vehicle = GetComponent<Vehicle>();
        }

        /// <summary>
        /// 根据每个邻居的位置获得反比与距离的操控力 (这里为了减少开平方开销 得到的操控力反比与距离的平方)
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetForce()
        {
            Vector3 steeringForce = Vector3.zero;
            Vector3 toNeighbor;
            foreach (var neighbor in m_Radar.m_AllNeighbors.Values)
            {
                toNeighbor = transform.position - neighbor.transform.position; //到当前邻居的距离
                float length = toNeighbor.magnitude;
                if (Mathf.Approximately(length, 0))
                {
                    length = m_MinDistanceNeighbor;
                    //测试发现 当多个AI(立方体) 初始位置一样时候会出现在垂直方向上叠加  所以这里设置一个随机方向
                    toNeighbor = new Vector3(Random.value, Random.value, Random.value);
                    // Debug.Log("a=" + gameObject.name + "    b=" + neighbor.name);
                }
                steeringForce += toNeighbor.normalized / length; //计算这个邻居引起的操控力
                if (length < m_MaxComfortDistance)
                    steeringForce *= m_MultiplierInsiderComforDistance;  //如果距离太近则乘以额外的因子
            }

            if (steeringForce.magnitude > m_Vehicle.m_MaxForce)
                steeringForce = Vector3.ClampMagnitude(steeringForce, m_Vehicle.m_MaxForce);

            if (m_Vehicle.m_IsPlanar)
                steeringForce.y = 0;

           // Debug.Log(gameObject.name + ": = " + steeringForce+"   max"+ m_Vehicle.m_MaxForce);
            return steeringForce;
        }


    }
}