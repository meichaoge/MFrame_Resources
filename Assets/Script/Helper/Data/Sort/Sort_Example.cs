﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort_Example : MonoBehaviour {

    int num = 15;

    string bubbleStr = string.Empty;

    string quickStr = string.Empty;

    string selectStr = string.Empty;

    string heapStr = string.Empty;

    string insertStr = string.Empty;

    string shellStr = string.Empty;

    string mergeStr = string.Empty;

    string radixStr_MSD = string.Empty;

    List<int> radixList = new List<int>();
    // Use this for initialization
    void Start()
    {

        quickList = getRanNum(num);

        selectList = getRanNum(num);

        heapList = getRanNum(num);

        insertList = getRanNum(num);

        shellList = getRanNum(num);

        mergeList = getRanNum(num);

        radixList_MSD = getRanNum(num);

        radixList.Add(1);

    }


    void OnGUI()


        #region bubble
        {
            //          BubbleSort ();
            BubbleSort(bubbleList, 0, num - 1);

        #endregion
        #region quick


        #endregion
        GUILayout.BeginHorizontal();



        #region select
        {


        #endregion
        #region heap


        #endregion
        GUILayout.BeginHorizontal();


        #region insert
        {


        #endregion
        #region shell


        #endregion
        GUILayout.BeginHorizontal();





        #region merge


        #endregion
        GUILayout.BeginHorizontal();





        #region radix










        #region MSD
        #endregion
        #region LSD








        #endregion
        #endregion







    #region Algorithm
    /// <summary>
        {
            {
                {

    void BubbleSort(List<int> list, int start, int end)
        {
            {
                {

    

    /// <summary>
        {

        int first = start;

        int last = end;

        while (first < last)
        {
            {
                {

            while (first < last && key >= list[first])
            {
                {

        list[first] = key;

        int i = first - 1;

        while (j < left && list[i] != key)
        {

        i = last + 1;
        {

    }













    /// <summary>
        {
            {
                {













    /// <summary>
        {
        //      for (int i = length - 1; i >= 1; i--) {
        //          Swap (list,0,i);
        //          HeapAdjust (list,0,i-1);
        //      }
        for (int i = 0; i < length - 1; i++)
        {

    void HeapAdjust(List<int> list, int begin, int end)
        {
            {
            {
            else
            {













    /// <summary>
        {
            {

    void ShellSort(List<int> list)
        {
            {
                {

    void MergeSort(List<int> list, int start, int end)
        {

    void Merge(List<int> list, int start, int mid, int end)
        {
        {
        {
            list[m] = tempList[n];

    void RadixSort_MSD(List<int> list, int start, int end, int d, List<int> radixList)

        for (int i = 0; i < radix; i++)
        {
        {
        {
        {

        for (int i = start, j = 0; i <= end; i++, j++)
        {

        for (int i = 0; i <= radix - 1; i++)
        {
            {

    void RadixSort_LSD(List<int> list, int start, int end, List<int> radixList)
            for (int i = 0; i < radix; i++)
            {
            {
            {
            }
            {

                int digit = getdigit(list[i], radixList[d]);
            {

    int getdigit(int m, int n)


    #endregion
    List<int> getRanNum(int num)
        {

            numList.Add(Random.Range(0, 100));

    string getStrFromList(List<int> list)
        {

    void PatitionMedianOfThree(List<int> list, int start, int end)

        if (list[median] > list[last])
        {
        {
        {

    void Swap(List<int> list, int start, int end)


}