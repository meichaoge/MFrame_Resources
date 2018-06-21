using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.Data
{
    /// <summary>
    /// 堆排序算法实现
    /// 算法的时间空间效率位 O(N*LogN) , 时间复杂度 O(N*logN)   不适合数据较小的排序
    /// </summary>
    public class HeapSortHelper
    {
        //将数组分为两部分，一部分为有序区，在数组末尾，另一部分为无序区。堆属于无序区

        /// <summary>
        /// 堆排序 排序后的结果是一个从小到大的结果  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSources"></param>
        /// <param name="lessHandle">比较两个数据小于的比较方法</param>
        public static void Sort<T>(List<T> dataSources, SortCompareHandle<T> lessHandle)
        {
            BuildHeap(dataSources, dataSources.Count, lessHandle);  //首先建立基本的堆结构

            //for (int dex = 0; dex < dataSources.Count; ++dex)
            //{
            //    Debug.Log(dataSources[dex]);
            //}

            for (int i = dataSources.Count - 1; i > 0; i--)
            {//i为无序区的长度，经过如下两步，长度递减
             //堆顶即下标为0的元素
                SortHelper.Swap<T>(dataSources, i, 0);//1.每次将堆顶元素和无序区最后一个元素交换，即将无序区最大的元素放入有序区
                AdjustHeap(dataSources, 0, i, lessHandle);   //2.将无顺区调整为大顶堆，即选择出最大的元素。
            }
        }


        //建立堆，堆是从下往上建立的，因为adjustHeap函数是建立在子树已经为大顶堆。
        private static void BuildHeap<T>(List<T> dataSource, int size, SortCompareHandle<T> lessHandle)
        {
            for (int i = size / 2; i >= 0; i--)
            {//从最后一个非叶子节点，才能构成adjustHeap操作的目标二叉树
                AdjustHeap(dataSource, i, size, lessHandle);
            }
        }


        /// <summary>
        /// 从第一个非叶子节点一直到根节点  比较根节点与左右两个子节点 如果满足条件就交换 然后调整交换后的子堆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSources"></param>
        /// <param name="i">当前比较的局部子树节点索引</param>
        /// <param name="size"></param>
        /// <param name="lessHandle"></param>
        private static void AdjustHeap<T>(List<T> dataSources, int i, int size, SortCompareHandle<T> lessHandle)
        {
            int lChild = 2 * i + 1;        //左孩子
            int rChild = 2 * i + 2;        //右孩子
            int maxIndex = i;                //临时变量 记录最大的节点的索引值

            if (i < size / 2)
            {            //如果i是叶子节点就结束
                if (lChild < size && lessHandle(dataSources[maxIndex], dataSources[lChild]))      //dataSources[max] < dataSources[lChild])
                    maxIndex = lChild;
                if (rChild < size && lessHandle(dataSources[maxIndex], dataSources[rChild]))     //dataSources[max] < dataSources[rChild])
                    maxIndex = rChild;




                if (maxIndex != i)
                {
                    SortHelper.Swap<T>(dataSources, maxIndex, i);//交换后破环了子树的堆结构
                    AdjustHeap(dataSources, maxIndex, size, lessHandle);//递归，调节子树为堆
                }
            }
        }





    }
}