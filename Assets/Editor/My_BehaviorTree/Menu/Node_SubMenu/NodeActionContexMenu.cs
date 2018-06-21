using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 节点编辑区右键菜单项 显示节点Action树结构
    /// </summary>
    public class NodeActionContexMenu : ContexMenuParent
    {
        private string m_NodeBgPath = "Assets/Editor/EditorResources/ActionNodeItembg.png"; //背景图
        private Vector2 m_ScrollPosition = Vector2.zero;
        private const float m_TopSpaceHeight = 10;  //树形结构与工具之间的间隔

        private Vector2 offset = new Vector2(20, 20);

        private NodeActionTreeView m_NodeActionTreeView;
        /// <summary>
        /// 定义的所有Action 节点树形视图
        /// </summary>
        public NodeActionTreeView NodeActionTreeView_View
        {
            get
            {
                if (m_NodeActionTreeView == null)
                    InitialTreeView();
                return m_NodeActionTreeView;
            }
        }
 //       private List<TreeViewNodeInfor> m_AllActionAttribute;//所有定义的节点
        public TreeNodeBase CurSelectTreeNode { get; private set; }
        private int m_DefaultSelectItem = 0; //默认选中的对象



        #region 构造函数
        public NodeActionContexMenu(float x, float y, float width, float height, MenuAnchor anchor, string titleName) :
            base(x, y, width, height, anchor, titleName)
        {

        }

        public NodeActionContexMenu(Rect rect, MenuAnchor anchor, string titleName):
            base(rect, anchor, titleName)
        {

        }
        #endregion

        #region 绘制UI

        public override void DrawMenu()
        {
            if (IsActive == false)
            {
                //Debug.Log("处于非激活状态" + m_MenuName);
                return;
            }
            base.DrawMenu();
        //    Debug.Log("DrawMenu  " + m_CurShowArea);
           
            GUILayout.BeginArea(m_CurShowArea);
            GUI.DrawTexture(new Rect(0,0, m_CurShowArea.width, m_CurShowArea.height), EditorImageHelper.GetImageByPath(m_NodeBgPath));
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition, false, true, GUILayout.Width(Constants.ContexNodeActionMenuSize.x),
                GUILayout.Height(Constants.ContexNodeActionMenuSize.y));

            // Debug.Log("<<<<<<<< " + m_CurShowArea.height+ "     m_TopSpaceHeight="+ m_TopSpaceHeight);
            //***添加一个空白的标签以便于正确的识别树形结构的高度
            float treeViewHeight = NodeActionTreeView_View.ShowTreeView();
            GUILayout.Label("", GUILayout.Height(treeViewHeight));

            GUILayout.EndScrollView();
            GUILayout.EndArea();

            //Debug.Log("xxx" + Event.current.mousePosition);
        }
        protected override void UpdateMenu()
        {
            //Vector2 pos = Vector2.zero;
            //Debug.Log("BehaviorTreeWindow_Win  " + BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition + "     m_CurShowArea=" + m_CurShowArea);
            //if (BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.x + Constants.ContexNodeActionMenuSize.x > m_ParentAttachMenu.m_CurShowArea.x+ m_ParentAttachMenu. m_CurShowArea.width)
            //{
            //    if (BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.x - Constants.ContexNodeActionMenuSize.x < m_ParentAttachMenu.m_CurShowArea.x) //向左也会越界
            //        pos.x = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.x - Constants.ContexNodeActionMenuSize.x / 2f;  //剧中
            //    else
            //        pos.x = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.x - Constants.ContexNodeActionMenuSize.x;  //以光标点为结束点摆放
            //}///直接在鼠标位置为原点放置鼠标会越界
            //else
            //    pos.x = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.x + Constants.ContexNodeActionMenuSize.x;  //以光标点为结束点摆放

            //if (BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.y + Constants.ContexNodeActionMenuSize.y > m_ParentAttachMenu.m_CurShowArea.y+ m_ParentAttachMenu.m_CurShowArea.height)
            //{
            //    if (BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.y - Constants.ContexNodeActionMenuSize.y < m_ParentAttachMenu.m_CurShowArea.y)
            //        pos.y = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.y - Constants.ContexNodeActionMenuSize.y / 2f;  //剧中
            //    else
            //        pos.y = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.y - Constants.ContexNodeActionMenuSize.y;  //向上
            //}
            //else
            //    pos.y = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.y + Constants.ContexNodeActionMenuSize.y;  //向下


            //m_CurShowArea = new Rect(pos.x, pos.y, Constants.ContexNodeActionMenuSize.x, Constants.ContexNodeActionMenuSize.y);

            return;

            if(BehaviorTreeWindow.BehaviorTreeWindow_Win==null|| m_ParentAttachMenu==null)
            {
                Debug.Log("Error " + (BehaviorTreeWindow.BehaviorTreeWindow_Win == null) + "  " + (m_ParentAttachMenu == null));
                return;
            }
            Vector2 mouseReletivePos = new Vector2(BehaviorTreeWindow.CurMousePosition.x - m_ParentAttachMenu.m_CurShowArea.x,
                BehaviorTreeWindow.CurMousePosition.y - m_ParentAttachMenu.m_CurShowArea.y); //光标相对于父窗口的坐标
            Debug.Log("相对位置 " + mouseReletivePos);
            //Vector2 mouseReletivePos = new Vector2(m_ParentAttachMenu.m_CurShowArea.x,
            //    m_ParentAttachMenu.m_CurShowArea.y); //光标相对于父窗口的坐标

            m_CurShowArea = new Rect(mouseReletivePos.x+Constants.ContexNodeActionMenuSize.x, mouseReletivePos.y, Constants.ContexNodeActionMenuSize.x, Constants.ContexNodeActionMenuSize.y);

            Debug.Log("MousePos="+BehaviorTreeWindow.CurMousePosition + "m_CurShowArea  " + m_CurShowArea);
        }
        #endregion


        /// <summary>
        /// 初始化创建树
        /// </summary>
        private void InitialTreeView()
        {
           // Debug.Log("InitialTreeView");
            m_NodeActionTreeView = new NodeActionTreeView();

            List<TreeViewNodeInfor> m_AllActionAttribute = GetAllActionNodeScript.AllDefineAction; //遍历所有带有特定标签的脚本 获取设置的Action路径
            m_NodeActionTreeView.CreateTreeView(m_AllActionAttribute, m_DefaultSelectItem);
            m_NodeActionTreeView.OnSelectNodeItemChange += OnSelectNodeChange;
            CurSelectTreeNode = m_NodeActionTreeView.GetTreeNodeByIndex(m_DefaultSelectItem);
        }

        void OnSelectNodeChange(TreeNodeBase node)
        {
            CurSelectTreeNode = node;
            Vector2 pos = Event.current.mousePosition + new Vector2(m_CurShowArea.x, m_CurShowArea.y);
            BehaviorTreeEditorMenu menu = m_ParentAttachMenu as BehaviorTreeEditorMenu;
            if(menu!=null)
            {
                menu.RecordSelectNode(node as ActionTreeNode, pos);
            }
            ShowOrHideContex(false  );
        }

        public override bool CheckIfGetFocus(Vector2 pos)
        {
            //Debug.Log("检测是否获得了焦点"+ IsInside(pos)+ "pos="+ pos+ "m_CurShowArea="+ m_CurShowArea);
            return IsInside(pos);
        }


        public override void UpdateShowPosition(Vector2 pos)
        {
            m_CurShowArea = new Rect(pos.x+ Constants.ContexNodeActionMenuSize.x+ offset.x, pos.y+ offset.y, Constants.ContexNodeActionMenuSize.x, Constants.ContexNodeActionMenuSize.y);
        }


    }
}