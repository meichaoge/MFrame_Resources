using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace MFramework
{
    /// <summary>
    /// 定义如何加载和解析配置文件(json 格式) 的基类
    /// </summary>
    public class LocalFileBase
    {

        public string m_FilePath { protected set; get; }
        public JsonData m_Json { protected set; get; }
        public string m_TextData { protected set; get; }
        protected bool m_IsEnable = true; //标识当前文件是否解析正确
        protected bool m_IsInitialed = false;  //标识是否已经初始化了
        public LocalFileTypeEnum m_LocalFileTypeEnum { protected set; get; }


        public LocalFileBase(string filePath, string assetText)
        {
            m_FilePath = filePath;
            m_TextData = assetText;
            m_LocalFileTypeEnum = LocalFileTypeEnum.ObjectSimpleVale;
            if (string.IsNullOrEmpty(assetText))
            {
                m_IsEnable = false;
                Debug.LogError("AnalysisFile Fail,assetText Is NullOrEmpty!!  " + filePath);
                return;
            }
            AnalysisFile(assetText);
        }




        public virtual void AnalysisFile(string assetText)
        {
            m_Json = JsonMapper.ToObject(assetText);
        }

        #region        查找数值


        /// <summary>
        /// 根据给定的Key 解析文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string GetFileValueByKey(string key)
        {
            return "";
        }


        /// <summary>
        /// 根据给定的Key 解析文件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="compareFuc">比较Key值对应的Value 值是否相等的方法</param>
        /// <returns></returns>
        public virtual Dictionary<string, string> GetFileValueByKey(string key,System.Func<string,bool>  compareFuc)
        {
            return new Dictionary<string, string>();
        }

        #endregion

    }
}