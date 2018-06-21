using MFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 行为树Action 节点
    /// </summary>
    public class TreeViewNodeInfor_ActionNode : TreeViewNodeInfor
    {
        public Type m_ActionScriptType; //Action 对应的脚本名称
        public BehaviorActionAttribute m_Attribute;
        public TreeViewNodeInfor_ActionNode(string arrangenment, Type type, BehaviorActionAttribute attribute) : base(arrangenment)
        {
            m_ActionScriptType = type;
            m_Attribute = attribute;
        }

    }
}