using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//***2017/7/6 增加IsListenLocalTransPosition 以便于监听Transform的世界坐标变化并分发事件


/// <summary>
/// 辅助监听当前附着对象的Tranfrom 属性变化
/// </summary>
public class TransformListenerTool : MonoBehaviour
{
    /// <summary>
    /// 监听Transform 哪些属性的变化
    /// </summary>
    public enum ListenTransformChangeEventEnum
    {
        LocalPosition,  
        WorldPosition,
        Sacle,
        Rotation
    }
    /// <summary>
    /// 检测频率
    /// </summary>
    public enum CheckRate
    {
        Normal,
        DelayOneFrame,
        DelayTwoFrame
    }


    #region 事件定义
    Action<Transform> OnLocalPositionChangeEvent;
    Action<Transform> OnWorldPositionChangeEvent;

    Action<Transform> OnScaleChangeEvent;
    Action<Transform> OnRotationChangeEvent;

    private Dictionary<int, Action<Transform>> m_OnLocalPositionChangeEventCacheDic = new Dictionary<int, Action<Transform>>();  //缓存之前添加的事件
    private Dictionary<int, Action<Transform>> m_OnWorldPositionChangeEventCacheDic = new Dictionary<int, Action<Transform>>();  //缓存之前添加的事件

    private Dictionary<int, Action<Transform>> m_OnScaleChangeEventCacheDic = new Dictionary<int, Action<Transform>>();
    private Dictionary<int, Action<Transform>> m_OnRotationChangeEventCacheDic = new Dictionary<int, Action<Transform>>();


    private Dictionary<int, Action<Transform>> m_OnLocalPositionChangeEventRecordDic = new Dictionary<int, Action<Transform>>();  //记录已经添加的事件
    private Dictionary<int, Action<Transform>> m_OnWorldPositionChangeEventRecordDic = new Dictionary<int, Action<Transform>>();  //记录已经添加的事件

    private Dictionary<int, Action<Transform>> m_OnScaleChangeEventRecordDic = new Dictionary<int, Action<Transform>>();
    private Dictionary<int, Action<Transform>> m_OnRotationChangeEventRecordDic = new Dictionary<int, Action<Transform>>();
    #endregion

    #region 事件状态控制 属性
    public bool IsListenLocalTransPosition; //Local Or World

    [Range(0.001f, 10f)]
    public float m_PositionSensitivity = 0.1f;  //检测位置变化的灵敏度
    public bool m_ListenPositionChange = false;
    public bool ListenPositionChange
    {
        get { return m_ListenPositionChange; }
        set
        {
            if (m_ListenPositionChange == value) return;

#if UNITY_EDITOR
            Debug.Log("m_ListenPositionChange ...... " + value);
#else
           Debug.LogInfor("m_ListenPositionChange ...... " + value);
#endif

            m_ListenPositionChange = value;

            #region LocalTrans
            if (IsListenLocalTransPosition)
            {
                if (value == false)
                {
                    OnLocalPositionChangeEvent = null;
                    SaveEvntToCacheDic(ref m_OnLocalPositionChangeEventRecordDic, ref m_OnLocalPositionChangeEventCacheDic);
                }
                else
                    ReRegistEvent(ref OnLocalPositionChangeEvent, m_OnLocalPositionChangeEventCacheDic);
                return;
            }
            #endregion

            #region WorldPosition
            if (value == false)
            {
                OnWorldPositionChangeEvent = null;
                SaveEvntToCacheDic(ref m_OnWorldPositionChangeEventRecordDic, ref m_OnWorldPositionChangeEventCacheDic);
            }
            else
                ReRegistEvent(ref OnWorldPositionChangeEvent, m_OnWorldPositionChangeEventCacheDic);
            return;

            #endregion

        }
    }


