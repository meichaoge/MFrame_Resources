using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// LoopScrollRect 是所有子项的父类
/// </summary>
public class LoopScrollRectItemBase : MonoBehaviour
{
    public int m_DataIndex; //指向的数据索引
    protected RectTransform m_RectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if (m_RectTransform == null)
                m_RectTransform = transform as RectTransform;
            return m_RectTransform;
        }
        set
        {
            m_RectTransform = value;
        }
    }

  
    public virtual void ScrollCellIndex(int idx)
    {
        m_DataIndex = idx;

        //string name = "Cell " + idx.ToString();
        //gameObject.name = name;
    }

  
}
