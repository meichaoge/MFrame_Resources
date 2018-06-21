//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;


///// <summary>
///// ****创建不影响原有布局的Layout  使用base.OnInspectorGUI（） 会调用Editor 的OnInspectorGUI方法而破坏原有的布局 
///// </summary>
//[CustomEditor(typeof(RectTransform))]
//public class RectTransformInspectorEx : DecoratorEditor
//{
//    public RectTransformInspectorEx() : base("RectTransformEditor") { }
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        if (GUILayout.Button("Adding this button"))
//        {
//            Debug.Log("Adding this button");
//        }
//    }
//}



