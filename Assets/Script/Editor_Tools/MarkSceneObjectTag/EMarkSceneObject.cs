#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.EditorExpand
{
    /// <summary>
    /// 标记场景中的指定Tag 的object
    /// </summary>
    public class EMarkSceneObject : MonoBehaviour
    {

        [HideInInspector]
        public string marktag = "Untagged";
        void OnDrawGizmos()
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag(marktag))
            {
                Color c1 =  GUI.color;
                GUI.color = Color.red;
                UnityEditor.Handles.Label(go.transform.position, marktag);
                GUI.color = c1;
            }
        }
    }
}
#endif