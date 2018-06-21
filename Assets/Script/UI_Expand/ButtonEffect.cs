using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


/// <summary>
/// 点击时候Button 的缩放效果
/// </summary>
public class ButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Vector3 m_InitialScale;
    [Range(0,2f)]
    public float m_MaxScaleRate = 1.1f;


    private void Awake()
    {
        m_InitialScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = m_InitialScale * m_MaxScaleRate;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = m_InitialScale;
    }





}
