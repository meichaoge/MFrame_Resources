using MFramework;
using MFramework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSelectSort : MonoBehaviour {
    public int numberCount = 5;
    public List<int> data = new List<int>();
    public List<int> data2 = new List<int>();

    // Use this for initialization
    void Start () {
        data.AddRange(RandomTool.GetRandomInt(1, 100, numberCount));
        data2.AddRange(data);

        for (int dex = 0; dex < numberCount; ++dex)
        {
            Debug.Log("Before ; " + data[dex]);
        }
    }
	
	// Update is called once per framese
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("aaaaaaaaaaaaaaaaaaa");
            SelectSortHelper.Sort<int>(data , CompareRather);
            for (int dex = 0; dex < numberCount; ++dex)
            {
                Debug.Log("End ; " + data[dex]);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("BBBBBBBBBBBBBBBBBBBBB");
            SelectSortHelper.Sort<int>(data2 , CompareLess);
            for (int dex = 0; dex < numberCount; ++dex)
            {
                Debug.Log("End ; " + data2[dex]);
            }
        }
    }

    bool CompareLess(int a,int b)
    {
        return a < b;
    }

    bool CompareRather(int a, int b)
    {
        return a > b;
    }
}
