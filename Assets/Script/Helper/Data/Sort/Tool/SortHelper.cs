using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.Data
{
    public delegate bool SortCompareHandle<T>(T a, T b);  //比较规则

    /// <summary>
    /// 各种排序算法的基类
    /// </summary>
    public class SortHelper
    {
        /// <summary>
        /// 检测参数是否合法 或者是否需要排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSources"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static bool CheckParameter<T>(List<T> dataSources, SortCompareHandle<T> handle)
        {
            if (dataSources == null || dataSources.Count <= 1 || handle == null) return false;

            return true;
        }


        /// <summary>
        /// 交换数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSort"></param>
        /// <param name="startInde"></param>
        /// <param name="endIndex"></param>
        public static void Swap<T>( List<T> dataSources, int startIndex, int endIndex)
        {
            if (startIndex == endIndex) return;
            T temp = dataSources[startIndex];
            dataSources[startIndex] = dataSources[endIndex];
            dataSources[endIndex] = temp;
        }


    }
}