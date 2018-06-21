using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.Re
{
    /// <summary>
    /// 记录的AssetBundle 每一个项的信息
    /// </summary>
    public class HotAssetInfor_AssetBundle : HotAssetInfor
    {
        public readonly List<string> m_Dependece = new List<string>();  //AssetBundle 依赖

    }
}