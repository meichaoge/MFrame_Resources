using MFramework;
using MFramework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInsertSort : MonoBehaviour
{
    public int numberCount = 5;
    public List<int> data = new List<int>();

    // Use this for initialization
    void Start()
    {
        data.AddRange(RandomTool.GetRandomInt(1, 100, numberCount));
        for (int dex = 0; dex < numberCount; ++dex)
        {
            Debug.Log("Before ; " + data[dex]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("aaaaaaaaaaaaaaaaaaa");
            InsertSortHelper.Sort<int>(data, CompareRather);
            for (int dex = 0; dex < numberCount; ++dex)
            {
                Debug.Log("End ; " + data[dex]);
            }
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
