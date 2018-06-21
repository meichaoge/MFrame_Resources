using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRectTransformPos : MonoBehaviour
{
    private Canvas m_Canvas;
    private RectTransform m_Rectransform;

    void Start()
    {
        m_Rectransform = transform as RectTransform;
        m_Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    void Update()
    {
        Vector2 pos;
        //****RectTransformUtility.ScreenPointToLocalPointInRectangle() 对于给定的位置获取屏幕的位置坐标
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.transform as RectTransform, Input.mousePosition, 
                        m_Canvas.worldCamera, out pos))
        {
            m_Rectransform.anchoredPosition = pos;
        }//****效果当前UI元素会显示在屏幕上鼠标的位置

    }
}
