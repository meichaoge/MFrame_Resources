using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 管理UICachePoolManager(未知原因导致必须手动挂载在StartUp上) 
/// </summary>
public class UICachePoolManagerHelper : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(UICachePoolManager.UICachePoolCanvasTag);
        for (int dex = 0; dex < gos.Length; ++dex)
        {
            GameObject.Destroy(gos[dex]);
        }
    }


}
