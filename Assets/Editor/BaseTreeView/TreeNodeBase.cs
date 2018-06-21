using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 树形结构的节点基类
    /// </summary>
    public class TreeNodeBase
    {
        public enum NodeType
        {
            Switch,  //父级 开关项
            Item,    //叶子节点项
        }

        public NodeType TreeNodeType
        {
            get
            {
                if (m_ChildNodes != null && m_ChildNodes.Count > 0)
                    return NodeType.Switch;
                else
                    return NodeType.Item;
            }
        }  //当前树结点类型

        public TreeNodeBase m_ParentNode = null; //父级节点
        public List<TreeNodeBase> m_ChildNodes; //子节点
        public string m_NodeName = ""; //节点名
        public int Index { get; protected set; } //索引ID 
        public int ItemOffset { get; protected set; }// 项偏移
        public bool m_IsOpen = true;  //是否是展开状态

        #region 构造函数

        protected TreeNodeBase()
        {
            m_NodeName = "";
            m_ParentNode = null;
            m_ChildNodes = new List<TreeNodeBase>();
        }
        public TreeNodeBase(string _name, TreeNodeBase parent)
        {
            m_NodeName = _name;
            m_ParentNode = parent;
            if (parent != null)
                parent.AddChild(this);
            m_ChildNodes = new List<TreeNodeBase>();
        }
        public TreeNodeBase(string _name, TreeNodeBase parent, List<TreeNodeBase> childs)
        {
            m_NodeName = _name;
            m_ParentNode = parent;
            if (parent != null)
                parent.AddChild(this);
            m_ChildNodes = childs;
        }
        #endregion



        #region 添加删除树结点

        public void AddChild(TreeNodeBase node)
        {
            node.m_ParentNode = this;
            m_ChildNodes.Add(node);
        }
        public void DeleteChild(TreeNodeBase node)
        {
            int deleteIndex = -1;
            for (int dex = 0; dex < m_ChildNodes.Count; ++dex)
            {
                if (m_ChildNodes[dex] == node)
                {
                    deleteIndex = dex;
                    break;
                }
            }

            if (deleteIndex != -1)
                m_ChildNodes.RemoveAt(deleteIndex);
        }
        public void DeleteChild(int dex)
        {
            if (dex < 0 || dex > m_ChildNodes.Count - 1)
            {
                Debug.Log("DeleteChild Fail,Not Exit Index=" + dex);
                return;
            }
            m_ChildNodes.RemoveAt(dex);
        }
        #endregion

        /// <summary>
        /// 设置树节点参数
        /// </summary>
        /// <param name="dex">在TreeView中的索引</param>
        /// <param name="offset">标识层级关系 </param>
        public void SetIndex(int dex, int offset)
        {
            Index = dex;
            ItemOffset = offset;
        }


        /// <summary>
        /// 获得上面的兄弟节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public  TreeNodeBase GetUpBrotherNode( )
        {
            if (m_ParentNode == null) return null;
            for (int dex = 0; dex < m_ParentNode.m_ChildNodes.Count; ++dex)
            {
                if (m_ParentNode.m_ChildNodes[dex] == this)
                {
                    if (dex == 0)
                        return null;
                    return m_ParentNode.m_ChildNodes[dex - 1];
                }
            }//for
            Debug.LogError("GetUpBrotherNode Fail");
            return null;
        }


        #region  UI Show

        /// <summary>
        /// 获得当前节点的子节点展开的个数
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public  int GetShowTreeNodeCount()
        {
            int count = 0;
            for (int dex = 0; dex < m_ChildNodes.Count; ++dex)
            {
                if (m_ChildNodes[dex].TreeNodeType == TreeNodeBase.NodeType.Item)
                {
                    if (m_ChildNodes[dex].m_IsOpen)
                        ++count;
                }//叶子节点
                else
                {
                    if (m_ChildNodes[dex].m_IsOpen == false)
                        ++count;  //只计算当前的分支节点
                    else
                    {
                        ++count;
                        count += GetSubTreeNode(m_ChildNodes[dex]);
                    }
                }
            }//for
            return count;
        }
        private int GetSubTreeNode(TreeNodeBase node)
        {
            int count = 0;
            if (node == null) return 0;
            for (int dex = 0; dex < node.m_ChildNodes.Count; ++dex)
            {
                if (node.m_ChildNodes[dex].TreeNodeType == TreeNodeBase.NodeType.Item)
                {
                    if (node.m_ChildNodes[dex].m_IsOpen)
                        ++count;
                }//叶子节点
                else
                {
                    if (node.m_ChildNodes[dex].m_IsOpen == false)
                        ++count;  //只计算当前的分支节点
                    else
                    {
                        ++count;
                        count += GetSubTreeNode(node.m_ChildNodes[dex]);
                    }
                }
            }//for
            return count;
        }

        /// <summary>
        /// 获取当前节点的父节点状态是否有非展开状态的 如果有返回false 并记录那个父节点是非展开状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="beginNode"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static bool GetParentExpandState(out TreeNodeBase node, TreeNodeBase beginNode, TreeViewBase tree)
        {
            if (beginNode == null)
            {
                node = null;
                Debug.LogError("GetParentExpandState Fail, BeginNode is Null");
                return false;
            }

            while (beginNode.m_ParentNode != tree.m_Root)
            {
                if (beginNode.m_ParentNode == null)
                {
                    node = null;
                    return true;
                }

                if (beginNode.m_ParentNode.m_IsOpen == false)
                {
                    node = beginNode.m_ParentNode;
                    return false;
                }
                else
                {
                    beginNode = beginNode.m_ParentNode;
                }
            }//while  遍历当前节点的父节点
            node = null;
            return true;
        }


        #endregion






    }
}