    [Range(0.001f, 1f)]
    public float m_ScaleSensitivity = 0.1f;  //检测缩放变化的灵敏度
    public bool m_ListenScaleChange = false;
    public bool ListenScaleChange
    {
        get { return m_ListenScaleChange; }
        set
        {
            if (m_ListenScaleChange == value) return;
#if UNITY_EDITOR
            Debug.Log("m_ListenScaleChange ...... " + value);
#else
           Debug.LogInfor("m_ListenScaleChange ...... " + value);
#endif
            m_ListenScaleChange = value;
            if (value == false)
            {
                OnScaleChangeEvent = null;
                SaveEvntToCacheDic(ref m_OnScaleChangeEventRecordDic, ref m_OnScaleChangeEventCacheDic);

            }
            else
                ReRegistEvent(ref OnScaleChangeEvent, m_OnScaleChangeEventCacheDic);
        }
    }



    [Range(0.001f, 30f)]
    public float m_RotationSensitivity = 0.1f;  //检测旋转变化的灵敏度
    public bool m_ListenRotationChange = false;
    public bool ListenRotationChange
    {
        get { return m_ListenRotationChange; }
        set
        {
            if (m_ListenRotationChange == value) return;
#if UNITY_EDITOR
            Debug.Log("m_ListenRotationChange ...... " + value);
#else
           Debug.LogInfor("m_ListenRotationChange ...... " + value);
#endif
            m_ListenRotationChange = value;
            if (value == false)
            {
                OnRotationChangeEvent = null;
                SaveEvntToCacheDic(ref m_OnRotationChangeEventRecordDic, ref m_OnRotationChangeEventCacheDic);
            }
            else
                ReRegistEvent(ref OnRotationChangeEvent, m_OnRotationChangeEventCacheDic);
        }
    }


    #endregion

    #region 检测事件属性
   
    public CheckRate UpdateCheckRate = CheckRate.Normal;
    public bool ClearRecordEventOnHide = true; //是否在OnDisable 时候清理事件
    #endregion



    #region 初始化数据

    private Vector3 m_InitialLocalPosition;
    private Vector3 m_InitialWorldPosition;

    private Vector3 m_InitialScale;
    private Vector3 m_InitialRotation;

    #endregion



    #region Mono

    private void OnEnable()
    {
        SetTransformInfor();
    }

    private void OnDisable()
    {
        if (ClearRecordEventOnHide)
        {
            OnLocalPositionChangeEvent = null;
            m_OnLocalPositionChangeEventCacheDic.Clear();
            m_OnLocalPositionChangeEventRecordDic.Clear();

            OnWorldPositionChangeEvent = null;
            m_OnWorldPositionChangeEventCacheDic.Clear();
            m_OnWorldPositionChangeEventRecordDic.Clear();

            OnScaleChangeEvent = null;
            m_OnScaleChangeEventCacheDic.Clear();
            m_OnScaleChangeEventRecordDic.Clear();

            OnRotationChangeEvent = null;
            m_OnRotationChangeEventCacheDic.Clear();
            m_OnRotationChangeEventRecordDic.Clear();
        }

    }

    int m_TimeTickCount = 0;
    void Update()
    {

#if UNITY_EDITOR
        #region 编辑器测试
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Test .....PositonChange");
            if (OnLocalPositionChangeEvent != null)
                OnLocalPositionChangeEvent(transform);
            SetTransformInfor();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Test .....Scale Change");
            if (OnScaleChangeEvent != null)
                OnScaleChangeEvent(transform);
            SetTransformInfor();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Test .....Rotation Change");
            if(OnRotationChangeEvent!=null)
            OnRotationChangeEvent(transform);
            SetTransformInfor();
        }
        #endregion
#endif



        ++m_TimeTickCount;
        if (UpdateCheckRate == CheckRate.DelayOneFrame)
        {
            if (m_TimeTickCount % 2 != 0) return;
        }
        else if (UpdateCheckRate == CheckRate.DelayTwoFrame)
        {
            if (m_TimeTickCount % 3 != 0) return;
        }



        if (ListenPositionChange)
            OnCheckPositionChange();

        if (m_ListenScaleChange)
            CheckScaleChange();

