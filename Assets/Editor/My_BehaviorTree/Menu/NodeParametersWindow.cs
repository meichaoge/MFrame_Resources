using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 左侧的节点参数显示窗口
    /// </summary>
    public class NodeParametersWindow : MenuParent
    {
        /// <summary>
        /// 枚举需要显示的菜单项
        /// </summary>
        private enum ToolBarEnum
        {
            Action = 0, //Action 节点
            Variable,
            Inspector,
        }

        private string m_NodeBgPath = "Assets/Editor/EditorResources/ActionNodeItembg.png"; //背景图

        #region 工具栏
        private List<string> m_Tools; //当前窗口的工具栏选项
        public int SelectedToolIndex { get; private set; }  //选中的菜单栏
        #endregion

        #region  当前菜单包含的子菜单
        private NodeActionMenu m_NodeActionMenu = null;
        public NodeActionMenu NodeActionMenu_Menu
        {
            get
            {
                if (m_NodeActionMenu == null)
                    m_NodeActionMenu = new NodeActionMenu(new Rect(0, 0, Screen.width, Screen.height - Constants.ToolBarHeight), MenuAnchor.TOP_CENTER, "Action");
                return m_NodeActionMenu;
            }
        } //Action 节点

        #endregion

        public NodeParametersWindow(float x, float y, float width, float height, MenuAnchor anchor, string titleName) :
            base(x, y, width, height, anchor, titleName)
        {
            var tools = System.Enum.GetValues(typeof(ToolBarEnum));
            m_Tools = new List<string>();
            foreach (var tool in tools)
            {
                ToolBarEnum item = (ToolBarEnum)System.Enum.Parse(typeof(ToolBarEnum), tool.ToString());
                m_Tools.Add(item.ToString());
            }
            SelectedToolIndex = 0; //默认选中第0个工具栏
        }


        public override void DrawMenu()
        {
            base.DrawMenu();
            Rect MenuShowArea = new Rect(0, 0, Constants.NodeParametersWindowWidth, Screen.height - Constants.ToolBarHeight);  //菜单显示的区域 去除了工具栏的位置

            #region 绘制菜单显示区域的背景
            Color cor = GUI.color;
            GUI.color = Color.gray;  //更改当前绘制GUI的颜色
            GUI.DrawTexture(MenuShowArea, EditorImageHelper.GetImageByPath(m_NodeBgPath));
            GUI.color = cor;
            #endregion

            GUILayout.BeginVertical();
            #region 工具栏 也可以使用自己定义的工具栏
            SelectedToolIndex = GUILayout.Toolbar(SelectedToolIndex, m_Tools.ToArray(), GUILayout.Width(Constants.NodeParametersWindowWidth),
                                                    GUILayout.Height(Constants.ToolBarHeight));  //绘制工具栏
            #endregion

            #region 左侧的 窗口显示的内容  属性结构/节点属性/......
            GUILayout.BeginArea(new Rect(MenuShowArea.x, MenuShowArea.y+Constants.ToolBarHeight, MenuShowArea.width, MenuShowArea.height));
            //Debug.Log(">>>>> " + (Screen.height - Constants.ToolBarHeight));
            ShowSubMenuBySelectTool();
            GUILayout.EndArea();
            #endregion

            GUILayout.EndVertical();
        }

        protected override void UpdateMenu()
        {
            m_CurShowArea = new Rect(0, 0, Constants.NodeParametersWindowWidth, Screen.height);
        }

        /// <summary>
        /// 根据当前选择的工具栏显示不同的视图
        /// </summary>
        void ShowSubMenuBySelectTool()
        {
            //    Debug.Log("ShowSubMenuBySelectTool= " + (ToolBarEnum)SelectedToolIndex);
            switch (SelectedToolIndex)
            {
                case (int)ToolBarEnum.Action:
                    NodeActionMenu_Menu.DrawMenu();
                    break;
            }
        }


        MenuParent GetShowMenuBySelectedIndex()
        {
            switch (SelectedToolIndex)
            {
                case (int)ToolBarEnum.Action:
                    return NodeActionMenu_Menu;
            }
            return null;
        }

        #region 事件处理

        public override bool CheckIfGetFocus(Vector2 pos)
        {
            MenuParent menu = GetShowMenuBySelectedIndex();
            if (menu != null && menu.CheckIfGetFocus(pos))
            {
                OnGetFocus(menu);
                return false;
            }//子窗口获得焦点
            if (m_CurShowArea.Contains(pos))
            {
                OnGetFocus(this);
                return true;
            }//自己获得焦点
            return false;
        }

        public override void OnHandleEvent(Event e)
        {
            if (BehaviorTreeWindow.CurFocusMenu != this) return;
            switch (e.type)
            {
                //case EventType.:
                //    Debug.Log(m_MenuName + "::  " + e.type);
                //    break;
                case EventType.ContextClick: //右键
                    Debug.Log(m_MenuName + "::  " + e.type);
                    break;
            }

        }
        #endregion

    }
}
