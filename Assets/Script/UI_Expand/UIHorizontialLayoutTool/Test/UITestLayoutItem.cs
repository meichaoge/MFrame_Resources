using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITestLayoutItem : UILayoutItem
{
    public Button m_Itembtn;
    public TextMeshProUGUI m_NameText;

    private void Start()
    {
        m_Itembtn.onClick.AddListener(() =>
        {
            if (m_Callback != null)
                m_Callback(this, m_Itembtn.transform);
        });
    }

    public override void Initialed(int data, int index, Action<UILayoutItem, Transform> OnItemClickCallback)
    {
        base.Initialed(data, index, OnItemClickCallback);
        m_NameText.text = data.ToString();
        m_ItemIndex = index;
    }


}
