using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 树型结构视图的基类
    /// </summary>
    public class TreeViewBase
    {
        public TreeNodeBase m_Root = null;  //根节点
        protected List<TreeNodeBase> m_AllNode = new List<TreeNodeBase>();  //所有的子节点
        protected int m_Index = 0;  //当前记录的节点的索引
        protected int m_ItemWidth = 150;  //每一项的宽度
        protected int m_ItemHeight = 20;
        protected int m_ItemOffset = 20;  //每一项的便宜

        protected int m_SelectItemIndex = -1;  //当前选中的是哪一个节点
        protected const string RootNodeName = "Root";

        Dictionary<string, TreeNodeBase> m_TempNode = new Dictionary<string, TreeNodeBase>(); //临时字典方便按照名字分类和划分层级


        /// <summary>
        /// 参数标识当前选中的节点对象
        /// </summary>
        public System.Action<TreeNodeBase> OnSelectNodeItemChange;  //当选择的树形节点改变时候



        #region 根据给定的数据源创建树形链表结构

        /// <summary>
        /// 创建树视图结构
        /// </summary>
        /// <param name="items"></param>
        /// <param name="defaultSelectItem"> 默认选中哪一项</param>
        public virtual void CreateTreeView(List<TreeViewNodeInfor> items, int defaultSelectItem)
        {
            m_Index = 0;
            m_SelectItemIndex = -1;
            m_TempNode.Clear();
            if (items == null || items.Count == 0)
            {
                m_Root = null;
                Debug.Log("CreateTreeView Fail, Node Data Source is Null ");
                return;
            }
            m_SelectItemIndex = defaultSelectItem;
            CreateRootNode();
            RecordGroupNodeInfor(RootNodeName, m_Root);  //记录根节点对应关系

            for (int dex = 0; dex < items.Count; ++dex)
            {
                //    Debug.Log("CreateTreeView  dex=" + dex + ">>>" + items[dex]);
                CreateTreeNode(items[dex], "");
            }

            SearchTreeToRecordTreeNode();   //搜索整棵树结构以便于记录相关数据
        }

        protected virtual void CreateRootNode()
        {
            m_Root = new TreeNodeBase(RootNodeName, null);  //创建根节点
        }


        /// <summary>
        /// 分割节点路径并按照路径层级关系创建合适的节点以及关联父节点
        /// </summary>
        /// <param name="item">包含当前节点路径的对象</param>
        /// <param name="parentPath">已经分割完成的路径</param>
        /// <param name="splitchar">分隔符</param>
        protected virtual void CreateTreeNode(TreeViewNodeInfor item, string parentPath, char splitchar = '/')
        {
            //    TreeNodeBase parentNode = null;
            //    if (string.IsNullOrEmpty(parentPath))
            //        parentNode = m_Root;
            //    else
            //        parentNode = GetClassficationNodeByName(parentPath);  //获取当前节点所属于的父级分类节点

            //    string[] segment = itemPath.Split(splitchar);//分割字符串判断层级目录
            //    if (segment.Length == 1)
            //    {
            //        parentPath = CreateNode(parentPath, segment[0], parentNode, splitchar);
            //    }  //当前字符串层级关系确定完 
            //    else
            //    {
            //        parentPath = CreateNode(parentPath, segment[0], parentNode, splitchar);  //更新新的父节点路径
            //        itemPath = itemPath.Remove(0, (segment[0] + splitchar).Length);  //去掉一个层级目录后的路径
            //        CreateTreeNode(itemPath, parentPath, splitchar);
            //    }
        }


        /// <summary>
        /// 创建中间的一个组的父节点  后面会添加其他的叶子节点
        /// </summary>
        /// <param name="parentPath">当前节点所属于的父节点路径</param>
        /// <param name="segmentStr">当前节点名称</param>
        /// <param name="parentNode">父节点</param>
        /// <param name="splitchar">分隔符</param>
        /// <returns></returns>
        protected virtual string CreateGroupNode(string parentPath, string segmentStr, TreeNodeBase parentNode, char splitchar)
        {
            parentPath = parentPath + segmentStr + splitchar;  //当前需要创建的树节点的完整路径
            TreeNodeBase node = GetGroupNodeByName(parentPath);  //判断是否存在这个路径的节点
            if (node == null)
            {
                node = new TreeNodeBase(segmentStr, parentNode);  //创建当前节点
                RecordGroupNodeInfor(parentPath, node); //记录当前创建的节点
            }//当前子目录不存在
            return parentPath;
        }

        /// <summary>
        /// 创建本次遍历获取的叶子节点
        /// </summary>
        /// <param name="parentPath"></param>
        /// <param name="item"></param>
        /// <param name="parentNode"></param>
        /// <param name="splitchar"></param>
        /// <returns></returns>
        protected virtual void CreateLeafNode(string parentPath, TreeViewNodeInfor item, TreeNodeBase parentNode, char splitchar)
        {
            //TreeViewNodeInfor_ActionNode nodeInfor = item as TreeViewNodeInfor_ActionNode;
            //parentPath = parentPath + nodeInfor.m_Arrangenment + splitchar;  //当前需要创建的树节点的完整路径
            //TreeNodeBase node = GetClassficationNodeByName(parentPath);  //判断是否存在这个路径的节点
            //if (node == null)
            //{
            //    node = new TreeNodeBase(nodeInfor.m_Arrangenment, parentNode);  //创建当前节点
            //    RecordClassfication(parentPath, node); //记录当前创建的节点
            //}//当前子目录不存在
        }


        /// <summary>
        /// 记录分类的节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="node"></param>
        protected void RecordGroupNodeInfor(string name, TreeNodeBase node)
        {
            if (m_TempNode.ContainsKey(name))
            {
                return;
            }
            m_TempNode.Add(name, node);
        }

        /// <summary>
        /// 获取一个组的父节点
        /// </summary>
        /// <param name="name">需要获取的组节点的路径</param>
        /// <returns></returns>
        protected TreeNodeBase GetGroupNodeByName(string name)
        {
            if (m_TempNode.ContainsKey(name))
                return m_TempNode[name];
            return null;
        }

        #endregion


        /// <summary>
        ///  记录节点 并设置正确的索引
        /// </summary>
        /// <param name="node"></param>
        /// <param name="offset">每多一级分类则子类需要偏移一定距离</param>
        protected void RecordTreeNode(TreeNodeBase node, int offset)
        {
            m_AllNode.Add(node);
            node.SetIndex(m_Index, offset);
            ++m_Index;
        }


        #region 从根节点开始搜索整个树节点 并设置TreeNode的层级和索引

        /// <summary>
        /// 深度优先遍历树节点获取所有的子节点
        /// </summary>
        protected void SearchTreeToRecordTreeNode()
        {
            m_AllNode.Clear();
            m_Index = 0;
            m_SelectItemIndex = -1;
            if (m_Root == null || m_Root.m_ChildNodes.Count == 0)
                return;

            for (int dex = 0; dex < m_Root.m_ChildNodes.Count; ++dex)
            {
                SearchTreeView(m_Root.m_ChildNodes[dex], 0);
            }

        }
        /// <summary>
        /// 遍历所有的子节点和子项
        /// </summary>
        /// <param name="classifyNode"></param>
        protected virtual void SearchTreeView(TreeNodeBase classifyNode, int offset)
        {
            RecordTreeNode(classifyNode, offset);  //记录当前节点
            if (classifyNode.m_ChildNodes.Count == 0) return;
            ++offset;
            for (int dex = 0; dex < classifyNode.m_ChildNodes.Count; ++dex)
            {
                SearchTreeView(classifyNode.m_ChildNodes[dex], offset);
            }
        }
        #endregion


        /// <summary>
        /// 在编辑器状态下显示树形视图
        /// </summary>
        public virtual float ShowTreeView()
        {
            return 0;
            //int showRowCount = 0; //已经显示的树形节点的行数,如果有节点折叠则下面的叶子节点不显示
            //TreeNode parentNode = null; //折叠的父节点的索引
            //for (int dex = 0; dex < m_AllNode.Count; ++dex)
            //{
            //    parentNode = null;
            //    bool state = TreeNode.GetParentExpandState(out parentNode, m_AllNode[dex], this);  //获得当前节点是否需要显示
            //    //    Debug.Log(dex + "  :: state=" + state + "   :::parentNode=" + parentNode+" cur="+ m_AllNode[dex] .m_NodeName+ "   :::showRowCount="+ showRowCount);
            //    if (state == false) continue;  //不需要显示当前节点
            //    ++showRowCount;
            //    Rect rect;
            //    if (m_AllNode[dex].TreeNodeType == TreeNode.NodeType.Switch)
            //    {
            //        #region 显示分类节点
            //        rect = new Rect(m_ItemOffset * m_AllNode[dex].ItemOffset, m_ItemHeight * (showRowCount - 1), m_ItemWidth / 2, m_ItemHeight);
            //        if (GUI.Button(rect, new GUIContent(m_AllNode[dex].m_NodeName + " ::" + m_AllNode[dex].Index, m_ItemBg)))
            //        {
            //            m_AllNode[dex].m_IsOpen = !m_AllNode[dex].m_IsOpen;  //点击状态反转
            //            m_SelectItemIndex = m_AllNode[dex].Index;
            //            //int count = TreeNode.GetShowTreeNodeCount(m_AllNode[m_SelectItemIndex]);
            //            //Debug.Log("Switch Node   count=" + count);
            //        }
            //        #endregion
            //    }//一个有子节点的项
            //    else
            //    {
            //        #region 显示叶子节点
            //        rect = new Rect(m_ItemOffset * m_AllNode[dex].ItemOffset, m_ItemHeight * (showRowCount - 1), m_ItemWidth, m_ItemHeight);
            //        if (m_AllNode[dex].m_ParentNode != null && m_AllNode[dex].m_ParentNode.m_IsOpen)
            //        {
            //            if (GUI.Button(rect, new GUIContent(m_AllNode[dex].m_NodeName + " ::" + m_AllNode[dex].Index)))
            //            {
            //                m_SelectItemIndex = m_AllNode[dex].Index;
            //                int count = TreeNode.GetShowTreeNodeCount(m_AllNode[m_SelectItemIndex]);
            //                Debug.Log("ShowTreeView   m_SelectItemIndex= " + m_SelectItemIndex + "count=" + count);
            //            }
            //        }//if
            //        #endregion
            //    }//叶子节点项
            //}//for

            //return showRowCount * m_ItemHeight;
        }

        /// <summary>
        /// 根据索引获取对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TreeNodeBase GetTreeNodeByIndex(int index)
        {
            if (m_AllNode.Count == 0)
            {
                Debug.Log("GetTreeNodeByIndex Fail ,No Data");
                return null;
            }

            if (index < 0 || index > m_AllNode.Count)
            {
                Debug.Log("GetTreeNodeByIndex Fail ,Index Out Of Range " + index + "  Count=" + m_AllNode.Count);
                return null;
            }
            return m_AllNode[index];
        }



    }
}