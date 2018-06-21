using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLoopScrollRect : MonoBehaviour {

    public LoopScrollRect m_LoopScrollRect;
    public List<TestItem> m_AllItems = new List<TestItem>();
    public int ItemIndex = 0;
    public Vector2 pos;
    public int Number;
    // Use this for initialization
    void Start () {
        m_LoopScrollRect= GetComponent<LoopScrollRect>();
        m_LoopScrollRect.OnItemRemoveEvent += OnItemRemove;
        m_LoopScrollRect.OnItemCreateEvent += OnItemCreate;
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L))
        {
            if(m_LoopScrollRect is LoopVerticalScrollRect)
            {
                 Number = (int)(ItemIndex / m_LoopScrollRect.m_ContentConstraintCount);
                 pos =new Vector2(0, Number* m_LoopScrollRect.GetItemSize());
                m_LoopScrollRect.SetContentAnchoredPosition(pos);
            }

            if (m_LoopScrollRect is LoopHorizontalScrollRect)
            {
                Number = (int)(ItemIndex / m_LoopScrollRect.m_ContentConstraintCount);
                pos = new Vector2(Number* m_LoopScrollRect.GetItemSize(),   0);
                m_LoopScrollRect.SetContentAnchoredPosition(pos);
            }
        }
	}

    private void OnItemRemove(RectTransform item)
    {
        TestItem _TestItem = item.GetAddComponent<TestItem>();
        m_AllItems.Remove(_TestItem);
    }

    private void OnItemCreate(RectTransform item,int index)
    {
        TestItem _TestItem = item.GetAddComponent<TestItem>();
        _TestItem.InitialedItem(index);
        m_AllItems.Add(_TestItem);
    }

}
