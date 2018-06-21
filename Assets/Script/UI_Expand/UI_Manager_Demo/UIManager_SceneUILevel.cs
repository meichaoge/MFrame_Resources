using BeanVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework
{
    /// <summary>
    /// 管理场景中生成的UI
    /// </summary>
    public class UIManager_SceneUILevel : Singleton_Mono<UIManager_SceneUILevel>
    {
        private List<PanelInfor> m_AllSceneUI = new List<PanelInfor>(); //记录当前场景UI

    


        protected override void Awake()
        {
            base.Awake();
       //     GlobalEntity.GetInstance().AddListener<string>(SceneStateController.mEvent.loadingSwitchNotify, OnSceneLoadOver);
        }


        /// <summary>
        /// 当场景开始切换时  销毁所有的当前场景UI
        /// </summary>
        void OnSceneLoadOver(string scenName)
        {
            if (transform.childCount == 0) return;
         Debug.Log("OnSceneLoadOver Childs : " + transform.childCount);
            for (int dex = 0; dex < transform.childCount; ++dex)
            {
                System.Type type = transform.GetChild(dex).GetComponent<ViewBaseFU_EX>().GetType();
             //   UIManagerModel.GetInstance().DestroyUIByType(type);
                GameObject.Destroy(transform.GetChild(dex).gameObject);
            }
        }


        public Transform GetScenePanelByName(string panelName)
        {
            Transform trans = null;
            for (int dex = 0; dex < transform.childCount; ++dex)
            {
                trans = transform.GetChild(dex);
                if (trans.gameObject.name == panelName)
                {
                    return trans;
                }
            }
            return null;
        }

        public Transform[] GetScenePanelsByName(string panelName)
        {
            List<Transform> result = new List<Transform>();
            for (int dex = 0; dex < transform.childCount; ++dex)
            {
                Transform trans = transform.GetChild(dex);
                if (trans.gameObject.name == panelName)
                {
                    result.Add(trans);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        ///根据类型获得对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetScenePanelByType<T>() where T : ViewBaseFU_EX
        {
            Transform trans = null;
            T viewScript = null;
            for (int dex = 0; dex < transform.childCount; ++dex)
            {
                trans = transform.GetChild(dex);
                viewScript = trans.GetComponent<T>();
                if (viewScript == null)
                {
                    Debug.LogError("GetScenePanelByType Fail......");
                }
                else
                {
                    if (viewScript.GetType() == typeof(T))
                    {
                     Debug.Log("GetScenePanelByType Success..");
                        return viewScript;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据类型获得所有当前类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetScenePanelsByType<T>()
        {
            Transform trans = null;
            List<T> result = new List<T>();
            for (int dex = 0; dex < transform.childCount; ++dex)
            {
                trans = transform.GetChild(dex);
                T viewScript = trans.GetComponent<T>();
                if (viewScript == null)
                {
                    Debug.LogError("GetScenePanelByType Fail......" + typeof(T));
                    continue;
                }

                if (viewScript.GetType() == typeof(T))
                {
                 Debug.Log("GetScenePanelByType Success..");
                    result.Add(viewScript);
                }
            }//for

            if (result.Count == 0)
                return new T[0];
            return result.ToArray();
        }



    }
}
