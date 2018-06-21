using UnityEngine;
using System.Collections.Generic;


namespace MFramework
{

    public class MseeageBoxPool : IObjectPool<BaseMessageBox>
    {
        private Dictionary<MessageBoxResquest.MessageType, Stack<BaseMessageBox>> m_AllTypeBoxPool = new Dictionary<MessageBoxResquest.MessageType, Stack<BaseMessageBox>>();

        public string SourcePath { get; set; }
        public uint MaxCount { get; set; }
     

        public MseeageBoxPool(string sourcesPath, uint maxCount = 0)
        {
            SourcePath = sourcesPath;
            MaxCount = maxCount;

            //初始化对象池
            var allBoxTypeEnum = System.Enum.GetValues(typeof(MessageBoxResquest.MessageType)); //获得所有的枚举类型定义
            foreach (var item in allBoxTypeEnum)
            {
                MessageBoxResquest.MessageType _currentEnum = (MessageBoxResquest.MessageType)System.Enum.Parse(typeof(MessageBoxResquest.MessageType), item.ToString());//获得当前的枚举类型
                if (_currentEnum != MessageBoxResquest.MessageType.None)
                {
                    m_AllTypeBoxPool.Add(_currentEnum, new Stack<BaseMessageBox>(2));
                }
            }

        }


        public BaseMessageBox GetInstance(string boxName, object obj, Transform parent = null, System.Action<BaseMessageBox> action = null)
        {
            BaseMessageBox _result = null;
            MessageBoxResquest.MessageType boxType = (MessageBoxResquest.MessageType)obj;
            if (m_AllTypeBoxPool.ContainsKey((MessageBoxResquest.MessageType)boxType) == false)
            {
                Debug.LogError("GetRes UnIdentify Type :" + boxType);
                return null;
            }

            if (m_AllTypeBoxPool[boxType].Count > 0)
                _result = m_AllTypeBoxPool[boxType].Pop();

            if (_result != null)
            {
                _result.transform.SetParent(parent);
                _result.gameObject.SetActive(true);
            }
            else
            {
                GameObject go = Resources.Load<GameObject>(SourcePath + boxName);
                if (go != null)
                {
                    GameObject box = GameObject.Instantiate(go);
                    _result = box.GetComponent<BaseMessageBox>();
                }
            }


            if (action != null) action(_result);
            return _result;
        }


        public void Recycle(BaseMessageBox item, object obj, System.Action<BaseMessageBox> action = null)
        {
            if (item == null) return;
            MessageBoxResquest.MessageType boxType = (MessageBoxResquest.MessageType)obj;
            if (m_AllTypeBoxPool.ContainsKey(boxType) == false)
            {
                Debug.LogError(" Recycle   UnIdentify Type :" + boxType);
                return;
            }
            if (MaxCount == 0 || m_AllTypeBoxPool[boxType].Count < MaxCount)
            {
                if (action != null) action(item);
                BaseObjectPool.PushPoolItem(PoolType.MsgBox, item.gameObject);
                item.gameObject.SetActive(false);
                m_AllTypeBoxPool[boxType].Push(item);
            }
            else
                GameObject.DestroyImmediate(item.gameObject);


        }

    }
}
