using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEvent : MonoBehaviour
{
    public LoopVerticalScrollRect m_LoopVerticalScrollRect;
    public List<RectTransform> m_AllItems = new List<RectTransform>();
    [Range(0,100)]
    public int m_DataCount=5;

    private void Awake()
    {
        m_LoopVerticalScrollRect.OnItemRemoveEvent += OnItemRemove;
        m_LoopVerticalScrollRect.OnItemCreateEvent += OnItemCreate;

        m_LoopVerticalScrollRect.RefillCells(m_DataCount);
    }



    private void OnItemRemove(RectTransform item)
    {
        m_AllItems.Remove(item);
    }

    private void OnItemCreate(RectTransform item,int index)
    {
        m_AllItems.Add(item);
    }

}
