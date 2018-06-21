using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// �����Ƶ���չ ʵ������ʱ��ȸ��ƶ���
    /// </summary>
    public static class CloneObjectExpand
    {
        /// <summary>
        /// ��ȸ��ƶ���  Ҫ���Ƶ�ʵ����������л�������ʵ�����õ�����ʵ�����������ඨ��ʱ��[Serializable]���ԡ�  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RealObject"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //���� System.Runtime.Serialization���л��뷴���л�������ö���ĸ���     
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }


    }
}
