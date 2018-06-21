using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.Data
{
    /// <summary>
    /// 直接插入排序 时间复杂度(O(n^2))
    /// 基本思想 将带排序的数据依次找到已经排序号的数据中插入的位置
    /// </summary>
    public class InsertSortHelper
    {
        /// <summary>
        /// 插入排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSources"></param>
        /// <param name="handle"></param>
        public static void Sort<T>(List<T> dataSources, SortCompareHandle<T> handle)
        {
            if (SortHelper.CheckParameter<T>(dataSources, handle) == false)
                return;

            for (int i = 1; i < dataSources.Count; i++)
            {
                for (int j = i - 1; j >= 0 && handle(dataSources[j], dataSources[j + 1]); j--)
                {
                    SortHelper.Swap<T>(dataSources, j, j + 1);
                }
            }
        }


    }
}