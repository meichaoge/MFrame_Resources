using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MFramework.BehaviorTree
{
    public class EditorRecordNode 
    {
        public ActionTreeNode m_Node;
        public Vector2 m_ShowPos;
        public string m_UID;
        public NodeActionParent m_ActionScript;//脚本对象
        public NodeParent m_EditorNode; //编辑器视图对象

        public EditorRecordNode(ActionTreeNode node, Vector2 mousPos)
        {
            m_Node = node;
            m_ShowPos = mousPos;
            m_UID = GenerateUID.GetUID64();

            Debug.Log("m_Node.m_AttachType=" + m_Node.m_AttachType+ "mousPos=" + mousPos);
            object obj = m_Node.m_AttachType.Assembly.CreateInstance(m_Node.m_AttachType.Name);
            if(obj==null)
            {
                Debug.LogError(" m_Node.m_AttachType=" + m_Node.m_AttachType);
                return;
            }
            m_ActionScript = obj  as NodeActionParent;
            m_EditorNode = new NodeParent(m_ActionScript);
          var attributes=  m_Node.m_AttachType.GetCustomAttributes(false).OfType<BehaviorActionAttribute>();
            foreach (var item in attributes)
            {
                if (m_ActionScript.m_BehaviorActionAttribute == null)
                    m_ActionScript.m_BehaviorActionAttribute = item;
                 //   Debug.Log("zz " + item.ActionArrangement);
            }
            //   m_ShowNode.m_BehaviorActionAttribute = attributes[0];
            m_EditorNode.m_Pos = mousPos;
        }


    }
}