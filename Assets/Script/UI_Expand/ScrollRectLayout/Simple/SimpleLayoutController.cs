//using UnityEngine;
//using System.Collections;
//using DG.Tweening;
//using System;

//namespace MFramework.UI.Layout
//{

//    public abstract class SimpleLayoutController : BaseLayoutController
//    {
//        [SerializeField]
//        protected float m_TweenTime = 0.02f; //Tween 动画时间
//        public Transform m_PageScrollButtonTrans_N; //滑动按钮
//        public Transform m_PageScrollButtonTrans_S;
//        protected bool m_IscompleteRole = true;  //标示是否已经完成滚动

//        public override void Scroll_OnScrollLayoutView(ScrollPageDirection _Direction, float _moveSpeed)
//        {
//            // base.ScrollPageView(_Direction);
//            if (m_IscompleteRole == false || (m_InitialItemCount < RowNumber * ColumnNumber))
//            {// Debug.Log("操作过于频繁");   Debug.Log("内容太少不需要滑动");
//                if (m_LayoutViewTransChangeHandle != null)
//                {
//                    if (m_HorizontalLayout)
//                        m_LayoutViewTransChangeHandle(_Direction, m_ScrollSpeed * (m_ItemSpace.x + itemRect.x), m_ListPanelRectTrans, true);
//                    else
//                        m_LayoutViewTransChangeHandle(_Direction, m_ScrollSpeed * (m_ItemSpace.y + itemRect.y), m_ListPanelRectTrans, true);
//                }
//                return;
//            }
//            //Debug.Log("initialItemCount " + initialItemCount + "   _Direction  " + _Direction);
//            m_IscompleteRole = false; //进入滑动阶段，不允许再次滑动
//            RectTransform _operateItem;
//            switch (_Direction)
//            {
//                case ScrollPageDirection.LEFT:
//                    if (m_HorizontalLayout == false) return;  //垂直布局时不处理这个事件
//                    #region   LEFT Scroll  查看前面的内容
//                    if (m_InitialItemCount - RowNumber * ColumnNumber <= 0)
//                    {  //        Debug.Log("前面所有的元素显示完毕");
//                        m_IscompleteRole = true;
//                        if (m_LayoutViewTransChangeHandle != null)
//                            m_LayoutViewTransChangeHandle(_Direction, m_ScrollSpeed * (m_ItemSpace.x + itemRect.x), m_ListPanelRectTrans, true);
//                        return;
//                    }
//                    #region   逻辑部分
//                    if (m_InitialItemCount - RowNumber * ColumnNumber >= m_ScrollSpeed * RowNumber)
//                    {
//                        #region       前面有足够多的元素可以滚动一页
//                        //Debug.Log("前面有足够多的元素可以滚动一页");
//                        int _operateIndex = 0;
//                        #region   //*****需要先将后面的元素移动到前面去******//

//                        for (int _row = 0; _row < RowNumber; _row++)
//                        {
//                            for (int _index = 0; _index < m_ScrollSpeed; _index++)
//                            {
//                                _operateIndex = (_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1;      //每次都是操作一行的最后一个元素
//                                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                                _operateItem.gameObject.SetActive(false);
//                                _operateItem.anchoredPosition -= new Vector2((m_ScrollSpeed + ColumnNumber) * (m_ItemSpace.x + itemRect.x), 0);   //前面几个整体向左移动一整段距离
//                                _operateItem.SetSiblingIndex(_row * (m_ScrollSpeed + ColumnNumber)); //重新调整当前操作的列表项在m_ListPanelRectTrans的排列位置
//                            }
//                        }  //if
//                        #endregion

