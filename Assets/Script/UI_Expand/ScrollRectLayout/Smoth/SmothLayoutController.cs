using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

//2017/7/6 修复了在ShowLayoutWithSpecialRowOrColumn后向上或者向左滑动时候出现异常问题，添加标示m_JustFinishShowLayoutWithSpecialRowOrColumn
//使得重置后再下一次滑动时候又一次机会进行一些设置
//2016/12/14 修改了速度限制方式  改成使用data.position 限制
namespace MFramework.UI.Layout
{
    /// <summary>
    /// 平滑滑动的列表布局
    /// </summary>
    public abstract class SmothLayoutController : BaseLayoutController, IScrollHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [Range(1,500)]
        [SerializeField]
        protected float m_DempAcceleration = 150f;
        protected int moveCount = 0;  //划过的整行或者列的数目
        protected bool isInitialed = false;

        protected ScrollPageDirection pageDirection = ScrollPageDirection.None;  //滑动方向
        protected Vector2 lastTimeRecordPosition;  //上一次记录的list位置
        protected float moveSpeed;  //当前的移动速度
        RectTransform _operateItem;
        protected ScrollPageDirection   m_PreviousOperateDirection  = ScrollPageDirection.None;

        protected bool m_FinishLastMoveCurrent = true;//标示是否完成上一个滑动
        protected bool m_JustFinishShowLayoutWithSpecialRowOrColumn = false; //标示是否刚完成 ShowLayoutWithSpecialRowOrColumn 操作 每次操作后下一次滑动时候可以进行额外的处理
        [Header("界面引用")]
        public RectTransform m_PageScrollButtonTrans_N; //滑动按钮
        public RectTransform m_PageScrollButtonTrans_S;

        private void OnDisable()
        {
            m_JustFinishShowLayoutWithSpecialRowOrColumn = false;
        }



        protected override void LayoutInitial_CheckAndSetting()
        {
            lastTimeRecordPosition = m_ListPanelRectTrans.anchoredPosition;
            base.LayoutInitial_CheckAndSetting();
        }
        public override void Scroll_OnScrollLayoutView(ScrollPageDirection _Direction, float _moveSpeed)
        {
            _moveSpeed = Math.Min(_moveSpeed, m_MoveSpeed);  //防止数值过大  必须处理过大数值造成直接滑出控制区域
            m_ReckMask2D.enabled = true;
            if ((m_InitialItemCount < RowNumber * ColumnNumber))
            {//Can't Be Euqal ，Otherwith Can't Move
                Scroll_ForceStopScroll();
                return;
            }
            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);

