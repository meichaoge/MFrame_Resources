using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.BehaviorTree
{

    /// <summary>
    /// 自定义的行为树编辑器的行为节点
    /// </summary>
    [System.Serializable]
    public class ActionTreeNode : TreeNodeBase
    {
        public BehaviorActionAttribute m_BehaviorActionAttribute = null;
        public Type m_AttachType; //关联的类型

        #region  构造函数
        protected ActionTreeNode() : base()
        {
            m_BehaviorActionAttribute = null;
        }
        public ActionTreeNode(string _name, TreeNodeBase parent, BehaviorActionAttribute attribute, Type type = null) : base(_name, parent)
        {
            m_AttachType = type;
            m_BehaviorActionAttribute = attribute;
        }
        public ActionTreeNode(string _name, TreeNodeBase parent, List<TreeNodeBase> childs, BehaviorActionAttribute attribute, Type type = null) : base(_name, parent, childs)
        {
            m_AttachType = type;
            m_BehaviorActionAttribute = attribute;
        }
        #endregion

    }
}