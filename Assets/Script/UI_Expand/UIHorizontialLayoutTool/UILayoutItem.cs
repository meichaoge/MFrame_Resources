using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILayoutItem : MonoBehaviour
{
    protected System.Action<UILayoutItem, Transform> m_Callback;
    public int m_ItemIndex { get; protected set; }

    public bool m_IsFocusOn;//{ get; protected set; }
    protected static UILayoutItem m_PreviousSelect = null;

    protected virtual void Awake()
    {
        m_IsFocusOn = false;
    }

    public virtual void Initialed(int data, int index, System.Action<UILayoutItem, Transform> OnItemClickCallback)
    {
        m_Callback = OnItemClickCallback;
        m_ItemIndex = index;
    }
    //当点击后移动到正中心的操作  真正的点击操作
    public virtual void OnAfterItemClick(Transform souroucesTarget)
    {
        //Debug.Log("OnAfterItemClick " + gameObject.name);
    }

    /// <summary>
    /// 当当前卡牌处于正中间的时候
    /// </summary>
    public virtual void OnLayoutItemIsFocus()
    {
        if(m_PreviousSelect!=this)
        {
            if (m_PreviousSelect != null)
                m_PreviousSelect.OnLayoutItemLoseFocus();

        }
        m_PreviousSelect = this;
    }

    public virtual void OnLayoutItemLoseFocus()
    {
        m_IsFocusOn = false;
    }


}
