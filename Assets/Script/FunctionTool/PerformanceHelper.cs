using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceHelper
{

    /// <summary>
    /// 使用C#内置功能获取执行时间
    /// </summary>
    /// <param name="outputStr"></param>
    /// <param name="action"></param>
    public static void WatchPerformance(string outputStr, Action action)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start(); //  开始监视代码运行时间

        if (action != null)
        {
            action();
        }

        stopwatch.Stop(); //  停止监视
        TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
                                               //double seconds = timespan.TotalSeconds;  //  总秒数
        double millseconds = timespan.TotalMilliseconds;
        decimal seconds = (decimal)millseconds / 1000m;

        Debug.LogInfor(outputStr+" time="+ seconds.ToString("F7")); // 7位精度
    }
}
