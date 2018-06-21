using MFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Version(Name ="Hellow World ",Data ="2017-10-23", Description = "测试 Attribute")]
public class TestAttribute : MonoBehaviour
{

    private void Start()
    {
        var attribute = (VersionAttribute)Attribute.GetCustomAttribute(typeof(TestAttribute), typeof(VersionAttribute));
        Debug.Log(attribute.Name);
        Debug.Log(attribute.Data);
        Debug.Log(attribute.Description);


        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("32.>>" + GenerateUID.GetUID32());
            Debug.Log("64.>>" + GenerateUID.GetUID64());
            Debug.Log("");
        }
    }
}
