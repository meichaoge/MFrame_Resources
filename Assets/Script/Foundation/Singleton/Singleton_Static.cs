using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 非Mono的单例模板类 重写 InitialSingleton 方法实现初始化操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton_Static<T> where T : class, new()
    {
        protected static T instance = null;
        private static object obj = new object();

        public static T Instance { get { return GetInstance(); }   protected set { instance = value; } }
        public static T GetInstance()
        {
            if (instance == null)
            {
                lock (obj)   //必须加锁
                {
                    //***反射获取 非静态 InitialSingleton 方法 并调用
                    Type _type = typeof(T);
                    BindingFlags flag = BindingFlags.NonPublic | BindingFlags.Instance;
                    MethodInfo _infor = _type.GetMethod("InitialSingleton", flag); //反射获得InitialSingleton 方法
                    object obj = Activator.CreateInstance(_type);
                    instance = (T)obj;
                    //  Debug.Log(_type + "  instance=" + instance);
                    if (instance == null)
                        Debug.LogError("GetInstance Fail .... " + _type);

                    _infor.Invoke(obj, null); //调用方法
                }
            }
            return instance;
        }
  
        /// <summary>
        /// 初始化单例实例的接口
        /// </summary>
        protected abstract void InitialSingleton();

        public virtual void DisposeInstance()
        {
            instance = null;
        }



    }
}
