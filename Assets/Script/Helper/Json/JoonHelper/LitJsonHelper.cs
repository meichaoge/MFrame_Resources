using UnityEngine;
using LitJson;
using System;

public static class LitJsonHelper
{

    public static string GetJsonData<T>(this JsonData data, string key)
    {
        JsonData _jsonValue = null;
        try
        {
            _jsonValue = data[key];
        }
        catch (Exception e)
        {
            Debug.LogError("Cant Get Json Data OF Key:: " + key);
        }

        if (_jsonValue == null)
        {
            if (default(T) == null)
                return "";
            return default(T).ToString();
        }
        return _jsonValue.ToString();
    }

}
