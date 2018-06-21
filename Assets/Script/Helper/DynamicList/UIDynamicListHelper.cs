﻿using MFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 用于辅助需要根据需要显示或者生成某一个节点下的子项的需求
/// </summary>
public class UIDynamicListHelper : Singleton_Static<UIDynamicListHelper>
{
    protected override void InitialSingleton()  { }



    /// <summary>
    ///  获得匹配的元素个数
    /// </summary>
    /// <param name="listPrarent"></param>
    /// <param name="expectCount"></param>
    /// <param name="prefabPath">预制体的路径</param>
    /// <param name="onCreateItem">当创建一个UI项时候的回调</param>
    public void GetMatchUIItems(Transform listPrarent, int expectCount, string prefabPath, 
                        System.Action<GameObject> onCreateItem)
    {
        if(listPrarent==null)
        {
            Debug.LogError("GetMatchUIItems Fail,Parent Should Not Be Null");
            return;
        }

        int childCount = listPrarent.childCount;
        if (childCount > expectCount)
        {
            #region  已经生成的UI项过多 则隐藏后面的项
            for (int dex = expectCount; dex < childCount; ++dex)
            {
                listPrarent.GetChild(dex).gameObject.SetActive(false);
            }//隐藏多余的项
            #endregion
        }
        else
        {
            #region  需要生成一些UI项
            for (int dex = 0; dex < childCount; ++dex)
            {
                listPrarent.GetChild(dex).gameObject.SetActive(true);
            }//使得前面N项可见

            //2017.11.7 注释  ResourceMgr 不存在
            //GameObject prefab = ResourceMgr.instance.Load<GameObject>(prefabPath);  //获取Item项
            //for (int dex = 0; dex < expectCount - childCount; ++dex)
            //{
            //    GameObject go = ResourceMgr.instance.Instantiate(prefabPath, listPrarent, true);
            //    if (go == null)
            //    {
            //        Debug.LogError("GetMatchUIItems Fail,Prefab Not Exit " + prefabPath);
            //        return;
            //    }
            //    go.transform.localScale = Vector3.one;
            //    if (onCreateItem != null)
            //        onCreateItem(go);
            //}
            #endregion
        }
    }

    
}
