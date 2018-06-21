using System;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 内置的Button
/// </summary>
public class UICachePrefab_BuildInButton : UICachePrefabBase
{
    protected UnityEngine.UI.Button m_AttachComponent;//{ get; protected set; }
    protected UnityEngine.UI.Text m_AttachComponent_Lesser;//{ get; protected set; }
    protected UnityEngine.UI.Image m_AttachMainImageComponent;  //Button 上的Image 组件

    protected override  void Awake()
    {
        m_AttachComponent = gameObject.GetComponent<UnityEngine.UI.Button>();
        m_AttachComponent_Lesser= transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        m_AttachMainImageComponent = gameObject.GetComponent<UnityEngine.UI.Image>();



        MainAttachUI = m_AttachComponent;
        LesserAttachUI = m_AttachComponent_Lesser;
        if (MainAttachUI == null)
        {
            Debug.LogError("Not Contain component : UnityEngine.UI.Button");
        }
    }

    public override void OnRecycleUIItem()
    {
        //base.OnRecycle();
        if (MainAttachUI == null) return;
        m_AttachComponent.onClick.RemoveAllListeners();
        m_AttachComponent.interactable = true;


        if (m_AttachComponent_Lesser == null) return;
        m_AttachComponent_Lesser.text = "";
        m_AttachComponent_Lesser.color = Color.white;
    }


    public override Image GetMainAttachUI_ImageComponent()
    {
        if (m_AttachMainImageComponent == null)
        {
            Debug.LogError("GetMainAttachUI_Image2Component Fail,m_AttachMainImageComponent is null ");
            return null;
        }
        return   m_AttachMainImageComponent;
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
