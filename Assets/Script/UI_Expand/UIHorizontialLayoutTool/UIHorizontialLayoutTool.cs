using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StoneSkin.GUI
{
    /// <summary>
    /// 水平方向上的布局 会自动对齐到整数的节点上 对ScrollRect 的一种扩展吧
    /// </summary>
    public class UIHorizontialLayoutTool : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        //2018/1/15新增 方便新手引导时禁止滑动
        public bool m_IsEnableTool = true;  //当前脚本的拖动效果是否可用

        public RectTransform m_ItemTrans;
        public RectTransform m_ContentRectrans;   //ContentRectrans

        private Tweener m_CurrentTweenner;

        [Range(-200, 200)]
        public int m_ItemSpace = 5;  //列表项之间的间距
        [Range(1f, 200f)]
        public float m_DragingSpeedRadiu = 5;  //拖拽时候的速度比例
        [Range(1f, 200f)]
        public float m_DampingSpeedRadiu = 5;  //阻尼时候的速度比例
        [Header("阻尼加速度")]
        [Range(1, 300)]
        public float m_DampAcceleration = 120; //阻尼加速度
        [Header("阻尼运动时候当小于等于这个速度 就开启自动对齐")]
        [Range(1, 20)]
        public float m_DampMinSpeed = 1; //当速度小于这个值的时候DOTween

        [Header("当卡牌滑动到左右边界时候最远偏移多远弹回来")]
        [Range(100f, 1000)]
        public float m_MaxDistanceOffsetBoundary = 5;

        [Header("从边界点弹回来的速度")]
        [Range(100, 2000)]
        public float m_DampingBackSpeed = 1000;
        [Header("中间卡牌的缩放比例")]
        [Range(1f, 2f)]
        public float m_MaxScaleMiddle = 1.5f;
        [Header("卡牌厨师时候的正常缩放比例")]
        [Range(0.1f, 5f)]
        public float m_ItemInitialedScale = 0.8f;

        [Header("从中间元素开始第几个元素回复正常大小")]
        [Range(1, 100)]
        public int m_EffectItem = 2;

        #region 状态
        private bool m_IsLeftRotate = false; //是否左滑动
        private bool m_IsInitialed = false;  //是否初始化完成

        private bool m_IsDraging = false;
        private bool m_IsFinishDraging = false;  //是否完成了拖拽可以点击  EndDraging 时候为True

        private bool m_IsSelectMoving = false;  //点击选中一个项的移动中

        private bool m_IsDamping = false;  //拖拽后的阻尼运动
        private bool m_IsDampingTweenMoving = false;  //阻尼运动Tween 中
        #endregion

        #region 数据
        private float m_CurrentRotateSpeed = 0; //当前的速度
        private float m_PreviousMousPosX = 0;  //鼠标前一帧的位置
        private float m_CurrentMousPosX = 0;  //鼠标每一帧的位置

        private int m_ItemsCount = 5;  //列表项个数
        //[SerializeField]
        private float m_ItemsDistance = 0;  //任意两个项之间的距离
        private float m_AnchorPosX = 0;  //m_ContentRectrans 应该出现的坐标X
        private float m_MaxDistanceX;  //最远的坐标位置

        private List<RectTransform> m_AllItemsData = new List<RectTransform>();

        #endregion

        public System.Action OnRebuildLayoutViewAct = null;  //重建事件

        #region  事件处理
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (m_IsEnableTool == false) return;

            if (m_IsInitialed == false) return;
            m_IsFinishDraging = false;
            m_PreviousMousPosX = m_CurrentMousPosX = eventData.position.x;
            if (m_IsSelectMoving)
            {
                m_IsDraging = m_IsDamping = false;
                return;
            }
            m_IsDraging = true;
            m_IsDamping = false;
            m_CurrentRotateSpeed = 0;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (m_IsEnableTool == false) return;

            if (m_IsInitialed == false) return;
            if (m_IsDampingTweenMoving) return;
            if (m_IsSelectMoving) return;
            if (m_IsDamping) return;
            m_CurrentMousPosX = eventData.position.x;
            if (eventData.delta.x < 0)
            {
                m_IsLeftRotate = true;
            }//左移动
            else if (eventData.delta.x > 0)
            {
                m_IsLeftRotate = false;
            }//右边移动

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (m_IsEnableTool == false) return;

            if (m_IsInitialed == false) return;
            m_IsFinishDraging = true;
            m_IsDraging = false;
            if (m_IsSelectMoving) return;
            if ((m_ContentRectrans.localPosition.x <= 0 || m_ContentRectrans.localPosition.x >= -1 * m_ItemsDistance * (m_ItemsCount - 1)) && Mathf.Abs(m_ContentRectrans.localPosition.x) % m_ItemsDistance <= 1f)
            {
                //Debug.LogInfor("OnEndDrag 已经对齐了");
                return;  //已经对齐
            }

            m_CurrentRotateSpeed = m_DampingSpeedRadiu * Mathf.Abs(eventData.position.x - m_PreviousMousPosX);
            m_PreviousMousPosX = m_CurrentMousPosX = eventData.position.x;
            if (m_CurrentRotateSpeed == 0)
            {
                OnDampAdsorbentMoving();
                return;
            } //自动吸附
            m_IsDamping = true;  //开启阻尼运动
        }


        #endregion

        #region  MonoFrame
        private void Update()
        {
            if (m_IsEnableTool == false) return;
            if (m_IsInitialed == false) return;
            if (m_IsDraging)
            {
                DoDragingMove();
            }
            if (m_IsDamping)
            {
                DoDampMoving();
            }
        }

        private void OnDisable()
        {
            m_IsEnableTool = true;
            m_CurrentTweenner.Complete();
        }

        #endregion


        public void InitialLayoutView(int dataCount, System.Action<GameObject, int> OnItemCreateCallback)
        {
            if (m_IsInitialed)
            {
                //   Debug.LogError("InitialLayoutView Fail,Already Initialed");
                ResetStateForRebuild();

                if (OnRebuildLayoutViewAct != null)
                    OnRebuildLayoutViewAct();
                // return;
            }
            m_ItemsCount = dataCount;
            m_IsInitialed = true;
            m_ItemsDistance = m_ItemSpace + m_ItemTrans.sizeDelta.x;
            m_AllItemsData.Clear();
            m_MaxDistanceX = -1 * m_ItemsDistance * (m_ItemsCount - 1) - m_MaxDistanceOffsetBoundary;// 最远滑动到的坐标位置
            for (int dex = 0; dex < m_ItemsCount; ++dex)
            {
                GameObject goItem = GameObject.Instantiate<GameObject>(m_ItemTrans.gameObject);
                goItem.name = "items " + dex;
                goItem.transform.SetParent(m_ContentRectrans);
                goItem.transform.localScale = Vector3.one;
                goItem.transform.localRotation = Quaternion.identity;
                goItem.transform.localPosition = new Vector3(dex * (m_ItemSpace + m_ItemTrans.sizeDelta.x), 0, 0);
                m_AllItemsData.Add(goItem.transform as RectTransform);
                if (OnItemCreateCallback != null)
                    OnItemCreateCallback(goItem, dex);
            }

            OnItemMoveing();
        }

        /// <summary>
        /// 点击列表项 
        /// </summary>
        /// <param name="item">列表项</param>
        /// <param name="souroucesTarget">触发事件的事件源</param>
        public void OnItemClick(UILayoutItem item, Transform souroucesTarget)
        {
            if (m_IsDraging || m_IsDamping)
                return;
            if (m_IsSelectMoving) return;
            m_IsSelectMoving = true;
            if (m_ContentRectrans.anchoredPosition.x != -1 * item.m_ItemIndex * m_ItemsDistance)
            {
                float tweenTime = Mathf.Abs(Mathf.Abs(m_ContentRectrans.anchoredPosition.x) - item.m_ItemIndex * m_ItemsDistance) / m_DampingBackSpeed;
                Debug.Log("OnItemClick  tweenTime=" + tweenTime);
                m_CurrentTweenner = m_ContentRectrans.DOAnchorPos(new Vector2(-1 * item.m_ItemIndex * m_ItemsDistance, 0), tweenTime).
                    OnComplete(() =>
                    {
                        m_IsSelectMoving = false;
                        item.OnAfterItemClick(souroucesTarget);
                    }).OnUpdate(OnItemMoveing);
                return;
            }
            item.OnAfterItemClick(souroucesTarget);
            m_IsSelectMoving = false;
        }

        //为重建视图准备和重置数据和状态
        private void ResetStateForRebuild()
        {
            for (int i = 0; i < m_ContentRectrans.childCount; i++)
            {
                GameObject.Destroy(m_ContentRectrans.GetChild(i).gameObject);
            }

            m_ContentRectrans.anchoredPosition = Vector2.zero;
            m_PreviousMousPosX = m_CurrentMousPosX = 0;
            m_CurrentRotateSpeed = 0;

            m_IsDraging = m_IsDamping = m_IsSelectMoving = false;
        }

        //正常拖动
        void DoDragingMove()
        {
            m_CurrentRotateSpeed = m_DragingSpeedRadiu * Mathf.Abs(m_CurrentMousPosX - m_PreviousMousPosX);
            m_PreviousMousPosX = m_CurrentMousPosX;
            if (m_IsLeftRotate)
            {
                if (m_ContentRectrans.anchoredPosition.x <= m_MaxDistanceX)
                    return;

                if (m_ContentRectrans.anchoredPosition.x - m_CurrentRotateSpeed * Time.deltaTime < m_MaxDistanceX)
                    m_ContentRectrans.anchoredPosition = new Vector2(m_MaxDistanceX, 0);
                else
                    m_ContentRectrans.anchoredPosition -= new Vector2(m_CurrentRotateSpeed * Time.deltaTime, 0);
            }
            else
            {
                if (m_ContentRectrans.anchoredPosition.x >= m_MaxDistanceOffsetBoundary)
                    return;
                if (m_ContentRectrans.anchoredPosition.x + m_CurrentRotateSpeed * Time.deltaTime > m_MaxDistanceOffsetBoundary)
                    m_ContentRectrans.anchoredPosition = new Vector2(m_MaxDistanceOffsetBoundary, 0);
                else
                    m_ContentRectrans.anchoredPosition += new Vector2(m_CurrentRotateSpeed * Time.deltaTime, 0);
            }
            //Debug.Log("DoDragingMove   " + m_ContentRectrans.anchoredPosition);
            OnItemMoveing();
        }

        //阻尼运动
        void DoDampMoving()
        {
            if (m_IsDampingTweenMoving) return;
            m_IsDampingTweenMoving = true;

            if (m_IsLeftRotate)
            {
                #region 向左滚动
                m_CurrentRotateSpeed -= m_DampAcceleration * Time.deltaTime;
                if (m_CurrentRotateSpeed < 0)
                    m_CurrentRotateSpeed = Mathf.Abs(m_CurrentRotateSpeed);
                //Debug.Log("DoDampMoving1..." + m_CurrentRotateSpeed + "  m_DempAcceleration=" + m_DampAcceleration + "  ::" + Time.deltaTime);

                m_AnchorPosX = m_ContentRectrans.anchoredPosition.x - m_CurrentRotateSpeed * Time.deltaTime;
                if (OnDampingBeyondBundary())
                    return;

                if (Mathf.Abs(m_CurrentRotateSpeed) <= m_DampMinSpeed)
                {
                   //int dex = Mathf.RoundToInt((Mathf.Abs(m_ContentRectrans.anchoredPosition.x - m_ItemsDistance) / m_ItemsDistance));
                   // dex = Mathf.Clamp(dex, 0, m_ItemsCount - 1);
                    //    Debug.Log("DoDampMoving 向左滚动 002  自动吸附 dex=" + dex + "    m_ItemsCount=" + m_ItemsCount);
                    m_CurrentTweenner = m_ContentRectrans.DOAnchorPos(new Vector2(-1 * GetAdsorbentIndex() * m_ItemsDistance, 0), 0.3f).OnComplete(() =>
                     {
                         m_IsDamping = false;
                         m_IsDampingTweenMoving = false;
                     }).OnUpdate(OnItemMoveing);
                    return;
                }

                #endregion
            }
            else
            {
                #region 向右滚动
                m_CurrentRotateSpeed += m_DampAcceleration * Time.deltaTime;  //m_CurrentRotateSpeed 是负数
                if (m_CurrentRotateSpeed > 0)
                    m_CurrentRotateSpeed = -1 * Mathf.Abs(m_CurrentRotateSpeed);

                //   Debug.Log("DoDampMoving2 ..." + m_CurrentRotateSpeed + "  m_DempAcceleration=" + m_DampAcceleration + "  ::" + Time.deltaTime);
                m_AnchorPosX = m_ContentRectrans.anchoredPosition.x - m_CurrentRotateSpeed * Time.deltaTime;
                if (OnDampingBeyondBundary())
                    return;

                if (Mathf.Abs(m_CurrentRotateSpeed) <= m_DampMinSpeed)
                {
                    // Debug.Log("m_ContentRectrans.anchoredPosition.x+=" + m_ContentRectrans.anchoredPosition.x);
                    //Debug.Log("xx=" + Mathf.Abs(m_ContentRectrans.anchoredPosition.x + m_ItemsDistance ) / m_ItemsDistance);
                    //    int dex = Mathf.RoundToInt(Mathf.Abs(m_ContentRectrans.anchoredPosition.x + m_ItemsDistance) / m_ItemsDistance);
                    // Debug.Log("DoDampMoving 向右滚动 002 自动吸附 dex= " + dex);
                    //dex = Mathf.Clamp(dex, 0, m_ItemsCount - 1);
                    //dex = -1 * dex;

                    m_CurrentTweenner = m_ContentRectrans.DOAnchorPos(new Vector2(-1 * GetAdsorbentIndex() * m_ItemsDistance, 0), 0.3f).OnComplete(() =>
                   {
                       m_IsDamping = false;
                       m_IsDampingTweenMoving = false;
                   }).OnUpdate(OnItemMoveing);
                    return;
                }

                #endregion
            }
            m_ContentRectrans.anchoredPosition = new Vector2(m_AnchorPosX, 0);
            //Debug.Log("DoDampMoving   " + m_ContentRectrans.anchoredPosition + "             m_AnchorPosX=" + m_AnchorPosX + "m_CurrentRotateSpeed  =" + m_CurrentRotateSpeed);

            m_IsDampingTweenMoving = false;
            OnItemMoveing();
        }

        /// <summary>
        /// 获取吸附的索引位置
        /// </summary>
        /// <returns></returns>
        private int GetAdsorbentIndex()
        {
            int dex = (int)((Mathf.Abs(m_ContentRectrans.anchoredPosition.x) + m_ItemsDistance * 0.5f)/ m_ItemsDistance);
            dex = Mathf.Clamp(dex, 0, m_ItemsCount - 1);
            return dex;
        }


        //当阻尼运动超过边界时候的处理
        private bool OnDampingBeyondBundary()
        {
            if (m_ContentRectrans.anchoredPosition.x >= m_MaxDistanceOffsetBoundary)
            {
                //      Debug.Log("OnDampingBeyondBundary 超过右边界 001 ");
                m_CurrentTweenner = m_ContentRectrans.DOAnchorPos(Vector2.zero, Mathf.Abs(m_ContentRectrans.anchoredPosition.x) / m_DampingBackSpeed).OnComplete(() =>
                {
                    m_IsDamping = false;
                    m_IsDampingTweenMoving = false;
                }).OnUpdate(OnItemMoveing);
                return true;
            }

            if (m_ContentRectrans.anchoredPosition.x <= -1 * m_ItemsDistance * (m_ItemsCount - 1) - m_MaxDistanceOffsetBoundary)
            {
                //      Debug.Log("OnDampingBeyondBundary 超过左边界");
                float tweenTime = (Mathf.Abs(m_ContentRectrans.anchoredPosition.x) - m_ItemsDistance * (m_ItemsCount - 1)) / m_DampingBackSpeed;
                m_CurrentTweenner = m_ContentRectrans.DOAnchorPos(new Vector2(-1 * (m_ItemsCount - 1) * m_ItemsDistance, 0), tweenTime).OnComplete(() =>
                {
                    m_IsDamping = false;
                    m_IsDampingTweenMoving = false;
                }).OnUpdate(OnItemMoveing);
                return true;
            }
            return false;
        }

        //阻尼运动自动吸附到整节点位置
        void OnDampAdsorbentMoving()
        {
            if (m_IsDampingTweenMoving) return;
            m_IsDampingTweenMoving = true;
            int dex = 0;
            if (m_ContentRectrans.anchoredPosition.x >= 0)
            {
                //    Debug.Log("OnDampAdsorbentMoving  右边界 " + m_ContentRectrans.anchoredPosition.x);
                dex = 0;
            }  //右边界
            else if (m_ContentRectrans.anchoredPosition.x <= -1 * m_ItemsDistance * (m_ItemsCount - 1))
            {
                //      Debug.Log("OnDampAdsorbentMoving  左边界 " + m_ContentRectrans.anchoredPosition.x);
                dex = -1 * (m_ItemsCount - 1);
            } //左边界
            else
            {
                //if (m_IsLeftRotate)
                //    dex = Mathf.RoundToInt(Mathf.Abs(m_ContentRectrans.anchoredPosition.x - m_ItemsDistance) / m_ItemsDistance);
                //else
                //{
                //    //    float x = Mathf.Abs(m_ContentRectrans.anchoredPosition.x + m_ItemsDistance );
                //    //     Debug.Log("xx=" + x);
                //    //  Debug.Log("  " + x / m_ItemsDistance);
                //    dex = Mathf.RoundToInt((Mathf.Abs(m_ContentRectrans.anchoredPosition.x + m_ItemsDistance) / m_ItemsDistance));
                //}

                //Debug.Log("OnDampAdsorbentMoving 其他 " + m_ContentRectrans.anchoredPosition.x + "   dex=" + dex);
                //dex = Mathf.Clamp(dex, 0, m_ItemsCount - 1);
                ////     Debug.Log("dex=" + dex);
                //dex = -1 * dex;

                dex = -1 * GetAdsorbentIndex();
            }

            m_CurrentTweenner = m_ContentRectrans.DOAnchorPos(new Vector2(dex * m_ItemsDistance, 0), 0.3f).OnComplete(() =>
            {
                m_IsDamping = false;
                m_IsDampingTweenMoving = false;
            }).OnUpdate(OnItemMoveing);

            //    Debug.Log("OnDampAdsorbentMoving  dex=  " + dex+ "   m_ContentRectrans.anchoredPosition.x="+ m_ContentRectrans.anchoredPosition.x);
        }

        private float m_MinOffsetX = 0;
        private int m_SelectIndex = -1;

        /// <summary>
        /// 当滑动时候的视图项操作
        /// </summary>
        private void OnItemMoveing()
        {
            m_MinOffsetX = -100;
            m_SelectIndex = -1;
            //    Debug.Log("OnItemMoveing>>");
            for (int dex = 0; dex < m_AllItemsData.Count; ++dex)
            {
                float offsetX = Mathf.Abs(m_AllItemsData[dex].localPosition.x + m_AllItemsData[dex].transform.parent.localPosition.x);
                if (offsetX >= m_ItemsDistance * m_EffectItem)
                {
                    m_AllItemsData[dex].localScale = Vector3.one * m_ItemInitialedScale;
                    //Debug.Log("OnItemMoveing  " + "  offsetX=" + offsetX);
                    continue;
                }
                else
                {
                    if (m_MinOffsetX == -100)
                        m_MinOffsetX = offsetX;

                    float scale = Mathf.Abs((offsetX - m_ItemsDistance * m_EffectItem) / (m_ItemsDistance * m_EffectItem));
                    float scale2 = Mathf.Lerp(1, m_MaxScaleMiddle, scale);
                    m_AllItemsData[dex].localScale = Vector3.one * scale2 * m_ItemInitialedScale;
                    //    Debug.Log("OnItemMoveing  " + m_AllItemsData[dex].gameObject.name + "    offsetX=" + offsetX + "   scale=  " + scale + "    scale2=" + scale2);
                    if (offsetX <= m_MinOffsetX)
                    {
                        //      Debug.LogInfor("offsetX=" + offsetX + "  m_MinOffsetX= " + m_MinOffsetX + "  dex=" + dex);
                        m_MinOffsetX = offsetX;
                        m_SelectIndex = dex;
                    }
                }
            }

            if (m_SelectIndex != -1)
            {
                //  Debug.LogInfor("m_SelectIndex=" + m_SelectIndex + "   Name=" + m_AllItemsData[m_SelectIndex].gameObject.name);
                m_AllItemsData[m_SelectIndex].transform.SetAsLastSibling();
                m_AllItemsData[m_SelectIndex].GetComponent<UILayoutItem>().OnLayoutItemIsFocus();
            }
            //else
            //{
            //    Debug.LogError("OnItemMoveing Fail m_SelectIndex=-1 ");
            //}
        }


        public void ForceAutoSelectItem(int dex)
        {
            UILayoutItem item = m_ContentRectrans.GetChild(dex).GetComponent<UILayoutItem>();
            OnItemClick(item, item.transform);
        }




    }
}