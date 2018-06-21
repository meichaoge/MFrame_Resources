using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 只有一个能选中 的工具栏组
    /// </summary>
    public class ToolBarGroup_Signal : ToolBarGroup
    {
        public int m_SelectToolIndex = 0;  //当前选中的Tool
       
        public ToolBarGroup_Signal(List<ToolBarParent> tools, Vector2  startFrom) 
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


        /// <summary>
        /// 设置默认选中的Tool
        /// </summary>
        /// <param name="select"></param>
        public void SetDefaultSelect(int select)
        {
            if(select>=0&& select< m_AllControllToolBarItems.Count)
            m_SelectToolIndex = select;
            else
            {
                Debug.LogError("SetDefaultSelect  Fail, Not Avaliable "+select);
            }
        }

        public override void DrawToolBarGroup()
        {
            base.DrawToolBarGroup();

            if (m_SelectToolIndex >= 0 && m_SelectToolIndex < m_AllControllToolBarItems.Count)
                m_AllControllToolBarItems[m_SelectToolIndex].OnSelected(); //选中
        }

        public override void OnToolItemClick(ToolBarParent item)
        {
            bool isFind = false;
            for (int dex=0;dex< m_AllControllToolBarItems.Count;++dex)
            {
                if (m_AllControllToolBarItems[dex] == item)
                {
                    isFind = true;
                    m_SelectToolIndex = dex;
                }
            }

            if(isFind)
            {
                Debug.Log("选中工具栏 " + item.m_TitleName);
            }
        }



    }
}