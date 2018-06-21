//using MFramework.EditorExpand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//[System.Flags]
//public enum TransformAxis
//{
//    X = 0,
//    Y = 1,
//    Z = 2
//}
/// <summary>
/// ����ϵ
/// </summary>
public enum TransformCoordinate
{
    Local,
    World
}


[System.Serializable]
public class TransformListenerItem
{

    public bool m_ListenTransChange = false;
    public bool ListenTransChange
    {
        get { return m_ListenTransChange; }
        set
        {
            if (m_ListenTransChange == value) return;
#if UNITY_EDITOR
            Debug.Log("ListenTransChange ...... " + value + " :Coord=" + TransformCoordinateEnum);//+ "  TransformAxis="+ TransformAxisEnum);
//#else
//           Log4Helper.Info("ListenTransChange ...... " + value+" :Coord="+ TransformCoordinateEnum+ "  TransformAxis="+ TransformAxisEnum);
#endif
            m_ListenTransChange = value;
            if (value == false)
            {
                UnRegisterEventToCache();
                OnListenItemtTransChangeEvent = null;
                return;
            } //ȡ������ ��������

            RegisterEventFromCache(); //������ע�����
        }
    }

    public TransformCoordinate TransformCoordinateEnum=TransformCoordinate.Local;
    //[Enum_FlagsAttribute("Listen Axis")]
    //public TransformAxis TransformAxisEnum = TransformAxis.X; //��������

     [Header("Event Define")]
    //�¼�����
    protected Action<Transform> OnListenItemtTransChangeEvent;
    protected List<Action<Transform>> m_AllRecordAct = new List<Action<Transform>>();
    protected List<Action<Transform>> m_AllCacheAct = new List<Action<Transform>>();

    [Header("Check condition")]
    [Range(0.001f, 1f)]
    public float ListenItemSensitivity = 0.1f;  //���仯��������

    [Header("Record State")]
    [SerializeField]
    protected Vector3 m_InitialItemInfor_Local; //��ʼ��״̬
    [SerializeField]
    protected Vector3 m_InitialItemInfor_World; //��ʼ��״̬

    public void RegisterEvent(Action<Transform> action)
    {
        if (action == null) return;
        m_AllRecordAct.Add(action);
        OnListenItemtTransChangeEvent += action;
    }

    public void RegisterEventFromCache()
    {
        if (m_AllCacheAct == null || m_AllCacheAct.Count == 0) return;
        for (int dex = 0; dex < m_AllCacheAct.Count; ++dex)
        {
            m_AllRecordAct.Add(m_AllCacheAct[dex]);
            OnListenItemtTransChangeEvent += m_AllCacheAct[dex];
        }
        m_AllCacheAct.Clear();

    }

    public void UnRegisterEventToCache()
    {
        if (m_AllRecordAct == null || m_AllRecordAct.Count == 0) return;
        for (int dex = 0; dex < m_AllRecordAct.Count; ++dex)
        {
            m_AllCacheAct.Add(m_AllCacheAct[dex]);
            OnListenItemtTransChangeEvent -= m_AllCacheAct[dex];
        }
        m_AllRecordAct.Clear();

    }

    /// <summary>
    /// ����Ƿ����ı䲢�����¼�
    /// </summary>
    /// <param name="target"></param>
    public virtual void CheckAnInvokeEvent(Transform  target)   {  }

    protected virtual void UpdateRecordInfor(Transform target) { }

    public void ClearAllEvent()
    {
        OnListenItemtTransChangeEvent = null;
        m_AllRecordAct.Clear();
        m_AllCacheAct.Clear();
    }



}
