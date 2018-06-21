using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 窗口菜单栏的基类  
    /// </summary>
    public class MenuParent 
    {
        /// <summary>
        /// 窗口的锚定模式
        /// </summary>
        public enum MenuAnchor
        {
            TOP_LEFT = 0,
            TOP_CENTER,
            TOP_RIGHT,
            MIDDLE_LEFT,
            MIDDLE_CENTER,
            MIDDLE_RIGHT,
            BOTTOM_LEFT,
            BOTTOM_CENTER,
            BOTTOM_RIGHT,
            NONE
        }

        #region 共有属性
        public string m_MenuName { get; protected set; }  //窗口TitleName
        protected Rect m_MaximizedArea;  //最大化时候显示的大小
        public bool m_IsActive { get; protected set; } //是否处于活跃显示状态
        protected MenuAnchor m_MenuAnchor; //锚定模式
        public Rect m_CurShowArea { get; protected set;}  //当前显示的区域大小

        //protected virtual Texture2D m_MenuBg { get; private set; }

        #endregion

        public static System.Action<MenuParent> OnGetFocusAct = null;


        #region 构造函数
        public MenuParent(float x,float y,float width,float height, MenuAnchor anchor,string titleName)
        {
            m_MenuName = titleName;
            m_MaximizedArea = new Rect(x, y, width, height);
            m_MenuAnchor = anchor;
            m_CurShowArea = m_MaximizedArea;
        }

        public MenuParent(Rect rect, MenuAnchor anchor, string titleName)
        {
            m_MenuName = titleName;
            m_MaximizedArea = rect;
            m_MenuAnchor = anchor;
            m_CurShowArea = rect;
        }
        #endregion


        #region 绘制UI 和更新属性

        /// <summary>
        /// 被窗口类调用 绘制GUI
        /// </summary>
        public virtual void DrawMenu()
        {
            UpdateMenu();
            if (Event.current != null)
                CheckIfGetFocus(Event.current.mousePosition);
        }

        /// <summary>
        /// 跟新菜单信息
        /// </summary>
        protected virtual void UpdateMenu()
        {
        }
        #endregion

        #region 事件和状态维护

        /// <summary>
        /// 确定一个点是否在当前的控制范围
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public virtual bool IsInside(Vector2 pos)
        {
            return m_CurShowArea.Contains(pos);
        }
         /// <summary>
         /// 检查是否获得了焦点
         /// </summary>
         /// <param name="pos"></param>
        public virtual bool  CheckIfGetFocus(Vector2 pos)
        {
            return IsInside(pos);
        }

        protected void OnGetFocus(MenuParent menu)
        {
            if (OnGetFocusAct != null)
                OnGetFocusAct(menu);
        }

        /// <summary>
        ///子菜单处理事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns>返回是否处理改事件</returns>
        public virtual void OnHandleEvent(Event e )
        {
          
        }

        #endregion


    }
}