            switch (_Direction)
            {
                case ScrollPageDirection.UP:
                    if (m_HorizontalLayout) return;
                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
                    break;
                case ScrollPageDirection.DOWN:
                    if (m_HorizontalLayout) return;
                    if (pageDirection == ScrollPageDirection.None)
                        isInitialed = false;
                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
                    break;
                case ScrollPageDirection.LEFT:
                    if (m_HorizontalLayout == false) return;
                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
                    break;
                case ScrollPageDirection.RIGHT:
                    if (m_HorizontalLayout == false) return;
                    if (pageDirection == ScrollPageDirection.None)
                        isInitialed = false;
                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);
                    break;
            }
        }
        protected override void Scroll_ForceStopScroll()
        {
            moveSpeed = 0;
            m_LayoutState = LayoutState.Idle;
        }

        /// <summary>
        /// 计算每次滑动时候的速度
        /// </summary>
        /// <param name="_Direction"></param>
        protected void CaculateCurrentSpeed(ScrollPageDirection _Direction)
        {
            if (isDrawing)
            {
                moveSpeed = Mathf.Abs(m_CurrentMousPos - m_PreviousMousPos);
                m_PreviousMousPos = m_CurrentMousPos;
                Debug.LogInfor("CaculateCurrentSpeed  " + moveSpeed);
            }
            else
            {
                moveSpeed -= m_DempAcceleration * Time.fixedDeltaTime / 2f;
                Debug.Log("CaculateCurrentSpeed moveSpeed= " + moveSpeed);
            }

            if (moveSpeed <= 1 )
            {
                Scroll_ForceStopScroll();
                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);
                return;
            }
        }


        protected override void LayoutEffect_PanelEffect(bool _isOpen)
        {
            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);

            if (m_PreviousOperateDirection != ScrollPageDirection.None && m_FinishLastMoveCurrent == false)
            {
                Scroll_OnScrollLayoutView(m_PreviousOperateDirection, m_MoveSpeed);
            }

            base.LayoutEffect_PanelEffect(_isOpen);
        }

        ScrollPageDirection lastRecordDirection;
        public override void ReBuildView(int _dataCount, bool _isClearDataSource, bool _isSavingTransReference = true)
        {
            if (IsFirstTimeShow) return;
            lastRecordDirection = m_PreviousOperateDirection;
            base.ReBuildView(_dataCount, _isClearDataSource, _isSavingTransReference);
        }



        protected virtual void ScrollUp()
        { //查看前面的内容
            CaculateCurrentSpeed(ScrollPageDirection.UP);
            m_LayoutState = LayoutState.Sliding;

            if (m_InitialItemCount == RowNumber * ColumnNumber)
            {
                #region 已经快要到顶了
                moveCount = 0;
                lastTimeRecordPosition = listPannelTransInitialPositon; //记录位置为初始位置
                if (m_ListPanelRectTrans.anchoredPosition.y > listPannelTransInitialPositon.y)
                {
                    m_ListPanelRectTrans.anchoredPosition -= new Vector2(0, moveSpeed);  //移动位置
                    if (m_LayoutViewTransChangeHandle != null)
                        m_LayoutViewTransChangeHandle(ScrollPageDirection.UP, moveSpeed, m_ListPanelRectTrans, false);
                    return;
                }

                if (m_PreviousOperateDirection == ScrollPageDirection.UP && m_FinishLastMoveCurrent == false)
                {
                    m_FinishLastMoveCurrent = true;
                    m_PreviousOperateDirection = ScrollPageDirection.None;
                    ScrollUp_Items();
                    //   Debug.Log("触底反弹" + moveCount);
                }
                //		Debug.Log("m_ListPanelRectTrans="+m_ListPanelRectTrans.anchoredPosition+"  "+listPannelTransInitialPositon);
                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon;
                isInitialed = false;
                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
                if (m_LayoutViewTransChangeHandle != null)
                    m_LayoutViewTransChangeHandle(ScrollPageDirection.UP, moveSpeed, m_ListPanelRectTrans, true);

                Scroll_ForceStopScroll();
                return;
                #endregion
            }

            #region 正常向上滑动
            if (m_JustFinishShowLayoutWithSpecialRowOrColumn)
            {
                m_JustFinishShowLayoutWithSpecialRowOrColumn = false;
                ScrollUp_Items();
                isInitialed = false;
            } //当进行重置操作后第一次操作 需要设置一些参数

            if (pageDirection != ScrollPageDirection.UP && pageDirection != ScrollPageDirection.None)
            {
                #region 切换状态 由down 到Up
                if (m_PreviousOperateDirection == ScrollPageDirection.DOWN && m_FinishLastMoveCurrent == false)
                {
                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
                    //     Debug.Log(" 切换状态 由down 到Up 。。。。。。。。。。。。。   继续等待完成Down");
                    return;
                }
                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);

                if (m_InitialItemCount != DataCount)
                {
                    //       Debug.Log(moveCount + "由down 到Up 跳转" + lastTimeRecordPosition);
                    lastTimeRecordPosition += new Vector2(0, (itemRect.y + m_ItemSpace.y)); //此时记录的位置应该比实际的高一节
                    for (int _column = 0; _column < ColumnNumber; ++_column)
                    {
                        --m_InitialItemCount;
                        --arrayDataSourceInitialMark[_column];
                    }
                    isInitialed = true;
                }
                else
                {
                    //      Debug.Log("从最顶上开始向下滑动 显示前面的内容");
                    lastTimeRecordPosition = listPannelTransInitialPositon + new Vector2(0, moveCount * (itemRect.y + m_ItemSpace.y));
                    ScrollUp_Items();
                    isInitialed = false;
                }
                #endregion
            } // 切换状态 由down 到Up

            pageDirection = ScrollPageDirection.UP;  //设置状态

            if (Math.Abs(lastTimeRecordPosition.y - m_ListPanelRectTrans.anchoredPosition.y) >= (itemRect.y + m_ItemSpace.y) - 0.5f)
            {//滑过一行
                isInitialed = false;
                --moveCount;
                lastTimeRecordPosition -= new Vector2(0, (itemRect.y + m_ItemSpace.y));
                //Debug.Log(moveCount + "向shng滑动 了一行" + lastTimeRecordPosition.y);
                ScrollUp_Items();
            }//IF
            m_ListPanelRectTrans.anchoredPosition -= new Vector2(0, moveSpeed);  //移动位置

            if (isInitialed == false)
            { //初始化倒数第二行的数据
                isInitialed = true;
                #region 初始化第一行的数据
                for (int _column = 0; _column < ColumnNumber; ++_column)
                {
                    int _operateIndex = _column * (m_ScrollSpeed + RowNumber);
                    _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;

                    ////**3_29 新增  原因同ScrollLeft 
                    int _currentColumnShowCount = RowNumber;
                    for (int _dex = RowNumber; _dex > 0; --_dex)
                    {
                        int _Itemdex = _column * (m_ScrollSpeed + RowNumber) + RowNumber;
                        RectTransform _lastItem = m_ListPanelRectTrans.Getchild_Ex(_Itemdex);
                        if (_lastItem.gameObject.activeSelf == false)
                            --_currentColumnShowCount; //说明当前项没有数据
                    }       //此时每一列最前面的一行都是即将显示的数据项

                    if (arrayDataSourceInitialMark[_column] - _currentColumnShowCount - 1 >= 0)
                    {
                        _operateItem.gameObject.SetActive(true);
                        int _index = (_column + 1) * (m_ScrollSpeed + RowNumber) - 1;      //检查 每一列第显示区域后第一个元素 
                        RectTransform _item = m_ListPanelRectTrans.Getchild_Ex(_index);
                        if (_item.gameObject.activeSelf)
                        { //说明当前列最后一项有数据   则显示上面元素时需要减去标示     否则说明滑下去的一行数据不全
                            --m_InitialItemCount;
                            --arrayDataSourceInitialMark[_column];
                        }
                        _intialIndex = arrayListDataSource[_column][arrayDataSourceInitialMark[_column] - RowNumber];
                        FillItemButtonData(_operateItem, _intialIndex, _column, arrayDataSourceInitialMark[_column] - RowNumber);  //构建列表项
                    } ////说明还有数据没有显示
                    else
                        _operateItem.gameObject.SetActive(false);

                }//for
                #endregion
            }//if

            if (m_InitialItemCount == RowNumber * ColumnNumber)
            {
                //    Debug.Log("ScrollUp 触底反弹" + moveCount);
                m_FinishLastMoveCurrent = false;
                m_PreviousOperateDirection = ScrollPageDirection.UP;
            }
            if (m_LayoutViewTransChangeHandle != null)
                m_LayoutViewTransChangeHandle(ScrollPageDirection.UP, moveSpeed, m_ListPanelRectTrans, false);
            #endregion

        }

        /// <summary>
        /// 每一类最下面的元素移动到上面
        /// </summary>
        void ScrollUp_Items()
        {
            for (int _column = 0; _column < ColumnNumber; ++_column)
            {
                int _operateIndex = (_column + 1) * (m_ScrollSpeed + RowNumber) - 1;      //每一列第显示区域后第一个元素
                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;
                _operateItem.anchoredPosition += new Vector2(0, (m_ScrollSpeed + RowNumber) * (m_ItemSpace.y + itemRect.y)); //后面几个整体向上移动一整段距离
                _operateItem.SetSiblingIndex(_column * (m_ScrollSpeed + RowNumber));
                _operateItem.gameObject.SetActive(false);
            }
        }

        protected virtual void ScrollDown()
        {   //查看后面的内容
            CaculateCurrentSpeed(ScrollPageDirection.DOWN);
            m_LayoutState = LayoutState.Sliding;

            //Debug.Log("m_InitialItemCount= " + m_InitialItemCount+ "  DataCount= "+ DataCount);
            if (m_InitialItemCount == DataCount)
            {
                #region 快要到底了
                int _totalRow = (DataCount + ColumnNumber - 1) / ColumnNumber;
                moveCount = _totalRow - RowNumber;
                //      Debug.Log("m_InitialItemCount= " + m_InitialItemCount + "  DataCount= " + DataCount + "  moveCount=" + moveCount);
                lastTimeRecordPosition = listPannelTransInitialPositon + new Vector2(0, moveCount * (itemRect.y + m_ItemSpace.y));

                if (m_ListPanelRectTrans.anchoredPosition.y < listPannelTransInitialPositon.y + moveCount * (itemRect.y + m_ItemSpace.y))
                {
                    m_ListPanelRectTrans.anchoredPosition += new Vector2(0, moveSpeed);  //移动位置
                    if (m_LayoutViewTransChangeHandle != null)
                        m_LayoutViewTransChangeHandle(ScrollPageDirection.DOWN, moveSpeed, m_ListPanelRectTrans, false);
                    return;
                } //在靠近底部

                if (m_PreviousOperateDirection == ScrollPageDirection.DOWN && m_FinishLastMoveCurrent == false)
                {
                    //        Debug.Log("ScrollDown 到顶了");
                    m_FinishLastMoveCurrent = true;
                    m_PreviousOperateDirection = ScrollPageDirection.None;
                    ScrollDown_Items(); //当初始化每一列最后一个Item此时刚好显示完所有的数据时，为了保持一致性而使得结束状态与进入状态相同
                }

                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon + new Vector2(0, moveCount * (itemRect.y + m_ItemSpace.y));
                isInitialed = false;

                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
                if (m_LayoutViewTransChangeHandle != null)
                    m_LayoutViewTransChangeHandle(ScrollPageDirection.DOWN, moveSpeed, m_ListPanelRectTrans, true);

                Scroll_ForceStopScroll();
                #endregion
                return;
            }
            #region 正常向下滑动

            if (m_JustFinishShowLayoutWithSpecialRowOrColumn)
            {
                m_JustFinishShowLayoutWithSpecialRowOrColumn = false;
            }

            if (pageDirection != ScrollPageDirection.DOWN && pageDirection != ScrollPageDirection.None)
            { //由up状态切换到down
                if (m_PreviousOperateDirection == ScrollPageDirection.UP && m_FinishLastMoveCurrent == false)
                {
                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
                    //  Debug.Log(" 由up状态切换到down 。。。。。。。   继续等待Up完成");
                    return;
                }
                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
                //   Debug.Log(moveCount + " up 到 down 跳转" + m_InitialItemCount + " >>>");
                if (m_InitialItemCount != RowNumber * ColumnNumber)
                {
                    lastTimeRecordPosition -= new Vector2(0, (itemRect.y + m_ItemSpace.y)); //此时记录的位置应该比实际的高一节
                    for (int _column = 0; _column < ColumnNumber; ++_column)
                    {
                        int _operateIndex = (_column + 1) * (m_ScrollSpeed + RowNumber) - 1;      //每一列第一个元素
                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;
                        if (_operateItem.gameObject.activeSelf)
                        {
                            ++m_InitialItemCount;
                            ++arrayDataSourceInitialMark[_column];
                        }
                    }//for
                    isInitialed = true;
                }//if
                else
                {
                    for (int _column = 0; _column < ColumnNumber; ++_column)
                    {  //移动最上面的一行的数据到末尾
                        int _operateIndex = _column * (m_ScrollSpeed + RowNumber);      //每一列第一个元素
                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;
                        if (_operateItem.gameObject.activeSelf == false)
                        {//滑到顶部后再滑下去的
                            _operateItem.anchoredPosition -= new Vector2(0, (m_ScrollSpeed + RowNumber) * (m_ItemSpace.y + itemRect.y)); //后面几个整体向上移动一整段距离
                            _operateItem.SetSiblingIndex((_column + 1) * (m_ScrollSpeed + RowNumber) - 1);
                            _operateItem.gameObject.SetActive(false);
                        }
                    }  //for
                    isInitialed = false;
                }
            } //由up状态切换到down

            pageDirection = ScrollPageDirection.DOWN;

            if (Math.Abs(m_ListPanelRectTrans.anchoredPosition.y - lastTimeRecordPosition.y) >= (itemRect.y + m_ItemSpace.y) - 0.2f)
            {//滑过一行
                ++moveCount;
                isInitialed = false;
                lastTimeRecordPosition += new Vector2(0, (itemRect.y + m_ItemSpace.y));
                ScrollDown_Items();
            }//if   滑动了一行数据则将第一个Item移动到最后

            if (isInitialed == false)
            {
                #region 初始化每一列最后一个Item
                isInitialed = true;
                for (int _column = 0; _column < ColumnNumber; ++_column)
                {  //移动最上面的一行的数据到末尾
                    int _operateIndex = (_column + 1) * (m_ScrollSpeed + RowNumber) - 1;      //每一列最后一个元素
                    _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;
                    if (arrayDataSourceInitialMark[_column] < arrayListDataSource[_column].Count)
                    {
                        //   Debug.Log(m_InitialItemCount+"ScrollDown最后一列数据" + arrayDataSourceInitialMark[_column]+" ;;;;" + arrayListDataSource[_column].Count);
                        _operateItem.gameObject.SetActive(true);

                        _intialIndex = arrayListDataSource[_column][arrayDataSourceInitialMark[_column]];
                        FillItemButtonData(_operateItem, _intialIndex, _column, arrayDataSourceInitialMark[_column]);  //构建列表项
                        ++m_InitialItemCount;
                        ++arrayDataSourceInitialMark[_column];
                    }
                    else
                        _operateItem.gameObject.SetActive(false);  //当前行数据显示完了
                }  //for
                #endregion
            }//if
            m_ListPanelRectTrans.anchoredPosition += new Vector2(0, moveSpeed);  //移动位置
            if (m_InitialItemCount == DataCount)
            {
                m_PreviousOperateDirection = ScrollPageDirection.DOWN;
                m_FinishLastMoveCurrent = false;
                //  Debug.Log(moveCount + "触底反弹" + m_InitialItemCount);
            }
            if (m_LayoutViewTransChangeHandle != null)
                m_LayoutViewTransChangeHandle(ScrollPageDirection.DOWN, moveSpeed, m_ListPanelRectTrans, false);

            #endregion
        }
        void ScrollDown_Items()
        {
            for (int _column = 0; _column < ColumnNumber; ++_column)
            {  //移动最上面的一行的数据到末尾
                int _operateIndex = _column * (m_ScrollSpeed + RowNumber);      //每一列第一个元素
                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;
                _operateItem.anchoredPosition -= new Vector2(0, (m_ScrollSpeed + RowNumber) * (m_ItemSpace.y + itemRect.y)); //后面几个整体向上移动一整段距离
                _operateItem.SetSiblingIndex((_column + 1) * (m_ScrollSpeed + RowNumber) - 1);
                _operateItem.gameObject.SetActive(false);
            }  //for
        }



        protected virtual void ScrollLeft()
        {
            CaculateCurrentSpeed(ScrollPageDirection.LEFT);
            m_LayoutState = LayoutState.Sliding;

            if (m_InitialItemCount == RowNumber * ColumnNumber)
            {
                #region 已经快要到顶了
                //  Debug.Log("到达顶部" + moveCount);
                moveCount = 0;
                lastTimeRecordPosition = listPannelTransInitialPositon;
                isInitialed = false;

                if (m_ListPanelRectTrans.anchoredPosition.x < listPannelTransInitialPositon.x)
                {
                    m_ListPanelRectTrans.anchoredPosition += new Vector2(moveSpeed, 0);  //移动位置
                    if (m_LayoutViewTransChangeHandle != null)
                        m_LayoutViewTransChangeHandle(ScrollPageDirection.LEFT, moveSpeed, m_ListPanelRectTrans, false);
                    return;
                }

                if (m_PreviousOperateDirection == ScrollPageDirection.LEFT && m_FinishLastMoveCurrent == false)
                {
                    m_FinishLastMoveCurrent = true;
                    m_PreviousOperateDirection = ScrollPageDirection.None;
                    moveCount = 0;
                    ScrollLeft_Item();
                }

                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon;
                lastTimeRecordPosition = m_ListPanelRectTrans.anchoredPosition;
                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
                isInitialed = false;
                if (m_LayoutViewTransChangeHandle != null)
                    m_LayoutViewTransChangeHandle(ScrollPageDirection.LEFT, moveSpeed, m_ListPanelRectTrans, true);

                Scroll_ForceStopScroll();
                return;
                #endregion
            }
            else
            {
                if (m_JustFinishShowLayoutWithSpecialRowOrColumn)
                {
                    m_JustFinishShowLayoutWithSpecialRowOrColumn = false;
                    ScrollLeft_Item();
                    isInitialed = false;
                }

                if (pageDirection != ScrollPageDirection.LEFT && pageDirection != ScrollPageDirection.None)
                {
                    if (m_PreviousOperateDirection == ScrollPageDirection.RIGHT && m_FinishLastMoveCurrent == false)
                    {
                        EventCenter.GetInstance().AddFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);
                        //      Debug.Log(" 切换状态 由 Right 到 down  。。。。。。。。。。。。。   继续等待完成 Right");
                        return;
                    }
                    EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);
                    //Debug.Log("ScrollLeft  m_InitialItemCount= "+m_InitialItemCount+"  DataCount="+DataCount);
                    if (m_InitialItemCount != DataCount)
                    {
                        //	Debug.Log("执行assssaa");
                        lastTimeRecordPosition -= new Vector2((itemRect.x + m_ItemSpace.x), 0); //此时记录的位置应该比实际的高一节
                        for (int _row = 0; _row < RowNumber; ++_row)
                        {
                            --m_InitialItemCount;
                            --arrayDataSourceInitialMark[_row];
                        }
                        isInitialed = true;
                    }
                    else
                    {
                        //       Debug.Log("从最右边开始向左滑动 显示前面的内容");
                        lastTimeRecordPosition = listPannelTransInitialPositon - new Vector2(moveCount * (itemRect.x + m_ItemSpace.x), 0);
                        ScrollLeft_Item();
                        isInitialed = false;
                    }
                }//切换状态 由 right 到 left

                pageDirection = ScrollPageDirection.LEFT;

                if (Math.Abs(lastTimeRecordPosition.x - m_ListPanelRectTrans.anchoredPosition.x) >= (itemRect.x + m_ItemSpace.x) - 0.5f)
                {//滑过一行
                    isInitialed = false;
                    moveCount--;
                    lastTimeRecordPosition += new Vector2((itemRect.x + m_ItemSpace.x), 0);
                    //   Debug.Log(moveCount + "向右滑动 了一行" + lastTimeRecordPosition);
                    ScrollLeft_Item();
                }//滑过一行
                m_ListPanelRectTrans.anchoredPosition += new Vector2(moveSpeed, 0);  //移动位置

                if (isInitialed == false)
                { //初始化倒数第二行的数据
                    isInitialed = true;

                    #region 初始化第一行的数据
                    for (int _row = 0; _row < RowNumber; ++_row)
                    {
                        int _operateIndex = _row * (m_ScrollSpeed + ColumnNumber);
                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;

                        //***********2017/3/23新增***/
                        //测试时发现 当水平滑动时如果最后一行数据不足一整行 则滑到底后向做滑动会有一行无法显示
                        //测试 rownNumber=5,ColumeNumber=1; 数据总数9
                        int _currentRowShowItemCount = ColumnNumber; //当前行正在显示的项数目 有些行可能没有一整行完整的数据
                                                                     //此时每一行最前面的一列都是即将显示的数据项
                        for (int _dex = ColumnNumber; _dex > 0; _dex--)
                        {
                            int _Itemdex = _row * (m_ScrollSpeed + ColumnNumber) + ColumnNumber;
                            RectTransform _lastItem = m_ListPanelRectTrans.Getchild_Ex(_Itemdex);
                            if (_lastItem.gameObject.activeSelf == false)
                                _currentRowShowItemCount--; //说明当前项没有数据
                        }
                        if (arrayDataSourceInitialMark[_row] - _currentRowShowItemCount - 1 >= 0)
                        {//说明还有数据没有显示
                            _operateItem.gameObject.SetActive(true);
                            int _index = (_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1;      //每一列第显示区域后第一个元素
                            RectTransform _item = m_ListPanelRectTrans.Getchild_Ex(_index);
                            if (_item.gameObject.activeSelf)
                            { //说明最下面一行有数据 否则说明滑下去的一行数据不全
                                m_InitialItemCount--;
                                arrayDataSourceInitialMark[_row]--;
                            }
                            _intialIndex = arrayListDataSource[_row][arrayDataSourceInitialMark[_row] - ColumnNumber];
                            FillItemButtonData(_operateItem, _intialIndex, _row, arrayDataSourceInitialMark[_row] - ColumnNumber);  //构建列表项
                        }////说明还有数据没有显示
                        else
                            _operateItem.gameObject.SetActive(false);
                    }//for
                    #endregion
                }//if
                if (m_InitialItemCount == RowNumber * ColumnNumber)
                {
                    m_FinishLastMoveCurrent = false;
                    m_PreviousOperateDirection = ScrollPageDirection.LEFT;
                }
                if (m_LayoutViewTransChangeHandle != null)
                    m_LayoutViewTransChangeHandle(ScrollPageDirection.LEFT, moveSpeed, m_ListPanelRectTrans, false);
            }
        }
        void ScrollLeft_Item()
        {
            for (int _row = 0; _row < RowNumber; ++_row)
            {
                int _operateIndex = (_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1;      //每一列第显示区域后第一个元素
                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;
                _operateItem.anchoredPosition -= new Vector2((m_ScrollSpeed + ColumnNumber) * (itemRect.x + m_ItemSpace.x), 0); //后面几个整体向上移动一整段距离
                _operateItem.SetSiblingIndex(_row * (m_ScrollSpeed + ColumnNumber));
                _operateItem.gameObject.SetActive(false);
            }
        }


        protected virtual void ScrollRight()
        {
            CaculateCurrentSpeed(ScrollPageDirection.RIGHT);
            m_LayoutState = LayoutState.Sliding;
            if (m_InitialItemCount == DataCount)
            {
                #region 快要到底了
                int _totalRow = (DataCount + RowNumber - 1) / RowNumber;
                lastTimeRecordPosition = listPannelTransInitialPositon - new Vector2(moveCount * (itemRect.x + m_ItemSpace.x), 0);
                moveCount = _totalRow - ColumnNumber;
                isInitialed = false;

                if (m_ListPanelRectTrans.anchoredPosition.x >= listPannelTransInitialPositon.x - moveCount * (itemRect.x + m_ItemSpace.x))
                {
                    m_ListPanelRectTrans.anchoredPosition -= new Vector2(moveSpeed, 0);  //移动位置
                    if (m_LayoutViewTransChangeHandle != null)
                        m_LayoutViewTransChangeHandle(ScrollPageDirection.RIGHT, moveSpeed, m_ListPanelRectTrans, false);

                    Scroll_ForceStopScroll();
                    return;
                }

                if (m_PreviousOperateDirection == ScrollPageDirection.RIGHT && m_FinishLastMoveCurrent == false)
                {
                    m_FinishLastMoveCurrent = true;
                    m_PreviousOperateDirection = ScrollPageDirection.None;

                    ScrollRight_Items();// 当初始化每一列最后一个Item此时刚好显示完所有的数据时，为了保持一致性而使得结束状态与进入状态相同
                }

                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon - new Vector2(moveCount * (itemRect.x + m_ItemSpace.x), 0);
                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);

                isInitialed = false;
                if (m_LayoutViewTransChangeHandle != null)
                    m_LayoutViewTransChangeHandle(ScrollPageDirection.RIGHT, moveSpeed, m_ListPanelRectTrans, true);
                #endregion
                return;
            }

            #region 正常向下滑动

            #region 由up状态切换到down
            if (pageDirection != ScrollPageDirection.RIGHT && pageDirection != ScrollPageDirection.None)
            { //由up状态切换到down
                if (m_PreviousOperateDirection == ScrollPageDirection.LEFT && m_FinishLastMoveCurrent == false)
                {
                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
                    //   Debug.Log(" 由 Left  状态切换到 Right  。。。。。。。   继续等待 Left 完成");
                    return;
                }
                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
                if (m_InitialItemCount != RowNumber * ColumnNumber)
                {
                    //              Debug.Log(moveCount + " down  到 up 跳转" + lastTimeRecordPosition);
                    lastTimeRecordPosition += new Vector2((itemRect.x + m_ItemSpace.x), 0); //此时记录的位置应该比实际的高一节
                    for (int _row = 0; _row < RowNumber; _row++)
                    {
                        int _operateIndex = (_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1;      //每一列第一个元素
                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;
                        if (_operateItem.gameObject.activeSelf)
                        {
                            m_InitialItemCount++;
                            arrayDataSourceInitialMark[_row]++;
                        }
                    }//for
                    isInitialed = true;
                }//if
                else
                {
                    Debug.Log("AAAAAAAAAAA");
                    for (int _row = 0; _row < RowNumber; _row++)
                    {  //移动最上面的一行的数据到末尾
                        int _operateIndex = _row * (m_ScrollSpeed + ColumnNumber);      //每一列第一个元素
                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;
                        if (_operateItem.gameObject.activeSelf == false)
                        {//滑到顶部后再滑下去的
                            _operateItem.anchoredPosition += new Vector2((m_ScrollSpeed + ColumnNumber) * (itemRect.x + m_ItemSpace.x), 0); //后面几个整体向上移动一整段距离
                            _operateItem.SetSiblingIndex((_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1);
                            _operateItem.gameObject.SetActive(false);
                        }
                    }  //for
                    isInitialed = false;
                }
            }
            #endregion

            pageDirection = ScrollPageDirection.RIGHT;

            if (Math.Abs(m_ListPanelRectTrans.anchoredPosition.x - lastTimeRecordPosition.x) >= (itemRect.x + m_ItemSpace.x) - 0.5f)
            {//滑过一行
                ++moveCount;
                isInitialed = false;
                lastTimeRecordPosition -= new Vector2((itemRect.x + m_ItemSpace.x), 0);
                ScrollRight_Items();
            }//if  滑动了一行数据则将第一个Item移动到最后

            if (isInitialed == false)
            {
                isInitialed = true;
                for (int _row = 0; _row < RowNumber; _row++)
                {  //移动最上面的一行的数据到末尾
                    int _operateIndex = (_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1;      //每一列最后一个元素
                    _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;

                    if (arrayDataSourceInitialMark[_row] < arrayListDataSource[_row].Count)
                    {
                        _operateItem.gameObject.SetActive(true);
                        _intialIndex = arrayListDataSource[_row][arrayDataSourceInitialMark[_row]];
                        FillItemButtonData(_operateItem, _intialIndex, _row, arrayDataSourceInitialMark[_row]);  //构建列表项
                        m_InitialItemCount++;
                        arrayDataSourceInitialMark[_row]++;
                    }
                    else
                        _operateItem.gameObject.SetActive(false); //当前行显示完所有的数据 最后一项没有 数据
                }  //for
            }//if   初始化每一列最后一个Item

            m_ListPanelRectTrans.anchoredPosition -= new Vector2(moveSpeed, 0);  //移动位置
            if (m_InitialItemCount == DataCount)
            {
                m_PreviousOperateDirection = ScrollPageDirection.RIGHT;
                m_FinishLastMoveCurrent = false;
            }

            if (m_LayoutViewTransChangeHandle != null)
                m_LayoutViewTransChangeHandle(ScrollPageDirection.RIGHT, moveSpeed, m_ListPanelRectTrans, false);
            #endregion

        }
        void ScrollRight_Items()
        {
            for (int _row = 0; _row < RowNumber; ++_row)
            {  //移动最上面的一行的数据到末尾
                int _operateIndex = _row * (m_ScrollSpeed + ColumnNumber);      //每一列第一个元素
                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex) ;
                _operateItem.anchoredPosition += new Vector2((m_ScrollSpeed + ColumnNumber) * (itemRect.x + m_ItemSpace.x), 0); //后面几个整体向上移动一整段距离
                _operateItem.SetSiblingIndex((_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1);
                _operateItem.gameObject.SetActive(false);
            }  //for
        }



        protected override void Reset_ClearData(bool isSaveUIBtnTrans = true, bool isClearDataSource = true, bool isClearInitialRecord = true)
        {
            lastTimeMoveCount = moveCount;

            if (isClearInitialRecord)
            {//When Rebuild Total Need Move To Initial
                pageDirection = ScrollPageDirection.None;
                m_PreviousOperateDirection = ScrollPageDirection.None;
                lastTimeRecordPosition = listPannelTransInitialPositon;
            }
            else
            {
                lastTimeRecordPosition = m_ListPanelRectTrans.anchoredPosition;
            }

            moveCount = 0;
            isInitialed = false;

            base.Reset_ClearData(isSaveUIBtnTrans, isClearDataSource, isClearInitialRecord);

        }


        /// <summary>
        /// 根据上一次记录的显示的元素个数 初始化视图
        /// </summary>
        /// <param name="number"></param>
        protected override void ShowLayoutWithSpecialRowOrColumn(int number)
        {
            if (DataCount == 0)
            {
                //    Debug.Log("ShowLayoutWithSpecialRowOrColumn Fail,No Data To Show");
                return;
            }
            //   Debug.Log("InitialSomeRowOrColumn Start " + number);
            int maxNumber = 0;
            number = CheckAndGetTheRightNumber(number, ref maxNumber);
            //   Debug.Log("InitialSomeRowOrColumn End " + number);
            if (maxNumber == 0)
            {
                //     Debug.Log("ShowLayoutWithSpecialRowOrColumn Fail2 ,No Data To Show");
                m_InitialItemCount = DataCount;
                return;
            }
            RectTransform _operateButton;
            if (m_HorizontalLayout)
            {
                #region 水平布局
                m_InitialItemCount = number * RowNumber;
                ResetItemState(number);
                //Debug.Log("m_InitialItemCount =" + m_InitialItemCount);

                for (int _row = 0; _row < RowNumber; ++_row)
                {
                    arrayDataSourceInitialMark[_row] = number; //配置列表项初始化的标签
                    for (int _column = 0; _column < ColumnNumber; ++_column)
                    {
                        if (_column + number < arrayListDataSource[_row].Count)
                        {
                            _operateButton = m_ListPanelRectTrans.Getchild_Ex(_row * (ColumnNumber + m_ScrollSpeed) + _column); //Get the row
                            if (_operateButton.gameObject.activeSelf == false)
                                _operateButton.gameObject.SetActive(true);

                            FillItemButtonData(_operateButton, arrayListDataSource[_row][_column + number], _row, _column + number);
                            ++arrayDataSourceInitialMark[_row]; //Update The initial Mark
                            ++m_InitialItemCount;
                        }//if
                        else
                        {
                             _operateButton = m_ListPanelRectTrans.Getchild_Ex(_row * (ColumnNumber + m_ScrollSpeed) + _column); //Get the row
                            if (_operateButton.gameObject.activeSelf)
                                _operateButton.gameObject.SetActive(false);
                        }
                    }//for
                }//列表项的初始化和显示

                AfterShowLayoutWithSpecialRowOrColumn(number);

                #endregion
                return;
            }

            #region 垂直布局
            m_InitialItemCount = number * ColumnNumber;  //前面InitialedNumber 行或者列是填充满的
            ResetItemState(number); //设置每一个列表项的位置
                                    //   Debug.Log("m_InitialItemCount =" + m_InitialItemCount);
            for (int _column = 0; _column < ColumnNumber; ++_column)
            {
                arrayDataSourceInitialMark[_column] = number; //设置数据源初始化的元素个数
                for (int _row = 0; _row < RowNumber; ++_row)
                {
                    if (_row + number < arrayListDataSource[_column].Count)
                    {
                        _operateButton = m_ListPanelRectTrans.Getchild_Ex(_column * (RowNumber + m_ScrollSpeed) + _row); //Get the row
                        if (_operateButton.gameObject.activeSelf == false)
                            _operateButton.gameObject.SetActive(true);
                        FillItemButtonData(_operateButton, arrayListDataSource[_column][_row + number], _column, _row + number);
                        ++arrayDataSourceInitialMark[_column]; //Update The initial Mark
                        ++m_InitialItemCount;
                    }//if
                    else
                    {
                        _operateButton = m_ListPanelRectTrans.Getchild_Ex(_column * (RowNumber + m_ScrollSpeed) + _row); //Get the row
                        if (_operateButton.gameObject.activeSelf)
                            _operateButton.gameObject.SetActive(false);
                    }
                }//for
            }//for    ///显示和初始化列表项的内容

            AfterShowLayoutWithSpecialRowOrColumn(number);

            #endregion
        }


        protected void AfterShowLayoutWithSpecialRowOrColumn(int number)
        {
            //列表项状态的维护
            m_FinishLastMoveCurrent = true;
            m_PreviousOperateDirection = lastRecordDirection;
            moveCount = number;
            isInitialed = false;
            m_JustFinishShowLayoutWithSpecialRowOrColumn = true; //设置标示位

            if (m_HorizontalLayout)
            {
                pageDirection = ScrollPageDirection.RIGHT;
                lastTimeRecordPosition = listPannelTransInitialPositon - new Vector2((moveCount) * (itemRect.x + m_ItemSpace.x), 0);
                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon - new Vector2(moveCount * (itemRect.x + m_ItemSpace.x), 0);

                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
                if (m_LayoutViewTransChangeHandle != null)
                    m_LayoutViewTransChangeHandle(ScrollPageDirection.RIGHT, moveSpeed, m_ListPanelRectTrans, true);
            }
            else
            {
                pageDirection = ScrollPageDirection.None;
                lastTimeRecordPosition = listPannelTransInitialPositon + new Vector2(0, (moveCount) * (itemRect.y + m_ItemSpace.y));
                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon + new Vector2(0, moveCount * (itemRect.y + m_ItemSpace.y));

                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
                if (m_LayoutViewTransChangeHandle != null)
                    m_LayoutViewTransChangeHandle(ScrollPageDirection.None, moveSpeed, m_ListPanelRectTrans, true);
            }

            Scroll_ForceStopScroll();
        }



        /// <summary>
        /// 根据输入的行或者列数得到修正后的结果
        /// </summary>
        /// <param name="number">希望从哪一行(列)开始显示数据</param>
        /// <param name="maxNumber"></param>
        /// <returns>从哪一行或者列开始填充数据</returns>
        int CheckAndGetTheRightNumber(int number, ref int maxNumber)
        {
            if (m_HorizontalLayout)
                maxNumber = (DataCount + RowNumber - 1) / RowNumber; //计算最大的视图行数
            else
                maxNumber = (DataCount + ColumnNumber - 1) / ColumnNumber;

            if (DataCount <= RowNumber * ColumnNumber)
                return 0; //全部显示数据

            if (m_HorizontalLayout)
                return Mathf.Min(maxNumber - ColumnNumber, number); //确保显示是正确的视图
            return Mathf.Min(maxNumber - RowNumber, number);
        }



        bool isDrawing = false;
        private float m_PreviousMousPos= 0;  //鼠标前一帧的位置
        private float m_CurrentMousPos = 0;  //鼠标每一帧的位置
        Vector2 startDragPostion = Vector2.zero;
        [Header("OnDrag Sensitivity")]
        [SerializeField]
        [Range(0, 10)]
        protected float DragSensitivity = 3f; //拖拽多远开始响应
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData == null) return;
            isDrawing = true;
            startDragPostion = eventData.position;
            if (m_HorizontalLayout)
                m_CurrentMousPos= m_PreviousMousPos = eventData.position.x;
            else
                m_CurrentMousPos= m_PreviousMousPos = eventData.position.y;
            //  Debug.Log("Begin     " + eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData == null) return;
            isDrawing = false;
            moveSpeed = Mathf.Abs(eventData.position.x - m_PreviousMousPos);
            m_PreviousMousPos = m_CurrentMousPos = eventData.position.x;
            moveSpeed = Mathf.Clamp(moveSpeed, 1.5f, 60);

            Debug.LogInfor("OnEndDrag  moveSpeed=" + moveSpeed);
            //    Debug.Log("Draging ....   " + eventData.position);

        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData == null) return;
            if (m_HorizontalLayout)
                m_CurrentMousPos  = eventData.position.x;
            else
                m_CurrentMousPos  = eventData.position.y;

            Vector2 DragDistance = (eventData.position - startDragPostion);
            if (m_HorizontalLayout)
            {
                if (Math.Abs(DragDistance.x) > DragSensitivity)
                {
                    if (DragDistance.x > 0)
                    {
                        Scroll_OnScrollLayoutView(ScrollPageDirection.LEFT, Math.Abs(DragDistance.x));
                    }
                    else
                    {
                        Scroll_OnScrollLayoutView(ScrollPageDirection.RIGHT, Math.Abs(DragDistance.x));
                    }
                    startDragPostion = eventData.position;
                }
            }
            else
            {
                if (Math.Abs(DragDistance.y) > DragSensitivity)
                {
                    if (DragDistance.y > 0)
                    {
                        Scroll_OnScrollLayoutView(ScrollPageDirection.DOWN, Math.Abs(DragDistance.y));
                    }
                    else
                    {
                        Scroll_OnScrollLayoutView(ScrollPageDirection.UP, Math.Abs(DragDistance.y));
                    }
                    startDragPostion = eventData.position;
                }
            }

            //         Debug.Log("End     " + eventData.position);

        }












    }
}



