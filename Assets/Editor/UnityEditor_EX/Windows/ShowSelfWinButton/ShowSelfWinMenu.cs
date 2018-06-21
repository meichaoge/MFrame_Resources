using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MFramework.EditorExpand
{
    /// <summary>
    /// 自定义窗口右上角显示自己的图标
    /// </summary>
    public class ShowSelfWinMenu : EditorWindow
    {

        [MenuItem("Window/My Window With Self Icon")]
        static void Init()
        {
            ShowSelfWinMenu window = (ShowSelfWinMenu)EditorWindow.GetWindow(typeof(ShowSelfWinMenu));
            window.Show();
        }

        bool locked = false;

        private GUIStyle m_IconStyle = new GUIStyle();


        private void OnEnable()
        {
            Texture2D icon = Resources.Load<Texture2D>("SelfIcon");
            m_IconStyle.normal.background = icon;

        }

        void ShowButton(Rect rect)
        {
            locked = GUI.Toggle(rect, locked, GUIContent.none, "IN LockButton");
            rect.x -= 12.0f;
            GUI.Button(new Rect(rect.x, rect.y, 12, 12), GUIContent.none, m_IconStyle);

        }
    }
}