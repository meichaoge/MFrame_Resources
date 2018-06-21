using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MFramework.BehaviorTree
{
    public class ToolBarWithTexture : ToolBarParent
    {
        public string m_ImagePath = "";  //图片的路径
        public Rect m_ImageArea;   //图片显示区域
        protected Texture2D m_Img;

        #region 构造函数

        public ToolBarWithTexture(string title, string tips, float space, float width, ToolBarStateEnum state) : 
            base(title, tips, space, width, state)
        {

        }

        /// <summary>
        /// 创建一个带有纹理的工具栏
        /// </summary>
        /// <param name="title"></param>
        /// <param name="tips"></param>
        /// <param name="space"></param>
        /// <param name="width"></param>
        /// <param name="img"></param>
        /// <param name="rect">相对范围</param>
        /// <param name="state"></param>
        public ToolBarWithTexture(string title, string tips, float space, float width, string img,Rect rect,ToolBarStateEnum state) :
          base(title, tips, space, width, state)
        {
            m_ImagePath = img;
            m_ImageArea = rect;

            m_Img = AssetDatabase.LoadAssetAtPath(m_ImagePath, typeof(Texture2D)) as Texture2D;

            if(m_Img==null)
            {
                Debug.LogError("ToolBarWithTexture Create Fail ,Image Not Exit " + m_ImagePath);
            }
            else
            {
                if (string.IsNullOrEmpty(m_ToolTips))
                    m_GUIContent = new GUIContent(m_TitleName, m_Img);
                else
                    m_GUIContent = new GUIContent(m_TitleName, m_Img,m_ToolTips );
            }
        }

        public override void DrawToolItem(Vector2 startPos, ToolBarGroup group)
        {
            GUILayout.BeginArea(new Rect(startPos.x, startPos.y, m_Width, group.m_ToolBarHeight));

            Rect rec = new Rect(startPos.x + m_ImageArea.x, startPos.y + m_ImageArea.y, m_ImageArea.width, m_ImageArea.height);
            GUI.DrawTexture(rec, m_Img);  //绘制背景
        //    Debug.Log(m_TitleName + "DrawToolItem1 : " + rec);
          //  Debug.Log(m_TitleName + "DrawToolItem 2: " + m_Width + "  height= " + group.m_ToolBarHeight);
            if (GUILayout.Button(m_GUIContent, GUILayout.Width(m_Width),GUILayout.Height(group.m_ToolBarHeight)))
            {
                group.OnToolItemClick(this);
            }

            GUILayout.EndArea();
        }

        #endregion



    }
}