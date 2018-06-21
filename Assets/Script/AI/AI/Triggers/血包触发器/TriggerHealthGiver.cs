using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    public class TriggerHealthGiver : TriggerRespawning
    {
        public int m_HealthGiven = 10;//增加血量


        protected override void Start()
        {
            m_TimeBetweenRespawns = 6;
            base.Start();
        }




        public override void Try(Sensor s)
        {
            if(IsActivity&&IsTouchingTrigger(s))
            {
                //***增加血量

                s.Notify(this);

                SetTriggerState(false);
            }
        }


        protected override bool IsTouchingTrigger(Sensor s)
        {
            if (s.m_SensorType != Sensor.SensorType.Health) return false;

            if (Vector3.Distance(transform.position, s.transform.position) < m_Radius)
            {
                transform.GetComponent<MeshRenderer>().material.color = Color.red;
                return true;
            }
            transform.GetComponent<MeshRenderer>().material.color = Color.green;

            return false;
        }


    }
}