using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TransformListenItem_Rotation : TransformListenerItem
{
    public override void CheckAnInvokeEvent(Transform target)
    {
        if (target == null) return;
        if (ListenTransChange == false || OnListenItemtTransChangeEvent == null)
        {
            UpdateRecordInfor(target);
            return;
        }

        //     TransformAxis curSetting
        switch (TransformCoordinateEnum)
        {
            case TransformCoordinate.Local:
                #region Local Rotation Check
                if (Vector3.Distance(m_InitialItemInfor_Local, target.localEulerAngles) <= ListenItemSensitivity)
                    return;

                Debug.Log("TransformListenItem_Rotation");
                if (OnListenItemtTransChangeEvent != null)
                    OnListenItemtTransChangeEvent(target);

                UpdateRecordInfor(target);
                return;
            #endregion
            case TransformCoordinate.World:
                #region Wold Rotation Check
                if (Vector3.Distance(m_InitialItemInfor_World, target.rotation.eulerAngles) <= ListenItemSensitivity)
                    return;

                Debug.Log("TransformListenItem_Rotation");
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
        m_InitialItemInfor_Local = target.localEulerAngles;
        m_InitialItemInfor_World = target.rotation.eulerAngles;
    }



}
