using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.Data
{
    /// <summary>
    /// 选择排序
    /// 基本思想 从待排序的数据中每次选择一个最大或者最小的数据然后与前面的某个位置的数据交换 ,循环N次
    /// </summary>
    public class SelectSortHelper
    {
        /// <summary>
        /// 选择排序
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

            int selectIndex = 0;
            for (int dex = 0; dex < dataSources.Count - 1; ++dex)   //注意这里dex<endIndex-1 因为 下面一个循环中会取下一个元素
            {
                selectIndex = dex;
                for (int j = dex + 1; j < dataSources.Count; ++j)
                {
                    if (handle(dataSources[selectIndex], dataSources[j]))
                    {
                        selectIndex = j;
                        //Debug.Log("Sort selectIndex=" + selectIndex);
                    }
                }
                if (dex != selectIndex)
                {
                //    Debug.LogInfor("dex=" + dex + "    selectIndex=" + selectIndex);
                    SortHelper.Swap<T>(dataSources, dex, selectIndex);
                }
            }
        }


    }
}