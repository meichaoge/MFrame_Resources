using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 工具栏的父类
    /// </summary>
    public class ToolBarParent
    {
        public enum ToolBarStateEnum
        {
            Disable,
            Normal,
            Hover,    //光标悬停
            Selected,  //选中
        }
        public string m_TitleName;
        public string m_ToolTips;
        public float m_Space = 10; //与前一个Tool 的间距
        public float m_Width = 100;  //大小
        public ToolBarStateEnum m_ToolBarStateEnum = ToolBarStateEnum.Normal;
        protected GUIContent m_GUIContent; //Content

        public ToolBarParent(string title, string tips, float space, float width, ToolBarStateEnum state=ToolBarStateEnum.Normal)
        {
            m_TitleName = title;
            m_ToolTips = tips;
            m_Space = space;
            m_Width = width;
            m_ToolBarStateEnum = state;

            if (string.IsNullOrEmpty(m_ToolTips))
                m_GUIContent = new GUIContent(m_TitleName);
            else
                m_GUIContent = new GUIContent(m_TitleName, m_ToolTips);
        }

        /// <summary>
        /// 绘制一个工具栏
        /// </summary>
        public virtual void DrawToolItem(Vector2 startPos, ToolBarGroup group)
        {
            //Debug.Log(m_TitleName + "DrawToolItem3 : " + new Rect(startPos.x, startPos.y, m_Width, group.m_ToolBarHeight));
            if (GUI.Button(new Rect(startPos.x, startPos.y, m_Width, group.m_ToolBarHeight - 5), m_GUIContent))
            {
                group.OnToolItemClick(this);
            }

            //if (GUILayout.Button(m_GUIContent,GUILayout.Width(m_Width),GUILayout.Height(group.m_ToolBarHeight-5)))
            //{
            //    group.OnToolItemClick(this);
            //}
        }

        #region 工具栏的交互
        public virtual void OnSelected()
        {
        }

        public virtual void OnDeSelected()
        {
        }


        public virtual void OnDisable()
        {
        }

        public virtual void OnLock()
        {

        }

        #endregion

        #region  状态
        /// <summary>
        /// 检查是是否处于被选中状态
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckWhethIsSelect()
        {
            if (m_ToolBarStateEnum >= ToolBarStateEnum.Selected)
                return true;
            return false;
        }
        #endregion

    }
}