//***********备份
#region 
//    using UnityEngine;
//using System.Collections;
//using System;

////2017/7/6 修复了在ShowLayoutWithSpecialRowOrColumn后向上或者向左滑动时候出现异常问题，添加标示m_JustFinishShowLayoutWithSpecialRowOrColumn
//           //使得重置后再下一次滑动时候又一次机会进行一些设置

//namespace MFramework.UI.Layout
//{
//    /// <summary>
//    /// 平滑滑动的列表布局
//    /// </summary>
//    public abstract class SmothLayoutController : BaseLayoutController
//    {


//        [SerializeField]
//        protected AnimationCurve m_SpeedControllCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
//        [SerializeField]
//        [Range(0.1f, 2f)]
//        protected float m_TweenTime = 0.4f;
//        protected int moveCount = 0;  //划过的整行或者列的数目
//        protected bool isInitialed = false;

//        protected ScrollPageDirection pageDirection = ScrollPageDirection.None;  //滑动方向
//        protected Vector2 lastTimeRecordPosition;  //上一次记录的list位置
//        protected float moveSpeed;  //当前的移动速度
//        RectTransform _operateItem;
//        protected ScrollPageDirection m_PreviousOperateDirection = ScrollPageDirection.None;

//        protected bool m_FinishLastMoveCurrent = true;//标示是否完成上一个滑动
//        protected bool m_JustFinishShowLayoutWithSpecialRowOrColumn = false; //标示是否刚完成 ShowLayoutWithSpecialRowOrColumn 操作 每次操作后下一次滑动时候可以进行额外的处理
//        [Header("界面引用")]
//        public RectTransform m_PageScrollButtonTrans_N; //滑动按钮
//        public RectTransform m_PageScrollButtonTrans_S;

