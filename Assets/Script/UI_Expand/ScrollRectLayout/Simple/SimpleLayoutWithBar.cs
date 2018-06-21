//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//namespace MFramework.UI.Layout
//{

//    public abstract class SimpleLayoutWithBar : SimpleLayoutController
//    {
//        #region 滑块 模块
//        [SerializeField]
//        protected RectTransform verticalScrollBar_BG;
//        [SerializeField]
//        protected RectTransform verticalScrollBar;
//        [SerializeField]
//        protected RectTransform horizontialScrollBar_BG;
//        [SerializeField]
//        protected RectTransform horizontialScrollBar;

//        #endregion

//        public GameObject m_PageTipPrefab;
//        public Transform m_PageTiplistTrans;


//        protected int lastTimeItemCount = 0;  //记录上一次元素的个数便于判断是否有添加或者删除
//        protected int lastTimeInitialCount = 0;   //记录上一次初始化元素的个数 便于判断滑动的方向
//        protected int lastTimeRecordPageIndex = 0;         //上一次记录的页数

//        public override void ReBuildView(int _dataCount, bool _isClearDataSource, bool _isSavingTransReference = true)
//        {
//            base.ReBuildView(_dataCount, _isClearDataSource, _isSavingTransReference);
//            if (IsFirstTimeShow == false)
//            {
//                if (m_IsShowItemEffect == false)
//                {
//                    CreatePageTip();         //页面提示栏功能
//                    ChangePageTipView();  //更新页面提示数据视图
//                    CheckWhetherNeedToShow();
//                }
//            }
//        }

//        protected override void Reset_ClearData(bool _isSavingTransReference = true, bool _isClearDataSource = true, bool _isClearInitialmaskAndInitialCount = true)
//        {
//            base.Reset_ClearData(_isSavingTransReference, _isClearDataSource, _isClearInitialmaskAndInitialCount);
//            if (m_PageTiplistTrans != null && m_PageTipPrefab != null)
//            {
//                for (int _index = 0; _index < m_PageTiplistTrans.childCount; ++_index)
//                {  //清除页面提示的标示  必须清除
//                    m_PageTiplistTrans.GetChild(_index).GetChild(0).gameObject.SetActive(false);
//                }
//            }
//            lastTimeRecordPageIndex = -1;
//            lastTimeItemCount = 0;
//            lastTimeInitialCount = 0;  //重置滚动条记录标志
//        }



//        /// <summary>
//        /// 创建页面页数提示视图
//        /// </summary>
//        protected override void CreatePageTip()
//        {
//            if (m_PageTipPrefab == null || m_PageTiplistTrans == null)
//                return;
//            int _totalPage = (DataCount + RowNumber * ColumnNumber - 1) / (RowNumber * ColumnNumber);
//            DynamicListLayoutHelper.UpdateList(m_PageTiplistTrans, m_PageTipPrefab, _totalPage);
//        }
//        /// <summary>
//        /// 更新页面提示视图
//        /// </summary>
//        protected override void ChangePageTipView()
//        {
//            //    Debug.Log("ChangePageTipView " + gameObject.name);
//            if (m_PageTipPrefab == null || m_PageTiplistTrans == null)
//                return;
//            int _currentPageIndex = (m_InitialItemCount + RowNumber * ColumnNumber - 1) / (RowNumber * ColumnNumber);
//            if (lastTimeRecordPageIndex != _currentPageIndex)
//            {
//                if (lastTimeRecordPageIndex > 0) //这里必须有判断 否则在两个页面之间切换时会出现空引用
//                {//第一次改变时lastTimeRecordPageIndex=-1,
//                    m_PageTiplistTrans.GetChild(lastTimeRecordPageIndex - 1).GetChild(0).gameObject.SetActive(false);
//                }
//                if (_currentPageIndex > 0)
//                {
//                    m_PageTiplistTrans.GetChild(_currentPageIndex - 1).GetChild(0).gameObject.SetActive(true);

//                }
//                lastTimeRecordPageIndex = _currentPageIndex; //更新标志位
//            }
//            else
//            {
//                //由于临时方案是重新创建 所以有这个
//                if (_currentPageIndex > 0)
//                {
//                    m_PageTiplistTrans.GetChild(_currentPageIndex - 1).GetChild(0).gameObject.SetActive(true);
//                }
//            }
//        }

//        /// <summary>
//        /// 检查面板部分按钮等是否需要显示
//        /// </summary>
//        protected override void CheckWhetherNeedToShow()
//        {
//            ChangeScrollBarView();
//            if (isHideViewWhenNotFill == false) return; //此时不需要检测
//                                                        //       Debug.Log("CheckWhetherNeedToShow=");
//            if (m_PageScrollButtonTrans_N)
//            {
//                if (m_InitialItemCount > RowNumber * ColumnNumber)
//                {
//                    m_PageScrollButtonTrans_N.gameObject.SetActive(true);
//                }
//                else
//                {
//                    m_PageScrollButtonTrans_N.gameObject.SetActive(false);
//                }
//            }//if
//            if (m_PageScrollButtonTrans_S)
//            {
//                if (m_InitialItemCount >= RowNumber * ColumnNumber && m_InitialItemCount < DataCount)
//                {
//                    m_PageScrollButtonTrans_S.gameObject.SetActive(true);
//                }
//                else
//                {
//                    m_PageScrollButtonTrans_S.gameObject.SetActive(false);

