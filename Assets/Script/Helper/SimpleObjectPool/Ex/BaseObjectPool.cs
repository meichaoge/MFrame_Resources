using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MFramework
{
    public enum PoolType
    {
        MsgBox,  //
        MeshLine,
    }

    public class BaseObjectPool 
    {
        private static Transform ObjectPoolTrans;
        private static Dictionary<PoolType, object> m_AllPoolDataBase = new Dictionary<PoolType, object>();
        private static Dictionary<PoolType, GameObject> m_AllPoolView = new Dictionary<PoolType, GameObject>();
       

        public static bool IsInitialed = false;
        public static void InitialObjectPool(Transform trans)
        {
            if (IsInitialed) return;
            if (trans == null)
            {
                Debug.LogError("Object Pool Trans Is Null");
                return;
            }
            IsInitialed = true;
            ObjectPoolTrans = trans;
            GameObject.DontDestroyOnLoad(trans.gameObject);
        }



        public static object GetPoolByType(PoolType type, string sourcesPath, uint maxCount = 0)
        {
            if (IsInitialed == false)
            {
                Debug.LogError("BaseObject Pool Not Initialed");
                return null;
            }


            if (ObjectPoolTrans == null)
                GenerationObjUtility.CreateObjectByName("BaseObjectPool", null,true);

            object result = null;
            if (m_AllPoolDataBase.TryGetValue(type, out result))
                return result;
            switch (type)
            {
                case PoolType.MsgBox:
                    result = new MseeageBoxPool(sourcesPath, maxCount);
                    m_AllPoolDataBase[type] = result;
                    GetPoolTrans(type);
                    break;
                //case PoolType.MeshLine:
                //    result = new MeshLinePool(sourcesPath, maxCount);
                //    m_AllPoolDataBase[type] = result;
                //    GetPoolTrans(type);
                //    break;
            }
            return result;
        }


        public static GameObject GetPoolTrans(PoolType type)
        {
            if (IsInitialed == false)
            {
                Debug.LogError("BaseObject Pool Not Initialed");
                return null;
            }
            GameObject result = null;
            m_AllPoolView.TryGetValue(type, out result);
            if (result != null)
                return result;

            switch (type)
            {
                case PoolType.MsgBox:
                    result = GenerationObjUtility.CreateObjectByName("MseeageBoxPool", ObjectPoolTrans, true);
               //     result = CommonOperate.CreateObjWhithName("MseeageBoxPool", ObjectPoolTrans);
                    break;
                case PoolType.MeshLine:
                    result = GenerationObjUtility.CreateObjectByName("MseeageBoxPool", ObjectPoolTrans, true);
                  //  result = CommonOperate.CreateObjWhithName("MeshLinePool", ObjectPoolTrans);
                    break;
            }
            result.transform.SetTransLocalState(Vector3.one, Vector3.one * 100000, Vector3.zero, false);
            m_AllPoolView[type] = result;
            return result;
        }

        public static void PushPoolItem(PoolType type, GameObject item)
        {
            if (IsInitialed == false)
            {
                Debug.LogError("BaseObject Pool Not Initialed");
                return ;
            }
            GameObject poolParent;
            if (m_AllPoolView.TryGetValue(type, out poolParent))
            {
                if (item == null || poolParent == null) return;
                item.transform.SetParent(poolParent.transform);
            }
            else
            {
                GetPoolTrans(type);
                PushPoolItem(type, item);
            }
        }



    }
}
