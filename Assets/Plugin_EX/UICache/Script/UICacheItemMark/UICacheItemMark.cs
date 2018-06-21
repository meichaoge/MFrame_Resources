using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 挂载在每一个需要动态获取缓存的UI元素的父节点上  获取的缓存UI会自动对其在这个节点下
/// </summary>
[DisallowMultipleComponent]
public class UICacheItemMark : MonoBehaviour
{

    #region 编辑器Editor

#if UNITY_EDITOR
    [Header("当前标签位置需要显示的UI缓存Prefab Editor 下自动更新 m_LoadPrefabPath ")]
    public GameObject m_ReferenceCacheUIPrefab;
#endif
    [Space(5)]
    [Header("显示时加载缓存路径下UI的路径")]
    public string m_LoadPrefabPath;

#if UNITY_EDITOR

    [Space(5)]
    [Header("标识是否需要在代码中获取这个Mark Transform  如果不需要则为false ")]
    [SerializeField]
    protected bool m_IsNeedReMarkTag = true;

    [Space(5)]
    [Header("设置需要导出给外部访问的组件的名称列表")]
    public string[] m_ComponentNames;
#endif

    [Space(5)]
    [Header("标识需要获取时候额外的操作(比如控制Button 下面文本的样式) ")]
    public UICacheMarkOperate m_AdditionalOperate = UICacheMarkOperate.None;
    public string m_AdditialParameter = "0";
    #endregion

    public RectTransform  rectTransform { get { return transform as RectTransform; } }

    protected bool m_IsLoaded = false;
    protected UICachePrefabBase m_UICachePrefabScript;//获取的UI缓存Item 上的UICachePrefabBase 脚本组件
    protected RectTransform m_LoadUIItem;

    /// <summary>
    /// 加载UI缓存时触发该事件，用于UI控件的重新绑定
    /// </summary>
    public System.Action onLoad { set; get; }

    #region 编辑器Editor

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        if (Application.isPlaying) return;
        gameObject.transform.tag = "UICacheItemMark";
    }

    public void SetMarkTag()
    {
        //Debug.Log("SetMarkTag  " + m_IsNeedReMarkTag + "  " + gameObject.name);
        if (m_IsNeedReMarkTag)
            gameObject.transform.tag = "UICacheItemMark";
        else
            gameObject.transform.tag = "Untagged";
    }

    // 当组件改变时候调用
    protected virtual void OnValidate()
    {
        if (Application.isPlaying) return;
        SetMarkTag();
        if (m_ReferenceCacheUIPrefab != null)
        {
            string path = UnityEditor.AssetDatabase.GetAssetPath(m_ReferenceCacheUIPrefab);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("请关联Project 中 Resource 路径下的缓存UI预制体  ::" + gameObject.name);
                return;
            }
            m_LoadPrefabPath = UICachTool.GetPrefabRelativePath(path, "Resources/");
        }//自动更新 m_LoadPrefabPath 路径
    }


    [UnityEditor.MenuItem("CONTEXT/UICacheItemMark/显示预览")]
    static void ShowCacheUIItems(UnityEditor.MenuCommand cmd)
    {
        //2017/11/27注释掉
    //    if (Application.isPlaying&&Editor_ApplicationPause.IsUICacheEnableWhenPlaying==false) return;
        UICacheItemMark current = cmd.context as UICacheItemMark;
        if (current != null)
        {
            current.LoadCacheUIItem(current.m_AdditionalOperate, current.m_AdditialParameter);
        }
    }

    [UnityEditor.MenuItem("CONTEXT/UICacheItemMark/移除预览")]
    static void RemoveCacheUIItems(UnityEditor.MenuCommand cmd)
    {  //2017/11/27注释掉
     //   if (Application.isPlaying && Editor_ApplicationPause.IsUICacheEnableWhenPlaying == false) return;
        UICacheItemMark current = cmd.context as UICacheItemMark;
        if (current != null)
        {
            current.UnLoadItem();
        }
    }


