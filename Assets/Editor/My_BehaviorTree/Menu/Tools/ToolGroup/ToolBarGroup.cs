using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 一组ToolBar Item 
    /// </summary>
    public class ToolBarGroup
    {
        protected List<ToolBarParent> m_AllControllToolBarItems = new List<ToolBarParent>();
        public float m_ToolBarHeight = 30;
        public float m_ToolBarWidth { get; protected set; }  //显示区域宽
        protected Vector2 m_StartPos = Vector2.zero;
        public Vector2 m_ToolGroupSize { get; protected set; }  //显示区域大小

       

        public virtual void DrawToolBarGroup()
        {
            GetToolBarWidth();
            GUILayout.BeginHorizontal();
            float itemOffset = 0;
            for (int dex = 0; dex < m_AllControllToolBarItems.Count; ++dex)
            {
                if (dex == 0)
                {
                    m_AllControllToolBarItems[0].DrawToolItem(m_StartPos, this);
                }//第一个不需要考虑间距
                else
                {
                    itemOffset += m_AllControllToolBarItems[dex - 1].m_Space;  //下一个ToolBat Item Offset
                    m_AllControllToolBarItems[dex].DrawToolItem(new Vector2(m_StartPos.x    + itemOffset, m_StartPos.y), this);
                } //每一个之间有一定的间距
                itemOffset+= m_AllControllToolBarItems[dex].m_Width;
            }

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 获得宽度
        /// </summary>
        protected virtual void GetToolBarWidth()
        {
            m_ToolBarWidth = 0;
            for (int dex = 0; dex < m_AllControllToolBarItems.Count; dex++)
            {
                if (dex != 0)
                    m_ToolBarWidth+= m_AllControllToolBarItems[dex].m_Space;
                m_ToolBarWidth += m_AllControllToolBarItems[dex].m_Width;
            }
        }

        /// <summary>
        /// 添加工具项
        /// </summary>
        /// <param name="item"></param>
        public virtual void AddToolBarItem(ToolBarParent item)
        {
            if(m_AllControllToolBarItems.Contains(item))
            {
                Debug.LogError("AddToolBarItem Fail,Exit" + item.m_TitleName);
                return;
            }
            m_AllControllToolBarItems.Add(item);
       //     DrawToolBarGroup();  //刷新
        }


        /// <summary>
        /// 当工具栏被点击时候调用
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnToolItemClick(ToolBarParent item)
        {

        }


      

    }
}