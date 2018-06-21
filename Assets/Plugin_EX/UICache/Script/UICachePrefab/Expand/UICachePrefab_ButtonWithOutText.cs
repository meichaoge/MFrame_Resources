using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 不带Text 组件的Button
/// </summary>
public class UICachePrefab_ButtonWithOutText : UICachePrefab_BuildInButton
{

    protected override void Awake()
    {
        m_AttachComponent = gameObject.GetComponent<UnityEngine.UI.Button>();
        m_AttachMainImageComponent = gameObject.GetComponent<UnityEngine.UI.Image>();
        MainAttachUI = m_AttachComponent;
        LesserAttachUI = m_AttachComponent_Lesser = null;
        if (MainAttachUI == null)
        {
            Debug.LogError("Not Contain component : UnityEngine.UI.Button");
        }
    }

    public override void OnRecycleUIItem()
    {
        if (m_AttachComponent == null) return;
        m_AttachComponent.onClick.RemoveAllListeners();
        m_AttachComponent.interactable = true;
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
