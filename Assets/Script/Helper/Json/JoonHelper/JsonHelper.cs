using UnityEngine;
using System;
using LitJson;
using System.IO;

public class JsonHelper
{

    ///// <summary>
    ///// 记载资源并序列化
    ///// </summary>
    ///// <param name="path"></param>
    ///// <param name="isNetJson">序列化方式：LitJson /NewtonJson</param>
    ///// <param name="action"></param>
    //public static void LoadResourceAsset(string path,bool isLitJson, Action<JsonData> action)
    //{
    //    TextAsset asset = Resources.Load<TextAsset>(path);
    //    if (asset != null)
    //    {
    //        if (action != null)
    //        {
              
    //                action(JsonMapper.ToObject(asset.text));
    //        }
    //    }
    //    else
    //        Debug.LogError("Cant Get File " + path);
    //}





    //public static void LoadAssetOfPath(string path, Action<JsonData> action)
    //{
    //    string data = File.ReadAllText(path);
    //    if (data != null)
    //        action(JsonMapper.ToObject(data));

    //}

}
