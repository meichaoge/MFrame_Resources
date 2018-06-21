using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity 常用的UI元素缓存池
/// </summary>
public class UICachePoolManager
{
    private static UICachePoolManager _Instance;
    public static UICachePoolManager Instance
    {
        get
        {
            if (_Instance == null) _Instance = new UICachePoolManager();
            return _Instance;
        }
    }

    public const string UICachePoolCanvasName = "UICachePoolCanvas";
    public const string UICachePoolCanvasTag = "UICachePool"; //缓存池Canvas 标识

    public static Dictionary<string, Stack<RectTransform>> AllCacheUIItemsDic = new Dictionary<string, Stack<RectTransform>>();  //根据名字缓存的所有UI元素项
    private static Canvas m_PoolCanvas; //挂载所有缓存的UI项  不销毁
    public static Canvas PoolCanvas
    {
        get
        {
            if (m_PoolCanvas == null)
            {
                m_PoolCanvas = new GameObject(UICachePoolCanvasName).AddComponent<Canvas>();
                //  m_PoolCanvas.gameObject.hideFlags = HideFlags.DontSave;// | HideFlags.NotEditable;
                m_PoolCanvas.transform.gameObject.layer = LayerMask.NameToLayer("UnVisualLayer");  //不可见层
                m_PoolCanvas.transform.tag = UICachePoolCanvasTag;
                m_PoolCanvas.transform.localScale = Vector3.zero;
            }
            return m_PoolCanvas;
        }
    }

    private UICachePoolManager()
    {
        if (Application.isPlaying == false) return;
        GameObject.DontDestroyOnLoad(PoolCanvas);
    }

    #region 获取一个缓存UI

    /// <summary>
    /// 根据路径获取一个缓存的UI项
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public RectTransform GetUIItemByPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("GetUIItemByPath Fail,Path Is Null Or Empty");
            return null;
        }

        string prefabName = System.IO.Path.GetFileName(path);  //获取预制体名称
        if (AllCacheUIItemsDic.ContainsKey(prefabName) == false)
        {
            //  Debug.LogError("GetUIItem  Fail");
            return CreateObjectByRelativePath(path);
        }

        if (AllCacheUIItemsDic[prefabName].Count > 0)
        {
            RectTransform rect = AllCacheUIItemsDic[prefabName].Pop();
            if (rect != null)
                return rect;
        }

        return CreateObjectByRelativePath(path);
    }

    private RectTransform CreateObjectByRelativePath(string path)
    {
        GameObject go = null;   
        //2017/11/27注释掉
       // GameObject go = ResourceMgr.instance.Instantiate(path);
        go.name = System.IO.Path.GetFileName(path);
        return go.transform as RectTransform;
    }
    #endregion


    #region 回收UI

    /// <summary>
    /// 回收UI项
    /// </summary>
    /// <param name="uiitem"></param>
    public void RecycleUIItem(UICachePrefabBase uiitem)
    {
        if (uiitem == null)
        {
            Debug.LogError("RecycleUIItem Fail,is Null");
            return;
        }

        uiitem.OnRecycleUIItem();
        uiitem.gameObject.transform.SetParent(PoolCanvas.transform, false);

        if (AllCacheUIItemsDic.ContainsKey(uiitem.CachePrefabName))
        {
            AllCacheUIItemsDic[uiitem.CachePrefabName].Push(uiitem.transform as RectTransform);
            return;
        }

        Stack<RectTransform> pool = new Stack<RectTransform>();
        pool.Push(uiitem.transform as RectTransform);
        AllCacheUIItemsDic.Add(uiitem.CachePrefabName, pool);
    }
    #endregion

    /// <summary>
    /// 清除缓存池
    /// </summary>
    public static void ClearCachePool()
    {
        foreach (var cache in AllCacheUIItemsDic.Values)
        {
            if (cache != null)
            {
                foreach (var item in cache)
                {
                    if (item != null)
                        GameObject.DestroyImmediate(item.gameObject);
                }
                cache.Clear();
            }
        }

        //if (PoolCanvas != null)
        //{
        //    if (Application.isPlaying)
        //        GameObject.Destroy(PoolCanvas.gameObject);
        //    else
        //        GameObject.DestroyImmediate(PoolCanvas.gameObject);
        //}
    }

    public T TransferUI<T>(GameObject obj) where T : UnityEngine.EventSystems.UIBehaviour
    {
        return obj as T;
    }



}
