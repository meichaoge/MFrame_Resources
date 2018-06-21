using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    [System.Serializable]
    public class LoopScrollPrefabSource 
    {
        //public string prefabName;
        public GameObject m_ItemPrefab;
        public int poolSize = 5;

        private bool inited = false;
        public virtual GameObject GetObject()
        {
            if(!inited)
            {
                UIObjectPoolManager.Instance.InitPool(m_ItemPrefab, poolSize);
                inited = true;
            }
            return UIObjectPoolManager.Instance.GetObjectFromPool(m_ItemPrefab.name);
        }
    }
}
