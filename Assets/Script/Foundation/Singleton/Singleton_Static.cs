using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// ��Mono�ĵ���ģ���� ��д InitialSingleton ����ʵ�ֳ�ʼ������
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
                lock (obj)   //�������
                {
                    //***�����ȡ �Ǿ�̬ InitialSingleton ���� ������
                    Type _type = typeof(T);
                    BindingFlags flag = BindingFlags.NonPublic | BindingFlags.Instance;
                    MethodInfo _infor = _type.GetMethod("InitialSingleton", flag); //������InitialSingleton ����
                    object obj = Activator.CreateInstance(_type);
                    instance = (T)obj;
                    //  Debug.Log(_type + "  instance=" + instance);
                    if (instance == null)
                        Debug.LogError("GetInstance Fail .... " + _type);

                    _infor.Invoke(obj, null); //���÷���
                }
            }
            return instance;
        }
  
        /// <summary>
        /// ��ʼ������ʵ���Ľӿ�
        /// </summary>
        protected abstract void InitialSingleton();

        public virtual void DisposeInstance()
        {
            instance = null;
        }



    }
}
