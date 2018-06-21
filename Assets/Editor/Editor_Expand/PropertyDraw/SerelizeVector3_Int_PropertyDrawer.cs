using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace MFramework.EditorExpand
{
    [CustomPropertyDrawer(typeof(SerelizeVector3_Int))]
    public class SerelizeVector3_Int_PropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);

            var x = property.FindPropertyRelative("X");
            var y = property.FindPropertyRelative("Y");
            var z = property.FindPropertyRelative("Z");
            float LabelWidth = EditorGUIUtility.labelWidth;  //�༭��ÿһ������ �ֶ����Ŀ���
            var labelRect = new Rect(position.x, position.y, LabelWidth, position.height); //���ϽǺͿ���

            float WidthEachProperty = (position.width - LabelWidth) / 3; //ÿһ���ֶη���Ŀ���

            var xRect = new Rect(position.x + LabelWidth, position.y, WidthEachProperty, position.height);
            var yRect = new Rect(position.x + LabelWidth + WidthEachProperty, position.y, WidthEachProperty, position.height);
            var zRect = new Rect(position.x + LabelWidth + WidthEachProperty * 2, position.y, WidthEachProperty * 2, position.height);

            EditorGUIUtility.labelWidth = 12.0f;
            EditorGUI.LabelField(labelRect, label); //���ƶ�������
            EditorGUI.PropertyField(xRect, x); //�����ֶ�
            EditorGUI.PropertyField(yRect, y);
            EditorGUI.PropertyField(zRect, z);
            EditorGUIUtility.labelWidth = LabelWidth; //�ָ�Ĭ������

        }
    }
}