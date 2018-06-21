//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//namespace MFramework
//{

//    //Cache Data On Runtime
//    public class CacheHelper
//    {
//     //   public static List<LocalPlayerInfor> localAccountInfor { private set; get; }  //Runtime PlayerAccount And State Information
//        public Dictionary<string, string> CacheImageDic2 = new Dictionary<string, string>(); //Key URL,Value FilePath


//        private static CacheHelper instance;
//        public static CacheHelper GetInstance()
//        {
//            if (instance == null)
//                instance = new CacheHelper();
//            return instance;
//        }
//        private CacheHelper()
//        {
//     //       localAccountInfor = new List<LocalPlayerInfor>();
//        }

        


//        #region Image
//        //public void StoreImage(string pathName, byte[] data)
//        //{
//        //    byte[] oldData = null;
//        //    if (CacheImageDic.TryGetValue(pathName, out oldData))
//        //    {
//        //        CacheImageDic[pathName] = data;
//        //    }
//        //    else
//        //    {
//        //        CacheImageDic.Add(pathName, data);
//        //    }
//        //}

//        //public byte[] GetImageData(string pathName)
//        //{
//        //    byte[] imagedata = null;
//        //    CacheImageDic.TryGetValue(pathName, out imagedata);
//        //    return imagedata;
//        //}

//        //public void StoreText(string pathName, string data)
//        //{
//        //    string olddata = null;
//        //    if (CacheTextDic.TryGetValue(pathName, out olddata))
//        //    {
//        //        CacheTextDic[pathName] = data;
//        //    }
//        //    else
//        //    {
//        //        CacheTextDic.Add(pathName, data);
//        //    }
//        //}

//        //public string   GetTextData(string pathName)
//        //{
//        //   string data = null;
//        //    CacheTextDic.TryGetValue(pathName, out data);
//        //    return data;
//        //}
//        #endregion






//    }
//}
