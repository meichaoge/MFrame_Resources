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
            float LabelWidth = EditorGUIUtility.labelWidth;  //�༭��ÿһ������ �ֶ����Ŀ��
            var labelRect = new Rect(position.x, position.y, LabelWidth, position.height); //���ϽǺͿ��

            float WidthEachProperty = (position.width - LabelWidth) / 2; //ÿһ���ֶη���Ŀ��

            var xRect = new Rect(position.x + LabelWidth, position.y, WidthEachProperty, position.height);
            var yRect = new Rect(position.x + LabelWidth + WidthEachProperty, position.y, WidthEachProperty, position.height);

            EditorGUIUtility.labelWidth = 12.0f;
            EditorGUI.LabelField(labelRect, label); //���ƶ�������
            EditorGUI.PropertyField(xRect, x); //�����ֶ�
            EditorGUI.PropertyField(yRect, y);
            EditorGUIUtility.labelWidth = LabelWidth;  //�ָ�Ĭ������

        }


    }
}




