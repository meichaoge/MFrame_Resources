using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Image属性设置
/// </summary>
[System.Serializable]
public class ImageSetting
{
    /// <summary>
    /// 当前图片缓存的优先级
    /// </summary>
    public enum ImageCachePriority
    {
        High=-1,   //暂时没有定义
        Normal=0,  //常用的UI 
        MainScene_Low,  //只在主场景中 某些特定的地方出现的UI 退出后销毁
        LoadingScene_Low,  //只在Loading 场景出现
        FightScene_Low,  //只在战斗场景中使用的UI图片 退出后销毁
    }
    [Header("标识当前图片的缓存级别 方便后期卸载部分不常用的资源")]
    public ImageCachePriority m_ImageCachePriority = ImageCachePriority.Normal;

    //是否开启射线检测
    public bool m_RaycastTarget;
    //颜色
    public Color m_Color = Color.white;
    //图片格式
    public Image.Type m_ImageType = Image.Type.Simple;
    [Header("只在 m_ImageType =Image.Type.Simple 时有效 ,控制是否保持长宽比")]
    public bool m_IsPreserverAspec = true;
    [Space(5)]
    [Header("只在 m_ImageType =Image.Type.Filled 时有效")]
    //只在m_ImageType =Image.Type.Filled有效
    public Image.FillMethod m_ImageFillMethod = Image.FillMethod.Horizontal;
    public int m_ImageFillOrigin = 0;  //在不同的m_ImageFillMethod 下使用不同的值

    [Space(10)]
    //图片路径
    public string m_SpritePath;
#if UNITY_EDITOR
    //图片预制体
    public GameObject m_SpritePrefab;
#endif
}

/// <summary>
/// 挂载在每一个需要动态获取缓存的Image元素的父节点上
/// </summary>
public class UIImageCacheItemMark : UICacheItemMark
{
    public ImageSetting m_ImageSetting;

#if UNITY_EDITOR
    [UnityEditor.MenuItem("CONTEXT/UIImageCacheItemMark/选择图片")]
    static void SelectSprite(UnityEditor.MenuCommand cmd)
    {
        if (Application.isPlaying) return;
        UIImageCacheItemMark current = cmd.context as UIImageCacheItemMark;
        if (current != null)
        {
            string filePath = UnityEditor.EditorUtility.OpenFilePanel("选择图片", Application.dataPath + "/Resources/Sprite", "prefab");
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            string assetPath = filePath.Substring(filePath.IndexOf("Assets"));
            current.m_ImageSetting.m_SpritePrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            current.UpdateUICacheRefence();
        }
    }

    protected override void Reset()
    {
        base.Reset();
        if (Application.isPlaying) return;
        m_IsNeedReMarkTag = false;
        gameObject.transform.tag = "Untagged";
    }

    // 当组件改变时候调用
    protected override void OnValidate()
    {
        if (Application.isPlaying) return;
        base.OnValidate();
        UpdateUICacheRefence();
    }

    public virtual void UpdateUICacheRefence()
    {
        if (Application.isPlaying) return;
        m_ImageSetting.m_SpritePath = UICachTool.GetPrefabPath(m_ImageSetting.m_SpritePrefab.gameObject);
    }
#endif

    public override void LoadCacheUIItem(UICacheMarkOperate operateEnum, string parameter)
    {
        base.LoadCacheUIItem(operateEnum, parameter);
        if (m_UICachePrefabScript == null)
            return;

        //   Debug.Log("UIImageCacheItemMark");
        //设置Image的相关属性
        Image image = m_UICachePrefabScript.GetAttachComponent<Image>();
        if (image == null) return;
        if (m_ImageSetting.m_SpritePath != string.Empty)
        {
            //2017/11/27注释掉
       //     image.sprite = ResourceMgr.instance.LoadSprite(m_ImageSetting.m_SpritePath);

        }
        image.raycastTarget = m_ImageSetting.m_RaycastTarget;
        image.color = m_ImageSetting.m_Color;
        image.type = m_ImageSetting.m_ImageType;

        if (image.type == Image.Type.Filled)
        {
            image.fillMethod = m_ImageSetting.m_ImageFillMethod;
            image.fillOrigin = m_ImageSetting.m_ImageFillOrigin;
        }

        if (image.type == Image.Type.Simple)
        {
            image.preserveAspect = m_ImageSetting.m_IsPreserverAspec;
        }
    }
}
