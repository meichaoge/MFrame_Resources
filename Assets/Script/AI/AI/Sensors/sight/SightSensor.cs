using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.AI
{
    /// <summary>
    /// 视觉感知器
    /// </summary>
    public class SightSensor : Sensor
    {
        public float m_FieldOfView = 45;   //视线范围
        public float m_ViewDistance = 100f; //视线最远看到的距离

        protected override  void Start()
        {
            m_SensorType = SensorType.Sight; // 设置为视觉感知器
            base.Start();
        }

        /// <summary>
        /// 当视觉感知器感觉到某个触发器时调用
        /// </summary>
        /// <param name="t"></param>
        public override void Notify(Trigger t)
        {
            Debug.Log("I Can See " + t.gameObject.name);
            UnityEngine.Debug.DrawLine(transform.position, t.transform.position, Color.red);
        }


        void OnDrawGizmos()
        {
            Vector3 frontRayPoint = transform.position + transform.forward* m_ViewDistance;  //视野范围和距离
            float fieldOfViewRadius = Mathf.Deg2Rad * m_FieldOfView;

            Vector3 leftRayPoint = transform.TransformPoint(new Vector3(m_ViewDistance * Mathf.Sin(fieldOfViewRadius), 0, m_ViewDistance * Mathf.Cos(fieldOfViewRadius)));  //左侧视线边界
            Vector3 rightRayPoint = transform.TransformPoint(new Vector3(-1f*m_ViewDistance * Mathf.Sin(fieldOfViewRadius), 0, m_ViewDistance * Mathf.Cos(fieldOfViewRadius)));  //左侧视线边界

          UnityEngine.  Debug.DrawLine(transform.position + new Vector3(0, 1, 0), frontRayPoint, Color.green);
            UnityEngine.Debug.DrawLine(transform.position + new Vector3(0, 1, 0), leftRayPoint, Color.green);
            UnityEngine.Debug.DrawLine(transform.position + new Vector3(0, 1, 0), rightRayPoint, Color.green);


        }

    }
}