using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    /// <summary>
    /// 对于每一个子项都是一样大小的带有自动吸附功能
    /// </summary>
    public abstract class LoopScrollRectAutoAdsorbent : LoopScrollRect
    {
        [Header("自动吸附到某个节点上")]
        public bool m_IsAutoAdsorbent = false;

        [Header("当速度小于这个值的时候 自动吸附到某个节点上")]
        [Range(2, 100)]
        public float m_MinVelocityToAdsorbent = 5;

        //惯性运动
        protected override void DampingMoving()
        {
            if (m_IsAutoAdsorbent == false)
            {
                base.DampingMoving();
                return;
            }
            if (Mathf.Abs(velocity.y) > m_MinVelocityToAdsorbent)
            {
                base.DampingMoving();
                return;
            }

            if (content.childCount < 1) return;
            RectTransform item = content.GetChild(1) as RectTransform;
            float size = GetSize(item);
            int dex = (int)((Mathf.Abs(content.anchoredPosition.y) + size / 2f) / size);
            dex = Mathf.Clamp(dex, 0, content.childCount - 1);
            int i = 1;
            if (content.anchoredPosition.y < 0)
                i = -1;
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, i * dex * size);
            StopMovement();
            //Debug.Log("End" + Mathf.Abs(velocity.y)+ "       dex="+ dex);
            //    if (Mathf.Abs(velocity.y) == 0)
            //m_NeedUpdate = false;
        }


    }
}