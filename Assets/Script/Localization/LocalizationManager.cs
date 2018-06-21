using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 多语言支持管理器
    /// </summary>
    public class LocalizationManager : Singleton_Static<LocalizationManager>
    {
        protected override void InitialSingleton()
        {
            CurrentLanguage = LanguageType.cn;  //应该读取配置文件设置语言环境
        }

        public enum LanguageType
        {
            cn,  //中文
            en, //英文
        }

        public LanguageType CurrentLanguage { get; private set; }







        #region 本地化资源加载策略
        /// <summary>
        /// 获取精灵图片的路径(多语言支持)
        /// </summary>
        /// <param name="relativePath">相对于Resources/Sprite/路径</param>
        /// <returns></returns>
        public string GetLocalizationSprite(string relativePath)
        {
            return string.Format("Localization_Sprite/{0}/{1}", CurrentLanguage, relativePath);
        }

        /// <summary>
        /// 获取精灵图片的路径(多语言支持)
        /// </summary>
        /// <param name="relativePath">相对于Resources/Sprite/路径</param>
        /// <returns></returns>
        public string GetLocalizationFile(string relativePath)
        {
            return "";
            //return string.Format("Localization_Sprite/{0}/{1}", CurrentLanguage, relativePath);
        }


        public string GetLocalTextString(string fileName, string key, LanguageType language, LocalFileTypeEnum localFileEnum = LocalFileTypeEnum.ObjectSimpleVale)
        {
            string resultStr = "";
            string filePath = string.Format("Localization/{0}/{1}", language.ToString(), fileName);
            string path = Application.dataPath + "/Resources/" + filePath+ ".txt";
            Debug.Log("path=" + path);
            if (System.IO.File.Exists(path))
            {
                ResourcesMgr.Instance.LoadTextAsset(filePath, (localfileBase) =>
                {
                    if (localfileBase == null)
                    {
                        Debug.LogError("GetLocalTextString  Fail");
                        return;
                    }
                    resultStr = localfileBase.GetFileValueByKey(key);


                }, localFileEnum);
            }
            else
            {
                Debug.Log("GetLocalTextString Fail,当前文件不存在"+ fileName);
            }
            return resultStr;
        }



        #endregion

    }
}