//        private void OnDisable()
//        {
//            m_JustFinishShowLayoutWithSpecialRowOrColumn = false;
//        }



//        protected override void LayoutInitial_CheckAndSetting()
//        {
//            lastTimeRecordPosition = m_ListPanelRectTrans.anchoredPosition;
//            base.LayoutInitial_CheckAndSetting();
//        }
//        public override void Scroll_OnScrollLayoutView(ScrollPageDirection _Direction, float _moveSpeed)
//        {
//            _moveSpeed = Math.Min(_moveSpeed, m_MoveSpeed);  //防止数值过大  必须处理过大数值造成直接滑出控制区域
//            m_ReckMask2D.enabled = true;
//            if ((m_InitialItemCount < RowNumber * ColumnNumber))
//            {//Can't Be Euqal ，Otherwith Can't Move
//                Scroll_ForceStopScroll();
//                return;
//            }
//            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
//            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
//            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
//            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);

//            switch (_Direction)
//            {
//                case ScrollPageDirection.UP:
//                    if (m_HorizontalLayout) return;
//                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
//                    break;
//                case ScrollPageDirection.DOWN:
//                    if (m_HorizontalLayout) return;
//                    if (pageDirection == ScrollPageDirection.None)
//                        isInitialed = false;
//                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
//                    break;
//                case ScrollPageDirection.LEFT:
//                    if (m_HorizontalLayout == false) return;
//                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
//                    break;
//                case ScrollPageDirection.RIGHT:
//                    if (m_HorizontalLayout == false) return;
//                    if (pageDirection == ScrollPageDirection.None)
//                        isInitialed = false;
//                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);
//                    break;
//            }
//        }
//        protected override void Scroll_ForceStopScroll()
//        {
//            m_RecordCurveTime = 0;
//            moveSpeed = 0;
//            m_LayoutState = LayoutState.Idle;
//        }

