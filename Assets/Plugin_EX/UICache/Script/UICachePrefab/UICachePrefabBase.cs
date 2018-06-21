using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 缓存池中的预制体
/// </summary>
[ExecuteInEditMode]
public abstract class UICachePrefabBase : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("指向挂载当前脚本的预制体本身 在Editor下自动获取和更新m_AssetPath和m_RelativeCachePath 路径 ")]
    public GameObject m_CachePrefab;
#endif
    [Space(30)]
    public string m_AssetPath;
    [SerializeField]
    private string m_RelativeCachePath;
    public string RelativeCachePath { get { return m_RelativeCachePath; } }

    protected Component MainAttachUI { set; get; } //主要的挂在组件
    protected Component LesserAttachUI { set; get; } //次要的挂在组件

    [Header("预制体的最初始名称 不要修改它!!!!!")]
    public string m_CachePrefabName = "CachePrefabName";

    public string CachePrefabName { get { return m_CachePrefabName; } }  //当前prefab 的原始名称



    protected virtual void Awake() { }

#if UNITY_EDITOR

    [UnityEditor.MenuItem("CONTEXT/UICachePrefabBase/更新地址信息")]
    static void ShowCacheUIItems(UnityEditor.MenuCommand cmd)
    {
        if (Application.isPlaying) return;
        UICachePrefabBase current = cmd.context as UICachePrefabBase;

        if (current != null)
        {
            current.UpdateRelativeCachePath();
        }
    }

    protected virtual void Reset()
    {
        if (Application.isPlaying) return;
        m_CachePrefabName = gameObject.name;
        if (string.IsNullOrEmpty(UnityEditor.AssetDatabase.GetAssetPath(gameObject)))
        {
            //Debug.LogError("当前脚本必须挂载在Projct 视图中资源上 " + this.GetType());
            return;
        } //在Hierachy 视图中的挂载这个脚本会崩掉

        UICachePrefabBase[] scripts = GetComponents<UICachePrefabBase>();
        if (scripts.Length > 1)
        {
            Debug.LogError("当前Prefab 已经有一个 UICachePrefabBase 组件");
            //   GameObject.DestroyImmediate(scripts[1]);
            return;
        }
        UpdateRelativeCachePath();
    }

    protected virtual void OnValidate()
    {
        if (Application.isPlaying) return;
        if (m_CachePrefab == null) return;
        m_CachePrefabName = gameObject.name;

        m_AssetPath = UnityEditor.AssetDatabase.GetAssetPath(m_CachePrefab);
        if (string.IsNullOrEmpty(m_AssetPath))
        {
            // Debug.LogError("当前脚本必须挂载在Projct 视图中资源上 " + this.GetType());
            return;
        } //在Hierachy 视图中的挂载这个脚本会崩掉

        m_RelativeCachePath = UICachTool.GetPrefabRelativePath(m_AssetPath, "Resources/");

    }

    /// <summary>
    /// 更新缓存UI的信息
    /// </summary>
    public void UpdateRelativeCachePath()
    {
        if (Application.isPlaying) return;
        m_CachePrefabName = gameObject.name;
        m_AssetPath = UnityEditor.AssetDatabase.GetAssetPath(gameObject);
        if (string.IsNullOrEmpty(m_AssetPath))
        {
            Debug.LogError("当前脚本必须挂载在Projct 视图中资源上 " + this.GetType());
            return;
        } //在Hierachy 视图中的挂载这个脚本会崩掉
        m_RelativeCachePath = UICachTool.GetPrefabRelativePath(m_AssetPath, "Resources/");
    }

#endif

    /// <summary>
    ///  加载一个缓存UI后的操作
    /// </summary>
    /// <param name="operateEnum">操作类型</param>
    /// <param name="paramet">参数</param>
    public virtual void OnGetUIItemInitial(UICacheMarkOperate operateEnum, string paramet)
    {

    }

    public abstract void OnRecycleUIItem();




    /// <summary>
    /// 获得主Component    (当前节点的继承UICachePrefabBase 的类 )
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetMainAttachUIComponent<T>() where T : Component
    {
        if (MainAttachUI != null)
            return MainAttachUI as T;

        Debug.LogError("GetMainAttachUIComponent Fail, MainAttachUI is Null");
        return null;
    }
    /// <summary>
    /// 获得主Component    (用于Button 组件的Image 获取 )
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public virtual UnityEngine.UI.Image GetMainAttachUI_ImageComponent()
    {

        //   Debug.LogError("GetMainAttachUI_2Component Fail, MainAttachUI is Null");
        return null;
    }
    /// <summary>
    /// 获的二级Compoent (当前节点的子节点上的继承UICachePrefabBase 的类 )
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetLesserAttachUIComponent<T>() where T : Component
    {
        if (LesserAttachUI != null)
            return LesserAttachUI as T;

        Debug.LogError("GetLesserAttachUIComponent Fail, LesserAttachUI is Null");
        return null;
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
