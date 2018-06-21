using UnityEngine;


[DisallowMultipleComponent]
//[AddComponentMenu("")]
//对象池中的缓存对象
public class PoolObject : MonoBehaviour
{
    public string poolName;
    //defines whether the object is waiting in pool or is in use
    public bool isPooled;
}
