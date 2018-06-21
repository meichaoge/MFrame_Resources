using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.AI
{
    /// <summary>
    /// 视觉触发器
    /// </summary>
    public class SightTrigger : Trigger
    {
        public override void Try(Sensor s)
        {
            if(IsTouchingTrigger(s))
            {
                s.Notify(this);    //通知感知体
            }
        }


        /// <summary>
        ///判断感知器能否感知到这个触发器
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected override bool IsTouchingTrigger(Sensor s)
        {
            if (s.m_SensorType != Sensor.SensorType.Sight) return false;

            RaycastHit hit;
            Vector3 rayDirection = transform.position - s.gameObject.transform.position; //射线方向
            if (Vector3.Angle(rayDirection, s.gameObject.transform.forward) < (s as SightSensor).m_FieldOfView)
            {
                if (Physics.Raycast(s.gameObject.transform.position + new Vector3(0, 1, 0), rayDirection, out hit, (s as SightSensor).m_ViewDistance))
                {
                    return true;
                } //检测感知器与触发器之间是否有障碍物              
            }
            return false;
        }


    }
}