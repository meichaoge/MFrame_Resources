using MFramework;
using MFramework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuickSort : MonoBehaviour {
  public  int numberCount = 5;
    public List<int> data = new List<int>();
    public List<int> data2 = new List<int>();
    void Start () {
        data.AddRange(RandomTool.GetRandomInt(1, numberCount*2, numberCount));
        data2.AddRange(data);
        Debug.Log("--------------------------------");
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            PerformanceHelper.WatchPerformance("快速排序算法效率  ", ()=> { QuickSortHelper.Sort<int>(data2, 0, numberCount - 1, rather, less); });
            for (int dex = 0; dex < data.Count; ++dex)
            {
                if (IsExit(data[dex]) == false)
                {
                    Debug.LogError("Not Exit " + data[dex]);
                }
            }

        }
    }


    private bool IsExit(int search)
    {
        for (int dex = 0; dex < data2.Count; ++dex)
        {
            if (search == data2[dex])
                return true;
        }
        return false;
    }

    bool less(int a,int b)
    {
        return (a <= b);
    }

    bool rather(int a, int b)
    {
        return (a >= b);
    }

}
