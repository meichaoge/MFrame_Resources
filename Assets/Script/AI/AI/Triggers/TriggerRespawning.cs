using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.AI
{
    /// <summary>
    /// 触发之后一段时间又自动激活
    /// </summary>
    public class TriggerRespawning : Trigger
    {
        public float m_TimeBetweenRespawns;  //两次激活状态之间的间隔时间
        public bool IsActivity; //是否是激活状态
        protected float m_TimeRemainUntilRespawn = 0; //距离下一次激活剩余时间


        protected override void Start()
        {
            IsActivity = true;
            base.Start();
        }

        public void SetTriggerState(bool state)
        {
            if(IsActivity!= state)
            {
                if(state==false)
                {
                    Debug.Log("禁用触发器");
                    m_TimeRemainUntilRespawn = m_TimeBetweenRespawns; //设置计时时长
                }
            }
            IsActivity = state;  //更新状态
        }


        public override void Updateme()
        {
           if(IsActivity==false)
            {
                m_TimeRemainUntilRespawn -= Time.deltaTime;
                //Debug.Log("m_TimeRemainUntilRespawn=" + m_TimeRemainUntilRespawn);
                if(m_TimeRemainUntilRespawn<=0)
                {
                    m_TimeRemainUntilRespawn = 0;
                    SetTriggerState(true);  //重新激活
                    Debug.Log("重新激活触发器");
                }
            }
        }

    }
}