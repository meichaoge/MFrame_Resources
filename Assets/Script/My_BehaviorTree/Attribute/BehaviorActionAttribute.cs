using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.BehaviorTree
{
    /// <summary>
    /// Action 定义的类级特性 标识层级关系以及如何正确的分类和显示在树结构中
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BehaviorActionAttribute : Attribute
    {
        public string ActionArrangement ; // 类似与Action/Debug/Log  形式标识层级关系
        public Type ActionType;

        public BehaviorActionAttribute(string arrangement,Type type)
        {
            if (string.IsNullOrEmpty(arrangement))
                Debug.LogError("Behavior Node Arrangement Can't Be Null");
            ActionArrangement = arrangement;
            ActionType = type;
        }
    }
}