using UnityEngine;
using System.Collections;
using UnityEditor;
namespace MFramework
{

    [CustomEditor(typeof(VRInputField))]
    public class VRInputEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            VRInputField vrInputField = (VRInputField)target;

            //   base.OnInspectorGUI();
            //    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Transition"), true);

            //EditorGUILayout.PropertyField(serializedObject.FindProperty("para"), new GUIContent("提示栏"), vrInputField.para);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ShowText"), new GUIContent("输入栏"), vrInputField.m_ShowText);

            vrInputField.para = EditorGUILayout.ObjectField("输入tip", vrInputField.para, typeof(UnityEngine.UI.Text), true) as UnityEngine.UI.Text;
            vrInputField.m_ShowText = EditorGUILayout.ObjectField("输入栏", vrInputField.m_ShowText, typeof(UnityEngine.UI.Text), true) as UnityEngine.UI.Text;
            vrInputField.lineType = (UnityEngine.UI.InputField.LineType)EditorGUILayout.EnumPopup(new GUIContent("m_LineType"), vrInputField.lineType);

            vrInputField.vRInputRequest.m_InputContenType = (InputContenType)EditorGUILayout.EnumPopup(new GUIContent("InputContentType", "输入类型"), vrInputField.vRInputRequest.m_InputContenType);
            vrInputField.vRInputRequest.m_Limite_Min = EditorGUILayout.IntField(new GUIContent("最小字符数"), vrInputField.vRInputRequest.m_Limite_Min);
            vrInputField.vRInputRequest.m_Limite_Max = EditorGUILayout.IntField(new GUIContent("最大字符数"), vrInputField.vRInputRequest.m_Limite_Max);

            vrInputField.vRInputRequest.m_PasswordType = EditorGUILayout.Toggle(new GUIContent("密码模式"), vrInputField.vRInputRequest.m_PasswordType);

            vrInputField.vRInputRequest.m_KeyBoardOffeset_PC = EditorGUILayout.Vector3Field(new GUIContent("KeyBoard_Offset_Pc"), vrInputField.vRInputRequest.m_KeyBoardOffeset_PC);
            vrInputField.vRInputRequest.m_KeyBoardOffeset_Gear = EditorGUILayout.Vector3Field(new GUIContent("KeyBoard_Offset_Gear"), vrInputField.vRInputRequest.m_KeyBoardOffeset_Gear);
            vrInputField.vRInputRequest.m_KeyBoardAngle = EditorGUILayout.Vector3Field(new GUIContent("KeyBoard_Angle"), vrInputField.vRInputRequest.m_KeyBoardAngle);


            //    vrInputField.ShowPassword = EditorGUILayout.ObjectField("显示密码", vrInputField.ShowPassword, typeof(UIEffect),true)as UIEffect;


            // vrInputField.vRInputRequest.m_KeyBoardOffeset= EditorGUILayout
            //   vrInputField.vRInputRequest.m_KeyBoardOffeset = EditorGUILayout.Vector3Field(new GUIContent("键盘位置偏移"), vrInputField.vRInputRequest.m_KeyBoardOffeset);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("vRInputRequest"), true);
            //   EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputContenType"), true);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ShowText"), true);


        }

    }
}
