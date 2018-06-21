using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// TextMesh 插件的Text 组件
/// </summary>
public class UICachePrefab_TextMeshProUGUI : UICachePrefabBase
{

    protected TMPro.TextMeshProUGUI m_AttachComponent;

    protected override void Awake()
    {
        m_AttachComponent = gameObject.GetComponent<TMPro.TextMeshProUGUI>();

        MainAttachUI = m_AttachComponent;
        LesserAttachUI = null;
        if (MainAttachUI == null)
        {
            Debug.LogError("Not Contain component : UnityEngine.UI.Text");
        }
    }

    public override void OnRecycleUIItem()
    {
        //base.OnRecycleUIItem();
        if (m_AttachComponent == null) return;
        m_AttachComponent.text = "";
        m_AttachComponent.color = Color.white;
    }



}
