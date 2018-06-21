using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_AddTransformListener : MonoBehaviour {
    public TransformListenerTool_Ex m_TransformListenerTool_Ex;

    // Use this for initialization
    void Start ()
    {
        m_TransformListenerTool_Ex.AddEvent(TransformListenerTool_Ex.ListenTypeEnum.Position, FollowTarget_Position,true);
        m_TransformListenerTool_Ex.AddEvent(TransformListenerTool_Ex.ListenTypeEnum.Rotation, FollowTarget_Rotation, true);
        m_TransformListenerTool_Ex.AddEvent(TransformListenerTool_Ex.ListenTypeEnum.Scale, FollowTarget_Scale, true);

    }


    void FollowTarget_Position(Transform trans)
    {
        transform.position = trans.position;
    }


    void FollowTarget_Rotation(Transform trans)
    {
        transform.rotation = trans.rotation;
    }


    void FollowTarget_Scale(Transform trans)
    {
        transform.localScale = trans.lossyScale;
    }


}
