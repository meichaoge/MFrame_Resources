using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFramework;

/// <summary>
/// ��չ TransformListenerTool
/// </summary>
public class TransformListenerTool_Ex : MonoBehaviour
{
    public enum ListenTypeEnum
    {
        Position,
        Rotation,
        Scale
    }
    [Header("Whether StartUp Auto And Listener Self")]
    public bool IsListenSelf = true;
    public EventCenter.UpdateRate CheckRate = EventCenter.UpdateRate.NormalFrame;  //���Ƶ��


    [SerializeField]
    private bool m_IsInitialed = false;
    public bool IsInitialed
    {
        get { return m_IsInitialed; }
        private set
        {
            if (m_IsInitialed != value)
            {
                if (m_IsInitialed)
                    Debug.LogError("The TransformListener is Already StartUp");
                else
                    m_IsInitialed = value;
            }
        }
    } //��ʾ�Ƿ��Ѿ���ʼ��

    public Transform Target { private set; get; } //��������

    public TransformListenerItem_Position PositionListener;
    public TransformListenerItem_Rotation RotationListener;
    public TransformListenerItem_Scale ScaleListener;



    #region Mono Frame

    private void Awake()
    {
        if (IsListenSelf)
            StartUpListener(transform);

    }


    int m_TimeTickCount = 0;
    void Update()
    {
        if (false == IsInitialed || Target == null) return;

        ++m_TimeTickCount;
        if (CheckRate == EventCenter.UpdateRate.DelayOneFrame)
        {
            if (m_TimeTickCount % 2 != 0) return;
        }
        else if (CheckRate == EventCenter.UpdateRate.DelayTwooFrame)
        {
            if (m_TimeTickCount % 3 != 0) return;
        }

        if (PositionListener != null)
            PositionListener.CheckAnInvokeEvent(Target);

        if (RotationListener != null)
            RotationListener.CheckAnInvokeEvent(Target);

        if (ScaleListener != null)
            ScaleListener.CheckAnInvokeEvent(Target);

    }

    #endregion



    /// <summary>
    /// ���������� ������IsListenSelf =trueʱ�򣬻��Զ��Զ� �Ҽ����Լ�����������ֶ�ִ������
    /// </summary>
    /// <param name="listenTarget"></param>
    public void StartUpListener(Transform listenTarget)
    {
        if (IsInitialed)
        {
            Debug.Log("StartUpListener Fail,Repeat StartUpListener");
            return;
        }
        IsInitialed = true;
        Target = listenTarget;
    }




    /// <summary>
    /// ����ʱע������¼�
    /// </summary>
    /// <param name="CheckType"></param>
    /// <param name="action"></param>
    public void AddEvent(ListenTypeEnum CheckType, System.Action<Transform> action,bool InvokeOnceOnAddListener=false)
    {
        if (action == null) return;
        switch (CheckType)
        {
            case ListenTypeEnum.Position:
                PositionListener.RegisterEvent(action);
                break;
            case ListenTypeEnum.Rotation:
                RotationListener.RegisterEvent(action);
                break;
            case ListenTypeEnum.Scale:
                ScaleListener.RegisterEvent(action);
                break;
            default:
                Debug.LogError("AddEvent Fail... Not Define");
                break;
        }


        if (InvokeOnceOnAddListener)
            action(Target);
    }


}
