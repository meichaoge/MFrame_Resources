using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// 雷达组件  用于检测附近的AI 角色  
    /// </summary>
    public class Radar : MonoBehaviour
    {
        [Range(1,1000)]
        public float m_DetectRadius = 10f; //领域半径
        [Range(0,180)]
        public float m_DetectaAngle =150f;  //领域内可视角度范围
        [Range(0.02f,10)]
        public float m_CheckInterval = 0.3f; //检测的时间间隔
        public LayerMask m_LayerChecked; //需要检测哪一层的游戏对象

        [SerializeField]
         List<GameObject> m_AllNeighbors_forShow = new List<GameObject>(); //邻居列表
        //由于在多个AI使用时发现会出现重复情况  原因未知
        public Dictionary<int, GameObject> m_AllNeighbors = new Dictionary<int, GameObject>();//邻居列表

        private float m_Timer=0; //计时器
        private Collider[] m_Colliders;  //碰撞提组合
        private bool m_IsFirstTime = true;

        private void Start()
        {
            GetAllNeighbors();  //首先获取一个
        }


        private void Update()
        {
            m_Timer += Time.deltaTime;
            if(m_Timer> m_CheckInterval)
            {
                m_Timer = 0;
                GetAllNeighbors();
            }
        }

        /// <summary>
        /// 获取当前AI的邻居
        /// </summary>
        void GetAllNeighbors()
        {
            m_AllNeighbors.Clear();

            m_Colliders = Physics.OverlapSphere(transform.position, m_DetectRadius, m_LayerChecked);  //检测AI角色领域内所有的碰撞体
            Vehicle vehicle;
            for (int dex = 0; dex < m_Colliders.Length; ++dex)
            {
                #region 遍历每一个在指定范围内的对象
                if (m_Colliders[dex].gameObject == gameObject) continue;  //过滤掉自己
                vehicle = m_Colliders[dex].gameObject.GetComponent<Vehicle>();
                if (vehicle != null)
                {
                    //Debug.Log(Vector3.Angle(transform.forward, vehicle.gameObject.transform.position - transform.position));
                    if (Vector3.Angle(transform.forward, vehicle.gameObject.transform.position - transform.position) > m_DetectaAngle)
                        vehicle.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;  //不在视野范围
                    else
                    {
                        int hashcode = vehicle.gameObject.GetHashCode();
                        if (m_AllNeighbors.ContainsKey(hashcode) == false)
                            m_AllNeighbors.Add(hashcode, vehicle.gameObject);

                        vehicle.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    }//在领域内并且 在可视范围内
                }
                else
                {
                    m_Colliders[dex].gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
                }
                #endregion
            }//检测获取的领域范围内的碰撞体


# if UNITY_EDITOR
            m_AllNeighbors_forShow.Clear();
            foreach (var item in m_AllNeighbors)
            {
                m_AllNeighbors_forShow.Add(item.Value);
            }
#endif

        }

    }
}