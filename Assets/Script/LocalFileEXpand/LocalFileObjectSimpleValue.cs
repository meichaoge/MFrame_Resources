using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /*
     {
            "1":"学院",
            "2":"士兵"
       }  //********示例 json
    */  //示例Json 

    /// <summary>
    /// 适用于Json 数据是一个JsonObject  ,切其Vlue 值是单个值的情况
    /// </summary>
    public class LocalFileObjectSimpleValue : LocalFileBase
    {
        private Dictionary<string, string> m_DataDic = new Dictionary<string, string>();
        public LocalFileObjectSimpleValue(string filePath, string assetText) : base(filePath, assetText)
        {

        }


        public override void AnalysisFile(string assetText)
        {
            base.AnalysisFile(assetText);
            if (m_IsEnable == false) return;
            if (m_IsInitialed) return;
            m_IsInitialed = true;
            if (m_Json.IsObject == false)
            {
                Debug.Log("无法解析的类型 " + m_FilePath);
                return;
            }

            foreach (var key in m_Json.Keys)
            {
                if (m_DataDic.ContainsKey(key))
                {
                    Debug.LogError("LocalFileSimleValue 包含重复的Key FileName= "+m_FilePath+"  key=" + key);
                    continue;
                }
                if (m_Json[key] == null)
                    m_DataDic.Add(key, null);
                else
                    m_DataDic.Add(key, m_Json[key].ToString());
            }
            return;

        }


        public override string GetFileValueByKey(string key)
        {
            if (m_DataDic.ContainsKey(key))
                return m_DataDic[key];

            Debug.LogError("没有找到这个Key  FileName=" + m_FilePath +"  key=" + key);
            return "";
        }

    }
}