using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MFramework.EditorExpand
{
    /// <summary>
    /// ����EnumFlagsAttribute �ı���Ч��
    /// </summary>
    [CustomPropertyDrawer(typeof(Enum_FlagsAttribute))]
    public class Enum_FlagsAttributeDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // base.OnGUI(position, property, label);
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
        //    Debug.Log("CurSelect EnumFlag Value=" + property.intValue);
        }


    }
}
