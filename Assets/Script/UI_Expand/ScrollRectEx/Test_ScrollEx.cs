using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ScrollEx : MonoBehaviour
{
    public ScrollRectEx m_ScrollRectEx;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Show()
    {
        //   m_AllPantheonViewItems.Clear();
        m_ScrollRectEx.onInitializeItem = (go, index) =>
        {
            //UIPantheonListItem _UIPanthonListItemScript = go.GetAddComponent<UIPantheonListItem>();
            //_UIPanthonListItemScript.OnLoadUICacheItem();
            //_UIPanthonListItemScript.InitialItemView(m_PantheonData.m_AllPantheonInforList[index], OnPantheonItemClickCallback);
            //m_AllPantheonViewItems.Add(_UIPanthonListItemScript);
        };  //每次显示一个项的时候调用一次
        m_ScrollRectEx.Init(10);   //参数为总共的数据数量
        m_ScrollRectEx.SetContentPosition(0);  //设置显示的位置
    }

}