        if (ListenRotationChange)
            CheckRotationChange();
    }

    #endregion

    #region 条件检测
    /// <summary>
    /// 检测位置变化
    /// </summary>
    void OnCheckPositionChange()
    {
        if (IsListenLocalTransPosition)
        {
            #region Listem LocalTrans

            if (OnLocalPositionChangeEvent == null)
            {
                SetTransformInfor();
                return;
            }

            if (Vector3.Distance(m_InitialLocalPosition, transform.localPosition) <= m_PositionSensitivity)
                return;

            Debug.Log("OnCheckLocalPositionChange");
            OnLocalPositionChangeEvent(transform);
            #endregion
        }//
        else
        {
            #region ListenWorldTrans
            if (OnWorldPositionChangeEvent == null)
            {
                SetTransformInfor();
                return;
            }

            if (Vector3.Distance(m_InitialWorldPosition, transform.position) <= m_PositionSensitivity)
                return;

            Debug.Log("OnCheckWorldPositionChange");
            OnWorldPositionChangeEvent(transform);
            #endregion
        }
        SetTransformInfor();
    }

    /// <summary>
    /// 检测缩放变化
    /// </summary>
    void CheckScaleChange()
    {
        if (OnScaleChangeEvent == null)
        {
            SetTransformInfor();
            return;
        }

        if (Vector3.Distance(m_InitialScale, transform.localScale) <= m_ScaleSensitivity)
            return;

        Debug.Log("CheckScaleChange");
        OnScaleChangeEvent(transform);
        SetTransformInfor();
    }
    void CheckRotationChange()
    {
        if (OnRotationChangeEvent == null)
        {
            SetTransformInfor();
            return;
        }

        if (Vector3.Distance(m_InitialRotation, transform.localEulerAngles) <= m_RotationSensitivity)
            return;

        Debug.Log("CheckRotationChange");
        OnRotationChangeEvent(transform);
        SetTransformInfor();
    }


    #endregion

    #region 事件注册和取消注册
    public void AddEventListenner(ListenTransformChangeEventEnum _eventEnum, Action<Transform> action)
    {
        if (action == null)
        {
            Debug.LogError("AddEventListenner Fail.. action is null");
            return;
        }
        int hashCode = action.GetHashCode();
        Debug.Log("Register Success " + _eventEnum);

        if (ListenPositionChange)
        {
            if (IsListenLocalTransPosition)
                _eventEnum = ListenTransformChangeEventEnum.LocalPosition;
            else
                _eventEnum = ListenTransformChangeEventEnum.WorldPosition;
        }//强制设置类型

        switch (_eventEnum)
        {
            case ListenTransformChangeEventEnum.LocalPosition:
                #region LocalPosition
                if (m_OnLocalPositionChangeEventRecordDic.ContainsKey(hashCode))
                {
                    Debug.LogError("Repeat Add Event" + _eventEnum);
                    return;
                } //重复注册检测
                m_OnLocalPositionChangeEventRecordDic.Add(hashCode, action);
                OnLocalPositionChangeEvent += action;
                break;
                #endregion
            case ListenTransformChangeEventEnum.WorldPosition:
                #region WorldPostion
                if (m_OnWorldPositionChangeEventRecordDic.ContainsKey(hashCode))
                {
                    Debug.LogError("Repeat Add Event" + _eventEnum);
                    return;
                }//重复注册检测
                m_OnWorldPositionChangeEventRecordDic.Add(hashCode, action);
                OnWorldPositionChangeEvent += action;

                #endregion
                break;
            case ListenTransformChangeEventEnum.Sacle:
                #region Scale
                if (m_OnScaleChangeEventRecordDic.ContainsKey(hashCode))
                {
                    Debug.LogError("Repeat Add Event" + _eventEnum);
                    return;
                }//重复注册检测
                m_OnScaleChangeEventRecordDic.Add(hashCode, action);
                OnScaleChangeEvent += action;
                #endregion
                break;
            case ListenTransformChangeEventEnum.Rotation:
                #region Rotation
                if (m_OnRotationChangeEventRecordDic.ContainsKey(hashCode))
                {
                    Debug.LogError("Repeat Add Event" + _eventEnum);
                    return;
                }//重复注册检测
                Debug.Log("Register Success " + _eventEnum);
                m_OnRotationChangeEventRecordDic.Add(hashCode, action);
                OnRotationChangeEvent += action;
                #endregion
                break;

        }

    }

    public void RemoveListener(ListenTransformChangeEventEnum _eventEnum, Action<Transform> action)
    {
        if (action == null)
        {
            Debug.LogError("RemoveListener Fail.. action is null");
            return;
        }
        int hashCode = action.GetHashCode();
        Debug.Log("UnRegist Success " + _eventEnum);

        if (ListenPositionChange)
        {
            if (IsListenLocalTransPosition)
                _eventEnum = ListenTransformChangeEventEnum.LocalPosition;
            else
                _eventEnum = ListenTransformChangeEventEnum.WorldPosition;
        }//强制设置类型

        switch (_eventEnum)
        {
            case ListenTransformChangeEventEnum.LocalPosition:
                #region LocalPosition

                if (m_OnLocalPositionChangeEventRecordDic.ContainsKey(hashCode) == false)
                {
                    Debug.LogError("Not Register Event" + _eventEnum);
                    return;
                }
                m_OnLocalPositionChangeEventRecordDic.Remove(hashCode);
                OnLocalPositionChangeEvent -= action;
                break;
                #endregion
            case ListenTransformChangeEventEnum.WorldPosition:
                #region WorldPosition

                if (m_OnWorldPositionChangeEventRecordDic.ContainsKey(hashCode) == false)
                {
                    Debug.LogError("Not Register  Event" + _eventEnum);
                    return;
                }
                m_OnWorldPositionChangeEventRecordDic.Remove(hashCode);
                OnWorldPositionChangeEvent -= action;
                break;
                #endregion
            case ListenTransformChangeEventEnum.Sacle:
                #region Scale

                if (m_OnScaleChangeEventRecordDic.ContainsKey(hashCode) == false)
                {
                    Debug.LogError("Not Register Event" + _eventEnum);
                    return;
                }
                m_OnScaleChangeEventRecordDic.Remove(hashCode);
                OnScaleChangeEvent -= action;
                break;
                #endregion
            case ListenTransformChangeEventEnum.Rotation:
                #region Rotation

                if (m_OnRotationChangeEventRecordDic.ContainsKey(hashCode) == false)
                {
                    Debug.LogError("Not Register Event" + _eventEnum);
                    return;
                }
                m_OnRotationChangeEventRecordDic.Remove(hashCode);
                OnRotationChangeEvent -= action;
                break;
                #endregion
        }
    }

    #endregion

    #region 初始条件监听
    /// <summary>
    /// 设置初始条件的数据
    /// </summary>
    void SetTransformInfor()
    {
        m_InitialLocalPosition = transform.localPosition;
        m_InitialWorldPosition = transform.position;

        m_InitialScale = transform.localScale;
        m_InitialRotation = transform.localEulerAngles;
    }
    #endregion


    /// <summary>
    /// 将缓存的数据重新注册到数据中
    /// </summary>
    /// <param name="targetEvent"></param>
    /// <param name="eventDic"></param>
    void ReRegistEvent(ref Action<Transform> targetEvent, Dictionary<int, Action<Transform>> eventDic)
    {
        SetTransformInfor(); //重新设置初始条件
                             //        if (targetEvent == null)
                             //        {
                             //#if UNITY_EDITOR
                             //            Debug.LogError("ReRegistEvent Fail.....");
                             //#else
                             //            Debug.Error("ReRegistEvent Fail.....");
                             //#endif
                             //            return;
                             //        }

        if (eventDic == null || eventDic.Count == 0)
        {
#if UNITY_EDITOR
            Debug.LogError("ReRegistEvent Fail,Eventdatabase is Null Or Empty.....");
#else
            Debug.LogError("ReRegistEvent Fail,Eventdatabase is Null Or Empty.....");
#endif
            return;
        }
        foreach (var item in eventDic)
            targetEvent += item.Value;
    }





    /// <summary>
    /// 将记录的Event 保存到缓存字典中
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_to"></param>
    void SaveEvntToCacheDic(ref Dictionary<int, Action<Transform>> _from, ref Dictionary<int, Action<Transform>> _to)
    {
        if (_from == null || _from.Count == 0) return;
        if (_to == null) _to = new Dictionary<int, Action<Transform>>();

        foreach (var item in _from)
        {
            _to.Add(item.Key, item.Value);
        }
        _from.Clear();
    }




}

