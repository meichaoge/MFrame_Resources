using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 拥有TextMesh的Button
/// </summary>
public class UICachePrefab_ButtonWithTextMesh : UICachePrefabBase
{
    protected UnityEngine.UI.Button m_AttachComponent;//{ get; protected set; }
    protected TMPro.TextMeshProUGUI m_AttachComponent_Lesser;//{ get; protected set; }
    protected UnityEngine.UI.Image m_AttachMainImageComponent;  //Button 上的Image 组件

    protected List<Transform> m_AllTextChild = new List<Transform>();
    protected int m_SelectValue = 0;

#if UNITY_EDITOR

    protected override void Reset()
    {
        if (Application.isPlaying) return;
        base.Reset();
        GetAllSubText();
    }

    protected override void OnValidate()
    {
        if (Application.isPlaying) return;
        base.Reset();
        GetAllSubText();
    }

#endif


    protected override void Awake()
    {
        if (m_AllTextChild == null || m_AllTextChild.Count == 0)
            if (GetAllSubText() == false)
                return;

        m_AttachComponent = gameObject.GetComponent<UnityEngine.UI.Button>();
        m_AttachMainImageComponent = gameObject.GetComponent<UnityEngine.UI.Image>();

        MainAttachUI = m_AttachComponent;
        LesserAttachUI = null; //在后面赋值
        if (MainAttachUI == null)
        {
            Debug.LogError("Not Contain component : UnityEngine.UI.Button");
        }

    }
    /// <summary>
    /// 初始化Buttton 
    /// </summary>
    /// <param name="operateEnum"></param>
    /// <param name="paramet"></param>
    public override void OnGetUIItemInitial(UICacheMarkOperate operateEnum, string paramet)
    {
     //   Debug.LogInfor("OnGetUIItemInitial    searchIndex=" + paramet);
        //   if (operateEnum == UICacheMarkOperate.SelectBtnTextType)
        {
            if (m_AllTextChild == null || m_AllTextChild.Count == 0)
            {
                Debug.LogError("OnGetUIItemInitial Fail,This UICachePrefab_ButtonWithTextMesh Not Set Right Index  ::" + transform.parent.name);
                return;
            }
            if (int.TryParse(paramet, out m_SelectValue) == false)
            {
                Debug.LogError("选择的Text 索引不是整数形式  " + transform.parent.name);
                m_SelectValue = 0;
            }

            SelectTextByIndex(m_SelectValue);
        }
    }

    /// <summary>
    /// 获得所有的子Text 
    /// </summary>
    /// <returns></returns>
    protected bool GetAllSubText()
    {
        m_AllTextChild.Clear();
        for (int dex = 0; dex < transform.childCount; ++dex)
        {
            m_AllTextChild.Add(transform.GetChild(dex));
        }

        if (m_AllTextChild == null || m_AllTextChild.Count == 0)
        {
            Debug.LogError("UICachePrefab_ButtonWithTextMesh Must Have Childs");
            return false;
        }
        return true;
    }
    /// <summary>
    /// 设置需要显示的Text 
    /// </summary>
    /// <param name="searchIndex"></param>
    protected void SelectTextByIndex(int searchIndex)
    {
    //    Debug.LogInfor("SelectTextByIndex    searchIndex=" + searchIndex + "  m_AllTextChild.cou " + m_AllTextChild.Count);
        for (int dex = 0; dex < m_AllTextChild.Count; ++dex)
        {
            if (dex == searchIndex)
            {
                m_AttachComponent_Lesser = m_AllTextChild[dex].GetComponent<TMPro.TextMeshProUGUI>();
                LesserAttachUI = m_AttachComponent_Lesser;

                if (m_AllTextChild[dex].gameObject.activeSelf == false)
                    m_AllTextChild[dex].gameObject.SetActive(true);
            }
            else
            {
                if (m_AllTextChild[dex].gameObject.activeSelf)
                    m_AllTextChild[dex].gameObject.SetActive(false);
            }
        }
    }



    public override void OnRecycleUIItem()
    {
        if (m_AttachComponent == null) return;
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
        return m_AttachMainImageComponent;
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
