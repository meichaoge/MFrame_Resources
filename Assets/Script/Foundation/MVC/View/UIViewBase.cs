using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    public class UIViewBase : MonoBehaviour
    {
        protected Entity mEntity;

        protected virtual void Awake() { }
        protected virtual void Start()
        {
        }

        /// <summary>
        /// ������Ҫ��Entiyty
        /// </summary>
        /// <param name="entity">Entity.</param>
        public void SetEntity(Entity entity)
        {
            mEntity = entity;
        }

        protected void FireMsg< T>(System.Enum eventType, T agr1)
        {
            mEntity.Dispatch<T>(eventType, agr1);
        }
        protected void FireMsg<T,TU>(System.Enum eventType, T agr1, TU arg2)
        {
            mEntity.Dispatch<T, TU>(eventType, agr1, arg2);
        }
        protected void FireMsg<T, TU, TV>(System.Enum eventType, T agr1, TU arg2, TV arg3)
        {
            mEntity.Dispatch<T, TU, TV>(eventType, agr1, arg2, arg3);
        }
        protected void FireMsg<T, TU, TV, TW>(System.Enum eventType, T agr1, TU arg2, TV arg3, TW arg4)
        {
            mEntity.Dispatch<T, TU, TV,TW>(eventType, agr1, arg2, arg3, arg4);
        }


        protected void FireMsg(System.Enum eventType)
        {
            mEntity.Dispatch(eventType);
        }
        protected virtual void OnEnable()
        {
            RegistListener();
        }
        protected virtual void OnDisable()
        {
            UnRegistListener();  
        }
        protected virtual void RegistListener()  { }
        protected virtual void UnRegistListener()  { }

        public void OnDestroy()
        {
            Dispose();
        }
        protected virtual void Dispose()  { }



    }
}
