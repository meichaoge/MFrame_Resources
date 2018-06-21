using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 扩展Json 数据
    /// </summary>
    public static class JsonExpand
    {
        /// <summary>
        /// 判断当前Json 数据中是否包含这个数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsContainKey(this JsonData json,string key)
        {
            //if(json.IsObject==false)
            //{
            //    Debug.LogError("无法判断是否存在这个Key 值" + key);
            //    return false;
            //}

            foreach (var keyPara  in json.Keys)
            {
                if (key == keyPara)
                    return true;
            }
            return false;
        }


    }
}