using MFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace MFramework.UI.Layout
{

    public class LayoutScrollView : SmothLayoutWhithBar//, IScrollHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        //Vector2 startDragPostion = Vector2.zero;
        //[Header("OnDrag Sensitivity")]
        //[SerializeField]
        //[Range(0,10)]
        //protected float DragSensitivity = 3f; //拖拽多远开始响应
        //public void OnBeginDrag(PointerEventData eventData)
        //{
        //    if (eventData == null) return;
        //    startDragPostion = eventData.position;
        //    //  Debug.Log("Begin     " + eventData.position);
        //}

        //public void OnEndDrag(PointerEventData eventData)
        //{
        //    if (eventData == null) return;
        //    //    Debug.Log("Draging ....   " + eventData.position);

        //}

        //public void OnDrag(PointerEventData eventData)
        //{
        //    if (eventData == null) return;
        //    Vector2 DragDistance = (eventData.position - startDragPostion);
        //    if (m_HorizontalLayout)
        //    {
        //        if (Math.Abs(DragDistance.x) > DragSensitivity)
        //        {
        //            if (DragDistance.x > 0)
        //            {
        //                Scroll_OnScrollLayoutView(ScrollPageDirection.LEFT, Math.Abs(DragDistance.x));
        //            }
        //            else
        //            {
        //                Scroll_OnScrollLayoutView(ScrollPageDirection.RIGHT, Math.Abs(DragDistance.x));
        //            }
        //            startDragPostion = eventData.position;
        //        }
        //    }
        //    else
        //    {
        //        if (Math.Abs(DragDistance.y) > DragSensitivity)
        //        {
        //            if (DragDistance.y > 0)
        //            {
        //                Scroll_OnScrollLayoutView(ScrollPageDirection.DOWN, Math.Abs(DragDistance.y));
        //            }
        //            else
        //            {
        //                Scroll_OnScrollLayoutView(ScrollPageDirection.UP, Math.Abs(DragDistance.y));
        //            }
        //            startDragPostion = eventData.position;
        //        }
        //    }

        //    //         Debug.Log("End     " + eventData.position);

        //}


    }
}
