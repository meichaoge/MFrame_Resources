using System;
using Newtonsoft.Json;

public class JsonBoolConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value != null)
        {
            var b = (bool)value;
            writer.WriteValue(b ? 1 : 0);
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var v = (int)reader.Value;
        return v > 0;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(bool);
    }
}