using MFramework;
using MFramework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShellSort : MonoBehaviour
{
    public int numberCount = 5;
    public List<int> data = new List<int>();
    public List<int> data2 = new List<int>();

    // Use this for initialization
    void Start()
    {
        data.AddRange(RandomTool.GetRandomInt(1, 100, numberCount));
        data2.AddRange(data);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("aaaaaaaaaaaaaaaaaaa");
            ShellSortHelper.Sort<int>(data2, CompareRather);
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
