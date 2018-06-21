using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MFramework;


namespace MFramework.EditorExpand
{

    [CustomPropertyDrawer(typeof(SerelizeVector2_Float))]
    public class SerelizeVector2_Float_PropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);

            var x = property.FindPropertyRelative("X");
            var y = property.FindPropertyRelative("Y");
            float LabelWidth = EditorGUIUtility.labelWidth;  //编辑器每一条数据 字段名的宽度
            var labelRect = new Rect(position.x, position.y, LabelWidth, position.height); //左上角和宽高

            float WidthEachProperty = (position.width - LabelWidth) / 2; //每一个字段分配的宽度

            var xRect = new Rect(position.x + LabelWidth, position.y, WidthEachProperty, position.height);
            var yRect = new Rect(position.x + LabelWidth + WidthEachProperty, position.y, WidthEachProperty, position.height);

            EditorGUIUtility.labelWidth = 12.0f;
            EditorGUI.LabelField(labelRect, label); //绘制对象区域
            EditorGUI.PropertyField(xRect, x); //绘制字段
            EditorGUI.PropertyField(yRect, y);
            EditorGUIUtility.labelWidth = LabelWidth;  //恢复默认设置

        }


    }
}




