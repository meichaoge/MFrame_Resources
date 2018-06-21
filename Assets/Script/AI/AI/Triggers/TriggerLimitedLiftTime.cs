using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.AI
{
    /// <summary>
    /// 对于有生命周期的触发器的基类  
    /// </summary>
    public class TriggerLimitedLiftTime : Trigger
    {
        public float m_LiftTime; //持续时间

        public override void Updateme()
        {
            //   base.Updateme();
            m_LiftTime -= Time.deltaTime;
            if (m_LiftTime <= 0)
            {
                m_NeedToBeRemove = true;
            }  //持续一段时间后自动消失
        }

    }
}