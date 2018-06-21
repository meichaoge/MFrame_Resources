using MFramework.UI.Layout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIFriendItemBtn : BaseLayoutButton
{
    public Text m_NameText;
    public long m_ID;

    public override void InitialButtonItem(object _sender, int _dataIndex, int _relativeIndexList = -1, int _relativeIndexItem = -1)
    {
        base.InitialButtonItem(_sender, _dataIndex, _relativeIndexList, _relativeIndexItem);
        FriendCfg _data = UIFriendDataModel.GetInstance().GetDataByIndex(_dataIndex);
        if (_data == null)
        {
            Debug.LogError("InitialButtonItem Fail ,Data is Null");
            return;
        }
        m_ID = _data.m_ID;
        m_NameText.text = _data.m_Name;

    }

    /// <summary>
    /// 刷新
    /// </summary>
    public override void FlushView()
    {
        base.FlushView();
        if (LayoutScrollViewScript != null && gameObject.activeSelf)
        {
            Debug.Log("FlushView " + m_ID);
            InitialButtonItem(LayoutScrollViewScript, m_DataIndex, ViewRelativeIndex.X, ViewRelativeIndex.Y);
        }
    }



}
