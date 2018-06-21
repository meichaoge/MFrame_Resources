using StoneSkin.GUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIHorizontialLayoutTool : MonoBehaviour {
    public UIHorizontialLayoutTool m_UIHorizontialLayoutTool;
    // Use this for initialization
    void Start () {
        m_UIHorizontialLayoutTool.InitialLayoutView(8, OnItemCreate);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    m_UIHorizontialLayoutTool.OnItemMoveing();
        //}
    }



    void OnItemCreate(GameObject goItem, int dex)
    {
        UILayoutItem script = goItem.GetAddComponent<UILayoutItem>();
        script.Initialed(dex, dex, m_UIHorizontialLayoutTool.OnItemClick);
    }




}
