using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenPathCurveEditorTool))]
public class TweenPathCurveEditorTool_Editor : Editor
{
    private int m_InputIndex = 0;
    private string m_InputStr = "0";

    private int m_InputIndex_Sub = 0;
    private string m_InputStr_Sub = "0";

    private bool m_IsShowOperateSubItem = false;  //显示操作子菜单的工具栏
    private bool m_IsShowToolButton = false;
    TweenPathCurveEditorTool m_TweenPathCurveEditorTool;



    public override void OnInspectorGUI()
    {
        m_TweenPathCurveEditorTool = target as TweenPathCurveEditorTool;

        GUILayout.BeginVertical();
        m_IsShowToolButton = GUILayout.Toggle(m_IsShowToolButton, "是否显示曲线项的编辑工具按钮");
        if (m_IsShowToolButton)
        {

        #region  工具栏

        #region  操作主 TweenCurveInfor 的工具栏
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            Debug.LogInfor("OnInspectorGUI  Add");
            m_TweenPathCurveEditorTool.AddItem(m_InputIndex);
        }

        m_InputStr = GUILayout.TextField(m_InputStr.ToString(), GUILayout.Width(50));
        if (string.IsNullOrEmpty(m_InputStr)) m_InputStr = "0";
        if (int.TryParse(m_InputStr, out m_InputIndex) == false)
        {
            Debug.LogError("输入整形索引Index");
        }

        if (GUILayout.Button("Delete"))
        {
            Debug.LogInfor("OnInspectorGUI  Delete");
            m_TweenPathCurveEditorTool.DeleteItem(m_InputIndex);

        }
        GUILayout.EndHorizontal();
        #endregion

        #region 操作子曲线
        m_IsShowOperateSubItem = GUILayout.Toggle(m_IsShowOperateSubItem, "显示子曲线操作接口");
        if (m_IsShowOperateSubItem)
        {

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add SubItem"))
            {
                Debug.LogInfor("OnInspectorGUI  AddSubItem");
                m_TweenPathCurveEditorTool.AddSubItem(m_InputIndex, m_InputIndex_Sub);
            }

            m_InputStr_Sub = GUILayout.TextField(m_InputStr_Sub.ToString(), GUILayout.Width(50));
            if (string.IsNullOrEmpty(m_InputStr_Sub)) m_InputStr_Sub = "0";
            if (int.TryParse(m_InputStr_Sub, out m_InputIndex_Sub) == false)
            {
                Debug.LogError("输入整形索引Index");
            }

            if (GUILayout.Button("Delete SubItem"))
            {
                Debug.LogInfor("OnInspectorGUI  DeleteSubItem");
                m_TweenPathCurveEditorTool.DeleteSubItem(m_InputIndex, m_InputIndex_Sub);
            }
            GUILayout.EndHorizontal();
        }
            #endregion

            #endregion
        }


        base.OnInspectorGUI();
        GUILayout.EndVertical();
    }
}
