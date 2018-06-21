using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFramework;

public class Test_SectorLayoutView : MonoBehaviour
{
    private SectorLayout m_SectorLayoutScript;
    private List<string> m_ShowDataSources = new List<string>();
    private List<SectorLayout.SectorLayoutPointInfor> m_ShowingPoint = null;
    public GameObject m_ItemPrefab = null;
    private void Awake()
    {
        m_SectorLayoutScript = transform.GetComponent<SectorLayout>();
    }

    /// <summary>
    /// 显示前必须先获取将要显示的点的数据
    /// </summary>
    /// <param name="data"></param>
    public void SetDataSources(List<string> data)
    {
        if (data == null || data.Count == 0) return;
        m_ShowingPoint = m_SectorLayoutScript.GetSectorLayout((uint)data.Count);
    }

    public void Show()
    {
        for (int dex = 0; dex < m_ShowingPoint.Count; ++dex)
        {
            SectorLayout.SectorLayoutPointInfor pointInfor = m_ShowingPoint[dex];
            GameObject go = GameObject.Instantiate(m_ItemPrefab);
            go.transform.SetParent(m_SectorLayoutScript.SectorLayoutCenterTrans);
            go.transform.localPosition = pointInfor.m_RalativePos;
            go.transform.localEulerAngles = new Vector3(0, 0, pointInfor.m_Angle);
            go.transform.localScale = Vector3.one;
        }
    }


}
