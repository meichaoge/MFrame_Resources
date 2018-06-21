using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 缓存的对象池
/// </summary>
public class Pool
{
    private Stack<PoolObject> availableObjStack = new Stack<PoolObject>();

    //the root obj for unused obj
    private GameObject rootObj;
    private float lastUsedTime = -1;
    private string poolName;
    private int objectsInUse = 0;

    public Pool(string poolName, GameObject poolObjectPrefab, GameObject rootPoolObj, int initialCount)
    {
        lastUsedTime = Time.time;

        if (poolObjectPrefab == null)
        {
#if UNITY_EDITOR
            Debug.LogError("[ObjPoolManager] null pool object prefab !");
#endif
            return;
        }
        this.poolName = poolName;
        this.rootObj = new GameObject(poolName + "Pool");
        this.rootObj.transform.SetParent(rootPoolObj.transform, false);

        // In case the origin one is Destroyed, we should keep at least one
        GameObject go = GameObject.Instantiate(poolObjectPrefab);
        PoolObject po = go.GetComponent<PoolObject>();
        if (po == null)
        {
            po = go.AddComponent<PoolObject>();
        }
        po.poolName = poolName;
        AddObjectToPool(po);

        //populate the pool
        populatePool(Mathf.Max(initialCount, 1));
    }

    //o(1)
    private void AddObjectToPool(PoolObject po)
    {
        //add to pool
        po.gameObject.SetActive(false);
        po.gameObject.name = poolName;
        availableObjStack.Push(po);
        po.isPooled = true;
        //add to a root obj
        po.gameObject.transform.SetParent(rootObj.transform, false);
    }
    /// <summary>
    /// 根据初始化的数量生成初始的缓存对象
    /// </summary>
    /// <param name="initialCount"></param>
    private void populatePool(int initialCount)
    {
        for (int index = 0; index < initialCount; index++)
        {
            PoolObject po = GameObject.Instantiate(availableObjStack.Peek());
            AddObjectToPool(po);
        }
    }

    //o(1)
    public GameObject NextAvailableObject(bool autoActive)
    {
        lastUsedTime = Time.time;

        PoolObject po = null;
        if (availableObjStack.Count > 1)
        {
            po = availableObjStack.Pop();
        }
        else
        {
            //#if UNITY_EDITOR
            //                Debug.Log(string.Format("Growing pool {0}: {1} populated", poolName, increaseSize));
            //#endif
            populatePool(1);
            po = availableObjStack.Pop();
        }

        if (po != null)
        {
            objectsInUse++;
            po.isPooled = false;
            if (autoActive)
            {
                po.gameObject.SetActive(true);
            }
            return po.gameObject;
        }
        return null;
    }

    //o(1)
    public void ReturnObjectToPool(PoolObject po)
    {
        if (poolName.Equals(po.poolName))
        {
            objectsInUse--;
            /* we could have used availableObjStack.Contains(po) to check if this object is in pool.
             * While that would have been more robust, it would have made this method O(n) 
             */
            if (po.isPooled)
            {
#if UNITY_EDITOR
                Debug.LogWarning(po.gameObject.name + " is already in pool. Why are you trying to return it again? Check usage.");
#endif
            }
            else
            {
                AddObjectToPool(po);
            }
        }
        else
        {
            Debug.LogError(string.Format("Trying to add object to incorrect pool {0} {1}", po.poolName, poolName));
        }
    }
}