//                        #region  处理初始值减少的逻辑 特别处理最后一个页面
//                        if (m_InitialItemCount == DataCount)
//                        { //****对于滑动到最后然后左移动时，如果最后一页没有填充满，则滑动后初始化的元素个数应该减去最后一页的元素个数
//                          //*** 必须处理 比如一共13个元素 每次滑动4个元素，则当处于最后一页时m_InitialItemCount=13，往左滑动一次m_InitialItemCount=13-3=10；而不是减去4
//                            int _lastColumnNumber = m_InitialItemCount % RowNumber;
//                            if (_lastColumnNumber == 0)
//                            {//说明最后一列是填充满的
//                                m_InitialItemCount -= RowNumber * m_ScrollSpeed;
//                            }
//                            else
//                            {  //最后一列只填充了_lastColumnNumber个元素
//                               //Debug.Log(_lastColumnNumber);
//                                m_InitialItemCount = m_InitialItemCount - RowNumber * (m_ScrollSpeed - 1) - _lastColumnNumber;
//                            }
//                        }
//                        else
//                        { //不是最后一页往前翻动
//                            m_InitialItemCount -= RowNumber * m_ScrollSpeed;
//                        }//else
//                        #endregion

//                        for (int _row = 0; _row < RowNumber; _row++)
//                        {
//                            for (int _index = 0; _index < m_ScrollSpeed; _index++)
//                            {
//                                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_row * (m_ScrollSpeed + ColumnNumber) + _index);
//                                _operateItem.gameObject.SetActive(true);
//                                _intialIndex = arrayListDataSource[_row][arrayDataSourceInitialMark[_row] - m_ScrollSpeed + _index - ColumnNumber];  //需要使用的数据的索引

//                                FillItemButtonData(_operateItem, _intialIndex, _row, arrayDataSourceInitialMark[_row] - m_ScrollSpeed + _index - ColumnNumber);
//                            }
//                            // Debug.Log(_row + "  " + arrayDataSourceInitialMark[_row]);
//                            arrayDataSourceInitialMark[_row] -= m_ScrollSpeed;         //操作完一行后更新数据。这边便于处理
//                        }
//                        ViewItemRole(ScrollPageDirection.LEFT, m_ListPanelRectTrans, m_ScrollSpeed);
//                        #endregion
//                        //if (m_LayoutViewTransChangeHandle != null)
//                        //    m_LayoutViewTransChangeHandle(_Direction, m_ScrollSpeed * (m_ItemSpace.x + itemRect.x), m_ListPanelRectTrans, false);
//                    }    //if
//                    else
//                    {
//                        //Debug.Log("处理最后一个页面数据不能完全填充的情况");
//                        #region  处理最后一个页面数据不能完全填充的情况
//                        int _operatecolumn = (m_InitialItemCount - RowNumber * ColumnNumber + RowNumber - 1) / RowNumber; //获得可以操作的列数
//                        int _operateIndex = 0;
//                        for (int _row = 0; _row < RowNumber; _row++)
//                        {
//                            for (int _index = 0; _index < _operatecolumn; _index++)
//                            {
//                                _operateIndex = (_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1;      //最后一元素
//                                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                                _operateItem.gameObject.SetActive(false);
//                                _operateItem.anchoredPosition -= new Vector2((m_ScrollSpeed + ColumnNumber) * (m_ItemSpace.x + itemRect.x), 0);   //前面几个整体向右移动一整段距离
//                                _operateItem.SetSiblingIndex(_row * (m_ScrollSpeed + ColumnNumber));
//                            }
//                        }  //if

//                        #endregion
//                        m_InitialItemCount = RowNumber * ColumnNumber;
//                        for (int _row = 0; _row < RowNumber; _row++)
//                        {
//                            for (int _index = 0; _index < _operatecolumn; _index++)
//                            {
//                                if (arrayDataSourceInitialMark[_row] - _operatecolumn + _index - ColumnNumber < arrayListDataSource[_row].Count)
//                                {
//                                    _operateItem = m_ListPanelRectTrans.Getchild_Ex(_row * (m_ScrollSpeed + ColumnNumber) + _index);
//                                    _operateItem.gameObject.SetActive(true);
//                                    _intialIndex = arrayListDataSource[_row][arrayDataSourceInitialMark[_row] - _operatecolumn + _index - ColumnNumber];
//                                    FillItemButtonData(_operateItem, _intialIndex, _row, arrayDataSourceInitialMark[_row] - _operatecolumn + _index - ColumnNumber);
//                                }
//                            }
//                            //    Debug.Log(_row + "  " + arrayDataSourceInitialMark[_row]);
//                            arrayDataSourceInitialMark[_row] -= _operatecolumn;         //标志位计数+1
//                        }

