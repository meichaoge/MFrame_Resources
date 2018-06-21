using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework
{
    /*
     [
        {
             "aa": 11
         },
        {
            "aa": 112
         }
    ]
     */  //示例
    /// <summary>
    /// 
    /// </summary>
    public class LocalFileArraySimpleValue : LocalFileBase
    {
        private List<Dictionary<string, string>> m_DataList = new List<Dictionary<string, string>>();
        public LocalFileArraySimpleValue(string filePath, string assetText) : base(filePath, assetText)
        {
            m_LocalFileTypeEnum = LocalFileTypeEnum.ArraySimpleValue;
        }



        public override void AnalysisFile(string assetText)
        {
            base.AnalysisFile(assetText);

            if (m_IsEnable == false) return;
            if (m_IsInitialed) return;
            m_IsInitialed = true;
            if (m_Json.IsArray == false)
            {
                Debug.Log("无法解析的类型 " + m_FilePath);
                return;
            }

            for (int dex = 0; dex < m_Json.Count; ++dex)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                foreach (var key in m_Json[dex].Keys)
                {
                    if (data.ContainsKey(key))
                    {
                        Debug.LogError("包含重复的Key FileName= " + m_FilePath + "  key=" + key);
                        continue;
                    }
                    if (m_Json[dex][key] == null)
                        data.Add(key, null);
                    else
                        data.Add(key, m_Json[dex][key].ToString());
                }
                m_DataList.Add(data);
            }




        }



        public override Dictionary<string, string> GetFileValueByKey(string key, Func<string, bool> compareFuc)
        {
           for (int dex=0;dex< m_DataList.Count; ++dex)
           {
                if(m_DataList[dex].ContainsKey(key))
                {
                    if (compareFuc(m_DataList[dex][key]))
                        return m_DataList[dex];
                }
                else
                {
                    Debug.LogError("GetFileValueByKey  Fail,没有找到这个key  FileName=" + m_FilePath + "   Key=" + key);
                    return null;
                }
            }


            Debug.LogError("GetFileValueByKey  Fail,没有找到符合条件的数据 FileName=" + m_FilePath + "   Key=" + key);
            return null;
        }

    }
}