using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Unity 内置的 Image 组件缓存
/// </summary>
public class UICachePrefab_BuildInImage : UICachePrefabBase
{
    protected UnityEngine.UI.Image m_AttachComponent;//{ get; protected set; }

    protected override void Awake()
    {
        m_AttachComponent = gameObject.GetComponent<UnityEngine.UI.Image>();

        MainAttachUI = m_AttachComponent;
        LesserAttachUI = null;
        if (MainAttachUI == null)
        {
            Debug.LogError("Not Contain component : UnityEngine.UI.Image");
        }
    }

    public override void OnRecycleUIItem()
    {
        if (MainAttachUI == null) return;
        m_AttachComponent.sprite = null;
    }


}
