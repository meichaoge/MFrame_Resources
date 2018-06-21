using MFramework;
using MFramework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMergeSort : MonoBehaviour {

    public int numberCount = 5;
    public List<int> data = new List<int>();
    // Use this for initialization
    void Start()
    {
        data.AddRange(RandomTool.GetRandomInt(1, numberCount*2, numberCount));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("aaaaaaaaaaaaaaaaaaa");
            List<int> temp = new List<int>();
            temp.AddRange(data);
           PerformanceHelper.WatchPerformance("归并排序算法效率  ", () => { MergeSortHelper.Sort<int>(data, 0, data.Count - 1, CompareRather, temp); });
        }
    }


    bool CompareLess(int a, int b)
    {
        return a < b;
    }

    bool CompareRather(int a, int b)
    {
        return a > b;
    }
}
