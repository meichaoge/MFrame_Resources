using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestItem : MonoBehaviour
{
    private Text m_Text;
    public int   m_Index;
    private LoopScrollRectItemBase m_LoopScrollRectItemBase;

    private void Awake()
    {
        m_LoopScrollRectItemBase = transform.GetComponent<LoopScrollRectItemBase>();
        m_Text = transform.Find("Text").GetComponent<Text>();
    }


    public void InitialedItem(int dataIndex)
    {
        m_Index = dataIndex;
        m_Text.text = m_Index.ToString();
    }

}