//        int m_RecordCurveTime = 0;  //控制滑动速度的取值
//        /// <summary>
//        /// 计算每次滑动时候的速度
//        /// </summary>
//        /// <param name="_Direction"></param>
//        protected void CaculateCurrentSpeed(ScrollPageDirection _Direction)
//        {
//            moveSpeed = m_SpeedControllCurve.Evaluate(m_RecordCurveTime / (m_TweenTime * 30)) * m_MoveSpeed;
//            ++m_RecordCurveTime;
//            if (moveSpeed <= 1 || m_RecordCurveTime == (int)(m_TweenTime * 30))
//            {
//                Scroll_ForceStopScroll();
//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);
//                return;
//            }
//        }


//        protected override void LayoutEffect_PanelEffect(bool _isOpen)
//        {
//            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
//            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
//            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
//            EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);

//            if (m_PreviousOperateDirection != ScrollPageDirection.None && m_FinishLastMoveCurrent == false)
//            {
//                Scroll_OnScrollLayoutView(m_PreviousOperateDirection, m_MoveSpeed);
//            }

//            base.LayoutEffect_PanelEffect(_isOpen);
//        }

//        ScrollPageDirection lastRecordDirection;
//        public override void ReBuildView(int _dataCount, bool _isClearDataSource, bool _isSavingTransReference = true)
//        {
//            if (IsFirstTimeShow) return;
//            lastRecordDirection = m_PreviousOperateDirection;
//            base.ReBuildView(_dataCount, _isClearDataSource, _isSavingTransReference);
//        }



//        protected virtual void ScrollUp()
//        { //查看前面的内容
//            CaculateCurrentSpeed(ScrollPageDirection.UP);
//            m_LayoutState = LayoutState.Sliding;

//            if (m_InitialItemCount == RowNumber * ColumnNumber)
//            {
//                #region 已经快要到顶了
//                moveCount = 0;
//                lastTimeRecordPosition = listPannelTransInitialPositon; //记录位置为初始位置
//                if (m_ListPanelRectTrans.anchoredPosition.y > listPannelTransInitialPositon.y)
//                {
//                    m_ListPanelRectTrans.anchoredPosition -= new Vector2(0, moveSpeed);  //移动位置
//                    if (m_LayoutViewTransChangeHandle != null)
//                        m_LayoutViewTransChangeHandle(ScrollPageDirection.UP, moveSpeed, m_ListPanelRectTrans, false);
//                    return;
//                }

//                if (m_PreviousOperateDirection == ScrollPageDirection.UP && m_FinishLastMoveCurrent == false)
//                {
//                    m_FinishLastMoveCurrent = true;
//                    m_PreviousOperateDirection = ScrollPageDirection.None;
//                    ScrollUp_Items();
//                    //   Debug.Log("触底反弹" + moveCount);
//                }
//                //		Debug.Log("m_ListPanelRectTrans="+m_ListPanelRectTrans.anchoredPosition+"  "+listPannelTransInitialPositon);
//                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon;
//                isInitialed = false;
//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
//                if (m_LayoutViewTransChangeHandle != null)
//                    m_LayoutViewTransChangeHandle(ScrollPageDirection.UP, moveSpeed, m_ListPanelRectTrans, true);

//                Scroll_ForceStopScroll();
//                return;
//                #endregion
//            }

//            #region 正常向上滑动
//            if (m_JustFinishShowLayoutWithSpecialRowOrColumn)
//            {
//                m_JustFinishShowLayoutWithSpecialRowOrColumn = false;
//                ScrollUp_Items();
//                isInitialed = false;
//            } //当进行重置操作后第一次操作 需要设置一些参数

//            if (pageDirection != ScrollPageDirection.UP && pageDirection != ScrollPageDirection.None)
//            {
//                #region 切换状态 由down 到Up
//                if (m_PreviousOperateDirection == ScrollPageDirection.DOWN && m_FinishLastMoveCurrent == false)
//                {
//                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
//                    //     Debug.Log(" 切换状态 由down 到Up 。。。。。。。。。。。。。   继续等待完成Down");
//                    return;
//                }
//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);

//                if (m_InitialItemCount != DataCount)
//                {
//                    //       Debug.Log(moveCount + "由down 到Up 跳转" + lastTimeRecordPosition);
//                    lastTimeRecordPosition += new Vector2(0, (itemRect.y + m_ItemSpace.y)); //此时记录的位置应该比实际的高一节
//                    for (int _column = 0; _column < ColumnNumber; ++_column)
//                    {
//                        --m_InitialItemCount;
//                        --arrayDataSourceInitialMark[_column];
//                    }
//                    isInitialed = true;
//                }
//                else
//                {
//                    //      Debug.Log("从最顶上开始向下滑动 显示前面的内容");
//                    lastTimeRecordPosition = listPannelTransInitialPositon + new Vector2(0, moveCount * (itemRect.y + m_ItemSpace.y));
//                    ScrollUp_Items();
//                    isInitialed = false;
//                }
//                #endregion
//            } // 切换状态 由down 到Up

//            pageDirection = ScrollPageDirection.UP;  //设置状态

//            if (Math.Abs(lastTimeRecordPosition.y - m_ListPanelRectTrans.anchoredPosition.y) >= (itemRect.y + m_ItemSpace.y) - 0.5f)
//            {//滑过一行
//                isInitialed = false;
//                --moveCount;
//                lastTimeRecordPosition -= new Vector2(0, (itemRect.y + m_ItemSpace.y));
//                //Debug.Log(moveCount + "向shng滑动 了一行" + lastTimeRecordPosition.y);
//                ScrollUp_Items();
//            }//IF
//            m_ListPanelRectTrans.anchoredPosition -= new Vector2(0, moveSpeed);  //移动位置

//            if (isInitialed == false)
//            { //初始化倒数第二行的数据
//                isInitialed = true;
//                #region 初始化第一行的数据
//                for (int _column = 0; _column < ColumnNumber; ++_column)
//                {
//                    int _operateIndex = _column * (m_ScrollSpeed + RowNumber);
//                    _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);

//                    ////**3_29 新增  原因同ScrollLeft 
//                    int _currentColumnShowCount = RowNumber;
//                    for (int _dex = RowNumber; _dex > 0; --_dex)
//                    {
//                        int _Itemdex = _column * (m_ScrollSpeed + RowNumber) + RowNumber;
//                        RectTransform _lastItem = m_ListPanelRectTrans.Getchild_Ex(_Itemdex);
//                        if (_lastItem.gameObject.activeSelf == false)
//                            --_currentColumnShowCount; //说明当前项没有数据
//                    }       //此时每一列最前面的一行都是即将显示的数据项

