using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.Re
{

    [System.Serializable]
    /// <summary>
    /// 需要更新的资源信息基类（）
    /// </summary>
    public class HotAssetRecordInfor
    {
        public string m_Version = "1.0.0.0";  //版本号
                                       //Key 为当前 资源 相对路径
        public Dictionary<string, HotAssetInfor> m_AllAssetRecordsDic = new Dictionary<string, HotAssetInfor>(); //所有资源 AssetInfor

    }



}