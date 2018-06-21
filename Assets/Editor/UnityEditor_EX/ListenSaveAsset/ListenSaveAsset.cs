using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MFramework.EditorExpand
{
    /// <summary>
    /// 监听保存资源
    /// </summary>
    public class ListenSaveAsset : UnityEditor.AssetModificationProcessor
    {
        static public void OnWillSaveAssets(string[] names)
        {
            foreach (string name in names)
            {
                if (name.EndsWith(".unity"))
                {
                    Scene scene = SceneManager.GetSceneByPath(name);
                    Debug.Log("ListenSaveAsset 监听到你正在保存场景资源 ：" + scene.name);
                }
                else
                {
                    Debug.LogInfor(" name= " + name);
                }
            }
        }
    }
}