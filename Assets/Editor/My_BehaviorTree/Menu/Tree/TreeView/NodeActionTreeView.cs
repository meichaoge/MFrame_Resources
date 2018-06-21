using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 自定义的行为节点 树形结构
    /// </summary>
    public class NodeActionTreeView : TreeViewBase
    {
        private string m_NodeBgPath = "Assets/Editor/EditorResources/ActionNodeItembg.png"; //背景图


        protected override void CreateRootNode()
        {
            m_Root = new ActionTreeNode(RootNodeName,null,null,null);
        }



        public override float ShowTreeView()
        {
            int showRowCount = 0; //已经显示的树形节点的行数,如果有节点折叠则下面的叶子节点不显示
            TreeNodeBase parentNode = null; //折叠的父节点的索引
            for (int dex = 0; dex < m_AllNode.Count; ++dex)
            {
                parentNode = null;
                bool state = TreeNodeBase.GetParentExpandState(out parentNode, m_AllNode[dex], this);  //获得当前节点是否需要显示
                                                                                                   //    Debug.Log(dex + "  :: state=" + state + "   :::parentNode=" + parentNode+" cur="+ m_AllNode[dex] .m_NodeName+ "   :::showRowCount="+ showRowCount);
                if (state == false) continue;  //不需要显示当前节点
                ++showRowCount;
                Rect rect;
                if (m_AllNode[dex].TreeNodeType == TreeNodeBase.NodeType.Switch)
                {
                    #region 显示分类节点
                    rect = new Rect(m_ItemOffset * m_AllNode[dex].ItemOffset, m_ItemHeight * (showRowCount - 1), m_ItemWidth / 2, m_ItemHeight);
                    m_AllNode[dex].m_IsOpen= EditorGUI.Foldout(rect, m_AllNode[dex].m_IsOpen, m_AllNode[dex].m_NodeName, true);  //创建一个可折叠的项
                    m_SelectItemIndex = m_AllNode[dex].Index;

                    //if (GUI.Button(rect, new GUIContent(m_AllNode[dex].m_NodeName + " ::" + m_AllNode[dex].Index, m_ItemBg)))
                    //{
                    //    m_AllNode[dex].m_IsOpen = !m_AllNode[dex].m_IsOpen;  //点击状态反转
                    //    m_SelectItemIndex = m_AllNode[dex].Index;
                    //    //int count = TreeNode.GetShowTreeNodeCount(m_AllNode[m_SelectItemIndex]);
                    //    //Debug.Log("Switch Node   count=" + count);
                    //}
                    #endregion
                }//一个有子节点的项
                else
                {
                    #region 显示叶子节点
                    rect = new Rect(m_ItemOffset * m_AllNode[dex].ItemOffset, m_ItemHeight * (showRowCount - 1), m_ItemWidth, m_ItemHeight);
                    if (m_AllNode[dex].m_ParentNode != null && m_AllNode[dex].m_ParentNode.m_IsOpen)
                    {
                        if (GUI.Button(rect, new GUIContent(m_AllNode[dex].m_NodeName)))
                        {
                            if (m_AllNode[dex].Index != m_SelectItemIndex)
                            {
                                m_SelectItemIndex = m_AllNode[dex].Index;
                                int count = m_AllNode[m_SelectItemIndex].GetShowTreeNodeCount();
                                Debug.Log(this.GetType().Name + "::ShowTreeView   m_SelectItemIndex= " + m_SelectItemIndex + "   count=" + count);

                                if (OnSelectNodeItemChange != null)
                                    OnSelectNodeItemChange(m_AllNode[m_SelectItemIndex]);
                            }//if
                        }//if button
                    }//if
                    #endregion
                }//叶子节点项
            }//for

            return showRowCount * m_ItemHeight;
        }

        protected override void CreateTreeNode(TreeViewNodeInfor item, string parentPath, char splitchar = '/')
        {
            TreeViewNodeInfor_ActionNode nodeInfor = item as TreeViewNodeInfor_ActionNode;
            TreeNodeBase parentNode = null;
            if (string.IsNullOrEmpty(parentPath))
                parentNode = m_Root;
            else
                parentNode = GetGroupNodeByName(parentPath);  //获取当前节点所属于的父级分类节点

            string[] segment = nodeInfor.m_Arrangement.Split(splitchar);//分割字符串判断层级目录
            if (segment.Length == 1)
            {
                //Debug.Log("CreateNode2  ....  " + segment[0]);
                TreeViewNodeInfor_ActionNode newNodeInfor = new TreeViewNodeInfor_ActionNode(segment[0], nodeInfor.m_ActionScriptType,nodeInfor.m_Attribute);
                 CreateLeafNode(parentPath, newNodeInfor, parentNode, splitchar);
            }  //当前字符串层级关系确定完 
            else
            {
                parentPath = CreateGroupNode(parentPath, segment[0], parentNode, splitchar);  //更新新的父节点路径
                string path = nodeInfor.m_Arrangement.Remove(0, (segment[0] + splitchar).Length);  //去掉一个层级目录后的路径
                TreeViewNodeInfor_ActionNode newNodeInfor = new TreeViewNodeInfor_ActionNode(path, nodeInfor.m_ActionScriptType, nodeInfor.m_Attribute);

                CreateTreeNode(newNodeInfor, parentPath, splitchar);
            }
        }

        protected override void CreateLeafNode(string parentPath, TreeViewNodeInfor item, TreeNodeBase parentNode, char splitchar)
        {
            TreeViewNodeInfor_ActionNode nodeInfor = item as TreeViewNodeInfor_ActionNode;
            parentPath = parentPath + nodeInfor.m_Arrangement + splitchar;  //当前需要创建的树节点的完整路径
            TreeNodeBase node = GetGroupNodeByName(parentPath);  //判断是否存在这个路径的节点
            if (node == null)
            {
                node = new ActionTreeNode(nodeInfor.m_Arrangement, parentNode, nodeInfor.m_Attribute, nodeInfor.m_ActionScriptType);  //创建当前节点
                RecordGroupNodeInfor(parentPath, node); //记录当前创建的节点
            }//当前子目录不存在
        }


    }
}