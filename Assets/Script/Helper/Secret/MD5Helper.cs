﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace MFramework.Sercurity
{
    /// <summary>
    /// 获取文件MD5码的帮助类
    /// </summary>
    public class MD5Helper
    {
        /// <summary>
        /// 获取制定路径下文件的MD5 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileMD5(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                int len = (int)fs.Length;
                byte[] data = new byte[len];
                fs.Read(data, 0, len);
                fs.Close();
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] result = md5.ComputeHash(data);
                string fileMD5 = "";
                foreach (byte b in result)
                {
                    fileMD5 += Convert.ToString(b, 16);
                }
                return fileMD5;
            }
            catch (FileNotFoundException e)
            {
                Debug.LogError(e.Message);
                return "";
            }
        }



    }
}
