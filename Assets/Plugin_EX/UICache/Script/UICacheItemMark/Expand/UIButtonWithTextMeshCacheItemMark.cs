using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 挂载在每一个需要动态获取缓存的拥有TextMesh的Button元素的父节点上
/// </summary>
public class UIButtonWithTextMeshCacheItemMark : UIButtonWithoutTextCacheItemMark
{
    public TextMeshSetting m_TextMeshSetting;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        m_ComponentNames = new string[2]
        {
            "Button",
            "TMPro.TextMeshProUGUI",
        };
    }
#endif

    public override void LoadCacheUIItem(UICacheMarkOperate operateEnum, string parameter)
    {

        base.LoadCacheUIItem(operateEnum,parameter);
        if (m_UICachePrefabScript == null)
            return;

    //    Debug.Log("UIButtonWithTextMeshCacheItemMark");

        //设置TextMesh的相关属性
        TMPro.TextMeshProUGUI text = m_UICachePrefabScript.GetAttachComponent<TMPro.TextMeshProUGUI>();
        if (text == null)
            return;
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

    public override T GetAttachComponent<T>()
    {
        System.Type t = typeof(T);
        if (t == typeof(Image))
        {
            return GetMainAttachUI_ImageComponent() as T;
        }
        else if (t == typeof(TMPro.TextMeshProUGUI))
        {
            return GetLesserAttachUIComponent<T>();
        }
        return base.GetAttachComponent<T>();
    }
}