//                    if (arrayDataSourceInitialMark[_column] - _currentColumnShowCount - 1 >= 0)
//                    {
//                        _operateItem.gameObject.SetActive(true);
//                        int _index = (_column + 1) * (m_ScrollSpeed + RowNumber) - 1;      //检查 每一列第显示区域后第一个元素 
//                        RectTransform _item = m_ListPanelRectTrans.Getchild_Ex(_index);
//                        if (_item.gameObject.activeSelf)
//                        { //说明当前列最后一项有数据   则显示上面元素时需要减去标示     否则说明滑下去的一行数据不全
//                            --m_InitialItemCount;
//                            --arrayDataSourceInitialMark[_column];
//                        }
//                        _intialIndex = arrayListDataSource[_column][arrayDataSourceInitialMark[_column] - RowNumber];
//                        FillItemButtonData(_operateItem, _intialIndex, _column, arrayDataSourceInitialMark[_column] - RowNumber);  //构建列表项
//                    } ////说明还有数据没有显示
//                    else
//                        _operateItem.gameObject.SetActive(false);

//                }//for
//                #endregion
//            }//if

//            if (m_InitialItemCount == RowNumber * ColumnNumber)
//            {
//                //    Debug.Log("ScrollUp 触底反弹" + moveCount);
//                m_FinishLastMoveCurrent = false;
//                m_PreviousOperateDirection = ScrollPageDirection.UP;
//            }
//            if (m_LayoutViewTransChangeHandle != null)
//                m_LayoutViewTransChangeHandle(ScrollPageDirection.UP, moveSpeed, m_ListPanelRectTrans, false);
//            #endregion

//        }

//        /// <summary>
//        /// 每一类最下面的元素移动到上面
//        /// </summary>
//        void ScrollUp_Items()
//        {
//            for (int _column = 0; _column < ColumnNumber; ++_column)
//            {
//                int _operateIndex = (_column + 1) * (m_ScrollSpeed + RowNumber) - 1;      //每一列第显示区域后第一个元素
//                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                _operateItem.anchoredPosition += new Vector2(0, (m_ScrollSpeed + RowNumber) * (m_ItemSpace.y + itemRect.y)); //后面几个整体向上移动一整段距离
//                _operateItem.SetSiblingIndex(_column * (m_ScrollSpeed + RowNumber));
//                _operateItem.gameObject.SetActive(false);
//            }
//        }

//        protected virtual void ScrollDown()
//        {   //查看后面的内容
//            CaculateCurrentSpeed(ScrollPageDirection.DOWN);
//            m_LayoutState = LayoutState.Sliding;

//            //Debug.Log("m_InitialItemCount= " + m_InitialItemCount+ "  DataCount= "+ DataCount);
//            if (m_InitialItemCount == DataCount)
//            {
//                #region 快要到底了
//                int _totalRow = (DataCount + ColumnNumber - 1) / ColumnNumber;
//                moveCount = _totalRow - RowNumber;
//                //      Debug.Log("m_InitialItemCount= " + m_InitialItemCount + "  DataCount= " + DataCount + "  moveCount=" + moveCount);
//                lastTimeRecordPosition = listPannelTransInitialPositon + new Vector2(0, moveCount * (itemRect.y + m_ItemSpace.y));

//                if (m_ListPanelRectTrans.anchoredPosition.y < listPannelTransInitialPositon.y + moveCount * (itemRect.y + m_ItemSpace.y))
//                {
//                    m_ListPanelRectTrans.anchoredPosition += new Vector2(0, moveSpeed);  //移动位置
//                    if (m_LayoutViewTransChangeHandle != null)
//                        m_LayoutViewTransChangeHandle(ScrollPageDirection.DOWN, moveSpeed, m_ListPanelRectTrans, false);
//                    return;
//                } //在靠近底部

//                if (m_PreviousOperateDirection == ScrollPageDirection.DOWN && m_FinishLastMoveCurrent == false)
//                {
//                    //        Debug.Log("ScrollDown 到顶了");
//                    m_FinishLastMoveCurrent = true;
//                    m_PreviousOperateDirection = ScrollPageDirection.None;
//                    ScrollDown_Items(); //当初始化每一列最后一个Item此时刚好显示完所有的数据时，为了保持一致性而使得结束状态与进入状态相同
//                }

//                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon + new Vector2(0, moveCount * (itemRect.y + m_ItemSpace.y));
//                isInitialed = false;

//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
//                if (m_LayoutViewTransChangeHandle != null)
//                    m_LayoutViewTransChangeHandle(ScrollPageDirection.DOWN, moveSpeed, m_ListPanelRectTrans, true);

//                Scroll_ForceStopScroll();
//                #endregion
//                return;
//            }
//            #region 正常向下滑动

//            if (m_JustFinishShowLayoutWithSpecialRowOrColumn)
//            {
//                m_JustFinishShowLayoutWithSpecialRowOrColumn = false;
//            }

//            if (pageDirection != ScrollPageDirection.DOWN && pageDirection != ScrollPageDirection.None)
//            { //由up状态切换到down
//                if (m_PreviousOperateDirection == ScrollPageDirection.UP && m_FinishLastMoveCurrent == false)
//                {
//                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
//                    //  Debug.Log(" 由up状态切换到down 。。。。。。。   继续等待Up完成");
//                    return;
//                }
//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollUp, EventCenter.UpdateRate.DelayOneFrame);
//                //   Debug.Log(moveCount + " up 到 down 跳转" + m_InitialItemCount + " >>>");
//                if (m_InitialItemCount != RowNumber * ColumnNumber)
//                {
//                    lastTimeRecordPosition -= new Vector2(0, (itemRect.y + m_ItemSpace.y)); //此时记录的位置应该比实际的高一节
//                    for (int _column = 0; _column < ColumnNumber; ++_column)
//                    {
//                        int _operateIndex = (_column + 1) * (m_ScrollSpeed + RowNumber) - 1;      //每一列第一个元素
//                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                        if (_operateItem.gameObject.activeSelf)
//                        {
//                            ++m_InitialItemCount;
//                            ++arrayDataSourceInitialMark[_column];
//                        }
//                    }//for
//                    isInitialed = true;
//                }//if
//                else
//                {
//                    for (int _column = 0; _column < ColumnNumber; ++_column)
//                    {  //移动最上面的一行的数据到末尾
//                        int _operateIndex = _column * (m_ScrollSpeed + RowNumber);      //每一列第一个元素
//                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                        if (_operateItem.gameObject.activeSelf == false)
//                        {//滑到顶部后再滑下去的
//                            _operateItem.anchoredPosition -= new Vector2(0, (m_ScrollSpeed + RowNumber) * (m_ItemSpace.y + itemRect.y)); //后面几个整体向上移动一整段距离
//                            _operateItem.SetSiblingIndex((_column + 1) * (m_ScrollSpeed + RowNumber) - 1);
//                            _operateItem.gameObject.SetActive(false);
//                        }
//                    }  //for
//                    isInitialed = false;
//                }
//            } //由up状态切换到down

//            pageDirection = ScrollPageDirection.DOWN;

//            if (Math.Abs(m_ListPanelRectTrans.anchoredPosition.y - lastTimeRecordPosition.y) >= (itemRect.y + m_ItemSpace.y) - 0.2f)
//            {//滑过一行
//                ++moveCount;
//                isInitialed = false;
//                lastTimeRecordPosition += new Vector2(0, (itemRect.y + m_ItemSpace.y));
//                ScrollDown_Items();
//            }//if   滑动了一行数据则将第一个Item移动到最后

//            if (isInitialed == false)
//            {
//                #region 初始化每一列最后一个Item
//                isInitialed = true;
//                for (int _column = 0; _column < ColumnNumber; ++_column)
//                {  //移动最上面的一行的数据到末尾
//                    int _operateIndex = (_column + 1) * (m_ScrollSpeed + RowNumber) - 1;      //每一列最后一个元素
//                    _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                    if (arrayDataSourceInitialMark[_column] < arrayListDataSource[_column].Count)
//                    {
//                        //   Debug.Log(m_InitialItemCount+"ScrollDown最后一列数据" + arrayDataSourceInitialMark[_column]+" ;;;;" + arrayListDataSource[_column].Count);
//                        _operateItem.gameObject.SetActive(true);

//                        _intialIndex = arrayListDataSource[_column][arrayDataSourceInitialMark[_column]];
//                        FillItemButtonData(_operateItem, _intialIndex, _column, arrayDataSourceInitialMark[_column]);  //构建列表项
//                        ++m_InitialItemCount;
//                        ++arrayDataSourceInitialMark[_column];
//                    }
//                    else
//                        _operateItem.gameObject.SetActive(false);  //当前行数据显示完了
//                }  //for
//                #endregion
//            }//if
//            m_ListPanelRectTrans.anchoredPosition += new Vector2(0, moveSpeed);  //移动位置
//            if (m_InitialItemCount == DataCount)
//            {
//                m_PreviousOperateDirection = ScrollPageDirection.DOWN;
//                m_FinishLastMoveCurrent = false;
//                //  Debug.Log(moveCount + "触底反弹" + m_InitialItemCount);
//            }
//            if (m_LayoutViewTransChangeHandle != null)
//                m_LayoutViewTransChangeHandle(ScrollPageDirection.DOWN, moveSpeed, m_ListPanelRectTrans, false);

//            #endregion
//        }
//        void ScrollDown_Items()
//        {
//            for (int _column = 0; _column < ColumnNumber; ++_column)
//            {  //移动最上面的一行的数据到末尾
//                int _operateIndex = _column * (m_ScrollSpeed + RowNumber);      //每一列第一个元素
//                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                _operateItem.anchoredPosition -= new Vector2(0, (m_ScrollSpeed + RowNumber) * (m_ItemSpace.y + itemRect.y)); //后面几个整体向上移动一整段距离
//                _operateItem.SetSiblingIndex((_column + 1) * (m_ScrollSpeed + RowNumber) - 1);
//                _operateItem.gameObject.SetActive(false);
//            }  //for
//        }



//        protected virtual void ScrollLeft()
//        {
//            CaculateCurrentSpeed(ScrollPageDirection.LEFT);
//            m_LayoutState = LayoutState.Sliding;

//            if (m_InitialItemCount == RowNumber * ColumnNumber)
//            {
//                #region 已经快要到顶了
//                //  Debug.Log("到达顶部" + moveCount);
//                moveCount = 0;
//                lastTimeRecordPosition = listPannelTransInitialPositon;
//                isInitialed = false;

//                if (m_ListPanelRectTrans.anchoredPosition.x < listPannelTransInitialPositon.x)
//                {
//                    m_ListPanelRectTrans.anchoredPosition += new Vector2(moveSpeed, 0);  //移动位置
//                    if (m_LayoutViewTransChangeHandle != null)
//                        m_LayoutViewTransChangeHandle(ScrollPageDirection.LEFT, moveSpeed, m_ListPanelRectTrans, false);
//                    return;
//                }

