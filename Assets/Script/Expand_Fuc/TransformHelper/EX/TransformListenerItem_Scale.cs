using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 监听缩放改变
/// </summary>
[System.Serializable]
public class TransformListenerItem_Scale : TransformListenerItem
{
    public override void CheckAnInvokeEvent(Transform target)
    {
        if (target == null) return;
        if (ListenTransChange == false || OnListenItemtTransChangeEvent == null)
        {
            //Debug.Log("CheckAnInvokeEvent Fail... No Listener");
            UpdateRecordInfor(target);
            return;
        }

        //     TransformAxis curSetting
        switch (TransformCoordinateEnum)
        {
            case TransformCoordinate.Local:
                #region LocalPositon Check

                if (Vector3.Distance(m_InitialItemInfor_Local, target.localScale) <= ListenItemSensitivity)
                    return;

                Debug.Log("TransformListenItem_Scale");
                if (OnListenItemtTransChangeEvent != null)
                    OnListenItemtTransChangeEvent(target);

                UpdateRecordInfor(target);
                return;
            #endregion
            case TransformCoordinate.World:
                #region Wold Position Check
                if (Vector3.Distance(m_InitialItemInfor_World, target.lossyScale) <= ListenItemSensitivity)
                    return;

                Debug.Log("TransformListenItem_Scale");
                if (OnListenItemtTransChangeEvent != null)
                    OnListenItemtTransChangeEvent(target);

                UpdateRecordInfor(target);
                return;
            #endregion
            default:
                Debug.Log("Not Define");
                break;
        }

    }




    protected override void UpdateRecordInfor(Transform target)
    {
        base.UpdateRecordInfor(target);
        m_InitialItemInfor_Local = target.localScale;
        m_InitialItemInfor_World = target.lossyScale; //世界坐标系缩放 ，只读

    }


}
