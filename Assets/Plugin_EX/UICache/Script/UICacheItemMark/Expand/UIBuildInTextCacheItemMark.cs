using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Unity 默认的Text  控件属性设置
/// </summary>
[System.Serializable]
public class BuildInTextSetting
{
    //是否开启射线检测
    public bool m_RaycastTarget;
    //字体大小
    public int m_FontSize;
    //字体颜色
    public Color m_Color = Color.white;
    //字体对齐方式
    public TextAnchor m_Alignment = TextAnchor.MiddleCenter;
    //文本内容
    public string m_Text;
}

/// <summary>
/// Unity 内置的Text 组件
/// </summary>
public class UIBuildInTextCacheItemMark : UICacheItemMark
{
    public BuildInTextSetting m_BuildInTextSetting;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        m_ComponentNames = new string[1]
        {
            "Text",
        };
    }
#endif

    public override void LoadCacheUIItem(UICacheMarkOperate operateEnum, string parameter)
    {
        base.LoadCacheUIItem(operateEnum,parameter);
        if (m_UICachePrefabScript == null)
            return;

        //设置TextMesh的相关属性
        UnityEngine.UI.Text text = m_UICachePrefabScript.GetAttachComponent<UnityEngine.UI.Text>();
        if (text == null) return;
        text.fontSize = m_BuildInTextSetting.m_FontSize;
        text.color = m_BuildInTextSetting.m_Color;
        text.raycastTarget = m_BuildInTextSetting.m_RaycastTarget;
        text.alignment = m_BuildInTextSetting.m_Alignment;
        text.text = m_BuildInTextSetting.m_Text;
    }


}
