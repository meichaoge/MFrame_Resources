using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 曲线数值信息
/// </summary>
[System.Serializable]
public class TweenCurveInfor
{
    [System.Serializable]
    public class SubCurveInfor
    {
        public string m_CurveName;
        public AnimationCurve m_SubCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    }

    public string CurveName;



    [Tooltip("Replace Curve Parameter")]
    public SubCurveInfor[] SubCurveList = new SubCurveInfor[1];//多值曲线

    public Vector3 StartValueVec = Vector3.zero; ///开始执行时候的值 需要手动设置而不会自动改变 Curve 的值
    public Vector3 EndValueVec = Vector3.zero;  //结束时候的值

    public float TweenTime = 0.1f; //持续时间
    public float DelayTime = 0f; //延迟时间


#if UNITY_EDITOR

    public void AddSubItem(int dex)
    {
       // UnityEditor.Undo.RecordObject(this, "Add");
        List<SubCurveInfor> infors = new List<SubCurveInfor>();
        infors.AddRange(SubCurveList);
        SubCurveInfor item = new SubCurveInfor();
        item.m_CurveName = "Need ReName SubCurve";
        if (infors.Count >= dex && dex > 0)
        {
            infors.Insert(dex, item);
        }
        else
        {
            infors.Add(item);
        }

        SubCurveList = infors.ToArray();
    }

    public void DeleteSubItem(int dex)
    {
     //   UnityEditor.Undo.RecordObject(this, "Delete");
        List<SubCurveInfor> infors = new List<SubCurveInfor>();
        infors.AddRange(SubCurveList);
        if (infors.Count <= dex) return;
        infors.RemoveAt(dex);
        SubCurveList = infors.ToArray();
    }


#endif




    /// <summary>
    /// 根据名称获取子曲线
    /// </summary>
    /// <param name="curveName"></param>
    /// <returns></returns>
    public AnimationCurve GetSubCurveInforByName(string subCurveName)
    {
        if (SubCurveList == null)
        {
            Debug.LogError("GetSubCurveInforByName Fail Not Exit Sub Curve " + subCurveName);
            return null;
        }
        AnimationCurve infor = null;
        if (SubCurveList != null)
            for (int dex = 0; dex < SubCurveList.Length; ++dex)
            {
                if (SubCurveList[dex].m_CurveName == subCurveName)
                {
                    infor = SubCurveList[dex].m_SubCurve;
                    break;
                }
            }
        if (infor == null)
        {
            Debug.LogError("GetSubCurveInforByName Fail Not Exit Sub Curve " + subCurveName + "Of " + CurveName);
        }
        return infor;
    }

    /// <summary>
    /// 根据下标获取子曲线
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public AnimationCurve GetSubCurveInforByIndex(int index = 0)
    {
        if (SubCurveList == null)
        {
            Debug.LogError("GetSubCurveInforByName Fail Not Exit Sub Curve");
            return null;
        }
        if (index >= 0 && index < SubCurveList.Length)
        {
            return SubCurveList[index].m_SubCurve;
        }
        Debug.LogError("GetSubCurveInforByName Fail Not Exit Sub Curve index = " + index);
        return null;
    }


}