#endif

    #endregion


    public virtual void LoadCacheUIItem(UICacheMarkOperate operateEnum, string parameter)
    {
        if (m_IsLoaded)
            UnLoadItem();

        m_IsLoaded = true;
        //    Debug.Log("UICachePoolManager.Instance " + (UICachePoolManager.Instance == null));

        m_LoadUIItem = UICachePoolManager.Instance.GetUIItemByPath(m_LoadPrefabPath);
        m_UICachePrefabScript = m_LoadUIItem.GetComponent<UICachePrefabBase>();
      
        //设置自动对其到父节点下
        m_LoadUIItem.SetParent(transform, false);
        m_LoadUIItem.SetAsFirstSibling();
        m_LoadUIItem.localScale = Vector3.one;
        if (m_LoadUIItem.gameObject.activeSelf == false)
            m_LoadUIItem.gameObject.SetActive(true);
     //   Debug.Log("UICacheItemMark");

        m_UICachePrefabScript.OnGetUIItemInitial(operateEnum, parameter);  //对获取的UI缓存的额额哎处理

        m_LoadUIItem.localScale = Vector3.one;
        m_LoadUIItem.anchoredPosition = Vector2.zero;
        m_LoadUIItem.anchorMax = Vector2.one;
        m_LoadUIItem.anchorMin = Vector2.zero;
        m_LoadUIItem.sizeDelta = Vector2.zero;
        //设置位于父节点下的最前面，防止挡住其他节点
        m_LoadUIItem.SetAsFirstSibling();

        //在编辑模式下设置不可被保存，也就是apply后不会再被保存在prefab上
#if UNITY_EDITOR
        foreach (Transform t in m_LoadUIItem.GetComponentsInChildren<Transform>())
        {
            t.gameObject.hideFlags = HideFlags.NotEditable | HideFlags.DontSave;
        }
#endif
        if (onLoad != null)
            onLoad();
    }



    /// <summary>
    /// 卸载UI缓存对象
    /// </summary>
    public virtual void UnLoadItem()
    {
        if (m_IsLoaded == false)
            return;  //防止重复卸载

        if (m_LoadUIItem != null)
        {
            UICachePoolManager.Instance.RecycleUIItem(m_UICachePrefabScript);
        }

        m_IsLoaded = false;
    }

    /// <summary>
    /// 获得主Component    (当前节点 上继承UICachePrefabBase 的类 )
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetMainAttachUIComponent<T>() where T : Component
    {
        if (m_UICachePrefabScript == null)
        {
            Debug.LogError("GetMainAttachUIComponent Fail, m_UICachePrefabScript is Null");
            return null;
        }
        return m_UICachePrefabScript.GetMainAttachUIComponent<T>();
    }

    /// <summary>
    /// 获得类似于Button 上的Image 组件
    /// </summary>
    /// <returns></returns>
    public Image GetMainAttachUI_ImageComponent()
    {
        if (m_UICachePrefabScript == null)
        {
            Debug.LogError("GetMainAttachUI_ImageComponent Fail, m_UICachePrefabScript is Null");
            return null;
        }
        return m_UICachePrefabScript.GetMainAttachUI_ImageComponent();

    }

    /// <summary>
    /// 获的二级Compoent (当前节点的子节点上继承UICachePrefabBase 的类 )
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetLesserAttachUIComponent<T>() where T : Component
    {
        if (m_UICachePrefabScript == null)
        {
            Debug.LogError("GetLesserAttachUIComponent Fail, m_UICachePrefabScript is Null");
            return null;
        }
        return m_UICachePrefabScript.GetLesserAttachUIComponent<T>();
    }

    /// <summary>
    /// 获取绑定的组件，默认获取主组件，如果需要获取其他组件需要子类重写
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public virtual T GetAttachComponent<T>() where T : Component
    {
        return GetMainAttachUIComponent<T>();
    }


}
