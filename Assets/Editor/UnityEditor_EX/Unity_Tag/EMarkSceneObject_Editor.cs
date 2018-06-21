using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace MFramework.EditorExpand
{
    [CustomEditor(typeof(EMarkSceneObject))]
    public class EMarkSceneObject_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EMarkSceneObject mark = target as EMarkSceneObject;
            EditorGUI.BeginChangeCheck();
            mark.marktag = EditorGUILayout.TagField("Mark Tag For Objects", mark.marktag);
            if(EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(mark);  //标记当前物体已经改变
            }

        }
    }
}