//                        ViewItemRole(ScrollPageDirection.LEFT, m_ListPanelRectTrans, _operatecolumn);
//                    }
//                    break;
//                #endregion
//                #endregion
//                case ScrollPageDirection.RIGHT:
//                    if (m_HorizontalLayout == false) return;  //垂直布局时不处理这个事件
//                    #region  RIGHT Scroll 查看后面的对象
//                    if (m_InitialItemCount == DataCount)
//                    {
//                        //Debug.Log("已经没有数据了，最下面的数据显示完");
//                        if (m_LayoutViewTransChangeHandle != null)
//                            m_LayoutViewTransChangeHandle(_Direction, m_ScrollSpeed * (m_ItemSpace.x + itemRect.x), m_ListPanelRectTrans, true);
//                        m_IscompleteRole = true;
//                        return;
//                    }
//                    if (m_InitialItemCount + m_ScrollSpeed * RowNumber <= DataCount)
//                    {
//                        #region  //后面的元素足够全部生成出来
//                        //Debug.Log("down后面的元素足够全部生成出来");
//                        for (int _row = 0; _row < RowNumber; _row++)            //从向右从上到下初始化
//                        {
//                            for (int _index = 0; _index < m_ScrollSpeed; _index++)
//                            {
//                                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_row * (m_ScrollSpeed + ColumnNumber) + ColumnNumber + _index);
//                                _operateItem.gameObject.SetActive(true);
//                                //		Debug.Log(arrayDataSourceInitialMark[_row]+_index);
//                                _intialIndex = arrayListDataSource[_row][arrayDataSourceInitialMark[_row] + _index];
//                                FillItemButtonData(_operateItem, _intialIndex, _row, arrayDataSourceInitialMark[_row] + _index);
//                                //	Debug.Log(_row+"   "+_index+"   _operate "+_operate.gameObject.name);
//                                m_InitialItemCount++;       //更新计数
//                            }  //for
//                               //   Debug.Log(_row + "  " + arrayDataSourceInitialMark[_row]);
//                            arrayDataSourceInitialMark[_row] += m_ScrollSpeed;         //标志位计数+1
//                        }  //for
//                        ViewItemRole(ScrollPageDirection.RIGHT, m_ListPanelRectTrans, m_ScrollSpeed);
//                        #endregion

//                    } //if
//                    else
//                    {
//                        int _operatecolumn = (DataCount - m_InitialItemCount + RowNumber - 1) / RowNumber; //获得可以操作的列数
//                                                                                                           //  int _operateRow = (m_AllItemsDataSource.Count - m_InitialItemCount) % RowNumber; //受到影响的行 0到 RowNumber-1，其中0表示一整列
//                        #region //后面的元素不足以显示一个完整的翻页效果
//                        //  Debug.Log("down后面的元素不足以显示一个完整的翻页效果");
//                        for (int _row = 0; _row < RowNumber; _row++)
//                        {
//                            for (int _index = 0; _index < _operatecolumn; _index++)
//                            {
//                                //Debug.Log("索引 " + (m_InitialItemCount)+ " _index="+ _index);
//                                if (m_InitialItemCount < DataCount)
//                                {
//                                    if (arrayDataSourceInitialMark[_row] + _index < arrayListDataSource[_row].Count)
//                                    {//只有前面几列有数据的列才初始化创建
//                                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(_row * (m_ScrollSpeed + ColumnNumber) + ColumnNumber + _index);
//                                        _operateItem.gameObject.SetActive(true);
//                                        _intialIndex = arrayListDataSource[_row][arrayDataSourceInitialMark[_row] + _index];
//                                        FillItemButtonData(_operateItem, _intialIndex, _row, arrayDataSourceInitialMark[_row] + _index);  //构建列表项
//                                        m_InitialItemCount++;       //更新计数
//                                    }
//                                }
//                            }  //for
//                               //  if (_operateRow == 0 || _row < _operateRow)
//                            {//只有当整列都影响或者当前行索引小于受影响的行数时
//                             //Debug.Log(_row + "  " + arrayDataSourceInitialMark[_row]);
//                                arrayDataSourceInitialMark[_row] += _operatecolumn;
//                            }
//                        }  //for
//                        m_InitialItemCount = DataCount;  //更新计数
//                        ViewItemRole(ScrollPageDirection.RIGHT, m_ListPanelRectTrans, _operatecolumn);
//                        #endregion
//                    }

