using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerationObjUtility
{

    public static GameObject CreateObjectByName(string name,Transform parent,bool reset)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent);
        if (reset)
        {
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localPosition = Vector3.zero;
        }
        return go;
    }

    public static T GetAddComponent<T>(this GameObject obj)where T :Component
    {
        T result = obj.GetComponent<T>();
        if (result == null)
            result = obj.AddComponent<T>();

        return result;
    }

    public static T GetAddComponent<T>(this Transform trans) where T : Component
    {
        T result = trans.GetComponent<T>();
        if (result == null)
            result = trans.gameObject. AddComponent<T>();

        return result;
    }


}
