using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFramework;
public class bbb : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int dex = 0; dex < 5; ++dex)
        {
            GameObject go = new GameObject(dex + "");
            go.transform.SetAsFirstSibling();
            go.AddComponent<AAA>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AAA.GetInstance();
        }
	}
}
