using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 左侧树形结构 包含所有定义的节点
    /// </summary>
    public class NodeActionMenu : MenuParent
    {
        private Vector2 m_ScrollPosition = Vector2.zero;
        private const float m_TopSpaceHeight = 10;  //树形结构与工具之间的间隔

        private NodeActionTreeView m_NodeActionTreeView;
        public NodeActionTreeView NodeActionTreeView_View
        {
            get
            {
                if (m_NodeActionTreeView == null)
                    InitialTreeView();
                return m_NodeActionTreeView;
            }
        }//定义的所有Action 节点树形视图

        public TreeNodeBase CurSelectTreeNode { get; private set; }
        private int m_DefaultSelectItem = 0; //默认选中的对象

        #region 构造函数

        public NodeActionMenu(float x, float y, float width, float height, MenuAnchor anchor, string titleName) :
            base(x, y, width, height, anchor, titleName)
        {

        }

        public NodeActionMenu(Rect rect, MenuAnchor anchor, string titleName) :
            base(rect, anchor, titleName)
        {

        }
        #endregion

        #region 绘制UI

        public override void DrawMenu()
        {
            base.DrawMenu();
            GUILayout.Space(m_TopSpaceHeight);
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition, false, true, GUILayout.Width(m_CurShowArea.width),
                GUILayout.Height(m_CurShowArea.height - m_TopSpaceHeight-100 ));

            // Debug.Log("<<<<<<<< " + m_CurShowArea.height+ "     m_TopSpaceHeight="+ m_TopSpaceHeight);
            //***添加一个空白的标签以便于正确的识别树形结构的高度
            float treeViewHeight = NodeActionTreeView_View.ShowTreeView(); //获取当前树形结构展开的高度值
            GUILayout.Label("", GUILayout.Height(treeViewHeight));

            GUILayout.EndScrollView();
        }
        protected override void UpdateMenu()
        {
            m_CurShowArea = new Rect(0, Constants.ToolBarHeight, Constants.NodeParametersWindowWidth, Screen.height - Constants.ToolBarHeight);
        }
        #endregion


        /// <summary>
        /// 初始化创建树
        /// </summary>
        private void InitialTreeView()
        {
            Debug.Log("InitialTreeView");
            m_NodeActionTreeView = new NodeActionTreeView();
            List<TreeViewNodeInfor> allActionAttribute = GetAllActionNodeScript.AllDefineAction; //遍历所有带有特定标签的脚本 获取设置的Action路径

            m_NodeActionTreeView.CreateTreeView(allActionAttribute, m_DefaultSelectItem);
            m_NodeActionTreeView.OnSelectNodeItemChange += OnSelectNodeChange;
            CurSelectTreeNode = m_NodeActionTreeView.GetTreeNodeByIndex(m_DefaultSelectItem);
        }

        //当选择了树形节点时候时间回调
        void OnSelectNodeChange(TreeNodeBase node)
        {
            CurSelectTreeNode = node;
        }



    }
}