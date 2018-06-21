using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace MFramework.EditorExpand
{
    /// <summary>
    /// 显示所有Unity内置的Style
    /// </summary>
    public class ShowBuildInStyleWindow : EditorWindow
    {

        static List<GUIStyle> styles = null;
        [MenuItem("Window/ShowBuildInStyle")]
        public static void Test()
        {
            EditorWindow.GetWindow<ShowBuildInStyleWindow>("styles");

            styles = new List<GUIStyle>();
            foreach (PropertyInfo fi in typeof(EditorStyles).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                object o = fi.GetValue(null, null);
                if (o.GetType() == typeof(GUIStyle))
                {
                    styles.Add(o as GUIStyle);
                }
            }
        }

        public Vector2 scrollPosition = Vector2.zero;
        void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < styles.Count; i++)
            {
                GUILayout.Label("EditorStyles." + styles[i].name, styles[i]);
            }
            GUILayout.EndScrollView();
        }
    }
}