using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 定义一个Attribute 并限制只能在类上面
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public string Name { get; set; }
    public string Data { get; set; }
    public string Description { get; set; }

}
