using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 节点的输入输出属性
    /// </summary>
    public enum NodeInOutEnum
    {
        InOut,  //输入和输出同时具有
        In,  //输入节点
        Out,  //输出节点
        None
    }

    public enum NodeValueTypeEnum
    {
        Int,
        Float,
        String,
        Vector2,
        Vector3,
        Vector4,
        Matrix3x3,
        Matrix4x4,

        Color3,   //颜色属性
        Color4,
    }

 
}