//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//namespace MFramework
//{

//    public class MeshLinePool : IObjectPool<UIMeshLine>
//    {
//        private Stack<UIMeshLine> m_Pool = new Stack<UIMeshLine>();
//        public string SourcePath { get; set; }
//        public uint MaxCount { get; set; }
//        private static ILogger m_Log;
//        private static ILogger Log4Helper
//        {
//            get
//            {
//                if (m_Log == null)
//                    m_Log = BLog.GetLog<MeshLinePool>();
//                return m_Log;
//            }
//        }
//        public MeshLinePool(string sourcesPath, uint maxCount = 0)
//        {
//            SourcePath = sourcesPath;
//            MaxCount = maxCount;
//            m_Pool = new Stack<UIMeshLine>();
//            if (maxCount != 0)
//            {
//                EventCenter.GetInstance().StartCoroutine(PreviousInitial());
//            }
//        }

//        public UIMeshLine GetInstance(string boxName, object obj, Transform parent = null, System.Action<UIMeshLine> action = null)
//        {
//            UIMeshLine result = null;
//            if (m_Pool == null || m_Pool.Count == 0)
//            {
//                result = CreateMeshLine(boxName, parent);
//            }
//            else
//            {
//                result = m_Pool.Pop();
//                if (result == null)
//                {
//                    Log4Helper.Debug("MeshLine is Destroyed");
//                    return GetInstance(boxName, null, parent, action);
//                }
//                else
//                {
//                    result.transform.SetParent(parent);
//                }
//            }

//            result.gameObject.SetActive(true);
//            if (action != null) action(result);
//            return result;
//        }

//        UIMeshLine CreateMeshLine(string boxName, Transform parent = null)
//        {
//            GameObject go = Resources.Load<GameObject>(SourcePath + boxName);
//            if (go != null)
//            {
//                GameObject box = GameObject.Instantiate(go);
//                box.transform.SetParent(parent);
//                return box.GetOrAddCompont<UIMeshLine>();
//            }
//            return null;

//            //GameObject obj = CommonOperate.CreateObjWhithName("UIMeshLine" + boxName, parent);
//            //return obj.GetOrAddCompont<UIMeshLine>();
//        }

//        public void Recycle(UIMeshLine item, object obj, System.Action<UIMeshLine> action = null)
//        {
//            if (item == null)
//            {
//                Log4Helper.Debug("Recyle MeshLine Fail ,Object is Null");
//                return;
//            }

//            if (MaxCount == 0 || m_Pool.Count < MaxCount)
//            {
//                if (action != null) action(item);
//                BaseObjectPool.PushPoolItem(PoolType.MeshLine, item.gameObject);
//                item.gameObject.SetActive(false);
//                m_Pool.Push(item);
//            }
//            else
//            {
//                GameObject.DestroyImmediate(item.gameObject);
//            }
//        }


//        IEnumerator PreviousInitial()
//        {
//            for (int dex = 0; dex < MaxCount; dex++)
//            {
//                var item = CreateMeshLine("MeshLine", BaseObjectPool.GetPoolTrans(PoolType.MeshLine).transform);
//                Recycle(item, null);
//                yield return null;
//            }
//        }



//    }
//}