//                if (m_PreviousOperateDirection == ScrollPageDirection.LEFT && m_FinishLastMoveCurrent == false)
//                {
//                    m_FinishLastMoveCurrent = true;
//                    m_PreviousOperateDirection = ScrollPageDirection.None;
//                    moveCount = 0;
//                    ScrollLeft_Item();
//                }

//                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon;
//                lastTimeRecordPosition = m_ListPanelRectTrans.anchoredPosition;
//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
//                isInitialed = false;
//                if (m_LayoutViewTransChangeHandle != null)
//                    m_LayoutViewTransChangeHandle(ScrollPageDirection.LEFT, moveSpeed, m_ListPanelRectTrans, true);

//                Scroll_ForceStopScroll();
//                return;
//                #endregion
//            }
//            else
//            {
//                if (m_JustFinishShowLayoutWithSpecialRowOrColumn)
//                {
//                    m_JustFinishShowLayoutWithSpecialRowOrColumn = false;
//                    ScrollLeft_Item();
//                    isInitialed = false;
//                }

//                if (pageDirection != ScrollPageDirection.LEFT && pageDirection != ScrollPageDirection.None)
//                {
//                    if (m_PreviousOperateDirection == ScrollPageDirection.RIGHT && m_FinishLastMoveCurrent == false)
//                    {
//                        EventCenter.GetInstance().AddFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);
//                        //      Debug.Log(" 切换状态 由 Right 到 down  。。。。。。。。。。。。。   继续等待完成 Right");
//                        return;
//                    }
//                    EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);
//                    //Debug.Log("ScrollLeft  m_InitialItemCount= "+m_InitialItemCount+"  DataCount="+DataCount);
//                    if (m_InitialItemCount != DataCount)
//                    {
//                        //	Debug.Log("执行assssaa");
//                        lastTimeRecordPosition -= new Vector2((itemRect.x + m_ItemSpace.x), 0); //此时记录的位置应该比实际的高一节
//                        for (int _row = 0; _row < RowNumber; ++_row)
//                        {
//                            --m_InitialItemCount;
//                            --arrayDataSourceInitialMark[_row];
//                        }
//                        isInitialed = true;
//                    }
//                    else
//                    {
//                        //       Debug.Log("从最右边开始向左滑动 显示前面的内容");
//                        lastTimeRecordPosition = listPannelTransInitialPositon - new Vector2(moveCount * (itemRect.x + m_ItemSpace.x), 0);
//                        ScrollLeft_Item();
//                        isInitialed = false;
//                    }
//                }//切换状态 由 right 到 left

//                pageDirection = ScrollPageDirection.LEFT;

//                if (Math.Abs(lastTimeRecordPosition.x - m_ListPanelRectTrans.anchoredPosition.x) >= (itemRect.x + m_ItemSpace.x) - 0.5f)
//                {//滑过一行
//                    isInitialed = false;
//                    moveCount--;
//                    lastTimeRecordPosition += new Vector2((itemRect.x + m_ItemSpace.x), 0);
//                    //   Debug.Log(moveCount + "向右滑动 了一行" + lastTimeRecordPosition);
//                    ScrollLeft_Item();
//                }//滑过一行
//                m_ListPanelRectTrans.anchoredPosition += new Vector2(moveSpeed, 0);  //移动位置

//                if (isInitialed == false)
//                { //初始化倒数第二行的数据
//                    isInitialed = true;

//                    #region 初始化第一行的数据
//                    for (int _row = 0; _row < RowNumber; ++_row)
//                    {
//                        int _operateIndex = _row * (m_ScrollSpeed + ColumnNumber);
//                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);

//                        //***********2017/3/23新增***/
//                        //测试时发现 当水平滑动时如果最后一行数据不足一整行 则滑到底后向做滑动会有一行无法显示
//                        //测试 rownNumber=5,ColumeNumber=1; 数据总数9
//                        int _currentRowShowItemCount = ColumnNumber; //当前行正在显示的项数目 有些行可能没有一整行完整的数据
//                                                                     //此时每一行最前面的一列都是即将显示的数据项
//                        for (int _dex = ColumnNumber; _dex > 0; _dex--)
//                        {
//                            int _Itemdex = _row * (m_ScrollSpeed + ColumnNumber) + ColumnNumber;
//                            RectTransform _lastItem = m_ListPanelRectTrans.Getchild_Ex(_Itemdex);
//                            if (_lastItem.gameObject.activeSelf == false)
//                                _currentRowShowItemCount--; //说明当前项没有数据
//                        }
//                        if (arrayDataSourceInitialMark[_row] - _currentRowShowItemCount - 1 >= 0)
//                        {//说明还有数据没有显示
//                            _operateItem.gameObject.SetActive(true);
//                            int _index = (_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1;      //每一列第显示区域后第一个元素
//                            RectTransform _item = m_ListPanelRectTrans.Getchild_Ex(_index);
//                            if (_item.gameObject.activeSelf)
//                            { //说明最下面一行有数据 否则说明滑下去的一行数据不全
//                                m_InitialItemCount--;
//                                arrayDataSourceInitialMark[_row]--;
//                            }
//                            _intialIndex = arrayListDataSource[_row][arrayDataSourceInitialMark[_row] - ColumnNumber];
//                            FillItemButtonData(_operateItem, _intialIndex, _row, arrayDataSourceInitialMark[_row] - ColumnNumber);  //构建列表项
//                        }////说明还有数据没有显示
//                        else
//                            _operateItem.gameObject.SetActive(false);
//                    }//for
//                    #endregion
//                }//if
//                if (m_InitialItemCount == RowNumber * ColumnNumber)
//                {
//                    m_FinishLastMoveCurrent = false;
//                    m_PreviousOperateDirection = ScrollPageDirection.LEFT;
//                }
//                if (m_LayoutViewTransChangeHandle != null)
//                    m_LayoutViewTransChangeHandle(ScrollPageDirection.LEFT, moveSpeed, m_ListPanelRectTrans, false);
//            }
//        }
//        void ScrollLeft_Item()
//        {
//            for (int _row = 0; _row < RowNumber; ++_row)
//            {
//                int _operateIndex = (_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1;      //每一列第显示区域后第一个元素
//                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                _operateItem.anchoredPosition -= new Vector2((m_ScrollSpeed + ColumnNumber) * (itemRect.x + m_ItemSpace.x), 0); //后面几个整体向上移动一整段距离
//                _operateItem.SetSiblingIndex(_row * (m_ScrollSpeed + ColumnNumber));
//                _operateItem.gameObject.SetActive(false);
//            }
//        }


//        protected virtual void ScrollRight()
//        {
//            CaculateCurrentSpeed(ScrollPageDirection.RIGHT);
//            m_LayoutState = LayoutState.Sliding;
//            if (m_InitialItemCount == DataCount)
//            {
//                #region 快要到底了
//                int _totalRow = (DataCount + RowNumber - 1) / RowNumber;
//                lastTimeRecordPosition = listPannelTransInitialPositon - new Vector2(moveCount * (itemRect.x + m_ItemSpace.x), 0);
//                moveCount = _totalRow - ColumnNumber;
//                isInitialed = false;

//                if (m_ListPanelRectTrans.anchoredPosition.x >= listPannelTransInitialPositon.x - moveCount * (itemRect.x + m_ItemSpace.x))
//                {
//                    m_ListPanelRectTrans.anchoredPosition -= new Vector2(moveSpeed, 0);  //移动位置
//                    if (m_LayoutViewTransChangeHandle != null)
//                        m_LayoutViewTransChangeHandle(ScrollPageDirection.RIGHT, moveSpeed, m_ListPanelRectTrans, false);

//                    Scroll_ForceStopScroll();
//                    return;
//                }

//                if (m_PreviousOperateDirection == ScrollPageDirection.RIGHT && m_FinishLastMoveCurrent == false)
//                {
//                    m_FinishLastMoveCurrent = true;
//                    m_PreviousOperateDirection = ScrollPageDirection.None;

//                    ScrollRight_Items();// 当初始化每一列最后一个Item此时刚好显示完所有的数据时，为了保持一致性而使得结束状态与进入状态相同
//                }

//                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon - new Vector2(moveCount * (itemRect.x + m_ItemSpace.x), 0);
//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollRight, EventCenter.UpdateRate.DelayOneFrame);

//                isInitialed = false;
//                if (m_LayoutViewTransChangeHandle != null)
//                    m_LayoutViewTransChangeHandle(ScrollPageDirection.RIGHT, moveSpeed, m_ListPanelRectTrans, true);
//                #endregion
//                return;
//            }

//            #region 正常向下滑动

//            #region 由up状态切换到down
//            if (pageDirection != ScrollPageDirection.RIGHT && pageDirection != ScrollPageDirection.None)
//            { //由up状态切换到down
//                if (m_PreviousOperateDirection == ScrollPageDirection.LEFT && m_FinishLastMoveCurrent == false)
//                {
//                    EventCenter.GetInstance().AddFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
//                    //   Debug.Log(" 由 Left  状态切换到 Right  。。。。。。。   继续等待 Left 完成");
//                    return;
//                }
//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollLeft, EventCenter.UpdateRate.DelayOneFrame);
//                if (m_InitialItemCount != RowNumber * ColumnNumber)
//                {
//                    //              Debug.Log(moveCount + " down  到 up 跳转" + lastTimeRecordPosition);
//                    lastTimeRecordPosition += new Vector2((itemRect.x + m_ItemSpace.x), 0); //此时记录的位置应该比实际的高一节
//                    for (int _row = 0; _row < RowNumber; _row++)
//                    {
//                        int _operateIndex = (_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1;      //每一列第一个元素
//                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                        if (_operateItem.gameObject.activeSelf)
//                        {
//                            m_InitialItemCount++;
//                            arrayDataSourceInitialMark[_row]++;
//                        }
//                    }//for
//                    isInitialed = true;
//                }//if
//                else
//                {
//                    Debug.Log("AAAAAAAAAAA");
//                    for (int _row = 0; _row < RowNumber; _row++)
//                    {  //移动最上面的一行的数据到末尾
//                        int _operateIndex = _row * (m_ScrollSpeed + ColumnNumber);      //每一列第一个元素
//                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                        if (_operateItem.gameObject.activeSelf == false)
//                        {//滑到顶部后再滑下去的
//                            _operateItem.anchoredPosition += new Vector2((m_ScrollSpeed + ColumnNumber) * (itemRect.x + m_ItemSpace.x), 0); //后面几个整体向上移动一整段距离
//                            _operateItem.SetSiblingIndex((_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1);
//                            _operateItem.gameObject.SetActive(false);
//                        }
//                    }  //for
//                    isInitialed = false;
//                }
//            }
//            #endregion

