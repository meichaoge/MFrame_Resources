using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace MFramework.EditorExpand
{
    [System.Serializable]
    public class MyContent 
    {
        public string m_ContentTitle;  //目录描述
        
        public List<MonoScript> m_ConnectScript = new List<MonoScript>(0); //关联的脚本
    }

    public class ContenInfor: ScriptableObject
    {
        public List<MyContent> m_AllContenInfor = new List<MyContent>();  //所有的目录结构
    }



    /// <summary>
    /// 创建所有的资源以来关系
    /// </summary>
    public class CreateContentConfigureAsset
    {
        [MenuItem("Tools/创建目录功能Asset")]
        public static void CreateSceneItemConfg()
        {
            ScriptableObjectUtility.CreateUnityAsset<ContenInfor>();
        }

    }
}