using UnityEngine;
using System.Collections;

namespace MFramework.UI.Layout
{
    public interface ILayerItem
    {
        int m_DataIndex { get; }
        Vector2 m_IntialLocal { get; }

        LayoutScrollView LayoutScrollViewScript { get; }

        void InitialButtonItem(object _sender, int _dataIndex, int _relativeIndexList = -1, int _relativeIndexItem = -1);


        void StorePosition(Vector2 _position, Vector3 _worldPosition, int _itemIndex, int _dataIndex);

        /// <summary>
        /// update the item view
        /// </summary>
        /// <param name="itemEvent"></param>
        /// <param name="data">new data</param>
        void UpdateData(CollectionEvent itemEvent, object data);



    }
}
