using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.Data
{
    /// <summary>
    /// Shell(希尔) 排序  对直接插入排序算法的改良
    /// 基本思想 通过一顶变化量的步长将数据分成多组分别排序 直到增量为1时候 直接排序
    /// </summary>
    public class ShellSortHelper
    {
        public static void Sort<T>(List<T> dataSources, SortCompareHandle<T> handle)
        {
            if (SortHelper.CheckParameter<T>(dataSources, handle) == false)
                return;

            int[] sedgewick = Sedgewick(dataSources.Count);// 得到塞奇威克（Sedgewick） 步长序列数组
        
            for (int dex=0;dex<sedgewick.Length;++dex)
            {
                Debug.Log(sedgewick[dex]); 
            }

            int s, k, i, j;
            T t;
            // 循环出所有步长
            for (s = sedgewick.Length - 1; s >= 0; s--)
            {
                // 步长为sedgewick[s] 即分为 sedgewick[s] 个组
                for (k = 0; k < sedgewick[s]; k++)
                {
                    // 对每组数据进行排序
                    for (i = k + sedgewick[s]; i < dataSources.Count; i += sedgewick[s])
                    {
                        // 分组中，按插入排序排序数据，交换数据按步长 sedgewick[s]
                        t = dataSources[i];
                        j = i - sedgewick[s];
                        while (j >= 0 && handle(dataSources[j], t))       //dataSources[j] > t)
                        {
                            dataSources[j + sedgewick[s]] = dataSources[j];
                            j -= sedgewick[s];
                        }
                        dataSources[j + sedgewick[s]] = t;
                    }
                }
            }
        }

        // 塞奇威克（Sedgewick） 步长序列函数， 传入数组长度（最大分组个数不可超过数组长度）
        private static int[] Sedgewick(int length)
        {
            int[] arr = new int[length];
            int n, i = 0, j = 0;
            for (n = 0; n < length; n++)
            {
                if (n % 2 == 0)
                {
                    arr[n] = (int)(9 * (Mathf.Pow(4, i) - Mathf.Pow(2, i)) + 1);
                    i++;
                }
                else
                {
                    arr[n] = (int)(Mathf.Pow(2, j + 2) * (Mathf.Pow(2, j + 2) - 3) + 1);
                    j++;
                }
                if (arr[n] >= length)
                {
                    break;
                }
            }
            int[] a = new int[n];
            for (int k = 0; k < a.Length; k++)
            {
                a[k] = arr[k];
            }
            return a;
        }

    }
}