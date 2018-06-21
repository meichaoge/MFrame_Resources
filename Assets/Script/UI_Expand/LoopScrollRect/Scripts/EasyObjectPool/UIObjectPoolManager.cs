using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[DisallowMultipleComponent]
[AddComponentMenu("")]
public class UIObjectPoolManager : MonoBehaviour {

    //obj pool
    private Dictionary<string, Pool> poolDict = new Dictionary<string, Pool>();

    private static UIObjectPoolManager mInstance = null;

    public static UIObjectPoolManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject GO = new GameObject("UIObjectPoolManager", typeof(UIObjectPoolManager));
                // Kanglai: if we have `GO.hideFlags |= HideFlags.DontSave;`, we will encounter Destroy problem when exit playing
                // However we should keep using this in Play mode only!
                mInstance = GO.GetComponent<UIObjectPoolManager>();
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(mInstance.gameObject);
                }
                else
                {
                    Debug.LogWarning("[UIObjectPoolManager] You'd better ignore UIObjectPoolManager in Editor mode");
                }
            }
            return mInstance;
        }
    }
    public void InitPool(string poolName, int size)
    {
        if (poolDict.ContainsKey(poolName))
            return;
        GameObject pb = Resources.Load<GameObject>(poolName);
        if (pb == null)
        {
            Debug.LogError("[ResourceManager] Invalide prefab name for pooling :" + poolName);
            return;
        }
        poolDict[poolName] = new Pool(poolName, pb, gameObject, size);
    }


    public void InitPool(GameObject itemPrefab, int size)
    {
        if (itemPrefab == null)
        {
            Debug.LogError("[ResourceManager] InitPool prefab is Null:");
            return;
        }
        if (poolDict.ContainsKey(itemPrefab.name))
            return;
        
        poolDict[itemPrefab.name] = new Pool(itemPrefab.name, itemPrefab, gameObject, size);
    }

    /// <summary>
    /// Returns an available object from the pool 
    /// OR null in case the pool does not have any object available & can grow size is false.
    /// </summary>
    /// <param name="poolName"></param>
    /// <returns></returns>
    public GameObject GetObjectFromPool(string poolName, bool autoActive = true, int autoCreate = 0)
    {
        GameObject result = null;

        if (!poolDict.ContainsKey(poolName) && autoCreate > 0)
        {
            InitPool(poolName, autoCreate);
        }

        if (poolDict.ContainsKey(poolName))
        {
            Pool pool = poolDict[poolName];
            result = pool.NextAvailableObject(autoActive);
            //scenario when no available object is found in pool
#if UNITY_EDITOR
            if (result == null)
            {
                Debug.LogWarning("[ResourceManager]:No object available in " + poolName);
            }
#endif
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogError("[ResourceManager]:Invalid pool name specified: " + poolName);
        }
#endif
        return result;
    }

    /// <summary>
    /// Return obj to the pool
    /// </summary>
    /// <param name="go"></param>
    public void ReturnObjectToPool(GameObject go)
    {
        PoolObject po = go.GetComponent<PoolObject>();
        if (po == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Specified object is not a pooled instance: " + go.name);
#endif
        }
        else
        {
            Pool pool = null;
            if (poolDict.TryGetValue(po.poolName, out pool))
            {
                pool.ReturnObjectToPool(po);
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning("No pool available with name: " + po.poolName);
            }
#endif
        }
    }

 
}