//                }
//            }
//        }

//        /// <summary>
//        /// 改变滚动条大小以及滑块的位置
//        /// </summary>
//        protected  void ChangeScrollBarView()
//        {
//            int _totalPageCount = (DataCount + RowNumber * ColumnNumber - 1) / (RowNumber * ColumnNumber); //总页数
//            if (m_HorizontalLayout)
//            {//水平
//                #region 水平布局
//                if (horizontialScrollBar_BG == null || horizontialScrollBar == null) return;
//                if (DataCount <= RowNumber * ColumnNumber)
//                { //只有一页数据
//                    horizontialScrollBar.sizeDelta = horizontialScrollBar_BG.sizeDelta;
//                    horizontialScrollBar.localPosition = Vector3.zero;
//                }
//                else
//                {
//                    if (lastTimeItemCount != DataCount)
//                    {//添加或者删除了数据则置顶
//                        horizontialScrollBar.sizeDelta = new Vector2(horizontialScrollBar_BG.sizeDelta.x / _totalPageCount, horizontialScrollBar_BG.sizeDelta.y);
//                        horizontialScrollBar.localPosition = Vector3.zero;
//                    }
//                    else
//                    {
//                        if (m_InitialItemCount == lastTimeInitialCount)
//                        {
//                            return;
//                        } //没有滑行不改变位置
//                        float _scrollRectLength = horizontialScrollBar_BG.sizeDelta.x - horizontialScrollBar.sizeDelta.x;  //滑行区域长度
//                        int _totalColumn = (DataCount + RowNumber - 1) / RowNumber; //总列数
//                        if (m_InitialItemCount > lastTimeInitialCount)
//                        { //说明向右滑动
//                            horizontialScrollBar.localPosition += new Vector3(1f * m_ScrollSpeed / (_totalColumn - ColumnNumber) * _scrollRectLength, 0, 0);
//                        }
//                        else
//                        {
//                            horizontialScrollBar.localPosition -= new Vector3(1f * m_ScrollSpeed / (_totalColumn - ColumnNumber) * _scrollRectLength, 0, 0);
//                        }//else
//                    }//esle
//                }
//                #endregion
//            }
//            else
//            {
//                //    Debug.Log(gameObject.name+"_totalPageCount=" + _totalPageCount + "lastTimeItemCount=" + lastTimeItemCount + "m_InitialItemCount=" + m_InitialItemCount);
//                #region 垂直方向布局
//                if (verticalScrollBar_BG == null || verticalScrollBar == null) return;
//                if (DataCount <= RowNumber * ColumnNumber)
//                { //只有一页数据
//                  //  Debug.Log("只有一页数据");
//                    verticalScrollBar.sizeDelta = verticalScrollBar_BG.sizeDelta;
//                    verticalScrollBar.localPosition = Vector3.zero;
//                }
//                else
//                {
//                    if (lastTimeItemCount != DataCount)
//                    { //添加或者删除了数据则置顶
//                      //      Debug.Log("增加或者删除了项");
//                        verticalScrollBar.sizeDelta = new Vector2(verticalScrollBar_BG.sizeDelta.x, verticalScrollBar_BG.sizeDelta.y / _totalPageCount);
//                        verticalScrollBar.localPosition = Vector3.zero;
//                    }
//                    else
//                    {
//                        if (m_InitialItemCount == lastTimeInitialCount)
//                        {
//                            //         Debug.Log("没有滑行不改变位置");
//                            return;
//                        } //没有滑行不改变位置
//                        float _scrollRectLength = verticalScrollBar_BG.sizeDelta.y - verticalScrollBar.sizeDelta.y;  //滑行区域长度
//                        int _totalRow = (DataCount + ColumnNumber - 1) / ColumnNumber;  //滑行区域总行数
//                                                                                        //       Debug.Log("_scrollRectLength=" + _scrollRectLength + "_totalRow="+ _totalRow);
//                        if (m_InitialItemCount > lastTimeInitialCount)
//                        { //说明向下滑动      由于滑块滑动范围需要考虑滑块相对大小 所以需要减去RowNumber
//                          // Debug.Log("AAAAAAAAAAAAAA=" + 1f * m_ScrollSpeed / (_totalRow - RowNumber) * _scrollRectLength);
//                            verticalScrollBar.localPosition -= new Vector3(0, 1f * m_ScrollSpeed / (_totalRow - RowNumber) * _scrollRectLength, 0f);
//                        }
//                        else
//                        {
//                            verticalScrollBar.localPosition += new Vector3(0, 1f * m_ScrollSpeed / (_totalRow - RowNumber) * _scrollRectLength, 0f);
//                        }//else
//                    }//else
//                }

//                #endregion
//            }
//            lastTimeInitialCount = m_InitialItemCount;  //更新计数
//            lastTimeItemCount = DataCount;
//        }

//    }
//}
