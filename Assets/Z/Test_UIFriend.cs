using MFramework.UI.Layout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_UIFriend : MonoBehaviour {
    private SimpleGridLayout m_SimpleGridLayout;
    // Use this for initialization
    void Start () {
        m_SimpleGridLayout = transform.GetComponentInChildren<SimpleGridLayout>();
        m_SimpleGridLayout.OnLayoutInitial_AfterItemUIButtonCreateAct += OnFriendPageItemInitial;  //注册列表项点击事件
        m_SimpleGridLayout.InitialLayout();
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.O ))
        {
            Open();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Close();
        }
    }

    public void Open()
    {
        m_SimpleGridLayout.ReBuildView(UIFriendDataModel.GetInstance().CurViewBindDataBase.Count, true, true);
    }


    public void Close()
    {

    }
    //布局类初始化
    void OnFriendPageItemInitial(BaseLayoutButton itemBtn)
    {
        UIFriendItemBtn friendItemBtn = itemBtn as UIFriendItemBtn;
        if (friendItemBtn == null)
        {
            Debug.LogError("OnFriendPageItemInitial Fail,Not Right Type " + itemBtn.GetType());
            return;
        }
        Debug.Log("OnFriendPageItemInitial  " + friendItemBtn.m_ID);

    }

}
