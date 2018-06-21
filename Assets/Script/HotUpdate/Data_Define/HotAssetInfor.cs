﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MFramework.Re
{

    [System.Serializable]
    /// <summary>
    /// 记录的某一个需要热更新的资源的信息 基类
    /// </summary>
    public class HotAssetInfor
    {
        public string m_MD5Code;  //资源MD5 用于资源完整性校验
        public int m_ByteSize = 0; //资源大小
    }
}