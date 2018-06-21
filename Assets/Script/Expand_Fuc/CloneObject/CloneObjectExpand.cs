using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 对象复制的扩展 实现运行时深度复制对象
    /// </summary>
    public static class CloneObjectExpand
    {
        /// <summary>
        /// 深度复制对象  要复制的实例必须可序列化，包括实例引用的其它实例都必须在类定义时加[Serializable]特性。  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RealObject"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制     
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }


    }
}
