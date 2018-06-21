using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 自定义的行为树编辑器
    /// </summary>
    public class BehaviorTreeWindow : EditorWindow
    {
        public static BehaviorTreeWindow BehaviorTreeWindow_Win { protected set; get; }

        #region  打开窗口菜单
        [MenuItem("Test/My_BehaviorTree_Editor")]
        static void OpenBehaviorTreeWin()
        {
            BehaviorTreeWindow_Win = EditorWindow.GetWindow<BehaviorTreeWindow>("行为树编辑器");
            BehaviorTreeWindow_Win.minSize = Constants.BehaviorTreeWinMinSize;
            BehaviorTreeWindow_Win.Show();

           // EditorApplication.update += OnEditorUpdate;
        }
        #endregion

        #region 事件管理
        public static MenuParent CurFocusMenu{   get; private set;  } //当前鼠标所在的窗口 此窗口处理大部分事件
        public static Vector2 CurMousePosition  { get; private set; } //当前鼠标的位置

        private static Vector2 m_PreviousMousePosition = Vector2.zero;

        public static Vector2 m_MouseDetail = Vector2.zero;

        #endregion
         static bool NeedRepaint = false;

        #region 属性和字段   //窗口菜单
        private string m_NodeEditorBgPath = "Assets/Editor/EditorResources/Grid128.png"; //背景图

        #region 子窗口菜单
        private List<MenuParent> m_AllRegisterMenu = new List<MenuParent>();  //所有注册的窗口菜单

        private NodeParametersWindow m_NodeParametersWindow_Menu = null;
        private NodeParametersWindow NodeParametersWindow_Menu
        {
            get
            {
                if (m_NodeParametersWindow_Menu == null)
                    m_NodeParametersWindow_Menu = new NodeParametersWindow(0, 0, Constants.NodeParametersWindowWidth, Screen.height, MenuParent.MenuAnchor.TOP_LEFT, "Node Parameter");

                return m_NodeParametersWindow_Menu;
            }
        } //左侧的节点参数菜单

        private BehaviorTreeEditorMenu m_BehaviorTreeEditorMenu = null;
        private BehaviorTreeEditorMenu BehaviorTreeEditorMenu_Menu
        {
            get
            {
                if (m_BehaviorTreeEditorMenu == null)
                    m_BehaviorTreeEditorMenu = new BehaviorTreeEditorMenu(Constants.NodeParametersWindowWidth, 0, Screen.width - Constants.NodeParametersWindowWidth,
                        Screen.height, MenuParent.MenuAnchor.MIDDLE_RIGHT, "Node Editor Menu");

                return m_BehaviorTreeEditorMenu;
            }
        } //中间的编辑行为树编辑区
        #endregion

        #endregion

        #region Unity 标准框架
        private void OnEnable()
        {
            //  Debug.Log("OnEnable ");
            RegisterMenu(NodeParametersWindow_Menu);
            RegisterMenu(BehaviorTreeEditorMenu_Menu);
            MenuParent.OnGetFocusAct += OnMenuGetFocus;
        }
        private void OnDestroy()
        {
            Debug.Log("OnDestroy ");
            m_AllRegisterMenu.Clear();
            BehaviorTreeWindow_Win = null;
            CurFocusMenu = null;
            MenuParent.OnGetFocusAct -= OnMenuGetFocus;
        }
        private void OnGUI()
        {
            if(NeedRepaint)
            {
                NeedRepaint = false;
                Repaint();
            }

            m_MouseDetail = Event.current.mousePosition - m_PreviousMousePosition;
            m_PreviousMousePosition = CurMousePosition;

            #region 整体布局

            #region 绘制背景
            float scale = 1f;  //通过scale控制缩放
            // GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), m_MenuBg);  //背景
            //Debug.Log("position =" + position);
            //指定要显示在的屏幕区域
            Rect showArea = new Rect(0, 0, Screen.width, Screen.height);
            //指定要显示的图片区域 ***由于Unity UV与Unity坐标轴不一样 
            Texture editorBgImg = EditorImageHelper.GetImageByPath(m_NodeEditorBgPath);

            Rect sourceRect = new Rect(0, -Screen.height / editorBgImg.height, scale * Screen.width / editorBgImg.width, scale * Screen.height / editorBgImg.height);
            GUI.DrawTextureWithTexCoords(showArea, editorBgImg, sourceRect); //根据纹理绘制一张贴图  这里纹理的WrapMode=Repeat;
            #endregion

            #region 绘制 左右两个窗口
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginHorizontal();
            NodeParametersWindow_Menu.DrawMenu();  //节点信息区
            m_BehaviorTreeEditorMenu.DrawMenu(); //编辑区
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            #endregion

            #endregion

            CurMousePosition = Event.current.mousePosition;  //光标相对于当前窗口的位置
            // Debug.Log("m_MousePosition=" + CurMousePosition);
            HandleEvent();  //处理GUI事件

        }

        #endregion

        #region  事件的处理

        /// <summary>
        /// 处理GUI事件
        /// </summary>
        private void HandleEvent()
        {
            Event e = Event.current;
            if (e != null && CurFocusMenu != null)
                CurFocusMenu.OnHandleEvent(e);  //分发处理事件
        }

        /// <summary>
        /// 子窗口报告获取焦点
        /// </summary>
        /// <param name="manu"></param>
         void OnMenuGetFocus(MenuParent manu)
        {
            if (manu != null)
                CurFocusMenu = manu;
        }

        #endregion

        #region 菜单管理
        /// <summary>
        /// 注册窗口
        /// </summary>
        /// <param name="menu"></param>
        void RegisterMenu(MenuParent menu)
        {
            if (menu == null)
            {
                Debug.LogError("RegisterMenu Fail, Is Null");
                return;
            }
            if (m_AllRegisterMenu.Contains(menu))
            {
                Debug.LogError("RegisterMenu Fail,AllReady Register " + menu.m_MenuName);
                return;
            }
            m_AllRegisterMenu.Add(menu);
        }
        /// <summary>
        /// 根据类型和菜单名获取菜单窗口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="titleName"></param>
        /// <returns></returns>
        public MenuParent GetRegisterMenu(Type type, string titleName)
        {
            for (int dex = 0; dex < m_AllRegisterMenu.Count; ++dex)
            {
                if (m_AllRegisterMenu[dex].GetType() == type)
                {
                    if (string.IsNullOrEmpty(titleName))
                        return m_AllRegisterMenu[dex];
                    if (m_AllRegisterMenu[dex].m_MenuName == titleName)
                        return m_AllRegisterMenu[dex];
                }
            }
            Debug.LogError("GetRegisterMenu  Fail," + titleName + "  Type=" + type + " Not Exit");
            return null;
        }


        #endregion


        #region 对外辅助接口
        public static void ForceRepaintView()
        {
            NeedRepaint = true;
        }
        #endregion

    }
}