//                    break;
//                #endregion
//                case ScrollPageDirection.UP:
//                    if (m_HorizontalLayout) return;  //水平布局时不处理这个事件
//                    #region Up Scroll 查看前面的内容
//                    if (m_InitialItemCount - RowNumber * ColumnNumber <= 0)
//                    {//        Debug.Log("前面所有的元素显示完毕");
//                        if (m_LayoutViewTransChangeHandle != null)
//                            m_LayoutViewTransChangeHandle(_Direction, m_ScrollSpeed * (m_ItemSpace.y + itemRect.y), m_ListPanelRectTrans, true);
//                        m_IscompleteRole = true;
//                        return;
//                    }
//                    #region   逻辑部分
//                    if (m_InitialItemCount - RowNumber * ColumnNumber >= m_ScrollSpeed * ColumnNumber)
//                    {
//                        #region       前面有足够多的元素可以滚动一页

//                        #region    //*****需要先将后面的元素移动到前面去******//
//                        int _operateIndex = 0;
//                        for (int _column = 0; _column < ColumnNumber; _column++)
//                        {
//                            for (int _index = 0; _index < m_ScrollSpeed; _index++)
//                            {
//                                _operateIndex = (_column + 1) * (m_ScrollSpeed + RowNumber) - 1;      //每次都是操作一列的最后一个元素
//                                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                                _operateItem.gameObject.SetActive(false);

//                                _operateItem.anchoredPosition += new Vector2(0, (m_ScrollSpeed + RowNumber) * (m_ItemSpace.y + itemRect.y));  //后面几个整体向上移动一整段距离
//                                _operateItem.SetSiblingIndex(_column * (m_ScrollSpeed + RowNumber)); //重新调整当前操作的列表项在m_ListPanelRectTrans的排列位置
//                            }
//                        }  //if
//                        #endregion

//                        #region   处理m_InitialItemCount的变化 特变处理最后一页
//                        if (m_InitialItemCount == DataCount)
//                        { //****对于滑动到最后然后上移动时，如果最后一页没有填充满，则滑动后初始化的元素个数应该减去最后一页的元素个数
//                          //*** 必须处理 比如一共13个元素 每次滑动4个元素，则当处于最后一页时m_InitialItemCount=13，往上滑动一次m_InitialItemCount=13-3=10；而不是减去4
//                            int _lastRowNumber = m_InitialItemCount % ColumnNumber;
//                            if (_lastRowNumber == 0)
//                            {//说明最后一列是填充满的
//                                m_InitialItemCount -= ColumnNumber * m_ScrollSpeed;
//                            }
//                            else
//                            {  //最后一列只填充了_lastColumnNumber个元素
//                               //  Debug.Log(_lastRowNumber);
//                                m_InitialItemCount = m_InitialItemCount - ColumnNumber * (m_ScrollSpeed - 1) - _lastRowNumber;
//                            }
//                        }
//                        else
//                        { //不是最后一页往前翻动
//                            m_InitialItemCount -= ColumnNumber * m_ScrollSpeed;
//                        }//else

//                        #endregion

//                        for (int _column = 0; _column < ColumnNumber; _column++)
//                        {
//                            for (int _index = 0; _index < m_ScrollSpeed; _index++)
//                            {
//                                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_column * (m_ScrollSpeed + RowNumber) + _index);
//                                _operateItem.gameObject.SetActive(true);

//                                _intialIndex = arrayListDataSource[_column][arrayDataSourceInitialMark[_column] - RowNumber - m_ScrollSpeed + _index];
//                                FillItemButtonData(_operateItem, _intialIndex, _column, arrayDataSourceInitialMark[_column] - RowNumber - m_ScrollSpeed + _index);
//                            }
//                            arrayDataSourceInitialMark[_column] -= m_ScrollSpeed;         //操作完一行后更新数据。这边便于处理
//                        }
//                        ViewItemRole(ScrollPageDirection.UP, m_ListPanelRectTrans, m_ScrollSpeed);
//                        #endregion
//                    }    //if
//                    else
//                    {
//                        int _operateRow = (m_InitialItemCount - RowNumber * ColumnNumber + ColumnNumber - 1) / ColumnNumber; //获得可以操作的行数
//                        #region 处理最后一个页面数据不能完全填充的情况

