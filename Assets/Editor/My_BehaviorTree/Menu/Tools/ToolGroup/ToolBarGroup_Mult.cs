using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 可以同时选中多个的Tool 组
    /// </summary>
    public class ToolBarGroup_Mult :ToolBarGroup
    {
        protected List<ToolBarParent> m_AllSelectTool = new List<ToolBarParent>();  //所有处于选中状态的Tool

        #region 构造函数

        public ToolBarGroup_Mult(List<ToolBarParent> tools, Vector2 startFrom)
        {
            m_AllControllToolBarItems = new List<ToolBarParent>();
            if (tools == null)
            {
                Debug.LogError("ToolBarGroup  Create Fail,No Tools");
                return;
            }
            m_StartPos = startFrom;
            m_AllControllToolBarItems.AddRange(tools);
        }


        public override void DrawToolBarGroup()
        {
            base.DrawToolBarGroup();
        }

        /// <summary>
        /// 检查当前Tool 是否被选中了
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CheckIsToolSelected(ToolBarParent item)
        {
            for (int dex = 0; dex < m_AllControllToolBarItems.Count; ++dex)
            {
                if (m_AllControllToolBarItems[dex] == item)
                {
                    return m_AllControllToolBarItems[dex].CheckWhethIsSelect();
                }
            }

            Debug.Log("CheckIfToolSelect Fail,Not Exit "+ item.m_TitleName);
            return false;
        }

        public override void OnToolItemClick(ToolBarParent item)
        {
            //base.OnToolItemClick(item);
            if(m_AllControllToolBarItems.Contains(item)==false)
            {
                Debug.Log("OnToolItemClick Fail" +item.m_TitleName);
                return;
            }
           if(CheckIsToolSelected(item))
            {
                Debug.Log(" OnToolItemClick  反选中");
                item.OnDeSelected();
            }
           else
            {
                Debug.Log("选中这个");
                item.OnSelected();
            }
        }

        #endregion

    }
}