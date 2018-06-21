using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MFramework {
    [CustomEditor(typeof(Simple_Cube_SubMesh))]
    public class Simple_Cube_SubMesh_Editor : Editor
    {
        Simple_Cube_SubMesh m_CubeMesh
        {
            get { return target as Simple_Cube_SubMesh; }
        }

        private void OnSceneGUI()
        {
            // Debug.Log("m_QuadMesh " + m_CubeMesh.m_AllSubMeshPoints.Count);
            for (int dex = 0; dex < m_CubeMesh.m_AllSubMeshPoints.Count; dex++)
            {
                EditorGUI.BeginChangeCheck();
                //   Vector3 point = Handles.PositionHandle(m_Quad3DMesh.AllMeshPoints[dex], Quaternion.identity);
                Vector3 point = Handles.PositionHandle(m_CubeMesh.transform.TransformPoint(m_CubeMesh.m_AllSubMeshPoints[dex]), Quaternion.identity);

                Handles.Label(point, "Point " + dex);
                if (EditorGUI.EndChangeCheck())
                {
                    //serializedObject.FindProperty("points").GetArrayElementAtIndex(dex).vector3Value = point;
                    //serializedObject.ApplyModifiedProperties();
                    ///   m_QuadMesh.Flush();
                }
            }//for

            //for (int dex = 0; dex < m_Quad3DMesh.AllKeyPathpoints.Count; dex++)
            //{
            //    EditorGUI.BeginChangeCheck();
            //    Vector3 point = Handles.PositionHandle(m_Quad3DMesh.AllKeyPathpoints[dex].Point_Center, Quaternion.identity);
            //    Handles.Label(point, "Key " + dex);
            //    if (EditorGUI.EndChangeCheck())
            //    {
            //        //serializedObject.FindProperty("points").GetArrayElementAtIndex(dex).vector3Value = point;
            //        //serializedObject.ApplyModifiedProperties();
            //        ///   m_QuadMesh.Flush();
            //    }
            //}

        }
    }
}
