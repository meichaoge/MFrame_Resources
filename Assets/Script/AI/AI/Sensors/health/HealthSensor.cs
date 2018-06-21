using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.AI
{
    public class HealthSensor : Sensor
    {
        protected override void Start()
        {
            m_SensorType = Sensor.SensorType.Health;
            base.Start();
        }

        public override void Notify(Trigger t)
        {
            Debug.Log("Get Health Given " + t.gameObject.name + " value= " + (t as TriggerHealthGiver).m_HealthGiven);
        }


    }
}