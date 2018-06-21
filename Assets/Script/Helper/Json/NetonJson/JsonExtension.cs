using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class JExtConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var ext = value as JsonExtension;
        if (ext == null)
            writer.WriteNull();
        else
        {
            var json = ext.Serialize();
            writer.WriteValue(json);
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.Value == null) return null;
        var v = reader.Value.ToString();
        if (!string.IsNullOrEmpty(v))
        {
            JToken token = JToken.Parse(v);
            JsonExtension ext = new JsonExtension(token);
            return ext;
        }
        return null;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(JsonExtension);
    }
}

/// <summary>
/// json 序列化对象
/// </summary>
public class JsonExtension
{
    public string Value { get { return Serialize(); } }

    private Dictionary<string, object> _dictionary;


    public JsonExtension()
    {
        _dictionary = new Dictionary<string, object>();
    }

    public JsonExtension(Dictionary<string, object> dic)
    {
        _dictionary = dic;
    }

    public JsonExtension(JToken token)
    {
        var value = token.ToString();
        _dictionary = JsonParser.Deserialize<Dictionary<string, object>>(value);
    }

    public static JsonExtension createFromString(string json)
    {
        Dictionary<string, object> dictionary = JsonParser.Deserialize<Dictionary<string, object>>(json);
        if (dictionary != null)
            return new JsonExtension(dictionary);
        return null;
    }

    public void AddItem(string key, object value)
    {
        _dictionary[key] = value;
    }

    public string Serialize()
    {
        if (_dictionary != null && _dictionary.Count > 0)
            return JsonParser.Serialize(_dictionary);
        return null;
    }
}