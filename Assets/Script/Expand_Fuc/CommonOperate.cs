using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework
{
    /// <summary>
    /// 常用的工具类
    /// </summary>
    public class CommonOperate : Singleton_Static<CommonOperate>
    {
        protected override void InitialSingleton() { }

        /// <summary>
        /// 在手机端调用硬件的震动效果
        /// </summary>
        public void ShakeEffectAtPhone()
        {
#if UNITY_IPHONE || UNITY_ANDROID
            //震屏
            Handheld.Vibrate();
#endif
        }






    }
}