using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;


/// <summary>
/// 几种方法获取方法执行耗时
/// </summary>
public class GetMethodExcuteTime : MonoBehaviour {

    void Start()
    {

        float t = Time.time;
        TestMethod();
        UnityEngine.Debug.Log(string.Format("total: {0} ms", Time.time - t));


        //2
        Stopwatch sw = new Stopwatch();
        sw.Start();
        TestMethod();
        sw.Stop();
        UnityEngine.Debug.Log(string.Format("total: {0} ms", sw.ElapsedMilliseconds));

        //3  (需要打开Profiler 找到那一帧)
        Profiler.BeginSample("TestMethod");
        TestMethod();
        Profiler.EndSample();
    }


    void TestMethod()
    {
        for (int i = 0; i < 10000000; i++)
        {
        }
    }
}
