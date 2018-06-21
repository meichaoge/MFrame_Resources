using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort_Example : MonoBehaviour {

    int num = 15;

    string bubbleStr = string.Empty;    List<int> bubbleList = new List<int>();

    string quickStr = string.Empty;    List<int> quickList = new List<int>();

    string selectStr = string.Empty;    List<int> selectList = new List<int>();

    string heapStr = string.Empty;    List<int> heapList = new List<int>();

    string insertStr = string.Empty;    List<int> insertList = new List<int>();

    string shellStr = string.Empty;    List<int> shellList = new List<int>();    List<int> ShellParam = new List<int>();

    string mergeStr = string.Empty;    List<int> mergeList = new List<int>();

    string radixStr_MSD = string.Empty;    string radixStr_LSD = string.Empty;    List<int> radixList_MSD = new List<int>();    List<int> radixList_LSD = new List<int>();

    List<int> radixList = new List<int>();
    // Use this for initialization
    void Start()
    {        bubbleList = getRanNum(num);        bubbleStr = getStrFromList(bubbleList);

        quickList = getRanNum(num);        quickStr = getStrFromList(quickList);

        selectList = getRanNum(num);        selectStr = getStrFromList(selectList);

        heapList = getRanNum(num);        heapStr = getStrFromList(heapList);

        insertList = getRanNum(num);        insertStr = getStrFromList(insertList);

        shellList = getRanNum(num);        shellStr = getStrFromList(shellList);        ShellParam.Add(7);        ShellParam.Add(5);        ShellParam.Add(3);        ShellParam.Add(1);

        mergeList = getRanNum(num);        mergeStr = getStrFromList(mergeList);

        radixList_MSD = getRanNum(num);        radixList_LSD = getRanNum(num);        radixStr_MSD = getStrFromList(radixList_MSD);        radixStr_LSD = getStrFromList(radixList_LSD);

        radixList.Add(1);        radixList.Add(10);        radixList.Add(100);        radixList.Add(1000);

    }


    void OnGUI()    {        GUI.BeginGroup(new Rect(0, 0, 1000, 500));        GUILayout.BeginVertical();


        #region bubble        GUILayout.BeginHorizontal();        GUILayout.Label(bubbleStr);        if (GUILayout.Button("冒泡排序"))
        {
            //          BubbleSort ();
            BubbleSort(bubbleList, 0, num - 1);        }        GUILayout.Label(getStrFromList(bubbleList));        GUILayout.EndHorizontal();

        #endregion
        #region quick        GUILayout.BeginHorizontal();        GUILayout.Label(quickStr);        if (GUILayout.Button("快速排序"))        {            QuickSort(quickList, 0, num - 1);        }        GUILayout.Label(getStrFromList(quickList));        GUILayout.EndHorizontal();


        #endregion
        GUILayout.BeginHorizontal();        GUILayout.Label("-----------------------------------------------------------------------" +                         "-----------------------------------------------------------------------" +                         "-----------------------------------------------------------------------");        GUILayout.EndHorizontal();



        #region select        GUILayout.BeginHorizontal();        GUILayout.Label(selectStr);        if (GUILayout.Button("选择排序"))
        {            SelectSort();        }        GUILayout.Label(getStrFromList(selectList));        GUILayout.EndHorizontal();


        #endregion
        #region heap        GUILayout.BeginHorizontal();        GUILayout.Label(heapStr);        if (GUILayout.Button("堆 排 序"))        {            HeapSort(heapList, 0, num - 1);        }        GUILayout.Label(getStrFromList(heapList));        GUILayout.EndHorizontal();


        #endregion
        GUILayout.BeginHorizontal();        GUILayout.Label("-----------------------------------------------------------------------" +                         "-----------------------------------------------------------------------" +                         "-----------------------------------------------------------------------");        GUILayout.EndHorizontal();


        #region insert        GUILayout.BeginHorizontal();        GUILayout.Label(insertStr);        if (GUILayout.Button("插入排序"))
        {            InsertSort();        }        GUILayout.Label(getStrFromList(insertList));        GUILayout.EndHorizontal();


        #endregion
        #region shell        GUILayout.BeginHorizontal();        GUILayout.Label(shellStr);        if (GUILayout.Button("希尔排序"))        {            ShellSort(shellList);        }        GUILayout.Label(getStrFromList(shellList));        GUILayout.EndHorizontal();


        #endregion
        GUILayout.BeginHorizontal();        GUILayout.Label("-----------------------------------------------------------------------" +                         "-----------------------------------------------------------------------" +                         "-----------------------------------------------------------------------");        GUILayout.EndHorizontal();





        #region merge        GUILayout.BeginHorizontal();        GUILayout.Label(mergeStr);        if (GUILayout.Button("归并排序"))        {            MergeSort(mergeList, 0, num - 1);        }        GUILayout.Label(getStrFromList(mergeList));        GUILayout.EndHorizontal();


        #endregion
        GUILayout.BeginHorizontal();        GUILayout.Label("-----------------------------------------------------------------------" +            "-----------------------------------------------------------------------" +            "-----------------------------------------------------------------------");        GUILayout.EndHorizontal();





        #region radix        GUILayout.BeginHorizontal();        GUILayout.Label(radixStr_MSD);        if (GUILayout.Button("高位基数排序"))        {            RadixSort_MSD(radixList_MSD, 0, num - 1, radixList.Count - 1, radixList);        }        GUILayout.Label(getStrFromList(radixList_MSD));        GUILayout.EndHorizontal();










        #region MSD
        #endregion
        #region LSD        GUILayout.BeginHorizontal();        GUILayout.Label(radixStr_LSD);        if (GUILayout.Button("低位基数排序"))        {            RadixSort_LSD(radixList_LSD, 0, num - 1, radixList);        }        GUILayout.Label(getStrFromList(radixList_LSD));        GUILayout.EndHorizontal();








        #endregion
        #endregion        GUILayout.EndVertical();        GUI.EndGroup();    }







    #region Algorithm
    /// <summary>    /// Bubbles the sort.    /// </summary>    void BubbleSort()    {        for (int i = 1; i < num; i++)
        {            for (int j = 0; j < num - i; j++)
            {                if (bubbleList[j] > bubbleList[j + 1])
                {                    Swap(bubbleList, j, j + 1);                }            }        }    }

    void BubbleSort(List<int> list, int start, int end)    {        for (int i = start, count = 0; i < end; i++, count++)
        {            for (int j = start; j < end - count; j++)
            {                if (list[j] > list[j + 1])
                {                    Swap(list, j, j + 1);                }            }        }    }

    

    /// <summary>    /// Quicks the sort.    /// </summary>    /// <param name="list">List.</param>    /// <param name="start">Start.</param>    /// <param name="end">End.</param>    void QuickSort(List<int> list, int start, int end)    {        if (end - start + 1 <= 10)
        {            BubbleSort(list, start, end);            return;        }        PatitionMedianOfThree(list, start, end);

        int first = start;        int left = start;        int leftCount = 0;

        int last = end;        int right = end;        int rightCount = 0;        int key = list[first];

        while (first < last)
        {            while (first < last && key <= list[last])
            {                if (key == list[last])
                {                    Swap(list, right, last);                    right--;                    rightCount++;                }                last--;            }            list[first] = list[last];

            while (first < last && key >= list[first])
            {                if (key == list[first])
                {                    Swap(list, first, left);                    left++;                    leftCount++;                }                first++;            }            list[last] = list[first];        }

        list[first] = key;

        int i = first - 1;        int j = start;

        while (j < left && list[i] != key)
        {            Swap(list, j, i);            j++;            i--;        }

        i = last + 1;        j = end;        while (j > right && list[i] != key)
        {            Swap(list, i, j);            i++;            j--;        }        QuickSort(list, start, first - leftCount - 1);        QuickSort(list, last + rightCount + 1, end);

    }













    /// <summary>    /// Selects the sort.    /// </summary>    void SelectSort()    {        int selectIndex = 0;        for (int i = 0; i < num - 1; i++)
        {            selectIndex = i;            for (int j = i + 1; j < num; j++)
            {                if (selectList[selectIndex] > selectList[j])
                {                    selectIndex = j;                }            }            Swap(selectList, i, selectIndex);        }    }













    /// <summary>    /// Heaps the sort.    /// </summary>    /// <param name="list">List.</param>    /// <param name="start">Start.</param>    /// <param name="end">End.</param>    void HeapSort(List<int> list, int start, int end)    {        int length = end - start + 1;        for (int i = (length - 1) / 2; i >= 0; i--)
        {            HeapAdjust(list, i, end);        }
        //      for (int i = length - 1; i >= 1; i--) {
        //          Swap (list,0,i);
        //          HeapAdjust (list,0,i-1);
        //      }
        for (int i = 0; i < length - 1; i++)
        {            Swap(list, 0, end - i);            HeapAdjust(list, 0, end - i - 1);        }    }

    void HeapAdjust(List<int> list, int begin, int end)    {        int key = list[begin];        for (int i = begin * 2 + 1; i <= end; i = i * 2 + 1)
        {            if (i + 1 <= end && list[i] < list[i + 1])
            {                i++;            }            if (key < list[i])
            {                list[begin] = list[i];                begin = i;            }
            else
            {                break;            }        }        list[begin] = key;    }













    /// <summary>    /// Inserts the sort.    /// </summary>    void InsertSort()    {        for (int i = 1; i < num; i++)
        {            for (int j = i - 1; j >= 0 && insertList[j] > insertList[j + 1]; j--)
            {                Swap(insertList, j, j + 1);            }        }    }

    void ShellSort(List<int> list)    {        for (int m = 0; m < ShellParam.Count; m++)
        {            for (int i = 1; i < num; i++)
            {                for (int j = i - ShellParam[m]; j >= 0 && shellList[j] > shellList[j + ShellParam[m]]; j--)
                {                    Swap(list, j, j + 1);                }            }        }    }

    void MergeSort(List<int> list, int start, int end)    {        if (start != end)
        {            int mid = (end + start) / 2;            MergeSort(list, start, mid);            MergeSort(list, mid + 1, end);            Merge(list, start, mid, end);        }    }

    void Merge(List<int> list, int start, int mid, int end)    {        List<int> tempList = new List<int>();        int i = start;        int j = mid + 1;        while (i <= mid && j <= end)
        {            tempList.Add(list[i] <= list[j] ? list[i] : list[j]);            int a = list[i] <= list[j] ? i++ : j++;        }        while (i <= mid)
        {            tempList.Add(list[i]);            i++;        }        while (j <= end)
        {            tempList.Add(list[j]);            j++;        }        for (int m = start, n = 0; m <= end; m++, n++)        {
            list[m] = tempList[n];        }    }

    void RadixSort_MSD(List<int> list, int start, int end, int d, List<int> radixList)    {        int radix = 10;        int[] count = new int[10];        int[] bucket = new int[end - start + 1];

        for (int i = 0; i < radix; i++)
        {            count[i] = 0;        }        for (int i = start; i <= end; i++)
        {            count[getdigit(list[i], radixList[d])]++;        }        for (int i = 1; i < radix; i++)
        {            count[i] = count[i] + count[i - 1];        }        int maxIndex = count[radix - 1] - 1;        for (int i = end; i >= start; i--)
        {            int digit = getdigit(list[i], radixList[d]);            bucket[count[digit] - 1] = list[i];            count[digit]--;        }

        for (int i = start, j = 0; i <= end; i++, j++)
        {            list[i] = bucket[j];        }

        for (int i = 0; i <= radix - 1; i++)
        {            int p = count[i];            int q = i == radix - 1 ? maxIndex : count[i + 1] - 1;            if (p < q && d >= 1)
            {                RadixSort_MSD(list, p, q, d - 1, radixList);            }        }    }

    void RadixSort_LSD(List<int> list, int start, int end, List<int> radixList)    {        int radix = 10;        int[] count = new int[10];        int[] bucket = new int[end - start + 1];        for (int d = 0; d < radixList.Count; d++)        {
            for (int i = 0; i < radix; i++)
            {                count[i] = 0;            }            for (int i = start; i <= end; i++)
            {                count[getdigit(list[i], radixList[d])]++;            }            for (int i = 1; i < radix; i++)
            {                count[i] = count[i] + count[i - 1];
            }            for (int i = end; i >= start; i--)
            {

                int digit = getdigit(list[i], radixList[d]);                bucket[count[digit] - 1] = list[i];                count[digit]--;            }            for (int i = start, j = 0; i <= end; i++, j++)
            {                list[i] = bucket[j];            }        }    }

    int getdigit(int m, int n)    {        return m / n % 10;    }


    #endregion
    List<int> getRanNum(int num)    {        List<int> numList = new List<int>(); ;        for (int i = 0; i < num; i++)
        {

            numList.Add(Random.Range(0, 100));        }        return numList;    }

    string getStrFromList(List<int> list)    {        string numStr = string.Empty;        for (int i = 0; i < list.Count; i++)
        {            numStr += " " + list[i] + "  ";        }        return numStr;    }

    void PatitionMedianOfThree(List<int> list, int start, int end)    {        int first = start;        int median = (int)((end - start) / 2);        int last = end;

        if (list[median] > list[last])
        {            Swap(list, median, last);        }        if (list[first] > list[last])
        {            Swap(list, first, last);        }        if (list[first] < list[median])
        {            Swap(list, first, median);        }    }

    void Swap(List<int> list, int start, int end)    {        int temp = list[start];        list[start] = list[end];        list[end] = temp;    }


}