//            pageDirection = ScrollPageDirection.RIGHT;

//            if (Math.Abs(m_ListPanelRectTrans.anchoredPosition.x - lastTimeRecordPosition.x) >= (itemRect.x + m_ItemSpace.x) - 0.5f)
//            {//滑过一行
//                ++moveCount;
//                isInitialed = false;
//                lastTimeRecordPosition -= new Vector2((itemRect.x + m_ItemSpace.x), 0);
//                ScrollRight_Items();
//            }//if  滑动了一行数据则将第一个Item移动到最后

//            if (isInitialed == false)
//            {
//                isInitialed = true;
//                for (int _row = 0; _row < RowNumber; _row++)
//                {  //移动最上面的一行的数据到末尾
//                    int _operateIndex = (_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1;      //每一列最后一个元素
//                    _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);

//                    if (arrayDataSourceInitialMark[_row] < arrayListDataSource[_row].Count)
//                    {
//                        _operateItem.gameObject.SetActive(true);
//                        _intialIndex = arrayListDataSource[_row][arrayDataSourceInitialMark[_row]];
//                        FillItemButtonData(_operateItem, _intialIndex, _row, arrayDataSourceInitialMark[_row]);  //构建列表项
//                        m_InitialItemCount++;
//                        arrayDataSourceInitialMark[_row]++;
//                    }
//                    else
//                        _operateItem.gameObject.SetActive(false); //当前行显示完所有的数据 最后一项没有 数据
//                }  //for
//            }//if   初始化每一列最后一个Item

//            m_ListPanelRectTrans.anchoredPosition -= new Vector2(moveSpeed, 0);  //移动位置
//            if (m_InitialItemCount == DataCount)
//            {
//                m_PreviousOperateDirection = ScrollPageDirection.RIGHT;
//                m_FinishLastMoveCurrent = false;
//            }

//            if (m_LayoutViewTransChangeHandle != null)
//                m_LayoutViewTransChangeHandle(ScrollPageDirection.RIGHT, moveSpeed, m_ListPanelRectTrans, false);
//            #endregion

//        }
//        void ScrollRight_Items()
//        {
//            for (int _row = 0; _row < RowNumber; ++_row)
//            {  //移动最上面的一行的数据到末尾
//                int _operateIndex = _row * (m_ScrollSpeed + ColumnNumber);      //每一列第一个元素
//                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                _operateItem.anchoredPosition += new Vector2((m_ScrollSpeed + ColumnNumber) * (itemRect.x + m_ItemSpace.x), 0); //后面几个整体向上移动一整段距离
//                _operateItem.SetSiblingIndex((_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1);
//                _operateItem.gameObject.SetActive(false);
//            }  //for
//        }



//        protected override void Reset_ClearData(bool isSaveUIBtnTrans = true, bool isClearDataSource = true, bool isClearInitialRecord = true)
//        {
//            lastTimeMoveCount = moveCount;

//            if (isClearInitialRecord)
//            {//When Rebuild Total Need Move To Initial
//                pageDirection = ScrollPageDirection.None;
//                m_PreviousOperateDirection = ScrollPageDirection.None;
//                lastTimeRecordPosition = listPannelTransInitialPositon;
//            }
//            else
//            {
//                lastTimeRecordPosition = m_ListPanelRectTrans.anchoredPosition;
//            }

//            moveCount = 0;
//            isInitialed = false;

//            base.Reset_ClearData(isSaveUIBtnTrans, isClearDataSource, isClearInitialRecord);

//        }


//        /// <summary>
//        /// 根据上一次记录的显示的元素个数 初始化视图
//        /// </summary>
//        /// <param name="number"></param>
//        protected override void ShowLayoutWithSpecialRowOrColumn(int number)
//        {
//            if (DataCount == 0)
//            {
//                //    Debug.Log("ShowLayoutWithSpecialRowOrColumn Fail,No Data To Show");
//                return;
//            }
//            //   Debug.Log("InitialSomeRowOrColumn Start " + number);
//            int maxNumber = 0;
//            number = CheckAndGetTheRightNumber(number, ref maxNumber);
//            //   Debug.Log("InitialSomeRowOrColumn End " + number);
//            if (maxNumber == 0)
//            {
//                //     Debug.Log("ShowLayoutWithSpecialRowOrColumn Fail2 ,No Data To Show");
//                m_InitialItemCount = DataCount;
//                return;
//            }
//            RectTransform _operateButton;
//            if (m_HorizontalLayout)
//            {
//                #region 水平布局
//                m_InitialItemCount = number * RowNumber;
//                ResetItemState(number);
//                //Debug.Log("m_InitialItemCount =" + m_InitialItemCount);

//                for (int _row = 0; _row < RowNumber; ++_row)
//                {
//                    arrayDataSourceInitialMark[_row] = number; //配置列表项初始化的标签
//                    for (int _column = 0; _column < ColumnNumber; ++_column)
//                    {
//                        if (_column + number < arrayListDataSource[_row].Count)
//                        {
//                            _operateButton = m_ListPanelRectTrans.Getchild_Ex(_row * (ColumnNumber + m_ScrollSpeed) + _column); //Get the row
//                            if (_operateButton.gameObject.activeSelf == false)
//                                _operateButton.gameObject.SetActive(true);

//                            FillItemButtonData(_operateButton, arrayListDataSource[_row][_column + number], _row, _column + number);
//                            ++arrayDataSourceInitialMark[_row]; //Update The initial Mark
//                            ++m_InitialItemCount;
//                        }//if
//                        else
//                        {
//                            _operateButton = m_ListPanelRectTrans.Getchild_Ex(_row * (ColumnNumber + m_ScrollSpeed) + _column); //Get the row
//                            if (_operateButton.gameObject.activeSelf)
//                                _operateButton.gameObject.SetActive(false);
//                        }
//                    }//for
//                }//列表项的初始化和显示

//                AfterShowLayoutWithSpecialRowOrColumn(number);

//                #endregion
//                return;
//            }

//            #region 垂直布局
//            m_InitialItemCount = number * ColumnNumber;  //前面InitialedNumber 行或者列是填充满的
//            ResetItemState(number); //设置每一个列表项的位置
//                                    //   Debug.Log("m_InitialItemCount =" + m_InitialItemCount);
//            for (int _column = 0; _column < ColumnNumber; ++_column)
//            {
//                arrayDataSourceInitialMark[_column] = number; //设置数据源初始化的元素个数
//                for (int _row = 0; _row < RowNumber; ++_row)
//                {
//                    if (_row + number < arrayListDataSource[_column].Count)
//                    {
//                        _operateButton = m_ListPanelRectTrans.Getchild_Ex(_column * (RowNumber + m_ScrollSpeed) + _row); //Get the row
//                        if (_operateButton.gameObject.activeSelf == false)
//                            _operateButton.gameObject.SetActive(true);
//                        FillItemButtonData(_operateButton, arrayListDataSource[_column][_row + number], _column, _row + number);
//                        ++arrayDataSourceInitialMark[_column]; //Update The initial Mark
//                        ++m_InitialItemCount;
//                    }//if
//                    else
//                    {
//                        _operateButton = m_ListPanelRectTrans.Getchild_Ex(_column * (RowNumber + m_ScrollSpeed) + _row); //Get the row
//                        if (_operateButton.gameObject.activeSelf)
//                            _operateButton.gameObject.SetActive(false);
//                    }
//                }//for
//            }//for    ///显示和初始化列表项的内容

//            AfterShowLayoutWithSpecialRowOrColumn(number);

//            #endregion
//        }


//        protected void AfterShowLayoutWithSpecialRowOrColumn(int number)
//        {
//            //列表项状态的维护
//            m_FinishLastMoveCurrent = true;
//            m_PreviousOperateDirection = lastRecordDirection;
//            moveCount = number;
//            isInitialed = false;
//            m_JustFinishShowLayoutWithSpecialRowOrColumn = true; //设置标示位

//            if (m_HorizontalLayout)
//            {
//                pageDirection = ScrollPageDirection.RIGHT;
//                lastTimeRecordPosition = listPannelTransInitialPositon - new Vector2((moveCount) * (itemRect.x + m_ItemSpace.x), 0);
//                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon - new Vector2(moveCount * (itemRect.x + m_ItemSpace.x), 0);

//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
//                if (m_LayoutViewTransChangeHandle != null)
//                    m_LayoutViewTransChangeHandle(ScrollPageDirection.RIGHT, moveSpeed, m_ListPanelRectTrans, true);
//            }
//            else
//            {
//                pageDirection = ScrollPageDirection.None;
//                lastTimeRecordPosition = listPannelTransInitialPositon + new Vector2(0, (moveCount) * (itemRect.y + m_ItemSpace.y));
//                m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon + new Vector2(0, moveCount * (itemRect.y + m_ItemSpace.y));

//                EventCenter.GetInstance().RemoveFixedUpdateEvent(ScrollDown, EventCenter.UpdateRate.DelayOneFrame);
//                if (m_LayoutViewTransChangeHandle != null)
//                    m_LayoutViewTransChangeHandle(ScrollPageDirection.None, moveSpeed, m_ListPanelRectTrans, true);
//            }

//            Scroll_ForceStopScroll();
//        }



//        /// <summary>
//        /// 根据输入的行或者列数得到修正后的结果
//        /// </summary>
//        /// <param name="number">希望从哪一行(列)开始显示数据</param>
//        /// <param name="maxNumber"></param>
//        /// <returns>从哪一行或者列开始填充数据</returns>
//        int CheckAndGetTheRightNumber(int number, ref int maxNumber)
//        {
//            if (m_HorizontalLayout)
//                maxNumber = (DataCount + RowNumber - 1) / RowNumber; //计算最大的视图行数
//            else
//                maxNumber = (DataCount + ColumnNumber - 1) / ColumnNumber;

//            if (DataCount <= RowNumber * ColumnNumber)
//                return 0; //全部显示数据

//            if (m_HorizontalLayout)
//                return Mathf.Min(maxNumber - ColumnNumber, number); //确保显示是正确的视图
//            return Mathf.Min(maxNumber - RowNumber, number);
//        }


//    }
//}

#endregion