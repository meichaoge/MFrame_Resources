using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MFramework.EditorExpand
{
    /// <summary>
    /// 菜单打开资源
    /// </summary>
    public class OpenAsset_Editor : Editor
    {

        [MenuItem("Assets/Auto Open")]
        static void Run()
        {
            var obj = Selection.activeObject;
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(obj.GetInstanceID())))
                {
                    AssetDatabase.OpenAsset(obj);
                }
            }
        }

        [MenuItem("Assets/Auto Open2")]
        static void Run1()
        {
            var obj = Selection.activeObject;
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(obj.GetInstanceID())))
                {
                    EditorApplication.ExecuteMenuItem("Assets/Open");
                }
            }
        }
    }
}