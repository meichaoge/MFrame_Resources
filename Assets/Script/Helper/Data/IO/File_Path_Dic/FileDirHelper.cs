using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace MFramework
{

    public class FileDirHelper
    {
        ////Get FileName From Url
        //public static string URL2FileName(string url)
        //{   //**** use Path.GetFileName()
        //    if (string.IsNullOrEmpty(url)) return "";
        //    string _fileName;  //the realname
        //    string[] ur = url.Split('/');
        //    if (ur.Length > 0)
        //        _fileName = ur[ur.Length - 1];
        //    else
        //        _fileName = url;
        //    return _fileName;
        //}

        public static string GetFilePathWithOneDir(string url)
        {
            if (string.IsNullOrEmpty(url)) return "";
            string _fileName;  //the realname
            string[] ur = url.Split('/');
            if (ur.Length > 0)
            {
                if (ur.Length > 1)
                    _fileName = ur[ur.Length - 2] + "/" + ur[ur.Length - 1];
                else
                    _fileName = ur[ur.Length - 1];
            }
            else
                _fileName = url;
            //Debug.Log("AAAAAAA " + _fileName);
            return _fileName;
        }

        /// <summary>
        /// 获得Url中真实的文件路径 Exap:"http://wandou.beanvr.com/image/diy_ourtyard_room.jpg"
        /// </summary>
        /// <param name="url"></param>
        /// <param name="DirStartString"></param>
        /// <returns>image/diy_ourtyard_room.jpg</returns>
        public static string GetUrlFileDir(string url, string DirStartString = ".com/")
        {
            if (string.IsNullOrEmpty(url)) return "";
            if (url.Length <= DirStartString.Length) return "";
            int dex = url.IndexOf(DirStartString);
            if (dex == -1)
            {
                Debug.LogError("Url " + url + " Dose't Contain" + DirStartString);
                return "";
            }
            Debug.Log("Url " + url + "         Contain::" + DirStartString);
            return url.Substring(dex + DirStartString.Length, url.Length - dex - DirStartString.Length);

        }


        /// <summary>
        /// 删除一个目录下所有的文件和目录  包含当前的目录
        /// </summary>
        /// <param name="direc"></param>
        public static void DeleteDirectory(string deleteDire)
        {
            try
            {
                if (Directory.Exists(deleteDire) == false)
                    return;

                if (Directory.GetDirectories(deleteDire).Length > 0)
                {//还有子目录
                    foreach (var subDire in Directory.GetDirectories(deleteDire))
                        DeleteDirectory(subDire);
                }

                if (Directory.GetFiles(deleteDire).Length > 0)
                {//删除文件以及当前目录
                    foreach (var file in Directory.GetFiles(deleteDire))
                        File.Delete(file);
                }

                Directory.Delete(deleteDire, true);
            }
            catch { }

        }


   
        //public static string[] GetAllFiles(string topDirectoryPath,string patter=".txt")
        //{
        //    string[] allDirectory = Directory.GetFiles(topDirectoryPath, patter,SearchOption.AllDirectories);
        //}

       


    }
}
