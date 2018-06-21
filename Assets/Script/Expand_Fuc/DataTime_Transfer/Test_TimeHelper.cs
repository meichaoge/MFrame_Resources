using MFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_TimeHelper : MonoBehaviour {

    public long time;
    // Use this for initialization
    void Start()
    {
        DateTime systemTime = TimeHelper.Second2DateTime(time);
        Debug.Log(systemTime.ToString("yyyy-MM-dd HH:mm:ss"));

        Debug.Log(TimeHelper.DateTime2Secend_Now());
        Debug.Log(DateTime.Now.Millisecond);
    }
}
