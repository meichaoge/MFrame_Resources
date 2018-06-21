using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformListenerItem_Position : TransformListenerItem
{

    public float Test_Resoult = 0;

    public override void CheckAnInvokeEvent(Transform target)
    {
        if (target == null) return;
        if (ListenTransChange == false || OnListenItemtTransChangeEvent == null)
        {
            Debug.Log("CheckAnInvokeEvent Fail... No Listener");
            UpdateRecordInfor(target);
            return;
        }

        //     TransformAxis curSetting
        switch (TransformCoordinateEnum)
        {
            case TransformCoordinate.Local:
                #region LocalPositon Check
                Test_Resoult = Vector3.Distance(m_InitialItemInfor_Local, target.localPosition);

                if (Vector3.Distance(m_InitialItemInfor_Local, target.localPosition) <= ListenItemSensitivity)
                    return;

                Debug.Log("TransformListenerItem_Position");
                if (OnListenItemtTransChangeEvent != null)
                    OnListenItemtTransChangeEvent(target);

                UpdateRecordInfor(target);
                return;
                #endregion
            case TransformCoordinate.World:
                #region Wold Position Check
                Test_Resoult = Vector3.Distance(m_InitialItemInfor_World, target.position);
                if (Vector3.Distance(m_InitialItemInfor_World, target.position) <= ListenItemSensitivity)
                    return;

                Debug.Log("TransformListenerItem_Position");
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
        m_InitialItemInfor_Local = target.localPosition;
        m_InitialItemInfor_World = target.position;
    }





}
