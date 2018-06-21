using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class System_IO_Path_Ex
{
    /// <summary>
    /// 获取一个路径的父级目录名
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="split"></param>
    /// <returns></returns>
    public static string GetPathParentDirectoryName(string filePath, char split = '/')
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("GetPathParentDirectory Fail,Path is null or empty " + (filePath == null));
            return "";
        }
        string[] paths = filePath.Split(split);
        if (paths.Length == 1)
        {
            Debug.LogError("GetPathParentDirectory Fail,is topDirectory " + filePath);
            return "";
        }
        return paths[paths.Length - 2];


    }

}
