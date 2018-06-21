using UnityEngine;
using System.Collections;
using System;
using BeanVR;

/// <summary>
/// UICanvas 控制类  通过接口 IsRaycastLocationValid 返回值控制Canvas 是否能被射线检测到
/// </summary>
public class UICanvasController : MonoBehaviour, ICanvasRaycastFilter
{
   // [HideInInspector]
    public bool m_CanOperate = true;
    void Awake()
    {
        m_CanOperate = true;
     //   UIManagerModel.GetInstance().RegistCanvas(this, true);
    }

    /// <summary>
    /// 射线照射时候通过调用这个接口判断是否接收事件
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="eventCamera"></param>
    /// <returns></returns>
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return m_CanOperate;  //返回fase 则不接受事件
    }

    void OnDestroy()
    {
//        UIManagerModel.GetInstance().RegistCanvas(this, false);
    }


}
