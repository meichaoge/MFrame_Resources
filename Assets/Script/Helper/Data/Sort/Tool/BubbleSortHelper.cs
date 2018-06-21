using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.Data
{

    /// <summary>
    /// 简单冒泡排序实现  时间复杂度O(n^2)
    /// </summary>
    public class BubbleSortHelper
    {

        /// <summary>
        /// 简单的冒泡排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSources"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="handle"></param>
        public static void Sort<T>(List<T> dataSources, SortCompareHandle<T> handle)
        {
            if (SortHelper.CheckParameter<T>(dataSources, handle) == false)
                return;

            int count = 0;  //已经完成的排序次数
            for (int dex = 0; dex < dataSources.Count; ++dex)
            {
                for (int j = 0; j < dataSources.Count - count-1; ++j)
                {
                    if (handle(dataSources[j], dataSources[j + 1]))
                        SortHelper.Swap<T>(dataSources, j, j + 1);
                }
                ++count;
            }
        }



    }
}