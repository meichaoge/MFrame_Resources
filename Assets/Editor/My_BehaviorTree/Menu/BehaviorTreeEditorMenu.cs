using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 行为树中间的编辑区
    /// </summary>
    public class BehaviorTreeEditorMenu : MenuParent
    {
        protected Dictionary<string, EditorRecordNode> m_AllShowingNode = new Dictionary<string, EditorRecordNode>(); //所有创建的节点
        protected Dictionary<string, EditorRecordNode> m_AllSelectedNodes = new Dictionary<string, EditorRecordNode>(); //选中的节点 会控制移动
        protected static HashSet<NodeParent.LinConnectInfor> m_AllConnectedLinePoints = new HashSet<NodeParent.LinConnectInfor>(); //所有的连线

        #region 构造函数
        public BehaviorTreeEditorMenu(float x, float y, float width, float height, MenuAnchor anchor, string titleName) :
            base(x, y, width, height, anchor, titleName)
        {

        }

        public BehaviorTreeEditorMenu(Rect rect, MenuAnchor anchor, string titleName) :
            base(rect, anchor, titleName)
        {

        }
        #endregion

        #region 子菜单

        #region 工具栏
        private BehaviorTreeEditorMenu_TopTool m_BehaviorTreeEditorMenu_TopTool;
        public BehaviorTreeEditorMenu_TopTool BehaviorTreeEditorMenu_TopTool_Menu
        {
            get
            {
                if (m_BehaviorTreeEditorMenu_TopTool == null)
                    m_BehaviorTreeEditorMenu_TopTool = new BehaviorTreeEditorMenu_TopTool(Constants.NodeParametersWindowWidth, 0, Screen.width - Constants.NodeParametersWindowWidth,
                        Constants.ToolBarHeight, MenuAnchor.TOP_CENTER, "BehaviorTreeEditorTools");

                return m_BehaviorTreeEditorMenu_TopTool;
            }
        } //顶部的工具栏
        #endregion

        #region 右键的节点菜单
        private NodeActionContexMenu m_NodeActionContexMenu = null;
        public NodeActionContexMenu NodeActionContexMenu_ContexMenu
        {
            get
            {

                if (m_NodeActionContexMenu == null)
                {
                    //Vector2 pos = Vector2.zero;
                    //Debug.Log("BehaviorTreeWindow_Win  " + BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition + "     m_CurShowArea=" + m_CurShowArea);
                    //if (BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.x + Constants.ContexNodeActionMenuSize.x > m_CurShowArea.width)
                    //{
                    //    if (BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.x - Constants.ContexNodeActionMenuSize.x < 0) //向左也会越界
                    //        pos.x = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.x - Constants.ContexNodeActionMenuSize.x / 2f;  //剧中
                    //    else
                    //        pos.x = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.x - Constants.ContexNodeActionMenuSize.x;  //以光标点为结束点摆放
                    //}///直接在鼠标位置为原点放置鼠标会越界
                    //else
                    //    pos.x = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.x + Constants.ContexNodeActionMenuSize.x;  //以光标点为结束点摆放

                    //if (BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.y + Constants.ContexNodeActionMenuSize.x > m_CurShowArea.height)
                    //{
                    //    if (BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.y - Constants.ContexNodeActionMenuSize.y < 0)
                    //        pos.y = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.y - Constants.ContexNodeActionMenuSize.y / 2f;  //剧中
                    //    else
                    //        pos.y = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.y - Constants.ContexNodeActionMenuSize.y;  //向上
                    //}
                    //else
                    //    pos.y = BehaviorTreeWindow.BehaviorTreeWindow_Win.CurMousePosition.y + Constants.ContexNodeActionMenuSize.y;  //向下


                    //m_CurShowArea = new Rect(pos.x, pos.y, Constants.ContexNodeActionMenuSize.x, Constants.ContexNodeActionMenuSize.y);
                    //Debug.Log("m_CurShowArea  " + m_CurShowArea);

                    //m_NodeActionContexMenu = new NodeActionContexMenu(Constants.NodeParametersWindowWidth, 0, Constants.ContexNodeActionMenuSize.x, Constants.ContexNodeActionMenuSize.y,
                    //    MenuAnchor.NONE,"Context Node Action");
                    Vector2 mouseReletivePos = Vector2.zero;
                    if (Event.current != null)
                    {
                        mouseReletivePos = new Vector2(Event.current.mousePosition.x - m_CurShowArea.x, Event.current.mousePosition.y - m_CurShowArea.y); //光标相对于父窗口的坐标
                    }

                    //Debug.Log("相对位置 " + mouseReletivePos);
                    Rect area = new Rect(mouseReletivePos.x + Constants.ContexNodeActionMenuSize.x, mouseReletivePos.y, Constants.ContexNodeActionMenuSize.x, Constants.ContexNodeActionMenuSize.y);

                    //Debug.Log("area=" + area);
                    m_NodeActionContexMenu = new NodeActionContexMenu(area, MenuAnchor.NONE, "Context Node Action");
                }

                m_NodeActionContexMenu.m_ParentAttachMenu = this;
                return m_NodeActionContexMenu;
            }
        }
        #endregion

        #endregion

        #region  绘制
        public override void DrawMenu()
        {
            base.DrawMenu();
            BehaviorTreeEditorMenu_TopTool_Menu.DrawMenu();  //显示工具栏
            NodeActionContexMenu_ContexMenu.DrawMenu();
       

            DrawNodeConnectLine();

            DrawNodes();
        }
        protected override void UpdateMenu()
        {
            m_CurShowArea = new Rect(Constants.NodeParametersWindowWidth, 0, Screen.width - Constants.NodeParametersWindowWidth, Screen.height);
        }

        /// <summary>
        /// 绘制所有记录的节点
        /// </summary>
        void DrawNodes()
        {
            foreach (var node in m_AllShowingNode.Values)
            {
                if (node.m_EditorNode != null)
                    node.m_EditorNode.DrawNode(node);
            }
        }
        /// <summary>
        /// 绘制连线
        /// </summary>
        void DrawNodeConnectLine()
        {
            if (NodeParent.m_SelectNodeLineBegin != null)
            {
                //Debug.Log("rttttttttttttt");
                GLDraw.DrawBezier(NodeParent.m_SelectNodeLineBegin.m_Pos, Event.current.mousePosition, 100, Color.red, 2);
            }
            //  Debug.Log("DrawNodeConnectLine " + m_AllConnectedLinePoints.Count);
            foreach (var item in m_AllConnectedLinePoints)
            {
                GLDraw.DrawBezier(item.m_BeginNodeLintPoint.m_Pos, item.m_EndNodeLintPoint.m_Pos, 100, Color.green, 2);
            }//绘制所有的连线

        }

        #endregion

        #region 事件处理
        public override bool CheckIfGetFocus(Vector2 pos)
        {
            if (NodeActionContexMenu_ContexMenu != null)
            {
                if (NodeActionContexMenu_ContexMenu.CheckIfGetFocus(pos))
                {
                    //Debug.Log("子窗口获得焦点 " + NodeActionContexMenu_ContexMenu);
                    OnGetFocus(NodeActionContexMenu_ContexMenu);
                    return false;
                }//子窗口获得焦点
            }

            if (m_CurShowArea.Contains(pos))
            {
                OnGetFocus(this);
                return true;
            }//自己获得焦点
            return false;
        }

        public override bool IsInside(Vector2 pos)
        {
            if (NodeActionContexMenu_ContexMenu != null && NodeActionContexMenu_ContexMenu.IsActive)
            {
                if (NodeActionContexMenu_ContexMenu.IsInside(pos))
                    return false; //右键菜单响应鼠标操作
            }
            return base.IsInside(pos);
        }

        public override void OnHandleEvent(Event e)
        {
            if (BehaviorTreeWindow.CurFocusMenu != this)
            {
                Debug.Log("OnHandleEvent  " + BehaviorTreeWindow.CurFocusMenu);
                return;
            }
            //Debug.Log("vvvvvvvvvvvvv " + e.type);
            switch (e.type)
            {
                case EventType.MouseDown:
                    UpdateNodesState(true);
                    CheckIfTryMoveNode();
                    Debug.Log(m_MenuName + "MouseDown::  " + e.type);
                    if (NodeActionContexMenu_ContexMenu != null)
                        NodeActionContexMenu_ContexMenu.ShowOrHideContex(false);
                    break;
                case EventType.ContextClick: //右键
                    BehaviorTreeWindow.ForceRepaintView(); //重绘
                    Debug.Log(m_MenuName + "ContextClick::  " + e.type);
                    if (NodeActionContexMenu_ContexMenu != null)
                    {
                        NodeActionContexMenu_ContexMenu.ShowOrHideContex(true);
                        Vector2 mouseReletivePos = Event.current.mousePosition - new Vector2(m_CurShowArea.x, m_CurShowArea.y); //光标相对于父窗口的坐标
                        m_NodeActionContexMenu.UpdateShowPosition(mouseReletivePos);  //更新右键菜单的位置
                    }
                    //Debug.Log(m_MenuName + "::  " + e.type);
                    break;
                case EventType.DragPerform:
                    //    Debug.Log("EventType.DragPerform");
                    break;

                case EventType.MouseDrag:
                    //Debug.Log("EventType.MouseDrag");
                    BehaviorTreeWindow.ForceRepaintView(); //重绘
                    if (NodeParent.m_SelectNodeLineBegin == null)
                    {
                        foreach (var node in m_AllSelectedNodes)
                            node.Value.m_EditorNode.MoveNode(BehaviorTreeWindow.m_MouseDetail);
                    }
                    break;
                case EventType.MouseUp:
                    UpdateNodesState(false);
                    //Debug.Log("EventType.MouseUp");
                    m_AllSelectedNodes.Clear();
                    TryConnectLinePoint();
                    NodeParent.m_SelectNodeLineBegin = NodeParent.m_SelectNodeLineEnd = null;
                    BehaviorTreeWindow.ForceRepaintView(); //重绘
                    break;

            }

        }

        #endregion


        #region 移动节点和选择连线点/记录连线

        /// <summary>
        /// 检测是否鼠标按下时候是在节点连接点上 想要连线
        /// </summary>
        void UpdateNodesState(bool isPointDown)
        {
            //  bool isSelectedLinePoint = false;
            foreach (var node in m_AllShowingNode)
            {
                node.Value.m_EditorNode.UpdateNodeSelectState(Event.current.mousePosition, isPointDown);
                //if (node.Value.m_EditorNode.IsSelectNodeLinePoint)
                //{
                //   // isSelectedLinePoint = true;
                //    continue;  //说明选中了一个连线点
                //}
            }
            //if (isSelectedLinePoint == false)
            //    NodeParent.m_SelectNodeLine = null;
        }
        /// <summary>
        /// 检测是否想要移动节点
        /// </summary>
        void CheckIfTryMoveNode()
        {
            if (NodeParent.m_SelectNodeLineBegin != null)
                return;
            foreach (var node in m_AllShowingNode)
            {
                if (node.Value.m_EditorNode.IsSelectNode)
                {
                    if (m_AllSelectedNodes.Count >= 1)
                        break; //目前只能选中一个
                    if (m_AllSelectedNodes.ContainsKey(node.Key) == false)
                        m_AllSelectedNodes.Add(node.Key, node.Value);
                    break;
                }//选择了需要跟随移动的节点
            }
        }
        /// <summary>
        /// 尝试链接两个节点
        /// </summary>
        void TryConnectLinePoint()
        {
            NodeParent.NodeLineInfor. TryAddLineConnect(NodeParent.m_SelectNodeLineBegin, NodeParent.m_SelectNodeLineEnd);
        }

        /// <summary>
        /// 节点链接玩回调
        /// </summary>
        /// <param name="connected"></param>
       public static void RecordConnectedLine(NodeParent.LinConnectInfor connected)
        {
            foreach (var item in m_AllConnectedLinePoints)
            {
                if(item.m_BeginNodeLintPoint==connected.m_BeginNodeLintPoint&&
                    item.m_EndNodeLintPoint == connected.m_EndNodeLintPoint)
                {
                    return;
                }
            }
            //Debug.Log("RecordConnectedLine " + connected.m_BeginNodeLintPoint+"  ::: " + connected.m_EndNodeLintPoint);
            m_AllConnectedLinePoints.Add(connected);
        }

        #endregion

        #region 选择树形节点
        public void RecordSelectNode(ActionTreeNode node, Vector2 relativePos)
        {
            EditorRecordNode recordNode = new EditorRecordNode(node, relativePos);
            while (m_AllShowingNode.ContainsKey(recordNode.m_UID))
            {
                recordNode.m_UID = GenerateUID.GetUID64();
            }
            m_AllShowingNode.Add(recordNode.m_UID, recordNode);
        }
        #endregion


    }



}