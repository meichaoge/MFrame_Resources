using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Unity 内置的Text 组件缓存
/// </summary>
public class UICachePrefab_BuildInText : UICachePrefabBase
{
    protected UnityEngine.UI.Text m_AttachComponent;

    protected override void Awake()
    {
        m_AttachComponent = gameObject.GetComponent<UnityEngine.UI.Text>();


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
