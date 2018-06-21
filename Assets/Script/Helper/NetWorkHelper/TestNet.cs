using MFramework.NetWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNet : MonoBehaviour {
    public float  size = 1;
    public int times = 1;
    public int Subtract = 0;
    public NetDataEnum m_NetDataEnum;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
        {
            for (int dex=0;dex< times;++dex)
            {
                size *= 1024;
            }
            size -= Subtract;
            Debug.Log("Before   size=" + size + "  " + m_NetDataEnum);
            NetWorkTool.GetInstance().GetNetDataDesciption(ref size, ref m_NetDataEnum,2);
            Debug.Log("size=" + size + "   " + m_NetDataEnum);
        }
	}
}
