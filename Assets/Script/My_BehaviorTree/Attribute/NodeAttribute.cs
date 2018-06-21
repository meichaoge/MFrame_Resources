using MFramework.BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MFramework
{
    /// <summary>
    /// 描述每一个节点中定义的字段的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class NodeAttribute : Attribute
    {
        public string Name;  //节点名称
        public NodeInOutEnum InOut; //输入输出属性
        public bool SignalConnect = true;  //是否只允许一个连线 
        public NodeValueTypeEnum ValueType; //节点属性的值类型
        public string Description;  //节点描述
        public object DefaultValue; //默认值

    
        public NodeAttribute(string name, NodeInOutEnum inout, bool isSignalConnect, NodeValueTypeEnum value, object defaultVlaue, string description="")
        {
            Name = name;
            InOut = inout;
            SignalConnect = isSignalConnect;
            ValueType = value;
            Description = description;
            DefaultValue = defaultVlaue;
        }

    }
}