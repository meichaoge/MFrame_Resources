using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;


namespace MFramework.EditorExpand
{

    //**********当Transform 的 m_IsLockAndListen=true 时候锁定当前Transform
    [CustomEditor(typeof(Transform))]
    public class TransformListenerEditor : Editor
    {
        [InitializeOnLoadMethod]
        static void IInitializeOnLoadMethod()
        {
            TransformListenerEditor.onPostion = delegate (Transform transform)
            {

                Debug.Log(string.Format("transform = {0}  positon = {1}", transform.name, transform.localPosition));
            };

            TransformListenerEditor.onRotation = delegate (Transform transform)
            {

                Debug.Log(string.Format("transform = {0}   rotation = {1}", transform.name, transform.localRotation.eulerAngles));
            };

            TransformListenerEditor.onScale = delegate (Transform transform)
            {

                Debug.Log(string.Format("transform = {0}   scale = {1}", transform.name, transform.localScale));
            };
        }

        public delegate void Change(Transform transform);
        static public Change onPostion;
        static public Change onRotation;
        static public Change onScale;


        private Editor editor;
        private Transform transform;
        private Vector3 startPostion = Vector3.zero;
        private Vector3 startRotation = Vector3.zero;
        private Vector3 startScale = Vector3.zero;
        private bool m_IsLockAndListen = false;

        void OnEnable()
        {
            transform = target as Transform;
            editor = Editor.CreateEditor(target, Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.TransformInspector", true));
            startPostion = transform.localPosition;
            startRotation = transform.localRotation.eulerAngles;
            startScale = transform.localScale;
        }

        public override void OnInspectorGUI()
        {
            ////2017/11/11临时注释掉
            //m_IsLockAndListen = GUILayout.Toggle(m_IsLockAndListen, "IsLock ,Show Debug When Change");
            //if (m_IsLockAndListen == false)
            //{
            //    editor.OnInspectorGUI();
            //    return;
            //}
            editor.OnInspectorGUI();


            ////2017/11/11临时注释掉
            //if (GUI.changed)
            //{
            //    if (startPostion != transform.localPosition)
            //        if (onPostion != null) onPostion(transform);

            //    if (startRotation != transform.localRotation.eulerAngles)
            //        if (onRotation != null) onRotation(transform);

            //    if (startScale != transform.localScale)
            //        if (onScale != null) onScale(transform);

            //    startPostion = transform.localPosition;
            //    startRotation = transform.localRotation.eulerAngles;
            //    startScale = transform.localScale;
            //}

        }

    }
}