using UnityEngine;
using System.Collections;
using System;
using Newtonsoft.Json;

public interface IJsonObject
{
    string Serialize();
}

public class BaseJsonObject<T> : IJsonObject where T : BaseJsonObject<T>
{
    public string Serialize()
    {
        return JsonParser.Serialize(this);
    }

    public virtual string SerializeWithIndented()
    {
        return JsonParser.SerializeWithIndented(this);
    }

    public static T Deserialize(string json)
    {
        if (string.IsNullOrEmpty(json))
            return default(T);
        try
        {
            T ret = JsonParser.Deserialize<T>(json);
            ret.RawJson = json;
            return ret;
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine("SDKJsonObject.Deserialize\r\n" + e.ToString() + "\r\njson:" + json);
            return default(T);
        }
    }

    [JsonIgnore]
    public string RawJson { get; private set; }
}
