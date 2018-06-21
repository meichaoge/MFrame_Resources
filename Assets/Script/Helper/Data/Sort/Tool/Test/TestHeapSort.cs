using MFramework;
using MFramework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHeapSort : MonoBehaviour {
    public int numberCount = 5;
    public List<int> data = new List<int>();
    public List<int> data2 = new List<int>();
    public bool m_IsRandom = true;

    // Use this for initialization
    void Start () {
        if (m_IsRandom)
            data.AddRange(RandomTool.GetRandomInt(1, numberCount * 2, numberCount));
        data2.AddRange(data);
        //for (int dex = 0; dex < numberCount; ++dex)
        //{
        //    Debug.Log("Before ; " + data[dex]);
        //}
        Debug.Log("--------------------------------");
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("BBBBBBBBBBBBBBBBBBBBB");

            PerformanceHelper.WatchPerformance("堆排序算法效率  ", () => { HeapSortHelper.Sort<int>(data2, (a, b) => { return a < b; }); });

            //for (int dex = 0; dex < numberCount; ++dex)
            //{
            //    Debug.Log("End ; " + data[dex]);
            //}
        }
    }
}
