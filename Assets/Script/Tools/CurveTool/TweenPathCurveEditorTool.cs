using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂在在需要执行DoTween动画而需要使用自定义的动画效果的对象上
/// </summary>
public class TweenPathCurveEditorTool : MonoBehaviour
{

    [Header("AnimationCurve Name Should Be Different")]
    [SerializeField]
    private TweenCurveInfor[] m_AnimationCurve = new TweenCurveInfor[1];
    public TweenCurveInfor[] AnimationCurveInformation { get { return m_AnimationCurve; } }

    private Dictionary<string, TweenCurveInfor> m_AllCurveConfiguredIC = new Dictionary<string, TweenCurveInfor>();

#if UNITY_EDITOR
    public void AddItem(int dex)
    {
        UnityEditor.Undo.RecordObject(this, "Add");
        List<TweenCurveInfor> infors = new List<TweenCurveInfor>();
        infors.AddRange(m_AnimationCurve);
        TweenCurveInfor item = new TweenCurveInfor();
        item.CurveName = "Need ReName";
        if (infors.Count >= dex&&dex>0)
        {
            infors.Insert(dex, item);
        }
        else
        {
            infors.Add(item);
        }

        m_AnimationCurve = infors.ToArray();
    }

    public void DeleteItem(int dex  )
    {
        UnityEditor.Undo.RecordObject(this, "Add");
        List<TweenCurveInfor> infors = new List<TweenCurveInfor>();
        infors.AddRange(m_AnimationCurve);
        if (infors.Count <= dex)
        {
            if (infors.Count > 0)
                infors.RemoveAt(infors.Count - 1);
        }
        else
        {
            infors.RemoveAt(dex);
        }
     
        m_AnimationCurve = infors.ToArray();
    }


    public void AddSubItem(int curveIndex,int subCurveIndex)
    {
        UnityEditor.Undo.RecordObject(this, "AddSubItem");
        List<TweenCurveInfor> infors = new List<TweenCurveInfor>();
        infors.AddRange(m_AnimationCurve);
        if(infors.Count< curveIndex+1)
        {
            Debug.LogError("AddSubItem Curve Not Exit");
            return;
        }
        infors[curveIndex].AddSubItem(subCurveIndex);
        m_AnimationCurve = infors.ToArray();
    }

    public void DeleteSubItem(int curveIndex, int subCurveIndex)
    {
        UnityEditor.Undo.RecordObject(this, "DeleteSubItem");
        List<TweenCurveInfor> infors = new List<TweenCurveInfor>();
        infors.AddRange(m_AnimationCurve);
        if (infors.Count < curveIndex + 1)
        {
            Debug.LogError("AddSubItem Curve Not Exit");
            return;
        }
        infors[curveIndex].DeleteSubItem(subCurveIndex);
        m_AnimationCurve = infors.ToArray();
    }


#endif



    private void Awake()
    {
        RecordConfg();
    }

    void RecordConfg()
    {
        if (AnimationCurveInformation == null) return;
        foreach (var item in m_AnimationCurve)
        {
            if (m_AllCurveConfiguredIC.ContainsKey(item.CurveName))
                Debug.LogError("Animation CurveName Should Be Different" + item.CurveName);
            else
                m_AllCurveConfiguredIC.Add(item.CurveName, item);
        }
    }

    /// <summary>
    /// 根据名称获取设置的曲线
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public TweenCurveInfor GetCurveInforByName(string name)
    {
        if (m_AllCurveConfiguredIC.ContainsKey(name) == false)
            return null;
        return m_AllCurveConfiguredIC[name];
    }



}
