using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace MFramework.EditorExpand
{
    /// <summary>
    /// 监听Prefab 保存的事件
    /// </summary>
    public class ListenerPrefabApply
    {
        [InitializeOnLoadMethod]
        static void StartInitializeOnLoadMethod()
        {
            PrefabUtility.prefabInstanceUpdated = delegate (GameObject instance)
            {
            //prefab保存的路径
            Debug.Log(AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(instance)));
            };
        }
    }
}