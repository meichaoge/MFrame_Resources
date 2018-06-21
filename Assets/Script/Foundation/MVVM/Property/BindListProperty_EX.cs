using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MFramework
{

    public delegate bool CompareKeyPropertyEqual<T>(T otherData);
    public enum CollectionEvent
    {
        AddSignal,
        AddRang,
        DeleteSignal,
        DeleteRangle,
        Clear,
        Update,
        Insert,   //
        Flush,   //刷新
    }

    public class BindListProperty<T> : IList<T>
    {

        private List<T> m_BindData = new List<T>();
        public delegate void ValueChangedHandler(T oldValue, T newValue, int _index, CollectionEvent action);
        public delegate void ValueChangedHandler2(IList<T> oldValue, IList<T> newValue, CollectionEvent action);

        public event ValueChangedHandler OnValueChangedEvent;
        public event ValueChangedHandler2 OnValueChangedEvent2;
        //public bool m_DispatchEvent = false; //Whether trigger  event default Not 
        protected bool m_DispatchEvent = true; //Whether trigger  event default Not 

        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < m_BindData.Count)
                    return m_BindData[index];
                else
                {
                    Debug.LogError("index not between 0 ~ " + m_BindData.Count);
                    return default(T);
                }
            }
            set
            {
                if (index >= 0 && index < m_BindData.Count )
                {
                    m_BindData[index] = value;
                }//if   
                else
                {
                        Debug.LogError(index + "index not between 0 ~ " + m_BindData.Count);
                }
            }
        }

        public int Count
        {
            get
            {
                return m_BindData.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(T item)
        {
            if (m_BindData.Contains(item))
            {
                 Debug.Log("Already Contain");
                return;
            }
            m_BindData.Add(item);
            if (OnValueChangedEvent != null)
                OnValueChangedEvent(default(T), item, m_BindData.Count - 1, CollectionEvent.AddSignal);
        }

        //when The Key Value Is The Same Then We Not Add
        //public void Add_EX(T item, CompareKeyPropertyEqual<T> handle)
        //{
        //    if (handle != null && handle(item))
        //    {
        //        //  Debug.Log("Already Contain");
        //        return;
        //    }
        //    m_BindData.Add(item);
        //    if (OnValueChangedEvent != null)
        //        OnValueChangedEvent(default(T), item, m_BindData.Count - 1, CollectionEvent.AddSignal);
        //}

        public void AddRange(IEnumerable<T> collection)
        {
            int previousCount = m_BindData.Count;
            m_BindData.AddRange(collection);
            if (OnValueChangedEvent2 != null)
                OnValueChangedEvent2(null, (IList<T>)collection, CollectionEvent.AddRang);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="dex"></param>
        /// <param name="_infor"></param>
        public void UpdateInfor(int dex,T _infor)
        {
            if (dex < 0 || dex > Count - 1)
            {
                Debug.LogError("NotValible Index " + dex);
                return;
            }
            T oldData = this[dex];
            this[dex] = _infor;
            if (OnValueChangedEvent != null)
                OnValueChangedEvent(oldData, _infor, dex, CollectionEvent.Update);
        }



        public void Clear()
        {
            m_BindData.Clear();
        }

        public bool Contains(T item)
        {
            return m_BindData.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_BindData.CopyTo(array, arrayIndex);

            //   Debug.LogError("Can't Use");
        }

        public T[] ToArray()
        {
          return  m_BindData.ToArray();
        }


        public IEnumerator<T> GetEnumerator()
        {
         //   Debug.LogError(" Not   Use  !!!");
            for (int i = 0; i < this.m_BindData.Count;++ i)
                yield return this.m_BindData[i];
        }

        public int IndexOf(T item)
        {
            return m_BindData.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            m_BindData.Insert(index, item);
            if (OnValueChangedEvent != null)
                OnValueChangedEvent(default(T), item, index, CollectionEvent.Insert);
        }

        public bool Remove(T item)
        {
            Debug.LogError("Use RemoveAt !!!");
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < m_BindData.Count)
            {
                T oldData = this[index];
                m_BindData.RemoveAt(index);
                if (OnValueChangedEvent != null)
                    OnValueChangedEvent(oldData, default(T), index, CollectionEvent.DeleteSignal);
            }
            else
                Debug.LogError("index not between 0 ~ " + m_BindData.Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_BindData.GetEnumerator();
        }



        /// <summary>
        /// 跑出数据源为空事件
        /// </summary>
        public void DispatchEmptyEvent()
        {
            Clear();
            if (OnValueChangedEvent2 != null)
                OnValueChangedEvent2(null, null, CollectionEvent.Clear);
        }

        /// <summary>
        /// 强制刷新
        /// </summary>
        public void DispatchFlushEvent()
        {
            if (OnValueChangedEvent2 != null)
                OnValueChangedEvent2(null, null, CollectionEvent.Flush);

        }

   
    }
}
