using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TransformListenerTool))]
public class ListenTransformChange_Editor : Editor
{
    TransformListenerTool m_TransformListenerToolScript;


    public override void OnInspectorGUI()
    {
        m_TransformListenerToolScript = target as TransformListenerTool;


        //�������
        m_TransformListenerToolScript.UpdateCheckRate = (TransformListenerTool.CheckRate)EditorGUILayout.EnumPopup(new GUIContent("Check Rate"), m_TransformListenerToolScript.UpdateCheckRate);

        //�Ƿ���������
        m_TransformListenerToolScript.ClearRecordEventOnHide = EditorGUILayout.Toggle("Clear OnHide", m_TransformListenerToolScript.ClearRecordEventOnHide);

        EditorGUILayout.Space();

      


        m_TransformListenerToolScript.ListenPositionChange = EditorGUILayout.Toggle(new GUIContent("Listen Position Change"), m_TransformListenerToolScript.m_ListenPositionChange);
        if (m_TransformListenerToolScript.ListenPositionChange)
        {
            //���� Position
            m_TransformListenerToolScript.IsListenLocalTransPosition = EditorGUILayout.Toggle(new GUIContent("Listen LocalSpace "), m_TransformListenerToolScript.IsListenLocalTransPosition);//��������
        }
      


        m_TransformListenerToolScript.m_PositionSensitivity = EditorGUILayout.Slider("PositionSensitivity", m_TransformListenerToolScript.m_PositionSensitivity, 0.001f, 100f);

        //���� Scale
        m_TransformListenerToolScript.ListenScaleChange = EditorGUILayout.Toggle(new GUIContent("Listen Scale Change"), m_TransformListenerToolScript.m_ListenScaleChange);
        m_TransformListenerToolScript.m_ScaleSensitivity = EditorGUILayout.Slider("ScaleSensitivity", m_TransformListenerToolScript.m_ScaleSensitivity,0.001f,1);

        //���� Rotation
        m_TransformListenerToolScript.ListenRotationChange = EditorGUILayout.Toggle(new GUIContent("Listen Rotation Change"), m_TransformListenerToolScript.m_ListenRotationChange);
        m_TransformListenerToolScript.m_RotationSensitivity = EditorGUILayout.Slider("RotationSensitivity", m_TransformListenerToolScript.m_RotationSensitivity, 0.001f, 10);





    }


}
