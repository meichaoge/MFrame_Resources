using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class TransFormHelper
{
    /// <summary>
    /// @from 正方向面向@to ，且只在水平方向上面向，及只控制Y轴旋转
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="additionalAngle">额外的角度 一般是0或者180</param>
    public static void LookAtTargetAtHorizontal(Transform @from, Transform @to)
    {
        //***************DoTwee
        @from.DOLookAt(@to.position, 0, AxisConstraint.Y, Vector3.up);
        //if (isreverse)
        //    @from.localEulerAngles += new Vector3(0, 180, 0);
    }


    public static void LookAtTargetAtHorizontal_Immediately(Transform @from, Transform @to)
    {
        Vector3 _RelativePosition = @to.position - @from.position;
        if (@to.position.y > @from.position.y)
            _RelativePosition = Vector3.ProjectOnPlane(_RelativePosition, Vector3.up); //在水平面下下的投影
        else
            _RelativePosition = Vector3.ProjectOnPlane(_RelativePosition, Vector3.down); //在水平面下下的投影

        Vector3 _horizontialFoward; //@from 水平方向上的前方
        if (@from.position.y > 0)
            _horizontialFoward = Vector3.ProjectOnPlane(@from.forward, Vector3.up); //在水平面下下的投影
        else
            _horizontialFoward = Vector3.ProjectOnPlane(@from.forward, Vector3.down); //在水平面下下的投影

        float _Angle = Vector3.Angle(_horizontialFoward, _RelativePosition);


        // Debug.Log("_Angle=" + _Angle);

        if (_Angle < 5f) return; //必须加阈值判断 否则可能出现连续翻转180
        @from.Rotate(0, _Angle, 0, Space.Self); //有些时候需要旋转180
    }

    public static void FollowCameraHorizontal(Transform target, float distanceFromForward)
    {
        float des = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z).magnitude;
        target.position = Camera.main.transform.position + new Vector3(Camera.main.transform.forward.x / des, 0, Camera.main.transform.forward.z / des) * distanceFromForward;
        target.eulerAngles = new Vector3(target.eulerAngles.x, Camera.main.transform.eulerAngles.y, target.eulerAngles.z);
    }

    /// <summary>
    /// 在水平方向上跟随主相机 垂直方向上不跟随
    /// </summary>
    public static void FollowCameraXZ(Transform trans, float distance, Camera camera, float yPosition = 0)
    {
        float des = new Vector2(camera.transform.forward.x, camera.transform.forward.z).magnitude;
        trans.position = Camera.main.transform.position + new Vector3(camera.transform.forward.x / des, 0, camera.transform.forward.z / des) * distance + new Vector3(0, yPosition, 0);
        trans.eulerAngles = new Vector3(trans.eulerAngles.x, camera.transform.eulerAngles.y, trans.eulerAngles.z);
    }



    /// Get Trans Compont ,IF null Then Add This 
    public static T GetOrAddCompont<T>(this Transform trans) where T : Component
    {
        if (trans == null) return null;
        return GetOrAddCompont<T>(trans.gameObject);
    }
    public static T GetOrAddCompont<T>(this GameObject obj) where T : Component
    {
        if (obj == null) return null;
        T result = obj.GetComponent<T>();
        if (result == null)
            result = obj.gameObject.AddComponent<T>();
        return result;
    }

    /// <summary>
    /// Set TransForm Local  State 
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="localScale"></param>
    /// <param name="localPosition"></param>
    /// <param name="angles"></param>
    /// <param name="Parent"></param>
    public static void SetTransLocalState(this Transform trans, Vector3 localScale, Vector3 localPosition, Vector3 angles, bool isSetParent, Transform Parent = null)
    {
        if (isSetParent)
            trans.SetParent(Parent);
        trans.localScale = localScale;
        trans.localPosition = localPosition;
        trans.localEulerAngles = angles;
    }

    /// <summary>
    /// 设置同一个父节点下front 在 behind 出现在Hierachy前
    /// </summary>
    /// <param name="front">出现在前面的Trans</param>
    /// <param name="behind"></param>
    public static void SetTransBehind(Transform front, Transform behind)
    {
        if (front == null || behind == null) { Debug.LogError("SetTransBehind Fail, Is Null"); return; }
        if (front.parent != behind.parent) { Debug.LogError("SetTransBehind Fail,Not The Same Parent"); return; }
        if (front.parent == null || behind.parent == null)
        {
            behind.SetAsLastSibling();
            Debug.Log("SetTransBehind Success ,Parent is Null");
            return;
        }
        int Count = front.parent.childCount;
        bool isNeedSet = false;
        for (int dex = 0; dex < Count; ++dex)
        {
            if (front.parent.GetChild(dex) == front)
            {
                if (isNeedSet)
                    behind.SetSiblingIndex(dex);

                Debug.Log("SetTransBehind Success isNeedSet=" + isNeedSet);
                return;
            }

            if (behind.parent.GetChild(dex) == behind)
            {
                isNeedSet = true;
            }//说明在找到前面时候已经找到后面的Trans 需要设置Trans
        }
    }

    public static RectTransform Getchild_Ex(this RectTransform target, int dex)
    {
        if (target == null) return null;
        if (dex >= target.childCount)
        {
            Debug.LogError("Getchild_Ex Out Of range of " + target.childCount + "  index=" + dex);
            return null;
        }

        return target.GetChild(dex) as RectTransform;
    }

}
