using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

//2017/7/6 添加m_AllVitewItemRefDic 以便保存对列表项的索引 避免每次 GetComponent
namespace MFramework.UI.Layout
{
    /// <summary>
    ///  翻页方向
    /// </summary>
    public enum ScrollPageDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        None  //初始状态
    }

    public delegate void LayoutViewTransChangeHandle(ScrollPageDirection direction, float distance, RectTransform tracker, bool isLayoutStopMove, bool isSelfControll = true);

    public abstract class BaseLayoutController : MonoBehaviour, ILayoutEvent
    {
        public enum LayoutState
        {
            Idle,
            Sliding
        }
        #region  状态维护

        public bool IsAutoInitial = false; //自己初始化还是被调用InitialLayout初始化
        protected LayoutState m_LayoutState = LayoutState.Idle;
        //  public LayoutEnum m_LayoutEnum = LayoutEnum.Smooth;
        #endregion

        #region 参数
        [Header("布局设置")]
        public bool m_HorizontalLayout = true;  //水平布局只会在左右滚动,列表项从左到右从上到下       垂直布局上下滑动列表项从上到下从左到右
        [Range(2, 200)]
        public float m_MoveSpeed = 200;

        #region 状态以及属性
        //**列表属性
        [Range(1, 20)]
        public int RowNumber = 4;           //Item排列的行数
        [Range(1, 20)]
        public int ColumnNumber = 3;    //Item排列的行数
        [SerializeField]
        protected Vector2 m_ItemSpace = new Vector2(20, 20);//两个元素之间的间距


        protected bool m_IsShowItemEffect = false; //m_IsShowItemEffect=true 标示执行 playEffect ;m_IsShowItemEffect=false 标示不执行
        [SerializeField]
        protected bool isHideViewWhenNotFill = true; //isHideViewWhenNotFill=true表示当元素不足时自动隐藏部分视图 ;m_IsShowItemEffect=false 标示不执行检测
        [SerializeField]
        protected bool isAutoSetMaskSize = true;
        [SerializeField]
        protected bool isAutoHideMask = true;

        //Item 项
        public GameObject m_ItemPrefab;        //列表项的prefab
        public RectTransform m_ListPanelRectTrans;
        public UnityEngine.UI.RectMask2D m_ReckMask2D; //列表项的外部mask


        protected Vector2 itemRect = new Vector2();       //列表项大小

        protected int m_InitialItemCount = 0;          //标示已经初始化的元素个数 *******这里的数值永远>=0;不能设置成-1初始值
        protected int m_ScrollSpeed = 1;  //移动速度 相对于Item+m_ItemSpace的宽度 控制额外生成的列表项的个数
        public bool IsFirstTimeShow { protected set; get; }  //第一次展示才创建Button
        public int LastActiveItemInitialIndex { protected set; get; }  //保存最下面个活动的Item的Index;  默认值应该是-1而不是0

        public List<int>[] arrayListDataSource;//   保存每一行的数据其中的值时源数据的索引 避免消耗过大保存两份数据
        protected int[] arrayDataSourceInitialMark; //标记每一个数据源列表项被实例化的索引数 避免使用一个int 标记而导致的记录混乱    这个数组项用于判断当前列表源列表是否还是有数据没有显示出来
        protected List<BaseLayoutButton> AllViewItemBtnRefences { get; private set; }  //指向创建的列表项的引用
        protected Vector2 listPannelTransInitialPositon;
        protected int _intialIndex = 0;//当前使用的指向源数据的索引

        #endregion

        #endregion

        protected int DataCount = 0; //数据总数
        protected LayoutViewTransChangeHandle m_LayoutViewTransChangeHandle;
        protected int lastTimeMoveCount;  //记录上一次划过的行或者列数  

        #region Event Define
        public Action<BaseLayoutButton> OnLayoutInitial_AfterItemUIButtonCreateAct;  //当列表项Btn被创建时执行
        public Action OnLayoutEffect_ItemDoLastActionAct;  //当布局不需要播放特效时 最后播放面板效果
        public Action OnLayoutEffect_OnPanelIsEmptyAct;   //当面板没有任何元素时候
        #endregion


        #region InitialLayout CreateUIBtn And Setting
        protected virtual void Awake()
        {
            if (IsAutoInitial)
                InitialLayout();
        }


        /// <summary>
        /// 初始化 
        /// </summary>
        public virtual void InitialLayout()
        {
            IsFirstTimeShow = true;
            LayoutInitial_CheckAndSetting();
            LayoutInitial_CreateItemUIButton();
        }

        /// <summary>
        /// 视图初始化检测和相关设置
        /// </summary>
        protected virtual void LayoutInitial_CheckAndSetting()
        {
            if (!IsFirstTimeShow) return;          //只有第一次展示才生成
            if (m_ItemPrefab == null)
            {
                Debug.LogError("Cant get m_ItemPrefab: " + gameObject.name);
                return;
            }
            LastActiveItemInitialIndex = -1;
            RectTransform _itemRect = m_ItemPrefab.transform as RectTransform;
            if (_itemRect == null)
            {
                Debug.LogError("m_ItemPrefab need RectTransform Component");
                return;
            }

            itemRect.y = _itemRect.rect.height;     //列表子项宽
            itemRect.x = _itemRect.rect.width;     //列表子项长

            #region  对mask 和List 布局 **********自动对 mask/ list 居中布局*******
            //  ***根据列表项的大小、行列数、间距计算和调整mask/list位置以及大小
            if (isAutoSetMaskSize)
            {
                Vector2 _containerSize = new Vector2((itemRect.x + m_ItemSpace.x) * ColumnNumber, (itemRect.y + m_ItemSpace.y) * RowNumber);  //计算外部 mask 和list 大小
                RectTransform rectMask = m_ReckMask2D.transform as RectTransform;
                rectMask.anchorMax = Vector2.one / 2f;
                rectMask.anchorMin = Vector2.one / 2f;
                rectMask.sizeDelta = _containerSize;   //调整mask 的size 大小

                m_ListPanelRectTrans.anchorMax = Vector2.one / 2f;
                m_ListPanelRectTrans.anchorMin = Vector2.one / 2f;
                m_ListPanelRectTrans.sizeDelta = _containerSize;  //调整list mask 大小
                m_ListPanelRectTrans.anchoredPosition = Vector2.zero;    //调整list 的位置
            }

            listPannelTransInitialPositon = m_ListPanelRectTrans.anchoredPosition;   //更新list 保存的初始位置

            #endregion

            #region  RowNumber/ColumnNumber参数设置检测
            if (RowNumber < 1 || ColumnNumber < 1)
            {
                Debug.LogError("LayoutInitialCheckAndSetting error: " + RowNumber + "  " + ColumnNumber + gameObject.name);
                return;
            }
            if (m_HorizontalLayout)
            {
                if (m_ScrollSpeed < 1 || m_ScrollSpeed > ColumnNumber)
                {
                    Debug.LogError(gameObject.name + "    m_ScrollSpeed must rather than 1");
                    m_ScrollSpeed = 1;
                }
            }
            else
            {
                if (m_ScrollSpeed < 1 || m_ScrollSpeed > RowNumber)
                {
                    Debug.LogError(gameObject.name + "    m_ScrollSpeed must rather than 1");
                    m_ScrollSpeed = 1;
                }
            }
            #endregion

            #region 初始化相关容器
            if (m_HorizontalLayout)
            {//水平布局
                arrayListDataSource = new List<int>[RowNumber];
                arrayDataSourceInitialMark = new int[RowNumber];
                for (int _index = 0; _index < RowNumber; ++_index)
                    arrayListDataSource[_index] = new List<int>(); //*****必须new 否则没有申请存储空间
            }
            else
            {
                arrayListDataSource = new List<int>[ColumnNumber];
                arrayDataSourceInitialMark = new int[ColumnNumber];
                for (int _index = 0; _index < ColumnNumber; ++_index)
                    arrayListDataSource[_index] = new List<int>(); //*****必须new 否则没有申请存储空间
            }//
            #endregion

        }//IF

        /// <summary>
        /// 生成面板时创建 ItemButton，后面不再重新创建 ,初始创建时列表项以及
        /// 用于滚动的额外列表项已经排列整齐，并且不可见状态
        /// </summary>
        protected void LayoutInitial_CreateItemUIButton()
        {
            if (!IsFirstTimeShow) return;          //只有第一次展示才生成
            IsFirstTimeShow = false;
            if (m_HorizontalLayout)
                AllViewItemBtnRefences = new List<BaseLayoutButton>(RowNumber * (ColumnNumber + m_ScrollSpeed));  //创建保存可视区域UI按钮的引用数组
            else
                AllViewItemBtnRefences = new List<BaseLayoutButton>(ColumnNumber * (RowNumber + m_ScrollSpeed));


            #region  普通的一次全部创建列表项视图
            RectTransform _newItemButton;
            int _xIndex, _yIndex;
            int itemBtnTotalCount = 0; //需要创建的ItemButton 数量
            if (m_HorizontalLayout)
                itemBtnTotalCount = RowNumber * (ColumnNumber + m_ScrollSpeed);
            else
                itemBtnTotalCount = ColumnNumber * (RowNumber + m_ScrollSpeed);

            for (int _index = 0; _index < itemBtnTotalCount; ++_index)
            {
                _newItemButton = (Instantiate(m_ItemPrefab, m_ListPanelRectTrans) as GameObject).transform as RectTransform;
                _newItemButton.gameObject.SetActive(true);
                _newItemButton.gameObject.name = "Item" + _index;
                _newItemButton.localScale = Vector3.one;
                _newItemButton.localRotation = Quaternion.identity;

                if (m_HorizontalLayout)
                { //从左向右 从上到下创建
                    _xIndex = _index / (ColumnNumber + m_ScrollSpeed);             //显示的行索引
                    _yIndex = _index % (ColumnNumber + m_ScrollSpeed);              //列索引 
                }
                else
                { //从上到下 从左到右创建
                    _xIndex = _index % (RowNumber + m_ScrollSpeed);    //显示行的索引
                    _yIndex = _index / (RowNumber + m_ScrollSpeed);   //列索引 
                }

                _newItemButton.anchoredPosition = new Vector2(_yIndex * (itemRect.x + m_ItemSpace.x) + itemRect.x * 0.5f, -1 * _xIndex * (itemRect.y + m_ItemSpace.y) - itemRect.y * 0.5f)
                    + new Vector2(-1 * m_ListPanelRectTrans.sizeDelta.x / 2f, m_ListPanelRectTrans.sizeDelta.y / 2f);

                BaseLayoutButton _itemButton = _newItemButton.GetComponent<BaseLayoutButton>();
                LayoutInitial_AfterItemUIButtonCreate(_itemButton, _index, _xIndex, _yIndex);
                _newItemButton.gameObject.SetActive(false);
                AllViewItemBtnRefences.Add(_itemButton);
            }
            //Debug.Log("完成阶段 CreateItemButton   " +gameObject.name);
            #endregion
        }

        /// <summary>
        /// 列表项被创建出来之后执行的操作 一般用于进行初始化设置和保存数据
        /// </summary>
        /// <typeparam name="TButton"></typeparam>
        /// <param name="_operateButton"></param>
        /// <param name="_index"></param>
        /// <param name="_rowIndex"></param>
        /// <param name="_columnIndex"></param>
        protected virtual void LayoutInitial_AfterItemUIButtonCreate(BaseLayoutButton _itemButton, int _index, int _rowIndex, int _columnIndex)
        {
            if (_itemButton == null)
            {
                Debug.LogError("LayoutInitial_AfterItemUIButtonCreate Fail,No BaseLayoutButton ComPonent");
                return;
            }
            _itemButton.StorePosition(_itemButton.rectTransform.anchoredPosition, _itemButton.rectTransform.TransformPoint(Vector3.zero), _index, _index);//获得要显示时的局部坐标和世界坐标

            if (OnLayoutInitial_AfterItemUIButtonCreateAct != null)
                OnLayoutInitial_AfterItemUIButtonCreateAct(_itemButton);  //执行自定义初始化逻辑
        }


        #endregion

        #region DataSource Controll 
        /// <summary>
        /// 保存数据源信息的索引到List<BaseItemData>[]  ArrayListDataSource
        /// </summary>
        protected void SavaDataSource(int _dataCount)
        {
            DataCount = _dataCount;
            for (int _index = 0; _index < arrayListDataSource.Length; ++_index)
                arrayListDataSource[_index].Clear(); //清除列表数据源

            if (m_HorizontalLayout)
            {
                #region 水平布局   数据必须按照一行一行的排列，只允许最后一列的数据从上到下排列，两行数据之间最多相差1
                int _totalColumnCount_FullData = DataCount / RowNumber;  // 得到完全填充满的数据行数 (每个列表中平均的数据源对象个数，允许前面部分列表比后面的多1个)
                int _addtionanlRowCount = DataCount - _totalColumnCount_FullData * RowNumber; //计算最后不足一列的元素个数，将他们分别放在前面_addtionanlRownCount个列表中最后一项
                int _listCountNumber = _totalColumnCount_FullData; //标示每一行对应的列表要填充的元素个数，部分列表项相差1
                int _initialCount = 0;  //已经处理的数据个数

                #region 数据显示方式1 按照每一行都是从左到右排列的

                //for (int _rowCount = 0; _rowCount < RowNumber; ++_rowCount)
                //{ //按照从左到右 从上到下一行一行的填充数据
                //    if (_rowCount < _addtionanlRowCount)
                //        _listCountNumber = _totalRowCount_FullData + 1; //对于前面 _addtionanlRownCount行数据，数据列表中保存的元素个数比平均数多1
                //    else
                //        _listCountNumber = _totalRowCount_FullData;

                //    for (int _index = 0 + _initialCount; _index < _listCountNumber + _initialCount; ++_index)
                //        arrayListDataSource[_rowCount].Add(_index);
                //    _initialCount += arrayListDataSource[_rowCount].Count; //更新计数
                //}//for

                #endregion

                #region 数据显示方式2  按照每一列都是从上到下排序

                for (int dataColumn = 0; dataColumn < _totalColumnCount_FullData + _addtionanlRowCount; ++dataColumn)
                { //新的数据填充方式 按照从上到下, 从左到右  一列一列的处理填充数据
                    for (int _rowCount = 0; _rowCount < RowNumber; ++_rowCount)
                    {
                        if (_initialCount == DataCount)
                        { //数据处理完成
                            Debug.Log("SavaDataSource Finish ! dataColumn=" + dataColumn + "   _rowCount=" + _rowCount);
                            return;
                        }
                        arrayListDataSource[_rowCount].Add(_initialCount);
                        ++_initialCount;
                    }
                }
                #endregion

                #endregion
            }//if
            else
            {
                #region 垂直布局  数据必须按照一列一列的排列，只允许最后一行的数据从左到右排列，两列数据之间个数最多相差1
                int _totalRowCount_FullData = DataCount / ColumnNumber;  //得到完全填充满的数据列数(每个列表中平均的数据源对象个数，允许前面部分行列表比后面的多1个)
                int _addtionanlcolumnCount = DataCount - _totalRowCount_FullData * ColumnNumber; //计算最后不足一行的元素个数，将他们分别放在前面_addtionanlRownCount个列表中最后一项
                int _listCountNumber = _totalRowCount_FullData; //标示每一行对应的列表要填充的元素个数，部分列表项相差1
                int _initialCount = 0;  //已经处理的数据个数

                #region 数据显示方式1 每一列都是按照从上到下顺序拍下去的 , 

                //for (int _columnCount = 0; _columnCount < ColumnNumber; ++_columnCount)
                //{
                //    if (_columnCount < _addtionanlcolumnCount)
                //        _listCountNumber = _totalRowCount_FullData + 1; //对于前面 _addtionanlRownCount行数据，数据列表中保存的元素个数比平均数多1
                //    else
                //        _listCountNumber = _totalRowCount_FullData;

                //    for (int _index = 0 + _initialCount; _index < _listCountNumber + _initialCount; ++_index)
                //        arrayListDataSource[_columnCount].Add(_index);

                //    _initialCount += arrayListDataSource[_columnCount].Count; //更新计数

                //    //string msg = "";
                //    //for (int dex = 0; dex < arrayListDataSource[_columnCount].Count; ++dex)
                //    //{
                //    //    msg += " dex=" + dex + " ::" + arrayListDataSource[_columnCount][dex];
                //    //}
                //    //Debug.Log(msg);
                //    //msg = "";
                //}//for
                #endregion

                // Debug.Log("AAAAAAAAAAAAAA " + _dataCount+ " :: _totalRowCount_FullData="+ _totalRowCount_FullData+ "_addtionanlcolumnCount "+ _addtionanlcolumnCount);
                #region 数据显示方式2  每一行都是从左到右顺序排列
                for (int dataRow = 0; dataRow < _totalRowCount_FullData + _addtionanlcolumnCount; ++dataRow)
                { //新的数据填充方式 按照 从左到右 从上到下  一行一行的处理填充数据
                    for (int _column = 0; _column < ColumnNumber; ++_column)
                    {
                        if (_initialCount == DataCount)
                        { //数据处理完成
                            //Debug.Log("SavaDataSource Finish ! dataRow=" + dataRow + "   _column=" + _column);
                            return;
                        }
                        arrayListDataSource[_column].Add(_initialCount);
                        ++_initialCount;
                    }
                }
                #endregion

                #endregion
            }
        }
        #endregion


        #region Reset And Clear 

        /// <summary>
        /// 重构视图
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_isClearDataSource"></param>
        /// <param name="_isSavingTransReference"></param>
        public virtual void ReBuildView(int _dataCount, bool _isClearDataSource, bool _isSavingTransReference = true)
        {
            if (IsFirstTimeShow)
            {
                Debug.LogError("ReBuildView Fail,First Should Initial Layout....");
                return;
            }
            Reset_PanelLayout();  //需要考虑是否需要这个TODO***********************
            Reset_ClearData(_isSavingTransReference, _isClearDataSource, true);  //重构数据源
            SavaDataSource(_dataCount);  //重新分配数据源
            //Debug.Log("ReBuildView  lastTimeMoveCount=" + lastTimeMoveCount);
            ShowLayoutWithSpecialRowOrColumn(lastTimeMoveCount);

            GetLastActiveItemIndex();  //获得最下面活动Index
            if (m_IsShowItemEffect)
            {
                LayoutEffect_PanelEffect(true);  //播放面板打开动画
            }
        }


        /// <summary>
        /// 重置面板位置和列表项的索引位置和顺序
        /// </summary>
        protected void Reset_PanelLayout()
        {
            if (m_ListPanelRectTrans == null) return;    //当m_ListPanelRectTrans被场景管理且在切换场景时候无法获取对象 这里是异常处理
            m_ListPanelRectTrans.anchoredPosition = listPannelTransInitialPositon; //恢复初始位置
            for (int _index = 0; _index < AllViewItemBtnRefences.Count; ++_index)
            {
                AllViewItemBtnRefences[_index].ResetLayoutItem();
            }//for
        }

        /// <summary>
        /// 清理数据和引用
        /// </summary>
        /// <param name="isSaveUIBtnTrans">是否保留创建的UIBtn,默认True</param>
        /// <param name="isClearDataSource">是否保存数据源</param>
        /// <param name="isClearInitialRecord">是否保留初始化个数记录</param>
        protected virtual void Reset_ClearData(bool isSaveUIBtnTrans = true, bool isClearDataSource = true, bool isClearInitialRecord = true)
        {
            if (isClearDataSource)
            {
                for (int _index = 0; _index < arrayListDataSource.Length; ++_index)
                    arrayListDataSource[_index].Clear(); //清除列表数据源
            }

            if (isClearInitialRecord)
            {
                m_InitialItemCount = 0; //清空初始化个数的引用
                for (int _index = 0; _index < arrayDataSourceInitialMark.Length; ++_index)
                    arrayDataSourceInitialMark[_index] = 0;  //清空每一个行或者列初始化的个数
            }

            if (isSaveUIBtnTrans == false)
            {//保存创建对创建的Button 的引用
                AllViewItemBtnRefences.Clear();   //清空对创建的button的索引
                int _listChildCout = m_ListPanelRectTrans.childCount;
                for (int _index2 = 0; _index2 < _listChildCout; ++_index2)
                    DestroyImmediate(m_ListPanelRectTrans.GetChild(0));  //立刻销毁m_list下面创建的button
                IsFirstTimeShow = true;   //清空了所有的数据包括创建的item 下次重新从头创建初始化
            }

            LastActiveItemInitialIndex = -1;
        }

        #endregion

        #region ShowLayout

        /// <summary>
        /// 根据指定的行列数显示布局
        /// </summary>
        protected virtual void ShowLayoutWithSpecialRowOrColumn(int number) { }

        /// <summary>
        /// 按照给定的行(列)数据设置列表项的位置
        /// </summary>
        /// <param name="_moveCount"></param>
        protected void ResetItemState(int _moveCount)
        {
            if (m_HorizontalLayout)
            {
                for (int _index = 0; _index < RowNumber * (ColumnNumber + m_ScrollSpeed); ++_index)
                    AllViewItemBtnRefences[_index].rectTransform.anchoredPosition += new Vector2(_moveCount * (itemRect.x + m_ItemSpace.x), 0);
                return;
            }
            for (int _index = 0; _index < ColumnNumber * (RowNumber + m_ScrollSpeed); ++_index)
                AllViewItemBtnRefences[_index].rectTransform.anchoredPosition -= new Vector2(0, _moveCount * (itemRect.y + m_ItemSpace.y));
        }


        /// <summary>
        /// 获得最下面一个活动的Item的Index
        /// </summary>
        protected void GetLastActiveItemIndex()
        {
            if (m_ListPanelRectTrans == null)
            {
                Debug.Log("GetLastActiveItemIndex Fail... m_ListPanelRectTrans is null");
                return;
            }
            //应该记录的时最下面一个可见对象的创建时的索引而不是在m_ListPanelRectTrans中的索引
            int _activeIndex = -1;
            for (int _index = 0; _index < m_ListPanelRectTrans.childCount; ++_index)
            {
                if (m_ListPanelRectTrans.GetChild(_index).gameObject.activeSelf)
                    _activeIndex = _index;
            }
            if (_activeIndex == -1)
                LastActiveItemInitialIndex = -1;
            else
            {
                LastActiveItemInitialIndex = m_ListPanelRectTrans.GetChild(_activeIndex).GetComponent<BaseLayoutButton>().m_ItemIndex;
                //     Debug.Log("最下面活动的item 索引是 "+ LastActiveItemInitialIndex);
            }
            //Debug.Log("完成阶段 GetLastActiveItemInitialIndexInParent   " + gameObject.name);
        }

        /// <summary>
        /// 填充具体的Button的视图
        /// </summary>
        /// <param name="_operateTrans">操作对象</param>
        /// <param name="_data">数据参数</param>
        protected void FillItemButtonData(RectTransform _operateTrans, int _dateIndex, int _relativeIndexList, int _relativeIndexItem)
        {
            BaseLayoutButton button = _operateTrans.GetComponent<BaseLayoutButton>();
            if (button == null) return;
            button.InitialButtonItem(this, _dateIndex, _relativeIndexList, _relativeIndexItem);
        }

        protected BaseLayoutButton GetItemBaseLayoutButtonComponent(int searchDataIndex)
        {
            for (int dex = 0; dex < AllViewItemBtnRefences.Count; ++dex)
            {
                if (AllViewItemBtnRefences[dex].m_DataIndex == searchDataIndex)
                    return AllViewItemBtnRefences[dex];
            }

            Debug.LogError("GetItemBaseLayoutButtonComponent  Fail,Not Exit" + searchDataIndex);
            return null;
        }

        #endregion


        #region 列表效果

        /// <summary>
        /// 面板出现与消失的子Item动画   当面板被关闭时必须调用，内部包含当m_IsShowItemEffect==false的时候的处理逻辑
        /// </summary>
        /// <param name="_isOpen"></param>
        protected virtual void LayoutEffect_PanelEffect(bool _isOpen)
        {
            #region   实现
            if (isAutoHideMask)
                m_ReckMask2D.enabled = false;

            if (LastActiveItemInitialIndex == -1)
            {
                //Debug.Log(gameObject.name+"当前面板没有元素");
                LayoutEffect_OnPanelIsEmpty(_isOpen);   //当列表项为空时调用
                return;
            }
            if (m_IsShowItemEffect)
            {
                LayoutEffect_ItemEffect(_isOpen);
            }// 显示子项的特效
            else
            {
                //Debug.Log("m_IsShowItemEffect " + m_IsShowItemEffect + "直接完成" + gameObject.name);
                LayoutEffect_ItemDoLastAction(_isOpen);
            } //直接播放最后的效果
            //Debug.Log("完成阶段 PlayPannelEffect   " + gameObject.name);
            #endregion
        }

        /// <summary>
        /// 列表项布局特效 子项在打开和关闭时候特效
        /// </summary>
        /// <param name="_isOpen"></param>
        protected virtual void LayoutEffect_ItemEffect(bool _isOpen)
        {
            int _delayTimeIndex = 0;  //顺序
            float delayTime = 0.02f;   //延时的单位时间
            for (int _index = 0; _index < AllViewItemBtnRefences.Count; ++_index)
            {
                if (AllViewItemBtnRefences[_index] != null)
                {
                    if (AllViewItemBtnRefences[_index].gameObject.activeSelf)
                    {//避免不可见的item也执行tween影响性能 某些情况下这个判断不能缺少，因为没有显示的项没有初始化 显示的Item执行动画
                        ++_delayTimeIndex;
                        AllViewItemBtnRefences[_index].PlayEffect(_isOpen, delayTime * _delayTimeIndex);
                    }
                    else
                        LayoutEffect_EffectOfHideItem(AllViewItemBtnRefences[_index], _isOpen);  //由于隐藏的项可能没有初始化不能正确的执行PlayEffect ，所以这里需要自定义其他操作
                }
            }//for
        }


        /// <summary>
        ///在执行PlayPannelEffect 时隐藏项执行的操作 可以为空
        ///用于特定条件下自定义了Item 操作而需要恢复
        /// </summary>
        /// <param name="_operateItemTrans">操作的项</param>
        /// <param name="_isOpen"></param>
        protected virtual void LayoutEffect_EffectOfHideItem(BaseLayoutButton _itemButton, bool _isOpen)
        {
            if (_isOpen)   //需要考虑是否有必要执行
            {  //由于创建时有缩放 在这里每次打开时恢复正常大小 避免无法显示
                _itemButton.rectTransform.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// 当不需要使用列表项特效时 直接播放最后的效果
        /// </summary>
        /// <param name="_isFinishAction"></param>
        public virtual void LayoutEffect_ItemDoLastAction(bool _isFinishAction)
        {
            if (_isFinishAction)
            {

            }//if
            else
            {
                Reset_PanelLayout();  //恢复页面到初始状态
                if (OnLayoutEffect_ItemDoLastActionAct != null)
                    OnLayoutEffect_ItemDoLastActionAct();  //当关闭面板时
            }
        }

        /// <summary>
        /// 当项要播放子项特效 而列表为空时
        /// </summary>
        /// <param name="_isOpen"></param>
        protected virtual void LayoutEffect_OnPanelIsEmpty(bool _isOpen)
        {
            if (_isOpen == false)
            {
                if (OnLayoutEffect_OnPanelIsEmptyAct != null)
                    OnLayoutEffect_OnPanelIsEmptyAct();  //当面板关闭 时候
            }
        }




        #endregion


        #region 列表的滑动事件处理逻辑

        /// <summary>
        /// *****父类实现 IScrollHandler 滑动接口，但是子类必须自己继承这个接口
        /// </summary>
        /// <param name="eventData"></param>
        public void OnScroll(PointerEventData eventData)
        {
            if (eventData == null)
                return;

            //************需要考虑是否添加条件以便于控制页面在某些时候静止滑动;TODO
            //Debug.Log("eventData.scrollDelta " + eventData.scrollDelta + "    m_MoveSpeed=" + m_MoveSpeed);
            if (m_HorizontalLayout)
            {
                if (eventData.scrollDelta.x < 0)
                {
                    Scroll_OnScrollLayoutView(ScrollPageDirection.LEFT, m_MoveSpeed);
                    //  Debug.Log("左");
                }
                else if (eventData.scrollDelta.x > 0)
                {
                    Scroll_OnScrollLayoutView(ScrollPageDirection.RIGHT, m_MoveSpeed);
                    //    Debug.Log("右");
                }
                return;
            }

            if (eventData.scrollDelta.y < 0)
            {
                Scroll_OnScrollLayoutView(ScrollPageDirection.DOWN, m_MoveSpeed);
                //      Debug.Log("下");
            }
            else if (eventData.scrollDelta.y > 0)
            {
                Scroll_OnScrollLayoutView(ScrollPageDirection.UP, m_MoveSpeed);
                //     Debug.Log("上");
            }
        }

        /// <summary>
        /// 滑动视图页面
        /// </summary>
        /// <param name="_Direction">滚动的方向 需要与m_HorizontalLayout对应，否则操作被忽略</param>
        public abstract void Scroll_OnScrollLayoutView(ScrollPageDirection _Direction, float _moveSpeed);

        /// <summary>
        /// 强制列表停止滚动
        /// </summary>
        protected abstract void Scroll_ForceStopScroll();

        #endregion


        #region  页面提示栏视图  滑块
        /// <summary>
        /// 创建页面页数提示视图
        /// </summary>
        protected virtual void CreatePageTip() { }

        /// <summary>
        /// 更新页面提示视图
        /// </summary>
        protected virtual void ChangePageTipView() { }


        /// <summary>
        /// 检查面板部分按钮等是否需要显示
        /// </summary>
        protected virtual void CheckWhetherNeedToShow() { }


        #endregion


        #region 处理项列表中动态添加和删除数据是列表项的更新逻辑
        public virtual void DataModelChange<T>(CollectionEvent collEvent, IList<T> _newData, int _dex, IList<T> _dataSource)
        {
            int maxValue = 0;
            if (m_HorizontalLayout)
                maxValue = (m_InitialItemCount + RowNumber - 1) / RowNumber * RowNumber;
            else
                maxValue = (m_InitialItemCount + ColumnNumber - 1) / ColumnNumber * ColumnNumber;

            maxValue = Mathf.Max(maxValue, ColumnNumber * RowNumber);

            List<object> _ShowDataList = new List<object>(_dataSource.Count);
            for (int dex = 0; dex < _dataSource.Count; ++dex)
                _ShowDataList.Add(_dataSource[dex]);

            //Debug.Log(minValue + "    " + maxVaue + "   " + m_InitialItemCount);
            switch (collEvent)
            {
                case CollectionEvent.AddRang:
                    if (m_LayoutState != LayoutState.Idle)
                    {//Record
                        Scroll_ForceStopScroll();
                        DataModelChange<T>(collEvent, _newData, _dex, _dataSource);
                        return;
                    }
                    if (m_ListPanelRectTrans == null)
                    {//*************************未知原因
                        Debug.Log("*************************Not expected    *****************  m_ListPanelRectTrans=null " + gameObject);
                        return;
                    }
                    ReBuildView(_ShowDataList.Count, true, true);
                    break;
                case CollectionEvent.DeleteRangle:
                    if (m_LayoutState != LayoutState.Idle)
                    {//Record
                        Scroll_ForceStopScroll();
                        DataModelChange<T>(collEvent, _newData, _dex, _dataSource);
                        return;
                    }
                    ReBuildView(_ShowDataList.Count, true, true);
                    break;
                case CollectionEvent.Clear:
                    ReBuildView(_ShowDataList.Count, true, true);
                    Scroll_ForceStopScroll();
                    break;
                case CollectionEvent.Flush:
                    for (int dex = 0; dex < AllViewItemBtnRefences.Count; ++dex)
                        AllViewItemBtnRefences[dex].FlushView();
                    break;
            }

        }

        /// <summary>
        /// 数据层有一个对象改变时候
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collEvent"></param>
        /// <param name="newData"></param>
        /// <param name="_dex"></param>
        /// <param name="_data"></param>
        /// <param name="sourceData"></param>
        public virtual void DataModelChangeSignal<T>(CollectionEvent collEvent, T newData, int _dex, T _data, IList<T> sourceData)
        {
            int maxVaue = 0;  //获得当前能显示的数据最大数量(在这个范围内的数据改变需要刷新，否则只需要更新数据源数据即可)
            if (m_HorizontalLayout)
                maxVaue = (m_InitialItemCount + RowNumber - 1) / RowNumber * RowNumber;
            else
                maxVaue = (m_InitialItemCount + ColumnNumber - 1) / ColumnNumber * ColumnNumber;

            maxVaue = Mathf.Max(maxVaue, ColumnNumber * RowNumber); //最小的显示区域既是初始设置的显示区域

            RectTransform item = null;
            //   Debug.Log(_dex+ " maxVaue= " +maxVaue + "   " + m_InitialItemCount);
            switch (collEvent)
            {
                case CollectionEvent.AddSignal:
                    #region AddSignal
                    if (m_LayoutState != LayoutState.Idle)
                    {
                        Scroll_ForceStopScroll();
                        DataModelChangeSignal<T>(collEvent, newData, _dex, _data, sourceData);
                        return;
                    }
                    #region AddSignal
                    if (_dex >= maxVaue)
                    {
                        //Debug.Log("新数据不再显示区域不需要重构" + _dex + " m_InitialItemCount=" + m_InitialItemCount);
                        SavaDataSource(sourceData.Count);
                        return;
                    } //新数据不再显示区域不需要重构 
                    ReBuildView(sourceData.Count, true, true);
                    #endregion
                    #endregion
                    break;
                case CollectionEvent.DeleteSignal:
                    #region DeleteSignal
                    if (m_LayoutState != LayoutState.Idle)
                    {
                        Scroll_ForceStopScroll();
                        DataModelChangeSignal<T>(collEvent, newData, _dex, _data, sourceData);
                        return;
                    }
                    if (_dex > maxVaue)
                    {
                        //    Debug.Log("新数据不再显示区域不需要重构" + _dex + " m_InitialItemCount=" + m_InitialItemCount);
                        Reset_ClearData(true, true, false);  //重构数据源
                        SavaDataSource(sourceData.Count);
                        return;
                    } //新数据不再显示区域不需要重构
                    ReBuildView(sourceData.Count, true, true);
                    #endregion
                    break;
                case CollectionEvent.Update:
                    BaseLayoutButton button = GetItemBaseLayoutButtonComponent(_dex);
                    if (button == null)
                    {
                        Debug.LogError("Update Fail Miss " + _dex);
                        return;
                    }
                    if (button.gameObject.activeSelf)
                    {
                        button.UpdateData(collEvent, newData);
                        return;
                    }

                    #region 旧的处理方式


                    //#region Horizontal
                    //if (m_HorizontalLayout)
                    //{
                    //    for (int row = 0; row < RowNumber; ++row)
                    //    {
                    //        for (int _column = 0; _column < ColumnNumber + m_ScrollSpeed; ++_column)
                    //        {
                    //            item = m_ListPanelRectTrans.Getchild_Ex(row * (ColumnNumber + m_ScrollSpeed) + _column);
                    //            BaseLayoutButton button = GetItemBaseLayoutButtonComponent(item);
                    //            if (button == null)
                    //            {
                    //                Debug.LogError("Miss " + _dex);
                    //                continue;
                    //            }

                    //            if (button.m_DataIndex == _dex && item.gameObject.activeSelf)
                    //            {
                    //                button.UpdateData(collEvent, newData);
                    //                //     Debug.Log(_column + "更新数据 " + item.gameObject.name);
                    //                return;
                    //            }
                    //        }
                    //    }
                    //    return;
                    //}
                    //#endregion

                    //#region Vertical
                    //for (int _column = 0; _column < ColumnNumber; ++_column)
                    //{
                    //    for (int row = 0; row < RowNumber; ++row)
                    //    {
                    //        item = m_ListPanelRectTrans.Getchild_Ex(_column * (RowNumber + m_ScrollSpeed) + row);
                    //        BaseLayoutButton button = GetItemBaseLayoutButtonComponent(item);
                    //        if (button == null)
                    //        {
                    //            Debug.LogError("Miss " + _dex);
                    //            continue;
                    //        }

                    //        if (button.m_DataIndex == _dex && item.gameObject.activeSelf)
                    //        {
                    //            //   Debug.Log(_column + "更新数据 " + item.gameObject.name);
                    //            button.UpdateData(collEvent, newData);
                    //            return;
                    //        }
                    //    }
                    //}
                    //#endregion
                    #endregion

                    break;
            }
        }




        #endregion


        #region Event

        /// <summary>
        /// Add Listenner To LayoutViewTransChangeHandle when List Scroll It While Hear
        /// </summary>
        /// <param name="handle"></param>
        public void AddLayoutViewTransChangeListener(LayoutViewTransChangeHandle handle)
        {
            m_LayoutViewTransChangeHandle = Delegate.Combine(m_LayoutViewTransChangeHandle, handle) as LayoutViewTransChangeHandle;
        }

        public void RemoveLayoutViewTransChangeListener(LayoutViewTransChangeHandle handle)
        {
            m_LayoutViewTransChangeHandle = Delegate.Remove(m_LayoutViewTransChangeHandle, handle) as LayoutViewTransChangeHandle;
        }



        #endregion

    }
}
