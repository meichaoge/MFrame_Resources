using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//2017/7/6 修改 ChangeScrollBarView 实现..确保在使用重置时候依旧能够正确的显示

namespace MFramework.UI.Layout
{
    public abstract class SmothLayoutWhithBar : SmothLayoutController
    {
        [Header("滑块")]
        [SerializeField]
        protected RectTransform verticalScrollBar_BG;
        [SerializeField]
        protected RectTransform verticalScrollBar;
        [SerializeField]
        protected RectTransform horizontialScrollBar_BG;
        [SerializeField]
        protected RectTransform horizontialScrollBar;

        [Header("页面提示栏")]
        public GameObject m_PageTipPrefab;
        public RectTransform m_PageTiplistTrans;


        protected int lastTimeRecordPageIndex = 0;         //上一次记录的页数

        public override void ReBuildView(int _dataCount, bool _isClearDataSource, bool _isSavingTransReference = true)
        {
            if (IsFirstTimeShow) return;
            SetScrollBarState(_dataCount);
            base.ReBuildView(_dataCount, _isClearDataSource, _isSavingTransReference);

            if (m_IsShowItemEffect == false)
            {
                CreatePageTip();         //页面提示栏功能
                ChangePageTipView();  //更新页面提示数据视图
                CheckWhetherNeedToShow();
            }
        }

        /// <summary>
        /// 设置滑动条状态
        /// </summary>
        /// <param name="_dataCount"></param>
        void SetScrollBarState(int _dataCount)
        {
            if (_dataCount < ColumnNumber * RowNumber)
            {
                if (verticalScrollBar_BG != null && verticalScrollBar_BG.gameObject.activeSelf)
                    verticalScrollBar_BG.gameObject.SetActive(false);
                if (verticalScrollBar != null && verticalScrollBar.gameObject.activeSelf)
                    verticalScrollBar.gameObject.SetActive(false);

                if (horizontialScrollBar_BG != null && horizontialScrollBar_BG.gameObject.activeSelf)
                    horizontialScrollBar_BG.gameObject.SetActive(false);
                if (horizontialScrollBar != null && horizontialScrollBar.gameObject.activeSelf)
                    horizontialScrollBar.gameObject.SetActive(false);
            }
            else
            {
                if (verticalScrollBar_BG != null && verticalScrollBar_BG.gameObject.activeSelf == false)
                    verticalScrollBar_BG.gameObject.SetActive(true);
                if (verticalScrollBar != null && verticalScrollBar.gameObject.activeSelf == false)
                    verticalScrollBar.gameObject.SetActive(true);

                if (horizontialScrollBar_BG != null && horizontialScrollBar_BG.gameObject.activeSelf == false)
                    horizontialScrollBar_BG.gameObject.SetActive(true);
                if (horizontialScrollBar != null && horizontialScrollBar.gameObject.activeSelf == false)
                    horizontialScrollBar.gameObject.SetActive(true);
            }

        }


        protected override void Reset_ClearData(bool _isSavingTransReference = true, bool _isClearDataSource = true, bool _isClearInitialmaskAndInitialCount = true)
        {
            base.Reset_ClearData(_isSavingTransReference, _isClearDataSource, _isClearInitialmaskAndInitialCount);
            if (m_PageTiplistTrans != null && m_PageTipPrefab != null)
            {
                RectTransform pageItem = null;
                for (int _index = 0; _index < m_PageTiplistTrans.childCount; ++_index)
                {  //清除页面提示的标示  必须清除
                    pageItem = m_PageTiplistTrans.Getchild_Ex(_index).Getchild_Ex(0);
                    if (pageItem.gameObject.activeSelf)
                        pageItem.gameObject.SetActive(false);
                }
            }
            lastTimeRecordPageIndex = -1;
        }



        protected override void ScrollUp()
        {
            ChangePageTipView(); //更新页面提示视图
            CheckWhetherNeedToShow();
            base.ScrollUp();
        }
        protected override void ScrollDown()
        {
            ChangePageTipView(); //更新页面提示视图
            CheckWhetherNeedToShow();
            base.ScrollDown();
        }
        protected override void ScrollLeft()
        {
            ChangePageTipView(); //更新页面提示视图
            CheckWhetherNeedToShow();
            base.ScrollLeft();
        }
        protected override void ScrollRight()
        {
            ChangePageTipView(); //更新页面提示视图
            CheckWhetherNeedToShow();
            base.ScrollRight();
        }



        public override void LayoutEffect_ItemDoLastAction(bool _isFinishAction)
        {
            CreatePageTip();         //页面提示栏功能
            ChangePageTipView();  //更新页面提示数据视图
            CheckWhetherNeedToShow();  //检查是否需要显示一些部件
            base.LayoutEffect_ItemDoLastAction(_isFinishAction);
        }