//                        #region  移动下面的Button 到上面去
//                        int _operateIndex = 0;
//                        for (int _column = 0; _column < ColumnNumber; _column++)
//                        {
//                            for (int _index = 0; _index < _operateRow; _index++)
//                            {
//                                _operateIndex = (_column + 1) * (m_ScrollSpeed + RowNumber) - 1;      //每一列最后一元素
//                                _operateItem = m_ListPanelRectTrans.Getchild_Ex(_operateIndex);
//                                _operateItem.gameObject.SetActive(false);

//                                _operateItem.anchoredPosition += new Vector2(0, (m_ScrollSpeed + RowNumber) * (m_ItemSpace.y + itemRect.y)); //后面几个整体向上移动一整段距离
//                                _operateItem.SetSiblingIndex(_column * (m_ScrollSpeed + RowNumber));
//                            }
//                        }  //if
//                        #endregion

//                        m_InitialItemCount = RowNumber * ColumnNumber;
//                        for (int _column = 0; _column < ColumnNumber; _column++)
//                        {
//                            for (int _index = 0; _index < _operateRow; _index++)
//                            {
//                                if (arrayDataSourceInitialMark[_column] - RowNumber - _operateRow + _index < arrayListDataSource[_column].Count)
//                                {
//                                    _operateItem = m_ListPanelRectTrans.Getchild_Ex(_column * (m_ScrollSpeed + RowNumber) + _index);
//                                    _operateItem.gameObject.SetActive(true);

//                                    _intialIndex = arrayListDataSource[_column][arrayDataSourceInitialMark[_column] - RowNumber - _operateRow + _index];
//                                    FillItemButtonData(_operateItem, _intialIndex, _column, arrayDataSourceInitialMark[_column] - RowNumber - _operateRow + _index);
//                                }
//                            }
//                            //*******这里是否有问题，如果不足一列可可操作的行数应该也会小于_operateRow   arrayDataSourceInitialMark[_column] 会小于0
//                            //****由于从上往下填充，所以不存在这种情况？上面的永远是整行，下面的可能不是整行
//                            arrayDataSourceInitialMark[_column] -= _operateRow;         //标志位计数+1
//                        }
//                        ViewItemRole(ScrollPageDirection.UP, m_ListPanelRectTrans, _operateRow);

//                        #endregion
//                    }
//                    #endregion
//                    break;
//                #endregion
//                case ScrollPageDirection.DOWN:
//                    if (m_HorizontalLayout) return;  //水平布局时不处理这个事件
//                    #region Down Scroll 查看后面的内容
//                    if (m_InitialItemCount == DataCount)
//                    { //Debug.Log("已经没有数据了，最下面的数据显示完");
//                        m_IscompleteRole = true;
//                        return;
//                    }
//                    if (m_InitialItemCount + m_ScrollSpeed * ColumnNumber <= DataCount)
//                    {
//                        #region //后面的元素足够全部生成出来
//                        //Debug.Log(m_InitialItemCount + "  " + m_AllItemsDataSource.Count);
//                        for (int column = 0; column < ColumnNumber; column++)            //从上到下从左向右初始化
//                        {
//                            for (int _index = 0; _index < m_ScrollSpeed; _index++)
//                            {
//                                _operateItem = m_ListPanelRectTrans.Getchild_Ex(column * (m_ScrollSpeed + RowNumber) + RowNumber + _index);
//                                _operateItem.gameObject.SetActive(true);

//                                //  Debug.Log(arrayDataSourceInitialMark[column] + "   " + _index);
//                                _intialIndex = arrayListDataSource[column][arrayDataSourceInitialMark[column] + _index];
//                                FillItemButtonData(_operateItem, _intialIndex, column, arrayDataSourceInitialMark[column] + _index);
//                                //  Debug.Log(column + "   " + _index + "   _operate " + _operateItem.gameObject.name);
//                                m_InitialItemCount++;       //更新计数
//                            }  //for
//                            arrayDataSourceInitialMark[column] += m_ScrollSpeed;         //标志位计数+1
//                        }  //for
//                        ViewItemRole(ScrollPageDirection.DOWN, m_ListPanelRectTrans, m_ScrollSpeed);
//                        #endregion
//                    } //if

