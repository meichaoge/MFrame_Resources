using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    public class Singleton_Mono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object obj = new object();
        private static T instance;


#if UNITY_EDITOR
        /// <summary>
        /// ���ڱ༭�����ڽű�ʱ����
        /// </summary>
        protected virtual void Reset()
        {
            if (GameObject.FindObjectsOfType<T>().Length > 1)
            {
                GameObject.DestroyImmediate(gameObject.GetComponent<T>());
                Debug.LogError("There are Already Exit " + typeof(T));
            }
        }
#endif

        /// <summary>
        /// ȷ��������ʱ��̬��������������Ȼ����ֻ��һ������
        /// </summary>
        protected virtual void Awake()
        {
            GetInstance(false);  //Makesure the other Component is destroyed
        }

        // protected static bool m_IsInitialed = false;
        public static T GetInstance(bool isIgnoreCheck = true)
        {
            if (instance != null && isIgnoreCheck)
                return instance;

            lock (obj)
            {
                var result = GameObject.FindObjectsOfType<T>();
                if (result.Length == 0)
                {
                 //   Debug.LogError("MonoSingleton ... Not Initialed :" + typeof(T));
                       instance = new GameObject(typeof(T).Name).AddComponent<T>();  //���Է��ֵ�����ʱ���������һ�����л����ɶ������
                }
                else if (result.Length == 1)
                {
                    instance = result[0];
                }
                else
                {
                    instance = result[result.Length - 1];  //Keep the First Initialed one  Be The Avalable
                    Debug.LogError("There are " + result.Length + " " + typeof(T));
                    for (int dex = 0; dex < result.Length - 1; ++dex)
                        GameObject.DestroyImmediate(result[dex]);
                }
            }
            return instance;
        }

        protected virtual  void  OnDestroy()
        {
            instance = null;
            obj = null;
        }

        /// <summary>
        /// �ͷŵ�ǰ����
        /// </summary>
        public virtual void DisposeInstance()
        {
            GameObject.DestroyObject(gameObject);
            instance = null;
        }

    }
}
