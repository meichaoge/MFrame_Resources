using MFramework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBubbleSort : MonoBehaviour
{
    public int m_InitialedData = 10;
    public List<int> m_SourcesData = new List<int>();
    public List<int> m_SortData = new List<int>();
    // Use this for initialization
    void Start()
    {
        m_SourcesData.Clear();

        for (int dex = 0; dex < m_InitialedData; ++dex)
        {
            m_SourcesData.Add(Random.Range(0, 100));
        }
        m_SortData.AddRange(m_SourcesData);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            BubbleSortHelper.Sort<int>(m_SortData, (a, b) =>
            {
                return a > b;
            });
        }
    }
}
