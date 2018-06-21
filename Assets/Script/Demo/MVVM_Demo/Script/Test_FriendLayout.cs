using MFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_FriendLayout : MonoBehaviour
{
    public UIFriendView m_UIFriendView;
    public int m_Data = 50;
    // Use this for initialization
    void Start()
    {
        for (int dex=0;dex<m_Data;++dex)
        {
            FriendCfg cfg = new FriendCfg();
            cfg.m_ID = dex;
            cfg.m_Name = "好友 " + dex;
            UIFriendDataModel.GetInstance().CurViewBindDataBase.Add(cfg);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            m_UIFriendView.Open();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            m_UIFriendView.Close();
        }
    }
}
