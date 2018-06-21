
using System;
using System.Reflection;
using UnityEngine;

namespace MFramework
{
    public class BindListItemEX<T> : ICloneable, IBindJsonBase<T> where T : IBindJsonBase<T>
    {
        public delegate void SubItemChange(object oldValu, int _index);
        protected BindListItemEX<T> OldValue;
        public string RawJson { get; set; }

        public SubItemChange m_SubItemChangeHandle;
        public int m_ItemIndex;


        public string Serialize()
        {
            return JsonParser.Serialize(this);
        }

        public T Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            try
            {
                T ret = JsonParser.Deserialize<T>(json);
                ret.RawJson = json;
                return ret;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SDKJsonObject.Deserialize\r\n" + e.ToString() + "\r\njson:" + json);
                return default(T);
            }
        }


        protected virtual void OnItemValueChange()
        {
            if (m_SubItemChangeHandle != null)
            {
                m_SubItemChangeHandle(OldValue, m_ItemIndex);
            }
        }

        public virtual void Update(T data)
        {

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}