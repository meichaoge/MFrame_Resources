using UnityEngine;

/// <summary>
/// TextMesh控件属性设置
/// </summary>
[System.Serializable]
public class TextMeshSetting
{
    //是否开启射线检测
    public bool m_RaycastTarget;
    //字体大小
    public float m_FontSize;
    //字体颜色
    public Color m_Color = Color.white;
    //字体对齐方式
    public TMPro.TextAlignmentOptions m_Alignment = TMPro.TextAlignmentOptions.Center;
    //裁剪模式
    public TMPro.TextOverflowModes m_OverFlowMode = TMPro.TextOverflowModes.Overflow;
    //文本内容
    public string m_Text;
}

/// <summary>
/// 挂载在每一个需要动态获取缓存的Image元素的父节点上
/// </summary>
public class UITextMeshCacheItemMark : UICacheItemMark
{
    public TextMeshSetting m_TextMeshSetting;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        m_ComponentNames = new string[1]
        {
            "TMPro.TextMeshProUGUI",
        };
    }
#endif

    public override void LoadCacheUIItem(UICacheMarkOperate operateEnum, string parameter)
    {
        base.LoadCacheUIItem(operateEnum,parameter);
        if (m_UICachePrefabScript == null)
            return;

        //设置TextMesh的相关属性
        TMPro.TextMeshProUGUI text = m_UICachePrefabScript.GetAttachComponent<TMPro.TextMeshProUGUI>();
        if (text == null) return;
        text.fontSize = m_TextMeshSetting.m_FontSize;
        text.color = m_TextMeshSetting.m_Color;
        text.raycastTarget = m_TextMeshSetting.m_RaycastTarget;
        text.alignment = m_TextMeshSetting.m_Alignment;
        text.overflowMode = m_TextMeshSetting.m_OverFlowMode;

#if UNITY_EDITOR
        if (Application.isPlaying) return;
        text.text = m_TextMeshSetting.m_Text;
#endif
    }
}
