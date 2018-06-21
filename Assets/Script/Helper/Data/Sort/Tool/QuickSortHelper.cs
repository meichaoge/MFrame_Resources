using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.Data
{
    /// <summary>
    /// 快速 排序  (算法不稳定  在某些情况下效率与冒泡排序一样)
    /// 基本思想将待排序的数据分成2队其中一堆元素比另一堆的小，然后递归分组
    /// </summary>
    public class QuickSortHelper
    {

        /// <summary>
        ///  简单的快速排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSources">需要排序的数据源</param>
        /// <param name="startIndex">排序的起始索引</param>
        /// <param name="endIndex">排序的结束索引</param>
        /// <param name="ratherEqualHandle">泛型化判断是否是大于等于    </param>
        /// <param name="lessEqualHandle">泛型化判断是否是小于等于    </param>
        public static void Sort<T>(List<T> dataSources, int startIndex, int endIndex, SortCompareHandle<T> ratherEqualHandle, SortCompareHandle<T> lessEqualHandle)
        {
            if (startIndex < endIndex)
            {
                int pivot = Partition<T>(dataSources, startIndex, endIndex, ratherEqualHandle, lessEqualHandle);        //将数组分为两部分
                Sort<T>(dataSources, startIndex, pivot - 1, ratherEqualHandle, lessEqualHandle);                   //递归排序左子数组
                Sort<T>(dataSources, pivot + 1, endIndex, ratherEqualHandle, lessEqualHandle);                  //递归排序右子数组
            }
        }

        /// <summary>
        /// 数据分组 返回值时下一次排序时候中间轴的位置索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSources"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="ratherEqualHandle"></param>
        /// <param name="lessEqualHandle"></param>
        /// <returns></returns>
        private static int Partition<T>(List<T> dataSources, int startIndex, int endIndex, SortCompareHandle<T> ratherEqualHandle, SortCompareHandle<T> lessEqualHandle)
        {
            int pivotIndex = startIndex;
            T pivot = dataSources[startIndex];     //枢轴记录
            int  leftPoint = 0; //记录左侧比轴数据小的数据索引
            int rightPoint = 0;//记录右 侧比轴数据大的数据索引

            //****思路： 先从当前子数组的末尾开始找到第一个小于轴数据并记录索引值，然后从左往右找到第一个比轴数据大的元素索引，然后交换两个记录索引对应的数据，继续查找 ，直到左右两个索引相同时候与轴数据交换
            while (startIndex < endIndex)
            {
                while (startIndex < endIndex && ratherEqualHandle(dataSources[endIndex], pivot))                // dataSources[endIndex] >= pivot)
                    --endIndex;
                rightPoint = endIndex;  //小于轴数据并记录索引值
                while (startIndex < endIndex && lessEqualHandle(dataSources[startIndex], pivot))           //dataSources[startIndex] <= pivot)
                    ++startIndex;
                leftPoint = startIndex;  //大于轴数据并记录索引值

               SortHelper.Swap<T>(dataSources, leftPoint, rightPoint);  //交换两个记录数据
            }
            SortHelper.Swap<T>(dataSources, pivotIndex, startIndex); //当前这次已经找到轴的位置
            //返回的是枢轴的位置
            return startIndex;
        }





    }
}