using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MFramework
{
    [CustomEditor(typeof(Simple_Point_SubMesh))]

    public class Simple_Point_SubMesh_Editor : Editor
    {
        Simple_Point_SubMesh m_SimplePoint
        {
            get
            {
                return target as Simple_Point_SubMesh;
            }
        }


        private void OnSceneGUI()
        {
            for (int dex = 0; dex < m_SimplePoint.m_AllSubMeshPoints.Count; dex++)
            {
                EditorGUI.BeginChangeCheck();
                // Vector3 point = Handles.PositionHandle(m_QuadMesh.AllMeshPoints[dex], Quaternion.identity);
                Vector3 point = Handles.PositionHandle(m_SimplePoint.transform.TransformPoint(m_SimplePoint.m_AllSubMeshPoints[dex]), Quaternion.identity);
                Handles.Label(point, "Point " + dex);
                if (EditorGUI.EndChangeCheck())
                {
                    //serializedObject.FindProperty("points").GetArrayElementAtIndex(dex).vector3Value = point;
                    //serializedObject.ApplyModifiedProperties();
                    ///   m_QuadMesh.Flush();
                }
            }//for
        }

    }
}