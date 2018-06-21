using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MFramework;


namespace MFramework.EditorExpand
{

    [CustomPropertyDrawer(typeof(SerelizeVector3_Float))]
    public class SerelizeVector3_Float_PropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);

            var x = property.FindPropertyRelative("X");
            var y = property.FindPropertyRelative("Y");
            var z = property.FindPropertyRelative("Z");
            float LabelWidth = EditorGUIUtility.labelWidth;  //编辑器每一条数据 字段名的宽度
            var labelRect = new Rect(position.x, position.y, LabelWidth, position.height); //左上角和宽高

            float WidthEachProperty = (position.width - LabelWidth) / 3 ; //每一个字段分配的宽度

            var xRect = new Rect(position.x + LabelWidth, position.y, WidthEachProperty, position.height);
            var yRect = new Rect(position.x + LabelWidth + WidthEachProperty, position.y, WidthEachProperty, position.height);
            var zRect = new Rect(position.x + LabelWidth+ WidthEachProperty *2, position.y, WidthEachProperty*2, position.height);

            EditorGUIUtility.labelWidth = 12.0f;
            EditorGUI.LabelField(labelRect, label); //绘制对象区域
            EditorGUI.PropertyField(xRect, x); //绘制字段
            EditorGUI.PropertyField(yRect, y);
            EditorGUI.PropertyField(zRect, z);
            EditorGUIUtility.labelWidth = LabelWidth;  //恢复默认设置

        }


    }
}
