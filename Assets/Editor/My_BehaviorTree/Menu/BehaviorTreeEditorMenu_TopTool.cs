using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace MFramework.BehaviorTree
{
    /// <summary>
    /// 行为树中间编辑器的上面工具栏
    /// </summary>
    public class BehaviorTreeEditorMenu_TopTool : MenuParent
    {
        private string m_BgImagePath = "Assets/Editor/EditorResources/ActionNodeItembg.png"; //背景图


        #region 构造函数

        public BehaviorTreeEditorMenu_TopTool(float x, float y, float width, float height, MenuAnchor anchor, string titleName) :
            base(x, y, width, height, anchor, titleName)
        {
            InitialToolBatItem();
        }

        public BehaviorTreeEditorMenu_TopTool(Rect rect, MenuAnchor anchor, string titleName):
            base(rect, anchor, titleName)
        {
            InitialToolBatItem();
        }
        #endregion

        private ToolBarGroup_Signal m_ToolBarGroup_Signal = null;
        public ToolBarGroup_Signal ToolBarGroup_Signal_Menu
        {
            get
            {
                if (m_ToolBarGroup_Signal == null)
                    m_ToolBarGroup_Signal = new ToolBarGroup_Signal(new List<ToolBarParent>(),new Vector2(m_MaximizedArea.x, m_MaximizedArea.y));
                return m_ToolBarGroup_Signal;
            }
        } //只能有一个选中的工具组

        /// <summary>
        /// 注册添加工具
        /// </summary>
        /// <param name="item"></param>
        public void RegisterTool (ToolBarParent item)
        {
            ToolBarGroup_Signal_Menu.AddToolBarItem(item);
        }

        public override void DrawMenu()
        {
            base.DrawMenu();
            GUI.DrawTexture(m_CurShowArea, EditorImageHelper.GetImageByPath(m_BgImagePath)); //背景

            ToolBarGroup_Signal_Menu.DrawToolBarGroup();
        }

        protected override void UpdateMenu()
        {
            m_CurShowArea = new Rect(Constants.NodeParametersWindowWidth, 5, Screen.width- Constants.NodeParametersWindowWidth,  Constants.ToolBarHeight);
        }


        /// <summary>
        /// 初始化生成工具箱
        /// </summary>
        private void InitialToolBatItem()
        {
            string img1 = "Assets/Editor/EditorResources/PreviewOff.png";
            Rect rect1 = new Rect(80, 0, 16, 16);
            ToolBarWithTexture tools_Left = new ToolBarWithTexture("Left","",10,100, img1, rect1, ToolBarParent.ToolBarStateEnum.Normal);
            RegisterTool(tools_Left);

            string img2 = "Assets/Editor/EditorResources/PreviewOn.png";
            Rect rect2 = new Rect(80, 0, 16, 16);
            ToolBarWithTexture tools_Right = new ToolBarWithTexture("Right", "", 10, 100, img2, rect2, ToolBarParent.ToolBarStateEnum.Normal);
            RegisterTool(tools_Right);

            ToolBarParent tools_infor1 = new ToolBarParent("Infor1","",20,150);
            RegisterTool(tools_infor1);

        }



    }
}