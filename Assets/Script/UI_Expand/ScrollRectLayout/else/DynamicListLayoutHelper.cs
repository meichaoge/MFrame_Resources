using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MFramework.UI.Layout
{

    public static class DynamicListLayoutHelper
    {
        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="_list">要操作的列表</param>
        /// <param name="_prefab">预制体</param>
        /// <param name="_destinationCount">目标个数</param>
        public static void UpdateList(RectTransform _list, GameObject _prefab, int _destinationCount)
        {
            int _countIndex = 0;
            if (_list.childCount == _destinationCount)
            { ///数量相符合
            }
            else if (_list.childCount < _destinationCount)
            {//需要创建
                GameObject _newObj = null;
                for (_countIndex = _list.childCount; _countIndex < _destinationCount; ++_countIndex)
                {
                    _newObj = GameObject.Instantiate(_prefab, _list) as GameObject;
                    _newObj.transform.localScale = Vector3.one;
                    _newObj.transform.localRotation = Quaternion.identity;
                    _newObj.transform.localPosition = Vector3.zero;
                }
            }//else if
            else
            {
                for (_countIndex = _destinationCount; _countIndex < _list.childCount; ++_countIndex)
                {  //当列表项个数比需要的多时候的操作
                    _list.GetChild(_countIndex).GetChild(0).gameObject.SetActive(false); //隐藏中间标示
                    _list.GetChild(_countIndex).gameObject.SetActive(false);

                }
            }//else

            for (_countIndex = 0; _countIndex < _destinationCount; ++_countIndex)
            {
                _list.GetChild(_countIndex).gameObject.SetActive(true);
            }

        }

    }
}