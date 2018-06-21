using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Sec : MonoBehaviour {
    public Test_SectorLayoutView m_Test_SectorLayoutView;
    public int dateCount = 5;
    private List<string> data = new List<string>();
    // Use this for initialization
    void Start () {
		for (int dex=0;dex<dateCount;++dex)
		{
            data.Add(dex.ToString());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
        {
            m_Test_SectorLayoutView.SetDataSources(data);
            m_Test_SectorLayoutView.Show();
        }
	}
}
