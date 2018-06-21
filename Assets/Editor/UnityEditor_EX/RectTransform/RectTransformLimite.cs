//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class RectTransformLimite : Editor {

//    static DrivenRectTransformTracker tracker = new DrivenRectTransformTracker();
//    [MenuItem("Test/Limit")]
//    static void Check()
//    {
//        tracker.Clear();
//        tracker.Add(Selection.activeGameObject, Selection.activeGameObject.GetComponent<RectTransform>(), 
//                DrivenTransformProperties.Pivot | DrivenTransformProperties.Anchors);  //限制当前选择的Rectransform的部分部分属性
//    }
//}
