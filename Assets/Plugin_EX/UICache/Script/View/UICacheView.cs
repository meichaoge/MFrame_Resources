using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI缓存
/// </summary>
[ExecuteInEditMode]
public class UICacheView : MonoBehaviour
{
    protected UICacheItemMark[] m_AllCacheUIItemMarks;

#region Editor编辑器

#if UNITY_EDITOR
    [UnityEditor.MenuItem("CONTEXT/UICacheView/更新带有UICacheItemMark 节点的Tag")]
    static void UpdateMarkTransTag(UnityEditor.MenuCommand cmd)
    {
        if (Application.isPlaying) return;
        UICacheView current = cmd.context as UICacheView;
        if (current != null)
        {
            current.UpdateUIMarkTransTagState();
        }
    }

    [UnityEditor.MenuItem("CONTEXT/UICacheView/更新UI缓存索引")]
    static void UpdateCacheUIItemMark(UnityEditor.MenuCommand cmd)
    {
        if (Application.isPlaying) return;

        UICacheView current = cmd.context as UICacheView;
        if (current != null)
        {
            current.UpdateUICacheRefence();
        }
    }

    [UnityEditor.MenuItem("CONTEXT/UICacheView/显示预览项")]
    static void ShowCacheUIItems(UnityEditor.MenuCommand cmd)
    {
        if (Application.isPlaying) return;

        UICacheView current = cmd.context as UICacheView;
        if (current != null)
        {
            current.OnLoadUIItems();
        }
    }

    [UnityEditor.MenuItem("CONTEXT/UICacheView/移除预览项")]
    static void RemoveCacheUIItems(UnityEditor.MenuCommand cmd)
    {
        if (Application.isPlaying) return;

        UICacheView current = cmd.context as UICacheView;
        if (current != null)
        {
            current.OnUnLoadUIItems();
        }
    }

    [UnityEditor.MenuItem("CONTEXT/UICacheView/清除缓存池")]
    static void ClearCachePools(UnityEditor.MenuCommand cmd)
    {
        if (Application.isPlaying) return;

        UICachePoolManager.ClearCachePool();
    }

    protected virtual void Reset()
    {
        if (Application.isPlaying) return;
        OnLoadUIItems();
    }
    // 当组件改变时候调用
    void OnValidate()
    {
        if (Application.isPlaying) return;
        UpdateUICacheRefence();
        m_AllCacheUIItemMarks = transform.GetComponentsInChildren<UICacheItemMark>(true);
    }


    void UpdateUICacheRefence()
    {
        if (Application.isPlaying) return;

        m_AllCacheUIItemMarks = transform.GetComponentsInChildren<UICacheItemMark>(true);
    }
    /// <summary>
    /// 更新Mark 节点的Tag
    /// </summary>
    void UpdateUIMarkTransTagState()
    {
        foreach (var item in m_AllCacheUIItemMarks)
        {
            item.SetMarkTag();
        }
    }

#endif

    #endregion

    /// <summary>
    ///  加载缓存UI
    /// </summary>
    /// <param name="isReName">是否需要重命名</param>
    public virtual void OnLoadUIItems()
    {
        if (m_AllCacheUIItemMarks == null)
            m_AllCacheUIItemMarks = transform.GetComponentsInChildren<UICacheItemMark>(true);
        foreach (var item in m_AllCacheUIItemMarks)
        {
            item.LoadCacheUIItem(item.m_AdditionalOperate, item.m_AdditialParameter);
        }
    }

    /// <summary>
    /// 清除缓存UI
    /// </summary>
    public virtual void OnUnLoadUIItems()
    {
        if (m_AllCacheUIItemMarks == null) return;
        foreach (var item in m_AllCacheUIItemMarks)
        {
            if(item!=null)
            item.UnLoadItem();
        }
    }

    //private void OnDestroy()
    //{
    //    OnUnLoadUIItems();
    //}


}