        /// <summary>
        /// 创建页面页数提示视图
        /// </summary>
        protected override void CreatePageTip()
        {
            if (m_PageTipPrefab == null || m_PageTiplistTrans == null)
                return;
            int _totalPage = (DataCount + RowNumber * ColumnNumber - 1) / (RowNumber * ColumnNumber);
            DynamicListLayoutHelper.UpdateList(m_PageTiplistTrans, m_PageTipPrefab, _totalPage);
        }
        /// <summary>
        /// 更新页面提示视图
        /// </summary>
        protected override void ChangePageTipView()
        {
            //    Debug.Log("ChangePageTipView " + gameObject.name);
            if (m_PageTipPrefab == null || m_PageTiplistTrans == null)
                return;
            int _currentPageIndex = (m_InitialItemCount + RowNumber * ColumnNumber - 1) / (RowNumber * ColumnNumber);
            //     Debug.Log(gameObject.name+"_currentPageIndex=" + _currentPageIndex+ ":::lastTimeRecordPageIndex= "+ lastTimeRecordPageIndex);
            if (lastTimeRecordPageIndex != _currentPageIndex)
            {
                if (lastTimeRecordPageIndex > 0) //这里必须有判断 否则在两个页面之间切换时会出现空引用
                {//第一次改变时lastTimeRecordPageIndex=-1,
                    m_PageTiplistTrans.GetChild(lastTimeRecordPageIndex - 1).GetChild(0).gameObject.SetActive(false);

                }
                if (_currentPageIndex > 0)
                {
                    m_PageTiplistTrans.GetChild(_currentPageIndex - 1).GetChild(0).gameObject.SetActive(true);
                }
                lastTimeRecordPageIndex = _currentPageIndex; //更新标志位
            }
            else
            {
                //由于临时方案是重新创建 所以有这个
                if (_currentPageIndex > 0)
                {
                    m_PageTiplistTrans.GetChild(_currentPageIndex - 1).GetChild(0).gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// 检查面板部分按钮等是否需要显示
        /// </summary>
        protected override void CheckWhetherNeedToShow()
        {
            ChangeScrollBarView();
            if (isHideViewWhenNotFill == false) return; //此时不需要检测
                                                        //       Debug.Log("CheckWhetherNeedToShow=");
            if (m_PageScrollButtonTrans_N)
            {
                if (m_InitialItemCount > RowNumber * ColumnNumber)
                { 
                    m_PageScrollButtonTrans_N.gameObject.SetActive(true);
                }
                else
                {
                    m_PageScrollButtonTrans_N.gameObject.SetActive(false);
                }
            }//if
            if (m_PageScrollButtonTrans_S)
            {
                if (m_InitialItemCount >= RowNumber * ColumnNumber && m_InitialItemCount < DataCount)
                {
                    m_PageScrollButtonTrans_S.gameObject.SetActive(true);
                }
                else
                {
                    m_PageScrollButtonTrans_S.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 改变滚动条大小以及滑块的位置
        /// </summary>
        void ChangeScrollBarView()
        {
            int _totalPageCount = (DataCount + RowNumber * ColumnNumber - 1) / (RowNumber * ColumnNumber); //总页数
            if (m_HorizontalLayout)
            {//水平
                #region 水平布局
                if (horizontialScrollBar_BG == null || horizontialScrollBar == null|| horizontialScrollBar.gameObject.activeSelf==false) return;
                if (DataCount <= RowNumber * ColumnNumber)
                { //只有一页数据
                    horizontialScrollBar.sizeDelta = horizontialScrollBar_BG.sizeDelta;
                    horizontialScrollBar.localPosition = Vector3.zero;
                }
                else
                {
                    int needMoveColumnCount = Mathf.Max(0, (m_InitialItemCount - RowNumber * ColumnNumber + RowNumber - 1) / RowNumber); //需要滑动的列数
                    int _totalColumnCount = (DataCount + RowNumber - 1) / RowNumber;  //滑行区域总列数
                    float _scrollRectLength = horizontialScrollBar_BG.sizeDelta.x - horizontialScrollBar.sizeDelta.x;  //滑行区域长度
                    horizontialScrollBar.sizeDelta = new Vector2(horizontialScrollBar_BG.sizeDelta.x * ColumnNumber / _totalColumnCount, horizontialScrollBar_BG.sizeDelta.y);
                    horizontialScrollBar.localPosition = new Vector3(1f * needMoveColumnCount / (_totalColumnCount - ColumnNumber) * _scrollRectLength, 0, 0f);
                }
                #endregion
                return;
            }

            #region 垂直方向布局
            if (verticalScrollBar_BG == null || verticalScrollBar == null || verticalScrollBar.gameObject.activeSelf == false) return;
            if (DataCount <= RowNumber * ColumnNumber)
            { //只有一页数据
              //  Debug.Log("只有一页数据");
                verticalScrollBar.sizeDelta = verticalScrollBar_BG.sizeDelta;
                verticalScrollBar.localPosition = Vector3.zero;
            }
            else
            {
                int curMoveRowCount = Mathf.Max(0, (m_InitialItemCount - RowNumber * ColumnNumber + ColumnNumber - 1) / ColumnNumber); //需要滑动的行数
                int _totalRowCount = (DataCount + ColumnNumber - 1) / ColumnNumber;  //滑行区域总行数
                float _scrollRectLength = verticalScrollBar_BG.sizeDelta.y - verticalScrollBar.sizeDelta.y;  //滑行区域长度
                verticalScrollBar.sizeDelta = new Vector2(verticalScrollBar_BG.sizeDelta.x, verticalScrollBar_BG.sizeDelta.y * RowNumber / _totalRowCount);
                verticalScrollBar.localPosition = new Vector3(0, -1f * curMoveRowCount / (_totalRowCount - RowNumber) * _scrollRectLength, 0f);
            }

            #endregion
        }


    }
}
