using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine; 

namespace MFramework.BehaviorTree
{

    /// <summary>
    /// 获取所有继承自NodeActionParent 的自定义Action 节点
    /// </summary>
    public class GetAllActionNodeScript 
    {
        private static List<TreeViewNodeInfor> m_AllActionAttribute = new List<TreeViewNodeInfor>();
        public static  List<TreeViewNodeInfor> AllDefineAction
        {
            get
            {
                if (m_AllActionAttribute == null || m_AllActionAttribute.Count == 0)
                    m_AllActionAttribute = GetAllActionNodes();

                return m_AllActionAttribute;
            }
        }


        /// <summary>
        /// 遍历反射获取所有定义的Action 节点
        /// </summary>
        /// <returns></returns>
         static List<TreeViewNodeInfor> GetAllActionNodes()
        {
            List<TreeViewNodeInfor> m_AllActionAttribute = new List<TreeViewNodeInfor>();

            //*******遍历获取程序集并获得所有标有指定特性的类
            //**这里获得所有标记由BehaviorActionAttribute 的Action 类
            IEnumerable<Type> availableTypes = AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(type => type.GetTypes());
            foreach (Type type in availableTypes)
            {
                foreach (BehaviorActionAttribute attribute in Attribute.GetCustomAttributes(type).OfType<BehaviorActionAttribute>())
                {
                    //      Debug.Log(attribute.ActionArrangement);
                   // Debug.Log("GetAllActionNode : " + type.Name);
                    TreeViewNodeInfor_ActionNode nodeInfor = new TreeViewNodeInfor_ActionNode(attribute.ActionArrangement, type, attribute);
                    m_AllActionAttribute.Add(nodeInfor);
                }
            }
            return m_AllActionAttribute;
        }

    }
}