//                    else
//                    {
//                        #region    后面的元素不足以显示一个完整的翻页效果
//                        int _operateRow = (DataCount - m_InitialItemCount + ColumnNumber - 1) / ColumnNumber; //获得可以操作的行数
//                                                                                                              //     int _operateColumn = (m_AllItemsDataSource.Count - m_InitialItemCount) % ColumnNumber; //受到影响的列 0到ColumnNumber-1，其中0表示一整行
//                        for (int column = 0; column < ColumnNumber; column++)
//                        {
//                            for (int _index = 0; _index < _operateRow; _index++)
//                            {
//                                //   Debug.Log("索引 " + (m_InitialItemCount)+ " _index="+ _index);
//                                if (m_InitialItemCount < DataCount)
//                                {
//                                    if (arrayDataSourceInitialMark[column] + _index < arrayListDataSource[column].Count)
//                                    {
//                                        _operateItem = m_ListPanelRectTrans.Getchild_Ex(column * (m_ScrollSpeed + RowNumber) + RowNumber + _index);
//                                        _operateItem.gameObject.SetActive(true);

//                                        _intialIndex = arrayListDataSource[column][arrayDataSourceInitialMark[column] + _index];
//                                        FillItemButtonData(_operateItem, _intialIndex, column, arrayDataSourceInitialMark[column] + _index);  //构建列表项
//                                        m_InitialItemCount++;       //更新计数
//                                    }
//                                }
//                            }  //for
//                               //  if (_operateColumn == 0 || column < _operateColumn)
//                            {//只有当整行都影响或者当前行索引小于受影响的行数时
//                                arrayDataSourceInitialMark[column] += _operateRow;  //可能arrayDataSourceInitialMark[column]计数大于实际的 arrayListDataSource[column]元素个数，所以前面要有一个判断
//                            }
//                        }  //for

//                        m_InitialItemCount = DataCount;  //更新计数
//                        ViewItemRole(ScrollPageDirection.DOWN, m_ListPanelRectTrans, _operateRow);
//                        #endregion
//                    }

//                    #endregion
//                    break;
//            }   //SWITCH
//                //    Debug.Log("完成阶段 ScrollPageView   " + gameObject.name);
//        }

//        //实际滚动效果的实现           _Distance !=0标示滑动到最下面使用距离计算
//        protected void ViewItemRole(ScrollPageDirection _Direction, RectTransform _OperateTrans, int _operatecolumn)
//        {
//            Vector2 _currentPosition = _OperateTrans.anchoredPosition;
//            m_ReckMask2D.enabled = true;
//            RectTransform _operateTrans = null;
//            switch (_Direction)
//            {
//                case ScrollPageDirection.LEFT:     //往左显示前的内容 localx++
//                    if (m_HorizontalLayout == false) return;  //垂直布局时不处理这个事件
//                                                              //doTween 实现
//                    _OperateTrans.DOLocalMoveX(_currentPosition.x + _operatecolumn * (itemRect.x + m_ItemSpace.x), m_TweenTime).OnComplete(() =>
//                    {
//                        for (int _row = 0; _row < RowNumber; _row++)
//                        {
//                            for (int _index = 0; _index < _operatecolumn; _index++)
//                            {
//                                m_ListPanelRectTrans.GetChild(_row * (m_ScrollSpeed + ColumnNumber) + ColumnNumber + _index).gameObject.SetActive(false);
//                            }
//                        }  //if
//                        GetLastActiveItemIndex(); //更新计数
//                        m_IscompleteRole = true;
//                        if (isAutoHideMask)
//                            m_ReckMask2D.enabled = false;
//                    });

//                    break;

//                case ScrollPageDirection.RIGHT:       //往右显示后面的内容 localx--
//                    if (m_HorizontalLayout == false) return;  //垂直布局时不处理这个事件

