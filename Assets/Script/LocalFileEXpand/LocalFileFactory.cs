using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 解析文件的类型 
    /// </summary>
    public enum LocalFileTypeEnum
    {
        ObjectSimpleVale,  //单个Object  且value 是单个字符串的情况
        ArraySimpleValue, // Object 数组 每一个元素都是一个 ObjectSimpleVale

    }
    public class LocalFileFactory : Singleton_Static<LocalFileFactory>
    {
        protected override void InitialSingleton()
        {

        }


        public LocalFileBase GetLocalFileBase(LocalFileTypeEnum fileType,string filePath,string assetText)
        {
            switch (fileType)
            {
                case LocalFileTypeEnum.ObjectSimpleVale:
                    return new LocalFileObjectSimpleValue(filePath, assetText);
                case LocalFileTypeEnum.ArraySimpleValue:
                    return new LocalFileArraySimpleValue(filePath, assetText);
                default:
                    Debug.LogError("GetLocalFileBase  Fail,没有识别的类型 " + fileType);
                    return  null;
            }
        }



    }
}