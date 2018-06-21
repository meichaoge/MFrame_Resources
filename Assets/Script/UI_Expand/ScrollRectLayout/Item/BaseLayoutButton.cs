using UnityEngine;
using System.Collections;
using System;

namespace MFramework.UI.Layout
{
    public class BaseLayoutButton : MonoBehaviour, ILayerItem
    {
        public RectTransform rectTransform { get { return transform as RectTransform; } }

        public int m_ItemIndex = 0;
        protected Vector3 intialWorldPosition;
        [SerializeField]
        private int _dataIndex;
        public int m_DataIndex
        {
            get { return _dataIndex; }
            protected set { _dataIndex = value; }
        }

        public LayoutScrollView LayoutScrollViewScript { get; protected set; }

        private Vector2 IntialLocal;
        public Vector2 m_IntialLocal
        {
            get { return IntialLocal; }
            protected set { IntialLocal = value; }
        }
        [SerializeField]
        private SerelizeVector2_Int _viewRelativeIndex = new SerelizeVector2_Int(-1, -1);
        public SerelizeVector2_Int ViewRelativeIndex
        {
            get { return _viewRelativeIndex; }
            set { _viewRelativeIndex = value; }
        }




        /// <summary>
        /// 按钮项初始化
        /// </summary>
        /// <param name="_sender">穿件的View</param>
        /// <param name="_data"></param>
        public virtual void InitialButtonItem(object _sender, int _dataIndex, int _relativeIndexList = -1, int _relativeIndexItem = -1)
        {
            m_DataIndex = _dataIndex;
            ViewRelativeIndex = new SerelizeVector2_Int(_relativeIndexList, _relativeIndexItem);
            LayoutScrollViewScript = _sender as LayoutScrollView;
        }
        public void StorePosition(Vector2 _position, Vector3 _worldPosition, int _itemIndex, int _dataIndex)
        {
            m_IntialLocal = _position;
            intialWorldPosition = _worldPosition;
            m_ItemIndex = _itemIndex;
            m_DataIndex = _dataIndex;
            //  Debug.Log(gameObject.name+"  "+ _position +"    "+ _worldPosition);
        }
        /// <summary>
        /// 列表项特效
        /// </summary>
        /// <param name="_isOpen"></param>
        /// <param name="delayTime">延迟打开时间</param>
        public virtual void PlayEffect(bool _isOpen, float delayTime = 0f) { }

        public virtual void UpdateData(CollectionEvent itemEvent, object data)
        {
            switch (itemEvent)
            {
                case CollectionEvent.Update:
                    FlushView();
                    break;
                default:
                    Debug.LogError("UpdateData Fail.....Not Define " + itemEvent);
                    break;
            }
        }

        /// <summary>
        /// 当View 调用Reset_PanelLayout 时候重置每一个Item
        /// </summary>
        public void ResetLayoutItem()
        {
            rectTransform.anchoredPosition = m_IntialLocal; //恢复到初始生成位置
            rectTransform.SetSiblingIndex(m_ItemIndex); //恢复到初始生成顺序
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        public virtual void FlushView()
        {

        }


    }
}