//                    _OperateTrans.DOLocalMoveX(_currentPosition.x - _operatecolumn * (itemRect.x + m_ItemSpace.x), m_TweenTime).OnComplete(() =>
//                    {
//                        //****需要将前面几个不可见的移动到后面去 
//                        for (int _row = 0; _row < RowNumber; _row++)
//                        {
//                            for (int _index = 0; _index < _operatecolumn; _index++)
//                            {
//                                _operateTrans = m_ListPanelRectTrans.Getchild_Ex(_row * (m_ScrollSpeed + ColumnNumber));
//                                if (_operateTrans != null)
//                                {
//                                    _operateTrans.gameObject.SetActive(false);
//                                    _operateTrans.anchoredPosition += new Vector2((m_ScrollSpeed + ColumnNumber) * (m_ItemSpace.x + itemRect.x), 0);   //前面几个整体向右移动一整段距离
//                                    _operateTrans.SetSiblingIndex((_row + 1) * (m_ScrollSpeed + ColumnNumber) - 1);
//                                }
//                            }//for
//                        }  //for
//                        GetLastActiveItemIndex(); //更新计数
//                        m_IscompleteRole = true;
//                        if (isAutoHideMask)
//                            m_ReckMask2D.enabled = false;

//                        //if (m_LayoutViewTransChangeHandle != null)
//                        //    m_LayoutViewTransChangeHandle(_Direction, _operatecolumn * (m_ItemSpace.x + itemRect.x), m_ListPanelRectTrans, true);
//                    });

//                    break;
//                case ScrollPageDirection.UP:
//                    if (m_HorizontalLayout) return;  //水平布局时不处理这个事件
//                    _OperateTrans.DOLocalMoveY(_currentPosition.y - _operatecolumn * (itemRect.y + m_ItemSpace.y), m_TweenTime).OnComplete(() =>
//                    {
//                        for (int _column = 0; _column < ColumnNumber; _column++)
//                        {
//                            for (int _index = 0; _index < _operatecolumn; _index++)
//                            {
//                                m_ListPanelRectTrans.GetChild(_column * (m_ScrollSpeed + RowNumber) + RowNumber + _index).gameObject.SetActive(false);

//                            }
//                        }  //if
//                        GetLastActiveItemIndex(); //更新计数
//                        m_IscompleteRole = true;
//                        if (isAutoHideMask)
//                            m_ReckMask2D.enabled = false;
//                        //if (m_LayoutViewTransChangeHandle != null)
//                        //    m_LayoutViewTransChangeHandle(_Direction, m_ScrollSpeed * (m_ItemSpace.y + itemRect.y), m_ListPanelRectTrans, false);
//                    });


//                    break;
//                case ScrollPageDirection.DOWN:
//                    if (m_HorizontalLayout) return;  //水平布局时不处理这个事件

//                    _OperateTrans.DOLocalMoveY(_currentPosition.y + _operatecolumn * (itemRect.y + m_ItemSpace.y), m_TweenTime).OnComplete(() =>
//                    {
//                        //****需要将上面几个不可见的移动到下面去 
//                        for (int _column = 0; _column < ColumnNumber; _column++)
//                        {
//                            for (int _index = 0; _index < _operatecolumn; _index++)
//                            {
//                                _operateTrans = m_ListPanelRectTrans.Getchild_Ex(_column * (m_ScrollSpeed + RowNumber));
//                                if (_operateTrans != null)
//                                {
//                                    _operateTrans.gameObject.SetActive(false);

//                                    _operateTrans.anchoredPosition -= new Vector2(0, (m_ScrollSpeed + RowNumber) * (m_ItemSpace.y + itemRect.y));   //前面几个整体向右移动一整段距离
//                                    _operateTrans.SetSiblingIndex((_column + 1) * (m_ScrollSpeed + RowNumber) - 1);
//                                }
//                            }
//                        }  //if
//                        GetLastActiveItemIndex(); //更新计数
//                        m_IscompleteRole = true;
//                        if (isAutoHideMask)
//                            m_ReckMask2D.enabled = false;
//                        //if (m_LayoutViewTransChangeHandle != null)
//                        //    m_LayoutViewTransChangeHandle(_Direction, m_ScrollSpeed * (m_ItemSpace.y + itemRect.y), m_ListPanelRectTrans, false);
//                    });

//                    break;
//            }
//        }


//    }
//}
